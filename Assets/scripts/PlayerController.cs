using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody; 　　　　　　 //Rigidbody2D型の情報を持ってる変数の宣言
    float axisH = 0.0f; 　　　　　　//プレイヤー
    public float speed = 3.0f; 　　//移動速度　これを設定することでunity側のinspectorに項目が追加される
    public float jump = 9.0f; 　　　//ジャンプの力
    public LayerMask groundLayer; 　//着地できるレイヤー指定
    bool goJump = false;　　　　　　//ジャンプ開始フラグ
    bool onGround = false;     //地面フラグ

    // アニメーション対応
    Animator animator; // アニメーター

    //値はあくまでアニメーションクリップ名
    public string stopAnime = "Idle";
    public string moveAnime = "Run";
    public string jumpAnime = "Jump";
    public string goalAnime = "Goal";
    public string deadAnime = "Dead";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0;  //スコア

    InputAction moveAction; //Moveアクション
    InputAction jumpAction;　//jumpアクション
    PlayerInput input;  //PlayerInputコンポーネント

    GameManager gm; //GameManagerスクリプト

    void OnMove(InputValue value)
    {
        //取得した情報をVectoe2形式で抽出
        Vector2 moveInput = value.Get<Vector2>();
        axisH = moveInput.x;  //そ９のx成分をaxisHに代入
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            goJump = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() //1番最初に発動するものがスタートメソッド。基本的に1回のみの処理
    {
        //Rigidbody2Dを取ってくる
        rbody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();        // Animator を取ってくる
        nowAnime = stopAnime;                       // 停止から開始する
        oldAnime = stopAnime;                       // 停止から開始する

        input = GetComponent<PlayerInput>();  //PlayerInputコンポーネントの取得
        moveAction = input.currentActionMap.FindAction("Move");　//Moveアクションの取得
        jumpAction = input.currentActionMap.FindAction("Jump"); //Jumpアクション取得
        InputActionMap uiMap = input.actions.FindActionMap("ui");  //UIマップの取得
        uiMap.Disable();  //UIマップは無効化

        //GameObject型のアタッチされている特定のコンポーネントを探してくるメソッド
        gm = GameObject.FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()　//スタートが終わった後に永久ループするのがアップデートメソッド。入力を監視
    {

        if (GameManager.gameState != GameState.InGame)　　//ゲームマネージャーが持っているゲームステータスがIngameでなければなにもやらない
        {
            return;    //Updateを中断
        }

        //地上判定　Physics2Dの中にCircleCastというメソッドがあるのでそれを使う
        onGround = Physics2D.CircleCast(transform.position,  //発射位置　プレイヤーのピポット
                                        0.2f,　　　　　　　　//円の半径
                                        Vector2.down,　　　　//発射方向 downには(0,-1,0）情報が入っている
                                        0.0f,　　　　　　　　//発射距離
                                        groundLayer);　　　　//検出するレイヤー groundLayerとぶつかったら…
        //if (Input.GetButtonDown("Jump"))         //キャラクターをジャンプさせるキーが押されたか
        //{
        //    goJump = true;     //ジャンプフラグを立てる
        //}

        //InputActionのPlayerマップの"Jump"アクションに登録されたボタンが押されたか
        if (jumpAction.WasPressedThisFrame())
        {
            goJump = true;
        }

        ////水平方向のチェックをする　左右に関連するキーの値をaxisHに代入
        //axisH = Input.GetAxisRaw("Horizontal");

        //InputActionのPlayerマップの"Move"アクションに登録されたボタンをVector2形式で読み取り、そのうちx成分をaxisHに代入
        //axisH = moveAction.ReadValue<Vector2>().x;

        if (axisH > 0.0f) //向きの調節
        {
            //Debug.Log("右押されてる");
            transform.localScale = new Vector2(1, 1);　//右移動
        }
        else if (axisH < 0.0f)
        {
            //Debug.Log("左押されてる");
            transform.localScale = new Vector2(-1, 1);　//左右反転させる
        }

        // アニメーション更新
        if (onGround)       // 地面の上
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime; // 停止中
            }
            else
            {
                nowAnime = moveAnime; // 移動
            }
        }
        else                // 空中
        {
            nowAnime = jumpAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // アニメーション再生
        }

    }

    private void FixedUpdate()　//こちらもずっとループしている。入力に応じて動きを変える
    {

        if (GameManager.gameState != GameState.InGame)　　//ゲームマネージャーが持っているゲームステータスがIngameでなければなにもやらない
        {
            return;    //Updateを中断
        }

        if (onGround || axisH != 0)     //地面の上or速度が0ではない 
        {
            //速度を更新する
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)　　//地面の上でジャンプキーが押された場合
        {
            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);  //ジャンプさせるベクトルを作るjumpPwという変数。直下の行のために作成
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);  //瞬間的な力を加える (方向,どういう力のかけかたか）
            goJump = false;      //ジャンプフラグをおろす
        }
    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();　　　//ゴール！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     //ゲームオーバー！
        }

        else if (collision.gameObject.tag == "ScoreItem")
        {
            // スコアアイテム
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();  // ScoreItemを得る			
            score = item.itemdata.value;                // スコアを得る
            UIController ui = Object.FindFirstObjectByType<UIController>();      // UIControllerを探す
            if (ui != null)
            {
                ui.UpdateScore(score);                  // スコア表示を更新する
            }
            score = 0; //次に備えてスコアをリセット
            Destroy(collision.gameObject);              // アイテム削除する
        }
    }

    //ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        GameManager.gameState = GameState.GameClear;    //ステータス切り替え
        GameStop();    //プレイヤーのVelocityをストップする自作メソッド
    }
    //ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
        GameManager.gameState = GameState.GameOver;
        GameStop();    //プレイヤーのVelocityをストップ

        //ゲームオーバー演出
        GetComponent<CapsuleCollider2D>().enabled = false;   //当たり判定を消す
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);　　//上に少し跳ね上げる
        Destroy(gameObject, 2.0f);   //2秒後にヒエラルキーからオブジェクトを抹殺　(this.gameObject,何秒後に)
    }

    //プレイヤー停止
    void GameStop()
    {
        rbody.linearVelocity = new Vector2(0, 0);    //速度を0にして強制停止

        input.currentActionMap.Disable();
        input.SwitchCurrentActionMap("UI");　//アクションマップをUIマップに
        input.currentActionMap.Enable(); //UIマップを有効化
    }

    //UI表示にSubmitボタンが押されたら
    void OnSubmit(InputValue value)
    {
        //もしゲーム中でなければ
        if(GameManager.gameState != GameState.InGame)
        {
            //ゲームマネージャースクリプトのGameEndメソッドの発動
            gm.GameEnd();
        }
    }

    //プレイヤーのaxisH()の値を取得
    public float GetAxisH()
    {
        return axisH;
    }
}
