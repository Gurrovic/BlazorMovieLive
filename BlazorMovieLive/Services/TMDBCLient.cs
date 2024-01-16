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

        public Task<PopularMoviePagedResponse?> GetMoviesAsync(string category, int page = 1)
        {
            if (page < 1) page = 1;
            if (page > 500) page = 500;
            return _httpClient.GetFromJsonAsync<PopularMoviePagedResponse>($"movie/{category}?page={page}");
        }

        public Task<MovieDetails?> GetMovieDetailsAsync(int id)
        {           
            return _httpClient.GetFromJsonAsync<MovieDetails>($"movie/{id}");
        }

        public async Task<PopularMoviePagedResponse?> GetMoviesByIdsAsync(List<int> movieIds)
        {
            var movies = new List<PopularMovie>();

            foreach (var id in movieIds)
            {
                var movieDetails = await GetMovieDetailsAsync(id);
                if (movieDetails != null)
                {
                    movies.Add(ConvertToMovieType(movieDetails));
                }
            }
            
            var response = new PopularMoviePagedResponse
            {
                Page = 1, // Since this is a custom list, Page can be set to 1
                TotalResults = movies.Count,               
                TotalPages = 1, // Assuming all results fit in one page
                Results = movies
            };

            return response;
        }

        private PopularMovie ConvertToMovieType(MovieDetails movieDetails)
        {
            var popularMovie = new PopularMovie
            {
                Id = movieDetails.Id,
                Title = movieDetails.Title,
                Adult = movieDetails.Adult,
                BackdropPath = movieDetails.BackdropPath,
                OriginalLanguage = movieDetails.OriginalLanguage,
                OriginalTitle = movieDetails.OriginalTitle,
                Overview = movieDetails.Overview,
                Popularity = movieDetails.Popularity,
                PosterPath = movieDetails.PosterPath,
                ReleaseDate = movieDetails.ReleaseDate,
                Video = movieDetails.Video,
                VoteAverage = movieDetails.VoteAverage,
                VoteCount = movieDetails.VoteCount
            };

            // Convert MovieDetails Genres to PopularMovie GenreIds
            if (movieDetails.Genres != null && movieDetails.Genres.Length > 0)
            {
                popularMovie.GenreIds = movieDetails.Genres.Select(genre => genre.Id).ToArray();
            }
            else
            {
                popularMovie.GenreIds = new int[0]; // Empty array if no genres are available
            }

            return popularMovie;
        }
    }
}

