﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropCreate.Common
{
    class UploadImage
    {

        public string id = "__dataURI:0__";
        public string uri = "";

        public UploadImage(string file)
        {
            uri = "data:image/png;base64," + ImageBase64(file);
        }

        public static string ImageBase64(string path)
        {
            try
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(path);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                return base64ImageRepresentation;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
