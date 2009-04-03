using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlienGame
{
	partial class PropertiesForm : Form
	{
		Action onChange;

		public PropertiesForm( Action onChange )
		{
			InitializeComponent();
			this.onChange = onChange;
		}

		public void SetActor(Actor a)
		{
			if (a == null)
			{
				Visible = false;
				return;
			}

			propertyGrid1.SelectedObject = a;
			Text = string.Format("{0} ({1})", a.Name, a.GetType().Name);
			Visible = true;
		}

		void ValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			SetActor(propertyGrid1.SelectedObject as Actor);
			onChange();
		}

		void PropertiesForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}
	}
}
