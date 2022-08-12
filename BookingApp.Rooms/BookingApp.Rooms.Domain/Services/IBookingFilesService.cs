namespace BookingApp.Rooms.Domain.Services
{
    public interface IBookingFilesService
    {
        Task UploadBookingFileAsync(Stream file, string filePath);
        Task<byte[]> DownloadBookingFileAsync(string filePath);
    }
}
