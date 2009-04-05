using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AlienGame.Tools
{
	class EditActorTool : Tool
	{
		public override string Name { get { return "Edit Actors"; } }
		public static PropertiesForm Ui;
		Actor selected;

		public override bool OnMouseDown(Surface s, Model m, Point square, Point offset, MouseButtons mb)
		{
			var q = square;
			if (mb == MouseButtons.Left)
			{
				Ui.SetActor(selected = ActorAt(m, q));
				return true;
			}

			if (mb == MouseButtons.Right)
			{
				var a = ActorAt(m, q);
				if (a != null)
					m.RemoveActor(a);
				m.SyncActorList();
				return true;
			}

			return false;
		}

		public override bool OnMouseMove(Surface s, Model m, Point square, Point offset, MouseButtons mb)
		{
			if (selected != null && mb == MouseButtons.Left)
			{
				selected.Position = square.SquareToCenter();
				if (Ui.Visible) Ui.SetActor(selected);	// refresh the view in the property editor
				return true;
			}

			return false;
		}

		static Actor ActorAt(Model m, Point p)
		{
			return m.Actors.FirstOrDefault(
				a => p == new Point(a.Position.X / 40, a.Position.Y / 40));
		}
	}
}
