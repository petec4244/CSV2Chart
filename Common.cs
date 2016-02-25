using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CSV2Chart
{
	public class Common
	{
		#region FileIO
		public string FileName = "";
		
		public DialogResult openFile(string ftype, string startpath)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if(startpath != "")
			{
				ofd.InitialDirectory = startpath;
			}
			ofd.Filter = ftype;
			DialogResult result = ofd.ShowDialog();
			if(result == DialogResult.OK)
			{
				FileName = ofd.FileName;
			}
			return result;
		}
		
		public bool TextToCSV(string filepath)
		{
			try
			{
				File.WriteAllLines((filepath + ".csv"), File.ReadAllLines(filepath).Select(line => line.Replace("\t", ",")));
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show("ERROR: " + e.ToString() + "\nStopping Execution");
				return false;
			}
			FileName = FileName + ".csv";
			return true;
		}
		
		public bool GetRules(string filepath, CSVRuleSettings Rule)
		{
			//First Clear other rules
			Rule.clearLists();
		
			bool HaveHeader = false, SkipLines = false, Columns = false, Highlights = false;
			try
			{
			string line = "";
			using(var stream = File.OpenRead(filepath))
			{
				using(StreamReader sr = new StreamReader(stream))
				{
					while(sr.Peek() != -1)
					{
						line = sr.ReadLine();
						if(!line.StartsWith("#") && !(line == ""))
						{
							if(line == "[HeaderLoc]")
							{
								HaveHeader = true;
								continue;
							}
							
							if(!SkipLines && HaveHeader) //read for header or next block
							{
								if(line == "[SkipLines]")
								{
									SkipLines = true;
									continue;
								}
								else //not reached the skip lines yet.
								{
									Rule.headerLoc = Int32.Parse(line);
									continue;
								}
							}
							
							if(!Columns && SkipLines)
							{
								if(line == "[Selected Columns]")
								{
									Columns = true;
									continue;
								}
								else
								{
									Rule.SkipLines.Add(Int32.Parse(line));
									continue;
								}
							}
							
							if(!Highlights && Columns) //assume have skiplines read for columns or next block
							{
								if(line == "[Highlight List]")
								{
									Highlights = true;
									continue;
								}
								else
								{
									Rule.FilterColumns.Add(line);
									continue;
								}
							}
							if(Highlights)
							{
								if(line == "[END]") break;
								else
								{
									string[] Add = line.Split('|');
									Highlight_Rules HR = new Highlight_Rules();
									HR.Color = Add[1];
									HR.Value = Add[0];
									Rule.HighlightList.Add(HR);
									continue;
								}
							}
						}//else starts with # or is blank
						else continue;
					} //End While SRpeek
				}
			}
		}//end try
		catch (Exception e)
		{
			System.Windows.Forms.MessageBox.Show("Error: " e.ToString());
			return false;
		}
		if(HaveHeader && SkipLines && Columns && Highlights)
			return true;
		else return false;
	}
	
	#endregion 
	
	public DataTable PopulateDataTable(string filepath, CSVRuleSettings Rules)
	{
		string[] Lines = File.ReadAllLines(filepath);
		string[] Fields = Lines[0].Split(new char[] { ',' });
		int Cols = Fields.GetLength(0);
		DataTable dt = new DataTable();
		
		//Expect column names in first row, make this configurable in the future!!!
		for(int i = 0; i < Cols; i++)
			dt.Columns.Add(Fields[i].ToLower(), typeof(string));
			DataRow Row;
			
			for(int i = 2 i < Lines.GetLength(0); i++)
			{
				Fields = Lines[i].Split(new char[] { ','});
				Row = dt.NewRow();
				for(int f=0; f<Cols; f++)
					Row[f] = Fields[f];
				dt.Rows.Add(Row);
			}
			return dt;
	}
	
	public DataTable FilterRows(CSVRuleSettings rules, DataTable DT)
	{
		try
		{
			DataRow[] FilteredData;
			if(rules.FilterColumns.Count == 1)
			{
				if(!(rules.FilterColumns[0] == "*")) //Filter All
				{
					FilteredData = DT.Select(rules.FilterColumns[0].ToString());
				}
			}
			else //More than one column to filter, so do so..
			{
				string selectStatement = String.Join(",", rules.FilterColumns);
				string[] SplitSelect = selectStatement.Split(',');
				
				DataTable newTable = new DataView(DT).ToTable(false, SplitSelect);
				DT.Clear();
				DT = newTable;
				return DT;
			}
		}
		catch (Exception e)
		{
			System.Windows.Forms.MessageBox.Show("Error: " e.ToString());
		}
		return DT;
	}
}
