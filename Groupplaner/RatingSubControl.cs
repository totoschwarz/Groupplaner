using Gruppenplaner.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Groupplaner
{
  public class RatingSubControl : UserControl
  {
    private readonly string GENDER = "Geschlecht";
    private readonly string SOCIALINDEX = "Sozialindex";
    private readonly string PERFORMANCE = "Leistung";
    private IContainer components;
    private Button buttonTrash;
    private NumericUpDown weightControl;
    private ComboBox ratingFieldControl;
    private Label labelRatingField;
    private Label labelWeight;
    private Label labelType;
    private RadioButton selectorHeterogeneous;
    private RadioButton selectorHomogeneousText;
    private RadioButton selectorHomogeneousNumber;

    public Gruppenplaner.Grouping.Grouping Grouping { set; get; }

    public RatingSubControl(Gruppenplaner.Grouping.Grouping Gi)
    {
      this.InitializeComponent();
      this.Grouping = Gi;
      this.setFields();
    }

    private void setFields()
    {
      this.ratingFieldControl.Items.Clear();
      this.ratingFieldControl.Items.Add((object) this.GENDER);
      this.ratingFieldControl.Items.Add((object) this.SOCIALINDEX);
      this.ratingFieldControl.Items.Add((object) this.PERFORMANCE);
      foreach (object otherField in (IEnumerable<string>) this.Grouping.OtherFields)
        this.ratingFieldControl.Items.Add(otherField);
    }

    public IGroupRating getRating()
    {
      string text = this.ratingFieldControl.Text;
      IGroupRating groupRating = (IGroupRating) null;
      if (this.selectorHeterogeneous.Checked)
      {
        if (text == this.GENDER)
          groupRating = (IGroupRating) new GenderHetRating(this.Grouping);
        else if (text == this.SOCIALINDEX)
          groupRating = (IGroupRating) new SocialindexHetRating(this.Grouping);
        else if (text == this.PERFORMANCE)
        {
          groupRating = (IGroupRating) new PerformanceHetRating(this.Grouping);
        }
        else
        {
          if (!this.Grouping.OtherFields.Contains(text))
            throw new InvalidOperationException("Die Verteilung kann das Feld '" + text + "' nicht einbeziehen.");
          groupRating = (IGroupRating) new OtherFieldHeterogeneousRating(this.Grouping, text);
        }
      }
      else if (this.selectorHomogeneousText.Checked)
      {
        if (text == this.GENDER)
          groupRating = (IGroupRating) new GenderHomTextRating(this.Grouping);
        else if (text == this.SOCIALINDEX)
          groupRating = (IGroupRating) new SocialindexHomTextRating(this.Grouping);
        else if (text == this.PERFORMANCE)
        {
          groupRating = (IGroupRating) new PerformanceHomTextRating(this.Grouping);
        }
        else
        {
          if (!this.Grouping.OtherFields.Contains(text))
            throw new InvalidOperationException("Die Verteilung kann das Feld '" + text + "' nicht einbeziehen.");
          groupRating = (IGroupRating) new OtherFieldHomTextRating(this.Grouping, text);
        }
      }
      else if (this.selectorHomogeneousNumber.Checked)
      {
        if (text == this.GENDER)
          groupRating = (IGroupRating) new GenderHomScaledRating(this.Grouping);
        else if (text == this.SOCIALINDEX)
          groupRating = (IGroupRating) new SocialindexHomScaledRating(this.Grouping);
        else if (text == this.PERFORMANCE)
        {
          groupRating = (IGroupRating) new PerformanceHomScaledRating(this.Grouping);
        }
        else
        {
          if (!this.Grouping.OtherFields.Contains(text))
            throw new InvalidOperationException("Die Verteilung kann das Feld '" + text + "' nicht einbeziehen.");
          groupRating = (IGroupRating) new OtherFieldHomScaledRating(this.Grouping, text);
        }
      }
      return groupRating;
    }

    public double getWeight() => (double) this.weightControl.Value;

    private void buttonTrash_Click(object sender, EventArgs e) => this.Parent.Controls.Remove((Control) this);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RatingSubControl));
      this.buttonTrash = new Button();
      this.weightControl = new NumericUpDown();
      this.ratingFieldControl = new ComboBox();
      this.labelRatingField = new Label();
      this.labelWeight = new Label();
      this.labelType = new Label();
      this.selectorHeterogeneous = new RadioButton();
      this.selectorHomogeneousText = new RadioButton();
      this.selectorHomogeneousNumber = new RadioButton();
      this.weightControl.BeginInit();
      this.SuspendLayout();
      this.buttonTrash.Image = (Image) componentResourceManager.GetObject("buttonTrash.Image");
      this.buttonTrash.Location = new Point(8, 10);
      this.buttonTrash.Name = "buttonTrash";
      this.buttonTrash.Size = new Size(29, 32);
      this.buttonTrash.TabIndex = 0;
      this.buttonTrash.UseVisualStyleBackColor = true;
      this.buttonTrash.Click += new EventHandler(this.buttonTrash_Click);
      this.weightControl.Location = new Point(340, 22);
      this.weightControl.Maximum = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      this.weightControl.Name = "weightControl";
      this.weightControl.Size = new Size(60, 20);
      this.weightControl.TabIndex = 1;
      this.weightControl.Value = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      this.ratingFieldControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.ratingFieldControl.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.ratingFieldControl.FormattingEnabled = true;
      this.ratingFieldControl.Location = new Point(50, 22);
      this.ratingFieldControl.Name = "ratingFieldControl";
      this.ratingFieldControl.Size = new Size(121, 21);
      this.ratingFieldControl.TabIndex = 3;
      this.labelRatingField.AutoSize = true;
      this.labelRatingField.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelRatingField.Location = new Point(47, 6);
      this.labelRatingField.Name = "labelRatingField";
      this.labelRatingField.Size = new Size(27, 13);
      this.labelRatingField.TabIndex = 5;
      this.labelRatingField.Text = "Feld";
      this.labelWeight.AutoSize = true;
      this.labelWeight.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelWeight.Location = new Point(337, 6);
      this.labelWeight.Name = "labelWeight";
      this.labelWeight.Size = new Size(63, 13);
      this.labelWeight.TabIndex = 6;
      this.labelWeight.Text = "Gewichtung";
      this.labelType.AutoSize = true;
      this.labelType.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelType.Location = new Point(174, 6);
      this.labelType.Name = "labelType";
      this.labelType.Size = new Size(54, 13);
      this.labelType.TabIndex = 7;
      this.labelType.Text = "Verteilung";
      this.selectorHeterogeneous.AutoSize = true;
      this.selectorHeterogeneous.Checked = true;
      this.selectorHeterogeneous.Location = new Point(231, 5);
      this.selectorHeterogeneous.Name = "selectorHeterogeneous";
      this.selectorHeterogeneous.Size = new Size(73, 17);
      this.selectorHeterogeneous.TabIndex = 8;
      this.selectorHeterogeneous.TabStop = true;
      this.selectorHeterogeneous.Text = "heterogen";
      this.selectorHeterogeneous.UseVisualStyleBackColor = true;
      this.selectorHomogeneousText.AutoSize = true;
      this.selectorHomogeneousText.Location = new Point(231, 20);
      this.selectorHomogeneousText.Name = "selectorHomogeneousText";
      this.selectorHomogeneousText.Size = new Size(96, 17);
      this.selectorHomogeneousText.TabIndex = 9;
      this.selectorHomogeneousText.Text = "homogen, Text";
      this.selectorHomogeneousText.UseVisualStyleBackColor = true;
      this.selectorHomogeneousNumber.AutoSize = true;
      this.selectorHomogeneousNumber.Location = new Point(231, 35);
      this.selectorHomogeneousNumber.Name = "selectorHomogeneousNumber";
      this.selectorHomogeneousNumber.Size = new Size(108, 17);
      this.selectorHomogeneousNumber.TabIndex = 10;
      this.selectorHomogeneousNumber.Text = "homogen, Zahlen";
      this.selectorHomogeneousNumber.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.selectorHomogeneousNumber);
      this.Controls.Add((Control) this.selectorHomogeneousText);
      this.Controls.Add((Control) this.selectorHeterogeneous);
      this.Controls.Add((Control) this.labelType);
      this.Controls.Add((Control) this.labelWeight);
      this.Controls.Add((Control) this.labelRatingField);
      this.Controls.Add((Control) this.ratingFieldControl);
      this.Controls.Add((Control) this.weightControl);
      this.Controls.Add((Control) this.buttonTrash);
      this.Name = nameof (RatingSubControl);
      this.Size = new Size(410, 55);
      this.weightControl.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
