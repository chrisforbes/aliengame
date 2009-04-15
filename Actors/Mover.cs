using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Drawing;
using IjwFramework.Types;

namespace AlienGame.Actors
{
	using Order = Func<Actor, bool>;

	abstract class Mover : Actor
	{
		Stack<Goal> goals = new Stack<Goal>();

		public void PushGoal(Goal g)
		{
			goals.Push(g);
			g.MakePlan(this);
		}

		public void PopGoal()
		{
			goals.Pop();
			if (goals.Count > 0)
				goals.Peek().MakePlan(this);
			else
				CancelOrders();
		}

		public void ReplaceGoal(Goal g)
		{
			goals.Pop();
			PushGoal(g);
		}

		public bool HasAnyGoals() { return goals.Count > 0; }

		List<Order> orders = new List<Order>();

		public void CancelOrders() { orders.Clear(); }
		public void SetOrders(IEnumerable<Order> orders)
		{
			this.orders = orders.ToList();
		}

		public override void Tick()
		{
			// goal-based planner!
			if (goals.Count > 0)
			{
				var g = goals.Peek();
				for(;;)
				{
					g.Tick(this);
					if (goals.Count == 0 || goals.Peek() == g)
						break;
					g = goals.Peek();
				}
			}

			while (orders.Count > 0 && orders[0](this))
				orders.RemoveAt(0);

			if (orders.Count == 0 && goals.Count > 0)
				goals.Peek().MakePlan(this);
		}

		protected Mover(Model m) : base(m) { }
		protected Mover(Model m, XmlElement e) : base(m, e) { }

		public IEnumerable<Order> PlanPathTo(Point to)
		{
			var from = Position.ToSquare();
			var pf = new Pathfinder(m);

			var path = pf.FindPath(from, to).ToList();
			for (int i = path.Count - 1; i >= 0; i--)
			{
				var walkTo = path[i].SquareToCenter();
				yield return Orders.Face(MakeDirection(from, path[i], 0), 1);
				yield return Orders.Walk(walkTo, 6);
				from = path[i];
			}
		}
	}
}
