# Core Catcher — Development Portfolio

[Core Catcher](https://github.com/xoxohoon01/Core-Catcher) 개발 과정에서 나온 설계 결정, 문제 해결 과정, 배운 점을 정리하는 레포입니다.

게임 자체의 소스 코드는 별도 레포에 있고, 여기는 "왜 그렇게 했는가"와 "어떻게 풀었는가"에 집중한 글만 모읍니다.

## 목차

(항목이 쌓이면 여기에 링크를 추가합니다)

## 코드 히스토리

`Assets/Scripts` 전체와 아티팩트/스킬/스킬트리 ScriptableObject 데이터(`Assets/Resources/{ArtifactSO,CharacterSkillSO,SkillTreeSO}`)의 커밋 이력(132개 커밋)을 [`code-history`](../../tree/code-history) 브랜치에 그대로 보존해 두었습니다. 원래 커밋 메시지·날짜를 유지한 채로, `Scripts/`·`Resources/`로 정리했습니다. SO 데이터를 포함한 이유는 밸런스·아티팩트 설계를 코드만으로는 보여주기 어렵기 때문입니다.

구매 에셋·이미지·사운드 등은 라이선스상 포함하지 않습니다.

## 글 구조

각 글은 `entries/` 아래에 한 파일로 작성하며, 다음 틀을 기본으로 합니다 (`entries/TEMPLATE.md` 참고):

- 배경
- 문제
- 해결 과정
- 결과 / 배운 점
