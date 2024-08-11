namespace Identity.Application.Services.Models.DTOs;

public class GenerateTokenResultDTO
{
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
}