namespace WinUIStreamingClient.Core.Contracts.Services;

public interface IFileService
{
    T? Read<T>(string folderPath, string fileName) where T : class;

    void Save<T>(string folderPath, string fileName, T content);

    void Delete(string folderPath, string fileName);
}
