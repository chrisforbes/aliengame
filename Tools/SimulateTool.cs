using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using AlienGame.Actors;

namespace AlienGame
{
	class SimulateTool : Tool
	{
		Actor selected;

		public override string Name { get { return "Run"; } }

		public override void DrawToolOverlay(Surface s, Graphics g, Model m)
		{
			if (selected != null)
				g.DrawRectangle(Pens.White, selected.Position.X - 20, selected.Position.Y - 20, 40, 40);
		}

		public override bool OnMouseDown(Surface s, Model m, Point square, Point offset, MouseButtons mb)
		{
			if (mb == MouseButtons.Left)
				selected = m.ActorsAt(square).Where(a => a is IOrderTarget).FirstOrDefault();

			if (mb == MouseButtons.Right && selected != null)
				((IOrderTarget)selected).AcceptOrder( m, square );

			return true;
		}
	}
}
