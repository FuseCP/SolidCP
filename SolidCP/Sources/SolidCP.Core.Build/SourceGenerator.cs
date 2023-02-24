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
				var methods = ws.Class.Members.OfType<MethodDeclarationSyntax>()
							.Where(m => m.AttributeLists
								.Any(l => l.Attributes
								.Any(a => ((INamedTypeSymbol)ws.Model.GetTypeInfo(a).Type).GetFullTypeName() == "System.Web.Services.WebMethodAttribute")))
							.ToArray();


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
						.AddUsings(UsingDirective(ParseName("CoreWCF")))
						.AddUsings(UsingDirective(ParseName("System.ServiceModel")));

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
						.AddUsings(UsingDirective(ParseName("System.ServiceModel")));

					clientTree = CompilationUnit()
						.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						.AddUsings(UsingDirective(oldNS.Name))
						.AddUsings(UsingDirective(ParseName("CoreWCF")))
						.AddUsings(UsingDirective(ParseName("System.ServiceModel")));

				}


				var intf = InterfaceDeclaration($"I{ws.Class.Identifier.Text}")
					.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
					.WithAttributeLists(ws.Class.AttributeLists
						.Add(AttributeList(
							SingletonSeparatedList<AttributeSyntax>(
								attr.WithName(IdentifierName("ServiceContract"))
						))))
					.WithMembers(List(
						ws.Class.Members.OfType<MethodDeclarationSyntax>()
							.Where(m => m.AttributeLists
								.Any(l => l.Attributes
								.Any(a => ((INamedTypeSymbol)ws.Model.GetTypeInfo(a).Type).GetFullTypeName() == "System.Web.Services.WebMethodAttribute")))
							.Select(m => (MemberDeclarationSyntax)MethodDeclaration(m.ReturnType, m.Identifier)
								.WithAttributeLists(m.AttributeLists
									.Add(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("OperationContract"))))))
								.WithParameterList(m.ParameterList)
								.WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))));

				var service = ClassDeclaration($"{ws.Class.Identifier.Text}Service")
					.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
					.WithBaseList(BaseList(SeparatedList<BaseTypeSyntax>(
						new BaseTypeSyntax[]
						{
							oldNS.Name.ToString() == null ? SimpleBaseType(ParseTypeName($"global::{ws.Class.Identifier.Text}")) :
								SimpleBaseType(ParseTypeName($"{oldNS.Name.ToString()}.{ws.Class.Identifier.Text}")),
							SimpleBaseType(ParseTypeName($"I{ws.Class.Identifier.Text}"))
						})))
					.WithMembers(List(methods
							.Select(m => (MemberDeclarationSyntax)MethodDeclaration(m.ReturnType, m.Identifier)
								.WithModifiers(m.Modifiers.Add(Token(SyntaxKind.NewKeyword)))
								.WithParameterList(m.ParameterList)
								.WithBody(Block(
									SingletonList<StatementSyntax>(
										(m.ReturnType is PredefinedTypeSyntax && ((PredefinedTypeSyntax)m.ReturnType).Keyword.IsKind(SyntaxKind.VoidKeyword)) ?
											(StatementSyntax)ExpressionStatement(
												InvocationExpression(
													MemberAccessExpression(
														SyntaxKind.SimpleMemberAccessExpression,
														BaseExpression(),
														IdentifierName(m.Identifier)))
												.WithArgumentList(
													ArgumentList(
														SeparatedList<ArgumentSyntax>(m.ParameterList.Parameters
															.Select(par => Argument(IdentifierName(par.Identifier)))))))
											:
											(StatementSyntax)ReturnStatement(
												InvocationExpression(
													MemberAccessExpression(
														SyntaxKind.SimpleMemberAccessExpression,
														BaseExpression(),
														IdentifierName(m.Identifier)))
													.WithArgumentList(
														ArgumentList(
															SeparatedList<ArgumentSyntax>(m.ParameterList.Parameters
																.Select(par => Argument(IdentifierName(par.Identifier)))))))))))));

				serverNS = serverNS
					.WithMembers(List(new MemberDeclarationSyntax[]
						{
							intf, service
						}));


				var client = ClassDeclaration(ws.Class.Identifier.Text)
					.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
					.WithMembers(List<MemberDeclarationSyntax>(
						new MemberDeclarationSyntax[]{
							PropertyDeclaration(
								GenericName(
									 Identifier("ChannelFactory"))
								.WithTypeArgumentList(
									 TypeArgumentList(
										  SingletonSeparatedList<TypeSyntax>(
												IdentifierName("T")))),
								Identifier("_Factory"))
						  .WithAccessorList(
								AccessorList(
									 List<AccessorDeclarationSyntax>(
										  new AccessorDeclarationSyntax[]{
												AccessorDeclaration(
													 SyntaxKind.GetAccessorDeclaration)
												.WithSemicolonToken(
													 Token(SyntaxKind.SemicolonToken)),
												AccessorDeclaration(
													 SyntaxKind.SetAccessorDeclaration)
												.WithSemicolonToken(
													 Token(SyntaxKind.SemicolonToken))}))),
						  PropertyDeclaration(
								IdentifierName("Credentials"),
								Identifier("Credentials"))
						  .WithModifiers(
								TokenList(
									 Token(SyntaxKind.PublicKeyword)))
						  .WithAccessorList(
								AccessorList(
									 List<AccessorDeclarationSyntax>(
										  new AccessorDeclarationSyntax[]{
												AccessorDeclaration(
													 SyntaxKind.GetAccessorDeclaration)
												.WithSemicolonToken(
													 Token(SyntaxKind.SemicolonToken)),
												AccessorDeclaration(
													 SyntaxKind.SetAccessorDeclaration)
												.WithSemicolonToken(
													 Token(SyntaxKind.SemicolonToken))}))),
						  PropertyDeclaration(
								PredefinedType(
									 Token(SyntaxKind.ObjectKeyword)),
								Identifier("SoapHeader"))
						  .WithModifiers(
								TokenList(
									 Token(SyntaxKind.PublicKeyword)))
						  .WithAccessorList(
								AccessorList(
									 List<AccessorDeclarationSyntax>(
										  new AccessorDeclarationSyntax[]{
												AccessorDeclaration(
													 SyntaxKind.GetAccessorDeclaration)
												.WithSemicolonToken(
													 Token(SyntaxKind.SemicolonToken)),
												AccessorDeclaration(
													 SyntaxKind.SetAccessorDeclaration)
												.WithSemicolonToken(
													 Token(SyntaxKind.SemicolonToken))}))),
						  MethodDeclaration(
								PredefinedType(
									 Token(SyntaxKind.VoidKeyword)),
								Identifier("Test"))
						  .WithBody(
								Block(
									 SingletonList<StatementSyntax>(
										  TryStatement(
												SingletonList<CatchClauseSyntax>(
													 CatchClause()
													 .WithBlock(
														  Block(
																SingletonList<StatementSyntax>(
																	 ExpressionStatement(
																		  ConditionalAccessExpression(
																				ParenthesizedExpression(
																					 BinaryExpression(
																						  SyntaxKind.AsExpression,
																						  IdentifierName("client"),
																						  IdentifierName("ICommunicationObject"))),
																				InvocationExpression(
																					 MemberBindingExpression(
																						  IdentifierName("Abort"))))))))))
										  .WithBlock(
												Block(
													 LocalDeclarationStatement(
														  VariableDeclaration(
																IdentifierName(
																	 Identifier(
																		  TriviaList(),
																		  SyntaxKind.VarKeyword,
																		  "var",
																		  "var",
																		  TriviaList())))
														  .WithVariables(
																SingletonSeparatedList<VariableDeclaratorSyntax>(
																	 VariableDeclarator(
																		  Identifier("client"))
																	 .WithInitializer(
																		  EqualsValueClause(
																				InvocationExpression(
																					 MemberAccessExpression(
																						  SyntaxKind.SimpleMemberAccessExpression,
																						  IdentifierName("_Factory"),
																						  IdentifierName("CreateChannel")))))))),
													 ExpressionStatement(
														  InvocationExpression(
																MemberAccessExpression(
																	 SyntaxKind.SimpleMemberAccessExpression,
																	 IdentifierName("client"),
																	 IdentifierName("MyServiceOperation")))),
													 ExpressionStatement(
														  InvocationExpression(
																MemberAccessExpression(
																	 SyntaxKind.SimpleMemberAccessExpression,
																	 ParenthesizedExpression(
																		  CastExpression(
																				IdentifierName("ICommunicationObject"),
																				IdentifierName("client"))),
																	 IdentifierName("Close")))),
													 ExpressionStatement(
														  InvocationExpression(
																MemberAccessExpression(
																	 SyntaxKind.SimpleMemberAccessExpression,
																	 IdentifierName("_Factory"),
																	 IdentifierName("Close")))))))))}));

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

				var serverText = serverTree.ToString()
					.Replace("using CoreWCF;", $"#if NET6_0{NewLine}using CoreWCF;{NewLine}#endif")
					.Replace("using System.ServiceModel;", $"#if !NET6_0{NewLine}using System.ServiceModel;{NewLine}#endif");
	
				var clientText = clientTree.ToString()
					.Replace("using CoreWCF;", $"#if NET6_0{NewLine}using CoreWCF;{NewLine}#endif")
					.Replace("using System.ServiceModel;", $"#if !NET6_0{NewLine}using System.ServiceModel;{NewLine}#endif");

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