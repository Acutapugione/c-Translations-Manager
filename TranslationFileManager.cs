using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace TranslationReferenceBooks
{
    public static class TranslationFileManager
    {
        public static JsonSerializerOptions options = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.Cyrillic, UnicodeRanges.All),
            WriteIndented = true
        };
        public static string filename = @"c:\temp\my_file.txt";
        public static async Task SaveAsync()
        {
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
               await streamWriter.WriteAsync( JsonSerializer.Serialize(TranslationDictManager.TranslationDicts, options));
            }
            
        }
        public static async Task<List<TranslationDict>> LoadAsync()
        {
            if (!File.Exists(filename))
            {
                return new List<TranslationDict>();
            }
            
            using (FileStream fileStream = File.OpenRead(filename))
            {
                if (!fileStream.CanRead)
                    return new List<TranslationDict>();
                return await JsonSerializer.DeserializeAsync<List<TranslationDict>>(fileStream, options);
            }
            
        }
    }
}
