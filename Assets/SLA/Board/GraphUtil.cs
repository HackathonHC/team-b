using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public static class GraphUtil
    {
        static public Graph<Point2> ConvertToGraph(int[,] cellWeights)
        {
            return ConvertToGraph(cellWeights, Point2.directions);
        }

        static public Graph<Point2> ConvertToGraph(int[,] cellWeights, Point2[] directions)
        {
            var directionCosts = new Dictionary<Point2, int>(directions.Length);
            foreach(var it in directions)
            {
                directionCosts.Add(it, 1);
            }

            return ConvertToGraph(cellWeights, directionCosts);
        }

        static public Graph<Point2> ConvertToGraph(int[, ] cellWeights, Dictionary<Point2, int> directionCosts)
        {
            var result = new Graph<Point2>();
            result.edgeWeights = new Dictionary<Point2, Dictionary<Point2, int>>(cellWeights.Length);

            for(int i=0; i<cellWeights.GetLength(0) ; ++i)
            {
                for(int j=0; j<cellWeights.GetLength(1) ; ++j)
                {
                    var point = new Point2(i ,j);
                    result.edgeWeights.Add(point, new Dictionary<Point2, int>(Point2.directions.Length));

                    foreach(var directionCost in directionCosts)
                    {
                        var next = new Point2(i, j);
                        next += directionCost.Key;
                        if (next.x >= 0 && next.x < cellWeights.GetLength(0) && next.y>=0 && next.y < cellWeights.GetLength(1))
                        {
                            if (cellWeights[next.x, next.y] > 0)
                            {
                                int weight = cellWeights[next.x, next.y] * directionCost.Value;
                                result.edgeWeights[point].Add(next, weight);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
