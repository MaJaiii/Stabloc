using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBlockPreview : MonoBehaviour
{
    public Transform pivotObj;                  // Active block
    public Transform ghostBlock;                // Visual prediction
    public LayerMask placementMask;             // e.g., "Default"
    public Material ghostBlockMaterial;         // Material of normal block
    public Material ghostWeightedBlockMaterial; // Material of weighted block

    public bool isActive = false;               // Activation of ghost system

    Color recentColor;

    #region Fill field
    [SerializeField] GameObject fillBlockPrefab;
    //List<GameObject> sameX = new List<GameObject>();
    //List<float> sameXdist = new List<float>();
    //GameObject sameXpivot;
    //[SerializeField]
    //List<GameObject> sameY = new List<GameObject>();
    //List<float> sameYdist = new List<float>();
    //GameObject sameYpivot;
    //List<GameObject> sameZ = new List<GameObject>();
    //List<float> sameZdist = new List<float>();
    //GameObject sameZpivot;
    GameObject fillGhostParent;
    #endregion

    public void CreateGhost(GameObject block, Color color)
    {
        if (ghostBlock != null) Destroy(ghostBlock.gameObject);


        ghostBlock = Instantiate(block).transform;
        ghostBlock.transform.position = block.transform.position;
        ghostBlock.name = "GhostBlock";
        ghostBlock.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        DestroyImmediate(ghostBlock.GetComponent<Rigidbody>());
        DestroyImmediate(ghostBlock.GetComponent<HingeJoint>());
        ghostBlock.GetComponent<BlockColor>().isGhost = true ;

        foreach (Transform child in ghostBlock)
        {
            Destroy(child.GetChild(0).gameObject);
            DestroyImmediate(child.GetComponent<Collider>());
            DestroyImmediate(child.GetComponent<Rigidbody>());

            var renderer = child.GetComponent<MeshRenderer>();
            if (renderer)
            {
                renderer.enabled = true;
                var col = color;
                Material mat;
                if (child.GetComponent<BlockWeight>() != null)
                {
                    mat = new Material(ghostWeightedBlockMaterial);
                    col.a = 1f;
                }
                else
                {
                    mat = new Material(ghostBlockMaterial);
                    col.a = .8f;
                }
                
                recentColor = color;
                mat.color = col;
                renderer.material = mat;
            }
        }
    }

    public void UpdateGhostPosition()
    {
        if (pivotObj == null || ghostBlock == null || !isActive) return;

        #region Fill initialize
        //sameX.Clear();
        //sameXdist.Clear();
        //if (sameXpivot != null) DestroyImmediate(sameXpivot.gameObject);
        //sameY.Clear();
        //sameYdist.Clear();
        //if (sameYpivot != null) DestroyImmediate(sameYpivot.gameObject);
        //sameZ.Clear();
        //sameZdist.Clear();
        //if (sameZpivot != null) DestroyImmediate(sameZpivot.gameObject);
        #endregion


        float minDrop = Mathf.Infinity;
        Color tempCol = recentColor;
        foreach (Transform child in pivotObj)
        {
            Vector3 origin = child.position + Vector3.up * 0.1f;
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 100f, placementMask))
            {
                float drop = child.position.y - hit.point.y;
                if (drop < minDrop)
                {
                    minDrop = drop;

                    tempCol = hit.collider.CompareTag("GameOver") ? Color.blue : recentColor;
                }
            }
        }

        if (minDrop != Mathf.Infinity)
        {
            ghostBlock.position = pivotObj.position - new Vector3(0, minDrop - .5f, 0);
            ghostBlock.rotation = pivotObj.rotation;
            foreach (Transform child in ghostBlock)
            {
                if (child.GetComponent<CheckCore>() != null)
                {
                    #region tempCodeForCheckingCore
                    //GameObject[] obj = GameObject.FindGameObjectsWithTag("Placed");
                    //foreach (GameObject obj2 in obj)
                    //{
                    //    if (obj2.GetComponent<BlockWeight>() == null) continue;
                    //    if (Mathf.Abs(Vector3.Dot(child.up, obj2.transform.up) % 90) >= 5) continue;
                    //    if (obj2 == child.gameObject) continue;
                    //    
                    //    //    Vector3 pos = obj2.transform.position;
                    //    //    if (Mathf.Abs(child.position.y - pos.y) <= .125f)   // check nearly same Y level
                    //    //    {
                    //    //        if (Mathf.Abs(Vector3.Magnitude(child.transform.position - pos) - 4) <= .15f &&                             //distance between two points
                    //    //            (Mathf.Abs(child.position.x - pos.x) <= .15f || Mathf.Abs(child.position.z - pos.z) <= .15f))           //check nearly same X or Z
                    //    //            sameY.Add(obj2);
                    //    //        else if (Mathf.Abs(Vector3.Magnitude(child.transform.position - pos) - 4 * Mathf.Sqrt(2)) <= .15f &&        //distance between two points
                    //    //            (Mathf.Abs(child.position.x - pos.x - 4) <= .15f && Mathf.Abs(child.position.z - pos.z - 4) <= .15f))   //check nearly diagonal
                    //    //            sameY.Add(obj2);
                    //    //    }
                    //    //}
                    //    //if (sameY.Count == 4)
                    //    //{
                    //    //    Vector3 tempValue = child.position;
                    //    //    Vector3 diaPos = Vector3.one * Mathf.Infinity;
                    //    //    foreach (GameObject listObj in sameY)
                    //    //    {
                    //    //        tempValue += listObj.transform.position;
                    //    //        if (Vector3.Magnitude(listObj.transform.position - child.position) > Vector3.Magnitude(diaPos - child.position)) diaPos = listObj.transform.position;
                    //    //    }
                    //    //    if (Mathf.Abs(Vector3.Magnitude(tempValue / 4 - child.position) - 2.5f * Mathf.Sqrt(2)) > .15f) sameY.Clear();
                    //    //    else
                    //    //    {
                    //    //        sameYpivot = new GameObject("sameY_Ghost_Parent");
                    //    //        Vector3 minValue = Vector3.zero;
                    //    //        minValue.y = child.transform.position.y;
                    //    //        foreach (GameObject listObj in sameY)
                    //    //        {
                    //    //            minValue.x = Mathf.Min(tempValue.x, diaPos.x); minValue.z = Mathf.Min(tempValue.z, diaPos.z);
                    //    //            diaPos.x = Mathf.Max(tempValue.x, diaPos.x); diaPos.z = Mathf.Max(tempValue.z, diaPos.z);
                    //    //        }
                    //    //        for (int x = Mathf.FloorToInt(minValue.x); x < Mathf.CeilToInt(diaPos.x); x++)
                    //    //        {
                    //    //            for (int z = Mathf.FloorToInt(minValue.z); z < Mathf.CeilToInt(diaPos.z); z++)
                    //    //            {
                    //    //                Instantiate(fillBlockPrefab, new Vector3(x, minValue.y, z), Quaternion.identity).transform.parent = sameYpivot.transform;
                    //    //            }
                    //    //        }
                    //    //    }
                    //    
                    //}
                    #endregion
                    CheckCore checkCore = child.GetComponent<CheckCore>();
                    Vector3[] vertex = checkCore.CheckFill();
                    if (vertex.Length % 4 == 0 && vertex.Length != 0)
                    {
                        fillGhostParent = new GameObject("fillGhostParent");
                        Vector3 minVertex = Vector3.zero;
                        Vector3 maxVertex = Vector3.zero;
                        for (int i = 0; i < vertex.Length; i++)
                        {
                            if (vertex[i].x <  minVertex.x) minVertex.x = vertex[i].x;
                            else if (vertex[i].x > maxVertex.x) maxVertex.x = vertex[i].x;

                            if (vertex[i].y < minVertex.y) minVertex.y = vertex[i].y;
                            else if (vertex[i].y > maxVertex.y) maxVertex.y = vertex[i].y;

                            if (vertex[i].z < minVertex.z) minVertex.z = vertex[i].z;
                            else if (vertex[i].z > maxVertex.z) maxVertex.z = vertex[i].z;
                        }
                        for (float x  = minVertex.x; x < maxVertex.x; x += 1)
                        {

                        }
                    }
                    tempCol.a = .9f;
                }
                else
                {
                    tempCol = recentColor;
                    tempCol.a = .8f;
                    child.GetComponent<MeshRenderer>().material.color = tempCol;
                }

            }
        }


    }

    private void Update()
    {
        if (!isActive && ghostBlock != null) Destroy(ghostBlock.gameObject) ;
    }
}
