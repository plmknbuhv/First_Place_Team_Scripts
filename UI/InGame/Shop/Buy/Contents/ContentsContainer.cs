using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    public class ContentsContainer : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup layoutGroup;

        private RectTransform rectTransform;
        private const float CHILD_SIZE = 140f;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetContainerSize(int childCount)
        {
            if (childCount <= 0)
                return;

            float width =
                layoutGroup.padding.left +
                layoutGroup.padding.right +
                (CHILD_SIZE * childCount) +
                (layoutGroup.spacing * (childCount - 1));

            rectTransform.sizeDelta = new Vector2(
                width,
                rectTransform.sizeDelta.y
            );

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
