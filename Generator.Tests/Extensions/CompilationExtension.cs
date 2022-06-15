using Generator.Tests.Exceptions;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Reflection;

namespace Generator.Tests.Extensions
{
    internal static class CompilationExtension
    {
        public static TOutput Run<TOutput>(this Compilation compilation)
        {
            using (var ms = new MemoryStream())
            {
                // write IL code into memory
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    throw new TestCompilationRunException(result.Diagnostics);
                }

                // load this 'virtual' DLL so that we can use
                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());

                // create instance of the desired class and call the desired function
                Type type = assembly.GetType("Generator.Tests.TestCaseClass");
                object obj = Activator.CreateInstance(type);
                
                return (TOutput)type.InvokeMember("Main",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    obj,
                    new object[] { });
            }
        }
    }
}
