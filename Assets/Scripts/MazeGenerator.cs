using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Topics:
//  Braiding
//  6 algs
//    Kruskal
//    Simple Prim's
//    Full Prim's
//    Growing Tree
//    Eller
//    Recursive Division

//Evaluation:
//  Longest path
//  Number of branches encountered on longest path
//    Or as part of path distance
//  Dead end count
//    Length of dead end (since prior branch)

//Options:
//  Weaving - (Doors?)
//    Door only if sufficient 'distance' between sides of door
//    Otherwise either leave dead end or clear wall
//      If low distance, clear (or keep) wall
//      If mid distance, normal door
//      If large distance, locked door (lockpick) or keep wall
//  Game
//      Torches (or light spell)
//      Lockpicks
//      Mapping Gems
//  Party
//      Paper dolls?
//      All actions have "all action" recovery + "same action" recovery (opt. "other action" recovery)
//      Human Barbarian (from Half-Elf)
//          Attack, Block, Lunge, [Rage-Timer+Portrait]
//      Gnome Monk
//          Attack, Evade, Stun,  [Fear-Timer+Gothmog]
//      Elf Druid
//          Spear(Delay Snipe), Shoot(Delay Spear), Heal Party(auto-distribute), Cure/Bless
//      Dwarf Sorcerer
//          Staff(Melee), Spark(Short), Fireball(Long,else hurt party), Flash(Mass stun, include party but can move/recharge) 
//  Rooms

abstract public class MazeGenerator : MonoBehaviour
{
    protected float cellDelay = 0.04f;

    protected MazeGrid grid;
    
    public void Generate(MazeGrid _grid)
    {
        grid = _grid;
        StartCoroutine(Generate());
    }

    protected abstract IEnumerator Generate();

    protected YieldInstruction OnYield(int count = 1)
    {
        return new WaitForSeconds(cellDelay * count);
    }

    protected void SetComplete()
    {
        grid.focus.Set(0,0);
        Debug.Log("Done!");
    }

    protected void SetFocus(MazeCell c)
    {
        grid.focus.Set(c.row, c.col);
    }

    protected void AddFocus(MazeCell c)
    {
        grid.focus.Add(c.row, c.col);
    }

    protected MazeCell RandomCell()
    {
        (int i, int j) = RandomIndex();

        return grid[i, j];
    }

    protected (int, int) RandomIndex()
    {
        int i = Random.Range(0, grid.rows) + 1;
        int j = Random.Range(0, grid.cols) + 1;

        return (i, j);
    }

    protected int RandomDir(MazeCell c)
    {
        int k = Random.Range(0, 4);
        k = grid.DeflectDir(c.row, c.col, k);

        return k;
    }

    //-1 if no option
    protected int RandomDirToLocked(MazeCell c, bool isLocked = true)
    {
        (int[] d, int r) = c.GetLocked(isLocked);

        if (r == 0) return -1;

        int k = Random.Range(0, r);

        return d[k];
    }

    protected List<(int,int)> AllIndices()
    {
        var result = new List<(int,int)>(grid.rows * grid.cols);

        //Add all unvisited indices
        for(int i = 0; i < grid.rows; i++)
        {
            for(int j = 0; j < grid.cols; j++)
            {
                result.Add((i+1,j+1));
            }
        }

        return result;
    }
}
