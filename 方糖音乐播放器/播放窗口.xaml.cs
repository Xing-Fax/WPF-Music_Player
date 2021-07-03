using Microsoft.Win32;
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
    /// 播放窗口.xaml 的交互逻辑
    /// </summary>
    public partial class 播放窗口 : Window
    {
        System.Timers.Timer t = new System.Timers.Timer(50);//实例化Timer类用于更新时间
        System.Timers.Timer t2 = new System.Timers.Timer(5000);//实例化Timer类用于关闭底部播放栏
        System.Timers.Timer t3 = new System.Timers.Timer(200);//实例化Timer类用用于判断是否双击屏幕
        private string Song_time;
        private string SP_sile;
        public 播放窗口(Color color, string file)
        {
            InitializeComponent();
            SP_sile = file;
            方块一.Fill = new SolidColorBrush(color);
            Play(file);
        }

        private void Play(string file)
        {
            播放器.Stop();
            TagLib.File Read_information = TagLib.File.Create(file);
            播放窗口1.Title = System.IO.Path.GetFileNameWithoutExtension(file);
            Song_time = Convert.ToString(Read_information.Properties.Duration).Substring(3, 5);//获取时长
            int second = int.Parse(Convert.ToString(Read_information.Properties.Duration).Substring(3, 2)) * 60 + int.Parse(Convert.ToString(Read_information.Properties.Duration).Substring(6, 2));//计算视频秒数
            进度条.Maximum = second;//将进度条最大值赋值
            进度条.Value = 0;

            t.Elapsed += new System.Timers.ElapsedEventHandler(theout1);//到达时间的时候执行事件
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
            t.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件

            t2.Elapsed += new System.Timers.ElapsedEventHandler(theout2);//到达时间的时候执行事件
            t2.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
            t2.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件

            t3.Elapsed += new System.Timers.ElapsedEventHandler(theout3);//到达时间的时候执行事件
            t3.AutoReset = false;//设置是执行一次（false）还是一直执行(true)
            t3.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件

            循环.IsChecked = Properties.Settings.Default.循环视频;

            播放器.Source = new Uri(file, UriKind.Relative);//定位视频
            播放器.Play();//开始播放
            播放_PreviewMouseUp(null, null);
            动画播放("菜单打开");
        }

        public void theout1(object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(//同步线程
                   new Action(
                 delegate
                 {
                     if (进度条.Value == 进度条.Maximum && button == 0)//判断进度条是否到底
                     {//如果到底
                         if (循环.IsChecked == false) { Close(); }
                         else
                         {
                             t.Close();
                             t2.Close();
                             Play(SP_sile);
                             t.Start();
                             t2.Start();
                         }
                     }
                     else
                     {//如果没有到底，继续更新时间
                      //刷新播放时间
                         if (button == 0)
                         {
                             播放时间.Content = Convert.ToString(播放器.Position).Substring(3, 5) + "-" + Song_time;
                             进度条.Value = 播放器.Position.TotalSeconds;
                         }
                     }
                 }));
        }

        public void theout2(object source, System.Timers.ElapsedEventArgs e)
        {
            if (底部播放栏 .Visibility == Visibility.Visible)
            {
                动画播放("菜单关闭");
            }
        }
        //播放动画函数
        Storyboard story;
        private void 动画播放(string a)
        {
            Dispatcher.Invoke(//同步线程
                   new Action(
                 delegate
                 {
                     story = (Storyboard)FindResource(a);
                     BeginStoryboard(story);
                 }));
        }

        private void 播放_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            播放.Visibility = Visibility.Collapsed;
            暂停.Visibility = Visibility.Visible;
            播放器.Play();
        }

        private void 暂停_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            暂停.Visibility = Visibility.Collapsed;
            播放.Visibility = Visibility.Visible;
            播放器.Pause();
        }

        private void 快退_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try { 播放器.Position -= TimeSpan.FromSeconds(10); }
            catch { }
        }

        private void 快进_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try { 播放器.Position += TimeSpan.FromSeconds(30); }
            catch { }
        }

        private void 全屏_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            播放窗口1_MouseDoubleClick(null, null);
        }

        private void 退出全屏_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            播放窗口1_MouseDoubleClick(null, null);
        }

        private void 音量_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (音量框架.Visibility == Visibility.Collapsed )
            {
                动画播放("音量打开");
            }
            else
            {
                动画播放("音量关闭");
            }
        }

        private void 音量框架_MouseLeave(object sender, MouseEventArgs e)
        {
            动画播放("音量关闭");
        }
        private int button = 0;//，在用户拖动进度条时，判断是否要刷新进度条
        private void 进度条_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)//左键按下
            {
                button = 1;
            }
        }

        private void 播放器_MediaOpened(object sender, RoutedEventArgs e)
        {
            t.Start();
            t2.Stop();
        }

        private void 进度条_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (button == 1)
            {
                播放时间.Content = Function_list.sec_to_hms(进度条.Value).Substring(3, 5) + "-" + Song_time;
            }
        }

        private void 进度条_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            播放器.Position = TimeSpan.FromSeconds(进度条.Value);//调整进度
            button = 0;
        }

        private void 进度条_MouseEnter(object sender, MouseEventArgs e)
        {
            t.Interval = 1000;
        }

        private void 进度条_MouseLeave(object sender, MouseEventArgs e)
        {
            t.Interval = 50;
        }
        int temp = 0;
        private void 音量条_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (temp == 1)
            {
                音量显示.Content = (int)音量条.Value + "%";
                播放器.Volume = 音量条.Value / 100;
                if (音量条.Value == 0)
                {
                    有.Visibility = Visibility.Collapsed;
                    无.Visibility = Visibility.Visible;
                }
                else
                {
                    有.Visibility = Visibility.Visible;
                    无.Visibility = Visibility.Collapsed;
                }
            }
            temp = 1;
        }

        private void 播放窗口1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FullScreenHelper.IsFullscreen(this))
            {
                FullScreenHelper.ExitFullscreen(this);//退出
                全屏.Visibility = Visibility.Visible;
                退出全屏.Visibility = Visibility.Collapsed;
            }
            else
            {
                FullScreenHelper.GoFullscreen(this);//打开
                全屏.Visibility = Visibility.Collapsed;
                退出全屏.Visibility = Visibility.Visible;
            }
        }

        private void 播放窗口1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            播放器.Width = ActualWidth;
            播放器.Height = ActualHeight;
        }

        private void 播放窗口1_MouseMove(object sender, MouseEventArgs e)
        {
            if (底部播放栏 .Visibility == Visibility.Collapsed) { 动画播放("菜单打开"); }
            else
            {
                t2.Stop();
                t2.Interval = 5000;
                t2.Start();
            }
        }

        private void 打开文件_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog Open1 = new OpenFileDialog();
            Open1.Filter = "视频文件(*.mp4,*.avi,*.wmv)|*.mp4;*.avi;*.wmv";
            if (Open1.ShowDialog(this) == true)
            {
                Play(Open1.FileName);
                SP_sile = Open1.FileName;
            }
        }

        private void 播放窗口1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Space")//空格
            {
                if (播放.Visibility == Visibility.Visible) { 播放_PreviewMouseUp(null, null); }
                else { 暂停_PreviewMouseUp(null, null); }
            }
            else if (e.Key.ToString() == "Left")//左
            {
                if (快退.Visibility == Visibility.Visible) { 快退_PreviewMouseUp(null, null); }
            }
            else if (e.Key.ToString() == "Right")//右
            {
                if (快进.Visibility == Visibility.Visible) { 快进_PreviewMouseUp(null, null); }
            }
        }

        private void 播放窗口1_Activated(object sender, EventArgs e)
        {
            Topmost = true;
            Topmost = false;
        }

        private void 循环_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.循环视频 = (bool)循环.IsChecked;
            Properties.Settings.Default.Save();//保存设置
        }
        //判断是否双击
        public void theout3(object source, System.Timers.ElapsedEventArgs e)
        {
            if (button2 >= 2)
            {//双击

            }
            else if (button2 == 1)
            {//单机
                if (底部播放栏.Visibility == Visibility.Collapsed)
                {
                    动画播放("菜单打开");
                }
                else
                {
                    动画播放("菜单关闭");
                    if (音量框架.Visibility == Visibility.Visible)
                    {
                        动画播放("音量关闭");
                    }
                }
            }
            button2 = 0;
            boole = true;
        }
        int button2 = 0;//判断是双击还是单机
        bool boole = true;//放在两次触发计数器
        private void 播放窗口1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button2++;
            if (boole == true)
            {
                t3.Start();
                boole = false;
            }
        }

        private void 播放窗口1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effects = DragDropEffects.Link;
            else e.Effects = DragDropEffects.None;
        }

        private void 播放窗口1_Drop(object sender, DragEventArgs e)
        {
            string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (System.IO.Path.GetExtension(filePath[0]) == ".mp4" || System.IO.Path.GetExtension(filePath[0]) == ".wmv" || System.IO.Path.GetExtension(filePath[0]) == ".avi")
            {
                Play(filePath[0]);
                SP_sile = filePath[0];
            }
        }
    }
}
