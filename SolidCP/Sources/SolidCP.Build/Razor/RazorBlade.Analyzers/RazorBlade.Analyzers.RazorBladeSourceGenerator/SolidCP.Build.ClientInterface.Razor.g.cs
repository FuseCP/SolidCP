﻿#pragma checksum "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8b376fe93c0d1dabec8438c0736dd2161b4056b3"
// <auto-generated/>
#pragma warning disable 1591
namespace SolidCP.Build
{
    #line hidden
#nullable restore
#line 2 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
using Microsoft.CodeAnalysis;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
using Microsoft.CodeAnalysis.CSharp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
using Microsoft.CodeAnalysis.CSharp.Syntax;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
using System.Collections.Generic;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
using System.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
using System;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#line default
#line hidden
#nullable disable
    #nullable restore
    internal partial class ClientInterface : RazorBlade.PlainTextTemplate
    #nullable disable
    {
        #pragma warning disable 1998
        protected async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("// wcf client contract\r\n");
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
            WriteLiteral("\r\n[System.CodeDom.Compiler.GeneratedCodeAttribute(\"SolidCP.Build\", \"1.0\")]\r\n[ServiceContract(ConfigurationName=\"I");
#nullable restore
#line (15,40)-(15,56) 6 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
Write(Class.Identifier);

#line default
#line hidden
#nullable disable
            WriteLiteral("\", Namespace=\"");
#nullable restore
#line (15,73)-(15,92) 6 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
Write(WebServiceNamespace);

#line default
#line hidden
#nullable disable
            WriteLiteral("\")]\r\npublic interface I");
#nullable restore
#line (16,21)-(16,37) 6 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
Write(Class.Identifier);

#line default
#line hidden
#nullable disable
            WriteLiteral(" {\r\n\r\n");
#nullable restore
#line 18 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
     foreach (var method in Methods)
	{
		

#line default
#line hidden
#nullable disable
#nullable restore
#line (20,4)-(20,38) 6 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
Write(method.Item1.NormalizeWhitespace());

#line default
#line hidden
#nullable disable
#nullable restore
#line (22,4)-(22,38) 6 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
Write(method.Item2.NormalizeWhitespace());

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
                                           
	}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n}\r\n\r\n");
        }
        #pragma warning restore 1998
#nullable restore
#line 28 "C:\GitHub\SolidCP\SolidCP\Sources\SolidCP.Build\ClientInterface.cshtml"
  
	public string WebServiceNamespace { get; set; }
	public ClassDeclarationSyntax Class { get; set; }
	public IEnumerable<MethodDeclarationSyntax> WebMethods { get; set; }
		public IEnumerable<Tuple<MethodDeclarationSyntax, MethodDeclarationSyntax>> Methods => WebMethods
			.Select(m => new
			{
				Method = m,
				IsVoid = (m.ReturnType is PredefinedTypeSyntax && ((PredefinedTypeSyntax)m.ReturnType).Keyword.IsKind(SyntaxKind.VoidKeyword))
			})
			.Select(m => new Tuple<MethodDeclarationSyntax, MethodDeclarationSyntax>(
				MethodDeclaration(m.Method.ReturnType, m.Method.Identifier)
					.WithAttributeLists(SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("OperationContract"),
						ParseAttributeArgumentList($"(Action = \"{WebServiceNamespace}I{Class.Identifier}/{m.Method.Identifier}\", ReplyAction = \"{WebServiceNamespace}I{Class.Identifier}/{m.Method.Identifier}Response\")"))))))
					.WithParameterList(m.Method.ParameterList)
					.WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
				MethodDeclaration(ParseTypeName((m.IsVoid) ? "System.Threading.Tasks.Task" : $"System.Threading.Tasks.Task<{m.Method.ReturnType}>"), $"{m.Method.Identifier}Async")
					.WithAttributeLists(SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("OperationContract"),
						ParseAttributeArgumentList($"(Action = \"{WebServiceNamespace}I{Class.Identifier}/{m.Method.Identifier}\", ReplyAction = \"{WebServiceNamespace}I{Class.Identifier}/{m.Method.Identifier}Response\")"))))))
					.WithParameterList(m.Method.ParameterList)
					.WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
			));


#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591