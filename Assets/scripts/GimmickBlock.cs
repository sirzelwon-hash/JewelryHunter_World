using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f;     // 自動落下検知距離
    public bool isDelete = false;   // 何かに触れたら削除するフラグ
    bool fade = false;            // 消滅開始フラグ
    float fadeTime = 0.5f;          // フェードアウト時間

    GameObject deadObj;             // 死亡当たり

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidbody2Dの物理挙動を停止
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
        deadObj = transform.Find("DeadObject").gameObject;  //死亡あたり取得
        deadObj.SetActive(false);                           //死亡あたりを非表示

        player = GameObject.FindGameObjectWithTag("Player"); // プレイヤーを探す
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // プレイヤーとの距離計測
            float d = Vector2.Distance(transform.position, player.transform.position);
            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static)
                {
                    // Rigidbody2Dの物理挙動を開始
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                    deadObj.SetActive(true);    //死亡あたりを表示
                }
            }
        }

        if (fade) //消滅開始フラグがオンになってから
        {
            // 透明値を変更してフェードアウトさせる
            fadeTime -= Time.deltaTime; // 前フレームの差分秒マイナス
            Color col = GetComponent<SpriteRenderer>().color;   // カラーを取り出す
            col.a = fadeTime;   // 透明値を変更
            GetComponent<SpriteRenderer>().color = col; // カラーを再設定する
            if (fadeTime <= 0.0f)
            {
                // 0以下(透明)になったら消す
                Destroy(gameObject);
            }
        }
    }

    // 接触開始
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete) //何かに触れたら消える設定の場合
        {
            fade = true; // 消滅開始フラグオン
        }
    }

    //範囲表示
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, length);
    }
}
