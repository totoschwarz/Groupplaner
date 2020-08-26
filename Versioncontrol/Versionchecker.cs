using System;

namespace Versioncontrol
{
  public class Checker
  {
    private DateTime runsUntil = new DateTime(2021, 5, 31, 23, 59, 59);
    private string version = "0.85beta";

    public DateTime RunsUntil
    {
      get => this.runsUntil;
      set => this.runsUntil = value;
    }

    public string Version
    {
      get => this.version;
      set => this.version = value;
    }

    public static Checker GetInstance() => new Checker();

    public bool CopyIsRunnable() => DateTime.Now <= this.runsUntil;

    public string GetLicenceHint() => !this.CopyIsRunnable() ? this.Version + ", abgelaufen seit " + this.RunsUntil.ToString() : this.Version + ", nutzbar bis " + this.RunsUntil.ToString();
  }
}
