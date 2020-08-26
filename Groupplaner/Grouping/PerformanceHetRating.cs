using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class PerformanceHetRating : HeterogeneousRating
  {
    public PerformanceHetRating(Gruppenplaner.Grouping.Grouping gi)
      : base(gi)
    {
    }

    protected override string GetValue(Member m) => m.Performance;
  }
}
