using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorldGen
{
    public static class StringGen
    {
        public static int GA_POPSIZE = 2048;
        public static int GA_MAXITER = 16384;
        public static float GA_ELITRATE = 0.10f;
        public static float GA_MUTATIONRATE = 0.25f;
        public static string GA_TARGET = "My name is Dima! And i love gen AI";


        private static List<PopulationString> populations;
        private static List<PopulationString> populationsBuffer;

        public static List<PopulationString> GetPopulation()
        {
            return populations;
        }

        /// <summary>
        /// Initiate population with start parameters
        /// </summary>
        public static void InitPopulation()
        {
            populationsBuffer = new List<PopulationString>();
            populations = new List<PopulationString>();
            for (int i = 0; i < GA_POPSIZE; i++)
            {
                PopulationString citizen = new PopulationString();
                citizen.Applicability = 0;
                citizen.Contetnt = String.Empty;

                Random random = new Random();


                for (int j = 0; j < GA_TARGET.Length; j++)
                    citizen.Contetnt += ((char)random.Next(97, 122)).ToString();

                populations.Add(citizen);
                populationsBuffer.Add(new PopulationString());
            }
        }

        /// <summary>
        /// Calculate for every population Applicability by counting character difference
        /// </summary>
        public static void CalcApplicability()
        {
            int applicability;

            for (int i = 0; i < GA_POPSIZE; i++)
            {
                applicability = 0;
                for (int j = 0; j < GA_TARGET.Length; j++)
                {
                    applicability += Math.Abs((int)(populations[i].Contetnt[j] - GA_TARGET[j]));
                }

                populations[i].Applicability = applicability;
            }
        }

        public static bool CheckPopulationsApplicability(PopulationString x, PopulationString y)
        {
            return (x.Applicability < y.Applicability);
        }

        public static void Duplicate(int esize)
        {
            for (int i = 0; i < esize; i++)
            {
                populationsBuffer[i].Contetnt = populations[i].Contetnt;
                populationsBuffer[i].Applicability = populations[i].Applicability;
            }
        }

        public static void Mutate(PopulationString member)
        {
            Random random = new Random();
            int tsize = GA_TARGET.Length;
            int ipos = random.Next() % GA_TARGET.Length;
            int delta = random.Next(97, 122);

            char[] chars = member.Contetnt.ToCharArray();
            chars[ipos] = (char)((member.Contetnt[ipos] + delta) % 122);
            member.Contetnt = new string(chars);

        }


        public static void Mate()
        {
            int esize = Convert.ToInt32(GA_POPSIZE * GA_ELITRATE);
            int tsize = GA_TARGET.Length;
            int spos, i1, i2;

            Duplicate(esize);

            // Mate the rest
            for (int i = esize; i < GA_POPSIZE; i++)
            {
                Random random = new Random();
                i1 = random.Next() % (GA_POPSIZE / 2);
                i2 = random.Next() % (GA_POPSIZE / 2);
                spos = random.Next() % tsize;

                populationsBuffer[i].Contetnt = populations[i1].Contetnt.Substring(0, spos) +
                                populations[i2].Contetnt.Substring(spos, tsize - spos);

                if (random.Next() < GA_MUTATIONRATE * random.Next()) Mutate(populationsBuffer[i]);
            }
        }

        public static String ShowBest()
        {
            return $"Best:  {populations[0].Contetnt} ({populations[0].Applicability})";
        }

        public static void SortByApplicability()
        {
            populations.Sort((x, y) => x.Applicability.CompareTo(y.Applicability));
        }

        public static bool CheckPopulationApplicability(int value)
        {
            return populations[0].Applicability == value;
        }

        public static void SwapBuffer()
        {
            var temp = populations;
            populations = populationsBuffer;
            populationsBuffer = temp;
        }

    }

}
