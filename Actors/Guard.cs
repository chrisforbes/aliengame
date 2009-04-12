using System;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using System.Linq;

namespace AlienGame.Actors
{
	using Order = Func<Actor,Model,bool>;

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

		public Guard() : base() { Target = "";  }
		public Guard(XmlElement e) : base(e) { Target = e.GetAttribute("target"); }

		enum AiState { Idle, FollowingPath, RespondToAlarm };
		AiState state = AiState.Idle;

		public override void Use(Model m, Actor user)
		{
			// we've just got a tip about an alien at `user`.
			SetOrders(PlanPathTo(m, user.Position.ToSquare()).Concat(
				new Order[] 
				{
					Orders.Use((Alarm)user)
				}));

			state = AiState.RespondToAlarm;

			// todo: use a different planner, or override it for spotting an alien!
			// todo: mill around after reaching target position
			// todo: jitter target position so we dont look dumb when there's more than one dude spawned
			// todo: get bored and go back to the spawner [and unspawn] after a while
		}

		Order SetTarget(string newTarget)
		{
			return (a, m) => { (a as Guard).Target = newTarget; return true; };
		}

		Order SetState(AiState newState)
		{
			return (a, m) => { (a as Guard).state = newState; return true; };
		}

		void PlanToWaypoint(Model m, Waypoint w)
		{
			SetOrders( PlanPathTo( m, w.Position.ToSquare() ).Concat(
				new Order[] 
				{
					SetTarget( w.Target ),
					SetState(AiState.Idle)
				}));

			state = AiState.FollowingPath;
		}

		public override void Tick(Model m)
		{
			base.Tick(m);

			switch (state)
			{
				case AiState.Idle:
					var target = Actor.FindTargets(m, Target)
						.Where(a => a is Waypoint).Cast<Waypoint>().FirstOrDefault();
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
