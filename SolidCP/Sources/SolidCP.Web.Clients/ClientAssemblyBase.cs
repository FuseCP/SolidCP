using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Client
{
	public class ClientAssemblyBase
	{

		public string AssemblyName { get; set; }
		public ClientBase Client { get; set; }

		Assembly Assembly => Assembly.Load(AssemblyName);
		protected T Invoke<T>(string typeName, string methodName, params object[] parameters)
		{
			var type = Assembly.GetType(typeName);
			var method = type.GetMethod(methodName);
			//TODO authentication & soap headers
			throw new NotImplementedException("Assembly protocol not implemented.");
			return (T)method.Invoke(Activator.CreateInstance(type), parameters);
		}
        protected void Invoke(string typeName, string methodName, params object[] parameters)
		{
			Invoke<object>(typeName, methodName, null, parameters);
		}


        protected T Invoke<T, TItem>(string typeName, string methodName, params object[] parameters)
		{
			var result = Invoke<object>(typeName, methodName, parameters);
			if (result is IEnumerable<TItem> list) return (T)((object)list.ToArray());
			throw new NotSupportedException();
		}

		protected Task<T> InvokeAsync<T>(string typeName, string methodName, params object[] parameters)
		{
			return Task.Factory.StartNew<T>(() => Invoke<T>(typeName, methodName, parameters));
		}
		protected Task<T> InvokeAsync<T, TItem>(string typeName, string methodName, params object[] parameters)
		{
			return Task.Factory.StartNew<T>(() => Invoke<T, TItem>(typeName, methodName, parameters));
		}
        protected Task InvokeAsync(string typeName, string methodName, params object[] parameters)
        {
            return Task.Factory.StartNew(() => Invoke<object>(typeName, methodName, parameters));
        }

    }
}
