using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	using Order = Func<Actor, Model, bool>;

	interface IOrderTarget
	{
		void AcceptOrder(Point targetSquare);
	}

	class TestAlien : Mover, IOrderTarget
	{
		public override void Draw(Graphics g)
		{
			g.DrawRectangle(Pens.Blue,
				Position.X - 10, Position.Y - 10, 20, 20);
			g.DrawString("TestAlien\n" + Name, 
				Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);
		}

		public void AcceptOrder(Point targetSquare)
		{
			// todo: non-move orders
			this.SetOrders(PlanPathTo(targetSquare));
		}

		public IEnumerable<Order> PlanPathTo(Point to)
		{
			var from = new Point( Position.X / 40, Position.Y / 40 );
			var pf = Pathfinder.Instance;

			var path = pf.FindPath( from, to ).ToList();
			for( int i = path.Count - 1 ; i >= 0 ; i-- )
			{
				var walkTo = new Point( path[ i ].X * 40 + 20, path[ i ].Y * 40 + 20 );
				yield return Orders.Walk( walkTo, 6 );
			}
			
			//while (a != p)
			//{
			//    var da = p.X - a.X;
			//    var db = p.Y - a.Y;

			//    a.Offset(Math.Sign(da), Math.Sign(db));
			//    yield return Orders.Walk(new Point(a.X * 40 + 20, a.Y * 40 + 20), 4);
			//}
		}

		public TestAlien() : base() { }
		public TestAlien(XmlElement e) : base(e) { }
	}
}
