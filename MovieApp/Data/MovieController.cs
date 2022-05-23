namespace MovieApp.Data;

public class MovieController
{
    private MovieDBO _movieDbo;

    public MovieController()
    {
        _movieDbo = new MovieDBO();
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
}