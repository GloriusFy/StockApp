using System.Diagnostics.CodeAnalysis;

namespace Stock.WebAPI.Swagger.Filters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class SwaggerGroupAttribute : Attribute
{
    public string GroupName { get; }

    public SwaggerGroupAttribute(string groupName)
    {
        GroupName = groupName;
    }
}