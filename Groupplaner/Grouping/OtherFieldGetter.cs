using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class OtherFieldGetter : IMemberValueGetter
  {
    private string Fieldname;

    public OtherFieldGetter(string fieldname) => this.Fieldname = fieldname;

    public string GetValue(Member m) => m.GetValueOf(this.Fieldname);
  }
}
