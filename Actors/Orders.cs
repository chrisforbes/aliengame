using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlienGame
{
	using Order = Func<Actor, Model, bool>;
using AlienGame.Actors;

	static class Orders
	{
		static int TurnToward(int a, int b)
		{
			a %= 8; b %= 8;
			var d = b - a;
			if (d > 4) d -= 8;
			if (d < -4) d += 8;

			return a + Math.Sign(d);
		}

		public static Order Face(int direction, float speed)
		{
			float s = 0;
			direction %= 8;

			return (a, m) =>
			{
				if (a.Direction == direction)
					return true;
				if ((s += speed) >= 1)
				{
					a.Direction = TurnToward(a.Direction, direction);
					s = 0;
				}
				return false;
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
						var otherFoods = m.GetRoomAt(a.Position.ToSquare()).Actors
							.Where(b => b.GetType() == typeof(Food))
							.Cast<Food>();

						foreach (var x in otherFoods)
							x.Panic(m);

						return true;
					}
					else
					{
						((IOrderTarget)a).AcceptOrder(m, f.Position.ToSquare());
						return true;
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
