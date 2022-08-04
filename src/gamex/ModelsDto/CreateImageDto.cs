namespace gamexModels;

public class CreateImageDto
{
    public byte[] ImageStream { get; set; }
    public int GameId { get; set; }
    public string Extension { get; set; }
}