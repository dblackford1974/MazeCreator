using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    const int dirs = 4;

    //Row and col are set to 0 for padding cells
    public int row;
    public int col;

    public bool[] pass;
    public MazeCell[] next;

    public MazeCell(int _row, int _col)
    {
        row = _row;
        col = _col;
        pass = new bool[dirs];
        next = new MazeCell[dirs];
    }

    public (int, int) index
    {
        get {return (row, col);}
    }

    public bool isLocked
    {
        get => (!(pass[0] || pass[1] || pass[2] || pass[3]));
    }

    public bool isPadding
    {
        get => (row == 0);
    }

    public bool LastRow(MazeGrid grid)
    {
        return (row == grid.rows);
    }

    public bool LastCol(MazeGrid grid)
    {
        return (col == grid.cols);
    }

    public (int[], int) GetLocked(bool _isLocked)
    {
        int[] result = new int[4];
        int c = 0;

        for(int i = 0; i < 4; i++)
        {
            MazeCell d = next[i];
            if (!d.isPadding && (d.isLocked == _isLocked))
            {
                result[c++] = i;
            }
        }

        return (result, c);
    }

    public bool this[int dir]
    {
        get => pass[dir];
        set => SetPass(dir, value);
    }

    public void SetPass(int dir, bool _pass)
    {
        _SetPass(dir, _pass);
        next[dir]._SetPass((dir+2)%4, _pass);
    }

    public void SetNextDown(MazeCell next)
    {
        _SetNext(2, next);
        next._SetNext(0, this);
    }

    public void SetNextLeft(MazeCell next)
    {
        _SetNext(3, next);
        next._SetNext(1, this);
    }

    private void _SetPass(int dir, bool _pass)
    {
        pass[dir] = _pass;
    }

    private void _SetNext(int dir, MazeCell _next)
    {
        next[dir] = _next;
    }
}

