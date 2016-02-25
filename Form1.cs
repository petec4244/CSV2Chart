
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSV2Chart
{
	public partial class Form1:Form
	{
		Common Cmn = new Common();
		CSVRuleSettings Rules = new CSVRuleSettings();
		
		public string csvFile = "";
		
		public Form1()
		{
			InitializeComponent()
		}
		
		///Summary
		///Open CSV or Text
		
		private void button1_Click(object sender, EventArgs e)
		{
			//DialogResult D = Cmn.openFile("Csv Files (.csv)|*.csv|Text Files (.txt)|*.txt|All Files (*.*)|*.*", "");
			DialogResult D = Cmn.openFile("All Files (*.*)|*.*|Csv Files (.csv)|*.csv|Text Files (.txt)|*.txt", "");
			if(D == DialogResult.OK)
			{
				CsvPathBox.Text = Cmn.FileName;
				csvFile = CsvPathBox.Text;
				if(!(CsvPathBox.Text.EndsWith(".csv")))
				{
					Cmn.TextToCSV(csvFile);
					CsvPathBox.Text = Cmn.FileName;
				}
			}
		}
		
		///Summary
		///Open Rules File
		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult D = Cmn.openFile("Text Files (.txt)|*.txt|All Files (*.*)|*.*", "");
			if(D == DialogResult.OK)
			{
				RuleBox.Text = Cmn.FileName;
				Cmn.GetRules(RuleBox.Text, Rules);
			}
		}
		
		///Summary
		///Run For GRID
		private void button3_Click(object sender, EventArgs e)
		{
			Grid Grid = new Grid();
			Grid.rules = Rules;
			Grid.RowData = Cmn.PopulateDataTable(CsvPathBox.Text, Rules);
			Grid.FilterRows();
			Grid.Show();
			Grid.FilterColors();
		}
		
		///Summary
		///Run for graph
		private void button4_Click(object sender, EventArgs e)
		{
			Chart Chart = new Chart();
			Chart.Rules = Rules;
			Chart.ChartData = Cmn.PopulateDataTable(CsvPathBox.Text, Rules);
			Chart.Show();
		}
	}
	
	public class Highlight_Rules
	{
		public string Color = "";
		public string Value = "";
	}
	
	public class CSVRuleSettings
	{
		public List<string> FilterColumns = new List<string>();
		public List<Highlight_Rules> HighlightList = new List<Highlight_Rules>();
		public List<int> SkipLines = new List<int>();
		public int headerLoc = 0;
		
		public void clearLists()
		{
			FilterColumns.Clear();
			HighlightList.Clear();
			SkipLines.Clear();
			headerLoc = 0;
		}
	}
}
