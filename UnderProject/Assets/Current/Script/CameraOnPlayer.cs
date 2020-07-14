using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnPlayer : MonoBehaviour
{
    //References
    private Camera cam;
    public GameObject player;

    //Controllers
    public float xAxisVelocity;
    public float yAxisVelocity;

    //Camera Enclosures
    [Range(0.0f,8.0f)]
    public float sideEnclosure;
    [Range(0.0f,50.0f)]
    public float heightEnclosure;

    //Player Enclosures
    private float deviationY;
    public float playerFreeAreaDeviationY;
    [Range(0.0f,5.0f)]
    public float playerFreeAreaX;
    [SerializeField]

    //Spacial Variables
    public float floor;
    public float floorDistance;

    public bool downLook;

    public Vector3 lastPosition;

    //Booleans
    public bool noBuilding;

    public BuildingNew building;

    //-----------------------------------------------------------------------------
    //This solution only works if all floors are equally distant from each another.
    //-----------------------------------------------------------------------------

    void Start()
    {
        cam = GetComponent<Camera>();
        // player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        floor = building.currentRoom;

        //Get Camera and Player positions
        Vector3 temp_cPos = cam.transform.position;
        Vector3 temp_pPos = player.transform.position;

        //Split height enclosure into top and bottom points based on a given floor (floor 0 == v(0,0,0))
        float temp_EnclosureTop = heightEnclosure - (floor * floorDistance);
        float temp_EnclosureBottom = -heightEnclosure - (floor * floorDistance);

        //Get camera range clamped two times:

        //first - Camera only returns it's real value if player is (< given value || > given value) (+ smooth)
        float temp_firstClampX = InvDepClamp(temp_cPos.x,playerFreeAreaX,playerFreeAreaX,temp_pPos.x);
        
        if(downLook)
        {
            deviationY = temp_pPos.y - playerFreeAreaDeviationY;
        }
        else
        {
            deviationY = temp_pPos.y + playerFreeAreaDeviationY;
        }

        //second - Camera only returns it's real value if camera is (> given value && < given value)
        float temp_secondClampX = Mathf.Clamp(temp_firstClampX,-sideEnclosure,sideEnclosure);
        float temp_secondClampY = Mathf.Clamp(deviationY,temp_EnclosureBottom,temp_EnclosureTop);

        float temp_goesToX;
        float temp_goesToY;

        if(noBuilding)
        {
            temp_goesToX = temp_firstClampX;
            temp_goesToY = deviationY;
        }
        else
        {
            temp_goesToX = temp_secondClampX;
            temp_goesToY = temp_secondClampY;
        }

        float temp_smoothFixX = Mathf.MoveTowards(temp_cPos.x,temp_goesToX,(xAxisVelocity * Time.deltaTime));
        float temp_smoothFixY = Mathf.MoveTowards(temp_cPos.y,temp_goesToY,(yAxisVelocity * Time.deltaTime));

        cam.transform.position = new Vector3(temp_smoothFixX,temp_smoothFixY,temp_cPos.z);
    }

    public bool IsMoving()
    {
        if(transform.position != lastPosition)
        {
            lastPosition = transform.position;
            return true;
        }

        return false;
    }

    float InvDepClamp(float origin,float point1,float point2,float value) //Like clamp but inverse and var dependent
    {
        if( value < (origin - point1) || value > (origin + point2) )
        {
            origin = value;
        }

        return origin;
    }

    void OnDrawGizmosSelected()
    {
        cam = GetComponent<Camera>();
        Vector3 camPos = cam.transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(0,-floorDistance * floor,0),new Vector3(1,1,0));

        Gizmos.color = Color.white;
        Gizmos.DrawCube(new Vector3(sideEnclosure,camPos.y,camPos.z),new Vector3(1,1,0));
        Gizmos.DrawCube(new Vector3(-sideEnclosure,camPos.y,camPos.z),new Vector3(1,1,0));
        
        Gizmos.color = Color.white;
        Gizmos.DrawCube(new Vector3(camPos.x,heightEnclosure,camPos.z),new Vector3(1,1,0));
        Gizmos.DrawCube(new Vector3(camPos.x,-heightEnclosure,camPos.z),new Vector3(1,1,0));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(camPos.x + playerFreeAreaX,camPos.y,camPos.z),new Vector3(.5f,.5f,0));
        Gizmos.DrawCube(new Vector3(camPos.x - playerFreeAreaX,camPos.y,camPos.z),new Vector3(.5f,.5f,0));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(camPos.x,deviationY,camPos.z),new Vector3(.5f,.5f,0));
    }
}