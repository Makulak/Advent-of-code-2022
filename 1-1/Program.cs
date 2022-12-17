string[] text = File.ReadAllLines(@"../../../input.txt");

int[] sums = new int[text.Length];
int counter = 0;

for (int i = 0; i < text.Length; i++)
{
    if (text[i] == string.Empty)
    {
        counter++;
        continue;
    }
    sums[counter] += int.Parse(text[i]);
}

Array.Sort(sums);
Array.Reverse(sums);

Console.WriteLine(sums[0] + sums[1] + sums[2]);