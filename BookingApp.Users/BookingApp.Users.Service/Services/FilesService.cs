using AutoMapper;
using BookingApp.Users.Client;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using static BookingApp.Users.Client.FilesService;

namespace BookingApp.Users.Service.Services
{
    public class FilesService : FilesServiceBase
    {
        private readonly ILogger<FilesService> _logger;
        private readonly IMapper _mapper;

        public FilesService(ILogger<FilesService> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<FileResponse> UploadFile(IAsyncStreamReader<FileRequest> requestStream, ServerCallContext context)
        {
            var filePath = "";
            while (await requestStream.MoveNext())
            {
                var current = requestStream.Current;
                filePath = current.FilePath;
                using var stream = new FileStream(current.FilePath, FileMode.Create);
                await stream.WriteAsync(current.FileData.ToByteArray().AsMemory(0, current.FileData.Length));
            }

            return new FileResponse
            {
                FilePath = filePath
            };
        }

        public override async Task DownloadFile(DownloadFileRequest request, IServerStreamWriter<DownloadFileResponse> responseStream, ServerCallContext context)
        {
            var path = Path.Combine(request.FilePath);

            using var fs = new FileStream(path, FileMode.Open);
            var memoryStream = new MemoryStream();
            fs.CopyTo(memoryStream);

            var t = new DownloadFileResponse
            {
                DownloadFile = ByteString.CopyFrom(memoryStream.ToArray())
            };

            await responseStream.WriteAsync(t);
        }
    }
}
