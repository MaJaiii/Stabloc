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

                    tempCol = hit.collider.CompareTag("GameOver") ? Color.red : recentColor;
                }
            }
        }

        if (minDrop != Mathf.Infinity)
        {
            ghostBlock.position = pivotObj.position - new Vector3(0, minDrop - .5f, 0);
            ghostBlock.rotation = pivotObj.rotation;
            foreach (Transform child in ghostBlock)
            {
                tempCol.a = child.GetComponent<BlockWeight>() != null ? .9f : .8f;
                child.GetComponent<MeshRenderer>().material.color = tempCol;
            }
        }
    }

    private void Update()
    {
        if (!isActive && ghostBlock != null) Destroy(ghostBlock.gameObject) ;
    }
}
