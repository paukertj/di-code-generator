using Microsoft.CodeAnalysis;
using System;

namespace DiDemo.Generator.Generator.Models
{
    internal class TypeGeneratedCodeInstance
    {
        internal string Namespace { get; }
        internal string TypeName { get; }
   
        internal TypeGeneratedCodeInstance(ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
            {
                throw new ArgumentNullException(nameof(typeSymbol));
            }

            Namespace = typeSymbol.ContainingNamespace.ToString();
            TypeName = typeSymbol.Name;
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
