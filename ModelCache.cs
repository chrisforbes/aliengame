﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlienGame
{
	struct CellCacheEntry
	{
		public int? content;
		public int brushes;
		public int doors;
		public int wallMask;
	}

	class ModelCache
	{
		readonly CellCacheEntry[,] cce;

		public ModelCache(Model m)
		{
			var mx = m.Brushes.Select(b => b.Bounds.Right + 1).DefaultIfEmpty(0).Max();
			var my = m.Brushes.Select(b => b.Bounds.Bottom + 1).DefaultIfEmpty(0).Max();

			cce = new CellCacheEntry[mx, my];

			for (var x = 0; x < mx; x++)
				for (var y = 0; y < my; y++)
				{
					cce[x, y].content = m.GetFloorContent(x, y);
					cce[x, y].brushes = m.GetBrushesAt(new Point(x, y)).Count();
					cce[x, y].doors = m.GetDoorsEndFrom(new Point(x, y)).Count();

					if (m.HasWall(x, y, x + 1, y)) cce[x, y].wallMask |= 0x1;
					if (m.HasWall(x, y, x, y + 1)) cce[x, y].wallMask |= 0x2;
					if (m.HasDoor(x, y, x + 1, y) != null) cce[x, y].wallMask |= 0x4;
					if (m.HasDoor(x, y, x, y + 1) != null) cce[x, y].wallMask |= 0x8;
					if (m.HasWall(x - 1, y, x, y)) cce[x, y].wallMask |= 0x10;
					if (m.HasWall(x, y - 1, x, y)) cce[x, y].wallMask |= 0x20;
					if (m.HasDoor(x - 1, y, x, y) != null) cce[x, y].wallMask |= 0x40;
					if (m.HasDoor(x, y - 1, x, y) != null) cce[x, y].wallMask |= 0x80;
				}
		}

		public int? GetContentAt(int x, int y)
		{
			if (x < 0 || y < 0 || x >= cce.GetUpperBound(0) || y >=cce.GetUpperBound(1))
				return null;
			return cce[x, y].content;
		}

		public int GetNumBrushesAt(int x, int y)
		{
			if (x < 0 || y < 0 || x >= cce.GetUpperBound(0) || y >=cce.GetUpperBound(1))
				return 0;
			return cce[x, y].brushes;
		}

		public int GetNumDoorsAt(int x, int y)
		{
			if (x < 0 || y < 0 || x >= cce.GetUpperBound(0) || y >=cce.GetUpperBound(1))
				return 0;
			return cce[x, y].doors;
		}

		public int? GetWallMask(int x, int y)
		{
			if (x < 0 || y < 0 || x >= cce.GetUpperBound(0) || y >= cce.GetUpperBound(1))
				return null;
			return cce[x, y].wallMask;
		}
	}
}
