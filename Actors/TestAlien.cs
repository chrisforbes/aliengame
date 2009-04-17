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

			RemoveAllGoals();
			PushGoal((food == null) ? Goal.WalkTo(targetSquare) : Goal.Eat(food));
		}

		public TestAlien(Model m) : base(m) { }
		public TestAlien(Model m, XmlElement e) : base(m,e) { }
	}
}
