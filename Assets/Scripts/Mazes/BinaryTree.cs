using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazes
{
    public class BinaryTree : MazeGenerator
    {
        protected override IEnumerator Generate()
        {
            foreach(MazeCell c in grid.Cells())
            {
                if (c.LastRow(grid))
                {
                    if (!c.LastCol(grid)) c[1] = true;
                }
                else if (c.LastCol(grid))
                {
                    c[0] = true;
                }
                else
                {
                    int i = Random.Range(0, 2);
                    c[i] = true;
                }
    
                SetFocus(c);

                yield return OnYield();
            }

            SetComplete();
        }
    }
}
