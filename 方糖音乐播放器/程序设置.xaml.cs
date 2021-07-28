using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
//using System.Collections.Generic;
using System.IO;
//using System.Linq;
//using System.Text;
using System.Threading;
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
using System.Windows.Threading;
//using 方糖音乐播放器.Properties;

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

        public event Func<string, int> fcc1;//修改背景恢复默认图片
        public event Func<int, int> fcc2;//背景模糊调节
        public event Func<int, int> fcc3;//歌词显示
        public event Func<int, int> fcc4;//如果取消修改
        public event Func<int, int> fcc5;//设置只能打开一个设置窗口
        public event Func<int, int> fcc6;//背景的
        private string 背景路径;
        private string 歌词目录;
        private string 歌曲目录2;
        public 程序设置(Color color)
        {
            InitializeComponent();
            帮助.Visibility = Visibility.Collapsed;
            关于.Visibility = Visibility.Collapsed;
            默认.Background = new SolidColorBrush(color);
            模糊.Value = Properties.Settings.Default.背景模糊程度;
            歌词.IsChecked = Properties.Settings.Default.是否歌词显示;
            独立播放1.IsChecked = Properties.Settings.Default.独立播放视频;
            桌面歌词1.IsChecked = Properties.Settings.Default.桌面歌词;
            嵌入歌词1.IsChecked = Properties.Settings.Default.嵌入歌词;
            错误.IsChecked = Properties.Settings.Default.错误报告;
            专辑图片1.IsChecked = Properties.Settings.Default.显示专辑;
            if (Properties.Settings.Default.背景填充 == 1) { 原始.IsChecked = true; }
            else if (Properties.Settings.Default.背景填充 == 2) { 填充.IsChecked = true; }
            else if (Properties.Settings.Default.背景填充 == 3) { 居中.IsChecked = true; }
            else if (Properties.Settings.Default.背景填充 == 4) { 拉伸.IsChecked = true; }

            if (Properties.Settings.Default.网络接口参数 == "腾讯") { 腾讯.IsChecked = true; }
            else if (Properties.Settings.Default.网络接口参数 == "网易") { 网易.IsChecked = true; }
            else if (Properties.Settings.Default.网络接口参数 == "酷狗") { 酷狗.IsChecked = true; }
            else if (Properties.Settings.Default.网络接口参数 == "虾米") { 虾米.IsChecked = true; }

            动画播放("窗体打开");
            路径.Content = "当前路径：" + Properties.Settings.Default.歌词目录;
            歌曲路径.Content = "当前路径：" + Properties.Settings.Default.网络歌曲缓存目录;
            Topmost = true;
        }

        private void 标题_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        [Obsolete]
        private void 默认背景_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Topmost = false;
            OpenFileDialog Open1 = new OpenFileDialog();
            Open1.Filter = "图片文件(*.jpg,*.png,*.jpeg,*.bmp)|*.jpg;*.png;*.jpeg;*.bmp";
            if (Open1.ShowDialog(this) == true)
            {
                try
                {
                    fcc1(Open1.FileName);
                    背景路径 = Open1.FileName;
                }
                catch { }
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
            else if (歌词.IsChecked == false)
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
            if (歌曲目录2 != null)
            {
                Properties.Settings.Default.网络歌曲缓存目录 = 歌曲目录2;
            }
            Properties.Settings.Default.独立播放视频 = (bool)独立播放1.IsChecked;
            Properties.Settings.Default.桌面歌词 = (bool)桌面歌词1.IsChecked;
            Properties.Settings.Default.嵌入歌词 = (bool)嵌入歌词1.IsChecked;
            Properties.Settings.Default.背景模糊程度 = 模糊.Value;
            Properties.Settings.Default.错误报告 = (bool)错误.IsChecked;
            Properties.Settings.Default.显示专辑 = (bool)专辑图片1.IsChecked;
            Properties.Settings.Default.是否歌词显示 = (bool)歌词.IsChecked;
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
                Topmost = true;
                if (dialog.FileName != "")
                {
                    歌词目录 = dialog.FileName;
                    路径.Content = "当前路径：" + dialog.FileName;
                    路径.ToolTip = dialog.FileName;
                }
                
            }
            catch { }
        }

        private void 默认_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.背景模糊程度 = 0;
            Properties.Settings.Default.背景图片 = "默认";
            Properties.Settings.Default.歌词目录 = "无";
            Properties.Settings.Default.歌单路径 = "五";
            Properties.Settings.Default.音量 = 100;
            Properties.Settings.Default.播放设置 = 0;
            Properties.Settings.Default.主题颜色 = "System.Windows.Controls.ComboBoxItem: 桃花粉";
            Properties.Settings.Default.是否歌词显示 = true;
            Properties.Settings.Default.背景填充 = 2;
            Properties.Settings.Default.嵌入歌词 = false;
            Properties.Settings.Default.独立播放视频 = true;
            Properties.Settings.Default.桌面歌词 = false;
            Properties.Settings.Default.错误报告 = true;
            Properties.Settings.Default.网络接口参数 = "腾讯";
            Properties.Settings.Default.网络歌曲缓存目录 = "系统音乐目录";
            Properties.Settings.Default.Save();
            fcc5(0);
            fcc6(2);
            填充.IsChecked = true;
            fcc1("默认");
            exit();
        }

        private void 路径_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.歌词目录 != "无" && Directory.Exists(Properties.Settings.Default.歌词目录))
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

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            动画播放("关于关闭");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            动画播放("关于打开");
            动画播放("恐龙跳2");
        }
        Color color = (Color)ColorConverter.ConvertFromString("#FF000000");//黑色
        Color color2 = (Color)ColorConverter.ConvertFromString("#FF595959");//灰色
        private void Github_MouseEnter(object sender, MouseEventArgs e)
        {
            Github.Foreground = new SolidColorBrush(color);
        }

        private void Github_MouseLeave(object sender, MouseEventArgs e)
        {
            Github.Foreground = new SolidColorBrush(color2);
        }

        private void Csharp_MouseEnter(object sender, MouseEventArgs e)
        {
            Csharp.Foreground = new SolidColorBrush(color);
        }

        private void Csharp_MouseLeave(object sender, MouseEventArgs e)
        {
            Csharp.Foreground = new SolidColorBrush(color2);
        }

        private void Net_MouseEnter(object sender, MouseEventArgs e)
        {
            Net.Foreground = new SolidColorBrush(color);
        }

        private void Net_MouseLeave(object sender, MouseEventArgs e)
        {
            Net.Foreground = new SolidColorBrush(color2);
        }

        private void _32位版本__MouseEnter(object sender, MouseEventArgs e)
        {
            _32位版本_.Foreground = new SolidColorBrush(color);
        }

        private void _32位版本__MouseLeave(object sender, MouseEventArgs e)
        {
            _32位版本_.Foreground = new SolidColorBrush(color2);
        }

        private void GiftOutline_MouseEnter(object sender, MouseEventArgs e)
        {
            GiftOutline.Foreground = new SolidColorBrush(color);
        }

        private void GiftOutline_MouseLeave(object sender, MouseEventArgs e)
        {
            GiftOutline.Foreground = new SolidColorBrush(color2);
        }

        private void image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            动画播放("旋转");
        }

        private void packIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            动画播放("恐龙跳2");
        }

        private void 腾讯_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.网络接口参数 = "腾讯";
        }

        private void 网易_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.网络接口参数 = "网易";
        }

        private void 酷狗_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.网络接口参数 = "酷狗";
        }

        private void Github_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try { System.Diagnostics.Process.Start("https://github.com/xingchuanzhen/WPF-Music_Player"); }
            catch (UnauthorizedAccessException ex) { MessageBox.Show(ex.Message); }
            fcc5(0);
            exit();
        }

        private void 歌曲路径_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.网络歌曲缓存目录 != "系统临时目录")
            {
                try
                {
                    System.Diagnostics.Process.Start(Properties.Settings.Default.网络歌曲缓存目录 + @"\");
                }
                catch { }
            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(System.IO.Path.GetTempPath() + @"方糖音乐\");
                }
                catch { }
            }
        }

        private void Grid_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Topmost = false;
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                Topmost = true;
                if (dialog.FileName != "")
                {
                    歌曲目录2 = dialog.FileName;
                    歌曲路径.Content = "当前路径：" + dialog.FileName;
                    歌曲路径.ToolTip = dialog.FileName;
                }

            }
            catch { }
        }

        private void GiftOutline_MouseUp(object sender, MouseButtonEventArgs e)
        {
            动画播放("收款码打开");
        }

        private void 关闭_Click(object sender, RoutedEventArgs e)
        {
            动画播放("收款码关闭");
        }

        private void 专辑图片1_Click(object sender, RoutedEventArgs e)
        {
            if (专辑图片1 .IsChecked == false)
            {
                fcc3(7);
            }
            else { fcc3(8); }
        }
    }
}
