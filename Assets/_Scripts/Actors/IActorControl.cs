
// abstract: 직계 상속
// 변수 선언 가능

// interface: 먼 친척 관계
// 변수 선언 불가능 -> get; set; 형태로 자손에 전달 -> 자손은 반드시 정의

public interface IActorControl
{
    public ActorProfile Profile {get; set;}
}
