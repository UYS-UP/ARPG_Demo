using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UCameraController : MonoBehaviour
{
    public Transform target;

    //������� �����ת
    //������ ������ɫԶ��

    CharacterController controller;
    public Vector3 offset=new Vector3(0,1.575f,0);
    Vector3 offset_org;
    Vector3 offset_crn;




    float xMouse;
    float yMouse;

    public float distanceFromTarget = 3f;
     float distanceFromTarget_org = 3f;
    public float mouse_scrollwheel_scale = 10;//�������ٶȵĵ���(����)
    public float speed = 5;//�����ٶ�
    float speed_org;
    public Camera _camera;
    private void Awake()
    {
        _camera=this.GetComponent<Camera>();
        _camera.farClipPlane = 3000;
        speed_org = speed;
        offset_org = new Vector3(offset.x, offset.y, offset.z);
        distanceFromTarget_org = distanceFromTarget;
    }

    void Start()
    {
        if (target != null) {

                 ChangeCursor(false);        
               controller = target.GetComponent<CharacterController>();
            //offset=controller.center * 1.75f;
        }
    }

    public void ChangeCursor(bool enable) {
        if (enable == true) {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;//������� ���ɼ�
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;//������� ���ɼ�
        }
       
    }

    public int state;//0���ڿ��е�״̬ 1����״̬ 2��ʱ��ͷЧ�� 3ע�ӹҵ�

    CameraConfig cameraConfig;
    Vector3 cmd_pos;
    Coroutine block_coroutine;
    public void Handle(CameraConfig cameraConfig,Transform follow) {

        this.cameraConfig= cameraConfig;
        this.speed = cameraConfig.speed;
        cmd_pos = target.position;//follow.position;
        state = 2;
        block_coroutine = StartCoroutine(DoEffect());
    }

    IEnumerator DoEffect() {
        float begin = GameTime.time;
        float _y_mouse = yMouse;
        while (GameTime.time - begin <= 0.03f)
        {
            var lerp = (GameTime.time - begin) / 0.03f;
            yMouse = Mathf.Lerp(_y_mouse, cameraConfig.y_mouse,lerp);
            distanceFromTarget = Mathf.Lerp(distanceFromTarget_org, cameraConfig.dst, lerp);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(cameraConfig.time);

        this.speed = speed_org;
        target = UnitManager.Instance.player._transform;//���¸�������

        float reset_begin = GameTime.time;
        while (GameTime.time- reset_begin<=0.02f)
        {
            var lerp = (GameTime.time - reset_begin) / 0.02f;
            //offset = Vector3.Lerp(cameraConfig.offset, offset_org,lerp);
            distanceFromTarget = Mathf.Lerp( cameraConfig.dst, distanceFromTarget_org, lerp);
            yield return new WaitForEndOfFrame();
        }
        offset =new Vector3(offset_org.x,offset_org.y,offset_org.z);
        state = 1;
    }
    
    float execute_begin;
    public void OnExecute(CameraConfig cameraConfig, Transform follow) {
        if (block_coroutine != null) {
            StopCoroutine(block_coroutine);
            block_coroutine = null;
        }
        target = follow;
        this.cameraConfig = cameraConfig;
        execute_begin = GameTime.time;
        state = 3;
    }

   
    public void SetTarget(Transform target) { 
        this.target = target;
        if (target != null)
        {

            Cursor.lockState = CursorLockMode.Locked; ;
            Cursor.visible = false;//������� ���ɼ�
            controller = target.GetComponent<CharacterController>();
            //offset = controller.center * 1.75f;
        }
        state = 1;
        Follow(false);  
        this.gameObject.SetActive(true);
        //Follow()
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Follow();
    }

    private void Follow(bool lerp=true)
    {
        if ((state ==1) && target != null)
        {
            //��껬�� �����ֵ
            if (state==1)
            {
                xMouse += UInput.GetAxis_Mouse_X();
                yMouse -= UInput.GetAxis_Mouse_Y();
            }
            yMouse = Mathf.Clamp(yMouse, -30f, 80f);
            //�����ֵ����� ��ǰ�������� ���󻬶��为����
            //���ɫԽ��(��ǰ����) 
            //distanceFromTarget -= UInput.GetAxis_Mouse_ScrollWheell() * mouse_scrollwheel_scale; //����������Զ ���ﾵͷ

            distanceFromTarget = Mathf.Clamp(distanceFromTarget, 2, 15);

            Quaternion targetRotation = Quaternion.Euler(yMouse, xMouse, 0);

            Vector3 targetPosition = target.position + targetRotation * new Vector3(0, 0, -distanceFromTarget) + offset;

            
            speed = controller.velocity.magnitude > 0.1f ? Mathf.Lerp(speed, 7.5f, 5f * GameTime.deltaTime)
                              : Mathf.Lerp(speed, 25f, 5f * GameTime.deltaTime);
           
          

            if (lerp==false)
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, GameTime.deltaTime * speed);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, GameTime.deltaTime * 25f);
            }
   
        }
        else if (state==2)
        {
            //cmd_pos
            yMouse = Mathf.Clamp(yMouse, -30f, 80f);
            //�����ֵ����� ��ǰ�������� ���󻬶��为����
            //���ɫԽ��(��ǰ����) 
            //distanceFromTarget -= UInput.GetAxis_Mouse_ScrollWheell() * mouse_scrollwheel_scale; //����������Զ ���ﾵͷ

            distanceFromTarget = Mathf.Clamp(distanceFromTarget, 2, 15);

            Quaternion targetRotation = Quaternion.Euler(yMouse, xMouse, 0);

            Vector3 targetPosition = cmd_pos + targetRotation * new Vector3(0, 0, -distanceFromTarget) + offset;

            transform.position = Vector3.Lerp(transform.position, targetPosition, GameTime.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, GameTime.deltaTime * 25f);
        }
        else if (state==3)
        {
            if (GameTime.time- execute_begin<this.cameraConfig.time)
            {
                var pos = target.position + this.cameraConfig.offset;
                transform.position = Vector3.Lerp(transform.position, pos, speed * GameTime.deltaTime);
                transform.LookAt(target);
            }
            else
            {
                target = UnitManager.Instance.player._transform;
                state = 1;
                speed = speed_org;
            }
        }
    }
}
