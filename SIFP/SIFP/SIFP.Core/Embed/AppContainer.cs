using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace SIFP.Core.Embed
{
    public class AppContainer : ContentControl
    {
#if false
        private WindowsFormsHost _winFormHost;
        private System.Windows.Forms.Panel _hostPanel;
        private Process _process = null;

        public AppContainer()
        {
            var res = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/PowerPixel;component/Embed/AppContainer.xaml")
            };
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(res);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _winFormHost = GetTemplateChild("PART_Host") as WindowsFormsHost;
            if (_winFormHost != null)
            {
                _hostPanel = new System.Windows.Forms.Panel();
                _winFormHost.Child = _hostPanel;
                _hostPanel.Resize += _hostPanel_Resize;
            }
        }

        private void _hostPanel_Resize(object sender, EventArgs e)
        {
            SetBounds();
        }

        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    if (_process != null)
        //    {
        //        Win32Api.MoveWindow(_process.MainWindowHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
        //    }

        //    base.OnRender(drawingContext);
        //}

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    InvalidateVisual();
        //    base.OnRenderSizeChanged(sizeInfo);
        //}

        public bool StartAndEmbedProcess(string processPath)
        {
            if (null != _process)
                return true;

            ProcessStartInfo info = new ProcessStartInfo(processPath);
            info.WindowStyle = ProcessWindowStyle.Maximized;//默认最小化到任务栏，不弹出界面。
            info.Arguments = "-popupwindow";//Unity的命令行参数

            _process = Process.Start(info);
            if (_process == null)
                return false;

            _process.WaitForInputIdle();

            return EmbedApp();
        }

        /// <summary>
        /// 将外进程嵌入到当前程序
        /// </summary>
        /// <param name="process"></param>
        private bool EmbedApp()
        {
            //是否嵌入成功标志，用作返回值
            var isEmbedSuccess = false;

            //进程句柄
            while (true)
            {
                if (_process.MainWindowHandle != (IntPtr)0)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            //容器句柄
            var panelHwnd = _hostPanel.Handle;

            if (_process.MainWindowHandle != IntPtr.Zero)
            {
                var processHwnd = _process.MainWindowHandle;
                while (!isEmbedSuccess)
                {
                    // Put it into this form
                    isEmbedSuccess = Win32Api.SetParent(processHwnd, panelHwnd) != 0;
                    Thread.Sleep(10);
                }
                SetBounds();
            }
            return isEmbedSuccess;
        }

        public void SetBounds()
        {
            SetBounds(_hostPanel.Width, _hostPanel.Height);
        }

        public void SetBounds(int width, int height)
        {
            if (_process == null)
                return;

            if (_process.MainWindowHandle == IntPtr.Zero)
                return;

            if (width <= 0 || height <= 0)
                return;

            Win32Api.MoveWindow(_process.MainWindowHandle, 0, 0, width, height, false);

            ActivateWindow();//激活
        }

        public void ActivateWindow()
        {
            if (_process == null)
                return;

            if (_process.MainWindowHandle == IntPtr.Zero)
                return;

            Win32Api.SendMessage(_process.MainWindowHandle, Win32Api.WM_ACTIVATE, Win32Api.WA_ACTIVE, IntPtr.Zero);
        }

        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="process"></param>
        private void CloseApp(Process process)
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();//这种情况下unity不会触发destroy事件或者applicationquit事件
            }
        }

        public void CloseProcess()
        {
            CloseApp(_process);
            _process = null;
        }
