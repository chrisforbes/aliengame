using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AlienGame.Tools
{
	class DoorTool : Tool
	{
		Point q;
		int k;
		Door d;

		public override string Name { get { return "Place Doors"; }}

		public override void DrawToolOverlay(Surface s, Graphics g, Model m)
		{
			if (d != null)
			{
				if (d.Kind == 0)
					g.DrawRectangle(Pens.Red, new Rectangle(d.Position.X * 40 - 3, d.Position.Y * 40, 6, 40));
				else if (d.Kind == 1)
					g.DrawRectangle(Pens.Red, new Rectangle(d.Position.X * 40, d.Position.Y * 40 - 3, 40, 6));
			}
			else
				if (k == 0)
					g.DrawRectangle(Pens.Violet, new Rectangle(q.X * 40 - 3, q.Y * 40, 6, 40));
				else if (k == 1)
					g.DrawRectangle(Pens.Violet, new Rectangle(q.X * 40, q.Y * 40 - 3, 40, 6));
		}

		public override bool OnMouseMove( Surface s, Model m, Point square, Point offset, MouseButtons mb )
		{
			q = square;
			k = offset.X <= offset.Y ? 0 : 1;
			if( offset.X >= offset.Y && offset.X >= 20 )
			{
				++q.X;
				k ^= 1;
			}
			else if( offset.X <= offset.Y && offset.Y >= 20 )
			{
				++q.Y;
				k ^= 1;
			}

			d = m.doors.FirstOrDefault( a => a.Position == q && a.Kind == k );
			return true;
		}

		public override bool OnMouseDown( Surface s, Model m, Point square, Point offset, MouseButtons mb )
		{
			if (mb == MouseButtons.Left)
			{
				d = new Door(q, k, 0);
				m.doors.Add(d);
			}

			if (mb == MouseButtons.Right)
			{
				if (d != null)
				{
					m.doors.Remove(d);
					d = null;
				}
			}

			return true;
		}
	}
}
