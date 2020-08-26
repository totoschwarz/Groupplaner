using System.Collections.Generic;

namespace Groupplaner.Basics
{
  public class Member
  {
    public static readonly string ID_ID = nameof (Id);
    public static readonly string NAME_ID = "Name";
    public static readonly string FIRSTNAME_ID = "Vorname";
    public static readonly string GENDER_ID = "Geschlecht";
    public static readonly string SOCIALINDEX_ID = "Sozialindex";
    public static readonly string PERFORMANCE_ID = "Leistung";
    public static readonly string FRIENDS_ID = "zusammen mit";
    private static readonly IList<string> StandardFieldsLow = (IList<string>) new List<string>()
    {
      Member.ID_ID.ToLower(),
      Member.NAME_ID.ToLower(),
      Member.FIRSTNAME_ID.ToLower(),
      Member.GENDER_ID.ToLower(),
      Member.SOCIALINDEX_ID.ToLower(),
      Member.PERFORMANCE_ID.ToLower(),
      Member.FRIENDS_ID.ToLower()
    };
    private ICollection<Member> Friends = (ICollection<Member>) new List<Member>();
    private IDictionary<string, string> OtherFields = (IDictionary<string, string>) new Dictionary<string, string>();

    public string Id { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Gender { get; set; }

    public string Socialindex { get; set; }

    public string Performance { get; set; }

    public ICollection<Member> GetFriends() => this.Friends;

    public void AddFriend(Member f)
    {
      if (this.Friends.Contains(f))
        return;
      this.Friends.Add(f);
    }

    public bool HasFriend(Member f) => this.Friends.Contains(f);

    public static bool IsStandardField(string fieldname) => Member.StandardFieldsLow.Contains(fieldname.ToLower());

    public bool HasField(string fieldname) => this.OtherFields.ContainsKey(fieldname.ToLower());

    public string GetValueOf(string fieldname)
    {
      string str = (string) null;
      this.OtherFields.TryGetValue(fieldname.ToLower(), out str);
      return str;
    }

    public void AddField(string fieldname) => this.OtherFields.Add(fieldname.ToLower(), "");

    public void SetField(string fieldname, string value)
    {
      this.OtherFields.Remove(fieldname.ToLower());
      this.OtherFields.Add(fieldname.ToLower(), value);
    }
  }
}
