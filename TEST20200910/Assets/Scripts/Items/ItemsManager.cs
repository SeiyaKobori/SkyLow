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
        public ItemBase item;
        public int percent; //全体の中での比率
        [HideInInspector]
        public ObjectPool _pool = null;
    }

    [SerializeField]
    private List<ItemInfo> items = new List<ItemInfo>();

    private List<GameObject> activeItemList = new List<GameObject>();

    private readonly Vector3 generateOrigin = new Vector3(0, -200, 500);

    public float generateJammerSpan = 0; //大体n秒後に邪魔アイテムがスポーン
    private float nextJammerSpawnTime = 0;
    private float genJammertime = 0f;

    public float generateItemSpan = 0; //大体n秒後にアイテムがスポーン
    private float nextItemSpawnTime = 0;
    private float genItemtime = 0f;


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

    }

    private void SetupItemInfo()
    {
        for (int i = 0; i < items.Count; i++)
        {
            //アイテムIDを登録
            items[i].item = items[i].itemPrefab.GetComponent<ItemBase>();

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
            if (items[i].item.GetItemId() != id)
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

    public void UpdateItemSpawn()
    {
        genItemtime += Time.deltaTime;
        if (genItemtime > nextItemSpawnTime)
        {
            ActivateRandomItem(ItemBase.ItemTyoe.item);
            genItemtime = 0;
            nextItemSpawnTime = GetRandomItemSpawnTime();
        }
    }

    public void UpdateJammerSpawn()
    {
        genJammertime += Time.deltaTime;
        if(genJammertime > nextJammerSpawnTime)
        {
            ActivateRandomItem(ItemBase.ItemTyoe.jammer);
            genJammertime = 0;
            nextJammerSpawnTime = GetRandomJammerSpawnTime();
        }
    }

    public void ActivateRandomItem(ItemBase.ItemTyoe generateItemType)
    {
        List<int> itemList = new List<int>();
        foreach(var i in items)
        {
            if (i.item.type == generateItemType)
                itemList.Add(i.item.GetItemId());
        }

        int count = Random.Range(0, itemList.Count);

        var obj = ActivateItem(itemList[count]);
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

    public void SetJammerSpawnSpan(float span)
    {
        generateJammerSpan = 60 / span;
        nextJammerSpawnTime = GetRandomJammerSpawnTime();
    }

    private float GetRandomJammerSpawnTime()
    {
        return Random.Range(0, generateJammerSpan * 2);
    }

    public void SetItemSpawnSpan(float span)
    {
        generateItemSpan = 60 / span;
        nextItemSpawnTime = GetRandomItemSpawnTime();
    }

    private float GetRandomItemSpawnTime()
    {
        return Random.Range(0, generateJammerSpan * 2);
    }
}
