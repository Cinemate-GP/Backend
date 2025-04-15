using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Service_Contract
{
    public interface IFileService
    {
        public Task<string> SaveFileAsync(IFormFile imageFile, string subfolder);
        public void DeleteFile(string file, string subfolder);


    }
}
