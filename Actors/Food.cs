using System;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace AlienGame.Actors
{
	using Order = Func<Actor, Model, bool>;

	class Food : Mover
	{
		public override void Draw(Graphics g)
		{
			DrawDirection(g);
			DrawBasicActor(g, Pens.Green);
		}

		public Food() : base() { }
		public Food(XmlElement e) : base(e) { }

		enum AiState { Idle, PanicStart, PanicGoForButton, PanicRun, PanicReplan };

		AiState state = AiState.Idle;

		Order SetState(AiState newState)
		{
			return (a, m) => { (a as Food).state = newState; return true; };
		}

		Alarm FindAlarm( Model m )
		{
			var room = m.GetRoomAt(Position.ToSquare());
			return room.Actors.OfType<Alarm>().ClosestTo(this);
		}

		Point ChooseRandomDestination(Model m)
		{
			var room = m.GetRoomAt(Position.ToSquare());
			return room.ChooseRandomTile();
		}

		public override void Tick(Model m)
		{
			base.Tick(m);

			switch (state)
			{
				case AiState.Idle:
					if (GetVisibleActors(m).Any(a => a is TestAlien)) state = AiState.PanicStart;
					break;

				case AiState.PanicStart:
					var button = FindAlarm(m);
					if (button != null)
					{
						state = AiState.PanicGoForButton;

						SetOrders(PlanPathTo(m, button.Position.ToSquare())
							.Concat(new Order[] 
							{ 
								Orders.Face(button.Direction, 1 ),
								Orders.Use(button),
								SetState(AiState.PanicReplan)
							}));
					}
					else
						state = AiState.PanicReplan;
					break;

				case AiState.PanicGoForButton:
				case AiState.PanicRun:
					break;	// todo: if we see something (an alien!) that freaks us out more, replan

				case AiState.PanicReplan:
					state = AiState.PanicRun;

					var dest = ChooseRandomDestination(m);
					SetOrders(PlanPathTo(m, dest)
						.Concat(new Order[] { SetState(AiState.PanicReplan) }));
					break;
			}
		}

		public void Panic( Model m )
		{
			state = AiState.PanicStart;
		}
	}
}
