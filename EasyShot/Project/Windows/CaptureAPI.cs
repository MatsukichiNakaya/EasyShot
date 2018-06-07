using System;
using System.Runtime.InteropServices;

namespace Project.Windows
{
    /// <summary></summary>
    public class CaptureAPI
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public Int32 Left;
            public Int32 Top;
            public Int32 Right;
            public Int32 Bottom;
        }

        public enum DWMWINDOWATTRIBUTE : UInt32
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        }

        protected class NativeMethod
        {
            public const Int32 SRCCOPY = 13369376;
            public const Int32 CAPTUREBLT = 1073741824;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="hWnd"></param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            public static extern IntPtr GetDC(IntPtr hWnd);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="hDestDC"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="Width"></param>
            /// <param name="Height"></param>
            /// <param name="hSrcDC"></param>
            /// <param name="xSrc"></param>
            /// <param name="ySrc"></param>
            /// <param name="dwRop"></param>
            /// <returns></returns>
            [DllImport("gdi32.dll")]
            public static extern Int32 BitBlt(
                IntPtr hDestDC,
                Int32 x, Int32 y, Int32 Width, Int32 Height,
                IntPtr hSrcDC,
                Int32 xSrc, Int32 ySrc,
                Int32 dwRop);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="hWnd"></param>
            /// <param name="hdc"></param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hdc);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="hwnd"></param>
            /// <param name="lpRect"></param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            public static extern Int32 GetWindowRect(IntPtr hwnd, ref Rect lpRect);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="hwnd"></param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hwnd);


            [DllImport("dwmapi.dll")]
            public static extern Int32 DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute,
                                                             out Rect pvAttribute, Int32 cbAttribute);

        }
    }
}