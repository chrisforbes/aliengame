using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using IjwFramework.Collections;
using IjwFramework.Types;

namespace AlienGame
{
	class Pathfinder
	{
		readonly Model model;
		int contentmask;

		public Pathfinder( Model m, int mask )
		{
			model = m;
			contentmask = mask;
		}

		struct QueueNode : IComparable<QueueNode>
		{
			public double DistanceSoFar;
			public double DistanceToGo;
			public Point Location;

			double TotalDistance { get { return DistanceSoFar + DistanceToGo; } }

			public QueueNode( double soFar, double toGo, Point loc )
			{
				if( double.IsNaN( soFar))
					throw new NotImplementedException();
				DistanceSoFar = soFar;
				if( double.IsNaN(toGo))
					throw new NotImplementedException();
				DistanceToGo = toGo;
				Location = loc;
			}

			public int CompareTo( QueueNode other )
			{
				return TotalDistance.CompareTo( other.TotalDistance );
			}
		}

		static double Distance( Point a, Point b )
		{
			var dx = a.X - b.X;
			var dy = a.Y - b.Y;
			return Math.Sqrt( ( dx * dx ) + ( dy * dy ) );
		}

		IEnumerable<Point> FindNeighbours( Point location, Point endpoint )
		{
			foreach( var brush in model.GetBrushesAt( location ) )
			{
				if( brush.Bounds.Contains( endpoint ) )
					yield return endpoint;

				// todo: cache brush-overlap somewhere.
				for( var x = brush.Bounds.Left ; x < brush.Bounds.Right ; x++ )
				{
					for( var y = brush.Bounds.Top ; y < brush.Bounds.Bottom ; y++ )
					{
						var xy = new Point( x, y );
						if( xy == location )
							continue;
						if( (model.Cache.Value.GetNumBrushesAt(x,y) != 1) 
							|| ( model.Cache.Value.GetNumDoorsAt(x,y) != 0)) 
							yield return xy;
					}
				}
			}
			foreach( var doorLocation in model.GetDoorsEndFrom( location ) )
				yield return doorLocation;
		}

		static IEnumerable<Point> TracePath(QueueNode k, Point from, Dictionary<Point, Pair<double,Point>> seen)
		{
			var nodeInfo = seen[k.Location];
			yield return k.Location;
			while (nodeInfo.Second != from)
			{
				yield return nodeInfo.Second;
				nodeInfo = seen[nodeInfo.Second];
			}
		}

		static IEnumerable<Point> NoPath = new Point[] { };

		bool CanWalkAt(Point p)
		{
			var content = model.Cache.Value.GetContentAt(p.X, p.Y);
			if (content == null) return false;
			return ((1 << content.Value) & contentmask) != 0;
		}

		// return a path in reverse order.
		public IEnumerable<Point> FindPath( Point from, Point to )
		{
			if (!CanWalkAt(to)) return NoPath;	// quick out!

			var queue = new PriorityQueue<QueueNode>();
			var seen = new Dictionary<Point, Pair<double, Point>>();

			queue.Add( new QueueNode( 0, Distance( from, to ), from ) );
			seen.Add( from, new Pair<double, Point>( 0, from ) );

			while( !queue.Empty )
			{
				var k = queue.Pop();
				if (k.Location == to) return TracePath(k, from, seen);

				var nodeInfo = seen[k.Location];

				if( k.DistanceSoFar > nodeInfo.First ) continue;
				if( k.DistanceSoFar < nodeInfo.First )
					throw new NotImplementedException( "WTF: found a faster path than mindistance" );

				foreach( var node in FindNeighbours( k.Location, to ).Where( CanWalkAt ))
				{
					var dist = Distance( k.Location, node ) + k.DistanceSoFar;
					seen.MinimizeValue(node, new Pair<double, Point>(dist, k.Location),
						Pair<double, Point>.AsFirst);
					queue.Add( new QueueNode( dist, Distance( node, to ), node ) );
				}
			}

			return NoPath;
		}
	}

	static class AStarExts
	{
		public static void MinimizeValue<T, U>(this Dictionary<T, U> dict,
			T key, U newValue, Func<U,double> f)
		{
			U oldValue;
			if (!dict.TryGetValue(key, out oldValue) || !(f(newValue) >= f(oldValue)))
				dict[key] = newValue;
		}
	}
}
