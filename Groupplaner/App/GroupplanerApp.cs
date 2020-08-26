using System;
using System.Windows.Forms;

namespace Groupplaner.App
{
  public class GroupplanerApp
  {
    [STAThread]
    public static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MainWindow());
    }
  }
}
