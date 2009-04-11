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
			w.WriteAttributeString("x", Position.X.ToString());
			w.WriteAttributeString("y", Position.Y.ToString());
			w.WriteAttributeString("kind", Kind.ToString());
			w.WriteAttributeString("state", State.ToString());
			w.WriteEndElement();
		}
	}
}
