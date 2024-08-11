using Identity.Api.Models.Common;

namespace Identity.Api.Models.Authentication.Request;

public class LoginRequest : BaseRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}