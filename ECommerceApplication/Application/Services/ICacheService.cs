namespace Application.Services;

public interface ICacheService
{
    Task<string> GetValueAsync(string key);
    Task<bool> SetValueAsync(string key, string value);
    Task ClearAsync(string key);
    Task ClearAllAsync();
}
