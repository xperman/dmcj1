using UnityEngine;
/*************************
 * 这个脚本用来控制本地玩家眼里
 * 的动画
 * ***********************/
public class AnimatorPlayerLocal : MonoBehaviour
{
    private Animator anmLocal;
    void Start()
    {
        anmLocal = this.GetComponent<Animator>();
    }

    public void RunAnimator(float velocity)
    {
        anmLocal.SetFloat("Run", velocity);
    }

    public void ShootAnimator()
    {

    }

    public void ReloadAnimator()
    {

    }
    void Update()
    {
        
    }
}
