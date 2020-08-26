using Groupplaner.Basics;
using Gruppenplaner.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Versioncontrol;

namespace Groupplaner
{
  public class MainWindow : Form
  {
    private Members ms;
    private Gruppenplaner.Grouping.Grouping Gi;
    public readonly string FRIEND_SEP = ", ";
    public readonly string FIELD_SEP = "\t";
    public readonly string ROW_SEP = "\r\n";
    private string csvText = "";
    private IContainer components;
    private Label labelRating;
    private Label labelActualRating;
    private Button buttonOptimize;
    private Button buttonReadCSV;
    private NumericUpDown numericNoOfGroups;
    private Label label1;
    private ListBox listBoxSelectGroup;
    private DataGridView dataGridViewGroup;
    private Label labelAnzahlMem;
    private NumericUpDown numericDepth;
    private Label labelSteps;
    private Label labelMitglieder;
    private GroupBox groupOptimization;
    private Label labelNotAssigned;
    private Label labelCountNotAssigned;
    private Button buttonCopyToClipboard;
    private Label labelDepth;
    private NumericUpDown numericSteps;
    private RatingMainControl ratingMainControl;
    private DataGridView dataGridViewNotAssigned;
    private Button buttonToGroup;
    private Button buttonToNotAssigned;
    private GroupBox groupOptimize;
    private Button buttonRemoveUnsatisfied;
    private LinkLabel linkAuthor;
    private Label labelGrpauswahl;
    private Label labelVersionHint;

    public MainWindow()
    {
      this.InitializeComponent();
      this.ClearAll();
      this.EnableGUI();
      this.Versioncontrol();
    }

    private void Versioncontrol()
    { 
      Checker instance = Checker.GetInstance();
      if (!instance.CopyIsRunnable())
        this.groupOptimize.Enabled = false;
      this.labelVersionHint.Text = instance.GetLicenceHint();
    }

    private void buttonReadCSV_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Title = "Schülerdaten öffnen";
      openFileDialog1.CheckFileExists = true;
      openFileDialog1.CheckPathExists = true;
      openFileDialog1.DefaultExt = "csv";
      openFileDialog1.Filter = "csv files (*.csv)|*.csv";
      openFileDialog1.FilterIndex = 2;
      openFileDialog1.RestoreDirectory = true;
      openFileDialog1.ReadOnlyChecked = true;
      openFileDialog1.ShowReadOPnly = true;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (openFileDialog2.ShowDialog() == DialogResult.OK)
      {
        this.ClearAll();
        try
        {
          this.SetText(new StreamReader(openFileDialog2.FileName, Encoding.Default).ReadToEnd());
          this.DoMembersAndGrouping();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Beim Lesen der Datei ist ein Fehler aufgetreten: " + ex.Message);
          this.ClearAll();
        }
      }
      this.EnableGUI();
    }

    private void ClearAll()
    {
      this.ms = (Members) null;
      this.Gi = (Gruppenplaner.Grouping.Grouping) null;
      this.SetText("");
      this.ratingMainControl.ClearAndDisable();
      this.listBoxSelectGroup.DataSource = (object) new List<string>();
      this.redrawCountNotAssigned();
      this.dataGridViewGroup.Rows.Clear();
      this.dataGridViewNotAssigned.Rows.Clear();
      this.redrawCountNotAssigned();
      this.ratingMainControl.ClearAndDisable();
    }

    private void DoMembersAndGrouping()
    {
      this.ms = new Members(this.csvText);
      this.Gi = new Gruppenplaner.Grouping.Grouping(this.ms, Convert.ToInt32(this.numericNoOfGroups.Value));
      this.InitListBox();
      this.ratingMainControl.Grouping = this.Gi;
      this.RedrawDataViewNotAssigned();
      this.redrawCountNotAssigned();
    }

    private void SetText(string text) => this.csvText = text;

    private void EnableGUI()
    {
      this.ratingMainControl.Enabled = this.Gi != null;
      this.groupOptimization.Enabled = this.Gi != null;
      this.ratingMainControl.Enabled = this.Gi != null;
      this.buttonCopyToClipboard.Enabled = this.Gi != null;
      this.buttonRemoveUnsatisfied.Enabled = this.Gi != null;
      this.buttonToGroup.Enabled = this.Gi != null;
      this.buttonToNotAssigned.Enabled = this.Gi != null;
    }

