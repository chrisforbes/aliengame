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
			g.DrawString("TestAlien\n" + Name, 
				Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);
		}

		public void AcceptOrder(Model m, Point targetSquare)
		{
			var food = m.ActorsAt(targetSquare)
				.Where(a => a.GetType() == typeof(Food))
				.Cast<Food>().FirstOrDefault();

			if (food == null)
				this.SetOrders(PlanPathTo(targetSquare));
			else
				this.SetOrders(PlanToEat(food, targetSquare));
		}

		public IEnumerable<Order> PlanPathTo(Point to)
		{
			var from = new Point( Position.X / 40, Position.Y / 40 );
			var pf = Pathfinder.Instance;

			var path = pf.FindPath( from, to ).ToList();
			for( int i = path.Count - 1 ; i >= 0 ; i-- )
			{
				var walkTo = new Point( path[ i ].X * 40 + 20, path[ i ].Y * 40 + 20 );
				yield return Orders.Walk( walkTo, 6 );
			}
		}

		public IEnumerable<Order> PlanToEat(Food f, Point to)
		{
			return PlanPathTo(to).Concat(new Order[] { Orders.Eat(f) });
		}

		public TestAlien() : base() { }
		public TestAlien(XmlElement e) : base(e) { }
	}
}
