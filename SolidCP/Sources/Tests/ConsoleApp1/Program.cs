using SolidCP.EnterpriseServer.Data;

namespace ConsoleApp1
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");

#if !NETFRAMEWORK
			SolidCP.Web.Services.Configuration.ConnectionString = "DbType=MsSql;Server=127.0.0.1;Database=SolidCP;uid=sa;pwd=Password12;TrustServerCertificate=true";
#endif
			DbContext.Init();

			using (var db = new DbContext())
			{
				foreach (var p in db.Providers)
				{
					Console.WriteLine(p.DisplayName);
				}
			}
		}
	}
}
