using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace MovieApp.Data;

[Serializable]
public class Movie
{
    [NotNull]
    public int Id { get; set; }
    [AllowNull]
    public string Title { get; set; }
    [AllowNull]
    public int Year;
    [AllowNull]
    public Person Actor { get; set; }
    [AllowNull]
    public Person Director { get; set; }
    
}