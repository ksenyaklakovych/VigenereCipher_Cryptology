using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;

namespace cryptology_wpf_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentFileName;
        const string uaAlphabet = "абвгґдеєжзиіїклмнопрстуфхцчшщьюяАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
        const string enAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.,!?():;";
        int A;
        int B;
        int C = -1;
        public MainWindow()
        {
            InitializeComponent();
        }

        public enum Direction { Left, Right }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //encryption
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            string firstText = rawTextBox.Text.ToString();
            Direction dir = Direction.Right;
            string result = "";

            if ((bool)englishCheckBox.IsChecked)
            {
                const string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), defaultAlphabet).ToLower();
            }
            else
            {
                const string defaultAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
                result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), defaultAlphabet).ToLower();
            }
            finalTextBox.Text = result;
        }


        public void WriteToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(finalTextBox.Text);
            }
        }

        //decryption       
        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string firstText = rawTextBox.Text.ToString();
            Direction dir = Direction.Right;
            string result;

            if ((bool)englishCheckBox.IsChecked)
            {
                const string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), defaultAlphabet, false).ToLower();

            }
            else
            {
                const string defaultAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
                result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), defaultAlphabet, false).ToLower();
            }
            finalTextBox.Text = result;
        }


        //other buttons
        private void CreateFileButton_Click(object sender, RoutedEventArgs e)
        {
            currentFileName = null;
            rawTextBox.Text = String.Empty;
            finalTextBox.Text = String.Empty;
        }
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentFileName == null)
            {
                currentFileName = "../../resultFile.txt";
            }
            WriteToFile(currentFileName);
            MessageBox.Show($"{currentFileName}\nSuccessfully saved!");
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                rawTextBox.Text = File.ReadAllText(openFileDialog.FileName);
            currentFileName = openFileDialog.FileName;
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This text Caesar Cipher encryption application was created by Ksenia Klakovych.");
        }

        private void PrintFileButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.ShowDialog();
        }
        //Vigenere
        //генерування повторюваного пароля

        private string GetRepeatKey(string s, int n)
        {
            var p = s;
            while (p.Length < n)
            {
                p += p;
            }

            return p.Substring(0, n);
        }
        private string Vigenere(string text, string password, string alph, bool encrypting = true)
        {
            string letters = alph;

            var gamma = GetRepeatKey(password, text.Length);
            var retValue = "";
            var q = letters.Length;

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = letters.IndexOf(text[i]);
                var codeIndex = letters.IndexOf(gamma[i]);
                if (letterIndex < 0)
                {
                    //якщо літера не знайдена, додаємо її в незмінному вигляді
                    retValue += text[i].ToString();
                }
                else
                {
                    retValue += letters[(q + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % q].ToString();
                }
            }

            return retValue;
        }

        private void SwitchTextButton_Click(object sender, RoutedEventArgs e)
        {
            string first = rawTextBox.Text;
            rawTextBox.Text = finalTextBox.Text;
            finalTextBox.Text = first;
        }
    }
}
