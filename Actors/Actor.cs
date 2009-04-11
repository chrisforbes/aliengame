using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Xml;

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
	}
}
