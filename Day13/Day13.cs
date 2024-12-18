using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day13
    {
        //vector math
        //X*V1 + Y*V2 = Prize Coordinates. Find X and Y for each machine, given V1, V2, and Prize Coordinates
        //V1 = (94, 34)
        //V2 = (22, 67)
        //Prize Coordinates = (8400, 5400)

        //80(94, 34) + 40(22, 67) = (8400, 5400)
        //3(80) + 40 = 280 tokens for this prize

        //then, 3X + Y = Total Tokens
        //how many total tokens do you spend to win the most prizes?

        public static void DoWork(string[] rawInput)
        {
            Console.ForegroundColor = ConsoleColor.White;
            int totalTokens = 0;
            foreach (var mc in ParseRawInput(CreateSingleStringFromRawInput(rawInput)))
            {
                Console.WriteLine(mc.ToString());
                int i = mc.TokensSpentOnMachine();
                if (i >= 0)
                {
                    totalTokens += i;
                    Console.WriteLine("Tokens: " + i);
                }
                else
                    Console.WriteLine("Unable to achieve prize");

                Console.WriteLine();

            }
            if (30922 < totalTokens && totalTokens < 50829)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {

                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine("Total Tokens: " + totalTokens);
            Console.ForegroundColor = ConsoleColor.White;
        }

        //parser
        static List<ClawMachine> ParseRawInput(string singleStringInput)
        {
            List<ClawMachine> result = new List<ClawMachine>();
            Regex regex = new Regex("Button A: X\\+[0-9]{1,15}, Y\\+[0-9]{1,15}\\nButton B: X\\+[0-9]{1,15}, Y\\+[0-9]{1,15}\\nPrize: X=[0-9]{1,15}, Y\\=[0-9]{1,15}");
            Regex numberMatcherX = new Regex("X\\+[0-9]{1,15}");
            Regex numberMatcherY = new Regex("Y\\+[0-9]{1,15}");

            foreach (Match match in regex.Matches(singleStringInput))
            {

                string matchString = match.Value;

                var matchSplit = matchString.Split('\n');

                Vector2 aButton = Vector2.Zero;
                Vector2 bButton = Vector2.Zero;
                Vector2 prizeLocation = Vector2.Zero;

                foreach (string line in matchSplit)
                {
                    if (Regex.IsMatch(line, "A"))
                    {
                        string xVal = Regex.Replace(numberMatcherX.Match(line).Value, "[^0-9]", "");
                        string yVal = Regex.Replace(numberMatcherY.Match(line).Value, "[^0-9]", "");

                        aButton = new Vector2(int.Parse(xVal), int.Parse(yVal));


                    }
                    else if (Regex.IsMatch(line, "B"))
                    {
                        string xVal = Regex.Replace(numberMatcherX.Match(line).Value, "[^0-9]", "");
                        string yVal = Regex.Replace(numberMatcherY.Match(line).Value, "[^0-9]", "");

                        bButton = new Vector2(int.Parse(xVal), int.Parse(yVal));
                    }
                    else if (Regex.IsMatch(line, "Prize"))
                    {
                        string xVal = Regex.Replace(Regex.Match(line, "X=[0-9]{1,15}").Value, "[^0-9]", "");
                        string yVal = Regex.Replace(Regex.Match(line, "Y=[0-9]{1,15}").Value, "[^0-9]", "");

                        prizeLocation = new Vector2(int.Parse(xVal), int.Parse(yVal));

                    }
                }
                result.Add(new ClawMachine(aButton, bButton, prizeLocation));
            }

            return result;
        }

        static string CreateSingleStringFromRawInput(string[] rawInput)
        {
            string result = "";
            foreach (string input in rawInput)
            {
                result += input + "\n";
            }
            return result;
        }

    }


    public class ClawMachine
    {
        Vector2 AButton, BButton, PrizePosition;

        public ClawMachine(Vector2 aButton, Vector2 bButton, Vector2 prizePosition)
        {
            AButton = aButton;
            BButton = bButton;
            PrizePosition = prizePosition;
        }
        
        public int TokensSpentOnMachine(int startingAPresses = 0, int startingBPresses = 0)
        {
            if (startingBPresses > 100) return -1;
            Vector2 addingVector = Vector2.Zero;
            Vector2 subtractVector = Vector2.Zero;
            int buttonApressed = startingAPresses;
            int buttonBpressed = startingBPresses;
            bool done = false;

            while (!done)
            {

                addingVector -= subtractVector;
                subtractVector = Vector2.Zero;
                buttonApressed = 0;
                do
                {
                    addingVector += BButton;
                    // tokensSpent++;
                    buttonBpressed++;
                    if (addingVector.X > PrizePosition.X || addingVector.Y > PrizePosition.Y || buttonBpressed>100)
                    {
                        done = true;
                        break;
                    }
                }
                while ((PrizePosition.X - addingVector.X) % AButton.X != 0 && (PrizePosition.Y - addingVector.Y) % AButton.Y != 0);
                if (done) break;

                done = true;
                do
                {
                    if (addingVector.X > PrizePosition.X || addingVector.Y > PrizePosition.Y || buttonBpressed>100)
                    {
                        done = false;
                        break;
                    }
                    addingVector += AButton;
                    //tokensSpent += 3;
                    buttonApressed++;
                    subtractVector += AButton;
                }
                while (addingVector.X != PrizePosition.X && addingVector.Y != PrizePosition.Y);

                if (buttonApressed > 100 || buttonBpressed > 100)
                    done = false;

            }
            /*if(this.TokensSpentOnMachine(0, buttonBpressed + 1) < (buttonApressed * 3) + buttonBpressed)
            {
                return this.TokensSpentOnMachine(0, buttonBpressed + 1);
            }
            else */if (addingVector.X == PrizePosition.X || addingVector.Y == PrizePosition.Y)
            {
                Console.WriteLine("B: " + buttonBpressed);
                Console.WriteLine("A: " + buttonApressed);

                return (buttonApressed * 3) + buttonBpressed;
            }
            else
            {
                Console.WriteLine("B: " + buttonBpressed);
                Console.WriteLine("A: " + buttonApressed);
                return -1;
            }

            //addingVector + X(V2) = targetVector
            // X = (targetVector - addingVector)/V2
            //tokensSpent += X;

            //return tokensSpent;
        }

        public override string ToString()
        {
            return "A Button: " + AButton + "\nB Button: " + BButton + "\nPrizePos: " + PrizePosition + "\n-";
        }
    }



}
//30922 < My Answer < 50829