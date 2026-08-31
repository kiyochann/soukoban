using Const;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.Rendering.DebugUI;
public class GameManager : MonoBehaviour
{
    //[SerializeField] private GameObject outputObject;
    private outputManager outPutManager;

    private MapData mapdata;

    private int score;

    private Vector2 playerPosition;
    private char playerDirection;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject block_a, block_b;
    
    [SerializeField] private GameObject cantUndoButton;
    
    [SerializeField] private GameObject pauseObject;
    


    private MoveToPosition playerMover;
    private MoveToPosition block_aMover;
    private MoveToPosition block_bMover;

    private Block block_aSprite;
    private Block block_bSprite;

    private bool isMoving;
    private int completedBlocks;

    private int firstStagePar = 15;

    private Vector3 worldStartPos;
    private Vector2 arrayStartPos;
    private bool isInitialized = false;

    public bool isUndo = false;
    public bool isRestart = false;
    public bool isPause = false;

    public bool isPlaying = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outPutManager = GetComponent<outputManager>();

        playerMover = player.GetComponent<MoveToPosition>();
        block_aMover = block_a.GetComponent<MoveToPosition>();
        block_bMover = block_b.GetComponent<MoveToPosition>();

        block_aSprite = block_a.GetComponent<Block>();
        block_bSprite = block_b.GetComponent<Block>();

        


        Map startMap = new Map();
        
        startMap.tileMap = new char[Const.CONST.MAPSIZE, Const.CONST.MAPSIZE]
        {//    0   1   2   3   4   5   6   7
            { ' ',' ',' ','w','w','w','w',' ' },// 0
            { ' ',' ',' ','w','g','x','w',' ' },// 1
            { ' ',' ',' ','w','g','g','w',' ' },// 2
            { ' ',' ',' ','w','g','g','w',' ' },// 3
            { ' ',' ',' ','w','g','g','w',' ' },// 4
            { ' ',' ',' ','w','x','g','w',' ' },// 5
            { ' ',' ',' ','w','g','g','w',' ' },// 6
            { ' ',' ',' ','w','w','w','w',' ' } // 7
        };
        startMap.entityMap = new char[Const.CONST.MAPSIZE, Const.CONST.MAPSIZE]
        {//    0   1   2   3   4   5   6   7
            { ' ',' ',' ',' ',' ',' ',' ',' ' },// 0
            { ' ',' ',' ',' ','>',' ',' ',' ' },// 1
            { ' ',' ',' ',' ','b',' ',' ',' ' },// 2
            { ' ',' ',' ',' ',' ',' ',' ',' ' },// 3
            { ' ',' ',' ',' ',' ','b',' ',' ' },// 4
            { ' ',' ',' ',' ',' ',' ',' ',' ' },// 5
            { ' ',' ',' ',' ',' ',' ',' ',' ' },// 6
            { ' ',' ',' ',' ',' ',' ',' ',' ' } // 7
        };

        mapdata = new MapData(startMap);


        playerPosition = mapdata.SearchPlayer();
        playerDirection = mapdata.EntityDataChack((int)playerPosition.x, (int)playerPosition.y);


        outPutManager.setPar(firstStagePar);

