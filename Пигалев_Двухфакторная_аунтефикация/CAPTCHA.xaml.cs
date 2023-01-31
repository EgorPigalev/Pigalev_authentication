using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Xml;
using System.Xml.Linq;

namespace Пигалев_Двухфакторная_аунтефикация
{
    /// <summary>
    /// Логика взаимодействия для CAPTCHA.xaml
    /// </summary>
    public partial class CAPTCHA : Window
    {
        public static string text;
        public CAPTCHA()
        {
            InitializeComponent();
            Random rand = new Random();
            int kolLine = rand.Next(5, 16); // Рандомное количество линий
            for(int i = 0; i < kolLine; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256))); // Рандомный RGB цвет
                Line l = new Line()
                {
                    X1 = rand.Next((int)CvField.Width),
                    Y1 = rand.Next((int)CvField.Height),
                    X2 = rand.Next((int)CvField.Width),
                    Y2 = rand.Next((int)CvField.Height),
                    Stroke = brush,
                };
                CvField.Children.Add(l);
            }
            int kolText = rand.Next(7, 11); // Количество символов в строке
            text = "";
            for(int i = 0; i < kolText; i++)
            {
                int j = rand.Next(2); // Выбор 0 - число; 1 - буква
                if(j == 0)
                {
                    text = text + rand.Next(9).ToString();
                }
                else
                {
                    int l = rand.Next(2); // Выбор 0 - заглавная; 1 - маленькая буква
                    if (l == 0)
                    {
                        text = text + (char)rand.Next('A', 'Z' + 1);
                    }
                    else
                    {
                        text = text + (char)rand.Next('a', 'z' + 1);
                    }
                }
            }
            // Переменные для того, чтобы символы шли по порядку
            int widthBegin = 0; // Начало отрезка
            int widthEnd = 0; // Конец отрезка
            int h = (int)CvField.Width / text.Length; // Шаг разбиения
            for (int i = 0; i < text.Length; i++) // Заполнение текста, где у каждого символа разное оформление
            {
                if (i == 0) // Если первое разбиение
                {
                    widthEnd += h;
                }
                else
                {
                    widthBegin = widthEnd;
                    widthEnd += h;
                }
                int height = rand.Next((int)CvField.Height);
                int width = rand.Next(widthBegin, widthEnd);
                if (height > 170) // Чтобы не выходило за пределы поля (30 - это самое большая высота символа)
                {
                    height -= 30;
                }
                if(width > 590) // Чтобы не выходило за пределы поля (10 - это самое большая длина символа)
                {
                    widthEnd -= 10;
                }
                int j = rand.Next(3); // Выбор стиля символа (0 - жирный шрифт; 1 - курсивный; 2 - жирный и курсивный)
                if (j == 0)
                {
                    int fontSize = rand.Next(16, 33);
                    TextBlock txt = new TextBlock()
                    {
                        Text = text[i].ToString(),
                        FontWeight = FontWeights.Bold,
                        Padding = new Thickness(width, height, 0, 0),
                        FontSize = fontSize,
                    };
                    CvField.Children.Add(txt);
                }
                else if (j == 1)
                {
                    int fontSize = rand.Next(16, 33);
                    TextBlock txt = new TextBlock()
                    {
                        Text = text[i].ToString(),
                        FontStyle = FontStyles.Italic,
                        Padding = new Thickness(width, height, 0, 0),
                        FontSize = fontSize,
                    };
                    CvField.Children.Add(txt);
                }
                else if (j == 2)
                {
                    int fontSize = rand.Next(16, 33);
                    TextBlock txt = new TextBlock()
                    {
                        Text = text[i].ToString(),
                        FontWeight = FontWeights.Bold,
                        FontStyle = FontStyles.Italic,
                        Padding = new Thickness(width, height, 0, 0),
                        FontSize = fontSize,
                    };
                    CvField.Children.Add(txt);
                }
            }
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            if (tbInputField.Text == text)
            {
                MainWindow.correct = true;
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
    }
}
