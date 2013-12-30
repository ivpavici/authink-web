using System;
using System.IO;
using System.Web;

namespace Authink.Core.Fx
{
    public static class FileHelpers
    {
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
