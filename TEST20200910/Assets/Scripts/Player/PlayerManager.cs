using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerLocomotor))]
[RequireComponent(typeof(PlayerAnimationControler))]
public class PlayerManager : MonoBehaviour
{
    private PlayerLocomotor locomotor = null;
    private PlayerAnimationControler animControler = null;
    [SerializeField]
    private PlayerCameraControler cameraControler = null;
    private CapsuleCollider p_collider = null;
    [HideInInspector]
    public Vector2 moveInput = Vector2.zero;

    private bool isTakeOff = false;
    private bool isBoosted = false;

    private float ParalyzeTime = 2;
    private bool isParalyzed = false;

    private float burstTime = 0;
    private const float burstTimeMax = 10f;
    private bool isBurst = false;

    [SerializeField]
    private ParticleSystem boostParticle = null;
    [SerializeField]
    private ParticleSystem burstParticle = null;

    private bool isIngame = false;

    public delegate void TouchGravityDelegate();
    public event TouchGravityDelegate OnTouchGravity;

    public delegate void FallStageDelegate();
    public event FallStageDelegate OnFallStage;

    private SkinnedMeshRenderer meshrenderer = null;

    private float burstForwardPower = 1000;
    private float defaultForwardPower = 0; //PlayerLocomotorの初期最大値を参照

    private void Awake()
    {
        SystemManager.player = this;
        locomotor = GetComponent<PlayerLocomotor>();
        animControler = GetComponent<PlayerAnimationControler>();
        p_collider = GetComponent<CapsuleCollider>();
        cameraControler.target = this.transform;
        OnTouchGravity += SwitchCamToGravity;
        SystemManager.IsIngameSwitch += SetIsIngame;

        meshrenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        defaultForwardPower = locomotor.airForwardPowerMAX;
    }

    // Start is called before the first frame update
    void Start()
    {
        SystemManager.SetPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isIngame)
            return;

        UpdateIsGrounded();
        UpdateBoost();
        UpdateIsBurst();

        UpdateIsGameOverFall();

        if (locomotor.isCoroutine || isParalyzed)
            return;

        if (SystemManager.GetIsGrounded())
        {
            MoveOnGround();
        }
        else
        {
            MoveinAir();
        }

        if (Input.GetKeyDown(KeyCode.A))
            GetDamage();
    }

    private void UpdateIsGrounded()
    {
        if (locomotor.isCoroutine || isParalyzed)
            return;

        var origin = transform.position;
        origin.y += p_collider.height / 2;
        Ray ray = new Ray(origin, Vector3.down * (p_collider.height / 2));
        Debug.DrawRay(origin, Vector3.down * (p_collider.height / 2), Color.red, 5f);
        RaycastHit hit;
        var ig = SystemManager.GetIsGrounded();
        if (ig != Physics.Raycast(origin, Vector3.down, out hit, (p_collider.height / 2) + 0.3f, 1 << LayerMask.NameToLayer("Ground")))
        {
            SystemManager.SetIsGrounded(!ig);
            if (SystemManager.GetIsGrounded())
            {
                animControler.Landing();
                SetAirBoostActive(false);
            }
            else
                animControler.TakeOff();
        }
    }

    public void SetAirBoostActive(bool active)
    {
        isTakeOff = active;
        animControler.SetBoost(active);

        if (active)
        {
            animControler.TakeOff();
            locomotor.TakeOffWithJump();
        }
        else
        {
            animControler.Landing();
        }
    }

    private void MoveinAir()
    {
        locomotor.MoveInAir(moveInput);       
    }

    private void MoveOnGround()
    {
        locomotor.MoveGround(moveInput);
        animControler.SetRun(moveInput.magnitude);
    }

    private void UpdateBoost()
    {
        var isTouching = locomotor.isBoosted;

        if (isTouching != isBoosted)
        {
            isBoosted = isTouching;
            animControler.SetBoost(isBoosted);
            locomotor.isBoosted = isBoosted;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.gameObject.GetComponent<ItemInterface>(); //アイテムを取得したらその効果を発生させる
        if (item != null)
        {
            item.ObtainItem(this);
            return;
        }

        if (other.tag == "Gravity")
            OnTouchGravity();

        if (other.tag == "GameOverFall")
            FallenStage();
    }

    public void GetDamage(float paralyzeTime = 2.0f)
    {
        if (isBurst)
            return;

        this.ParalyzeTime = paralyzeTime;
        StartCoroutine(ParalyzeCoroutine());
    }

    private IEnumerator ParalyzeCoroutine()
    {
        SetParalyzeActive(true);
        float time = 0;

        while (time < ParalyzeTime)
        {
            
            time += Time.deltaTime;
            yield return null;
        }

        SetParalyzeActive(false);
    }

    private void SetParalyzeActive(bool active)
    {
        animControler.SetDamage(active);
        isParalyzed = active;
        locomotor.SetParalyzeActive(active);
    }

    public void AddBoost(float value)
    {
        locomotor.AddBoost(value);
    }

    public void Burst(bool active)
    {
        if (isBurst == active)
            return;

        if (active && isParalyzed)
            SetParalyzeActive(false);

        isBurst = active;
        locomotor.SetIsBurst(active);
        locomotor.boostParticle.Stop();
        locomotor.boostParticle = active ? burstParticle : boostParticle;
        locomotor.airForwardPowerMAX = active ? burstForwardPower : defaultForwardPower;
        burstTime = 0;
    }

    private void UpdateIsBurst()
    {
        if (!isBurst)
            return;

        burstTime += Time.deltaTime;

        if (burstTime > burstTimeMax)
            Burst(false);
    }

    private void SwitchCamToGravity()
    {
        cameraControler.SetGameOverCamPosPattern(2);
    }

    private void UpdateIsGameOverFall()
    {
        if (gameObject.transform.position.y < -80 && isIngame)
            FallenStage();
    }

    private void FallenStage()
    {
        cameraControler.SetGameOverCamPosPattern(1);
        OnFallStage();
    }

    private void SetIsIngame(bool active)
    {
        isIngame = active;
    }

    public void ActivateBlinkEffect()
    {
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        float t = 0;
        float tMax = 3f;

        float bt = 0;
        float blinkTimeMax = 0.2f;

        while (t < tMax)
        {
            t += Time.deltaTime;
            bt += Time.deltaTime;
            if (bt > blinkTimeMax)
            {
                meshrenderer.enabled = !meshrenderer.enabled;
                bt = 0;
            }
            yield return null;
        }
        meshrenderer.enabled = true;
    }
}
