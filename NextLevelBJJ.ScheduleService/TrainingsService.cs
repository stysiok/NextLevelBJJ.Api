﻿using NextLevelBJJ.ScheduleService.Models;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NextLevelBJJ.ScheduleService
{
    public class TrainingsService : AbstractWebContent, ITrainingsService
    {
        private Dictionary<DayOfWeek, string> _daySiteIdDictionary;
        
        public TrainingsService() : base(@"https://www.nextlevelbjj.pl/grafik")
        {
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
                return new TrainingDay
                {
                    Day = dayOfWeek,
                    Classes = null
                };
            }

            var xPathSelector = @"//*[@id='" + _daySiteIdDictionary[dayOfWeek] + "']";
            var expression = @"(\d{2}:\d{2}) - (\d{2}:\d{2})[\s]{0,}(.*)";

            string trainingDayText = "";

            try
            {
                trainingDayText = HtmlDocument.DocumentNode
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
                        StartHour = TimeSpan.Parse(m.Groups[1].Value, CultureInfo.CurrentCulture),
                        FinishHour = TimeSpan.Parse(m.Groups[2].Value, CultureInfo.CurrentCulture)
                    };
                }).ToList();
    }
}