using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace 方糖音乐播放器.Properties
{
    /// <summary>
    /// 弹窗提示.xaml 的交互逻辑
    /// </summary>
    public partial class 弹窗提示 : Window
    {
        //播放动画函数
        Storyboard story;
        private void 动画播放(string a)
        {
            story = (Storyboard)FindResource(a);
            BeginStoryboard(story);
        }
        public event Func<int, int> fcc1;
        public 弹窗提示(int parameter, Color color,int Other_parameters)
        {
            InitializeComponent();
            模板一.Visibility = Visibility.Collapsed;
            模板二.Visibility = Visibility.Collapsed;
            单文件.Background = new SolidColorBrush(color);
            if (parameter == 0)
            {
                模板一.Visibility = Visibility.Visible;
            }
            else if (parameter == 1)
            {
                模板二.Visibility = Visibility.Visible;
                if (Other_parameters == 0)
                {
                    提示.Content = "共扫描到了" + Other_parameters + "首歌曲，似乎神马也木有扫" + "\n" + "描到(＃°Д°)";
                }
                else
                {
                    提示.Content = "共扫描到了" + Other_parameters + "首歌曲";
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void 单文件_Click(object sender, RoutedEventArgs e)
        {
            fcc1(1);
            exit();
        }

        private void 全文件_Click(object sender, RoutedEventArgs e)
        {
            fcc1(2);
            exit();
        }

        private void 取消_Click(object sender, RoutedEventArgs e)
        {
            fcc1(3);
            exit();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            exit();
        }

        private void exit()
        {
            动画播放("关闭");
            System.Timers.Timer t1 = new System.Timers.Timer(200);//实例化Timer类
            t1.Elapsed += new System.Timers.ElapsedEventHandler(theout1);//到达时间的时候执行事件
            t1.AutoReset = false;//设置是执行一次（false）还是一直执行(true)
            t1.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }
        public void theout1(object source, System.Timers.ElapsedEventArgs e)
        {
            new Thread(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        Close();
                    }));
            }).Start();
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            动画播放("打开");
        }
    }
}
