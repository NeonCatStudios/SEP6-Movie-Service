namespace MovieApp.Data;

[Serializable]
public class FavList
{
    public List<Movie> favorites { get; set; }

    public FavList(List<Movie> favorites)
    {
        this.favorites = favorites;
    }
    
    public bool contains(int movieId)
    {
        foreach(Movie movie in favorites)
        {
            if (movie.id == movieId)
            {
                return true;
            }
        }
        return false;
    }

}