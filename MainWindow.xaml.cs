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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace mTeacher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double windowDefaultHeight = 342;
        double windowDefaultWidth = 167;
        double heightSetter = 0;
        double widthSetter = 0;
        int[] checkedUnits = new int[1000];

        public MainWindow()
        {
            InitializeComponent();

            windowDefaultHeight = Application.Current.MainWindow.Height;
            windowDefaultWidth = Application.Current.MainWindow.Width;

            heightSetter = windowDefaultHeight;
            widthSetter = windowDefaultWidth;


            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();

            if (File.Exists(DBEdit.databaseDefaultPath))
                DBHandler.populateFormatedUnitListBox(DBHandler.ReadFromXML(DBEdit.databaseDefaultPath), Unit_ListBox);
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (Math.Abs(Application.Current.MainWindow.Height - heightSetter) > 1)
            {
                Application.Current.MainWindow.Height = Application.Current.MainWindow.Height - (Application.Current.MainWindow.Height - heightSetter) / 15;

            }
            if (Math.Abs(Application.Current.MainWindow.Width - widthSetter) > 1)
            {
                Application.Current.MainWindow.Width = Application.Current.MainWindow.Width - (Application.Current.MainWindow.Width - widthSetter) / 15;
            }
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Unit_Select_Button_Click(object sender, RoutedEventArgs e)
        {
            widthSetter = 500;
        }

        private void DBEditor_Button_Click(object sender, RoutedEventArgs e)
        {
            Window DBEdit = new DBEdit();
            DBEdit.Show();
        }

        private void Main_Window_Activated(object sender, EventArgs e)
        {
            if (File.Exists(DBEdit.databaseDefaultPath))
                DBHandler.populateFormatedUnitListBox(DBHandler.ReadFromXML(DBEdit.databaseDefaultPath), Unit_ListBox);
           
        }
        
        private void Unit_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string desc = "";
            string wordlist = "";
            int index = Unit_ListBox.Items.IndexOf(Unit_ListBox.SelectedItems[0]);
            foreach (var unit in DBHandler.MainDB.units)
            {
                //string checkboxName = unit.id.ToString();
                if (unit.id == index)
                {
                    desc = unit.description + "\n\n";
                    wordlist += "Lista słów:\n";
                    foreach (var item in unit.words)
                    {
                        wordlist += item.english + ", ";
                    }
                }
                //var lista = Unit_ListBox.Items.OfType<CheckBox>;
                {

                }

            }
           
            Unit_Desc_Text.Text = desc;
            Unit_Wordlist_Text.Text = wordlist;
        }


         private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            string message = "";
            foreach (var unit in DBHandler.SelectedDB.units)
            {
                message += unit.id + ":" + unit.name + "\n";
            }
            MessageBox.Show(message);
        }


        
    }
}
