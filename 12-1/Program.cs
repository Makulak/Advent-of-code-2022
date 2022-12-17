string[] text = File.ReadAllLines(@"../../../input.txt");

var grid = new GridWrap(text);
grid.Start();

class GridWrap
{
    Point[,] Grid { get; set; }

    Point StartPoint { get; set; }
    Point EndPoint { get; set; }

    List<Point> StartingPoints { get; set; }

    int GridHeight;
    int GridWidth;

    public GridWrap(string[] text)
    {
        GridHeight = text.Length;
        GridWidth = text[0].Length;

        Grid = new Point[text.Length, text[0].Length];

        StartingPoints = new List<Point>();

        for (int y = 0; y < text.Length; y++)
        {
            for (int x = 0; x < text[y].Length; x++)
            {
                Grid[y, x] = new Point(x, y, text[y][x]);

                if (text[y][x] == 'S')
                {
                    StartingPoints.Add(Grid[y, x]);
                    StartPoint = Grid[y, x];
                }
                if (text[y][x] == 'a')
                    StartingPoints.Add(Grid[y, x]);
                if (text[y][x] == 'E')
                    EndPoint = Grid[y, x];

            }
        }
    }

    public void Start()
    {
        ComputeGraph(StartPoint);
        int min = int.MaxValue;
        foreach (var point in StartingPoints)
        {
            Dijkstra(point);
            var shortestPath = new List<Point>();
            shortestPath.Add(EndPoint);
            BuildShortestPath(shortestPath, EndPoint);
            if (shortestPath.Count - 1 < min)
                min = shortestPath.Count - 1;
            Console.WriteLine(shortestPath.Count - 1);
            ResetPoint(point, new List<Point>());
        }
        Console.WriteLine(min);
    }

    void ResetPoint(Point point, List<Point> clearedPoints)
    {
        point.MinCostToStart = null;
        point.IsVisited2 = false;
        point.NearestToStart = null;
        clearedPoints.Add(point);

        foreach (var p in point.PossibleMoves.Where(pp => !clearedPoints.Any(x => x.X == pp.X && x.Y == pp.Y)))
            ResetPoint(p, clearedPoints);
    }

    void ComputeGraph(Point point)
    {
        if (point.IsVisited)
            return;

        point.IsVisited = true;

        AssignPossibleMoves(point);

        foreach (var p in point.PossibleMoves)
            ComputeGraph(p);
    }

    private void BuildShortestPath(List<Point> list, Point node)
    {
        if (node.NearestToStart == null)
            return;
        list.Add(node.NearestToStart);
        BuildShortestPath(list, node.NearestToStart);
    }

    void Dijkstra(Point startPoint)
    {
        startPoint.MinCostToStart = 0;
        var prioQueue = new List<Point>();
        prioQueue.Add(startPoint);
        do
        {
            prioQueue = prioQueue.OrderBy(x => x.MinCostToStart).ToList();
            var node = prioQueue.First();
            prioQueue.Remove(node);
            foreach (var cnn in node.PossibleMoves)
            {
                var childNode = cnn;
                if (childNode.IsVisited2)
                    continue;
                if (childNode.MinCostToStart == null ||
                    node.MinCostToStart + 1 < childNode.MinCostToStart)
                {
                    childNode.MinCostToStart = node.MinCostToStart + 1;
                    childNode.NearestToStart = node;
                    if (!prioQueue.Contains(childNode))
                        prioQueue.Add(childNode);
                }
            }
            node.IsVisited2 = true;
            if (node.IsFinish)
                return;
        } while (prioQueue.Any());

    }

    //void GetShortestPath(Point point, Stack<Point> path)
    //{
    //    if (point.IsFinish)
    //        Console.WriteLine(path.Count);

    //    if (point.PossibleMoves.Count == 0 && path.Count > 0)
    //    {
    //        GetShortestPath(path.Pop(), path);
    //    }

    //    path.Push(point);

    //    foreach (var p in point.PossibleMoves)
    //    {
    //        if (path.Any(pp => pp.X == p.X && pp.Y == p.Y))
    //            continue;

    //        GetShortestPath(p, path);
    //    }
    //}

    private void AssignPossibleMoves(Point point)
    {
        int pointX = point.X;
        int pointY = point.Y;

        if (pointX > 0 && CanMoveToPoint(point, Grid[pointY, pointX - 1])) // lewo
            point.PossibleMoves.Add(Grid[pointY, pointX - 1]);
        if (pointY > 0 && CanMoveToPoint(point, Grid[pointY - 1, pointX])) // gora
            point.PossibleMoves.Add(Grid[pointY - 1, pointX]);
        if (pointX < GridWidth - 1 && CanMoveToPoint(point, Grid[pointY, pointX + 1])) // prawo
            point.PossibleMoves.Add(Grid[pointY, pointX + 1]);
        if (pointY < GridHeight - 1 && CanMoveToPoint(point, Grid[pointY + 1, pointX])) // dol
            point.PossibleMoves.Add(Grid[pointY + 1, pointX]);
    }

    private bool CanMoveToPoint(Point from, Point to)
    {
        if (from.IsStart || (to.IsFinish && from.Height == 'z'))
            return true;
        if (from.IsFinish)
            return false;

        if (!to.IsFinish)
            return to.Height - 1 <= from.Height;

        return false;
    }
}

class Point
{
    public int X { get; set; }

    public int Y { get; set; }

    public char Height { get; set; }

    public bool IsVisited { get; set; }



    public int? MinCostToStart { get; set; }
    public bool IsVisited2 { get; set; }
    public Point NearestToStart { get; set; }



    public Point(int x, int y, char height)
    {
        X = x;
        Y = y;
        Height = height;
        PossibleMoves = new();
    }

    public bool IsFinish => Height == 'E';
    public bool IsStart => Height == 'S';

    public List<Point> PossibleMoves { get; set; }

    public override string ToString()
    {
        return "X: " + X + " Y: " + Y + " Value: " + Height + " PossibleMoves: " + PossibleMoves.Count;
    }
}