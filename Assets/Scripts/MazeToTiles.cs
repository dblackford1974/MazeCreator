using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeToTiles : MonoBehaviour
{
    public TileBase floorTile;
    public TileBase floorLocked;
    public TileBase floorFocus;
    public TileBase wallTile;
    public Tilemap floorTarget;
    public Tilemap wallTarget;

    public void Convert(MazeGrid grid)
    {
        //Write to tilemap.  Could subclass here for 3D hedge or other variants.
        CreateFloors(grid);
        CreateWalls(grid);
    }

    private void CreateFloors(MazeGrid grid)
    {
        floorTarget.ClearAllTiles();

        int rows = grid.rows;
        int cols = grid.cols;

        Vector3Int off = new Vector3Int(-cols, -rows, 0) / 2;

        for(int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                TileBase tile = floorTile;

                if (grid.focus.IsFocus(i+1,j+1)) tile = floorFocus;
                else if (grid[i+1,j+1].isLocked) tile = floorLocked;

                floorTarget.SetTile(new Vector3Int(j, i, 0) + off, tile);
            }
        }
    }

    private void CreateWalls(MazeGrid grid)
    {
        Vector3Int off = new Vector3Int(-grid.cols-2, -grid.rows-2, 0) * 2;

        wallTarget.ClearAllTiles();

        foreach(MazeCell c in grid.Cells())
        {
            (int i, int j) = c.index;
            Vector3Int s = new Vector3Int(j*4, i*4, 0) + off;
            
            if (!c[0]) WallNorth(s);
            if (!c[1]) WallEast(s);
            if (!c[2]) WallNorth(s, true);
            if (!c[3]) WallEast(s, true);
        }
    }

    private void WallNorth(Vector3Int s, bool flip = false)
    {
        if (!flip) s.y += 4;

        for(int i = 0; i < 5; i++)
        {
            wallTarget.SetTile(s, wallTile);
            s.x++;
        }        
    }

    private void WallEast(Vector3Int s, bool flip = false)
    {
        if (!flip) s.x += 4;

        for(int i = 0; i < 5; i++)
        {
            wallTarget.SetTile(s, wallTile);
            s.y++;
        }        
    }
}
