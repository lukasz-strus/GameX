namespace gamexDesktopApp.Models;

public class GameBuilder
{
    private Game _game = new();

    public Game Build() => _game;

    public GameBuilder SetId(int id)
    {
        _game.Id = id;
        return this;
    }

    public GameBuilder SetName(string name)
    {
        _game.Name = name;
        return this;
    }

    public GameBuilder SetDescription(string description)
    {
        _game.Description = description;
        return this;
    }

    public GameBuilder SetPrice(decimal price)
    {
        _game.Price = price;
        return this;
    }
}