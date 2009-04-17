using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AlienGame.Tools
{
	class MoveBrushTool : Tool
	{
		Brush b;
		Point q;

		Pen brushPen = new Pen(Color.Red, 3) { DashStyle = DashStyle.Dash };

		public override string Name { get { return "Move Brush"; } }

		public override bool OnMouseMove(Surface s, Model m, Point square, Point offset, MouseButtons mb)
		{
			if (mb == MouseButtons.None)
				b = m.Brushes.LastOrDefault(a => a.Bounds.Contains(square));
			else if (b != null)
			{
				b.Bounds.Offset(square.X - q.X, square.Y - q.Y);
				m.Cache.Invalidate();
			}

			q = square;

			return true;
		}

		public override bool OnMouseDown(Surface s, Model m, Point square, Point offset, MouseButtons mb)
		{
			q = square;

			if (mb == MouseButtons.Right && b != null)
			{
				m.RemoveBrush(b);
				m.Cache.Invalidate();
				b = null;
			}

			return true;
		}

		public override void DrawToolOverlay(Surface s, Graphics g, Model m)
		{
			if (b != null)
			{
				var r = b.Bounds.SquaresToPixels();
				g.DrawRectangle(brushPen, r);
			}
		}
	}
}
