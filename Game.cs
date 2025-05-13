using System;

namespace DetectiveGame
{
    class Game
    {
        private CaseManager caseManager;

        public void Start()
        {
            Console.WriteLine("�i���� � ��i '��i����� ���� ��������'.");
            Console.WriteLine("������i�� ����-��� ����i��, ��� ������...");
            Console.ReadKey();
            Console.WriteLine();

            caseManager = new CaseManager();
            caseManager.InitializeCases();
            caseManager.MainLoop();
        }
    }
}