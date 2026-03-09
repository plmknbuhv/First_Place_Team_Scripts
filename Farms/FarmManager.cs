using Code.Core;
using UnityEngine;

namespace Code.Farms
{
    public class FarmManager : MonoSingleton<FarmManager>
    {
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private Vector2Int farmSize;
        [SerializeField] private SelectBox selectBoxObj;

        private Cell[,] _cellArray;
        private Grid _grid;
        
        private void Awake()
        {
            _cellArray = new Cell[farmSize.y, farmSize.x];
            
            _grid = GetComponent<Grid>();
        }

        private void Start()
        {
            MakeFarm();
        }

        private void MakeFarm()
        {
            for (int i = 0; i < farmSize.y; i++)
            {
                for (int j = 0; j < farmSize.x; j++)
                {
                    Cell cell = Instantiate(cellPrefab, transform);
                    cell.transform.localScale = _grid.cellSize;
                    cell.transform.localPosition = _grid.CellToLocal(new Vector3Int(j, i)) + _grid.cellSize * 0.5f;
                    
                    _cellArray[i, j] = cell;
                }
            }
        }

        public bool TryGetCell(Vector3 worldPos, out Cell cell)
        {
            cell = null;
            
            Vector3Int cellPos = _grid.WorldToCell(worldPos);
            Vector3 cellWorldPos = _grid.CellToWorld(cellPos);

            Vector3 local = worldPos - cellWorldPos;

            // 밭안에 있는 셀인지
            bool isInFarmCell = 
                cellPos.x < farmSize.x && cellPos.x >= 0 && 
                cellPos.y < farmSize.y && cellPos.y >= 0;

            if (isInFarmCell == false) return false;
            
            // 셀 안에 있는지
            bool isInsideCell =
                local.x >= 0 && local.x <= _grid.cellSize.x &&
                local.y >= 0 && local.y <= _grid.cellSize.y;
            
            if (isInsideCell == false) return false;
            
            cell = _cellArray[cellPos.y, cellPos.x];
            return true;
        }

        public void SetActiveSelectBox(bool isActive, Cell cell = null)
        {
            if (isActive) // 만약 Box를 킬거면
            {
                Debug.Assert(cell != null, "Active 할 Cell이 없음");
                selectBoxObj.transform.position = cell.transform.position;
            }
            
            selectBoxObj.gameObject.SetActive(isActive);
        }
    }
}
