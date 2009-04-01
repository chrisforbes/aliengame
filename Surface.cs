﻿using System;
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
	}

	class Surface : Control
	{
		Model model;
		Tool tool;
		RenderOptions options = RenderOptions.Brushes;
		DBrush brushBrush = new SolidBrush(Color.FromArgb(128, Color.Gray));
		Pen brushPen = Pens.Silver;
		Pen wallPen = new Pen(Color.Blue, 3.0f);
		DBrush floorBrush = new SolidBrush(Color.FromArgb(40, 40, 40));

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
					if (model.HasFloor(i, j))
						e.Graphics.FillRectangle(floorBrush,
							i * 40, j * 40, 40, 40);

			for( var i = 0; i < ClientSize.Width / 40; i++ )
				for (var j = 0; j < ClientSize.Height / 40; j++)
				{
					if (model.HasWall(i, j, i + 1, j))
						e.Graphics.DrawLine(wallPen, i * 40 + 40, j * 40, i * 40 + 40, j * 40 + 40);

					if (model.HasWall(i, j, i, j + 1))
						e.Graphics.DrawLine(wallPen, i * 40, j * 40 + 40, i * 40 + 40, j * 40 + 40);
				}

			foreach (var d in model.doors)
				if (d.State == 0)
					if (d.Kind == 0)
						e.Graphics.FillRectangle(Brushes.LimeGreen, d.Position.X * 40 - 3, d.Position.Y * 40,
							7, 40);
					else
						e.Graphics.FillRectangle(Brushes.LimeGreen, d.Position.X * 40, d.Position.Y * 40 - 3,
							40, 7);

			if (0 != (options & RenderOptions.Brushes))
				foreach (var b in model.brushes)
				{
					var r = new Rectangle(b.Bounds.Left * 40, b.Bounds.Top * 40, 
						b.Bounds.Width * 40, b.Bounds.Height * 40);
					e.Graphics.FillRectangle(brushBrush, r);
					e.Graphics.DrawRectangle(brushPen, r);
				}

			if (tool != null)
				tool.DrawToolOverlay(this, e.Graphics, model);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (tool != null)
				if (tool.OnMouseDown(this, model, e.Location, e.Button))
					Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (tool != null)
				if (tool.OnMouseMove(this, model, e.Location, e.Button))
					Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (tool != null)
				if (tool.OnMouseUp(this, model, e.Location, e.Button))
					Invalidate();
		}
	}

	class Model
	{
		public List<Brush> brushes = new List<Brush>();
		public List<Door> doors = new List<Door>();

		public bool HasWall(int x1, int y1, int x2, int y2)
		{
			return brushes.Any(
				b => b.Bounds.Contains(x1, y1) || b.Bounds.Contains(x2, y2))
				&& !brushes.Any(
					b => b.Bounds.Contains(x1, y1)
						&& b.Bounds.Contains(x2, y2))
				&& null == HasDoor(x1, y1, x2, y2);
		}

		public bool HasFloor(int x, int y)
		{
			return brushes.Any(b => b.Bounds.Contains(x, y));
		}

		public Door HasDoor(int x1, int y1, int x2, int y2)
		{
			if (x1 != x2 && y1 != y2)
				throw new ArgumentException("wtf; not adjacent cells!");

			var x = Math.Max(x1, x2);
			var y = Math.Max(y1, y2);

			return doors.FirstOrDefault(
				d => d.Position == new Point(x,y)
					&& d.Kind == ((x1 == x2) ? 1 : 0));
		}
	}

	class Brush
	{
		public Rectangle Bounds;

		public Brush(Rectangle bounds)
		{
			Bounds = bounds;
		}
	}

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
