using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGrid
{
    public Vector3Int GridDimensions { get; private set; }
    private Voxel[,,] _voxels;
    public float Randomness;
    public int Counter;
    public VoxelGrid PreGrid;

    public VoxelGrid(Vector3Int gridDimensions, float randomness)
    {
        GridDimensions = gridDimensions;
        Counter = 0;
        Randomness = randomness;
        MakeVoxels();    
    }

    public VoxelGrid(Vector3Int gridDimensions, VoxelGrid preGrid)
    {
        GridDimensions = gridDimensions;
        Counter = preGrid.Counter + 1;
        PreGrid = preGrid;
        MakeVoxels();
    }

    public void GOL()
    {
        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {
                Voxel vo = _voxels[x, 0, z];
                List<Voxel> neis = vo.GetNeighbourList();

                int livencount = 0;
                foreach (Voxel nei in neis)
                {
                    if (nei.Alive)
                    {
                        livencount++;
                    }
                }
                vo.lnc = livencount;
            }
        }

        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {
                Check(x, z);
            }
        }
    }

    void Check(int x, int z)
    {
        Voxel vo = _voxels[x, 0, z];
        int livencount = vo.lnc;
        if (vo.Alive)
        {
            if (livencount < 2)
            {
                vo.Alive = false;
            }
            if (livencount > 3)
            {
                vo.Alive = false;
            }
        }
        else if (livencount == 3)
        {
            vo.Alive = true;
        }
    }

    private void MakeVoxels()
    {
        _voxels = new Voxel[GridDimensions.x, GridDimensions.y, GridDimensions.z];
        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {
                if (Counter == 0)
                {
                    _voxels[x, 0, z] = new Voxel(x, z, this, Randomness);
                }
                else
                {
                    _voxels[x, 0, z] = new Voxel(x, z, this, Counter);

                    Vector3Int Index = new Vector3Int(x, 0, z);
                    bool fate = PreGrid.GetVoxelByIndex(Index).Alive;
                    _voxels[x, 0, z].Alive = fate;
                }
            }
        }
    }

    public Voxel GetVoxelByIndex(Vector3Int index)
    {
        if(!CheckInBounds(GridDimensions,index)||_voxels[index.x, index.y, index.z]==null)
        {
            Debug.Log($"A Voxel at {index} doesn't exist");
            return null;
        }
        return _voxels[index.x, index.y, index.z];
    }

    public static bool CheckInBounds(Vector3Int gridDimensions, Vector3Int index)
    {
        if (index.x < 0 || index.x >= gridDimensions.x) return false;
        if (index.y < 0 || index.y >= gridDimensions.y) return false;
        if (index.z < 0 || index.z >= gridDimensions.z) return false;
        return true;
    }
}
