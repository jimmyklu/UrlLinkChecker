using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace UrlLinkChecker
{
    #region WM - Window Messages
    public enum WM
    {
        WM_NULL = 0x0000,
        WM_CREATE = 0x0001,
        WM_DESTROY = 0x0002,
        WM_MOVE = 0x0003,
        WM_SIZE = 0x0005,
        WM_ACTIVATE = 0x0006,
        WM_SETFOCUS = 0x0007,
        WM_KILLFOCUS = 0x0008,
        WM_ENABLE = 0x000A,
        WM_SETREDRAW = 0x000B,
        WM_SETTEXT = 0x000C,
        WM_GETTEXT = 0x000D,
        WM_GETTEXTLENGTH = 0x000E,
        WM_PAINT = 0x000F,
        WM_CLOSE = 0x0010,
        WM_QUERYENDSESSION = 0x0011,
        WM_QUIT = 0x0012,
        WM_QUERYOPEN = 0x0013,
        WM_ERASEBKGND = 0x0014,

    }
    #endregion

    #region RECT
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    #endregion


    
    public class CustomListView : ListView
    {
        bool updating;
        int itemnumber;

        #region Imported User32.DLL functions
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool ValidateRect(IntPtr handle, ref RECT rect);
        #endregion


        public CustomListView()
            : base()
        {
        }

        public void UpdateItem(int iIndex)
        {
            updating = true;
            itemnumber = iIndex;
            this.Update();
            updating = false;
        }


        public void DelayRefresh()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        protected override void WndProc(ref Message messg)
        {
            if (updating)
            {
                if ((int)WM.WM_ERASEBKGND == messg.Msg)
                    messg.Msg = (int)WM.WM_NULL;
                else if ((int)WM.WM_PAINT == messg.Msg)
                {
                    RECT vrect = this.GetWindowRECT();
                    ValidateRect(this.Handle, ref vrect);
                    Invalidate(this.Items[itemnumber].Bounds);
                }
            }
            base.WndProc(ref messg);
        }

        #region private helperfunctions

        private RECT GetWindowRECT()
        {
            RECT rect = new RECT();
            rect.left = this.Left;
            rect.right = this.Right;
            rect.top = this.Top;
            rect.bottom = this.Bottom;
            return rect;
        }

        #endregion
    }
}
