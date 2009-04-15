using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.Linq;

namespace AlienGame.Actors
{
	class Waypoint : Actor
	{
		[Browsable(true), Category("Waypoint")]
		public string Target { get; set; }

		public override void Draw(Graphics g) { DrawPointActor(g, Pens.Green); }
		public override void DrawOverlay(Graphics g)
		{
			foreach( var a in FindTargets( Target ))
				g.DrawLine(arrowPen, Position, a.Position);
		}

		protected override void SaveAttributes(XmlWriter w)
		{
			base.SaveAttributes(w);
			w.WriteAttribute("target", Target);
		}

		public Waypoint(Model m) : base(m) { Target = ""; }
		public Waypoint(Model m, XmlElement e) : base(m,e) { Target = e.GetAttribute("target"); }

		public Waypoint Next()
		{
			return FindTargets(Target).OfType<Waypoint>().FirstOrDefault();
		}
	}
}
