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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.InGame;  //ゲームスタートしたらまずステータスをゲーム中にする
    }

    // Update is called once per frame
    void Update()
    {

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
}
