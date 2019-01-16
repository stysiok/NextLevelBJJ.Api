using HtmlAgilityPack;
using NextLevelBJJ.ScheduleService.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NextLevelBJJ.ScheduleService
{
    public class TrainingsService
    {
        private Dictionary<DayOfWeek, string> _daySiteIdDictionary;

        private HtmlDocument _htmlDoc;

        public TrainingsService()
        {
            _htmlDoc = LoadHtmlSchedule();
            _daySiteIdDictionary = new Dictionary<DayOfWeek, string>()
            {
                { DayOfWeek.Monday, "comp-jlqn2pas" },
                { DayOfWeek.Tuesday, "comp-jlqn2pbf" },
                { DayOfWeek.Wednesday, "comp-jlqn2pc3" },
                { DayOfWeek.Thursday, "comp-jlqn2pcf" },
                { DayOfWeek.Friday, "comp-jlqn2pb3" },
                { DayOfWeek.Saturday, "comp-jlqn2pbr" },
            };
        }

        public TrainingDay GetTrainingDay(DayOfWeek dayOfWeek)
        {
            if (!_daySiteIdDictionary.ContainsKey(dayOfWeek))
            {
                return null;
            }

            var xPathSelector = @"//*[@id='" + _daySiteIdDictionary[dayOfWeek] + "']";
            var expression = @"(\d{2}:\d{2}) - (\d{2}:\d{2})\s{3}(.*)";

            var traingDayText = _htmlDoc.DocumentNode
                .SelectSingleNode(xPathSelector)
                .InnerText
                .Replace("&nbsp;", " ");

            var regex = new Regex(expression);

            var textGropus = regex.Matches(traingDayText);

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

        private HtmlDocument LoadHtmlSchedule()
        {
            var siteUrl = @"https://www.nextlevelbjj.pl/grafik";
            var web = new HtmlWeb();

            return web.Load(siteUrl);
        }

        private IEnumerable<Class> MapMatchesToClasses(MatchCollection collection, DayOfWeek day)
        => collection.Select(m =>
                {
                    return new Class
                    {
                        Day = day,
                        Name = m.Groups[3].Value,
                        StartHour = DateTime.ParseExact(m.Groups[1].Value, "HH:mm", CultureInfo.InvariantCulture),
                        FinishHour = DateTime.ParseExact(m.Groups[2].Value, "HH:mm", CultureInfo.InvariantCulture)
                    };
                }).ToList();
    }
}
