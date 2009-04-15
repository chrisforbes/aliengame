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

		public override void DrawOverlay(Graphics g)
		{
			foreach (var a in FindTargets(Target))
				g.DrawLine(arrowPen, Position, a.Position);
		}

		public Alarm(Model m) : base(m) { Target = ""; }
		public Alarm(Model m, XmlElement e) : base(m,e) { Target = e.GetAttribute("target"); }

		protected override void SaveAttributes(XmlWriter w)
		{
			base.SaveAttributes(w);
			w.WriteAttribute("target", Target);
		}

		public override void Use(Actor usedBy)
		{
			if (!(usedBy is Guard) && !triggered)	// food turns the alarm on. this also allows chaining.
			{
				triggered = true;
				foreach( var a in FindTargets(Target) )
					a.Use(this);
			}

			if (usedBy is Guard && triggered)	// guard turns the alarm off
				triggered = false;
		}
	}
}
