using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelloWorldGen
{
	//https://habr.com/ru/post/100990/
	class Program
    {
        static void Main(string[] args)
        { 

			StringGen.InitPopulation();
			string temp = string.Empty;
			for (int i = 0; i < StringGen.GA_MAXITER; i++)
			{
				StringGen.CalcApplicability();      // вычисляем пригодность
				StringGen.SortByApplicability();       // сортируем популяцию 

				 
				Console.WriteLine($"Interation({i}) {StringGen.ShowBest()}");

				if (StringGen.CheckPopulationApplicability(0)) 
					break;

				StringGen.Mate();     // спариваем популяции
				StringGen.SwapBuffer();       // очищаем буферы
			}

			Console.ReadKey();

		}
    }
}
