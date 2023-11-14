using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TableReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = inputString.Text;
            string result = "";

            int mathSymbolIndex = text.IndexOfAny(new char[]{ '*', '-', '+', '/' });

            if(mathSymbolIndex == -1)
            {
                string allNumbers = new string(text.Where(element => char.IsDigit(element)).ToArray());
                result = allNumbers != "" ? allNumbers : "0";
            }
            else
            {
                string firstPart = new string(text.Take(mathSymbolIndex)
                    .Where(element => char.IsDigit(element)).ToArray());
                string secondPart = new string(text.Skip(mathSymbolIndex)
                    .Where(element => char.IsDigit(element)).ToArray());

                int firstNumb =   string.IsNullOrEmpty(firstPart) ? 0 : int.Parse(firstPart);
                int secondNumb = string.IsNullOrEmpty(secondPart) ? 0 : int.Parse(secondPart);

                char symbol = text[mathSymbolIndex];

                result = symbol switch
                {
                    '+' => $"{firstNumb} + {secondNumb} = {firstNumb + secondNumb}",
                    '-' => $"{firstNumb} - {secondNumb} = {firstNumb - secondNumb}",
                    '*' => $"{firstNumb} * {secondNumb} = {firstNumb * secondNumb}",
                    '/' => secondNumb != 0 ? $"{firstNumb} / {secondNumb} = {Math.Round((double)firstNumb / secondNumb, 1)}"
                    : "Can't devide by zero!",
                    _ => "Something went wrong!",
                };
            }

            MessageBox.Show(result);
        }
    }
}
