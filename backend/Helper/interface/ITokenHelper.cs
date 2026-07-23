namespace backend.Helper;

public interface ITokenHelper
{
    public Task<bool> ValidateTokenAsync(string token);
    public string GenerateToken(string username);    
}