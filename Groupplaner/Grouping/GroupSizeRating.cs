using System;
using System.Collections.Generic;
using System.Linq;

namespace Gruppenplaner.Grouping
{
  public class GroupSizeRating : IGroupRating
  {
    private Gruppenplaner.Grouping.Grouping Gi;

    public GroupSizeRating(Gruppenplaner.Grouping.Grouping gi) => this.Gi = gi;

    public double GetRating()
    {
      double sumOfDeviations = 0.0;
      double num1 = (double) this.Gi.CountAllMembers() / (double) this.Gi.Count<Group>();
      int num2 = 0;
      int num3 = this.Gi.CountAllMembers();
      foreach (Group group in (List<Group>) this.Gi)
        sumOfDeviations += Math.Abs((double) group.Count - num1);
      return GroupSizeRating.Normalize(sumOfDeviations, 0.0, (double) this.Gi.Count<Group>(), (double) num2, (double) num3);
    }

    private static double Normalize(
      double sumOfDeviations,
      double minX,
      double maxX,
      double minY,
      double maxY)
    {
      return (sumOfDeviations / (maxX - minX) - minY) / (maxY - minY);
    }
  }
}
