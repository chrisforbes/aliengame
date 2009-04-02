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
	}
}
