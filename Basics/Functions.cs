using System;

namespace Basics
{
  public class Functions
  {
    public static int TranslateToRange(double r, int min, int max) => (int) (r * (double) (max - min + 1)) + min;

    public static int RandomInt(Random r, int min, int max) => Functions.TranslateToRange(r.NextDouble(), min, max);

    public static double StrToDouble(string s)
    {
      double num = 0.0;
      try
      {
        num = double.Parse(s);
      }
      catch (Exception ex)
      {
      }
      return num;
    }
  }
}
