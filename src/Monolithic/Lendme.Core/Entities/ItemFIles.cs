namespace Lendme.Core.Entities;

public class ItemFIles
{
    public int Id { get; set; }
    public string Path { get; set; }
    public FileType Type { get; set; }
}

public enum FileType
{
    Image,
    Video,
    Audio
}