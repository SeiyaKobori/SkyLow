using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ParalyzeStorm : ItemBase
{
    protected override int itemId { get; set; } = 1;
    private Rigidbody rb = null;

    public Vector3 targetPos { set; get; } = Vector3.zero;
    public float moveSpeed { set; get; } = 10f;

    public override void ObtainItem(PlayerManager player)
    {
        player.GetDamage(1.0f);
        gameObject.SetActive(false);
    }

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        SystemManager.SetRigidbodyList(rb);
        targetPos = SystemManager.gravity.transform.position;
    }

    protected void Update()
    {
        if (transform.position.z <= -100)
            gameObject.SetActive(false);

        MoveToTarget();
    }

    private void MoveToTarget()
    {
        var vector = (transform.position - targetPos).normalized;
        rb.AddForce(vector * moveSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
            gameObject.SetActive(false);
    }
}
