using Basics;
using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class GenderHomScaledRating : HomogeneousScaledRating
  {
    public GenderHomScaledRating(Gruppenplaner.Grouping.Grouping gi)
      : base(gi)
    {
    }

    protected override double GetValue(Member m) => Functions.StrToDouble(m.Gender);
  }
}
