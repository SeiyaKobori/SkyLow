using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EffectManager))]
[RequireComponent(typeof(LevelManager))]
public class GameSystemManager : MonoBehaviour
{
    public PlayerManager player = null;

    [SerializeField]
    private Camera mainCamera = null;

    [SerializeField]
    private List<Rigidbody> rigidBodyList = new List<Rigidbody>();
    private Rigidbody playerRigidbody = null;
    private GameOverPanel gameOverPanel = null;

    private bool isIngame = false;
    private bool isGravity = false;
    private Vector3 gravityValue = new Vector3(0, -100, 0);
    private float gravityAcceleration = 0;

    [SerializeField]
    private GravityWall gravityWall = null;

    [SerializeField]
    private Rigidbody distanceWatcher = null;
    private float posOld = 0;
    private float highScore = 0;
    [SerializeField]
    private Text highScoreText = null;
    [SerializeField]
    private float distance = 0; //進んだ距離
    private float distanceSpan = 0;
    private float GenerateStepSpan = 40;

    private float levelUpBoader = 0;
    private const float levelupSpan = 1000f;
    private int currentLevel = 1;

    [SerializeField]
    private Text distanceText = null;

    [SerializeField]
    private StepsManager stepManager = null;
    [SerializeField]
    private ItemsManager itemManager = null;

    [SerializeField]
    private Slider distanceSlider = null;

    [SerializeField]
    private GameObject startMenuCanvas = null;
    [SerializeField]
    private GameObject ingameCanvas = null;
    [SerializeField]
    private GameObject gameoverCanvas = null;
    [SerializeField]
    private Text gameOverScoreText = null;
    private CanvasGroup cg = null;

    [SerializeField]
    private StartFloorManager startFloor = null;

    [SerializeField]
    private Animator GravityAnim = null;
    [SerializeField]
    private Camera StartCamera = null;
    private StartCameraAnimControler startCamAnim = null;

    [SerializeField]
    private AdsManager adManager = null;

    [SerializeField]
    private UIManager uiManager = null;
    private bool isPause = false;

    private bool isDanger = false;
    private EffectManager effectManager = null;

    private LevelManager levelManager = null;

    private void Awake()
    {
        if (!SystemManager.onSceneDestroyed)
        {
            SceneManager.sceneUnloaded += SystemManager.ResetDelegates;
            SystemManager.onSceneDestroyed = true;
        }

        SystemManager.SetGameSystemManager(this);
        rigidBodyList.Add(distanceWatcher);
        posOld = distanceWatcher.position.z;
        cg = gameoverCanvas.GetComponent<CanvasGroup>();
        SystemManager.IsIngameSwitch += SetIsIngame;
        startCamAnim = StartCamera.gameObject.GetComponent<StartCameraAnimControler>();
        startCamAnim.OnFinishAnim += GameStart;
        effectManager = GetComponent<EffectManager>();
        levelManager = GetComponent<LevelManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateHighscoreText();
        player.OnTouchGravity += GameOverGravity;
        player.OnFallStage += GameOverFall;
        playerRigidbody = SystemManager.player.gameObject.GetComponent<Rigidbody>();
        SystemManager.gravity.SetGravityPower(levelManager.GetGravityPower(currentLevel));
        itemManager.SetItemSpawnSpan(levelManager.GetItemSpan(currentLevel));
        itemManager.SetJammerSpawnSpan(levelManager.GetjammerSpan(currentLevel));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SystemManager.gravity.GetDistanceWithGravityWall(player.transform.position);
            

        if (!isIngame)
            return;

        UpdateIsDanger();
        UpdateIsGameOverFall();
        UpdateDistanceWatcher();
        UpdateStepsManager();
        UpdateGameOverHeight();
        gravityWall.Gravity();
        UpdateItemManager();
        Gravity();
    }

    public void PlayerMove(Vector3 vector) //プレイヤーが動くことに合わせてゲーム内全てのrigidbodyを逆に移動させる
    {
        for (int i = 0; i < rigidBodyList.Count; i++)
        {
            rigidBodyList[i].AddForce(-vector, ForceMode.Acceleration);
        }
        gameOverPanel.PlayerMoveForward(vector);
    }

    public void Gravity()
    {
        if (!playerRigidbody.useGravity || SystemManager.GetIsGrounded())
        {
            gravityAcceleration = 0;
            return;
        }

        for (int i = 0; i < rigidBodyList.Count; i++)
        {
            if (rigidBodyList[i].gameObject.tag == "Player")
                continue;
            rigidBodyList[i].AddForce(-(gravityValue + (gravityValue)), ForceMode.Acceleration);
        }
        gravityAcceleration += Time.deltaTime;
    }

    public void SetRigidbodyList(Rigidbody rb)
    {
        rigidBodyList.Add(rb);
        gravityWall.SetGravityList(rb);
    }

    public void RemoveRigidbodyList(Rigidbody rb)
    {
        if(rigidBodyList.Contains(rb))
            rigidBodyList.Remove(rb);
        gravityWall.RemoveGravityList(rb);
    }

