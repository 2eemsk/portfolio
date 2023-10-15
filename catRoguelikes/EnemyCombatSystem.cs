using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatSystem : MonoBehaviour
{
    [SerializeField] private float HP = 30f;
    private Rigidbody _rigid;
    private Material _mat;
    private Animator _anim;
 
    public void Damage(float _damage, Vector3 _reactDir)
    {
        HP -= _damage;

        if (HP > 0)
        {
            StartCoroutine(ChangeColor());
        }
        else
        {
            _mat.color = Color.red;
            _reactDir = _reactDir.normalized;
            _reactDir = new Vector3(_reactDir.x, 1f, _reactDir.z);
            _rigid.AddForce(_reactDir * 5f, ForceMode.Impulse);
            StartCoroutine(EnemyDeath());
        }
    }

    IEnumerator ChangeColor()
    {
        _mat.color = Color.red;
        _anim.SetBool("isHit", true);
        _anim.SetFloat("hit", Random.Range(0, 2));
        yield return new WaitForSeconds(1f);
        _anim.SetBool("isHit", false);
        _mat.color = Color.white;
    }

    IEnumerator EnemyDeath()
    {
        _anim.SetBool("death", true);
        print("Death");

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _mat = GetComponent<MeshRenderer>().material;
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //_rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            var _reactDir = transform.position - other.transform.position;
            Damage(10f,_reactDir);
        }
        else if (other.gameObject.layer == 12)
        {
            print("Parrying");
            _anim.SetTrigger("Parried");
            _anim.SetBool("Stun", true);
        }
    }
}
