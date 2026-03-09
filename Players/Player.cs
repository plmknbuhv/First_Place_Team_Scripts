using Code.Entities;
using Input;
using UnityEngine;

namespace Code.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [field: SerializeField] public PlayerDataSO PlayerData { get; private set; }
    }
}