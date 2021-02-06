using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using Shell32;
using System;
//using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
//using System.Linq;
using System.Text;
//using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
using TagLib;
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
        ////获得模板内控件属性
        //public T GetVisualChild<T>(DependencyObject parent, Func<T, bool> predicate) where T : Visual
        //{
        //    int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < numVisuals; i++)
        //    {
        //        DependencyObject v = VisualTreeHelper.GetChild(parent, i);
        //        T child = v as T;
        //        if (child == null)
        //        {
        //            child = GetVisualChild<T>(v, predicate);
        //            if (child != null)
        //            {
        //                return child;
        //            }
        //        }
        //        else
        //        {
        //            if (predicate(child))
        //            {
        //                return child;
        //            }
        //        }
        //    }
        //    return null;
        //}
        //[System.Runtime.InteropServices.DllImport("gdi32.dll")]

        //public static extern bool DeleteObject(IntPtr hObject);

        //private BitmapImage Bitmap2BitmapImage(System.Drawing.Bitmap bitmap)

        //{
        //    IntPtr hBitmap = bitmap.GetHbitmap();

        //    BitmapImage retval;

        //    try

        //    {
        //        retval = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(

        //        hBitmap,

        //        IntPtr.Zero,

        //        Int32Rect.Empty,

        //        BitmapSizeOptions.FromEmptyOptions());

        //    }

        //    finally

        //    {
        //        DeleteObject(hBitmap);

        //    }

        //    return retval;

        //}
        //public static System.Drawing.Bitmap KiResizeImage(System.Drawing.Bitmap bmp, int newW, int newH)
        //{
        //    try
        //    {
        //        System.Drawing.Bitmap b = new System.Drawing.Bitmap(newW, newH);
        //        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b);

        //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        //        g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, newW, newH), new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.GraphicsUnit.Pixel);
        //        g.Dispose();

        //        return b;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        //回收内存

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]//立即回收内存
        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        public void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        [Obsolete]
        private void Error_capture (string str)
        {
            if (Properties.Settings.Default.错误报告 == true)
            {
                Tips = new 弹窗提示(4, color3, 0, str);
                Tips.ShowDialog();
                Tips = null;
            }
        }
        [Obsolete]
        private void Song_scanning(int a,string file)//1全盘2单文件夹，文件夹路径
        {
            Kill();
            主页的播放列表.Items.Clear();//清空原始歌曲
            播放列队.Items.Clear();
            Array.Clear(Number, 0, Number.Length);//清空数组
            if (a == 1)
            {
                ListFiles(new DirectoryInfo(file));
                Number_Serial = 0;//复位歌曲填充序号
                Properties.Settings.Default.歌单路径 = file;
                Properties.Settings.Default.歌单路径参数 = 2;
            }
            else if (a == 2)
            {
                query(file);
                Properties.Settings.Default.歌单路径 = file;
                Properties.Settings.Default.歌单路径参数 = 1;
            }
            Tips = new 弹窗提示(1, color3, Number_of_songs,null);
            Tips.ShowDialog();
        }
        //秒转分钟函数
        private string sec_to_hms(double duration)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
            string str = "";
            if (ts.Hours > 0)
            {
                str = String.Format("{0:00}", ts.Hours) + ":" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = "00:" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:00:" + String.Format("{0:00}", ts.Seconds);
            }
            return str;
        }

        ////删除文件夹
        //[Obsolete]
        //public void DeleteDir(string file)
        //{
        //    try
        //    {
        //        //去除文件夹和子文件的只读属性
        //        //去除文件夹的只读属性
        //        System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
        //        fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
        //        //去除文件的只读属性
        //        System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
        //        //判断文件夹是否还存在
        //        if (Directory.Exists(file))
        //        {
        //            foreach (string f in Directory.GetFileSystemEntries(file))
        //            {
        //                if (System.IO.File.Exists(f))
        //                {
        //                    //如果有子文件删除文件
        //                    System.IO.File.Delete(f);
        //                    Console.WriteLine(f);
        //                }
        //                else
        //                {
        //                    //循环递归删除子文件夹
        //                    DeleteDir(f);
        //                }
        //            }
        //            //删除空文件夹
        //            Directory.Delete(file);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Tips = new 弹窗提示(4, color3, 0, "问题详情:" + ex);
        //        Tips.ShowDialog();
        //        Tips = null;
        //    }
        //}

        ////保存文件函数(路径，内容)
        //[Obsolete]
        //public void WriteFile(string Path, string Strings)
        //{
        //    try
        //    {
        //        if (Strings == null)
        //        {
        //            Strings = "[00:05.00]该歌曲为纯音乐,或没有找到歌词文件";
        //            if (Get != null)
        //            {
        //                Get.主.Content = "该歌曲为纯音乐,或没有找到歌词文件";
        //                Get.父.Content = "";
        //            }
        //        }

        //        if (System.IO.File.Exists(Path))
        //        {
        //            //如果文件存在就删除
        //            System.IO.File.Delete(Path);
        //            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, true, System.Text.Encoding.UTF8);
        //            f2.WriteLine(Strings);
        //            f2.Close();
        //            f2.Dispose();
        //        }
        //        else
        //        {
        //            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, true, System.Text.Encoding.UTF8);
        //            f2.WriteLine(Strings);
        //            f2.Close();
        //            f2.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Tips = new 弹窗提示(4, color3, 0, "问题详情:" + ex);
        //        Tips.ShowDialog();
        //        Tips = null;
        //    }
        //}
        //利用字符串indexof截取时间
        public static string Substring(string sourse, string startstr, string endstr)
        {
            string result = string.Empty;
            int startindex, endindex;
            try
            {
                startindex = sourse.IndexOf(startstr);
                if (startindex == -1)
                {
                    return result;
                }
                string tmpstr = sourse.Substring(startindex + startstr.Length);
                endindex = tmpstr.IndexOf(endstr);
                if (endindex == -1)
                {
                    return result;
                }
                result = tmpstr.Remove(endindex);
            }
            catch { }
            return result;
        }

        private void Read_lyrics_string(string lrc)
        {
            int a = 0;
            string[] temp = lrc.Split('\n');
            for (int i = 0; i < temp.Length; i ++)
            {
                //去除歌词前部分歌曲信息
                if (temp[i] != "" && temp[i].Substring(1, 2) != "ar" && temp[i].Substring(1, 2) != "ti" && temp[i].Substring(1, 2) != "al" && temp[i].Substring(1, 2) != "by" && temp[i].Substring(1, 2) != "of")
                {
                    lrc_time[a] = Substring(temp[i], "[", "]"); //截取时间
                    lrc_lyrics[a] = temp[i].Substring(lrc_time[a].Length + 2, temp[i].Length - lrc_time[a].Length - 2);//截取歌词
                    if (lrc_lyrics[a] == "" || lrc_lyrics[a] == "//") { }//剔除空行和//
                    else { a++; }//当符合条件数组下标才会定位到下一行
                }
            }
            打印歌词();//将读取后的歌词打印到滚动歌词控件上
        }

        //读取歌词函数
        private string 读取歌词(string file)
        {
            string msg;
            string lrc = "";
            using (StreamReader reader = new StreamReader(file, Encoding.Default))
            {
                while ((msg = reader.ReadLine()) != null) { lrc += msg + "\n"; }//!=  不等于   
            }
            return lrc;
        }
        //将歌词打印到歌词滚动上
        private void 打印歌词()
        {
            for (int i = 0; i < lrc_time.Length; i++)
            { //遍历打印歌词
                if (lrc_lyrics[i] != null) { 填充菜单(歌词滚动显示, lrc_lyrics[i], null, 470, 35, 15, true); }
            }
            try { ((ListBoxItem)歌词滚动显示.Items[0]).Foreground = new SolidColorBrush(color); }//将第一句歌词颜色该为红色
            catch { }
            if (歌词滚动显示 .Items .Count > 2)
            {
                for (int i = 0; i < 5; i++) { 填充菜单(歌词滚动显示, null, null, 470, 35, 15, true); }//多打印空白行5个
            }
        }

        //读取专辑文件
        [Obsolete]
        private BitmapImage GetCover(string path)
        {
            BitmapImage bmp = new BitmapImage();
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                if (f.Tag.Pictures != null && f.Tag.Pictures.Length != 0)
                {
                    //字节流转BitmapImage对象
                    bmp.BeginInit();
                    bmp.StreamSource = new MemoryStream(f.Tag.Pictures[0].Data.Data);
                    bmp.EndInit();
                }
            }
            catch (Exception ex)
            {
                Error_capture("读取专辑图片时发生异常，问题详细信息:\n" + ex.ToString());
                bmp = null;
            }
            return bmp;
        }

        //public static TagLib.File ParsePhoto(string path)
        //{
        //    if (string.IsNullOrEmpty(path))
        //    {
        //        throw new ArgumentNullException(path);
        //    }
        //    TagLib.File file;
        //    try
        //    {
        //        file = TagLib.File.Create(path);
        //    }
        //    catch (UnsupportedFormatException uex)
        //    {
        //        throw uex;
        //    }
        //    var image = file as TagLib.Image.File;
        //    if (file == null)
        //    {
        //        throw new ArgumentNullException("file");
        //    }
        //    return image;
        //}
        ////字节流转图片
        //private void Bytes2File(byte[] b, string file)
        //{
        //    FileStream fs = null;
        //    try
        //    {
        //        fs = new FileStream(file, FileMode.Create, FileAccess.Write);
        //        fs.Write(b, 0, b.Length);
        //    }
        //    catch { }
        //    finally
        //    {

        //        fs.Close();//释放文件句柄
        //        fs.Dispose();
        //    }
        //}

        //单文件夹扫描歌曲
        [Obsolete]
        private void query(string str)//str歌曲路径
        {
            int sum = 0;
            string[] ssg = { "*.mp3", "*.wav", "*.flac" };//设置过滤器
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    string[] dirs = Directory.GetFiles(str + @"\", ssg[i]);
                    foreach (string dir in dirs)
                    {
                        Number[sum] = dir;
                        sum ++;
                        填充菜单(主页的播放列表, System.IO.Path.GetFileNameWithoutExtension(dir), System.IO.Path.GetFileNameWithoutExtension(dir), 315, 40, 16, false);
                        填充菜单(播放列队, System.IO.Path.GetFileNameWithoutExtension(dir), System.IO.Path.GetFileNameWithoutExtension(dir), 315, 40, 16, false);
                    }
                }
                catch (Exception ex)
                {
                    Error_capture( "扫描歌曲时发生异常,异常详细信息:\n" + ex.ToString());
                }
            }
            歌曲数量.Content = "共" + sum + "首歌曲";
            Number_of_songs = sum;
        }
        //全盘递归扫描歌曲文件
        private int Number_Serial = 0;//存储与歌单列表对应的序号
        [Obsolete]
        private void ListFiles(FileSystemInfo info)
        {
            string Ext;
            string[] ssgb = { "mp3", "wav", "flac" };//设置过滤器
            for (int i = 0; i < 3; i++)
            {
                Ext = ssgb[i];
                if (!info.Exists) return;
                DirectoryInfo dir = info as DirectoryInfo;
                //不是目录 
                if (dir == null) return;
                try
                {
                    FileSystemInfo[] files = dir.GetFileSystemInfos();
                    for (int j = 0; j < files.Length; j++)
                    {
                        //是文件
                        if (files[j] is FileInfo file && file.Extension.ToUpper() == "." + Ext.ToUpper())
                        {
                            int id = Array.IndexOf(Number, file.FullName);
                            if (id == -1)
                            {
                                填充菜单(主页的播放列表, System.IO.Path.GetFileNameWithoutExtension(file.FullName), System.IO.Path.GetFileNameWithoutExtension(file.FullName), 315, 40, 16, false);
                                填充菜单(播放列队, System.IO.Path.GetFileNameWithoutExtension(file.FullName), System.IO.Path.GetFileNameWithoutExtension(file.FullName), 275, 40, 16, false);
                                Number[Number_Serial] = file.FullName;
                                Number_Serial += 1;
                            }
                        }
                        //对于子目录，进行递归调用 
                        else ListFiles(files[j]);
                    }
                }
                catch (Exception ex)
                {
                    Error_capture("扫描歌曲时发生异常，异常详细信息:\n"+ex.ToString());
                }
            }
            歌曲数量.Content = "共" + Number_Serial + "首歌曲";
            Number_of_songs = Number_Serial;
        }
        //填充菜单
        private void 填充菜单(ListBox box, string name, string tooltip, int width, int height, int fontsize, bool Centered)
        {
            ListBoxItem item = new ListBoxItem();
            item.Content = name;//设置显示名称
            item.Width = width;//设置宽度
            item.Height = height;//设置高度
            item.FontSize = fontsize;//设置字号
            item.ToolTip = tooltip;//设置提示
            if (Centered == true)
            {
                item.HorizontalContentAlignment = HorizontalAlignment.Center;//设置文本上下左右居中
            }
            box.Items.Add(item);//将控件添加到集合里
        }

        private string Embedded_lyrics()
        {
            string a = "";
            for (int i = 0; i < lrc_time.Length; i++)
            {
                if (lrc_time[i] != null) { a += "[" + lrc_time[i] + "]" + lrc_lyrics[i] + "\n"; }
            }
            return a;
        }

        ////图片转bitmapImage对象，防止图片被占用
        //[Obsolete]
        //private BitmapImage Album_pictures(string file)
        //{
        //    BitmapImage bitmapImage = new BitmapImage();
        //    bitmapImage.BeginInit();
        //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //    try
        //    {
        //        bitmapImage.UriSource = new Uri(file);//图片的全路径
        //        bitmapImage.EndInit();
        //        bitmapImage.Freeze();
        //        return bitmapImage;
        //    }
        //    catch (Exception ex)
        //    {
        //        Error_capture(ex.ToString());
        //        return bitmapImage;
        //    }
        //}
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
                    string Albumstr = Read_information.Tag.Album;//获取专辑;
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
                    //读取歌词
                    if (namestr != null)//判断歌曲是否内嵌入歌词
                    {//如果有
                        Read_lyrics_string(namestr);
                        Geci = 0;
                    }
                    else if (Properties.Settings.Default.歌词目录 != "默认" && Directory.Exists(Properties.Settings.Default.歌词目录) && System.IO.File.Exists(Properties.Settings.Default.歌词目录 + @"\" + System.IO.Path.GetFileNameWithoutExtension(File_s) + ".lrc"))//如果没有找到歌词，从用户设置目录读取
                    {//如果用户设置了目录，并且目录存在,并且目录下存在与歌曲名称同名的歌词文件
                        Read_lyrics_string(读取歌词(Properties.Settings.Default.歌词目录 + @"\" + System.IO.Path.GetFileNameWithoutExtension(File_s) + ".lrc"));
                        Geci = 0;
                        if (Properties.Settings.Default.嵌入歌词 == true)
                        {
                            Read_information.Tag.Lyrics = Embedded_lyrics();
                            Read_information.Save();
                        }

                    }//从歌曲目录下寻找歌词文件
                    else if (System.IO.File.Exists(Regex.Replace(File_s, System.IO.Path.GetFileName(File_s), string.Empty, RegexOptions.Compiled) + System.IO.Path.GetFileNameWithoutExtension(File_s) + ".lrc"))
                    {//如果存在
                        Read_lyrics_string(读取歌词(Regex.Replace(File_s, System.IO.Path.GetFileName(File_s), string.Empty, RegexOptions.Compiled) + System.IO.Path.GetFileNameWithoutExtension(File_s) + ".lrc"));
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
                    }//歌词不存在，不打印歌词

                    //读取专辑图片
                    if (GetCover(File_s) != null) { 播放栏专辑.Source = GetCover(File_s); }
                    else { 播放栏专辑.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/图标.png")); }

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
                        }
                        catch { }
                    }
                    t2.Start();//时间刷新
                    播放器.Play();//开始播放
                }
            }
            catch (Exception ex)
            {
                Error_capture("播放歌曲时发生异常，异常详细信息:\n" + ex.ToString());
            }
            
        }

        //用于判断播放条件
        [Obsolete]
        private void Playing_conditions()
        {
            if (底部列表.SelectedIndex == -1) { 底部列表.SelectedIndex = 0; }//如果用户没有选择播放模式默认列表循环
            Kill();
            if (底部列表.SelectedIndex == 0)//列表循环
            {
                if (主页的播放列表.SelectedIndex + 1 == 主页的播放列表.Items.Count)//判断是否到列表底部
                {
                    Play_Misic(Number[0]);//从列表第一个开始播放
                    主页的播放列表.SelectedIndex = 0;//定位
                }
                else
                {
                    Play_Misic(Number[主页的播放列表.SelectedIndex + 1]);//播放下一个
                    主页的播放列表.SelectedIndex += 1;//定位
                }
            }
            else if (底部列表.SelectedIndex == 1)//单曲循环
            {
                Play_Misic(Number[主页的播放列表.SelectedIndex]);//播放当前选中项
            }
            else if (底部列表.SelectedIndex == 2)//随机播放
            {
                int stc = 0;//循环条件
                while (stc == 0)//循环
                {
                    Random rd = new Random();//定义随机数
                    int i = rd.Next(主页的播放列表.Items.Count - 1);//随机范围在列表最大-1
                    if (主页的播放列表.SelectedIndex != i)//排除当前正在播放的
                    {
                        Play_Misic(Number[i]);//播放歌曲
                        主页的播放列表.SelectedIndex = i;//定位
                        stc = 1;//条件排除
                    }
                }
            }
            else if (底部列表.SelectedIndex == 3)//顺序播放
            {
                if (主页的播放列表.SelectedIndex + 1 != 主页的播放列表.Items.Count)
                {
                    Play_Misic(Number[主页的播放列表.SelectedIndex + 1]);
                    主页的播放列表.SelectedIndex += 1;
                }
            }
        }
        //初始化播放，为下一次播放做准备
        private void Kill()
        {
            t1.Close();//释放计时器占用的资源
            t2.Close();
            Array.Clear(lrc_time, 0, lrc_time.Length);//清空歌词数组
            Array.Clear(lrc_lyrics, 0, lrc_lyrics.Length);
            if (Get != null)
            {
                Get.主.Content = "";
                Get.父.Content = "";
            }
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
            FlushMemory();//回收内存
        }

        //更新时间，判断播放状态
        [Obsolete]
        public void theout2(object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(//同步线程
                   new Action(
                 delegate
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
                         {
                             播放时间.Content = Convert.ToString(播放器.Position).Substring(3, 5) + "-" + Song_time;
                             进度条.Value = 播放器.Position.TotalSeconds;
                         }
                     }
                 }));
        }
        //this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
        //{
        //    try
        //    {
        //        for (int i = 0; i < lrc_time.Length; i++)
        //        {
        //            if (lrc_time[i] != null && lrc_time[i].Substring(0, 7) == Convert.ToString(播放器.Position).Substring(3, 7) && lyrics_display == true)
        //            {
        //                歌词滚动显示.SelectedIndex = i;
        //                ((ListBoxItem)歌词滚动显示.Items[i]).Foreground = new SolidColorBrush(color);//将当前一句颜色该为红色
        //                if (Rolling_condition == 0)
        //                {
        //                    歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex + 5]);//让歌词显示在中间
        //                }
        //                for (int j = 0; j < 歌词滚动显示.SelectedIndex; j++)
        //                {
        //                    ((ListBoxItem)歌词滚动显示.Items[j]).Foreground = new SolidColorBrush(color2);
        //                }
        //                for (int o = 歌词滚动显示.Items.Count - 5; o > 歌词滚动显示.SelectedIndex; o--)
        //                {
        //                    ((ListBoxItem)歌词滚动显示.Items[o]).Foreground = new SolidColorBrush(color2);
        //                }
        //                if (Get != null)
        //                {
        //                    Get.主.Content = lrc_lyrics[i];
        //                    Get.父.Content = lrc_lyrics[i + 1];
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //});
        //new Thread(() =>
        //{
        //    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //        new Action(() =>
        //        {
        //            try
        //            {
        //                for (int i = 0; i < lrc_time.Length; i++)
        //                {
        //                    if (lrc_time[i] != null && lrc_time[i].Substring(0, 7) == Convert.ToString(播放器.Position).Substring(3, 7) && lyrics_display == true)
        //                    {
        //                        歌词滚动显示.SelectedIndex = i;
        //                        ((ListBoxItem)歌词滚动显示.Items[i]).Foreground = new SolidColorBrush(color);//将当前一句颜色该为红色
        //                        if (Rolling_condition == 0)
        //                        {
        //                            歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex + 5]);//让歌词显示在中间
        //                        }
        //                        for (int j = 0; j < 歌词滚动显示.SelectedIndex; j++)
        //                        {
        //                            ((ListBoxItem)歌词滚动显示.Items[j]).Foreground = new SolidColorBrush(color2);
        //                        }
        //                        for (int o = 歌词滚动显示.Items.Count - 5; o > 歌词滚动显示.SelectedIndex; o--)
        //                        {
        //                            ((ListBoxItem)歌词滚动显示.Items[o]).Foreground = new SolidColorBrush(color2);
        //                        }
        //                        if (Get != null)
        //                        {
        //                            Get.主.Content = lrc_lyrics[i];
        //                            Get.父.Content = lrc_lyrics[i + 1];
        //                        }
        //                    }
        //                }
        //            }
        //            catch { }
        //            //Thread.Sleep(TimeSpan.FromSeconds(2));
        //            //this.lblHello.Content = "欢迎你光临WPF的世界,Dispatche 异步方法！！" + DateTime.Now.ToString();
        //        }));
        //}).Start();

        //刷新歌词计时器事件
        Color color = (Color)ColorConverter.ConvertFromString("#F8BF2424");//红色
        Color color2 = (Color)ColorConverter.ConvertFromString("#FF000000");//黑色
        public void theout1(object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(//同步线程
                  new Action(
                delegate
                {
                    try
                    {
                        for (int i = 0; i < lrc_time.Length; i++)
                        {
                            if (lrc_time[i] != null && lrc_time[i].Substring(0, 7) == Convert.ToString(播放器.Position).Substring(3, 7) && lyrics_display == true)
                            {
                                歌词滚动显示.SelectedIndex = i;
                                ((ListBoxItem)歌词滚动显示.Items[i]).Foreground = new SolidColorBrush(color);//将当前一句颜色该为红色
                                if (Rolling_condition == 0)
                                {
                                    歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex + 5]);//让歌词显示在中间
                                }
                                for (int j = 0; j < 歌词滚动显示.SelectedIndex; j++)
                                {
                                    ((ListBoxItem)歌词滚动显示.Items[j]).Foreground = new SolidColorBrush(color2);
                                }
                                for (int o = 歌词滚动显示.Items.Count - 5; o > 歌词滚动显示.SelectedIndex; o--)
                                {
                                    ((ListBoxItem)歌词滚动显示.Items[o]).Foreground = new SolidColorBrush(color2);
                                }
                                if (Get != null)
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

        private string[] lrc_time = new string[200];//存储时间
        private string[] lrc_lyrics = new string[200];//存储与时间对应的歌词
        private string[] Number = new string[2000];//存储歌单列表，格式为绝对路径
        private bool lyrics_display;//决定是否要显示滚动歌词
        private int Number_of_songs = 0;//存储歌曲数量
        private int Rolling_condition = 0;
        System.Timers.Timer t1 = new System.Timers.Timer(50);//实例化Timer类用于刷新歌词
        System.Timers.Timer t2 = new System.Timers.Timer(80);//实例化Timer类用于更新时间
        桌面歌词 Get;
        弹窗提示 Tips ;

        //private bool Dynamic = false;
        //public event Func<string,string, int> Fcc1;//定义委托.刷新桌面歌词
        //System.Timers.Timer t3 = new System.Timers.Timer(10);//实例化Timer类用于刷新歌词
        //System.Timers.Timer t4 = new System.Timers.Timer(10);//实例化Timer类用于刷新歌词
        [Obsolete]
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                速度.Visibility = Visibility.Collapsed;
                RenderOptions.SetBitmapScalingMode(歌词滚动显示, BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(播放列队框架, BitmapScalingMode.NearestNeighbor);
                //if (false == System.IO.Directory.Exists(System.IO.Path.GetTempPath() + @"方糖音乐\"))//判断目录是否存在
                //{
                //    //创建临时目录
                //    System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + @"方糖音乐\");
                //}
                t1.Elapsed += new System.Timers.ElapsedEventHandler(theout1);//到达时间的时候执行事件
                t1.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
                t1.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件

                t2.Elapsed += new System.Timers.ElapsedEventHandler(theout2);//到达时间的时候执行事件
                t2.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
                t2.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件

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
                    背景.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/默认背景.png"));//加载背景图片
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
                            query(Properties.Settings.Default.歌单路径);

                        }
                        else if (Properties.Settings.Default.歌单路径参数 == 2)//全盘
                        {
                            ListFiles(new DirectoryInfo(Properties.Settings.Default.歌单路径));
                            Number_Serial = 0;//复位歌曲填充序号
                        }
                    }
                }
                //加载颜色
                string a = Properties.Settings.Default.主题颜色;
                if (a == "System.Windows.Controls.ComboBoxItem: 宝石绿")
                {
                    主题颜色.SelectedIndex = 0;
                }
                else if (a == "System.Windows.Controls.ComboBoxItem: 桃花粉")
                {
                    主题颜色.SelectedIndex = 1;
                }
                else if (a == "System.Windows.Controls.ComboBoxItem: 旬子蓝")
                {
                    主题颜色.SelectedIndex = 2;
                }
                else if (a == "System.Windows.Controls.ComboBoxItem: 山茶红")
                {
                    主题颜色.SelectedIndex = 3;
                }

                if (Properties.Settings.Default.桌面歌词 == true)
                {
                    Get = new 桌面歌词(color3);
                    Get.fcc1 += Playback_status;
                    Get.Show();
                }
                播放栏专辑.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/图标.png"));//从资源文件加载图片
                音量条.Value = Properties.Settings.Default.音量;
                背景模糊.Radius = Properties.Settings.Default.背景模糊程度;
            }
            catch (Exception ex)
            {
                Error_capture("程序初始化异常\n" + ex.ToString());
                Tips = new 弹窗提示(1, color3, 0, "由于程序初始化遇到问题，已经将程序恢复默认值,稍后您可以重新启动");
                Tips.ShowDialog();
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
        private void 音量条_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Temp3 == 1)
            {
                音量显示.Content = 音量条.Value + "%";
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
            Rolling_condition = 1;
        }
        //当鼠标离开歌词时，开始重新定位，开始滚动
        private void 歌词滚动显示_MouseLeave(object sender, MouseEventArgs e)
        {
            Rolling_condition = 0;
            try
            {
                if (歌词滚动显示.SelectedIndex <= 5)
                {
                    //歌词滚动显示.SelectedIndex = 0;
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
                    int a1 = int.Parse(lrc_time[Xhao].Substring(0, 2)) * 60 + int.Parse(lrc_time[Xhao].Substring(3, 2));
                    播放器.Position = TimeSpan.FromSeconds(a1);
                    歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex + 5]);//让歌词显示在中间
                }
                catch { }
            }
        }

        //private string CheckTrueFileName(string file)
        //{
        //    string path = file;
        //    System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //    System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
        //    string bx = " ";
        //    byte buffer;
        //    try
        //    {
        //        buffer = r.ReadByte();
        //        bx = buffer.ToString();
        //        buffer = r.ReadByte();
        //        bx += buffer.ToString();
        //    }
        //    catch (Exception exc) { Console.WriteLine(exc.Message); }
        //    r.Close();
        //    fs.Close();
        //    Console.WriteLine(bx);
        //    return bx;
        //}
        [Obsolete]
        private void 大专辑_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (播放器.Source != null)
            {
                Tips = new 弹窗提示(2, color3, 0, 播放器.Source.ToString());
                Tips.ShowDialog();
            }
            //MessageBox.Show(CheckTrueFileName(@"E:\歌曲\MusicDownload\爱的奇妙物语 - 张子枫.flac"));
            //System.IO.FileInfo f = new FileInfo(@"E:\歌曲\MusicDownload\爱的奇妙物语 - 张子枫.flac");
            ////double a = GetFileSize(f.Length);
            //MessageBox.Show(GetFileSize(f.Length));
            //BitmapImage bitmapImage = new BitmapImage();//用于读取专辑图片，防止文件占用
            //bitmapImage.BeginInit();
            //bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //bitmapImage.UriSource = new Uri(@"C:\Users\邢传真\AppData\Local\Temp\方糖音乐\白月光与朱砂痣 - 大籽.png");//szPath为图片的全路径
            //bitmapImage.EndInit();
            //bitmapImage.Freeze();
            //播放栏专辑.Source = bitmapImage;
            //播放栏专辑.Source = new BitmapImage(new Uri(@"C:\Users\邢传真\AppData\Local\Temp\方糖音乐\空山新雨后 - 音阙诗听&锦零.png"));
            //Get = new 桌面歌词();
            //Get.Show();
            //Get.主.Content = "11";
            //Fcc1("", "");
            //桌面歌词 child = new 桌面歌词();
            //child.Show();
            //FileStream Stream = System.IO.File.Open(@"D:\白月光与朱砂痣 - 大籽.png", FileMode.Open);
            //BitmapImage bitmap = new BitmapImage();
            //bitmap.StreamSource = Stream;
            //播放栏专辑.Source = bitmap;

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
                Play_Misic(Number[主页的播放列表.SelectedIndex]);//播放歌曲
            }
        }
        [Obsolete]
        private void 播放列队_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (播放列队.SelectedIndex != -1)
            {
                Temp5 = 0;
                Kill();//初始化播放器
                Play_Misic(Number[播放列队.SelectedIndex]);//播放歌曲
                主页的播放列表.SelectedIndex = 播放列队.SelectedIndex;
                黑幕_MouseUp(null, null);
            }
        }
        //单击播放按钮
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
                }
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
            }
        }

        [Obsolete]
        private void 选择判断(int r)
        {
            if (r == 1)
            {
                Kill();
                if (主页的播放列表.SelectedIndex == 0)
                {
                    Play_Misic(Number[主页的播放列表.Items.Count - 1]);
                    主页的播放列表.SelectedIndex = 0;
                }
                else
                {
                    Play_Misic(Number[主页的播放列表.SelectedIndex - 1]);
                    主页的播放列表.SelectedIndex -= 1;
                }
            }
            else if (r == 2)
            {
                Kill();
                if (主页的播放列表.SelectedIndex + 1 == 主页的播放列表.Items.Count)
                {
                    Play_Misic(Number[0]);
                    主页的播放列表.SelectedIndex = 0;
                }
                else
                {
                    Play_Misic(Number[主页的播放列表.SelectedIndex + 1]);
                    主页的播放列表.SelectedIndex += 1;
                }
            }
        }
        //单击下一首，如果在最底下就转到最上面
        [Obsolete]
        private void 下一曲_Click(object sender, RoutedEventArgs e)
        {

            if (底部列表.SelectedIndex != -1)
            {
                if (底部列表.SelectedIndex == 0 || 底部列表.SelectedIndex == 1 || 底部列表.SelectedIndex == 3)
                {
                    选择判断(2);
                }
                else if (底部列表.SelectedIndex == 2)
                {
                    Playing_conditions();
                }
            }
            else
            {
                底部列表.SelectedIndex = 0;
                下一曲_Click(null, null);
            }
        }
        //单击上一曲,如果在底部就转到最底部
        [Obsolete]
        private void 上一曲_Click(object sender, RoutedEventArgs e)
        {
            if (主页的播放列表.SelectedIndex != -1)
            {
                if (底部列表.SelectedIndex != -1)
                {
                    if (底部列表.SelectedIndex == 0 || 底部列表.SelectedIndex == 1 || 底部列表.SelectedIndex == 3)
                    {
                        选择判断(1);
                    }
                    else if (底部列表.SelectedIndex == 2)
                    {
                        Playing_conditions();
                    }
                }
                else
                {
                    底部列表.SelectedIndex = 0;
                    上一曲_Click(null, null);
                }
            }
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
                if (dialog.FileName != "")
                {
                    Song_scanning(2, dialog.FileName);
                }
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
                if (dialog.FileName != "")
                {
                    Song_scanning(1, dialog.FileName);
                }
            }
            catch { }
        }

        //public T GetVisualChild<T>(DependencyObject parent, Func<T, bool> predicate) where T : Visual
        //{
        //    int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < numVisuals; i++)
        //    {
        //        DependencyObject v = VisualTreeHelper.GetChild(parent, i);
        //        if (!(v is T child))
        //        {
        //            child = GetVisualChild<T>(v, predicate);
        //            if (child != null) { return child; }
        //        }
        //        else
        //        {
        //            if (predicate(child)) { return child; }
        //        }
        //    }
        //    return null;
        //}
        //TextBlock 时间;
        //时间 = GetVisualChild<TextBlock>(进度条, v => v.Name == "进度条显示");
        //时间.Text = sec_to_hms(进度条.Value).Substring(3, 5);
        private void 进度条_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (button == 1)
            {
                播放时间.Content = sec_to_hms(进度条.Value).Substring(3, 5) + "-" + Song_time;
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

            if (Get != null)
            {
                Get .背景 .Fill = new SolidColorBrush(color3);
            }
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
            if (搜索框.SelectedIndex != -1 && ID[0] != null)
            {
                Kill();//清除播放器状态
                for (int i = 0; i < Number.Length; i++)
                {
                    if (Number[i] == ID[搜索框.SelectedIndex])//定位文件
                    {
                        主页的播放列表.SelectedIndex = i;
                    }
                }
                Play_Misic(ID[搜索框.SelectedIndex]);//开始播放
            }
        }
        //搜索框，可以搜索歌曲
        int sub, stc = 0;
        private string[] ID = new string[2000];//定义数组存储搜索结果
        int 搜索框关闭;
        private void 搜索_KeyUp(object sender, KeyEventArgs e)
        {
            搜索框关闭 = 1;
            搜索框.Items.Clear();//清空内容
            stc = 0;
            Array.Clear(ID, 0, ID.Length);//清空数组
            for (int i = 0; i < Number.Length; i++)//循环最大长度为歌单列表的最大长度
            {
                string s = System.IO.Path.GetFileNameWithoutExtension(Number[i]);//从路径取文件名称
                string v = 搜索.Text;//获取输入的内容
                if (s != null && 搜索.Text != "")//判断值是否为空
                {
                    if (s.Contains(v))//用Contains函数判断用户输入的数值是否包含
                    {
                        ID[stc] = Number[i];//记录文件路径
                        //向列表框中添加搜索结果
                        填充菜单(搜索框, System.IO.Path.GetFileNameWithoutExtension(Number[i]), System.IO.Path.GetFileNameWithoutExtension(Number[i]), 248, 40, 15, false);
                        //序号加加
                        stc++;
                    }
                }
            }
            if (stc == 0 && 搜索.Text != "")
            {
                Label item = new Label();
                item.Content = "什么也木有找到w(ﾟДﾟ)w";//设置显示名称
                item.Width = 248;//设置宽度
                item.Height = 35;//设置高度
                item.FontSize = 15;//设置字号
                搜索框.Items.Add(item);//将控件添加到集合里
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
        private bool form = false;//设置窗口只可以打开一个

        [Obsolete]
        private void 程序设置_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (form == false)
            {
                程序设置 child = new 程序设置(color3);
                child.fcc1 += background;
                child.fcc2 += Vague;
                child.fcc3 += Truefalse;
                child.fcc4 += cancel;
                child.fcc5 += form1;
                child.fcc6 += Zoom_mode;
                zoom();
                child.Show();
                form = true;
                搜索框_MouseLeave(null, null);
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
                if (d == "默认") { 背景.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/默认背景.png")); }
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
                lyrics_display = false;
            }
            else if (sum == 2)
            {
                歌词滚动显示.Visibility = Visibility.Visible;
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
                背景.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/默认背景.png"));//加载背景图片
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
            else
            {
                if (Get != null)
                {
                    Get.Close();
                }
            }
            背景模糊.Radius = Properties.Settings.Default.背景模糊程度;
            Zoom_mode(zoom_temp);
            return 0;
        }

        //快捷键播放暂停，上/下一曲
        [Obsolete]
        private void window窗体_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Space")//空格
            {
                if (播放.Visibility == Visibility.Visible) { 播放_Click(null, null); }
                else { 暂停_Click(null, null); }
            }
            else if (e.Key.ToString() == "Left")//左
            {
                if (上一曲.Visibility == Visibility.Visible) { 上一曲_Click(null, null); }
            }
            else if (e.Key.ToString() == "Right")//右
            {
                if (下一曲.Visibility == Visibility.Visible) { 下一曲_Click(null, null); }
            }
        }

        private int Zoom_mode(int a)
        {
            if (a == 1) { 背景.Stretch = Stretch.None; }
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

        private void 播放列表_MouseLeave(object sender, MouseEventArgs e)
        {
            播放列表.SelectedIndex = -1;
        }

        private void 主界面菜单选择_MouseEnter(object sender, MouseEventArgs e)
        {
            主界面菜单选择.SelectedIndex = -1;
        }

        private void colorZone_MouseEnter(object sender, MouseEventArgs e)
        {
            //动画播放("顶部透明开");
        }

        private void colorZone_MouseLeave(object sender, MouseEventArgs e)
        {
            //动画播放("顶部透明关");
        }

        private void 进度条_MouseEnter(object sender, MouseEventArgs e)
        {
            t2.Interval = 1000;
        }

        private void 进度条_MouseLeave(object sender, MouseEventArgs e)
        {
            t2.Interval = 100;
        }

        private void 速度_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (速度框架.Visibility == Visibility.Collapsed) { 动画播放("速度打开"); }
            else { 动画播放("速度关闭"); }

        }

        private void 速度框架_MouseLeave(object sender, MouseEventArgs e)
        {
            if (速度框架.Visibility == Visibility.Visible) { 动画播放("速度关闭"); }
        }
        int axvb = 0;
        private void 速度条_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (axvb == 1)
            {
                播放器.SpeedRatio = 速度条.Value;
                速度显示.Content = 速度条.Value + "倍速";
            }
            axvb = 1;

        }

        private void 速度调节_MouseLeave(object sender, MouseEventArgs e)
        {
            速度调节.SelectedIndex = -1;
        }

        private void 播放器_MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                主页的播放列表.ScrollIntoView(主页的播放列表.Items[主页的播放列表.SelectedIndex]);
                播放列队.ScrollIntoView(播放列队.Items[播放列队.SelectedIndex]);
            }
            catch { }
        }

        [Obsolete]
        private void window窗体_Drop(object sender, DragEventArgs e)
        {
            string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (System.IO.Path.GetExtension(filePath[0]) != "")
            {
                if(System.IO.Path.GetExtension(filePath[0]) == ".mp3" || System.IO.Path.GetExtension(filePath[0]) == ".wav" || System.IO.Path.GetExtension(filePath[0]) == ".flac")
                {
                    主页的播放列表.SelectedIndex = -1;
                    Kill();
                    Temp5 = 1;
                    Play_Misic(filePath[0]);

                }
                else if(System.IO.Path.GetExtension(filePath[0]) == ".mp4" || System.IO.Path.GetExtension(filePath[0]) == ".wmv"|| System.IO.Path.GetExtension(filePath[0]) == ".avi")
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
                弹窗提示 Tips = new 弹窗提示(0, color3,0,null);
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
        //void bw_DoWork(object sender, DoWorkEventArgs e)//主
        //{
        //    ListFiles(new DirectoryInfo(@"E:\歌曲"));
        //}
        //void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)//返回
        //{
        //    MessageBox.Show("");
        //}
        //[Obsolete]
        private void 播放歌曲名称_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string a = "sdaffaf\naffafaf\ndfsdsgsgs\n";
            string[] temp = a.Split('\n');
            MessageBox.Show(temp[0]);
            MessageBox.Show(temp[1]);
            //string[] temp = Vote.Text.Split('\n');


            //Tips = new 弹窗提示(3, color3, 0, "");
            //Tips.Show();
            // using (BackgroundWorker bw = new BackgroundWorker())
            // {
            //    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            //   bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            //   bw.RunWorkerAsync("Tank");
            //}
        }

        [Obsolete]
        private int Playback_status(int a)
        {
            if (a == 1)
            {
                播放_Click(null, null);
            }
            else if(a == 2)
            {
                暂停_Click(null, null);
            }
            else if (a == 3)
            {
                上一曲_Click(null, null);
            }
            else if (a == 4)
            {
                下一曲_Click(null, null);
            }
            return 0;
        }

        private void 音量调节_MouseLeave(object sender, MouseEventArgs e)
        {
            
            音量调节.SelectedIndex = -1;
        }
    }
}

//绿色#FF2C7F55
//紫色#FF7858BD
//蓝色#FF10AEC2
//红色#FFED556A