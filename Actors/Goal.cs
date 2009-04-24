using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlienGame.Actors
{
	using Order = Func<Mover, bool>;

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
				a => { if (a.Position == p.SquareToCenter()) a.PopGoal(); },
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

		public static Goal Eat(Actor b)
		{
			Point plannedDestination = new Point();
			bool isLunging = false;
			const float lungeThreshold = 4;

			return new Goal("Eat",
				a => 
				{
					if (!a.m.Actors.Contains(b)) { a.PopGoal(); return; }
					if (b.Position.ToSquare() != plannedDestination)
						a.CurrentGoal().MakePlan(a);
					if (a.Position.ToSquare().DistanceSqTo(plannedDestination) <= lungeThreshold)
						a.CurrentGoal().MakePlan(a);
				},
				a =>
				{
					if (isLunging) return;

					plannedDestination = b.Position.ToSquare();
					if (a.Position.ToSquare().DistanceSqTo(plannedDestination) > lungeThreshold)
						Goal.WalkTo(plannedDestination).MakePlan(a);
					else
					{
						isLunging = true; 
						a.SetOrders(new Order[] { Orders.Lunge(b as Food, 0.3f) });
					}
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
						{
							w = w.Next();
							if (w != null && a is Guard)
								(a as Guard).Target = w.Name;	// for the level designer's benefit
						}
				});
		}

		public static Goal Hunt(Actor t)
		{
			var isRunning = false;
			return new Goal("Hunt",
				a => 
				{ 
					// is the target dead?
					if (!a.m.Actors.Contains(t)) { a.PopGoal(); return; };

					// did we enter/leave visibility?
					if (a.CanSee(t) ^ isRunning)
						a.CurrentGoal().MakePlan(a);
				},
				a =>
				{
					// todo
					//if (a.CanSee(t))
				});
		}
	}
}
