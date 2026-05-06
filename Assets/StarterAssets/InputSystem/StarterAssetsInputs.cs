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
            // Hareket tetiklendiyse auto-run mantýðýný çalýþtýr
            if (isAutoRunning && isMovementStarted)
            {
                // KURAL: Karakter artýk kameradan baðýmsýz olduðu için DAÝMA ileri (+1) koþmalý.
                float forwardMove = 1f;

                // X eksenini kurala göre yönet
                float horizontalMove = canControlCharacterHorizontal ? rawInput.x : 0f;

                // EÐER KAMERA TERSSE: Oyuncu A'ya (ekranda sol) bastýðýnda, 
                // dünyanýn +X'ine (ekranda sað gibi algýlanýr) gitmesi için X girdisini tersine çevir.
                if (isCameraInverted)
                {
                    horizontalMove *= -1f;
                }

                move = new Vector2(horizontalMove, forwardMove);
            }
            else if (isAutoRunning && !isMovementStarted)
            {
                // Hareket henüz baþlamadýysa karakteri Idle'da beklet
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