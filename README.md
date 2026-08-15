# 황금사냥꾼 (Golden Hunter)

> Unity 6 기반 2D Pixel Art 플랫폼 액션 포트폴리오

## 🎮 소개

몬스터를 사냥해 얻은 골드로 능력치를 랜덤 성장시켜 최종 보스를 처치하는 룸 돌파형 액션 게임입니다.

- 마을 → 던전 진입(F) → 룸 돌파(몬스터 처치 · 골드 루팅) → 보스방 → 마을 귀환으로 이어지는 게임 루프
- 재화를 단일화한 **'획득 = 즉시 성장'** 보상 루프를 핵심 재미 요소로 구성
- 플레이어 / 몬스터 / 보스의 전투 및 상태 관리
- 게임 오버 및 재시작 처리
- 씬 전환 및 게임 상태 관리

---

## 🛠️ 개발 정보

| 항목 | 내용 |
|---|---|
| 개발 기간 | 2026.07.07 ~ 2026.08.07 |
| 개발 인원 | 1인 |
| 개발 환경 | Unity 6.3 |
| 언어 | C# |
| Render Pipeline | URP |
| 비동기 처리 | UniTask |
| 버전 관리 | Git / Git LFS |

---

## 🏗️ 핵심 시스템

### Persistent Scene

`Additive Scene Loading`을 활용하여 게임 씬과 Persistent Scene을 분리했습니다.

씬 전환에도 지속적으로 유지되어야 하는 객체와 매니저를 Persistent Scene에서 관리하여 중복 생성과 데이터 단절을 방지했습니다.

~~~text
Persistent Scene
├─ PersistentManager
├─ UIManager
├─ Player
├─ Camera
└─ EventSystem

        ↓ Additive Load

Game Scene
├─ Stage
├─ Enemy
├─ Boss
└─ Portal
~~~

### FSM (Finite State Machine)

플레이어, 몬스터, 보스의 행동을 State 단위로 분리하여 상태별 책임을 명확하게 구성했습니다.

~~~text
Enemy
├─ Idle
├─ Walk
├─ Attack
├─ Hurt
└─ Death
~~~

각 상태의 진입과 종료를 분리하여 행동 로직을 독립적으로 관리하고, 새로운 상태를 추가하거나 기존 상태를 수정하기 용이하도록 구성했습니다.

### Strategy Pattern

보스의 공격 패턴을 개별 전략으로 분리하여 보스의 상태 관리와 공격 패턴을 분리했습니다.

이를 통해 새로운 공격 패턴을 추가할 때 기존 보스 상태 로직의 수정 범위를 최소화했습니다.

### ScriptableObject

캐릭터와 몬스터의 스탯 데이터를 ScriptableObject로 분리하여 게임 로직과 데이터의 관심사를 분리했습니다.

~~~text
ScriptableObject
      ↓
Character / Enemy
      ↓
Runtime Stat
~~~

기본 스탯과 런타임 스탯을 분리하고 Dictionary 기반으로 실시간 스탯을 관리하여 성장에 따른 최종 스탯을 계산하도록 구성했습니다.

### Observer Pattern

`IDamageable`을 기반으로 데미지 처리와 관련된 객체 간 결합도를 낮추고 이벤트 기반으로 상태 변화를 전달하도록 구성했습니다.

이를 통해 공격 주체가 특정 대상의 구체적인 구현에 의존하지 않도록 설계했습니다.

### Mediator Pattern

`UIManager`를 중심으로 여러 UI 시스템을 관리하여 게임 로직과 개별 UI 간의 직접적인 참조를 줄였습니다.

게임 시스템에서 발생한 상태 변화를 UI에 전달하는 역할을 중앙에서 관리하도록 구성했습니다.

### UniTask

시간 기반 연출 및 비동기 작업에 UniTask를 사용했습니다.

`GetCancellationTokenOnDestroy()`를 활용하여 GameObject가 파괴될 경우 실행 중인 비동기 작업이 함께 취소되도록 구성했습니다.

이를 통해 씬 전환이나 객체 제거 이후에도 이전 객체의 비동기 작업이 계속 실행되는 문제를 방지했습니다.

---

## 🔧 Technical Challenges

**씬 전환 시 게임 객체 및 데이터 유지**

게임 씬마다 Player와 UI를 생성하는 방식 대신 Persistent Scene을 구성하고 Additive Scene Loading을 적용했습니다.

이를 통해 씬 전환 시 Player, Camera, UI 및 Manager의 중복 생성을 방지하고 게임 진행에 필요한 객체를 지속적으로 유지할 수 있도록 구성했습니다.

**비동기 작업과 씬 전환**

UniTask 기반 비동기 작업이 실행되는 도중 GameObject가 제거될 수 있는 상황을 고려하여 `GetCancellationTokenOnDestroy()`를 적용했습니다.

객체가 파괴되면 해당 객체와 연결된 CancellationToken을 통해 비동기 작업을 안전하게 종료하도록 구성했습니다.

---

## 📁 Project Structure

~~~text
Assets/_Project/
├─ Scripts/
│  ├─ Character/
│  │  ├─ Player
│  │  ├─ Enemy
│  │  └─ Boss
│  │
│  ├─ Core/
│  │  ├─ Damage
│  │  ├─ Gold
│  │  └─ Stat
│  │
│  ├─ Managers/
│  │  ├─ Persistent
│  │  ├─ UI
│  │  ├─ Stage
│  │  └─ Camera
│  │
│  └─ UIs/
│
├─ SO/
├─ Scenes/
├─ Animation/
├─ Resources/
├─ Systems/
├─ Fonts/
└─ Downloads/
~~~

---

## 📦 Assets

본 프로젝트에는 Unity Asset Store 및 외부 에셋을 활용했습니다.

사용된 에셋의 저작권 및 라이선스는 각 에셋의 원 저작권자에게 있습니다.

---

## 👤 Developer

**김정현 (kun93686)**

- Email: kun93686@gmail.com
- GitHub: [rakoon98/unity_gold_eater](https://github.com/rakoon98/unity_gold_eater/tree/master)