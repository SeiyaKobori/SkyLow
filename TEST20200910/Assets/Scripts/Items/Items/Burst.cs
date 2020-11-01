using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : ItemBase
{
    protected override int itemId { get; set; } = 3;
    private Rigidbody rb = null;

    public override void ObtainItem(PlayerManager player)
    {
        player.Burst(true);
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SystemManager.SetRigidbodyList(rb);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= -100)
            gameObject.SetActive(false);
    }
}
