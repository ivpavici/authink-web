using System;
using System.IO;
using System.Net;
using NLog;

namespace Authink.Data.ResourceFileStorage
{
    public class ResourceFileStorageAdapter: IFileStorageAdapter
    {
        public ResourceFileStorageAdapter
        (
            Settings settings,
            Logger   logger
        )
        {
            this.settings = settings;
            this.logger   = logger;
        }

        private readonly Settings settings;
        private readonly Logger   logger;

        public Uri  Upload(ResourceFile resourceFile, string baseFoldername  )
        {
            var uploadedFileName = GetResourceFileNameWithRandomPart(resourceFile.FileName);

            var isUploadSuccessful = UploadFile
            (
                data:           resourceFile.Data,
                fileName:       uploadedFileName,
                baseFolderName: baseFoldername
            );

            if (isUploadSuccessful)
            {
                var httpFileUrl = new Uri(settings.HttpHostname, string.Format("{0}{1}", baseFoldername, uploadedFileName));

                return httpFileUrl;
            }

            return null;
        }
        public bool Remove(Uri resourceFileDownloadUrl)
        {
            logger.Info("Removing resource with url = {0}", resourceFileDownloadUrl.ToString());

            var fileName            = resourceFileDownloadUrl.AbsolutePath.TrimStart('/');
            var isRemovalSuccessful = RemoveFile(fileName);

            return isRemovalSuccessful;
        }

        private bool UploadFile(byte[] data, string fileName, string baseFolderName)
        {
            var ftpStorageLocationPath = new Uri(settings.FtpHostname, baseFolderName);

            var ftpRequestUrl = new Uri(ftpStorageLocationPath, fileName);

            try
            {
                var request = (FtpWebRequest)WebRequest.Create(ftpRequestUrl);
            
                request.Method        = WebRequestMethods.Ftp.UploadFile;
                request.Credentials   = new NetworkCredential(settings.Username, settings.Password);
                request.ContentLength = data.Length;
                
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }

                var response = (FtpWebResponse)request.GetResponse();
                var responseStatusCode = response.StatusCode;
                response.Close();

                return responseStatusCode == FtpStatusCode.ClosingData;
            }
            catch (Exception ex)
            {
                logger.Error("Ftp web request failed", ex);
                throw;
            }
            
        }
        private bool RemoveFile(string fileName             )
        {
            var ftpRequestUrl = new Uri(settings.FtpHostname, fileName);
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(ftpRequestUrl);

                request.Method      = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(settings.Username, settings.Password);

                var response = (FtpWebResponse)request.GetResponse();
                var responseStatusCode = response.StatusCode;
                response.Close();

                return responseStatusCode == FtpStatusCode.FileActionOK;
            }
            catch (Exception ex)
            {
                logger.Error("Ftp web request failed", ex);
                throw;
            } 
        }

        private string GetResourceFileNameWithRandomPart(string fileName)
        {
            var randomPartLength      = this.settings.UploadedFileNameRandomPartLength;
            var randomPart            = Guid.NewGuid().ToString("n").Substring(0, randomPartLength);
            var originalFileName      = Path.GetFileNameWithoutExtension(fileName);
            var originalFileExtension = Path.GetExtension(fileName);

            var destinationFileName = String.Format("{0}-{1}{2}", originalFileName, randomPart, originalFileExtension);

            return destinationFileName;
        }

        public class Settings
        {
            public Settings
            (
                Uri    ftpHostname,
                string username,
                string password,
                int    uploadedFileNameRandomPartLength,
                Uri    httpHostname
            )
            {
                this.FtpHostname                      = ftpHostname;
                this.Username                         = username;
                this.Password                         = password;
                this.UploadedFileNameRandomPartLength = uploadedFileNameRandomPartLength;
                this.HttpHostname                     = httpHostname;
            }

            public Uri    FtpHostname                      { get; private set; }
            public Uri    HttpHostname                     { get; private set; }
            public string Username                         { get; private set; }
            public string Password                         { get; private set; }
            public int    UploadedFileNameRandomPartLength { get; private set; }
        }

        public class ResourceFile
        {
            public ResourceFile
            (
                string fileName,
                byte[] data
            )
            {
                this.FileName = fileName;
                this.Data     = data;
            }

            public string FileName { get; private set; }
            public byte[] Data     { get; private set; }
        }
    }
}
