using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using Microsoft.Management.Infrastructure.Serialization;
using System;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.Virtualization
{
    public class MiManager : IDisposable //MI/CIM is WMIv2 
    {
        private CimSession _session;
        private readonly object _disposeThreadSafetyLock = new object();
        private readonly string _namespacePath;

        public bool IsDisposed { get; private set; } = false;

        public MiManager(string targetComputer, CimSessionMode cimSessionMode, string namespacePath)
        {
            CimSessionOptions options;

            switch (cimSessionMode)
            {
                case CimSessionMode.DCom:
                    options = new DComSessionOptions(); // old (WMI)
                    ((DComSessionOptions)options).Impersonation = ImpersonationType.Impersonate;
                    break;
                case CimSessionMode.WSMan:
                    options = new WSManSessionOptions(); // new MI (CIM)
                    break;
                default:
                    throw new ArgumentException("Invalid CIM session option.", nameof(cimSessionMode));
            }
            //CimCredential Credentials = new CimCredential(PasswordAuthenticationMechanism.Default, "domain", "username", "securepassword");
            //options.AddDestinationCredentials(Credentials);
            string target = string.IsNullOrWhiteSpace(targetComputer) ? null : targetComputer;
            _session = CimSession.Create(target, options);
            _namespacePath = namespacePath;
        }

        public string SerializeToCimDtd20(CimInstance cimInstance)
        {
            CimSerializer serializer = CimSerializer.Create();
            byte[] serializedBytes = serializer.Serialize(cimInstance, InstanceSerializationOptions.None);
            return Encoding.Unicode.GetString(serializedBytes);
        }

        public CimInstance GetCimInstance(string className)
        {
            return EnumerateCimInstances(className).FirstOrDefault();
        }

        /// <summary>
        /// IMPORTANT! Use GetCimInstance without filter if possible (especially if it gets Accociations), as it is faster; 
        /// otherwise, you will likely have to use GetCimInstanceWithSelect.
        /// </summary>
        public CimInstance GetCimInstance(string className, string filter, params object[] args)
        {
            return GetCimInstanceWithSelect(className, null, filter, args);
        }

        public CimInstance GetAssociatedCimInstance(CimInstance sourceInstance, string resultClassName)
        {
            return EnumerateAssociatedInstances(sourceInstance, null, resultClassName).FirstOrDefault();
        }

        public CimInstance GetAssociatedCimInstance(CimInstance sourceInstance, string resultClassName, string associationClassName)
        {
            return EnumerateAssociatedInstances(sourceInstance, associationClassName, resultClassName).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a single instance of the specified CIM class using a WQL SELECT query.
        /// IMPORTANT! To improve performance, avoid using arg select "*" or "null". Instead, specify only the required properties.
        /// </summary>
        public CimInstance GetCimInstanceWithSelect(string className, string select, string filter, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(className)) {
                throw new ArgumentException("Class name cannot be empty.", nameof(className));
            }           

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT")
                        .Append(" ")
                        .Append(string.IsNullOrWhiteSpace(select) ? "*" : select.Trim());

            queryBuilder.Append(" ")
                        .Append("FROM")
                        .Append(" ")
                        .Append(className.Trim());

            if (!string.IsNullOrWhiteSpace(filter)) {
                queryBuilder.Append(" ")
                            .Append("WHERE")
                            .Append(" ")
                            .Append(filter.Trim());
            }

            string query = queryBuilder.ToString();
            if (args != null && args.Length > 0) {
                query = string.Format(query, args);
            }
            return QueryCimInstances(query).FirstOrDefault();
        }

        public CimClass GetCimClass(string className)
        {
            return _session.GetClass(_namespacePath, className, null);
        }

        /// <summary>
        /// Get a CIM instance based on the provided light/proxy/empty instance object.
        /// </summary>
        public CimInstance GetInstance(CimInstance notInitInstance)
        {
            return _session.GetInstance(_namespacePath, notInitInstance);
        }

        /// <summary>
        /// Queries instances of the specified CIM class.
        /// </summary>
        public CimInstance[] EnumerateCimInstances(string className)
        {
            AssertNotDisposed();
            if (string.IsNullOrWhiteSpace(className)){
                throw new ArgumentException("Class name cannot be empty.", nameof(className));
            }

            return _session.EnumerateInstances(_namespacePath, className).ToArray();
        }

        /// <summary>
        /// IMPORTANT! Use EnumerateCimInstances if possible, as it is faster; otherwise, you will likely have to use SELECT.
        /// Queries instances of the specified CIM class.
        /// WQL - https://learn.microsoft.com/en-us/windows/win32/wmisdk/wql-sql-for-wmi.
        /// </summary>
        public CimInstance[] QueryCimInstances(string query)
        {
            AssertNotDisposed();
            if (string.IsNullOrWhiteSpace(query)){
                throw new ArgumentException("Query cannot be empty.", nameof(query));
            }
            return _session.QueryInstances(_namespacePath, "WQL", query).ToArray();
        }

        /// <summary>
        /// Retrieves an array of CIM instances that are associated with the specified source instance using the given association parameters.
        /// </summary>
        /// <param name="sourceInstance">The source instance from which to retrieve associated instances.</param>
        /// <param name="associationClassName">
        /// =============================
        /// IMPORTANT! If the association class is not specified, performance WILL BE significantly reduced.
        /// =============================
        /// The name of the association class that defines the relationship between the source and target objects. 
        /// Use null if no specific association class filter is needed.
        /// </param>
        /// <param name="resultClassName">
        /// The name of the target (result) class. Use null if no specific result class filter is needed.
        /// </param>
        /// <param name="sourceRole">
        /// Optional. The role of the source instance in the association. Pass null if not required.
        /// </param>
        /// <param name="resultRole">
        /// Optional. The role of the target instance in the association. Pass null if not required.
        /// </param>
        /// <returns>
        /// An array of CIM instances that are associated with the given source instance according to the specified parameters.
        /// </returns>
        public CimInstance[] EnumerateAssociatedInstances(
            CimInstance sourceInstance, 
            string associationClassName, 
            string resultClassName, 
            string sourceRole = null, 
            string resultRole = null
            )
        {
            AssertNotDisposed();

            return _session.EnumerateAssociatedInstances(
                _namespacePath, 
                sourceInstance, 
                associationClassName,
                resultClassName,
                sourceRole,
                resultRole
                ).ToArray();
        }

        /// <summary>
        /// Invokes the specified method on a given CIM instance.
        /// </summary>
        /// <param name="instance">The CIM instance.</param>
        /// <param name="methodName">The method name to invoke.</param>
        /// <param name="parameters">Optional method parameters.</param>
        /// <returns>The result of the method invocation.</returns>
        public CimMethodResult InvokeMethod(CimInstance instance, string methodName, CimMethodParametersCollection parameters = null)
        {
            AssertNotDisposed();

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentException("Method name cannot be empty.", nameof(methodName));

            return _session.InvokeMethod(instance, methodName, parameters);
        }

        internal void AssertNotDisposed()
        {
            lock (_disposeThreadSafetyLock)
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException(ToString());
                }
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                _session?.Dispose();
                _session = null;
                IsDisposed = true;
            }
        }        
    }
}
