using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Profile
{
    public record UpdateProfileReauestBack
    {
       public string? FullName { get; init; }
       public string? Email { get; init; }
       public string? Profile_Image { get; init; }
    }
}
