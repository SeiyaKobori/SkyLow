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

    bool is_first_login = false;

    private void Awake()
    {
        over_letter_error_message.gameObject.SetActive(false);
        title_menu = transform.Find(title_menu_key).gameObject;
        name_entry_dialog = transform.Find(name_entry_dialog_key).gameObject;
        is_first_login = PlayerPrefs.GetInt(SystemManager.check_first_login_key) < 1;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateUserNameText()
    {
        user_name.text = FindObjectOfType<UserAuth>().currentPlayer();
    }

    public void SetNameEntryDialogActive(bool active)
    {
        name_entry_dialog.SetActive(active);

        if (active)
        {
            name_entry_input.textComponent.text = FindObjectOfType<UserAuth>().currentPlayer();
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
        FindObjectOfType<UserAuth>().signUp(input);
    }

    private IEnumerator DisplayOverLetterErrorCoroutine()
    {
        over_letter_error_message.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        over_letter_error_message.gameObject.SetActive(false);
    }
}
