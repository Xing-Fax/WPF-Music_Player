using System;
using System.Collections;
//using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using 方糖音乐播放器.Properties;

namespace 方糖音乐播放器
{
    public static class Function_list
    {
        //秒转分钟函数--秒
        public static string sec_to_hms(double duration)
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
        //立即回收内存
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        public static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        //截取字符串--目标--从这里--到这里
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
        //图片转bitmapImage对象，防止图片被占用
        public  static BitmapImage Album_pictures(string file)
        {
            if (file != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                try
                {
                    bitmapImage.UriSource = new Uri(file);//图片的全路径
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    return bitmapImage;
                }
                catch { return null; }
            }
            else { return null; }
        }
        //读取专辑文件
        public static BitmapImage GetCover(string path)
        {
            BitmapImage bmp = new BitmapImage();
            TagLib.File f = TagLib.File.Create(path);
            try
            {
                if (f.Tag.Pictures != null && f.Tag.Pictures.Length != 0)
                {
                    //字节流转BitmapImage对象
                    bmp.BeginInit();
                    bmp.StreamSource = new MemoryStream(f.Tag.Pictures[0].Data.Data);
                    bmp.EndInit();
                    return bmp;//返回图片对象
                }
            }
            catch //(Exception ex)如果图片格式不支持
            {
                FileStream fs = null;
                try
                {//将图片写入到新的路径
                    fs = new FileStream(System.IO.Path.GetTempPath() + "临时文件.png", FileMode.Create, FileAccess.Write);
                    fs.Write(f.Tag.Pictures[0].Data.Data, 0, f.Tag.Pictures[0].Data.Data.Length);
                }
                catch { }
                finally
                {
                    fs.Close();//释放文件句柄
                    fs.Dispose();
                }//图片格式转换
                System.Drawing.Image Dummy = System.Drawing.Image.FromFile(System.IO.Path.GetTempPath() + "临时文件.png");
                Dummy.Save(System.IO.Path.GetDirectoryName(path) + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + ".png", System.Drawing.Imaging.ImageFormat.Bmp);
                bmp = Function_list.Album_pictures(System.IO.Path.GetDirectoryName(path) + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + ".png");
                Dummy.Dispose();
                System.IO.File.Delete(System.IO.Path.GetDirectoryName(path) + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + ".png");
                System.IO.File.Delete(System.IO.Path.GetTempPath() + "临时文件.png");
                return bmp;//返回图片对象
            }
            return null;
        }
        //填充菜单-要填充的对象--名称--鼠标悬浮提示--对象宽度--对象高度--字体大小--文本内容上下居中
        public static void 填充菜单(ListBox box, string name, string tooltip, int width, int height, int fontsize, bool Centered)
        {
            ListBoxItem item = new ListBoxItem();
            item.Content = name;//设置显示名称
            item.Width = width;//设置宽度
            item.Height = height;//设置高度
            //item.FontFamily fontFamily = "";
            item.FontSize = fontsize;//设置字号
            item.ToolTip = tooltip;//设置提示
            //item.Focusable = false;
            if (Centered == true) { item.HorizontalContentAlignment = HorizontalAlignment.Center; }//设置文本上下左右居中
            box.Items.Add(item);//将控件添加到集合里
        }
        //读取歌词函数--歌词文件路径
        public  static string 读取歌词(string file)
        {
            string msg;
            string lrc = "";
            using (StreamReader reader = new StreamReader(file, Encoding.Default))
            {
                while ((msg = reader.ReadLine()) != null) { lrc += msg + "\n"; }//!=  不等于   
            }
            return lrc;
        }
        //错误弹窗
        [Obsolete]
        public  static void Error_capture(string str,弹窗提示 Tips,Color color3)
        {
            if (Properties.Settings.Default.错误报告 == true)
            {
                Tips = new 弹窗提示(4, color3, 0, str);
                Tips.ShowDialog();
            }
        }
        //单文件夹扫描歌曲
        [Obsolete]//--要填充的控件--路径--歌曲目录数组--错误弹窗对象--主题颜色
        public static void  query(ListBox list ,string file, ArrayList array, 弹窗提示 Tips, Color color3)
        {
            string[] ssg = { "*.mp3", "*.wav", "*.flac" };//设置过滤器
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    string[] dirs = Directory.GetFiles(file + @"\", ssg[i]);
                    foreach (string dir in dirs)
                    {
                        array.Add(dir);
                        填充菜单(list, System.IO.Path.GetFileNameWithoutExtension(dir), System.IO.Path.GetFileNameWithoutExtension(dir), 315, 40, 16, false);
                    }
                }
                catch (Exception ex)
                {
                    Error_capture("扫描歌曲时发生异常,异常详细信息:\n" + ex.ToString(), Tips, color3);
                }
            }
        }
        //全盘递归扫描歌曲
        [Obsolete]//--要填充的控件--路径--歌曲目录数组--错误弹窗对象--主题颜色
        public static void  ListFiles(ListBox list, FileSystemInfo info, ArrayList array, 弹窗提示 Tips, Color color3)
        {
            string Ext;
            string[] ssgb = { "mp3", "wav", "flac" };//设置过滤器
            for (int i = 0; i < 3; i++)
            {
                Ext = ssgb[i];
                if (!info.Exists) return ;
                DirectoryInfo dir = info as DirectoryInfo;
                //不是目录 
                if (dir == null) return ;
                try
                {
                    FileSystemInfo[] files = dir.GetFileSystemInfos();
                    for (int j = 0; j < files.Length; j++)
                    {
                        //是文件
                        if (files[j] is FileInfo file && file.Extension.ToUpper() == "." + Ext.ToUpper())
                        {
                            int id = Array.IndexOf(array.ToArray(), file.FullName);
                            if (id == -1)
                            {
                                填充菜单(list, System.IO.Path.GetFileNameWithoutExtension(file.FullName), System.IO.Path.GetFileNameWithoutExtension(file.FullName), 315, 40, 16, false);
                                array.Add(file.FullName);
                            }
                        }
                        //对于子目录，进行递归调用 
                        else ListFiles(list, files[j],array ,Tips,color3);
                    }
                }
                catch (Exception ex)
                {
                    Error_capture("扫描歌曲时发生异常，异常详细信息:\n" + ex.ToString(), Tips, color3);
                }
            }
        }
    }
}
