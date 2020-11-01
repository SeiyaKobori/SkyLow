using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    private Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SystemManager.SetRigidbodyList(rb);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        transform.position = new Vector3(0, transform.position.y, 0);
    }
}
