using Microsoft.VisualBasic;
using Newtonsoft.Json;
using MyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolCusTomImage.Presentation_Layer.IMAGE
{
    public partial class CreateBanner : Form
    {
        static Accounts _acc = new Accounts();
        static Manager _manager = new Manager();
        static Template _curTemp = new Template();

        //static Accounts _acc = "";
        //static Manager _manager = "";
        //static Template _curTemp = "";

        private List<CheckBox> _list_Guy = new List<CheckBox>();
        private List<CheckBox> _list_Ladies = new List<CheckBox>();
        private List<CheckBox> _list_Hoodie = new List<CheckBox>();
        private List<CheckBox> _list_Sweatshirt = new List<CheckBox>();
        private List<CheckBox> _list_Guy_Vneck = new List<CheckBox>();
        private List<CheckBox> _list_Ladies_Vneck = new List<CheckBox>();
        private List<CheckBox> _list_Unisex_Tank_Tops = new List<CheckBox>();
        private List<CheckBox> _list_Unisex_Long_Sleeve = new List<CheckBox>();
        private List<CheckBox> _list_Youth = new List<CheckBox>();
        private List<CheckBox> _list_Hat = new List<CheckBox>();
        private List<CheckBox> _list_Trucker_Cap = new List<CheckBox>();
        private List<CheckBox> _list_Legging = new List<CheckBox>();
        private string _Folder_Temp = "Data\\Template";

        private List<string> _list_type_def = new List<string>();

        private int _total_guys = 0;
        private int _total_ladies = 0;
        private int _total_hoodie = 0;
        private int _total_sweatshirt = 0;
        private int _total_guys_vneck = 0;
        private int _total_ladies_vneck = 0;
        private int _total_unisex_tank_top = 0;
        private int _total_unisex_long_sleeve = 0;
        private int _total_youth = 0;

        static int _num_thread = 0;
        static int _sleep = 0;

        static object _lock = new object();
        static int _index = 0;
        public CreateBanner()
        {
           
            //if (string.IsNullOrWhiteSpace(_acc.SELLER_ID))
            //    Environment.Exit(0);
          
            InitializeComponent();           
        }

        private void WriteLog(string mess)
        {
            richTextBox1.Invoke((Action)(() => richTextBox1.AppendText(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n")));
        }

        private void CreateBanner_Load(object sender, EventArgs e)
        {
            #region check login success
            //if (!Directory.Exists(_Folder_Temp)) Directory.CreateDirectory(_Folder_Temp);
            ////check login
            //loginForm f = new loginForm(ref _manager, ref _acc);
            //f.ShowDialog();
            //if (!f.IsLoginDone)
            //{
            //    this.Close();
            //    //Application.Exit();
            //}
            #endregion

            //WriteLog("Login successfull!");
            //this.Text = string.Format("Welcome back {0}, {1}", _acc.EMAIL, _acc.SELLER_ID);

            //load list Template
            Refresh_List_Temp();

            //list_Legging
            _list_Legging.Add(checkBox1);

            //list guy
            _list_Guy.Add(chb_0_0);
            _list_Guy.Add(chb_0_1);
            _list_Guy.Add(chb_0_2);
            _list_Guy.Add(chb_0_3);
            _list_Guy.Add(chb_0_4);
            _list_Guy.Add(chb_0_5);
            _list_Guy.Add(chb_0_6);
            _list_Guy.Add(chb_0_7);
            _list_Guy.Add(chb_0_8);
            _list_Guy.Add(chb_0_9);
            _list_Guy.Add(chb_0_10);
            _list_Guy.Add(chb_0_11);
            _list_Guy.Add(chb_0_12);
            _list_Guy.Add(chb_0_13);
            _list_Guy.Add(chb_0_14);
            _list_Guy.Add(chb_0_15);
            _list_Guy.Add(chb_0_16);
            //list ladies
            _list_Ladies.Add(chb_1_0);
            _list_Ladies.Add(chb_1_1);
            _list_Ladies.Add(chb_1_2);
            _list_Ladies.Add(chb_1_3);
            _list_Ladies.Add(chb_1_4);
            _list_Ladies.Add(chb_1_5);
            _list_Ladies.Add(chb_1_6);
            _list_Ladies.Add(chb_1_7);
            _list_Ladies.Add(chb_1_8);
            _list_Ladies.Add(chb_1_9);
            _list_Ladies.Add(chb_1_10);
            _list_Ladies.Add(chb_1_11);
            _list_Ladies.Add(chb_1_12);
            _list_Ladies.Add(chb_1_13);
            _list_Ladies.Add(chb_1_14);
            _list_Ladies.Add(chb_1_15);
            _list_Ladies.Add(chb_1_16);
            //list hoodie
            _list_Hoodie.Add(chb_2_0);
            _list_Hoodie.Add(chb_2_1);
            _list_Hoodie.Add(chb_2_2);
            _list_Hoodie.Add(chb_2_3);
            _list_Hoodie.Add(chb_2_4);
            _list_Hoodie.Add(chb_2_5);
            _list_Hoodie.Add(chb_2_6);
            _list_Hoodie.Add(chb_2_7);
            _list_Hoodie.Add(chb_2_8);
            _list_Hoodie.Add(chb_2_9);

            //list sweatshirts
            _list_Sweatshirt.Add(chb_3_0);
            _list_Sweatshirt.Add(chb_3_1);
            _list_Sweatshirt.Add(chb_3_2);
            _list_Sweatshirt.Add(chb_3_3);
            _list_Sweatshirt.Add(chb_3_4);
            _list_Sweatshirt.Add(chb_3_5);
            _list_Sweatshirt.Add(chb_3_6);

            //list guy_vneck 
            _list_Guy_Vneck.Add(chb_4_0);
            _list_Guy_Vneck.Add(chb_4_1);
            _list_Guy_Vneck.Add(chb_4_2);
            _list_Guy_Vneck.Add(chb_4_3);
            _list_Guy_Vneck.Add(chb_4_4);

            //list ladies_vneck
            _list_Ladies_Vneck.Add(chb_5_0);
            _list_Ladies_Vneck.Add(chb_5_1);
            _list_Ladies_Vneck.Add(chb_5_2);
            _list_Ladies_Vneck.Add(chb_5_3);
            _list_Ladies_Vneck.Add(chb_5_4);
            _list_Ladies_Vneck.Add(chb_5_5);
            _list_Ladies_Vneck.Add(chb_5_6);

            //list Unisex tank tops
            _list_Unisex_Tank_Tops.Add(chb_6_0);
            _list_Unisex_Tank_Tops.Add(chb_6_1);
            _list_Unisex_Tank_Tops.Add(chb_6_2);
            _list_Unisex_Tank_Tops.Add(chb_6_3);
            _list_Unisex_Tank_Tops.Add(chb_6_4);
            _list_Unisex_Tank_Tops.Add(chb_6_5);

            //list unisex long sleeve
            _list_Unisex_Long_Sleeve.Add(chb_7_0);
            _list_Unisex_Long_Sleeve.Add(chb_7_1);
            _list_Unisex_Long_Sleeve.Add(chb_7_2);
            _list_Unisex_Long_Sleeve.Add(chb_7_3);
            _list_Unisex_Long_Sleeve.Add(chb_7_4);
            _list_Unisex_Long_Sleeve.Add(chb_7_5);

            //list youth
            _list_Youth.Add(chb_8_0);
            _list_Youth.Add(chb_8_1);
            _list_Youth.Add(chb_8_2);
            _list_Youth.Add(chb_8_3);
            _list_Youth.Add(chb_8_4);
            _list_Youth.Add(chb_8_5);
            _list_Youth.Add(chb_8_6);

            //list hat
            _list_Hat.Add(chb_11_0);
            _list_Hat.Add(chb_11_1);
            _list_Hat.Add(chb_11_2);
            _list_Hat.Add(chb_11_3);
            _list_Hat.Add(chb_11_4);

            //list trucker cap
            _list_Trucker_Cap.Add(chb_12_0);
            _list_Trucker_Cap.Add(chb_12_1);
            _list_Trucker_Cap.Add(chb_12_2);
            _list_Trucker_Cap.Add(chb_12_3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //read curtemp
            Template temp = ReadCurrentTemplate();

            //string name = Interaction.InputBox("name?");
            string name = "";
            if(!string.IsNullOrWhiteSpace(name))
            {
                File.WriteAllText(_Folder_Temp + "\\" + name + ".json", JsonConvert.SerializeObject(temp));
                MessageBox.Show("Saved!");
                Refresh_List_Temp();
            }
        }

        private Template ReadCurrentTemplate()
        {
            Template temp = new Template();
            //read infor
            temp.Title = txtTitle.Text;
            temp.Description = txtDesc.Text;
            temp.Keywords = txtKeywords.Text;
            temp.Collection = txbCollection.Text;
            if (checkBox2.Checked) temp.showStoreFront = true;
            //read def
            if (cobDefType.Text != "")
                temp.DefType = (Enums.ShirtsType)Enum.Parse(typeof(Enums.ShirtsType), cobDefType.Text);
            if (cobDefColor.Text != "")
                temp.DefColors = cobDefColor.Text;
            if (cobDefCategory.Text != "")
                temp.Category = (Enums.Category)Enum.Parse(typeof(Enums.Category), cobDefCategory.Text);
            if (rShirts.Checked)
                temp.UploadType = Enums.UploadType.Shirts;
            if (rLeggings.Checked)
            {
                temp.UploadType = Enums.UploadType.Leggings;
                UploadTypes types_leggings = new UploadTypes();
                types_leggings.id = 120;
                types_leggings.name = "Leggings";
                types_leggings.price = double.Parse(txtPriceLeggings.Text);
                types_leggings.colors.Add("Black");
                temp.Types.Add(types_leggings);
            }
            if (rMugs.Checked)
                temp.UploadType = Enums.UploadType.Mugs;
            if (rPosters.Checked)
                temp.UploadType = Enums.UploadType.Posters;
            if (rCavans.Checked)
            {
                temp.UploadType = Enums.UploadType.Cavans;
                UploadTypes types_cavans = new UploadTypes();
                types_cavans.id = 143;
                types_cavans.name = "Canvas 16x20";
                types_cavans.price = double.Parse(txtPrice_Cavans.Text);
                types_cavans.colors.Add("White");
                temp.Types.Add(types_cavans);
            }
            if (rHat.Checked)
                temp.UploadType = Enums.UploadType.Hat;
            if (rTruckerCap.Checked)
                temp.UploadType = Enums.UploadType.Trucker_Cap;
            temp.UploadBack = chbUpBack.Checked;

            // read hidden
            if (chb_hidden_1.Checked)
                temp.Hidden1 = true;
            if (chb_hidden_2.Checked)
                temp.Hidden2 = true;
            if (chb_hidden_3.Checked)
                temp.Hidden3 = true;

            //read colors

            //guys
            UploadTypes types_guys = new UploadTypes();
            foreach (var chb in _list_Guy)
            {
                if (chb.Checked)
                {
                    types_guys.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_guys.colors.Count > 0)
            {
                types_guys.id = 8;
                types_guys.name = "Guys Tee";
                types_guys.price = double.Parse(txtPrice_Guys.Text);
                temp.Types.Add(types_guys);
            }
            //ladies
            UploadTypes types_ladies = new UploadTypes();
            foreach (var chb in _list_Ladies)
            {
                if (chb.Checked)
                {
                    types_ladies.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_ladies.colors.Count > 0)
            {
                types_ladies.id = 34;
                types_ladies.name = "Ladies Tee";
                types_ladies.price = double.Parse(txtPrice_Ladies.Text);
                temp.Types.Add(types_ladies);
            }
            //hoodie
            UploadTypes types_hoodie = new UploadTypes();
            foreach (var chb in _list_Hoodie)
            {
                if (chb.Checked)
                {
                    types_hoodie.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_hoodie.colors.Count > 0)
            {
                types_hoodie.id = 19;
                types_hoodie.name = "Hoodie";
                types_hoodie.price = double.Parse(txtPrice_Hoodie.Text);
                temp.Types.Add(types_hoodie);
            }
            //sweatshirt
            UploadTypes types_sweatshirt = new UploadTypes();
            foreach (var chb in _list_Sweatshirt)
            {
                if (chb.Checked)
                {
                    types_sweatshirt.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_sweatshirt.colors.Count > 0)
            {
                types_sweatshirt.id = 27;
                types_sweatshirt.name = "Sweat Shirt";
                types_sweatshirt.price = double.Parse(txtPrice_Sweatshirt.Text);
                temp.Types.Add(types_sweatshirt);
            }
            //guy vneck
            UploadTypes types_guys_vneck = new UploadTypes();
            foreach (var chb in _list_Guy_Vneck)
            {
                if (chb.Checked)
                {
                    types_guys_vneck.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_guys_vneck.colors.Count > 0)
            {
                types_guys_vneck.id = 50;
                types_guys_vneck.name = "Guys V-Neck";
                types_guys_vneck.price = double.Parse(txtPrice_Guys_Vneck.Text);
                temp.Types.Add(types_guys_vneck);
            }
            //ladies vneck
            UploadTypes types_ladies_vneck = new UploadTypes();
            foreach (var chb in _list_Ladies_Vneck)
            {
                if (chb.Checked)
                {
                    types_ladies_vneck.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_ladies_vneck.colors.Count > 0)
            {
                types_ladies_vneck.id = 116;
                types_ladies_vneck.name = "Ladies V-Neck";
                types_ladies_vneck.price = double.Parse(txtPrice_Ladies_Vneck.Text);
                temp.Types.Add(types_ladies_vneck);
            }
            //unisex tank top
            UploadTypes types_unisex_tank_top = new UploadTypes();
            foreach (var chb in _list_Unisex_Tank_Tops)
            {
                if (chb.Checked)
                {
                    types_unisex_tank_top.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_unisex_tank_top.colors.Count > 0)
            {
                types_unisex_tank_top.id = 118;
                types_unisex_tank_top.name = "Unisex Tank Top";
                types_unisex_tank_top.price = double.Parse(txtPrice_Unisex_Tank_Tops.Text);
                temp.Types.Add(types_unisex_tank_top);
            }
            //unisex long sleeve
            UploadTypes types_unisex_long_sleeve = new UploadTypes();
            foreach (var chb in _list_Unisex_Long_Sleeve)
            {
                if (chb.Checked)
                {
                    types_unisex_long_sleeve.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_unisex_long_sleeve.colors.Count > 0)
            {
                types_unisex_long_sleeve.id = 119;
                types_unisex_long_sleeve.name = "Unisex Long Sleeve";
                types_unisex_long_sleeve.price = double.Parse(txtPrice_Unisex_Long_Sleeve.Text);
                temp.Types.Add(types_unisex_long_sleeve);
            }
            //youth
            UploadTypes types_youth = new UploadTypes();
            foreach (var chb in _list_Youth)
            {
                if (chb.Checked)
                {
                    types_youth.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_youth.colors.Count > 0)
            {
                types_youth.id = 35;
                types_youth.name = "Youth Tee";
                types_youth.price = double.Parse(txtPrice_Youth.Text);
                temp.Types.Add(types_youth);
            }
            //mug
            if(chb_9_0.Checked)
            {
                UploadTypes mug_colored = new UploadTypes();
                mug_colored.id = 128;
                mug_colored.name = "Coffee Mug (colored)";
                mug_colored.price = double.Parse(txtPrice_Mugs_Colored.Text);
                mug_colored.colors.Add("Black");
                temp.Types.Add(mug_colored);
            }
            if (chb_9_1.Checked)
            {
                UploadTypes mug_white = new UploadTypes();
                mug_white.id = 129;
                mug_white.name = "Coffee Mug (white)";
                mug_white.price = double.Parse(txtPrice_Mugs_White.Text);
                mug_white.colors.Add("White");
                temp.Types.Add(mug_white);
            }
            if (chb_9_2.Checked)
            {
                UploadTypes mug_color_change = new UploadTypes();
                mug_color_change.id = 145;
                mug_color_change.name = "Coffee Mug (color change)";
                mug_color_change.price = double.Parse(txtPrice_Mugs_Color_Change.Text);
                mug_color_change.colors.Add("White");
                temp.Types.Add(mug_color_change);
            }
            //poster
            if(chb_10_0.Checked)
            {
                UploadTypes posters_16x24 = new UploadTypes();
                posters_16x24.id = 137;
                posters_16x24.name = "Posters 16x24";
                posters_16x24.price = double.Parse(txtPirce_Posters_16x24.Text);
                posters_16x24.colors.Add("White");
                temp.Types.Add(posters_16x24);
            }
            if (chb_10_1.Checked)
            {
                UploadTypes posters_24x16 = new UploadTypes();
                posters_24x16.id = 138;
                posters_24x16.name = "Posters 24x16";
                posters_24x16.price = double.Parse(txtPirce_Posters_24x16.Text);
                posters_24x16.colors.Add("White");
                temp.Types.Add(posters_24x16);
            }
            if (chb_10_2.Checked)
            {
                UploadTypes posters_11x17 = new UploadTypes();
                posters_11x17.id = 139;
                posters_11x17.name = "Posters 11x17";
                posters_11x17.price = double.Parse(txtPirce_Posters_11x17.Text);
                posters_11x17.colors.Add("White");
                temp.Types.Add(posters_11x17);
            }
            if (chb_10_3.Checked)
            {
                UploadTypes posters_17x11 = new UploadTypes();
                posters_17x11.id = 140;
                posters_17x11.name = "Posters 17x11";
                posters_17x11.price = double.Parse(txtPirce_Posters_17x11.Text);
                posters_17x11.colors.Add("White");
                temp.Types.Add(posters_17x11);
            }

            //hat
            UploadTypes types_hat = new UploadTypes();
            foreach (var chb in _list_Hat)
            {
                if (chb.Checked)
                {
                    types_hat.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_hat.colors.Count > 0)
            {
                types_hat.id = 147;
                types_hat.name = "Hat";
                types_hat.price = double.Parse(txtPrice_Hat.Text);
                temp.Types.Add(types_hat);
            }
            //trucker cap
            UploadTypes types_trucker_cap = new UploadTypes();
            foreach (var chb in _list_Trucker_Cap)
            {
                if (chb.Checked)
                {
                    types_trucker_cap.colors.Add(toolTip1.GetToolTip(chb));
                }
            }
            if (types_trucker_cap.colors.Count > 0)
            {
                types_trucker_cap.id = 148;
                types_trucker_cap.name = "Trucker Cap";
                types_trucker_cap.price = double.Parse(txtPrice_Trucker_Cap.Text);
                temp.Types.Add(types_trucker_cap);
            }

            return temp;
        }

        //Refresh func
        private void Refresh_List_Temp()
        {
            cobTemplate.Items.Clear();
            string[] aFiles_Temp = Directory.GetFiles(_Folder_Temp, "*.json");
            for (int i = 0; i < aFiles_Temp.Length; i++)
            {
                cobTemplate.Items.Add(Path.GetFileNameWithoutExtension(aFiles_Temp[i]));
            }
        }

        private void cobTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Template temp = JsonConvert.DeserializeObject<Template>(File.ReadAllText(_Folder_Temp + "\\" + cobTemplate.Text + ".json"));
                //load infor
                txtTitle.Text = temp.Title;
                txtDesc.Text = temp.Description;
                txtKeywords.Text = temp.Keywords;
                txbCollection.Text = temp.Collection;
                //load hidden
                if (temp.Hidden1)
                    chb_hidden_1.Checked = true;
                else
                    chb_hidden_1.Checked = false;
                if (temp.Hidden2)
                    chb_hidden_2.Checked = true;
                else
                    chb_hidden_2.Checked = false;
                if (temp.Hidden3)
                    chb_hidden_3.Checked = true;
                else
                    chb_hidden_3.Checked = false;

                //reset upload types
                rShirts.Checked = false;
                rLeggings.Checked = false;
                rMugs.Checked = false;
                rPosters.Checked = false;
                rCavans.Checked = false;
                rHat.Checked = false;
                rTruckerCap.Checked = false;
                switch(temp.UploadType)
                {
                    case Enums.UploadType.Shirts:
                        rShirts.Checked = true;
                        break;
                    case Enums.UploadType.Leggings:
                        rLeggings.Checked = true;
                        break;
                    case Enums.UploadType.Mugs:
                        rMugs.Checked = true;
                        break;
                    case Enums.UploadType.Posters:
                        rPosters.Checked = true;
                        break;
                    case Enums.UploadType.Cavans:
                        rCavans.Checked = true;
                        break;
                    case Enums.UploadType.Hat:
                        rHat.Checked = true;
                        break;
                    case Enums.UploadType.Trucker_Cap:
                        rTruckerCap.Checked = true;
                        break;
                }
                //load color
                //clear all color
                foreach (var chb in _list_Guy)
                    chb.Checked = false;
                foreach (var chb in _list_Ladies)
                    chb.Checked = false;
                foreach (var chb in _list_Hoodie)
                    chb.Checked = false;
                foreach (var chb in _list_Sweatshirt)
                    chb.Checked = false;
                foreach (var chb in _list_Guy_Vneck)
                    chb.Checked = false;
                foreach (var chb in _list_Ladies_Vneck)
                    chb.Checked = false;
                foreach (var chb in _list_Unisex_Tank_Tops)
                    chb.Checked = false;
                foreach (var chb in _list_Unisex_Long_Sleeve)
                    chb.Checked = false;
                foreach (var chb in _list_Youth)
                    chb.Checked = false;
                foreach (var chb in _list_Hat)
                    chb.Checked = false;
                foreach (var chb in _list_Trucker_Cap)
                    chb.Checked = false;
                //mug
                chb_9_0.Checked = false;
                chb_9_1.Checked = false;
                chb_9_2.Checked = false;
                //poster
                chb_10_0.Checked = false;
                chb_10_1.Checked = false;
                chb_10_2.Checked = false;
                chb_10_3.Checked = false;
                foreach (UploadTypes types in temp.Types)
                {
                    switch(types.id)
                    {
                        case 8:// guy
                            txtPrice_Guys.Text = types.price.ToString();
                            foreach(string color in types.colors)
                            {
                                switch(color)
                                {
                                    case "White":
                                        _list_Guy[0].Checked = true;
                                        break;
                                    case "Sports Grey":
                                        _list_Guy[1].Checked = true;
                                        break;
                                    case "Dark Grey":
                                        _list_Guy[2].Checked = true;
                                        break;
                                    case "Brown":
                                        _list_Guy[3].Checked = true;
                                        break;
                                    case "Light Pink":
                                        _list_Guy[4].Checked = true;
                                        break;
                                    case "Hot Pink":
                                        _list_Guy[5].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Guy[6].Checked = true;
                                        break;
                                    case "Orange":
                                        _list_Guy[7].Checked = true;
                                        break;
                                    case "Yellow":
                                        _list_Guy[8].Checked = true;
                                        break;
                                    case "Green":
                                        _list_Guy[9].Checked = true;
                                        break;
                                    case "Forest":
                                        _list_Guy[10].Checked = true;
                                        break;
                                    case "Light Blue":
                                        _list_Guy[11].Checked = true;
                                        break;
                                    case "Royal Blue":
                                        _list_Guy[12].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Guy[13].Checked = true;
                                        break;
                                    case "Purple":
                                        _list_Guy[14].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Guy[15].Checked = true;
                                        break;
                                    case "Maroon":
                                        _list_Guy[16].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 34:// ladies
                            txtPrice_Ladies.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "White":
                                        _list_Ladies[0].Checked = true;
                                        break;
                                    case "Sports Grey":
                                        _list_Ladies[1].Checked = true;
                                        break;
                                    case "Dark Grey":
                                        _list_Ladies[2].Checked = true;
                                        break;
                                    case "Brown":
                                        _list_Ladies[3].Checked = true;
                                        break;
                                    case "Light Pink":
                                        _list_Ladies[4].Checked = true;
                                        break;
                                    case "Hot Pink":
                                        _list_Ladies[5].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Ladies[6].Checked = true;
                                        break;
                                    case "Orange":
                                        _list_Ladies[7].Checked = true;
                                        break;
                                    case "Yellow":
                                        _list_Ladies[8].Checked = true;
                                        break;
                                    case "Green":
                                        _list_Ladies[9].Checked = true;
                                        break;
                                    case "Forest":
                                        _list_Ladies[10].Checked = true;
                                        break;
                                    case "Light Blue":
                                        _list_Ladies[11].Checked = true;
                                        break;
                                    case "Royal Blue":
                                        _list_Ladies[12].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Ladies[13].Checked = true;
                                        break;
                                    case "Purple":
                                        _list_Ladies[14].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Ladies[15].Checked = true;
                                        break;
                                    case "Maroon":
                                        _list_Ladies[16].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 35:// youth
                            txtPrice_Youth.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "White":
                                        _list_Youth[0].Checked = true;
                                        break;
                                    case "Sports Grey":
                                        _list_Youth[1].Checked = true;
                                        break;
                                    case "Dark Grey":
                                        _list_Youth[2].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Youth[3].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Youth[4].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Youth[5].Checked = true;
                                        break;
                                    case "Royal Blue":
                                        _list_Youth[6].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 19:// hoodie
                            txtPrice_Hoodie.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "White":
                                        _list_Hoodie[0].Checked = true;
                                        break;
                                    case "Sports Grey":
                                        _list_Hoodie[1].Checked = true;
                                        break;
                                    case "Maroon":
                                        _list_Hoodie[2].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Hoodie[3].Checked = true;
                                        break;
                                    case "Royal Blue":
                                        _list_Hoodie[4].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Hoodie[5].Checked = true;
                                        break;
                                    case "Charcoal":
                                        _list_Hoodie[6].Checked = true;
                                        break;
                                    case "Forest":
                                        _list_Hoodie[7].Checked = true;
                                        break;
                                    case "Green":
                                        _list_Hoodie[8].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Hoodie[9].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 27: //Sweat shirt
                            txtPrice_Sweatshirt.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "White":
                                        _list_Sweatshirt[0].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Sweatshirt[1].Checked = true;
                                        break;
                                    case "Forest":
                                        _list_Sweatshirt[2].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Sweatshirt[3].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Sweatshirt[4].Checked = true;
                                        break;
                                    case "Royal Blue":
                                        _list_Sweatshirt[5].Checked = true;
                                        break;
                                    case "Sports Grey":
                                        _list_Sweatshirt[6].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 50: //Guy Vneck
                            txtPrice_Guys_Vneck.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "White":
                                        _list_Guy_Vneck[0].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Guy_Vneck[1].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Guy_Vneck[2].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Guy_Vneck[3].Checked = true;
                                        break;
                                    case "Sports Grey":
                                        _list_Guy_Vneck[4].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 116://Ladies Vneck
                            txtPrice_Ladies_Vneck.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "White":
                                        _list_Ladies_Vneck[0].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Ladies_Vneck[1].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Ladies_Vneck[2].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Ladies_Vneck[3].Checked = true;
                                        break;
                                    case "Royal Blue":
                                        _list_Ladies_Vneck[4].Checked = true;
                                        break;
                                    case "Dark Grey":
                                        _list_Ladies_Vneck[5].Checked = true;
                                        break;
                                    case "Purple":
                                        _list_Ladies_Vneck[6].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 118://Unisex Tank Tops
                            txtPrice_Unisex_Tank_Tops.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "White":
                                        _list_Unisex_Tank_Tops[0].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Unisex_Tank_Tops[1].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Unisex_Tank_Tops[2].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Unisex_Tank_Tops[3].Checked = true;
                                        break;
                                    case "Royal Blue":
                                        _list_Unisex_Tank_Tops[4].Checked = true;
                                        break;
                                    case "Sports Grey":
                                        _list_Unisex_Tank_Tops[5].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 119://Unisex Long Sleeve
                            txtPrice_Unisex_Long_Sleeve.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "White":
                                        _list_Unisex_Long_Sleeve[0].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Unisex_Long_Sleeve[1].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Unisex_Long_Sleeve[2].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Unisex_Long_Sleeve[3].Checked = true;
                                        break;
                                    case "Dark Grey":
                                        _list_Unisex_Long_Sleeve[4].Checked = true;
                                        break;
                                    case "Sports Grey":
                                        _list_Unisex_Long_Sleeve[5].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 120://legging
                            txtPriceLeggings.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {                                    
                                    case "Black":
                                        _list_Legging[0].Checked = true;
                                        break;                                   
                                }
                            }
                            break;
                        case 128://coffee mug colored
                            chb_9_0.Checked = true;
                            break;
                        case 129://coffee mug white
                            chb_9_1.Checked = true;
                            break;
                        case 145://coffee mug color change
                            chb_9_2.Checked = true;
                            break;
                        case 137://Posters 16x24
                            chb_10_0.Checked = true;
                            break;
                        case 138://Posters 24x16
                            chb_10_1.Checked = true;
                            break;
                        case 139://Posters 11x17
                            chb_10_2.Checked = true;
                            break;
                        case 140://Posters 17x11
                            chb_10_3.Checked = true;
                            break;
                        case 147://Hat
                            txtPrice_Hat.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "Black":
                                        _list_Hat[0].Checked = true;
                                        break;
                                    case "Green":
                                        _list_Hat[1].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Hat[2].Checked = true;
                                        break;
                                    case "Red":
                                        _list_Hat[3].Checked = true;
                                        break;
                                    case "Royal Blue":
                                        _list_Hat[4].Checked = true;
                                        break;
                                }
                            }
                            break;
                        case 148://trucker cap
                            txtPrice_Trucker_Cap.Text = types.price.ToString();
                            foreach (string color in types.colors)
                            {
                                switch (color)
                                {
                                    case "White":
                                        _list_Trucker_Cap[0].Checked = true;
                                        break;
                                    case "Black":
                                        _list_Trucker_Cap[1].Checked = true;
                                        break;
                                    case "Navy Blue":
                                        _list_Trucker_Cap[2].Checked = true;
                                        break;
                                    case "Dark Grey":
                                        _list_Trucker_Cap[3].Checked = true;
                                        break;
                                }
                            }
                            break;
                    }
                }
                //load def
                cobDefCategory.Text = temp.Category.ToString();
                cobDefType.Text = temp.DefType.ToString();
                cobDefColor.Text = temp.DefColors;
                if (temp.UploadBack)
                    chbUpBack.Checked = true;
                else
                    chbUpBack.Checked = false;
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string select_temp = cobTemplate.Text;
            if (string.IsNullOrWhiteSpace(select_temp))
                return;
            try
            {
                File.Delete(_Folder_Temp + "\\" + select_temp + ".json");
                Refresh_List_Temp();
            }
            catch { }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _curTemp = ReadCurrentTemplate();
            if (!int.TryParse(txtThread.Text, out _num_thread))
            {
                MessageBox.Show("Nhap so luong thread hop le");
                return;
            }
            if (!int.TryParse(txtSleep.Text, out _sleep))
            {
                MessageBox.Show("Nhap thoi gian sleep hop le");
                return;
            }
            _sleep *= 1000;
            WriteLog("> num thread: " + _num_thread);
            WriteLog("> sleep: " + _sleep);
            btnStart.Enabled = false;
            Thread tManager = new Thread(() =>
            {
                //khoi tao thread
                _index = -1;
                List<Thread> threads = new List<Thread>();
                for (int i = 0; i < _num_thread; i++)
                {
                    Thread t = new Thread(Thread_upload);
                    threads.Add(t);
                    t.Start(i);
                    Thread.Sleep(5000);
                }
                
                //int index = -1;
                //while(true)
                //{
                //    bool done = false;
                //    for(int i =0; i<_num_thread; i++)
                //    {
                //        index += 1;
                //        string filePNG = "";
                //        listView1.Invoke((Action)(() =>
                //        {
                //            if (index >= listView1.Items.Count)
                //            {
                //                done = true;
                //            }
                //            else
                //                filePNG = listView1.Items[_index].SubItems[1].Text;
                //        }));
                //        if (done)
                //            break;
                //        if (string.IsNullOrWhiteSpace(filePNG))
                //            continue;
                //        Thread th = new Thread(Upload(index, filePNG, _curTemp))
                //        {
                //            IsBackground = true
                //        };
                //        th.Start();
                //        threads.Add(th);
                //        Thread.Sleep(100);
                //    }
                //    foreach (Thread thread in threads)
                //    {
                //        thread.Join();
                //    }
                //    if (done)
                //        break;
                //    Thread.Sleep(_sleep);
                //}
                while(true)
                {
                    bool done = true;
                    for(int i =0; i<threads.Count;i++)
                    {
                        if(threads[i].IsAlive)
                        {
                            done = false;
                            break;
                        }
                    }
                    if (done)
                        break;
                    Thread.Sleep(1000);
                }
                btnStart.Invoke((Action)(()=>btnStart.Enabled = true));
                MessageBox.Show("Hoan thanh!");
            });
            tManager.Start();
        }

        private void Thread_upload(object obj)
        {
            int STT = (int)obj;
            int _tmp_index;
            while (true)
            {
                bool done = false;
                lock(_lock)
                {
                    _index += 1;
                    _tmp_index = _index;
                }
                string filePNG = "";

                listView1.Invoke((Action)(() =>
                {
                    if (_tmp_index >= listView1.Items.Count)
                    {
                        done = true;
                        return;
                    }
                    else
                        filePNG = listView1.Items[_tmp_index].SubItems[1].Text;
                }));
                if (done)
                    break;
                if (string.IsNullOrWhiteSpace(filePNG))
                    continue;
                WriteLog("Thread "+STT+": start upload " + Path.GetFileNameWithoutExtension(filePNG));
                //upload
                var result = _manager.Upload(filePNG, _curTemp);
                //update result
                listView1.Invoke((Action)(() =>
                {
                    if (!string.IsNullOrWhiteSpace(result.Link))
                    {
                        listView1.Items[_tmp_index].SubItems[2].Text = result.Link;
                        result.Description = "OK";
                        MoveToUploaded(filePNG);
                        WriteFileLog(result);
                    }
                    else
                    {
                        listView1.Items[_tmp_index].SubItems[2].Text = "get link error";
                    }
                    listView1.Items[_tmp_index].SubItems[3].Text = result.Description;
                }));
                Thread.Sleep(_sleep);
            }
            WriteLog("Thread " + STT + ": Stop");
        }

        private void MoveToUploaded(string path)
        {
            string filename = Path.GetFileName(path);
            string folder = Path.GetDirectoryName(path);
            string folder_ok = folder + "\\OK";
            if (!Directory.Exists(folder_ok))
                Directory.CreateDirectory(folder_ok);
            string filesave = folder_ok +"\\"+ filename;

            if (File.Exists(filesave))
            {
                File.Delete(filesave);
            }

            try
            {
                File.Move(path, filesave);
                WriteLog(" Move file " + path + " to " + filesave + " OK!");
            }
            catch
            {
                WriteLog(" Move file " + path + " to "+filesave+" error!");
            }
        }

        private void WriteFileLog(Result result)
        {
            string log = result.Title + "|" + result.Image + "|" + result.Link + "\r\n";
            string today = DateTime.Today.ToString("dd-MM-yyyy");
            string file_log = "Data\\Logs\\" + today + ".txt";
            while(true)
            {
                try
                {
                    File.AppendAllText(file_log,log);
                    break;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                System.Threading.Thread.Sleep(100);
            }
        }

        private void addDesignsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "PNG|*.png";
            if(open.ShowDialog()!=DialogResult.OK)
                return;
            foreach(string file in open.FileNames)
            {
                listView1.Items.Add(new ListViewItem(new string[] { Path.GetFileNameWithoutExtension(file), file,"","" }));
            }
            open.Dispose();
        }


        private void removesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = listView1.SelectedItems;
            if (selected.Count < 1)
                return;
            for (int i = selected.Count - 1; i >= 0; i--)
                listView1.Items.Remove(selected[i]);
        }

        private void openOnBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = listView1.SelectedItems;
            if (selected.Count < 1)
                return;
            for (int i = 0; i <selected.Count; i++)
            {
                string link = selected[i].SubItems[2].Text;
                if (!string.IsNullOrWhiteSpace(link))
                {
                    Process.Start(link);
                    Thread.Sleep(150);
                }
            }
        }

        private void btnClearColor_Click(object sender, EventArgs e)
        {
            foreach (var chb in _list_Guy)
                chb.Checked = false;
            foreach (var chb in _list_Ladies)
                chb.Checked = false;
            foreach (var chb in _list_Hoodie)
                chb.Checked = false;
            foreach (var chb in _list_Sweatshirt)
                chb.Checked = false;
            foreach (var chb in _list_Guy_Vneck)
                chb.Checked = false;
            foreach (var chb in _list_Ladies_Vneck)
                chb.Checked = false;
            foreach (var chb in _list_Unisex_Tank_Tops)
                chb.Checked = false;
            foreach (var chb in _list_Unisex_Long_Sleeve)
                chb.Checked = false;
            foreach (var chb in _list_Youth)
                chb.Checked = false;
            foreach (var chb in _list_Hat)
                chb.Checked = false;
            foreach (var chb in _list_Trucker_Cap)
                chb.Checked = false;
            //mug
            chb_9_0.Checked = false;
            chb_9_1.Checked = false;
            chb_9_2.Checked = false;
            //poster
            chb_10_0.Checked = false;
            chb_10_1.Checked = false;
            chb_10_2.Checked = false;
            chb_10_3.Checked = false;
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = listView1.Items.Count-1; i>=0; i--)
            {
                listView1.Items.RemoveAt(i);
            }
        }

        private void Check_Guys(CheckBox cob)
        {
            if (cob.Checked)
            {
                if (!_list_type_def.Contains("Guys"))
                {
                    _list_type_def.Add("Guys");
                    cobDefType.Items.Add("Guys");
                }
                _total_guys += 1;
                if (_total_guys > 5)
                {
                    cob.Checked = false;
                    MessageBox.Show("Limited Colors");
                    return;
                }
            }
            else
            {
                _total_guys -= 1;
                if (_total_guys <= 0)
                {
                    _list_type_def.Remove("Guys");
                    cobDefType.Items.Remove("Guys");
                }
            }
        }

        private void Check_Laides(CheckBox cob)
        {
            if (cob.Checked)
            {
                if (!_list_type_def.Contains("Ladies"))
                {
                    _list_type_def.Add("Ladies");
                    cobDefType.Items.Add("Ladies");
                }
                _total_ladies += 1;
                if (_total_ladies > 5)
                {
                    cob.Checked = false;
                    MessageBox.Show("Limited Colors");
                    return;
                }
            }
            else
            {
                _total_ladies -= 1;
                if (_total_ladies <= 0)
                {
                    _list_type_def.Remove("Ladies");
                    cobDefType.Items.Remove("Ladies");
                }
            }
        }

        private void Check_Youth(CheckBox cob)
        {
            if (cob.Checked)
            {
                if (!_list_type_def.Contains("Youth"))
                {
                    _list_type_def.Add("Youth");
                    cobDefType.Items.Add("Youth");
                }
                _total_youth += 1;
                if (_total_youth > 5)
                {
                    cob.Checked = false;
                    MessageBox.Show("Limited Colors");
                    return;
                }
            }
            else
            {
                _total_youth -= 1;
                if (_total_youth <= 0)
                {
                    _list_type_def.Remove("Youth");
                    cobDefType.Items.Remove("Youth");
                }
            }
        }

        private void Check_Hoodie(CheckBox cob)
        {
            if (cob.Checked)
            {
                if (!_list_type_def.Contains("Hoodie"))
                {
                    _list_type_def.Add("Hoodie");
                    cobDefType.Items.Add("Hoodie");
                }
                _total_hoodie += 1;
                if (_total_hoodie > 5)
                {
                    cob.Checked = false;
                    MessageBox.Show("Limited Colors");
                    return;
                }
            }
            else
            {
                _total_hoodie -= 1;
                if (_total_hoodie <= 0)
                {
                    _list_type_def.Remove("Hoodie");
                    cobDefType.Items.Remove("Hoodie");
                }
            }
        }

        private void Check_Sweatshirt(CheckBox cob)
        {
            if (cob.Checked)
            {
                if (!_list_type_def.Contains("Sweatshirt"))
                {
                    _list_type_def.Add("Sweatshirt");
                    cobDefType.Items.Add("Sweatshirt");
                }
                _total_sweatshirt += 1;
                if (_total_sweatshirt > 5)
                {
                    cob.Checked = false;
                    MessageBox.Show("Limited Colors");
                    return;
                }
            }
            else
            {
                _total_sweatshirt -= 1;
                if (_total_sweatshirt <= 0)
                {
                    _list_type_def.Remove("Sweatshirt");
                    cobDefType.Items.Remove("Sweatshirt");
                }
            }
        }

        private void Check_Guys_VNeck(CheckBox cob)
        {
            if (cob.Checked)
            {
                if (!_list_type_def.Contains("Guys_VNeck"))
                {
                    _list_type_def.Add("Guys_VNeck");
                    cobDefType.Items.Add("Guys_VNeck");
                }
                _total_guys_vneck += 1;
                if (_total_guys_vneck > 5)
                {
                    cob.Checked = false;
                    MessageBox.Show("Limited Colors");
                    return;
                }
            }
            else
            {
                _total_guys_vneck -= 1;
                if (_total_guys_vneck <= 0)
                {
                    _list_type_def.Remove("Guys_VNeck");
                    cobDefType.Items.Remove("Guys_VNeck");
                }
            }
        }

        private void Check_Ladies_VNeck(CheckBox cob)
        {
            if (cob.Checked)
            {
                if (!_list_type_def.Contains("Ladies_VNeck"))
                {
                    _list_type_def.Add("Ladies_VNeck");
                    cobDefType.Items.Add("Ladies_VNeck");
                }
                _total_ladies_vneck += 1;
                if (_total_ladies_vneck > 5)
                {
                    cob.Checked = false;
                    MessageBox.Show("Limited Colors");
                    return;
                }
            }
            else
            {
                _total_ladies_vneck -= 1;
                if (_total_ladies_vneck <= 0)
                {
                    _list_type_def.Remove("Ladies_VNeck");
                    cobDefType.Items.Remove("Ladies_VNeck");
                }
            }
        }

        private void Check_Unisex_Tank_Tops(CheckBox cob)
        {
            if (cob.Checked)
            {
                if (!_list_type_def.Contains("Unisex_Tank_Tops"))
                {
                    _list_type_def.Add("Unisex_Tank_Tops");
                    cobDefType.Items.Add("Unisex_Tank_Tops");
                }
                _total_unisex_tank_top += 1;
                if (_total_unisex_tank_top > 5)
                {
                    cob.Checked = false;
                    MessageBox.Show("Limited Colors");
                    return;
                }
            }
            else
            {
                _total_unisex_tank_top -= 1;
                if (_total_unisex_tank_top <= 0)
                {
                    _list_type_def.Remove("Unisex_Tank_Tops");
                    cobDefType.Items.Remove("Unisex_Tank_Tops");
                }
            }
        }

        private void Check_Unisex_Long_Sleeve(CheckBox cob)
        {
            if (cob.Checked)
            {
                if (!_list_type_def.Contains("Unisex_Long_Sleeve"))
                {
                    _list_type_def.Add("Unisex_Long_Sleeve");
                    cobDefType.Items.Add("Unisex_Long_Sleeve");
                }
                _total_unisex_long_sleeve += 1;
                if (_total_unisex_long_sleeve > 5)
                {
                    cob.Checked = false;
                    MessageBox.Show("Limited Colors");
                    return;
                }
            }
            else
            {
                _total_unisex_long_sleeve -= 1;
                if (_total_unisex_long_sleeve <= 0)
                {
                    _list_type_def.Remove("Unisex_Long_Sleeve");
                    cobDefType.Items.Remove("Unisex_Long_Sleeve");
                }
            }
        }

        private void chb_0_0_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_5_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_6_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_7_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_8_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_9_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_10_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_11_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_12_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_13_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_14_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_15_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void chb_0_16_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys((CheckBox)sender);
        }

        private void cobDefType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _curTemp = ReadCurrentTemplate();
            string text = cobDefType.Text;
            cobDefColor.Items.Clear();
            switch(text)
            {
                case "Guys":
                    cobDefColor.Items.AddRange(_curTemp.Types.Find(x => x.id == 8).colors.ToArray());
                    break;
                case "Ladies":
                    cobDefColor.Items.AddRange(_curTemp.Types.Find(x => x.id == 34).colors.ToArray());
                    break;
                case "Youth":
                    cobDefColor.Items.AddRange(_curTemp.Types.Find(x => x.id == 35).colors.ToArray());
                    break;
                case "Hoodie":
                    cobDefColor.Items.AddRange(_curTemp.Types.Find(x => x.id == 19).colors.ToArray());
                    break;
                case "Sweatshirt":
                    cobDefColor.Items.AddRange(_curTemp.Types.Find(x => x.id == 27).colors.ToArray());
                    break;
                case "Guys_VNeck":
                    cobDefColor.Items.AddRange(_curTemp.Types.Find(x => x.id == 50).colors.ToArray());
                    break;
                case "Ladies_VNeck":
                    cobDefColor.Items.AddRange(_curTemp.Types.Find(x => x.id == 116).colors.ToArray());
                    break;
                case "Unisex_Tank_Tops":
                    cobDefColor.Items.AddRange(_curTemp.Types.Find(x => x.id == 118).colors.ToArray());
                    break;
                case "Unisex_Long_Sleeve":
                    cobDefColor.Items.AddRange(_curTemp.Types.Find(x => x.id == 119).colors.ToArray());
                    break;
            }
        }

        private void chb_1_0_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_5_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_6_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_7_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_8_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_9_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_10_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_11_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_12_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_13_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_14_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_15_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_1_16_CheckedChanged(object sender, EventArgs e)
        {
            Check_Laides((CheckBox)sender);
        }

        private void chb_2_0_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_2_1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_2_2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_2_3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_2_4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_2_5_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_2_6_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_2_7_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_2_8_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_2_9_CheckedChanged(object sender, EventArgs e)
        {
            Check_Hoodie((CheckBox)sender);
        }

        private void chb_3_1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Sweatshirt((CheckBox)sender);
        }

        private void chb_3_0_CheckedChanged(object sender, EventArgs e)
        {
            Check_Sweatshirt((CheckBox)sender);
        }

        private void chb_3_2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Sweatshirt((CheckBox)sender);
        }

        private void chb_3_3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Sweatshirt((CheckBox)sender);
        }

        private void chb_3_4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Sweatshirt((CheckBox)sender);
        }

        private void chb_3_5_CheckedChanged(object sender, EventArgs e)
        {
            Check_Sweatshirt((CheckBox)sender);
        }

        private void chb_3_6_CheckedChanged(object sender, EventArgs e)
        {
            Check_Sweatshirt((CheckBox)sender);
        }

        private void chb_4_0_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys_VNeck((CheckBox)sender);
        }

        private void chb_4_1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys_VNeck((CheckBox)sender);
        }

        private void chb_4_2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys_VNeck((CheckBox)sender);
        }

        private void chb_4_3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys_VNeck((CheckBox)sender);
        }

        private void chb_4_4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Guys_VNeck((CheckBox)sender);
        }

        private void chb_5_0_CheckedChanged(object sender, EventArgs e)
        {
            Check_Ladies_VNeck((CheckBox)sender);
        }

        private void chb_5_1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Ladies_VNeck((CheckBox)sender);
        }

        private void chb_5_2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Ladies_VNeck((CheckBox)sender);
        }

        private void chb_5_3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Ladies_VNeck((CheckBox)sender);
        }

        private void chb_5_4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Ladies_VNeck((CheckBox)sender);
        }

        private void chb_5_5_CheckedChanged(object sender, EventArgs e)
        {
            Check_Ladies_VNeck((CheckBox)sender);
        }

        private void chb_5_6_CheckedChanged(object sender, EventArgs e)
        {
            Check_Ladies_VNeck((CheckBox)sender);
        }

        private void chb_6_0_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Tank_Tops((CheckBox)sender);
        }

        private void chb_6_1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Tank_Tops((CheckBox)sender);
        }

        private void chb_6_2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Tank_Tops((CheckBox)sender);
        }

        private void chb_6_3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Tank_Tops((CheckBox)sender);
        }

        private void chb_6_4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Tank_Tops((CheckBox)sender);
        }

        private void chb_6_5_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Tank_Tops((CheckBox)sender);
        }

        private void chb_7_0_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Long_Sleeve((CheckBox)sender);
        }

        private void chb_7_1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Long_Sleeve((CheckBox)sender);
        }

        private void chb_7_2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Long_Sleeve((CheckBox)sender);
        }

        private void chb_7_3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Long_Sleeve((CheckBox)sender);
        }

        private void chb_7_4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Long_Sleeve((CheckBox)sender);
        }

        private void chb_7_5_CheckedChanged(object sender, EventArgs e)
        {
            Check_Unisex_Long_Sleeve((CheckBox)sender);
        }

        private void chb_8_0_CheckedChanged(object sender, EventArgs e)
        {
            Check_Youth((CheckBox)sender);
        }

        private void chb_8_1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Youth((CheckBox)sender);
        }

        private void chb_8_2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Youth((CheckBox)sender);
        }

        private void chb_8_3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Youth((CheckBox)sender);
        }

        private void chb_8_4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Youth((CheckBox)sender);
        }

        private void chb_8_5_CheckedChanged(object sender, EventArgs e)
        {
            Check_Youth((CheckBox)sender);
        }

        private void chb_8_6_CheckedChanged(object sender, EventArgs e)
        {
            Check_Youth((CheckBox)sender);
        }

        private void CreateBanner_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
