namespace gamexAPI.Entities;

public class GameSerial
{
    public int Id { get; set; }
    public string Value { get; set; }
    public int GameId { get; set; }
    public virtual Game Game { get; set; }
}