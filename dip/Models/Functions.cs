using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace dip.Models
{
    public static class Functions
    {
        public static string NullToEmpryStr(string str)
        {
            return str ?? "";
        }

        /// <summary>
        /// метод для преобразования HttpPostedFileBase в байты
        /// </summary>
        /// <param name="uploadImage">изображения</param>
        /// <returns>список массивов байтов</returns>
        public static List<byte[]> Get_photo_post(HttpPostedFileBase[] uploadImage)//this Image a,
        {
            List<byte[]> res = new List<byte[]>();
            if (uploadImage != null)
            {
                foreach (var i in uploadImage)
                {
                    try
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(i.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(i.ContentLength);
                        }
                        res.Add(imageData);
                    }
                    catch
                    { }
                }
            }
            return res;
        }
    }
}