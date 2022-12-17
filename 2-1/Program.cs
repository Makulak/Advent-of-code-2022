namespace App
{
    enum Move
    {
        Rock,
        Paper,
        Scissors
    }

    enum Result
    {
        Lose,
        Draw,
        Win
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string[] text = File.ReadAllLines(@"../../../input.txt");

            int sum = 0;

            foreach (string line in text)
            {
                string[] moves = line.Split(' ');

                Move opponentMove = getMoveFromInput(moves[0]);
                Result result = getResultFromInput(moves[1]);

                if (result == Result.Draw)
                    sum += 3;
                if (result == Result.Win)
                    sum += 6;

                Move myMove = getProperMove(opponentMove, result);

                sum += getPointsForMove(myMove);
            }

            Console.WriteLine(sum);
        }

        static Move getProperMove(Move opponentMove, Result result)
        {
            if (result == Result.Draw)
                return opponentMove;
            if(result == Result.Lose)
            {
                if (opponentMove == Move.Rock)
                    return Move.Scissors;
                if (opponentMove == Move.Scissors)
                    return Move.Paper;
                else
                    return Move.Rock;
            }
            if (result == Result.Win)
            {
                if (opponentMove == Move.Rock)
                    return Move.Paper;
                if (opponentMove == Move.Scissors)
                    return Move.Rock;
                else
                    return Move.Scissors;
            }

            throw new Exception();
        }

        static Move getMoveFromInput(string input)
        {
            if (input == "A" || input == "X")
                return Move.Rock;
            if (input == "B" || input == "Y")
                return Move.Paper;
            else
                return Move.Scissors;
        }

        static Result getResultFromInput(string input)
        {
            if (input == "X")
                return Result.Lose;
            if (input == "Y")
                return Result.Draw;
            else
                return Result.Win;
        }

        static int getPointsForMove(Move move)
        {
            if (move == Move.Rock)
                return 1;
            if (move == Move.Paper)
                return 2;
            else
                return 3;
        }
    }
}

//namespace App
//{
//    enum Move
//    {
//        Rock,
//        Paper,
//        Scissors 
//    }

//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            string[] text = File.ReadAllLines(@"../../../input.txt");

//            int sum = 0;

//            foreach (string line in text)
//            {
//                string[] moves = line.Split(' ');

//                Move opponentMove = getMoveFromInput(moves[0]);
//                Move myMove = getMoveFromInput(moves[1]);

//                sum += getPointsForMove(myMove);

//                if (opponentMove == myMove)
//                    sum += 3;
//                else if ((opponentMove == Move.Scissors && myMove == Move.Rock) || (opponentMove == Move.Paper && myMove == Move.Scissors) || (opponentMove == Move.Rock && myMove == Move.Paper))
//                    sum += 6;
//            }

//            Console.WriteLine(sum);
//        }

//        static Move getMoveFromInput(string input) {
//            if (input == "A" || input == "X")
//                return Move.Rock;
//            if (input == "B" || input == "Y")
//                return Move.Paper;
//            else 
//                return Move.Scissors;
//        }

//        static int getPointsForMove(Move move)
//        {
//            if (move == Move.Rock)
//                return 1;
//            if (move == Move.Paper)
//                return 2;
//            else
//                return 3;
//        }
//    }
//}