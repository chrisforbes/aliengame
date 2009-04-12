using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	class Marker : Actor
	{
		public override void Draw(Graphics g) { DrawBasicActor(g, Pens.Orange); }

		public Marker() : base() { }
		public Marker(XmlElement e) : base(e) { }
	}
}
