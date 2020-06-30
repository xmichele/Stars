using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Stars.Router
{
    internal class WebResource : IResource
    {
        private WebResponse webResponse;
        private Stream responseStream;

        public WebResource(WebResponse webResponse)
        {
            this.webResponse = webResponse;
            responseStream = CopyAndClose(webResponse.GetResponseStream());
        }

        public string Label => webResponse.ResponseUri.ToString();

        public ContentType ContentType => new ContentType(webResponse.ContentType);

        public Uri Uri => webResponse.ResponseUri;

        public string ReadAsString()
        {
            responseStream.Position = 0;
            return new StreamReader(responseStream).ReadToEnd();
        }

        private static Stream CopyAndClose(Stream inputStream)
        {
            const int readSize = 256;
            byte[] buffer = new byte[readSize];
            MemoryStream ms = new MemoryStream();

            int count = inputStream.Read(buffer, 0, readSize);
            while (count > 0)
            {
                ms.Write(buffer, 0, count);
                count = inputStream.Read(buffer, 0, readSize);
            }
            ms.Position = 0;
            inputStream.Close();
            return ms;
        }
    }
}