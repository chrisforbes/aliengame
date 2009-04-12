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
		public static Pathfinder Instance;

		readonly Model model;
		public Pathfinder( Model m )
		{
			model = m;
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

		IEnumerable<Point> FindPathWithinBrush( Point location, Point endpoint )
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
						if( x == location.X && y == location.Y )
							continue;
						if( model.GetBrushesAt( new Point( x, y ) ).Count() != 1 )
							yield return new Point( x, y );
						// TODO: if x,y is on either side of a door
						//	 yield return new Point( x, y );
					}
				}
			}
		}

		// return a path in reverse order.
		public IEnumerable<Point> FindPath( Point from, Point to )
		{
			var queue = new PriorityQueue<QueueNode>();
			var seen = new Dictionary<Point, Pair<double, Point>>();

			queue.Add( new QueueNode( 0, Distance( from, to ), from ) );
			seen.Add( from, new Pair<double, Point>( 0, from ) );

			while( !queue.Empty )
			{
				var k = queue.Pop();
				var nodeInfo = seen[ k.Location ];
				if( k.DistanceSoFar > nodeInfo.First )
					continue;
				if( k.DistanceSoFar < nodeInfo.First )
					throw new NotImplementedException( "WTF: found a faster path than mindistance" );

				foreach( var node in FindPathWithinBrush( k.Location, to ) )
				{
					if( node == to )
					{
						yield return node;
						yield return k.Location;
						while( nodeInfo.Second != from )
						{
							yield return nodeInfo.Second;
							seen.TryGetValue( nodeInfo.Second, out nodeInfo );
						}
						yield break;
					}
					var dist = Distance( k.Location, node ) + k.DistanceSoFar;
					if( seen.TryGetValue( node, out nodeInfo ) )
					{
						if( nodeInfo.First > dist )
							seen[ node ] = new Pair<double, Point>( dist, k.Location );
					}
					else
						seen.Add( node, new Pair<double, Point>( dist, k.Location ) );

					queue.Add( new QueueNode( dist, Distance( node, to ), node ) );
				}
			}
			// no path exists.
			yield break;
		}
	}
}
