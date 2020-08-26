using Basics;
using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class SocialindexHomScaledRating : HomogeneousScaledRating
  {
    public SocialindexHomScaledRating(Gruppenplaner.Grouping.Grouping gi)
      : base(gi)
    {
    }

    protected override double GetValue(Member m) => Functions.StrToDouble(m.Socialindex);
  }
}
