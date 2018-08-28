using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayfairCipher
{
    class Data
    {

        public string PublicText { get; set; }
        public char[] PublicTextCharArray { get; set; }
        public string Key { get; set; }
        public char[] KeyCharArray { get; set; }
        public char[,] PlayfairTab { get; set; }
        public int[] Coordinates { get; set; }
        public int[] TypeOfCoding { get; set; }
        public int[] EncriptedCoordinates { get; set; }
        public char[] EncriptedCharArray { get; set; }
        public string EncriptedString { get; set; }

        public int[] CoordinatesDecripted { get; set; }
        public int[] TypeOfCodingTabDecripted { get; set; }
        public char[] DecriptedCharTab { get; set; }
        public string DecriptedString { get; set; }
    }
}
