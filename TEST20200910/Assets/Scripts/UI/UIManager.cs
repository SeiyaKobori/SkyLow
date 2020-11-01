using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(VirtualPadManager))]
public class UIManager : MonoBehaviour
{
    private VirtualPadManager virtualPadManager = null;
    private Canvas canvas = null;
    private RectTransform canvasRect = null;

    [SerializeField]
    private Image pauseImage = null;

    private void Awake()
    {
        virtualPadManager = GetComponent<VirtualPadManager>();
        canvas = GetComponent<Canvas>();
        canvasRect = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMovavbleAreaImage(Vector2 screenPos)
    {
        virtualPadManager.UpdateMovavbleAreaImage(screenPos);
    }

    public void UpdateCurrentTouchPosImage(Vector2 screenPos)
    {
        virtualPadManager.UpdateCurrentTouchPosImage(screenPos);
    }

    public void SetActiveVirtualPad(bool active)
    {
        virtualPadManager.SetActiveVirtualPad(active);
    }

    public void SetPauseImageActive(bool active)
    {
        pauseImage.gameObject.SetActive(active);
    }
}
