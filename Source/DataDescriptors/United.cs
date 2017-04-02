using System;
using System.Data;

namespace CBComponents.DataDescriptors
{
  /// <summary>
  /// Data visualization style
  /// </summary>
  public enum EditorDataStyle
  {
    Decimal, Number, Money, Percent, DateTime, Date
  }

  /// <summary>
  /// Delegate of getting data method for EditorColumnMode.ListBox
  /// </summary>
  /// <returns></returns>
  public delegate object GetListBoxItemsDelegate();

  /// <summary>
  /// Delegate of format data method 
  /// </summary>
  /// <param name="DataBoundItem"></param>
  /// <param name="DataPropertyName"></param>
  /// <returns></returns>
  public delegate object FormatValueDelegate(object DataBoundItem, string DataPropertyName);

}
