string text = File.ReadAllText(@"../../../input.txt");

for(int i = 0; i < text.Length-14; i++)
{
    string part = text.Substring(i, 14);
    if (part.Distinct().Count() == part.Length)
    {
        Console.WriteLine(i + 14 + " - " + part);
        break;
    }
}