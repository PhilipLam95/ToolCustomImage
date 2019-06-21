using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;
using ToolCusTomImage.Common;
using DevExpress.XtraLayout.Utils;

namespace ToolCusTomImage.Presentation_Layer.IMAGE
{
    public partial class Crop_Canvasize : Form
    {



        #region Image
        private Thread ImageLoaderThread;
        int cropX;
        int cropY;
        int cropWidth;

        int cropHeight;
        int oCropX;
        int oCropY;
        public Pen cropPen;


        int ocropWidth;
        int ocropHeight;

        public int maxFileShow = 0;

        public FileInfo[] Files;
        public DashStyle cropDashStyle = DashStyle.DashDot;
        public bool Makeselection = false;

        public bool CreateText = false;
        private Image Img;

        private int defaultwidth = 0;

        private float scaleImageWidth = 0;
        private float scaleImageHeight = 0;

        private int canvansize = 0;
        static int numthreads = 0;

        static int _index = 0;

        static int numberFileOneThread = 50;
        static object syncObj = new object();
        DataTable list_table = new DataTable();

        private static int demFileSuccess = 0;
        private static int demFileError = 0;

        private static int demFileLoadSuccess = 0;
        private static int demFileLoadError = 0;

        OpenFileDialog dialog;
        ToolCusTomImage.CallThread callthread = new ToolCusTomImage.CallThread(1000);
        #endregion
        public Crop_Canvasize()
        {
            InitializeComponent();
            label2.Text = "";
            layoutControlItem4.Width = 324;
            layoutControlItem4.Height = 324;
            defaultwidth = 324;

        }

        private void Crop_Canvasize_Load(object sender, EventArgs e)
        {
            defaultwidth = layoutControlItem4.Width;
            tbox_thongtin.Multiline = true;
            // Add vertical scroll bars to the TextBox control.
            tbox_thongtin.ScrollBars = ScrollBars.Vertical;
            // Allow the RETURN key to be entered in the TextBox control.
            tbox_thongtin.AcceptsReturn = true;
            // Allow the TAB key to be entered in the TextBox control.
            tbox_thongtin.AcceptsTab = true;
            // Set WordWrap to true to allow text to wrap to the next line.
            tbox_thongtin.WordWrap = true;
            // Set the default text of the control.
            pictureEdit1.Properties.ShowMenu = false;

            panel1.Visible = false;
            layoutControlItem14.Visibility = LayoutVisibility.Never;

            progresBarLoad.Visible = false;



        }


