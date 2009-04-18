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

			foreach (var t in TilesUnderRay(start.Value.SquareToCenter(), end.Value.SquareToCenter()))
				g.DrawRectangle(Pens.Magenta, t.ToPointRect().SquaresToPixels());

			g.DrawRectangle(Pens.LimeGreen, start.Value.ToPointRect().SquaresToPixels());
			g.DrawRectangle(Pens.LimeGreen, end.Value.ToPointRect().SquaresToPixels());

			g.DrawLine(Pens.LimeGreen, start.Value.SquareToCenter(), end.Value.SquareToCenter());

			foreach (var w in WallsUnderRay(start.Value.SquareToCenter(), end.Value.SquareToCenter(), m))
				DrawCross(g,
					Lerp(0.5f, w.First.SquareToCenter(), w.Second.SquareToCenter()));
		}

		static void DrawCross(Graphics g, Point p)
		{
			g.DrawLine(Pens.Red, p.X - 5, p.Y - 5, p.X + 5, p.Y + 5);
			g.DrawLine(Pens.Red, p.X - 5, p.Y + 5, p.X + 5, p.Y - 5);
		}

		static IEnumerable<Pair<Point, Point>> WallsUnderRay(Point a, Point b, Model m)
		{
			var tiles = TilesUnderRay(a, b);
			var prev = a.ToSquare();

			foreach (var t in tiles)
			{
				var i = MakeDirIndex(t.X - prev.X, t.Y - prev.Y);
				var mask = m.Cache.Value.GetWallMask(prev.X, prev.Y);
				if (i != null && mask != null)
					if ((mask.Value & (5 << i)) == (1 << i))
						yield return new Pair<Point, Point>(t, prev);
				prev = t;
			}
		}

		static int? MakeDirIndex(int dx, int dy)
		{
			if (dx == 1 && dy == 0) return 0;
			if (dx == 0 && dy == 1) return 1;
			if (dx == -1 && dy == 0) return 4;
			if (dx == 0 && dy == -1) return 5;
			return null;
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
				var q = Lerp(f,a,b).ToSquare();

				if (p != q) yield return p = q;
			}
		}

		static Point Lerp(float t, Point a, Point b)
		{
			return new Point(
					(int)((1 - t) * a.X + t * b.X),
					(int)((1 - t) * a.Y + t * b.Y));
		}
	}
}
