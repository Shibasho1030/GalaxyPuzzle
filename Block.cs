using UnityEngine;

public class Block : MonoBehaviour
{

    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    // TransformからBlockを作成するコンストラクタ
        public Block(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.rotation;
        }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 変数の作成
    // 回転していいブロックかどうか
    [SerializeField]
    private bool canRotate = true;

    // 関数の作成
    // 移動用
    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }
    // 移動関数を呼ぶ関数（4種類）
    public void MoveLeft()
    {
        Move(new Vector3(-1,0,0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1,0,0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0,1,0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0,-1,0));
    }
    
    // 回転用（2種類）
    public void RotateRight()
    {
        if(canRotate)
        {
            transform.Rotate(0,0,-90);
        }
    }

    public void RotateLeft()
    {
        if(canRotate)
        {
            transform.Rotate(0,0,90);
        }
    }

}
