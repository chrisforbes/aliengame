using System;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace AlienGame.Actors
{
	using Order = Func<Actor,bool>;

	class Guard : Mover
	{
		[Browsable(true), Category("Guard")]
		public string Target { get; set; }

		public override void Draw(Graphics g)
		{
			DrawDirection(g);
			DrawBasicActor(g, Pens.Fuchsia);
		}

		protected override void SaveAttributes(XmlWriter w)
		{
			base.SaveAttributes(w);
			w.WriteAttribute("target", Target);
		}

		public Guard(Model m) : base(m) { Target = "";  }
		public Guard(Model m, XmlElement e) : base(m,e) { Target = e.GetAttribute("target"); }

		enum AiState { Idle, FollowingPath, RespondToAlarm };
		AiState state = AiState.Idle;

		public override void Use(Actor user)
		{
			var initialPosition = Position.ToSquare();

			// we've just got a tip about an alien at `user`.
			SetOrders(PlanPathTo(user.Position.ToSquare())
				.Concat(
					Orders.Use((Alarm)user),
					// todo: in here, do some kind of `securing the area` behavior
					a =>		// omfg this is an ugly hack!
					{
						SetOrders(PlanPathTo(initialPosition).Concat(Orders.Use(
							m.ActorsAt(initialPosition).OfType<GuardSpawner>().First()))); return false;
					}));

			state = AiState.RespondToAlarm;
		}

		Order SetTarget(string newTarget)
		{
			return a => { (a as Guard).Target = newTarget; return true; };
		}

		Order SetState(AiState newState)
		{
			return a => { (a as Guard).state = newState; return true; };
		}

		void PlanToWaypoint(Model m, Waypoint w)
		{
			SetOrders( PlanPathTo( w.Position.ToSquare() )
				.Concat(
					SetTarget(w.Target),
					SetState(AiState.Idle)));

			state = AiState.FollowingPath;
		}

		public override void Tick()
		{
			base.Tick();

			switch (state)
			{
				case AiState.Idle:
					var target = FindTargets(Target).OfType<Waypoint>().FirstOrDefault();
					if (target != null)
						PlanToWaypoint(m, target);
					break;

				case AiState.FollowingPath:
				case AiState.RespondToAlarm:
					break;	// todo  replan if we see stuff
			}
		}
	}
}
