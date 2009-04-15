using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

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

		public static void WriteAttribute(this XmlWriter w, string name, string value)
		{
			w.WriteAttributeString(name, value);
		}

		public static void WriteAttribute(this XmlWriter w, string name, int value)
		{
			w.WriteAttributeString(name, value.ToString());
		}

		public static void WriteAttribute(this XmlWriter w, string name, float value)
		{
			w.WriteAttributeString(name, value.ToString());
		}

		public static int GetAttributeInt(this XmlElement e, string name, int def)
		{
			int val = def;
			return int.TryParse(e.GetAttribute(name), out val) ? val : def;
		}

		public static float GetAttributeFloat(this XmlElement e, string name, float def)
		{
			float val = def;
			return float.TryParse(e.GetAttribute(name), out val) ? val : def;
		}

		public static Point ToSquare(this Point p)
		{
			return new Point(p.X / 40, p.Y / 40);
		}

		public static int DistanceSqTo(this Point p, Point q)
		{
			var dx = p.X - q.X;
			var dy = p.Y - q.Y;
			return dx * dx + dy * dy;
		}

		public static T ClosestTo<T>(this IEnumerable<T> ts, Actor a)
			where T : Actor
		{
			return ts.OrderBy(t => t.Position.DistanceSqTo(a.Position))
				.FirstOrDefault();
		}

		public static IEnumerable<T> Concat<T>(this IEnumerable<T> xs, params T[] ys)
		{
			return xs.Concat((IEnumerable<T>)ys);
		}
	}
}
