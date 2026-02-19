using NUnit.Framework.Constraints;
using System.Diagnostics;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

//どのアイテムタイプか
public enum AdventItemType
{
    None,
    Arrow,
    Key,
    Life
}

public class Advent_Item : MonoBehaviour
{
    //オブジェクトのアイテムタイプ
    public AdventItemType type = AdventItemType.None;
    public int numberOfArrow = 10;　//タイプがArrowだった場合の増加量
    public int recoveryValue = 1;　//タイプがLifeだった場合の回復量

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤータグと接触したときのタイプ毎の処理
        if (collision.gameObject.tag == "Player")
        {
            switch (type) //自分のタイプが対象
            {
                case AdventItemType.Key:
                    GameManager.keys++; //KeyGotに登録
                    GameManager.keyGot[SceneManager.GetActiveScene().name] = true;
                    break;

                case AdventItemType.Arrow:
                    GameManager.arrows += numberOfArrow;
                    break;

                case AdventItemType.Life:
                    PlayerController.PlayerRecovery(recoveryValue);
                    break;
            }

            // アイテムゲット演出
            GetComponent<CircleCollider2D>().enabled = false;      // 当たりを消す
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            rbody.gravityScale = 1.0f; //重力を戻す
            rbody.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); // 上に少し跳ね上げる
            Destroy(gameObject, 0.5f); // 1秒後にヒエラルキーからオブジェクトを抹消
        }
    }
}