    private void InitListBox()
    {
      ICollection<string> strings = (ICollection<string>) new List<string>();
      foreach (Group group in (List<Group>) this.Gi)
        strings.Add(group.Name);
      this.listBoxSelectGroup.DataSource = (object) strings;
    }

    private void buttonOptimize_Click(object sender, EventArgs e)
    {
      if (this.DoUnsatisfiedMemExsist())
        return;
      IGroupRating groupRating = this.GetGroupRating(true);
      if (groupRating == null)
        return;
      Finder finder = new Finder(this.Gi, groupRating);
      for (int index = 1; (Decimal) index <= this.numericSteps.Value; ++index)
        finder.OptimizationStep(Convert.ToInt32(this.numericDepth.Value));
      this.Gi = finder.GetGrouping();
      this.RedrawRating();
      this.listBoxSelectGroup.SelectedIndex = 0;
      this.RedrawDataViewGroup(this.Gi.First<Group>());
      this.RedrawDataViewNotAssigned();
      this.redrawCountNotAssigned();
    }

    private bool DoUnsatisfiedMemExsist()
    {
      Group group1 = new Group();
      foreach (Group group2 in (List<Group>) this.Gi)
      {
        Member unsatisfiedMember = group2.GetOneUnsatisfiedMember();
        if (unsatisfiedMember != null)
          group1.Add(unsatisfiedMember);
      }
      Member unsatisfiedMember1 = this.Gi.NotAssigned.GetOneUnsatisfiedMember();
      if (unsatisfiedMember1 != null)
        group1.Add(unsatisfiedMember1);
      if (group1.Count > 0)
      {
        string str = "";
        foreach (Member member in (List<Member>) group1)
        {
          if (str != "")
            str += ", ";
          str += member.Id;
        }
        int num = (int) MessageBox.Show("Folgende Mitglieder weisen fehlende Freunde in ihren Gruppen auf: " + str + ". Bitte manuell korrigieren.");
      }
      return group1.Count > 0;
    }

    private IGroupRating GetGroupRating(bool doWarning)
    {
      try
      {
        return this.ratingMainControl.getRating();
      }
      catch (Exception ex)
      {
        if (doWarning)
        {
          int num = (int) MessageBox.Show("Die Optimierung ist nicht möglich: " + ex.Message);
        }
        return (IGroupRating) null;
      }
    }

    private void RedrawRating()
    {
      IGroupRating groupRating = this.GetGroupRating(false);
      if (groupRating != null)
      {
        double rating = groupRating.GetRating();
        if (rating > 0.0 && rating < 1E-06)
          this.labelActualRating.Text = "< 0.000001";
        else
          this.labelActualRating.Text = Convert.ToString(rating);
      }
      else
        this.labelActualRating.Text = "";
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      Group selectedGroup = this.GetSelectedGroup();
      if (selectedGroup == null)
        return;
      this.RedrawDataViewGroup(selectedGroup);
    }

    private Group GetSelectedGroup()
    {
      Group group1 = (Group) null;
      foreach (Group group2 in (List<Group>) this.Gi)
      {
        if (group2.Name.Equals(this.listBoxSelectGroup.Text))
        {
          group1 = group2;
          break;
        }
      }
      return group1;
    }

    private string[] GetTitles(Gruppenplaner.Grouping.Grouping gi)
    {
      int index = 0;
      string[] strArray = new string[7 + gi.OtherFields.Count];
      strArray[0] = "ID";
      strArray[1] = "Name";
      strArray[2] = "Vorname";
      strArray[3] = "Geschlecht";
      strArray[4] = "Zusammen mit";
      strArray[5] = "Sozialindex";
      strArray[6] = "Leistung";
      foreach (string otherField in (IEnumerable<string>) gi.OtherFields)
      {
        strArray[7 + index] = gi.OtherFields.ElementAt<string>(index);
        ++index;
      }
      return strArray;
    }

    private void RedrawDataViewGeneric(Group g, DataGridView dgv)
    {
      string[] titles = this.GetTitles(this.Gi);
      this.labelAnzahlMem.Text = Convert.ToString(g.Count);
      dgv.Rows.Clear();
      dgv.ColumnCount = titles.Length;
      int index = 0;
      foreach (string str in titles)
      {
        dgv.Columns[index].Name = str;
        ++index;
      }
      dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      dgv.MultiSelect = false;
      foreach (Member m in (List<Member>) g)
        dgv.Rows.Add((object[]) this.GetMemberData(m));
    }

