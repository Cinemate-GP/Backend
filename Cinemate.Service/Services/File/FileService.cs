using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Cinemate.Service.Services.File
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadsPath;
        private readonly string _imagesPath;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _uploadsPath = $"{_webHostEnvironment.WebRootPath}/uploads";
            _imagesPath = $"{_webHostEnvironment.WebRootPath}/images";
        }
        public async Task<string> SaveFileAsync(IFormFile imageFile, string subfolder)
        {
            if (imageFile is null)
                throw new ArgumentNullException(nameof(imageFile));

            var path = Path.Combine(_imagesPath, subfolder);

            // Ensure the directory exists
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Get the file extension from the uploaded file
            var extension = Path.GetExtension(imageFile.FileName);

            // Generate a unique name for the file
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fileNamePath = Path.Combine(path, fileName);

            // Save the file to the uploads directory
            using var stream = new FileStream(fileNamePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return fileName;
        }


        public void DeleteFile(string file, string subfolder)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException(nameof(file), "The file name cannot be null or empty.");

            if (string.IsNullOrEmpty(subfolder))
                throw new ArgumentNullException(nameof(subfolder), "The subfolder cannot be null or empty.");

            var path = Path.Combine(_imagesPath, subfolder, file);

            // Log the path for debugging purposes
            Console.WriteLine($"Deleting file at: {path}");

            if (!System.IO.File.Exists(path))
            {
                // Provide a detailed exception message
                throw new FileNotFoundException($"File not found at path: {path}");
            }

            try
            {
                System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                // Catch any other IO exceptions and log them
                throw new IOException($"An error occurred while deleting the file: {path}", ex);
            }
        }

    }
}