        worldStartPos = player.transform.position; // (ほぼ0,0,0)
        arrayStartPos = mapdata.SearchPlayer();    // (4, 1)
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving && isPlaying)
        {
            Move();

            //Debug.Log(GetCompletedBlocks());
        }
        
        if (isUndo && isPlaying)
        {
            Undo();
            undoButtonAlp(mapdata.moves == 0);
            isUndo = false;
        }

        if (isRestart)
        {
            Restart();
            undoButtonAlp(mapdata.moves == 0);
            isRestart = false;
        }

        Pause();

        if (GetCompletedBlocks() == 2 && !isMoving)
        {
            if (0 <  firstStagePar+(firstStagePar - mapdata.moves)) score += firstStagePar + firstStagePar - mapdata.moves;
            //Debug.Log(firstStagePar + firstStagePar - mapdata.moves);
            SceneManager.LoadSceneAsync("TitleScene");
        }


        //Debug.Log(mapdata.maps[1].entityMap[4,1]);
        //Debug.Log(mapdata.maps[mapdata.maps.Count-1].entityMap[4,1]);
        //Debug.Log(mapdata.maps[mapdata.maps.Count-1].entityMap);
        //Debug.Log(mapdata.maps[mapdata.maps.Count-1]);

        /*
        if (Input.GetKeyDown(KeyCode.F1)) num = 1;
        if (Input.GetKeyDown(KeyCode.F2)) num = 2;
        if (Input.GetKeyDown(KeyCode.F3)) num = 3;
        if (Input.GetKeyDown(KeyCode.F4)) num = 4;
        if (Input.GetKeyDown(KeyCode.F5)) num = 5;
        if (Input.GetKeyDown(KeyCode.F6)) num = 6;
        if (Input.GetKeyDown(KeyCode.F7)) num = 7;
        if (Input.GetKeyDown(KeyCode.F8)) num = 8;

        Debug.Log(num);

        /*
        string log = "";
        for (int i = 0; i < 8; i++) // 行
        {
            for (int j = 0; j < 8; j++) // 列
            {
                log += mapdata.maps[mapdata.maps.Count-1].entityMap[i, j] + ", ";
            }
            log += "\n"; // 改行
        }
        Debug.Log(log);
        */

        //Debug.Log(mapdata.moves);
    }

    private void Move()
    {
        Vector2 targetPosition;
        bool isKey = false;
        char targetTileData = ' ';
        char targetEntityData = ' ';
        char direction = ' ';

        

        // ↓移動方向の座標を保存(確認した方向も保存)
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetPosition = new Vector2((int)(playerPosition.x) + (int)(directions.left), (int)(playerPosition.y));
            direction = (char)symbolDirections.left;
            isKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetPosition = new Vector2((int)(playerPosition.x) + (int)(directions.right), (int)(playerPosition.y));
            direction = (char)symbolDirections.right;
            isKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            targetPosition = new Vector2((int)(playerPosition.x), (int)(playerPosition.y) + (int)(directions.up));
            direction = (char)symbolDirections.up;
            isKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            targetPosition = new Vector2((int)(playerPosition.x), (int)(playerPosition.y) + (int)(directions.down));
            direction = (char)symbolDirections.down;
            isKey = true;
        }
        else
        {
            isKey = false;
            targetPosition = Vector2.zero;
        }

        // ↓保存した座標に何があるかを確認
        if (isKey)
        {
            PlayerMoveAngleAnimation(direction, player);
            targetTileData = mapdata.TileDataChack((int)targetPosition.x, (int)targetPosition.y);
            targetEntityData = mapdata.EntityDataChack((int)targetPosition.x, (int)targetPosition.y);
        }

        // ↓保存したデータから移動できるかを確認。もしできれば実行
        //   実行するとき、最新のマップをコピー作成し、変更点を変更ののち適用させる
        if ((targetTileData == 'g' || targetTileData == 'x') && isKey)
        {
            if (targetEntityData == ' ')
            {
                Map nextMap = mapdata.GetLatestMap();
                nextMap = PlayerMove(nextMap, direction, playerPosition, targetPosition);
                mapdata.AddNextData(nextMap);
                playerPosition = mapdata.SearchPlayer();// 移動後の座標に修正

                PlayerMoveAngleAnimation(direction,player);

                StartCoroutine(PlayerMoveSequence(direction));

            }
            else if (targetEntityData == 'b')
            {
                // ↓更に1マス先(blockの先)の確認
                Vector2 blockTargetPosition;
                if (direction == (char)symbolDirections.left) { blockTargetPosition = new Vector2((int)(playerPosition.x) + (int)directions.left * 2, (int)(playerPosition.y)); }
                else if (direction == (char)symbolDirections.right) { blockTargetPosition = new Vector2((int)(playerPosition.x) + (int)directions.right * 2, (int)(playerPosition.y)); }
                else if (direction == (char)symbolDirections.up) { blockTargetPosition = new Vector2((int)(playerPosition.x), (int)(playerPosition.y) + (int)directions.up * 2); }
                else if (direction == (char)symbolDirections.down) { blockTargetPosition = new Vector2((int)(playerPosition.x), (int)(playerPosition.y) + (int)directions.down * 2); }
                else
                {
                    blockTargetPosition = new Vector2((int)(playerPosition.x), (int)(playerPosition.y));
                    Debug.Log("error//方向を見失っている");
                }

                // ↓デバッグ用 blockがあったときにblockの先に何があるかを出力
                //if (mapdata.TileDataChack((int)blockTargetPosition.x, (int)blockTargetPosition.y) == 'g') Debug.Log("aaaa");
                //Debug.Log(mapdata.EntityDataChack((int)blockTargetPosition.x, (int)blockTargetPosition.y));

                // ↓blockの先にtileは' '又は'x'且つentityは' 'の状態で実行
                if (
                (mapdata.TileDataChack((int)blockTargetPosition.x, (int)blockTargetPosition.y) == 'g' || mapdata.TileDataChack((int)blockTargetPosition.x, (int)blockTargetPosition.y) == 'x')
                && mapdata.EntityDataChack((int)blockTargetPosition.x, (int)blockTargetPosition.y) == ' '
                )
                {

                    Map nextMap = mapdata.GetLatestMap();

                    // ↓blockの移動処理
                    nextMap = BlockMove(nextMap, targetPosition, blockTargetPosition);

                    // ↓プレイヤの移動処理
                    nextMap = PlayerMove(nextMap, direction, playerPosition, targetPosition);

                    

                    // ↓適用
                    mapdata.AddNextData(nextMap);

                    // ↓移動後の座標に修正
                    playerPosition = mapdata.SearchPlayer();

                    /*
                     ↓処理のイメージ

                    処理前
                    npbnn

                    blockの移動後
                    npnbn

                    プレイヤの移動後
                    nnpbn


                    となるためプレイヤの移動を先にするとblockの処理でプレイヤのデータが上書きされてしまうので注意
                    */


                    // 判定用の「プレイヤーの今の世界座標」を取得
                    Vector2 currentInvPos = player.transform.position;

                    // プレイヤーの「今の見た目の座標」に「移動量」を足した場所が、ブロックがいるはずの場所
                    Vector3 expectedBlockPos = player.transform.position;

                    // 方向に応じて「ブロックがいるはずの場所」を計算
                    if (direction == (char)symbolDirections.left) expectedBlockPos.x += (float)directions.left;
                    if (direction == (char)symbolDirections.right) expectedBlockPos.x += (float)directions.right;
                    if (direction == (char)symbolDirections.up) expectedBlockPos.y -= (float)directions.up;
                    if (direction == (char)symbolDirections.down) expectedBlockPos.y -= (float)directions.down;

                    // その場所にいるブロックを特定する（距離が近ければOKとする）
                    GameObject targetBlockObj = null;
                    if (Vector2.Distance(block_a.transform.position, expectedBlockPos) < 0.1f) targetBlockObj = block_a;
                    else if (Vector2.Distance(block_b.transform.position, expectedBlockPos) < 0.1f) targetBlockObj = block_b;

                    if (targetBlockObj != null)
                    {
                        // 見つかったら、そのブロックに対してアニメーションを実行
                        StartCoroutine(BlockMoveSequence(direction, targetBlockObj, (int)blockTargetPosition.x, (int)blockTargetPosition.y));
                    }




                    PlayerMoveAngleAnimation(direction, player);

                    StartCoroutine(PlayerMoveSequence(direction));

                }
            }
        }
        outPutManager.setMove(mapdata.moves);
        undoButtonAlp(mapdata.moves == 0);
    }

    private Map PlayerMove(Map map_, char direction_, Vector2 playerPosition_, Vector2 targetPosition_)
    {
        map_.entityMap[(int)playerPosition_.y,(int)playerPosition_.x] = ' ';
        map_.entityMap[(int)targetPosition_.y,(int)targetPosition_.x] = direction_;
        return map_;
    }
    private Map BlockMove(Map map_, Vector2 myPosition, Vector2 targetPosition)
    {
        map_.entityMap[(int)myPosition.y, (int)myPosition.x] = ' ';
        map_.entityMap[(int)targetPosition.y, (int)targetPosition.x] = 'b';
        return map_;
    }


    private void PlayerMoveAngleAnimation(char direction_,GameObject player_)
    {
        Vector3 rotation = new Vector3(0,0,0);
        if (direction_ == (char)symbolDirections.left) rotation.z = (float)directionAngle.left; 
        if (direction_ == (char)symbolDirections.right) rotation.z = (float)directionAngle.right;
        if (direction_ == (char)symbolDirections.up) rotation.x = (float)directionAngle.up;
        if (direction_ == (char)symbolDirections.down) rotation.x = (float)directionAngle.down;

        player_.transform.eulerAngles = rotation;
    }

    private IEnumerator PlayerMoveSequence(char direction)
    {
        isMoving = true;
        yield return StartCoroutine(playerMover.MoveToPositionCoroutine(direction, 0.1f));
        isMoving = false;
    }

    private IEnumerator BlockMoveSequence(char direction, GameObject targetBlockObj_,int x_, int y_)
    {
        Debug.Log(y_);
        yield return StartCoroutine(targetBlockObj_.GetComponent<MoveToPosition>().MoveToPositionCoroutine(direction, 0.1f));
        targetBlockObj_.GetComponent<Block>().SetSprite(mapdata.TileDataChack(x_, y_) == 'x' && mapdata.EntityDataChack(x_, y_) == 'b');
    }
   


    private int GetCompletedBlocks()
    {
        int conpletedCount = 0;

        for(int y = 0; y < Const.CONST.MAPSIZE; ++y)
        {
            for (int x = 0; x < Const.CONST.MAPSIZE; ++x)
            {
                if (mapdata.TileDataChack(x,y) == 'x' && mapdata.EntityDataChack(x,y) == 'b')
                {
                    ++conpletedCount;
                }
            }
        }
        return conpletedCount;
    }

    private void Undo()
    {
        mapdata.Undo();
        playerPosition = mapdata.SearchPlayer();

        // 悪い例：player.transform.position = playerPosition; 
        // ※Vector2を代入するとZが0になる

        // 良い例：現在のZを維持する
        player.transform.position = new Vector3(
            playerPosition.x * Const.CONST.MOVEDISTANCE,
            -playerPosition.y * Const.CONST.MOVEDISTANCE, // グリッドの向きに合わせる
            player.transform.position.z // 元々インスペクターで設定した-1などの値を維持
        );
        ReflectDataToWorld();

        playerDirection = mapdata.EntityDataChack((int)playerPosition.x, (int)playerPosition.y);

        PlayerMoveAngleAnimation(playerDirection, player);

        // ... UI更新など ...
    }

    private void ReflectDataToWorld()
    {
        if (!isInitialized) return;

        Map currentMap = mapdata.GetLatestMap();

        // --- 1. プレイヤーの再配置 ---
        Vector2 currentArrayPos = mapdata.SearchPlayer();

        // 【魔法の計算式】
        // (現在の配列位置 - 最初の配列位置) = 「何マス動いたか」
        // それに距離を掛けて、初期のワールド座標に足す
        float newX = worldStartPos.x + (currentArrayPos.x - arrayStartPos.x) * Const.CONST.MOVEDISTANCE;
        float newY = worldStartPos.y - (currentArrayPos.y - arrayStartPos.y) * Const.CONST.MOVEDISTANCE;

        player.transform.position = new Vector3(newX, newY, player.transform.position.z);

        // --- 2. ブロックの再配置 ---
        // ブロックも「プレイヤーの初期位置」を基準に計算するとズレません
        int blockCount = 0;
        for (int y = 0; y < Const.CONST.MAPSIZE; y++)
        {
            for (int x = 0; x < Const.CONST.MAPSIZE; x++)
            {
                if (currentMap.entityMap[y, x] == 'b')
                {
                    GameObject targetBlock = (blockCount == 0) ? block_a : block_b;

                    float bX = worldStartPos.x + (x - arrayStartPos.x) * Const.CONST.MOVEDISTANCE;
                    float bY = worldStartPos.y - (y - arrayStartPos.y) * Const.CONST.MOVEDISTANCE;

                    targetBlock.transform.position = new Vector3(bX, bY, targetBlock.transform.position.z);
                    blockCount++;
                }
            }
        }
    }

    private void undoButtonAlp(bool tf_)
    {
        if (tf_)
        {
            cantUndoButton.gameObject.SetActive(true);
        }
        else
        {
            cantUndoButton.gameObject.SetActive(false);
        }
    }


    private void Restart()
    {
        while(mapdata.moves > 0)
        {
            Undo();
        }   
    }

    private void Pause()
    {
        if (isPause)
        {
            pauseObject.SetActive(true);
            isPlaying = false;
        }
        else
        {
            pauseObject.SetActive(false);
            isPlaying = true;
        }
    }
}




