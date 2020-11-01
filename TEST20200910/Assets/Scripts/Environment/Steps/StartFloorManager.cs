using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFloorManager : MonoBehaviour
{
    private List<Transform> startFloors = null;
    private List<Vector3> defaultPoses = new List<Vector3>();
    private List<Vector3> defaultRots = new List<Vector3>();
    private List<Rigidbody> rigidBodys = null;
    private List<MeshRenderer> meshrenderers = null;

    private void Awake()
    {
        startFloors = new List<Transform>(transform.GetComponentsInChildren<Transform>());
        startFloors.Remove(transform);
        for (int i = 0; i < startFloors.Count; i++)
        {
            defaultPoses.Add(startFloors[i].position);
            defaultRots.Add(startFloors[i].rotation.eulerAngles);
        }
        rigidBodys = new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>());
        meshrenderers = new List<MeshRenderer>(transform.GetComponentsInChildren<MeshRenderer>());
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    startFloors = new List<Transform>(transform.GetComponentsInChildren<Transform>());
    //    rigidBodys = new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>());
    //    meshrenderers = new List<MeshRenderer>(transform.GetComponentsInChildren<MeshRenderer>());
    //    startFloors.Remove(transform);
    //}

    // Update is called once per frame
    void Update()
    {
        var active = false; //1つでもオブジェクトがアクティブならtrue
        foreach(var o in startFloors)
        {
            if(o.gameObject.activeInHierarchy)
            {
                active = true;
                break;
            }
        }

        if (!active)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetStartFloor()
    {
        for(int i = 0; i < startFloors.Count; i++)
        {
            startFloors[i].gameObject.SetActive(true);
            startFloors[i].position = defaultPoses[i];
            startFloors[i].rotation = Quaternion.Euler(defaultRots[i]);
        }
        //foreach (var rb in rigidBodys)
        //{
        //    rb.isKinematic = false;
        //}

        gameObject.SetActive(true);
    }
}
