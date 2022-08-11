namespace gamexEntities;

public class Image
{
    public int Id { get; set; }
    public byte[] ImageStream { get; set; }
    public int GameId { get; set; }
    public virtual Game Game { get; set; }
    public string Extension { get; set; }
}