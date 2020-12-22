using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWall : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> rigidBodyList = new List<Rigidbody>();
    [SerializeField]
    private float gravityPower = 20; //この倍率分すわれていく 40にもなると激ムズ

    private Animator gravityAnim = null;

    private Collider gravityCollider = null;

    private void Awake()
    {
        SystemManager.gravity = this;
        SystemManager.gravityPower = gravityPower;
        gravityAnim = GetComponent<Animator>();
        gravityCollider = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Gravity()
    {
        for(int i = 0; i < rigidBodyList.Count; i++)
        {
            var vec = (transform.position - rigidBodyList[i].transform.position).normalized;
            vec *= gravityPower;
            rigidBodyList[i].AddForce(vec, ForceMode.Acceleration);
        }
    }

    public void SetGravityList(Rigidbody rb)
    {
        rigidBodyList.Add(rb);
    }

    public void RemoveGravityList(Rigidbody rb)
    {
        if (!rigidBodyList.Contains(rb))
            return;

        rigidBodyList.Remove(rb);
    }

    public void SetGravityPower(float value)
    {
        gravityPower = value;
        SystemManager.gravityPower = value;
    }

    public void SetGravityAnim(bool active)
    {
        gravityAnim.SetBool("Appear", active);
    }

    public float GetDistanceWithGravityWall(Vector3 from)
    {
        float dis = -1;
        var dir = (transform.position - from).normalized;
        Ray ray = new Ray(from, dir);
        RaycastHit hit;
        if (gravityCollider.Raycast(ray, out hit, Mathf.Infinity))
            dis = hit.distance;

        return dis;
    }
}
