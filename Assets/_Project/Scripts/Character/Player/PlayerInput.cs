using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    public float moveX { get; private set; }
    public bool jumpPressed { get; private set; }   // 버퍼링 적용된 점프 입력
    public bool jumpHeld { get; private set; }
    public bool dashPressed { get; private set; }
    public bool interactPressed { get; private set; }
    public bool attackPressed { get; private set; }
    public bool attackHeld { get; private set; }
    public bool absorbPressed { get; private set; }
    public bool statInfoPressed { get; private set; }

    [Header("입력 버퍼링 — 상세기획서 '조작 & 프레임 데이터' 기준")]
    [SerializeField] 
    private float jumpBufferTime = 0.15f; // 9f @ 60fps
    private float jumpBufferTimer;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction interactAction;
    private InputAction attackAction;
    private InputAction absorbAction;
    private InputAction statInfoAction;

    private void Awake()
    {
        // 이동 방향키
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/leftArrow")
            .With("Positive", "<Keyboard>/rightArrow");
        moveAction.AddBinding("Gamepad/leftStick/x"); // pc 패드 모바일 공용으로

        // 점프
        jumpAction = new InputAction("Jump", InputActionType.Button);
        jumpAction.AddBinding("<Keyboard>/space");
        jumpAction.AddBinding("<Gamepad>/buttonSouth");

        // 대쉬
        dashAction = new InputAction("Dash", InputActionType.Button);
        dashAction.AddBinding("<Keyboard>/leftShift");
        dashAction.AddBinding("<Gamepad>/buttonWest");

        // 상호작용
        interactAction = new InputAction("Interact", InputActionType.Button);
        interactAction.AddBinding("<Keyboard>/f");
        interactAction.AddBinding("<Gamepad>/buttonNorth");

        // 공격
        attackAction = new InputAction("Attack", InputActionType.Button);
        attackAction.AddBinding("<Keyboard>/q");
        attackAction.AddBinding("<Gamepad>/buttonWest");

        // 금 섭취
        absorbAction = new InputAction("Absorb", InputActionType.Button);
        absorbAction.AddBinding("<Keyboard>/r");
        absorbAction.AddBinding("<Gamepad>/buttonEast");

        // 스탯창
        statInfoAction = new InputAction("StatInfo", InputActionType.Button);
        statInfoAction.AddBinding("<Keyboard>/i");
        //absorbAction.AddBinding("<Gamepad>/buttonEast");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        dashAction.Enable();
        interactAction.Enable();
        attackAction.Enable();
        absorbAction.Enable();
        statInfoAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        dashAction.Disable();
        interactAction.Disable();
        attackAction.Disable();
        absorbAction.Disable();
        statInfoAction.Disable();
    }

    private void OnDestroy()
    {
        moveAction.Dispose();
        jumpAction.Dispose();
        dashAction.Dispose();
        interactAction.Dispose();
        attackAction.Dispose();
        absorbAction.Dispose();
        statInfoAction.Dispose();
    }

    private void Update()
    {
        moveX = moveAction.ReadValue<float>();
        jumpHeld = jumpAction.IsPressed();
        dashPressed = dashAction.WasPressedThisFrame();
        interactPressed = interactAction.WasPressedThisFrame();
        attackPressed = attackAction.WasPressedThisFrame();
        attackHeld = attackAction.IsPressed();
        absorbPressed = absorbAction.WasPressedThisFrame();
        statInfoPressed = statInfoAction.WasPressedThisFrame();

        // 점프: 버퍼링 대상 착지 직전에 눌러도 버퍼 시간내면 점프되게 
        if (jumpAction.WasPressedThisFrame())        
            jumpBufferTimer = jumpBufferTime;        

        jumpBufferTimer -= Time.deltaTime;
        jumpPressed = jumpBufferTimer > 0f;
        
        // 대쉬 패링은 선입력 방지 버퍼링 대상에서 제외

        if(statInfoPressed)
            UIManager.instance.ToggleStatPanel();
    }

    public void ConsumeJump()
    {
        jumpBufferTimer = 0f;
    }

    public void ConsumeAttackPressed()
    {
        attackPressed = false;
    }

    public void ResetInput()
    {
        moveX = 0f;

        jumpPressed = false;
        interactPressed = false;
        absorbPressed = false;
    }
}
