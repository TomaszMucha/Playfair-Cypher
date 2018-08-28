using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Text;
using System.Threading.Tasks;

namespace PlayfairCipher
{
    class Program
    {
        //public static int origCol;
        //public static int origRow;
        //private static void WriteAt(string s, int x, int y)
        //{
        //    try
        //    {
        //        Console.SetCursorPosition(origCol + x, origRow + y);
        //        Console.Write(s);
        //    }
        //    catch (ArgumentOutOfRangeException e)
        //    {
        //        Console.Clear();
        //        Console.WriteLine(e.Message);
        //    }
        //}

        static void Main(string[] args)
        {
            Init();            
        }

        internal static void Init()
        {
            int control = -1;
            do
            {
                Console.WriteLine("Wybierz jedną z opcji: \n 1) Szyfrowanie \n 2) Deszyfrowanie \n 0) Wyjscie");
                control = Int32.Parse(Console.ReadLine());
                if (control==1)
                {
                    Data data = Encription();
                    Console.WriteLine(data.EncriptedString);
                }
                else
                {
                    if (control==2)
                    {
                        Data data = Decription();
                        Console.WriteLine(data.DecriptedString + "\n");
                    }
                }
            } while (control!=0);
        }

        #region EncriptionFunction

        internal static Data Encription()
        {
            Data data = new Data();
            data = SetPublicText(data);
            data = PublicTextToCharArray(data);
            data = SetKey(data);
            data = KeyToCharArray(data);
            data = CreatePlayfairTab(data);
            data = AddictionalLetter(data);
            data = Coordinates(data);
            data = TypeOfCoding(data);
            data = PlayfairCypherEncription(data);
            data = EncriptedCoordinatesToCharTab(data);
            data = EncriptedCharArrayToString(data);
            return data;
        }
        private static Data SetPublicText(Data data)
        {
            Console.Write("Podaj tekst jawny: ");
            data.PublicText = Console.ReadLine().ToUpper().Replace(" ", "");
            return data;
        }
        private static Data PublicTextToCharArray(Data data)
        {
            data.PublicTextCharArray = new char[data.PublicText.Length];
            data.PublicTextCharArray = data.PublicText.ToArray();
            return data;
        }
        private static Data SetKey(Data data)
        {
            Console.Write("Podaj klucz szyfrujący: ");
            data.Key = Console.ReadLine().ToUpper().Replace(" ", "");
            return data;
        }
        private static Data KeyToCharArray(Data data)
        {
            data.KeyCharArray = new char[data.Key.Length];
            data.KeyCharArray = data.Key.ToCharArray();
            return data;
        }
        //private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e, Data data, DateTime t)
        //{
        //    TimeSpan dtts = new TimeSpan(t.Day, t.Hour, t.Minute,t.Second);
        //    int dttsTempSec = (int)dtts.TotalSeconds;
        //    TimeSpan ts = new TimeSpan(e.SignalTime.Day, e.SignalTime.Hour, e.SignalTime.Minute, e.SignalTime.Second);
        //    int tempSec = (int)ts.TotalSeconds;
        //    t =t.Add(ts);

        //    Random rnd = new Random();
        //    for (int i = tempSec - dttsTempSec; i < data.EncriptedCharArray.Length; i++)
        //    {
        //        WriteAt(((char)rnd.Next(65,90)).ToString(), i, 4);
        //    }
        //    if (tempSec - dttsTempSec< data.EncriptedCharArray.Length)
        //    {
        //        WriteAt(data.EncriptedCharArray[tempSec - dttsTempSec].ToString(), tempSec - dttsTempSec, 4);
        //    }

