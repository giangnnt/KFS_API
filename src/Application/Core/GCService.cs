using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace KFS.src.Application.Core
{
    public interface IGCService
    {
        Task<string?> UploadFileAsync(Stream fileStream, string destinationFileName, string contentType);
        Task<bool> DeleteFileAsync(string fileUrl);
    }
    public class GCService : IGCService
    {
        private const string PROJECT_ID = "kfsapi-436409";
        private const string FOLDER = "media/";
        private readonly string BucketName;
        private readonly StorageClient StorageClient;
        private readonly string FilePrefix;
        public GCService()
        {
            // Initialize the Google Cloud Storage client
            string credentialsPath = "firebase.json"; // Replace with your Service Account Key path

            var credentials = GoogleCredential.FromFile(credentialsPath);
            StorageClient = StorageClient.Create(credentials);
            BucketName = PROJECT_ID + ".appspot.com"; // Default Firebase Storage bucket
            FilePrefix = "https://storage.googleapis.com/" + BucketName + "/";
        }
        public async Task<string?> UploadFileAsync(Stream fileStream, string destinationFileName, string contentType)
        {
            try
            {
                var objectName = FOLDER + destinationFileName;

                // Upload the file to Firebase Storage
                var options = new UploadObjectOptions
                {
                    PredefinedAcl = PredefinedObjectAcl.PublicRead // Set the PredefinedAcl to PublicRead
                };
                await StorageClient.UploadObjectAsync(BucketName, objectName, contentType, fileStream, options);

                // Generate a URL to the uploaded content
                var fileUrl = FilePrefix + objectName;
                return fileUrl;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the upload
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                // Get the file name from the file URL
                var fileName = fileUrl.Replace(FilePrefix, "");

                // Delete the file from Firebase Storage
                await StorageClient.DeleteObjectAsync(BucketName, fileName);

                return true;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the delete
                Console.WriteLine($"Error deleting file: {ex.Message}");
                return false;
            }
        }
    }
}