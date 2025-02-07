using System.Collections;
using Microsoft.Win32;

namespace SolidCP.UniversalInstaller;

[RunInstaller(true)]
public partial class SetPathCustomAction : System.Configuration.Install.Installer
{
	string environmentKey = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
	string pathUrl => System.IO.Path.GetDirectoryName(this.Context.Parameters["assemblypath"]);//"C:\\Program Files (86)\\TargetFolder";
	
	public SetPathCustomAction()
	{
		InitializeComponent();
	}

	[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
	public override void Install(IDictionary stateSaver)
	{
		base.Install(stateSaver);
	}

	[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
	public override void Commit(IDictionary savedState)
	{
		string environmentVar = Environment.GetEnvironmentVariable("PATH");


		//get non-expanded PATH environment variable            
		string oldPath = (string)Registry.LocalMachine.CreateSubKey(environmentKey).GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames);


		var index = oldPath.IndexOf(pathUrl);
		if (index < 0)
		{
			//set the path as an an expandable string
			Registry.LocalMachine.CreateSubKey(environmentKey).SetValue("Path", oldPath + ";" + pathUrl, RegistryValueKind.ExpandString);
		}

		base.Commit(savedState);
	}

	[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
	public override void Rollback(IDictionary savedState)
	{
		base.Rollback(savedState);


	}

	[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
	public override void Uninstall(IDictionary savedState)
	{
		//get non-expanded PATH environment variable            
		string oldPath = (string)Registry.LocalMachine.CreateSubKey(environmentKey).GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames);

		string removeString = pathUrl + ";";
		var index = oldPath.IndexOf(removeString);
		if (index < 0)
		{
			removeString = pathUrl;
			index = oldPath.IndexOf(removeString);
		}

		if (index > -1)
		{
			oldPath = oldPath.Remove(index, pathUrl.Length);
			//set the path as an an expandable string
			Registry.LocalMachine.CreateSubKey(environmentKey).SetValue("Path", oldPath, RegistryValueKind.ExpandString);
		}

		base.Uninstall(savedState);
	}
}