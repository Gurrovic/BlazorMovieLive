using BlazorMovieLive.Client.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlazorMovieLive.Client.Services
{
    public class TMDBCLient
    {
        private readonly HttpClient _httpClient;        
        private readonly string _bearerToken;

        public TMDBCLient(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _httpClient = clientFactory.CreateClient("ExternalApi");

            _bearerToken = config["TMDBBearerToken"] ?? throw new Exception("TMDBBearerToken not found!");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
        }

        //public TMDBCLient(HttpClient httpClient, IConfiguration config) 
        //{
        //    _httpClient = httpClient;

        //    _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
        //    _httpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));

        //    _bearerToken = config["TMDBBearerToken"] ?? throw new Exception("TMDBBearerToken not found!");
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
        //}

        public Task<PopularMoviePagedResponse?> GetPopularMoviesAsync(int page = 1)
        {
            if (page < 1) page = 1;
            if (page > 500) page = 500;
            return _httpClient.GetFromJsonAsync<PopularMoviePagedResponse>($"movie/popular?page={page}");
        }        

        public Task<MovieDetails?> GetMovieDetailsAsync(int id)
        {           
            return _httpClient.GetFromJsonAsync<MovieDetails>($"movie/{id}");
        }
    }
}
