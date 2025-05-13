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
			cases.Add(new Case("iван Петрович", new List<Suspect>
	  {
		new Suspect("Анна", new List<string>
		{
		  "Я була з iваном в останнiй вечiр. Ми говорили про наше майбутнє.",
		  "Я не хочу говорити про це, але це було дуже емоцiйно.",
		  "Вiн обiцяв менi, що не залишить мене одну. Я не можу повiрити, що це сталося.",
		  "Я чула, як вiн говорив по телефону, але не знала, з ким."
		}),
		new Suspect("Борис", new List<string>
		{
		  "Чому ви мене пiдозрюєте? Я нiчого не знаю! Я був удома, коли це сталося.",
		  "Я не мав жодних проблем з iваном. Ми дружили ще зi школи.",
		  "Я чув пострiл, але думав, що це фейерверк, адже було свято.",
		  "Мої сусiди можуть пiдтвердити, що я був вдома. Перевiрте це!"
		}),
		new Suspect("Сергiй", new List<string>
		{
		  "Я не мав жодного конфлiкту з iваном. Вiн був хорошим другом.",
		  "Я чув пострiл, але не бачив нiчого. Я думав, що це знову фейерверки.",
		  "Ми разом працювали над проектом, але це не означає, що я знав про його плани.",
		  "Я можу надати вам докази, що я був на зустрiчi в той вечiр."
		}),
		new Suspect("Олена", new List<string>
		{
		  "Ми зустрiчалися в кафе, але це не означає нiчого. Я люблю його, але вiн не вiдповiдав на мої дзвiнки.",
		  "Я була зайнята на роботi, у мене були термiновi справи.",
		  "Я не знала, що у нього були проблеми. Вiн нiколи не говорив про це.",
		  "Вiн запрошував мене на зустрiч, але я не змогла прийти."
		}),
		new Suspect("Дмитро", new List<string>
		{
		  "Я не знаю, хто це мiг зробити. Я був у спортзалi в ту нiч.",
		  "У мене були свiдки, якi можуть пiдтвердити моє алiбi.",
		  "Я чув про його проблеми, але нiколи не думав, що це може призвести до такого.",
		  "Я знав, що у нього були фiнансовi труднощi, але не бiльше."
		})
	  }, new List<string> { "Вiтальня", "Кухня", "Спальня", "Офiс", "Пiдвал" }));
			cases.Add(new Case("Оксана Сидоренко", new List<Suspect>
	  {
		new Suspect("Артем", new List<string>
		{
		  "Я не бачив Оксану в той вечiр.",
		  "Я був на вечiрцi, не запитуйте мене!",
		  "Чесно кажучи, я не дуже добре її знав.",
		  "Вечiрка була у мого друга за мiстом, там було багато людей, я не пам'ятаю всiх.",
		  "Нi, я не помiчав нiчого дивного того вечора.",
		  "Можете запитати у будь-кого на вечiрцi, я весь час був там."
		}),
		new Suspect("Людмила", new List<string>
		{
		  "Ми не були близькими друзями, але це не означає, що я щось знаю.",
		  "Я зайнята, щоб вiдповiдати на вашi питання.",
		  "Останнiм часом ми майже не спiлкувалися.",
		  "У мене були свої справи, менi не до Оксани.",
		  "Я не знаю, що могло статися.",
		  "Звернiться до її ближчих друзiв, вони, можливо, знають бiльше."
		}),
		new Suspect("Вiктор", new List<string>
		{
		  "Я не знаю, чому я тут.",
		  "Я слухав музику, коли це сталося.",
		  "Музика була дуже гучною, я нiчого не чув i не бачив.",
		  "Я взагалi рiдко перетинався з Оксаною.",
		  "Не розумiю, як я можу бути причетний.",
		  "Просто залиште мене у спокої, я нiчого не знаю."
		}),
		new Suspect("Маша", new List<string>
		{
		  "Ми просто знайомi, я не можу нiчого сказати.",
		  "Це не моя справа!",
		  "Я бачила її кiлька разiв, але ми нiколи не розмовляли по душах.",
		  "У мене своїх проблем вистачає.",
		  "Не думаю, що можу вам чимось допомогти.",
		  "Краще пошукайте серед її близького оточення."
		}),
		new Suspect("Тарас", new List<string>
		{
		  "Я був на зустрiчi з колегами, це не мiй стиль.",
		  "Навiщо ви мене допитуєте?",
		  "У нас була важлива нарада до пiзнього вечора, є свiдки.",
		  "Я нiколи не мав жодних конфлiктiв з Оксаною.",
		  "Менi неприємно, що мене пiдозрюють у такому.",
		  "Мої колеги можуть пiдтвердити моє алiбi."
		})
	  }, new List<string> { "Квартира", "Салон краси", "Ресторан", "Офiс", "Склад" }));

			cases.Add(new Case("Петро Коваль", new List<Suspect>
	  {
		new Suspect("iгор", new List<string>
		{
		  "Я не знаю, що сталося.",
		  "Я був на стадiонi, тому не можу допомогти.",
		  "Матч був дуже важливий, я не вiдходив вiд екрану.",
		  "Я не бачив Петра останнiм часом.",
		  "Не маю жодного уявлення, хто мiг це зробити.",
		  "Можете перевiрити записи з камер на стадiонi, я був там вiд початку до кiнця."
		}),
		new Suspect("Свiтлана", new List<string>
		{
		  "Я не мала з ним справи, я зайнята.",
		  "Це не можливо, я була в iншому мiстi.",
		  "Я була у вiдрядженнi в Краковi, є квитки та пiдтвердження з готелю.",
		  "Петро? Ми не спiлкувалися вже кiлька мiсяцiв.",
		  "У мене свої турботи, менi не до чужих справ.",
		  "Звернiться до його колег або родичiв, вони, ймовiрно, знають бiльше."
		}),
		new Suspect("Роман", new List<string>
		{
		  "Я не можу допомогти, я нiчого не знаю.",
		  "Це не моя проблема.",
		  "Я взагалi не знав Петра близько.",
		  "Ми лише вiталися i все.",
		  "Не розумiю, чому ви мене питаєте.",
		  "Я маю свої власнi справи, не втягуйте мене в це."
		}),
		new Suspect("Наталя", new List<string>
		{
		  "Я була в клубi, це все, що я знаю.",
		  "Не запитуйте мене про це бiльше.",
		  "У клубi була гучна музика i багато людей, я нiчого не помiтила.",
		  "Я танцювала i розважалася з друзями.",
		  "Петра там не бачила, принаймнi, не пам'ятаю.",
		  "Залиште мене в спокої, я хочу забути той вечiр."
		}),
		new Suspect("Юлiя", new List<string>
		{
		  "Я була зайнята в той вечiр, i менi не потрiбнi цi питання.",
		  "Я знову повторюю, я не знаю нiчого.",
		  "У мене була важлива зустрiч, яка затягнулася допiзна.",
		  "Я не перетиналася з Петром того дня.",
		  "Менi неприємно вiдповiдати на цi запитання, я нiчого не знаю.",
		  "Будь ласка, звернiться до когось iншого, хто мiг його бачити."
		})
	  }, new List<string> { "Офiс", "Кафе", "Гардеробна", "Темний коридор", "Секретна кiмната" }));
            var selectedCase = cases[rnd.Next(cases.Count)];
            Console.WriteLine($"Справу розпочато. Жертва: {selectedCase.Victim}, вбивство у закритiй кiмнатi.");
            Console.WriteLine("Пiдозрюванi: " + string.Join(", ", selectedCase.Suspects.Select(s => s.Name)) + ".");
            Console.WriteLine();

            foreach (var locationName in selectedCase.Locations)
            {
                locations.Add(new Location(locationName));
            }

            suspects = selectedCase.Suspects;
            killer = suspects[rnd.Next(suspects.Count)];
            Console.WriteLine($"(Секретно: Випадково обраний вбивця — {killer.Name})");
        }
        public void MainLoop()
        {
            while (!caseSolved && timeLeft > 0)
            {
                Console.WriteLine($"Часу залишилось: {timeLeft} ходiв");

                if (currentLocation == null)
                {
                    Console.WriteLine("Оберiть локацiю:");
                    for (int i = 0; i < locations.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {locations[i].Name}");
                    }

                    if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= locations.Count)
                    {
                        currentLocation = locations[index - 1];
                        Console.WriteLine($"Ви обрали локацiю: {currentLocation.Name}");
                        currentLocation.Describe();
                    }
                    else
                    {
                        Console.WriteLine("Невiрний вибiр.\n");
                        continue;
                    }
                }

                Console.WriteLine("1. Дослiдити локацiю");
                Console.WriteLine("2. Допитати пiдозрюваного");
                Console.WriteLine("3. Проаналiзувати докази");
                Console.WriteLine("4. Звинуватити пiдозрюваного");
                Console.WriteLine("5. Перевiрити оцiнку");
                Console.WriteLine("6. Переглянути список доказiв");
                Console.WriteLine("7. Переглянути журнал подiй");
                Console.WriteLine("8. Отримати пiдказку (-5 балiв)");
                Console.WriteLine("9. Перевiрити iнвентар");
                Console.WriteLine("10. Розвинути навички");
                Console.WriteLine("11. Здати зразки в лабораторiю");
                Console.WriteLine("12. Вийти з гри");

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
                        Console.WriteLine($"Ваш поточний рахунок: {score} балiв.\n");
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
                        Console.WriteLine("Гру завершено.");
                        return;
                    default:
                        Console.WriteLine("Невiрний вибiр. Спробуйте ще раз.");
                        break;
                }

                if (timeLeft == 0)
                {
                    Console.WriteLine("Час вичерпано! Справу не розкрито.");
                }
            }
        }
		private void ShowInventory()
		{
			Console.WriteLine("Ваш iнвентар:");
			if (inventory.Count == 0)
			{
				Console.WriteLine("iнвентар порожнiй.");
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
			Console.WriteLine("Журнал подiй:");
			if (journal.Count == 0)
			{
				Console.WriteLine("Журнал порожнiй.");
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
			Console.WriteLine("Знайденi докази:");
			if (evidences.Count == 0)
			{
				Console.WriteLine("Докази ще не знайденi.");
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
			Console.WriteLine("Кого ви хочете звинуватити?");
			for (int i = 0; i < suspects.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {suspects[i].Name}");
			}

			if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= suspects.Count)
			{
				Console.WriteLine();

				if (suspects[index - 1] == killer)
				{
					Console.WriteLine($"Ви правильно розкрили справу! Винний — {killer.Name}.");
					score += 20;
				}
				else
				{
					Console.WriteLine($"Невiрний вибiр. Справжнiй злочинець — {killer.Name}.");
					score -= 10;
				}

				journal.Add($"Звинуватили: {suspects[index - 1].Name}");
				Console.WriteLine($"Ваш фiнальний рахунок: {score} балiв.\n");
				caseSolved = true;
			}
			else
			{
				Console.WriteLine("Невiрний вибiр.\n");
			}
		}

		private void AnalyzeEvidence()
		{
			Console.WriteLine("Аналiз доказiв...");
			int analysisTime = 3000 - (skills.AnalysisSpeed * 500);
			Thread.Sleep(Math.Max(1000, analysisTime));

			Console.WriteLine("Експерт провiв аналiз i встановив, що вбивство сталося приблизно о 22:30.");
			journal.Add("Проведено аналiз доказiв");
			score += 2;

			Console.WriteLine("Пiдказка: шукайте зв'язок мiж доказами та пiдозрюваними.");
		}

		private void InterrogateSuspect()
		{
			Console.WriteLine("Оберiть пiдозрюваного:");
			for (int i = 0; i < suspects.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {suspects[i].Name}");
			}

			if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= suspects.Count)
			{
				Console.WriteLine();
				suspects[index - 1].Interrogate(skills.DetectLies);
				journal.Add($"Допитано пiдозрюваного: {suspects[index - 1].Name}");
				score += 3;

				int timeCost = 1 - (skills.FastInterrogation);
				timeLeft -= Math.Max(0, timeCost);

				Console.WriteLine("Пiдказка: звернiть увагу на деталi в заявах пiдозрюваних.");
			}
			else
			{
				Console.WriteLine("Невiрний вибiр.\n");
			}
		}