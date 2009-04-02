using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AlienGame
{
	using DBrush = System.Drawing.Brush;
	class CreateBrushTool : Tool
	{
		Brush b;
		Point q,sq;

		Pen newBrushPen = new Pen(Color.Orange, 3) { DashStyle = DashStyle.Dash };
		DBrush newBrushBrush = new SolidBrush(Color.FromArgb(128, Color.Orange));

		public override string Name { get { return "Create Brush"; } }

		public override bool OnMouseMove(Surface s, Model m, Point p, MouseButtons mb)
		{
			q = new Point((p.X) / 40, (p.Y) / 40);
			if (mb == MouseButtons.Left) b.Bounds = Rectangle.FromLTRB(sq.X, sq.Y, q.X, q.Y).Fix();
			return true;
		}

		public override bool OnMouseDown(Surface s, Model m, Point p, MouseButtons mb)
		{
			if (mb == MouseButtons.Left)
			{
				b = new Brush(new Rectangle(q.X, q.Y, 1, 1));
				sq = q;
				m.brushes.Add(b);
			}
			return true;
		}

		public override bool OnMouseUp(Surface s, Model m, Point p, MouseButtons mb)
		{
			b = null;
			return true;
		}

		public override void DrawToolOverlay(Surface s, Graphics g, Model m)
		{
			if (b != null)
			{
				var r = new Rectangle(b.Bounds.Left * 40, b.Bounds.Top * 40, b.Bounds.Width * 40, b.Bounds.Height * 40);
				g.FillRectangle(newBrushBrush, r);
				g.DrawRectangle(newBrushPen, r);
			}

			var z = new Rectangle(q.X * 40, q.Y * 40, 40, 40);
			g.DrawRectangle(Pens.Red,z);
		}
	}

	class MoveBrushTool : Tool
	{
		Brush b;
		Point q;

		Pen brushPen = new Pen(Color.Red, 3) { DashStyle = DashStyle.Dash };

		public override string Name { get { return "Move Brush"; } }

		public override bool OnMouseMove(Surface s, Model m, Point p, MouseButtons mb)
		{
			var x = new Point(p.X / 40, p.Y / 40);

			if (mb == MouseButtons.None)
				b = m.brushes.LastOrDefault(a => a.Bounds.Contains(x));
			else if (b != null)
				b.Bounds.Offset(x.X - q.X, x.Y - q.Y);

			q = x;

			return true;
		}

		public override bool OnMouseDown(Surface s, Model m, Point p, MouseButtons mb)
		{
			q = new Point(p.X / 40, p.Y / 40);

			if (mb == MouseButtons.Right && b != null)
			{
				m.brushes.Remove(b);
				b = null;
			}

			return true;
		}

		public override void DrawToolOverlay(Surface s, Graphics g, Model m)
		{
			if (b != null)
			{
				var r = new Rectangle(b.Bounds.Left * 40, b.Bounds.Top * 40, 
					b.Bounds.Width * 40, b.Bounds.Height * 40);
				g.DrawRectangle(brushPen, r);
			}
		}
	}
}