    private void RedrawDataViewGroup(Group g) => this.RedrawDataViewGeneric(g, this.dataGridViewGroup);

    private void RedrawDataViewNotAssigned() => this.RedrawDataViewGeneric(this.Gi.NotAssigned, this.dataGridViewNotAssigned);

    private string[] GetMemberData(Member m)
    {
      string str = "";
      foreach (Member friend in (IEnumerable<Member>) m.GetFriends())
      {
        if (str != "")
          str += this.FRIEND_SEP;
        str += friend.Id;
      }
      string[] strArray = new string[7 + this.Gi.OtherFields.Count];
      strArray[0] = m.Id;
      strArray[1] = m.LastName;
      strArray[2] = m.FirstName;
      strArray[3] = m.Gender;
      strArray[4] = str;
      strArray[5] = m.Socialindex;
      strArray[6] = m.Performance;
      int index = 0;
      foreach (string otherField in (IEnumerable<string>) this.Gi.OtherFields)
      {
        strArray[7 + index] = m.GetValueOf(this.Gi.OtherFields.ElementAt<string>(index));
        ++index;
      }
      return strArray;
    }

    private void redrawCountNotAssigned()
    {
      if (this.Gi != null)
        this.labelCountNotAssigned.Text = Convert.ToString(this.Gi.NotAssigned.Count);
      else
        this.labelCountNotAssigned.Text = "";
    }

    private void buttonCopyToClipboard_Click(object sender, EventArgs e) => Clipboard.SetText(this.groupingToString(this.Gi));

    private string groupingToString(Gruppenplaner.Grouping.Grouping gi)
    {
      string str = "" + this.stringarrayToString(this.GetTitles(gi)) + this.FIELD_SEP + "Gruppe";
      int num = 1;
      foreach (Group g in (List<Group>) gi)
      {
        if (str != "")
          str += this.ROW_SEP;
        str += this.groupToString(g, new int?(num));
        ++num;
      }
      if (gi.NotAssigned.Count > 0)
      {
        if (str != "")
          str += this.ROW_SEP;
        str += this.groupToString(gi.NotAssigned, new int?());
      }
      return str;
    }

    private string groupToString(Group g, int? groupNo)
    {
      string str1 = "";
      string str2 = !groupNo.HasValue ? "" : groupNo.ToString() ?? "";
      foreach (Member m in (List<Member>) g)
      {
        if (str1 != "")
          str1 += this.ROW_SEP;
        str1 += this.stringarrayToString(this.GetMemberData(m));
        str1 = str1 + this.FIELD_SEP + str2;
      }
      return str1;
    }

    private string stringarrayToString(string[] strings)
    {
      string str1 = "";
      foreach (string str2 in strings)
      {
        if (str1 != "")
          str1 += this.FIELD_SEP;
        str1 += str2;
      }
      return str1;
    }

    private void buttonToGroup_Click(object sender, EventArgs e) => this.Transfer(this.Gi.NotAssigned, this.GetSelectedGroup(), this.dataGridViewNotAssigned);

    private void buttonToNotAssigned_Click(object sender, EventArgs e) => this.Transfer(this.GetSelectedGroup(), this.Gi.NotAssigned, this.dataGridViewGroup);

    private void Transfer(Group source, Group target, DataGridView dgv)
    {
      if (source == null || target == null)
        return;
      foreach (DataGridViewRow selectedRow in (BaseCollection) dgv.SelectedRows)
      {
        string ID = selectedRow.Cells[0].Value.ToString();
        Member member = source.GetMember(ID);
        source.Remove(member);
        target.Add(member);
      }
      this.RedrawDataViewsAndInfos();
    }

    private void RedrawDataViewsAndInfos()
    {
      Group selectedGroup = this.GetSelectedGroup();
      this.RedrawDataViewNotAssigned();
      this.RedrawDataViewGroup(selectedGroup);
      this.RedrawRating();
      this.redrawCountNotAssigned();
    }

