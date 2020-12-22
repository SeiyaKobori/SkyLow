using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsManager : MonoBehaviour
{
    [System.Serializable]
    public class StepInfo
    {
        public GameObject stepPrefab = null;
        [HideInInspector]
        public int id;
        public int percent; //全体の中での比率
        [HideInInspector]
        public ObjectPool _pool = null;
    }

    [SerializeField]
    private List<StepInfo> steps = new List<StepInfo>();

    private List<GameObject> activeStepList = new List<GameObject>();

    private readonly Vector3 generateOrigin = new Vector3(0, -200, 500);

    private void Awake()
    {
        SetupStepInfo();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetupStepInfo()
    {
        for(int i = 0; i < steps.Count; i++)
        {
            //足場IDを登録
            steps[i].id = steps[i].stepPrefab.GetComponent<StepBase>().GetStepId(); 

            //オブジェクトプールを生成
            var _pool = gameObject.AddComponent<ObjectPool>();
            steps[i]._pool = _pool;
            _pool.CreatePool(steps[i].stepPrefab, 3);
        }
    }

    public GameObject ActivateStep(int id)
    {
        StepInfo stepInfo = null;
        for(int i = 0; i < steps.Count; i++)
        {
            if (steps[i].id != id)
                continue;

            stepInfo = steps[i];
            break;
        }

        if(stepInfo == null)
        {
            Debug.Log("与えられたID(" + id + ")を持つ足場がありませんでした。");
            return null;
        }

        var obj = stepInfo._pool.GetObject();
        activeStepList.Add(obj);

        return obj;
    }

    public void ActivateRandomStep()
    {
        int id = Random.Range(0, steps.Count);

        var obj = ActivateStep(steps[id].id);
        obj.transform.position = GetRandomStepGeneratePos();
    }

    private Vector3 GetRandomStepGeneratePos() //ランダムな足場を返す
    {
        var pos = Vector3.zero;

        pos.y = Random.Range(0, 50);
        pos.x = Random.Range(0, 50);

        int miY = Random.Range(0, 2);
        if (miY == 1)
            pos.y *= -1;

        int miX = Random.Range(0, 2);
        if (miX == 1)
            pos.x *= -1;

        pos += generateOrigin;

        return pos;
    }

    public float GetBottomOfHeight()
    {
        float h = 0;

        foreach(var obj in activeStepList)
        {
            var height = obj.transform.position.y;
            if (height < h)
                h = height;
        }

        return h;
    }
}
