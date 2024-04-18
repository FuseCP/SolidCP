#if !NETFRAMEWORK

namespace SolidCP.EnterpriseServer
{
	public static class Program
	{

		public static void Main(string[] args)
		{
            //if (!Debugger.IsAttached) Debugger.Launch();
            UsernamePasswordValidator.Init();
            Web.Clients.CertificateValidator.Init();
				//Web.Clients.AssemblyLoader.Init(null, null, false);
				Web.Services.StartupCore.Init(args);
        }
    }
}

#endif