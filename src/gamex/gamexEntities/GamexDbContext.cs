using Microsoft.EntityFrameworkCore;

namespace gamexEntities;

public class GamexDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<GameSerial> GameSerials { get; set; }
    public DbSet<Image> Images { get; set; }

    public GamexDbContext(DbContextOptions<GamexDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Games

        modelBuilder.Entity<Game>()
            .ToTable("Games");

        modelBuilder.Entity<Game>()
            .HasKey(g => g.Id);

        modelBuilder.Entity<Game>()
            .Property(g => g.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Game>()
            .Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Game>()
            .Property(g => g.Description)
            .HasMaxLength(1000);

        modelBuilder.Entity<Game>()
            .Property(g => g.Price)
            .HasPrecision(9, 2)
            .IsRequired();

        #endregion Games

        #region User

        modelBuilder.Entity<User>()
            .ToTable("Users");

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<User>()
            .Property(u => u.Login)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<User>()
            .Property(u => u.Total)
            .HasPrecision(9, 2)
            .HasDefaultValue(0);

        #endregion User

        #region Role

        modelBuilder.Entity<Role>()
            .ToTable("Roles");

        modelBuilder.Entity<Role>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .IsRequired();

        #endregion Role

        #region GameSerial

        modelBuilder.Entity<GameSerial>()
            .ToTable("GameSerials");

        #endregion GameSerial

        #region Image

        modelBuilder.Entity<Image>()
            .ToTable("Images");

        modelBuilder.Entity<Image>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Image>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Image>()
            .Property(i => i.ImageStream)
            .IsRequired();

        modelBuilder.Entity<Image>()
            .Property(i => i.GameId)
            .IsRequired();

        modelBuilder.Entity<Image>()
            .Property(i => i.Extension)
            .IsRequired();

        #endregion Image
    }
}