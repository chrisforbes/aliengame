using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	class GuardSpawner : Actor
	{
		public override void Draw(Graphics g) { DrawPointActor(g, Pens.Orange); }

		public GuardSpawner() : base() { }
		public GuardSpawner(XmlElement e) : base(e) { }

		public override void Use(Model m, Actor user)
		{
			var a = new Guard() { Position = this.Position, Direction = this.Direction };
			m.AddActor(a);
			a.Use(m, user);
		}
	}
}
