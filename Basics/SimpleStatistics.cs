using System;
using System.Collections.Generic;

namespace Basics
{
  public class SimpleStatistics : List<double>
  {
    public double Min()
    {
      double val1 = double.MaxValue;
      foreach (double val2 in (List<double>) this)
        val1 = Math.Min(val1, val2);
      return val1;
    }

    public double Max()
    {
      double val1 = double.MinValue;
      foreach (double val2 in (List<double>) this)
        val1 = Math.Max(val1, val2);
      return val1;
    }

    public double Span() => this.Max() - this.Min();

    public double Average() => this.Sum() / (double) this.Count;

    public double Sum()
    {
      double num1 = 0.0;
      foreach (double num2 in (List<double>) this)
        num1 += num2;
      return num1;
    }
  }
}
