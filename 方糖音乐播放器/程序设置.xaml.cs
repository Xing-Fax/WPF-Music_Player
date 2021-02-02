using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace 方糖音乐播放器
{
    /// <summary>
    /// 程序设置.xaml 的交互逻辑
    /// </summary>
    public partial class 程序设置 : Window
    {
        //播放动画函数
        Storyboard story;
        private void 动画播放(string a)
        {
            story = (Storyboard)FindResource(a);
            BeginStoryboard(story);
        }
        public event Func<string, int> fcc1;//定义委托
        public event Func<int, int> fcc2;
        public event Func<int, int> fcc3;
        public event Func<int, int> fcc4;
        public event Func<int, int> fcc5;
        public event Func<int, int> fcc6;
        private string 背景路径;
        private string 歌词目录;
        public 程序设置(Color color)
        {
            InitializeComponent();
            默认.Background = new SolidColorBrush(color);
            模糊.Value = Properties.Settings.Default.背景模糊程度;
            歌词.IsChecked = Properties.Settings.Default.是否歌词显示;
            独立播放1.IsChecked = Properties.Settings.Default.独立播放视频;
            桌面歌词1.IsChecked = Properties.Settings.Default.桌面歌词;
            嵌入歌词1.IsChecked = Properties.Settings.Default.嵌入歌词;
            if (Properties.Settings.Default.背景填充 == 1) { 原始.IsChecked = true; }
            else if (Properties.Settings.Default.背景填充 == 2) { 填充.IsChecked = true; }
            else if (Properties.Settings.Default.背景填充 == 3) { 居中.IsChecked = true; }
            else if (Properties.Settings.Default.背景填充 == 4) { 拉伸.IsChecked = true; }
            动画播放("窗体打开");
            路径.Content = "当前路径：" + Properties.Settings.Default.歌词目录;
            Topmost = true;
        }

        private void 标题_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        
        private void 默认背景_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Topmost = false;
            OpenFileDialog Open1 = new OpenFileDialog();
            Open1.Filter = "图片文件(*.jpg,*.png,*.jpeg,*.bmp)|*.jpg;*.png;*.jpeg;*.bmp";
            if (Open1.ShowDialog(this) == true)
            {
                fcc1(Open1.FileName);
                背景路径 = Open1.FileName;
            }
            Topmost = true;
        }
        int Temp = 0;
        private void 模糊_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Temp > 2)
            {
                fcc2((int)Math.Round(模糊.Value));
            }
            Temp += 1;
        }

        private void 取消_Click(object sender, RoutedEventArgs e)
        {
            fcc4(0);
            fcc5(0);
            exit();
        }

        private void 歌词_Click(object sender, RoutedEventArgs e)
        {
            if (歌词.IsChecked == true)
            {
                fcc3(2);
            }
            else if (歌词.IsChecked == false )
            {
                fcc3(1);
            }
        }

        private void 平滑_Click(object sender, RoutedEventArgs e)
        {
            if (平滑.IsChecked == true)
            {
                fcc3(3);
            }
            else if (平滑.IsChecked == false)
            {
                fcc3(4);
            }
        }

        private void 保存_Click(object sender, RoutedEventArgs e)
        {
            if (背景路径 != null)
            {
                Properties.Settings.Default.背景图片 = 背景路径;
            }
            if (歌词目录 != null)
            {
                Properties.Settings.Default.歌词目录 = 歌词目录;
            }
            Properties.Settings.Default.独立播放视频 = (bool)独立播放1.IsChecked;
            Properties.Settings.Default.桌面歌词 = (bool)桌面歌词1.IsChecked;
            Properties.Settings.Default.嵌入歌词 = (bool)嵌入歌词1.IsChecked;
            Properties.Settings.Default.背景模糊程度 = 模糊.Value;
            Properties.Settings.Default.Save();//保存设置

            fcc5(0);
            exit();

        }

        private void exit()
        {
            动画播放("窗体关闭");
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
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Topmost = false;
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                if (dialog.FileName != "")
                {
                    歌词目录 = dialog.FileName;
                    路径.Content = "当前路径：" + dialog.FileName;
                    路径.ToolTip = dialog.FileName;
                }
                Topmost = true;
            }
            catch { }
        }

        private void 默认_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.背景模糊程度 = 5;
            Properties.Settings.Default.背景图片 = "默认";
            Properties.Settings.Default.歌词目录 = "无";
            Properties.Settings.Default.音量 = 100;
            Properties.Settings.Default.播放设置 = 0;
            Properties.Settings.Default.主题颜色 = "System.Windows.Controls.ComboBoxItem: 宝石绿";
            Properties.Settings.Default.是否歌词显示 = true;
            Properties.Settings.Default.背景填充 = 2;
            Properties.Settings.Default.嵌入歌词 = false;
            Properties.Settings.Default.独立播放视频 = true;
            Properties.Settings.Default.桌面歌词 = false;
            Properties.Settings.Default.Save();
            fcc5(0);
            fcc6(2);

            //fcc3(6);

            填充.IsChecked = true;
            fcc1("默认");
            exit();
        }

        private void 路径_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(Properties.Settings.Default.歌词目录 != "无" && Directory.Exists(Properties.Settings.Default.歌词目录))
            {
                if (歌词目录 != null)
                {
                    System.Diagnostics.Process.Start(歌词目录);
                }
                else
                {
                    System.Diagnostics.Process.Start(Properties.Settings.Default.歌词目录);
                }

            }
        }

        private void 桌面歌词1_Click(object sender, RoutedEventArgs e)
        {
            if (桌面歌词1.IsChecked == true)
            {
                fcc3(5);
            }
            else
            {
                fcc3(6);
            }
        }

        private void 原始_Click(object sender, RoutedEventArgs e)
        {
            fcc6(1); Properties.Settings.Default.背景填充 = 1;
        }

        private void 填充_Click(object sender, RoutedEventArgs e)
        {
            fcc6(2); Properties.Settings.Default.背景填充 = 2;
        }

        private void 拉伸_Click(object sender, RoutedEventArgs e)
        {
            fcc6(3); Properties.Settings.Default.背景填充 = 3;
        }

        private void 居中_Click(object sender, RoutedEventArgs e)
        {
            fcc6(4); Properties.Settings.Default.背景填充 = 4;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            动画播放("帮助关闭");
        }

        private void ListBoxItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            动画播放("帮助打开");
        }

        private void Grid_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


    }
}
