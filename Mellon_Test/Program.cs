using ApiClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mellon_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", true, true)
                  .Build();

            var baseUrl = config["BaseUrl"];

            TestApiLibAsync(baseUrl).Wait();
        }

        static async Task TestApiLibAsync(string baseUrl)
        {
            var cli = await Client.CreateAsync(new Uri(baseUrl), "TestUser", "ZfuzpbZ8Mo4");

            var newActor = await cli.CreateActorAsync("testActor", new DateTime(1996, 10, 10));
            var newActor2 = await cli.GetActorAsync(newActor.Id);
            var updatedActor = await cli.UpdateActorAsync(newActor.Id, "updatedActor", new DateTime(1996, 8, 8));
            await cli.DeleteActorAsync(updatedActor.Id);
            var actors = await cli.ListActorsAsync();

            var newDirector = await cli.CreateDirectorAsync("testDirector", new DateTime(1996, 10, 10));
            var updatedDirector = await cli.UpdateDirectorAsync(newDirector.Id, "updatedDirector", new DateTime(1996, 8, 8));
            await cli.DeleteDirectorAsync(updatedDirector.Id);
            var directors = await cli.ListDirectorsAsync();


            var selectedActors = actors
                                    .Select(a => a.Id)
                                    .Take(2)
                                    .ToList();

            var selectedDirector = directors.First().Id;

            var newMovie = await cli.CreateMovieAsync("testMovie", new DateTime(1996, 10, 10), selectedDirector, selectedActors);
            var updatedMovie = await cli.UpdateMovieAsync(newMovie.Id, "updatedMovie", new DateTime(1996, 8, 8), selectedDirector, selectedActors);
            await cli.DeleteMovieAsync(updatedMovie.Id);
            var movies = await cli.ListMoviesAsync();
        }
    }
}
