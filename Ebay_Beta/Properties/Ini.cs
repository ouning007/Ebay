using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace Ebay_Beta.Properties
{
    class Ini:IDisposable
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        private string inipath;
        public Ini(string path)
        {
            inipath = path;
        }

        public void WriteIni(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, inipath);
        }

        public void WriteIni(string section, string key, string value,string sys)
        {
            WritePrivateProfileString(section, key, value, sys);
        }

        public string GetIni(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        public string GetIni(string Section, string Key,string path)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, path);
            return temp.ToString();
        }

        public bool RemoveKey(string Section,string Key)
        {

            WritePrivateProfileString(Section, Key, null, this.inipath);
            return true;
        }

        public bool RemoveKey(string Section, string Key,string path)
        {
            WritePrivateProfileString(Section, Key, null, path);
            return true;
        }

        public bool RemoveSection(string Section)
        {
            WritePrivateProfileString(Section, null, null, this.inipath);
            return true;
        }

        public bool RemoveSection(string Section,string path)
        {
            WritePrivateProfileString(Section, null, null, path);
            return true;
        }


        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }



        public bool DeleteFile()
        {
            try
            {
                File.Delete(this.inipath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Ini() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}