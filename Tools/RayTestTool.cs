using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using IjwFramework.Types;

namespace AlienGame.Tools
{
	class RayTestTool : Tool
	{
		Point? start;
		Point? end;

		public override string Name { get { return "Ray Test"; } }

		public override bool OnMouseDown(Surface s, Model m, Point square, Point offset, MouseButtons mb)
		{
			start = square;
			end = square;
			return true;
		}

		public override bool OnMouseMove(Surface s, Model m, Point square, Point offset, MouseButtons mb)
		{
			if (mb == MouseButtons.Left)
			{
				end = square;
				return true;
			}
			return false;
		}

		public override bool OnMouseUp(Surface s, Model m, Point square, Point offset, MouseButtons mb)
		{
			start = null;
			end = null;
			return true;
		}

		public override void DrawToolOverlay(Surface s, Graphics g, Model m)
		{
			if (start == null || end == null)
				return;

			foreach (var t in RayCast.TilesUnderRay(start.Value.SquareToCenter(), end.Value.SquareToCenter()))
				g.DrawRectangle(Pens.Magenta, t.ToPointRect().SquaresToPixels());

			g.DrawRectangle(Pens.LimeGreen, start.Value.ToPointRect().SquaresToPixels());
			g.DrawRectangle(Pens.LimeGreen, end.Value.ToPointRect().SquaresToPixels());

			g.DrawLine(Pens.LimeGreen, start.Value.SquareToCenter(), end.Value.SquareToCenter());

			foreach (var w in RayCast.WallsUnderRay(start.Value.SquareToCenter(), end.Value.SquareToCenter(), m))
				DrawCross(g,
					(0.5f).Lerp(w.First.SquareToCenter(), w.Second.SquareToCenter()));
		}

		static void DrawCross(Graphics g, Point p)
		{
			g.DrawLine(Pens.Red, p.X - 5, p.Y - 5, p.X + 5, p.Y + 5);
			g.DrawLine(Pens.Red, p.X - 5, p.Y + 5, p.X + 5, p.Y - 5);
		}
	}
}
