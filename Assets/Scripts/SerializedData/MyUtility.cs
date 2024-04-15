using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
// using UnityEngine.Localization.Settings;
using Random = UnityEngine.Random;
using System.IO;

namespace MyUtility
{
    static class Converter {
        const string format = "yyyy/MM/dd HH:mm:ss";
        private static System.IFormatProvider provider;
        private static CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        
        public static int StringToInt(string value) {
            int number;
            if (int.TryParse(value, out number))
                return number;
            else
                return 0;
        }

        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString(format);
        }

        public static DateTime StringToDateTime(string dateTimeString)
        {
            DateTime dateTime;
            bool success = DateTime.TryParseExact(dateTimeString, format, provider, DateTimeStyles.None, out dateTime);

            if (success) return dateTime;
            return DateTime.Now;
        }

        public static int[] List2Array(List<int> list)
        {
            int[] array = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                array[i] = list[i];
            }

            return array;
        }
        
        private static Dictionary<string, KeyValuePair<string, string>> koreanParticles = new Dictionary<string, KeyValuePair<string, string>>
        {
            { "을/를", new KeyValuePair<string, string>("을", "를") },
            { "이/가", new KeyValuePair<string, string>("이", "가") },
            { "은/는", new KeyValuePair<string, string>("은", "는") },
        };

        public static string KoreanParticle(string text)
        {
            foreach (var particle in koreanParticles)
            {
                var index = text.IndexOf(particle.Key) - 1;
                while (index >= 0)
                {
                    var word = (text[index] - 0xAC00) % 28 > 0 ? particle.Value.Key : particle.Value.Value;
                    text = text.Remove(index + 1, particle.Key.Length).Insert(index + 1, word);
                    index = text.IndexOf(particle.Key) - 1;
                }
            }
            return text;
        }

        public static string IntToCommaSeparatedString(int amt)
        {
            return string.Format("{0:#,###0}", amt);
        }
        
        public static int GetWeekOfYear(DateTime date)
        {
            return date.Year * 1000 + cultureInfo.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }

        public static int GetDayOfYear(DateTime date)
        {
            return date.Year * 1000 + date.DayOfYear;
        }
    }
    
    // static class Localize {
    //     public static string GetLocalizedString(string input)
    //     {
    //         if (input.Contains('[') && input.Contains(']'))
    //         {
    //             string[] sliced = input.Split('[', ']');
    //             string key = sliced[input.IndexOf('[') + 1];
    //             return LocalizationSettings.StringDatabase.GetLocalizedString("UI", key);
    //         }
    //         
    //         // Debug.Log("LocaleCodeNotFound for string : " + input);
    //         return input;
    //     }
    //     
    //     public static string GetLocalizedPetDialogue(string input)
    //     {
    //         if (input.Contains("<LOTTERY>"))
    //         {
    //             DateTime date =DateTime.Now;
    //             int weekOfMonth=(date.Day + ((int)date.DayOfWeek)) / 7 + 1;
    //             
    //             string lottery;
    //             int maxN = PlayerPrefs.GetString("language") == "ko" ? 46 : 70;
    //             
    //             float rnd = Random.Range(0f, 1f);
    //             if (rnd < 0.33f)
    //             {
    //                 Random.InitState(date.Year * date.Month + weekOfMonth);
    //                 lottery = Random.Range(1, maxN).ToString();
    //                 Random.InitState(date.Year * date.Month + weekOfMonth + 10);
    //                 lottery += " " + Random.Range(1, maxN);
    //             }
    //             else if(rnd < 0.66)
    //             {
    //                 Random.InitState(date.Year * date.Month + weekOfMonth + 11);
    //                 lottery = Random.Range(1, maxN).ToString();
    //                 Random.InitState(date.Year * date.Month + weekOfMonth + 12);
    //                 lottery += " " + Random.Range(1, maxN);
    //                 Random.InitState(date.Year * date.Month + weekOfMonth + 13);
    //                 lottery += " " + Random.Range(1, maxN);
    //             }
    //             else
    //             {
    //                 Random.InitState(date.Year * date.Month + weekOfMonth + 14);
    //                 lottery = Random.Range(1, 27).ToString();
    //             }
    //
    //             Random.InitState((int)(Time.time * 1000f));
    //             return lottery;
    //         }
    //         
    //         if (input.Contains('[') && input.Contains(']'))
    //         {
    //             string[] sliced = input.Split('[', ']');
    //             string key = sliced[input.IndexOf('[') + 1];
    //             return LocalizationSettings.StringDatabase.GetLocalizedString("PetDialogue", key);
    //         }
    //         
    //         // Debug.Log("LocaleCodeNotFound for string : " + input);
    //         return input;
    //     }
    // }

    static class IO {
        public static string GetJsom(string name)
        {
            return File.ReadAllText(Application.dataPath + "/Resources/JSON/"+ name +".json"); 
        }
    }
}