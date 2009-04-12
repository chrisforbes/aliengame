using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

namespace AlienGame.Actors
{
	class Food : Mover
	{
		public override void Draw(Graphics g)
		{
			g.DrawRectangle(Pens.Green,
				Position.X - 10, Position.Y - 10, 20, 20);
			g.DrawString("Food\n" + Name, Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);
		}

		public Food() : base() { }
		public Food(XmlElement e) : base(e) { }
	}
}
