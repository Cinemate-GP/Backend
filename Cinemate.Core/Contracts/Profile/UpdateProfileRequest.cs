using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Profile
{
    public record UpdateProfileRequest
    (
        string? FullName,
        string? Email,
        string? Password,
        IFormFile? Profile_Image
        );
}