        //}
        private static Data CreatePlayfairTab(Data data)
        {
            char[] AlphabetTab = new char[26];
            AlphabetTab[0] = 'A';
            for (int i = 1; i < AlphabetTab.Length; i++)
            {
                AlphabetTab[i] = (char)((int)AlphabetTab[0] + i);
            }
            char[] PlayfairTab1D = new char[data.KeyCharArray.Length + AlphabetTab.Length];
            PlayfairTab1D = data.KeyCharArray.Concat(AlphabetTab).ToArray().ToList().Distinct().ToArray().Where(x => x != 'J').ToArray();
            data.PlayfairTab = new char[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    data.PlayfairTab[i, j] = PlayfairTab1D[5 * i + j];
                }
            }
            return data;
        }
        private static Data AddictionalLetter(Data data)
        {
            int index = -2;
            do
            {
                index = ReturnDoubleLetterIndex(data.PublicTextCharArray);
                if (index != -1)
                {
                    List<char> tempCharList = data.PublicTextCharArray.ToList();
                    tempCharList.Insert(index, 'X');
                    data.PublicTextCharArray = tempCharList.ToArray();
                }
            } while (index != -1);
            if (data.PublicTextCharArray.Length % 2 == 1)
            {
                List<char> tempCharList = data.PublicTextCharArray.ToList();
                tempCharList.Insert(data.PublicTextCharArray.Length, 'X');
                data.PublicTextCharArray = tempCharList.ToArray();
            }
            return data;
        }
        private static int ReturnDoubleLetterIndex(char[] publicTextCharTab)
        {
            for (int i = 0; i < publicTextCharTab.Length; i += 2)
            {
                if (i + 1 < publicTextCharTab.Length)
                {
                    if (publicTextCharTab[i] == publicTextCharTab[i + 1])
                    {
                        return i + 1;
                    }
                }
            }
            return -1;
        }
        private static Data Coordinates(Data data)
        {
            data.Coordinates = new int[(data.PublicTextCharArray.Length) * 2];
            for (int k = 0; k < data.PublicTextCharArray.Length; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (data.PublicTextCharArray[k] == data.PlayfairTab[i, j])
                        {
                            data.Coordinates[2 * k] = i;
                            data.Coordinates[2 * k + 1] = j;
                        }
                    }
                }
            }
            return data;
        }
        private static Data TypeOfCoding(Data data)
        {
            data.TypeOfCoding = new int[data.Coordinates.Length / 4];
            for (int i = 0; i < data.Coordinates.Length / 4; i++)
            {
                if (4 * i + 3 < data.Coordinates.Length)
                {
                    if (data.Coordinates[4 * i] == data.Coordinates[4 * i + 2])
                    {
                        data.TypeOfCoding[i] = 2;
                    }
                    else
                    {
                        if (data.Coordinates[4 * i + 1] == data.Coordinates[4 * i + 3])
                        {
                            data.TypeOfCoding[i] = 1;
                        }
                        else
                        {
                            data.TypeOfCoding[i] = 3;
                        }
                    }
                }
            }
            return data;
        }
        private static Data PlayfairCypherEncription(Data data)
        {
            for (int i = 0; i < data.TypeOfCoding.Length; i++)
            {
                if (data.TypeOfCoding[i] == 1)
                {
                    data.Coordinates[4 * i] = (data.Coordinates[4 * i] + 1) % 5;
                    data.Coordinates[4 * i + 2] = (data.Coordinates[4 * i + 2] + 1) % 5;
                }
                else
                {
                    if (data.TypeOfCoding[i] == 2)
                    {
                        data.Coordinates[4 * i + 1] = (data.Coordinates[4 * i + 1] + 1) % 5;
                        data.Coordinates[4 * i + 3] = (data.Coordinates[4 * i + 3] + 1) % 5;
                    }
                    else
                    {
                        int temp = 0;
                        temp = data.Coordinates[4 * i];
                        data.Coordinates[4 * i] = data.Coordinates[4 * i + 2];
                        data.Coordinates[4 * i + 2] = temp;
                    }
                }
            }
            data.EncriptedCoordinates = data.Coordinates;
            return data;
        }
        private static Data EncriptedCoordinatesToCharTab(Data data)
        {
            data.EncriptedCharArray = new char[data.EncriptedCoordinates.Length / 2];
            for (int i = 0; i < data.EncriptedCoordinates.Length/2; i++)
            {
                data.EncriptedCharArray[i] = data.PlayfairTab[data.EncriptedCoordinates[2*i], data.EncriptedCoordinates[2*i + 1]];
            }
            return data;
        }

        private static Data EncriptedCharArrayToString(Data data)
        {
            data.EncriptedString = (new String(data.EncriptedCharArray)).Replace("X","");
            return data;
        }

        #endregion

        #region DecriptionFunction

        public static Data SetEncriptedString()
        {
            Data data = new Data();
            int decriptControl = -1;
            Console.WriteLine("Podaj zaszyfrowany kod:");
            while (decriptControl != 1)
            {
                data.EncriptedString = Console.ReadLine();
                decriptControl = 1;
                if (data.EncriptedString.Length%2!=0)
                {
                    Console.WriteLine("Podany tekst jest nieprawidłowy. Sprobój ponownie lub wpisz 0 aby wyjść.");
                    decriptControl = 0;
                }
            } 
            return data;
        }

        public static Data EncriptedStringToCharArray(Data data)
        {
            data.EncriptedCharArray = data.EncriptedString.ToCharArray();
            return data;
        }

        public static Data CoordinatesDecription(Data data)
        {
            data.CoordinatesDecripted = new int[(data.EncriptedCharArray.Length) * 2];
            for (int k = 0; k < data.EncriptedCharArray.Length; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (data.EncriptedCharArray[k] == data.PlayfairTab[i, j])
                        {
                            data.CoordinatesDecripted[2 * k] = i;
                            data.CoordinatesDecripted[2 * k + 1] = j;
                        }
                    }
                }
            }
            return data;
        }

        public static Data TypeOfCodingDecripted(Data data)
        {
            data.TypeOfCodingTabDecripted = new int[data.CoordinatesDecripted.Length / 4];
            for (int i = 0; i < data.CoordinatesDecripted.Length / 4; i++)
            {
                if (4 * i + 3 < data.CoordinatesDecripted.Length)
                {
                    if (data.CoordinatesDecripted[4 * i] == data.CoordinatesDecripted[4 * i + 2])
                    {
                        data.TypeOfCodingTabDecripted[i] = 2;
                    }
                    else
                    {
                        if (data.CoordinatesDecripted[4 * i + 1] == data.CoordinatesDecripted[4 * i + 3])
                        {
                            data.TypeOfCodingTabDecripted[i] = 1;
                        }
                        else
                        {
                            data.TypeOfCodingTabDecripted[i] = 3;
                        }
                    }
                }
            }
            return data;
        }

        public static Data PlayfairCypherDecription(Data data)
        {
            for (int i = 0; i < data.TypeOfCodingTabDecripted.Length; i++)
            {
                if (data.TypeOfCodingTabDecripted[i] == 1)
                {
                    if ((data.CoordinatesDecripted[4 * i] - 1) % 5 < 0)
                    {
                        data.CoordinatesDecripted[4 * i] = ((data.CoordinatesDecripted[4 * i] - 1) % 5)+5;
                    }
                    else
                    {
                        data.CoordinatesDecripted[4 * i] = ((data.CoordinatesDecripted[4 * i] - 1) % 5);
                    }
                    if ((data.CoordinatesDecripted[4 * i + 2] - 1) % 5 < 0)
                    {
                        data.CoordinatesDecripted[4 * i + 2] = ((data.CoordinatesDecripted[4 * i + 2] - 1) % 5)+5;
                    }
                    else
	                {
                        data.CoordinatesDecripted[4 * i + 2] = (data.CoordinatesDecripted[4 * i + 2] - 1) % 5;
	                }

                }
                else
                {
                    if (data.TypeOfCodingTabDecripted[i] == 2)
                    {
                        if ((data.CoordinatesDecripted[4 * i + 1] - 1) % 5 < 0)
                        {
                            data.CoordinatesDecripted[4 * i + 1] = ((data.CoordinatesDecripted[4 * i + 1 ] - 1) % 5) + 5;
                        }
                        else
                        {
                            data.CoordinatesDecripted[4 * i + 1] = ((data.CoordinatesDecripted[4 * i + 1] - 1) % 5);
                        }
                        if ((data.CoordinatesDecripted[4 * i + 3] - 1) % 5 < 0)
                        {
                            data.CoordinatesDecripted[4 * i + 3] = ((data.CoordinatesDecripted[4 * i + 3] - 1) % 5) + 5;
                        }
                        else
                        {
                            data.CoordinatesDecripted[4 * i + 3] = (data.CoordinatesDecripted[4 * i + 3] - 1) % 5;
                        }
                    }
                    else
                    {
                        int temp = 0;
                        temp = data.CoordinatesDecripted[4 * i];
                        data.CoordinatesDecripted[4 * i] = data.CoordinatesDecripted[4 * i + 2];
                        data.CoordinatesDecripted[4 * i + 2] = temp;
                    }
                }
            }
            return data;
        }

        public static Data DecriptedCoordinatesToCharTab(Data data)
        {
            data.DecriptedCharTab = new char[data.CoordinatesDecripted.Length / 2];
            for (int i = 0; i < data.CoordinatesDecripted.Length / 2; i++)
            {
                data.DecriptedCharTab[i] = data.PlayfairTab[data.CoordinatesDecripted[2 * i], data.CoordinatesDecripted[2 * i + 1]];
            }
            return data;
        }

        private static Data DecriptedCharArrayToString(Data data)
        {
            data.DecriptedString = new string(data.DecriptedCharTab).Replace("X","");
            return data;
        }

        public static Data Decription()
        {
            Data data = SetEncriptedString();
            data = EncriptedStringToCharArray(data);
            data = SetKey(data);
            data = KeyToCharArray(data);
            data = CreatePlayfairTab(data);
            data = CoordinatesDecription(data);
            data = TypeOfCodingDecripted(data);
            data = PlayfairCypherDecription(data);
            data = DecriptedCoordinatesToCharTab(data);
            data = DecriptedCharArrayToString(data);
            return data;
        }
        #endregion
    }
}
