string[] text = File.ReadAllLines(@"../../../input.txt");

int sum = 0;

for (int i = 0; i+2 < text.Length; i += 3)
{
    char item = getSharedItemForBackpacks(new string[] { text[i], text[i+1], text[i+2] });
    sum += getPriority(item);
}

Console.WriteLine(sum);

char getSharedItem(string line)
{
    string firstCompartment = line.Substring(0, line.Length / 2);
    string secondCompartment = line.Substring(line.Length / 2);

    return firstCompartment.Where(x => secondCompartment.Contains(x)).First();
}

char getSharedItemForBackpacks(string[] lines)
{
    return lines[0].Where(x => lines[1].Contains(x) && lines[2].Contains(x)).First();
}

int getPriority(char item)
{
    return Char.IsUpper(item) ? (int)item % 32 + 26 : (int)item % 32;
}