using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Links
{
    public class Circular : LinkGenerator
    {
        public float range = 100f;  //Larger for more random off circle

        public override List<MazeLink> Generate(int rows, int cols)
        {
            List<MazeLink> result = new List<MazeLink>(rows*cols*2);

            float ri = -range;
            float rj = (ri * rows) / cols;

            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    float a = Mathf.Atan2((i - rows * 0.5f), (j - cols * 0.5f));
                    float wi = Random.Range(ri, Mathf.Abs(Mathf.Cos(a) * 100.0f));
                    float wj = Random.Range(rj, Mathf.Abs(Mathf.Sin(a) * 100.0f));

                    if (i > 0) result.Add(new MazeLink {row=i, col=j, dir=2, weight=wi});
                    if (j > 0) result.Add(new MazeLink {row=i, col=j, dir=3, weight=wj});
                }
            }

            return result;
        }
    }
}
