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
    }
    if (firstArr.Length > secondArr.Length && minLength > 0 && firstArr[minLength - 1] >= secondArr[minLength - 1])
        return false;

    return true;
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