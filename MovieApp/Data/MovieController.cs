namespace MovieApp.Data;

public class MovieController
{
    private MovieDBO _movieDbo;

    public MovieController()
    {
        _movieDbo = new MovieDBO();
    }

    public OMDBMovie getMovies(String movieId)
    {
        return _movieDbo.getMovieInfoJson(movieId).Result;
    }
}