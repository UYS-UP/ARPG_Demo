using System;
using System.Collections;
using System.Collections.Generic;
using Game.Config;
using Google.Protobuf.GS;
using UnityEngine;
using Pathfinding;

public class FSM : MonoBehaviour
{
    public bool AI;
    [Header("全局唯一ID")]
    public int global_id;
    public int global_state;//全局的状态 0战斗 1和平

    public int id;

    [Header("副本ID")]
    public int scene_id;

    [Header("等级")]
    public int level=1;

    public UnitEntity unitEntity;//单位基础表
    public PlayerState currentState;
    Dictionary<int, PlayerState> stateData = new Dictionary<int, PlayerState>();

    [HideInInspector]
    public Transform _transform;
    [HideInInspector]
    public GameObject _gameObject;
    [HideInInspector]
    public int instance_id;

    public Animator animator;
    public CharacterController characterController;

    public UnitAttEntity att_base;//总属性
    public UnitAttEntity att_crn;//当前属性==>生命值

    public Seeker seeker;
    public bool yet = false;

    [Header("测试用的追踪目标")]
    public GameObject test_track_target;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if(yet) return;
        yet = true;
        _transform = this.transform;
        _gameObject = this.gameObject;
        instance_id=_gameObject.GetInstanceID();

        if (AI)
        {
            this.gameObject.layer = GameDefine.GetEnemyLayer(GameDefine.enemy_layer);
        }
        else
        {
            this.gameObject.layer = GameDefine.GetEnemyLayer(GameDefine.player_layer);
            DontDestroyOnLoad(this.gameObject);
        }

        seeker = _transform.GetComponent<Seeker>();
        if (seeker == null)
        {
            seeker = _gameObject.AddComponent<Seeker>();
        }
        seeker.startEndModifier = new StartEndModifier();
        seeker.startEndModifier.exactStartPoint = StartEndModifier.Exactness.Original;// NodeConnection;
        seeker.startEndModifier.exactEndPoint = StartEndModifier.Exactness.Original;// NodeConnection;
        seeker.drawGizmos = true;

        _gameObject.AddComponent<SimpleSmoothModifier>();

        animator = _transform.GetChild(0).GetComponent<Animator>();
        animator.applyRootMotion = false;
        characterController = GetComponent<CharacterController>();

        unitEntity = UnitData.Get(id);

        att_base = AttHelper.Instance.Creat(unitEntity.att_id);
        att_crn = AttHelper.Instance.Creat(att_base);

        ServiceInit();
        StateInit();

        if (string.IsNullOrEmpty(anmConfig.hit_obj) == false) {
            hit_target=GetHangPoint(anmConfig.hit_obj);
        }
        else
        {
            hit_target = _gameObject;
        }

        if (AI==false)
        {
            UnitManager.Instance.player = this;
        }
        
