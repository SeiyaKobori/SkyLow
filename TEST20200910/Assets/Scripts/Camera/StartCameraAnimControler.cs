using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StartCameraAnimControler : MonoBehaviour
{
    private Animator anim = null;

    public delegate void AnimFinishDelegate();
    public event AnimFinishDelegate OnFinishAnim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraAnimAcitve(bool active)
    {
        anim.SetBool("Appear", active);
    }

    public void ActivateCameraAnim()
    {
        anim.SetBool("Appear", true);
    }

    public void DactivateCameraAnim()
    {
        anim.SetBool("Appear", false);
    }

    public void OnFinishAnimation()
    {
        OnFinishAnim();
    }
}
