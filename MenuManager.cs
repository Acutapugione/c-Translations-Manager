using System;
using System.Linq;
using System.Threading.Tasks;
using TranslationReferenceBooks;

namespace MenuManagerModule
{

    public enum MainMenuPoints
    {
        Load,
        Save,
        Edit,
        Print,
        Exit
    }
    public enum MenuPoints
    {
        Add,
        Edit,
        Remove,
        Print,
        Exit
    }
    public enum TranslationMenuPoints
    {
        Add_Translation,
        Add_TargetWord,
        Replace_Target_Word,
        Replace_Translation,
        Remove_Target_Word,
        Remove_Translate,
        Print,
        Exit
    }
    public static class MenuManager
    {
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        public static string AskUserStrInput(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }
        public static TEnum AskUserDesision<TEnum>(
            string question, TEnum defaultVal, string tittle = "", int intendations = 0, ConsoleColor ForegroundColor = ConsoleColor.White) where TEnum : Enum
        {
            Console.ForegroundColor = ForegroundColor;

            Console.WriteLine(new string('\t', intendations) + tittle);
            Console.WriteLine(new string('\t', intendations) + question);
            string[] separated;
            uint cntr = 0;
            foreach (var item in (TEnum[])Enum.GetValues(typeof(TEnum)))
            {
                separated = new string($"{item}").Split('_');
                if (separated.Length > 1)
                {
                    Console.WriteLine($"{new string('\t', intendations)}{cntr}. { string.Join('\0', separated) }");
                } 
                else
                {
                    Console.WriteLine($"{new string('\t', intendations)}{cntr}. { item }");
                }
                cntr++;
            }

            if (!Enum.TryParse(typeof(TEnum), Console.ReadLine(), true, out object enumVal))
            {
                return defaultVal;
            }
            return (TEnum)enumVal;
        }
        private static TranslationDict GetTranslationDictionary()
        {
            if (TranslationDictManager.TranslationDicts.Count == 0)
                CreateTranslationDictionary();
            foreach (var item in TranslationDictManager.TranslationDicts)
            {
                Console.WriteLine(item.ToString());
            }
            return TranslationDictManager.Find( AskUserStrInput("Enter the target language"), AskUserStrInput("Enter the translate language"));
        }
        private static void CreateTranslationDictionary()
        {
            TranslationDictManager.AddTranslationDict(AskUserStrInput("Enter the target language"), AskUserStrInput("Enter the translate language"));
        }
        public static async Task MainMenuInit()
        {
            while (await MenuHandle(AskUserDesision("Enter main menu point:", MainMenuPoints.Exit, "Main menu", ForegroundColor: ConsoleColor.Blue)))
            {
            }
        }
        public static async void MenuInit()
        {            
            while (await MenuHandle(AskUserDesision("Enter menu point:", MenuPoints.Exit, "Menu", 1, ForegroundColor: ConsoleColor.Cyan)))
            {
            }
        }
        public static async Task TransationMenuInit()
        {
            var current = GetTranslationDictionary();
            if (current == null)
                return;
            while (await MenuHandle(AskUserDesision("Enter translation menu point:", TranslationMenuPoints.Exit, "Translation menu", 2, ForegroundColor: ConsoleColor.Magenta), current))
            {

            }
        }
#nullable enable
        public static async Task<bool> MenuHandle<TEnum>(TEnum point, TranslationDict? current = null)
        {
            if (typeof(TEnum) == typeof(MainMenuPoints))
            {
                switch (point)
                {
                    case MainMenuPoints.Load:
                        Console.Clear();
                        await LoadData();
                        break;
                    case MainMenuPoints.Save:
                        Console.Clear();
                        await SaveData();
                        break;
                    case MainMenuPoints.Edit:
                        Console.Clear();
                        MenuInit();
                        break;
                    case MainMenuPoints.Print:
                        Console.Clear();
                        Print<object>();
                        break;
                    case MainMenuPoints.Exit:
                        Console.Clear();
                        return false;
                    default:
                        break;
                }
            }
            else if (typeof(TEnum) == typeof(MenuPoints))
            {
                switch (point)
                {
                    case MenuPoints.Add:
                        Console.Clear();
                        Add();
                        break;
                    case MenuPoints.Edit:
                        Console.Clear();
                        await TransationMenuInit();
                        break;
                    case MenuPoints.Remove:
                        Console.Clear();
                        Remove();
                        break;
                    case MenuPoints.Exit:
                        Console.Clear();
                        return false;
                    case MenuPoints.Print:
                        Console.Clear();
                        Print<object>();
                        break;
                    default:
                        break;
                }
            }
            else if (typeof(TEnum) == typeof(TranslationMenuPoints))
            {
                switch (point)
                {
                    case TranslationMenuPoints.Add_Translation:
                        AddTranslation(current) ;
                        break;
                    case TranslationMenuPoints.Add_TargetWord:
                        AddTargetWord(current);
                        break;
                    case TranslationMenuPoints.Replace_Target_Word:
                        ReplaceTargetWord(current);
                        break;
                    case TranslationMenuPoints.Replace_Translation:
                        ReplaceTranslation(current);
                        break;
                    case TranslationMenuPoints.Remove_Target_Word:
                        RemoveTargetWord(current);
                        break;
                    case TranslationMenuPoints.Remove_Translate:
                        RemoveTranslate(current);
                        break;
                    case TranslationMenuPoints.Print:
                        Print<TranslationDict>(current);
                        break;
                    case TranslationMenuPoints.Exit:
                        Console.Clear();
                        return false;
                    default:
                        break;
                }
            }
            return true;
        }
#nullable disable
        private static async Task LoadData()
        {
            string fileName = @$"{ AskUserStrInput("Enter the file path:") }";

            if (!string.IsNullOrEmpty(fileName))
                TranslationFileManager.filename = fileName;
            TranslationDictManager.TranslationDicts = await TranslationFileManager.LoadAsync();
        }
        private static async Task SaveData()
        {
            string fileName = @$"{ AskUserStrInput("Enter the file path:") }";

            if (!string.IsNullOrEmpty(fileName))
                TranslationFileManager.filename = fileName;
            await TranslationFileManager.SaveAsync();
        }
        private static void Remove()
        {
            TranslationDictManager.RemoveTranslationDict(new(AskUserStrInput("Enter the target language"), AskUserStrInput("Enter the translate language")));
        }
        private static void Add()
        {
            TranslationDict translationDict = new TranslationDict(
                new(),
                type: new(
                    AskUserStrInput("Enter the target language:"),
                    AskUserStrInput("Enter the translation language:")
                    )
                );
            TranslationDictManager.AddTranslationDict(translationDict);
        }
        private static void Print<T>(params TranslationDict[] items)
        {
             if (typeof(T) == typeof(TranslationDict))
            {
                foreach (var item in items)
                {
                    Console.WriteLine(item.ToString());
                }
            } else
            {
                Console.WriteLine(TranslationDictManager.ToString());
            }
        }
        private static bool AddTranslation(TranslationDict current) => current.TryAddTranslation(
            AskUserStrInput("Enter the target word"),
            AskUserStrInput("Enter the translation")
            );
        private static bool RemoveTranslate(TranslationDict current) => current.TryRemoveTranslate(
            AskUserStrInput("Enter the target word"),
            AskUserStrInput("Enter the translate")
            );
        private static bool RemoveTargetWord(TranslationDict current) => current.TryRemoveTargetWord(
            AskUserStrInput("Enter the target word to remove")
            );
        private static bool ReplaceTranslation(TranslationDict current) => current.TryReplaceTranslation
            (AskUserStrInput("Enter the target word"), 
            AskUserStrInput("Enter the replaceable translation"),
            AskUserStrInput("Enter the translation replacement")
            );
        private static bool ReplaceTargetWord(TranslationDict current) => 
            current.TryReplaceTargetWord(
                AskUserStrInput("Enter the replaceable target word"),
                AskUserStrInput("Enter the target word replacement")
                );
        private static bool AddTargetWord(TranslationDict current)  => 
            current.TryAddTargetWord(
                AskUserStrInput("Enter the target word")
                );
    }
}
