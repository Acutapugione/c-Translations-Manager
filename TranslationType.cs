using System;
using System.Text.Json.Serialization;

namespace TranslationReferenceBooks
{
    public class TranslationType
    {
        public TranslationType(string targetlanguage, string fromlanguage)
        {
            Targetlanguage = targetlanguage ?? throw new ArgumentNullException(nameof(targetlanguage));
            Fromlanguage = fromlanguage ?? throw new ArgumentNullException(nameof(fromlanguage));
        }

        public string Targetlanguage { get; set; }
        public string Fromlanguage { get; set; }
        public static bool operator ==(TranslationType typ1, TranslationType typ2)
        {
            return typ1.Targetlanguage == typ2.Targetlanguage && typ1.Fromlanguage == typ2.Fromlanguage;
        }
        public static bool operator !=(TranslationType typ1, TranslationType typ2)
        {
            return typ1.Targetlanguage != typ2.Targetlanguage && typ1.Fromlanguage != typ2.Fromlanguage;
        }
        [JsonIgnore]
        public new string ToString
        {
            get
            {
                return $"{Targetlanguage} - {Fromlanguage}";
            }
        }
    }

    
}