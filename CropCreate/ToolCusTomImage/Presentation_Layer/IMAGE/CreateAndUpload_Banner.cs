using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WooCommerceNET;
using Newtonsoft.Json;
using CropCreate.Common;
using Renci.SshNet;
using System.IO;
using Renci.SshNet.Sftp;
using WooCommerceNET.WooCommerce.v3;
using CropCreate.DTO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Diagnostics;
using MyLib;
using System.Threading;
using Microsoft.VisualBasic;
//using MyLib;

namespace ToolCusTomImage.Presentation_Layer.IMAGE
{
    public partial class CreateAndUpload_Banner : DevExpress.XtraEditors.XtraForm
    {


        private static string wc_key = "";
        private static string ws_Key = "";
        private static string domain = "";

        private static int lengthFile = 0;


        RestAPI rest = new RestAPI(AppSettingConfig.GetAppSetting("LinkWordpress"),
                                      AppSettingConfig.GetAppSetting("woo_ck_key"),
                                      AppSettingConfig.GetAppSetting("woo_cs_key"));
        private string _Folder_Temp = "Data\\Template";

        private string _Folder_Log = "Data\\Log";

        static Template _curTemp = new Template();
        static Manager _manager = new Manager();

        

        Product_Category products = new Product_Category();
        static int _num_thread = 0;
        static int _sleep = 0;
        static string cboDefaultTye = "";



        static object _lock = new object();
        static int _index = 0;

        Attri attribute1 = new Attri();
        Attri attribute2 = new Attri();

        List<ProductTag> tag = new List<ProductTag>();
        const string alphanumericCharacters =
                    "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                    "abcdefghijklmnopqrstuvwxyz" +
                    "0123456789";

        List<List<CheckBox>> _listRunl = new List<List<CheckBox>>();
        #region List<checkbox> Apparel
        private List<List<CheckBox>> _listApparel = new List<List<CheckBox>>();
        private List<CheckBox> _list_UnisexShortSleepClassic_Tee = new List<CheckBox>();
        private List<CheckBox> _list_CanvasRingspun_Tee = new List<CheckBox>();
        private List<CheckBox> _list_UnisexBaseBall_Tee = new List<CheckBox>();
        private List<CheckBox> _list_NextLevelWomens_Crew = new List<CheckBox>();
        private List<CheckBox> _list_NextLevelMen_Crew = new List<CheckBox>();
        private List<CheckBox> _list_BellaWomens_Crew = new List<CheckBox>();
        private List<CheckBox> _list_UnisexLongSleeveClassic_Tee = new List<CheckBox>();
        private List<CheckBox> _list_UnisexLongSleeveBasic_Tee = new List<CheckBox>();
        private List<CheckBox> _list_BellaWomenFlowy_Tank = new List<CheckBox>();
        private List<CheckBox> _list_BellaWomenFitted_Tank = new List<CheckBox>();
        private List<CheckBox> _list_CanvasUnisexRingspun_Tank = new List<CheckBox>();
        private List<CheckBox> _list_CanvasRingspunV_Neck = new List<CheckBox>();
        private List<CheckBox> _list_CanvasPolyCotton_Hoodie = new List<CheckBox>();
        private List<CheckBox> _list_WomensShortSleeveJerseyV_neck = new List<CheckBox>();
        private List<CheckBox> _list_UnisexFleecePullover_Sweatshirt = new List<CheckBox>();
        private List<CheckBox> _list_WomensRelaxedFit_Tee = new List<CheckBox>();
        private List<CheckBox> _list_BellaWideNeck_Sweatshirt = new List<CheckBox>();
        private List<CheckBox> _list_BellaLadiesSlouchy_Tee = new List<CheckBox>();
        private List<CheckBox> _list_UnisexHeavyweightZip_Hoodie = new List<CheckBox>();
        private List<CheckBox> _list_CanvasTriblend_Tee = new List<CheckBox>();
        private List<CheckBox> _list_MensPerformance_Tee = new List<CheckBox>();
        private List<CheckBox> _list_WomenPerformance_Tee = new List<CheckBox>();
        private List<CheckBox> _list_KidClassic_Tee = new List<CheckBox>();
        private List<CheckBox> _list_KidsHeavyweightPullover_Hoodie = new List<CheckBox>();
        private List<CheckBox> _list_RabbitSkinsBaby_Onesie = new List<CheckBox>();
        private List<CheckBox> _list_KidsFleecePullover_Sweatshirt = new List<CheckBox>();


        private List<List<CheckBox>> _listDrinkWare = new List<List<CheckBox>>();
        private List<CheckBox> _list_11ozCM = new List<CheckBox>();
        private List<CheckBox> _list_15ozCM = new List<CheckBox>();



        #region http grealaunce
        //private string UnisexShortSleepClassic_Tee = "https://gearlaunch-product-images.imgix.net/img/product/UnisexCrew_FRONT_46be88da32.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string CanvasRingspun_Tee = "https://gearlaunch-product-images.imgix.net/img/product/UnisexCrew_FRONT_46be88da32.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string UnisexBaseBall_Tee = "https://gearlaunch-product-images.imgix.net/img/product/Bella3200BaseballTee_FRONT_197_82_310_413_980c3c23af.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string NextLevelWomens_Crew = "https://gearlaunch-product-images.imgix.net/img/product/WomensCrew_FRONT_ef1b70af84.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string NextLevelMen_Crew = "https://gearlaunch-product-images.imgix.net/img/product/UnisexCrew_FRONT_46be88da32.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string BellaWomens_Crew = "https://gearlaunch-product-images.imgix.net/img/product/WomensCrew_FRONT_ef1b70af84.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string UnisexLongSleeveClassic_Tee = "https://gearlaunch-product-images.imgix.net/img/product/UnisexLongSleeve_FRONT_e4bc4896e8.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string UnisexLongSleeveBasic_Tee = "https://gearlaunch-product-images.imgix.net/img/product/UnisexLongSleeve_FRONT_e4bc4896e8.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string BellaWomenFlowy_Tank = "https://gearlaunch-product-images.imgix.net/img/product/BellaFlowyTank_FRONT_55d571ed39.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string BellaWomenFitted_Tank = "https://gearlaunch-product-images.imgix.net/img/product/LadiesRibbedTank_FRONT_2617384e21.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string CanvasUnisexRingspun_Tank = "https://gearlaunch-product-images.imgix.net/img/product/UnisexTank_FRONT_6a677605d9.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string CanvasRingspunV_Neck = "https://gearlaunch-product-images.imgix.net/img/product/Male_V-neck_FRONT_28199541e2.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string CanvasPolyCotton_Hoodie = "https://gearlaunch-product-images.imgix.net/img/product/PulloverHoodie_FRONT_722edad976.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string WomensShortSleeveJerseyV_neck = "https://gearlaunch-product-images.imgix.net/img/product/WomansV-neck_FRONT_fedb4daed2.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string UnisexFleecePullover_Sweatshirt = "https://gearlaunch-product-images.imgix.net/img/product/SweatshirtCrew_FRONT_25f28e21df.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string WomensRelaxedFit_Tee = "https://gearlaunch-product-images.imgix.net/img/product/WomensCrew_FRONT_ef1b70af84.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string BellaWideNeck_Sweatshirt = "https://gearlaunch-product-images.imgix.net/img/product/LadiesSlouchyFleece_FRONT_72bc0e9a68.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string BellaLadiesSlouchy_Tee = "https://gearlaunch-product-images.imgix.net/img/product/BellaSlouchyTee_FRONT_b52bd4147e.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string UnisexHeavyweightZip_Hoodie = "https://gearlaunch-product-images.imgix.net/img/product/ZipHoodie-v1-Front_1524c407cb.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string CanvasTriblend_Tee = "https://gearlaunch-product-images.imgix.net/img/product/UnisexCrew_FRONT_46be88da32.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string MensPerformance_Tee = "https://gearlaunch-product-images.imgix.net/img/product/UnisexCoolDri_FRONT_dc2fa3ec82.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string WomenPerformance_Tee = "https://gearlaunch-product-images.imgix.net/img/product/LadiesCoolDri_FRONT_af53ceb105.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string KidClassic_Tee = "https://gearlaunch-product-images.imgix.net/img/product/YouthShirt_FRONT_dfeb1e9d36.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string KidsHeavyweightPullover_Hoodie = "https://gearlaunch-product-images.imgix.net/img/product/KidsHoodie_FRONT_fa578dab94.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string RabbitSkinsBaby_Onesie = "https://gearlaunch-product-images.imgix.net/img/product/Onesie_FRONT_a68c91ec5a.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";

        //private string KidsFleecePullover_Sweatshirt = "https://gearlaunch-product-images.imgix.net/img/product/KidsSweatshirt_FRONT_94c2789ab7.png?bg=FFFFFF&fit=clamp&ixlib=java-1.1.0&mark64=&markw=300&markx=209&marky=98";
        #endregion

        #endregion

        public CreateAndUpload_Banner()
        {
            InitializeComponent();
            _listRunl = _listApparel;
            FillData();
            HideTabPage();
            tabControl1.TabPages.Add(tabApparel);
            var task1 = Task.Factory.StartNew(AddListListCheckBox);
            var task2 = Task.Factory.StartNew(AddListCheckBox);

            Task.WaitAll(task1, task2);

            if (!Directory.Exists(_Folder_Temp)) Directory.CreateDirectory(_Folder_Temp);
            if (!Directory.Exists(_Folder_Log)) Directory.CreateDirectory(_Folder_Log);

            Refresh_List_Temp();
        }

