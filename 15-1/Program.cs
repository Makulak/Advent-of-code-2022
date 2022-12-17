string[] text = File.ReadAllLines(@"../../../input.txt");

int yPosOfLine = 0;

List<Coordinates> coordinatesOnYLine = new();
List<Coordinates> beaconCoords = new();

foreach (string line in text)
{
    Console.Clear();
    string[] parts = line.Split(":");
    Coordinates sensorCoord = new Coordinates(parts[0]);
    Coordinates beaconCoord = new Coordinates(parts[1]);

    beaconCoords.Add(beaconCoord);

    int distanceToNearestBeacon = sensorCoord.GetDistanceTo(beaconCoord);
    int distanceToYLine = sensorCoord.GetDistanceTo(new Coordinates(sensorCoord.X, yPosOfLine));
    int impossiblePointsCount = (distanceToNearestBeacon - distanceToYLine) * 2;

    for (int x = 0; x < (int)Math.Floor((double)impossiblePointsCount / 2) + 1; x++)
    {
        Coordinates right = new Coordinates(sensorCoord.X + x, yPosOfLine);
        Coordinates left = new Coordinates(sensorCoord.X - x, yPosOfLine);

        if (!beaconCoords.Any(b => b.IsEqual(right)))
            coordinatesOnYLine.Add(right);
        if (!beaconCoords.Any(b => b.IsEqual(left)))
            coordinatesOnYLine.Add(left);
    }
}
int sum = coordinatesOnYLine.Select(c => c.X).Distinct().Count();

Console.WriteLine(sum);

class SensorBeacon
{
    public Coordinates Sensor { get; set; }
    public Coordinates Beacon { get; set; }
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

    public int GetDistanceTo(Coordinates destination)
    {
        int MinX = Math.Min(destination.X, X);
        int MaxX = Math.Max(destination.X, X);
        int MinY = Math.Min(destination.Y, Y);
        int MaxY = Math.Max(destination.Y, Y);

        return MaxX - MinX + MaxY - MinY;
    }

    public bool IsEqual(Coordinates coord)
    {
        return X == coord.X && Y == coord.Y;
    }
}