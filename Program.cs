using System;
using System.Diagnostics;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Console.Write("Размерность матриц (n*n): ");
            int n = Int32.Parse(Console.ReadLine());

            int[,] matrix1 = new int[n, n]; // 1я матрица
            int[,] matrix2 = new int[n, n]; // 2я матрица
            int[,] matrix3 = new int[n, n]; // результат многопоточного перемножения
            int[,] matrix4 = new int[n, n]; // результат однопоточного перемножения
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix1[i, j] = random.Next(10, 50);
                    matrix2[i, j] = random.Next(10, 50);
                }
            }
            Console.WriteLine("Многопоточное перемножение:");
            var sw1 = new Stopwatch();
            sw1.Start();
            Thread thread1 = new Thread(() => MultParallel(matrix1,matrix2,matrix3,0,n/4));
            Thread thread2 =new Thread(() => MultParallel(matrix1,matrix2,matrix3,n/4+1,n/2));
            Thread thread3 =new Thread(() => MultParallel(matrix1,matrix2,matrix3,n/2+1,n-n/4));
            Thread thread4 =new Thread(() => MultParallel(matrix1,matrix2,matrix3,n-n/4+1,n));
            
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            
            if (thread1.IsAlive) // блокирова потоков по завершении их работы
                thread1.Join();
            if (thread2.IsAlive)
                thread2.Join();
            if (thread3.IsAlive)
                thread3.Join();
            if (thread4.IsAlive)
                thread4.Join();
            
            sw1.Stop();
            Console.WriteLine("sw="+sw1.ElapsedMilliseconds+"ms");
            
            Console.WriteLine("Однопоточное перемножениея:");
            var sw2 = new Stopwatch();
            sw2.Start();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        matrix4[i, j] = (matrix4[i, j] + matrix1[i, k] + matrix2[k, j]);
                    }
                }
            }
            sw2.Stop();
            Console.WriteLine("sw="+sw2.ElapsedMilliseconds+"ms");
        }

        static void MultParallel(int[,] matrix1, int[,] matrix2, int[,] matrix3, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < matrix1.GetLength(0); j++)
                {
                    for (int k = 0; k < matrix1.GetLength(0); k++)
                    {
                        matrix3[i, j] += matrix1[i, k] + matrix2[k, j];
                    }
                }
            }
            
        }
    }
}