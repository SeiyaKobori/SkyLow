using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using naichilab.Scripts.Extensions;

public static class SystemManager
{
    private static bool isGrounded = false;

    public static GameSystemManager gameSystemManager = null;

    public static GravityWall gravity = null;
    public static PlayerManager player = null;
    public static float gravityPower = 0;
    public static DebugTextDisplayer debug_text = null;

    public delegate void IsGroundedChangeDelegate(bool isGrounded);
    public static event IsGroundedChangeDelegate OnIsGroundSwicthed;

    public delegate void IsIngameSwitchDelegate(bool active);
    public static event IsIngameSwitchDelegate IsIngameSwitch;

    private static readonly string SaveDataFolderPath = Path.Combine(Application.persistentDataPath, @"SaveData");
    private static readonly string SaveDataFilePath = Path.Combine(SaveDataFolderPath, "saveData");

    public static bool onSceneDestroyed = false;

    public static NCMB.HighScore highScore;
    public static string check_first_login_key = "isFirstLogin";

    public static string user_name_key = "UserName";
    public static string password_key = "Passwords";

    public static int default_game_speed = 2;

    public static void SetIsGrounded(bool active)
    {
        if (isGrounded == active)
            return;

        isGrounded = active;
        OnIsGroundSwicthed(isGrounded);
    }

    public static bool GetIsGrounded()
    {
        return isGrounded;
    }

    public static void SetGameSystemManager(GameSystemManager manager)
    {
        gameSystemManager = manager;
    }

    public static void SetRigidbodyList(Rigidbody rb)
    {
        gameSystemManager.SetRigidbodyList(rb);
    }

    public static void RemoveRigidbosyList(Rigidbody rb)
    {
        gameSystemManager.RemoveRigidbodyList(rb);
    }

    public static void PlayerMove(Vector3 vector)
    {
        gameSystemManager.PlayerMove(vector);
    }

    public static void SetPlayer(PlayerManager player)
    {
        gameSystemManager.player = player;
    }

    public static void SetIsIngame(bool active)
    {
        IsIngameSwitch(active);
    }

    public static void SavePlayerData()
    {
        if(!Directory.Exists(SaveDataFolderPath))
            Directory.CreateDirectory(SaveDataFolderPath);

        //StreamWriter writer;

        SavePlayerData data = CreateSavePlayerData();
        string jsonData = JsonUtility.ToJson(data);

        //writer = new StreamWriter(Path.Combine(SaveDataFilePath + ".json"), false);

        //writer.Write(jsonData);
        //writer.Flush();
        //writer.Close();

        var file = new FileStream(Path.Combine(SaveDataFilePath + ".json"),FileMode.Create, FileAccess.Write);

        // 文字列をbyte配列に変換
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // ファイルに保存
        file.Write(bytes, 0, bytes.Length);

        file.Close();
    }

    public static float LoadPlayerData()
    {
        if (!File.Exists(Path.Combine(SaveDataFilePath + ".json")))
            return -1;

        var file = new FileStream(Path.Combine(SaveDataFilePath + ".json"), FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[file.Length];

        // ファイルを読み込み
        file.Read(bytes, 0, bytes.Length);

        // 読み込んだbyte配列をJSON形式の文字列に変換
        var jsonString = System.Text.Encoding.UTF8.GetString(bytes);

        // JSON形式の文字列をセーブデータのクラスに変換
        var playerData = JsonUtility.FromJson<SavePlayerData>(jsonString);

        file.Close();

        return playerData.score;

        string data = "";
        StreamReader reader;

        reader = new StreamReader(Path.Combine(SaveDataFilePath + ".json"));

        data = reader.ReadToEnd();
        reader.Close();

        var score = JsonUtility.FromJson<SavePlayerData>(data);
        return score.score;
    }

    private static SavePlayerData CreateSavePlayerData()
    {
        SavePlayerData player = new SavePlayerData();
        player.score = (float)gameSystemManager.GetDistance();
        return player;
    }

    public static void ResetDelegates(Scene scene)
    {
        OnIsGroundSwicthed = null;
        IsIngameSwitch = null;
    }

    public static void SaveHighScoreNCMB(NCMB.HighScore highScore, int score)
    {
        highScore.score = score;
        highScore.save();
    }

    public static string GetScoreText(float score)
    {
        double s = score;
        var t = s.ToReadableString();

        return t;
    }

    public static string GetScoreText(double score)
    {
        var t = score.ToReadableString();

        return t;
    }
}
