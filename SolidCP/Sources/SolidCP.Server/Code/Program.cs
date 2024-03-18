#if !NETFRAMEWORK

namespace SolidCP.Server
{

	public static class Program
	{

		public static void Main(string[] args)
		{
			//if (!Debugger.IsAttached) Debugger.Launch();
			PasswordValidator.Init();
			SolidCP.Web.Services.StartupCore.Init(args);
		}
	}
}

#endif