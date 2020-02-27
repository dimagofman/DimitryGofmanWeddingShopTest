using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using DimitryGofmanWeddingShopTest.Classes;

namespace DimitryGofmanWeddingShopTest
{
	/// <summary>This form takes user input and shows the results</summary>
	public partial class Form1 : Form
	{

		public Form1() => InitializeComponent();

		private async void Button1_Click(object sender, EventArgs e) //async so as not to block ui thread
		{
			button1.Enabled = false;
			await Utils.RunSimulation(textBox1.Text, checkBox1.Checked, textBox2, pictureBox1);
			button1.Enabled = true;
		}


	}
}
