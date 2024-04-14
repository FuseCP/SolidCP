using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;
using System.Text.RegularExpressions;

namespace TypeScriptClientGenerator
{
    public class SolidCPOperationNameGenerator : IOperationNameGenerator
    {
        public bool SupportsMultipleClients => true;

        public HashSet<string> Clients = new HashSet<string>();
        public string GetClientName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation)
        {
            var clientName = Regex.Match(path, @"(?<=^/?api/)[^/]*").Value;
            Clients.Add(clientName);
            return clientName;
        }

        public string GetOperationName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation)
        {
            var operationName = Regex.Match(path, @"(?<=^/?api/[^/]*?/)[^/]*").Value;
            return operationName;
        }
    }
}
