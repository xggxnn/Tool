public interface IConfigReader
{
    void Load();
    void Reload();

    void OnGameConfigLoaded();
}