// See https://aka.ms/new-console-template for more information

using System;
using System.Reflection;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.TypeScript;
using NSwag.CodeGeneration.OperationNameGenerators;

namespace TypeScriptClientGenerator
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version;

            Console.WriteLine($"SolidCP TypeScriptClientGenerator, v{version}\n" +
                $"Usage: TypeScriptClientGenerator <URL to swagger.json> <output path>\n");

            var interactive = false;
            string url, generatePath;
            if (args.Length != 2)
            {
                //throw new ArgumentException("Expecting 2 arguments: URL, generatePath");

                interactive = true;
                Console.Write("Url: ");
                url = Console.ReadLine();
                Console.Write("Output Path: ");
                generatePath = Console.ReadLine();
            }
            else
            {
                url = args[0];
                generatePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), args[1]));
            }

            Console.WriteLine($"Generating TypeScript client for url:\n{url}\n");

            await GenerateTypeScriptClient(url, generatePath);

            if (interactive)
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        async static Task GenerateTypeScriptClient(string url, string generatePath)
        {
            Console.WriteLine($"Generating {generatePath}...");

            var document = await OpenApiDocument.FromUrlAsync(url);
            if (document != null)
            {
                var settings = new TypeScriptClientGeneratorSettings();

                settings.ClientBaseClass = "ApiClientBase";
                settings.TypeScriptGeneratorSettings.TypeStyle = TypeScriptTypeStyle.Interface;
                settings.TypeScriptGeneratorSettings.TypeScriptVersion = 3.5M;
                settings.TypeScriptGeneratorSettings.DateTimeType = TypeScriptDateTimeType.String;
                settings.OperationNameGenerator = new SolidCPOperationNameGenerator();

                var generator = new NSwag.CodeGeneration.TypeScript.TypeScriptClientGenerator(document, settings);
                var code = generator.GenerateFile();

                await File.WriteAllTextAsync(generatePath, code);

                Console.WriteLine($"Generated {generatePath}");
            } else
            {
                Console.WriteLine($"Error while generating {generatePath}");
            }

        }
    }
}