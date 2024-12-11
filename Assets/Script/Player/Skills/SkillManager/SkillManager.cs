
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dashSkill { get; private set;}
    public CloneSkill cloneSkill { get; private set;}
    
    public GrenadeSkill grenadeSkill { get; private set;}
    
    public BlackholeSkill blackholeSkill { get; private set;}
    
    public CrystalSkill crystalSkill { get; private set;}
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        grenadeSkill = GetComponent<GrenadeSkill>();
        blackholeSkill = GetComponent<BlackholeSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
    }
}
