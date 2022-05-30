using System.Diagnostics.CodeAnalysis;

namespace MovieApp.Data;

[Serializable]
public class AvgRating
{
    public double avgRating { get; set; }
    [AllowNull]
    public int person_id { get; set; }
}