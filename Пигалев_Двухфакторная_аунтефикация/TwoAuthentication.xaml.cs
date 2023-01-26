using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Пигалев_Двухфакторная_аунтефикация
{
    /// <summary>
    /// Логика взаимодействия для TwoAuthentication.xaml
    /// </summary>
    public partial class TwoAuthentication : Window
    {
        int count = 10; // Время для ввода кода
        DispatcherTimer disTimer = new DispatcherTimer();
        string code;
        public TwoAuthentication(string code)
        {
            InitializeComponent();
            disTimer.Interval = new TimeSpan(0, 0, 1);
            disTimer.Tick += new EventHandler(DisTimer_Tick);
            disTimer.Start();
            MainWindow.countNumber = 0;
            this.code = code;
        }

        private void tbCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbCode.Text == code)
            {
                MainWindow.correct = true;
                this.Close();
            }
            MainWindow.countNumber = tbCode.Text.Length;
        }
        private void DisTimer_Tick(object sender, EventArgs e)
        {
            if(count == 0)
            {
                this.Close();
            }
            else
            {
                tbRemainingTime.Text = "Оставшееся время: " + count + " секунд";
            }
            count--;
        }

        private void tbCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }
    }
}
