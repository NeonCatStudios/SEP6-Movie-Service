namespace MovieApp.Data;

public static class Utils
{
    public static string YearFilter(int? year)
    {
        return year > 0 ? $"({year})" : "";
    }
    public static string BirthFilter(int? birth)
    {
        return birth > 0 ? $"({birth})" : "";
    }
}