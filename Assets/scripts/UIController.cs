using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;               // UIを使うのに必要

public class UIController : MonoBehaviour
{

    public GameObject mainImage;        // 画像を持つImageゲームオブジェクト
    public Sprite gameOverSpr;          // GAME OVER画像
    public Sprite gameClearSpr;         // GAME CLEAR画像
    public GameObject panel;            // パネル
    public GameObject restartButton;    // RESTARTボタン
    public GameObject nextButton;       // NEXTボタン

    // 時間制限追加
    public GameObject timeBar;          // 時間表示バー
    public GameObject timeText;         // 時間テキスト
    TimeController timeController;             // TimeController
    bool useTime = true;               // 時間制限を使うかどうかのフラグ

    // プレイヤー情報
    GameObject player;
    PlayerController playerController;

    //スコア追加
    public GameObject scoreText;  //スコアテキスト
    public int stageScore = 0;  //ステージスコア

    //矢と鍵の表示更新
    public TextMeshProUGUI keyText;
    int currentKeys;
    public TextMeshProUGUI arrowText;
    int currentArrows;

    //体力の表示更新
    public Slider lifeSlider;
    int currentLife;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InactiveImage", 1.0f);  // 1秒後に画像を非表示にする　invokeは時間差(対象メソッド,時間差)
        panel.SetActive(false);         // パネルを非表示にする

        // 時間制限のプログラム
        timeController = GetComponent<TimeController>();   // TimeControllerを取得
        if (timeController != null)
        {
            if (timeController.gameTime == 0.0f) //もしgameTimeがもともと0なら時間制限は設けない
            {
                timeBar.SetActive(false);   // 制限時間なしなら隠す
                useTime = false;　//時間制限を使わないフラグ
            }
        }

        //プレイヤー情報とPlayerControllerコンポーネントの取得
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        //スコア追加
        UpdateScore();
    }

    // 画像を非表示にする　自作メソッド　invokeで指名できるようにするためわざわざメソッド化する
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameState.GameClear)
        {
            // ゲームクリア
            mainImage.SetActive(true);  // 画像を表示する
            panel.SetActive(true);      // ボタン（パネル）を表示する
            // RESTARTボタンを無効化する
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;  // 画像を設定する

            //時間カウントを停止
            if (timeController != null)
            {
                timeController.IsTimeOver();  //停止フラグ

                //整数に型変形することで少数を切り捨てる
                int time = (int)timeController.GetDisplayTime();
                GameManager.totalScore += time * 10;  //残り時間をスコアに加える
            }

            GameManager.totalScore += stageScore; //トータルスコアの最終確定
            stageScore = 0;  //ステージスコアリセット
            UpdateScore(); 　//スコア表示の更新
        }

        else if (GameManager.gameState == GameState.GameOver)
        {
            // ゲームオーバー
            mainImage.SetActive(true);  // 画像を表示する
            panel.SetActive(true);      // ボタン（パネル）を表示する
            // NEXTボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;       // 画像を設定する

            //時間カウントを停止
            if (timeController != null)
            {
                timeController.IsTimeOver();  //停止フラグ
            }
        }


        else if (GameManager.gameState == GameState.InGame)
        {
            if (player == null)  //プレイヤー消滅後は何もしない
            {
                return;
            }

            //タイムを更新する
            if (timeController != null && useTime)
            {
                // float型のUI用表示変数を取得し、整数に型変換することで小数を切り捨てる
                int time = (int)timeController.GetDisplayTime();  //int型に型変換し、小数点切り捨て
                // タイム更新
                timeText.GetComponent<TextMeshProUGUI>().text = time.ToString();　//TextMeshProUGUIはUIのtext。オブジェクトのtextもまた別にTextMeshProもある

                if (useTime && timeController.isCountDown && time <= 0) //カウントダウンモードで時間が0なら
                {
                    playerController.GameOver(); // ゲームオーバーにする
                }
                else if (useTime && !timeController.isCountDown && time >= timeController.gameTime) //カウントアップモードで制限時間を超えたら
                {
                    playerController.GameOver(); // ゲームオーバーにする 
                }
            }
        }

        //把握していた鍵の数とGameManagerの鍵の数に違いが出たら、正しい数にUIを更新
        if (currentKeys != GameManager.keys)
        {
            currentKeys = GameManager.keys;
            keyText.text = currentKeys.ToString();
        }
        //把握していた矢の数とGameManagerの矢の数に違いが出たら、正しい数となるようにUIを更新
        if (currentArrows != GameManager.arrows)
        {
            currentArrows = GameManager.arrows;
            arrowText.text = currentArrows.ToString();
        }

        //把握していた体力と？？？の体力に違いが出たら、正しい値になるようにUIを更新
        if(currentLife != PlayerController.playerLife)
        {
            currentLife = PlayerController.playerLife;
            lifeSlider.value = currentLife;
            
        }
    }

    // 現在スコアのUI表示更新
    void UpdateScore()
    {
        int currentScore = stageScore + GameManager.totalScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = currentScore.ToString();
    }

    // プレイヤーから呼び出される 獲得スコアを追加した上でのUI表示更新
    public void UpdateScore(int score)
    {
        stageScore += score;
        int currentScore = stageScore + GameManager.totalScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = currentScore.ToString();
    }
}
