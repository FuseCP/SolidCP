using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Octokit;

namespace SolidCP.UniversalInstaller
{
	public class GitHubReleases: Octokit.GitHubClient
	{
		public class ComponentWithRelease
		{
			public ComponentInfo Component { get; set; }
			public Release Release { get; set; }
		
			public ComponentWithRelease(ComponentInfo component, Release release)
			{
				Component = component;
				Release = release;
			}
		}

		const string ApplicationId = "SolidCPInstaller";
		public string Url { get; set; } 
		public string Owner => new Uri(Url).AbsolutePath.Split('/').Skip(1).FirstOrDefault();
		public string Repo => new Uri(Url).AbsolutePath.Split('/').Skip(2).FirstOrDefault();
		public GitHubReleases(string url): base(new ProductHeaderValue(ApplicationId)) { Url = url; }

		private async Task<IEnumerable<ComponentWithRelease>> ReleaseComponentsAsync(Release release)
		{
			try
			{
				var releaseJsonAsset = release.Assets.FirstOrDefault(asset => asset.Name.Equals("release.json", StringComparison.OrdinalIgnoreCase));

				if (releaseJsonAsset != null)
				{
					using (var httpClient = new HttpClient())
					{
						var json = await httpClient.GetStringAsync(releaseJsonAsset.Url);
						return JsonConvert.DeserializeObject<ElementJson[]>(json)
							.Select(e =>
							{
								e.version = release.Name;
								var component = new ComponentInfo(new ElementInfo(e));
								component.GitHub = true;
								return new ComponentWithRelease(component, release);
							});
					}
				}
			}
			catch (Exception ex) { }
			return null;
		}
		public async Task<List<ComponentInfo>> GetAvailableComponentsAsync()
		{
			try
			{
				var latest = await Repository.Release.GetLatest(Owner, Repo);
				if (latest != null)
				{
					return (await ReleaseComponentsAsync(latest))
						.Select(c =>
						{
							var component = c.Component;
							if (component != null) component.GitHub = true;
							return component;
						})
						.ToList();
				}
			} catch (Exception ex) { }
			return null;
		}

		public async Task<ComponentUpdateInfo> GetComponentUpdateAsync(string componentCode, string release)
		{
			Version version = default;
			var vm = Regex.Match(release, "[0-9][0-9.]+");
			if (vm.Success) version = Version.Parse(vm.Value);
			var releases = (await Repository.Release.GetAll(Owner, Repo)).ToList();
			var ghrelease = releases
				.FirstOrDefault(release =>
				{
					vm = Regex.Match(release.Name, "[0-9][0-9.]+");
					return vm.Success && Version.Parse(vm.Value) == version;
				});
			var index = releases.IndexOf(ghrelease);
			if (index == 0) return null;
			ghrelease = releases[index - 1];
			var component = (await ReleaseComponentsAsync(ghrelease))
				.Where(component => component.Component.ComponentCode == componentCode)
				.Select(component => new ComponentUpdateInfo(component.Component))
				.FirstOrDefault();
			if (component != null) component.GitHub = true;
			return component;
		}

		public async Task<ComponentUpdateInfo> GetLatestComponentUpdateAsync(string componentCode)
		{
			var releases = (await Repository.Release.GetAll(Owner, Repo))
				.Select(r =>
				{
					var vm = Regex.Match(r.Name, "[0-9][0-9.]+");
					if (vm.Success)
					{
						return new
						{
							Release = r,
							Version = Version.Parse(vm.Value)
						};
					}
					else return null;
				})
				.Where(r => r != null);

			var release = releases
				.OrderByDescending(r => r.Version)
				.FirstOrDefault()
				?.Release;

			var component = (await ReleaseComponentsAsync(release))
				.Where(component => component.Component.ComponentCode == componentCode)
				.Select(component => new ComponentUpdateInfo(component.Component))
				.FirstOrDefault();
			if (component != null) component.GitHub = true;
			return component;
		}

		public async Task<ReleaseFileInfo> GetReleaseFileInfoAsync(string componentCode, string release)
		{
			Version version = default;
			var vm = Regex.Match(release, "[0-9][0-9.]+");
			if (vm.Success) version = Version.Parse(vm.Value);
			var releases = (await Repository.Release.GetAll(Owner, Repo)).ToList();
			var ghrelease = releases
				.FirstOrDefault(release =>
				{
					vm = Regex.Match(release.Name, "[0-9][0-9.]+");
					return vm.Success && Version.Parse(vm.Value) == version;
				});
			var component = (await ReleaseComponentsAsync(ghrelease))
				.Where(component => component.Component.ComponentCode == componentCode)
				.FirstOrDefault().Component;
			if (component != null) component.GitHub = true;
			return component;
		}

		public async Task GetFileAsync(RemoteFile file, string destinationFile, Action<long, long> progress = null)
		{
			var componentTasks = (await Repository.Release.GetAll(Owner, Repo))
				.Select(release => ReleaseComponentsAsync(release));
			var component = (await Task.WhenAll(componentTasks))
				.SelectMany(component => component)
				.FirstOrDefault(component => component.Component.ReleaseFileId == file.Release.ReleaseFileId);
			var filename = file.FullFile ? component.Component.FullFilePath : component.Component.UpgradeFilePath;
			filename = filename.Split('/').LastOrDefault();
			var url = component.Release.Assets.FirstOrDefault(asset => asset.Name.EndsWith(filename))?.Url;

			const int BufferSize = 8 * 1024;

			using (var httpClient = new HttpClient())
			using (var dest = new FileStream(destinationFile, System.IO.FileMode.Create, FileAccess.Write, FileShare.None, BufferSize))
			using (var response = await httpClient.GetAsync(url))
			{
				response.EnsureSuccessStatusCode();
				var sizestr = response.Content.Headers.FirstOrDefault(h => h.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase)).Value?.FirstOrDefault() ?? "0";
				long size = 0, read = 0;
				long.TryParse(sizestr, out size);
				if (size == 0) size = 100 * 1024 * 1024;
				var buff = new byte[BufferSize];
				using (var src = await response.Content.ReadAsStreamAsync())
				{
					var n = await src.ReadAsync(buff, 0, BufferSize);
					while (n > 0)
					{
						await dest.WriteAsync(buff, 0, n);
						read += n;
						progress(Math.Min(read, size), size);
						
						Installer.Current.Cancel.Token.ThrowIfCancellationRequested();

						n = await src.ReadAsync(buff, 0, BufferSize);
					}
				}
			}
		}
	}
}
