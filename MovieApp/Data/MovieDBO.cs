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

    public String getMovieInfoJson(String movieId)
    {
      
        
        return movieDictionary;
    }
}