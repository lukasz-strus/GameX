using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using gamexModelsDto;

namespace gamexDesktopApp.Models;

public class GamexMappingProfile : Profile
{
    public GamexMappingProfile()
    {
        CreateMap<Game, GameDto>();

        CreateMap<User, UserDto>();

        CreateMap<List<Game>, List<GameDto>>();
    }
}