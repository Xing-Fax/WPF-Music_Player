using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace 方糖音乐播放器
{
    /// <summary>
    /// Wpf应用程序全屏辅助类
    /// 
    /// ——全部使用.Net类进行操作
    /// ——可进入全屏和退出全屏
    /// ——可恢复到全屏前的状态
    /// ——全屏时可切换到其他应用程序
    /// </summary>
    public static class FullScreenHelper
    {
        private static Window _fullWindow;
        private static WindowState _windowState;
        private static WindowStyle _windowStyle;
        private static bool _windowTopMost;
        private static ResizeMode _windowResizeMode;
        private static Rect _windowRect;

        /// <summary>
        /// 进入全屏
        /// </summary>
        /// <param name="window"></param>
        public static void GoFullscreen(this Window window)
        {
            //已经是全屏
            if (window.IsFullscreen()) return;
            //存储窗体信息
            _windowState = window.WindowState;
            _windowStyle = window.WindowStyle;
            _windowTopMost = window.Topmost;
            _windowResizeMode = window.ResizeMode;
            _windowRect.X = window.Left;
            _windowRect.Y = window.Top;
            _windowRect.Width = window.Width;
            _windowRect.Height = window.Height;
            //变成无边窗体
            window.WindowState = WindowState.Normal;//假如已经是Maximized，就不能进入全屏，所以这里先调整状态
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            window.Topmost = true;//最大化后总是在最上面
            //调整窗口最大化,全屏的关键代码就是下面3句
            window.MaxWidth = SystemParameters.PrimaryScreenWidth;
            window.MaxHeight = SystemParameters.PrimaryScreenHeight;
            window.WindowState = WindowState.Maximized;
            //解决切换应用程序的问题
            window.Activated += new EventHandler(window_Activated);
            window.Deactivated += new EventHandler(window_Deactivated);
            //记住成功最大化的窗体
            _fullWindow = window;
        }

        static void window_Deactivated(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Topmost = false;
        }

        static void window_Activated(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Topmost = true;
        }

        /// <summary>
        /// 退出全屏
        /// </summary>
        /// <param name="window"></param>
        public static void ExitFullscreen(this Window window)
        {
            //已经不是全屏无操作
            if (!window.IsFullscreen()) return;

            //恢复窗口先前信息，这样就退出了全屏
            window.Topmost = _windowTopMost;
            window.WindowStyle = _windowStyle;

            window.ResizeMode = ResizeMode.CanResize;//设置为可调整窗体大小
            window.Left = _windowRect.Left;
            window.Width = _windowRect.Width;
            window.Top = _windowRect.Top;
            window.Height = _windowRect.Height;

            window.WindowState = _windowState;//恢复窗口状态信息

            window.ResizeMode = _windowResizeMode;//恢复窗口可调整信息

            //移除不需要的事件
            window.Activated -= window_Activated;
            window.Deactivated -= window_Deactivated;

            _fullWindow = null;
        }

        /// <summary>
        /// 窗体是否在全屏状态
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static bool IsFullscreen(this Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }
            return _fullWindow == window;
        }
    }
}
