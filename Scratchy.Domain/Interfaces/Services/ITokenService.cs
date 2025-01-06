namespace Scratchy.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync();
    }
}
