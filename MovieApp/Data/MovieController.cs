namespace MovieApp.Data;

public class MovieController
{
    private MovieDBO _movieDbo;

    public MovieController()
    {
        _movieDbo = new MovieDBO();
    }

    public async Task<Dictionary<String, String>> getMovies(String movieId)
    {
        Dictionary<String, String> movies = _movieDbo.getMovieInfo(movieId);
        return movies;
    }
}