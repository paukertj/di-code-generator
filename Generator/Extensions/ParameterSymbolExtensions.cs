﻿using DiDemo.Generator.Generator.Models.Generating;
using Microsoft.CodeAnalysis;

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
