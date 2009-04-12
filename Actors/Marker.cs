using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	class Marker : Actor
	{
		public override void Draw(Graphics g)
		{
			g.DrawRectangle(Pens.Orange,
				Position.X - 10, Position.Y - 10, 20, 20);
			g.DrawString("Marker\n" + Name, Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);
		}

		public Marker() : base() { }
		public Marker(XmlElement e) : base(e) { }
	}
}
