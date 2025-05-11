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
			Console.WriteLine("Вiтаємо у грi 'Слiдство веде детектив'.");
			Console.WriteLine("Натисніть будь-яку клавішу, щоб почати...");
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
			cases.Add(new Case("Іван Петрович", new List<Suspect>
			{
				new Suspect("Анна", new List<string>
				{
					"Я була з Іваном в останній вечір. Ми говорили про наше майбутнє.",
					"Я не хочу говорити про це, але це було дуже емоційно.",
					"Він обіцяв мені, що не залишить мене одну. Я не можу повірити, що це сталося.",
					"Я чула, як він говорив по телефону, але не знала, з ким.",
					"Я не могла бути винною, я не знаю, чому ви мене підозрюєте.",
					"Коли я дізналася про його смерть, я була в шокові.",
					"Він часто говорив про свої проблеми, але я не уявляла, що це закінчиться так."
				}),
				new Suspect("Борис", new List<string>
				{
					"Чому ви мене підозрюєте? Я нічого не знаю! Я був удома, коли це сталося.",
					"Я не мав жодних проблем з Іваном. Ми дружили ще зі школи.",
					"Я чув постріл, але думав, що це фейерверк, адже було свято.",
					"Мої сусіди можуть підтвердити, що я був вдома. Перевірте це!",
					"Ви дійсно думаєте, що я зміг би це зробити? Я ніколи не піднімав на нього руку.",
					"Наші стосунки були хорошими, ви повинні це знати."
				}),
				new Suspect("Сергій", new List<string>
				{
					"Я не мав жодного конфлікту з Іваном. Він був хорошим другом.",
					"Я чув постріл, але не бачив нічого. Я думав, що це знову фейерверки.",
					"Ми разом працювали над проектом, але це не означає, що я знав про його плани.",
					"Я можу надати вам докази, що я був на зустрічі в той вечір.",
					"Я не розумію, чому ви мене запитуєте.",
					"Я завжди готовий допомогти, але в чому я винен?"
				}),
				new Suspect("Олена", new List<string>
				{
					"Ми зустрічалися в кафе, але це не означає нічого. Я люблю його, але він не відповідав на мої дзвінки.",
					"Я була зайнята на роботі, у мене були термінові справи.",
					"Я не знала, що у нього були проблеми. Він ніколи не говорив про це.",
					"Він запрошував мене на зустріч, але я не змогла прийти.",
					"Я не знала, що у нього були такі серйозні проблеми.",
					"Чи є у вас докази, що я могла бути причетною до цього?"
				}),
				new Suspect("Дмитро", new List<string>
				{
					"Я не знаю, хто це міг зробити. Я був у спортзалі в ту ніч.",
					"У мене були свідки, які можуть підтвердити моє алібі.",
					"Я чув про його проблеми, але ніколи не думав, що це може призвести до такого.",
					"Я знав, що у нього були фінансові труднощі, але не більше.",
					"Я ніколи не думав, що таке може статися.",
					"Мої алібі підтверджені свідками, перевірте їх."
				})
			}, new List<string> { "Вітальня", "Кухня", "Спальня", "Офіс", "Підвал" }));

			cases.Add(new Case("Оксана Сидоренко", new List<Suspect>
			{
				new Suspect("Артем", new List<string>
				{
					"Я не бачив Оксану в той вечір.",
					"Я був на вечірці, не запитуйте мене!",
					"Чесно кажучи, я не дуже добре її знав.",
					"Вечірка була у мого друга за містом, там було багато людей, я не пам'ятаю всіх.",
					"Ні, я не помічав нічого дивного того вечора.",
					"Можете запитати у будь-кого на вечірці, я весь час був там."
				}),
				new Suspect("Людмила", new List<string>
				{
					"Ми не були близькими друзями, але це не означає, що я щось знаю.",
					"Я зайнята, щоб відповідати на ваші питання.",
					"Останнім часом ми майже не спілкувалися.",
					"У мене були свої справи, мені не до Оксани.",
					"Я не знаю, що могло статися.",
					"Зверніться до її ближчих друзів, вони, можливо, знають більше."
				}),
				new Suspect("Віктор", new List<string>
				{
					"Я не знаю, чому я тут.",
					"Я слухав музику, коли це сталося.",
					"Музика була дуже гучною, я нічого не чув і не бачив.",
					"Я взагалі рідко перетинався з Оксаною.",
					"Не розумію, як я можу бути причетний.",
					"Просто залиште мене у спокої, я нічого не знаю."
				}),
				new Suspect("Маша", new List<string>
				{
					"Ми просто знайомі, я не можу нічого сказати.",
					"Це не моя справа!",
					"Я бачила її кілька разів, але ми ніколи не розмовляли по душах.",
					"У мене своїх проблем вистачає.",
					"Не думаю, що можу вам чимось допомогти.",
					"Краще пошукайте серед її близького оточення."
				}),
				new Suspect("Тарас", new List<string>
				{
					"Я був на зустрічі з колегами, це не мій стиль.",
					"Навіщо ви мене допитуєте?",
					"У нас була важлива нарада до пізнього вечора, є свідки.",
					"Я ніколи не мав жодних конфліктів з Оксаною.",
					"Мені неприємно, що мене підозрюють у такому.",
					"Мої колеги можуть підтвердити моє алібі."
				})
			}, new List<string> { "Квартира", "Салон краси", "Ресторан", "Офіс", "Склад" }));

			cases.Add(new Case("Петро Коваль", new List<Suspect>
			{
				new Suspect("Ігор", new List<string>
				{
					"Я не знаю, що сталося.",
					"Я був на стадіоні, тому не можу допомогти.",
					"Матч був дуже важливий, я не відходив від екрану.",
					"Я не бачив Петра останнім часом.",
					"Не маю жодного уявлення, хто міг це зробити.",
					"Можете перевірити записи з камер на стадіоні, я був там від початку до кінця."
				}),
				new Suspect("Світлана", new List<string>
				{
					"Я не мала з ним справи, я зайнята.",
					"Це не можливо, я була в іншому місті.",
					"Я була у відрядженні в Кракові, є квитки та підтвердження з готелю.",
					"Петро? Ми не спілкувалися вже кілька місяців.",
					"У мене свої турботи, мені не до чужих справ.",
					"Зверніться до його колег або родичів, вони, ймовірно, знають більше."
				}),
				new Suspect("Роман", new List<string>
				{
					"Я не можу допомогти, я нічого не знаю.",
					"Це не моя проблема.",
					"Я взагалі не знав Петра близько.",
					"Ми лише віталися і все.",
					"Не розумію, чому ви мене питаєте.",
					"Я маю свої власні справи, не втягуйте мене в це."
				}),
				new Suspect("Наталя", new List<string>
				{
					"Я була в клубі, це все, що я знаю.",
					"Не запитуйте мене про це більше.",
					"У клубі була гучна музика і багато людей, я нічого не помітила.",
					"Я танцювала і розважалася з друзями.",
					"Петра там не бачила, принаймні, не пам'ятаю.",
					"Залиште мене в спокої, я хочу забути той вечір."
				}),
				new Suspect("Юлія", new List<string>
				{
					"Я була зайнята в той вечір, і мені не потрібні ці питання.",
					"Я знову повторюю, я не знаю нічого.",
					"У мене була важлива зустріч, яка затягнулася допізна.",
					"Я не перетиналася з Петром того дня.",
					"Мені неприємно відповідати на ці запитання, я нічого не знаю.",
					"Будь ласка, зверніться до когось іншого, хто міг його бачити."
				})
			}, new List<string> { "Офіс", "Кафе", "Гардеробна", "Темний коридор", "Секретна кімната" }));

			var selectedCase = cases[rnd.Next(cases.Count)];
			Console.WriteLine($"Справу розпочато. Жертва: {selectedCase.Victim}, вбивство у закритій кімнаті.");
			Console.WriteLine("Підозрювані: " + string.Join(", ", selectedCase.Suspects.Select(s => s.Name)) + ".");
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
				Console.WriteLine($"Часу залишилось: {timeLeft} ходів");

				if (currentLocation == null)
				{
					Console.WriteLine("Оберіть локацію:");
					for (int i = 0; i < locations.Count; i++)
					{
						Console.WriteLine($"{i + 1}. {locations[i].Name}");
					}

					if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= locations.Count)
					{
						currentLocation = locations[index - 1];
						Console.WriteLine($"Ви обрали локацію: {currentLocation.Name}");
						currentLocation.Describe();
					}
					else
					{
						Console.WriteLine("Невірний вибір.\n");
						continue;
					}
				}

				Console.WriteLine("1. Дослідити локацію");
				Console.WriteLine("2. Допитати підозрюваного");
				Console.WriteLine("3. Проаналізувати докази");
				Console.WriteLine("4. Звинуватити підозрюваного");
				Console.WriteLine("5. Перевірити оцінку");
				Console.WriteLine("6. Переглянути список доказів");
				Console.WriteLine("7. Переглянути журнал подій");
				Console.WriteLine("8. Отримати підказку (-5 балів)");
				Console.WriteLine("9. Перевірити інвентар");
				Console.WriteLine("10. Розвинути навички");
				Console.WriteLine("11. Здати зразки в лабораторію");
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
						Console.WriteLine($"Ваш поточний рахунок: {score} балів.\n");
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
						Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
						break;
				}

				if (timeLeft == 0)
				{
					Console.WriteLine("Час вичерпано! Справу не розкрито.");
				}
			}
		}

		private void ExploreCurrentLocation()
		{
			Console.WriteLine($"{currentLocation.Name}:");
			string foundItem = currentLocation.Explore(evidences);
			journal.Add($"Досліджено локацію: {currentLocation.Name}");

			int itemChance = 30 + (skills.SearchSkill * 10);
			if (rnd.Next(100) < itemChance)
			{
				string item = currentLocation.FindItem();
				inventory.Add(item);
				Console.WriteLine($"Знайдено предмет: {item}");
			}

			score += 5;
		}

		private void InterrogateSuspect()
		{
			Console.WriteLine("Оберіть підозрюваного:");
			for (int i = 0; i < suspects.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {suspects[i].Name}");
			}

			if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= suspects.Count)
			{
				Console.WriteLine();
				suspects[index - 1].Interrogate(skills.DetectLies);
				journal.Add($"Допитано підозрюваного: {suspects[index - 1].Name}");
				score += 3;

				int timeCost = 1 - (skills.FastInterrogation);
				timeLeft -= Math.Max(0, timeCost);
			}
			else
			{
				Console.WriteLine("Невірний вибір.\n");
			}
		}

		private void AnalyzeEvidence()
		{
			Console.WriteLine("Аналіз доказів...");
			int analysisTime = 3000 - (skills.AnalysisSpeed * 500);
			Thread.Sleep(Math.Max(1000, analysisTime));

			Console.WriteLine("Експерт провів аналіз і встановив, що вбивство сталося приблизно о 22:30.");
			journal.Add("Проведено аналіз доказів");
			score += 2;
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
					Console.WriteLine($"Невірний вибір. Справжній злочинець — {killer.Name}.");
					score -= 10;
				}

				journal.Add($"Звинуватили: {suspects[index - 1].Name}");
				Console.WriteLine($"Ваш фінальний рахунок: {score} балів.\n");
				caseSolved = true;
			}
			else
			{
				Console.WriteLine("Невірний вибір.\n");
			}
		}

		private void ShowEvidence()
		{
			Console.WriteLine("Знайдені докази:");
			if (evidences.Count == 0)
			{
				Console.WriteLine("Докази ще не знайдені.");
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
			Console.WriteLine("Журнал подій:");
			if (journal.Count == 0)
			{
				Console.WriteLine("Журнал порожній.");
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
			Console.WriteLine("Ваш інвентар:");
			if (inventory.Count == 0)
			{
				Console.WriteLine("Інвентар порожній.");
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
			Console.WriteLine("Доступні навички для розвитку:");
			Console.WriteLine("1. Швидкість аналізу доказів");
			Console.WriteLine("2. Вміння розпізнавати брехню");
			Console.WriteLine("3. Кращий обшук (більше шансів знайти предмети)");
			Console.WriteLine("4. Швидкий допит (менше часу витрачається)");
			Console.WriteLine("Вибір:");
			string input = Console.ReadLine();

			switch (input)
			{
				case "1":
					skills.AnalysisSpeed++;
					Console.WriteLine("Швидкість аналізу збільшено.\n");
					break;
				case "2":
					skills.DetectLies++;
					Console.WriteLine("Вміння розпізнавати брехню збільшено.\n");
					break;
				case "3":
					skills.SearchSkill++;
					Console.WriteLine("Навик пошуку збільшено.\n");
					break;
				case "4":
					skills.FastInterrogation++;
					Console.WriteLine("Швидкість допиту збільшено.\n");
					break;
				default:
					Console.WriteLine("Невірний вибір.\n");
					break;
			}
		}

		private void SubmitSampleToLab()
		{
			Console.WriteLine("Оберіть зразок для здачі в лабораторію:");
			Console.WriteLine("1. Волосся");
			Console.WriteLine("2. Пляма");
			Console.WriteLine("3. Зразок крові");
			Console.WriteLine("4. Зразок одягу");
			Console.WriteLine("5. Інший предмет з інвентарю");

			string choice = Console.ReadLine();
			string report = "";
			Suspect matchingSuspect = null;

			switch (choice)
			{
				case "1":
					matchingSuspect = killer;
					report = "Зразок волосся: результати очікуються.";
					break;
				case "2":
					matchingSuspect = killer;
					report = "Зразок плями: результати очікуються.";
					break;
				case "3":
					matchingSuspect = killer;
					report = "Зразок крові: результати очікуються.";
					break;
				case "4":
					matchingSuspect = killer;
					report = "Зразок одягу: результати очікуються.";
					break;
				case "5":
					if (inventory.Count > 0)
					{
						Console.WriteLine("Оберіть предмет з інвентарю:");
						for (int i = 0; i < inventory.Count; i++)
						{
							Console.WriteLine($"{i + 1}. {inventory[i]}");
						}
						if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex >= 1 && itemIndex <= inventory.Count)
						{
							report = $"{inventory[itemIndex - 1]}: результати очікуються.";
						}
						else
						{
							Console.WriteLine("Невірний вибір.");
							return;
						}
					}
					else
					{
						Console.WriteLine("Інвентар порожній.");
						return;
					}
					break;
				default:
					Console.WriteLine("Невірний вибір.");
					return;
			}

			labReports.Add(report);
			Console.WriteLine(report);
			Console.WriteLine("Чекайте результати...");
			Thread.Sleep(2000);

			ProvideLabResults(matchingSuspect);
			journal.Add("Здано зразки в лабораторію.");
			score += 5;
		}

		private void ProvideLabResults(Suspect matchingSuspect)
		{
			string[] results =
			{
				"Аналіз показав, що волосся належить {0}. Це важливий доказ.",
				"Аналіз плями виявив сліди крові, які належать {0}.",
				"Аналіз зразка крові підтвердив, що він належить {0}.",
				"Зразок одягу містить сліди волосся {0}.",
				"Зразок з інвентарю вказує на {0}, що було на місці злочину.",
				"Аналіз матеріалів показав, що на зразку є ДНК {0}."
			};

			if (matchingSuspect != null)
			{
				string result = string.Format(results[rnd.Next(results.Length)], matchingSuspect.Name);
				Console.WriteLine("Звіт експерта готовий.");
				Console.WriteLine("Результати: " + result);
			}
			else
			{
				Console.WriteLine("На жаль, не знайдено жодних збігів з підозрюваними.");
			}
		}

		private void GiveHint()
		{
			if (score >= 5)
			{
				Console.WriteLine("Підказка: підозрювані можуть надавати важливу інформацію, якщо запитати їх про деталі.");
				journal.Add("Отримано підказку");
				score -= 5;
				Console.WriteLine();
				timeLeft--;
			}
			else { Console.WriteLine("У вас недостатньо балів, щоб використати підсказку!"); }
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
				"Вітальня" => "Ви перебуваєте в затишній вітальні, де жертва проводила багато часу.",
				"Кухня" => "Кухня, де готували їжу, але тут щось дивне.",
				"Спальня" => "Темна спальня, де можуть бути сліди боротьби.",
				"Офіс" => "Офіс, де жертва працювала над важливими справами.",
				"Підвал" => "Темний підвал, де можуть бути приховані докази.",
				"Квартира" => "Звичайна квартира, але в ній щось приховано.",
				"Салон краси" => "Місце, де жертва могла зустрічати підозрюваних.",
				"Ресторан" => "Ресторан, де жертва мала зустрічі.",
				"Гардеробна" => "Закрите місце, де можуть бути важливі речі.",
				"Темний коридор" => "Темний коридор, де могла статися атака.",
				"Секретна кімната" => "Таємна кімната, в якій можуть бути приховані факти.",
				_ => "Це незнайома локація."
			};

			Console.WriteLine($"Опис локації: {description}");
		}

		public string Explore(List<Evidence> evidences)
		{
			string[] findings = {
				"знайдено лист з погрозами",
				"сліди взуття біля вікна",
				"зламаний годинник",
				"розбите дзеркало",
				"ключ з ініціалами",
				"сліди бруду на підлозі",
				"фотографія з жертвою",
				"книга з таємним записом",
				"викрадений мобільний телефон",
				"пошкоджений комп'ютер",
				"згорток з наркотиками",
				"згорілий документ",
				"сліди від крапель крові",
				"незвичайний предмет, який не повинен бути тут"
			};
			string found = findings[rnd.Next(findings.Length)];
			Console.WriteLine($"{found}.");
			evidences.Add(new Evidence(found));
			return found;
		}

		public string FindItem()
		{
			string[] items = {
				"ліхтарик",
				"секретний код",
				"гумові рукавички",
				"сканер відбитків",
				"шпилька для замків",
				"пакет доказів",
				"картка доступу",
				"червона фарба",
				"ноутбук",
				"записник з нотатками",
				"діамант з викрадення",
				"диск з відеозаписом",
				"пластикова пляшка з відбитками",
				"зразок шкіри",
				"флешка з даними",
				"підозрілий запис",
				"старий телефон з повідомленнями",
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
			string answer = isLying
				? $"{phrases[rnd.Next(phrases.Count)]}"
				: $"{phrases[rnd.Next(phrases.Count)]}";

			Console.WriteLine($"{Name} каже: \"{answer}\"");

			if (detectLies > 0 && isLying)
			{
				Console.WriteLine("(Ваш навик підказує: підозрюваний бреше!)");
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