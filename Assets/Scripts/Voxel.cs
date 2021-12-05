using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel
{

    public bool Alive    
    {
        get
        {
            return _alive;
        }
        set
        {
            if(vox!=null)
            {
                MeshRenderer renderer = vox.GetComponent<MeshRenderer>();
                renderer.enabled = value; 
            }
            _alive = value;
        }
    }

    public static List<Vector3Int> Directions = new List<Vector3Int>
    {
        new Vector3Int(-1,0,0),
        new Vector3Int(1,0,0),
        new Vector3Int(0,0,-1),
        new Vector3Int(0,0,1),
        new Vector3Int(-1,0,1),
        new Vector3Int(1,0,-1),
        new Vector3Int(-1,0,-1),
        new Vector3Int(1,0,1)
    };

    public Vector3Int Index { get; private set; }
    private GameObject vox;
    private VoxelGrid _grid;
    private bool _alive;
    public int lnc;
    public int Counter;

    public Voxel(int x, int z, VoxelGrid grid, float radomness)
    {
        _grid = grid;
        Index = new Vector3Int(x, 0, z);
        CreateGameobject();
        Alive = Random.value < radomness ? true : false;
    }

    public Voxel(int x, int z, VoxelGrid grid, int counter)
    {
        Counter = counter;
        _grid = grid;
        Index = new Vector3Int(x, 0, z);
        CreateGameobject();
        Alive = true;
    }

    public void CreateGameobject()
    {
        Vector3Int Index1 = Index + new Vector3Int(0, Counter, 0);
        vox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vox.name = $"Voxel {Index1}";
        vox.tag = "Voxel";
        vox.transform.position = Index1;
        vox.transform.localScale = Vector3.one * 0.95f;
        VoxelTrigger trigger = vox.AddComponent<VoxelTrigger>();
        trigger.AttachedVoxel = this;
    }

    public List<Voxel> GetNeighbourList()
    {
        List<Voxel> neighbours = new List<Voxel>();
        foreach (var direction in Directions)
        {

            Vector3Int neighbourIndex = Index + direction;
            if (VoxelGrid.CheckInBounds(_grid.GridDimensions, neighbourIndex))
            {
                neighbours.Add(_grid.GetVoxelByIndex(neighbourIndex));
            }
        }
        return neighbours;
    }

    public void ToggleNeighbours()
    {
        List<Voxel> neighbours = GetNeighbourList();

        foreach (var neighbour in neighbours)
        {
            neighbour.Alive = !neighbour.Alive;
        }
    }

}
