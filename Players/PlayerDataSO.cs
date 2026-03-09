using Code.Core.EventSystem;
using Code.Events;
using Code.Managers;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Players
{
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "SO/Player/PlayerData", order = 0)]
    public class PlayerDataSO : ScriptableObject
    {
        public float moveSpeed;
        public int maxSeedCount;

        private void OnEnable()
        {
            moveSpeed = 3.5f;
            maxSeedCount = 3;
        }
    }
}