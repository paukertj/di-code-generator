using Microsoft.CodeAnalysis;

namespace DiCodeGenerator.Generator.Services.SemanticData
{
    public interface ISemanticDataService
    {
        SemanticModel GetSemanticModel(SyntaxTree syntaxTree);
    }
}
