using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure;


public class DbExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetails;

    public DbExceptionHandler(IProblemDetailsService problemDetails)
    {
        _problemDetails = problemDetails;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        string detail;
        switch (exception)
        
        {
            case DbUpdateConcurrencyException:
                detail = "Данные были изменены другим запросом. Повторите операцию.";
                break;
            case DbUpdateException:
                detail = "Операция нарушает ограничения базы данных: значение должно быть уникальным " +
                         "или на запись ссылаются другие данные.";
                break;
            default:
                return false;
        }

        context.Response.StatusCode = StatusCodes.Status409Conflict;
        return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Конфликт данных",
                Detail = detail
            }
        });
    }
}
