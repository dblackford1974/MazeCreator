using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazes
{
    public class Sidewinder : MazeGenerator
    {
        public float runWeight = 0.33f;

        protected override IEnumerator Generate()
        {
            for(int i = 1; i < grid.rows; i++)
            {
                foreach(var v in SetRow(i, grid.cols))
                {
                    yield return v;
                }
            }

            SetLastRow(grid.rows, grid.cols);

            SetComplete();
        }

        private IEnumerable SetRow(int row, int count)
        {
            int runStart = 1;
            int runEnd = 2;

            for (int i = 2; i <= count; i++)
            {
                float r = Random.Range(0.0f, 1.0f);
                
                if (r < runWeight)
                {
                    SetRun(row, runStart, runEnd);
                    runStart = i;
                    runEnd = i;

                    yield return OnYield(2);
                }
                else
                {
                    grid[row, i][3] = true;
                    SetFocus(grid[row, i]);
                    yield return OnYield();
                }

                runEnd++;
            }

            SetRun(row, runStart, runEnd);
        }

        private void SetRun(int row, int start, int end)
        {
            int j = Random.Range(start, end);
            grid[row, j][0] = true;

            SetFocus(grid[row, j]);
        }

        private void SetLastRow(int row, int count)
        {
            for (int i = 1; i < count; i++)
            {
                grid[row, i][1] = true;
            }
        }
    }
}
