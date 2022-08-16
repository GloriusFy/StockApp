using System.Text.Json;
using FluentValidation.Results;

namespace Stock.Application.Common.Exceptions;

public class InputValidationException : Exception
{
    public InputValidationException(Exception? innerException = null)
        : base("One or more validation failures have occurred.", innerException)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public InputValidationException(params (string PropertyName, string ErrorMessage)[] failures) : this()
    {
        SaveGroupedErrors(failures);
    }

    public InputValidationException(Exception innerException,
        params (string PropertyName, string ErrorMessage)[] failures) : this(innerException)
    {
        SaveGroupedErrors(failures);
    }

    public InputValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        SaveGroupedErrors(failures.Select(x => (x.PropertyName, x.ErrorMessage)));
    }

    public IDictionary<string, string[]> Errors { get; }

    private void SaveGroupedErrors(IEnumerable<(string PropertyName, string ErrorMessage)> failures)
    {
        var failureGroups = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Errors.Add(propertyName, propertyFailures);
        }
    }

    public override string ToString()
    {
        return nameof(InputValidationException) + ": " +
               JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}