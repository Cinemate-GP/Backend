using Cinemate.Core.Abstractions.Consts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Files
{
    public class BlockedSignaturesValidator : AbstractValidator<IFormFile>
    {
        public BlockedSignaturesValidator()
        {
            RuleFor(x => x)
            .Must((request, context) =>
            {
                BinaryReader binary = new(request.OpenReadStream());
                var bytes = binary.ReadBytes(2);

                var fileSequenceHex = BitConverter.ToString(bytes);

                foreach (var signature in FileSettings.BlockedSignatures)
                    if (signature.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                        return false;

                return true;
            })
            .WithMessage("Not allowed file content")
            .When(x => x is not null);
        }
    }
}
