using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomInspector;


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

        Deactivate(d.Flag);
        
        datas.Remove(d);
        flags.Remove(d.Flag, null);
        actives.Remove(d.Flag);
    }

    // 모든 잠재능력 제거
    public void RemoveAll()
    {
        DeactivateAll();

        flags = AbilityFlag.None;
        actives.Clear();
        datas.Clear();
    }


    // 잠재 능력 활성화 및 업데이트 추가
    // forceDeactivate : TRUE -> 모든 능력 제거 후 flag 해당 능력만 활성화
    public void Activate(AbilityFlag flag, bool forceDeactivate, object obj)
    {
        if (forceDeactivate)
            DeactivateAll();

        // 실시간 삭제된 데이터를 반영하기위해 , 임시변수에 담아놓는다.
        List<AbilityData> temp = new List<AbilityData>();
        temp.AddRange(datas.GetRange(0,datas.Count));

        foreach( var d in temp )
        {
            if ((d.Flag & flag) == flag)
            {
                if (actives.ContainsKey(flag) == false)
                    actives[flag] = d.CreateAbility(GetComponent<CharacterControl>());

                actives[flag].Activate(obj);
            }
        }        
    }

    // 능력 비활성화
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

    // 모든 능력 비활성화
    public void DeactivateAll()
    {
        foreach( var a in actives )
            a.Value.Deactivate();

        actives.Clear();
    }
    
}
