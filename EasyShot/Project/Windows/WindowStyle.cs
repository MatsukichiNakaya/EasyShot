using System;

namespace Project.Windows
{
    /// <summary>
    /// ウインドウスタイルのオプションを取得する
    /// </summary>
    public class WindowStyle : WindowStyleAPI
    {
        public static UInt32 GetLong(IntPtr hWnd, GWL index)
        {
            return NativeMethod.GetWindowLong(hWnd, index);
        }

        public static UInt32 SetLong(IntPtr hWnd, GWL index, UInt32 newLong)
        {
            return NativeMethod.SetWindowLong(hWnd, index, newLong);
        }

        // Todo いろいろなウインドウ設定のプリセット定義を作成する

        // 常に最前面
        // ToTopmost()

        // 半透明時にウィンドウをすり抜ける
        // ToSlipThrough()
    }
}