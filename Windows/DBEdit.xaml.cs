using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace mTeacher
{
    /// <summary>
    /// Interaction logic for DBEdit.xaml
    /// </summary>
    public partial class DBEdit : Window
    {
        public List<int> checkedUnits = new List<int>();

        string lastDialoguePath = "";

        public static string databaseDefaultPath = "database.db";

        public DBEdit()
        {
            InitializeComponent();
            if (File.Exists(databaseDefaultPath))
                DBHandler.populateUnitListBox(DBHandler.ReadFromXML(databaseDefaultPath), Unit_List);
            else
                DBHandler.MainDB.UnitCounter = 0;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public Unit[] convert(string data)
        {
            int entryIndex = -1;
            
            bool firstEntry = true;
            string name = "NULL";
            string description = "NULL";
            DateTime time = DateTime.Now;
            List <Word> newWords = new List <Word>();
            Unit[] unit = new Unit[100];
            string[] poSplicie = data.Split('\n');
            string[] bufor;

            foreach (string linia in poSplicie)
            {
                if (linia.StartsWith("##"))
                {
                    if (!firstEntry) unit[entryIndex] = Unit.newUnit(name, description, time, newWords);
                    firstEntry = false;
                    newWords.Clear();
                    entryIndex++;
                   
                    
                    bufor = linia.Split('#');
                    name = bufor[2];
                    description = bufor[3];
                }
                else if (linia != "")
                {

                    bufor = linia.Split('#');
                    newWords.Add(Word.newWord(bufor[1], bufor[0]));
                }
            }

            unit[entryIndex] = Unit.newUnit(name, description, time, newWords);
                
            return unit;
        }

        public string openFile(string path)
        {
            string data = "";
            string filename = path;

            string line;
            using (StreamReader reader = new StreamReader(filename))
            {
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null)
                        break;
                    data += "\n" + line;
                }
            }
            return data;
        }

        public Unit[] openTextDatabase(string path)
        {
            return convert(openFile(path));
        }

        private void File_Picker_Button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();
            string data = "";
            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                lastDialoguePath = filename;
                string line;
                using (StreamReader reader = new StreamReader(filename))
                {
                    while (true)
                    {
                        line = reader.ReadLine();
                        if (line == null)
                            break;
                        data += "\n" + line;
                    }
                }

            }

            

            Unit[] sample = convert(data);
            //addingBufor = sample;

            Word[] export = new Word[1000];
            Label nameLbl = new Label();
            Label name2Lbl = new Label();
            string stringBufor = null;
            int i = 0;
            Results_ListBox.Items.Clear();
            for (i = 0; i < sample.Length; i++)
            {
                if (sample[i] != null)
                {

                    //nameLbl.Content = "Nazwa unitu: " + sample[i].name;
                    stringBufor = "---------------------------------\n";
                    stringBufor += "Nazwa unitu: " + sample[i].name;
                    Results_ListBox.Items.Add(new ListBoxItem().Content = stringBufor);
                    foreach (var word in sample[i].words)
                    {
                        stringBufor = "Słowo id " + word.id + ": " + word.english + " - " + word.polish;
                        Results_ListBox.Items.Add(new ListBoxItem().Content = stringBufor);
                    }
                }
                else break;
            }
            DBHandler.MainDB.UnitCounter -= i;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
                Database newDB = new Database();
            if (File.Exists(databaseDefaultPath))
                newDB = DBHandler.ReadFromXML(databaseDefaultPath);
            
            DBHandler.Add(newDB, openTextDatabase(lastDialoguePath));
            DBHandler.WriteToXML(newDB, databaseDefaultPath);
            DBHandler.populateUnitListBox(DBHandler.ReadFromXML(databaseDefaultPath), Unit_List);
        }

        private void Unit_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string desc = "";
            int index = Unit_List.Items.IndexOf(Unit_List.SelectedItems[0]);
            foreach (var unit in DBHandler.MainDB.units)
            {
                if (unit.id == index)
                {
                    desc = "Opis: " + unit.description + "\n\nID: " + unit.id + "\n\nIlość słów: " + unit.wordCount + "\n\nData utworzenia: " + unit.addTime.ToString() + "\n\n";
                    desc += "\n\nLista słów:";
                    foreach (var item in unit.words)
                    {
                        desc += "\n" + item.id + ": " + item.english + " - " + item.polish;
                    }
                }

            }
            Unit_Desc.Text = desc;
        }

        
    }
}
