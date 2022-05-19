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

    public List<Person> checkActors(OMDBMovie omdbMovie)
    {
        return _movieDbo.getMovieActorsFromDB(omdbMovie).Result;
    }

    public PersonPageInfo getPersonPageInfo(int personId)
    {
        return _movieDbo.getPersonPageInfo(personId).Result;
    }
}