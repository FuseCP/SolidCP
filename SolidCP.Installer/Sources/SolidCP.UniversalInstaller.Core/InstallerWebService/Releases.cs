using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;

namespace SolidCP.UniversalInstaller;

public class Releases
{
	public GitHubReleases GitHub => Installer.Current.GitHub;
	public IInstallerWebService WebService = Installer.Current.InstallerWebService;

	public const int ChunkSize = 262144;

	public ComponentUpdateInfo GetComponentUpdate(string componentCode, string release)
		=> GetComponentUpdateAsync(componentCode, release).Result;
	public async Task<ComponentUpdateInfo> GetComponentUpdateAsync(string componentCode, string release)
	{
		var infos = new[] {
			GitHub.GetComponentUpdateAsync(componentCode, release),
			WebService.GetComponentUpdateAsync(componentCode, release)
		};
		var info = await infos[0];
		if (info == null) return await infos[1];
		return info;
	}
	public List<ComponentInfo> GetAvailableComponents() => GetAvailableComponentsAsync().Result;
	public async Task<List<ComponentInfo>> GetAvailableComponentsAsync()
	{
		var components = new[]
		{
			GitHub.GetAvailableComponentsAsync(),
			WebService.GetAvailableComponentsAsync()
		};
		await Task.WhenAll(components);
		var ghcomponents = await components[0];
		var wscomponents = await components[1];
		if (ghcomponents != null && ghcomponents.Count > 0 &&
			(wscomponents == null || wscomponents.Count == 0 ||
			ghcomponents[0].Version > wscomponents[0].Version)) return ghcomponents;
		else return wscomponents;
	}
	public ComponentUpdateInfo GetLatestComponentUpdate(string componentCode)
		=> GetLatestComponentUpdateAsync(componentCode).Result;
	public async Task<ComponentUpdateInfo> GetLatestComponentUpdateAsync(string componentCode)
	{
		var infos = new[]
		{
			GitHub.GetLatestComponentUpdateAsync(componentCode),
			WebService.GetLatestComponentUpdateAsync(componentCode)
		};
		var ghinfo = await infos[0];
		var wsinfo = await infos[1];
		if (ghinfo != null &&
			(wsinfo == null || ghinfo.Version > wsinfo.Version)) return ghinfo;
		else return wsinfo;
	}
	public ComponentUpdateInfo GetReleaseFileInfo(string componentCode, string version)
		=> GetReleaseFileInfoAsync(componentCode, version).Result;
	public async Task<ComponentUpdateInfo> GetReleaseFileInfoAsync(string componentCode, string version)
	{
		var infos = new[]
		{
			GitHub.GetReleaseFileInfoAsync(componentCode, version),
			WebService.GetReleaseFileInfoAsync(componentCode, version)
		};
		var info = await infos[0] ?? await infos[1];
		return info != null ? new ComponentUpdateInfo(info, version) : null;
	}
	public void GetFile(RemoteFile file, string destinationFile, Action<long, long> progress = null)
		=> Task.Run(() => GetFileAsync(file, destinationFile, progress)).Wait();
	public async Task GetFileAsync(RemoteFile file, string destinationFile, Action<long, long> progress = null)
	{
		if (file.Release.GitHub) await GitHub.GetFileAsync(file, destinationFile, progress);
		else
		{
			var service = WebService;
			
			var destinationPath = Path.GetDirectoryName(destinationFile);
			if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath); 

			long downloaded = 0;
			long fileSize = service.GetFileSize(file.File);

			if (fileSize == 0)
			{
				throw new FileNotFoundException("Service returned empty file.", file.File);
			}

			byte[] content;

			while (downloaded < fileSize)
			{
				// Throw OperationCancelledException if there is an incoming cancel request
				Installer.Current.Cancel.Token.ThrowIfCancellationRequested();

				content = service.GetFileChunk(file.File, (int)downloaded, ChunkSize);
				if (content == null)
				{
					throw new FileNotFoundException("Service returned NULL file content.", file.File);
				}
				FileUtils.AppendFileContent(destinationFile, content);

				downloaded += content.Length;

				progress?.Invoke(downloaded, fileSize);

				if (content.Length < ChunkSize)
					break;
			}
		}
	}
}
