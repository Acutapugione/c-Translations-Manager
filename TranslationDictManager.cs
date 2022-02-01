using System;
using System.Collections.Generic;

namespace TranslationReferenceBooks
{
    public static class TranslationDictManager
    {
        private static List<TranslationDict> translationDicts = new();
        public static void AddTranslationDict(TranslationDict translationDict)
        {
            if (translationDict is null)
            {
                throw new ArgumentNullException(nameof(translationDict));
            }
            if (Find(translationDict.Type)!=null) return;
            TranslationDicts.Add(translationDict);
        }
        public static void AddTranslationDict(TranslationType type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (Find(type) != null) return;
            TranslationDicts.Add(new TranslationDict(new Dictionary<string, List<string>>(), type));
        }
        public static void AddTranslationDict(string targetLanguage, string fromLanguage)
        {
            if (string.IsNullOrEmpty(targetLanguage))
            {
                throw new ArgumentException($"\"{nameof(targetLanguage)}\" не может быть неопределенным или пустым.", nameof(targetLanguage));
            }

            if (string.IsNullOrEmpty(fromLanguage))
            {
                throw new ArgumentException($"\"{nameof(fromLanguage)}\" не может быть неопределенным или пустым.", nameof(fromLanguage));
            }
            if (Find(new TranslationType(targetLanguage, fromLanguage)) != null) return;
            TranslationDicts.Add(new TranslationDict(new Dictionary<string, List<string>>(), new TranslationType(targetLanguage, fromLanguage)));
        }
        public static void RemoveTranslationDict(TranslationType type)
        {
            if (Find(type) != null)
                TranslationDicts.Remove(Find(type));
        }
        #nullable enable
        public static TranslationDict? Find(TranslationType type)
        {
            return TranslationDicts.Find(x => x.Type == type);
        }
        public static TranslationDict? Find(string targetLanguage, string fromLanguage)
        {
            return TranslationDicts.Find(x => x.Type == new TranslationType(targetLanguage, fromLanguage));
        }
        #nullable disable
        public static new string ToString()
        {
            string result = "";
            foreach (var item in TranslationDicts)
            {
                
                result += item.ToString();
            }
            return result;
        }

        public static List<TranslationDict> TranslationDicts
        {
            get
            {
                return translationDicts;
            }

            set
            {
                translationDicts = value;
            }
        }
    }
}
