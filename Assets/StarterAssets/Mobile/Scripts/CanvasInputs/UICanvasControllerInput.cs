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

        
      

        public void VirtualJumpInput(bool virtualJumpState)
        {
            player.JumpInput(virtualJumpState);
        }

       
    }

}
