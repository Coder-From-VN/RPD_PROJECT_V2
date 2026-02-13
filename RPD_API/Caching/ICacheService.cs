namespace RPD_API.Caching
{
    public interface ICacheService
    {
        Task RemoveByPrefixAsync(string prefix);
        Task ClearAllAsync();
    }
}
