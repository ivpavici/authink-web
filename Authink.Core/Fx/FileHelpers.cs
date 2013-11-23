using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Authink.Core.Fx
{
    public static class FileHelpers
    {
        public static string CreateUniqueFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);

            return Guid.NewGuid().ToString() + fileExtension;
        }
        public static byte[] Transform_HttpPostedFileBase_Into_Bytes(HttpPostedFileBase file)
        {
            var buffer = new byte[file.ContentLength];
            file.InputStream.Read
            (
                buffer: buffer,
                offset: 0,
                count: file.ContentLength
            );

            return buffer;
        }
    }
}
