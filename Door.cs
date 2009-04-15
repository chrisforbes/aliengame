using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace AlienGame
{
	class Door
	{
		public Point Position;
		public int Kind;
		public int State;

		public Door(Point position, int kind, int state)
		{
			Position = position;
			Kind = kind;
			State = state;
		}

		public void Save(XmlWriter w)
		{
			w.WriteStartElement("door");
			w.WriteAttribute("x", Position.X);
			w.WriteAttribute("y", Position.Y);
			w.WriteAttribute("kind", Kind);
			w.WriteAttribute("state", State);
			w.WriteEndElement();
		}

		public Door(XmlElement e)
		{
			Position = new Point(e.GetAttributeInt("x", 0), e.GetAttributeInt("y", 0));
			Kind = e.GetAttributeInt("kind", 0);
			State = e.GetAttributeInt("state", 0);
		}
	}
}
