using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CSV2Chart
{
	public partial class Chart : Form
	{
		public DataTable ChartData = new DataTable();
		public CSVRuleSettings Rules = new CSVRuleSettings();
		public Common cmn = new Common();
		
		public Chart()
		{
			InitializeComponent();
		}
		
		private void Chart_Load(object sender, EventArgs e)
		{
			populateDT();
			chart1.Series.Clear();
			
			string[] columnNames = ChartData.Columns.AsEnumerable().Select(column => column.ColumnName).ToArray();
			
			int i = 0;
			for(int j = 1; j < columnNames.Length; j++)
			{
				chart1.Series.Add(columnNames[j]);
				chart1.Series[i].XValueMember = ChartData.Columns[0].ColumnName; //Timestamp for my purposes, in the future make configurable
				chart1.Series[i].YValueMembers = ChartData.Columns[j].ColumnName;
				i++;
			}
			chart1.DataSource = ChartData;
			chart1.DataBind();
		}
		
		private void populateDT()
		{
			ChartData = cmn.FilterRows(Rules, ChartData);
		}
	}
	
	public static class DataColumnCollectionExtensions
	{
		public static IEnumerable<DataColumn> AsEnumerable(this DataColumnCollection source)
		{
			return source.Cast<DataColumn>();
		}
	}
}
