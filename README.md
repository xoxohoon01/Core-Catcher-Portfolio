# Core Catcher — Development Portfolio

[Core Catcher](https://github.com/xoxohoon01/Core-Catcher) 개발 과정에서 나온 설계 결정, 문제 해결 과정, 배운 점을 정리하는 레포입니다.

게임 자체의 소스 코드는 별도 레포에 있고, 여기는 "왜 그렇게 했는가"와 "어떻게 풀었는가"에 집중한 글만 모읍니다.

## 목차

- [Unity 6 마이그레이션 중 발견한 "GUID 자동 삭제" 문제와 복구](entries/unity6-migration-guid-recovery.md)
- [3D 모델 파일을 직접 고쳐서 저장하면 안 되는 이유 — 캐릭터 한 명을 통째로 잃고 배운 것](entries/raven-character-rebuild.md)
- [보스가 하늘로 계속 떠오르던 버그 — 상태는 끝났다는데 왜 계속 움직였을까](entries/boss-airborne-velocity-feedback-loop.md)
- [성장이 보이는 재미를 다른 방식으로 풀어낸 아티팩트 진화 시스템](entries/artifact-evolution-system.md)
- [이동속도용 변수 하나가 스킬 지속시간까지 바꿔버린 이유](entries/attack-move-speed-animator-speed-bug.md)
- [위치가 이상하다 한 마디 뒤에 겹겹이 숨어있던 네 가지 원인](entries/lobby-ui-static-placement-layered-bugs.md)
- [리소스 경로를 의심했는데, 범인은 자식 오브젝트 순서였다](entries/character-select-child-index-nullref.md)
- [축복 배너가 누를수록 삐뚤어지던 버그 — 애니메이션에게 지금 위치를 물어보면 안 되는 이유](entries/blessing-ribbon-position-drift.md)
- [아웃라인이라는 이름의 함정 — 그리고 몬스터 여러 마리가 재질 하나를 나눠 쓰면 생기는 일](entries/monster-outline-shared-material-bug.md)
- [몬스터가 밀려요 — 물리 버그를 한참 파다가, 사실은 전혀 다른 곳에 있었다](entries/lingering-attack-indicator-mistaken-for-push.md)
- [밸런스 조정용으로 만든 도구가, 만들자마자 진짜 버그를 하나 잡아냈다](entries/artifact-balance-tool-maxlevel-bug.md)
- [이름 하나 바꾸려다 실행 구조 전체를 다시 짠 이야기](entries/passive-artifact-unification.md)

## 코드 히스토리

`Assets/Scripts` 전체와 아티팩트/스킬/스킬트리 ScriptableObject 데이터(`Assets/Resources/{ArtifactSO,CharacterSkillSO,SkillTreeSO}`)의 커밋 이력(137개 커밋)을 [`code-history`](../../tree/code-history) 브랜치에 그대로 보존해 두었습니다. 원래 커밋 메시지·날짜를 유지한 채로, `Scripts/`·`Resources/`로 정리했습니다. SO 데이터를 포함한 이유는 밸런스·아티팩트 설계를 코드만으로는 보여주기 어렵기 때문입니다.

구매 에셋·이미지·사운드 등은 라이선스상 포함하지 않습니다.

## 글 구조

각 글은 `entries/` 아래에 한 파일로 작성하며, 다음 틀을 기본으로 합니다 (`entries/TEMPLATE.md` 참고):

- 배경
- 문제
- 해결 과정
- 결과 / 배운 점
