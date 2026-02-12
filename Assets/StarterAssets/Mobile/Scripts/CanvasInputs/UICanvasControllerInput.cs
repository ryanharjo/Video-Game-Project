using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [Header("Player Reference")]
        public PlayerRunner player; // Moved inside the class

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            player.MoveInput(virtualMoveDirection);
        }

        // Changed from Vector2 to bool to match your PlayerRunner script
        public void VirtualSlideInput(bool virtualSlideState)
        {
            player.SlideInput(virtualSlideState);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            player.JumpInput(virtualJumpState);
        }

        public void VirtualFlipInput(bool virtualFlipState)
        {
            player.FlipInput(virtualFlipState);
        }
    }

}
