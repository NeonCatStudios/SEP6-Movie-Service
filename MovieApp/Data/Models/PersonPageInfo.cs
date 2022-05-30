namespace MovieApp.Data;

[Serializable] public class PersonPageInfo
{
    public List<Movie> starredMovies { get; set; }
    public List<Movie> directedMovies { get; set; }
    public Person person { get; set; }
    
    public PersonPageInfo()
    {
        starredMovies = new List<Movie>();
        directedMovies = new List<Movie>();
    }

}