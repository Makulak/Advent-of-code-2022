string[] text = File.ReadAllLines(@"../../../input.txt");

Simulator simulator = new Simulator();

foreach (string line in text)
{
    string[] points = line.Split(" -> ");

    for (int i = 0; i < points.Length - 1; i++)
    {
        Coordinates coordsFrom = new Coordinates(points[i]);
        Coordinates coordsTo = new Coordinates(points[i + 1]);

        simulator.InsertRocks(coordsFrom, coordsTo);
    }
    //simulator.WriteGridToConsole();
    //Console.WriteLine();
}

int count = 0;
while (simulator.SimulateNewSandUnit())
    count++;

Console.WriteLine(count);

class Simulator
{
    char[,] field = new char[1000, 1000];

    public bool IsFreeSpace(Coordinates coords) => field[coords.X, coords.Y] == default(char) && coords.Y < LowestYPoint + 2;
    public bool FlowIntoAbyss(Coordinates coords) => coords.Y >= field.GetLength(1);
    public bool ReachFloor(Coordinates coords) => coords.Y + 1 == LowestYPoint + 2;

    public int LowestYPoint = 0;

    public bool SimulateNewSandUnit()
    {
        Coordinates sandCoord = new Coordinates(500, 0);
        bool canMove = true;

        while (canMove && !FlowIntoAbyss(sandCoord.OneDown))
        {
            if (IsFreeSpace(sandCoord.OneDown))
                sandCoord = sandCoord.OneDown;
            else if (IsFreeSpace(sandCoord.DiagLeft))
                sandCoord = sandCoord.DiagLeft;
            else if (IsFreeSpace(sandCoord.DiagRight))
                sandCoord = sandCoord.DiagRight;
            else
                canMove = false;
        }
        if (FlowIntoAbyss(sandCoord.OneDown) || (sandCoord.X == 500 && sandCoord.Y == 0))
            return false;

        field[sandCoord.X, sandCoord.Y] = 'O';

        //WriteGridToConsole();

        return true;
    }

    public void InsertRocks(Coordinates from, Coordinates to)
    {
        if (from.X == to.X)
        {
            int fromY = Math.Min(from.Y, to.Y);
            int toY = Math.Max(from.Y, to.Y);

            for (int i = fromY; i <= toY; i++)
            {
                field[from.X, i] = '#';
            }

            if (toY > LowestYPoint)
                LowestYPoint = toY;
        }
        if (from.Y == to.Y)
        {
            int fromX = Math.Min(from.X, to.X);
            int toX = Math.Max(from.X, to.X);

            for (int i = fromX; i <= toX; i++)
            {
                field[i, from.Y] = '#';
            }

            if (from.Y > LowestYPoint)
                LowestYPoint = from.Y;
        }
    }

    public void WriteGridToConsole()
    {
        for (int y = 0; y < 12 /*field.GetLength(1)*/; y++)
        {
            for (int x = 484; x < 514/*field.GetLength(1)*/; x++)
            {
                Console.Write(field[x, y] == '#' || field[x, y] == 'O' ? field[x, y] : '.');
            }
            Console.WriteLine();
        }
    }
}

class Coordinates
{
    public int X { get; set; }
    public int Y { get; set; }

    public Coordinates(string input)
    {
        string[] splitInput = input.Split(',');
        X = int.Parse(splitInput[0]);
        Y = int.Parse(splitInput[1]);
    }

    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coordinates OneDown => new Coordinates(X, Y + 1);
    public Coordinates DiagLeft => new Coordinates(X - 1, Y + 1);
    public Coordinates DiagRight => new Coordinates(X + 1, Y + 1);
}