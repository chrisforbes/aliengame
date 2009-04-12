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

	class Guard : Mover
	{
		public override void Draw(Graphics g)
		{
			g.DrawRectangle(Pens.Fuchsia,
					Position.X - 10, Position.Y - 10, 20, 20);
			g.DrawString("Guard\n" + Name, Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);
		}

		public Guard() : base() { }
		public Guard(XmlElement e) : base(e) { }

		public override void Use(Model m, Actor user)
		{
			// we've just got a tip about an alien at `user`.
			SetOrders(PlanPathTo(m, user.Position));

			// todo: use a different planner, or override it for spotting an alien!
			// todo: mill around after reaching target position
			// todo: jitter target position so we dont look dumb when there's more than one dude spawned
			// todo: get bored and go back to the spawner [and unspawn] after a while
		}
	}
}
