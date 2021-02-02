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

namespace 方糖音乐播放器
{
    /// <summary>
    /// 桌面歌词.xaml 的交互逻辑
    /// </summary>
    public partial class 桌面歌词 : Window
    {
        public 桌面歌词(Color color)
        {

            InitializeComponent();
            Topmost = true;
            背景.Fill = new SolidColorBrush(color);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void 桌面歌词主窗口_MouseEnter(object sender, MouseEventArgs e)
        {
            //桌面歌词主窗口.Background.Opacity = 30;
        }

        private void 桌面歌词主窗口_MouseLeave(object sender, MouseEventArgs e)
        {
            //桌面歌词主窗口.Background.Opacity = 1;
        }
    }
}
