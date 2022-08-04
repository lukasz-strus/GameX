namespace gamexModels;

public class ImageDto
{
    public int Id { get; set; }
    public byte[] ImageStream { get; set; }
    public int GameId { get; set; }
    public string Extension { get; set; }
}