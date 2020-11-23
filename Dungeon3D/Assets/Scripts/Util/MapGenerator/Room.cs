using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public bool isConnected = false;
    public bool isItemRoom;
    public Vector2Int coord; //왼쪽 위 모서리의 좌표
    public Vector2Int size;//벽 제외
    public Vector2Int Center
    {
        get
        {
            return new Vector2Int(
                coord.x + (size.x) / 2,
                coord.y + (size.y) / 2
            );
        }
    }
    public int RoomSize { get => size.x * size.y; }
    //점이 이 방 안에 있는지 확인
    public bool IsInner(int x, int y)
    {
        if ((x >= coord.x) && (x < coord.x + size.x) && (y >= coord.y) && (y < coord.y + size.y))
            return true;
        else return false;
    }
    public void DrawTo(ref int[,] target)
    {
        for (var y = coord.y; y < (coord.y + size.y); y++)
        {
            for (var x = coord.x; x < (coord.x + size.x); x++)
            {
                target[y, x] = 1;
            }
        }
    }
    //배열과 이 방이 겹치는지 확인
    public bool IsIntersect(int[,] target, Vector2Int maxCoord, Vector2Int minCoord, int margin = 0)
    {
        for (var y = coord.y - margin; y < (coord.y + size.y + margin); y++)
        {
            for (var x = coord.x - margin; x < (coord.x + size.x + margin); x++)
            {
                //배열 초과 예외처리
                if (coord.x >= maxCoord.x || coord.y >= maxCoord.y
                || coord.x < minCoord.x || coord.y < minCoord.y) continue;
                if (target[y, x] != 0) return true;
            }
        }
        return false;
    }

    public void RandomMove(Vector2Int maxCoord, Vector2Int minCoord)
    {
        //상하좌우 대각선
        var delta = new List<Vector2Int>() {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, +1)
        };
        coord += delta[Random.Range(0, delta.Count - 1)];
        if (coord.x >= maxCoord.x) coord.x = maxCoord.x - 1;
        if (coord.y >= maxCoord.y) coord.y = maxCoord.y - 1;
        if (coord.x < minCoord.x) coord.x = minCoord.x;
        if (coord.y < minCoord.y) coord.y = minCoord.y;
    }
    
    bool[,] innerTable;
    bool[,] InnerTable{get{
        if(innerTable == null) 
        {
            innerTable = new bool[size.y-2,size.x-2];
            for (int i = 0; i < size.y-2; i++)
                for (int j = 0; j < size.x-2; j++)
                innerTable[i,j] = false;
        } 
            return innerTable;
        } 
        set{innerTable = value;}
    }

    public Vector3 RandomInnerPos()
    {
        while (true)
        {
            var randy = Random.Range(0, size.y-2);
            var randx = Random.Range(0, size.x-2);
            if(InnerTable[randy, randx] == false)
            {
                InnerTable[randy, randx] = true;
                return new Vector3(coord.x+1 + randx,0, coord.y+1 + randy);
            }
        }
    }
}