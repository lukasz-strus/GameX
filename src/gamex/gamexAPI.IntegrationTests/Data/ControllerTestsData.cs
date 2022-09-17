using gamexModels;

namespace gamexAPI.IntegrationTests.Data;

public static class ControllerTestsData
{
    public static IEnumerable<object[]> GetSampleValidGameData()
    {
        var list = new List<CreateGameDto>()
        {
            new CreateGameDto()
            {
                Name = "Test",
                Description = "Test Description",
                Price = 100m
            },
            new CreateGameDto()
            {
                Name = "Test1",
                Price = 100m
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleInvalidGameData()
    {
        var list = new List<CreateGameDto>()
        {
            new CreateGameDto()
            {
                Name = "",
                Description = "Test Description",
                Price = 100m
            },
            new CreateGameDto()
            {
                Name = "Test Name",
                Description = "Test Description"
            },
            new CreateGameDto()
            {
                Description = "Test Description"
            }
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleValidQueryParams()
    {
        var list = new List<string>()
        {
            "pageSize=5&pageNumber=1",
            "pageSize=10&pageNumber=2",
            "pageSize=15&pageNumber=3"
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleInvalidQueryParams()
    {
        var list = new List<string>()
        {
            "pageSize=1&pageNumber=1",
            "pageSize=-1&pageNumber=1",
            "pageSize=5&pageNumber=-1",
            "pageSize=5&pageNumber=0",
            "pageSize=0&pageNumber=1"
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleValidUpdateGameData()
    {
        var list = new List<UpdateGameDto>()
        {
            new UpdateGameDto()
            {
                Description = "Test Description",
                Price = 100
            },
            new UpdateGameDto()
            {
                Name = "Test 2",
                Description = "Test Description"
            },
            new UpdateGameDto()
            {
                Name = "Test 1",
                Price = 120
            }
        };
        return list.Select(q => new object[] { q });
    }
}