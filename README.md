황금사냥꾼 (Golden Hunter)

Unity6 2D 플랫폼 액션 포트폴리오

개발기간: 2026.07.07 ~ 2026.08.07 (1개월)
개발환경: Unity 6.3, C#, URP
개발자: 김정현 (kun93686@gmail.com)
소개

몬스터를 사냥해 얻은 골드로 능력치를 랜덤 성장시켜 최종 보스를 처치하는 룸 돌파형 액션 게임입니다. <던전 슬래셔> 특유의 플랫포머 손맛과 액션감을 직접 구현하는 것을 목표로 제작했습니다.

마을 → 던전 진입(F) → 룸 돌파(몬스터 처치·골드 루팅) → 보스방 → 마을 귀환으로 이어지는 게임 루프
재화를 단일화한 '획득 = 즉시 성장' 보상 루프가 핵심 재미 요소
핵심 시스템
Persistent Scene 구조: Additive 씬 로딩 + 상주 매니저(PersistentManager, UIManager)로 씬 전환 시 데이터 연속성 확보
디자인 패턴: FSM(플레이어/몬스터/보스), Strategy(보스 패턴), Observer(IDamageable), Mediator(UIManager)
데이터 관리: ScriptableObject로 캐릭터/몬스터 스탯 분리, Dictionary 기반 실시간 스탯 산출
비동기 처리: UniTask + GetCancellationTokenOnDestroy로 씬 전환 시 작업 자동 취소
폴더 구조
Assets/
├─ Scripts/
│  ├─ Character/  # Player, Enemy, Boss (FSM, Pattern)
│  ├─ Core/       # Damage, Gold, Stat
│  ├─ Managers/   # Persistent, UI, Stage, Camera
│  └─ UIs/
├─ SO/            # ScriptableObject 데이터
└─ Animation/
Contact
Email: kun93686@gmail.com
Github: (링크 추가 예정)