    private void buttonRemoveUnsatisfied_Click(object sender, EventArgs e)
    {
      Group selectedGroup = this.GetSelectedGroup();
      Member unsatisfiedMember;
      do
      {
        unsatisfiedMember = selectedGroup.GetOneUnsatisfiedMember();
        if (unsatisfiedMember != null)
          new Gruppenplaner.Grouping.Transfer(selectedGroup, this.Gi.NotAssigned).TransferSafelyWithFriends(unsatisfiedMember);
      }
      while (unsatisfiedMember != null);
      this.RedrawDataViewsAndInfos();
    }

    private void MainWindow_Resize(object sender, EventArgs e) => this.setWidgetSizes();

    private void MainWindow_Shown(object sender, EventArgs e) => this.setWidgetSizes();

    private void setWidgetSizes()
    {
      int distanceToWinBorder = 10;
      int num = 5;
      this.setRightBorderPosition((Control) this.ratingMainControl, distanceToWinBorder);
      this.setRightBorderPosition((Control) this.dataGridViewNotAssigned, distanceToWinBorder);
      this.setRightBorderPosition((Control) this.dataGridViewGroup, distanceToWinBorder);
      this.setBottomBorderPosition((Control) this.dataGridViewGroup, this.buttonCopyToClipboard.Height + distanceToWinBorder + num);
      Rectangle bounds = this.buttonCopyToClipboard.Bounds;
      bounds.X = this.ClientRectangle.Width - (bounds.Width + distanceToWinBorder);
      bounds.Y = this.ClientRectangle.Height - (bounds.Height + distanceToWinBorder + num);
      this.buttonCopyToClipboard.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
      int y = this.ClientRectangle.Height - (this.linkAuthor.Height + distanceToWinBorder);
      this.linkAuthor.SetBounds(this.linkAuthor.Bounds.X, y, this.linkAuthor.Width, this.linkAuthor.Height);
      this.labelVersionHint.SetBounds(this.labelVersionHint.Bounds.X, y, this.labelVersionHint.Width, this.labelVersionHint.Height);
    }

    private void setRightBorderPosition(Control c, int distanceToWinBorder)
    {
      Rectangle bounds = c.Bounds;
      int num = this.ClientRectangle.Width - (bounds.X + distanceToWinBorder);
      bounds.Width = num;
      c.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }

    private void setBottomBorderPosition(Control c, int distanceToWinBorder)
    {
      Rectangle bounds = c.Bounds;
      int num = this.ClientRectangle.Height - (bounds.Y + distanceToWinBorder);
      bounds.Height = num;
      c.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }

