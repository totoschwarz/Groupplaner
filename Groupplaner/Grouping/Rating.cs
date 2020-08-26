using System.Collections.Generic;

namespace Gruppenplaner.Grouping
{
  public class CombinedRating : IGroupRating
  {
    private IDictionary<IGroupRating, double> ratings = (IDictionary<IGroupRating, double>) new Dictionary<IGroupRating, double>();
    private double weightsSum;

    public void Add(IGroupRating r, double weight)
    {
      this.ratings.Add(r, weight);
      this.weightsSum += weight;
    }

    public double GetRating()
    {
      double num = 0.0;
      foreach (KeyValuePair<IGroupRating, double> rating in (IEnumerable<KeyValuePair<IGroupRating, double>>) this.ratings)
        num += rating.Key.GetRating() * rating.Value;
      return num / this.weightsSum;
    }
  }
}
