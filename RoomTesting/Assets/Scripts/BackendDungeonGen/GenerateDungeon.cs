using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BackendDungeonGen
{
    class GenerateDungeon
    {
        public int startingRoomIndex;

        public static void SetupBoard(ref Room[,] Board)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Board[i, j] = new Room(-1, "null");
                }
            }
        }

        public static (int SRY, int SRX) PlaceStartEnd(ref Room[,] Board)
        {
            char[] orientations = new char[] { 'N', 'E', 'S', 'W' };
            Random rnd = new Random();
            int SRX = rnd.Next(4, 7);
            int SRY = rnd.Next(4, 7);

            char BROrientation = orientations[rnd.Next(4)];


            Board[SRY, SRX] = new Room(1, "SR__");
            // places boss room next to starting room depending on orientation
            // orientation is the direction of the room's entrance
            if (BROrientation == 'N')
            {
                //Console.WriteLine("north");
                Board[SRY + 1, SRX] = new Room(1, "BR_" + BROrientation.ToString());
            }
            else if (BROrientation == 'E')
            {
                //Console.WriteLine("east");
                Board[SRY, SRX - 1] = new Room(1, "BR_" + BROrientation.ToString());
            }
            else if (BROrientation == 'S')
            {
                //Console.WriteLine("south");
                Board[SRY - 1, SRX] = new Room(1, "BR_" + BROrientation.ToString());
            }
            else // W
            {
                //Console.WriteLine("west");
                Board[SRY, SRX + 1] = new Room(1, "BR_" + BROrientation.ToString());
            }

            (int SRY, int SRX) returnTuple = (SRY, SRX);

            return returnTuple;
        }

        public static void PrintBoard(Room[,] Board)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(Board[i, j].tag.PadLeft(6));
                }
                Console.WriteLine();
            }
        }

        // checks if all door locations are valid
        // x and y is the potential location for currentRoom to be placed in Board
        // mustContain is an orientation that the room must contain to tie it back to the room it is generating off
        public static bool CheckDoorPlacement(ref Room[,] Board, int y, int x, Room currentRoom, char mustContain)
        {
            // 3d_esw
            int noofDoors = Convert.ToInt32(currentRoom.tag[0].ToString());
            //Console.WriteLine(noofDoors);

            // must contain check: checks that the room ties back to the room it is generating off
            if (!currentRoom.tag.ToLower().Contains(mustContain.ToString().ToLower()))
            {
                //Console.WriteLine("must contain check failed");
                return false;
            }

            // edge room checks: checks if any rooms on the edge of the board do not have a room facing off the board
            // 4 checks: y - 1, y + 1, x - 1, x + 1
            // coordinate +- 1 && currentRoom.tag.ToLower().Contains(orientation)
            if ((y - 1 < 0 && currentRoom.tag.ToLower().Contains('n')) || (y + 1 > 9 && currentRoom.tag.ToLower().Contains('s')) || (x - 1 < 0 && currentRoom.tag.ToLower().Contains('w')) || (x + 1 > 9 && currentRoom.tag.ToLower().Contains('e')))
            {
                //Console.WriteLine("edge room checks failed");
                return false;
            }

            // room's door checks: checks that all doors are facing valid locations
            int sidesVerified = 0; // the number of sides that have been verified. If == noof sides then return true
            // if there is a room facing <direction> and it is either:
            //  a null space or
            //  that room has a door facing the currentRoom
            // that side is verified
            if (currentRoom.tag.ToLower().Contains('n'))
            { // use of nested ifs prevents the occurence of index out of bounds errors after the edge room checks
                if (Board[y - 1, x].sigLevel == -1 || Board[y - 1, x].tag.ToLower().Contains('s') || Board[y - 1, x].tag == "SR__")
                {
                    //Console.WriteLine("north side check");
                    sidesVerified++;

                }
            }
            if (currentRoom.tag.ToLower().Contains('e'))
            {
                if (Board[y, x + 1].sigLevel == -1 || Board[y, x + 1].tag.ToLower().Contains('w') || Board[y, x + 1].tag == "SR__")
                {
                    //Console.WriteLine("east side check");
                    sidesVerified++;

                }
            }
            if (currentRoom.tag.ToLower().Contains('s'))
            {
                if (Board[y + 1, x].sigLevel == -1 || Board[y + 1, x].tag.ToLower().Contains('n') || Board[y + 1, x].tag == "SR__")
                {
                    //Console.WriteLine("south side check");
                    sidesVerified++;

                }
            }
            if (currentRoom.tag.ToLower().Contains('w'))
            {
                if (Board[y, x - 1].sigLevel == -1 || Board[y, x - 1].tag.ToLower().Contains('e') || Board[y, x - 1].tag == "SR__")
                {
                    //Console.WriteLine("west side check");
                    sidesVerified++;

                }
            }

            // room's walls checks
            // checks that all the walls of the given room are either facing another wall or a null space
            // get char array of wall orientations
            // check all orientations of wall adjascent rooms
            // get adjascent rooms char array of walls
            // check if orientation flipped is in that char array
            // if yes + 1
            char[] orientation = { 'n', 'e', 's', 'w' };
            char[] orientationFlipped = { 's', 'w', 'n', 'e' };
            int[] xOrientationValues = { 0, 1, 0, -1 };
            int[] yOrientationValues = { -1, 0, 1, 0 };
            char[] doorsOrientation = currentRoom.tag.Remove(0, 3).ToCharArray();
            char[] wallsOrientation = orientation.Except(doorsOrientation).ToArray(); // stores the orientation of all walls on a room
            int noofWalls = wallsOrientation.Length;
            int wallsVerified = 0;
            int orientationIndex = 0;

            for (int i = 0; i < wallsOrientation.Length; i++)
            {
                while (wallsOrientation[i] != orientation[orientationIndex])
                {
                    orientationIndex++;
                }

                if (x + xOrientationValues[orientationIndex] > 9 || x + xOrientationValues[orientationIndex] < 0 || y + yOrientationValues[orientationIndex] > 9 || y + yOrientationValues[orientationIndex] < 0) // checks if index is out of bounds of array
                {
                    wallsVerified++;
                    continue;
                }
                //Console.WriteLine(orientationIndex);
                Room adjascentRoom = Board[x + xOrientationValues[orientationIndex], y + yOrientationValues[orientationIndex]];
                char[] adjascentDoors = adjascentRoom.tag.Remove(0, 3).ToCharArray();
                char[] adjascentWalls = orientation.Except(adjascentDoors).ToArray();

                if (adjascentWalls.Contains(orientationFlipped[orientationIndex]))
                {
                    wallsVerified++;
                }
            }

            if (sidesVerified == noofDoors) //  && wallsVerified == noofWalls
            {
                return true;
            }

            return false;
        }

        // Generates a string of 'length' characters randomly seleted from 'array'
        // Example: SelectSubSet(2, {'n', 'e', 's', 'w'}) => "ns" or "es" (always in order of the original array)
        private static string SelectSubSet(int length, char[] array)
        {
            char[] arrayCounter = array; // holds version of array that is modified
            string returnString = ""; // holds string that is to be returned

            for (int lengthCounter = length; lengthCounter > 0; lengthCounter--) // iterates the number of times given on call
            {
                int subArrayMax = arrayCounter.Length - lengthCounter; // maximum possible index of item to be selected such that the output will be of length 'length' (if length is 3, the first value won't be the last in the array provided)
                char[] subArray = Range.charRange(arrayCounter, 0, subArrayMax + 1); // array of available characters to select for this iteration
                Random rnd = new Random();
                int selectIndex = rnd.Next(subArray.Length); // generates index to be randomly selected from subarray
                returnString = returnString + subArray[selectIndex]; // selects value from subarray and adds it to the returnString
                arrayCounter = Range.charRange(arrayCounter, selectIndex + 1, -1); // modifies arrayCounter so that the selected value and all values before it (so the order is kept) are removed from following iterations
            }

            return returnString;
        }

        public static void GenerateOffStartRoom(ref Room[,] Board, ref Stack stack, Room startRoom, int y, int x)
        {
            stack.push(startRoom);
            double sigLevel = startRoom.sigLevel;
            char[] orientation = { 'n', 'e', 's', 'w' };
            char[] orientationFlipped = { 's', 'w', 'n', 'e' };
            int[] xOrientationValues = { 0, 1, 0, -1 };
            int[] yOrientationValues = { -1, 0, 1, 0 };

            for (int i = 0; i < 4; i++)
            {
                if (Board[y + yOrientationValues[i], x + xOrientationValues[i]].tag.ToLower().Contains("br")) // skips current iteration if the targeted location has the boss room
                {
                    continue;
                }

                bool verifiedRoom = false;
                Room newRoom = new Room(-1, "null");
                Random rnd = new Random();

                while (!verifiedRoom)
                {
                    double num = rnd.NextDouble();
                    int noofRooms = 0;

                    if (num >= 0.5)
                    {
                        noofRooms = 3;
                    }
                    else
                    {
                        noofRooms = 2;
                    }

                    string tag = "";
                    tag += $"{Convert.ToString(noofRooms)}d_";
                    tag += SelectSubSet(noofRooms, orientation);

                    newRoom.sigLevel = sigLevel - 0.2;
                    newRoom.tag = tag;

                    verifiedRoom = CheckDoorPlacement(ref Board, y + yOrientationValues[i], x + xOrientationValues[i], newRoom, orientationFlipped[i]);
                    Console.WriteLine(verifiedRoom);

                }
                Board[y + yOrientationValues[i], x + xOrientationValues[i]] = newRoom;
                GenerateRoom(ref Board, ref stack, newRoom, y + yOrientationValues[i], x + xOrientationValues[i]);
            }
        }

        public static void GenerateRoom(ref Room[,] Board, ref Stack stack, Room baseRoom, int y, int x)
        {
            stack.push(baseRoom);
            double sigLevel = baseRoom.sigLevel; // determines size of rooms generated around it

            int noofIter = Convert.ToInt32(baseRoom.tag[0].ToString()); ; // Holds the number of iterations to be run in this generation

            char[] orientations = { 'n', 'e', 's', 'w' };
            char[] orientationFlipped = { 's', 'w', 'n', 'e' };
            int[] xOrientationValues = { 0, 1, 0, -1 };
            int[] yOrientationValues = { -1, 0, 1, 0 };

            char[] roomOrientations = Range.charRange(baseRoom.tag.ToCharArray(), baseRoom.tag.Length, -1);
            int orientationCounter = 0;

            double sigLevelVariance = 0.2;

            for (int i = noofIter; i > 0; i--)
            {
                while (!(orientations[orientationCounter] == baseRoom.tag[baseRoom.tag.Length - i]))
                {
                    orientationCounter++;
                }

                Room targetedRoom = Board[y + yOrientationValues[orientationCounter], x + xOrientationValues[orientationCounter]];

                if (noofIter == 1) // if it is a 1 door room, we do not need to generate a room
                {
                    break;
                }

                Random rnd = new Random();
                bool verifiedRoom = false;
                string newTag = "";
                bool gen1D = false; // if the sig level of the room is less than the sig level variance (in the next generation the sig level of the room will be <= 0) then this will be true so that the room generated can be a 1d room
                Room newRoom = new Room(-1, "null");



                while (!verifiedRoom)
                {
                    double num = rnd.NextDouble();
                    int noofRooms = 0;

                    if (sigLevel <= sigLevelVariance)
                    {
                        gen1D = true;
                    }

                    if (sigLevel > 0.5 && !gen1D)
                    {
                        if (num > sigLevel)
                        {
                            // 3d
                            noofRooms = 3;
                            newRoom.sigLevel = baseRoom.sigLevel - sigLevelVariance;

                            newTag = $"{noofRooms}d_";
                            newTag = newTag + SelectSubSet(noofRooms, orientations);

                            newRoom.tag = newTag;
                        }
                        else
                        {
                            // 2d
                            noofRooms = 2;
                            newRoom.sigLevel = baseRoom.sigLevel - sigLevelVariance;

                            newTag = $"{noofRooms}d_";
                            newTag = newTag + SelectSubSet(noofRooms, orientations);

                            newRoom.tag = newTag;
                        }
                    }

                    else
                    {
                        if (num > sigLevel && !gen1D)
                        {
                            // 2d
                            noofRooms = 2;
                            newRoom.sigLevel = baseRoom.sigLevel - sigLevelVariance;

                            newTag = $"{noofRooms}d_";
                            newTag = newTag + SelectSubSet(noofRooms, orientations);

                            newRoom.tag = newTag;
                        }
                        else
                        {
                            // 1d
                            noofRooms = 1;
                            newRoom.sigLevel = baseRoom.sigLevel - sigLevelVariance;

                            newTag = $"{noofRooms}d_";
                            newTag = newTag + SelectSubSet(noofRooms, orientations);

                            newRoom.tag = newTag;
                        }
                    }

                    verifiedRoom = CheckDoorPlacement(ref Board, y + yOrientationValues[orientationCounter], x + xOrientationValues[orientationCounter], newRoom, orientationFlipped[orientationCounter]);
                    //PrintBoard(Board);
                    //Console.WriteLine(verifiedRoom);
                    //PrintBoard(Board);
                    //Console.WriteLine(newRoom.tag);
                    //Console.WriteLine(i);
                    //Console.WriteLine(newRoom.tag);
                    //Console.WriteLine(Board[y + yOrientationValues[orientationCounter] + 1, x + xOrientationValues[orientationCounter]].tag);
                    //Console.ReadLine();
                }

                if (Board[y + yOrientationValues[orientationCounter], x + xOrientationValues[orientationCounter]].tag == "null")
                {
                    Board[y + yOrientationValues[orientationCounter], x + xOrientationValues[orientationCounter]] = newRoom;
                    //PrintBoard(Board);
                    //Console.ReadLine();
                    GenerateRoom(ref Board, ref stack, newRoom, y + yOrientationValues[orientationCounter], x + xOrientationValues[orientationCounter]);
                }
                //else
                //{
                //    newRoom.sigLevel = -1;
                //}

                //if (targetedRoom.tag != "null") //2d_ne
                //{
                //    newRoom.sigLevel = -1;
                //}

                //PrintBoard(Board);
                //Console.ReadLine();

                //if (targetedRoom.tag == "null")
                //{
                //    Console.WriteLine("Gen new room!");

                //}
                orientationCounter++;
            }
            stack.pop();
        }

        // Modifies the generated board for testing
        public static void TestBoard(ref Room[,] Board, int SRX, int SRY)
        {
            if (Board[SRY + 1, SRX].sigLevel == -1 && Board[SRY, SRX - 1].sigLevel == -1)
            {
                Board[SRY + 1, SRX] = new Room(0.8, "2d_NW");
                Board[SRY, SRX - 1] = new Room(0.8, "2d_ES");
                //Console.WriteLine(CheckDoorPlacement(ref Board, SRY + 1, SRX - 1, new Room(0.8, "3d_NES"), 'E'));
            }
            else
            {
                //Console.WriteLine("Run again!");
            }
            //Console.WriteLine($"Edge rooms: {CheckDoorPlacement(ref Board, 0, 5, new Room(1, "3d_ESW"))}");
        }

        public static Room[,] Run()
        {
            // TODO: Finish refactoring CheckDoorPlacement (first if statment done, just do 2-4 inclusive)
            // TODO: NoofDoors for CheckDoorPlacement not giving the right value
            // TODO: Bug where GenerateRoom gets stuck in a loop of false sometimes, note, check where the room is updated? maybe it only works if it is true the first time and if it is false, it does not try a new room

            Room[,] Board = new Room[10, 10]; // IMPORTANT: [Y-AXIS *INVERSE*, X-AXIS]
            SetupBoard(ref Board);
            //Console.WriteLine("Board setup");
            (int SRY, int SRX) startingRoomIndexes = PlaceStartEnd(ref Board);
            //Console.WriteLine("start end placed");
            int SRX = startingRoomIndexes.SRX;
            int SRY = startingRoomIndexes.SRY;
            //TestBoard(ref Board, SRX, SRY);
            // PrintBoard(Board);
            Stack stack = new Stack();
            GenerateOffStartRoom(ref Board, ref stack, Board[SRY, SRX], SRY, SRX);
            PrintBoard(Board);
            return Board;
        }
    }
}
