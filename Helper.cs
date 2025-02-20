﻿using System;
using System.Text;
using static language_cli.Consts;

namespace language_cli
{
    public static class Helper
    {
        public static string BuildUrl(string word, ApiType types = ApiType.TURENG)
        {
            switch (types)
            {
                case ApiType.DICTIONARY_API:
                    return Consts.DICTIONARY_API_URL + word;
                default:
                    return Consts.TURENG_URL + word;
            }
        } 

        public static void ParseWordListHtml(string html)
        {
            HtmlAgilityPack.HtmlDocument document = new();
            document.LoadHtml(html);

            var table = document.DocumentNode.SelectNodes(Consts.TABLE_SELECTOR);

            if (table is null)
            {
                Console.WriteLine("The word written is wrong.");
                //Console.WriteLine("The word written is wrong. Did you mean one of that below?");
                //TODO
                //site suggests some close words to what you write. i can give these as an option
            }
            else
            {
                WriteWordList(document);
            }
        }

        private static void WriteWordList(HtmlAgilityPack.HtmlDocument document)
        {
            Console.OutputEncoding = Encoding.UTF8;

            WriteFirstWord(document);

            int index = 2;

            for (int i = 5; i < Consts.COUNT_OF_WORD; i++)
            {
                index = WriteOtherWords(document, index, i);
            }
        }

        private static int WriteOtherWords(HtmlAgilityPack.HtmlDocument document, int index, int i)
        {
            string selector = $"{Consts.FIRST_PART}{i}{Consts.SECOND_PART}";
            var selectedHtml = document.DocumentNode.SelectSingleNode(selector);

            // reason of this condition is because of html desing of site
            // there are some empty <td>s for responsive design. 
            // so this is to skip them
            if (selectedHtml is not null)
            {
                string innerText = selectedHtml.InnerText;
                Console.WriteLine($"{index}. {innerText}", Console.OutputEncoding);
                index++;
            }

            return index;
        }

        private static void WriteFirstWord(HtmlAgilityPack.HtmlDocument document)
        {
            var firstSelectedHtml = document.DocumentNode.SelectSingleNode(Consts.FIRST_ROW);
            string text = firstSelectedHtml.InnerText;
            Console.WriteLine($"1. {text}", Console.OutputEncoding);
        }

        /// <summary>
        /// Just entrance logs.
        /// </summary>
        public static void Welcome()
        {
            Console.WriteLine(" * Welcome Translate Lover! *");
        }

        /// <summary>
        /// The method for combine group of words. 
        /// For example, make money
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string BuildWord(string[] words, ApiType apiType = ApiType.TURENG)
        {
            StringBuilder builder = new();
            string appender = apiType == ApiType.TURENG ? " " : "%20";
            builder.AppendJoin(appender, words);
            return builder.ToString();
        }
    }
}
