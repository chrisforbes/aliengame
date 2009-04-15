using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace AlienGame
{
	using DBrush = System.Drawing.Brush;

	abstract class Actor
	{
		[Browsable(true), Category("Actor")]
		public Point Position { get; set; }
		[Browsable(true), Category("Actor")]
		public int Direction { get; set; }
		[Browsable(true), Category("Actor")]
		public string Name { get; set; }

		public Model m;

		protected Actor(Model m) { this.m = m; Name = ""; }

		public virtual void Draw(Graphics g) { }
		public virtual void DrawOverlay(Graphics g) { }
		public virtual void Tick() { }
		public virtual void Use(Actor user) { }

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

		protected Actor(Model m, XmlElement e)
		{
			this.m = m;
			Position = new Point(e.GetAttributeInt("x", 0),
								 e.GetAttributeInt("y", 0));
			Direction = e.GetAttributeInt("dir", 0);
			Name = e.GetAttribute("name");
		}

		public static Actor Load(Model m, XmlElement e)
		{
			var className = e.GetAttribute("class");
			var type = Assembly.GetExecutingAssembly().GetType(className);
			var ctor = type.GetConstructor(new Type[] { typeof(Model), typeof(XmlElement) });
			return (Actor)ctor.Invoke(new object[] { m, e });
		}

		protected static Pen arrowPen = new Pen(Color.Orange) { EndCap = LineCap.ArrowAnchor };

		public IEnumerable<Actor> FindTargets(string name)
		{
			return m.Actors.Where(x => x.Name == name);
		}

		public static int MakeDirection(Point from, Point to, int def)
		{
			var dx = Math.Sign(to.X - from.X);
			var dy = Math.Sign(to.Y - from.Y);
			var dirs = new int[] { 5, 6, 7, 4, def, 0, 3, 2, 1 };
			return dirs[3 * dy + dx +4];
		}

		DBrush visionBrush = new SolidBrush(Color.Yellow.WithAlpha(128));

		protected void DrawDirection(Graphics g)
		{
			g.FillPie(visionBrush, new Rectangle(Position.X - 20, Position.Y - 20, 40, 40),
				Direction * 45 - 45, 90);
		}

		protected void DrawBasicActor(Graphics g, Pen p)
		{
			g.DrawRectangle(p, Position.X - 10, Position.Y - 10, 20, 20);
			g.DrawString(GetType().Name + "\n" + Name, Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);
		}

		protected void DrawPointActor(Graphics g, Pen p)
		{
			g.DrawLine(p, Position.X - 10, Position.Y, Position.X + 10, Position.Y);
			g.DrawLine(p, Position.X, Position.Y - 10, Position.X, Position.Y + 10);
			g.DrawString(GetType().Name + "\n" + Name, Form1.font, Brushes.White, Position.X - 8, Position.Y - 8);
		}

		public IEnumerable<Actor> GetVisibleActors()
		{
			// just actors in this room!
			return m.GetRoomAt(Position.ToSquare()).Actors.Where(CanSee);
		}

		public bool CanSee(Actor other)
		{
			// todo: real occlusion! this is just a cone test
			var dir = MakeDirection(Position.ToSquare(), other.Position.ToSquare(), Direction);
			return (dir == Direction || (dir + 1) % 8 == Direction || (dir + 7) % 8 == Direction);
		}
	}
}
