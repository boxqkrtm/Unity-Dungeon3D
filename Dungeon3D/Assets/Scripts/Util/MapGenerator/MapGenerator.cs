using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject floorTile;
    public GameObject wallTile;
    public GameObject stair;
    public GameObject startDoor;
    public GameObject chest;
    public GameObject torchWall;
    public Image loadBar;
    public Text loadText;
    public GameObject loadingUI;
    public List<GameObject> mobList = new List<GameObject>();
    private int[,] map;
    int maxMapSize = 500;
    public int floor = 1;
    void Start()
    {
        map = new int[maxMapSize, maxMapSize];
        GenerateFloor();
    }

    public void DebugForceRegen()
    {
        GenerateFloor();
    }
    public void GenerateFloor()
    {
        StartCoroutine(SlowMapGen());
    }

    IEnumerator SlowMapGen()
    {
        loadingUI.SetActive(true);
        GameObject.Find("Fade").GetComponent<Fade>().FadeOut();
        var floorSize = 2; //바닥 타일 하나 크기가 2임
        GameManager.Instance.PlayerObj.GetComponent<Rigidbody>().isKinematic = true;
        //Random.InitState(1);
        /*
            0,0에 방을 생성함
            겹치지 않게 방을 사방으로 퍼뜨림
            길연결 벽생성 후 완성
        */
        var nowFloor = floor;
        ClearFloor();
        int mapCount = 3 + nowFloor;
        mapCount = mapCount > 10 ? 10 : mapCount;
        var rooms = new List<Room>();

        //랜덤한 크기의 방 생성
        for (int i = 0; i < mapCount; i++)
        {
            var room = new Room();
            int center = maxMapSize / 2;
            room.coord = new Vector2Int(center, center);
            room.size = new Vector2Int(Random.Range(4, 15), Random.Range(4, 15));
            rooms.Add(room);
        }

        //모든방이 겹치지 않게 퍼뜨림
        foreach (var room in rooms)
        {
            int[,] tempMap = new int[maxMapSize, maxMapSize];
            foreach (var r in rooms)
                r.DrawTo(ref tempMap);
            while (room.IsIntersect(tempMap,
                    new Vector2Int(maxMapSize, maxMapSize),
                    new Vector2Int(0, 0),
                    3))
                room.RandomMove(
                    new Vector2Int(maxMapSize, maxMapSize),
                    new Vector2Int(0, 0));
        }

        //완성 된 방 전부 그리기
        foreach (var room in rooms)
            room.DrawTo(ref map);

        //방 연결하기
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            var a = i; 
            var b = i + 1;
            if (rooms[a].isConnected == false || rooms[b].isConnected == false)
                ConnectRoom(rooms[a], rooms[b], rooms);
        }

        var renderFrameSkip = 2;
        var frame = 0;

        //방 렌더링하기
        var navList = new List<NavMeshSurface>();
        for (int y = 0; y < maxMapSize; y++)
        {
            bool hasTile = false;
            for (int x = 0; x < maxMapSize; x++)
            {
                if (map[y, x] == 1)
                {
                    hasTile = true;
                    var abc = Instantiate(floorTile);
                    abc.transform.position = new Vector3(x * 2, 0, y * 2);
                    abc.transform.localScale = Vector3.one;
                    abc.transform.rotation = Quaternion.Euler(Vector3.zero);
                    navList.Add(abc.GetComponent<NavMeshSurface>());
                }
            }
            if (frame++ % renderFrameSkip == 0 && hasTile == true)
            {
                yield return null;
                var progress = (float)y / maxMapSize / 2;
                loadText.text = "Dungeon Loading... " + Mathf.FloorToInt(progress * 100 + 1).ToString() + "%";
                loadBar.fillAmount = progress;
            }
        }
        var xdelta = new int[4] { 0, -1, 0, +1 };
        var ydelta = new int[4] { -1, 0, +1, 0 };
        var wallCnt = 0;
        for (int y = 0; y < maxMapSize; y++)
        {
            bool hasTile = false;
            for (int x = 0; x < maxMapSize; x++)
            {
                //타일 상하좌우로 비어있는 영역의 방향 탐색 후 벽 설치
                if (map[y, x] == 1)
                {
                    hasTile = true;
                    for (var i = 0; i < 4; i++)
                    {
                        if (map[y + ydelta[i], x + xdelta[i]] == 0)
                        {
                            GameObject abc;
                            if(wallCnt++ % 10 == 0)
                                abc = Instantiate(torchWall);
                            else
                                abc = Instantiate(wallTile);
                            abc.transform.position = new Vector3(x * 2, 0, y * 2);
                            abc.transform.localScale = Vector3.one * 100;
                            abc.transform.rotation = Quaternion.Euler(-90f, 0, 0);
                            switch (i)
                            {
                                //하
                                case 0:
                                    abc.transform.position = new Vector3(x * 2, 0, y * 2 - 1);
                                    abc.transform.rotation = Quaternion.Euler(-90f, -180f, 0);
                                    break;
                                //좌
                                case 1:
                                    abc.transform.position = new Vector3(x * 2 - 1, 0, y * 2);
                                    abc.transform.rotation = Quaternion.Euler(-90f, -90f, 0);
                                    break;
                                //상 //3D기준 앞
                                case 2:
                                    abc.transform.position = new Vector3(x * 2, 0, y * 2 + 1);
                                    break;
                                //우
                                case 3:
                                    abc.transform.position = new Vector3(x * 2 + 1, 0, y * 2);
                                    abc.transform.rotation = Quaternion.Euler(-90f, 90f, 0);
                                    break;
                            }
                        }
                    }
                }
            }
            if (frame++ % renderFrameSkip == 0 && hasTile == true)
            {
                yield return null;
                var progress = 0.5f + (float)y / maxMapSize / 2;
                loadText.text = "Dungeon Loading... " + Mathf.FloorToInt(progress * 100 + 1).ToString() + "%";
                loadBar.fillAmount = progress;
            }
        }
        for (var i = 0; i < navList.Count; i++)
        {
            //yield return null;
            var progress = (float)(i + 1) / navList.Count;
            //loadText.text = "Loading AI... " + Mathf.FloorToInt(progress * 100).ToString() + "%";
            //loadBar.fillAmount = progress;
            navList[i].BuildNavMesh();
            break;//모든 바닥이 연결되어있어서 하나만 빌드하면 됨
        }
        loadingUI.SetActive(false);

        //스폰 룸 지정 및 플레이어 좌표 설정
        var startRoomIndex = Random.Range(0, rooms.Count);
        GameManager.Instance.PlayerObj.transform.position = new Vector3(rooms[startRoomIndex].Center.x * floorSize, 0, rooms[startRoomIndex].Center.y * floorSize);
        var e = Instantiate(startDoor);
        e.transform.position = new Vector3(rooms[startRoomIndex].Center.x * floorSize, 0, rooms[startRoomIndex].Center.y * floorSize - 1 * floorSize);


        //플레이어 활성화
        GameManager.Instance.PlayerObj.GetComponent<Rigidbody>().isKinematic = false;

        var itemAndMobCount = Mathf.RoundToInt((float)mapCount * 2 / 3);
        //몹 설치하기
        var roomIndexList = new List<int>();
        for (var i = 0; i < rooms.Count; i++)
        {
            roomIndexList.Add(i);
        }
        roomIndexList.RemoveAt(startRoomIndex);
        //shuffle
        for (var i = 0; i < roomIndexList.Count; i++)
        {
            var temp = roomIndexList[i];
            var rand1 = Random.Range(0, roomIndexList.Count);
            roomIndexList[i] = roomIndexList[rand1];
            roomIndexList[rand1] = temp;
        }
        //set
        for (int i = 0; i < roomIndexList.Count; i++)
        {
            var nowRoomsCenter = rooms[roomIndexList[i]].Center;
            for (var j = 0; j < Random.Range(1,3); j++)
            {
                var mob = Instantiate(mobList[Random.Range(0, mobList.Count)]);
                mob.transform.position = rooms[roomIndexList[i]].RandomInnerPos() * floorSize;
                if (mob.GetComponent<NavMeshAgent>() != null)
                {
                    mob.GetComponent<NavMeshAgent>().enabled = true;
                }
                var mobS = mob.GetComponent<Monster>();
                var dungeonLv = floor +3;
                mobS.ud.MaxHp = Mathf.RoundToInt((float)mobS.ud.MaxHp/mobS.ud.Lv*dungeonLv);
                mobS.ud.MaxMp = Mathf.RoundToInt((float)mobS.ud.MaxMp/mobS.ud.Lv*dungeonLv);
                mobS.ud.Gold = Mathf.RoundToInt((float)mobS.ud.Gold/mobS.ud.Lv*dungeonLv);
                mobS.ud.Atk = Mathf.RoundToInt((float)mobS.ud.Atk/mobS.ud.Lv*dungeonLv);
                mobS.ud.Def = Mathf.RoundToInt((float)mobS.ud.Def/mobS.ud.Lv*dungeonLv);
                mobS.ud.Exp = Mathf.RoundToInt((float)mobS.ud.Exp/mobS.ud.Lv*dungeonLv);
                mobS.ud.HpRatio = 1f;
                mobS.ud.MpRatio = 1f;
                mobS.ud.Lv = dungeonLv;

            }
        }
        //아이템 드랍상자설치
        for (int i = 0; i < itemAndMobCount - 1; i++)
        {
            var nowRoomsCenter = rooms[roomIndexList[i]].Center;
            var itembox = Instantiate(chest);
            var items = new List<Item>();
            
            for(var j = 0; j < Random.Range(1,5);j++)
                items.Add(ItemManager.Instance.CodeToItem(Random.Range(4, 10)));

            itembox.GetComponent<DungeonChest>().Items = items;
            itembox.transform.position =  rooms[roomIndexList[i]].RandomInnerPos() * floorSize;
        }
        //계단 설치하기 플레이어 스폰지점 제외
        var rand = Random.Range(0, rooms.Count);
        while (startRoomIndex == rand) rand = Random.Range(0, rooms.Count);
        var nowRoomsCenter2 = rooms[rand].Center;
        var stairObj = Instantiate(stair);
        stairObj.transform.position = new Vector3(nowRoomsCenter2.x * floorSize, 0.5f, nowRoomsCenter2.y * floorSize);

        GameObject.Find("Fade").GetComponent<Fade>().FadeIn();
        AlertManager.Instance.AddGameLog("알수없는 던전 " + floor.ToString() + "F");
    }

    //해당 좌표를 랜덤으로 한칸 상하좌우중 아무대나 움직여서 반환하는 함수
    void ConnectRoom(Room a, Room b, List<Room> rooms, int width = 2)
    {
        //가로축
        int x = a.Center.x;
        int y = a.Center.y;
        for (; x != b.Center.x;)
        {
            if (x > b.Center.x)
                x--;
            else x++;
            //길 생성 width만큼 좌우에 추가 길 생성 도중 다른 방에 닿으면 연결 설정
            for (var i = 0; i < width; i++)
            {
                map[y + i, x] = 1;
                map[y - i, x] = 1;
                foreach (var room in rooms)
                    if (room.IsInner(x + i, y) == true || room.IsInner(x - i, y) == true)
                        room.isConnected = true;
            }
        }
        //세로축
        for (; y != b.Center.y;)
        {
            if (y > b.Center.y)
                y--;
            else y++;
            for (var i = 0; i < width; i++)
            {
                map[y, x + i] = 1;
                map[y, x - i] = 1;
                foreach (var room in rooms)
                    if (room.IsInner(x, y - i) == true || room.IsInner(x, y + i) == true)
                        room.isConnected = true;
            }
        }
    }
    void ClearFloor()
    {
        var delList = new List<GameObject>();
        var delTag = new string[] {
             "FloorTile", "WallTile", "StairTile", "Monster",
             "DroppedItem"
            };

        foreach (var i in delTag)
            foreach (var j in GameObject.FindGameObjectsWithTag(i))
                delList.Add(j);

        foreach (var i in delList) Destroy(i);
        for (int i = 0; i < maxMapSize; i++)
            for (int j = 0; j < maxMapSize; j++)
                map[i, j] = 0;
    }
}
