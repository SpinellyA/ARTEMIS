public interface ILocalStorageService
{
    ValueTask ClearAsync();
    ValueTask<bool> ContainsKeyAsync(string key);
    ValueTask<T?> GetAsync<T>(string key);
    ValueTask<string?> KeyAsync(int index);
    ValueTask<int> LengthAsync();
    ValueTask RemoveAsync(string key);
    ValueTask SetAsync<T>(string key, T value);

    ValueTask SetRawAsync(string key, string value);
    ValueTask<string?> GetRawAsync(string key);
}