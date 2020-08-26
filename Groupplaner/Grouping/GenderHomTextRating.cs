namespace Gruppenplaner.Grouping
{
  public class GenderHomTextRating : HomogeneousTextRating
  {
    public GenderHomTextRating(Gruppenplaner.Grouping.Grouping gi)
      : base(gi, (IMemberValueGetter) new GenderGetter())
    {
    }
  }
}
