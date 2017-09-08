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
        private const int X1 = 0;                       //在行行变换，列列变换所标记的9个小九宫格的边界
        private const int X2 = 3;
        private const int X3 = 6;
        private const int Y1 = 0;
        private const int Y2 = 3;
        private const int Y3 = 6;

        private const int FAST_MAX = 645100;            //使用行行变换，列列变换所能达到的最大不同个数的数独
        private byte[,] a = new byte[9, 9];             //二维数组，用来存放整个数独的每一个数
        private byte[,] flag_hang = new byte[9, 10];    //记录每行（共9行）的1-9的出现情况
        private byte[,] flag_lie = new byte[9, 10];     //记录每列（共9列）的1-9的出现情况
        private byte[,] flag_jiu = new byte[9, 10];     //记录每个九宫格（共9个）的1-9的出现情况
        private int over;                               //总共输出几次
        private int count;                              //输出结果的次数
        private Random re = new Random();
        private List<byte> mylist = new List<byte>();
        private static string path = "..\\sudoku.txt";
        StreamWriter ws = new StreamWriter(path);
        public Creator(int temp)                        //构造函数，初始化
        {
            over = temp;
            count = 0;
            a[0, 0] = 3;
            Begin();
        }
        public void Begin()                             //开始寻找数独
        {
            //分两种方法寻找数独
            //行行变换，列列变换寻找  优点：快，                    缺点：只能生成FAST_MAX = 645100个不同的数组
            //通过回溯寻找，          优点：可以生成很多很多的数独，缺点：慢

            for (int i = 1; i < 10; i++)                               
            {
                //初始化mylist 
                if (i == 3) continue;
                mylist.Add((byte)i);
            }
            FindFirst(2);                               //使用行行变换，列列变换寻找        
            if (count < over)                           //当行行变换，列列变换寻找达不到要求时使用回溯
            {
                int temp, j, jj;
                for (j = 0; j < 9; j++)
                {
                    //初始化flag
                    for (jj = 0; jj < 9; jj++)
                    {
                        temp = (j / 3) + (jj / 3) * 3;
                        if (j < 3 && jj < 3)
                        {
                            flag_hang[jj, a[j, jj]] = 1;
                            flag_lie[j, a[j, jj]] = 1;
                            flag_jiu[temp, a[j, jj]] = 1;
                        }
                        else
                        {
                            flag_hang[jj, a[j, jj]] = 0;
                            flag_lie[j, a[j, jj]] = 0;
                            flag_jiu[temp, a[j, jj]] = 0;
                        }
                    }
                }
                Dfs(4);
            }
            ws.Close();
        }
        public void FindFirst(int p)                    //寻找第一个小九宫格
        {
            int x, y, i, j, rand;
            if (count == over) return;
            if (p == 10)
            {
                FindAll();
                return;
            }
            x = (p - 1) % 3;
            y = (p - 1) / 3;
            rand = re.Next(1, 100);
            for (j = 0; j < mylist.Count(); j++)
            {
                i = (j + rand) % mylist.Count();
                a[x, y] = mylist[i];
                mylist.RemoveAt(i);
                FindFirst(p + 1);
                mylist.Insert(i, a[x, y]);
            }
        }
        public void FindAll()                           //找到剩下的8个九宫格
        {
            //采用行行变换，列列变换
            int i,q,w,e,r;
            if (count == over) return;
            for (q = 0; q < 2; q++)
            {
                //第2、3个九宫格变换
                for (i = 0; i < 3; i++)
                {
                    a[X2 + i, Y1 + 0] = a[X1 + i, Y1 + ((q == 0) ? 1 : 2)];         //2
                    a[X2 + i, Y1 + 1] = a[X1 + i, Y1 + ((q == 0) ? 2 : 0)];         //2
                    a[X2 + i, Y1 + 2] = a[X1 + i, Y1 + ((q == 0) ? 0 : 1)];         //2
                    a[X3 + i, Y1 + 0] = a[X1 + i, Y1 + ((q == 0) ? 2 : 1)];         //3
                    a[X3 + i, Y1 + 1] = a[X1 + i, Y1 + ((q == 0) ? 0 : 2)];         //3
                    a[X3 + i, Y1 + 2] = a[X1 + i, Y1 + ((q == 0) ? 1 : 0)];         //3
                }
                for (w = 0; w < 2; w++)
                {
                    //第4、7个九宫格变换
                    for (i = 0; i < 3; i++)
                    {
                        a[X1 + 0, Y2 + i] = a[X1 + ((w == 0) ? 2 : 1), Y1 + i];         //4
                        a[X1 + 1, Y2 + i] = a[X1 + ((w == 0) ? 0 : 2), Y1 + i];         //4
                        a[X1 + 2, Y2 + i] = a[X1 + ((w == 0) ? 1 : 0), Y1 + i];         //4
                        a[X1 + 0, Y3 + i] = a[X1 + ((w == 0) ? 1 : 2), Y1 + i];         //7
                        a[X1 + 1, Y3 + i] = a[X1 + ((w == 0) ? 2 : 0), Y1 + i];         //7
                        a[X1 + 2, Y3 + i] = a[X1 + ((w == 0) ? 0 : 1), Y1 + i];         //7
                    }
                    for (e = 0; e < 2; e++)
                    {
                        //第5、8个九宫格变换
                        for (i = 0; i < 3; i++)
                        {
                            a[X2 + 0, Y2 + i] = a[X2 + ((e == 0) ? 2 : 1), Y1 + i];         //5
                            a[X2 + 1, Y2 + i] = a[X2 + ((e == 0) ? 0 : 2), Y1 + i];         //5
                            a[X2 + 2, Y2 + i] = a[X2 + ((e == 0) ? 1 : 0), Y1 + i];         //5
                            a[X2 + 0, Y3 + i] = a[X2 + ((e == 0) ? 1 : 2), Y1 + i];         //8
                            a[X2 + 1, Y3 + i] = a[X2 + ((e == 0) ? 2 : 0), Y1 + i];         //8
                            a[X2 + 2, Y3 + i] = a[X2 + ((e == 0) ? 0 : 1), Y1 + i];         //8
                        }
                        for (r = 0; r < 2; r++)
                        {
                            //第6、9个九宫格变换
                            for (i = 0; i < 3; i++)
                            {
                                a[X3 + 0, Y2 + i] = a[X3 + ((r == 0) ? 2 : 1), Y1 + i];         //6
                                a[X3 + 1, Y2 + i] = a[X3 + ((r == 0) ? 0 : 2), Y1 + i];         //6
                                a[X3 + 2, Y2 + i] = a[X3 + ((r == 0) ? 1 : 0), Y1 + i];         //6
                                a[X3 + 0, Y3 + i] = a[X3 + ((r == 0) ? 1 : 2), Y1 + i];         //9
                                a[X3 + 1, Y3 + i] = a[X3 + ((r == 0) ? 2 : 0), Y1 + i];         //9
                                a[X3 + 2, Y3 + i] = a[X3 + ((r == 0) ? 0 : 1), Y1 + i];         //9
                            }
                            if (count == over) return;
                            if (count < FAST_MAX) Write();  //理论上有645120个数独，为了使用回溯时避免重复，最后一组16个数独不用
                            if (count == over) return;
                        }
                        if (count == over) return;
                    }
                    if (count == over) return;
                }
                if (count == over) return;
            }
        }
        private void Dfs(int p)                         //回溯寻找数独
        {

            int x, y, i, j, temp,rand;
            if (p == 1 || p == 2 || p == 3 || p == 10 || p == 11 || p == 12 || p == 19 || p == 20 || p == 21)
            {
                Dfs(p + 1);
                return;
            }
            if (count == over)  return;
            if (p == 82)
            {
                Write();
                return;
            }
            x = (p - 1) % 9;
            y = (p - 1) / 9;
            rand = re.Next(1, 100);
            for (j = 0; j < 10; j++)
            {
                i = (rand + j) % 9 + 1;
                if (flag_hang[y, i] == 1) continue;
                if (flag_lie[x, i] == 1) continue;
                temp = (x / 3) + (y / 3) * 3;
                if (flag_jiu[temp, i] == 1) continue;
                else
                {
                    a[x, y] = (byte)i;
                    flag_lie[x, i] = 1;
                    flag_hang[y, i] = 1;
                    flag_jiu[temp, i] = 1;
                    Dfs(p + 1);
                    flag_lie[x, i] = 0;
                    flag_hang[y, i] = 0;
                    flag_jiu[temp, i] = 0;
                }
            }
        }
        private void Write()                            //写
        {
            int i, j;
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
        }
    }
}
