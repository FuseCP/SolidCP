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
using RazorEngine;

namespace SolidCP.Core.Build
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
						.AddUsings(UsingDirective(ParseName("CoreWCF")))
						.AddUsings(UsingDirective(ParseName("System.ServiceModel")));

					clientTree = CompilationUnit()
						.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						.AddUsings(UsingDirective(ParseName("CoreWCF"))
							.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NET6_0"), true, true, true)))
							.WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true))))
						.AddUsings(UsingDirective(ParseName("System.ServiceModel"))
							.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NET48"), true, true, true)))
							.WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true))));

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
						.AddUsings(UsingDirective(ParseName("CoreWCF")))
							.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NET6_0"), true, true, true)))
							.WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true)))
						.AddUsings(UsingDirective(ParseName("System.ServiceModel"))
							.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NET48"), true, true, true)))
							.WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true))));

					clientTree = CompilationUnit()
						.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						.AddUsings(UsingDirective(oldNS.Name))
						.AddUsings(UsingDirective(ParseName("CoreWCF")))
						.AddUsings(UsingDirective(ParseName("System.ServiceModel")));

				}

				// wcf contract interface
				var intf = ParseMemberDeclaration(
					Razor.Parse(@"
						public interface @Model.Name {
							
							@for (var method in Model.Methods) {
								@method
							}
						}", new {
							Name = $"I{ws.Class.Identifier.Text}",
							Methods = methods
								// select method signature
								.Select(m => (MemberDeclarationSyntax)MethodDeclaration(m.ReturnType, m.Identifier)
									.WithAttributeLists(m.AttributeLists
										.Add(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("OperationContract"))))))
									.WithParameterList(m.ParameterList)
									.WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))
							}));


				var service = ParseMemberDeclaration(
					Razor.Parse(@"
						// wcf service
						public class @Model.Name: @Model.BaseType, @Model.ContractInterface {

							@for (var method in Model.Methods) {
								 @:public @method.ReturnType @method.Name @method.ParameterList {
									@m.ReturnToken base.@method.Name (@method.Arguments);
								}
							}
						}
					",
						new
						{
							Name = $"{ws.Class.Identifier.Text}Service",
							BaseType = "",
							ContractInterface = "",
							Methods = methods
								.Select(m => new
								{
									Name = m.Identifier.Text,
									ReturnType = m.ReturnType,
									ParameterList = m.ParameterList,
									Arguments = ArgumentList(
														SeparatedList<ArgumentSyntax>(m.ParameterList.Parameters
															.Select(par => Argument(IdentifierName(par.Identifier))))),
									ReturnToken = ((m.ReturnType is PredefinedTypeSyntax && ((PredefinedTypeSyntax)m.ReturnType).Keyword.IsKind(SyntaxKind.VoidKeyword))  ?
										"return" : "")
								})
						}));

				serverNS = serverNS
					.WithMembers(List(new MemberDeclarationSyntax[]
						{
							intf, service
						}));


				var client = ParseMemberDeclaration(
					Razor.Parse(@"

						// web service client
						public class Model.Name {
							
						}
"));

				clientNS = clientNS
					.WithMembers(List(new MemberDeclarationSyntax[] {
						intf, client
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
#if DEBUG
			if (!Debugger.IsAttached)
			{
				//Debugger.Launch();
			}
#endif
		}
	}
}