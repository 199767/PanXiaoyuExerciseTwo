using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVoxelGridManager : MonoBehaviour
{
    [SerializeField]
    private Vector3Int _gridDimensions = new Vector3Int(10, 1, 10);
    [SerializeField]
    public float randomness = 0.3f;

    private OldVoxelGrid startGrid;
    private OldVoxelGrid preGrid;

    public int counter = 1;

    void Start()
    {
        startGrid = new OldVoxelGrid(_gridDimensions, randomness);
        preGrid = startGrid;
        counter++;
    }

    void Update()
    {
        PerformRaycast();
    }

/*    public void CreateUp()
    {
        //Duplicate
        _grid = new OldVoxelGrid(_gridDimensions, randomness);

    }*/

    /*    public void Next()
        {
            _grid.GOL();
        }*/

    public void Next()
    {
        OldVoxelGrid curGrid = new OldVoxelGrid(_gridDimensions, preGrid, counter);
        curGrid.GOL();

        preGrid = curGrid;
        counter++;
    }

    public void Auto()
    {
        StartCoroutine(Autoo());
    }

    public IEnumerator Autoo()
    {
        while (true)
        {
            Next();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void PerformRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Voxel")
                {
                    GameObject hitObject = hit.transform.gameObject;
                    var voxel = hitObject.GetComponent<OldVoxelTrigger>().AttachedVoxel;

                    voxel.ToggleNeighbours();
                }
            }
        }
    }
}

