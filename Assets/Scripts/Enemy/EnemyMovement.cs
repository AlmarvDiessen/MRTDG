using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] private GameObject target;
    private float movementSpeed;

    public GameObject Target { get => target; set => target = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

    private void Awake() {
    }

    private void OnCollisionEnter(Collision collision) {

    }
    private void Update() {
        if(target != null) {
            //gameObject.transform.LookAt(target.transform);
            Vector3 dir = (target.transform.position - transform.position).normalized;
            transform.position -= movementSpeed * Time.deltaTime * transform.forward;
            //transform.position += dir * movementSpeed * Time.deltaTime;
        }
    }

    public void SetMovement(GameObject target, float speed) {
        Target = target;
        movementSpeed = speed;
    }

    private Vector3 Direction() {
        Vector3 direction = new Vector3();

        //switch based on if the t

        return direction;
    }
}
