using DiDemo.Generator.Generator.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiDemo.Generator.Generator.Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static string ToSourceCode(this IReadOnlyList<ReferenceGeneratedCodeInstance> instances)
        {
            if (instances == null)
            {
                return string.Empty;
            }

            var e = instances.Select(i => i.ToSourceCode());
            return string.Join(", ", e);
        }
    }
}
