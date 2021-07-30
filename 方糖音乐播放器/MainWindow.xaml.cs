using Meting4Net.Core;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using Shell32;
using System;
using System.Collections;
//using System.Collections.Generic;
using System.ComponentModel;
//using System.Diagnostics;
using System.IO;
using System.Net;
//using System.Runtime.InteropServices;
//using System.Security.Cryptography.X509Certificates;
//using System.Linq;
//using System.Text;
//using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
//using System.Threading;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
//using TagLib;
using 方糖音乐播放器.Properties;
//using System.Threading;
//using Microsoft.Expression.Interactivity.Layout;

namespace 方糖音乐播放器
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        [Obsolete]
        private void Song_scanning(int a, string file)//1全盘2单文件夹，文件夹路径
        {
            Kill();
            主页的播放列表.Items.Clear();//清空原始歌曲
            Number.Clear();//清空数组
            if (a == 1)
            {
                Function_list.ListFiles(主页的播放列表, new DirectoryInfo(file), Number, Tips, color3);
                Number_of_songs = Number.Count;
                歌曲数量.Content = "共" + Number_of_songs + "首歌曲";
                Properties.Settings.Default.歌单路径 = file;
                Properties.Settings.Default.歌单路径参数 = 2;
            }
            else if (a == 2)
            {
                Function_list.query(主页的播放列表, file, Number, Tips, color3);
                Number_of_songs = Number.Count;
                歌曲数量.Content = "共" + Number_of_songs + "首歌曲";
                Properties.Settings.Default.歌单路径 = file;
                Properties.Settings.Default.歌单路径参数 = 1;
            }
            Tips = new 弹窗提示(1, color3, Number_of_songs, null);
            Tips.ShowDialog();
        }

        private void Read_lyrics_string(string lrc)
        {
            int a = 0;
            string[] temp = lrc.Split('\n');//拆分字符串
            for (int i = 0; i < temp.Length; i++)
            {
                //去除歌词前部分歌曲信息
                if (temp[i] != "" && temp[i].Substring(1, 2) != "ar" && temp[i].Substring(1, 2) != "ti" && temp[i].Substring(1, 2) != "al" && temp[i].Substring(1, 2) != "by" && temp[i].Substring(1, 2) != "of")
                {
                    temp[i] = temp[i].Replace(@"\r", "");//某些歌词可能存在\r字符串
                    lrc_time.Add(Function_list.Substring(temp[i], "[", "]"));//截取字符串，分离时间
                    lrc_lyrics.Add(temp[i].Substring(lrc_time[a].ToString().Length + 2, temp[i].Length - lrc_time[a].ToString().Length - 2));//截取歌词
                    if (lrc_lyrics[a].ToString() == "" || lrc_lyrics[a].ToString() == "//\r") //剔除空行和"//"
                    {
                        lrc_time.RemoveAt(a);
                        lrc_lyrics.RemoveAt(a);
                    }
                    else { a ++; }//当符合条件数组下标才会定位到下一行
                }
            }
            打印歌词();//将读取后的歌词打印到滚动歌词控件上
        }

        //将歌词打印到歌词滚动上
        private void 打印歌词()
        {
            for (int i = 0; i < lrc_time.Count; i++)
            { //遍历打印歌词
                if (lrc_lyrics[i] != null) { Function_list.填充菜单(歌词滚动显示, lrc_lyrics[i].ToString(), null, 470, 35, 16, true); }
            }
            try { ((ListBoxItem)歌词滚动显示.Items[0]).Foreground = new SolidColorBrush(color); }//将第一句歌词颜色该为红色
            catch { }
            if (歌词滚动显示.Items.Count > 3)
            {
                for (int i = 0; i < 5; i++) { Function_list.填充菜单(歌词滚动显示, null, null, 470, 35, 15, true); }//多打印空白行5个
            }
        }
        //自动嵌入歌词函数
        private string Embedded_lyrics()
        {
            string lrc = "";
            for (int i = 0; i < lrc_time.Count; i++)
            { 
                if (lrc_time[i] != null) 
                {//将读取到的歌词写格式化写入到字符串
                    lrc += "[" + lrc_time[i] + "]" + lrc_lyrics[i] + "\n"; 
                }
            }
            return lrc;
        }

        //播放音乐函数
        private string Song_time;//存储歌曲最大时间单位00：00
        private int Geci = 0;//决定是否要播放歌词
        [Obsolete]
        private void Play_Misic(string File_s)
        {
            try
            {
                if (System.IO.File.Exists(File_s))//判断文件是否存在
                {//状态正常
                    TagLib.File Read_information = TagLib.File.Create(File_s);
                    string name = Read_information.Tag.Title;//获取歌曲名称
                    string Albumstr = Read_information.Tag.Album;//获取专辑
                    string namestr = Read_information.Tag.Lyrics;//获取歌词
                    string[] artist = Read_information.Tag.Artists;//获取歌手
                    Song_time = Convert.ToString(Read_information.Properties.Duration).Substring(3, 5);//获取时长
                    进度条.Maximum = int.Parse(Convert.ToString(Read_information.Properties.Duration).Substring(3, 2)) * 60 + int.Parse(Convert.ToString(Read_information.Properties.Duration).Substring(6, 2));//计算歌曲秒数//将进度条最大值赋值
                    string Singer_collection = "";//存储歌手集合
                    //如果有多位艺术家可将他们叠加到一起，分隔符“：”
                    for (int i = 0; i < artist.Length; i++)
                    {
                        if (i > 0) { Singer_collection += ";" + artist[i]; }
                        else { Singer_collection = artist[i]; }
                    }
                    //写入歌词名称专辑...到控件
                    if (name == null || name == "kuwo")
                    {
                        歌曲名称.Content = System.IO.Path.GetFileNameWithoutExtension(File_s);
                        播放歌曲名称.Content = System.IO.Path.GetFileNameWithoutExtension(File_s);
                        歌手专辑.Content = "歌手:" + "未知艺术家";
                        歌手专辑2.Content = "专辑:" + "未知专辑";
                    }
                    else
                    {
                        歌曲名称.Content = name + "-" + Singer_collection;
                        播放歌曲名称.Content = name;
                        歌手专辑.Content = "歌手:" + Singer_collection;
                        歌手专辑2.Content = "专辑:" + Albumstr;
                    }
                    try
                    {
                        //读取歌词
                        if (namestr != null)//判断歌曲是否内嵌入歌词
                        {//如果有
                            Read_lyrics_string(namestr);
                            Geci = 0;
                        }
                        else if (Properties.Settings.Default.歌词目录 != "默认" && Directory.Exists(Properties.Settings.Default.歌词目录) && System.IO.File.Exists(Properties.Settings.Default.歌词目录 + @"\" + System.IO.Path.GetFileNameWithoutExtension(File_s) + ".lrc"))//如果没有找到歌词，从用户设置目录读取
                        {//如果用户设置了目录，并且目录存在,并且目录下存在与歌曲名称同名的歌词文件
                            Read_lyrics_string(Function_list.读取歌词(Properties.Settings.Default.歌词目录 + @"\" + System.IO.Path.GetFileNameWithoutExtension(File_s) + ".lrc"));
                            Geci = 0;
                            if (Properties.Settings.Default.嵌入歌词 == true)
                            {
                                Read_information.Tag.Lyrics = Embedded_lyrics();
                                Read_information.Save();
                            }

                        }//从歌曲目录下寻找歌词文件
                        else if (System.IO.File.Exists(Regex.Replace(File_s, System.IO.Path.GetFileName(File_s), string.Empty, RegexOptions.Compiled) + System.IO.Path.GetFileNameWithoutExtension(File_s) + ".lrc"))
                        {//如果存在
                            Read_lyrics_string(Function_list.读取歌词(Regex.Replace(File_s, System.IO.Path.GetFileName(File_s), string.Empty, RegexOptions.Compiled) + System.IO.Path.GetFileNameWithoutExtension(File_s) + ".lrc"));
                            Geci = 0;
                            if (Properties.Settings.Default.嵌入歌词 == true)
                            {
                                Read_information.Tag.Lyrics = Embedded_lyrics();
                                Read_information.Save();
                            }
                        }
                        else
                        {
                            Read_lyrics_string("[00:05.00]该歌曲为纯音乐,或没有找到歌词文件");
                            Geci = 1;
                            if (Get != null)//如果启用了桌面歌词
                            {
                                Get.主.Content = name;
                                Get.父.Content = "该歌曲为纯音乐,或没有找到歌词文件";
                            }
                        }//歌词不存在，不打印歌词
                    }
                    catch (Exception ex)
                    {
                        Function_list.Error_capture("读写歌词时发生异常:\n" + ex.ToString(), Tips, color3);
                    }

                    //读取专辑图片
                    if (Read_information.Tag.Pictures != null && Read_information.Tag.Pictures.Length != 0)
                    {
                        if (Function_list.GetCover(File_s) != null) { 播放栏专辑.Source = Function_list.GetCover(File_s); }
                        else { 播放栏专辑.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/图标.png")); }
                    }

                    进度条.IsEnabled = true;//进度条可用
                    播放.Visibility = Visibility.Collapsed;
                    暂停.Visibility = Visibility.Visible;
                    播放器.Source = new Uri(File_s, UriKind.Relative);//定位视频文件
                    if (Geci == 0)//判断是否要播放歌词
                    {//需要播放
                        try
                        {
                            歌词滚动显示.SelectedIndex = 0;
                            ((ListBoxItem)歌词滚动显示.SelectedItem).Foreground = new SolidColorBrush(color);//将当前一句颜色该为红色
                            歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex]);
                            t1.Start();//歌词滚动
                            if (Get != null)//如果启用了桌面歌词
                            {
                                Get.主.Content = 播放歌曲名称.Content;
                                Get.父.Content = Singer_collection;
                            }
                        }
                        catch { }
                    }
                    t2.Start();//时间刷新
                    播放器.Play();//开始播放
                }
            }
            catch (Exception ex)
            {
                Function_list.Error_capture("播放歌曲时发生异常，异常详细信息:\n" + ex.ToString(), Tips, color3);
            }
        }
        private void Program_background(string str)
        {
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);//完成后返回
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);//建立后台
                bw.RunWorkerAsync(str);//开始执行
                加载中.Visibility = Visibility.Visible;
            }
        }
        //用于判断播放条件
        [Obsolete]
        private void Playing_conditions()
        {
            if (current_state != true)
            {
                if (底部列表.SelectedIndex == -1) { 底部列表.SelectedIndex = 0; }//如果用户没有选择播放模式默认列表循环
                Kill();
                if (底部列表.SelectedIndex == 0)//列表循环
                {
                    if (网络搜索结果.SelectedIndex + 1 == 网络搜索结果.Items.Count)//判断是否到列表底部
                    {
                        Program_background(Web_search_results[0].ToString());
                        网络搜索结果.SelectedIndex = 0;//定位
                    }
                    else
                    {
                        Program_background(Web_search_results[网络搜索结果.SelectedIndex + 1].ToString());
                        网络搜索结果.SelectedIndex += 1;//定位
                    }
                }
                else if (底部列表.SelectedIndex == 1)//单曲循环 
                {
                    Program_background(Web_search_results[网络搜索结果.SelectedIndex].ToString());
                }
                else if (底部列表.SelectedIndex == 2)//随机播放
                {
                    if (主页的播放列表.Items.Count == 1)
                    {
                        Program_background(Web_search_results[0].ToString());
                        网络搜索结果.SelectedIndex = 0;//定位
                    }
                    else
                    {
                        int stc = 0;//循环条件
                        while (stc == 0)//循环
                        {
                            Random rd = new Random();//定义随机数
                            int i = rd.Next(网络搜索结果.Items.Count - 1);//随机范围在列表最大-1
                            if (网络搜索结果.SelectedIndex != i)//排除当前正在播放的
                            {
                                Program_background(Web_search_results[i].ToString());
                                网络搜索结果.SelectedIndex = i;//定位
                                stc = 1;//条件排除
                            }
                        }
                    }

                }
                else if (底部列表.SelectedIndex == 3)//顺序播放
                {
                    if (网络搜索结果.SelectedIndex + 1 != 网络搜索结果.Items.Count)
                    {
                        Program_background(Web_search_results[网络搜索结果.SelectedIndex + 1].ToString());
                        网络搜索结果.SelectedIndex += 1;
                    }
                }
            }
            else
            {
                if (底部列表.SelectedIndex == -1) { 底部列表.SelectedIndex = 0; }//如果用户没有选择播放模式默认列表循环
                Kill();
                if (底部列表.SelectedIndex == 0)//列表循环
                {
                    if (主页的播放列表.SelectedIndex + 1 == 主页的播放列表.Items.Count)//判断是否到列表底部
                    {
                        Play_Misic(Number[0].ToString());//从列表第一个开始播放
                        主页的播放列表.SelectedIndex = 0;//定位
                    }
                    else
                    {
                        Play_Misic(Number[主页的播放列表.SelectedIndex + 1].ToString());//播放下一个
                        主页的播放列表.SelectedIndex += 1;//定位
                    }
                }
                else if (底部列表.SelectedIndex == 1)//单曲循环
                {
                    Play_Misic(Number[主页的播放列表.SelectedIndex].ToString());//播放当前选中项
                }
                else if (底部列表.SelectedIndex == 2)//随机播放
                {
                    if (主页的播放列表 .Items .Count == 1)
                    {
                        Play_Misic(Number[0].ToString());//播放歌曲
                        主页的播放列表.SelectedIndex = 0;//定位
                    }
                    else
                    {
                        int stc = 0;//循环条件
                        while (stc == 0)//循环
                        {
                            Random rd = new Random();//定义随机数
                            int i = rd.Next(主页的播放列表.Items.Count - 1);//随机范围在列表最大-1
                            if (主页的播放列表.SelectedIndex != i)//排除当前正在播放的
                            {
                                Play_Misic(Number[i].ToString());//播放歌曲
                                主页的播放列表.SelectedIndex = i;//定位
                                stc = 1;//条件排除
                            }
                        }
                    }
                }
                else if (底部列表.SelectedIndex == 3)//顺序播放
                {
                    if (主页的播放列表.SelectedIndex + 1 != 主页的播放列表.Items.Count)
                    {
                        Play_Misic(Number[主页的播放列表.SelectedIndex + 1].ToString());
                        主页的播放列表.SelectedIndex += 1;
                    }
                }
            }
        }
        //初始化播放，为下一次播放做准备
        private void Kill()
        {
            t1.Close();//释放计时器占用的资源
            t2.Close();
            lrc_time.Clear();//清空歌词数组
            lrc_lyrics.Clear();
            //重置专辑图片
            播放栏专辑.Source = null;
            播放器.Source = null;
            播放栏专辑.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/图标.png"));
            歌词滚动显示.Items.Clear();//清空滚动歌词列表
            歌曲名称.Content = "请选择要播放的歌曲...";//清空歌曲名称
            播放时间.Content = "00:00-00:00";//初始化播放时间
            歌手专辑2.Content = "";
            歌手专辑.Content = "";
            播放歌曲名称.Content = 歌曲名称.Content;
            上一曲.Visibility = Visibility.Visible;
            下一曲.Visibility = Visibility.Visible;
            播放.Visibility = Visibility.Visible;
            暂停.Visibility = Visibility.Collapsed;//显示播放按钮
            播放器.Close();//关闭媒体
            进度条.Value = 0;
            进度条.IsEnabled = false;//进度条状态不可用
            Function_list.FlushMemory();//回收内存
            if (Get != null)
            {
                Get.主.Content = "请选择要播放的歌曲";
                Get.父.Content = "§(*￣▽￣*)§";
            }
        }

        //更新时间，判断播放状态
        [Obsolete]
        private void theout2(object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(delegate
                 {
                     if (进度条.Value == 进度条.Maximum && button == 0)//判断进度条是否到底
                     {//如果到底
                         Kill();//初始化播放器
                         if (Temp5 == 0) { Playing_conditions(); }
                     }
                     else
                     {//如果没有到底，继续更新时间
                      //刷新播放时间
                         if (button == 0)
                         {//刷新时间
                             播放时间.Content = Convert.ToString(播放器.Position).Substring(3, 5) + "-" + Song_time;
                             进度条.Value = 播放器.Position.TotalSeconds;//定位进度条
                         }
                     }
                 }));
        }

        Color color = (Color)ColorConverter.ConvertFromString("#F8BF2424");//红色
        Color color2 = (Color)ColorConverter.ConvertFromString("#FF000000");//黑色
        private void theout1(object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(//同步线程
                  new Action(
                delegate
                {
                    try
                    {
                        for (int i = 0; i < lrc_time.Count; i++)
                        {
                            if (lrc_time[i].ToString().Substring(0, 7) == Convert.ToString(播放器.Position).Substring(3, 7) && lyrics_display == true)
                            {
                                歌词滚动显示.SelectedIndex = i;
                                ((ListBoxItem)歌词滚动显示.Items[i]).Foreground = new SolidColorBrush(color);//将当前一句颜色该为红色
                                if (Rolling_condition == false)
                                {
                                    歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex + 5]);//让歌词显示在中间
                                }

                                for (int j = 0; j < 歌词滚动显示.SelectedIndex; j ++)//上面歌词改颜色为黑色
                                {
                                    ((ListBoxItem)歌词滚动显示.Items[j]).Foreground = new SolidColorBrush(color2);
                                }

                                for (int o = 歌词滚动显示.Items.Count - 5; o > 歌词滚动显示.SelectedIndex; o --)//下面歌词修改为黑色
                                {
                                    ((ListBoxItem)歌词滚动显示.Items[o]).Foreground = new SolidColorBrush(color2);
                                }
                                if (Get != null)//如果启用了桌面歌词
                                {
                                    Get.主.Content = lrc_lyrics[i];
                                    Get.父.Content = lrc_lyrics[i + 1];
                                }
                            }
                        }
                    }
                    catch { }
                }));
        }
        /// <summary>
        /// 存储歌单列表，格式为绝对路径
        /// </summary>
        ArrayList Number = new ArrayList();
        /// <summary>
        /// 存储歌词分离出的时间
        /// </summary>
        ArrayList lrc_time = new ArrayList();
        /// <summary>
        /// 存储与时间对应的歌词
        /// </summary>
        ArrayList lrc_lyrics = new ArrayList();
        /// <summary>
        /// 用于存储网络搜索结果
        /// </summary>
        ArrayList Web_search_results = new ArrayList();
        /// <summary>
        /// 判断是本地播放还是在线播放，true为本地，false为在线
        /// </summary>
        bool current_state = true;
        /// <summary>
        /// 默认歌曲缓存目录
        /// </summary>
        string Web_file = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\方糖音乐\";
        /// <summary>
        /// 是否要显示滚动格式，true为显示，false为不显示
        /// </summary>
        bool lyrics_display;
        /// <summary>
        /// 用于指示本地导入的歌曲数量
        /// </summary>
        int Number_of_songs = 0;
        /// <summary>
        /// 用于指示是否用户鼠标停留在了滚动歌词上，true为停留在上，false为木有停留
        /// </summary>
        bool Rolling_condition = false;
        /// <summary>
        /// 实例化Timer类用于刷新歌词
        /// </summary>
        System.Timers.Timer t1 = new System.Timers.Timer(50);
        /// <summary>
        /// 实例化Timer类用于更新播放进度和时间
        /// </summary>
        System.Timers.Timer t2 = new System.Timers.Timer(100);
        /// <summary>
        /// 用于指示默认播放音乐接口供应商
        /// </summary>
        Meting Search_interface;//音乐接口api

        桌面歌词 Get;
        弹窗提示 Tips;
        [Obsolete]
        public MainWindow()
        {
            InitializeComponent();

            //以下代码用作校验程序签名是否完整，调试时请将其注释

            //if (Function_list.Document_verification() != true)
            //{
            //    Tips = new 弹窗提示(3, color3, 0, "程序签名校验失败，可能程序被恶意软件非法篡改，轻击以退出程序");
            //    Tips.ShowDialog();
            //    Environment.Exit(0);
            //}

            //到此结束

            联网播放.Visibility = Visibility.Collapsed;//隐藏联网播放的listbox
            try
            {
                if (Properties.Settings.Default.网络歌曲缓存目录 != "系统音乐目录")//判断用户是否设置了网络缓存目录
                {
                    //如果用户创建了，先判断目录是否存在
                    if (false == System.IO.Directory.Exists(Properties.Settings.Default.网络歌曲缓存目录 + @"\"))
                    {
                        //如果不在，则创建临时目录
                        System.IO.Directory.CreateDirectory(Properties.Settings.Default.网络歌曲缓存目录 + @"\");
                    }
                    //默认目录替换为用户设定的目录
                    Web_file = Properties.Settings.Default.网络歌曲缓存目录 + @"\";
                }
                else
                {
                    //如果用户木有设置缓存目录，先判断目录是否存在
                    if (false == System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\方糖音乐\"))
                    {
                        //如果不在，则创建临时目录
                        System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\方糖音乐\");
                    }
                    //默认状态目录已经在加载时被赋值
                }
                //设置滚动歌词缩放模式，有助于提高性能
                RenderOptions.SetBitmapScalingMode(歌词滚动显示, BitmapScalingMode.NearestNeighbor);

                //初始化计时器
                t1.Elapsed += new System.Timers.ElapsedEventHandler(theout1);//到达时间的时候执行事件
                t1.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
                t1.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件

                t2.Elapsed += new System.Timers.ElapsedEventHandler(theout2);//到达时间的时候执行事件
                t2.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
                t2.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件

                //初始化网络接口参数
                if (Properties.Settings.Default.网络接口参数 == "腾讯")
                {
                    Search_interface = new Meting(ServerProvider.Tencent);
                }
                else if (Properties.Settings.Default.网络接口参数 == "网易")
                {
                    Search_interface = new Meting(ServerProvider.Netease);
                }
                else if (Properties.Settings.Default.网络接口参数 == "酷狗")
                {
                    Search_interface = new Meting(ServerProvider.Kugou);
                }
                //是否显示歌词
                if (Properties.Settings.Default.是否歌词显示 == true)
                {
                    lyrics_display = true;
                    歌词滚动显示.Visibility = Visibility.Visible;
                    播放歌曲名称.Visibility = Visibility.Visible;
                    歌手专辑.Visibility = Visibility.Visible;
                    歌手专辑2.Visibility = Visibility.Visible;
                }
                else
                {
                    lyrics_display = false;
                    歌词滚动显示.Visibility = Visibility.Collapsed;
                    播放歌曲名称.Visibility = Visibility.Collapsed;
                    歌手专辑.Visibility = Visibility.Collapsed;
                    歌手专辑2.Visibility = Visibility.Collapsed;
                }
                //是否要显示专辑
                if (Properties.Settings.Default.显示专辑 == true)
                {
                    大专辑.Visibility = Visibility.Visible;
                }
                else
                {
                    大专辑.Visibility = Visibility.Collapsed;
                }
                //加载背景
                if (Properties.Settings.Default.背景图片 != "默认" && System.IO.File.Exists(Properties.Settings.Default.背景图片))
                {
                    背景.Source = new BitmapImage(new Uri(Properties.Settings.Default.背景图片));//加载背景图片
                }
                else
                {
                    背景.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/默认背景.jpg"));//加载背景图片
                }
                if (Properties.Settings.Default.背景填充 == 1) { Zoom_mode(1); }
                else if (Properties.Settings.Default.背景填充 == 2) { Zoom_mode(2); }
                else if (Properties.Settings.Default.背景填充 == 3) { Zoom_mode(3); }
                else if (Properties.Settings.Default.背景填充 == 4) { Zoom_mode(4); }

                //加载播放设置
                底部列表.SelectedIndex = Properties.Settings.Default.播放设置;
                //添加歌单
                if (Properties.Settings.Default.歌单路径 != "无")
                {
                    if (Directory.Exists(Properties.Settings.Default.歌单路径))
                    {
                        if (Properties.Settings.Default.歌单路径参数 == 1)//单文件夹
                        {
                            Function_list.query(主页的播放列表, Properties.Settings.Default.歌单路径, Number, Tips, color3);
                        }
                        else if (Properties.Settings.Default.歌单路径参数 == 2)//全盘
                        {
                            Function_list.ListFiles(主页的播放列表, new DirectoryInfo(Properties.Settings.Default.歌单路径), Number, Tips, color3);
                        }
                        歌曲数量.Content = "共" + Number.Count + "首歌曲";
                    }
                }
                //加载颜色
                string a = Properties.Settings.Default.主题颜色;
                if (a == "System.Windows.Controls.ComboBoxItem: 宝石绿") { 主题颜色.SelectedIndex = 0; }
                else if (a == "System.Windows.Controls.ComboBoxItem: 桃花粉") { 主题颜色.SelectedIndex = 1; }
                else if (a == "System.Windows.Controls.ComboBoxItem: 旬子蓝") { 主题颜色.SelectedIndex = 2; }
                else if (a == "System.Windows.Controls.ComboBoxItem: 山茶红") { 主题颜色.SelectedIndex = 3; }

                if (Properties.Settings.Default.桌面歌词 == true)
                {
                    Get = new 桌面歌词(color3);
                    Get.fcc1 += Playback_status;
                    Get.Show();
                }
                播放栏专辑.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/图标.png"));//从资源文件加载图片
                音量条.Value = Properties.Settings.Default.音量;
                播放器.Volume = Properties.Settings.Default.音量 / 100;
                背景模糊.Radius = Properties.Settings.Default.背景模糊程度;
            }
            catch (Exception ex)
            {
                Function_list.Error_capture("程序初始化异常：\n" + ex.ToString(), Tips, color3);
                Tips = new 弹窗提示(3, color3, 0, "由于程序初始化遇到问题，已经将程序部分设置恢复默认值,稍后您可以重新启动");
                Tips.ShowDialog();
                Properties.Settings.Default.背景图片 = "默认";
                Properties.Settings.Default.音量 = 100;
                Properties.Settings.Default.播放设置 = 0;
                Properties.Settings.Default.独立播放视频 = true;
                Properties.Settings.Default.桌面歌词 = false;
                Properties.Settings.Default.错误报告 = true;
                Properties.Settings.Default.网络接口参数 = "腾讯";
                Properties.Settings.Default.Save();
                Environment.Exit(0); 
            }
        }
        //播放动画函数
        Storyboard story;
        private void 动画播放(string a)
        {
            story = (Storyboard)FindResource(a);
            BeginStoryboard(story);
        }
        //标题栏拖动窗体
        private void ColorZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        //关闭程序事件处理
        private void 关闭程序_Click(object sender, RoutedEventArgs e)
        {
            播放器.Stop();
            Properties.Settings.Default.主题颜色 = 主题颜色.SelectedValue.ToString();
            Properties.Settings.Default.播放设置 = 底部列表.SelectedIndex;
            Properties.Settings.Default.音量 = 音量条.Value;
            Properties.Settings.Default.Save();//保存设置
            using (BackgroundWorker bw = new BackgroundWorker())//建立后台进程
            {
                bw.DoWork += new DoWorkEventHandler(Exit_from);//后台事件
                bw.RunWorkerAsync("Tank");//开始执行
            }
        }

        [Obsolete]
        void Exit_from(object sender, DoWorkEventArgs e)
        {
            //DeleteDir(System.IO.Path.GetTempPath() + @"方糖音乐");//删除临时目录
            Environment.Exit(0);
        }
        //最小化事件处理
        private void 最小化_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        //前端显示开关事件处理
        int Temp = 0;
        private void 前端显示_Click(object sender, RoutedEventArgs e)
        {
            if (Temp == 0)
            {
                Topmost = true;
                Temp = 1;
            }
            else if (Temp == 1)
            {
                Topmost = false;
                Temp = 0;
            }
        }
        //音量调节事件处理
        int Temp2 = 0;
        private void ListBoxItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Temp2 == 0)
            {
                动画播放("音量条打开");
                Temp2 = 1;
            }
            else if (Temp2 == 1)
            {
                动画播放("音量条关闭");
                Temp2 = 0;
            }
        }
        //当鼠标移除音量调节块范围时事件处理
        private void 音量调节1_MouseLeave(object sender, MouseEventArgs e)
        {
            动画播放("音量条关闭");
            Temp2 = 0;
        }
        //调节音量大小事件处理
        int Temp3 = 0;
        private void 音量条_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Temp3 == 1)
            {
                音量显示.Content = (int)音量条.Value + "%";
                播放器.Volume = 音量条.Value / 100;
            }
            Temp3 = 1;
        }


        //右菜单打开事件处理
        private void 右菜单_MouseUp(object sender, MouseButtonEventArgs e)
        {
            动画播放("黑幕动画开");
            动画播放("右菜单弹出");
        }
        //当打开右菜单，用户单击黑幕可以关闭菜单
        private void 黑幕_MouseUp(object sender, MouseButtonEventArgs e)
        {
            动画播放("黑幕动画关");
            动画播放("右菜单关闭");
        }
        //当打开右菜单时，单击按钮可以关闭右菜单
        private void 关闭右菜单1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            动画播放("黑幕动画关");
            动画播放("右菜单关闭");
        }
        //当鼠标悬浮在歌词上时，歌词不会自动滚动
        private void 歌词滚动显示_MouseEnter(object sender, MouseEventArgs e)
        {
            Rolling_condition = true;
        }
        //当鼠标离开歌词时，开始重新定位，开始滚动
        private void 歌词滚动显示_MouseLeave(object sender, MouseEventArgs e)
        {
            Rolling_condition = false;
            try
            {
                if (歌词滚动显示.SelectedIndex <= 5)
                {
                    歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex]);
                }
                else
                {
                    歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex - 4]);//让歌词显示在中间
                }
            }
            catch { }
        }
        //单击歌词定位进度
        private void 歌词滚动显示_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (歌词滚动显示.SelectedIndex != -1)
            {
                try
                {
                    int Xhao = 歌词滚动显示.SelectedIndex;
                    int a1 = int.Parse(lrc_time[Xhao].ToString().Substring(0, 2)) * 60 + int.Parse(lrc_time[Xhao].ToString().Substring(3, 2));
                    播放器.Position = TimeSpan.FromSeconds(a1);
                    歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex + 5]);//让歌词显示在中间
                }
                catch { }
            }
        }

        [Obsolete]
        private void 大专辑_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (播放器.Source != null)
            {
                Tips = new 弹窗提示(2, color3, 0, 播放器.Source.ToString());
                Tips.ShowDialog();
            }
        }

        //在左键按下的时候不会刷新进度条
        private int button = 0;//，在用户拖动进度条时，判断是否要刷新进度条
        private void 进度条_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)//左键按下
            {
                button = 1;
            }
        }
        //当用户拖动进度条时
        private void 进度条_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ((ListBoxItem)歌词滚动显示.SelectedItem).Foreground = new SolidColorBrush(color2);//将当前一句颜色该为黑色
                歌词滚动显示.SelectedIndex = 0;
                歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex]);
            }
            catch { }
            播放器.Position = TimeSpan.FromSeconds(进度条.Value);//调整进度
            button = 0;
        }
        //单击播放列表时定位歌曲
        [Obsolete]
        private void 主页的播放列表_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (主页的播放列表.SelectedIndex != -1)
            {
                Temp5 = 0;
                Kill();//初始化播放器
                Play_Misic(Number[主页的播放列表.SelectedIndex].ToString());//播放歌曲
            }
        }

        //[Obsolete]
        //private void 播放列队_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (播放列队.SelectedIndex != -1)
        //    {
        //        Temp5 = 0;
        //        Kill();//初始化播放器
        //        Play_Misic(Number[播放列队.SelectedIndex].ToString());//播放歌曲
        //        主页的播放列表.SelectedIndex = 播放列队.SelectedIndex;
        //        黑幕_MouseUp(null, null);
        //    }
        //}
        //单击播放按钮

        [Obsolete]
        private void 播放_Click(object sender, RoutedEventArgs e)
        {
            if (进度条.IsEnabled == true)
            {
                if (暂停.Visibility == Visibility.Collapsed && 进度条.IsEnabled == true)
                {
                    播放.Visibility = Visibility.Collapsed;
                    暂停.Visibility = Visibility.Visible;
                    播放器.Play();
                    t1.Start();
                    t2.Start();
                    App.播放状态 = 0;
                }
            }
            else
            {
                Play_Misic(Number[0].ToString());//播放歌曲
            }
        }
        //单击暂停按钮
        private void 暂停_Click(object sender, RoutedEventArgs e)
        {
            if (播放.Visibility == Visibility.Collapsed)
            {
                暂停.Visibility = Visibility.Collapsed;
                播放.Visibility = Visibility.Visible;
                播放器.Pause();
                t1.Stop();
                t2.Stop();
                App.播放状态 = 1;
            }
        }
        //用于后台播放歌曲，预加载
        private void backstage(int sum)
        {
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                load = true;//锁定防止连续单击
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);//完成后返回
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);//建立后台
                bw.RunWorkerAsync(Web_search_results[sum].ToString());//开始执行
            }
        }

        [Obsolete]
        private void 选择判断(int r)
        {
            if (current_state == true)
            {
                Kill();
                if (r == 1)
                {
                    if (主页的播放列表.SelectedIndex == 0)//从最底部播放
                    {
                        Play_Misic(Number[主页的播放列表.Items.Count - 1].ToString());
                        主页的播放列表.SelectedIndex = 主页的播放列表.Items.Count - 1;
                    }
                    else//正常上一曲
                    {
                        Play_Misic(Number[主页的播放列表.SelectedIndex - 1].ToString());
                        主页的播放列表.SelectedIndex -= 1;
                    }
                }
                else if (r == 2)
                {

                    if (主页的播放列表.SelectedIndex + 1 == 主页的播放列表.Items.Count)
                    {
                        Play_Misic(Number[0].ToString());
                        主页的播放列表.SelectedIndex = 0;
                    }
                    else
                    {
                        Play_Misic(Number[主页的播放列表.SelectedIndex + 1].ToString());
                        主页的播放列表.SelectedIndex += 1;
                    }
                }
            }
            else
            {
                if (r == 1)
                {
                    if (网络搜索结果.SelectedIndex == 0)
                    {
                        backstage(网络搜索结果.Items.Count - 1);
                        网络搜索结果.SelectedIndex = 网络搜索结果.Items.Count - 1;
                    }
                    else
                    {
                        backstage(网络搜索结果.SelectedIndex - 1);
                        网络搜索结果.SelectedIndex -= 1;
                    }
                }
                else if (r == 2)
                {
                    if (主页的播放列表.SelectedIndex + 1 == 网络搜索结果.Items.Count)
                    {
                        backstage(0);
                        网络搜索结果.SelectedIndex = 0;
                    }
                    else
                    {
                        backstage(网络搜索结果.SelectedIndex + 1);
                        网络搜索结果.SelectedIndex += 1;
                    }
                }
            }

        }
        //单击下一首，如果在最底下就转到最上面
        [Obsolete]
        private void 下一曲_Click(object sender, RoutedEventArgs e)
        {
            if (current_state == true)
            {
                if (底部列表.SelectedIndex != -1)
                {
                    if (底部列表.SelectedIndex == 0 || 底部列表.SelectedIndex == 1 || 底部列表.SelectedIndex == 3) { 选择判断(2); }
                    else if (底部列表.SelectedIndex == 2) { Playing_conditions(); }
                }
                else
                {
                    if (主页的播放列表.Items.Count != 0)
                    {
                        底部列表.SelectedIndex = 0;
                        下一曲_Click(null, null);
                    }
                    else
                    {
                        Tips = new 弹窗提示(3, color3, 0, "歌木有任何歌曲（；´д｀）ゞ");
                        Tips.ShowDialog();
                    }
                }
            }
            else
            {
                if (load == false)
                {
                    if (网络搜索结果.SelectedIndex != -1)
                    {
                        if (底部列表.SelectedIndex == 0 || 底部列表.SelectedIndex == 1 || 底部列表.SelectedIndex == 3) { 选择判断(2); }
                        else if (底部列表.SelectedIndex == 2) { Playing_conditions(); }
                    }
                    else
                    {
                        if (网络搜索结果.Items.Count != 0)
                        {
                            网络搜索结果.SelectedIndex = 0;
                            下一曲_Click(null, null);
                        }
                        else
                        {
                            Tips = new 弹窗提示(3, color3, 0, "歌木有任何歌曲（；´д｀）ゞ");
                            Tips.ShowDialog();
                        }
                    }
                }
            }
        }
        //单击上一曲,如果在底部就转到最底部
        [Obsolete]
        private void 上一曲_Click(object sender, RoutedEventArgs e)
        {
            if (current_state == true)
            {
                if (主页的播放列表.SelectedIndex != -1)
                {
                    if (底部列表.SelectedIndex != -1)
                    {
                        if (底部列表.SelectedIndex == 0 || 底部列表.SelectedIndex == 1 || 底部列表.SelectedIndex == 3) { 选择判断(1); }
                        else if (底部列表.SelectedIndex == 2) { Playing_conditions(); }
                    }
                    else
                    {
                        if (主页的播放列表.Items.Count != 0)
                        {
                            底部列表.SelectedIndex = 0;
                            上一曲_Click(null, null);
                        }
                        else
                        {
                            Tips = new 弹窗提示(3, color3, 0, "歌木有任何歌曲（；´д｀）ゞ");
                            Tips.ShowDialog();
                        }
                    }
                }
            }
            else
            {
                if (load == false)
                {
                    if (网络搜索结果.SelectedIndex != -1)
                    {
                        if (底部列表.SelectedIndex != -1)
                        {
                            if (底部列表.SelectedIndex == 0 || 底部列表.SelectedIndex == 1 || 底部列表.SelectedIndex == 3) { 选择判断(1); }
                            else if (底部列表.SelectedIndex == 2) { Playing_conditions(); }
                        }
                        else
                        {
                            if (网络搜索结果.Items.Count != 0)
                            {
                                底部列表.SelectedIndex = 0;
                                上一曲_Click(null, null);
                            }
                            else
                            {
                                Tips = new 弹窗提示(3, color3, 0, "歌木有任何歌曲（；´д｀）ゞ");
                                Tips.ShowDialog();
                            }
                        }
                    }
                }
            }
        }

        private void listBoxItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(播放器.Volume.ToString());
            if (current_state == true)
            {
                动画播放("切换动画开");
                切换.ToolTip = "本地播放";
                current_state = false;
            }
            else
            {
                动画播放("切换动画关");
                切换.ToolTip = "在线播放";
                current_state = true;
            }
        }

        private void 网络搜索结果_MouseEnter(object sender, MouseEventArgs e)
        {
            动画播放("滚动条打开1");
        }

        private void 网络搜索结果_MouseLeave(object sender, MouseEventArgs e)
        {
            动画播放("滚动条关闭1");
        }

        private void 大专辑_MouseEnter(object sender, MouseEventArgs e)
        {
            //动画播放("专辑放大");
        }

        private void 大专辑_MouseLeave(object sender, MouseEventArgs e)
        {
            //动画播放("专辑缩小");
        }

        private void window窗体_Activated(object sender, EventArgs e)
        {
            Function_list.FlushMemory();
        }
        //播放视频函数
        private void Play_video(string file)
        {
            string[] Info = new string[7];
            Shell32.Shell sh = new Shell();
            Folder dir = sh.NameSpace(System.IO.Path.GetDirectoryName(file));
            FolderItem item = dir.ParseName(System.IO.Path.GetFileName(file));
            Info[3] = dir.GetDetailsOf(item, 27);  // 获取歌曲时长
            播放器.Source = new Uri(file, UriKind.Relative);//定位视频文件
            //记录最大值
            Song_time = Info[3].Substring(3, 5);
            int s1 = int.Parse(Info[3].Substring(3, 2)) * 60;
            //将转化后的秒加上取的秒
            int s2 = int.Parse(Info[3].Substring(6, 2)) + s1;
            //设定进度条最大值
            进度条.Maximum = s2;
            进度条.IsEnabled = true;
            歌曲名称.Content = System.IO.Path.GetFileNameWithoutExtension(file);
            播放.Visibility = Visibility.Collapsed;
            暂停.Visibility = Visibility.Visible;
            上一曲.Visibility = Visibility.Collapsed;
            下一曲.Visibility = Visibility.Collapsed;
            播放器.Play();
            t2.Start();
        }
        //单击打开音乐文件
        int Temp5 = 0;
        [Obsolete]
        private void 打开音乐_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog Open1 = new OpenFileDialog();
            Open1.Filter = "音乐文件(*.mp3,*.flac,*.wav)|*.mp3;*.flac;*.wav";
            if (Open1.ShowDialog(this) == true)
            {
                主页的播放列表.SelectedIndex = -1;
                Kill();
                Temp5 = 1;
                Play_Misic(Open1.FileName);
            }
        }
        //单击打开视频文件
        [Obsolete]
        private void 打开视频_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog Open1 = new OpenFileDialog();
            Open1.Filter = "视频文件(*.mp4,*.avi,*.wmv)|*.mp4;*.avi;*.wmv";
            if (Open1.ShowDialog(this) == true)
            {
                if (Properties.Settings.Default.独立播放视频 == true)
                {
                    主页的播放列表.SelectedIndex = -1;
                    暂停_Click(null, null);
                    播放窗口 child = new 播放窗口(color3, Open1.FileName);
                    child.Show();
                }
                else
                {
                    主页的播放列表.SelectedIndex = -1;
                    Kill();
                    Temp5 = 1;
                    Play_video(Open1.FileName);
                }
            }
        }

        //添加文件夹单文件夹
        [Obsolete]
        private void 添加歌曲_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                if (dialog.FileName != "") { Song_scanning(2, dialog.FileName); }
            }
            catch { }
        }

        //全盘扫描歌曲
        [Obsolete]
        private void 扫描歌曲_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                if (dialog.FileName != "") { Song_scanning(1, dialog.FileName); }
            }
            catch { }
        }

        private void 进度条_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (button == 1)
            {
                播放时间.Content = Function_list.sec_to_hms(进度条.Value).Substring(3, 5) + "-" + Song_time;
            }
        }
        //调整主题颜色
        public Color color3;
        private void 主题颜色_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string a = 主题颜色.SelectedValue.ToString();
            if (a == "System.Windows.Controls.ComboBoxItem: 宝石绿")
            {
                color3 = (Color)ColorConverter.ConvertFromString("#FF2CCFA0");
                colorZone.Background = new SolidColorBrush(color3);
            }
            else if (a == "System.Windows.Controls.ComboBoxItem: 桃花粉")
            {
                color3 = (Color)ColorConverter.ConvertFromString("#FFFF9999");
                colorZone.Background = new SolidColorBrush(color3);
            }
            else if (a == "System.Windows.Controls.ComboBoxItem: 旬子蓝")
            {
                color3 = (Color)ColorConverter.ConvertFromString("#FF10AEC2");
                colorZone.Background = new SolidColorBrush(color3);
            }
            else if (a == "System.Windows.Controls.ComboBoxItem: 山茶红")
            {
                color3 = (Color)ColorConverter.ConvertFromString("#FFED556A");
                colorZone.Background = new SolidColorBrush(color3);
            }
            if (Get != null) { Get.背景.Fill = new SolidColorBrush(color3); }//判断设置界面是否被打开
        }


        private void 搜索框_MouseLeave(object sender, MouseEventArgs e)
        {
            if (搜索框关闭 == 1)
            {
                动画播放("搜索框关闭");
                sub = 0;
                搜索.Text = "";
                搜索框关闭 = 0;
            }
        }

        [Obsolete]
        private void 搜索框_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (搜索框.SelectedIndex != -1 && ID.Count != 0)
            {
                Kill();//清除播放器状态
                for (int i = 0; i < Number.Count; i++)
                {
                    if (Number[i].ToString() == ID[搜索框.SelectedIndex].ToString())//定位文件
                    {
                        主页的播放列表.SelectedIndex = i;
                    }
                }
                Play_Misic(ID[搜索框.SelectedIndex].ToString());//开始播放
                ID.Clear();
            }
        }
        //搜索框，可以搜索歌曲
        int sub = 0;
        ArrayList ID = new ArrayList();//定义数组存储搜索结果
        int 搜索框关闭;
        private void 搜索_KeyUp(object sender, KeyEventArgs e)
        {
            搜索框关闭 = 1;
            搜索框.Items.Clear();//清空内容
            ID.Clear();//清空数组
            for (int i = 0; i < Number.Count; i++)//循环最大长度为歌单列表的最大长度
            {
                string s = System.IO.Path.GetFileNameWithoutExtension(Number[i].ToString());//从路径取文件名称
                string v = 搜索.Text;//获取输入的内容
                if (s != null && 搜索.Text != "")//判断值是否为空
                {
                    if (s.Contains(v))//用Contains函数判断用户输入的数值是否包含
                    {
                        ID.Add(Number[i].ToString());//记录文件路径
                        //向列表框中添加搜索结果
                        Function_list.填充菜单(搜索框, System.IO.Path.GetFileNameWithoutExtension(Number[i].ToString()), System.IO.Path.GetFileNameWithoutExtension(Number[i].ToString()), 315, 40, 15, false);
                    }
                }
            }
            if (ID.Count == 0 && 搜索.Text != "")
            {
                Function_list.填充菜单(搜索框, "什么也木有找到w(ﾟДﾟ)w", null, 315, 40, 15, false);
            }
            if (sub == 0 && 搜索.Text != "")//打开列表框
            {
                动画播放("搜索框打开");
                sub = 1;
            }
            if (搜索.Text == "" && sub == 1)
            {
                动画播放("搜索框关闭");
                sub = 0;
            }
        }
        bool form = false;//设置窗口只可以打开一个
        [Obsolete]
        private void 程序设置_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (form == false)
            {
                程序设置 child = new 程序设置(color3);//实例化窗口

                //添加委托事件
                child.fcc1 += background;//对背景的更改
                child.fcc2 += Vague;//背景模糊度的更改
                child.fcc3 += Truefalse;//歌词的显示方式
                child.fcc4 += cancel;//取消，恢复之前设置
                child.fcc5 += form1;
                child.fcc6 += Zoom_mode;//背景填充方式
                zoom();//保存当前的背景图片对其方式
                child.Show();//启动窗口
                form = true;
            }
        }
        int zoom_temp = 0;

        private void zoom()
        {
            if (背景.Stretch == Stretch.None) { zoom_temp = 1; }
            if (背景.Stretch == Stretch.Fill) { zoom_temp = 2; }
            if (背景.Stretch == Stretch.Uniform) { zoom_temp = 3; }
            if (背景.Stretch == Stretch.UniformToFill) { zoom_temp = 4; }
        }

        //委托事件
        [Obsolete]
        private int background(string d)//修改背景图片
        {
            try
            {
                if (d == "默认") { 背景.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/默认背景.jpg")); }
                else { 背景.Source = new BitmapImage(new Uri(d)); }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Tips = new 弹窗提示(4, color3, 0, "问题详情:" + ex);
                Tips.ShowDialog();
                Tips = null;
            }

            return 0;
        }

        private int Vague(int num)//调节背景模糊度
        {
            背景模糊.Radius = num;
            return 0;
        }

        //private 桌面歌词 Get = new 桌面歌词();
        [Obsolete]
        private int Truefalse(int sum)//是否显示歌词//和平滑显示//桌面歌词
        {
            if (sum == 1)
            {
                歌词滚动显示.Visibility = Visibility.Collapsed;
                播放歌曲名称.Visibility = Visibility.Collapsed;
                歌手专辑.Visibility = Visibility.Collapsed;
                歌手专辑2.Visibility = Visibility.Collapsed;
                lyrics_display = false;
            }
            else if (sum == 2)
            {
                歌词滚动显示.Visibility = Visibility.Visible;
                播放歌曲名称.Visibility = Visibility.Visible;
                歌手专辑.Visibility = Visibility.Visible;
                歌手专辑2.Visibility = Visibility.Visible;
                lyrics_display = true;
            }
            else if (sum == 3)//开
            {
                //FluidMoveBehavior fluidMove = 歌词滚动显示.Template.FindName("x2", 歌词滚动显示) as FluidMoveBehavior;
                //_ = fluidMove.AppliesTo == FluidMoveScope.Children;
                //fluidMove.AppliesTo ==  
                //FluidMoveBehavior fluidMove = GetVisualChild<FluidMoveBehavior>(歌词滚动显示, v => v. );
            }
            else if (sum == 4)
            {
                //FluidMoveBehavior fluidMove = 歌词滚动显示.Template.FindName("x2", 歌词滚动显示) as FluidMoveBehavior;
                //_ = fluidMove.AppliesTo == FluidMoveScope.Self;
            }
            else if (sum == 5)//显示歌词
            {
                Get = new 桌面歌词(color3);
                Get.fcc1 += Playback_status;
                Get.Show();
            }
            else if (sum == 6)//不显示歌词
            {
                Get.Visibility = Visibility.Collapsed;
                Get = null;
            }
            else if (sum == 7)
            {
                大专辑.Visibility = Visibility.Collapsed;
            }
            else if (sum == 8)
            {
                大专辑.Visibility = Visibility.Visible;
            }
            return 0;
        }

        private void 主页的播放列表_MouseEnter(object sender, MouseEventArgs e)
        {
            动画播放("滚动条打开");
        }

        private void 主页的播放列表_MouseLeave(object sender, MouseEventArgs e)
        {
            动画播放("滚动条关闭");
        }

        private int cancel(int exit)//取消
        {
            if (Properties.Settings.Default.是否歌词显示 == true)
            {
                lyrics_display = true;
                歌词滚动显示.Visibility = Visibility.Visible;
            }
            else if (Properties.Settings.Default.是否歌词显示 == false)
            {
                lyrics_display = false;
                歌词滚动显示.Visibility = Visibility.Collapsed;
            }
            //加载背景
            if (Properties.Settings.Default.背景图片 != "默认" && System.IO.File.Exists(Properties.Settings.Default.背景图片))
            {
                背景.Source = new BitmapImage(new Uri(Properties.Settings.Default.背景图片));//加载背景图片
            }
            else
            {
                背景.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/默认背景.jpg"));//加载背景图片
                Properties.Settings.Default.背景图片 = "默认";
            }
            if (Properties.Settings.Default.桌面歌词 == true)
            {
                if (Get == null)
                {
                    Get = new 桌面歌词(color3);
                    Get.Show();
                }
            }
            else if (Get != null) { Get.Close(); }

            if (Properties.Settings.Default.显示专辑 == true)
            {
                大专辑.Visibility = Visibility.Visible;
            }
            else
            {
                大专辑.Visibility = Visibility.Collapsed;
            }

            背景模糊.Radius = Properties.Settings.Default.背景模糊程度;


            Zoom_mode(zoom_temp);
            return 0;
        }

        //快捷键播放暂停，上/下一曲
        [Obsolete]
        private void window窗体_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key.ToString() == "Space")//空格
            //{
            //    if (播放.Visibility == Visibility.Visible) { 播放_Click(null, null); }
            //    else { 暂停_Click(null, null); }
            //}
            //else if (e.Key.ToString() == "Left")//左
            //{
            //    if (上一曲.Visibility == Visibility.Visible) { 上一曲_Click(null, null); }
            //}
            //else if (e.Key.ToString() == "Right")//右
            //{
            //    if (下一曲.Visibility == Visibility.Visible) { 下一曲_Click(null, null); }
            //}
        }

        private int Zoom_mode(int a)
        {
            if      (a == 1) { 背景.Stretch = Stretch.None; }
            else if (a == 2) { 背景.Stretch = Stretch.Fill; }
            else if (a == 3) { 背景.Stretch = Stretch.Uniform; }
            else if (a == 4) { 背景.Stretch = Stretch.UniformToFill; }
            return 0;
        }

        private int form1(int a)
        {
            form = false;
            return 0;
        }

        private void 主界面菜单选择_MouseEnter(object sender, MouseEventArgs e)
        {
            主界面菜单选择.SelectedIndex = -1;
        }

        private void 进度条_MouseEnter(object sender, MouseEventArgs e)
        {
            t2.Interval = 1000;
        }

        private void 进度条_MouseLeave(object sender, MouseEventArgs e)
        {
            t2.Interval = 100;
        }

        private void 播放器_MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                App.播放状态 = 0;
                主页的播放列表.ScrollIntoView(主页的播放列表.Items[主页的播放列表.SelectedIndex]);
            }
            catch { }
        }

        [Obsolete]
        private void window窗体_Drop(object sender, DragEventArgs e)
        {
            string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (System.IO.Path.GetExtension(filePath[0]) != "")
            {
                if (System.IO.Path.GetExtension(filePath[0]) == ".mp3" || System.IO.Path.GetExtension(filePath[0]) == ".wav" || System.IO.Path.GetExtension(filePath[0]) == ".flac")
                {
                    主页的播放列表.SelectedIndex = -1;
                    Kill();
                    Temp5 = 1;
                    Play_Misic(filePath[0]);
                }
                else if (System.IO.Path.GetExtension(filePath[0]) == ".mp4" || System.IO.Path.GetExtension(filePath[0]) == ".wmv" || System.IO.Path.GetExtension(filePath[0]) == ".avi")
                {
                    if (Properties.Settings.Default.独立播放视频 == true)
                    {
                        主页的播放列表.SelectedIndex = -1;
                        暂停_Click(null, null);
                        播放窗口 child = new 播放窗口(color3, filePath[0]);
                        child.Show();
                    }
                    else
                    {
                        主页的播放列表.SelectedIndex = -1;
                        Kill();
                        Temp5 = 1;
                        Play_video(filePath[0]);
                    }
                }
            }
            else
            {
                弹窗提示 Tips = new 弹窗提示(0, color3, 0, null);
                Tips.fcc1 += Return_value;
                Tips.ShowDialog();
                if (Return_value1 == 1)//单文件夹
                {
                    Song_scanning(2, filePath[0]);
                }
                if (Return_value1 == 2)//全盘
                {
                    Song_scanning(1, filePath[0]);
                }
            }
        }

        int Return_value1 = 0;//返回参数
        private int Return_value(int a)
        {
            Return_value1 = a;
            return 0;
        }

        private void window窗体_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effects = DragDropEffects.Link;
            else e.Effects = DragDropEffects.None;
        }

        [Obsolete]
        private int Playback_status(int a)
        {
            if (a == 1) 
            { 
                播放_Click(null, null);
            }
            else if (a == 2) { 暂停_Click(null, null); }
            else if (a == 3) { 上一曲_Click(null, null); }
            else if (a == 4) { 下一曲_Click(null, null); }
            return 0;
        }

        private void 音量调节_MouseLeave(object sender, MouseEventArgs e)
        {
            音量调节.SelectedIndex = -1;
        }

        bool load = false;
        string jsonStr;
        private void 网络搜索_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return" && 网络搜索.Text != "" && load == false)
            {//开始搜索
                网络搜索结果.Items.Clear();//清空内容
                Web_search_results.Clear();//清空搜索结果
                加载中.Visibility = Visibility.Visible;
                load = true;
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted2);//完成后返回
                    bw.DoWork += new DoWorkEventHandler(bw_DoWork2);//建立后台
                    bw.RunWorkerAsync(网络搜索.Text);//开始执行
                }
            }
        }

        [Obsolete]
        void bw_DoWork2(object sender, DoWorkEventArgs e)//后台
        {
            try
            {
                //获得json数据
                jsonStr = Search_interface.FormatMethod(true).Search(e.Argument.ToString(), new Meting4Net.Core.Models.Standard.Options
                {
                    page = 1,//页数
                    limit = 30//输出30条数据
                });
            }
            catch
            {//错误报告，网络超时
                new Thread(() =>//异步调用
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,new Action(() =>
                        {
                            Tips = new 弹窗提示(3, color3, 0, "搜索失败，请检查网络后重试！");
                            Tips.ShowDialog();
                        }));
                }).Start();
            }
        }

        void bw_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)//更新控件
        {
            //Clipboard.SetText(jsonStr);
            //MessageBox.Show(jsonStr);
            //序列化数据
            bool temp = true;
            while (temp)
            {
                string temp2 = Function_list.Substring(jsonStr, "{", "}");
                if (temp2 == "") { temp = false; }
                if (temp2 != "")
                {//填充搜索结果
                    Web_search_results.Add(temp2);
                    string name = Function_list.Substring(Web_search_results[Web_search_results.Count - 1].ToString(), "\"name\":\"", "\"");
                    name += "   歌手:" + Function_list.Substring(Web_search_results[Web_search_results.Count - 1].ToString(), "[\"", "\"]").Replace("\"", "").Replace(",", "-");
                    Function_list.填充菜单(网络搜索结果, name, name, 315, 40, 16, false);
                    jsonStr = jsonStr.Replace("{" + temp2 + "}", "");
                }
            }
            load = false;
            加载中.Visibility = Visibility.Collapsed;
        }

        private void 网络搜索结果_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (网络搜索结果.SelectedIndex != -1 && load == false)
            {
                Program_background(Web_search_results[网络搜索结果.SelectedIndex].ToString());
            }
        }

        [Obsolete]
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)//更新控件
        {
            load = false;
            加载中.Visibility = Visibility.Collapsed;
            if (temp23 == true)
            {
                if (Song_path != null)
                {
                    Kill();
                    Play_Misic(Song_path);
                    歌曲名称.Content = Album_Singer[0] + "-" + Album_Singer[1];
                    播放歌曲名称.Content = Album_Singer[0];
                    歌手专辑.Content = "歌手：" + Album_Singer[1];
                    歌手专辑2.Content = "专辑：" + Album_Singer[2];
                }
                try
                {
                    if (Function_list.Album_pictures(Album_path) != null) { 播放栏专辑.Source = Function_list.Album_pictures(Album_path); }
                    else { 播放栏专辑.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/图标.png")); }
                }
                catch { }
            }
        }

        string Song_path = null;//歌曲路径
        string Album_path = null;//专辑图片路径
        string Lyrics_path = null;//歌词路径
        string[] Album_Singer = new string[3];//存储歌曲信息
        bool temp23 = true;//是否要播放
        [Obsolete]
        void bw_DoWork(object sender, DoWorkEventArgs e)//后台
        {
            try
            {
                string song = e.Argument.ToString();//获得歌曲json 数据
                string name = Function_list.Substring(song, "\"name\":\"", "\"");//取名称
                name += "-" + Function_list.Substring(song, "[\"", "\"]").Replace("\"", "");//取歌手
                name += "-" + Function_list.Substring(song, "\"id\":\"", "\"");//取唯一id,避免重复

                //暂时存储歌曲信息
                Album_Singer[0] = Function_list.Substring(song, "\"name\":\"", "\"");//取名称
                Album_Singer[1] = Function_list.Substring(song, "[\"", "\"]").Replace("\"", "");//歌手
                Album_Singer[2] = Function_list.Substring(song, "\"album\":\"", "\"");//专辑

                if (System.IO.File.Exists(Web_file + name + ".mp3"))//判断下载目录是否存在同名称文件
                {
                    Song_path = Web_file + name + ".mp3";//设置下载名称和路径
                }
                else
                {   //解析歌曲json数据，读取歌曲名称
                    string url = Search_interface.FormatMethod(true).Url(Function_list.Substring(song, "\"id\":\"", "\""), 128);//获取歌曲
                    if (Function_list.Substring(url, "\"url\":\"", "\"") == "")//如果为空，代表歌曲受版权保护，弹出错误窗口
                    {
                        temp23 = false;//不要播放
                        new Thread(() =>//异步调用
                        {
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                new Action(() =>
                                {
                                    Tips = new 弹窗提示(3, color3, 0, "暂时无法播放此歌曲！可能原因如下\n\n1.歌曲包含VIP付费内容\n2.网络连接出现问题\n3.您所在的地区暂时无法提供歌曲服务");
                                    Tips.ShowDialog();
                                }));
                        }).Start();
                    }
                    else//如果歌曲链接可以正常获取
                    {
                        try
                        {
                            Song_path = Web_file + name + ".mp3";
                            using (var client = new WebClient())//对链接进行下载
                            {
                                client.DownloadFile(Function_list.Substring(url, "\"url\":\"", "\""), Song_path);//下载文件
                            }
                            temp23 = true;//可以播放
                        }
                        catch (Exception ex)
                        {
                            temp23 = false;//下载时出现错误，不播放歌曲弹出错误窗口
                            new Thread(() =>//异步调用
                            {
                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                    new Action(() =>
                                    {
                                        Tips = new 弹窗提示(4, color3, 0, "播放失败：\n" + ex.ToString());
                                        Tips.ShowDialog();
                                    }));
                            }).Start();
                        }
                    }
                }

                if (System.IO.File.Exists(Web_file + name + ".png"))//判断下载目录是否有歌曲专辑文件
                {
                    Album_path = Web_file + name + ".png";
                }
                else
                {
                    string pic = Search_interface.FormatMethod(true).Pic(Function_list.Substring(song, "\"pic_id\":\"", "\""), 300);//获取歌曲专辑图片
                    Album_path = Web_file + name + ".jpg";//写入路径
                    using (var client = new WebClient())//对专辑图片下载
                    {
                        client.DownloadFile(Function_list.Substring(pic, "\"url\":\"", "\""), Album_path);//下载文件
                    }

                    //图片转png格式。可以修复部分图片读取失败的方案
                    System.Drawing.Image Dummy = System.Drawing.Image.FromFile(Album_path);
                    Dummy.Save(Path.GetDirectoryName(Album_path) + @"\" + Path.GetFileNameWithoutExtension(Album_path) + ".png", System.Drawing.Imaging.ImageFormat.Bmp);
                    Dummy.Dispose();
                    System.IO.File.Delete(Album_path);//删除源文件
                    Album_path = Web_file + name + ".png";
                }

                if (System.IO.File.Exists(Web_file + name + ".lrc"))//判断下载目录下是否存在歌词文件
                {
                    Lyrics_path = Web_file + name + ".lrc";
                }
                else
                {
                    string lrc = Web_file + name + ".lrc";//设置路径

                    Lyrics_path = Function_list.Substring(Search_interface.FormatMethod(true).Lyric(Function_list.Substring(song, "\"id\":\"", "\"")), "\"lyric\":\"", "\"");//获取歌词
                    Lyrics_path = Lyrics_path.Replace(@"\n", "\n");//自动换行
                    System.IO.StreamWriter f2 = new System.IO.StreamWriter(lrc, true, System.Text.Encoding.Default);
                    f2.WriteLine(Lyrics_path);//写入本地文件
                    f2.Close();
                    f2.Dispose();//关闭文件流
                }
            }
            catch { }
        }
    }
}