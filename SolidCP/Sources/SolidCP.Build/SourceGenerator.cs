using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SolidCP.Build
{
	public static class INamedTypeSymbolExtensions
	{
		public static IEnumerable<string> GetNamespaces(INamedTypeSymbol symbol)
		{
			var current = symbol.ContainingNamespace;
			while (current != null)
			{
				if (current.IsGlobalNamespace)
					break;
				yield return current.Name;
				current = current.ContainingNamespace;
			}
		}

		public static string GetFullNamespace(this INamedTypeSymbol symbol)
		{
			return string.Join(".", GetNamespaces(symbol).Reverse());
		}

		public static string GetFullTypeName(this INamedTypeSymbol symbol)
		{
			return string.Join(".", GetNamespaces(symbol).Reverse().Concat(new[] { symbol.Name }));
		}

		public static MethodDeclarationSyntax[] WebMethods(this ClassDeclarationSyntax classDeclaration, SemanticModel model)
		{
			return classDeclaration.Members.OfType<MethodDeclarationSyntax>()
						.Where(m => m.AttributeLists
							.Any(l => l.Attributes
							.Any(a => ((INamedTypeSymbol)model.GetTypeInfo(a).Type).GetFullTypeName() == "System.Web.Services.WebMethodAttribute")))
						.ToArray();

		}
	}

	[Generator(LanguageNames.CSharp)]
	public class GenerateWebServices : ISourceGenerator
	{

		public static readonly string NewLine = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\r\n" : "\n";

		public void Execute(GeneratorExecutionContext context)
		{

#if DEBUG
			if (!Debugger.IsAttached)
			{
				//Debugger.Launch();
			}
#endif

			// get WebServices
			var classesWithAttributes = context.Compilation.SyntaxTrees
				.Select(tree => new
				{
					Tree = tree,
					Classes = tree.GetRoot()
						.DescendantNodes()
						.OfType<ClassDeclarationSyntax>()
						.Where(classDeclaration => classDeclaration
							.DescendantNodes()
							.OfType<AttributeSyntax>()
							.Any())
				})
				.ToList();
			var classesWithModel = classesWithAttributes
				.Select(c => new
				{
					Model = context.Compilation.GetSemanticModel(c.Tree),
					Classes = c.Classes
				})
				.SelectMany(c => c.Classes
					.Select(d => new
					{
						Model = c.Model,
						Class = d
					}))
				.ToList();

			var webServiceClasses = classesWithModel
				.Where(c => c.Class.AttributeLists
				.Any(al => al.Attributes
					.Any(a =>
						((INamedTypeSymbol)c.Model.GetTypeInfo(a).Type).GetFullTypeName() == "System.Web.Services.WebServiceAttribute")))
				.ToList();
					
			foreach (var ws in webServiceClasses)
			{

				var tree = ws.Class.SyntaxTree;
				var oldTree = tree.GetRoot() as CompilationUnitSyntax;
				CompilationUnitSyntax serverTree;
				CompilationUnitSyntax clientTree;
				var methods = ws.Class.WebMethods(ws.Model);


				var attr = ws.Class.AttributeLists
					.SelectMany(l => l.Attributes)
					.FirstOrDefault(a =>
						((INamedTypeSymbol)ws.Model.GetTypeInfo(a).Type).GetFullTypeName() == "System.Web.Services.WebServiceAttribute");


				var parent = ws.Class.Parent;
				while (parent != null && !(parent is NamespaceDeclarationSyntax)) parent = parent.Parent;
				NamespaceDeclarationSyntax serverNS, clientNS, oldNS;
				if (parent == null)
				{
					oldNS = null;
					serverNS = NamespaceDeclaration(IdentifierName("SolidCP.Services"));
					clientNS = NamespaceDeclaration(IdentifierName("SolidCP.Client"));

					serverTree = CompilationUnit()
						.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						.AddUsings(
							UsingDirective(ParseName("System.ServiceModel")),
							UsingDirective(ParseName("System.ServiceModel.Activation")));

					clientTree = CompilationUnit()
						.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						//.AddUsings(UsingDirective(ParseName("CoreWCF"))
						//	.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NET"), true, true, true)))
						//	.WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true))))
						.AddUsings(UsingDirective(ParseName("System.ServiceModel")));
							//.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NET48"), true, true, true)))
							//.WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true)))); ;

				}
				else
				{
					oldNS = (NamespaceDeclarationSyntax)parent;

					serverNS = NamespaceDeclaration(
						QualifiedName(oldNS.Name, IdentifierName("Services")))
						.WithUsings(oldNS.Usings)
						.WithExterns(oldNS.Externs);
					clientNS = NamespaceDeclaration(
						QualifiedName(oldNS.Name, IdentifierName("Client")))
						.WithUsings(oldNS.Usings)
						.WithExterns(oldNS.Externs);

					serverTree = CompilationUnit()
						.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						.AddUsings(UsingDirective(oldNS.Name))
						.AddUsings(
							UsingDirective(ParseName("System.ServiceModel")),
							UsingDirective(ParseName("System.ServiceModel.Activation")));

					clientTree = CompilationUnit()
						.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						.AddUsings(UsingDirective(oldNS.Name))
						.AddUsings(
							UsingDirective(ParseName("System.ServiceModel")));

				}

				var webServiceNamespace = attr.ArgumentList.Arguments
					.Where(a => a.NameEquals != null && a.NameEquals.Name.ToString() == "Namespace" && a.Expression is LiteralExpressionSyntax)
					.Select(a => (string)((LiteralExpressionSyntax)a.Expression).Token.Value)
					.FirstOrDefault() ?? "http://tempuri.org/";
				if (!webServiceNamespace.EndsWith("/")) webServiceNamespace = $"{webServiceNamespace}/";

				// wcf service contract interface
				var intf = ParseMemberDeclaration(
					new ServiceInterface()
					{
						WebServiceNamespace = webServiceNamespace,
						Class = ws.Class,
						WebMethods = methods
					}
					.Render())
					.NormalizeWhitespace();

				//wcf service class
				var service = ParseMemberDeclaration(
					new ServiceClass()
					{
						OldNamespace = oldNS.Name.ToString(),
						Class = ws.Class,
						WebMethods = methods
					}
					.Render())
					.NormalizeWhitespace();
				
				serverNS = serverNS
					.WithMembers(List(new MemberDeclarationSyntax[]
						{
							intf, service
						}));


				var client = ParseMemberDeclaration(
					new ClientClass()
					{
						Class = ws.Class,
						WebMethods = methods
					}
					.Render())
					.NormalizeWhitespace();


				var clientIntf = ParseMemberDeclaration(
					new ClientInterface()
					{
						WebServiceNamespace = webServiceNamespace,
						Class = ws.Class,
						WebMethods = methods
					}
					.Render())
					.NormalizeWhitespace();

				var clientAssemblyClass = ParseMemberDeclaration(
					new ClientAssemblyClass()
					{
						OldNamespace = oldNS.Name.ToString(),
						Class = ws.Class,
						WebMethods = methods
					}
					.Render())
					.NormalizeWhitespace();

				clientNS = clientNS
					.WithMembers(List(new MemberDeclarationSyntax[] {
						clientIntf, clientAssemblyClass, client
					}));


				serverTree = serverTree
					.AddMembers(serverNS)
					.NormalizeWhitespace();

				clientTree = clientTree
					.AddMembers(clientNS)
					.NormalizeWhitespace();

				var serverText = serverTree.ToString();
				var clientText = clientTree.ToString();
				serverText = Regex.Replace(serverText, "#(?:end)?region.*?(?=\\r?\\n)", "", RegexOptions.Singleline);
				clientText = Regex.Replace(clientText, "#(?:end)?region.*?(?=\\r?\\n)", "", RegexOptions.Singleline);

				var typeName = ws.Class.Identifier.Text;

				context.AddSource($"{typeName}.g", $"#if !Client{NewLine}{serverText}{NewLine}#endif");
				context.AddSource($"{typeName}Client.g", $"#if Client{NewLine}{clientText}{NewLine}#endif");
			}

		}

		public void Initialize(GeneratorInitializationContext context) {
		}
	}
}