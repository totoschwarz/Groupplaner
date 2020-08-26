using Groupplaner.Basics;
using System.Collections.Generic;

namespace Gruppenplaner.Grouping
{
  public class Transfer
  {
    private Group From;
    private Group To;
    private ICollection<Member> Transfered = (ICollection<Member>) new List<Member>();

    public Transfer(Group from, Group to)
    {
      this.From = from;
      this.To = to;
    }

    public void TransferMember(Member m)
    {
      this.From.TransferMember(m, this.To);
      this.Transfered.Add(m);
    }

    public void TransferSafelyWithFriends(Member m)
    {
      this.TransferMember(m);
      bool flag = true;
      while (flag)
      {
        flag = false;
        Member unsatisfiedMember1 = this.From.GetOneUnsatisfiedMember();
        if (unsatisfiedMember1 != null)
        {
          flag = true;
          this.TransferMember(unsatisfiedMember1);
        }
        Member unsatisfiedMember2 = this.To.GetOneUnsatisfiedMember();
        if (unsatisfiedMember2 != null)
        {
          flag = true;
          Group group = new Group();
          foreach (Member friend in (IEnumerable<Member>) unsatisfiedMember2.GetFriends())
          {
            if (this.From.Contains(friend))
              group.Add(friend);
          }
          this.TransferMember(group.GetRandomElement());
        }
      }
    }

    public void UndoCompleteTransfer()
    {
      foreach (Member m in (IEnumerable<Member>) this.Transfered)
        this.To.TransferMember(m, this.From);
      this.Transfered.Clear();
    }
  }
}
