﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UlearnGame.Models;

namespace UlearnGame.Managers
{
    public class ScoreManager //логика обновление состояния счёта
    {
        private static string _fileName = "scores.xml";

        public List<Score> HighScores { get; private set; } //Рекорды

        public List<Score> Scores { get; private set; } //счёт

        public ScoreManager()
          : this(new List<Score>())
        {

        }

        public ScoreManager(List<Score> scores) //задаём счёт и обновляем
        {
            Scores = scores;

            UpdateHighscores();
        }

        public void Add(Score score) //добавляем счёт и обновляем рекорды
        {
            Scores.Add(score);

            Scores = Scores.OrderByDescending(c => c.Value).ToList();

            UpdateHighscores();
        }

        public static ScoreManager Load()
        {
            if (!File.Exists(_fileName))
                return new ScoreManager();

            using (var reader = new StreamReader(new FileStream(_fileName, FileMode.Open)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));

                var scores = (List<Score>)serializer.Deserialize(reader);

                return new ScoreManager(scores);
            }
        }

        public void UpdateHighscores() //обновляем рекорды
        {
            HighScores = Scores.Take(10).ToList();
        }

        public static void Save(ScoreManager scoreManager) //сохраняем счёт
        {
            using (var reader = new StreamWriter(new FileStream(_fileName, FileMode.Create)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));

                serializer.Serialize(reader, scoreManager.Scores);
            }
        }
    }
}
