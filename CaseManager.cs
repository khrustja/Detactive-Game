using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;

namespace DetectiveGame
{
	class CaseManager
	{
		private List<Location> locations = new List<Location>();
		private List<Suspect> suspects = new List<Suspect>();
		private List<Evidence> evidences = new List<Evidence>();
		private List<string> journal = new List<string>();
		private List<string> inventory = new List<string>();
		private List<string> labReports = new List<string>();

		private bool caseSolved = false;
		private int score = 0;
		private int timeLeft = 30;
		private PlayerSkills skills = new PlayerSkills();
		private Random rnd = new Random();
		private Suspect killer;

		private List<Case> cases = new List<Case>();
		private Location currentLocation;

		public void InitializeCases()
		{
			cases.Add(new Case("i��� ��������", new List<Suspect>
	  {
		new Suspect("����", new List<string>
		{
		  "� ���� � i����� � ������i� ���i�. �� �������� ��� ���� �������.",
		  "� �� ���� �������� ��� ��, ��� �� ���� ���� ����i���.",
		  "�i� ��i��� ���i, �� �� �������� ���� ����. � �� ���� ���i����, �� �� �������.",
		  "� ����, �� �i� ������� �� ��������, ��� �� �����, � ���."
		}),
		new Suspect("�����", new List<string>
		{
		  "���� �� ���� �i��������? � �i���� �� ����! � ��� �����, ���� �� �������.",
		  "� �� ��� ������ ������� � i�����. �� ������� �� �i �����.",
		  "� ��� �����i�, ��� �����, �� �� ���������, ���� ���� �����.",
		  "�� ���i�� ������ �i���������, �� � ��� �����. �����i��� ��!"
		}),
		new Suspect("����i�", new List<string>
		{
		  "� �� ��� ������� �����i��� � i�����. �i� ��� ������� ������.",
		  "� ��� �����i�, ��� �� ����� �i����. � �����, �� �� ����� ����������.",
		  "�� ����� ��������� ��� ��������, ��� �� �� ������, �� � ���� ��� ���� �����.",
		  "� ���� ������ ��� ������, �� � ��� �� �����i�i � ��� ���i�."
		}),
		new Suspect("�����", new List<string>
		{
		  "�� �����i������ � ����, ��� �� �� ������ �i����. � ����� ����, ��� �i� �� �i����i��� �� �� ���i���.",
		  "� ���� ������� �� �����i, � ���� ���� ����i���i ������.",
		  "� �� �����, �� � ����� ���� ��������. �i� �i���� �� ������� ��� ��.",
		  "�i� ���������� ���� �� �����i�, ��� � �� ������ ������."
		}),
		new Suspect("������", new List<string>
		{
		  "� �� ����, ��� �� �i� �������. � ��� � ��������i � �� �i�.",
		  "� ���� ���� ��i���, ��i ������ �i��������� �� ��i�i.",
		  "� ��� ��� ���� ��������, ��� �i���� �� �����, �� �� ���� ��������� �� ������.",
		  "� ����, �� � ����� ���� �i������i �������i, ��� �� �i����."
		})
	  }, new List<string> { "�i������", "�����", "�������", "��i�", "�i����" }));
			cases.Add(new Case("������ ���������", new List<Suspect>
	  {
		new Suspect("�����", new List<string>
		{
		  "� �� ����� ������ � ��� ���i�.",
		  "� ��� �� ���i��i, �� ��������� ����!",
		  "����� ������, � �� ���� ����� �� ����.",
		  "���i��� ���� � ���� ����� �� �i����, ��� ���� ������ �����, � �� ���'���� ��i�.",
		  "�i, � �� ���i��� �i���� ������� ���� ������.",
		  "������ �������� � ����-���� �� ���i��i, � ���� ��� ��� ���."
		}),
		new Suspect("�������", new List<string>
		{
		  "�� �� ���� ��������� �������, ��� �� �� ������, �� � ���� ����.",
		  "� �������, ��� �i����i���� �� ���i �������.",
		  "������i� ����� �� ����� �� ��i���������.",
		  "� ���� ���� ��� ������, ���i �� �� ������.",
		  "� �� ����, �� ����� �������.",
		  "�����i���� �� �� ������� ����i�, ����, �������, ������ �i����."
		}),
		new Suspect("�i����", new List<string>
		{
		  "� �� ����, ���� � ���.",
		  "� ������ ������, ���� �� �������.",
		  "������ ���� ���� ������, � �i���� �� ��� i �� �����.",
		  "� ������i �i��� ����������� � �������.",
		  "�� �����i�, �� � ���� ���� ���������.",
		  "������ ������� ���� � �����, � �i���� �� ����."
		}),
		new Suspect("����", new List<string>
		{
		  "�� ������ ������i, � �� ���� �i���� �������.",
		  "�� �� ��� ������!",
		  "� ������ �� �i���� ���i�, ��� �� �i���� �� ���������� �� �����.",
		  "� ���� ���� ������� �������.",
		  "�� �����, �� ���� ��� ������ ���������.",
		  "����� ��������� ����� �� ��������� ��������."
		}),
		new Suspect("�����", new List<string>
		{
		  "� ��� �� �����i�i � ��������, �� �� �i� �����.",
		  "���i�� �� ���� ��������?",
		  "� ��� ���� ������� ������ �� �i������ ������, � ��i���.",
		  "� �i���� �� ��� ������ �����i��i� � �������.",
		  "���i ��������, �� ���� �i�������� � ������.",
		  "�� ������ ������ �i��������� �� ��i�i."
		})
	  }, new List<string> { "��������", "����� �����", "��������", "��i�", "�����" }));

			cases.Add(new Case("����� ������", new List<Suspect>
	  {
		new Suspect("i���", new List<string>
		{
		  "� �� ����, �� �������.",
		  "� ��� �� ����i��i, ���� �� ���� ���������.",
		  "���� ��� ���� ��������, � �� �i������ �i� ������.",
		  "� �� ����� ����� ������i� �����.",
		  "�� ��� ������� ��������, ��� �i� �� �������.",
		  "������ �����i���� ������ � ����� �� ����i��i, � ��� ��� �i� ������� �� �i���."
		}),
		new Suspect("��i�����", new List<string>
		{
		  "� �� ���� � ��� ������, � �������.",
		  "�� �� �������, � ���� � i����� �i��i.",
		  "� ���� � �i��������i � ������i, � ������ �� �i����������� � ������.",
		  "�����? �� �� ��i��������� ��� �i���� �i���i�.",
		  "� ���� ��� �������, ���i �� �� ����� �����.",
		  "�����i���� �� ���� ����� ��� �����i�, ����, ����i���, ������ �i����."
		}),
		new Suspect("�����", new List<string>
		{
		  "� �� ���� ���������, � �i���� �� ����.",
		  "�� �� ��� ��������.",
		  "� ������i �� ���� ����� �������.",
		  "�� ���� �i������ i ���.",
		  "�� �����i�, ���� �� ���� ������.",
		  "� ��� ��� �����i ������, �� �������� ���� � ��."
		}),
		new Suspect("������", new List<string>
		{
		  "� ���� � ����i, �� ���, �� � ����.",
		  "�� ��������� ���� ��� �� �i����.",
		  "� ����i ���� ����� ������ i ������ �����, � �i���� �� ���i����.",
		  "� ��������� i ����������� � �������.",
		  "����� ��� �� ������, ��������i, �� ���'����.",
		  "������� ���� � �����, � ���� ������ ��� ���i�."
		}),
		new Suspect("��i�", new List<string>
		{
		  "� ���� ������� � ��� ���i�, i ���i �� ����i��i �i �������.",
		  "� ����� ��������, � �� ���� �i����.",
		  "� ���� ���� ������� �����i�, ��� ����������� ���i���.",
		  "� �� ������������ � ������ ���� ���.",
		  "���i �������� �i����i���� �� �i ���������, � �i���� �� ����.",
		  "���� �����, �����i���� �� ������ i�����, ��� �i� ���� ������."
		})
	  }, new List<string> { "��i�", "����", "����������", "������ �������", "�������� �i�����" }));
            var selectedCase = cases[rnd.Next(cases.Count)];
            Console.WriteLine($"������ ���������. ������: {selectedCase.Victim}, �������� � ������i� �i����i.");
            Console.WriteLine("�i��������i: " + string.Join(", ", selectedCase.Suspects.Select(s => s.Name)) + ".");
            Console.WriteLine();

            foreach (var locationName in selectedCase.Locations)
            {
                locations.Add(new Location(locationName));
            }

            suspects = selectedCase.Suspects;
            killer = suspects[rnd.Next(suspects.Count)];
            Console.WriteLine($"(��������: ��������� ������� ������ � {killer.Name})");
        }
        public void MainLoop()
        {
            while (!caseSolved && timeLeft > 0)
            {
                Console.WriteLine($"���� ����������: {timeLeft} ���i�");

                if (currentLocation == null)
                {
                    Console.WriteLine("����i�� �����i�:");
                    for (int i = 0; i < locations.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {locations[i].Name}");
                    }

                    if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= locations.Count)
                    {
                        currentLocation = locations[index - 1];
                        Console.WriteLine($"�� ������ �����i�: {currentLocation.Name}");
                        currentLocation.Describe();
                    }
                    else
                    {
                        Console.WriteLine("���i���� ���i�.\n");
                        continue;
                    }
                }

                Console.WriteLine("1. ����i���� �����i�");
                Console.WriteLine("2. �������� �i�����������");
                Console.WriteLine("3. �������i������ ������");
                Console.WriteLine("4. ����������� �i�����������");
                Console.WriteLine("5. �����i���� ��i���");
                Console.WriteLine("6. ����������� ������ �����i�");
                Console.WriteLine("7. ����������� ������ ���i�");
                Console.WriteLine("8. �������� �i������ (-5 ���i�)");
                Console.WriteLine("9. �����i���� i�������");
                Console.WriteLine("10. ��������� �������");
                Console.WriteLine("11. ����� ������ � ���������i�");
                Console.WriteLine("12. ����� � ���");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ExploreCurrentLocation();
                        timeLeft--;
                        break;
                    case "2":
                        InterrogateSuspect();
                        timeLeft--;
                        break;
                    case "3":
                        AnalyzeEvidence();
                        timeLeft--;
                        break;
                    case "4":
                        AccuseSuspect();
                        timeLeft--;
                        break;
                    case "5":
                        Console.WriteLine($"��� �������� �������: {score} ���i�.\n");
                        break;
                    case "6":
                        ShowEvidence();
                        timeLeft--;
                        break;
                    case "7":
                        ShowJournal();
                        timeLeft--;
                        break;
                    case "8":
                        GiveHint();
                        break;
                    case "9":
                        ShowInventory();
                        timeLeft--;
                        break;
                    case "10":
                        UpgradeSkills();
                        timeLeft--;
                        break;
                    case "11":
                        SubmitSampleToLab();
                        timeLeft--;
                        break;
                    case "12":
                        Console.WriteLine("��� ���������.");
                        return;
                    default:
                        Console.WriteLine("���i���� ���i�. ��������� �� ���.");
                        break;
                }

