using Groupplaner.Basics;
using System.Collections.Generic;

namespace Gruppenplaner.Grouping
{
  public class Group : ListWithRandomAccess<Member>
  {
    public string Name { get; set; }

    public bool IsFriendshipComplete() => this.GetOneUnsatisfiedMember() == null;

    public Member GetOneUnsatisfiedMember()
    {
      Member member1 = (Member) null;
      foreach (Member member2 in (List<Member>) this)
      {
        if (member2.GetFriends().Count > 0)
        {
          member1 = member2;
          foreach (Member friend in (IEnumerable<Member>) member2.GetFriends())
          {
            if (this.Contains(friend))
            {
              member1 = (Member) null;
              break;
            }
          }
          if (member1 != null)
            break;
        }
      }
      return member1;
    }

    public void TransferMember(Member m, Group to)
    {
      this.Remove(m);
      to.Add(m);
    }

    public Member GetMember(string ID)
    {
      Member member1 = (Member) null;
      foreach (Member member2 in (List<Member>) this)
      {
        if (member2.Id == ID)
        {
          member1 = member2;
          break;
        }
      }
      return member1;
    }
  }
}