        private void AddListListCheckBox()
        {

            #region Add _listApparel
            _listApparel.Add(_list_UnisexShortSleepClassic_Tee);
            _listApparel.Add(_list_CanvasRingspun_Tee);
            _listApparel.Add(_list_UnisexBaseBall_Tee);
            _listApparel.Add(_list_NextLevelWomens_Crew);
            _listApparel.Add(_list_NextLevelMen_Crew);
            _listApparel.Add(_list_BellaWomens_Crew);
            _listApparel.Add(_list_UnisexLongSleeveClassic_Tee);
            _listApparel.Add(_list_UnisexLongSleeveBasic_Tee);
            _listApparel.Add(_list_BellaWomenFlowy_Tank);
            _listApparel.Add(_list_BellaWomenFitted_Tank);
            _listApparel.Add(_list_CanvasUnisexRingspun_Tank);
            _listApparel.Add(_list_CanvasRingspunV_Neck);
            _listApparel.Add(_list_CanvasPolyCotton_Hoodie);
            _listApparel.Add(_list_WomensShortSleeveJerseyV_neck);
            _listApparel.Add(_list_UnisexFleecePullover_Sweatshirt);
            _listApparel.Add(_list_WomensRelaxedFit_Tee);
            _listApparel.Add(_list_BellaWideNeck_Sweatshirt);
            _listApparel.Add(_list_BellaLadiesSlouchy_Tee);
            _listApparel.Add(_list_UnisexHeavyweightZip_Hoodie);
            _listApparel.Add(_list_CanvasTriblend_Tee);
            _listApparel.Add(_list_MensPerformance_Tee);
            _listApparel.Add(_list_WomenPerformance_Tee);
            _listApparel.Add(_list_KidClassic_Tee);
            _listApparel.Add(_list_KidsHeavyweightPullover_Hoodie);
            _listApparel.Add(_list_RabbitSkinsBaby_Onesie);
            _listApparel.Add(_list_KidsFleecePullover_Sweatshirt);
            #endregion

            #region Add _listDrinkware
            _listDrinkWare.Add(_list_11ozCM);
            _listDrinkWare.Add(_list_15ozCM);
            #endregion


        }




        //private CheckBox additemTextforCheckbox(string chuoiGearleanch, string price, string style)
        //{
        //    CheckBox item = new CheckBox();
        //    item.Text = "{'http':" + "'" + chuoiGearleanch + "'" + "," + "'price" + "':" + price + "," + "'style':" + "'" + style + "'" + "}";
        //    item.Checked = true;
        //    return item;
        //}

