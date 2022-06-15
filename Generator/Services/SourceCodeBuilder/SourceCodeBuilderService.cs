using DiCodeGenerator.Generator.Enums;
using DiCodeGenerator.Generator.Extensions;
using DiCodeGenerator.Generator.Models.Generating.DependencyInjectionGeneratedCode;
using DiCodeGenerator.Generator.Models.Generating.ReferenceGeneratedCode;
using DiCodeGenerator.Generator.Services.SourceCodeBuilder.Enums;
using DiCodeGenerator.Generator.Services.SourceCodeBuilder.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiCodeGenerator.Generator.Services.SourceCodeBuilder
{
    internal class SourceCodeBuilderService : ISourceCodeBuilderService
    {
        private readonly StringBuilder _namespaces = new StringBuilder();
        private readonly Dictionary<string, ISourceCodeBuilderService> _namespacesBlocks = new Dictionary<string, ISourceCodeBuilderService>();
        private readonly Dictionary<string, ISourceCodeBuilderService> _classesBlocks = new Dictionary<string, ISourceCodeBuilderService>();
        private readonly Dictionary<string, ISourceCodeBuilderService> _methodsBlocks = new Dictionary<string, ISourceCodeBuilderService>();
        private readonly StringBuilder _codeBlock = new StringBuilder();

        public ISourceCodeBuilderService AddUsing(IEnumerable<string> value)
        {
            if (value == null)
            {
                return this;
            }

            foreach (string ns in value)
            {
                AddUsing(ns);
            }

            return this;
        }

        public ISourceCodeBuilderService AddUsing(string value)
        {
            _namespaces.Append("using ");
            _namespaces.Append(value);
            _namespaces.Append(';');
            _namespaces.AppendLine();

            return this;
        }

        public ISourceCodeBuilderService AddNamespace(string value)
        {
            if (_namespacesBlocks.ContainsKey(value))
            {
                throw new ArgumentException($"Namespace '{value}' has been registered!");
            }

            var innerBuilder = new SourceCodeBuilderService();

            _namespacesBlocks.Add(value, innerBuilder);

            return innerBuilder;
        }

        public ISourceCodeBuilderService AddClass(Modifiers modifiers, string name)
        {
            string classSignature = ModifiersToString(modifiers);

            classSignature += "class " + name;

            if (_classesBlocks.ContainsKey(classSignature))
            {
                throw new ArgumentException($"Class '{classSignature}' has been registered in this namespace!");
            }

            var innerBuilder = new SourceCodeBuilderService();

            _classesBlocks.Add(classSignature, innerBuilder);

            return innerBuilder;
        }

        public ISourceCodeBuilderService AddMethod(Modifiers modifiers, string name, params Argument[] arguments)
        {
            return AddMethod(modifiers, name, "void", arguments);
        }

        public ISourceCodeBuilderService AddMethod<TReturn>(Modifiers modifiers, string name, params Argument[] arguments)
        {
            return AddMethod(modifiers, name, typeof(TReturn).Name, arguments);
        }

        private ISourceCodeBuilderService AddMethod(Modifiers modifiers, string name, string returnType, params Argument[] arguments)
        {
            string methodSignature = ModifiersToString(modifiers);

            methodSignature += returnType + ' ' + name + "(" + ArgumentsToString(arguments) + ")";

            if (_methodsBlocks.ContainsKey(methodSignature))
            {
                throw new ArgumentException($"Method '{methodSignature}' has been registered in this class!");
            }

            var innerBuilder = new SourceCodeBuilderService();

            _methodsBlocks.Add(methodSignature, innerBuilder);

            return innerBuilder;
        }

        public ISourceCodeBuilderService AddServiceCollection(string collection, IEnumerable<IDependencyInjectionGeneratedCodeInstance> instances)
        {
            if (instances == null)
            {
                return this;
            }

            foreach (var instance in instances)
            {
                AddServiceCollection(collection, instance);
            }

            return this;
        }

        public ISourceCodeBuilderService AddServiceCollection(string collection, IDependencyInjectionGeneratedCodeInstance instance)
        {
            // TODO: Remove! instance.References.ToSourceCode()
            AddServiceCollection(collection, instance.Service.TypeName, instance.Implementation.TypeName, instance.References, instance.ServiceLifetime);

            return this;
        }

        private ISourceCodeBuilderService AddServiceCollection(string collection, string serviceTypeName, string implementationTypeName, IEnumerable<IReferenceGeneratedCodeInstance> parameters, ServiceLifetime serviceLifetime)
        {
            // $"{collection}.Add(new ServiceDescriptor(typeof({serviceTypeName}), (p) => new {implementationTypeName}({parameters}), ServiceLifetime.{serviceLifetime}));

            _codeBlock.Append(collection);
            _codeBlock.Append(".Add(new ServiceDescriptor(typeof(");
            _codeBlock.Append(serviceTypeName);
            _codeBlock.Append("), (p) => new ");
            _codeBlock.Append(implementationTypeName);
            _codeBlock.Append('(');

            AddServiceCollectionParameters(parameters);

            _codeBlock.Append("), ServiceLifetime.");
            _codeBlock.Append(serviceLifetime);
            _codeBlock.Append("));");
            _codeBlock.AppendLine();

            return this;
        }

        private void AddServiceCollectionParameters(IEnumerable<IReferenceGeneratedCodeInstance> parameters)
        {
            var eParameters = parameters?.ToList();

            if (eParameters?.Any() != true)
            {
                return;
            }

            for (int i = 0; i < eParameters.Count; i++)
            {
                _codeBlock.Append("p.GetRequiredService<");
                _codeBlock.Append(eParameters[i].Service.TypeName);
                _codeBlock.Append(">()");

                if (i < eParameters.Count - 1)
                {
                    _codeBlock.Append(", ");
                }
            }
        }

        private string ArgumentsToString(params Argument[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach (var argument in arguments)
            {
                sb.Append(argument.Type);
                sb.Append(' ');
                sb.Append(argument.Name);
                sb.Append(", ");
            }

            return sb
                .ToString()
                .Trim()
                .TrimEnd(',');
        }

        private string ModifiersToString(Modifiers modifiers)
        {
            string s = string.Empty;

            if (modifiers == Modifiers.Public || (modifiers & Modifiers.Public) != 0)
            {
                s += "public ";
            }

            if (modifiers == Modifiers.Internal || (modifiers & Modifiers.Internal) != 0)
            {
                s += "internal ";
            }

            if (modifiers == Modifiers.Private || (modifiers & Modifiers.Private) != 0)
            {
                s += "private ";
            }

            if (modifiers == Modifiers.Protected || (modifiers & Modifiers.Protected) != 0)
            {
                s += "protected ";
            }

            if (modifiers == Modifiers.Static || (modifiers & Modifiers.Static) != 0)
            {
                s += "static ";
            }

            if (modifiers == Modifiers.Partial || (modifiers & Modifiers.Partial) != 0)
            {
                s += "partial ";
            }

            return s;
        }

        public StringBuilder ToSourceCode()
        {
            var sb = new StringBuilder();

            sb.Append(_namespaces);

            foreach (var r in GetNamespacesBlocks())
            {
                BuildFromNamespaces(sb, r.Key, r.Value);
            }

            return sb;
        }

        private void BuildFromNamespaces(StringBuilder sb, string namespaceSignature, ISourceCodeBuilderService content)
        {
            sb.AppendLine();
            sb.Append("namespace ");
            sb.Append(namespaceSignature);
            sb.AppendLine();
            sb.AppendLine("{");

            foreach (var r in content.GetClassesBlocks())
            {
                BuildFromClass(sb, r.Key, r.Value, 1);
            }

            sb.AppendLine("}");
        }

        private void BuildFromClass(StringBuilder sb, string classSignature, ISourceCodeBuilderService content, int level)
        {
            AddTab(sb, level);
            sb.AppendLine(classSignature);
            AddTab(sb, level);
            sb.AppendLine("{");

            foreach (var r in content.GetMethodsBlocks())
            {
                BuildFromMethod(sb, r.Key, r.Value, level + 1);
            }

            AddTab(sb, level);
            sb.AppendLine("}");
        }

        private void BuildFromMethod(StringBuilder sb, string methodSignature, ISourceCodeBuilderService content, int level)
        {
            AddTab(sb, level);
            sb.AppendLine(methodSignature);
            AddTab(sb, level);
            sb.AppendLine("{");

            var codeBlock = content.GetSourceBlock();
            
            AddTab(sb, level);
            sb.Append(codeBlock);

            sb.AppendLine("}");
        }

        private void AddTab(StringBuilder sb, int level)
        {
            if (level <= 0)
            {
                return;
            }

            for (int i = 0; i < level; i++)
            {
                sb.Append("\t");
            }
        }

        public StringBuilder GetSourceBlock()
        {
            return _codeBlock;
        }

        public IReadOnlyDictionary<string, ISourceCodeBuilderService> GetMethodsBlocks()
        {
            return _methodsBlocks;
        }

        public IReadOnlyDictionary<string, ISourceCodeBuilderService> GetClassesBlocks()
        {
            return _classesBlocks;
        }

        public IReadOnlyDictionary<string, ISourceCodeBuilderService> GetNamespacesBlocks()
        {
            return _namespacesBlocks;
        }
    }
}
