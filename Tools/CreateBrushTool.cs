using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AlienGame.Tools
{
	using DBrush = System.Drawing.Brush;

	abstract class CreateBrushTool : Tool
	{
		Brush b;
		Point q,sq;

		Pen newBrushPen = new Pen(Color.Orange, 3) { DashStyle = DashStyle.Dash };
		DBrush newBrushBrush = new SolidBrush(Color.Orange.WithAlpha(128));

		protected abstract void PrepareBrush(Brush b);

		public override bool OnMouseMove( Surface s, Model m, Point square, Point offset, MouseButtons mb )
		{
			q = square;
			if (b == null) return true;
			if (mb == MouseButtons.Left)
			{
				b.Bounds = Rectangle.FromLTRB(sq.X, sq.Y, q.X, q.Y).Fix();
				m.Cache.Invalidate();
			}
			return true;
		}

		public override bool OnMouseDown( Surface s, Model m, Point square, Point offset, MouseButtons mb )
		{
			if (mb == MouseButtons.Left)
			{
				b = new Brush(q.ToPointRect());
				PrepareBrush(b);
				sq = q;
				m.AddBrush(b);
				m.Cache.Invalidate();
			}
			return true;
		}

		public override bool OnMouseUp( Surface s, Model m, Point square, Point offset, MouseButtons mb )
		{
			b = null;
			return true;
		}

		public override void DrawToolOverlay(Surface s, Graphics g, Model m)
		{
			if (b != null)
			{
				var r = b.Bounds.SquaresToPixels();
				g.FillRectangle(newBrushBrush, r);
				g.DrawRectangle(newBrushPen, r);
			}

			var z = q.ToPointRect().SquaresToPixels();
			g.DrawRectangle(Pens.Red,z);
		}
	}

	class CreateNormalBrushTool : CreateBrushTool
	{
		protected override void PrepareBrush(Brush b) { b.Content = 0; }
		public override string Name { get { return "Create Normal Brush"; } }
	}

	class CreateVentBrushTool : CreateBrushTool
	{
		protected override void PrepareBrush(Brush b) { b.Content = 1; }
		public override string Name { get { return "Create Vent Brush"; } }
	}
}
