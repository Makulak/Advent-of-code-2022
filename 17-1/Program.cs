string pattern = File.ReadAllText(@"../../../input.txt");

var tetris = new Tetris(pattern);

tetris.Start(5262);
var result = tetris.GetHighestPoint();
Console.WriteLine(result);

//tetris.Start2();

enum EDirection
{
    Left = '<',
    Right = '>',
    Down = 'v'
}

class Tetris
{
    char[,] Chamber { get; set; }
    string Pattern { get; set; }

    int moveCounter { get; set; }
    int rockCounter { get; set; }

    public Tetris(string pattern)
    {
        Chamber = new char[7, 12000];
        Pattern = pattern;
    }

    public void Start(int maxRocks)
    {
        for (rockCounter = 0; rockCounter < maxRocks; rockCounter++)
        {
            SimulateRock();
            //WriteChamber();
        }
        //WriteChamber();
    }

    public void Start2()
    {
        Start(6000);
        int matchingCount = 0;

        for (int a = 149; a <= 149; a++)
        {
            for (int j = 1 + a; j < Chamber.GetLength(1); j++)
            {
                string startCycle = new string(Enumerable.Range(0, 7).Select(x => Chamber[x, matchingCount + a]).ToArray());

                string preposition = new string(Enumerable.Range(0, 7).Select(x => Chamber[x, j]).ToArray());

                if (preposition == startCycle)
                {
                    matchingCount++;
                    if (matchingCount == 30)
                    {
                        Console.WriteLine("For a = " + a + " Cycle found on: " + j);
                        matchingCount = 0;
                    }
                }
                else
                {
                    j -= matchingCount;
                    matchingCount = 0;
                }
            }
            Console.WriteLine("===========");
        }
    }

    public int GetHighestPoint()
    {
        for (int i = 0; i < Chamber.GetLength(1); i++)
        {
            if (Enumerable.Range(0, 7).Select(x => Chamber[x, i]).All(x => x == default(char)))
                return i;
        }
        return 0;
    }

    public void WriteChamber()
    {
        int highestPoint = GetHighestPoint();
        for (int y = highestPoint; y >= 0; y--)
        {
            Console.Write("|");
            for (int x = 0; x < Chamber.GetLength(0); x++)
            {
                Console.Write(Chamber[x, y] == default(char) ? '.' : '#');
            }
            Console.Write("|");
            Console.WriteLine();
        }
        Console.WriteLine("+-------+");
        Console.WriteLine();
    }

    private void SimulateRock()
    {
        int highestPoint = GetHighestPoint();
        char[,] rock = GetProperRock();

        Coordinates rockPosition = new Coordinates(2, highestPoint + 3);

        bool blockMoved;

        do
        {
            TryMoveRock(ref rockPosition, rock, GetHorizontalDirection());
            blockMoved = TryMoveRock(ref rockPosition, rock, EDirection.Down);

            moveCounter++;
        } while (blockMoved);

        PlaceRock(rockPosition, rock);
    }

    private void PlaceRock(Coordinates rockPosition, char[,] rock)
    {
        int rockWidth = Rock.Width(rock);
        int rockHeight = Rock.Height(rock);

        for (int x = rockPosition.X; x < rockWidth + rockPosition.X; x++)
        {
            for (int y = rockPosition.Y; y < rockPosition.Y + rockHeight; y++)
            {
                if (rock[x - rockPosition.X, y - rockPosition.Y] != default(char))
                    Chamber[x, y] = rock[x - rockPosition.X, y - rockPosition.Y];
            }
        }
    }

    private bool TryMoveRock(ref Coordinates rockPos, char[,] rock, EDirection direction)
    {
        Coordinates positionAfterMove = rockPos.GetCoordinatesAfterMove(direction);

        if (CanPlaceRock(positionAfterMove, rock))
        {
            rockPos = positionAfterMove;
            return true;
        }
        return false;
    }

    private bool CanPlaceRock(Coordinates rockPos, char[,] rock)
    {
        if (rockPos.X < 0 || rockPos.X + Rock.Width(rock) > Chamber.GetLength(0) || rockPos.Y < 0)
            return false;

        for (int y = 0; y < Rock.Height(rock); y++)
        {
            for (int x = 0; x < Rock.Width(rock); x++)
            {
                if (rock[x, y] == default(char))
                    continue;

                if (Chamber[rockPos.X + x, rockPos.Y + y] != default(char))
                    return false;
            }
        }

        return true;
    }

    private EDirection GetHorizontalDirection() => (EDirection)Pattern[moveCounter % Pattern.Length];

    private char[,] GetProperRock()
    {
        int modResult = rockCounter % 5;

        if (modResult == 0)
            return Rock.LineHorizontal;
        else if (modResult == 1)
            return Rock.Plus;
        else if (modResult == 2)
            return Rock.Corner;
        else if (modResult == 3)
            return Rock.LineVertical;
        else if (modResult == 4)
            return Rock.Box;

        throw new Exception();
    }
}

class Coordinates
{
    public int X { get; set; }
    public int Y { get; set; }

    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coordinates GetCoordinatesAfterMove(EDirection direction)
    {
        if (direction == EDirection.Left)
            return new Coordinates(X - 1, Y);
        if (direction == EDirection.Right)
            return new Coordinates(X + 1, Y);
        if (direction == EDirection.Down)
            return new Coordinates(X, Y - 1);

        throw new Exception();
    }
}

static class Rock
{
    public static char[,] LineHorizontal => new char[4, 1] { { '#' }, { '#' }, { '#' }, { '#' } };
    public static char[,] Plus => new char[3, 3] { { '\0', '#', '\0' }, { '#', '#', '#' }, { '\0', '#', '\0' } };
    public static char[,] Corner => new char[3, 3] { { '#', '\0', '\0' }, { '#', '\0', '\0' }, { '#', '#', '#' } };
    public static char[,] LineVertical => new char[1, 4] { { '#', '#', '#', '#' } };
    public static char[,] Box => new char[2, 2] { { '#', '#' }, { '#', '#' } };

    public static int Width(char[,] rock) => rock.GetLength(0);
    public static int Height(char[,] rock) => rock.GetLength(1);
}