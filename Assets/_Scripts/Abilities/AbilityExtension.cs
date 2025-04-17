// 반복 , 자주 , 편리성

using UnityEngine.Events;

public static class AbilityExtension
{
    // 해당 Ability 갖고있나 ?
    public static bool Has(this ref AbilityFlag abilities, AbilityFlag a)
    {
        return (abilities & a) == a;
    }

    // 해당 Ability 를 추가
    public static void Add(this ref AbilityFlag abilities, AbilityFlag a, UnityAction oncomplete)
    {
        abilities |= a;

        oncomplete?.Invoke();
    }

    // 해당 Ability 를 삭제
    public static void Remove(this ref AbilityFlag abilities, AbilityFlag a, UnityAction oncomplete)
    {
        abilities &= ~a;

        oncomplete?.Invoke();
    }

    // 해당 Ability 를 사용 -> 액션 발동
    public static void Use(this ref AbilityFlag abilities, AbilityFlag a, UnityAction oncomplete)
    {
        if (abilities.Has(a))
            oncomplete?.Invoke();
    }
}
