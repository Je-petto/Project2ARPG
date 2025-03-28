using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using System.Linq;
using UnityEngine.InputSystem;

// abilityDatas : 외부에서 능력 부여/회수 인터페이스
// abilities : abilityDatas 갱신해서 행동
public class AbilityControl : MonoBehaviour
{
    [Space(10), Title("ABILITY SYSTEM", underlined:true, fontSize = 15, alignment = TextAlignment.Center), HideField] public bool _t0;

    [Space(10), ReadOnly] public AbilityFlag flags = AbilityFlag.None;

    // 보유(잠재된) 중인 능력들(Abilities)
    [Space(10), SerializeField] List<AbilityData> datas = new List<AbilityData>();

    // <Key 값, Value 값>
    // 사용할 수 있는 능력
    private readonly Dictionary<AbilityFlag, Ability> actives = new Dictionary<AbilityFlag, Ability>();

    //활성화된 능력만 Update
    private void Update()
    {
        foreach( var a in actives.ToList())
            a.Value?.Update();
    }

    private void FixedUpdate()
    {
        foreach( var a in actives.ToList())
            a.Value?.FixedUpdate();
    }

    // 잠재능력을 추가 -> 발동: Activate (X)
    public void Add(AbilityData d, bool immediate = false)
    {
        if (datas.Contains(d) == true || d == null)
            return;
        
        flags.Add(d.Flag, null);
        datas.Add(d);
        var ability = d.CreateAbility(GetComponent<CharacterControl>());

        if( immediate )
            actives[d.Flag] = ability;
    }

    // 잠재능력을 제거, 발동: Activate (X)
    public void Remove(AbilityData d)
    {
        if (datas.Contains(d) == false || d == null)
            return;
        
        actives[d.Flag].Deactivate();
        
        datas.Remove(d);
        flags.Remove(d.Flag, null);
        actives.Remove(d.Flag);
    }

    // 잠재 능력 활성화 및 업데이트 추가
    public void Activate(AbilityFlag flag, InputAction.CallbackContext ctx)
    {
        foreach( var d in datas)
        {
            if ((d.Flag & flag) == flag)
            {
                if (actives.ContainsKey(flag) == false)
                    actives[flag] = d.CreateAbility(GetComponent<CharacterControl>());

                actives[flag].Activate(ctx);
                
            }
        }
    }

    // 현재 활성 능력 비활성화 및 업데이트 제거
    public void Deactivate(AbilityFlag flag)
    {
        foreach( var d in datas)
        {
            if ((d.Flag & flag) == flag)
            {
                if (actives.ContainsKey(flag) == true)
                {
                    actives[flag].Deactivate();
                    actives[flag] = null;
                    actives.Remove(flag);
                }

            }
        }
    }


}
