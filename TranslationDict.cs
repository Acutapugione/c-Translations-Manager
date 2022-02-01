using System;
using System.Collections.Generic;
using System.Linq;


namespace TranslationReferenceBooks
{

    public partial class TranslationDict : ITranslationDict
    {
        public TranslationDict(Dictionary<string, List<string>> translation_items, TranslationType type)
        {
            Translation_items = translation_items ?? throw new ArgumentNullException(nameof(translation_items));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public Dictionary<string, List<string>> Translation_items { get; set; }
        public TranslationType Type { get; set; }

        public bool TryAddTranslationItem(Dictionary<string, List<string>> translation_item)
        {
            if (translation_item is null)
            {
                throw new ArgumentNullException(nameof(translation_item));
            }

            foreach (var item in translation_item)
            {
                if (Translation_items.ContainsKey(item.Key))
                {
                    Translation_items[item.Key] = item.Value.Union(Translation_items[item.Key]).ToList();
                }
                else
                {
                    Translation_items[item.Key] = item.Value;
                }
            }
            return true;
        }
        public bool TryAddTranslationItem(TranslationDict translation_item)
        {
            if (translation_item is null)
            {
                throw new ArgumentNullException(nameof(translation_item));
            }
            return TryAddTranslationItem(translation_item.Translation_items);
        }
       
        public bool TryAddTranslation(string targetWord, IEnumerable<string> translations)
        {
            if (string.IsNullOrEmpty(targetWord))
            {
                throw new ArgumentException($"{nameof(targetWord)}");
            }

            if (translations is null)
            {
                throw new ArgumentNullException(nameof(translations));
            }

            foreach (var item in translations)
            {
                TryAddTranslation(targetWord, item);
            }
            return true;
        }
        public bool TryAddTranslation(string targetWord, params string[] translations)
        {
            if (string.IsNullOrEmpty(targetWord))
            {
                throw new ArgumentException($"\"{nameof(targetWord)}\" не может быть неопределенным или пустым.", nameof(targetWord));
            }

            if (translations is null)
            {
                throw new ArgumentNullException(nameof(translations));
            }

            foreach (var item in translations)
            {
                TryAddTranslation(targetWord, item);
            }
            return true;
        }
        public bool TryAddTranslation(string targetWord, string translation)
        {
            if (string.IsNullOrEmpty(targetWord))
            {
                throw new System.ArgumentException($"{nameof(targetWord)}");
            }

            if (string.IsNullOrEmpty(translation))
            {
                throw new System.ArgumentException($"{nameof(translation)}");
            }
            List<string> tmp = Translation_items.GetValueOrDefault(targetWord);
            if (tmp != null)
            {
                if (!tmp.Contains(translation))
                {
                    tmp.Add(translation);
                }
            }
            else
            {
                tmp = new List<string>();
                tmp.Add(translation);

            }

            return Translation_items.TryAdd(targetWord, tmp);
        }
       
        public bool TryAddTargetWord(string targetWord)
        {
            if (string.IsNullOrEmpty(targetWord))
            {
                throw new System.ArgumentException($"{nameof(targetWord)}");
            }

            return Translation_items.TryAdd(targetWord, null);
        }
        public bool TryReplaceTargetWord(string replaceableTargetWord, string targetWordReplacement)
        {
            if (string.IsNullOrEmpty(replaceableTargetWord))
            {
                throw new ArgumentException($"\"{nameof(replaceableTargetWord)}\" не может быть неопределенным или пустым.", nameof(replaceableTargetWord));
            }

            if (string.IsNullOrEmpty(targetWordReplacement))
            {
                throw new ArgumentException($"\"{nameof(targetWordReplacement)}\" не может быть неопределенным или пустым.", nameof(targetWordReplacement));
            }

            if (Translation_items.Count == 0) return false;
            var item = Translation_items.Where(x => x.Key == replaceableTargetWord).First().Value;
            Translation_items.Remove(replaceableTargetWord);
            Translation_items.Add(targetWordReplacement, item);
            return true;
        }
        public bool TryReplaceTranslation(string targetWord, string replaceableTranslation, string translationReplacement)
        {
            if (string.IsNullOrEmpty(targetWord))
            {
                throw new ArgumentException($"{nameof(targetWord)}");
            }

            if (string.IsNullOrEmpty(replaceableTranslation))
            {
                throw new ArgumentException($"{nameof(replaceableTranslation)}");
            }

            if (string.IsNullOrEmpty(translationReplacement))
            {
                throw new ArgumentException($"{nameof(translationReplacement)}");
            }
            if (Translation_items.Count == 0) return false;
            if (!Translation_items.ContainsKey(targetWord)) return false;

            var translations = Translation_items.Where(x => x.Key == targetWord)
                .First()
                .Value;
            if (translations == null) return false;
            if (!translations.Contains(replaceableTranslation)) return false;
            translations.Remove(replaceableTranslation);
            translations.Add(translationReplacement);
            return true;
        }
        public bool TryRemoveTargetWord(string targetWord)
        {
            if (string.IsNullOrEmpty(targetWord))
            {
                throw new ArgumentException($"\"{nameof(targetWord)}\" не может быть неопределенным или пустым.", nameof(targetWord));
            }
            if (Translation_items.Count == 0) return false;
            if (!Translation_items.ContainsKey(targetWord)) return false;
            return Translation_items.Remove(targetWord);
        }
        public bool TryRemoveTranslate(string targetWord, string translation)
        {
            if (string.IsNullOrEmpty(targetWord))
            {
                throw new ArgumentException($"\"{nameof(targetWord)}\" не может быть неопределенным или пустым.", nameof(targetWord));
            }
            if (string.IsNullOrEmpty(translation))
            {
                throw new ArgumentException($"\"{nameof(translation)}\" не может быть неопределенным или пустым.", nameof(translation));
            }
            if (!Translation_items.ContainsKey(targetWord)) return false;
            if (!Translation_items[targetWord].Contains(translation)) return false;
            if (Translation_items[targetWord].Count == 1) return false;
            return Translation_items[targetWord].Remove(translation);
        }
        public new string ToString()
        {
            string result = $"{Type.ToString}";
            foreach (var item in Translation_items)
            {
                item.Value.Sort();
                result += $"\t\"{item.Key}\" =>  [\"{ string.Join("\", \"", item.Value) }\"]\n";
            }
            result += "";
            return result;
        }

    }
}