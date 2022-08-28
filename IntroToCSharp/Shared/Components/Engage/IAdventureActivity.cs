public interface IAdventureActivity
{
    public string DBName { get; }
    public string Title { get; }
    public int Gold { get; }
    public int XP { get; }
    public bool IsComplete { get; }
    public void AddTask (IAdventureTask task);
    public void Notify();
}