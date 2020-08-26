using Basics;
using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class OtherFieldHomScaledRating : HomogeneousScaledRating
  {
    private string Fieldname { get; set; }

    public OtherFieldHomScaledRating(Gruppenplaner.Grouping.Grouping gi, string fieldname)
      : base(gi)
      => this.Fieldname = fieldname;

    protected override double GetValue(Member m) => Functions.StrToDouble(m.GetValueOf(this.Fieldname));
  }
}
