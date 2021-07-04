using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace 方糖音乐播放器
{
    public static class Refresh_lyrics
    {
        public static void Lyrics_scrolling(ListBoxItem listBox,MediaElement media )
        {

        }
    }
}

//一下为部分测试代码,不舍得丢掉,万一哪一天又想捡起她



//绿色#FF2C7F55
//紫色#FF7858BD
//蓝色#FF10AEC2
//红色#FFED556A
//private void Lyrics_reset(int i)
//{
//    ((ListBoxItem)歌词滚动显示.Items[i]).Foreground = new SolidColorBrush(color2);
//}

//private string Time_acquisition()
//{
//    try
//    {
//        string time = 播放器.Position.ToString().Substring(3, 7);
//        return time;
//    }
//    catch
//    {
//        return null;
//    }
//}

//private void Lyrics_turn_red(int i)
//{
//    歌词滚动显示.SelectedIndex = i;
//    ((ListBoxItem)歌词滚动显示.Items[i]).Foreground = new SolidColorBrush(color);//将当前一句颜色该为红色
//}

//private void Lyrics_in_the_middle()
//{
//    歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex + 5]);//让歌词显示在中间
//}

//private int Maximum_quantity()
//{
//    return 歌词滚动显示.SelectedIndex;
//}

//private int Maximum_quantity2()
//{
//    return 歌词滚动显示.Items.Count;
//}
//刷新歌词计时器事件

//if (Time_acquisition() != null)
//{
//    for (int i = 0; i < lrc_time.Count; i++)
//    {
//        if (lrc_time[i] != null && lrc_time[i].ToString().Substring(0, 7) == Time_acquisition() && lyrics_display == true)
//        {
//            Lyrics_turn_red(i);
//            歌词滚动显示.SelectedIndex = i;
//            ((ListBoxItem)歌词滚动显示.Items[i]).Foreground = new SolidColorBrush(color);//将当前一句颜色该为红色
//            if (Rolling_condition == 0)
//            {
//                Lyrics_in_the_middle();
//                歌词滚动显示.ScrollIntoView(歌词滚动显示.Items[歌词滚动显示.SelectedIndex + 5]);//让歌词显示在中间
//            }
//            for (int j = 0; j < Maximum_quantity(); j++)
//            {
//                Lyrics_reset(j);
//                ((ListBoxItem)歌词滚动显示.Items[j]).Foreground = new SolidColorBrush(color2);
//            }
//            for (int o = Maximum_quantity2() - 5; o > Maximum_quantity(); o--)
//            {
//                Lyrics_reset(o);
//                ((ListBoxItem)歌词滚动显示.Items[o]).Foreground = new SolidColorBrush(color2);
//            }
//            if (Get != null)
//            {
//                Get.主.Content = lrc_lyrics[i];
//                Get.父.Content = lrc_lyrics[i + 1];
//            }
//        }
//    }
//}
//Dispatcher.Invoke(new Action(delegate
//{

//}));

//Dispatcher.Invoke(new Action(delegate
//{
//    try
//    {
//    }
//    catch { }
//}));
//string a = "sdaffaf\naffafaf\ndfsdsgsgs\n";
//string[] temp = a.Split('\n');
//MessageBox.Show(temp[0]);
//MessageBox.Show(temp[1]);
//string[] temp = Vote.Text.Split('\n');


//Tips = new 弹窗提示(3, color3, 0, "");
//Tips.Show();
// using (BackgroundWorker bw = new BackgroundWorker())
// {
//    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
//   bw.DoWork += new DoWorkEventHandler(bw_DoWork);
//   bw.RunWorkerAsync("Tank");
//}

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
//private bool Dynamic = false;
//public event Func<string,string, int> Fcc1;//定义委托.刷新桌面歌词
//System.Timers.Timer t3 = new System.Timers.Timer(10);//实例化Timer类用于刷新歌词
//System.Timers.Timer t4 = new System.Timers.Timer(10);//实例化Timer类用于刷新歌词
//if (false == System.IO.Directory.Exists(System.IO.Path.GetTempPath() + @"方糖音乐\"))//判断目录是否存在
//{
//    //创建临时目录
//    System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + @"方糖音乐\");
//}
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
//}        ////删除文件夹
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
//        ////获得模板内控件属性
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
//MessageBox.Show("");
//System.Drawing.Image Dummy = System.Drawing.Image.FromFile(@"I:\方糖音乐缓存目录\清空-麦小兜-000XtdRd1kwYrR.jpg");
//Dummy.Save(@"I:\方糖音乐缓存目录\清空-麦小兜-000XtdRd1kwYrR.png", System.Drawing.Imaging.ImageFormat.Bmp);
//System.Drawing.Image Dummy = System.Drawing.Image.FromFile(Album_path);
//MessageBox.Show(System.IO.Path.GetDirectoryName(Album_path) + @"\" + System.IO.Path.GetFileNameWithoutExtension(Album_path) + ".png");
//((ListBoxItem)歌词滚动显示.Items[i]).FontSize = 18;//字体放大
//DoubleAnimation doubleAnimation = new DoubleAnimation();
////设置From属性。
//doubleAnimation.From = 15;
////设置To属性。
//doubleAnimation.To = 16;
////设置Duration属性。
//doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
////为元素设置BeginAnimation方法。
//((ListBoxItem)歌词滚动显示.Items[i]).BeginAnimation(ListBoxItem.FontSizeProperty , doubleAnimation);
//Dummy.Save(System.IO.Path.GetDirectoryName(Album_path) + @"\" + System.IO.Path.GetFileNameWithoutExtension(Album_path) + ".png", System.Drawing.Imaging.ImageFormat.Bmp);