using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazes
{
    public class Backtrack : MazeGenerator
    {
        private int vc;
        Stack<MazeCell> path;
        MazeCell current;

        protected override IEnumerator Generate()
        {
            current = RandomCell();

            vc = 0;
            int total = grid.cellCount;
            path = new Stack<MazeCell>();

            while(vc < total)
            {
                foreach(var v in RandomWalk(total))
                {
                    yield return v;
                }

                if (vc < total)
                {
                    foreach(var v in BacktrackPath())
                    {
                        yield return v;
                    }
                }
            }

            SetComplete();
        }

        private IEnumerable BacktrackPath()
        {
            current = null;

            while((current == null) && (path.Count > 0))
            {
                MazeCell c = path.Pop();

                int k = RandomDirToLocked(c);
                SetFocus(c);

                if (k >= 0)
                {
                    path.Push(c);
                    grid[c.row, c.col][k] = true;
                    current = c.next[k];
                }

                yield return OnYield();
            }
        }

        private IEnumerable RandomWalk(int total)
        {
            vc++;

            while(vc < total)
            {
                //Pick direction
                int k = RandomDirToLocked(current);
                if (k < 0) break;

                path.Push(current);
                grid[current.row, current.col][k] = true;
                current = current.next[k];
                SetFocus(current);
                vc++;

                yield return OnYield();
            }
        }
    }
}
