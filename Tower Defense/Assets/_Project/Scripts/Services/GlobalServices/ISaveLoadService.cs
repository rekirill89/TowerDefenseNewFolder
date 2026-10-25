namespace _Project.Scripts
{
    public interface ISaveLoadService
    {
        void Save(SaveData data);
        SaveData Load();
    }
}