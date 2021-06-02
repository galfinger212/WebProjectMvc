using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebProject.Helpers
{
    public class FileUpload
    {
        internal string UpLoadFile(IFormFile file)//return file name that we need to save in DB(base64string)
        {
            string s = null;
            if (file == null) return null;
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    s += "data:"+file.ContentType + ";base64,";
                    s += Convert.ToBase64String(fileBytes);
                }
            }
            return s;
        }
    }
}
