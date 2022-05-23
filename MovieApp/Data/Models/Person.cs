using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace MovieApp.Data;

[Serializable]
public class Person
{
    [NotNull] public int id { get; set; }
    [NotNull] public string name { get; set; }
    [AllowNull] public int? birth { get; set; }
   // [AllowNull] public bool isDirector { get; set; }
    
    [AllowNull] public bool isInDB = true;
}