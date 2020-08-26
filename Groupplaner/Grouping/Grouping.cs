using Groupplaner.Basics;
using System.Collections.Generic;

namespace Gruppenplaner.Grouping
{
  public class Grouping : ListWithRandomAccess<Group>
  {
    public Group NotAssigned { set; get; }

    public ICollection<string> OtherFields { set; get; }

    public Grouping(Members ms, int noOfGroups)
    {
      for (int index = 0; index < noOfGroups; ++index)
        this.Add(new Group()
        {
          Name = "Gruppe " + (index + 1).ToString()
        });
      this.NotAssigned = new Group();
      foreach (Member m in (List<Member>) ms)
        this.NotAssigned.Add(m);
      this.OtherFields = ms.GetOtherFields();
    }

    public int CountAllMembers()
    {
      int num = 0;
      foreach (Group group in (List<Group>) this)
        num += group.Count;
      return num + this.NotAssigned.Count;
    }
  }
}
