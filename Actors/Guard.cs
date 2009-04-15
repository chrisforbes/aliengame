﻿using System;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace AlienGame.Actors
{
	using Order = Func<Actor, bool>;

	class Guard : Mover
	{
		[Browsable(true), Category("Guard")]
		public string Target { get; set; }

		public override void Draw(Graphics g)
		{
			DrawDirection(g);
			DrawBasicActor(g, Pens.Fuchsia);
		}

		protected override void SaveAttributes(XmlWriter w)
		{
			base.SaveAttributes(w);
			w.WriteAttribute("target", Target);
		}

		public Guard(Model m) : base(m) { Target = "";  }
		public Guard(Model m, XmlElement e) : base(m,e) { Target = e.GetAttribute("target"); }

		public override void Use(Actor user)
		{
			var spawner = m.ActorsAt(Position.ToSquare()).OfType<GuardSpawner>().First();
			if (spawner == null)
				throw new NotImplementedException("odd kind of guard");

			PushGoal(Goal.Use(spawner));
			PushGoal(Goal.Use(user));
		}
	}
}
