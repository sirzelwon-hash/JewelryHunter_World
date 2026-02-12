using UnityEngine;

public class SwitchAction : MonoBehaviour
{
    public GameObject targetMoveBlock; //連動する対象のブロック
    public Sprite imageOff;　//スイッチオフの時に表示したいスプライト
    public Sprite imageOn;　//スイッチオンの時に表示したいスプライト
    public bool on = false; // スイッチの状態(true:押されている false:押されていない)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (on)
        {
            GetComponent<SpriteRenderer>().sprite = imageOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = imageOff;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D col)
    {
        //プレイヤーと接触した時
        if (col.gameObject.tag == "Player")
        {
            //もしスイッチがもともとONであれば
            if (on)
            {
                //OFFにする
                on = false; 
                GetComponent<SpriteRenderer>().sprite = imageOff;
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock>();
                movBlock.Stop();　//ブロックの動きも止める
            }
            else　//もしスイッチがもともとOFFであれば
            {
                //ONにする
                on = true;
                GetComponent<SpriteRenderer>().sprite = imageOn;
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock>();
                movBlock.Move();//ブロックの動きを開始する
            }
        }
    }
}
