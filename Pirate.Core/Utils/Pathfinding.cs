using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Pirate.Core.Utils;

public class Node
{
    public int X, Y;
    public bool IsWalkable;
    public double G = double.MaxValue; // Cost from start
    public double H; // Heuristic to end
    public double F => G + H;
    public Node Parent;

    public Node(int x, int y, bool walkable)
    {
        X = x;
        Y = y;
        IsWalkable = walkable;
    }

    public static implicit operator Vector2(Node node)
    {
        return new Vector2(node.X, node.Y);
    }
}

public interface IPathfinder
{
    public List<Vector2> FindPath(Node start, Node goal, Navmap navmap)
    {
        var openSet = new PriorityQueue<Node, double>();
        var closedSet = new HashSet<Node>();

        start.G = 0;
        start.H = GetDistance(start, goal);
        start.Parent = start; // Theta* initialization: start is its own parent
        openSet.Enqueue(start, start.F);

        while (openSet.Count > 0)
        {
            Node s = openSet.Dequeue();

            if (s == goal) return ReconstructPath(s);

            closedSet.Add(s);

            foreach (Node neighbor in GetNeighbors(s, navmap))
            {
                if (closedSet.Contains(neighbor)) continue;

                UpdateVertex(s, neighbor, openSet, navmap);
            }
        }

        return null; // No path found
    }

    bool HasLineOfSight(Node a, Node b, Navmap navmap)
    {
        int x0 = a.X; int y0 = a.Y;
        int x1 = b.X; int y1 = b.Y;

        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            if (!navmap.IsSailable(x0,y0)) return false;
            if (x0 == x1 && y0 == y1) return true;

            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }
    }

    void UpdateVertex(Node s, Node neighbor, PriorityQueue<Node, double> openSet, Navmap navmap)
    {
        double oldG = neighbor.G;

        // Line of Sight Check: Can we skip 's' and go straight from s.Parent?
        if (HasLineOfSight(s.Parent, neighbor, navmap))
        {
            double distance = GetDistance(s.Parent, neighbor);
            if (s.Parent.G + distance < neighbor.G)
            {
                neighbor.G = s.Parent.G + distance;
                neighbor.Parent = s.Parent;
            }
        }
        else
        {
            // Standard A* move: Neighbor must go through 's'
            double distance = GetDistance(s, neighbor);
            if (s.G + distance < neighbor.G)
            {
                neighbor.G = s.G + distance;
                neighbor.Parent = s;
            }
        }

        // If we improved the path, update the priority queue
        if (neighbor.G < oldG)
        {
            // In .NET 6+ PriorityQueue doesn't have an UpdateKey, 
            // so we just enqueue again. The closedSet handles duplicates.
            openSet.Enqueue(neighbor, neighbor.F);
        }
    }

    double GetDistance(Node a, Node b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }

    private List<Vector2> ReconstructPath(Node goalNode)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = goalNode;

        // Follow the parent pointers back to the start
        // Note: In Theta*, the start node's parent is itself
        while (currentNode != null)
        {
            path.Add(currentNode);

            // Check if we've reached the start node
            if (currentNode.Parent == currentNode)
                break;

            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbors(Node node, Navmap navmap)
    {
        List<Node> res = new List<Node>();

        res.Add(new Node(node.X, node.Y-1, navmap.IsSailable(node.X, node.Y-1)));
        res.Add(new Node(node.X, node.Y+1, navmap.IsSailable(node.X, node.Y+1)));
        res.Add(new Node(node.X-1, node.Y, navmap.IsSailable(node.X-1, node.Y)));
        res.Add(new Node(node.X+1, node.Y, navmap.IsSailable(node.X+1, node.Y)));

        return res;
    }
}


