using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

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

			foreach (var t in TilesUnderRay(start.Value.SquareToCenter(), end.Value.SquareToCenter()))
				g.DrawRectangle(Pens.Magenta, t.ToPointRect().SquaresToPixels());

			g.DrawRectangle(Pens.LimeGreen, start.Value.ToPointRect().SquaresToPixels());
			g.DrawRectangle(Pens.LimeGreen, end.Value.ToPointRect().SquaresToPixels());
		}

		static IEnumerable<Point> TilesUnderRay(Point a, Point b)
		{
			if (a == b)
			{
				yield return a;
				yield break;
			}

			var dist = Math.Sqrt(a.DistanceSqTo(b));
			var frac = 1.0f / (float)dist;

			var p = a.ToSquare();
			yield return p;
			for (var f = 0.0f; f <= 1.0f; f += frac)
			{
				var q = new Point( 
					(int)((1 - f) * a.X + f * b.X),
					(int)((1 - f) * a.Y + f * b.Y)).ToSquare();

				if (p != q) yield return p = q;
			}
		}
	}
}
