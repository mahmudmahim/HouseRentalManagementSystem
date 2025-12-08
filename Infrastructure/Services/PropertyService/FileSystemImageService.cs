using HouseRentalApplication.Common.Interfaces.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseRentalDomain.Entities.Common.CommonEntities;

namespace HouseRentalInfrastructure.Services.PropertyService
{
    public class FileSystemImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileSystemImageService> _logger;
        private const string BaseFolder = "uploads/properties";

        public FileSystemImageService(IWebHostEnvironment env, ILogger<FileSystemImageService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<ImageSaveResult> SaveAsync(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var folder = Path.Combine(_env.WebRootPath, BaseFolder);

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, fileName);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            // return relative URL (served from wwwroot)
            var url = $"/{BaseFolder}/{fileName}".Replace("\\", "/");
            return new ImageSaveResult { Url = url, Id = fileName };
        }

        public Task DeleteAsync(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) return Task.CompletedTask;
                var filename = Path.GetFileName(url);
                var filePath = Path.Combine(_env.WebRootPath, BaseFolder, filename);
                if (File.Exists(filePath)) File.Delete(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete image");
            }
            return Task.CompletedTask;
        }
    }
}
