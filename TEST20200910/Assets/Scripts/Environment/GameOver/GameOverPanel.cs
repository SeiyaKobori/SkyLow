using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    private Rigidbody rb = null;

    private float HeightForFail = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        HeightForFail = transform.position.y; //再生時の高さをゲームオーバーの値にする
    }
    // Start is called before the first frame update
    void Start()
    {
        SystemManager.SetRigidbodyList(rb);
        SystemManager.gameSystemManager.SetGameOverPanel(this);
    }

    public void PlayerMoveForward(Vector3 vector)
    {
        if (transform.position.y < HeightForFail)
            return;

        rb.AddForce(0, -vector.z, 0, ForceMode.Acceleration);
    }

    public void ResetGameOverPanel()
    {
        var pos = transform.position;
        pos.y = HeightForFail;
        transform.position = pos;
    }

    public void CheckLowestOfHeight(float bottom)
    {
        if (transform.position.y > bottom)
        {
            var pos = transform.position;
            pos.y = bottom;
            transform.position = pos;
        }
    }
}
