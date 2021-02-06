using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazes
{
    public class Eller : MazeGenerator
    {
        protected override IEnumerator Generate()
        {
            yield return OnYield();

            SetComplete();
        }
    }
}
