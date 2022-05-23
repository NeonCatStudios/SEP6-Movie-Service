using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace MovieApp.Data;

[Serializable]
public class Movie
{
    [NotNull] public int id { get; set; }
    [AllowNull] public string title { get; set; }
    [AllowNull] public int year { get; set; }
    
    [AllowNull] public float rating { get; set; }
    [AllowNull] public int votes { get; set; }
    
    [AllowNull] public Person Actor { get; set; }
    [AllowNull] public Person Director { get; set; }
    
}