    private void linkAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start("mailto:thomasweiss@gmx.de");

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainWindow));
      this.labelRating = new Label();
      this.labelActualRating = new Label();
      this.buttonOptimize = new Button();
      this.buttonReadCSV = new Button();
      this.numericNoOfGroups = new NumericUpDown();
      this.label1 = new Label();
      this.listBoxSelectGroup = new ListBox();
      this.dataGridViewGroup = new DataGridView();
      this.labelAnzahlMem = new Label();
      this.numericDepth = new NumericUpDown();
      this.labelSteps = new Label();
      this.labelMitglieder = new Label();
      this.groupOptimization = new GroupBox();
      this.labelDepth = new Label();
      this.numericSteps = new NumericUpDown();
      this.labelNotAssigned = new Label();
      this.labelCountNotAssigned = new Label();
      this.buttonCopyToClipboard = new Button();
      this.dataGridViewNotAssigned = new DataGridView();
      this.buttonToGroup = new Button();
      this.buttonToNotAssigned = new Button();
      this.groupOptimize = new GroupBox();
      this.buttonRemoveUnsatisfied = new Button();
      this.linkAuthor = new LinkLabel();
      this.labelGrpauswahl = new Label();
      this.labelVersionHint = new Label();
      this.ratingMainControl = new RatingMainControl();
      this.numericNoOfGroups.BeginInit();
      ((ISupportInitialize) this.dataGridViewGroup).BeginInit();
      this.numericDepth.BeginInit();
      this.groupOptimization.SuspendLayout();
      this.numericSteps.BeginInit();
      ((ISupportInitialize) this.dataGridViewNotAssigned).BeginInit();
      this.groupOptimize.SuspendLayout();
      this.SuspendLayout();
      this.labelRating.AutoSize = true;
      this.labelRating.Location = new Point(348, 10);
      this.labelRating.Name = "labelRating";
      this.labelRating.Size = new Size(58, 13);
      this.labelRating.TabIndex = 2;
      this.labelRating.Text = "Bewertung";
      this.labelActualRating.Location = new Point(351, 25);
      this.labelActualRating.Name = "labelActualRating";
      this.labelActualRating.Size = new Size(75, 13);
      this.labelActualRating.TabIndex = 3;
      this.labelActualRating.Text = "______";
      this.buttonOptimize.Location = new Point(6, 16);
      this.buttonOptimize.Name = "buttonOptimize";
      this.buttonOptimize.Size = new Size(125, 23);
      this.buttonOptimize.TabIndex = 4;
      this.buttonOptimize.Text = "Optimiere";
      this.buttonOptimize.UseVisualStyleBackColor = true;
      this.buttonOptimize.Click += new EventHandler(this.buttonOptimize_Click);
      this.buttonReadCSV.Location = new Point(7, 15);
      this.buttonReadCSV.Name = "buttonReadCSV";
      this.buttonReadCSV.Size = new Size(119, 23);
      this.buttonReadCSV.TabIndex = 5;
      this.buttonReadCSV.Text = "CSV lesen";
      this.buttonReadCSV.UseVisualStyleBackColor = true;
      this.buttonReadCSV.Click += new EventHandler(this.buttonReadCSV_Click);
      this.numericNoOfGroups.Location = new Point(79, 40);
      this.numericNoOfGroups.Maximum = new Decimal(new int[4]
      {
        25,
        0,
        0,
        0
      });
      this.numericNoOfGroups.Minimum = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.numericNoOfGroups.Name = "numericNoOfGroups";
      this.numericNoOfGroups.Size = new Size(47, 20);
      this.numericNoOfGroups.TabIndex = 6;
      this.numericNoOfGroups.Value = new Decimal(new int[4]
      {
        5,
        0,
        0,
        0
      });
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 42);
      this.label1.Name = "label1";
      this.label1.Size = new Size(58, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "# Gruppen";
      this.listBoxSelectGroup.FormattingEnabled = true;
      this.listBoxSelectGroup.Location = new Point(12, 99);
      this.listBoxSelectGroup.Name = "listBoxSelectGroup";
      this.listBoxSelectGroup.Size = new Size(138, 186);
      this.listBoxSelectGroup.TabIndex = 8;
      this.listBoxSelectGroup.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
      this.dataGridViewGroup.AllowUserToAddRows = false;
      this.dataGridViewGroup.AllowUserToDeleteRows = false;
      this.dataGridViewGroup.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewGroup.Location = new Point(12, 472);
      this.dataGridViewGroup.Name = "dataGridViewGroup";
      this.dataGridViewGroup.ReadOnly = true;
      this.dataGridViewGroup.Size = new Size(592, 163);
      this.dataGridViewGroup.TabIndex = 9;
      this.labelAnzahlMem.Location = new Point(70, 456);
      this.labelAnzahlMem.Name = "labelAnzahlMem";
      this.labelAnzahlMem.Size = new Size(30, 13);
      this.labelAnzahlMem.TabIndex = 10;
      this.labelAnzahlMem.Text = "-";
      this.numericDepth.Location = new Point(170, 19);
      this.numericDepth.Maximum = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.numericDepth.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.numericDepth.Name = "numericDepth";
      this.numericDepth.Size = new Size(55, 20);
      this.numericDepth.TabIndex = 11;
      this.numericDepth.Value = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      this.labelSteps.AutoSize = true;
      this.labelSteps.Location = new Point(232, 21);
      this.labelSteps.Name = "labelSteps";
      this.labelSteps.Size = new Size(43, 13);
      this.labelSteps.TabIndex = 12;
      this.labelSteps.Text = "Schritte";
      this.labelMitglieder.AutoSize = true;
      this.labelMitglieder.Location = new Point(15, 456);
      this.labelMitglieder.Name = "labelMitglieder";
      this.labelMitglieder.Size = new Size(52, 13);
      this.labelMitglieder.TabIndex = 13;
      this.labelMitglieder.Text = "Mitglieder";
      this.groupOptimization.Controls.Add((Control) this.labelDepth);
      this.groupOptimization.Controls.Add((Control) this.numericSteps);
      this.groupOptimization.Controls.Add((Control) this.buttonOptimize);
      this.groupOptimization.Controls.Add((Control) this.numericDepth);
      this.groupOptimization.Controls.Add((Control) this.labelSteps);
      this.groupOptimization.Controls.Add((Control) this.labelRating);
      this.groupOptimization.Controls.Add((Control) this.labelActualRating);
      this.groupOptimization.Enabled = false;
      this.groupOptimization.Location = new Point(161, 249);
      this.groupOptimization.Name = "groupOptimization";
      this.groupOptimization.Size = new Size(437, 45);
      this.groupOptimization.TabIndex = 16;
      this.groupOptimization.TabStop = false;
      this.groupOptimization.Text = "4. Optimierung";
      this.labelDepth.AutoSize = true;
      this.labelDepth.Location = new Point(133, 21);
      this.labelDepth.Name = "labelDepth";
      this.labelDepth.Size = new Size(31, 13);
      this.labelDepth.TabIndex = 14;
      this.labelDepth.Text = "Tiefe";
      this.numericSteps.Location = new Point(281, 19);
      this.numericSteps.Maximum = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.numericSteps.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.numericSteps.Name = "numericSteps";
      this.numericSteps.Size = new Size(55, 20);
      this.numericSteps.TabIndex = 13;
      this.numericSteps.Value = new Decimal(new int[4]
      {
        100,
        0,
        0,
        0
      });
      this.labelNotAssigned.AutoSize = true;
      this.labelNotAssigned.Location = new Point(18, 296);
      this.labelNotAssigned.Name = "labelNotAssigned";
      this.labelNotAssigned.Size = new Size(145, 13);
      this.labelNotAssigned.TabIndex = 17;
      this.labelNotAssigned.Text = "Nicht zugewiesene Mitglieder";
      this.labelCountNotAssigned.Location = new Point(165, 296);
      this.labelCountNotAssigned.Name = "labelCountNotAssigned";
      this.labelCountNotAssigned.Size = new Size(30, 13);
      this.labelCountNotAssigned.TabIndex = 18;
      this.labelCountNotAssigned.Text = "-";
      this.buttonCopyToClipboard.Location = new Point(509, 638);
      this.buttonCopyToClipboard.Name = "buttonCopyToClipboard";
      this.buttonCopyToClipboard.Size = new Size(75, 23);
      this.buttonCopyToClipboard.TabIndex = 19;
      this.buttonCopyToClipboard.Text = "Copy";
      this.buttonCopyToClipboard.UseVisualStyleBackColor = true;
      this.buttonCopyToClipboard.Click += new EventHandler(this.buttonCopyToClipboard_Click);
      this.dataGridViewNotAssigned.AllowUserToAddRows = false;
      this.dataGridViewNotAssigned.AllowUserToDeleteRows = false;
      this.dataGridViewNotAssigned.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewNotAssigned.Location = new Point(12, 312);
      this.dataGridViewNotAssigned.Name = "dataGridViewNotAssigned";
      this.dataGridViewNotAssigned.ReadOnly = true;
      this.dataGridViewNotAssigned.Size = new Size(592, 135);
      this.dataGridViewNotAssigned.TabIndex = 21;
      this.buttonToGroup.Location = new Point(544, 448);
      this.buttonToGroup.Name = "buttonToGroup";
      this.buttonToGroup.Size = new Size(24, 23);
      this.buttonToGroup.TabIndex = 22;
      this.buttonToGroup.Text = "↓";
      this.buttonToGroup.UseVisualStyleBackColor = true;
      this.buttonToGroup.Click += new EventHandler(this.buttonToGroup_Click);
      this.buttonToNotAssigned.Location = new Point(574, 448);
      this.buttonToNotAssigned.Name = "buttonToNotAssigned";
      this.buttonToNotAssigned.Size = new Size(24, 23);
      this.buttonToNotAssigned.TabIndex = 23;
      this.buttonToNotAssigned.Text = "↑";
      this.buttonToNotAssigned.UseVisualStyleBackColor = true;
      this.buttonToNotAssigned.Click += new EventHandler(this.buttonToNotAssigned_Click);
      this.groupOptimize.Controls.Add((Control) this.buttonReadCSV);
      this.groupOptimize.Controls.Add((Control) this.numericNoOfGroups);
      this.groupOptimize.Controls.Add((Control) this.label1);
      this.groupOptimize.Location = new Point(12, 8);
      this.groupOptimize.Name = "groupOptimize";
      this.groupOptimize.Size = new Size(138, 69);
      this.groupOptimize.TabIndex = 24;
      this.groupOptimize.TabStop = false;
      this.groupOptimize.Text = "1. Basisdaten";
      this.buttonRemoveUnsatisfied.Location = new Point(357, 448);
      this.buttonRemoveUnsatisfied.Name = "buttonRemoveUnsatisfied";
      this.buttonRemoveUnsatisfied.Size = new Size(177, 23);
      this.buttonRemoveUnsatisfied.TabIndex = 25;
      this.buttonRemoveUnsatisfied.Text = "„Unzufriedene“ herausnehmen ↑";
      this.buttonRemoveUnsatisfied.UseVisualStyleBackColor = true;
      this.buttonRemoveUnsatisfied.Click += new EventHandler(this.buttonRemoveUnsatisfied_Click);
      this.linkAuthor.AutoSize = true;
      this.linkAuthor.Location = new Point(18, 642);
      this.linkAuthor.Name = "linkAuthor";
      this.linkAuthor.Size = new Size(92, 13);
      this.linkAuthor.TabIndex = 26;
      this.linkAuthor.TabStop = true;
      //TODO change 
      this.linkAuthor.Text = "\xD83D\xDCE7 Thomas Weiss";
      this.linkAuthor.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkAuthor_LinkClicked);
      this.labelGrpauswahl.AutoSize = true;
      this.labelGrpauswahl.Location = new Point(15, 82);
      this.labelGrpauswahl.Name = "labelGrpauswahl";
      this.labelGrpauswahl.Size = new Size(87, 13);
      this.labelGrpauswahl.TabIndex = 27;
      this.labelGrpauswahl.Text = "Gruppenauswahl";
      this.labelVersionHint.Location = new Point(116, 642);
      this.labelVersionHint.Name = "labelVersionHint";
      this.labelVersionHint.Size = new Size((int) byte.MaxValue, 13);
      this.labelVersionHint.TabIndex = 28;
      this.labelVersionHint.Text = "Versionshinweis";
      this.ratingMainControl.Enabled = false;
      this.ratingMainControl.Grouping = (Gruppenplaner.Grouping.Grouping) null;
      this.ratingMainControl.Location = new Point(161, 12);
      this.ratingMainControl.Name = "ratingMainControl";
      this.ratingMainControl.Size = new Size(440, 235);
      this.ratingMainControl.TabIndex = 20;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(613, 681);
      this.Controls.Add((Control) this.labelVersionHint);
      this.Controls.Add((Control) this.labelGrpauswahl);
      this.Controls.Add((Control) this.linkAuthor);
      this.Controls.Add((Control) this.buttonRemoveUnsatisfied);
      this.Controls.Add((Control) this.groupOptimize);
      this.Controls.Add((Control) this.buttonToNotAssigned);
      this.Controls.Add((Control) this.buttonToGroup);
      this.Controls.Add((Control) this.dataGridViewNotAssigned);
      this.Controls.Add((Control) this.ratingMainControl);
      this.Controls.Add((Control) this.buttonCopyToClipboard);
      this.Controls.Add((Control) this.labelCountNotAssigned);
      this.Controls.Add((Control) this.labelNotAssigned);
      this.Controls.Add((Control) this.groupOptimization);
      this.Controls.Add((Control) this.labelMitglieder);
      this.Controls.Add((Control) this.labelAnzahlMem);
      this.Controls.Add((Control) this.dataGridViewGroup);
      this.Controls.Add((Control) this.listBoxSelectGroup);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MainWindow);
      this.Text = "Gruppenplaner";
      this.Shown += new EventHandler(this.MainWindow_Shown);
      this.Resize += new EventHandler(this.MainWindow_Resize);
      this.numericNoOfGroups.EndInit();
      ((ISupportInitialize) this.dataGridViewGroup).EndInit();
      this.numericDepth.EndInit();
      this.groupOptimization.ResumeLayout(false);
      this.groupOptimization.PerformLayout();
      this.numericSteps.EndInit();
      ((ISupportInitialize) this.dataGridViewNotAssigned).EndInit();
      this.groupOptimize.ResumeLayout(false);
      this.groupOptimize.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
