using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using System.Linq;



// abilityDatas : 외부에서 능력 부여/회수 인터페이스
// abilities : abilityDatas 갱신해서 행동
public class AbilityControl : MonoBehaviour
{


    [HorizontalLine("CURRENT ABILITIES"),HideField] public bool _h1;

    [ReadOnly] public AbilityFlag flags = AbilityFlag.None;
    
    // 보유(잠재된)중인 능력들(Abilities)
    [SerializeField] List<AbilityData> datas = new List<AbilityData>();
    
    // Pair <Key , Value>
    // 사용할수 있는 능력
    private readonly Dictionary<AbilityFlag,Ability> actives = new Dictionary<AbilityFlag, Ability>();





    // 활성화된 능력만 Update
    private void Update()
    {
        foreach(var a in actives.ToList())
            a.Value?.Update();
    }

    private void FixedUpdate()
    {
        foreach(var a in actives.ToList())
            a.Value?.FixedUpdate();
    }


    // 잠재능력 추가 -> 발동:Activate (x)
    public void Add(AbilityData d, bool immediate = false)
    {
        if (datas.Contains(d) == true || d == null)
            return;

        flags.Add(d.Flag, null);
        
        datas.Add(d);
        var ability = d.CreateAbility(GetComponent<CharacterControl>());

        if (immediate)
        {
            actives[d.Flag] = ability;
            ability.Activate(null);
        }
    }

    // 잠재능력 제거
    public void Remove(AbilityData d)
    {
        if (datas.Contains(d) == false || d == null)
            return;
        
        datas.Remove(d);
        flags.Remove(d.Flag, null);
        actives.Remove(d.Flag);
    }


    // 잠재 능력 활성화 및 업데이트 추가
    // forceDeactivate : True -> 모든 능력 제거 후 flag 해당 능력만 활성화
    public void Activate(AbilityFlag flag, bool forceDeactivate, object obj)
    {
        if(forceDeactivate)
            DeactivateAll();
        
        foreach( var d in datas )
        {
            if ((d.Flag & flag) == flag)
            {
                if (actives.ContainsKey(flag) == false)
                    actives[flag] = d.CreateAbility(GetComponent<CharacterControl>());

                actives[flag].Activate(obj);
            }
        }        
    }

    // 활성 능력 비활성화 및 업데이트 제거
    public void Deactivate(AbilityFlag flag)
    {        
        foreach( var d in datas )
        {
            if ((d.Flag & flag) == flag)               
            {
                if (actives.ContainsKey(flag) == true)
                {
                    flags.Remove(flag, null);
                    actives[flag].Deactivate();
                    actives[flag] = null;
                    actives.Remove(flag);
                }
            }
        }
    }
    
    public void DeactivateAll()
    {
        foreach( var a in actives )
            a.Value.Deactivate();


        actives.Clear();
    }
}
