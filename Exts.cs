using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlienGame
{
	static class RectangleExts
	{
		public static Rectangle Fix(this Rectangle r)
		{
			if (r.Width < 0)
			{
				r.X = r.X + r.Width;
				r.Width = -r.Width;
			}

			if (r.Height < 0)
			{
				r.Y = r.Y + r.Height;
				r.Height = -r.Height;
			}

			++r.Width;
			++r.Height;
			return r;
		}

		public static void Remove<T>(this List<T> t, IEnumerable<T> toRemove)
		{
			t.RemoveAll(x => toRemove.Contains(x));
		}

		public static Rectangle ToPointRect(this Point p)
		{
			return new Rectangle(p.X, p.Y, 1, 1);
		}

		public static Rectangle SquaresToPixels(this Rectangle r)
		{
			return new Rectangle(r.Left * 40, r.Top * 40, r.Width * 40, r.Height * 40);
		}

		public static Point SquareToCenter(this Point p)
		{
			return new Point(p.X * 40 + 20, p.Y * 40 + 20);
		}

		public static Color WithAlpha(this Color c, int alpha)
		{
			return Color.FromArgb(alpha, c);
		}
	}
}
