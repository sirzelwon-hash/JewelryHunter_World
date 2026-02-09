using TMPro;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InactiveImage", 1.0f);  // 1秒後に画像を非表示にする　invokeは時間差(対象メソッド,時間差)
        panel.SetActive(false);         // パネルを非表示にする
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
        }
    }
}
