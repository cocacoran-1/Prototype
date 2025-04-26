using UnityEngine;
using UnityEngine.U2D;

namespace Game.Map
{
    public class InfiniteMapManager : MonoBehaviour
    {
        [Header("타일 프리팹 및 크기")]
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Vector2 tileSize = new Vector2(1f, 1f);

        [Header("플레이어")]
        [SerializeField] private Transform player;

        private TileCell[,] tiles;
        private Vector2Int tileCenterIndex;
        private float unitsPerPixel;
        private int tileCountX;
        private int tileCountY;

        private void Start()
        {
            var cam = Camera.main;
            var pixelCam = cam.GetComponent<PixelPerfectCamera>();
            unitsPerPixel = 1f / pixelCam.assetsPPU;

            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;

            tileCountX = Mathf.CeilToInt((halfWidth * 2f) / tileSize.x) + 2;
            tileCountY = Mathf.CeilToInt((halfHeight * 2f) / tileSize.y) + 2;

            tiles = new TileCell[tileCountX, tileCountY];
            tileCenterIndex = WorldToTileIndex(player.position);

            InitializeTiles();
            UpdateTiles();
        }

        private void Update()
        {
            Vector2Int currentIndex = WorldToTileIndex(player.position);
            if (currentIndex != tileCenterIndex)
            {
                tileCenterIndex = currentIndex;
                UpdateTiles();
            }
        }

        private void InitializeTiles()
        {
            Vector2Int arrayCenter = new Vector2Int(tileCountX / 2, tileCountY / 2);

            for (int y = 0; y < tileCountY; y++)
            {
                for (int x = 0; x < tileCountX; x++)
                {
                    GameObject tileObj = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, transform);
                    tiles[x, y] = tileObj.AddComponent<TileCell>();
                }
            }
        }

        private void UpdateTiles()
        {
            Vector2Int arrayCenter = new Vector2Int(tileCountX / 2, tileCountY / 2);

            for (int y = 0; y < tileCountY; y++)
            {
                for (int x = 0; x < tileCountX; x++)
                {
                    Vector2Int offsetFromCenter = new Vector2Int(x, y) - arrayCenter;
                    Vector2Int worldTileIndex = tileCenterIndex + offsetFromCenter;

                    Vector2 worldPos = TileIndexToWorld(worldTileIndex);

                    float snappedX = Mathf.Round(worldPos.x / unitsPerPixel) * unitsPerPixel;
                    float snappedY = Mathf.Round(worldPos.y / unitsPerPixel) * unitsPerPixel;

                    tiles[x, y].UpdateTile(worldTileIndex, new Vector3(snappedX, snappedY, 0));
                }
            }
        }

        private Vector2 TileIndexToWorld(Vector2Int index)
        {
            return new Vector2((index.x + 0.5f) * tileSize.x, (index.y + 0.5f) * tileSize.y);
        }

        private Vector2Int WorldToTileIndex(Vector2 worldPos)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPos.x / tileSize.x),
                Mathf.FloorToInt(worldPos.y / tileSize.y)
            );
        }
    }

    public class TileCell : MonoBehaviour
    {
        public Vector2Int worldIndex;

        public void UpdateTile(Vector2Int newWorldIndex, Vector3 newWorldPosition)
        {
            worldIndex = newWorldIndex;
            transform.position = newWorldPosition;

            // TODO: worldIndex에 따라 스프라이트나 타입 갱신 가능
        }
    }
}
