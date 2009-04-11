using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using AlienGame.Tools;
using System.Xml;

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

		void ChangeRenderOption(object sender, EventArgs e)
		{
			var bit = (RenderOptions)int.Parse((string)(sender as Control).Tag);
			if ((sender as CheckBox).Checked) surface1.Options |= bit;
			else surface1.Options &= ~bit;
		}

		void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			surface1.Tool = comboBox1.SelectedItem as Tool;
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

		void Save(object sender, EventArgs e)
		{
			var sfd = new SaveFileDialog() 
			{ 
				RestoreDirectory = true, 
				DefaultExt = ".ags", 
				Filter = "Alien Game Scenario (*.ags)|*.ags",
			};

			if (sfd.ShowDialog() == DialogResult.OK) 
			{
				var w = XmlWriter.Create(sfd.FileName,
					new XmlWriterSettings() { Indent = true });
				surface1.Model.Save(w);
				w.Close();
			}
		}
	}
}
