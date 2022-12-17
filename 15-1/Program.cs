using System.Diagnostics;

string[] text = File.ReadAllLines(@"../../../input.txt");

List<SensorBeacon> pairs = new();

foreach (string line in text)
{
    string[] parts = line.Split(":");
    Coordinates sensorCoord = new Coordinates(parts[0]);
    Coordinates beaconCoord = new Coordinates(parts[1]);

    pairs.Add(new SensorBeacon(sensorCoord, beaconCoord));
}

//PartOne(10);
//PartOne(2000000);

//PartTwo(20);
PartTwo(4000000);

void PartTwo(int range)
{
    var s = new Stopwatch();
    for (int y = 0; y < range; y++)
    {
        List<Range> ranges = new();

        foreach (SensorBeacon sb in pairs)
            ranges.Add(sb.GetRangeForY(y));

        var result = ranges
            .Where(x => !x.IsEmpty)
            .OrderBy(x => x.XFrom)
            .ThenBy(x => x.XTo)
            .ToList();

        for (int i = 0; i < result.Count - 1; i++)
        {
                if (result[i].XFrom < 0)
                    result[i].XFrom = 0;
                if (result[i].XTo > range)
                    result[i].XTo = range;
            result[i].Intersect(result[i + 1]);
        }

        result = result.Where(x => !x.IsEmpty).ToList();

        int max = -1;
        foreach(Range r in result)
        {
            if(max + 1 < r.XFrom)
                Console.WriteLine((max + 1) * 4000000 + y);

            max = Math.Max(max, r.XTo);
        }
    }
}

void PartOne(int y)
{
    List<Range> ranges = new();

    foreach (SensorBeacon sb in pairs)
        ranges.Add(sb.GetRangeForY(y));

    var beaconCoords = pairs.Where(x => x.Beacon.Y == y).Select(x => x.Beacon.X);

    var result = ranges.Where(x => !x.IsEmpty).SelectMany(x => x.Values).Distinct().Where(x => !beaconCoords.Contains(x)).Count();

    Console.WriteLine(result);
}

class Range
{
    public int XFrom;
    public int XTo;

    public Range(int xFrom, int xTo)
    {
        XFrom = xFrom;
        XTo = xTo;
    }

    public bool IsEmpty => XFrom > XTo;

    public static Range Empty => new(0, -1);

    public IEnumerable<int> Values => IsEmpty ? Enumerable.Empty<int>() : Enumerable.Range(XFrom, XTo - XFrom + 1);

    public bool Overlaps(Range other) => !IsEmpty
                                        && !other.IsEmpty
                                        && XFrom <= other.XTo
                                        && XTo >= other.XFrom;

    public Range Intersect(Range other) => Overlaps(other) ? new(Math.Max(XFrom, other.XFrom), Math.Min(XTo, other.XTo)) : Empty;

    public override string ToString() => XFrom.ToString() + " - " + XTo.ToString();
}

class SensorBeacon
{
    public Coordinates Sensor { get; set; }
    public Coordinates Beacon { get; set; }

    public SensorBeacon(Coordinates sensor, Coordinates beacon)
    {
        Sensor = sensor;
        Beacon = beacon;
    }

    public Range GetRangeForY(int y)
    {
        int distanceToNearestBeacon = Sensor.GetDistanceTo(Beacon);
        int distanceToYLine = Sensor.GetDistanceTo(new Coordinates(Sensor.X, y));
        int radius = distanceToNearestBeacon - distanceToYLine;

        return new Range(Sensor.X - radius, Sensor.X + radius);
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