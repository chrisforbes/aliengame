using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Xml;
using System.Reflection;

namespace AlienGame
{
	abstract class Actor
	{
		[Browsable(true), Category("Actor")]
		public Point Position { get; set; }
		[Browsable(true), Category("Actor")]
		public int Direction { get; set; }
		[Browsable(true), Category("Actor")]
		public string Name { get; set; }

		protected Actor() { Name = ""; }

		public virtual void Draw(Graphics g) { }
		public virtual void DrawOverlay(Graphics g) { }
		public virtual void Tick(Model m) { }
		public virtual void Use(Model m, Actor user) { }

		public void Save(XmlWriter w)
		{
			w.WriteStartElement("actor");
			SaveAttributes(w);
			w.WriteEndElement();
		}

		protected virtual void SaveAttributes(XmlWriter w)
		{
			w.WriteAttribute("class", GetType().ToString());
			w.WriteAttribute("x", Position.X);
			w.WriteAttribute("y", Position.Y);
			w.WriteAttribute("dir", Direction);
			w.WriteAttribute("name", Name);
		}

		protected Actor(XmlElement e)
		{
			Position = new Point(e.GetAttributeInt("x"),
								 e.GetAttributeInt("y"));
			Direction = e.GetAttributeInt("dir");
			Name = e.GetAttribute("name");
		}

		public static Actor Load(XmlElement e)
		{
			var className = e.GetAttribute("class");
			var type = Assembly.GetExecutingAssembly().GetType(className);
			var ctor = type.GetConstructor(new Type[] { typeof(XmlElement) });
			return (Actor)ctor.Invoke(new object[] { e });
		}

		public static void UseTargets(Model m, Actor user, string name)
		{
			foreach (var a in m.Actors.Where(x => x.Name == name))
				a.Use(m, user);
		}

		public static int MakeDirection(Point from, Point to, int def)
		{
			var dx = Math.Sign(to.X - from.X);
			var dy = Math.Sign(to.Y - from.Y);
			var dirs = new int[] { 5, 6, 7, 4, def, 0, 3, 2, 1 };
			return dirs[3 * dy + dx +4];
		}

		System.Drawing.Brush visionBrush = new SolidBrush(Color.Yellow.WithAlpha(128));

		protected void DrawDirection(Graphics g)
		{
			g.FillPie(visionBrush, new Rectangle(Position.X - 20, Position.Y - 20, 40, 40),
				Direction * 45 - 45, 90);
		}

		public IEnumerable<Actor> GetVisibleActors(Model m)
		{
			// just actors in this room!
			return m.GetRoomAt(Position.ToSquare()).Actors
				.Where(a => CanSee(a));
		}

		public bool CanSee(Actor other)
		{
			// todo: real occlusion! this is just a cone test
			var dir = MakeDirection(Position.ToSquare(), other.Position.ToSquare(), Direction);
			return (dir == Direction || (dir + 1) % 8 == Direction || (dir + 7) % 8 == Direction);
		}
	}
}
