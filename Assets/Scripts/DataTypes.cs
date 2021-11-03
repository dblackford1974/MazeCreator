using System.Collections.Generic;

public class MazeLink
{
    public int row;
    public int col;
    public int dir;
    public float weight;
}

public struct GridFocus
{
    bool multi;
    (int,int) focus;
    List<(int,int)> list;

    public void Init(int count)
    {
        list = new List<(int,int)>(count);            
    }

    public void Set(int i, int j)
    {
        list.Clear();
        multi = false;
        focus = (i,j);
    }

    public void Add(int i, int j)
    {
        if (!multi)
        {
            list.Add(focus);
            multi = true;
        }

        list.Add((i, j));
    }

    public bool IsFocus(int i, int j)
    {
        if (!multi)
        {
            return ((i,j) == focus);
        }
        else
        {
            return (list.IndexOf((i,j)) >= 0);
        }
    }
}
