using System;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace AlienGame.Actors
{
	using Order = Func<Actor, bool>;

	class Food : Mover
	{
		public override void Draw(Graphics g)
		{
			DrawDirection(g);
			DrawBasicActor(g, Pens.Green);
		}

		public Food(Model m) : base(m) { }
		public Food(Model m, XmlElement e) : base(m,e) { }

		enum AiState { Idle, PanicStart, PanicGoForButton, PanicRun, PanicReplan };

		AiState state = AiState.Idle;

		Order SetState(AiState newState)
		{
			return a => { (a as Food).state = newState; return true; };
		}

		public void Die() { m.RemoveActor(this); }

		public override void Tick()
		{
			base.Tick();

			switch (state)
			{
				case AiState.Idle:
					if (GetVisibleActors().Any(a => a is TestAlien)) state = AiState.PanicStart;
					break;

				case AiState.PanicStart:
					var button = m.GetRoomAt(Position.ToSquare())
						.Actors.OfType<Alarm>().ClosestTo(this);

					if (button != null)
					{
						state = AiState.PanicGoForButton;

						SetOrders(PlanPathTo(button.Position.ToSquare())
							.Concat(
								Orders.Face(button.Direction, 1),
								Orders.Use(button),
								SetState(AiState.PanicReplan)));
					}
					else
						state = AiState.PanicReplan;
					break;

				case AiState.PanicGoForButton:
				case AiState.PanicRun:
					break;	// todo: if we see something (an alien!) that freaks us out more, replan

				case AiState.PanicReplan:
					state = AiState.PanicRun;

					var dest = m.GetRoomAt(Position.ToSquare())
						.ChooseRandomTile();

					SetOrders(PlanPathTo(dest)
						.Concat(SetState(AiState.PanicReplan)));
					break;
			}
		}

		public void Panic( Model m )
		{
			state = AiState.PanicStart;
		}
	}
}
