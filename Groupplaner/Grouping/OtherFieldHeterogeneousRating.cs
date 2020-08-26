using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class OtherFieldHeterogeneousRating : HeterogeneousRating
  {
    private string Fieldname;

    public OtherFieldHeterogeneousRating(Gruppenplaner.Grouping.Grouping gi, string fieldname)
      : base(gi)
      => this.Fieldname = fieldname;

    protected override string GetValue(Member m) => m.GetValueOf(this.Fieldname);
  }
}
