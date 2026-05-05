using UnityEngine;

public class Board : MonoBehaviour
{
    // 2次元配列の作成
    private Transform[,] grid; 

    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private SoundManager2 soundManager2;
    [SerializeField]
    private SoundManager3 soundManager3;

    public GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateBoard();

        //gameManager = new GameManager();
        //GameObject gameObject = new GameObject("GameManager");
        //gameObject.AddComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 変数の作成
    // ボード基板用のempty.pngの四角枠格納用変数を設定
    // ボードの高さ、幅、ヘッダー用数値
    [SerializeField]
    private Transform emptySprite;

    [SerializeField]
    private int height = 30,width = 10,header = 8;

    private void Awake()
    {
         grid = new Transform[width, height];
    }

    // 関数の作成
    // ボードを作成する関数の作成
    void CreateBoard()
    {
        if (emptySprite)
        {
            for (int y = 0; y < height - header; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone = Instantiate(emptySprite, new Vector3(x,y,0), Quaternion.identity);

                    //このボードスクリプトが付いているオブジェクトの子にするコード
                    clone.transform.parent = transform;
                }
            }
        }
    }

    //ブロックが枠内にあるのか判定する関数を呼ぶ関数
    public bool CheckPosition(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);
            if(!BoardOutCheck((int)pos.x,(int)pos.y))
            {
                return false;
            }

            if(BlockCheck((int)pos.x, (int)pos.y, block))
            {
                return false;
            }

        }

        return true;    
        
    }

    public bool CheckSidePosition(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);
            if (!BoardOutCheck((int)pos.x, (int)pos.y))
            {
                return false;
            }

        }
        return true;
    }

    public bool BlockSideCheck(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);
            if(BlockCheck((int)pos.x, (int)pos.y, block))
            {
                return false;
            }

        }
        return true;
    }

    bool BoardOutCheck(int x,int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    bool BlockCheck(int x, int y, Block block)
    {
        //二次元配列が空ではないのは他のブロックがある時
        return(grid[x,y] != null && grid[x,y].parent != block.transform);
    }

    // ブロックが落ちたポジションを記録する関数
    public void SaveBlockInGrid(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }



    // 関数の作成
    // すべての行をチェックして、埋まっていれば削除する関数
    public void ClearAllRows()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsComplete(y))
            {
                ClearRow(y);

                ShiftRowsDown(y);

                y--;

            }    
        }
    }

    // 全ての行をチェックする関数
    bool IsComplete(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x,y] == null)
            {
                return false;
            }
        }
        return true;
    }

    // 削除する関数
    void ClearRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x,y] != null)
            {
                Destroy(grid[x,y].gameObject);
                grid[x,y] = null;
                soundManager2.ClearRowSound();
                gameManager.gameScorePoint += 10;
            }  
        }
    }
 
    // 上にあるブロックを一段下げる関数
    void ShiftRowsDown(int startY)
    {
        for (int y = startY; y < height-1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(grid[x,y+1] != null)
                {
                    grid[x,y] = grid[x,y+1];
                    grid[x,y+1] = null;
                    grid[x,y].position += new Vector3(0,-1,0);
                }
            }
        }
    }
 
    public bool OverLimit(Block block)
    {
        foreach (Transform item in block.transform)
        {
            if (item.transform.position.y >= height - header-1)
            {
                return true;
            }

        }
        return false;
    }

    public void BomBlock(Block block)
    {
        Vector2 pos = Rounding.Round(block.transform.position);
            for (int x = Mathf.Clamp((int)pos.x - 2, 0, width - 1); x < Mathf.Clamp((int) pos.x + 4, 0, width); x++)
            {
                for (int y = Mathf.Clamp((int)pos.y - 2, 0, height - header); y < Mathf.Clamp((int)pos.y + 4, 0, height - header + 1); y++)
                {
                    if (grid[x,y] != null)
                    {
                        Destroy(grid[x,y].gameObject);
                        grid[x,y] = null;
                        soundManager3.BomSound();
                        gameManager.gameScorePoint += 5;
                    }
                
                }
            }
            for (int x = Mathf.Clamp((int)pos.x - 2, 0, width - 1); x < Mathf.Clamp((int) pos.x + 4, 0, width); x++)
            {
                for (int y = Mathf.Clamp((int)pos.y + 4, 4, height-header); y < height - header + 1; y++)
                {
                    if (grid[x,y] != null)
                    {
                        int i = 1;
                        while (y-i >= 0 && grid[x,y-i] == null)
                        {
                            grid[x,y-i] = grid[x,y-i+1];
                            grid[x,y-i+1] = null;
                            //if(grid[x,y-i] != null)
                            
                            grid[x,y-i].position += new Vector3(0,-1,0);
                            
                            i++;
                            if (y-i < 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        
    }

}

