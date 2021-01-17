using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuCanvasManager : MonoBehaviour
{
    private GameObject title_menu = null;
    private const string title_menu_key = "TitleMenu";
    private GameObject name_entry_dialog = null;
    private const string name_entry_dialog_key = "NameEntryDialog";

    [SerializeField]
    private InputField name_entry_input = null;
    [SerializeField]
    private Text over_letter_error_message = null;
    [SerializeField]
    private Text user_name = null;
    [SerializeField]
    private GameObject entry_user_name_button = null;

    bool is_first_login = false;

    [SerializeField]
    private RankingUIManager ranking_ui_manager = null;
    private LeaderBoard leader_board = null;

    private UserAuth user_auth = null;

    private bool isScoreFetched = false;
    private bool isRankFetched = false;
    private bool isNeighborFetched = false;

    private void Awake()
    {
        leader_board = new LeaderBoard();
        over_letter_error_message.gameObject.SetActive(false);
        title_menu = transform.Find(title_menu_key).gameObject;
        name_entry_dialog = transform.Find(name_entry_dialog_key).gameObject;
        is_first_login = PlayerPrefs.GetInt(SystemManager.check_first_login_key) < 1;
        user_auth = FindObjectOfType<UserAuth>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isScoreFetched && leader_board.topRankers != null)
        {
            isScoreFetched = true;
            //Debug.Log("Top Rank Fetched!!");

            CreateTopRanking();
            UpdateUserNameText();
        }

        if(!isNeighborFetched && leader_board.neighbors != null)
        {
            isNeighborFetched = true;
            Debug.Log("neighbor Fetched!!");
            CreateNeighborRanking();
        }
    }

    private void UpdateUserNameText()
    {
        if (!user_auth.isLogIn)
            return;

        entry_user_name_button.SetActive(false);
        user_name.gameObject.SetActive(true);
        user_name.text = user_auth.currentPlayer();
    }

    public void SetNameEntryDialogActive(bool active)
    {
        name_entry_dialog.SetActive(active);

        if (active)
        {
            name_entry_input.textComponent.text = user_auth.currentPlayer();
        }
        else
        {
            UpdateUserNameText();
        }
    }

    public void CheckEnteringName()
    {
        string input = name_entry_input.textComponent.text;
        if (input.Length > 10)
        {
            StartCoroutine(DisplayOverLetterErrorCoroutine());
            name_entry_input.textComponent.text = null;
            return;
        }

        if (input.Length <= 0)
            return;

        name_entry_input.textComponent.text = input;
        PlayerPrefs.SetInt(SystemManager.check_first_login_key, 1);
        user_auth.signUp(input);

        CreateRankingBoard();
    }

    private IEnumerator DisplayOverLetterErrorCoroutine()
    {
        over_letter_error_message.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        over_letter_error_message.gameObject.SetActive(false);
    }

    public void CreateRankingBoard()
    {
        isScoreFetched = false;
        isNeighborFetched = false;

        ranking_ui_manager.ResetContent(true);
        ranking_ui_manager.ResetContent(false);

        leader_board.fetchTopRankers(100);
    }

    public void FetchPlayerRank(int user_score)
    {
        if (!user_auth.isLogIn)
            return;

            leader_board.fetchRank(user_score);
            leader_board.fetchNeighbors(20);//自分の前後の10人のデータを取得
    }

    private void CreateTopRanking()
    {
        for (int i = 0; i < leader_board.topRankers.Count; i++)
        {
            ranking_ui_manager.SetUserData_Top(i + 1, leader_board.topRankers[i].name, leader_board.topRankers[i].score);
        }
    }

    private void CreateNeighborRanking()
    {
        var t = leader_board.currentRank - (leader_board.neighbors.Count / 2);
        for (int i = 0; i < leader_board.neighbors.Count; i++)
        {
            ranking_ui_manager.SetUserData_Player(i + t, leader_board.neighbors[i].name, leader_board.neighbors[i].score);
        }
    }
}
