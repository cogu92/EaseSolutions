using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EaseSolutions
{
    class Program
    {
        private static int[][] map;
        private static int row;
        private static int column;
        private static int[][] path;
        private static int[][] drop;
        static void Main(string[] args)
        {

            Console.WriteLine("please insert the path of youre file");
            string fileName = Console.ReadLine();
            //@"C:\Users\Nicolas\Source\Repos\EaseSolutions\EaseSolutions\EaseSolutions\txt\map.txt";
            loadData(fileName);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    int[] tmp = serchDrop(i, j);
                    path[i][j] = tmp[0];
                    drop[i][j] = map[i][j] - map[tmp[1]][tmp[2]];
                }
            }


            int[] upVectors = new int[2];
            int pathUp = -1;
            int dropUp = -1;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (path[i][j] > pathUp)
                    {
                        pathUp = path[i][j];
                        dropUp = drop[i][j];
                        upVectors[0] = i;
                        upVectors[1] = j;
                    }
                    if (path[i][j] == pathUp)
                    {
                        if (drop[i][j] > dropUp)
                        {
                            dropUp = drop[i][j];
                            upVectors[0] = i;
                            upVectors[1] = j;
                        }
                    }
                }
            }


            List<int> list = serchPath(upVectors[0], upVectors[1]);
            list.Reverse();
            Console.WriteLine("The  routes is: ");
            int m = 0;
            int[] vectors = new int[2];
            foreach (int tmp in list)
            {
                vectors[m % 2] = tmp;
                m++;
                if (m % 2 == 0)
                    Console.WriteLine(map[vectors[0]][vectors[1]]);
            }
            Console.WriteLine("Path: " + pathUp);
            Console.WriteLine( "Drop: " + dropUp);
            Console.Read();
        }

        public static List<int> serchPath(int vectoX, int vectorY)
        {
            ArrayList list = new ArrayList();
            ArrayList current = new ArrayList();
            if (vectorY > 0 && map[vectoX][vectorY] > map[vectoX][vectorY - 1])
            {
                current = new ArrayList(serchPath(vectoX, vectorY - 1));
                if (current.Count > list.Count)
                    list = current;
            }
            if (vectorY < (column - 1) && map[vectoX][vectorY] > map[vectoX][vectorY + 1])
            {
                current = new ArrayList(serchPath(vectoX, vectorY + 1));
                if (current.Count > list.Count)
                    list = current;
            }
            if (vectoX > 0 && map[vectoX][vectorY] > map[vectoX - 1][vectorY])
            {
                current = new ArrayList(serchPath(vectoX - 1, vectorY));
                if (current.Count > list.Count)
                    list = current;
            }
            if (vectoX < (row - 1) && map[vectoX][vectorY] > map[vectoX + 1][vectorY])
            {
                current = new ArrayList(serchPath(vectoX + 1, vectorY));
                if (current.Count > list.Count)
                    list = current;
            }
            list.Add(vectorY);
            list.Add(vectoX);
            return list.Cast<int>().ToList();
        }

        public static int[] serchDrop(int i, int j)
        {
            int[] pathAndDrop = { 0, i, j };
            int[] currentDrop = new int[2];
            if (j > 0 && map[i][j] > map[i][j - 1])
            {
                currentDrop = serchDrop(i, j - 1);
                if (currentDrop[0] > pathAndDrop[0])
                    Array.Copy(currentDrop, pathAndDrop, currentDrop.Length);
            }
            if (j < (column - 1) && map[i][j] > map[i][j + 1])
            {
                currentDrop = serchDrop(i, j + 1);
                if (currentDrop[0] > pathAndDrop[0])
                    Array.Copy(currentDrop, pathAndDrop, currentDrop.Length);
            }
            if (i > 0 && map[i][j] > map[i - 1][j])
            {
                currentDrop = serchDrop(i - 1, j);
                if (currentDrop[0] > pathAndDrop[0])
                    Array.Copy(currentDrop, pathAndDrop, currentDrop.Length);
            }
            if (i < (row - 1) && map[i][j] > map[i + 1][j])
            {
                currentDrop = serchDrop(i + 1, j);
                if (currentDrop[0] > pathAndDrop[0])
                    Array.Copy(currentDrop, pathAndDrop, currentDrop.Length);
            }
            pathAndDrop[0]++;
            return pathAndDrop;
        }

        public static void loadData(String filePath)
        {
            try
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    String line;
                    if ((line = sr.ReadLine()) != null)
                    {
                        String[] str = line.Split(' ');
                        row = Convert.ToInt32(str[0]);
                        column = Convert.ToInt32(str[1]);
                        map = CreateArray<int>(row, column);
                        path = CreateArray<int>(row, column);
                        drop = CreateArray<int>(row, column);
                    }
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        int j = 0;
                        String[] str = line.Split(' ');
                        foreach (String s in str)
                        {
                            map[i][j++] = Convert.ToInt32(s);
                        }
                        i++;
                    }

                }
            }
            catch
            {
                Console.WriteLine("Please Check your path and file name");
                Console.Read();
            }
        }

        static T[][] CreateArray<T>(int rows, int cols)
        {
            T[][] array = new T[rows][];
            for (int i = 0; i < array.GetLength(0); i++)
                array[i] = new T[cols];

            return array;
        }
    }
}
