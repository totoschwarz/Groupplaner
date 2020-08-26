using Gruppenplaner.Grouping;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Groupplaner
{
  public class RatingMainControl : UserControl
  {
    private int row;
    private IContainer components;
    private Label labelGroupsize;
    private NumericUpDown groupWeightControl;
    private Label labelWeight;
    private GroupBox groupRating;
    private GroupBox groupSize;
    private Panel panelScrollHelper;
    private Button buttonAddRating;
    private TableLayoutPanel tableRatings;

    public Gruppenplaner.Grouping.Grouping Grouping { set; get; }

    public RatingMainControl()
    {
      this.InitializeComponent();
      this.ClearAndDisable();
    }

    private void buttonAddRating_Click(object sender, EventArgs e)
    {
      this.tableRatings.Controls.Add((Control) new RatingSubControl(this.Grouping), 0, this.row);
      ++this.row;
    }

    public IGroupRating getRating()
    {
      CombinedRating combinedRating = new CombinedRating();
      bool flag = true;
      if (this.groupWeightControl.Value != 0M)
      {
        flag = false;
        combinedRating.Add((IGroupRating) new GroupSizeRating(this.Grouping), (double) this.groupWeightControl.Value);
      }
      foreach (RatingSubControl control in (ArrangedElementCollection) this.tableRatings.Controls)
      {
        IGroupRating rating = control.getRating();
        double weight = control.getWeight();
        combinedRating.Add(rating, weight);
        if (weight != 0.0)
          flag = false;
      }
      if (flag)
        throw new InvalidOperationException("Alle Gewichte sind 0. Wenigstens ein Kriterium muss ein Gewicht ungleich 0 besitzen.");
      return (IGroupRating) combinedRating;
    }

    public void ClearAndDisable()
    {
      this.Enabled = false;
      this.tableRatings.RowCount = 0;
      this.tableRatings.RowStyles.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.labelGroupsize = new Label();
      this.groupWeightControl = new NumericUpDown();
      this.labelWeight = new Label();
      this.groupRating = new GroupBox();
      this.panelScrollHelper = new Panel();
      this.tableRatings = new TableLayoutPanel();
      this.groupSize = new GroupBox();
      this.buttonAddRating = new Button();
      this.groupWeightControl.BeginInit();
      this.groupRating.SuspendLayout();
      this.panelScrollHelper.SuspendLayout();
      this.groupSize.SuspendLayout();
      this.SuspendLayout();
      this.labelGroupsize.AutoSize = true;
      this.labelGroupsize.Location = new Point(5, 32);
      this.labelGroupsize.Name = "labelGroupsize";
      this.labelGroupsize.Size = new Size(177, 13);
      this.labelGroupsize.TabIndex = 2;
      this.labelGroupsize.Text = "Die Gruppen sollen gleich groß sein.";
      this.groupWeightControl.Location = new Point(188, 30);
      this.groupWeightControl.Maximum = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      this.groupWeightControl.Name = "groupWeightControl";
      this.groupWeightControl.Size = new Size(60, 20);
      this.groupWeightControl.TabIndex = 3;
      this.groupWeightControl.Value = new Decimal(new int[4]
      {
        15,
        0,
        0,
        0
      });
      this.labelWeight.AutoSize = true;
      this.labelWeight.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelWeight.Location = new Point(185, 14);
      this.labelWeight.Name = "labelWeight";
      this.labelWeight.Size = new Size(63, 13);
      this.labelWeight.TabIndex = 7;
      this.labelWeight.Text = "Gewichtung";
      this.groupRating.Controls.Add((Control) this.panelScrollHelper);
      this.groupRating.Location = new Point(1, 68);
      this.groupRating.Name = "groupRating";
      this.groupRating.Size = new Size(435, 165);
      this.groupRating.TabIndex = 8;
      this.groupRating.TabStop = false;
      this.groupRating.Text = "3. Weitere Verteilungskriterien";
      this.panelScrollHelper.AutoScroll = true;
      this.panelScrollHelper.Controls.Add((Control) this.tableRatings);
      this.panelScrollHelper.Dock = DockStyle.Fill;
      this.panelScrollHelper.Location = new Point(3, 16);
      this.panelScrollHelper.Name = "panelScrollHelper";
      this.panelScrollHelper.Size = new Size(429, 146);
      this.panelScrollHelper.TabIndex = 2;
      this.tableRatings.AutoSize = true;
      this.tableRatings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.tableRatings.ColumnCount = 1;
      this.tableRatings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 450f));
      this.tableRatings.Dock = DockStyle.Top;
      this.tableRatings.Location = new Point(0, 0);
      this.tableRatings.Name = "tableRatings";
      this.tableRatings.RowCount = 1;
      this.tableRatings.RowStyles.Add(new RowStyle(SizeType.Absolute, 55f));
      this.tableRatings.Size = new Size(429, 55);
      this.tableRatings.TabIndex = 2;
      this.groupSize.Controls.Add((Control) this.labelGroupsize);
      this.groupSize.Controls.Add((Control) this.groupWeightControl);
      this.groupSize.Controls.Add((Control) this.labelWeight);
      this.groupSize.Location = new Point(2, 3);
      this.groupSize.Name = "groupSize";
      this.groupSize.Size = new Size(434, 59);
      this.groupSize.TabIndex = 9;
      this.groupSize.TabStop = false;
      this.groupSize.Text = "2. Kriterium Gruppengröße";
      this.buttonAddRating.Location = new Point(415, 211);
      this.buttonAddRating.Name = "buttonAddRating";
      this.buttonAddRating.Size = new Size(20, 20);
      this.buttonAddRating.TabIndex = 10;
      this.buttonAddRating.Text = "+";
      this.buttonAddRating.UseVisualStyleBackColor = true;
      this.buttonAddRating.Click += new EventHandler(this.buttonAddRating_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.buttonAddRating);
      this.Controls.Add((Control) this.groupSize);
      this.Controls.Add((Control) this.groupRating);
      this.Name = nameof (RatingMainControl);
      this.Size = new Size(439, 235);
      this.groupWeightControl.EndInit();
      this.groupRating.ResumeLayout(false);
      this.panelScrollHelper.ResumeLayout(false);
      this.panelScrollHelper.PerformLayout();
      this.groupSize.ResumeLayout(false);
      this.groupSize.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
