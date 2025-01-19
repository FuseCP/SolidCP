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
		var info = await GitHub.GetComponentUpdateAsync(componentCode, release);
		if (info == null) return await WebService.GetComponentUpdateAsync(componentCode, release);
		return info;
	}
	public List<ComponentInfo> GetAvailableComponents() => GetAvailableComponentsAsync().Result;
	public async Task<List<ComponentInfo>> GetAvailableComponentsAsync()
	{
		var ghcomponents = await GitHub.GetAvailableComponentsAsync();
		var wscomponents = await WebService.GetAvailableComponentsAsync();
		if (ghcomponents != null && ghcomponents.Count > 0 &&
			(wscomponents == null || wscomponents.Count == 0 ||
			ghcomponents[0].Version > wscomponents[0].Version)) return ghcomponents;
		else return wscomponents;
	}
	public ComponentUpdateInfo GetLatestComponentUpdate(string componentCode)
		=> GetLatestComponentUpdateAsync(componentCode).Result;
	public async Task<ComponentUpdateInfo> GetLatestComponentUpdateAsync(string componentCode)
	{
		var ghinfo = await GitHub.GetLatestComponentUpdateAsync(componentCode);
		var wsinfo = await WebService.GetLatestComponentUpdateAsync(componentCode);
		if (ghinfo != null &&
			(wsinfo == null || ghinfo.Version > wsinfo.Version)) return ghinfo;
		else return wsinfo;
	}
	public ReleaseFileInfo GetReleaseFileInfo(string componentCode, string version)
		=> GetReleaseFileInfoAsync(componentCode, version).Result;
	public async Task<ReleaseFileInfo> GetReleaseFileInfoAsync(string componentCode, string version)
	{
		var ghinfo = await GitHub.GetReleaseFileInfoAsync(componentCode, version);
		if (ghinfo != null) return ghinfo;
		else return await WebService.GetReleaseFileInfoAsync(componentCode, version);
	}
	public void GetFile(RemoteFile file, string destinationFile, Action<long, long> progress = null)
		=> Task.Run(() => GetFileAsync(file, destinationFile, progress)).Wait();
	public async Task GetFileAsync(RemoteFile file, string destinationFile, Action<long, long> progress = null)
	{
		if (file.Release.GitHub) await GitHub.GetFileAsync(file, destinationFile, progress);
		else
		{
			var service = WebService;

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
