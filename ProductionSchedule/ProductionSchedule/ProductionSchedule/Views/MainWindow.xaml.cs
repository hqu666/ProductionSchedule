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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProductionSchedule.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        ////////////////////////////////////////////////////////////////
        public static void MyLog(string TAG, string dbMsg)
        {
#if DEBUG
            Console.WriteLine(TAG + "[CalcTestView]" + dbMsg);
#endif
        }

        public static void MyErrorLog(string TAG, string dbMsg, Exception err)
        {
            Console.WriteLine(TAG + "[CalcTestView]" + dbMsg + "でエラー発生;" + err);
        }


        public MessageBoxResult MessageShowWPF(String msgStr,
                                                                                String titolStr = null,
                                                                                MessageBoxButton buttns = MessageBoxButton.OK,
                                                                                MessageBoxImage icon = MessageBoxImage.None
                                                                                )
        {
            String TAG = "MessageShowWPF";
            String dbMsg = "開始";
            MessageBoxResult result = 0;
            try
            {
                dbMsg = "titolStr=" + titolStr;
                dbMsg += "mggStr=" + msgStr;
                //メッセージボックスを表示する		https://docs.microsoft.com/ja-jp/dotnet/api/system.windows.messagebox?view=netcore-3.1
                if (titolStr == null)
                {
                    result = MessageBox.Show(msgStr);
                }
                else if (icon == MessageBoxImage.None)
                {
                    result = MessageBox.Show(msgStr, titolStr, buttns);
                }
                else
                {
                    result = MessageBox.Show(msgStr, titolStr, buttns, icon);
                }
                dbMsg += ",result=" + result;
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyLog(TAG, dbMsg + "で" + er.ToString());
            }
            return result;
        }

    }
}
