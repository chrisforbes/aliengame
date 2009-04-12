using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	class GuardSpawner : Actor
	{
		public override void Draw(Graphics g)
		{
			g.DrawRectangle(Pens.Orange,
					Position.X - 10, Position.Y - 10, 20, 20);
			g.DrawString("GuardSpawner\n" + Name, Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);
		}

		public GuardSpawner() : base() { }
		public GuardSpawner(XmlElement e) : base(e) { }

		public override void Use(Model m, Actor user)
		{
			var a = new Guard() { Position = this.Position, Direction = this.Direction };
			m.AddActor(a);
			a.Use(m, user);
		}
	}
}
