namespace Identity.Api.Models.Common.Objects;

public class TokenObject
{
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
}