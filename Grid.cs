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
	public partial class Grid : Form
	{
		public Grid()
		{
			InitializeComponent();
		}
	
		public DataTable RowData = new DataTable();
		public CSVRuleSettings rules = new CSVRuleSettings();
		public Common cmn = new Common();
		
		public void FilterRows()
		{
			RowData = cmn.FilterRows(rules, RowData);
			SetSource();
		}
		
		public void SetSource()
		{
			dataGridView1.DataSource = RowData;
		}
		
		public void FilterColors()
		{
			for (int i = 0; i < rules.HighlightList.Count; i++)
			{
				if(!(rules.HighlightList[i].Value == "*"))
				{
					foreach(DataGridViewRow row in dataGridView1.Rows)
					{
						foreach(DataGridViewCell cell in row.Cells)
						{
							try
							{
								if(cell.Value.ToString() == rules.HighlightList[i].Value)
								{
									switch (rules.HighlightList[i].Color)
									{
										case "Red":
											cell.Style.BackColor = Color.Red;
											break;
											
										case "Green":
											cell.Style.BackColor = Color.Green;
											break;
										case "Yellow":
											cell.Style.BackColor = Color.Yellow;
											break;
										case "Blue":
											cell.Style.BackColor = Color.Blue;
											break;
										case "Orange":
											cell.Style.BackColor = Color.Orange;
											break;
										case "Pink":
											cell.Style.BackColor = Color.Pink;
											break;
										case "Fuchsia":
											cell.Style.BackColor = Color.Fuchsia;
											break;
										case "Black":
											cell.Style.BackColor = Color.Black;
											break;
										case "Magenta":
											cell.Style.BackColor = Color.Magenta;
											break;
				//New Colors
										case "Aquamarine":
											cell.Style.BackColor = Color.Aquamarine;
											break;
											
										case "OliveDrab":
											cell.Style.BackColor = Color.OliveDrab;
											break;
											
										case "Grey":
											cell.Style.BackColor = Color.Grey;
											break;
											
										case "Lavender":
											cell.Style.BackColor = Color.Lavender;
											break;
											
										case "Khaki":
											cell.Style.BackColor = Color.Khaki;
											break;
											
										case "LimeGreen":
											cell.Style.BackColor = Color.LimeGreen;
											break;
											
										case "Navy":
											cell.Style.BackColor = Color.Navy;
											break;
											
										case "SteelBlue":
											cell.Style.BackColor = Color.SteelBlue;
											break;
											
										case "YellowGreen":
											cell.Style.BackColor = Color.YellowGreen;
											break;
									} //end switch
								}//end if Value.ToString() == rules.HighlightList[i]
							}//End try
							catch(Exception e)
							{
								
							}
						}//end each cell
					}//end each ro
				}//end if value not *
			}// End for each rule
			
			dataGridView1.EndEdit();
			dataGridView1.Update();
		}
	}
}
