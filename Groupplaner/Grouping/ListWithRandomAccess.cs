using Basics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gruppenplaner.Grouping
{
  public class ListWithRandomAccess<T> : List<T>
  {
    private Random r;
    private static int Seed;

    public ListWithRandomAccess() => this.r = new Random(ListWithRandomAccess<T>.Seed++);

    public T GetRandomElement() => this.ElementAt<T>(Functions.RandomInt(this.r, 0, this.Count - 1));
  }
}
