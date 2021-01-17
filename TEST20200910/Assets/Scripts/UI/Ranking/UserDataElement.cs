using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserDataElement : MonoBehaviour
{
    [SerializeField]
    private Text rank = null;

    [SerializeField]
    private Text user_name = null;

    [SerializeField]
    private Text score = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUserData(int rank, string user_name, float score)
    {
        this.rank.text = rank.ToString();
        this.user_name.text = user_name;
        this.score.text = SystemManager.GetScoreText(score);
    }

    public void SetUserData(int rank, string user_name, double score)
    {
        this.rank.text = rank.ToString();
        this.user_name.text = user_name;
        this.score.text = SystemManager.GetScoreText(score);
    }
}
