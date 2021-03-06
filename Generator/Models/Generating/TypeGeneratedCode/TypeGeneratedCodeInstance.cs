using Microsoft.CodeAnalysis;
using System;

namespace DiCodeGenerator.Generator.Models.Generating.TypeGeneratedCode
{
    internal class TypeGeneratedCodeInstance : ITypeGeneratedCodeInstance
    {
        public string FullName { get; }
        public string Namespace { get; }
        public string TypeName { get; }

        internal TypeGeneratedCodeInstance(ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
            {
                throw new ArgumentNullException(nameof(typeSymbol));
            }

            Namespace = typeSymbol.ContainingNamespace.ToString();
            TypeName = typeSymbol.Name;

            FullName = string.Join(".", Namespace, TypeName);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not TypeGeneratedCodeInstance typeGeneratedCodeInstance)
            {
                return false;
            }

            return typeGeneratedCodeInstance.TypeName == TypeName && typeGeneratedCodeInstance.Namespace == Namespace;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
