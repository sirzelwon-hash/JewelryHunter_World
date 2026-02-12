using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;          //X移動距離
    public float moveY = 2.0f;          //Y移動距離
    public float times = 3.0f;          //片道の時間
    public float wait = 0.0f;           //インターバルの時間
    public bool isMoveWhenOn = false;   //乗ってから動くかどうかフラグ
    public bool isCanMove = true;       //動作フラグ

    Vector3 startPos;                   //初期位置
    Vector3 endPos;                     //移動位置
    bool isReverse = false;             //反対方向フラグ

    float movep = 0;                    //移動補完値：進捗ともいえる/全体を1とした時の割合

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;                                 //初期位置
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY);  //移動位置
        //もし乗ってから動くフラグが真なら
        if (isMoveWhenOn)
        {
            //乗った時に動くので最初は動かさない
            isCanMove = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //動作フラグが経っている時
        if (isCanMove)
        {

            float distance = Vector2.Distance(startPos, endPos);        //移動すべき全体の距離
            float ds = distance / times;                                //1秒あたりに移動すべき距離
            float df = ds * Time.deltaTime;                             //1フレームあたりに移動すべき距離

            movep += df / distance;                                     //移動補完値：全体に対してどこまで移動完了すべきか割合

            //逆方向のフラグが立っていなければ
            if (!isReverse)
            {
                //スタート～エンド間の、今いるべき進捗をmovepで提示
                transform.position = Vector2.Lerp(startPos, endPos, movep);  //正移動                
            }
            else　//逆向きのフラグが立っている
            {
                //エンド～スタート間の、今いるべき進捗をmovepで提示
                transform.position = Vector2.Lerp(endPos, startPos, movep);  //逆移動
            }
            if (movep >= 1.0f)//進捗が1に届いたら（ゴールにきたら）
            {
                movep = 0.0f;                   //逆方向に備えて移動補完値をリセット
                isReverse = !isReverse;         //逆転フラグを切り替える
                Stop();                         //Stopメソッドを通していったん停止しておく

                //もし乗ってから動くフラグのない床であれば自動で逆方向への移動を再開したい
                if (isMoveWhenOn == false)
                {
                    Invoke("Move", wait);       //wait秒後にMoveメソッドを発動（動き始める）
                }
            }
        }
    }

    //移動フラグを立てることで動かす
    public void Move()
    {
        isCanMove = true;
    }

    //移動フラグを下ろして止める
    public void Stop()
    {
        isCanMove = false;
    }

    //接触したら処理
    void OnCollisionEnter2D(Collision2D collision)
    {
        //接触したのがプレイヤーなら移動床の子にする
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーの親を自分（床）にする
            collision.transform.SetParent(transform);

            //乗った時に動くフラグが立っているなら
            if (isMoveWhenOn)
            {                
                isCanMove = true;   //プレイヤーと接触することで移動フラグが立つことになる
            }
        }
    }
    //接触が終わったら処理
    void OnCollisionExit2D(Collision2D collision)
    {
        //接触したのがプレイヤーなら
        if (collision.gameObject.tag == "Player" && collision.gameObject.transform.parent != null)
        {
            //プレイヤーの親をなしにする（親子関係の解消）
            collision.transform.SetParent(null);
        }
    }
    //移動範囲表示
    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //移動線
        Gizmos.DrawLine(fromPos, new Vector2(fromPos.x + moveX, fromPos.y + moveY));
        //スプライトのサイズ
        Vector2 size = GetComponent<SpriteRenderer>().size;
        //初期位置
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));
        //移動位置
        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
