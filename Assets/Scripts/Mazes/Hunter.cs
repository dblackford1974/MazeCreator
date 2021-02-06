using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazes
{
    public class Hunter : MazeGenerator
    {
        public bool reverseY;
        public bool reverseX;
        public bool toggleReverse;  //'Hunt-and-Release'

        private int vc;

        protected override IEnumerator Generate()
        {
            MazeCell start = RandomCell();

            vc = 0;
            int total = grid.cellCount;

            while(vc < total)
            {
                foreach(var v in RandomWalk(start, total))
                {
                    yield return v;
                }

                if (vc < total)
                {
                    IEnumerable e = grid.LockedCells(reverseX, reverseY);
                    if(toggleReverse)
                    {
                        reverseY = !reverseY;
                        if (reverseY) reverseX = !reverseX;
                    }

                    foreach(MazeCell c in e)
                    {
                        int k = RandomDirToLocked(c, false);
                        SetFocus(c);
                        yield return OnYield();

                        if (k >= 0)
                        {
                            grid[c.row, c.col][k] = true;
                            start = c;
                            break;
                        }
                    }

                    yield return OnYield();
                }
            }

            SetComplete();
        }

        private IEnumerable RandomWalk(MazeCell c, int total)
        {
            vc++;

            while(vc < total)
            {
                //Pick direction
                int k = RandomDirToLocked(c);
                if (k < 0) break;

                grid[c.row, c.col][k] = true;
                c = c.next[k];
                SetFocus(c);
                vc++;

                yield return OnYield();
            }
        }
    }
}
