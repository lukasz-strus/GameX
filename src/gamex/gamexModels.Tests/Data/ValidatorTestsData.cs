namespace gamexModels.Tests.Data;

public static class ValidatorTestsData
{
    public static IEnumerable<object[]> GetSampleValidData()
    {
        var list = new List<GetAllQuery>()
        {
            new GetAllQuery()
            {
                PageNumber = 1,
                PageSize = 5
            },
            new GetAllQuery()
            {
                PageNumber = 2,
                PageSize = 10
            },
            new GetAllQuery()
            {
                PageNumber = 22,
                PageSize = 15
            },
            new GetAllQuery()
            {
                PageNumber = 22,
                PageSize = 5,
                SortBy = nameof(Game.Name)
            },
            new GetAllQuery()
            {
                PageNumber = 22,
                PageSize = 15,
                SortBy = nameof(Game.Price)
            },
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleInvalidData()
    {
        var list = new List<GetAllQuery>()
        {
            new GetAllQuery()
            {
                PageNumber = 0,
                PageSize = 5
            },
            new GetAllQuery()
            {
                PageNumber = 2,
                PageSize = 13
            },
            new GetAllQuery()
            {
                PageNumber = 22,
                PageSize = 5,
                SortBy = nameof(User.PasswordHash)
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleValidRegisterData()
    {
        var list = new List<RegisterUserDto>()
        {
            new RegisterUserDto()
            {
                Login = "Test123",
                Email = "test123@test.com",
                ConfirmEmail = "test123@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleInvalidRegisterData()
    {
        var list = new List<RegisterUserDto>()
        {
            new RegisterUserDto()
            {
                Login = "test1",
                Email = "test23@test.com",
                ConfirmEmail = "test23@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            },
            new RegisterUserDto()
            {
                Login = "test23",
                Email = "test1@test.com",
                ConfirmEmail = "test1@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            },
            new RegisterUserDto()
            {
                Login = "test23",
                Email = "test23@test.com",
                ConfirmEmail = "test1@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            },
            new RegisterUserDto()
            {
                Login = "test23",
                Email = "test23@test.com",
                ConfirmEmail = "test23@test.com",
                Password = "password123",
                ConfirmPassword = "password1"
            },
            new RegisterUserDto()
            {
                Login = "test23",
                Email = "test23@test.com",
                ConfirmEmail = "test23@test.com",
                Password = "pas",
                ConfirmPassword = "pas"
            },
            new RegisterUserDto()
            {
                Login = "test23",
                Email = "test23",
                ConfirmEmail = "test23",
                Password = "password123",
                ConfirmPassword = "password123"
            },
            new RegisterUserDto()
            {
                Email = "test23@test.com",
                ConfirmEmail = "test23@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            },
            new RegisterUserDto()
            {
                Login = "test23",
                Email = "test23@test.com",
                ConfirmEmail = "test23@test.com",
                ConfirmPassword = "password123"
            },
            new RegisterUserDto()
            {
                Login = "test23",
                ConfirmEmail = "test23@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            },
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleValidCreateGameData()
    {
        var list = new List<CreateGameDto>()
        {
            new CreateGameDto()
            {
                Name = "test23",
                Description = "test23",
                Price = 100m
            },
            new CreateGameDto()
            {
                Name = "test234",
                Price = 100m
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleInvalidCreateGameData()
    {
        var list = new List<CreateGameDto>()
        {
            new CreateGameDto()
            {
                Name = "test1",
                Description = "test23",
                Price = 100m
            },
            new CreateGameDto()
            {
                Description = "test23",
                Price = 100m
            },
            new CreateGameDto()
            {
                Name = "test23",
                Description = "test23"
            },
            new CreateGameDto()
            {
                Name = "test23",
                Description = "test23",
                Price = -100
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleValidUpdateUserData()
    {
        var list = new List<UpdateUserDto>()
        {
            new UpdateUserDto()
            {
                Login = "test56",
                Email = "test56@test.com",
                ConfirmEmail = "test56@test.com",
                Password = "password123",
                ConfirmPassword = "password123",
                RoleId = 1
            },
            new UpdateUserDto()
            {
                Login = "test56"
            },
            new UpdateUserDto()
            {
                RoleId = 2
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleInvalidUpdateUserData()
    {
        var list = new List<UpdateUserDto>()
        {
            new UpdateUserDto()
            {
                Login = "test5",
                Email = "test23@test.com",
                ConfirmEmail = "test23@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            },
            new UpdateUserDto()
            {
                Login = "test23",
                Email = "test6@test.com",
                ConfirmEmail = "test6@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            },
            new UpdateUserDto()
            {
                Login = "test23",
                Email = "test23@test.com",
                ConfirmEmail = "test1@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            },
            new UpdateUserDto()
            {
                Login = "test23",
                Email = "test23@test.com",
                ConfirmEmail = "test23@test.com",
                Password = "password123",
                ConfirmPassword = "password1"
            },
            new UpdateUserDto()
            {
                Login = "test23",
                Email = "test23@test.com",
                ConfirmEmail = "test23@test.com",
                Password = "pas",
                ConfirmPassword = "pas"
            },
            new UpdateUserDto()
            {
                Login = "test23",
                Email = "test23",
                ConfirmEmail = "test23",
                Password = "password123",
                ConfirmPassword = "password123"
            },
            new UpdateUserDto()
            {
                Login = "test23",
                Email = "test23@test.com",
                ConfirmEmail = "test23@test.com",
                ConfirmPassword = "password123"
            },
            new UpdateUserDto()
            {
                Login = "test23",
                ConfirmEmail = "test23@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            },
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleValidUpdateGameData()
    {
        var list = new List<UpdateGameDto>()
        {
            new UpdateGameDto()
            {
                Name = "test50",
                Description = "test50",
                Price = 100m
            },
            new UpdateGameDto()
            {
                Name = "test50"
            },
            new UpdateGameDto()
            {
                Price = 100m
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleInvalidUpdateGameData()
    {
        var list = new List<UpdateGameDto>()
        {
            new UpdateGameDto()
            {
                Name = "test5",
                Description = "test5",
                Price = 100m
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleValidPasswordData()
    {
        var list = new List<UserPasswordDto>()
        {
            new UserPasswordDto()
            {
                Password = "passowrd123",
                ConfirmPassword = "passowrd123"
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleInvalidPasswordData()
    {
        var list = new List<UserPasswordDto>()
        {
            new UserPasswordDto()
            {
                Password = "passowrd123",
                ConfirmPassword = "passowrd1"
            },
            new UserPasswordDto()
            {
                Password = "pas",
                ConfirmPassword = "pas"
            }
        };

        return list.Select(q => new object[] { q });
    }
}