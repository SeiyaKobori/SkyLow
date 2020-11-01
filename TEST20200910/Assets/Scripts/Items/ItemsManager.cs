using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemInfo
    {
        public GameObject itemPrefab = null;
        [HideInInspector]
        public int id;
        public int percent; //全体の中での比率
        [HideInInspector]
        public ObjectPool _pool = null;
    }

    [SerializeField]
    private List<ItemInfo> items = new List<ItemInfo>();

    private List<GameObject> activeItemList = new List<GameObject>();

    private readonly Vector3 generateOrigin = new Vector3(0, -200, 500);

    private float randomGenTime = 0;
    [SerializeField]
    private float genSpan = 0.25f;

    private void Awake()
    {
        SetupItemInfo();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //テスト用生成
        randomGenTime += Time.deltaTime;
        if (randomGenTime > genSpan)
        {
            ActivateRandomItem();
            randomGenTime = 0;
        }
    }

    private void SetupItemInfo()
    {
        for (int i = 0; i < items.Count; i++)
        {
            //アイテムIDを登録
            items[i].id = items[i].itemPrefab.GetComponent<ItemBase>().GetItemId();

            //オブジェクトプールを生成
            var _pool = gameObject.AddComponent<ObjectPool>();
            items[i]._pool = _pool;
            _pool.CreatePool(items[i].itemPrefab, 3);
        }
    }

    public GameObject ActivateItem(int id)
    {
        ItemInfo info = null;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id != id)
                continue;

            info = items[i];
            break;
        }

        if (info == null)
        {
            Debug.Log("与えられたID(" + id + ")を持つアイテムがありませんでした。");
            return null;
        }

        var obj = info._pool.GetObject();
        activeItemList.Add(obj);

        return obj;
    }

    public void ActivateRandomItem()
    {
        int id = Random.Range(0, items.Count);

        var obj = ActivateItem(items[id].id);
        obj.transform.position = GetRandomItemGeneratePos();
    }

    private Vector3 GetRandomItemGeneratePos() //ランダムな足場を返す
    {
        var pos = Vector3.zero;

        pos.y = Random.Range(0, 100);
        pos.x = Random.Range(0, 100);

        int miY = Random.Range(0, 2);
        if (miY == 1)
            pos.y *= -1;

        int miX = Random.Range(0, 2);
        if (miX == 1)
            pos.x *= -1;

        pos += generateOrigin;

        return pos;
    }
}
