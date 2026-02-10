using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true;     // true=時間をカウントダウン計測する
    public float gameTime = 0;          // ゲームの最大時間
    bool isTimeOver = false;     // true=タイマー停止
    float displayTime = 0;       // UI用
    float times = 0;                    // 経過時間

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isCountDown)// カウントダウン
        {
            displayTime = gameTime;     //UI用の表示をゲームの最大時間と同じにする
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeOver == false)
        {
            times += Time.deltaTime;　// 経過時間を算出する

            if (isCountDown)        // カウントダウン
            {
                displayTime = gameTime - times + 1; // 残り時間をUI用の表示に反映 ※最初の1秒ずれを考慮
                if (displayTime <= 0.0f) // 時間切れ
                {
                    displayTime = 0.0f; // UI用の表示を0に整える
                    IsTimeOver(); //停止フラグを立てる
                }
            }
            else                    // カウントアップ
            {
                displayTime = times - 1; // 経過時間そのものをUI用の表示に反映 ※最初の1秒ずれを考慮
                if (displayTime >= gameTime) //時間切れ
                {
                    displayTime = gameTime; // UI用の表示をゲームの最大時間に整える
                    IsTimeOver(); //停止フラグを立てる
                }
            }
            Debug.Log("TIMES: " + displayTime);
        }
    }

    //停止フラグを立てるメソッド UIControllerからも呼び出せる
    public void IsTimeOver()
    {
        isTimeOver = true;
    }

    //UI用の表示時間を取得するメソッド
    public float GetDisplayTime()
    {
        return displayTime;
    }
}