using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public partial class UnitTest
  {
    [TestMethod]
    public void TestObjects()
    {
      using (var _to = new TestObjects())
      {
        Assert.AreEqual(_to.DataGrid.Parent, _to.Form);
        Assert.AreEqual(_to.Form.Location, new System.Drawing.Point(0, 0));
        Assert.AreEqual(_to.Form.Width, 640);
        Assert.AreEqual(_to.Form.Height, 480);
        Assert.AreEqual(_to.DataGrid.Location, new System.Drawing.Point(0, 0));
        Assert.AreEqual(_to.DataGrid.Width, 320);
        Assert.AreEqual(_to.DataGrid.Height, 240);
      }
    }
  }
}
