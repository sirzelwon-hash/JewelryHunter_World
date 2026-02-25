using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public string sceneName;            // 読み込むシーン名

    public GameObject startButton; //スタートボタンオブジェクト
    public GameObject continueButton; //コンティニューボタンオブジェクト


    //public InputAction submitAction; //決定のInputAction;

    //void OnEnable()
    //{
    //    submitAction.Enable();  //InputActionを有効化
    //}
    //void OnDisable()
    //{
    //    submitAction.Disable();  //InputActionを無効化
    //}


    //InputSystem?Actionsで決めたUIマップのSubmitアクションが押されたとき
    void OnSubmit(InputValue yaluse)
    {
        Load();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // PlayerPrefsからJSON文字列をロード
        string jsonData = PlayerPrefs.GetString("SaveData");

        // JSONデータが存在しない場合、エラーを回避し処理を中断
        if (string.IsNullOrEmpty(jsonData))
        {
            continueButton.GetComponent<Button>().interactable = false;  //ボタン機能を無効
        }

        SoundManager.currentSoundManager.StopBGM(); //BGMをストップ
        SoundManager.currentSoundManager.PlayBGM(BGMType.Title); //タイトルのBGMを再生
    }

    // Update is called once per frame
    void Update()
    {

        //if (submitAction.WasPressedThisFrame())
        //{
        //    Load();
        //}

        ////列挙型のKeyboard型の値を変数kbに代入
        //Keyboard kb = Keyboard.current;
        //if(kb != null) //キーボードが繋がっていれば
        //{
        //    if (kb.enterKey.wasPressedThisFrame) //エンターキーが押された状態なら
        //    {
        //        Load();
        //    }
        //}

    }

    // シーンを読み込む
    public void Load()
    {
        SaveDataManager.Initialize(); //セーブデータを初期化する
        GameManager.totalScore = 0; //新しくゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);
    }

    //セーブデータを読み込んでから始めるメソッド
    public void ContinueLoad()
    {
        SaveDataManager.LoadGameData(); //セーブデータを読み込む
        SceneManager.LoadScene(sceneName);
    }
}