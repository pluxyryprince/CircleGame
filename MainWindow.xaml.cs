using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CircleGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int Time; //отсчет времени

        Ellipse o; //объект шарика

        double R = 10; //радиус шарика

        double x = 0, y = 0; //координаты шарика

        double V = 3; //скорость шарика
        double vx, vy; //вектор скорости (направление)

        Rectangle plate; //платформа

        double H = 100; //ширина платформы
        double Px; //координаты платформы
        double Pv = 25; //скорость движения платформы

        DispatcherTimer Timer; //таймер

        public MainWindow()
        {
            InitializeComponent();

            Restart(); //инициализация переменных 

            //шарик
            o = new Ellipse(); //создание объекта шарика
            o.Fill = Brushes.Purple;//цвет фиолетовый
            o.Width = 2 * R;//ширина
            o.Height = 2 * R;//высота
            o.Margin = new Thickness(x, y, 0, 0);//положение
            g.Children.Add(o);//добавить на холст

            //платформа
            plate = new Rectangle();//создвание объекта платформы
            plate.Fill = Brushes.Blue;//цвет синий
            plate.Width = H;//ширина
            plate.Height = 5;//высота
            Px = g.Width / 2 - H / 2;//начальное положение
            plate.Margin = new Thickness(Px, g.Height, 0, 0);//положение
            g.Children.Add(plate);//добавить на холст
            
            //таймер
            Timer = new DispatcherTimer();//создание таймера
            Timer.Tick += new EventHandler(onTick);//обращение к обработчику таймера
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);//утсановить таймер на 10 миллисекунд
            Timer.Start();//запуск таймера
        }

        public void Restart()//метод для перезапуска игры
        {
            x = g.Width / 2 - R;//начальные координаты шарика - центр холста
            y = g.Height / 2 - R;

            Random rand = new Random();
            double alpha = rand.NextDouble() * Math.PI / 2 + Math.PI / 4;//случайный угол полета шарика

            vx = V * Math.Cos(alpha);//направление скорости 
            vy = V * Math.Sin(alpha);//модуль скорости

            Px = g.Width / 2 - H / 2;//начальное положение платформы

            Time = 0;//обнуление таймера
        }

        public void onTick(object sender, EventArgs e)//обработчик таймера 
        {
            Time++;//увеличить отсчет времени на единицу

            if ((x < 0) || (x > g.Width - 2 * R))
            {
                vx = -vx; //если шарик ударился о вертикальную стенку, то отразить (поменять знак вектора направления)
            }

            if ((y < 0) || (y > g.Height - 2 * R))
            {
                vy = -vy; //аналогично если шарик ударился о горизонтальную стенку
            }


            //если шарик ударился о нижнюю стенку
            if (y > g.Height - 2 * R)
            {
                double c = x + R;//х - координата центра шарика


                //проверка на столкновение с платформой
                if ((c >= Px) && (c <= Px + H))
                {
                    //если столкновились с ракеткой, то увеличить скорость на 10%
                    vx *= 1.1;
                    vy *= 1.1;
                }
                else
                {
                    //иначе выводится сообщщение и игра перезапускается
                    MessageBox.Show("Game over");
                    Restart();
                    plate.Margin = new Thickness(Px, g.Height, 0, 0);//положение платформы по середние
                }

            }
            x += vx;//изменение координат шарика
            y += vy;

            o.Margin = new Thickness(x, y, 0, 0);//изменить положение шарика

            tbTime.Text = (Time / 100).ToString();//обновление значение в текстбоксе
        }

        private void cmKey(object sender, KeyEventArgs e)//обработчик нажатий на стрелки
        {
            if (e.Key == Key.Left)//если нажата стрелка влево
            {
                Px -= Pv;//движение платформы влево
            }
            if (e.Key == Key.Right)//если нажата стрелка вправо
            {
                Px += Pv;//движение платформы вправо
            }
            if (Px < 0)
            {
                Px = 0;//предотвращение выхода ракетки за пределы поля
            }
            if (Px > g.Width - H)
            {
                Px = g.Width - H;//предотвращение выхода ракетки за пределы поля
            }
            plate.Margin = new Thickness(Px, g.Height, 0, 0);//изменение положения ракетки
        }
    }
}
