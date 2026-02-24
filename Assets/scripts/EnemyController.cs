using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          // 移動速度
    public bool isToRight = false;      // true=右向き　false=左向き
    public float revTime = 0;           // 反転時間
    public LayerMask groundLayer;       // 地面レイヤー
    bool onGround = false;              // 地面フラグ
    float time = 0;

    public float enemyLife = 3; //敵の体力
    bool inDamage;  //ダメージ管理フラグ

    Rigidbody2D rbody; //死亡演出のため

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1);// 向きの変更
        }

        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 地上判定
        onGround = Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        groundLayer);          // 検出するレイヤー
        if (revTime > 0)
        {
            time += Time.deltaTime;
            if (time >= revTime)
            {
                isToRight = !isToRight;     //フラグを反転させる
                time = 0;                   //タイマーを初期化
                if (isToRight)
                {
                    transform.localScale = new Vector2(-1, 1);  // 向きの変更
                }
                else
                {
                    transform.localScale = new Vector2(1, 1);   // 向きの変更
                }
            }
        }

        //ダメージ管理フラグが立っていたら点滅処理
        if (inDamage)
        {
            //三角関数Sinに角度（時間経過）を与えて正/負の値を算出
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0) //正なら表示
            {
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else //負なら非表示
            { GetComponent<SpriteRenderer>().enabled = false; }
        }
    }

    void FixedUpdate()
    {
        if (onGround)
        {
            // 速度を更新する
            // Rigidbody2D を取ってくる
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (isToRight)
            {
                rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);
            }
            else
            {
                rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y);
            }
        }
    }

    // 接触
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isToRight = !isToRight;     //フラグを反転させる
        time = 0;                   //タイマーを初期化
        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1); // 向きの変更
        }
        else
        {
            transform.localScale = new Vector2(1, 1); // 向きの変更
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inDamage)
        {

            //ぶつかった相手がArrowだったら
            if (collision.gameObject.tag == "Arrow")
            {
                //ぶつかった矢のスクリプトを取得
                 ArrowController arrowCnt = collision.gameObject.GetComponent<ArrowController>();
                //相手の変数attackpower分だけ体力を減らす
                enemyLife -= arrowCnt.attackPower;

                //ダメージ管理フラグを立てる
                inDamage = true;
                Invoke("DamageEnd", 0.25f);

                if (enemyLife <= 0) //死亡
                {
                    rbody.linearVelocity = Vector2.zero; //動きを止める
                    GetComponent<CircleCollider2D>().enabled = false;
                    rbody.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                    Destroy(gameObject, 0.3f);
                }
            }
        }
    }

    void DamageEnd()
    {
        inDamage = false; //フラグを戻す
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
