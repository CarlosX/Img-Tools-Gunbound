using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ImgTools
{
    public class FileIcon : IDisposable
    {

        [Flags]
        public enum SHGetFileInfoConstants
        {
            SHGFI_ICON = 0x100,
            SHGFI_DISPLAYNAME = 0x200,
            SHGFI_TYPENAME = 0x400,
            SHGFI_ATTRIBUTES = 0x800,
            SHGFI_ICONLOCATION = 0x1000,
            SHGFI_EXETYPE = 0x2000,
            SHGFI_SYSICONINDEX = 0x4000,
            SHGFI_LINKOVERLAY = 0x8000,
            SHGFI_SELECTED = 0x10000,
            SHGFI_ATTR_SPECIFIED = 0x20000,
            SHGFI_LARGEICON = 0,
            SHGFI_SMALLICON = 1,
            SHGFI_OPENICON = 2,
            SHGFI_SHELLICONSIZE = 4,
            SHGFI_USEFILEATTRIBUTES = 0x10,
            SHGFI_ADDOVERLAYS = 0x20,
            SHGFI_OVERLAYINDEX = 0x40
        }

        private struct SHFILEINFO
        {

            public int dwAttributes;
            public IntPtr hIcon;
            public int iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;

        }

        private const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100;
        private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x2000;
        private const int FORMAT_MESSAGE_FROM_HMODULE = 0x800;
        private const int FORMAT_MESSAGE_FROM_STRING = 0x400;
        private const int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
        private const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;
        private const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 0xFF;
        private const int MAX_PATH = 0x104;

        private string displayName;
        private Icon fileIcon;
        private string fileName;
        private FileIcon.SHGetFileInfoConstants flags;
        private string typeName;

        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        public FileIcon.SHGetFileInfoConstants Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }

        public Icon ShellIcon
        {
            get
            {
                return fileIcon;
            }
        }

        public string TypeName
        {
            get
            {
                return typeName;
            }
        }

        public FileIcon(bool folder, string fileName, FileIcon.SHGetFileInfoConstants flags)
        {
            this.fileName = fileName;
            this.flags = flags;
            GetInfo(folder);
        }

        public void Dispose()
        {
            if (fileIcon != null)
                fileIcon.Dispose();
        }

        public void GetInfo(bool folder)
        {
            FileIcon.SHFILEINFO shfileinfo;

            if (fileIcon != null)
                fileIcon.Dispose();
            fileIcon = null;
            typeName = "";
            displayName = "";
            shfileinfo = new FileIcon.SHFILEINFO();
            uint ui = (uint)Marshal.SizeOf(shfileinfo.GetType());
            int i1 = FileIcon.SHGetFileInfo(fileName, folder ? 0x10 : 0, ref shfileinfo, ui, (uint)flags);
            if (i1 != 0)
            {
                if (shfileinfo.hIcon != IntPtr.Zero)
                    fileIcon = Icon.FromHandle(shfileinfo.hIcon);
                typeName = shfileinfo.szTypeName;
                displayName = shfileinfo.szDisplayName;
            }
            else
            {
                int i2 = FileIcon.GetLastError();
                Console.WriteLine("Error {0}", i2);
                string s = new String('\0', 0x100);
                int i3 = FileIcon.FormatMessage(0x1200, IntPtr.Zero, i2, 0, s, 0x100, 0);
                Console.WriteLine("Len {0} text {1}", i3, s);
            }
        }

        [PreserveSig]
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private static extern int DestroyIcon(IntPtr hIcon);

        [PreserveSig]
        [DllImport("kernel32", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, string lpBuffer, uint nSize, int argumentsLong);

        [PreserveSig]
        [DllImport("kernel32", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private static extern int GetLastError();

        [PreserveSig]
        [DllImport("shell32", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private static extern int SHGetFileInfo(string pszPath, int dwFileAttributes, ref FileIcon.SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

    } // class FileIcon
}
