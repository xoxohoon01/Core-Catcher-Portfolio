# 이미지 폴더 규칙

글마다 쓰는 이미지는 `entries/images/<글 파일명(확장자 제외)>/` 아래에 둔다.

예: `entries/hit-sound-voice-stealing-and-mixer-ducking.md` 글에서 쓰는 이미지는
`entries/images/hit-sound-voice-stealing-and-mixer-ducking/` 아래.

파일명은 `01-설명.png`처럼 번호 + 짧은 설명으로, 글에서 참조하는 순서대로 번호를 매긴다.

글에서는 상대 경로로 참조한다:

```markdown
![설명](images/<글 파일명>/01-설명.png)
```

## 진행 방식

글을 쓸 때 Claude가 "이런 장면/구조를 캡처해주세요"라고 먼저 제안하고, 사용자가 실제 스크린샷·다이어그램을
찍어서 이 폴더에 넣은 뒤 알려주면 Claude가 글에 삽입한다.
