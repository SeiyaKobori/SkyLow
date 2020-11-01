using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class StepBase : MonoBehaviour
{
    protected abstract int stepId { set; get; }
    protected Rigidbody rb = null;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SystemManager.SetRigidbodyList(rb);
    }

    protected virtual void Update()
    {
        if (transform.position.z <= -100)
            gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SystemManager.RemoveRigidbosyList(rb);
    }

    public int GetStepId()
    {
        return stepId;
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Ground")
        {
            if(other.transform.position.y < transform.position.y)
                gameObject.SetActive(false);
        }
    }
}
