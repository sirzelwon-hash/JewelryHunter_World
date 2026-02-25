using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState           // ゲームの状態　型を列挙型で作成している
{
    InGame,                     // ゲーム中
    GameClear,                  // ゲームクリア
    GameOver,                   // ゲームオーバー
    GameEnd,                    // ゲーム終了
}

public class GameManager : MonoBehaviour
{
    //ゲームの状態
    public static GameState gameState;
    public string nextSceneName;            // 次のシーン名

    //サウンド関連
    //public AudioClip meGameClear; //ゲームクリアの音源
    //public AudioClip meGameOver;  //ゲームオーバーの音
    //AudioSource soundPlayer; //AudioSorce型の変数

    public bool isGameClear = false;  //ゲームクリア判定
    public bool isGameOver = false; //ゲームオーバー判定

    //スコア追加
    public static int totalScore;  //合計スコア

    //ワールドマップで最後に入ったエントランスのドア番号
    public static int currentDoorNumber = 0;

    //所持アイテム　鍵の管理
    public static int keys = 1;
    //どのステージの鍵が入手済かを管理
    public static Dictionary<string, bool> keyGot;  //シーン名,true/false

    //所持アイテム　矢の管理
    public static int arrows = 10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameState = GameState.InGame;  //ゲームスタートしたらまずステータスをゲーム中にする
        //soundPlayer = GetComponent<AudioSource>();  //AudioSorceを参照する

        //KeyGotが何もない状態だったときのみ初期化
        if (keyGot == null)
        {
            keyGot = new Dictionary<string, bool>();
        }

        //もしも現シーン名がDictonary(KeyGot)に登録されていなければ
        if (!(keyGot.ContainsKey(SceneManager.GetActiveScene().name)))
        {
            //Dictonary(KeyGot)に登録しておく
            keyGot.Add(SceneManager.GetActiveScene().name, false);
        }
    }

    void Start()
    {
        string currentScane = SceneManager.GetActiveScene().name;

        if (currentScane != "WorldMap")
        {
            SoundManager.currentSoundManager.restartBGM = true;

            if (currentScane == "Boss")
            {
                SoundManager.currentSoundManager.StopBGM();

                SoundManager.currentSoundManager.PlayBGM(BGMType.InBoss);
            }

            else
            {
                SoundManager.currentSoundManager.StopBGM();

                SoundManager.currentSoundManager.PlayBGM(BGMType.InGame);
            }
        }

        else if (SoundManager.currentSoundManager.restartBGM)
        {
            SoundManager.currentSoundManager.StopBGM();

            SoundManager.currentSoundManager.PlayBGM(BGMType.Title);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameState == GameState.GameClear)
        {
            //soundPlayer.Stop(); //ステージ曲を止める
            SoundManager.currentSoundManager.StopBGM();

            //soundPlayer.PlayOneShot(meGameClear); //ゲームクリアの曲を1回だけ鳴らす
            SoundManager.currentSoundManager.PlayBGM(BGMType.GameClear);

            isGameClear = true; //クリアフラグ
            Invoke("GameStatusChange", 0.02f);
            //gameState = GameState.GameEnd; //ゲームの状態を更新
        }

        else if (gameState == GameState.GameOver)
        {
            //soundPlayer.Stop(); //ステージ曲を止める
            SoundManager.currentSoundManager.StopBGM();

            //soundPlayer.PlayOneShot(meGameOver); //ゲームオーバーの曲を1回だけ鳴らす
            SoundManager.currentSoundManager.PlayBGM(BGMType.GameOver);

            isGameOver = true;  //ゲームオーバーフラグ
            Invoke("GameStatusChange", 0.02f);
            //gameState = GameState.GameEnd; //ゲームの状態を更新
        }
    }

    //リスタート
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //次へ
    public void Next()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    //PlayerController経由でUIマップのSubmitが押されたとき呼び出される
    public void GameEnd()
    {
        //UI表示が終わって最後の状態であれば
        if (gameState == GameState.GameEnd)
        {
            //ゲームクリアの状態なら
            if (isGameClear)
            {
                Next();
            }

            //ゲームオーバーの状態なら
            else if (isGameOver)
            {
                Restart();
            }
        }
    }
}
