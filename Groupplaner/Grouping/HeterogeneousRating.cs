using Basics;
using Groupplaner.Basics;
using System;
using System.Collections.Generic;

namespace Gruppenplaner.Grouping
{
  public abstract class HeterogeneousRating : IGroupRating
  {
    private Gruppenplaner.Grouping.Grouping Gi;
    private FrequencyTable CompleteValueFrequencies;

    public HeterogeneousRating(Gruppenplaner.Grouping.Grouping gi) => this.Gi = gi;

    public double GetRating()
    {
      this.DoHistogram();
      double num = 0.0;
      foreach (Group g in (List<Group>) this.Gi)
        num += this.RateGroup(g);
      return num / (double) this.Gi.Count;
    }

    protected abstract string GetValue(Member m);

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

    private string GetValueNotNull(Member m) => this.GetValue(m) ?? "";

    private double RateGroup(Group g)
    {
      double num1 = 0.0;
      FrequencyTable ft = new FrequencyTable();
      this.BuildFrequencies(g, ft);
      foreach (string str in (IEnumerable<string>) this.CompleteValueFrequencies.Items())
      {
        double relativeFrequency = this.CompleteValueFrequencies.GetRelativeFrequency(str);
        double num2 = ft.Contains(str) ? ft.GetRelativeFrequency(str) : 0.0;
        num1 += Math.Abs(num2 - relativeFrequency);
      }
      return num1 / 2.0;
    }
  }
}
