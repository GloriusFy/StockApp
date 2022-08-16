using System.ComponentModel.DataAnnotations;
using MediatR;
using Stock.Application.Common.Exceptions;

namespace Stock.Application.Common.Dependency.DataAccess.Repositories.Common;

public class PaginatedQueryModel<TDto> : IRequest<IPaginatedResponseModel<TDto>>
{
    private const int DEFAULT_PAGESIZE = 20;
    private const int MAX_PAGESIZE = 100;

    [Range(1, int.MaxValue, ErrorMessage = "The minimum page index is 1.")]
    public int PageIndex { get; set; } = 1;

    [Range(1, MAX_PAGESIZE)] public int PageSize { get; set; } = DEFAULT_PAGESIZE;

    public string OrderBy { get; set; } = "id";

    public string Filter { get; set; }

    public void ThrowOrderByIncorrectException(Exception innerException)
    {
        throw new InputValidationException(innerException,
            (
                PropertyName: nameof(OrderBy),
                ErrorMessage: $"The specified orderBy string '{OrderBy}' is invalid."
            )
        );
    }

    public void ThrowFilterIncorrectException(Exception innerException)
    {
        throw new InputValidationException(innerException,
            (
                PropertyName: nameof(Filter),
                ErrorMessage: $"The specified filter string '{Filter}' is invalid."
            )
        );
    }
}