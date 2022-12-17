string[] text = File.ReadAllLines(@"../../../input.txt");
int count = 0;
foreach (string line in text)
{
    string[] ranges = line.Split(',');

    Range rangeOne = new Range(ranges[0]);
    Range rangeTwo = new Range(ranges[1]);

    //if(rangeOne.IsInside(rangeTwo) || rangeTwo.IsInside(rangeOne))
    //    count++;
    if (rangeOne.IsOverlap(rangeTwo) || rangeTwo.IsOverlap(rangeOne))
        count++;
}
Console.WriteLine(count);

class Range
{
    public int Start;
    public int End;

    public Range(string input)
    {
        string[] values = input.Split('-');

        Start = int.Parse(values[0]);
        End = int.Parse(values[1]);
    }

    public bool IsInside(Range range) => Start >= range.Start && End <= range.End;

    public bool IsOverlap(Range range) => (Start >= range.Start && Start <= range.End) || (End >= range.Start && End <= range.End);
}