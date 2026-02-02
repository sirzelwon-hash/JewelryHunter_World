using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody; //Rigidbody2D型の変数
    float axisH = 0.0f; //入力

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() //1番最初に発動するものがスタートメソッド。基本的に1回のみの処理
    {
        //Rigidbody2Dを取ってくる
        rbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()　//スタートが終わった後に永久ループするのがアップデートメソッド。入力を監視
    {
        //水平方向のチェックをする
        axisH = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()　//こちらもずっとループしている。入力に応じて動きを変える
    {
        //速度を更新する
        rbody.linearVelocity = new Vector2(axisH * 3.0f,rbody.linearVelocity.y);
    }
}
