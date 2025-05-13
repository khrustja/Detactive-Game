using System;
using System.Collections.Generic;

namespace DetectiveGame
{
    class Location
    {
        public string Name { get; set; }
        private Random rnd = new Random();

        public Location(string name) => Name = name;

        public void Describe()
        {
            string description = Name switch
            {
                "Вiтальня" => "Ви перебуваєте в затишнiй вiтальнi, де жертва проводила багато часу.",
                "Кухня" => "Кухня, де готували їжу, але тут щось дивне.",
                "Спальня" => "Темна спальня, де можуть бути слiди боротьби.",
                "Офiс" => "Офiс, де жертва працювала над важливими справами.",
                "Пiдвал" => "Темний пiдвал, де можуть бути прихованi докази.",
                "Квартира" => "Звичайна квартира, але в нiй щось приховано.",
                "Салон краси" => "Мiсце, де жертва могла зустрiчати пiдозрюваних.",
                "Ресторан" => "Ресторан, де жертва мала зустрiчi.",
                "Гардеробна" => "Закрите мiсце, де можуть бути важливi речi.",
                "Темний коридор" => "Темний коридор, де могла статися атака.",
                "Секретна кiмната" => "Таємна кiмната, в якiй можуть бути прихованi факти.",
                _ => "Це незнайома локацiя."
            };

            Console.WriteLine($"Опис локацiї: {description}");
        }

        public string Explore(List<Evidence> evidences)
        {
            string[] findings = {
                "знайдено лист з погрозами", "слiди взуття бiля вiкна",
                "зламаний годинник", "розбите дзеркало",
                "ключ з iнiцiалами", "слiди бруду на пiдлозi",
                "фотографiя з жертвою", "книга з таємним записом",
                "викрадений мобiльний телефон", "пошкоджений комп'ютер",
                "згорток з наркотиками", "згорiлий документ",
                "слiди вiд крапель кровi", "незвичайний предмет, який не повинен бути тут"
            };
            string found = findings[rnd.Next(findings.Length)];
            Console.WriteLine($"{found}.");
            evidences.Add(new Evidence(found));
            return found;
        }

        public string FindItem()
        {
            string[] items = {
                "лiхтарик", "секретний код", "гумовi рукавички",
                "сканер вiдбиткiв", "шпилька для замкiв", "пакет доказiв",
                "картка доступу", "червона фарба", "ноутбук",
                "записник з нотатками", "дiамант з викрадення",
                "диск з вiдеозаписом", "пластикова пляшка з вiдбитками",
                "зразок шкiри", "флешка з даними",
                "пiдозрiлий запис", "старий телефон з повiдомленнями",
                "записка з адресою"
            };
            return items[rnd.Next(items.Length)];
        }
    }

    class Suspect
    {
        public string Name { get; set; }
        private List<string> phrases;
        private Random rnd = new Random();

        public Suspect(string name, List<string> phrases)
        {
            Name = name;
            this.phrases = phrases;
        }

        public void Interrogate(int detectLies)
        {
            bool isLying = rnd.Next(100) < 50;
            string answer = phrases[rnd.Next(phrases.Count)];

            Console.WriteLine($"{Name} каже: \"{answer}\"");

            if (detectLies > 0 && isLying)
            {
                Console.WriteLine("(Ваш навик пiдказує: пiдозрюваний бреше!)");
            }
            Console.WriteLine();
        }
    }

    class Evidence
    {
        public string Description { get; set; }
        public Evidence(string desc) => Description = desc;
    }

    class PlayerSkills
    {
        public int AnalysisSpeed { get; set; } = 0;
        public int DetectLies { get; set; } = 0;
        public int SearchSkill { get; set; } = 0;
        public int FastInterrogation { get; set; } = 0;
    }
    class Case
    {
        public string Victim { get; private set; }
        public List<Suspect> Suspects { get; private set; }
        public List<string> Locations { get; private set; }

        public Case(string victim, List<Suspect> suspects, List<string> locations)
        {
            Victim = victim;
            Suspects = suspects;
            Locations = locations;
        }
    }
}