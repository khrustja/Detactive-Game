using System;

namespace DetectiveGame
{
    class Game
    {
        private CaseManager caseManager;

        public void Start()
        {
            Console.WriteLine("Вiтаємо у грi 'Слiдство веде детектив'.");
            Console.WriteLine("Натиснiть будь-яку клавiшу, щоб почати...");
            Console.ReadKey();
            Console.WriteLine();

            caseManager = new CaseManager();
            caseManager.InitializeCases();
            caseManager.MainLoop();
        }
    }
}