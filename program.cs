using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DetectiveGame
{
	class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();
			game.Start();
		}
	}

	class Game
	{
		private CaseManager caseManager;

		public void Start()
		{
			Console.WriteLine("�i���� � ��i '��i����� ���� ��������'.");
			Console.WriteLine("�������� ����-��� ������, ��� ������...");
			Console.ReadKey();
			Console.WriteLine();

			caseManager = new CaseManager();
			caseManager.InitializeCases();
			caseManager.MainLoop();
		}
	}

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
			cases.Add(new Case("���� ��������", new List<Suspect>
			{
				new Suspect("����", new List<string>
				{
					"� ���� � ������ � ������� �����. �� �������� ��� ���� �������.",
					"� �� ���� �������� ��� ��, ��� �� ���� ���� ��������.",
					"³� ����� ���, �� �� �������� ���� ����. � �� ���� �������, �� �� �������.",
					"� ����, �� �� ������� �� ��������, ��� �� �����, � ���.",
					"� �� ����� ���� ������, � �� ����, ���� �� ���� ���������.",
					"���� � �������� ��� ���� ������, � ���� � �����.",
					"³� ����� ������� ��� ��� ��������, ��� � �� �������, �� �� ���������� ���."
				}),
				new Suspect("�����", new List<string>
				{
					"���� �� ���� ���������? � ����� �� ����! � ��� �����, ���� �� �������.",
					"� �� ��� ������ ������� � ������. �� ������� �� � �����.",
					"� ��� ������, ��� �����, �� �� ���������, ���� ���� �����.",
					"�� ����� ������ ����������, �� � ��� �����. �������� ��!",
					"�� ����� ������, �� � ��� �� �� �������? � ����� �� ������ �� ����� ����.",
					"���� �������� ���� ��������, �� ������ �� �����."
				}),
				new Suspect("�����", new List<string>
				{
					"� �� ��� ������� �������� � ������. ³� ��� ������� ������.",
					"� ��� ������, ��� �� ����� �����. � �����, �� �� ����� ����������.",
					"�� ����� ��������� ��� ��������, ��� �� �� ������, �� � ���� ��� ���� �����.",
					"� ���� ������ ��� ������, �� � ��� �� ������� � ��� �����.",
					"� �� ������, ���� �� ���� ��������.",
					"� ������ ������� ���������, ��� � ���� � �����?"
				}),
				new Suspect("�����", new List<string>
				{
					"�� ����������� � ����, ��� �� �� ������ �����. � ����� ����, ��� �� �� �������� �� �� ������.",
					"� ���� ������� �� �����, � ���� ���� ������� ������.",
					"� �� �����, �� � ����� ���� ��������. ³� ����� �� ������� ��� ��.",
					"³� ���������� ���� �� ������, ��� � �� ������ ������.",
					"� �� �����, �� � ����� ���� ��� ������� ��������.",
					"�� � � ��� ������, �� � ����� ���� ��������� �� �����?"
				}),
				new Suspect("������", new List<string>
				{
					"� �� ����, ��� �� �� �������. � ��� � �������� � �� ��.",
					"� ���� ���� �����, �� ������ ���������� �� ���.",
					"� ��� ��� ���� ��������, ��� ����� �� �����, �� �� ���� ��������� �� ������.",
					"� ����, �� � ����� ���� �������� ��������, ��� �� �����.",
					"� ����� �� �����, �� ���� ���� �������.",
					"�� ��� ���������� �������, �������� ��."
				})
			}, new List<string> { "³������", "�����", "�������", "����", "ϳ����" }));

			cases.Add(new Case("������ ���������", new List<Suspect>
			{
				new Suspect("�����", new List<string>
				{
					"� �� ����� ������ � ��� �����.",
					"� ��� �� �������, �� ��������� ����!",
					"����� ������, � �� ���� ����� �� ����.",
					"������� ���� � ���� ����� �� �����, ��� ���� ������ �����, � �� ���'���� ���.",
					"ͳ, � �� ������ ����� ������� ���� ������.",
					"������ �������� � ����-���� �� �������, � ���� ��� ��� ���."
				}),
				new Suspect("�������", new List<string>
				{
					"�� �� ���� ��������� �������, ��� �� �� ������, �� � ���� ����.",
					"� �������, ��� ��������� �� ���� �������.",
					"������� ����� �� ����� �� �����������.",
					"� ���� ���� ��� ������, ��� �� �� ������.",
					"� �� ����, �� ����� �������.",
					"��������� �� �� ������� �����, ����, �������, ������ �����."
				}),
				new Suspect("³����", new List<string>
				{
					"� �� ����, ���� � ���.",
					"� ������ ������, ���� �� �������.",
					"������ ���� ���� ������, � ����� �� ��� � �� �����.",
					"� ������ ���� ����������� � �������.",
					"�� ������, �� � ���� ���� ���������.",
					"������ ������� ���� � �����, � ����� �� ����."
				}),
				new Suspect("����", new List<string>
				{
					"�� ������ ������, � �� ���� ����� �������.",
					"�� �� ��� ������!",
					"� ������ �� ����� ����, ��� �� ����� �� ���������� �� �����.",
					"� ���� ���� ������� �������.",
					"�� �����, �� ���� ��� ������ ���������.",
					"����� ��������� ����� �� ��������� ��������."
				}),
				new Suspect("�����", new List<string>
				{
					"� ��� �� ������� � ��������, �� �� �� �����.",
					"����� �� ���� ��������?",
					"� ��� ���� ������� ������ �� ������� ������, � �����.",
					"� ����� �� ��� ������ �������� � �������.",
					"��� ��������, �� ���� ��������� � ������.",
					"�� ������ ������ ���������� �� ���."
				})
			}, new List<string> { "��������", "����� �����", "��������", "����", "�����" }));

			cases.Add(new Case("����� ������", new List<Suspect>
			{
				new Suspect("����", new List<string>
				{
					"� �� ����, �� �������.",
					"� ��� �� ������, ���� �� ���� ���������.",
					"���� ��� ���� ��������, � �� ������� �� ������.",
					"� �� ����� ����� ������� �����.",
					"�� ��� ������� ��������, ��� �� �� �������.",
					"������ ��������� ������ � ����� �� ������, � ��� ��� �� ������� �� ����."
				}),
				new Suspect("�������", new List<string>
				{
					"� �� ���� � ��� ������, � �������.",
					"�� �� �������, � ���� � ������ ���.",
					"� ���� � ��������� � ������, � ������ �� ������������ � ������.",
					"�����? �� �� ����������� ��� ����� ������.",
					"� ���� ��� �������, ��� �� �� ����� �����.",
					"��������� �� ���� ����� ��� �������, ����, �������, ������ �����."
				}),
				new Suspect("�����", new List<string>
				{
					"� �� ���� ���������, � ����� �� ����.",
					"�� �� ��� ��������.",
					"� ������ �� ���� ����� �������.",
					"�� ���� ������� � ���.",
					"�� ������, ���� �� ���� ������.",
					"� ��� ��� ����� ������, �� �������� ���� � ��."
				}),
				new Suspect("������", new List<string>
				{
					"� ���� � ����, �� ���, �� � ����.",
					"�� ��������� ���� ��� �� �����.",
					"� ���� ���� ����� ������ � ������ �����, � ����� �� �������.",
					"� ��������� � ����������� � �������.",
					"����� ��� �� ������, ��������, �� ���'����.",
					"������� ���� � �����, � ���� ������ ��� �����."
				}),
				new Suspect("���", new List<string>
				{
					"� ���� ������� � ��� �����, � ��� �� ������ �� �������.",
					"� ����� ��������, � �� ���� �����.",
					"� ���� ���� ������� ������, ��� ����������� ������.",
					"� �� ������������ � ������ ���� ���.",
					"��� �������� ��������� �� �� ���������, � ����� �� ����.",
					"���� �����, ��������� �� ������ ������, ��� �� ���� ������."
				})
			}, new List<string> { "����", "����", "����������", "������ �������", "�������� ������" }));

			var selectedCase = cases[rnd.Next(cases.Count)];
			Console.WriteLine($"������ ���������. ������: {selectedCase.Victim}, �������� � ������� �����.");
			Console.WriteLine("ϳ��������: " + string.Join(", ", selectedCase.Suspects.Select(s => s.Name)) + ".");
			Console.WriteLine();

			foreach (var locationName in selectedCase.Locations)
			{
				locations.Add(new Location(locationName));
			}

			suspects = selectedCase.Suspects;
			killer = suspects[rnd.Next(suspects.Count)];
		}

		public void MainLoop()
		{
			while (!caseSolved && timeLeft > 0)
			{
				Console.WriteLine($"���� ����������: {timeLeft} ����");

				if (currentLocation == null)
				{
					Console.WriteLine("������ �������:");
					for (int i = 0; i < locations.Count; i++)
					{
						Console.WriteLine($"{i + 1}. {locations[i].Name}");
					}

					if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= locations.Count)
					{
						currentLocation = locations[index - 1];
						Console.WriteLine($"�� ������ �������: {currentLocation.Name}");
						currentLocation.Describe();
					}
					else
					{
						Console.WriteLine("������� ����.\n");
						continue;
					}
				}

				Console.WriteLine("1. �������� �������");
				Console.WriteLine("2. �������� ������������");
				Console.WriteLine("3. ������������� ������");
				Console.WriteLine("4. ����������� ������������");
				Console.WriteLine("5. ��������� ������");
				Console.WriteLine("6. ����������� ������ ������");
				Console.WriteLine("7. ����������� ������ ����");
				Console.WriteLine("8. �������� ������� (-5 ����)");
				Console.WriteLine("9. ��������� ��������");
				Console.WriteLine("10. ��������� �������");
				Console.WriteLine("11. ����� ������ � ����������");
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
						Console.WriteLine($"��� �������� �������: {score} ����.\n");
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
						Console.WriteLine("������� ����. ��������� �� ���.");
						break;
				}

				if (timeLeft == 0)
				{
					Console.WriteLine("��� ���������! ������ �� ��������.");
				}
			}
		}

		private void ExploreCurrentLocation()
		{
			Console.WriteLine($"{currentLocation.Name}:");
			string foundItem = currentLocation.Explore(evidences);
			journal.Add($"��������� �������: {currentLocation.Name}");

			int itemChance = 30 + (skills.SearchSkill * 10);
			if (rnd.Next(100) < itemChance)
			{
				string item = currentLocation.FindItem();
				inventory.Add(item);
				Console.WriteLine($"�������� �������: {item}");
			}

			score += 5;
		}

		private void InterrogateSuspect()
		{
			Console.WriteLine("������ ������������:");
			for (int i = 0; i < suspects.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {suspects[i].Name}");
			}

			if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= suspects.Count)
			{
				Console.WriteLine();
				suspects[index - 1].Interrogate(skills.DetectLies);
				journal.Add($"�������� ������������: {suspects[index - 1].Name}");
				score += 3;

				int timeCost = 1 - (skills.FastInterrogation);
				timeLeft -= Math.Max(0, timeCost);
			}
			else
			{
				Console.WriteLine("������� ����.\n");
			}
		}

		private void AnalyzeEvidence()
		{
			Console.WriteLine("����� ������...");
			int analysisTime = 3000 - (skills.AnalysisSpeed * 500);
			Thread.Sleep(Math.Max(1000, analysisTime));

			Console.WriteLine("������� ����� ����� � ���������, �� �������� ������� ��������� � 22:30.");
			journal.Add("��������� ����� ������");
			score += 2;
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
					Console.WriteLine($"������� ����. �������� ��������� � {killer.Name}.");
					score -= 10;
				}

				journal.Add($"�����������: {suspects[index - 1].Name}");
				Console.WriteLine($"��� ��������� �������: {score} ����.\n");
				caseSolved = true;
			}
			else
			{
				Console.WriteLine("������� ����.\n");
			}
		}

		private void ShowEvidence()
		{
			Console.WriteLine("������� ������:");
			if (evidences.Count == 0)
			{
				Console.WriteLine("������ �� �� �������.");
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

		private void ShowJournal()
		{
			Console.WriteLine("������ ����:");
			if (journal.Count == 0)
			{
				Console.WriteLine("������ �������.");
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

		private void ShowInventory()
		{
			Console.WriteLine("��� ��������:");
			if (inventory.Count == 0)
			{
				Console.WriteLine("�������� �������.");
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

		private void UpgradeSkills()
		{
			Console.WriteLine("������� ������� ��� ��������:");
			Console.WriteLine("1. �������� ������ ������");
			Console.WriteLine("2. ����� ����������� ������");
			Console.WriteLine("3. ������ ����� (����� ����� ������ ��������)");
			Console.WriteLine("4. ������� ����� (����� ���� �����������)");
			Console.WriteLine("����:");
			string input = Console.ReadLine();

			switch (input)
			{
				case "1":
					skills.AnalysisSpeed++;
					Console.WriteLine("�������� ������ ��������.\n");
					break;
				case "2":
					skills.DetectLies++;
					Console.WriteLine("����� ����������� ������ ��������.\n");
					break;
				case "3":
					skills.SearchSkill++;
					Console.WriteLine("����� ������ ��������.\n");
					break;
				case "4":
					skills.FastInterrogation++;
					Console.WriteLine("�������� ������ ��������.\n");
					break;
				default:
					Console.WriteLine("������� ����.\n");
					break;
			}
		}

		private void SubmitSampleToLab()
		{
			Console.WriteLine("������ ������ ��� ����� � ����������:");
			Console.WriteLine("1. �������");
			Console.WriteLine("2. �����");
			Console.WriteLine("3. ������ ����");
			Console.WriteLine("4. ������ �����");
			Console.WriteLine("5. ����� ������� � ���������");

			string choice = Console.ReadLine();
			string report = "";
			Suspect matchingSuspect = null;

			switch (choice)
			{
				case "1":
					matchingSuspect = killer;
					report = "������ �������: ���������� ����������.";
					break;
				case "2":
					matchingSuspect = killer;
					report = "������ �����: ���������� ����������.";
					break;
				case "3":
					matchingSuspect = killer;
					report = "������ ����: ���������� ����������.";
					break;
				case "4":
					matchingSuspect = killer;
					report = "������ �����: ���������� ����������.";
					break;
				case "5":
					if (inventory.Count > 0)
					{
						Console.WriteLine("������ ������� � ���������:");
						for (int i = 0; i < inventory.Count; i++)
						{
							Console.WriteLine($"{i + 1}. {inventory[i]}");
						}
						if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex >= 1 && itemIndex <= inventory.Count)
						{
							report = $"{inventory[itemIndex - 1]}: ���������� ����������.";
						}
						else
						{
							Console.WriteLine("������� ����.");
							return;
						}
					}
					else
					{
						Console.WriteLine("�������� �������.");
						return;
					}
					break;
				default:
					Console.WriteLine("������� ����.");
					return;
			}

			labReports.Add(report);
			Console.WriteLine(report);
			Console.WriteLine("������� ����������...");
			Thread.Sleep(2000);

			ProvideLabResults(matchingSuspect);
			journal.Add("����� ������ � ����������.");
			score += 5;
		}

		private void ProvideLabResults(Suspect matchingSuspect)
		{
			string[] results =
			{
				"����� �������, �� ������� �������� {0}. �� �������� �����.",
				"����� ����� ������ ���� ����, �� �������� {0}.",
				"����� ������ ���� ���������, �� �� �������� {0}.",
				"������ ����� ������ ���� ������� {0}.",
				"������ � ��������� ����� �� {0}, �� ���� �� ���� �������.",
				"����� �������� �������, �� �� ������ � ��� {0}."
			};

			if (matchingSuspect != null)
			{
				string result = string.Format(results[rnd.Next(results.Length)], matchingSuspect.Name);
				Console.WriteLine("��� �������� �������.");
				Console.WriteLine("����������: " + result);
			}
			else
			{
				Console.WriteLine("�� ����, �� �������� ������ ���� � ������������.");
			}
		}

		private void GiveHint()
		{
			if (score >= 5)
			{
				Console.WriteLine("ϳ������: ��������� ������ �������� ������� ����������, ���� �������� �� ��� �����.");
				journal.Add("�������� �������");
				score -= 5;
				Console.WriteLine();
				timeLeft--;
			}
			else { Console.WriteLine("� ��� ����������� ����, ��� ����������� ��������!"); }
		}
	}

	class Location
	{
		public string Name { get; set; }
		private Random rnd = new Random();

		public Location(string name)
		{
			Name = name;
		}

		public void Describe()
		{
			string description = Name switch
			{
				"³������" => "�� ���������� � ������� ������, �� ������ ��������� ������ ����.",
				"�����" => "�����, �� �������� ���, ��� ��� ���� �����.",
				"�������" => "����� �������, �� ������ ���� ���� ��������.",
				"����" => "����, �� ������ ��������� ��� ��������� ��������.",
				"ϳ����" => "������ �����, �� ������ ���� �������� ������.",
				"��������" => "�������� ��������, ��� � �� ���� ���������.",
				"����� �����" => "̳���, �� ������ ����� ��������� �����������.",
				"��������" => "��������, �� ������ ���� �������.",
				"����������" => "������� ����, �� ������ ���� ������ ����.",
				"������ �������" => "������ �������, �� ����� ������� �����.",
				"�������� ������" => "����� ������, � ��� ������ ���� �������� �����.",
				_ => "�� ��������� �������."
			};

			Console.WriteLine($"���� �������: {description}");
		}

		public string Explore(List<Evidence> evidences)
		{
			string[] findings = {
				"�������� ���� � ���������",
				"���� ������ ��� ����",
				"�������� ��������",
				"������� ��������",
				"���� � ���������",
				"���� ����� �� �����",
				"���������� � �������",
				"����� � ������ �������",
				"���������� �������� �������",
				"����������� ����'����",
				"������� � �����������",
				"������� ��������",
				"���� �� ������� ����",
				"����������� �������, ���� �� ������� ���� ���"
			};
			string found = findings[rnd.Next(findings.Length)];
			Console.WriteLine($"{found}.");
			evidences.Add(new Evidence(found));
			return found;
		}

		public string FindItem()
		{
			string[] items = {
				"�������",
				"��������� ���",
				"����� ���������",
				"������ �������",
				"������� ��� �����",
				"����� ������",
				"������ �������",
				"������� �����",
				"�������",
				"�������� � ���������",
				"������ � ����������",
				"���� � �����������",
				"���������� ������ � ���������",
				"������ ����",
				"������ � ������",
				"�������� �����",
				"������ ������� � �������������",
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
			string answer = isLying
				? $"{phrases[rnd.Next(phrases.Count)]}"
				: $"{phrases[rnd.Next(phrases.Count)]}";

			Console.WriteLine($"{Name} ����: \"{answer}\"");

			if (detectLies > 0 && isLying)
			{
				Console.WriteLine("(��� ����� ������: ����������� �����!)");
			}
			Console.WriteLine();
		}
	}

	class Evidence
	{
		public string Description { get; set; }

		public Evidence(string desc)
		{
			Description = desc;
		}
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