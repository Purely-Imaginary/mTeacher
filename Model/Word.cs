using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mTeacher
{
    public class Word
    {
        public int id;
        public string polish;
        public string english;
        public static int nextId;

        public static Word newWord(string polish, string english)
        {
            Word nowy = new Word();
            nowy.id = Word.nextId;
            nowy.polish = polish;
            nowy.english = english;
            Word.nextId++;
            return nowy;
        }
    }
    
}
