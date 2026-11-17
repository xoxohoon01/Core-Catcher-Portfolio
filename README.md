# Core Catcher — Development Portfolio

> **왜 그렇게 했는가, 어떻게 풀었는가.**

1인 개발 중인 3D 로그라이트 액션 게임 [Core Catcher](https://github.com/xoxohoon01/Core-Catcher)의 설계 결정과 트러블슈팅 기록입니다. 게임 소스 코드는 별도 레포에 있고, 이곳에는 문제를 어떻게 풀었는지에 대한 글만 모았습니다.

| 기록 | 분야 | 작성 기간 | 엔진 |
|:-:|:-:|:-:|:-:|
| **22편** | 5개 | 2026.09.24 ~ 11.18 | Unity 6 · URP · C# |

모든 글은 **배경 → 문제 → 해결 과정 → 결과 / 배운 점** 순서로 정리했습니다.

## 먼저 읽어볼 글

### [몬스터 버그를 막으려고 복제한 재질이, 최적화할 때 발목을 잡았다](entries/monster-material-sharing-optimization.md)
`렌더링 · 최적화` · 2026.11.13

몬스터마다 재질을 복제하던 구조 때문에 GPU 배칭이 전혀 걸리지 않아 Batches 3091개 중 배칭으로 줄인 건 14개뿐이었습니다. `MaterialPropertyBlock`으로 바꿔 개체별 효과는 유지하면서 재질을 공유하게 했고, 몬스터 한 마리당 CPU 비용을 약 37% 줄였습니다.

### [성장이 "보이는" 재미를 다른 방식으로 풀어낸 아티팩트 진화 시스템](entries/artifact-evolution-system.md)
`시스템 설계 · 툴링` · 2026.09.25

만렙 아티팩트와 특정 아이템을 조합해 진화하는 시스템을 설계·구현했습니다. 이 과정에서 진화 전에도 효과가 발동하던 기존 버그를 찾았고, 진화 후보가 여러 개일 때 카드 슬롯에 고르게 배정되도록 레벨업 카드 로직을 다시 짰습니다.

### [이름 하나 바꾸려다 실행 구조 전체를 다시 짠 이야기](entries/passive-artifact-unification.md)
`시스템 설계 · 툴링` · 2026.10.01

아티팩트 효과 하나를 패시브로 옮기려다 보니, 두 시스템의 초기화 방식이 달라 이벤트가 레벨 수만큼 중복 구독되는 구조였습니다. 선택지 세 가지를 비교한 뒤 패시브를 검증된 아티팩트 구조로 통일했고, 데이터 구조도 키 기반으로 확장했습니다.

### ["맞았는데 소리가 안 나요" — 범인은 하나가 아니라 둘이었다](entries/hit-sound-voice-stealing-and-mixer-ducking.md)
`게임플레이 · 애니메이션 · 오디오` · 2026.10.14

여러 마리를 동시에 때릴 때 타격음이 사라지는 문제의 원인은 두 가지였습니다. 믹서 버스 Compressor가 다른 그룹 소리까지 함께 눌렀고, `Real Voice Count` 한도(32)를 넘은 소리는 엔진이 가상화해 묵음 처리하고 있었습니다.

## 분야별 목차

### 시스템 설계 · 툴링 (5)

| 글 | 날짜 | 태그 |
|---|:-:|---|
| [애니메이션 옵션 하나 그리는 커스텀 에디터가, 상속 트리 전체의 필드를 가리고 있었다](entries/uibase-custom-editor-hid-subclass-fields.md) | 26.10.22 | `에디터 툴링` `트러블슈팅` |
| [이름 하나 바꾸려다 실행 구조 전체를 다시 짠 이야기](entries/passive-artifact-unification.md) | 26.10.01 | `게임 설계` `아키텍처` |
| [밸런스 조정용으로 만든 도구가, 만들자마자 진짜 버그를 하나 잡아냈다](entries/artifact-balance-tool-maxlevel-bug.md) | 26.10.01 | `에디터 툴` `디버깅` `밸런싱` |
| [성장이 "보이는" 재미를 다른 방식으로 풀어낸 아티팩트 진화 시스템](entries/artifact-evolution-system.md) | 26.09.25 | `게임 설계` `시스템 구현` |
| [세이브 파일 하나가 깨졌을 뿐인데, 멀쩡한 캐릭터가 통째로 사라졌다](entries/skillmanager-corrupted-save-cascade-failure.md) | 26.11.18 | `저장 시스템` `디버깅` |

### 렌더링 · 최적화 (3)

| 글 | 날짜 | 태그 |
|---|:-:|---|
| [몬스터 버그를 막으려고 복제한 재질이, 최적화할 때 발목을 잡았다](entries/monster-material-sharing-optimization.md) | 26.11.13 | `렌더링` `최적화` `GPU 인스턴싱` |
| [렌더러를 의심하고, 카메라를 의심했는데 — 진짜 범인은 태그 한 줄이었다](entries/napalm-decal-urp-rendering-path-tag.md) | 26.11.13 | `URP` `셰이더` `디버깅` |
| ["아웃라인"이라는 이름의 함정 — 그리고 몬스터 여러 마리가 재질 하나를 나눠 쓰면 생기는 일](entries/monster-outline-shared-material-bug.md) | 26.09.30 | `셰이더` `렌더링` `디버깅` |

### 게임플레이 · 애니메이션 · 오디오 (5)

| 글 | 날짜 | 태그 |
|---|:-:|---|
| [보스를 때리는 타이밍에 따라 영원히 멈춰버리는 버그](entries/draug-attack-animation-event-timing.md) | 26.11.11 | `애니메이션` `트러블슈팅` |
| ["맞았는데 소리가 안 나요" — 범인은 하나가 아니라 둘이었다](entries/hit-sound-voice-stealing-and-mixer-ducking.md) | 26.10.14 | `오디오` `디버깅` |
| ["몬스터가 밀려요" — 물리 버그를 한참 파다가, 사실은 전혀 다른 곳에 있었다](entries/lingering-attack-indicator-mistaken-for-push.md) | 26.09.30 | `디버깅` `게임플레이` |
| [보스가 하늘로 계속 떠오르던 버그 — 상태는 "끝났다"는데 왜 계속 움직였을까](entries/boss-airborne-velocity-feedback-loop.md) | 26.09.25 | `물리` `디버깅` |
| ["이동속도용" 변수 하나가 스킬 지속시간까지 바꿔버린 이유](entries/attack-move-speed-animator-speed-bug.md) | 26.09.25 | `애니메이션` `트러블슈팅` |

### UI (6)

| 글 | 날짜 | 태그 |
|---|:-:|---|
| [분명히 막아뒀는데 왜 계속 끊길까 — 호버 애니메이션과 슬라이드 애니메이션의 자리싸움](entries/levelup-card-hover-slide-conflict.md) | 26.11.03 | `UI` `애니메이션` `트러블슈팅` |
| [ESC를 눌러도 안 닫히던 설정 창 — 범인은 하나가 아니라 셋이었다](entries/settings-panel-pause-menu-layered-bugs.md) | 26.10.23 | `UI` `트러블슈팅` |
| [카드가 두 겹으로 떴다 — 서로의 존재를 몰랐던 두 개의 리워드 UI](entries/levelup-blessing-card-overlap.md) | 26.10.14 | `UI` `디버깅` `동시성` |
| [축복 배너가 누를수록 삐뚤어지던 버그 — 애니메이션에게 "지금 위치"를 물어보면 안 되는 이유](entries/blessing-ribbon-position-drift.md) | 26.09.29 | `UI` `애니메이션` `디버깅` |
| [리소스 경로를 의심했는데, 범인은 자식 오브젝트 순서였다](entries/character-select-child-index-nullref.md) | 26.09.28 | `UI` `트러블슈팅` |
| ["위치가 이상하다" 한 마디 뒤에 겹겹이 숨어있던 네 가지 원인](entries/lobby-ui-static-placement-layered-bugs.md) | 26.09.25 | `UI` `트러블슈팅` |

### 에셋 파이프라인 (3)

| 글 | 날짜 | 태그 |
|---|:-:|---|
| [몬스터 이름 하나 넣으려고 로컬라이제이션 테이블을 손으로 역설계한 이야기](entries/monster-codex-localization-table-by-hand.md) | 26.10.08 | `로컬라이제이션` `에셋 파이프라인` `트러블슈팅` |
| [3D 모델 파일을 직접 고쳐서 저장하면 안 되는 이유 — 캐릭터 한 명을 통째로 잃고 배운 것](entries/raven-character-rebuild.md) | 26.09.25 | `캐릭터 파이프라인` `3D 애셋 관리` |
| [Unity 6 마이그레이션 중 발견한 "GUID 자동 삭제" 문제와 복구](entries/unity6-migration-guid-recovery.md) | 26.09.24 ~ 09.25 | `에셋 파이프라인` `트러블슈팅` |

## 코드 히스토리

`Assets/Scripts` 전체와 아티팩트/스킬/스킬트리 ScriptableObject 데이터(`Assets/Resources/{ArtifactSO,CharacterSkillSO,SkillTreeSO}`)의 커밋 이력(137개 커밋)을 [`code-history`](../../tree/code-history) 브랜치에 그대로 보존해 두었습니다. 원래 커밋 메시지·날짜를 유지한 채로, `Scripts/`·`Resources/`로 정리했습니다. SO 데이터를 포함한 이유는 밸런스·아티팩트 설계를 코드만으로는 보여주기 어렵기 때문입니다.

구매 에셋·이미지·사운드 등은 라이선스상 포함하지 않습니다.

## 글 구조

각 글은 `entries/` 아래에 한 파일로 작성하며, 다음 틀을 기본으로 합니다 (`entries/TEMPLATE.md` 참고):

- 배경
- 문제
- 해결 과정
- 결과 / 배운 점

새로 쓰는 글에는 가능하면 이미지(스크린샷·다이어그램)를 포함합니다. 어떤 장면/구조가 필요한지는 Claude가
먼저 제안하고, 실제 촬영·제작은 사용자가 해서 `entries/images/<글 파일명>/`에 넣습니다 (규칙:
`entries/images/README.md` 참고).
