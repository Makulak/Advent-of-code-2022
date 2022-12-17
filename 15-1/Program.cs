string[] text = File.ReadAllLines(@"../../../input.txt");
var field = new FieldH(100000, 50000);

foreach (string line in text)
{
    Console.Clear();
    string[] parts = line.Split(":");
    Coordinates sensorCoord = new Coordinates(parts[0]);
    Coordinates beaconCoord = new Coordinates(parts[1]);

    field.AddSensorAndBeacon(sensorCoord, beaconCoord);
    //field.WriteGridToConsole();
}
    
Console.WriteLine(field.WriteGridToConsole(60));

class FieldH
{
    public char[,] Field { get; set; }
    public int MinusMargin { get; set; }

    public FieldH(int size, int minusMargin)
    {
        Field = new char[size, size];
        MinusMargin = minusMargin;
    }

    public void AddMarginToCoordinates(Coordinates coord)
    {
        coord.X += MinusMargin;
        coord.Y += MinusMargin;
    }

    public void AddSensorAndBeacon(Coordinates sensor, Coordinates beacon)
    {
        AddMarginToCoordinates(sensor);
        AddMarginToCoordinates(beacon);

        int radius = GetDistance(sensor, beacon);
        MarkRadius(sensor, radius);

        Field[sensor.X, sensor.Y] = 'S';
        Field[beacon.X, beacon.Y] = 'B';
    }

    private void MarkRadius(Coordinates sensor, int maxDistance)
    {
        for (int y = 0; y < Field.GetLength(0); y++)
        {
            for (int x = 0; x < Field.GetLength(1); x++)
            {
                if (GetDistance(sensor, new Coordinates(x, y)) <= maxDistance && Field[x, y] == default(char))
                    Field[x, y] = '#';
            }
        }
    }

    public int WriteGridToConsole(int? countLineNo = null)
    {
        int counter = 0;
        for (int y = 0; y < Field.GetLength(0); y++)
        {
            for (int x = 0; x < Field.GetLength(1); x++)
            {
                //Console.Write(Field[x, y] == default(char) ?'.' : Field[x, y]);

                if (y == countLineNo && Field[x, y] == '#')
                    counter++;
            }
            //Console.WriteLine();
        }
        //Console.WriteLine();
        //Console.WriteLine();

        return counter;
    }

    private int GetDistance(Coordinates sensor, Coordinates beacon)
    {
        int MinX = Math.Min(sensor.X, beacon.X);
        int MaxX = Math.Max(sensor.X, beacon.X);
        int MinY = Math.Min(sensor.Y, beacon.Y);
        int MaxY = Math.Max(sensor.Y, beacon.Y);

        return MaxX - MinX + MaxY - MinY;
    }
}

class Coordinates
{
    public int X { get; set; }
    public int Y { get; set; }

    public Coordinates(string input)
    {
        string[] splitInput = input.Split('=');
        X = int.Parse(splitInput[1].Split(',')[0]);
        Y = int.Parse(splitInput[2]);
    }

    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }
}