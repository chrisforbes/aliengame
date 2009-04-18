using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace AlienGame
{
	using DBrush = System.Drawing.Brush;

	[Flags]
	enum RenderOptions
	{
		Brushes = 1,
		Grid = 2,
		ActorDebug = 4,
	}

	class Surface : Control
	{
		Model model;
		Tool tool;
		RenderOptions options = RenderOptions.Brushes;
		DBrush brushBrush = new SolidBrush(Color.Gray.WithAlpha(128));
		Pen brushPen = Pens.Silver;
		Pen wallPen = new Pen(Color.Blue, 3.0f);
		DBrush[] floorBrush = new DBrush[] { new SolidBrush(Color.FromArgb(40, 40, 40)), Brushes.Indigo };
		DBrush[] doorBrush = new DBrush[] { Brushes.LimeGreen, Brushes.HotPink };

		public Model Model
		{
			get { return model; }
			set { model = value; Invalidate(); }
		}

		public Tool Tool
		{
			get { return tool; }
			set { tool = value; Invalidate(); }
		}

		public RenderOptions Options
		{
			get { return options; }
			set { options = value; Invalidate(); }
		}

		public Surface()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			UpdateStyles();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (model == null) return;
			if (0 != (options & RenderOptions.Grid))
				ControlPaint.DrawGrid( e.Graphics, ClientRectangle, new Size(40,40), Color.Black );

			// draw the real stuff
			for (var i = 0; i < ClientSize.Width / 40; i++)
				for (var j = 0; j < ClientSize.Height / 40; j++)
				{
					var content = model.Cache.Value.GetContentAt(i, j);
					if (content != null)
						e.Graphics.FillRectangle(floorBrush[content.Value], new Point(i, j).ToPointRect().SquaresToPixels());
				}

			for( var i = 0; i < ClientSize.Width / 40; i++ )
				for (var j = 0; j < ClientSize.Height / 40; j++)
				{
					var wallMask = model.Cache.Value.GetWallMask(i, j);
					if (wallMask == null) continue;

					if ((wallMask & 0x5) == 1)
						e.Graphics.DrawLine(wallPen, i * 40 + 40, j * 40, i * 40 + 40, j * 40 + 40);
					if ((wallMask & 0xa) == 2)
						e.Graphics.DrawLine(wallPen, i * 40, j * 40 + 40, i * 40 + 40, j * 40 + 40);						
				}

			foreach (var d in model.doors)
				if (d.Kind == 0)
					e.Graphics.FillRectangle(doorBrush[d.State], d.Position.X * 40 - 3, d.Position.Y * 40,
						7, 40);
				else
					e.Graphics.FillRectangle(doorBrush[d.State], d.Position.X * 40, d.Position.Y * 40 - 3,
						40, 7);

			if (0 != (options & RenderOptions.Brushes))
				foreach (var b in model.Brushes)
				{
					var r = b.Bounds.SquaresToPixels();
					e.Graphics.FillRectangle(brushBrush, r);
					e.Graphics.DrawRectangle(brushPen, r);
				}

			foreach (var a in model.Actors)
				a.Draw(e.Graphics);

			if (0 != (options & RenderOptions.ActorDebug))
				foreach (var a in model.Actors)
					a.DrawOverlay(e.Graphics);

			if (tool != null)
				tool.DrawToolOverlay(this, e.Graphics, model);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			var square = new Point( e.Location.X / 40, e.Location.Y / 40 );
			var offset = new Point( e.Location.X % 40, e.Location.Y % 40 );

			if( tool != null )
				if( tool.OnMouseDown( this, model, square, offset, e.Button ) )
					Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var square = new Point( e.Location.X / 40, e.Location.Y / 40 );
			var offset = new Point( e.Location.X % 40, e.Location.Y % 40 );

			if (tool != null)
				if (tool.OnMouseMove(this, model, square, offset, e.Button))
				{
					Invalidate();
					Update();
				}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			var square = new Point( e.Location.X / 40, e.Location.Y / 40 );
			var offset = new Point( e.Location.X % 40, e.Location.Y % 40 );
			if( tool != null )
				if (tool.OnMouseUp(this, model, square, offset, e.Button))
					Invalidate();
		}
	}
}
