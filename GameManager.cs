using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
  
    //変数の作成
    //スポナー
    //生成されたブロック格納
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private KeySettingManager keySettingManager;
    Block activeBlock;
    Block backBlock1;
    Block backBlock2;
    Block backBlock3;
    Block airActiveBlock;
    //Block airBlock;

    // 変数の作成
    // 次にブロックが落ちるまでのインターバル時間
    // 次にブロックが落ちるまでの時間
    [SerializeField]
    private float dropInterval = 0.25f;
    float nextdropTimer;

    // 変数の作成
    // ボードのスクリプトを格納
    [SerializeField]
    private Board board;

    // 変数の作成
    // 入力受付タイマー(3種類)
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;
 
    // 入力インターバル(3種類)
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval, firstKeyLeftRightInterval;

    [SerializeField]
    private Transform airActiveBlockSprite;


    // 変数の作成
    // パネルの格納
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject nextPanel;

    [SerializeField]
    private GameObject gameStartPanel;

    // ゲームオーバー判定
    public bool gameOver;

    public bool gameStart = false;

    private bool isGameStart = false;

    private bool isGameOver = false;


    public GameObject bgmObject;
    public GameObject bgm2Object;

    //Sound用格納変数
    [SerializeField]
    private SoundManager soundManager;
 
    //一定時間経過後にブロック落ちるの早くする
    [SerializeField]
    private float speedUpRatio;

    public int gameScorePoint = 0;

    public TMP_Text uiGameScorePoint;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private GameObject skybox;



    //スポナーオブジェクトをスポナー変数に格納するコードの記述
    private void Start()
    {
        //spawner = GameObject.FindObjectOfType<Spawner>();

        // ボードを変数に格納する
        //board = GameObject.FindObjectOfType<Board>();

        //spawner.transform.position = Rounding.Round(spawner.transform.position);

        // タイマーの初期設定
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

        



        // ゲームオーバーパネルの非表示設定
        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }

        if (nextPanel.activeInHierarchy)
        {
            nextPanel.SetActive(false);

        }

        // if (!gameStartPanel.activeInHierarchy)
        // {
        //     gameStartPanel.SetActive(true);
        // }

        //60秒のタイマー毎にスピードアップさせる
        Invoke("OneMinutePassed", 60f);


    }
    

    private void Update() 
    {

        if (gameOver)
        {
            return;
        }
        else if (!gameStart)
        {
            return;
        }
        else if(audioManager.isSettingUIActive)
        {
            return;
        }
        PlayerInput();



        if (isGameOver)
        {
            bgmObject.SetActive(false);
            bgm2Object.SetActive(false);
            isGameOver = false;
        }

        if (isGameStart)
        {
            //bgmObject.SetActive(true);
            //bgm2Object.SetActive(true);
            isGameStart = false;
        }

        uiGameScorePoint.text = gameScorePoint.ToString(); 

    
    }


    // 関数の作成
    // キーの入力を検知してブロックを動かす関数
    // ボードの底に着いたときに次のブロックを生成する関数
    void PlayerInput()
    {
        if (Input.GetKey(keySettingManager.moveRightKey) && (Time.time > nextKeyLeftRightTimer) || Input.GetKeyDown(keySettingManager.moveRightKey))
        {
            activeBlock.MoveRight();//右に動かす

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (Input.GetKeyDown(keySettingManager.moveRightKey))
            {
                nextKeyLeftRightTimer += firstKeyLeftRightInterval;
            }

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }

            if (airActiveBlock)
            {
                DestroyAirActiveBlock();
                CreateAirActiveBlock();
            }
            DistanceCheck();
        }
        else if (Input.GetKey(keySettingManager.moveLeftKey) && (Time.time > nextKeyLeftRightTimer) || Input.GetKeyDown(keySettingManager.moveLeftKey))
        {
            activeBlock.MoveLeft();//左に動かす

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (Input.GetKeyDown(keySettingManager.moveLeftKey))
            {
                nextKeyLeftRightTimer += firstKeyLeftRightInterval;
            }

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
            if (airActiveBlock)
            {
                DestroyAirActiveBlock();
                CreateAirActiveBlock();
            }
            DistanceCheck();
        }
        else if (Input.GetKey(keySettingManager.rotateRightKey) && (Time.time > nextKeyRotateTimer) || Input.GetKeyDown(keySettingManager.rotateRightKey))
        {
            activeBlock.RotateRight();//右に回転させる
            soundManager.RotateSound(); 

            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                if (activeBlock.transform.position.x < 5)
                {
                    activeBlock.MoveRight();
                    
                    if (!board.CheckSidePosition(activeBlock))
                    {
                        activeBlock.MoveRight();
                        if (!board.BlockSideCheck(activeBlock))
                        {
                            activeBlock.MoveLeft();
                            activeBlock.RotateLeft();
                        }
                    }
                    else if (!board.BlockSideCheck(activeBlock))
                    {
                        activeBlock.MoveLeft();
                        activeBlock.RotateLeft();

                    }
                
                }
                else if (activeBlock.transform.position.x >= 5)
                {
                    activeBlock.MoveLeft();
                    
                    if (!board.CheckSidePosition(activeBlock))
                    {
                        activeBlock.MoveLeft();
                        if (!board.BlockSideCheck(activeBlock))
                        {
                            activeBlock.MoveRight();
                            activeBlock.RotateLeft();
                        }
                    }
                    else if (!board.BlockSideCheck(activeBlock))
                    {
                        activeBlock.MoveRight();
                        activeBlock.RotateLeft();

                    }
                
                }
                
                
                
            }
            if (airActiveBlock)
            {
                DestroyAirActiveBlock();
                CreateAirActiveBlock();
            }
            DistanceCheck(); 
        }
        else if (Input.GetKeyDown(keySettingManager.quicklyDownKey))
        {
            while(board.CheckPosition(activeBlock))//一番下まで動かす
            {
                activeBlock.MoveDown();
            }
            
             if (!board.CheckPosition(activeBlock))
            {
                if (board.OverLimit(activeBlock))
                {
                    GameOver();
                }
                else
                {
                    //そこに付いた時の処理 
                    BottomBoard();
                }
            }
        }
        else if (Input.GetKey(keySettingManager.moveDownKey) && (Time.time > nextKeyDownTimer) || (Time.time > nextdropTimer))
        {
            activeBlock.MoveDown();//下に動かす

            nextKeyDownTimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;

            DistanceCheck(); 
            
            if (Input.GetKeyDown(keySettingManager.moveDownKey))
            {
                nextKeyDownTimer += firstKeyLeftRightInterval;
            }

             if (!board.CheckPosition(activeBlock))
            {
                if (board.OverLimit(activeBlock))
                {
                    GameOver();
                }
                else
                {
                //そこに付いた時の処理
                BottomBoard();
                }
            }
        }

    }

    void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);
        board.ClearAllRows();//埋まっていれば削除する
        if (activeBlock.tag == "Bom")
        {
            board.BomBlock(activeBlock); 
            board.ClearAllRows();
        }

        activeBlock = backBlock1;
        activeBlock.transform.position = new Vector3(4.25f, 25, -1);
        activeBlock.transform.position = Rounding.Round(activeBlock.transform.position);

        backBlock1 = backBlock2;
        backBlock1.transform.position = new Vector3(11, 23, -1);
        backBlock1.transform.position = Rounding.Round(backBlock1.transform.position);

        backBlock2 = backBlock3;
        backBlock2.transform.position = new Vector3(11, 16, -1);
        backBlock2.transform.position = Rounding.Round(backBlock2.transform.position);
        
        backBlock3 = spawner.SpawnBlock();
        backBlock3.transform.position = new Vector3(11, 9, -1);
        backBlock3.transform.position = Rounding.Round(backBlock3.transform.position);

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;
        soundManager.BottomSound();


        if (airActiveBlock)
            {
                DestroyAirActiveBlock();
            }
        CreateAirActiveBlock();

    }

    // 関数の作成
    // ゲームオーバーになったらパネルを表示する
    void GameOver()
    {
        activeBlock.MoveUp();

        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }

        if (nextPanel.activeInHierarchy)
        {
            nextPanel.SetActive(false);

        }

        gameOver = true;

        soundManager.GameOverSound();

        isGameOver = true;

    }

    public void NormalModeStart()
    {
        spawner.NewBlocks = new Block[7];
        for (int i = 0; i < 7; i++)
        {
            if (spawner.Blocks[i])
            {
                spawner.NewBlocks[i] = spawner.Blocks[i];
            }
        }

        bgm2Object.SetActive(true);

        //スポナークラスからブロック生成関数を読んで変数に格納する
        if(!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
            activeBlock.transform.position = new Vector3(4.25f, 25, -1);
            activeBlock.transform.position = Rounding.Round(activeBlock.transform.position);
            CreateAirActiveBlock();
        }

        if(!backBlock1)
        {
            backBlock1 = spawner.SpawnBlock();
            backBlock1.transform.position = new Vector3(11, 23, -1);
            backBlock1.transform.position = Rounding.Round(backBlock1.transform.position);
        }

        if(!backBlock2)
        {
            backBlock2 = spawner.SpawnBlock();
            backBlock2.transform.position = new Vector3(11, 18, -1);
            backBlock2.transform.position = Rounding.Round(backBlock2.transform.position);
        }

        if(!backBlock3)
        {
            backBlock3 = spawner.SpawnBlock();
        }



        if (gameStartPanel.activeInHierarchy)
        {
            gameStartPanel.SetActive(false);
        }

        if (!nextPanel.activeInHierarchy)
        {
            nextPanel.SetActive(true);

        }

        gameStart = true;

        isGameStart = true;

    }

    public void HardModeStart()
    {
        spawner.NewBlocks = new Block[spawner.Blocks.Length];
        for (int i = 0; i < spawner.Blocks.Length; i++)
        {
            spawner.NewBlocks[i] = spawner.Blocks[i];
        }
    
        bgmObject.SetActive(true);

        //スポナークラスからブロック生成関数を読んで変数に格納する
        if(!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
            activeBlock.transform.position = new Vector3(4.25f, 25, -1);
            activeBlock.transform.position = Rounding.Round(activeBlock.transform.position);
            CreateAirActiveBlock();
        }

        if(!backBlock1)
        {
            backBlock1 = spawner.SpawnBlock();
            backBlock1.transform.position = new Vector3(11, 23, -1);
            backBlock1.transform.position = Rounding.Round(backBlock1.transform.position);
        }

        if(!backBlock2)
        {
            backBlock2 = spawner.SpawnBlock();
            backBlock2.transform.position = new Vector3(11, 16, -1);
            backBlock2.transform.position = Rounding.Round(backBlock2.transform.position);
        }

        if(!backBlock3)
        {
            backBlock3 = spawner.SpawnBlock();
        }



        if (gameStartPanel.activeInHierarchy)
        {
            gameStartPanel.SetActive(false);
        }

        if (!nextPanel.activeInHierarchy)
        {
            nextPanel.SetActive(true);

        }

        gameStart = true;

        isGameStart = true;

        skybox.SetActive(false);
    }

    // シーンを再読み込みする（ボタン押下で呼ぶ）
    public void Restart()
    {
        SceneManager.LoadScene(0);
        if (gameStartPanel.activeInHierarchy)
        {
            gameStartPanel.SetActive(false);
        }

        // if (!nextPanel.activeInHierarchy)
        // {
        //     nextPanel.SetActive(true);

        // }

        gameStart = true;

        isGameStart = true;

        soundManager.GameStartSound();
    }

    void CreateAirActiveBlock()
    {
        // foreach (Transform item in activeBlock.transform)
        // {
        //     Transform airBlock = Instantiate(airActiveBlockSprite, item.transform.position, Quaternion.identity);
        // }

        // // TransformをBlockに変換する
        // Transform transform = airBlock.transform; // 既存のTransform
        // Block airActiveBlock = new Block(transform); // Blockを生成
        
        //airActiveBlock = GameObject.Find<"airActiveBlock">;

        //spawner = GameObject.FindObjectOfType<Spawner>();
        

        //if(!airActiveBlock)
        {
            airActiveBlock = Instantiate(activeBlock);
        }

        while (board.CheckPosition(airActiveBlock))
        {
            airActiveBlock.MoveDown();//下に動かす
        }


        //nextAirActiveBlockDropTimer = Time.time;
        // if (Time.time > nextAirActiveBlockDropTimer)
        // {
        //      airActiveBlock.MoveDown();//下に動かす
        //      nextAirActiveBlockDropTimer = Time.time + airActiveBlockDropInterval;


        if (!board.CheckPosition(airActiveBlock))
        {
            //そこに付いた時の処理
            airActiveBlock.MoveUp();

            // if (airActiveBlock.GetComponent<MeshRenderer>() == null)
            // {
            //     airActiveBlock.gameObject.AddComponent<MeshRenderer>();
            // }
            // else
            // {
            //     Debug.Log("MeshRenderer はすでにこのオブジェクトにアタッチされています。");
            // }
            // GetComponent<MeshRenderer>().material.color = Color.red;


            // if (airActiveBlock != null)
            // {
            //     MeshRenderer blockRenderer = airActiveBlock.GetComponent<MeshRenderer>();
            //     if (blockRenderer != null)
            //     {
            //         // 色を変更
            //         blockRenderer.material.color = Color.red;
            //     }
            //     else
            //     {
            //         Debug.LogWarning("MeshRendererがairActiveBlockにアタッチされていません");
            //     }
            // }
            // else
            // {
            //     Debug.LogError("airActiveBlockが設定されていません");
            // }
        }

        


        
    }

    void DestroyAirActiveBlock()
    {
        if (airActiveBlock)
        {
            Destroy(airActiveBlock.gameObject);
        }
    }

    void OneMinutePassed()//60秒たった時に行う処理
    {
        dropInterval *= speedUpRatio;
        Invoke("OneMinutePassed", 60f);
    }


    void DistanceCheck()
    {
        if (!airActiveBlock)
        {
            CreateAirActiveBlock();
        }
        foreach (Transform airitem in airActiveBlock.transform)
            {
                foreach (Transform item in activeBlock.transform)
                {
                    if (item.position.y - airitem.position.y <3)
                    {
                        DestroyAirActiveBlock();
                    
                    }
                }
            }
        
    }

    
             
}
