using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Abstractions.Consts
{
    public class FileSettings
    {
        public const int MaxFileSizeInMB = 1;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
        public static readonly string[] BlockedSignatures = ["4D-5A", "2F-2A", "D0-CF"];
        public static readonly string[] AllowedImagesExtensions = [".jpg", ".jpeg", ".png"];
    }
}