                if (timeLeft == 0)
                {
                    Console.WriteLine("��� ���������! ������ �� ��������.");
                }
            }
        }
		private void ShowInventory()
		{
			Console.WriteLine("��� i�������:");
			if (inventory.Count == 0)
			{
				Console.WriteLine("i������� ������i�.");
			}
			else
			{
				foreach (var item in inventory.Distinct())
				{
					Console.WriteLine("- " + item);
				}
			}
			Console.WriteLine();
		}

		private void ShowJournal()
		{
			Console.WriteLine("������ ���i�:");
			if (journal.Count == 0)
			{
				Console.WriteLine("������ ������i�.");
			}
			else
			{
				foreach (var entry in journal)
				{
					Console.WriteLine("- " + entry);
				}
			}
			Console.WriteLine();
		}

		private void ShowEvidence()
		{
			Console.WriteLine("�������i ������:");
			if (evidences.Count == 0)
			{
				Console.WriteLine("������ �� �� �������i.");
			}
			else
			{
				foreach (var ev in evidences.Distinct())
				{
					Console.WriteLine("- " + ev.Description);
				}
			}
			Console.WriteLine();
		}

		private void AccuseSuspect()
		{
			Console.WriteLine("���� �� ������ �����������?");
			for (int i = 0; i < suspects.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {suspects[i].Name}");
			}

			if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= suspects.Count)
			{
				Console.WriteLine();

				if (suspects[index - 1] == killer)
				{
					Console.WriteLine($"�� ��������� �������� ������! ������ � {killer.Name}.");
					score += 20;
				}
				else
				{
					Console.WriteLine($"���i���� ���i�. �������i� ��������� � {killer.Name}.");
					score -= 10;
				}

				journal.Add($"�����������: {suspects[index - 1].Name}");
				Console.WriteLine($"��� �i������� �������: {score} ���i�.\n");
				caseSolved = true;
			}
			else
			{
				Console.WriteLine("���i���� ���i�.\n");
			}
		}

		private void AnalyzeEvidence()
		{
			Console.WriteLine("����i� �����i�...");
			int analysisTime = 3000 - (skills.AnalysisSpeed * 500);
			Thread.Sleep(Math.Max(1000, analysisTime));

			Console.WriteLine("������� ����i� ����i� i ���������, �� �������� ������� ��������� � 22:30.");
			journal.Add("��������� ����i� �����i�");
			score += 2;

			Console.WriteLine("�i������: ������� ��'���� �i� �������� �� �i�����������.");
		}

		private void InterrogateSuspect()
		{
			Console.WriteLine("����i�� �i�����������:");
			for (int i = 0; i < suspects.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {suspects[i].Name}");
			}

			if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= suspects.Count)
			{
				Console.WriteLine();
				suspects[index - 1].Interrogate(skills.DetectLies);
				journal.Add($"�������� �i�����������: {suspects[index - 1].Name}");
				score += 3;

				int timeCost = 1 - (skills.FastInterrogation);
				timeLeft -= Math.Max(0, timeCost);

				Console.WriteLine("�i������: �����i�� ����� �� �����i � ������ �i����������.");
			}
			else
			{
				Console.WriteLine("���i���� ���i�.\n");
			}
		}