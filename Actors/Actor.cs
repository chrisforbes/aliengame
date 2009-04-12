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
	}
}
