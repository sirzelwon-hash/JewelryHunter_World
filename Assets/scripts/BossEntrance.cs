using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEntrance : MonoBehaviour
{
    //各エントランスのクリア状況を管理
    public static Dictionary<int, bool> stagesClear;
    public string sceneName; //シーン切り替え先
    bool isOpened; //開き状況

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] obj =
            GameObject.FindGameObjectsWithTag("Entrance");

        //リストがない時の情報取得とセッティング
        if (stagesClear == null)
        {
            stagesClear = new Dictionary<int, bool>(); // 最初に初期化が必要

            //集めてきたEntranceを全点検
            for (int i = 0; i < obj.Length; i++)
            {
                //Entranceオブジェクトが持っているEntranceControllerを取得
                EntranceController entranceController = obj[i].GetComponent<EntranceController>();
                if (entranceController != null)
                {
                    //帳簿（KeyOpenedディクショナリー）に変数doorNumberと変数Opendの状況を記録
                    stagesClear.Add(
                        entranceController.doorNumber,
                        entranceController.opened
                    );
                }

            }
        }
        else  //データがあったら
        {
            int sum = 0; //クリアがどのくらいあるのかカウント用
            //エントランスの数分だけstagesClereの中身をチェック
            for (int i = 0; i < obj.Length; i++)
            {
                if (stagesClear[i])  //stagesClereディクショナリーの中身を順にチェック
                {
                    sum++;  //もしtrue(クリア済)ならカウント
                }

            }
            if (sum >= obj.Length)  //もしクリア（true）の数とEntranceの数が一致していたら
            {
                //全部クリアしたので扉を開ける
                GetComponent<SpriteRenderer>().enabled = false; //ボス扉の見た目を消す
                isOpened = true;  //扉が開いたという状態にする
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //触れた相手がPlayerかつ扉が開いていれば
        if (collision.gameObject.tag == "Player" && isOpened)
        {
            SceneManager.LoadScene(sceneName); //ボス部屋に行く
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
