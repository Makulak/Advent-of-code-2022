string[] text = File.ReadAllLines(@"../../../input.txt");

int cyclesNeededToCompleteOperation = 0;
int X = 1;
int cyclesCount = 0;

int signalSums = 0;

for (int i = 0; i < text.Length; i++)
{
    string[] line = text[i].Split(' ');

    if (line[0].StartsWith("addx"))
        cyclesNeededToCompleteOperation += 2;
    else if (line[0].StartsWith("noop"))
        cyclesNeededToCompleteOperation += 1;

    while (cyclesNeededToCompleteOperation > 0)
    {
        cyclesCount++;
        cyclesNeededToCompleteOperation--;

        if (new int[] { X - 1, X, X + 1 }.Contains((cyclesCount-1) % 40))
            Console.Write("#");
        else
            Console.Write(".");

        if (cyclesCount % 40 == 0)
            Console.WriteLine();

        if (IsImportantCycle())
            signalSums += cyclesCount * X;
    }

    if (line[0].StartsWith("addx"))
        X += int.Parse(line[1]);
}

bool IsImportantCycle() => (cyclesCount - 20) % 40 == 0;
