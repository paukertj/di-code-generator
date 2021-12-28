using DiDemo.Generator.Generator.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiDemo.Generator.Generator.Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static void AppendRecord(this StringBuilder sb, KeyValuePair<DependencyInjectionInstance, List<string>> data, Dictionary<DependencyInjectionInstance, List<string>> map)
        {
            //if (data.Key == null || sb == null)
            //{
            //    return;
            //}

            //string sParams = string.Empty;
           
            //if (data.Value != null)
            //{
            //    foreach (var relationship in data.Value.Distinct())
            //    {
            //        var r = map.SingleOrDefault(r => r.Key.Service == relationship);

            //        sParams += $"p.GetRequiredService<{r.Key.Service}>(), ";
            //    }

            //    sParams = sParams.Trim().TrimEnd(',');
            //}

            //string s = $"serviceCollection.Add(new ServiceDescriptor(typeof({data.Key.Service}), (p) => new {data.Key.Implementation}({sParams}), ServiceLifetime.{data.Key.ServiceLifetime}));";

            //sb.AppendLine(s);
        }

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
