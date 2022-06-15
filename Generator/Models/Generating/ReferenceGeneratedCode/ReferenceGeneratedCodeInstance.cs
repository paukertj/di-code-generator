using DiCodeGenerator.Generator.Enums;
using DiCodeGenerator.Generator.Models.Generating.TypeGeneratedCode;
using System.Collections.Generic;

namespace DiCodeGenerator.Generator.Models.Generating.ReferenceGeneratedCode
{
    internal class ReferenceGeneratedCodeInstance : IReferenceGeneratedCodeInstance
    {
        public ITypeGeneratedCodeInstance Service { get; }

        internal ReferenceGeneratedCodeInstance(ITypeGeneratedCodeInstance service)
        {
            Service = service;
        }
    }
}
