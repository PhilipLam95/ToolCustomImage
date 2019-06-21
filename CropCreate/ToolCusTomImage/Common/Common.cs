using SSIS.Extensions.SFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropCreate.Common
{
    class Common
    {


        private async void WriteLog<T>(string mess, Control control)
        {
            //if (control is TextBox)
            //{
            //    control.Invoke((Action)(() => (TextBox)control.AppendText(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n")));
            //}

            TextBox tb = new TextBox();
           
            //tbox_thongtin.AppendText(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n");
        }

        private void WriteLogToTxt(string filepath, string mess)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write))
                using (StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8))
                {
                    sWriter.WriteLine(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n");
                    sWriter.Flush();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SuccessFile(int tongfile)
        {
            //try
            //{
            //    label2.Invoke((Action)(() => label2.Text = demFileSuccess.ToString() + " / " + tongfile));
            //}
            //catch (Exception ex)
            //{
            //    WriteLog(ex.ToString());
            //}

        }

        private void ErrorFile()
        {
            //label1.Invoke((Action)(() => label1.Text = "Error : " + demFileError.ToString()));
        }
    }

    #region Class
    /// <summary>
    /// Stores the Remote file information
    /// </summary>
    public class RemoteFileInfo 
    {
        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name;

        /// <summary>
        /// Gets or sets the full file name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string fullName;

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public string extension;

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public long size;

        /// <summary>
        /// Gets or sets the modified time.
        /// </summary>
        /// <value>
        /// The modified time.
        /// </value>
        public DateTime modifiedTime;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is directory.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is directory; otherwise, <c>false</c>.
        /// </value>
        public bool isDirectory;


        /// <summary>
        /// Gets or sets link image
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is directory; otherwise, <c>false</c>.
        /// </value>
        public string linkImage;


        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    Name = name.Substring(0,name.IndexOf("."));
                }
            }
        }

        public string FullName
        {
            get { return fullName; }
            set
            {
                if (fullName != value)
                {
                    FullName = value;
                }
            }
        }

        public string Extension
        {
            get { return extension; }
            set
            {
                if (extension != value)
                {
                    Extension = value;
                }
            }
        }

        public long Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    Size = value;
                }
            }
        }


        public DateTime ModifiedTime
        {
            get { return modifiedTime; }
            set
            {
                if (modifiedTime != value)
                {
                    ModifiedTime = value;
                }
            }
        }

        public bool IsDirectory
        {
            get { return isDirectory; }
            set
            {
                if (isDirectory != value)
                {
                    IsDirectory = value;
                }
            }
        }

        public string LinkImage
        {
            get { return linkImage; }
            set
            {
                if (linkImage != value)
                {
                    LinkImage = value;
                }
            }
        }
    }

    #endregion
}
