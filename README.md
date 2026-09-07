# NewAPF

## Project Overview

NewAPF는 여러 미니게임을 플레이하며 기록을 세우고 점수를 경쟁하는
모바일 게임 ProjectAPF를 리팩토링한 개인 프로젝트입니다.

게임의 기본적인 콘텐츠와 외형은 ProjectAPF를 기반으로 유지했으며,
사운드 시스템을 추가하고 기존 프로젝트에서 발견한 구조와 성능 문제를 개선하는 데 중점을 두었습니다.

## Tech Stack

- Unity
- C#
- PlayFab

## 주요 개선 사항

- **시간 기반 재화 충전**
  - 기존: 재화별 충전 예정 시간을 DateTime 배열로 관리
  - 개선: 다음 충전 완료 시간을 기준으로 관리하고 Unix Time + Offset으로 계산

- **객체 관리**
  - 기존: Instantiate / Destroy 방식
  - 개선: Object Pooling 적용

- **사운드 시스템**
  - 기존: 분산된 사운드 재생 처리
  - 개선: SoundManager를 통한 BGM 및 효과음 관리

- **UI 구조**
  - UI 처리 구조를 정리하고 시스템과 UI의 연결 방식 개선

## Portfolio

프로젝트 상세 설명 및 플레이 영상은 아래 링크에서 확인할 수 있습니다.

[NewAPF Portfolio](https://enut4ljr.dothome.co.kr/NewAPF.html)
