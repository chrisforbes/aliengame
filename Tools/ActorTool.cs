using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AlienGame
{
	class ActorTool : Tool
	{
		public override string Name { get { return "Place Actors"; } }
		Point q;
		public static Type NewActorType;

		public override bool OnMouseMove( Surface s, Model m, Point square, Point offset, MouseButtons mb )
		{
			q = square;
			return true;
		}

		public override void DrawToolOverlay(Surface s, Graphics g, Model m)
		{
			var z = new Rectangle(q.X * 40, q.Y * 40, 40, 40);
			g.DrawRectangle(Pens.Green, z);
		}

		public override bool OnMouseDown( Surface s, Model m, Point square, Point offset, MouseButtons mb )
		{
			var a = Activator.CreateInstance(NewActorType) as Actor;
			a.Position = new Point(q.X * 40 + 20, q.Y * 40 + 20);
			m.AddActor(a);
			m.SyncActorList();
			return true;
		}
	}

	class EditActorTool : Tool
	{
		public override string Name { get { return "Edit Actors"; } }
		public static PropertiesForm Ui;

		public override bool OnMouseDown(Surface s, Model m, Point square, Point offset, MouseButtons mb)
		{
			var q = square;
			if (mb == MouseButtons.Left)
			{
				Ui.SetActor( ActorAt( m, q ));
				return true;
			}

			if (mb == MouseButtons.Right)
			{
				var a = ActorAt(m, q );
				if (a != null)
					m.RemoveActor( a );
				m.SyncActorList();
				return true;
			}

			return false;
		}

		static Actor ActorAt(Model m, Point p)
		{
			return m.Actors.FirstOrDefault(
				a => p == new Point(a.Position.X / 40, a.Position.Y / 40));
		}
	}
}
