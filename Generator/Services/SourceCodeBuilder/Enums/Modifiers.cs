using System;

namespace DiCodeGenerator.Generator.Services.SourceCodeBuilder.Enums
{
    [Flags]
    public enum Modifiers
    {
        Public = 0,
        Internal = 1,
        Protected = 2,
        Private = 4,
        Static = 8,
        Partial = 16
    }
}
