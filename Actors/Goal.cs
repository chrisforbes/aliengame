using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlienGame.Actors
{
	using Order = Func<Actor, bool>;

	class Goal
	{
		public readonly string Name;
		public readonly Action<Mover> Tick;
		public readonly Action<Mover> MakePlan;

		public Goal(string name, Action<Mover> tick, Action<Mover> makePlan)
		{
			Name = name;
			Tick = tick;
			MakePlan = makePlan;
		}

		public static Goal Face(int direction)
		{
			return new Goal("Face",
				a => { if (a.Direction == direction) a.PopGoal(); },
				a => a.SetOrders(new Order[] { Orders.Face(direction, 1) }));
		}

		public static Goal WalkTo(Point p)
		{
			return new Goal("Walk",
				a => { if (a.Position.ToSquare() == p) a.PopGoal(); },
				a => a.SetOrders(a.PlanPathTo(p)));
		}

		public static Goal Use(Actor b)
		{
			bool completed = false;
			return new Goal("Use",
				a => { if (completed) a.PopGoal(); },
				a => 
				{
					if (a.Position.ToSquare() != b.Position.ToSquare())
						Goal.WalkTo(b.Position.ToSquare()).MakePlan(a);
					else if (a.Direction != b.Direction)
						Goal.Face(b.Direction).MakePlan(a);
					else
						a.SetOrders(new Order[] { Orders.Use(b), 
							_ => { completed = true; return true; } });
				});
		}

		public static Goal FollowWaypoints(Waypoint w)
		{
			return new Goal("FollowWaypoints",
				a => { if (w == null) a.PopGoal(); },	// should contain interruption conditions
				a =>
				{
					if (w != null)
						if (w.Position.ToSquare() != a.Position.ToSquare())
							a.SetOrders(a.PlanPathTo(w.Position.ToSquare()));
						else
							w = w.Next();
				});
		}
	}
}
