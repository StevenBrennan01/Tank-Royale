using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "TankData")]
public class TankData : ScriptableObject
{
    [System.Serializable]
    public struct TankMovementSettings
    {
        public float tankMoveSpeed;
        public float hullRotateSpeed; 
        public float turretRotateSpeed; 
        public float towerRotateSpeed;
    }
    [SerializeField] private TankMovementSettings movementSettings;
}