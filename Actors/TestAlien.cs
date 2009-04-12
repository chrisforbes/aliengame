using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	using Order = Func<Actor, Model, bool>;

	interface IOrderTarget
	{
		void AcceptOrder(Model m, Point targetSquare);
	}

	class TestAlien : Mover, IOrderTarget
	{
		public override void Draw(Graphics g)
		{
			g.DrawRectangle(Pens.Blue,
				Position.X - 10, Position.Y - 10, 20, 20);

			DrawDirection(g);

			g.DrawString("TestAlien\n" + Name, 
				Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);
		}

		public void AcceptOrder(Model m, Point targetSquare)
		{
			var food = m.ActorsAt(targetSquare)
				.Where(a => a is Food)
				.Cast<Food>().FirstOrDefault();

			if (food == null)
				this.SetOrders(PlanPathTo(m, targetSquare));
			else
				this.SetOrders(PlanToEat(m, food, targetSquare));
		}

		public IEnumerable<Order> PlanToEat(Model m, Food f, Point to)
		{
			return PlanPathTo(m, to).Concat(new Order[] { Orders.Eat(f) });
		}

		public TestAlien() : base() { }
		public TestAlien(XmlElement e) : base(e) { }
	}
}
