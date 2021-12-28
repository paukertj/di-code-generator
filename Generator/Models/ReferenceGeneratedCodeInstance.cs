using DiDemo.Generator.Generator.Enums;

namespace DiDemo.Generator.Generator.Models
{
    internal class ReferenceGeneratedCodeInstance
    {
        internal TypeGeneratedCodeInstance Service { get; }
        internal ServiceLifetime ServiceLifetime { get; set;  } /// TODO - useless

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
