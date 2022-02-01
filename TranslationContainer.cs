using System;

namespace TranslationReferenceBooks
{
    public class TranslationContainer 
    {

        public TranslationDict TranslationDict
        {
            get
            {
                return translationDict;
            }

            set
            {
                translationDict = value;
            }
        }

        private TranslationDict translationDict;

        public TranslationContainer(string language_target, string language_from)
        {
            if (string.IsNullOrEmpty(language_target))
            {
                throw new ArgumentException($"{nameof(language_target)}");
            }

            if (string.IsNullOrEmpty(language_from))
            {
                throw new ArgumentException($"{nameof(language_from)}");
            }

            //translationType = new TranslationType();
            translationDict = new TranslationDict();
        }
        public TranslationContainer()
        {
            //translationType = new TranslationType("none", "none");
            translationDict = new TranslationDict();
        }
    }

}
