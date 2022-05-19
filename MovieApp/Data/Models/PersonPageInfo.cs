namespace MovieApp.Data;

[Serializable] public class PersonPageInfo
{
    public PersonPageInfo()
    {
        StarredMovies = new List<Movie>();
        DirectedMovies = new List<Movie>();

    }
    
    public Person person;
    public List<Movie> StarredMovies;
    public List<Movie> DirectedMovies;

}