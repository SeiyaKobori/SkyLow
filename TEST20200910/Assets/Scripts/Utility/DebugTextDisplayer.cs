using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTextDisplayer : MonoBehaviour
{
    [SerializeField]
    private Text debug_text = null;

    // Start is called before the first frame update
    void Start()
    {
        SystemManager.debug_text = this;
        debug_text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDebugText(string debugText)
    {
        debug_text.gameObject.SetActive(true);
        debug_text.text = debugText;
        StartCoroutine(displayTextCoroutine());
    }

    private IEnumerator displayTextCoroutine()
    {
        yield return new WaitForSeconds(3f);
        debug_text.gameObject.SetActive(false);
    }
}
