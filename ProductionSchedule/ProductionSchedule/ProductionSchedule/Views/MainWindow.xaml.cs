using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ProductionSchedule.ViewModels;
using ProductionSchedule.Properties;

namespace ProductionSchedule.Views
{

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModels.MainViewModel VM;

        public MainWindow()
        {
            InitializeComponent();
            VM = new ViewModels.MainViewModel();
            this.DataContext = VM;
            this.Loaded += this_loaded;
        }
      
        /// <summary>
        /// 読込み終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_loaded(object sender, RoutedEventArgs e)
        {
            //ViewとViewModelの紐付け
            VM.MyView = this;
       //     VM.CalenderGR = this.CalenderGR;
            VM.CalenderDG = this.CalenderDG;
            // ウィンドウのサイズを復元
            RecoverWindowBounds();
            VM.MainWindowWidth = this.Width;
        }

        /// <summary>
        /// ウィンドウの位置・サイズを復元します。
        /// </summary>
        void RecoverWindowBounds()
        {
            string TAG = "RecoverWindowBounds";
            string dbMsg = "";
            try
            {
                var settings = Settings.Default;
                dbMsg += "(" + settings.WindowLeft + "," + settings.WindowTop + ")";
                // 左
                if (settings.WindowLeft >= 0 &&
                    (settings.WindowLeft + settings.WindowWidth) < SystemParameters.VirtualScreenWidth)
                {
                    Left = settings.WindowLeft;
                }
                // 上
                if (settings.WindowTop >= 0 &&
                    (settings.WindowTop + settings.WindowHeight) < SystemParameters.VirtualScreenHeight) { Top = settings.WindowTop; }
                dbMsg += "[" + settings.WindowWidth + "×" + settings.WindowHeight + "]" + settings.WindowMaximized;

                // 幅
                double MainWindowWidth = this.Width;
                if (settings.WindowWidth > 0 &&
                    settings.WindowWidth <= SystemParameters.WorkArea.Width) { Width = settings.WindowWidth; }
                // 高さ
                if (settings.WindowHeight > 0 &&
                    settings.WindowHeight <= SystemParameters.WorkArea.Height) { Height = settings.WindowHeight; }
                // 最大化
                if (settings.WindowMaximized)
                {
                    // ロード後に最大化
                    Loaded += (o, e) => WindowState = WindowState.Maximized;
                }
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }

        }

        /// <summary>
        /// ウィンドウの位置・サイズを保存します。
        /// </summary>
        void SaveWindowBounds()
        {
            string TAG = "SaveWindowBounds";
            string dbMsg = "";
            try
            {
                dbMsg += "(" + Left+ "," + Top + ")[" + Width + "×" + Height + "]" + WindowState;
                var settings = Settings.Default;
                settings.WindowMaximized = WindowState == WindowState.Maximized;
                WindowState = WindowState.Normal; // 最大化解除
                settings.WindowLeft = Left;
                settings.WindowTop = Top;
                settings.WindowWidth = Width;
                settings.WindowHeight = Height;
                settings.Save();
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // ウィンドウのサイズを保存
            string TAG = "Window_Closing";
            string dbMsg = "";
            try
            {
                SaveWindowBounds();
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }


        ////////////////////////////////////////////////////////////////
        public static void MyLog(string TAG, string dbMsg)
        {
            dbMsg = "[MainWindow ]" + dbMsg;
            //dbMsg = "[" + MethodBase.GetCurrentMethod().Name + "]" + dbMsg;
            CS_Util Util = new CS_Util();
            Util.MyLog(TAG, dbMsg);
        }

        public static void MyErrorLog(string TAG, string dbMsg, Exception err)
        {
            dbMsg = "[MainWindow ]" + dbMsg;
            CS_Util Util = new CS_Util();
            Util.MyErrorLog(TAG, dbMsg, err);
        }

        public MessageBoxResult MessageShowWPF(String titolStr, String msgStr,
                                                                        MessageBoxButton buttns,
                                                                        MessageBoxImage icon
                                                                        )
        {
            CS_Util Util = new CS_Util();
            return Util.MessageShowWPF(msgStr, titolStr, buttns, icon);
        }

    }
}