    private void UpdateDistanceWatcher()
    {
        var pos = distanceWatcher.position;
        var diff = Mathf.Abs(pos.z) - Mathf.Abs(posOld);
        if (pos.z < 0)
        {
            diff = posOld;
            distanceWatcher.position = new Vector3(pos.x, pos.y, 1000);
        }

        distance += Mathf.Abs(diff);
        levelUpBoader += Mathf.Abs(diff);
        if(levelUpBoader > levelupSpan)
        {
            LevelUp();
            levelUpBoader = 0;
        }
        distanceText.text = (distance / 100).ToString("N2") + "km";
        distanceSpan += Mathf.Abs(diff);
        posOld = distanceWatcher.position.z;
    }

    private void UpdateIsGameOverFall()
    {
        if (player.gameObject.transform.position.y < -80)
            GameOverFall();
    }

    private void UpdateItemManager()
    {
        itemManager.UpdateItemSpawn();
        itemManager.UpdateJammerSpawn();
    }

    private void LevelUp()
    {
        ++currentLevel;
        Debug.Log("LEVEL UP!! : " + currentLevel);
        GenerateStepSpan = levelManager.GetGenerateStepSpan(currentLevel);
        SystemManager.gravity.SetGravityPower(levelManager.GetGravityPower(currentLevel));
        itemManager.SetJammerSpawnSpan(levelManager.GetjammerSpan(currentLevel));
        itemManager.SetItemSpawnSpan(levelManager.GetItemSpan(currentLevel));
    }

    private void GameOverFall()
    {
        GameOverCommon();
        StartCoroutine(GameOverFallCoroutine());
    }

    private void GameOverGravity()
    {
        GameOverCommon();
        StartCoroutine(GameOverFallCoroutine());
    }

    private void GameOverCommon()
    {
        SystemManager.SetIsIngame(false);
        ingameCanvas.SetActive(false);
        gameOverScoreText.text = (distance / 100).ToString("N2") + "km";
        if (distance > highScore)
        {
            try
            {
                SystemManager.SavePlayerData();
            }
            catch
            {
                SystemManager.debug_text.DisplayDebugText("Failed to save");
            }
            UpdateHighscoreText();
        }
    }

    private IEnumerator GameOverFallCoroutine()
    {
        var t = 0f;
        var waitTime = 1.5f;
        while(t < waitTime)
        {
            playerRigidbody.AddForce(gravityValue, ForceMode.Acceleration);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        var fadeTime = 0.8f;
        gameoverCanvas.gameObject.SetActive(true);
        while(t < fadeTime)
        {
            t += Time.deltaTime;
            var per = Mathf.InverseLerp(0, fadeTime, t);
            var a = Mathf.Lerp(0, 1, per);
            cg.alpha = a;
            yield return null;
        }
        cg.alpha = 1;
        player.gameObject.SetActive(false);
    }

    private void SetIsIngame(bool active)
    {
        isIngame = active;
    }

    public void ContinueStart()
    {
        StartCoroutine(ContinueCoroutine());
    }

    private void Continue()
    {
        SystemManager.SetIsIngame(true);
        ingameCanvas.SetActive(true);
        gameoverCanvas.SetActive(false);
        player.gameObject.SetActive(true);
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.Euler(Vector3.zero);
        player.AddBoost(100);
        mainCamera.transform.position = player.transform.position;
        startFloor.ResetStartFloor();
        player.ActivateBlinkEffect();
        gameOverPanel.ResetGameOverPanel();
    }

    public void GameStart()
    {
        ingameCanvas.SetActive(true);
        mainCamera.gameObject.SetActive(true);
        StartCamera.gameObject.SetActive(false);

        SystemManager.SetIsIngame(true);
    }

    public void ActivateGameStartCoroutine()
    {
        startMenuCanvas.SetActive(false);
        StartCoroutine(GameStartCoroutine());
    }

    private IEnumerator GameStartCoroutine()
    {
        gravityWall.SetGravityAnim(true);
        yield return new WaitForSeconds(3f);
        startCamAnim.SetCameraAnimAcitve(true);
    }

    private void UpdateStepsManager()
    {
        if (distanceSpan > GenerateStepSpan)
        {
            stepManager.ActivateRandomStep();
            distanceSpan = 0;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator ContinueCoroutine()
    {
        while (!adManager.isRewardAdReady())
            yield return null;
        adManager.DisplayUnityAds();
        yield return null;
        while (adManager.isRewardAdShowing())
            yield return null;

        Continue();
    }

    public void Pause()
    {
        isPause = !isPause;
        Time.timeScale = isPause ? 0 : 1;
        uiManager.SetPauseImageActive(isPause);
        SystemManager.SetIsIngame(!isPause);
    }

    private void OnApplicationQuit()
    {
        if(highScore < distance)
            SystemManager.SavePlayerData();
    }

    public float GetDistance()
    {
        return distance;
    }

    private void UpdateHighscoreText()
    {
        highScore = SystemManager.LoadPlayerData();
        highScoreText.text = (highScore / 100).ToString("N2") + "km";
    }

    public void SaveData()
    {
        SystemManager.SavePlayerData();
    }

    public void SetGameOverPanel(GameOverPanel panel)
    {
        gameOverPanel = panel;
    }

    private void UpdateIsDanger()
    {
        var dis = SystemManager.gravity.GetDistanceWithGravityWall(player.transform.position);

        if(isDanger)
        {
            if (dis > 20)
                effectManager.SetGravityParticleActive(false);
        }
        else
        {
            if (dis < 20)
                effectManager.SetGravityParticleActive(true);
        }
    }

    private void UpdateGameOverHeight()
    {
        gameOverPanel.CheckLowestOfHeight(stepManager.GetBottomOfHeight());
    }
}
