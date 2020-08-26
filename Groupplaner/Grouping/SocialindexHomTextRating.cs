namespace Gruppenplaner.Grouping
{
  public class SocialindexHomTextRating : HomogeneousTextRating
  {
    public SocialindexHomTextRating(Gruppenplaner.Grouping.Grouping gi)
      : base(gi, (IMemberValueGetter) new SocialindexGetter())
    {
    }
  }
}
