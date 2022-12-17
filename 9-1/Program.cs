// See https://aka.ms/new-console-template for more information
string[] text = File.ReadAllLines(@"../../../input.txt");

Field field = new Field(2);

foreach (string line in text)
{
    string[] split = line.Split(' ');

    EDirection direction = (EDirection)split[0][0];
    int count = int.Parse(split[1]);

    field.Move(direction, count);
}

Console.WriteLine(field.DistinctVisitedCoords.Count);

public enum EDirection
{
    Up = 'U',
    Down = 'D',
    Left = 'L',
    Right = 'R',
}

public class CoordinatesComparer : IEqualityComparer<Coordinates>
{
    public bool Equals(Coordinates x, Coordinates y)
    {
        return x.Y == y.Y && x.X == y.X;
    }

    public int GetHashCode(Coordinates obj)
    {
        return obj.GetHashCode();
    }
}


public class Coordinates
{
    public int X { get; set; }
    public int Y { get; set; }

    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coordinates(Coordinates coords)
    {
        X = coords.X;
        Y = coords.Y;
    }

    public void Move(EDirection direction)
    {
        switch (direction)
        {
            case EDirection.Up:
                Y--;
                break;
            case EDirection.Down:
                Y++;
                break;
            case EDirection.Left:
                X--;
                break;
            case EDirection.Right:
                X++;
                break;
            default:
                break;
        }
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    public override string ToString()
    {
        return "X: " + X + " Y: " + Y;
    }
}

class Field
{
    public List<Coordinates> Knots { get; set; }

    public List<Coordinates> VisitedCoords { get; set; }

    public List<Coordinates> DistinctVisitedCoords => VisitedCoords.Distinct(new CoordinatesComparer()).ToList();

    public Field(int knotsCount)
    {
        VisitedCoords = new();

        Knots = new();
        Enumerable.Range(0, knotsCount).ToList().ForEach(x => Knots.Add(new Coordinates(0, 0)));
    }

    public void Move(EDirection direction, int count)
    {
        for (int i = 0; i < count; i++)
        {

            Knots[0].Move(direction);
            for (int j = 1; j < Knots.Count; j++)
            {
                MoveKnots(Knots[j - 1], Knots[j]);
            }

            VisitedCoords.Add(new Coordinates(Knots.Last()));
        }
    }

    private void MoveKnots(Coordinates knotAhead, Coordinates knotBehind)
    {
        int xDiff = knotAhead.X - knotBehind.X;
        int yDiff = knotAhead.Y - knotBehind.Y;

        if (xDiff > 1 || (xDiff == 1 && (yDiff > 1 || yDiff < -1)))
            knotBehind.Move(EDirection.Right);
        else if (xDiff < -1 || (xDiff == -1 && (yDiff > 1 || yDiff < -1)))
            knotBehind.Move(EDirection.Left);

        if (yDiff > 1 || (yDiff == 1 && (xDiff > 1 || xDiff < -1)))
            knotBehind.Move(EDirection.Down);
        else if (yDiff < -1 || (yDiff == -1 && (xDiff > 1 || xDiff < -1)))
            knotBehind.Move(EDirection.Up);
    }
}