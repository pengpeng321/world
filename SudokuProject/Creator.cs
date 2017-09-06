using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sudoku
{
    public class Creator
    {
        private int[,] a = new int[9, 9];               //二维数组，用来存放整个数独的每一个数
        private byte[,] flag_hang = new byte[9, 10];    //记录每行（共9行）的1-9的出现情况
        private byte[,] flag_lie = new byte[9, 10];     //记录每列（共9列）的1-9的出现情况
        private byte[,] flag_jiu = new byte[9, 10];     //记录每个九宫格（共9个）的1-9的出现情况
        private int over;                               //总共输出几次
        private int count;                              //输出结果的次数
        private Random re = new Random();
        private static string path = "..\\BIN\\output.txt";
        StreamWriter ws = new StreamWriter(path);

        public Creator(int temp)                        //构造函数，初始化
        {
            over = temp;
            a[0, 0] = 3;
            flag_hang[0, 3] = 1;
            flag_lie[0, 3] = 1;
            flag_jiu[0, 3] = 1;
            F(2);
        }                                               

        private void F(int p)
        {
            int x, y, i, j, temp;
            if (count == over)                          
            {
                ws.Close();
                return;
            }
            if (p == 82)
            {
                for (i = 0; i < 9; i++)
                {
                    for (j = 0; j < 9; j++)
                    {
                        ws.Write(a[i, j]);
                        ws.Write(' ');
                    }
                    ws.WriteLine();
                }
                ws.WriteLine();
                count++;

                return;
            }
            x = (p - 1) % 9;
            y = (p - 1) / 9;
            
            i = re.Next(1, 100);
            for (j = 1; j < 10; j++)
            {
                i = (i + 1) % 9 + 1;
                i = j;
                if (flag_hang[y, i] == 1) continue;
                if (flag_lie[x, i] == 1) continue;
                temp = (x / 3) + (y / 3) * 3;
                if (flag_jiu[temp, i] == 1) continue;
                else
                {
                    a[x, y] = i;
                    flag_lie[x, i] = 1;
                    flag_hang[y, i] = 1;
                    flag_jiu[temp, i] = 1;
                    F(p + 1);
                    flag_lie[x, i] = 0;
                    flag_hang[y, i] = 0;
                    flag_jiu[temp, i] = 0;
                }

            }
        }
    }
}
