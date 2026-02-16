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
    public AudioClip meGameClear; //ゲームクリアの音源
    public AudioClip meGameOver;  //ゲームオーバーの音
    AudioSource soundPlayer; //AudioSorce型の変数

    public bool isGameClear = false;  //ゲームクリア判定
    public bool isGameOver = false; //ゲームオーバー判定

    //スコア追加
    public static int totalScore;  //合計スコア

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.InGame;  //ゲームスタートしたらまずステータスをゲーム中にする
        soundPlayer = GetComponent<AudioSource>();  //AudioSorceを参照する
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameState == GameState.GameClear)
        {
            soundPlayer.Stop(); //ステージ曲を止める
            soundPlayer.PlayOneShot(meGameClear); //ゲームクリアの曲を1回だけ鳴らす
            isGameClear = true; //クリアフラグ
            gameState = GameState.GameEnd; //ゲームの状態を更新
        }

        else if (gameState == GameState.GameOver)
        {
            soundPlayer.Stop(); //ステージ曲を止める
            soundPlayer.PlayOneShot(meGameOver); //ゲームオーバーの曲を1回だけ鳴らす
            isGameOver = true;  //ゲームオーバーフラグ
            gameState = GameState.GameEnd; //ゲームの状態を更新
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
