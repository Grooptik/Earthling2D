using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iframes")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberofFlashes;
    private SpriteRenderer spriteRend;


    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            //iframes
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");

                //player
                if(GetComponent<PlayerMovement>() != null)
                {
                GetComponent<PlayerMovement>().enabled = false;
                //Switch to Game Over screen
                SceneManager.LoadScene("GameOver");
                }

                //Enemy
                if(GetComponentInParent<EnemyPatrol>() != null)
                {
                    GetComponentInParent<EnemyPatrol>().enabled = false;
                }

                if(GetComponent<MeleeEnemy>() != null)
                {
                    GetComponent<MeleeEnemy>().enabled = false;
                    Destroy(gameObject,1f);
                }

                if (GetComponent<EnemyBase>() != null)
                {
                    GetComponent<EnemyBase>().enabled = false;
                    Destroy(gameObject);
                }
               
                dead = true;
            }
        
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
  private IEnumerator Invunerability()
  {
      Physics2D.IgnoreLayerCollision(10, 11, true);
      //invulnerablitiy duration
      for(int i = 0; i < numberofFlashes; i++)
      {
          spriteRend.color = new Color(1, 0, 0, 0.5f);
          yield return new WaitForSeconds(iFramesDuration / (numberofFlashes * 2));
          spriteRend.color = Color.white;
          yield return new WaitForSeconds(iFramesDuration / (numberofFlashes * 2));

      }
  }

}
