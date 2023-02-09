using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Пигалев_Двухфакторная_аунтефикация
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Логин: user
        // Пароль: user
        public static int countNumber; // Колличество введённых чисел
        public static bool correct; // Индефикатор правильности введённого кода (true - правильный код, false - не правильный)
        int countTime; // Время для повторного получения кода
        int kolError = 0; // Количество неверных ответов
        DispatcherTimer disTimer = new DispatcherTimer(); 

        public MainWindow()
        {
            InitializeComponent();
            disTimer.Interval = new TimeSpan(0, 0, 1);
            disTimer.Tick += new EventHandler(DisTimer_Tick);
        }

        private void BtnAutorization_Click(object sender, RoutedEventArgs e)
        {
            if(tbLogin.Text == "user")
            {
                if (pbPassword.Password == "user")
                {
                    ShowAuthorization();
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логиным и паролем не найден");
                }
            }
            else
            {
                MessageBox.Show("Пользователь с таким логиным не найден");
            }
        }

        private void BtnNextAutorization_Click(object sender, RoutedEventArgs e) // При перехода на авторизацию возвращаем все поля
        {
            kolError = 0;
            tbLogin.Text = "";
            pbPassword.Password = "";
            Main.Visibility = Visibility.Collapsed;
            Authorization.Visibility = Visibility.Visible;
            BtnAutorization.Visibility = Visibility.Visible;
            BtnGetCode.Visibility = Visibility.Collapsed;
            tbNewCode.Visibility = Visibility.Collapsed;
            tbLogin.IsEnabled = true;
            pbPassword.IsEnabled = true;
        }
        private void DisTimer_Tick(object sender, EventArgs e)
        {
            if (countTime == 0) // Если 60 секунд закончились, то выводим кнопку для получения нового кода
            {
                BtnGetCode.IsEnabled = true;
                BtnGetCode.Visibility = Visibility.Visible;
                disTimer.Stop();
                tbNewCode.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbNewCode.Text = "Получить новый код можно будет через " + countTime + " секунд";
            }
            countTime--;
        }
        private void ShowAuthorization() // Если введен правильный логин и пароль
        {
            correct = false;
            Random random = new Random();
            int code = random.Next(0, 100000);
            MessageBox.Show("Код для входа: " + code.ToString("D5") + "\nЗапомните его. После нажатия \"Ок\" вам предстоит ввести его");
            TwoAuthentication twoAuthentication = new TwoAuthentication(code.ToString("D5"));
            twoAuthentication.ShowDialog();
            if (correct == true) // Если пароль правильный, то выводим форму после авторизации
            {
                Main.Visibility = Visibility.Visible;
                Authorization.Visibility = Visibility.Collapsed;
                
            }
            else // Если введено неверное число или не успели
            {
                if (kolError >= 1) // Если второй раз код введён не верно, то выводим CAPTCHA
                {
                    CAPTCHA captcha = new CAPTCHA();
                    captcha.ShowDialog();
                    if (correct == true)
                    {
                        Main.Visibility = Visibility.Visible;
                        Authorization.Visibility = Visibility.Collapsed;
                    }
                    else // Если введено не верно
                    {
                        MessageBox.Show("Текст введён не верно! Попробуйте ещё раз!");
                        CAPTCHA captchaReplay = new CAPTCHA();
                        captchaReplay.ShowDialog();
                        if (correct == true)
                        {
                            Main.Visibility = Visibility.Visible;
                            Authorization.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            MessageBox.Show("Вы не подтвердили, что вы не робот. Вход не удачен");
                            tbLogin.Text = "";
                            pbPassword.Password = "";
                            tbLogin.IsEnabled = true;
                            pbPassword.IsEnabled = true;
                            BtnAutorization.Visibility = Visibility.Visible;
                            BtnGetCode.Visibility = Visibility.Collapsed;
                            kolError = 0;
                        }
                    }
                }
                else // Если первый раз код введён не верно
                {
                    if (countNumber == 5) // Если введено 5 значное число
                    {
                        MessageBox.Show("Введенный код не является верным! ");
                    }
                    BtnAutorization.Visibility = Visibility.Collapsed;
                    BtnGetCode.Visibility = Visibility.Collapsed;
                    tbLogin.IsEnabled = false;
                    pbPassword.IsEnabled = false;
                    countTime = 60;
                    tbNewCode.Text = "Получить новый код можно будет через " + countTime + " секунд";
                    tbNewCode.Visibility = Visibility.Visible;
                    disTimer.Start();
                    kolError++;
                }
            }
        }

        private void BtnGetCode_Click(object sender, RoutedEventArgs e)
        {
            ShowAuthorization();
        }
    }
}
