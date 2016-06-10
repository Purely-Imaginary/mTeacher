using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace mTeacher
{
    public static class DBHandler
    {
        public static Database MainDB = new Database();
        public static Database SelectedDB = new Database();


        public static Database ReadFromXML(string path)
        {
            Database db = new Database();
           
            XDocument file;
            file = XDocument.Load(path);
            var resultsxx = file.Descendants("Unit");
            var results1 = file.Descendants("Unit").Elements("Word");
            var results2 = file.Descendants("Unit").Descendants("Word");

            var results = from query1 in file.Descendants("Unit")
                          select new Unit
                          {
                              id = Int32.Parse(query1.Element("ID").Value),
                              name = query1.Element("Name").Value,
                              description = query1.Element("Description").Value,
                              addTime = DateTime.Parse(query1.Element("TimeStamp").Value),
                              wordCount = Int32.Parse(query1.Element("WordCount").Value),
                              words = (from query2 in query1.Elements("Word")
                                      select new Word
                                      {
                                          id = Int32.Parse(query2.Element("ID").Value),
                                          polish = query2.Element("polish").Value,
                                          english = query2.Element("english").Value,
                                      }).ToList()

                          };
            
            Unit[] wynik = results.ToArray();
            db.UnitCounter = wynik.Length;
            db = DBHandler.Add(db, wynik);
            DBHandler.MainDB = db;

            return db;
        }
        public static void WriteToXML(Database db, string path)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create("database.db", xmlWriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Database");

                foreach(Unit unit in db.units)
                {
                    if (unit == null) break;
                    writer.WriteStartElement("Unit");
                    writer.WriteElementString("ID", unit.id.ToString());
                    writer.WriteElementString("Name", unit.name);
                    writer.WriteElementString("Description", unit.description);
                    writer.WriteElementString("TimeStamp", unit.addTime.ToString());
                    writer.WriteElementString("WordCount", unit.wordCount.ToString());
                    foreach (var word in unit.words)
                    {
                        writer.WriteStartElement("Word");
                        writer.WriteElementString("ID", word.id.ToString());
                        writer.WriteElementString("polish", word.polish);
                        writer.WriteElementString("english", word.english);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }


        }
        public static Database Add(Database db, Unit[] units)
        {
            foreach (var unit in units)
            {
                db.units.Add(unit);
            }
            return db;
        }
        public static Database Add(Database db, Unit unit)
        {
            db.units.Add(unit);

            return db;
        }

        public static void populateUnitListBox(Database db, ListBox lb)
        {
            lb.Items.Clear();
            foreach (var unit in db.units)
            {
                if (unit == null) break;
                lb.Items.Add(new ListBoxItem().Content = unit.id+1 + ". " + unit.name);

            }
        }
        
        public static void populateFormatedUnitListBox(Database db, ListBox lb)
        {
            lb.Items.Clear();
            SelectedDB.units.Clear();
            int index = 0;
            foreach (var unit in db.units)
            {
                if (unit == null) break;
                var item = new StackPanel();
                item.Orientation = Orientation.Horizontal;
                item.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                item.Width = lb.Width-5;
                item.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                
                var row1 = new StackPanel();
                row1.Orientation = Orientation.Vertical;
                row1.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                var checkbox = new CheckBox();
                checkbox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                checkbox.Name = "Checkbox_" + index.ToString();
                checkbox.Checked += checkbox_Checked;
                index++;
              
                item.Children.Add(checkbox);

                var text = new TextBlock();
                text.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                text.Text = unit.name;
                text.FontSize = 10;
                
                row1.Children.Add(text);
                
                
                var row2 = new TextBlock();
                row2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                row2.Text = " 0 / " + unit.wordCount + " (0%)";
                row2.FontSize = 10;
                row1.Children.Add(row2);

                item.Children.Add(row1);

                lb.Items.Add(item);
            }
        }

        static void checkbox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox a = (CheckBox)sender;
            string name = a.Name;
            string index_s = name.Split('_')[1];
            int index = int.Parse(index_s.ToString());
           // MessageBox.Show("SZUKAM INDEXU " + index_s);
            
            foreach (var unit in DBHandler.MainDB.units)
            {

               // MessageBox.Show("POROWNUJE " + index_s + " i " + unit.id.ToString());
                if (unit.id == index)
                {
                    SelectedDB.units.Add(unit);
                    //MessageBox.Show("DODANO " + unit.name);
                    break;
                }
                
            }

            
        }
    }
}
