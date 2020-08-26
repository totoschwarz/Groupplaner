namespace Gruppenplaner.Grouping
{
  public class PerformanceHomTextRating : HomogeneousTextRating
  {
    public PerformanceHomTextRating(Gruppenplaner.Grouping.Grouping gi)
      : base(gi, (IMemberValueGetter) new PerformanceGetter())
    {
    }
  }
}
