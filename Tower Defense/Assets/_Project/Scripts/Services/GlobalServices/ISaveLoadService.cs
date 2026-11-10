namespace _Project.Scripts.Services
{
    public interface ISaveLoadService
    {
        void Save(SaveData data);
        SaveData Load();
    }
}