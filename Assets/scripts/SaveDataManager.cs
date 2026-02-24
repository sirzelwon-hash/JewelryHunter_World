using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SaveDataManager : MonoBehaviour
{

    // ゲームデータをPlayerPrefsに保存するメソッド
    public static void SaveGamedata()
    {
        SaveData saveData = new SaveData(); //saveDataクラスを実体化
        //記憶したい値をsaveDataクラスの変数にまとめる
        saveData.saveDoorNumber = GameManager.currentDoorNumber;
        saveData.saveTotalScore = GameManager.totalScore;
        saveData.saveKeys = GameManager.keys;

        // Dictionary -> List<KeyGotEntry> への変換
        saveData.saveKeyGot = new List<KeyGotEntry>(); // リストを初期化
        if (GameManager.keyGot != null) // GameManager.keyGot が null でないことを確認
        {
            foreach (var entry in GameManager.keyGot)
            {
                saveData.saveKeyGot.Add(new KeyGotEntry { key = entry.Key, got = entry.Value });
            }
        }

        // Dictionary -> List<KeyOpened> への変換
        saveData.saveKeyOpened = new List<KeyOpenedEntry>(); // リストを初期化
        if (World_UIController.keyOpened != null) // GameManager.keyGot が null でないことを確認
        {
            foreach (var entry in World_UIController.keyOpened)
            {
                saveData.saveKeyOpened.Add(new KeyOpenedEntry { doorNumber = entry.Key, opened = entry.Value });
            }
        }

        saveData.saveArrows = GameManager.arrows;

        //SaveDataクラスのインスタンスをJSON文字列に変換
        string jsonData = JsonUtility.ToJson(saveData);

        //JSON文字列をPlayerPrefsに保存
        PlayerPrefs.SetString("SaveData", jsonData); //セッティング
        PlayerPrefs.Save(); //変更を書き込む

        Debug.Log("セーブしました：" + jsonData);
        Debug.Log(GameManager.keyGot);
    }


    // PlayerPrefsからJSONをロードし、ゲームデータに適用するメソッド
    public static void LoadGameData()
    {
        // PlayerPrefsからJSON文字列をロード
        string jsonData = PlayerPrefs.GetString("SaveData");

        // JSONデータが存在しない場合、エラーを回避し処理を中断
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogWarning("セーブデータが見つかりません");
            Initialize(); // 初期化する
            return;
        }

        // JSON文字列をSaveDataクラスのインスタンスに変換して初期化
        SaveData loadData = JsonUtility.FromJson<SaveData>(jsonData);

        // ロードしたデータを各static変数に適用
        GameManager.currentDoorNumber = loadData.saveDoorNumber;
        GameManager.totalScore = loadData.saveTotalScore;
        GameManager.keys = loadData.saveKeys;

        //List<KeyGotEntry> -> Dictionary への変換
        GameManager.keyGot = new Dictionary<string, bool>(); // Dictionaryを初期化
        if (loadData.saveKeyGot != null) // ロードしたリストが null でないことを確認
        {
            foreach (var entry in loadData.saveKeyGot)
            {
                GameManager.keyGot.Add(entry.key, entry.got);
            }
        }

        //List<KeyOpenedEntry> -> Dictionary への変換
        World_UIController.keyOpened = new Dictionary<int, bool>(); // Dictionaryを初期化
        if (loadData.saveKeyOpened != null) // ロードしたリストが null でないことを確認
        {
            foreach (var entry in loadData.saveKeyOpened)
            {
                World_UIController.keyOpened.Add(entry.doorNumber, entry.opened);
            }
        }

        GameManager.arrows = loadData.saveArrows;

        //WorldMapに行く
        SceneManager.LoadScene("WorldMap");
    }

    //データの初期化
    public static void Initialize()
    {
        PlayerPrefs.DeleteAll(); //全部消す
        GameManager.currentDoorNumber = 0;
        GameManager.totalScore = 0;
        GameManager.keys = 0;
        if (GameManager.keyGot != null)
        {
            GameManager.keyGot.Clear(); //ディクショナリーを削除
        }
        if (World_UIController.keyOpened != null)
        {
            World_UIController.keyOpened.Clear(); //ディクショナリーを削除
        }
        GameManager.arrows = 10;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


[System.Serializable] // JsonUtility でシリアライズ可能（JSON化の準備）にするために必要
public class SaveData
{
    //ワールドマップにおける位置
    public int saveDoorNumber;

    // スコア
    public int saveTotalScore;

    //鍵の数
    public int saveKeys;

    // Dictionary の代わりに List を使用
    //鍵の取得状況
    public List<KeyGotEntry> saveKeyGot = new List<KeyGotEntry>();
    //鍵の開錠状況
    public List<KeyOpenedEntry> saveKeyOpened = new List<KeyOpenedEntry>();

    //矢の数
    public int saveArrows;

    //体力
    public int savePlayerLife;
}

// GamaManager.keyGotをList型で管理するためのクラス
[System.Serializable]
public class KeyGotEntry
{
    public string key;
    public bool got;
}

// GamaManager.keyGotをList型で管理するためのクラス
[System.Serializable]
public class KeyOpenedEntry
{
    public int doorNumber;
    public bool opened;
}