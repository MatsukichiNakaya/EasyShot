using System.Drawing;
using System.Windows.Forms;
using System;
using Project.Windows;
using System.Runtime.InteropServices;

namespace Project.Drawing
{
    /// <summary>
    /// スクリーンショット撮影クラス
    /// </summary>
    public class PrintScreen : CaptureAPI
    {
        /// <summary>
        /// プライマリスクリーンの画像を取得する
        /// </summary>
        /// <returns>プライマリスクリーンの画像</returns>
        public static Bitmap CaptureScreen()
        {
            //プライマリモニタのデバイスコンテキストを取得
            IntPtr disDC = NativeMethod.GetDC(IntPtr.Zero);

            //Bitmapの作成
            var bmp = new Bitmap(
                Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);

            //Graphicsの作成
            using (var g = Graphics.FromImage(bmp))
            {
                //Graphicsのデバイスコンテキストを取得
                IntPtr hDC = g.GetHdc();

                //Bitmapに画像をコピーする
                NativeMethod.BitBlt(hDC, 0, 0, bmp.Width, bmp.Height,
                        disDC, 0, 0, NativeMethod.SRCCOPY);
                //解放
                g.ReleaseHdc(hDC);
            }
            NativeMethod.ReleaseDC(IntPtr.Zero, disDC); //32bit

            return bmp;
        }

        /// <summary>
        /// アクティブなウィンドウの画像を取得する
        /// </summary>
        /// <returns>アクティブなウィンドウの画像</returns>
        public static Bitmap CaptureActiveWindow()
        {
            //アクティブなウィンドウのデバイスコンテキストを取得
            IntPtr hWnd = WindowAPIWrapper.GetForegroundWindowHandle();
            IntPtr hWinDC = NativeMethod.GetWindowDC(hWnd);

            //ウィンドウの大きさを取得
            var winRect = new Rect();
            NativeMethod.GetWindowRect(hWnd, ref winRect);

            //Bitmapの作成
            var bmp = new Bitmap(winRect.Right - winRect.Left,
                winRect.Bottom - winRect.Top);

            //Graphicsの作成
            using (var g = Graphics.FromImage(bmp))
            {
                //Graphicsのデバイスコンテキストを取得
                IntPtr hDC = g.GetHdc();

                //Bitmapに画像をコピーする
                NativeMethod.BitBlt(hDC, 0, 0, bmp.Width, bmp.Height,
                    hWinDC, 0, 0, NativeMethod.SRCCOPY);

                //解放
                g.ReleaseHdc(hDC);
            }
            NativeMethod.ReleaseDC(hWnd, hWinDC);

            return bmp;
        }
        
        /// <summary>
        /// Windows10のドロップシャドウ対策入りのキャプチャ
        /// </summary>
        /// <returns></returns>
        public static Bitmap CaptureActiveWindow10()
        {
            //アクティブなウィンドウのデバイスコンテキストを取得
            IntPtr hWnd = WindowAPIWrapper.GetForegroundWindowHandle();
            IntPtr hWinDC = NativeMethod.GetWindowDC(hWnd);

            //ウィンドウの大きさを取得
            var winRect = new Rect();
            Rect bounds;
            NativeMethod.DwmGetWindowAttribute(hWnd,
                DWMWINDOWATTRIBUTE.DWMWA_EXTENDED_FRAME_BOUNDS,
                out bounds, Marshal.SizeOf(typeof(Rect)));

            NativeMethod.GetWindowRect(hWnd, ref winRect);
            //Bitmapの作成
            var offsetX = bounds.Left - winRect.Left;
            var offsetY = bounds.Top - winRect.Top;
            var bmp = new Bitmap(bounds.Right - bounds.Left,
                bounds.Bottom - bounds.Top);

            //Graphicsの作成
            using (var g = Graphics.FromImage(bmp))
            {
                //Graphicsのデバイスコンテキストを取得
                IntPtr hDC = g.GetHdc();
                //Bitmapに画像をコピーする
                Int32 bmpWidth = bmp.Width;
                Int32 bmpHeight = bmp.Height;
                Console.WriteLine(winRect);
                NativeMethod.BitBlt(hDC, 0, 0, bmpWidth, bmpHeight,
                    hWinDC, offsetX, offsetY, NativeMethod.SRCCOPY);
                //解放
                g.ReleaseHdc(hDC);
            }
            NativeMethod.ReleaseDC(hWnd, hWinDC);
            return bmp;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class WindowAPIWrapper : WindowAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetForegroundWindowHandle()
        {
            return NativeMethod.GetForegroundWindow();
        }
    }
}
