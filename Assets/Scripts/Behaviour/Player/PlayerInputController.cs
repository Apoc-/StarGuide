using Behaviour.World;
using UnityEngine;

namespace Behaviour
{
    public class PlayerInputController : MonoBehaviour
    {
        private void Update()
        {
            if(Input.GetButtonDown("Fire1"))
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.5f);

                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    
                    var ia = hit.collider.gameObject.GetComponent<Interactible>();
                    if (ia != null)
                    {
                           
                    }    
                }
            }
        }
    }
}