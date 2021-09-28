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

namespace NotAbuse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Windows.Forms.FolderBrowserDialog  folder { get; set; }
        public System.Windows.Forms.FolderBrowserDialog copyTo { get; set; }
        public string path { get; set; }
        public string[] abuseArr { get; set; }
        public bool IsStarted { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            IsStarted = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowDialog();
            folder = folderDialog;
            AppDomain Domain = AppDomain.CreateDomain("Demo Domain");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                path = fileDialog.FileName;
            }
            AppDomain Domain = AppDomain.CreateDomain("Demo Domain");
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    Console.WriteLine();
                    abuseArr = sr.ReadToEnd().ToString().Split(new char[] { ' ' });
                    if(abuse.Text == "" || abuse.Text == "Abuse")
                    {
                        abuse.Text = "";
                        foreach (var str in abuseArr)
                        {
                            if (abuseArr[abuseArr.Length - 1] == str)
                            {
                                abuse.Text += str;
                            }
                            else
                            {
                                abuse.Text += str + ", ";
                            }
                        }
                    }
                    else
                    {
                        abuse.Text += ", ";
                        foreach (var str in abuseArr)
                        {
                            if (abuseArr[abuseArr.Length - 1] == str)
                            {
                                abuse.Text += str;
                            }
                            else
                            {
                                abuse.Text += str + ", ";
                            }
                        }
                        abuseArr = abuse.Text.ToString().Split(new char[] { ' ',',' });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowDialog();
            copyTo = folderDialog;
            AppDomain Domain = AppDomain.CreateDomain("Demo Domain");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if(folder == null)
            {
                MessageBox.Show("Select Folder");
            }
            else if(copyTo == null)
            {
                MessageBox.Show("Select Folder for Copy To");
            }
            else if (abuseArr == null)
            {
                MessageBox.Show("Set Abuse");
            }
            else if(IsStarted)
            {
                MessageBox.Show("You can not started again now");
            }
            else
            {
                lock (this)
                {   
                    lb.Items.Clear();
                    prog.Value = 0;
                    prog.Maximum = 0;
                    Parallel.ForEach(Directory.GetFiles(folder.SelectedPath), FindAbuse);
                    //foreach (var item in Directory.GetFiles(folder.SelectedPath))
                    //{
                    //    int wordNum = 0;
                    //    prog.Maximum++;
                    //    foreach (var i in File.ReadLines(item))
                    //    {
                    //        string[] textArray = i.Split(new char[] { ' ', ',', '.', '!', '?' });
                    //        foreach (var str in textArray)
                    //        {
                    //            foreach (var word in abuseArr)
                    //            {
                    //                if (str == word)
                    //                {
                    //                    wordNum++;
                    //                }
                    //            }
                    //        }
                    //        prog.Value++;
                    //    }
                    //    lb.Items.Add(wordNum + "     " + File.Open(item, FileMode.Open).Name);
                    //}
                    folder = null;
                }   

            }
        }
        
        private void FindAbuse(string item)
        {
            int wordNum = 0;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { prog.Maximum++; }));
            //Application.Current.Dispatcher.Invoke(new Action(() => { prog.Maximum++; }));

            foreach (var i in File.ReadLines(item))
            {
                string[] textArray = i.Split(new char[] { ' ', ',', '.', '!', '?' });
                foreach (var str in textArray)
                {
                    foreach (var word in abuseArr)
                    {
                        if (str == word)
                        {
                            wordNum++;
                        }
                    }
                }
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { prog.Value++; }));
                //Application.Current.Dispatcher.Invoke(new Action(() => { prog.Value++; }));

            }
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { lb.Items.Add(wordNum + "     " + File.Open(item, FileMode.Open).Name); }));
            //Application.Current.Dispatcher.Invoke(new Action(() => { lb.Items.Add(wordNum + "     " + File.Open(item, FileMode.Open).Name); }));

        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Не зупиняйся. Твій шлях ще не закінчився");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }
    }
}
