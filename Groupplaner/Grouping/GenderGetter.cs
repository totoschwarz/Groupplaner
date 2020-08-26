using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class GenderGetter : IMemberValueGetter
  {
    public string GetValue(Member m) => m.Gender;
  }
}
