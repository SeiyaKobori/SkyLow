using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControler : MonoBehaviour
{
    private Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeOff()
    {
        animator.SetBool("Air", true);
    }

    public void Landing()
    {
        animator.SetBool("Air", false);
    }

    public void SetRun(float parameter)
    {
        var param = Mathf.Clamp(parameter, 0, 1.0f);
        animator.SetFloat("Run", param);
    }

    public void SetBoost(bool active)
    {
        if (animator.GetBool("Boost") == active)
            return;

        animator.SetBool("Boost", active);
    }

    public void SetDamage(bool active)
    {
        if (animator.GetBool("Damage") == active)
            return;

        animator.SetBool("Damage", active);
    }
}
