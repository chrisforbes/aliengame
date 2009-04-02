using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlienGame
{
	abstract class Actor
	{
		public Point Position;
		public int Direction;
		public string Name = "";

		public virtual void Draw(Graphics g) { }
		public virtual void Tick(Model m) { }
	}
}