        ToNext(1001);
    }

    public GameObject hit_target;

    private void OnEnable()
    {
        EnableBossHp();
    }

    public void EnableBossHp() {
        if (AI)
        {
            if (global_state != 1 && unitEntity.type == 3)
            {
                MainViewController.Instance.EnableBossHP(true, unitEntity.info);
            }
        }

    }

    private void OnDisable()
    {
        if (AI)
        {
            if (unitEntity.type == 3)
            {
                MainViewController.Instance.EnableBossHP(false, unitEntity.info);
            }
        }
    }

    void Update()
    {
        if (currentState != null)
        {
            if (ServicesOnUpdate() == true)
            {
                DOStateEvent(currentState.id, StateEventType.update);//状态每帧执行的事件
            }

            ToGround();
        }

        if (AI)
        {
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                ForceLockPlayer();
                UTransform.LookTarget(_transform, UnitManager.Instance.player._transform);
               
                ToNext(1005);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                ForceLockPlayer();
                UTransform.LookTarget(_transform, UnitManager.Instance.player._transform);
                ToNext(1006);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                ForceLockPlayer();
                UTransform.LookTarget(_transform, UnitManager.Instance.player._transform);
                ToNext(1007);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                ForceLockPlayer();
                UTransform.LookTarget(_transform, UnitManager.Instance.player._transform);
                ToNext(1008);
            }


            if (Input.GetKeyDown(KeyCode.U))
            {
                ForceLockPlayer();
                UTransform.LookTarget(_transform,UnitManager.Instance.player._transform);
                ToNext(1009);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                ForceLockPlayer();
                UTransform.LookTarget(_transform, UnitManager.Instance.player._transform);
                ToNext(1010);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                ForceLockPlayer();
                UTransform.LookTarget(_transform, UnitManager.Instance.player._transform);
                ToNext(1011);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                ForceLockPlayer();
                UTransform.LookTarget(_transform, UnitManager.Instance.player._transform);
                ToNext(1012);
            }
        }
    }

    StateScriptableObject anmConfig;
    public void StateInit() {
        anmConfig = Resources.Load<StateScriptableObject>($"StateConfig/{id}");
        Dictionary<int, StateEntity> state_config = new Dictionary<int, StateEntity>();
        foreach (var item in anmConfig.states)
        {
            state_config[item.id] = item;
        }

        var clips = animator.runtimeAnimatorController.animationClips;
        Dictionary<string, float> clipLength = new Dictionary<string, float>();
        foreach (var clip in clips)
        {
            clipLength[clip.name] = clip.length;
        }


        if (PlayerStateData.all != null) {
            foreach (var item in PlayerStateData.all)
            {
                PlayerState p = new PlayerState();
                p.excel_config = item.Value;
                p.id = item.Key;
                p.stateEntity = state_config[p.id];
                if (clipLength.TryGetValue(item.Value.anm_name, out var length_clip))
                {
                    p.clipLength = length_clip;
                }
                stateData[item.Key] = p;
            }
        }

        //设置技能数据
        stateData[1005].skill = SkillData.Get(unitEntity.ntk1);//普攻1
        stateData[1006].skill = SkillData.Get(unitEntity.ntk2);
        stateData[1007].skill = SkillData.Get(unitEntity.ntk3);
        stateData[1008].skill = SkillData.Get(unitEntity.ntk4);//普攻4

        stateData[1009].skill = SkillData.Get(unitEntity.skill1);//技能1
        stateData[1010].skill = SkillData.Get(unitEntity.skill2);
        stateData[1011].skill = SkillData.Get(unitEntity.skill3);
        stateData[1012].skill = SkillData.Get(unitEntity.skill4);//技能4

        stateData[1046].skill = SkillData.Get(unitEntity.use_prop_1);//道具1
        stateData[1047].skill = SkillData.Get(unitEntity.use_prop_2);//道具2
        stateData[1048].skill = SkillData.Get(unitEntity.use_prop_3);//道具3
        stateData[1049].skill = SkillData.Get(unitEntity.use_prop_4);//道具4

        if (AI==false)
        {
            //主角要注册的事件
            foreach (var item in stateData)
            {
                if (item.Value.excel_config.on_move != null)
                {
                    AddListener(item.Key, StateEventType.update, OnMove);
                }

                if (item.Value.excel_config.do_move == 1)
                {
                    AddListener(item.Key, StateEventType.update, PlayerMove);
                }

                if (item.Value.excel_config.on_stop != 0)
                {
                    AddListener(item.Key, StateEventType.update, OnStop);
                }

                if (item.Value.excel_config.on_jump != null)
                {
                    AddListener(item.Key, StateEventType.update, OnJump);
                }

                if (item.Value.excel_config.on_jump_end != 0)
                {
                    AddListener(item.Key, StateEventType.update, OnJumpUpdate);
                }

                if (item.Value.excel_config.add_f_move > 0)
                {
                    AddListener(item.Key, StateEventType.update, AddForwardMove);
                }

                if (item.Value.excel_config.on_atk != null)
                {
                    AddListener(item.Key, StateEventType.update, OnAtk);
                }

                if (item.Value.excel_config.on_skill1 != null)
                {
                    AddListener(item.Key, StateEventType.update, OnSkill1);
                }

                if (item.Value.excel_config.on_skill2 != null)
                {
                    AddListener(item.Key, StateEventType.update, OnSkill2);
                }

                if (item.Value.excel_config.on_skill3 != null)
                {
                    AddListener(item.Key, StateEventType.update, OnSkill3);
                }

                if (item.Value.excel_config.on_skill4 != null)
                {
                    AddListener(item.Key, StateEventType.update, OnSkill4);
                }

                if (item.Value.excel_config.on_defense != null)
                {
                    AddListener(item.Key, StateEventType.update, OnDefense);
                }

                if (item.Value.excel_config.on_defense_quit != 0)
                {
                    AddListener(item.Key, StateEventType.update, OnDefenseQuit);
                }

                if (item.Value.excel_config.on_sprint != null)
                {
                    AddListener(item.Key, StateEventType.update, OnSprint);
                }

                if (item.Value.excel_config.on_pow_atk != null)
                {
                    AddListener(item.Key, StateEventType.update, OnPowAtk);
                }

                if (item.Value.excel_config.do_rotate != 0)
                {
                    AddListener(item.Key, StateEventType.update, DORotate);
                }

                if (item.Value.stateEntity.ignor_collision == true)
                {
                    AddListener(item.Key, StateEventType.begin, DisableCollider);
                    AddListener(item.Key, StateEventType.end, EnableCollider);
                }
                if (item.Value.excel_config.on_use_prop != null)
                {
                    AddListener(item.Key, StateEventType.update, UseProp);
                }

                //释放技能后 需要调度设置cd的接口
                if (item.Value.skill!=null)
                {
                    //AddListener(item.Key, StateEventType.begin, SkillFx);
                    AddListener(item.Key, StateEventType.begin, OnSkillBegin);
                    AddListener(item.Key, StateEventType.begin, SetSkillCD);
                }
            }

            AddListener(1013, StateEventType.begin, Block_FX);
            AddListener(1044, StateEventType.update, CreateExecuteEffect);
        }
        else
        {
            foreach (var item in stateData)
            {
                //进入状态超过多长时间
                if (item.Value.excel_config.active_attack>0)
                {
                    AddListener(item.Key, StateEventType.update, AutoTriggerAtk_AI);//自动攻击
                }

                if (item.Value.excel_config.trigger_pacing>0)
                {
                    AddListener(item.Key, StateEventType.onAnmEnd, TriggerPacing);//战斗踱步
                }

                if (item.Value.excel_config.tag==4)
                {
                    AddListener(item.Key, StateEventType.update, OnPacingUpdate);
                }

                if (item.Value.excel_config.trigger_patrol>0)
                {
                    AddListener(item.Key, StateEventType.update, TriggerPatrol);
                }
            }
            //AI要注册的事件
            GameEvent.OnPlayerAtk += OnPlayerAtk;
            AddListener(1001, StateEventType.update, LookAtkTarget);
            AddListener(1014, StateEventType.onAnmEnd, OnDashEnd);
            AddListener(1042, StateEventType.update, OnMoveToPoint);
            AddListener(1042, StateEventType.end, NavStop);

            AddListener(1013, StateEventType.update, AI_Defending);
            AddListener(10131, StateEventType.update, AI_Defending);
            AddListener(1043, StateEventType.update, OnPatrolUpdate);
            AddListener(1043, StateEventType.begin, ChangeMoveSpeed);
            AddListener(1043, StateEventType.end, OnPatrolEnd);
            AddListener(1031, StateEventType.update, TriggerAtk_AI);
        }

        AddListener(1017, StateEventType.update, OnBashUpdate);
        AddListener(1017, StateEventType.end, OnBashEnd);

        AddListener(1018, StateEventType.update, OnBashUpdate);
        AddListener(1018, StateEventType.end, OnBashEnd);
    }

    private void Block_FX()
    {
        var beginPos = _transform.position + Vector3.up;
        RaycastHit hitinfo;
        var result = Linecast_FxToTarget(beginPos, beginPos + _transform.forward * 7, out hitinfo);

        if (result)
        {
            return;
        }
        //45
        var count = 90/5;

        for (int i = 1; i <= count; i++)
        {
            var result1 = Linecast_FxToTarget(beginPos, _transform.GetOffsetPoint(7, i * 5), out hitinfo);
            if (result1)
            {
                return;
            }
            var result2 = Linecast_FxToTarget(beginPos, _transform.GetOffsetPoint(7, i * -5), out hitinfo);
            if (result2)
            {
                return;
            }
        }
    }

    private void SkillFx(PlayerState state)
    {
        if (state.skill!=null&& state.skill.fx_angle>0)
        {
            var beginPos = _transform.position + Vector3.up;
            RaycastHit hitinfo;
            var result= Linecast_FxToTarget(beginPos, beginPos + _transform.forward * state.skill.atk_distance,out hitinfo);

            if (result)
            {
                return;
            }
            //45
            var count=state.skill.fx_angle/5;

            for (int i = 1; i <= count; i++)
            {
                var result1 = Linecast_FxToTarget(beginPos, _transform.GetOffsetPoint(state.skill.atk_distance, i *5),out hitinfo);
                if (result1)
                {
                    return;
                }
                var result2 = Linecast_FxToTarget(beginPos, _transform.GetOffsetPoint(state.skill.atk_distance, i *-5),out hitinfo);
                if (result2)
                {
                    return;
                }
            }
        }
        atk_fsm = null;
    }

    private FSM atk_fsm;

    public FSM GetAtkTarget() {
        ForceLockPlayer();
        return atk_fsm;
    }

    public void ForceLockPlayer() {
        if (atk_fsm==null)
        {
            atk_fsm = UnitManager.Instance.player;
        }
    }

    public bool Linecast_FxToTarget(Vector3 beginPos,Vector3 endPos,out RaycastHit hitinfo) {
        var result = Physics.Linecast(beginPos, endPos,
                out hitinfo, GetEnemyLayerMask());
        if (result) {
            atk_fsm = _transform.GetComponent<FSM>();
            if (atk_fsm != null)
            {
                if (atk_fsm.IsDead())
                {
                    return false;
                }
            }
            else
            {
               var f= _transform.GetComponentInParent<FSM>();
                if (f.IsDead())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
         _transform.LookTarget(hitinfo.transform);
        }
        return result;
    }

    private void SetSkillCD()
    {
        if (currentState.id==1009)
        {
            MainViewController.Instance.SetSkillCD(0, currentState.skill.cd);
        }
        else if (currentState.id == 1010)
        {
            MainViewController.Instance.SetSkillCD(1, currentState.skill.cd);
        }
        else if (currentState.id == 1011)
        {
            MainViewController.Instance.SetSkillCD(2, currentState.skill.cd);
        }
        else if (currentState.id == 1012)
        {
            MainViewController.Instance.SetSkillCD(3, currentState.skill.cd);
        }
    }

    private void ChangeMoveSpeed()
    {
        _speed = 2.5f;
    }

    private void OnPatrolEnd()
    {
        _speed = 5f;
        navigationService.Stop();
    }

    private void TriggerPatrol()
    {
        if (global_state == 1)
        {
            return;
        }

        if (atk_target==null||GetDst()>10)
        {
            if (GameTime.time - currentState.begin >= currentState.excel_config.trigger_patrol)
            {
                //进入巡逻
                //ToNext(1043);

                //自己周边3-6米的位置 
                var r = IntEx.Range(3, 6);
                var a = IntEx.Range(0, 359);
                var target = _transform.GetOffsetPoint(r, a);
                navigationService.Move(target, ToPatrol);
            }
        }
    }

    //切换到巡逻状态
    public void ToPatrol() {
        ToNext(1043);
    }

    //巡逻到目的地(或者超时) 切换到待机  
    public void OnPatrolUpdate() {
        if (GameTime.time - currentState.begin>=5f||navigationService.IsEnd())
        {
            ToNext(1001);
        }
        
    }


    private void AI_Defending()
    {
        LookAtkTarget();

        //如果超过2.5f 让它回到待机的状态
        if (GameTime.time-currentState.begin>=2.5f)
        {
            ToNext(10132);
        }
    }

    public void LookAtkTarget() {
        if (atk_target != null)
        {
            this._transform.LookTarget(atk_target._transform);
        }
    }
    public float GetDst()
    {
        return Vector3.Distance(this._transform.position, atk_target._transform.position);
    }

    public float GetDst(Transform target) {
        return Vector3.Distance(this._transform.position, target.position);
    }

    private void OnPacingUpdate()
    {
        if (GameTime.time - currentState.begin >= 5)
        {
            ToNext(1001);
        }

        if (atk_target!=null)
        {
            LookAtkTarget();

            if (GetDst() <= 3)
            {
                AIAtk();
                return;
            }
        }
    }

    private void TriggerPacing()
    {
        if (global_state == 1)
        {
            return;
        }
        if (IsDead())
        {
            return;
        }

        if (unitEntity.pacing_probability>0)
        {

            if (unitEntity.pacing_probability.InRange())
            {
                if (atk_target==null)
                {
                    var dst = Vector3.Distance(_transform.position, UnitManager.Instance.player._transform.position);
                    if (dst < 10) {
                        atk_target = UnitManager.Instance.player;
                    }
                    else
                    {
                        return;
                    }
                }

                if (currentState.excel_config.tag==4)
                {
                    if (GameTime.time-currentState.begin>=IntEx.Range(3,6))
                    {
                        var next= IntEx.Range(1036, 1041);
                        _transform.LookTarget(atk_target._transform);
                        ToNext(next);
                    }
                }
                else
                {
                    var next = IntEx.Range(1036, 1041);
                    _transform.LookTarget(atk_target._transform);
                    ToNext(next);
                }

            }
        }
    }

    private void AutoTriggerAtk_AI()
    {
        if (global_state == 1)
        {
            return;
        }
        if (GameTime.time-currentState.begin>=currentState.excel_config.active_attack)
        {
            AIAtk();
        }
    }

    float _OnMoveToPoint_CheckTime;
    private void OnMoveToPoint()
    {
        if (GameTime.time- _OnMoveToPoint_CheckTime>0.1)
        {
            _OnMoveToPoint_CheckTime = GameTime.time;

            if (next_atk!=0&&atk_target!=null)
            {
                var dst = GetDst();
                if (dst <= stateData[next_atk].skill.atk_distance)
                {
                    navigationService.Stop();
                    this._transform.LookTarget(atk_target._transform);
                    ToNext(next_atk);
                    next_atk = 0;
                }
                else
                {
                    //寻路到终点了  或者移动超时5f
                    if (navigationService.IsEnd()||GameTime.time- currentState.begin>=5f)
                    {
                        navigationService.Stop();
                        ToNext(1001);
                        AIAtk();
                        next_atk = 0;
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    public void RemoveListener() {
        GameEvent.OnPlayerAtk -= OnPlayerAtk;
    }

    private void OnPlayerAtk(FSM atk, SkillEntity arg2)
    {
        if (global_state==1)
        {
            return;
        }
        if (att_crn.hp<=0)
        {
            return;
        }

        float a = Vector3.Angle(atk._transform.position, this._transform.position);
        var is_forward = _transform.ForwardOrBack(atk._transform.position)>0;
        //5米距离内 需要去处理做出响应
        if (GetDst(atk._transform)<=arg2.atk_distance)
        {
            if (unitEntity.block_probability.InRange()&& is_forward && a <= 45f)
            {
                if (CheckConfig(currentState.excel_config.on_defense))
                {
                    _transform.LookTarget(atk._transform);
                    var result = ToNext((int)currentState.excel_config.on_defense[2]);
                    if (result)
                    {
                        return;
                    }
                }
            }
            //躲闪概率的检测
            else if (unitEntity.dodge_probability.InRange()&& a <= 45f&& is_forward)
            {
                if (currentState.excel_config.trigger_dodge>0)
                {
                    _transform.LookTarget(atk._transform);
                    var next= IntEx.Range(1032, 1035);
                    var result= ToNext(next);
                    if (result)
                    {
                        return;
                    }
                }
            }
            else if (unitEntity.atk_probability.InRange())
            { 
                //抢手攻击的检测
                if (currentState.excel_config.first_strike>0)
                {
                    TriggerAtk_AI();
                }
            }
        }

    }

    private void TriggerAtk_AI()
    {
        if (animationService.normalizedTime>=currentState.excel_config.trigger_atk)
        {
            AIAtk();
        }
    }

    public bool IsDead() {
        return att_crn.hp <= 0;
    }

    int next_atk;
    private void AIAtk()
    {
        if (IsDead())
        {
            return;
        }
        next_atk = IntEx.Range(1005, 1012);//1005;//
        //挑选攻击的决策
        //根据对方的技能ID 自己做出对应的技能ID

        if (stateData[next_atk].skill.atk_distance>0)
        {
            if (atk_target==null)
            {
                atk_target = UnitManager.Instance.player;
            }
            if (GetDst() >= stateData[next_atk].skill.atk_distance)
            {
                this._transform.LookTarget(atk_target._transform);

               
                if (IntEx.Range(0, 100) <= 50)
                {
                    //寻路移动过去 
                    if (navigationService.state==0)
                    {
                        navigationService.Move(atk_target._transform.position, MoveToPoint);
                    }
                }
                else
                {
                    ToNext(1014); //突进冲刺 
                }
               
            }
            else
            {
                this._transform.LookTarget(atk_target._transform);
                ToNext(next_atk);
            }
        }
    }

    private void MoveToPoint()
    {
        //寻路的状态
        ToNext(1042);
    }

    public void NavStop()
    {
        navigationService.Stop();
    }

    public void OnDashEnd()
    {
        if (next_atk!=0&&atk_target!=null)
        {
            var dst = GetDst();
            if (dst <= stateData[next_atk].skill.atk_distance)
            {
                this._transform.LookTarget(atk_target._transform);
                ToNext(next_atk);
                next_atk = 0;
            }
            else
            {
                AIAtk();
            }
        }

    }

    private void OnSkillBegin()
    {
        GameEvent.OnPlayerAtk?.Invoke(this,currentState.skill);
    }

    private void EnableCollider()
    {
        characterController.excludeLayers = 0;
    }

    private void DisableCollider()
    {
        characterController.excludeLayers = GameDefine.Enemy_LayerMask;
    }

    private void DORotate()
    {
        if (animationService.normalizedTime <= currentState.excel_config.do_rotate)
        {
            var x = UInput.GetAxis_Horizontal();
            var z = UInput.GetAxis_Vertical();
            if (x != 0 || z != 0)
            {
                Vector3 inputDirection = new Vector3(x, 0.0f, z).normalized;
                //2：Atan2 的优点在于 如果 x2-x1等于0 依然可以计算，但是Atan函数就会导致程序出错。
                //3：Mathf.Atan()返回的值的范围是[-π / 2, π / 2]，Mathf.Atan()返回的值的范围是[-π / 2, π / 2]。
                //正切函数求弧度
                //正切函数 *弧度转度 

                //第一:先求出输入的角度
                //第二:加上当前相机的Y轴旋转的量
                //第三:得到目标朝向的角度
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  GameDefine._Camera.eulerAngles.y;

                //做一个插值运动
                float rotation = Mathf.SmoothDampAngle(_transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    0.025f);

                //角色先旋转到目标角度去
                _transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }
    }

    float pow_atk_begin;
    private void OnPowAtk()
    {
        if (UInput.GetMouseButton_0()) {
            if (pow_atk_begin == 0)
            {
                pow_atk_begin = Time.time;
            }
            if (Time.time - pow_atk_begin >= 0.1f)
            {
                if (CheckConfig(currentState.excel_config.on_pow_atk))
                {
                    ToNext((int)currentState.excel_config.on_pow_atk[2]);
                }
            }
        }
    }

    private void OnSprint()
    {
        if (UInput.GetKeyUp_LeftShift())
        {
            if (CheckConfig(currentState.excel_config.on_sprint))
            {
                ToNext((int)currentState.excel_config.on_sprint[2]);
            }
        }
    }

    private void OnDefense()
    {
        if (UInput.GetMouseButtonDown_1())
        {
            if (CheckConfig(currentState.excel_config.on_defense))
            {
                ToNext((int)currentState.excel_config.on_defense[2]);
            }
        }
    }

    private void OnDefenseQuit()
    {
        if (UInput.GetMouseButtonUP_1())
        {
            ToNext((int)currentState.excel_config.on_defense_quit);
        }
    }


    private void OnSkill1()
    {
        if (UInput.GetKeyDown_Q())
        {
            if (CheckConfig(currentState.excel_config.on_skill1))
            {
                ToNext((int)currentState.excel_config.on_skill1[2]);
            }
        }
    }

    private void OnSkill2()
    {
        if (UInput.GetKeyDown_E()) {

            if (CheckConfig(currentState.excel_config.on_skill2))
            {
                ToNext((int)currentState.excel_config.on_skill2[2]);
            }
        }

    }

    private void OnSkill3()
    {
        if (UInput.GetKeyDown_R())
        {
            if (CheckConfig(currentState.excel_config.on_skill3))
            {
                ToNext((int)currentState.excel_config.on_skill3[2]);
            }
        }
    }

    private void OnSkill4()
    {
        if (UInput.GetKeyDown_T())
        {
            if (CheckConfig(currentState.excel_config.on_skill4))
            {
                ToNext((int)currentState.excel_config.on_skill4[2]);
            }
        }
    }

    private void OnAtk()
    {
        if (UInput.GetMouseButtonUp_0())
        {
            if (CheckConfig(currentState.excel_config.on_atk))
            {
                //目标修正检测 如果发现目标 并且目标是背向我们  就调用处决的动作
                var beginPos = _transform.position + Vector3.up;
                RaycastHit hitinfo;
                var result = Linecast_FxToTarget(beginPos, beginPos + _transform.forward * 3, out hitinfo);

                if (result)
                {
                    if (DOExecute(hitinfo.transform))
                    {
                        return;
                    }
                }
                //45
                var count = 45 / 5;

                for (int i = 1; i <= count; i++)
                {
                    var result1 = Linecast_FxToTarget(beginPos, _transform.GetOffsetPoint(3, i * 5), out hitinfo);
                    if (result1)
                    {
                        if (DOExecute(hitinfo.transform))
                        {
                            return;
                        } 
                    }
                    var result2 = Linecast_FxToTarget(beginPos, _transform.GetOffsetPoint(3, i * -5), out hitinfo);
                    if (result2)
                    {
                        if (DOExecute(hitinfo.transform))
                        {
                            return;
                        }
                    }
                }



                ToNext((int)currentState.excel_config.on_atk[2]);
            }
        }
    }
    void CreateExecuteEffect() {
        if (execute_effect_state == 0 && animationService.normalizedTime >= 0.18f)
        {
                execute_effect_state = 1;
                var effect_parent = execute_target_fsm.GetHangPoint(execute_target_fsm.anmConfig.be_execut_effect);
                var obj = ResourcesManager.Instance.Create_Skill("Effect/Execut");
                if (effect_parent != null)
                {
                    obj.transform.position = effect_parent.transform.position;
                    obj.transform.forward = effect_parent.transform.position - GameSystem.Instance.CameraController.transform.position;
                }
        }
        else if (execute_effect_state == 1 && animationService.normalizedTime >= 0.543f) {

            execute_effect_state = 2;
            var effect_parent = execute_target_fsm.GetHangPoint(execute_target_fsm.anmConfig.be_execut_effect);
            var obj = ResourcesManager.Instance.Create_Skill("Effect/Execut");
            if (effect_parent != null)
            {
                obj.transform.position = effect_parent.transform.position;
                obj.transform.forward = effect_parent.transform.position - GameSystem.Instance.CameraController.transform.position;
            }
        }
    }
    int execute_effect_state;//1表名已经创建了第一个特效  如果是2 表明已经创建第二个特效
    FSM execute_target_fsm;

    public void EnableCharacterController(bool enable) {
        characterController.enabled = enable;
    }

    public bool DOExecute(Transform target) {
        if (target.transform.ForwardOrBack(this._transform.position)<0)
        {
            var _target_fsm = target.GetComponent<FSM>();
            if (_target_fsm != null&&_target_fsm.global_state==1) {
                return false;
            }
            execute_effect_state = 0;
            ToNext(1044);
         
            //将当前角色的位置,修正到敌方背后的0.3米处
            _transform.position = _target_fsm._transform.position - _target_fsm._transform.forward * 0.3f;
            _target_fsm.EnableCharacterController(false);
            Physics.SyncTransforms();
            _transform.LookTarget(_target_fsm._transform);

            execute_target_fsm = target.GetComponent<FSM>();
            execute_target_fsm.ToNext(1045);
            _target_fsm.UpdateHP_OnHit((int)_target_fsm.att_crn.hp);

            var follow_target = GetHangPoint(anmConfig.block_camera_follow);
            if (follow_target != null)
            {
                GameSystem.Instance.CameraController.OnExecute(CombatConfig.Instance.Config().execute_camer_config, follow_target.transform);
            }

            return true;
        }
        return false;
      
    }

    private void AddForwardMove()
    {
        var x = UInput.GetAxis_Horizontal();
        var z = UInput.GetAxis_Vertical();
        if (x != 0 || z > 0)
        {
            var v = new Vector3(x, 0, z > 0 ? z : 0).normalized * currentState.excel_config.add_f_move;
            Move(v, true, true, false, false);
        }
    }
    public float jump_count;
    internal float GetForceMutilpe()
    {
        if (jump_count == 0)
        {
            return 1;
        }
        else
        {
            return 1.5f;
        }
    }

    private void OnJump()
    {
        if (UInput.GetKeyDown_Space())
        {
            if (CheckConfig(currentState.excel_config.on_jump))
            {
                if (currentState.id==1020|| currentState.id == 1021){
                    //当前脚是不是碰到墙壁 如果是就可以垫脚再进行跳跃
                    if (Physics.Raycast(_transform.position, _transform.forward, 0.5f, GameDefine.Obs_Layer)==false) {
                        return;
                    }
                    else
                    {
                        if (jump_count<1)
                        {
                            jump_count += 1;
                        }
                        else
                        {
                            return;
                        }
                        
                    }

                }
                //Debug.LogError("跳跃"+currentState.id);
                ToNext((int)currentState.excel_config.on_jump[2]);
            }
        }
    }
    public void OnJumpUpdate() {
        if (Physics.Raycast(_transform.position, -_transform.up, 0.15f, GameDefine.Jump_End_LayerMask))
        {
            ToNext(currentState.excel_config.on_jump_end);
        }
    }


    private void OnStop()
    {
        if (UInput.GetAxis_Horizontal() == 0 && UInput.GetAxis_Vertical() == 0)
        {
            jump_count = 0;
            ToNext(currentState.excel_config.on_stop);
        }
    }

    float _targetRotation;
    float _rotationVelocity;
    float RotationSmoothTime = 0.05f;
    public float _speed = 5;
    private void PlayerMove()
    {
        var x = UInput.GetAxis_Horizontal();
        var z = UInput.GetAxis_Vertical();
        if (x != 0 || z != 0)
        {
            Vector3 inputDirection = new Vector3(x, 0f, z).normalized;

            //Mathf.Atan2 正切函数 求弧度 * Mathf.Rad2Deg(弧度转度数) >> 度数

            //第一:先求出输入的角度
            //第二:加上当前相机的Y轴旋转的量
            //第三:得到目标朝向的角度
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              GameDefine._Camera.eulerAngles.y;

            //做一个插值运动
            float rotation = Mathf.SmoothDampAngle(_transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            //角色先旋转到目标角度去
            // rotate to face input direction relative to camera position
            _transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            //计算目标方向 通过这个角度
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            Move(targetDirection.normalized * (_speed * GameTime.deltaTime), false, false, false, true);
        }
    }



    public bool CheckConfig(float[] config) {
        if (config == null) {
            return false;
        }
        else
        {
            //看是用 unity里的状态配置的参数为准 还是excel的
            if (currentState.stateEntity.overwrite_atk)
            {

                if ((animationService.normalizedTime >= 0 && animationService.normalizedTime <= currentState.stateEntity.atk_before)
                            || animationService.normalizedTime >= currentState.stateEntity.atk_after)
                {
                    return true;
                }
            }
            else
            {

                if ((animationService.normalizedTime >= 0 && animationService.normalizedTime <= config[0])
                            || animationService.normalizedTime >= config[1])
                {
                    return true;
                }
            }

            return false;
        }
    }

    private void OnMove()
    {
        //键盘输入 wasd 
        if (UInput.GetAxis_Horizontal() != 0 || UInput.GetAxis_Vertical() != 0)
        {
            if (CheckConfig(currentState.excel_config.on_move))
            {
                ToNext((int)currentState.excel_config.on_move[2]);
            }
        }
    }
    bool ground_check = false;

    public void ToGround() {
        if (ground_check)
        {
            //射线投射 
            if (Physics.Linecast(_transform.position, _transform.position + GameDefine._Ground_Dst, GameDefine.Jump_End_LayerMask))
            {
                ground_check = false;
            }
            else
            {
                Move(_transform.up * -9.81f, false, false, false, false);
            }
        }
    }

    /// <summary>
    /// 移动核心接口 动作位移配置 输入位移wasd 击飞 击退 ==统一走这个接口
    /// </summary>
    /// <param name="d">移动的向量</param>
    /// <param name="transforDirection">是否将本地坐标转为世界空间坐标</param>
    /// <param name="delteTime">2帧间隔时间</param>
    /// <param name="_add_gravity">是否添加重力</param>
    /// <param name="_do_ground_check">是否移动后标记为接地检测</param>
    public void Move(Vector3 d, bool transforDirection, bool delteTime = true, bool _add_gravity = true, bool _do_ground_check = true)
    {
        if (transforDirection)
        {
            d = this._transform.TransformDirection(d);
        }
        Vector3 d2;
        if (_add_gravity)
        {
            d2 = (d + GameDefine._Gravity) * (delteTime ? GameTime.deltaTime : 1);
        }
        else
        {
            d2 = d * (delteTime ? GameTime.deltaTime : 1);
        }

        characterController.Move(d2);

        if (_do_ground_check)
        {
            ground_check = true;
        }
    }

    public bool ToNext(int next) {
        if (stateData.ContainsKey(next))
        {

            if (currentState != null)
            {
                Debug.Log($"{this.gameObject.name}:切换状态:{stateData[next].Info()}  当前是:{currentState.Info()}");
            }
            else
            {
                Debug.Log($"{this.gameObject.name}:切换状态:{stateData[next].Info()}");
            }

            //下一个状态 是否还处于CD中
            var next_state = stateData[next];
            //调试 先注释掉
            //if (next_state.skill!=null&& next_state.begin!=0&&GameTime.time- next_state.begin< next_state.skill.cd)
            //{
            //    MainViewController.Instance.OpenCD_Tips();
            //    return false;
            //}

            if (currentState != null) {
                DOStateEvent(currentState.id, StateEventType.end);//状态绑定的退出事件
                ServicesOnEnd();
            }

            pow_atk_begin = 0;

            if (next_state != null&&AI==false)
            {
                SkillFx(next_state);
            }
           
            currentState = next_state;
            if (!(currentState.id == 1020 || currentState.id == 1021))
            {
                jump_count = 0;
            }
            currentState.SetBeginTime();

            ServicesOnBegin();
            DOStateEvent(currentState.id, StateEventType.begin); //执行当前状态的开始(进入)事件
            return true;
        }
        return false;
    }

    public void AnimationOnPlayEnd() {
        var _id = currentState.id;

        DOStateEvent(currentState.id, StateEventType.onAnmEnd);
        ServicesOnAnimationEnd();

        if (currentState.id != _id)
        {
            return;
        }

        switch (currentState.excel_config.on_anm_end)
        {
            case -1:
                break;
            case 0:
                ServicesOnReStart();
                return;
            default:
                ToNext(currentState.excel_config.on_anm_end);
                break;
        }
    }

    //alt+enter

    //存储每个状态<不同类型的事件,<同一个类型可以存在多个事件,所以用了List来进行缓存>>
    //int 状态ID,Dictionary事件容器
    //事件容器StateEventType:事件类型 value(List<Action>) 代表类型对应的事件列表
    public Dictionary<int, Dictionary<StateEventType, List<Action>>> actions = new Dictionary<int, Dictionary<StateEventType, List<Action>>>();
    /// <summary>
    /// 添加事件的接口
    /// </summary>
    /// <param name="id">状态ID</param>
    /// <param name="t">事件类型</param>
    /// <param name="action">事件</param>
    public void AddListener(int id, StateEventType t, Action action) {
        if (!actions.ContainsKey(id))
        {
            actions[id] = new Dictionary<StateEventType, List<Action>>();
        }

        //如果不包含对应的事件类型 begin end
        if (actions[id].ContainsKey(t) == false)
        {
            //actions[id] = new Dictionary<StateEventType, List<Action>>();
            List<Action> list = new List<Action>();
            list.Add(action);
            actions[id][t] = list;
        }
        else
        {
            actions[id][t].Add(action);
        }
    }

    /// <summary>
    /// 提供快速调用不同状态不同类型的事件
    /// </summary>
    /// <param name="id">状态ID</param>
    /// <param name="t">事件类型</param>
    public void DOStateEvent(int id, StateEventType t) {
        if (actions.TryGetValue(id, out var v))
        {
            if (v.TryGetValue(t, out var lst))
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    lst[i].Invoke();
                }
            }
        }
    }


    //service
    public List<FSMServiceBase> fSMServices = new List<FSMServiceBase>();
    AnimationService animationService;
    PhysicsService physicsService;
    ObjService objService;
    HitlagService hitlagService;
    RadialBlurService radialBlurService;
    HitService hitService;
    NavigationService navigationService;
    EffectService effectService;
    SummonService summonService;
    public T AddService<T>() where T : FSMServiceBase, new()
    {
        T com = new T();
        fSMServices.Add(com);
        com.Init(this);
        return com;
    }

    int service_count;
    //注册 添加不同服务组件
    public void ServiceInit() {
        animationService = AddService<AnimationService>();
        physicsService = AddService<PhysicsService>();
        objService = AddService<ObjService>();
        hitlagService = AddService<HitlagService>();
        radialBlurService=AddService<RadialBlurService>();
        hitService=AddService<HitService>();
        navigationService=AddService<NavigationService>();
        effectService=AddService<EffectService>();
        summonService = AddService<SummonService>();
        service_count = fSMServices.Count;
    }

    public void ServicesOnBegin() {
        for (int i = 0; i < service_count; i++)
        {
            fSMServices[i].OnBegin(currentState);
        }
    }

    public bool ServicesOnUpdate()
    {
        int crn_state_id = currentState.id;
        for (int i = 0; i < service_count; i++)
        {
            fSMServices[i].OnUpdate(animationService.normalizedTime, currentState);
            if (currentState.id != crn_state_id)
            {
                return false;
            }
        }
        return true;
    }

    public void ServicesOnEnd()
    {
        for (int i = 0; i < service_count; i++)
        {
            fSMServices[i].OnEnd(currentState);
        }
    }

    public void ServicesOnAnimationEnd()
    {
        for (int i = 0; i < service_count; i++)
        {
            fSMServices[i].OnAnimationEnd(currentState);
        }
    }

    public void ServicesOnReStart()
    {
        for (int i = 0; i < service_count; i++)
        {
            fSMServices[i].ReStart(currentState);
        }
    }


    public void ServicesOnDisable()
    {
        for (int i = 0; i < service_count; i++)
        {
            fSMServices[i].OnDisable(currentState);
        }
    }

    internal void AddForce(Vector3 speed, bool ignore_gravity)
    {
        Move(speed, true, _add_gravity: ignore_gravity == false, _do_ground_check: !ignore_gravity);
    }

    internal int GetEnemyLayerMask()
    {
        if (AI)
        {
            return GameDefine.Player_LayerMask;
        }
        else
        {
            return GameDefine.Enemy_LayerMask;
        }
    }

    internal void RemoveForce()
    {

    }

    Dictionary<string, GameObject> hangPoint = new Dictionary<string, GameObject>();
    internal GameObject GetHangPoint(string o_id)
    {
        if (string.IsNullOrEmpty(o_id))
        {
            return _gameObject;
        }

        if (hangPoint.TryGetValue(o_id, out var x))
        {
            return x;
        }
        var go = _transform.Find(o_id);
        if (go != null){
            hangPoint[o_id] = go.gameObject;
            return go.gameObject;
        }
        else
        {
            hangPoint[o_id] = null;
            return null;
        }
    }
    EnemyHUD enemyHUD;
    internal void UpdateHP_OnHit(int damage)
    {
        //if (AI == false) {
        //    this.att_crn.hp -= 1;
        //}
        //else
        //{
        //    //this.att_crn.hp -= 1;
        //}

        this.att_crn.hp -= damage;

        if (this.att_crn.hp<0)
        {
            this.att_crn.hp = 0;
        }
        float v = this.att_crn.hp / this.att_base.hp;
        //Debug.LogError($"{this._gameObject.name}:剩余血量:{this.att_crn.hp}");
        if (AI)
        {
            //更新敌人血条 
            if (unitEntity.type==3)
            {
                //更新Boss的血条
                MainViewController.Instance.UpdateBossHP(v);
            }
            else
            {
                //更新小兵的血条
                UpdateEnemyHUD();
            }
        }
        else
        {
            //更新主角的血条
            MainViewController.Instance.UpdatePlayerHP(v);
        }
    }

    private void UpdateEnemyHUD()
    {
        if (AI)
        {
            if (unitEntity.type == 1 || unitEntity.type == 2 || unitEntity.type == 0)
            {
                if (enemyHUD == null)
                {
                    enemyHUD = ResourcesManager.Instance.CreateEnemyHUD();
                }
                enemyHUD.UpdateHP(att_crn.hp / att_base.hp, this._transform, unitEntity.info);
            }
           
        }
    }

    public FSM atk_target;//攻击方

    public void SetAtkTarget(FSM f) {
        this.atk_target = f;
    }

    //受击 0前方受击 1后方受击
    public void OnHit(int fd,FSM atk)
    {
        if (currentState.excel_config.on_hit!=null)
        {
            // ToNext(currentState.excel_config.on_hit[fd]);
            if (fd == 0)
            {
                ToNext(currentState.excel_config.on_hit[0]);
            }
            else if (fd == 1)
            {
                ToNext(currentState.excel_config.on_hit[1]);
            }

        }
    }

    public void OnDeath(int fd) {
        if (currentState.excel_config.on_death != null) {

            if (fd == 0)//前方
            {
                ToNext(currentState.excel_config.on_death[0]);
            }
            else if (fd == 1)//后方
            {
                ToNext(currentState.excel_config.on_death[1]);
            }
            characterController.enabled = false;
            //击杀boss 特殊逻辑处理

            //主角死亡  特殊的逻辑处理(游戏循环)
            if (AI)
            {
                UnitManager.Instance.RemoveNPC(this);

                if (unitEntity.drop!=null)
                {
                    foreach (var item in unitEntity.drop)
                    {
                        int count = UnityEngine.Random.Range(1, 11);
                        for (int i = 0; i < count; i++)
                        {
                            var pos = new Vector3(_transform.position.x + UnityEngine.Random.Range(-3,3.0f),_transform.position.y, _transform.position.z + UnityEngine.Random.Range(-3, 3.0f));
                            var drop=ResourcesManager.Instance.Instantiate<GameObject>($"drop/{item}");
                            drop.transform.position = pos;
                        }
                    }
                }

                if (scene_id!=0)
                {
                    MainViewController.Instance.OnLevelComplete(true, GameSystem.Instance.SceneController.LoadLastScene);
                    //退出场景
                }
            }
            else
            {
                if (string.IsNullOrEmpty(GameSystem.Instance.SceneController.now_scene_id)==false)
                {
                    //退出场景
                    //GameSystem.Instance.SceneController.LoadLastScene();
                    MainViewController.Instance.OnLevelComplete(false, GameSystem.Instance.SceneController.LoadLastScene);
                }
            }
        }
    }

    internal void Attack_Hitlag(PlayerState state)
    {
        //currentState ==> 技能的接口 传过来的 射弓箭-->5s命中敌人
        hitlagService.DOHitlag_OnAttack(animationService.normalizedTime, state);
    }

    //格挡成功的时候
    internal void OnBlockSucces(FSM atk)
    {
        this.atk_target = atk;
        UpdateEnemyHUD();
        if (currentState.excel_config.on_block_succes!=0)
        {
            ToNext(currentState.excel_config.on_block_succes);
        }

        var follow_target= GetHangPoint(anmConfig.block_camera_follow);
        if (follow_target!=null)
        {
            GameSystem.Instance.CameraController.Handle(CombatConfig.Instance.Config().block_camer_config, follow_target.transform);
        }
        //UnityEditor.EditorApplication.isPaused = true;
    }

    public void BeBlock(FSM player) {

        if (currentState.excel_config.be_block!=null)
        {
            if (_transform.ForwardOrBack(player._transform.position)>0)
            {
                ToNext(currentState.excel_config.be_block[0]);
            }
            else 
            {
                ToNext(currentState.excel_config.be_block[1]);
            }

        }
    }

    public float GetMoveSpeed()
    {
        return _speed;
    }

    Vector3 bash_add_fly;
    Vector3 bash_fly_dir;
    public void OnBash(int fb, FSM atk, float[] add_fly, Vector3 point)
    {
        atk_target = atk;
        bash_add_fly = new Vector3(add_fly[0], add_fly[1], add_fly[2]);
        bash_fly_dir = (this._transform.position - atk.transform.position).normalized;

        if (currentState.excel_config.on_bash!=null)
        {
            ToNext(currentState.excel_config.on_bash[fb]);
        }
    }

    public void OnBashUpdate() {
        //时长 0.2f (0.1)上升  (0.1)下降的过程
        var f = GameTime.time - currentState.begin;
        if (f <= 0.1f)
        {
            var d = bash_fly_dir * (bash_add_fly.z / 0.2f);
            d.y = bash_add_fly.y / 0.2f;
            Move(d, false, _add_gravity: false, _do_ground_check: false);
        }
        else if (f <= 0.2f) {
            var d = bash_fly_dir * (bash_add_fly.z / 0.2f);
            d.y = -(bash_add_fly.y / 0.2f * 2);
            Move(d, false, _add_gravity: false, _do_ground_check: false);
        }
    }
    //OnBashEnd
    //OnBashUpdate
    public void OnBashEnd() {
        ground_check = true;
    }

    int use_prop_id;
    public void UseProp() {
        if (CheckConfig(currentState.excel_config.on_use_prop))
        {
            if (DialogueViewController.Instance.IsOpen())
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ToNext(1046);
                On_Use_Prop(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ToNext(1047);
                On_Use_Prop(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ToNext(1048);
                On_Use_Prop(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ToNext(1049);
                On_Use_Prop(4);
            }
        } 
    }

    void On_Use_Prop(int grild_id) {
        var data = BagData.Instance.Get_Quick(grild_id);
        if (data != null && data.count > 0)
        {
            if (data.entity.chang_state != 0)
            {
                use_prop_id = data.id;
                ToNext(data.entity.chang_state);
            }
        }
    }

    public GameObject GetTrackTarget() {
        if (test_track_target!=null)
        {
            var f = test_track_target.GetComponent<FSM>();
            if (f!=null)
            {
                return f.hit_target;
            }
            return test_track_target;
        }
        return GetAtkTarget().hit_target;
    }

    internal GameObject GetAtkTarget(int spawn_point_type, string spawn_hang_point,bool autoLock=true)
    {
        if (spawn_point_type == 0)//0自身 1目标 挂点
        {
            return GetHangPoint(spawn_hang_point);
        }
        else if (spawn_point_type==1)
        {
            if (autoLock)
            {
                ForceLockPlayer();
            }

            if (atk_fsm != null)
            {
                return atk_fsm.GetHangPoint(spawn_hang_point);
            }
            else
            {
                return null;
            }
        }
        return null;
    }

    internal bool IsBlockState()
    {
        return currentState != null && (currentState.id == 1013 || currentState.id == 10131);
    }

    internal Vector3 GetBlockEffectPoint()
    {
        if (string.IsNullOrEmpty(anmConfig.block_effect_point)) {
            return _transform.position + _transform.forward * 0.8f + _transform.up * 1.5f;
        }

        var t = GetHangPoint(anmConfig.block_effect_point);
        if (t != null) {
            return t.transform.position;
        }
        else
        {
            return  _transform.position + _transform.forward * 0.8f + _transform.up * 1.5f;
        }
        
    }

    Vector3 to_fight_pos;
    Vector3 to_fight_rot;
    internal void ToFightState()
    {
        global_state = 0;
        to_fight_pos = this._transform.position;
        to_fight_rot = this.transform.eulerAngles;
        EnableBossHp();//和平空手待机->拔刀
    }

    internal void SetPosition(Transform t)
    {
        _transform.position = t.position;
        _transform.forward=t.forward;
        Physics.SyncTransforms();
    }

    public void Recover(SavePlayerInfo savePlayerInfo)
    {
        Init();
        _transform.position = new Vector3(savePlayerInfo.Pos[0], savePlayerInfo.Pos[1],
            savePlayerInfo.Pos[2]);
        _transform.eulerAngles = new Vector3(savePlayerInfo.Rot[0], savePlayerInfo.Rot[1],
            savePlayerInfo.Rot[2]);
        Physics.SyncTransforms();
        att_crn.hp = savePlayerInfo.Hp;
        this._gameObject.SetActive(savePlayerInfo.Active);
        ToNext(savePlayerInfo.State);
    }
}

public class PlayerState
{
    public int id;
    public PlayerStateEntity excel_config;//alt + enter
    //动作通知事件
    public StateEntity stateEntity;
    public float clipLength;
    public SkillEntity skill;
    public float begin;//进入状态的开始时间

    public void SetBeginTime() {
        begin = Time.time;
    }

    public bool IsCD() { 
    
        if (skill != null) { return false; }
        return Time.time- begin <skill.cd;
    }

    public string Info() {
        return $"状态:{id}_{excel_config.info}";
    }

}

public enum StateEventType
{ 
    begin,//开始进入
    update,//每帧更新
    end,//状态退出
    onAnmEnd,//当动作结束的时候
}