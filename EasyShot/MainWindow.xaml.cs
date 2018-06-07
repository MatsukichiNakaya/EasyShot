using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Project.Drawing;
using Project.Hook;
using Project.Serialization.Xml;

namespace EasyShot
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private LLKeyHook keyhook;
        private Int32 _closeCount = 0;

        private const Int32 PRINT_MODE = 0;
        private const Int32 DISP_MODE = 1;

        private Config Conf { get; set; }
        private Storyboard boardDisp;
        private Storyboard boardFlash;

        public MainWindow()
        {
            InitializeComponent();

            this.Conf = XmlSerializer.Load<Config>(@".\Configure.xml");

            this.keyhook = new LLKeyHook();
            this.keyhook.Set();
            this.keyhook.KeyDown += new KeyEvent(KeyDownEvet);
            this.keyhook.KeyUp += new KeyEvent(KeyUpEvet);

            var hide = new DoubleAnimation();
            Storyboard.SetTarget(hide, this.DispButton);
            Storyboard.SetTargetProperty(hide, new PropertyPath("(Button.Opacity)"));
            hide.From = 1;
            hide.To = 0;
            hide.Duration = TimeSpan.FromSeconds(2);
            this.boardDisp = new Storyboard();
            this.boardDisp.Children.Add(hide);

            var flash = new DoubleAnimation();
            Storyboard.SetTarget(flash, this.MainGrid);
            Storyboard.SetTargetProperty(flash, new PropertyPath("(Grid.Opacity)"));
            flash.From = 1;
            flash.To = 0;
            flash.Duration = TimeSpan.FromMilliseconds(500);
            this.boardFlash = new Storyboard();
            this.boardFlash.Children.Add(flash);
        }

        // フックしたキーを受け取るメソッド
        public void KeyDownEvet(Int32 key)
        {
            switch (key)
            {
                case 27:
                    this._closeCount++;
                    if (2 < this._closeCount)
                    {
                        this.Close();
                    }
                    break;
                default:
                    if (this.Conf.BootMode == 0)
                    {
                        if (key == this.Conf.PrintKeyCode)
                        {
                            if (!System.IO.Directory.Exists(this.Conf.SaveDir)) { return; }
                            PrintScreen.CaptureActiveWindow()
                                .Save(System.IO.Path.Combine(System.IO.Path.GetFullPath(this.Conf.SaveDir), 
                                      String.Format(@"{0}.png", DateTime.Now.ToString("yyyyMMddHHmmss"))),
                                      System.Drawing.Imaging.ImageFormat.Png);
                            if (this.Conf.Effect == 1)
                            {
                                this.boardFlash.Begin();
                            }
                        }
                    }
                    else
                    {
                        this.DispButton.Content = key;
                        this.boardDisp.Begin();
                    }

                    this._closeCount = 0;
                    break;
            }
        }

        public void KeyUpEvet(Int32 key) { }

        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            this.WindowState = WindowState.Maximized;
        }

        private void Window_Closing(Object sender, System.ComponentModel.CancelEventArgs e)
        {
            // キーフック解除
            this.keyhook.UnSet();
        }
    }
}
