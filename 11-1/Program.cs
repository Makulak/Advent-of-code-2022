using System.Text.Json;

string[] text = File.ReadAllLines(@"../../../input.txt");

List<Monke> monkes = new();

for (int i = 0; i < text.Length; i += 7)
{
    int MonkeNumber = int.Parse(text[i].Replace("Monkey ", "")[0].ToString());
    List<decimal> MonkeItems = JsonSerializer.Deserialize<List<decimal>>(text[i + 1].Replace("Starting items: ", "").Insert(0, "[").Append(']').ToArray());
    EOperation MonkeOperation = text[i + 2].Contains('*') ? EOperation.Multiple : EOperation.Add;
    string MonkeOperationValue = text[i + 2].Split(" ").Last();
    int MonkeTestNumber = int.Parse(text[i + 3].Split(" ").Last());
    int MonkeThrowToWhenTrue = int.Parse(text[i + 4].Split(" ").Last());
    int MonkeThrowToWhenFalse = int.Parse(text[i + 5].Split(" ").Last());

    monkes.Add(new Monke
    {
        MonkeNumber = MonkeNumber,
        Items = new Queue<decimal>(MonkeItems),
        Operation = MonkeOperation,
        OperationValue = MonkeOperationValue,
        DivNumber = MonkeTestNumber,
        ThrowWhenFalseTo = MonkeThrowToWhenFalse,
        ThrowWhenTrueTo = MonkeThrowToWhenTrue
    });
}

for(int i = 0; i < 10000; i++)
{
    var div = monkes.Aggregate(1, (div, monkey) => div * monkey.DivNumber);
    foreach(var monke in monkes)
    {
        while (monke.Items.Count > 0)
        {
            dynamic result = monke.Run();
            if (result != null)
            {
                monkes.First(x => x.MonkeNumber == result.destination).Items.Enqueue(result.value % div);
            }
        }
    }
    if (i % 50 == 0)
        Console.WriteLine("Progress: " + i);
}

var x = monkes.Select(x => x.OperationCount).ToList();
x.Sort();

Console.WriteLine(x[x.Count-1] + " " + x[x.Count-2]);

enum EOperation
{
    Multiple,
    Add
}

class Monke
{
    public int MonkeNumber { get; set; }

    public Queue<decimal> Items { get; set; }

    public EOperation Operation { get; set; }

    public string OperationValue { get; set; }

    public int DivNumber { get; set; }

    public int ThrowWhenFalseTo { get; set; }
    public int ThrowWhenTrueTo { get; set; }

    public int OperationCount { get; set; }

    public dynamic Run()
    {
        if (Items.Count == 0)
            return null;

        OperationCount++;

        decimal item = Items.Dequeue();

        decimal result = CalculateOperation(item);
        //int result = (int)Math.Floor((decimal)CalculateOperation(item) / 3);

        if (TestPassed(result))
            return new
            {
                destination = ThrowWhenTrueTo,
                value = result
            };

        return new
        {
            destination = ThrowWhenFalseTo,
            value = result
        };
    }

    private decimal CalculateOperation(decimal itemVal)
    {
        decimal operationNumber = itemVal;
        if(decimal.TryParse(OperationValue, out var result))
            operationNumber = result;

        if (Operation == EOperation.Multiple)
            return itemVal * operationNumber;
        else if (Operation == EOperation.Add)
            return itemVal + operationNumber;

        throw new Exception("Unknown operation");
    }

    private bool TestPassed(decimal itemVal)
    {
        return itemVal % DivNumber == 0;
    }
}