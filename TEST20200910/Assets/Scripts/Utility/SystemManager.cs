using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SystemManager
{
    private static bool isGrounded = false;

    private static GameSystemManager gameSystemManager = null;

    public static GravityWall gravity = null;
    public static PlayerManager player = null;
    public static float gravityPower = 0;
    public static DebugTextDisplayer debug_text = null;

    public delegate void IsGroundedChangeDelegate(bool isGrounded);
    public static event IsGroundedChangeDelegate OnIsGroundSwicthed;

    public delegate void IsIngameSwitchDelegate(bool active);
    public static event IsIngameSwitchDelegate IsIngameSwitch;

    private static readonly string SaveDataFolderPath = Path.Combine(Application.dataPath, @"SaveData");
    private static readonly string SaveDataFilePath = Path.Combine(SaveDataFolderPath, "saveData");

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
        SavePlayerData player = CreateSavePlayerData();
        // バイナリ形式でシリアル化
        BinaryFormatter bf = new BinaryFormatter();

        // 指定したパスにファイルを作成
        FileStream file = File.Create(SaveDataFilePath);

        try
        {
            // 指定したオブジェクトを上で作成したストリームにシリアル化する
            bf.Serialize(file, player);
        }
        finally
        {
            // ファイル操作には明示的な破棄が必要です。Closeを忘れないように。
            if (file != null)
                file.Close();
        }
    }

    public static float LoadPlayerData()
    {
        if (File.Exists(SaveDataFilePath))
        {
            // バイナリ形式でデシリアライズ
            BinaryFormatter bf = new BinaryFormatter();
            // 指定したパスのファイルストリームを開く
            FileStream file = File.Open(SaveDataFilePath, FileMode.Open);
            try
            {
                // 指定したファイルストリームをオブジェクトにデシリアライズ。
                SavePlayerData player = (SavePlayerData)bf.Deserialize(file);
                return player.score;
            }
            finally
            {
                // ファイル操作には明示的な破棄が必要です。Closeを忘れないように。
                if (file != null)
                    file.Close();
            }
        }
        else
        {
            Debug.Log("no load file");
        }

        return -1;
    }

    private static SavePlayerData CreateSavePlayerData()
    {
        SavePlayerData player = new SavePlayerData();
        player.score = gameSystemManager.GetDistance();
        return player;
    }
}
