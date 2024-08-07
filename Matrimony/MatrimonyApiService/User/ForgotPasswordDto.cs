using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.User;

public record ForgotPasswordDto
{
    [Required]
    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    [MaxLength(256)]
    public string Email { get; init; }
}