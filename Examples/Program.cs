using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Examples
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG  
      Examples.Database.daoDataSet.PrepareDatabase();
#endif
      Application.Run(new MainForm());
    }
  }
}
