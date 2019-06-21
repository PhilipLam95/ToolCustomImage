using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet;
using System.IO;
using CropCreate.Presentation_Layer.IMAGE;
using ToolCusTomImage;

namespace CropCreate
{
    class SftpConnect
    {
        public static string host = "149.28.75.33";
        public static int port = 22;
        public static string username = "root";
        public static string password =  "!zV6$*Lhh,%FD{d4";

        public static string directory = @"/var/www/html/wp-content/uploads/2019/uploadImage//";
        public static string newDirectory = "";

        public static SftpClient client;

        UploadLogo f = Program.fLOGO; 
        //Constructor


        //Initialize values
        public static bool Connect()
        {
            if (client != null && client.IsConnected)
            {
                client.Disconnect();
            }
            try
            {
                ConnectionInfo connectionInfo = new PasswordConnectionInfo(host, username, password);

                using (client = new SftpClient(host, connectionInfo.Port, username, password))
                {
                    client.Connect();
                    var files = new List<String>();
                    if (client.IsConnected)
                    {
                        return true;
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

        }

        public static void Close()
        {
            if (client.IsConnected)
            {
                client.Disconnect();
            }
             
        }


        public  bool UploadFileFromFolder(FileInfo[] filePath)
        {
            int dem = 0;
            try
            {
                ConnectionInfo connectionInfo = new PasswordConnectionInfo(host, username, password);

                using (client = new SftpClient(host, connectionInfo.Port, username, password))
                {
                    client.Connect();
                    var files = new List<String>();
                    var uploadPath = @"/var/www/html/wp-content/uploads/2019//";
                    if (client.IsConnected)
                    {
                        client.ChangeDirectory(directory);
                        try
                        {
                            for (int i = 0; i < filePath.Count(); i++)
                            {
                                using (var file = File.OpenRead(filePath[i].ToString()))
                                {

                                    try
                                    {
                                        string filename = Path.GetFileName(filePath[i].ToString());
                                        client.UploadFile(file, filename);
                                        dem++;
                                       
                                    }
                                    catch (Exception ex)
                                    {
                                       
                                    }
                                    
                                }
                            }

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






    }
}
