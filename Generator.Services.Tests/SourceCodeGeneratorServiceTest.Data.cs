using DiCodeGenerator.Generator.Services.SourceCodeBuilder;
using DiCodeGenerator.Generator.Services.SourceCodeBuilder.Enums;
using DiCodeGenerator.Generator.Services.SourceCodeBuilder.Primitives;
using System;
using System.Collections.Generic;

namespace DiCodeGenerator.Generator.Services.Tests
{
    public partial class SourceCodeGeneratorServiceTest
    {
        public static IEnumerable<object[]> TestCases => new List<object[]>
        {
            new object[]
            {
                @"
using System;

namespace Test
{
	public class GeneratedBuilder
	{
		private void TestMethod()
		{
		}
	}
}",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddUsing("System")
                    .AddNamespace("Test")
                    .AddClass(Modifiers.Public, "GeneratedBuilder")
                    .AddMethod(Modifiers.Private, "TestMethod"))
            },
            new object[]
            {
                @"
using System;

namespace Test
{
	internal static partial class GeneratedBuilder
	{
		internal static partial void TestMethod()
		{
		}
	}
}",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddUsing("System")
                    .AddNamespace("Test")
                    .AddClass(Modifiers.Internal | Modifiers.Static | Modifiers.Partial, "GeneratedBuilder")
                    .AddMethod(Modifiers.Internal | Modifiers.Static | Modifiers.Partial, "TestMethod"))
            },
            new object[]
            {
                @"
namespace Gereated.Test.Namespace
{
	internal partial class InternalPartialClass
	{
		internal static void InternalStaticMethod()
		{
		}
	}
}",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddNamespace("Gereated.Test.Namespace")
                    .AddClass(Modifiers.Internal | Modifiers.Partial, "InternalPartialClass")
                    .AddMethod(Modifiers.Internal | Modifiers.Static, "InternalStaticMethod"))
            },
            new object[]
            {
                @"
namespace Gereated.Test.Namespace
{
	public class PublicClass
	{
		private void PrivateMethod(string str)
		{
		}
	}
}",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddNamespace("Gereated.Test.Namespace")
                    .AddClass(Modifiers.Public, "PublicClass")
                    .AddMethod(Modifiers.Private, "PrivateMethod", new Argument("string", "str")))
            },
            new object[]
            {
                @"
namespace Gereated.Test.Namespace
{
	public class PublicClass
	{
		private void PrivateMethod(string str, int i)
		{
		}
	}
}",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddNamespace("Gereated.Test.Namespace")
                    .AddClass(Modifiers.Public, "PublicClass")
                    .AddMethod(Modifiers.Private, "PrivateMethod", new Argument("string", "str"), new Argument("int", "i")))
            },
            new object[]
            {
                @"
namespace Gereated.Test.Namespace
{
	public class PublicClass
	{
		private void PrivateMethod(string str, params int[] i)
		{
		}
	}
}",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddNamespace("Gereated.Test.Namespace")
                    .AddClass(Modifiers.Public, "PublicClass")
                    .AddMethod(Modifiers.Private, "PrivateMethod", new Argument("string", "str"), new Argument("params int[]", "i")))
            },
            new object[]
            {
                @"
namespace Gereated.Test.Namespace
{
	public class PublicClass
	{
		private void PrivateMethod(params string[] str)
		{
		}
	}
}",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddNamespace("Gereated.Test.Namespace")
                    .AddClass(Modifiers.Public, "PublicClass")
                    .AddMethod(Modifiers.Private, "PrivateMethod", new Argument("params string[]", "str")))
            },
            new object[]
            {
                @"
namespace Gereated.Test.Namespace
{
	public class PublicClass
	{
	}
}",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddNamespace("Gereated.Test.Namespace")
                    .AddClass(Modifiers.Public, "PublicClass"))
            },
            new object[]
            {
                @"
namespace Gereated.Test.Namespace
{
}",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddNamespace("Gereated.Test.Namespace"))
            },
            new object[]
            {
                @"
using SomeUsing;",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddUsing("SomeUsing"))
            },
            new object[]
            {
                @"
using Using1;
using Using2;
using Using3;",
                new Action<ISourceCodeBuilderService>(sb => sb
                    .AddUsing("Using1")
                    .AddUsing("Using2")
                    .AddUsing("Using3"))
            }
        };
    }
}
