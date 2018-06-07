using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Project.Hook
{
    #region キーフッククラス
    public class LLKeyHook : HookAPI, IDisposable
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LLKeyHook() { }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~LLKeyHook() { Dispose(); }


        ///	<summary>
        ///	コールバック用
        ///	</summary>
        private LowLevelKeyboardProc callBack = null;

        ///	<summary>
        ///	フックＩＤ
        ///	</summary>
        private IntPtr hookId = IntPtr.Zero;

        ///	<summary>
        ///	キーフックを開始します。
        ///	</summary>
        ///	<returns></returns>
        public void Set()
        {
            // コールバック用のデリゲートを作成。
            if (this.callBack == null)
            {
                this.callBack = new LowLevelKeyboardProc(this.MyCallBack);
                IntPtr hMod = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
                // 低レベルキーフックに登録。
                this.hookId = NativeMethod.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, this.callBack, hMod, 0);
            }
        }

        ///	<summary>
        ///	キーフックを終了します。
        ///	</summary>
        public void UnSet()
        {
            if (this.hookId != IntPtr.Zero)
            {
                for (Int32 i = 0; i < 5; i++)
                {
                    if (NativeMethod.UnhookWindowsHookEx(this.hookId))
                    {
                        break;
                    }
                }
                this.hookId = IntPtr.Zero;
            }
        }

        #region	キーフック関連
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public Int32 MyCallBack(Int32 code, WindowsMessages wParam, KBDLLHOOKSTRUCT lParam)
        {
            if (code < 0)
            {
                return NativeMethod.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
            }

            Int32 key;
            switch (wParam)
            {
                case WindowsMessages.WM_IME_KEYDOWN:
                case WindowsMessages.WM_KEYDOWN:
                case WindowsMessages.WM_SYSKEYDOWN:
                    key = lParam.vkCode;
                    if (KeyDown == null) { break; }
                    KeyDown(key);
                    break;
                case WindowsMessages.WM_IME_KEYUP:
                case WindowsMessages.WM_KEYUP:
                case WindowsMessages.WM_SYSKEYUP:
                    key = lParam.vkCode;
                    if(KeyUp == null) { break; }
                    KeyUp(key);
                    break;
            }
            // キーの処理を次に回して完了。
            return NativeMethod.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
        #endregion

        /// <summary></summary>
        public event KeyEvent KeyDown;
        /// <summary></summary>
        public event KeyEvent KeyUp;

        #region IDisposable メンバ
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() { UnSet(); }
        #endregion
    }
    #endregion
}