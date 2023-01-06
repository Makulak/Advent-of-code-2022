string[] text = File.ReadAllLines(@"../../../input.txt");

int size = 22;
char[,,] cube = new char[size, size, size];
int surface = 0;

foreach (string line in text)
{
    string[] coordinates = line.Split(',');
    int x = int.Parse(coordinates[0]);
    int y = int.Parse(coordinates[1]);
    int z = int.Parse(coordinates[2]);

    cube[x + 1, y + 1, z + 1] = '#';
}

for (int x = 1; x < size; x++)
{
    for (int y = 1; y < size; y++)
    {
        for (int z = 1; z < size; z++)
        {
            if (cube[x, y, z] != '#')
            {
                if (IsInside(x, y, z))
                    cube[x, y, z] = '@';
            }
        }
    }
}

for (int x = 0; x < size; x++)
{
    for (int y = 0; y < size; y++)
    {
        for (int z = 0; z < size; z++)
        {
            Console.Write(cube[x, y, z] == default(char) ? ". " : cube[x, y, z] + " ");
        }
        Console.WriteLine();
    }
    Console.ReadLine();
    Console.Clear();
}

for (int x = 1; x < size; x++)
{
    for (int y = 1; y < size; y++)
    {
        for (int z = 1; z < size; z++)
        {
            if (cube[x, y, z] != default(char))
            {
                if (cube[x + 1, y, z] == default(char))
                    surface++;
                if (cube[x - 1, y, z] == default(char))
                    surface++;
                if (cube[x, y + 1, z] == default(char))
                    surface++;
                if (cube[x, y - 1, z] == default(char))
                    surface++;
                if (cube[x, y, z + 1] == default(char))
                    surface++;
                if (cube[x, y, z - 1] == default(char))
                    surface++;
            }
        }
    }
}

bool IsInside(int x, int y, int z)
{
    var xx = Enumerable.Range(0, size).Select(i => cube[i, y, z]).ToList();
    var isInX = xx.IndexOf('#') < x && xx.LastIndexOf('#') > x;

    var yy = Enumerable.Range(0, size).Select(i => cube[x, i, z]).ToList();
    var isInY = yy.IndexOf('#') < y && yy.LastIndexOf('#') > y;

    var zz = Enumerable.Range(0, size).Select(i => cube[x, y, i]).ToList();
    var isInZ = zz.IndexOf('#') < z && zz.LastIndexOf('#') > z;

    return isInX && isInY && isInZ;
}

Console.WriteLine(surface);