using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Links
{
    public class Uniform : LinkGenerator
    {
        public override List<MazeLink> Generate(int rows, int cols)
        {
            List<MazeLink> result = new List<MazeLink>(rows*cols*2);

            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    float wi = Random.Range(0f, 100f);
                    float wj = Random.Range(0f, 100f);

                    if (i > 0) result.Add(new MazeLink {row=i, col=j, dir=2, weight=wi});
                    if (j > 0) result.Add(new MazeLink {row=i, col=j, dir=3, weight=wj});
                }
            }

            return result;
        }
    }
}
