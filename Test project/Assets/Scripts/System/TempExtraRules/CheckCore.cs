using UnityEngine;

public class CheckCore : MonoBehaviour
{
    public Vector4 origins;

    [SerializeField] Vector3 pos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        origins = new Vector4(-2, -2, 2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.z = Mathf.RoundToInt(pos.z);
    }

    public Vector3[] CheckFill()
    {

        Vector3[] result = new Vector3[0];

        Debug.Log($"Call CheckFill, {pos}");

        Ray rayFromThisToRight = new Ray(pos, Vector3.right);       //x+
        Ray rayFromThisToLeft = new Ray(pos, Vector3.left);         //x-
        Ray rayFromThisToUp = new Ray(pos, Vector3.up);             //y+
        Ray rayFromThisToDown = new Ray(pos, Vector3.down);         //y-
        Ray rayFromThisToForward = new Ray(pos, Vector3.forward);   //z+
        Ray rayFromThisBack = new Ray(pos, Vector3.back);           //z-

        
        if (pos.x == origins.x && pos.z == origins.y)     //Position 0(-2,-2)
        {
            //Horizontal
            RaycastHit[] hits0 = Physics.RaycastAll(rayFromThisToForward, Mathf.Infinity, LayerMask.GetMask("Core"));
            foreach (RaycastHit hit0 in hits0) if (Mathf.Abs(Vector3.Distance(pos, hit0.transform.position) - 4) < .5f)
            {
                RaycastHit[] hits1 = Physics.RaycastAll(rayFromThisToRight, Mathf.Infinity, LayerMask.GetMask("Core"));
                foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                {
                    RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                    {
                        result = new Vector3[4] {pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                        for (int i = 0; i < result.Length; i++) result[i].y = Mathf.Max(pos.y, hit0.transform.position.y, hit1.transform.position.y, hit2.transform.position.y);
                        return result;
                    }
                }
            }
            //Vertical
            hits0 = Physics.RaycastAll(rayFromThisToDown, Mathf.Infinity, LayerMask.GetMask("Core"));
            foreach (RaycastHit hit0 in hits0) if (hit0.transform.name != this.name)
            {
                RaycastHit[] hits1 = Physics.RaycastAll(rayFromThisToRight, Mathf.Infinity, LayerMask.GetMask("Core"));
                foreach(RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                {
                    RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                    {
                        result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                        return result;
                    }
                }
                hits1 = Physics.RaycastAll(rayFromThisToForward, Mathf.Infinity, LayerMask.GetMask("Core"));
                foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                {
                    RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                    {
                        result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                        return result;
                    }
                }
            }
        }
        else if (pos.x == origins.x && pos.z == origins.w)     //Position 1(-2,2)
        {
            //Horizontal
            RaycastHit[] hits0 = Physics.RaycastAll(rayFromThisBack, Mathf.Infinity, LayerMask.GetMask("Core"));
            foreach (RaycastHit hit0 in hits0) if (Mathf.Abs(Vector3.Distance(pos, hit0.transform.position) - 4) < .5f)
                {
                    RaycastHit[] hits1 = Physics.RaycastAll(rayFromThisToRight, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                        {
                            RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.back, Mathf.Infinity, LayerMask.GetMask("Core"));
                            foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                                {
                                    result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                                    for (int i = 0; i < result.Length; i++) result[i].y = Mathf.Max(pos.y, hit0.transform.position.y, hit1.transform.position.y, hit2.transform.position.y);
                                    return result;
                                }
                        }
                }
            //Vertical
            hits0 = Physics.RaycastAll(pos, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
            foreach (RaycastHit hit0 in hits0) if (hit0.transform.name != this.name)
                {
                    RaycastHit[] hits1 = Physics.RaycastAll(pos, Vector3.right, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                        {
                            RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
                            foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                                {
                                    result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                                    return result;
                                }
                        }
                    hits1 = Physics.RaycastAll(pos, Vector3.back, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                        {
                            RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
                            foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                                {
                                    result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                                    return result;
                                }
                        }
                }
        }
        else if (pos.x == origins.z && pos.z == origins.y)     //Position 2(2,2)
        {
            //Horizontal
            RaycastHit[] hits0 = Physics.RaycastAll(pos, Vector3.back, Mathf.Infinity, LayerMask.GetMask("Core"));
            foreach (RaycastHit hit0 in hits0) if (Mathf.Abs(Vector3.Distance(pos, hit0.transform.position) - 4) < .5f)
                {
                    RaycastHit[] hits1 = Physics.RaycastAll(pos, Vector3.left, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                        {
                            RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.back, Mathf.Infinity, LayerMask.GetMask("Core"));
                            foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                                {
                                    result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                                    for (int i = 0; i < result.Length; i++) result[i].y = Mathf.Max(pos.y, hit0.transform.position.y, hit1.transform.position.y, hit2.transform.position.y);
                                    return result;
                                }
                        }
                }
            //Vertical
            hits0 = Physics.RaycastAll(pos, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
            foreach (RaycastHit hit0 in hits0) if (hit0.transform.name != this.name)
                {
                    RaycastHit[] hits1 = Physics.RaycastAll(pos, Vector3.left, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                        {
                            RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
                            foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                                {
                                    result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                                    return result;
                                }
                        }
                    hits1 = Physics.RaycastAll(pos, Vector3.back, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                        {
                            RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
                            foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                                {
                                    result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                                    return result;
                                }
                        }
                }
        }
        else if (pos.x == origins.z && pos.z == origins.y)     //Position 3(2,-2)
        {
            //Horizontal
            RaycastHit[] hits0 = Physics.RaycastAll(pos, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("Core"));
            foreach (RaycastHit hit0 in hits0) if (Mathf.Abs(Vector3.Distance(pos, hit0.transform.position) - 4) < .5f)
                {
                    RaycastHit[] hits1 = Physics.RaycastAll(pos, Vector3.left, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                        {
                            RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("Core"));
                            foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                                {
                                    result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                                    for (int i = 0; i < result.Length; i++) result[i].y = Mathf.Max(pos.y, hit0.transform.position.y, hit1.transform.position.y, hit2.transform.position.y);
                                    return result;
                                }
                        }
                }
            //Vertical
            hits0 = Physics.RaycastAll(pos, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
            foreach (RaycastHit hit0 in hits0) if (hit0.transform.name != this.name)
                {
                    RaycastHit[] hits1 = Physics.RaycastAll(pos, Vector3.left, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                        {
                            RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
                            foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                                {
                                    result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                                    return result;
                                }
                        }
                    hits1 = Physics.RaycastAll(pos, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("Core"));
                    foreach (RaycastHit hit1 in hits1) if (Mathf.Abs(Vector3.Distance(pos, hit1.transform.position) - 4) < .5f)
                        {
                            RaycastHit[] hits2 = Physics.RaycastAll(hit1.transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Core"));
                            foreach (RaycastHit hit2 in hits2) if (Mathf.Abs(Vector3.Distance(hit1.transform.position, hit2.transform.position) - 4) < .5f)
                                {
                                    result = new Vector3[4] { pos, hit0.transform.position, hit1.transform.position, hit2.transform.position };
                                    return result;
                                }
                        }
                }
        }
        return result;
    }
}
