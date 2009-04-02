using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlienGame
{
	class Model
	{
		public List<Brush> brushes = new List<Brush>();
		public List<Door> doors = new List<Door>();
		public List<Actor> actors = new List<Actor>();

		public bool HasWall(int x1, int y1, int x2, int y2)
		{
			return brushes.Any(
				b => b.Bounds.Contains(x1, y1) || b.Bounds.Contains(x2, y2))
				&& !brushes.Any(
					b => b.Bounds.Contains(x1, y1)
						&& b.Bounds.Contains(x2, y2))
				&& null == HasDoor(x1, y1, x2, y2);
		}

		public bool HasFloor(int x, int y)
		{
			return brushes.Any(b => b.Bounds.Contains(x, y));
		}

		public Door HasDoor(int x1, int y1, int x2, int y2)
		{
			if (x1 != x2 && y1 != y2)
				throw new ArgumentException("wtf; not adjacent cells!");

			var x = Math.Max(x1, x2);
			var y = Math.Max(y1, y2);

			return doors.FirstOrDefault(
				d => d.Position == new Point(x, y)
					&& d.Kind == ((x1 == x2) ? 1 : 0));
		}

		public void Tick()
		{
			foreach (var a in actors)
				a.Tick(this);
		}
	}
}
