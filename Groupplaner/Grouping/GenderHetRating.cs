using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class GenderHetRating : HeterogeneousRating
  {
    public GenderHetRating(Gruppenplaner.Grouping.Grouping gi)
      : base(gi)
    {
    }

    protected override string GetValue(Member m) => m.Gender;
  }
}
