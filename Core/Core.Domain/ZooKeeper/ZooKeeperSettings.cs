namespace Core.Domain.ZooKeeper;

public class ZooKeeperSettings
{
    public string Url { get; set; }
    public string Path { get; set; }
    public Dictionary<string, string> Services { get; set; }
}