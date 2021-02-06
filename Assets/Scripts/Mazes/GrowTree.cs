using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazes
{
    public class GrowTree : MazeGenerator
    {
        public LinkGenerator linkGenerator;
        public float decay = 1.0f;

        private List<MazeLink> links;
        private List<MazeLink> pick;
        
        protected override IEnumerator Generate()
        {
            links = linkGenerator.Generate(grid.rows, grid.cols);
            pick = new List<MazeLink>(grid.cellCount);

            MazeCell current = RandomCell();
            AddLinks(current);
            SetFocus(current);
            yield return OnYield(2);

            while(pick.Count > 0)
            {
                MazeLink link = PickLink();
                MazeCell from = grid[link.row+1, link.col+1];
                MazeCell to = from.next[link.dir];

                if (from.isLocked || to.isLocked)
                {
                    current = (from.isLocked) ? from : to;
                    from[link.dir] = true;
                    AddLinks(current);
                    SetFocus(current);
                    yield return OnYield();
                }
            }

            SetComplete();
        }

        protected void AddLinks(MazeCell c)
        {
            System.Predicate<MazeLink> finder = 
                ((a) => ((a.row == c.row-1) && (a.col == c.col-1))
                  ||  ((a.row == c.row) && (a.col == c.col-1) && (a.dir == 2))
                  ||  ((a.row == c.row-1) && (a.col == c.col) && (a.dir == 3))
                );
            var add = links.FindAll(finder);
            links.RemoveAll(finder);

            pick.AddRange(add);
        }

        protected MazeLink PickLink()
        {
            foreach(MazeLink link in pick)
            {
                link.weight += decay;
            }

            pick.Sort((a,b) => (a.weight.CompareTo(b.weight)));
            MazeLink result = pick[0];
            pick.RemoveAt(0);

            return result;
        }
    }
}
