using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class MazeGrid : MonoBehaviour
{
    public MazeGenerator generator;
    public MazeToTiles converter;
    public int rows;
    public int cols;

    public GridFocus focus;

    public string outputPath;
    public string outputFile;

    //File format is 1 int per row:
    //  0 - Rows
    //  1 - Cols
    //  2 + n*4 - 4 directions per cell, 1 if blocked, else 0
    public void SaveMaze()
    {
        if ((outputFile != null) && (outputFile.Trim() != ""))
        {
            string path = System.IO.Path.Combine(outputPath, outputFile);
            using (StreamWriter output = new StreamWriter(path))
            {
                output.WriteLine(rows);
                output.WriteLine(cols);

                for (int i = 1; i <= rows; i++)
                {
                    for (int j = 1; j <= cols; j++)
                    {
                        for (int k = 0; k < MazeCell.dirs; k++)
                        {
                            output.WriteLine((cells[i,j][k]) ? "0" : "1");
                        }
                    }
                }
            }
        }
    }

    public int cellCount
    {
        get => rows * cols;
    }

    public IEnumerable<MazeCell> Cells()
    {
        for(int i = 1; i <= rows; i++)
        {
            for(int j = 1; j <= cols; j++)
            {
                yield return cells[i,j];
            }
        }
    }

    public IEnumerable<MazeCell> LockedCells(bool flipX = false, bool flipY = false, bool isLocked = true)
    {
        for(int i = 1; i <= rows; i++)
        {
            for(int j = 1; j <= cols; j++)
            {
                int u = (!flipY) ? i : (rows - i + 1);
                int v = (!flipX) ? j : (cols - j + 1);

                MazeCell c = cells[u,v];
                
                if (c.isLocked == isLocked)
                    yield return c;
            }
        }
    }

    public MazeCell this[int i, int j]
    {
        get => cells[i, j];
    }

    //Deflect direction 'k' if at bound
    public int DeflectDir(int i, int j, int k)
    {
        if ((i == 1) && (k == 2)) k = 0;
        if ((j == 1) && (k == 3)) k = 1;
        if ((i == rows) && (k == 0)) k = 2;
        if ((j == cols) && (k == 1)) k = 3;

        return k;
    }

    void Awake()
    {
        InitGrid();
        focus.Init(cellCount);
        generator.Generate(this);
    }

    void Update()
    {
        converter.Convert(this);
    }

    private void InitGrid()
    {
        int padRows = rows + 2;
        int padCols = cols + 2;
        cells = new MazeCell[padRows, padCols];

        for(int i = 0; i < padRows; i++)
        {
            for(int j = 0; j < padCols; j++)
            {
                bool pad = (i == 0) || (i == (rows+1)) || (j == 0) || (j == (cols+1));
                MazeCell c = (!pad) ? new MazeCell(i, j) : new MazeCell(0, 0);

                if (i > 0) c.SetNextDown(cells[i-1,j]);
                if (j > 0) c.SetNextLeft(cells[i,j-1]);

                cells[i,j] = c;  
            }
        }
    }

    private MazeCell[,] cells;
}
