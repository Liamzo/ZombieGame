using UnityEngine;
using System.Collections;

public class EnemyScript : Damagable {

    // For health
    float maxHealth = 100f;
    float curHealth;

    // For chasing the player
    public GameObject target;
    Transform targetTransform;
    NavMeshAgent agent;

    // For choosing whether to chase or attack
    public enum Action {
        CHASING,
        ATTACKING
    };
    public Action curAction;

    // For attacking
    float attackRange = 3.5f;
    float damageRange = 4.5f;
    float attackTime = 0.5f;
    float curAttackTime;

    float attackCD = 0.5f;
    float curAttackCD;

    // For Animation
    Animator animator;

    // Use this for initialization
    void Start() {
        curHealth = maxHealth;

        agent = GetComponent<NavMeshAgent>();

        curAction = Action.CHASING;

        curAttackTime = attackTime;

        curAttackCD = 0;

        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        curAttackCD -= Time.deltaTime;

        Vector3 dir = targetTransform.position - this.transform.position;

        if (target.tag == "Barricade" && dir.magnitude < attackRange + 0.5f) {
            BarricadeScript bs = (BarricadeScript)target.GetComponent("BarricadeScript");
            if (bs.getCurHealth() <= 0) {
                target = GameObject.FindGameObjectWithTag("MainGuy");
                targetTransform = target.transform;
            }
        }

        if (curAction == Action.CHASING) {
            if (dir.magnitude <= attackRange && curAttackCD <= 0) {
                curAction = Action.ATTACKING;
                animator.SetInteger("curAction", 1);
                agent.Stop();
                attack(dir);
            } else {
                agent.destination = targetTransform.position;
                agent.Resume();
            }
        } else if (curAction == Action.ATTACKING) {
            attack(dir);
        }
    }

    void attack (Vector3 dir) {
        Damagable ts = (Damagable)target.GetComponent<Damagable>();

        curAttackTime -= Time.deltaTime;

        if (curAttackTime <= 0) {
            curAttackCD = attackCD;
            curAttackTime = attackTime;
            if (dir.magnitude <= damageRange) {
                ts.takeDamage(60f);
            }
            curAction = Action.CHASING;
            animator.SetInteger("curAction", 0);
        }
    }

    public void setTarget(GameObject target) {
        this.target = target;
        targetTransform = target.transform;
    }

    override
    public void takeDamage (float damage) {
        curHealth -= damage;
        UIScript ui = (UIScript)GameObject.FindGameObjectWithTag("Manager").GetComponent("UIScript");
        ui.addScore(10);

        if (curHealth <= 0) {
            ui.addScore(60);
            ui.killedZom();
            GameObject.FindObjectOfType<SpawnManager>().killedZom();
            Destroy(this.gameObject);
        }
    }
}
