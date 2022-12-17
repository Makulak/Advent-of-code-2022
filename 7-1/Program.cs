// See https://aka.ms/new-console-template for more information
string[] text = File.ReadAllLines(@"../../../input.txt");

Node currentNode = Node.CreateDirectory("/");

for (int i = 1; i < text.Length; i++) // Ignore first line with directory '/'
{
    if (text[i].StartsWith("$"))
    {
        string[] parts = text[i].Split(' ');
        string operation = parts[1];

        if (operation == "cd")
        {
            string name = parts[2];
            if (name == "..")
                currentNode = currentNode.Parent;
            else
                currentNode = currentNode.Childrens.First(x => x.Name == name);
        }
        else if (operation == "ls")
        {
            do
            {
                i++;
                string data = text[i];
                if (data.StartsWith("dir"))
                {
                    string dirName = data.Split(' ')[1];

                    currentNode.AddChildren(Node.CreateDirectory(dirName));
                }
                else
                {
                    int size = int.Parse(data.Split(' ')[0]);
                    string name = data.Split(' ')[1];

                    currentNode.AddChildren(Node.CreateFile(name, size));
                }
            } while (i + 1 < text.Length && !text[i + 1].StartsWith('$'));
        }
    }
}

while (currentNode.Parent != null)
    currentNode = currentNode.Parent;

int sum = SumSizes(currentNode);

int freeSize = 70000000 - currentNode.Size;
int missingSize = 30000000 - freeSize;

List<int> smallestDirToRemove = FlattenSizes(currentNode).Where(x => x > missingSize).ToList();
smallestDirToRemove.Sort();

var x = FlattenSizes(currentNode).Where(x => x < 100000).Sum();


Console.WriteLine(smallestDirToRemove.First());

List<int> FlattenSizes(Node node)
{
    List<int> sizes = new List<int>();

    if (node.Type == EType.Directory)
        sizes.Add(node.Size);

    if (node.Childrens != null)
        foreach (Node child in node.Childrens)
        {
            sizes.AddRange(FlattenSizes(child));
        }

    return sizes;
}

int SumSizes(Node node)
{
    int sum = 0;

    if (node.Size < 100000 && node.Type == EType.Directory && node.Name != "/")
        sum += node.Size;

    if (node.Childrens != null)
        foreach (Node child in node.Childrens)
        {
            sum += SumSizes(child);
        }

    return sum;
}

public enum EType
{
    Directory,
    File
}

public class Node
{
    private int size;

    public string Name { get; private set; }

    public EType Type { get; private set; }

    public List<Node> Childrens { get; private set; }

    public Node Parent { get; private set; }

    public int Size => Type == EType.File ? size : Childrens?.Sum(c => c.Size) ?? 0;

    private Node() { }

    public static Node CreateFile(string name, int size)
    {
        return new Node
        {
            Name = name,
            Type = EType.File,
            size = size,
            Childrens = new()
        };
    }

    public static Node CreateDirectory(string name)
    {
        return new Node
        {
            Name = name,
            Type = EType.Directory,
            size = 0,
            Childrens = new()
        };
    }

    public void AddChildren(Node node)
    {
        node.Parent = this;
        Childrens.Add(node);
    }
}