        private void WriteLog(string mess)
        {
            tbox_thongtin.Invoke((Action)(() => tbox_thongtin.AppendText(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n")));
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
            try
            {
                label2.Invoke((Action)(() => label2.Text = demFileSuccess.ToString() + " / " + tongfile));
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }

        }

        private void ErrorFile()
        {
            label1.Invoke((Action)(() => label1.Text = "Error : " + demFileError.ToString()));
        }

        /// <summary>
        /// Builds the filter condition.
        /// </summary>
        /// <param name="searchFolder"> folder directory</param>
        /// <param name="filter"> </param>
        /// <param name="isRecursive"> check ma</param>
        public static String[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }



        private void buttonEdit1_Click(object sender, EventArgs e)
        {
            try
            {
                list_table = new DataTable();


                list_table.Columns.Add("STT", typeof(System.Int32));
                list_table.Columns.Add("ListFile", typeof(System.String));
                list_table.Columns.Add("Path", typeof(System.String));


                demFileLoadSuccess = 0;
                demFileLoadError = 0;

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



                //dialog = new OpenFileDialog();
                //dialog.Title = "Select image";
                //dialog.Multiselect = true;
                //dialog.Filter = "*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                //ListFile_Control.DataSource = null;

                //if (dialog.ShowDialog() == DialogResult.OK)
                //{

                if (Files.Any())
                {

                    // đếm tổng file để chạy số lượng hình ảnh tương đương trên mỗi thread 
                    if (Files.Count() > numberFileOneThread && Files.Count() > 1000)
                    {
                        numthreads = (int)Math.Ceiling(float.Parse("1000".ToString()) / numberFileOneThread);
                        maxFileShow = 1000;
                    }

                    if (Files.Count() > numberFileOneThread && Files.Count() < 1000)
                    {
                        numthreads = (int)Math.Ceiling(float.Parse(Files.Count().ToString()) / numberFileOneThread);
                        maxFileShow = Files.Count();
                    }
                    if (Files.Count() < numberFileOneThread)
                    {
                        numthreads = 1;
                        maxFileShow = Files.Count();
                    }
                    progresBarLoad.Visible = true;
                    Thread thr = new Thread(() =>
                    {
                        //khoi tao thread
                        _index = -1;
                        List<Thread> threads = new List<Thread>();
                        for (int i = 0; i < numthreads; i++)
                        {
                            Thread t = new Thread(LoadImageToGridView);
                            threads.Add(t);
                            t.Start(i);
                            Thread.Sleep(100);
                        }

                        while (true)
                        {
                            bool done = true;
                            for (int i = 0; i < threads.Count; i++)
                            {
                                if (threads[i].IsAlive)
                                {
                                    done = false;
                                    buttonEdit1.BeginInvoke(new MethodInvoker(delegate ()
                                    {
                                        buttonEdit1.Enabled = false;
                                    }));
                                    break;
                                }
                            }
                            if (done)
                            {
                                buttonEdit1.BeginInvoke(new MethodInvoker(delegate ()
                                {
                                    buttonEdit1.Enabled = true;
                                }));
                                try
                                {
                                    ListFile_Control.DataSource = list_table;
                                    WriteLog("Load  file Success");
                                }
                                catch
                                {
                                    ListFile_Control.DataSource = list_table;
                                }
                                WriteLog("Load  file Success");
                                progressBarLoading.Invoke(new MethodInvoker(delegate ()
                                {
                                    progresBarLoad.Value = 100;
                                }));


                                break;
                            }
                            Thread.Sleep(1000);
                        }
                    });
                    thr.Start();
                    buttonEdit1.Text = fd.SelectedPath;
                    progresBarLoad.Visible = false;
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        private void LoadImageToGridView(object obj)
        {
            int STT = (int)obj;

            //lock (syncObj)
            //{
            if (STT == numthreads - 1 && STT > 0)
            {
                //for (int i = (numberFileOneThread * STT); i < (dialog.FileNames.Count()); i++)
                //{
                //    FileInfo file = new FileInfo(dialog.FileNames[i]);
                //    list_table.Rows.Add(i, file.Name, file.FullName);
                //    progressBarLoading.Invoke(new MethodInvoker(delegate ()
                //    {
                //        progressBarLoading.Value = (i * 100 / (dialog.FileNames.Count() - 1));
                //    }));
                //}

                for (int i = (numberFileOneThread * STT); i < maxFileShow; i++)
                {

                    list_table.Rows.Add(i, Files[i].Name, Files[i].FullName);
                    progressBarLoading.Invoke(new MethodInvoker(delegate ()
                    {
                        progresBarLoad.Value = (i * 100 / (maxFileShow - 1));
                    }));
                }
            }
            else
            {
                if (maxFileShow < numberFileOneThread)
                {
                    //for (int i = (numberFileOneThread * STT); i < dialog.FileNames.Count(); i++)
                    //{

                    //    FileInfo file = new FileInfo(dialog.FileNames[i]);
                    //    list_table.Rows.Add(i, file.Name, file.FullName);
                    //    progressBarLoading.Invoke(new MethodInvoker(delegate ()
                    //    {
                    //        progressBarLoading.Value = (i * 100 / dialog.FileNames.Count());
                    //    }));
                    //}
                    for (int i = (numberFileOneThread * STT); i < maxFileShow; i++)
                    {

                        //FileInfo file = new FileInfo(Files[i]);
                        list_table.Rows.Add(i, Files[i].Name, Files[i].FullName);
                        progresBarLoad.Invoke(new MethodInvoker(delegate ()
                        {
                            progresBarLoad.Value = (i * 100 / maxFileShow);
                        }));
                    }
                }
                else
                {
                    for (int i = (numberFileOneThread * STT); i < (numberFileOneThread * (STT + 1)); i++)
                    {

                        /*FileInfo file = new FileInfo(Files[i]);*/
                        list_table.Rows.Add(i, Files[i].Name, Files[i].FullName);
                        progresBarLoad.Invoke(new MethodInvoker(delegate ()
                        {
                            progresBarLoad.Value = (i * 100 / maxFileShow);
                        }));
                    }
                }

            }
            //}


        }


        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            string x = gridView1.GetRowCellValue(e.RowHandle, gridColumn3.FieldName).ToString();
            Img = null;
            FileStream fs = null;
            try
            {
                fs = new FileStream(x, FileMode.Open, FileAccess.Read);
                Img = Image.FromStream(fs);
            }
            finally
            {
                fs.Close();
            }

            LoadImage();

        }

        private void LoadImage()
        {
            //we set the picturebox size according to image, we can get image width and height with the help of Image.Width and Image.height properties.
            int imgWidth = Img.Width;
            int imghieght = Img.Height;
            pictureEdit1.Image = Img;


            float total = 0;

            float width = float.Parse(imgWidth.ToString());
            float height = float.Parse(imghieght.ToString());


            total = float.Parse(defaultwidth.ToString()) * (width / height);
            layoutControlItem4.Width = Convert.ToInt32(total);
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.StretchHorizontal;
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.StretchVertical;
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Clip;
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;

            //MessageBox.Show(pictureEdit1.Width.ToString()+ "*" + pictureEdit1.Height.ToString());
            ////MessageBox.Show(pictureEdit1.Height.ToString() + "*" + pictureEdit1.Height.ToString());
            //pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            //pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;

            SizeIMG_textbox.Text = (Img.Width + "x" + Img.Height);


        }

        private void pictureEdit1_MouseClick(object sender, MouseEventArgs e)
        {

            PictureEdit pe = sender as PictureEdit;
            Point mousePosition = pe.PointToClient(Control.MousePosition);
            Point centerPoint = pe.ViewportToImage(new Point(pe.Width / 2, pe.Height / 2));
            Point currentPoint = pe.ViewportToImage(mousePosition);
            int xOffset = (currentPoint.X - centerPoint.X);
            int yOffset = (currentPoint.Y - centerPoint.Y);

            Cursor = Cursors.Default;
            Makeselection = true;

        }


        /// <summary>
        /// Chuột di chuyển trên pictureEdit1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> </param>
        /// 
        private void pictureEdit1_MouseMove(object sender, MouseEventArgs e)
        {
            //Cursor = Cursors.Default;
            //PictureEdit pe = sender as PictureEdit;
            //Point mousePosition = pe.PointToClient(Control.MousePosition);


            ////MessageBox.Show(Cursor.Position.ToString());

            Point TextStartLocation = e.Location;
            if (CreateText)
            {
                Cursor = Cursors.IBeam;
            }

            Cursor = Cursors.Default;
            if (Makeselection)
            {

                try
                {
                    if (pictureEdit1.Image == null)
                        return;


                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        pictureEdit1.Refresh();
                        cropWidth = e.X - cropX;
                        cropHeight = e.Y - cropY;

                        ocropWidth = Cursor.Position.X;
                        ocropHeight = Cursor.Position.Y;



                        pictureEdit1.CreateGraphics().DrawRectangle(cropPen, cropX, cropY, cropWidth, cropHeight);
                        Console.WriteLine("MouseMoveX :" + e.X);
                        Console.WriteLine("MouseMoveY :" + e.Y);


                        Width_domaniUpdown.Text = Convert.ToInt32((float.Parse(e.X.ToString()) * scaleImageWidth)
                                                        - Convert.ToInt32(PointX_domaniUpdown.Text)).ToString();

                        Height_domaniUpdown.Text = Convert.ToInt32((float.Parse(e.Y.ToString()) * scaleImageWidth)
                                                        - Convert.ToInt32(PointY_domaniUpdown.Text)).ToString();

                    }



                }
                catch (Exception ex)
                {
                    //if (ex.Number == 5)
                    //    return;
                }
            }
        }

        bool MousePressed = false;
        Point startPoint = new Point();



        /// <summary>
        /// chECK SU KIỆN KHI VỪA CLICK CHUỘT TRÊN pictureEdit1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> </param>
        /// 
        private void pictureEdit1_MouseDown(object sender, MouseEventArgs e)
        {

            Makeselection = true;

            Point TextStartLocation = e.Location;
            if (CreateText)
            {
                Cursor = Cursors.IBeam;
            }


            Cursor = Cursors.Default;
            if (Makeselection)
            {

                try
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        Cursor = Cursors.Cross;
                        cropX = e.X;
                        cropY = e.Y;


                        oCropX = Cursor.Position.X;
                        oCropY = Cursor.Position.Y;
                        cropPen = new Pen(Color.Green, 1);
                        cropPen.DashStyle = DashStyle.Solid;

                        Console.WriteLine("MouseDownX :" + cropX);
                        Console.WriteLine("MouseDownY :" + cropY);

                        Console.WriteLine("LocationX :" + this.Location.X);
                        Console.WriteLine("LocationY :" + this.Location.Y);



                        scaleImageWidth = float.Parse(Img.Width.ToString()) / float.Parse(layoutControlItem4.Width.ToString());
                        scaleImageHeight = float.Parse(Img.Height.ToString()) / float.Parse(layoutControlItem4.Height.ToString());

                        PointX_domaniUpdown.Text = Convert.ToInt32((scaleImageWidth * float.Parse(cropX.ToString()))).ToString();
                        PointY_domaniUpdown.Text = Convert.ToInt32((scaleImageHeight * float.Parse(cropY.ToString()))).ToString();
                    }

                    pictureEdit1.Refresh();

                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// chECK SU KIỆN KHI VỪA Thả chuột TRÊN pictureEdit1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> </param>
        private void pictureEdit1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Makeselection)
            {
                Cursor = Cursors.Default;


                if (pictureEdit1.Image == null)
                    return;
                Console.WriteLine("MouseUp" + e.X + ":" + e.Y);
                //Console.WriteLine(layoutControlItem4.Width.ToString());
                //Console.WriteLine(layoutControlItem4.Height.ToString());
                //Console.WriteLine(float.Parse(Img.Width.ToString()));
                //Console.WriteLine(float.Parse(Img.Height.ToString()));

                Width_domaniUpdown.Text = Convert.ToInt32((float.Parse(e.X.ToString()) * scaleImageWidth)
                                                - Convert.ToInt32(PointX_domaniUpdown.Text)).ToString();

                Height_domaniUpdown.Text = Convert.ToInt32((float.Parse(e.Y.ToString()) * scaleImageHeight)
                                                - Convert.ToInt32(PointY_domaniUpdown.Text)).ToString();
            }
        }

        private void btnCrop_Click(object sender, EventArgs e)
        {
            try
            {

                string folderName = buttonEdit1.Text;
                string pathString = "";
                string fileName = "";
                string filepath = pathString + "\\" + fileName;
                int dem = 0;
                layoutControlItem14.Visibility = LayoutVisibility.Always;
                panel1.Visible = true;
                if (pictureEdit1.Image == null)
                    return;
                if (Convert.ToInt32(Width_domaniUpdown.Text) < 1)
                {
                    return;
                }

                demFileSuccess = 0;
                demFileError = 0;
                label1.Text = "Error : " + demFileError.ToString();
                pathString = System.IO.Path.Combine(folderName, "Crop_Done");

                if (!System.IO.Directory.Exists(pathString)) System.IO.Directory.CreateDirectory(pathString);
                try
                {

                    numthreads = ResizeImage.autoGetNumberFileOneThread(Files.Count(), numberFileOneThread);
                    callThread_CropImage(1, "crop_", numthreads, pathString, btnCrop.Name);
                    WriteLog("All file done");
                    progressBarLoading.Invoke(new MethodInvoker(delegate ()
                    {
                        progressBarLoading.Value = 100;
                    }));



                    //thr.Start();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
            catch (Exception ex)
            {
            }
        }

        public void callThread_CropImage(int numberthread, string name, int numthreads, string pathString, string buttonName)
        {
            //string fileName, string filepath, string pathString
            int Width_resize = Convert.ToInt32(Width_domaniUpdown.Text);
            int Height_resize = Convert.ToInt32(Height_domaniUpdown.Text);
            object obj = new object[5] { numberthread, pathString, Width_resize, Height_resize, buttonName };
            Thread Thrd = new Thread(new ParameterizedThreadStart(CropCanvasize_Imagẹ̣̣));
            Thrd.Name = name + "_" + numberthread.ToString();
            Thrd.Start(obj);

            Thread.Sleep(100);
        }


        //public void Crop_Imagẹ̣̣(object obj)
        //{

        //    string filepath = "";
        //    Array objArray = new object[4];
        //    objArray = (Array)obj;
        //    int STT = Convert.ToInt32(objArray.GetValue(0));
        //    int Width_resize = Convert.ToInt32(objArray.GetValue(2));
        //    int Height_resize = Convert.ToInt32(objArray.GetValue(3));

        //    try
        //    {
        //        lock (syncObj)
        //        {
        //            if (STT == numthreads - 1 && STT > 0)
        //            {
        //                for (int i = (numberFileOneThread * STT); i < gridView1.DataRowCount; i++)
        //                {
        //                    DataRow row = gridView1.GetDataRow(i);
        //                    Bitmap OriginalImage = new Bitmap(row[2].ToString());

        //                    try
        //                    {
        //                        if (OriginalImage.Width == 2000 && OriginalImage.Height == 1871)
        //                        {
        //                            Rectangle newrect = new Rectangle((cropX * OriginalImage.Width) / pictureEdit1.Width,
        //                                            (cropY * OriginalImage.Height) / pictureEdit1.Height,
        //                                            (cropWidth * OriginalImage.Width) / pictureEdit1.Width,
        //                                            (cropHeight * OriginalImage.Height) / pictureEdit1.Height);
        //                            //First we define a rectangle with the help of already calculated points

        //                            Bitmap target = new Bitmap((cropWidth * OriginalImage.Width) / pictureEdit1.Width,
        //                                                (cropHeight * OriginalImage.Height) / pictureEdit1.Height);

        //                            Graphics gfx = Graphics.FromImage(target);
        //                            gfx.SmoothingMode = SmoothingMode.AntiAlias;
        //                            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                            gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //                            gfx.DrawImage(OriginalImage, new Rectangle(0, 0, newrect.Width, newrect.Height), newrect.X, newrect.Y, Width_resize, Height_resize, GraphicsUnit.Pixel);


        //                            Bitmap newimage = new Bitmap(target, Width_resize, Height_resize); //resizing image

        //                            filepath = objArray.GetValue(1).ToString() + "\\" + gridView1.GetRowCellValue(i, gridColumn2.FieldName).ToString();
        //                            newimage.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
        //                            OriginalImage.Dispose();
        //                            gfx.Dispose();
        //                            newimage.Dispose();
        //                            target.Dispose();
        //                            demFileSuccess++;
        //                            SuccessFile(gridView1.DataRowCount);
        //                            WriteLog(gridView1.GetRowCellValue(i, gridColumn2.FieldName).ToString() + " done");
        //                        }


        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw (ex);
        //                        WriteLog(gridView1.GetRowCellValue(i, gridColumn2.FieldName).ToString() + " error");
        //                        demFileError++;
        //                        ErrorFile();
        //                        continue;
        //                    }
        //                    progressBarLoading.Invoke(new MethodInvoker(delegate ()
        //                    {
        //                        progressBarLoading.Value = (demFileSuccess * 100 / (gridView1.DataRowCount));
        //                    }));
        //                }
        //                callthread.RemoveThread();
        //            }
        //            else
        //            {
        //                if (gridView1.DataRowCount < numberFileOneThread)
        //                {
        //                    for (int i = (numberFileOneThread * STT); i < gridView1.DataRowCount; i++)
        //                    {

        //                        DataRow row = gridView1.GetDataRow(i);
        //                        Bitmap OriginalImage = new Bitmap(row[2].ToString());

        //                        try
        //                        {
        //                            if (OriginalImage.Width == 2000 && OriginalImage.Height == 1871)
        //                            {
        //                                Rectangle newrect = new Rectangle((cropX * OriginalImage.Width) / pictureEdit1.Width,
        //                                                (cropY * OriginalImage.Height) / pictureEdit1.Height,
        //                                                (cropWidth * OriginalImage.Width) / pictureEdit1.Width,
        //                                                (cropHeight * OriginalImage.Height) / pictureEdit1.Height);
        //                                //First we define a rectangle with the help of already calculated points

        //                                Bitmap target = new Bitmap((cropWidth * OriginalImage.Width) / pictureEdit1.Width,
        //                                                    (cropHeight * OriginalImage.Height) / pictureEdit1.Height);

        //                                Graphics gfx = Graphics.FromImage(target);
        //                                gfx.SmoothingMode = SmoothingMode.AntiAlias;
        //                                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //                                gfx.DrawImage(OriginalImage, new Rectangle(0, 0, newrect.Width, newrect.Height), newrect.X, newrect.Y, Width_resize, Height_resize, GraphicsUnit.Pixel);


        //                                Bitmap newimage = new Bitmap(target, Width_resize, Height_resize); //resizing image

        //                                filepath = objArray.GetValue(1).ToString() + "\\" + gridView1.GetRowCellValue(i, gridColumn2.FieldName).ToString();
        //                                newimage.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
        //                                OriginalImage.Dispose();
        //                                gfx.Dispose();
        //                                newimage.Dispose();
        //                                target.Dispose();
        //                                demFileSuccess++;
        //                                SuccessFile(gridView1.DataRowCount);
        //                                WriteLog(gridView1.GetRowCellValue(i, gridColumn2.FieldName).ToString() + " done");
        //                            }


        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            throw (ex);
        //                            WriteLog(gridView1.GetRowCellValue(i, gridColumn2.FieldName).ToString() + " error");
        //                            demFileError++;
        //                            ErrorFile();
        //                            continue;
        //                        }
        //                        progressBarLoading.Invoke(new MethodInvoker(delegate ()
        //                        {
        //                            progressBarLoading.Value = (demFileSuccess * 100 / (gridView1.DataRowCount));
        //                        }));
        //                    }
        //                    callthread.RemoveThread();
        //                }
        //                else
        //                {
        //                    for (int i = (numberFileOneThread * STT); i < (numberFileOneThread * (STT + 1)); i++)
        //                    {

        //                        DataRow row = gridView1.GetDataRow(i);
        //                        Bitmap OriginalImage = new Bitmap(row[2].ToString());

        //                        try
        //                        {
        //                            if (OriginalImage.Width == 2000 && OriginalImage.Height == 1871)
        //                            {
        //                                Rectangle newrect = new Rectangle((cropX * OriginalImage.Width) / pictureEdit1.Width,
        //                                                (cropY * OriginalImage.Height) / pictureEdit1.Height,
        //                                                (cropWidth * OriginalImage.Width) / pictureEdit1.Width,
        //                                                (cropHeight * OriginalImage.Height) / pictureEdit1.Height);
        //                                //First we define a rectangle with the help of already calculated points

        //                                Bitmap target = new Bitmap((cropWidth * OriginalImage.Width) / pictureEdit1.Width,
        //                                                    (cropHeight * OriginalImage.Height) / pictureEdit1.Height);

        //                                Graphics gfx = Graphics.FromImage(target);
        //                                gfx.SmoothingMode = SmoothingMode.AntiAlias;
        //                                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //                                gfx.DrawImage(OriginalImage, new Rectangle(0, 0, newrect.Width, newrect.Height), newrect.X, newrect.Y, Width_resize, Height_resize, GraphicsUnit.Pixel);


        //                                Bitmap newimage = new Bitmap(target, Width_resize, Height_resize); //resizing image

        //                                filepath = objArray.GetValue(1).ToString() + "\\" + gridView1.GetRowCellValue(i, gridColumn2.FieldName).ToString();
        //                                newimage.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
        //                                OriginalImage.Dispose();
        //                                newimage.Dispose();
        //                                gfx.Dispose();
        //                                target.Dispose();
        //                                demFileSuccess++;
        //                                SuccessFile(gridView1.DataRowCount);
        //                                WriteLog(gridView1.GetRowCellValue(i, gridColumn2.FieldName).ToString() + " done");
        //                            }


        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            throw (ex);
        //                            WriteLog(gridView1.GetRowCellValue(i, gridColumn2.FieldName).ToString() + " error");
        //                            demFileError++;
        //                            ErrorFile();
        //                            continue;
        //                        }
        //                        progressBarLoading.Invoke(new MethodInvoker(delegate ()
        //                        {
        //                            progressBarLoading.Value = (demFileSuccess * 100 / (gridView1.DataRowCount));
        //                        }));
        //                    }
        //                    callthread.RemoveThread();
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog("Lỗi : " + ex.ToString());
        //    }
        //}

        public void CropCanvasize_Imagẹ̣̣(object obj)
        {

            string filepath = "";
            string pathError = "";
            string error = "";
            Array objArray = new object[5];
            objArray = (Array)obj;
            int STT = Convert.ToInt32(objArray.GetValue(0));
            int Width_resize = Convert.ToInt32(objArray.GetValue(2));
            int Height_resize = Convert.ToInt32(objArray.GetValue(3));

            pathError = objArray.GetValue(1).ToString() + "\\" + "log_" + objArray.GetValue(4) + ".txt";// đường dẫn của file muốn tạo
            error = pathError.Clone().ToString();
            if (!File.Exists(pathError))
            {
                FileStream fs = new FileStream(pathError, FileMode.Create);//Tạo file mới tên là test.txt            
            }

            for (int i = 0; i < Files.Count(); i++)
            {
                try
                {
                    using (Bitmap OriginalImage = new Bitmap(Files[i].FullName))
                    {
                        filepath = objArray.GetValue(1).ToString() + "\\" + Files[i].Name.ToString();
                        try
                        {
                            if (OriginalImage.Width == 2000 && OriginalImage.Height == 1871)
                            {
                                Rectangle newrect = new Rectangle((cropX * OriginalImage.Width) / pictureEdit1.Width,
                                                (cropY * OriginalImage.Height) / pictureEdit1.Height,
                                                (cropWidth * OriginalImage.Width) / pictureEdit1.Width,
                                                (cropHeight * OriginalImage.Height) / pictureEdit1.Height);
                                //First we define a rectangle with the help of already calculated points

                                Bitmap target = new Bitmap((cropWidth * OriginalImage.Width) / pictureEdit1.Width,
                                                    (cropHeight * OriginalImage.Height) / pictureEdit1.Height);

                                Graphics gfx = Graphics.FromImage(target);
                                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                gfx.DrawImage(OriginalImage, new Rectangle(0, 0, newrect.Width, newrect.Height), newrect.X, newrect.Y, Width_resize, Height_resize, GraphicsUnit.Pixel);


                                Bitmap newimage = new Bitmap(target, Width_resize, Height_resize); //resizing image
                                if (objArray.GetValue(4) == btn_CropCanvasize.Name)
                                {
                                    newimage = new Bitmap(newimage, Convert.ToInt32(Wresize_dmupdown.Text), Convert.ToInt32(Hresize_dmupdown.Text));
                                }
                                newimage.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
                                gfx.Dispose();
                                newimage.Dispose();
                                target.Dispose();
                                demFileSuccess++;
                                SuccessFile(Files.Count());
                                WriteLog(Files[i].Name.ToString() + " done");


                            }
                            else
                            {

                                if (objArray.GetValue(4) == btn_CropCanvasize.Name)
                                {
                                    Bitmap bmp = new Bitmap(Files[i].FullName.ToString());//get image
                                    Bitmap bmpResized = new Bitmap(bmp, Convert.ToInt32(Wresize_dmupdown.Text), Convert.ToInt32(Hresize_dmupdown.Text)); //resizing image


                                    bmpResized.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
                                    bmp.Dispose();
                                    bmpResized.Dispose();
                                }
                                else System.IO.File.Copy(Files[i].FullName, filepath, true);
                                demFileSuccess++;
                                SuccessFile(Files.Count());
                                WriteLog(Files[i].Name.ToString() + " done");

                            }
                            OriginalImage.Dispose();

                        }
                        catch (Exception ex)
                        {

                            WriteLogToTxt(error, Files[i].Name.ToString() + " error: " + ex.ToString());
                            WriteLog(Files[i].Name.ToString() + " error");
                            demFileError++;
                            ErrorFile();
                            continue;
                        }
                    }

                    progressBarLoading.Invoke(new MethodInvoker(delegate ()
                    {
                        progressBarLoading.Value = (demFileSuccess * 100 / Files.Count());
                    }));

                }
                catch (Exception ex)
                {
                    demFileError++;
                    ErrorFile();
                    WriteLogToTxt(error, Files[i].Name.ToString() + " error: " + ex.ToString());
                }
            }

        }

        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }

        public void SaveImage(Image img)
        {

            try
            {
                if (Convert.ToInt32(Width_domaniUpdown.Text) < 1)
                {
                    return;
                }
                if (pictureEdit1.Image == null)
                    return;

                Bitmap oldimage = new Bitmap(img);
                Bitmap newimage = new Bitmap(Convert.ToInt32(Width_domaniUpdown.Text), Convert.ToInt32(Height_domaniUpdown.Text));
                Graphics g = Graphics.FromImage((Image)newimage);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(oldimage,
                                0,
                                0,
                                newimage.Width,
                                newimage.Height);


                string folderName = buttonEdit1.Text;
                string pathString = "";
                string fileName = "";
                string filepath = pathString + "\\" + fileName;

                // To create a string that specifies the path to a subfolder under your 
                // top-level folder, add a name for the subfolder to folderName.
                pathString = System.IO.Path.Combine(folderName, "Crop_Done");
                if (!System.IO.Directory.Exists(pathString)) System.IO.Directory.CreateDirectory(pathString);

                // Create a file name for the file you want to create. 
                fileName = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridColumn2.FieldName).ToString();
                filepath = pathString + "\\" + fileName;
                newimage.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
                oldimage.Dispose();

                newimage.Dispose();
                oldimage.Dispose();
                img.Dispose();
            }
            catch (Exception ex)
            {
                //throw (ex);
                tbox_thongtin.Text = canvansize.ToString() + (ex.ToString());


            }

        }
        private void pictureEdit1_MouseLeave(object sender, EventArgs e)
        {
            //cropPen = new Pen(Color.Green, 1);
            //cropPen.DashStyle = DashStyle.Solid;
            //pictureEdit1.CreateGraphics().DrawRectangle(cropPen, cropX, cropY, cropWidth, cropHeight);
        }

        /// <summary>
        /// convert int  sau khi thực hiện toán tử kiểu float với nhau trước 
        /// </summary>
        /// <param name="scale"> "tỉ lệ width-height giữa hình gốc và pictureEdit"</param>
        /// <param name="value">giá trị width-height của pictureEdit </param>
        /// <param name="operate"> "toán tử"</param>
        /// 
        public static int convertFloatToInt(string scale, string value, string operate)
        {
            float _scale = float.Parse(scale.Trim());
            float _value = float.Parse(value.Trim());

            int total = 0;

            switch (operate)
            {
                case "+":
                    total = Convert.ToInt32(_value + _scale);
                    break;
                case "-":
                    total = Convert.ToInt32(_value - _scale);
                    break;
                case "*":
                    total = Convert.ToInt32(_value * _scale);
                    break;
                case "/":
                    total = Convert.ToInt32(_value / _scale);
                    break;
                default:
                    total = 0;
                    break;
            }

            return total;

        }

        private void pictureEdit1_PopupMenuShowing(object sender, DevExpress.XtraEditors.Events.PopupMenuShowingEventArgs e)
        {

        }

        private void btn_Canvasize_Click(object sender, EventArgs e)
        {

            string folderName = buttonEdit1.Text;
            string pathString = "";
            string fileName = "";
            string filepath = pathString + "\\" + fileName;
            int dem = 0;
            layoutControlItem14.Visibility = LayoutVisibility.Always;
            panel1.Visible = true;
            if (pictureEdit1.Image == null)
                return;
            if (Convert.ToInt32(Width_domaniUpdown.Text) < 1)
            {
                return;
            }

            demFileSuccess = 0;
            demFileError = 0;

            label1.Text = "Error : " + demFileError.ToString();

            pathString = System.IO.Path.Combine(folderName, "Canvasize_Done");
            if (!System.IO.Directory.Exists(pathString)) System.IO.Directory.CreateDirectory(pathString);
            callThread_ResizeImage(1, "canvasize", numthreads, pathString);


        }




        public void callThread_ResizeImage(int numberthread, string name, int numthreads, string pathString)
        {
            //string fileName, string filepath, string pathString
            int Width_resize = Convert.ToInt32(Width_domaniUpdown.Text);
            int Height_resize = Convert.ToInt32(Height_domaniUpdown.Text);
            object obj = new object[4] { numberthread, pathString, Width_resize, Height_resize };
            Thread Thrd = new Thread(new ParameterizedThreadStart(Canvasize_Imagẹ̣̣));
            Thrd.Name = name + "_" + numberthread.ToString();
            Thrd.Start(obj);

            Thread.Sleep(100);
        }
        public void Canvasize_Imagẹ̣̣(object obj)
        {

            string filepath = "";
            string pathError = "";
            string error = "";
            Array objArray = new object[4];
            objArray = (Array)obj;
            int STT = Convert.ToInt32(objArray.GetValue(0));
            int Width_resize = Convert.ToInt32(objArray.GetValue(2));
            int Height_resize = Convert.ToInt32(objArray.GetValue(3));
            pathError = objArray.GetValue(1).ToString() + "\\" + "log_" + "log_Canvasize.txt";// đường dẫn của file muốn tạo
            error = pathError.Clone().ToString();


            if (!File.Exists(pathError))
            {
                FileStream fs = new FileStream(pathError, FileMode.Create);//Tạo file mới tên là test.txt            
            }


            for (int i = 0; i < Files.Count(); i++)
            {
                filepath = objArray.GetValue(1).ToString() + "\\" + Files[i].Name.ToString();
                try
                {

                    Bitmap bmp = new Bitmap(Files[i].ToString());//get image
                    Bitmap bmpResized = new Bitmap(bmp, Width_resize, Height_resize); //resizing image


                    bmpResized.Save(filepath);
                    bmp.Dispose();
                    bmpResized.Dispose();
                    demFileSuccess++;
                    SuccessFile(Files.Count());
                    WriteLog(Files[i].Name.ToString() + " done");
                }
                catch (Exception ex)
                {
                    WriteLogToTxt(error, Files[i].Name.ToString() + " error: " + ex.ToString());
                    WriteLog(Files[i].Name.ToString() + " error");
                    demFileError++;
                    ErrorFile();
                    continue;
                }
                progressBarLoading.Invoke(new MethodInvoker(delegate ()
                {
                    progressBarLoading.Value = (demFileSuccess * 100 / (Files.Count()));
                }));
            }


        }

        public void thread_Canvasize_Imagẹ̣̣()
        {

        }

        private void btn_CropCanvasize_Click(object sender, EventArgs e)
        {
            try
            {

                string folderName = buttonEdit1.Text;
                string pathString = "";
                string fileName = "";
                string filepath = pathString + "\\" + fileName;
                int dem = 0;
                layoutControlItem14.Visibility = LayoutVisibility.Always;
                panel1.Visible = true;
                if (pictureEdit1.Image == null)
                    return;
                if (Convert.ToInt32(Wresize_dmupdown.Text) < 1 || Convert.ToInt32(Hresize_dmupdown.Text) < 1)
                {
                    MessageBox.Show("Width > 0 & Height > 0");
                    return;
                }

                demFileSuccess = 0;
                demFileError = 0;
                label1.Text = "Error : " + demFileError.ToString();

                pathString = System.IO.Path.Combine(folderName, "CropCanvasize_Done");

                if (!System.IO.Directory.Exists(pathString)) System.IO.Directory.CreateDirectory(pathString);

                numthreads = ResizeImage.autoGetNumberFileOneThread(Files.Count(), numberFileOneThread);


                callThread_CropImage(1, "crop_", numthreads, pathString, btn_CropCanvasize.Name.ToString());

                progressBarLoading.Invoke(new MethodInvoker(delegate ()
                {
                    progressBarLoading.Value = 100;
                }));

            }
            catch (Exception ex)
            {
            }
        }

        private void buttonEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        //public void resize(string tempPath, string fname, string extension, int x, int y)
        //{

        //    string TempPath = Path.GetTempFileName();
        //    System.Drawing.Image img = System.Drawing.Image.FromFile(tempPath);

        //    using (MemoryStream memory = new MemoryStream())
        //    {
        //        Bitmap tnBitmap = new Bitmap(img);
        //        Graphics tnGraph = Graphics.FromImage(tnBitmap);
        //        tnGraph.CompositingQuality = CompositingQuality.HighQuality;
        //        //settings ..
        //        double ratioX = (double)x / (double)tnBitmap.Width;
        //        double ratioY = (double)y / (double)tnBitmap.Height;
        //        double ratio = ratioX < ratioY ? ratioX : ratioY;
        //        int newHeight = Convert.ToInt32(tnBitmap.Height * ratio);
        //        int newWidth = Convert.ToInt32(tnBitmap.Width * ratio);
        //        int posX = Convert.ToInt32((x - (tnBitmap.Width * ratio)) / 2);
        //        int posY = Convert.ToInt32((y - (tnBitmap.Height * ratio)) / 2);
        //        tnGraph.DrawImage(img, posX, posY, newWidth, newHeight);
        //        img.Dispose();
        //        using (FileStream fs = new FileStream(tempPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
        //        {
        //            tnBitmap.Save(memory, ImageFormat.Png);
        //            byte[] bytes = memory.ToArray();
        //            fs.Write(bytes, 0, bytes.Length);
        //            fs.Close();
        //            try
        //            {
        //                //FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://ftp.address.com/httpdocs/img-cdn" + @"\" + fname + extension);
        //                //req.UseBinary = true;
        //                //req.Method = WebRequestMethods.Ftp.UploadFile;
        //                //req.Credentials = new NetworkCredential("username", "pw");
        //                //StreamReader rdr = new StreamReader(memory);

        //                //rdr.Close();
        //                //req.ContentLength = memory.ToArray().Length;

        //                //Stream reqStream = req.GetRequestStream();

        //                //reqStream.Write(memory.ToArray(), 0, memory.ToArray().Length);
        //                //reqStream.Close();

        //                //        else
        //                //{
        //                //            pathString = System.IO.Path.Combine(folderName, "Canvasize_Done");


        //                //            if (!System.IO.Directory.Exists(pathString)) System.IO.Directory.CreateDirectory(pathString);
        //                //            for (canvansize = canvansize; canvansize < gridView1.DataRowCount; canvansize++)
        //                //            {
        //                //                Img.Dispose();

        //                //                DataRow row = gridView1.GetDataRow(canvansize);

        //                //                Img = Image.FromFile(gridView1.GetDataRow(canvansize)[2].ToString());
        //                //                fileName = gridView1.GetRowCellValue(canvansize, gridColumn2.FieldName).ToString();
        //                //                filepath = pathString + "\\" + fileName;
        //                //                newimage.Save(filepath, ImageFormat.Png);
        //                //                newimage.Dispose();

        //                //                canvansize++;
        //                //                if (canvansize == gridView1.DataRowCount)
        //                //                {
        //                //                    canvansize = 0;
        //                //                    break;
        //                //                }
        //                //                //ImageLoaderThread = new Thread(new ParameterizedThreadStart(LoadImage));
        //                //                SaveImage(Img, btn_Canvasize.Name);

        //                //                //gridView1.Dispose();
        //                //                oldimage.Dispose();

        //                //            }
        //                //        }




        //            }
        //            catch (Exception ex)
        //            {
        //                //String status = ((FtpWebResponse)e.Response).StatusDescription;
        //                throw (ex);
        //            }

        //        }
        //        memory.Dispose();
        //        tnBitmap.Dispose();
        //        tnGraph.Dispose();
        //    }
        //}


    }



}
