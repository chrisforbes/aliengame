using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace AlienGame
{
	class Brush
	{
		public Rectangle Bounds;

		public Brush(Rectangle bounds)
		{
			Bounds = bounds;
		}

		public void Save(XmlWriter w)
		{
			w.WriteStartElement("brush");
			w.WriteAttribute("left", Bounds.Left);
			w.WriteAttribute("top", Bounds.Top);
			w.WriteAttribute("right", Bounds.Right);
			w.WriteAttribute("bottom", Bounds.Bottom);
			w.WriteEndElement();
		}

		public Brush(XmlElement e)
		{
			Bounds = Rectangle.FromLTRB(
				e.GetAttributeInt("left", 0),
				e.GetAttributeInt("top", 0),
				e.GetAttributeInt("right", 0),
				e.GetAttributeInt("bottom", 0));
		}
	}
}
