using DiDemo.Generator.Generator.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiDemo.Generator.Generator.Extensions
{
    internal static class ParameterSymbolExtensions
    {
        internal static ReferenceGeneratedCodeInstance ToReferenceGeneratedCodeInstance(this IParameterSymbol parameterSymbol)
        {
            if (parameterSymbol?.Type == null)
            {
                return null;
            }

            var typeGeneratedCodeInstance = new TypeGeneratedCodeInstance(parameterSymbol.Type);

            return new ReferenceGeneratedCodeInstance(typeGeneratedCodeInstance);
        }
    }
}
