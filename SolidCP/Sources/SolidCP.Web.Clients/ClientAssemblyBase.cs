using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Client
{
	public class ClientAssemblyBase
	{
		public string AssemblyName { get; set; }

		Assembly a;
		Assembly assembly => a ?? (a = Assembly.Load(AssemblyName));
		protected object Invoke(string typeName, string methodName, params object[] parameters)
		{
			var type = assembly.GetType(typeName);
			var method = type.GetMethod(methodName);
			return method.Invoke(Activator.CreateInstance(type), parameters);
		}

		protected Task<T> InvokeAsync<T>(string typeName, string methodName, params object[] parameters)
		{
			return Task.Factory.StartNew<T>(() => (T)Invoke(typeName, methodName, parameters));
		}
	}
}
