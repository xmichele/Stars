using System;
using System.IO;
using System.Net;
using System.Net.S3;
using System.Threading.Tasks;
using Terradue.Stars.Interface;
using Terradue.Stars.Interface.Router;
using Terradue.Stars.Interface.Supplier.Destination;
using Terradue.Stars.Services.Router;

namespace Terradue.Stars.Services.Supplier.Destination
{
    public class S3ObjectDestination : IDestination
    {
        private readonly Uri s3Uri;

        private S3ObjectDestination(Uri s3Uri)
        {
            if (!s3Uri.Scheme.Equals("s3", StringComparison.CurrentCultureIgnoreCase))
                throw new InvalidDataException("Only s3 URL supported");
            this.s3Uri = s3Uri;
        }

        public string BucketName => S3UriParser.GetBucketName(s3Uri);

        public static S3ObjectDestination Create(string s3UriStr, IResource route)
        {
            Uri s3Uri = new Uri(s3UriStr);
            return (S3ObjectDestination)(new S3ObjectDestination(s3Uri)).To(route);
        }

        public Uri Uri => s3Uri;

        public void PrepareDestination()
        {
            // S3WebRequest s3WebRequest = (S3WebRequest)WebRequest.Create(s3Uri);
        }

        public IDestination To(IResource origin, string relPathFix = null)
        {
            // we first integrate the relPath
            string relPath = relPathFix ?? "";

            // we identify the filename
            string filename = Path.GetFileName(origin.Uri.IsAbsoluteUri ? origin.Uri.LocalPath : origin.Uri.ToString());
            if (origin.ContentDisposition != null && !string.IsNullOrEmpty(origin.ContentDisposition.FileName))
                filename = origin.ContentDisposition.FileName;

            // if the relPath requested is null, we will build one from the origin route to the new one
            if (relPathFix == null)
            {
                if (origin.Uri.IsAbsoluteUri)
                {
                    // Let's see if the 2 routes are relative
                    var relUri = origin.Uri.MakeRelativeUri(Uri);
                    // If not, let's see if they have a common pattern
                    if (relUri.IsAbsoluteUri)
                    {
                        if (!string.IsNullOrEmpty(Path.GetDirectoryName(origin.Uri.AbsolutePath)) &&
                            !string.IsNullOrEmpty(Path.GetDirectoryName(Uri.AbsolutePath)) &&
                            Path.GetDirectoryName(origin.Uri.AbsolutePath).StartsWith(Path.GetDirectoryName(Uri.AbsolutePath)))
                        {
                            relPath = Path.GetDirectoryName(origin.Uri.AbsolutePath).Replace(Path.GetDirectoryName(Uri.AbsolutePath), "");
                        }
                    }
                    else
                        relPath = Path.GetDirectoryName(relUri.ToString());
                }
                else
                    relPath = Path.GetDirectoryName(origin.Uri.ToString());
            }
            var newFilePath = Path.Join(relPath, filename);
            Uri newUri = new Uri(string.Format("s3:/" + Path.GetFullPath(Path.Join(
                                               "/" + S3UriParser.GetBucketName(s3Uri),
                                               Path.GetDirectoryName(S3UriParser.GetKey(s3Uri)),
                                               newFilePath))));
            return new S3ObjectDestination(newUri);
        }

        public override string ToString()
        {
            return s3Uri.ToString();
        }
    }
}