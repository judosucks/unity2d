using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    [Header("major stats")] 
    public Stat strength;// 1 point increase damage by 1 point and crit damage by 1 point
    public Stat agility;// 1 point increase speed by 1% and crit chance by 1%
    public Stat intelligence;// 1 point increase magic damage by 1 point and magic resistance by 3
    public Stat vitality; //1point increase health by 3 or 5 points
    [Header("deafensive stats")] 
    public Stat evasion;
    public Stat armor;
    public Stat maxHealth;
    public Stat magicResistance;
    [Header("offensive stats")]
    public Stat critChance;
    public Stat damage;
    public Stat critPower; //default 150
    
    [Header("magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited; //does damage over time
    public bool isChilled; //reduce armor by 20%
    public bool isShocked; //reduce accuracy by 20%
    private float ignitedTimer;
    private float chillTimer;
    private float shockTimer;
    
    private float ignitedDamageCooldown = .3f;
    private float ignitedDamageTimer;
    private int iginitedDamage;
    public int currentHealth;

    public System.Action OnHealthChanged;
    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue();
        critPower.SetDefaultValue(150);
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;
        chillTimer -= Time.deltaTime;
        if (ignitedTimer < 0)
        {
            
            isIgnited = false;
        }

        if (chillTimer < 0)
        {
            
            isChilled = false;
        }

        if (shockTimer < 0)
        {
            
            isShocked = false;
        }
        if (ignitedDamageTimer < 0 && isIgnited)
        {
            Debug.Log("burn damage"+" "+iginitedDamage);
            currentHealth -= iginitedDamage;
            if (currentHealth < 0)
            {
                Die();
            }
            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (CanAviodAttack(_targetStats))
        {
            return;
        }
        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
           
        }
        
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicDamage(_targetStats);
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        
        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }
        
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            Debug.Log("No ailments to apply");
            if (Random.value < 0.3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Fire damage");
                return;
            }
            if(Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Ice damage");
                return;
            }

            if (Random.value < 0.5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Lightning damage");
                return;
            }
        }

        if (canApplyIgnite)
        {
            Debug.Log("can apply ignite");
            _targetStats.SetupIgnitedDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }
        
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        
    }

    private static int CheckTargetResistance(CharacterStats _targetStats,int totalMagicDamage)
    {
        totalMagicDamage = _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilments(bool _isIgnited, bool _isChilled, bool _isShocked)
    {
        if (isIgnited || isChilled || isShocked)
        {
            Debug.Log("Ailments already applied");
            return;
        }

        if (_isIgnited)
        {
            Debug.Log("Ignited");
          isIgnited = _isIgnited;
          ignitedTimer = 2;
        }

        if (_isChilled)
        {
            Debug.Log("Chilled");
           isChilled = _isChilled;
           chillTimer = 2; 
        }

        if (_isShocked)
        {
            Debug.Log("Shocked");
            isShocked = _isShocked;
            shockTimer = 2;
        }
        
    }
    public void SetupIgnitedDamage(int _damage)=> iginitedDamage = _damage;
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        
        
        float critDamage = _damage * totalCritPower;
        
        
        return Mathf.RoundToInt(critDamage);
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage)
    {
        if (_targetStats.isChilled)
        {
            Debug.Log("Chilled - 20% armor");
            _totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            _totalDamage -= _targetStats.armor.GetValue();
        }
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);
        return _totalDamage;
    }
    private bool CanAviodAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            Debug.Log("Shocked + 20% evasion");
            totalEvasion += 20;
        }
        if (Random.Range(0, 100) < totalEvasion)
        {
            
            return true;
        }
        return false;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);
        if (currentHealth < 0)
        {
            Die();
        }
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    protected virtual void Die()
    {
        Debug.Log("Dead");
    }
}
