using NextLevelBJJ.WebContentServices.Models;
using NextLevelBJJ.WebContentServices.Abstraction;
using NextLevelBJJ.WebContentServices.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NextLevelBJJ.WebContentServices
{
    public class TrainingsService : ITrainingsService
    {
        private Dictionary<DayOfWeek, string> _daySiteIdDictionary;
        private IWebHtmlLoadHelper _webHtmlLoadHelper;
        private string _url;
        
        public TrainingsService()
        {
            _daySiteIdDictionary = new Dictionary<DayOfWeek, string>()
            {
                { DayOfWeek.Monday, "comp-jwnx1wkw" },
                { DayOfWeek.Tuesday, "comp-jxboh9f7" },
                { DayOfWeek.Wednesday, "comp-jxboha4c" },
                { DayOfWeek.Thursday, "comp-jxbohd96" },
                { DayOfWeek.Friday, "comp-jxbohdrr" },
                { DayOfWeek.Saturday, "comp-jxbohcow" },
            };
            _webHtmlLoadHelper = new WebHtmlLoadHelper();
            _url = @"https://www.akademianextlevel.com/grafik";
        }

        public TrainingsService(Dictionary<DayOfWeek, string> daySiteIdDictionary, IWebHtmlLoadHelper webHtmlLoadHelper, string url)
        {
            _daySiteIdDictionary = daySiteIdDictionary;
            _webHtmlLoadHelper = webHtmlLoadHelper;
            _url = url;
        }

        public TrainingDay GetTrainingDay(DayOfWeek dayOfWeek)
        {
            if (!_daySiteIdDictionary.ContainsKey(dayOfWeek))
            {
                return new TrainingDay
                {
                    Day = dayOfWeek,
                    Classes = null
                };
            }

            var xPathSelector = @"//*[@id='" + _daySiteIdDictionary[dayOfWeek] + "']";
            var expression = @"(\d{2}.\d{2}) - (\d{2}.\d{2})[\s]{0,}(.*)";

            string trainingDayText = "";
            var htmlDocument = _webHtmlLoadHelper.LoadContentFromUrl(_url);

            try
            {
                trainingDayText = htmlDocument.DocumentNode
                .SelectSingleNode(xPathSelector)
                .InnerText
                .Replace("&nbsp;", " ")
                .Replace("&amp;", "")
                .Replace("&Oacute;", "Ó");
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas przetwarzania grafiku ze strony internetowej. Dodatkowa informacja: " + ex.Message);
            }

            var regex = new Regex(expression);

            var textGropus = regex.Matches(trainingDayText);

            return new TrainingDay
            {
                Day = dayOfWeek,
                Classes = MapMatchesToClasses(textGropus, dayOfWeek)
            };
        }

        public List<TrainingDay> GetTrainingWeek()
        {
            List<TrainingDay> week = new List<TrainingDay>();

            foreach(DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                var trainingDay = GetTrainingDay(day);

                if(trainingDay != null)
                {
                    week.Add(trainingDay);
                }
            }

            return week;
        }
        
        private IEnumerable<Class> MapMatchesToClasses(MatchCollection collection, DayOfWeek day)
        => collection.Select(m =>
                {
                    return new Class
                    {
                        Day = day,
                        Name = m.Groups[3].Value,
                        StartHour = TimeSpan.ParseExact(m.Groups[1].Value, "hh\\.mm", CultureInfo.CurrentCulture),
                        FinishHour = TimeSpan.ParseExact(m.Groups[2].Value, "hh\\.mm", CultureInfo.CurrentCulture)
                    };
                }).ToList();
    }
}
