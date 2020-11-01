using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPadManager : MonoBehaviour
{
    private RectTransform canvasRect = null;

    [SerializeField]
    private Image movableAreaImage = null;

    [SerializeField]
    private Image currentTouchPosImage = null;

    private void Awake()
    {
        canvasRect = GetComponentInParent<RectTransform>();
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
        Vector2 localPos = screenPos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out localPos);
        //localPos.x -= Screen.width / 2;
        //localPos.y -= Screen.height / 2;
        localPos = transform.InverseTransformPoint(screenPos);
        movableAreaImage.rectTransform.localPosition = localPos;
    }

    public void UpdateCurrentTouchPosImage(Vector2 screenPos)
    {
        Vector2 localPos = screenPos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out localPos);
        //localPos.x -= Screen.width / 2;
        //localPos.y -= Screen.height / 2;
        localPos = transform.InverseTransformPoint(screenPos);
        currentTouchPosImage.rectTransform.localPosition = localPos;
    }

    public void SetActiveVirtualPad(bool active)
    {
        movableAreaImage.gameObject.SetActive(active);
        currentTouchPosImage.gameObject.SetActive(active);
    }
}
