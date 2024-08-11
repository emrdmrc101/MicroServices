namespace Identity.Application.Services.Models.Objects;

public class TokenObject
{
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
}