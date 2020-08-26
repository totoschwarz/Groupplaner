using Basics;
using Groupplaner.Basics;
using System;
using System.Collections.Generic;

namespace Gruppenplaner.Grouping
{
  public abstract class HomogeneousScaledRating : IGroupRating
  {
    private double min;
    private double max;
    private double span;
    private bool minMaxSpanIsDone;

    private Gruppenplaner.Grouping.Grouping Gi { get; set; }

    public HomogeneousScaledRating(Gruppenplaner.Grouping.Grouping gi) => this.Gi = gi;

    public double GetRating()
    {
      double num1 = 0.0;
      int num2 = 0;
      this.DoMinMaxSpan();
      if (this.span <= 0.0)
        return 0.0;
      foreach (Group g in (List<Group>) this.Gi)
      {
        num1 += (double) g.Count * this.GetGroupRating(g);
        num2 += g.Count;
      }
      return num1 / (double) num2;
    }

    protected abstract double GetValue(Member m);

    private void DoMinMaxSpan()
    {
      if (this.minMaxSpanIsDone)
        return;
      this.minMaxSpanIsDone = true;
      SimpleStatistics simpleStatistics = new SimpleStatistics();
      foreach (List<Member> memberList in (List<Group>) this.Gi)
      {
        foreach (Member m in memberList)
          simpleStatistics.Add(this.GetValue(m));
      }
      foreach (Member m in (List<Member>) this.Gi.NotAssigned)
        simpleStatistics.Add(this.GetValue(m));
      this.min = simpleStatistics.Min();
      this.max = simpleStatistics.Max();
      this.span = simpleStatistics.Span();
    }

    private double GetGroupRating(Group g)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      if (g.Count <= 1)
        return 0.0;
      foreach (Member m in (List<Member>) g)
        num2 += this.GetValue(m);
      double num3 = num2 / (double) g.Count;
      foreach (Member m in (List<Member>) g)
        num1 += Math.Abs(num3 - this.GetValue(m));
      return num1 / (double) g.Count / this.span * 2.0;
    }
  }
}
