using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mTeacher
{
    public class Unit
    {
        public int id;
        public string name;
        public string description;
        public DateTime addTime;
        public List <Word> words;
        public int wordCount;

        public static Unit newUnit(string name, string description, DateTime addTime, List <Word> words)
        {
            Unit nowy = new Unit();
            nowy.id = DBHandler.MainDB.UnitCounter;
            DBHandler.MainDB.UnitCounter++;
            nowy.name = name;
            nowy.description = description;
            nowy.addTime = addTime;
            List<Word> copy = new List<Word>(words);
            nowy.words = copy;
            nowy.wordCount = words.Count;
            return nowy;
        }
    }
}
