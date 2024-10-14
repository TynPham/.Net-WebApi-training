using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class IdentityErrorHandler
{
    public static IActionResult HandleIdentityError(IEnumerable<IdentityError> errors)
    {
        var errorList = errors.Select(e => new { e.Code, e.Description }).ToList();
        return new BadRequestObjectResult(new
        {
            Errors = errorList
        });
    }
}