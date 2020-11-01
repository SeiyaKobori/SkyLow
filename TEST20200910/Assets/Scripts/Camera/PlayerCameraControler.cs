using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControler : MonoBehaviour
{
    [HideInInspector]
    public Transform target = null;

    private readonly Vector3 landingCamPos = new Vector3(0, 4, -5);
    private readonly Vector3 landingCamRot = new Vector3(25, 0, 0);
    private readonly Vector3 airCamPos = new Vector3(0, 6, -10);
    private readonly Vector3 airCamRot = new Vector3(30, 0, 0);
    [SerializeField]
    private float camMoveSpeed = 7f;

    private int CamPosMovePattern = 2;
    private int gameoverCamPosPattern = 1;

    private readonly Vector3 gameOverGravityCamPos = new Vector3(700, -220, 300);
    private readonly Vector3 gameOverGravityCamRot = new Vector3(-20, -130, 0);


    private bool isIngame = false;

    private void Awake()
    {
        SystemManager.IsIngameSwitch += SetIsIngame;
    }

    private void Update()
    {
        UpdateCamPos();
    }

    public void UpdateCamPos()
    {
        if(target == null)
        {
            Debug.Log("プレイヤーが設定されていません");
            return;
        }

        if (isIngame)
        {
            switch (CamPosMovePattern)
            {
                case 1:
                    UpdateCamPosPattern01();
                    break;

                case 2:
                    UpdateCamPosPattern02();
                    break;
            }
        }
        else
            switch(gameoverCamPosPattern)
            {
                case 1:
                    LookAtPlayer();
                    break;

                case 2:
                    CamMoveGravity();
                    break;
            }
    }

    private void UpdateCamPosPattern01()    //常にプレイヤーの真後ろに追従する
    {
        var desPos = target.TransformPoint(SystemManager.GetIsGrounded() ? landingCamPos : airCamPos);
        var desRot = target.localRotation.eulerAngles + (SystemManager.GetIsGrounded() ? landingCamRot : airCamRot);

        var pos = transform.position;
        var rot = transform.rotation.eulerAngles;

        pos.x = Mathf.LerpAngle(pos.x, desPos.x, Time.deltaTime * camMoveSpeed);
        pos.y = Mathf.LerpAngle(pos.y, desPos.y, Time.deltaTime * camMoveSpeed);
        pos.z = Mathf.LerpAngle(pos.z, desPos.z, Time.deltaTime * camMoveSpeed);

        rot.x = Mathf.LerpAngle(rot.x, desRot.x, Time.deltaTime * camMoveSpeed);
        rot.y = Mathf.LerpAngle(rot.y, desRot.y, Time.deltaTime * camMoveSpeed);
        rot.z = Mathf.LerpAngle(rot.z, desRot.z, Time.deltaTime * camMoveSpeed);

        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
    }

    private void UpdateCamPosPattern02()    //プレイヤーの角度に関係なく常に正面を向く
    {
        var desPos = target.transform.position + (SystemManager.GetIsGrounded() ? landingCamPos : airCamPos);
        var desRot = (SystemManager.GetIsGrounded() ? landingCamRot : airCamRot);

        var pos = transform.position;
        var rot = transform.rotation.eulerAngles;

        pos.x = Mathf.LerpAngle(pos.x, desPos.x, Time.deltaTime * camMoveSpeed);
        pos.y = Mathf.LerpAngle(pos.y, desPos.y, Time.deltaTime * camMoveSpeed);
        pos.z = Mathf.LerpAngle(pos.z, desPos.z, Time.deltaTime * camMoveSpeed);

        rot.x = Mathf.LerpAngle(rot.x, desRot.x, Time.deltaTime * camMoveSpeed);
        rot.y = Mathf.LerpAngle(rot.y, desRot.y, Time.deltaTime * camMoveSpeed);
        rot.z = Mathf.LerpAngle(rot.z, desRot.z, Time.deltaTime * camMoveSpeed);

        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);

    }

    private void LookAtPlayer()
    {
        var rot = transform.rotation;
        var to = (target.transform.position - transform.position);
        var quaternion = Quaternion.RotateTowards(rot, Quaternion.LookRotation(to), Time.deltaTime * camMoveSpeed);
        transform.rotation = quaternion;
    }

    private void CamMoveGravity()
    {
        var pos = transform.position;
        var rot = transform.rotation;
        pos.x = Mathf.Lerp(pos.x, gameOverGravityCamPos.x, Time.deltaTime * camMoveSpeed);
        pos.y = Mathf.Lerp(pos.y, gameOverGravityCamPos.y, Time.deltaTime * camMoveSpeed);
        pos.z = Mathf.Lerp(pos.z, gameOverGravityCamPos.z, Time.deltaTime * camMoveSpeed);
        //var qua = Quaternion.RotateTowards(rot, Quaternion.Euler(gameOverGravityCamRot), Time.deltaTime * camMoveSpeed * 3);
        var qua = Quaternion.Euler(gameOverGravityCamRot);

        transform.position = pos;
        transform.rotation = qua;
    }


    private void SetIsIngame(bool active)
    {
        isIngame = active;
    }

    public void SetGameOverCamPosPattern(int i)
    {
        gameoverCamPosPattern = i;
    }
}
