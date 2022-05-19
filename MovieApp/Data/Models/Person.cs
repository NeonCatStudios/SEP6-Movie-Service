using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace MovieApp.Data;

[Serializable]
public class Person
{
    [NotNull] public int Id { get; set; }
    [NotNull] public string Name { get; set; }
    [AllowNull] public BigInteger BirthYear { get; set; }
    [AllowNull] public bool isDirector { get; set; }
    [AllowNull] public bool isInDB { get; set; }
}