using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using IjwFramework.Types;

namespace AlienGame
{
	static class RayCast
	{
		public static IEnumerable<Pair<Point, Point>> WallsUnderRay(Point a, Point b, Model m)
		{
			var tiles = TilesUnderRay(a, b);
			var prev = a.ToSquare();

			foreach (var t in tiles)
			{
				var i = MakeDirMask(Math.Sign(t.X - prev.X), Math.Sign(t.Y - prev.Y));
				var mask = m.Cache.Value.GetWallMask(prev.X, prev.Y);
				if (i != 0 && mask != null)
					if ((mask.Value & i) != ((mask.Value >> 2) & i))
						yield return new Pair<Point, Point>(t, prev);
				prev = t;
			}
		}

		static int MakeDirMask(int dx, int dy)
		{
			return new int[] { 0x30, 0x20, 0x21, 0x10, 0, 0x1, 0x12, 0x2, 0x3 }[dx + 3 * dy + 4];
		}

		public static IEnumerable<Point> TilesUnderRay(Point a, Point b)
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
				var q = Lerp(f, a, b).ToSquare();
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
