using UnityEngine;
using System.Collections;
using NCMB;
using System.Collections.Generic;

public class UserAuth : MonoBehaviour
{

    private string currentPlayerName;
    private UserAuth instance = null;

    public bool isLogIn = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            string name = gameObject.name;
            gameObject.name = name + "(Singleton)";

            GameObject duplicater = GameObject.Find(name);
            if (duplicater != null)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.name = name;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // mobile backendに接続してログイン ------------------------

    public void logIn()
    {
        string id = PlayerPrefs.GetString(SystemManager.user_name_key);
        string pass = PlayerPrefs.GetString(SystemManager.password_key);
        if(pass.Length < 8)
        {
            Debug.Log("未登録ユーザー");
            return;
        }

        NCMBUser.LogInAsync(id, pass, (NCMBException e) => {
            // 接続成功したら
            if (e == null)
            {
                currentPlayerName = id;
                isLogIn = true;
            }
        });
    }

    // mobile backendに接続して新規会員登録 ------------------------

    public void signUp(string id)
    {

        NCMBUser user = new NCMBUser();
        user.UserName = id;
        PlayerPrefs.SetString(SystemManager.user_name_key, id);
        var pass = StringUtility.GeneratePassword(8);
        PlayerPrefs.SetString(SystemManager.password_key, pass);
        user.Password = pass;
        user.SignUpAsync((NCMBException e) => {

            if (e == null)
            {
                currentPlayerName = id;
            }
        });
    }

    // mobile backendに接続してログアウト ------------------------

    public void logOut()
    {

        NCMBUser.LogOutAsync((NCMBException e) => {
            if (e == null)
            {
                currentPlayerName = null;
            }
        });
    }

    // 現在のプレイヤー名を返す --------------------
    public string currentPlayer()
    {
        return currentPlayerName;
    }

}