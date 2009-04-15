using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	class Marker : Actor
	{
		public override void Draw(Graphics g) { DrawBasicActor(g, Pens.Orange); }

		public Marker(Model m) : base(m) { }
		public Marker(Model m, XmlElement e) : base(m,e) { }
	}
}
