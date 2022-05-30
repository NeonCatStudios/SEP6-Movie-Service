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
        HttpResponseMessage response = client.GetAsync("?" + queryString.ToString()).Result;
        if (response.IsSuccessStatusCode)
        {
            String omdbMovieString = await response.Content.ReadAsStringAsync();
            movie = JsonSerializer.Deserialize<OMDBMovie>(omdbMovieString);
        }
        else
        {
        }

        return movie;
    }

    public async Task<List<Person>> getMovieActorsFromDB(String movieId)
    {
        using var httpClient = new HttpClient();
        movieId = movieId.Remove(0, 2);
        HttpResponseMessage responseMessage = httpClient.GetAsync($"https://europe-west1-sep6-movie-service.cloudfunctions.net/getActorsForMovie?movieId={movieId}").Result;

        List<Person> actorsList = new List<Person>();
        if (responseMessage.IsSuccessStatusCode)
        {
            string res = responseMessage.Content.ReadAsStringAsync().Result;
            actorsList = JsonSerializer.Deserialize<List<Person>>(res);
        }
        else
        {
            Console.WriteLine(responseMessage.StatusCode);
        }
        return actorsList;
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

    public async Task<double> getRatingForFilms(int personId)
    {
        using var httpClient = new HttpClient();
        HttpResponseMessage responseMessage = httpClient.GetAsync($"https://europe-west1-sep6-movie-service.cloudfunctions.net/getAvgRating?actorId={personId}").Result;

        AvgRating? result = new AvgRating();
        if (responseMessage.IsSuccessStatusCode)
        {
            string res = responseMessage.Content.ReadAsStringAsync().Result;
            if (res.Length > 0)
            {
                result = JsonSerializer.Deserialize<AvgRating>(res);
            }
            Console.WriteLine(result);
        }
        return result.avgRating;
    }

    public async Task<List<Movie>> GetMovies(int page, string title)
    {
        if (page <= 0)
        {
            page = 1;
        }
        int offset = (page - 1) * 50;
        
        string parameterString = $"offset={offset}";

        if (title != null)
        {
            parameterString += $"&title={title}";
        }

        using var httpClient = new HttpClient();
        HttpResponseMessage responseMessage = httpClient.GetAsync($"https://europe-west1-sep6-movie-service.cloudfunctions.net/getMovies?{parameterString}").Result;
        
        if (responseMessage.IsSuccessStatusCode)
        {
            Console.WriteLine(responseMessage);
            string result = responseMessage.Content.ReadAsStringAsync().Result;
            List<Movie> res = JsonSerializer.Deserialize<List<Movie>>(result);
            return res;
        }
        return null;
    }
    public async Task<List<Person>> GetPeople(int page)
    {
        if (page <= 0)
        {
            page = 1;
        }
        int offset = (page - 1) * 50;
        
        string parameterString = $"offset={offset}";
        
        using var httpClient = new HttpClient();
        HttpResponseMessage responseMessage = httpClient.GetAsync($"https://europe-west1-sep6-movie-service.cloudfunctions.net/getPeople-1?{parameterString}").Result;
        
        if (responseMessage.IsSuccessStatusCode)
        {
            Console.WriteLine(responseMessage);
            string result = responseMessage.Content.ReadAsStringAsync().Result;
            List<Person> res = JsonSerializer.Deserialize<List<Person>>(result);
            return res;
        }
        return null;
    }
}