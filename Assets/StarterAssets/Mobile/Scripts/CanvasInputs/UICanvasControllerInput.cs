using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [Header("Output")]
        public PlayerRunner playerRunner;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            if (playerRunner != null)
            {
                playerRunner.MoveInput(virtualMoveDirection);
            }
        }
    }
}
