using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVoxel
{

   
    public bool Alive    
    {
        get
        {
            return _alive;
        }
        set
        {
            if(_goVoxelTrigger!=null)
            {
                MeshRenderer renderer = _goVoxelTrigger.GetComponent<MeshRenderer>();
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
    private GameObject _goVoxelTrigger;
    private OldVoxelGrid _grid;
    private bool _alive;
    public int lnc;
    public int Counter;


    public OldVoxel(int x, int z, OldVoxelGrid grid, float radomness)
    {
        Index = new Vector3Int(x, 0, z);
        _grid = grid;
        
        CreateGameobject();
        Alive = Random.value < radomness ? true : false;
        
    }

    public OldVoxel(int x, int z, OldVoxelGrid grid, int counter)
    {
        
        _grid = grid;
        Counter = counter;
        Index = new Vector3Int(x, 0, z);
        CreateGameobject();
        Alive = true;
    }

    public void CreateGameobject()
    {
        Vector3Int Index1 = Index + new Vector3Int(0, Counter, 0);
        _goVoxelTrigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _goVoxelTrigger.name = $"Voxel {Index1}";
        _goVoxelTrigger.tag = "Voxel";
        _goVoxelTrigger.transform.position = Index1;
        _goVoxelTrigger.transform.localScale = Vector3.one * 0.95f;
        OldVoxelTrigger trigger = _goVoxelTrigger.AddComponent<OldVoxelTrigger>();
        trigger.AttachedVoxel = this;
    }

    public List<OldVoxel> GetNeighbourList()
    {
        List<OldVoxel> neighbours = new List<OldVoxel>();


        foreach (var direction in Directions)
        {

            Vector3Int neighbourIndex = Index + direction;
            if (OldVoxelGrid.CheckInBounds(_grid.GridDimensions, neighbourIndex))
            {
                neighbours.Add(_grid.GetVoxelByIndex(neighbourIndex));
            }
        }

        return neighbours;
    }

    public void ToggleNeighbours()
    {
        List<OldVoxel> neighbours = GetNeighbourList();

        foreach (var neighbour in neighbours)
        {
            neighbour.Alive = !neighbour.Alive;
        }
    }

}
