using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class PlayerLocomotor : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb = null;
    private Rigidbody RB { set; get; }

    [SerializeField]
    private float moveOnGroundPower = 100f;
    [SerializeField]
    private float moveInAirPower = 100f;

    [SerializeField]
    private float airForwardPower = 6;

    private float airForwardPowerCurrent = 0;
    public float airForwardPowerMAX = 200f;

    private const float floatingSpeed = 7f; //システム的に前に進むスピード

    [HideInInspector]
    public bool isBoosted = false;
    private bool isBoostedOld = false;

    [SerializeField]
    private Slider boostGauge = null;
    private const float boostMax = 100f; //ブースト燃料のマックス
    private float currentBoostRemain = 100; //ブースト燃料の残り
    private const float boostCost = 10f; //ブーストするのに必要な燃料コスト。コスト毎秒必要 
    private const float boostRecoverValue = 30f; //燃料の自動回復速度。値毎秒回復する

    public ParticleSystem boostParticle = null;

    public bool isCoroutine = false;

    private bool isParalyzed = false;
    private Vector3 paralyzedVector = Vector3.zero;  //麻痺した時の方向ベクトル

    public bool isBurst = false;

    private bool isIngame = false;

    private void Awake()
    {
        currentBoostRemain = boostMax;
        SystemManager.IsIngameSwitch += SetIsIngame;
        RB = transform.GetComponentInChildren<Rigidbody>();
    }

    private void Start()
    {
        SystemManager.SetRigidbodyList(RB);
        SystemManager.OnIsGroundSwicthed += OnGroundSwitched;
        UpdateBoostUI();
        RB = transform.GetComponentInChildren<Rigidbody>();

    }

    private void Update()
    {
        if (!isIngame)
            return;

        UpdateBoost();
        UpdateBoostUI();
        UpdateParalyze();

        if (isBoosted)
            airForwardPowerCurrent = Mathf.Lerp(airForwardPowerCurrent, airForwardPowerMAX, Time.deltaTime * airForwardPower);
        else
            airForwardPowerCurrent = Mathf.Lerp(airForwardPowerCurrent, 0, Time.deltaTime * airForwardPower);
    }

    public void MoveGround(Vector2 vector)
    {
        UpdateBoostOnGround();

        var vec = new Vector3(vector.x * moveOnGroundPower, 0, vector.y * moveOnGroundPower);
        AddForce(vec, ForceMode.Acceleration);

        UpdateRotationOnGround(vec);

        //var gVec = new Vector3(vec.x, 0, vec.z);
        //SystemManager.PlayerMove(gVec);
    }

    public void MoveInAir(Vector2 vector)
    {
        UpdateBoostInAir();
        if (currentBoostRemain <= 0)
            return;

        var vec = new Vector3((vector.x * moveInAirPower), (vector.y * moveInAirPower * 2.0f), airForwardPowerCurrent);
        AddForce(vec, ForceMode.Acceleration);

        UpdateRotationOnAir(vec);
        UpdateBoostParticle(vector.y);

        var gVec = new Vector3(vec.x, vec.y, vec.z - (transform.position.z < 0 ? SystemManager.gravityPower + floatingSpeed : 0));
        SystemManager.PlayerMove(gVec);
    }

    public void AddForce(Vector3 vec, ForceMode mode = ForceMode.Acceleration)
    {
        if (isParalyzed)
            return;

        RB.AddForce(vec, mode);
    }

    private void UpdateRotationOnGround(Vector3 lookAtLocal)
    {
        if (isParalyzed)
            return;

        if (Time.timeScale < 1)
            return;

        var vec = lookAtLocal.normalized;
        vec += transform.position;

        transform.LookAt(vec);
    }

    private void UpdateRotationOnAir(Vector3 lookAtLocal)
    {
        if (isParalyzed)
            return;

        var vec = lookAtLocal.normalized;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(vec), moveInAirPower * Time.deltaTime);
    }

    private void UpdateBoost()
    {
        if (isParalyzed)
            return;

        isBoosted = (TouchUtility.GetTouch() == TouchInfo.Moved || TouchUtility.GetTouch() == TouchInfo.Stationary);


        if (currentBoostRemain <= 0)
            isBoosted = false;

        if (isBoosted != isBoostedOld)
        {
            isBoostedOld = isBoosted;
            if(!SystemManager.GetIsGrounded())
                RB.useGravity = !isBoosted;
        }
    }

    private void OnGroundSwitched(bool isGrounded)
    {
        if (isParalyzed)
            return;

        if (isGrounded)
            RB.useGravity = true;
        else
            RB.useGravity = !isBoosted;
    }

    private void UpdateBoostOnGround()
    {
        currentBoostRemain += boostRecoverValue * Time.deltaTime;
        if (currentBoostRemain > boostMax)
            currentBoostRemain = boostMax;
    }

    private void UpdateBoostInAir()
    {
        if (isParalyzed)
        {
            if (boostParticle.isEmitting)
                SetBoostParticleActive(false);
            return;
        }

        if (TouchUtility.GetTouch() == TouchInfo.Moved || TouchUtility.GetTouch() == TouchInfo.Stationary)
        {
            currentBoostRemain -= boostCost * Time.deltaTime;
            if (currentBoostRemain < 0)
                currentBoostRemain = 0;

            if (isBurst)
                currentBoostRemain = boostMax;

            if (!boostParticle.isEmitting && currentBoostRemain > 0)
                SetBoostParticleActive(true);
        }
        else if (boostParticle.isEmitting)
            SetBoostParticleActive(false);
    }

    private void SetBoostParticleActive(bool active)
    {
        if (active)
            boostParticle.Play();
        else
            boostParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    private void UpdateBoostUI()
    {
        var v = Mathf.InverseLerp(0, boostMax, currentBoostRemain);
        boostGauge.value = v;
    }

    public void TakeOffWithJump()
    {
        StartCoroutine(TakeOffCoroutine());
    }

    private IEnumerator TakeOffCoroutine()
    {

        isCoroutine = true;
        var boostedY = transform.position.y;
        var destY = boostedY + 3;

        yield return null;

        while (transform.position.y < destY)
        {
            var vector = transform.forward * 2 + transform.up * 5;

            AddForce(vector, ForceMode.Impulse);
            yield return null;
        }
        AddForce(Vector3.forward * 50, ForceMode.Impulse);

        isCoroutine = false;
    }

    public void SetParalyzeActive(bool active)
    {
        isParalyzed = active;
        RB.useGravity = active;

        if (active)
            paralyzedVector = transform.rotation.eulerAngles;
    }

    private void UpdateParalyze()
    {
        if (!isParalyzed)
            return;

        SystemManager.PlayerMove(paralyzedVector.normalized);
    }

    private void UpdateBoostParticle(float inputYvec)
    {
        if (isBurst)
            return;

        var main = boostParticle.main;
        main.gravityModifier = inputYvec;
    }

    public void AddBoost(float value)
    {
        currentBoostRemain = Mathf.Clamp(currentBoostRemain + value, 0, boostMax);
    }

    private void SetIsIngame(bool active)
    {
        isIngame = active;
    }
}
