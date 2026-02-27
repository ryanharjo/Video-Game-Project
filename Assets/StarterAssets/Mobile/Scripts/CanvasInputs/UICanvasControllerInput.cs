using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [Header("Player Reference")]
        public PlayerRunner player;

        // 1. Logic for On-Screen Joysticks or D-Pads
        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            if (player != null)
            {
                player.MoveInput(virtualMoveDirection);
            }
        }

        // 2. Logic for On-Screen Jump Button
        public void VirtualJumpInput(bool virtualJumpState)
        {
            if (player != null)
            {
                // We only trigger the jump when the button is first pressed (true)
                player.JumpInput(virtualJumpState);
            }
        }
    }
}
