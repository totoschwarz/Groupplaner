using Groupplaner.Basics;
using System;

namespace Gruppenplaner.Grouping
{
  public class Finder
  {
    private Gruppenplaner.Grouping.Grouping Gi;
    private IGroupRating Rating;
    private double ActualRating;

    public Finder(Gruppenplaner.Grouping.Grouping gi, IGroupRating rating)
    {
      this.Gi = gi.Count >= 2 ? gi : throw new ArgumentException("Eine Aufteilung auf weniger als zwei Gruppen ist sinnlos.");
      this.Rating = rating;
      this.ActualRating = this.Rating.GetRating();
    }

    public double GetRating() => this.ActualRating;

    public Gruppenplaner.Grouping.Grouping GetGrouping() => this.Gi;

    public void OptimizationStep(int maxDepth)
    {
      if (maxDepth == 0)
        return;
      Group from = this.Gi.NotAssigned.Count > 0 ? this.Gi.NotAssigned : this.Gi.GetRandomElement();
      while (from.Count == 0)
        from = this.Gi.GetRandomElement();
      Group randomElement1;
      do
      {
        randomElement1 = this.Gi.GetRandomElement();
      }
      while (randomElement1 == from);
      Member randomElement2 = from.GetRandomElement();
      Transfer transfer = new Transfer(from, randomElement1);
      transfer.TransferSafelyWithFriends(randomElement2);
      double rating = this.Rating.GetRating();
      double actualRating = this.ActualRating;
      if (from == this.Gi.NotAssigned || rating < this.ActualRating)
      {
        this.ActualRating = rating;
        this.OptimizationStep(maxDepth - 1);
      }
      else
      {
        this.OptimizationStep(maxDepth - 1);
        if (actualRating > this.ActualRating)
          return;
        transfer.UndoCompleteTransfer();
      }
    }
  }
}
