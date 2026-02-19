using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class World_UIController : MonoBehaviour
{

    //各エントランスのDoorNumberごとに開錠か非開錠かをまとめておく
    public static Dictionary<int, bool> keyOpened;

    public TextMeshProUGUI keyText;
    int currentKeys;
    public TextMeshProUGUI arrowText;
    int currentArrows;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //WorldMap中のEntransseオブジェクトを集めて配列objに代入
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Entrance");

        //リストがない時の情報取得とセッティング
        if (keyOpened == null)
        {
            keyOpened = new Dictionary<int, bool>(); // 最初に初期化が必要

            //集めてきたEntranceを全点検
            for (int i = 0; i < obj.Length; i++)
            {
                //Entranceオブジェクト
                EntranceController entranceController = obj[i].GetComponent<EntranceController>();
                if (entranceController != null)
                {
                    keyOpened.Add(
                        entranceController.doorNumber,
                        entranceController.opened
                    );
                }
            }
        }

        //プレイヤーの位置
        player = GameObject.FindGameObjectWithTag("Player");
        //暫定のプレイヤーの位置
        Vector2 currentPlayerPos = Vector2.zero;

        //GameManagerに記録されているcurrentDoorNumberと一致するdoorNumberを持っているEntranceを探す
        for (int i = 0; i < obj.Length; i++)
        {
            //EntranceのEntranceControllerの変数doorNumberが、GameManagerの把握しているcurrentDoorNumberと同じかチェック
            if (obj[i].GetComponent<EntranceController>().doorNumber == GameManager.currentDoorNumber)
            {
                //暫定プレイヤーの位置を一致したEntranceオブジェクトの位置に書き換え
                currentPlayerPos = obj[i].transform.position;
            }
        }
        player.transform.position = currentPlayerPos;

    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
