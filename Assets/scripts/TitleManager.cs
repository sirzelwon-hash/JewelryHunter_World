using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string sceneName;            // 読み込むシーン名
    //public InputAction submitAction; //決定のInputAction


    //void OnEnable()
    //{
    //    submitAction.Enable();  //InputActionを有効化
    //}
    //void OnDisable()
    //{
    //    submitAction.Disable();  //InputActionを無効化
    //}


    //InputSystem?Actionsで決めたUIマップのSubmitアクションが押されたとき
    void OnSubmit(InputValue yaluse)
    {
        Load();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //if (submitAction.WasPressedThisFrame())
        //{
        //    Load();
        //}

        ////列挙型のKeyboard型の値を変数kbに代入
        //Keyboard kb = Keyboard.current;
        //if(kb != null) //キーボードが繋がっていれば
        //{
        //    if (kb.enterKey.wasPressedThisFrame) //エンターキーが押された状態なら
        //    {
        //        Load();
        //    }
        //}

    }

    // シーンを読み込む
    public void Load()
    {
        GameManager.totalScore = 0; //新しくゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);
    }
}