using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Service
{
    public interface IProfanityScannerService
    {
        Task<List<(int Index, string Word)>> FindProfanityInText(string text);
    }
}
