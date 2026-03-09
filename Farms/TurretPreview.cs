using UnityEngine;

namespace Code.Farms
{
    public class TurretPreview : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetUpImage(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }
}