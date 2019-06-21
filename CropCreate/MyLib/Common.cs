
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MyLib
{
    class Common
    {

        public RemoteFileInfo remote { get; set; }

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

    public class Attri
    {
        public string name { get; set; }
        public string slug { get; set; }
        public int id { get; set; }
    }

    #endregion
}
