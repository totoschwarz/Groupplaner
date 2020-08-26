using Basics;
using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class PerformanceHomScaledRating : HomogeneousScaledRating
  {
    public PerformanceHomScaledRating(Gruppenplaner.Grouping.Grouping gi): base(gi){}

    protected override double GetValue(Member m) => Functions.StrToDouble(m.Performance);
  }
}
