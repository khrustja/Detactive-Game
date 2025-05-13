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
                "�i������" => "�� ���������� � ������i� �i�����i, �� ������ ��������� ������ ����.",
                "�����" => "�����, �� �������� ���, ��� ��� ���� �����.",
                "�������" => "����� �������, �� ������ ���� ��i�� ��������.",
                "��i�" => "��i�, �� ������ ��������� ��� ��������� ��������.",
                "�i����" => "������ �i����, �� ������ ���� ��������i ������.",
                "��������" => "�������� ��������, ��� � �i� ���� ���������.",
                "����� �����" => "�i���, �� ������ ����� �����i���� �i����������.",
                "��������" => "��������, �� ������ ���� �����i�i.",
                "����������" => "������� �i���, �� ������ ���� ������i ���i.",
                "������ �������" => "������ �������, �� ����� ������� �����.",
                "�������� �i�����" => "����� �i�����, � ��i� ������ ���� ��������i �����.",
                _ => "�� ��������� �����i�."
            };

            Console.WriteLine($"���� �����i�: {description}");
        }

        public string Explore(List<Evidence> evidences)
        {
            string[] findings = {
                "�������� ���� � ���������", "��i�� ������ �i�� �i���",
                "�������� ��������", "������� ��������",
                "���� � i�i�i�����", "��i�� ����� �� �i����i",
                "��������i� � �������", "����� � ������ �������",
                "���������� ���i����� �������", "����������� ����'����",
                "������� � �����������", "����i��� ��������",
                "��i�� �i� ������� ����i", "����������� �������, ���� �� ������� ���� ���"
            };
            string found = findings[rnd.Next(findings.Length)];
            Console.WriteLine($"{found}.");
            evidences.Add(new Evidence(found));
            return found;
        }

        public string FindItem()
        {
            string[] items = {
                "�i������", "��������� ���", "�����i ���������",
                "������ �i�����i�", "������� ��� ����i�", "����� �����i�",
                "������ �������", "������� �����", "�������",
                "�������� � ���������", "�i����� � ����������",
                "���� � �i����������", "���������� ������ � �i��������",
                "������ ��i��", "������ � ������",
                "�i����i��� �����", "������ ������� � ���i����������",
                "������� � �������"
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

            Console.WriteLine($"{Name} ����: \"{answer}\"");

            if (detectLies > 0 && isLying)
            {
                Console.WriteLine("(��� ����� �i�����: �i���������� �����!)");
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