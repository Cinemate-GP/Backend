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
        string FirstName,
        string LastName,
        string Gender,
        DateOnly BirthDay

        );
}
