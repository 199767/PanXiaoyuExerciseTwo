using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGridManager : MonoBehaviour
{
    [SerializeField]
    private Vector3Int _gridDimensions = new Vector3Int(10, 1, 10);
    [SerializeField]
    public float randomness = 0.3f;
    private VoxelGrid startGrid;
    private VoxelGrid preGrid;
    private bool bAuto = false;
    private bool bAutoStack = false;

    void Start()
    {
        startGrid = new VoxelGrid(_gridDimensions, randomness);
        preGrid = startGrid;
    }

    void Update()
    {
        PerformRaycast();
    }

    public void Next()
    {
        preGrid.GOL();
    }

    public void Auto()
    {
        bAuto = !bAuto;
        bAutoStack = false;
        StartCoroutine(AutoC());
    }

    public IEnumerator AutoC()
    {
        while (bAuto)
        {
            Next();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void NextStack()
    {
        VoxelGrid curGrid = new VoxelGrid(_gridDimensions, preGrid);
        curGrid.GOL();
        preGrid = curGrid;
    }

    public void AutoStack()
    {
        bAutoStack = !bAutoStack;
        bAuto = false;
        StartCoroutine(AutoCorStack());
    }

    public IEnumerator AutoCorStack()
    {
        while (bAutoStack)
        {
            NextStack();
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
                    var voxel = hitObject.GetComponent<VoxelTrigger>().AttachedVoxel;
                    voxel.ToggleNeighbours();
                }
            }
        }
    }
}

