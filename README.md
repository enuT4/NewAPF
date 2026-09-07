# NewAPF

## Project Overview

NewAPF는 액션퍼즐 패밀리를 참고해 제작한 미출시 개인 프로젝트
[ProjectAPF](https://enut4ljr.dothome.co.kr/ProjectAPF.html)를 리팩토링한 프로젝트입니다.

기존 ProjectAPF의 게임 콘텐츠와 기본적인 외형은 유지하면서,
사운드 시스템을 추가하고 구조와 성능상의 문제를 개선하는 데 중점을 두었습니다.

## Tech Stack

- Unity
- C#
- PlayFab

## 주요 개선 사항

- **시간 기반 재화 충전**
  - 기존: 로컬 시간 기반으로 재화별 충전 예정 시간을 DateTime 배열로 관리
  - 개선: 서버 시간 기반으로 변경하고 Unix Time + Offset을 활용해 계산

- **객체 관리**
  - 기존: Instantiate / Destroy 방식
  - 개선: Object Pooling을 적용해 객체 재사용

- **사운드 시스템**
  - 기존: 별도의 사운드 시스템 없음
  - 개선: 효과음과 배경음을 분리해 관리하는 사운드 시스템 추가

## 주요 코드

프로젝트의 전체 스크립트 중 주요 시스템과 게임 로직은 아래 코드에서 확인할 수 있습니다.

- **시간 기반 재화 시스템**
  - [LoginPanel.cs](https://github.com/enuT4/NewAPF/blob/main/Assets/Scripts/LoginPanel.cs)
    - PlayFab 로그인 및 서버 시간 동기화, 로컬 시간과의 Offset 설정
  - [ReadySceneMgr.cs](https://github.com/enuT4/NewAPF/blob/main/Assets/Scripts/ReadySceneMgr.cs) (`UpdateRiceFunc`)
    - 재화 충전 상태 확인 및 충전량 계산

- **Object Pooling**
  - [MemoryPoolManager.cs](https://github.com/enuT4/NewAPF/blob/main/Assets/Scripts/MemoryPool/MemoryPoolManager.cs)
    - Pool 생성 및 객체 획득·반환 관리
  - [MemoryPoolObject.cs](https://github.com/enuT4/NewAPF/blob/main/Assets/Scripts/MemoryPool/MemoryPoolObject.cs)
    - Pool 객체의 반환 요청 및 초기화 기능

- **Sound System**
  - [SoundManager.cs](https://github.com/enuT4/NewAPF/blob/main/Assets/Scripts/SoundManager.cs)
    - 효과음 관리
  - [MusicManager.cs](https://github.com/enuT4/NewAPF/blob/main/Assets/Scripts/MusicManager.cs)
    - 배경음악 관리

- **In-Game Game Logic**
  - [SDJRIngameMgr.cs](https://github.com/enuT4/NewAPF/blob/main/Assets/Scripts/SDJRIngameMgr.cs)
    - SDJR 미니게임의 플레이 흐름 및 게임 로직
  - [YSMSIngameMgr.cs](https://github.com/enuT4/NewAPF/blob/main/Assets/Scripts/YSMSIngameMgr.cs)
    - YSMS 미니게임의 플레이 흐름 및 게임 로직

## Portfolio

프로젝트 상세 설명 및 플레이 영상은 아래 링크에서 확인할 수 있습니다.

[NewAPF Portfolio](https://enut4ljr.dothome.co.kr/NewAPF.html)
