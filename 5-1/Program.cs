using System.Text.RegularExpressions;

string[] text = File.ReadAllLines(@"../../../input.txt");

int separatorIdx = Array.FindIndex(text, x => string.IsNullOrEmpty(x));

List<string> operations = text.Skip(separatorIdx + 1).ToList();

List<Stack<string>> stacks = new List<Stack<string>>();
stacks.Add(new Stack<string>(new string[] { "S", "M", "R", "N", "W", "J", "V", "T" }));
stacks.Add(new Stack<string>(new string[] { "B", "W", "D", "J", "Q", "P", "C", "V" }));
stacks.Add(new Stack<string>(new string[] { "B", "J", "F", "H", "D", "R", "P" }));
stacks.Add(new Stack<string>(new string[] { "F", "R", "P", "B", "M", "N", "D" }));
stacks.Add(new Stack<string>(new string[] { "H", "V", "R", "P", "T", "B" }));
stacks.Add(new Stack<string>(new string[] { "C", "B", "P", "T" }));
stacks.Add(new Stack<string>(new string[] { "B", "J", "R", "P", "L" }));
stacks.Add(new Stack<string>(new string[] { "N", "C", "S", "L", "T", "Z", "B", "W" }));
stacks.Add(new Stack<string>(new string[] { "L", "S", "G" }));

//stacks.Add(new Stack<string>(new string[] { "Z", "N" }));
//stacks.Add(new Stack<string>(new string[] { "M", "C", "D" }));
//stacks.Add(new Stack<string>(new string[] { "P" }));


foreach (string operation in operations)
{
    var resultString = Regex.Matches(operation, @"\d+").ToList();
    int count = int.Parse(resultString[0].ToString());
    int from = int.Parse(resultString[1].ToString());
    int to = int.Parse(resultString[2].ToString());

    List<string> takenCrates = new List<string>();

    for (int i = 0; i < count; i++)
    {
        takenCrates.Add(stacks[from - 1].Pop());
    }
    takenCrates.Reverse();
    takenCrates.ForEach(x => stacks[to - 1].Push(x));
}

stacks.ForEach(x => Console.Write(x.Pop()));