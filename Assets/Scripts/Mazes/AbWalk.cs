using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazes
{
    public class AbWalk : MazeGenerator
    {
        protected override IEnumerator Generate()
        {
            const int maxIPerCell = 100;

            int vc = 1;
            int ic = 0;
            int total = grid.cellCount;
            int maxI = total * maxIPerCell;
            MazeCell c = RandomCell();

            while((vc < total) && (ic < maxI))
            {
                //Pick direction
                int k = RandomDir(c);

                MazeCell d = c.next[k];

                if (d.isLocked)
                {
                    SetFocus(d);
                    yield return OnYield(2);
                    c[k] = true;
                    vc++;
                }

                c = d;
                ic++;

                if ((ic % 4) == 0)
                {
                    SetFocus(c);
                    yield return OnYield();
                }
            }

            SetComplete();
        }
    }
}
