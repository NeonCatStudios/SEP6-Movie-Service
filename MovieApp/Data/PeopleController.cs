namespace MovieApp.Data;

public class PeopleController
{
    private MovieDBO _movieDbo;

    public PeopleController()
    {
        _movieDbo = new MovieDBO();
    }

    public List<Person> getPeople(int page, string name)
    {
        return _movieDbo.GetPeople(page: page, name: name).Result;
    }
}