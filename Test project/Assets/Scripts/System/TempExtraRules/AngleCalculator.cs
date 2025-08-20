using UnityEngine;
using UnityEngine.UI;

public class AngleCalculator : MonoBehaviour
{
    public Transform cube; // The cube above the plane
    public MeshRenderer ground;

    [SerializeField] Text angleDegreesText;
    [SerializeField] Text sineAngleText;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(0, 0, 0), Vector3.up, out hit, Mathf.Infinity))
        {
            Vector3 normal = hit.normal;
            angleDegreesText.text = Vector3.Angle(Vector3.up, normal).ToString();
        }
    }
}
