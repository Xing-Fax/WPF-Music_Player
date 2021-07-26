using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace 方糖音乐播放器
{
    /// <summary>
    /// 桌面歌词.xaml 的交互逻辑
    /// </summary>
    public partial class 桌面歌词 : Window
    {
        public event Func<int, int> fcc1;
        //播放动画函数
        Storyboard story;
        private void 动画播放(string a)
        {
            story = (Storyboard)FindResource(a);
            BeginStoryboard(story);
        }

        public 桌面歌词(Color color)
        {

            InitializeComponent();
            Topmost = true;
            背景.Fill = new SolidColorBrush(color);
            按键.Opacity = 0;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        [Obsolete]
        private void 桌面歌词主窗口_MouseEnter(object sender, MouseEventArgs e)
        {
             动画播放("打开"); 

            if (App.播放状态 == 0)
            {
                播放.Visibility = Visibility.Collapsed;
                暂停.Visibility = Visibility.Visible;
            }
            else
            {
                播放.Visibility = Visibility.Visible;
                暂停.Visibility = Visibility.Collapsed;
            }
        }

        private void 桌面歌词主窗口_MouseLeave(object sender, MouseEventArgs e)
        {
            动画播放("关闭"); 
        }

        [Obsolete]
        private void 播放_Click(object sender, RoutedEventArgs e)
        {
            fcc1(1);
            播放.Visibility = Visibility.Collapsed;
            暂停.Visibility = Visibility.Visible;
        }

        private void 暂停_Click(object sender, RoutedEventArgs e)
        {
            fcc1(2);
            播放.Visibility = Visibility.Visible;
            暂停.Visibility = Visibility.Collapsed;
        }

        private void 上一曲_Click(object sender, RoutedEventArgs e)
        {
            fcc1(3);
        }

        private void 下一曲_Click(object sender, RoutedEventArgs e)
        {
            fcc1(4);
        }
    }
}
