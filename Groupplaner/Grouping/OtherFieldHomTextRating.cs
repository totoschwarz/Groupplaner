namespace Gruppenplaner.Grouping
{
  public class OtherFieldHomTextRating : HomogeneousTextRating
  {
    public OtherFieldHomTextRating(Gruppenplaner.Grouping.Grouping gi, string fieldname)
      : base(gi, (IMemberValueGetter) new OtherFieldGetter(fieldname))
    {
    }
  }
}
