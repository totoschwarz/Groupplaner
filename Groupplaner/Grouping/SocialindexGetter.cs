using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class SocialindexGetter : IMemberValueGetter
  {
    public string GetValue(Member m) => m.Socialindex;
  }
}
