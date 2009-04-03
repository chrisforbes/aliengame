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
		public override string Name { get { return "Place Actors"; }}
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
			a.Position.X = q.X * 40 + 20;
			a.Position.Y = q.Y * 40 + 20;
			m.AddActor(a);
			m.SyncActorList();
			return true;
		}
	}
}
