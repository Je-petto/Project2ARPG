using UnityEngine;
using DungeonArchitect;
using Unity.AI.Navigation;

public class EventDungeonBuilt : DungeonEventListener
{
    public bool IsAuto = false;
    public NavMeshSurface nvsurface;

    private void OnValidate()
    {
        if (IsAuto == false)
            return;

        if (nvsurface == null)
            nvsurface = FindFirstObjectByType<NavMeshSurface>();

        if (nvsurface == null)
            nvsurface = new GameObject("NavMeshSurface").AddComponent<NavMeshSurface>();
    }

    public override void OnPostDungeonBuild(Dungeon dungeon, DungeonModel model)
    {
        if (IsAuto == false)
            return;

        base.OnPostDungeonBuild(dungeon, model);

        // 던전 생성 후 길찾기 알고리즘 자동 실행
        nvsurface?.BuildNavMesh();
    }
}
