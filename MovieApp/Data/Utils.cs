namespace MovieApp.Data;

public static class Utils
{
    public static string YearFilter(int? year)
    {
        return year > 0 ? $"({year})" : "";
    }
}