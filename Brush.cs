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
			w.WriteAttributeString("left", Bounds.Left.ToString());
			w.WriteAttributeString("top", Bounds.Top.ToString());
			w.WriteAttributeString("right", Bounds.Right.ToString());
			w.WriteAttributeString("bottom", Bounds.Bottom.ToString());
			w.WriteEndElement();
		}
	}
}
