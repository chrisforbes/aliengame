using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	class GuardSpawner : Actor
	{
		public override void Draw(Graphics g) { DrawPointActor(g, Pens.Orange); }

		public GuardSpawner(Model m) : base(m) { }
		public GuardSpawner(Model m, XmlElement e) : base(m,e) { }

		public override void Use(Actor user)
		{
			var a = new Guard(m) { Position = this.Position, Direction = this.Direction };
			m.AddActor(a);
			a.Use(user);
		}
	}
}
