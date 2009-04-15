using System;
using System.Drawing;
using System.Linq;
using AlienGame.Actors;

namespace AlienGame
{
	using Order = Func<Actor, Model, bool>;

	static class Orders
	{
		static int TurnToward(int a, int b)
		{
			a %= 8; b %= 8;
			var d = b - a;
			if (d > 4) d -= 8;
			if (d < -4) d += 8;
			return (a + Math.Sign(d) + 8) % 8;
		}

		public static Order Face(int direction, float speed)
		{
			float s = 0;
			direction %= 8;

			return (a, m) =>
			{
				s += speed;
				a.Direction %= 8;

				while (a.Direction != direction && s >= 1)
				{
					a.Direction = TurnToward(a.Direction, direction);
					--s;
				}

				return a.Direction % 8 == direction;
			};
		}

		public static Order Walk(Point p, float speed)
		{
			float fx = 0, fy = 0;

			return (a, m) =>
			{
				var ux = p.X - a.Position.X - fx;
				var uy = p.Y - a.Position.Y - fy;

				var l = (float)Math.Sqrt(ux * ux + uy * uy);
				if (l <= Math.Max(1, speed))
				{
					a.Position = p;
					return true;
				}

				fx += (ux / l) * speed;
				fy += (uy / l) * speed;

				a.Position = new Point(a.Position.X + (int)fx, a.Position.Y + (int)fy);
				fx -= (int)fx;
				fy -= (int)fy;
				return false;
			};
		}

		public static Order Eat(Food f)
		{
			return (a, m) =>
				{
					if (a.Position.ToSquare() == f.Position.ToSquare())
					{
						// got eaten!
						// todo: some animation bs
						m.RemoveActor(f);
						
						// alert all the other dudes!
						var otherFoods = m.GetRoomAt(a.Position.ToSquare()).Actors.OfType<Food>();

						foreach (var x in otherFoods)
							x.Panic(m);

						return true;
					}
					else
					{
						((IOrderTarget)a).AcceptOrder(m, f.Position.ToSquare());
						return false;
					}
				};
		}

		public static Order Use(Alarm b)
		{
			return (a, m) =>
				{
					b.Use(m, a);
					return true;
				};
		}
	}
}
