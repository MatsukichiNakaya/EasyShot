using System;

namespace Project.Windows
{
    /// <summary>
    /// �E�C���h�E�X�^�C���̃I�v�V�������擾����
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

        // Todo ���낢��ȃE�C���h�E�ݒ�̃v���Z�b�g��`���쐬����

        // ��ɍőO��
        // ToTopmost()

        // ���������ɃE�B���h�E�����蔲����
        // ToSlipThrough()
    }
}