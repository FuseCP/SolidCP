using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SolidCP.Web.Clients;

namespace SolidCP.Tests
{
	public class Server
	{
		public const string AssemblyUrl = $"assembly://{Paths.App}.Server";
		public const string Password = "cRDtpNCeBiql5KOQsKVyrA0sAiA=";
		public static void Init()
		{
			AssemblyLoader.Init(@$"..\{Paths.App}.Server\bin;..\{Paths.App}.Server\bin\Lazy;..\{Paths.App}.Server\bin\netstandard", "none", false);

			try
			{
				var aserver = Assembly.Load("SolidCP.Server");
				if (aserver != null)
				{
					var validatorType = aserver.GetType("SolidCP.Server.PasswordValidator");
					var init = validatorType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
					init.Invoke(null, new object[0]);
				}
			}
			catch (Exception ex) { }

		}
	}
}
