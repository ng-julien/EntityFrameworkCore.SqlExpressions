using System.Reflection;
using EntityFrameworkCore.SqlExpressions.InMemory.Query.Internal;
using EntityFrameworkCore.SqlExpressions.Query;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EntityFrameworkCore.SqlExpressions;

internal sealed class DbContextInMemoryOptionsExtension : IDbContextOptionsExtension
{
    private readonly Dictionary<MethodInfo, MethodInfo> translateMethodInfoByOriginMethodInfo = new();
    private DbContextOptionsExtension.ExtensionInfo? info;

    public DbContextInMemoryOptionsExtension()
    {
        this.AddOriginDeclarationTranslate(SqlDbFunctionsExtensions.SoundexMethodInfo,
            SqlDbFunctionsInMemoryExtensions.SoundexMethodInfo);
    }

    public void ApplyServices(IServiceCollection services)
    {
        services.AddScoped<IDictionary<MethodInfo, MethodInfo>>(_ => this.translateMethodInfoByOriginMethodInfo)
            .AddScoped<IQueryableMethodTranslatingExpressionVisitorFactory,
                InMemoryQueryableMethodTranslatingExpressionVisitorFactory>()
            .AddScoped<IEvaluatableExpressionFilter>(provider =>
            {
                var dependencies = provider.GetRequiredService<EvaluatableExpressionFilterDependencies>();
                return new SqlDbFunctionEvaluatableExpressionFilter(dependencies,
                    this.translateMethodInfoByOriginMethodInfo.Keys.Select(k => k.DeclaringType!).ToArray());
            });
    }

    public void Validate(IDbContextOptions options)
    {
        // Method intentionally left empty.
    }

    public DbContextOptionsExtensionInfo Info => this.info ??= new DbContextOptionsExtension.ExtensionInfo(this,
        string.Join(", ",
            this.translateMethodInfoByOriginMethodInfo.Values.Select(m => $"{m.Name}: Ok")));

    internal void AddOriginDeclarationTranslate(MethodInfo origin, MethodInfo inMemory)
    {
        this.translateMethodInfoByOriginMethodInfo[origin] = inMemory;
    }
}