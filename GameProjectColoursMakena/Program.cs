using System;

namespace GameProjectColoursMakena
{
    class Program
    {
        static string Me = "X";
        static Int32 MeX = 30;
        static Int32 MeY = 15;
        static Int32 OldX;
        static Int32 OldY;
        static Int32 score = 0;

        static void Main(string[] args) 
        {
            //////////////////////////////////////////////////////////////////// Declaring variables and arrays for other functions that I need to keep the data for during the game loops
            Int32[] DotsX = new Int32[50];
            Int32[] DotsY = new Int32[50];
            Int32[] MyDotsX = new Int32[50];
            Int32[] MyDotsY = new Int32[50];
            Int32 OldPhase=0;
            Int32 arraycounter = 0;
            ConsoleKey key;
            int validdots=0;
            bool end = false;
            ////////////////////////////////////////////////////////////////////
            key = Console.ReadKey(true).Key;
            IntroScreen();
            Console.CursorVisible = false;

            do
            {
                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true).Key;
                    Move(key, MyDotsX, MyDotsY, DotsX, DotsY, ref arraycounter, ref OldPhase, ref end);
                }
                Draw(MyDotsX, MyDotsY,DotsX,DotsY, ref OldPhase, ref validdots, ref arraycounter, ref end);
            } while (end == false);
        }

        static void IntroScreen() //this is the starting screen
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Welcome to the A game!");
            Console.WriteLine("The goal of the A game is for you to match the placement of the As in the box across from you");
            Console.WriteLine("Be careful where you place your As, you can only place 4 more than the amount placed on the other side,");
            Console.WriteLine("and if you place one down, you can't pick it back up. If you run out you should restart the level.");
            Console.WriteLine("Controls:");
            Console.WriteLine("Arrow Keys: Move");
            Console.WriteLine("F: Place A");
            Console.WriteLine("Spacebar: Restart Level");
            Console.WriteLine("Tab: Skip to next level");
            Console.WriteLine("Hit any key to start");
            Console.ReadKey();
            Console.Clear();
        }

        static void Move(ConsoleKey K, int[] MyDotsX, int[] MyDotsY, int[] DotsX, int[] DotsY, ref int arraycounter, ref int OldPhase, ref bool end)
        {
            OldX = MeX;
            OldY = MeY;

            if (K == ConsoleKey.LeftArrow)
            {
                MeX--;
            }
            if (K == ConsoleKey.RightArrow)
            {
                MeX++;
            }
            if (K == ConsoleKey.UpArrow)
            {
                MeY--;
            }
            if (K == ConsoleKey.DownArrow)
            {
                MeY++;
            }
            if (K == ConsoleKey.F)
            {
                TrackMyDot(MyDotsX,MyDotsY,ref arraycounter, ref OldPhase, ref end);
            }
            if (K == ConsoleKey.Spacebar)
            {
                OldPhase = 0;
            }
            if (K== ConsoleKey.Tab)
            {
                score = score + 100;
            }
            limits();


        } //this is the keybindings 

        static void TrackMyDot(int[] MyDotsX, int[] MyDotsY, ref int arraycounter, ref int OldPhase, ref bool end) //this is where the array for tracking where your markers are gets its information
        {
            bool NewPhase;
            int dotsamount = 0;
            PhaseDotsParameter(ref dotsamount, ref end);
            dotsamount = dotsamount + 3;
            NewPhase = NewPhaseTest(ref OldPhase, ref end);
            
            if (arraycounter <= dotsamount)
            {
                MyDotsX[arraycounter] = MeX;
                MyDotsY[arraycounter] = MeY;
                arraycounter++;
            }
            
        }
        static void Draw(int[] MyDotsX, int[] MyDotsY,int[] DotsX,int[] DotsY, ref int OldPhase, ref int validdots, ref int arraycounter, ref bool end)
        { 
            DrawMe();
            DrawRoom();
            DrawImageBorder();
            DrawDots(MyDotsX, MyDotsY,DotsX,DotsY,ref validdots, ref OldPhase, ref arraycounter, ref end);
            DrawBanner(ref OldPhase, ref validdots, ref end);

        } //this is the main draw function that then calls the others
        static void DrawBanner(ref int OldPhase, ref int validdots, ref bool end)
        {
            String Phase;
            int CurrentPhase,amountofdots=0;
            bool isNewPhase = false;
            isNewPhase = NewPhaseTest(ref OldPhase, ref end);
            PhaseDotsParameter(ref amountofdots, ref end);
            Console.CursorVisible = false;
            Phase = PhaseChecker(ref end);
            CurrentPhase = WhichPhase(ref end);
            if (isNewPhase == true)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("                                   ");
            }
           
            Console.SetCursorPosition(0, 0);
            Console.Write("Level: " + Phase + "        Number of Correct Placements: " + validdots +"/"+amountofdots+"         Score : " +score);
            
        } //this draws the banner above the game

        static void DrawMe()
        {
            Console.SetCursorPosition(OldX, OldY);
            Console.Write(" ");
            Console.SetCursorPosition(MeX, MeY);
            Console.Write(Me);
        } //this draws the player's X

        static void DrawDots(int[] MyDotsX, int[] MyDotsY, int[] DotsX, int[] DotsY, ref int validdots, ref int OldPhase, ref int arraycounter, ref bool end) //this function goes through the array and draws the dots, then calls the game
        {
            int i = 0;
            int amtofdots=0;
            PhaseDotsParameter(ref amtofdots, ref end);
            amtofdots = amtofdots + 3;
            while (i <= amtofdots) 
            {
                if (MyDotsX[i] != 0 && MyDotsY[i] != 0) //testing for only placed dots
                {
                    Console.SetCursorPosition(MyDotsX[i], MyDotsY[i]);
                    Console.Write("A");
                }
                i++;
            }
            TheGame(MyDotsX, MyDotsY, DotsX,DotsY, ref validdots, ref OldPhase, ref arraycounter, ref end);
            

        }

        static int WhichPhase(ref bool end) //this function checks to see what level it is based on the score
        {
            int Phase=0;
            if (score == 0)
            {
                Phase = 1;
            }
            if (score == 100)
            {
                Phase = 2;
            }
            if (score == 200)
            {
                Phase = 3;
            }
            if (score == 300)
            {
                Phase = 4;
            }
            if (score == 400)
            {
                Phase = 5;
            }
            if (score == 500)
            {
                EndScreen(ref end);
                
            }
            return Phase;
        }

        static void TheGame(int[] MyDotsX, int[] MyDotsY, int[] DotsX, int[] DotsY, ref int validdots, ref int OldPhase, ref int arraycounter, ref bool end) //This function fills the arrays with random numbers
        {
            Random r = new Random();
            bool NewPhase;
            int left, right, top, bottom, l, h;
            int CurrentPhase=0;
            RoomLimits(out top, out right, out bottom, out left, out l, out h);
            int amountofdots=0;
            PhaseDotsParameter(ref amountofdots, ref end);
            int dotx, doty;
            right = right + 46;
            left = left + 46;
            NewPhase = NewPhaseTest(ref OldPhase, ref end);
            if (NewPhase == true)
            {
                for (int i=0; i<40;i++)
                {
                    DotsX[i] = 0;
                    DotsY[i] = 0;
                    MyDotsX[i] = 0;
                    MyDotsY[i] = 0;
                    arraycounter = 0;           
                }
                Console.Clear();
            }
            if (DotsX[0] == 0 || NewPhase == true)
            {
                for (int i = 0; i != amountofdots; i++)
                {
                    DotsX[i] = r.Next(left, right-1);
                }
                for (int j = 0; j != amountofdots; j++)
                {
                    DotsY[j] = r.Next(top+1, bottom-1);
                }
            }
            for (int a=0;a!=amountofdots;a++)
            {
                Console.SetCursorPosition(DotsX[a], DotsY[a]);
                dotx = DotsX[a];
                doty = DotsY[a];
                Console.Write("A");
            }
            CurrentPhase = WhichPhase(ref end);
            OldPhase = CurrentPhase;
            ColourSwitch(CurrentPhase);
            validdots = DotChecker(MyDotsX, MyDotsY, DotsX, DotsY, ref end);
            if (validdots == amountofdots)
            {
                score = score + 100;
            }
        }

        static void ColourSwitch(int Phase)
        {
            switch (Phase)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
        }

        static void PhaseDotsParameter(ref int AmountDots, ref bool end)
        {
            int Phase;
            Phase = WhichPhase(ref end);
            switch (Phase)
            {
                case 1:
                    AmountDots = 5;
                    break;
                case 2:
                    AmountDots = 10;
                    break;
                case 3:
                    AmountDots = 15;
                    break;
                case 4:
                    AmountDots = 20;
                    break;
                case 5:
                    AmountDots = 25;
                    break;
            }




        } //this function says how many dots there are for each level

        static int DotChecker(int[] MyDotsX, int[] MyDotsY, int[] DotsX, int[] DotsY, ref bool end) //trying in here to see if they match up
        {
            int DotX, DotY, MyDotX, MyDotY, amountofdots =0;
            int j=0;
            int validdots = 0;
            bool[] Equivalent = new bool[50];
            PhaseDotsParameter(ref amountofdots, ref end);
            amountofdots = amountofdots + 3;
            do
            {
                DotX = DotsX[j] - 45;
                DotY = DotsY[j];
                for (int i = 0; i <= amountofdots; i++)
                {
                    MyDotX = MyDotsX[i];
                    MyDotY = MyDotsY[i];
                    if (MyDotX == 0 && MyDotY == 0) //break it out of the loop if the array is empty
                    {
                        i = i + 100;
                    }
                    if (DotX == MyDotX && DotY == MyDotY)
                    {
                        Equivalent[j] = true;
                    }
                                      
                }
                j++;
            } while (j < amountofdots);
            int t=0;
            do
            {
                if (Equivalent[t] == true)
                {
                    validdots++;
                }
                t++;
            } while (t<amountofdots);
            
            return validdots;           
        }

        static void DrawRoom()
        {
            Int32 RoomX = 9, RoomY = 1;
            string BorderChar = "O";
            Int32 Top, Bottom, Left, Right, Length, Height;
            RoomLimits(out Top, out Right, out Bottom, out Left, out Length, out Height);


            for (Int32 i = 0; i < Length; i++) //printing the top and bottom of the room
            {
                RoomX++;
                RoomY = Top;
                Console.SetCursorPosition(RoomX, RoomY);
                Console.Write(BorderChar);
                RoomY = Bottom;
                Console.SetCursorPosition(RoomX, RoomY);
                Console.Write(BorderChar);

            }
            RoomY = Bottom;
            RoomX = 10;
            for (Int32 j = 0; j < Height; j++)
            {
                RoomY--;
                RoomX = Left;
                Console.SetCursorPosition(RoomX, RoomY);
                Console.Write(BorderChar);
                RoomX = Right;
                Console.SetCursorPosition(RoomX, RoomY);
                Console.SetCursorPosition(RoomX, RoomY);
                Console.Write(BorderChar);

            }
        } //this draws the borders of player's room

        static void DrawImageBorder()
        {
            Int32 RoomX = 9, RoomY = 1;
            string BorderChar = "O";
            Int32 Top, Bottom, Left, Right, Length, Height;
            RoomLimits(out Top, out Right, out Bottom, out Left, out Length, out Height);
            Right = Right + 45;
            Left = Left + 45;
            RoomX = RoomX + 45;

            for (Int32 i = 0; i < Length; i++) //printing the top and bottom of the room
            {
                RoomX++;
                RoomY = Top;
                Console.SetCursorPosition(RoomX, RoomY);
                Console.Write(BorderChar);
                RoomY = Bottom;
                Console.SetCursorPosition(RoomX, RoomY);
                Console.Write(BorderChar);

            }
            RoomY = Bottom;
            RoomX = 10;
            for (Int32 j = 0; j < Height; j++)
            {
                RoomY--;
                RoomX = Left;
                Console.SetCursorPosition(RoomX, RoomY);
                Console.Write(BorderChar);
                RoomX = Right;
                Console.SetCursorPosition(RoomX, RoomY);
                Console.SetCursorPosition(RoomX, RoomY);
                Console.Write(BorderChar);

            }
        } //this draws the border of the dot's room
        static String PhaseChecker(ref bool end)
        {
            int P=0;
            string CurrentPhase;
            String[] PhasePoints = new String[10];
            P = WhichPhase(ref end);
            PhasePoints[1] = "One";
            PhasePoints[2] = "Two";
            PhasePoints[3] = "Three";
            PhasePoints[4] = "Four";
            PhasePoints[5] = "Five";
            CurrentPhase = PhasePoints[P];

            return CurrentPhase;

        } //this translates the phases into letters for the banner
        static void limits()
        {
            int MaxY, MaxX, MinY, MinX, A, B;
            RoomLimits(out MaxY, out MaxX, out MinY, out MinX, out A, out B);

            if (MeX >= MaxX)
            {
                MeX = MaxX - 1;
            }
            if (MeY <= MaxY)
            {
                MeY = MaxY + 1;
            }
            if (MeX <= MinX)
            {
                MeX = MinX + 1;
            }
            if (MeY >= MinY)
            {
                MeY = MinY - 1;
            }
        }//this prevents the x from going through the wall

        static bool NewPhaseTest(ref int OldPhase, ref bool end)
        {
            bool NewPhase=false;
            int CurrentPhase = 0;
            CurrentPhase = WhichPhase(ref end);
            if (CurrentPhase == OldPhase)
            {
                NewPhase = false;
            }
            else
            {
                NewPhase = true;
            }
            return NewPhase;
        }//this tests if the game needs to change things for the next level

        static void RoomLimits(out int RoomTop, out int RoomRight, out int RoomBottom, out int RoomLeft, out int RoomLength, out int RoomHeight)
        {
            RoomTop = 5;
            RoomBottom = 25;
            RoomLeft = 10;
            RoomRight = 52;
            RoomLength = 43;
            RoomHeight = 20;
        }//this has the information for what the dimensions of the room are
        
        static void EndScreen(ref bool end)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*****************************************");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Congradulations! You have won the A Game.");
            Console.WriteLine("Press the enter key 5 times to close the program");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*****************************************");
            Console.SetCursorPosition(0, 3);
            end = true;
            Console.ReadKey();
        }

    }
}
