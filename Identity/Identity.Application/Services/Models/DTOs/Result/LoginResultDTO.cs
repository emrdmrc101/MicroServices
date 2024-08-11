namespace Identity.Application.Services.Models.DTOs;

public class LoginResultDTO
{
    public string Token { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public bool Succeeded { get; set; }
    
    public string ErrorMessage { get; set; }
}