#else

        private WindowsFormsHost _winFormHost;
        private System.Windows.Forms.Panel _hostPanel;
        private readonly ManualResetEvent _eventDone = new ManualResetEvent(false);
        private Process _process = null;
        internal IntPtr _embededWindowHandle;

        public AppContainer()
        {
            var res = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/SIFP.Core;component/Embed/AppContainer.xaml")
            };
            Application.Current.Resources.MergedDictionaries.Add(res);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _winFormHost = GetTemplateChild("PART_Host") as WindowsFormsHost;
            if (_winFormHost != null)
            {
                _hostPanel = new System.Windows.Forms.Panel();
                _hostPanel.AutoSize = true;
                _winFormHost.Child = _hostPanel;
                _hostPanel.Resize += _hostPanel_Resize;
            }
        }

        private void _hostPanel_Resize(object sender, EventArgs e)
        {
            SetBounds();
        }

        public void ActivateWindow()
        {
            if (_process == null)
                return;

            if (_process.MainWindowHandle == IntPtr.Zero)
                return;

            Win32Api.SendMessage(_process.MainWindowHandle, Win32Api.WM_ACTIVATE, Win32Api.WA_ACTIVE, IntPtr.Zero);
        }

        public void SetBounds()
        {
            SetBounds(_hostPanel.Width, _hostPanel.Height);
        }

        public void SetBounds(int width, int height)
        {
            if (_process == null)
                return;

            if (_process.MainWindowHandle == IntPtr.Zero)
                return;

            if (width <= 0 || height <= 0)
                return;

            Win32Api.MoveWindow(_process.MainWindowHandle, 0, 0, width, height, true);

            ActivateWindow();//激活
        }

        public bool StartAndEmbedProcess(string processPath, string arg)
        {
            if (null != _process)
                return true;

            var isStartAndEmbedSuccess = false;
            _eventDone.Reset();

            // Start the process
            ProcessStartInfo info = new ProcessStartInfo(processPath);
            info.WindowStyle = ProcessWindowStyle.Maximized;//默认最大化，不弹出界面。
            info.Arguments = $"-popupwindow {arg}";//Unity的命令行参数

            _process = Process.Start(info);

            if (_process == null)
            {
                return false;
            }

            // Wait for process to be created and enter idle condition
            _process.WaitForInputIdle();

            // Get the main handle
            var thread = new Thread(() =>
            {
                while (true)
                {
                    if (_process.MainWindowHandle != (IntPtr)0)
                    {
                        _eventDone.Set();
                        break;
                    }
                    Thread.Sleep(10);
                }
            });
            thread.Start();

            //嵌入进程
            if (_eventDone.WaitOne(10000))
            {
                isStartAndEmbedSuccess = EmbedApp(_process);
                if (!isStartAndEmbedSuccess)
                {
                    CloseApp(_process);
                }
            }
            return isStartAndEmbedSuccess;
        }

        public bool EmbedExistProcess(Process process)
        {
            _process = process;
            return EmbedApp(process);
        }

        /// <summary>
        /// 将外进程嵌入到当前程序
        /// </summary>
        /// <param name="process"></param>
        private bool EmbedApp(Process process)
        {
            //是否嵌入成功标志，用作返回值
            var isEmbedSuccess = false;
            //外进程句柄
            var processHwnd = process.MainWindowHandle;
            //容器句柄
            IntPtr panelHwnd=IntPtr.Zero;
            Application.Current.Dispatcher.Invoke(() =>
              panelHwnd = _hostPanel.Handle
            );
            if (processHwnd != (IntPtr)0 && panelHwnd != (IntPtr)0)
            {
                //把本窗口句柄与目标窗口句柄关联起来
                var setTime = 0;
                while (!isEmbedSuccess && setTime < 50)
                {
                    // Put it into this form
                    isEmbedSuccess = Win32Api.SetParent(processHwnd, panelHwnd) != 0;
                    Thread.Sleep(10);
                    setTime++;
                }

                // Remove border and whatnot
                //Win32Api.SetWindowLong(processHwnd, Win32Api.GWL_STYLE, Win32Api.WS_CHILDWINDOW | Win32Api.WS_CLIPSIBLINGS | Win32Api.WS_CLIPCHILDREN | Win32Api.WS_VISIBLE);

                SetBounds();

                //// Move the window to overlay it on this window
                //Win32Api.MoveWindow(_process.MainWindowHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
            }

            if (isEmbedSuccess)
            {
                _embededWindowHandle = _process.MainWindowHandle;
            }

            return isEmbedSuccess;
        }

        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    if (_process != null)
        //    {
        //        Win32Api.MoveWindow(_process.MainWindowHandle, 0, 0, _hostPanel.Width, _hostPanel.Height, true);
        //    }

        //    base.OnRender(drawingContext);
        //}

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    InvalidateVisual();
        //    base.OnRenderSizeChanged(sizeInfo);
        //}

        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="process"></param>
        private void CloseApp(Process process)
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
            }
        }

        public void CloseProcess()
        {
            CloseApp(_process);
            _process = null;
        }

#endif
    }
}
