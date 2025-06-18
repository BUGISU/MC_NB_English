# 🪄 영어 마법 모험 (English Magic Adventure)

> **기초 영어 단어와 문장 구성 학습**을 위한
> 초등학교 1학년 대상 **3D 인터랙티브 영어 콘텐츠**

---

## 📌 개요

\*\*“영어 마법 모험”\*\*은 Leia LUME PAD 2의 3D 입체 디스플레이를 활용하여
아이들이 영어 단어를 즐겁고 자연스럽게 익히도록 돕는 **영어 단어/문장 학습 콘텐츠**입니다.

학생은 마법사 '에이미'와 함께
① 단어 찾기 → ② 문장 만들기 → ③ 마무리 피드백의 구조로
학습의 몰입과 반복 효과를 동시에 경험하게 됩니다.

---

## 🛠 개발 정보

| 항목       | 내용                                         |
| -------- | ------------------------------------------ |
| 엔진       | Unity 2022.3 LTS                           |
| 대상 디바이스  | Leia Lume Pad 2, Android 기반                |
| 주요 기술    | DOTween, TextMeshPro, URP, 3D Stereoscopic |
| 인터페이스 요소 | 3D 캐릭터, 드래그 앤 드롭, 나레이션 텍스트/오디오             |

---

## 📚 학습 콘텐츠 구성 방식

### 🎯 Mission01: 영어 단어 찾기

* **방식**: 마법사가 지시하는 단어(예: “apple”)를 화면에서 터치
* **목표**: 단어 인식, 영어 음성 피드백 체험
* **정답 시**: 애니메이션 확대 + 칭찬 나레이션 + 보상 아이콘

### 🧩 Mission02: 문장 만들기

* **방식**: 단어 조각을 드래그하여 빈칸에 올바르게 조합
* **목표**: 영어 문장 구조 이해 및 어휘 반복 학습
* **정답 시**: 문장 완성 이펙트 + 캐릭터 칭찬 + 다음 문제 자동 진행

---

## 🖼️ 예시 이미지

### Mission01 - 영어 단어 찾기

| 메인 화면                               | 미션 1 -화면구성 및 타이틀                                | 오답 시 화면                              |
| ----------------------------------- | -------------------------------------- | ------------------------------------ |
| ![](/ScreenShots/Screenshot_20250325_215755.jpg) | ![](/ScreenShots/Screenshot_20250325_221054.jpg) | ![](/ScreenShots/Mission1_Wrong.jpg) |

### Mission02 - 문장 조합하기

| 문제 화면                               | 정답 시 효과                                |
| ----------------------------------- | -------------------------------------- |
| ![](/ScreenShots/Screenshot_20250325_221134.jpg) | ![](/ScreenShots/Screenshot_20250325_221154.jpg) |

---

## 📁 프로젝트 구조 개요

해당 프로젝트는 다음과 같은 구성으로 되어 있습니다:

1. **Introduction**: 마법사 인사, 영어 세계 입장
2. **Mission 1**: 단어 찾기 (알파벳 떠다니는 공간 터치)
3. **Mission 2**: 문장 만들기 (단어 조각 드래그하여 빈칸에 배치)
4. **Ending**: 마무리 피드백 및 다음 학습 예고

---

## 📦 폴더 구조

```plaintext
/Assets
├── Scenes/
│   ├── Introduction.unity
│   ├── Mission1.unity
│   └── Mission2.unity
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs
│   │   ├── SoundManager.cs
│   │   ├── SettingManager.cs
│   ├── Managers/
│   │   ├── NarrationManager.cs
│   │   ├── CoroutineRunner.cs
│   ├── Missions/
│   │   ├── Mission1/
│   │   │   ├── Mission1_GameManager.cs
│   │   │   ├── Mission1_DataManager.cs
│   │   │   ├── Mission1_UIManager.cs
│   │   ├── Mission2/
│   │   │   ├── Mission2_GameManager.cs
│   │   │   ├── Mission2_DataManager.cs
│   │   │   ├── Mission2_UIManager.cs
│   ├── UI/
│   │   ├── Introduction_UIManager.cs
│   ├── Utilities/
│   │   ├── TouchObjectDetector.cs
│   │   ├── TouchSelf.cs
│   │   ├── ObjectBlink.cs
│   │   ├── FloatkoreaAlphabet.cs
│   │   ├── StringKeys.cs
```

---

## 🔁 실행 흐름 요약

### 🎬 1. 인트로 씬 (`Introduction_UIManager`)

* “터치하여 시작” 버튼 → 캐릭터 인사 및 인트로 나레이션 출력
* 카메라 이동 + 3D 이펙트 → `Mission1` 씬으로 자동 전환

---

### ✋ 2. Mission01 - 영어 단어 찾기

* `Mission1_DataManager` → 단어 리스트 생성 및 퀴즈 세팅
* `TouchObjectDetector`로 알파벳 선택
* `TouchSelf`로 정답/오답 이벤트 처리
* 정답 시 `NPCController` 애니메이션 + 나레이션 연출

---

### ✍️ 3. Mission02 - 문장 조합하기

* 문장에서 빈칸 위치 선정 (`Mission2_DataManager`)
* 단어 후보 섞기 및 선택지 구성
* 올바른 순서로 단어를 드래그하여 문장 완성
* 모든 문제 완료 시 → 마법사 캐릭터 등장 + 종료 메시지

---

## 🔧 주요 클래스 설명

| 클래스 이름                   | 역할 / 기능 요약                         |
| ------------------------ | ---------------------------------- |
| ✅ `GameManager`          | 씬 전환, 터치 가능 여부 제어, 전체 흐름 제어        |
| ✅ `NarrationManager`     | 텍스트 타이핑 + 나레이션 오디오 동기화 처리          |
| ✅ `SoundManager`         | 배경음악, 효과음, 나레이션 음성 재생 관리           |
| ✅ `CoroutineRunner`      | 코루틴 실행 제어 및 중복 방지, 타임아웃 지원         |
| ✅ `TouchObjectDetector`  | 터치/드래그 오브젝트 감지 및 Mission별 입력 분기 처리 |
| ✅ `Mission1_GameManager` | Mission1 전체 흐름 및 정답 판정 제어          |
| ✅ `Mission2_GameManager` | Mission2 전체 퀴즈 루프 제어 및 정답 처리       |
| ✅ `Mission1_DataManager` | 단어 리스트 랜덤 구성 및 정답 설정               |
| ✅ `Mission2_DataManager` | 문장 조각 분해, 정답/선택지 분리, 문장 완성 로직 구성   |
| ✅ `UIManager`들           | 각 미션의 연출, 효과, 나레이션 호출 시점 제어        |

---

## 🧩 기타 유틸리티 요소

* `FloatkoreaAlphabet`: 알파벳이 부유하는 애니메이션 처리
* `ObjectBlink`: 터치 유도 UI 깜빡임 효과
* `NPCController`: 마법사 캐릭터 애니메이션 트리거 제어
* `SettingManager`: 볼륨, 재시작, 3D 모드 설정 기능 제공

