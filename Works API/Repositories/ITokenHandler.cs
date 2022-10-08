namespace Works_API.Repositories
{
    public interface ITokenHandler
    {
      public Task<string> CreateTokenAsync(Models.Domain.User user);
    }
}
