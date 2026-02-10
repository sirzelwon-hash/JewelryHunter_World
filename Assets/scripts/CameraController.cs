using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float camLeft = 0.0f;   //カメラ左スクロールリミット
    public float camRight = 0.0f;　//カメラ右スクロールリミット
    public float camTop = 0.0f;　　//カメラ上スクロールリミット
    public float camBottom = 0.0f;　　//カメラ下スクロールリミット

    GameObject player;  //追随対象のプレイヤーオブジェクト
    PlayerController playerController;   //プレイヤーコントローラー

    //サブ背景
    public GameObject subBack1;
    public GameObject subBack2;
    public float subBackScrollSpeed = 0.005f;  //サブ背景スクロール速度

    //強制スクロール
    public bool isForceScrollX = false; //強制スクロールフラグ
    public float forceScrollSpeedX = 0.5f;  //1秒で動かすx距離
    public bool isForceScrollY = false;  //Y軸強制スクロールフラグ
    public float forceScrollSpeedY = 0.5f; //１秒で動かすY距離

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //プレイヤー情報をGameObjectクラスのFindWithTagメソッドで探し出す
        player = GameObject.FindWithTag("Player");
        //取得してきたプレイヤー情報に不随するPlayerControllerコンポーネントを取得する
        playerController = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) { return; }  //プレイヤー消失後は何もしない

        //カメラ制御
        float x;
        float y;

        //強制スクロールならカメラも強制で動く（ただしClampで制限をかける）
        if (isForceScrollX)  //横強制スクロール
        {
            x = transform.position.x + (forceScrollSpeedX * Time.deltaTime);
            x = Mathf.Clamp(x, camLeft, camRight);
        }

        else  //強制スクロールでないならプレイヤーの位置をカメラの位置にする
        {
            x = Mathf.Clamp(player.transform.position.x, camLeft, camRight);
        }

        if (isForceScrollY) //縦強制スクロール
        {
            y = transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            y = Mathf.Clamp(y, camLeft, camRight);
        }

        else //強制スクロールでないならプレイヤーの位置をカメラの位置にする
        {
            y = Mathf.Clamp(player.transform.position.y, camBottom, camTop);
        }
        Vector3 camPos = new Vector3(x, y, -10);  //カメラ位置のVector3を作る
        transform.position = camPos;  //カメラの更新座標


        //サブ背景スクロール処理
        //カメラのx座標がリミット内にある場合のみサブ背景のスクロール処理を行う
        if (x > camLeft && x < camRight)
        {
            //強制スクロールの場合
            if (isForceScrollX == true)
            {
                //Debug.Log("強制スクロールなら");

                //強制的にサブ背景が動く
                subBack1.transform.localPosition -= new Vector3(forceScrollSpeedX * subBackScrollSpeed, 0, 0);
                subBack2.transform.localPosition -= new Vector3(forceScrollSpeedX * subBackScrollSpeed, 0, 0);

                SubBackPositionChange("right");  //背景の入れ替え
            }

            else   //強制スクロールでなかった場合
            {
                //Debug.Log("強制スクロールではない"）;

                //もしも水平入力があったら
                if (playerController.GetAxisH() != 0)
                {
                    //PlayerControllerのaxisHの値（入力値）に応じてサブ背景が動く
                    subBack1.transform.localPosition -= new Vector3(playerController.GetAxisH() * subBackScrollSpeed, 0, 0);
                    subBack2.transform.localPosition -= new Vector3(playerController.GetAxisH() * subBackScrollSpeed, 0, 0);

                    //右方向入力時
                    if (playerController.GetAxisH() > 0)
                    {
                        SubBackPositionChange("right");  //背景の入れ替え
                    }
                    //左方向入力時
                    if (playerController.GetAxisH() < 0)
                    {
                        SubBackPositionChange("left");　//背景の入れ替え
                    }

                }
            }
        }
    }

    //特定のポイントまでサブ背景が移動したら位置をワープさせる関数
    //引数に"right"か"left"を指定することでワープの方向を決定する
    void SubBackPositionChange(string vector)
    {
        if (vector == "right")//右方向を入力時
        {
            //もしもサブ背景1のローカルX座標が-19.2f以下になったら
            if (subBack1.transform.localPosition.x <= -19.2f)
            {
                //誤差記録用
                float diff = -19.2f - subBack1.transform.localPosition.x;
                //サブ背景1のローカルX座標を19.2fから誤差調整した位置にワープする
                subBack1.transform.localPosition = new Vector3(
                    19.2f - diff,
                    subBack1.transform.localPosition.y,
                    subBack1.transform.localPosition.z);
            }
            //サブ背景2についてもサブ背景1と同様の処理を行う
            if (subBack2.transform.localPosition.x <= -19.2f)
            {
                float diff = -19.2f - subBack2.transform.localPosition.x;
                subBack2.transform.localPosition = new Vector3(
                    19.2f - diff,
                    subBack2.transform.localPosition.y,
                    subBack2.transform.localPosition.z);
            }
        }
        else if (vector == "left") //左方向を入力時
        {
            //もしもサブ背景1のローカルX座標が19.2f以上になったら
            if (subBack1.transform.localPosition.x >= 19.2f)
            {
                //誤差記録用
                float diff = subBack1.transform.localPosition.x - 19.2f;
                //サブ背景1のローカルX座標を-19.2fから誤差調整した位置にワープする
                subBack1.transform.localPosition = new Vector3(
                    -19.2f + diff,
                    subBack1.transform.localPosition.y,
                    subBack1.transform.localPosition.z);
            }
            //サブ背景2についてもサブ背景1と同様の処理を行う
            if (subBack2.transform.localPosition.x >= 19.2f)
            {
                float diff = subBack2.transform.localPosition.x - 19.2f;
                subBack2.transform.localPosition = new Vector3(
                    -19.2f + diff,
                    subBack2.transform.localPosition.y,
                    subBack2.transform.localPosition.z);
            }
        }
    }
}
