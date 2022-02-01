using System.Collections.Generic;

namespace TranslationReferenceBooks
{
    public interface ITranslationDict
    {
        Dictionary<string, List<string>> Translation_items { get; set; }
        TranslationType Type { get; set; }
        
        string ToString();
        bool TryAddTargetWord(string targetWord);
        bool TryAddTranslation(string targetWord, string translation);
        bool TryAddTranslation(string targetWord, IEnumerable<string> translations);
        bool TryAddTranslation(string targetWord, params string[] translations);
        bool TryAddTranslationItem(Dictionary<string, List<string>> translation_item);
        bool TryAddTranslationItem(TranslationDict translation_item);
        bool TryRemoveTargetWord(string targetWord);
        bool TryRemoveTranslate(string targetWord, string translation);
        bool TryReplaceTargetWord(string replaceableTargetWord, string targetWordReplacement);
        bool TryReplaceTranslation(string targetWord, string replaceableTranslation, string translationReplacement);
    }
}