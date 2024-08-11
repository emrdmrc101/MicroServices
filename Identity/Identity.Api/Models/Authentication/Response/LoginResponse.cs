using Identity.Api.Models.Common;

namespace Identity.Api.Models.Authentication.Response;

public class LoginResponse : BaseResponse
{
    public string Token { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public bool Succeeded { get; set; }

    public string ErrorMessage { get; set; }
}