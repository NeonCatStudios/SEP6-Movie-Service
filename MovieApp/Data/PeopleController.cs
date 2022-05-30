namespace MovieApp.Data;

public class PeopleController
{
    private MovieDBO _movieDbo;

    public PeopleController()
    {
        _movieDbo = new MovieDBO();
    }

    public List<Person> getPeople(int page)
    {
        return _movieDbo.GetPeople(page: page).Result;
    }
}