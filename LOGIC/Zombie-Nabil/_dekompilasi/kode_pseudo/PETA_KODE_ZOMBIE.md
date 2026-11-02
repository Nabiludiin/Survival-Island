# Peta Kode Zombie (server.dll)

Dibuat dari pseudo-C Ghidra. Lokasi `bagian_00X.c:baris` mengacu ke folder `server\`.

**Tingkat keyakinan:** nama di bagian 1 berasal dari teks yang tertanam di fungsinya sendiri.
Kategori di bagian 2 hanya perkiraan dari kata kunci teks, jadi satu fungsi bisa muncul di beberapa
kategori dan sebagian bisa salah sasaran. Sebagian besar fungsi tanpa teks tidak tercakup di peta ini.

## 1. Fungsi dengan nama asli yang tertanam

| Alamat | Nama | Lokasi | Baris |
|---|---|---|---|
| `102c7d10` | `CTankRock::ProximityThink` | bagian_004.c:70055 | 95 |
| `1021fcd0` | `Director::OnPlayerTransitioned` | bagian_003.c:68897 | 22 |
| `10235ff0` | `Director::Update` | bagian_003.c:84503 | 375 |
| `10324c60` | `Infected::Update` | bagian_005.c:12760 | 94 |
| `1032bf40` | `InfectedAlert::Update` | bagian_005.c:17028 | 131 |
| `1032e050` | `InfectedAttack::ChaseVictim::OnContact` | bagian_005.c:18121 | 175 |
| `1032efc0` | `InfectedAttack::ChaseVictim::Update` | bagian_005.c:18634 | 217 |
| `1032d7b0` | `InfectedAttack::OnPunch` | bagian_005.c:17869 | 107 |
| `1032edf0` | `InfectedAttack::OnStart` | bagian_005.c:18546 | 53 |
| `1032e710` | `InfectedAttack::PunchVictim::Update` | bagian_005.c:18297 | 203 |
| `103322f0` | `InfectedStaggerAround::Update` | bagian_005.c:20464 | 32 |
| `10333970` | `InfectedWander::Update` | bagian_005.c:20938 | 73 |
| `102f39f0` | `SurvivorTeamSituation::GetPathToFriend` | bagian_004.c:93569 | 85 |
| `1031a5a0` | `ZombieBotBody::Update` | bagian_005.c:6722 | 250 |
| `1031ccd0` | `ZombieBotLocomotion::ApplyAccumulatedApproach` | bagian_005.c:8310 | 137 |
| `1031ebb0` | `ZombieBotLocomotion::ResolveCollision` | bagian_005.c:9320 | 288 |
| `1031d2f0` | `ZombieBotLocomotion::ResolveZombieCollisions` | bagian_005.c:8536 | 81 |
| `1031f930` | `ZombieBotLocomotion::Update` | bagian_005.c:9734 | 226 |
| `1031d540` | `ZombieBotLocomotion::UpdateGroundConstraint` | bagian_005.c:8618 | 199 |
| `1031f3a0` | `ZombieBotLocomotion::UpdatePosition` | bagian_005.c:9609 | 28 |
| `10320590` | `ZombieBotVision::Update` | bagian_005.c:10185 | 127 |
| `102c1ee0` | `ZombieManager::CollectSpawnAreas` | bagian_004.c:67386 | 309 |
| `102c3800` | `ZombieManager::StartFrame` | bagian_004.c:68396 | 194 |
| `10225400` | `Director::StartPanicEvent` (dibaca manual) | bagian_003.c:73052 | 74 |

## 2. Fungsi per kategori (perkiraan, skor tertinggi dulu)

### Director / spawn (150 fungsi)

| Alamat | Lokasi | Baris | Teks penanda |
|---|---|---|---|
| `1023d230` | bagian_003.c:88811 | 364 | AdvanceFinaleState / CFinaleTrigger / DLC3FinaleEnableChaseTanks |
| `10227260` | bagian_003.c:74514 | 202 | %3.2f: Director: Final Tank has FAILED to spawn! Giving up and startin / %3.2f: Director: Final Tank has spawned. / %3.2f: Director: Finale Escape blocked, spawning horde |
| `10225400` | bagian_003.c:73052 | 74 | %3.2f: Director: Panic Stage = STAGE_INITIAL_DELAY / %3.2f: Director::StartPanicEvent( %d ): ERROR: It is too soon to resta / %3.2f: Director::StartPanicEvent( %d ): Panic event started. |
| `10235ff0` | bagian_003.c:84503 | 375 | ------------------------------ Director state change BUILD_UP -> SUSTA / ------------------------------ Director state change PEAK_FADE -> RELA / ------------------------------ Director state change RELAX -> BUILD_UP |
| `10233660` | bagian_003.c:83163 | 333 | Intervals: Horde: %f, Specials: %f, Tank: %f, Lull: %f / Mob spawn area is #%d, alternate is #%d / Mob spawn timer: %3.1f |
| `1022ecc0` | bagian_003.c:79882 | 122 | PlayerWarnMegaMob / STAGE_INITIAL_DELAY -> STAGE_MEGA_MOB / STAGE_MEGA_MOB -> STAGE_WAIT_FOR_COMBAT_TO_END |
| `102a4840` | bagian_004.c:51462 | 1302 | %s spawned as a %s / CCSPlayer::Spawn / Player %s spawned as %s after waiting %3.2f seconds |
| `1022ac80` | bagian_003.c:77273 | 200 | #L4D_Scenario_Finale_Won / FinaleLost / finale_trigger |
| `102c3800` | bagian_004.c:68396 | 194 | %3.2f: (MOB) Mob zombie spawned, %d remain. / %3.2f: (MOB) Pending mob done. / Mob zombie spawned in alternate area - %d left |
| `1022cea0` | bagian_003.c:78658 | 157 | [HOLDOUT]: HORDE ATTACK! / [HOLDOUT]: Horde Stage interval: %f / [HOLDOUT]: Special Spawn Interval: %f |
| `10237720` | bagian_003.c:85552 | 111 | %3.2f: Director: Finale Escape blocked, spawning horde / %3.2f: Director: Finale state = FINALE_HORDE_ESCAPE / DLC3FinaleLull |
| `10237330` | bagian_003.c:85400 | 100 | %3.2f: Director: Finale state = FINALE_HORDE_ATTACK_1 / %3.2f: Director::OnDLC3FinaleStarted: ERROR - Finale already started. / %3.2f: Director::OnDLC3FinaleStarted: Finale started. |
| `10321770` | bagian_005.c:10795 | 90 | spawning %s in %s, %d%% / spawning a random zombie in area \'%s\' - %d%% < 100%%? / spawning a random zombie in unpopulated area \'%s\' |
| `1022b330` | bagian_003.c:77506 | 69 | %3.2f: Director: Finale state = FINALE_HORDE_ATTACK_1 / %3.2f: Director::OnFinaleStarted: ERROR - Finale already started. / %3.2f: Director::OnFinaleStarted: Finale started. |
| `102246d0` | bagian_003.c:72379 | 46 | FinaleWon / finale_trigger / finale_win |
| `10249040` | bagian_003.c:96202 | 25 | %s Music::OnFinaleWave / Event.FinaleWave2 / Event.FinaleWave3 |
| `10220490` | bagian_003.c:69270 | 22 | %3.2f: Director::OnFinalePause / FinalePause / finale_trigger |
| `10275a00` | bagian_004.c:22037 | 481 |  FINALE /  RESCUE_CLOSET /  RESCUE_VEHICLE |
| `1022f360` | bagian_003.c:80164 | 207 | FinaleLost / finale_trigger / trigger_finale |
| `10238cd0` | bagian_003.c:86391 | 178 | CoOp Boss Spawning:  / Spawn Witch and Tank ? %d  / Versus Boss Spawning:  |
| `1026f800` | bagian_004.c:17831 | 155 | GetGoalArea: Cannot find SPAWN_RESCUE_CLOSET area in FINALE, thus cann / GetGoalArea: Cannot find end area - no checkpoint or finale located.  / trigger_finale |
| `1025e8a0` | bagian_004.c:8555 | 142 | (RESCUE) Rescue spot at %.0f, %.0f, %.0f (area %d) now spawning. / rescuer / survivor_rescued |
| `10236dc0` | bagian_003.c:85102 | 114 | DLC3 Finale failed: Less than 1 player alive. / DLC3 Finale failed: Less than 2 survivors alive. / Resource/UI/Dlc3FinaleFailed.res |
| `10231100` | bagian_003.c:81524 | 107 | info_survivor_rescue / rescue spot %d(%s) closes door to start calling for help with nearest  / rescue spot %d(%s) starts calling for help for %s with nearest survivo |
| `1025fa00` | bagian_004.c:9121 | 101 | (RESCUE) Rescue spot at %.0f, %.0f, %.0f (area %d) activated for %s(%d / (RESCUE) Rescue spot at %.0f, %.0f, %.0f (area %d) deactivated. / Rescue |

### Common infected (110 fungsi)

| Alamat | Lokasi | Baris | Teks penanda |
|---|---|---|---|
| `10118aa0` | bagian_001.c:83207 | 1617 | CRagdollProp / m_ragdoll.allowStretch / m_ragdoll.listCount |
| `10002240` | bagian_000.c:932 | 2108 | ACT_DIERAGDOLL / ACT_MELEE_SHOVE_RIFLE_IDLE / ACT_MELEE_SHOVE_RIFLE_RUN |
| `10292d40` | bagian_004.c:40680 | 164 | Player.Shoved / Weapon.HitInfected / models/infected/limbs/exploded_boomer.mdl |
| `1027fc20` | bagian_004.c:28634 | 167 | m_staggerDir / m_staggerDist / m_staggerStart |
| `10260c20` | bagian_004.c:9807 | 465 | Team \"%s\" triggered \"%s\" (Infected \"%i\") (Survivor \"%i\") / Team \"Infected\" scored \"%i\" with \"%i\" players / World triggered \"%s\" (Infected \"%i\") (Survivor \"%i\") |
| `10354130` | bagian_005.c:37303 | 398 | Weapon.HitInfected / entity_shoved / player_shoved |
| `1029d4f0` | bagian_004.c:46429 | 184 | Player.Shoved / lunge_shove / player_shoved |
| `100da1c0` | bagian_001.c:41416 | 97 | BounceGibTouch / CGib / StickyGibTouch |
| `100da740` | bagian_001.c:41577 | 58 | models/germangibs.mdl / models/gibs/agibs.mdl / models/gibs/hgibs.mdl |
| `1027f7b0` | bagian_004.c:28581 | 52 | m_flNextShoveTime / m_iShovePenalty / m_shoveForce |
| `100934a0` | bagian_001.c:860 | 45 | GibShooter Body is <= 1! / models/gibs/hgibs.mdl / violence_hgibs |
| `10200cb0` | bagian_003.c:48931 | 34 | m_ragdollType / m_vecRagdollOrigin / m_vecRagdollVelocity |
| `102b0ac0` | bagian_004.c:56392 | 350 | Infected / InfectedState |
| `102dd510` | bagian_004.c:80069 | 213 | infected / models/infected/%s.mdl |
| `10163930` | bagian_002.c:38487 | 127 | ParseRagdollIntoCache:  Couldn\'t Lookup Bone %s / ragdollconstraint |
| `1023f750` | bagian_003.c:90247 | 118 | info_infected_zoo_puppet / models/infected/%s.mdl |
| `10323810` | bagian_005.c:12083 | 111 | common / models/infected/%s.mdl |
| `100a9ab0` | bagian_001.c:14266 | 91 | AE_NPC_RAGDOLL / AE_RAGDOLL |
| `10030690` | bagian_000.c:34578 | 50 | violence_agibs / violence_hgibs |
| `10268db0` | bagian_004.c:13995 | 48 | (Infected) / *DEAD*(Infected) |
| `101fb550` | bagian_003.c:46090 | 45 | (Infected) / *DEAD*(Infected) |
| `10028850` | bagian_000.c:28683 | 43 | prop_ragdoll / ragdoll_dissolved |
| `1001f2f0` | bagian_000.c:21043 | 40 | infected/common / infected/witch |
| `10078ef0` | bagian_000.c:82996 | 37 | models/germangibs.mdl / models/gibs/hgibs.mdl |
| `100ece40` | bagian_001.c:53236 | 37 | phys_ragdollconstraint / prop_ragdoll |

### Hunter (49 fungsi)

| Alamat | Lokasi | Baris | Teks penanda |
|---|---|---|---|
| `10002240` | bagian_000.c:932 | 2108 | ACT_IDLE_POUNCED / ACT_TERROR_HUNTER_LANDING_HARD / ACT_TERROR_HUNTER_LUNGE |
| `10292d40` | bagian_004.c:40680 | 164 | HunterZombie.Alert / HunterZombie.Death / HunterZombie.Growl |
| `1027fc20` | bagian_004.c:28634 | 167 | m_checkpointPZHunterDamage / m_checkpointPZPounces / m_isAttemptingToPounce |
| `10297050` | bagian_004.c:42633 | 175 | %s starting a lunge, %d damage interrupts / %s starting a pounce, %d damage interrupts / HunterZombie.Lunge |
| `1028e190` | bagian_004.c:37567 | 39 | %s ending a lunge / %s ending a pounce / HunterZombie.LungeLand |
| `102a9020` | bagian_004.c:53369 | 394 | %s pounced on %s, %d damage interrupts / Pounce by %s dealt %0.1f damage from a 2d distance of %.0f / lunge_pounce |
| `102b4f70` | bagian_004.c:59063 | 347 | HunterZombie.Growl / HunterZombie.Pounce.shred / HunterZombie.Warn |
| `10319170` | bagian_005.c:6109 | 61 | Shoved during Pounce - trying to escape now / hunter_punched / hunteruserid |
| `102c4ea0` | bagian_004.c:69073 | 21 | m_lungeAgainTimer / m_lungeStartTime / m_queuedLunge |
| `102493b0` | bagian_003.c:96333 | 20 | %s Music::OnPounced / Event.HunterHit / Event.HunterPounce |
| `1029ab10` | bagian_004.c:45052 | 698 | HunterZombie.Death / pounce_stopped |
| `102b0ac0` | bagian_004.c:56392 | 350 | PounceVictim / ZombiePresentHunter |
| `102ea990` | bagian_004.c:87989 | 251 | SurvivorBotMoveToSavePouncedFriend / Trying to dislodge Pounced friend! |
| `1028e250` | bagian_004.c:37607 | 196 | HunterZombie.Pounce.Cancel / pounce_end |
| `1029d4f0` | bagian_004.c:46429 | 184 | Survivor was lunged while %sfacing the hunter and %sbashing / lunge_shove |
| `10285490` | bagian_004.c:31686 | 167 | %3.1f: %s Say( Player.ScreamWhilePounced ) / Player.ScreamWhilePounced |
| `10241030` | bagian_003.c:91007 | 124 | OnSpawnHunter / m_OnSpawn[ZOMBIE_HUNTER] |
| `1027e920` | bagian_004.c:27866 | 99 | lunges / pounces |
| `102836c0` | bagian_004.c:30308 | 67 | lunges / pounces |
| `102859b0` | bagian_004.c:31854 | 44 | HunterZombie.Pain / HunterZombie.PainShort |
| `10248140` | bagian_003.c:95676 | 13 | %s Music::OnPounceEnded / Event.HunterPounce |
| `102a4840` | bagian_004.c:51462 | 1302 | HunterZombie.Alert |
| `10244580` | bagian_003.c:93339 | 536 | pounce_stopped |
| `10354130` | bagian_005.c:37303 | 398 | pounce_stopped |
| `10233660` | bagian_003.c:83163 | 333 | %d Hunters  |

### Smoker (58 fungsi)

| Alamat | Lokasi | Baris | Teks penanda |
|---|---|---|---|
| `10002240` | bagian_000.c:932 | 2108 | ACT_TERROR_CHOKING_TONGUE_GROUND / ACT_TERROR_CHOKING_TONGUE_GROUND_FEMALE / ACT_TERROR_CHOKING_TONGUE_GROUND_MALE |
| `10292d40` | bagian_004.c:40680 | 164 | Knife.BreakTongue / Player.Choke / SmokerZombie.Alert |
| `1027fc20` | bagian_004.c:28634 | 167 | m_checkpointPZSmokerDamage / m_initialTonguePullDir / m_isHangingFromTongue |
| `102c9bc0` | bagian_004.c:71158 | 34 | m_tongueGrabStartingHealth / m_tongueHitRange / m_tongueHitTimestamp |
| `1029ab10` | bagian_004.c:45052 | 698 | SmokerZombie.Death / SmokerZombie.Explode / smoker_smokecloud |
| `102cb820` | bagian_004.c:72011 | 147 | %s neutralized the Smoker\'s Tongue attack! / PlayerGrabbedByTongue / PlayerNeutralizedTongue |
| `10354130` | bagian_005.c:37303 | 398 | Tongue letting go for a melee. / choke_stopped / tongue_pull_stopped |
| `102ea990` | bagian_004.c:87989 | 251 | Rescuing friend ledge-hanging from a Smoker Tongue! / SurvivorBotMoveToSaveTonguedFriend / Trying to dislodge Smoker-tongued friend! |
| `102ca0c0` | bagian_004.c:71193 | 64 | Tongue letting go for taking too much damage. / choke_stopped / tongue_pull_stopped |
| `102492b0` | bagian_003.c:96292 | 19 | %s Music::OnPulledByTongue / Event.SmokerDrag / Event.SmokerDragHit |
| `102480c0` | bagian_003.c:95661 | 14 | %s Music::OnReleasedByTongue / Event.SmokerChoke / Event.SmokerDrag |
| `102b0ac0` | bagian_004.c:56392 | 350 | HangingFromTongue / ZombiePresentSmoker |
| `10233660` | bagian_003.c:83163 | 333 | %d Smokers  / %d Smokers (%d max) |
| `102cced0` | bagian_004.c:72905 | 152 | SmokerZombie.TongueAttack / SmokerZombie.TongueFly |
| `102b1f70` | bagian_004.c:57164 | 132 | SmokerZombie.Attack / SmokerZombie.Breathe |
| `10241030` | bagian_003.c:91007 | 124 | OnSpawnSmoker / m_OnSpawn[ZOMBIE_SMOKER] |
| `102cc940` | bagian_004.c:72751 | 123 | Tongue not targeting anyone. / Tongue targeting new victim. |
| `1028e950` | bagian_004.c:37915 | 110 | Tongue letting go on getting bashed. / tongue_release |
| `1027e920` | bagian_004.c:27866 | 99 | chokes / pulls |
| `10289350` | bagian_004.c:34579 | 86 | tongue_grab / tongue_release |
| `102836c0` | bagian_004.c:30308 | 67 | chokes / pulls |
| `102ca3a0` | bagian_004.c:71298 | 53 | (TONGUE) %s  Smoker: %s. Victim: %s. / (TONGUE) %s  Smoker:%s. Victim:%s. |
| `102859b0` | bagian_004.c:31854 | 44 | SmokerZombie.Pain / SmokerZombie.PainShort |
| `102cb6f0` | bagian_004.c:71967 | 43 | PlayerTonguePullStart / SmokerZombie.TongueRetract |
| `102ca2a0` | bagian_004.c:71258 | 39 | (TONGUE) %s  Smoker: %s / (TONGUE) %s  Smoker:%s. |

### Boomer (55 fungsi)

| Alamat | Lokasi | Baris | Teks penanda |
|---|---|---|---|
| `10292d40` | bagian_004.c:40680 | 164 | BoomerZombie.Alert / BoomerZombie.Pain / BoomerZombie.PainShort |
| `1027fc20` | bagian_004.c:28634 | 167 | m_checkpointPZBoomerDamage / m_checkpointPZVomited / m_vomitFadeStart |
| `1029ab10` | bagian_004.c:45052 | 698 | BoomerZombie.Detonate / boomer_explode / boomer_exploded |
| `102b1f70` | bagian_004.c:57164 | 132 | BoomerZombie.Alert / BoomerZombie.Attack / BoomerZombie.Groan |
| `1028f1f0` | bagian_004.c:38251 | 120 | #L4D_NOTIFY_VOMIT_ON / PlayerNeutralizedVomit / PlayerVomitInFace |
| `103102d0` | bagian_005.c:1188 | 102 | Lost my vomit target / No vomit ability! / Vomit completed |
| `10249230` | bagian_003.c:96271 | 20 | %s Music::OnVomitedUpon / Event.BoomerHit / Event.VomitInTheFace |
| `10002240` | bagian_000.c:932 | 2108 | ACT_VM_VOMIT / ACT_VM_VOMIT_LAYER |
| `10233660` | bagian_003.c:83163 | 333 | %d Boomers  / %d Boomers (%d max) |
| `102dd910` | bagian_004.c:80350 | 267 | BoomerZombie.Detonate / boomer_explode |
| `1030f4e0` | bagian_005.c:794 | 250 | I\'m exposed and my vomit is recharging / Launching vomit attack! |
| `10241030` | bagian_003.c:91007 | 124 | OnSpawnBoomer / m_OnSpawn[ZOMBIE_BOOMER] |
| `102859b0` | bagian_004.c:31854 | 44 | BoomerZombie.Pain / BoomerZombie.PainShort |
| `102a4840` | bagian_004.c:51462 | 1302 | BoomerZombie.Alert |
| `102994b0` | bagian_004.c:44248 | 433 | BoomerZombie.Gurgle |
| `10354130` | bagian_005.c:37303 | 398 | Weapon.HitBoomer |
| `102fc000` | bagian_004.c:97835 | 364 | %3.2f: %s: NOT FIRING because we would pop the Boomer on a teammate |
| `102b0ac0` | bagian_004.c:56392 | 350 | ZombiePresentBoomer |
| `102bed00` | bagian_004.c:65182 | 306 | zombieclass:BOOMER |
| `102d35e0` | bagian_004.c:75901 | 248 | boomer |
| `102dd510` | bagian_004.c:80069 | 213 | boomer |
| `10296ab0` | bagian_004.c:42416 | 186 | PlayerWarnBoomer |
| `10297f50` | bagian_004.c:43246 | 145 | weapon_boomer_claw |
| `102975e0` | bagian_004.c:42809 | 144 | boomer |
| `102d18f0` | bagian_004.c:74866 | 143 | boomer |

### Tank (51 fungsi)

| Alamat | Lokasi | Baris | Teks penanda |
|---|---|---|---|
| `10002240` | bagian_000.c:932 | 2108 | ACT_HULK_ATTACK_LOW / ACT_HULK_THROW / ACT_TERROR_HULK_VICTORY |
| `10233660` | bagian_003.c:83163 | 333 | %d Tanks (%d active) / %d Tanks (%d max) Interval: %f / Intervals: Horde: %f, Specials: %f, Tank: %f, Lull: %f |
| `1022c3d0` | bagian_003.c:78309 | 191 | TANK OFFER: Looking for players / TANK OFFER: No tickets, choosing randomly / TANK OFFER: Player %s has %d tickets |
| `10292d40` | bagian_004.c:40680 | 164 | HulkZombie.Pain / HulkZombie.PainFire / HulkZombie.Thud |
| `102b1f70` | bagian_004.c:57164 | 132 | HulkZombie.Breathe / HulkZombie.Growl / HulkZombie.PainFire |
| `10227260` | bagian_003.c:74514 | 202 | %3.2f: Director: Final Tank has FAILED to spawn! Giving up and startin / %3.2f: Director: Final Tank has spawned. / %3.2f: Director: Halftime Tank has spawned. |
| `102c6570` | bagian_004.c:69569 | 87 | HulkZombie.Throw.FlyLoop / tank_rock_throw_impact / tank_rock_throw_impact_stump |
| `102c7100` | bagian_004.c:69836 | 73 | Tank rock %s / tank_rock_throw_rock / tank_rock_throw_stump |
| `10223240` | bagian_003.c:71568 | 47 | Tank offer: Going to the only PZ / Tank offer: Starting the lottery / Tank offer: making a bot, and giving him to nobody |
| `10238cd0` | bagian_003.c:86391 | 178 | Allow Tanks: %d / Spawn Witch and Tank ? %d  |
| `1022cea0` | bagian_003.c:78658 | 157 | [HOLDOUT]: TANK ATTACK! Tanks this spawn: %d (Tank Events: %d) / [HOLDOUT]: Tank Stage interval: %f |
| `102bed00` | bagian_004.c:65182 | 306 | zombieclass:TANK |
| `1021dfa0` | bagian_003.c:67382 | 287 | Turtle Rock Studios |
| `1030b250` | bagian_004.c:106337 | 226 | Tank committing suicide |
| `10296ab0` | bagian_004.c:42416 | 186 | PlayerWarnHulk |
| `10300290` | bagian_004.c:100301 | 181 | A Tank is coming |
| `102286e0` | bagian_003.c:75432 | 168 | Update Versus Boss Spawning: Highest survivor flow distance = %f  Tank |
| `1027fc20` | bagian_004.c:28634 | 167 | m_frustration |
| `10159270` | bagian_002.c:31741 | 147 | tank_rock |
| `1030adf0` | bagian_004.c:106189 | 147 | Throwing rock |
| `100b9540` | bagian_001.c:22627 | 145 | tank_rock |
| `10343820` | bagian_005.c:29565 | 136 | tank_rock |
| `1028eea0` | bagian_004.c:38116 | 134 | HulkZombie.Thud |
| `102b6020` | bagian_004.c:59693 | 129 | tank_frustrated |
| `102259f0` | bagian_003.c:73315 | 124 | Init Versus Boss Spawning: MinFlow %.2f MaxFlow %.2f - Team 0: Tank %. |

### Witch (66 fungsi)

| Alamat | Lokasi | Baris | Teks penanda |
|---|---|---|---|
| `10333f90` | bagian_005.c:21150 | 29 | WitchZombie.Despair / WitchZombie.Die / WitchZombie.GrowlHigh |
| `10334d80` | bagian_005.c:21630 | 47 | ACT_TERROR_WITCH_COVER_EARS_BEGIN / ACT_TERROR_WITCH_COVER_EARS_END / ACT_TERROR_WITCH_COVER_EARS_LOOP |
| `10002240` | bagian_000.c:932 | 2108 | ACT_TERROR_WITCH_ANGRY / ACT_TERROR_WITCH_ANGRY_HIGH / ACT_TERROR_WITCH_IDLE |
| `1022f790` | bagian_003.c:80372 | 177 | Switching Survivor Bot \'%s\' from team %d to SPECTATOR / Switching Survivor \'%s\' from team %d to ZOMBIE / Switching Zombie \'%s\' from team %d to SURVIVOR %s |
| `1024aaa0` | bagian_003.c:96927 | 17 | %s Music::OnWitchAttackBurning / Event.WitchAttack / Event.WitchBurning |
| `1024abb0` | bagian_003.c:96962 | 17 | %s Music::OnWitchKilled / Event.WitchAttack / Event.WitchBurning |
| `1024b400` | bagian_003.c:97228 | 852 | Event.WitchAttack / Event.WitchBurning / Event.WitchRage |
| `103058f0` | bagian_004.c:102936 | 152 | %3.2f: %s switching to closer victim / %3.2f: %s switching to directly visible victim / %3.2f: %s switching to mobile victim |
| `10335420` | bagian_005.c:21902 | 108 | Witch / WitchZombie.Pain / WitchZombie.PainFire |
| `10335da0` | bagian_005.c:22149 | 71 | WitchZombie.GrowlHigh / WitchZombie.GrowlLow / WitchZombie.GrowlMedium |
| `10337ca0` | bagian_005.c:23074 | 55 | WitchZombie.Die / witch_killed / witchid |
| `1024ab30` | bagian_003.c:96945 | 16 | %s Music::OnWitchAttackDone / Event.WitchAttack / Event.WitchBurning |
| `10249430` | bagian_003.c:96354 | 15 | %s Music::OnWitchAttacking / Event.WitchAttack / Event.WitchRage |
| `100a1360` | bagian_001.c:9222 | 448 | InputSwitchOverlay / SwitchOverlay |
| `10336c10` | bagian_005.c:22459 | 180 | WitchZombie.Rage / WitchZombie.RageBeforeKillingFrenzy |
| `10238cd0` | bagian_003.c:86391 | 178 | Allow Witches: %d / Spawn Witch and Tank ? %d  |
| `101bdf90` | bagian_003.c:6799 | 131 | autoswitchfrom / autoswitchto |
| `10241030` | bagian_003.c:91007 | 124 | OnSpawnWitch / m_OnSpawn[ZOMBIE_WITCH] |
| `10333970` | bagian_005.c:20938 | 73 | InfectedWander::Update / Zombie.Wander |
| `10334a40` | bagian_005.c:21490 | 53 | Tongue letting go for a witch cutting it. / WitchZombie.GrowlHigh |
| `101dbde0` | bagian_003.c:25356 | 38 | Bot.EditSwitchOff / Bot.EditSwitchOn |
| `10334c80` | bagian_005.c:21591 | 38 | witch_harasser_set / witchid |
| `101ddda0` | bagian_003.c:26705 | 29 | Bot.EditSwitchOff / Bot.EditSwitchOn |
| `1021f7d0` | bagian_003.c:68634 | 29 | witch_and_tank / witch_chance |
| `10244580` | bagian_003.c:93339 | 536 | witch_harasser_set |

### Survivor bot vs infected (146 fungsi)

| Alamat | Lokasi | Baris | Teks penanda |
|---|---|---|---|
| `10002240` | bagian_000.c:932 | 2108 | ACT_DIE_INCAP / ACT_IDLE_INCAP / ACT_IDLE_INCAP_ELITES |
| `1027fc20` | bagian_004.c:28634 | 167 | m_checkpointIncaps / m_checkpointPZIncaps / m_checkpointSurvivorDamage |
| `102b0ac0` | bagian_004.c:56392 | 350 | Incapacitated / IncapacitatedCount / IsIncapacitated |
| `10292d40` | bagian_004.c:40680 | 164 | Weapon.HitSurvivor / glow_boomer_it_victim / glow_survivor_ability_target |
| `10260c20` | bagian_004.c:9807 | 465 | (INCAP)%s was incapped by %s with \"%s\" / Team \"%s\" triggered \"%s\" (Infected \"%i\") (Survivor \"%i\") / Team \"Survivor\" scored \"%i\" with \"%i\" players |
| `1022f790` | bagian_003.c:80372 | 177 | Switching Survivor Bot \'%s\' from team %d to SPECTATOR / Switching Survivor \'%s\' from team %d to ZOMBIE / Switching Zombie \'%s\' from team %d to SURVIVOR %s |
| `102516f0` | bagian_004.c:610 | 570 | Respawning dead survivor %s / incapacitated / missionSurvivorDamage |
| `1032efc0` | bagian_005.c:18634 | 217 | InfectedAttack::ChaseVictim::Update / Victim died / Victim pointer is NULL |
| `102aeca0` | bagian_004.c:55842 | 194 | m_hasAnySurvivorLeftSafeArea / m_isIncapacitated / m_isSurvivorTeamReadyTime |
| `10336c10` | bagian_005.c:22459 | 180 | Killed our Survivor victim / Lost our victim / Retreating in horror from killing a Survivor |
| `1029ab10` | bagian_004.c:45052 | 698 | ,SubjectTeam:Survivor / SurvivorDied / victim |
| `10244580` | bagian_003.c:93339 | 536 | player_incapacitated_start / survivor_rescued / victim |
| `10233660` | bagian_003.c:83163 | 333 | Survivor ready time: %0.0f seconds / TANK IN PLAY - SURVIVORS RUNNING FOR CHECKPOINT / Waiting for survivors |
| `102ea990` | bagian_004.c:87989 | 251 | SurvivorBotMoveToSavePouncedFriend / SurvivorBotMoveToSaveTonguedFriend / SurvivorBotMovingToReviveFriend |
| `1032e710` | bagian_005.c:18297 | 203 | InfectedAttack::PunchVictim::Update / Watch our victim / Zombie.RageAtVictim |
| `102e6d00` | bagian_004.c:85866 | 166 | %3.2f: ERROR: SurvivorBot( %s ) stuck here ( %3.2f, %3.2f, %3.2f ) - f / %3.2f: ERROR: SurvivorBot( %s ) stuck here ( %3.2f, %3.2f, %3.2f ) - f / %3.2f: ResolveStuckSituation ERROR: Cannot resolve SurvivorBot( %s ) s |
| `102eb060` | bagian_004.c:88241 | 153 | SurvivorBotMoveToRescueFriendFromCloset / SurvivorBotMovingToReviveFriend / info_survivor_rescue |
| `103058f0` | bagian_004.c:102936 | 152 | %3.2f: %s switching to closer victim / %3.2f: %s switching to directly visible victim / %3.2f: %s switching to mobile victim |
| `10231100` | bagian_003.c:81524 | 107 | info_survivor_rescue / rescue spot %d(%s) closes door to start calling for help with nearest  / rescue spot %d(%s) starts calling for help for %s with nearest survivo |
| `102c9bc0` | bagian_004.c:71158 | 34 | m_tongueVictimDistance / m_tongueVictimLastOnGroundTime / m_tongueVictimPositionTime |
| `10237ef0` | bagian_003.c:85930 | 460 | SURVIVOR / Printing survivor items: |
| `10354130` | bagian_005.c:37303 | 398 | Weapon.HitSurvivor / victim |
| `102b3c40` | bagian_004.c:58350 | 275 | %s was incapacitated and stops reviving %s / blood_incapacitated |
| `102d35e0` | bagian_004.c:75901 | 248 | player_incapacitated / victim |
| `1029eeb0` | bagian_004.c:47434 | 201 | ,IncapacitatedCharacter: / PlayerIncapacitated |

## 3. Kelas C++ terkait zombie dari RTTI (160)

`Boomer`, `BoomerAttack`, `BoomerBehavior`, `BoomerFilter`, `BoomerHide`, `BoomerIntention`, `BoomerLocomotion`, `BoomerRetreatToCover`, `BoomerVomitOnVictim`, `BossZombiePlayerBot`, `CBoomerBlunderAwardTemplate`, `CBoomerClaw`, `CBoomerComboAwardTemplate`, `CCSHLTVDirector`, `CCommentaryZombieSpawner`, `CFuncPlayerInfectedClip`, `CFuncTankTrain`, `CHLTVDirector`, `CHeadhunterAwardTemplate`, `CHunterClaw`, `CInfectedAnimationLayer`, `CInfectedZooMaker`, `CInfectedZooPuppet`, `CInfoDirector`, `CInfoGoalInfectedChase`, `CInfoZombieSpawn`, `CItSurvivorAwardTemplate`, `CLunge`, `CNoSurvivorsAwardTemplate`, `COxygenTank`, `CPounceAwardTemplate`, `CPropaneTank`, `CSmokerClaw`, `CSurvivorCamera`, `CSurvivorGroupInfo`, `CSurvivorKillAwardTemplate`, `CSurvivorPosition`, `CSurvivorRescue`, `CTankClaw`, `CTankRock`, `CTankTargetChange`, `CTankTrainAI`, `CTongue`, `CTongueChokeAwardTemplate`, `CTraceFilterSimpleListNoInfectedOrGhosts`, `CTraceFilterTongue`, `CVomit`, `CVomitHitAwardTemplate`, `CVomitParticle`, `CVomitStopAwardTemplate`, `CWitchHarassmentAwardTemplate`, `CWitchSlayerAwardTemplate`, `Director`, `FilterInfectedClass`, `Hunter`, `HunterAttack`, `HunterBehavior`, `HunterIntention`, `HunterLeapToCover`, `HunterLocomotion`, `HunterLungeAtVictim`, `IHLTVDirector`, `Infected`, `InfectedAlert`, `InfectedAttack`, `InfectedBehavior`, `InfectedBlinded`, `InfectedBurn`, `InfectedChangePosture`, `InfectedChasePath`, `InfectedDying`, `InfectedExecAction`, `InfectedIntention`, `InfectedLeanOnWall`, `InfectedLieDown`, `InfectedPathCost`, `InfectedShoved`, `InfectedSitDown`, `InfectedStaggerAround`, `InfectedStandDazed`, `InfectedStandingActivity`, `InfectedWander`, `Smoker`, `SmokerAttack`, `SmokerBehavior`, `SmokerIntention`, `SmokerLocomotion`, `SmokerMoveToAttackPosition`, `SmokerPathCost`, `SmokerRetreatToCover`, `SmokerTongueVictim`, `SurvivorAmbushBoomer`, `SurvivorAttack`, `SurvivorBehavior`, `SurvivorBody`, `SurvivorBot`, `SurvivorBotMeleeOnReply`, `SurvivorBotPathCost`, `SurvivorBotUseOnReply`, `SurvivorCheckpointLeaving`, `SurvivorCheckpointReached`, `SurvivorCloseDoor`, `SurvivorCollectObject`, `SurvivorDebugApproach`, `SurvivorDislodgeVictim`, `SurvivorElevatorEnter`, `SurvivorElevatorRide`, `SurvivorEscapeFlames`, `SurvivorEscapeLadderAmbush`, `SurvivorGivePillsToFriend`, `SurvivorHealFriend`, `SurvivorHealSelf`, `SurvivorIntention`, `SurvivorLegsApproach`, `SurvivorLegsApproachElevator`, `SurvivorLegsBattleStations`, `SurvivorLegsCoverFriendInCombat`, `SurvivorLegsCoverOrphan`, `SurvivorLegsEnterRescueVehicle`, `SurvivorLegsMeetRescueVehicleArrival`, `SurvivorLegsMoveOn`, `SurvivorLegsRegroup`, `SurvivorLegsRetreat`, `SurvivorLegsStayClose`, `SurvivorLegsWait`, `SurvivorLiberateBesiegedFriend`, `SurvivorLocomotion`, `SurvivorRescueBarricadedFriend`, `SurvivorReviveFriend`, `SurvivorTakePills`, `SurvivorTeamSituation`, `SurvivorUseObject`, `SurvivorVision`, `Tank`, `TankAttack`, `TankBehavior`, `TankIdle`, `TankIntention`, `TankLocomotion`, `TraceFilterIgnoreInfected`, `Witch`, `WitchAngry`, `WitchAttack`, `WitchBehavior`, `WitchBody`, `WitchBurn`, `WitchDying`, `WitchExecAction`, `WitchIdle`, `WitchIntention`, `WitchKillIncapVictim`, `WitchLocomotion`, `WitchRetreat`, `WitchSlashEnumerator`, `WitchVision`, `ZombieBotBody`, `ZombieBotCollisionTraceFilter`, `ZombieBotLocomotion`, `ZombieBotVision`, `ZombieManager`

## 4. Nama mirip pengaturan di dalam DLL (677)

Diambil dari teks mentah DLL, jadi sebagian bukan pengaturan (misalnya nama event atau suara).
Kode pendaftarannya tidak tertangkap di pseudo-C.

`boomer_claw`, `boomer_explode`, `boomer_exploded`, `boomer_exposed_time_tolerance`, `boomer_near`, `boomer_pz_claw_dmg`, `boomer_vomit_delay`, `boomer_vomit_screeneffect`, `director_afk_timeout`, `director_ai_tanks`, `director_build_up_min_interval`, `director_convert_pills`, `director_convert_pills_critical_health`, `director_debug`, `director_debug_scavenge_items`, `director_debug_threat_placement`, `director_finale_infinite`, `director_finale_item_cluster_count`, `director_finale_max_loops`, `director_finale_mob_max_interval`, `director_finale_mob_min_interval`, `director_finale_mob_relax_interval`, `director_finale_stage_delay`, `director_force_background`, `director_force_panic_event`, `director_force_tank`, `director_force_versus_start`, `director_force_witch`, `director_garage_pistol_density`, `director_garage_weapon_density`, `director_gas_can_density`, `director_ingress_range`, `director_intensity_relax_allow_wanderers_threshold`, `director_intensity_relax_allow_wanderers_threshold_expert`, `director_intensity_relax_allow_wanderers_threshold_hard`, `director_intensity_relax_threshold`, `director_intensity_threshold`, `director_item_cluster_range`, `director_log_boss_threats`, `director_log_scavenge_items`, `director_min_start_players`, `director_molotov_density`, `director_must_create_all_scavenge_items`, `director_no_bosses`, `director_no_death_check`, `director_no_mobs`, `director_no_specials`, `director_no_survivor_bots`, `director_oxygen_tank_density`, `director_pain_pill_density`, `director_panic_forever`, `director_pipe_bomb_density`, `director_pistol_density`, `director_print_player_counts`, `director_propane_tank_density`, `director_race_mode`, `director_ready_duration`, `director_ready_radius`, `director_relax_max_flow_travel`, `director_relax_max_interval`, `director_relax_min_interval`, `director_report`, `director_report_screen_delay`, `director_scavenge_item_override`, `director_short_finale`, `director_solo_load`, `director_solo_mode`, `director_special_battlefield_respawn_interval`, `director_special_finale_offer_length`, `director_special_initial_spawn_delay_max`, `director_special_initial_spawn_delay_max_extra`, `director_special_initial_spawn_delay_min`, `director_special_original_offer_length`, `director_special_respawn_interval`, `director_spectate_specials`, `director_start`, `director_stop`, `director_sustain_peak_max_time`, `director_sustain_peak_min_time`, `director_swap_teams`, `director_tank_bypass_max_flow_travel`, `director_tank_checkpoint_interval`, `director_tank_force_offer`, `director_tank_lottery_entry_time`, `director_tank_lottery_selection_time`, `director_tank_max_interval`, `director_tank_min_interval`, `director_tank_offer_debug`, `director_threat_clear_radius`, `director_threat_max_separation`, `director_threat_min_separation`, `director_threat_radius`, `director_transition_timeout`, `director_unfreeze_time`, `director_vs_convert_pills`, `director_weapon_density`, `tank_attack_range`, `tank_auto_swing`, `tank_burn_duration_expert`, `tank_burn_duration_hard`, `tank_burn_duration_normal`, `tank_burn_duration_vs`, `tank_chance`, `tank_claw`, `tank_fist_radius`, `tank_frustrated`, `tank_ground_pound_duration`, `tank_ground_pound_reveal_distance`, `tank_killed`, `tank_object_throw`, `tank_projectile`, `tank_pz_forward`, `tank_raffle_debug`, `tank_rock`, `tank_rock_overhead_percent`, `tank_rock_throw_impact`, `tank_rock_throw_impact_stump`, `tank_rock_throw_rock`, `tank_rock_throw_stump`, `tank_spawn`, `tank_stasis_time_suicide`, `tank_stuck_failsafe`, `tank_stuck_time_choose_new_target`, `tank_stuck_time_suicide`, `tank_stuck_visibility_tolerance_choose_new_target`, `tank_stuck_visibility_tolerance_suicide`, `tank_survivor_pound`, `tank_swing_arc`, `tank_swing_duration`, `tank_swing_fast_interval`, `tank_swing_interval`, `tank_swing_miss_interval`, `tank_swing_physics_prop_force`, `tank_swing_range`, `tank_swing_yaw`, `tank_throw_aim_error`, `tank_throw_allow_range`, `tank_throw_lead_time_factor`, `tank_throw_loft_rate`, `tank_throw_max_loft_angle`, `tank_throw_min_interval`, `tank_visibility_tolerance_suicide`, `tank_windup_time`, `tongue_allow_voluntary_release`, `tongue_angle`, `tongue_bend_point_deflection`, `tongue_break_from_damage_amount`, `tongue_bullet_radius`, `tongue_choke_damage_amount`, `tongue_choke_damage_interval`, `tongue_cone_start_tolerance`, `tongue_debug`, `tongue_drag_damage_amount`, `tongue_dropping_to_ground_time`, `tongue_fire_hit_mulligan`, `tongue_fly_speed`, `tongue_force_break`, `tongue_grab`, `tongue_gravity_force`, `tongue_health`, `tongue_hit_delay`, `tongue_kill_smoker_on_detach`, `tongue_los_forgiveness_time`, `tongue_miss_delay`, `tongue_no_progress_break_interval`, `tongue_no_progress_choke_early_ambush_delay`, `tongue_no_progress_choke_early_delay`, `tongue_no_progress_choke_time`, `tongue_no_progress_damage_interval`, `tongue_no_progress_tolerance`, `tongue_player_dropping_to_ground_time`, `tongue_pull_stopped`, `tongue_range`, `tongue_release`, `tongue_release_fatigue_penalty`, `tongue_start_pull_delay`, `tongue_unbend`, `tongue_vertical_choke_dot`, `tongue_vertical_choke_height`, `tongue_vertical_choke_time_off_ground`, `tongue_victim_acceleration`, `tongue_victim_accuracy_penalty`, `tongue_victim_max_speed`, `tongue_vs_cone_start_tolerance`, `z_abort`, `z_acquire_far_range`, `z_acquire_far_time`, `z_acquire_near_range`, `z_acquire_near_time`, `z_acquire_time_variance_factor`, `z_add`, `z_added_light_max`, `z_added_light_min`, `z_alert_dot`, `z_alert_range`, `z_allow_ai_to_use_abilities`, `z_anim_idle_speed_max`, `z_anim_idle_speed_min`, `z_attack_change_target_range`, `z_attack_flow_range`, `z_attack_incapacitated_damage`, `z_attack_interval`, `z_attack_max_range`, `z_attack_min_range`, `z_attack_movement_penalty`, `z_attack_on_the_run_range`, `z_attention_range_lying_modifier`, `z_attention_range_sitting_modifier`, `z_avoid_max_range`, `z_avoid_min_range`, `z_avoid_power`, `z_avoidforce`, `z_award_debug`, `z_background_limit`, `z_backspeed`, `z_bbq_min`, `z_bbq_rate`, `z_blend_attack_layer`, `z_boomer_near_dist`, `z_boss_crouch`, `z_boundary_clear_type`, `z_boundary_max_range`, `z_boundary_spread_speed`, `z_brawl_chance`, `z_breakable_damage`, `z_burn_max`, `z_burn_rate`, `z_burning_lifetime`, `z_carry_max_mass`, `z_chance`, `z_checkpoint_debug`, `z_claw_hit_pitch_max`, `z_claw_hit_pitch_min`, `z_claw_hit_yaw_max`, `z_claw_hit_yaw_min`, `z_clear_area_range`, `z_clear_max_time`, `z_clear_min_time`, `z_clear_min_time_range`, `z_clear_range`, `z_close_target_notice_distance`, `z_common_limit`, `z_cooldown_spawn_safety_range`, `z_cough_cloud_expire`, `z_cough_cloud_initial_cough_delay`, `z_cough_cloud_radius`, `z_cough_duration`, `z_cough_fatigue`, `z_cough_interval`, `z_credits_interval`, `z_crouch_speed`, `z_cull_timeout`, `z_damage_screen_fade_alpha`, `z_damage_screen_fade_duration`, `z_deafen_radius_one`, `z_deafen_radius_three`, `z_deafen_radius_two`, `z_debug`, `z_debug_activities`, `z_debug_avoid`, `z_debug_breakables`, `z_debug_climb`, `z_debug_cull`, `z_debug_escape_scan`, `z_debug_falling_damage`, `z_debug_fog`, `z_debug_infected_anim_report`, `z_debug_infected_server_anim`, `z_debug_mob_spawn`, `z_debug_neighbors`, `z_debug_path_stress_test`, `z_debug_population`, `z_debug_shared_random`, `z_debug_spawn_ahead`, `z_debug_spawn_set`, `z_debug_spawn_volumes`, `z_debug_spawnable_areas`, `z_debug_tank_spawn`, `z_decals`, `z_density_region_length`, `z_destroy_on_attack`, `z_difficulty`, `z_difficulty_locked`, `z_director_special_spawn_delay`, `z_discard_min_range`, `z_discard_range`, `z_do_tracers`, `z_dont_clear`, `z_door_pound_damage`, `z_door_reopen_interval`, `z_door_retry_interval`, `z_elevator_in_air`, `z_exploding_force`, `z_exploding_health`, `z_exploding_inner_radius`, `z_exploding_limit`, `z_exploding_outer_radius`, `z_exploding_shove_interval`, `z_exploding_shove_max`, `z_exploding_shove_min`, `z_exploding_speed`, `z_exploding_splat`, `z_exploding_splat_radius`, `z_expressions`, `z_falling_land_forward_speed`, `z_falling_land_hard_speed`, `z_falling_land_speed`, `z_finale_chance`, `z_finale_spawn_safety_range`, `z_first_aid_cabinets`, `z_force_attack_from_sound_range`, `z_forcezombiemodel`, `z_forcezombiemodelname`, `z_forwardspeed`, `z_friendly_fire_forgiveness`, `z_frustration`, `z_frustration_lifetime`, `z_frustration_los_delay`, `z_frustration_spawn_delay`, `z_gas_health`, `z_gas_limit`, `z_gas_speed`, `z_ghost_ahead_flow`, `z_ghost_allowed_in_battlefield`, `z_ghost_checkpoint_spawn_interval`, `z_ghost_cooldown`, `z_ghost_delay_max`, `z_ghost_delay_min`, `z_ghost_delay_minspawn`, `z_ghost_duration`, `z_ghost_finale_spawn_interval`, `z_ghost_friction`, `z_ghost_group_spawn`, `z_ghost_line_of_scrimmage_far`, `z_ghost_line_of_scrimmage_near`, `z_ghost_line_of_scrimmage_safety`, `z_ghost_line_of_scrimmage_threat_bonus`, `z_ghost_los_expected_progress`, `z_ghost_offer_acceptance_time`, `z_ghost_offer_spawn_safety_time`, `z_ghost_spawn_distance`, `z_ghost_spawn_in_start`, `z_ghost_spawn_interval`, `z_ghost_speed`, `z_ghost_travel_distance`, `z_gib_limb_distance`, `z_gib_limb_distance_buckshot`, `z_gib_limb_distance_headshot`, `z_gib_limb_distance_headshot_zoomed`, `z_gib_limb_distance_zoomed`, `z_gib_limbs`, `z_grab_force`, `z_grab_ledges_solo`, `z_gun_damage`, `z_gun_debug_player_index`, `z_gun_force`, `z_gun_horiz_punch`, `z_gun_kick`, `z_gun_physics_force`, `z_gun_range`, `z_gun_start_empty`, `z_gun_stun_duration`, `z_gun_survivor_force`, `z_gun_survivor_friend_push`, `z_gun_survivor_shove_dot`, `z_gun_swing_duration`, `z_gun_swing_interval`, `z_gun_vertical_punch`, `z_health`, `z_hear_gunfire_range`, `z_hear_runner_far_range`, `z_hear_runner_near_range`, `z_hit_from_behind_cosine`, `z_hit_from_behind_factor`, `z_hit_incap_factor_easy`, `z_hit_incap_factor_expert`, `z_hit_incap_factor_hard`, `z_hit_incap_factor_normal`, `z_hunter_claw_buildup_growl_percent`, `z_hunter_claw_force`, `z_hunter_claw_max_buildup_time`, `z_hunter_claw_plays_hit_anims`, `z_hunter_claw_range`, `z_hunter_claw_range_down`, `z_hunter_claw_start_buildup_time`, `z_hunter_claw_swing_duration`, `z_hunter_claw_swing_interval`, `z_hunter_claw_swing_miss_interval`, `z_hunter_ground_normal`, `z_hunter_health`, `z_hunter_limit`, `z_hunter_lunge_distance`, `z_hunter_lunge_pitch`, `z_hunter_lunge_stagger_time`, `z_hunter_max_pounce_bonus_damage`, `z_hunter_speed`, `z_increment_head`, `z_increment_lower`, `z_increment_skin`, `z_increment_upper`, `z_infected_flashlight`, `z_infected_invuln`, `z_infected_move`, `z_last_area_update_tolerance`, `z_last_man_run_interval`, `z_lean_wall_align_speed`, `z_look_at_local_player`, `z_lunge_cooldown`, `z_lunge_distance_damage`, `z_lunge_interval`, `z_lunge_power`, `z_lunge_reflect`, `z_lunge_release`, `z_lunge_specials`, `z_lunge_up`, `z_max_hunter_pounce_stagger_duration`, `z_max_neighbor_range`, `z_max_path_length`, `z_max_player_zombies`, `z_max_stagger_duration`, `z_max_survivor_damage`, `z_mega_mob_size`, `z_mega_mob_spawn_max_interval`, `z_mega_mob_spawn_min_interval`, `z_min_ladder_mount_dot`, `z_minigun_atomize`, `z_minigun_cooldown_time`, `z_minigun_damage_rate`, `z_minigun_debug`, `z_minigun_fire_anim`, `z_minigun_fire_anim_speed`, `z_minigun_fire_think_interval`, `z_minigun_firing_speed`, `z_minigun_overheat_time`, `z_minigun_rate_of_fire`, `z_minigun_spin_down_speed`, `z_minigun_spin_up_speed`, `z_minigun_spread`, `z_minigun_stand_distance`, `z_minion_aim_tolerance`, `z_minion_limit`, `z_mob_min_notify_count`, `z_mob_music_size`, `z_mob_population_density`, `z_mob_recharge_rate`, `z_mob_sacrifice_timeout`, `z_mob_spawn_finale_size`, `z_mob_spawn_max_interval_easy`, `z_mob_spawn_max_interval_expert`, `z_mob_spawn_max_interval_hard`, `z_mob_spawn_max_interval_normal`, `z_mob_spawn_max_size`, `z_mob_spawn_min_interval_easy`, `z_mob_spawn_min_interval_expert`, `z_mob_spawn_min_interval_hard`, `z_mob_spawn_min_interval_normal`, `z_mob_spawn_min_size`, `z_must_wander`, `z_mute_infected`, `z_nav_debug`, `z_no_cull`, `z_noise_level_display`, `z_noise_level_fade_rate`, `z_noise_level_footstep`, `z_noise_level_footstep_in_water`, `z_noise_level_hold_time`, `z_noise_level_max`, `z_noise_level_vocalize`, `z_non_head_damage_factor_easy`, `z_non_head_damage_factor_expert`, `z_non_head_damage_factor_hard`, `z_non_head_damage_factor_normal`, `z_notice_it_range`, `z_notice_near_range`, `z_player_lunge_up`, `z_player_zombie_debug`, `z_player_zombie_land_delay`, `z_player_zombie_min_dead_time`, `z_population`, `z_pounce_allow_partial_hidden`, `z_pounce_crouch_delay`, `z_pounce_damage`, `z_pounce_damage_delay`, `z_pounce_damage_interrupt`, `z_pounce_damage_interval`, `z_pounce_damage_range_max`, `z_pounce_damage_range_min`, `z_pounce_delay`, `z_pounce_door_damage`, `z_pounce_shake_amplitude`, `z_pounce_shake_duration`, `z_pounce_shake_radius`, `z_pounce_silence_range`, `z_pounce_stumble_force`, `z_pounce_stumble_radius`, `z_prevent_burrowing`, `z_puking_eye_height`, `z_push_force`, `z_push_mass_max`, `z_pushaway_force`, `z_ragdoll_discard_range`, `z_randombodygroups`, `z_randomskins`, `z_reload_chatter_debug`, `z_reload_chatter_intensity`, `z_reload_chatter_nearby_friend_range`, `z_reload_chatter_recent_enemy`, `z_reload_chatter_shotgun_ammo_threshold`, `z_reload_chatter_shotgun_interval`, `z_removeitems`, `z_reset_population_counter`, `z_respawn_distance`, `z_respawn_interval`, `z_restrict_team_change`, `z_round_start_attack_prevention`, `z_round_start_replacement_time`, `z_run_exertion_interval`, `z_safe_spawn_range`, `z_scout_mob_spawn_range`, `z_scrimmage_creep_delay`, `z_scrimmage_creep_rate`, `z_scrimmage_sphere`, `z_shotgun_bonus_damage_multiplier`, `z_shotgun_bonus_damage_range`, `z_shove_friend_speed`, `z_show_bottlenecks`, `z_show_clear`, `z_show_completely_visible_to_survivor_team`, `z_show_damaging`, `z_show_escape_route`, `z_show_flow_delta`, `z_show_flow_distance`, `z_show_infected`, `z_show_last_area`, `z_show_mutually_visible_set`, `z_show_population_density`, `z_show_potentially_visible`, `z_show_potentially_visible_to_survivor_team`, `z_show_swings`, `z_show_traffic`, `z_sidespeed`, `z_skirmish_spawn_max_interval`, `z_skirmish_spawn_max_size`, `z_skirmish_spawn_min_interval`, `z_skirmish_spawn_min_size`, `z_spawn`, `z_spawn_flow_limit`, `z_spawn_height`, `z_spawn_mobs_behind_chance`, `z_spawn_mobs_from_selected_set`, `z_spawn_range`, `z_spawn_safety_range`, `z_spawn_speed`, `z_spec_nightvision`, `z_special_spawn_interval`, `z_speed`, `z_splat_survivor_pitch_max`, `z_splat_survivor_pitch_min`, `z_splat_survivor_shake_amplitude`, `z_splat_survivor_shake_duration`, `z_splat_survivor_shake_frequency`, `z_splat_survivor_shake_radius`, `z_splat_survivor_yaw_max`, `z_splat_survivor_yaw_min`, `z_splatteralpha`, `z_splatterdistance`, `z_splatterfade`, `z_splatterhold`, `z_stagger_immunity_time`, `z_state_debug`, `z_stomp_always`, `z_stumble_max_curve_accel`, `z_stumble_max_curve_rate`, `z_survivor_respawn_health`, `z_tank_attack_interval`, `z_tank_autoshotgun_dmg_scale`, `z_tank_burning_lifetime`, `z_tank_damage_slow_max_range`, `z_tank_damage_slow_min_range`, `z_tank_footstep_shake_amplitude`, `z_tank_footstep_shake_duration`, `z_tank_footstep_shake_interval`, `z_tank_footstep_shake_radius`, `z_tank_grenade_damage`, `z_tank_grenade_roll`, `z_tank_grenade_slowdown`, `z_tank_has_special_blood`, `z_tank_health`, `z_tank_incapacitated_decay_rate`, `z_tank_incapacitated_health`, `z_tank_max_stagger_distance`, `z_tank_max_stagger_duration`, `z_tank_max_stagger_fade_duration`, `z_tank_rock_debug`, `z_tank_rock_radius`, `z_tank_speed`, `z_tank_speed_vs`, `z_tank_stagger_fade_alpha`, `z_tank_stagger_fade_duration`, `z_tank_throw_fail_interval`, `z_tank_throw_force`, `z_tank_throw_health`, `z_tank_throw_interval`, `z_tank_walk_speed`, `z_tanks_block_molotovs`, `z_terrify_duration`, `z_throttle_hit_interval_easy`, `z_throttle_hit_interval_expert`, `z_throttle_hit_interval_hard`, `z_throttle_hit_interval_normal`, `z_throwforce`, `z_tracer_particles`, `z_tracer_spacing`, `z_transitioning_players_remove`, `z_use_tolerance`, `z_versus_boomer_limit`, `z_versus_smoker_limit`, `z_versus_wandering_density`, `z_vision_range`, `z_vision_range_alert`, `z_vision_range_obscured`, `z_vision_range_obscured_alert`, `z_vocalize_burn_max_interval`, `z_vocalize_burn_min_interval`, `z_vocalize_shot_interval`, `z_vomit`, `z_vomit_boxsize`, `z_vomit_debug`, `z_vomit_drag`, `z_vomit_duration`, `z_vomit_fade_duration`, `z_vomit_fade_start`, `z_vomit_fatigue`, `z_vomit_float`, `z_vomit_hit_pitch_max`, `z_vomit_hit_pitch_min`, `z_vomit_hit_yaw_max`, `z_vomit_hit_yaw_min`, `z_vomit_lifetime`, `z_vomit_maxdamagedist`, `z_vomit_range`, `z_vomit_slide_mult`, `z_vomit_slide_rate`, `z_vomit_vecrand`, `z_vomit_velocity`, `z_vomit_velocityfadeend`, `z_vomit_velocityfadestart`, `z_walk_speed`, `z_wandering_density`, `z_witch_allow_change_victim`, `z_witch_anger_rate`, `z_witch_attack_range`, `z_witch_berserk_range`, `z_witch_burn_time`, `z_witch_damage`, `z_witch_damage_per_kill_hit`, `z_witch_discard_range`, `z_witch_flashlight_range`, `z_witch_health`, `z_witch_hostile_at_me_anger`, `z_witch_max_retreat_range`, `z_witch_max_threat_time`, `z_witch_min_retreat_range`, `z_witch_min_threat_time`, `z_witch_personal_space`, `z_witch_relax_rate`, `z_witch_retreat_exit_hidden_duration`, `z_witch_retreat_exit_range`, `z_witch_retreat_min_duration`, `z_witch_speed`, `z_witch_speed_inured`, `z_witch_threat_hostile_range`, `z_witch_threat_normal_range`, `z_witch_tongue_range`, `z_zombie_knockoff_death`, `z_zombie_lunge_push`, `z_zombieforcemodelname`
