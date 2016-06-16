namespace Exercise2_Notes.Services
{
    public interface IStorageService
    {
        void Write<T>(string key, T value);

        T Read<T>(string key);
        T Read<T>(string key, T defaultValue);
    }
}