// See https://aka.ms/new-console-template for more information
string[] text = File.ReadAllLines(@"../../../input.txt");

int sum = 0;

int max = 0;

for (int y = 0; y < text.Length; y++)
{
    string line = text[y];

    for (int x = 0; x < line.Length; x++)
    {
        isVisible(text, y, x);
        //if (isVisible(text, y, x))
        //    sum++;
    }
}

Console.WriteLine(max);

bool isVisible(string[] forest, int y, int x)
{
    if (y == 0 || x == 0 || y == forest.Length - 1 || x == forest[0].Length - 1)
        return true;

    string horLine = forest[y];
    string vertLine = "";
    foreach(string line in forest)
    {
        vertLine += line[x];
    }
    int treeHeight = int.Parse(horLine[x].ToString());

    //return handleFirstTask(horLine, vertLine, treeHeight, x, y);

    handleSecondTask(horLine, vertLine, treeHeight, x, y);

    return true;
}

 int handleSecondTask(string horLine, string vertLine, int treeHeight, int x, int y) {
    string leftSide = new String(horLine.Substring(0, x).Reverse().ToArray());
    string rightSide = horLine.Substring(x + 1);
    string upSide = new String(vertLine.Substring(0, y).Reverse().ToArray());
    string downSide = vertLine.Substring(y + 1);

    int lc = getIndex(leftSide, treeHeight) + 1;
    int rc = getIndex(rightSide, treeHeight) + 1;
    int uc = getIndex(upSide, treeHeight) + 1;
    int dc = getIndex(downSide, treeHeight) + 1;

    int result = lc * rc * uc * dc;

    if (result > max)
        max = result;

    return result;
}

int getIndex(string text, int minVal)
{
    if (string.IsNullOrEmpty(text))
        return -1;
    for(int i =0;i<text.Length; i++)
    {
        if (int.Parse(text[i].ToString()) >= minVal)
            return i;
    }
    return text.Length-1;
}

bool handleFirstTask(string horLine, string vertLine, int treeHeight, int x, int y)
{
    string leftSide = horLine.Substring(0, x);
    string rightSide = horLine.Substring(x + 1);
    string upSide = vertLine.Substring(0, y);
    string downSide = vertLine.Substring(y + 1);

    return isVisibleInLine(leftSide, treeHeight) || isVisibleInLine(rightSide, treeHeight) || isVisibleInLine(upSide, treeHeight) || isVisibleInLine(downSide, treeHeight);
}

bool isVisibleInLine (string line, int height) => line.Max(x => int.Parse(x.ToString())) < height;