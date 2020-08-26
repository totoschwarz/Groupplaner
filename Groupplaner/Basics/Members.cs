using Groupplaner.CSV;
using System;
using System.Collections.Generic;

namespace Groupplaner.Basics
{
  public class Members : List<Member>
  {
    private int IdCol;
    private int[] friendCols;
    private int? LNameCol;
    private int? FNameCol;
    private int? GenderCol;
    private int? SocialiCol;
    private int? PerformanceCol;
    private IDictionary<string, int> OtherCols;

    public Members(string FromString)
    {
      SimpleCSVReader scr = new SimpleCSVReader(FromString, "\r\n", ";");
      scr.RemoveEmptyLines();
      scr.TrimAllFields();
      int[] numArray = scr.Find(0, Member.ID_ID, false);
      if (numArray.Length == 0)
        throw new FormatException("Keine ID-Spalte.");
      this.IdCol = numArray.Length <= 1 ? numArray[0] : throw new FormatException("Mehrere ID-Spalten.");
      this.LNameCol = this.FindHeaderIndex(scr, Member.NAME_ID);
      this.FNameCol = this.FindHeaderIndex(scr, Member.FIRSTNAME_ID);
      this.GenderCol = this.FindHeaderIndex(scr, Member.GENDER_ID);
      this.SocialiCol = this.FindHeaderIndex(scr, Member.SOCIALINDEX_ID);
      this.PerformanceCol = this.FindHeaderIndex(scr, Member.PERFORMANCE_ID);
      this.friendCols = scr.Find(0, Member.FRIENDS_ID, false);
      this.FindOtherCols(scr);
      this.CreateMembers(scr);
      this.CreateFriendship(scr);
    }

    public ICollection<string> GetOtherFields()
    {
      string[] array = new string[this.OtherCols.Keys.Count];
      this.OtherCols.Keys.CopyTo(array, 0);
      return (ICollection<string>) new List<string>((IEnumerable<string>) array);
    }

    private void FindOtherCols(SimpleCSVReader scr)
    {
      this.OtherCols = (IDictionary<string, int>) new Dictionary<string, int>();
      for (int col = 0; col < scr.Width(); ++col)
      {
        if (!Member.IsStandardField(scr.FieldAt(0, col)))
        {
          string key = scr.FieldAt(0, col);
          if (this.OtherCols.ContainsKey(key))
            throw new FormatException("Mehrere Spalten haben den gleichen Namen.");
          this.OtherCols.Add(key, col);
        }
      }
    }

    public Member GetMember(string id)
    {
      Member member1 = (Member) null;
      foreach (Member member2 in (List<Member>) this)
      {
        if (member2.Id.Equals(id))
        {
          member1 = member2;
          break;
        }
      }
      return member1;
    }

    private void CreateFriendship(SimpleCSVReader scr)
    {
      for (int index = 0; index < this.friendCols.Length; ++index)
      {
        for (int row = 1; row < scr.Height(); ++row)
        {
          string id1 = scr.FieldAt(row, this.IdCol);
          string id2 = scr.FieldAt(row, this.friendCols[index]);
          if (!id2.Equals(""))
            this.GetMember(id1).AddFriend(this.GetMember(id2));
        }
      }
    }

    private void CreateMembers(SimpleCSVReader scr)
    {
      for (int row = 1; row < scr.Height(); ++row)
      {
        Member mem = new Member();
        string str = scr.FieldAt(row, this.IdCol);
        mem.Id = !str.Equals("") ? str : throw new FormatException("Id in Zeile " + row.ToString() + " ist leer.)");
        if (this.LNameCol.HasValue)
          mem.LastName = scr.FieldAt(row, this.LNameCol.Value);
        if (this.FNameCol.HasValue)
          mem.FirstName = scr.FieldAt(row, this.FNameCol.Value);
        if (this.GenderCol.HasValue)
          mem.Gender = scr.FieldAt(row, this.GenderCol.Value);
        if (this.SocialiCol.HasValue)
          mem.Socialindex = scr.FieldAt(row, this.SocialiCol.Value);
        if (this.PerformanceCol.HasValue)
          mem.Performance = scr.FieldAt(row, this.PerformanceCol.Value);
        foreach (KeyValuePair<string, int> otherCol in (IEnumerable<KeyValuePair<string, int>>) this.OtherCols)
        {
          mem.AddField(otherCol.Key);
          mem.SetField(otherCol.Key, scr.FieldAt(row, otherCol.Value));
        }
        this.Add(mem);
      }
    }

    public new void Add(Member mem)
    {
      if (this.ContainsID(mem.Id))
        throw new FormatException("Mehrfache Verwendung der Id " + mem.Id + ".");
      base.Add(mem);
    }

    private bool ContainsID(string id)
    {
      bool flag = false;
      foreach (Member member in (List<Member>) this)
      {
        if (member.Id.Equals(id))
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private int? FindHeaderIndex(SimpleCSVReader scr, string headerName)
    {
      int? nullable = new int?();
      int[] numArray = scr.Find(0, headerName, false);
      if (numArray.Length > 1)
        throw new FormatException("Mehrere " + headerName + "-Spalten.");
      if (numArray.Length == 1)
        nullable = new int?(numArray[0]);
      return nullable;
    }
  }
}
