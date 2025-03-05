using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Authentication
{
    public record RegisterRequest
    (
        string Email,
        string Password,
        string FullName,
        string Gender,
        DateOnly BirthDay
    );
}
