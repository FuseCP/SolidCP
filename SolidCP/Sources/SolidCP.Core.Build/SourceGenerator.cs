#if NETSTANDARD2_0_OR_GREATER

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

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
				.Where(c => c.Class
					.DescendantNodes()
					.OfType<AttributeSyntax>()
					.Any(d => 
					((INamedTypeSymbol)c.Model.GetTypeInfo(d).Type).GetFullTypeName() == "SolidCP.WebServices.WebServiceAttribute"))
					//.Any(e =>
					//	c.Model.GetTypeInfo(e).Type.Name == "SolidCP.WebServices.WebService"))
				.ToList();
					
			foreach (var ws in webServiceClasses)
			{
				// create WCF service
				var typeName = ws.Model.GetDeclaredSymbol(ws.Class).Name;
				context.AddSource(typeName, $"//WebService {typeName}");
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
#endif