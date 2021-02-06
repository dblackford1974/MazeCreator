using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LinkGenerator : MonoBehaviour
{
    public abstract List<MazeLink> Generate(int rows, int cols);
}
