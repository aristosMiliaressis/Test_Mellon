using ApiClient.DTO;
using ApiClient.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient
{
    public class Client
    {
        private Uri _baseUrl;
        private string _accessToken;

        private Client(Uri baseUrl, string accessToken)
        {
            _baseUrl = baseUrl;
            _accessToken = accessToken;
        }

        public static async Task<Client> CreateAsync(Uri baseUrl, string userName, string passWord)
        {
            HttpResponseMessage response = null;
            using (var handler = new HttpClientHandler())
            {
                handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(handler))
                {
                    var fullUrl = new Uri(baseUrl, "/api/api-token-auth/");
                    var request = new HttpRequestMessage(HttpMethod.Post, fullUrl);

                    var creds = new Dictionary<string, string>();
                    creds.Add("username", userName);
                    creds.Add("password", passWord);
                    request.Content = new FormUrlEncodedContent(creds);

                    try
                    {
                        response = await client.SendAsync(request);
                    }
                    catch (Exception ex)
                    {
                        throw new TransferErrorException(ex);
                    }
                }
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new HttpErrorResponseException("Unable to log in with provided credentials.", response.StatusCode);
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpErrorResponseException("Unexpected HTTP error occurred!", response.StatusCode);
            }

            var contentString = await response.Content.ReadAsStringAsync();

            dynamic contentObj = JsonConvert.DeserializeObject(contentString);

            return new Client(baseUrl, (string)contentObj.token);
        }

        #region Actors
        public async Task<List<Actor>> ListActorsAsync()
        {
            var fullUrl = new Uri(_baseUrl, "/api/actors/");

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Get, _accessToken, null);

            var contentString = await responseContent.ReadAsStringAsync();
            var actors = JsonConvert.DeserializeObject<List<Actor>>(contentString);

            return actors;
        }

        public async Task<Actor> GetActorAsync(int id)
        {
            var fullUrl = new Uri(_baseUrl, $"/api/actors/{id}");

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Get, _accessToken, null);

            var contentString = await responseContent.ReadAsStringAsync();
            var actor = JsonConvert.DeserializeObject<Actor>(contentString);

            return actor;
        }

        public async Task<Actor> CreateActorAsync(string name, DateTime dateOfBirth)
        {
            var fullUrl = new Uri(_baseUrl, "/api/actors/");

            var newActor = new
            {
                name = name,
                birthday = dateOfBirth.ToString("yyyy-MM-dd")
            };

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Post, _accessToken, newActor);

            var contentString = await responseContent.ReadAsStringAsync();
            var actor = JsonConvert.DeserializeObject<Actor>(contentString);

            return actor;
        }

        public async Task<Actor> UpdateActorAsync(int id, string name, DateTime dateOfBirth)
        {
            var fullUrl = new Uri(_baseUrl, $"/api/actors/{id}/");

            var newActor = new
            {
                name = name,
                birthday = dateOfBirth.ToString("yyyy-MM-dd")
            };

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Put, _accessToken, newActor);

            var contentString = await responseContent.ReadAsStringAsync();
            var actor = JsonConvert.DeserializeObject<Actor>(contentString);

            return actor;
        }

        public async Task DeleteActorAsync(int id)
        {
            var fullUrl = new Uri(_baseUrl, $"/api/actors/{id}/");

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Delete, _accessToken, null);
        }
        #endregion Actors

        #region Directors
        public async Task<List<Director>> ListDirectorsAsync()
        {
            var fullUrl = new Uri(_baseUrl, "/api/directors/");

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Get, _accessToken, null);

            var contentString = await responseContent.ReadAsStringAsync();
            var directors = JsonConvert.DeserializeObject<List<Director>>(contentString);

            return directors;
        }

        public async Task<Director> GetDirectorAsync(int id)
        {
            var fullUrl = new Uri(_baseUrl, $"/api/directors/{id}");

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Get, _accessToken, null);

            var contentString = await responseContent.ReadAsStringAsync();
            var director = JsonConvert.DeserializeObject<Director>(contentString);

            return director;
        }

        public async Task<Director> CreateDirectorAsync(string name, DateTime dateOfBirth)
        {
            var fullUrl = new Uri(_baseUrl, "/api/directors/");

            var newDirector = new
            {
                name = name,
                birthday = dateOfBirth.ToString("yyyy-MM-dd")
            };

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Post, _accessToken, newDirector);

            var contentString = await responseContent.ReadAsStringAsync();
            var director = JsonConvert.DeserializeObject<Director>(contentString);

            return director;
        }

        public async Task<Director> UpdateDirectorAsync(int id, string name, DateTime dateOfBirth)
        {
            var fullUrl = new Uri(_baseUrl, $"/api/directors/{id}/");

            var newDirector = new
            {
                name = name,
                birthday = dateOfBirth.ToString("yyyy-MM-dd")
            };

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Put, _accessToken, newDirector);

            var contentString = await responseContent.ReadAsStringAsync();
            var director = JsonConvert.DeserializeObject<Director>(contentString);

            return director;
        }

        public async Task DeleteDirectorAsync(int id)
        {
            var fullUrl = new Uri(_baseUrl, $"/api/directors/{id}/");

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Delete, _accessToken, null);
        }
        #endregion Directors

        #region Movies
        public async Task<List<Movie>> ListMoviesAsync()
        {
            var fullUrl = new Uri(_baseUrl, "/api/movies/");

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Get, _accessToken, null);

            var contentString = await responseContent.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movie>>(contentString);

            return movies;
        }

        public async Task<Movie> GetMovieAsync(int id)
        {
            var fullUrl = new Uri(_baseUrl, $"/api/movies/{id}");

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Get, _accessToken, null);

            var contentString = await responseContent.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(contentString);

            return movie;
        }

        public async Task<Movie> CreateMovieAsync(string name, DateTime year, int director, List<int> actors)
        {
            var fullUrl = new Uri(_baseUrl, "/api/movies/");

            var newMovie = new
            {
                name = name,
                year = year.Year,
                director = director,
                actors = actors
            };

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Post, _accessToken, newMovie);

            var contentString = await responseContent.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(contentString);

            return movie;
        }

        public async Task<Movie> UpdateMovieAsync(int id, string name, DateTime year, int director, List<int> actors)
        {
            var fullUrl = new Uri(_baseUrl, $"/api/movies/{id}/");

            var newMovie = new
            {
                name = name,
                year = year.Year,
                director = director,
                actors = actors
            };

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Put, _accessToken, newMovie);

            var contentString = await responseContent.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(contentString);

            return movie;
        }

        public async Task DeleteMovieAsync(int id)
        {
            var fullUrl = new Uri(_baseUrl, $"/api/movies/{id}/");

            var responseContent = await HttpHelper.SendRequestAsync(fullUrl, HttpMethod.Delete, _accessToken, null);
        }
        #endregion Movies
    }
}
