using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class UserPreferences
    {
        [MaxLength(255)]
        public string About { get; set; }
        public int? ThemeId { get; private set; } = null;

        public void SetTheme(string websiteTheme, string editorTheme)
        {
            ThemeId = (websiteTheme, editorTheme) switch
            {
                ("light", "Visual Studio") => 1,
                ("light", "Visual Studio Dark") => 2,
                ("light", "High Contrast Black") => 3,
                ("dark", "Visual Studio") => 4,
                ("dark", "Visual Studio Dark") => 5,
                ("dark", "High Contrast Black") => 6,
                _ => null
            };
        }
    }
}
