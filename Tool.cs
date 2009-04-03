using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AlienGame
{
	abstract class Tool
	{
		public abstract string Name { get; }
		public virtual void DrawToolOverlay(Surface s, Graphics g, Model m) { }
		public virtual bool OnMouseDown(Surface s, Model m, Point square, Point offset, MouseButtons mb) { return false; }
		public virtual bool OnMouseMove(Surface s, Model m, Point square, Point offset, MouseButtons mb) { return false; }
		public virtual bool OnMouseUp(Surface s, Model m, Point square, Point offset, MouseButtons mb) { return false; }

		public override string ToString() { return Name; }
	}
}
