using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	class Waypoint : Actor
	{
		[Browsable(true), Category("Waypoint")]
		public string Target { get; set; }

		public override void Draw(Graphics g) { DrawPointActor(g, Pens.Green); }
		public override void DrawOverlay(Model m, Graphics g)
		{
			foreach( var a in Actor.FindTargets( m, Target ))
				g.DrawLine(arrowPen, Position, a.Position);
		}

		protected override void SaveAttributes(XmlWriter w)
		{
			base.SaveAttributes(w);
			w.WriteAttribute("target", Target);
		}

		public Waypoint() : base() { Target = ""; }
		public Waypoint(XmlElement e) : base(e) { Target = e.GetAttribute("target"); }
	}
}
