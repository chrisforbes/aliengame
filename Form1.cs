using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace AlienGame
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			var model = new Model();
			surface1.Model = model;

			var tools = Assembly.GetExecutingAssembly().GetTypes()
				.Where(t => t.BaseType == typeof(Tool))
				.Select(t => Activator.CreateInstance(t));

			comboBox1.Items.AddRange(tools.ToArray());
			comboBox1.SelectedIndex = 0;
			surface1.Tool = comboBox1.SelectedItem as Tool;
		}

		void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked) 
				surface1.Options |= RenderOptions.Brushes;
			else
				surface1.Options &= ~RenderOptions.Brushes;
		}

		void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			surface1.Tool = comboBox1.SelectedItem as Tool;
		}

		void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox2.Checked)
				surface1.Options |= RenderOptions.Grid;
			else
				surface1.Options &= ~RenderOptions.Grid;
		}
	}

	abstract class Tool
	{
		public abstract string Name { get; }
		public virtual void DrawToolOverlay(Surface s, Graphics g, Model m) { }
		public virtual bool OnMouseDown(Surface s, Model m, Point p, MouseButtons mb) { return false; }
		public virtual bool OnMouseMove(Surface s, Model m, Point p, MouseButtons mb) { return false; }
		public virtual bool OnMouseUp(Surface s, Model m, Point p, MouseButtons mb) { return false; }

		public override string ToString() { return Name; }
	}
}
