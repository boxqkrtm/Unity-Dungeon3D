// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/PlayerControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControl : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""Play"",
            ""id"": ""d017ca9b-f421-41f3-a37f-1a5c95ed4361"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""82ad5088-939e-42f3-8742-015637092cd0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveScreen"",
                    ""type"": ""Value"",
                    ""id"": ""b15aca85-e873-4878-b3b0-096dd779627b"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""106bb435-dc65-4e65-8db3-37893f0526c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""AttackOff"",
                    ""type"": ""Button"",
                    ""id"": ""9446f8f2-94e2-4d78-ab59-0457420fe234"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""PassThrough"",
                    ""id"": ""54fc33e5-edf7-45eb-b94f-08ae33d240c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""DashHold"",
                    ""type"": ""Button"",
                    ""id"": ""0a3cf411-87b2-4f49-8578-f662dcc7fbd4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.02,pressPoint=0.5)""
                },
                {
                    ""name"": ""DashDown"",
                    ""type"": ""Button"",
                    ""id"": ""91ba58ca-85d7-4e2a-85b2-7c4f1b1972a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill1"",
                    ""type"": ""Button"",
                    ""id"": ""1ded5fed-710c-4f1a-b755-092c712f4cb5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SkillWindow"",
                    ""type"": ""Button"",
                    ""id"": ""be7fe3e8-df4f-4486-85e4-c5c6d4e39e03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""1"",
                    ""type"": ""Button"",
                    ""id"": ""9d6e801e-4e2f-405b-a04a-e64200ac569f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""2"",
                    ""type"": ""Button"",
                    ""id"": ""b7f31659-078e-485b-a062-142718b118d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""3"",
                    ""type"": ""Button"",
                    ""id"": ""313d6587-523a-46b1-94da-edaf64f14e97"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""4"",
                    ""type"": ""Button"",
                    ""id"": ""9cc6895d-524f-411e-86e2-6c7eb8d72e13"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b3d915a9-a200-445f-aa13-df085904baac"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""94d67ad6-2720-4fe7-8447-165537d2fe73"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6c29acb4-d2ce-4b85-ba55-0b17f5a0b435"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3990e7ac-56dc-4e84-9cc0-26f528f941f5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ff6be67a-5fb4-4c61-aaa0-da64ea50bb29"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8f780291-f3ad-4bad-a465-c9316f188c39"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""91654843-6db3-4538-9b8c-7022204e6586"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveScreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd2e0ab8-fd87-41c7-82fe-9907a46bd3bc"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e94a0f95-bdc9-444a-b7ea-6024b6b1fb6f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC+CONSOLE"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""39913eaa-4210-4488-a3eb-085c1839e18a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6575360f-9ae5-46ac-be34-8193d3e721ea"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC+CONSOLE"",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2f5e2cd-7119-43d9-a4f1-a2246d8ffc47"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DashHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7791a565-5c04-47bc-ba78-71e5ecaa5b9a"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DashHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa5a0ea1-304e-41cc-8fcc-30468e746a08"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DashDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7ba47b7-6863-46df-b08e-c76636754d5c"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DashDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b013f49-e2d1-419a-8fe0-a585121cfe42"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC+CONSOLE"",
                    ""action"": ""AttackOff"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c224e98a-d92c-4542-8e86-927cd9015480"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC+CONSOLE"",
                    ""action"": ""AttackOff"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d7053b14-f08e-4cfd-ab12-b1ca2ea2e37d"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8732a63c-b8af-44af-949e-e4efe925f5fd"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkillWindow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2c81c78-7f35-40f3-8013-c42236df5185"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8bb97169-1284-4262-9345-8610680e007b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47083058-a91c-4282-baf9-f0cdce9230ec"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a67fc522-518a-4717-8f5b-06eabdea91da"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ff45c03-8f7c-4d31-8fde-51f730857088"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC+CONSOLE"",
            ""bindingGroup"": ""PC+CONSOLE"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Play
        m_Play = asset.FindActionMap("Play", throwIfNotFound: true);
        m_Play_Move = m_Play.FindAction("Move", throwIfNotFound: true);
        m_Play_MoveScreen = m_Play.FindAction("MoveScreen", throwIfNotFound: true);
        m_Play_Attack = m_Play.FindAction("Attack", throwIfNotFound: true);
        m_Play_AttackOff = m_Play.FindAction("AttackOff", throwIfNotFound: true);
        m_Play_Inventory = m_Play.FindAction("Inventory", throwIfNotFound: true);
        m_Play_DashHold = m_Play.FindAction("DashHold", throwIfNotFound: true);
        m_Play_DashDown = m_Play.FindAction("DashDown", throwIfNotFound: true);
        m_Play_Skill1 = m_Play.FindAction("Skill1", throwIfNotFound: true);
        m_Play_SkillWindow = m_Play.FindAction("SkillWindow", throwIfNotFound: true);
        m_Play__1 = m_Play.FindAction("1", throwIfNotFound: true);
        m_Play__2 = m_Play.FindAction("2", throwIfNotFound: true);
        m_Play__3 = m_Play.FindAction("3", throwIfNotFound: true);
        m_Play__4 = m_Play.FindAction("4", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Play
    private readonly InputActionMap m_Play;
    private IPlayActions m_PlayActionsCallbackInterface;
    private readonly InputAction m_Play_Move;
    private readonly InputAction m_Play_MoveScreen;
    private readonly InputAction m_Play_Attack;
    private readonly InputAction m_Play_AttackOff;
    private readonly InputAction m_Play_Inventory;
    private readonly InputAction m_Play_DashHold;
    private readonly InputAction m_Play_DashDown;
    private readonly InputAction m_Play_Skill1;
    private readonly InputAction m_Play_SkillWindow;
    private readonly InputAction m_Play__1;
    private readonly InputAction m_Play__2;
    private readonly InputAction m_Play__3;
    private readonly InputAction m_Play__4;
    public struct PlayActions
    {
        private @PlayerControl m_Wrapper;
        public PlayActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Play_Move;
        public InputAction @MoveScreen => m_Wrapper.m_Play_MoveScreen;
        public InputAction @Attack => m_Wrapper.m_Play_Attack;
        public InputAction @AttackOff => m_Wrapper.m_Play_AttackOff;
        public InputAction @Inventory => m_Wrapper.m_Play_Inventory;
        public InputAction @DashHold => m_Wrapper.m_Play_DashHold;
        public InputAction @DashDown => m_Wrapper.m_Play_DashDown;
        public InputAction @Skill1 => m_Wrapper.m_Play_Skill1;
        public InputAction @SkillWindow => m_Wrapper.m_Play_SkillWindow;
        public InputAction @_1 => m_Wrapper.m_Play__1;
        public InputAction @_2 => m_Wrapper.m_Play__2;
        public InputAction @_3 => m_Wrapper.m_Play__3;
        public InputAction @_4 => m_Wrapper.m_Play__4;
        public InputActionMap Get() { return m_Wrapper.m_Play; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayActions set) { return set.Get(); }
        public void SetCallbacks(IPlayActions instance)
        {
            if (m_Wrapper.m_PlayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnMove;
                @MoveScreen.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnMoveScreen;
                @MoveScreen.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnMoveScreen;
                @MoveScreen.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnMoveScreen;
                @Attack.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnAttack;
                @AttackOff.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnAttackOff;
                @AttackOff.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnAttackOff;
                @AttackOff.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnAttackOff;
                @Inventory.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnInventory;
                @DashHold.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnDashHold;
                @DashHold.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnDashHold;
                @DashHold.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnDashHold;
                @DashDown.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnDashDown;
                @DashDown.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnDashDown;
                @DashDown.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnDashDown;
                @Skill1.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnSkill1;
                @Skill1.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnSkill1;
                @Skill1.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnSkill1;
                @SkillWindow.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnSkillWindow;
                @SkillWindow.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnSkillWindow;
                @SkillWindow.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnSkillWindow;
                @_1.started -= m_Wrapper.m_PlayActionsCallbackInterface.On_1;
                @_1.performed -= m_Wrapper.m_PlayActionsCallbackInterface.On_1;
                @_1.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.On_1;
                @_2.started -= m_Wrapper.m_PlayActionsCallbackInterface.On_2;
                @_2.performed -= m_Wrapper.m_PlayActionsCallbackInterface.On_2;
                @_2.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.On_2;
                @_3.started -= m_Wrapper.m_PlayActionsCallbackInterface.On_3;
                @_3.performed -= m_Wrapper.m_PlayActionsCallbackInterface.On_3;
                @_3.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.On_3;
                @_4.started -= m_Wrapper.m_PlayActionsCallbackInterface.On_4;
                @_4.performed -= m_Wrapper.m_PlayActionsCallbackInterface.On_4;
                @_4.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.On_4;
            }
            m_Wrapper.m_PlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @MoveScreen.started += instance.OnMoveScreen;
                @MoveScreen.performed += instance.OnMoveScreen;
                @MoveScreen.canceled += instance.OnMoveScreen;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @AttackOff.started += instance.OnAttackOff;
                @AttackOff.performed += instance.OnAttackOff;
                @AttackOff.canceled += instance.OnAttackOff;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @DashHold.started += instance.OnDashHold;
                @DashHold.performed += instance.OnDashHold;
                @DashHold.canceled += instance.OnDashHold;
                @DashDown.started += instance.OnDashDown;
                @DashDown.performed += instance.OnDashDown;
                @DashDown.canceled += instance.OnDashDown;
                @Skill1.started += instance.OnSkill1;
                @Skill1.performed += instance.OnSkill1;
                @Skill1.canceled += instance.OnSkill1;
                @SkillWindow.started += instance.OnSkillWindow;
                @SkillWindow.performed += instance.OnSkillWindow;
                @SkillWindow.canceled += instance.OnSkillWindow;
                @_1.started += instance.On_1;
                @_1.performed += instance.On_1;
                @_1.canceled += instance.On_1;
                @_2.started += instance.On_2;
                @_2.performed += instance.On_2;
                @_2.canceled += instance.On_2;
                @_3.started += instance.On_3;
                @_3.performed += instance.On_3;
                @_3.canceled += instance.On_3;
                @_4.started += instance.On_4;
                @_4.performed += instance.On_4;
                @_4.canceled += instance.On_4;
            }
        }
    }
    public PlayActions @Play => new PlayActions(this);
    private int m_PCCONSOLESchemeIndex = -1;
    public InputControlScheme PCCONSOLEScheme
    {
        get
        {
            if (m_PCCONSOLESchemeIndex == -1) m_PCCONSOLESchemeIndex = asset.FindControlSchemeIndex("PC+CONSOLE");
            return asset.controlSchemes[m_PCCONSOLESchemeIndex];
        }
    }
    public interface IPlayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMoveScreen(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnAttackOff(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnDashHold(InputAction.CallbackContext context);
        void OnDashDown(InputAction.CallbackContext context);
        void OnSkill1(InputAction.CallbackContext context);
        void OnSkillWindow(InputAction.CallbackContext context);
        void On_1(InputAction.CallbackContext context);
        void On_2(InputAction.CallbackContext context);
        void On_3(InputAction.CallbackContext context);
        void On_4(InputAction.CallbackContext context);
    }
}
