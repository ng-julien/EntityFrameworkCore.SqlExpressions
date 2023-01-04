using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.SqlExpressions.Query.SqlExpressions;

public sealed class SoundexSqlFunctionExpression : SqlFunctionExpression
{
    public SoundexSqlFunctionExpression(params SqlExpression[] arguments)
        : base("SOUNDEX", arguments, false, arguments.Select(a => false), typeof(string), null)
    {
    }
}