using Microsoft.CodeAnalysis;

namespace DiCodeGenerator.Generator.Services.SemanticData
{
    internal class SemanticDataService : ISemanticDataService
    {
        private readonly GeneratorExecutionContext _context;

        public SemanticDataService(GeneratorExecutionContext context)
        {
            _context = context;
        }

        public SemanticModel GetSemanticModel(SyntaxTree syntaxTree)
        {
            return _context.Compilation.GetSemanticModel(syntaxTree);
        }
    }
}
