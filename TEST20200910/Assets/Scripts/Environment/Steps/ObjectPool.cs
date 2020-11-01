using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> _poolObjList;

    private GameObject _poolObj;

    // オブジェクトプールを作成
    public void CreatePool(GameObject obj, int maxCount)
    {
        _poolObj = obj;
        _poolObjList = new List<GameObject>();
        for (int i = 0; i < maxCount; i++)
        {
            var newObj = CreateNewObject();
            newObj.gameObject.SetActive(false);
            _poolObjList.Add(newObj);
        }
    }

    public GameObject GetObject()
    {
        // 使用中でないものを探して返す
        foreach (var obj in _poolObjList)
        {
            if (obj.gameObject.activeSelf == false)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        // 全て使用中だったら新しく作って返す
        var newObj = CreateNewObject();
        newObj.gameObject.SetActive(true);
        _poolObjList.Add(newObj);

        return newObj;
    }

    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(_poolObj, transform);
        newObj.name = _poolObj.name + (_poolObjList.Count + 1);

        return newObj;
    }
}