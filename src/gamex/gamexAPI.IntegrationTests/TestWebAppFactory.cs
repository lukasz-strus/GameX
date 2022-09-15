using gamexEntities;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace gamexAPI.IntegrationTests;

public class TestWebAppFactory<TEntryPoint> : WebApplicationFactory<Program>
        where TEntryPoint : Program
{
    public ITestOutputHelper Output { get; set; }

    public TestWebAppFactory([NotNull] ITestOutputHelper output)
    {
        Output = output;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptions = services
                .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<GamexDbContext>));

            services.Remove(dbContextOptions);

            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

            services.AddMvc(option => option.Filters.Add(new FakeUserFilter()))
                    .AddApplicationPart(typeof(Program).Assembly);

            services
             .AddDbContext<GamexDbContext>(options => options.UseInMemoryDatabase("GamexDb"));
        });
    }
}