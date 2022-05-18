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
        
        return movies;
    }
}