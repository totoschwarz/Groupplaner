using Basics;
using Groupplaner.Basics;
using System.Collections.Generic;

namespace Gruppenplaner.Grouping
{
  public class HomogeneousTextRating : IGroupRating
  {
    private Gruppenplaner.Grouping.Grouping Gi;
    private bool setupIsDone;
    private FrequencyTable CompleteValueFrequencies;
    private CombinedRating cr = new CombinedRating();
    private IMemberValueGetter Mvg;

    public HomogeneousTextRating(Gruppenplaner.Grouping.Grouping gi, IMemberValueGetter mvg)
    {
      this.Gi = gi;
      this.Mvg = mvg;
    }

    public double GetRating()
    {
      this.DoSetup();
      return this.cr.GetRating();
    }

    private void DoSetup()
    {
      if (this.setupIsDone)
        return;
      this.setupIsDone = true;
      this.DoHistogram();
      this.DoCombinedRatings();
    }

    private void DoCombinedRatings()
    {
      foreach (string category in (IEnumerable<string>) this.CompleteValueFrequencies.Items())
      {
        double absoluteFrequency = (double) this.CompleteValueFrequencies.GetAbsoluteFrequency(category);
        this.cr.Add((IGroupRating) new HomogeneousTextRating.SingleCategoryHomRating(this.Gi, this.Mvg, category), absoluteFrequency);
      }
    }

    private void DoHistogram()
    {
      if (this.CompleteValueFrequencies != null)
        return;
      this.CompleteValueFrequencies = new FrequencyTable();
      foreach (Group g in (List<Group>) this.Gi)
        this.BuildFrequencies(g, this.CompleteValueFrequencies);
      this.BuildFrequencies(this.Gi.NotAssigned, this.CompleteValueFrequencies);
    }

    private void BuildFrequencies(Group g, FrequencyTable ft)
    {
      List<string> items = new List<string>();
      foreach (Member m in (List<Member>) g)
        items.Add(this.GetValueNotNull(m));
      ft.AddAndCountAll(items);
    }

    private string GetValueNotNull(Member m) => this.Mvg.GetValue(m) ?? "";

    public class SingleCategoryHomRating : HomogeneousScaledRating
    {
      private string Category;
      private IMemberValueGetter Mvg;

      public SingleCategoryHomRating(Gruppenplaner.Grouping.Grouping gi, IMemberValueGetter mvg, string category)
        : base(gi)
      {
        this.Category = category;
        this.Mvg = mvg;
      }

      protected override double GetValue(Member m) => !this.Mvg.GetValue(m).Equals(this.Category) ? 1.0 : 0.0;
    }
  }
}
