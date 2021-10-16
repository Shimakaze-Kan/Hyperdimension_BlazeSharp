using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Service
{
    public class ProfanityScannerService : IProfanityScannerService
    {
        private List<(int Index, string Word)> _foundWords;
        private List<string> _profaneWords;

        public async Task<List<(int Index, string Word)>> FindProfanityInText(string text)
        {
            _foundWords = new();
            if (_profaneWords is null)
            {
                _profaneWords = await GetProfaneWords().ConfigureAwait(false);
            }
            
            AddCommonWordsToCollection(text, _profaneWords);

            return _foundWords;
        }

        private void AddCommonWordsToCollection(string text, List<string> words)
        {
            var commonWords = Regex.Split(text, @"[^\w+]").Select(x => x.ToLower()).Intersect(words);

            foreach (var word in commonWords)
            {
                _foundWords.Add((text.IndexOf(word), word));
            }
        }

        private static async Task<List<string>> GetProfaneWords()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("ProfaneWords/words.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var words = JsonConvert.DeserializeObject<List<string>>(json);
            return words;
        }
    }
}
