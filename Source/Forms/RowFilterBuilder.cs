using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CBComponents.Forms
{
  internal static class RowFilterBuilder
  {
    public static string BuildColumnFilter(string filterExpression, IList table)
    {
      if (table is DataView) return RowFilterBuilder.BuildColumnFilter(filterExpression, (DataView)table);
      else
      {
        var _typeOfList = table.GetType();
        if (_typeOfList.IsGenericType)
        {
          var _genericArguments = _typeOfList.GetGenericArguments();
          if (_genericArguments.Length > 0)
            return RowFilterBuilder.BuildColumnFilter(filterExpression, _genericArguments[0].GetProperties().Where(c => c.PropertyType != typeof(Guid) && c.PropertyType != typeof(Boolean)).Select(c => c.Name).ToArray());
          else return string.Empty;
        }
        else return string.Empty;
      }
    }

    public static string BuildColumnFilter(string filterExpression, DataTable table)
    {
      return BuildColumnFilter(filterExpression, table.Columns);
    }

    public static string BuildColumnFilter(string filterExpression, DataView view)
    {
      return BuildColumnFilter(filterExpression, view.Table.Columns);
    }

    public static string BuildColumnFilter(string filterExpression, DataColumnCollection columns)
    {
      StringCollection columNames = new StringCollection();
      foreach (DataColumn col in columns)
        if (col.DataType != typeof(Guid) && col.DataType != typeof(Boolean))
          columNames.Add(col.ColumnName);
      return BuildColumnFilter(filterExpression, columNames);
    }

    public static string BuildColumnFilter(string filterExpression, StringCollection columns)
    {
      string[] columnNames = new string[columns.Count];
      columns.CopyTo(columnNames, 0);
      return BuildColumnFilter(filterExpression, columnNames);
    }

    public static string BuildColumnFilter(string filterExpression, DataGridView gridView)
    {
      return BuildColumnFilter(filterExpression, gridView.Columns);
    }

    public static string BuildColumnFilter(string filterExpression, DataGridViewColumnCollection columns)
    {
      // filtering TextBoxColumns
      StringCollection columnNames = new StringCollection();
      foreach (DataGridViewColumn column in columns)
        if (column is DataGridViewTextBoxColumn && column.Visible && !string.IsNullOrEmpty(column.DataPropertyName))
          columnNames.Add(column.DataPropertyName);
      string result = BuildColumnFilter(filterExpression, columnNames);
      // filtering ComboBoxColumns
      foreach (DataGridViewColumn column in columns)
      {
        var _column = column as DataGridViewComboBoxColumn;
        if (_column != null && _column.Visible && !string.IsNullOrEmpty(_column.DataPropertyName) && _column.DataSource != null && (_column.DataSource is IList || _column.DataSource is IListSource) && !string.IsNullOrEmpty(_column.ValueMember) && !string.IsNullOrEmpty(_column.DisplayMember))
          try
          {
            StringBuilder _result = new StringBuilder();
            IList _ds = _column.DataSource as IList;
            if (_ds == null) _ds = ((IListSource)_column.DataSource).GetList();
            foreach (DataRowView _row in _ds.Cast<DataRowView>())
              if (_row[_column.DisplayMember].ToString().IndexOf(filterExpression, StringComparison.OrdinalIgnoreCase) >= 0)
              {
                if (_result.Length > 0) _result.Append(" OR ");
                _result.AppendFormat("(CONVERT([{0}], 'System.String') = '{1}')", _column.DataPropertyName, _row[_column.ValueMember]);
              }
            if (_result.Length > 0)
            {
              if (string.IsNullOrEmpty(result)) result = _result.ToString();
              else result += " OR (" + _result.ToString() + ")";
            }
          }
          catch { }
      }
      return result;
    }

    public static string BuildColumnFilter(string filterExpression, DataGridViewColumnCollection columns, DataTable relationTable, string masterDataPropertyName, string relationDataPropertyName)
    {
      // first, filtering text table columns 
      string result = BuildColumnFilter(filterExpression, columns);
      // second, filtering related table columns
      try
      {
        var _values = relationTable.Select(BuildColumnFilter(filterExpression, relationTable)).Select(c => c[relationDataPropertyName].ToString()).Distinct();
        StringBuilder _result = new StringBuilder();
        foreach (var _value in _values)
        {
          if (_result.Length > 0) _result.Append(" OR ");
          _result.AppendFormat("(CONVERT([{0}], 'System.String') = '{1}')", masterDataPropertyName, _value);
        }
        if (_result.Length > 0)
        {
          if (string.IsNullOrEmpty(result)) result = _result.ToString();
          else result += " OR (" + _result.ToString() + ")";
        }
      }
      catch { }
      return result;
    }

    /// <summary>
    /// Builds a string that can be used for DataViews as Row filter.
    /// You might pass 2 arguments: a string that repressents the expressiuon for the filter, seperated by blancs
    /// and an array of coloumn names
    /// </summary>
    /// <param name="filterExpression"></param>
    /// An Expression that might be used for filter. for Example: "Thomas Haller"
    /// <param name="coloumns"></param>
    /// An String Array with the Name of Coloumns
    /// for Example "Prename, Name"
    /// <returns></returns>
    public static string BuildColumnFilter(string filterExpression, string[] columns)
    {
      if (filterExpression == null || columns.Length == 0) return string.Empty;
      filterExpression = filterExpression.Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");

      string[] filters = filterExpression.Split(" ".ToCharArray());
      StringBuilder result = new StringBuilder();

      for (int i = 0; i < filters.Length; i++)
      {
        if (i != 0) result.Append(" AND ");
        result.Append("(");
        string filter = filters[i];
        for (int j = 0; j < columns.Length; j++)
        {
          string column = columns[j];
          if (j != 0) result.Append(" OR "); //we need an prefix "OR" for every operator - but not for the first one
          result.AppendFormat("(CONVERT([{0}], 'System.String') like '%{1}%')", column, filter);
        }
        result.Append(")");
      }
      return result.ToString();
    }

  }
}
