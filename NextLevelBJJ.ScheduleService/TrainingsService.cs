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
        private Dictionary<DayOfWeek, string> _daySiteIdDictionaryRoomA;
        private Dictionary<DayOfWeek, string> _daySiteIdDictionaryRoomB;
        private IWebHtmlLoadHelper _webHtmlLoadHelper;
        private string _url;
        
        public TrainingsService()
        {
            _daySiteIdDictionaryRoomA = new Dictionary<DayOfWeek, string>()
            {
                { DayOfWeek.Monday, "comp-jwnx1wkw" },
                { DayOfWeek.Tuesday, "comp-jxboh9f7" },
                { DayOfWeek.Wednesday, "comp-jxboha4c" },
                { DayOfWeek.Thursday, "comp-jxbohd96" },
                { DayOfWeek.Friday, "comp-jxbohdrr" },
                { DayOfWeek.Saturday, "comp-jxbohcow" },
            };

            _daySiteIdDictionaryRoomB = new Dictionary<DayOfWeek, string>()
            {
                { DayOfWeek.Monday, "comp-jy74ah77" },
                { DayOfWeek.Tuesday, "comp-jy74ah8t" },
                { DayOfWeek.Wednesday, "comp-jy74ahdf" },
                { DayOfWeek.Thursday, "comp-jy74ahad" },
                { DayOfWeek.Friday, "comp-jy74ahbw" },
            };

            _webHtmlLoadHelper = new WebHtmlLoadHelper();
            _url = @"https://www.akademianextlevel.com/grafik";
        }

        public TrainingsService(Dictionary<DayOfWeek, string> daySiteIdDictionary, IWebHtmlLoadHelper webHtmlLoadHelper, string url)
        {
            _daySiteIdDictionaryRoomA = daySiteIdDictionary;
            _webHtmlLoadHelper = webHtmlLoadHelper;
            _url = url;
        }

        public TrainingDay GetTrainingDay(DayOfWeek dayOfWeek)
        {
            MatchCollection roomATrainings = null, roomBTrainings = null;
            IEnumerable<Class> classes = new List<Class>();
            List<Class> roomAClasses = new List<Class>(), roomBClasses = new List<Class>();
            if (_daySiteIdDictionaryRoomA.ContainsKey(dayOfWeek))
            {
                roomATrainings = GetTrainingsFromWebsite(_daySiteIdDictionaryRoomA[dayOfWeek]);
                roomAClasses = MapMatchesToClasses(roomATrainings, dayOfWeek, "SALA A").ToList();
            }

            if (_daySiteIdDictionaryRoomB.ContainsKey(dayOfWeek))
            {
                roomBTrainings = GetTrainingsFromWebsite(_daySiteIdDictionaryRoomB[dayOfWeek]);
                roomBClasses = MapMatchesToClasses(roomBTrainings, dayOfWeek, "SALA B").ToList();
            }


            roomAClasses.AddRange(roomBClasses);

            return new TrainingDay
            {
                Day = dayOfWeek,
                Classes = roomAClasses
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

        private MatchCollection GetTrainingsFromWebsite(string selector)
        {
            var xPathSelector = @"//*[@id='" + selector + "']";
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

            return regex.Matches(trainingDayText);
        }
        
        private IEnumerable<Class> MapMatchesToClasses(MatchCollection collection, DayOfWeek day, string room)
        => collection.Select(m =>
                {
                    return new Class
                    {
                        Day = day,
                        Name = m.Groups[3].Value,
                        StartHour = TimeSpan.ParseExact(m.Groups[1].Value, "hh\\.mm", CultureInfo.CurrentCulture),
                        FinishHour = TimeSpan.ParseExact(m.Groups[2].Value, "hh\\.mm", CultureInfo.CurrentCulture),
                        Room = room
                    };
                }).ToList();
    }
}
