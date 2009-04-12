using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace AlienGame.Actors
{
	class Alarm : Actor
	{
		[Browsable(true), Category("Alarm")]
		public string Target { get; set; }
		public bool triggered;

		public override void Draw(Graphics g)
		{
			DrawPointActor(g, Pens.Orange);

			if (triggered)
				g.DrawString("Triggered !!", Form1.font, Brushes.White, Position.X - 8, Position.Y + 16);
		}

		public override void DrawOverlay(Model m, Graphics g)
		{
			foreach (var a in Actor.FindTargets(m, Target))
				g.DrawLine(arrowPen, Position, a.Position);
		}

		public Alarm() : base() { Target = ""; }
		public Alarm(XmlElement e) : base(e) { Target = e.GetAttribute("target"); }

		protected override void SaveAttributes(XmlWriter w)
		{
			base.SaveAttributes(w);
			w.WriteAttribute("target", Target);
		}

		public override void Use(Model m, Actor usedBy)
		{
			if (!triggered)
			{
				triggered = true;
				foreach( var a in Actor.FindTargets(m, Target) )
					a.Use(m, this);
			}
		}
	}
}
