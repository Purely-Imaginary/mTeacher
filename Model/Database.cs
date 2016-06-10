using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mTeacher
{
    public class Database
    {
        public List <Unit> units;
        public int UnitCounter;

        public Database()
        {
            List<Unit> newUnits = new List<Unit>();
            units = newUnits;
            UnitCounter = 0;
        }

        public static Database newDatabase(List<Unit> units)
        {
            Word.nextId = 0;
            Database nowa = new Database();
            nowa.units = units;
            return nowa;
        }
    }
}
