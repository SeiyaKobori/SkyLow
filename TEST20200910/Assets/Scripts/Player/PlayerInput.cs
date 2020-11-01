using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerManager))]
public class PlayerInput : MonoBehaviour
{
    private PlayerManager manager = null;

    private Vector2 touchStartPos = Vector2.zero;
    private Vector2 touchCurrentPos = Vector2.zero;
    private bool touchingScreen = false;
    private float touchRadiusMax = 100;
    [SerializeField]
    private Image touchRadiusImage = null;

    [SerializeField]
    private UIManager uiManager = null;

    [HideInInspector]
    public bool isTouching = false;

    private bool isIngame = false;

    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
        if (touchRadiusImage != null) //半径イメージ画像があればその画像の半分を取得する
            touchRadiusMax = touchRadiusImage.rectTransform.sizeDelta.x / 2;
        SystemManager.IsIngameSwitch += SetIsIngame;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isIngame)
            return;

        manager.moveInput = GetKeyboardInput();
        manager.moveInput = GetVirtualPadInput();
    }



    private Vector2 GetKeyboardInput()
    {
        Vector2 input = Vector2.zero;

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        return input;
    }

    private Vector2 GetVirtualPadInput()
    {
        Vector2 input = Vector2.zero;
        UpdateVirtualInput();

        if (!touchingScreen)
            return input;

        input.x = touchCurrentPos.x - touchStartPos.x;
        input.y = touchCurrentPos.y - touchStartPos.y;

        input.x = Mathf.InverseLerp(0, touchRadiusMax, Mathf.Abs(input.x)) * (input.x > 0 ? 1 : -1);
        input.y = Mathf.InverseLerp(0, touchRadiusMax, Mathf.Abs(input.y)) * (input.y > 0 ? 1 : -1);

        return input;
    }

    private void UpdateVirtualInput()
    {
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(0))
            return;

        switch(TouchUtility.GetTouch())
        {
            case TouchInfo.Began: //タッチ初めにスクリーン上の位置を記憶
                touchStartPos = TouchUtility.GetTouchPosition();
                touchingScreen = true;
                uiManager.SetActiveVirtualPad(true);
                uiManager.UpdateMovavbleAreaImage(touchStartPos);
                break;

            case TouchInfo.Moved: //タッチ中にスクリーン上の位置を記憶①
                touchCurrentPos = TouchUtility.GetTouchPosition();
                uiManager.UpdateCurrentTouchPosImage(touchCurrentPos);
                break;

            case TouchInfo.Stationary: //タッチ中にスクリーン上の位置を記憶②
                touchCurrentPos = TouchUtility.GetTouchPosition();
                uiManager.UpdateCurrentTouchPosImage(touchCurrentPos);
                break;

            case TouchInfo.Ended: //タッチ終了後にタッチ位置をリセット
                touchStartPos = Vector2.zero;
                touchCurrentPos = Vector2.zero;
                touchingScreen = false;
                uiManager.SetActiveVirtualPad(false);
                break;
        }
    }

    private void SetIsIngame(bool active)
    {
        isIngame = active;
    }


}
