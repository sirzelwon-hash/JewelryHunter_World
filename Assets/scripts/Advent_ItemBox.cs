using UnityEngine;
using UnityEngine.SceneManagement;

public class Advent_ItemBox : MonoBehaviour
{
    public Sprite openImage;  //開いたときの画像
    public GameObject itemPrefab; //宝箱の中に格納するオブジェクト
    public bool isClosed = true; //閉じているかのフラグ
    public AdventItemType type = AdventItemType.None; //宝箱のタイプ


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //もしも自分のタイプがKeyだったら
        if (type == AdventItemType.Key)
        {
            //もしもGameManagerのKeyGotディクショナリーの該当シーンがtrue(取得済)だったら
            if (GameManager.keyGot[SceneManager.GetActiveScene().name])
            {
                //close状態をfalseにする
                isClosed = false;
                //見た目をオープンの絵にすること
                GetComponent<SpriteRenderer>().sprite = openImage;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //宝箱がクローズのとき、触れた相手のタグがPlayerなら
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isClosed && collision.gameObject.tag == "Player")
        {
            //宝箱の絵をOpenの絵に変更
            GetComponent<SpriteRenderer>().sprite = openImage;
            //Closeのフラグを解除
            isClosed = false;

            //その場に変数に指定したプレハブオブジェクトを生成
            //（もし変数にプレハブオブジェクトが指定されていれば）
            if (itemPrefab != null)
            {
                Instantiate
                    (itemPrefab, //生成する対象オブジェクト
                    transform.position,　//生成位置
                    Quaternion.identity);　//生成時の回転、今回は無回転
            }
        }
    }
}
