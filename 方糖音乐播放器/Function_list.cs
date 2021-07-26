using System;
using System.Collections;
using System.Diagnostics;
//using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using System.Web.UI.WebControls;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using 方糖音乐播放器.Properties;

namespace 方糖音乐播放器
{
    public static class Function_list
    {
        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public static int CopyFolder2(string sourceFolder, string destFolder)
        {
            try
            {
                string folderName = System.IO.Path.GetFileName(sourceFolder);
                string destfolderdir = System.IO.Path.Combine(destFolder, folderName);
                string[] filenames = System.IO.Directory.GetFileSystemEntries(sourceFolder);
                foreach (string file in filenames)// 遍历所有的文件和目录
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        string currentdir = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(currentdir))
                        {
                            System.IO.Directory.CreateDirectory(currentdir);
                        }
                        CopyFolder2(file, destfolderdir);
                    }
                    else
                    {
                        string srcfileName = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(destfolderdir))
                        {
                            System.IO.Directory.CreateDirectory(destfolderdir);
                        }
                        System.IO.File.Copy(file, srcfileName);
                    }
                }

                return 1;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
                return 0;
            }

        }
        /// <summary>
        /// MD5校验工具
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail, error:" +ex.Message);
            }
        }

        public static bool Document_verification()
        {
            //将自己复制到系统临时文件目录
            //File.Copy(Process.GetCurrentProcess().MainModule.FileName, System.Environment.GetEnvironmentVariable("TMP") + @"\校验.temp", true);
            //string Check_value = GetMD5HashFromFile(System.Environment.GetEnvironmentVariable("TMP") + @"\校验.temp.");
            //MessageBox.Show(Check_value);
            try
            {
                X509Certificate cert = X509Certificate.CreateFromSignedFile(Process.GetCurrentProcess().MainModule.FileName);
                string f = cert.GetCertHashString();
                //MessageBox.Show(cert.GetCertHashString());
                if (f == "36A888B9F2A505BF92AC6B2796C2188E639AB1D1")
                {
                    //MessageBox.Show("成功");
                    return true;
                }
                else
                {
                    //MessageBox.Show("失败");
                    return false;
                }

            }
            catch
            {
                //MessageBox.Show("错误");
                return false;
            }
            
            //MessageBox.Show(cert.GetCertHashString());

        }


        /// <summary>
        /// 秒转时间
        /// </summary>
        /// <param name="duration">单位秒</param>
        /// <returns>返回“00：00：00”格式的时间</returns>
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
        /// <summary>
        /// 强制回收内存
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourse">目标字符串</param>
        /// <param name="startstr">从哪里</param>
        /// <param name="endstr">到这里</param>
        /// <returns>返回不包含传入参数的字符串</returns>
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
        /// <summary>
        /// 图片转bitmapImage对象
        /// </summary>
        /// <param name="file">图片路径</param>
        /// <returns>返回bitmapImage对象</returns>
        public static BitmapImage Album_pictures(string file)
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
        /// <summary>
        /// 读取专辑图片
        /// 有些图片格式不支持会导致NET框架内部出错，会自动将图片转换为.png格式的图片文件
        /// </summary>
        /// <param name="path">歌曲路径</param>
        /// <returns></returns>
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
        /// <summary>
        /// 填充ListBox菜单
        /// </summary>
        /// <param name="box">要填充的ListBox对象</param>
        /// <param name="name">要显示的名称字符串</param>
        /// <param name="tooltip">鼠标悬浮提示</param>
        /// <param name="width">对象宽度</param>
        /// <param name="height">对象高度</param>
        /// <param name="fontsize">字体大小</param>
        /// <param name="Centered">是否文本内容上下左右居中，true 为居中，false 为不居中</param>
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
        /// <summary>
        /// 读取歌词函数
        /// </summary>
        /// <param name="file">歌词文件路径</param>
        /// <returns>返回格式化后的字符串</returns>
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
        /// <summary>
        /// 错误弹窗
        /// </summary>
        /// <param name="str">提示字符串</param>
        /// <param name="Tips">窗体对象</param>
        /// <param name="color3">主题颜色</param>
        [Obsolete]
        public  static void Error_capture(string str,弹窗提示 Tips,Color color3)
        {
            if (Properties.Settings.Default.错误报告 == true)//判断是否开启的错误报告
            {
                Tips = new 弹窗提示(4, color3, 0, str);
                Tips.ShowDialog();
            }
        }
        /// <summary>
        /// 单玩家夹扫描歌曲，并将他添加到ListBox上，还添加到歌曲目录数组内
        /// </summary>
        /// <param name="list">ListBox对象</param>
        /// <param name="file">文件夹路径</param>
        /// <param name="array">歌曲目录数组</param>
        /// <param name="Tips">窗体对象</param>
        /// <param name="color3">主题颜色</param>
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
        /// <summary>
        /// 全盘扫描歌曲
        /// 采用递归方式扫描
        /// 添加到ListBox控件内
        /// 添加到歌曲目录数组内
        /// </summary>
        /// <param name="list">要填的listbox对象</param>
        /// <param name="info">路径</param>
        /// <param name="array">歌曲目录数组</param>
        /// <param name="Tips">窗体对象</param>
        /// <param name="color3">错误提示</param>
        [Obsolete]
        public static void  ListFiles(ListBox list, FileSystemInfo info, ArrayList array, 弹窗提示 Tips, Color color3)
        {
            string Ext;
            string[] ssgb = { "mp3", "wav", "flac" };//设置过滤器
            for (int i = 0; i < ssgb .Length; i++)
            {
                Ext = ssgb[i];
                //目录不存在
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
