using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AlienGame.Tools
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
			var z = q.ToPointRect().SquaresToPixels();
			g.DrawRectangle(Pens.Green, z);
		}

		public override bool OnMouseDown( Surface s, Model m, Point square, Point offset, MouseButtons mb )
		{
			var a = Activator.CreateInstance(NewActorType, m) as Actor;
			a.Position = q.SquareToCenter();
			m.AddActor(a);
			m.SyncActorList();
			return true;
		}
	}
}
