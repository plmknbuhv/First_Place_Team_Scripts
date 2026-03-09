using System;
using Code.Entities;
using Code.Farms;
using UnityEngine;

namespace Code.Players
{
    public class PotChecker : MonoBehaviour, IEntityComponent
    {
        private Player _player;
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }

        private void Update()
        {

        }
    }
}