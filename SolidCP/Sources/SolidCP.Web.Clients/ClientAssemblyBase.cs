using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolidCP.Providers;

namespace SolidCP.Web.Clients
{
	public class ClientAssemblyBase
	{
		public string AssemblyName { get; set; }
		public ClientBase Client { get; set; }

		Assembly Assembly => Assembly.Load(AssemblyName);
		static Assembly serviceAssembly = null;
		static object Lock = new object();

		void GetMethod(string typeName, string methodName, out Assembly assembly, out Type type, out MethodInfo method)
		{
			assembly = Assembly;
			type = assembly.GetType(typeName);
			method = type.GetMethod(methodName);
			if (method == null) throw new ArgumentException($"Method {methodName} not found");
		}

		protected T Invoke<T>(string typeName, string methodName, params object[] parameters)
		{
			Assembly assembly;
			Type type;
			MethodInfo method;
			GetMethod(typeName, methodName, out assembly, out type, out method);
			return Invoke<T>(assembly, type, method, methodName, parameters);
		}

		protected T Invoke<T>(Assembly assembly, Type type, MethodInfo method, string methodName, params object[] parameters)
		{
			// authentication
			if (Client.IsAuthenticated)
			{
				if (Client.Credentials == null) throw new UnauthorizedAccessException("You must specify user credentials to access this service.");

				lock (Lock)
				{
					if (serviceAssembly == null) serviceAssembly = Assembly.Load("SolidCP.Web.Services");
				}
				var validatorType = serviceAssembly.GetType("SolidCP.Web.Services.UserNamePasswordValidator");
				var validatorPolicyField = validatorType.GetField("Policy");
				var validator = Activator.CreateInstance(validatorType);
				var policyType = serviceAssembly.GetType("SolidCP.Web.Services.PolicyAttribute");
				var policy = Activator.CreateInstance(policyType, Client.Policy.Policy);
				validatorPolicyField.SetValue(validator, policy);

				var validateMethod = validatorType.GetMethod("Validate", BindingFlags.Public | BindingFlags.Instance);
				if (validateMethod != null) validateMethod.Invoke(validator, new object[] { Client.Credentials.UserName, Client.Credentials.Password });
				else
				{
					validateMethod = validatorType.GetMethod("ValidateAsync", BindingFlags.Public | BindingFlags.Instance);
					if (validateMethod != null)
					{
						(validateMethod.Invoke(validator, new object[] { Client.Credentials.UserName, Client.Credentials.Password }) as Task).Wait();
					}
					else throw new NotSupportedException("Validator method not found.");
				}
			}
			object service = Activator.CreateInstance(type);

			try
			{
				// set soap headers
				if (Client.HasSoapHeaders)
				{
					var serviceInterface = Client.ServiceInterface;
					var serviceMethod = serviceInterface.GetMethod(methodName);
					var attr = serviceMethod.GetCustomAttribute<SoapHeaderAttribute>();
					if (attr != null)
					{
						var soapHeader = Client.SoapHeader;
						if (soapHeader == null) throw new Exception($"The call to {type.Name}.{methodName} requires a SoapHeader.");

						soapHeader = DataContractCopier.Clone(soapHeader);

						var prop = type.GetProperty(attr.Field);
						if (prop == null)
						{
							var field = type.GetField(attr.Field);
							field.SetValue(service, soapHeader);
						}
						else
						{
							prop.SetValue(service, soapHeader);
						}
					}
				}

				parameters = (object[])DataContractCopier.Clone(parameters);

				var result = method.Invoke(service, parameters);

				return (T)DataContractCopier.Clone(result);
			}
			finally
			{
				if (service is IDisposable disposableService) disposableService.Dispose();
			}
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
