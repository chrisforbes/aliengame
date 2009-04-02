using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlienGame
{
	class Door
	{
		public Point Position;
		public int Kind;
		public int State;

		public Door(Point position, int kind, int state)
		{
			Position = position;
			Kind = kind;
			State = state;
		}
	}
}
