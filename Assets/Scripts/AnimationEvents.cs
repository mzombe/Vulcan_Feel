using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
   private GameObject obj;

   private void Start() {
        obj = this.gameObject;
   }

   public void TurnFalseAnimation(){
        obj.GetComponent<Animator>().SetBool("Active", false);
   }
}
