using System.Text.Json;
using System.Text.RegularExpressions;

string[] text = File.ReadAllLines(@"../../../input.txt");

int count = 0;
int exampleCount = 1;
for (int i = 0; i < text.Length; i += 3)
{
    string first = text[i];
    string second = text[i + 1];

    if (map(first, second))
        count += exampleCount;

    exampleCount++;
}

Console.WriteLine(count);

bool map(string first, string second)
{
    if (first == "" && second != "")
        return true;

    bool firstArrOk = IsValidArray(first, out int[] firstArr);
    bool secondArrOk = IsValidArray(second, out int[] secondArr);

    if (firstArrOk && secondArrOk)
        return Validate(firstArr, secondArr);

    List<string> firstList = GetElementsToCompare(first);
    List<string> secondList = GetElementsToCompare(second);

    for (int i = 0; i < Math.Min(firstList.Count, secondList.Count); i++)
    {
        if (!map(firstList[i], secondList[i]))
            return false;
    }

    return true;
}

List<string> GetElementsToCompare(string input)
{
    var result = new List<string>();

    var trimmedInput = input.StartsWith('[') && input.EndsWith(']') ? input.Substring(1, input.Length - 2) : input;

    var dividersIdx = trimmedInput
        .AllIndexesOf(",")
        .Where(idx =>
            trimmedInput
            .Substring(0, idx)
            .Where(c => c == '[')
            .Count()
            ==
            trimmedInput
            .Substring(0, idx)
            .Where(c => c == ']')
            .Count())
        .ToList();

    dividersIdx.Add(0);
    dividersIdx.Add(trimmedInput.Length);
    dividersIdx.Sort();

    if (dividersIdx.Count() == 0)
    {
        result.Add(trimmedInput);
        return result;
    }

    for (int i = 0; i < dividersIdx.Count - 1; i++)
    {
        var x = trimmedInput.Substring(dividersIdx[i], dividersIdx[i + 1] - dividersIdx[i]).Trim(',');
        if (x.StartsWith('[') && !x.EndsWith(']'))
            x = x.TrimStart('[');
        if (!x.StartsWith('[') && x.EndsWith(']'))
            x = x.TrimEnd(']');
        if (x.StartsWith('[') && x.EndsWith(']') && x.Where(a => a == '[').Count() != x.Where(a => a == ']').Count())
            x = "[" + x.Trim('[').Trim(']') + "]";

        result.Add(x);
    }
    return result;
}

bool Validate(int[] firstArr, int[] secondArr)
{
    if (secondArr == null && firstArr != null)
        return false;
    if (secondArr != null && firstArr == null)
        return true;

    int minLength = Math.Min(firstArr.Length, secondArr.Length);
    for (int i = 0; i < minLength; i++)
    {
        if (firstArr[i] > secondArr[i])
            return false;
        if (firstArr[i] < secondArr[i])
            return true;
    }
    if (firstArr.Length <= secondArr.Length)
        return true;

    return false;
}

bool IsValidArray(string input, out int[] output)
{
    try
    {
        output = String.IsNullOrEmpty(input) ? null : JsonSerializer.Deserialize<int[]>(("[" + input + "]").Replace("[[", "[").Replace("]]", "]"));
        return true;
    }
    catch
    {
        output = null;
        return false;
    }
}

public static class Ext
{
    public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
    {
        int minIndex = str.IndexOf(searchstring);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
        }
    }
}

//using System.Collections.Immutable;

//var input = ParseInput(File.ReadLines(args.FirstOrDefault() ?? @"../../../input.txt")).ToList();

//var result1 = from i in Enumerable.Range(0, input.Count)
//let pair = input[i]
//where pair[0].CompareTo(pair[1]) <= 0
//select i + 1;

//Console.WriteLine($"Part 1 Result = {result1.Sum()}");

//var dividers = new[] { ParseList("[[2]]", out _), ParseList("[[6]]", out _) };
//var result2 = input.SelectMany(x => x)
//                   .Concat(dividers)
//                   .OrderBy(x => x)
//                   .Select((x, i) => (item: x, pos: i + 1))
//                   .Where(x => dividers.Contains(x.item))
//                   .Select(x => x.pos)
//                   .Aggregate((a, b) => a * b);

//Console.WriteLine($"Part 2 Result = {result2}");

//static IEnumerable<ImmutableArray<ListItem>> ParseInput(IEnumerable<string> input)
//  => input.Where(x => !string.IsNullOrEmpty(x))
//          .Select(line => ParseList(line, out _))
//          .Chunk(2)
//          .Select(p => p.ToImmutableArray());

//static ListItem ParseList(ReadOnlySpan<char> input, out ReadOnlySpan<char> tail)
//{
//input = input[1..];
//var result = ImmutableList.CreateBuilder<Item>();
//while (input.Length > 0)
//{
//var ch = input[0];
//if (ch == ']')
//break;
//else if (ch == ',')
//input = input[1..];
//else
//result.Add(ParseItem(input, out input));
//}
//tail = input[1..];

//return new ListItem(result.ToImmutable());
//}

//static NumberItem ParseNumber(ReadOnlySpan<char> input, out ReadOnlySpan<char> tail)
//{
//var length = input.IndexOfAny(',', ']');
//tail = input[length..];
//return new NumberItem(int.Parse(input[..length]));
//}

//static Item ParseItem(ReadOnlySpan<char> input, out ReadOnlySpan<char> tail)
//  => input[0] switch
//{
//'[' => ParseList(input, out tail),
//_ => ParseNumber(input, out tail)
//};

//abstract record Item : IComparable<Item>
//{
//    public int CompareTo(Item? other)
//      => (this, other) switch
//      {
//          (NumberItem num1, NumberItem num2) => num1.Value.CompareTo(num2.Value),
//          (ListItem list1, ListItem list2) => list1.CompareTo(list2),
//          (NumberItem num, ListItem list) => num.ToListItem().CompareTo(list),
//          (ListItem list, NumberItem num) => list.CompareTo(num.ToListItem()),
//          _ => throw new InvalidOperationException()
//      };
//}

//record NumberItem(int Value) : Item
//{
//    public ListItem ToListItem() => new(ImmutableList.Create(this as Item));
//}

//record ListItem(ImmutableList<Item> Items) : Item, IComparable<ListItem>
//{
//    public int CompareTo(ListItem? other)
//    {
//        other ??= new(ImmutableList<Item>.Empty);

//        var i = 0;
//        while (i < Items.Count && i < other.Items.Count)
//        {
//            var cmp = Items[i].CompareTo(other.Items[i]);
//            if (cmp != 0)
//                return cmp;

//            ++i;
//        }

//        return Items.Count.CompareTo(other.Items.Count);
//    }
//}