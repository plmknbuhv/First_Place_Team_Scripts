using UnityEngine;

namespace Code.ETC
{
    public class SortingRenderer : MonoBehaviour
    {
        [SerializeField] private int offset;
        
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void LateUpdate()
        {
            int sortingOrder = (int)(transform.position.y * 100) + offset;
            _renderer.sortingOrder = -sortingOrder;
        }
    }
}