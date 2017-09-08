using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string c1 = args[0];                    //判断第一个参数是否有误
            if (c1 != "-c")
            {
                Console.WriteLine("第一个参数输入有误，应输入的是\"-c1\"，请重新操作！");
                return;
            }

            char[] c2 = args[1].ToCharArray();      //判断第二个参数是否有误
            foreach (char cc in c2)
            {
                if (cc > '9' || cc < '0')
                {
                    Console.WriteLine("第二个参数输入有误，应输入的是数字，请重新操作！");
                    return;
                }
            }

            int temp = Convert.ToInt32(args[1]);
            Creator a = new Creator(temp);
        }
    }
}
