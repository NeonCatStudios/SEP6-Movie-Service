using System.Collections.Specialized;
using System.Text.Json;
namespace MovieApp.Data;

public class MovieDBO
{
    private String OMDBApiKey = "7463b8f9";
    private String OMDBUrl = "http://www.omdbapi.com/";
    static HttpClient client = new HttpClient();
    public MovieDBO()
    {
        client.BaseAddress = new Uri(OMDBUrl);
    }
    public async Task<OMDBMovie> getMovieInfoJson(String movieId)
    {
        OMDBMovie movie = new OMDBMovie();
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("i", movieId);
        queryString.Add("apikey", OMDBApiKey);
        HttpResponseMessage response = client.GetAsync("?"+queryString.ToString()).Result;
        if (response.IsSuccessStatusCode)
        {
            String omdbMovieString =  response.Content.ReadAsStringAsync().Result;
            movie = JsonSerializer.Deserialize<OMDBMovie>(omdbMovieString);
        }
        else
        {
          Console.WriteLine(response.StatusCode);  
        }
        return movie;
    }
}