using AutoMapper;
using gamexAPI.Entities;
using gamexModels;

namespace gamexAPI;

public class GamexMappingProfile : Profile
{
    public GamexMappingProfile()
    {
        CreateMap<Game, GameDto>();

        CreateMap<User, UserDto>();

        CreateMap<CreateGameDto, Game>();

        CreateMap<RegisterUserDto, User>();

        CreateMap<UserPasswordDto, User>();

        CreateMap<UpdateGameDto, GameDto>();

        CreateMap<GameSerialDto, GameSerial>();

        CreateMap<ImageDto, Image>();
    }
}