using System;

namespace DiCodeGenerator.Generator.Services.SourceCodeBuilder.Primitives
{
    public record Argument
    {
        public string Name { get; }

        public string Type { get; }

        public Argument(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public static Argument New<T>(string name)
        {
            return new Argument(typeof(T).Name, name);
        }
    }
}
