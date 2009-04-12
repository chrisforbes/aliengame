using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace AlienGame
{
	class Model
	{
		List<Brush> brushes = new List<Brush>();
		public List<Door> doors = new List<Door>();
		List<Actor> actors = new List<Actor>();

		List<Actor> addedActors = new List<Actor>();
		List<Actor> removedActors = new List<Actor>();

		List<Room> rooms = new List<Room>();

		public void AddBrush(Brush b) { brushes.Add(b); rooms = Room.MakeRooms(this); }
		public void RemoveBrush(Brush b) { brushes.Remove(b); rooms = Room.MakeRooms(this); }

		public IEnumerable<Brush> Brushes { get { return brushes; } }

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

			SyncActorList();
		}

		public void AddActor(Actor a) { addedActors.Add(a); }
		public void RemoveActor(Actor a) { if (!removedActors.Contains(a)) removedActors.Add(a); }

		public void SyncActorList()
		{
			actors.AddRange(addedActors);
			actors.RemoveAll(a => removedActors.Contains(a));
			addedActors.Clear();
			removedActors.Clear();
		}

		public IEnumerable<Actor> Actors { get { return actors; } }

		public IEnumerable<Actor> ActorsAt(Point p)
		{
			return Actors.Where(a => p == new Point(a.Position.X / 40, a.Position.Y / 40));
		}

		public IEnumerable<Brush> GetBrushesAt( Point p )
		{
			return brushes.Where( x => x.Bounds.Contains( p ) );
		}

		public IEnumerable<Point> GetDoorsEndFrom( Point location )
		{
			if( null != HasDoor( location.X, location.Y, location.X + 1, location.Y ) )
				yield return new Point( location.X + 1, location.Y );
			if( null != HasDoor( location.X, location.Y, location.X - 1, location.Y ) )
				yield return new Point( location.X - 1, location.Y );
			if( null != HasDoor( location.X, location.Y, location.X, location.Y + 1 ) )
				yield return new Point( location.X, location.Y + 1 );
			if( null != HasDoor( location.X, location.Y, location.X, location.Y - 1 ) )
				yield return new Point( location.X, location.Y - 1 );
		}

		public Room GetRoomAt(Point p)
		{
			return rooms.FirstOrDefault(
				r => r.brushes.Any(b => b.Bounds.Contains(p)));
		}

		public void Save(XmlWriter w)
		{
			SyncActorList();

			w.WriteStartElement("model");

			foreach (var b in brushes) b.Save(w);
			foreach (var d in doors) d.Save(w);
			foreach (var a in actors) a.Save(w);

			w.WriteEndElement();
		}

		public Model() {}

		public Model(XmlDocument doc)
		{
			// NB: actor loading is different, because the actual type of the resulting object varies.

			brushes = doc.SelectNodes("//brush").Cast<XmlElement>().Select(e => new Brush(e)).ToList();
			doors = doc.SelectNodes("//door").Cast<XmlElement>().Select(e => new Door(e)).ToList();
			actors = doc.SelectNodes("//actor").Cast<XmlElement>().Select(e => Actor.Load(e)).ToList();

			rooms = Room.MakeRooms(this);
		}
	}
}
