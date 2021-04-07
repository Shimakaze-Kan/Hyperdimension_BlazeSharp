using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Components;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Hyperdimension_BlazeSharp.Client
{
    public class CompileService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _uriHelper;
        public List<string> CompileLog { get; set; }
        private List<MetadataReference> references { get; set; }


        public CompileService(HttpClient http, NavigationManager uriHelper)
        {
            _http = http;
            _uriHelper = uriHelper;
        }

        public async Task Init()
        {
            if (references == null)
            {
                references = new List<MetadataReference>();
                //foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                //{
                //    if (assembly.IsDynamic)
                //    {
                //        continue;
                //    }
                //    var name = assembly.GetName().Name + ".dll";
                //    if(name.Contains("crosoft.CodeAnalys"))
                //    {
                //        continue;
                //    }
                //    Console.WriteLine(name);
                //    references.Add(
                //        MetadataReference.CreateFromStream(
                //            await this._http.GetStreamAsync(_uriHelper.BaseUri + "_framework/" + name)));
                //}

                //references.Add(
                //        MetadataReference.CreateFromStream(
                //            await this._http.GetStreamAsync(_uriHelper.BaseUri + "_framework/mscorlib.dll")));

                var assemblies = new string[]
                    {"System.Private.CoreLib.dll",
                    "System.Runtime.dll",
                    "System.Collections.dll",
                    "System.Collections.Concurrent.dll",
                    "System.Text.Json.dll",
                    "System.Private.Uri.dll",
                    "System.dll",
                    "System.Console.dll",
                    "System.Linq.dll",
                    "System.ComponentModel.dll",
                    "netstandard.dll",
                    "System.Net.Http.dll",
                    "System.ObjectModel.dll",
                    "System.Text.Encodings.Web.dll",
                    "System.Memory.dll",
                    "System.Text.Encoding.Extensions.dll",
                    "System.Runtime.Intrinsics.dll",
                    "System.Threading.dll",
                    "System.Net.Primitives.dll",
                    "System.Diagnostics.Tracing.dll",
                    "System.Runtime.Loader.dll",
                    "System.Runtime.InteropServices.RuntimeInformation.dll",
                    "System.Reflection.Emit.Lightweight.dll",
                    "System.Reflection.Emit.ILGeneration.dll",
                    "System.Reflection.Primitives.dll ",
                    "System.Diagnostics.DiagnosticSource.dll",
                    "System.Linq.Expressions.dll",
                    "System.Net.Http.Json.dll",
                    "System.Runtime.CompilerServices.Unsafe.dll",
                    "System.Numerics.Vectors.dll",
                    "System.Runtime.Extensions.dll",
                    "System.Resources.ResourceManager.dll",
                    "System.Collections.Immutable.dll",
                    "System.Reflection.Metadata.dll"
                    };

                foreach (var item in assemblies)
                {
                    references.Add(
                            MetadataReference.CreateFromStream(
                                await this._http.GetStreamAsync(_uriHelper.BaseUri + $"_framework/{item}")));
                }
            }
        }


        

        public async Task<Assembly> Compile(string code)
        {
            await Init();

            SyntaxTree syntaxTree = await Task.Run(() => CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.Preview)));
            foreach (var diagnostic in syntaxTree.GetDiagnostics())
            {
                CompileLog.Add(diagnostic.ToString());
            }

            if (syntaxTree.GetDiagnostics().Any(i => i.Severity == DiagnosticSeverity.Error))
            {
                CompileLog.Add("Parse SyntaxTree Error!");
                return null;
            }

            CSharpCompilation compilation = await Task.Run(() => CSharpCompilation.Create("BlazeSharpPlayground", new[] { syntaxTree },
                references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)));

            using (MemoryStream stream = new MemoryStream())
            {
                EmitResult result = compilation.Emit(stream);

                foreach (var diagnostic in result.Diagnostics)
                {
                    CompileLog.Add(diagnostic.ToString());
                }

                if (!result.Success)
                {
                    CompileLog.Add("Compilation error");
                    return null;
                }

                stream.Seek(0, SeekOrigin.Begin);

                //                var context = new CollectibleAssemblyLoadContext();
                Assembly assemby = AppDomain.CurrentDomain.Load(stream.ToArray());
                return assemby;
            }

            return null;
        }


        //        public class CollectibleAssemblyLoadContext : AssemblyLoadContext
        //        {
        //            public CollectibleAssemblyLoadContext() : base()
        //            {
        //            }
        //
        //
        //            protected override Assembly Load(AssemblyName assemblyName)
        //            {
        //                return null;
        //            }
        //        }


        public async Task<string> CompileAndRun(string code)
        {
            await Init();

            var assemby = await this.Compile(code);
            if (assemby != null)
            {
                var type = assemby.GetExportedTypes().FirstOrDefault();
                var methodInfo = type.GetMethod("Run");
                var instance = Activator.CreateInstance(type);
                return (string)methodInfo.Invoke(instance, null);
            }

            return null;
        }
    }
}
