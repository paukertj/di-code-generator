using DiDemo.Generator.Generator.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiDemo.Generator.Generator.Models
{
    internal class ReferenceGeneratedCodeInstance
    {
        internal TypeGeneratedCodeInstance Service { get; }
        internal ServiceLifetime ServiceLifetime { get; set;  } /// TODO

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
