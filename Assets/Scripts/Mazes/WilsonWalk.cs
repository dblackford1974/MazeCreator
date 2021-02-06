using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazes
{
    public class WilsonWalk : MazeGenerator
    {
        public int initVisitCount = 1;

        private bool[,] visited;
        private List<(int,int)> unvisited;
        private List<(int,int)> loop;
        private List<(int,int,int)> path;

        private int ic;

        protected override IEnumerator Generate()
        {
            const int maxIPerCell = 100;

            int vc = initVisitCount;
            int rows = grid.rows;
            int cols = grid.cols;
            int total = rows * cols;
            int maxI = total * maxIPerCell;
            visited = new bool[rows, cols];
            unvisited = AllIndices();
            loop = new List<(int,int)>(total);
            path = new List<(int,int,int)>(total);

            //Set random cells to visited
            for (int h = 0; h < initVisitCount; h++)
            {
                (int i, int j) = RandomIndex();
                visited[i-1,j-1] = true;
                unvisited.Remove((i,j));
            }

            while(vc < total)
            {
                foreach(var v in RandomWalk(maxI))
                {
                    yield return v;
                }

                if (ic == maxI) break;

                //Add path, increment vc
                foreach(var v in UpdatePath())
                {
                    yield return v;
                }
                vc += path.Count;

                yield return OnYield();
            }

            SetComplete();
        }

        private IEnumerable UpdatePath()
        {
            foreach((int i, int j, int k) in path)
            {
                visited[i-1,j-1] = true;
                grid[i,j][k] = true;
                unvisited.Remove((i,j));

                SetFocus(grid[i,j]);
                yield return OnYield();
            }
        }

        private IEnumerable RandomWalk(int maxI)
        {
            ic = 0;
            loop.Clear();
            path.Clear();

            int r = Random.Range(0, unvisited.Count);
            (int i, int j) = unvisited[r];
            MazeCell c = grid[i, j];
            
            while(ic < maxI)
            {
                //Pick direction
                int k = RandomDir(c);

                loop.Add((i,j));
                path.Add((i,j,k));

                MazeCell d = c.next[k];
                (i,j) = (d.row, d.col);

                if (visited[i-1,j-1])
                {
                    break;
                }
                else
                {
                    int h = loop.IndexOf((i,j));
                    
                    //Remove loop
                    if (h >= 0)
                    {
                        loop.RemoveRange(h, loop.Count - h);
                        path.RemoveRange(h, path.Count - h);
                        SetFocus(d);
                        yield return OnYield();
                    }
                }
                
                c = d;
                ic++;
            }
        }
    }
}
