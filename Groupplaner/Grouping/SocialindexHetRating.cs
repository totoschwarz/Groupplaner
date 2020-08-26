using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class SocialindexHetRating : HeterogeneousRating
  {
    public SocialindexHetRating(Gruppenplaner.Grouping.Grouping gi)
      : base(gi)
    {
    }

    protected override string GetValue(Member m) => m.Socialindex;
  }
}
