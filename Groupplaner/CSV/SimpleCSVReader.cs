using System;
using System.Collections.Generic;

namespace Groupplaner.CSV
{
  public class SimpleCSVReader
  {
    private string Linebreak;
    private string Fieldbreak;
    private string[] lines;
    private int width;
    private int height;
    private string[,] fields;

    public SimpleCSVReader(string data) => this.Init(data, "\r\n", ";");

    public SimpleCSVReader(string data, string linebreak, string fieldbreak) => this.Init(data, linebreak, fieldbreak);

    private void Init(string data, string linebreak, string fieldbreak)
    {
      this.Linebreak = linebreak;
      this.Fieldbreak = fieldbreak;
      this.lines = this.SplitString(data, this.Linebreak);
      if (data.Length == 0)
        this.InitFields(0, 0);
      else
        this.InitFields();
    }

    private void InitFields(int width, int height)
    {
      this.height = height;
      this.width = width;
      this.fields = new string[height, width];
    }

    private void InitFields() => this.InitFields(this.SplitString(this.lines[0], this.Fieldbreak).Length, this.lines.Length);

    public int Width() => this.width;

    public int Height() => this.height;

    public string FieldAt(int row, int col)
    {
      if (this.fields[row, col] == null)
      {
        string[] strArray = this.SplitString(this.lines[row], this.Fieldbreak);
        for (int index = 0; index < this.Width(); ++index)
          this.fields[row, index] = index >= strArray.Length ? "" : strArray[index];
      }
      return this.fields[row, col];
    }

    public void SetFieldAt(int row, int col, string value) => this.fields[row, col] = value;

    public int[] Find(int row, string value, bool caseSensitive)
    {
      ICollection<int> ints = (ICollection<int>) new List<int>();
      string str = caseSensitive ? value : value.ToLower();
      for (int col = 0; col < this.Width(); ++col)
      {
        if ((caseSensitive ? this.FieldAt(row, col) : this.FieldAt(row, col).ToLower()).Equals(str))
          ints.Add(col);
      }
      int[] array = new int[ints.Count];
      ints.CopyTo(array, 0);
      return array;
    }

    private string[] SplitString(string s, string delimiter) => s.Split(new string[1]
    {
      delimiter
    }, StringSplitOptions.None);

    public void TrimAllFields()
    {
      for (int row = 0; row < this.Height(); ++row)
      {
        for (int col = 0; col < this.Width(); ++col)
        {
          string str = this.FieldAt(row, col);
          this.SetFieldAt(row, col, str.Trim());
        }
      }
    }

    public void RemoveEmptyLines()
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.lines.Length; ++index)
      {
        if (this.lines[index].Length != 0)
          stringList.Add(this.lines[index]);
      }
      this.lines = stringList.ToArray();
      this.InitFields();
    }
  }
}
