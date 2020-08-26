using Groupplaner.Basics;

namespace Gruppenplaner.Grouping
{
  public class PerformanceGetter : IMemberValueGetter
  {
    public string GetValue(Member m) => m.Performance;
  }
}