        public void AddListCheckBox()
        {

            #region trash
            //list_UnisexShortSleepClassic_Tee. 1
            //_list_UnisexShortSleepClassic_Tee.Add(additemTextforCheckbox(UnisexShortSleepClassic_Tee, txtPrice_USSCT.Text.ToString(), groupBox_USSCT.Text));
            //_list_CanvasRingspun_Tee.Add(additemTextforCheckbox(CanvasRingspun_Tee, txtPrice_CRT.Text.ToString(), groupBox_CRVN.Text));
            //_list_UnisexBaseBall_Tee.Add(additemTextforCheckbox(UnisexBaseBall_Tee, txtPrice_U3BT.Text.Trim(), groupBox_U3BT.Text));
            //_list_NextLevelWomens_Crew.Add(additemTextforCheckbox(NextLevelWomens_Crew, txtPrice_NLWC.Text.Trim(), groupBox_NLWC.Text));
            //_list_NextLevelMen_Crew.Add(additemTextforCheckbox(NextLevelMen_Crew, txtPrice_NLMC.Text.Trim(), groupBox_NLMC.Text));
            //_list_BellaWomens_Crew.Add(additemTextforCheckbox(BellaWomens_Crew, txtPrice_BWC.Text.Trim(), groupBox_BWC.Text));
            //_list_UnisexLongSleeveClassic_Tee.Add(additemTextforCheckbox(UnisexLongSleeveClassic_Tee, txtPrice_ULSCT.Text.Trim(), groupBox_ULSCT.Text));
            //_list_UnisexLongSleeveBasic_Tee.Add(additemTextforCheckbox(UnisexLongSleeveBasic_Tee, txtPrice_ULSBT.Text.Trim(), groupBox_ULSBT.Text));
            //_list_BellaWomenFlowy_Tank.Add(additemTextforCheckbox(BellaWomenFlowy_Tank, txtPrice_BWFT.Text.Trim(), groupBox_BWFT.Text));
            //_list_BellaWomenFitted_Tank.Add(additemTextforCheckbox(BellaWomenFitted_Tank, txtPrice_BWFiT.Text.Trim(), groupBox_BWFiT.Text));
            //_list_CanvasUnisexRingspun_Tank.Add(additemTextforCheckbox(CanvasUnisexRingspun_Tank, txtPrice_CURT.Text.Trim(), groupBox_CURT.Text));
            //_list_CanvasRingspunV_Neck.Add(additemTextforCheckbox(CanvasRingspunV_Neck, txtPrice_CRVN.Text.Trim(), groupBox_CRVN.Text));
            //_list_CanvasPolyCotton_Hoodie.Add(additemTextforCheckbox(CanvasPolyCotton_Hoodie, txtPrice_CPCH.Text.Trim(), groupBox_CPCH.Text));
            //_list_WomensShortSleeveJerseyV_neck.Add(additemTextforCheckbox(WomensShortSleeveJerseyV_neck, txtPrice_WSSJVN.Text.Trim(), groupBox_WSSJVN.Text));
            //_list_UnisexFleecePullover_Sweatshirt.Add(additemTextforCheckbox(UnisexFleecePullover_Sweatshirt, txtPrice_UFPS.Text.Trim(), groupBox_UFPS.Text));
            //_list_WomensRelaxedFit_Tee.Add(additemTextforCheckbox(WomensRelaxedFit_Tee, txtPrice_WRFT.Text.Trim(), groupBox_WRFT.Text));
            //_list_BellaWideNeck_Sweatshirt.Add(additemTextforCheckbox(BellaWideNeck_Sweatshirt, txtPrice_BWNS.Text.Trim(), groupBox_BWNS.Text));
            //_list_BellaLadiesSlouchy_Tee.Add(additemTextforCheckbox(BellaLadiesSlouchy_Tee, txtPrice_BLST.Text.Trim(), groupBox_BLST.Text));
            //_list_UnisexHeavyweightZip_Hoodie.Add(additemTextforCheckbox(UnisexHeavyweightZip_Hoodie, txtPrice_UHZH.Text.Trim(), groupBox_UHZH.Text));
            //_list_CanvasTriblend_Tee.Add(additemTextforCheckbox(CanvasTriblend_Tee, txtPrice_CTT.Text.Trim(), groupBox_CTT.Text));
            //_list_MensPerformance_Tee.Add(additemTextforCheckbox(MensPerformance_Tee, txtPrice_MPT.Text.Trim(), groupBox_MPT.Text));
            //_list_WomenPerformance_Tee.Add(additemTextforCheckbox(WomenPerformance_Tee, txtPrice_WPT.Text.Trim(), groupBox_WPT.Text));
            //_list_KidClassic_Tee.Add(additemTextforCheckbox(KidClassic_Tee, txtPrice_KCT.Text.Trim(), groupBox_KCT.Text));
            //_list_KidsHeavyweightPullover_Hoodie.Add(additemTextforCheckbox(KidsHeavyweightPullover_Hoodie, txtPrice_KHPH.Text.Trim(), groupBox_KHPH.Text));
            //_list_RabbitSkinsBaby_Onesie.Add(additemTextforCheckbox(RabbitSkinsBaby_Onesie, txtPrice_RSBO.Text.Trim(), groupBox_RSBO.Text));
            //_list_KidsFleecePullover_Sweatshirt.Add(additemTextforCheckbox(KidsFleecePullover_Sweatshirt, txtPrice_KFPS.Text.Trim(), groupBox_KFPS.Text));
            #endregion

            #region _listApparel
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT1);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT2);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT3);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT4);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT5);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT6);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT7);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT8);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT9);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT10);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT11);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT12);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT13);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT14);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT15);
            _list_UnisexShortSleepClassic_Tee.Add(chb_ap_USSCT16);




            _list_CanvasRingspun_Tee.Add(chb_ap_CRT1);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT2);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT3);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT4);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT5);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT6);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT7);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT8);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT9);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT10);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT11);
            _list_CanvasRingspun_Tee.Add(chb_ap_CRT12);


            // _list_UnisexBaseBall_Tee . 3

            _list_UnisexBaseBall_Tee.Add(chb_ap_U3BT1);
            _list_UnisexBaseBall_Tee.Add(chb_ap_U3BT2);
            _list_UnisexBaseBall_Tee.Add(chb_ap_U3BT3);

            // _list_NextLevelWomens_Crew. 4

            _list_NextLevelWomens_Crew.Add(chb_ap_NLWC1);
            _list_NextLevelWomens_Crew.Add(chb_ap_NLWC2);
            _list_NextLevelWomens_Crew.Add(chb_ap_NLWC3);
            _list_NextLevelWomens_Crew.Add(chb_ap_NLWC4);
            _list_NextLevelWomens_Crew.Add(chb_ap_NLWC5);
            _list_NextLevelWomens_Crew.Add(chb_ap_NLWC6);
            _list_NextLevelWomens_Crew.Add(chb_ap_NLWC7);


            // __list_NextLevelMen_Crew. 5

            _list_NextLevelMen_Crew.Add(chb_ap_NLMC1);
            _list_NextLevelMen_Crew.Add(chb_ap_NLMC2);
            _list_NextLevelMen_Crew.Add(chb_ap_NLMC3);
            _list_NextLevelMen_Crew.Add(chb_ap_NLMC4);
            _list_NextLevelMen_Crew.Add(chb_ap_NLMC5);
            _list_NextLevelMen_Crew.Add(chb_ap_NLMC6);
            _list_NextLevelMen_Crew.Add(chb_ap_NLMC7);

            //_list_BellaWomens_Crew 26

            _list_BellaWomens_Crew.Add(chb_ap_BWC1);
            _list_BellaWomens_Crew.Add(chb_ap_BWC2);
            _list_BellaWomens_Crew.Add(chb_ap_BWC3);
            _list_BellaWomens_Crew.Add(chb_ap_BWC4);
            _list_BellaWomens_Crew.Add(chb_ap_BWC5);
            _list_BellaWomens_Crew.Add(chb_ap_BWC6);
            _list_BellaWomens_Crew.Add(chb_ap_BWC7);
            _list_BellaWomens_Crew.Add(chb_ap_BWC8);


            //_list_UnisexLongSleeveClassic_Tee. 6

            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT1);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT2);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT3);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT4);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT5);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT6);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT7);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT8);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT9);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT10);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT11);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT12);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT13);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT14);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT15);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT16);
            _list_UnisexLongSleeveClassic_Tee.Add(chb_ap_ULSCT17);

            //_list_UnisexLongSleeveBasic_Tee. 7

            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT1);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT2);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT3);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT4);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT5);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT6);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT7);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT8);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT9);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT10);
            _list_UnisexLongSleeveBasic_Tee.Add(chb_ap_ULSBT11);


            //_list_BellaWomenFlowy_Tank. 8

            _list_BellaWomenFlowy_Tank.Add(chb_ap_BWFT1);
            _list_BellaWomenFlowy_Tank.Add(chb_ap_BWFT2);
            _list_BellaWomenFlowy_Tank.Add(chb_ap_BWFT3);
            _list_BellaWomenFlowy_Tank.Add(chb_ap_BWFT4);
            _list_BellaWomenFlowy_Tank.Add(chb_ap_BWFT5);
            _list_BellaWomenFlowy_Tank.Add(chb_ap_BWFT6);
            _list_BellaWomenFlowy_Tank.Add(chb_ap_BWFT7);
            _list_BellaWomenFlowy_Tank.Add(chb_ap_BWFT8);
            _list_BellaWomenFlowy_Tank.Add(chb_ap_BWFT9);

            //_list_BellaWomenFitted_Tank. 9

            _list_BellaWomenFitted_Tank.Add(chb_ap_BWFiT1);
            _list_BellaWomenFitted_Tank.Add(chb_ap_BWFiT2);
            _list_BellaWomenFitted_Tank.Add(chb_ap_BWFiT3);


            // _list_CanvasUnisexRingspun_Tank . 10

            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT1);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT2);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT3);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT4);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT5);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT6);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT7);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT8);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT9);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT10);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT11);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT12);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT13);
            _list_CanvasUnisexRingspun_Tank.Add(chb_ap_CURT14);

            // _list_CanvasRingspunV_Neck . 11

            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN1);
            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN2);
            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN3);
            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN4);
            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN5);
            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN6);
            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN7);
            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN8);
            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN9);
            _list_CanvasRingspunV_Neck.Add(chb_ap_CRVN10);


            // _list_CanvasPolyCotton_Hoodie . 12

            _list_CanvasPolyCotton_Hoodie.Add(chb_ap_CPCH1);
            _list_CanvasPolyCotton_Hoodie.Add(chb_ap_CPCH2);
            _list_CanvasPolyCotton_Hoodie.Add(chb_ap_CPCH3);
            _list_CanvasPolyCotton_Hoodie.Add(chb_ap_CPCH4);
            _list_CanvasPolyCotton_Hoodie.Add(chb_ap_CPCH5);
            _list_CanvasPolyCotton_Hoodie.Add(chb_ap_CPCH6);


            // _list_WomensShortSleeveJerseyV_neck . 13

            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN1);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN2);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN3);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN4);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN5);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN6);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN7);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN8);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN9);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN10);
            _list_WomensShortSleeveJerseyV_neck.Add(chb_ap_WSSJVN11);

            // _list_UnisexFleecePullover_Sweatshirt  . 14

            _list_UnisexFleecePullover_Sweatshirt.Add(chb_ap_UFPS1);
            _list_UnisexFleecePullover_Sweatshirt.Add(chb_ap_UFPS2);
            _list_UnisexFleecePullover_Sweatshirt.Add(chb_ap_UFPS3);
            _list_UnisexFleecePullover_Sweatshirt.Add(chb_ap_UFPS4);
            _list_UnisexFleecePullover_Sweatshirt.Add(chb_ap_UFPS5);
            _list_UnisexFleecePullover_Sweatshirt.Add(chb_ap_UFPS6);
            _list_UnisexFleecePullover_Sweatshirt.Add(chb_ap_UFPS7);
            _list_UnisexFleecePullover_Sweatshirt.Add(chb_ap_UFPS8);


            // _list_WomensRelaxedFit_Tee . 15

            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT1);
            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT2);
            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT3);
            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT4);
            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT5);
            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT6);
            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT7);
            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT8);
            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT9);
            _list_WomensRelaxedFit_Tee.Add(chb_ap_WRFT10);


            // _list_BellaWideNeck_Sweatshirt . 16

            _list_BellaWideNeck_Sweatshirt.Add(chb_ap_BWNS1);
            _list_BellaWideNeck_Sweatshirt.Add(chb_ap_BWNS2);
            _list_BellaWideNeck_Sweatshirt.Add(chb_ap_BWNS3);
            _list_BellaWideNeck_Sweatshirt.Add(chb_ap_BWNS4);
            _list_BellaWideNeck_Sweatshirt.Add(chb_ap_BWNS5);
            _list_BellaWideNeck_Sweatshirt.Add(chb_ap_BWNS6);


            // _list_BellaLadiesSlouchy_Tee . 17

            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST1);
            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST2);
            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST3);
            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST4);
            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST5);
            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST6);
            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST7);
            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST8);
            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST9);
            _list_BellaLadiesSlouchy_Tee.Add(chb_ap_BLST10);

            // _list_UnisexHeavyweightZip_Hoodie  . 18

            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH1);
            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH2);
            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH3);
            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH4);
            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH5);
            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH6);
            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH7);
            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH8);
            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH9);
            _list_UnisexHeavyweightZip_Hoodie.Add(chb_ap_UHZH10);

            // _list_CanvasTriblend_Tee . 19

            _list_CanvasTriblend_Tee.Add(chb_ap_CTT1);
            _list_CanvasTriblend_Tee.Add(chb_ap_CTT2);
            _list_CanvasTriblend_Tee.Add(chb_ap_CTT3);
            _list_CanvasTriblend_Tee.Add(chb_ap_CTT4);
            _list_CanvasTriblend_Tee.Add(chb_ap_CTT5);
            _list_CanvasTriblend_Tee.Add(chb_ap_CTT6);

            // _list_MensPerformance_Tee . 20 

            _list_MensPerformance_Tee.Add(chb_ap_MPT1);
            _list_MensPerformance_Tee.Add(chb_ap_MPT2);
            _list_MensPerformance_Tee.Add(chb_ap_MPT3);
            _list_MensPerformance_Tee.Add(chb_ap_MPT4);
            _list_MensPerformance_Tee.Add(chb_ap_MPT5);
            _list_MensPerformance_Tee.Add(chb_ap_MPT6);

            // _list_WomenPerformance_Tee  . 21

            _list_WomenPerformance_Tee.Add(chb_ap_WPT1);
            _list_WomenPerformance_Tee.Add(chb_ap_WPT2);
            _list_WomenPerformance_Tee.Add(chb_ap_WPT3);
            _list_WomenPerformance_Tee.Add(chb_ap_WPT4);

            // _list_KidClassic_Tee . 22

            _list_KidClassic_Tee.Add(chb_ap_KCT1);
            _list_KidClassic_Tee.Add(chb_ap_KCT2);
            _list_KidClassic_Tee.Add(chb_ap_KCT3);
            _list_KidClassic_Tee.Add(chb_ap_KCT4);
            _list_KidClassic_Tee.Add(chb_ap_KCT5);
            _list_KidClassic_Tee.Add(chb_ap_KCT6);
            _list_KidClassic_Tee.Add(chb_ap_KCT7);
            _list_KidClassic_Tee.Add(chb_ap_KCT8);
            _list_KidClassic_Tee.Add(chb_ap_KCT9);
            _list_KidClassic_Tee.Add(chb_ap_KCT10);
            _list_KidClassic_Tee.Add(chb_ap_KCT11);

            // _list_KidsHeavyweightPullover_Hoodie . 23

            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH1);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH2);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH3);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH4);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH5);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH6);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH7);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH8);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH9);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH10);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH11);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH12);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH13);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH14);
            _list_KidsHeavyweightPullover_Hoodie.Add(chb_ap_KHPH15);

            // _list_RabbitSkinsBaby_Onesie  . 24

            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO1);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO2);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO3);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO4);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO5);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO6);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO7);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO8);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO9);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO10);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO11);
            _list_RabbitSkinsBaby_Onesie.Add(chb_ap_RSBO12);

            // _list_KidsFleecePullover_Sweatshirt . 25 

            _list_KidsFleecePullover_Sweatshirt.Add(chb_ap_KFPS1);
            _list_KidsFleecePullover_Sweatshirt.Add(chb_ap_KFPS2);
            _list_KidsFleecePullover_Sweatshirt.Add(chb_ap_KFPS3);
            _list_KidsFleecePullover_Sweatshirt.Add(chb_ap_KFPS4);
            _list_KidsFleecePullover_Sweatshirt.Add(chb_ap_KFPS5);
            _list_KidsFleecePullover_Sweatshirt.Add(chb_ap_KFPS6);
            _list_KidsFleecePullover_Sweatshirt.Add(chb_ap_KFPS7);
            _list_KidsFleecePullover_Sweatshirt.Add(chb_ap_KFPS8);
            _list_KidsFleecePullover_Sweatshirt.Add(chb_ap_KFPS9);
            #endregion

            #region _listDrinkWare
            _list_11ozCM.Add(chb_dw_11ozCM1);
            _list_11ozCM.Add(chb_dw_11ozCM2);

            _list_15ozCM.Add(chb_dw_15ozCM1);
            _list_15ozCM.Add(chb_dw_15ozCM2);
            #endregion
        }

        #region ReadCurrentTemplate
        private Template ReadCurrentTemplate()
        {
            Template temp = new Template();
            //read infor
            temp.Title = txtTitle.Text;
            temp.Description = txtDesc.Text;
        

            temp.Keywords = txtKeywords.Text;
            //read def
            if (cobDefType.Text != "")
                temp.DefType = cobDefType.Text.ToString();
            if (cobDefColor.Text != "")
                temp.DefColors = cobDefColor.Text;
            if (cobDefCategory.Text != "")
                temp.Category = (cobDefCategory.SelectedItem as dynamic).Value;
            if (tboxServer.Text != "")
                temp.Server = tboxServer.Text.Trim();
            if (tboxDirectUpload.Text != "")
                temp.DirectoryGetImage = tboxDirectUpload.Text.Trim();
            if (tboxUserName.Text != "")
                temp.username = tboxUserName.Text.Trim();
            if (tboxwc_Key.Text != "")
                temp.wc_key = tboxwc_Key.Text.Trim();
            if (tboxws_Key.Text != "")
                temp.ws_key = tboxws_Key.Text.Trim();
            if (tboxDomain.Text != "")
                temp.Domain = tboxDomain.Text.Trim();
            if (txtSleep.Text.Trim() != "")
                temp.Sleep = Convert.ToDouble(txtSleep.Text.Trim());
            if (txtSleep.Text.Trim() != "")
                temp.Thread = Convert.ToInt32(txtThread.Text.Trim());



            string gearlaunchDefault = "";
            string priceDefault = "";
            string styleDefault = "";

            List<string> optionAttriBute = new List<string>();
            List<string> optionAttriButeStyle = new List<string>();
            List<WooCommerceNET.WooCommerce.v3.ProductAttributeLine> attri = new List<WooCommerceNET.WooCommerce.v3.ProductAttributeLine>();
            List<WooCommerceNET.WooCommerce.v3.ProductTagLine> tags = new List<WooCommerceNET.WooCommerce.v3.ProductTagLine>();
            var _product = new WooCommerceNET.WooCommerce.v3.Product();
            List<Variation> vari = new List<Variation>();
            List<WooCommerceNET.WooCommerce.v3.ProductImage> productImageList = new List<WooCommerceNET.WooCommerce.v3.ProductImage>();
            //byte[] bytes = Encoding.UTF8.GetBytes(fileList[].LinkImage);
            //arrayBase64 = Convert.ToBase64String(bytes);s
            #region run LIST CHECKBOX
            for (int check = 0; check < _listRunl.Count; check++) // 
            {

                Random random = new Random();

                var listcheck = _listRunl[check].FindAll(p => p.Checked == true);


                try
                {

                    #region list Normal
                    if (listcheck.Any() && listcheck.Count > 0)
                    {
                        string gearlaunch = "";
                        string price = "";
                        string style = "";
                        bool flag = false;
                        List<CheckBox> _checked = new List<CheckBox>();

                        flag = false;
                        Control chilldControl = listcheck[0].Parent;
                        if (chilldControl.Controls.Contains(listcheck[0]))
                        {
                            gearlaunch = chilldControl.Tag.ToString();
                            style = chilldControl.Text.ToString();
                            foreach (Control c in chilldControl.Controls)
                            {
                                if (c is TextBox)
                                {
                                    price = c.Text;

                                }
                                if (c is CheckBox && ((CheckBox)c).Checked == true)
                                {
                                    _checked.Add((CheckBox)c);
                                }
                            }
                            flag = true;
                        }

                        if (flag)
                        {
                            UploadTypes type = new UploadTypes();
                            type.id = check;
                            type.name = style;
                            type.price = double.Parse(price);
                            for (int count = 0; count < _checked.Count; count++)
                            {
                                //temp.optionAttriBute.Add(_checked[count].Tag.ToString());
                                type.colors.Add(_checked[count].BackColor.Name);
                            }
                            temp.Types.Add(type);
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
            #endregion

            return temp;
        }
        #endregion


        private void CreateAndUpload_Banner_Load(object sender, EventArgs e)
        {
            label12.Text = "";
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("per_page", "100");
            //int pageNumber = 1;
            //dic.Add("page", pageNumber.ToString());

        }


        private async void FillData()
        {
            //LoadCategories();
        }


        private async void LoadCategories()
        {

            wc_key = tboxwc_Key.Text.Trim().ToString();
            ws_Key = tboxws_Key.Text.Trim().ToString();
            domain = tboxDomain.Text.Trim().ToString();
            domain += "/wp-json/wc/v3/";



            if (!String.IsNullOrEmpty(wc_key) && !String.IsNullOrEmpty(ws_Key) && !String.IsNullOrEmpty(domain))
            {
                rest = new RestAPI(domain, wc_key, ws_Key);
            }

            try
            {
                //RestAPI rest = new RestAPI("http://mycavnus.com/wc-api/v3/", "ck_xxx", "cs_xxx");
                WooCommerceNET.WooCommerce.v3.WCObject wc = new WooCommerceNET.WooCommerce.v3.WCObject(rest);

                DateTime datefrom = new DateTime();
                DateTime dateto = new DateTime();

                TimeSpan aInterval = new System.TimeSpan(1, 1, 0, 20);
                dateto = DateTime.Now;
                datefrom = DateTime.Now.Subtract(aInterval);

                var products_ = wc.Product.GetAll();
                //var categories = await wc.GetProductCategories();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("per_page", "100");
                int pageNumber = 1;
                dic.Add("page", pageNumber.ToString());

                var categories = await wc.Category.GetAll(dic);

                cobDefCategory.DisplayMember = "Text";
                cobDefCategory.ValueMember = "Value";

                if (categories.Any())
                {
                    foreach (var item in categories)
                    {
                        products = new Product_Category()
                        {
                            id = item.id,
                            name = item.name,
                            slug = item.slug,
                            parent = item.parent,
                            display = item.display,
                            description = item.description,
                            count = item.count
                        };
                        cobDefCategory.Items.Add(new { Text = item.name, Value = item.id });
                    }
                    cobDefCategory.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                cobDefCategory.Items.Clear();
                MessageBox.Show("sai key");
            }

        }


        #region Upload Folder Image to Server


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


        //public static void UploadSFTPFile(string host, string username, string password, string sourcefile, string destinationpath, int port)
        //{
        //    using (SftpClient client = new SftpClient(host, port, username, password))
        //    {
        //        client.Connect();
        //        client.ChangeDirectory(destinationpath);
        //        using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
        //        {
        //            client.BufferSize = 4 * 1024;
        //            client.UploadFile(fs, Path.GetFileName(sourcefile));
        //        }
        //    }
        //}



        private void btnChose_EditValueChanged(object sender, EventArgs e)
        {

        }


        private void SuccessFile(int tongfile)
        {
            try
            {
                label12.Invoke((Action)(() => label12.Text = demFileSuccess.ToString() + " / " + tongfile));
            }
            catch (Exception ex)
            {
                //WriteLog(ex.ToString());
            }

        }

        private void ErrorFile()
        {
            label11.Invoke((Action)(() => label11.Text = "Error : " + demFileError.ToString()));
        }

        private void WriteLogToTxt(string filepath, string mess)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write))
                using (StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8))
                {
                    sWriter.WriteLine(mess);
                    sWriter.Flush();
                }
            }
            catch (Exception ex)
            {
            }



        }

        private void WriteLog(string mess)
        {

            if (!richTextBox_thongtin.IsHandleCreated)
            {
                richTextBox_thongtin.CreateControl();
            }

            richTextBox_thongtin.Invoke(new MethodInvoker(delegate ()
            {
                richTextBox_thongtin.AppendText(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n");
            }));
            //richTextBox_thongtin.Invoke((Action)(() => richTextBox_thongtin.AppendText(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n")));
            //tbox_thongtin.AppendText(DateTime.Now.ToShortTimeString() + " => " + mess + "\r\n");
        }

        public static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", "length");
            if (characterSet == null)
                throw new ArgumentNullException("characterSet");
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            var result = new char[length];
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                cryptoProvider.GetBytes(bytes);
            }
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }

        private void before_runProgressbarloading(System.Windows.Forms.ProgressBar progress)
        {
            progress.Value = 0;
        }

        private void SetProgressStatus(long cur, long max, string file)
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

        #endregion

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }


        List<MyLib.RemoteFileInfo> fileList = new List<MyLib.RemoteFileInfo>();
        MyLib.RemoteFileInfo SingleFile = new MyLib.RemoteFileInfo();
        ProductTag tagUpload = new ProductTag();


        private void SetProgressStatus(long cur, long max)
        {
            Action del = () =>
            {
                progressBar1.Maximum = 100;
                progressBar1.Value = (int)((cur * 100) / (max));
                if (cur == max)
                {
                    progressBar1.Maximum = 100;
                    progressBar1.Value = 100;

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



        private void Thread_uploadAndPush(object obj)
        {
            int _tmp_index;
            int STT = (int)obj;
            WCObject wc = new WCObject(rest);
            while (true)
            {
                bool done = false;
                lock (_lock)
                {
                    _index += 1;
                    _tmp_index = _index;
                }
                string filePNG = "";

                if (_tmp_index >= Files.Count())
                {
                    done = true;
                    return;
                }
                else
                    filePNG = Files[_tmp_index].FullName;

                if (done)
                    break;
                if (string.IsNullOrWhiteSpace(filePNG))
                    continue;


                WriteLog("Thread " + STT + ": start upload " + Path.GetFileNameWithoutExtension(filePNG));

                using (Stream stream = File.OpenRead(filePNG))
                {

                    IAsyncResult UploadResult = client.BeginUploadFile(stream, Files[_tmp_index].Name);
                    SftpUploadAsyncResult UploadProgress = new SftpUploadAsyncResult(null, UploadResult.AsyncState);
                    while (!UploadProgress.IsCompleted)
                    {
                        UploadProgress = new SftpUploadAsyncResult(null, UploadResult.AsyncState);
                        Thread.Sleep(1000);
                    }
                    UploadProgress.AsyncWaitHandle.WaitOne();


                    //this.CreateFileInfo(FileShare[_tmp_index],client)
                    string remoth = newDirectory;
                    ListFiles(remoth, Files[_tmp_index]);
                }


            }
        }
        private void UploadAndPushFile()
        {

            if (btnChose.Text.Trim() == "")
            {
                MessageBox.Show("Chọn thư mục upload");
                btnChose.Focus();
                return;
            }

            if (Files != null && Files.Count() > 1)
            {
                before_runProgressbarloading(progressBar1);
            }
            else { CheckEnableTrue(); return; }

            try
            {
                string folderName = btnChose.Text.Trim().ToString();
                string pathString = System.IO.Path.Combine(folderName, "Upload_Done");
                if (!System.IO.Directory.Exists(pathString)) System.IO.Directory.CreateDirectory(pathString);
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
                var tagUpload = new ProductTag();

                Thread tManager = new Thread(() =>
                {
                    //khoi tao thread
                    _index = -1;
                    List<Thread> threads = new List<Thread>();
                    for (int i = 0; i < _num_thread; i++)
                    {


                        Thread t = new Thread(Thread_uploadAndPush);
                        threads.Add(t);
                        t.Start(i);
                        Thread.Sleep(_sleep);
                    }
                    while (true)
                    {
                        bool done = true;
                        for (int i = 0; i < threads.Count; i++)
                        {
                            if (threads[i].IsAlive)
                            {
                                done = false;
                                break;
                            }
                        }
                        if (done)
                        {
                            break;
                        }

                        Thread.Sleep(1000);
                    }

                });
                tManager.Start();
            }

            catch
            {

            }


        }




        private void btnStart_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(tboxServer.Text.Trim()) ||
                String.IsNullOrEmpty(tboxUserName.Text.Trim()) ||
                String.IsNullOrEmpty(tboxPassword.Text.Trim()))
            {
                MessageBox.Show("Nhập đầy đủ thông tin trước khi chạy!!");
                return;
            }
            
            if (cbExternal.Checked)
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


                    while (true)
                    {
                        bool done = true;
                        for (int i = 0; i < threads.Count; i++)
                        {
                            if (threads[i].IsAlive)
                            {
                                done = false;
                                break;
                            }
                        }
                        if (done)
                            break;
                        Thread.Sleep(1000);
                    }
                    btnStart.Invoke((Action)(() => btnStart.Enabled = true));
                    MessageBox.Show("Hoan thanh!");
                });
            }
            else
            {

                bool checkfirst = DeclareFirstBeforeStart();
                if (!checkfirst) return;
                if (cboxUploadAndPush.Checked) UploadAndPushFile();
                else OnlyPushFile();
            }
        }



        private async Task GetAllTag()
        {
            WCObject wc = new WCObject(rest);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("per_page", "100");
            int pageNumber = 1;
            dic.Add("page", pageNumber.ToString());

            bool endWhile = false;
            while (!endWhile)
            {
                var tempTag = await wc.Tag.GetAll(dic).ConfigureAwait(true);
                if (tempTag.Count > 0)
                {
                    tag.AddRange(tempTag);
                    pageNumber++;
                    dic["page"] = pageNumber.ToString();
                }
                else
                {
                    endWhile = true;
                }
            }
        }

        private void OnlyPushFile()
        {


            try
            {
                CheckAttriBute();
                string folderName = btnChose.Text.Trim().ToString();
                string pathString = System.IO.Path.Combine(folderName, "Upload_Done");
                if (!System.IO.Directory.Exists(pathString)) System.IO.Directory.CreateDirectory(pathString);
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
                var tagUpload = new ProductTag();
                
                var that = this;
                Thread tManager = new Thread(() =>
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    Task mainTask = GetAllTag();
                    mainTask.Wait();   //wait for start to complete --- how should I do it??

                    sw.Stop();
                    Console.WriteLine(sw.Elapsed);

                    
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
                    while (true)
                    {
                        bool done = true;
                        for (int i = 0; i < threads.Count; i++)
                        {
                            if (threads[i].IsAlive)
                            {
                                done = false;
                                break;
                            }
                        }
                        if (done)
                        {
                                break;
                        }
                        Thread.Sleep(1000);
                    }


                    if (!btnStart.IsHandleCreated)
                    {
                        btnStart.CreateControl();
                    }

                    btnStart.Invoke(new MethodInvoker(delegate ()
                    {
                        btnStart.Enabled = true;
                    }));

                });
                tManager.Start();
            }
            catch
            {
                return;
            }
        }

        private async void Thread_upload(object obj)
        {
            
            int _tmp_index;
            int STT = (int)obj;
            WCObject wc = new WCObject(rest);
            while (true)
            {
                bool done = false;
                lock (_lock)
                {
                    _index += 1;
                    _tmp_index = _index;
                }
                string filePNG = "";

                if (_tmp_index >= fileList.Count())
                {
                    done = true;
                    
                }
                else
                    filePNG = fileList[_tmp_index].FullName;

                if (done)
                {
                    MessageBox.Show(" Done !!!");
                    CheckEnableTrueForSuccess();
                    return;
                }
                   
                if (string.IsNullOrWhiteSpace(filePNG))
                    continue;

                WriteLog("Thread " + STT + ": start upload " + Path.GetFileNameWithoutExtension(filePNG));
                string arrayBase64 = "";

                string folderName = btnChose.Text.Trim().ToString();
                string pathString = System.IO.Path.Combine(folderName, "Upload_Done");
                if (!System.IO.Directory.Exists(pathString)) System.IO.Directory.CreateDirectory(pathString);
                

                List<ProductCategoryLine> category = new List<ProductCategoryLine>();

                category.Add(new WooCommerceNET.WooCommerce.v3.ProductCategoryLine()
                {
                    id = _curTemp.Category
                });

                WooCommerceNET.WooCommerce.v3.Variation variations = new Variation();
                

                string da = DateTime.Now.ToString("dd-MM-yyyy");

                //string filePath = System.IO.Directory.GetCurrentDirectory() + "\\";

                //if (!Directory.Exists(_Folder_Temp)) Directory.CreateDirectory(_Folder_Temp);


                string extension = _Folder_Log + "\\" + "Logdata_" + da + ".txt";
                if (!File.Exists(extension))
                {
                    FileStream fs = new FileStream(extension, FileMode.Create);//Tạo file mới tên là test.txt            
                }

                UploadData(_tmp_index, wc, arrayBase64, category, extension);

                int groupID = 0;




                string keyword = _curTemp.Keywords;

                wc = new WCObject(rest);
                #region Convert Keywords
                string[] strArrayOne = new string[] { "" };



                if (keyword.Contains("{1}"))
                {
                    tagUpload = new ProductTag();
                    keyword = keyword.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));

                    if (keyword.Contains(",")) strArrayOne = keyword.Split(',');
                    else strArrayOne[0] = keyword;
                    try
                    {
                        tagUpload = await wc.Tag.Add(new ProductTag()
                        {
                            name = keyword,
                            slug = keyword,
                            description = "",
                            count = strArrayOne.Count()
                        });
                    }
                    catch
                    {
                        var _tag = tag.Find(x => x.name == keyword.ToString());
                        tagUpload.id = _tag.id;
                        tagUpload.name = keyword;
                        tagUpload.slug = keyword;
                        tagUpload.description = "";
                        tagUpload.count = strArrayOne.Count();
                    }
                }
                else
                {
                    if (_tmp_index == 0)
                    {
                        if (keyword.Contains("{1}"))
                        {
                            keyword = keyword.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));
                        }
                        if (keyword.Contains(",")) strArrayOne = keyword.Split(',');
                        else strArrayOne[0] = keyword;

                        keyword = keyword.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));
                        try
                        {
                            tagUpload = await wc.Tag.Add(new ProductTag()
                            {
                                name = keyword,
                                slug = keyword,
                                description = "",
                                count = strArrayOne.Count()
                            });
                        }
                        catch
                        {

                            var _tag = tag.Find(x => x.name == keyword.ToString());
                            tagUpload.id = _tag.id;
                            tagUpload.name = keyword;
                            tagUpload.slug = keyword;
                            tagUpload.description = "";
                            tagUpload.count = strArrayOne.Count();
                        }
                    }
                }
                #endregion
                Random rd = new Random();

                WooCommerceNET.WooCommerce.v3.Product product = new WooCommerceNET.WooCommerce.v3.Product();


                groupID = rd.Next(minValue: 100000001, maxValue: 999999999);

                string id_group = rd.Next(minValue: 1001, maxValue: 99999).ToString() + "_" + rd.Next(minValue: 100001, maxValue: 999999).ToString();
                string tit = _curTemp.Title.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));
                keyword = keyword.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));



                string description = "";
                txtDesc.Invoke((MethodInvoker)delegate {
                    for (int line = 1; line < txtDesc.Lines.Count(); line++)
                    {
                        description += txtDesc.Lines[line] + " "; 
                    }

                });

                description = description.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));
                string log = id_group.ToString() + "," + '"' + tit + '"' + "," + '"' + keyword + '"' + ",," + '"' + description + '"';

                List<string> optionAttriBute = new List<string>();
                List<string> optionAttriButeStyle = new List<string>();
                List<WooCommerceNET.WooCommerce.v3.ProductAttributeLine> attri = new List<WooCommerceNET.WooCommerce.v3.ProductAttributeLine>();
                List<WooCommerceNET.WooCommerce.v3.ProductTagLine> tags = new List<WooCommerceNET.WooCommerce.v3.ProductTagLine>();
                var _product = new WooCommerceNET.WooCommerce.v3.Product();
                List<Variation> vari = new List<Variation>();
                string gearlaunchDefault = "";
                string priceDefault = "";
                string styleDefault = "";
                List<WooCommerceNET.WooCommerce.v3.ProductImage> productImageList = new List<WooCommerceNET.WooCommerce.v3.ProductImage>();
                byte[] bytes = Encoding.UTF8.GetBytes(fileList[_tmp_index].LinkImage);
                arrayBase64 = Convert.ToBase64String(bytes);


                #region run LIST CHECKBOX
                for (int check = 0; check < _listRunl.Count; check++) // 
                {

                    Random random = new Random();

                    var listcheck = _listRunl[check].FindAll(p => p.Checked == true && p.Parent.Text != cboDefaultTye);
                    var listcheckDefault = _listRunl[check].FindAll(p => p.Checked == true && p.Parent.Text == cboDefaultTye);


                    try
                    {
                        #region Default 
                        if (listcheckDefault.Count > 0 && listcheckDefault.Any())
                        {
                            bool flagDefault = false;

                            List<CheckBox> _checkedDefault = new List<CheckBox>();
                            Control ctrl = listcheckDefault[0].Parent;
                            if (ctrl.Controls.Contains(listcheckDefault[0]))
                            {
                                gearlaunchDefault = ctrl.Tag.ToString();
                                styleDefault = ctrl.Text.ToString();
                                foreach (Control c in ctrl.Controls)
                                {
                                    if (c is TextBox)
                                    {
                                        priceDefault = c.Text;

                                    }
                                    if (c is CheckBox && ((CheckBox)c).Checked == true)
                                    {
                                        _checkedDefault.Add((CheckBox)c);
                                    }
                                }
                                flagDefault = true;
                            }

                            if (flagDefault)
                            {

                                var _checkExistColor = _checkedDefault.Find(l => l.Tag == _curTemp.DefColors);//check checkbox tồn tại color của combobox cobdefColor
                                var tmpDefault = _checkedDefault[0]; //  
                                if (_checkExistColor != null)
                                {
                                    for (int count = 0; count < _checkedDefault.Count; count++)
                                    {
                                        if (_checkedDefault[count].Tag == _curTemp.DefColors)
                                        {
                                            _checkedDefault[0] = _checkExistColor;
                                            _checkedDefault[count] = tmpDefault;
                                        }
                                    }
                                }

                                Task t1 = Task.Factory.StartNew(() =>
                                {
                                    for (int count = 0; count < _checkedDefault.Count; count++)
                                    {
                                        optionAttriBute.Add(_checkedDefault[count].Tag.ToString());


                                        productImageList.Insert(count, new WooCommerceNET.WooCommerce.v3.ProductImage()
                                        {
                                            src = gearlaunchDefault.Replace("&mark64=", "&mark64=" + arrayBase64).Replace("FFFFFF", _checkedDefault[count].BackColor.Name),
                                            name = _checkedDefault[count].Tag.ToString(),

                                        });
                                    }
                                });


                                VariationImage vrIamge = new VariationImage();
                                Task t2 = Task.Factory.StartNew(() =>
                                {
                                    for (int countImage = 0; countImage < _checkedDefault.Count; countImage++)
                                    {
                                        Variation _variations = new Variation();

                                        vrIamge = new VariationImage
                                        {
                                            src = gearlaunchDefault.Replace("&mark64=", "&mark64=" + arrayBase64).Replace("FFFFFF", _checkedDefault[countImage].BackColor.Name),
                                        };

                                        _variations.regular_price = Convert.ToDecimal(priceDefault);
                                        _variations.sale_price = Convert.ToDecimal(priceDefault);
                                        _variations.stock_quantity = 100;
                                        _variations.sku = rd.Next(minValue: 1000000001, maxValue: 1999999999).ToString() + "-" + _checkedDefault[countImage].Tag.ToString() + "-" + styleDefault;
                                        _variations.image = vrIamge;
                                        _variations.weight = Convert.ToDecimal(1.2);
                                        List<VariationAttribute> attribute = new List<VariationAttribute>();
                                        attribute.Add(new VariationAttribute()
                                        {
                                            id = attribute2.id,
                                            option = styleDefault,
                                            name = styleDefault
                                        });
                                        attribute.Add(new VariationAttribute()
                                        {
                                            id = attribute1.id,
                                            option = _checkedDefault[countImage].Tag.ToString(),
                                            name = _checkedDefault[countImage].Tag.ToString()
                                        });

                                        _variations.attributes = attribute;
                                        vari.Insert(countImage, _variations);
                                        optionAttriButeStyle.Insert(countImage, styleDefault);
                                    }
                                });

                                Task.WaitAll(t1, t2);
                            }

                        }
                        #endregion

                        #region list Normal
                        if (listcheck.Any() && listcheck.Count > 0)
                        {
                            string gearlaunch = "";
                            string price = "";
                            string style = "";
                            bool flag = false;
                            List<CheckBox> _checked = new List<CheckBox>();

                            flag = false;
                            Control chilldControl = listcheck[0].Parent;
                            if (chilldControl.Text == "")
                            {
                                chilldControl.Refresh();
                            }

                            if (chilldControl.Controls.Contains(listcheck[0]))
                            {
                                gearlaunch = chilldControl.Tag.ToString();
                                style = chilldControl.Text.ToString();
                                foreach (Control c in chilldControl.Controls)
                                {
                                    if (c is TextBox)
                                    {
                                        price = c.Text;

                                    }
                                    if (c is CheckBox && ((CheckBox)c).Checked == true)
                                    {
                                        _checked.Add((CheckBox)c);
                                    }
                                }
                                flag = true;
                            }


                            if (flag)
                            {


                                var _checkExistColor = _checked.Find(l => l.Tag == _curTemp.DefColors);//check checkbox tồn tại color của combobox cobdefColor
                                var tmpDefault = _checked[0]; //  
                                if (_checkExistColor != null)
                                {
                                    for (int count = 0; count < _checked.Count; count++)
                                    {
                                        if (_checked[count].Tag == _curTemp.DefColors)
                                        {
                                            _checked[0] = _checkExistColor;
                                            _checked[count] = tmpDefault;
                                        }
                                    }
                                }
                                Task t3 = Task.Factory.StartNew(() =>
                                {
                                    for (int count = 0; count < _checked.Count; count++)
                                    {
                                        optionAttriBute.Add(_checked[count].Tag.ToString());

                                        productImageList.Add(new WooCommerceNET.WooCommerce.v3.ProductImage()
                                        {
                                            src = gearlaunch.Replace("&mark64=", "&mark64=" + arrayBase64).Replace("FFFFFF", _checked[count].BackColor.Name),
                                            name = _checked[count].Tag.ToString(),
                                        });
                                    }
                                });




                                VariationImage vrIamge = new VariationImage();
                                Task t4 = Task.Factory.StartNew(() =>
                                {
                                    for (int count = 0; count < _checked.Count; count++)
                                    {
                                        Variation _variations = new Variation();
                                        vrIamge = new VariationImage
                                        {
                                            src = gearlaunch.Replace("&mark64=", "&mark64=" + arrayBase64).Replace("FFFFFF", _checked[count].BackColor.Name),
                                        };


                                        _variations.regular_price = Convert.ToDecimal(price);
                                        _variations.sale_price = Convert.ToDecimal(price);
                                        _variations.stock_quantity = 100;
                                        _variations.image = vrIamge;
                                        _variations.weight = Convert.ToDecimal(1.2);
                                        _variations.sku = rd.Next(minValue: 1000000001, maxValue: 1999999999).ToString() + "-" + _checked[count].Tag.ToString() + "-" + style;
                                        List<VariationAttribute> attribute = new List<VariationAttribute>();
                                        attribute.Add(new VariationAttribute()
                                        {
                                            id = attribute2.id,
                                            option = _checked[count].Tag.ToString(),
                                            name = _checked[count].Tag.ToString()
                                        });
                                        attribute.Add(new VariationAttribute()
                                        {
                                            id = attribute1.id,
                                            option = style,
                                            name = style
                                        });
                                        _variations.attributes = attribute;
                                        vari.Add(_variations);
                                        optionAttriButeStyle.Add(style);
                                    }
                                });

                                Task.WaitAll(t3, t4);

                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }

                }
                #endregion


                for (int tmpkey = 0; tmpkey < strArrayOne.Count(); tmpkey++)
                {
                    tags.Add(new WooCommerceNET.WooCommerce.v3.ProductTagLine()
                    {
                        id = tagUpload.id,
                        name = strArrayOne[tmpkey].ToString(),
                        slug = strArrayOne[tmpkey].ToString(),
                    });
                }


                attri.Add(new WooCommerceNET.WooCommerce.v3.ProductAttributeLine()
                {
                    id = attribute2.id,
                    variation = true,
                    name = attribute2.name,
                    options = optionAttriButeStyle,
                    visible = true
                });

                attri.Add(new WooCommerceNET.WooCommerce.v3.ProductAttributeLine()
                {
                    id = attribute1.id,
                    variation = true,
                    name = attribute1.name,
                    options = optionAttriBute,
                    visible = true
                });


                if (_curTemp.Title.Contains("{1}")) product.name = _curTemp.Title.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));
                else product.name = _curTemp.Title;


                if (_curTemp.Title.Contains("{1}")) product.title = _curTemp.Title.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));
                else product.title = _curTemp.Title;


                if (_curTemp.Description.Contains("{1}")) product.description = _curTemp.Description.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));
                else product.description = _curTemp.Description;


                if (product.name.Contains("{1}")) product.name = product.name.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));
                if (product.title.Contains("{1}")) product.title = product.title.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));
                if (product.description.Contains("{1}")) product.description = product.description.Replace("{1}", fileList[_tmp_index].Name.Replace(".png", ""));

                product.price = Convert.ToDecimal(priceDefault);
                product.sale_price = Convert.ToDecimal(priceDefault);
                product.regular_price = Convert.ToDecimal(priceDefault);

                product.sku = id_group;
                product.type = "variable";
                product.attributes = attri;
                product.images = productImageList;
                product.categories = category;
                product.tags = tags;
                try
                {
                    _product = await wc.Product.Add(product);
                }
                catch
                {
                    id_group = rd.Next(minValue: 1001, maxValue: 99999).ToString() + "_" + rd.Next(minValue: 100001, maxValue: 999999).ToString();
                    product.sku = id_group;
                    _product = await wc.Product.Add(product);
                }

                string[] Pricearray = new string[_product.images.Count];
                string[] StyleArray = new string[_product.images.Count];
                string[] SkuArray = new string[_product.images.Count];


                for (int demVar = 0; demVar < vari.Count; demVar++)
                {
                    var response = new Variation();
                    try
                    {
                        vari[demVar].sku = _product.id.ToString() + "_" + vari[demVar].sku;
                        response = await wc.Product.Variations.Add(vari[demVar], Convert.ToInt32(_product.id), null);
                    }
                    catch
                    {
                    
                        vari[demVar].sku = (1234 + rd.Next(minValue: 000000, maxValue: 999999)).ToString() + vari[demVar].sku;
                        response = await wc.Product.Variations.Add(vari[demVar], Convert.ToInt32(_product.id), null);
                    }

                    Pricearray[demVar] = response.price.ToString();
                    StyleArray[demVar] = response.attributes[1].option;
                    SkuArray[demVar] = response.sku;
                }

                string _text = "";
                for (int tmpLog = 0; tmpLog < _product.images.Count; tmpLog++)
                {
                    if(tmpLog == _product.images.Count -1)
                        _text += "," + '"' + '"' + "," + '"' + '"' + ",," + '"' + '"' + "," + '"' + '"' + ","
                                + StyleArray[tmpLog] + "," + '"' + _product.images[tmpLog].name + '"' + "," + SkuArray[tmpLog] + ","
                                + Pricearray[tmpLog] + "," + _product.images[tmpLog].src;
                    else
                        _text += "," + '"' + '"' + "," + '"' + '"' + ",," + '"' + '"' + "," + '"' + '"' + ","
                                + StyleArray[tmpLog] + "," + '"' + _product.images[tmpLog].name + '"' + "," + SkuArray[tmpLog] + ","
                                + Pricearray[tmpLog] + "," + _product.images[tmpLog].src + "\n";



                    //_text += (tmpLog == _product.images.Count - 1 ? "," + '"' + '"' + "," + '"' + '"' + ",," + '"' + '"' + "," + '"' + '"' + ","
                    //            + StyleArray[tmpLog] + "," + '"' + _product.images[tmpLog].name + '"' + "," + SkuArray[tmpLog] + ","
                    //            + Pricearray[tmpLog] + "," + _product.images[tmpLog].src :
                    //            "," + '"' + '"' + "," + '"' + '"' + ",," + '"' + '"' + "," + '"' + '"' + ","
                    //            + StyleArray[tmpLog] + "," + '"' + _product.images[tmpLog].name + '"' + "," + SkuArray[tmpLog] + ","
                    //            + Pricearray[tmpLog] + "," + _product.images[tmpLog].src + "\n");

                }

                log += _text;
                WriteLogToTxt(extension, log);
                
            }
          
        }


        private async void UploadData(int _tmp_index, WCObject wc,string arrayBase64, List<ProductCategoryLine> category,string extension)
        {
           
            

        }

        private bool DeclareFirstBeforeStart()
        {


            #region first
            bool checkFirst = true;
            wc_key = tboxwc_Key.Text.Trim().ToString();
            ws_Key = tboxws_Key.Text.Trim().ToString();
            domain = tboxDomain.Text.Trim().ToString();
            domain += "/wp-json/wc/v3/";
            CheckEnableFalse();

            if (!String.IsNullOrEmpty(wc_key) && !String.IsNullOrEmpty(ws_Key) && !String.IsNullOrEmpty(domain))
            {
                rest = new RestAPI(domain, wc_key, ws_Key);

            }



            cboDefaultTye = cobDefType.Text;
            WCObject wc = new WCObject(rest);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("per_page", "100");
            

            username = tboxUserName.Text;
            password = tboxPassword.Text;
            host = tboxServer.Text;
            newDirectory = tboxDirectUpload.Text;

            int noError = 0;
            string _errors = "Nội dung bạn nhập có 1 số lỗi sau. Vui lòng sửa trước khi chạy.";
            if (string.IsNullOrEmpty(cobDefCategory.Text.Trim()))
            {
                _errors += "\r\n+  Fill Category";
                noError++;
            }

            if (string.IsNullOrEmpty(txtKeywords.Text.Trim()))
            {
                _errors += "\r\n+  Key Words can't empty";
                noError++;
            }

            if (noError > 0)
            {

                MessageBox.Show(_errors);
                CheckEnableTrue();
                checkFirst = false;
            }


            
            //GetAllTag();

            #endregion
            richTextBox1.Multiline = false;
            _curTemp = ReadCurrentTemplate();
            ConnectionInfo connectionInfo = new PasswordConnectionInfo(host, username, password);
            try
            {
                using (client = new SftpClient(host, connectionInfo.Port, username, password))
                {
                    client.Connect();
                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                    var files = new List<String>();
                    if (client.IsConnected)
                    {
                        if (string.IsNullOrEmpty(newDirectory))
                        {
                            try
                            {
                                client.ChangeDirectory(directory);

                            }
                            catch
                            {
                                MessageBox.Show("Directory ko đúng hoặc ko đúng định dạng");
                                checkFirst = false;
                                CheckEnableTrue();
                            }

                        }
                        else
                        {
                            try
                            {
                                client.ChangeDirectory(newDirectory);
                            }
                            catch
                            {
                                MessageBox.Show("Directory ko đúng hoặc ko đúng định dạng");
                                checkFirst = false;
                                CheckEnableTrue();
                            }
                        }

                        string remoth = "";
                        remoth = newDirectory;
                        ListFiles(remoth);
                    }
                }
            }

            catch
            {
                MessageBox.Show("Sai thông tin kết nối server");
                checkFirst = false;
            }

            return checkFirst;

        }


        /// <summary>
        /// Lists the files.
        /// </summary>
        /// <param name="remotePath">The remote path.</param>
        /// <param name="failRemoteNotExists">if set to <c>true</c> [fail remote not exists].</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public MyLib.RemoteFileInfo ListFiles(string remotePath, FileInfo file)
        {

            try
            {

                if (!client.Exists(remotePath))
                {
                    MessageBox.Show("not exist Directory to get Image Logo");
                }
                else
                {
                    SftpFile sftpFileInfo = client.Get(remotePath + "/" + file.Name);
                    if (sftpFileInfo.IsRegularFile)
                    {
                        SingleFile = new MyLib.RemoteFileInfo();
                        string filename = System.IO.Path.GetFileName(sftpFileInfo.FullName);
                        SingleFile = CreateFileInfo(sftpFileInfo, client);
                    }
                    else
                    {
                        SingleFile = new MyLib.RemoteFileInfo();
                        SingleFile = CreateFileInfo(sftpFileInfo, client);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SingleFile;
        }

        public List<MyLib.RemoteFileInfo> ListFiles(string remotePath)
        {
            try
            {
                if (!client.Exists(remotePath))
                {
                    MessageBox.Show("not exist Directory to get Image Logo");
                }
                else
                {
                    List<SftpFile> toReturn = new List<SftpFile>();
                    fileList = new List<MyLib.RemoteFileInfo>();
                    toReturn = client.ListDirectory(remotePath).ToList();


                    for (int i = 0; i < toReturn.Count; i++)
                    {
                        fileList.Add(this.CreateFileInfo(toReturn[i], client));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileList;
        }

        private MyLib.RemoteFileInfo CreateFileInfo(SftpFile sftpFile, SftpClient sftp)
        {
            MyLib.RemoteFileInfo fileInfo = new MyLib.RemoteFileInfo();
            fileInfo.name = sftpFile.Name;
            fileInfo.fullName = sftpFile.FullName;
            fileInfo.extension = Path.GetExtension(sftpFile.FullName);
            fileInfo.isDirectory = sftpFile.IsDirectory;
            fileInfo.size = sftpFile.Length;
            fileInfo.modifiedTime = sftpFile.LastWriteTime;
            string tdirect = tboxDirectUpload.Text.Trim();
            string cutString = tdirect.Substring(tdirect.IndexOf("wp-content"), tdirect.Length - tdirect.IndexOf("wp-content"));
            string LinkImage = tboxDomain.Text.Trim() + "/" + cutString + "/" + fileInfo.name;
            if (tboxDomain.Text.Trim() == "https://149.28.75.33") fileInfo.linkImage = LinkImage.Replace("https", "http");
            else fileInfo.linkImage = LinkImage;

            fileInfo.linkImage = LinkImage;
            return fileInfo;
        }


        private MyLib.RemoteFileInfo CreateFileInfo(FileInfo sftpFile, SftpClient sftp)
        {
            MyLib.RemoteFileInfo fileInfo = new MyLib.RemoteFileInfo();
            fileInfo.name = sftpFile.Name;
            fileInfo.fullName = sftpFile.FullName;
            fileInfo.extension = Path.GetExtension(sftpFile.FullName);
            //fileInfo.isDirectory = sftpFile.IsDirectory;
            fileInfo.size = sftpFile.Length;
            fileInfo.modifiedTime = sftpFile.LastWriteTime;
            string tdirect = tboxDirectUpload.Text.Trim();
            string cutString = tdirect.Substring(tdirect.IndexOf("wp-content"), tdirect.Length - tdirect.IndexOf("wp-content"));
            string LinkImage = tboxDomain.Text.Trim() + "/" + cutString + "/" + fileInfo.name;
            if (tboxDomain.Text.Trim() == "https://149.28.75.33") fileInfo.linkImage = LinkImage.Replace("https", "http");
            else fileInfo.linkImage = LinkImage;
            return fileInfo;
        }



        private void rShirts_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void HideTabPage()
        {
            tabControl1.TabPages.Remove(tabCanvasPrint);
            tabControl1.TabPages.Remove(tabPillows);
            tabControl1.TabPages.Remove(tabToteBags);
            tabControl1.TabPages.Remove(tabJewelry);
            tabControl1.TabPages.Remove(tabDrinkware);
            tabControl1.TabPages.Remove(tabPrintsAndFramedArt);
            tabControl1.TabPages.Remove(tabApparel);
        }



        /// <summary>
        /// check CheckBox to add combobox Color.
        /// </summary>
        /// <param name="list"> list is List<List<CheckBox>>  </param>
        /// <returns></returns>
        private void checkCheckBoxAddColor(List<List<CheckBox>> list)
        {

            cobDefColor.DataSource = null;
            cobDefColor.Items.Clear();
            cobDefColor.ResetText();
            cobDefColor.DisplayMember = "Text";
            cobDefColor.ValueMember = "Value";


            string[] value = new string[1000];
            int dem = 1;
            for (int check = 0; check < list.Count; check++)
            {
                for (int i = 0; i < list[check].Count; i++)
                {
                    if (list[check][i].Checked)
                    {
                        if (!value.Contains(list[check][i].Tag.ToString().Trim().ToUpper()))
                        {
                            cobDefColor.Items.Add((new { Text = list[check][i].Tag.ToString(), Value = list[check][i].BackColor.Name }));
                            value[dem] = list[check][i].Tag.ToString().Trim().ToUpper();
                            dem++;
                        }
                    }

                }
            }
            if (cobDefColor.Items.Count > 0)
            {
                cobDefColor.SelectedIndex = 0;
            }





        }
        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }



        private void CheckCheckBoxAddType()
        {
            cobDefType.DataSource = null;
            cobDefType.Items.Clear();
            cobDefType.ResetText();
            string[] value = new string[1000];
            int dem = 0;
            for (int check = 0; check < _listRunl.Count; check++)
            {
                for (int i = 0; i < _listRunl[check].Count; i++)
                {
                    if (_listRunl[check][i].Checked)
                    {
                        if (!value.Contains(_listRunl[check][i].Parent.Text.ToString().Trim().ToUpper()))
                        {
                            cobDefType.Items.Add(new { Text = _listRunl[check][i].Parent.Text });
                            value[dem] = _listRunl[check][i].Parent.Text.ToString().Trim().ToUpper();
                            dem++;
                        }
                    }

                }
            }
            if (cobDefType.Items.Count > 0)
            {
                cobDefType.SelectedIndex = 0;
            }
        }
        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            Console.WriteLine(chk);
            cobDefType.DisplayMember = "Text";

            CheckCheckBoxAddType();
            checkCheckBoxAddColor(_listRunl);


        }

        private void CheckListListCheckBox()
        {
            foreach (Control control in tabControl1.Controls)
            {
                if (control.ToString().StartsWith("TabPage"))
                {
                    switch (control.Name.Trim().ToString())
                    {
                        case "tabApparel":
                            _listRunl = _listApparel;
                            break;
                        case "tabDrinkware":
                            _listRunl = _listDrinkWare;
                            break;
                        case "tabCanvasPrint":
                            _listRunl = _listDrinkWare;
                            break;
                        case "tabPillows":
                            _listRunl = _listDrinkWare;
                            break;
                        case "tabToteBags":
                            _listRunl = _listDrinkWare;
                            break;
                        case "tabJewelry":
                            _listRunl = _listDrinkWare;
                            break;
                        case "tabPrintsAndFramedArt":
                            _listRunl = _listDrinkWare;
                            break;
                    }
                }
            }


        }


        private void CheckTabPageSelected()
        {
            HideTabPage();
            if (rApparel.Checked) tabControl1.TabPages.Add(tabApparel);
            if (rDrinkware.Checked) tabControl1.TabPages.Add(tabDrinkware);
            if (rCanvasPrint.Checked) tabControl1.TabPages.Add(tabCanvasPrint);
            if (rPillows.Checked) tabControl1.TabPages.Add(tabPillows);
            if (rToteBags.Checked) tabControl1.TabPages.Add(tabToteBags);
            if (rJewelry.Checked) tabControl1.TabPages.Add(tabJewelry);
            if (rPrintsAndFramed.Checked) tabControl1.TabPages.Add(tabPrintsAndFramedArt);
        }
        private void rApparel_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Checked)
            {
                CheckTabPageSelected();
                CheckListListCheckBox();
            }
        }

        private void rDrinkware_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Checked)
            {
                CheckTabPageSelected();
                CheckListListCheckBox();
            }
        }

        private void rCanvasPrint_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Checked)
            {
                CheckTabPageSelected();
                CheckListListCheckBox();
            }
        }

        private void rPillows_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Checked)
            {
                CheckTabPageSelected();
                CheckListListCheckBox();
            }
        }

        private void rToteBags_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Checked)
            {
                CheckTabPageSelected();
                CheckListListCheckBox();
            }
        }

        private void rJewelry_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Checked)
            {
                CheckTabPageSelected();
                CheckListListCheckBox();
            }
        }

        private void rPrintsAndFramed_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Checked)
            {
                CheckTabPageSelected();
                CheckListListCheckBox();
            }
        }

        private void btnCheckKey_Click(object sender, EventArgs e)
        {


            wc_key = tboxwc_Key.Text.Trim().ToString();
            ws_Key = tboxws_Key.Text.Trim().ToString();
            domain = tboxDomain.Text.Trim().ToString();
            domain += "/wp-json/wc/v3/";
            try
            {
                RestAPI rest = new RestAPI(domain,
                                       wc_key,
                                       ws_Key);
                WCObject wc = new WCObject(rest);

                if (rest != null) MessageBox.Show("Connect Success");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Can't Connect");
            }
        }

        private void CheckEnableFalse()
        {
            for (int check = 0; check < _listRunl.Count; check++)
            {
                for (int i = 0; i < _listRunl[check].Count; i++)
                {
                    _listRunl[check][i].Enabled = false;
                }
            }
            txtTitle.Enabled = false;
            txtDesc.Enabled = false;
            txtKeywords.Enabled = false;
            cobDefType.Enabled = false;
            cobDefColor.Enabled = false;
            cobDefCategory.Enabled = false;
            cobTemplate.Enabled = false;
        }

        private void CheckEnableTrue()
        {
            for (int check = 0; check < _listRunl.Count; check++)
            {
                for (int i = 0; i < _listRunl[check].Count; i++)
                {
                    _listRunl[check][i].Enabled = true;
                }
            }
            txtTitle.Enabled = true;
            txtDesc.Enabled = true;
            txtKeywords.Enabled = true;
            btnStart.Enabled = true; cobDefType.Enabled = true;
            cobDefColor.Enabled = true;
            cobDefCategory.Enabled = true;
            cobTemplate.Enabled = true;
        }

        private void CheckEnableTrueForSuccess()
        {
            for (int check = 0; check < _listRunl.Count; check++)
            {
                for (int i = 0; i < _listRunl[check].Count; i++)
                {
                    _listRunl[check][i].Invoke(new MethodInvoker(delegate ()
                    {
                        _listRunl[check][i].Enabled = true;
                    }));
                }
            }



            txtTitle.Invoke((MethodInvoker)delegate { txtTitle.Enabled = true; });
            txtDesc.Invoke((MethodInvoker)delegate { txtDesc.Enabled = true; });
            txtKeywords.Invoke((MethodInvoker)delegate { txtKeywords.Enabled = true; });
            btnStart.Invoke((MethodInvoker)delegate { btnStart.Enabled = true; });
            cobDefColor.Invoke((MethodInvoker)delegate { cobDefColor.Enabled = true; });
            cobDefCategory.Invoke((MethodInvoker)delegate { cobDefCategory.Enabled = true; });
            cobTemplate.Invoke((MethodInvoker)delegate { cobTemplate.Enabled = true; });

        }

        private void btnClearColor_Click(object sender, EventArgs e)
        {
            for (int check = 0; check < _listRunl.Count; check++)
            {
                for (int i = 0; i < _listRunl[check].Count; i++)
                {
                    _listRunl[check][i].Checked = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void btnSaveTemp_Click(object sender, EventArgs e)
        {
            Template temp = ReadCurrentTemplate();

            string name = Interaction.InputBox("name?");
            if (!string.IsNullOrWhiteSpace(name))
            {
                File.WriteAllText(_Folder_Temp + "\\" + name + ".json", JsonConvert.SerializeObject(temp));
                MessageBox.Show("Saved!");
                Refresh_List_Temp();
            }
        }

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
                txtTitle.Text = temp.Title;
                txtDesc.Text = temp.Description;
                txtKeywords.Text = temp.Keywords;
                tboxServer.Text = temp.Server;
                tboxUserName.Text = temp.username;
                tboxDirectUpload.Text = temp.DirectoryGetImage;
                tboxwc_Key.Text = temp.wc_key;
                tboxws_Key.Text = temp.ws_key;
                tboxDomain.Text = temp.Domain;
                txtThread.Text = temp.Thread.ToString();
                txtSleep.Text = temp.Sleep.ToString();


                foreach (var item in temp.Types)
                {
                    foreach (Control c in tabApparel.Controls)
                    {
                        if (c is GroupBox)
                        {
                            if (c.Text == item.name)
                            {
                                foreach (Control ctrl in c.Controls)
                                {
                                    if (ctrl is CheckBox)
                                    {
                                        CheckBox _checkBox = ctrl as CheckBox;
                                        _checkBox.Checked = false;
                                        if (item.colors.Contains(ctrl.BackColor.Name))
                                        {
                                            _checkBox.Checked = true;

                                        }
                                    }

                                    if (ctrl is TextBox)
                                    {
                                        var lab = temp.Types.Find(x => x.name == c.Text.ToString());
                                        if (lab != null)
                                        {
                                            TextBox tex = ctrl as TextBox;
                                            tex.Text = lab.price.ToString(); ;
                                        }

                                    }
                                }
                            }
                        }

                        
                    }
                }

                Debug.WriteLine(temp);
            }
            catch
            {

            }
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

        private async void CheckAttriBute()
        {
            ProductAttribute item1 = new ProductAttribute();
            item1.name = "Color";
            item1.slug = "Color";

            ProductAttribute item2 = new ProductAttribute();
            item2.name = "Style";
            item2.slug = "Style";



            fileList = fileList.FindAll(x => x.extension == ".png" || x.extension == ".img").ToList();
            fileList = fileList.FindAll(x => !x.Name.Contains("#")).ToList();
            WCObject wc = new WCObject(rest);
            var attri = await wc.Attribute.GetAll();

            attribute1 = new Attri();
            attribute2 = new Attri();
            if (attri.Count < 1)
            {
                var at1 = await wc.Attribute.Add(item1);
                attribute1.id = (int)at1.id;
                attribute1.name = at1.name;
                attribute1.slug = at1.slug;

                var at2 = await wc.Attribute.Add(item2);
                attribute2.id = (int)at2.id;
                attribute2.name = at2.name;
                attribute2.slug = at2.slug;
            }
            else
            {
                var at1 = attri.Find(x => x.name == "Color");
                var at2 = attri.Find(x => x.name == "Style");


                if (at1 == null) at1 = await wc.Attribute.Add(item1);
                if (at2 == null) at2 = await wc.Attribute.Add(item2);

                attribute1.id = (int)at1.id;
                attribute1.name = at1.name;
                attribute1.slug = at1.slug;

                attribute2.id = (int)at2.id;
                attribute2.name = at2.name;
                attribute2.slug = at2.slug;
            }

        }
    }


    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<string> data { get; set; }
    }


}