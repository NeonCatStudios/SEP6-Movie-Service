using System.Collections.Specialized;
using Microsoft.CSharp.RuntimeBinder;
using MySql.Data.MySqlClient;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MovieApp.Data;

public class MovieDBO
{
    private String OMDBApiKey = "7463b8f9";
    private String OMDBUrl = "http://www.omdbapi.com/";
    static HttpClient client = new HttpClient();


    //To be deleted, database stuff for actors, has to be replaced with cloud functions
    private string databaseIP = "34.140.207.17";
    private string DatabaseName = "movie";
    private string usernameDatabase = "root";
    private string databasePassword = "hellokitty123";

    private MySqlConnection databaseConnection { get; set; }
    private string ConnectionString { get; set; }

    //-----------------------------------------------------------------------------------
    public MovieDBO()
    {
        //---------------to be removed---------------
        ConnectionString = string.Format("Server={0}; database={1}; UID={2}; password={3}", databaseIP,
            DatabaseName, usernameDatabase, databasePassword);
        databaseConnection = new MySqlConnection(ConnectionString);
        databaseConnection.Open();
        //----------------------------------------
        client.BaseAddress = new Uri(OMDBUrl);
    }

    public async Task<OMDBMovie> getMovieInfoJson(String movieId)
    {
        OMDBMovie movie = new OMDBMovie();
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("i", movieId);
        queryString.Add("apikey", OMDBApiKey);
        Console.WriteLine("About to wait lol");
        HttpResponseMessage response = client.GetAsync("?" + queryString.ToString()).Result;
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.StatusCode);
            String omdbMovieString = response.Content.ReadAsStringAsync().Result;
            movie = JsonSerializer.Deserialize<OMDBMovie>(omdbMovieString);
        }
        else
        {
        }

        return movie;
    }

    public async Task<List<Person>> getMovieActorsFromDB(OMDBMovie omdbMovie)
    {
        List<Person> persons = new List<Person>();
        /*String sqlList = "";
        int count = persons.Count;
        foreach (var i in persons)
        {
            if (count == 1)
            {
                sqlList += $"'{i.Name}'";
                count--;
            }
            else
            {
                sqlList += $"'{i.Name}', ";
                count--;
            }
       
            
        }*/
        foreach (var actor in omdbMovie.Actors.Split(", ").ToList())
        {
            String sql = $"SELECT * from people where people.name = '{actor}'";
          await using var cmd = new MySqlCommand(sql, databaseConnection);
           await using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Person person = new Person();
                if (Convert.ToInt32(rdr["id"]) > 0)
                {
                    person.name = Convert.ToString(rdr["name"]);
                    person.id = Convert.ToInt32(rdr["id"]);
                    person.birth = Convert.ToInt32(rdr["birth"]);
                    person.isDirector = false;
                    person.isInDB = true;
                }
                else
                {
                    person.name = actor;
                    person.isInDB = false;
                }

                persons.Add(person);
            }
            rdr.Close();
        }
        String sqlDirector = $"SELECT * from people where people.name = '{omdbMovie.Director}'";
       await using var cmd2 = new MySqlCommand(sqlDirector, databaseConnection);
       await using MySqlDataReader rdr2 = cmd2.ExecuteReader();
        while (rdr2.Read())
        {
            Person person = new Person();
            if (Convert.ToInt32(rdr2["id"]) == 0)
            {
                person.name = Convert.ToString(rdr2["name"]);
                person.id = Convert.ToInt32(rdr2["id"]);
                person.birth = Convert.ToInt16(rdr2["birth"]);
                person.isDirector = false;
                person.isInDB = true;
            }
            else
            {
                person.name = omdbMovie.Director;
                person.isInDB = false;
            }

            persons.Add(person);
            
        }
        rdr2.Close();
        return persons;
    }

    public async Task<PersonPageInfo> getPersonPageInfo(int personId)
    {
        using var httpClient = new HttpClient();
        HttpResponseMessage responseMessage = httpClient.GetAsync($"https://europe-west1-sep6-movie-service.cloudfunctions.net/getPersonInfo?personId={personId}").Result;

        PersonPageInfo result = new PersonPageInfo();
        if (responseMessage.IsSuccessStatusCode)
        {
            string res = responseMessage.Content.ReadAsStringAsync().Result;
            result = JsonSerializer.Deserialize<PersonPageInfo>(res);
        }
        return result;

    }

    public async Task<List<Movie>> GetMovies(int page)
    {
        int offset = (page - 1) * 50;
        Console.WriteLine(offset);
        using var httpClient = new HttpClient();
        HttpResponseMessage responseMessage = httpClient.GetAsync($"https://europe-west1-sep6-movie-service.cloudfunctions.net/getMovies?offset={offset}").Result;
        if (responseMessage.IsSuccessStatusCode)
        {
            Console.WriteLine(responseMessage);
            string result = responseMessage.Content.ReadAsStringAsync().Result;
            List<Movie> res = JsonSerializer.Deserialize<List<Movie>>(result);
            return res;
        }
        return null;
    }
}