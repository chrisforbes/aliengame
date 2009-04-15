using System.Drawing;
using System.Xml;
using System.ComponentModel;

namespace AlienGame.Actors
{
	class GuardSpawner : Actor
	{
		[Browsable(true), Category("GuardSpawner")]
		public int NumGuards { get; set; }
		[Browsable(true), Category("GuardSpawner")]
		public int MaxGuards { get; set; }
		[Browsable(true), Category("GuardSpawner")]
		public float RespawnRate { get; set; }

		public override void Draw(Graphics g)
		{
			DrawPointActor(g, Pens.Orange);
			g.DrawString(string.Format("{0}/{1} {2:F3}",
				NumGuards, MaxGuards, RespawnRate), Form1.font,
				Brushes.White, Position.X - 8, Position.Y + 24);
		}

		public GuardSpawner(Model m) : base(m) { NumGuards = 2; MaxGuards = 2; RespawnRate = 0; }
		public GuardSpawner(Model m, XmlElement e)
			: base(m, e)
		{
			NumGuards = e.GetAttributeInt("numguards", 2);
			MaxGuards = e.GetAttributeInt("maxguards", 2);
			RespawnRate = e.GetAttributeFloat("respawnrate", 0);
		}

		protected override void SaveAttributes(XmlWriter w)
		{
			base.SaveAttributes(w);
			w.WriteAttribute("numguards", NumGuards);
			w.WriteAttribute("maxguards", MaxGuards);
			w.WriteAttribute("respawnrate", RespawnRate);
		}

		public override void Use(Actor user)
		{
			if (user is Guard)
			{
				m.RemoveActor(user);	// unspawn this dude
				++NumGuards;
				return;
			}

			if (NumGuards > 0)
			{
				var a = new Guard(m) { Position = this.Position, Direction = this.Direction };
				m.AddActor(a);
				a.Use(user);
				--NumGuards;
			}
		}

		float fracRespawn = 0.0f;

		public override void Tick()
		{
			base.Tick();
			fracRespawn += RespawnRate;
			while (fracRespawn >= 1.0f)
			{
				if (NumGuards < MaxGuards)
					++NumGuards;
				fracRespawn--;
			}
		}
	}
}
