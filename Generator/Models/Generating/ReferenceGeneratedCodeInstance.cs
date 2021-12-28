using DiDemo.Generator.Generator.Enums;
using System.Collections.Generic;

namespace DiDemo.Generator.Generator.Models.Generating
{
    internal class ReferenceGeneratedCodeInstance
    {
        internal TypeGeneratedCodeInstance Service { get; }

        internal ReferenceGeneratedCodeInstance(TypeGeneratedCodeInstance service)
        {
            Service = service;
        }

        internal string ToSourceCode()
        {
            return $"p.GetRequiredService<{Service.TypeName}>()";
        }
    }
}
