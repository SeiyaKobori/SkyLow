using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject user_data_element = null;

    [SerializeField]
    private GameObject scroll_view_top = null;
    [SerializeField]
    private GameObject scroll_view_player = null;

    [SerializeField]
    private Transform content_top = null;
    [SerializeField]
    private Transform content_player = null;

    [SerializeField]
    private Button button_top = null;
    [SerializeField]
    private Button button_player = null;
    private Color32 press_button_color = new Color32(255, 0, 255, 170);

    // Start is called before the first frame update
    void Start()
    {
        ActivateTopRank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUserData_Top(int rank, string user_name, float score)
    {
        var scr = Instantiate(user_data_element, content_top).GetComponent<UserDataElement>();
        scr.SetUserData(rank, user_name, score);
    }

    public void SetUserData_Top(int rank, string user_name, double score)
    {
        var scr = Instantiate(user_data_element, content_top).GetComponent<UserDataElement>();
        scr.SetUserData(rank, user_name, score);
    }

    public void SetUserData_Player(int rank, string user_name, float score)
    {
        var scr = Instantiate(user_data_element, content_top).GetComponent<UserDataElement>();
        scr.SetUserData(rank, user_name, score);
    }

    public void SetUserData_Player(int rank, string user_name, double score)
    {
        var scr = Instantiate(user_data_element, content_top).GetComponent<UserDataElement>();
        scr.SetUserData(rank, user_name, score);
    }

    public void ResetContent(bool isTop)
    {
        Transform t = isTop ? content_top : content_player;
        foreach (Transform n in t)
        {
            GameObject.Destroy(n.gameObject);
        }
    }

    public void ActivateTopRank()
    {
        scroll_view_top.SetActive(true);
        scroll_view_player.SetActive(false);
        button_top.image.color = press_button_color;
        button_player.image.color = new Color32(0, 0, 0, 170);
    }

    public void ActivatePlayerRank()
    {
        scroll_view_top.SetActive(false);
        scroll_view_player.SetActive(true);
        button_top.image.color = new Color32(0, 0, 0, 170);
        button_player.image.color = press_button_color;
    }
}
