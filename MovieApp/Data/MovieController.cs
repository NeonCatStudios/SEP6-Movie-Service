using Microsoft.JSInterop;
using MovieApp.Data.Models.AuthenticationModels;
using Newtonsoft.Json;

namespace MovieApp.Data;

public class MovieController
{
    private MovieDBO _movieDbo;
    private readonly IJSRuntime jsRuntime;
    private List<Movie> _favList;

    public MovieController()
    {
        _movieDbo = new MovieDBO();
        _favList = new List<Movie>();
    }

    public async Task<List<Movie>> GetFavListByUserId(string userId)
    {
        return await _movieDbo.GetFavListByUserId(userId);
    }

    public async Task<OMDBMovie> getMovies(String movieId)
    {
        return await _movieDbo.getMovieInfoJson(movieId);
    }

    public List<Person> checkActors(String movieId)
    {
        return _movieDbo.getMovieActorsFromDB(movieId).Result;
    }

    public PersonPageInfo getPersonPageInfo(int personId)
    {
        return _movieDbo.getPersonPageInfo(personId).Result;
    }

    public List<Movie> getMovies(int page, string title) 
    {
        return _movieDbo.GetMovies(page: page, title: title).Result;
    }

    public double getAvgRating(int personId)
    {
        return _movieDbo.getRatingForFilms(personId).Result;
    }

    public void RemoveFromFav(CachedUser user, int movieId)
    {
        _movieDbo.RemoveFromFav(user, movieId);
    }

    public async Task<bool> AddToFav(CachedUser user, int movieId)
    {
        return await _movieDbo.AddToFav(user, movieId);
    }
}