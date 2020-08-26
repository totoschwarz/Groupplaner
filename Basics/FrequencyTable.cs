using System;
using System.Collections.Generic;

namespace Basics
{
  public class FrequencyTable
  {
    private IDictionary<string, int> AbsoluteFrequencies = (IDictionary<string, int>) new Dictionary<string, int>();
    private int TotalNumber;
    private bool TotalNumberIsAccurate;

    public void Add(string item) => this.AbsoluteFrequencies.Add(item, 0);

    public bool Contains(string item) => this.AbsoluteFrequencies.Keys.Contains(item);

    public ICollection<string> Items() => this.AbsoluteFrequencies.Keys;

    public int GetAbsoluteFrequency(string item) => this.AbsoluteFrequencies[item];

    public double GetRelativeFrequency(string item) => (double) this.AbsoluteFrequencies[item] / (double) this.GetTotalNumber();

    public int GetTotalNumber()
    {
      if (this.TotalNumberIsAccurate)
        return this.TotalNumber;
      this.TotalNumber = 0;
      foreach (KeyValuePair<string, int> absoluteFrequency in (IEnumerable<KeyValuePair<string, int>>) this.AbsoluteFrequencies)
        this.TotalNumber += absoluteFrequency.Value;
      this.TotalNumberIsAccurate = true;
      return this.TotalNumber;
    }

    public void SetAbsoluteFrequency(string item, int value)
    {
      this.AbsoluteFrequencies[item] = value >= 0 ? value : throw new ArgumentException("Eine absolute Häufigkeit kann nicht negativ sein.");
      this.TotalNumberIsAccurate = false;
    }

    public void Increase(string item, int value)
    {
      int num = this.AbsoluteFrequencies[item] + value;
      this.SetAbsoluteFrequency(item, num);
    }

    public void Increase(string item) => this.Increase(item, 1);

    public void Decrease(string item, int value) => this.Increase(item, -value);

    public void Decrease(string item) => this.Increase(item, -1);

    public void AddAndCountAll(List<string> items)
    {
      foreach (string key in items)
      {
        if (!this.AbsoluteFrequencies.ContainsKey(key))
          this.Add(key);
        this.Increase(key);
      }
    }
  }
}
