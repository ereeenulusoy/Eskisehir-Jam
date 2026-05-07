using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [Tooltip("Aktifse karakter otomatik koþmaz. A-D tuþlarý karakteri Z ekseninde ileri-geri yürütür (2.5D Platformer).")]
        public bool isMarioMode = false;

        // --- GAME JAM KURALLARI ---
        [Header("No Rules Settings")]
        [Tooltip("Karakter otomatik olarak hep ileri koþar.")]
        public bool isAutoRunning = true;

        [Tooltip("Þu an aktif olarak oynanan bölümün numarasý")]
        public int currentActiveLevel = 1;

        [Tooltip("Aktifse A-D karakteri hareket ettirir. Kapalýysa karakter düz koþar, A-D engelleri kaydýrýr.")]
        public bool canControlCharacterHorizontal = false;

        [Tooltip("Aktifse Shift tuþu ile hýzý 2'ye düþürüp yürüyebilir (Karakterin bilinç kazanmasý).")]
        public bool canUseShiftToSlowDown = false;

        [Tooltip("Aktifse karakter zýplayabilir.")]
        public bool canJump = false;

        [Tooltip("Kamera 180 derece döndüyse bu kuralý aktif edin. Karakteri ve kontrolleri düzeltir.")]
        public bool isCameraInverted = false;

        [Tooltip("Oyunun baþlayýp baþlamadýðýný kontrol eder")]
        public bool isMovementStarted = false;

        [Tooltip("Engellerin okuyabilmesi için oyuncunun asýl bastýðý yön tuþlarý")]
        public Vector2 rawInput;
        // --------------------------

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            // Oyuncunun klavyeden girdiði saf veriyi kaydet
            rawInput = newMoveDirection;

            // Herhangi bir yön tuþuna (W,A,S,D) basýlýrsa oyunu baþlat
            if (!isMovementStarted && newMoveDirection.sqrMagnitude > 0)
            {
                isMovementStarted = true;
            }

            if (!isAutoRunning)
            {
                move = newMoveDirection;
            }
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            // Space tuþuna basýlýrsa oyunu baþlat
            if (!isMovementStarted && newJumpState)
            {
                isMovementStarted = true;
            }

            // Kural aktifse zýpla, deðilse zýplamayý iptal et
            jump = canJump ? newJumpState : false;
        }

        public void SprintInput(bool newSprintState)
        {
            // Shift tuþuna basýlýrsa oyunu baþlat
            if (!isMovementStarted && newSprintState)
            {
                isMovementStarted = true;
            }

            // Kural aktifse yavaþla/koþ
            sprint = canUseShiftToSlowDown ? newSprintState : false;
        }

        private void Update()
        {
            // 1. KURAL: MARIO MODU (2.5D Parkur)
            if (isMarioMode)
            {
                // Otomatik koþu yok. Sadece A-D (rawInput.x) çalýþýr ve bu Z eksenine (move.y) aktarýlýr.
                float zMove = rawInput.x;

                // Eðer kamera ters yönden bakýyorsa sað-sol hissiyatýný düzeltmek için
                if (isCameraInverted)
                {
                    zMove *= -1f;
                }

                // X ekseninde hareket 0, sadece Z ekseninde (ileri/geri) hareket ediyoruz.
                move = new Vector2(0f, zMove);
            }
            // 2. KURAL: STANDART AUTO-RUN MODU
            else if (isAutoRunning && isMovementStarted)
            {
                float forwardMove = 1f;
                float horizontalMove = canControlCharacterHorizontal ? rawInput.x : 0f;

                if (isCameraInverted)
                {
                    horizontalMove *= -1f;
                }

                move = new Vector2(horizontalMove, forwardMove);
            }
            // 3. KURAL: BEKLEME MODU
            else if (isAutoRunning && !isMovementStarted)
            {
                move = Vector2.zero;
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}