using Const;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;


class MapData
{
    public List<Map> maps = new List<Map>();
    public int moves;

    public MapData(Map startMap_)
    {
        
        maps.Add(startMap_);
        moves = 0;
    }
    public char TileDataChack(int x_, int y_)
    {
        return maps[moves].tileMap[y_, x_];
    }
    public char EntityDataChack(int x_, int y_)
    {
        return maps[moves].entityMap[y_, x_];
    }

    public Map GetLatestMap()
    {
        Map current = maps[moves];

        Map copy = new Map();

        copy.tileMap = (char[,])current.tileMap.Clone();
        copy.entityMap = (char[,])current.entityMap.Clone();

        return copy;
    }

    public void AddNextData(Map nextMap_)
    {
        
        maps.Add(nextMap_);
        ++moves;
    }

    public void Undo()
    {
        if (moves > 0)
        {
            // 現在の最新（moves番目）を削除する
            maps.RemoveAt(moves);
            // インデックスを一つ前に戻す
            --moves;
        }
    }

    public Vector2 SearchPlayer()
    {
        for (int y = 0; y < Const.CONST.MAPSIZE; ++y)
        {
            for (int x = 0; x < Const.CONST.MAPSIZE; ++x)
            {
                if (EntityDataChack(x, y) == (char)symbolDirections.left || EntityDataChack(x, y) == (char)symbolDirections.right || EntityDataChack(x, y) == (char)symbolDirections.up || EntityDataChack(x, y) == (char)symbolDirections.down)
                {
                    return new Vector2(x, y);
                }
            }
        }

        Debug.Log("error//プレイヤが見つかりませんでした");
        return new Vector2(0, 0);
    }
    
    
}

class Map
{
    public char[,] tileMap = new char[Const.CONST.MAPSIZE, Const.CONST.MAPSIZE]
    {
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' }
    };

    public char[,] entityMap = new char[Const.CONST.MAPSIZE, Const.CONST.MAPSIZE]
    {
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' },
        { ' ',' ',' ',' ',' ',' ',' ',' ' }
    };
}

