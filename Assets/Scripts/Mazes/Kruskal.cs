using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazes
{
    public class Kruskal : MazeGenerator
    {
        public LinkGenerator linkGenerator;

        private List<(int,int)>[] sets;
        private List<MazeLink> links;
        private int[,] setIndex;

        protected override IEnumerator Generate()
        {
            BuildSets();
            links = linkGenerator.Generate(grid.rows, grid.cols);
            links.Sort((a,b)=>a.weight.CompareTo(b.weight));

            foreach(MazeLink link in links)
            {
                int i0 = link.row;
                int j0 = link.col;
                int k = link.dir;

                MazeCell cell = grid[i0+1,j0+1];
                MazeCell next = cell.next[k];
                int i1 = next.row-1;
                int j1 = next.col-1;

                int s1 = setIndex[i0,j0];
                int s2 = setIndex[i1,j1];

                if (s1 != s2)
                {
                    cell[k] = true;

                    SetFocus(cell);
                    if (sets[s1].Count > sets[s2].Count) MergeSets(s1, s2);
                    else MergeSets(s2, s1);
                    yield return OnYield(2);
                }
            }

            SetComplete();
        }

        private void MergeSets(int s1, int s2)
        {
            foreach((int i,int j) in sets[s2])
            {
                sets[s1].Add((i,j));
                setIndex[i,j] = s1;
                AddFocus(grid[i+1,j+1]);
            }

            sets[s2].Clear();
        }

        private void BuildSets()
        {
            sets = new List<(int,int)>[grid.cellCount];
            setIndex = new int[grid.rows, grid.cols];

            int h = 0;
            for(int i = 0; i < grid.rows; i++)
            {
                for (int j = 0; j < grid.cols; j++, h++)
                {
                    sets[h] = new List<(int,int)>(grid.cellCount);
                    sets[h].Add((i,j));
                    setIndex[i,j] = h;
                }
            }
        }
    }
}
