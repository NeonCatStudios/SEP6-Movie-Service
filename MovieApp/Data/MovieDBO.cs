namespace MovieApp.Data;
using MySql.Data.MySqlClient;
    
public class MovieDBO
{
    private string databaseIP = "34.140.207.17";
    private string DatabaseName = "movie";
    private string usernameDatabase = "root";
    private string databasePassword = "hellokitty123";
    private MySqlConnection databaseConnection { get; set; }

    public MovieDBO()
    {
        string connectionString = string.Format("Server={0}; database={1}; UID={2}; password={3}", databaseIP, DatabaseName, usernameDatabase, databasePassword);
        databaseConnection = new MySqlConnection(connectionString);
        databaseConnection.Open();
        Console.WriteLine($"MySQL version : {databaseConnection.ServerVersion}");
    }

    public Dictionary<String, String> getMovieInfo(String movieId)
    {
        Dictionary<string, string> movieDictionary =
            new Dictionary<string, string>();
        string sql = $"SELECT * FROM movies m" +
                     $" JOIN directors d on d.movie_id = m.id" +
                     $" JOIN people p on p.id = d.person_id" +
                     $" JOIN ratings r on r.movie_id = m.id" +
                     $" where m.id={movieId}";
        using var cmd = new MySqlCommand(sql, databaseConnection);
        using MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            String movieTitle = Convert.ToString(rdr["title"]);
            String movieYear = Convert.ToString(rdr["year"]);
            String movieDirector = Convert.ToString(rdr["name"]);
            String movieRating = Convert.ToString(rdr["rating"]);
            String movieVotes = Convert.ToString(rdr["votes"]);
            
            movieDictionary.Add("Title", movieTitle);
            movieDictionary.Add("Year", movieYear);
            movieDictionary.Add("Director", movieDirector);
            movieDictionary.Add("Rating", movieRating);
            movieDictionary.Add("Votes", movieVotes);
            
            Console.WriteLine(Convert.ToString(rdr["title"]));
        }
        
        return movieDictionary;
    }
}