using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Xml;
using System.Windows.Forms;

namespace AlienGame.Actors
{
	class Alarm : Actor
	{
		[Browsable(true), Category("Alarm")]
		public string Target { get; set; }
		public bool triggered;

		public override void Draw(Graphics g)
		{
			g.DrawRectangle(Pens.Orange,
				Position.X - 10, Position.Y - 10, 20, 20);
			g.DrawString("Alarm\n" + Name, Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);

			if (triggered)
				g.DrawString("Triggered !!", Form1.font, Brushes.White, Position.X - 8, Position.Y + 16);
		}

		public Alarm() : base() { Target = ""; }
		public Alarm( XmlElement e ) : base(e) {}

		protected override void SaveAttributes(XmlWriter w)
		{
			base.SaveAttributes(w);
			w.WriteAttribute("target", Target);
		}

		public void Use(Model m, Actor usedBy)
		{
			triggered = true;
		}
	}
}
