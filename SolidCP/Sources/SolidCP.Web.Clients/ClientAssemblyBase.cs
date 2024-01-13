using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolidCP.Providers;

namespace SolidCP.Web.Client
{
	public class ClientAssemblyBase
	{

		public string AssemblyName { get; set; }
		public ClientBase Client { get; set; }

		Assembly Assembly => Assembly.Load(AssemblyName);
		protected T Invoke<T>(string typeName, string methodName, params object[] parameters)
		{
			var assembly = Assembly;
			var type = assembly.GetType(typeName);
			var method = type.GetMethod(methodName);
			// authentication
			if (Client.IsAuthenticated)
			{
				var serviceAssembly = Assembly.Load("SolidCP.Web.Services");
				var validator = serviceAssembly.GetType("UserNamePasswordValidator");
				var validateMethod = validator.GetMethod("Validate", BindingFlags.Public | BindingFlags.Static);
				if (validateMethod == null) validateMethod = validator.GetMethod("ValidateAsync", BindingFlags.Public | BindingFlags.Static);
				if (validateMethod != null) validateMethod.Invoke(null, new object[] { Client.Credentials.UserName, Client.Credentials.Password });
			}
			object service = Activator.CreateInstance(type);
			// set soap headers
			if (Client.HasSoapHeaders)
			{
				var attr = method.GetCustomAttribute<SoapHeaderAttribute>();
				var prop = type.GetProperty(attr.Field);
				if (prop == null)
				{
					var field = type.GetField(attr.Field);
					field.SetValue(service, Client.SoapHeader);
				}
				else
				{
					prop.SetValue(service, Client.SoapHeader);
				}
			}
			return (T)method.Invoke(service, parameters);
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
			return Task.Factory.StartNew<T>(() => Invoke<T>(typeName, methodName, parameters),
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext());
		}
		protected Task<T> InvokeAsync<T, TItem>(string typeName, string methodName, params object[] parameters)
		{
			return Task.Factory.StartNew<T>(() => Invoke<T, TItem>(typeName, methodName, parameters),
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext());
		}
		protected Task InvokeAsync(string typeName, string methodName, params object[] parameters)
		{
			return Task.Factory.StartNew(() => Invoke<object>(typeName, methodName, parameters),
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext());
		}

	}
}
