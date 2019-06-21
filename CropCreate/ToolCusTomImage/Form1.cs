using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ToolCusTomImage.Presentation_Layer;
using ToolCusTomImage.Presentation_Layer.IMAGE;
using CropCreate.Presentation_Layer.IMAGE;

namespace ToolCusTomImage
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            InitForm1();
        }

        /// <summary>
        /// Edit form before run
        /// </summary>
        private void InitForm1()
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }


        //check form 
        public Form CheckExists(Type ftype)
        {

            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        private void crop_canvasize_menustrip_Click(object sender, EventArgs e)
        {

            Form frm = this.CheckExists(typeof(Presentation_Layer.IMAGE.Crop_Canvasize));
            if (frm != null) frm.Activate();
            else
            {
                Presentation_Layer.IMAGE.Crop_Canvasize f = new Presentation_Layer.IMAGE.Crop_Canvasize()
                {
                    MdiParent = this,
                    Text = "Crop , Canvasize"
                };
                f.Show();
            }
        }

        private void createBannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form frm = this.CheckExists(typeof(Presentation_Layer.IMAGE.CreateAndUpload_Banner));
                if (frm != null) frm.Activate();
                else
                {
                    CreateAndUpload_Banner f = new CreateAndUpload_Banner()
                    {
                        MdiParent = this,
                        Text = "Create Banner"
                    };
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //UploadLogo f = new UploadLogo();
            //f.Show();
            try
            {
                Form frm = this.CheckExists(typeof(UploadLogo));
                if (frm != null) frm.Activate();
                else
                {
                    UploadLogo f = new UploadLogo()
                    {
                        MdiParent = this,
                        Text = "Upload Logo"
                    };
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
