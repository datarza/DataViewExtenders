using System;
using System.Data;

namespace CBComponents.DataDescriptors
{
  /// <summary>
  /// Data style
  /// </summary>
  public enum EditorDataStyle
  {
    Quantity, Price, Percent, DateTime, Date, Weight, Volume
  }

  /// <summary>
  /// Delegate of getting data method for EditorColumnMode.ListBox
  /// </summary>
  /// <returns></returns>
  public delegate object GetListBoxItemsDelegate();

  /// <summary>
  ///  Delegate of format data method 
  /// </summary>
  /// <param name="DataBoundItem"></param>
  /// <param name="DataPropertyName"></param>
  /// <returns></returns>
  public delegate object FormatValueDelegate(object DataBoundItem, string DataPropertyName);

}
