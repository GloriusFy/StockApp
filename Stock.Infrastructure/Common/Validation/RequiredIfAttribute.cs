using System.ComponentModel.DataAnnotations;

namespace Stock.Infrastructure.Common.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfAttribute : RequiredAttribute
{
    private readonly string _flagName;
    private readonly bool _condition;

    public RequiredIfAttribute(string flagName, bool condition)
    {
        _flagName = flagName;
        _condition = condition;
    }

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        var instance = context.ObjectInstance;
        var type = instance.GetType();

        if (!bool.TryParse(type.GetProperty(_flagName)!.GetValue(instance)?.ToString(), out var flagValue))
        {
            throw new InvalidOperationException($"{nameof(RequiredIfAttribute)} can be used only on bool properties.");
        }

        if (flagValue == _condition && (value == null || (value is string s && string.IsNullOrEmpty(s))))
        {
            return new ValidationResult($"Property {context.MemberName} must have a value when {_flagName} is {_condition}");
        }

        return ValidationResult.Success;
    }
}