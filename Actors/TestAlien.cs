using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace AlienGame.Actors
{
	using Order = Func<Actor, bool>;

	interface IOrderTarget
	{
		void AcceptOrder(Point targetSquare);
	}

	class TestAlien : Mover, IOrderTarget
	{
		public override void Draw(Graphics g)
		{
			DrawDirection(g);
			DrawBasicActor(g, Pens.Blue);
		}

		public void AcceptOrder(Point targetSquare)
		{
			var food = m.ActorsAt(targetSquare).OfType<Food>().FirstOrDefault();

			if (food == null)
				SetOrders(PlanPathTo(targetSquare));
			else
				SetOrders(PlanToEat(food, targetSquare));
		}

		public IEnumerable<Order> PlanToEat(Food f, Point to)
		{
			return PlanPathTo(to).Concat(Orders.Eat(f));
		}

		public TestAlien(Model m) : base(m) { }
		public TestAlien(Model m, XmlElement e) : base(m,e) { }
	}
}
