using BookingApp.Rooms.Domain.Services;
using BookingApp.Users.Client;
using Google.Protobuf;
using Grpc.Core;

namespace BookingApp.Rooms.DomainService
{
    public class BookingFilesService : IBookingFilesService
    {
        private readonly FilesService.FilesServiceClient _filesServiceClient;

        public BookingFilesService(FilesService.FilesServiceClient filesServiceClient)
        {
            _filesServiceClient = filesServiceClient;
        }

        public async Task UploadBookingFileAsync(Stream file, string filePath)
        {
            using var clientBulk = _filesServiceClient.UploadFile();

            var fileRequest = new FileRequest
            {
                FilePath = filePath
            };

            using var memoryStream = new MemoryStream();
            file.Position = 0;
            file.CopyTo(memoryStream);

            // Convert Stream To Array
            byte[] byteArray = memoryStream.ToArray();
            fileRequest.FileData = ByteString.CopyFrom(byteArray);

            await clientBulk.RequestStream.WriteAsync(fileRequest);
            await clientBulk.RequestStream.CompleteAsync();

            var responseBulk = await clientBulk;
        }

        public async Task<byte[]> DownloadBookingFileAsync(string filePath)
        {
            var req = new DownloadFileRequest()
            {
                FilePath = filePath
            };

            var arr = Array.Empty<byte>();

            using var clientBulk = _filesServiceClient.DownloadFile(req);
            await foreach (var fileResponse in clientBulk.ResponseStream.ReadAllAsync())
            {
                arr = fileResponse.DownloadFile.ToArray();
            }

            return arr;
        }
    }
}
