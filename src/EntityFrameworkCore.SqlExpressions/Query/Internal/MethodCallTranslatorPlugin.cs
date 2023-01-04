using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.SqlExpressions.Query.Internal;

internal sealed class MethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
{
    public MethodCallTranslatorPlugin(IMethodCallTranslator[] methodCallTranslators)
    {
        this.Translators = methodCallTranslators;
    }

    public IEnumerable<IMethodCallTranslator> Translators { get; }
}