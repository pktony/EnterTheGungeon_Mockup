엔터더건전 모작 (2D Portfolio)

작업 기간 : 2022.7.15. ~ 2022.10.24.

랜덤으로 생성되는 맵에서 몬스터를 처치하고 아이템을 모아, 보스를 처치하라 !

상세 설명 (Notion) : https://mighty-thrill-892.notion.site/Enter-The-Gungeon-8110afce0e2549608ce82e75b8b763c8

기능 구현 목록
 1. 플레이어
	1.1. 조작
		1.1.1. 움직임
		1.1.2. 공격 / 피격
		1.1.3. 회피
		1.1.4. 특수 기술
	1.2. 무기
		1.2.1. Pistol
		1.2.2. AR
		1.2.3. Shotgun
	1.3. 몬스터
		1.3.1. 일반 몬스터 
		1.3.2. 보스 (Bullet King)
		
 2. 적
	2.1. AI
	2.2. 스폰 관리 

 3. 카메라
	3.1. 마우스 위치와 캐릭터 위치의 비율 따른 카메라 움직임

 4. PostProcessing / Shader
	4.1. 2D light
	4.2. Bloom
	4.3. Sprite Shadow
	4.4. Outline Shader

 5. Optimization 
	5.1. String 비교 최소화
	5.2. 오브젝트 풀