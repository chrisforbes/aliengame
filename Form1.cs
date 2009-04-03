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
		public static Font font;
		public Form1()
		{
			InitializeComponent();

			var model = new Model();
			surface1.Model = model;

			var tools = Assembly.GetExecutingAssembly().GetTypes()
				.Where(t => t.BaseType == typeof(Tool))
				.Select(t => Activator.CreateInstance(t))
				.OrderBy(t => t.ToString());

			comboBox1.Items.AddRange(tools.ToArray());
			comboBox1.SelectedIndex = 0;
			surface1.Tool = comboBox1.SelectedItem as Tool;

			var actorClasses = Assembly.GetExecutingAssembly().GetTypes()
				.Where(t => !t.IsAbstract && typeof(Actor).IsAssignableFrom(t));

			comboBox2.Items.AddRange(actorClasses.ToArray());
			comboBox2.SelectedIndex = 0;
			ActorTool.NewActorType = comboBox2.SelectedItem as Type;
			EditActorTool.Ui = new PropertiesForm(() => { model.SyncActorList(); surface1.Invalidate(); });
			font = Font;
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

		void timer1_Tick(object sender, EventArgs e)
		{
			if (surface1.Tool.GetType() == typeof(SimulateTool))
			{
				surface1.Model.Tick();
				surface1.Invalidate();
			}
		}

		void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			ActorTool.NewActorType = comboBox2.SelectedItem as Type;
		}
	}
}
