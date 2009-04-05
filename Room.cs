using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlienGame
{
	class Room
	{
		public List<Brush> brushes = new List<Brush>();

		public Room(Brush firstBrush) { brushes.Add(firstBrush); }
		public bool IntersectsWith(Rectangle r)
		{
			return brushes.Any(b => b.Bounds.IntersectsWith(r));
		}

		public static List<Room> MakeRooms(Model m)
		{
			var rooms = new List<Room>();
			foreach (var b in m.Brushes)
			{
				var rr = rooms.Where(r => r.IntersectsWith(b.Bounds)).ToList();
				switch (rr.Count)
				{
					case 0:
						rooms.Add(new Room(b));
						break;
					case 1:
						rr[0].brushes.Add(b);
						break;
					default:
						rr[0].brushes.Add(b);
						var toRemove = rr.Skip(1);
						rr[0].brushes.AddRange(toRemove.SelectMany(a => a.brushes));
						rooms.Remove(toRemove);
						break;
				}
			}

			return rooms;
		}
	}
}
