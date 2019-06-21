using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;

using Renci.SshNet.Sftp;
using SSIS.Extensions.SFTP;
using CropCreate.Common;

namespace CropCreate.Presentation_Layer.IMAGE
{
    public partial class UploadLogo : Form
    {
        public static string host = "149.28.75.33";
        public static int port = 22;
        public static string username = "root";
        public static string password = "!zV6$*Lhh,%FD{d4";

        public static string directory = @"/var/www/html/wp-content/uploads/2019/newfolder//";
        public static string newDirectory = "";

        public static SftpClient client;
        public FileInfo[] Files;


        private static int demFileSuccess = 0;
        private static int demFileError = 0;

        private static int demFileLoadSuccess = 0;
        private static int demFileLoadError = 0;

        public UploadLogo()
        {
            InitializeComponent();
        }

        private async void btnUpLoad_Click(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// Lists the files.
        /// </summary>
        /// <param name="remotePath">The remote path.</param>
        /// <param name="failRemoteNotExists">if set to <c>true</c> [fail remote not exists].</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public List<RemoteFileInfo> ListFiles(string remotePath)
        {
            List<RemoteFileInfo> fileList = new List<RemoteFileInfo>();
            try
            {
                
                using (SftpClient sftp = new SftpClient(host, 22, username, password))
                {
                    sftp.Connect();
                    if (!sftp.Exists(remotePath))
                    {
                        MessageBox.Show("not exist");
                    }
                    else
                    {


                        SftpFile sftpFileInfo = sftp.Get(remotePath);
                        if (sftpFileInfo.IsDirectory)
                        {
                            IEnumerable<SftpFile> dirList = sftp.ListDirectory(remotePath);
                            foreach (SftpFile sftpFile in dirList)
                                fileList.Add(this.CreateFileInfo(sftpFile,sftp));
                        }
                        else
                        {
                        
                            fileList.Add(CreateFileInfo(sftpFileInfo,sftp));
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileList;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string remoth = @"/var/www/html/wp-content/uploads/2019/newfolder";
            ListFiles(remoth);
        }

        private RemoteFileInfo CreateFileInfo(SftpFile sftpFile, SftpClient sftp)
        {
            RemoteFileInfo fileInfo = new RemoteFileInfo();
            fileInfo.name = sftpFile.Name;
            fileInfo.fullName = sftpFile.FullName;
            fileInfo.extension = Path.GetExtension(sftpFile.FullName);
            fileInfo.isDirectory = sftpFile.IsDirectory;
            fileInfo.size = sftpFile.Length;
            fileInfo.modifiedTime = sftpFile.LastWriteTime;
            fileInfo.linkImage = sftp.ConnectionInfo.Host + @"/wp-content/uploads/2019/newfolder/" + fileInfo.name;
            return fileInfo;
        }






        private void UploadLogo_Load(object sender, EventArgs e)
        {
            label2.Text = "";
        }

        public static void UploadSFTPFile(string host, string username, string password, string sourcefile, string destinationpath, int port)
        {
            using (SftpClient client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                client.ChangeDirectory(destinationpath);
                using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                {
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(fs, Path.GetFileName(sourcefile));
                }
            }
        }



        private void btnChose_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void SuccessFile(int tongfile)
        {
            try
            {
                label2.Invoke((Action)(() => label2.Text = demFileSuccess.ToString() + " / " + tongfile));
            }
            catch (Exception ex)
            {
                //WriteLog(ex.ToString());
            }

        }

        private void ErrorFile()
        {
            label1.Invoke((Action)(() => label1.Text = "Error : " + demFileError.ToString()));
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

        private void WriteLog(string mess)
        {
            richTextBox_thongtin.Invoke((Action)(() => richTextBox_thongtin.AppendText(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n")));
            //tbox_thongtin.AppendText(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n");
        }

        private void btnUpload_Click_1(object sender, EventArgs e)
        {

            username = tboxUserName.Text;
            password = tboxPassword.Text;
            host = tboxServer.Text;
            newDirectory = SftpConnect.directory + "ImageUpload";

            Task<bool> x = this.UploadFileFromFolder(Files);

            Console.WriteLine(x);
            //var host = "149.28.75.33";
            //var port = 22;
            //var username = "root";
            //var password = "!zV6$*Lhh,%FD{d4";

            //var uploadFile = @"E:/1/-_Big-Fan-Gift-Funny-singer_.png";
            //var uploadPath = @"/var/www/html/wp-content/uploads/2019//";
            ////byte[] csvFile = DownloadCSV(); // Function returns byte[] csv file

            //using (var client = new SftpClient(host, port, username, password))
            //{ 
            //    try
            //    {
            //        client.Connect();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw (ex);
            //    }
            //    var files = new List<String>();
            //    if (client.IsConnected)
            //    {

            //        try
            //        {
            //            client.ChangeDirectory(uploadPath);
            //        }
            //        catch (Exception ex)
            //        {
            //            throw (ex);
            //        }


            //        //Debug.WriteLine("I'm connected to the client");
            //        //using (var fileStream = new FileStream(uploadFile, FileMode.Open))
            //        //{
            //        //    client.BufferSize = 4 * 1024; // bypass Payload error large files
            //        //    client.UploadFile(fileStream, Path.GetFileName(uploadFile));
            //        //    //client.up
            //        //}
            //        MessageBox.Show("Connected");
            //    }
            //    else
            //    {
            //        MessageBox.Show(" Not Connected");
            //        //Debug.WriteLine("I couldn't connect");
            //    }
            //}
        }

        private void btnChose_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();

            string[] extensions = new string[] { ".BMP", ".JPG", ".GIF", ".PNG" };
            var fd = new System.Windows.Forms.FolderBrowserDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //DirectoryInfo d = new DirectoryInfo(fd.SelectedPath);//Assuming Test is your Folder
                //String searchFolder = fd.SelectedPath;
                //var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
                //Files = GetFilesFrom(searchFolder, filters, false);

                //FileInfo[] FileInfos = d.GetFiles("*.png"); //Getting Text files

                List<string> ext = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".tif" };
                Files = new DirectoryInfo(fd.SelectedPath).EnumerateFiles("*.*", SearchOption.TopDirectoryOnly)
               .Where(path => ext.Contains(Path.GetExtension(path.Name)))
               .Select(x => new FileInfo(x.FullName)).ToArray();
                Console.WriteLine("10");
            }
            btnChose.Text = fd.SelectedPath;
        }
        private void before_runProgressbarloading(ProgressBar progress)
        {
            progress.Value = 0;
        }

        public async Task<bool> UploadFileFromFolder(FileInfo[] filePath)
        {
            demFileSuccess = 0;
            demFileError = 0;
            var tasks = new List<Task>();
            /*label2.Text = "0 / 883385";*/ /*+ filePath.Count().ToString();*/
            string error = "";
            string pathError = "";
            pathError = btnChose.Text.Trim().ToString() + "\\" + "log_upload.txt";// đường dẫn của file muốn tạo
            error = pathError.Clone().ToString();
            if (!File.Exists(pathError))
            {
                FileStream fs = new FileStream(pathError, FileMode.Create);
            }
            MemoryStream ms = new MemoryStream();

            if (filePath.Count() > 1)
            {
                before_runProgressbarloading(progressBar1);
            }
            try
            {
                ConnectionInfo connectionInfo = new PasswordConnectionInfo(host, username, password);

                using (client = new SftpClient(host, connectionInfo.Port, username, password))
                {
                    client.Connect();
                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                    var files = new List<String>();
                    if (client.IsConnected)
                    {
                        client.ChangeDirectory(directory);
                        try
                        {
                            for (int i = 0; i < filePath.Count(); i++)
                            {
                                using (var file = File.OpenRead(filePath[i].ToString()))
                                {
                                    tasks.Add(UploadFileAsync(filePath[i].FullName, filePath[i].Name, error));

                                }
                            }

                            
                            await Task.WhenAll(tasks);
                            WriteLog("All file done");
                            progressBar1.Invoke(new MethodInvoker(delegate ()
                            {
                                progressBar1.Value = 100;
                            }));
                            client.Disconnect();

                        }
                        catch (Renci.SshNet.Common.SshConnectionException)
                        {
                            Console.WriteLine("Cannot connect to the server.");
                            return false;
                        }
                        catch (System.Net.Sockets.SocketException)
                        {
                            Console.WriteLine("Unable to establish the socket.");
                            return false;
                        }
                        catch (Renci.SshNet.Common.SshAuthenticationException)
                        {
                            Console.WriteLine("Authentication of SSH session failed.");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(" Not Connected");

                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }


            return true;
        }

        public async Task UploadFileAsync(string pathfile, string file, string error)
        {

            using (Stream stream = File.OpenRead(pathfile))
            {

                try
                {
                    //long totaluploaded = 0;
                    //var task = client.BeginUploadFile(stream, file) as SftpUploadAsyncResult;
                    //while (!task.IsCompleted)
                    //{
                    //    SetProgressStatus(totaluploaded + (long)task.UploadedBytes, stream.Length,file);
                    //}
                    //client.EndUploadFile(task);
                    //totaluploaded += stream.Length; // ko dung

                    var task = Task.Factory.FromAsync(client.BeginUploadFile(stream, file), client.EndUploadFile);
                    demFileSuccess++;
                    progressBar1.Invoke(new MethodInvoker(delegate ()
                    {
                        progressBar1.Value = ((demFileSuccess - 1) * 100 / Files.Count());

                    }));
                    SuccessFile(Files.Count());
                    WriteLog(file + " done");

                    await task;
                }
                catch (Exception ex)
                {
                    demFileError++;
                    ErrorFile();
                    WriteLogToTxt(error, file + " error: " + ex.ToString());

                }

                
            }

        }


        private void SetProgressStatus(long cur, long max,string file)
        {
            Action del = () =>
            {
                progressBar1.Maximum = 100;
                progressBar1.Value = (int)((cur * 100) / (max));
                label2.Text = cur.ToString() + " / " + max.ToString();
                if (cur == max)
                {
                    progressBar1.Maximum = 100;
                    progressBar1.Value = 100;
                    demFileSuccess++;
                    label2.Text = cur.ToString() + " / " + max.ToString();
                    //SuccessFile(Files.Count());
                    //WriteLog(file + " " + cur + " done");
                    //EnableButtons();
                    Console.WriteLine(demFileSuccess);
                }
            };
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(del);
            }
            else
            {
                del();
            }
        }

        
    }
}

