using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Services
{
    public class RandomGenerator
    {
        public static string GetRandomString(int lenght)
        {
            //97-122 a-z
            //65-90 A-Z
            //48-57 0-9
            Random rand = new Random();
            StringBuilder randstr = new StringBuilder("");
            for (int i = 0; i < lenght; i++)
            {
                switch (rand.Next(1, 4))
                {
                    case 1:
                        randstr.Append((char)rand.Next(48, 58));
                        break;
                    case 2:
                        randstr.Append((char)rand.Next(65, 91));
                        break;
                    case 3:
                        randstr.Append((char)rand.Next(97, 123));
                        break;
                }
            }
            return randstr.ToString();
        }
    }
}
