using System.Collections.Specialized;
using MovieApp.Data.Models.AuthenticationModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MovieApp.Data;

public class MovieDBO
{
    private String OMDBApiKey = "7463b8f9";
    private String OMDBUrl = "http://www.omdbapi.com/";

    public MovieDBO()
    {
    }

    public async Task<OMDBMovie> getMovieInfoJson(String movieId)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(OMDBUrl);
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
    public async Task<List<Person>> GetPeople(int page, string name)
    {
        if (page <= 0)
        {
            page = 1;
        }
        int offset = (page - 1) * 50;
        
        string parameterString = $"offset={offset}";
        
        if (name != "" && name != null)
        {
            parameterString += "&name="+name;
        }
        
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

    public async Task<List<Movie>> GetFavListByUserId(string userId)
    {
        var url = "https://europe-west1-sep6-movie-service.cloudfunctions.net/getFavListForUser";
        using var httpClient = new HttpClient();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("user_id", userId);
        HttpResponseMessage responseMessage = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));

        List<Movie> res = new List<Movie>();
        
        if (responseMessage.IsSuccessStatusCode)
        {
            string result = responseMessage.Content.ReadAsStringAsync().Result;
            res = JsonSerializer.Deserialize<List<Movie>>(result);
        }

        return res;
    }

    public async Task<bool> RemoveFromFav(CachedUser user, int movieId)
    {
        string url = "https://europe-west1-sep6-movie-service.cloudfunctions.net/removeFavListForUser";
        
        using var httpClient = new HttpClient();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("user_id", user.UserId);
        data.Add("movie_id", movieId+"");
        data.Add("token", user.Token);
        HttpResponseMessage responseMessage = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));
        if (responseMessage.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            throw new Exception("AddToFav went wrong");
        }
    }


    public async Task<bool> AddToFav(CachedUser user, int movieId)
    {
        string url = "https://europe-west1-sep6-movie-service.cloudfunctions.net/addFavListForUser";
        
        using var httpClient = new HttpClient();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("user_id", user.UserId);
        data.Add("movie_id", movieId+"");
        data.Add("token", user.Token);
        HttpResponseMessage responseMessage = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));
        if (responseMessage.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            throw new Exception("AddToFav went wrong");
        }
    }
}