// 10001000 FUN_10001000

char * FUN_10001000(void)

{
  char cVar1;
  HANDLE hHeap;
  char *lpFilename;
  char *pcVar2;
  char *pcVar3;
  DWORD dwFlags;
  SIZE_T dwBytes;
  _MEMORY_BASIC_INFORMATION _Stack_1c;
  
  cVar1 = (**(code **)(**(int **)g_pMemAlloc_exref + 100))();
  if (cVar1 != '\0') {
    dwBytes = 0x104;
    dwFlags = 0;
    hHeap = GetProcessHeap();
    lpFilename = (char *)HeapAlloc(hHeap,dwFlags,dwBytes);
    VirtualQuery(&DAT_100379ac,&_Stack_1c,0x1c);
    GetModuleFileNameA((HMODULE)_Stack_1c.AllocationBase,lpFilename,0x104);
    pcVar2 = _strrchr(lpFilename,0x2e);
    if (pcVar2 != (char *)0x0) {
      pcVar3 = _strrchr(lpFilename,0x5c);
      if (pcVar3 != (char *)0x0) {
        lpFilename = pcVar3 + 1;
        *pcVar2 = '\0';
      }
    }
    return lpFilename;
  }
  return (char *)0x0;
}


// 10001090 FUN_10001090

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_10001090(size_t param_1)

{
  void *pvVar1;
  
  if ((_DAT_100379b4 & 1) == 0) {
    _DAT_100379b4 = _DAT_100379b4 | 1;
    DAT_100379b0 = FUN_10001000();
  }
  if (DAT_100379b0 != (char *)0x0) {
    pvVar1 = (void *)(**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_100379b0,0);
    _memset(pvVar1,0,param_1);
    return pvVar1;
  }
  pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  _memset(pvVar1,0,param_1);
  return pvVar1;
}


// 10001110 FUN_10001110

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10001110(undefined4 param_1,undefined4 param_2)

{
  if ((_DAT_100379bc & 1) == 0) {
    _DAT_100379bc = _DAT_100379bc | 1;
    DAT_100379b8 = FUN_10001000();
  }
  if (DAT_100379b8 != (char *)0x0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 8))(param_1,param_2,DAT_100379b8,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(param_1,param_2);
  return;
}


// 100011f0 FUN_100011f0

void __cdecl FUN_100011f0(int param_1,int param_2)

{
  FUN_10001090(param_1 * param_2);
  return;
}


// 10001240 FUN_10001240

void __cdecl FUN_10001240(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x20))(param_1);
  return;
}


// 10001260 FUN_10001260

bool FUN_10001260(void)

{
  return *(int *)g_pMemAlloc_exref != 0;
}


// 10001270 FUN_10001270

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10001270(undefined4 param_1)

{
  if ((_DAT_100379b4 & 1) == 0) {
    _DAT_100379b4 = _DAT_100379b4 | 1;
    DAT_100379b0 = FUN_10001000();
  }
  if (DAT_100379b0 != (char *)0x0) {
    (**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_100379b0,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  return;
}


// 100012d0 FUN_100012d0

void __cdecl FUN_100012d0(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1);
  return;
}


// 10001300 FUN_10001300

int * __fastcall FUN_10001300(int param_1)

{
  int *piVar1;
  
  if (*(void **)(param_1 + 4) == (void *)0x0) {
    return (int *)0x0;
  }
  piVar1 = FUN_100058e0(*(void **)(param_1 + 4),"Missions",0);
  return piVar1;
}


// 10001330 FUN_10001330

undefined4 * __cdecl FUN_10001330(undefined4 *param_1,char *param_2)

{
  char *pcVar1;
  undefined4 *puVar2;
  int iVar3;
  
  puVar2 = param_1;
  pcVar1 = (char *)((int)param_1 + 5);
  *param_1 = &PTR_LAB_10025284;
  *(undefined1 *)(param_1 + 1) = 1;
  param_1 = (undefined4 *)((uint)param_1 & 0xffffff00);
  FUN_10009cf0(pcVar1,0xff,param_2,&stack0x0000000c,(int)&param_1);
  *(undefined1 *)(puVar2 + 0x41) = 0;
  if ((((char)param_1 != '\0') && (*(char *)(puVar2 + 1) == '\0')) && (DAT_100379c0 < 5)) {
    DAT_100379c0 = DAT_100379c0 + 1;
  }
  iVar3 = FUN_10009820(pcVar1);
  puVar2[0x42] = iVar3;
  return puVar2;
}


// 100013a0 FUN_100013a0

void __thiscall FUN_100013a0(void *this,undefined4 param_1,undefined4 param_2)

{
  undefined4 *puVar1;
  int *piVar2;
  int iVar3;
  void *this_00;
  undefined4 local_10c [67];
  
  puVar1 = FUN_10001330(local_10c,"GameModes/%s");
  piVar2 = FUN_100058e0(*(void **)((int)this + 4),(char *)((int)puVar1 + 5),0);
  if (piVar2 != (int *)0x0) {
    puVar1 = FUN_10005e40(piVar2);
    FUN_10005350(puVar1,param_2);
    piVar2 = FUN_100058e0(*(void **)((int)this + 4),"GameModes",0);
    FUN_10005a40(piVar2,puVar1);
  }
                    /* WARNING: Load size is inaccurate */
  iVar3 = (*(code *)**this)();
  if (iVar3 != 0) {
    this_00 = (void *)FUN_10005290();
    while (this_00 != (void *)0x0) {
      puVar1 = FUN_10001330(local_10c,"Modes/%s");
      piVar2 = FUN_100058e0(this_00,(char *)((int)puVar1 + 5),0);
      if (piVar2 != (int *)0x0) {
        puVar1 = FUN_10005e40(piVar2);
        FUN_10005350(puVar1,param_2);
        piVar2 = FUN_100058e0(this_00,"Modes",0);
        FUN_10005a40(piVar2,puVar1);
      }
      this_00 = (void *)FUN_100052c0();
    }
  }
  return;
}


// 10001490 FUN_10001490

void FUN_10001490(void)

{
  undefined **local_8;
  undefined4 local_4;
  
  DevMsg("Left 4 Dead 1 metadata:\n");
  local_8 = &PTR_LAB_1002528c;
  local_4 = 0;
  FUN_10005560(DAT_1003403c,(int *)&local_8,1);
  DevMsg("Left 4 Dead 1 metadata end.\n");
  return;
}


// 100014e0 FUN_100014e0

int * __thiscall FUN_100014e0(void *this,void *param_1,undefined4 *param_2)

{
  char *pcVar1;
  int iVar2;
  undefined4 *puVar3;
  void *this_00;
  int iVar4;
  int *piVar5;
  undefined4 local_218 [67];
  undefined4 local_10c [67];
  
  if (*(int *)((int)this + 4) == 0) {
    return (int *)0x0;
  }
  pcVar1 = (char *)FUN_100069e0(param_1,"Game/mode",0);
  if ((pcVar1 != (char *)0x0) && (*pcVar1 != '\0')) {
    pcVar1 = (char *)FUN_100069e0(param_1,"Game/campaign",0);
    if ((pcVar1 != (char *)0x0) && (*pcVar1 != '\0')) {
      iVar2 = FUN_10005a70(param_1,"Game/chapter",0);
      if (0 < iVar2) {
        puVar3 = FUN_10001330(local_10c,"GameModes/%s/%s");
        this_00 = (void *)FUN_10005bb0(*(void **)((int)this + 4),(char *)((int)puVar3 + 5),0);
        if (this_00 != (void *)0x0) {
          puVar3 = FUN_10001330(local_218,"Modes/%s/chapters");
          iVar4 = FUN_10005a70(this_00,(char *)((int)puVar3 + 5),0);
          if (iVar2 <= iVar4) {
            puVar3 = FUN_10001330(local_218,"Modes/%s/%d");
            piVar5 = FUN_100058e0(this_00,(char *)((int)puVar3 + 5),0);
            if (piVar5 != (int *)0x0) {
              if (param_2 != (undefined4 *)0x0) {
                *param_2 = this_00;
              }
              return piVar5;
            }
          }
        }
      }
    }
    return (int *)0x0;
  }
  return (int *)0x0;
}


// 10001610 FUN_10001610

int * __thiscall FUN_10001610(void *this,void *param_1,byte *param_2,undefined4 *param_3)

{
  char *pcVar1;
  undefined4 *puVar2;
  int *piVar3;
  void *pvVar4;
  int iVar5;
  int *this_00;
  byte *pbVar6;
  uint uVar7;
  int iVar8;
  undefined4 local_218 [67];
  undefined4 local_10c [67];
  
  if (((*(int *)((int)this + 4) == 0) || (param_2 == (byte *)0x0)) || (*param_2 == 0)) {
    return (int *)0x0;
  }
  pcVar1 = (char *)FUN_100069e0(param_1,"Game/mode",0);
  if ((pcVar1 != (char *)0x0) && (*pcVar1 != '\0')) {
    puVar2 = FUN_10001330(local_218,"GameModes/%s");
    piVar3 = FUN_100058e0(*(void **)((int)this + 4),(char *)((int)puVar2 + 5),0);
    if (piVar3 != (int *)0x0) {
      pvVar4 = (void *)FUN_100052f0();
      if (pvVar4 == (void *)0x0) {
        return (int *)0x0;
      }
      do {
        pvVar4 = (void *)FUN_10005bb0(pvVar4,(char *)0x0,0);
        if (pvVar4 != (void *)0x0) {
          puVar2 = FUN_10001330(local_218,"Modes/%s");
          piVar3 = FUN_100058e0(pvVar4,(char *)((int)puVar2 + 5),0);
          if (piVar3 != (int *)0x0) {
            iVar5 = FUN_10005a70(piVar3,"chapters",0);
            iVar8 = 1;
            if (0 < iVar5) {
              do {
                puVar2 = FUN_10001330(local_10c,"%d");
                this_00 = FUN_100058e0(piVar3,(char *)((int)puVar2 + 5),0);
                if (this_00 == (int *)0x0) break;
                pbVar6 = (byte *)FUN_100069e0(this_00,"map",0x1002536c);
                uVar7 = FUN_100098d0(pbVar6,param_2);
                if (uVar7 == 0) {
                  if (param_3 != (undefined4 *)0x0) {
                    *param_3 = pvVar4;
                  }
                  return this_00;
                }
                iVar8 = iVar8 + 1;
              } while (iVar8 <= iVar5);
            }
          }
        }
        pvVar4 = (void *)FUN_10005320();
        if (pvVar4 == (void *)0x0) {
          return (int *)0x0;
        }
      } while( true );
    }
  }
  return (int *)0x0;
}


// 100017c0 FUN_100017c0

int __thiscall FUN_100017c0(void *this,int param_1,int param_2,int param_3)

{
  int iVar1;
  int iVar2;
  
  if (param_2 != 0) {
    if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + param_2) {
      FUN_1000e340(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + param_2);
    }
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
                    /* WARNING: Load size is inaccurate */
    iVar2 = *this;
    iVar1 = (*(int *)((int)this + 0xc) - param_1) - param_2;
    *(int *)((int)this + 0x10) = iVar2;
    if ((0 < iVar1) && (0 < param_2)) {
      FUN_10009be0((void *)(iVar2 + (param_1 + param_2) * 4),(void *)(iVar2 + param_1 * 4),iVar1 * 4
                  );
    }
    if ((param_3 != 0) && (iVar2 = 0, 0 < param_2)) {
      iVar1 = param_1 * 4;
      do {
                    /* WARNING: Load size is inaccurate */
        *(undefined4 *)(iVar1 + *this) = *(undefined4 *)(param_3 + iVar2 * 4);
        iVar2 = iVar2 + 1;
        iVar1 = iVar1 + 4;
      } while (iVar2 < param_2);
    }
    return param_1;
  }
  return param_1;
}


// 10001860 FUN_10001860

int __thiscall FUN_10001860(void *this,byte *param_1)

{
  int iVar1;
  uint uVar2;
  
  FUN_100096f0((void *)((int)this + 0x14),(ushort *)&param_1,param_1);
  uVar2 = (uint)param_1 & 0xffff;
  iVar1 = *(int *)((int)this + 0xc);
  if ((iVar1 <= (int)uVar2) && (iVar1 < (int)(uVar2 + 1))) {
    FUN_100017c0(this,iVar1,(uVar2 + 1) - iVar1,0);
  }
                    /* WARNING: Load size is inaccurate */
  return *this + uVar2 * 4;
}


// 100018a0 FUN_100018a0

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */

void __thiscall FUN_100018a0(void *this,byte *param_1,undefined4 *param_2)

{
  int iVar1;
  char cVar2;
  byte bVar3;
  int *piVar4;
  ushort *puVar5;
  undefined4 *puVar6;
  void *pvVar7;
  byte *pbVar8;
  int iVar9;
  int *this_00;
  undefined1 *puVar10;
  undefined1 *puVar11;
  undefined4 *puVar12;
  byte *pbVar13;
  byte *pbVar14;
  byte *pbVar15;
  uint uVar16;
  char *pcVar17;
  undefined4 uVar18;
  undefined4 local_34c [67];
  undefined4 local_240 [67];
  undefined4 local_134;
  char local_12f [263];
  int *local_28;
  int *local_24;
  undefined4 *local_20;
  void *local_1c;
  byte *local_18;
  undefined4 local_14;
  byte *local_10;
  byte *local_c;
  byte *local_8;
  
  local_1c = this;
  piVar4 = FUN_100058e0(*(void **)((int)this + 4),"GameModes",0);
  local_24 = piVar4;
  local_28 = FUN_100058e0(*(void **)((int)this + 4),"Missions",0);
  if (local_28 == (int *)0x0) {
    return;
  }
  if (piVar4 == (int *)0x0) {
    return;
  }
  puVar5 = FUN_100094a0((void *)((int)this + 0x1c),(ushort *)((int)&local_14 + 2),(int)param_1);
  if (*puVar5 != 0xffff) {
    return;
  }
  puVar6 = (undefined4 *)FUN_10001860((void *)((int)this + 8),param_1);
  *puVar6 = 0;
  pvVar7 = (void *)FUN_100054d0(0x24);
  if (pvVar7 == (void *)0x0) {
    local_20 = (undefined4 *)0x0;
  }
  else {
    local_20 = FUN_10005890(pvVar7,"mission");
  }
  puVar6 = local_20;
  if (DAT_10037bb0 == 0) {
    piVar4 = (int *)0x0;
  }
  else {
    piVar4 = (int *)(DAT_10037bb0 + 4);
  }
  cVar2 = FUN_100079d0(local_20,piVar4,param_1);
  if (cVar2 == '\0') {
    pcVar17 = "MissionManager:  Mission file \"%s\" is malformed, failed to parse.\n";
  }
  else {
    pbVar8 = (byte *)FUN_100069e0(puVar6,"name",0);
    local_18 = pbVar8;
    local_10 = (byte *)FUN_100069e0(puVar6,"version",0);
    if ((pbVar8 == (byte *)0x0) || (local_10 == (byte *)0x0)) {
      pcVar17 = "MissionManager:  Mission file \"%s\" is missing name and version\n";
    }
    else {
      bVar3 = *pbVar8;
      pbVar13 = pbVar8;
      if (bVar3 != 0) {
LAB_100019a3:
        if (((('`' < (char)bVar3) && ((char)bVar3 < '{')) ||
            (('@' < (char)bVar3 && ((char)bVar3 < '[')))) || ((byte)(bVar3 - 0x30) < 10))
        goto LAB_100019b9;
        Warning("MissionManager:  Only alphanumeric characters allowed in mission name: \"%s\" in \"%s\"\n"
                ,pbVar8,param_1);
        pbVar13 = pbVar8;
        do {
          bVar3 = *pbVar13;
          pbVar13 = pbVar13 + 1;
        } while (bVar3 != 0);
        local_c = &stack0xfffffca8;
        _memset(&stack0xfffffca8,0,((int)pbVar13 - (int)(pbVar8 + 1)) * 3 + 1);
        bVar3 = *pbVar8;
        pbVar13 = pbVar8;
        pbVar15 = local_c;
        while (bVar3 != 0) {
          bVar3 = *pbVar13;
          if (bVar3 != 0x20) {
            if ((((char)bVar3 < 'a') || ('z' < (char)bVar3)) &&
               ((((char)bVar3 < 'A' || ('Z' < (char)bVar3)) && (9 < (byte)(bVar3 - 0x30))))) {
              *pbVar15 = 0x5f;
              pbVar14 = pbVar15 + 2;
              pbVar15[1] = (char)bVar3 % '\x1a' + 0x61;
              *pbVar14 = ((char)bVar3 / '\x1a') % '\x1a' + 0x61;
            }
            else {
              *pbVar15 = bVar3;
              pbVar14 = pbVar15;
            }
            pbVar15 = pbVar14 + 1;
          }
          pbVar13 = pbVar13 + 1;
          bVar3 = *pbVar13;
        }
        local_8 = pbVar13;
        Warning("MissionManager:  renamed mission name: \"%s\"->\"%s\" in \"%s\"\n",pbVar8,local_c,
                param_1);
        pcVar17 = (char *)FUN_100069e0(puVar6,"DisplayTitle",0x1002536c);
        if ((pcVar17 == (char *)0x0) || (*pcVar17 == '\0')) {
          FUN_10005cb0(puVar6,"DisplayTitle",(char *)pbVar8);
        }
        FUN_10005cb0(puVar6,"Name",(char *)local_c);
        pbVar8 = (byte *)FUN_100069e0(puVar6,"Name",0x1002536c);
        local_18 = pbVar8;
      }
LAB_10001ad9:
      pbVar13 = local_10;
      bVar3 = *local_10;
      pbVar15 = local_10;
      if (bVar3 != 0) {
LAB_10001ae5:
        if ((byte)(bVar3 - 0x30) < 10) goto code_r0x10001aeb;
        Warning("MissionManager:  Only numeric characters allowed in mission version: \"%s\" in \"%s\"\n"
                ,local_10,param_1);
        pbVar15 = pbVar13 + 1;
        do {
          bVar3 = *pbVar13;
          pbVar13 = pbVar13 + 1;
        } while (bVar3 != 0);
        local_8 = pbVar13 + (1 - (int)pbVar15);
        _memset(&stack0xfffffca8,0,(size_t)local_8);
        bVar3 = *local_10;
        pbVar13 = local_10;
        local_8 = &stack0xfffffca8;
        while (bVar3 != 0) {
          if ((byte)(*pbVar13 - 0x30) < 10) {
            *local_8 = *pbVar13;
            local_8 = local_8 + 1;
          }
          pbVar13 = pbVar13 + 1;
          bVar3 = *pbVar13;
        }
        Warning("MissionManager:  renamed mission version: \"%s\"->\"%s\" in \"%s\"\n",local_10,
                &stack0xfffffca8,param_1);
        FUN_10005cb0(puVar6,"version",&stack0xfffffca8);
        local_10 = (byte *)FUN_100069e0(puVar6,"version",0x1002536c);
      }
LAB_10001ba9:
      pbVar13 = local_10;
      local_8 = (byte *)FUN_100058e0(puVar6,"modes",0);
      if (local_8 == (byte *)0x0) {
        pcVar17 = "MissionManager:  Mission file \"%s\" is missing data for any game modes\n";
      }
      else {
        FUN_10001330(&local_134,"%s_%s");
        puVar5 = FUN_100094a0((void *)((int)local_1c + 0x68),(ushort *)((int)&local_14 + 2),
                              (int)pbVar8);
        pvVar7 = local_1c;
        if (*puVar5 != 0xffff) {
          piVar4 = (int *)((int)local_1c + 0x54);
          FUN_100096f0((void *)((int)local_1c + 0x68),(ushort *)&param_2,pbVar8);
          local_8 = (byte *)((uint)param_2 & 0xffff);
          iVar9 = *(int *)((int)pvVar7 + 0x60);
          if ((iVar9 <= (int)local_8) && (iVar9 < (int)(local_8 + 1))) {
            FUN_100017c0(piVar4,iVar9,(int)(local_8 + 1) - iVar9,0);
          }
          iVar9 = FUN_100069e0(*(void **)(*piVar4 + (int)local_8 * 4),"CfgFile",0x1002536c);
          Warning("MissionManager:  Duplicate mission \"%s\" in file \"%s\", already loaded from \"%s\".\n"
                  ,pbVar8,param_1,iVar9);
          goto LAB_10001fd0;
        }
        DevMsg("\tMission %s ver %s loading...\n",pbVar8,pbVar13);
        pcVar17 = (char *)FUN_100069e0(puVar6,"DisplayTitle",0x1002536c);
        if ((pcVar17 == (char *)0x0) || (*pcVar17 == '\0')) {
          FUN_10005cb0(puVar6,"DisplayTitle",(char *)pbVar8);
        }
        FUN_10005350(puVar6,pbVar8);
        FUN_10005db0(puVar6,"BuiltIn",(uint)((char)param_2 != '\0'));
        FUN_10005cb0(puVar6,"CfgFile",(char *)param_1);
        FUN_10005cb0(puVar6,"CfgTag",local_12f);
        local_c = (byte *)0x0;
        param_2 = (undefined4 *)FUN_10005290();
        puVar12 = param_2;
        if (param_2 != (undefined4 *)0x0) {
          do {
            uVar18 = 0;
            pcVar17 = FUN_10005080(param_2);
            piVar4 = FUN_100058e0(local_8,pcVar17,uVar18);
            if (piVar4 != (int *)0x0) {
              local_14 = 0;
              puVar6 = FUN_10001330(local_240,"%d");
              this_00 = FUN_100058e0(piVar4,(char *)((int)puVar6 + 5),0);
              if (this_00 != (int *)0x0) {
                iVar9 = 1;
                do {
                  pcVar17 = (char *)FUN_100069e0(this_00,"map",0x1002536c);
                  if ((pcVar17 == (char *)0x0) || (*pcVar17 == '\0')) {
                    puVar10 = FUN_10005080(this_00);
                    puVar11 = FUN_10005080(param_2);
                    Warning("MissionManager:  Mission file \"%s\" has invalid map specified for modes/%s/%s\n"
                            ,param_1,puVar11,puVar10);
                    goto LAB_10001e2c;
                  }
                  pcVar17 = (char *)FUN_100069e0(this_00,"DisplayName",0x1002536c);
                  if ((pcVar17 == (char *)0x0) || (*pcVar17 == '\0')) {
                    FUN_10005080(this_00);
                    puVar6 = FUN_10001330(local_34c,"%s-%s");
                    FUN_10005cb0(this_00,"DisplayName",(char *)((int)puVar6 + 5));
                  }
                  pcVar17 = (char *)FUN_100069e0(this_00,"image",0x1002536c);
                  if ((pcVar17 == (char *)0x0) || (*pcVar17 == '\0')) {
                    FUN_10005cb0(this_00,"image","maps/unknown");
                  }
                  FUN_10005db0(this_00,"chapter",iVar9);
                  local_14 = local_14 + 1;
                  iVar9 = iVar9 + 1;
                  puVar6 = FUN_10001330(local_240,"%d");
                  this_00 = FUN_100058e0(piVar4,(char *)((int)puVar6 + 5),0);
                  iVar1 = local_14;
                } while (this_00 != (int *)0x0);
                if (local_14 != 0) {
                  FUN_10005db0(piVar4,"chapters",local_14);
                  puVar10 = FUN_10005080(param_2);
                  DevMsg("\t\tloaded %d %s chapters.\n",iVar1,puVar10);
                  local_c = local_c + 1;
                  puVar6 = local_20;
                  goto LAB_10001eb2;
                }
              }
LAB_10001e2c:
              FUN_10005240(local_8,(int)piVar4);
              FUN_10005ff0((int)piVar4);
              puVar10 = FUN_10005080(param_2);
              Warning("MissionManager:  Mission file \"%s\" has invalid settings for game mode %s\n"
                      ,param_1,puVar10);
              puVar6 = local_20;
            }
LAB_10001eb2:
            param_2 = (undefined4 *)FUN_100052c0();
            pvVar7 = local_1c;
          } while (param_2 != (undefined4 *)0x0);
          puVar12 = (undefined4 *)0x0;
          if (local_c != (byte *)0x0) {
            piVar4 = (int *)((int)local_1c + 8);
            FUN_100096f0((void *)((int)local_1c + 0x1c),(ushort *)&param_2,param_1);
            uVar16 = (uint)param_2 & 0xffff;
            iVar9 = *(int *)((int)pvVar7 + 0x14);
            if ((iVar9 <= (int)uVar16) && (iVar9 < (int)(uVar16 + 1))) {
              FUN_100017c0(piVar4,iVar9,(uVar16 + 1) - iVar9,0);
            }
            pvVar7 = local_1c;
            *(undefined4 **)(*piVar4 + uVar16 * 4) = puVar6;
            piVar4 = (int *)((int)local_1c + 0x54);
            FUN_100096f0((void *)((int)local_1c + 0x68),(ushort *)&param_2,local_18);
            uVar16 = (uint)param_2 & 0xffff;
            iVar9 = *(int *)((int)pvVar7 + 0x60);
            if ((iVar9 <= (int)uVar16) && (iVar9 < (int)(uVar16 + 1))) {
              FUN_100017c0(piVar4,iVar9,(uVar16 + 1) - iVar9,0);
            }
            *(undefined4 **)(*piVar4 + uVar16 * 4) = puVar6;
            FUN_10005a40(local_28,puVar6);
            puVar12 = (undefined4 *)FUN_10005290();
            pbVar8 = local_8;
            while (puVar12 != (undefined4 *)0x0) {
              uVar18 = 0;
              pcVar17 = FUN_10005080(puVar12);
              piVar4 = FUN_100058e0(pbVar8,pcVar17,uVar18);
              if (piVar4 != (int *)0x0) {
                FUN_10005e20(puVar12,(char *)local_18,(int)puVar6);
              }
              puVar12 = (undefined4 *)FUN_100052c0();
            }
            DevMsg("\tMission %s ver %s loaded %d game modes.\n",local_18,local_10,local_c);
            return;
          }
        }
        param_2 = puVar12;
        pcVar17 = 
        "MissionManager:  Mission file \"%s\" does not have valid data for any supported game mode\n"
        ;
      }
    }
  }
  Warning(pcVar17,param_1);
LAB_10001fd0:
  if (puVar6 != (undefined4 *)0x0) {
    FUN_10005ff0((int)puVar6);
  }
  return;
LAB_100019b9:
  bVar3 = pbVar13[1];
  pbVar13 = pbVar13 + 1;
  if (bVar3 == 0) goto LAB_10001ad9;
  goto LAB_100019a3;
code_r0x10001aeb:
  bVar3 = pbVar15[1];
  pbVar15 = pbVar15 + 1;
  if (bVar3 == 0) goto LAB_10001ba9;
  goto LAB_10001ae5;
}


// 10001ff0 FUN_10001ff0

void __fastcall FUN_10001ff0(void *param_1)

{
  undefined4 *puVar1;
  int iVar2;
  undefined4 unaff_EBP;
  undefined1 auStack_20c [4];
  char acStack_208 [252];
  byte abStack_10c [268];
  
  DevMsg("Loading Mission Data\n");
  if (*(int *)((int)param_1 + 4) != 0) {
    FUN_10005ff0(*(int *)((int)param_1 + 4));
    *(undefined4 *)((int)param_1 + 4) = 0;
  }
  *(undefined4 *)((int)param_1 + 0x14) = 0;
  if (-1 < *(int *)((int)param_1 + 0x10)) {
    if (*(int *)((int)param_1 + 8) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)param_1 + 8));
      *(undefined4 *)((int)param_1 + 8) = 0;
    }
    *(undefined4 *)((int)param_1 + 0xc) = 0;
  }
  *(undefined4 *)((int)param_1 + 0x18) = *(undefined4 *)((int)param_1 + 8);
  FUN_10009550((void *)((int)param_1 + 0x1c));
  *(undefined4 *)((int)param_1 + 0x60) = 0;
  if (-1 < *(int *)((int)param_1 + 0x5c)) {
    if (*(int *)((int)param_1 + 0x54) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)param_1 + 0x54));
      *(undefined4 *)((int)param_1 + 0x54) = 0;
    }
    *(undefined4 *)((int)param_1 + 0x58) = 0;
  }
  *(undefined4 *)((int)param_1 + 100) = *(undefined4 *)((int)param_1 + 0x54);
  FUN_10009550((void *)((int)param_1 + 0x68));
  puVar1 = FUN_10006410("Left4Dead1",
                        " GameModes {  coop { }  versus { }  survival { }  }  Missions {  } ");
  *(undefined4 **)((int)param_1 + 4) = puVar1;
  FUN_100018a0(param_1,(byte *)"missions/hospital.txt",(undefined4 *)0x1);
  FUN_100018a0(param_1,(byte *)"missions/smalltown.txt",(undefined4 *)0x1);
  FUN_100018a0(param_1,(byte *)"missions/airport.txt",(undefined4 *)0x1);
  FUN_100018a0(param_1,(byte *)"missions/farm.txt",(undefined4 *)0x1);
  FUN_100018a0(param_1,(byte *)"missions/garage.txt",(undefined4 *)0x1);
  FUN_100018a0(param_1,(byte *)"missions/lighthouse.txt",(undefined4 *)0x1);
  FUN_100018a0(param_1,(byte *)"missions/river.txt",(undefined4 *)0x1);
  FUN_100018a0(param_1,(byte *)"missions/credits.txt",(undefined4 *)0x1);
  FUN_10009c50(acStack_208,0x104,"missions/*.txt");
  FUN_10009b80(acStack_208,'\\');
  iVar2 = (**(code **)(*DAT_10037bb0 + 0x70))(acStack_208,auStack_20c);
  while (iVar2 != 0) {
    FUN_10009c50((char *)abStack_10c,0x104,"missions/%s");
    iVar2 = (**(code **)(*DAT_10037bb0 + 0x74))(unaff_EBP);
    FUN_100018a0(param_1,abStack_10c,(undefined4 *)0x0);
  }
  FUN_100013a0(param_1,"versus","teamversus");
  DevMsg("Loading Mission Data Finished\n");
  return;
}


// 100021f0 FUN_100021f0

undefined4 __fastcall FUN_100021f0(int param_1)

{
  return *(undefined4 *)(param_1 + 0x50);
}


// 100023c0 FUN_100023c0

undefined4 FUN_100023c0(void)

{
  int *piVar1;
  undefined4 uVar2;
  
  piVar1 = (int *)(**(code **)(*DAT_10037be4 + 0x24))();
  if (piVar1 != (int *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x100023d8. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    uVar2 = (**(code **)(*piVar1 + 4))();
    return uVar2;
  }
  return 0;
}


// 100023e0 FUN_100023e0

void FUN_100023e0(undefined4 *param_1)

{
  byte *pbVar1;
  uint uVar2;
  int *piVar3;
  void *pvVar4;
  char *pcVar5;
  
  pbVar1 = FUN_10005080(param_1);
  uVar2 = FUN_100098d0((byte *)"OnPlayerRemoved",pbVar1);
  if ((uVar2 == 0) || (uVar2 = FUN_100098d0((byte *)"OnPlayerUpdated",pbVar1), uVar2 == 0)) {
    piVar3 = (int *)(**(code **)(*DAT_10037be4 + 0x24))();
    if (piVar3 == (int *)0x0) {
      FUN_10004c90();
      return;
    }
    (**(code **)(*piVar3 + 4))();
    FUN_10004c90();
    return;
  }
  uVar2 = FUN_100098d0((byte *)"OnMatchSessionUpdate",pbVar1);
  if (uVar2 != 0) {
    return;
  }
  pcVar5 = "updated";
  pbVar1 = (byte *)FUN_100069e0(param_1,"state",0x1002536c);
  uVar2 = FUN_100098d0(pbVar1,(byte *)pcVar5);
  if (uVar2 == 0) {
    piVar3 = FUN_100058e0(param_1,"Update",0);
    if (piVar3 == (int *)0x0) {
      return;
    }
    pvVar4 = (void *)FUN_100023c0();
    FUN_10004c40(pvVar4);
    return;
  }
  pcVar5 = "created";
  pbVar1 = (byte *)FUN_100069e0(param_1,"state",0x1002536c);
  uVar2 = FUN_100098d0(pbVar1,(byte *)pcVar5);
  if (uVar2 != 0) {
    pcVar5 = "ready";
    pbVar1 = (byte *)FUN_100069e0(param_1,"state",0x1002536c);
    uVar2 = FUN_100098d0(pbVar1,(byte *)pcVar5);
    if (uVar2 != 0) {
      pcVar5 = "closed";
      pbVar1 = (byte *)FUN_100069e0(param_1,"state",0x1002536c);
      uVar2 = FUN_100098d0(pbVar1,(byte *)pcVar5);
      if (uVar2 != 0) {
        return;
      }
      FUN_10004c40((void *)0x0);
      return;
    }
  }
  pvVar4 = (void *)FUN_100023c0();
  FUN_10004c40(pvVar4);
  return;
}


// 10002560 FUN_10002560

void __fastcall FUN_10002560(undefined4 *param_1)

{
  param_1[1] = &PTR_LAB_100259cc;
  param_1[2] = &PTR_LAB_100259bc;
  *param_1 = &PTR_LAB_100259ec;
  param_1[1] = &PTR_FUN_100259e4;
  param_1[2] = &PTR_LAB_100259d4;
  return;
}


// 10002590 FUN_10002590

undefined4 * __thiscall FUN_10002590(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_LAB_100259ec;
  *(undefined ***)((int)this + 4) = &PTR_FUN_100259e4;
  *(undefined ***)((int)this + 8) = &PTR_LAB_100259d4;
  *(undefined ***)((int)this + 8) = &PTR_LAB_100259bc;
  if ((param_1 & 1) != 0) {
    FUN_100012d0(this);
  }
  return (undefined4 *)this;
}


// 10002b40 FUN_10002b40

undefined4 __fastcall FUN_10002b40(int param_1)

{
  return *(undefined4 *)(param_1 + 0x54);
}


// 10002b50 FUN_10002b50

void __cdecl FUN_10002b50(char *param_1)

{
  void *this;
  char *pcVar1;
  void *unaff_ESI;
  int *this_00;
  
  this_00 = (int *)0x0;
  if (unaff_ESI != (void *)0x0) {
    if ((param_1 == (char *)0x0) || (*param_1 == '\0')) {
      pcVar1 = (char *)FUN_100069e0((void *)0x0,"Version",0x10025d74);
      FUN_10005cb0(unaff_ESI,"Version",pcVar1);
      pcVar1 = (char *)FUN_100069e0((void *)0x0,"BuiltIn",0x10025d74);
      FUN_10005cb0(unaff_ESI,"BuiltIn",pcVar1);
      pcVar1 = (char *)FUN_100069e0((void *)0x0,"DisplayTitle",0x10025d54);
      FUN_10005cb0(unaff_ESI,"DisplayTitle",pcVar1);
      pcVar1 = (char *)FUN_100069e0((void *)0x0,"Author",0x1002536c);
      FUN_10005cb0(unaff_ESI,"Author",pcVar1);
      this_00 = (int *)0x0;
    }
    else {
      this = (void *)(*(code *)**(undefined4 **)PTR_PTR_10034018)();
      if (this != (void *)0x0) {
        this_00 = FUN_100058e0(this,param_1,0);
      }
      pcVar1 = (char *)FUN_100069e0(this_00,"Version",0x1002536c);
      FUN_10005cb0(unaff_ESI,"Version",pcVar1);
      pcVar1 = (char *)FUN_100069e0(this_00,"BuiltIn",0x10025d74);
      FUN_10005cb0(unaff_ESI,"BuiltIn",pcVar1);
      pcVar1 = (char *)FUN_100069e0(this_00,"DisplayTitle",0x1002536c);
      FUN_10005cb0(unaff_ESI,"DisplayTitle",pcVar1);
      pcVar1 = (char *)FUN_100069e0(this_00,"Author",0x1002536c);
      FUN_10005cb0(unaff_ESI,"Author",pcVar1);
    }
    pcVar1 = (char *)FUN_100069e0(this_00,"Website",0x1002536c);
    FUN_10005cb0(unaff_ESI,"Website",pcVar1);
  }
  return;
}


// 10002cb0 FUN_10002cb0

ulonglong __cdecl FUN_10002cb0(undefined4 param_1)

{
  void *this;
  void *unaff_EDI;
  ulonglong uVar1;
  ulonglong uVar2;
  undefined4 local_4;
  
  local_4 = 0;
  this = (void *)(**(code **)(*(int *)PTR_PTR_10034018 + 4))(param_1,&local_4);
  uVar1 = FUN_10005af0(this,"dlcmask",0,0);
  uVar2 = FUN_10005af0(unaff_EDI,"dlcmask",0,0);
  return uVar1 | uVar2;
}


// 10002d50 FUN_10002d50

void * __fastcall FUN_10002d50(void *param_1)

{
  void *this;
  
  FUN_1000ae50(param_1,0);
  FUN_1000ae70(param_1,0);
  FUN_1000ae20(param_1,3);
  this = (void *)((int)param_1 + 0xc);
  FUN_1000ae50(this,0);
  FUN_1000ae70(this,0);
  FUN_1000ae20(this,3);
  return param_1;
}


// 10002e10 FUN_10002e10

void __cdecl FUN_10002e10(uint *param_1,uint *param_2)

{
  char cVar1;
  uint *puVar2;
  uint *puVar3;
  uint uVar4;
  int iVar5;
  uint *puVar6;
  
  puVar3 = param_2;
  if (param_1 != (uint *)0x0) {
    cVar1 = (char)*param_1;
    puVar2 = param_1;
    while (cVar1 != '\0') {
      cVar1 = (char)*puVar2;
      puVar6 = puVar2;
      while ((cVar1 != '\0' && (uVar4 = FUN_1000f931((int)cVar1), uVar4 == 0))) {
        cVar1 = *(char *)((int)puVar6 + 1);
        puVar6 = (uint *)((int)puVar6 + 1);
      }
      if (puVar2 < puVar6) {
        FUN_1000afd0(puVar3,puVar2,(int)puVar6 - (int)puVar2);
      }
      if ((char)*puVar6 != '\0') {
        iVar5 = _tolower((int)(char)*puVar6);
        param_1 = (uint *)CONCAT31(param_1._1_3_,(char)iVar5);
        puVar6 = (uint *)((int)puVar6 + 1);
        FUN_1000afd0(puVar3,(uint *)&param_1,1);
      }
      puVar2 = puVar6;
      cVar1 = (char)*puVar6;
    }
  }
  return;
}


// 10002e90 FUN_10002e90

void __cdecl FUN_10002e90(void *param_1)

{
  void *in_EAX;
  char *pcVar1;
  char *pcVar2;
  byte *pbVar3;
  uint uVar4;
  int iVar5;
  void *this;
  void *this_00;
  undefined4 *puVar6;
  int *this_01;
  
  pcVar1 = (char *)FUN_100069e0(in_EAX,"Game/MissionInfo/BuiltIn",0);
  pcVar2 = (char *)FUN_100069e0(in_EAX,"Game/campaign",0x1002536c);
  if (*pcVar2 == '\0') {
    pbVar3 = (byte *)FUN_100069e0(in_EAX,"Options/action",0x1002536c);
    uVar4 = FUN_100098d0(pbVar3,(byte *)"quickmatch");
    if ((uVar4 == 0) || (uVar4 = FUN_100098d0(pbVar3,(byte *)"custommatch"), uVar4 == 0)) {
      pcVar1 = "official";
    }
    pcVar2 = (char *)FUN_100069e0(in_EAX,"Game/mode",0x1002536c);
    pcVar2 = FUN_100099b0(pcVar2,"team");
    if (pcVar2 != (char *)0x0) {
      pcVar1 = "official";
      goto LAB_10002f2f;
    }
  }
  if ((byte *)pcVar1 == (byte *)0x0) {
    return;
  }
LAB_10002f2f:
  uVar4 = FUN_100098d0((byte *)pcVar1,(byte *)"addon");
  if (uVar4 != 0) {
    uVar4 = FUN_100098d0((byte *)pcVar1,(byte *)"official");
    if (uVar4 == 0) {
      FUN_10005db0(param_1,"Filter=/Game:MissionInfo:BuiltIn",1);
      return;
    }
    uVar4 = FUN_100098d0((byte *)pcVar1,(byte *)"installedaddon");
    if (uVar4 != 0) {
      uVar4 = FUN_100098d0((byte *)pcVar1,(byte *)"notinstalledaddon");
      if (uVar4 == 0) {
        FUN_10005db0(param_1,"Filter=/Game:MissionInfo:BuiltIn",0);
        iVar5 = (*(code *)**(undefined4 **)PTR_PTR_10034018)();
        if (iVar5 != 0) {
          this = (void *)FUN_10005290();
          while (this != (void *)0x0) {
            iVar5 = FUN_10005a70(this,"BuiltIn",0);
            if (iVar5 == 0) {
              this_00 = (void *)FUN_100054d0(0x24);
              if (this_00 == (void *)0x0) {
                puVar6 = (undefined4 *)0x0;
              }
              else {
                FUN_100069e0(this,"Name",0x1002536c);
                puVar6 = FUN_100068f0(this_00,"Game:campaign");
              }
              this_01 = FUN_100058e0(param_1,"Filter<>",1);
              FUN_10005a40(this_01,puVar6);
            }
            this = (void *)FUN_100052c0();
          }
        }
        FUN_10005cb0(param_1,"Filter<>/Game:MissionInfo:Website","");
      }
      return;
    }
  }
  FUN_10005db0(param_1,"Filter=/Game:MissionInfo:BuiltIn",0);
  return;
}


// 10003140 FUN_10003140

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10003140(void)

{
  bool bVar1;
  undefined1 *puVar2;
  void *this;
  char *pcVar3;
  int iVar4;
  ulonglong uVar5;
  void *unaff_retaddr;
  undefined1 *this_00;
  undefined1 local_30 [12];
  undefined1 local_24 [10];
  char cStack_1a;
  undefined1 *puStack_10;
  
  FUN_1000ae50(local_30,0);
  FUN_1000ae70(local_30,0);
  FUN_1000ae20(local_30,3);
  FUN_1000ae50(local_24,0);
  FUN_1000ae70(local_24,0);
  FUN_1000ae20(local_24,3);
  (**(code **)(**(int **)(PTR_PTR_10035700 + 0x54) + 0x18))(local_30);
  puVar2 = puStack_10;
  if ((cStack_1a == '\0') && (*(int **)(PTR_PTR_10035700 + 0x5c) != (int *)0x0)) {
    puVar2 = (undefined1 *)(**(code **)(**(int **)(PTR_PTR_10035700 + 0x5c) + 0xd4))();
  }
  if (puVar2 == (undefined1 *)0x0) {
    puVar2 = &DAT_1002536c;
  }
  FUN_10005cb0(unaff_retaddr,"Game/state","game");
  if ((_DAT_10037a88 & 1) == 0) {
    _DAT_10037a88 = _DAT_10037a88 | 1;
    FUN_1000ab60(&DAT_10037a80,"mp_gamemode",'\x01');
  }
  bVar1 = FUN_1000a790((undefined4 *)&DAT_10037a80);
  if (bVar1) {
    FUN_10005cb0(unaff_retaddr,"Game/mode",*(char **)(DAT_10037a84 + 0x24));
  }
  this_00 = &stack0xffffffc8;
  this = (void *)(**(code **)(*(int *)PTR_PTR_10034018 + 8))(unaff_retaddr,puVar2);
  if (this != (void *)0x0) {
    pcVar3 = (char *)FUN_100069e0(this_00,"Name",0x1002536c);
    FUN_10005cb0(unaff_retaddr,"Game/campaign",pcVar3);
    iVar4 = FUN_10005a70(this,"chapter",0);
    FUN_10005db0(unaff_retaddr,"Game/chapter",iVar4);
    FUN_100058e0(unaff_retaddr,"Game/MissionInfo",1);
    pcVar3 = (char *)FUN_100069e0(unaff_retaddr,"Game/campaign",0x1002536c);
    FUN_10002b50(pcVar3);
  }
  if ((_DAT_10037a88 & 2) == 0) {
    _DAT_10037a88 = _DAT_10037a88 | 2;
    FUN_1000ab60(&DAT_10037a78,"z_difficulty",'\x01');
  }
  bVar1 = FUN_1000a790((undefined4 *)&DAT_10037a78);
  if (bVar1) {
    FUN_10005cb0(unaff_retaddr,"Game/difficulty",*(char **)(DAT_10037a7c + 0x24));
  }
  if ((_DAT_10037a88 & 4) == 0) {
    _DAT_10037a88 = _DAT_10037a88 | 4;
    FUN_1000ab60(&DAT_10037a70,"mp_roundlimit",'\x01');
  }
  bVar1 = FUN_1000a790((undefined4 *)&DAT_10037a70);
  if (bVar1) {
    FUN_10005db0(unaff_retaddr,"Game/maxrounds",*(int *)(DAT_10037a74 + 0x30));
  }
  uVar5 = FUN_10002cb0(unaff_retaddr);
  FUN_10005dd0(unaff_retaddr,"Game/dlcrequired",(int)uVar5,(int)(uVar5 >> 0x20));
  return;
}


// 10003360 FUN_10003360

int * __cdecl FUN_10003360(void *param_1,undefined4 param_2,undefined4 param_3,undefined4 *param_4)

{
  int *this;
  int iVar1;
  undefined4 *puVar2;
  int *this_00;
  int iVar3;
  int *this_01;
  int iVar4;
  longlong lVar5;
  int iStack_224;
  undefined4 auStack_218 [67];
  undefined4 auStack_10c [67];
  
  if (param_4 != (undefined4 *)0x0) {
    *param_4 = 0;
  }
  if (param_1 == (void *)0x0) {
    return (int *)0x0;
  }
  this = FUN_100058e0(param_1,"Members",0);
  if (this == (int *)0x0) {
    return (int *)0x0;
  }
  iVar1 = FUN_10005a70(this,"numMachines",0);
  iStack_224 = 0;
  if (iVar1 < 1) {
    return (int *)0x0;
  }
  do {
    puVar2 = FUN_10001330(auStack_218,"machine%d");
    this_00 = FUN_100058e0(this,(char *)((int)puVar2 + 5),0);
    if (this_00 != (int *)0x0) {
      iVar3 = FUN_10005a70(this_00,"numPlayers",0);
      iVar4 = 0;
      if (0 < iVar3) {
        do {
          puVar2 = FUN_10001330(auStack_10c,"player%d");
          this_01 = FUN_100058e0(this_00,(char *)((int)puVar2 + 5),0);
          if ((this_01 != (int *)0x0) &&
             (lVar5 = FUN_10005af0(this_01,"xuid",0,0), lVar5 == CONCAT44(param_3,param_2))) {
            if (param_4 != (undefined4 *)0x0) {
              *param_4 = this_00;
            }
            return this_01;
          }
          iVar4 = iVar4 + 1;
        } while (iVar4 < iVar3);
      }
    }
    iStack_224 = iStack_224 + 1;
    if (iVar1 <= iStack_224) {
      return (int *)0x0;
    }
  } while( true );
}


// 100033a7 FUN_100033a7

int * __cdecl FUN_100033a7(int param_1,undefined4 param_2,void *param_3)

{
  int iVar1;
  undefined4 *puVar2;
  int *this;
  int iVar3;
  int *this_00;
  void *unaff_EDI;
  int iVar4;
  longlong lVar5;
  undefined4 in_stack_00000230;
  undefined4 in_stack_00000234;
  undefined4 *in_stack_00000238;
  
  iVar1 = FUN_10005a70(unaff_EDI,"numMachines",0);
  param_1 = 0;
  if (iVar1 < 1) {
    return (int *)0x0;
  }
  do {
    puVar2 = FUN_10001330((undefined4 *)&stack0x00000010,"machine%d");
    this = FUN_100058e0(unaff_EDI,(char *)((int)puVar2 + 5),0);
    if (this != (int *)0x0) {
      iVar3 = FUN_10005a70(this,"numPlayers",0);
      iVar4 = 0;
      if (0 < iVar3) {
        do {
          puVar2 = FUN_10001330((undefined4 *)&stack0x0000011c,"player%d");
          this_00 = FUN_100058e0(this,(char *)((int)puVar2 + 5),0);
          if ((this_00 != (int *)0x0) &&
             (lVar5 = FUN_10005af0(this_00,"xuid",0,0),
             lVar5 == CONCAT44(in_stack_00000234,in_stack_00000230))) {
            if (in_stack_00000238 != (undefined4 *)0x0) {
              *in_stack_00000238 = this;
            }
            return this_00;
          }
          iVar4 = iVar4 + 1;
        } while (iVar4 < iVar3);
      }
    }
    param_1 = param_1 + 1;
    unaff_EDI = param_3;
    if (iVar1 <= param_1) {
      return (int *)0x0;
    }
  } while( true );
}


// 10004980 FUN_10004980

void __thiscall FUN_10004980(void *this,undefined4 *param_1)

{
  void *in_EAX;
  byte *pbVar1;
  int *this_00;
  int *this_01;
  byte *pbVar2;
  uint uVar3;
  int *this_02;
  int iVar4;
  undefined4 *puVar5;
  int *this_03;
  int iVar6;
  int iVar7;
  int iVar8;
  bool bVar9;
  longlong lVar10;
  int local_230;
  undefined4 local_218 [67];
  undefined4 local_10c [67];
  
  lVar10 = FUN_10005af0(this,"xuid",0,0);
  pbVar1 = (byte *)FUN_100069e0(this,"avatar",0x1002536c);
  this_00 = FUN_100058e0(in_EAX,"Members",0);
  if ((this_00 != (int *)0x0) &&
     (this_01 = FUN_10003360(in_EAX,(int)lVar10,(int)((ulonglong)lVar10 >> 0x20),(undefined4 *)0x0),
     this_01 != (int *)0x0)) {
    pbVar2 = (byte *)FUN_100069e0(this_01,"Game/avatar",0x1002536c);
    uVar3 = FUN_100098d0(pbVar1,pbVar2);
    if ((uVar3 != 0) && (this_02 = FUN_100058e0(this_01,"Game",1), this_02 != (int *)0x0)) {
      if (*pbVar1 == 0) {
        pbVar1 = &DAT_1002536c;
      }
      else {
        local_230 = 0;
        iVar4 = FUN_10005a70(this_00,"numMachines",0);
        iVar7 = 0;
        iVar6 = 0;
        if (0 < iVar4) {
          do {
            puVar5 = FUN_10001330(local_218,"machine%d");
            this_03 = FUN_100058e0(this_00,(char *)((int)puVar5 + 5),0);
            if (this_03 != (int *)0x0) {
              iVar6 = FUN_10005a70(this_03,"numPlayers",0);
              iVar8 = 0;
              if (0 < iVar6) {
                do {
                  puVar5 = FUN_10001330(local_10c,"player%d/Game/avatar");
                  pbVar2 = (byte *)FUN_100069e0(this_03,(char *)((int)puVar5 + 5),0x1002536c);
                  uVar3 = FUN_100098d0(pbVar2,pbVar1);
                  if (uVar3 == 0) {
                    local_230 = local_230 + 1;
                  }
                  iVar8 = iVar8 + 1;
                } while (iVar8 < iVar6);
              }
            }
            iVar7 = iVar7 + 1;
            iVar6 = local_230;
          } while (iVar7 < iVar4);
        }
        uVar3 = FUN_100098d0(pbVar1,(byte *)"Infected");
        if (uVar3 == 0) {
          bVar9 = SBORROW4(iVar6,4);
          iVar4 = -4;
        }
        else {
          bVar9 = SBORROW4(iVar6,1);
          iVar4 = -1;
        }
        if (bVar9 == iVar6 + iVar4 < 0) {
          return;
        }
      }
      FUN_10005cb0(this_02,"avatar",(char *)pbVar1);
      *param_1 = this_01;
    }
  }
  return;
}


// 10004b40 FUN_10004b40

void FUN_10004b40(undefined4 *param_1,void *param_2,void *param_3,undefined4 *param_4)

{
  byte *pbVar1;
  uint uVar2;
  char *pcVar3;
  
  pbVar1 = FUN_10005080(param_1);
  uVar2 = FUN_100098d0((byte *)"Game::Avatar",pbVar1);
  if (uVar2 == 0) {
    pbVar1 = (byte *)FUN_100069e0(param_2,"type",0x100263e8);
    uVar2 = FUN_100098d0(&DAT_100263e8,pbVar1);
    if ((uVar2 == 0) && (param_4 != (undefined4 *)0x0)) {
      pcVar3 = (char *)FUN_100069e0(param_1,"avatar",0x1002536c);
      if (*pcVar3 == '\0') {
        pbVar1 = (byte *)FUN_100069e0(param_2,"state",0x10025e9c);
        uVar2 = FUN_100098d0((byte *)"lobby",pbVar1);
        if (uVar2 != 0) {
          return;
        }
        pcVar3 = (char *)FUN_100069e0(param_3,"System/lock",0x1002536c);
        if (*pcVar3 != '\0') {
          return;
        }
      }
      FUN_10004980(param_1,param_4);
    }
  }
  return;
}


// 10004c10 FUN_10004c10

void FUN_10004c10(void)

{
  if (PTR_DAT_100342c0 != (undefined *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x10004c1f. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (**(code **)(*(int *)PTR_DAT_100342c0 + 0x34))();
    return;
  }
  return;
}


// 10004c30 FUN_10004c30

undefined4 FUN_10004c30(void)

{
  return 0;
}


// 10004c40 FUN_10004c40

void __cdecl FUN_10004c40(void *param_1)

{
  char *pcVar1;
  void *this;
  undefined4 uVar2;
  
  if (param_1 != (void *)0x0) {
    pcVar1 = (char *)FUN_100069e0(param_1,"Game/campaign",0x1002536c);
    if (*pcVar1 != '\0') {
      uVar2 = 0;
      this = (void *)(*(code *)**(undefined4 **)PTR_PTR_10034018)();
      FUN_100058e0(this,pcVar1,uVar2);
    }
    (**(code **)(*(int *)PTR_PTR_10034018 + 4))(param_1,0);
  }
  return;
}


// 10004c90 FUN_10004c90

void FUN_10004c90(void)

{
  return;
}


// 10004cf0 FUN_10004cf0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10004cf0(int param_1,int param_2)

{
  int iVar1;
  
  iVar1 = 0;
  if (0 < param_2) {
    do {
      if (DAT_10037bb0 == 0) {
        DAT_10037bb0 = (**(code **)(param_1 + iVar1 * 4))("VFileSystem018",0);
      }
      if (DAT_10037bb4 == 0) {
        DAT_10037bb4 = (**(code **)(param_1 + iVar1 * 4))("VMaterialSystem080",0);
        _DAT_10037bb8 = DAT_10037bb4;
      }
      if (DAT_10037bbc == 0) {
        DAT_10037bbc = (**(code **)(param_1 + iVar1 * 4))("InputSystemVersion001",0);
      }
      if (DAT_10037bc0 == 0) {
        DAT_10037bc0 = (**(code **)(param_1 + iVar1 * 4))("NetworkSystemVersion001",0);
      }
      if (DAT_10037bc4 == 0) {
        DAT_10037bc4 = (**(code **)(param_1 + iVar1 * 4))("MaterialSystemHardwareConfig013",0);
      }
      if (DAT_10037bc8 == 0) {
        DAT_10037bc8 = (**(code **)(param_1 + iVar1 * 4))("DebugTextureInfo001",0);
      }
      if (DAT_10037bcc == 0) {
        DAT_10037bcc = (**(code **)(param_1 + iVar1 * 4))("VBAllocTracker001",0);
      }
      if (DAT_10037bd0 == 0) {
        DAT_10037bd0 = (**(code **)(param_1 + iVar1 * 4))("COLORCORRECTION_VERSION_1",0);
      }
      if (DAT_10037bd4 == 0) {
        DAT_10037bd4 = (**(code **)(param_1 + iVar1 * 4))("VP4001",0);
      }
      if (DAT_10037bd8 == 0) {
        DAT_10037bd8 = (**(code **)(param_1 + iVar1 * 4))("VMDLLIB001",0);
      }
      if (DAT_10037bdc == 0) {
        DAT_10037bdc = (**(code **)(param_1 + iVar1 * 4))("QueuedLoaderVersion001",0);
      }
      if (DAT_10037be0 == 0) {
        DAT_10037be0 = (**(code **)(param_1 + iVar1 * 4))("VResourceAccessControl001",0);
      }
      if (DAT_10037be4 == 0) {
        DAT_10037be4 = (**(code **)(param_1 + iVar1 * 4))("MATCHFRAMEWORK_001",0);
      }
      iVar1 = iVar1 + 1;
    } while (iVar1 < param_2);
  }
  return;
}


// 10004ea0 FUN_10004ea0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10004ea0(void)

{
  DAT_10037bb0 = 0;
  _DAT_10037bb8 = 0;
  DAT_10037bb4 = 0;
  DAT_10037bc4 = 0;
  DAT_10037bc8 = 0;
  DAT_10037bbc = 0;
  DAT_10037bc0 = 0;
  DAT_10037bd0 = 0;
  DAT_10037bd4 = 0;
  DAT_10037bd8 = 0;
  DAT_10037bdc = 0;
  DAT_10037be0 = 0;
  DAT_10037be4 = 0;
  return;
}


// 10004ef0 FUN_10004ef0

void __fastcall FUN_10004ef0(undefined4 param_1,size_t param_2,wchar_t *param_3,wchar_t *param_4)

{
  FID_conflict___vswprintf_p_l(param_3,param_2,param_4,(_locale_t)0x0,&stack0x0000000c);
  return;
}


// 10004f10 FUN_10004f10

void __thiscall FUN_10004f10(void *this,undefined4 param_1)

{
  int *piVar1;
  undefined4 uVar2;
  int iVar3;
  char *pcVar4;
  
  Warning("KeyValues Error: %s in file %s\n",param_1,*(undefined4 *)((int)this + 0x100));
  iVar3 = 0;
  if (0 < *(int *)((int)this + 0x108)) {
    do {
      if ((iVar3 < 0x40) && (*(int *)((int)this + iVar3 * 4) != -1)) {
        if (iVar3 < *(int *)((int)this + 0x104)) {
          piVar1 = (int *)KeyValuesSystem();
          uVar2 = (**(code **)(*piVar1 + 0x10))(*(undefined4 *)((int)this + iVar3 * 4));
          pcVar4 = "%s, ";
        }
        else {
          piVar1 = (int *)KeyValuesSystem();
          uVar2 = (**(code **)(*piVar1 + 0x10))(*(undefined4 *)((int)this + iVar3 * 4));
          pcVar4 = "(*%s*), ";
        }
        Warning(pcVar4,uVar2);
      }
      iVar3 = iVar3 + 1;
    } while (iVar3 < *(int *)((int)this + 0x108));
  }
  Warning(&DAT_10027aa4);
  return;
}


// 10004ff0 FUN_10004ff0

void __fastcall FUN_10004ff0(int param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  
  iVar2 = *(int *)(param_1 + 0x18);
  while (iVar2 != 0) {
    iVar1 = *(int *)(iVar2 + 0x14);
    *(undefined4 *)(iVar2 + 0x14) = 0;
    FUN_10004ff0(iVar2);
    piVar3 = (int *)KeyValuesSystem();
    (**(code **)(*piVar3 + 8))(iVar2);
    iVar2 = iVar1;
  }
  iVar2 = *(int *)(param_1 + 0x14);
  while ((iVar2 != 0 && (iVar2 != param_1))) {
    iVar1 = *(int *)(iVar2 + 0x14);
    *(undefined4 *)(iVar2 + 0x14) = 0;
    FUN_10004ff0(iVar2);
    piVar3 = (int *)KeyValuesSystem();
    (**(code **)(*piVar3 + 8))(iVar2);
    iVar2 = iVar1;
  }
  FUN_100012d0(*(undefined4 *)(param_1 + 4));
  *(undefined4 *)(param_1 + 4) = 0;
  FUN_100012d0(*(undefined4 *)(param_1 + 8));
  *(undefined4 *)(param_1 + 8) = 0;
  return;
}


// 10005080 FUN_10005080

undefined1 * __fastcall FUN_10005080(undefined4 *param_1)

{
  int *piVar1;
  undefined1 *puVar2;
  
  if (param_1 != (undefined4 *)0x0) {
    piVar1 = (int *)KeyValuesSystem();
    puVar2 = (undefined1 *)(**(code **)(*piVar1 + 0x10))(*param_1);
    return puVar2;
  }
  return &DAT_1002536c;
}


// 100050b0 FUN_100050b0

undefined1 * __thiscall
FUN_100050b0(void *this,int *param_1,undefined1 *param_2,undefined1 *param_3)

{
  char cVar1;
  bool bVar2;
  bool bVar3;
  uint uVar4;
  char *pcVar5;
  undefined4 *puVar6;
  int iVar7;
  int iVar8;
  
  *param_2 = 0;
  *param_3 = 0;
  if ((char)param_1[5] == '\0') {
    FUN_1000b760(param_1);
    cVar1 = (char)param_1[5];
    while (cVar1 == '\0') {
      uVar4 = FUN_1000bf20(param_1);
      if ((char)uVar4 == '\0') {
        pcVar5 = (char *)FUN_1000b4a0(param_1,1,0);
        if (pcVar5 == (char *)0x0) {
          return (undefined1 *)0x0;
        }
        cVar1 = *pcVar5;
        if (cVar1 == '\"') {
          *param_2 = 1;
          if (*(char *)((int)this + 0x11) == '\0') {
            puVar6 = (undefined4 *)FUN_1000b270();
          }
          else {
            puVar6 = (undefined4 *)FUN_1000b260();
          }
          FUN_1000c0e0(param_1,puVar6,&DAT_10037c00,0x400);
          return &DAT_10037c00;
        }
        if ((cVar1 == '{') || (cVar1 == '}')) {
          DAT_10037c01 = 0;
          DAT_10037c00 = cVar1;
          FUN_1000b4e0(param_1,1,1);
          return &DAT_10037c00;
        }
        iVar8 = 0;
        bVar3 = false;
        bVar2 = false;
        pcVar5 = (char *)FUN_1000b4a0(param_1,1,0);
        goto joined_r0x1000516e;
      }
      FUN_1000b760(param_1);
      cVar1 = (char)param_1[5];
    }
  }
  return (undefined1 *)0x0;
joined_r0x1000516e:
  if ((((pcVar5 == (char *)0x0) || (cVar1 = *pcVar5, cVar1 == '\0')) || (cVar1 == '\"')) ||
     ((cVar1 == '{' || (cVar1 == '}')))) {
LAB_10005209:
    (&DAT_10037c00)[iVar8] = 0;
    return &DAT_10037c00;
  }
  if (cVar1 == '[') {
    bVar2 = true;
  }
  if ((cVar1 == ']') && (bVar2)) {
    bVar2 = false;
    *param_3 = 1;
  }
  iVar7 = _isspace((int)*pcVar5);
  if ((iVar7 != 0) && (!bVar2)) goto LAB_10005209;
  if (iVar8 < 0x3ff) {
    (&DAT_10037c00)[iVar8] = *pcVar5;
    iVar8 = iVar8 + 1;
  }
  else if (!bVar3) {
    bVar3 = true;
    FUN_10004f10(&DAT_10034fe8," ReadToken overflow");
  }
  FUN_1000b4e0(param_1,1,1);
  pcVar5 = (char *)FUN_1000b4a0(param_1,1,0);
  goto joined_r0x1000516e;
}


// 10005240 FUN_10005240

void __thiscall FUN_10005240(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  
  if (param_1 != 0) {
    iVar2 = *(int *)((int)this + 0x18);
    if (iVar2 == param_1) {
      *(undefined4 *)((int)this + 0x18) = *(undefined4 *)(param_1 + 0x14);
      *(undefined4 *)(param_1 + 0x14) = 0;
      return;
    }
    if (*(int *)(iVar2 + 0x14) != 0) {
      while (iVar1 = *(int *)(iVar2 + 0x14), iVar1 != param_1) {
        iVar2 = iVar1;
        if (*(int *)(iVar1 + 0x14) == 0) {
          *(undefined4 *)(param_1 + 0x14) = 0;
          return;
        }
      }
      *(undefined4 *)(iVar2 + 0x14) = *(undefined4 *)(param_1 + 0x14);
    }
    *(undefined4 *)(param_1 + 0x14) = 0;
  }
  return;
}


// 10005290 FUN_10005290

int FUN_10005290(void)

{
  int iVar1;
  int in_ECX;
  
  if (in_ECX != 0) {
    for (iVar1 = *(int *)(in_ECX + 0x18); (iVar1 != 0 && (*(char *)(iVar1 + 0x10) != '\0'));
        iVar1 = *(int *)(iVar1 + 0x14)) {
    }
    return iVar1;
  }
  return 0;
}


// 100052c0 FUN_100052c0

int FUN_100052c0(void)

{
  int iVar1;
  int in_ECX;
  
  if (in_ECX != 0) {
    for (iVar1 = *(int *)(in_ECX + 0x14); (iVar1 != 0 && (*(char *)(iVar1 + 0x10) != '\0'));
        iVar1 = *(int *)(iVar1 + 0x14)) {
    }
    return iVar1;
  }
  return 0;
}


// 100052f0 FUN_100052f0

int FUN_100052f0(void)

{
  int iVar1;
  int in_ECX;
  
  if (in_ECX != 0) {
    for (iVar1 = *(int *)(in_ECX + 0x18); (iVar1 != 0 && (*(char *)(iVar1 + 0x10) == '\0'));
        iVar1 = *(int *)(iVar1 + 0x14)) {
    }
    return iVar1;
  }
  return 0;
}


// 10005320 FUN_10005320

int FUN_10005320(void)

{
  int iVar1;
  int in_ECX;
  
  if (in_ECX != 0) {
    for (iVar1 = *(int *)(in_ECX + 0x14); (iVar1 != 0 && (*(char *)(iVar1 + 0x10) == '\0'));
        iVar1 = *(int *)(iVar1 + 0x14)) {
    }
    return iVar1;
  }
  return 0;
}


// 10005350 FUN_10005350

void __thiscall FUN_10005350(void *this,undefined4 param_1)

{
  int *piVar1;
  undefined4 uVar2;
  
  piVar1 = (int *)KeyValuesSystem();
  uVar2 = (**(code **)(*piVar1 + 0xc))(param_1,1);
  *(undefined4 *)this = uVar2;
  return;
}


// 10005370 FUN_10005370

void __thiscall FUN_10005370(void *this,int param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  
  puVar2 = (undefined4 *)0x0;
  for (puVar1 = *(undefined4 **)((int)this + 0x18); puVar1 != (undefined4 *)0x0;
      puVar1 = (undefined4 *)puVar1[5]) {
    puVar3 = FUN_10005e40(puVar1);
    if (puVar2 == (undefined4 *)0x0) {
      *(undefined4 **)(param_1 + 0x18) = puVar3;
    }
    else {
      puVar2[5] = puVar3;
    }
    puVar3[5] = 0;
    puVar2 = puVar3;
  }
  return;
}


// 100053b0 FUN_100053b0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

bool FUN_100053b0(void)

{
  char cVar1;
  int iVar2;
  char *pcVar3;
  uint uVar4;
  char *unaff_ESI;
  bool bVar5;
  byte local_40 [64];
  
  iVar2 = FUN_10009820(unaff_ESI);
  if (iVar2 < 4) {
    bVar5 = false;
  }
  else {
    cVar1 = unaff_ESI[2];
    pcVar3 = unaff_ESI + 2;
    if (cVar1 == '!') {
      pcVar3 = unaff_ESI + 3;
    }
    FUN_10009c10((char *)local_40,pcVar3,0x40);
    iVar2 = FUN_10009820((char *)local_40);
    local_40[iVar2 + -1] = 0;
    if ((_DAT_100380bc & 1) == 0) {
      _DAT_100380bc = _DAT_100380bc | 1;
      FUN_1000ab40(&DAT_100380b4,"cl_language");
    }
    uVar4 = FUN_100098d0(local_40,*(byte **)(DAT_100380b8 + 0x24));
    bVar5 = uVar4 == 0;
    if (cVar1 == '!') {
      return !bVar5;
    }
  }
  return bVar5;
}


// 10005450 FUN_10005450

bool FUN_10005450(void)

{
  byte *in_EAX;
  char *pcVar1;
  uint uVar2;
  bool bVar3;
  
  pcVar1 = FUN_10009a10((char *)in_EAX,"$OSX");
  if (pcVar1 == (char *)0x0) {
    pcVar1 = FUN_10009a10((char *)in_EAX,"$LINUX");
    if (pcVar1 == (char *)0x0) {
      pcVar1 = FUN_10009a10((char *)in_EAX,"$POSIX");
      if (pcVar1 == (char *)0x0) {
        pcVar1 = FUN_10009a10((char *)in_EAX,"$WINDOWS");
        if (pcVar1 != (char *)0x0) {
          return true;
        }
        uVar2 = FUN_100098d0((byte *)"[$WIN32]",in_EAX);
        bVar3 = uVar2 == 0;
        if (!bVar3) {
          bVar3 = FUN_100053b0();
        }
        return bVar3;
      }
    }
  }
  return false;
}


// 100054d0 FUN_100054d0

void __cdecl FUN_100054d0(undefined4 param_1)

{
  int *piVar1;
  
  piVar1 = (int *)KeyValuesSystem();
  (**(code **)(*piVar1 + 4))(param_1);
  return;
}


// 100054f0 FUN_100054f0

char * __cdecl FUN_100054f0(undefined4 *param_1)

{
  char cVar1;
  char *in_EAX;
  int iVar2;
  char *pcVar3;
  
  iVar2 = _isspace((int)*in_EAX);
  while (iVar2 != 0) {
    pcVar3 = in_EAX + 1;
    in_EAX = in_EAX + 1;
    iVar2 = _isspace((int)*pcVar3);
  }
  cVar1 = *in_EAX;
  pcVar3 = in_EAX;
  while ((cVar1 != '\0' && (iVar2 = _isspace((int)cVar1), iVar2 == 0))) {
    cVar1 = pcVar3[1];
    pcVar3 = pcVar3 + 1;
  }
  if (param_1 != (undefined4 *)0x0) {
    *param_1 = pcVar3;
  }
  return in_EAX;
}


// 10005560 FUN_10005560

uint __thiscall FUN_10005560(void *this,int *param_1,int param_2)

{
  uint uVar1;
  void *this_00;
  int iVar2;
  int iVar3;
  int unaff_retaddr;
  
  uVar1 = (**(code **)*param_1)(this,param_2);
  if ((char)uVar1 == '\0') {
    return uVar1;
  }
  if (this != (void *)0x0) {
    for (iVar3 = *(int *)((int)this + 0x18); (iVar3 != 0 && (*(char *)(iVar3 + 0x10) == '\0'));
        iVar3 = *(int *)(iVar3 + 0x14)) {
    }
    if (iVar3 != 0) {
      iVar2 = param_2 + 1;
      do {
        uVar1 = (**(code **)(*param_1 + 4))(iVar3,iVar2);
        if ((char)uVar1 == '\0') goto LAB_10005651;
        if (iVar3 == 0) {
          iVar3 = 0;
        }
        else {
          for (iVar3 = *(int *)(iVar3 + 0x14); (iVar3 != 0 && (*(char *)(iVar3 + 0x10) == '\0'));
              iVar3 = *(int *)(iVar3 + 0x14)) {
          }
        }
        param_2 = unaff_retaddr;
      } while (iVar3 != 0);
    }
    for (this_00 = *(void **)((int)this + 0x18);
        (this_00 != (void *)0x0 && (*(char *)((int)this_00 + 0x10) != '\0'));
        this_00 = *(void **)((int)this_00 + 0x14)) {
    }
    if (this_00 != (void *)0x0) {
      iVar3 = param_2 + 1;
      do {
        uVar1 = FUN_10005560(this_00,param_1,iVar3);
        if ((char)uVar1 == '\0') {
LAB_10005651:
          return uVar1 & 0xffffff00;
        }
        if (this_00 == (void *)0x0) {
          this_00 = (void *)0x0;
        }
        else {
          for (this_00 = *(void **)((int)this_00 + 0x14);
              (this_00 != (void *)0x0 && (*(char *)((int)this_00 + 0x10) != '\0'));
              this_00 = *(void **)((int)this_00 + 0x14)) {
          }
        }
        param_2 = unaff_retaddr;
      } while (this_00 != (void *)0x0);
    }
  }
  uVar1 = (**(code **)(*param_1 + 8))(this,param_2);
  return uVar1;
}


// 10005660 FUN_10005660

undefined4 __thiscall FUN_10005660(void *this,undefined4 *param_1,undefined4 param_2)

{
  char cVar1;
  int *piVar2;
  undefined4 uVar3;
  
  if (param_1 == (undefined4 *)0x0) {
                    /* WARNING: Load size is inaccurate */
    cVar1 = (**(code **)(*this + 0xc))(param_2);
    if (cVar1 == '\0') {
      return 0;
    }
                    /* WARNING: Load size is inaccurate */
    cVar1 = (**(code **)(*this + 0x10))("<< NULL >>\n");
  }
  else {
                    /* WARNING: Load size is inaccurate */
    cVar1 = (**(code **)(*this + 0xc))(param_2);
    if (cVar1 == '\0') {
      return 0;
    }
    piVar2 = (int *)KeyValuesSystem();
    uVar3 = (**(code **)(*piVar2 + 0x10))(*param_1);
                    /* WARNING: Load size is inaccurate */
    cVar1 = (**(code **)(*this + 0x10))(uVar3);
    if (cVar1 == '\0') {
      return 0;
    }
                    /* WARNING: Load size is inaccurate */
    cVar1 = (**(code **)(*this + 0x10))(&DAT_10027b38);
  }
  if (cVar1 == '\0') {
    return 0;
  }
  return 1;
}


// 10005740 FUN_10005740

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

void __thiscall FUN_10005740(void *this,int param_1)

{
  code *pcVar1;
  int iVar2;
  undefined4 uStack_20;
  int aiStack_1c [2];
  size_t sStack_14;
  
  sStack_14 = 0x10005757;
  iVar2 = -(param_1 * 2 + 0x10U & 0xfffffff0);
  *(int *)((int)&sStack_14 + iVar2) = param_1 * 2;
  *(undefined4 *)((int)aiStack_1c + iVar2 + 4) = 0x20;
  *(undefined1 **)((int)aiStack_1c + iVar2) = &stack0xfffffff0 + iVar2;
  *(undefined4 *)((int)&uStack_20 + iVar2) = 0x10005765;
  _memset(*(void **)((int)aiStack_1c + iVar2),*(int *)((int)aiStack_1c + iVar2 + 4),
          *(size_t *)((int)&sStack_14 + iVar2));
  (&stack0xfffffff0)[param_1 * 2 + iVar2] = 0;
                    /* WARNING: Load size is inaccurate */
  pcVar1 = *(code **)(*this + 0x10);
  *(undefined1 **)((int)&sStack_14 + iVar2) = &stack0xfffffff0 + iVar2;
  *(undefined4 *)((int)aiStack_1c + iVar2 + 4) = 0x10005776;
  (*pcVar1)();
  return;
}


// 10005820 FUN_10005820

void __cdecl FUN_10005820(void *param_1,undefined1 *param_2)

{
  undefined1 local_8;
  undefined1 uStack_7;
  undefined1 uStack_6;
  undefined1 uStack_5;
  undefined1 local_4;
  undefined1 uStack_3;
  undefined1 uStack_2;
  undefined1 uStack_1;
  
  _local_8 = CONCAT13(param_2[4],CONCAT12(param_2[5],CONCAT11(param_2[6],param_2[7])));
  _local_4 = CONCAT13(*param_2,CONCAT12(param_2[1],CONCAT11(param_2[2],param_2[3])));
  FUN_10009bb0(param_1,&local_8,8);
  return;
}


// 10005890 FUN_10005890

undefined4 * __thiscall FUN_10005890(void *this,undefined4 param_1)

{
  int *piVar1;
  undefined4 uVar2;
  
  *(undefined4 *)this = 0xffffffff;
  *(undefined1 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x18) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 0x1c) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined4 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined1 *)((int)this + 0x11) = 0;
  *(undefined4 *)((int)this + 0x20) = 0;
  *(undefined2 *)((int)this + 0x12) = 0;
  piVar1 = (int *)KeyValuesSystem();
  uVar2 = (**(code **)(*piVar1 + 0xc))(param_1,1);
  *(undefined4 *)this = uVar2;
  return (undefined4 *)this;
}


// 100058e0 FUN_100058e0

int * __thiscall FUN_100058e0(void *this,char *param_1,undefined4 param_2)

{
  char *pcVar1;
  int *piVar2;
  int iVar3;
  int *piVar4;
  void *this_00;
  int unaff_EBP;
  char *pcVar5;
  int unaff_ESI;
  undefined4 unaff_retaddr;
  char local_100;
  undefined1 local_ff [255];
  
  if (this == (void *)0x0) {
    return (int *)this;
  }
  if ((param_1 != (char *)0x0) && (*param_1 != '\0')) {
    local_100 = '\0';
    _memset(local_ff,0,0xff);
    pcVar1 = _strchr(param_1,0x2f);
    pcVar5 = param_1;
    if (pcVar1 != (char *)0x0) {
      pcVar1 = pcVar1 + (1 - (int)param_1);
      if (0xff < (int)pcVar1) {
        pcVar1 = (char *)0x100;
      }
      FUN_10009c10(&local_100,param_1,(size_t)pcVar1);
      pcVar5 = &local_100;
    }
    piVar2 = (int *)KeyValuesSystem();
    iVar3 = (**(code **)(*piVar2 + 0xc))(pcVar5,param_2);
    if (iVar3 == -1) {
      return (int *)0x0;
    }
    piVar4 = *(int **)(unaff_EBP + 0x18);
    piVar2 = (int *)0x0;
    while (this = piVar4, (int *)this != (int *)0x0) {
                    /* WARNING: Load size is inaccurate */
      if (*this == iVar3) goto LAB_10005a05;
      piVar2 = (int *)this;
      piVar4 = *(int **)((int)this + 0x14);
    }
    if (*(void **)(unaff_EBP + 0x1c) != (void *)0x0) {
      this = FUN_100058e0(*(void **)(unaff_EBP + 0x1c),param_1,0);
    }
    if ((int *)this == (int *)0x0) {
      if ((char)unaff_retaddr == '\0') {
        return (int *)0x0;
      }
      piVar4 = (int *)KeyValuesSystem();
      this_00 = (void *)(**(code **)(*piVar4 + 4))(0x24);
      if (this_00 == (void *)0x0) {
        this = (int *)0x0;
      }
      else {
        this = FUN_10005890(this_00,pcVar5);
      }
      if (piVar2 == (int *)0x0) {
        *(void **)(unaff_EBP + 0x18) = this;
      }
      else {
        piVar2[5] = (int)this;
      }
      *(int *)((int)this + 0x14) = 0;
      *(undefined1 *)(unaff_EBP + 0x10) = 0;
    }
LAB_10005a05:
    if (unaff_ESI != 0) {
      this = FUN_100058e0(this,(char *)(unaff_ESI + 1),unaff_retaddr);
    }
  }
  return (int *)this;
}


// 10005a40 FUN_10005a40

void __thiscall FUN_10005a40(void *this,undefined4 param_1)

{
  int iVar1;
  int iVar2;
  
  iVar2 = *(int *)((int)this + 0x18);
  if (*(int *)((int)this + 0x18) == 0) {
    *(undefined4 *)((int)this + 0x18) = param_1;
    return;
  }
  do {
    iVar1 = *(int *)(iVar2 + 0x14);
    if (iVar1 == 0) break;
    iVar2 = iVar1;
  } while (iVar1 != 0);
  *(undefined4 *)(iVar2 + 0x14) = param_1;
  return;
}


// 10005a70 FUN_10005a70

int __thiscall FUN_10005a70(void *this,char *param_1,int param_2)

{
  int *piVar1;
  long lVar2;
  int iVar3;
  undefined4 extraout_EDX;
  ulonglong uVar4;
  
  piVar1 = FUN_100058e0(this,param_1,0);
  if (piVar1 != (int *)0x0) {
    iVar3 = (char)piVar1[4] + -1;
    switch(iVar3) {
    case 0:
      lVar2 = _atol((char *)piVar1[1]);
      return lVar2;
    default:
      return piVar1[3];
    case 2:
      uVar4 = FUN_1000ffe0(iVar3,extraout_EDX);
      return (int)uVar4;
    case 4:
      iVar3 = thunk_FUN_1000ffae((wchar_t *)piVar1[2]);
      return iVar3;
    case 6:
      return 0;
    }
  }
  return param_2;
}


// 10005af0 FUN_10005af0

longlong __thiscall FUN_10005af0(void *this,char *param_1,undefined4 param_2,undefined4 param_3)

{
  int *piVar1;
  int iVar2;
  undefined4 extraout_EDX;
  ulonglong uVar3;
  
  piVar1 = FUN_100058e0(this,param_1,0);
  if (piVar1 != (int *)0x0) {
    iVar2 = (char)piVar1[4] + -1;
    switch(iVar2) {
    case 0:
      param_2 = 0;
      param_3 = 0;
      FID_conflict__sscanf((char *)piVar1[1],"%lld",&param_2);
      break;
    default:
      return (longlong)piVar1[3];
    case 2:
      uVar3 = FUN_1000ffe0(iVar2,extraout_EDX);
      return (longlong)(int)uVar3;
    case 4:
      param_2 = 0;
      param_3 = 0;
      FID_conflict__swscanf((wchar_t *)piVar1[2],L"%lld",&param_2);
      return CONCAT44(param_3,param_2);
    case 6:
      return *(longlong *)piVar1[1];
    }
  }
  return CONCAT44(param_3,param_2);
}


// 10005bb0 FUN_10005bb0

int __thiscall FUN_10005bb0(void *this,char *param_1,int param_2)

{
  int *piVar1;
  
  piVar1 = FUN_100058e0(this,param_1,0);
  if (piVar1 == (int *)0x0) {
    return param_2;
  }
  if ((char)piVar1[4] != '\x04') {
    return 0;
  }
  return piVar1[3];
}


// 10005be0 FUN_10005be0

float10 __thiscall FUN_10005be0(void *param_1,char *param_2,float param_3)

{
  int *piVar1;
  double dVar2;
  
  piVar1 = FUN_100058e0(param_1,param_2,0);
  if (piVar1 != (int *)0x0) {
    switch((char)piVar1[4]) {
    case '\x01':
      dVar2 = _atof((char *)piVar1[1]);
      return (float10)dVar2;
    case '\x02':
      return (float10)piVar1[3];
    case '\x03':
      return (float10)(float)piVar1[3];
    default:
      return (float10)0;
    case '\x05':
      dVar2 = __wtof((wchar_t *)piVar1[2]);
      return (float10)dVar2;
    case '\a':
      return -(float10)(longlong)(((ulonglong)*(uint *)(piVar1[1] + 4) & 0x80000000) << 0x20) +
             (float10)(*(ulonglong *)piVar1[1] & 0x7fffffffffffffff);
    }
  }
  return (float10)param_3;
}


// 10005cb0 FUN_10005cb0

void __thiscall FUN_10005cb0(void *this,char *param_1,char *param_2)

{
  int *piVar1;
  int iVar2;
  void *pvVar3;
  
  piVar1 = FUN_100058e0(this,param_1,1);
  if (piVar1 != (int *)0x0) {
    FUN_100012d0(piVar1[1]);
    FUN_100012d0(piVar1[2]);
    piVar1[2] = 0;
    if (param_2 == (char *)0x0) {
      param_2 = "";
    }
    iVar2 = FUN_10009820(param_2);
    pvVar3 = (void *)FUN_10001270(iVar2 + 1U);
    piVar1[1] = (int)pvVar3;
    FUN_10009bb0(pvVar3,param_2,iVar2 + 1U);
    *(undefined1 *)(piVar1 + 4) = 1;
  }
  return;
}


// 10005d20 FUN_10005d20

void __thiscall FUN_10005d20(void *this,char *param_1,short *param_2)

{
  short sVar1;
  longlong lVar2;
  int *piVar3;
  short *psVar4;
  int iVar5;
  void *pvVar6;
  
  piVar3 = FUN_100058e0(this,param_1,1);
  if (piVar3 != (int *)0x0) {
    FUN_100012d0(piVar3[2]);
    FUN_100012d0(piVar3[1]);
    piVar3[1] = 0;
    if (param_2 == (short *)0x0) {
      param_2 = &DAT_10027b6c;
    }
    psVar4 = param_2;
    do {
      sVar1 = *psVar4;
      psVar4 = psVar4 + 1;
    } while (sVar1 != 0);
    iVar5 = (int)psVar4 - (int)(param_2 + 1) >> 1;
    lVar2 = (ulonglong)(iVar5 + 1) * 2;
    pvVar6 = (void *)FUN_10001270(-(uint)((int)((ulonglong)lVar2 >> 0x20) != 0) | (uint)lVar2);
    piVar3[2] = (int)pvVar6;
    FUN_10009bb0(pvVar6,param_2,iVar5 * 2 + 2);
    *(undefined1 *)(piVar3 + 4) = 5;
  }
  return;
}


// 10005db0 FUN_10005db0

void __thiscall FUN_10005db0(void *this,char *param_1,int param_2)

{
  int *piVar1;
  
  piVar1 = FUN_100058e0(this,param_1,1);
  if (piVar1 != (int *)0x0) {
    piVar1[3] = param_2;
    *(undefined1 *)(piVar1 + 4) = 2;
  }
  return;
}


// 10005dd0 FUN_10005dd0

void __thiscall FUN_10005dd0(void *this,char *param_1,undefined4 param_2,undefined4 param_3)

{
  int *piVar1;
  undefined4 *puVar2;
  
  piVar1 = FUN_100058e0(this,param_1,1);
  if (piVar1 != (int *)0x0) {
    FUN_100012d0(piVar1[1]);
    FUN_100012d0(piVar1[2]);
    piVar1[2] = 0;
    puVar2 = (undefined4 *)FUN_10001270(8);
    piVar1[1] = (int)puVar2;
    *puVar2 = param_2;
    puVar2[1] = param_3;
    *(undefined1 *)(piVar1 + 4) = 7;
  }
  return;
}


// 10005e20 FUN_10005e20

void __thiscall FUN_10005e20(void *this,char *param_1,int param_2)

{
  int *piVar1;
  
  piVar1 = FUN_100058e0(this,param_1,1);
  if (piVar1 != (int *)0x0) {
    piVar1[3] = param_2;
    *(undefined1 *)(piVar1 + 4) = 4;
  }
  return;
}


// 10005e40 FUN_10005e40

undefined4 * __fastcall FUN_10005e40(undefined4 *param_1)

{
  short *psVar1;
  short sVar2;
  longlong lVar3;
  int *piVar4;
  void *pvVar5;
  undefined4 uVar6;
  undefined4 *puVar7;
  short *psVar8;
  int iVar9;
  
  piVar4 = (int *)KeyValuesSystem();
  pvVar5 = (void *)(**(code **)(*piVar4 + 4))(0x24);
  if (pvVar5 == (void *)0x0) {
    puVar7 = (undefined4 *)0x0;
  }
  else if (param_1 == (undefined4 *)0x0) {
    puVar7 = FUN_10005890(pvVar5,&DAT_1002536c);
  }
  else {
    piVar4 = (int *)KeyValuesSystem();
    uVar6 = (**(code **)(*piVar4 + 0x10))(*param_1);
    puVar7 = FUN_10005890(pvVar5,uVar6);
  }
  *(undefined1 *)(puVar7 + 4) = *(undefined1 *)(param_1 + 4);
  switch(*(undefined1 *)(param_1 + 4)) {
  case 1:
    if ((char *)param_1[1] != (char *)0x0) {
      iVar9 = FUN_10009820((char *)param_1[1]);
      pvVar5 = (void *)FUN_10001270(iVar9 + 1U);
      puVar7[1] = pvVar5;
      FUN_10009bb0(pvVar5,(void *)param_1[1],iVar9 + 1U);
      FUN_10005370(param_1,(int)puVar7);
      return puVar7;
    }
    break;
  case 2:
    puVar7[3] = param_1[3];
    FUN_10005370(param_1,(int)puVar7);
    return puVar7;
  case 3:
    puVar7[3] = param_1[3];
    FUN_10005370(param_1,(int)puVar7);
    return puVar7;
  case 4:
    puVar7[3] = param_1[3];
    FUN_10005370(param_1,(int)puVar7);
    return puVar7;
  case 5:
    psVar8 = (short *)param_1[2];
    if (psVar8 != (short *)0x0) {
      psVar1 = psVar8 + 1;
      do {
        sVar2 = *psVar8;
        psVar8 = psVar8 + 1;
      } while (sVar2 != 0);
      iVar9 = (int)psVar8 - (int)psVar1 >> 1;
      lVar3 = (ulonglong)(iVar9 + 1) * 2;
      pvVar5 = (void *)FUN_10001270(-(uint)((int)((ulonglong)lVar3 >> 0x20) != 0) | (uint)lVar3);
      puVar7[2] = pvVar5;
      FUN_10009bb0(pvVar5,(void *)param_1[2],iVar9 * 2 + 2);
      FUN_10005370(param_1,(int)puVar7);
      return puVar7;
    }
    break;
  case 6:
    *(undefined1 *)(puVar7 + 3) = *(undefined1 *)(param_1 + 3);
    *(undefined1 *)((int)puVar7 + 0xd) = *(undefined1 *)((int)param_1 + 0xd);
    *(undefined1 *)((int)puVar7 + 0xe) = *(undefined1 *)((int)param_1 + 0xe);
    *(undefined1 *)((int)puVar7 + 0xf) = *(undefined1 *)((int)param_1 + 0xf);
    FUN_10005370(param_1,(int)puVar7);
    return puVar7;
  case 7:
    pvVar5 = (void *)FUN_10001270(8);
    puVar7[1] = pvVar5;
    FUN_10009bb0(pvVar5,(void *)param_1[1],8);
  }
  FUN_10005370(param_1,(int)puVar7);
  return puVar7;
}


// 10005ff0 FUN_10005ff0

void __fastcall FUN_10005ff0(int param_1)

{
  int *piVar1;
  
  if (param_1 != 0) {
    FUN_10004ff0(param_1);
    piVar1 = (int *)KeyValuesSystem();
    (**(code **)(*piVar1 + 8))(param_1);
  }
  return;
}


// 10006010 FUN_10006010

void __thiscall FUN_10006010(void *this,int param_1)

{
  undefined4 *puVar1;
  int iVar2;
  int *piVar3;
  byte *pbVar4;
  byte *pbVar5;
  int iVar6;
  undefined4 *puVar7;
  
  puVar1 = *(undefined4 **)(param_1 + 0x18);
  do {
    if (puVar1 == (undefined4 *)0x0) {
      return;
    }
    for (puVar7 = *(undefined4 **)((int)this + 0x18); puVar7 != (undefined4 *)0x0;
        puVar7 = (undefined4 *)puVar7[5]) {
      piVar3 = (int *)KeyValuesSystem();
      pbVar4 = (byte *)(**(code **)(*piVar3 + 0x10))(*puVar7);
      if (puVar1 == (undefined4 *)0x0) {
        pbVar5 = &DAT_1002536c;
      }
      else {
        piVar3 = (int *)KeyValuesSystem();
        pbVar5 = (byte *)(**(code **)(*piVar3 + 0x10))(*puVar1);
      }
      iVar6 = FUN_10009870(pbVar5,pbVar4);
      if (iVar6 == 0) {
        FUN_10006010(puVar7,(int)puVar1);
        goto LAB_100060b2;
      }
    }
    puVar7 = FUN_10005e40(puVar1);
    iVar6 = *(int *)((int)this + 0x18);
    if (*(int *)((int)this + 0x18) == 0) {
      *(undefined4 **)((int)this + 0x18) = puVar7;
    }
    else {
      do {
        iVar2 = *(int *)(iVar6 + 0x14);
        if (iVar2 == 0) break;
        iVar6 = iVar2;
      } while (iVar2 != 0);
      *(undefined4 **)(iVar6 + 0x14) = puVar7;
    }
LAB_100060b2:
    puVar1 = (undefined4 *)puVar1[5];
  } while( true );
}


// 100060d0 FUN_100060d0

void __thiscall FUN_100060d0(void *this,int *param_1,undefined4 param_2)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  char *pcVar4;
  undefined4 *puVar5;
  undefined4 *puVar6;
  int iVar7;
  undefined4 uVar8;
  
  if (this == (void *)0x0) {
    return;
  }
  do {
    if (param_1 == (int *)0x0) {
      return;
    }
    switch(param_2) {
    case 0:
      uVar8 = 1;
      piVar2 = FUN_100058e0(param_1,"Update",0);
      FUN_100060d0(this,piVar2,uVar8);
      uVar8 = 2;
      piVar2 = FUN_100058e0(param_1,"Delete",0);
      FUN_100060d0(this,piVar2,uVar8);
      param_2 = 3;
      param_1 = FUN_100058e0(param_1,"Borrow",0);
      break;
    case 1:
      piVar2 = (int *)FUN_10005290();
      while (piVar2 != (int *)0x0) {
        piVar3 = (int *)KeyValuesSystem();
        pcVar4 = (char *)(**(code **)(*piVar3 + 0x10))(*piVar2);
        piVar3 = FUN_100058e0(this,pcVar4,0);
        if (piVar3 == (int *)0x0) {
          puVar5 = FUN_10005e40(piVar2);
          FUN_10005a40(this,puVar5);
        }
        else {
          FUN_100060d0(piVar3,piVar2,1);
        }
        piVar2 = (int *)FUN_100052c0();
      }
      puVar5 = (undefined4 *)FUN_100052f0();
      if (puVar5 == (undefined4 *)0x0) {
        return;
      }
      do {
        if (puVar5 == (undefined4 *)0x0) {
          pcVar4 = "";
        }
        else {
          piVar2 = (int *)KeyValuesSystem();
          pcVar4 = (char *)(**(code **)(*piVar2 + 0x10))(*puVar5);
        }
        piVar2 = FUN_100058e0(this,pcVar4,0);
        if (piVar2 != (int *)0x0) {
          FUN_10005240(this,(int)piVar2);
          FUN_10004ff0((int)piVar2);
          piVar3 = (int *)KeyValuesSystem();
          (**(code **)(*piVar3 + 8))(piVar2);
        }
        puVar6 = FUN_10005e40(puVar5);
        iVar7 = *(int *)((int)this + 0x18);
        if (*(int *)((int)this + 0x18) == 0) {
          *(undefined4 **)((int)this + 0x18) = puVar6;
        }
        else {
          do {
            iVar1 = *(int *)(iVar7 + 0x14);
            if (iVar1 == 0) break;
            iVar7 = iVar1;
          } while (iVar1 != 0);
          *(undefined4 **)(iVar7 + 0x14) = puVar6;
        }
        if (puVar5 == (undefined4 *)0x0) {
          puVar5 = (undefined4 *)0x0;
        }
        else {
          for (puVar5 = (undefined4 *)puVar5[5];
              (puVar5 != (undefined4 *)0x0 && (*(char *)(puVar5 + 4) == '\0'));
              puVar5 = (undefined4 *)puVar5[5]) {
          }
        }
        if (puVar5 == (undefined4 *)0x0) {
          return;
        }
      } while( true );
    case 2:
      piVar2 = (int *)FUN_10005290();
      while (piVar2 != (int *)0x0) {
        piVar3 = (int *)KeyValuesSystem();
        pcVar4 = (char *)(**(code **)(*piVar3 + 0x10))(*piVar2);
        piVar3 = FUN_100058e0(this,pcVar4,0);
        if (piVar3 != (int *)0x0) {
          FUN_100060d0(piVar3,piVar2,2);
        }
        piVar2 = (int *)FUN_100052c0();
      }
      puVar5 = (undefined4 *)FUN_100052f0();
      while (puVar5 != (undefined4 *)0x0) {
        if (puVar5 == (undefined4 *)0x0) {
          pcVar4 = "";
        }
        else {
          piVar2 = (int *)KeyValuesSystem();
          pcVar4 = (char *)(**(code **)(*piVar2 + 0x10))(*puVar5);
        }
        piVar2 = FUN_100058e0(this,pcVar4,0);
        if (piVar2 != (int *)0x0) {
          FUN_10005240(this,(int)piVar2);
          FUN_10004ff0((int)piVar2);
          piVar3 = (int *)KeyValuesSystem();
          (**(code **)(*piVar3 + 8))(piVar2);
        }
        if (puVar5 == (undefined4 *)0x0) {
          puVar5 = (undefined4 *)0x0;
        }
        else {
          for (puVar5 = (undefined4 *)puVar5[5];
              (puVar5 != (undefined4 *)0x0 && (*(char *)(puVar5 + 4) == '\0'));
              puVar5 = (undefined4 *)puVar5[5]) {
          }
        }
      }
      return;
    case 3:
      piVar2 = (int *)FUN_10005290();
      while (piVar2 != (int *)0x0) {
        piVar3 = (int *)KeyValuesSystem();
        pcVar4 = (char *)(**(code **)(*piVar3 + 0x10))(*piVar2);
        piVar3 = FUN_100058e0(this,pcVar4,0);
        if (piVar3 != (int *)0x0) {
          FUN_100060d0(piVar3,piVar2,3);
        }
        piVar2 = (int *)FUN_100052c0();
      }
      puVar5 = (undefined4 *)FUN_100052f0();
      if (puVar5 == (undefined4 *)0x0) {
        return;
      }
      do {
        if (puVar5 == (undefined4 *)0x0) {
          pcVar4 = "";
        }
        else {
          piVar2 = (int *)KeyValuesSystem();
          pcVar4 = (char *)(**(code **)(*piVar2 + 0x10))(*puVar5);
        }
        piVar2 = FUN_100058e0(this,pcVar4,0);
        if (piVar2 != (int *)0x0) {
          FUN_10005240(this,(int)piVar2);
          FUN_10004ff0((int)piVar2);
          piVar3 = (int *)KeyValuesSystem();
          (**(code **)(*piVar3 + 8))(piVar2);
          puVar6 = FUN_10005e40(puVar5);
          iVar7 = *(int *)((int)this + 0x18);
          if (*(int *)((int)this + 0x18) == 0) {
            *(undefined4 **)((int)this + 0x18) = puVar6;
          }
          else {
            do {
              iVar1 = *(int *)(iVar7 + 0x14);
              if (iVar1 == 0) break;
              iVar7 = iVar1;
            } while (iVar1 != 0);
            *(undefined4 **)(iVar7 + 0x14) = puVar6;
          }
        }
        if (puVar5 == (undefined4 *)0x0) {
          puVar5 = (undefined4 *)0x0;
        }
        else {
          for (puVar5 = (undefined4 *)puVar5[5];
              (puVar5 != (undefined4 *)0x0 && (*(char *)(puVar5 + 4) == '\0'));
              puVar5 = (undefined4 *)puVar5[5]) {
          }
        }
        if (puVar5 == (undefined4 *)0x0) {
          return;
        }
      } while( true );
    default:
      return;
    }
  } while( true );
}


// 10006410 FUN_10006410

undefined4 * __cdecl FUN_10006410(undefined1 *param_1,char *param_2)

{
  char cVar1;
  int iVar2;
  int *piVar3;
  void *this;
  undefined4 *this_00;
  char *pcVar4;
  undefined4 *puVar5;
  long lVar6;
  uint uVar7;
  char *pcVar8;
  int iVar9;
  char *unaff_EBP;
  char *pcVar10;
  byte *pbVar11;
  char cStack_204;
  undefined1 auStack_203 [255];
  byte bStack_104;
  undefined1 auStack_103 [259];
  
  if (param_1 == (undefined1 *)0x0) {
    param_1 = &DAT_1002536c;
  }
  pcVar10 = param_2;
  if (param_2 == (char *)0x0) {
    pcVar10 = "";
  }
  piVar3 = (int *)KeyValuesSystem();
  this = (void *)(**(code **)(*piVar3 + 4))(0x24);
  if ((this == (void *)0x0) || (this_00 = FUN_10005890(this,param_1), this_00 == (undefined4 *)0x0))
  {
    return (undefined4 *)0x0;
  }
  cStack_204 = '\0';
  _memset(auStack_203,0,0xff);
  bStack_104 = 0;
  _memset(auStack_103,0,0xff);
  pcVar4 = FUN_100054f0((undefined4 *)&stack0xfffffdf8);
  cVar1 = *pcVar4;
  do {
    if (cVar1 == '\0') {
LAB_10006634:
      if (param_2 != (char *)0x0) {
        *(char **)param_2 = pcVar10;
      }
      return this_00;
    }
    if (cVar1 == '}') {
      pcVar10 = pcVar4 + 1;
      goto LAB_10006634;
    }
    pcVar10 = unaff_EBP + (1 - (int)pcVar4);
    if ((char *)0x100 < pcVar10) {
      pcVar10 = (char *)0x100;
    }
    FUN_10009c10(&cStack_204,pcVar4,(size_t)pcVar10);
    pcVar4 = &cStack_204;
    if (cStack_204 == '{') {
      pcVar4 = "";
LAB_10006543:
      puVar5 = FUN_10006410(pcVar4,unaff_EBP);
      if (puVar5 != (undefined4 *)0x0) {
        iVar9 = this_00[6];
        if (this_00[6] == 0) {
          this_00[6] = puVar5;
        }
        else {
          do {
            iVar2 = *(int *)(iVar9 + 0x14);
            if (iVar2 == 0) break;
            iVar9 = iVar2;
          } while (iVar2 != 0);
          *(undefined4 **)(iVar9 + 0x14) = puVar5;
        }
      }
    }
    else {
      pcVar10 = FUN_100054f0((undefined4 *)&stack0xfffffdf8);
      if (*pcVar10 == '}') {
        pcVar10 = pcVar10 + 1;
        FUN_10005cb0(this_00,&cStack_204,"");
        goto LAB_10006634;
      }
      pcVar8 = unaff_EBP + (1 - (int)pcVar10);
      if ((char *)0x100 < pcVar8) {
        pcVar8 = (char *)0x100;
      }
      FUN_10009c10((char *)&bStack_104,pcVar10,(size_t)pcVar8);
      if (bStack_104 == 0x7b) goto LAB_10006543;
      pcVar10 = FUN_100099b0((char *)&bStack_104,"#int#");
      if (pcVar10 == (char *)0x0) {
        uVar7 = FUN_100098d0(&bStack_104,(byte *)"#empty#");
        if (uVar7 == 0) {
          pbVar11 = &DAT_1002536c;
        }
        else {
          pbVar11 = &bStack_104;
        }
        FUN_10005cb0(this_00,&cStack_204,(char *)pbVar11);
      }
      else {
        lVar6 = _atol(pcVar10);
        piVar3 = FUN_100058e0(this_00,&cStack_204,1);
        if (piVar3 != (int *)0x0) {
          piVar3[3] = lVar6;
          *(undefined1 *)(piVar3 + 4) = 2;
        }
      }
    }
    pcVar4 = FUN_100054f0((undefined4 *)&stack0xfffffdf8);
    cVar1 = *pcVar4;
    pcVar10 = unaff_EBP;
  } while( true );
}


// 10006650 FUN_10006650

void __thiscall FUN_10006650(void *this,void *param_1,void *param_2,int param_3)

{
  int iVar1;
  undefined1 *puVar2;
  
  if ((0 < param_3) && (param_1 != (void *)0x0)) {
    if (param_2 == (void *)0x0) {
      param_2 = param_1;
    }
                    /* WARNING: Load size is inaccurate */
    if ((*this & 1) == 0) {
      if ((param_2 != (void *)0x0) && (param_1 != param_2)) {
        _memcpy(param_1,param_2,param_3 * 4);
      }
    }
    else if (0 < param_3) {
      puVar2 = (undefined1 *)((int)param_2 + 2);
      iVar1 = param_3;
      do {
        param_3 = CONCAT13(puVar2[-2],CONCAT12(puVar2[-1],CONCAT11(*puVar2,puVar2[1])));
        FUN_10009bb0(param_1,&param_3,4);
        param_1 = (void *)((int)param_1 + 4);
        puVar2 = puVar2 + 4;
        iVar1 = iVar1 + -1;
      } while (iVar1 != 0);
      return;
    }
  }
  return;
}


// 100066f0 FUN_100066f0

void __thiscall FUN_100066f0(void *this,undefined4 param_1)

{
  undefined4 uVar1;
  
  uVar1 = FUN_1000b540(this,4);
  if ((char)uVar1 != '\0') {
    if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
                    /* WARNING: Load size is inaccurate */
      FUN_10006650((void *)((int)this + 0x2c),
                   (void *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this),
                   &param_1,1);
      *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 4;
      FUN_1000b590((int *)this);
      return;
    }
                    /* WARNING: Load size is inaccurate */
    *(undefined4 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 4;
    FUN_1000b590((int *)this);
  }
  return;
}


// 10006750 FUN_10006750

void __thiscall FUN_10006750(void *this,undefined1 *param_1,undefined1 *param_2,uint param_3)

{
  uint uVar1;
  int iVar2;
  
  if ((0 < (int)param_3) && (param_1 != (undefined1 *)0x0)) {
    if (param_2 == (undefined1 *)0x0) {
      param_2 = param_1;
    }
                    /* WARNING: Load size is inaccurate */
    if ((*this & 1) == 0) {
      if ((param_2 != (undefined1 *)0x0) && (param_1 != param_2)) {
        _memcpy(param_1,param_2,param_3 * 2);
      }
    }
    else if (0 < (int)param_3) {
      iVar2 = (int)param_1 - (int)param_2;
      uVar1 = param_3;
      do {
        param_3 = (uint)CONCAT11(*param_2,param_2[1]);
        FUN_10009bb0(param_2 + iVar2,&param_3,2);
        param_2 = param_2 + 2;
        uVar1 = uVar1 - 1;
      } while (uVar1 != 0);
      return;
    }
  }
  return;
}


// 100067e0 FUN_100067e0

void __thiscall FUN_100067e0(void *this,void *param_1,void *param_2,int param_3)

{
  int iVar1;
  
  if ((0 < param_3) && (param_1 != (void *)0x0)) {
    if (param_2 == (void *)0x0) {
      param_2 = param_1;
    }
                    /* WARNING: Load size is inaccurate */
    if ((*this & 1) == 0) {
      if ((param_2 != (void *)0x0) && (param_1 != param_2)) {
        _memcpy(param_1,param_2,param_3 * 8);
      }
    }
    else if (0 < param_3) {
      iVar1 = (int)param_2 - (int)param_1;
      do {
        FUN_10005820(param_1,(undefined1 *)(iVar1 + (int)param_1));
        param_1 = (void *)((int)param_1 + 8);
        param_3 = param_3 + -1;
      } while (param_3 != 0);
      return;
    }
  }
  return;
}


// 10006850 FUN_10006850

void __thiscall FUN_10006850(void *this,void *param_1,void *param_2,int param_3)

{
  int iVar1;
  undefined1 *puVar2;
  
  if ((0 < param_3) && (param_1 != (void *)0x0)) {
    if (param_2 == (void *)0x0) {
      param_2 = param_1;
    }
                    /* WARNING: Load size is inaccurate */
    if ((*this & 1) == 0) {
      if ((param_2 != (void *)0x0) && (param_1 != param_2)) {
        _memcpy(param_1,param_2,param_3 * 4);
      }
    }
    else if (0 < param_3) {
      puVar2 = (undefined1 *)((int)param_2 + 2);
      iVar1 = param_3;
      do {
        param_3 = CONCAT13(puVar2[-2],CONCAT12(puVar2[-1],CONCAT11(*puVar2,puVar2[1])));
        FUN_10009bb0(param_1,&param_3,4);
        param_1 = (void *)((int)param_1 + 4);
        puVar2 = puVar2 + 4;
        iVar1 = iVar1 + -1;
      } while (iVar1 != 0);
      return;
    }
  }
  return;
}


// 100068f0 FUN_100068f0

undefined4 * __thiscall FUN_100068f0(void *this,char *param_1)

{
  int *piVar1;
  undefined4 uVar2;
  char *unaff_retaddr;
  
  *(undefined4 *)this = 0xffffffff;
  *(undefined1 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x18) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 0x1c) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined4 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined1 *)((int)this + 0x11) = 0;
  *(undefined4 *)((int)this + 0x20) = 0;
  *(undefined2 *)((int)this + 0x12) = 0;
  piVar1 = (int *)KeyValuesSystem();
  uVar2 = (**(code **)(*piVar1 + 0xc))(param_1,1);
  *(undefined4 *)this = uVar2;
  FUN_10005cb0(this,unaff_retaddr,param_1);
  return (undefined4 *)this;
}


// 10006950 FUN_10006950

int * __fastcall FUN_10006950(int param_1)

{
  int iVar1;
  int *piVar2;
  void *this;
  int *piVar3;
  int iVar4;
  undefined4 unaff_retaddr;
  
  piVar2 = (int *)KeyValuesSystem();
  this = (void *)(**(code **)(*piVar2 + 4))(0x24);
  if (this != (void *)0x0) {
    piVar2 = FUN_10005890(this,unaff_retaddr);
    if (piVar2 != (int *)0x0) {
      if (*piVar2 != -1) {
        *(bool *)((int)piVar2 + 0x11) = *(char *)(param_1 + 0x11) != '\0';
        iVar4 = *(int *)(param_1 + 0x18);
        if (*(int *)(param_1 + 0x18) == 0) {
          *(int **)(param_1 + 0x18) = piVar2;
          return piVar2;
        }
        do {
          iVar1 = *(int *)(iVar4 + 0x14);
          if (iVar1 == 0) break;
          iVar4 = iVar1;
        } while (iVar1 != 0);
        *(int **)(iVar4 + 0x14) = piVar2;
        return piVar2;
      }
      FUN_10004ff0((int)piVar2);
      piVar3 = (int *)KeyValuesSystem();
      (**(code **)(*piVar3 + 8))(piVar2);
    }
  }
  return (int *)0x0;
}


// 100069e0 FUN_100069e0

int __thiscall FUN_100069e0(void *this,char *param_1,int param_2)

{
  int *piVar1;
  int iVar2;
  char local_240 [64];
  CHAR local_200 [512];
  
  piVar1 = FUN_100058e0(this,param_1,0);
  if (piVar1 != (int *)0x0) {
    switch((char)piVar1[4]) {
    case '\x01':
switchD_10006a14_caseD_1:
      return piVar1[1];
    case '\x02':
    case '\x04':
      FUN_10009c50(local_240,0x40,"%d");
      FUN_10005cb0(this,param_1,local_240);
      return piVar1[1];
    case '\x03':
      FUN_10009c50(local_240,0x40,"%f");
      FUN_10005cb0(this,param_1,local_240);
      return piVar1[1];
    case '\x05':
      iVar2 = FUN_10009db0((LPCWSTR)piVar1[2],local_200,0x200);
      if (iVar2 != 0) {
        FUN_10005cb0(this,param_1,local_200);
        goto switchD_10006a14_caseD_1;
      }
      break;
    case '\a':
      FUN_10009c50(local_240,0x40,"%lld");
      FUN_10005cb0(this,param_1,local_240);
      return piVar1[1];
    }
  }
  return param_2;
}


// 10006b30 FUN_10006b30

int __thiscall FUN_10006b30(void *this,char *param_1,int param_2)

{
  longlong lVar1;
  int *piVar2;
  int iVar3;
  LPWSTR pWVar4;
  undefined4 extraout_ECX;
  wchar_t local_80 [64];
  
  piVar2 = FUN_100058e0(this,param_1,0);
  if (piVar2 == (int *)0x0) {
    return param_2;
  }
  switch((char)piVar2[4]) {
  case '\x01':
    iVar3 = FUN_10009820((char *)piVar2[1]);
    lVar1 = (ulonglong)(iVar3 + 1U) * 2;
    pWVar4 = (LPWSTR)FUN_10001270(-(uint)((int)((ulonglong)lVar1 >> 0x20) != 0) | (uint)lVar1);
    iVar3 = FUN_10009d50((LPCSTR)piVar2[1],pWVar4,(iVar3 + 1U) * 2);
    if (iVar3 < 0) {
      FUN_100012d0(pWVar4);
      return param_2;
    }
    FUN_10005d20(this,param_1,pWVar4);
    FUN_100012d0(pWVar4);
switchD_10006b65_caseD_5:
    return piVar2[2];
  case '\x02':
  case '\x04':
    FUN_10004ef0(extraout_ECX,0x40,local_80,L"%d");
    FUN_10005d20(this,param_1,local_80);
    return piVar2[2];
  case '\x03':
    FUN_10004ef0(extraout_ECX,0x40,local_80,L"%f");
    FUN_10005d20(this,param_1,local_80);
    return piVar2[2];
  case '\x05':
    goto switchD_10006b65_caseD_5;
  default:
    return param_2;
  case '\a':
    FUN_10004ef0(local_80,0x40,local_80,L"%lld");
    FUN_10005d20(this,param_1,local_80);
    return piVar2[2];
  }
}


// 10006ce0 FUN_10006ce0

void __thiscall FUN_10006ce0(void *this,int *param_1)

{
  int iVar1;
  int iVar2;
  void *pvVar3;
  void *pvVar4;
  int iVar5;
  
  iVar2 = param_1[3];
  iVar5 = 0;
  if (0 < iVar2) {
    do {
      iVar1 = iVar5 * 4;
      pvVar3 = this;
      do {
        pvVar4 = pvVar3;
        if (pvVar4 == (void *)0x0) break;
        pvVar3 = *(void **)((int)pvVar4 + 0x14);
      } while (*(void **)((int)pvVar4 + 0x14) != (void *)0x0);
      iVar5 = iVar5 + 1;
      *(undefined4 *)((int)pvVar4 + 0x14) = *(undefined4 *)(*param_1 + iVar1);
    } while (iVar5 < iVar2);
  }
  return;
}


// 10006d20 FUN_10006d20

void __thiscall FUN_10006d20(void *this,undefined4 param_1,int *param_2)

{
  byte bVar1;
  int iVar2;
  char *pcVar3;
  int *this_00;
  byte *_Str;
  void *pvVar4;
  longlong *plVar5;
  undefined4 uVar6;
  float10 fVar7;
  longlong lVar8;
  char local_27;
  char local_26;
  char local_25;
  byte *local_24;
  byte *local_20;
  void *local_1c;
  int local_18;
  int local_14;
  int *local_10;
  int local_c;
  byte *local_8;
  byte *local_4;
  
  if (this == (void *)0x0) {
    uVar6 = 0xffffffff;
  }
  else {
                    /* WARNING: Load size is inaccurate */
    uVar6 = *this;
  }
  if (DAT_100350ec < 0x40) {
    *(undefined4 *)(&DAT_10034fe8 + DAT_100350ec * 4) = uVar6;
  }
  iVar2 = DAT_100350ec + 1;
  if (DAT_100350f0 <= DAT_100350ec) {
    DAT_100350f0 = DAT_100350ec;
  }
  local_1c = this;
  if (100 < DAT_100350ec) {
    DAT_100350ec = iVar2;
    FUN_10004f10(&DAT_10034fe8,"RecursiveLoadFromBuffer:  recursion overflow");
    DAT_100350ec = DAT_100350ec + -1;
    return;
  }
  if (iVar2 < 0x40) {
    DAT_100350ec = iVar2;
    *(undefined4 *)(&DAT_10034fe8 + iVar2 * 4) = 0xffffffff;
    iVar2 = DAT_100350ec;
  }
  DAT_100350ec = iVar2 + 1;
  if (DAT_100350f0 <= iVar2) {
    DAT_100350f0 = iVar2;
  }
  local_25 = '\x01';
  local_18 = iVar2;
  pcVar3 = FUN_100050b0(this,param_2,&local_26,&local_27);
  do {
    if (pcVar3 == (char *)0x0) {
      pcVar3 = "RecursiveLoadFromBuffer:  got EOF instead of keyname";
LAB_10007120:
      FUN_10004f10(&DAT_10034fe8,pcVar3);
LAB_1000712a:
      DAT_100350ec = DAT_100350ec + -2;
      return;
    }
    if (*pcVar3 == '\0') {
      pcVar3 = "RecursiveLoadFromBuffer:  got empty keyname";
      goto LAB_10007120;
    }
    if ((*pcVar3 == '}') && (local_26 == '\0')) goto LAB_1000712a;
    this_00 = FUN_10006950((int)this);
    local_10 = this_00;
    if (this_00 == (int *)0x0) {
LAB_1000710d:
      pcVar3 = "RecursiveLoadFromBuffer:  got NULL key";
      goto LAB_10007120;
    }
    if (iVar2 < 0x40) {
      *(int *)(&DAT_10034fe8 + iVar2 * 4) = *this_00;
    }
    _Str = FUN_100050b0(this,param_2,&local_26,&local_27);
    if (local_27 != '\0') {
      local_20 = _Str;
      if (_Str == (byte *)0x0) goto LAB_1000710d;
      local_25 = FUN_10005450();
      _Str = FUN_100050b0(this,param_2,&local_26,&local_27);
    }
    local_20 = _Str;
    if (_Str == (byte *)0x0) goto LAB_1000710d;
    if ((*_Str == 0x7d) && (local_26 == '\0')) {
      pcVar3 = "RecursiveLoadFromBuffer:  got } in key";
      goto LAB_10007120;
    }
    if ((*_Str == 0x7b) && (local_26 == '\0')) {
      if (local_18 < 0x40) {
        *(undefined4 *)(&DAT_10034fe8 + local_18 * 4) = 0xffffffff;
      }
      FUN_10006d20(this_00,param_1,param_2);
    }
    else {
      if (local_27 != '\0') {
        pcVar3 = "RecursiveLoadFromBuffer:  got conditional between key and value";
        goto LAB_10007120;
      }
      if (this_00[1] != 0) {
        FUN_100012d0(this_00[1]);
        this_00[1] = 0;
      }
      iVar2 = FUN_10009820((char *)_Str);
      local_c = iVar2;
      local_14 = _strtol((char *)_Str,(char **)&local_4,10);
      fVar7 = (float10)FUN_10010409(_Str,&local_8);
      if (*_Str == 0) {
LAB_10006f0a:
        *(undefined1 *)(this_00 + 4) = 1;
      }
      else if (((iVar2 == 0x12) && (*_Str == 0x30)) && (_Str[1] == 0x78)) {
        lVar8 = 0;
        local_24 = _Str + 3;
        local_14 = 4;
        do {
          bVar1 = local_24[-1];
          if ((char)bVar1 < 'a') {
            if ('@' < (char)bVar1) {
              bVar1 = bVar1 - 7;
            }
          }
          else {
            bVar1 = bVar1 - 0x27;
          }
          lVar8 = __allmul((uint)lVar8,(int)((ulonglong)lVar8 >> 0x20),0x10,0);
          lVar8 = lVar8 + ((char)bVar1 + -0x30);
          bVar1 = *local_24;
          if ((char)bVar1 < 'a') {
            if ('@' < (char)bVar1) {
              bVar1 = bVar1 - 7;
            }
          }
          else {
            bVar1 = bVar1 - 0x27;
          }
          lVar8 = __allmul((uint)lVar8,(int)((ulonglong)lVar8 >> 0x20),0x10,0);
          lVar8 = lVar8 + ((char)bVar1 + -0x30);
          bVar1 = local_24[1];
          if ((char)bVar1 < 'a') {
            if ('@' < (char)bVar1) {
              bVar1 = bVar1 - 7;
            }
          }
          else {
            bVar1 = bVar1 - 0x27;
          }
          lVar8 = __allmul((uint)lVar8,(int)((ulonglong)lVar8 >> 0x20),0x10,0);
          lVar8 = lVar8 + ((char)bVar1 + -0x30);
          bVar1 = local_24[2];
          if ((char)bVar1 < 'a') {
            if ('@' < (char)bVar1) {
              bVar1 = bVar1 - 7;
            }
          }
          else {
            bVar1 = bVar1 - 0x27;
          }
          lVar8 = __allmul((uint)lVar8,(int)((ulonglong)lVar8 >> 0x20),0x10,0);
          lVar8 = lVar8 + ((char)bVar1 + -0x30);
          local_24 = local_24 + 4;
          local_14 = local_14 + -1;
        } while (local_14 != 0);
        plVar5 = (longlong *)FUN_10001270(8);
        local_10[1] = (int)plVar5;
        *plVar5 = lVar8;
        *(undefined1 *)(local_10 + 4) = 7;
        _Str = local_20;
        iVar2 = local_c;
        this_00 = local_10;
      }
      else if ((local_4 < local_8) && (local_8 == _Str + iVar2)) {
        this_00[3] = (int)(float)fVar7;
        *(undefined1 *)(this_00 + 4) = 3;
      }
      else {
        if (local_4 != _Str + iVar2) goto LAB_10006f0a;
        this_00[3] = local_14;
        *(undefined1 *)(this_00 + 4) = 2;
      }
      if ((char)this_00[4] == '\x01') {
        pvVar4 = (void *)FUN_10001270(iVar2 + 1U);
        this_00[1] = (int)pvVar4;
        FUN_10009bb0(pvVar4,_Str,iVar2 + 1U);
      }
      iVar2 = param_2[3];
      FUN_100050b0(local_1c,param_2,&local_26,&local_27);
      if (local_27 == '\0') {
        FUN_1000b4e0(param_2,0,iVar2);
        this = local_1c;
      }
      else {
        local_25 = FUN_10005450();
        this = local_1c;
      }
    }
    if (local_25 == '\0') {
      FUN_10005240(this,(int)this_00);
      FUN_10005ff0((int)this_00);
    }
    local_25 = '\x01';
    pcVar3 = FUN_100050b0(this,param_2,&local_26,&local_27);
    iVar2 = local_18;
  } while( true );
}


// 10007140 FUN_10007140

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */

uint __thiscall FUN_10007140(void *this,void *param_1)

{
  char cVar1;
  uint uVar2;
  int *piVar3;
  short *psVar4;
  int iVar5;
  code *pcVar6;
  char acStack_a0 [112];
  undefined4 uStack_30;
  
                    /* WARNING: Load size is inaccurate */
  uVar2 = (**(code **)(*this + 0xc))();
  if (param_1 == (void *)0x0) {
                    /* WARNING: Load size is inaccurate */
    if (((char)uVar2 != '\0') && (cVar1 = (**(code **)(*this + 0x10))(), cVar1 != '\0')) {
      return 1;
    }
    return 0;
  }
  if ((char)uVar2 == '\0') {
LAB_100071f8:
    return uVar2 & 0xffffff00;
  }
  piVar3 = (int *)KeyValuesSystem();
  (**(code **)(*piVar3 + 0x10))();
                    /* WARNING: Load size is inaccurate */
  uVar2 = (**(code **)(*this + 0x10))();
                    /* WARNING: Load size is inaccurate */
  if (((char)uVar2 == '\0') || (uVar2 = (**(code **)(*this + 0x10))(), (char)uVar2 == '\0'))
  goto LAB_100071f8;
  switch(*(undefined1 *)((int)param_1 + 0x10)) {
  case 1:
                    /* WARNING: Load size is inaccurate */
    iVar5 = *this;
    FUN_100069e0(param_1,(char *)0x0,0x1002536c);
    uVar2 = (**(code **)(iVar5 + 0x10))();
    if ((char)uVar2 == '\0') goto LAB_100071f8;
    goto switchD_100071d1_caseD_6;
  case 2:
    FUN_10005a70(param_1,(char *)0x0,0);
    FUN_10009c50(acStack_a0,0x80,"int( %d = 0x%X )");
    break;
  case 3:
    FUN_10005be0(param_1,(char *)0x0,0.0);
    FUN_10009c50(acStack_a0,0x80,"float( %f )");
    break;
  case 4:
    FUN_10005bb0(param_1,(char *)0x0,0);
    FUN_10009c50(acStack_a0,0x80,"ptr( 0x%p )");
    break;
  case 5:
    psVar4 = (short *)FUN_10006b30(param_1,(char *)0x0,0x10027b6c);
    iVar5 = FUN_10009850(psVar4);
    FUN_10009c50(acStack_a0,iVar5 * 2 + 0x40,"%ls [wstring, len = %d]");
                    /* WARNING: Load size is inaccurate */
    pcVar6 = *(code **)(*this + 0x10);
    goto LAB_10007238;
  default:
    goto switchD_100071d1_caseD_6;
  case 7:
    uStack_30 = 0x1000731f;
    FUN_10005af0(param_1,(char *)0x0,0,0);
    FUN_10009c50(acStack_a0,0x80,"u64( %lld = 0x%llX )");
  }
                    /* WARNING: Load size is inaccurate */
  pcVar6 = *(code **)(*this + 0x10);
LAB_10007238:
  uVar2 = (*pcVar6)();
  if ((char)uVar2 == '\0') {
    return uVar2;
  }
switchD_100071d1_caseD_6:
                    /* WARNING: Load size is inaccurate */
  uVar2 = (**(code **)(*this + 0x10))();
  return uVar2;
}


// 10007390 FUN_10007390

void __thiscall FUN_10007390(void *this,undefined4 *param_1,char *param_2)

{
  char cVar1;
  
  if ((*(byte *)((int)this + 0x15) & 1) != 0) {
    *param_1 = 0;
    FUN_1000c530(this,param_2);
    return;
  }
  cVar1 = FUN_1000b3d0(this,4);
  if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    *param_1 = *(undefined4 *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this);
    if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
      FUN_10006650((byte *)((int)this + 0x2c),param_1,param_1,1);
    }
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 4;
    return;
  }
  *param_1 = 0;
  return;
}


// 10007400 FUN_10007400

void __thiscall FUN_10007400(void *this,undefined2 *param_1)

{
  char cVar1;
  int iVar2;
  
  cVar1 = FUN_1000b3d0(this,2);
  if (cVar1 == '\0') {
    *param_1 = 0;
    return;
  }
  iVar2 = *(int *)((int)this + 0xc) - *(int *)((int)this + 0x20);
  if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
                    /* WARNING: Load size is inaccurate */
    FUN_10006750((void *)((int)this + 0x2c),(undefined1 *)param_1,(undefined1 *)(iVar2 + *this),1);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 2;
    return;
  }
                    /* WARNING: Load size is inaccurate */
  *param_1 = *(undefined2 *)(iVar2 + *this);
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 2;
  return;
}


// 10007460 FUN_10007460

void __thiscall FUN_10007460(void *this,undefined4 *param_1)

{
  int iVar1;
  char cVar2;
  int iVar3;
  
  cVar2 = FUN_1000b3d0(this,8);
  if (cVar2 == '\0') {
    *param_1 = 0;
    param_1[1] = 0;
    return;
  }
  iVar3 = *(int *)((int)this + 0xc) - *(int *)((int)this + 0x20);
  if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
                    /* WARNING: Load size is inaccurate */
    FUN_100067e0((void *)((int)this + 0x2c),param_1,(void *)(iVar3 + *this),1);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 8;
    return;
  }
                    /* WARNING: Load size is inaccurate */
  iVar1 = *this;
  *param_1 = *(undefined4 *)(iVar3 + iVar1);
  param_1[1] = *(undefined4 *)(iVar3 + 4 + iVar1);
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 8;
  return;
}


// 100074d0 FUN_100074d0

void __thiscall FUN_100074d0(void *this,undefined4 *param_1)

{
  char cVar1;
  int iVar2;
  
  cVar1 = FUN_1000b3d0(this,4);
  if (cVar1 == '\0') {
    *param_1 = 0;
    return;
  }
  iVar2 = *(int *)((int)this + 0xc) - *(int *)((int)this + 0x20);
  if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
                    /* WARNING: Load size is inaccurate */
    FUN_10006850((void *)((int)this + 0x2c),param_1,(void *)(iVar2 + *this),1);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 4;
    return;
  }
                    /* WARNING: Load size is inaccurate */
  *param_1 = *(undefined4 *)(iVar2 + *this);
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 4;
  return;
}


// 10007530 FUN_10007530

void __thiscall FUN_10007530(void *this,undefined2 param_1)

{
  undefined4 uVar1;
  
  uVar1 = FUN_1000b540(this,2);
  if ((char)uVar1 != '\0') {
    if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
                    /* WARNING: Load size is inaccurate */
      FUN_10006750((void *)((int)this + 0x2c),
                   (undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this)
                   ,(undefined1 *)&param_1,1);
      *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 2;
      FUN_1000b590((int *)this);
      return;
    }
                    /* WARNING: Load size is inaccurate */
    *(undefined2 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 2;
    FUN_1000b590((int *)this);
  }
  return;
}


// 10007590 FUN_10007590

void __thiscall FUN_10007590(void *this,undefined4 param_1)

{
  undefined4 uVar1;
  
  uVar1 = FUN_1000b540(this,4);
  if ((char)uVar1 != '\0') {
    if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
                    /* WARNING: Load size is inaccurate */
      FUN_10006850((void *)((int)this + 0x2c),
                   (void *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this),
                   &param_1,1);
      *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 4;
      FUN_1000b590((int *)this);
      return;
    }
                    /* WARNING: Load size is inaccurate */
    *(undefined4 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 4;
    FUN_1000b590((int *)this);
  }
  return;
}


// 100075f0 FUN_100075f0

void __thiscall FUN_100075f0(void *this,undefined4 param_1,undefined4 param_2)

{
  int iVar1;
  undefined4 uVar2;
  int iVar3;
  undefined4 *puVar4;
  
  uVar2 = FUN_1000b540(this,8);
  if ((char)uVar2 != '\0') {
    iVar3 = *(int *)((int)this + 0x10) - *(int *)((int)this + 0x20);
    if ((*(byte *)((int)this + 0x2c) & 1) == 0) {
                    /* WARNING: Load size is inaccurate */
      iVar1 = *this;
      *(undefined4 *)(iVar3 + iVar1) = param_1;
      *(undefined4 *)(iVar3 + 4 + iVar1) = param_2;
    }
    else {
                    /* WARNING: Load size is inaccurate */
      puVar4 = (undefined4 *)(iVar3 + *this);
      if (puVar4 != (undefined4 *)0x0) {
        if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
          FUN_10005820(puVar4,(undefined1 *)&param_1);
          *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 8;
          FUN_1000b590((int *)this);
          return;
        }
        if (puVar4 != &param_1) {
          *puVar4 = param_1;
          puVar4[1] = param_2;
          *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 8;
          FUN_1000b590((int *)this);
          return;
        }
      }
    }
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 8;
    FUN_1000b590((int *)this);
  }
  return;
}


// 10007680 FUN_10007680

float10 __fastcall FUN_10007680(void *param_1)

{
  void *local_4;
  
  local_4 = param_1;
  FUN_10007390(param_1,&local_4,"%f");
  return (float10)(float)local_4;
}


// 100076a0 FUN_100076a0

void __thiscall FUN_100076a0(void *this,undefined4 param_1)

{
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    FUN_100066f0(this,param_1);
    return;
  }
  FUN_1000bee0(this,"%f");
  return;
}


// 100076e0 FUN_100076e0

void __fastcall FUN_100076e0(int *param_1)

{
  if (-1 < param_1[2]) {
    if (*param_1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
      *param_1 = 0;
    }
    param_1[1] = 0;
  }
  return;
}


// 10007710 FUN_10007710

undefined1 __fastcall FUN_10007710(int *param_1)

{
  undefined1 uVar1;
  char cVar2;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) != 0) {
    FUN_1000c530(param_1,"%c");
    return 0;
  }
  cVar2 = FUN_1000b3d0(param_1,1);
  if (cVar2 != '\0') {
    uVar1 = *(undefined1 *)((param_1[3] - param_1[8]) + *param_1);
    param_1[3] = param_1[3] + 1;
    return uVar1;
  }
  return 0;
}


// 10007760 FUN_10007760

undefined1 __fastcall FUN_10007760(int *param_1)

{
  undefined1 uVar1;
  char cVar2;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) != 0) {
    FUN_1000c530(param_1,"%u");
    return 0;
  }
  cVar2 = FUN_1000b3d0(param_1,1);
  if (cVar2 != '\0') {
    uVar1 = *(undefined1 *)((param_1[3] - param_1[8]) + *param_1);
    param_1[3] = param_1[3] + 1;
    return uVar1;
  }
  return 0;
}


// 100077b0 FUN_100077b0

uint __fastcall FUN_100077b0(void *param_1)

{
  undefined2 extraout_var;
  undefined2 extraout_var_00;
  void *local_4;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) == 0) {
    local_4 = param_1;
    FUN_10007400(param_1,(undefined2 *)&local_4);
    return CONCAT22(extraout_var,local_4._0_2_);
  }
  local_4 = (void *)0x0;
  FUN_1000c530(param_1,"%d");
  return CONCAT22(extraout_var_00,local_4._0_2_);
}


// 100077f0 FUN_100077f0

void * __fastcall FUN_100077f0(void *param_1)

{
  void *local_4;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) == 0) {
    local_4 = param_1;
    FUN_100074d0(param_1,&local_4);
    return local_4;
  }
  local_4 = (void *)0x0;
  FUN_1000c530(param_1,"%d");
  return local_4;
}


// 10007830 FUN_10007830

undefined8 __fastcall FUN_10007830(void *param_1)

{
  undefined4 local_8;
  undefined4 local_4;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) == 0) {
    FUN_10007460(param_1,&local_8);
    return CONCAT44(local_4,local_8);
  }
  local_8 = 0;
  local_4 = 0;
  FUN_1000c530(param_1,"%lld");
  return CONCAT44(local_4,local_8);
}


// 10007880 FUN_10007880

void * __fastcall FUN_10007880(void *param_1)

{
  void *local_4;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) == 0) {
    local_4 = param_1;
    FUN_100074d0(param_1,&local_4);
    return local_4;
  }
  local_4 = (void *)0x0;
  FUN_1000c530(param_1,"%u");
  return local_4;
}


// 100078c0 FUN_100078c0

void __thiscall FUN_100078c0(void *this,undefined1 param_1)

{
  undefined4 uVar1;
  
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    uVar1 = FUN_1000b540(this,1);
    if ((char)uVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
      *(undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
      *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
      FUN_1000b590((int *)this);
      return;
    }
  }
  else {
    FUN_1000bee0(this,"%u");
  }
  return;
}


// 10007910 FUN_10007910

void __thiscall FUN_10007910(void *this,undefined2 param_1)

{
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    FUN_10007530(this,param_1);
    return;
  }
  FUN_1000bee0(this,"%d");
  return;
}


// 10007940 FUN_10007940

void __thiscall FUN_10007940(void *this,undefined4 param_1)

{
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    FUN_10007590(this,param_1);
    return;
  }
  FUN_1000bee0(this,"%d");
  return;
}


// 10007970 FUN_10007970

void __thiscall FUN_10007970(void *this,undefined4 param_1,undefined4 param_2)

{
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    FUN_100075f0(this,param_1,param_2);
    return;
  }
  FUN_1000bee0(this,"%lld");
  return;
}


// 100079a0 FUN_100079a0

void __thiscall FUN_100079a0(void *this,undefined4 param_1)

{
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    FUN_10007590(this,param_1);
    return;
  }
  FUN_1000bee0(this,"%u");
  return;
}


// 100079d0 FUN_100079d0

undefined1 __thiscall FUN_100079d0(void *this,int *param_1,undefined *param_2)

{
  int *piVar1;
  char cVar2;
  void *this_00;
  int iVar3;
  uint uVar4;
  char *pcVar5;
  char unaff_BL;
  undefined1 unaff_SI;
  uint auStack_70 [2];
  void *pvStack_68;
  undefined1 *puStack_64;
  undefined4 uStack_60;
  undefined4 uStack_5c;
  void *pvStack_58;
  char *pcStack_38;
  undefined4 uStack_20;
  int iStack_1c;
  undefined1 auStack_c [12];
  
  this_00 = (void *)(**(code **)(*param_1 + 8))();
  if (this_00 != (void *)0x0) {
    PTR_s_unknown_10034fd0 = param_2;
    uStack_5c = 0x10007a18;
    pvStack_58 = this_00;
    iVar3 = (**(code **)(*param_1 + 0x1c))();
    uStack_5c = 0;
    uStack_60 = 0;
    piVar1 = param_1 + -1;
    puStack_64 = auStack_c;
    uVar4 = iVar3 + 2;
    auStack_70[1] = 0x10007a39;
    pvStack_68 = this_00;
    cVar2 = (**(code **)(*piVar1 + 0x150))();
    if (cVar2 != '\0') {
      uVar4 = iStack_1c + -1 + uVar4 & ~(iStack_1c - 1U);
    }
    auStack_70[1] = 0;
    auStack_70[0] = uVar4;
    pcVar5 = (char *)(**(code **)(*piVar1 + 0x154))();
    iVar3 = (**(code **)(*piVar1 + 0x118))(pcVar5,uStack_20,puStack_64);
    pcStack_38 = (char *)CONCAT31(pcStack_38._1_3_,iVar3 != 0);
    (**(code **)(*param_1 + 0xc))(this_00);
    if (unaff_BL != '\0') {
      pcVar5[(int)this_00] = '\0';
      (pcVar5 + 1)[(int)this_00] = '\0';
      FUN_1000bc50(auStack_70,0,0,0);
      iVar3 = FUN_10009820(pcVar5);
      FUN_1000bd50(auStack_70,(int)pcVar5,(int)this,iVar3,9);
      FUN_100080e0(this_00,pcStack_38,(int *)auStack_70,(int)param_1,(void *)0x0);
      if ((-1 < (int)pvStack_68) && (auStack_70[0] != 0)) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(auStack_70[0]);
      }
    }
    (**(code **)(*piVar1 + 0x158))(pcVar5);
    return unaff_SI;
  }
  return 0;
}


// 10007b30 FUN_10007b30

uint __thiscall FUN_10007b30(void *this,int *param_1)

{
  undefined1 uVar1;
  uint in_EAX;
  undefined4 uVar2;
  int *piVar3;
  char *pcVar4;
  int iVar5;
  int iVar6;
  
  if (((*(byte *)((int)param_1 + 0x15) & 1) == 0) && ((char)param_1[5] == '\0')) {
    if (this != (void *)0x0) {
      do {
        uVar1 = *(undefined1 *)((int)this + 0x10);
        if ((*(byte *)((int)param_1 + 0x15) & 1) == 0) {
          uVar2 = FUN_1000b540(param_1,1);
          if ((char)uVar2 != '\0') {
            *(undefined1 *)((*param_1 - param_1[8]) + param_1[4]) = uVar1;
            param_1[4] = param_1[4] + 1;
            FUN_1000b590(param_1);
          }
        }
        else {
          FUN_1000bee0(param_1,"%u");
        }
        piVar3 = (int *)KeyValuesSystem();
                    /* WARNING: Load size is inaccurate */
        pcVar4 = (char *)(**(code **)(*piVar3 + 0x10))(*this);
        FUN_1000bdf0(param_1,pcVar4);
        switch(*(undefined1 *)((int)this + 0x10)) {
        case 0:
          FUN_10007b30(*(void **)((int)this + 0x18),param_1);
          break;
        case 1:
          pcVar4 = *(char **)((int)this + 4);
          if ((pcVar4 == (char *)0x0) || (*pcVar4 == '\0')) {
            FUN_1000bdf0(param_1,"");
          }
          else {
            FUN_1000bdf0(param_1,pcVar4);
          }
          break;
        case 2:
          FUN_10007940(param_1,*(undefined4 *)((int)this + 0xc));
          break;
        case 3:
          FUN_100076a0(param_1,*(undefined4 *)((int)this + 0xc));
          break;
        case 4:
          FUN_100079a0(param_1,*(undefined4 *)((int)this + 0xc));
          break;
        case 5:
          if (*(short **)((int)this + 8) == (short *)0x0) {
            iVar5 = 0;
          }
          else {
            iVar5 = FUN_10009850(*(short **)((int)this + 8));
          }
          FUN_10007910(param_1,(short)iVar5);
          iVar6 = 0;
          if (0 < iVar5) {
            do {
              if ((*(byte *)((int)param_1 + 0x15) & 1) == 0) {
                FUN_10007530(param_1,*(undefined2 *)(*(int *)((int)this + 8) + iVar6 * 2));
              }
              else {
                FUN_1000bee0(param_1,"%d");
              }
              iVar6 = iVar6 + 1;
            } while (iVar6 < iVar5);
          }
          break;
        case 6:
          FUN_100078c0(param_1,*(undefined1 *)((int)this + 0xc));
          FUN_100078c0(param_1,*(undefined1 *)((int)this + 0xd));
          FUN_100078c0(param_1,*(undefined1 *)((int)this + 0xe));
          FUN_100078c0(param_1,*(undefined1 *)((int)this + 0xf));
          break;
        case 7:
          FUN_10007970(param_1,**(undefined4 **)((int)this + 4),(*(undefined4 **)((int)this + 4))[1]
                      );
        }
        this = *(void **)((int)this + 0x14);
      } while ((undefined4 *)this != (undefined4 *)0x0);
    }
    if ((*(byte *)((int)param_1 + 0x15) & 1) == 0) {
      uVar2 = FUN_1000b540(param_1,1);
      if ((char)uVar2 != '\0') {
        *(undefined1 *)((*param_1 - param_1[8]) + param_1[4]) = 0xb;
        param_1[4] = param_1[4] + 1;
        FUN_1000b590(param_1);
        return (uint)((char)param_1[5] == '\0');
      }
    }
    else {
      FUN_1000bee0(param_1,"%u");
    }
    return (uint)((char)param_1[5] == '\0');
  }
  return in_EAX & 0xffffff00;
}


// 10007d50 FUN_10007d50

uint __thiscall FUN_10007d50(void *this,int *param_1,int param_2)

{
  longlong lVar1;
  char cVar2;
  undefined1 uVar3;
  uint in_EAX;
  int *piVar4;
  void *pvVar5;
  undefined4 *this_00;
  uint uVar6;
  void *extraout_EAX;
  undefined3 extraout_var;
  uint *puVar7;
  int iVar8;
  int iVar9;
  float10 fVar10;
  undefined8 uVar11;
  int unaff_retaddr;
  undefined1 local_400 [1015];
  undefined1 uStack_9;
  undefined1 local_1;
  
  if (((char)param_1[5] != '\0') || (100 < param_2)) {
    return in_EAX & 0xffffff00;
  }
  cVar2 = FUN_10007760(param_1);
  if (cVar2 != '\v') {
LAB_10007d92:
    *(char *)((int)this + 0x10) = cVar2;
    FUN_1000bf90(param_1,local_400,0x3ff);
    local_1 = 0;
    piVar4 = (int *)KeyValuesSystem();
    pvVar5 = (void *)(**(code **)(*piVar4 + 0xc))(local_400,1);
    *(void **)this = pvVar5;
    switch(cVar2) {
    case '\0':
      pvVar5 = (void *)FUN_100054d0(0x24);
      if (pvVar5 == (void *)0x0) {
        this_00 = (undefined4 *)0x0;
      }
      else {
        this_00 = FUN_10005890(pvVar5,&DAT_1002536c);
      }
      *(undefined4 **)((int)this + 0x18) = this_00;
      pvVar5 = (void *)FUN_10007d50(this_00,param_1,unaff_retaddr + 1);
      if ((char)pvVar5 == '\0') goto LAB_10007e12;
      break;
    case '\x01':
      FUN_1000bf90(param_1,&stack0xfffffbf8,0x3ff);
      uStack_9 = 0;
      iVar8 = FUN_10009820(&stack0xfffffbf8);
      pvVar5 = (void *)FUN_10001270(iVar8 + 1U);
      *(void **)((int)this + 4) = pvVar5;
      pvVar5 = (void *)FUN_10009bb0(pvVar5,&stack0xfffffbf8,iVar8 + 1U);
      break;
    case '\x02':
      pvVar5 = FUN_100077f0(param_1);
      goto LAB_10007f30;
    case '\x03':
      fVar10 = FUN_10007680(param_1);
      *(float *)((int)this + 0xc) = (float)fVar10;
      pvVar5 = extraout_EAX;
      break;
    case '\x04':
      pvVar5 = FUN_10007880(param_1);
LAB_10007f30:
      *(void **)((int)this + 0xc) = pvVar5;
      break;
    case '\x05':
      pvVar5 = (void *)FUN_100077b0(param_1);
      iVar8 = (int)(short)pvVar5;
      if ((iVar8 < 0) || (pvVar5 = (void *)(iVar8 * 2), (void *)(param_1[7] - param_1[3]) < pvVar5))
      goto LAB_10007e12;
      lVar1 = (ulonglong)(iVar8 + 1) * 2;
      uVar6 = FUN_10001270(-(uint)((int)((ulonglong)lVar1 >> 0x20) != 0) | (uint)lVar1);
      iVar9 = 0;
      *(uint *)((int)this + 8) = uVar6;
      if (0 < iVar8) {
        do {
          uVar6 = FUN_100077b0(param_1);
          *(short *)(*(uint *)((int)this + 8) + iVar9 * 2) = (short)uVar6;
          iVar9 = iVar9 + 1;
        } while (iVar9 < iVar8);
      }
      *(undefined2 *)((int)pvVar5 + *(uint *)((int)this + 8)) = 0;
      break;
    case '\x06':
      uVar3 = FUN_10007760(param_1);
      *(undefined1 *)((int)this + 0xc) = uVar3;
      uVar3 = FUN_10007760(param_1);
      *(undefined1 *)((int)this + 0xd) = uVar3;
      uVar3 = FUN_10007760(param_1);
      *(undefined1 *)((int)this + 0xe) = uVar3;
      uVar3 = FUN_10007760(param_1);
      pvVar5 = (void *)CONCAT31(extraout_var,uVar3);
      *(undefined1 *)((int)this + 0xf) = uVar3;
      break;
    case '\a':
      uVar6 = FUN_10001270(8);
      *(uint *)((int)this + 4) = uVar6;
      uVar11 = FUN_10007830(param_1);
      pvVar5 = (void *)uVar11;
      **(undefined8 **)((int)this + 4) = uVar11;
    }
    if ((char)param_1[5] != '\0') {
LAB_10007e12:
      return (uint)pvVar5 & 0xffffff00;
    }
    cVar2 = FUN_10007760(param_1);
    if (cVar2 != '\v') {
      piVar4 = (int *)KeyValuesSystem();
      pvVar5 = (void *)(**(code **)(*piVar4 + 4))(0x24);
      if (pvVar5 == (void *)0x0) {
        *(uint *)((int)this + 0x14) = 0;
        this = (uint *)0x0;
      }
      else {
        puVar7 = FUN_10005890(pvVar5,&DAT_1002536c);
        *(uint **)((int)this + 0x14) = puVar7;
        this = puVar7;
      }
      goto LAB_10007d92;
    }
  }
  return (uint)((char)param_1[5] == '\0');
}


// 10007fc0 FUN_10007fc0

void __thiscall FUN_10007fc0(void *this,char *param_1,char *param_2,int param_3,void *param_4)

{
  char cVar1;
  int iVar2;
  int *piVar3;
  void *this_00;
  undefined4 *this_01;
  undefined1 auStack_204 [3];
  char acStack_201 [513];
  
  if (param_3 != 0) {
    FUN_10009c10(acStack_201 + 1,param_1,0x200);
    for (iVar2 = FUN_10009820(acStack_201 + 1);
        ((0 < iVar2 && (acStack_201[iVar2] != '\\')) && (acStack_201[iVar2] != '/'));
        iVar2 = iVar2 + -1) {
      acStack_201[iVar2] = '\0';
    }
    FUN_10009af0(acStack_201 + 1,param_2,0x200,0xffffffff);
    piVar3 = (int *)KeyValuesSystem();
    this_00 = (void *)(**(code **)(*piVar3 + 4))(0x24);
    if (this_00 == (void *)0x0) {
      this_01 = (undefined4 *)0x0;
    }
    else {
      this_01 = FUN_10005890(this_00,auStack_204);
    }
    *(bool *)((int)this_01 + 0x11) = *(char *)((int)this + 0x11) != '\0';
    cVar1 = FUN_100079d0(this_01,(int *)param_3,auStack_204);
    if (cVar1 != '\0') {
      FUN_10008ec0(param_4,*(int *)((int)param_4 + 0xc),(undefined4 *)&stack0xfffffdf8);
      return;
    }
    DevMsg("KeyValues::ParseIncludedKeys: Couldn\'t load included keyvalue file %s\n",auStack_204);
    FUN_10004ff0((int)this_01);
    piVar3 = (int *)KeyValuesSystem();
    (**(code **)(*piVar3 + 8))(this_01);
  }
  return;
}


// 100080e0 FUN_100080e0

undefined4 __thiscall FUN_100080e0(void *this,char *param_1,int *param_2,int param_3,void *param_4)

{
  int iVar1;
  int iVar2;
  DWORD DVar3;
  DWORD DVar4;
  DWORD DVar5;
  byte *pbVar6;
  uint uVar7;
  char *pcVar8;
  void *this_00;
  undefined4 *this_01;
  undefined4 uVar9;
  int *piVar10;
  int iVar11;
  undefined4 *puVar12;
  int iVar13;
  char cStack_2f;
  char cStack_2e;
  char cStack_2d;
  undefined4 *puStack_2c;
  DWORD *pDStack_28;
  undefined4 uStack_24;
  int iStack_20;
  int iStack_1c;
  DWORD *pDStack_18;
  int iStack_14;
  undefined4 uStack_10;
  int iStack_c;
  int iStack_8;
  int iStack_4;
  
  DVar4 = GetCurrentThreadId();
  if (DVar4 != DAT_1003800c) {
    DVar5 = 0;
    LOCK();
    DVar3 = DVar4;
    if (DAT_1003800c != 0) {
      DVar5 = DAT_1003800c;
      DVar3 = DAT_1003800c;
    }
    DAT_1003800c = DVar3;
    UNLOCK();
    if (DVar5 != 0) {
      CThreadFastMutex::Lock((CThreadFastMutex *)&DAT_1003800c,DVar4,0);
      goto LAB_10008127;
    }
  }
  DAT_10038010 = DAT_10038010 + 1;
LAB_10008127:
  puStack_2c = (undefined4 *)0x0;
  pDStack_28 = (DWORD *)0x0;
  uStack_24 = 0;
  iStack_20 = 0;
  iStack_1c = 0;
  pDStack_18 = (DWORD *)0x0;
  iStack_14 = 0;
  uStack_10 = 0;
  iStack_c = 0;
  iStack_8 = 0;
  iStack_4 = 0;
  PTR_DAT_100350e8 = param_1;
  DAT_100350f0 = 0;
  this_01 = (undefined4 *)this;
  do {
    cStack_2d = '\x01';
    pbVar6 = FUN_100050b0(this,param_2,&cStack_2f,&cStack_2e);
    if ((((char)param_2[5] != '\0') || (pbVar6 == (byte *)0x0)) ||
       ((cStack_2f == '\0' && (*pbVar6 == 0)))) break;
    uVar7 = FUN_100098d0(pbVar6,(byte *)"#include");
    puVar12 = this_01;
    if (uVar7 == 0) {
      pcVar8 = FUN_100050b0(this,param_2,&cStack_2f,&cStack_2e);
      if ((pcVar8 == (char *)0x0) || (*pcVar8 == '\0')) {
        FUN_10004f10(&DAT_10034fe8,"#include is NULL ");
      }
      else {
        FUN_10007fc0(this,param_1,pcVar8,param_3,param_4);
      }
    }
    else {
      uVar7 = FUN_100098d0(pbVar6,(byte *)"#base");
      if (uVar7 == 0) {
        pcVar8 = FUN_100050b0(this,param_2,&cStack_2f,&cStack_2e);
        if ((pcVar8 == (char *)0x0) || (*pcVar8 == '\0')) {
          FUN_10004f10(&DAT_10034fe8,"#base is NULL ");
        }
        else {
          FUN_10007fc0(this,param_1,pcVar8,param_3,param_4);
        }
      }
      else {
        piVar10 = (int *)KeyValuesSystem();
        if (this_01 == (undefined4 *)0x0) {
          this_00 = (void *)(**(code **)(*piVar10 + 4))(0x24);
          if (this_00 == (void *)0x0) {
            this_01 = (undefined4 *)0x0;
          }
          else {
            this_01 = FUN_10005890(this_00,pbVar6);
          }
          *(bool *)((int)this_01 + 0x11) = *(char *)((int)this + 0x11) != '\0';
          if (puStack_2c != (undefined4 *)0x0) {
            puStack_2c[5] = this_01;
          }
        }
        else {
          uVar9 = (**(code **)(*piVar10 + 0xc))(pbVar6,1);
          *this_01 = uVar9;
        }
        pcVar8 = FUN_100050b0(this,param_2,&cStack_2f,&cStack_2e);
        if (cStack_2e != '\0') {
          cStack_2d = FUN_10005450();
          pcVar8 = FUN_100050b0(this,param_2,&cStack_2f,&cStack_2e);
        }
        if (((pcVar8 == (char *)0x0) || (*pcVar8 != '{')) || (cStack_2f != '\0')) {
          FUN_10004f10(&DAT_10034fe8,"LoadFromBuffer: missing {");
        }
        else {
          FUN_10006d20(this_01,param_1,param_2);
        }
        if (cStack_2d == '\0') {
          if (puStack_2c != (undefined4 *)0x0) {
            puStack_2c[5] = 0;
          }
          iVar1 = this_01[6];
          if (iVar1 != 0) {
            FUN_10004ff0(iVar1);
            piVar10 = (int *)KeyValuesSystem();
            (**(code **)(*piVar10 + 8))(iVar1);
          }
          this_01[6] = 0;
          *(undefined1 *)(this_01 + 4) = 0;
          puVar12 = this_01;
        }
        else {
          puVar12 = (undefined4 *)0x0;
          puStack_2c = this_01;
        }
      }
    }
    this_01 = puVar12;
  } while ((char)param_2[5] == '\0');
  FUN_10006ce0(this,(int *)&pDStack_28);
  iVar13 = iStack_14;
  iVar2 = iStack_8;
  iVar1 = iStack_1c;
  while (iVar1 = iVar1 + -1, iStack_14 = iVar13, iStack_8 = iVar2, 0 < iVar1) {
    DVar4 = pDStack_28[iVar1];
    if (DVar4 != 0) {
      FUN_10004ff0(DVar4);
      piVar10 = (int *)KeyValuesSystem();
      (**(code **)(*piVar10 + 8))(DVar4);
      iVar13 = iStack_14;
      iVar2 = iStack_8;
    }
  }
  iVar11 = 0;
  iVar1 = iStack_c;
  if (0 < iVar2) {
    do {
      FUN_10006010(this,*(int *)(iVar13 + iVar11 * 4));
      iVar11 = iVar11 + 1;
      iVar1 = iStack_c;
    } while (iVar11 < iVar2);
  }
  while (iVar2 = iVar2 + -1, iStack_c = iVar1, -1 < iVar2) {
    iVar11 = *(int *)(iVar13 + iVar2 * 4);
    if (iVar11 != 0) {
      FUN_10004ff0(iVar11);
      piVar10 = (int *)KeyValuesSystem();
      (**(code **)(*piVar10 + 8))(iVar11);
      iVar1 = iStack_c;
    }
  }
  PTR_DAT_100350e8 = &DAT_1002536c;
  DAT_100350f0 = 0;
  iStack_8 = 0;
  if (-1 < iVar1) {
    if (iVar13 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar13);
      iVar13 = 0;
      iStack_14 = 0;
    }
    uStack_10 = 0;
  }
  iStack_4 = iVar13;
  if (-1 < iVar1) {
    if (iVar13 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar13);
      iStack_14 = 0;
    }
    uStack_10 = 0;
  }
  iVar1 = iStack_20;
  iStack_1c = 0;
  if (-1 < iStack_20) {
    if (pDStack_28 != (DWORD *)0x0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(pDStack_28);
      pDStack_28 = (DWORD *)0x0;
    }
    uStack_24 = 0;
  }
  if ((-1 < iVar1) && (pDStack_28 != (DWORD *)0x0)) {
    pDStack_18 = pDStack_28;
    pDStack_28 = (DWORD *)(**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(pDStack_28);
  }
  DAT_10038010 = DAT_10038010 + -1;
  if (DAT_10038010 == 0) {
    pDStack_28 = &DAT_1003800c;
    LOCK();
    DAT_1003800c = 0;
    UNLOCK();
  }
  return CONCAT31((int3)((uint)pDStack_28 >> 8),1);
}


// 100084f0 FUN_100084f0

uint __thiscall FUN_100084f0(void *this,int *param_1)

{
  uint in_EAX;
  uint uVar1;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) != 0) {
    return in_EAX & 0xffffff00;
  }
  if ((char)param_1[5] == '\0') {
    FUN_10004ff0((int)this);
    *(undefined4 *)this = 0xffffffff;
    *(undefined1 *)((int)this + 0x10) = 0;
    *(undefined4 *)((int)this + 0x18) = 0;
    *(undefined4 *)((int)this + 0x14) = 0;
    *(undefined4 *)((int)this + 0x1c) = 0;
    *(undefined4 *)((int)this + 4) = 0;
    *(undefined4 *)((int)this + 8) = 0;
    *(undefined4 *)((int)this + 0xc) = 0;
    *(undefined1 *)((int)this + 0x11) = 0;
    *(undefined4 *)((int)this + 0x20) = 0;
    *(undefined2 *)((int)this + 0x12) = 0;
    uVar1 = FUN_10007d50(this,param_1,0);
    if ((char)uVar1 != '\0') {
      return CONCAT31((int3)(uVar1 >> 8),1);
    }
    in_EAX = FUN_10004ff0((int)this);
    *(undefined4 *)this = 0xffffffff;
    *(undefined1 *)((int)this + 0x10) = 0;
    *(undefined4 *)((int)this + 0x18) = 0;
    *(undefined4 *)((int)this + 0x14) = 0;
    *(undefined4 *)((int)this + 0x1c) = 0;
    *(undefined4 *)((int)this + 4) = 0;
    *(undefined4 *)((int)this + 8) = 0;
    *(undefined4 *)((int)this + 0xc) = 0;
    *(undefined1 *)((int)this + 0x11) = 0;
    *(undefined4 *)((int)this + 0x20) = 0;
    *(undefined2 *)((int)this + 0x12) = 0;
  }
  return in_EAX & 0xffffff00;
}


// 10008580 FUN_10008580

void __thiscall FUN_10008580(void *this,int param_1)

{
  int iVar1;
  uint uVar2;
  undefined4 uVar3;
  uint uVar4;
  
  iVar1 = *(int *)((int)this + 8);
  if (-1 < iVar1) {
    uVar2 = *(uint *)((int)this + 4);
    uVar4 = param_1 + uVar2;
    if (iVar1 == 0) {
      if (uVar2 == 0) {
        uVar2 = 3;
      }
      for (; (int)uVar2 < (int)uVar4; uVar2 = uVar2 * 2) {
      }
    }
    else {
      uVar2 = ((int)(uVar4 - 1) / iVar1 + 1) * iVar1;
    }
    if ((int)(uVar2 & 0xffff) < (int)uVar4) {
      if (((short)uVar2 == 0) && ((int)uVar4 < 0x10000)) {
        uVar2 = uVar2 - 1;
      }
      else {
        if ((uVar4 & 0xffff) != uVar4) {
          return;
        }
        do {
          uVar2 = (int)(uVar2 + uVar4) / 2;
        } while ((int)(uVar2 & 0xffff) < (int)uVar4);
      }
    }
                    /* WARNING: Load size is inaccurate */
    *(uint *)((int)this + 4) = uVar2;
    if (*this != 0) {
      uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
      *(undefined4 *)this = uVar3;
      return;
    }
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(uVar2 * 0xc);
    *(undefined4 *)this = uVar3;
  }
  return;
}


// 10008630 FUN_10008630

void __thiscall FUN_10008630(void *this,short *param_1,short *param_2)

{
  ushort uVar1;
  
  uVar1 = *param_2 + 1;
  if (((uVar1 == 0) || (uVar1 != 0)) && ((int)(uint)uVar1 < *(int *)((int)this + 4))) {
    *param_1 = *param_2 + 1;
    return;
  }
  *param_1 = -1;
  return;
}


// 10008670 FUN_10008670

ushort __fastcall FUN_10008670(int param_1)

{
  ushort *puVar1;
  ushort uVar2;
  ushort *puVar3;
  int iVar4;
  ushort local_2;
  
  uVar2 = *(ushort *)(param_1 + 0x14);
  if (uVar2 != 0xffff) {
    *(undefined2 *)(param_1 + 0x14) = *(undefined2 *)(*(int *)(param_1 + 4) + 2 + (uint)uVar2 * 0xc)
    ;
    *(int *)(param_1 + 0x18) = *(int *)(param_1 + 4);
    return uVar2;
  }
  uVar2 = *(ushort *)(param_1 + 0x16);
  puVar1 = (ushort *)(param_1 + 0x16);
  if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)uVar2))
  {
    iVar4 = *(int *)(param_1 + 8);
    uVar2 = (0 < iVar4) - 1;
  }
  else {
    uVar2 = *puVar1 + 1;
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 <= (int)(uint)uVar2)) {
      uVar2 = 0xffff;
    }
    else {
      uVar2 = *puVar1 + 1;
    }
  }
  if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 <= (int)(uint)uVar2)) {
    FUN_10008580((void *)(param_1 + 4),1);
    uVar2 = *puVar1;
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)uVar2)
       ) {
      iVar4 = *(int *)(param_1 + 8);
      puVar3 = &local_2;
    }
    else {
      puVar3 = (ushort *)FUN_10008630((void *)(param_1 + 4),(short *)&local_2,(short *)puVar1);
    }
    uVar2 = *puVar3;
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 <= (int)(uint)uVar2)) {
      Error("CUtlRBTree overflow!\n");
    }
  }
  *puVar1 = uVar2;
  *(undefined4 *)(param_1 + 0x18) = *(undefined4 *)(param_1 + 4);
  return uVar2;
}


// 10008790 FUN_10008790

undefined2 * __thiscall FUN_10008790(void *this,ushort param_1)

{
  if ((DAT_100380e4 & 1) == 0) {
    DAT_100380e4 = DAT_100380e4 | 1;
    DAT_100380dc = 0xffff;
    DAT_100380de = 0xffff;
    DAT_100380e0 = 0xffff;
    DAT_100380e2 = 1;
  }
  if (param_1 != 0xffff) {
    return (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
  }
  return &DAT_100380dc;
}


// 100087f0 FUN_100087f0

bool __thiscall FUN_100087f0(void *this,ushort *param_1,ushort *param_2)

{
  byte bVar1;
  int iVar2;
  ushort uVar3;
  ushort uVar4;
  byte *pbVar6;
  ushort *puVar7;
  bool bVar8;
  uint uVar5;
  
  if ((*param_1 == 0xffff) && (param_1[1] == 0xffff)) {
    pbVar6 = *(byte **)((int)this + 0x20);
    if (*(char *)((int)this + 0x1c) == '\0') {
      uVar5 = FUN_1000b1b0(pbVar6);
      uVar3 = (ushort)uVar5;
    }
    else {
      uVar5 = FUN_1000b1f0(pbVar6);
      uVar3 = (ushort)uVar5;
    }
  }
  else {
    iVar2 = *(int *)(*(int *)((int)this + 0x24) + (uint)*param_1 * 4);
    uVar3 = *(ushort *)(iVar2 + 8 + (uint)param_1[1]);
    pbVar6 = (byte *)(iVar2 + (uint)param_1[1] + 10);
  }
  if ((*param_2 == 0xffff) && (param_2[1] == 0xffff)) {
    puVar7 = *(ushort **)((int)this + 0x20);
    if (*(char *)((int)this + 0x1c) == '\0') {
      uVar5 = FUN_1000b1b0((byte *)puVar7);
      uVar4 = (ushort)uVar5;
    }
    else {
      uVar5 = FUN_1000b1f0((byte *)puVar7);
      uVar4 = (ushort)uVar5;
    }
  }
  else {
    puVar7 = (ushort *)
             (*(int *)(*(int *)((int)this + 0x24) + (uint)*param_2 * 4) + 8 + (uint)param_2[1]);
    uVar4 = *puVar7;
    puVar7 = puVar7 + 1;
  }
  if (uVar3 != uVar4) {
    return uVar3 < uVar4;
  }
  if (pbVar6 == (byte *)0x0) {
    if (puVar7 != (ushort *)0x0) {
      return true;
    }
    return false;
  }
  if (puVar7 == (ushort *)0x0) {
    return true;
  }
  if (*(char *)((int)this + 0x1c) != '\0') {
    uVar5 = FUN_100098d0(pbVar6,(byte *)puVar7);
    return (int)uVar5 < 0;
  }
  while( true ) {
    bVar1 = *pbVar6;
    bVar8 = bVar1 < (byte)*puVar7;
    if (bVar1 != (byte)*puVar7) break;
    if (bVar1 == 0) {
      return false;
    }
    bVar1 = pbVar6[1];
    bVar8 = bVar1 < *(byte *)((int)puVar7 + 1);
    if (bVar1 != *(byte *)((int)puVar7 + 1)) break;
    pbVar6 = pbVar6 + 2;
    puVar7 = puVar7 + 1;
    if (bVar1 == 0) {
      return false;
    }
  }
  return (int)((1 - (uint)bVar8) - (uint)(bVar8 != 0)) < 0;
}


// 10008930 FUN_10008930

int __thiscall FUN_10008930(void *this,int param_1)

{
  int iVar1;
  int *piVar2;
  
  iVar1 = 0;
  if (0 < *(int *)((int)this + 0x30)) {
    piVar2 = *(int **)((int)this + 0x24);
    do {
      if (param_1 <= *(int *)*piVar2 - ((int *)*piVar2)[1]) {
        return iVar1;
      }
      iVar1 = iVar1 + 1;
      piVar2 = piVar2 + 1;
    } while (iVar1 < *(int *)((int)this + 0x30));
  }
  return -1;
}


// 10008970 FUN_10008970

ushort * __thiscall FUN_10008970(void *this,ushort *param_1,ushort *param_2,undefined1 *param_3)

{
  ushort uVar1;
  int iVar2;
  bool bVar3;
  ushort *puVar4;
  
  uVar1 = *(ushort *)((int)this + 0x10);
  *param_2 = 0xffff;
  *param_3 = 0;
  puVar4 = param_2;
  while (uVar1 != 0xffff) {
    *param_2 = uVar1;
    iVar2 = (uint)uVar1 * 0xc;
    bVar3 = FUN_100087f0(this,param_1,(ushort *)(iVar2 + 8 + *(int *)((int)this + 4)));
    if (bVar3) {
      *param_3 = 1;
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100380dc;
        uVar1 = DAT_100380dc;
      }
      else {
        puVar4 = (ushort *)(*(int *)((int)this + 4) + iVar2);
        uVar1 = *(ushort *)(*(int *)((int)this + 4) + iVar2);
      }
    }
    else {
      *param_3 = 0;
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100380dc;
      }
      else {
        puVar4 = (ushort *)(*(int *)((int)this + 4) + iVar2);
      }
      uVar1 = puVar4[1];
    }
  }
  return puVar4;
}


// 10008a80 FUN_10008a80

void __thiscall FUN_10008a80(void *this,ushort param_1)

{
  ushort uVar1;
  undefined2 *puVar2;
  short *psVar3;
  ushort *puVar4;
  
  puVar2 = FUN_10008790(this,param_1);
  uVar1 = puVar2[1];
  puVar2 = FUN_10008790(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + 2 + (uint)param_1 * 0xc) = *puVar2;
  psVar3 = FUN_10008790(this,uVar1);
  if (*psVar3 != -1) {
    puVar4 = FUN_10008790(this,uVar1);
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)*puVar4 * 0xc) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar2 = FUN_10008790(this,param_1);
    *(undefined2 *)(*(int *)((int)this + 4) + 4 + (uint)uVar1 * 0xc) = puVar2[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar2 = FUN_10008790(this,param_1);
    puVar4 = FUN_10008790(this,puVar2[2]);
    if (*puVar4 == param_1) {
      puVar2 = FUN_10008790(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar2[2] * 0xc) = uVar1;
    }
    else {
      puVar2 = FUN_10008790(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)(ushort)puVar2[2] * 0xc) = uVar1;
    }
  }
  *(ushort *)(*(int *)((int)this + 4) + (uint)uVar1 * 0xc) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0xc) = uVar1;
  }
  return;
}


// 10008b80 FUN_10008b80

void __thiscall FUN_10008b80(void *this,ushort param_1)

{
  ushort uVar1;
  ushort *puVar2;
  undefined2 *puVar3;
  
  puVar2 = FUN_10008790(this,param_1);
  uVar1 = *puVar2;
  puVar3 = FUN_10008790(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc) = puVar3[1];
  puVar3 = FUN_10008790(this,uVar1);
  if (puVar3[1] != -1) {
    puVar3 = FUN_10008790(this,uVar1);
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)(ushort)puVar3[1] * 0xc) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar3 = FUN_10008790(this,param_1);
    *(undefined2 *)(*(int *)((int)this + 4) + 4 + (uint)uVar1 * 0xc) = puVar3[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar3 = FUN_10008790(this,param_1);
    puVar3 = FUN_10008790(this,puVar3[2]);
    if (puVar3[1] == param_1) {
      puVar3 = FUN_10008790(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)(ushort)puVar3[2] * 0xc) = uVar1;
    }
    else {
      puVar3 = FUN_10008790(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar3[2] * 0xc) = uVar1;
    }
  }
  *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)uVar1 * 0xc) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0xc) = uVar1;
  }
  return;
}


// 10008c80 FUN_10008c80

int __thiscall FUN_10008c80(void *this,undefined4 param_1,int param_2)

{
  undefined4 uVar1;
  
  *(undefined4 *)((int)this + 4) = 0;
  *(int *)((int)this + 8) = param_2;
  *(undefined4 *)((int)this + 0xc) = param_1;
  if (param_2 != 0) {
    uVar1 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_2 * 0xc);
    *(undefined4 *)((int)this + 4) = uVar1;
  }
  *(undefined2 *)((int)this + 0x10) = 0xffff;
  *(undefined2 *)((int)this + 0x14) = 0xffff;
  *(undefined2 *)((int)this + 0x12) = 0;
  *(undefined2 *)((int)this + 0x16) = 0xffff;
  *(undefined4 *)((int)this + 0x18) = *(undefined4 *)((int)this + 4);
  return (int)this;
}


// 10008ce0 FUN_10008ce0

ushort __thiscall FUN_10008ce0(void *this,ushort *param_1)

{
  int iVar1;
  bool bVar2;
  undefined2 *puVar3;
  ushort uVar4;
  
  uVar4 = *(ushort *)((int)this + 0x10);
  if (uVar4 == 0xffff) {
    return 0xffff;
  }
  do {
    iVar1 = (uint)uVar4 * 0xc;
    bVar2 = FUN_100087f0(this,param_1,(ushort *)(*(int *)((int)this + 4) + 8 + iVar1));
    if (bVar2) {
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      bVar2 = uVar4 != 0xffff;
      uVar4 = DAT_100380dc;
      if (bVar2) {
        uVar4 = *(ushort *)(*(int *)((int)this + 4) + iVar1);
      }
    }
    else {
      bVar2 = FUN_100087f0(this,(ushort *)(*(int *)((int)this + 4) + 8 + iVar1),param_1);
      if (!bVar2) {
        return uVar4;
      }
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      if (uVar4 == 0xffff) {
        puVar3 = &DAT_100380dc;
      }
      else {
        puVar3 = (undefined2 *)(*(int *)((int)this + 4) + iVar1);
      }
      uVar4 = puVar3[1];
    }
  } while (uVar4 != 0xffff);
  return 0xffff;
}


// 10008df0 FUN_10008df0

void __fastcall FUN_10008df0(void *param_1)

{
  ushort uVar1;
  int iVar2;
  ushort uVar3;
  ushort uVar4;
  ushort *puVar5;
  
  uVar1 = *(ushort *)((int)param_1 + 0x16);
  if (uVar1 != 0xffff) {
    uVar3 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (uVar3 != 0xffff) {
      if ((((uVar3 == 0) || (uVar3 != 0)) && ((int)(uint)uVar3 < *(int *)((int)param_1 + 8))) &&
         ((uVar3 <= uVar1 && (puVar5 = FUN_10008790(param_1,uVar3), *puVar5 != uVar3)))) {
        iVar2 = (uint)uVar3 * 0xc;
        *(undefined2 *)(iVar2 + 2 + *(int *)((int)param_1 + 4)) =
             *(undefined2 *)((int)param_1 + 0x14);
        *(ushort *)(iVar2 + *(int *)((int)param_1 + 4)) = uVar3;
        *(ushort *)((int)param_1 + 0x14) = uVar3;
      }
      uVar1 = *(ushort *)((int)param_1 + 0x16);
      if (uVar3 == uVar1) break;
      uVar4 = uVar3 + 1;
      if (((uVar4 != 0) && (uVar4 == 0)) || (*(int *)((int)param_1 + 8) <= (int)(uint)uVar4)) {
        uVar3 = 0xffff;
      }
      else {
        uVar3 = uVar3 + 1;
      }
    }
    *(undefined2 *)((int)param_1 + 0x10) = 0xffff;
    *(undefined2 *)((int)param_1 + 0x12) = 0;
  }
  return;
}


// 10008ec0 FUN_10008ec0

int __thiscall FUN_10008ec0(void *this,int param_1,undefined4 *param_2)

{
  void *pvVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_1000e340(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar3 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar3) {
    pvVar1 = (void *)(*this + param_1 * 4);
    FUN_10009be0((void *)((int)pvVar1 + 4),pvVar1,iVar3 * 4);
  }
                    /* WARNING: Load size is inaccurate */
  puVar2 = (undefined4 *)(*this + param_1 * 4);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_2;
  }
  return param_1;
}


// 10008f30 FUN_10008f30

void __thiscall FUN_10008f30(void *this,ushort param_1)

{
  ushort uVar1;
  ushort uVar2;
  ushort uVar3;
  undefined2 *puVar4;
  ushort *puVar5;
  uint uVar6;
  uint uVar7;
  
  if (param_1 != *(ushort *)((int)this + 0x10)) {
    do {
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      if (param_1 == 0xffff) {
        puVar4 = &DAT_100380dc;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
      }
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      if (puVar4[2] == 0xffff) {
        puVar4 = &DAT_100380dc;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0xc);
      }
      if (puVar4[3] != 0) break;
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      if (param_1 == 0xffff) {
        puVar4 = &DAT_100380dc;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
      }
      uVar1 = puVar4[2];
      uVar7 = (uint)uVar1;
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100380dc;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar1 * 0xc);
      }
      uVar2 = puVar4[2];
      uVar6 = (uint)uVar2;
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100380dc;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar1 * 0xc);
      }
      if ((DAT_100380e4 & 1) == 0) {
        DAT_100380e4 = DAT_100380e4 | 1;
        DAT_100380dc = 0xffff;
        DAT_100380de = 0xffff;
        DAT_100380e0 = 0xffff;
        DAT_100380e2 = 1;
      }
      if (puVar4[2] == 0xffff) {
        puVar5 = &DAT_100380dc;
      }
      else {
        puVar5 = (ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0xc);
      }
      if (*puVar5 == uVar1) {
        if ((DAT_100380e4 & 1) == 0) {
          DAT_100380e4 = DAT_100380e4 | 1;
          DAT_100380dc = 0xffff;
          DAT_100380de = 0xffff;
          DAT_100380e0 = 0xffff;
          DAT_100380e2 = 1;
        }
        if (uVar2 == 0xffff) {
          puVar4 = &DAT_100380dc;
        }
        else {
          puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar2 * 0xc);
        }
        uVar3 = puVar4[1];
        if ((DAT_100380e4 & 1) == 0) {
          DAT_100380e4 = DAT_100380e4 | 1;
          DAT_100380dc = 0xffff;
          DAT_100380de = 0xffff;
          DAT_100380e0 = 0xffff;
          DAT_100380e2 = 1;
        }
        if (uVar3 == 0xffff) {
          puVar4 = &DAT_100380dc;
        }
        else {
          puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar3 * 0xc);
        }
        if (puVar4[3] == 0) {
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar1 * 0xc) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar3 * 0xc) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0xc) = 0;
          param_1 = uVar2;
        }
        else {
          if ((DAT_100380e4 & 1) == 0) {
            DAT_100380e4 = DAT_100380e4 | 1;
            DAT_100380dc = 0xffff;
            DAT_100380de = 0xffff;
            DAT_100380e0 = 0xffff;
            DAT_100380e2 = 1;
          }
          if (param_1 == 0xffff) {
            puVar4 = &DAT_100380dc;
          }
          else {
            puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
          }
          if ((DAT_100380e4 & 1) == 0) {
            DAT_100380e4 = DAT_100380e4 | 1;
            DAT_100380dc = 0xffff;
            DAT_100380de = 0xffff;
            DAT_100380e0 = 0xffff;
            DAT_100380e2 = 1;
          }
          if (puVar4[2] == 0xffff) {
            puVar4 = &DAT_100380dc;
          }
          else {
            puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0xc);
          }
          if (puVar4[1] == param_1) {
            FUN_10008a80(this,uVar1);
            puVar4 = FUN_10008790(this,uVar1);
            uVar7 = (uint)(ushort)puVar4[2];
            puVar4 = FUN_10008790(this,puVar4[2]);
            uVar6 = (uint)(ushort)puVar4[2];
            param_1 = uVar1;
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar7 * 0xc) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0xc) = 0;
          FUN_10008b80(this,(ushort)uVar6);
        }
      }
      else {
        if ((DAT_100380e4 & 1) == 0) {
          DAT_100380e4 = DAT_100380e4 | 1;
          DAT_100380dc = 0xffff;
          DAT_100380de = 0xffff;
          DAT_100380e0 = 0xffff;
          DAT_100380e2 = 1;
        }
        if (uVar2 == 0xffff) {
          puVar5 = &DAT_100380dc;
        }
        else {
          puVar5 = (ushort *)(*(int *)((int)this + 4) + (uint)uVar2 * 0xc);
        }
        uVar3 = *puVar5;
        if ((DAT_100380e4 & 1) == 0) {
          DAT_100380e4 = DAT_100380e4 | 1;
          DAT_100380dc = 0xffff;
          DAT_100380de = 0xffff;
          DAT_100380e0 = 0xffff;
          DAT_100380e2 = 1;
        }
        if (uVar3 == 0xffff) {
          puVar4 = &DAT_100380dc;
        }
        else {
          puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar3 * 0xc);
        }
        if (puVar4[3] == 0) {
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar1 * 0xc) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar3 * 0xc) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar2 * 0xc) = 0;
          param_1 = uVar2;
        }
        else {
          if ((DAT_100380e4 & 1) == 0) {
            DAT_100380e4 = DAT_100380e4 | 1;
            DAT_100380dc = 0xffff;
            DAT_100380de = 0xffff;
            DAT_100380e0 = 0xffff;
            DAT_100380e2 = 1;
          }
          if (param_1 == 0xffff) {
            puVar4 = &DAT_100380dc;
          }
          else {
            puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
          }
          if ((DAT_100380e4 & 1) == 0) {
            DAT_100380e4 = DAT_100380e4 | 1;
            DAT_100380dc = 0xffff;
            DAT_100380de = 0xffff;
            DAT_100380e0 = 0xffff;
            DAT_100380e2 = 1;
          }
          if (puVar4[2] == 0xffff) {
            puVar5 = &DAT_100380dc;
          }
          else {
            puVar5 = (ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0xc);
          }
          if (*puVar5 == param_1) {
            FUN_10008b80(this,uVar1);
            puVar4 = FUN_10008790(this,uVar1);
            uVar7 = (uint)(ushort)puVar4[2];
            puVar4 = FUN_10008790(this,puVar4[2]);
            uVar6 = (uint)(ushort)puVar4[2];
            param_1 = uVar1;
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar7 * 0xc) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0xc) = 0;
          FUN_10008a80(this,(ushort)uVar6);
        }
      }
    } while (param_1 != *(ushort *)((int)this + 0x10));
  }
  *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)*(ushort *)((int)this + 0x10) * 0xc) = 1;
  return;
}


// 100094a0 FUN_100094a0

ushort * __thiscall FUN_100094a0(void *this,ushort *param_1,int param_2)

{
  ushort uVar1;
  
  if (param_2 == 0) {
    *param_1 = 0xffff;
    return param_1;
  }
  *(int *)((int)this + 0x20) = param_2;
  param_2 = -1;
  uVar1 = FUN_10008ce0(this,(ushort *)&param_2);
  *param_1 = uVar1;
  return param_1;
}


// 100094e0 FUN_100094e0

void __thiscall FUN_100094e0(void *this,ushort param_1,ushort param_2,char param_3)

{
  undefined2 *puVar1;
  
  puVar1 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
  puVar1[2] = param_2;
  puVar1[1] = 0xffff;
  *puVar1 = 0xffff;
  puVar1[3] = 0;
  if (param_2 == 0xffff) {
    *(ushort *)((int)this + 0x10) = param_1;
    FUN_10008f30(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(ushort *)(*(int *)((int)this + 4) + (uint)param_2 * 0xc) = param_1;
    FUN_10008f30(this,param_1);
    return;
  }
  *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)param_2 * 0xc) = param_1;
  FUN_10008f30(this,param_1);
  return;
}


// 10009550 FUN_10009550

void __fastcall FUN_10009550(void *param_1)

{
  int iVar1;
  
  FUN_10008df0(param_1);
  *(undefined2 *)((int)param_1 + 0x14) = 0xffff;
  if (-1 < *(int *)((int)param_1 + 0xc)) {
    if (*(int *)((int)param_1 + 4) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)param_1 + 4));
      *(undefined4 *)((int)param_1 + 4) = 0;
    }
    *(undefined4 *)((int)param_1 + 8) = 0;
  }
  *(undefined2 *)((int)param_1 + 0x16) = 0xffff;
  iVar1 = 0;
  if (0 < *(int *)((int)param_1 + 0x30)) {
    do {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
                (*(undefined4 *)(*(int *)((int)param_1 + 0x24) + iVar1 * 4));
      iVar1 = iVar1 + 1;
    } while (iVar1 < *(int *)((int)param_1 + 0x30));
  }
  *(undefined4 *)((int)param_1 + 0x30) = 0;
  return;
}


// 100095d0 FUN_100095d0

void __fastcall FUN_100095d0(void *param_1)

{
  FUN_10008df0(param_1);
  *(undefined2 *)((int)param_1 + 0x14) = 0xffff;
  if (-1 < *(int *)((int)param_1 + 0xc)) {
    if (*(int *)((int)param_1 + 4) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)param_1 + 4));
      *(undefined4 *)((int)param_1 + 4) = 0;
    }
    *(undefined4 *)((int)param_1 + 8) = 0;
  }
  *(undefined2 *)((int)param_1 + 0x16) = 0xffff;
  if (-1 < *(int *)((int)param_1 + 0xc)) {
    if (*(int *)((int)param_1 + 4) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)param_1 + 4));
      *(undefined4 *)((int)param_1 + 4) = 0;
    }
    *(undefined4 *)((int)param_1 + 8) = 0;
  }
  return;
}


// 10009630 FUN_10009630

void __fastcall FUN_10009630(void *param_1)

{
  FUN_10009550(param_1);
  FUN_1000d370((int *)((int)param_1 + 0x24));
  FUN_100095d0(param_1);
  return;
}


// 10009650 FUN_10009650

undefined4 __thiscall FUN_10009650(void *this,ushort *param_1)

{
  undefined4 *puVar1;
  ushort *puVar2;
  ushort uVar3;
  void *local_4;
  
  puVar2 = param_1;
  local_4 = this;
  FUN_10008970(this,param_1,(ushort *)&local_4,(undefined1 *)&param_1);
  uVar3 = FUN_10008670((int)this);
  FUN_100094e0(this,uVar3,(ushort)local_4,(char)param_1);
  *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + 1;
  puVar1 = (undefined4 *)(*(int *)((int)this + 4) + 8 + (uint)uVar3 * 0xc);
  if (puVar1 != (undefined4 *)0x0) {
    *puVar1 = *(undefined4 *)puVar2;
  }
  return CONCAT22((short)((uint)puVar1 >> 0x10),uVar3);
}


// 100096b0 FUN_100096b0

void * __thiscall FUN_100096b0(void *this,undefined4 param_1,int param_2,undefined1 param_3)

{
  FUN_10008c80(this,param_1,param_2);
  *(undefined1 *)((int)this + 0x1c) = param_3;
  *(undefined4 *)((int)this + 0x24) = 0;
  *(undefined4 *)((int)this + 0x28) = 0;
  *(undefined4 *)((int)this + 0x30) = 0;
  *(undefined4 *)((int)this + 0x34) = 0;
  *(undefined4 *)((int)this + 0x2c) = 8;
  return this;
}


// 100096f0 FUN_100096f0

ushort * __thiscall FUN_100096f0(void *this,ushort *param_1,byte *param_2)

{
  undefined2 *puVar1;
  byte bVar2;
  int iVar3;
  byte *pbVar4;
  ushort uVar5;
  undefined2 uVar6;
  byte *pbVar7;
  int iVar8;
  undefined4 uVar9;
  uint uVar10;
  uint uVar11;
  int *local_8;
  byte *local_4;
  
  pbVar4 = param_2;
  if (param_2 == (byte *)0x0) {
    *param_1 = 0xffff;
    return param_1;
  }
  *(byte **)((int)this + 0x20) = param_2;
  local_8 = (int *)0xffffffff;
  uVar5 = FUN_10008ce0(this,(ushort *)&local_8);
  if (uVar5 == 0xffff) {
    pbVar7 = pbVar4;
    do {
      bVar2 = *pbVar7;
      pbVar7 = pbVar7 + 1;
    } while (bVar2 != 0);
    local_4 = pbVar7 + (1 - (int)(pbVar4 + 1));
    uVar10 = (uint)(pbVar7 + (4 - (int)(pbVar4 + 1))) & 0xfffffffe;
    iVar8 = FUN_10008930(this,uVar10);
    if (iVar8 == -1) {
      uVar11 = uVar10;
      if ((int)uVar10 < 0x801) {
        uVar11 = 0x800;
      }
      local_8 = (int *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(uVar11);
      *local_8 = uVar11 - 0xc;
      local_8[1] = 0;
      iVar8 = FUN_10008ec0((void *)((int)this + 0x24),*(int *)((int)this + 0x30),&local_8);
    }
    if (*(char *)((int)this + 0x1c) == '\0') {
      uVar11 = FUN_1000b1b0(pbVar4);
      uVar6 = (undefined2)uVar11;
    }
    else {
      uVar11 = FUN_1000b1f0(pbVar4);
      uVar6 = (undefined2)uVar11;
    }
    iVar3 = *(int *)(*(int *)((int)this + 0x24) + iVar8 * 4);
    local_8 = (int *)(uint)*(ushort *)(iVar3 + 4);
    puVar1 = (undefined2 *)(iVar3 + 8 + *(int *)(iVar3 + 4));
    *puVar1 = uVar6;
    _memcpy(puVar1 + 1,param_2,(size_t)local_4);
    *(int *)(iVar3 + 4) = *(int *)(iVar3 + 4) + uVar10;
    param_2 = (byte *)CONCAT22(local_8._0_2_,(short)iVar8);
    uVar9 = FUN_10009650(this,(ushort *)&param_2);
    uVar5 = (ushort)uVar9;
  }
  *param_1 = uVar5;
  return param_1;
}


// 10009820 FUN_10009820

int __cdecl FUN_10009820(char *param_1)

{
  char *pcVar1;
  char cVar2;
  
  AssertValidStringPtr(param_1,0xffffff);
  pcVar1 = param_1 + 1;
  do {
    cVar2 = *param_1;
    param_1 = param_1 + 1;
  } while (cVar2 != '\0');
  return (int)param_1 - (int)pcVar1;
}


// 10009850 FUN_10009850

int __cdecl FUN_10009850(short *param_1)

{
  short *psVar1;
  short sVar2;
  
  psVar1 = param_1 + 1;
  do {
    sVar2 = *param_1;
    param_1 = param_1 + 1;
  } while (sVar2 != 0);
  return (int)param_1 - (int)psVar1 >> 1;
}


// 10009870 FUN_10009870

int __cdecl FUN_10009870(byte *param_1,byte *param_2)

{
  byte bVar1;
  bool bVar2;
  
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  while( true ) {
    bVar1 = *param_1;
    bVar2 = bVar1 < *param_2;
    if (bVar1 != *param_2) break;
    if (bVar1 == 0) {
      return 0;
    }
    bVar1 = param_1[1];
    bVar2 = bVar1 < param_2[1];
    if (bVar1 != param_2[1]) break;
    param_1 = param_1 + 2;
    param_2 = param_2 + 2;
    if (bVar1 == 0) {
      return 0;
    }
  }
  return (1 - (uint)bVar2) - (uint)(bVar2 != 0);
}


// 100098d0 FUN_100098d0

uint __cdecl FUN_100098d0(byte *param_1,byte *param_2)

{
  uint uVar1;
  uint uVar2;
  
  do {
    while( true ) {
      uVar1 = (uint)*param_1;
      uVar2 = (uint)*param_2;
      if (uVar1 == uVar2) {
        if (uVar1 == 0) {
          return 0;
        }
      }
      else {
        if (uVar2 == 0) {
          return uVar1;
        }
        if (uVar1 - 0x41 < 0x1a) {
          uVar1 = uVar1 + 0x20;
        }
        if (uVar2 - 0x41 < 0x1a) {
          uVar2 = uVar2 + 0x20;
        }
        if (uVar1 != uVar2) goto LAB_1000994a;
      }
      uVar1 = (uint)param_1[1];
      uVar2 = (uint)param_2[1];
      param_1 = param_1 + 2;
      param_2 = param_2 + 2;
      if (uVar1 != uVar2) break;
      if (uVar1 == 0) {
        return 0;
      }
    }
    if (uVar2 == 0) {
      return uVar1;
    }
    if (uVar1 - 0x41 < 0x1a) {
      uVar1 = uVar1 + 0x20;
    }
    if (uVar2 - 0x41 < 0x1a) {
      uVar2 = uVar2 + 0x20;
    }
  } while (uVar1 == uVar2);
LAB_1000994a:
  return uVar1 - uVar2;
}


// 10009950 FUN_10009950

int __cdecl FUN_10009950(char *param_1,char *param_2,int param_3)

{
  AssertValidStringPtr(param_1,param_3);
  AssertValidStringPtr(param_2,param_3);
  if (0 < param_3) {
    do {
      param_3 = param_3 + -1;
      if (*param_1 != *param_2) {
        return (uint)(*param_2 <= *param_1) * 2 + -1;
      }
      if (*param_1 == '\0') {
        return 0;
      }
      param_1 = param_1 + 1;
      param_2 = param_2 + 1;
    } while (0 < param_3);
  }
  return 0;
}


// 100099b0 FUN_100099b0

char * __cdecl FUN_100099b0(char *param_1,char *param_2)

{
  char cVar1;
  int iVar2;
  int iVar3;
  
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  do {
    if (*param_2 == '\0') {
      return param_1;
    }
    cVar1 = *param_1;
    iVar2 = _tolower((int)*param_2);
    iVar3 = _tolower((int)cVar1);
    param_2 = param_2 + 1;
    param_1 = param_1 + 1;
  } while (iVar3 == iVar2);
  return (char *)0x0;
}


// 10009a10 FUN_10009a10

char * __cdecl FUN_10009a10(char *param_1,char *param_2)

{
  char cVar1;
  int iVar2;
  char cVar3;
  char cVar4;
  char *pcVar5;
  
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  if ((param_1 != (char *)0x0) && (param_2 != (char *)0x0)) {
    cVar3 = *param_1;
    if (cVar3 != '\0') {
      cVar1 = *param_2;
      do {
        if ((byte)(cVar3 + 0xbfU) < 0x1a) {
          cVar3 = cVar3 + ' ';
        }
        cVar4 = cVar1;
        if ((byte)(cVar1 + 0xbfU) < 0x1a) {
          cVar4 = cVar1 + ' ';
        }
        if (cVar3 == cVar4) {
          cVar3 = param_2[1];
          pcVar5 = param_2 + 1;
          if (cVar3 == '\0') {
            return param_1;
          }
          iVar2 = 1 - (int)pcVar5;
          while( true ) {
            cVar4 = pcVar5[(int)(param_1 + iVar2)];
            if (cVar4 == '\0') {
              return (char *)0x0;
            }
            if ((byte)(cVar4 + 0xbfU) < 0x1a) {
              cVar4 = cVar4 + ' ';
            }
            if ((byte)(cVar3 + 0xbfU) < 0x1a) {
              cVar3 = cVar3 + ' ';
            }
            if (cVar4 != cVar3) break;
            cVar3 = pcVar5[1];
            pcVar5 = pcVar5 + 1;
            if (cVar3 == '\0') {
              return param_1;
            }
          }
          if (*pcVar5 == '\0') {
            return param_1;
          }
        }
        cVar3 = param_1[1];
        param_1 = param_1 + 1;
      } while (cVar3 != '\0');
    }
    return (char *)0x0;
  }
  return (char *)0x0;
}


// 10009af0 FUN_10009af0

char * __cdecl FUN_10009af0(char *param_1,char *param_2,uint param_3,size_t param_4)

{
  char cVar1;
  char *pcVar2;
  char *pcVar3;
  size_t _Count;
  
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  pcVar3 = param_2;
  do {
    cVar1 = *pcVar3;
    pcVar3 = pcVar3 + 1;
  } while (cVar1 != '\0');
  _Count = (int)pcVar3 - (int)(param_2 + 1);
  if ((-1 < (int)param_4) && ((int)param_4 < (int)_Count)) {
    _Count = param_4;
  }
  if (param_3 <= ((int)pcVar2 - (int)(param_1 + 1)) + _Count) {
    _Count = (param_3 - ((int)pcVar2 - (int)(param_1 + 1))) - 1;
  }
  if (_Count == 0) {
    return param_1;
  }
  pcVar2 = _strncat(param_1,param_2,_Count);
  pcVar2[param_3 - 1] = '\0';
  return pcVar2;
}


// 10009b80 FUN_10009b80

void __cdecl FUN_10009b80(char *param_1,char param_2)

{
  char cVar1;
  
  cVar1 = *param_1;
  while (cVar1 != '\0') {
    if ((*param_1 == '/') || (*param_1 == '\\')) {
      *param_1 = param_2;
    }
    param_1 = param_1 + 1;
    cVar1 = *param_1;
  }
  return;
}


// 10009bb0 FUN_10009bb0

void __cdecl FUN_10009bb0(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  _memcpy(param_1,param_2,param_3);
  return;
}


// 10009be0 FUN_10009be0

void __cdecl FUN_10009be0(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  _memmove(param_1,param_2,param_3);
  return;
}


// 10009c10 FUN_10009c10

void __cdecl FUN_10009c10(char *param_1,char *param_2,size_t param_3)

{
  _AssertValidWritePtr(param_1,param_3);
  AssertValidStringPtr(param_2,0xffffff);
  _strncpy(param_1,param_2,param_3);
  if (0 < (int)param_3) {
    param_1[param_3 - 1] = '\0';
  }
  return;
}


// 10009c50 FUN_10009c50

size_t __cdecl FUN_10009c50(char *param_1,size_t param_2,char *param_3)

{
  size_t sVar1;
  
  _AssertValidWritePtr(param_1,param_2);
  AssertValidStringPtr(param_3,0xffffff);
  sVar1 = __vsnprintf(param_1,param_2,param_3,&stack0x00000010);
  if ((int)sVar1 < 0) {
    param_1[param_2 - 1] = '\0';
    sVar1 = param_2;
  }
  return sVar1;
}


// 10009ca0 FUN_10009ca0

size_t __cdecl FUN_10009ca0(char *param_1,size_t param_2,char *param_3,va_list param_4)

{
  size_t sVar1;
  
  _AssertValidWritePtr(param_1,param_2);
  AssertValidStringPtr(param_3,0xffffff);
  sVar1 = __vsnprintf(param_1,param_2,param_3,param_4);
  if ((int)sVar1 < 0) {
    param_1[param_2 - 1] = '\0';
    sVar1 = param_2;
  }
  return sVar1;
}


// 10009cf0 FUN_10009cf0

size_t __cdecl FUN_10009cf0(char *param_1,size_t param_2,char *param_3,va_list param_4,int param_5)

{
  size_t sVar1;
  
  _AssertValidWritePtr(param_1,param_2);
  AssertValidStringPtr(param_3,0xffffff);
  sVar1 = __vsnprintf(param_1,param_2,param_3,param_4);
  if (param_5 != 0) {
    *(bool *)param_5 = (int)sVar1 < 0;
  }
  if ((int)sVar1 < 0) {
    param_1[param_2 - 1] = '\0';
    sVar1 = param_2;
  }
  return sVar1;
}


// 10009d50 FUN_10009d50

int __cdecl FUN_10009d50(LPCSTR param_1,LPWSTR param_2,uint param_3)

{
  int iVar1;
  
  if (param_1 == (LPCSTR)0x0) {
    return 0;
  }
  AssertValidStringPtr(param_1,0xffffff);
  _AssertValidWritePtr(param_2,1);
  *param_2 = L'\0';
  iVar1 = MultiByteToWideChar(0xfde9,0,param_1,-1,param_2,param_3 >> 1);
  param_2[(param_3 >> 1) - 1] = L'\0';
  return iVar1;
}


// 10009db0 FUN_10009db0

void __cdecl FUN_10009db0(LPCWSTR param_1,LPSTR param_2,int param_3)

{
  AssertValidStringPtr(param_2,param_3);
  _AssertValidReadPtr(param_1,1);
  if (0 < param_3) {
    *param_2 = '\0';
  }
  WideCharToMultiByte(0xfde9,0,param_1,-1,param_2,param_3,(LPCSTR)0x0,(LPBOOL)0x0);
  if (0 < param_3) {
    param_2[param_3 + -1] = '\0';
  }
  return;
}


// 10009e00 FUN_10009e00

undefined4 __cdecl FUN_10009e00(byte *param_1,size_t param_2)

{
  uint in_EAX;
  int iVar1;
  size_t sVar2;
  
  if (((*param_1 == 0) || (in_EAX = FUN_100098d0(param_1,&DAT_10027e04), in_EAX == 0)) ||
     (in_EAX = FUN_100098d0(param_1,&DAT_10027e00), in_EAX == 0)) {
    return in_EAX & 0xffffff00;
  }
  iVar1 = FUN_10009820((char *)param_1);
  if ((param_1[iVar1 + -1] == 0x5c) || (param_1[iVar1 + -1] == 0x2f)) {
    iVar1 = iVar1 + -1;
  }
  do {
    if (iVar1 < 1) {
      if (iVar1 == 0) {
        sVar2 = FUN_10009c50((char *)param_1,param_2,".%c");
        return CONCAT31((int3)(sVar2 >> 8),1);
      }
LAB_10009e91:
      return CONCAT31((int3)((uint)iVar1 >> 8),1);
    }
    if ((param_1[iVar1 + -1] == 0x5c) || (param_1[iVar1 + -1] == 0x2f)) {
      param_1[iVar1] = 0;
      iVar1 = FUN_10009b80((char *)param_1,'\\');
      goto LAB_10009e91;
    }
    iVar1 = iVar1 + -1;
  } while( true );
}


// 10009ea0 FUN_10009ea0

void __thiscall FUN_10009ea0(void *this,undefined4 param_1,undefined4 param_2)

{
  *(undefined4 *)((int)this + 4) = param_2;
  *(undefined4 *)this = param_1;
  *(void **)((int)this + 8) = DAT_100380f0;
  DAT_100380f0 = this;
  return;
}


// 10009ec0 FUN_10009ec0

undefined4 __cdecl FUN_10009ec0(byte *param_1,undefined4 *param_2)

{
  int *piVar1;
  byte bVar2;
  byte *pbVar3;
  int iVar4;
  undefined4 uVar5;
  byte *pbVar6;
  undefined4 *puVar7;
  bool bVar8;
  
  puVar7 = DAT_100380f0;
  if (DAT_100380f0 != (undefined4 *)0x0) {
    do {
      pbVar3 = (byte *)puVar7[1];
      pbVar6 = param_1;
      do {
        bVar2 = *pbVar3;
        bVar8 = bVar2 < *pbVar6;
        if (bVar2 != *pbVar6) {
LAB_10009ef5:
          iVar4 = (1 - (uint)bVar8) - (uint)(bVar8 != 0);
          goto LAB_10009efa;
        }
        if (bVar2 == 0) break;
        bVar2 = pbVar3[1];
        bVar8 = bVar2 < pbVar6[1];
        if (bVar2 != pbVar6[1]) goto LAB_10009ef5;
        pbVar3 = pbVar3 + 2;
        pbVar6 = pbVar6 + 2;
      } while (bVar2 != 0);
      iVar4 = 0;
LAB_10009efa:
      if (iVar4 == 0) {
        if (param_2 != (undefined4 *)0x0) {
          *param_2 = 0;
        }
                    /* WARNING: Could not recover jumptable at 0x10009f2a. Too many branches */
                    /* WARNING: Treating indirect jump as call */
        uVar5 = (*(code *)*puVar7)();
        return uVar5;
      }
      piVar1 = puVar7 + 2;
      puVar7 = (undefined4 *)*piVar1;
    } while ((undefined4 *)*piVar1 != (undefined4 *)0x0);
  }
  if (param_2 != (undefined4 *)0x0) {
    *param_2 = 1;
  }
  return 0;
}


// 10009f40 FUN_10009f40

undefined * FUN_10009f40(void)

{
  return FUN_10009ec0;
}


// 10009f50 FUN_10009f50

void __cdecl FUN_10009f50(undefined4 param_1,undefined **param_2)

{
  int *piVar1;
  int *piVar2;
  
  if ((DAT_1003969c != (int *)0x0) && (DAT_10038204 == '\0')) {
    DAT_10038200 = param_1;
    DAT_10038204 = '\x01';
    DAT_10035124 = (**(code **)(*DAT_1003969c + 0x14))();
    piVar2 = DAT_100381f8;
    DAT_100381fc = param_2;
    if (param_2 == (undefined **)0x0) {
      DAT_100381fc = &PTR_PTR_10035130;
    }
    while (piVar2 != (int *)0x0) {
      piVar1 = (int *)piVar2[1];
      (**(code **)(*piVar2 + 0xc))(DAT_10038200);
      (**(code **)(*piVar2 + 0x2c))();
      piVar2 = piVar1;
    }
    (**(code **)(*DAT_1003969c + 0x70))();
    (**(code **)(*DAT_1003969c + 0x94))();
    DAT_100381f8 = (int *)0x0;
  }
  return;
}


// 1000a000 FUN_1000a000

void FUN_1000a000(void)

{
  if ((DAT_1003969c != (int *)0x0) && (DAT_10038204 != '\0')) {
    (**(code **)(*DAT_1003969c + 0x74))(DAT_10035124);
    (**(code **)(*DAT_1003969c + 0x20))(DAT_10035124);
    DAT_10035124 = 0xffffffff;
    DAT_10038204 = '\0';
  }
  return;
}


// 1000a0c0 FUN_1000a0c0

void __fastcall FUN_1000a0c0(undefined4 param_1)

{
  if (DAT_100381fc != (undefined4 *)0x0) {
    (**(code **)*DAT_100381fc)(param_1);
  }
  return;
}


// 1000a140 FUN_1000a140

int * __thiscall
FUN_1000a140(void *this,int param_1,int param_2,undefined *param_3,uint param_4,undefined *param_5)

{
  code *pcVar1;
  
  *(byte *)((int)this + 0x20) = *(byte *)((int)this + 0x20) | 2;
  *(int *)((int)this + 0x18) = param_2;
  *(undefined1 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)this = &PTR_LAB_10027e44;
  pcVar1 = (code *)param_5;
  if (param_5 == (undefined *)0x0) {
    pcVar1 = FUN_10004c30;
  }
  *(code **)((int)this + 0x1c) = pcVar1;
  *(byte *)((int)this + 0x20) = param_5 != (undefined *)0x0 | *(byte *)((int)this + 0x20) & 0xfa;
  *(undefined1 *)((int)this + 8) = 0;
  *(int *)((int)this + 0xc) = param_1;
  if (param_3 == (undefined *)0x0) {
    param_3 = PTR_DAT_1003517c;
  }
  *(undefined **)((int)this + 0x10) = param_3;
  *(uint *)((int)this + 0x14) = param_4;
  if ((param_4 & 1) == 0) {
    *(void **)((int)this + 4) = DAT_100381f8;
    DAT_100381f8 = this;
  }
  else {
    *(undefined4 *)((int)this + 4) = 0;
  }
  if (DAT_100381fc != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x2c))();
  }
  return (int *)this;
}


// 1000a240 FUN_1000a240

void __fastcall FUN_1000a240(undefined4 *param_1)

{
  *param_1 = &PTR_FUN_10027ec4;
  param_1[6] = &PTR_LAB_10027ea4;
  if (param_1[9] != 0) {
    FUN_100012d0(param_1[9]);
    param_1[9] = 0;
  }
  *param_1 = &PTR_LAB_10027e0c;
  return;
}


// 1000a270 FUN_1000a270

undefined4 __thiscall FUN_1000a270(void *this,uint param_1)

{
  return CONCAT31((int3)((uint)*(int *)((int)this + 0x1c) >> 8),
                  (*(uint *)(*(int *)((int)this + 0x1c) + 0x14) & param_1) != 0);
}


// 1000a2c0 FUN_1000a2c0

undefined4 __fastcall FUN_1000a2c0(int param_1)

{
  return *(undefined4 *)(*(int *)(param_1 + 0x1c) + 0xc);
}


// 1000a2e0 FUN_1000a2e0

void __fastcall FUN_1000a2e0(int *param_1)

{
  char cVar1;
  undefined4 extraout_ECX;
  undefined4 extraout_ECX_00;
  undefined4 uVar2;
  undefined4 extraout_EDX;
  undefined4 extraout_EDX_00;
  undefined4 uVar3;
  int *piVar4;
  ulonglong uVar5;
  double dVar6;
  int *unaff_retaddr;
  float fVar7;
  int aiStack_28 [10];
  
  cVar1 = (**(code **)(*param_1 + 8))();
  if ((cVar1 != '\0') && (DAT_1003969c != (int *)0x0)) {
    cVar1 = (**(code **)(*DAT_1003969c + 0x80))();
    if (cVar1 == '\0') {
      (**(code **)(*DAT_1003969c + 0x8c))(param_1,unaff_retaddr);
      return;
    }
  }
  aiStack_28[0] = param_1[0xb];
  piVar4 = unaff_retaddr;
  if (unaff_retaddr == (int *)0x0) {
    piVar4 = (int *)&DAT_1002536c;
  }
  fVar7 = 0.0;
  if (unaff_retaddr != (int *)0x0) {
    dVar6 = _atof((char *)unaff_retaddr);
    fVar7 = (float)dVar6;
  }
  cVar1 = (**(code **)(*param_1 + 0x3c))(&stack0xffffffd4);
  uVar2 = extraout_ECX;
  uVar3 = extraout_EDX;
  if (cVar1 != '\0') {
    FUN_10009c50((char *)aiStack_28,0x20,"%f");
    piVar4 = aiStack_28;
    uVar2 = extraout_ECX_00;
    uVar3 = extraout_EDX_00;
  }
  param_1[0xb] = 0xb00000;
  uVar5 = FUN_1000ffe0(uVar2,uVar3);
  param_1[0xc] = (int)uVar5;
  if ((param_1[5] & 0x1000U) == 0) {
    (**(code **)(*param_1 + 0x40))(piVar4,fVar7);
  }
  return;
}


// 1000a3c0 FUN_1000a3c0

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

void __thiscall FUN_1000a3c0(void *this,undefined4 param_1,undefined4 param_2)

{
  code *pcVar1;
  int iVar2;
  int iVar3;
  undefined4 uVar4;
  undefined4 extraout_ECX;
  undefined1 *puVar5;
  undefined1 *puVar6;
  undefined4 auStack_24 [2];
  size_t asStack_1c [3];
  
  asStack_1c[2] = 0x1000a3d6;
  iVar2 = -(*(int *)((int)this + 0x28) + 0xfU & 0xfffffff0);
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x28);
  *(undefined4 *)((int)asStack_1c + iVar2 + 4) = uVar4;
  *(undefined1 **)((int)asStack_1c + iVar2) = &stack0xfffffff0 + iVar2;
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = 0x1000a3e3;
  _memcpy(*(void **)((int)asStack_1c + iVar2),*(void **)((int)asStack_1c + iVar2 + 4),
          *(size_t *)((int)asStack_1c + iVar2 + 8));
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = param_1;
  *(undefined4 *)((int)auStack_24 + iVar2) = 0x1000a3ec;
  iVar3 = FUN_10009820(*(char **)((int)auStack_24 + iVar2 + 4));
  iVar3 = iVar3 + 1;
  puVar6 = &stack0xfffffff0 + iVar2;
  if (*(int *)((int)this + 0x28) < iVar3) {
    if (*(int *)((int)this + 0x24) != 0) {
      *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x24);
      *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x1000a406;
      FUN_100012d0(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    }
    puVar6 = &stack0xfffffff0 + iVar2;
    *(int *)((int)asStack_1c + iVar2 + 8) = iVar3;
    *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x1000a40f;
    uVar4 = FUN_10001270(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    *(undefined4 *)((int)this + 0x24) = uVar4;
    *(int *)((int)this + 0x28) = iVar3;
  }
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)(puVar6 + -4) = iVar3;
  *(undefined4 *)(puVar6 + -8) = param_1;
  *(undefined4 *)(puVar6 + -0xc) = uVar4;
  *(undefined4 *)(puVar6 + -0x10) = 0x1000a426;
  _memcpy(*(void **)(puVar6 + -0xc),*(void **)(puVar6 + -8),*(size_t *)(puVar6 + -4));
  pcVar1 = *(code **)((int)this + 0x44);
  if (pcVar1 != (code *)0x0) {
    *(undefined4 *)(puVar6 + -4) = extraout_ECX;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(int *)(puVar6 + -0xc) = (int)this + 0x18;
    puVar5 = puVar6 + -0x10;
    *(undefined4 *)(puVar6 + -0x10) = 0x1000a43e;
    (*pcVar1)();
    puVar6 = puVar5 + 0xc;
  }
  if (DAT_1003969c != (int *)0x0) {
    pcVar1 = *(code **)(*DAT_1003969c + 0x48);
    *(int **)(puVar6 + -4) = DAT_1003969c;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(void **)(puVar6 + -0xc) = this;
    *(undefined4 *)(puVar6 + -0x10) = 0x1000a45b;
    (*pcVar1)();
  }
  return;
}


// 1000a4c0 FUN_1000a4c0

void __thiscall FUN_1000a4c0(void *this,float param_1)

{
  char cVar1;
  undefined4 extraout_ECX;
  undefined4 extraout_EDX;
  ulonglong uVar2;
  undefined4 uVar3;
  undefined4 uStack_4;
  
  if (*(float *)((int)this + 0x2c) != param_1) {
                    /* WARNING: Load size is inaccurate */
    cVar1 = (**(code **)(*this + 8))(0xb00000);
    if (((cVar1 != '\0') && (DAT_1003969c != (int *)0x0)) &&
       (cVar1 = (**(code **)(*DAT_1003969c + 0x80))(), cVar1 == '\0')) {
      (**(code **)(*DAT_1003969c + 0x84))();
      return;
    }
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x3c))(&stack0x00000000);
    uVar3 = *(undefined4 *)((int)this + 0x2c);
    *(undefined4 *)((int)this + 0x2c) = uStack_4;
    uVar2 = FUN_1000ffe0(extraout_ECX,extraout_EDX);
    *(int *)((int)this + 0x30) = (int)uVar2;
    if ((*(uint *)((int)this + 0x14) & 0x1000) == 0) {
      FUN_10009c50(&stack0xffffffd8,0x20,"%f");
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x40))(&stack0xffffffd8,uVar3);
      return;
    }
  }
  return;
}


// 1000a660 FUN_1000a660

void __thiscall
FUN_1000a660(void *this,int param_1,char *param_2,uint param_3,undefined *param_4,undefined1 param_5
            ,int param_6,undefined1 param_7,int param_8,int param_9)

{
  char *pcVar1;
  char cVar2;
  void *_Dst;
  undefined4 extraout_ECX;
  undefined4 extraout_EDX;
  ulonglong uVar3;
  double dVar4;
  
  *(void **)((int)this + 0x1c) = this;
  if (param_2 == (char *)0x0) {
    param_2 = PTR_DAT_100351cc;
  }
  *(int *)((int)this + 0x38) = param_6;
  *(undefined1 *)((int)this + 0x34) = param_5;
  *(int *)((int)this + 0x40) = param_8;
  *(undefined1 *)((int)this + 0x3c) = param_7;
  *(char **)((int)this + 0x20) = param_2;
  *(int *)((int)this + 0x44) = param_9;
  pcVar1 = param_2 + 1;
  do {
    cVar2 = *param_2;
    param_2 = param_2 + 1;
  } while (cVar2 != '\0');
  *(char **)((int)this + 0x28) = param_2 + (1 - (int)pcVar1);
  _Dst = (void *)FUN_10001270(param_2 + (1 - (int)pcVar1));
  *(void **)((int)this + 0x24) = _Dst;
  _memcpy(_Dst,*(void **)((int)this + 0x20),*(size_t *)((int)this + 0x28));
  dVar4 = _atof(*(char **)((int)this + 0x24));
  *(float *)((int)this + 0x2c) = (float)dVar4;
  uVar3 = FUN_1000ffe0(extraout_ECX,extraout_EDX);
  *(int *)((int)this + 0x30) = (int)uVar3;
  if (((param_3 & 0x84200) == 0) && (-1 < (char)param_3)) {
    param_3 = param_3 | 2;
  }
  *(int *)((int)this + 0xc) = param_1;
  *(undefined1 *)((int)this + 8) = 0;
  if (param_4 == (undefined *)0x0) {
    param_4 = PTR_DAT_1003517c;
  }
  *(undefined **)((int)this + 0x10) = param_4;
  *(uint *)((int)this + 0x14) = param_3;
  if ((param_3 & 1) == 0) {
    *(void **)((int)this + 4) = DAT_100381f8;
    DAT_100381f8 = this;
  }
  else {
    *(undefined4 *)((int)this + 4) = 0;
  }
  if (DAT_100381fc != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x2c))();
  }
  return;
}


// 1000a790 FUN_1000a790

bool __fastcall FUN_1000a790(undefined4 *param_1)

{
  return (undefined *)*param_1 != &DAT_10038228;
}


// 1000a800 FUN_1000a800

undefined4 * __thiscall FUN_1000a800(void *this,int param_1,char *param_2,uint param_3)

{
  *(undefined1 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_10027e84;
  *(undefined ***)this = &PTR_FUN_10027ec4;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_10027ea4;
  FUN_1000a660(this,param_1,param_2,param_3,(undefined *)0x0,0,0,0,0,0);
  return (undefined4 *)this;
}


// 1000a860 FUN_1000a860

undefined4 * __thiscall FUN_1000a860(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_10027ec4;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_10027ea4;
  if (*(int *)((int)this + 0x24) != 0) {
    FUN_100012d0(*(int *)((int)this + 0x24));
    *(undefined4 *)((int)this + 0x24) = 0;
  }
  *(undefined ***)this = &PTR_LAB_10027e0c;
  if ((param_1 & 1) != 0) {
    FUN_100012d0(this);
  }
  return (undefined4 *)this;
}


// 1000a8b0 FUN_1000a8b0

undefined4 * __thiscall
FUN_1000a8b0(void *this,int param_1,char *param_2,uint param_3,undefined *param_4)

{
  *(undefined1 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_10027e84;
  *(undefined ***)this = &PTR_FUN_10027ec4;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_10027ea4;
  FUN_1000a660(this,param_1,param_2,param_3,param_4,0,0,0,0,0);
  return (undefined4 *)this;
}


// 1000a910 FUN_1000a910

int * __fastcall FUN_1000a910(int *param_1)

{
  undefined *puVar1;
  char *pcVar2;
  char *pcVar3;
  void *_Dst;
  undefined4 extraout_ECX;
  undefined4 extraout_EDX;
  ulonglong uVar4;
  double dVar5;
  
  *(undefined1 *)(param_1 + 2) = 0;
  param_1[3] = 0;
  param_1[4] = 0;
  param_1[5] = 0;
  param_1[1] = 0;
  param_1[6] = (int)&PTR_LAB_10027e84;
  param_1[0xe] = 0;
  param_1[0x10] = 0;
  *param_1 = (int)&PTR_FUN_10027ec4;
  param_1[6] = (int)&PTR_LAB_10027ea4;
  param_1[7] = (int)param_1;
  param_1[8] = (int)&DAT_10027f84;
  *(undefined1 *)(param_1 + 0xd) = 0;
  *(undefined1 *)(param_1 + 0xf) = 0;
  param_1[0x11] = 0;
  pcVar2 = "0";
  do {
    pcVar3 = pcVar2;
    pcVar2 = pcVar3 + 1;
  } while (*pcVar3 != '\0');
  param_1[10] = (int)(pcVar3 + -0x10027f83);
  _Dst = (void *)FUN_10001270(pcVar3 + -0x10027f83);
  param_1[9] = (int)_Dst;
  _memcpy(_Dst,(void *)param_1[8],param_1[10]);
  dVar5 = _atof((char *)param_1[9]);
  param_1[0xb] = (int)(float)dVar5;
  uVar4 = FUN_1000ffe0(extraout_ECX,extraout_EDX);
  puVar1 = PTR_DAT_1003517c;
  param_1[0xc] = (int)uVar4;
  *(undefined1 *)(param_1 + 2) = 0;
  param_1[3] = (int)&DAT_1002536c;
  param_1[4] = (int)puVar1;
  param_1[5] = 2;
  param_1[1] = (int)DAT_100381f8;
  DAT_100381f8 = param_1;
  if (DAT_100381fc != 0) {
    (**(code **)(*param_1 + 0x2c))();
  }
  *param_1 = (int)&PTR_FUN_10027f3c;
  param_1[6] = (int)&PTR_LAB_10027f18;
  return param_1;
}


// 1000aa30 FUN_1000aa30

void __thiscall FUN_1000aa30(void *this,undefined4 param_1,char param_2)

{
  int iVar1;
  undefined *puVar2;
  
  if (DAT_1003969c == (int *)0x0) {
    puVar2 = &DAT_10038228;
  }
  else {
    iVar1 = (**(code **)(*DAT_1003969c + 0x34))(param_1);
    if (iVar1 == 0) {
      puVar2 = (undefined *)0x0;
    }
    else {
      puVar2 = (undefined *)(iVar1 + 0x18);
    }
  }
  *(undefined **)this = puVar2;
  if (puVar2 == (undefined *)0x0) {
    *(undefined **)this = &DAT_10038228;
  }
                    /* WARNING: Load size is inaccurate */
  *(undefined **)((int)this + 4) = *this + -0x18;
  if ((*this == &DAT_10038228) && ((DAT_1003969c != (int *)0x0 || (DAT_1003520b != '\0')))) {
    if (param_2 == '\0') {
      Warning("ConVarRef %s doesn\'t point to an existing ConVar\n",param_1);
    }
    DAT_1003520b = '\0';
  }
  return;
}


// 1000aab0 FUN_1000aab0

void __thiscall FUN_1000aab0(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  undefined4 uVar3;
  uint uVar4;
  
  iVar1 = *(int *)((int)this + 8);
  if (iVar1 < 0) {
    return;
  }
  iVar2 = *(int *)((int)this + 4);
  uVar4 = param_1 + iVar2;
  if (iVar1 == 0) {
    if (iVar2 == 0) {
      iVar2 = 2;
    }
    if ((int)uVar4 <= iVar2) goto LAB_1000ab0b;
    do {
      iVar2 = iVar2 * 2;
    } while (iVar2 < (int)uVar4);
  }
  else {
    iVar2 = ((int)(uVar4 - 1) / iVar1 + 1) * iVar1;
  }
  if (iVar2 < (int)uVar4) {
    if ((iVar2 == 0) && (0x7fffffff < uVar4)) {
      iVar2 = -1;
    }
    else {
      do {
        iVar2 = (int)(iVar2 + uVar4) / 2;
      } while (iVar2 < (int)uVar4);
    }
  }
LAB_1000ab0b:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 << 4);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 1000ab40 FUN_1000ab40

void * __thiscall FUN_1000ab40(void *this,undefined4 param_1)

{
  FUN_1000aa30(this,param_1,'\0');
  return this;
}


// 1000ab60 FUN_1000ab60

void * __thiscall FUN_1000ab60(void *this,undefined4 param_1,char param_2)

{
  FUN_1000aa30(this,param_1,param_2);
  return this;
}


// 1000ab80 FUN_1000ab80

int __thiscall FUN_1000ab80(void *this,int param_1,int *param_2)

{
  int iVar1;
  void *pvVar2;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_1000aab0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar2 = (void *)(*this + param_1 * 0x10);
    FUN_10009be0((void *)((int)pvVar2 + 0x10),pvVar2,iVar1 * 0x10);
  }
                    /* WARNING: Load size is inaccurate */
  pvVar2 = (void *)(param_1 * 0x10 + *this);
  if (pvVar2 != (void *)0x0) {
    FUN_1000c710(pvVar2,param_2);
  }
  return param_1;
}


// 1000abf0 FUN_1000abf0

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

int __thiscall FUN_1000abf0(void *this,undefined4 param_1,void *param_2)

{
  int iVar1;
  char *pcVar2;
  int iVar3;
  int aiStack_1010 [4];
  char local_1000 [4092];
  undefined4 uStack_4;
  
  uStack_4 = 0x1000abfa;
  if ((*(byte *)((int)this + 0x20) & 4) == 0) {
    if (*(code **)((int)this + 0x1c) != (code *)0x0) {
      iVar1 = (**(code **)((int)this + 0x1c))(param_1,local_1000);
      if (0 < iVar1) {
        pcVar2 = local_1000;
        iVar3 = iVar1;
        do {
          FUN_1000c6d0(aiStack_1010,pcVar2);
          FUN_1000ab80(param_2,*(int *)((int)param_2 + 0xc),aiStack_1010);
          if (-1 < aiStack_1010[2]) {
            if (aiStack_1010[0] != 0) {
              (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(aiStack_1010[0]);
              aiStack_1010[0] = 0;
            }
            aiStack_1010[1] = 0;
          }
          pcVar2 = pcVar2 + 0x40;
          iVar3 = iVar3 + -1;
        } while (iVar3 != 0);
      }
      return iVar1;
    }
  }
  else if (*(int *)((int)this + 0x1c) != 0) {
    iVar1 = (**(code **)**(undefined4 **)((int)this + 0x1c))(param_1,param_2);
    return iVar1;
  }
  return 0;
}


// 1000acd0 FUN_1000acd0

undefined4 __thiscall FUN_1000acd0(void *this,int *param_1,char param_2)

{
  char *pcVar1;
  int iVar2;
  
                    /* WARNING: Load size is inaccurate */
  iVar2 = *this;
  if ((*param_1 == iVar2) &&
     (((iVar2 == 1 || (iVar2 == 2)) ||
      (((iVar2 == 3 &&
        (((param_2 != '\0' || (*(short *)((int)this + 8) == (short)param_1[2])) &&
         ((char)param_1[1] == *(char *)((int)this + 4))))) &&
       (((*(char *)((int)param_1 + 5) == *(char *)((int)this + 5) &&
         (*(char *)((int)param_1 + 6) == *(char *)((int)this + 6))) &&
        (pcVar1 = (char *)((int)param_1 + 7),
        param_1 = (int *)CONCAT31((int3)((uint)param_1 >> 8),*pcVar1),
        *pcVar1 == *(char *)((int)this + 7))))))))) {
    return CONCAT31((int3)((uint)param_1 >> 8),1);
  }
  return (uint)param_1 & 0xffffff00;
}


// 1000ad30 FUN_1000ad30

char * __thiscall FUN_1000ad30(void *this,char param_1)

{
  char *pcVar1;
  int iVar2;
  uint uVar3;
  
  uVar3 = DAT_10038358 & 0x80000003;
  if ((int)uVar3 < 0) {
    uVar3 = (uVar3 - 1 | 0xfffffffc) + 1;
  }
  pcVar1 = &DAT_10038258 + uVar3 * 0x40;
  DAT_10038358 = DAT_10038358 + 1;
  FUN_10009c10(pcVar1,"unknown",0x40);
                    /* WARNING: Load size is inaccurate */
  iVar2 = *this;
  if (iVar2 == 1) {
    FUN_10009c10(pcVar1,"loopback",0x40);
    return pcVar1;
  }
  if (iVar2 == 2) {
    FUN_10009c10(pcVar1,"broadcast",0x40);
    return pcVar1;
  }
  if (iVar2 == 3) {
    if (param_1 != '\0') {
      FUN_10009c50(pcVar1,0x40,"%i.%i.%i.%i");
      return pcVar1;
    }
    ntohs(*(u_short *)((int)this + 8));
    FUN_10009c50(pcVar1,0x40,"%i.%i.%i.%i:%i");
  }
  return pcVar1;
}


// 1000ae20 FUN_1000ae20

void __thiscall FUN_1000ae20(void *this,undefined4 param_1)

{
  *(undefined4 *)this = param_1;
  return;
}


// 1000ae30 FUN_1000ae30

undefined4 __fastcall FUN_1000ae30(undefined4 *param_1)

{
  return *param_1;
}


// 1000ae40 FUN_1000ae40

undefined4 __fastcall FUN_1000ae40(int param_1)

{
  return *(undefined4 *)(param_1 + 4);
}


// 1000ae50 FUN_1000ae50

void __thiscall FUN_1000ae50(void *this,uint param_1)

{
  *(uint *)((int)this + 4) =
       param_1 >> 0x18 | (param_1 & 0xff0000) >> 8 | (param_1 & 0xff00) << 8 | param_1 << 0x18;
  return;
}


// 1000ae60 FUN_1000ae60

undefined2 __fastcall FUN_1000ae60(int param_1)

{
  return CONCAT11((char)*(undefined2 *)(param_1 + 8),
                  (char)((ushort)*(undefined2 *)(param_1 + 8) >> 8));
}


// 1000ae70 FUN_1000ae70

void __thiscall FUN_1000ae70(void *this,undefined2 param_1)

{
  *(ushort *)((int)this + 8) = CONCAT11((char)param_1,(char)((ushort)param_1 >> 8));
  return;
}


// 1000ae80 FUN_1000ae80

void __thiscall FUN_1000ae80(void *this,char *param_1,char param_2)

{
  char *pcVar1;
  int iVar2;
  hostent *phVar3;
  u_long uVar4;
  long lVar5;
  undefined1 local_114 [4];
  undefined1 local_110 [4];
  undefined1 local_10c [4];
  undefined2 local_108 [2];
  undefined1 local_104 [4];
  char local_100 [256];
  
  *(undefined1 *)((int)this + 7) = 0;
  *(undefined1 *)((int)this + 6) = 0;
  *(undefined1 *)((int)this + 5) = 0;
  *(undefined1 *)((int)this + 4) = 0;
  *(undefined2 *)((int)this + 8) = 0;
  *(undefined4 *)this = 3;
  if (param_1 != (char *)0x0) {
    if ((('/' < *param_1) && (*param_1 < ':')) &&
       (pcVar1 = _strchr(param_1,0x2e), pcVar1 != (char *)0x0)) {
      iVar2 = FID_conflict__sscanf
                        (param_1,"%d.%d.%d.%d:%d",local_104,local_10c,local_114,local_110,local_108)
      ;
      if (3 < iVar2) {
        *(undefined1 *)((int)this + 4) = local_104[0];
        *(undefined1 *)((int)this + 5) = local_10c[0];
        *(undefined1 *)((int)this + 6) = local_114[0];
        *(undefined1 *)((int)this + 7) = local_110[0];
      }
      if (iVar2 != 5) {
        return;
      }
      FUN_1000ae70(this,local_108[0]);
      return;
    }
    if (param_2 != '\0') {
      FUN_10009c10(local_100,param_1,0x100);
      pcVar1 = _strchr(local_100,0x3a);
      if (pcVar1 != (char *)0x0) {
        *pcVar1 = '\0';
      }
      phVar3 = gethostbyname(local_100);
      if (phVar3 != (hostent *)0x0) {
        uVar4 = ntohl(*(u_long *)*phVar3->h_addr_list);
        FUN_1000ae50(this,uVar4);
        if (pcVar1 != (char *)0x0) {
          lVar5 = _atol(pcVar1 + 1);
          FUN_1000ae70(this,(short)lVar5);
        }
      }
    }
  }
  return;
}


// 1000afc0 FUN_1000afc0

void __cdecl FUN_1000afc0(undefined4 *param_1)

{
  *param_1 = 0xffffffff;
  return;
}


// 1000afd0 FUN_1000afd0

void __cdecl FUN_1000afd0(uint *param_1,uint *param_2,uint param_3)

{
  uint uVar1;
  uint uVar2;
  int iVar3;
  
  uVar1 = *param_1;
  do {
    switch(param_3) {
    case 3:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 2:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 1:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
    case 0:
      *param_1 = uVar1;
      return;
    case 7:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 6:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 5:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 4:
      uVar1 = (uVar1 ^ *param_2) >> 8 ^ *(uint *)(&DAT_10028000 + ((uVar1 ^ *param_2) & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4);
      *param_1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4);
      return;
    }
    uVar2 = (uint)param_2 & 3;
    if (uVar2 == 1) {
LAB_1000b02d:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    }
    else {
      if (uVar2 == 2) {
LAB_1000b018:
        uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
        param_2 = (uint *)((int)param_2 + 1);
        goto LAB_1000b02d;
      }
      if (uVar2 == 3) {
        uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
        param_2 = (uint *)((int)param_2 + 1);
        goto LAB_1000b018;
      }
    }
    for (iVar3 = (int)(param_3 - uVar2) >> 3; iVar3 != 0; iVar3 = iVar3 + -1) {
      uVar1 = (uVar1 ^ *param_2) >> 8 ^ *(uint *)(&DAT_10028000 + ((uVar1 ^ *param_2) & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4);
      uVar1 = *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4) ^ param_2[1] ^ uVar1 >> 8;
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_10028000 + (uVar1 & 0xff) * 4);
      param_2 = param_2 + 2;
    }
    param_3 = param_3 - uVar2 & 7;
  } while( true );
}


// 1000b1b0 FUN_1000b1b0

uint __fastcall FUN_1000b1b0(byte *param_1)

{
  byte *pbVar1;
  byte bVar2;
  uint uVar3;
  uint uVar4;
  
  uVar3 = 0;
  uVar4 = 0;
  bVar2 = *param_1;
  while (bVar2 != 0) {
    uVar3 = *(uint *)(&DAT_10035210 + (bVar2 ^ uVar4) * 4);
    pbVar1 = param_1 + 1;
    param_1 = param_1 + 2;
    if (*pbVar1 == 0) break;
    uVar4 = *(uint *)(&DAT_10035210 + (*pbVar1 ^ uVar3) * 4);
    bVar2 = *param_1;
  }
  return uVar3 << 8 | uVar4;
}


// 1000b1f0 FUN_1000b1f0

uint __fastcall FUN_1000b1f0(byte *param_1)

{
  uint uVar1;
  uint uVar2;
  uint uVar3;
  
  uVar3 = 0;
  uVar2 = 0;
  uVar1 = _toupper((uint)*param_1);
  while (uVar1 != 0) {
    uVar3 = *(uint *)(&DAT_10035210 + (uVar1 ^ uVar2) * 4);
    uVar1 = _toupper((uint)param_1[1]);
    param_1 = param_1 + 2;
    if (uVar1 == 0) break;
    uVar2 = *(uint *)(&DAT_10035210 + (uVar1 ^ uVar3) * 4);
    uVar1 = _toupper((uint)*param_1);
  }
  return uVar3 << 8 | uVar2;
}


// 1000b260 FUN_1000b260

undefined * FUN_1000b260(void)

{
  return &DAT_10038368;
}


// 1000b270 FUN_1000b270

undefined * FUN_1000b270(void)

{
  return &DAT_10038d78;
}


// 1000b2a0 FUN_1000b2a0

undefined4 * __thiscall
FUN_1000b2a0(void *this,undefined1 param_1,char *param_2,int param_3,int param_4)

{
  int *piVar1;
  char cVar2;
  char *pcVar3;
  int iVar4;
  int iVar5;
  
  *(undefined1 *)((int)this + 4) = param_1;
  *(undefined ***)this = &PTR_FUN_1002842c;
  *(char **)((int)this + 8) = param_2;
  *(int *)((int)this + 0x10) = param_3;
  iVar4 = FUN_10009820(param_2);
  iVar5 = 0;
  *(int *)((int)this + 0xc) = iVar4;
  *(undefined4 *)((int)this + 0x14) = 0;
  _memset((void *)((int)this + 0x118),0,0x7f8);
  if (0 < param_3) {
    do {
      cVar2 = *(char *)(param_4 + iVar5 * 8);
      *(char *)((int)this + iVar5 + 0x18) = cVar2;
      pcVar3 = *(char **)(param_4 + 4 + iVar5 * 8);
      piVar1 = (int *)((int)this + cVar2 * 8 + 0x118);
      piVar1[1] = (int)pcVar3;
      iVar4 = FUN_10009820(pcVar3);
      *piVar1 = iVar4;
      if (*(int *)((int)this + 0x14) < iVar4) {
        *(int *)((int)this + 0x14) = iVar4;
      }
      iVar5 = iVar5 + 1;
    } while (iVar5 < param_3);
  }
  return (undefined4 *)this;
}


// 1000b330 FUN_1000b330

uint __thiscall FUN_1000b330(void *this,byte *param_1,undefined4 *param_2)

{
  undefined4 uVar1;
  uint in_EAX;
  int iVar2;
  
  iVar2 = 0;
  if (0 < *(int *)((int)this + 0x10)) {
    do {
      in_EAX = FUN_10009870(param_1,*(byte **)((int)this +
                                              *(char *)((int)this + iVar2 + 0x18) * 8 + 0x11c));
      if (in_EAX == 0) {
        uVar1 = *(undefined4 *)((int)this + *(char *)(iVar2 + 0x18 + (int)this) * 8 + 0x118);
        *param_2 = uVar1;
        return CONCAT31((int3)((uint)uVar1 >> 8),*(undefined1 *)(iVar2 + 0x18 + (int)this));
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < *(int *)((int)this + 0x10));
  }
  *param_2 = 0;
  return in_EAX & 0xffffff00;
}


// 1000b390 FUN_1000b390

void __thiscall FUN_1000b390(void *this,char param_1)

{
  *(uint *)((int)this + 0x2c) =
       (uint)(param_1 != '\0') | (uint)(param_1 != '\0') * 2 |
       *(uint *)((int)this + 0x2c) & 0xfffffffc;
  return;
}


// 1000b3d0 FUN_1000b3d0

undefined1 __thiscall FUN_1000b3d0(void *this,int param_1)

{
  int iVar1;
  char cVar2;
  
  if ((*(byte *)((int)this + 0x14) & 2) == 0) {
    iVar1 = *(int *)((int)this + 0xc);
    if (iVar1 + param_1 <= *(int *)((int)this + 0x1c)) {
      if ((iVar1 < *(int *)((int)this + 0x20)) ||
         (*(int *)((int)this + 4) < (iVar1 - *(int *)((int)this + 0x20)) + param_1)) {
        cVar2 = (**(code **)((int)this + 0x24))(param_1);
        if (cVar2 == '\0') {
          *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
          return 0;
        }
      }
      return 1;
    }
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
  }
  return 0;
}


// 1000b430 FUN_1000b430

uint __thiscall FUN_1000b430(void *this,int param_1,int *param_2)

{
  int iVar1;
  int iVar2;
  
  iVar2 = *(int *)((int)this + 0xc);
  iVar1 = *(int *)((int)this + 0x1c);
  if (iVar1 <= iVar2 + param_1) {
    *param_2 = 0;
    return (uint)param_2 & 0xffffff00;
  }
  if (iVar1 < *param_2 + iVar2 + param_1) {
    *param_2 = (iVar1 - iVar2) - param_1;
  }
  if ((*(byte *)((int)this + 0x14) & 2) == 0) {
    FUN_1000b3d0(this,*param_2 + param_1);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
  }
  iVar2 = *(int *)((int)this + 0x1c) - *(int *)((int)this + 0xc);
  if (iVar2 < *param_2) {
    *param_2 = iVar2;
  }
  return (uint)(*param_2 != 0);
}


// 1000b4a0 FUN_1000b4a0

int __thiscall FUN_1000b4a0(void *this,int param_1,int param_2)

{
  char cVar1;
  
  if ((*(byte *)((int)this + 0x14) & 2) == 0) {
    cVar1 = FUN_1000b3d0(this,param_1 + param_2);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
    if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
      return (*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this + param_2;
    }
  }
  return 0;
}


// 1000b4e0 FUN_1000b4e0

void __thiscall FUN_1000b4e0(void *this,int param_1,int param_2)

{
  int iVar1;
  
  if (param_1 != 0) {
    if (param_1 == 1) {
      *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
      goto LAB_1000b50c;
    }
    if (param_1 != 2) goto LAB_1000b50c;
    param_2 = *(int *)((int)this + 0x1c) - param_2;
  }
  *(int *)((int)this + 0xc) = param_2;
LAB_1000b50c:
  iVar1 = *(int *)((int)this + 0xc);
  if (iVar1 <= *(int *)((int)this + 0x1c)) {
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
    if ((iVar1 < *(int *)((int)this + 0x20)) ||
       (*(int *)((int)this + 4) + *(int *)((int)this + 0x20) <= iVar1)) {
      (**(code **)((int)this + 0x24))(0xffffffff);
    }
    return;
  }
  *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
  return;
}


// 1000b540 FUN_1000b540

uint __thiscall FUN_1000b540(void *this,int param_1)

{
  uint in_EAX;
  
  if (((*(byte *)((int)this + 0x14) & 1) == 0) && ((*(byte *)((int)this + 0x15) & 8) == 0)) {
    if ((*(int *)((int)this + 0x10) < *(int *)((int)this + 0x20)) ||
       (in_EAX = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1,
       *(int *)((int)this + 4) < (int)in_EAX)) {
      in_EAX = (**(code **)((int)this + 0x28))(param_1);
      if ((char)in_EAX == '\0') {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 1;
        goto LAB_1000b576;
      }
    }
    return CONCAT31((int3)(in_EAX >> 8),1);
  }
LAB_1000b576:
  return in_EAX & 0xffffff00;
}


// 1000b590 FUN_1000b590

void __fastcall FUN_1000b590(int *param_1)

{
  undefined4 uVar1;
  
  if (param_1[7] < param_1[4]) {
    if (((*(byte *)((int)param_1 + 0x15) & 8) == 0) && ((*(byte *)(param_1 + 5) & 1) == 0)) {
      uVar1 = FUN_1000b540(param_1,1);
      if ((char)uVar1 != '\0') {
        *(undefined1 *)((param_1[4] - param_1[8]) + *param_1) = 0;
        param_1[7] = param_1[4];
        return;
      }
      *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) & 0xfe;
    }
    param_1[7] = param_1[4];
  }
  return;
}


// 1000b5e0 FUN_1000b5e0

void __thiscall FUN_1000b5e0(void *this,undefined4 param_1)

{
  size_t _Size;
  void *_Dst;
  
  if (*(int *)((int)this + 8) < 0) {
    _Size = *(size_t *)((int)this + 4);
    *(undefined4 *)((int)this + 8) = param_1;
    if (_Size != 0) {
      _Dst = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(_Size);
                    /* WARNING: Load size is inaccurate */
      _memcpy(_Dst,*this,_Size);
      *(void **)this = _Dst;
      return;
    }
    *(undefined4 *)this = 0;
  }
  return;
}


// 1000b630 FUN_1000b630

void __thiscall FUN_1000b630(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  undefined4 uVar3;
  uint uVar4;
  
  iVar1 = *(int *)((int)this + 8);
  if (iVar1 < 0) {
    return;
  }
  iVar2 = *(int *)((int)this + 4);
  uVar4 = param_1 + iVar2;
  if (iVar1 == 0) {
    if (iVar2 == 0) {
      iVar2 = 0x20;
    }
    if ((int)uVar4 <= iVar2) goto LAB_1000b68b;
    do {
      iVar2 = iVar2 * 2;
    } while (iVar2 < (int)uVar4);
  }
  else {
    iVar2 = ((int)(uVar4 - 1) / iVar1 + 1) * iVar1;
  }
  if (iVar2 < (int)uVar4) {
    if ((iVar2 == 0) && (0x7fffffff < uVar4)) {
      iVar2 = -1;
    }
    else {
      do {
        iVar2 = (int)(iVar2 + uVar4) / 2;
      } while (iVar2 < (int)uVar4);
    }
  }
LAB_1000b68b:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 1000b6c0 FUN_1000b6c0

void __thiscall FUN_1000b6c0(void *this,int param_1)

{
  undefined4 uVar1;
  int iVar2;
  int iVar3;
  
  iVar2 = param_1 + 1;
  iVar3 = iVar2;
  if (((*(int *)((int)this + 8) < 0) && (iVar3 = param_1, (*(byte *)((int)this + 0x15) & 2) != 0))
     && (*(int *)((int)this + 4) < iVar2)) {
    FUN_1000b5e0(this,0);
    iVar3 = iVar2;
  }
  if ((*(int *)((int)this + 4) < iVar3) && (-1 < *(int *)((int)this + 8))) {
                    /* WARNING: Load size is inaccurate */
    *(int *)((int)this + 4) = iVar3;
    if (*this != 0) {
      uVar1 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
      *(undefined4 *)this = uVar1;
      return;
    }
    uVar1 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar3);
    *(undefined4 *)this = uVar1;
  }
  return;
}


// 1000b720 FUN_1000b720

void __thiscall FUN_1000b720(void *this,void *param_1,size_t param_2)

{
  char cVar1;
  
  cVar1 = FUN_1000b3d0(this,param_2);
  if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    _memcpy(param_1,(void *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this),
            param_2);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
  }
  return;
}


// 1000b760 FUN_1000b760

void __fastcall FUN_1000b760(int *param_1)

{
  char cVar1;
  int iVar2;
  
  if (((*(byte *)((int)param_1 + 0x15) & 1) != 0) && ((char)param_1[5] == '\0')) {
    while ((*(byte *)(param_1 + 5) & 2) == 0) {
      iVar2 = param_1[3];
      if ((param_1[7] < iVar2 + 1) ||
         (((iVar2 < param_1[8] || (param_1[1] < (iVar2 - param_1[8]) + 1)) &&
          (cVar1 = (*(code *)param_1[9])(1), cVar1 == '\0')))) {
        *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) | 2;
        return;
      }
      iVar2 = _isspace((uint)*(byte *)((param_1[3] - param_1[8]) + *param_1));
      if (iVar2 == 0) {
        return;
      }
      param_1[3] = param_1[3] + 1;
    }
  }
  return;
}


// 1000b7d0 FUN_1000b7d0

int __thiscall FUN_1000b7d0(void *this,int param_1)

{
  bool bVar1;
  char cVar2;
  int iVar3;
  
  if (((*(byte *)((int)this + 0x15) & 1) != 0) && (*(char *)((int)this + 0x14) == '\0')) {
    while( true ) {
      if ((*(byte *)((int)this + 0x14) & 2) != 0) {
        return param_1;
      }
      iVar3 = *(int *)((int)this + 0xc);
      if (*(int *)((int)this + 0x1c) < iVar3 + 1 + param_1) {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
        bVar1 = false;
      }
      else if (((iVar3 < *(int *)((int)this + 0x20)) ||
               (*(int *)((int)this + 4) < (iVar3 - *(int *)((int)this + 0x20)) + 1 + param_1)) &&
              (cVar2 = (**(code **)((int)this + 0x24))(param_1 + 1), cVar2 == '\0')) {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
        bVar1 = false;
      }
      else {
        bVar1 = true;
      }
      *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
      if (!bVar1) break;
                    /* WARNING: Load size is inaccurate */
      iVar3 = _isspace((uint)*(byte *)((*this - *(int *)((int)this + 0x20)) + param_1 +
                                      *(int *)((int)this + 0xc)));
      if (iVar3 == 0) {
        return param_1;
      }
      param_1 = param_1 + 1;
    }
    return param_1;
  }
  return 0;
}


// 1000b870 FUN_1000b870

int __fastcall FUN_1000b870(int *param_1)

{
  int iVar1;
  char cVar2;
  int iVar3;
  int iVar4;
  int iVar5;
  int iVar6;
  int local_8;
  
  if ((char)param_1[5] != '\0') {
    return 0;
  }
  local_8 = 0;
  iVar1 = local_8;
  if ((*(byte *)((int)param_1 + 0x15) & 1) != 0) {
    local_8 = FUN_1000b7d0(param_1,0);
    iVar1 = local_8;
  }
  while( true ) {
    iVar4 = param_1[3];
    iVar3 = param_1[7];
    iVar6 = 0x80;
    if (iVar3 <= iVar4 + local_8) break;
    if (iVar3 < iVar4 + 0x80 + local_8) {
      iVar6 = (iVar3 - iVar4) - local_8;
    }
    if ((*(byte *)(param_1 + 5) & 2) == 0) {
      iVar5 = local_8 + iVar6;
      if (iVar3 < iVar4 + iVar5) {
        *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) | 2;
      }
      else if (((iVar4 < param_1[8]) || (param_1[1] < (iVar4 - param_1[8]) + iVar5)) &&
              (cVar2 = (*(code *)param_1[9])(iVar5), cVar2 == '\0')) {
        *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) | 2;
      }
      *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) & 0xfd;
    }
    iVar4 = param_1[7] - param_1[3];
    if (iVar4 < iVar6) {
      iVar6 = iVar4;
    }
    if (iVar6 == 0) break;
    iVar4 = (param_1[3] - param_1[8]) + *param_1 + local_8;
    if ((*(byte *)((int)param_1 + 0x15) & 1) == 0) {
      iVar3 = 0;
      if (iVar6 < 1) goto LAB_1000b975;
      do {
        if (*(char *)(iVar3 + iVar4) == '\0') {
          return (iVar3 - iVar1) + 1 + local_8;
        }
        iVar3 = iVar3 + 1;
      } while (iVar3 < iVar6);
      local_8 = local_8 + iVar6;
    }
    else {
      iVar3 = 0;
      if (0 < iVar6) {
        do {
          iVar5 = _isspace((uint)*(byte *)(iVar4 + iVar3));
          if ((iVar5 != 0) || (*(char *)(iVar4 + iVar3) == '\0')) {
            return local_8 + 1 + (iVar3 - iVar1);
          }
          iVar3 = iVar3 + 1;
        } while (iVar3 < iVar6);
      }
LAB_1000b975:
      local_8 = local_8 + iVar6;
    }
  }
  if (local_8 != iVar1) {
    return (local_8 - iVar1) + 1;
  }
  return 0;
}


// 1000b9d0 FUN_1000b9d0

uint __thiscall FUN_1000b9d0(void *this,int param_1,char *param_2,int param_3)

{
  char cVar1;
  uint in_EAX;
  undefined3 extraout_var;
  int iVar2;
  
  if ((*(byte *)((int)this + 0x14) & 2) == 0) {
    cVar1 = FUN_1000b3d0(this,param_1 + param_3);
    in_EAX = CONCAT31(extraout_var,cVar1);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
    if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
      iVar2 = FUN_10009950((char *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this
                                   + param_1),param_2,param_3);
      return (uint)(iVar2 == 0);
    }
  }
  return in_EAX & 0xffffff00;
}


// 1000ba30 FUN_1000ba30

void __thiscall FUN_1000ba30(void *this,void *param_1,size_t param_2)

{
  char cVar1;
  
  if (((param_2 != 0) && ((*(byte *)((int)this + 0x14) & 1) == 0)) &&
     ((*(byte *)((int)this + 0x15) & 8) == 0)) {
    if ((*(int *)((int)this + 0x10) < *(int *)((int)this + 0x20)) ||
       (*(int *)((int)this + 4) <
        (int)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_2))) {
      cVar1 = (**(code **)((int)this + 0x28))(param_2);
      if (cVar1 == '\0') {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 1;
        return;
      }
    }
                    /* WARNING: Load size is inaccurate */
    _memcpy((void *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this),param_1,
            param_2);
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + param_2;
    FUN_1000b590((int *)this);
  }
  return;
}


// 1000baa0 FUN_1000baa0

uint __thiscall FUN_1000baa0(void *this,int param_1)

{
  uint in_EAX;
  int iVar1;
  
  if (*(int *)((int)this + 8) < 0) {
    if ((*(byte *)((int)this + 0x15) & 2) == 0) {
      return in_EAX & 0xffffff00;
    }
    FUN_1000b5e0(this,0);
  }
  iVar1 = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1;
  if (*(int *)((int)this + 4) < iVar1) {
    do {
      iVar1 = FUN_1000b630(this,1);
    } while (*(int *)((int)this + 4) <
             (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1);
  }
  return CONCAT31((int3)((uint)iVar1 >> 8),1);
}


// 1000baf0 FUN_1000baf0

void __thiscall FUN_1000baf0(void *this,int param_1,int param_2)

{
  undefined4 uVar1;
  int iVar2;
  
  iVar2 = param_2;
  if (param_1 != 0) {
    if (param_1 == 1) {
      iVar2 = *(int *)((int)this + 0x10) + param_2;
    }
    else {
      iVar2 = *(int *)((int)this + 0x10);
      if (param_1 == 2) {
        iVar2 = *(int *)((int)this + 0x1c) - param_2;
      }
    }
  }
  (**(code **)((int)this + 0x28))(-1 - iVar2);
  *(int *)((int)this + 0x10) = iVar2;
  if (*(int *)((int)this + 0x1c) < iVar2) {
    if (((*(byte *)((int)this + 0x15) & 8) == 0) && ((*(byte *)((int)this + 0x14) & 1) == 0)) {
      uVar1 = FUN_1000b540(this,1);
      if ((char)uVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
        *(undefined1 *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0x10)) = 0;
        *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)((int)this + 0x10);
        return;
      }
      *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfe;
    }
    *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)((int)this + 0x10);
  }
  return;
}


// 1000bb70 FUN_1000bb70

void __thiscall FUN_1000bb70(void *this,undefined1 param_1)

{
  char cVar1;
  undefined4 uVar2;
  
  if (((*(byte *)((int)this + 0x14) & 1) == 0) && ((*(byte *)((int)this + 0x15) & 8) == 0)) {
    if ((*(int *)((int)this + 0x10) < *(int *)((int)this + 0x20)) ||
       (*(int *)((int)this + 4) < (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + 1)) {
      cVar1 = (**(code **)((int)this + 0x28))(1);
      if (cVar1 == '\0') {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 1;
        return;
      }
    }
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0x10)) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
    if (*(int *)((int)this + 0x1c) < *(int *)((int)this + 0x10)) {
      if (((*(byte *)((int)this + 0x15) & 8) == 0) && ((*(byte *)((int)this + 0x14) & 1) == 0)) {
        uVar2 = FUN_1000b540(this,1);
        if ((char)uVar2 != '\0') {
                    /* WARNING: Load size is inaccurate */
          *(undefined1 *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0x10)) = 0;
          *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)((int)this + 0x10);
          return;
        }
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfe;
      }
      *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)((int)this + 0x10);
    }
  }
  return;
}


// 1000bc10 FUN_1000bc10

void __fastcall FUN_1000bc10(void *param_1)

{
  int iVar1;
  
  if ((*(byte *)((int)param_1 + 0x15) & 0x10) == 0) {
    iVar1 = *(int *)((int)param_1 + 0x18);
  }
  else {
    iVar1 = 0;
  }
  while (iVar1 = iVar1 + -1, -1 < iVar1) {
    FUN_1000bb70(param_1,9);
  }
  return;
}


// 1000bc50 FUN_1000bc50

int * __thiscall FUN_1000bc50(void *this,int param_1,int param_2,byte param_3)

{
  undefined4 uVar1;
  
  *(undefined4 *)this = 0;
  *(int *)((int)this + 4) = param_2;
  *(int *)((int)this + 8) = param_1;
  if (param_2 != 0) {
    uVar1 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_2);
    *(undefined4 *)this = uVar1;
  }
  *(undefined1 *)((int)this + 0x14) = 0;
  *(uint *)((int)this + 0x2c) = *(uint *)((int)this + 0x2c) & 0xfffffffc;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x18) = 0;
  *(undefined4 *)((int)this + 0x20) = 0;
  *(byte *)((int)this + 0x15) = param_3;
  if ((param_2 == 0) || ((param_3 & 8) != 0)) {
    *(undefined4 *)((int)this + 0x1c) = 0;
  }
  else {
    *(undefined4 *)((int)this + 0x1c) = 0xffffffff;
    FUN_1000b590((int *)this);
  }
  *(undefined1 **)((int)this + 0x24) = &LAB_1000aa00;
  *(code **)((int)this + 0x28) = FUN_1000baa0;
  return (int *)this;
}


// 1000bcd0 FUN_1000bcd0

int * __thiscall FUN_1000bcd0(void *this,int param_1,int param_2,byte param_3)

{
  undefined4 uVar1;
  
  *(int *)this = param_1;
  *(int *)((int)this + 4) = param_2;
  *(undefined4 *)((int)this + 8) = 0xffffffff;
  *(undefined1 *)((int)this + 0x14) = 0;
  *(uint *)((int)this + 0x2c) = *(uint *)((int)this + 0x2c) & 0xfffffffc;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x18) = 0;
  *(undefined4 *)((int)this + 0x20) = 0;
  *(byte *)((int)this + 0x15) = param_3;
  if ((param_3 & 8) == 0) {
    *(undefined4 *)((int)this + 0x1c) = 0xffffffff;
    if ((*(byte *)((int)this + 0x14) & 1) == 0) {
      uVar1 = FUN_1000b540(this,1);
      if ((char)uVar1 == '\0') {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfe;
      }
      else {
                    /* WARNING: Load size is inaccurate */
        *(undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = 0;
      }
    }
    *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)((int)this + 0x10);
  }
  else {
    *(int *)((int)this + 0x1c) = param_2;
  }
  *(undefined1 **)((int)this + 0x24) = &LAB_1000aa00;
  *(code **)((int)this + 0x28) = FUN_1000baa0;
  return (int *)this;
}


// 1000bd50 FUN_1000bd50

void __thiscall FUN_1000bd50(void *this,int param_1,int param_2,int param_3,byte param_4)

{
  undefined4 uVar1;
  
  if (-1 < *(int *)((int)this + 8)) {
                    /* WARNING: Load size is inaccurate */
    if (*this != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*this);
      *(undefined4 *)this = 0;
    }
    *(undefined4 *)((int)this + 4) = 0;
  }
  *(int *)((int)this + 4) = param_2;
  *(int *)this = param_1;
  *(undefined4 *)((int)this + 8) = 0xffffffff;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(int *)((int)this + 0x10) = param_3;
  *(undefined4 *)((int)this + 0x18) = 0;
  *(undefined1 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 0x20) = 0;
  *(byte *)((int)this + 0x15) = param_4;
  *(undefined4 *)((int)this + 0x1c) = 0xffffffff;
  if (-1 < param_3) {
    if ((param_4 & 8) == 0) {
      uVar1 = FUN_1000b540(this,1);
      if ((char)uVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
        *(undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = 0;
        *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)((int)this + 0x10);
        return;
      }
      *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfe;
    }
    *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)((int)this + 0x10);
  }
  return;
}


// 1000bdf0 FUN_1000bdf0

void __thiscall FUN_1000bdf0(void *this,char *param_1)

{
  int iVar1;
  char *pcVar2;
  size_t sVar3;
  
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    if (param_1 != (char *)0x0) {
      iVar1 = FUN_10009820(param_1);
      FUN_1000ba30(this,param_1,iVar1 + 1);
      return;
    }
    FUN_1000bb70(this,0);
    return;
  }
  if (param_1 != (char *)0x0) {
    if (((*(byte *)((int)this + 0x15) & 0x10) == 0) && (0 < *(int *)((int)this + 0x18))) {
                    /* WARNING: Load size is inaccurate */
      if ((*(int *)((int)this + 0x10) != 0) &&
         (*(char *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + -1 + *this) == '\n')
         ) {
        FUN_1000bc10(this);
      }
      pcVar2 = _strchr(param_1,10);
      while (pcVar2 != (char *)0x0) {
        FUN_1000ba30(this,param_1,(size_t)(pcVar2 + (1 - (int)param_1)));
        param_1 = pcVar2 + 1;
        if (pcVar2[1] == '\0') break;
        if ((*(byte *)((int)this + 0x15) & 0x10) == 0) {
          iVar1 = *(int *)((int)this + 0x18);
        }
        else {
          iVar1 = 0;
        }
        while (iVar1 = iVar1 + -1, -1 < iVar1) {
          FUN_1000bb70(this,9);
        }
        pcVar2 = _strchr(param_1,10);
      }
    }
    sVar3 = FUN_10009820(param_1);
    if (sVar3 != 0) {
      FUN_1000ba30(this,param_1,sVar3);
    }
  }
  return;
}


// 1000bee0 FUN_1000bee0

void __cdecl FUN_1000bee0(void *param_1,char *param_2)

{
  char local_800 [2048];
  
  FUN_10009ca0(local_800,0x800,param_2,&stack0x0000000c);
  FUN_1000bdf0(param_1,local_800);
  return;
}


// 1000bf20 FUN_1000bf20

uint __fastcall FUN_1000bf20(int *param_1)

{
  undefined3 uVar1;
  char cVar2;
  char cVar3;
  char *in_EAX;
  undefined3 extraout_var;
  undefined3 extraout_var_00;
  undefined3 extraout_var_01;
  
  if (((*(byte *)((int)param_1 + 0x15) & 1) != 0) && ((char)param_1[5] == '\0')) {
    cVar2 = FUN_1000b3d0(param_1,2);
    in_EAX = (char *)CONCAT31(extraout_var,cVar2);
    *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) & 0xfd;
    if (cVar2 != '\0') {
      in_EAX = (char *)((param_1[3] - param_1[8]) + *param_1);
      if (((in_EAX != (char *)0x0) && (*in_EAX == '/')) && (in_EAX[1] == '/')) {
        param_1[3] = param_1[3] + 2;
        cVar3 = FUN_10007710(param_1);
        cVar2 = (char)param_1[5];
        uVar1 = extraout_var_00;
        while ((cVar2 == '\0' && (cVar3 != '\n'))) {
          cVar3 = FUN_10007710(param_1);
          cVar2 = (char)param_1[5];
          uVar1 = extraout_var_01;
        }
        return CONCAT31(uVar1,1);
      }
    }
  }
  return (uint)in_EAX & 0xffffff00;
}


// 1000bf90 FUN_1000bf90

void __thiscall FUN_1000bf90(void *this,undefined1 *param_1,int param_2)

{
  int iVar1;
  
  if (*(char *)((int)this + 0x14) != '\0') {
    *param_1 = 0;
    return;
  }
  if (param_2 == 0) {
    param_2 = 0x7fffffff;
  }
  iVar1 = FUN_1000b870((int *)this);
  if ((*(byte *)((int)this + 0x15) & 1) != 0) {
    FUN_1000b760((int *)this);
  }
  if (iVar1 == 0) {
    *param_1 = 0;
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
    return;
  }
  if (param_2 < iVar1) {
    FUN_1000b720(this,param_1,param_2 - 1);
    param_1[param_2 + -1] = 0;
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + (iVar1 - param_2) + -1;
    iVar1 = *(int *)((int)this + 0xc);
    if (*(int *)((int)this + 0x1c) < iVar1) {
      *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
    }
    else {
      *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
      if ((iVar1 < *(int *)((int)this + 0x20)) ||
         (*(int *)((int)this + 4) + *(int *)((int)this + 0x20) <= iVar1)) {
        (**(code **)((int)this + 0x24))(0xffffffff);
      }
    }
  }
  else {
    FUN_1000b720(this,param_1,iVar1 - 1);
    param_1[iVar1 + -1] = 0;
  }
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    FUN_10007710((int *)this);
  }
  return;
}


// 1000c050 FUN_1000c050

uint __thiscall FUN_1000c050(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  char cVar2;
  undefined3 extraout_var;
  uint uVar3;
  int iVar4;
  int unaff_EBX;
  
  cVar2 = FUN_10007710((int *)this);
  puVar1 = param_1;
  iVar4 = CONCAT31(extraout_var,cVar2);
  if (cVar2 == *(char *)(param_1 + 1)) {
    param_1 = (undefined4 *)param_1[5];
    uVar3 = FUN_1000b430(this,0,(int *)&param_1);
    if ((char)uVar3 == '\0') {
      return uVar3;
    }
                    /* WARNING: Load size is inaccurate */
    cVar2 = (**(code **)*puVar1)
                      ((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this,&param_1);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + unaff_EBX;
    iVar4 = *(int *)((int)this + 0xc);
    if (*(int *)((int)this + 0x1c) < iVar4) {
      *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
      return CONCAT31((int3)((uint)iVar4 >> 8),cVar2);
    }
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
    if ((iVar4 < *(int *)((int)this + 0x20)) ||
       (*(int *)((int)this + 4) + *(int *)((int)this + 0x20) <= iVar4)) {
      iVar4 = (**(code **)((int)this + 0x24))(0xffffffff);
    }
  }
  return CONCAT31((int3)((uint)iVar4 >> 8),cVar2);
}


// 1000c0e0 FUN_1000c0e0

void __thiscall FUN_1000c0e0(void *this,undefined4 *param_1,undefined1 *param_2,int param_3)

{
  char cVar1;
  int iVar2;
  uint uVar3;
  int iVar4;
  
  if (((*(byte *)((int)this + 0x15) & 1) == 0) || (param_1 == (undefined4 *)0x0)) {
    FUN_1000bf90(this,param_2,param_3);
  }
  else {
    if (*(char *)((int)this + 0x14) != '\0') {
      *param_2 = 0;
      return;
    }
    if (param_3 == 0) {
      param_3 = 0x7fffffff;
    }
    FUN_1000b760((int *)this);
    uVar3 = FUN_1000b9d0(this,0,(char *)param_1[2],param_1[3]);
    if ((char)uVar3 != '\0') {
      *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_1[3];
      iVar4 = *(int *)((int)this + 0xc);
      if (*(int *)((int)this + 0x1c) < iVar4) {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
      }
      else {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
        if ((iVar4 < *(int *)((int)this + 0x20)) ||
           (*(int *)((int)this + 4) + *(int *)((int)this + 0x20) <= iVar4)) {
          (**(code **)((int)this + 0x24))(0xffffffff);
        }
      }
      iVar4 = 0;
      cVar1 = *(char *)((int)this + 0x14);
      do {
        if (cVar1 != '\0') {
LAB_1000c1da:
          if (param_3 <= iVar4) {
            iVar4 = param_3 + -1;
          }
          param_2[iVar4] = 0;
          return;
        }
        uVar3 = FUN_1000b9d0(this,0,(char *)param_1[2],param_1[3]);
        if ((char)uVar3 != '\0') {
          *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_1[3];
          iVar2 = *(int *)((int)this + 0xc);
          if (*(int *)((int)this + 0x1c) < iVar2) {
            *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
          }
          else {
            *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
            if ((iVar2 < *(int *)((int)this + 0x20)) ||
               (*(int *)((int)this + 4) + *(int *)((int)this + 0x20) <= iVar2)) {
              (**(code **)((int)this + 0x24))(0xffffffff);
            }
          }
          goto LAB_1000c1da;
        }
        uVar3 = FUN_1000c050(this,param_1);
        if (iVar4 < param_3) {
          param_2[iVar4] = (char)uVar3;
          iVar4 = iVar4 + 1;
        }
        cVar1 = *(char *)((int)this + 0x14);
      } while( true );
    }
  }
  return;
}


// 1000c210 FUN_1000c210

int __thiscall FUN_1000c210(void *this,char *param_1,int param_2)

{
  undefined4 *puVar1;
  undefined1 *puVar2;
  float *pfVar3;
  char cVar4;
  char cVar5;
  uint uVar6;
  ulong uVar7;
  char *pcVar8;
  ulong *puVar9;
  int iVar10;
  float10 fVar11;
  int local_8;
  char *local_4;
  
  if ((*(char *)((int)this + 0x14) != '\0') || ((*(byte *)((int)this + 0x15) & 1) == 0)) {
    return 0;
  }
  cVar5 = *param_1;
  local_8 = 0;
  if (cVar5 != '\0') {
    iVar10 = param_2 + -4;
    do {
      pcVar8 = param_1 + 1;
      if (*(int *)((int)this + 0x1c) <= *(int *)((int)this + 0xc)) {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
        return local_8;
      }
      if (cVar5 == ' ') {
        FUN_1000b760((int *)this);
        goto LAB_1000c48b;
      }
      if (cVar5 != '%') {
        if ((*(byte *)((int)this + 0x14) & 2) != 0) {
          return local_8;
        }
        cVar4 = FUN_1000b3d0(this,1);
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
        if (cVar4 == '\0') {
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        if (cVar5 != *(char *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this)) {
          return local_8;
        }
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
        goto LAB_1000c48b;
      }
      cVar5 = *pcVar8;
      pcVar8 = param_1 + 2;
      if (cVar5 == '\0') {
        return local_8;
      }
      switch(cVar5) {
      case 'c':
        puVar2 = *(undefined1 **)(iVar10 + 4);
        iVar10 = iVar10 + 4;
        if ((*(byte *)((int)this + 0x14) & 2) != 0) {
LAB_1000c4b6:
          *puVar2 = 0;
          return local_8;
        }
        cVar5 = FUN_1000b3d0(this,1);
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
        if (cVar5 == '\0') goto LAB_1000c4b6;
                    /* WARNING: Load size is inaccurate */
        *puVar2 = *(undefined1 *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc));
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
        local_8 = local_8 + 1;
        break;
      case 'd':
      case 'i':
        puVar9 = *(ulong **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000b430(this,0,&param_2);
        if ((char)uVar6 == '\0') {
LAB_1000c4c7:
          *puVar9 = 0;
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        uVar7 = _strtol((char *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc)),
                        &local_4,10);
        goto LAB_1000c356;
      default:
        goto switchD_1000c2d2_caseD_65;
      case 'f':
        pfVar3 = *(float **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000b430(this,0,&param_2);
        if ((char)uVar6 == '\0') {
          *pfVar3 = 0.0;
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        fVar11 = (float10)FUN_10010409((byte *)((*this - *(int *)((int)this + 0x20)) +
                                               *(int *)((int)this + 0xc)),&local_4);
        *pfVar3 = (float)fVar11;
        goto LAB_1000c35c;
      case 's':
        puVar1 = (undefined4 *)(iVar10 + 4);
        iVar10 = iVar10 + 4;
        FUN_1000bf90(this,(undefined1 *)*puVar1,0);
        local_8 = local_8 + 1;
        break;
      case 'u':
        puVar9 = *(ulong **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000b430(this,0,&param_2);
        if ((char)uVar6 == '\0') goto LAB_1000c4c7;
                    /* WARNING: Load size is inaccurate */
        uVar7 = _strtoul((char *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc)),
                         &local_4,10);
LAB_1000c356:
        *puVar9 = uVar7;
LAB_1000c35c:
        iVar10 = iVar10 + 4;
                    /* WARNING: Load size is inaccurate */
        if (local_4 + ((*(int *)((int)this + 0x20) - *(int *)((int)this + 0xc)) - *this) ==
            (char *)0x0) {
          return local_8;
        }
        local_8 = local_8 + 1;
        *(char **)((int)this + 0xc) =
             local_4 + ((*(int *)((int)this + 0x20) - *(int *)((int)this + 0xc)) - *this) +
             *(int *)((int)this + 0xc);
        break;
      case 'x':
        puVar9 = *(ulong **)(iVar10 + 4);
        iVar10 = iVar10 + 4;
        param_2 = 0x80;
        uVar6 = FUN_1000b430(this,0,&param_2);
        if ((char)uVar6 == '\0') goto LAB_1000c4c7;
                    /* WARNING: Load size is inaccurate */
        uVar7 = _strtol((char *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc)),
                        &local_4,0x10);
        *puVar9 = uVar7;
                    /* WARNING: Load size is inaccurate */
        if (local_4 + ((*(int *)((int)this + 0x20) - *(int *)((int)this + 0xc)) - *this) ==
            (char *)0x0) {
          return local_8;
        }
        local_8 = local_8 + 1;
        *(char **)((int)this + 0xc) =
             local_4 + ((*(int *)((int)this + 0x20) - *(int *)((int)this + 0xc)) - *this) +
             *(int *)((int)this + 0xc);
      }
LAB_1000c48b:
      cVar5 = *pcVar8;
      param_1 = pcVar8;
    } while (cVar5 != '\0');
  }
switchD_1000c2d2_caseD_65:
  return local_8;
}


// 1000c530 FUN_1000c530

void __cdecl FUN_1000c530(void *param_1,char *param_2)

{
  FUN_1000c210(param_1,param_2,(int)&stack0x0000000c);
  return;
}


// 1000c550 FUN_1000c550

void __fastcall FUN_1000c550(undefined4 *param_1)

{
  *param_1 = 0;
  return;
}


// 1000c560 FUN_1000c560

void FUN_1000c560(int *param_1)

{
  if ((int *)*param_1 != (int *)0x0) {
    FUN_1000c560((int *)*param_1);
    FUN_1000c560((int *)(*param_1 + 4));
    FUN_100012d0(*param_1);
    *param_1 = 0;
  }
  return;
}


// 1000c5a0 FUN_1000c5a0

void __fastcall FUN_1000c5a0(int *param_1)

{
  if ((int *)*param_1 != (int *)0x0) {
    FUN_1000c560((int *)*param_1);
    FUN_1000c560((int *)(*param_1 + 4));
    FUN_100012d0(*param_1);
    *param_1 = 0;
  }
  return;
}


// 1000c5d0 FUN_1000c5d0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1000c5d0(int param_1,int param_2)

{
  int iVar1;
  int iVar2;
  
  if (DAT_100396a4 == '\0') {
    iVar2 = 0;
    DAT_100396a4 = '\x01';
    iVar1 = DAT_1003969c;
    if (0 < param_2) {
      do {
        if (iVar1 == 0) {
          iVar1 = (**(code **)(param_1 + iVar2 * 4))("VEngineCvar007",0);
          _DAT_10039698 = iVar1;
          DAT_1003969c = iVar1;
        }
        if (DAT_100396a0 == 0) {
          DAT_100396a0 = (**(code **)(param_1 + iVar2 * 4))("VProcessUtils001",0);
          iVar1 = DAT_1003969c;
        }
        iVar2 = iVar2 + 1;
      } while (iVar2 < param_2);
    }
  }
  return;
}


// 1000c640 FUN_1000c640

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_1000c640(void)

{
  if (DAT_100396a4 != '\0') {
    _DAT_10039698 = 0;
    DAT_1003969c = 0;
    DAT_100396a0 = 0;
    DAT_100396a4 = '\0';
  }
  return;
}


// 1000c660 FUN_1000c660

void __thiscall FUN_1000c660(void *this,void *param_1,int param_2)

{
  size_t _Size;
  void *_Dst;
  
  if (param_1 == (void *)0x0) {
    param_2 = 0;
  }
  *(int *)((int)this + 0xc) = param_2;
  if (*(int *)((int)this + 4) < param_2) {
    FUN_1000b630(this,param_2 - *(int *)((int)this + 4));
    if (*(int *)((int)this + 4) < param_2) {
      *(int *)((int)this + 0xc) = *(int *)((int)this + 4);
    }
  }
  _Size = *(size_t *)((int)this + 0xc);
  if (_Size != 0) {
                    /* WARNING: Load size is inaccurate */
    _Dst = *this;
    if ((_Dst < (void *)((int)param_1 + param_2)) && (param_1 < (void *)((int)_Dst + _Size))) {
      _memmove(_Dst,param_1,_Size);
      return;
    }
    _memcpy(_Dst,param_1,_Size);
  }
  return;
}


// 1000c6d0 FUN_1000c6d0

undefined4 * __thiscall FUN_1000c6d0(void *this,char *param_1)

{
  int iVar1;
  
  iVar1 = 0;
  *(undefined4 *)this = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined4 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  if (param_1 != (char *)0x0) {
    iVar1 = FUN_10009820(param_1);
    iVar1 = iVar1 + 1;
  }
  FUN_1000c660(this,param_1,iVar1);
  return (undefined4 *)this;
}


// 1000c710 FUN_1000c710

undefined4 * __thiscall FUN_1000c710(void *this,int *param_1)

{
  int iVar1;
  char *pcVar2;
  
  iVar1 = 0;
  *(undefined4 *)this = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined4 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  if (param_1[3] == 0) {
    pcVar2 = "";
  }
  else {
    pcVar2 = (char *)*param_1;
    if (pcVar2 == (char *)0x0) goto LAB_1000c743;
  }
  iVar1 = FUN_10009820(pcVar2);
  iVar1 = iVar1 + 1;
LAB_1000c743:
  FUN_1000c660(this,pcVar2,iVar1);
  return (undefined4 *)this;
}


// 1000c760 FUN_1000c760

void __thiscall FUN_1000c760(void *this,byte *param_1,undefined4 param_2)

{
  uint uVar1;
  undefined **ppuVar2;
  
  ppuVar2 = &PTR_s_VGUI_Localize005_100357b0;
  if (PTR_s_VGUI_Localize005_100357b0 != (undefined *)0x0) {
    do {
      uVar1 = FUN_100098d0(*ppuVar2,param_1);
      if (uVar1 == 0) {
        *(undefined4 *)(ppuVar2[1] + 0x50 + (int)this) = param_2;
      }
      ppuVar2 = ppuVar2 + 2;
    } while (*ppuVar2 != (undefined *)0x0);
  }
  return;
}


// 1000c7b0 FUN_1000c7b0

void __thiscall FUN_1000c7b0(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  undefined4 uVar3;
  uint uVar4;
  
  iVar1 = *(int *)((int)this + 8);
  if (iVar1 < 0) {
    return;
  }
  iVar2 = *(int *)((int)this + 4);
  uVar4 = param_1 + iVar2;
  if (iVar1 == 0) {
    if (iVar2 == 0) {
      iVar2 = 4;
    }
    if ((int)uVar4 <= iVar2) goto LAB_1000c80b;
    do {
      iVar2 = iVar2 * 2;
    } while (iVar2 < (int)uVar4);
  }
  else {
    iVar2 = ((int)(uVar4 - 1) / iVar1 + 1) * iVar1;
  }
  if (iVar2 < (int)uVar4) {
    if ((iVar2 == 0) && (0x7fffffff < uVar4)) {
      iVar2 = -1;
    }
    else {
      do {
        iVar2 = (int)(iVar2 + uVar4) / 2;
      } while (iVar2 < (int)uVar4);
    }
  }
LAB_1000c80b:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 * 8);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 1000c840 FUN_1000c840

int __thiscall FUN_1000c840(void *this,int param_1,int param_2,undefined4 *param_3)

{
  int iVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (param_2 == 0) {
    return param_1;
  }
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + param_2) {
    FUN_1000c7b0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + param_2);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
                    /* WARNING: Load size is inaccurate */
  iVar3 = *this;
  iVar1 = (*(int *)((int)this + 0xc) - param_1) - param_2;
  *(int *)((int)this + 0x10) = iVar3;
  if (0 < iVar1) {
    if (param_2 < 1) goto LAB_1000c8c8;
    FUN_10009be0((void *)(iVar3 + (param_1 + param_2) * 8),(void *)(iVar3 + param_1 * 8),iVar1 * 8);
  }
  if (0 < param_2) {
    iVar3 = param_1 * 8;
    iVar1 = param_2;
    do {
                    /* WARNING: Load size is inaccurate */
      puVar2 = (undefined4 *)(*this + iVar3);
      if (puVar2 != (undefined4 *)0x0) {
        *puVar2 = 0;
        puVar2[1] = 0;
      }
      iVar3 = iVar3 + 8;
      iVar1 = iVar1 + -1;
    } while (iVar1 != 0);
  }
LAB_1000c8c8:
  if ((param_3 != (undefined4 *)0x0) && (0 < param_2)) {
    iVar3 = param_1 * 8;
    do {
                    /* WARNING: Load size is inaccurate */
      iVar1 = *this;
      *(undefined4 *)(iVar3 + iVar1) = *param_3;
      *(undefined4 *)(iVar3 + 4 + iVar1) = param_3[1];
      param_3 = param_3 + 2;
      iVar3 = iVar3 + 8;
      param_2 = param_2 + -1;
    } while (param_2 != 0);
  }
  return param_1;
}


// 1000c910 FUN_1000c910

int __thiscall FUN_1000c910(void *this,byte *param_1)

{
  int iVar1;
  uint uVar2;
  
  FUN_100096f0((void *)((int)this + 0x14),(ushort *)&param_1,param_1);
  uVar2 = (uint)param_1 & 0xffff;
  iVar1 = *(int *)((int)this + 0xc);
  if ((iVar1 <= (int)uVar2) && (iVar1 < (int)(uVar2 + 1))) {
    FUN_1000c840(this,iVar1,(uVar2 + 1) - iVar1,(undefined4 *)0x0);
  }
                    /* WARNING: Load size is inaccurate */
  return *this + uVar2 * 8;
}


// 1000c950 FUN_1000c950

void __thiscall FUN_1000c950(void *this,byte *param_1,int param_2)

{
  int *piVar1;
  
  if ((param_1 != (byte *)0x0) && (param_2 != 0)) {
    piVar1 = (int *)FUN_1000c910((void *)((int)this + 4),param_1);
    if (piVar1[1] < 1) {
      *piVar1 = param_2;
    }
    piVar1[1] = piVar1[1] + 1;
    FUN_1000c760(this,param_1,param_2);
  }
  return;
}


// 1000c990 FUN_1000c990

void __thiscall FUN_1000c990(void *this,byte *param_1,int param_2)

{
  undefined4 *puVar1;
  int iVar2;
  
  if ((param_1 != (byte *)0x0) && (param_2 != 0)) {
    puVar1 = (undefined4 *)FUN_1000c910((void *)((int)this + 4),param_1);
    if ((0 < (int)puVar1[1]) && (iVar2 = puVar1[1] + -1, puVar1[1] = iVar2, iVar2 == 0)) {
      *puVar1 = 0;
      FUN_1000c760(this,param_1,0);
    }
  }
  return;
}


// 1000ca40 FUN_1000ca40

int __cdecl FUN_1000ca40(undefined4 param_1,undefined4 *param_2)

{
  int iVar1;
  code *pcVar2;
  undefined4 *puVar3;
  
  if (param_2 != (undefined4 *)0x0) {
    *param_2 = 0;
  }
  if ((DAT_100396c8 != (code *)0x0) && (iVar1 = (*DAT_100396c8)(param_1,param_2), iVar1 != 0)) {
    return iVar1;
  }
  puVar3 = param_2;
  pcVar2 = (code *)FUN_10009f40();
  iVar1 = (*pcVar2)(param_1,puVar3);
  if (iVar1 == 0) {
    if (param_2 != (undefined4 *)0x0) {
      *param_2 = 1;
    }
    iVar1 = 0;
  }
  return iVar1;
}


// 1000cbe0 FUN_1000cbe0

undefined * FUN_1000cbe0(void)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  undefined4 uVar4;
  char *pcVar5;
  char *pcVar6;
  
  iVar1 = FUN_10009820(&DAT_10039700);
  if (iVar1 != 0) {
    return &DAT_10039700;
  }
  piVar2 = (int *)CommandLine_Tier0();
  piVar3 = (int *)CommandLine_Tier0();
  iVar1 = *piVar3;
  uVar4 = (**(code **)(*piVar2 + 0x20))("-defaultgamedir",&DAT_1002852c);
  pcVar5 = (char *)(**(code **)(iVar1 + 0x20))("-game",uVar4);
  FUN_10009c10(&DAT_10039700,pcVar5,0x104);
  pcVar6 = _strchr(&DAT_10039700,0x2f);
  if ((pcVar6 == (char *)0x0) && (pcVar6 = _strchr(&DAT_10039700,0x5c), pcVar6 == (char *)0x0)) {
    return &DAT_10039700;
  }
  FUN_10009e00(&DAT_10039700,0x104);
  iVar1 = FUN_10009820(&DAT_10039700);
  FUN_10009c10(&DAT_10039700,pcVar5 + iVar1,0x104 - iVar1);
  return &DAT_10039700;
}


// 1000cd50 FUN_1000cd50

void __fastcall FUN_1000cd50(int param_1)

{
  FUN_10004c90();
  FUN_1000db20();
  if (*(int *)(param_1 + 8) != 0) {
                    /* WARNING: Could not recover jumptable at 0x1000cd78. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (**(code **)(**(int **)(param_1 + 8) + 0x10))();
    return;
  }
  return;
}


// 1000cf20 FUN_1000cf20

void __fastcall FUN_1000cf20(undefined4 *param_1)

{
  param_1[1] = &PTR_LAB_100259cc;
  *param_1 = &PTR_LAB_10028544;
  param_1[1] = &PTR_LAB_1002853c;
  param_1[2] = 0;
  return;
}


// 1000cf40 FUN_1000cf40

undefined4 __fastcall FUN_1000cf40(int param_1)

{
  return *(undefined4 *)(param_1 + 8);
}


// 1000cf50 FUN_1000cf50

int * __fastcall FUN_1000cf50(int param_1)

{
  int *piVar1;
  
  if (*(void **)(param_1 + 0xc) != (void *)0x0) {
    piVar1 = FUN_100058e0(*(void **)(param_1 + 0xc),"stat",0);
    return piVar1;
  }
  return (int *)0x0;
}


// 1000cf70 FUN_1000cf70

undefined4 * __thiscall FUN_1000cf70(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_100285d8;
  *(undefined ***)((int)this + 4) = &PTR_FUN_100285d0;
  if (*(int *)((int)this + 0xc) != 0) {
    FUN_10005ff0(*(int *)((int)this + 0xc));
  }
  *(undefined4 *)((int)this + 0xc) = 0;
  if (*(int *)((int)this + 8) != 0) {
    FUN_10005ff0(*(int *)((int)this + 8));
  }
  *(undefined4 *)((int)this + 8) = 0;
  if ((param_1 & 1) != 0) {
    FUN_100012d0(this);
  }
  return (undefined4 *)this;
}


// 1000cfc0 FUN_1000cfc0

void __thiscall FUN_1000cfc0(void *this,char param_1)

{
  float fVar1;
  float10 fVar2;
  
  DevMsg("Datacenter::EnableUpdate( %d ), current state = %d\n");
  if (param_1 == '\0') {
    fVar2 = (float10)Plat_FloatTime(*(undefined4 *)((int)this + 0x14));
    DevMsg("Datacenter::RequestStop, time %.2f, state %d\n",(double)fVar2);
    *(undefined4 *)((int)this + 0x14) = 3;
    return;
  }
  if (*(int *)((int)this + 0x14) == 3) {
    *(undefined4 *)((int)this + 0x14) = 0;
    fVar1 = *(float *)(DAT_10039964 + 0x2c);
    fVar2 = (float10)Plat_FloatTime();
    fVar2 = fVar2 + (float10)fVar1;
    if ((float10)*(float *)((int)this + 0x10) < fVar2) {
      *(float *)((int)this + 0x10) = (float)fVar2;
      return;
    }
  }
  return;
}


// 1000d050 FUN_1000d050

void FUN_1000d050(undefined4 *param_1)

{
  byte *pbVar1;
  uint uVar2;
  
  pbVar1 = FUN_10005080(param_1);
  uVar2 = FUN_100098d0(pbVar1,(byte *)"OnProfileStorageAvailable");
  if (uVar2 == 0) {
    FUN_10005a70(param_1,"iController",0);
  }
  return;
}


// 1000d080 FUN_1000d080

void __fastcall FUN_1000d080(int param_1)

{
  float10 fVar1;
  
  fVar1 = (float10)Plat_FloatTime(*(undefined4 *)(param_1 + 0x14));
  DevMsg("Datacenter::RequestStop, time %.2f, state %d\n",(double)fVar1);
  *(undefined4 *)(param_1 + 0x14) = 0;
  DevMsg("Datacenter::RequestUpdate %s\n","failed");
  return;
}


// 1000d0c0 FUN_1000d0c0

void FUN_1000d0c0(void)

{
  void *pvVar1;
  undefined **ppuStack_8;
  undefined4 uStack_4;
  
  pvVar1 = (void *)(**(code **)(*(int *)PTR_PTR_10035900 + 0xc))();
  ppuStack_8 = &PTR_LAB_1002528c;
  uStack_4 = 1;
  FUN_10005560(pvVar1,(int *)&ppuStack_8,1);
  pvVar1 = (void *)(*(code *)**(undefined4 **)PTR_PTR_10035900)();
  ppuStack_8 = &PTR_LAB_1002528c;
  uStack_4 = 1;
  FUN_10005560(pvVar1,(int *)&ppuStack_8,1);
  return;
}


// 1000d120 FUN_1000d120

uint __fastcall FUN_1000d120(int param_1)

{
  char cVar1;
  uint uVar2;
  undefined2 extraout_var;
  float10 fVar3;
  float10 fVar4;
  
  if (*(int *)(param_1 + 0x14) == 0) {
    fVar3 = (float10)Plat_FloatTime();
    fVar4 = (float10)*(float *)(param_1 + 0x10);
    uVar2 = CONCAT22(extraout_var,
                     (ushort)(fVar4 < fVar3) << 8 | (ushort)(NAN(fVar4) || NAN(fVar3)) << 10 |
                     (ushort)(fVar4 == fVar3) << 0xe);
    if (fVar4 < fVar3) {
      if ((PTR_PTR_10035700 == (undefined *)0x0) ||
         (*(int **)(PTR_PTR_10035700 + 0x5c) == (int *)0x0)) {
        return 0;
      }
      cVar1 = (**(code **)(**(int **)(PTR_PTR_10035700 + 0x5c) + 0x6c))();
      if ((cVar1 == '\0') &&
         ((cVar1 = (**(code **)(**(int **)(PTR_PTR_10035700 + 0x5c) + 0x70))(), cVar1 == '\0' &&
          (cVar1 = (**(code **)(**(int **)(PTR_PTR_10035700 + 0x5c) + 0x28c))(), cVar1 == '\0')))) {
        return 0;
      }
      return 1;
    }
  }
  else {
    uVar2 = *(int *)(param_1 + 0x14) - 1;
    if (uVar2 < 2) {
      uVar2 = FUN_1000d080(param_1);
      return uVar2;
    }
  }
  return uVar2;
}


// 1000d190 FUN_1000d190

undefined4 * __fastcall FUN_1000d190(undefined4 *param_1)

{
  float10 fVar1;
  
  *param_1 = &PTR_LAB_10028850;
  fVar1 = (float10)Plat_FloatTime();
  param_1[1] = (float)fVar1;
  param_1[2] = 0;
  param_1[3] = 0;
  param_1[4] = 0;
  return param_1;
}


// 1000d1c0 FUN_1000d1c0

undefined4 * __thiscall FUN_1000d1c0(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_LAB_10028850;
  if (*(int *)((int)this + 0x10) != 0) {
    FUN_10005ff0(*(int *)((int)this + 0x10));
  }
  *(undefined4 *)((int)this + 0x10) = 0;
  if ((param_1 & 1) != 0) {
    FUN_100012d0(this);
  }
  return (undefined4 *)this;
}


// 1000d290 FUN_1000d290

undefined4 * FUN_1000d290(int *param_1,undefined4 param_2,undefined4 param_3)

{
  undefined4 *puVar1;
  int iVar2;
  longlong lVar3;
  
  iVar2 = 0;
  if (0 < param_1[3]) {
    do {
      puVar1 = *(undefined4 **)(*param_1 + iVar2 * 4);
      if ((puVar1 != (undefined4 *)0x0) &&
         (lVar3 = (**(code **)*puVar1)(), lVar3 == CONCAT44(param_3,param_2))) {
        return puVar1;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_1[3]);
  }
  return (undefined4 *)0x0;
}


// 1000d300 FUN_1000d300

void __fastcall FUN_1000d300(int param_1)

{
  void *pvVar1;
  int iVar2;
  int iVar3;
  float10 fVar4;
  
  fVar4 = (float10)Plat_FloatTime();
  iVar3 = 0;
  if (0 < *(int *)(param_1 + 0x20)) {
    do {
      iVar2 = *(int *)(*(int *)(param_1 + 0x14) + iVar3 * 4);
      if ((iVar2 == 0) || (*(float *)(DAT_10039ae4 + 0x2c) <= (float)fVar4 - *(float *)(iVar2 + 4)))
      {
        iVar2 = (*(int *)(param_1 + 0x20) - iVar3) + -1;
        if (0 < iVar2) {
          pvVar1 = (void *)(*(int *)(param_1 + 0x14) + iVar3 * 4);
          FUN_10009be0(pvVar1,(void *)((int)pvVar1 + 4),iVar2 * 4);
        }
        *(int *)(param_1 + 0x20) = *(int *)(param_1 + 0x20) + -1;
        iVar3 = iVar3 + -1;
      }
      iVar3 = iVar3 + 1;
    } while (iVar3 < *(int *)(param_1 + 0x20));
  }
  return;
}


// 1000d370 FUN_1000d370

void __fastcall FUN_1000d370(int *param_1)

{
  int iVar1;
  
  param_1[3] = 0;
  if (-1 < param_1[2]) {
    if (*param_1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
      *param_1 = 0;
    }
    param_1[1] = 0;
  }
  iVar1 = *param_1;
  param_1[4] = iVar1;
  if (-1 < param_1[2]) {
    if (iVar1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
      *param_1 = 0;
    }
    param_1[1] = 0;
  }
  return;
}


// 1000d3e0 FUN_1000d3e0

uint __thiscall FUN_1000d3e0(void *this,int *param_1)

{
  int iVar1;
  int *piVar2;
  uint uVar3;
  int *piVar4;
  
  iVar1 = *(int *)((int)this + 0xc);
  uVar3 = 0;
  if (0 < iVar1) {
                    /* WARNING: Load size is inaccurate */
    piVar2 = *this;
    piVar4 = piVar2;
    do {
      if (*piVar4 == *param_1) {
        if (uVar3 != 0xffffffff) {
          if (0 < iVar1) {
            if (uVar3 != iVar1 - 1U) {
              piVar2[uVar3] = piVar2[iVar1 + -1];
            }
            *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + -1;
          }
          return CONCAT31((int3)(uVar3 >> 8),1);
        }
        break;
      }
      uVar3 = uVar3 + 1;
      piVar4 = piVar4 + 1;
    } while ((int)uVar3 < iVar1);
  }
  return uVar3 & 0xffffff00;
}


// 1000d430 FUN_1000d430

void __fastcall FUN_1000d430(int *param_1)

{
  int *piVar1;
  int iVar2;
  
  iVar2 = 0;
  if (0 < param_1[3]) {
    do {
      piVar1 = *(int **)(*param_1 + iVar2 * 4);
      if (piVar1 != (int *)0x0) {
        (**(code **)(*piVar1 + 0x10))(1);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_1[3]);
  }
  param_1[3] = 0;
  if (param_1[2] < 0) {
    param_1[4] = *param_1;
    return;
  }
  if (*param_1 != 0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
    *param_1 = 0;
  }
  param_1[1] = 0;
  param_1[4] = *param_1;
  return;
}


// 1000d4a0 FUN_1000d4a0

void __fastcall FUN_1000d4a0(int param_1)

{
  void *this;
  undefined4 *puVar1;
  int *piVar2;
  
  DevMsg(2,"Server manager refresh completed.\n");
  *(undefined4 *)(param_1 + 0x3c) = 0;
  if (*(char *)(param_1 + 8) == '\0') {
    FUN_1000d430((int *)(param_1 + 0x14));
    FUN_1000d430((int *)(param_1 + 0x28));
  }
  else {
    FUN_1000d300(param_1);
  }
  this = (void *)FUN_100054d0(0x24);
  if (this == (void *)0x0) {
    puVar1 = (undefined4 *)0x0;
  }
  else {
    puVar1 = FUN_100068f0(this,"OnMatchServerMgrUpdate");
  }
  piVar2 = (int *)(**(code **)(*DAT_10037be4 + 0x1c))();
  (**(code **)(*piVar2 + 8))(puVar1);
  return;
}


// 1000d520 FUN_1000d520

undefined4 * __thiscall FUN_1000d520(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_100288c0;
  *(undefined ***)((int)this + 4) = &PTR_FUN_100288b8;
  FUN_1000d430((int *)((int)this + 0x14));
  FUN_1000d430((int *)((int)this + 0x28));
  FUN_1000d370((int *)((int)this + 0x4c));
  FUN_1000d370((int *)((int)this + 0x28));
  FUN_1000d370((int *)((int)this + 0x14));
  if ((param_1 & 1) != 0) {
    FUN_100012d0(this);
  }
  return (undefined4 *)this;
}


// 1000d580 FUN_1000d580

void __thiscall FUN_1000d580(void *this,char param_1)

{
  *(undefined4 *)((int)this + 0xc) = 0;
  *(char *)((int)this + 8) = param_1;
  if (param_1 == '\0') {
    FUN_1000d430((int *)((int)this + 0x14));
    FUN_1000d430((int *)((int)this + 0x28));
    FUN_1000d4a0((int)this);
  }
  return;
}


// 1000d5b0 FUN_1000d5b0

void __thiscall FUN_1000d5b0(void *this,undefined4 *param_1)

{
  byte *pbVar1;
  uint uVar2;
  int *piVar3;
  int *piVar4;
  char *pcVar5;
  undefined4 uVar6;
  undefined4 *puVar7;
  undefined4 *puVar8;
  int iVar9;
  void *pvVar10;
  undefined4 extraout_ECX;
  undefined4 extraout_ECX_00;
  undefined4 extraout_EDX;
  undefined4 extraout_EDX_00;
  int unaff_EBX;
  int unaff_ESI;
  int unaff_EDI;
  float10 fVar11;
  ulonglong uVar12;
  longlong lVar13;
  void *unaff_retaddr;
  int local_4c;
  int *local_44;
  int *local_40;
  char *local_3c [3];
  undefined1 auStack_30 [8];
  undefined1 auStack_28 [14];
  char cStack_1a;
  undefined4 *apuStack_8 [2];
  
  pbVar1 = FUN_10005080(param_1);
  uVar2 = FUN_100098d0(pbVar1,(byte *)"OnNetLanConnectionlessPacket");
  if (uVar2 == 0) {
    local_3c[0] = "LanSearchServerPing";
    local_3c[1] = "ConnectServerDetailsRequest";
    local_3c[2] = "InetSearchServerDetails";
    uVar2 = 0;
    do {
      piVar3 = FUN_100058e0(param_1,local_3c[uVar2],0);
      if (piVar3 != (int *)0x0) {
        piVar4 = (int *)(**(code **)(*DAT_10037be4 + 0x28))();
        pvVar10 = (void *)(**(code **)(*piVar4 + 4))();
        if (pvVar10 == (void *)0x0) {
          return;
        }
        piVar4 = FUN_100058e0(pvVar10,"Server",0);
        if (piVar4 != (int *)0x0) {
          uVar6 = 1;
          piVar4 = FUN_100058e0(pvVar10,local_3c[uVar2 - 1],1);
          FUN_100060d0(piVar4,piVar3,uVar6);
          uVar6 = 1;
          pcVar5 = (char *)FUN_100069e0(param_1,"from",0x1002536c);
          FUN_1000e0e0(pvVar10,pcVar5,uVar6);
        }
        FUN_10005ff0((int)pvVar10);
        return;
      }
      uVar2 = uVar2 + 1;
    } while (uVar2 < 3);
    piVar3 = FUN_100058e0(param_1,"GameDetailsServer",0);
    if (piVar3 != (int *)0x0) {
      local_4c = 0;
      local_40 = piVar3;
      fVar11 = FUN_10005be0(piVar3,"LanSearchServerPing/timestamp",0.0);
      local_44 = (int *)(float)fVar11;
      if ((float10)0 == fVar11) {
        lVar13 = FUN_10005af0(piVar3,"InetSearchServerDetails/pingxuid",0,0);
        if (lVar13 == 0) {
          iVar9 = FUN_100069e0(piVar3,"ConnectServerDetailsRequest/server",0x1002536c);
          if (iVar9 == 0) {
            return;
          }
          (**(code **)(**(int **)(PTR_PTR_10035700 + 0x54) + 0x2c))();
          return;
        }
        piVar4 = FUN_1000d290((int *)((int)this + 0x24),(int)lVar13,(int)((ulonglong)lVar13 >> 0x20)
                             );
        if (piVar4 == (int *)0x0) {
          return;
        }
        fVar11 = FUN_10005be0(piVar3,"InetSearchServerDetails/timestamp",0.0);
        local_44 = (int *)(float)fVar11;
        if ((float10)0 != fVar11) {
          Plat_FloatTime();
          uVar12 = FUN_1000ffe0(extraout_ECX_00,extraout_EDX_00);
          local_4c = (int)uVar12;
          if (local_4c < 0) {
            local_4c = 0;
          }
          else if (999 < local_4c) {
            local_4c = 999;
          }
        }
        local_44 = piVar4;
        FUN_1000d3e0((int *)((int)this + 0x24),(int *)&local_44);
        (**(code **)(*piVar4 + 0x10))();
      }
      else {
        Plat_FloatTime();
        uVar12 = FUN_1000ffe0(extraout_ECX,extraout_EDX);
        local_4c = (int)uVar12;
        if (local_4c < 0) {
          local_4c = 0;
        }
        else if (999 < local_4c) {
          local_4c = 999;
        }
      }
      pcVar5 = (char *)FUN_100069e0(piVar3,"Server/adronline",0);
      if ((pcVar5 != (char *)0x0) && (*pcVar5 != '\0')) {
        FUN_1000ae50(local_3c,0);
        FUN_1000ae70(local_3c,0);
        FUN_1000ae20(local_3c,3);
        FUN_1000ae20(local_3c,3);
        FUN_1000ae70(local_3c,0x6987);
        FUN_1000ae80(local_3c,pcVar5,'\0');
        FUN_10002d50(auStack_30);
        (**(code **)(**(int **)(PTR_PTR_10035700 + 0x54) + 0x18))();
        if ((cStack_1a == '\0') ||
           ((uVar6 = FUN_1000acd0(local_3c + 2,(int *)&local_40,'\0'), (char)uVar6 == '\0' &&
            (uVar6 = FUN_1000acd0(auStack_28,(int *)&local_40,'\0'), (char)uVar6 == '\0')))) {
          FUN_1000ae60((int)&local_40);
          lVar13 = __allshl(0x20,0);
          uVar6 = (undefined4)((ulonglong)lVar13 >> 0x20);
          uVar2 = FUN_1000ae40((int)&local_40);
          uVar2 = uVar2 | (uint)lVar13;
          puVar7 = FUN_10005e40(local_44);
          FUN_10005350(puVar7,"Settings");
          piVar3 = FUN_100058e0(puVar7,"LanSearch",0);
          if (piVar3 != (int *)0x0) {
            FUN_10005240(puVar7,(int)piVar3);
            FUN_10005ff0((int)piVar3);
          }
          FUN_10005db0(puVar7,"Server/ping",unaff_EBX);
          pcVar5 = (char *)FUN_100069e0(unaff_retaddr,"from",0);
          if (pcVar5 != (char *)0x0) {
            FUN_10005cb0(puVar7,"Server/connectstring",pcVar5);
          }
          puVar8 = (undefined4 *)(**(code **)(*(int *)(local_4c + -4) + 0xc))(uVar2,uVar6);
          if (puVar8 == (undefined4 *)0x0) {
            puVar8 = (undefined4 *)FUN_10001270(0x18);
            if (puVar8 == (undefined4 *)0x0) {
              puVar8 = (undefined4 *)0x0;
            }
            else {
              puVar8 = FUN_1000d190(puVar8);
            }
            puVar8[2] = uVar2;
            puVar8[3] = uVar6;
            apuStack_8[0] = puVar8;
            FUN_10008ec0((void *)(unaff_ESI + 0x10),*(int *)(unaff_ESI + 0x1c),apuStack_8);
          }
          if (puVar8[4] != 0) {
            iVar9 = FUN_10005a70((void *)puVar8[4],"Server/ping",0);
            if (iVar9 < unaff_EDI) {
              FUN_10005db0(puVar7,"Server/ping",(unaff_EDI + iVar9 * 9) / 10);
            }
            FUN_10005ff0(puVar8[4]);
          }
          puVar8[4] = puVar7;
          fVar11 = (float10)Plat_FloatTime();
          puVar8[1] = (float)fVar11;
          pvVar10 = (void *)FUN_100054d0(0x24);
          if (pvVar10 == (void *)0x0) {
            puVar7 = (undefined4 *)0x0;
          }
          else {
            puVar7 = FUN_100068f0(pvVar10,"OnMatchServerMgrUpdate");
          }
          FUN_10005dd0(puVar7,"xuid",uVar2,uVar6);
          piVar3 = (int *)(**(code **)(*DAT_10037be4 + 0x1c))();
          (**(code **)(*piVar3 + 8))(puVar7);
        }
      }
    }
  }
  else {
    uVar2 = FUN_100098d0((byte *)"Client::ResendGameDetailsRequest",pbVar1);
    if (uVar2 == 0) {
      pvVar10 = (void *)FUN_100054d0(0x24);
      if (pvVar10 == (void *)0x0) {
        puVar7 = (undefined4 *)0x0;
      }
      else {
        puVar7 = FUN_10005890(pvVar10,"ConnectServerDetailsRequest");
      }
      pcVar5 = (char *)FUN_100069e0(param_1,"to",0x1002536c);
      FUN_10005cb0(puVar7,"server",pcVar5);
      uVar6 = 0;
      pcVar5 = (char *)FUN_100069e0(param_1,"to",0x1002536c);
      FUN_1000e0e0(puVar7,pcVar5,uVar6);
      if (puVar7 != (undefined4 *)0x0) {
        FUN_10005ff0((int)puVar7);
        return;
      }
    }
  }
  return;
}


// 1000db20 FUN_1000db20

void FUN_1000db20(void)

{
  if (DAT_10039da0 != (int *)0x0) {
    (**(code **)(*DAT_10039da0 + 0x18))();
  }
  if (PTR_PTR_10035970 != (undefined *)0x0) {
    FUN_10004c90();
  }
  if (PTR_PTR_10035900 != (undefined *)0x0) {
    FUN_1000d120((int)PTR_PTR_10035900);
    return;
  }
  return;
}


// 1000db50 FUN_1000db50

void __fastcall FUN_1000db50(int *param_1)

{
  param_1[3] = 0;
  param_1[4] = 0;
  *(undefined1 *)(param_1 + 5) = 0;
  param_1[8] = 0;
  param_1[7] = -1;
  FUN_1000b590(param_1);
  return;
}


// 1000db90 FUN_1000db90

undefined4 * __thiscall FUN_1000db90(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  bool bVar3;
  uint uVar4;
  void *this_00;
  undefined4 uVar5;
  int *this_01;
  int iVar6;
  int *piVar7;
  undefined4 *this_02;
  uint uVar8;
  uint local_4;
  
  puVar2 = param_1;
  if (((param_1 == (undefined4 *)0x0) || (param_1[0x10] == 0)) || (param_1[6] == 0)) {
    return (undefined4 *)0x0;
  }
  iVar6 = param_1[0xc];
  this_02 = param_1 + 7;
  if (iVar6 < 0x20) {
    uVar8 = param_1[0xb];
    iVar6 = 0x20 - iVar6;
    puVar1 = (undefined4 *)param_1[0xd];
    if (puVar1 == (undefined4 *)param_1[0xe]) {
      param_1[0xc] = 1;
      param_1[0xb] = 0;
      *(undefined1 *)(param_1 + 8) = 1;
LAB_1000dc52:
      param_1[0xd] = puVar1 + 1;
    }
    else {
      if (puVar1 <= (undefined4 *)param_1[0xe]) {
        param_1[0xb] = *puVar1;
        goto LAB_1000dc52;
      }
      *(undefined1 *)(param_1 + 8) = 1;
      param_1[0xb] = 0;
    }
    if (*(char *)(param_1 + 8) == '\0') {
      uVar4 = *(uint *)(&DAT_1002aa78 + iVar6 * 4);
      uVar5 = param_1[0xc];
      param_1[0xc] = 0x20 - iVar6;
      uVar8 = uVar8 | (uVar4 & param_1[0xb]) << ((byte)uVar5 & 0x1f);
      param_1[0xb] = (uint)param_1[0xb] >> ((byte)iVar6 & 0x1f);
      goto LAB_1000dc81;
    }
  }
  else {
    uVar8 = param_1[0xb];
    param_1[0xc] = iVar6 + -0x20;
    if (iVar6 + -0x20 == 0) {
      puVar1 = (undefined4 *)param_1[0xd];
      param_1[0xc] = 0x20;
      if (puVar1 == (undefined4 *)param_1[0xe]) {
        param_1[0xc] = 1;
        param_1[0xb] = 0;
        param_1[0xd] = puVar1 + 1;
      }
      else if ((undefined4 *)param_1[0xe] < puVar1) {
        *(undefined1 *)(param_1 + 8) = 1;
        param_1[0xb] = 0;
      }
      else {
        param_1[0xb] = *puVar1;
        param_1[0xd] = puVar1 + 1;
      }
    }
    else {
      param_1[0xb] = 0;
    }
LAB_1000dc81:
    if (uVar8 != 0) {
      return (undefined4 *)0x0;
    }
  }
  iVar6 = param_1[0xc];
  piVar7 = *(int **)(PTR_PTR_10035700 + 0x54);
  if (iVar6 < 0x20) {
    uVar8 = param_1[0xb];
    iVar6 = 0x20 - iVar6;
    puVar1 = (undefined4 *)param_1[0xd];
    if (puVar1 == (undefined4 *)param_1[0xe]) {
      param_1[0xc] = 1;
      param_1[0xb] = 0;
      *(undefined1 *)(param_1 + 8) = 1;
LAB_1000dd2f:
      param_1[0xd] = puVar1 + 1;
    }
    else {
      if (puVar1 <= (undefined4 *)param_1[0xe]) {
        param_1[0xb] = *puVar1;
        goto LAB_1000dd2f;
      }
      *(undefined1 *)(param_1 + 8) = 1;
      param_1[0xb] = 0;
    }
    if (*(char *)(param_1 + 8) == '\0') {
      uVar4 = *(uint *)(&DAT_1002aa78 + iVar6 * 4);
      uVar5 = param_1[0xc];
      param_1[0xc] = 0x20 - iVar6;
      uVar8 = uVar8 | (uVar4 & param_1[0xb]) << ((byte)uVar5 & 0x1f);
      param_1[0xb] = (uint)param_1[0xb] >> ((byte)iVar6 & 0x1f);
    }
    else {
      uVar8 = 0;
    }
  }
  else {
    uVar8 = param_1[0xb];
    param_1[0xc] = iVar6 + -0x20;
    if (iVar6 + -0x20 == 0) {
      puVar1 = (undefined4 *)param_1[0xd];
      param_1[0xc] = 0x20;
      if (puVar1 == (undefined4 *)param_1[0xe]) {
        param_1[0xc] = 1;
        param_1[0xb] = 0;
        param_1[0xd] = puVar1 + 1;
      }
      else if ((undefined4 *)param_1[0xe] < puVar1) {
        *(undefined1 *)(param_1 + 8) = 1;
        param_1[0xb] = 0;
      }
      else {
        param_1[0xb] = *puVar1;
        param_1[0xd] = puVar1 + 1;
      }
    }
    else {
      param_1[0xb] = 0;
    }
  }
  uVar4 = (**(code **)(*piVar7 + 0x14))();
  if (uVar8 != uVar4) {
    return (undefined4 *)0x0;
  }
  iVar6 = param_1[0xc];
  if (iVar6 < 0x20) {
    uVar8 = param_1[0xb];
    iVar6 = 0x20 - iVar6;
    puVar1 = (undefined4 *)param_1[0xd];
    if (puVar1 == (undefined4 *)param_1[0xe]) {
      param_1[0xc] = 1;
      param_1[0xb] = 0;
      *(undefined1 *)(param_1 + 8) = 1;
LAB_1000de13:
      param_1[0xd] = puVar1 + 1;
    }
    else {
      if (puVar1 <= (undefined4 *)param_1[0xe]) {
        param_1[0xb] = *puVar1;
        goto LAB_1000de13;
      }
      *(undefined1 *)(param_1 + 8) = 1;
      param_1[0xb] = 0;
    }
    if (*(char *)(param_1 + 8) != '\0') {
      return (undefined4 *)0x0;
    }
    uVar4 = *(uint *)(&DAT_1002aa78 + iVar6 * 4);
    uVar5 = param_1[0xc];
    param_1[0xc] = 0x20 - iVar6;
    uVar8 = uVar8 | (uVar4 & param_1[0xb]) << ((byte)uVar5 & 0x1f);
    param_1[0xb] = (uint)param_1[0xb] >> ((byte)iVar6 & 0x1f);
  }
  else {
    uVar8 = param_1[0xb];
    param_1[0xc] = iVar6 + -0x20;
    if (iVar6 + -0x20 == 0) {
      puVar1 = (undefined4 *)param_1[0xd];
      param_1[0xc] = 0x20;
      if (puVar1 == (undefined4 *)param_1[0xe]) {
        param_1[0xc] = 1;
        param_1[0xb] = 0;
        param_1[0xd] = puVar1 + 1;
      }
      else if ((undefined4 *)param_1[0xe] < puVar1) {
        *(undefined1 *)(param_1 + 8) = 1;
        param_1[0xb] = 0;
      }
      else {
        param_1[0xb] = *puVar1;
        param_1[0xd] = puVar1 + 1;
      }
    }
    else {
      param_1[0xb] = 0;
    }
  }
  if ((int)uVar8 < 1) {
    return (undefined4 *)0x0;
  }
  if (0x3ff < (int)uVar8) {
    return (undefined4 *)0x0;
  }
  piVar7 = (int *)((int)this + 4);
  FUN_1000db50(piVar7);
  FUN_1000b6c0(piVar7,uVar8);
  bVar3 = FUN_1001f120(this_02,(uint *)*piVar7,uVar8);
  if (!bVar3) {
    return (undefined4 *)0x0;
  }
  FUN_1000baf0(piVar7,0,uVar8);
  FUN_1000b390(piVar7,'\x01');
  this_00 = (void *)FUN_100054d0(0x24);
  if (this_00 == (void *)0x0) {
    param_1 = (undefined4 *)0x0;
  }
  else {
    param_1 = FUN_10005890(this_00,&DAT_1002536c);
  }
  uVar5 = FUN_100084f0(param_1,piVar7);
  if ((char)uVar5 == '\0') goto LAB_1000e056;
  this_01 = FUN_100058e0(param_1,"binary",0);
  if (this_01 == (int *)0x0) {
    local_4 = 0;
  }
  else {
    local_4 = FUN_10005a70(this_01,"size",0);
    FUN_10005240(param_1,(int)this_01);
    FUN_10005ff0((int)this_01);
  }
  iVar6 = puVar2[0xc];
  if (iVar6 < 0x20) {
    uVar8 = puVar2[0xb];
    iVar6 = 0x20 - iVar6;
    puVar1 = (undefined4 *)puVar2[0xd];
    if (puVar1 == (undefined4 *)puVar2[0xe]) {
      puVar2[0xc] = 1;
      puVar2[0xb] = 0;
      *(undefined1 *)(puVar2 + 8) = 1;
LAB_1000dfb6:
      puVar2[0xd] = puVar1 + 1;
    }
    else {
      if (puVar1 <= (undefined4 *)puVar2[0xe]) {
        puVar2[0xb] = *puVar1;
        goto LAB_1000dfb6;
      }
      *(undefined1 *)(puVar2 + 8) = 1;
      puVar2[0xb] = 0;
    }
    if (*(char *)(puVar2 + 8) == '\0') {
      uVar4 = *(uint *)(&DAT_1002aa78 + iVar6 * 4);
      uVar5 = puVar2[0xc];
      puVar2[0xc] = 0x20 - iVar6;
      uVar8 = uVar8 | (uVar4 & puVar2[0xb]) << ((byte)uVar5 & 0x1f);
      puVar2[0xb] = (uint)puVar2[0xb] >> ((byte)iVar6 & 0x1f);
    }
    else {
      uVar8 = 0;
    }
  }
  else {
    uVar8 = puVar2[0xb];
    puVar2[0xc] = iVar6 + -0x20;
    if (iVar6 + -0x20 == 0) {
      puVar1 = (undefined4 *)puVar2[0xd];
      puVar2[0xc] = 0x20;
      if (puVar1 == (undefined4 *)puVar2[0xe]) {
        puVar2[0xb] = 0;
        puVar2[0xc] = 1;
        puVar2[0xd] = puVar1 + 1;
      }
      else if ((undefined4 *)puVar2[0xe] < puVar1) {
        puVar2[0xb] = 0;
        *(undefined1 *)(puVar2 + 8) = 1;
      }
      else {
        puVar2[0xb] = *puVar1;
        puVar2[0xd] = puVar1 + 1;
      }
    }
    else {
      puVar2[0xb] = 0;
    }
  }
  if (((uVar8 == local_4) && (-1 < (int)uVar8)) && ((int)uVar8 < 0x400)) {
    if ((int)uVar8 < 1) {
      return param_1;
    }
    FUN_1000db50(piVar7);
    FUN_1000b6c0(piVar7,uVar8);
    bVar3 = FUN_1001f120(this_02,(uint *)*piVar7,uVar8);
    if (bVar3) {
      FUN_1000baf0(piVar7,0,uVar8);
      FUN_10005e20(param_1,"Binary/ptr",*piVar7);
      FUN_10005db0(param_1,"Binary/size",uVar8);
      return param_1;
    }
  }
LAB_1000e056:
  FUN_10005ff0((int)param_1);
  return (undefined4 *)0x0;
}


// 1000e070 FUN_1000e070

undefined4 __thiscall FUN_1000e070(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  void *this_00;
  undefined4 *this_01;
  char *pcVar2;
  undefined4 uVar3;
  
  puVar1 = FUN_1000db90(this,param_1);
  if (puVar1 == (undefined4 *)0x0) {
    return 0;
  }
  this_00 = (void *)FUN_100054d0(0x24);
  if (this_00 == (void *)0x0) {
    this_01 = (undefined4 *)0x0;
  }
  else {
    this_01 = FUN_10005890(this_00,"OnNetLanConnectionlessPacket");
  }
  pcVar2 = FUN_1000ad30(param_1,'\0');
  FUN_10005cb0(this_01,"from",pcVar2);
  FUN_10005a40(this_01,puVar1);
  uVar3 = (**(code **)(*(int *)PTR_DAT_10035b64 + 8))(this_01);
  return CONCAT31((int3)((uint)uVar3 >> 8),1);
}


// 1000e0e0 FUN_1000e0e0

void FUN_1000e0e0(void *param_1,char *param_2,undefined4 param_3)

{
  char cVar1;
  undefined2 uVar2;
  uint uVar3;
  int *this;
  int *this_00;
  uint *puVar4;
  int iVar6;
  undefined4 auStack_504 [3];
  undefined4 local_4f8 [3];
  int iStack_4ec;
  uint *apuStack_4e0 [2];
  int iStack_4d8;
  uint uStack_4c4;
  undefined1 local_4b0 [1200];
  long lVar5;
  
  FUN_1001f480(local_4f8,local_4b0,0x4b0,-1);
  FUN_1001f560(local_4f8,0xffffffff);
  FUN_1001f560(local_4f8,0);
  uVar3 = (**(code **)(**(int **)(PTR_PTR_10035700 + 0x54) + 0x14))();
  FUN_1001f560(local_4f8,uVar3);
  FUN_1000bc50(apuStack_4e0,0,0,0);
  FUN_1000b390(apuStack_4e0,'\x01');
  FUN_10007b30(param_1,(int *)apuStack_4e0);
  FUN_1001f560(local_4f8,uStack_4c4);
  FUN_1001f460(local_4f8,apuStack_4e0[0],uStack_4c4);
  this = FUN_100058e0(param_1,"Binary/ptr",0);
  this_00 = FUN_100058e0(param_1,"Binary/size",0);
  if ((this == (int *)0x0) || (this_00 == (int *)0x0)) {
LAB_1000e1f2:
    FUN_1001f560(local_4f8,0);
  }
  else {
    puVar4 = (uint *)FUN_10005bb0(this,(char *)0x0,0);
    uVar3 = FUN_10005a70(this_00,(char *)0x0,0);
    if ((puVar4 == (uint *)0x0) || (uVar3 == 0)) goto LAB_1000e1f2;
    FUN_1001f560(local_4f8,uVar3);
    cVar1 = FUN_1001f460(local_4f8,puVar4,uVar3);
    if (cVar1 == '\0') {
      FUN_100076e0((int *)apuStack_4e0);
      return;
    }
  }
  FUN_1000ae50(auStack_504,0);
  FUN_1000ae70(auStack_504,0);
  FUN_1000ae20(auStack_504,3);
  if (param_2 == (char *)0x0) {
    FUN_1000ae20(auStack_504,2);
    uVar2 = 0;
  }
  else {
    if ((*param_2 != '*') || (param_2[1] != ':')) {
      FUN_1000ae80(auStack_504,param_2,'\0');
      goto LAB_1000e272;
    }
    FUN_1000ae20(auStack_504,2);
    lVar5 = _atol(param_2 + 2);
    uVar2 = (undefined2)lVar5;
  }
  FUN_1000ae70(auStack_504,uVar2);
LAB_1000e272:
  iVar6 = FUN_1000ae30(auStack_504);
  if ((iVar6 != 2) || (*(int *)(DAT_10039c7c + 0x30) != 0)) {
    (**(code **)(**(int **)(PTR_PTR_10035700 + 0x54) + 0x38))
              (0,param_3,auStack_504,local_4f8[0],iStack_4ec + 7 >> 3,0,0);
  }
  if ((-1 < iStack_4d8) && (apuStack_4e0[0] != (uint *)0x0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(apuStack_4e0[0]);
  }
  return;
}


// 1000e2f0 FUN_1000e2f0

undefined4 * __thiscall FUN_1000e2f0(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_10028e1c;
  if (-1 < *(int *)((int)this + 0xc)) {
    if (*(int *)((int)this + 4) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)this + 4));
      *(undefined4 *)((int)this + 4) = 0;
    }
    *(undefined4 *)((int)this + 8) = 0;
  }
  *(undefined ***)this = &PTR_LAB_10028de8;
  if ((param_1 & 1) != 0) {
    FUN_100012d0(this);
  }
  return (undefined4 *)this;
}


// 1000e340 FUN_1000e340

void __thiscall FUN_1000e340(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  undefined4 uVar3;
  uint uVar4;
  
  iVar1 = *(int *)((int)this + 8);
  if (iVar1 < 0) {
    return;
  }
  iVar2 = *(int *)((int)this + 4);
  uVar4 = param_1 + iVar2;
  if (iVar1 == 0) {
    if (iVar2 == 0) {
      iVar2 = 8;
    }
    if ((int)uVar4 <= iVar2) goto LAB_1000e39b;
    do {
      iVar2 = iVar2 * 2;
    } while (iVar2 < (int)uVar4);
  }
  else {
    iVar2 = ((int)(uVar4 - 1) / iVar1 + 1) * iVar1;
  }
  if (iVar2 < (int)uVar4) {
    if ((iVar2 == 0) && (0x7fffffff < uVar4)) {
      iVar2 = -1;
    }
    else {
      do {
        iVar2 = (int)(iVar2 + uVar4) / 2;
      } while (iVar2 < (int)uVar4);
    }
  }
LAB_1000e39b:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 * 4);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 1000e450 FUN_1000e450

void __thiscall FUN_1000e450(void *this,int param_1)

{
  void *pvVar1;
  int iVar2;
  
  iVar2 = (*(int *)((int)this + 0xc) - param_1) + -1;
  if (0 < iVar2) {
                    /* WARNING: Load size is inaccurate */
    pvVar1 = (void *)(*this + param_1 * 4);
    FUN_10009be0(pvVar1,(void *)((int)pvVar1 + 4),iVar2 * 4);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + -1;
  return;
}


// 1000e520 FUN_1000e520

void __fastcall FUN_1000e520(undefined4 *param_1)

{
  *param_1 = &PTR_FUN_10028e58;
  param_1[1] = 0;
  param_1[2] = 0;
  param_1[3] = 0;
  param_1[4] = 0;
  param_1[5] = 0;
  param_1[6] = 0;
  param_1[7] = 0;
  param_1[8] = 0;
  param_1[9] = 0;
  param_1[10] = 0;
  param_1[0xb] = 0;
  param_1[0xc] = 0;
  param_1[0xd] = 0;
  param_1[0xe] = 0;
  param_1[0xf] = 0;
  *(undefined1 *)(param_1 + 0x10) = 0;
  *(undefined1 *)((int)param_1 + 0x41) = 0;
  param_1[0x11] = 0;
  param_1[0x12] = 0;
  param_1[0x13] = 0;
  param_1[0x14] = 0;
  param_1[0x15] = 0;
  param_1[0x16] = 0;
  param_1[0x17] = 0;
  param_1[0x18] = 0;
  param_1[0x19] = 0;
  param_1[0x1a] = 0;
  param_1[0x1b] = 0;
  param_1[0x1c] = 0;
  param_1[0x1d] = 0;
  param_1[0x1e] = 0;
  param_1[0x1f] = 0;
  return;
}


// 1000e590 FUN_1000e590

int __thiscall FUN_1000e590(void *this,int param_1)

{
  int iVar1;
  int in_EAX;
  int iVar2;
  int *piVar3;
  
  if (param_1 != 0) {
    iVar2 = 0;
    if (0 < *(int *)((int)this + 0x10)) {
      piVar3 = *(int **)((int)this + 4);
      do {
        if (*piVar3 == param_1) {
          if ((-1 < iVar2) && (iVar2 < *(int *)((int)this + 0x10))) {
            iVar1 = *(int *)((int)this + 0x18);
            piVar3 = (int *)(iVar1 + iVar2 * 4);
            *piVar3 = *piVar3 + 1;
            return iVar1 + iVar2 * 4;
          }
          break;
        }
        iVar2 = iVar2 + 1;
        piVar3 = piVar3 + 1;
      } while (iVar2 < *(int *)((int)this + 0x10));
    }
    FUN_10008ec0((undefined4 *)((int)this + 4),*(int *)((int)this + 0x10),&param_1);
    param_1 = 1;
    in_EAX = FUN_10008ec0((void *)((int)this + 0x18),*(int *)((int)this + 0x24),&param_1);
  }
  return in_EAX;
}


// 1000e610 FUN_1000e610

void __thiscall FUN_1000e610(void *this,undefined4 *param_1)

{
  int *this_00;
  int *this_01;
  int *piVar1;
  undefined4 uVar2;
  undefined4 *this_02;
  byte *pbVar3;
  uint uVar4;
  int iVar5;
  
  this_02 = param_1;
  pbVar3 = FUN_10005080(param_1);
  uVar4 = FUN_100098d0((byte *)"OnNetLanConnectionlessPacket",pbVar3);
  if ((uVar4 == 0) && (iVar5 = FUN_10005bb0(this_02,"rawpkt",0), iVar5 != 0)) {
    FUN_10005ff0((int)this_02);
    (**(code **)(*(int *)PTR_PTR_10035ac0 + 4))(iVar5);
    return;
  }
  if ((*(char *)((int)this + 0x40) != '\0') && (*(char *)((int)this + 0x41) == '\0')) {
    FUN_10008ec0((void *)((int)this + 0x44),*(int *)((int)this + 0x50),&param_1);
    return;
  }
  this_00 = (int *)((int)this + 0x6c);
  *(undefined1 *)((int)this + 0x40) = 1;
  FUN_10008ec0(this_00,0,&param_1);
  (*(code *)**(undefined4 **)(PTR_DAT_10035810 + 4))(this_02);
  this_01 = (int *)((int)this + 0x2c);
  FUN_10008ec0(this_01,*(int *)((int)this + 0x38),(undefined4 *)&stack0x00000000);
  if (*(int *)(*this_01 + -4 + *(int *)((int)this + 0x38) * 4) < *(int *)((int)this + 0x10)) {
    do {
      (**(code **)**(undefined4 **)
                    (*(int *)((int)this + 4) +
                    *(int *)(*this_01 + -4 + *(int *)((int)this + 0x38) * 4) * 4))(this_02);
      piVar1 = (int *)(*this_01 + -4 + *(int *)((int)this + 0x38) * 4);
      *piVar1 = *piVar1 + 1;
    } while (*(int *)(*this_01 + -4 + *(int *)((int)this + 0x38) * 4) < *(int *)((int)this + 0x10));
  }
  *(int *)((int)this + 0x38) = *(int *)((int)this + 0x38) + -1;
  *(undefined1 *)((int)this + 0x40) = 0;
  if (*(int *)((int)this + 0x50) < 1) {
    iVar5 = 0;
    if (0 < *(int *)((int)this + 100)) {
      do {
        FUN_10005ff0(*(int *)(*(int *)((int)this + 0x58) + iVar5 * 4));
        iVar5 = iVar5 + 1;
      } while (iVar5 < *(int *)((int)this + 100));
    }
    *(undefined4 *)((int)this + 100) = 0;
    if (-1 < *(int *)((int)this + 0x60)) {
      if (*(int *)((int)this + 0x58) != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)this + 0x58));
        *(undefined4 *)((int)this + 0x58) = 0;
      }
      *(undefined4 *)((int)this + 0x5c) = 0;
    }
    *(undefined4 *)((int)this + 0x68) = *(undefined4 *)((int)this + 0x58);
    *(undefined4 *)((int)this + 0x78) = 0;
    if (-1 < *(int *)((int)this + 0x74)) {
      if (*this_00 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*this_00);
        *this_00 = 0;
      }
      *(undefined4 *)((int)this + 0x70) = 0;
    }
    *(int *)((int)this + 0x7c) = *this_00;
  }
  else {
    uVar2 = **(undefined4 **)((int)this + 0x44);
    iVar5 = *(int *)((int)this + 0x50) + -1;
    if (0 < iVar5) {
      FUN_10009be0(*(void **)((int)this + 0x44),(void *)((int)*(void **)((int)this + 0x44) + 4),
                   iVar5 * 4);
    }
    *(int *)((int)this + 0x50) = *(int *)((int)this + 0x50) + -1;
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 8))(uVar2);
  }
  if (this_02 != (undefined4 *)0x0) {
    FUN_10005ff0((int)this_02);
  }
  return;
}


// 1000e7f0 FUN_1000e7f0

void __thiscall FUN_1000e7f0(void *this,undefined4 *param_1)

{
  int *piVar1;
  byte *pbVar2;
  byte *pbVar3;
  uint uVar4;
  int iVar5;
  
  if (((param_1 != (undefined4 *)0x0) && (pbVar2 = FUN_10005080(param_1), pbVar2 != (byte *)0x0)) &&
     (*pbVar2 != 0)) {
    iVar5 = 0;
    if (0 < *(int *)((int)this + 100)) {
      do {
        piVar1 = (int *)(*(int *)((int)this + 0x58) + iVar5 * 4);
        pbVar3 = FUN_10005080(*(undefined4 **)(*(int *)((int)this + 0x58) + iVar5 * 4));
        uVar4 = FUN_100098d0(pbVar2,pbVar3);
        if (uVar4 == 0) {
          FUN_10005ff0(*piVar1);
          *piVar1 = (int)param_1;
          return;
        }
        iVar5 = iVar5 + 1;
      } while (iVar5 < *(int *)((int)this + 100));
    }
    FUN_10008ec0((void *)((int)this + 0x58),*(int *)((int)this + 100),&param_1);
  }
  return;
}


// 1000e870 FUN_1000e870

undefined4 * FUN_1000e870(undefined4 *param_1)

{
  undefined4 uVar1;
  undefined4 *puVar2;
  undefined4 local_14 [5];
  
  puVar2 = (undefined4 *)FUN_1000efc0(local_14);
  *param_1 = *puVar2;
  param_1[1] = puVar2[1];
  param_1[2] = puVar2[2];
  uVar1 = puVar2[4];
  param_1[3] = puVar2[3];
  param_1[4] = uVar1;
  return param_1;
}


// 1000e920 FUN_1000e920

undefined4 * FUN_1000e920(void)

{
  char cVar1;
  undefined4 *this;
  char *pcVar2;
  int *piVar3;
  uint uVar4;
  void *this_00;
  undefined4 *puVar5;
  uint uVar6;
  undefined4 unaff_retaddr;
  undefined1 local_30 [8];
  undefined1 auStack_28 [4];
  undefined1 local_24 [10];
  char cStack_1a;
  char cStack_19;
  char cStack_18;
  char cStack_17;
  char *pcStack_14;
  uint uStack_c;
  uint uStack_8;
  
  FUN_1000ae50(local_30,0);
  FUN_1000ae70(local_30,0);
  FUN_1000ae20(local_30,3);
  FUN_1000ae50(local_24,0);
  FUN_1000ae70(local_24,0);
  FUN_1000ae20(local_24,3);
  (**(code **)(**(int **)(PTR_PTR_10035700 + 0x54) + 0x18))(local_30);
  if (cStack_1a == '\0') {
    piVar3 = *(int **)(PTR_PTR_10035700 + 0x5c);
    if (piVar3 == (int *)0x0) {
      return (undefined4 *)0x0;
    }
    cVar1 = (**(code **)(*piVar3 + 0x164))();
    if (cVar1 != '\0') {
      return (undefined4 *)0x0;
    }
    pcVar2 = (char *)(**(code **)(*piVar3 + 0xd4))();
    if (pcVar2 == (char *)0x0) {
      return (undefined4 *)0x0;
    }
    if (*pcVar2 == '\0') {
      return (undefined4 *)0x0;
    }
    this_00 = (void *)FUN_100054d0(0x24);
    if (this_00 == (void *)0x0) {
      return (undefined4 *)0x0;
    }
    this = FUN_10005890(this_00,"GameDetailsClient");
  }
  else {
    this = FUN_10006410("GameDetailsServer",
                        " System {  network LIVE  access public  }  Server {  name =  server =  adronline =  adrlocal =  }  Members {  numSlots #int#0  numPlayers #int#0  } "
                       );
    FUN_10005cb0(this,"Server/name",pcStack_14);
    pcVar2 = "dedicated";
    if (cStack_19 == '\0') {
      pcVar2 = "listen";
    }
    FUN_10005cb0(this,"Server/server",pcVar2);
    pcVar2 = FUN_1000ad30(auStack_28,'\0');
    FUN_10005cb0(this,"Server/adronline",pcVar2);
    pcVar2 = FUN_1000ad30(&stack0xffffffcc,'\0');
    FUN_10005cb0(this,"Server/adrlocal",pcVar2);
    if (((cStack_19 != '\0') && (cStack_18 != '\0')) && (cStack_17 != '\0')) {
      FUN_10005cb0(this,"System/access","friends");
    }
    piVar3 = (int *)(**(code **)(*(int *)PTR_DAT_10035810 + 0x20))();
    uVar4 = (**(code **)(*piVar3 + 0x10))();
    uVar6 = (uVar4 < uStack_c) - 1;
    uStack_c = ~((-1 < (int)uStack_c) - 1) & ~uVar6 & uVar4 | uVar6 & uStack_c;
    FUN_10005db0(this,"Members/numSlots",uStack_c);
    uVar4 = (uStack_c < uStack_8) - 1;
    uStack_8 = uVar4 & uStack_8 | ~((-1 < (int)uStack_8) - 1) & ~uVar4 & uStack_c;
    FUN_10005db0(this,"Members/numPlayers",uStack_8);
  }
  if (this == (undefined4 *)0x0) {
    return (undefined4 *)0x0;
  }
  puVar5 = (undefined4 *)(**(code **)(*(int *)PTR_DAT_10035810 + 0x40))();
  (**(code **)*puVar5)(this,unaff_retaddr);
  return this;
}


// 1000ed80 FUN_1000ed80

void FUN_1000ed80(void)

{
  return;
}


// 1000edb0 FUN_1000edb0

/* WARNING: Removing unreachable block (ram,0x1000ee4e) */
/* WARNING: Removing unreachable block (ram,0x1000ee52) */
/* WARNING: Removing unreachable block (ram,0x1000ee74) */
/* WARNING: Removing unreachable block (ram,0x1000ee98) */
/* WARNING: Removing unreachable block (ram,0x1000eeaf) */
/* WARNING: Removing unreachable block (ram,0x1000eeb7) */
/* WARNING: Removing unreachable block (ram,0x1000eec0) */
/* WARNING: Removing unreachable block (ram,0x1000eee6) */
/* WARNING: Removing unreachable block (ram,0x1000eeea) */
/* WARNING: Removing unreachable block (ram,0x1000eef3) */
/* WARNING: Removing unreachable block (ram,0x1000eef7) */
/* WARNING: Removing unreachable block (ram,0x1000ef06) */
/* WARNING: Removing unreachable block (ram,0x1000ee94) */

undefined4 FUN_1000edb0(undefined4 param_1,undefined4 param_2)

{
  int *piVar1;
  void *pvVar2;
  undefined4 *puVar3;
  undefined8 uVar4;
  int aiStack_114 [69];
  
  piVar1 = (int *)(**(code **)(*DAT_10037be4 + 0x24))();
  if (piVar1 == (int *)0x0) {
    return 0;
  }
  pvVar2 = (void *)(**(code **)(*piVar1 + 4))();
  aiStack_114[0] = 0;
  piVar1 = FUN_10003360(pvVar2,param_1,param_2,aiStack_114);
  if (aiStack_114[0] == 0) {
    return (uint)piVar1 & 0xffffff00;
  }
  puVar3 = (undefined4 *)(**(code **)(*DAT_10039da0 + 4))(0);
  uVar4 = (**(code **)*puVar3)();
  piVar1 = FUN_10003360(pvVar2,(int)uVar4,(int)((ulonglong)uVar4 >> 0x20),
                        (undefined4 *)&stack0xfffffee8);
  return (uint)piVar1 & 0xffffff00;
}


// 1000ef30 FUN_1000ef30

bool __thiscall FUN_1000ef30(void *this,undefined4 param_1,undefined4 param_2)

{
  char cVar1;
  undefined4 uVar2;
  
  uVar2 = FUN_1000edb0(param_1,param_2);
  if ((char)uVar2 != '\0') {
    return false;
  }
                    /* WARNING: Load size is inaccurate */
  cVar1 = (**(code **)(*this + 8))(param_1,param_2);
  return cVar1 == '\0';
}


// 1000efc0 FUN_1000efc0

void __cdecl FUN_1000efc0(undefined4 *param_1)

{
  undefined4 uVar1;
  undefined4 uVar2;
  
  uVar1 = DAT_10035d00;
  *param_1 = DAT_10035cfc;
  uVar2 = DAT_10035d04;
  param_1[1] = uVar1;
  uVar1 = DAT_10035d08;
  param_1[2] = uVar2;
  uVar2 = DAT_10035d0c;
  param_1[3] = uVar1;
  param_1[4] = uVar2;
  return;
}


// 1000f000 _strrchr

/* Library Function - Single Match
    _strrchr
   
   Library: Visual Studio 2005 Release */

char * __cdecl _strrchr(char *_Str,int _Ch)

{
  char cVar1;
  int iVar2;
  char *pcVar3;
  char *pcVar4;
  
  iVar2 = -1;
  do {
    pcVar4 = _Str;
    if (iVar2 == 0) break;
    iVar2 = iVar2 + -1;
    pcVar4 = _Str + 1;
    cVar1 = *_Str;
    _Str = pcVar4;
  } while (cVar1 != '\0');
  iVar2 = -(iVar2 + 1);
  pcVar4 = pcVar4 + -1;
  do {
    pcVar3 = pcVar4;
    if (iVar2 == 0) break;
    iVar2 = iVar2 + -1;
    pcVar3 = pcVar4 + -1;
    cVar1 = *pcVar4;
    pcVar4 = pcVar3;
  } while ((char)_Ch != cVar1);
  pcVar3 = pcVar3 + 1;
  if (*pcVar3 != (char)_Ch) {
    pcVar3 = (char *)0x0;
  }
  return pcVar3;
}


// 1000f030 _memset

/* Library Function - Single Match
    _memset
   
   Libraries: Visual Studio 2005 Debug, Visual Studio 2005 Release, Visual Studio 2008 Debug, Visual
   Studio 2008 Release */

void * __cdecl _memset(void *_Dst,int _Val,size_t _Size)

{
  uint uVar1;
  undefined1 (*pauVar2) [16];
  uint uVar3;
  size_t sVar4;
  uint *puVar5;
  
  if (_Size == 0) {
    return _Dst;
  }
  uVar1 = _Val & 0xff;
  if ((((char)_Val == '\0') && (0xff < _Size)) && (DAT_1004bcfc != 0)) {
    pauVar2 = __VEC_memzero((undefined1 (*) [16])_Dst,_Val,_Size);
    return pauVar2;
  }
  puVar5 = (uint *)_Dst;
  if (3 < _Size) {
    uVar3 = -(int)_Dst & 3;
    sVar4 = _Size;
    if (uVar3 != 0) {
      sVar4 = _Size - uVar3;
      do {
        *(char *)puVar5 = (char)_Val;
        puVar5 = (uint *)((int)puVar5 + 1);
        uVar3 = uVar3 - 1;
      } while (uVar3 != 0);
    }
    uVar1 = uVar1 * 0x1010101;
    _Size = sVar4 & 3;
    uVar3 = sVar4 >> 2;
    if (uVar3 != 0) {
      for (; uVar3 != 0; uVar3 = uVar3 - 1) {
        *puVar5 = uVar1;
        puVar5 = puVar5 + 1;
      }
      if (_Size == 0) {
        return _Dst;
      }
    }
  }
  do {
    *(char *)puVar5 = (char)uVar1;
    puVar5 = (uint *)((int)puVar5 + 1);
    _Size = _Size - 1;
  } while (_Size != 0);
  return _Dst;
}


// 1000f0aa __vsnprintf_l

/* Library Function - Single Match
    __vsnprintf_l
   
   Library: Visual Studio 2005 Release */

int __cdecl
__vsnprintf_l(char *_DstBuf,size_t _MaxCount,char *_Format,_locale_t _Locale,va_list _ArgList)

{
  DWORD *pDVar1;
  int iVar2;
  FILE local_24;
  
  if (_Format == (char *)0x0) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  else if ((_MaxCount == 0) || (_DstBuf != (char *)0x0)) {
    local_24._cnt = 0x7fffffff;
    if (_MaxCount < 0x80000000) {
      local_24._cnt = _MaxCount;
    }
    local_24._flag = 0x42;
    local_24._base = _DstBuf;
    local_24._ptr = _DstBuf;
    iVar2 = FUN_1001136a(&local_24,(byte *)_Format,_Locale,(wchar_t *)_ArgList);
    if (_DstBuf != (char *)0x0) {
      local_24._cnt = local_24._cnt - 1;
      if (local_24._cnt < 0) {
        __flsbuf(0,&local_24);
      }
      else {
        *local_24._ptr = '\0';
      }
    }
  }
  else {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  return iVar2;
}


// 1000f15b __vsnprintf

/* Library Function - Single Match
    __vsnprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl __vsnprintf(char *_Dest,size_t _Count,char *_Format,va_list _Args)

{
  int iVar1;
  
  iVar1 = __vsnprintf_l(_Dest,_Count,_Format,(_locale_t)0x0,_Args);
  return iVar1;
}


// 1000f180 _memcpy

/* Library Function - Single Match
    _memcpy
   
   Libraries: Visual Studio 2005 Debug, Visual Studio 2005 Release, Visual Studio 2008 Debug, Visual
   Studio 2008 Release */

void * __cdecl _memcpy(void *_Dst,void *_Src,size_t _Size)

{
  undefined4 *puVar1;
  uint uVar2;
  uint uVar3;
  undefined4 *puVar4;
  
  if ((_Src < _Dst) && (_Dst < (void *)(_Size + (int)_Src))) {
    puVar1 = (undefined4 *)((_Size - 4) + (int)_Src);
    puVar4 = (undefined4 *)((_Size - 4) + (int)_Dst);
    if (((uint)puVar4 & 3) == 0) {
      uVar2 = _Size >> 2;
      uVar3 = _Size & 3;
      if (7 < uVar2) {
        for (; uVar2 != 0; uVar2 = uVar2 - 1) {
          *puVar4 = *puVar1;
          puVar1 = puVar1 + -1;
          puVar4 = puVar4 + -1;
        }
        switch(uVar3) {
        case 0:
          return _Dst;
        case 2:
          goto switchD_1000f363_caseD_2;
        case 3:
          goto switchD_1000f363_caseD_3;
        }
        goto switchD_1000f363_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_1000f363_caseD_0;
      case 1:
        goto switchD_1000f363_caseD_1;
      case 2:
        goto switchD_1000f363_caseD_2;
      case 3:
        goto switchD_1000f363_caseD_3;
      default:
        uVar2 = _Size - ((uint)puVar4 & 3);
        switch((uint)puVar4 & 3) {
        case 1:
          uVar3 = uVar2 & 3;
          *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
          puVar1 = (undefined4 *)((int)puVar1 + -1);
          uVar2 = uVar2 >> 2;
          puVar4 = (undefined4 *)((int)puVar4 - 1);
          if (7 < uVar2) {
            for (; uVar2 != 0; uVar2 = uVar2 - 1) {
              *puVar4 = *puVar1;
              puVar1 = puVar1 + -1;
              puVar4 = puVar4 + -1;
            }
            switch(uVar3) {
            case 0:
              return _Dst;
            case 2:
              goto switchD_1000f363_caseD_2;
            case 3:
              goto switchD_1000f363_caseD_3;
            }
            goto switchD_1000f363_caseD_1;
          }
          break;
        case 2:
          uVar3 = uVar2 & 3;
          *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
          uVar2 = uVar2 >> 2;
          *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
          puVar1 = (undefined4 *)((int)puVar1 + -2);
          puVar4 = (undefined4 *)((int)puVar4 - 2);
          if (7 < uVar2) {
            for (; uVar2 != 0; uVar2 = uVar2 - 1) {
              *puVar4 = *puVar1;
              puVar1 = puVar1 + -1;
              puVar4 = puVar4 + -1;
            }
            switch(uVar3) {
            case 0:
              return _Dst;
            case 2:
              goto switchD_1000f363_caseD_2;
            case 3:
              goto switchD_1000f363_caseD_3;
            }
            goto switchD_1000f363_caseD_1;
          }
          break;
        case 3:
          uVar3 = uVar2 & 3;
          *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
          *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
          uVar2 = uVar2 >> 2;
          *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
          puVar1 = (undefined4 *)((int)puVar1 + -3);
          puVar4 = (undefined4 *)((int)puVar4 - 3);
          if (7 < uVar2) {
            for (; uVar2 != 0; uVar2 = uVar2 - 1) {
              *puVar4 = *puVar1;
              puVar1 = puVar1 + -1;
              puVar4 = puVar4 + -1;
            }
            switch(uVar3) {
            case 0:
              return _Dst;
            case 2:
              goto switchD_1000f363_caseD_2;
            case 3:
              goto switchD_1000f363_caseD_3;
            }
            goto switchD_1000f363_caseD_1;
          }
        }
      }
    }
    switch(uVar2) {
    case 7:
      puVar4[7 - uVar2] = puVar1[7 - uVar2];
    case 6:
      puVar4[6 - uVar2] = puVar1[6 - uVar2];
    case 5:
      puVar4[5 - uVar2] = puVar1[5 - uVar2];
    case 4:
      puVar4[4 - uVar2] = puVar1[4 - uVar2];
    case 3:
      puVar4[3 - uVar2] = puVar1[3 - uVar2];
    case 2:
      puVar4[2 - uVar2] = puVar1[2 - uVar2];
    case 1:
      puVar4[1 - uVar2] = puVar1[1 - uVar2];
      puVar1 = puVar1 + -uVar2;
      puVar4 = puVar4 + -uVar2;
    }
    switch(uVar3) {
    case 1:
switchD_1000f363_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_1000f363_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_1000f363_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_1000f363_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_1004bcfc != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
    puVar1 = __VEC_memcpy((undefined4 *)_Dst,(undefined4 *)_Src,_Size);
    return puVar1;
  }
  puVar1 = (undefined4 *)_Dst;
  if (((uint)_Dst & 3) == 0) {
    uVar2 = _Size >> 2;
    uVar3 = _Size & 3;
    if (7 < uVar2) {
                    /* WARNING: Load size is inaccurate */
      for (; uVar2 != 0; uVar2 = uVar2 - 1) {
        *puVar1 = *_Src;
        _Src = (undefined4 *)((int)_Src + 4);
        puVar1 = puVar1 + 1;
      }
      switch(uVar3) {
      case 0:
        return _Dst;
      case 2:
        goto switchD_1000f1dc_caseD_2;
      case 3:
        goto switchD_1000f1dc_caseD_3;
      }
      goto switchD_1000f1dc_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_1000f1dc_caseD_0;
    case 1:
      goto switchD_1000f1dc_caseD_1;
    case 2:
      goto switchD_1000f1dc_caseD_2;
    case 3:
      goto switchD_1000f1dc_caseD_3;
    default:
      uVar2 = (_Size - 4) + ((uint)_Dst & 3);
      switch((uint)_Dst & 3) {
      case 1:
        uVar3 = uVar2 & 3;
                    /* WARNING: Load size is inaccurate */
        *(undefined1 *)_Dst = *_Src;
        *(undefined1 *)((int)_Dst + 1) = *(undefined1 *)((int)_Src + 1);
        uVar2 = uVar2 >> 2;
        *(undefined1 *)((int)_Dst + 2) = *(undefined1 *)((int)_Src + 2);
        _Src = (void *)((int)_Src + 3);
        puVar1 = (undefined4 *)((int)_Dst + 3);
        if (7 < uVar2) {
                    /* WARNING: Load size is inaccurate */
          for (; uVar2 != 0; uVar2 = uVar2 - 1) {
            *puVar1 = *_Src;
            _Src = (undefined4 *)((int)_Src + 4);
            puVar1 = puVar1 + 1;
          }
          switch(uVar3) {
          case 0:
            return _Dst;
          case 2:
            goto switchD_1000f1dc_caseD_2;
          case 3:
            goto switchD_1000f1dc_caseD_3;
          }
          goto switchD_1000f1dc_caseD_1;
        }
        break;
      case 2:
        uVar3 = uVar2 & 3;
                    /* WARNING: Load size is inaccurate */
        *(undefined1 *)_Dst = *_Src;
        uVar2 = uVar2 >> 2;
        *(undefined1 *)((int)_Dst + 1) = *(undefined1 *)((int)_Src + 1);
        _Src = (void *)((int)_Src + 2);
        puVar1 = (undefined4 *)((int)_Dst + 2);
        if (7 < uVar2) {
                    /* WARNING: Load size is inaccurate */
          for (; uVar2 != 0; uVar2 = uVar2 - 1) {
            *puVar1 = *_Src;
            _Src = (undefined4 *)((int)_Src + 4);
            puVar1 = puVar1 + 1;
          }
          switch(uVar3) {
          case 0:
            return _Dst;
          case 2:
            goto switchD_1000f1dc_caseD_2;
          case 3:
            goto switchD_1000f1dc_caseD_3;
          }
          goto switchD_1000f1dc_caseD_1;
        }
        break;
      case 3:
        uVar3 = uVar2 & 3;
                    /* WARNING: Load size is inaccurate */
        *(undefined1 *)_Dst = *_Src;
        _Src = (void *)((int)_Src + 1);
        uVar2 = uVar2 >> 2;
        puVar1 = (undefined4 *)((int)_Dst + 1);
        if (7 < uVar2) {
                    /* WARNING: Load size is inaccurate */
          for (; uVar2 != 0; uVar2 = uVar2 - 1) {
            *puVar1 = *_Src;
            _Src = (undefined4 *)((int)_Src + 4);
            puVar1 = puVar1 + 1;
          }
          switch(uVar3) {
          case 0:
            return _Dst;
          case 2:
            goto switchD_1000f1dc_caseD_2;
          case 3:
            goto switchD_1000f1dc_caseD_3;
          }
          goto switchD_1000f1dc_caseD_1;
        }
      }
    }
  }
  switch(uVar2) {
  case 7:
    puVar1[uVar2 - 7] = *(undefined4 *)((int)_Src + (uVar2 - 7) * 4);
  case 6:
    puVar1[uVar2 - 6] = *(undefined4 *)((int)_Src + (uVar2 - 6) * 4);
  case 5:
    puVar1[uVar2 - 5] = *(undefined4 *)((int)_Src + (uVar2 - 5) * 4);
  case 4:
    puVar1[uVar2 - 4] = *(undefined4 *)((int)_Src + (uVar2 - 4) * 4);
  case 3:
    puVar1[uVar2 - 3] = *(undefined4 *)((int)_Src + (uVar2 - 3) * 4);
  case 2:
    puVar1[uVar2 - 2] = *(undefined4 *)((int)_Src + (uVar2 - 2) * 4);
  case 1:
    puVar1[uVar2 - 1] = *(undefined4 *)((int)_Src + (uVar2 - 1) * 4);
    _Src = (void *)((int)_Src + uVar2 * 4);
    puVar1 = puVar1 + uVar2;
  }
  switch(uVar3) {
  case 1:
switchD_1000f1dc_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_1000f1dc_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_1000f1dc_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_1000f1dc_caseD_0:
  return _Dst;
}


// 1000f4e5 FUN_1000f4e5

void FUN_1000f4e5(void)

{
  return;
}


// 1000f4e6 __cfltcvt_init

/* Library Function - Single Match
    __cfltcvt_init
   
   Library: Visual Studio 2005 Release */

void __cfltcvt_init(void)

{
  PTR_LAB_10035ec8 = __cfltcvt;
  PTR_LAB_10035ecc = &LAB_1001219a;
  PTR_LAB_10035ed0 = &LAB_10012158;
  PTR_LAB_10035ed4 = &LAB_1001218c;
  PTR_LAB_10035ed8 = &LAB_10012102;
  PTR_LAB_10035edc = __cfltcvt;
  PTR_LAB_10035ee0 = __cfltcvt_l;
  PTR_LAB_10035ee4 = __fassign_l;
  PTR_LAB_10035ee8 = __cropzeros_l;
  PTR_LAB_10035eec = __forcdecpt_l;
  return;
}


// 1000f546 __fpmath

/* Library Function - Single Match
    __fpmath
   
   Library: Visual Studio 2005 Release */

void __cdecl __fpmath(int param_1)

{
  __cfltcvt_init();
  DAT_10039e14 = __ms_p5_mp_test_fdiv();
  if (param_1 != 0) {
    __setdefaultprecision();
  }
  return;
}


// 1000f564 type_info::~type_info

/* Library Function - Single Match
    public: virtual __thiscall type_info::~type_info(void)
   
   Library: Visual Studio 2005 Release */

void __thiscall type_info::~type_info(type_info *this)

{
  *(undefined ***)this = &PTR__scalar_deleting_destructor__1002911c;
  _Type_info_dtor(this);
  return;
}


// 1000f572 type_info::`scalar_deleting_destructor'

/* Library Function - Single Match
    public: virtual void * __thiscall type_info::`scalar deleting destructor'(unsigned int)
   
   Library: Visual Studio 2005 Release */

void * __thiscall type_info::_scalar_deleting_destructor_(type_info *this,uint param_1)

{
  ~type_info(this);
  if ((param_1 & 1) != 0) {
    FUN_100012d0(this);
  }
  return this;
}


// 1000f5c0 __alloca_probe_16

/* WARNING: This is an inlined function */
/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */
/* Library Function - Single Match
    __alloca_probe_16
   
   Library: Visual Studio 2005 Release */

uint __alloca_probe_16(void)

{
  uint in_EAX;
  uint uVar1;
  
  uVar1 = 4 - in_EAX & 0xf;
  return in_EAX + uVar1 | -(uint)CARRY4(in_EAX,uVar1);
}


// 1000f5d6 __alloca_probe_8

/* WARNING: This is an inlined function */
/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */
/* Library Function - Single Match
    __alloca_probe_8
   
   Library: Visual Studio */

uint __alloca_probe_8(void)

{
  uint in_EAX;
  uint uVar1;
  
  uVar1 = 4 - in_EAX & 7;
  return in_EAX + uVar1 | -(uint)CARRY4(in_EAX,uVar1);
}


// 1000f5ec FUN_1000f5ec

int __cdecl FUN_1000f5ec(int param_1)

{
  int *piVar1;
  int *piVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  
  piVar1 = (int *)FUN_10013035(DAT_1004bcec);
  piVar2 = (int *)FUN_10013035(DAT_1004bce8);
  if (piVar1 <= piVar2) {
    uVar4 = ((int)piVar2 - (int)piVar1) + 4;
    if (3 < uVar4) {
      uVar3 = FUN_10001240(piVar1);
      if (uVar3 < uVar4) {
        uVar4 = 0x800;
        if (uVar3 < 0x800) {
          uVar4 = uVar3;
        }
        if ((uVar4 + uVar3 < uVar3) ||
           (iVar5 = thunk_FUN_10001110(piVar1,uVar4 + uVar3), iVar5 == 0)) {
          if (uVar3 + 0x10 < uVar3) {
            return 0;
          }
          iVar5 = thunk_FUN_10001110(piVar1,uVar3 + 0x10);
          if (iVar5 == 0) {
            return 0;
          }
        }
        piVar2 = (int *)(iVar5 + ((int)piVar2 - (int)piVar1 >> 2) * 4);
        DAT_1004bcec = FUN_10012fbe(iVar5);
      }
      iVar5 = FUN_10012fbe(param_1);
      *piVar2 = iVar5;
      DAT_1004bce8 = FUN_10012fbe((int)(piVar2 + 1));
      return param_1;
    }
  }
  return 0;
}


// 1000f6d4 __onexit

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __onexit
   
   Library: Visual Studio 2005 Release */

_onexit_t __cdecl __onexit(_onexit_t _Func)

{
  _onexit_t p_Var1;
  
  FUN_100135e0();
  p_Var1 = (_onexit_t)FUN_1000f5ec((int)_Func);
  FUN_1000f70a();
  return p_Var1;
}


// 1000f70a FUN_1000f70a

void FUN_1000f70a(void)

{
  FUN_100135e9();
  return;
}


// 1000f710 _atexit

/* Library Function - Single Match
    _atexit
   
   Library: Visual Studio 2005 Release */

int __cdecl _atexit(_func_4879 *param_1)

{
  _onexit_t p_Var1;
  
  p_Var1 = __onexit((_onexit_t)param_1);
  return (p_Var1 != (_onexit_t)0x0) - 1;
}


// 1000f722 _LocaleUpdate::_LocaleUpdate

/* Library Function - Single Match
    public: __thiscall _LocaleUpdate::_LocaleUpdate(struct localeinfo_struct *)
   
   Library: Visual Studio 2005 Release */

_LocaleUpdate * __thiscall
_LocaleUpdate::_LocaleUpdate(_LocaleUpdate *this,localeinfo_struct *param_1)

{
  uint *puVar1;
  _ptiddata p_Var2;
  pthreadlocinfo ptVar3;
  pthreadmbcinfo ptVar4;
  
  this[0xc] = (_LocaleUpdate)0x0;
  if (param_1 == (localeinfo_struct *)0x0) {
    p_Var2 = __getptd();
    *(_ptiddata *)(this + 8) = p_Var2;
    *(pthreadlocinfo *)this = p_Var2->ptlocinfo;
    *(pthreadmbcinfo *)(this + 4) = p_Var2->ptmbcinfo;
    if ((*(undefined **)this != PTR_DAT_100365d8) && ((p_Var2->_ownlocale & DAT_100364f4) == 0)) {
      ptVar3 = ___updatetlocinfo();
      *(pthreadlocinfo *)this = ptVar3;
    }
    if ((*(undefined **)(this + 4) != PTR_DAT_100363f8) &&
       ((*(uint *)(*(int *)(this + 8) + 0x70) & DAT_100364f4) == 0)) {
      ptVar4 = ___updatetmbcinfo();
      *(pthreadmbcinfo *)(this + 4) = ptVar4;
    }
    if ((*(byte *)(*(int *)(this + 8) + 0x70) & 2) == 0) {
      puVar1 = (uint *)(*(int *)(this + 8) + 0x70);
      *puVar1 = *puVar1 | 2;
      this[0xc] = (_LocaleUpdate)0x1;
    }
  }
  else {
    *(pthreadlocinfo *)this = param_1->locinfo;
    *(pthreadmbcinfo *)(this + 4) = param_1->mbcinfo;
  }
  return this;
}


// 1000f7a4 __tolower_l

/* Library Function - Single Match
    __tolower_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __tolower_l(int _C,_locale_t _Locale)

{
  int iVar1;
  uint uVar2;
  int iVar3;
  DWORD *pDVar4;
  CHAR CVar5;
  localeinfo_struct local_1c;
  int local_14;
  char local_10;
  byte local_c;
  undefined1 local_b;
  CHAR local_8;
  CHAR local_7;
  undefined1 local_6;
  
  iVar1 = _C;
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_1c,_Locale);
  if ((uint)_C < 0x100) {
    if ((int)(local_1c.locinfo)->locale_name[3] < 2) {
      uVar2 = (byte)local_1c.locinfo[1].lc_category[0].locale[_C * 2] & 1;
    }
    else {
      uVar2 = __isctype_l(_C,1,&local_1c);
    }
    if (uVar2 == 0) {
LAB_1000f803:
      if (local_10 == '\0') {
        return iVar1;
      }
      *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
      return iVar1;
    }
    uVar2 = (uint)*(byte *)((int)local_1c.locinfo[1].lc_category[0].wlocale + _C);
  }
  else {
    CVar5 = (CHAR)_C;
    if (((int)(local_1c.locinfo)->locale_name[3] < 2) ||
       (iVar3 = __isleadbyte_l(_C >> 8 & 0xff,&local_1c), iVar3 == 0)) {
      pDVar4 = FUN_10011e63();
      *pDVar4 = 0x2a;
      local_7 = '\0';
      iVar3 = 1;
      local_8 = CVar5;
    }
    else {
      _C._0_1_ = (CHAR)((uint)_C >> 8);
      local_8 = (CHAR)_C;
      local_6 = 0;
      iVar3 = 2;
      local_7 = CVar5;
    }
    iVar3 = ___crtLCMapStringA(&local_1c,(local_1c.locinfo)->lc_category[0].wlocale,0x100,&local_8,
                               iVar3,(LPSTR)&local_c,3,(local_1c.locinfo)->lc_codepage,1);
    if (iVar3 == 0) goto LAB_1000f803;
    if (iVar3 == 1) {
      uVar2 = (uint)local_c;
    }
    else {
      uVar2 = (uint)CONCAT11(local_c,local_b);
    }
  }
  if (local_10 != '\0') {
    *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
  }
  return uVar2;
}


// 1000f8bb _tolower

/* Library Function - Single Match
    _tolower
   
   Library: Visual Studio 2005 Release */

int __cdecl _tolower(int _C)

{
  if (DAT_1003a1cc == 0) {
    if (_C - 0x41U < 0x1a) {
      return _C + 0x20;
    }
  }
  else {
    _C = __tolower_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 1000f8e2 __isupper_l

/* Library Function - Single Match
    __isupper_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __isupper_l(int _C,_locale_t _Locale)

{
  uint uVar1;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  if ((int)(local_14.locinfo)->locale_name[3] < 2) {
    uVar1 = (byte)local_14.locinfo[1].lc_category[0].locale[_C * 2] & 1;
  }
  else {
    uVar1 = __isctype_l(_C,1,&local_14);
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 1000f931 FUN_1000f931

uint __cdecl FUN_1000f931(int param_1)

{
  uint uVar1;
  
  if (DAT_1003a1cc == 0) {
    return (byte)PTR_DAT_100365c8[param_1 * 2] & 1;
  }
  uVar1 = __isupper_l(param_1,(_locale_t)0x0);
  return uVar1;
}


// 1000f95a __isdigit_l

/* Library Function - Single Match
    __isdigit_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __isdigit_l(int _C,_locale_t _Locale)

{
  uint uVar1;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  if ((int)(local_14.locinfo)->locale_name[3] < 2) {
    uVar1 = (byte)local_14.locinfo[1].lc_category[0].locale[_C * 2] & 4;
  }
  else {
    uVar1 = __isctype_l(_C,4,&local_14);
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 1000f9a9 _isdigit

/* Library Function - Single Match
    _isdigit
   
   Library: Visual Studio 2005 Release */

int __cdecl _isdigit(int _C)

{
  int iVar1;
  
  if (DAT_1003a1cc == 0) {
    return (byte)PTR_DAT_100365c8[_C * 2] & 4;
  }
  iVar1 = __isdigit_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 1000f9d2 __isxdigit_l

/* Library Function - Single Match
    __isxdigit_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __isxdigit_l(int _C,_locale_t _Locale)

{
  uint uVar1;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  if ((int)(local_14.locinfo)->locale_name[3] < 2) {
    uVar1 = (byte)local_14.locinfo[1].lc_category[0].locale[_C * 2] & 0x80;
  }
  else {
    uVar1 = __isctype_l(_C,0x80,&local_14);
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 1000fa51 __isspace_l

/* Library Function - Single Match
    __isspace_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __isspace_l(int _C,_locale_t _Locale)

{
  uint uVar1;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  if ((int)(local_14.locinfo)->locale_name[3] < 2) {
    uVar1 = (byte)local_14.locinfo[1].lc_category[0].locale[_C * 2] & 8;
  }
  else {
    uVar1 = __isctype_l(_C,8,&local_14);
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 1000faa0 _isspace

/* Library Function - Single Match
    _isspace
   
   Library: Visual Studio 2005 Release */

int __cdecl _isspace(int _C)

{
  int iVar1;
  
  if (DAT_1003a1cc == 0) {
    return (byte)PTR_DAT_100365c8[_C * 2] & 8;
  }
  iVar1 = __isspace_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 1000fac9 __CRT_INIT@12

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __CRT_INIT@12
   
   Library: Visual Studio 2005 Release */

undefined4 __CRT_INIT_12(undefined4 param_1,int param_2,int param_3)

{
  DWORD DVar1;
  DWORD DVar2;
  DWORD DVar3;
  bool bVar4;
  HANDLE pvVar5;
  LPOSVERSIONINFOA lpVersionInformation;
  BOOL BVar6;
  undefined3 extraout_var;
  int iVar7;
  DWORD *pDVar8;
  code *pcVar9;
  uint uVar10;
  DWORD DVar11;
  undefined4 uVar12;
  SIZE_T dwBytes;
  DWORD *pDVar13;
  
  if (param_2 == 1) {
    dwBytes = 0x94;
    DVar11 = 0;
    pvVar5 = GetProcessHeap();
    lpVersionInformation = (LPOSVERSIONINFOA)HeapAlloc(pvVar5,DVar11,dwBytes);
    if (lpVersionInformation != (LPOSVERSIONINFOA)0x0) {
      lpVersionInformation->dwOSVersionInfoSize = 0x94;
      BVar6 = GetVersionExA(lpVersionInformation);
      DVar11 = 0;
      if (BVar6 == 0) {
        pvVar5 = GetProcessHeap();
        HeapFree(pvVar5,DVar11,lpVersionInformation);
      }
      else {
        DVar1 = lpVersionInformation->dwPlatformId;
        DVar2 = lpVersionInformation->dwMajorVersion;
        DVar3 = lpVersionInformation->dwMinorVersion;
        uVar10 = lpVersionInformation->dwBuildNumber & 0x7fff;
        pvVar5 = GetProcessHeap();
        HeapFree(pvVar5,DVar11,lpVersionInformation);
        if (DVar1 != 2) {
          uVar10 = uVar10 | 0x8000;
        }
        _DAT_1003a174 = DVar2 * 0x100 + DVar3;
        DAT_1003a16c = DVar1;
        _DAT_1003a170 = uVar10;
        DAT_1003a178 = DVar2;
        _DAT_1003a17c = DVar3;
        bVar4 = FUN_10001260();
        if (CONCAT31(extraout_var,bVar4) != 0) {
          iVar7 = __mtinit();
          if (iVar7 != 0) {
            __RTC_Initialize();
            DAT_1004bd00 = GetCommandLineA();
            DAT_10039e1c = ___crtGetEnvironmentStringsA();
            iVar7 = __ioinit();
            if (-1 < iVar7) {
              iVar7 = __setargv();
              if (((-1 < iVar7) && (iVar7 = __setenvp(), -1 < iVar7)) &&
                 (iVar7 = __cinit(0), iVar7 == 0)) {
                DAT_10039e18 = DAT_10039e18 + 1;
                return 1;
              }
              __ioterm();
            }
            __mtterm();
          }
          FUN_10004c90();
        }
      }
    }
  }
  else if (param_2 == 0) {
    if (0 < DAT_10039e18) {
      DAT_10039e18 = DAT_10039e18 + -1;
      if (DAT_1003a1a8 == 0) {
        __cexit();
      }
      if (param_3 == 0) {
        __ioterm();
        __mtterm();
        FUN_10004c90();
      }
      return 1;
    }
  }
  else {
    if (param_2 != 2) {
      if (param_2 != 3) {
        return 1;
      }
      __freeptd((_ptiddata)0x0);
      return 1;
    }
    FUN_100130ac();
    pDVar8 = (DWORD *)FUN_100011f0(1,0x214);
    if (pDVar8 != (DWORD *)0x0) {
      uVar12 = DAT_10035fc0;
      pDVar13 = pDVar8;
      pcVar9 = (code *)FUN_10013035(DAT_1003a154);
      iVar7 = (*pcVar9)(uVar12,pDVar13);
      if (iVar7 != 0) {
        FUN_1001311b((int)pDVar8,0);
        DVar11 = GetCurrentThreadId();
        pDVar8[1] = 0xffffffff;
        *pDVar8 = DVar11;
        return 1;
      }
      FUN_100012d0(pDVar8);
    }
  }
  return 0;
}


// 1000fca2 ___DllMainCRTStartup

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x1000fd34) */
/* WARNING: Removing unreachable block (ram,0x1000fce1) */
/* WARNING: Removing unreachable block (ram,0x1000fd61) */
/* Library Function - Single Match
    ___DllMainCRTStartup
   
   Library: Visual Studio 2005 Release */

int __fastcall ___DllMainCRTStartup(int param_1,int param_2,undefined4 param_3)

{
  int iVar1;
  int local_20;
  
  if (((param_2 == 0) && (DAT_10039e18 == 0)) ||
     (((param_2 == 1 || (param_2 == 2)) &&
      (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)))) {
    local_20 = 0;
  }
  else {
    local_20 = FUN_100151c6();
    if ((param_2 == 1) && (local_20 == 0)) {
      FUN_100151c6();
      __CRT_INIT_12(param_3,0,param_1);
    }
    if (((param_2 == 0) || (param_2 == 3)) &&
       (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)) {
      local_20 = 0;
    }
  }
  return local_20;
}


// 1000fd98 entry

void entry(undefined4 param_1,int param_2,int param_3)

{
  if (param_2 == 1) {
    ___security_init_cookie();
  }
  ___DllMainCRTStartup(param_3,param_2,param_1);
  return;
}


// 1000fdb9 __vswprintf_helper

/* Library Function - Single Match
    __vswprintf_helper
   
   Library: Visual Studio 2005 Release */

int __cdecl
__vswprintf_helper(undefined *param_1,char *param_2,uint param_3,int param_4,undefined4 param_5,
                  undefined4 param_6)

{
  DWORD *pDVar1;
  int iVar2;
  int iVar3;
  FILE local_24;
  
  if (param_4 == 0) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  if ((param_3 != 0) && (param_2 == (char *)0x0)) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  local_24._flag = 0x42;
  local_24._base = param_2;
  local_24._ptr = param_2;
  if (param_3 < 0x40000000) {
    local_24._cnt = param_3 * 2;
  }
  else {
    local_24._cnt = 0x7fffffff;
  }
  iVar2 = (*(code *)param_1)(&local_24,param_4,param_5,param_6);
  if (param_2 == (char *)0x0) {
    return iVar2;
  }
  if (-1 < iVar2) {
    local_24._cnt = local_24._cnt + -1;
    if (local_24._cnt < 0) {
      iVar3 = __flsbuf(0,&local_24);
      if (iVar3 == -1) goto LAB_1000fe9b;
    }
    else {
      *local_24._ptr = '\0';
      local_24._ptr = local_24._ptr + 1;
    }
    local_24._cnt = local_24._cnt + -1;
    if (-1 < local_24._cnt) {
      *local_24._ptr = '\0';
      return iVar2;
    }
    iVar3 = __flsbuf(0,&local_24);
    if (iVar3 != -1) {
      return iVar2;
    }
  }
LAB_1000fe9b:
  (param_2 + param_3 * 2 + -2)[0] = '\0';
  (param_2 + param_3 * 2 + -2)[1] = '\0';
  return (-1 < local_24._cnt) - 2;
}


// 1000feaf FID_conflict:__vswprintf_p_l

/* Library Function - Multiple Matches With Different Base Names
    __vswprintf_c_l
    __vswprintf_p_l
   
   Library: Visual Studio 2005 Release */

int __cdecl
FID_conflict___vswprintf_p_l
          (wchar_t *_DstBuf,size_t _MaxCount,wchar_t *_Format,_locale_t _Locale,va_list _ArgList)

{
  int iVar1;
  
  iVar1 = __vswprintf_helper(&LAB_100152f9,(char *)_DstBuf,_MaxCount,(int)_Format,_Locale,_ArgList);
  if (iVar1 < 0) {
    iVar1 = -1;
  }
  return iVar1;
}


// 1000fef0 _strchr

/* Library Function - Single Match
    _strchr
   
   Libraries: Visual Studio 2005, Visual Studio 2008, Visual Studio 2010, Visual Studio 2019 */

char * __cdecl _strchr(char *_Str,int _Val)

{
  uint uVar1;
  char cVar2;
  uint uVar3;
  uint uVar4;
  uint *puVar5;
  
  uVar1 = (uint)_Str & 3;
  while (uVar1 != 0) {
    if ((char)*(uint *)_Str == (char)_Val) {
      return (char *)(uint *)_Str;
    }
    if ((char)*(uint *)_Str == '\0') {
      return (char *)0x0;
    }
    uVar1 = (uint)((int)_Str + 1) & 3;
    _Str = (char *)((int)_Str + 1);
  }
  while( true ) {
    while( true ) {
      uVar1 = *(uint *)_Str;
      uVar4 = uVar1 ^ CONCAT22(CONCAT11((char)_Val,(char)_Val),CONCAT11((char)_Val,(char)_Val));
      uVar3 = uVar1 ^ 0xffffffff ^ uVar1 + 0x7efefeff;
      puVar5 = (uint *)((int)_Str + 4);
      if (((uVar4 ^ 0xffffffff ^ uVar4 + 0x7efefeff) & 0x81010100) != 0) break;
      _Str = (char *)puVar5;
      if ((uVar3 & 0x81010100) != 0) {
        if ((uVar3 & 0x1010100) != 0) {
          return (char *)0x0;
        }
        if ((uVar1 + 0x7efefeff & 0x80000000) == 0) {
          return (char *)0x0;
        }
      }
    }
    uVar1 = *(uint *)_Str;
    if ((char)uVar1 == (char)_Val) {
      return (char *)(uint *)_Str;
    }
    if ((char)uVar1 == '\0') {
      return (char *)0x0;
    }
    cVar2 = (char)(uVar1 >> 8);
    if (cVar2 == (char)_Val) {
      return (char *)((int)_Str + 1);
    }
    if (cVar2 == '\0') break;
    cVar2 = (char)(uVar1 >> 0x10);
    if (cVar2 == (char)_Val) {
      return (char *)((int)_Str + 2);
    }
    if (cVar2 == '\0') {
      return (char *)0x0;
    }
    cVar2 = (char)(uVar1 >> 0x18);
    if (cVar2 == (char)_Val) {
      return (char *)((int)_Str + 3);
    }
    _Str = (char *)puVar5;
    if (cVar2 == '\0') {
      return (char *)0x0;
    }
  }
  return (char *)0x0;
}


// 1000ffae FUN_1000ffae

void __cdecl FUN_1000ffae(wchar_t *param_1)

{
  _wcstol(param_1,(wchar_t **)0x0,10);
  return;
}


// 1000ffc4 _atol

/* Library Function - Single Match
    _atol
   
   Library: Visual Studio 2005 Release */

long __cdecl _atol(char *_Str)

{
  long lVar1;
  
  lVar1 = _strtol(_Str,(char **)0x0,10);
  return lVar1;
}


// 1000ffe0 FUN_1000ffe0

ulonglong __fastcall FUN_1000ffe0(undefined4 param_1,undefined4 param_2)

{
  ulonglong uVar1;
  uint uVar2;
  float fVar3;
  float10 in_ST0;
  uint local_20;
  float fStack_1c;
  
  if (DAT_1004bcfc == 0) {
    uVar1 = (ulonglong)ROUND(in_ST0);
    local_20 = (uint)uVar1;
    fStack_1c = (float)(uVar1 >> 0x20);
    fVar3 = (float)in_ST0;
    if ((local_20 != 0) || (fVar3 = fStack_1c, (uVar1 & 0x7fffffff00000000) != 0)) {
      if ((int)fVar3 < 0) {
        uVar1 = uVar1 + (0x80000000 < (uint)-(float)(in_ST0 - (float10)(longlong)uVar1));
      }
      else {
        uVar2 = (uint)(0x80000000 < (uint)(float)(in_ST0 - (float10)(longlong)uVar1));
        uVar1 = CONCAT44((int)fStack_1c - (uint)(local_20 < uVar2),local_20 - uVar2);
      }
    }
    return uVar1;
  }
  return CONCAT44(param_2,(int)in_ST0);
}


// 1001008b vscan_fn

/* Library Function - Single Match
    _vscan_fn
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl vscan_fn(undefined *param_1,int param_2,undefined4 param_3,undefined4 param_4)

{
  DWORD *pDVar1;
  undefined4 uVar2;
  short *unaff_ESI;
  
  FUN_10015e29(unaff_ESI);
  if ((unaff_ESI == (short *)0x0) || (param_2 == 0)) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    uVar2 = 0xffffffff;
  }
  else {
    uVar2 = (*(code *)param_1)(&stack0xffffffdc,param_2,param_3,param_4);
  }
  return uVar2;
}


// 100100f9 FID_conflict:_swscanf

/* Library Function - Multiple Matches With Different Base Names
    _swscanf
    _swscanf_s
   
   Library: Visual Studio 2005 Release */

int __cdecl FID_conflict__swscanf(wchar_t *_Src,wchar_t *_Format,...)

{
  int iVar1;
  
  iVar1 = vscan_fn(&DAT_10015efb,(int)_Format,0,&stack0x0000000c);
  return iVar1;
}


// 10010118 vscan_fn

/* Library Function - Single Match
    _vscan_fn
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl vscan_fn(undefined *param_1,int param_2,undefined4 param_3,undefined4 param_4)

{
  DWORD *pDVar1;
  undefined4 uVar2;
  char *unaff_ESI;
  
  _strlen(unaff_ESI);
  if ((unaff_ESI == (char *)0x0) || (param_2 == 0)) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    uVar2 = 0xffffffff;
  }
  else {
    uVar2 = (*(code *)param_1)(&stack0xffffffdc,param_2,param_3,param_4);
  }
  return uVar2;
}


// 10010180 FID_conflict:_sscanf

/* Library Function - Multiple Matches With Different Base Names
    _sscanf
    _sscanf_s
   
   Library: Visual Studio 2005 Release */

int __cdecl FID_conflict__sscanf(char *_Src,char *_Format,...)

{
  int iVar1;
  
  iVar1 = vscan_fn(&DAT_10016cca,(int)_Format,0,&stack0x0000000c);
  return iVar1;
}


// 1001019f __wtof_l

/* Library Function - Single Match
    __wtof_l
   
   Library: Visual Studio 2005 Release */

double __cdecl __wtof_l(wchar_t *_Str,_locale_t _Locale)

{
  double dVar1;
  DWORD *pDVar2;
  int iVar3;
  _locale_t _Locale_00;
  FLT p_Var4;
  _flt local_2c;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  if (_Str == (wchar_t *)0x0) {
    pDVar2 = FUN_10011e63();
    *pDVar2 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    dVar1 = 0.0;
  }
  else {
    while( true ) {
      iVar3 = __iswctype_l(*_Str,8,&local_14);
      if (iVar3 == 0) break;
      _Str = _Str + 1;
    }
    _Locale_00 = (_locale_t)FUN_10015e29(_Str);
    p_Var4 = __wfltin2(&local_2c,_Str,_Locale_00);
    dVar1 = p_Var4->dval;
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return dVar1;
}


// 10010229 __wtof

/* Library Function - Single Match
    __wtof
   
   Library: Visual Studio 2005 Release */

double __cdecl __wtof(wchar_t *_Str)

{
  double dVar1;
  
  dVar1 = __wtof_l(_Str,(_locale_t)0x0);
  return dVar1;
}


// 10010237 __atof_l

/* Library Function - Single Match
    __atof_l
   
   Library: Visual Studio 2005 Release */

double __cdecl __atof_l(char *_String,_locale_t _Locale)

{
  double dVar1;
  DWORD *pDVar2;
  uint uVar3;
  _locale_t _Locale_00;
  FLT p_Var4;
  _flt local_2c;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  if (_String == (char *)0x0) {
    pDVar2 = FUN_10011e63();
    *pDVar2 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    dVar1 = 0.0;
  }
  else {
    while( true ) {
      if ((int)(local_14.locinfo)->locale_name[3] < 2) {
        uVar3 = (byte)local_14.locinfo[1].lc_category[0].locale[(uint)(byte)*_String * 2] & 8;
      }
      else {
        uVar3 = __isctype_l((uint)(byte)*_String,8,&local_14);
      }
      if (uVar3 == 0) break;
      _String = (char *)((byte *)_String + 1);
    }
    _Locale_00 = (_locale_t)_strlen(_String);
    p_Var4 = __fltin2(&local_2c,_String,_Locale_00);
    dVar1 = p_Var4->dval;
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return dVar1;
}


// 100102e0 _atof

/* Library Function - Single Match
    _atof
   
   Library: Visual Studio 2005 Release */

double __cdecl _atof(char *_String)

{
  double dVar1;
  
  dVar1 = __atof_l(_String,(_locale_t)0x0);
  return dVar1;
}


// 100102ee FUN_100102ee

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 __cdecl FUN_100102ee(byte *param_1,undefined4 *param_2,localeinfo_struct *param_3)

{
  DWORD *pDVar1;
  uint uVar2;
  _locale_t _Locale;
  FLT p_Var3;
  byte *_Str;
  _flt local_34;
  localeinfo_struct local_1c;
  int local_14;
  char local_10;
  double local_c;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_1c,param_3);
  if (param_2 != (undefined4 *)0x0) {
    *param_2 = param_1;
  }
  _Str = param_1;
  if (param_1 == (byte *)0x0) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_10 != '\0') {
      *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
    }
    return (float10)0;
  }
  while( true ) {
    if ((int)(local_1c.locinfo)->locale_name[3] < 2) {
      uVar2 = (byte)local_1c.locinfo[1].lc_category[0].locale[(uint)*_Str * 2] & 8;
    }
    else {
      uVar2 = __isctype_l((uint)*_Str,8,&local_1c);
    }
    if (uVar2 == 0) break;
    _Str = _Str + 1;
  }
  _Locale = (_locale_t)_strlen((char *)_Str);
  p_Var3 = __fltin2(&local_34,(char *)_Str,_Locale);
  if (param_2 != (undefined4 *)0x0) {
    *param_2 = _Str + p_Var3->nbytes;
  }
  uVar2 = p_Var3->flags;
  if ((uVar2 & 0x240) == 0) {
    if ((uVar2 & 0x81) == 0) {
      if (((uVar2 & 0x100) == 0) || (local_c = 0.0, p_Var3->dval != 0.0)) {
        local_c = p_Var3->dval;
        goto LAB_100103f4;
      }
    }
    else {
      local_c = _DAT_100366b0;
      if (*_Str == 0x2d) {
        local_c = -_DAT_100366b0;
      }
    }
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x22;
  }
  else {
    local_c = 0.0;
    if (param_2 != (undefined4 *)0x0) {
      *param_2 = param_1;
    }
  }
LAB_100103f4:
  if (local_10 != '\0') {
    *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
  }
  return (float10)local_c;
}


// 10010409 FUN_10010409

void __cdecl FUN_10010409(byte *param_1,undefined4 *param_2)

{
  FUN_100102ee(param_1,param_2,(localeinfo_struct *)0x0);
  return;
}


// 1001041c strtoxl

/* Library Function - Single Match
    unsigned long __cdecl strtoxl(struct localeinfo_struct *,char const *,char const * *,int,int)
   
   Library: Visual Studio 2005 Release */

ulong __cdecl
strtoxl(localeinfo_struct *param_1,char *param_2,char **param_3,int param_4,int param_5)

{
  ushort uVar1;
  byte *pbVar2;
  DWORD *pDVar3;
  uint uVar4;
  pthreadlocinfo ptVar5;
  uint uVar6;
  int iVar7;
  byte bVar8;
  byte *pbVar9;
  localeinfo_struct local_18;
  int local_10;
  char local_c;
  uint local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_18,param_1);
  if (param_3 != (char **)0x0) {
    *param_3 = param_2;
  }
  if ((param_2 == (char *)0x0) || ((param_4 != 0 && ((param_4 < 2 || (0x24 < param_4)))))) {
    pDVar3 = FUN_10011e63();
    *pDVar3 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
    }
    return 0;
  }
  bVar8 = *param_2;
  local_8 = 0;
  ptVar5 = local_18.locinfo;
  pbVar2 = (byte *)param_2;
  while( true ) {
    pbVar9 = pbVar2 + 1;
    if ((int)ptVar5->locale_name[3] < 2) {
      uVar4 = (byte)ptVar5[1].lc_category[0].locale[(uint)bVar8 * 2] & 8;
    }
    else {
      uVar4 = __isctype_l((uint)bVar8,8,&local_18);
      ptVar5 = local_18.locinfo;
    }
    if (uVar4 == 0) break;
    bVar8 = *pbVar9;
    pbVar2 = pbVar9;
  }
  if (bVar8 == 0x2d) {
    param_5 = param_5 | 2;
LAB_100104d3:
    bVar8 = *pbVar9;
    pbVar9 = pbVar2 + 2;
  }
  else if (bVar8 == 0x2b) goto LAB_100104d3;
  if (((param_4 < 0) || (param_4 == 1)) || (0x24 < param_4)) {
    if (param_3 != (char **)0x0) {
      *param_3 = param_2;
    }
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
    }
    return 0;
  }
  if (param_4 == 0) {
    if (bVar8 != 0x30) {
      param_4 = 10;
      goto LAB_10010539;
    }
    if ((*pbVar9 != 0x78) && (*pbVar9 != 0x58)) {
      param_4 = 8;
      goto LAB_10010539;
    }
    param_4 = 0x10;
  }
  else if ((param_4 != 0x10) || (bVar8 != 0x30)) goto LAB_10010539;
  if ((*pbVar9 == 0x78) || (*pbVar9 == 0x58)) {
    bVar8 = pbVar9[1];
    pbVar9 = pbVar9 + 2;
  }
LAB_10010539:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  do {
    uVar1 = *(ushort *)(ptVar5[1].lc_category[0].locale + (uint)bVar8 * 2);
    if ((uVar1 & 4) == 0) {
      if ((uVar1 & 0x103) == 0) {
LAB_10010593:
        pbVar9 = pbVar9 + -1;
        if ((param_5 & 8U) == 0) {
          if (param_3 != (char **)0x0) {
            pbVar9 = (byte *)param_2;
          }
          local_8 = 0;
        }
        else if (((param_5 & 4U) != 0) ||
                (((param_5 & 1U) == 0 &&
                 ((((param_5 & 2U) != 0 && (0x80000000 < local_8)) ||
                  (((param_5 & 2U) == 0 && (0x7fffffff < local_8)))))))) {
          pDVar3 = FUN_10011e63();
          *pDVar3 = 0x22;
          if ((param_5 & 1U) == 0) {
            local_8 = ((param_5 & 2U) != 0) + 0x7fffffff;
          }
          else {
            local_8 = 0xffffffff;
          }
        }
        if (param_3 != (char **)0x0) {
          *param_3 = (char *)pbVar9;
        }
        if ((param_5 & 2U) != 0) {
          local_8 = -local_8;
        }
        if (local_c == '\0') {
          return local_8;
        }
        *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
        return local_8;
      }
      iVar7 = (int)(char)bVar8;
      if ((byte)(bVar8 + 0x9f) < 0x1a) {
        iVar7 = iVar7 + -0x20;
      }
      uVar6 = iVar7 - 0x37;
    }
    else {
      uVar6 = (int)(char)bVar8 - 0x30;
    }
    if ((uint)param_4 <= uVar6) goto LAB_10010593;
    if ((local_8 < uVar4) ||
       ((local_8 == uVar4 && (uVar6 <= (uint)(0xffffffff % (ulonglong)(uint)param_4))))) {
      local_8 = local_8 * param_4 + uVar6;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (char **)0x0) goto LAB_10010593;
    }
    bVar8 = *pbVar9;
    pbVar9 = pbVar9 + 1;
  } while( true );
}


// 10010647 _strtol

/* Library Function - Single Match
    _strtol
   
   Library: Visual Studio 2005 Release */

long __cdecl _strtol(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_1003a1cc == 0) {
    ppuVar2 = &PTR_DAT_100365e0;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 10010670 _strtoul

/* Library Function - Single Match
    _strtoul
   
   Library: Visual Studio 2005 Release */

ulong __cdecl _strtoul(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_1003a1cc == 0) {
    ppuVar2 = &PTR_DAT_100365e0;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,1);
  return uVar1;
}


// 100106a0 __allmul

/* Library Function - Single Match
    __allmul
   
   Library: Visual Studio */

longlong __allmul(uint param_1,int param_2,uint param_3,int param_4)

{
  if (param_4 == 0 && param_2 == 0) {
    return (ulonglong)param_1 * (ulonglong)param_3;
  }
  return CONCAT44((int)((ulonglong)param_1 * (ulonglong)param_3 >> 0x20) +
                  param_2 * param_3 + param_1 * param_4,
                  (int)((ulonglong)param_1 * (ulonglong)param_3));
}


// 100106e0 _strncpy

/* Library Function - Single Match
    _strncpy
   
   Library: Visual Studio */

char * __cdecl _strncpy(char *_Dest,char *_Source,size_t _Count)

{
  uint uVar1;
  uint uVar2;
  char cVar3;
  uint uVar4;
  uint *puVar5;
  
  if (_Count == 0) {
    return _Dest;
  }
  puVar5 = (uint *)_Dest;
  if (((uint)_Source & 3) != 0) {
    while( true ) {
      uVar4 = *(uint *)_Source;
      _Source = (char *)((int)_Source + 1);
      *(char *)puVar5 = (char)uVar4;
      puVar5 = (uint *)((int)puVar5 + 1);
      _Count = _Count - 1;
      if (_Count == 0) {
        return _Dest;
      }
      if ((char)uVar4 == '\0') break;
      if (((uint)_Source & 3) == 0) {
        uVar4 = _Count >> 2;
        goto joined_r0x1001072c;
      }
    }
    do {
      if (((uint)puVar5 & 3) == 0) {
        uVar4 = _Count >> 2;
        cVar3 = '\0';
        if (uVar4 == 0) goto LAB_10010773;
        goto LAB_100107e9;
      }
      *(char *)puVar5 = '\0';
      puVar5 = (uint *)((int)puVar5 + 1);
      _Count = _Count - 1;
    } while (_Count != 0);
    return _Dest;
  }
  uVar4 = _Count >> 2;
  if (uVar4 != 0) {
    do {
      uVar1 = *(uint *)_Source;
      uVar2 = *(uint *)_Source;
      _Source = (char *)((int)_Source + 4);
      if (((uVar1 ^ 0xffffffff ^ uVar1 + 0x7efefeff) & 0x81010100) != 0) {
        if ((char)uVar2 == '\0') {
          *puVar5 = 0;
joined_r0x100107e5:
          while( true ) {
            uVar4 = uVar4 - 1;
            puVar5 = puVar5 + 1;
            if (uVar4 == 0) break;
LAB_100107e9:
            *puVar5 = 0;
          }
          cVar3 = '\0';
          _Count = _Count & 3;
          if (_Count != 0) goto LAB_10010773;
          return _Dest;
        }
        if ((char)(uVar2 >> 8) == '\0') {
          *puVar5 = uVar2 & 0xff;
          goto joined_r0x100107e5;
        }
        if ((uVar2 & 0xff0000) == 0) {
          *puVar5 = uVar2 & 0xffff;
          goto joined_r0x100107e5;
        }
        if ((uVar2 & 0xff000000) == 0) {
          *puVar5 = uVar2;
          goto joined_r0x100107e5;
        }
      }
      *puVar5 = uVar2;
      puVar5 = puVar5 + 1;
      uVar4 = uVar4 - 1;
joined_r0x1001072c:
    } while (uVar4 != 0);
    _Count = _Count & 3;
    if (_Count == 0) {
      return _Dest;
    }
  }
  do {
    cVar3 = (char)*(uint *)_Source;
    _Source = (char *)((int)_Source + 1);
    *(char *)puVar5 = cVar3;
    puVar5 = (uint *)((int)puVar5 + 1);
    if (cVar3 == '\0') {
      while (_Count = _Count - 1, _Count != 0) {
LAB_10010773:
        *(char *)puVar5 = cVar3;
        puVar5 = (uint *)((int)puVar5 + 1);
      }
      return _Dest;
    }
    _Count = _Count - 1;
  } while (_Count != 0);
  return _Dest;
}


// 10010896 __freea

/* Library Function - Single Match
    __freea
   
   Library: Visual Studio 2005 Release */

void __cdecl __freea(void *_Memory)

{
  if ((_Memory != (void *)0x0) && (*(int *)((int)_Memory + -8) == 0xdddd)) {
    FUN_100012d0((int *)((int)_Memory + -8));
  }
  return;
}


// 100108c0 _strncat

/* Library Function - Single Match
    _strncat
   
   Library: Visual Studio */

char * __cdecl _strncat(char *_Dest,char *_Source,size_t _Count)

{
  byte bVar1;
  uint uVar2;
  uint uVar3;
  uint uVar4;
  uint *puVar5;
  uint *puVar6;
  
  if (_Count != 0) {
    uVar4 = (uint)_Dest & 3;
    puVar5 = (uint *)_Dest;
    while (uVar4 != 0) {
      uVar4 = *puVar5;
      puVar5 = (uint *)((int)puVar5 + 1);
      if ((byte)uVar4 == 0) goto LAB_1001091d;
      uVar4 = (uint)puVar5 & 3;
    }
    do {
      do {
        puVar6 = puVar5;
        puVar5 = puVar6 + 1;
      } while (((*puVar6 ^ 0xffffffff ^ *puVar6 + 0x7efefeff) & 0x81010100) == 0);
      uVar4 = *puVar6;
      if ((char)uVar4 == '\0') goto LAB_1001092f;
      if ((char)(uVar4 >> 8) == '\0') {
        puVar6 = (uint *)((int)puVar6 + 1);
        goto LAB_1001092f;
      }
      if ((uVar4 & 0xff0000) == 0) {
        puVar6 = (uint *)((int)puVar6 + 2);
        goto LAB_1001092f;
      }
    } while ((uVar4 & 0xff000000) != 0);
LAB_1001091d:
    puVar6 = (uint *)((int)puVar5 + -1);
LAB_1001092f:
    if (((uint)_Source & 3) == 0) {
      uVar3 = _Count >> 2;
    }
    else {
      do {
        bVar1 = (byte)*(uint *)_Source;
        uVar4 = (uint)bVar1;
        _Source = (char *)((int)_Source + 1);
        if (bVar1 == 0) goto LAB_1001098a;
        *(byte *)puVar6 = bVar1;
        puVar6 = (uint *)((int)puVar6 + 1);
        _Count = _Count - 1;
        if (_Count == 0) goto LAB_10010980;
      } while (((uint)_Source & 3) != 0);
      uVar3 = _Count >> 2;
    }
    for (; uVar3 != 0; uVar3 = uVar3 - 1) {
      uVar2 = *(uint *)_Source;
      uVar4 = *(uint *)_Source;
      _Source = (char *)((int)_Source + 4);
      if (((uVar2 ^ 0xffffffff ^ uVar2 + 0x7efefeff) & 0x81010100) != 0) {
        if ((char)uVar4 == '\0') {
LAB_1001098a:
          *(byte *)puVar6 = (byte)uVar4;
          return _Dest;
        }
        if ((char)(uVar4 >> 8) == '\0') {
          *(short *)puVar6 = (short)uVar4;
          return _Dest;
        }
        if ((uVar4 & 0xff0000) == 0) {
          *(short *)puVar6 = (short)uVar4;
          *(byte *)((int)puVar6 + 2) = 0;
          return _Dest;
        }
        if ((uVar4 & 0xff000000) == 0) {
          *puVar6 = uVar4;
          return _Dest;
        }
      }
      *puVar6 = uVar4;
      puVar6 = puVar6 + 1;
    }
    for (_Count = _Count & 3; _Count != 0; _Count = _Count - 1) {
      uVar4 = *(uint *)_Source;
      _Source = (char *)((int)_Source + 1);
      *(byte *)puVar6 = (byte)uVar4;
      puVar6 = (uint *)((int)puVar6 + 1);
      if ((byte)uVar4 == 0) {
        return _Dest;
      }
    }
LAB_10010980:
    *(byte *)puVar6 = (byte)_Count;
  }
  return _Dest;
}


// 10010a00 _memmove

/* Library Function - Multiple Matches With Different Base Names
    _memcpy
    _memmove
   
   Libraries: Visual Studio 2005 Debug, Visual Studio 2005 Release, Visual Studio 2008 Debug, Visual
   Studio 2008 Release */

void * __cdecl _memmove(void *_Dst,void *_Src,size_t _Size)

{
  undefined4 *puVar1;
  uint uVar2;
  uint uVar3;
  undefined4 *puVar4;
  
  if ((_Src < _Dst) && (_Dst < (void *)(_Size + (int)_Src))) {
    puVar1 = (undefined4 *)((_Size - 4) + (int)_Src);
    puVar4 = (undefined4 *)((_Size - 4) + (int)_Dst);
    if (((uint)puVar4 & 3) == 0) {
      uVar2 = _Size >> 2;
      uVar3 = _Size & 3;
      if (7 < uVar2) {
        for (; uVar2 != 0; uVar2 = uVar2 - 1) {
          *puVar4 = *puVar1;
          puVar1 = puVar1 + -1;
          puVar4 = puVar4 + -1;
        }
        switch(uVar3) {
        case 0:
          return _Dst;
        case 2:
          goto switchD_10010be3_caseD_2;
        case 3:
          goto switchD_10010be3_caseD_3;
        }
        goto switchD_10010be3_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_10010be3_caseD_0;
      case 1:
        goto switchD_10010be3_caseD_1;
      case 2:
        goto switchD_10010be3_caseD_2;
      case 3:
        goto switchD_10010be3_caseD_3;
      default:
        uVar2 = _Size - ((uint)puVar4 & 3);
        switch((uint)puVar4 & 3) {
        case 1:
          uVar3 = uVar2 & 3;
          *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
          puVar1 = (undefined4 *)((int)puVar1 + -1);
          uVar2 = uVar2 >> 2;
          puVar4 = (undefined4 *)((int)puVar4 - 1);
          if (7 < uVar2) {
            for (; uVar2 != 0; uVar2 = uVar2 - 1) {
              *puVar4 = *puVar1;
              puVar1 = puVar1 + -1;
              puVar4 = puVar4 + -1;
            }
            switch(uVar3) {
            case 0:
              return _Dst;
            case 2:
              goto switchD_10010be3_caseD_2;
            case 3:
              goto switchD_10010be3_caseD_3;
            }
            goto switchD_10010be3_caseD_1;
          }
          break;
        case 2:
          uVar3 = uVar2 & 3;
          *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
          uVar2 = uVar2 >> 2;
          *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
          puVar1 = (undefined4 *)((int)puVar1 + -2);
          puVar4 = (undefined4 *)((int)puVar4 - 2);
          if (7 < uVar2) {
            for (; uVar2 != 0; uVar2 = uVar2 - 1) {
              *puVar4 = *puVar1;
              puVar1 = puVar1 + -1;
              puVar4 = puVar4 + -1;
            }
            switch(uVar3) {
            case 0:
              return _Dst;
            case 2:
              goto switchD_10010be3_caseD_2;
            case 3:
              goto switchD_10010be3_caseD_3;
            }
            goto switchD_10010be3_caseD_1;
          }
          break;
        case 3:
          uVar3 = uVar2 & 3;
          *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
          *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
          uVar2 = uVar2 >> 2;
          *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
          puVar1 = (undefined4 *)((int)puVar1 + -3);
          puVar4 = (undefined4 *)((int)puVar4 - 3);
          if (7 < uVar2) {
            for (; uVar2 != 0; uVar2 = uVar2 - 1) {
              *puVar4 = *puVar1;
              puVar1 = puVar1 + -1;
              puVar4 = puVar4 + -1;
            }
            switch(uVar3) {
            case 0:
              return _Dst;
            case 2:
              goto switchD_10010be3_caseD_2;
            case 3:
              goto switchD_10010be3_caseD_3;
            }
            goto switchD_10010be3_caseD_1;
          }
        }
      }
    }
    switch(uVar2) {
    case 7:
      puVar4[7 - uVar2] = puVar1[7 - uVar2];
    case 6:
      puVar4[6 - uVar2] = puVar1[6 - uVar2];
    case 5:
      puVar4[5 - uVar2] = puVar1[5 - uVar2];
    case 4:
      puVar4[4 - uVar2] = puVar1[4 - uVar2];
    case 3:
      puVar4[3 - uVar2] = puVar1[3 - uVar2];
    case 2:
      puVar4[2 - uVar2] = puVar1[2 - uVar2];
    case 1:
      puVar4[1 - uVar2] = puVar1[1 - uVar2];
      puVar1 = puVar1 + -uVar2;
      puVar4 = puVar4 + -uVar2;
    }
    switch(uVar3) {
    case 1:
switchD_10010be3_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_10010be3_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_10010be3_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_10010be3_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_1004bcfc != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
    puVar1 = __VEC_memcpy((undefined4 *)_Dst,(undefined4 *)_Src,_Size);
    return puVar1;
  }
  puVar1 = (undefined4 *)_Dst;
  if (((uint)_Dst & 3) == 0) {
    uVar2 = _Size >> 2;
    uVar3 = _Size & 3;
    if (7 < uVar2) {
                    /* WARNING: Load size is inaccurate */
      for (; uVar2 != 0; uVar2 = uVar2 - 1) {
        *puVar1 = *_Src;
        _Src = (undefined4 *)((int)_Src + 4);
        puVar1 = puVar1 + 1;
      }
      switch(uVar3) {
      case 0:
        return _Dst;
      case 2:
        goto switchD_10010a5c_caseD_2;
      case 3:
        goto switchD_10010a5c_caseD_3;
      }
      goto switchD_10010a5c_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_10010a5c_caseD_0;
    case 1:
      goto switchD_10010a5c_caseD_1;
    case 2:
      goto switchD_10010a5c_caseD_2;
    case 3:
      goto switchD_10010a5c_caseD_3;
    default:
      uVar2 = (_Size - 4) + ((uint)_Dst & 3);
      switch((uint)_Dst & 3) {
      case 1:
        uVar3 = uVar2 & 3;
                    /* WARNING: Load size is inaccurate */
        *(undefined1 *)_Dst = *_Src;
        *(undefined1 *)((int)_Dst + 1) = *(undefined1 *)((int)_Src + 1);
        uVar2 = uVar2 >> 2;
        *(undefined1 *)((int)_Dst + 2) = *(undefined1 *)((int)_Src + 2);
        _Src = (void *)((int)_Src + 3);
        puVar1 = (undefined4 *)((int)_Dst + 3);
        if (7 < uVar2) {
                    /* WARNING: Load size is inaccurate */
          for (; uVar2 != 0; uVar2 = uVar2 - 1) {
            *puVar1 = *_Src;
            _Src = (undefined4 *)((int)_Src + 4);
            puVar1 = puVar1 + 1;
          }
          switch(uVar3) {
          case 0:
            return _Dst;
          case 2:
            goto switchD_10010a5c_caseD_2;
          case 3:
            goto switchD_10010a5c_caseD_3;
          }
          goto switchD_10010a5c_caseD_1;
        }
        break;
      case 2:
        uVar3 = uVar2 & 3;
                    /* WARNING: Load size is inaccurate */
        *(undefined1 *)_Dst = *_Src;
        uVar2 = uVar2 >> 2;
        *(undefined1 *)((int)_Dst + 1) = *(undefined1 *)((int)_Src + 1);
        _Src = (void *)((int)_Src + 2);
        puVar1 = (undefined4 *)((int)_Dst + 2);
        if (7 < uVar2) {
                    /* WARNING: Load size is inaccurate */
          for (; uVar2 != 0; uVar2 = uVar2 - 1) {
            *puVar1 = *_Src;
            _Src = (undefined4 *)((int)_Src + 4);
            puVar1 = puVar1 + 1;
          }
          switch(uVar3) {
          case 0:
            return _Dst;
          case 2:
            goto switchD_10010a5c_caseD_2;
          case 3:
            goto switchD_10010a5c_caseD_3;
          }
          goto switchD_10010a5c_caseD_1;
        }
        break;
      case 3:
        uVar3 = uVar2 & 3;
                    /* WARNING: Load size is inaccurate */
        *(undefined1 *)_Dst = *_Src;
        _Src = (void *)((int)_Src + 1);
        uVar2 = uVar2 >> 2;
        puVar1 = (undefined4 *)((int)_Dst + 1);
        if (7 < uVar2) {
                    /* WARNING: Load size is inaccurate */
          for (; uVar2 != 0; uVar2 = uVar2 - 1) {
            *puVar1 = *_Src;
            _Src = (undefined4 *)((int)_Src + 4);
            puVar1 = puVar1 + 1;
          }
          switch(uVar3) {
          case 0:
            return _Dst;
          case 2:
            goto switchD_10010a5c_caseD_2;
          case 3:
            goto switchD_10010a5c_caseD_3;
          }
          goto switchD_10010a5c_caseD_1;
        }
      }
    }
  }
  switch(uVar2) {
  case 7:
    puVar1[uVar2 - 7] = *(undefined4 *)((int)_Src + (uVar2 - 7) * 4);
  case 6:
    puVar1[uVar2 - 6] = *(undefined4 *)((int)_Src + (uVar2 - 6) * 4);
  case 5:
    puVar1[uVar2 - 5] = *(undefined4 *)((int)_Src + (uVar2 - 5) * 4);
  case 4:
    puVar1[uVar2 - 4] = *(undefined4 *)((int)_Src + (uVar2 - 4) * 4);
  case 3:
    puVar1[uVar2 - 3] = *(undefined4 *)((int)_Src + (uVar2 - 3) * 4);
  case 2:
    puVar1[uVar2 - 2] = *(undefined4 *)((int)_Src + (uVar2 - 2) * 4);
  case 1:
    puVar1[uVar2 - 1] = *(undefined4 *)((int)_Src + (uVar2 - 1) * 4);
    _Src = (void *)((int)_Src + uVar2 * 4);
    puVar1 = puVar1 + uVar2;
  }
  switch(uVar3) {
  case 1:
switchD_10010a5c_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_10010a5c_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_10010a5c_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_10010a5c_caseD_0:
  return _Dst;
}


// 10010ee0 __alloca_probe

/* WARNING: This is an inlined function */
/* Library Function - Single Match
    __chkstk
   
   Library: Visual Studio */

void __alloca_probe(void)

{
  undefined1 *in_EAX;
  undefined4 *puVar1;
  undefined4 *puVar2;
  undefined4 unaff_retaddr;
  undefined1 auStack_4 [4];
  
  puVar2 = (undefined4 *)((int)&stack0x00000000 - (int)in_EAX & ~-(uint)(&stack0x00000000 < in_EAX))
  ;
  for (puVar1 = (undefined4 *)((uint)auStack_4 & 0xfffff000); puVar2 < puVar1;
      puVar1 = puVar1 + -0x400) {
  }
  *puVar2 = unaff_retaddr;
  return;
}


// 10010f0b __toupper_l

/* Library Function - Single Match
    __toupper_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __toupper_l(int _C,_locale_t _Locale)

{
  int iVar1;
  uint uVar2;
  int iVar3;
  DWORD *pDVar4;
  CHAR CVar5;
  localeinfo_struct local_1c;
  int local_14;
  char local_10;
  byte local_c;
  undefined1 local_b;
  CHAR local_8;
  CHAR local_7;
  undefined1 local_6;
  
  iVar1 = _C;
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_1c,_Locale);
  if ((uint)_C < 0x100) {
    if ((int)(local_1c.locinfo)->locale_name[3] < 2) {
      uVar2 = (byte)local_1c.locinfo[1].lc_category[0].locale[_C * 2] & 2;
    }
    else {
      uVar2 = __isctype_l(_C,2,&local_1c);
    }
    if (uVar2 == 0) {
LAB_10010f68:
      if (local_10 == '\0') {
        return iVar1;
      }
      *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
      return iVar1;
    }
    uVar2 = (uint)*(byte *)((int)local_1c.locinfo[1].lc_category[0].refcount + _C);
  }
  else {
    CVar5 = (CHAR)_C;
    if (((int)(local_1c.locinfo)->locale_name[3] < 2) ||
       (iVar3 = __isleadbyte_l(_C >> 8 & 0xff,&local_1c), iVar3 == 0)) {
      pDVar4 = FUN_10011e63();
      *pDVar4 = 0x2a;
      local_7 = '\0';
      iVar3 = 1;
      local_8 = CVar5;
    }
    else {
      _C._0_1_ = (CHAR)((uint)_C >> 8);
      local_8 = (CHAR)_C;
      local_6 = 0;
      iVar3 = 2;
      local_7 = CVar5;
    }
    iVar3 = ___crtLCMapStringA(&local_1c,(local_1c.locinfo)->lc_category[0].wlocale,0x200,&local_8,
                               iVar3,(LPSTR)&local_c,3,(local_1c.locinfo)->lc_codepage,1);
    if (iVar3 == 0) goto LAB_10010f68;
    if (iVar3 == 1) {
      uVar2 = (uint)local_c;
    }
    else {
      uVar2 = (uint)CONCAT11(local_c,local_b);
    }
  }
  if (local_10 != '\0') {
    *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
  }
  return uVar2;
}


// 10011023 _toupper

/* Library Function - Single Match
    _toupper
   
   Library: Visual Studio 2005 Release */

int __cdecl _toupper(int _C)

{
  if (DAT_1003a1cc == 0) {
    if (_C - 0x61U < 0x1a) {
      return _C + -0x20;
    }
  }
  else {
    _C = __toupper_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 10011050 __allshl

/* Library Function - Single Match
    __allshl
   
   Library: Visual Studio */

longlong __fastcall __allshl(byte param_1,int param_2)

{
  uint in_EAX;
  
  if (0x3f < param_1) {
    return 0;
  }
  if (param_1 < 0x20) {
    return CONCAT44(param_2 << (param_1 & 0x1f) | in_EAX >> 0x20 - (param_1 & 0x1f),
                    in_EAX << (param_1 & 0x1f));
  }
  return (ulonglong)(in_EAX << (param_1 & 0x1f)) << 0x20;
}


// 1001106f fastzero_I

/* Library Function - Single Match
    _fastzero_I
   
   Libraries: Visual Studio 2005, Visual Studio 2008, Visual Studio 2019 */

void __cdecl fastzero_I(undefined1 (*param_1) [16],uint param_2)

{
  uint uVar1;
  
  uVar1 = param_2 >> 7;
  do {
    *param_1 = (undefined1  [16])0x0;
    param_1[1] = (undefined1  [16])0x0;
    param_1[2] = (undefined1  [16])0x0;
    param_1[3] = (undefined1  [16])0x0;
    param_1[4] = (undefined1  [16])0x0;
    param_1[5] = (undefined1  [16])0x0;
    param_1[6] = (undefined1  [16])0x0;
    param_1[7] = (undefined1  [16])0x0;
    param_1 = param_1 + 8;
    uVar1 = uVar1 - 1;
  } while (uVar1 != 0);
  return;
}


// 100110c6 __VEC_memzero

/* Library Function - Single Match
    __VEC_memzero
   
   Libraries: Visual Studio 2005 Debug, Visual Studio 2005 Release, Visual Studio 2008 Debug, Visual
   Studio 2008 Release */

undefined1 (*) [16] __cdecl
__VEC_memzero(undefined1 (*param_1) [16],undefined4 param_2,uint param_3)

{
  int iVar1;
  uint uVar2;
  int iVar3;
  undefined1 *puVar4;
  undefined1 (*pauVar5) [16];
  
  uVar2 = (int)param_1 >> 0x1f;
  iVar3 = (((uint)param_1 ^ uVar2) - uVar2 & 0xf ^ uVar2) - uVar2;
  if (iVar3 == 0) {
    uVar2 = param_3 & 0x7f;
    if (param_3 != uVar2) {
      fastzero_I(param_1,param_3 - uVar2);
    }
    if (uVar2 != 0) {
      puVar4 = (undefined1 *)((int)param_1 + (param_3 - uVar2));
      for (; uVar2 != 0; uVar2 = uVar2 - 1) {
        *puVar4 = 0;
        puVar4 = puVar4 + 1;
      }
    }
  }
  else {
    iVar3 = 0x10 - iVar3;
    pauVar5 = param_1;
    for (iVar1 = iVar3; iVar1 != 0; iVar1 = iVar1 + -1) {
      (*pauVar5)[0] = 0;
      pauVar5 = (undefined1 (*) [16])(*pauVar5 + 1);
    }
    __VEC_memzero((undefined1 (*) [16])((int)param_1 + iVar3),0,param_3 - iVar3);
  }
  return param_1;
}


// 10011169 __flsbuf

/* Library Function - Single Match
    __flsbuf
   
   Library: Visual Studio 2005 Release */

int __cdecl __flsbuf(int _Ch,FILE *_File)

{
  char *_Buf;
  char *pcVar1;
  FILE *_File_00;
  DWORD *pDVar2;
  undefined **ppuVar3;
  int iVar4;
  undefined *puVar5;
  int unaff_EDI;
  uint uVar6;
  longlong lVar7;
  uint local_8;
  
  _File_00 = _File;
  _File = (FILE *)__fileno(_File);
  uVar6 = _File_00->_flag;
  if ((uVar6 & 0x82) == 0) {
    pDVar2 = FUN_10011e63();
    *pDVar2 = 9;
LAB_1001118d:
    _File_00->_flag = _File_00->_flag | 0x20;
    return -1;
  }
  if ((uVar6 & 0x40) != 0) {
    pDVar2 = FUN_10011e63();
    *pDVar2 = 0x22;
    goto LAB_1001118d;
  }
  if ((uVar6 & 1) != 0) {
    _File_00->_cnt = 0;
    if ((uVar6 & 0x10) == 0) {
      _File_00->_flag = uVar6 | 0x20;
      return -1;
    }
    _File_00->_ptr = _File_00->_base;
    _File_00->_flag = uVar6 & 0xfffffffe;
  }
  uVar6 = _File_00->_flag;
  _File_00->_flag = uVar6 & 0xffffffef | 2;
  _File_00->_cnt = 0;
  local_8 = 0;
  if (((uVar6 & 0x10c) == 0) &&
     (((ppuVar3 = FUN_10019493(), _File_00 != (FILE *)(ppuVar3 + 8) &&
       (ppuVar3 = FUN_10019493(), _File_00 != (FILE *)(ppuVar3 + 0x10))) ||
      (iVar4 = __isatty((int)_File), iVar4 == 0)))) {
    __getbuf(_File_00);
  }
  if ((_File_00->_flag & 0x108) == 0) {
    uVar6 = 1;
    local_8 = __write((int)_File,&_Ch,1);
  }
  else {
    _Buf = _File_00->_base;
    pcVar1 = _File_00->_ptr;
    _File_00->_ptr = _Buf + 1;
    uVar6 = (int)pcVar1 - (int)_Buf;
    _File_00->_cnt = _File_00->_bufsiz + -1;
    if ((int)uVar6 < 1) {
      if ((_File == (FILE *)0xffffffff) || (_File == (FILE *)0xfffffffe)) {
        puVar5 = &DAT_100365e8;
      }
      else {
        puVar5 = (undefined *)(((uint)_File & 0x1f) * 0x38 + (&DAT_1004bbe0)[(int)_File >> 5]);
      }
      if (((puVar5[4] & 0x20) != 0) &&
         (lVar7 = __lseeki64((int)_File,0x200000000,unaff_EDI), lVar7 == -1)) goto LAB_100112b3;
    }
    else {
      local_8 = __write((int)_File,_Buf,uVar6);
    }
    *_File_00->_base = (char)_Ch;
  }
  if (local_8 == uVar6) {
    return _Ch & 0xff;
  }
LAB_100112b3:
  _File_00->_flag = _File_00->_flag | 0x20;
  return -1;
}


// 100112c9 write_char

/* Library Function - Single Match
    _write_char
   
   Library: Visual Studio 2005 Release */

void __cdecl write_char(void)

{
  int *piVar1;
  byte in_AL;
  uint uVar2;
  FILE *in_ECX;
  int *unaff_ESI;
  
  if (((in_ECX->_flag & 0x40) == 0) || (in_ECX->_base != (char *)0x0)) {
    piVar1 = &in_ECX->_cnt;
    *piVar1 = *piVar1 + -1;
    if (*piVar1 < 0) {
      uVar2 = __flsbuf((int)(char)in_AL,in_ECX);
    }
    else {
      *in_ECX->_ptr = in_AL;
      in_ECX->_ptr = in_ECX->_ptr + 1;
      uVar2 = (uint)in_AL;
    }
    if (uVar2 == 0xffffffff) {
      *unaff_ESI = -1;
      return;
    }
  }
  *unaff_ESI = *unaff_ESI + 1;
  return;
}


// 100112fc write_multi_char

/* Library Function - Single Match
    _write_multi_char
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release, Visual Studio 2008 Release,
   Visual Studio 2010 Release */

void __cdecl write_multi_char(undefined4 param_1,int param_2)

{
  int *in_EAX;
  
  do {
    if (param_2 < 1) {
      return;
    }
    param_2 = param_2 + -1;
    write_char();
  } while (*in_EAX != -1);
  return;
}


// 10011320 write_string

/* Library Function - Single Match
    _write_string
   
   Library: Visual Studio 2005 Release */

void __cdecl write_string(int param_1)

{
  int *in_EAX;
  DWORD *pDVar1;
  int unaff_EDI;
  
  if (((*(byte *)(unaff_EDI + 0xc) & 0x40) == 0) || (*(int *)(unaff_EDI + 8) != 0)) {
    while (0 < param_1) {
      param_1 = param_1 + -1;
      write_char();
      if (*in_EAX == -1) {
        pDVar1 = FUN_10011e63();
        if (*pDVar1 != 0x2a) {
          return;
        }
        write_char();
      }
    }
  }
  else {
    *in_EAX = *in_EAX + param_1;
  }
  return;
}


// 1001136a FUN_1001136a

/* WARNING: Type propagation algorithm not settling */

void __cdecl FUN_1001136a(FILE *param_1,byte *param_2,localeinfo_struct *param_3,wchar_t *param_4)

{
  byte bVar1;
  wchar_t _WCh;
  short *psVar2;
  int *piVar3;
  DWORD *pDVar4;
  int iVar5;
  uint uVar6;
  undefined *puVar7;
  undefined3 extraout_var;
  int iVar8;
  code *pcVar9;
  errno_t eVar10;
  int extraout_ECX;
  uint uVar11;
  byte *pbVar12;
  size_t sVar13;
  wchar_t *pwVar14;
  bool bVar15;
  undefined8 uVar16;
  undefined4 *puVar17;
  wchar_t *pwVar18;
  undefined4 uVar19;
  localeinfo_struct *plVar20;
  undefined4 local_27c;
  undefined4 local_278;
  undefined4 local_274;
  int local_270;
  int local_26c [2];
  size_t local_264;
  localeinfo_struct local_260;
  int local_258;
  char local_254;
  wchar_t *local_250;
  int local_24c;
  int local_248;
  byte *local_244;
  int local_240;
  int local_23c;
  int local_238;
  undefined1 local_234;
  char local_233;
  int local_230;
  FILE *local_22c;
  wchar_t *local_228;
  size_t local_224;
  wchar_t *local_220;
  int local_21c;
  byte local_215;
  uint local_214;
  wchar_t local_210 [10];
  undefined1 local_1fc [491];
  undefined2 local_11;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)local_1fc;
  local_22c = param_1;
  local_228 = param_4;
  local_248 = 0;
  local_214 = 0;
  local_23c = 0;
  local_21c = 0;
  local_238 = 0;
  local_24c = 0;
  local_240 = 0;
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_260,param_3);
  if (local_22c != (FILE *)0x0) {
    if ((local_22c->_flag & 0x40) == 0) {
      iVar5 = __fileno(local_22c);
      if ((iVar5 == -1) || (iVar5 = __fileno(local_22c), iVar5 == -2)) {
        puVar7 = &DAT_100365e8;
      }
      else {
        iVar5 = __fileno(local_22c);
        uVar6 = __fileno(local_22c);
        puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar5 >> 5]);
      }
      if ((puVar7[0x24] & 0x7f) == 0) {
        iVar5 = __fileno(local_22c);
        if ((iVar5 == -1) || (iVar5 = __fileno(local_22c), iVar5 == -2)) {
          puVar7 = &DAT_100365e8;
        }
        else {
          iVar5 = __fileno(local_22c);
          uVar6 = __fileno(local_22c);
          puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar5 >> 5]);
        }
        if ((puVar7[0x24] & 0x80) == 0) goto LAB_100114a8;
      }
    }
    else {
LAB_100114a8:
      if (param_2 != (byte *)0x0) {
        local_215 = *param_2;
        local_230 = 0;
        local_224 = 0;
        local_250 = (wchar_t *)0x0;
        iVar5 = 0;
        while ((local_215 != 0 && (pbVar12 = param_2 + 1, local_244 = pbVar12, -1 < local_230))) {
          if ((byte)(local_215 - 0x20) < 0x59) {
            uVar6 = *(byte *)((int)&DAT_10029120 + (int)(char)local_215) & 0xf;
          }
          else {
            uVar6 = 0;
          }
          local_270 = (int)(char)(&DAT_10029140)[uVar6 * 8 + iVar5] >> 4;
          switch(local_270) {
          case 0:
switchD_1001150c_caseD_0:
            local_240 = 0;
            iVar5 = __isleadbyte_l((uint)local_215,&local_260);
            if (iVar5 != 0) {
              write_char();
              local_244 = param_2 + 2;
              if (*pbVar12 == 0) {
LAB_10011c9f:
                pDVar4 = FUN_10011e63();
                *pDVar4 = 0x16;
                goto LAB_100113da;
              }
            }
            write_char();
            break;
          case 1:
            local_21c = -1;
            local_274 = 0;
            local_24c = 0;
            local_23c = 0;
            local_238 = 0;
            local_214 = 0;
            local_240 = 0;
            break;
          case 2:
            if (local_215 == 0x20) {
              local_214 = local_214 | 2;
            }
            else if (local_215 == 0x23) {
              local_214 = local_214 | 0x80;
            }
            else if (local_215 == 0x2b) {
              local_214 = local_214 | 1;
            }
            else if (local_215 == 0x2d) {
              local_214 = local_214 | 4;
            }
            else if (local_215 == 0x30) {
              local_214 = local_214 | 8;
            }
            break;
          case 3:
            if (local_215 == 0x2a) {
              local_228 = param_4 + 2;
              local_23c = *(int *)param_4;
              if (local_23c < 0) {
                local_214 = local_214 | 4;
                local_23c = -local_23c;
              }
            }
            else {
              local_23c = local_23c * 10 + -0x30 + (int)(char)local_215;
            }
            break;
          case 4:
            local_21c = 0;
            break;
          case 5:
            if (local_215 == 0x2a) {
              local_228 = param_4 + 2;
              local_21c = *(int *)param_4;
              if (local_21c < 0) {
                local_21c = -1;
              }
            }
            else {
              local_21c = local_21c * 10 + -0x30 + (int)(char)local_215;
            }
            break;
          case 6:
            if (local_215 == 0x49) {
              bVar1 = *pbVar12;
              if ((bVar1 == 0x36) && (param_2[2] == 0x34)) {
                local_214 = local_214 | 0x8000;
                local_244 = param_2 + 3;
              }
              else if ((bVar1 == 0x33) && (param_2[2] == 0x32)) {
                local_214 = local_214 & 0xffff7fff;
                local_244 = param_2 + 3;
              }
              else if (((((bVar1 != 100) && (bVar1 != 0x69)) && (bVar1 != 0x6f)) &&
                       ((bVar1 != 0x75 && (bVar1 != 0x78)))) && (bVar1 != 0x58)) {
                local_270 = 0;
                goto switchD_1001150c_caseD_0;
              }
            }
            else if (local_215 == 0x68) {
              local_214 = local_214 | 0x20;
            }
            else if (local_215 == 0x6c) {
              if (*pbVar12 == 0x6c) {
                local_214 = local_214 | 0x1000;
                local_244 = param_2 + 2;
              }
              else {
                local_214 = local_214 | 0x10;
              }
            }
            else if (local_215 == 0x77) {
              local_214 = local_214 | 0x800;
            }
            break;
          case 7:
            if ((char)local_215 < 'e') {
              if (local_215 == 100) {
LAB_100118eb:
                local_214 = local_214 | 0x40;
LAB_100118ef:
                local_224 = 10;
LAB_100118f6:
                if (((short)local_214 < 0) || ((local_214 & 0x1000) != 0)) {
                  uVar6 = *(uint *)param_4;
                  uVar11 = *(uint *)(param_4 + 2);
                  local_228 = param_4 + 4;
                }
                else {
                  local_228 = param_4 + 2;
                  if ((local_214 & 0x20) == 0) {
                    uVar6 = *(uint *)param_4;
                    if ((local_214 & 0x40) == 0) {
                      uVar11 = 0;
                    }
                    else {
                      uVar11 = (int)uVar6 >> 0x1f;
                    }
                  }
                  else {
                    if ((local_214 & 0x40) == 0) {
                      uVar6 = (uint)(ushort)*param_4;
                    }
                    else {
                      uVar6 = (uint)*param_4;
                    }
                    uVar11 = (int)uVar6 >> 0x1f;
                  }
                }
                if ((((local_214 & 0x40) != 0) && ((int)uVar11 < 1)) && ((int)uVar11 < 0)) {
                  bVar15 = uVar6 != 0;
                  uVar6 = -uVar6;
                  uVar11 = -(uVar11 + bVar15);
                  local_214 = local_214 | 0x100;
                }
                uVar16 = CONCAT44(uVar11,uVar6);
                if ((local_214 & 0x9000) == 0) {
                  uVar11 = 0;
                }
                if (local_21c < 0) {
                  local_21c = 1;
                }
                else {
                  local_214 = local_214 & 0xfffffff7;
                  if (0x200 < local_21c) {
                    local_21c = 0x200;
                  }
                }
                if (uVar6 == 0 && uVar11 == 0) {
                  local_238 = 0;
                }
                pwVar14 = &local_11;
                while( true ) {
                  uVar6 = uVar11;
                  iVar5 = local_21c + -1;
                  if ((local_21c < 1) && ((uint)uVar16 == 0 && uVar6 == 0)) break;
                  local_21c = iVar5;
                  uVar16 = __aulldvrm((uint)uVar16,uVar6,local_224,(int)local_224 >> 0x1f);
                  iVar5 = extraout_ECX + 0x30;
                  if (0x39 < iVar5) {
                    iVar5 = iVar5 + local_248;
                  }
                  *(char *)pwVar14 = (char)iVar5;
                  pwVar14 = (wchar_t *)((int)pwVar14 + -1);
                  uVar11 = (uint)((ulonglong)uVar16 >> 0x20);
                  local_264 = uVar6;
                }
                local_224 = (int)&local_11 + -(int)pwVar14;
                local_220 = (wchar_t *)((int)pwVar14 + 1);
                local_21c = iVar5;
                if (((local_214 & 0x200) != 0) && ((local_224 == 0 || (*(char *)local_220 != '0'))))
                {
                  *(char *)pwVar14 = '0';
                  local_224 = (int)&local_11 + -(int)pwVar14 + 1;
                  local_220 = pwVar14;
                }
              }
              else if ((char)local_215 < 'T') {
                if (local_215 == 0x53) {
                  if ((local_214 & 0x830) == 0) {
                    local_214 = local_214 | 0x800;
                  }
                  goto LAB_10011774;
                }
                if (local_215 == 0x41) {
LAB_10011720:
                  local_215 = local_215 + 0x20;
                  local_274 = 1;
LAB_1001172d:
                  local_214 = local_214 | 0x40;
                  local_264 = 0x200;
                  pwVar14 = local_210;
                  sVar13 = local_264;
                  pwVar18 = local_210;
                  if (local_21c < 0) {
                    local_21c = 6;
                  }
                  else if (local_21c == 0) {
                    if (local_215 == 0x67) {
                      local_21c = 1;
                    }
                  }
                  else {
                    if (0x200 < local_21c) {
                      local_21c = 0x200;
                    }
                    if (0xa3 < local_21c) {
                      sVar13 = local_21c + 0x15d;
                      local_220 = local_210;
                      local_250 = (wchar_t *)FUN_10001270(sVar13);
                      pwVar14 = local_250;
                      pwVar18 = local_250;
                      if (local_250 == (wchar_t *)0x0) {
                        local_21c = 0xa3;
                        pwVar14 = local_210;
                        sVar13 = local_264;
                        pwVar18 = local_220;
                      }
                    }
                  }
                  local_220 = pwVar18;
                  local_264 = sVar13;
                  local_27c = *(undefined4 *)param_4;
                  local_228 = param_4 + 4;
                  local_278 = *(undefined4 *)(param_4 + 2);
                  plVar20 = &local_260;
                  iVar8 = (int)(char)local_215;
                  puVar17 = &local_27c;
                  pwVar18 = pwVar14;
                  sVar13 = local_264;
                  iVar5 = local_21c;
                  uVar19 = local_274;
                  pcVar9 = (code *)FUN_10013035((int)PTR_LAB_10035ee0);
                  (*pcVar9)(puVar17,pwVar18,sVar13,iVar8,iVar5,uVar19,plVar20);
                  uVar6 = local_214 & 0x80;
                  if ((uVar6 != 0) && (local_21c == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_10013035((int)PTR_LAB_10035eec);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((local_215 == 0x67) && (uVar6 == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_10013035((int)PTR_LAB_10035ee8);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((char)*pwVar14 == '-') {
                    local_214 = local_214 | 0x100;
                    local_220 = (wchar_t *)((int)pwVar14 + 1);
                    pwVar14 = local_220;
                  }
LAB_10011861:
                  local_224 = _strlen((char *)pwVar14);
                }
                else if (local_215 == 0x43) {
                  if ((local_214 & 0x830) == 0) {
                    local_214 = local_214 | 0x800;
                  }
LAB_100117d1:
                  local_228 = param_4 + 2;
                  if ((local_214 & 0x810) == 0) {
                    local_210[0]._0_1_ = (char)*param_4;
                    local_224 = 1;
                  }
                  else {
                    eVar10 = _wctomb_s((int *)&local_224,(char *)local_210,0x200,*param_4);
                    if (eVar10 != 0) {
                      local_24c = 1;
                    }
                  }
                  local_220 = local_210;
                }
                else if ((local_215 == 0x45) || (local_215 == 0x47)) goto LAB_10011720;
              }
              else {
                if (local_215 == 0x58) goto LAB_100119f6;
                if (local_215 == 0x5a) {
                  psVar2 = *(short **)param_4;
                  local_228 = param_4 + 2;
                  pwVar14 = (wchar_t *)PTR_s__null__10035d50;
                  local_220 = (wchar_t *)PTR_s__null__10035d50;
                  if ((psVar2 == (short *)0x0) ||
                     (pwVar18 = *(wchar_t **)(psVar2 + 2), pwVar18 == (wchar_t *)0x0))
                  goto LAB_10011861;
                  local_224 = (size_t)*psVar2;
                  local_220 = pwVar18;
                  if ((local_214 & 0x800) == 0) {
                    local_240 = 0;
                  }
                  else {
                    local_224 = (int)local_224 / 2;
                    local_240 = 1;
                  }
                }
                else {
                  if (local_215 == 0x61) goto LAB_1001172d;
                  if (local_215 == 99) goto LAB_100117d1;
                }
              }
LAB_10011b6e:
              if (local_24c == 0) {
                if ((local_214 & 0x40) != 0) {
                  if ((local_214 & 0x100) == 0) {
                    if ((local_214 & 1) == 0) {
                      if ((local_214 & 2) == 0) goto LAB_10011ba4;
                      local_234 = 0x20;
                    }
                    else {
                      local_234 = 0x2b;
                    }
                  }
                  else {
                    local_234 = 0x2d;
                  }
                  local_238 = 1;
                }
LAB_10011ba4:
                iVar5 = (local_23c - local_224) - local_238;
                if ((local_214 & 0xc) == 0) {
                  write_multi_char(0x20,iVar5);
                }
                write_string(local_238);
                if (((local_214 & 8) != 0) && ((local_214 & 4) == 0)) {
                  write_multi_char(0x30,iVar5);
                }
                if ((local_240 == 0) || ((int)local_224 < 1)) {
                  write_string(local_224);
                }
                else {
                  local_264 = local_224;
                  pwVar14 = local_220;
                  do {
                    _WCh = *pwVar14;
                    local_264 = local_264 - 1;
                    pwVar14 = pwVar14 + 1;
                    eVar10 = _wctomb_s(local_26c,(char *)((int)&local_11 + 1),6,_WCh);
                    if ((eVar10 != 0) || (local_26c[0] == 0)) {
                      local_230 = -1;
                      break;
                    }
                    write_string(local_26c[0]);
                  } while (local_264 != 0);
                }
                if ((-1 < local_230) && ((local_214 & 4) != 0)) {
                  write_multi_char(0x20,iVar5);
                }
              }
            }
            else {
              if ('p' < (char)local_215) {
                if (local_215 == 0x73) {
LAB_10011774:
                  iVar5 = local_21c;
                  if (local_21c == -1) {
                    iVar5 = 0x7fffffff;
                  }
                  local_228 = param_4 + 2;
                  local_220 = *(wchar_t **)param_4;
                  if ((local_214 & 0x810) == 0) {
                    pwVar14 = local_220;
                    if (local_220 == (wchar_t *)0x0) {
                      pwVar14 = (wchar_t *)PTR_s__null__10035d50;
                      local_220 = (wchar_t *)PTR_s__null__10035d50;
                    }
                    for (; (iVar5 != 0 && (iVar5 = iVar5 + -1, (char)*pwVar14 != '\0'));
                        pwVar14 = (wchar_t *)((int)pwVar14 + 1)) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220;
                  }
                  else {
                    if (local_220 == (wchar_t *)0x0) {
                      local_220 = (wchar_t *)PTR_u__null__10035d54;
                    }
                    local_240 = 1;
                    for (pwVar14 = local_220;
                        (iVar5 != 0 && (iVar5 = iVar5 + -1, *pwVar14 != L'\0'));
                        pwVar14 = pwVar14 + 1) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220 >> 1;
                  }
                  goto LAB_10011b6e;
                }
                if (local_215 == 0x75) goto LAB_100118ef;
                if (local_215 != 0x78) goto LAB_10011b6e;
                local_248 = 0x27;
LAB_10011a1c:
                local_224 = 0x10;
                if ((local_214 & 0x80) != 0) {
                  local_233 = (char)local_248 + 'Q';
                  local_234 = 0x30;
                  local_238 = 2;
                }
                goto LAB_100118f6;
              }
              if (local_215 == 0x70) {
                local_21c = 8;
LAB_100119f6:
                local_248 = 7;
                goto LAB_10011a1c;
              }
              if ((char)local_215 < 'e') goto LAB_10011b6e;
              if ((char)local_215 < 'h') goto LAB_1001172d;
              if (local_215 == 0x69) goto LAB_100118eb;
              if (local_215 != 0x6e) {
                if (local_215 != 0x6f) goto LAB_10011b6e;
                local_224 = 8;
                if ((local_214 & 0x80) != 0) {
                  local_214 = local_214 | 0x200;
                }
                goto LAB_100118f6;
              }
              piVar3 = *(int **)param_4;
              local_228 = param_4 + 2;
              bVar15 = FUN_10019661();
              if (CONCAT31(extraout_var,bVar15) == 0) goto LAB_10011c9f;
              if ((local_214 & 0x20) == 0) {
                *piVar3 = local_230;
              }
              else {
                *(undefined2 *)piVar3 = (undefined2)local_230;
              }
              local_24c = 1;
            }
            if (local_250 != (wchar_t *)0x0) {
              FUN_100012d0(local_250);
              local_250 = (wchar_t *)0x0;
            }
          }
          local_215 = *local_244;
          iVar5 = local_270;
          param_2 = local_244;
          param_4 = local_228;
        }
        if (local_254 != '\0') {
          *(uint *)(local_258 + 0x70) = *(uint *)(local_258 + 0x70) & 0xfffffffd;
        }
        goto LAB_10011cc6;
      }
    }
  }
  pDVar4 = FUN_10011e63();
  *pDVar4 = 0x16;
LAB_100113da:
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_254 != '\0') {
    *(uint *)(local_258 + 0x70) = *(uint *)(local_258 + 0x70) & 0xfffffffd;
  }
LAB_10011cc6:
  __security_check_cookie(local_8 ^ (uint)local_1fc);
  return;
}


// 10011cfe FUN_10011cfe

void __cdecl FUN_10011cfe(undefined4 param_1)

{
  DAT_10039e2c = param_1;
  return;
}


// 10011d08 __invoke_watson

/* Library Function - Single Match
    __invoke_watson
   
   Library: Visual Studio 2005 Release */

void __cdecl
__invoke_watson(wchar_t *param_1,wchar_t *param_2,wchar_t *param_3,uint param_4,uintptr_t param_5)

{
  int iVar1;
  BOOL BVar2;
  LONG LVar3;
  HANDLE hProcess;
  UINT uExitCode;
  EXCEPTION_RECORD local_32c;
  _EXCEPTION_POINTERS local_2dc;
  undefined4 local_2d4;
  
  iVar1 = DAT_100366b8;
  local_2d4 = 0x10001;
  _memset(&local_32c,0,0x50);
  local_2dc.ExceptionRecord = &local_32c;
  local_2dc.ContextRecord = (PCONTEXT)&local_2d4;
  local_32c.ExceptionCode = 0xc000000d;
  BVar2 = IsDebuggerPresent();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  LVar3 = UnhandledExceptionFilter(&local_2dc);
  if ((LVar3 == 0) && (BVar2 == 0)) {
    FUN_10019895();
  }
  uExitCode = 0xc000000d;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  __security_check_cookie(iVar1);
  return;
}


// 10011e04 FUN_10011e04

void __cdecl
FUN_10011e04(wchar_t *param_1,wchar_t *param_2,wchar_t *param_3,uint param_4,uintptr_t param_5)

{
  code *UNRECOVERED_JUMPTABLE;
  
  UNRECOVERED_JUMPTABLE = (code *)FUN_10013035(DAT_10039e2c);
  if (UNRECOVERED_JUMPTABLE != (code *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x10011e18. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (*UNRECOVERED_JUMPTABLE)();
    return;
  }
  FUN_10019895();
                    /* WARNING: Subroutine does not return */
  __invoke_watson(param_1,param_2,param_3,param_4,param_5);
}


// 10011e28 FUN_10011e28

int __cdecl FUN_10011e28(int param_1)

{
  uint uVar1;
  
  uVar1 = 0;
  do {
    if (param_1 == (&DAT_10035d58)[uVar1 * 2]) {
      return *(int *)(uVar1 * 8 + 0x10035d5c);
    }
    uVar1 = uVar1 + 1;
  } while (uVar1 < 0x2d);
  if (param_1 - 0x13U < 0x12) {
    return 0xd;
  }
  return (-(uint)(0xe < param_1 - 0xbcU) & 0xe) + 8;
}


// 10011e63 FUN_10011e63

DWORD * FUN_10011e63(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_100131da();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10035ec0;
  }
  return pDVar1 + 2;
}


// 10011e76 FUN_10011e76

DWORD * FUN_10011e76(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_100131da();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10035ec4;
  }
  return pDVar1 + 3;
}


// 10011e89 FUN_10011e89

void __cdecl FUN_10011e89(DWORD param_1)

{
  DWORD *pDVar1;
  DWORD DVar2;
  
  pDVar1 = FUN_10011e76();
  *pDVar1 = param_1;
  DVar2 = FUN_10011e28(param_1);
  pDVar1 = FUN_10011e63();
  *pDVar1 = DVar2;
  return;
}


// 10011ea7 FUN_10011ea7

void __cdecl FUN_10011ea7(undefined4 *param_1,undefined4 *param_2,uint param_3)

{
  undefined4 uVar1;
  undefined4 uVar2;
  undefined4 uVar3;
  undefined4 uVar4;
  undefined4 uVar5;
  undefined4 uVar6;
  undefined4 uVar7;
  undefined4 uVar8;
  undefined4 uVar9;
  undefined4 uVar10;
  undefined4 uVar11;
  undefined4 uVar12;
  undefined4 uVar13;
  undefined4 uVar14;
  undefined4 uVar15;
  uint uVar16;
  
  uVar16 = param_3 >> 7;
  do {
    uVar1 = param_2[1];
    uVar2 = param_2[2];
    uVar3 = param_2[3];
    uVar4 = param_2[4];
    uVar5 = param_2[5];
    uVar6 = param_2[6];
    uVar7 = param_2[7];
    uVar8 = param_2[8];
    uVar9 = param_2[9];
    uVar10 = param_2[10];
    uVar11 = param_2[0xb];
    uVar12 = param_2[0xc];
    uVar13 = param_2[0xd];
    uVar14 = param_2[0xe];
    uVar15 = param_2[0xf];
    *param_1 = *param_2;
    param_1[1] = uVar1;
    param_1[2] = uVar2;
    param_1[3] = uVar3;
    param_1[4] = uVar4;
    param_1[5] = uVar5;
    param_1[6] = uVar6;
    param_1[7] = uVar7;
    param_1[8] = uVar8;
    param_1[9] = uVar9;
    param_1[10] = uVar10;
    param_1[0xb] = uVar11;
    param_1[0xc] = uVar12;
    param_1[0xd] = uVar13;
    param_1[0xe] = uVar14;
    param_1[0xf] = uVar15;
    uVar1 = param_2[0x11];
    uVar2 = param_2[0x12];
    uVar3 = param_2[0x13];
    uVar4 = param_2[0x14];
    uVar5 = param_2[0x15];
    uVar6 = param_2[0x16];
    uVar7 = param_2[0x17];
    uVar8 = param_2[0x18];
    uVar9 = param_2[0x19];
    uVar10 = param_2[0x1a];
    uVar11 = param_2[0x1b];
    uVar12 = param_2[0x1c];
    uVar13 = param_2[0x1d];
    uVar14 = param_2[0x1e];
    uVar15 = param_2[0x1f];
    param_1[0x10] = param_2[0x10];
    param_1[0x11] = uVar1;
    param_1[0x12] = uVar2;
    param_1[0x13] = uVar3;
    param_1[0x14] = uVar4;
    param_1[0x15] = uVar5;
    param_1[0x16] = uVar6;
    param_1[0x17] = uVar7;
    param_1[0x18] = uVar8;
    param_1[0x19] = uVar9;
    param_1[0x1a] = uVar10;
    param_1[0x1b] = uVar11;
    param_1[0x1c] = uVar12;
    param_1[0x1d] = uVar13;
    param_1[0x1e] = uVar14;
    param_1[0x1f] = uVar15;
    param_2 = param_2 + 0x20;
    param_1 = param_1 + 0x20;
    uVar16 = uVar16 - 1;
  } while (uVar16 != 0);
  return;
}


// 10011f2e __VEC_memcpy

/* Library Function - Single Match
    __VEC_memcpy
   
   Libraries: Visual Studio 2005 Debug, Visual Studio 2005 Release, Visual Studio 2008 Debug, Visual
   Studio 2008 Release */

undefined4 * __cdecl __VEC_memcpy(undefined4 *param_1,undefined4 *param_2,uint param_3)

{
  int iVar1;
  int iVar2;
  uint uVar3;
  undefined1 *puVar4;
  undefined4 *puVar5;
  uint uVar6;
  undefined1 *puVar7;
  undefined4 *puVar8;
  
  uVar3 = (int)param_2 >> 0x1f;
  iVar1 = (((uint)param_2 ^ uVar3) - uVar3 & 0xf ^ uVar3) - uVar3;
  uVar3 = (int)param_1 >> 0x1f;
  uVar6 = ((uint)param_1 ^ uVar3) - uVar3 & 0xf ^ uVar3;
  if (iVar1 == 0 && uVar6 == uVar3) {
    uVar3 = param_3 & 0x7f;
    if (param_3 != uVar3) {
      FUN_10011ea7(param_1,param_2,param_3 - uVar3);
    }
    if (uVar3 != 0) {
      puVar4 = (undefined1 *)((int)param_2 + (param_3 - uVar3));
      puVar7 = (undefined1 *)((int)param_1 + (param_3 - uVar3));
      for (; uVar3 != 0; uVar3 = uVar3 - 1) {
        *puVar7 = *puVar4;
        puVar4 = puVar4 + 1;
        puVar7 = puVar7 + 1;
      }
    }
  }
  else if (iVar1 == uVar6 - uVar3) {
    iVar1 = 0x10 - iVar1;
    puVar5 = param_2;
    puVar8 = param_1;
    for (iVar2 = iVar1; iVar2 != 0; iVar2 = iVar2 + -1) {
      *(undefined1 *)puVar8 = *(undefined1 *)puVar5;
      puVar5 = (undefined4 *)((int)puVar5 + 1);
      puVar8 = (undefined4 *)((int)puVar8 + 1);
    }
    __VEC_memcpy((undefined4 *)((int)param_1 + iVar1),(undefined4 *)((int)param_2 + iVar1),
                 param_3 - iVar1);
  }
  else {
    puVar5 = param_1;
    for (uVar3 = param_3 >> 2; uVar3 != 0; uVar3 = uVar3 - 1) {
      *puVar5 = *param_2;
      param_2 = param_2 + 1;
      puVar5 = puVar5 + 1;
    }
    for (uVar3 = param_3 & 3; uVar3 != 0; uVar3 = uVar3 - 1) {
      *(undefined1 *)puVar5 = *(undefined1 *)param_2;
      param_2 = (undefined4 *)((int)param_2 + 1);
      puVar5 = (undefined4 *)((int)puVar5 + 1);
    }
  }
  return param_1;
}


// 10012011 __forcdecpt_l

/* Library Function - Single Match
    __forcdecpt_l
   
   Library: Visual Studio 2005 Release */

void __cdecl __forcdecpt_l(char *_Buf,_locale_t _Locale)

{
  byte bVar1;
  byte bVar2;
  int iVar3;
  bool bVar4;
  int local_14 [2];
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
  iVar3 = _tolower((int)*_Buf);
  bVar4 = iVar3 == 0x65;
  while (!bVar4) {
    _Buf = (char *)((byte *)_Buf + 1);
    iVar3 = _isdigit((uint)(byte)*_Buf);
    bVar4 = iVar3 == 0;
  }
  iVar3 = _tolower((int)*_Buf);
  if (iVar3 == 0x78) {
    _Buf = (char *)((byte *)_Buf + 2);
  }
  bVar2 = *_Buf;
  *_Buf = *(byte *)**(undefined4 **)(local_14[0] + 0xbc);
  do {
    _Buf = (char *)((byte *)_Buf + 1);
    bVar1 = *_Buf;
    *_Buf = bVar2;
    bVar2 = bVar1;
  } while (*_Buf != 0);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return;
}


// 10012082 __cropzeros_l

/* Library Function - Single Match
    __cropzeros_l
   
   Library: Visual Studio 2005 Release */

void __cdecl __cropzeros_l(char *_Buf,_locale_t _Locale)

{
  char *pcVar1;
  char cVar3;
  int local_14 [2];
  int local_c;
  char local_8;
  char *pcVar2;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
  cVar3 = *_Buf;
  if (cVar3 != '\0') {
    do {
      if (cVar3 == *(char *)**(undefined4 **)(local_14[0] + 0xbc)) break;
      _Buf = _Buf + 1;
      cVar3 = *_Buf;
    } while (cVar3 != '\0');
  }
  if (*_Buf != '\0') {
    do {
      _Buf = _Buf + 1;
      cVar3 = *_Buf;
      pcVar1 = _Buf;
      if ((cVar3 == '\0') || (cVar3 == 'e')) break;
    } while (cVar3 != 'E');
    do {
      pcVar2 = pcVar1;
      pcVar1 = pcVar2 + -1;
    } while (*pcVar1 == '0');
    if (*pcVar1 == *(char *)**(undefined4 **)(local_14[0] + 0xbc)) {
      pcVar1 = pcVar2 + -2;
    }
    do {
      cVar3 = *_Buf;
      pcVar1 = pcVar1 + 1;
      _Buf = _Buf + 1;
      *pcVar1 = cVar3;
    } while (cVar3 != '\0');
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return;
}


// 10012118 __fassign_l

/* Library Function - Single Match
    __fassign_l
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release */

void __cdecl __fassign_l(int flag,char *argument,char *number,_locale_t param_4)

{
  _CRT_FLOAT local_c;
  undefined4 local_8;
  
  if (flag == 0) {
    FID_conflict___atoflt_l((_CRT_FLOAT *)&flag,number,param_4);
    *(int *)argument = flag;
  }
  else {
    FID_conflict___atoflt_l(&local_c,number,param_4);
    *(float *)argument = local_c.f;
    *(undefined4 *)(argument + 4) = local_8;
  }
  return;
}


// 1001216f __shift

/* Library Function - Single Match
    __shift
   
   Library: Visual Studio 2005 Release */

void __shift(void)

{
  char *in_EAX;
  size_t sVar1;
  int unaff_EDI;
  
  if (unaff_EDI != 0) {
    sVar1 = _strlen(in_EAX);
    _memmove(in_EAX + unaff_EDI,in_EAX,sVar1 + 1);
  }
  return;
}


// 100121a8 __cftoe2_l

/* Library Function - Single Match
    __cftoe2_l
   
   Library: Visual Studio 2005 Release */

DWORD __cdecl
__cftoe2_l(uint param_1,int param_2,int param_3,int *param_4,char param_5,localeinfo_struct *param_6
          )

{
  undefined1 *in_EAX;
  DWORD *pDVar1;
  errno_t eVar2;
  int iVar3;
  int iVar4;
  undefined1 *puVar5;
  undefined1 *puVar6;
  char *_Dst;
  DWORD DVar7;
  DWORD DVar8;
  char *pcVar9;
  int local_14 [2];
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,param_6);
  if ((in_EAX == (undefined1 *)0x0) || (param_1 == 0)) {
    pDVar1 = FUN_10011e63();
    DVar7 = 0x16;
  }
  else {
    iVar3 = param_2;
    if (param_2 < 1) {
      iVar3 = 0;
    }
    if (iVar3 + 9U < param_1) {
      if (param_5 != '\0') {
        __shift();
      }
      puVar5 = in_EAX;
      if (*param_4 == 0x2d) {
        *in_EAX = 0x2d;
        puVar5 = in_EAX + 1;
      }
      puVar6 = puVar5;
      if (0 < param_2) {
        puVar6 = puVar5 + 1;
        *puVar5 = *puVar6;
        *puVar6 = *(undefined1 *)**(undefined4 **)(local_14[0] + 0xbc);
      }
      _Dst = puVar6 + (uint)(param_5 == '\0') + param_2;
      if (param_1 == 0xffffffff) {
        puVar5 = (undefined1 *)0xffffffff;
      }
      else {
        puVar5 = in_EAX + (param_1 - (int)_Dst);
      }
      pcVar9 = "e+000";
      eVar2 = _strcpy_s(_Dst,(rsize_t)puVar5,"e+000");
      if (eVar2 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      if (param_3 != 0) {
        *_Dst = 'E';
      }
      if (*(char *)param_4[3] != '0') {
        iVar3 = param_4[1] + -1;
        if (iVar3 < 0) {
          iVar3 = -iVar3;
          _Dst[1] = '-';
        }
        if (99 < iVar3) {
          pcVar9 = (char *)0x64;
          iVar4 = iVar3 / 100;
          iVar3 = iVar3 % 100;
          _Dst[2] = _Dst[2] + (char)iVar4;
        }
        if (9 < iVar3) {
          pcVar9 = (char *)0xa;
          iVar4 = iVar3 / 10;
          iVar3 = iVar3 % 10;
          _Dst[3] = _Dst[3] + (char)iVar4;
        }
        _Dst[4] = _Dst[4] + (char)iVar3;
      }
      if (((DAT_1003a460 & 1) != 0) && (_Dst[2] == '0')) {
        pcVar9 = (char *)0x3;
        _memmove(_Dst + 2,_Dst + 3,3);
      }
      if (local_8 == '\0') {
        return (DWORD)pcVar9;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return (DWORD)pcVar9;
    }
    pDVar1 = FUN_10011e63();
    DVar7 = 0x22;
  }
  DVar8 = 0;
  *pDVar1 = DVar7;
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return DVar8;
}


// 10012315 __cftoe_l

/* Library Function - Single Match
    __cftoe_l
   
   Library: Visual Studio 2005 Release */

void __cdecl
__cftoe_l(double *param_1,undefined1 *param_2,uint param_3,int param_4,int param_5,
         localeinfo_struct *param_6)

{
  DWORD *pDVar1;
  size_t _SizeInBytes;
  errno_t eVar2;
  _strflt local_30;
  char local_20 [24];
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    if (param_3 == 0xffffffff) {
      _SizeInBytes = 0xffffffff;
    }
    else {
      _SizeInBytes = (param_3 - (local_30.sign == 0x2d)) - (uint)(0 < param_4);
    }
    eVar2 = __fptostr(param_2 + (uint)(0 < param_4) + (uint)(local_30.sign == 0x2d),_SizeInBytes,
                      param_4 + 1,&local_30);
    if (eVar2 == 0) {
      __cftoe2_l(param_3,param_4,param_5,&local_30.sign,'\0',param_6);
    }
    else {
      *param_2 = 0;
    }
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 100123e3 __cftoe

/* Library Function - Single Match
    __cftoe
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __cftoe(double *_Value,char *_Buf,size_t _SizeInBytes,int _Dec,int _Caps)

{
  errno_t eVar1;
  
  eVar1 = __cftoe_l(_Value,_Buf,_SizeInBytes,_Dec,_Caps,(localeinfo_struct *)0x0);
  return eVar1;
}


// 10012401 __cftoa_l

/* Library Function - Single Match
    __cftoa_l
   
   Library: Visual Studio 2005 Release */

DWORD __cdecl
__cftoa_l(double *param_1,undefined1 *param_2,uint param_3,size_t param_4,int param_5,
         localeinfo_struct *param_6)

{
  ushort uVar1;
  DWORD *pDVar2;
  size_t _SizeInBytes;
  errno_t eVar3;
  char *pcVar4;
  char *pcVar5;
  uint uVar6;
  uint uVar7;
  uint extraout_ECX;
  uint extraout_ECX_00;
  uint extraout_ECX_01;
  uint uVar8;
  short sVar9;
  DWORD unaff_EBX;
  char *pcVar10;
  char *pcVar11;
  bool bVar12;
  ulonglong uVar13;
  undefined8 uVar14;
  DWORD DVar15;
  DWORD DVar16;
  int local_28 [2];
  int local_20;
  char local_1c;
  uint local_18;
  undefined4 local_14;
  uint local_10;
  uint local_c;
  int local_8;
  
  local_18 = 0x3ff;
  local_8 = 0x30;
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_28,param_6);
  if ((int)param_4 < 0) {
    param_4 = 0;
  }
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar2 = FUN_10011e63();
    DVar15 = 0x16;
LAB_1001243a:
    DVar16 = 0;
    *pDVar2 = DVar15;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_1c != '\0') {
      *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
    }
    return DVar16;
  }
  *param_2 = 0;
  if (param_3 <= param_4 + 0xb) {
    pDVar2 = FUN_10011e63();
    DVar15 = 0x22;
    goto LAB_1001243a;
  }
  local_10 = *(uint *)param_1;
  if ((*(uint *)((int)param_1 + 4) >> 0x14 & 0x7ff) == 0x7ff) {
    if (param_3 == 0xffffffff) {
      _SizeInBytes = 0xffffffff;
    }
    else {
      _SizeInBytes = param_3 - 2;
    }
    eVar3 = __cftoe(param_1,param_2 + 2,_SizeInBytes,param_4,0);
    if (eVar3 != 0) {
      *param_2 = 0;
      if (local_1c == '\0') {
        return unaff_EBX;
      }
      *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      return unaff_EBX;
    }
    if (param_2[2] == '-') {
      *param_2 = 0x2d;
      param_2 = param_2 + 1;
    }
    *param_2 = 0x30;
    param_2[1] = ((param_5 == 0) - 1U & 0xe0) + 0x78;
    pcVar4 = _strrchr(param_2 + 2,0x65);
    if (pcVar4 != (char *)0x0) {
      *pcVar4 = ((param_5 == 0) - 1U & 0xe0) + 0x70;
      pcVar4[3] = '\0';
    }
    goto LAB_1001275e;
  }
  if ((*(uint *)((int)param_1 + 4) & 0x80000000) != 0) {
    *param_2 = 0x2d;
    param_2 = param_2 + 1;
  }
  *param_2 = 0x30;
  param_2[1] = ((param_5 == 0) - 1U & 0xe0) + 0x78;
  sVar9 = (-(ushort)(param_5 != 0) & 0xffe0) + 0x27;
  if (((ulonglong)*param_1 & 0x7ff0000000000000) == 0) {
    param_2[2] = 0x30;
    if (*(int *)param_1 == 0 && ((ulonglong)*param_1 & 0xfffff00000000) == 0) {
      local_18 = 0;
    }
    else {
      local_18 = 0x3fe;
    }
  }
  else {
    param_2[2] = 0x31;
  }
  pcVar11 = param_2 + 3;
  pcVar4 = param_2 + 4;
  if (param_4 == 0) {
    *pcVar11 = '\0';
  }
  else {
    *pcVar11 = *(char *)**(undefined4 **)(local_28[0] + 0xbc);
  }
  if ((((ulonglong)*param_1 & 0xfffff00000000) != 0) || (local_c = 0, *(int *)param_1 != 0)) {
    local_10 = 0;
    local_c = 0xf0000;
    do {
      if ((int)param_4 < 1) break;
      uVar13 = __aullshr((byte)local_8,*(uint *)((int)param_1 + 4) & local_c & 0xfffff);
      uVar1 = (short)uVar13 + 0x30;
      if (0x39 < uVar1) {
        uVar1 = uVar1 + sVar9;
      }
      local_8 = local_8 + -4;
      *pcVar4 = (char)uVar1;
      local_10 = local_10 >> 4 | local_c << 0x1c;
      local_c = local_c >> 4;
      pcVar4 = pcVar4 + 1;
      param_4 = param_4 - 1;
    } while (-1 < (short)local_8);
    if ((-1 < (short)local_8) &&
       (uVar13 = __aullshr((byte)local_8,*(uint *)((int)param_1 + 4) & local_c & 0xfffff),
       pcVar10 = pcVar4, 8 < (ushort)uVar13)) {
      while( true ) {
        pcVar5 = pcVar10 + -1;
        if ((*pcVar5 != 'f') && (*pcVar5 != 'F')) break;
        *pcVar5 = '0';
        pcVar10 = pcVar5;
      }
      if (pcVar5 == pcVar11) {
        pcVar10[-2] = pcVar10[-2] + '\x01';
      }
      else if (*pcVar5 == '9') {
        *pcVar5 = (char)sVar9 + ':';
      }
      else {
        *pcVar5 = *pcVar5 + '\x01';
      }
    }
  }
  if (0 < (int)param_4) {
    _memset(pcVar4,0x30,param_4);
    pcVar4 = pcVar4 + param_4;
  }
  if (*pcVar11 == '\0') {
    pcVar4 = pcVar11;
  }
  *pcVar4 = ((param_5 == 0) - 1U & 0xe0) + 0x70;
  uVar13 = __aullshr(0x34,*(uint *)((int)param_1 + 4));
  uVar6 = (uint)(uVar13 & 0x7ff);
  uVar7 = uVar6 - local_18;
  uVar6 = (uint)(uVar6 < local_18);
  uVar8 = -uVar6;
  if (uVar6 == 0) {
    pcVar4[1] = '+';
  }
  else {
    pcVar4[1] = '-';
    bVar12 = uVar7 != 0;
    uVar7 = -uVar7;
    uVar8 = -(uVar8 + bVar12);
  }
  pcVar10 = pcVar4 + 2;
  *pcVar10 = '0';
  pcVar11 = pcVar10;
  if (((int)uVar8 < 0) || (((int)uVar8 < 1 && (uVar7 < 1000)))) {
LAB_1001270d:
    if ((-1 < (int)uVar8) && ((0 < (int)uVar8 || (99 < uVar7)))) goto LAB_10012718;
  }
  else {
    uVar14 = __alldvrm(uVar7,uVar8,1000,0);
    local_14 = (undefined4)((ulonglong)uVar14 >> 0x20);
    *pcVar10 = (char)uVar14 + '0';
    pcVar11 = pcVar4 + 3;
    uVar8 = 0;
    uVar7 = extraout_ECX;
    if (pcVar11 == pcVar10) goto LAB_1001270d;
LAB_10012718:
    uVar14 = __alldvrm(uVar7,uVar8,100,0);
    local_14 = (undefined4)((ulonglong)uVar14 >> 0x20);
    *pcVar11 = (char)uVar14 + '0';
    pcVar11 = pcVar11 + 1;
    uVar8 = 0;
    uVar7 = extraout_ECX_00;
  }
  if ((pcVar11 != pcVar10) || ((-1 < (int)uVar8 && ((0 < (int)uVar8 || (9 < uVar7)))))) {
    uVar14 = __alldvrm(uVar7,uVar8,10,0);
    *pcVar11 = (char)uVar14 + '0';
    pcVar11 = pcVar11 + 1;
    uVar7 = extraout_ECX_01;
  }
  *pcVar11 = (char)uVar7 + '0';
  pcVar11[1] = '\0';
LAB_1001275e:
  if (local_1c == '\0') {
    return unaff_EBX;
  }
  *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  return unaff_EBX;
}


// 10012772 __cftof2_l

/* Library Function - Single Match
    __cftof2_l
   
   Library: Visual Studio 2005 Release */

undefined4 __thiscall
__cftof2_l(void *this,int param_1,size_t param_2,char param_3,localeinfo_struct *param_4)

{
  int iVar1;
  int *in_EAX;
  DWORD *pDVar2;
  undefined1 *puVar3;
  undefined4 uVar4;
  int local_14 [2];
  int local_c;
  char local_8;
  
  iVar1 = in_EAX[1];
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,param_4);
  if ((this == (void *)0x0) || (param_1 == 0)) {
    pDVar2 = FUN_10011e63();
    uVar4 = 0x16;
    *pDVar2 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  else {
    if ((param_3 != '\0') && (iVar1 - 1U == param_2)) {
      puVar3 = (undefined1 *)((uint)(*in_EAX == 0x2d) + (iVar1 - 1U) + (int)this);
      *puVar3 = 0x30;
      puVar3[1] = 0;
    }
    if (*in_EAX == 0x2d) {
      *(undefined1 *)this = 0x2d;
      this = (void *)((int)this + 1);
    }
    if (in_EAX[1] < 1) {
      __shift();
      *(undefined1 *)this = 0x30;
      puVar3 = (undefined1 *)((int)this + 1);
    }
    else {
      puVar3 = (undefined1 *)((int)this + in_EAX[1]);
    }
    if (0 < (int)param_2) {
      __shift();
      *puVar3 = *(undefined1 *)**(undefined4 **)(local_14[0] + 0xbc);
      iVar1 = in_EAX[1];
      if (iVar1 < 0) {
        if ((param_3 != '\0') || (SBORROW4(param_2,-iVar1) == (int)(param_2 + iVar1) < 0)) {
          param_2 = -iVar1;
        }
        __shift();
        _memset(puVar3 + 1,0x30,param_2);
      }
    }
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    uVar4 = 0;
  }
  return uVar4;
}


// 10012867 __cftof_l

/* Library Function - Single Match
    __cftof_l
   
   Library: Visual Studio 2005 Release */

void __cdecl
__cftof_l(double *param_1,undefined1 *param_2,int param_3,size_t param_4,localeinfo_struct *param_5)

{
  DWORD *pDVar1;
  size_t _SizeInBytes;
  errno_t eVar2;
  _strflt local_30;
  char local_20 [24];
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    if (param_3 == -1) {
      _SizeInBytes = 0xffffffff;
    }
    else {
      _SizeInBytes = param_3 - (uint)(local_30.sign == 0x2d);
    }
    eVar2 = __fptostr(param_2 + (local_30.sign == 0x2d),_SizeInBytes,local_30.decpt + param_4,
                      &local_30);
    if (eVar2 == 0) {
      __cftof2_l(param_2,param_3,param_4,'\0',param_5);
    }
    else {
      *param_2 = 0;
    }
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10012920 __cftog_l

/* Library Function - Single Match
    __cftog_l
   
   Library: Visual Studio 2005 Release */

void __cdecl
__cftog_l(double *param_1,undefined1 *param_2,uint param_3,size_t param_4,int param_5,
         localeinfo_struct *param_6)

{
  char *pcVar1;
  DWORD *pDVar2;
  errno_t eVar3;
  size_t _SizeInBytes;
  char *pcVar4;
  _strflt local_34;
  int local_24;
  char local_20 [24];
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_34,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar2 = FUN_10011e63();
    *pDVar2 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    local_24 = local_34.decpt + -1;
    if (param_3 == 0xffffffff) {
      _SizeInBytes = 0xffffffff;
    }
    else {
      _SizeInBytes = param_3 - (local_34.sign == 0x2d);
    }
    eVar3 = __fptostr(param_2 + (local_34.sign == 0x2d),_SizeInBytes,param_4,&local_34);
    if (eVar3 == 0) {
      local_34.decpt = local_34.decpt + -1;
      if ((local_34.decpt < -4) || ((int)param_4 <= local_34.decpt)) {
        __cftoe2_l(param_3,param_4,param_5,&local_34.sign,'\x01',param_6);
      }
      else {
        pcVar1 = param_2 + (local_34.sign == 0x2d);
        if (local_24 < local_34.decpt) {
          do {
            pcVar4 = pcVar1;
            pcVar1 = pcVar4 + 1;
          } while (*pcVar4 != '\0');
          pcVar4[-1] = '\0';
        }
        __cftof2_l(param_2,param_3,param_4,'\x01',param_6);
      }
    }
    else {
      *param_2 = 0;
    }
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10012a18 __cfltcvt_l

/* Library Function - Single Match
    __cfltcvt_l
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl
__cfltcvt_l(double *arg,char *buffer,size_t sizeInBytes,int format,int precision,int caps,
           _locale_t plocinfo)

{
  errno_t eVar1;
  errno_t extraout_EAX;
  
  if ((format == 0x65) || (format == 0x45)) {
    eVar1 = __cftoe_l(arg,buffer,sizeInBytes,precision,caps,plocinfo);
  }
  else {
    if (format == 0x66) {
      eVar1 = __cftof_l(arg,buffer,sizeInBytes,precision,plocinfo);
      return eVar1;
    }
    if ((format == 0x61) || (format == 0x41)) {
      __cftoa_l(arg,buffer,sizeInBytes,precision,caps,plocinfo);
      eVar1 = extraout_EAX;
    }
    else {
      eVar1 = __cftog_l(arg,buffer,sizeInBytes,precision,caps,plocinfo);
    }
  }
  return eVar1;
}


// 10012a9e __cfltcvt

/* Library Function - Single Match
    __cfltcvt
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release,
   Visual Studio 2012 Release */

errno_t __cdecl
__cfltcvt(double *arg,char *buffer,size_t sizeInBytes,int format,int precision,int caps)

{
  errno_t eVar1;
  
  eVar1 = __cfltcvt_l(arg,buffer,sizeInBytes,format,precision,caps,(_locale_t)0x0);
  return eVar1;
}


// 10012abf __initp_misc_cfltcvt_tab

/* Library Function - Single Match
    __initp_misc_cfltcvt_tab
   
   Library: Visual Studio 2005 Release */

void __initp_misc_cfltcvt_tab(void)

{
  int *piVar1;
  int iVar2;
  uint uVar3;
  
  uVar3 = 0;
  do {
    piVar1 = (int *)((int)&PTR_LAB_10035ec8 + uVar3);
    iVar2 = FUN_10012fbe(*piVar1);
    uVar3 = uVar3 + 4;
    *piVar1 = iVar2;
  } while (uVar3 < 0x28);
  return;
}


// 10012ade __setdefaultprecision

/* Library Function - Single Match
    __setdefaultprecision
   
   Library: Visual Studio 2005 Release */

void __setdefaultprecision(void)

{
  errno_t eVar1;
  
  eVar1 = __controlfp_s((uint *)0x0,0x10000,0x30000);
  if (eVar1 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return;
}


// 10012b07 __ms_p5_test_fdiv

/* WARNING: Removing unreachable block (ram,0x10012b3a) */
/* Library Function - Single Match
    __ms_p5_test_fdiv
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

undefined4 __ms_p5_test_fdiv(void)

{
  return 0;
}


// 10012b43 __ms_p5_mp_test_fdiv

/* Library Function - Single Match
    __ms_p5_mp_test_fdiv
   
   Library: Visual Studio 2005 Release */

void __ms_p5_mp_test_fdiv(void)

{
  HMODULE hModule;
  FARPROC pFVar1;
  
  hModule = GetModuleHandleA("KERNEL32");
  if (hModule != (HMODULE)0x0) {
    pFVar1 = GetProcAddress(hModule,"IsProcessorFeaturePresent");
    if (pFVar1 != (FARPROC)0x0) {
      (*pFVar1)(0);
      return;
    }
  }
  __ms_p5_test_fdiv();
  return;
}


// 10012b6c type_info::_Type_info_dtor

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    private: static void __cdecl type_info::_Type_info_dtor(class type_info *)
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release */

void __cdecl type_info::_Type_info_dtor(type_info *param_1)

{
  int *piVar1;
  int *piVar2;
  int *piVar3;
  
  __lock(0xe);
  piVar1 = DAT_10039e34;
  if (*(int *)(param_1 + 4) != 0) {
    piVar2 = (int *)&DAT_10039e30;
    do {
      piVar3 = piVar2;
      if (DAT_10039e34 == (int *)0x0) goto LAB_10012bb0;
      piVar2 = DAT_10039e34;
    } while (*DAT_10039e34 != *(int *)(param_1 + 4));
    piVar3[1] = DAT_10039e34[1];
    FUN_100012d0(piVar1);
LAB_10012bb0:
    FUN_100012d0(*(undefined4 *)(param_1 + 4));
    *(undefined4 *)(param_1 + 4) = 0;
  }
  FUN_10012bd3();
  return;
}


// 10012bd3 FUN_10012bd3

void FUN_10012bd3(void)

{
  FUN_100189ed(0xe);
  return;
}


// 10012be0 _strcmp

/* Library Function - Single Match
    _strcmp
   
   Library: Visual Studio */

int __cdecl _strcmp(char *_Str1,char *_Str2)

{
  undefined2 uVar1;
  undefined4 uVar2;
  byte bVar3;
  byte bVar4;
  bool bVar5;
  
  if (((uint)_Str1 & 3) != 0) {
    if (((uint)_Str1 & 1) != 0) {
      bVar4 = *_Str1;
      _Str1 = _Str1 + 1;
      bVar5 = bVar4 < (byte)*_Str2;
      if (bVar4 != *_Str2) goto LAB_10012c24;
      _Str2 = _Str2 + 1;
      if (bVar4 == 0) {
        return 0;
      }
      if (((uint)_Str1 & 2) == 0) goto LAB_10012bf0;
    }
    uVar1 = *(undefined2 *)_Str1;
    _Str1 = _Str1 + 2;
    bVar4 = (byte)uVar1;
    bVar5 = bVar4 < (byte)*_Str2;
    if (bVar4 != *_Str2) goto LAB_10012c24;
    if (bVar4 == 0) {
      return 0;
    }
    bVar4 = (byte)((ushort)uVar1 >> 8);
    bVar5 = bVar4 < ((byte *)_Str2)[1];
    if (bVar4 != ((byte *)_Str2)[1]) goto LAB_10012c24;
    if (bVar4 == 0) {
      return 0;
    }
    _Str2 = (char *)((byte *)_Str2 + 2);
  }
LAB_10012bf0:
  while( true ) {
    uVar2 = *(undefined4 *)_Str1;
    bVar4 = (byte)uVar2;
    bVar5 = bVar4 < (byte)*_Str2;
    if (bVar4 != *_Str2) break;
    if (bVar4 == 0) {
      return 0;
    }
    bVar4 = (byte)((uint)uVar2 >> 8);
    bVar5 = bVar4 < ((byte *)_Str2)[1];
    if (bVar4 != ((byte *)_Str2)[1]) break;
    if (bVar4 == 0) {
      return 0;
    }
    bVar4 = (byte)((uint)uVar2 >> 0x10);
    bVar5 = bVar4 < ((byte *)_Str2)[2];
    if (bVar4 != ((byte *)_Str2)[2]) break;
    bVar3 = (byte)((uint)uVar2 >> 0x18);
    if (bVar4 == 0) {
      return 0;
    }
    bVar5 = bVar3 < ((byte *)_Str2)[3];
    if (bVar3 != ((byte *)_Str2)[3]) break;
    _Str2 = (char *)((byte *)_Str2 + 4);
    _Str1 = _Str1 + 4;
    if (bVar3 == 0) {
      return 0;
    }
  }
LAB_10012c24:
  return (uint)bVar5 * -2 + 1;
}


// 10012c68 _abort

/* Library Function - Single Match
    _abort
   
   Library: Visual Studio 2005 Release */

void __cdecl _abort(void)

{
  _PHNDLR p_Var1;
  EXCEPTION_RECORD local_32c;
  _EXCEPTION_POINTERS local_2dc;
  undefined4 local_2d4;
  
  if (((byte)DAT_10035f00 & 1) != 0) {
    FUN_10012d79(10);
  }
  p_Var1 = ___get_sigabrt();
  if (p_Var1 != (_PHNDLR)0x0) {
    _raise(0x16);
  }
  if (((byte)DAT_10035f00 & 2) != 0) {
    local_2d4 = 0x10001;
    _memset(&local_32c,0,0x50);
    local_2dc.ExceptionRecord = &local_32c;
    local_2dc.ContextRecord = (PCONTEXT)&local_2d4;
    local_32c.ExceptionCode = 0x40000015;
    SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
    UnhandledExceptionFilter(&local_2dc);
  }
                    /* WARNING: Subroutine does not return */
  __exit(3);
}


// 10012d5b FUN_10012d5b

void __cdecl FUN_10012d5b(uint param_1,uint param_2)

{
  DAT_10035f00 = ~param_2 & DAT_10035f00 | param_1 & param_2;
  return;
}


// 10012d79 FUN_10012d79

void __cdecl FUN_10012d79(int param_1)

{
  undefined4 *puVar1;
  int iVar2;
  errno_t eVar3;
  DWORD DVar4;
  size_t sVar5;
  char *_Dst;
  HANDLE hFile;
  uint uVar6;
  DWORD *lpNumberOfBytesWritten;
  LPOVERLAPPED lpOverlapped;
  DWORD DStack_4;
  
  uVar6 = 0;
  do {
    if (param_1 == (&DAT_10035f08)[uVar6 * 2]) break;
    uVar6 = uVar6 + 1;
  } while (uVar6 < 0x17);
  if (uVar6 < 0x17) {
    iVar2 = __set_error_mode(3);
    if ((iVar2 == 1) || ((iVar2 = __set_error_mode(3), iVar2 == 0 && (DAT_10039e28 == 1)))) {
      hFile = GetStdHandle(0xfffffff4);
      if ((hFile != (HANDLE)0x0) && (hFile != (HANDLE)0xffffffff)) {
        lpOverlapped = (LPOVERLAPPED)0x0;
        lpNumberOfBytesWritten = &DStack_4;
        puVar1 = (undefined4 *)(uVar6 * 8 + 0x10035f0c);
        sVar5 = _strlen((char *)*puVar1);
        WriteFile(hFile,(LPCVOID)*puVar1,sVar5,lpNumberOfBytesWritten,lpOverlapped);
      }
    }
    else if (param_1 != 0xfc) {
      eVar3 = _strcpy_s(&DAT_10039e38,0x314,"Runtime Error!\n\nProgram: ");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      DAT_10039f55 = 0;
      DVar4 = GetModuleFileNameA((HMODULE)0x0,&DAT_10039e51,0x104);
      if ((DVar4 == 0) &&
         (eVar3 = _strcpy_s(&DAT_10039e51,0x2fb,"<program name unknown>"), eVar3 != 0)) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      sVar5 = _strlen(&DAT_10039e51);
      if (0x3c < sVar5 + 1) {
        sVar5 = _strlen(&DAT_10039e51);
        _Dst = (char *)((int)&DAT_10039e14 + sVar5 + 2);
        eVar3 = _strncpy_s(_Dst,(int)&DAT_1003a14c - (int)_Dst,"...",3);
        if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
      }
      eVar3 = _strcat_s(&DAT_10039e38,0x314,"\n\n");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      eVar3 = _strcat_s(&DAT_10039e38,0x314,*(char **)(uVar6 * 8 + 0x10035f0c));
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      FUN_10019fdc(&DAT_10039e38,"Microsoft Visual C++ Runtime Library",0x12010);
    }
  }
  return;
}


// 10012f19 __FF_MSGBANNER

/* Library Function - Single Match
    __FF_MSGBANNER
   
   Library: Visual Studio 2005 Release */

void __cdecl __FF_MSGBANNER(void)

{
  int iVar1;
  
  iVar1 = __set_error_mode(3);
  if (iVar1 != 1) {
    iVar1 = __set_error_mode(3);
    if (iVar1 != 0) {
      return;
    }
    if (DAT_10039e28 != 1) {
      return;
    }
  }
  FUN_10012d79(0xfc);
  FUN_10012d79(0xff);
  return;
}


// 10012f52 FUN_10012f52

undefined4 FUN_10012f52(void)

{
  int iVar1;
  HMODULE pHVar2;
  int iVar3;
  uint uVar4;
  char *_Str2;
  int local_8;
  
  uVar4 = 0;
  local_8 = 0;
  FUN_10013661(&local_8);
  if (local_8 < 6) {
    pHVar2 = GetModuleHandleA((LPCSTR)0x0);
    iVar1 = pHVar2[0xf].unused;
    _Str2 = (char *)((int)&pHVar2[6].unused +
                    iVar1 + (uint)*(ushort *)((int)&pHVar2[5].unused + iVar1));
    if (*(short *)((int)&pHVar2[1].unused + iVar1 + 2) != 0) {
      do {
        iVar3 = _strcmp(".mixcrt",_Str2);
        if (iVar3 == 0) {
          return 0;
        }
        uVar4 = uVar4 + 1;
        _Str2 = _Str2 + 0x28;
      } while (uVar4 < *(ushort *)((int)&pHVar2[1].unused + iVar1 + 2));
    }
  }
  return 1;
}


// 10012fbe FUN_10012fbe

int __cdecl FUN_10012fbe(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_10035fc4);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10035fc0 != -1)) {
    iVar3 = DAT_10035fc0;
    pcVar2 = (code *)TlsGetValue(DAT_10035fc4);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1f8);
      goto LAB_10013018;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_10012f52();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"EncodePointer");
LAB_10013018:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 1001302c __encoded_null

/* Library Function - Single Match
    __encoded_null
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __encoded_null(void)

{
  FUN_10012fbe(0);
  return;
}


// 10013035 FUN_10013035

int __cdecl FUN_10013035(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_10035fc4);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10035fc0 != -1)) {
    iVar3 = DAT_10035fc0;
    pcVar2 = (code *)TlsGetValue(DAT_10035fc4);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1fc);
      goto LAB_1001308f;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_10012f52();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"DecodePointer");
LAB_1001308f:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 100130ac FUN_100130ac

LPVOID FUN_100130ac(void)

{
  LPVOID lpTlsValue;
  
  lpTlsValue = TlsGetValue(DAT_10035fc4);
  if (lpTlsValue == (LPVOID)0x0) {
    lpTlsValue = (LPVOID)FUN_10013035(DAT_1003a150);
    TlsSetValue(DAT_10035fc4,lpTlsValue);
  }
  return lpTlsValue;
}


// 100130de __mtterm

/* Library Function - Single Match
    __mtterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __mtterm(void)

{
  code *pcVar1;
  int iVar2;
  
  if (DAT_10035fc0 != -1) {
    iVar2 = DAT_10035fc0;
    pcVar1 = (code *)FUN_10013035(DAT_1003a158);
    (*pcVar1)(iVar2);
    DAT_10035fc0 = -1;
  }
  if (DAT_10035fc4 != 0xffffffff) {
    TlsFree(DAT_10035fc4);
    DAT_10035fc4 = 0xffffffff;
  }
  __mtdeletelocks();
  return;
}


// 1001311b FUN_1001311b

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

void __cdecl FUN_1001311b(int param_1,int param_2)

{
  HMODULE hModule;
  int iVar1;
  FARPROC pFVar2;
  
  hModule = GetModuleHandleA("KERNEL32.DLL");
  *(undefined **)(param_1 + 0x5c) = &DAT_10036620;
  *(undefined4 *)(param_1 + 0x14) = 1;
  if (hModule != (HMODULE)0x0) {
    iVar1 = FUN_10012f52();
    if (iVar1 != 0) {
      pFVar2 = GetProcAddress(hModule,"EncodePointer");
      *(FARPROC *)(param_1 + 0x1f8) = pFVar2;
      pFVar2 = GetProcAddress(hModule,"DecodePointer");
      *(FARPROC *)(param_1 + 0x1fc) = pFVar2;
    }
  }
  *(undefined4 *)(param_1 + 0x70) = 1;
  *(undefined1 *)(param_1 + 200) = 0x43;
  *(undefined1 *)(param_1 + 0x14b) = 0x43;
  *(undefined **)(param_1 + 0x68) = &DAT_10035fd0;
  InterlockedIncrement((LONG *)&DAT_10035fd0);
  __lock(0xc);
  *(int *)(param_1 + 0x6c) = param_2;
  if (param_2 == 0) {
    *(undefined **)(param_1 + 0x6c) = PTR_DAT_100365d8;
  }
  ___addlocaleref(*(LONG **)(param_1 + 0x6c));
  FUN_100131d1();
  return;
}


// 100131d1 FUN_100131d1

void FUN_100131d1(void)

{
  FUN_100189ed(0xc);
  return;
}


// 100131da FUN_100131da

DWORD * FUN_100131da(void)

{
  DWORD dwErrCode;
  code *pcVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD DVar4;
  undefined4 uVar5;
  DWORD *pDVar6;
  
  dwErrCode = GetLastError();
  uVar5 = DAT_10035fc0;
  pcVar1 = (code *)FUN_100130ac();
  pDVar2 = (DWORD *)(*pcVar1)(uVar5);
  if (pDVar2 == (DWORD *)0x0) {
    pDVar2 = (DWORD *)FUN_100011f0(1,0x214);
    if (pDVar2 != (DWORD *)0x0) {
      uVar5 = DAT_10035fc0;
      pDVar6 = pDVar2;
      pcVar1 = (code *)FUN_10013035(DAT_1003a154);
      iVar3 = (*pcVar1)(uVar5,pDVar6);
      if (iVar3 == 0) {
        FUN_100012d0(pDVar2);
        pDVar2 = (DWORD *)0x0;
      }
      else {
        FUN_1001311b((int)pDVar2,0);
        DVar4 = GetCurrentThreadId();
        pDVar2[1] = 0xffffffff;
        *pDVar2 = DVar4;
      }
    }
  }
  SetLastError(dwErrCode);
  return pDVar2;
}


// 10013251 __getptd

/* Library Function - Single Match
    __getptd
   
   Library: Visual Studio 2005 Release */

_ptiddata __cdecl __getptd(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = (_ptiddata)FUN_100131da();
  if (p_Var1 == (_ptiddata)0x0) {
    __amsg_exit(0x10);
  }
  return p_Var1;
}


// 10013269 __freefls@4

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __freefls@4
   
   Library: Visual Studio 2005 Release */

void __freefls_4(int param_1)

{
  LONG *pLVar1;
  LONG LVar2;
  
  if (param_1 != 0) {
    if (*(int *)(param_1 + 0x24) != 0) {
      FUN_100012d0(*(int *)(param_1 + 0x24));
    }
    if (*(int *)(param_1 + 0x2c) != 0) {
      FUN_100012d0(*(int *)(param_1 + 0x2c));
    }
    if (*(int *)(param_1 + 0x34) != 0) {
      FUN_100012d0(*(int *)(param_1 + 0x34));
    }
    if (*(int *)(param_1 + 0x3c) != 0) {
      FUN_100012d0(*(int *)(param_1 + 0x3c));
    }
    if (*(int *)(param_1 + 0x44) != 0) {
      FUN_100012d0(*(int *)(param_1 + 0x44));
    }
    if (*(int *)(param_1 + 0x48) != 0) {
      FUN_100012d0(*(int *)(param_1 + 0x48));
    }
    if (*(undefined **)(param_1 + 0x5c) != &DAT_10036620) {
      FUN_100012d0(*(undefined **)(param_1 + 0x5c));
    }
    __lock(0xd);
    pLVar1 = *(LONG **)(param_1 + 0x68);
    if (pLVar1 != (LONG *)0x0) {
      LVar2 = InterlockedDecrement(pLVar1);
      if ((LVar2 == 0) && (pLVar1 != (LONG *)&DAT_10035fd0)) {
        FUN_100012d0(pLVar1);
      }
    }
    FUN_10013375();
    __lock(0xc);
    pLVar1 = *(LONG **)(param_1 + 0x6c);
    if (pLVar1 != (LONG *)0x0) {
      ___removelocaleref(pLVar1);
      if (((pLVar1 != (LONG *)PTR_DAT_100365d8) && (pLVar1 != (LONG *)&DAT_10036500)) &&
         (*pLVar1 == 0)) {
        ___freetlocinfo((int)pLVar1);
      }
    }
    FUN_10013381();
    FUN_100012d0(param_1);
  }
  return;
}


// 10013375 FUN_10013375

void FUN_10013375(void)

{
  FUN_100189ed(0xd);
  return;
}


// 10013381 FUN_10013381

void FUN_10013381(void)

{
  FUN_100189ed(0xc);
  return;
}


// 1001338a __freeptd

/* Library Function - Single Match
    __freeptd
   
   Library: Visual Studio 2005 Release */

void __cdecl __freeptd(_ptiddata _Ptd)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  undefined4 uVar4;
  
  if (DAT_10035fc0 != -1) {
    if ((_Ptd == (_ptiddata)0x0) && (pvVar1 = TlsGetValue(DAT_10035fc4), pvVar1 != (LPVOID)0x0)) {
      iVar3 = DAT_10035fc0;
      pcVar2 = (code *)TlsGetValue(DAT_10035fc4);
      _Ptd = (_ptiddata)(*pcVar2)(iVar3);
    }
    uVar4 = 0;
    iVar3 = DAT_10035fc0;
    pcVar2 = (code *)FUN_10013035(DAT_1003a154);
    (*pcVar2)(iVar3,uVar4);
    __freefls_4((int)_Ptd);
  }
  if (DAT_10035fc4 != 0xffffffff) {
    TlsSetValue(DAT_10035fc4,(LPVOID)0x0);
  }
  return;
}


// 100133f3 __mtinit

/* Library Function - Single Match
    __mtinit
   
   Library: Visual Studio 2005 Release */

int __cdecl __mtinit(void)

{
  HMODULE hModule;
  BOOL BVar1;
  int iVar2;
  code *pcVar3;
  DWORD *pDVar4;
  DWORD DVar5;
  code *pcVar6;
  DWORD *pDVar7;
  
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    __mtterm();
    return 0;
  }
  DAT_1003a14c = GetProcAddress(hModule,"FlsAlloc");
  DAT_1003a150 = GetProcAddress(hModule,"FlsGetValue");
  DAT_1003a154 = GetProcAddress(hModule,"FlsSetValue");
  DAT_1003a158 = GetProcAddress(hModule,"FlsFree");
  if ((((DAT_1003a14c == (FARPROC)0x0) || (DAT_1003a150 == (FARPROC)0x0)) ||
      (DAT_1003a154 == (FARPROC)0x0)) || (DAT_1003a158 == (FARPROC)0x0)) {
    DAT_1003a150 = TlsGetValue_exref;
    DAT_1003a14c = (FARPROC)&LAB_100130a3;
    DAT_1003a154 = TlsSetValue_exref;
    DAT_1003a158 = TlsFree_exref;
  }
  DAT_10035fc4 = TlsAlloc();
  if ((DAT_10035fc4 != 0xffffffff) && (BVar1 = TlsSetValue(DAT_10035fc4,DAT_1003a150), BVar1 != 0))
  {
    __init_pointers();
    DAT_1003a14c = (FARPROC)FUN_10012fbe((int)DAT_1003a14c);
    DAT_1003a150 = (FARPROC)FUN_10012fbe((int)DAT_1003a150);
    DAT_1003a154 = (FARPROC)FUN_10012fbe((int)DAT_1003a154);
    DAT_1003a158 = (FARPROC)FUN_10012fbe((int)DAT_1003a158);
    iVar2 = __mtinitlocks();
    if (iVar2 != 0) {
      pcVar6 = __freefls_4;
      pcVar3 = (code *)FUN_10013035((int)DAT_1003a14c);
      DAT_10035fc0 = (*pcVar3)(pcVar6);
      if ((DAT_10035fc0 != -1) && (pDVar4 = (DWORD *)FUN_100011f0(1,0x214), pDVar4 != (DWORD *)0x0))
      {
        iVar2 = DAT_10035fc0;
        pDVar7 = pDVar4;
        pcVar3 = (code *)FUN_10013035((int)DAT_1003a154);
        iVar2 = (*pcVar3)(iVar2,pDVar7);
        if (iVar2 != 0) {
          FUN_1001311b((int)pDVar4,0);
          DVar5 = GetCurrentThreadId();
          pDVar4[1] = 0xffffffff;
          *pDVar4 = DVar5;
          return 1;
        }
      }
    }
    __mtterm();
  }
  return 0;
}


// 10013577 FUN_10013577

void __cdecl FUN_10013577(undefined4 param_1)

{
  DAT_1003a164 = param_1;
  return;
}


// 10013581 __amsg_exit

/* Library Function - Single Match
    __amsg_exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __amsg_exit(int param_1)

{
  code *pcVar1;
  
  __FF_MSGBANNER();
  FUN_10012d79(param_1);
  pcVar1 = (code *)FUN_10013035((int)PTR___exit_10035fc8);
  (*pcVar1)(0xff);
  return;
}


// 100135a5 FUN_100135a5

void __cdecl FUN_100135a5(undefined4 param_1)

{
  HMODULE hModule;
  FARPROC pFVar1;
  
  hModule = GetModuleHandleA("mscoree.dll");
  if (hModule != (HMODULE)0x0) {
    pFVar1 = GetProcAddress(hModule,"CorExitProcess");
    if (pFVar1 != (FARPROC)0x0) {
      (*pFVar1)(param_1);
    }
  }
  return;
}


// 100135cb ___crtExitProcess

/* Library Function - Single Match
    ___crtExitProcess
   
   Library: Visual Studio 2005 Release */

void __cdecl ___crtExitProcess(int param_1)

{
  FUN_100135a5(param_1);
                    /* WARNING: Subroutine does not return */
  ExitProcess(param_1);
}


// 100135e0 FUN_100135e0

void FUN_100135e0(void)

{
  __lock(8);
  return;
}


// 100135e9 FUN_100135e9

void FUN_100135e9(void)

{
  FUN_100189ed(8);
  return;
}


// 100135f2 FUN_100135f2

void __cdecl FUN_100135f2(undefined4 *param_1)

{
  undefined4 *in_EAX;
  
  for (; in_EAX < param_1; in_EAX = in_EAX + 1) {
    if ((code *)*in_EAX != (code *)0x0) {
      (*(code *)*in_EAX)();
    }
  }
  return;
}


// 1001360a __initterm_e

/* Library Function - Single Match
    __initterm_e
   
   Library: Visual Studio 2005 Release */

void __cdecl __initterm_e(undefined4 *param_1,undefined4 *param_2)

{
  int iVar1;
  
  iVar1 = 0;
  while ((param_1 < param_2 && (iVar1 == 0))) {
    if ((code *)*param_1 != (code *)0x0) {
      iVar1 = (*(code *)*param_1)();
    }
    param_1 = param_1 + 1;
  }
  return;
}


// 1001362a __get_osplatform

/* Library Function - Single Match
    __get_osplatform
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __get_osplatform(int *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (int *)0x0) && (DAT_1003a16c != 0)) {
    *param_1 = DAT_1003a16c;
    return 0;
  }
  pDVar1 = FUN_10011e63();
  *pDVar1 = 0x16;
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 10013661 FUN_10013661

undefined4 __cdecl FUN_10013661(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (undefined4 *)0x0) && (DAT_1003a16c != 0)) {
    *param_1 = DAT_1003a178;
    return 0;
  }
  pDVar1 = FUN_10011e63();
  *pDVar1 = 0x16;
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 1001369d __cinit

/* Library Function - Single Match
    __cinit
   
   Library: Visual Studio 2005 Release */

int __cdecl __cinit(int param_1)

{
  BOOL BVar1;
  int iVar2;
  undefined4 *puVar3;
  
  BVar1 = __IsNonwritableInCurrentImage((PBYTE)&PTR___fpmath_100290fc);
  if (BVar1 != 0) {
    __fpmath(param_1);
  }
  __initp_misc_cfltcvt_tab();
  iVar2 = __initterm_e((undefined4 *)&DAT_1002521c,(undefined4 *)&DAT_10025234);
  if (iVar2 == 0) {
    _atexit((_func_4879 *)&LAB_10015029);
    puVar3 = &DAT_10025164;
    do {
      if ((code *)*puVar3 != (code *)0x0) {
        (*(code *)*puVar3)();
      }
      puVar3 = puVar3 + 1;
    } while (puVar3 < &DAT_10025218);
    if ((DAT_1004bcf4 != (code *)0x0) &&
       (BVar1 = __IsNonwritableInCurrentImage((PBYTE)&DAT_1004bcf4), BVar1 != 0)) {
      (*DAT_1004bcf4)(0,2,0);
    }
    iVar2 = 0;
  }
  return iVar2;
}


// 1001372f FUN_1001372f

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x10013802) */

void __cdecl FUN_1001372f(int param_1,int param_2,int param_3)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  int iVar4;
  code *pcVar5;
  
  __lock(8);
  if (DAT_1003a1ac != 1) {
    DAT_1003a1a8 = 1;
    DAT_1003a1a4 = (undefined1)param_3;
    if (param_2 == 0) {
      piVar2 = (int *)FUN_10013035(DAT_1004bcec);
      piVar3 = (int *)FUN_10013035(DAT_1004bce8);
      if (piVar2 != (int *)0x0) {
        while (piVar3 = piVar3 + -1, piVar2 <= piVar3) {
          if (*piVar3 != 0) {
            iVar1 = *piVar3;
            iVar4 = __encoded_null();
            if (iVar1 != iVar4) {
              pcVar5 = (code *)FUN_10013035(iVar1);
              (*pcVar5)();
            }
          }
        }
      }
      FUN_100135f2((undefined4 *)&DAT_10025244);
    }
    FUN_100135f2((undefined4 *)&DAT_1002524c);
  }
  FUN_100137fc();
  if (param_3 != 0) {
    return;
  }
  DAT_1003a1ac = 1;
  FUN_100189ed(8);
  ___crtExitProcess(param_1);
  return;
}


// 100137fc FUN_100137fc

void FUN_100137fc(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + 0x10) != 0) {
    FUN_100189ed(8);
  }
  return;
}


// 10013811 __exit

/* Library Function - Single Match
    __exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __exit(int _Code)

{
  FUN_1001372f(_Code,1,0);
  return;
}


// 10013822 __cexit

/* Library Function - Single Match
    __cexit
   
   Library: Visual Studio 2005 Release */

void __cdecl __cexit(void)

{
  FUN_1001372f(0,0,1);
  return;
}


// 10013831 __init_pointers

/* Library Function - Single Match
    __init_pointers
   
   Library: Visual Studio 2005 Release */

void __cdecl __init_pointers(void)

{
  undefined4 uVar1;
  
  uVar1 = __encoded_null();
  FUN_1001a57e(uVar1);
  FUN_1001a49f(uVar1);
  FUN_10011cfe(uVar1);
  FUN_10013577(uVar1);
  FUN_1001a495(uVar1);
  __initp_misc_winsig(uVar1);
  FUN_1000f4e5();
  __initp_eh_hooks();
  PTR___exit_10035fc8 = (undefined *)FUN_10012fbe(0x10013811);
  return;
}


// 10013880 __SEH_prolog4

/* WARNING: This is an inlined function */
/* WARNING: Unable to track spacebase fully for stack */
/* WARNING: Variable defined which should be unmapped: param_2 */
/* Library Function - Single Match
    __SEH_prolog4
   
   Library: Visual Studio */

void __cdecl __SEH_prolog4(undefined4 param_1,int param_2)

{
  int iVar1;
  undefined4 unaff_EBX;
  undefined4 unaff_ESI;
  undefined4 unaff_EDI;
  int *unaff_FS_OFFSET;
  undefined4 unaff_retaddr;
  uint auStack_1c [5];
  undefined1 local_8 [8];
  
  iVar1 = -param_2;
  *(undefined4 *)((int)auStack_1c + iVar1 + 0x10) = unaff_EBX;
  *(undefined4 *)((int)auStack_1c + iVar1 + 0xc) = unaff_ESI;
  *(undefined4 *)((int)auStack_1c + iVar1 + 8) = unaff_EDI;
  *(uint *)((int)auStack_1c + iVar1 + 4) = DAT_100366b8 ^ (uint)&param_2;
  *(undefined4 *)((int)auStack_1c + iVar1) = unaff_retaddr;
  *unaff_FS_OFFSET = (int)local_8;
  return;
}


// 100138c5 __SEH_epilog4

/* WARNING: This is an inlined function */
/* Library Function - Single Match
    __SEH_epilog4
   
   Library: Visual Studio */

void __SEH_epilog4(void)

{
  undefined4 *unaff_EBP;
  undefined4 *unaff_FS_OFFSET;
  undefined4 unaff_retaddr;
  
  *unaff_FS_OFFSET = unaff_EBP[-4];
  *unaff_EBP = unaff_retaddr;
  return;
}


// 100138e0 __except_handler4

/* Library Function - Single Match
    __except_handler4
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __except_handler4(int *param_1,PVOID param_2,undefined4 param_3)

{
  int iVar1;
  uint uVar2;
  bool bVar3;
  int iVar4;
  BOOL BVar5;
  undefined4 uVar6;
  uint uVar7;
  int *piVar8;
  undefined4 local_c;
  int *local_8;
  undefined4 local_4;
  
  piVar8 = (int *)(*(uint *)((int)param_2 + 8) ^ DAT_100366b8);
  bVar3 = false;
  local_c = 1;
  iVar1 = (int)param_2 + 0x10;
  if (*piVar8 != -2) {
    __security_check_cookie(piVar8[1] + iVar1 ^ *(uint *)(*piVar8 + iVar1));
  }
  __security_check_cookie(piVar8[3] + iVar1 ^ *(uint *)(piVar8[2] + iVar1));
  if ((*(byte *)(param_1 + 1) & 0x66) == 0) {
    local_8 = param_1;
    local_4 = param_3;
    *(int ***)((int)param_2 + -4) = &local_8;
    uVar7 = *(uint *)((int)param_2 + 0xc);
    if (*(uint *)((int)param_2 + 0xc) == 0xfffffffe) {
      return 1;
    }
    do {
      uVar2 = piVar8[uVar7 * 3 + 4];
      if ((undefined *)piVar8[uVar7 * 3 + 5] != (undefined *)0x0) {
        iVar4 = _EH4_CallFilterFunc((undefined *)piVar8[uVar7 * 3 + 5]);
        bVar3 = true;
        if (iVar4 < 0) {
          local_c = 0;
          goto LAB_1001398b;
        }
        if (0 < iVar4) {
          if (((*param_1 == -0x1f928c9d) && (DAT_1004bce0 != (code *)0x0)) &&
             (BVar5 = __IsNonwritableInCurrentImage((PBYTE)&DAT_1004bce0), BVar5 != 0)) {
            (*DAT_1004bce0)(param_1,1);
          }
          _EH4_GlobalUnwind(param_2);
          if (*(uint *)((int)param_2 + 0xc) != uVar7) {
            _EH4_LocalUnwind((int)param_2,uVar7,iVar1,&DAT_100366b8);
          }
          *(uint *)((int)param_2 + 0xc) = uVar2;
          if (*piVar8 != -2) {
            __security_check_cookie(piVar8[1] + iVar1 ^ *(uint *)(*piVar8 + iVar1));
          }
          __security_check_cookie(piVar8[3] + iVar1 ^ *(uint *)(piVar8[2] + iVar1));
          uVar6 = _EH4_TransferToHandler((undefined *)(piVar8 + uVar7 * 3 + 4)[2]);
          return uVar6;
        }
      }
      uVar7 = uVar2;
    } while (uVar2 != 0xfffffffe);
    if (!bVar3) {
      return 1;
    }
  }
  else {
    if (*(int *)((int)param_2 + 0xc) == -2) {
      return 1;
    }
    _EH4_LocalUnwind((int)param_2,0xfffffffe,iVar1,&DAT_100366b8);
  }
LAB_1001398b:
  if (*piVar8 != -2) {
    __security_check_cookie(piVar8[1] + iVar1 ^ *(uint *)(*piVar8 + iVar1));
  }
  __security_check_cookie(piVar8[3] + iVar1 ^ *(uint *)(piVar8[2] + iVar1));
  return local_c;
}


// 10013a76 FUN_10013a76

undefined4 FUN_10013a76(void)

{
  int in_EAX;
  
  if (in_EAX == 0x3a4) {
    return 0x411;
  }
  if (in_EAX == 0x3a8) {
    return 0x804;
  }
  if (in_EAX == 0x3b5) {
    return 0x412;
  }
  if (in_EAX != 0x3b6) {
    return 0;
  }
  return 0x404;
}


// 10013aa5 setSBCS

/* Library Function - Single Match
    void __cdecl setSBCS(struct threadmbcinfostruct *)
   
   Library: Visual Studio 2005 Release */

void __cdecl setSBCS(threadmbcinfostruct *param_1)

{
  int in_EAX;
  undefined1 *puVar1;
  int iVar2;
  
  iVar2 = 0x101;
  puVar1 = (undefined1 *)(in_EAX + 0x1c);
  _memset(puVar1,0,0x101);
  *(undefined4 *)(in_EAX + 4) = 0;
  *(undefined4 *)(in_EAX + 8) = 0;
  *(undefined4 *)(in_EAX + 0xc) = 0;
  *(undefined4 *)(in_EAX + 0x10) = 0;
  *(undefined4 *)(in_EAX + 0x14) = 0;
  *(undefined4 *)(in_EAX + 0x18) = 0;
  do {
    *puVar1 = puVar1[(int)&DAT_10035fd0 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  puVar1 = (undefined1 *)(in_EAX + 0x11d);
  iVar2 = 0x100;
  do {
    *puVar1 = puVar1[(int)&DAT_10035fd0 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  return;
}


// 10013afa setSBUpLow

/* Library Function - Single Match
    void __cdecl setSBUpLow(struct threadmbcinfostruct *)
   
   Library: Visual Studio 2005 Release */

void __cdecl setSBUpLow(threadmbcinfostruct *param_1)

{
  byte *pbVar1;
  char *pcVar2;
  BOOL BVar3;
  uint uVar4;
  CHAR CVar5;
  char cVar6;
  BYTE *pBVar7;
  int unaff_ESI;
  _cpinfo local_51c;
  WORD local_508 [52];
  undefined1 local_4a0 [408];
  CHAR local_308 [256];
  CHAR local_208 [256];
  CHAR local_108 [256];
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)local_4a0;
  BVar3 = GetCPInfo(*(UINT *)(unaff_ESI + 4),&local_51c);
  if (BVar3 == 0) {
    uVar4 = 0;
    do {
      pcVar2 = (char *)(unaff_ESI + 0x11d + uVar4);
      if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) + 0x20 < (char *)0x1a) {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        cVar6 = (char)uVar4 + ' ';
LAB_10013c61:
        *pcVar2 = cVar6;
      }
      else {
        if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) < (char *)0x1a) {
          pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
          *pbVar1 = *pbVar1 | 0x20;
          cVar6 = (char)uVar4 + -0x20;
          goto LAB_10013c61;
        }
        *pcVar2 = '\0';
      }
      uVar4 = uVar4 + 1;
    } while (uVar4 < 0x100);
  }
  else {
    uVar4 = 0;
    do {
      local_108[uVar4] = (CHAR)uVar4;
      uVar4 = uVar4 + 1;
    } while (uVar4 < 0x100);
    local_108[0] = ' ';
    if (local_51c.LeadByte[0] != 0) {
      pBVar7 = local_51c.LeadByte + 1;
      do {
        uVar4 = (uint)local_51c.LeadByte[0];
        if (uVar4 <= *pBVar7) {
          _memset(local_108 + uVar4,0x20,(*pBVar7 - uVar4) + 1);
        }
        local_51c.LeadByte[0] = pBVar7[1];
        pBVar7 = pBVar7 + 2;
      } while (local_51c.LeadByte[0] != 0);
    }
    ___crtGetStringTypeA
              ((_locale_t)0x0,1,local_108,0x100,local_508,*(int *)(unaff_ESI + 4),
               *(BOOL *)(unaff_ESI + 0xc));
    ___crtLCMapStringA((_locale_t)0x0,*(LPCWSTR *)(unaff_ESI + 0xc),0x100,local_108,0x100,local_208,
                       0x100,*(int *)(unaff_ESI + 4),0);
    ___crtLCMapStringA((_locale_t)0x0,*(LPCWSTR *)(unaff_ESI + 0xc),0x200,local_108,0x100,local_308,
                       0x100,*(int *)(unaff_ESI + 4),0);
    uVar4 = 0;
    do {
      if ((local_508[uVar4] & 1) == 0) {
        if ((local_508[uVar4] & 2) != 0) {
          pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
          *pbVar1 = *pbVar1 | 0x20;
          CVar5 = local_308[uVar4];
          goto LAB_10013c08;
        }
        *(undefined1 *)(unaff_ESI + 0x11d + uVar4) = 0;
      }
      else {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        CVar5 = local_208[uVar4];
LAB_10013c08:
        *(CHAR *)(unaff_ESI + 0x11d + uVar4) = CVar5;
      }
      uVar4 = uVar4 + 1;
    } while (uVar4 < 0x100);
  }
  __security_check_cookie(local_8 ^ (uint)local_4a0);
  return;
}


// 10013c84 ___updatetmbcinfo

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___updatetmbcinfo
   
   Library: Visual Studio 2005 Release */

pthreadmbcinfo __cdecl ___updatetmbcinfo(void)

{
  _ptiddata p_Var1;
  LONG LVar2;
  pthreadmbcinfo lpAddend;
  
  p_Var1 = __getptd();
  if (((p_Var1->_ownlocale & DAT_100364f4) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xd);
    lpAddend = p_Var1->ptmbcinfo;
    if (lpAddend != (pthreadmbcinfo)PTR_DAT_100363f8) {
      if (lpAddend != (pthreadmbcinfo)0x0) {
        LVar2 = InterlockedDecrement(&lpAddend->refcount);
        if ((LVar2 == 0) && (lpAddend != (pthreadmbcinfo)&DAT_10035fd0)) {
          FUN_100012d0(lpAddend);
        }
      }
      p_Var1->ptmbcinfo = (pthreadmbcinfo)PTR_DAT_100363f8;
      lpAddend = (pthreadmbcinfo)PTR_DAT_100363f8;
      InterlockedIncrement((LONG *)PTR_DAT_100363f8);
    }
    FUN_10013d1f();
  }
  else {
    lpAddend = p_Var1->ptmbcinfo;
  }
  if (lpAddend == (pthreadmbcinfo)0x0) {
    __amsg_exit(0x20);
  }
  return lpAddend;
}


// 10013d1f FUN_10013d1f

void FUN_10013d1f(void)

{
  FUN_100189ed(0xd);
  return;
}


// 10013d28 getSystemCP

/* Library Function - Single Match
    int __cdecl getSystemCP(int)
   
   Library: Visual Studio 2005 Release */

int __cdecl getSystemCP(int param_1)

{
  UINT UVar1;
  int unaff_ESI;
  int local_14 [2];
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,(localeinfo_struct *)0x0);
  DAT_1003a1b0 = 0;
  if (unaff_ESI == -2) {
    DAT_1003a1b0 = 1;
    UVar1 = GetOEMCP();
  }
  else if (unaff_ESI == -3) {
    DAT_1003a1b0 = 1;
    UVar1 = GetACP();
  }
  else {
    if (unaff_ESI != -4) {
      if (local_8 == '\0') {
        DAT_1003a1b0 = 0;
        return unaff_ESI;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return unaff_ESI;
    }
    UVar1 = *(UINT *)(local_14[0] + 4);
    DAT_1003a1b0 = 1;
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return UVar1;
}


// 10013da2 FUN_10013da2

void __cdecl FUN_10013da2(undefined4 param_1,int param_2)

{
  BYTE *pBVar1;
  byte *pbVar2;
  byte bVar3;
  uint uVar4;
  uint uVar5;
  BOOL BVar6;
  undefined2 *puVar7;
  byte *pbVar8;
  undefined4 uVar9;
  int extraout_ECX;
  undefined2 *puVar10;
  int iVar11;
  undefined4 extraout_EDX;
  BYTE *pBVar12;
  threadmbcinfostruct *unaff_EDI;
  uint local_24;
  byte *local_20;
  _cpinfo local_1c;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  uVar4 = getSystemCP((int)unaff_EDI);
  if (uVar4 != 0) {
    local_20 = (byte *)0x0;
    uVar5 = 0;
LAB_10013dde:
    if (*(uint *)((int)&DAT_10036400 + uVar5) != uVar4) goto code_r0x10013dea;
    _memset((void *)(param_2 + 0x1c),0,0x101);
    local_24 = 0;
    pbVar8 = &DAT_10036410 + (int)local_20 * 0x30;
    local_20 = pbVar8;
    do {
      for (; (*pbVar8 != 0 && (bVar3 = pbVar8[1], bVar3 != 0)); pbVar8 = pbVar8 + 2) {
        for (uVar5 = (uint)*pbVar8; uVar5 <= bVar3; uVar5 = uVar5 + 1) {
          pbVar2 = (byte *)(param_2 + 0x1d + uVar5);
          *pbVar2 = *pbVar2 | *(byte *)(local_24 + 0x100363fc);
          bVar3 = pbVar8[1];
        }
      }
      local_24 = local_24 + 1;
      pbVar8 = local_20 + 8;
      local_20 = pbVar8;
    } while (local_24 < 4);
    *(uint *)(param_2 + 4) = uVar4;
    *(undefined4 *)(param_2 + 8) = 1;
    uVar9 = FUN_10013a76();
    *(undefined4 *)(param_2 + 0xc) = uVar9;
    puVar7 = (undefined2 *)(param_2 + 0x10);
    puVar10 = (undefined2 *)(&DAT_10036404 + extraout_ECX);
    iVar11 = 6;
    do {
      *puVar7 = *puVar10;
      puVar10 = puVar10 + 1;
      puVar7 = puVar7 + 1;
      iVar11 = iVar11 + -1;
    } while (iVar11 != 0);
    goto LAB_10013f0f;
  }
LAB_10013dcb:
  setSBCS(unaff_EDI);
LAB_10013f6c:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
code_r0x10013dea:
  local_20 = (byte *)((int)local_20 + 1);
  uVar5 = uVar5 + 0x30;
  if (0xef < uVar5) goto code_r0x10013df7;
  goto LAB_10013dde;
code_r0x10013df7:
  if (((uVar4 == 65000) || (uVar4 == 0xfde9)) ||
     (BVar6 = IsValidCodePage(uVar4 & 0xffff), BVar6 == 0)) goto LAB_10013f6c;
  BVar6 = GetCPInfo(uVar4,&local_1c);
  if (BVar6 != 0) {
    _memset((void *)(param_2 + 0x1c),0,0x101);
    *(uint *)(param_2 + 4) = uVar4;
    *(undefined4 *)(param_2 + 0xc) = 0;
    if (local_1c.MaxCharSize < 2) {
      *(undefined4 *)(param_2 + 8) = 0;
    }
    else {
      if (local_1c.LeadByte[0] != '\0') {
        pBVar12 = local_1c.LeadByte + 1;
        do {
          bVar3 = *pBVar12;
          if (bVar3 == 0) break;
          for (uVar4 = (uint)pBVar12[-1]; uVar4 <= bVar3; uVar4 = uVar4 + 1) {
            pbVar8 = (byte *)(param_2 + 0x1d + uVar4);
            *pbVar8 = *pbVar8 | 4;
          }
          pBVar1 = pBVar12 + 1;
          pBVar12 = pBVar12 + 2;
        } while (*pBVar1 != 0);
      }
      pbVar8 = (byte *)(param_2 + 0x1e);
      iVar11 = 0xfe;
      do {
        *pbVar8 = *pbVar8 | 8;
        pbVar8 = pbVar8 + 1;
        iVar11 = iVar11 + -1;
      } while (iVar11 != 0);
      uVar9 = FUN_10013a76();
      *(undefined4 *)(param_2 + 0xc) = uVar9;
      *(undefined4 *)(param_2 + 8) = extraout_EDX;
    }
    *(undefined4 *)(param_2 + 0x10) = 0;
    *(undefined4 *)(param_2 + 0x14) = 0;
    *(undefined4 *)(param_2 + 0x18) = 0;
LAB_10013f0f:
    setSBUpLow(unaff_EDI);
    goto LAB_10013f6c;
  }
  if (DAT_1003a1b0 == 0) goto LAB_10013f6c;
  goto LAB_10013dcb;
}


// 10013f7b __setmbcp

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __setmbcp
   
   Library: Visual Studio 2005 Release */

int __cdecl __setmbcp(int _CodePage)

{
  _ptiddata p_Var1;
  int iVar2;
  pthreadmbcinfo ptVar3;
  LONG LVar4;
  DWORD *pDVar5;
  int iVar6;
  pthreadmbcinfo ptVar7;
  pthreadmbcinfo ptVar8;
  int in_stack_ffffffc8;
  int local_24;
  
  local_24 = -1;
  p_Var1 = __getptd();
  ___updatetmbcinfo();
  ptVar3 = p_Var1->ptmbcinfo;
  iVar2 = getSystemCP(in_stack_ffffffc8);
  if (iVar2 == ptVar3->mbcodepage) {
    local_24 = 0;
  }
  else {
    ptVar3 = (pthreadmbcinfo)FUN_10001270(0x220);
    if (ptVar3 != (pthreadmbcinfo)0x0) {
      ptVar7 = p_Var1->ptmbcinfo;
      ptVar8 = ptVar3;
      for (iVar6 = 0x88; iVar6 != 0; iVar6 = iVar6 + -1) {
        ptVar8->refcount = ptVar7->refcount;
        ptVar7 = (pthreadmbcinfo)&ptVar7->mbcodepage;
        ptVar8 = (pthreadmbcinfo)&ptVar8->mbcodepage;
      }
      ptVar3->refcount = 0;
      local_24 = FUN_10013da2(iVar2,(int)ptVar3);
      if (local_24 == 0) {
        LVar4 = InterlockedDecrement(&p_Var1->ptmbcinfo->refcount);
        if ((LVar4 == 0) && (p_Var1->ptmbcinfo != (pthreadmbcinfo)&DAT_10035fd0)) {
          FUN_100012d0(p_Var1->ptmbcinfo);
        }
        p_Var1->ptmbcinfo = ptVar3;
        InterlockedIncrement((LONG *)ptVar3);
        if (((p_Var1->_ownlocale & 2) == 0) && (((byte)DAT_100364f4 & 1) == 0)) {
          __lock(0xd);
          _DAT_1003a1c0 = ptVar3->mbcodepage;
          _DAT_1003a1c4 = ptVar3->ismbcodepage;
          _DAT_1003a1c8 = *(undefined4 *)ptVar3->mbulinfo;
          for (iVar2 = 0; iVar2 < 5; iVar2 = iVar2 + 1) {
            (&DAT_1003a1b4)[iVar2] = ptVar3->mbulinfo[iVar2 + 2];
          }
          for (iVar2 = 0; iVar2 < 0x101; iVar2 = iVar2 + 1) {
            (&DAT_100361f0)[iVar2] = ptVar3->mbctype[iVar2 + 4];
          }
          for (iVar2 = 0; iVar2 < 0x100; iVar2 = iVar2 + 1) {
            (&DAT_100362f8)[iVar2] = ptVar3->mbcasemap[iVar2 + 4];
          }
          LVar4 = InterlockedDecrement((LONG *)PTR_DAT_100363f8);
          if ((LVar4 == 0) && (PTR_DAT_100363f8 != &DAT_10035fd0)) {
            FUN_100012d0(PTR_DAT_100363f8);
          }
          PTR_DAT_100363f8 = (undefined *)ptVar3;
          InterlockedIncrement((LONG *)ptVar3);
          FUN_100140dc();
        }
      }
      else if (local_24 == -1) {
        if (ptVar3 != (pthreadmbcinfo)&DAT_10035fd0) {
          FUN_100012d0(ptVar3);
        }
        pDVar5 = FUN_10011e63();
        *pDVar5 = 0x16;
      }
    }
  }
  return local_24;
}


// 100140dc FUN_100140dc

void FUN_100140dc(void)

{
  FUN_100189ed(0xd);
  return;
}


// 10014115 ___initmbctable

/* Library Function - Single Match
    ___initmbctable
   
   Library: Visual Studio 2005 Release */

undefined4 ___initmbctable(void)

{
  if (DAT_1004bcf0 == 0) {
    __setmbcp(-3);
    DAT_1004bcf0 = 1;
  }
  return 0;
}


// 10014133 ___freetlocinfo

/* Library Function - Single Match
    ___freetlocinfo
   
   Library: Visual Studio 2005 Release */

void __cdecl ___freetlocinfo(int param_1)

{
  undefined **ppuVar1;
  int *piVar2;
  int iVar3;
  int *piVar4;
  
  if ((((*(undefined ***)(param_1 + 0xbc) != (undefined **)0x0) &&
       (*(undefined ***)(param_1 + 0xbc) != &PTR_DAT_10036b38)) &&
      (*(int **)(param_1 + 0xb0) != (int *)0x0)) && (**(int **)(param_1 + 0xb0) == 0)) {
    piVar4 = *(int **)(param_1 + 0xb8);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_100012d0(piVar4);
      ___free_lconv_mon(*(int *)(param_1 + 0xbc));
    }
    piVar4 = *(int **)(param_1 + 0xb4);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_100012d0(piVar4);
      ___free_lconv_num(*(int **)(param_1 + 0xbc));
    }
    FUN_100012d0(*(undefined4 *)(param_1 + 0xb0));
    FUN_100012d0(*(undefined4 *)(param_1 + 0xbc));
  }
  if ((*(int **)(param_1 + 0xc0) != (int *)0x0) && (**(int **)(param_1 + 0xc0) == 0)) {
    FUN_100012d0(*(int *)(param_1 + 0xc4) + -0xfe);
    FUN_100012d0(*(int *)(param_1 + 0xcc) + -0x80);
    FUN_100012d0(*(int *)(param_1 + 0xd0) + -0x80);
    FUN_100012d0(*(undefined4 *)(param_1 + 0xc0));
  }
  ppuVar1 = *(undefined ***)(param_1 + 0xd4);
  if ((ppuVar1 != &PTR_DAT_10036a78) && (ppuVar1[0x2d] == (undefined *)0x0)) {
    ___free_lc_time(ppuVar1);
    FUN_100012d0(*(undefined4 *)(param_1 + 0xd4));
  }
  piVar4 = (int *)(param_1 + 0x50);
  iVar3 = 6;
  do {
    if ((((undefined *)piVar4[-2] != &DAT_100364f8) &&
        (piVar2 = (int *)*piVar4, piVar2 != (int *)0x0)) && (*piVar2 == 0)) {
      FUN_100012d0(piVar2);
    }
    if (((piVar4[-1] != 0) && (piVar2 = (int *)piVar4[1], piVar2 != (int *)0x0)) && (*piVar2 == 0))
    {
      FUN_100012d0(piVar2);
    }
    piVar4 = piVar4 + 4;
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  FUN_100012d0(param_1);
  return;
}


// 10014273 ___addlocaleref

/* Library Function - Single Match
    ___addlocaleref
   
   Library: Visual Studio 2005 Release */

void __cdecl ___addlocaleref(LONG *param_1)

{
  LONG *pLVar1;
  int iVar2;
  
  InterlockedIncrement(param_1);
  if ((LONG *)param_1[0x2c] != (LONG *)0x0) {
    InterlockedIncrement((LONG *)param_1[0x2c]);
  }
  if ((LONG *)param_1[0x2e] != (LONG *)0x0) {
    InterlockedIncrement((LONG *)param_1[0x2e]);
  }
  if ((LONG *)param_1[0x2d] != (LONG *)0x0) {
    InterlockedIncrement((LONG *)param_1[0x2d]);
  }
  if ((LONG *)param_1[0x30] != (LONG *)0x0) {
    InterlockedIncrement((LONG *)param_1[0x30]);
  }
  pLVar1 = param_1 + 0x14;
  iVar2 = 6;
  do {
    if (((undefined *)pLVar1[-2] != &DAT_100364f8) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
      InterlockedIncrement((LONG *)*pLVar1);
    }
    if ((pLVar1[-1] != 0) && ((LONG *)pLVar1[1] != (LONG *)0x0)) {
      InterlockedIncrement((LONG *)pLVar1[1]);
    }
    pLVar1 = pLVar1 + 4;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  InterlockedIncrement((LONG *)(param_1[0x35] + 0xb4));
  return;
}


// 100142f9 ___removelocaleref

/* Library Function - Single Match
    ___removelocaleref
   
   Library: Visual Studio 2005 Release */

LONG * __cdecl ___removelocaleref(LONG *param_1)

{
  LONG *pLVar1;
  int iVar2;
  
  if (param_1 != (LONG *)0x0) {
    InterlockedDecrement(param_1);
    if ((LONG *)param_1[0x2c] != (LONG *)0x0) {
      InterlockedDecrement((LONG *)param_1[0x2c]);
    }
    if ((LONG *)param_1[0x2e] != (LONG *)0x0) {
      InterlockedDecrement((LONG *)param_1[0x2e]);
    }
    if ((LONG *)param_1[0x2d] != (LONG *)0x0) {
      InterlockedDecrement((LONG *)param_1[0x2d]);
    }
    if ((LONG *)param_1[0x30] != (LONG *)0x0) {
      InterlockedDecrement((LONG *)param_1[0x30]);
    }
    pLVar1 = param_1 + 0x14;
    iVar2 = 6;
    do {
      if (((undefined *)pLVar1[-2] != &DAT_100364f8) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
        InterlockedDecrement((LONG *)*pLVar1);
      }
      if ((pLVar1[-1] != 0) && ((LONG *)pLVar1[1] != (LONG *)0x0)) {
        InterlockedDecrement((LONG *)pLVar1[1]);
      }
      pLVar1 = pLVar1 + 4;
      iVar2 = iVar2 + -1;
    } while (iVar2 != 0);
    InterlockedDecrement((LONG *)(param_1[0x35] + 0xb4));
  }
  return param_1;
}


// 10014385 __updatetlocinfoEx_nolock

/* Library Function - Single Match
    __updatetlocinfoEx_nolock
   
   Library: Visual Studio 2005 Release */

LONG * __updatetlocinfoEx_nolock(void)

{
  LONG *pLVar1;
  undefined4 *in_EAX;
  LONG *unaff_EDI;
  
  if ((unaff_EDI != (LONG *)0x0) && (in_EAX != (undefined4 *)0x0)) {
    pLVar1 = (LONG *)*in_EAX;
    if (pLVar1 != unaff_EDI) {
      *in_EAX = unaff_EDI;
      ___addlocaleref(unaff_EDI);
      if (pLVar1 != (LONG *)0x0) {
        ___removelocaleref(pLVar1);
        if ((*pLVar1 == 0) && (pLVar1 != (LONG *)&DAT_10036500)) {
          ___freetlocinfo((int)pLVar1);
        }
      }
    }
    return unaff_EDI;
  }
  return (LONG *)0x0;
}


// 100143c3 ___updatetlocinfo

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___updatetlocinfo
   
   Library: Visual Studio 2005 Release */

pthreadlocinfo __cdecl ___updatetlocinfo(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = __getptd();
  if (((p_Var1->_ownlocale & DAT_100364f4) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xc);
    __updatetlocinfoEx_nolock();
    FUN_1001442d();
  }
  else {
    p_Var1 = __getptd();
    p_Var1 = (_ptiddata)p_Var1->ptlocinfo;
  }
  if (p_Var1 == (_ptiddata)0x0) {
    __amsg_exit(0x20);
  }
  return (pthreadlocinfo)p_Var1;
}


// 1001442d FUN_1001442d

void FUN_1001442d(void)

{
  FUN_100189ed(0xc);
  return;
}


// 10014439 __crtLCMapStringA_stat

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* Library Function - Single Match
    int __cdecl __crtLCMapStringA_stat(struct localeinfo_struct *,unsigned long,unsigned long,char
   const *,int,char *,int,int,int)
   
   Library: Visual Studio 2005 Release */

int __cdecl
__crtLCMapStringA_stat
          (localeinfo_struct *param_1,ulong param_2,ulong param_3,char *param_4,int param_5,
          char *param_6,int param_7,int param_8,int param_9)

{
  uint uVar1;
  bool bVar2;
  int iVar3;
  DWORD DVar4;
  char *pcVar5;
  uint cchWideChar;
  LPCWSTR pWVar6;
  UINT UVar7;
  int *in_ECX;
  char *pcVar8;
  LPSTR lpMultiByteStr;
  int local_14;
  LPCWSTR local_10;
  uint local_c;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  if (DAT_1003a1f0 == 0) {
    iVar3 = LCMapStringW(0,0x100,L"",1,(LPWSTR)0x0,0);
    if (iVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_1003a1f0 = 2;
      }
    }
    else {
      DAT_1003a1f0 = 1;
    }
  }
  pcVar5 = (char *)param_3;
  pcVar8 = param_4;
  if (0 < (int)param_4) {
    do {
      pcVar8 = pcVar8 + -1;
      if (*pcVar5 == '\0') goto LAB_100144a8;
      pcVar5 = pcVar5 + 1;
    } while (pcVar8 != (char *)0x0);
    pcVar8 = (char *)0xffffffff;
LAB_100144a8:
    pcVar5 = param_4 + -(int)pcVar8;
    bVar2 = (int)(pcVar5 + -1) < (int)param_4;
    param_4 = pcVar5 + -1;
    if (bVar2) {
      param_4 = pcVar5;
    }
  }
  if ((DAT_1003a1f0 == 2) || (DAT_1003a1f0 == 0)) {
    local_10 = (LPCWSTR)0x0;
    local_14 = 0;
    if (param_1 == (localeinfo_struct *)0x0) {
      param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
    }
    if (param_7 == 0) {
      param_7 = *(int *)(*in_ECX + 4);
    }
    UVar7 = ___ansicp((LCID)param_1);
    if (UVar7 == 0xffffffff) goto LAB_100147c9;
    if (UVar7 == param_7) {
      LCMapStringA((LCID)param_1,param_2,(LPCSTR)param_3,(int)param_4,(LPSTR)param_5,(int)param_6);
    }
    else {
      local_10 = (LPCWSTR)___convertcp(param_7,UVar7,(char *)param_3,(uint *)&param_4,(LPSTR)0x0,0);
      if (local_10 == (LPCWSTR)0x0) goto LAB_100147c9;
      local_c = LCMapStringA((LCID)param_1,param_2,(LPCSTR)local_10,(int)param_4,(LPSTR)0x0,0);
      if (local_c != 0) {
        if (((int)local_c < 1) || (0xffffffe0 < local_c)) {
          pcVar5 = (char *)0x0;
        }
        else if (local_c + 8 < 0x401) {
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_100147a6;
          pcVar5 = &stack0xffffffe4;
        }
        else {
          pcVar5 = (char *)FUN_10001270(local_c + 8);
          if (pcVar5 != (char *)0x0) {
            pcVar5[0] = -0x23;
            pcVar5[1] = -0x23;
            pcVar5[2] = '\0';
            pcVar5[3] = '\0';
            pcVar5 = pcVar5 + 8;
          }
        }
        if (pcVar5 != (char *)0x0) {
          _memset(pcVar5,0,local_c);
          local_c = LCMapStringA((LCID)param_1,param_2,(LPCSTR)local_10,(int)param_4,pcVar5,local_c)
          ;
          if (local_c != 0) {
            local_14 = ___convertcp(UVar7,param_7,pcVar5,&local_c,(LPSTR)param_5,(int)param_6);
          }
          __freea(pcVar5);
        }
      }
    }
LAB_100147a6:
    if (local_10 != (LPCWSTR)0x0) {
      FUN_100012d0(local_10);
    }
    if ((local_14 != 0) && (param_5 != local_14)) {
      FUN_100012d0(local_14);
    }
    goto LAB_100147c9;
  }
  if (DAT_1003a1f0 != 1) goto LAB_100147c9;
  local_c = 0;
  if (param_7 == 0) {
    param_7 = *(int *)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar(param_7,(uint)(param_8 != 0) * 8 + 1,(LPCSTR)param_3,
                                    (int)param_4,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_100147c9;
  if (((int)cchWideChar < 1) || (0xffffffe0 / cchWideChar < 2)) {
    local_10 = (LPCWSTR)0x0;
  }
  else {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar6 = (LPCWSTR)&stack0xffffffdc;
      local_10 = (LPCWSTR)&stack0xffffffdc;
      if (&stack0x00000000 != (undefined1 *)0x24) {
LAB_10014550:
        local_10 = pWVar6 + 4;
      }
    }
    else {
      pWVar6 = (LPCWSTR)FUN_10001270(uVar1);
      local_10 = pWVar6;
      if (pWVar6 != (LPCWSTR)0x0) {
        pWVar6[0] = L'\xdddd';
        pWVar6[1] = L'\0';
        goto LAB_10014550;
      }
    }
  }
  if (local_10 == (LPCWSTR)0x0) goto LAB_100147c9;
  iVar3 = MultiByteToWideChar(param_7,1,(LPCSTR)param_3,(int)param_4,local_10,cchWideChar);
  if ((iVar3 != 0) &&
     (local_c = LCMapStringW((LCID)param_1,param_2,local_10,cchWideChar,(LPWSTR)0x0,0), local_c != 0
     )) {
    if ((param_2 & 0x400) == 0) {
      if (((int)local_c < 1) || (0xffffffe0 / local_c < 2)) {
        pWVar6 = (LPCWSTR)0x0;
      }
      else {
        uVar1 = local_c * 2 + 8;
        if (uVar1 < 0x401) {
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_1001465f;
          pWVar6 = (LPCWSTR)&stack0xffffffe4;
        }
        else {
          pWVar6 = (LPCWSTR)FUN_10001270(uVar1);
          if (pWVar6 != (LPCWSTR)0x0) {
            pWVar6[0] = L'\xdddd';
            pWVar6[1] = L'\0';
            pWVar6 = pWVar6 + 4;
          }
        }
      }
      if (pWVar6 != (LPCWSTR)0x0) {
        iVar3 = LCMapStringW((LCID)param_1,param_2,local_10,cchWideChar,pWVar6,local_c);
        if (iVar3 != 0) {
          lpMultiByteStr = (LPSTR)param_5;
          pcVar5 = param_6;
          if (param_6 == (char *)0x0) {
            lpMultiByteStr = (LPSTR)0x0;
            pcVar5 = (char *)0x0;
          }
          local_c = WideCharToMultiByte(param_7,0,pWVar6,local_c,lpMultiByteStr,(int)pcVar5,
                                        (LPCSTR)0x0,(LPBOOL)0x0);
        }
        __freea(pWVar6);
      }
    }
    else if ((param_6 != (char *)0x0) && ((int)local_c <= (int)param_6)) {
      LCMapStringW((LCID)param_1,param_2,local_10,cchWideChar,(LPWSTR)param_5,(int)param_6);
    }
  }
LAB_1001465f:
  __freea(local_10);
LAB_100147c9:
  iVar3 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar3;
}


// 100147db ___crtLCMapStringA

/* Library Function - Single Match
    ___crtLCMapStringA
   
   Library: Visual Studio 2005 Release */

int __cdecl
___crtLCMapStringA(_locale_t _Plocinfo,LPCWSTR _LocaleName,DWORD _DwMapFlag,LPCSTR _LpSrcStr,
                  int _CchSrc,LPSTR _LpDestStr,int _CchDest,int _Code_page,BOOL _BError)

{
  int iVar1;
  int in_stack_ffffffec;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&stack0xffffffec,_Plocinfo);
  iVar1 = __crtLCMapStringA_stat
                    ((localeinfo_struct *)_LocaleName,_DwMapFlag,(ulong)_LpSrcStr,(char *)_CchSrc,
                     (int)_LpDestStr,(char *)_CchDest,_Code_page,_BError,in_stack_ffffffec);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return iVar1;
}


// 1001481e __isleadbyte_l

/* Library Function - Single Match
    __isleadbyte_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __isleadbyte_l(int _C,_locale_t _Locale)

{
  ushort uVar1;
  int local_14 [2];
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
  uVar1 = *(ushort *)(*(int *)(local_14[0] + 200) + (_C & 0xffU) * 2);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1 & 0x8000;
}


// 10014854 _isleadbyte

/* Library Function - Single Match
    _isleadbyte
   
   Library: Visual Studio 2005 Release */

int __cdecl _isleadbyte(int _C)

{
  int iVar1;
  
  iVar1 = __isleadbyte_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 10014862 __isctype_l

/* Library Function - Single Match
    __isctype_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __isctype_l(int _C,int _Type,_locale_t _Locale)

{
  int iVar1;
  BOOL BVar2;
  CHAR CVar3;
  localeinfo_struct local_1c;
  int local_14;
  char local_10;
  CHAR local_c;
  CHAR local_b;
  undefined1 local_a;
  ushort local_8 [2];
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_1c,_Locale);
  if (_C + 1U < 0x101) {
    local_8[0] = *(ushort *)(local_1c.locinfo[1].lc_category[0].locale + _C * 2);
  }
  else {
    iVar1 = __isleadbyte_l(_C >> 8 & 0xff,&local_1c);
    CVar3 = (CHAR)_C;
    if (iVar1 == 0) {
      local_b = '\0';
      iVar1 = 1;
      local_c = CVar3;
    }
    else {
      _C._0_1_ = (CHAR)((uint)_C >> 8);
      local_c = (CHAR)_C;
      local_a = 0;
      iVar1 = 2;
      local_b = CVar3;
    }
    BVar2 = ___crtGetStringTypeA
                      (&local_1c,1,&local_c,iVar1,local_8,(local_1c.locinfo)->lc_codepage,
                       (BOOL)(local_1c.locinfo)->lc_category[0].wlocale);
    if (BVar2 == 0) {
      if (local_10 != '\0') {
        *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
      }
      return 0;
    }
  }
  if (local_10 != '\0') {
    *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
  }
  return (uint)local_8[0] & _Type;
}


// 10014918 __ioinit

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __ioinit
   
   Library: Visual Studio 2005 Release */

int __cdecl __ioinit(void)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  DWORD DVar3;
  HANDLE pvVar4;
  int iVar5;
  UINT *pUVar6;
  int iVar7;
  int *piVar8;
  UINT UVar9;
  UINT UVar10;
  _STARTUPINFOA local_68;
  uint local_24;
  byte *local_20;
  undefined4 uStack_c;
  undefined4 local_8;
  
  uStack_c = 0x10014924;
  local_8 = 0;
  GetStartupInfoA(&local_68);
  local_8 = 0xfffffffe;
  puVar2 = (undefined4 *)FUN_100011f0(0x20,0x38);
  if (puVar2 == (undefined4 *)0x0) {
LAB_10014b4f:
    iVar7 = -1;
  }
  else {
    DAT_1004bbc4 = 0x20;
    DAT_1004bbe0 = puVar2;
    for (; puVar2 < DAT_1004bbe0 + 0x1c0; puVar2 = puVar2 + 0xe) {
      *(undefined1 *)(puVar2 + 1) = 0;
      *puVar2 = 0xffffffff;
      *(undefined1 *)((int)puVar2 + 5) = 10;
      puVar2[2] = 0;
      *(undefined1 *)(puVar2 + 9) = 0;
      *(undefined1 *)((int)puVar2 + 0x25) = 10;
      *(undefined1 *)((int)puVar2 + 0x26) = 10;
    }
    if ((local_68.cbReserved2 != 0) && ((UINT *)local_68.lpReserved2 != (UINT *)0x0)) {
      UVar9 = *(UINT *)local_68.lpReserved2;
      pUVar6 = (UINT *)((int)local_68.lpReserved2 + 4);
      local_20 = (byte *)((int)pUVar6 + UVar9);
      if (0x7ff < (int)UVar9) {
        UVar9 = 0x800;
      }
      iVar7 = 1;
      while ((UVar10 = UVar9, (int)DAT_1004bbc4 < (int)UVar9 &&
             (puVar2 = (undefined4 *)FUN_100011f0(0x20,0x38), UVar10 = DAT_1004bbc4,
             puVar2 != (undefined4 *)0x0))) {
        (&DAT_1004bbe0)[iVar7] = puVar2;
        DAT_1004bbc4 = DAT_1004bbc4 + 0x20;
        puVar1 = puVar2;
        for (; puVar2 < puVar1 + 0x1c0; puVar2 = puVar2 + 0xe) {
          *(undefined1 *)(puVar2 + 1) = 0;
          *puVar2 = 0xffffffff;
          *(undefined1 *)((int)puVar2 + 5) = 10;
          puVar2[2] = 0;
          *(byte *)(puVar2 + 9) = *(byte *)(puVar2 + 9) & 0x80;
          *(undefined1 *)((int)puVar2 + 0x25) = 10;
          *(undefined1 *)((int)puVar2 + 0x26) = 10;
          puVar1 = (&DAT_1004bbe0)[iVar7];
        }
        iVar7 = iVar7 + 1;
      }
      local_24 = 0;
      if (0 < (int)UVar10) {
        do {
          pvVar4 = *(HANDLE *)local_20;
          if ((((pvVar4 != (HANDLE)0xffffffff) && (pvVar4 != (HANDLE)0xfffffffe)) &&
              ((*pUVar6 & 1) != 0)) &&
             (((*pUVar6 & 8) != 0 || (DVar3 = GetFileType(pvVar4), DVar3 != 0)))) {
            puVar2 = (undefined4 *)
                     ((local_24 & 0x1f) * 0x38 + (int)(&DAT_1004bbe0)[(int)local_24 >> 5]);
            *puVar2 = *(undefined4 *)local_20;
            *(byte *)(puVar2 + 1) = (byte)*pUVar6;
            iVar7 = ___crtInitCritSecAndSpinCount(puVar2 + 3,4000);
            if (iVar7 == 0) goto LAB_10014b4f;
            puVar2[2] = puVar2[2] + 1;
          }
          local_24 = local_24 + 1;
          pUVar6 = (UINT *)((int)pUVar6 + 1);
          local_20 = local_20 + 4;
        } while ((int)local_24 < (int)UVar10);
      }
    }
    iVar7 = 0;
    do {
      piVar8 = DAT_1004bbe0 + iVar7 * 0xe;
      if ((*piVar8 == -1) || (*piVar8 == -2)) {
        *(undefined1 *)(piVar8 + 1) = 0x81;
        if (iVar7 == 0) {
          DVar3 = 0xfffffff6;
        }
        else {
          DVar3 = 0xfffffff5 - (iVar7 != 1);
        }
        pvVar4 = GetStdHandle(DVar3);
        if (((pvVar4 == (HANDLE)0xffffffff) || (pvVar4 == (HANDLE)0x0)) ||
           (DVar3 = GetFileType(pvVar4), DVar3 == 0)) {
          *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 0x40;
          *piVar8 = -2;
        }
        else {
          *piVar8 = (int)pvVar4;
          if ((DVar3 & 0xff) == 2) {
            *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 0x40;
          }
          else if ((DVar3 & 0xff) == 3) {
            *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 8;
          }
          iVar5 = ___crtInitCritSecAndSpinCount(piVar8 + 3,4000);
          if (iVar5 == 0) goto LAB_10014b4f;
          piVar8[2] = piVar8[2] + 1;
        }
      }
      else {
        *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 0x80;
      }
      iVar7 = iVar7 + 1;
    } while (iVar7 < 3);
    SetHandleCount(DAT_1004bbc4);
    iVar7 = 0;
  }
  return iVar7;
}


// 10014b58 __ioterm

/* Library Function - Single Match
    __ioterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __ioterm(void)

{
  uint uVar1;
  uint *puVar2;
  uint uVar3;
  
  puVar2 = &DAT_1004bbe0;
  do {
    uVar3 = *puVar2;
    uVar1 = uVar3;
    if (uVar3 != 0) {
      for (; uVar3 < uVar1 + 0x700; uVar3 = uVar3 + 0x38) {
        if (*(int *)(uVar3 + 8) != 0) {
          DeleteCriticalSection((LPCRITICAL_SECTION)(uVar3 + 0xc));
        }
        uVar1 = *puVar2;
      }
      FUN_100012d0(*puVar2);
      *puVar2 = 0;
    }
    puVar2 = puVar2 + 1;
  } while ((int)puVar2 < 0x1004bce0);
  return;
}


// 10014ba4 __setenvp

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __setenvp
   
   Library: Visual Studio 2005 Release */

int __cdecl __setenvp(void)

{
  undefined4 *puVar1;
  size_t sVar2;
  char *_Dst;
  errno_t eVar3;
  rsize_t _SizeInBytes;
  char *pcVar4;
  int iVar5;
  
  if (DAT_1004bcf0 == 0) {
    ___initmbctable();
  }
  iVar5 = 0;
  pcVar4 = DAT_10039e1c;
  if (DAT_10039e1c != (char *)0x0) {
    for (; *pcVar4 != '\0'; pcVar4 = pcVar4 + sVar2 + 1) {
      if (*pcVar4 != '=') {
        iVar5 = iVar5 + 1;
      }
      sVar2 = _strlen(pcVar4);
    }
    puVar1 = (undefined4 *)FUN_100011f0(iVar5 + 1,4);
    pcVar4 = DAT_10039e1c;
    DAT_1003a18c = puVar1;
    if (puVar1 != (undefined4 *)0x0) {
      do {
        if (*pcVar4 == '\0') {
          FUN_100012d0(DAT_10039e1c);
          DAT_10039e1c = (char *)0x0;
          *puVar1 = 0;
          _DAT_1004bce4 = 1;
          return 0;
        }
        sVar2 = _strlen(pcVar4);
        _SizeInBytes = sVar2 + 1;
        if (*pcVar4 != '=') {
          _Dst = (char *)FUN_100011f0(_SizeInBytes,1);
          *puVar1 = _Dst;
          if (_Dst == (char *)0x0) {
            FUN_100012d0(DAT_1003a18c);
            DAT_1003a18c = (undefined4 *)0x0;
            return -1;
          }
          eVar3 = _strcpy_s(_Dst,_SizeInBytes,pcVar4);
          if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
            __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
          }
          puVar1 = puVar1 + 1;
        }
        pcVar4 = pcVar4 + _SizeInBytes;
      } while( true );
    }
  }
  return -1;
}


// 10014c7f parse_cmdline

/* Library Function - Single Match
    _parse_cmdline
   
   Library: Visual Studio 2005 Release */

void __cdecl parse_cmdline(undefined4 *param_1,byte *param_2,int *param_3)

{
  bool bVar1;
  bool bVar2;
  int iVar3;
  uint uVar4;
  byte *in_EDX;
  byte *pbVar5;
  byte bVar6;
  byte *pbVar7;
  byte *pbVar8;
  int *unaff_EDI;
  
  *unaff_EDI = 0;
  *param_3 = 1;
  if (param_1 != (undefined4 *)0x0) {
    *param_1 = param_2;
    param_1 = param_1 + 1;
  }
  bVar2 = false;
  pbVar5 = param_2;
  do {
    if (*in_EDX == 0x22) {
      bVar2 = !bVar2;
      bVar6 = 0x22;
      pbVar7 = in_EDX + 1;
    }
    else {
      *unaff_EDI = *unaff_EDI + 1;
      if (pbVar5 != (byte *)0x0) {
        *pbVar5 = *in_EDX;
        param_2 = pbVar5 + 1;
      }
      bVar6 = *in_EDX;
      pbVar7 = in_EDX + 1;
      iVar3 = __ismbblead((uint)bVar6);
      if (iVar3 != 0) {
        *unaff_EDI = *unaff_EDI + 1;
        if (param_2 != (byte *)0x0) {
          *param_2 = *pbVar7;
          param_2 = param_2 + 1;
        }
        pbVar7 = in_EDX + 2;
      }
      pbVar5 = param_2;
      if (bVar6 == 0) {
        pbVar7 = pbVar7 + -1;
        goto LAB_10014d11;
      }
    }
    in_EDX = pbVar7;
  } while ((bVar2) || ((bVar6 != 0x20 && (bVar6 != 9))));
  if (pbVar5 != (byte *)0x0) {
    pbVar5[-1] = 0;
  }
LAB_10014d11:
  bVar2 = false;
  while (*pbVar7 != 0) {
    for (; (*pbVar7 == 0x20 || (*pbVar7 == 9)); pbVar7 = pbVar7 + 1) {
    }
    if (*pbVar7 == 0) break;
    if (param_1 != (undefined4 *)0x0) {
      *param_1 = pbVar5;
      param_1 = param_1 + 1;
    }
    *param_3 = *param_3 + 1;
    while( true ) {
      bVar1 = true;
      uVar4 = 0;
      for (; *pbVar7 == 0x5c; pbVar7 = pbVar7 + 1) {
        uVar4 = uVar4 + 1;
      }
      if (*pbVar7 == 0x22) {
        pbVar8 = pbVar7;
        if (((uVar4 & 1) == 0) && ((!bVar2 || (pbVar8 = pbVar7 + 1, *pbVar8 != 0x22)))) {
          bVar1 = false;
          bVar2 = !bVar2;
          pbVar8 = pbVar7;
        }
        uVar4 = uVar4 >> 1;
        pbVar7 = pbVar8;
      }
      while (uVar4 != 0) {
        uVar4 = uVar4 - 1;
        if (pbVar5 != (byte *)0x0) {
          *pbVar5 = 0x5c;
          pbVar5 = pbVar5 + 1;
        }
        *unaff_EDI = *unaff_EDI + 1;
        param_2 = pbVar5;
      }
      bVar6 = *pbVar7;
      if ((bVar6 == 0) || ((!bVar2 && ((bVar6 == 0x20 || (bVar6 == 9)))))) break;
      if (bVar1) {
        if (pbVar5 == (byte *)0x0) {
          iVar3 = __ismbblead((int)(char)bVar6);
          if (iVar3 != 0) {
            pbVar7 = pbVar7 + 1;
            *unaff_EDI = *unaff_EDI + 1;
          }
        }
        else {
          iVar3 = __ismbblead((int)(char)bVar6);
          if (iVar3 != 0) {
            *param_2 = *pbVar7;
            pbVar7 = pbVar7 + 1;
            *unaff_EDI = *unaff_EDI + 1;
            param_2 = param_2 + 1;
          }
          *param_2 = *pbVar7;
          param_2 = param_2 + 1;
        }
        *unaff_EDI = *unaff_EDI + 1;
        pbVar5 = param_2;
      }
      pbVar7 = pbVar7 + 1;
    }
    if (pbVar5 != (byte *)0x0) {
      *pbVar5 = 0;
      pbVar5 = pbVar5 + 1;
      param_2 = pbVar5;
    }
    *unaff_EDI = *unaff_EDI + 1;
  }
  if (param_1 != (undefined4 *)0x0) {
    *param_1 = 0;
  }
  *param_3 = *param_3 + 1;
  return;
}


// 10014e17 __setargv

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __setargv
   
   Library: Visual Studio 2005 Release */

int __cdecl __setargv(void)

{
  uint uVar1;
  uint uVar2;
  undefined4 *puVar3;
  uint local_10;
  uint local_c;
  char *local_8;
  
  if (DAT_1004bcf0 == 0) {
    ___initmbctable();
  }
  DAT_1003a2fc = 0;
  GetModuleFileNameA((HMODULE)0x0,&DAT_1003a1f8,0x104);
  _DAT_1003a19c = &DAT_1003a1f8;
  if ((DAT_1004bd00 == (char *)0x0) || (local_8 = DAT_1004bd00, *DAT_1004bd00 == '\0')) {
    local_8 = &DAT_1003a1f8;
  }
  parse_cmdline((undefined4 *)0x0,(byte *)0x0,(int *)&local_c);
  uVar2 = local_c;
  if ((local_c < 0x3fffffff) && (local_10 != 0xffffffff)) {
    uVar1 = local_c * 4 + local_10;
    if ((local_10 <= uVar1) &&
       (puVar3 = (undefined4 *)FUN_10001270(uVar1), puVar3 != (undefined4 *)0x0)) {
      parse_cmdline(puVar3,(byte *)(puVar3 + uVar2),(int *)&local_c);
      _DAT_1003a180 = local_c - 1;
      _DAT_1003a184 = puVar3;
      return 0;
    }
  }
  return -1;
}


// 10014ed0 ___crtGetEnvironmentStringsA

/* Library Function - Single Match
    ___crtGetEnvironmentStringsA
   
   Library: Visual Studio 2005 Release */

LPVOID __cdecl ___crtGetEnvironmentStringsA(void)

{
  char cVar1;
  WCHAR WVar2;
  DWORD DVar3;
  WCHAR *pWVar4;
  int iVar6;
  int cbMultiByte;
  LPSTR lpMultiByteStr;
  LPCH _Src;
  char *pcVar7;
  void *_Dst;
  LPWCH lpWideCharStr;
  LPSTR local_8;
  WCHAR *pWVar5;
  char *pcVar8;
  
  lpWideCharStr = (LPWCH)0x0;
  if (DAT_1003a300 == 0) {
    lpWideCharStr = GetEnvironmentStringsW();
    if (lpWideCharStr != (LPWCH)0x0) {
      DAT_1003a300 = 1;
      goto LAB_10014f22;
    }
    DVar3 = GetLastError();
    if (DVar3 == 0x78) {
      DAT_1003a300 = 2;
    }
  }
  if (DAT_1003a300 != 1) {
    if ((DAT_1003a300 != 2) && (DAT_1003a300 != 0)) {
      return (LPVOID)0x0;
    }
    _Src = GetEnvironmentStrings();
    if (_Src == (LPCH)0x0) {
      return (LPVOID)0x0;
    }
    cVar1 = *_Src;
    pcVar7 = _Src;
    while (cVar1 != '\0') {
      do {
        pcVar8 = pcVar7;
        pcVar7 = pcVar8 + 1;
      } while (*pcVar7 != '\0');
      pcVar7 = pcVar8 + 2;
      cVar1 = *pcVar7;
    }
    _Dst = (void *)FUN_10001270(pcVar7 + (1 - (int)_Src));
    if (_Dst == (void *)0x0) {
      FreeEnvironmentStringsA(_Src);
      return (LPVOID)0x0;
    }
    _memcpy(_Dst,_Src,(size_t)(pcVar7 + (1 - (int)_Src)));
    FreeEnvironmentStringsA(_Src);
    return _Dst;
  }
LAB_10014f22:
  if ((lpWideCharStr == (LPWCH)0x0) &&
     (lpWideCharStr = GetEnvironmentStringsW(), lpWideCharStr == (LPWCH)0x0)) {
    return (LPVOID)0x0;
  }
  WVar2 = *lpWideCharStr;
  pWVar4 = lpWideCharStr;
  while (WVar2 != L'\0') {
    do {
      pWVar5 = pWVar4;
      pWVar4 = pWVar5 + 1;
    } while (*pWVar4 != L'\0');
    pWVar4 = pWVar5 + 2;
    WVar2 = *pWVar4;
  }
  iVar6 = ((int)pWVar4 - (int)lpWideCharStr >> 1) + 1;
  cbMultiByte = WideCharToMultiByte(0,0,lpWideCharStr,iVar6,(LPSTR)0x0,0,(LPCSTR)0x0,(LPBOOL)0x0);
  local_8 = (LPSTR)0x0;
  if (((cbMultiByte != 0) &&
      (lpMultiByteStr = (LPSTR)FUN_10001270(cbMultiByte), lpMultiByteStr != (LPSTR)0x0)) &&
     (iVar6 = WideCharToMultiByte(0,0,lpWideCharStr,iVar6,lpMultiByteStr,cbMultiByte,(LPCSTR)0x0,
                                  (LPBOOL)0x0), local_8 = lpMultiByteStr, iVar6 == 0)) {
    FUN_100012d0(lpMultiByteStr);
    local_8 = (LPSTR)0x0;
  }
  FreeEnvironmentStringsW(lpWideCharStr);
  return local_8;
}


// 10015005 __RTC_Initialize

/* WARNING: Removing unreachable block (ram,0x10015017) */
/* WARNING: Removing unreachable block (ram,0x1001501d) */
/* WARNING: Removing unreachable block (ram,0x1001501f) */
/* Library Function - Single Match
    __RTC_Initialize
   
   Library: Visual Studio 2005 Release */

void __RTC_Initialize(void)

{
  return;
}


// 1001504d FUN_1001504d

DWORD * __cdecl FUN_1001504d(int param_1,DWORD param_2)

{
  int *piVar1;
  code *pcVar2;
  DWORD DVar3;
  DWORD DVar4;
  DWORD *pDVar5;
  int *piVar6;
  int iVar7;
  int iVar8;
  
  pDVar5 = FUN_100131da();
  if (pDVar5 != (DWORD *)0x0) {
    piVar1 = (int *)pDVar5[0x17];
    piVar6 = piVar1;
    do {
      if (*piVar6 == param_1) break;
      piVar6 = piVar6 + 3;
    } while (piVar6 < piVar1 + DAT_100366a4 * 3);
    if ((piVar1 + DAT_100366a4 * 3 <= piVar6) || (*piVar6 != param_1)) {
      piVar6 = (int *)0x0;
    }
    if ((piVar6 == (int *)0x0) || (pcVar2 = (code *)piVar6[2], pcVar2 == (code *)0x0)) {
      pDVar5 = (DWORD *)0x0;
    }
    else if (pcVar2 == (code *)0x5) {
      piVar6[2] = 0;
      pDVar5 = (DWORD *)0x1;
    }
    else {
      if (pcVar2 != (code *)0x1) {
        DVar3 = pDVar5[0x18];
        pDVar5[0x18] = param_2;
        if (piVar6[1] == 8) {
          if (DAT_10036698 < DAT_1003669c + DAT_10036698) {
            iVar7 = DAT_10036698 * 0xc;
            iVar8 = DAT_10036698;
            do {
              *(undefined4 *)(iVar7 + 8 + pDVar5[0x17]) = 0;
              iVar8 = iVar8 + 1;
              iVar7 = iVar7 + 0xc;
            } while (iVar8 < DAT_1003669c + DAT_10036698);
          }
          iVar8 = *piVar6;
          DVar4 = pDVar5[0x19];
          if (iVar8 == -0x3fffff72) {
            pDVar5[0x19] = 0x83;
          }
          else if (iVar8 == -0x3fffff70) {
            pDVar5[0x19] = 0x81;
          }
          else if (iVar8 == -0x3fffff6f) {
            pDVar5[0x19] = 0x84;
          }
          else if (iVar8 == -0x3fffff6d) {
            pDVar5[0x19] = 0x85;
          }
          else if (iVar8 == -0x3fffff73) {
            pDVar5[0x19] = 0x82;
          }
          else if (iVar8 == -0x3fffff71) {
            pDVar5[0x19] = 0x86;
          }
          else if (iVar8 == -0x3fffff6e) {
            pDVar5[0x19] = 0x8a;
          }
          (*pcVar2)(8,pDVar5[0x19]);
          pDVar5[0x19] = DVar4;
        }
        else {
          piVar6[2] = 0;
          (*pcVar2)(piVar6[1]);
        }
        pDVar5[0x18] = DVar3;
      }
      pDVar5 = (DWORD *)0xffffffff;
    }
  }
  return pDVar5;
}


// 100151c6 FUN_100151c6

undefined4 FUN_100151c6(void)

{
  return 1;
}


// 100151cc ___security_init_cookie

/* Library Function - Single Match
    ___security_init_cookie
   
   Library: Visual Studio 2005 Release */

void __cdecl ___security_init_cookie(void)

{
  DWORD DVar1;
  DWORD DVar2;
  DWORD DVar3;
  uint uVar4;
  LARGE_INTEGER local_14;
  _FILETIME local_c;
  
  local_c.dwLowDateTime = 0;
  local_c.dwHighDateTime = 0;
  if ((DAT_100366b8 == 0xbb40e64e) || ((DAT_100366b8 & 0xffff0000) == 0)) {
    GetSystemTimeAsFileTime(&local_c);
    uVar4 = local_c.dwHighDateTime ^ local_c.dwLowDateTime;
    DVar1 = GetCurrentProcessId();
    DVar2 = GetCurrentThreadId();
    DVar3 = GetTickCount();
    QueryPerformanceCounter(&local_14);
    DAT_100366b8 = uVar4 ^ DVar1 ^ DVar2 ^ DVar3 ^ local_14.s.HighPart ^ local_14.s.LowPart;
    if (DAT_100366b8 == 0xbb40e64e) {
      DAT_100366b8 = 0xbb40e64f;
    }
    else if ((DAT_100366b8 & 0xffff0000) == 0) {
      DAT_100366b8 = DAT_100366b8 | DAT_100366b8 << 0x10;
    }
    DAT_100366bc = ~DAT_100366b8;
  }
  else {
    DAT_100366bc = ~DAT_100366b8;
  }
  return;
}


// 10015260 write_char

/* Library Function - Single Match
    _write_char
   
   Library: Visual Studio 2005 Release */

void __cdecl write_char(wchar_t param_1)

{
  wint_t wVar1;
  FILE *in_EAX;
  int *unaff_ESI;
  
  if (((in_EAX->_flag & 0x40) == 0) || (in_EAX->_base != (char *)0x0)) {
    wVar1 = __fputwc_nolock(param_1,in_EAX);
    if (wVar1 == 0xffff) {
      *unaff_ESI = -1;
      return;
    }
  }
  *unaff_ESI = *unaff_ESI + 1;
  return;
}


// 10015285 write_multi_char

/* Library Function - Single Match
    _write_multi_char
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release */

void __cdecl write_multi_char(wchar_t param_1,int param_2)

{
  int *in_EAX;
  
  do {
    if (param_2 < 1) {
      return;
    }
    param_2 = param_2 + -1;
    write_char(param_1);
  } while (*in_EAX != -1);
  return;
}


// 100152aa write_string

/* Library Function - Single Match
    _write_string
   
   Library: Visual Studio 2005 Release */

void __cdecl write_string(int param_1)

{
  int *in_EAX;
  DWORD *pDVar1;
  wchar_t *in_ECX;
  int unaff_EDI;
  
  if (((*(byte *)(unaff_EDI + 0xc) & 0x40) == 0) || (*(int *)(unaff_EDI + 8) != 0)) {
    while (0 < param_1) {
      param_1 = param_1 + -1;
      write_char(*in_ECX);
      in_ECX = in_ECX + 1;
      if (*in_EAX == -1) {
        pDVar1 = FUN_10011e63();
        if (*pDVar1 != 0x2a) {
          return;
        }
        write_char(L'?');
      }
    }
  }
  else {
    *in_EAX = *in_EAX + param_1;
  }
  return;
}


// 10015c09 wcstoxl

/* Library Function - Single Match
    unsigned long __cdecl wcstoxl(struct localeinfo_struct *,wchar_t const *,wchar_t const *
   *,int,int)
   
   Library: Visual Studio 2005 Release */

ulong __cdecl
wcstoxl(localeinfo_struct *param_1,wchar_t *param_2,wchar_t **param_3,int param_4,int param_5)

{
  wchar_t _C;
  wchar_t *pwVar1;
  DWORD *pDVar2;
  int iVar3;
  uint uVar4;
  uint uVar5;
  uint uVar6;
  wchar_t *pwVar7;
  ushort uVar8;
  localeinfo_struct local_1c;
  int local_14;
  char local_10;
  uint local_c;
  ulong uVar9;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_1c,param_1);
  if (param_3 != (wchar_t **)0x0) {
    *param_3 = param_2;
  }
  if ((param_2 == (wchar_t *)0x0) || ((param_4 != 0 && ((param_4 < 2 || (0x24 < param_4)))))) {
    pDVar2 = FUN_10011e63();
    *pDVar2 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_10 != '\0') {
      *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
    }
    return 0;
  }
  _C = *param_2;
  uVar9 = 0;
  pwVar1 = param_2;
  while( true ) {
    pwVar7 = pwVar1 + 1;
    iVar3 = __iswctype_l(_C,8,&local_1c);
    if (iVar3 == 0) break;
    _C = *pwVar7;
    pwVar1 = pwVar7;
  }
  if (_C == L'-') {
    param_5 = param_5 | 2;
LAB_10015ca0:
    _C = *pwVar7;
    pwVar7 = pwVar1 + 2;
  }
  else if (_C == L'+') goto LAB_10015ca0;
  uVar6 = (uint)(ushort)_C;
  if (param_4 == 0) {
    iVar3 = __wchartodigit(_C);
    if (iVar3 != 0) {
      param_4 = 10;
      goto LAB_10015d04;
    }
    if ((*pwVar7 != L'x') && (*pwVar7 != L'X')) {
      param_4 = 8;
      goto LAB_10015d04;
    }
    param_4 = 0x10;
  }
  if (((param_4 == 0x10) && (iVar3 = __wchartodigit(_C), iVar3 == 0)) &&
     ((*pwVar7 == L'x' || (*pwVar7 == L'X')))) {
    uVar6 = (uint)(ushort)pwVar7[1];
    pwVar7 = pwVar7 + 2;
  }
LAB_10015d04:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  local_c = (uint)(0xffffffff % (ulonglong)(uint)param_4);
  do {
    uVar8 = (ushort)uVar6;
    uVar5 = __wchartodigit(uVar8);
    if (uVar5 == 0xffffffff) {
      if (((uVar8 < 0x41) || (0x5a < uVar8)) && (0x19 < (ushort)(uVar8 - 0x61))) {
LAB_10015d63:
        pwVar7 = pwVar7 + -1;
        if ((param_5 & 8U) == 0) {
          if (param_3 != (wchar_t **)0x0) {
            pwVar7 = param_2;
          }
          uVar9 = 0;
        }
        else if (((param_5 & 4U) != 0) ||
                (((param_5 & 1U) == 0 &&
                 ((((param_5 & 2U) != 0 && (0x80000000 < uVar9)) ||
                  (((param_5 & 2U) == 0 && (0x7fffffff < uVar9)))))))) {
          pDVar2 = FUN_10011e63();
          *pDVar2 = 0x22;
          if ((param_5 & 1U) == 0) {
            uVar9 = ((param_5 & 2U) != 0) + 0x7fffffff;
          }
          else {
            uVar9 = 0xffffffff;
          }
        }
        if (param_3 != (wchar_t **)0x0) {
          *param_3 = pwVar7;
        }
        if ((param_5 & 2U) != 0) {
          uVar9 = -uVar9;
        }
        if (local_10 != '\0') {
          *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
        }
        return uVar9;
      }
      if ((ushort)(uVar8 - 0x61) < 0x1a) {
        uVar6 = uVar6 - 0x20;
      }
      uVar5 = uVar6 - 0x37;
    }
    if ((uint)param_4 <= uVar5) goto LAB_10015d63;
    if ((uVar9 < uVar4) || ((uVar9 == uVar4 && (uVar5 <= local_c)))) {
      uVar9 = uVar9 * param_4 + uVar5;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (wchar_t **)0x0) goto LAB_10015d63;
    }
    uVar6 = (uint)(ushort)*pwVar7;
    pwVar7 = pwVar7 + 1;
  } while( true );
}


// 10015e00 _wcstol

/* Library Function - Single Match
    _wcstol
   
   Library: Visual Studio 2005 Release */

long __cdecl _wcstol(wchar_t *_Str,wchar_t **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_1003a1cc == 0) {
    ppuVar2 = &PTR_DAT_100365e0;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = wcstoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 10015e29 FUN_10015e29

int __cdecl FUN_10015e29(short *param_1)

{
  short sVar1;
  short *psVar2;
  
  psVar2 = param_1;
  do {
    sVar1 = *psVar2;
    psVar2 = psVar2 + 1;
  } while (sVar1 != 0);
  return ((int)psVar2 - (int)param_1 >> 1) + -1;
}


// 10016bb0 _strlen

/* Library Function - Single Match
    _strlen
   
   Library: Visual Studio */

size_t __cdecl _strlen(char *_Str)

{
  uint uVar1;
  uint *puVar2;
  uint *puVar3;
  
  uVar1 = (uint)_Str & 3;
  puVar2 = (uint *)_Str;
  while (uVar1 != 0) {
    uVar1 = *puVar2;
    puVar2 = (uint *)((int)puVar2 + 1);
    if ((char)uVar1 == '\0') goto LAB_10016c13;
    uVar1 = (uint)puVar2 & 3;
  }
  do {
    do {
      puVar3 = puVar2;
      puVar2 = puVar3 + 1;
    } while (((*puVar3 ^ 0xffffffff ^ *puVar3 + 0x7efefeff) & 0x81010100) == 0);
    uVar1 = *puVar3;
    if ((char)uVar1 == '\0') {
      return (int)puVar3 - (int)_Str;
    }
    if ((char)(uVar1 >> 8) == '\0') {
      return (size_t)((int)puVar3 + (1 - (int)_Str));
    }
    if ((uVar1 & 0xff0000) == 0) {
      return (size_t)((int)puVar3 + (2 - (int)_Str));
    }
  } while ((uVar1 & 0xff000000) != 0);
LAB_10016c13:
  return (size_t)((int)puVar2 + (-1 - (int)_Str));
}


// 10017917 __wfltin2

/* Library Function - Single Match
    __wfltin2
   
   Library: Visual Studio 2005 Release */

FLT __cdecl __wfltin2(FLT _Fit,wchar_t *_Str,_locale_t _Locale)

{
  INTRNCVT_STATUS IVar1;
  FLT p_Var2;
  uint uVar3;
  int *in_stack_00000018;
  int local_28;
  wchar_t *local_24;
  _CRT_DOUBLE local_20;
  uint local_18;
  _LDBL12 local_14;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  local_24 = _Str;
  uVar3 = 0;
  local_18 = FUN_10017aae((undefined2 *)&local_14,&local_28,(ushort *)_Str,0,0,0,0,in_stack_00000018
                         );
  if ((local_18 & 4) == 0) {
    IVar1 = __ld12tod(&local_14,&local_20);
    if (((local_18 & 2) != 0) || (IVar1 == INTRNCVT_OVERFLOW)) {
      uVar3 = 0x80;
    }
    if (((local_18 & 1) != 0) || (IVar1 == INTRNCVT_UNDERFLOW)) {
      uVar3 = uVar3 | 0x100;
    }
  }
  else {
    uVar3 = 0x200;
    local_20.x._0_4_ = 0;
    local_20.x._4_4_ = 0;
  }
  _Fit->nbytes = local_28 - (int)local_24 >> 1;
  *(undefined4 *)&_Fit->dval = local_20.x._0_4_;
  *(undefined4 *)((int)&_Fit->dval + 4) = local_20.x._4_4_;
  _Fit->flags = uVar3;
  p_Var2 = (FLT)__security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return p_Var2;
}


// 100179be __iswctype_l

/* Library Function - Single Match
    __iswctype_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __iswctype_l(wint_t _C,wctype_t _Type,_locale_t _Locale)

{
  int iVar1;
  localeinfo_struct local_18;
  int local_10;
  char local_c;
  ushort local_8 [2];
  
  if (_C == 0xffff) {
    local_8[0] = 0;
  }
  else if (_C < 0x100) {
    local_8[0] = *(ushort *)(PTR_DAT_10036a74 + (uint)_C * 2) & _Type;
  }
  else {
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_18,_Locale);
    iVar1 = ___crtGetStringTypeW
                      (&local_18,1,(wchar_t *)&_C,1,local_8,(local_18.locinfo)->lc_codepage,
                       (int)(local_18.locinfo)->lc_category[0].wlocale);
    if (iVar1 == 0) {
      local_8[0] = 0;
    }
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
    }
  }
  return (uint)(local_8[0] & _Type);
}


// 10017a40 _iswctype

/* Library Function - Single Match
    _iswctype
   
   Library: Visual Studio 2005 Release */

int __cdecl _iswctype(wint_t _C,wctype_t _Type)

{
  int iVar1;
  undefined2 in_stack_00000006;
  undefined2 in_stack_0000000a;
  ushort local_8 [2];
  
  if (_C == 0xffff) {
    return 0;
  }
  if (_C < 0x100) {
    return (uint)(*(ushort *)(PTR_DAT_10036a74 + (uint)_C * 2) & _Type);
  }
  if (DAT_1003a1cc == 0) {
    ___crtGetStringTypeW
              ((localeinfo_struct *)&PTR_DAT_100365e0,1,(wchar_t *)&_C,1,local_8,DAT_10036504,
               DAT_10036514);
  }
  iVar1 = __iswctype_l((wint_t)__C,(wctype_t)__Type,(_locale_t)0x0);
  return iVar1;
}


// 10017aae FUN_10017aae

void __cdecl
FUN_10017aae(undefined2 *param_1,int *param_2,ushort *param_3,int param_4,int param_5,int param_6,
            int param_7,int *param_8)

{
  int iVar1;
  uint uVar2;
  int iVar3;
  bool bVar4;
  bool bVar5;
  bool bVar6;
  ushort uVar7;
  ushort uVar8;
  DWORD *pDVar9;
  uint uVar10;
  uint uVar11;
  int iVar12;
  ushort uVar13;
  int iVar14;
  uint uVar15;
  ushort uVar16;
  ushort *puVar17;
  undefined4 uVar18;
  undefined **ppuVar19;
  undefined **ppuVar20;
  short *psVar21;
  undefined4 uVar22;
  ushort uVar23;
  bool bVar24;
  ushort *local_70;
  int local_6c;
  int local_68;
  undefined **local_64;
  int local_60;
  char *local_5c;
  ushort *local_58;
  int local_54;
  uint local_50;
  undefined2 local_4c;
  undefined4 uStack_4a;
  undefined2 uStack_46;
  undefined *local_44;
  undefined4 local_40;
  undefined4 local_3c;
  ushort uStack_38;
  ushort local_36;
  byte local_30;
  undefined1 uStack_2f;
  undefined4 uStack_2e;
  undefined4 uStack_2a;
  ushort uStack_26;
  char local_24 [23];
  char local_d;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  local_5c = local_24;
  iVar14 = 0;
  uVar7 = 0;
  local_68 = 1;
  local_50 = 0;
  bVar4 = false;
  bVar6 = false;
  bVar5 = false;
  local_6c = 0;
  local_54 = 0;
  if (param_8 != (int *)0x0) {
    local_58 = param_3;
    for (; (((uVar23 = *param_3, uVar23 == 0x20 || (uVar23 == 9)) || (uVar23 == 10)) ||
           (uVar23 == 0xd)); param_3 = param_3 + 1) {
    }
LAB_10017b44:
    uVar23 = *param_3;
    uVar10 = (uint)uVar23;
    puVar17 = param_3 + 1;
    switch(iVar14) {
    case 0:
      if ((ushort)(uVar23 - 0x31) < 9) {
LAB_10017b62:
        iVar14 = 3;
        goto LAB_10017b64;
      }
      if (uVar10 == (int)*(char *)**(undefined4 **)(*param_8 + 0xbc)) {
LAB_10017b80:
        iVar14 = 5;
        param_3 = puVar17;
      }
      else {
        iVar14 = 2;
        if (uVar10 == 0x2b) {
          uVar7 = 0;
          param_3 = puVar17;
        }
        else {
          if (uVar10 != 0x2d) {
            if (uVar10 == 0x30) goto LAB_10017b97;
            goto LAB_10017d5c;
          }
          uVar7 = 0x8000;
          param_3 = puVar17;
        }
      }
      goto LAB_10017b44;
    case 1:
      bVar4 = true;
      if ((ushort)(uVar23 - 0x31) < 9) goto LAB_10017b62;
      if (uVar10 == (int)*(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10017bd1;
      if ((uVar10 == 0x2b) || (uVar10 == 0x2d)) goto LAB_10017c00;
      if (uVar10 == 0x30) goto LAB_10017b97;
      goto LAB_10017be3;
    case 2:
      if ((ushort)(uVar23 - 0x31) < 9) goto LAB_10017b62;
      if (uVar10 == (int)*(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10017b80;
      param_3 = local_58;
      if (uVar10 != 0x30) goto LAB_10017d88;
LAB_10017b97:
      iVar14 = 1;
      param_3 = puVar17;
      goto LAB_10017b44;
    case 3:
      while ((0x2f < (ushort)uVar10 && ((ushort)uVar10 < 0x3a))) {
        if (local_50 < 0x19) {
          local_50 = local_50 + 1;
          *local_5c = (char)uVar10 + -0x30;
          local_5c = local_5c + 1;
        }
        else {
          local_54 = local_54 + 1;
        }
        uVar23 = *puVar17;
        puVar17 = puVar17 + 1;
        uVar10 = (uint)uVar23;
      }
      if (uVar10 != (int)*(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10017c89;
LAB_10017bd1:
      bVar4 = true;
      iVar14 = 4;
      param_3 = puVar17;
      goto LAB_10017b44;
    case 4:
      bVar6 = true;
      if (local_50 == 0) {
        while ((short)uVar10 == 0x30) {
          local_54 = local_54 + -1;
          uVar23 = *puVar17;
          puVar17 = puVar17 + 1;
          uVar10 = (uint)uVar23;
        }
      }
      while ((0x2f < (ushort)uVar10 && ((ushort)uVar10 < 0x3a))) {
        if (local_50 < 0x19) {
          local_50 = local_50 + 1;
          local_54 = local_54 + -1;
          *local_5c = (char)uVar10 + -0x30;
          local_5c = local_5c + 1;
        }
        uVar23 = *puVar17;
        puVar17 = puVar17 + 1;
        uVar10 = (uint)uVar23;
      }
LAB_10017c89:
      if ((uVar10 == 0x2b) || (uVar10 == 0x2d)) {
LAB_10017c00:
        bVar4 = true;
        iVar14 = 0xb;
        param_3 = puVar17 + -1;
      }
      else {
LAB_10017be3:
        bVar4 = true;
        if ((uVar10 < 0x44) || ((0x45 < uVar10 && (1 < uVar10 - 100)))) goto LAB_10017d5c;
        iVar14 = 6;
        param_3 = puVar17;
      }
      goto LAB_10017b44;
    case 5:
      bVar6 = true;
      param_3 = local_58;
      if ((ushort)(uVar23 - 0x30) < 10) {
        iVar14 = 4;
        goto LAB_10017b64;
      }
      goto LAB_10017d88;
    case 6:
      local_58 = param_3 + -1;
      if (8 < (ushort)(uVar23 - 0x31)) {
        if (uVar10 == 0x2b) goto LAB_10017d3d;
        if (uVar10 == 0x2d) goto LAB_10017d31;
        bVar24 = uVar10 == 0x30;
LAB_10017d24:
        param_3 = local_58;
        if (!bVar24) goto LAB_10017d88;
        iVar14 = 8;
        param_3 = puVar17;
        goto LAB_10017b44;
      }
      break;
    case 7:
      if (8 < (ushort)(uVar23 - 0x31)) {
        bVar24 = uVar23 == 0x30;
        goto LAB_10017d24;
      }
      break;
    case 8:
      bVar5 = true;
      while ((short)uVar10 == 0x30) {
        uVar23 = *puVar17;
        puVar17 = puVar17 + 1;
        uVar10 = (uint)uVar23;
      }
      if (8 < (ushort)((short)uVar10 - 0x31U)) goto LAB_10017d5c;
      break;
    case 9:
      bVar5 = true;
      local_6c = 0;
      goto LAB_10017de8;
    default:
      goto switchD_10017b52_caseD_a;
    case 0xb:
      if (param_7 != 0) {
        local_58 = param_3;
        if (uVar10 == 0x2b) {
LAB_10017d3d:
          iVar14 = 7;
          param_3 = puVar17;
        }
        else {
          if (uVar10 != 0x2d) goto LAB_10017d88;
LAB_10017d31:
          local_68 = -1;
          iVar14 = 7;
          param_3 = puVar17;
        }
        goto LAB_10017b44;
      }
      iVar14 = 10;
      puVar17 = param_3;
switchD_10017b52_caseD_a:
      param_3 = puVar17;
      if (iVar14 != 10) goto LAB_10017b44;
      goto LAB_10017d88;
    }
    iVar14 = 9;
LAB_10017b64:
    param_3 = puVar17 + -1;
    goto LAB_10017b44;
  }
  pDVar9 = FUN_10011e63();
  *pDVar9 = 0x16;
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
  goto LAB_10018194;
LAB_10017de8:
  if (((ushort)uVar10 < 0x30) || (0x39 < (ushort)uVar10)) goto LAB_10017e08;
  local_6c = local_6c * 10 + -0x30 + uVar10;
  if (local_6c < 0x1451) {
    uVar10 = (uint)*puVar17;
    puVar17 = puVar17 + 1;
    goto LAB_10017de8;
  }
  local_6c = 0x1451;
LAB_10017e08:
  while ((0x2f < (ushort)uVar10 && ((ushort)uVar10 < 0x3a))) {
    uVar23 = *puVar17;
    puVar17 = puVar17 + 1;
    uVar10 = (uint)uVar23;
  }
LAB_10017d5c:
  param_3 = puVar17 + -1;
LAB_10017d88:
  *param_2 = (int)param_3;
  if (bVar4) {
    if (0x18 < local_50) {
      if ('\x04' < local_d) {
        local_d = local_d + '\x01';
      }
      local_50 = 0x18;
      local_5c = local_5c + -1;
      local_54 = local_54 + 1;
    }
    if (local_50 == 0) goto LAB_10018175;
    while (local_5c = local_5c + -1, *local_5c == '\0') {
      local_50 = local_50 - 1;
      local_54 = local_54 + 1;
    }
    ___mtold12(local_24,local_50,&local_40);
    iVar12 = CONCAT22(local_3c._2_2_,(undefined2)local_3c);
    uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
    iVar1 = CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a);
    uVar10 = CONCAT22(uStack_2e._2_2_,(ushort)uStack_2e);
    iVar14 = CONCAT22(uStack_4a._2_2_,(undefined2)uStack_4a);
    if (local_68 < 0) {
      local_6c = -local_6c;
    }
    local_58 = (ushort *)(local_6c + local_54);
    if (!bVar5) {
      local_58 = (ushort *)((int)local_58 + param_5);
    }
    if (!bVar6) {
      local_58 = (ushort *)((int)local_58 - param_6);
    }
    if ((int)local_58 < 0x1451) {
      if ((int)local_58 < -0x1450) goto LAB_10018175;
      ppuVar20 = &PTR_DAT_10036b50;
      if (local_58 != (ushort *)0x0) {
        if ((int)local_58 < 0) {
          local_58 = (ushort *)-(int)local_58;
          ppuVar20 = (undefined **)&DAT_10036cb0;
        }
        if (param_4 == 0) {
          local_40._0_2_ = 0;
        }
        iVar14 = uStack_4a;
        uVar10 = uStack_2e;
        iVar1 = uStack_2a;
        uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
        iVar12 = local_3c;
joined_r0x10017ea3:
        if (local_58 != (ushort *)0x0) {
          uVar15 = (int)local_58 >> 3;
          ppuVar20 = ppuVar20 + 0x15;
          uVar11 = (uint)local_58 & 7;
          local_58 = (ushort *)uVar15;
          if (uVar11 != 0) {
            ppuVar19 = ppuVar20 + uVar11 * 3;
            if (0x7fff < *(ushort *)ppuVar19) {
              local_4c = SUB42(*ppuVar19,0);
              uStack_4a._0_2_ = (undefined2)((uint)*ppuVar19 >> 0x10);
              uStack_4a._2_2_ = SUB42(ppuVar19[1],0);
              uStack_46 = (undefined2)((uint)ppuVar19[1] >> 0x10);
              local_44 = ppuVar19[2];
              iVar14 = CONCAT22(uStack_4a._2_2_,(undefined2)uStack_4a) + -1;
              uStack_4a._0_2_ = (undefined2)iVar14;
              uStack_4a._2_2_ = (undefined2)((uint)iVar14 >> 0x10);
              ppuVar19 = (undefined **)&local_4c;
            }
            local_5c = (char *)0x0;
            local_30 = 0;
            uStack_2f = 0;
            uStack_2e._0_2_ = 0;
            uStack_2e._2_2_ = 0;
            uVar15 = 0;
            uStack_2a._0_2_ = 0;
            uStack_2a._2_2_ = 0;
            iVar3 = 0;
            uStack_26 = 0;
            uVar13 = local_36 & 0x7fff;
            uVar16 = *(ushort *)((int)ppuVar19 + 10) & 0x7fff;
            uVar8 = (*(ushort *)((int)ppuVar19 + 10) ^ local_36) & 0x8000;
            uVar23 = uVar16 + uVar13;
            uVar10 = 0;
            iVar1 = 0;
            if (((uVar13 < 0x7fff) && (uVar10 = 0, iVar1 = 0, uVar16 < 0x7fff)) &&
               (uVar10 = uVar15, iVar1 = iVar3, uVar23 < 0xbffe)) {
              if (0x3fbf < uVar23) {
                if (((uVar13 == 0) &&
                    (uVar23 = uVar23 + 1, (CONCAT22(local_36,uStack_38) & 0x7fffffff) == 0)) &&
                   ((iVar12 == 0 && (uVar2 == 0)))) {
                  local_36 = 0;
                }
                else if (((uVar16 == 0) &&
                         (uVar23 = uVar23 + 1, ((uint)ppuVar19[2] & 0x7fffffff) == 0)) &&
                        ((ppuVar19[1] == (undefined *)0x0 && (*ppuVar19 == (undefined *)0x0)))) {
                  uStack_38 = 0;
                  local_36 = 0;
                  local_3c._0_2_ = 0;
                  local_3c._2_2_ = 0;
                  local_40._0_2_ = 0;
                  local_40._2_2_ = 0;
                  uVar2 = 0;
                  iVar12 = 0;
                }
                else {
                  local_68 = 0;
                  psVar21 = (short *)((int)&uStack_2e + 2);
                  local_60 = 5;
                  do {
                    local_6c = local_60;
                    if (0 < local_60) {
                      local_70 = (ushort *)((int)&local_40 + local_68 * 2);
                      local_64 = ppuVar19 + 2;
                      do {
                        bVar4 = false;
                        uVar10 = *(uint *)(psVar21 + -2) +
                                 (uint)*local_70 * (uint)*(ushort *)local_64;
                        if ((uVar10 < *(uint *)(psVar21 + -2)) ||
                           (uVar10 < (uint)*local_70 * (uint)*(ushort *)local_64)) {
                          bVar4 = true;
                        }
                        *(uint *)(psVar21 + -2) = uVar10;
                        if (bVar4) {
                          *psVar21 = *psVar21 + 1;
                        }
                        local_70 = local_70 + 1;
                        local_64 = (undefined **)((int)local_64 + -2);
                        local_6c = local_6c + -1;
                      } while (0 < local_6c);
                    }
                    psVar21 = psVar21 + 1;
                    local_68 = local_68 + 1;
                    local_60 = local_60 + -1;
                  } while (0 < local_60);
                  uVar23 = uVar23 + 0xc002;
                  if ((short)uVar23 < 1) {
LAB_10018059:
                    uVar23 = uVar23 - 1;
                    if ((short)uVar23 < 0) {
                      uVar10 = (uint)(ushort)-uVar23;
                      uVar23 = 0;
                      do {
                        if ((local_30 & 1) != 0) {
                          local_5c = (char *)((int)local_5c + 1);
                        }
                        iVar12 = CONCAT22(uStack_26,uStack_2a._2_2_);
                        uVar2 = CONCAT22((ushort)uStack_2a,uStack_2e._2_2_);
                        iVar1 = CONCAT22((ushort)uStack_2a,uStack_2e._2_2_);
                        uStack_2a._2_2_ = (ushort)(CONCAT22(uStack_26,uStack_2a._2_2_) >> 1);
                        uStack_26 = uStack_26 >> 1;
                        uStack_2a._0_2_ =
                             (ushort)uStack_2a >> 1 | (ushort)((uint)(iVar12 << 0x1f) >> 0x10);
                        uVar15 = CONCAT22((ushort)uStack_2e,CONCAT11(uStack_2f,local_30)) >> 1;
                        uStack_2e._0_2_ =
                             (ushort)uStack_2e >> 1 | (ushort)((uint)(iVar1 << 0x1f) >> 0x10);
                        uVar10 = uVar10 - 1;
                        uStack_2e._2_2_ = (ushort)(uVar2 >> 1);
                        local_30 = (byte)uVar15;
                        uStack_2f = (undefined1)(uVar15 >> 8);
                      } while (uVar10 != 0);
                      if (local_5c != (char *)0x0) {
                        local_30 = local_30 | 1;
                      }
                    }
                  }
                  else {
                    do {
                      uVar13 = (ushort)uStack_2e;
                      if ((uStack_26 & 0x8000) != 0) break;
                      iVar1 = CONCAT22((ushort)uStack_2e,CONCAT11(uStack_2f,local_30)) << 1;
                      local_30 = (byte)iVar1;
                      uStack_2f = (undefined1)((uint)iVar1 >> 8);
                      uStack_2e._0_2_ = (ushort)((uint)iVar1 >> 0x10);
                      iVar1 = CONCAT22((ushort)uStack_2a,uStack_2e._2_2_) * 2;
                      uStack_2e._2_2_ = (ushort)iVar1 | uVar13 >> 0xf;
                      iVar12 = CONCAT22(uStack_26,uStack_2a._2_2_) * 2;
                      uStack_2a._2_2_ = (ushort)iVar12 | (ushort)uStack_2a >> 0xf;
                      uVar23 = uVar23 - 1;
                      uStack_2a._0_2_ = (ushort)((uint)iVar1 >> 0x10);
                      uStack_26 = (ushort)((uint)iVar12 >> 0x10);
                    } while (0 < (short)uVar23);
                    if ((short)uVar23 < 1) goto LAB_10018059;
                  }
                  if ((0x8000 < CONCAT11(uStack_2f,local_30)) ||
                     (iVar1 = CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a),
                     uVar10 = CONCAT22(uStack_2e._2_2_,(ushort)uStack_2e),
                     (CONCAT22((ushort)uStack_2e,CONCAT11(uStack_2f,local_30)) & 0x1ffff) == 0x18000
                     )) {
                    if (CONCAT22(uStack_2e._2_2_,(ushort)uStack_2e) == -1) {
                      uStack_2e._0_2_ = 0;
                      uStack_2e._2_2_ = 0;
                      uVar10 = 0;
                      if (CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a) == -1) {
                        uStack_2a._0_2_ = 0;
                        uStack_2a._2_2_ = 0;
                        if (uStack_26 == 0xffff) {
                          uStack_26 = 0x8000;
                          uVar23 = uVar23 + 1;
                          iVar1 = 0;
                          uVar10 = 0;
                        }
                        else {
                          uStack_26 = uStack_26 + 1;
                          iVar1 = 0;
                          uVar10 = 0;
                        }
                      }
                      else {
                        iVar1 = CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a) + 1;
                        uStack_2a._0_2_ = (ushort)iVar1;
                        uStack_2a._2_2_ = (ushort)((uint)iVar1 >> 0x10);
                      }
                    }
                    else {
                      uVar10 = CONCAT22(uStack_2e._2_2_,(ushort)uStack_2e) + 1;
                      uStack_2e._0_2_ = (ushort)uVar10;
                      uStack_2e._2_2_ = (ushort)(uVar10 >> 0x10);
                      iVar1 = CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a);
                    }
                  }
                  if (0x7ffe < uVar23) goto LAB_10018118;
                  local_40 = uVar10;
                  local_3c = iVar1;
                  uStack_38 = uStack_26;
                  local_36 = uVar23 | uVar8;
                  uVar2 = uVar10;
                  iVar12 = iVar1;
                }
                goto joined_r0x10017ea3;
              }
              iVar12 = 0;
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            else {
LAB_10018118:
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              iVar12 = (-(uint)(uVar8 != 0) & 0x80000000) + 0x7fff8000;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            uStack_38 = (ushort)iVar12;
            local_36 = (ushort)((uint)iVar12 >> 0x10);
            uVar2 = 0;
            iVar12 = 0;
          }
          goto joined_r0x10017ea3;
        }
      }
      local_3c._2_2_ = (undefined2)((uint)iVar12 >> 0x10);
      local_3c._0_2_ = (undefined2)iVar12;
      local_40._2_2_ = (undefined2)(uVar2 >> 0x10);
      local_40._0_2_ = (undefined2)uVar2;
      uVar22 = CONCAT22((undefined2)local_3c,local_40._2_2_);
      uVar18 = CONCAT22(uStack_38,local_3c._2_2_);
      uVar23 = local_36;
      uStack_4a = iVar14;
      uStack_2e = uVar10;
      uStack_2a = iVar1;
      local_3c = iVar12;
    }
    else {
      uVar22 = 0;
      uVar23 = 0x7fff;
      uVar18 = 0x80000000;
      local_40._0_2_ = 0;
    }
  }
  else {
LAB_10018175:
    uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
    local_40._0_2_ = 0;
    uVar23 = 0;
    uVar18 = 0;
    uVar22 = 0;
  }
  *param_1 = (undefined2)local_40;
  param_1[5] = uVar23 | uVar7;
  *(undefined4 *)(param_1 + 1) = uVar22;
  *(undefined4 *)(param_1 + 3) = uVar18;
LAB_10018194:
  local_40 = uVar2;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 100181d2 __fltin2

/* Library Function - Single Match
    __fltin2
   
   Library: Visual Studio 2005 Release */

FLT __cdecl __fltin2(FLT _Flt,char *_Str,_locale_t _Locale)

{
  INTRNCVT_STATUS IVar1;
  FLT p_Var2;
  uint uVar3;
  int *in_stack_00000018;
  int local_28;
  char *local_24;
  _CRT_DOUBLE local_20;
  uint local_18;
  _LDBL12 local_14;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  local_24 = _Str;
  uVar3 = 0;
  local_18 = FUN_10018277((undefined2 *)&local_14,&local_28,_Str,0,0,0,0,in_stack_00000018);
  if ((local_18 & 4) == 0) {
    IVar1 = __ld12tod(&local_14,&local_20);
    if (((local_18 & 2) != 0) || (IVar1 == INTRNCVT_OVERFLOW)) {
      uVar3 = 0x80;
    }
    if (((local_18 & 1) != 0) || (IVar1 == INTRNCVT_UNDERFLOW)) {
      uVar3 = uVar3 | 0x100;
    }
  }
  else {
    uVar3 = 0x200;
    local_20.x._0_4_ = 0;
    local_20.x._4_4_ = 0;
  }
  _Flt->nbytes = local_28 - (int)local_24;
  *(undefined4 *)&_Flt->dval = local_20.x._0_4_;
  *(undefined4 *)((int)&_Flt->dval + 4) = local_20.x._4_4_;
  _Flt->flags = uVar3;
  p_Var2 = (FLT)__security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return p_Var2;
}


// 10018277 FUN_10018277

void __cdecl
FUN_10018277(undefined2 *param_1,int *param_2,char *param_3,int param_4,int param_5,int param_6,
            int param_7,int *param_8)

{
  int iVar1;
  uint uVar2;
  int iVar3;
  bool bVar4;
  bool bVar5;
  bool bVar6;
  ushort uVar7;
  char cVar8;
  ushort uVar9;
  DWORD *pDVar10;
  uint uVar11;
  int iVar12;
  ushort uVar13;
  int iVar14;
  uint uVar15;
  ushort uVar16;
  char *pcVar17;
  undefined4 uVar18;
  undefined **ppuVar19;
  undefined **ppuVar20;
  short *psVar21;
  uint uVar22;
  undefined4 uVar23;
  ushort uVar24;
  char *pcVar25;
  int local_6c;
  int local_68;
  undefined **local_64;
  ushort *local_60;
  int local_5c;
  char *local_58;
  int local_54;
  uint local_50;
  undefined2 local_4c;
  undefined4 uStack_4a;
  undefined2 uStack_46;
  undefined *local_44;
  undefined4 local_40;
  undefined4 local_3c;
  ushort uStack_38;
  ushort local_36;
  byte local_30;
  undefined1 uStack_2f;
  undefined4 uStack_2e;
  undefined4 uStack_2a;
  ushort uStack_26;
  char local_24 [23];
  char local_d;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  iVar14 = 0;
  pcVar25 = local_24;
  uVar7 = 0;
  local_6c = 1;
  local_50 = 0;
  bVar4 = false;
  bVar6 = false;
  bVar5 = false;
  local_68 = 0;
  local_54 = 0;
  if (param_8 != (int *)0x0) {
    local_58 = param_3;
    for (; (((cVar8 = *param_3, cVar8 == ' ' || (cVar8 == '\t')) || (cVar8 == '\n')) ||
           (cVar8 == '\r')); param_3 = param_3 + 1) {
    }
LAB_100182fc:
    cVar8 = *param_3;
    pcVar17 = param_3 + 1;
    switch(iVar14) {
    case 0:
      if ((byte)(cVar8 - 0x31U) < 9) {
LAB_10018319:
        iVar14 = 3;
        goto LAB_1001831b;
      }
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) {
LAB_10018330:
        iVar14 = 5;
        param_3 = pcVar17;
      }
      else if (cVar8 == '+') {
        uVar7 = 0;
        iVar14 = 2;
        param_3 = pcVar17;
      }
      else {
        if (cVar8 != '-') {
          if (cVar8 == '0') goto LAB_1001834a;
          goto LAB_100184d5;
        }
        iVar14 = 2;
        uVar7 = 0x8000;
        param_3 = pcVar17;
      }
      goto LAB_100182fc;
    case 1:
      bVar4 = true;
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_10018319;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10018381;
      if ((cVar8 == '+') || (cVar8 == '-')) goto LAB_100183b1;
      if (cVar8 == '0') goto LAB_1001834a;
      goto LAB_10018391;
    case 2:
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_10018319;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10018330;
      param_3 = local_58;
      if (cVar8 != '0') goto LAB_10018500;
LAB_1001834a:
      iVar14 = 1;
      param_3 = pcVar17;
      goto LAB_100182fc;
    case 3:
      while (('/' < cVar8 && (cVar8 < ':'))) {
        if (local_50 < 0x19) {
          local_50 = local_50 + 1;
          *pcVar25 = cVar8 + -0x30;
          pcVar25 = pcVar25 + 1;
        }
        else {
          local_54 = local_54 + 1;
        }
        cVar8 = *pcVar17;
        pcVar17 = pcVar17 + 1;
      }
      if (cVar8 != *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10018424;
LAB_10018381:
      bVar4 = true;
      iVar14 = 4;
      param_3 = pcVar17;
      goto LAB_100182fc;
    case 4:
      bVar6 = true;
      if (local_50 == 0) {
        while (cVar8 == '0') {
          local_54 = local_54 + -1;
          cVar8 = *pcVar17;
          pcVar17 = pcVar17 + 1;
        }
      }
      while (('/' < cVar8 && (cVar8 < ':'))) {
        if (local_50 < 0x19) {
          local_50 = local_50 + 1;
          *pcVar25 = cVar8 + -0x30;
          pcVar25 = pcVar25 + 1;
          local_54 = local_54 + -1;
        }
        cVar8 = *pcVar17;
        pcVar17 = pcVar17 + 1;
      }
LAB_10018424:
      if ((cVar8 == '+') || (cVar8 == '-')) {
LAB_100183b1:
        bVar4 = true;
        iVar14 = 0xb;
        param_3 = pcVar17 + -1;
      }
      else {
LAB_10018391:
        bVar4 = true;
        if ((cVar8 < 'D') || (('E' < cVar8 && ((cVar8 < 'd' || ('e' < cVar8)))))) goto LAB_100184d5;
        iVar14 = 6;
        param_3 = pcVar17;
      }
      goto LAB_100182fc;
    case 5:
      bVar6 = true;
      param_3 = local_58;
      if ((byte)(cVar8 - 0x30U) < 10) {
        iVar14 = 4;
        goto LAB_1001831b;
      }
      goto LAB_10018500;
    case 6:
      local_58 = param_3 + -1;
      if (8 < (byte)(cVar8 - 0x31U)) {
        if (cVar8 == '+') goto LAB_100184bc;
        if (cVar8 == '-') goto LAB_100184b0;
LAB_100184a3:
        param_3 = local_58;
        if (cVar8 != '0') goto LAB_10018500;
        iVar14 = 8;
        param_3 = pcVar17;
        goto LAB_100182fc;
      }
      break;
    case 7:
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_100184a3;
      break;
    case 8:
      bVar5 = true;
      while (cVar8 == '0') {
        cVar8 = *pcVar17;
        pcVar17 = pcVar17 + 1;
      }
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_100184d5;
      break;
    case 9:
      bVar5 = true;
      local_68 = 0;
      goto LAB_10018562;
    default:
      goto switchD_10018308_caseD_a;
    case 0xb:
      if (param_7 != 0) {
        local_58 = param_3;
        if (cVar8 == '+') {
LAB_100184bc:
          iVar14 = 7;
          param_3 = pcVar17;
        }
        else {
          if (cVar8 != '-') goto LAB_10018500;
LAB_100184b0:
          local_6c = -1;
          iVar14 = 7;
          param_3 = pcVar17;
        }
        goto LAB_100182fc;
      }
      iVar14 = 10;
      pcVar17 = param_3;
switchD_10018308_caseD_a:
      param_3 = pcVar17;
      if (iVar14 != 10) goto LAB_100182fc;
      goto LAB_10018500;
    }
    iVar14 = 9;
LAB_1001831b:
    param_3 = pcVar17 + -1;
    goto LAB_100182fc;
  }
  pDVar10 = FUN_10011e63();
  *pDVar10 = 0x16;
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
  goto LAB_100188f8;
LAB_10018562:
  if ((cVar8 < '0') || ('9' < cVar8)) goto LAB_1001857d;
  local_68 = local_68 * 10 + -0x30 + (int)cVar8;
  if (local_68 < 0x1451) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
    goto LAB_10018562;
  }
  local_68 = 0x1451;
LAB_1001857d:
  while (('/' < cVar8 && (cVar8 < ':'))) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
  }
LAB_100184d5:
  param_3 = pcVar17 + -1;
LAB_10018500:
  *param_2 = (int)param_3;
  if (bVar4) {
    if (0x18 < local_50) {
      if ('\x04' < local_d) {
        local_d = local_d + '\x01';
      }
      pcVar25 = pcVar25 + -1;
      local_54 = local_54 + 1;
      local_50 = 0x18;
    }
    if (local_50 == 0) goto LAB_100188da;
    while (pcVar25 = pcVar25 + -1, *pcVar25 == '\0') {
      local_50 = local_50 - 1;
      local_54 = local_54 + 1;
    }
    ___mtold12(local_24,local_50,&local_40);
    iVar12 = CONCAT22(local_3c._2_2_,(undefined2)local_3c);
    uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
    iVar1 = CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a);
    uVar22 = CONCAT22(uStack_2e._2_2_,(ushort)uStack_2e);
    iVar14 = CONCAT22(uStack_4a._2_2_,(undefined2)uStack_4a);
    if (local_6c < 0) {
      local_68 = -local_68;
    }
    local_58 = (char *)(local_68 + local_54);
    if (!bVar5) {
      local_58 = (char *)((int)local_58 + param_5);
    }
    if (!bVar6) {
      local_58 = (char *)((int)local_58 - param_6);
    }
    if ((int)local_58 < 0x1451) {
      if ((int)local_58 < -0x1450) goto LAB_100188da;
      ppuVar20 = &PTR_DAT_10036b50;
      if (local_58 != (char *)0x0) {
        if ((int)local_58 < 0) {
          local_58 = (char *)-(int)local_58;
          ppuVar20 = (undefined **)&DAT_10036cb0;
        }
        if (param_4 == 0) {
          local_40._0_2_ = 0;
        }
        iVar14 = uStack_4a;
        uVar22 = uStack_2e;
        iVar1 = uStack_2a;
        uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
        iVar12 = local_3c;
joined_r0x10018608:
        if (local_58 != (char *)0x0) {
          uVar15 = (int)local_58 >> 3;
          ppuVar20 = ppuVar20 + 0x15;
          uVar11 = (uint)local_58 & 7;
          local_58 = (char *)uVar15;
          if (uVar11 != 0) {
            ppuVar19 = ppuVar20 + uVar11 * 3;
            if (0x7fff < *(ushort *)ppuVar19) {
              local_4c = SUB42(*ppuVar19,0);
              uStack_4a._0_2_ = (undefined2)((uint)*ppuVar19 >> 0x10);
              uStack_4a._2_2_ = SUB42(ppuVar19[1],0);
              uStack_46 = (undefined2)((uint)ppuVar19[1] >> 0x10);
              local_44 = ppuVar19[2];
              iVar14 = CONCAT22(uStack_4a._2_2_,(undefined2)uStack_4a) + -1;
              uStack_4a._0_2_ = (undefined2)iVar14;
              uStack_4a._2_2_ = (undefined2)((uint)iVar14 >> 0x10);
              ppuVar19 = (undefined **)&local_4c;
            }
            local_54 = 0;
            local_30 = 0;
            uStack_2f = 0;
            uStack_2e._0_2_ = 0;
            uStack_2e._2_2_ = 0;
            uVar15 = 0;
            uStack_2a._0_2_ = 0;
            uStack_2a._2_2_ = 0;
            iVar3 = 0;
            uStack_26 = 0;
            uVar13 = local_36 & 0x7fff;
            uVar16 = *(ushort *)((int)ppuVar19 + 10) & 0x7fff;
            uVar9 = (*(ushort *)((int)ppuVar19 + 10) ^ local_36) & 0x8000;
            uVar24 = uVar16 + uVar13;
            uVar22 = 0;
            iVar1 = 0;
            if (((uVar13 < 0x7fff) && (uVar22 = 0, iVar1 = 0, uVar16 < 0x7fff)) &&
               (uVar22 = uVar15, iVar1 = iVar3, uVar24 < 0xbffe)) {
              if (0x3fbf < uVar24) {
                if (((uVar13 == 0) &&
                    (uVar24 = uVar24 + 1, (CONCAT22(local_36,uStack_38) & 0x7fffffff) == 0)) &&
                   ((iVar12 == 0 && (uVar2 == 0)))) {
                  local_36 = 0;
                }
                else if (((uVar16 == 0) &&
                         (uVar24 = uVar24 + 1, ((uint)ppuVar19[2] & 0x7fffffff) == 0)) &&
                        ((ppuVar19[1] == (undefined *)0x0 && (*ppuVar19 == (undefined *)0x0)))) {
                  uStack_38 = 0;
                  local_36 = 0;
                  local_3c._0_2_ = 0;
                  local_3c._2_2_ = 0;
                  local_40._0_2_ = 0;
                  local_40._2_2_ = 0;
                  uVar2 = 0;
                  iVar12 = 0;
                }
                else {
                  local_6c = 0;
                  psVar21 = (short *)((int)&uStack_2e + 2);
                  local_5c = 5;
                  do {
                    local_68 = local_5c;
                    if (0 < local_5c) {
                      local_60 = (ushort *)((int)&local_40 + local_6c * 2);
                      local_64 = ppuVar19 + 2;
                      do {
                        bVar4 = false;
                        uVar22 = *(uint *)(psVar21 + -2) +
                                 (uint)*(ushort *)local_64 * (uint)*local_60;
                        if ((uVar22 < *(uint *)(psVar21 + -2)) ||
                           (uVar22 < (uint)*(ushort *)local_64 * (uint)*local_60)) {
                          bVar4 = true;
                        }
                        *(uint *)(psVar21 + -2) = uVar22;
                        if (bVar4) {
                          *psVar21 = *psVar21 + 1;
                        }
                        local_60 = local_60 + 1;
                        local_64 = (undefined **)((int)local_64 + -2);
                        local_68 = local_68 + -1;
                      } while (0 < local_68);
                    }
                    psVar21 = psVar21 + 1;
                    local_6c = local_6c + 1;
                    local_5c = local_5c + -1;
                  } while (0 < local_5c);
                  uVar24 = uVar24 + 0xc002;
                  if ((short)uVar24 < 1) {
LAB_100187be:
                    uVar24 = uVar24 - 1;
                    if ((short)uVar24 < 0) {
                      uVar22 = (uint)(ushort)-uVar24;
                      uVar24 = 0;
                      do {
                        if ((local_30 & 1) != 0) {
                          local_54 = local_54 + 1;
                        }
                        iVar12 = CONCAT22(uStack_26,uStack_2a._2_2_);
                        uVar2 = CONCAT22((ushort)uStack_2a,uStack_2e._2_2_);
                        iVar1 = CONCAT22((ushort)uStack_2a,uStack_2e._2_2_);
                        uStack_2a._2_2_ = (ushort)(CONCAT22(uStack_26,uStack_2a._2_2_) >> 1);
                        uStack_26 = uStack_26 >> 1;
                        uStack_2a._0_2_ =
                             (ushort)uStack_2a >> 1 | (ushort)((uint)(iVar12 << 0x1f) >> 0x10);
                        uVar15 = CONCAT22((ushort)uStack_2e,CONCAT11(uStack_2f,local_30)) >> 1;
                        uStack_2e._0_2_ =
                             (ushort)uStack_2e >> 1 | (ushort)((uint)(iVar1 << 0x1f) >> 0x10);
                        uVar22 = uVar22 - 1;
                        uStack_2e._2_2_ = (ushort)(uVar2 >> 1);
                        local_30 = (byte)uVar15;
                        uStack_2f = (undefined1)(uVar15 >> 8);
                      } while (uVar22 != 0);
                      if (local_54 != 0) {
                        local_30 = local_30 | 1;
                      }
                    }
                  }
                  else {
                    do {
                      uVar13 = (ushort)uStack_2e;
                      if ((uStack_26 & 0x8000) != 0) break;
                      iVar1 = CONCAT22((ushort)uStack_2e,CONCAT11(uStack_2f,local_30)) << 1;
                      local_30 = (byte)iVar1;
                      uStack_2f = (undefined1)((uint)iVar1 >> 8);
                      uStack_2e._0_2_ = (ushort)((uint)iVar1 >> 0x10);
                      iVar1 = CONCAT22((ushort)uStack_2a,uStack_2e._2_2_) * 2;
                      uStack_2e._2_2_ = (ushort)iVar1 | uVar13 >> 0xf;
                      iVar12 = CONCAT22(uStack_26,uStack_2a._2_2_) * 2;
                      uStack_2a._2_2_ = (ushort)iVar12 | (ushort)uStack_2a >> 0xf;
                      uVar24 = uVar24 - 1;
                      uStack_2a._0_2_ = (ushort)((uint)iVar1 >> 0x10);
                      uStack_26 = (ushort)((uint)iVar12 >> 0x10);
                    } while (0 < (short)uVar24);
                    if ((short)uVar24 < 1) goto LAB_100187be;
                  }
                  if ((0x8000 < CONCAT11(uStack_2f,local_30)) ||
                     (iVar1 = CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a),
                     uVar22 = CONCAT22(uStack_2e._2_2_,(ushort)uStack_2e),
                     (CONCAT22((ushort)uStack_2e,CONCAT11(uStack_2f,local_30)) & 0x1ffff) == 0x18000
                     )) {
                    if (CONCAT22(uStack_2e._2_2_,(ushort)uStack_2e) == -1) {
                      uStack_2e._0_2_ = 0;
                      uStack_2e._2_2_ = 0;
                      uVar22 = 0;
                      if (CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a) == -1) {
                        uStack_2a._0_2_ = 0;
                        uStack_2a._2_2_ = 0;
                        if (uStack_26 == 0xffff) {
                          uStack_26 = 0x8000;
                          uVar24 = uVar24 + 1;
                          iVar1 = 0;
                          uVar22 = 0;
                        }
                        else {
                          uStack_26 = uStack_26 + 1;
                          iVar1 = 0;
                          uVar22 = 0;
                        }
                      }
                      else {
                        iVar1 = CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a) + 1;
                        uStack_2a._0_2_ = (ushort)iVar1;
                        uStack_2a._2_2_ = (ushort)((uint)iVar1 >> 0x10);
                      }
                    }
                    else {
                      uVar22 = CONCAT22(uStack_2e._2_2_,(ushort)uStack_2e) + 1;
                      uStack_2e._0_2_ = (ushort)uVar22;
                      uStack_2e._2_2_ = (ushort)(uVar22 >> 0x10);
                      iVar1 = CONCAT22(uStack_2a._2_2_,(ushort)uStack_2a);
                    }
                  }
                  if (0x7ffe < uVar24) goto LAB_1001887d;
                  local_40 = uVar22;
                  local_3c = iVar1;
                  uStack_38 = uStack_26;
                  local_36 = uVar24 | uVar9;
                  uVar2 = uVar22;
                  iVar12 = iVar1;
                }
                goto joined_r0x10018608;
              }
              iVar12 = 0;
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            else {
LAB_1001887d:
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              iVar12 = (-(uint)(uVar9 != 0) & 0x80000000) + 0x7fff8000;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            uStack_38 = (ushort)iVar12;
            local_36 = (ushort)((uint)iVar12 >> 0x10);
            uVar2 = 0;
            iVar12 = 0;
          }
          goto joined_r0x10018608;
        }
      }
      local_3c._2_2_ = (undefined2)((uint)iVar12 >> 0x10);
      local_3c._0_2_ = (undefined2)iVar12;
      local_40._2_2_ = (undefined2)(uVar2 >> 0x10);
      local_40._0_2_ = (undefined2)uVar2;
      uVar23 = CONCAT22((undefined2)local_3c,local_40._2_2_);
      uVar18 = CONCAT22(uStack_38,local_3c._2_2_);
      uVar24 = local_36;
      uStack_4a = iVar14;
      uStack_2e = uVar22;
      uStack_2a = iVar1;
      local_3c = iVar12;
    }
    else {
      uVar23 = 0;
      uVar24 = 0x7fff;
      uVar18 = 0x80000000;
      local_40._0_2_ = 0;
    }
  }
  else {
LAB_100188da:
    uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
    local_40._0_2_ = 0;
    uVar24 = 0;
    uVar18 = 0;
    uVar23 = 0;
  }
  *param_1 = (undefined2)local_40;
  param_1[5] = uVar24 | uVar7;
  *(undefined4 *)(param_1 + 1) = uVar23;
  *(undefined4 *)(param_1 + 3) = uVar18;
LAB_100188f8:
  local_40 = uVar2;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10018940 __security_check_cookie

/* Library Function - Single Match
    @__security_check_cookie@4
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release
   __fastcall __security_check_cookie,4 */

void __fastcall __security_check_cookie(int param_1)

{
  if (param_1 == DAT_100366b8) {
    return;
  }
                    /* WARNING: Subroutine does not return */
  ___report_gsfailure();
}


// 1001894f __mtinitlocks

/* Library Function - Single Match
    __mtinitlocks
   
   Library: Visual Studio 2005 Release */

int __cdecl __mtinitlocks(void)

{
  int iVar1;
  int iVar2;
  undefined *puVar3;
  
  iVar2 = 0;
  puVar3 = &DAT_1003a308;
  do {
    if ((&DAT_100366c4)[iVar2 * 2] == 1) {
      (&DAT_100366c0)[iVar2 * 2] = puVar3;
      puVar3 = puVar3 + 0x18;
      iVar1 = ___crtInitCritSecAndSpinCount((&DAT_100366c0)[iVar2 * 2],4000);
      if (iVar1 == 0) {
        (&DAT_100366c0)[iVar2 * 2] = 0;
        return 0;
      }
    }
    iVar2 = iVar2 + 1;
  } while (iVar2 < 0x24);
  return 1;
}


// 10018998 __mtdeletelocks

/* Library Function - Single Match
    __mtdeletelocks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __cdecl __mtdeletelocks(void)

{
  LPCRITICAL_SECTION lpCriticalSection;
  undefined4 *puVar1;
  
  puVar1 = &DAT_100366c0;
  do {
    lpCriticalSection = (LPCRITICAL_SECTION)*puVar1;
    if ((lpCriticalSection != (LPCRITICAL_SECTION)0x0) && (puVar1[1] != 1)) {
      DeleteCriticalSection(lpCriticalSection);
      FUN_100012d0(lpCriticalSection);
      *puVar1 = 0;
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x100367e0);
  puVar1 = &DAT_100366c0;
  do {
    if (((LPCRITICAL_SECTION)*puVar1 != (LPCRITICAL_SECTION)0x0) && (puVar1[1] == 1)) {
      DeleteCriticalSection((LPCRITICAL_SECTION)*puVar1);
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x100367e0);
  return;
}


// 100189ed FUN_100189ed

void __cdecl FUN_100189ed(int param_1)

{
  LeaveCriticalSection((LPCRITICAL_SECTION)(&DAT_100366c0)[param_1 * 2]);
  return;
}


// 10018a02 __mtinitlocknum

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __mtinitlocknum
   
   Library: Visual Studio 2005 Release */

int __cdecl __mtinitlocknum(int _LockNum)

{
  int *piVar1;
  DWORD *pDVar2;
  int iVar3;
  int iVar4;
  int local_20;
  
  iVar4 = 1;
  local_20 = 1;
  if (DAT_10034004 == 0) {
    __FF_MSGBANNER();
    FUN_10012d79(0x1e);
    ___crtExitProcess(0xff);
  }
  piVar1 = &DAT_100366c0 + _LockNum * 2;
  if (*piVar1 == 0) {
    iVar4 = FUN_10001270(0x18);
    if (iVar4 == 0) {
      pDVar2 = FUN_10011e63();
      *pDVar2 = 0xc;
      iVar4 = 0;
    }
    else {
      __lock(10);
      if (*piVar1 == 0) {
        iVar3 = ___crtInitCritSecAndSpinCount(iVar4,4000);
        if (iVar3 == 0) {
          FUN_100012d0(iVar4);
          pDVar2 = FUN_10011e63();
          *pDVar2 = 0xc;
          local_20 = 0;
        }
        else {
          *piVar1 = iVar4;
        }
      }
      else {
        FUN_100012d0(iVar4);
      }
      FUN_10018abc();
      iVar4 = local_20;
    }
  }
  return iVar4;
}


// 10018abc FUN_10018abc

void FUN_10018abc(void)

{
  FUN_100189ed(10);
  return;
}


// 10018ac5 __lock

/* Library Function - Single Match
    __lock
   
   Library: Visual Studio 2005 Release */

void __cdecl __lock(int _File)

{
  int iVar1;
  
  if ((&DAT_100366c0)[_File * 2] == 0) {
    iVar1 = __mtinitlocknum(_File);
    if (iVar1 == 0) {
      __amsg_exit(0x11);
    }
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(&DAT_100366c0)[_File * 2]);
  return;
}


// 10018af6 FUN_10018af6

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

undefined4 FUN_10018af6(void)

{
  return 1;
}


// 10018b46 __get_sse2_info

/* WARNING: Removing unreachable block (ram,0x10018b81) */
/* WARNING: Removing unreachable block (ram,0x10018b6e) */
/* Library Function - Single Match
    __get_sse2_info
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

undefined4 __get_sse2_info(void)

{
  uint uVar1;
  int iVar2;
  undefined4 uVar3;
  byte in_AF;
  byte in_TF;
  byte in_IF;
  byte in_NT;
  byte in_AC;
  byte in_VIF;
  byte in_VIP;
  byte in_ID;
  uint uVar4;
  undefined4 local_8;
  
  local_8 = 0;
  uVar4 = (uint)(in_NT & 1) * 0x4000 | (uint)(in_IF & 1) * 0x200 | (uint)(in_TF & 1) * 0x100 | 0x40
          | (uint)(in_AF & 1) * 0x10 | 4 | (uint)(in_ID & 1) * 0x200000 |
          (uint)(in_VIP & 1) * 0x100000 | (uint)(in_VIF & 1) * 0x80000 | (uint)(in_AC & 1) * 0x40000
  ;
  uVar1 = uVar4 ^ 0x200000;
  if (((uint)((uVar1 & 0x4000) != 0) * 0x4000 | (uint)((uVar1 & 0x200) != 0) * 0x200 |
       (uint)((uVar1 & 0x100) != 0) * 0x100 | (uint)((uVar1 & 0x40) != 0) * 0x40 |
       (uint)((uVar1 & 0x10) != 0) * 0x10 | (uint)((uVar1 & 4) != 0) * 4 |
       (uint)((uVar1 & 0x200000) != 0) * 0x200000 | (uint)((uVar1 & 0x40000) != 0) * 0x40000) !=
      uVar4) {
    cpuid_basic_info(0);
    iVar2 = cpuid_Version_info(1);
    local_8 = *(uint *)(iVar2 + 8);
  }
  if (((local_8 & 0x4000000) == 0) || (iVar2 = FUN_10018af6(), iVar2 == 0)) {
    uVar3 = 0;
  }
  else {
    uVar3 = 1;
  }
  return uVar3;
}


// 10018bb3 __lseeki64_nolock

/* Library Function - Single Match
    __lseeki64_nolock
   
   Library: Visual Studio 2005 Release */

longlong __cdecl __lseeki64_nolock(int _FileHandle,longlong _Offset,int _Origin)

{
  byte *pbVar1;
  HANDLE hFile;
  DWORD *pDVar2;
  DWORD DVar3;
  DWORD DVar4;
  LONG in_stack_00000008;
  LONG local_8;
  
  local_8 = (LONG)_Offset;
  hFile = (HANDLE)__get_osfhandle(_FileHandle);
  if (hFile == (HANDLE)0xffffffff) {
    pDVar2 = FUN_10011e63();
    *pDVar2 = 9;
LAB_10018be2:
    DVar3 = 0xffffffff;
    local_8 = -1;
  }
  else {
    DVar3 = SetFilePointer(hFile,in_stack_00000008,&local_8,_Offset._4_4_);
    if (DVar3 == 0xffffffff) {
      DVar4 = GetLastError();
      if (DVar4 != 0) {
        FUN_10011e89(DVar4);
        goto LAB_10018be2;
      }
    }
    pbVar1 = (byte *)((&DAT_1004bbe0)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38);
    *pbVar1 = *pbVar1 & 0xfd;
  }
  return CONCAT44(local_8,DVar3);
}


// 10018c36 __lseeki64

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __lseeki64
   
   Library: Visual Studio 2005 Release */

longlong __cdecl __lseeki64(int _FileHandle,longlong _Offset,int _Origin)

{
  DWORD *pDVar1;
  int iVar2;
  int in_stack_ffffffc8;
  undefined8 local_28;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_10011e76();
    *pDVar1 = 0;
    pDVar1 = FUN_10011e63();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1004bbc4)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_1004bbe0)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
        pDVar1 = FUN_10011e76();
        *pDVar1 = 0;
        pDVar1 = FUN_10011e63();
        *pDVar1 = 9;
        FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        local_28._4_4_ = 0xffffffff;
        local_28._0_4_ = 0xffffffff;
      }
      else {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_1004bbe0)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_10011e63();
          *pDVar1 = 9;
          pDVar1 = FUN_10011e76();
          *pDVar1 = 0;
          local_28 = -1;
        }
        else {
          local_28 = __lseeki64_nolock(_FileHandle,_Offset,in_stack_ffffffc8);
        }
        FUN_10018d45();
      }
      goto LAB_10018d3f;
    }
    pDVar1 = FUN_10011e76();
    *pDVar1 = 0;
    pDVar1 = FUN_10011e63();
    *pDVar1 = 9;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  local_28._0_4_ = 0xffffffff;
  local_28._4_4_ = 0xffffffff;
LAB_10018d3f:
  return CONCAT44(local_28._4_4_,(undefined4)local_28);
}


// 10018d45 FUN_10018d45

void FUN_10018d45(void)

{
  int unaff_EBP;
  
  FUN_1001cd06(*(uint *)(unaff_EBP + 8));
  return;
}


// 10018d4f __write_nolock

/* WARNING: Type propagation algorithm not settling */
/* Library Function - Single Match
    __write_nolock
   
   Library: Visual Studio 2005 Release */

int __cdecl __write_nolock(int _FileHandle,void *_Buf,uint _MaxCharCount)

{
  WCHAR *pWVar1;
  char cVar2;
  wchar_t *pwVar3;
  WCHAR WVar4;
  wchar_t wVar5;
  DWORD *pDVar6;
  int iVar7;
  _ptiddata p_Var8;
  BOOL BVar9;
  DWORD DVar10;
  int iVar11;
  uint uVar12;
  WCHAR *pWVar13;
  uint uVar14;
  char *pcVar15;
  int unaff_EDI;
  int *piVar16;
  UINT local_598;
  uint local_594;
  WCHAR *local_590;
  int *local_58c;
  char *local_588;
  int local_584;
  WCHAR *local_580;
  char *local_57c;
  WCHAR *local_578;
  char local_571;
  DWORD local_570;
  WCHAR *local_56c;
  WCHAR local_568 [38];
  undefined1 local_51c [264];
  CHAR local_414 [688];
  WCHAR local_164 [170];
  CHAR local_10 [8];
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)local_51c;
  local_580 = (WCHAR *)_Buf;
  local_57c = (char *)0x0;
  local_584 = 0;
  if (_MaxCharCount == 0) goto LAB_100192ff;
  if (_Buf == (void *)0x0) {
    pDVar6 = FUN_10011e76();
    *pDVar6 = 0;
    pDVar6 = FUN_10011e63();
    *pDVar6 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_100192ff;
  }
  iVar11 = (_FileHandle & 0x1fU) * 0x38;
  piVar16 = &DAT_1004bbe0 + (_FileHandle >> 5);
  local_571 = (char)(*(char *)(*piVar16 + iVar11 + 0x24) * '\x02') >> 1;
  local_58c = piVar16;
  if (((local_571 == '\x02') || (local_571 == '\x01')) && ((~_MaxCharCount & 1) == 0)) {
    pDVar6 = FUN_10011e76();
    *pDVar6 = 0;
    pDVar6 = FUN_10011e63();
    *pDVar6 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_100192ff;
  }
  if ((*(byte *)(*piVar16 + iVar11 + 4) & 0x20) != 0) {
    __lseeki64_nolock(_FileHandle,0x200000000,unaff_EDI);
  }
  iVar7 = __isatty(_FileHandle);
  if ((iVar7 == 0) || ((*(byte *)(iVar11 + 4 + *piVar16) & 0x80) == 0)) {
LAB_1001903a:
    if ((*(byte *)((undefined4 *)(*piVar16 + iVar11) + 1) & 0x80) == 0) {
      BVar9 = WriteFile(*(HANDLE *)(*piVar16 + iVar11),local_580,_MaxCharCount,(LPDWORD)&local_588,
                        (LPOVERLAPPED)0x0);
      if (BVar9 == 0) {
LAB_10019294:
        local_570 = GetLastError();
      }
      else {
        local_570 = 0;
        local_57c = local_588;
      }
LAB_1001929d:
      piVar16 = local_58c;
      if (local_57c != (char *)0x0) goto LAB_100192ff;
      goto LAB_100192a7;
    }
    local_570 = 0;
    if (local_571 == '\0') {
      local_56c = local_580;
      if (_MaxCharCount == 0) goto LAB_100192ce;
      do {
        local_578 = (WCHAR *)0x0;
        uVar12 = (int)local_56c - (int)local_580;
        pWVar13 = local_568;
        do {
          if (_MaxCharCount <= uVar12) break;
          pWVar1 = (WCHAR *)((int)local_56c + 1);
          WVar4 = *local_56c;
          uVar12 = uVar12 + 1;
          if ((char)WVar4 == '\n') {
            local_584 = local_584 + 1;
            *(char *)pWVar13 = '\r';
            pWVar13 = (WCHAR *)((int)pWVar13 + 1);
            local_578 = (WCHAR *)((int)local_578 + 1);
          }
          *(char *)pWVar13 = (char)WVar4;
          pWVar13 = (WCHAR *)((int)pWVar13 + 1);
          local_578 = (WCHAR *)((int)local_578 + 1);
          local_56c = pWVar1;
        } while (local_578 < (WCHAR *)0x400);
        BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_568,(int)pWVar13 - (int)local_568,
                          (LPDWORD)&local_588,(LPOVERLAPPED)0x0);
        if (BVar9 == 0) goto LAB_10019294;
        local_57c = local_57c + (int)local_588;
      } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
              ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
      goto LAB_1001929d;
    }
    if (local_571 == '\x02') {
      local_56c = local_580;
      if (_MaxCharCount != 0) {
        do {
          uVar14 = 0;
          uVar12 = (int)local_56c - (int)local_580;
          pWVar13 = local_568;
          do {
            if (_MaxCharCount <= uVar12) break;
            pWVar1 = local_56c + 1;
            WVar4 = *local_56c;
            uVar12 = uVar12 + 2;
            if (WVar4 == L'\n') {
              local_584 = local_584 + 2;
              *pWVar13 = L'\r';
              pWVar13 = pWVar13 + 1;
              uVar14 = uVar14 + 2;
            }
            *pWVar13 = WVar4;
            pWVar13 = pWVar13 + 1;
            uVar14 = uVar14 + 2;
            piVar16 = local_58c;
            local_56c = pWVar1;
          } while (uVar14 < 0x3ff);
          BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_568,(int)pWVar13 - (int)local_568,
                            (LPDWORD)&local_588,(LPOVERLAPPED)0x0);
          if (BVar9 == 0) goto LAB_10019294;
          local_57c = local_57c + (int)local_588;
        } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
                ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
        goto LAB_1001929d;
      }
    }
    else {
      local_578 = local_580;
      if (_MaxCharCount != 0) {
        do {
          local_56c = (WCHAR *)0x0;
          uVar12 = (int)local_578 - (int)local_580;
          pWVar13 = local_164;
          do {
            if (_MaxCharCount <= uVar12) break;
            WVar4 = *local_578;
            local_578 = local_578 + 1;
            uVar12 = uVar12 + 2;
            if (WVar4 == L'\n') {
              *pWVar13 = L'\r';
              pWVar13 = pWVar13 + 1;
              local_56c = local_56c + 1;
            }
            local_56c = local_56c + 1;
            *pWVar13 = WVar4;
            pWVar13 = pWVar13 + 1;
          } while (local_56c < (WCHAR *)0x152);
          pcVar15 = (char *)0x0;
          iVar7 = WideCharToMultiByte(0xfde9,0,local_164,((int)pWVar13 - (int)local_164) / 2,
                                      local_414,0x2ab,(LPCSTR)0x0,(LPBOOL)0x0);
          if (iVar7 == 0) goto LAB_10019294;
          do {
            BVar9 = WriteFile(*(HANDLE *)(iVar11 + *local_58c),local_414 + (int)pcVar15,
                              iVar7 - (int)pcVar15,(LPDWORD)&local_588,(LPOVERLAPPED)0x0);
            if (BVar9 == 0) {
              local_570 = GetLastError();
              break;
            }
            pcVar15 = pcVar15 + (int)local_588;
          } while ((int)pcVar15 < iVar7);
        } while ((iVar7 <= (int)pcVar15) &&
                (local_57c = (char *)((int)local_578 - (int)local_580), local_57c < _MaxCharCount));
        goto LAB_1001929d;
      }
    }
LAB_100192ce:
    DVar10 = 0;
    if (((*(byte *)(iVar11 + 4 + *piVar16) & 0x40) != 0) && ((char)*local_580 == '\x1a'))
    goto LAB_100192ff;
    pDVar6 = FUN_10011e63();
    *pDVar6 = 0x1c;
  }
  else {
    p_Var8 = __getptd();
    pwVar3 = p_Var8->ptlocinfo->lc_category[0].wlocale;
    BVar9 = GetConsoleMode(*(HANDLE *)(iVar11 + *piVar16),&local_598);
    if ((BVar9 == 0) || ((pwVar3 == (wchar_t *)0x0 && (local_571 == '\0')))) goto LAB_1001903a;
    local_598 = GetConsoleCP();
    local_56c = (WCHAR *)0x0;
    local_590 = local_580;
    if (_MaxCharCount != 0) {
      local_578 = (WCHAR *)0x0;
      do {
        pWVar13 = local_590;
        if (local_571 == '\0') {
          cVar2 = (char)*local_590;
          local_594 = (uint)(cVar2 == '\n');
          iVar7 = _isleadbyte(CONCAT22(cVar2 >> 7,(short)cVar2));
          if (iVar7 == 0) {
            iVar7 = _mbtowc((wchar_t *)&local_570,(char *)pWVar13,1);
            if (iVar7 == -1) break;
          }
          else {
            if (((char *)((int)local_580 + (_MaxCharCount - (int)pWVar13)) < (char *)0x2) ||
               (iVar7 = _mbtowc((wchar_t *)&local_570,(char *)pWVar13,2), iVar7 == -1)) break;
            pWVar13 = (WCHAR *)((int)pWVar13 + 1);
            local_578 = (WCHAR *)((int)local_578 + 1);
          }
          local_590 = (WCHAR *)((int)pWVar13 + 1);
          local_578 = (WCHAR *)((int)local_578 + 1);
          DVar10 = WideCharToMultiByte(local_598,0,(LPCWSTR)&local_570,1,local_10,5,(LPCSTR)0x0,
                                       (LPBOOL)0x0);
          if (DVar10 == 0) break;
          BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_10,DVar10,(LPDWORD)&local_56c,
                            (LPOVERLAPPED)0x0);
          if (BVar9 == 0) goto LAB_10019294;
          local_57c = local_57c + (int)local_56c;
          if ((int)local_56c < (int)DVar10) break;
          if (local_594 != 0) {
            local_10[0] = '\r';
            BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_10,1,(LPDWORD)&local_56c,
                              (LPOVERLAPPED)0x0);
            if (BVar9 == 0) goto LAB_10019294;
            if ((int)local_56c < 1) break;
            local_584 = local_584 + 1;
            local_57c = local_57c + 1;
          }
        }
        else {
          if ((local_571 == '\x01') || (local_571 == '\x02')) {
            local_570 = (DWORD)(ushort)*local_590;
            local_594 = (uint)(*local_590 == L'\n');
            local_590 = local_590 + 1;
            local_578 = local_578 + 1;
          }
          if ((local_571 == '\x01') || (local_571 == '\x02')) {
            wVar5 = __putwch_nolock((wchar_t)local_570);
            if (wVar5 != (wchar_t)local_570) goto LAB_10019294;
            local_57c = local_57c + 1;
            if (local_594 != 0) {
              local_570 = 0xd;
              wVar5 = __putwch_nolock(L'\r');
              if (wVar5 != (wchar_t)local_570) goto LAB_10019294;
              local_57c = local_57c + 1;
              local_584 = local_584 + 1;
            }
          }
        }
      } while (local_578 < _MaxCharCount);
      goto LAB_1001929d;
    }
LAB_100192a7:
    if (local_570 == 0) goto LAB_100192ce;
    DVar10 = 5;
    if (local_570 != 5) {
      FUN_10011e89(local_570);
      goto LAB_100192ff;
    }
    pDVar6 = FUN_10011e63();
    *pDVar6 = 9;
  }
  pDVar6 = FUN_10011e76();
  *pDVar6 = DVar10;
LAB_100192ff:
  iVar11 = __security_check_cookie(local_8 ^ (uint)local_51c);
  return iVar11;
}


// 10019315 __write

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __write
   
   Library: Visual Studio 2005 Release */

int __cdecl __write(int _FileHandle,void *_Buf,uint _MaxCharCount)

{
  DWORD *pDVar1;
  int iVar2;
  int local_20;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_10011e76();
    *pDVar1 = 0;
    pDVar1 = FUN_10011e63();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1004bbc4)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_1004bbe0)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_1004bbe0)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_10011e63();
          *pDVar1 = 9;
          pDVar1 = FUN_10011e76();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = __write_nolock(_FileHandle,_Buf,_MaxCharCount);
        }
        FUN_100193e7();
        return local_20;
      }
    }
    pDVar1 = FUN_10011e76();
    *pDVar1 = 0;
    pDVar1 = FUN_10011e63();
    *pDVar1 = 9;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 100193e7 FUN_100193e7

void FUN_100193e7(void)

{
  int unaff_EBP;
  
  FUN_1001cd06(*(uint *)(unaff_EBP + 8));
  return;
}


// 100193f1 __getbuf

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __getbuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __getbuf(FILE *_File)

{
  char *pcVar1;
  
  _DAT_1003a458 = _DAT_1003a458 + 1;
  pcVar1 = (char *)FUN_10001270(0x1000);
  _File->_base = pcVar1;
  if (pcVar1 == (char *)0x0) {
    _File->_flag = _File->_flag | 4;
    _File->_base = (char *)&_File->_charbuf;
    _File->_bufsiz = 2;
  }
  else {
    _File->_flag = _File->_flag | 8;
    _File->_bufsiz = 0x1000;
  }
  _File->_cnt = 0;
  _File->_ptr = _File->_base;
  return;
}


// 10019435 __isatty

/* Library Function - Single Match
    __isatty
   
   Library: Visual Studio 2005 Release */

int __cdecl __isatty(int _FileHandle)

{
  DWORD *pDVar1;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 9;
    return 0;
  }
  if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1004bbc4)) {
    return *(byte *)((&DAT_1004bbe0)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38) & 0x40;
  }
  pDVar1 = FUN_10011e63();
  *pDVar1 = 9;
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0;
}


// 10019493 FUN_10019493

undefined ** FUN_10019493(void)

{
  return &PTR_DAT_100367e8;
}


// 1001956a FUN_1001956a

void __cdecl FUN_1001956a(uint param_1)

{
  if ((0x100367e7 < param_1) && (param_1 < 0x10036a49)) {
    __lock(((int)(param_1 + 0xeffc9818) >> 5) + 0x10);
    *(uint *)(param_1 + 0xc) = *(uint *)(param_1 + 0xc) | 0x8000;
    return;
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(param_1 + 0x20));
  return;
}


// 100195d4 FUN_100195d4

void __cdecl FUN_100195d4(uint param_1)

{
  if ((0x100367e7 < param_1) && (param_1 < 0x10036a49)) {
    *(uint *)(param_1 + 0xc) = *(uint *)(param_1 + 0xc) & 0xffff7fff;
    FUN_100189ed(((int)(param_1 + 0xeffc9818) >> 5) + 0x10);
    return;
  }
  LeaveCriticalSection((LPCRITICAL_SECTION)(param_1 + 0x20));
  return;
}


// 10019634 __fileno

/* Library Function - Single Match
    __fileno
   
   Library: Visual Studio 2005 Release */

int __cdecl __fileno(FILE *_File)

{
  DWORD *pDVar1;
  
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  return _File->_file;
}


// 10019661 FUN_10019661

bool FUN_10019661(void)

{
  return DAT_1003a45c == (DAT_100366b8 | 1);
}


// 10019677 __wctomb_s_l

/* Library Function - Single Match
    __wctomb_s_l
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl
__wctomb_s_l(int *_SizeConverted,char *_MbCh,size_t _SizeInBytes,wchar_t _WCh,_locale_t _Locale)

{
  char *lpMultiByteStr;
  size_t _Size;
  DWORD DVar1;
  DWORD *pDVar2;
  int iVar3;
  int local_14 [2];
  int local_c;
  char local_8;
  
  _Size = _SizeInBytes;
  lpMultiByteStr = _MbCh;
  if ((_MbCh == (char *)0x0) && (_SizeInBytes != 0)) {
    if (_SizeConverted != (int *)0x0) {
      *_SizeConverted = 0;
    }
LAB_10019699:
    DVar1 = 0;
  }
  else {
    if (_SizeConverted != (int *)0x0) {
      *_SizeConverted = -1;
    }
    if (0x7fffffff < _SizeInBytes) {
      pDVar2 = FUN_10011e63();
      *pDVar2 = 0x16;
      FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return 0x16;
    }
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
    if (*(int *)(local_14[0] + 0x14) == 0) {
      if ((ushort)_WCh < 0x100) {
        if (lpMultiByteStr != (char *)0x0) {
          if (_Size == 0) goto LAB_10019729;
          *lpMultiByteStr = (char)_WCh;
        }
        if (_SizeConverted != (int *)0x0) {
          *_SizeConverted = 1;
        }
LAB_10019760:
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        goto LAB_10019699;
      }
      if ((lpMultiByteStr != (char *)0x0) && (_Size != 0)) {
        _memset(lpMultiByteStr,0,_Size);
      }
    }
    else {
      _MbCh = (char *)0x0;
      iVar3 = WideCharToMultiByte(*(UINT *)(local_14[0] + 4),0,&_WCh,1,lpMultiByteStr,_Size,
                                  (LPCSTR)0x0,(LPBOOL)&_MbCh);
      if (iVar3 == 0) {
        DVar1 = GetLastError();
        if (DVar1 == 0x7a) {
          if ((lpMultiByteStr != (char *)0x0) && (_Size != 0)) {
            _memset(lpMultiByteStr,0,_Size);
          }
LAB_10019729:
          pDVar2 = FUN_10011e63();
          *pDVar2 = 0x22;
          FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
          if (local_8 == '\0') {
            return 0x22;
          }
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
          return 0x22;
        }
      }
      else if (_MbCh == (char *)0x0) {
        if (_SizeConverted != (int *)0x0) {
          *_SizeConverted = iVar3;
        }
        goto LAB_10019760;
      }
    }
    pDVar2 = FUN_10011e63();
    *pDVar2 = 0x2a;
    pDVar2 = FUN_10011e63();
    DVar1 = *pDVar2;
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return DVar1;
}


// 100197d6 _wctomb_s

/* Library Function - Single Match
    _wctomb_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _wctomb_s(int *_SizeConverted,char *_MbCh,rsize_t _SizeInBytes,wchar_t _WCh)

{
  errno_t eVar1;
  
  eVar1 = __wctomb_s_l(_SizeConverted,_MbCh,_SizeInBytes,_WCh,(_locale_t)0x0);
  return eVar1;
}


// 10019800 __aulldvrm

/* Library Function - Single Match
    __aulldvrm
   
   Library: Visual Studio */

undefined8 __aulldvrm(uint param_1,uint param_2,uint param_3,uint param_4)

{
  ulonglong uVar1;
  longlong lVar2;
  uint uVar3;
  int iVar4;
  uint uVar5;
  uint uVar6;
  uint uVar7;
  uint uVar8;
  uint uVar9;
  
  uVar3 = param_1;
  uVar8 = param_4;
  uVar6 = param_2;
  uVar9 = param_3;
  if (param_4 == 0) {
    uVar3 = param_2 / param_3;
    iVar4 = (int)(((ulonglong)param_2 % (ulonglong)param_3 << 0x20 | (ulonglong)param_1) /
                 (ulonglong)param_3);
  }
  else {
    do {
      uVar5 = uVar8 >> 1;
      uVar9 = uVar9 >> 1 | (uint)((uVar8 & 1) != 0) << 0x1f;
      uVar7 = uVar6 >> 1;
      uVar3 = uVar3 >> 1 | (uint)((uVar6 & 1) != 0) << 0x1f;
      uVar8 = uVar5;
      uVar6 = uVar7;
    } while (uVar5 != 0);
    uVar1 = CONCAT44(uVar7,uVar3) / (ulonglong)uVar9;
    iVar4 = (int)uVar1;
    lVar2 = (ulonglong)param_3 * (uVar1 & 0xffffffff);
    uVar3 = (uint)((ulonglong)lVar2 >> 0x20);
    uVar8 = uVar3 + iVar4 * param_4;
    if (((CARRY4(uVar3,iVar4 * param_4)) || (param_2 < uVar8)) ||
       ((param_2 <= uVar8 && (param_1 < (uint)lVar2)))) {
      iVar4 = iVar4 + -1;
    }
    uVar3 = 0;
  }
  return CONCAT44(uVar3,iVar4);
}


// 10019895 FUN_10019895

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10019895(void)

{
  _DAT_1004abb8 = 0;
  return;
}


// 1001989d FID_conflict:__atoflt_l

/* Library Function - Multiple Matches With Different Base Names
    __atodbl_l
    __atoflt_l
   
   Library: Visual Studio 2005 Release */

int __cdecl FID_conflict___atoflt_l(_CRT_FLOAT *_Result,char *_Str,_locale_t _Locale)

{
  INTRNCVT_STATUS IVar1;
  int iVar2;
  int local_2c;
  _LocaleUpdate local_28 [8];
  int local_20;
  char local_1c;
  uint local_18;
  _LDBL12 local_14;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_10018277((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tod(&local_14,(_CRT_DOUBLE *)_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_100198f4:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10019934;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_10019926:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10019934;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_10019926;
    goto LAB_100198f4;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_10019934:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 10019943 FID_conflict:__atoflt_l

/* Library Function - Multiple Matches With Different Base Names
    __atodbl_l
    __atoflt_l
   
   Library: Visual Studio 2005 Release */

int __cdecl FID_conflict___atoflt_l(_CRT_FLOAT *_Result,char *_Str,_locale_t _Locale)

{
  INTRNCVT_STATUS IVar1;
  int iVar2;
  int local_2c;
  _LocaleUpdate local_28 [8];
  int local_20;
  char local_1c;
  uint local_18;
  _LDBL12 local_14;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_10018277((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tof(&local_14,_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_1001999a:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_100199da;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_100199cc:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_100199da;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_100199cc;
    goto LAB_1001999a;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_100199da:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 100199e9 _strcpy_s

/* Library Function - Single Match
    _strcpy_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _strcpy_s(char *_Dst,rsize_t _SizeInBytes,char *_Src)

{
  char cVar1;
  DWORD *pDVar2;
  char *pcVar3;
  errno_t eVar4;
  
  if ((_Dst != (char *)0x0) && (_SizeInBytes != 0)) {
    pcVar3 = _Dst;
    if (_Src != (char *)0x0) {
      do {
        cVar1 = *_Src;
        *pcVar3 = cVar1;
        _Src = _Src + 1;
        if (cVar1 == '\0') break;
        _SizeInBytes = _SizeInBytes - 1;
        pcVar3 = pcVar3 + 1;
      } while (_SizeInBytes != 0);
      if (_SizeInBytes != 0) {
        return 0;
      }
      *_Dst = '\0';
      pDVar2 = FUN_10011e63();
      eVar4 = 0x22;
      *pDVar2 = 0x22;
      goto LAB_10019a08;
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_10011e63();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_10019a08:
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 10019a4e __fptostr

/* Library Function - Single Match
    __fptostr
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __fptostr(char *_Buf,size_t _SizeInBytes,int _Digits,STRFLT _PtFlt)

{
  DWORD *pDVar1;
  int iVar2;
  char *pcVar3;
  size_t sVar4;
  char cVar5;
  char *pcVar6;
  errno_t eVar7;
  
  pcVar6 = _PtFlt->mantissa;
  if ((_Buf == (char *)0x0) || (_SizeInBytes == 0)) {
    pDVar1 = FUN_10011e63();
    eVar7 = 0x16;
    *pDVar1 = 0x16;
  }
  else {
    *_Buf = '\0';
    iVar2 = _Digits;
    if (_Digits < 1) {
      iVar2 = 0;
    }
    if (iVar2 + 1U < _SizeInBytes) {
      *_Buf = '0';
      pcVar3 = _Buf + 1;
      if (0 < _Digits) {
        do {
          cVar5 = *pcVar6;
          if (cVar5 == '\0') {
            cVar5 = '0';
          }
          else {
            pcVar6 = pcVar6 + 1;
          }
          *pcVar3 = cVar5;
          pcVar3 = pcVar3 + 1;
          _Digits = _Digits + -1;
        } while (0 < _Digits);
      }
      *pcVar3 = '\0';
      if ((-1 < _Digits) && ('4' < *pcVar6)) {
        while (pcVar3 = pcVar3 + -1, *pcVar3 == '9') {
          *pcVar3 = '0';
        }
        *pcVar3 = *pcVar3 + '\x01';
      }
      if (*_Buf == '1') {
        _PtFlt->decpt = _PtFlt->decpt + 1;
      }
      else {
        sVar4 = _strlen(_Buf + 1);
        _memmove(_Buf,_Buf + 1,sVar4 + 1);
      }
      return 0;
    }
    pDVar1 = FUN_10011e63();
    eVar7 = 0x22;
    *pDVar1 = 0x22;
  }
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar7;
}


// 10019b0b ___dtold

/* Library Function - Single Match
    ___dtold
   
   Library: Visual Studio 2005 Release */

void __cdecl ___dtold(uint *param_1,uint *param_2)

{
  uint uVar1;
  ushort uVar2;
  ushort uVar3;
  ushort uVar4;
  uint local_8;
  
  uVar2 = *(ushort *)((int)param_2 + 6) >> 4;
  uVar4 = *(ushort *)((int)param_2 + 6) & 0x8000;
  uVar3 = uVar2 & 0x7ff;
  uVar1 = *param_2;
  local_8 = 0x80000000;
  if ((uVar2 & 0x7ff) == 0) {
    if (((param_2[1] & 0xfffff) == 0) && (uVar1 == 0)) {
      param_1[1] = 0;
      *param_1 = 0;
      goto LAB_10019bbd;
    }
    uVar3 = uVar3 + 0x3c01;
    local_8 = 0;
  }
  else if (uVar3 == 0x7ff) {
    uVar3 = 0x7fff;
  }
  else {
    uVar3 = uVar3 + 0x3c00;
  }
  param_1[1] = uVar1 >> 0x15 | (param_2[1] & 0xfffff) << 0xb | local_8;
  *param_1 = uVar1 << 0xb;
  while (local_8 == 0) {
    uVar1 = param_1[1];
    uVar3 = uVar3 - 1;
    param_1[1] = uVar1 * 2 | *param_1 >> 0x1f;
    *param_1 = *param_1 * 2;
    local_8 = uVar1 * 2 & 0x80000000;
  }
  uVar4 = uVar4 | uVar3;
LAB_10019bbd:
  *(ushort *)(param_1 + 2) = uVar4;
  return;
}


// 10019bc6 __fltout2

/* Library Function - Single Match
    __fltout2
   
   Library: Visual Studio 2005 Release */

STRFLT __cdecl __fltout2(_CRT_DOUBLE _Dbl,STRFLT _Flt,char *_ResultStr,size_t _SizeInBytes)

{
  int iVar1;
  errno_t eVar2;
  STRFLT p_Var3;
  short local_30;
  char local_2e;
  char local_2c [24];
  uint local_14;
  uint uStack_10;
  ushort uStack_c;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  ___dtold(&local_14,(uint *)&_Dbl);
  iVar1 = FUN_1001d010(local_14,uStack_10,uStack_c,0x11,0,&local_30);
  _Flt->flag = iVar1;
  _Flt->sign = (int)local_2e;
  _Flt->decpt = (int)local_30;
  eVar2 = _strcpy_s(_ResultStr,_SizeInBytes,local_2c);
  if (eVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  _Flt->mantissa = _ResultStr;
  p_Var3 = (STRFLT)__security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return p_Var3;
}


// 10019c60 __alldvrm

/* Library Function - Single Match
    __alldvrm
   
   Library: Visual Studio 2005 Release */

undefined8 __alldvrm(uint param_1,uint param_2,uint param_3,uint param_4)

{
  ulonglong uVar1;
  longlong lVar2;
  uint uVar3;
  int iVar4;
  uint uVar5;
  uint uVar6;
  uint uVar7;
  uint uVar8;
  bool bVar10;
  char cVar11;
  uint uVar9;
  
  cVar11 = (int)param_2 < 0;
  if ((bool)cVar11) {
    bVar10 = param_1 != 0;
    param_1 = -param_1;
    param_2 = -(uint)bVar10 - param_2;
  }
  if ((int)param_4 < 0) {
    cVar11 = cVar11 + '\x01';
    bVar10 = param_3 != 0;
    param_3 = -param_3;
    param_4 = -(uint)bVar10 - param_4;
  }
  uVar3 = param_1;
  uVar5 = param_3;
  uVar6 = param_2;
  uVar9 = param_4;
  if (param_4 == 0) {
    uVar3 = param_2 / param_3;
    iVar4 = (int)(((ulonglong)param_2 % (ulonglong)param_3 << 0x20 | (ulonglong)param_1) /
                 (ulonglong)param_3);
  }
  else {
    do {
      uVar8 = uVar9 >> 1;
      uVar5 = uVar5 >> 1 | (uint)((uVar9 & 1) != 0) << 0x1f;
      uVar7 = uVar6 >> 1;
      uVar3 = uVar3 >> 1 | (uint)((uVar6 & 1) != 0) << 0x1f;
      uVar6 = uVar7;
      uVar9 = uVar8;
    } while (uVar8 != 0);
    uVar1 = CONCAT44(uVar7,uVar3) / (ulonglong)uVar5;
    iVar4 = (int)uVar1;
    lVar2 = (ulonglong)param_3 * (uVar1 & 0xffffffff);
    uVar3 = (uint)((ulonglong)lVar2 >> 0x20);
    uVar5 = uVar3 + iVar4 * param_4;
    if (((CARRY4(uVar3,iVar4 * param_4)) || (param_2 < uVar5)) ||
       ((param_2 <= uVar5 && (param_1 < (uint)lVar2)))) {
      iVar4 = iVar4 + -1;
    }
    uVar3 = 0;
  }
  if (cVar11 == '\x01') {
    bVar10 = iVar4 != 0;
    iVar4 = -iVar4;
    uVar3 = -(uint)bVar10 - uVar3;
  }
  return CONCAT44(uVar3,iVar4);
}


// 10019d40 __aullshr

/* Library Function - Single Match
    __aullshr
   
   Library: Visual Studio 2005 Release */

ulonglong __fastcall __aullshr(byte param_1,uint param_2)

{
  uint in_EAX;
  
  if (0x3f < param_1) {
    return 0;
  }
  if (param_1 < 0x20) {
    return CONCAT44(param_2 >> (param_1 & 0x1f),
                    in_EAX >> (param_1 & 0x1f) | param_2 << 0x20 - (param_1 & 0x1f));
  }
  return (ulonglong)(param_2 >> (param_1 & 0x1f));
}


// 10019d68 __controlfp_s

/* Library Function - Single Match
    __controlfp_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __controlfp_s(uint *_CurrentState,uint _NewValue,uint _Mask)

{
  uint uVar1;
  DWORD *pDVar2;
  errno_t eVar3;
  
  uVar1 = _Mask & 0xfff7ffff;
  if ((_NewValue & uVar1 & 0xfcf0fce0) == 0) {
    if (_CurrentState == (uint *)0x0) {
      __control87(_NewValue,uVar1);
    }
    else {
      uVar1 = __control87(_NewValue,uVar1);
      *_CurrentState = uVar1;
    }
    eVar3 = 0;
  }
  else {
    if (_CurrentState != (uint *)0x0) {
      uVar1 = __control87(0,0);
      *_CurrentState = uVar1;
    }
    pDVar2 = FUN_10011e63();
    eVar3 = 0x16;
    *pDVar2 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return eVar3;
}


// 10019dd2 __initp_misc_winsig

/* Library Function - Single Match
    __initp_misc_winsig
   
   Library: Visual Studio 2005 Release */

void __cdecl __initp_misc_winsig(undefined4 param_1)

{
  DAT_1003a4a0 = param_1;
  DAT_1003a4a4 = param_1;
  DAT_1003a4a8 = param_1;
  DAT_1003a4ac = param_1;
  return;
}


// 10019deb siglookup

/* Library Function - Single Match
    _siglookup
   
   Library: Visual Studio 2005 Release */

uint __cdecl siglookup(uint param_1)

{
  uint uVar1;
  int in_EDX;
  
  uVar1 = param_1;
  do {
    if (*(int *)(uVar1 + 4) == in_EDX) break;
    uVar1 = uVar1 + 0xc;
  } while (uVar1 < DAT_100366a4 * 0xc + param_1);
  if ((DAT_100366a4 * 0xc + param_1 <= uVar1) || (*(int *)(uVar1 + 4) != in_EDX)) {
    uVar1 = 0;
  }
  return uVar1;
}


// 10019e1f ___get_sigabrt

/* Library Function - Single Match
    ___get_sigabrt
   
   Library: Visual Studio 2005 Release */

_PHNDLR __cdecl ___get_sigabrt(void)

{
  _PHNDLR p_Var1;
  
  p_Var1 = (_PHNDLR)FUN_10013035(DAT_1003a4a8);
  return p_Var1;
}


// 10019e2c _raise

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _raise
   
   Library: Visual Studio 2005 Release */

int __cdecl _raise(int _SigNum)

{
  bool bVar1;
  uint uVar2;
  int iVar3;
  code *pcVar4;
  undefined4 uVar5;
  undefined4 *puVar6;
  DWORD *pDVar7;
  DWORD local_34;
  DWORD local_30;
  int local_28;
  
  pDVar7 = (DWORD *)0x0;
  bVar1 = false;
  if (_SigNum < 0xc) {
    if (_SigNum != 0xb) {
      if (_SigNum == 2) {
        puVar6 = &DAT_1003a4a0;
        iVar3 = DAT_1003a4a0;
        goto LAB_10019ee1;
      }
      if (_SigNum != 4) {
        if (_SigNum == 6) goto LAB_10019ebf;
        if (_SigNum != 8) goto LAB_10019ea3;
      }
    }
    pDVar7 = FUN_100131da();
    if (pDVar7 == (DWORD *)0x0) {
      return -1;
    }
    uVar2 = siglookup(pDVar7[0x17]);
    puVar6 = (undefined4 *)(uVar2 + 8);
    pcVar4 = (code *)*puVar6;
  }
  else {
    if (_SigNum == 0xf) {
      puVar6 = &DAT_1003a4ac;
      iVar3 = DAT_1003a4ac;
    }
    else if (_SigNum == 0x15) {
      puVar6 = &DAT_1003a4a4;
      iVar3 = DAT_1003a4a4;
    }
    else {
      if (_SigNum != 0x16) {
LAB_10019ea3:
        pDVar7 = FUN_10011e63();
        *pDVar7 = 0x16;
        FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return -1;
      }
LAB_10019ebf:
      puVar6 = &DAT_1003a4a8;
      iVar3 = DAT_1003a4a8;
    }
LAB_10019ee1:
    bVar1 = true;
    pcVar4 = (code *)FUN_10013035(iVar3);
  }
  if (pcVar4 == (code *)0x1) {
    return 0;
  }
  if (pcVar4 == (code *)0x0) {
                    /* WARNING: Subroutine does not return */
    __exit(3);
  }
  if (bVar1) {
    __lock(0);
  }
  if (((_SigNum == 8) || (_SigNum == 0xb)) || (_SigNum == 4)) {
    local_30 = pDVar7[0x18];
    pDVar7[0x18] = 0;
    if (_SigNum == 8) {
      local_34 = pDVar7[0x19];
      pDVar7[0x19] = 0x8c;
      goto LAB_10019f45;
    }
  }
  else {
LAB_10019f45:
    if (_SigNum == 8) {
      for (local_28 = DAT_10036698; local_28 < DAT_1003669c + DAT_10036698; local_28 = local_28 + 1)
      {
        *(undefined4 *)(local_28 * 0xc + 8 + pDVar7[0x17]) = 0;
      }
      goto LAB_10019f7f;
    }
  }
  uVar5 = __encoded_null();
  *puVar6 = uVar5;
LAB_10019f7f:
  FUN_10019fa0();
  if (_SigNum == 8) {
    (*pcVar4)(8,pDVar7[0x19]);
  }
  else {
    (*pcVar4)(_SigNum);
    if ((_SigNum != 0xb) && (_SigNum != 4)) {
      return 0;
    }
  }
  pDVar7[0x18] = local_30;
  if (_SigNum == 8) {
    pDVar7[0x19] = local_34;
  }
  return 0;
}


// 10019fa0 FUN_10019fa0

void FUN_10019fa0(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + -0x1c) != 0) {
    FUN_100189ed(0);
  }
  return;
}


// 10019fdc FUN_10019fdc

undefined4 __cdecl FUN_10019fdc(undefined4 param_1,undefined4 param_2,uint param_3)

{
  HMODULE hModule;
  FARPROC pFVar1;
  int iVar2;
  code *pcVar3;
  code *pcVar4;
  undefined4 uVar5;
  undefined1 local_24 [8];
  byte local_1c;
  undefined1 local_18 [4];
  uint local_14;
  int local_10;
  int local_c;
  int local_8;
  
  local_c = __encoded_null();
  local_8 = 0;
  local_10 = 0;
  local_14 = 0;
  if (DAT_1003a4b4 == 0) {
    hModule = LoadLibraryA("USER32.DLL");
    if (hModule == (HMODULE)0x0) {
      return 0;
    }
    pFVar1 = GetProcAddress(hModule,"MessageBoxA");
    if (pFVar1 == (FARPROC)0x0) {
      return 0;
    }
    DAT_1003a4b4 = FUN_10012fbe((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetActiveWindow");
    DAT_1003a4b8 = FUN_10012fbe((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetLastActivePopup");
    DAT_1003a4bc = FUN_10012fbe((int)pFVar1);
    iVar2 = __get_osplatform(&local_10);
    if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
    if (local_10 == 2) {
      pFVar1 = GetProcAddress(hModule,"GetUserObjectInformationA");
      DAT_1003a4c4 = FUN_10012fbe((int)pFVar1);
      if (DAT_1003a4c4 != 0) {
        pFVar1 = GetProcAddress(hModule,"GetProcessWindowStation");
        DAT_1003a4c0 = FUN_10012fbe((int)pFVar1);
      }
    }
  }
  if ((DAT_1003a4c0 != local_c) && (DAT_1003a4c4 != local_c)) {
    pcVar3 = (code *)FUN_10013035(DAT_1003a4c0);
    pcVar4 = (code *)FUN_10013035(DAT_1003a4c4);
    if (((pcVar3 != (code *)0x0) && (pcVar4 != (code *)0x0)) &&
       (((iVar2 = (*pcVar3)(), iVar2 == 0 ||
         (iVar2 = (*pcVar4)(iVar2,1,local_24,0xc,local_18), iVar2 == 0)) || ((local_1c & 1) == 0))))
    {
      iVar2 = FUN_10013661(&local_14);
      if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      if (local_14 < 4) {
        param_3 = param_3 | 0x40000;
      }
      else {
        param_3 = param_3 | 0x200000;
      }
      goto LAB_1001a172;
    }
  }
  if ((((DAT_1003a4b8 != local_c) &&
       (pcVar3 = (code *)FUN_10013035(DAT_1003a4b8), pcVar3 != (code *)0x0)) &&
      (local_8 = (*pcVar3)(), local_8 != 0)) &&
     ((DAT_1003a4bc != local_c &&
      (pcVar3 = (code *)FUN_10013035(DAT_1003a4bc), pcVar3 != (code *)0x0)))) {
    local_8 = (*pcVar3)(local_8);
  }
LAB_1001a172:
  pcVar3 = (code *)FUN_10013035(DAT_1003a4b4);
  if (pcVar3 == (code *)0x0) {
    return 0;
  }
  uVar5 = (*pcVar3)(local_8,param_1,param_2,param_3);
  return uVar5;
}


// 1001a199 _strcat_s

/* Library Function - Single Match
    _strcat_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _strcat_s(char *_Dst,rsize_t _SizeInBytes,char *_Src)

{
  char cVar1;
  DWORD *pDVar2;
  char *pcVar3;
  errno_t eVar4;
  
  if ((_Dst != (char *)0x0) && (_SizeInBytes != 0)) {
    pcVar3 = _Dst;
    if (_Src != (char *)0x0) {
      do {
        if (*pcVar3 == '\0') break;
        pcVar3 = pcVar3 + 1;
        _SizeInBytes = _SizeInBytes - 1;
      } while (_SizeInBytes != 0);
      if (_SizeInBytes != 0) {
        do {
          cVar1 = *_Src;
          *pcVar3 = cVar1;
          pcVar3 = pcVar3 + 1;
          _Src = _Src + 1;
          if (cVar1 == '\0') break;
          _SizeInBytes = _SizeInBytes - 1;
        } while (_SizeInBytes != 0);
        if (_SizeInBytes != 0) {
          return 0;
        }
        *_Dst = '\0';
        pDVar2 = FUN_10011e63();
        eVar4 = 0x22;
        *pDVar2 = 0x22;
        goto LAB_1001a1b8;
      }
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_10011e63();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_1001a1b8:
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 1001a20a _strncpy_s

/* Library Function - Single Match
    _strncpy_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _strncpy_s(char *_Dst,rsize_t _SizeInBytes,char *_Src,rsize_t _MaxCount)

{
  char cVar1;
  DWORD *pDVar2;
  char *pcVar3;
  rsize_t rVar4;
  errno_t eVar5;
  
  if (_MaxCount == 0) {
    if (_Dst == (char *)0x0) {
      if (_SizeInBytes == 0) {
        return 0;
      }
    }
    else {
LAB_1001a22e:
      if (_SizeInBytes != 0) {
        if (_MaxCount == 0) {
          *_Dst = '\0';
          return 0;
        }
        if (_Src != (char *)0x0) {
          pcVar3 = _Dst;
          rVar4 = _SizeInBytes;
          if (_MaxCount == 0xffffffff) {
            do {
              cVar1 = *_Src;
              *pcVar3 = cVar1;
              pcVar3 = pcVar3 + 1;
              _Src = _Src + 1;
              if (cVar1 == '\0') break;
              rVar4 = rVar4 - 1;
            } while (rVar4 != 0);
          }
          else {
            do {
              cVar1 = *_Src;
              *pcVar3 = cVar1;
              pcVar3 = pcVar3 + 1;
              _Src = _Src + 1;
              if ((cVar1 == '\0') || (rVar4 = rVar4 - 1, rVar4 == 0)) break;
              _MaxCount = _MaxCount - 1;
            } while (_MaxCount != 0);
            if (_MaxCount == 0) {
              *pcVar3 = '\0';
            }
          }
          if (rVar4 != 0) {
            return 0;
          }
          if (_MaxCount == 0xffffffff) {
            _Dst[_SizeInBytes - 1] = '\0';
            return 0x50;
          }
          *_Dst = '\0';
          pDVar2 = FUN_10011e63();
          eVar5 = 0x22;
          *pDVar2 = 0x22;
          goto LAB_1001a23f;
        }
        *_Dst = '\0';
      }
    }
  }
  else if (_Dst != (char *)0x0) goto LAB_1001a22e;
  pDVar2 = FUN_10011e63();
  eVar5 = 0x16;
  *pDVar2 = 0x16;
LAB_1001a23f:
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar5;
}


// 1001a2bd __set_error_mode

/* Library Function - Single Match
    __set_error_mode
   
   Library: Visual Studio 2005 Release */

int __cdecl __set_error_mode(int _Mode)

{
  int iVar1;
  DWORD *pDVar2;
  
  if (-1 < _Mode) {
    if (_Mode < 3) {
      iVar1 = DAT_10039e24;
      DAT_10039e24 = _Mode;
      return iVar1;
    }
    if (_Mode == 3) {
      return DAT_10039e24;
    }
  }
  pDVar2 = FUN_10011e63();
  *pDVar2 = 0x16;
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1001a310 __ValidateImageBase

/* Library Function - Single Match
    __ValidateImageBase
   
   Library: Visual Studio 2005 Release */

BOOL __cdecl __ValidateImageBase(PBYTE pImageBase)

{
  if ((*(short *)pImageBase == 0x5a4d) &&
     (*(int *)(pImageBase + *(int *)(pImageBase + 0x3c)) == 0x4550)) {
    return (uint)((short)*(int *)((int)(pImageBase + *(int *)(pImageBase + 0x3c)) + 0x18) == 0x10b);
  }
  return 0;
}


// 1001a340 __FindPESection

/* Library Function - Single Match
    __FindPESection
   
   Library: Visual Studio 2005 Release */

PIMAGE_SECTION_HEADER __cdecl __FindPESection(PBYTE pImageBase,DWORD_PTR rva)

{
  int iVar1;
  PIMAGE_SECTION_HEADER p_Var2;
  uint uVar3;
  
  iVar1 = *(int *)(pImageBase + 0x3c);
  uVar3 = 0;
  p_Var2 = (PIMAGE_SECTION_HEADER)
           (pImageBase + *(ushort *)(pImageBase + iVar1 + 0x14) + 0x18 + iVar1);
  if (*(ushort *)(pImageBase + iVar1 + 6) != 0) {
    do {
      if ((p_Var2->VirtualAddress <= rva) &&
         (rva < (p_Var2->Misc).PhysicalAddress + p_Var2->VirtualAddress)) {
        return p_Var2;
      }
      uVar3 = uVar3 + 1;
      p_Var2 = p_Var2 + 1;
    } while (uVar3 < *(ushort *)(pImageBase + iVar1 + 6));
  }
  return (PIMAGE_SECTION_HEADER)0x0;
}


// 1001a390 __IsNonwritableInCurrentImage

/* Library Function - Single Match
    __IsNonwritableInCurrentImage
   
   Library: Visual Studio */

BOOL __cdecl __IsNonwritableInCurrentImage(PBYTE pTarget)

{
  uint uVar1;
  BOOL BVar2;
  PIMAGE_SECTION_HEADER p_Var3;
  int *unaff_FS_OFFSET;
  int local_14;
  code *pcStack_10;
  uint local_c;
  undefined4 local_8;
  
  pcStack_10 = __except_handler4;
  local_14 = *unaff_FS_OFFSET;
  local_c = DAT_100366b8 ^ 0x10032888;
  *unaff_FS_OFFSET = (int)&local_14;
  local_8 = 0;
  BVar2 = __ValidateImageBase((PBYTE)&IMAGE_DOS_HEADER_10000000);
  if (BVar2 != 0) {
    p_Var3 = __FindPESection((PBYTE)&IMAGE_DOS_HEADER_10000000,(DWORD_PTR)(pTarget + -0x10000000));
    if (p_Var3 != (PIMAGE_SECTION_HEADER)0x0) {
      uVar1 = p_Var3->Characteristics;
      *unaff_FS_OFFSET = local_14;
      return ~(uVar1 >> 0x1f) & 1;
    }
  }
  *unaff_FS_OFFSET = local_14;
  return 0;
}


// 1001a484 __initp_eh_hooks

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __initp_eh_hooks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __initp_eh_hooks(void)

{
  _DAT_1003a4c8 = FUN_10012fbe(0x1001a44b);
  return;
}


// 1001a495 FUN_1001a495

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1001a495(undefined4 param_1)

{
  _DAT_1003a4cc = param_1;
  return;
}


// 1001a49f FUN_1001a49f

void __cdecl FUN_1001a49f(undefined4 param_1)

{
  DAT_1003a4d0 = param_1;
  return;
}


// 1001a4a9 ___crtInitCritSecNoSpinCount@8

/* Library Function - Single Match
    ___crtInitCritSecNoSpinCount@8
   
   Library: Visual Studio 2005 Release */

undefined4 ___crtInitCritSecNoSpinCount_8(LPCRITICAL_SECTION param_1)

{
  InitializeCriticalSection(param_1);
  return 1;
}


// 1001a4b9 ___crtInitCritSecAndSpinCount

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___crtInitCritSecAndSpinCount
   
   Library: Visual Studio 2005 Release */

int __cdecl ___crtInitCritSecAndSpinCount(undefined4 param_1,undefined4 param_2)

{
  code *pcVar1;
  int iVar2;
  HMODULE hModule;
  int local_20 [5];
  undefined4 uStack_c;
  undefined *local_8;
  
  local_8 = &DAT_100328c8;
  uStack_c = 0x1001a4c5;
  local_20[0] = 0;
  pcVar1 = (code *)FUN_10013035(DAT_1003a4d0);
  if (pcVar1 != (FARPROC)0x0) goto LAB_1001a52f;
  iVar2 = __get_osplatform(local_20);
  if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  if (local_20[0] == 1) {
LAB_1001a51e:
    pcVar1 = ___crtInitCritSecNoSpinCount_8;
  }
  else {
    hModule = GetModuleHandleA("kernel32.dll");
    if (hModule == (HMODULE)0x0) goto LAB_1001a51e;
    pcVar1 = GetProcAddress(hModule,"InitializeCriticalSectionAndSpinCount");
    if (pcVar1 == (FARPROC)0x0) goto LAB_1001a51e;
  }
  DAT_1003a4d0 = FUN_10012fbe((int)pcVar1);
LAB_1001a52f:
  local_8 = (undefined *)0x0;
  iVar2 = (*pcVar1)(param_1,param_2);
  return iVar2;
}


// 1001a57e FUN_1001a57e

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1001a57e(undefined4 param_1)

{
  _DAT_1003a4d4 = param_1;
  return;
}


// 1001a588 __local_unwind4

/* Library Function - Single Match
    __local_unwind4
   
   Libraries: Visual Studio 2017 Debug, Visual Studio 2017 Release, Visual Studio 2019 Debug, Visual
   Studio 2019 Release */

void __cdecl __local_unwind4(uint *param_1,int param_2,uint param_3)

{
  undefined4 *puVar1;
  uint uVar2;
  undefined4 *unaff_FS_OFFSET;
  undefined4 uStack_28;
  undefined1 *puStack_24;
  uint local_20;
  uint uStack_1c;
  int iStack_18;
  uint *puStack_14;
  
  puStack_14 = param_1;
  iStack_18 = param_2;
  uStack_1c = param_3;
  puStack_24 = &LAB_1001a618;
  uStack_28 = *unaff_FS_OFFSET;
  local_20 = DAT_100366b8 ^ (uint)&uStack_28;
  *unaff_FS_OFFSET = &uStack_28;
  while( true ) {
    uVar2 = *(uint *)(param_2 + 0xc);
    if ((uVar2 == 0xfffffffe) || ((param_3 != 0xfffffffe && (uVar2 <= param_3)))) break;
    puVar1 = (undefined4 *)((*(uint *)(param_2 + 8) ^ *param_1) + 0x10 + uVar2 * 0xc);
    *(undefined4 *)(param_2 + 0xc) = *puVar1;
    if (puVar1[1] == 0) {
      __NLG_Notify(0x101);
      FUN_1001de3c();
    }
  }
  *unaff_FS_OFFSET = uStack_28;
  return;
}


// 1001a65e FUN_1001a65e

void FUN_1001a65e(int param_1)

{
  __local_unwind4(*(uint **)(param_1 + 0x28),*(int *)(param_1 + 0x18),*(uint *)(param_1 + 0x1c));
  return;
}


// 1001a67a _EH4_CallFilterFunc

/* Library Function - Single Match
    @_EH4_CallFilterFunc@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_CallFilterFunc,8 */

void __fastcall _EH4_CallFilterFunc(undefined *param_1)

{
  (*(code *)param_1)();
  return;
}


// 1001a691 _EH4_TransferToHandler

/* Library Function - Single Match
    @_EH4_TransferToHandler@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_TransferToHandler,8 */

void __fastcall _EH4_TransferToHandler(undefined *UNRECOVERED_JUMPTABLE)

{
  __NLG_Notify(1);
                    /* WARNING: Could not recover jumptable at 0x1001a6a8. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (*(code *)UNRECOVERED_JUMPTABLE)();
  return;
}


// 1001a6aa _EH4_GlobalUnwind

/* Library Function - Single Match
    @_EH4_GlobalUnwind@4
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_GlobalUnwind,4 */

void __fastcall _EH4_GlobalUnwind(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x1001a6bf,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 1001a6c4 _EH4_LocalUnwind

/* Library Function - Single Match
    @_EH4_LocalUnwind@16
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_LocalUnwind,16 */

void __fastcall _EH4_LocalUnwind(int param_1,uint param_2,undefined4 param_3,uint *param_4)

{
  __local_unwind4(param_4,param_1,param_2);
  return;
}


// 1001a6db __crtGetStringTypeA_stat

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* Library Function - Single Match
    int __cdecl __crtGetStringTypeA_stat(struct localeinfo_struct *,unsigned long,char const
   *,int,unsigned short *,int,int,int)
   
   Library: Visual Studio 2005 Release */

int __cdecl
__crtGetStringTypeA_stat
          (localeinfo_struct *param_1,ulong param_2,char *param_3,int param_4,ushort *param_5,
          int param_6,int param_7,int param_8)

{
  uint uVar1;
  BOOL BVar2;
  DWORD DVar3;
  uint cchWideChar;
  LPCWSTR pWVar4;
  int iVar5;
  ushort *puVar6;
  int *in_ECX;
  LPCWSTR lpWideCharStr;
  ulong uVar7;
  int *local_c;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  local_c = in_ECX;
  if (DAT_1003a4d8 == 0) {
    BVar2 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_1003a4d8 = 2;
      }
      goto LAB_1001a734;
    }
    DAT_1003a4d8 = 1;
  }
  else {
LAB_1001a734:
    if ((DAT_1003a4d8 == 2) || (DAT_1003a4d8 == 0)) {
      uVar7 = 0;
      if (param_6 == 0) {
        param_6 = *(int *)(*in_ECX + 0x14);
      }
      if (param_5 == (ushort *)0x0) {
        param_5 = *(ushort **)(*in_ECX + 4);
      }
      puVar6 = (ushort *)___ansicp(param_6);
      if ((puVar6 != (ushort *)0xffffffff) &&
         (((puVar6 == param_5 ||
           (uVar7 = ___convertcp((UINT)param_5,(UINT)puVar6,(char *)param_2,(uint *)&param_3,
                                 (LPSTR)0x0,0), param_2 = uVar7, uVar7 != 0)) &&
          (GetStringTypeA(param_6,(DWORD)param_1,(LPCSTR)param_2,(int)param_3,(LPWORD)param_4),
          uVar7 != 0)))) {
        FUN_100012d0(uVar7);
      }
      goto LAB_1001a881;
    }
    if (DAT_1003a4d8 != 1) goto LAB_1001a881;
  }
  local_c = (int *)0x0;
  if (param_5 == (ushort *)0x0) {
    param_5 = *(ushort **)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar((UINT)param_5,(uint)(param_7 != 0) * 8 + 1,(LPCSTR)param_2,
                                    (int)param_3,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_1001a881;
  lpWideCharStr = (LPCWSTR)0x0;
  if ((0 < (int)cchWideChar) && (cchWideChar < 0x7ffffff1)) {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar4 = (LPCWSTR)&stack0xffffffe8;
      lpWideCharStr = (LPCWSTR)&stack0xffffffe8;
      if (&stack0x00000000 != (undefined1 *)0x18) {
LAB_1001a7c4:
        lpWideCharStr = pWVar4 + 4;
      }
    }
    else {
      pWVar4 = (LPCWSTR)FUN_10001270(uVar1);
      lpWideCharStr = pWVar4;
      if (pWVar4 != (LPCWSTR)0x0) {
        pWVar4[0] = L'\xdddd';
        pWVar4[1] = L'\0';
        goto LAB_1001a7c4;
      }
    }
  }
  if (lpWideCharStr != (LPCWSTR)0x0) {
    _memset(lpWideCharStr,0,cchWideChar * 2);
    iVar5 = MultiByteToWideChar((UINT)param_5,1,(LPCSTR)param_2,(int)param_3,lpWideCharStr,
                                cchWideChar);
    if (iVar5 != 0) {
      local_c = (int *)GetStringTypeW((DWORD)param_1,lpWideCharStr,iVar5,(LPWORD)param_4);
    }
    __freea(lpWideCharStr);
  }
LAB_1001a881:
  iVar5 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar5;
}


// 1001a893 ___crtGetStringTypeA

/* Library Function - Single Match
    ___crtGetStringTypeA
   
   Library: Visual Studio 2005 Release */

BOOL __cdecl
___crtGetStringTypeA
          (_locale_t _Plocinfo,DWORD _DWInfoType,LPCSTR _LpSrcStr,int _CchSrc,LPWORD _LpCharType,
          int _Code_page,BOOL _BError)

{
  int iVar1;
  int in_stack_00000020;
  int in_stack_ffffffec;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&stack0xffffffec,_Plocinfo);
  iVar1 = __crtGetStringTypeA_stat
                    ((localeinfo_struct *)_DWInfoType,(ulong)_LpSrcStr,(char *)_CchSrc,
                     (int)_LpCharType,(ushort *)_Code_page,_BError,in_stack_00000020,
                     in_stack_ffffffec);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return iVar1;
}


// 1001a8d3 ___free_lc_time

/* Library Function - Single Match
    ___free_lc_time
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lc_time(undefined4 *param_1)

{
  if (param_1 != (undefined4 *)0x0) {
    FUN_100012d0(param_1[1]);
    FUN_100012d0(param_1[2]);
    FUN_100012d0(param_1[3]);
    FUN_100012d0(param_1[4]);
    FUN_100012d0(param_1[5]);
    FUN_100012d0(param_1[6]);
    FUN_100012d0(*param_1);
    FUN_100012d0(param_1[8]);
    FUN_100012d0(param_1[9]);
    FUN_100012d0(param_1[10]);
    FUN_100012d0(param_1[0xb]);
    FUN_100012d0(param_1[0xc]);
    FUN_100012d0(param_1[0xd]);
    FUN_100012d0(param_1[7]);
    FUN_100012d0(param_1[0xe]);
    FUN_100012d0(param_1[0xf]);
    FUN_100012d0(param_1[0x10]);
    FUN_100012d0(param_1[0x11]);
    FUN_100012d0(param_1[0x12]);
    FUN_100012d0(param_1[0x13]);
    FUN_100012d0(param_1[0x14]);
    FUN_100012d0(param_1[0x15]);
    FUN_100012d0(param_1[0x16]);
    FUN_100012d0(param_1[0x17]);
    FUN_100012d0(param_1[0x18]);
    FUN_100012d0(param_1[0x19]);
    FUN_100012d0(param_1[0x1a]);
    FUN_100012d0(param_1[0x1b]);
    FUN_100012d0(param_1[0x1c]);
    FUN_100012d0(param_1[0x1d]);
    FUN_100012d0(param_1[0x1e]);
    FUN_100012d0(param_1[0x1f]);
    FUN_100012d0(param_1[0x20]);
    FUN_100012d0(param_1[0x21]);
    FUN_100012d0(param_1[0x22]);
    FUN_100012d0(param_1[0x23]);
    FUN_100012d0(param_1[0x24]);
    FUN_100012d0(param_1[0x25]);
    FUN_100012d0(param_1[0x26]);
    FUN_100012d0(param_1[0x27]);
    FUN_100012d0(param_1[0x28]);
    FUN_100012d0(param_1[0x29]);
    FUN_100012d0(param_1[0x2a]);
  }
  return;
}


// 1001aa63 ___free_lconv_num

/* Library Function - Single Match
    ___free_lconv_num
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_num(int *param_1)

{
  if (param_1 != (int *)0x0) {
    if ((undefined *)*param_1 != PTR_DAT_10036b38) {
      FUN_100012d0((undefined *)*param_1);
    }
    if ((undefined *)param_1[1] != PTR_DAT_10036b3c) {
      FUN_100012d0((undefined *)param_1[1]);
    }
    if ((undefined *)param_1[2] != PTR_DAT_10036b40) {
      FUN_100012d0((undefined *)param_1[2]);
    }
  }
  return;
}


// 1001aaa3 ___free_lconv_mon

/* Library Function - Single Match
    ___free_lconv_mon
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_mon(int param_1)

{
  if (param_1 != 0) {
    if (*(undefined **)(param_1 + 0xc) != PTR_DAT_10036b44) {
      FUN_100012d0(*(undefined **)(param_1 + 0xc));
    }
    if (*(undefined **)(param_1 + 0x10) != PTR_DAT_10036b48) {
      FUN_100012d0(*(undefined **)(param_1 + 0x10));
    }
    if (*(undefined **)(param_1 + 0x14) != PTR_DAT_10036b4c) {
      FUN_100012d0(*(undefined **)(param_1 + 0x14));
    }
    if (*(undefined **)(param_1 + 0x18) != PTR_DAT_10036b50) {
      FUN_100012d0(*(undefined **)(param_1 + 0x18));
    }
    if (*(undefined **)(param_1 + 0x1c) != PTR_DAT_10036b54) {
      FUN_100012d0(*(undefined **)(param_1 + 0x1c));
    }
    if (*(undefined **)(param_1 + 0x20) != PTR_DAT_10036b58) {
      FUN_100012d0(*(undefined **)(param_1 + 0x20));
    }
    if (*(undefined **)(param_1 + 0x24) != PTR_DAT_10036b5c) {
      FUN_100012d0(*(undefined **)(param_1 + 0x24));
    }
  }
  return;
}


// 1001ab30 _strcspn

/* Library Function - Single Match
    _strcspn
   
   Library: Visual Studio */

size_t __cdecl _strcspn(char *_Str,char *_Control)

{
  byte bVar1;
  size_t sVar2;
  byte abStack_28 [32];
  
  abStack_28[0x1c] = 0;
  abStack_28[0x1d] = 0;
  abStack_28[0x1e] = 0;
  abStack_28[0x1f] = 0;
  abStack_28[0x18] = 0;
  abStack_28[0x19] = 0;
  abStack_28[0x1a] = 0;
  abStack_28[0x1b] = 0;
  abStack_28[0x14] = 0;
  abStack_28[0x15] = 0;
  abStack_28[0x16] = 0;
  abStack_28[0x17] = 0;
  abStack_28[0x10] = 0;
  abStack_28[0x11] = 0;
  abStack_28[0x12] = 0;
  abStack_28[0x13] = 0;
  abStack_28[0xc] = 0;
  abStack_28[0xd] = 0;
  abStack_28[0xe] = 0;
  abStack_28[0xf] = 0;
  abStack_28[8] = 0;
  abStack_28[9] = 0;
  abStack_28[10] = 0;
  abStack_28[0xb] = 0;
  abStack_28[4] = 0;
  abStack_28[5] = 0;
  abStack_28[6] = 0;
  abStack_28[7] = 0;
  abStack_28[0] = 0;
  abStack_28[1] = 0;
  abStack_28[2] = 0;
  abStack_28[3] = 0;
  while( true ) {
    bVar1 = *_Control;
    if (bVar1 == 0) break;
    _Control = (char *)((byte *)_Control + 1);
    abStack_28[(int)(uint)bVar1 >> 3] = abStack_28[(int)(uint)bVar1 >> 3] | '\x01' << (bVar1 & 7);
  }
  sVar2 = 0xffffffff;
  do {
    sVar2 = sVar2 + 1;
    bVar1 = *_Str;
    if (bVar1 == 0) {
      return sVar2;
    }
    _Str = (char *)((byte *)_Str + 1);
  } while ((abStack_28[(int)(uint)bVar1 >> 3] >> (bVar1 & 7) & 1) == 0);
  return sVar2;
}


// 1001ab80 _strpbrk

/* Library Function - Single Match
    _strpbrk
   
   Library: Visual Studio */

char * __cdecl _strpbrk(char *_Str,char *_Control)

{
  byte bVar1;
  byte *pbVar2;
  byte abStack_28 [32];
  
  abStack_28[0x1c] = 0;
  abStack_28[0x1d] = 0;
  abStack_28[0x1e] = 0;
  abStack_28[0x1f] = 0;
  abStack_28[0x18] = 0;
  abStack_28[0x19] = 0;
  abStack_28[0x1a] = 0;
  abStack_28[0x1b] = 0;
  abStack_28[0x14] = 0;
  abStack_28[0x15] = 0;
  abStack_28[0x16] = 0;
  abStack_28[0x17] = 0;
  abStack_28[0x10] = 0;
  abStack_28[0x11] = 0;
  abStack_28[0x12] = 0;
  abStack_28[0x13] = 0;
  abStack_28[0xc] = 0;
  abStack_28[0xd] = 0;
  abStack_28[0xe] = 0;
  abStack_28[0xf] = 0;
  abStack_28[8] = 0;
  abStack_28[9] = 0;
  abStack_28[10] = 0;
  abStack_28[0xb] = 0;
  abStack_28[4] = 0;
  abStack_28[5] = 0;
  abStack_28[6] = 0;
  abStack_28[7] = 0;
  abStack_28[0] = 0;
  abStack_28[1] = 0;
  abStack_28[2] = 0;
  abStack_28[3] = 0;
  while( true ) {
    bVar1 = *_Control;
    if (bVar1 == 0) break;
    _Control = (char *)((byte *)_Control + 1);
    abStack_28[(int)(uint)bVar1 >> 3] = abStack_28[(int)(uint)bVar1 >> 3] | '\x01' << (bVar1 & 7);
  }
  do {
    pbVar2 = (byte *)_Str;
    bVar1 = *pbVar2;
    if (bVar1 == 0) {
      return (char *)(uint)bVar1;
    }
    _Str = (char *)(pbVar2 + 1);
  } while ((abStack_28[(int)(uint)bVar1 >> 3] >> (bVar1 & 7) & 1) == 0);
  return (char *)pbVar2;
}


// 1001abc0 ___ansicp

/* Library Function - Single Match
    ___ansicp
   
   Library: Visual Studio 2005 Release */

void __cdecl ___ansicp(LCID param_1)

{
  int iVar1;
  CHAR local_10 [6];
  undefined1 local_a;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  local_a = 0;
  iVar1 = GetLocaleInfoA(param_1,0x1004,local_10,6);
  if (iVar1 != 0) {
    _atol(local_10);
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001ac07 ___convertcp

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* Library Function - Single Match
    ___convertcp
   
   Library: Visual Studio 2005 Release */

void __cdecl
___convertcp(UINT param_1,UINT param_2,char *param_3,uint *param_4,LPSTR param_5,int param_6)

{
  uint uVar1;
  uint cbMultiByte;
  bool bVar2;
  BOOL BVar3;
  size_t sVar4;
  LPCWSTR pWVar5;
  int iVar6;
  LPSTR lpMultiByteStr;
  uint uVar7;
  bool bVar8;
  LPCWSTR local_20;
  _cpinfo local_1c;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  cbMultiByte = *param_4;
  bVar2 = false;
  if (param_1 == param_2) goto LAB_1001ada7;
  BVar3 = GetCPInfo(param_1,&local_1c);
  if ((((BVar3 == 0) || (local_1c.MaxCharSize != 1)) ||
      (BVar3 = GetCPInfo(param_2,&local_1c), BVar3 == 0)) || (local_1c.MaxCharSize != 1)) {
    uVar7 = MultiByteToWideChar(param_1,1,param_3,cbMultiByte,(LPWSTR)0x0,0);
    bVar8 = uVar7 == 0;
    if (bVar8) goto LAB_1001ada7;
  }
  else {
    bVar2 = true;
    uVar7 = cbMultiByte;
    if (cbMultiByte == 0xffffffff) {
      sVar4 = _strlen(param_3);
      uVar7 = sVar4 + 1;
    }
    bVar8 = uVar7 == 0;
  }
  if ((bVar8 || (int)uVar7 < 0) || (0x7ffffff0 < uVar7)) {
    local_20 = (LPCWSTR)0x0;
  }
  else {
    uVar1 = uVar7 * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar5 = (LPCWSTR)&stack0xffffffbc;
      local_20 = (LPCWSTR)&stack0xffffffbc;
      if (&stack0x00000000 != (undefined1 *)0x44) {
LAB_1001ace7:
        local_20 = pWVar5 + 4;
      }
    }
    else {
      pWVar5 = (LPCWSTR)FUN_10001270(uVar1);
      local_20 = pWVar5;
      if (pWVar5 != (LPCWSTR)0x0) {
        pWVar5[0] = L'\xdddd';
        pWVar5[1] = L'\0';
        goto LAB_1001ace7;
      }
    }
  }
  if (local_20 != (LPCWSTR)0x0) {
    _memset(local_20,0,uVar7 * 2);
    iVar6 = MultiByteToWideChar(param_1,1,param_3,cbMultiByte,local_20,uVar7);
    if (iVar6 != 0) {
      if (param_5 == (LPSTR)0x0) {
        if (((bVar2) ||
            (uVar7 = WideCharToMultiByte(param_2,0,local_20,uVar7,(LPSTR)0x0,0,(LPCSTR)0x0,
                                         (LPBOOL)0x0), uVar7 != 0)) &&
           (lpMultiByteStr = (LPSTR)FUN_100011f0(1,uVar7), lpMultiByteStr != (LPSTR)0x0)) {
          uVar7 = WideCharToMultiByte(param_2,0,local_20,uVar7,lpMultiByteStr,uVar7,(LPCSTR)0x0,
                                      (LPBOOL)0x0);
          if (uVar7 == 0) {
            FUN_100012d0(lpMultiByteStr);
          }
          else if (cbMultiByte != 0xffffffff) {
            *param_4 = uVar7;
          }
        }
      }
      else {
        WideCharToMultiByte(param_2,0,local_20,uVar7,param_5,param_6,(LPCSTR)0x0,(LPBOOL)0x0);
      }
    }
    __freea(local_20);
  }
LAB_1001ada7:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001adb9 x_ismbbtype_l

/* Library Function - Single Match
    int __cdecl x_ismbbtype_l(struct localeinfo_struct *,unsigned int,int,int)
   
   Library: Visual Studio 2005 Release */

int __cdecl x_ismbbtype_l(localeinfo_struct *param_1,uint param_2,int param_3,int param_4)

{
  uint uVar1;
  int local_14;
  int local_10;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,param_1);
  if ((*(byte *)(local_10 + 0x1d + (param_2 & 0xff)) & (byte)param_4) == 0) {
    if (param_3 == 0) {
      uVar1 = 0;
    }
    else {
      uVar1 = (uint)*(ushort *)(*(int *)(local_14 + 200) + (param_2 & 0xff) * 2) & param_3;
    }
    if (uVar1 == 0) goto LAB_1001adfb;
  }
  uVar1 = 1;
LAB_1001adfb:
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 1001ae0a __ismbblead

/* Library Function - Single Match
    __ismbblead
   
   Library: Visual Studio 2005 Release */

int __cdecl __ismbblead(uint _C)

{
  int iVar1;
  
  iVar1 = x_ismbbtype_l((localeinfo_struct *)0x0,_C,0,4);
  return iVar1;
}


// 1001ae1d __fputwc_nolock

/* Library Function - Single Match
    __fputwc_nolock
   
   Library: Visual Studio 2005 Release */

wint_t __cdecl __fputwc_nolock(wchar_t _Ch,FILE *_File)

{
  int *piVar1;
  wint_t wVar2;
  int iVar3;
  uint uVar4;
  undefined *puVar5;
  errno_t eVar6;
  char cVar7;
  int local_14;
  char local_10 [8];
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  if ((_File->_flag & 0x40) == 0) {
    iVar3 = __fileno(_File);
    if ((iVar3 == -1) || (iVar3 = __fileno(_File), iVar3 == -2)) {
      puVar5 = &DAT_100365e8;
    }
    else {
      iVar3 = __fileno(_File);
      uVar4 = __fileno(_File);
      puVar5 = (undefined *)((uVar4 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar3 >> 5]);
    }
    if ((puVar5[0x24] & 0x7f) != 2) {
      iVar3 = __fileno(_File);
      if ((iVar3 == -1) || (iVar3 = __fileno(_File), iVar3 == -2)) {
        puVar5 = &DAT_100365e8;
      }
      else {
        iVar3 = __fileno(_File);
        uVar4 = __fileno(_File);
        puVar5 = (undefined *)((uVar4 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar3 >> 5]);
      }
      if ((puVar5[0x24] & 0x7f) == 1) {
        piVar1 = &_File->_cnt;
        *piVar1 = *piVar1 + -1;
        if (*piVar1 < 0) {
          uVar4 = __flsbuf((int)(char)_Ch,_File);
        }
        else {
          *_File->_ptr = (char)_Ch;
          uVar4 = (uint)(byte)*_File->_ptr;
          _File->_ptr = _File->_ptr + 1;
        }
        if (uVar4 != 0xffffffff) {
          piVar1 = &_File->_cnt;
          *piVar1 = *piVar1 + -1;
          cVar7 = (char)((ushort)_Ch >> 8);
          if (*piVar1 < 0) {
            __flsbuf((int)cVar7,_File);
          }
          else {
            *_File->_ptr = cVar7;
            _File->_ptr = _File->_ptr + 1;
          }
        }
        goto LAB_1001afec;
      }
      iVar3 = __fileno(_File);
      if ((iVar3 == -1) || (iVar3 = __fileno(_File), iVar3 == -2)) {
        puVar5 = &DAT_100365e8;
      }
      else {
        iVar3 = __fileno(_File);
        uVar4 = __fileno(_File);
        puVar5 = (undefined *)((uVar4 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar3 >> 5]);
      }
      if ((puVar5[4] & 0x80) != 0) {
        eVar6 = _wctomb_s(&local_14,local_10,5,_Ch);
        if ((eVar6 == 0) && (iVar3 = 0, 0 < local_14)) {
          do {
            piVar1 = &_File->_cnt;
            *piVar1 = *piVar1 + -1;
            if (*piVar1 < 0) {
              uVar4 = __flsbuf((int)local_10[iVar3],_File);
            }
            else {
              *_File->_ptr = local_10[iVar3];
              uVar4 = (uint)(byte)*_File->_ptr;
              _File->_ptr = _File->_ptr + 1;
            }
          } while ((uVar4 != 0xffffffff) && (iVar3 = iVar3 + 1, iVar3 < local_14));
        }
        goto LAB_1001afec;
      }
    }
  }
  piVar1 = &_File->_cnt;
  *piVar1 = *piVar1 + -2;
  if (*piVar1 < 0) {
    __flswbuf((uint)(ushort)_Ch,_File);
  }
  else {
    *(wchar_t *)_File->_ptr = _Ch;
    _File->_ptr = _File->_ptr + 2;
  }
LAB_1001afec:
  wVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return wVar2;
}


// 1001affb __mbtowc_l

/* Library Function - Single Match
    __mbtowc_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbtowc_l(wchar_t *_DstCh,char *_SrcCh,size_t _SrcSizeInBytes,_locale_t _Locale)

{
  wchar_t *pwVar1;
  int iVar2;
  DWORD *pDVar3;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  if ((_SrcCh != (char *)0x0) && (_SrcSizeInBytes != 0)) {
    if (*_SrcCh != '\0') {
      _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
      if ((local_14.locinfo)->lc_category[0].wlocale != (wchar_t *)0x0) {
        iVar2 = __isleadbyte_l((uint)(byte)*_SrcCh,&local_14);
        if (iVar2 == 0) {
          iVar2 = MultiByteToWideChar((local_14.locinfo)->lc_codepage,9,_SrcCh,1,_DstCh,
                                      (uint)(_DstCh != (wchar_t *)0x0));
          if (iVar2 != 0) goto LAB_1001b046;
        }
        else {
          pwVar1 = (local_14.locinfo)->locale_name[3];
          if ((((1 < (int)pwVar1) && ((int)pwVar1 <= (int)_SrcSizeInBytes)) &&
              (iVar2 = MultiByteToWideChar((local_14.locinfo)->lc_codepage,9,_SrcCh,(int)pwVar1,
                                           _DstCh,(uint)(_DstCh != (wchar_t *)0x0)), iVar2 != 0)) ||
             (((local_14.locinfo)->locale_name[3] <= _SrcSizeInBytes && (_SrcCh[1] != '\0')))) {
            pwVar1 = (local_14.locinfo)->locale_name[3];
            if (local_8 == '\0') {
              return (int)pwVar1;
            }
            *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
            return (int)pwVar1;
          }
        }
        pDVar3 = FUN_10011e63();
        *pDVar3 = 0x2a;
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        return -1;
      }
      if (_DstCh != (wchar_t *)0x0) {
        *_DstCh = (ushort)(byte)*_SrcCh;
      }
LAB_1001b046:
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
      return 1;
    }
    if (_DstCh != (wchar_t *)0x0) {
      *_DstCh = L'\0';
    }
  }
  return 0;
}


// 1001b10e _mbtowc

/* Library Function - Single Match
    _mbtowc
   
   Library: Visual Studio 2005 Release */

int __cdecl _mbtowc(wchar_t *_DstCh,char *_SrcCh,size_t _SrcSizeInBytes)

{
  int iVar1;
  
  iVar1 = __mbtowc_l(_DstCh,_SrcCh,_SrcSizeInBytes,(_locale_t)0x0);
  return iVar1;
}


// 1001b125 __wchartodigit

/* Library Function - Single Match
    __wchartodigit
   
   Library: Visual Studio 2005 Release */

int __cdecl __wchartodigit(ushort param_1)

{
  int iVar1;
  bool bVar2;
  
  if (0x2f < param_1) {
    if (param_1 < 0x3a) {
      return param_1 - 0x30;
    }
    iVar1 = 0xff10;
    if (param_1 < 0xff10) {
      iVar1 = 0x660;
      if (param_1 < 0x660) {
        return -1;
      }
      if (param_1 < 0x66a) goto LAB_1001b163;
      iVar1 = 0x6f0;
      if (param_1 < 0x6f0) {
        return -1;
      }
      if (param_1 < 0x6fa) goto LAB_1001b163;
      iVar1 = 0x966;
      if (param_1 < 0x966) {
        return -1;
      }
      if (param_1 < 0x970) goto LAB_1001b163;
      iVar1 = 0x9e6;
      if (param_1 < 0x9e6) {
        return -1;
      }
      if (param_1 < 0x9f0) goto LAB_1001b163;
      iVar1 = 0xa66;
      if (param_1 < 0xa66) {
        return -1;
      }
      if (param_1 < 0xa70) goto LAB_1001b163;
      iVar1 = 0xae6;
      if (param_1 < 0xae6) {
        return -1;
      }
      if (param_1 < 0xaf0) goto LAB_1001b163;
      iVar1 = 0xb66;
      if (param_1 < 0xb66) {
        return -1;
      }
      if (param_1 < 0xb70) goto LAB_1001b163;
      iVar1 = 0xc66;
      if (param_1 < 0xc66) {
        return -1;
      }
      if (param_1 < 0xc70) goto LAB_1001b163;
      iVar1 = 0xce6;
      if (param_1 < 0xce6) {
        return -1;
      }
      if (param_1 < 0xcf0) goto LAB_1001b163;
      iVar1 = 0xd66;
      if (param_1 < 0xd66) {
        return -1;
      }
      if (param_1 < 0xd70) goto LAB_1001b163;
      iVar1 = 0xe50;
      if (param_1 < 0xe50) {
        return -1;
      }
      if (param_1 < 0xe5a) goto LAB_1001b163;
      iVar1 = 0xed0;
      if (param_1 < 0xed0) {
        return -1;
      }
      if (param_1 < 0xeda) goto LAB_1001b163;
      iVar1 = 0xf20;
      if (param_1 < 0xf20) {
        return -1;
      }
      if (param_1 < 0xf2a) goto LAB_1001b163;
      iVar1 = 0x1040;
      if (param_1 < 0x1040) {
        return -1;
      }
      if (param_1 < 0x104a) goto LAB_1001b163;
      iVar1 = 0x17e0;
      if (param_1 < 0x17e0) {
        return -1;
      }
      if (param_1 < 0x17ea) goto LAB_1001b163;
      iVar1 = 0x1810;
      if (param_1 < 0x1810) {
        return -1;
      }
      bVar2 = param_1 < 0x181a;
    }
    else {
      bVar2 = param_1 < 0xff1a;
    }
    if (bVar2) {
LAB_1001b163:
      return (uint)param_1 - iVar1;
    }
  }
  return -1;
}


// 1001b2a7 __fgetwc_nolock

/* Library Function - Single Match
    __fgetwc_nolock
   
   Library: Visual Studio 2005 Release */

wint_t __cdecl __fgetwc_nolock(FILE *_File)

{
  int *piVar1;
  FILE *_File_00;
  wint_t wVar2;
  int iVar3;
  uint uVar4;
  undefined *puVar5;
  DWORD *pDVar6;
  size_t _SrcSizeInBytes;
  undefined4 local_8;
  
  _File_00 = _File;
  if ((_File->_flag & 0x40) == 0) {
    iVar3 = __fileno(_File);
    if ((iVar3 == -1) || (iVar3 = __fileno(_File_00), iVar3 == -2)) {
      puVar5 = &DAT_100365e8;
    }
    else {
      iVar3 = __fileno(_File_00);
      uVar4 = __fileno(_File_00);
      puVar5 = (undefined *)((uVar4 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar3 >> 5]);
    }
    if ((puVar5[0x24] & 0x7f) != 0) {
      piVar1 = &_File_00->_cnt;
      *piVar1 = *piVar1 + -1;
      if (*piVar1 < 0) {
        uVar4 = __filbuf(_File_00);
      }
      else {
        uVar4 = (uint)(byte)*_File_00->_ptr;
        _File_00->_ptr = _File_00->_ptr + 1;
      }
      if (uVar4 != 0xffffffff) {
        piVar1 = &_File_00->_cnt;
        *piVar1 = *piVar1 + -1;
        _File = (FILE *)CONCAT31(_File._1_3_,(char)uVar4);
        if (*piVar1 < 0) {
          uVar4 = __filbuf(_File_00);
        }
        else {
          uVar4 = (uint)(byte)*_File_00->_ptr;
          _File_00->_ptr = _File_00->_ptr + 1;
        }
        if (uVar4 != 0xffffffff) {
          _File._0_2_ = CONCAT11((char)uVar4,_File._0_1_);
          _File = (FILE *)(uint)(ushort)_File;
          goto LAB_1001b347;
        }
      }
      return 0xffff;
    }
    if ((_File_00->_flag & 0x40) == 0) {
      iVar3 = __fileno(_File_00);
      if ((iVar3 == -1) || (iVar3 = __fileno(_File_00), iVar3 == -2)) {
        puVar5 = &DAT_100365e8;
      }
      else {
        iVar3 = __fileno(_File_00);
        uVar4 = __fileno(_File_00);
        puVar5 = (undefined *)((uVar4 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar3 >> 5]);
      }
      if ((puVar5[4] & 0x80) != 0) {
        _SrcSizeInBytes = 1;
        piVar1 = &_File_00->_cnt;
        *piVar1 = *piVar1 + -1;
        if (*piVar1 < 0) {
          uVar4 = __filbuf(_File_00);
        }
        else {
          uVar4 = (uint)(byte)*_File_00->_ptr;
          _File_00->_ptr = _File_00->_ptr + 1;
        }
        if (uVar4 == 0xffffffff) {
          return 0xffff;
        }
        local_8 = CONCAT31(local_8._1_3_,(char)uVar4);
        iVar3 = _isleadbyte(uVar4 & 0xff);
        if (iVar3 != 0) {
          piVar1 = &_File_00->_cnt;
          *piVar1 = *piVar1 + -1;
          if (*piVar1 < 0) {
            uVar4 = __filbuf(_File_00);
          }
          else {
            uVar4 = (uint)(byte)*_File_00->_ptr;
            _File_00->_ptr = _File_00->_ptr + 1;
          }
          if (uVar4 == 0xffffffff) {
            _ungetc((int)(char)local_8,_File_00);
            return 0xffff;
          }
          local_8._0_2_ = CONCAT11((char)uVar4,(char)local_8);
          _SrcSizeInBytes = 2;
        }
        iVar3 = _mbtowc((wchar_t *)&_File,(char *)&local_8,_SrcSizeInBytes);
        if (iVar3 == -1) {
          pDVar6 = FUN_10011e63();
          *pDVar6 = 0x2a;
          return 0xffff;
        }
LAB_1001b347:
        return (ushort)_File;
      }
    }
  }
  piVar1 = &_File_00->_cnt;
  *piVar1 = *piVar1 + -2;
  if (*piVar1 < 0) {
    iVar3 = __filwbuf(_File_00);
    wVar2 = (wint_t)iVar3;
  }
  else {
    wVar2 = *(wint_t *)_File_00->_ptr;
    _File_00->_ptr = (char *)((int)_File_00->_ptr + 2);
  }
  return wVar2;
}


// 1001b44e __ungetwc_nolock

/* Library Function - Single Match
    __ungetwc_nolock
   
   Library: Visual Studio 2005 Release */

wint_t __cdecl __ungetwc_nolock(wint_t _Ch,FILE *_File)

{
  wint_t *pwVar1;
  wint_t wVar2;
  int iVar3;
  uint uVar4;
  undefined *puVar5;
  errno_t eVar6;
  undefined4 local_14;
  char local_10;
  undefined1 local_f;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  if (_Ch != 0xffff) {
    uVar4 = _File->_flag;
    if (((uVar4 & 1) != 0) || (((char)uVar4 < '\0' && ((uVar4 & 2) == 0)))) {
      if (_File->_base == (char *)0x0) {
        __getbuf(_File);
      }
      if ((_File->_flag & 0x40) == 0) {
        iVar3 = __fileno(_File);
        if ((iVar3 == -1) || (iVar3 = __fileno(_File), iVar3 == -2)) {
          puVar5 = &DAT_100365e8;
        }
        else {
          iVar3 = __fileno(_File);
          uVar4 = __fileno(_File);
          puVar5 = (undefined *)((uVar4 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar3 >> 5]);
        }
        if ((puVar5[4] & 0x80) != 0) {
          iVar3 = __fileno(_File);
          if ((iVar3 == -1) || (iVar3 = __fileno(_File), iVar3 == -2)) {
            puVar5 = &DAT_100365e8;
          }
          else {
            iVar3 = __fileno(_File);
            uVar4 = __fileno(_File);
            puVar5 = (undefined *)((uVar4 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar3 >> 5]);
          }
          if ((puVar5[0x24] & 0x7f) == 0) {
            eVar6 = _wctomb_s(&local_14,&local_10,5,_Ch);
            if (eVar6 != 0) goto LAB_1001b5d2;
          }
          else {
            local_10 = (char)_Ch;
            local_f = (char)(_Ch >> 8);
            local_14 = 2;
          }
          iVar3 = local_14;
          if (_File->_ptr < _File->_base + local_14) {
            if ((_File->_cnt != 0) || (_File->_bufsiz < local_14)) goto LAB_1001b5d2;
            _File->_ptr = _File->_base + local_14;
          }
          while (iVar3 = iVar3 + -1, -1 < iVar3) {
            _File->_ptr = _File->_ptr + -1;
            *_File->_ptr = (&local_10)[iVar3];
          }
          _File->_cnt = _File->_cnt + local_14;
          _File->_flag = _File->_flag & 0xffffffefU | 1;
          goto LAB_1001b5d2;
        }
      }
      if (_File->_ptr < _File->_base + 2) {
        if ((_File->_cnt != 0) || ((uint)_File->_bufsiz < 2)) goto LAB_1001b5d2;
        _File->_ptr = _File->_base + 2;
      }
      _File->_ptr = _File->_ptr + -2;
      pwVar1 = (wint_t *)_File->_ptr;
      if ((_File->_flag & 0x40) == 0) {
        *pwVar1 = _Ch;
      }
      else if (*pwVar1 != _Ch) {
        _File->_ptr = (char *)(pwVar1 + 1);
        goto LAB_1001b5d2;
      }
      _File->_cnt = _File->_cnt + 2;
      _File->_flag = _File->_flag & 0xffffffefU | 1;
    }
  }
LAB_1001b5d2:
  wVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return wVar2;
}


// 1001b5f9 __wcstombs_l_helper

/* Library Function - Single Match
    __wcstombs_l_helper
   
   Library: Visual Studio 2005 Release */

void __cdecl
__wcstombs_l_helper(LPSTR param_1,LPCWSTR param_2,uint param_3,localeinfo_struct *param_4)

{
  char cVar1;
  WCHAR WVar2;
  DWORD *pDVar3;
  LPCWSTR pWVar4;
  int iVar5;
  DWORD DVar6;
  uint uVar7;
  uint uVar8;
  LPCWSTR local_30;
  uint local_28;
  int local_24 [2];
  int local_1c;
  char local_18;
  BOOL local_14;
  CHAR local_10 [8];
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  local_30 = param_2;
  local_28 = 0;
  local_14 = 0;
  if ((param_1 != (LPSTR)0x0) && (param_3 == 0)) goto LAB_1001b782;
  if ((param_2 == (LPCWSTR)0x0) || (0x7fffffff < param_3)) {
    pDVar3 = FUN_10011e63();
    *pDVar3 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_1001b782;
  }
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_24,param_4);
  if (param_1 == (LPSTR)0x0) {
    if (*(int *)(local_24[0] + 0x14) == 0) {
      FUN_10015e29(param_2);
    }
    else {
      iVar5 = WideCharToMultiByte(*(UINT *)(local_24[0] + 4),0,param_2,-1,(LPSTR)0x0,0,(LPCSTR)0x0,
                                  &local_14);
      if ((iVar5 == 0) || (local_14 != 0)) goto LAB_1001b768;
    }
LAB_1001b6b7:
    if (local_18 != '\0') {
      *(uint *)(local_1c + 0x70) = *(uint *)(local_1c + 0x70) & 0xfffffffd;
    }
  }
  else {
    if (*(int *)(local_24[0] + 0x14) == 0) {
      if (param_3 != 0) {
        do {
          if (0xff < (ushort)*param_2) goto LAB_1001b768;
          param_1[local_28] = (CHAR)*param_2;
          WVar2 = *param_2;
          param_2 = param_2 + 1;
          if (WVar2 == L'\0') goto LAB_1001b6b7;
          local_28 = local_28 + 1;
        } while (local_28 < param_3);
      }
      if (local_18 != '\0') {
        *(uint *)(local_1c + 0x70) = *(uint *)(local_1c + 0x70) & 0xfffffffd;
      }
      goto LAB_1001b782;
    }
    if (*(int *)(local_24[0] + 0xac) == 1) {
      pWVar4 = param_2;
      uVar8 = param_3;
      if (param_3 != 0) {
        do {
          if (*pWVar4 == L'\0') break;
          pWVar4 = pWVar4 + 1;
          uVar8 = uVar8 - 1;
        } while (uVar8 != 0);
        if ((uVar8 != 0) && (*pWVar4 == L'\0')) {
          param_3 = ((int)pWVar4 - (int)param_2 >> 1) + 1;
        }
      }
      iVar5 = WideCharToMultiByte(*(UINT *)(local_24[0] + 4),0,param_2,param_3,param_1,param_3,
                                  (LPCSTR)0x0,&local_14);
      if (iVar5 != 0) {
joined_r0x1001b74a:
        if (local_14 == 0) goto LAB_1001b6b7;
      }
    }
    else {
      iVar5 = WideCharToMultiByte(*(UINT *)(local_24[0] + 4),0,param_2,-1,param_1,param_3,
                                  (LPCSTR)0x0,&local_14);
      if (iVar5 != 0) goto joined_r0x1001b74a;
      if (local_14 == 0) {
        DVar6 = GetLastError();
        uVar8 = 0;
        if (DVar6 == 0x7a) {
          while (uVar8 < param_3) {
            uVar7 = WideCharToMultiByte(*(UINT *)(local_24[0] + 4),0,local_30,1,local_10,
                                        *(int *)(local_24[0] + 0xac),(LPCSTR)0x0,&local_14);
            if ((((uVar7 == 0) || (local_14 != 0)) || ((int)uVar7 < 0)) || (5 < uVar7))
            goto LAB_1001b768;
            if (param_3 < uVar7 + uVar8) break;
            iVar5 = 0;
            if (0 < (int)uVar7) {
              do {
                cVar1 = local_10[iVar5];
                param_1[uVar8] = cVar1;
                if (cVar1 == '\0') goto LAB_1001b7f0;
                iVar5 = iVar5 + 1;
                uVar8 = uVar8 + 1;
              } while (iVar5 < (int)uVar7);
            }
            local_30 = local_30 + 1;
          }
LAB_1001b7f0:
          if (local_18 != '\0') {
            *(uint *)(local_1c + 0x70) = *(uint *)(local_1c + 0x70) & 0xfffffffd;
          }
          goto LAB_1001b782;
        }
      }
    }
LAB_1001b768:
    pDVar3 = FUN_10011e63();
    *pDVar3 = 0x2a;
    if (local_18 != '\0') {
      *(uint *)(local_1c + 0x70) = *(uint *)(local_1c + 0x70) & 0xfffffffd;
    }
  }
LAB_1001b782:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001b83f __wcstombs_s_l

/* Library Function - Single Match
    __wcstombs_s_l
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl
__wcstombs_s_l(size_t *_PtNumOfCharConverted,char *_Dst,size_t _DstSizeInBytes,wchar_t *_Src,
              size_t _MaxCountInBytes,_locale_t _Locale)

{
  size_t sVar1;
  int iVar2;
  DWORD *pDVar3;
  uint uVar4;
  DWORD DVar5;
  errno_t local_8;
  
  local_8 = 0;
  if (_Dst == (char *)0x0) {
    if (_DstSizeInBytes != 0) goto LAB_1001b899;
  }
  else if (_DstSizeInBytes == 0) {
LAB_1001b899:
    pDVar3 = FUN_10011e63();
    DVar5 = 0x16;
    goto LAB_1001b8a0;
  }
  if (_Dst != (char *)0x0) {
    *_Dst = '\0';
  }
  if (_PtNumOfCharConverted != (size_t *)0x0) {
    *_PtNumOfCharConverted = 0;
  }
  sVar1 = _MaxCountInBytes;
  if (_DstSizeInBytes < _MaxCountInBytes) {
    sVar1 = _DstSizeInBytes;
  }
  iVar2 = __wcstombs_l_helper(_Dst,_Src,sVar1,_Locale);
  if (iVar2 == -1) {
    if (_Dst != (char *)0x0) {
      *_Dst = '\0';
    }
    pDVar3 = FUN_10011e63();
    return *pDVar3;
  }
  uVar4 = iVar2 + 1;
  if (_Dst != (char *)0x0) {
    if (_DstSizeInBytes < uVar4) {
      if ((_MaxCountInBytes != 0xffffffff) && (*_Dst = '\0', _DstSizeInBytes <= uVar4)) {
        pDVar3 = FUN_10011e63();
        DVar5 = 0x22;
LAB_1001b8a0:
        *pDVar3 = DVar5;
        FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return DVar5;
      }
      local_8 = 0x50;
      uVar4 = _DstSizeInBytes;
    }
    _Dst[uVar4 - 1] = '\0';
  }
  if (_PtNumOfCharConverted == (size_t *)0x0) {
    return local_8;
  }
  *_PtNumOfCharConverted = uVar4;
  return local_8;
}


// 1001b8f0 _wcstombs_s

/* Library Function - Single Match
    _wcstombs_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl
_wcstombs_s(size_t *_PtNumOfCharConverted,char *_Dst,size_t _DstSizeInBytes,wchar_t *_Src,
           size_t _MaxCountInBytes)

{
  errno_t eVar1;
  
  eVar1 = __wcstombs_s_l(_PtNumOfCharConverted,_Dst,_DstSizeInBytes,_Src,_MaxCountInBytes,
                         (_locale_t)0x0);
  return eVar1;
}


// 1001b90e __filbuf

/* Library Function - Single Match
    __filbuf
   
   Library: Visual Studio 2005 Release */

int __cdecl __filbuf(FILE *_File)

{
  byte bVar1;
  DWORD *pDVar2;
  uint uVar3;
  int iVar4;
  undefined *puVar5;
  LPWSTR pWVar6;
  LPWSTR pWVar7;
  
  if (_File == (FILE *)0x0) {
    pDVar2 = FUN_10011e63();
    *pDVar2 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    uVar3 = _File->_flag;
    if (((uVar3 & 0x83) != 0) && ((uVar3 & 0x40) == 0)) {
      if ((uVar3 & 2) == 0) {
        _File->_flag = uVar3 | 1;
        if ((uVar3 & 0x10c) == 0) {
          __getbuf(_File);
        }
        else {
          _File->_ptr = _File->_base;
        }
        pWVar7 = (LPWSTR)_File->_bufsiz;
        pWVar6 = (LPWSTR)_File->_base;
        uVar3 = __fileno(_File);
        iVar4 = FUN_1001e6a2(uVar3,pWVar6,pWVar7);
        _File->_cnt = iVar4;
        if ((iVar4 != 0) && (iVar4 != -1)) {
          if ((_File->_flag & 0x82) == 0) {
            iVar4 = __fileno(_File);
            if ((iVar4 == -1) || (iVar4 = __fileno(_File), iVar4 == -2)) {
              puVar5 = &DAT_100365e8;
            }
            else {
              iVar4 = __fileno(_File);
              uVar3 = __fileno(_File);
              puVar5 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar4 >> 5]);
            }
            if ((puVar5[4] & 0x82) == 0x82) {
              _File->_flag = _File->_flag | 0x2000;
            }
          }
          if (((_File->_bufsiz == 0x200) && ((_File->_flag & 8U) != 0)) &&
             ((_File->_flag & 0x400U) == 0)) {
            _File->_bufsiz = 0x1000;
          }
          _File->_cnt = _File->_cnt + -1;
          bVar1 = *_File->_ptr;
          _File->_ptr = _File->_ptr + 1;
          return (uint)bVar1;
        }
        _File->_flag = _File->_flag | (-(uint)(iVar4 != 0) & 0x10) + 0x10;
        _File->_cnt = 0;
      }
      else {
        _File->_flag = uVar3 | 0x20;
      }
    }
  }
  return -1;
}


// 1001ba2e __ungetc_nolock

/* Library Function - Single Match
    __ungetc_nolock
   
   Library: Visual Studio 2005 Release */

int __cdecl __ungetc_nolock(int _Ch,FILE *_File)

{
  char *pcVar1;
  int iVar2;
  uint uVar3;
  undefined *puVar4;
  DWORD *pDVar5;
  
  if ((_File->_flag & 0x40) == 0) {
    iVar2 = __fileno(_File);
    if ((iVar2 == -1) || (iVar2 = __fileno(_File), iVar2 == -2)) {
      puVar4 = &DAT_100365e8;
    }
    else {
      iVar2 = __fileno(_File);
      uVar3 = __fileno(_File);
      puVar4 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar2 >> 5]);
    }
    if ((puVar4[0x24] & 0x7f) == 0) {
      iVar2 = __fileno(_File);
      if ((iVar2 == -1) || (iVar2 = __fileno(_File), iVar2 == -2)) {
        puVar4 = &DAT_100365e8;
      }
      else {
        iVar2 = __fileno(_File);
        uVar3 = __fileno(_File);
        puVar4 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar2 >> 5]);
      }
      if ((puVar4[0x24] & 0x80) == 0) goto LAB_1001baeb;
    }
    pDVar5 = FUN_10011e63();
    *pDVar5 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
LAB_1001baeb:
    if (_Ch != -1) {
      uVar3 = _File->_flag;
      if (((uVar3 & 1) != 0) || (((char)uVar3 < '\0' && ((uVar3 & 2) == 0)))) {
        if (_File->_base == (char *)0x0) {
          __getbuf(_File);
        }
        if (_File->_ptr == _File->_base) {
          if (_File->_cnt != 0) {
            return -1;
          }
          _File->_ptr = _File->_ptr + 1;
        }
        _File->_ptr = _File->_ptr + -1;
        pcVar1 = _File->_ptr;
        if ((_File->_flag & 0x40) == 0) {
          *pcVar1 = (char)_Ch;
        }
        else if (*pcVar1 != (char)_Ch) {
          _File->_ptr = pcVar1 + 1;
          return -1;
        }
        _File->_cnt = _File->_cnt + 1;
        _File->_flag = _File->_flag & 0xffffffefU | 1;
        return _Ch & 0xff;
      }
    }
  }
  return -1;
}


// 1001bb4c _ungetc

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _ungetc
   
   Library: Visual Studio 2005 Release */

int __cdecl _ungetc(int _Ch,FILE *_File)

{
  DWORD *pDVar1;
  int iVar2;
  
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  else {
    FUN_1001956a((uint)_File);
    iVar2 = __ungetc_nolock(_Ch,_File);
    FUN_1001bbb4();
  }
  return iVar2;
}


// 1001bbb4 FUN_1001bbb4

void FUN_1001bbb4(void)

{
  int unaff_EBP;
  
  FUN_100195d4(*(uint *)(unaff_EBP + 0xc));
  return;
}


// 1001bbbe __ld12tod

/* Library Function - Single Match
    __ld12tod
   
   Library: Visual Studio 2005 Release */

INTRNCVT_STATUS __cdecl __ld12tod(_LDBL12 *_Ifp,_CRT_DOUBLE *_D)

{
  undefined4 uVar1;
  int iVar2;
  INTRNCVT_STATUS IVar3;
  int iVar4;
  byte bVar5;
  uint *puVar6;
  uint *puVar7;
  uint uVar8;
  _LDBL12 *p_Var9;
  uint uVar10;
  uint uVar11;
  int iVar12;
  int iVar13;
  bool bVar14;
  uint local_24 [4];
  uint local_14;
  uint local_10;
  int local_c;
  _LDBL12 *local_8;
  
  local_24[3] = *(ushort *)(_Ifp->ld12 + 10) & 0x8000;
  uVar10 = *(uint *)(_Ifp->ld12 + 6);
  local_24[0] = uVar10;
  uVar1 = *(undefined4 *)(_Ifp->ld12 + 2);
  uVar11 = *(ushort *)(_Ifp->ld12 + 10) & 0x7fff;
  iVar12 = uVar11 - 0x3fff;
  iVar2 = (uint)*(ushort *)_Ifp->ld12 << 0x10;
  local_24[1] = uVar1;
  local_24[2] = iVar2;
  if (iVar12 == -0x3fff) {
    iVar12 = 0;
    iVar2 = 0;
    do {
      if (local_24[iVar2] != 0) {
        local_24[0] = 0;
        local_24[1] = 0;
        IVar3 = INTRNCVT_UNDERFLOW;
        goto LAB_1001c0bd;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10036b88 - 1;
    iVar4 = (int)(DAT_10036b88 + ((int)DAT_10036b88 >> 0x1f & 0x1fU)) >> 5;
    uVar8 = DAT_10036b88 & 0x8000001f;
    local_14 = iVar12;
    local_10 = iVar4;
    if ((int)uVar8 < 0) {
      uVar8 = (uVar8 - 1 | 0xffffffe0) + 1;
    }
    puVar7 = local_24 + iVar4;
    bVar5 = (byte)(0x1f - uVar8);
    local_c = 0x1f - uVar8;
    if ((*puVar7 & 1 << (bVar5 & 0x1f)) != 0) {
      uVar8 = local_24[iVar4] & ~(-1 << (bVar5 & 0x1f));
      while( true ) {
        if (uVar8 != 0) {
          iVar4 = (int)(iVar13 + (iVar13 >> 0x1f & 0x1fU)) >> 5;
          local_8 = (_LDBL12 *)0x0;
          p_Var9 = (_LDBL12 *)(1 << (0x1f - ((byte)iVar13 & 0x1f) & 0x1f));
          puVar6 = local_24 + iVar4;
          _Ifp = (_LDBL12 *)(p_Var9->ld12 + *puVar6);
          if (_Ifp < (_LDBL12 *)*puVar6) goto LAB_1001bcf1;
          bVar14 = _Ifp < p_Var9;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_1001bcf8;
LAB_1001bcf1:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_1001bcf8:
              iVar4 = iVar4 + -1;
              *puVar6 = (uint)_Ifp;
              if ((iVar4 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_1001bd06;
              }
              local_8 = (_LDBL12 *)0x0;
              puVar6 = local_24 + iVar4;
              _Ifp = (_LDBL12 *)(((_LDBL12 *)*puVar6)->ld12 + 1);
            } while (_Ifp < (_LDBL12 *)*puVar6);
            bVar14 = _Ifp == (_LDBL12 *)0x0;
          } while( true );
        }
        iVar4 = iVar4 + 1;
        if (2 < iVar4) break;
        uVar8 = local_24[iVar4];
      }
    }
LAB_1001bd06:
    *puVar7 = *puVar7 & -1 << ((byte)local_c & 0x1f);
    iVar4 = local_10 + 1;
    if (iVar4 < 3) {
      puVar7 = local_24 + iVar4;
      for (iVar13 = 3 - iVar4; iVar13 != 0; iVar13 = iVar13 + -1) {
        *puVar7 = 0;
        puVar7 = puVar7 + 1;
      }
    }
    if (_Ifp != (_LDBL12 *)0x0) {
      iVar12 = uVar11 - 0x3ffe;
    }
    if (iVar12 < (int)(DAT_10036b84 - DAT_10036b88)) {
      local_24[0] = 0;
      local_24[1] = 0;
    }
    else {
      if (DAT_10036b84 < iVar12) {
        if (iVar12 < DAT_10036b80) {
          local_24[0] = local_24[0] & 0x7fffffff;
          iVar12 = iVar12 + DAT_10036b94;
          iVar2 = (int)(DAT_10036b8c + ((int)DAT_10036b8c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_10036b8c & 0x8000001f;
          if ((int)uVar10 < 0) {
            uVar10 = (uVar10 - 1 | 0xffffffe0) + 1;
          }
          local_10 = 0;
          _Ifp = (_LDBL12 *)0x0;
          local_8 = (_LDBL12 *)(0x20 - uVar10);
          do {
            local_14 = local_24[(int)_Ifp] & ~(-1 << ((byte)uVar10 & 0x1f));
            local_24[(int)_Ifp] = local_24[(int)_Ifp] >> ((byte)uVar10 & 0x1f) | local_10;
            _Ifp = (_LDBL12 *)(_Ifp->ld12 + 1);
            local_10 = local_14 << ((byte)(0x20 - uVar10) & 0x1f);
          } while ((int)_Ifp < 3);
          iVar4 = 2;
          puVar7 = local_24 + (2 - iVar2);
          do {
            if (iVar4 < iVar2) {
              local_24[iVar4] = 0;
            }
            else {
              local_24[iVar4] = *puVar7;
            }
            iVar4 = iVar4 + -1;
            puVar7 = puVar7 + -1;
          } while (-1 < iVar4);
          IVar3 = INTRNCVT_OK;
        }
        else {
          local_24[1] = 0;
          local_24[2] = 0;
          local_24[0] = 0x80000000;
          iVar12 = (int)(DAT_10036b8c + ((int)DAT_10036b8c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_10036b8c & 0x8000001f;
          if ((int)uVar10 < 0) {
            uVar10 = (uVar10 - 1 | 0xffffffe0) + 1;
          }
          local_10 = 0;
          _Ifp = (_LDBL12 *)0x0;
          local_8 = (_LDBL12 *)(0x20 - uVar10);
          do {
            uVar11 = local_24[(int)_Ifp];
            local_14 = uVar11 & ~(-1 << ((byte)uVar10 & 0x1f));
            local_24[(int)_Ifp] = uVar11 >> ((byte)uVar10 & 0x1f) | local_10;
            _Ifp = (_LDBL12 *)(_Ifp->ld12 + 1);
            local_10 = local_14 << ((byte)(0x20 - uVar10) & 0x1f);
          } while ((int)_Ifp < 3);
          iVar2 = 2;
          puVar7 = local_24 + (2 - iVar12);
          do {
            if (iVar2 < iVar12) {
              local_24[iVar2] = 0;
            }
            else {
              local_24[iVar2] = *puVar7;
            }
            iVar2 = iVar2 + -1;
            puVar7 = puVar7 + -1;
          } while (-1 < iVar2);
          iVar12 = DAT_10036b94 + DAT_10036b80;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_1001c0bd;
      }
      local_14 = DAT_10036b84 - local_14;
      local_24[0] = uVar10;
      local_24[1] = uVar1;
      iVar12 = (int)(local_14 + ((int)local_14 >> 0x1f & 0x1fU)) >> 5;
      uVar10 = local_14 & 0x8000001f;
      if ((int)uVar10 < 0) {
        uVar10 = (uVar10 - 1 | 0xffffffe0) + 1;
      }
      local_10 = 0;
      _Ifp = (_LDBL12 *)0x0;
      local_8 = (_LDBL12 *)(0x20 - uVar10);
      do {
        uVar11 = local_24[(int)_Ifp];
        local_14 = uVar11 & ~(-1 << ((byte)uVar10 & 0x1f));
        local_24[(int)_Ifp] = uVar11 >> ((byte)uVar10 & 0x1f) | local_10;
        _Ifp = (_LDBL12 *)(_Ifp->ld12 + 1);
        local_10 = local_14 << ((byte)(0x20 - uVar10) & 0x1f);
      } while ((int)_Ifp < 3);
      iVar2 = 2;
      puVar7 = local_24 + (2 - iVar12);
      do {
        if (iVar2 < iVar12) {
          local_24[iVar2] = 0;
        }
        else {
          local_24[iVar2] = *puVar7;
        }
        iVar2 = iVar2 + -1;
        puVar7 = puVar7 + -1;
      } while (-1 < iVar2);
      iVar2 = DAT_10036b88 - 1;
      iVar12 = (int)(DAT_10036b88 + ((int)DAT_10036b88 >> 0x1f & 0x1fU)) >> 5;
      uVar10 = DAT_10036b88 & 0x8000001f;
      local_10 = iVar12;
      if ((int)uVar10 < 0) {
        uVar10 = (uVar10 - 1 | 0xffffffe0) + 1;
      }
      bVar5 = (byte)(0x1f - uVar10);
      puVar7 = local_24 + iVar12;
      local_14 = 0x1f - uVar10;
      if ((*puVar7 & 1 << (bVar5 & 0x1f)) != 0) {
        uVar10 = local_24[iVar12] & ~(-1 << (bVar5 & 0x1f));
        while (uVar10 == 0) {
          iVar12 = iVar12 + 1;
          if (2 < iVar12) goto LAB_1001bea9;
          uVar10 = local_24[iVar12];
        }
        iVar12 = (int)(iVar2 + (iVar2 >> 0x1f & 0x1fU)) >> 5;
        bVar14 = false;
        uVar8 = 1 << (0x1f - ((byte)iVar2 & 0x1f) & 0x1f);
        uVar11 = local_24[iVar12];
        uVar10 = uVar11 + uVar8;
        if ((uVar10 < uVar11) || (uVar10 < uVar8)) {
          bVar14 = true;
        }
        local_24[iVar12] = uVar10;
        while ((iVar12 = iVar12 + -1, -1 < iVar12 && (bVar14))) {
          uVar11 = local_24[iVar12];
          uVar10 = uVar11 + 1;
          bVar14 = false;
          if ((uVar10 < uVar11) || (uVar10 == 0)) {
            bVar14 = true;
          }
          local_24[iVar12] = uVar10;
        }
      }
LAB_1001bea9:
      *puVar7 = *puVar7 & -1 << ((byte)local_14 & 0x1f);
      iVar12 = local_10 + 1;
      if (iVar12 < 3) {
        puVar7 = local_24 + iVar12;
        for (iVar2 = 3 - iVar12; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar7 = 0;
          puVar7 = puVar7 + 1;
        }
      }
      uVar10 = DAT_10036b8c + 1;
      iVar12 = (int)(uVar10 + ((int)uVar10 >> 0x1f & 0x1fU)) >> 5;
      uVar10 = uVar10 & 0x8000001f;
      if ((int)uVar10 < 0) {
        uVar10 = (uVar10 - 1 | 0xffffffe0) + 1;
      }
      local_10 = 0;
      _Ifp = (_LDBL12 *)0x0;
      local_8 = (_LDBL12 *)(0x20 - uVar10);
      do {
        uVar11 = local_24[(int)_Ifp];
        local_14 = uVar11 & ~(-1 << ((byte)uVar10 & 0x1f));
        local_24[(int)_Ifp] = uVar11 >> ((byte)uVar10 & 0x1f) | local_10;
        _Ifp = (_LDBL12 *)(_Ifp->ld12 + 1);
        local_10 = local_14 << ((byte)(0x20 - uVar10) & 0x1f);
      } while ((int)_Ifp < 3);
      iVar2 = 2;
      puVar7 = local_24 + (2 - iVar12);
      do {
        if (iVar2 < iVar12) {
          local_24[iVar2] = 0;
        }
        else {
          local_24[iVar2] = *puVar7;
        }
        iVar2 = iVar2 + -1;
        puVar7 = puVar7 + -1;
      } while (-1 < iVar2);
    }
    iVar12 = 0;
    IVar3 = INTRNCVT_UNDERFLOW;
  }
LAB_1001c0bd:
  local_24[0] = iVar12 << (0x1fU - (char)DAT_10036b8c & 0x1f) |
                -(uint)(local_24[3] != 0) & 0x80000000 | local_24[0];
  if (DAT_10036b90 == 0x40) {
    *(uint *)((int)&_D->x + 4) = local_24[0];
    *(uint *)&_D->x = local_24[1];
  }
  else if (DAT_10036b90 == 0x20) {
    *(uint *)&_D->x = local_24[0];
  }
  return IVar3;
}


// 1001c100 __ld12tof

/* Library Function - Multiple Matches With Different Base Names
    __ld12tod
    __ld12tof
   
   Library: Visual Studio 2005 Release */

INTRNCVT_STATUS __cdecl __ld12tof(_LDBL12 *_Ifp,_CRT_FLOAT *_F)

{
  uint uVar1;
  int iVar2;
  INTRNCVT_STATUS IVar3;
  byte bVar4;
  uint *puVar5;
  uint *puVar6;
  uint uVar7;
  _LDBL12 *p_Var8;
  uint uVar9;
  uint uVar10;
  int iVar11;
  float fVar12;
  int iVar13;
  bool bVar14;
  uint local_24;
  float local_20;
  uint local_1c [5];
  _LDBL12 *local_8;
  
  local_1c[1] = *(ushort *)(_Ifp->ld12 + 10) & 0x8000;
  uVar9 = *(uint *)(_Ifp->ld12 + 6);
  local_24 = uVar9;
  fVar12 = *(float *)(_Ifp->ld12 + 2);
  uVar10 = *(ushort *)(_Ifp->ld12 + 10) & 0x7fff;
  iVar11 = uVar10 - 0x3fff;
  uVar1 = (uint)*(ushort *)_Ifp->ld12 << 0x10;
  local_20 = fVar12;
  local_1c[0] = uVar1;
  if (iVar11 == -0x3fff) {
    iVar11 = 0;
    iVar2 = 0;
    do {
      if ((&local_24)[iVar2] != 0) {
        local_24 = 0;
        local_20 = 0.0;
        IVar3 = INTRNCVT_UNDERFLOW;
        goto LAB_1001c5ff;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10036ba0 - 1;
    iVar2 = (int)(DAT_10036ba0 + ((int)DAT_10036ba0 >> 0x1f & 0x1fU)) >> 5;
    uVar7 = DAT_10036ba0 & 0x8000001f;
    local_1c[2] = iVar11;
    local_1c[3] = iVar2;
    if ((int)uVar7 < 0) {
      uVar7 = (uVar7 - 1 | 0xffffffe0) + 1;
    }
    puVar6 = &local_24 + iVar2;
    bVar4 = (byte)(0x1f - uVar7);
    local_1c[4] = 0x1f - uVar7;
    if ((*puVar6 & 1 << (bVar4 & 0x1f)) != 0) {
      uVar7 = (&local_24)[iVar2] & ~(-1 << (bVar4 & 0x1f));
      while( true ) {
        if (uVar7 != 0) {
          iVar2 = (int)(iVar13 + (iVar13 >> 0x1f & 0x1fU)) >> 5;
          local_8 = (_LDBL12 *)0x0;
          p_Var8 = (_LDBL12 *)(1 << (0x1f - ((byte)iVar13 & 0x1f) & 0x1f));
          puVar5 = &local_24 + iVar2;
          _Ifp = (_LDBL12 *)(p_Var8->ld12 + *puVar5);
          if (_Ifp < (_LDBL12 *)*puVar5) goto LAB_1001c233;
          bVar14 = _Ifp < p_Var8;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_1001c23a;
LAB_1001c233:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_1001c23a:
              iVar2 = iVar2 + -1;
              *puVar5 = (uint)_Ifp;
              if ((iVar2 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_1001c248;
              }
              local_8 = (_LDBL12 *)0x0;
              puVar5 = &local_24 + iVar2;
              _Ifp = (_LDBL12 *)(((_LDBL12 *)*puVar5)->ld12 + 1);
            } while (_Ifp < (_LDBL12 *)*puVar5);
            bVar14 = _Ifp == (_LDBL12 *)0x0;
          } while( true );
        }
        iVar2 = iVar2 + 1;
        if (2 < iVar2) break;
        uVar7 = (&local_24)[iVar2];
      }
    }
LAB_1001c248:
    *puVar6 = *puVar6 & -1 << ((byte)local_1c[4] & 0x1f);
    iVar2 = local_1c[3] + 1;
    if (iVar2 < 3) {
      puVar6 = &local_24 + iVar2;
      for (iVar13 = 3 - iVar2; iVar13 != 0; iVar13 = iVar13 + -1) {
        *puVar6 = 0;
        puVar6 = puVar6 + 1;
      }
    }
    if (_Ifp != (_LDBL12 *)0x0) {
      iVar11 = uVar10 - 0x3ffe;
    }
    if (iVar11 < (int)(DAT_10036b9c - DAT_10036ba0)) {
      local_24 = 0;
      local_20 = 0.0;
    }
    else {
      if (DAT_10036b9c < iVar11) {
        if (iVar11 < DAT_10036b98) {
          local_24 = local_24 & 0x7fffffff;
          iVar11 = iVar11 + DAT_10036bac;
          iVar2 = (int)(DAT_10036ba4 + ((int)DAT_10036ba4 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10036ba4 & 0x8000001f;
          if ((int)uVar9 < 0) {
            uVar9 = (uVar9 - 1 | 0xffffffe0) + 1;
          }
          local_1c[3] = 0;
          _Ifp = (_LDBL12 *)0x0;
          local_8 = (_LDBL12 *)(0x20 - uVar9);
          do {
            local_1c[2] = (&local_24)[(int)_Ifp] & ~(-1 << ((byte)uVar9 & 0x1f));
            (&local_24)[(int)_Ifp] = (&local_24)[(int)_Ifp] >> ((byte)uVar9 & 0x1f) | local_1c[3];
            _Ifp = (_LDBL12 *)(_Ifp->ld12 + 1);
            local_1c[3] = local_1c[2] << ((byte)(0x20 - uVar9) & 0x1f);
          } while ((int)_Ifp < 3);
          iVar13 = 2;
          puVar6 = local_1c + -iVar2;
          do {
            if (iVar13 < iVar2) {
              (&local_24)[iVar13] = 0;
            }
            else {
              (&local_24)[iVar13] = *puVar6;
            }
            iVar13 = iVar13 + -1;
            puVar6 = puVar6 + -1;
          } while (-1 < iVar13);
          IVar3 = INTRNCVT_OK;
        }
        else {
          local_20 = 0.0;
          local_1c[0] = 0;
          local_24 = 0x80000000;
          iVar11 = (int)(DAT_10036ba4 + ((int)DAT_10036ba4 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10036ba4 & 0x8000001f;
          if ((int)uVar9 < 0) {
            uVar9 = (uVar9 - 1 | 0xffffffe0) + 1;
          }
          local_1c[3] = 0;
          _Ifp = (_LDBL12 *)0x0;
          local_8 = (_LDBL12 *)(0x20 - uVar9);
          do {
            uVar1 = (&local_24)[(int)_Ifp];
            local_1c[2] = uVar1 & ~(-1 << ((byte)uVar9 & 0x1f));
            (&local_24)[(int)_Ifp] = uVar1 >> ((byte)uVar9 & 0x1f) | local_1c[3];
            _Ifp = (_LDBL12 *)(_Ifp->ld12 + 1);
            local_1c[3] = local_1c[2] << ((byte)(0x20 - uVar9) & 0x1f);
          } while ((int)_Ifp < 3);
          iVar2 = 2;
          puVar6 = local_1c + -iVar11;
          do {
            if (iVar2 < iVar11) {
              (&local_24)[iVar2] = 0;
            }
            else {
              (&local_24)[iVar2] = *puVar6;
            }
            iVar2 = iVar2 + -1;
            puVar6 = puVar6 + -1;
          } while (-1 < iVar2);
          iVar11 = DAT_10036bac + DAT_10036b98;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_1001c5ff;
      }
      local_1c[2] = DAT_10036b9c - local_1c[2];
      local_24 = uVar9;
      local_20 = fVar12;
      iVar11 = (int)(local_1c[2] + ((int)local_1c[2] >> 0x1f & 0x1fU)) >> 5;
      uVar9 = local_1c[2] & 0x8000001f;
      if ((int)uVar9 < 0) {
        uVar9 = (uVar9 - 1 | 0xffffffe0) + 1;
      }
      local_1c[3] = 0;
      _Ifp = (_LDBL12 *)0x0;
      local_8 = (_LDBL12 *)(0x20 - uVar9);
      do {
        uVar1 = (&local_24)[(int)_Ifp];
        local_1c[2] = uVar1 & ~(-1 << ((byte)uVar9 & 0x1f));
        (&local_24)[(int)_Ifp] = uVar1 >> ((byte)uVar9 & 0x1f) | local_1c[3];
        _Ifp = (_LDBL12 *)(_Ifp->ld12 + 1);
        local_1c[3] = local_1c[2] << ((byte)(0x20 - uVar9) & 0x1f);
      } while ((int)_Ifp < 3);
      iVar2 = 2;
      puVar6 = local_1c + -iVar11;
      do {
        if (iVar2 < iVar11) {
          (&local_24)[iVar2] = 0;
        }
        else {
          (&local_24)[iVar2] = *puVar6;
        }
        iVar2 = iVar2 + -1;
        puVar6 = puVar6 + -1;
      } while (-1 < iVar2);
      iVar2 = DAT_10036ba0 - 1;
      iVar11 = (int)(DAT_10036ba0 + ((int)DAT_10036ba0 >> 0x1f & 0x1fU)) >> 5;
      uVar9 = DAT_10036ba0 & 0x8000001f;
      local_1c[3] = iVar11;
      if ((int)uVar9 < 0) {
        uVar9 = (uVar9 - 1 | 0xffffffe0) + 1;
      }
      bVar4 = (byte)(0x1f - uVar9);
      puVar6 = &local_24 + iVar11;
      local_1c[2] = 0x1f - uVar9;
      if ((*puVar6 & 1 << (bVar4 & 0x1f)) != 0) {
        uVar9 = (&local_24)[iVar11] & ~(-1 << (bVar4 & 0x1f));
        while (uVar9 == 0) {
          iVar11 = iVar11 + 1;
          if (2 < iVar11) goto LAB_1001c3eb;
          uVar9 = (&local_24)[iVar11];
        }
        iVar11 = (int)(iVar2 + (iVar2 >> 0x1f & 0x1fU)) >> 5;
        bVar14 = false;
        uVar10 = 1 << (0x1f - ((byte)iVar2 & 0x1f) & 0x1f);
        uVar1 = (&local_24)[iVar11];
        uVar9 = uVar1 + uVar10;
        if ((uVar9 < uVar1) || (uVar9 < uVar10)) {
          bVar14 = true;
        }
        (&local_24)[iVar11] = uVar9;
        while ((iVar11 = iVar11 + -1, -1 < iVar11 && (bVar14))) {
          uVar1 = (&local_24)[iVar11];
          uVar9 = uVar1 + 1;
          bVar14 = false;
          if ((uVar9 < uVar1) || (uVar9 == 0)) {
            bVar14 = true;
          }
          (&local_24)[iVar11] = uVar9;
        }
      }
LAB_1001c3eb:
      *puVar6 = *puVar6 & -1 << ((byte)local_1c[2] & 0x1f);
      iVar11 = local_1c[3] + 1;
      if (iVar11 < 3) {
        puVar6 = &local_24 + iVar11;
        for (iVar2 = 3 - iVar11; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar6 = 0;
          puVar6 = puVar6 + 1;
        }
      }
      uVar9 = DAT_10036ba4 + 1;
      iVar11 = (int)(uVar9 + ((int)uVar9 >> 0x1f & 0x1fU)) >> 5;
      uVar9 = uVar9 & 0x8000001f;
      if ((int)uVar9 < 0) {
        uVar9 = (uVar9 - 1 | 0xffffffe0) + 1;
      }
      local_1c[3] = 0;
      _Ifp = (_LDBL12 *)0x0;
      local_8 = (_LDBL12 *)(0x20 - uVar9);
      do {
        uVar1 = (&local_24)[(int)_Ifp];
        local_1c[2] = uVar1 & ~(-1 << ((byte)uVar9 & 0x1f));
        (&local_24)[(int)_Ifp] = uVar1 >> ((byte)uVar9 & 0x1f) | local_1c[3];
        _Ifp = (_LDBL12 *)(_Ifp->ld12 + 1);
        local_1c[3] = local_1c[2] << ((byte)(0x20 - uVar9) & 0x1f);
      } while ((int)_Ifp < 3);
      iVar2 = 2;
      puVar6 = local_1c + -iVar11;
      do {
        if (iVar2 < iVar11) {
          (&local_24)[iVar2] = 0;
        }
        else {
          (&local_24)[iVar2] = *puVar6;
        }
        iVar2 = iVar2 + -1;
        puVar6 = puVar6 + -1;
      } while (-1 < iVar2);
    }
    iVar11 = 0;
    IVar3 = INTRNCVT_UNDERFLOW;
  }
LAB_1001c5ff:
  fVar12 = (float)(iVar11 << (0x1fU - (char)DAT_10036ba4 & 0x1f) |
                   -(uint)(local_1c[1] != 0) & 0x80000000 | local_24);
  if (DAT_10036ba8 == 0x40) {
    _F[1].f = fVar12;
    _F->f = local_20;
  }
  else if (DAT_10036ba8 == 0x20) {
    _F->f = fVar12;
  }
  return IVar3;
}


// 1001c642 __crtGetStringTypeW_stat

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* Library Function - Single Match
    int __cdecl __crtGetStringTypeW_stat(struct localeinfo_struct *,unsigned long,wchar_t const
   *,int,unsigned short *,int,int)
   
   Library: Visual Studio 2005 Release */

int __cdecl
__crtGetStringTypeW_stat
          (localeinfo_struct *param_1,ulong param_2,wchar_t *param_3,int param_4,ushort *param_5,
          int param_6,int param_7)

{
  LPWORD pWVar1;
  uint uVar2;
  BOOL BVar3;
  DWORD DVar4;
  uint _Size;
  LPSTR pCVar5;
  LPWORD lpCharType;
  int iVar6;
  LPSTR local_c;
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  if (param_4 < -1) goto LAB_1001c854;
  if (DAT_1003a4e0 == 0) {
    BVar3 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_1003a4e0 = 2;
      }
      goto LAB_1001c6a9;
    }
    DAT_1003a4e0 = 1;
  }
  else {
LAB_1001c6a9:
    if (DAT_1003a4e0 != 1) {
      if ((DAT_1003a4e0 != 2) && (DAT_1003a4e0 != 0)) goto LAB_1001c854;
      if (param_7 == 0) {
        param_7 = (int)param_1->locinfo->lc_category[0].wlocale;
      }
      if (param_6 == 0) {
        param_6 = param_1->locinfo->lc_codepage;
      }
      iVar6 = ___ansicp(param_7);
      if ((param_6 != iVar6) && (iVar6 != -1)) {
        param_6 = iVar6;
      }
      _Size = WideCharToMultiByte(param_6,0,param_3,param_4,(LPSTR)0x0,0,(LPCSTR)0x0,(LPBOOL)0x0);
      if (_Size == 0) goto LAB_1001c854;
      if (((int)_Size < 1) || (0xffffffe0 < _Size)) {
        local_c = (LPSTR)0x0;
      }
      else if (_Size + 8 < 0x401) {
        pCVar5 = &stack0xffffffe0;
        local_c = &stack0xffffffe0;
        if (&stack0x00000000 != (undefined1 *)0x20) {
LAB_1001c75a:
          local_c = pCVar5 + 8;
        }
      }
      else {
        pCVar5 = (LPSTR)FUN_10001270(_Size + 8);
        local_c = pCVar5;
        if (pCVar5 != (LPSTR)0x0) {
          pCVar5[0] = -0x23;
          pCVar5[1] = -0x23;
          pCVar5[2] = '\0';
          pCVar5[3] = '\0';
          goto LAB_1001c75a;
        }
      }
      if (local_c == (LPSTR)0x0) goto LAB_1001c854;
      _memset(local_c,0,_Size);
      iVar6 = WideCharToMultiByte(param_6,0,param_3,param_4,local_c,_Size,(LPCSTR)0x0,(LPBOOL)0x0);
      if (iVar6 != 0) {
        if (((int)(_Size + 1) < 1) || (0x7ffffff0 < _Size + 1)) {
          lpCharType = (LPWORD)0x0;
        }
        else {
          uVar2 = _Size * 2 + 10;
          if (uVar2 < 0x401) {
            if (&stack0x00000000 == (undefined1 *)0x20) goto LAB_1001c848;
            lpCharType = (LPWORD)&stack0xffffffe8;
          }
          else {
            lpCharType = (LPWORD)FUN_10001270(uVar2);
            if (lpCharType != (LPWORD)0x0) {
              lpCharType[0] = 0xdddd;
              lpCharType[1] = 0;
              lpCharType = lpCharType + 4;
            }
          }
        }
        pCVar5 = local_c;
        if (lpCharType != (LPWORD)0x0) {
          if (param_7 == 0) {
            param_7 = (int)param_1->locinfo->lc_category[0].wlocale;
          }
          pWVar1 = lpCharType + param_4;
          *pWVar1 = 0xffff;
          pWVar1[-1] = 0xffff;
          GetStringTypeA(param_7,param_2,pCVar5,_Size,lpCharType);
          if ((pWVar1[-1] != 0xffff) && (*pWVar1 == 0xffff)) {
            _memmove(param_5,lpCharType,param_4 * 2);
          }
          __freea(lpCharType);
        }
      }
LAB_1001c848:
      __freea(local_c);
      goto LAB_1001c854;
    }
  }
  GetStringTypeW(param_2,param_3,param_4,param_5);
LAB_1001c854:
  iVar6 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar6;
}


// 1001c866 ___crtGetStringTypeW

/* Library Function - Single Match
    ___crtGetStringTypeW
   
   Library: Visual Studio 2005 Release */

void __cdecl
___crtGetStringTypeW
          (localeinfo_struct *param_1,ulong param_2,wchar_t *param_3,int param_4,ushort *param_5,
          int param_6,int param_7)

{
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,param_1);
  __crtGetStringTypeW_stat(&local_14,param_2,param_3,param_4,param_5,param_6,param_7);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return;
}


// 1001c8a4 ___mtold12

/* Library Function - Single Match
    ___mtold12
   
   Library: Visual Studio 2005 Release */

void __cdecl ___mtold12(char *param_1,int param_2,uint *param_3)

{
  uint uVar1;
  uint uVar2;
  uint uVar3;
  bool bVar4;
  bool bVar5;
  short sVar6;
  uint uVar7;
  uint uVar8;
  uint uVar9;
  uint uVar10;
  uint uVar11;
  
  uVar7 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  sVar6 = 0x404e;
  *param_3 = 0;
  param_3[1] = 0;
  param_3[2] = 0;
  if (param_2 != 0) {
    do {
      uVar2 = *param_3;
      uVar10 = *param_3;
      uVar1 = param_3[1];
      uVar11 = param_3[2];
      uVar9 = param_3[1] * 2;
      bVar4 = false;
      uVar8 = (param_3[2] * 2 | param_3[1] >> 0x1f) * 2 | uVar9 >> 0x1f;
      uVar3 = uVar2 * 4;
      uVar9 = (uVar9 | uVar2 >> 0x1f) * 2 | uVar2 * 2 >> 0x1f;
      uVar2 = uVar3 + uVar10;
      *param_3 = uVar3;
      param_3[1] = uVar9;
      param_3[2] = uVar8;
      if ((uVar2 < uVar3) || (uVar2 < uVar10)) {
        bVar4 = true;
      }
      bVar5 = false;
      *param_3 = uVar2;
      if (bVar4) {
        uVar10 = uVar9 + 1;
        if ((uVar10 < uVar9) || (uVar10 == 0)) {
          bVar5 = true;
        }
        param_3[1] = uVar10;
        if (bVar5) {
          param_3[2] = uVar8 + 1;
        }
      }
      uVar10 = param_3[1] + uVar1;
      bVar4 = false;
      if ((uVar10 < param_3[1]) || (uVar10 < uVar1)) {
        bVar4 = true;
      }
      param_3[1] = uVar10;
      if (bVar4) {
        param_3[2] = param_3[2] + 1;
      }
      param_3[2] = param_3[2] + uVar11;
      bVar4 = false;
      uVar1 = uVar2 * 2;
      uVar11 = uVar10 * 2 | uVar2 >> 0x1f;
      uVar10 = param_3[2] * 2 | uVar10 >> 0x1f;
      *param_3 = uVar1;
      param_3[1] = uVar11;
      param_3[2] = uVar10;
      uVar2 = uVar1 + (int)*param_1;
      if ((uVar2 < uVar1) || (uVar2 < (uint)(int)*param_1)) {
        bVar4 = true;
      }
      *param_3 = uVar2;
      if (bVar4) {
        uVar2 = uVar11 + 1;
        bVar4 = false;
        if ((uVar2 < uVar11) || (uVar2 == 0)) {
          bVar4 = true;
        }
        param_3[1] = uVar2;
        if (bVar4) {
          param_3[2] = uVar10 + 1;
        }
      }
      param_2 = param_2 + -1;
      param_1 = param_1 + 1;
    } while (param_2 != 0);
  }
  while (param_3[2] == 0) {
    param_3[2] = param_3[1] >> 0x10;
    sVar6 = sVar6 + -0x10;
    param_3[1] = param_3[1] << 0x10 | *param_3 >> 0x10;
    *param_3 = *param_3 << 0x10;
  }
  uVar2 = param_3[2];
  while ((uVar2 & 0x8000) == 0) {
    uVar10 = *param_3;
    uVar1 = param_3[1];
    sVar6 = sVar6 + -1;
    *param_3 = uVar10 * 2;
    uVar2 = param_3[2] * 2;
    param_3[1] = uVar1 * 2 | uVar10 >> 0x1f;
    param_3[2] = uVar2 | uVar1 >> 0x1f;
  }
  *(short *)((int)param_3 + 10) = sVar6;
  __security_check_cookie(uVar7 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001ca70 ___report_gsfailure

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    ___report_gsfailure
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release */

void __cdecl ___report_gsfailure(void)

{
  undefined4 in_EAX;
  HANDLE hProcess;
  undefined4 in_ECX;
  undefined4 in_EDX;
  undefined4 unaff_EBX;
  undefined4 unaff_EBP;
  undefined4 unaff_ESI;
  undefined4 unaff_EDI;
  undefined2 in_ES;
  undefined2 in_CS;
  undefined2 in_SS;
  undefined2 in_DS;
  undefined2 in_FS;
  undefined2 in_GS;
  byte in_AF;
  byte in_TF;
  byte in_IF;
  byte in_NT;
  byte in_AC;
  byte in_VIF;
  byte in_VIP;
  byte in_ID;
  undefined4 unaff_retaddr;
  UINT uExitCode;
  undefined4 local_32c;
  undefined4 local_328;
  
  _DAT_1003a600 =
       (uint)(in_NT & 1) * 0x4000 | (uint)SBORROW4((int)&stack0xfffffffc,0x328) * 0x800 |
       (uint)(in_IF & 1) * 0x200 | (uint)(in_TF & 1) * 0x100 | (uint)((int)&local_32c < 0) * 0x80 |
       (uint)(&stack0x00000000 == (undefined1 *)0x32c) * 0x40 | (uint)(in_AF & 1) * 0x10 |
       (uint)((POPCOUNT((uint)&local_32c & 0xff) & 1U) == 0) * 4 |
       (uint)(&stack0xfffffffc < (undefined1 *)0x328) | (uint)(in_ID & 1) * 0x200000 |
       (uint)(in_VIP & 1) * 0x100000 | (uint)(in_VIF & 1) * 0x80000 | (uint)(in_AC & 1) * 0x40000;
  _DAT_1003a604 = &stack0x00000004;
  _DAT_1003a540 = 0x10001;
  _DAT_1003a4e8 = 0xc0000409;
  _DAT_1003a4ec = 1;
  local_32c = DAT_100366b8;
  local_328 = DAT_100366bc;
  _DAT_1003a4f4 = unaff_retaddr;
  _DAT_1003a5cc = in_GS;
  _DAT_1003a5d0 = in_FS;
  _DAT_1003a5d4 = in_ES;
  _DAT_1003a5d8 = in_DS;
  _DAT_1003a5dc = unaff_EDI;
  _DAT_1003a5e0 = unaff_ESI;
  _DAT_1003a5e4 = unaff_EBX;
  _DAT_1003a5e8 = in_EDX;
  _DAT_1003a5ec = in_ECX;
  _DAT_1003a5f0 = in_EAX;
  _DAT_1003a5f4 = unaff_EBP;
  DAT_1003a5f8 = unaff_retaddr;
  _DAT_1003a5fc = in_CS;
  _DAT_1003a608 = in_SS;
  DAT_1003a538 = IsDebuggerPresent();
  FUN_10019895();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  UnhandledExceptionFilter((_EXCEPTION_POINTERS *)&PTR_DAT_1002aa04);
  if (DAT_1003a538 == 0) {
    FUN_10019895();
  }
  uExitCode = 0xc0000409;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  return;
}


// 1001cb74 FUN_1001cb74

undefined4 __cdecl FUN_1001cb74(uint param_1)

{
  int *piVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD nStdHandle;
  
  if ((-1 < (int)param_1) && (param_1 < DAT_1004bbc4)) {
    iVar3 = (param_1 & 0x1f) * 0x38;
    piVar1 = (int *)((&DAT_1004bbe0)[(int)param_1 >> 5] + iVar3);
    if (((*(byte *)(piVar1 + 1) & 1) != 0) && (*piVar1 != -1)) {
      if (DAT_10039e28 == 1) {
        if (param_1 == 0) {
          nStdHandle = 0xfffffff6;
        }
        else if (param_1 == 1) {
          nStdHandle = 0xfffffff5;
        }
        else {
          if (param_1 != 2) goto LAB_1001cbd2;
          nStdHandle = 0xfffffff4;
        }
        SetStdHandle(nStdHandle,(HANDLE)0x0);
      }
LAB_1001cbd2:
      *(undefined4 *)(iVar3 + (&DAT_1004bbe0)[(int)param_1 >> 5]) = 0xffffffff;
      return 0;
    }
  }
  pDVar2 = FUN_10011e63();
  *pDVar2 = 9;
  pDVar2 = FUN_10011e76();
  *pDVar2 = 0;
  return 0xffffffff;
}


// 1001cbf5 __get_osfhandle

/* Library Function - Single Match
    __get_osfhandle
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __get_osfhandle(int _FileHandle)

{
  DWORD *pDVar1;
  intptr_t *piVar2;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_10011e76();
    *pDVar1 = 0;
    pDVar1 = FUN_10011e63();
    *pDVar1 = 9;
    return -1;
  }
  if (((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1004bbc4)) &&
     (piVar2 = (intptr_t *)((_FileHandle & 0x1fU) * 0x38 + (&DAT_1004bbe0)[_FileHandle >> 5]),
     (*(byte *)(piVar2 + 1) & 1) != 0)) {
    return *piVar2;
  }
  pDVar1 = FUN_10011e76();
  *pDVar1 = 0;
  pDVar1 = FUN_10011e63();
  *pDVar1 = 9;
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1001cc66 ___lock_fhandle

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___lock_fhandle
   
   Library: Visual Studio 2005 Release */

int __cdecl ___lock_fhandle(int _Filehandle)

{
  int iVar1;
  int iVar2;
  uint local_20;
  
  iVar2 = (_Filehandle & 0x1fU) * 0x38 + (&DAT_1004bbe0)[_Filehandle >> 5];
  local_20 = 1;
  if (*(int *)(iVar2 + 8) == 0) {
    __lock(10);
    if (*(int *)(iVar2 + 8) == 0) {
      iVar1 = ___crtInitCritSecAndSpinCount(iVar2 + 0xc,4000);
      local_20 = (uint)(iVar1 != 0);
      *(int *)(iVar2 + 8) = *(int *)(iVar2 + 8) + 1;
    }
    FUN_1001ccfd();
  }
  if (local_20 != 0) {
    EnterCriticalSection
              ((LPCRITICAL_SECTION)
               ((&DAT_1004bbe0)[_Filehandle >> 5] + 0xc + (_Filehandle & 0x1fU) * 0x38));
  }
  return local_20;
}


// 1001ccfd FUN_1001ccfd

void FUN_1001ccfd(void)

{
  FUN_100189ed(10);
  return;
}


// 1001cd06 FUN_1001cd06

void __cdecl FUN_1001cd06(uint param_1)

{
  LeaveCriticalSection
            ((LPCRITICAL_SECTION)
             ((&DAT_1004bbe0)[(int)param_1 >> 5] + 0xc + (param_1 & 0x1f) * 0x38));
  return;
}


// 1001cd28 __putwch_nolock

/* Library Function - Single Match
    __putwch_nolock
   
   Library: Visual Studio 2005 Release */

wint_t __cdecl __putwch_nolock(wchar_t _WCh)

{
  wint_t wVar1;
  BOOL BVar2;
  DWORD DVar3;
  UINT CodePage;
  wchar_t *lpWideCharStr;
  int cchWideChar;
  CHAR *lpMultiByteStr;
  int cbMultiByte;
  LPCSTR lpDefaultChar;
  LPBOOL lpUsedDefaultChar;
  DWORD local_14;
  CHAR local_10 [8];
  uint local_8;
  
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  if (DAT_10036e6c != 0) {
    if (DAT_10037034 == (HANDLE)0xfffffffe) {
      ___initconout();
    }
    if (DAT_10037034 == (HANDLE)0xffffffff) goto LAB_1001cdd1;
    BVar2 = WriteConsoleW(DAT_10037034,&_WCh,1,&local_14,(LPVOID)0x0);
    if (BVar2 != 0) {
      DAT_10036e6c = 1;
      goto LAB_1001cdd1;
    }
    if ((DAT_10036e6c != 2) || (DVar3 = GetLastError(), DVar3 != 0x78)) goto LAB_1001cdd1;
    DAT_10036e6c = 0;
  }
  lpUsedDefaultChar = (LPBOOL)0x0;
  lpDefaultChar = (LPCSTR)0x0;
  cbMultiByte = 5;
  lpMultiByteStr = local_10;
  cchWideChar = 1;
  lpWideCharStr = &_WCh;
  DVar3 = 0;
  CodePage = GetConsoleOutputCP();
  DVar3 = WideCharToMultiByte(CodePage,DVar3,lpWideCharStr,cchWideChar,lpMultiByteStr,cbMultiByte,
                              lpDefaultChar,lpUsedDefaultChar);
  if (DAT_10037034 != (HANDLE)0xffffffff) {
    WriteConsoleA(DAT_10037034,local_10,DVar3,&local_14,(LPVOID)0x0);
  }
LAB_1001cdd1:
  wVar1 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return wVar1;
}


// 1001d010 FUN_1001d010

/* WARNING: Removing unreachable block (ram,0x1001d501) */
/* WARNING: Removing unreachable block (ram,0x1001d50b) */
/* WARNING: Removing unreachable block (ram,0x1001d510) */

void __cdecl
FUN_1001d010(int param_1,uint param_2,ushort param_3,int param_4,byte param_5,short *param_6)

{
  int iVar1;
  undefined **ppuVar2;
  int iVar3;
  bool bVar4;
  int iVar5;
  errno_t eVar6;
  short *psVar7;
  ushort uVar8;
  ushort uVar9;
  undefined **ppuVar10;
  int iVar11;
  uint uVar12;
  ushort uVar13;
  uint uVar14;
  char cVar15;
  uint uVar16;
  int iVar17;
  short *psVar18;
  short *psVar19;
  int iVar20;
  uint uVar21;
  uint uVar22;
  ushort uVar23;
  ushort uVar24;
  char *pcVar25;
  undefined **local_6c;
  ushort *local_60;
  ushort *local_5c;
  undefined **local_58;
  int local_54;
  short local_50;
  ushort *local_4c;
  uint local_48;
  int local_44;
  undefined2 local_40;
  undefined4 uStack_3e;
  ushort uStack_3a;
  undefined *local_38;
  undefined4 local_34;
  undefined4 local_30;
  undefined2 local_2c;
  undefined1 uStack_2a;
  undefined1 uStack_29;
  undefined4 local_24;
  undefined4 uStack_20;
  ushort uStack_1c;
  undefined1 local_1a;
  byte bStack_19;
  byte local_14;
  undefined1 uStack_13;
  ushort uStack_12;
  undefined4 local_10;
  ushort local_c;
  ushort uStack_a;
  uint local_8;
  
  uVar16 = CONCAT22(local_24._2_2_,(undefined2)local_24);
  iVar1 = CONCAT22(uStack_3e._2_2_,(undefined2)uStack_3e);
  local_8 = DAT_100366b8 ^ (uint)&stack0xfffffffc;
  local_14 = (byte)param_1;
  uStack_13 = (undefined1)((uint)param_1 >> 8);
  uStack_12 = (ushort)((uint)param_1 >> 0x10);
  local_10._0_2_ = (ushort)param_2;
  iVar17 = CONCAT22((ushort)local_10,uStack_12);
  local_10._2_2_ = (ushort)(param_2 >> 0x10);
  local_c = param_3;
  uVar8 = param_3 & 0x8000;
  uVar14 = param_3 & 0x7fff;
  local_34 = 0xcccccccc;
  local_30 = 0xcccccccc;
  local_2c._0_1_ = 0xcc;
  local_2c._1_1_ = 0xcc;
  uStack_2a = 0xfb;
  uStack_29 = 0x3f;
  if (uVar8 == 0) {
    *(undefined1 *)(param_6 + 1) = 0x20;
  }
  else {
    *(undefined1 *)(param_6 + 1) = 0x2d;
  }
  if ((((short)uVar14 == 0) && (param_2 == 0)) && (param_1 == 0)) {
    *param_6 = 0;
    *(byte *)(param_6 + 1) = ((uVar8 != 0x8000) - 1U & 0xd) + 0x20;
    *(undefined1 *)((int)param_6 + 3) = 1;
    *(undefined1 *)(param_6 + 2) = 0x30;
    *(undefined1 *)((int)param_6 + 5) = 0;
    goto LAB_1001d88e;
  }
  if ((short)uVar14 == 0x7fff) {
    *param_6 = 1;
    if (((param_2 == 0x80000000) && (param_1 == 0)) || ((param_2 & 0x40000000) != 0)) {
      if ((uVar8 == 0) || (param_2 != 0xc0000000)) {
        if ((param_2 != 0x80000000) || (param_1 != 0)) goto LAB_1001d13f;
        pcVar25 = "1#INF";
      }
      else {
        if (param_1 != 0) {
LAB_1001d13f:
          pcVar25 = "1#QNAN";
          goto LAB_1001d144;
        }
        pcVar25 = "1#IND";
      }
      eVar6 = _strcpy_s((char *)(param_6 + 2),0x16,pcVar25);
      if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      *(undefined1 *)((int)param_6 + 3) = 5;
    }
    else {
      pcVar25 = "1#SNAN";
LAB_1001d144:
      eVar6 = _strcpy_s((char *)(param_6 + 2),0x16,pcVar25);
      if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      *(undefined1 *)((int)param_6 + 3) = 6;
    }
    iVar1 = CONCAT22(uStack_3e._2_2_,(undefined2)uStack_3e);
    param_2 = CONCAT22(local_10._2_2_,(ushort)local_10);
    uVar16 = CONCAT22(local_24._2_2_,(undefined2)local_24);
    goto LAB_1001d88e;
  }
  local_50 = (short)(((uVar14 >> 8) + (param_2 >> 0x18) * 2) * 0x4d + -0x134312f4 + uVar14 * 0x4d10
                    >> 0x10);
  uVar16 = (uint)local_50;
  local_1a = (undefined1)uVar14;
  bStack_19 = (byte)(uVar14 >> 8);
  local_24._2_2_ = (ushort)param_1;
  local_24._0_2_ = 0;
  local_6c = &PTR_DAT_10036b50;
  uStack_20._0_2_ = uStack_12;
  uStack_20._2_2_ = (ushort)local_10;
  uStack_1c = local_10._2_2_;
  if (uVar16 != 0) {
    iVar20 = param_1;
    uVar14 = -uVar16;
    if (0 < (int)uVar16) {
      local_6c = (undefined **)&DAT_10036cb0;
      uVar14 = uVar16;
    }
    while (uVar14 != 0) {
      uStack_20._0_2_ = (ushort)((uint)iVar20 >> 0x10);
      local_24._2_2_ = (ushort)iVar20;
      local_6c = local_6c + 0x15;
      iVar3 = CONCAT22(local_c,local_10._2_2_);
      if ((uVar14 & 7) == 0) goto LAB_1001d465;
      ppuVar10 = local_6c + (uVar14 & 7) * 3;
      if (0x7fff < *(ushort *)ppuVar10) {
        local_40 = SUB42(*ppuVar10,0);
        uStack_3e._0_2_ = (undefined2)((uint)*ppuVar10 >> 0x10);
        ppuVar2 = ppuVar10 + 2;
        uStack_3e._2_2_ = SUB42(ppuVar10[1],0);
        uStack_3a = (ushort)((uint)ppuVar10[1] >> 0x10);
        ppuVar10 = (undefined **)&local_40;
        local_38 = *ppuVar2;
        iVar1 = CONCAT22(uStack_3e._2_2_,(undefined2)uStack_3e) + -1;
        uStack_3e._0_2_ = (undefined2)iVar1;
        uStack_3e._2_2_ = (undefined2)((uint)iVar1 >> 0x10);
      }
      uVar13 = CONCAT11(bStack_19,local_1a) & 0x7fff;
      uVar23 = *(ushort *)((int)ppuVar10 + 10) & 0x7fff;
      local_4c = (ushort *)0x0;
      local_14 = 0;
      uStack_13 = 0;
      uStack_12 = 0;
      local_10._0_2_ = 0;
      iVar11 = 0;
      local_10._2_2_ = 0;
      local_c = 0;
      iVar5 = 0;
      uStack_a = 0;
      uVar9 = (*(ushort *)((int)ppuVar10 + 10) ^ CONCAT11(bStack_19,local_1a)) & 0x8000;
      uVar24 = uVar23 + uVar13;
      iVar17 = 0;
      iVar3 = 0;
      if (((uVar13 < 0x7fff) && (iVar17 = 0, iVar3 = 0, uVar23 < 0x7fff)) &&
         (iVar17 = iVar11, iVar3 = iVar5, uVar24 < 0xbffe)) {
        if (uVar24 < 0x3fc0) {
          uStack_1c = 0;
          local_1a = 0;
          bStack_19 = 0;
          uStack_20._0_2_ = 0;
          uStack_20._2_2_ = 0;
          local_24._0_2_ = 0;
          local_24._2_2_ = 0;
          iVar20 = 0;
        }
        else if (((uVar13 == 0) &&
                 (uVar24 = uVar24 + 1,
                 (CONCAT13(bStack_19,CONCAT12(local_1a,uStack_1c)) & 0x7fffffff) == 0)) &&
                ((CONCAT22(uStack_20._2_2_,(ushort)uStack_20) == 0 &&
                 (CONCAT22(local_24._2_2_,(undefined2)local_24) == 0)))) {
          local_1a = 0;
          bStack_19 = 0;
        }
        else if ((((uVar23 == 0) && (uVar24 = uVar24 + 1, ((uint)ppuVar10[2] & 0x7fffffff) == 0)) &&
                 (ppuVar10[1] == (undefined *)0x0)) && (*ppuVar10 == (undefined *)0x0)) {
          uStack_1c = 0;
          local_1a = 0;
          bStack_19 = 0;
          uStack_20._0_2_ = 0;
          uStack_20._2_2_ = 0;
          local_24._0_2_ = 0;
          local_24._2_2_ = 0;
          iVar20 = 0;
        }
        else {
          psVar7 = (short *)&local_10;
          local_60 = (ushort *)0x0;
          local_44 = 5;
          do {
            local_54 = local_44;
            if (0 < local_44) {
              local_5c = (ushort *)((int)&local_24 + (int)local_60 * 2);
              local_58 = ppuVar10 + 2;
              do {
                bVar4 = false;
                uVar16 = *(uint *)(psVar7 + -2) + (uint)*local_5c * (uint)*(ushort *)local_58;
                if ((uVar16 < *(uint *)(psVar7 + -2)) ||
                   (uVar16 < (uint)*local_5c * (uint)*(ushort *)local_58)) {
                  bVar4 = true;
                }
                *(uint *)(psVar7 + -2) = uVar16;
                if (bVar4) {
                  *psVar7 = *psVar7 + 1;
                }
                local_5c = local_5c + 1;
                local_58 = (undefined **)((int)local_58 + -2);
                local_54 = local_54 + -1;
              } while (0 < local_54);
            }
            psVar7 = psVar7 + 1;
            local_60 = (ushort *)((int)local_60 + 1);
            local_44 = local_44 + -1;
          } while (0 < local_44);
          uVar24 = uVar24 + 0xc002;
          if ((short)uVar24 < 1) {
LAB_1001d389:
            uVar24 = uVar24 - 1;
            if ((short)uVar24 < 0) {
              local_48 = (uint)(ushort)-uVar24;
              uVar24 = 0;
              do {
                if ((local_14 & 1) != 0) {
                  local_4c = (ushort *)((int)local_4c + 1);
                }
                iVar20 = CONCAT22(uStack_a,local_c);
                uVar16 = CONCAT22(local_10._2_2_,(ushort)local_10);
                iVar17 = CONCAT22(local_10._2_2_,(ushort)local_10);
                local_c = (ushort)(CONCAT22(uStack_a,local_c) >> 1);
                uStack_a = uStack_a >> 1;
                local_10._2_2_ = local_10._2_2_ >> 1 | (ushort)((uint)(iVar20 << 0x1f) >> 0x10);
                uVar12 = CONCAT22(uStack_12,CONCAT11(uStack_13,local_14)) >> 1;
                uStack_12 = uStack_12 >> 1 | (ushort)((uint)(iVar17 << 0x1f) >> 0x10);
                local_48 = local_48 - 1;
                local_10._0_2_ = (ushort)(uVar16 >> 1);
                local_14 = (byte)uVar12;
                uStack_13 = (undefined1)(uVar12 >> 8);
              } while (local_48 != 0);
              if (local_4c != (ushort *)0x0) {
                local_14 = local_14 | 1;
              }
            }
          }
          else {
            do {
              uVar13 = uStack_12;
              if ((uStack_a & 0x8000) != 0) break;
              iVar17 = CONCAT22(uStack_12,CONCAT11(uStack_13,local_14)) << 1;
              local_14 = (byte)iVar17;
              uStack_13 = (undefined1)((uint)iVar17 >> 8);
              uStack_12 = (ushort)((uint)iVar17 >> 0x10);
              iVar17 = CONCAT22(local_10._2_2_,(ushort)local_10) * 2;
              local_10._0_2_ = (ushort)iVar17 | uVar13 >> 0xf;
              iVar20 = CONCAT22(uStack_a,local_c) * 2;
              local_c = (ushort)iVar20 | local_10._2_2_ >> 0xf;
              uVar24 = uVar24 - 1;
              local_10._2_2_ = (ushort)((uint)iVar17 >> 0x10);
              uStack_a = (ushort)((uint)iVar20 >> 0x10);
            } while (0 < (short)uVar24);
            if ((short)uVar24 < 1) goto LAB_1001d389;
          }
          if ((0x8000 < CONCAT11(uStack_13,local_14)) ||
             (iVar3 = CONCAT22(local_c,local_10._2_2_),
             iVar17 = CONCAT22((ushort)local_10,uStack_12),
             (CONCAT22(uStack_12,CONCAT11(uStack_13,local_14)) & 0x1ffff) == 0x18000)) {
            if (CONCAT22((ushort)local_10,uStack_12) == -1) {
              uStack_12 = 0;
              local_10._0_2_ = 0;
              iVar17 = 0;
              if (CONCAT22(local_c,local_10._2_2_) == -1) {
                local_10._2_2_ = 0;
                local_c = 0;
                if (uStack_a == 0xffff) {
                  uStack_a = 0x8000;
                  uVar24 = uVar24 + 1;
                  iVar3 = 0;
                  iVar17 = 0;
                }
                else {
                  uStack_a = uStack_a + 1;
                  iVar3 = 0;
                  iVar17 = 0;
                }
              }
              else {
                iVar3 = CONCAT22(local_c,local_10._2_2_) + 1;
                local_10._2_2_ = (ushort)iVar3;
                local_c = (ushort)((uint)iVar3 >> 0x10);
              }
            }
            else {
              iVar17 = CONCAT22((ushort)local_10,uStack_12) + 1;
              uStack_12 = (ushort)iVar17;
              local_10._0_2_ = (ushort)((uint)iVar17 >> 0x10);
              iVar3 = CONCAT22(local_c,local_10._2_2_);
            }
          }
          local_10._0_2_ = (ushort)((uint)iVar17 >> 0x10);
          uStack_12 = (ushort)iVar17;
          local_c = (ushort)((uint)iVar3 >> 0x10);
          local_10._2_2_ = (ushort)iVar3;
          if (0x7ffe < uVar24) goto LAB_1001d449;
          local_24._0_2_ = uStack_12;
          local_24._2_2_ = (ushort)local_10;
          uStack_20._0_2_ = local_10._2_2_;
          iVar20 = CONCAT22(local_10._2_2_,(ushort)local_10);
          bStack_19 = (byte)(uVar24 >> 8) | (byte)(uVar9 >> 8);
          uStack_20._2_2_ = local_c;
          uStack_1c = uStack_a;
          local_1a = (undefined1)uVar24;
        }
      }
      else {
LAB_1001d449:
        uStack_20._0_2_ = 0;
        uStack_20._2_2_ = 0;
        iVar11 = (-(uint)(uVar9 != 0) & 0x80000000) + 0x7fff8000;
        local_24._0_2_ = 0;
        local_24._2_2_ = 0;
        iVar20 = 0;
        uStack_1c = (ushort)iVar11;
        local_1a = (undefined1)((uint)iVar11 >> 0x10);
        bStack_19 = (byte)((uint)iVar11 >> 0x18);
      }
LAB_1001d465:
      uStack_20._0_2_ = (ushort)((uint)iVar20 >> 0x10);
      local_24._2_2_ = (ushort)iVar20;
      local_c = (ushort)((uint)iVar3 >> 0x10);
      local_10._2_2_ = (ushort)iVar3;
      local_10._0_2_ = (ushort)((uint)iVar17 >> 0x10);
      uStack_12 = (ushort)iVar17;
      param_1 = CONCAT22(uStack_12,local_24._2_2_);
      param_2 = CONCAT22(local_10._2_2_,(ushort)local_10);
      uVar14 = (int)uVar14 >> 3;
    }
  }
  local_24._2_2_ = (ushort)param_1;
  uVar16 = CONCAT13(bStack_19,CONCAT12(local_1a,uStack_1c));
  uStack_20 = CONCAT22(uStack_20._2_2_,(ushort)uStack_20);
  if (0x3ffe < (ushort)(uVar16 >> 0x10)) {
    local_50 = local_50 + 1;
    uVar16 = uVar16 >> 0x10 & 0x7fff;
    iVar17 = uVar16 + 0x3ffb;
    local_5c = (ushort *)0x0;
    local_14 = 0;
    uStack_13 = 0;
    uStack_12 = 0;
    param_1 = param_1 & 0xffff;
    local_10._0_2_ = 0;
    local_10._2_2_ = 0;
    param_2 = 0;
    local_c = 0;
    uStack_a = 0;
    if (((ushort)uVar16 < 0x7fff) && ((ushort)iVar17 < 0xbffe)) {
      if (0x3fbf < (ushort)iVar17) {
        if (((((ushort)uVar16 == 0) &&
             (iVar17 = uVar16 + 0x3ffc,
             (CONCAT13(bStack_19,CONCAT12(local_1a,uStack_1c)) & 0x7fffffff) == 0)) &&
            (CONCAT22(uStack_20._2_2_,(ushort)uStack_20) == 0)) &&
           (CONCAT22(local_24._2_2_,(undefined2)local_24) == 0)) {
          local_1a = 0;
          bStack_19 = 0;
          uStack_20 = CONCAT22(uStack_20._2_2_,(ushort)uStack_20);
          goto LAB_1001d6cd;
        }
        local_58 = (undefined **)0x0;
        psVar7 = (short *)&local_10;
        local_44 = 5;
        do {
          local_54 = local_44;
          if (0 < local_44) {
            local_60 = &local_2c;
            local_4c = (ushort *)((int)&local_24 + (int)local_58 * 2);
            do {
              bVar4 = false;
              uVar16 = *(uint *)(psVar7 + -2) + (uint)*local_60 * (uint)*local_4c;
              if ((uVar16 < *(uint *)(psVar7 + -2)) || (uVar16 < (uint)*local_60 * (uint)*local_4c))
              {
                bVar4 = true;
              }
              *(uint *)(psVar7 + -2) = uVar16;
              if (bVar4) {
                *psVar7 = *psVar7 + 1;
              }
              local_4c = local_4c + 1;
              local_60 = local_60 + -1;
              local_54 = local_54 + -1;
            } while (0 < local_54);
          }
          psVar7 = psVar7 + 1;
          local_58 = (undefined **)((int)local_58 + 1);
          local_44 = local_44 + -1;
        } while (0 < local_44);
        iVar17 = iVar17 + 0xc002;
        if ((short)iVar17 < 1) {
LAB_1001d5db:
          uVar24 = (ushort)(iVar17 + 0xffff);
          if ((short)uVar24 < 0) {
            uVar16 = -(iVar17 + 0xffff);
            uVar14 = uVar16 & 0xffff;
            uVar24 = uVar24 + (short)uVar16;
            do {
              if ((local_14 & 1) != 0) {
                local_5c = (ushort *)((int)local_5c + 1);
              }
              iVar20 = CONCAT22(uStack_a,local_c);
              uVar16 = CONCAT22(local_10._2_2_,(ushort)local_10);
              iVar17 = CONCAT22(local_10._2_2_,(ushort)local_10);
              local_c = (ushort)(CONCAT22(uStack_a,local_c) >> 1);
              uStack_a = uStack_a >> 1;
              local_10._2_2_ = local_10._2_2_ >> 1 | (ushort)((uint)(iVar20 << 0x1f) >> 0x10);
              uVar12 = CONCAT22(uStack_12,CONCAT11(uStack_13,local_14)) >> 1;
              uStack_12 = uStack_12 >> 1 | (ushort)((uint)(iVar17 << 0x1f) >> 0x10);
              uVar14 = uVar14 - 1;
              local_10._0_2_ = (ushort)(uVar16 >> 1);
              local_14 = (byte)uVar12;
              uStack_13 = (undefined1)(uVar12 >> 8);
            } while (uVar14 != 0);
            if (local_5c != (ushort *)0x0) {
              local_14 = local_14 | 1;
            }
          }
        }
        else {
          do {
            uVar24 = uStack_12;
            if ((uStack_a & 0x8000) != 0) break;
            iVar20 = CONCAT22(uStack_12,CONCAT11(uStack_13,local_14)) << 1;
            local_14 = (byte)iVar20;
            uStack_13 = (undefined1)((uint)iVar20 >> 8);
            uStack_12 = (ushort)((uint)iVar20 >> 0x10);
            iVar20 = CONCAT22(local_10._2_2_,(ushort)local_10) * 2;
            local_10._0_2_ = (ushort)iVar20 | uVar24 >> 0xf;
            iVar3 = CONCAT22(uStack_a,local_c) * 2;
            local_c = (ushort)iVar3 | local_10._2_2_ >> 0xf;
            iVar17 = iVar17 + 0xffff;
            local_10._2_2_ = (ushort)((uint)iVar20 >> 0x10);
            uStack_a = (ushort)((uint)iVar3 >> 0x10);
          } while (0 < (short)iVar17);
          uVar24 = (ushort)iVar17;
          if ((short)uVar24 < 1) goto LAB_1001d5db;
        }
        iVar17 = 0;
        if ((0x8000 < CONCAT11(uStack_13,local_14)) ||
           (uStack_20 = CONCAT22(local_c,local_10._2_2_),
           iVar20 = CONCAT22((ushort)local_10,uStack_12),
           (CONCAT22(uStack_12,CONCAT11(uStack_13,local_14)) & 0x1ffff) == 0x18000)) {
          if (CONCAT22((ushort)local_10,uStack_12) == -1) {
            if (CONCAT22(local_c,local_10._2_2_) == -1) {
              if (uStack_a == 0xffff) {
                uStack_a = 0x8000;
                uVar24 = uVar24 + 1;
                uStack_20 = iVar17;
                iVar20 = iVar17;
              }
              else {
                uStack_a = uStack_a + 1;
                uStack_20 = iVar17;
                iVar20 = iVar17;
              }
            }
            else {
              uStack_20 = CONCAT22(local_c,local_10._2_2_) + 1;
              iVar20 = iVar17;
            }
          }
          else {
            iVar20 = CONCAT22((ushort)local_10,uStack_12) + 1;
            uStack_20 = CONCAT22(local_c,local_10._2_2_);
          }
        }
        local_10._0_2_ = (ushort)((uint)iVar20 >> 0x10);
        uStack_12 = (ushort)iVar20;
        local_c = (ushort)((uint)uStack_20 >> 0x10);
        local_10._2_2_ = (ushort)uStack_20;
        param_2 = CONCAT22(local_10._2_2_,(ushort)local_10);
        if (uVar24 < 0x7fff) {
          bStack_19 = (byte)(uVar24 >> 8) | bStack_19 & 0x80;
          uStack_1c = uStack_a;
          local_1a = (undefined1)uVar24;
        }
        else {
          iVar20 = (-(uint)((bStack_19 & 0x80) != 0) & 0x80000000) + 0x7fff8000;
          uStack_1c = (ushort)iVar20;
          local_1a = (undefined1)((uint)iVar20 >> 0x10);
          bStack_19 = (byte)((uint)iVar20 >> 0x18);
          uStack_20 = iVar17;
          iVar20 = iVar17;
        }
        local_24._2_2_ = (ushort)((uint)iVar20 >> 0x10);
        local_24._0_2_ = (undefined2)iVar20;
        param_1 = CONCAT22(uStack_12,local_24._2_2_);
        goto LAB_1001d6cd;
      }
      uStack_1c = 0;
      local_1a = 0;
      bStack_19 = 0;
    }
    else {
      iVar17 = (-(uint)((bStack_19 & 0x80) != 0) & 0x80000000) + 0x7fff8000;
      uStack_1c = (ushort)iVar17;
      local_1a = (undefined1)((uint)iVar17 >> 0x10);
      bStack_19 = (byte)((uint)iVar17 >> 0x18);
    }
    local_24._0_2_ = 0;
    param_1 = 0;
    param_2 = 0;
    uStack_20 = 0;
  }
LAB_1001d6cd:
  uStack_12 = (ushort)((uint)param_1 >> 0x10);
  local_24._2_2_ = (ushort)param_1;
  uVar16 = CONCAT22(local_24._2_2_,(undefined2)local_24);
  *param_6 = local_50;
  if (((param_5 & 1) == 0) || (param_4 = param_4 + local_50, 0 < param_4)) {
    if (0x15 < param_4) {
      param_4 = 0x15;
    }
    iVar20 = (CONCAT13(bStack_19,CONCAT12(local_1a,uStack_1c)) >> 0x10) - 0x3ffe;
    local_1a = 0;
    bStack_19 = 0;
    iVar17 = 8;
    do {
      uVar24 = local_24._2_2_;
      iVar3 = CONCAT22(local_24._2_2_,(undefined2)local_24) << 1;
      local_24._0_2_ = (undefined2)iVar3;
      local_24._2_2_ = (ushort)((uint)iVar3 >> 0x10);
      uVar16 = uStack_20 * 2;
      iVar3 = CONCAT13(bStack_19,CONCAT12(local_1a,uStack_1c)) * 2;
      uStack_1c = (ushort)iVar3 | (ushort)((uint)uStack_20 >> 0x1f);
      iVar17 = iVar17 + -1;
      uStack_20._0_2_ = (ushort)(uVar16 | uVar24 >> 0xf);
      uStack_20._2_2_ = (ushort)(uVar16 >> 0x10);
      local_1a = (undefined1)((uint)iVar3 >> 0x10);
      bStack_19 = (byte)((uint)iVar3 >> 0x18);
    } while (iVar17 != 0);
    if ((iVar20 < 0) && (uVar16 = -iVar20 & 0xff, uVar16 != 0)) {
      do {
        iVar20 = CONCAT13(bStack_19,CONCAT12(local_1a,uStack_1c));
        uVar12 = CONCAT22(uStack_20._2_2_,(ushort)uStack_20);
        iVar17 = CONCAT22(uStack_20._2_2_,(ushort)uStack_20);
        uVar14 = CONCAT13(bStack_19,CONCAT12(local_1a,uStack_1c)) >> 1;
        uStack_1c = (ushort)uVar14;
        local_1a = (undefined1)(uVar14 >> 0x10);
        bStack_19 = bStack_19 >> 1;
        uStack_20._2_2_ = uStack_20._2_2_ >> 1 | (ushort)((uint)(iVar20 << 0x1f) >> 0x10);
        uVar14 = CONCAT22(local_24._2_2_,(undefined2)local_24);
        local_24._2_2_ = local_24._2_2_ >> 1 | (ushort)((uint)(iVar17 << 0x1f) >> 0x10);
        uVar16 = uVar16 - 1;
        uStack_20._0_2_ = (ushort)(uVar12 >> 1);
        local_24._0_2_ = (undefined2)(uVar14 >> 1);
      } while (0 < (int)uVar16);
    }
    uVar16 = CONCAT22(local_24._2_2_,(undefined2)local_24);
    psVar7 = param_6 + 2;
    psVar18 = psVar7;
    uVar24 = uStack_20._2_2_;
    for (iVar17 = param_4 + 1; 0 < iVar17; iVar17 = iVar17 + -1) {
      local_24._2_2_ = (ushort)(uVar16 >> 0x10);
      local_24._0_2_ = (undefined2)uVar16;
      iVar1 = CONCAT22((ushort)uStack_20,local_24._2_2_);
      local_38 = (undefined *)CONCAT13(bStack_19,CONCAT12(local_1a,uStack_1c));
      uVar14 = CONCAT22(uVar24,(ushort)uStack_20) * 2;
      uVar12 = (CONCAT13(bStack_19,CONCAT12(local_1a,uStack_1c)) * 2 | (uint)(uVar24 >> 0xf)) * 2 |
               uVar14 >> 0x1f;
      uVar21 = (uVar14 | local_24._2_2_ >> 0xf) * 2 | (uVar16 << 1) >> 0x1f;
      uVar14 = uVar16 * 5;
      if ((uVar14 < uVar16 * 4) || (uVar22 = uVar21, uVar14 < uVar16)) {
        uVar22 = uVar21 + 1;
        bVar4 = false;
        if ((uVar22 < uVar21) || (uVar22 == 0)) {
          bVar4 = true;
        }
        if (bVar4) {
          uVar12 = uVar12 + 1;
        }
      }
      uVar21 = CONCAT22(uVar24,(ushort)uStack_20) + uVar22;
      if ((uVar21 < uVar22) || (uVar21 < CONCAT22(uVar24,(ushort)uStack_20))) {
        uVar12 = uVar12 + 1;
      }
      iVar20 = (int)(local_38 + uVar12) * 2;
      uStack_1c = (ushort)iVar20 | (ushort)(uVar21 >> 0x1f);
      uVar16 = uVar16 * 10;
      local_1a = (undefined1)((uint)iVar20 >> 0x10);
      uStack_20._0_2_ = (ushort)(uVar21 * 2) | (ushort)(uVar14 >> 0x1f);
      *(char *)psVar18 = (char)((uint)iVar20 >> 0x18) + '0';
      psVar18 = (short *)((int)psVar18 + 1);
      uStack_20._2_2_ = (ushort)(uVar21 * 2 >> 0x10);
      bStack_19 = 0;
      local_40 = (undefined2)local_24;
      uStack_3a = uVar24;
      uVar24 = uStack_20._2_2_;
    }
    uStack_20 = CONCAT22(uVar24,(ushort)uStack_20);
    psVar19 = psVar18 + -1;
    if (*(char *)((int)psVar18 + -1) < '5') {
      for (; (psVar7 <= psVar19 && ((char)*psVar19 == '0')); psVar19 = (short *)((int)psVar19 + -1))
      {
      }
      if (psVar19 < psVar7) {
        *param_6 = 0;
        *(undefined1 *)((int)param_6 + 3) = 1;
        *(byte *)(param_6 + 1) = ((uVar8 != 0x8000) - 1U & 0xd) + 0x20;
        *(char *)psVar7 = '0';
        *(undefined1 *)((int)param_6 + 5) = 0;
        goto LAB_1001d88e;
      }
    }
    else {
      for (; (psVar7 <= psVar19 && ((char)*psVar19 == '9')); psVar19 = (short *)((int)psVar19 + -1))
      {
        *(char *)psVar19 = '0';
      }
      if (psVar19 < psVar7) {
        psVar19 = (short *)((int)psVar19 + 1);
        *param_6 = *param_6 + 1;
      }
      *(char *)psVar19 = (char)*psVar19 + '\x01';
    }
    cVar15 = ((char)psVar19 - (char)param_6) + -3;
    *(char *)((int)param_6 + 3) = cVar15;
    *(undefined1 *)(cVar15 + 4 + (int)param_6) = 0;
  }
  else {
    *param_6 = 0;
    *(undefined1 *)((int)param_6 + 3) = 1;
    *(byte *)(param_6 + 1) = ((uVar8 != 0x8000) - 1U & 0xd) + 0x20;
    *(undefined1 *)(param_6 + 2) = 0x30;
    *(undefined1 *)((int)param_6 + 5) = 0;
  }
LAB_1001d88e:
  local_24 = uVar16;
  local_10 = param_2;
  uStack_3e = iVar1;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001d8d6 __hw_cw

/* Library Function - Single Match
    __hw_cw
   
   Library: Visual Studio 2005 Release */

uint __hw_cw(void)

{
  uint uVar1;
  uint uVar2;
  uint unaff_EBX;
  
  uVar1 = (uint)((unaff_EBX & 0x10) != 0);
  if ((unaff_EBX & 8) != 0) {
    uVar1 = uVar1 | 4;
  }
  if ((unaff_EBX & 4) != 0) {
    uVar1 = uVar1 | 8;
  }
  if ((unaff_EBX & 2) != 0) {
    uVar1 = uVar1 | 0x10;
  }
  if ((unaff_EBX & 1) != 0) {
    uVar1 = uVar1 | 0x20;
  }
  if ((unaff_EBX & 0x80000) != 0) {
    uVar1 = uVar1 | 2;
  }
  uVar2 = unaff_EBX & 0x300;
  if (uVar2 != 0) {
    if (uVar2 == 0x100) {
      uVar1 = uVar1 | 0x400;
    }
    else if (uVar2 == 0x200) {
      uVar1 = uVar1 | 0x800;
    }
    else if (uVar2 == 0x300) {
      uVar1 = uVar1 | 0xc00;
    }
  }
  if ((unaff_EBX & 0x30000) == 0) {
    uVar1 = uVar1 | 0x300;
  }
  else if ((unaff_EBX & 0x30000) == 0x10000) {
    uVar1 = uVar1 | 0x200;
  }
  if ((unaff_EBX & 0x40000) != 0) {
    uVar1 = uVar1 | 0x1000;
  }
  return uVar1;
}


// 1001d964 ___hw_cw_sse2

/* Library Function - Single Match
    ___hw_cw_sse2
   
   Library: Visual Studio 2005 Release */

uint __fastcall ___hw_cw_sse2(undefined4 param_1,uint param_2)

{
  uint uVar1;
  uint uVar2;
  
  uVar1 = 0;
  if ((param_2 & 0x10) != 0) {
    uVar1 = 0x80;
  }
  if ((param_2 & 8) != 0) {
    uVar1 = uVar1 | 0x200;
  }
  if ((param_2 & 4) != 0) {
    uVar1 = uVar1 | 0x400;
  }
  if ((param_2 & 2) != 0) {
    uVar1 = uVar1 | 0x800;
  }
  if ((param_2 & 1) != 0) {
    uVar1 = uVar1 | 0x1000;
  }
  if ((param_2 & 0x80000) != 0) {
    uVar1 = uVar1 | 0x100;
  }
  uVar2 = param_2 & 0x300;
  if (uVar2 != 0) {
    if (uVar2 == 0x100) {
      uVar1 = uVar1 | 0x2000;
    }
    else if (uVar2 == 0x200) {
      uVar1 = uVar1 | 0x4000;
    }
    else if (uVar2 == 0x300) {
      uVar1 = uVar1 | 0x6000;
    }
  }
  uVar2 = param_2 & 0x3000000;
  if (uVar2 == 0x1000000) {
    uVar1 = uVar1 | 0x8040;
  }
  else {
    if (uVar2 == 0x2000000) {
      return uVar1 | 0x40;
    }
    if (uVar2 == 0x3000000) {
      return uVar1 | 0x8000;
    }
  }
  return uVar1;
}


// 1001da04 __control87

/* Library Function - Single Match
    __control87
   
   Library: Visual Studio 2005 Release */

uint __cdecl __control87(uint _NewValue,uint _Mask)

{
  ushort uVar1;
  uint uVar2;
  uint uVar3;
  uint uVar4;
  uint uVar5;
  ushort in_FPUControlWord;
  
  uVar5 = 0;
  if ((in_FPUControlWord & 1) != 0) {
    uVar5 = 0x10;
  }
  if ((in_FPUControlWord & 4) != 0) {
    uVar5 = uVar5 | 8;
  }
  if ((in_FPUControlWord & 8) != 0) {
    uVar5 = uVar5 | 4;
  }
  if ((in_FPUControlWord & 0x10) != 0) {
    uVar5 = uVar5 | 2;
  }
  if ((in_FPUControlWord & 0x20) != 0) {
    uVar5 = uVar5 | 1;
  }
  if ((in_FPUControlWord & 2) != 0) {
    uVar5 = uVar5 | 0x80000;
  }
  uVar1 = in_FPUControlWord & 0xc00;
  if ((in_FPUControlWord & 0xc00) != 0) {
    if (uVar1 == 0x400) {
      uVar5 = uVar5 | 0x100;
    }
    else if (uVar1 == 0x800) {
      uVar5 = uVar5 | 0x200;
    }
    else if (uVar1 == 0xc00) {
      uVar5 = uVar5 | 0x300;
    }
  }
  if ((in_FPUControlWord & 0x300) == 0) {
    uVar5 = uVar5 | 0x20000;
  }
  else if ((in_FPUControlWord & 0x300) == 0x200) {
    uVar5 = uVar5 | 0x10000;
  }
  if ((in_FPUControlWord & 0x1000) != 0) {
    uVar5 = uVar5 | 0x40000;
  }
  uVar2 = ~_Mask & uVar5 | _NewValue & _Mask;
  if (uVar2 != uVar5) {
    uVar5 = __hw_cw();
    uVar2 = 0;
    if ((uVar5 & 1) != 0) {
      uVar2 = 0x10;
    }
    if ((uVar5 & 4) != 0) {
      uVar2 = uVar2 | 8;
    }
    if ((uVar5 & 8) != 0) {
      uVar2 = uVar2 | 4;
    }
    if ((uVar5 & 0x10) != 0) {
      uVar2 = uVar2 | 2;
    }
    if ((uVar5 & 0x20) != 0) {
      uVar2 = uVar2 | 1;
    }
    if ((uVar5 & 2) != 0) {
      uVar2 = uVar2 | 0x80000;
    }
    uVar3 = uVar5 & 0xc00;
    if (uVar3 != 0) {
      if (uVar3 == 0x400) {
        uVar2 = uVar2 | 0x100;
      }
      else if (uVar3 == 0x800) {
        uVar2 = uVar2 | 0x200;
      }
      else if (uVar3 == 0xc00) {
        uVar2 = uVar2 | 0x300;
      }
    }
    if ((uVar5 & 0x300) == 0) {
      uVar2 = uVar2 | 0x20000;
    }
    else if ((uVar5 & 0x300) == 0x200) {
      uVar2 = uVar2 | 0x10000;
    }
    if ((uVar5 & 0x1000) != 0) {
      uVar2 = uVar2 | 0x40000;
    }
  }
  if (DAT_1004bcfc != 0) {
    uVar5 = 0;
    if ((char)MXCSR < '\0') {
      uVar5 = 0x10;
    }
    if ((MXCSR & 0x200) != 0) {
      uVar5 = uVar5 | 8;
    }
    if ((MXCSR & 0x400) != 0) {
      uVar5 = uVar5 | 4;
    }
    if ((MXCSR & 0x800) != 0) {
      uVar5 = uVar5 | 2;
    }
    if ((MXCSR & 0x1000) != 0) {
      uVar5 = uVar5 | 1;
    }
    if ((MXCSR & 0x100) != 0) {
      uVar5 = uVar5 | 0x80000;
    }
    uVar3 = MXCSR & 0x6000;
    if (uVar3 != 0) {
      if (uVar3 == 0x2000) {
        uVar5 = uVar5 | 0x100;
      }
      else if (uVar3 == 0x4000) {
        uVar5 = uVar5 | 0x200;
      }
      else if (uVar3 == 0x6000) {
        uVar5 = uVar5 | 0x300;
      }
    }
    uVar4 = MXCSR & 0x8040;
    if (uVar4 == 0x40) {
      uVar5 = uVar5 | 0x2000000;
    }
    else if (uVar4 == 0x8000) {
      uVar5 = uVar5 | 0x3000000;
    }
    else if (uVar4 == 0x8040) {
      uVar5 = uVar5 | 0x1000000;
    }
    uVar4 = ~(_Mask & 0x308031f) & uVar5 | _Mask & 0x308031f & _NewValue;
    if (uVar4 != uVar5) {
      uVar5 = ___hw_cw_sse2(uVar3,uVar4);
      ___set_fpsr_sse2(uVar5);
      uVar5 = 0;
      if ((char)MXCSR < '\0') {
        uVar5 = 0x10;
      }
      if ((MXCSR & 0x200) != 0) {
        uVar5 = uVar5 | 8;
      }
      if ((MXCSR & 0x400) != 0) {
        uVar5 = uVar5 | 4;
      }
      if ((MXCSR & 0x800) != 0) {
        uVar5 = uVar5 | 2;
      }
      if ((MXCSR & 0x1000) != 0) {
        uVar5 = uVar5 | 1;
      }
      if ((MXCSR & 0x100) != 0) {
        uVar5 = uVar5 | 0x80000;
      }
      uVar3 = MXCSR & 0x6000;
      if (uVar3 != 0) {
        if (uVar3 == 0x2000) {
          uVar5 = uVar5 | 0x100;
        }
        else if (uVar3 == 0x4000) {
          uVar5 = uVar5 | 0x200;
        }
        else if (uVar3 == 0x6000) {
          uVar5 = uVar5 | 0x300;
        }
      }
      uVar3 = MXCSR & 0x8040;
      if (uVar3 == 0x40) {
        uVar5 = uVar5 | 0x2000000;
      }
      else if (uVar3 == 0x8000) {
        uVar5 = uVar5 | 0x3000000;
      }
      else if (uVar3 == 0x8040) {
        uVar5 = uVar5 | 0x1000000;
      }
    }
    uVar3 = uVar5 ^ uVar2;
    uVar2 = uVar5 | uVar2;
    if ((uVar3 & 0x8031f) != 0) {
      uVar2 = uVar2 | 0x80000000;
    }
  }
  return uVar2;
}


// 1001dd08 __global_unwind2

/* Library Function - Single Match
    __global_unwind2
   
   Library: Visual Studio */

void __cdecl __global_unwind2(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x1001dd20,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 1001dd6d __local_unwind2

/* Library Function - Single Match
    __local_unwind2
   
   Libraries: Visual Studio 2017 Debug, Visual Studio 2017 Release, Visual Studio 2019 Debug, Visual
   Studio 2019 Release */

void __cdecl __local_unwind2(int param_1,uint param_2)

{
  uint uVar1;
  undefined4 *unaff_FS_OFFSET;
  undefined4 local_20;
  undefined1 *puStack_1c;
  undefined4 local_18;
  int iStack_14;
  
  iStack_14 = param_1;
  puStack_1c = &LAB_1001dd28;
  local_20 = *unaff_FS_OFFSET;
  *unaff_FS_OFFSET = &local_20;
  while( true ) {
    uVar1 = *(uint *)(param_1 + 0xc);
    if ((uVar1 == 0xffffffff) || ((param_2 != 0xffffffff && (uVar1 <= param_2)))) break;
    local_18 = *(undefined4 *)(*(int *)(param_1 + 8) + uVar1 * 0xc);
    *(undefined4 *)(param_1 + 0xc) = local_18;
    if (*(int *)(*(int *)(param_1 + 8) + 4 + uVar1 * 0xc) == 0) {
      __NLG_Notify(0x101);
      FUN_1001de3c();
    }
  }
  *unaff_FS_OFFSET = local_20;
  return;
}


// 1001de1d __NLG_Notify

/* Library Function - Single Match
    __NLG_Notify
   
   Libraries: Visual Studio 2017 Debug, Visual Studio 2017 Release, Visual Studio 2019 Debug, Visual
   Studio 2019 Release */

void __NLG_Notify(ulong param_1)

{
  undefined4 in_EAX;
  undefined4 unaff_EBP;
  
  DAT_10036e78 = param_1;
  DAT_10036e74 = in_EAX;
  DAT_10036e7c = unaff_EBP;
  return;
}


// 1001de3c FUN_1001de3c

void FUN_1001de3c(void)

{
  code *in_EAX;
  
  (*in_EAX)();
  return;
}


// 1001de3f __flswbuf

/* Library Function - Single Match
    __flswbuf
   
   Library: Visual Studio 2005 Release */

int __cdecl __flswbuf(int _Ch,FILE *_File)

{
  uint uVar1;
  char *_Buf;
  char *pcVar2;
  uint _FileHandle;
  DWORD *pDVar3;
  undefined **ppuVar4;
  int iVar5;
  undefined *puVar6;
  int unaff_EDI;
  uint _MaxCharCount;
  longlong lVar7;
  uint local_8;
  
  _FileHandle = __fileno(_File);
  uVar1 = _File->_flag;
  if ((uVar1 & 0x82) == 0) {
    pDVar3 = FUN_10011e63();
    *pDVar3 = 9;
LAB_1001de63:
    _File->_flag = _File->_flag | 0x20;
    return 0xffff;
  }
  if ((uVar1 & 0x40) != 0) {
    pDVar3 = FUN_10011e63();
    *pDVar3 = 0x22;
    goto LAB_1001de63;
  }
  if ((uVar1 & 1) != 0) {
    _File->_cnt = 0;
    if ((uVar1 & 0x10) == 0) {
      _File->_flag = uVar1 | 0x20;
      return 0xffff;
    }
    _File->_ptr = _File->_base;
    _File->_flag = uVar1 & 0xfffffffe;
  }
  uVar1 = _File->_flag;
  _File->_cnt = 0;
  local_8 = 0;
  _MaxCharCount = 2;
  _File->_flag = uVar1 & 0xffffffef | 2;
  if (((uVar1 & 0x10c) == 0) &&
     (((ppuVar4 = FUN_10019493(), _File != (FILE *)(ppuVar4 + 8) &&
       (ppuVar4 = FUN_10019493(), _File != (FILE *)(ppuVar4 + 0x10))) ||
      (iVar5 = __isatty(_FileHandle), iVar5 == 0)))) {
    __getbuf(_File);
  }
  if ((_File->_flag & 0x108) == 0) {
    local_8 = CONCAT22(local_8._2_2_,(short)_Ch);
    local_8 = __write(_FileHandle,&local_8,2);
  }
  else {
    _Buf = _File->_base;
    pcVar2 = _File->_ptr;
    _File->_ptr = _Buf + 2;
    _MaxCharCount = (int)pcVar2 - (int)_Buf;
    _File->_cnt = _File->_bufsiz + -2;
    if ((int)_MaxCharCount < 1) {
      if ((_FileHandle == 0xffffffff) || (_FileHandle == 0xfffffffe)) {
        puVar6 = &DAT_100365e8;
      }
      else {
        puVar6 = (undefined *)((_FileHandle & 0x1f) * 0x38 + (&DAT_1004bbe0)[(int)_FileHandle >> 5])
        ;
      }
      if (((puVar6[4] & 0x20) != 0) &&
         (lVar7 = __lseeki64(_FileHandle,0x200000000,unaff_EDI), lVar7 == -1)) goto LAB_1001df98;
    }
    else {
      local_8 = __write(_FileHandle,_Buf,_MaxCharCount);
    }
    *(short *)_File->_base = (short)_Ch;
  }
  if (local_8 == _MaxCharCount) {
    return _Ch & 0xffff;
  }
LAB_1001df98:
  _File->_flag = _File->_flag | 0x20;
  return 0xffff;
}


// 1001dfaf __filwbuf

/* Library Function - Single Match
    __filwbuf
   
   Library: Visual Studio 2005 Release */

int __cdecl __filwbuf(FILE *_File)

{
  ushort uVar1;
  DWORD *pDVar2;
  uint uVar3;
  int iVar4;
  undefined *puVar5;
  LPWSTR pWVar6;
  LPWSTR pWVar7;
  
  if (_File == (FILE *)0x0) {
    pDVar2 = FUN_10011e63();
    *pDVar2 = 0x16;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    uVar3 = _File->_flag;
    if (((uVar3 & 0x83) != 0) && ((uVar3 & 0x40) == 0)) {
      if ((uVar3 & 2) == 0) {
        _File->_flag = uVar3 | 1;
        if ((uVar3 & 0x10c) == 0) {
          __getbuf(_File);
        }
        else {
          _File->_ptr = _File->_base;
        }
        pWVar7 = (LPWSTR)_File->_bufsiz;
        pWVar6 = (LPWSTR)_File->_base;
        uVar3 = __fileno(_File);
        iVar4 = FUN_1001e6a2(uVar3,pWVar6,pWVar7);
        _File->_cnt = iVar4;
        if (((iVar4 != 0) && (iVar4 != 1)) && (iVar4 != -1)) {
          if ((_File->_flag & 0x82) == 0) {
            iVar4 = __fileno(_File);
            if ((iVar4 == -1) || (iVar4 = __fileno(_File), iVar4 == -2)) {
              puVar5 = &DAT_100365e8;
            }
            else {
              iVar4 = __fileno(_File);
              uVar3 = __fileno(_File);
              puVar5 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_1004bbe0)[iVar4 >> 5]);
            }
            if ((puVar5[4] & 0x82) == 0x82) {
              _File->_flag = _File->_flag | 0x2000;
            }
          }
          if (((_File->_bufsiz == 0x200) && ((_File->_flag & 8U) != 0)) &&
             ((_File->_flag & 0x400U) == 0)) {
            _File->_bufsiz = 0x1000;
          }
          _File->_cnt = _File->_cnt + -2;
          uVar1 = *(ushort *)_File->_ptr;
          _File->_ptr = (char *)((int)_File->_ptr + 2);
          return (uint)uVar1;
        }
        _File->_flag = _File->_flag | (-(uint)(iVar4 != 0) & 0x10) + 0x10;
        _File->_cnt = 0;
      }
      else {
        _File->_flag = uVar3 | 0x20;
      }
    }
  }
  return 0xffff;
}


// 1001e0e1 FUN_1001e0e1

int __cdecl FUN_1001e0e1(uint param_1,LPWSTR param_2,LPWSTR param_3)

{
  int *piVar1;
  byte *pbVar2;
  byte bVar3;
  WCHAR WVar4;
  DWORD *pDVar5;
  LPWSTR pWVar6;
  BOOL BVar7;
  DWORD DVar8;
  LPWSTR pWVar9;
  int iVar10;
  LPWSTR pWVar11;
  int iVar12;
  int unaff_EDI;
  bool bVar13;
  longlong lVar14;
  LPWSTR local_1c;
  int local_18;
  LPWSTR local_14;
  LPWSTR local_10;
  undefined2 local_c;
  char local_6;
  char local_5;
  
  pWVar11 = param_3;
  local_18 = -2;
  if (param_1 == 0xfffffffe) {
    pDVar5 = FUN_10011e76();
    *pDVar5 = 0;
    pDVar5 = FUN_10011e63();
    *pDVar5 = 9;
    return -1;
  }
  if (((int)param_1 < 0) || (DAT_1004bbc4 <= param_1)) {
    pDVar5 = FUN_10011e76();
    *pDVar5 = 0;
    pDVar5 = FUN_10011e63();
    *pDVar5 = 9;
    FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  iVar12 = (param_1 & 0x1f) * 0x38;
  piVar1 = &DAT_1004bbe0 + ((int)param_1 >> 5);
  bVar3 = *(byte *)(*piVar1 + iVar12 + 4);
  if ((bVar3 & 1) == 0) {
    pDVar5 = FUN_10011e76();
    *pDVar5 = 0;
    pDVar5 = FUN_10011e63();
    *pDVar5 = 9;
    goto LAB_1001e1e9;
  }
  if (param_3 < (LPWSTR)0x80000000) {
    local_14 = (LPWSTR)0x0;
    if ((param_3 == (LPWSTR)0x0) || ((bVar3 & 2) != 0)) {
      return 0;
    }
    if (param_2 != (LPWSTR)0x0) {
      local_6 = (char)(*(char *)(*piVar1 + iVar12 + 0x24) * '\x02') >> 1;
      if (local_6 == '\x01') {
        if ((~(uint)param_3 & 1) == 0) goto LAB_1001e1d7;
        pWVar9 = (LPWSTR)((uint)param_3 >> 1);
        param_3 = (LPWSTR)0x4;
        if ((LPWSTR)0x3 < pWVar9) {
          param_3 = pWVar9;
        }
        local_10 = (LPWSTR)FUN_10001270(param_3);
        if (local_10 == (LPWSTR)0x0) {
          pDVar5 = FUN_10011e63();
          *pDVar5 = 0xc;
          pDVar5 = FUN_10011e76();
          *pDVar5 = 8;
          return -1;
        }
        lVar14 = __lseeki64_nolock(param_1,0x100000000,unaff_EDI);
        iVar10 = *piVar1;
        *(int *)(iVar12 + 0x28 + iVar10) = (int)lVar14;
        *(int *)(iVar12 + 0x2c + iVar10) = (int)((ulonglong)lVar14 >> 0x20);
      }
      else {
        if (local_6 == '\x02') {
          if ((~(uint)param_3 & 1) == 0) goto LAB_1001e1d7;
          param_3 = (LPWSTR)((uint)param_3 & 0xfffffffe);
        }
        local_10 = param_2;
      }
      pWVar6 = local_10;
      pWVar9 = param_3;
      if ((((*(byte *)(*piVar1 + iVar12 + 4) & 0x48) != 0) &&
          (bVar3 = *(byte *)(*piVar1 + iVar12 + 5), bVar3 != 10)) && (param_3 != (LPWSTR)0x0)) {
        *(byte *)local_10 = bVar3;
        pWVar6 = (LPWSTR)((int)local_10 + 1);
        pWVar9 = (LPWSTR)((int)param_3 + -1);
        local_14 = (LPWSTR)0x1;
        *(undefined1 *)(iVar12 + 5 + *piVar1) = 10;
        if (((local_6 != '\0') && (bVar3 = *(byte *)(iVar12 + 0x25 + *piVar1), bVar3 != 10)) &&
           (pWVar9 != (LPWSTR)0x0)) {
          *(byte *)pWVar6 = bVar3;
          pWVar6 = local_10 + 1;
          pWVar9 = param_3 + -1;
          local_14 = (LPWSTR)0x2;
          *(undefined1 *)(iVar12 + 0x25 + *piVar1) = 10;
          if (((local_6 == '\x01') && (bVar3 = *(byte *)(iVar12 + 0x26 + *piVar1), bVar3 != 10)) &&
             (pWVar9 != (LPWSTR)0x0)) {
            *(byte *)pWVar6 = bVar3;
            pWVar6 = (LPWSTR)((int)local_10 + 3);
            local_14 = (LPWSTR)0x3;
            *(undefined1 *)(iVar12 + 0x26 + *piVar1) = 10;
            pWVar9 = (LPWSTR)((int)param_3 + -3);
          }
        }
      }
      param_3 = pWVar9;
      BVar7 = ReadFile(*(HANDLE *)(iVar12 + *piVar1),pWVar6,(DWORD)param_3,(LPDWORD)&local_1c,
                       (LPOVERLAPPED)0x0);
      if (((BVar7 == 0) || ((int)local_1c < 0)) || (param_3 < local_1c)) {
        DVar8 = GetLastError();
        if (DVar8 != 5) {
          if (DVar8 == 0x6d) {
            local_18 = 0;
            goto LAB_1001e4f6;
          }
          goto LAB_1001e4eb;
        }
        pDVar5 = FUN_10011e63();
        *pDVar5 = 9;
        pDVar5 = FUN_10011e76();
        *pDVar5 = 5;
      }
      else {
        local_14 = (LPWSTR)((int)local_14 + (int)local_1c);
        pbVar2 = (byte *)(iVar12 + 4 + *piVar1);
        if ((*pbVar2 & 0x80) == 0) goto LAB_1001e4f6;
        if (local_6 == '\x02') {
          if ((local_1c == (LPWSTR)0x0) || (*local_10 != L'\n')) {
            *pbVar2 = *pbVar2 & 0xfb;
          }
          else {
            *pbVar2 = *pbVar2 | 4;
          }
          local_14 = (LPWSTR)((int)local_14 + (int)local_10);
          param_3 = local_10;
          pWVar11 = local_10;
          if (local_10 < local_14) {
            do {
              WVar4 = *param_3;
              if (WVar4 == L'\x1a') {
                pbVar2 = (byte *)(iVar12 + 4 + *piVar1);
                if ((*pbVar2 & 0x40) == 0) {
                  *pbVar2 = *pbVar2 | 2;
                }
                else {
                  *pWVar11 = *param_3;
                  pWVar11 = pWVar11 + 1;
                }
                break;
              }
              if (WVar4 == L'\r') {
                if (param_3 < local_14 + -1) {
                  if (param_3[1] == L'\n') {
                    pWVar9 = param_3 + 2;
                    goto LAB_1001e599;
                  }
LAB_1001e62e:
                  param_3 = param_3 + 1;
                  *pWVar11 = L'\r';
                }
                else {
                  pWVar9 = param_3 + 1;
                  BVar7 = ReadFile(*(HANDLE *)(iVar12 + *piVar1),&local_c,2,(LPDWORD)&local_1c,
                                   (LPOVERLAPPED)0x0);
                  if (((BVar7 == 0) && (DVar8 = GetLastError(), DVar8 != 0)) ||
                     (local_1c == (LPWSTR)0x0)) goto LAB_1001e62e;
                  if ((*(byte *)(iVar12 + 4 + *piVar1) & 0x48) == 0) {
                    if ((pWVar11 == local_10) && (local_c == 10)) goto LAB_1001e599;
                    __lseeki64_nolock(param_1,0x1ffffffff,unaff_EDI);
                    if (local_c == 10) goto LAB_1001e635;
                    goto LAB_1001e62e;
                  }
                  if (local_c == 10) {
LAB_1001e599:
                    param_3 = pWVar9;
                    *pWVar11 = L'\n';
                  }
                  else {
                    *pWVar11 = L'\r';
                    *(undefined1 *)(iVar12 + 5 + *piVar1) = (undefined1)local_c;
                    *(undefined1 *)(iVar12 + 0x25 + *piVar1) = local_c._1_1_;
                    *(undefined1 *)(iVar12 + 0x26 + *piVar1) = 10;
                    param_3 = pWVar9;
                  }
                }
                pWVar11 = pWVar11 + 1;
                pWVar9 = param_3;
              }
              else {
                *pWVar11 = WVar4;
                pWVar11 = pWVar11 + 1;
                pWVar9 = param_3 + 1;
              }
LAB_1001e635:
              param_3 = pWVar9;
            } while (param_3 < local_14);
          }
          local_14 = (LPWSTR)((int)pWVar11 - (int)local_10);
          goto LAB_1001e4f6;
        }
        if ((local_1c == (LPWSTR)0x0) || ((byte)*local_10 != 10)) {
          *pbVar2 = *pbVar2 & 0xfb;
        }
        else {
          *pbVar2 = *pbVar2 | 4;
        }
        local_14 = (LPWSTR)((int)local_14 + (int)local_10);
        param_3 = local_10;
        pWVar9 = local_10;
        if (local_10 < local_14) {
          do {
            bVar3 = (byte)*param_3;
            if (bVar3 == 0x1a) {
              pbVar2 = (byte *)(iVar12 + 4 + *piVar1);
              if ((*pbVar2 & 0x40) == 0) {
                *pbVar2 = *pbVar2 | 2;
              }
              else {
                *(byte *)pWVar9 = (byte)*param_3;
                pWVar9 = (LPWSTR)((int)pWVar9 + 1);
              }
              break;
            }
            if (bVar3 == 0xd) {
              if (param_3 < (LPWSTR)((int)local_14 + -1)) {
                if (*(char *)((int)param_3 + 1) == '\n') {
                  pWVar6 = param_3 + 1;
                  goto LAB_1001e376;
                }
LAB_1001e3ed:
                param_3 = (LPWSTR)((int)param_3 + 1);
                *(byte *)pWVar9 = 0xd;
              }
              else {
                pWVar6 = (LPWSTR)((int)param_3 + 1);
                BVar7 = ReadFile(*(HANDLE *)(iVar12 + *piVar1),&local_5,1,(LPDWORD)&local_1c,
                                 (LPOVERLAPPED)0x0);
                if (((BVar7 == 0) && (DVar8 = GetLastError(), DVar8 != 0)) ||
                   (local_1c == (LPWSTR)0x0)) goto LAB_1001e3ed;
                if ((*(byte *)(iVar12 + 4 + *piVar1) & 0x48) == 0) {
                  if ((pWVar9 == local_10) && (local_5 == '\n')) goto LAB_1001e376;
                  __lseeki64_nolock(param_1,0x1ffffffff,unaff_EDI);
                  if (local_5 == '\n') goto LAB_1001e3f1;
                  goto LAB_1001e3ed;
                }
                if (local_5 == '\n') {
LAB_1001e376:
                  param_3 = pWVar6;
                  *(byte *)pWVar9 = 10;
                }
                else {
                  *(byte *)pWVar9 = 0xd;
                  *(char *)(iVar12 + 5 + *piVar1) = local_5;
                  param_3 = pWVar6;
                }
              }
              pWVar9 = (LPWSTR)((int)pWVar9 + 1);
              pWVar6 = param_3;
            }
            else {
              *(byte *)pWVar9 = bVar3;
              pWVar9 = (LPWSTR)((int)pWVar9 + 1);
              pWVar6 = (LPWSTR)((int)param_3 + 1);
            }
LAB_1001e3f1:
            param_3 = pWVar6;
          } while (param_3 < local_14);
        }
        local_14 = (LPWSTR)((int)pWVar9 - (int)local_10);
        if ((local_6 != '\x01') || (local_14 == (LPWSTR)0x0)) goto LAB_1001e4f6;
        bVar3 = *(byte *)((int)pWVar9 + -1);
        if ((char)bVar3 < '\0') {
          iVar10 = 1;
          pWVar9 = (LPWSTR)((int)pWVar9 + -1);
          while ((((&DAT_10036f30)[bVar3] == '\0' && (iVar10 < 5)) && (local_10 <= pWVar9))) {
            pWVar9 = (LPWSTR)((int)pWVar9 + -1);
            bVar3 = *(byte *)pWVar9;
            iVar10 = iVar10 + 1;
          }
          if ((char)(&DAT_10036f30)[(byte)*pWVar9] == 0) {
            pDVar5 = FUN_10011e63();
            *pDVar5 = 0x2a;
            goto LAB_1001e4f2;
          }
          if ((char)(&DAT_10036f30)[(byte)*pWVar9] + 1 == iVar10) {
            pWVar9 = (LPWSTR)((int)pWVar9 + iVar10);
          }
          else if ((*(byte *)(*piVar1 + iVar12 + 4) & 0x48) == 0) {
            __lseeki64_nolock(param_1,CONCAT44(1,-iVar10 >> 0x1f),unaff_EDI);
          }
          else {
            pWVar6 = (LPWSTR)((int)pWVar9 + 1);
            *(byte *)(*piVar1 + iVar12 + 5) = (byte)*pWVar9;
            if (1 < iVar10) {
              *(byte *)(iVar12 + 0x25 + *piVar1) = *(byte *)pWVar6;
              pWVar6 = pWVar9 + 1;
            }
            if (iVar10 == 3) {
              *(byte *)(iVar12 + 0x26 + *piVar1) = (byte)*pWVar6;
              pWVar6 = (LPWSTR)((int)pWVar6 + 1);
            }
            pWVar9 = (LPWSTR)((int)pWVar6 - iVar10);
          }
        }
        iVar10 = (int)pWVar9 - (int)local_10;
        local_14 = (LPWSTR)MultiByteToWideChar(0xfde9,0,(LPCSTR)local_10,iVar10,param_2,
                                               (uint)pWVar11 >> 1);
        if (local_14 != (LPWSTR)0x0) {
          bVar13 = local_14 != (LPWSTR)iVar10;
          local_14 = (LPWSTR)((int)local_14 * 2);
          *(uint *)(iVar12 + 0x30 + *piVar1) = (uint)bVar13;
          goto LAB_1001e4f6;
        }
        DVar8 = GetLastError();
LAB_1001e4eb:
        FUN_10011e89(DVar8);
      }
LAB_1001e4f2:
      local_18 = -1;
LAB_1001e4f6:
      if (local_10 != param_2) {
        FUN_100012d0(local_10);
      }
      if (local_18 == -2) {
        return (int)local_14;
      }
      return local_18;
    }
  }
LAB_1001e1d7:
  pDVar5 = FUN_10011e76();
  *pDVar5 = 0;
  pDVar5 = FUN_10011e63();
  *pDVar5 = 0x16;
LAB_1001e1e9:
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1001e6a2 FUN_1001e6a2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

int __cdecl FUN_1001e6a2(uint param_1,LPWSTR param_2,LPWSTR param_3)

{
  DWORD *pDVar1;
  int iVar2;
  int local_20;
  
  if (param_1 == 0xfffffffe) {
    pDVar1 = FUN_10011e76();
    *pDVar1 = 0;
    pDVar1 = FUN_10011e63();
    *pDVar1 = 9;
    return -1;
  }
  if ((-1 < (int)param_1) && (param_1 < DAT_1004bbc4)) {
    iVar2 = (param_1 & 0x1f) * 0x38;
    if ((*(byte *)((&DAT_1004bbe0)[(int)param_1 >> 5] + 4 + iVar2) & 1) != 0) {
      if (param_3 < (LPWSTR)0x80000000) {
        ___lock_fhandle(param_1);
        if ((*(byte *)((&DAT_1004bbe0)[(int)param_1 >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_10011e63();
          *pDVar1 = 9;
          pDVar1 = FUN_10011e76();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = FUN_1001e0e1(param_1,param_2,param_3);
        }
        FUN_1001e795();
        return local_20;
      }
      pDVar1 = FUN_10011e76();
      *pDVar1 = 0;
      pDVar1 = FUN_10011e63();
      *pDVar1 = 0x16;
      goto LAB_1001e6f1;
    }
  }
  pDVar1 = FUN_10011e76();
  *pDVar1 = 0;
  pDVar1 = FUN_10011e63();
  *pDVar1 = 9;
LAB_1001e6f1:
  FUN_10011e04((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1001e795 FUN_1001e795

void FUN_1001e795(void)

{
  int unaff_EBP;
  
  FUN_1001cd06(*(uint *)(unaff_EBP + 8));
  return;
}


// 1001e79f ___initconout

/* Library Function - Single Match
    ___initconout
   
   Library: Visual Studio 2005 Release */

void __cdecl ___initconout(void)

{
  DAT_10037034 = CreateFileA("CONOUT$",0x40000000,3,(LPSECURITY_ATTRIBUTES)0x0,3,0,(HANDLE)0x0);
  return;
}


// 1001e9ba __statfp

/* Library Function - Single Match
    __statfp
   
   Library: Visual Studio 2005 Release */

int __statfp(void)

{
  short in_FPUStatusWord;
  
  return (int)in_FPUStatusWord;
}


// 1001e9c5 __clrfp

/* Library Function - Single Match
    __clrfp
   
   Library: Visual Studio 2005 Release */

int __clrfp(void)

{
  short in_FPUStatusWord;
  
  return (int)in_FPUStatusWord;
}


// 1001e9d1 __ctrlfp

/* Library Function - Single Match
    __ctrlfp
   
   Library: Visual Studio 2005 Release */

int __ctrlfp(void)

{
  short in_FPUControlWord;
  
  return (int)in_FPUControlWord;
}


// 1001e9f8 FUN_1001e9f8

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_1001e9f8(void)

{
  return;
}


// 1001ea4e ___set_fpsr_sse2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___set_fpsr_sse2
   
   Library: Visual Studio 2005 Release */

void __cdecl ___set_fpsr_sse2(uint param_1)

{
  if (DAT_1004bcfc != 0) {
    if (((param_1 & 0x40) == 0) || (DAT_10037050 == 0)) {
      MXCSR = param_1 & 0xffffffbf;
    }
    else {
      MXCSR = param_1;
    }
  }
  return;
}


// 1001eac0 ___ascii_strnicmp

/* Library Function - Single Match
    ___ascii_strnicmp
   
   Library: Visual Studio */

int __cdecl ___ascii_strnicmp(char *_Str1,char *_Str2,size_t _MaxCount)

{
  char cVar1;
  byte bVar2;
  ushort uVar3;
  uint uVar4;
  bool bVar5;
  
  if (_MaxCount != 0) {
    do {
      bVar2 = *_Str1;
      cVar1 = *_Str2;
      uVar3 = CONCAT11(bVar2,cVar1);
      if (bVar2 == 0) break;
      uVar3 = CONCAT11(bVar2,cVar1);
      uVar4 = (uint)uVar3;
      if (cVar1 == '\0') break;
      _Str1 = (char *)((byte *)_Str1 + 1);
      _Str2 = _Str2 + 1;
      if ((0x40 < bVar2) && (bVar2 < 0x5b)) {
        uVar4 = (uint)CONCAT11(bVar2 + 0x20,cVar1);
      }
      uVar3 = (ushort)uVar4;
      bVar2 = (byte)uVar4;
      if ((0x40 < bVar2) && (bVar2 < 0x5b)) {
        uVar3 = (ushort)CONCAT31((int3)(uVar4 >> 8),bVar2 + 0x20);
      }
      bVar2 = (byte)(uVar3 >> 8);
      bVar5 = bVar2 < (byte)uVar3;
      if (bVar2 != (byte)uVar3) goto LAB_1001eb11;
      _MaxCount = _MaxCount - 1;
    } while (_MaxCount != 0);
    _MaxCount = 0;
    bVar2 = (byte)(uVar3 >> 8);
    bVar5 = bVar2 < (byte)uVar3;
    if (bVar2 != (byte)uVar3) {
LAB_1001eb11:
      _MaxCount = 0xffffffff;
      if (!bVar5) {
        _MaxCount = 1;
      }
    }
  }
  return _MaxCount;
}


// 1001eb21 FUN_1001eb21

undefined4 __cdecl FUN_1001eb21(uint param_1)

{
  intptr_t iVar1;
  intptr_t iVar2;
  HANDLE hObject;
  BOOL BVar3;
  DWORD DVar4;
  undefined4 uVar5;
  
  iVar1 = __get_osfhandle(param_1);
  if (iVar1 != -1) {
    if (((param_1 == 1) && ((*(byte *)(DAT_1004bbe0 + 0x74) & 1) != 0)) ||
       ((param_1 == 2 && ((*(byte *)(DAT_1004bbe0 + 0x3c) & 1) != 0)))) {
      iVar1 = __get_osfhandle(2);
      iVar2 = __get_osfhandle(1);
      if (iVar2 == iVar1) goto LAB_1001eb80;
    }
    hObject = (HANDLE)__get_osfhandle(param_1);
    BVar3 = CloseHandle(hObject);
    if (BVar3 == 0) {
      DVar4 = GetLastError();
      goto LAB_1001eb82;
    }
  }
LAB_1001eb80:
  DVar4 = 0;
LAB_1001eb82:
  FUN_1001cb74(param_1);
  *(undefined1 *)((&DAT_1004bbe0)[(int)param_1 >> 5] + 4 + (param_1 & 0x1f) * 0x38) = 0;
  if (DVar4 == 0) {
    uVar5 = 0;
  }
  else {
    FUN_10011e89(DVar4);
    uVar5 = 0xffffffff;
  }
  return uVar5;
}


// 1001ecd0 FUN_1001ecd0

void __thiscall FUN_1001ecd0(void *this,uint *param_1,uint param_2)

{
  uint *puVar1;
  uint uVar2;
  undefined4 *puVar3;
  undefined4 uVar4;
  undefined4 uVar5;
  byte bVar6;
  uint uVar7;
  uint uVar8;
  int iVar9;
  uint *local_4;
  
  uVar8 = (uint)param_1 & 3;
  local_4 = param_1;
  for (param_1 = (uint *)param_2; (uVar8 != 0 && (7 < (int)param_1)); param_1 = param_1 + -2) {
    iVar9 = *(int *)((int)this + 0x14);
    if (iVar9 < 8) {
      uVar4 = *(undefined4 *)((int)this + 0x10);
      iVar9 = 8 - iVar9;
      puVar3 = *(undefined4 **)((int)this + 0x18);
      if (puVar3 == *(undefined4 **)((int)this + 0x1c)) {
        *(undefined4 *)((int)this + 0x14) = 1;
        *(undefined4 *)((int)this + 0x10) = 0;
        *(undefined1 *)((int)this + 4) = 1;
LAB_1001ed98:
        *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
      }
      else {
        if (puVar3 <= *(undefined4 **)((int)this + 0x1c)) {
          *(undefined4 *)((int)this + 0x10) = *puVar3;
          goto LAB_1001ed98;
        }
        *(undefined1 *)((int)this + 4) = 1;
        *(undefined4 *)((int)this + 0x10) = 0;
      }
      if (*(char *)((int)this + 4) == '\0') {
        uVar8 = *(uint *)(&DAT_1002aa78 + iVar9 * 4);
        uVar5 = *(undefined4 *)((int)this + 0x14);
        *(int *)((int)this + 0x14) = 0x20 - iVar9;
        bVar6 = (byte)uVar4 | (byte)((uVar8 & *(uint *)((int)this + 0x10)) << ((byte)uVar5 & 0x1f));
        *(uint *)((int)this + 0x10) = *(uint *)((int)this + 0x10) >> ((byte)iVar9 & 0x1f);
      }
      else {
        bVar6 = 0;
      }
    }
    else {
      bVar6 = (byte)*(uint *)((int)this + 0x10);
      *(int *)((int)this + 0x14) = iVar9 + -8;
      if (iVar9 + -8 == 0) {
        puVar3 = *(undefined4 **)((int)this + 0x18);
        *(undefined4 *)((int)this + 0x14) = 0x20;
        if (puVar3 == *(undefined4 **)((int)this + 0x1c)) {
          *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
          *(undefined4 *)((int)this + 0x14) = 1;
          *(undefined4 *)((int)this + 0x10) = 0;
        }
        else if (*(undefined4 **)((int)this + 0x1c) < puVar3) {
          *(undefined1 *)((int)this + 4) = 1;
          *(undefined4 *)((int)this + 0x10) = 0;
        }
        else {
          uVar4 = *puVar3;
          *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
          *(undefined4 *)((int)this + 0x10) = uVar4;
        }
      }
      else {
        *(uint *)((int)this + 0x10) = *(uint *)((int)this + 0x10) >> 8;
      }
    }
    *(byte *)local_4 = bVar6;
    local_4 = (uint *)((int)local_4 + 1);
    uVar8 = (uint)local_4 & 3;
  }
  if (0x1f < (int)param_1) {
    uVar8 = (uint)param_1 >> 5;
    param_1 = param_1 + uVar8 * -8;
    do {
      iVar9 = *(int *)((int)this + 0x14);
      if (iVar9 < 0x20) {
        uVar7 = *(uint *)((int)this + 0x10);
        iVar9 = 0x20 - iVar9;
        puVar3 = *(undefined4 **)((int)this + 0x18);
        if (puVar3 == *(undefined4 **)((int)this + 0x1c)) {
          *(undefined4 *)((int)this + 0x14) = 1;
          *(undefined4 *)((int)this + 0x10) = 0;
          *(undefined1 *)((int)this + 4) = 1;
LAB_1001ee9e:
          *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
        }
        else {
          if (puVar3 <= *(undefined4 **)((int)this + 0x1c)) {
            *(undefined4 *)((int)this + 0x10) = *puVar3;
            goto LAB_1001ee9e;
          }
          *(undefined1 *)((int)this + 4) = 1;
          *(undefined4 *)((int)this + 0x10) = 0;
        }
        if (*(char *)((int)this + 4) == '\0') {
          uVar2 = *(uint *)(&DAT_1002aa78 + iVar9 * 4);
          uVar4 = *(undefined4 *)((int)this + 0x14);
          *(int *)((int)this + 0x14) = 0x20 - iVar9;
          uVar7 = uVar7 | (uVar2 & *(uint *)((int)this + 0x10)) << ((byte)uVar4 & 0x1f);
          *(uint *)((int)this + 0x10) = *(uint *)((int)this + 0x10) >> ((byte)iVar9 & 0x1f);
        }
        else {
          uVar7 = 0;
        }
      }
      else {
        uVar7 = *(uint *)((int)this + 0x10);
        *(int *)((int)this + 0x14) = iVar9 + -0x20;
        if (iVar9 + -0x20 == 0) {
          puVar3 = *(undefined4 **)((int)this + 0x18);
          *(undefined4 *)((int)this + 0x14) = 0x20;
          if (puVar3 == *(undefined4 **)((int)this + 0x1c)) {
            *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
            *(undefined4 *)((int)this + 0x14) = 1;
            *(undefined4 *)((int)this + 0x10) = 0;
          }
          else if (*(undefined4 **)((int)this + 0x1c) < puVar3) {
            *(undefined1 *)((int)this + 4) = 1;
            *(undefined4 *)((int)this + 0x10) = 0;
          }
          else {
            uVar4 = *puVar3;
            *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
            *(undefined4 *)((int)this + 0x10) = uVar4;
          }
        }
        else {
          *(undefined4 *)((int)this + 0x10) = 0;
        }
      }
      puVar1 = local_4 + 1;
      uVar8 = uVar8 - 1;
      *local_4 = uVar7;
      local_4 = puVar1;
    } while (uVar8 != 0);
  }
  if (7 < (int)param_1) {
    uVar8 = (uint)param_1 >> 3;
    param_1 = param_1 + uVar8 * -2;
    do {
      iVar9 = *(int *)((int)this + 0x14);
      if (iVar9 < 8) {
        uVar4 = *(undefined4 *)((int)this + 0x10);
        iVar9 = 8 - iVar9;
        puVar3 = *(undefined4 **)((int)this + 0x18);
        if (puVar3 == *(undefined4 **)((int)this + 0x1c)) {
          *(undefined4 *)((int)this + 0x14) = 1;
          *(undefined4 *)((int)this + 0x10) = 0;
          *(undefined1 *)((int)this + 4) = 1;
LAB_1001efa3:
          *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
        }
        else {
          if (puVar3 <= *(undefined4 **)((int)this + 0x1c)) {
            *(undefined4 *)((int)this + 0x10) = *puVar3;
            goto LAB_1001efa3;
          }
          *(undefined1 *)((int)this + 4) = 1;
          *(undefined4 *)((int)this + 0x10) = 0;
        }
        if (*(char *)((int)this + 4) == '\0') {
          uVar7 = *(uint *)(&DAT_1002aa78 + iVar9 * 4);
          uVar5 = *(undefined4 *)((int)this + 0x14);
          *(int *)((int)this + 0x14) = 0x20 - iVar9;
          bVar6 = (byte)uVar4 |
                  (byte)((uVar7 & *(uint *)((int)this + 0x10)) << ((byte)uVar5 & 0x1f));
          *(uint *)((int)this + 0x10) = *(uint *)((int)this + 0x10) >> ((byte)iVar9 & 0x1f);
        }
        else {
          bVar6 = 0;
        }
      }
      else {
        bVar6 = (byte)*(uint *)((int)this + 0x10);
        *(int *)((int)this + 0x14) = iVar9 + -8;
        if (iVar9 + -8 == 0) {
          puVar3 = *(undefined4 **)((int)this + 0x18);
          *(undefined4 *)((int)this + 0x14) = 0x20;
          if (puVar3 == *(undefined4 **)((int)this + 0x1c)) {
            *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
            *(undefined4 *)((int)this + 0x14) = 1;
            *(undefined4 *)((int)this + 0x10) = 0;
          }
          else if (*(undefined4 **)((int)this + 0x1c) < puVar3) {
            *(undefined1 *)((int)this + 4) = 1;
            *(undefined4 *)((int)this + 0x10) = 0;
          }
          else {
            uVar4 = *puVar3;
            *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
            *(undefined4 *)((int)this + 0x10) = uVar4;
          }
        }
        else {
          *(uint *)((int)this + 0x10) = *(uint *)((int)this + 0x10) >> 8;
        }
      }
      *(byte *)local_4 = bVar6;
      local_4 = (uint *)((int)local_4 + 1);
      uVar8 = uVar8 - 1;
    } while (uVar8 != 0);
  }
  if (param_1 == (uint *)0x0) {
    return;
  }
  iVar9 = *(int *)((int)this + 0x14);
  if ((int)param_1 <= iVar9) {
    bVar6 = (byte)*(undefined4 *)(&DAT_1002aa78 + (int)param_1 * 4) &
            (byte)*(uint *)((int)this + 0x10);
    *(int *)((int)this + 0x14) = iVar9 - (int)param_1;
    if (iVar9 - (int)param_1 != 0) {
      *(uint *)((int)this + 0x10) = *(uint *)((int)this + 0x10) >> ((byte)param_1 & 0x1f);
      *(byte *)local_4 = bVar6;
      return;
    }
    puVar3 = *(undefined4 **)((int)this + 0x18);
    *(undefined4 *)((int)this + 0x14) = 0x20;
    if (puVar3 != *(undefined4 **)((int)this + 0x1c)) {
      if (puVar3 <= *(undefined4 **)((int)this + 0x1c)) {
        *(undefined4 *)((int)this + 0x10) = *puVar3;
        *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
        *(byte *)local_4 = bVar6;
        return;
      }
      *(undefined4 *)((int)this + 0x10) = 0;
      *(undefined1 *)((int)this + 4) = 1;
      *(byte *)local_4 = bVar6;
      return;
    }
    *(undefined4 *)((int)this + 0x10) = 0;
    *(undefined4 *)((int)this + 0x14) = 1;
    *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
    *(byte *)local_4 = bVar6;
    return;
  }
  uVar4 = *(undefined4 *)((int)this + 0x10);
  iVar9 = (int)param_1 - iVar9;
  puVar3 = *(undefined4 **)((int)this + 0x18);
  if (puVar3 == *(undefined4 **)((int)this + 0x1c)) {
    *(undefined4 *)((int)this + 0x14) = 1;
    *(undefined4 *)((int)this + 0x10) = 0;
    *(undefined1 *)((int)this + 4) = 1;
  }
  else {
    if (*(undefined4 **)((int)this + 0x1c) < puVar3) {
      *(undefined1 *)((int)this + 4) = 1;
      *(undefined4 *)((int)this + 0x10) = 0;
      goto LAB_1001f0cf;
    }
    *(undefined4 *)((int)this + 0x10) = *puVar3;
  }
  *(undefined4 **)((int)this + 0x18) = puVar3 + 1;
LAB_1001f0cf:
  if (*(char *)((int)this + 4) == '\0') {
    uVar8 = *(uint *)((int)this + 0x10);
    uVar7 = *(uint *)(&DAT_1002aa78 + iVar9 * 4);
    uVar5 = *(undefined4 *)((int)this + 0x14);
    *(int *)((int)this + 0x14) = 0x20 - iVar9;
    *(uint *)((int)this + 0x10) = uVar8 >> ((byte)iVar9 & 0x1f);
    *(byte *)local_4 = (byte)uVar4 | (byte)((uVar7 & uVar8) << ((byte)uVar5 & 0x1f));
    return;
  }
  *(byte *)local_4 = 0;
  return;
}


// 1001f120 FUN_1001f120

bool __thiscall FUN_1001f120(void *this,uint *param_1,int param_2)

{
  FUN_1001ecd0(this,param_1,param_2 * 8);
  return *(char *)((int)this + 4) == '\0';
}


// 1001f150 FUN_1001f150

void __thiscall FUN_1001f150(void *this,uint param_1,int param_2)

{
  uint *puVar1;
  uint uVar2;
  int iVar3;
  uint uVar4;
  int iVar5;
  
  uVar2 = *(uint *)((int)this + 0xc);
  if (*(int *)((int)this + 8) < (int)(uVar2 + param_2)) {
    *(int *)((int)this + 0xc) = *(int *)((int)this + 8);
    *(undefined1 *)((int)this + 0x10) = 1;
    return;
  }
                    /* WARNING: Load size is inaccurate */
  uVar4 = uVar2 & 0x1f;
  iVar5 = (int)uVar2 >> 5;
  *(uint *)(*this + iVar5 * 4) =
       (&DAT_1003a8d8)[uVar4 * 0x21 + param_2] & *(uint *)(*this + iVar5 * 4) |
       param_1 << (sbyte)uVar4;
  iVar3 = 0x20 - uVar4;
  if (iVar3 < param_2) {
                    /* WARNING: Load size is inaccurate */
    puVar1 = (uint *)(*this + 4 + iVar5 * 4);
    *puVar1 = (&DAT_1003a8d8)[param_2 - iVar3] & *puVar1 | param_1 >> ((byte)iVar3 & 0x1f);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
  return;
}


// 1001f2c0 FUN_1001f2c0

uint __thiscall FUN_1001f2c0(void *this,uint *param_1,uint param_2)

{
  uint uVar1;
  uint uVar2;
  int iVar3;
  uint *puVar4;
  size_t sVar5;
  uint uVar6;
  uint uVar7;
  
  uVar2 = *(int *)((int)this + 0xc) + param_2;
  if ((int)uVar2 <= *(int *)((int)this + 8)) {
    uVar7 = (uint)param_1 & 3;
    puVar4 = param_1;
    for (uVar2 = param_2; (uVar7 != 0 && (7 < (int)uVar2)); uVar2 = uVar2 - 8) {
      FUN_1001f150(this,(uint)(byte)*puVar4,8);
      puVar4 = (uint *)((int)puVar4 + 1);
      uVar7 = (uint)puVar4 & 3;
    }
    if (0x1f < (int)uVar2) {
      uVar7 = *(uint *)((int)this + 0xc);
      if ((uVar7 & 7) == 0) {
        sVar5 = (int)uVar2 >> 3;
                    /* WARNING: Load size is inaccurate */
        FUN_10009bb0((void *)(((int)uVar7 >> 3) + *this),puVar4,sVar5);
        puVar4 = (uint *)((int)puVar4 + sVar5);
        uVar2 = uVar2 + sVar5 * -8;
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + sVar5 * 8;
      }
      else {
        uVar6 = uVar7 & 0x1f;
        iVar3 = -uVar6 + 0x40;
                    /* WARNING: Load size is inaccurate */
        param_1 = (uint *)((int)uVar7 >> 5);
        uVar7 = *(uint *)(*this + (int)param_1 * 4) & *(uint *)(&DAT_1003a958 + uVar6 * 0x84);
        if (iVar3 <= (int)uVar2) {
          param_2 = (uVar2 - iVar3 >> 5) + 1;
          uVar2 = uVar2 + param_2 * -0x20;
          do {
            uVar1 = *puVar4;
            param_1 = (uint *)((int)param_1 + 1);
                    /* WARNING: Load size is inaccurate */
            *(uint *)(*this + -4 + (int)param_1 * 4) = uVar1 << (sbyte)uVar6 | uVar7;
            *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 0x20;
            uVar7 = uVar1 >> ((char)-uVar6 + 0x20U & 0x1f);
            puVar4 = puVar4 + 1;
            param_2 = param_2 - 1;
          } while (param_2 != 0);
        }
                    /* WARNING: Load size is inaccurate */
        *(uint *)(*this + (int)param_1 * 4) = uVar7;
        if (0x1f < (int)uVar2) {
          FUN_1001f150(this,*puVar4,0x20);
          puVar4 = puVar4 + 1;
          uVar2 = uVar2 - 0x20;
        }
      }
    }
    if (7 < (int)uVar2) {
      uVar7 = uVar2 >> 3;
      uVar2 = uVar2 + uVar7 * -8;
      do {
        FUN_1001f150(this,(uint)(byte)*puVar4,8);
        puVar4 = (uint *)((int)puVar4 + 1);
        uVar7 = uVar7 - 1;
      } while (uVar7 != 0);
    }
    if (uVar2 != 0) {
      FUN_1001f150(this,(uint)(byte)*puVar4,uVar2);
    }
    return (uint)(*(char *)((int)this + 0x10) == '\0');
  }
  *(undefined1 *)((int)this + 0x10) = 1;
  return uVar2 & 0xffffff00;
}


// 1001f460 FUN_1001f460

void __thiscall FUN_1001f460(void *this,uint *param_1,int param_2)

{
  FUN_1001f2c0(this,param_1,param_2 * 8);
  return;
}


// 1001f480 FUN_1001f480

void __thiscall FUN_1001f480(void *this,undefined4 param_1,uint param_2,int param_3)

{
  *(undefined4 *)this = param_1;
  *(undefined1 *)((int)this + 0x11) = 1;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(uint *)((int)this + 4) = param_2 & 0xfffffffc;
  if (param_3 == -1) {
    *(uint *)((int)this + 8) = (param_2 & 0xfffffffc) * 8;
    *(undefined4 *)((int)this + 0xc) = 0;
    *(undefined1 *)((int)this + 0x10) = 0;
    return;
  }
  *(int *)((int)this + 8) = param_3;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined1 *)((int)this + 0x10) = 0;
  return;
}


// 1001f4d0 FUN_1001f4d0

void __thiscall FUN_1001f4d0(void *this,uint param_1,int param_2)

{
  int iVar1;
  byte *pbVar2;
  
  if ((int)param_1 < 0) {
    FUN_1001f150(this,param_1 + 0x80000000,param_2 + -1);
    iVar1 = *(int *)((int)this + 0xc);
    if (*(int *)((int)this + 8) < iVar1 + 1) {
      *(undefined1 *)((int)this + 0x10) = 1;
    }
    if (*(char *)((int)this + 0x10) == '\0') {
                    /* WARNING: Load size is inaccurate */
      pbVar2 = (byte *)((iVar1 >> 3) + *this);
      *pbVar2 = *pbVar2 | '\x01' << ((byte)iVar1 & 7);
      *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
      return;
    }
  }
  else {
    FUN_1001f150(this,param_1,param_2 + -1);
    iVar1 = *(int *)((int)this + 0xc);
    if (*(int *)((int)this + 8) < iVar1 + 1) {
      *(undefined1 *)((int)this + 0x10) = 1;
    }
    if (*(char *)((int)this + 0x10) == '\0') {
                    /* WARNING: Load size is inaccurate */
      pbVar2 = (byte *)((iVar1 >> 3) + *this);
      *pbVar2 = *pbVar2 & ~('\x01' << ((byte)iVar1 & 7));
      *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
    }
  }
  return;
}


// 1001f560 FUN_1001f560

void __thiscall FUN_1001f560(void *this,uint param_1)

{
  FUN_1001f4d0(this,param_1,0x20);
  return;
}


// 1001f5b0 FUN_1001f5b0

void FUN_1001f5b0(void)

{
  float10 fVar1;
  int local_4;
  
  local_4 = 0;
  do {
    fVar1 = (float10)local_4;
    local_4 = local_4 + 1;
    fVar1 = (float10)fsin(fVar1 * (float10)0.02454369260617026);
    *(float *)(&DAT_1003b9e4 + local_4 * 4) = (float)fVar1;
  } while (local_4 < 0x100);
  return;
}


// 1001f5e0 FUN_1001f5e0

void FUN_1001f5e0(void)

{
  CPUInformation *pCVar1;
  
  if (DAT_1003bde8 == '\0') {
    pCVar1 = GetCPUInformation();
    if (((byte)pCVar1[4] & 8) == 0) {
      Error("SSE is required.");
    }
    DAT_1003bde8 = '\x01';
    FUN_1001f5b0();
    FUN_1001f770();
    FUN_1001f740(0x31415926);
  }
  return;
}


// 1001f650 FUN_1001f650

/* WARNING: Removing unreachable block (ram,0x1001f6e0) */
/* WARNING: Removing unreachable block (ram,0x1001f68c) */
/* WARNING: Removing unreachable block (ram,0x1001f6b7) */
/* WARNING: Removing unreachable block (ram,0x1001f708) */

void __thiscall FUN_1001f650(void *this,uint param_1)

{
  uint uVar1;
  float *pfVar2;
  int iVar3;
  
  *(int *)((int)this + 0x370) = (int)this + 0x170;
  *(int *)((int)this + 0x374) = (int)this + 0x360;
  pfVar2 = (float *)((int)this + 8);
  iVar3 = 0x37;
  do {
    pfVar2[-2] = (float)(param_1 >> 0x10) * 1.5258789e-05;
    uVar1 = param_1 * -0x44bf19d3 + 0xbb40e62d;
    pfVar2[-1] = (float)(uVar1 >> 0x10) * 1.5258789e-05;
    uVar1 = uVar1 * -0x44bf19d3 + 0xbb40e62d;
    *pfVar2 = (float)(uVar1 >> 0x10) * 1.5258789e-05;
    uVar1 = uVar1 * -0x44bf19d3 + 0xbb40e62d;
    pfVar2[1] = (float)(uVar1 >> 0x10) * 1.5258789e-05;
    param_1 = uVar1 * -0x44bf19d3 + 0xbb40e62d;
    pfVar2 = pfVar2 + 4;
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  return;
}


// 1001f740 FUN_1001f740

void __cdecl FUN_1001f740(int param_1)

{
  undefined *this;
  int iVar1;
  
  this = &DAT_1003be20;
  iVar1 = 0;
  do {
    FUN_1001f650(this,iVar1 + param_1);
    this = this + 0x380;
    iVar1 = iVar1 + 1;
  } while ((int)this < 0x10042e20);
  return;
}


// 1001f770 FUN_1001f770

void FUN_1001f770(void)

{
  int iVar1;
  byte bVar2;
  undefined4 extraout_ECX;
  undefined4 extraout_ECX_00;
  undefined4 extraout_ECX_01;
  undefined4 extraout_ECX_02;
  undefined4 extraout_EDX;
  undefined4 extraout_EDX_00;
  undefined4 extraout_EDX_01;
  undefined4 extraout_EDX_02;
  int iVar3;
  float10 fVar4;
  ulonglong uVar5;
  float fStack00000008;
  int in_stack_00000010;
  int local_10;
  
  iVar3 = 0;
  do {
    FUN_1001fe80();
    uVar5 = FUN_1000ffe0(extraout_ECX,extraout_EDX);
    iVar1 = (int)uVar5;
    if (iVar1 < 0) {
      iVar1 = 0;
    }
    else if (0xff < iVar1) {
      iVar1 = 0xff;
    }
    (&DAT_1004aaa8)[iVar3] = (char)iVar1;
    iVar3 = iVar3 + 1;
  } while (iVar3 < 0x100);
  local_10 = 0;
  do {
    FUN_1001fe80();
    uVar5 = FUN_1000ffe0(extraout_ECX_00,extraout_EDX_00);
    iVar3 = (int)uVar5;
    if (iVar3 < 0) {
      iVar3 = 0;
    }
    else if (0xff < iVar3) {
      iVar3 = 0xff;
    }
    (&DAT_100456a8)[local_10] = iVar3;
    local_10 = local_10 + 1;
  } while (local_10 < 0x400);
  iVar3 = 0;
  do {
    fVar4 = (float10)FUN_1001fe80();
    (&DAT_10042ea8)[iVar3] = (float)fVar4;
    fVar4 = (float10)FUN_1001fe80();
    (&DAT_100442a8)[iVar3] = (float)fVar4;
    fVar4 = (float10)FUN_1001fe80();
    (&DAT_100466a8)[iVar3] = (float)fVar4;
    iVar3 = iVar3 + 1;
  } while (iVar3 < 0x100);
  iVar3 = 0;
  do {
    FUN_1001fe80();
    uVar5 = FUN_1000ffe0(extraout_ECX_01,extraout_EDX_01);
    (&DAT_100446a8)[iVar3] = (int)uVar5;
    iVar3 = iVar3 + 1;
  } while (iVar3 < 0x400);
  fStack00000008 = 1.0;
  if (in_stack_00000010 == 2) {
    fStack00000008 = 0.5;
  }
  else if (in_stack_00000010 == 4) {
    fStack00000008 = 0.25;
  }
  iVar3 = 0;
  do {
    fVar4 = (float10)FUN_1001fe80();
    (&DAT_10046aa8)[iVar3] = (float)(fVar4 * (float10)fStack00000008);
    if ((float10)1 < fVar4 * (float10)fStack00000008) {
      (&DAT_10046aa8)[iVar3] = (float)(float10)1;
    }
    uVar5 = FUN_1000ffe0(extraout_ECX_02,extraout_EDX_02);
    if ((int)uVar5 < 0x100) {
      bVar2 = ((int)uVar5 < 0) - 1U & (byte)uVar5;
    }
    else {
      bVar2 = 0xff;
    }
    (&DAT_100432a8)[iVar3] = bVar2;
    iVar3 = iVar3 + 1;
  } while (iVar3 < 0x1000);
  return;
}


// 1001fe80 FUN_1001fe80

void FUN_1001fe80(void)

{
  ushort in_FPUControlWord;
  float10 in_ST0;
  float10 in_ST1;
  
  if ((DAT_1004bcf8 != 0) && ((MXCSR & 0x1f80) == 0x1f80 && (in_FPUControlWord & 0x7f) == 0x7f)) {
    FUN_10022310();
    return;
  }
  FUN_1001fedd(SUB84((double)in_ST1,0),(uint)((ulonglong)(double)in_ST1 >> 0x20),
               SUB84((double)in_ST0,0),(uint)((ulonglong)(double)in_ST0 >> 0x20));
  return;
}


// 1001fedd FUN_1001fedd
// diperbaiki: mode=decompile dengan parameter stack manual

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined1  [10] __cdecl FUN_1001fedd(byte stack_args [20])

{
  uint in_EAX;
  uint uVar1;
  char extraout_CL;
  byte extraout_CL_00;
  char extraout_CL_01;
  uint extraout_ECX;
  int extraout_ECX_00;
  int iVar2;
  int extraout_ECX_01;
  bool bVar3;
  short in_FPUControlWord;
  float10 extraout_ST0;
  undefined1 auVar4 [10];
  float10 extraout_ST0_00;
  float10 extraout_ST0_01;
  float10 fVar5;
  float10 extraout_ST1;
  float10 extraout_ST1_00;
  float10 extraout_ST1_01;
  float10 fVar6;
  unkbyte10 in_ST7;
  undefined4 unaff_retaddr;
  double *pdVar7;
  double local_78 [2];
  undefined4 uStack_64;
  undefined1 auStack_e [10];
  
  uVar1 = in_EAX;
  if (in_FPUControlWord != 0x27f) {
    auStack_e._6_4_ = 0x1001fef1;
    in_EAX = FUN_10020875();
    uVar1 = extraout_ECX;
  }
  bVar3 = (uVar1 & 0x7ff00000) == 0x7ff00000;
  if (bVar3) {
    if ((in_EAX & 0xfffff) == 0 && stack_args._8_4_ == 0) {
      auStack_e._6_4_ = 0x1001ffba;
      __fload_withFB(0,(int)stack_args);
      iVar2 = extraout_ECX_01;
      fVar5 = extraout_ST0_01;
      fVar6 = extraout_ST1_01;
      goto LAB_1001ffba;
    }
    auStack_e._6_4_ = 0x1001ff7d;
    __fload_withFB(0,(int)stack_args);
    fVar5 = extraout_ST0_00;
    fVar6 = extraout_ST1_00;
    if ((stack_args[0xe] & 8) == 0) {
      iVar2 = extraout_ECX_00 + 1;
      goto LAB_1001ffba;
    }
LAB_1001ff87:
    auVar4 = (undefined1  [10])(fVar5 + fVar6);
  }
  else {
    auStack_e._6_4_ = 0x1001ff0c;
    uVar1 = __fload_withFB(uVar1 & 0x7ff00000,(int)stack_args);
    if (!bVar3) {
      if (((uVar1 & 0x7ff00000) != 0) ||
         ((CONCAT12(stack_args[6],stack_args._4_2_) & 0xfffff) != 0 || stack_args._0_4_ != 0)) {
        if ((stack_args[7] & 0x80) != 0) {
          auStack_e._6_4_ = 0x10020084;
          FUN_100200a2();
          auVar4 = _DAT_10037690;
          if (extraout_CL_01 == '\0') goto LAB_1001ff55;
        }
        auStack_e._6_4_ = 0x1001ff31;
        auVar4 = FUN_10020860();
        if (extraout_CL == '\x01') {
          auVar4 = (undefined1  [10])-(float10)auVar4;
        }
        if (DAT_10039e10 != 0) {
          return auVar4;
        }
        auVar4 = (undefined1  [10])
                 FUN_10020949(&DAT_10037688,0x1d,unaff_retaddr,stack_args._0_4_,
                              CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)),
                              stack_args._8_4_,
                              CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)))
        ;
        return auVar4;
      }
      if ((CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)) & 0x7fffffff) == 0
          && stack_args._8_4_ == 0) {
        auVar4 = (undefined1  [10])FUN_100208fe();
        return auVar4;
      }
      auStack_e._6_4_ = 0x1002003c;
      FUN_100200a2();
      if ((stack_args._15_4_ & 0x80) == 0) {
        if ((stack_args[7] >> 7 & extraout_CL_00) == 0) {
          return (undefined1  [10])(float10)0;
        }
        auVar4 = (undefined1  [10])FUN_100208fe();
        return auVar4;
      }
      auVar4 = _DAT_10037830;
      if ((stack_args[7] >> 7 & extraout_CL_00) != 0) {
        auVar4 = (undefined1  [10])-(float10)_DAT_10037830;
      }
      goto LAB_1001ff55;
    }
    iVar2 = 0;
    fVar5 = extraout_ST0;
    fVar6 = extraout_ST1;
LAB_1001ffba:
    if (((CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)) & 0x7ff00000) ==
         0x7ff00000) &&
       ((CONCAT12(stack_args[6],stack_args._4_2_) & 0xfffff) != 0 || stack_args._0_4_ != 0)) {
      if ((stack_args[6] & 8) != 0) goto LAB_1001ff87;
    }
    else if (iVar2 == 0) {
      pdVar7 = local_78;
      uStack_64 = 0x1001ffe8;
      auStack_e = (undefined1  [10])in_ST7;
      iVar2 = FUN_100230db(SUB84((double)fVar5,0),(int)((ulonglong)(double)fVar5 >> 0x20),
                           SUB84((double)fVar6,0),(int)((ulonglong)(double)fVar6 >> 0x20),local_78);
      auVar4 = (undefined1  [10])(float10)*pdVar7;
      if (iVar2 == 0) {
        return (undefined1  [10])(float10)*pdVar7;
      }
      goto LAB_1001ff55;
    }
    auVar4 = (undefined1  [10])(fVar5 + fVar6);
  }
LAB_1001ff55:
  if (DAT_10039e10 != 0) {
    return auVar4;
  }
  auStack_e._6_4_ = 0x1001ff72;
  auVar4 = (undefined1  [10])
           FUN_10020800(&DAT_10037688,0x1d,in_FPUControlWord,unaff_retaddr,stack_args._0_4_,
                        CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)),
                        stack_args._8_4_,
                        CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)));
  return auVar4;
}
// 100200a2 FUN_100200a2

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_100200a2(void)

{
  float10 in_ST0;
  
  if (ROUND(in_ST0) == in_ST0) {
    return;
  }
  return;
}


// 100200d0 FUN_100200d0

void FUN_100200d0(void)

{
  float10 in_ST0;
  
  FUN_100200ee((double)in_ST0);
  return;
}


// 100200ee FUN_100200ee

float10 __cdecl FUN_100200ee(double param_1)

{
  undefined1 auVar1 [12];
  double dVar2;
  undefined1 auVar3 [12];
  uint uVar4;
  int iVar5;
  int iVar6;
  ulonglong uVar7;
  int in_XMM0_Da;
  uint in_XMM0_Db;
  uint in_XMM0_Dd;
  double dVar8;
  double dVar9;
  ulonglong in_XMM2_Qa;
  undefined1 auVar10 [16];
  undefined1 auVar11 [16];
  double dVar12;
  double dVar13;
  undefined1 auVar14 [16];
  undefined1 auVar15 [16];
  undefined4 uVar16;
  undefined1 auVar17 [16];
  ulonglong in_XMM7_Qa;
  double dVar18;
  undefined1 auVar19 [16];
  double local_c;
  
  dVar2 = (double)CONCAT44(in_XMM0_Db,in_XMM0_Da);
  uVar4 = in_XMM0_Db >> 0xc & 0x7ffff;
  if (uVar4 - 0x3fb00 < 0x3bb) {
    auVar10._0_8_ = ((ulonglong)in_XMM0_Db & 0xffffc000) << 0x20;
    auVar10._8_8_ = in_XMM2_Qa & in_XMM7_Qa;
    iVar5 = (in_XMM0_Db >> 0xc & 0xfffc) - 0xfb00;
    iVar6 = iVar5 * 4;
    dVar13 = SUB168(auVar10 | ZEXT816(0x200000000000),0);
    dVar12 = SQRT(1.0 - dVar2 * dVar2) * dVar13;
    dVar8 = dVar2 * *(double *)(&DAT_1002bd50 + iVar5 * 2) - dVar12;
    uVar4 = SUB164(auVar10 | ZEXT816(0x200000000000),4) & 0x80000000;
    dVar9 = dVar8 * dVar8;
    auVar3._4_8_ = 0;
    auVar3._0_4_ = uVar4;
    auVar11._0_12_ = auVar3 << 0x20;
    auVar11._12_4_ = uVar4;
    return (float10)((((dVar9 * -0.044642857142857144 + -0.075) * dVar8 * dVar9 * dVar9 +
                      (dVar8 * dVar9 * -0.16666666666666666 -
                      ((double)(*(ulonglong *)(&DAT_1002ae50 + iVar6) ^ (ulonglong)uVar4 << 0x20) -
                      6.123233995736766e-17))) -
                     ((dVar2 + dVar13) * ((double)CONCAT44(in_XMM0_Db,in_XMM0_Da) - dVar13)) /
                     (dVar2 * *(double *)(&DAT_1002bd50 + iVar5 * 2) + dVar12)) -
                    ((double)(*(ulonglong *)(&DAT_1002ae58 + iVar6) ^ auVar11._8_8_) -
                    1.5707963267948966));
  }
  if (uVar4 - 0x3febb < 0x41) {
    dVar18 = (double)((ulonglong)(in_XMM0_Db >> 6) << 0x26);
    auVar1._4_4_ = in_XMM0_Db;
    auVar1._0_4_ = in_XMM0_Da;
    auVar1._8_4_ = in_XMM0_Dd >> 0xc;
    dVar13 = 1.0 - dVar18 * dVar18;
    dVar8 = ((double)CONCAT44(in_XMM0_Db,in_XMM0_Da) + dVar18) * (dVar2 - dVar18);
    dVar12 = SQRT(dVar13 - dVar8);
    iVar5 = -(uint)(SUB121(auVar1 >> 0x3f,0) & 1);
    dVar9 = (double)((ulonglong)dVar12 & 0xffffc00000000000 |
                    ((ulonglong)in_XMM0_Db & 0x80000000) << 0x20 | 0x200000000000);
    auVar14._0_8_ = CONCAT44(iVar5,iVar5);
    auVar14._8_4_ = iVar5;
    auVar14._12_4_ = iVar5;
    iVar6 = (ushort)((ulonglong)((longlong)dVar12 << 2) >> 0x30) - 0xfec0;
    dVar12 = dVar12 * *(double *)(&DAT_1002bd50 + iVar6 * 8);
    dVar18 = (dVar18 * dVar9 - dVar12) + (dVar2 - dVar18) * dVar9;
    dVar2 = ((dVar13 - dVar9 * dVar9) - dVar8) / (dVar12 + dVar12 + dVar18);
    iVar6 = iVar6 * 0x10;
    dVar9 = (double)(auVar14._8_8_ & 0xc00921fb54442d18) + *(double *)(&DAT_1002ae58 + iVar6);
    dVar13 = dVar18 * dVar18;
    dVar8 = dVar2 + dVar9;
    return (float10)(double)((ulonglong)
                             ((dVar13 * -0.044642857142857144 + -0.075) * dVar18 * dVar13 * dVar13 +
                              dVar18 * dVar13 * -0.16666666666666666 +
                              (double)(auVar14._0_8_ & 0xbca1a62633145c07) +
                              *(double *)(&DAT_1002ae50 + iVar6) + dVar2 + (dVar9 - dVar8) + dVar8)
                            ^ (ulonglong)((ushort)iVar5 & 0x8000) << 0x30);
  }
  if (uVar4 - 0x3c300 < 0x3800) {
    dVar8 = (double)CONCAT44(in_XMM0_Db,in_XMM0_Da) * (double)CONCAT44(in_XMM0_Db,in_XMM0_Da);
    dVar9 = (double)CONCAT44(in_XMM0_Db,in_XMM0_Da) * (double)CONCAT44(in_XMM0_Db,in_XMM0_Da);
    dVar13 = dVar2 * dVar8;
    return (float10)((1.5707963267948966 - dVar2) +
                    (((6.123233995736766e-17 -
                      dVar13 * dVar13 * dVar13 *
                      (dVar8 * 0.022372159090909092 + 0.030381944444444444 +
                      dVar8 * dVar8 * 0.017352764423076924)) -
                     (double)CONCAT44(in_XMM0_Db,in_XMM0_Da) * dVar9 *
                     (dVar9 * 0.075 + 0.16666666666666666 + dVar9 * dVar9 * 0.044642857142857144)) -
                    (dVar2 - (1.5707963267948966 - (1.5707963267948966 - dVar2)))));
  }
  if (uVar4 - 0x3fefc < 4) {
    dVar2 = 0.5 - ABS(dVar2) * 0.5;
    uVar16 = (undefined4)((ulonglong)dVar2 >> 0x20);
    auVar19._8_4_ = SUB84(dVar2,0);
    auVar19._0_8_ = dVar2;
    auVar19._12_4_ = uVar16;
    dVar8 = SQRT(dVar2);
    dVar13 = auVar19._8_8_;
    auVar17._8_4_ = SUB84(dVar2,0);
    auVar17._0_8_ = dVar2;
    auVar17._12_4_ = uVar16;
    uVar7 = -(ulonglong)((double)CONCAT44(in_XMM0_Db,in_XMM0_Da) < 0.0);
    dVar9 = (double)((ulonglong)dVar8 & 0xfffffffff8000000);
    auVar15._8_4_ = SUB84(dVar8,0);
    auVar15._0_8_ = dVar8;
    auVar15._12_4_ = (int)((ulonglong)dVar8 >> 0x20);
    return (float10)(double)((ulonglong)
                             ((double)(uVar7 & 0xc00921fb54442d18) +
                             (dVar2 * 0.022372159090909092 + 0.030381944444444444 +
                             dVar2 * dVar2 * 0.017352764423076924) * dVar2 * dVar2 * dVar2 * dVar2 *
                             (dVar8 + dVar8) + (double)(uVar7 & 0xbca1a62633145c07) +
                             (dVar13 * 0.075 + 0.16666666666666666 +
                             dVar13 * dVar13 * 0.044642857142857144) * auVar17._8_8_ *
                             (auVar15._8_8_ + auVar15._8_8_) +
                             ((dVar2 - dVar9 * dVar9) -
                             (dVar8 - dVar9) * ((dVar8 + dVar8) - (dVar8 - dVar9))) / dVar8 +
                             dVar8 + dVar8) ^
                            (ulonglong)((ushort)(in_XMM0_Db >> 0x10) & 0x8000) << 0x30);
  }
  if (uVar4 < 0x3ff00) {
    return (float10)1.5707963267948966;
  }
  if ((in_XMM0_Db & 0x7fffffff) != 0x3ff00000 || in_XMM0_Da != 0) {
    if ((int)((((uint)((ulonglong)param_1 >> 0x20) & 0x7fffffff) + 0x80100000) -
             (uint)(SUB84(param_1,0) == 0)) < 0) {
      local_c = -NAN;
      iVar6 = 0x3a;
    }
    else {
      local_c = (double)CONCAT44(in_XMM0_Db,in_XMM0_Da) + 0.0;
      iVar6 = 0x3f0;
    }
    ___libm_error_support(&param_1,&param_1,&local_c,iVar6);
    return (float10)local_c;
  }
  uVar7 = CONCAT44((int)in_XMM0_Db >> 0x1f,(int)in_XMM0_Db >> 0x1f);
  return (float10)((double)(uVar7 & 0x400921fb54442d18) + (double)(uVar7 & 0x3ca1a62633145c07));
}


// 10020630 __trandisp1

/* Library Function - Single Match
    __trandisp1
   
   Library: Visual Studio */

void __fastcall __trandisp1(undefined4 param_1,int param_2)

{
  float10 fVar1;
  byte bVar2;
  undefined2 uVar3;
  int unaff_EBP;
  float10 in_ST0;
  
  if (*(char *)(param_2 + 0xe) == '\x05') {
    uVar3 = (undefined2)
            CONCAT31((uint3)((byte)((ushort)*(undefined2 *)(unaff_EBP + -0xa4) >> 8) & 0xfe | 2),
                     0x3f);
  }
  else {
    uVar3 = 0x133f;
  }
  *(undefined2 *)(unaff_EBP + -0xa2) = uVar3;
  fVar1 = (float10)0;
  *(int *)(unaff_EBP + -0x94) = param_2;
  *(ushort *)(unaff_EBP + -0xa0) =
       (ushort)NAN(in_ST0) << 8 | (ushort)(in_ST0 < fVar1) << 9 | (ushort)(in_ST0 != fVar1) << 10 |
       (ushort)(in_ST0 == fVar1) << 0xe;
  *(undefined1 *)(unaff_EBP + -0x90) = 0;
  bVar2 = (char)(*(char *)(unaff_EBP + -0x9f) << 1) >> 1;
                    /* WARNING: Could not recover jumptable at 0x10020695. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(param_2 + (char)(&DAT_100376ac)[(byte)((bVar2 & 7) << 1 | (char)bVar2 < '\0')] + 0x10
              ))();
  return;
}


// 10020697 __trandisp2

/* Library Function - Single Match
    __trandisp2
   
   Libraries: Visual Studio 1998, Visual Studio 2003, Visual Studio 2005, Visual Studio 2008 */

void __fastcall __trandisp2(undefined4 param_1,int param_2)

{
  float10 fVar1;
  char cVar2;
  byte bVar3;
  undefined2 uVar4;
  int unaff_EBP;
  float10 in_ST0;
  float10 in_ST1;
  
  if (*(char *)(param_2 + 0xe) == '\x05') {
    uVar4 = (undefined2)
            CONCAT31((uint3)((byte)((ushort)*(undefined2 *)(unaff_EBP + -0xa4) >> 8) & 0xfe | 2),
                     0x3f);
  }
  else {
    uVar4 = 0x133f;
  }
  *(undefined2 *)(unaff_EBP + -0xa2) = uVar4;
  fVar1 = (float10)0;
  *(int *)(unaff_EBP + -0x94) = param_2;
  *(ushort *)(unaff_EBP + -0xa0) =
       (ushort)NAN(in_ST0) << 8 | (ushort)(in_ST0 < fVar1) << 9 | (ushort)(in_ST0 != fVar1) << 10 |
       (ushort)(in_ST0 == fVar1) << 0xe;
  *(undefined1 *)(unaff_EBP + -0x90) = 0;
  fVar1 = (float10)0;
  *(ushort *)(unaff_EBP + -0xa0) =
       (ushort)NAN(in_ST1) << 8 | (ushort)(in_ST1 < fVar1) << 9 | (ushort)(in_ST1 != fVar1) << 10 |
       (ushort)(in_ST1 == fVar1) << 0xe;
  bVar3 = (char)(*(char *)(unaff_EBP + -0x9f) << 1) >> 1;
  cVar2 = (char)(*(char *)(unaff_EBP + -0x9f) << 1) >> 1;
                    /* WARNING: Could not recover jumptable at 0x10020721. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(param_2 + (char)((&DAT_100376ac)[(byte)(cVar2 << 1 | cVar2 < '\0') & 0xf] |
                               (&DAT_100376ac)[(byte)((bVar3 & 7) << 1 | (char)bVar3 < '\0')] << 2)
              + 0x10))();
  return;
}


// 10020800 FUN_10020800

float10 __fastcall
FUN_10020800(undefined4 param_1,int param_2,undefined2 param_3,undefined4 param_4,undefined4 param_5
            ,undefined4 param_6,undefined4 param_7,undefined4 param_8)

{
  float10 in_ST0;
  int local_24;
  undefined4 uStack_20;
  undefined4 uStack_1c;
  undefined4 uStack_18;
  undefined4 local_14;
  undefined4 local_10;
  double dStack_c;
  
  local_14 = param_7;
  local_10 = param_8;
  dStack_c = (double)in_ST0;
  uStack_1c = param_5;
  uStack_18 = param_6;
  uStack_20 = param_1;
  __87except(param_2,&local_24,&param_3);
  return (float10)dStack_c;
}


// 10020817 __startOneArgErrorHandling

/* Library Function - Single Match
    __startOneArgErrorHandling
   
   Library: Visual Studio 2005 Release */

float10 __fastcall
__startOneArgErrorHandling
          (undefined4 param_1,int param_2,ushort param_3,undefined4 param_4,undefined4 param_5,
          undefined4 param_6)

{
  float10 in_ST0;
  int local_24;
  undefined4 local_20;
  undefined4 local_1c;
  undefined4 local_18;
  double local_c;
  
  local_c = (double)in_ST0;
  local_1c = param_5;
  local_18 = param_6;
  local_20 = param_1;
  __87except(param_2,&local_24,&param_3);
  return (float10)local_c;
}


// 10020860 FUN_10020860

undefined1  [10] FUN_10020860(void)

{
  float10 in_ST0;
  float10 fVar1;
  undefined1 auVar2 [10];
  
  fVar1 = (float10)f2xm1(-(ROUND(in_ST0) - in_ST0));
  auVar2 = (undefined1  [10])fscale((float10)1 + fVar1,ROUND(in_ST0));
  return auVar2;
}


// 10020875 FUN_10020875

void FUN_10020875(void)

{
  return;
}


// 100208a5 __fload_withFB

/* Library Function - Single Match
    __fload_withFB
   
   Library: Visual Studio */

uint __fastcall __fload_withFB(undefined4 param_1,int param_2)

{
  uint uVar1;
  
  uVar1 = *(uint *)(param_2 + 4) & 0x7ff00000;
  if (uVar1 != 0x7ff00000) {
    return uVar1;
  }
  return *(uint *)(param_2 + 4);
}


// 100208fe FUN_100208fe

void FUN_100208fe(void)

{
  return;
}


// 10020949 FUN_10020949

void __fastcall
FUN_10020949(undefined4 param_1,int param_2,undefined4 param_3,undefined4 param_4,undefined4 param_5
            ,undefined4 param_6,undefined4 param_7)

{
  ushort in_FPUStatusWord;
  float10 in_ST0;
  ushort unaff_retaddr;
  uint uStack_4;
  
  uStack_4 = (uint)((ulonglong)(double)in_ST0 >> 0x20);
  if (((ulonglong)(double)in_ST0 & 0x7ff0000000000000) == 0) {
    fscale(in_ST0,(float10)1536.0);
  }
  else if ((uStack_4 & 0x7ff00000) == 0x7ff00000) {
    fscale(in_ST0,(float10)-1536.0);
  }
  else if (((unaff_retaddr == 0x27f) || ((unaff_retaddr & 0x20) != 0)) ||
          ((in_FPUStatusWord & 0x20) == 0)) {
    return;
  }
  if (param_2 == 0x1d) {
    FUN_10020800(param_1,0x1d,unaff_retaddr,param_3,param_4,param_5,param_6,param_7);
    return;
  }
  __startOneArgErrorHandling(param_1,param_2,unaff_retaddr,param_3,param_4,param_5);
  return;
}


// 100209f0 FUN_100209f0

void FUN_100209f0(void)

{
  float10 in_ST0;
  
  FUN_10020a0e((double)in_ST0);
  return;
}


// 10020a0e FUN_10020a0e

float10 __cdecl FUN_10020a0e(double param_1)

{
  double dVar1;
  double dVar2;
  undefined1 auVar3 [16];
  undefined1 auVar4 [12];
  uint uVar5;
  uint uVar6;
  int iVar7;
  int iVar8;
  double dVar9;
  undefined1 in_XMM0 [16];
  ulonglong in_XMM2_Qa;
  undefined1 auVar10 [16];
  undefined1 auVar11 [16];
  double dVar12;
  double dVar13;
  double dVar14;
  double dVar15;
  double dVar16;
  undefined1 auVar17 [16];
  undefined4 in_XMM7_Dc;
  undefined4 in_XMM7_Dd;
  double local_c;
  
  dVar9 = in_XMM0._0_8_;
  uVar6 = in_XMM0._4_4_ >> 0xc;
  uVar5 = uVar6 & 0x7ffff;
  if (uVar5 - 0x3fb00 < 0x3bb) {
    auVar10._0_8_ = (ulonglong)dVar9 & 0xffffc00000000000;
    auVar10._8_8_ = in_XMM2_Qa & CONCAT44(in_XMM7_Dd,in_XMM7_Dc);
    iVar7 = (uVar6 & 0xfffc) - 0xfb00;
    iVar8 = iVar7 * 4;
    dVar13 = SUB168(auVar10 | ZEXT816(0x200000000000),0);
    dVar12 = SQRT(1.0 - dVar9 * dVar9) * dVar13;
    dVar1 = dVar9 * *(double *)(&DAT_1002d5c0 + iVar7 * 2) - dVar12;
    uVar5 = SUB164(auVar10 | ZEXT816(0x200000000000),4) & 0x80000000;
    dVar2 = dVar1 * dVar1;
    auVar4._4_8_ = 0;
    auVar4._0_4_ = uVar5;
    auVar11._0_12_ = auVar4 << 0x20;
    auVar11._12_4_ = uVar5;
    return (float10)(((dVar9 - dVar13) * (dVar9 + dVar13)) /
                     (dVar9 * *(double *)(&DAT_1002d5c0 + iVar7 * 2) + dVar12) +
                     (dVar2 * 0.044642857142857144 + 0.075) * dVar1 * dVar2 * dVar2 +
                     dVar1 * dVar2 * 0.16666666666666666 +
                     (double)(*(ulonglong *)(&DAT_1002c6c0 + iVar8) ^ (ulonglong)uVar5 << 0x20) +
                    (double)(*(ulonglong *)(&UNK_1002c6c8 + iVar8) ^ auVar11._8_8_));
  }
  if (uVar5 - 0x3febb < 0x43) {
    dVar13 = SQRT(1.0 - dVar9 * dVar9);
    auVar3._8_4_ = in_XMM7_Dc;
    auVar3._0_8_ = dVar9;
    auVar3._12_4_ = in_XMM7_Dd;
    dVar2 = (double)((ulonglong)dVar9 & 0x7fffffc000000000);
    dVar1 = ABS(dVar9) - dVar2;
    dVar12 = (double)((ulonglong)dVar13 & 0xffffc00000000000 | 0x200000000000);
    iVar8 = (ushort)((ulonglong)((longlong)dVar13 << 2) >> 0x30) - 0xfec0;
    dVar13 = dVar13 * *(double *)(&DAT_1002d5c0 + iVar8 * 8);
    dVar14 = (dVar2 * dVar12 - dVar13) + dVar1 * dVar12;
    iVar8 = iVar8 * 0x10;
    dVar15 = dVar14 * dVar14;
    return (float10)(double)((ulonglong)
                             ((((dVar15 * 0.044642857142857144 + 0.075) * dVar14 * dVar15 * dVar15 +
                               dVar14 * dVar15 * 0.16666666666666666 +
                               (6.123233995736766e-17 - *(double *)(&DAT_1002c6c0 + iVar8))) -
                              (((1.0 - dVar2 * dVar2) - dVar12 * dVar12) -
                              (ABS(dVar9) + dVar2) * dVar1) / (dVar13 + dVar13 + dVar14)) +
                             (1.5707963267948966 - *(double *)(&UNK_1002c6c8 + iVar8))) |
                            (ulonglong)(ushort)((ushort)(SUB161(auVar3 >> 0x3f,0) & 1) << 0xf) <<
                            0x30);
  }
  if (uVar5 - 0x3c300 < 0x3800) {
    dVar2 = dVar9 * dVar9;
    dVar13 = dVar9 * dVar9;
    dVar1 = dVar9 * dVar2;
    return (float10)(dVar9 + dVar1 * dVar1 * dVar1 *
                             (dVar2 * 0.022372159090909092 + 0.030381944444444444 +
                             dVar2 * dVar2 * 0.017352764423076924) +
                             dVar9 * dVar13 *
                             (dVar13 * 0.075 + 0.16666666666666666 +
                             dVar13 * dVar13 * 0.044642857142857144));
  }
  if (uVar5 - 0x3fefe < 2) {
    dVar14 = SQRT(1.0 - dVar9 * dVar9);
    dVar1 = (double)((ulonglong)dVar9 & 0xfffffffff8000000);
    dVar15 = (double)((ulonglong)dVar14 & 0xfffffffff8000000);
    dVar9 = dVar9 - dVar1;
    auVar17._8_4_ = SUB84(dVar14,0);
    auVar17._0_8_ = dVar14;
    auVar17._12_4_ = (int)((ulonglong)dVar14 >> 0x20);
    dVar2 = dVar14 * dVar14;
    dVar13 = dVar14 * dVar14;
    dVar16 = dVar14 * dVar2;
    dVar12 = dVar16 * dVar16 * dVar16;
    return (float10)(double)((ulonglong)
                             (((((dVar2 * 0.011551800896136895 + 0.01396484375) *
                                 dVar16 * dVar16 * dVar12 + 6.123233995736766e-17) -
                               ((dVar2 * dVar2 * 0.017352764423076924 +
                                dVar2 * 0.022372159090909092 + 0.030381944444444444) * dVar12 +
                                (dVar13 * dVar13 * 0.044642857142857144 +
                                dVar13 * 0.075 + 0.16666666666666666) * auVar17._8_8_ * dVar13 +
                               (((((1.0 - dVar1 * dVar1) - (dVar1 + dVar1) * dVar9) -
                                 dVar15 * dVar15) - dVar9 * dVar9) +
                               (dVar14 + dVar14 + (dVar15 - dVar14)) * (dVar15 - dVar14)) /
                               (dVar14 + dVar14))) -
                              (dVar14 - ((dVar14 - 1.5707963267948966) + 1.5707963267948966))) -
                             (dVar14 - 1.5707963267948966)) |
                            (ulonglong)(in_XMM0._6_2_ & 0x8000) << 0x30);
  }
  if (uVar5 < 0x3ff00) {
    if (0x7fdf < ((ushort)((ulonglong)param_1 >> 0x30) & 0x7ff0) - 0x10) {
      param_1 = (double)((ulonglong)param_1 | (ulonglong)(param_1 + 0.0));
    }
    return (float10)param_1;
  }
  if ((in_XMM0._4_4_ & 0x7fffffff) != 0x3ff00000 || in_XMM0._0_4_ != 0) {
    if ((int)((((uint)((ulonglong)param_1 >> 0x20) & 0x7fffffff) + 0x80100000) -
             (uint)(SUB84(param_1,0) == 0)) < 0) {
      local_c = -NAN;
      iVar8 = 0x3d;
    }
    else {
      local_c = param_1 + 0.0;
      iVar8 = 0x3f1;
    }
    ___libm_error_support(&param_1,&param_1,&local_c,iVar8);
    return (float10)local_c;
  }
  return (float10)(double)((ulonglong)dVar9 & 0x8000000000000000 | 0x3ff921fb54442d18);
}


// 10020f70 ___libm_error_support

/* Library Function - Single Match
    ___libm_error_support
   
   Library: Visual Studio 2005 Release */

void __cdecl ___libm_error_support(double *param_1,undefined8 *param_2,double *param_3,int param_4)

{
  code *pcVar1;
  int iVar2;
  DWORD *pDVar3;
  undefined4 local_2c;
  char *local_28;
  double local_24;
  undefined8 local_1c;
  double local_14;
  undefined1 local_c;
  undefined1 uStack_b;
  undefined1 uStack_a;
  undefined1 uStack_9;
  undefined1 uStack_8;
  undefined1 uStack_7;
  undefined1 uStack_6;
  undefined1 uStack_5;
  
  local_c = 0;
  uStack_b = 0;
  uStack_a = 0;
  uStack_9 = 0;
  uStack_8 = 0;
  uStack_7 = 0;
  uStack_6 = 0;
  uStack_5 = 0;
  if (DAT_1004abb0 == 0) {
    pcVar1 = FUN_10023315;
  }
  else {
    pcVar1 = (code *)FUN_10013035(DAT_1004abb4);
  }
  if (param_4 < 0xa7) {
    if (param_4 == 0xa6) {
      local_2c = 3;
      local_28 = "exp10";
LAB_1002101f:
      local_24 = *param_1;
      local_1c = *param_2;
      local_14 = *param_3;
      iVar2 = (*pcVar1)(&local_2c);
      if (iVar2 == 0) {
        pDVar3 = FUN_10011e63();
        *pDVar3 = 0x22;
      }
    }
    else if (param_4 < 0x1a) {
      if (param_4 != 0x19) {
        local_2c = 2;
        if (param_4 == 2) {
          local_2c = 2;
          local_28 = "log";
        }
        else {
          if (param_4 == 3) {
            local_28 = "log";
            goto LAB_1002108d;
          }
          if (param_4 == 8) {
            local_28 = "log10";
          }
          else {
            if (param_4 == 9) {
              local_28 = "log10";
              goto LAB_1002108d;
            }
            if (param_4 != 0xe) {
              if (param_4 != 0xf) {
                if (param_4 != 0x18) {
                  return;
                }
                local_2c = 3;
                goto LAB_10021018;
              }
              local_28 = "exp";
              goto LAB_10021054;
            }
            local_2c = 3;
            local_28 = "exp";
          }
        }
        goto LAB_1002101f;
      }
      local_28 = "pow";
LAB_10021054:
      local_24 = *param_1;
      local_1c = *param_2;
      local_2c = 4;
      local_14 = *param_3;
      (*pcVar1)(&local_2c);
    }
    else {
      if (param_4 != 0x1a) {
        if (param_4 != 0x1b) {
          if (param_4 == 0x1c) goto switchD_1002114a_caseD_3ee;
          if (param_4 != 0x1d) {
            if (param_4 != 0x3a) {
              if (param_4 != 0x3d) {
                return;
              }
              goto switchD_1002114a_caseD_3f1;
            }
            goto switchD_1002114a_caseD_3f0;
          }
          local_28 = "pow";
          goto LAB_10021103;
        }
        local_2c = 2;
LAB_10021018:
        local_28 = "pow";
        goto LAB_1002101f;
      }
      local_14 = 1.0;
    }
    goto LAB_100211e3;
  }
  switch(param_4) {
  case 1000:
    local_28 = "log";
    break;
  case 0x3e9:
    local_28 = "log10";
    break;
  case 0x3ea:
    local_28 = "exp";
    break;
  case 0x3eb:
    local_28 = "atan";
    break;
  case 0x3ec:
    local_28 = "ceil";
    break;
  case 0x3ed:
    local_28 = "floor";
    break;
  case 0x3ee:
switchD_1002114a_caseD_3ee:
    local_28 = "pow";
    goto LAB_1002108d;
  case 0x3ef:
    local_28 = "modf";
    break;
  case 0x3f0:
switchD_1002114a_caseD_3f0:
    local_28 = "acos";
    goto LAB_1002108d;
  case 0x3f1:
switchD_1002114a_caseD_3f1:
    local_28 = "asin";
    goto LAB_1002108d;
  case 0x3f2:
    local_28 = "sin";
    goto LAB_100211af;
  case 0x3f3:
    local_28 = "cos";
    goto LAB_100211af;
  case 0x3f4:
    local_28 = "tan";
LAB_100211af:
    local_14 = *param_1 *
               (double)CONCAT17(uStack_5,CONCAT16(uStack_6,CONCAT15(uStack_7,CONCAT14(uStack_8,
                                                  CONCAT13(uStack_9,CONCAT12(uStack_a,CONCAT11(
                                                  uStack_b,local_c)))))));
    *param_3 = local_14;
    local_24 = *param_1;
    local_1c = *param_2;
    goto LAB_100211c0;
  default:
    goto switchD_1002114a_default;
  }
LAB_10021103:
  *param_3 = *param_1;
LAB_1002108d:
  local_24 = *param_1;
  local_1c = *param_2;
  local_14 = *param_3;
LAB_100211c0:
  local_2c = 1;
  iVar2 = (*pcVar1)(&local_2c);
  if (iVar2 == 0) {
    pDVar3 = FUN_10011e63();
    *pDVar3 = 0x21;
  }
LAB_100211e3:
  *param_3 = local_14;
switchD_1002114a_default:
  return;
}


// 10021220 FUN_10021220

float10 __cdecl FUN_10021220(double param_1)

{
  double dVar1;
  uint uVar2;
  int iVar3;
  float10 fVar4;
  uint uVar5;
  
  uVar2 = __ctrlfp();
  if ((param_1._6_2_ & 0x7ff0) == 0x7ff0) {
    iVar3 = __sptype(SUB84(param_1,0),(uint)((ulonglong)param_1 >> 0x20));
    if (0 < iVar3) {
      if (iVar3 < 3) {
        __ctrlfp();
        return (float10)param_1;
      }
      if (iVar3 == 3) {
        fVar4 = __handle_qnan1(0xb,param_1);
        return fVar4;
      }
    }
    dVar1 = param_1 + 1.0;
    uVar5 = 8;
  }
  else {
    fVar4 = __frnd(param_1);
    dVar1 = (double)fVar4;
    if ((param_1 == dVar1) || ((uVar2 & 0x20) != 0)) {
      __ctrlfp();
      return (float10)dVar1;
    }
    uVar5 = 0x10;
  }
  fVar4 = (float10)__except1(uVar5,0xb,param_1,dVar1,uVar2);
  return fVar4;
}


// 10021300 __cintrindisp2

/* Library Function - Single Match
    __cintrindisp2
   
   Library: Visual Studio */

void __fastcall __cintrindisp2(undefined4 param_1,int param_2)

{
  __trandisp2(param_1,param_2);
  FUN_100213ca();
  return;
}


// 1002133e __cintrindisp1

/* Library Function - Single Match
    __cintrindisp1
   
   Library: Visual Studio */

void __fastcall __cintrindisp1(undefined4 param_1,int param_2)

{
  __trandisp1(param_1,param_2);
  FUN_100213ca();
  return;
}


// 1002137b __ctrandisp2

/* Library Function - Single Match
    __ctrandisp2
   
   Libraries: Visual Studio 1998, Visual Studio 2003, Visual Studio 2005, Visual Studio 2008 */

void __cdecl __ctrandisp2(uint param_1,int param_2,uint param_3,int param_4)

{
  undefined4 extraout_ECX;
  int extraout_EDX;
  
  __fload(param_1,param_2);
  __fload(param_3,param_4);
  __trandisp2(extraout_ECX,extraout_EDX);
  FUN_100213c3();
  return;
}


// 100213c3 FUN_100213c3

void FUN_100213c3(void)

{
  char cVar1;
  ushort uVar2;
  int unaff_EBP;
  ushort in_FPUStatusWord;
  float10 in_ST0;
  
  *(byte *)(unaff_EBP + -0x2c8) = *(byte *)(unaff_EBP + -0x2c8) & 0xfe;
  if (DAT_10039e10 != 0) {
    return;
  }
  *(double *)(unaff_EBP + -0x2d0) = (double)in_ST0;
  cVar1 = *(char *)(unaff_EBP + -0x90);
  if (cVar1 != '\0') {
    if (cVar1 == -1) {
      uVar2 = *(ushort *)(unaff_EBP + -0x2ca) & 0x7ff0;
    }
    else {
      if (cVar1 != -2) {
        if (cVar1 == '\0') {
          return;
        }
        *(int *)(unaff_EBP + -0x8e) = (int)cVar1;
        goto LAB_100214af;
      }
      uVar2 = *(ushort *)(unaff_EBP + -0x2ca) & 0x7ff0;
      if (uVar2 == 0) {
        *(undefined4 *)(unaff_EBP + -0x8e) = 4;
        in_ST0 = (float10)fscale(in_ST0,(float10)1536.0);
        if (ABS(in_ST0) < (float10)2.2250738585072014e-308) {
          in_ST0 = in_ST0 * (float10)0.0;
        }
        goto LAB_100214af;
      }
    }
    if (uVar2 == 0x7ff0) {
      *(undefined4 *)(unaff_EBP + -0x8e) = 3;
      in_ST0 = (float10)fscale(in_ST0,(float10)-1536.0);
      if ((float10)1.7976931348623157e+308 < ABS(in_ST0)) {
        in_ST0 = in_ST0 * (float10)INFINITY;
      }
      goto LAB_100214af;
    }
  }
  if ((*(ushort *)(unaff_EBP + -0xa4) & 0x20) != 0) {
    return;
  }
  if ((in_FPUStatusWord & 0x20) == 0) {
    return;
  }
  *(undefined4 *)(unaff_EBP + -0x8e) = 8;
LAB_100214af:
  *(int *)(unaff_EBP + -0x8a) = *(int *)(unaff_EBP + -0x94) + 1;
  if ((*(byte *)(unaff_EBP + -0x2c8) & 1) == 0) {
    *(undefined4 *)(unaff_EBP + -0x86) = *(undefined4 *)(unaff_EBP + 8);
    *(undefined4 *)(unaff_EBP + -0x82) = *(undefined4 *)(unaff_EBP + 0xc);
    if (*(char *)(*(int *)(unaff_EBP + -0x94) + 0xd) != '\x01') {
      *(undefined4 *)(unaff_EBP + -0x7e) = *(undefined4 *)(unaff_EBP + 0x10);
      *(undefined4 *)(unaff_EBP + -0x7a) = *(undefined4 *)(unaff_EBP + 0x14);
    }
  }
  *(double *)(unaff_EBP + -0x76) = (double)in_ST0;
  __87except((int)*(char *)(*(int *)(unaff_EBP + -0x94) + 0xe),(int *)(unaff_EBP + -0x8e),
             (ushort *)(unaff_EBP + -0xa4));
  return;
}


// 100213ca FUN_100213ca

void FUN_100213ca(void)

{
  char cVar1;
  ushort uVar2;
  int unaff_EBP;
  ushort in_FPUStatusWord;
  float10 in_ST0;
  
  if (DAT_10039e10 != 0) {
    return;
  }
  *(double *)(unaff_EBP + -0x2d0) = (double)in_ST0;
  cVar1 = *(char *)(unaff_EBP + -0x90);
  if (cVar1 != '\0') {
    if (cVar1 == -1) {
      uVar2 = *(ushort *)(unaff_EBP + -0x2ca) & 0x7ff0;
    }
    else {
      if (cVar1 != -2) {
        if (cVar1 == '\0') {
          return;
        }
        *(int *)(unaff_EBP + -0x8e) = (int)cVar1;
        goto LAB_100214af;
      }
      uVar2 = *(ushort *)(unaff_EBP + -0x2ca) & 0x7ff0;
      if (uVar2 == 0) {
        *(undefined4 *)(unaff_EBP + -0x8e) = 4;
        in_ST0 = (float10)fscale(in_ST0,(float10)1536.0);
        if (ABS(in_ST0) < (float10)2.2250738585072014e-308) {
          in_ST0 = in_ST0 * (float10)0.0;
        }
        goto LAB_100214af;
      }
    }
    if (uVar2 == 0x7ff0) {
      *(undefined4 *)(unaff_EBP + -0x8e) = 3;
      in_ST0 = (float10)fscale(in_ST0,(float10)-1536.0);
      if ((float10)1.7976931348623157e+308 < ABS(in_ST0)) {
        in_ST0 = in_ST0 * (float10)INFINITY;
      }
      goto LAB_100214af;
    }
  }
  if ((*(ushort *)(unaff_EBP + -0xa4) & 0x20) != 0) {
    return;
  }
  if ((in_FPUStatusWord & 0x20) == 0) {
    return;
  }
  *(undefined4 *)(unaff_EBP + -0x8e) = 8;
LAB_100214af:
  *(int *)(unaff_EBP + -0x8a) = *(int *)(unaff_EBP + -0x94) + 1;
  if ((*(byte *)(unaff_EBP + -0x2c8) & 1) == 0) {
    *(undefined4 *)(unaff_EBP + -0x86) = *(undefined4 *)(unaff_EBP + 8);
    *(undefined4 *)(unaff_EBP + -0x82) = *(undefined4 *)(unaff_EBP + 0xc);
    if (*(char *)(*(int *)(unaff_EBP + -0x94) + 0xd) != '\x01') {
      *(undefined4 *)(unaff_EBP + -0x7e) = *(undefined4 *)(unaff_EBP + 0x10);
      *(undefined4 *)(unaff_EBP + -0x7a) = *(undefined4 *)(unaff_EBP + 0x14);
    }
  }
  *(double *)(unaff_EBP + -0x76) = (double)in_ST0;
  __87except((int)*(char *)(*(int *)(unaff_EBP + -0x94) + 0xe),(int *)(unaff_EBP + -0x8e),
             (ushort *)(unaff_EBP + -0xa4));
  return;
}


// 10021511 __ctrandisp1

/* Library Function - Single Match
    __ctrandisp1
   
   Library: Visual Studio */

void __cdecl __ctrandisp1(uint param_1,int param_2)

{
  undefined4 extraout_ECX;
  int extraout_EDX;
  
  __fload(param_1,param_2);
  __trandisp1(extraout_ECX,extraout_EDX);
  FUN_100213c3();
  return;
}


// 10021544 __fload

/* Library Function - Single Match
    __fload
   
   Libraries: Visual Studio 1998, Visual Studio 2003, Visual Studio 2005, Visual Studio 2008 */

float10 __cdecl __fload(uint param_1,int param_2)

{
  float10 fVar1;
  
  if ((param_2._2_2_ & 0x7ff0) == 0x7ff0) {
    fVar1 = (float10)CONCAT28(param_2._2_2_ | 0x7fff,
                              CONCAT44(param_2 << 0xb | param_1 >> 0x15,param_1));
  }
  else {
    fVar1 = (float10)(double)CONCAT26(param_2._2_2_,CONCAT24((undefined2)param_2,param_1));
  }
  return fVar1;
}


// 10021d86 FUN_10021d86

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __cdecl FUN_10021d86(undefined4 param_1,uint param_2,ushort param_3)

{
  undefined4 in_EAX;
  int iVar1;
  float10 fVar2;
  float10 fVar3;
  undefined4 in_stack_0000001c;
  undefined2 in_stack_00000020;
  undefined2 in_stack_00000022;
  ushort in_stack_00000024;
  
  if ((((((CONCAT22(param_3,param_2._2_2_) ^ 0x700) & 0x700) == 0) &&
       ((&DAT_100376ec)[(param_2._2_2_ & 0x7800) >> 0xb] != '\0')) && ((param_3 & 0x7fff) != 0x7fff)
      ) && ((((in_stack_00000024 & 0x7fff) != 0 && ((in_stack_00000024 & 0x7fff) != 0x7fff)) &&
            (((CONCAT22(in_stack_00000022,in_stack_00000020) & 0x7fffffff) == 0 &&
             ((param_2 & 0x7fffffff) == 0)))))) {
    if ((ushort)((param_3 & 0x7fff) + 0x3f) < (in_stack_00000024 & 0x7fff)) {
      iVar1 = ((in_stack_00000024 & 0x7fff) - (param_3 & 0x7fff) & 0x3f | 0x20) + 1;
      fVar3 = ABS((float10)CONCAT28(in_stack_00000024 & 0x7fff | param_3 & 0x8000,
                                    CONCAT26(param_2._2_2_,CONCAT24((undefined2)param_2,param_1))));
      fVar2 = ABS((float10)CONCAT28(in_stack_00000024,
                                    CONCAT26(in_stack_00000022,
                                             CONCAT24(in_stack_00000020,in_stack_0000001c))));
      do {
        if (fVar3 <= fVar2) {
          fVar2 = fVar2 - fVar3;
        }
        fVar3 = fVar3 * (float10)_DAT_1003771c;
        iVar1 = iVar1 + -1;
      } while (iVar1 != 0);
    }
    else {
      while (-1 < (int)((in_stack_00000024 & 0x7fff) - ((param_3 & 0x7fff) + 10))) {
        fVar2 = (float10)CONCAT28(in_stack_00000024,
                                  CONCAT26(in_stack_00000022,
                                           CONCAT24(in_stack_00000020,in_stack_0000001c)));
        fVar3 = (float10)CONCAT28((in_stack_00000024 & 0x7fff) -
                                  ((in_stack_00000024 & 0x7fff) - param_3 & 7 | 4) |
                                  param_3 & 0x8000,
                                  CONCAT26(param_2._2_2_,CONCAT24((undefined2)param_2,param_1)));
        fVar2 = fVar2 - (fVar2 / fVar3) * fVar3;
        in_stack_0000001c = SUB104(fVar2,0);
        in_stack_00000020 = (undefined2)((unkuint10)fVar2 >> 0x20);
        in_stack_00000022 = (undefined2)((unkuint10)fVar2 >> 0x30);
        in_stack_00000024 = (ushort)((unkuint10)fVar2 >> 0x40);
      }
    }
  }
  return in_EAX;
}


// 10021f8c FUN_10021f8c

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 FUN_10021f8c(void)

{
  float10 in_ST0;
  float10 extraout_ST0;
  float10 extraout_ST0_00;
  float10 in_ST1;
  uint uVar2;
  float10 fVar1;
  
  uVar2 = (uint)((unkuint10)in_ST1 >> 0x20);
  if (((unkuint10)in_ST1 & 0) != 0) {
    FUN_10021d86(SUB104(in_ST1,0),uVar2,(ushort)((unkuint10)in_ST1 >> 0x40));
    return extraout_ST0;
  }
  if (SUB104(in_ST1,0) != 0 || uVar2 != 0) {
    fVar1 = in_ST1 * (float10)_DAT_10037704;
    FUN_10021d86(SUB104(fVar1,0),(uint)((unkuint10)fVar1 >> 0x20),(ushort)((unkuint10)fVar1 >> 0x40)
                );
    return extraout_ST0_00;
  }
  return in_ST0 - (in_ST0 / in_ST1) * in_ST1;
}


// 10022310 FUN_10022310

void FUN_10022310(void)

{
  float10 in_ST0;
  float10 in_ST1;
  
  FUN_10022329((double)in_ST1,SUB84((double)in_ST0,0),(uint)((ulonglong)(double)in_ST0 >> 0x20));
  return;
}


// 10022329 FUN_10022329

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 __cdecl FUN_10022329(double param_1,int param_2,uint param_3)

{
  byte bVar1;
  uint uVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  uint uVar6;
  uint uVar7;
  uint uVar8;
  ushort uVar9;
  double dVar10;
  double dVar11;
  undefined8 in_XMM1_Qa;
  double dVar12;
  ushort uVar18;
  undefined1 auVar13 [16];
  undefined1 auVar14 [16];
  undefined1 auVar15 [16];
  undefined1 auVar16 [16];
  undefined1 auVar17 [16];
  ulonglong in_XMM2_Qa;
  longlong lVar19;
  longlong lVar23;
  undefined1 auVar20 [16];
  undefined1 auVar21 [16];
  undefined1 auVar22 [16];
  ulonglong in_XMM3_Qa;
  ulonglong uVar24;
  ulonglong uVar25;
  double dVar26;
  double dVar27;
  double dVar28;
  double dVar29;
  double dVar30;
  double dVar31;
  double dVar32;
  double dVar33;
  
  dVar10 = (double)((ulonglong)param_1 >> 0x2c);
  uVar9 = (ushort)((ulonglong)param_1 >> 0x30);
  uVar2 = (SUB82(dVar10,0) & 0xff) + 1 & 0x1fe;
  dVar32 = (double)((ulonglong)param_1 & 0xfffffffffffff | 0x3ff0000000000000) *
           *(double *)((int)&DAT_1002df80 + uVar2 * 4);
  dVar28 = *(double *)((int)&DAT_1002df80 + uVar2 * 4);
  dVar11 = *(double *)(&DAT_1002e390 + uVar2 * 8);
  dVar30 = *(double *)(&UNK_1002e398 + uVar2 * 8);
  uVar6 = 0x7fef - uVar9;
  uVar2 = SUB84(param_1,0);
  uVar8 = (uint)((ulonglong)param_1 >> 0x20);
  dVar29 = param_1;
  if (0x7fffffff < (uVar9 - 0x10 | uVar6)) {
    uVar7 = param_3 & 0x7fffffff;
    uVar24 = in_XMM3_Qa >> 0x30;
    if (uVar7 < 0x7ff00000) {
      if (param_2 == 0 && uVar7 == 0) {
        dVar28 = 1.0;
        iVar5 = 0x1a;
        if (uVar2 != 0 || ((ulonglong)param_1 & 0x7fffffff00000000) != 0) {
          iVar5 = 0x1d;
          if (((uVar8 & 0x7fffffff) < 0x7ff00001) &&
             (((uVar8 & 0x7fffffff) < 0x7ff00000 || (uVar2 == 0)))) {
            return (float10)1.0;
          }
        }
        goto LAB_10022b30;
      }
      if ((int)uVar6 < 0) {
        uVar25 = in_XMM3_Qa << 0x34;
        iVar5 = (uVar7 >> 0x14) - 0x3f3;
        in_XMM3_Qa = 0;
        uVar18 = (ushort)((ushort)((ulonglong)in_XMM1_Qa >> 0x30) & (ushort)uVar24) >> 4;
        lVar19 = (CONCAT44(param_3,param_2) | 0xfff0000000000000) <<
                 (ulonglong)
                 CONCAT22((ushort)(-1 < iVar5) * (short)((uint)iVar5 >> 0x10),
                          (ushort)(-1 < (short)iVar5) * (short)iVar5);
        lVar23 = (in_XMM2_Qa | uVar25) << (ulonglong)(ushort)((-1 < (short)uVar18) * uVar18);
        auVar20._0_4_ = -(uint)((int)lVar19 == 0);
        auVar20._4_4_ = -(uint)((int)((ulonglong)lVar19 >> 0x20) == 0);
        auVar20._8_4_ = -(uint)((int)lVar23 == 0);
        auVar20._12_4_ = -(uint)((int)((ulonglong)lVar23 >> 0x20) == 0);
        bVar1 = SUB161(auVar20 >> 7,0) & 1 | (SUB161(auVar20 >> 0xf,0) & 1) << 1 |
                (SUB161(auVar20 >> 0x17,0) & 1) << 2 | (SUB161(auVar20 >> 0x1f,0) & 1) << 3 |
                (SUB161(auVar20 >> 0x27,0) & 1) << 4 | (SUB161(auVar20 >> 0x2f,0) & 1) << 5 |
                (SUB161(auVar20 >> 0x37,0) & 1) << 6 | SUB161(auVar20 >> 0x3f,0) << 7;
        uVar7 = 0x7fef - uVar6 & 0x7fff;
        if (0x7fef < uVar7) {
          auVar14._0_4_ = -(uint)((int)((ulonglong)param_1 & 0xfffffffffffff) == 0);
          auVar14._4_4_ = -(uint)((int)(((ulonglong)param_1 & 0xfffffffffffff) >> 0x20) == 0);
          auVar14._8_4_ = 0xffffffff;
          auVar14._12_4_ = 0xffffffff;
          if ((byte)(SUB161(auVar14 >> 7,0) & 1 | (SUB161(auVar14 >> 0xf,0) & 1) << 1 |
                     (SUB161(auVar14 >> 0x17,0) & 1) << 2 | (SUB161(auVar14 >> 0x1f,0) & 1) << 3 |
                     (SUB161(auVar14 >> 0x27,0) & 1) << 4 | (SUB161(auVar14 >> 0x2f,0) & 1) << 5 |
                     (SUB161(auVar14 >> 0x37,0) & 1) << 6 | SUB161(auVar14 >> 0x3f,0) << 7) == 0xff)
          {
            if (((ulonglong)param_1 & 0x8000000000000000) != 0) {
              if ((bVar1 != 0xff) ||
                 (lVar19 = CONCAT44(param_3,param_2) <<
                           (ulonglong)(((param_3 & 0x7fffffff) >> 0x14) - 0x3f4),
                 auVar22._0_4_ = -(uint)((int)lVar19 == 0),
                 auVar22._4_4_ = -(uint)((int)((ulonglong)lVar19 >> 0x20) == 0),
                 auVar22._8_4_ = 0xffffffff, auVar22._12_4_ = 0xffffffff,
                 (byte)(SUB161(auVar22 >> 7,0) & 1 | (SUB161(auVar22 >> 0xf,0) & 1) << 1 |
                        (SUB161(auVar22 >> 0x17,0) & 1) << 2 | (SUB161(auVar22 >> 0x1f,0) & 1) << 3
                        | (SUB161(auVar22 >> 0x27,0) & 1) << 4 |
                        (SUB161(auVar22 >> 0x2f,0) & 1) << 5 | (SUB161(auVar22 >> 0x37,0) & 1) << 6
                       | SUB161(auVar22 >> 0x3f,0) << 7) == 0xff)) {
                if ((param_3 & 0x80000000) != 0) {
                  return (float10)0;
                }
                goto LAB_10022ab7;
              }
              if ((param_3 & 0x80000000) == 0) {
                return (float10)-INFINITY;
              }
              goto LAB_10022949;
            }
            if ((param_3 & 0x80000000) != 0) {
              return (float10)0;
            }
            goto LAB_10022ab7;
          }
          goto LAB_1002298f;
        }
        if (bVar1 == 0xff) {
          in_XMM3_Qa = 0;
          lVar19 = CONCAT44(param_3,param_2) <<
                   (ulonglong)(((param_3 & 0x7fffffff) >> 0x14) - 0x3f4);
          auVar21._0_4_ = -(uint)((int)lVar19 == 0);
          auVar21._4_4_ = -(uint)((int)((ulonglong)lVar19 >> 0x20) == -0x80000000);
          auVar21._8_4_ = -(uint)(auVar20._8_4_ == 0);
          auVar21._12_4_ = -(uint)(auVar20._12_4_ == 0);
          in_XMM2_Qa = auVar21._8_8_;
          uVar6 = (ushort)((ushort)(SUB161(auVar21 >> 7,0) & 1) |
                           (ushort)(SUB161(auVar21 >> 0xf,0) & 1) << 1 |
                           (ushort)(SUB161(auVar21 >> 0x17,0) & 1) << 2 |
                           (ushort)(SUB161(auVar21 >> 0x1f,0) & 1) << 3 |
                           (ushort)(SUB161(auVar21 >> 0x27,0) & 1) << 4 |
                           (ushort)(SUB161(auVar21 >> 0x2f,0) & 1) << 5 |
                           (ushort)(SUB161(auVar21 >> 0x37,0) & 1) << 6 |
                          (ushort)(SUB161(auVar21 >> 0x3f,0) & 1) << 7) + 0x3ff01 & 0x40000;
          if (0xf < uVar7) {
            uVar7 = 0xbfe7f;
            in_XMM3_Qa = 0;
            goto LAB_100223a6;
          }
          goto LAB_100227c4;
        }
        uVar24 = (ulonglong)param_1 >> 0x20;
        in_XMM2_Qa = auVar20._8_8_ >> 0x20;
        uVar7 = uVar2 | uVar8 & 0x7fffffff;
        uVar6 = 0;
        if (uVar7 != 0) {
          dVar28 = -NAN;
          iVar5 = 0x1c;
          goto LAB_10022b30;
        }
LAB_1002284a:
        dVar29 = dVar10;
        if ((uVar7 & 0x7fffffff) == 0) {
          if ((param_3 & 0x80000000) == 0) {
            if ((uVar7 & uVar6 << 0xd) == 0) {
              return (float10)0;
            }
LAB_10022949:
            return (float10)-0.0;
          }
          dVar28 = (double)((ulonglong)(uVar7 & uVar6 << 0xd | 0x7ff00000) << 0x20);
          iVar5 = 0x1b;
          goto LAB_10022b30;
        }
      }
      else {
        uVar6 = 0;
LAB_100227c4:
        dVar32 = 2.225073858507201e-308;
        uVar24 = 0x3ff0000000000000;
        dVar10 = param_1 * 1.8446744073709552e+19;
        uVar7 = uVar8;
        dVar29 = dVar10;
        if (uVar2 == 0) goto LAB_1002284a;
      }
      dVar10 = (double)((ulonglong)ABS(dVar29) >> 0x2c);
      uVar7 = (SUB82(dVar10,0) & 0xff) + 1 & 0x1fe;
      dVar32 = (double)((ulonglong)dVar32 & (ulonglong)dVar29 | uVar24) *
               *(double *)((int)&DAT_1002df80 + uVar7 * 4);
      dVar28 = *(double *)((int)&DAT_1002df80 + uVar7 * 4);
      dVar11 = *(double *)(&DAT_1002e390 + uVar7 * 8);
      dVar30 = *(double *)(&UNK_1002e398 + uVar7 * 8);
      uVar7 = 0x43e7f;
      goto LAB_100223a6;
    }
    dVar28 = param_1;
    if ((0x7fefffff < (uVar8 & 0x7fffffff)) && ((0x7ff00000 < (uVar8 & 0x7fffffff) || (uVar2 != 0)))
       ) {
LAB_1002298f:
      dVar28 = param_1 + param_1;
      iVar5 = 0x3ee;
      goto LAB_10022b30;
    }
    goto LAB_10022a45;
  }
  uVar6 = 0;
  uVar7 = 0x3fe7f;
LAB_100223a6:
  uVar3 = ((ushort)((ulonglong)dVar32 >> 0x26) & 0xff) + 1 & 0x1fe;
  dVar26 = (double)((ulonglong)dVar29 & 0xfffffffffffff | DAT_10031820);
  dVar12 = (double)((ulonglong)dVar26 & 0xfffffffff8000000);
  dVar26 = dVar26 - dVar12;
  uVar4 = ((ushort)((ulonglong)(dVar32 * *(double *)(&DAT_1002eba0 + uVar3 * 4)) >> 0x1f) & 0x1ff) +
          1 & 0x3fe;
  dVar27 = dVar28 * *(double *)(&DAT_1002eba0 + uVar3 * 4) * *(double *)(&DAT_1002f7c0 + uVar4 * 4);
  dVar32 = dVar32 * *(double *)(&DAT_1002eba0 + uVar3 * 4) * *(double *)(&DAT_1002f7c0 + uVar4 * 4);
  dVar29 = dVar11 + *(double *)(&DAT_1002efb0 + uVar3 * 8) +
           (double)(int)((longlong)dVar10 - (ulonglong)uVar7 >> 8) +
           *(double *)(&DAT_1002ffd0 + uVar4 * 8);
  dVar31 = dVar30 + *(double *)(&DAT_1002efb8 + uVar3 * 8) + *(double *)(&DAT_1002ffd8 + uVar4 * 8);
  dVar11 = (double)((ulonglong)dVar27 & 0xfffffffff8000000);
  dVar27 = dVar27 - dVar11;
  dVar33 = dVar32 + -1.442694902420044;
  dVar30 = dVar29 + dVar33;
  dVar28 = (double)CONCAT44(param_3,param_2);
  uVar18 = (ushort)(param_3 >> 0x10);
  dVar29 = dVar29 - dVar30;
  uVar3 = (uint)(ushort)((ulonglong)dVar30 >> 0x30);
  dVar11 = (((dVar32 - dVar11 * dVar12) - dVar12 * dVar27) - dVar11 * dVar26) - dVar26 * dVar27;
  dVar32 = dVar33 - dVar11;
  dVar10 = dVar30 - dVar11;
  uVar7 = uVar18 & 0x7ff0;
  if (0x7fef < uVar7) {
    uVar24 = CONCAT44(param_3,param_2) & 0xfffffffffffff;
    in_XMM3_Qa = in_XMM3_Qa >> 0x1f & in_XMM2_Qa;
    auVar15._0_4_ = -(uint)((int)uVar24 == 0);
    auVar15._4_4_ = -(uint)((int)(uVar24 >> 0x20) == 0);
    auVar15._8_4_ = -(uint)((int)in_XMM3_Qa == 0);
    auVar15._12_4_ = -(uint)((int)(in_XMM3_Qa >> 0x20) == 0);
    if ((byte)(SUB161(auVar15 >> 7,0) & 1 | (SUB161(auVar15 >> 0xf,0) & 1) << 1 |
               (SUB161(auVar15 >> 0x17,0) & 1) << 2 | (SUB161(auVar15 >> 0x1f,0) & 1) << 3 |
               (SUB161(auVar15 >> 0x27,0) & 1) << 4 | (SUB161(auVar15 >> 0x2f,0) & 1) << 5 |
               (SUB161(auVar15 >> 0x37,0) & 1) << 6 | SUB161(auVar15 >> 0x3f,0) << 7) == 0xff) {
      dVar28 = (double)CONCAT44(uVar6,uVar3);
      if (uVar2 == 0) {
        if (uVar8 != 0x3ff00000) {
          dVar28 = (double)CONCAT44(uVar6,uVar8);
          if (uVar8 == 0xbff00000) {
            return (float10)1;
          }
          goto LAB_10022a45;
        }
      }
      else {
LAB_10022a45:
        uVar24 = CONCAT44(param_3,param_2) & 0xfffffffffffff;
        auVar16._0_4_ = -(uint)((int)uVar24 == 0);
        auVar16._4_4_ = -(uint)((int)(uVar24 >> 0x20) == 0);
        auVar16._8_4_ = -(uint)((int)(in_XMM3_Qa & in_XMM2_Qa) == 0);
        auVar16._12_4_ = -(uint)((int)((in_XMM3_Qa & in_XMM2_Qa) >> 0x20) == 0);
        if ((byte)(SUB161(auVar16 >> 7,0) & 1 | (SUB161(auVar16 >> 0xf,0) & 1) << 1 |
                   (SUB161(auVar16 >> 0x17,0) & 1) << 2 | (SUB161(auVar16 >> 0x1f,0) & 1) << 3 |
                   (SUB161(auVar16 >> 0x27,0) & 1) << 4 | (SUB161(auVar16 >> 0x2f,0) & 1) << 5 |
                   (SUB161(auVar16 >> 0x37,0) & 1) << 6 | SUB161(auVar16 >> 0x3f,0) << 7) != 0xff)
        goto LAB_10022abe;
        if (dVar28 != -1.0) {
          if ((param_3 & 0x80000000) == 0) {
            if ((uVar9 & 0x7ff0) < 0x3ff0) {
              return (float10)0;
            }
          }
          else if (0x3fef < (uVar9 & 0x7ff0)) {
            return (float10)0;
          }
LAB_10022ab7:
          return (float10)INFINITY;
        }
      }
      dVar28 = 1.0;
      iVar5 = 0x1c;
    }
    else {
LAB_10022abe:
      dVar28 = (double)CONCAT44(param_3,param_2) + (double)CONCAT44(param_3,param_2);
      iVar5 = 0x3ee;
    }
    goto LAB_10022b30;
  }
  iVar5 = (uVar7 - 0x3ff0) + (uVar3 & 0x7ff0);
  if ((0x40a0U - iVar5 | iVar5 - 0x3c70U) < 0x80000000) {
LAB_100224f8:
    dVar12 = (double)((ulonglong)dVar28 & 0xfffffffff8000000);
    dVar26 = (double)((ulonglong)dVar10 & 0xfffffffff8000000);
    dVar11 = (dVar29 + dVar33) - (dVar11 - (dVar30 - dVar10));
    dVar30 = dVar12 * dVar26 * 128.0;
    uVar2 = (uint)ROUND(dVar30);
    dVar28 = dVar12 * (dVar10 - dVar26) + dVar26 * (dVar28 - dVar12) +
             (dVar10 - dVar26) * (dVar28 - dVar12);
    if (0 < (int)(0x1ff7f - uVar2 | uVar2 + 0x1e1ff)) {
      iVar5 = (uVar2 & 0x7f) * 0x10;
      auVar13._0_8_ = (double)((ulonglong)((uVar6 + uVar2 & 0xffffff80) + 0x1ff80) << 0x2d);
      auVar13._8_4_ = 0;
      auVar13._12_4_ = (int)((ulonglong)auVar13._0_8_ >> 0x20);
      dVar29 = *(double *)(&DAT_10031000 + iVar5) * auVar13._0_8_;
      dVar28 = (dVar30 - ((dVar30 + 6755399441055744.0) - 6755399441055744.0)) * 0.0078125 +
               dVar28 + ((dVar32 * 0.16015105075297303 + dVar32 * dVar32 * -0.08325619496072671) *
                         dVar32 * dVar32 +
                         dVar32 * 9.597935033233511e-08 + dVar32 * dVar32 * -0.3465736568077919 +
                        dVar31 + dVar11) * (double)CONCAT44(param_3,param_2);
      return (float10)(dVar28 * dVar28 * dVar29 *
                       (dVar28 * 0.0013333558146428443 + 0.009618129107628477) * dVar28 * dVar28 +
                       *(double *)(&UNK_10031008 + iVar5) * auVar13._8_8_ +
                       (dVar28 * 0.055504108664821576 + 0.2402265069591007) * dVar28 * dVar28 *
                       dVar29 + dVar28 * 0.6931471805599453 * dVar29 + dVar29);
    }
    if ((int)uVar2 < 1) {
      if ((int)uVar2 < -0x3fdff) {
LAB_10022d89:
        dVar28 = (double)((ulonglong)uVar6 << 0x2d);
        iVar5 = 0x19;
        goto LAB_10022b30;
      }
      uVar6 = uVar6 + 0x80;
      uVar8 = (uVar2 & 0xffffff80) + 0x3fe80;
      uVar9 = 0;
    }
    else {
      if (0x3ffff < uVar2) goto LAB_10022dac;
      uVar6 = uVar6 + 0x3ff00;
      uVar8 = uVar2 - 0x80 & 0xffffff80;
      uVar9 = 0x3ff0;
    }
    iVar5 = (uVar2 & 0x7f) * 0x10;
    uVar2 = ((int)-(uVar8 - 0x1ff80) >> 7) + 2;
    auVar17._0_8_ = (double)((ulonglong)uVar8 << 0x2d);
    auVar17._8_4_ = 0;
    auVar17._12_4_ = (int)((ulonglong)auVar17._0_8_ >> 0x20);
    dVar10 = *(double *)(&DAT_10031000 + iVar5) * auVar17._0_8_;
    dVar11 = (dVar30 - ((dVar30 + 6755399441055744.0) - 6755399441055744.0)) * 0.0078125 +
             dVar28 + ((dVar32 * 0.16015105075297303 + dVar32 * dVar32 * -0.08325619496072671) *
                       dVar32 * dVar32 +
                       dVar32 * 9.597935033233511e-08 + dVar32 * dVar32 * -0.3465736568077919 +
                      dVar31 + dVar11) * (double)CONCAT44(param_3,param_2);
    dVar28 = (double)((ulonglong)uVar6 << 0x2d);
    uVar24 = (ulonglong)(uVar2 + (uVar2 & 0x20));
    dVar30 = (double)(-1L << uVar24 & (ulonglong)dVar10);
    dVar11 = dVar11 * dVar11 * dVar10 *
             (dVar11 * 0.0013333558146428443 + 0.009618129107628477) * dVar11 * dVar11 +
             *(double *)(&UNK_10031008 + iVar5) * auVar17._8_8_ +
             (dVar11 * 0.055504108664821576 + 0.2402265069591007) * dVar11 * dVar11 * dVar10 +
             dVar11 * 0.6931471805599453 * dVar10;
    dVar29 = (double)((ulonglong)(dVar30 + dVar11) & -1L << uVar24);
    dVar11 = dVar11 + (dVar30 - dVar29) + (dVar10 - dVar30);
    if ((int)(uVar8 - 0x1ff80) < 1) {
      dVar28 = dVar11 * dVar28 + dVar29 * dVar28;
      dVar28 = dVar28 + (double)((ulonglong)uVar9 << 0x30) * dVar28;
      iVar5 = 0x18;
      if ((((ushort)((ulonglong)dVar28 >> 0x30) & 0x7ff0) != 0x7ff0) &&
         (iVar5 = 0x19, ((ulonglong)dVar28 & 0x7ff0000000000000) != 0)) {
        return (float10)dVar28;
      }
    }
    else {
      dVar28 = (dVar11 + dVar29) * dVar28;
      dVar28 = dVar28 + (double)((ulonglong)uVar9 << 0x30) * dVar28;
      iVar5 = 0x18;
      if ((((ushort)((ulonglong)dVar28 >> 0x30) & 0x7ff0) != 0x7ff0) &&
         (iVar5 = 0x19, ((ulonglong)dVar28 & 0x7ff0000000000000) != 0)) {
        return (float10)dVar28;
      }
    }
  }
  else {
    dVar30 = dVar28 * dVar10;
    uVar2 = (ushort)((ulonglong)dVar30 >> 0x30) & 0x7ff0;
    uVar8 = uVar2 - 0x3c70;
    if ((0x40a0 - uVar2 | uVar8) < 0x80000000) goto LAB_100224f8;
    if (0x7fffffff < uVar8) {
      return (float10)(double)((ulonglong)(uVar6 | 0x1ff80) << 0x2d);
    }
    if (((uVar18 ^ (uVar9 & 0x7ff0) + 0xc010) & 0x8000) != 0) goto LAB_10022d89;
LAB_10022dac:
    iVar5 = 0x18;
    if (uVar6 == 0) {
      dVar28 = INFINITY;
    }
    else {
      dVar28 = -INFINITY;
    }
  }
LAB_10022b30:
  ___libm_error_support(&param_1,(undefined8 *)&param_2,(double *)&stack0xfffffff4,iVar5);
  return (float10)dVar28;
}


// 10023077 __d_inttype

/* Library Function - Single Match
    __d_inttype
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __d_inttype(double param_1)

{
  uint uVar1;
  float10 fVar2;
  
  uVar1 = __fpclass(param_1);
  if ((uVar1 & 0x90) == 0) {
    fVar2 = __frnd(param_1);
    if (fVar2 == (float10)param_1) {
      fVar2 = __frnd(param_1 * 0.5);
      if (fVar2 == (float10)(param_1 * 0.5)) {
        return 2;
      }
      return 1;
    }
  }
  return 0;
}


// 100230db FUN_100230db

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __cdecl FUN_100230db(int param_1,int param_2,int param_3,int param_4,double *param_5)

{
  double dVar1;
  double dVar2;
  double dVar3;
  int iVar4;
  
  dVar2 = 0.0;
  dVar1 = (double)CONCAT44(param_2,param_1);
  if ((double)CONCAT44(param_2,param_1) < 0.0) {
    dVar1 = -dVar1;
  }
  dVar3 = _DAT_10037978;
  if (param_4 == 0x7ff00000) {
    if (param_3 == 0) {
      if ((dVar1 <= 1.0) && (dVar3 = 1.0, dVar1 < 1.0)) {
        dVar3 = dVar2;
      }
      goto LAB_10023208;
    }
  }
  else if ((param_4 == -0x100000) && (param_3 == 0)) {
    dVar3 = dVar2;
    if ((dVar1 <= 1.0) && (dVar3 = _DAT_10037978, 1.0 <= dVar1)) {
      *param_5 = _DAT_10037980;
      return 1;
    }
    goto LAB_10023208;
  }
  if (param_2 == 0x7ff00000) {
    if (param_1 != 0) {
      return 0;
    }
    if (((double)CONCAT44(param_4,param_3) <= 0.0) &&
       (dVar3 = dVar2, 0.0 <= (double)CONCAT44(param_4,param_3))) {
      dVar3 = 1.0;
    }
  }
  else {
    if (param_2 != -0x100000) {
      return 0;
    }
    if (param_1 != 0) {
      return 0;
    }
    iVar4 = __d_inttype((double)CONCAT44(param_4,param_3));
    if ((double)CONCAT44(param_4,param_3) <= 0.0) {
      if (0.0 <= (double)CONCAT44(param_4,param_3)) {
        dVar3 = 1.0;
      }
      else {
        dVar3 = 0.0;
        if (iVar4 == 1) {
          dVar3 = _DAT_10037998;
        }
      }
    }
    else {
      dVar3 = _DAT_10037978;
      if (iVar4 == 1) {
        dVar3 = -_DAT_10037978;
      }
    }
  }
LAB_10023208:
  *param_5 = dVar3;
  return 0;
}


// 10023219 __87except

/* Library Function - Single Match
    __87except
   
   Library: Visual Studio 2005 Release */

void __cdecl __87except(int param_1,int *param_2,ushort *param_3)

{
  bool bVar1;
  undefined3 extraout_var;
  int iVar2;
  uint uVar3;
  uint local_94;
  uint local_90 [12];
  undefined8 local_60;
  uint local_50;
  uint local_14;
  
  local_14 = DAT_100366b8 ^ (uint)&local_94;
  local_94 = (uint)*param_3;
  iVar2 = *param_2;
  if (iVar2 == 1) {
LAB_10023273:
    uVar3 = 8;
LAB_10023275:
    bVar1 = __handle_exc(uVar3,(double *)(param_2 + 6),local_94);
    if (CONCAT31(extraout_var,bVar1) == 0) {
      if (((param_1 == 0x10) || (param_1 == 0x16)) || (param_1 == 0x1d)) {
        local_60 = *(undefined8 *)(param_2 + 4);
        local_50 = local_50 & 0xffffffe3 | 3;
      }
      else {
        local_50 = local_50 & 0xfffffffe;
      }
      __raise_exc(local_90,&local_94,uVar3,param_1,(uint *)(param_2 + 2),(uint *)(param_2 + 6));
    }
  }
  else {
    if (iVar2 == 2) {
      uVar3 = 4;
      goto LAB_10023275;
    }
    if (iVar2 == 3) {
      uVar3 = 0x11;
      goto LAB_10023275;
    }
    if (iVar2 == 4) {
      uVar3 = 0x12;
      goto LAB_10023275;
    }
    if (iVar2 == 5) goto LAB_10023273;
    if (iVar2 == 7) {
      *param_2 = 1;
    }
    else if (iVar2 == 8) {
      uVar3 = 0x10;
      goto LAB_10023275;
    }
  }
  __ctrlfp();
  if ((*param_2 != 8) && (DAT_1003788c == 0)) {
    iVar2 = FUN_10023315();
    if (iVar2 != 0) goto LAB_10023300;
  }
  __set_errno_from_matherr(*param_2);
LAB_10023300:
  __security_check_cookie(local_14 ^ (uint)&local_94);
  return;
}


// 10023315 FUN_10023315

undefined4 FUN_10023315(void)

{
  return 0;
}


// 10023318 __raise_exc_ex

/* Library Function - Single Match
    __raise_exc_ex
   
   Library: Visual Studio 2005 Release */

void __cdecl
__raise_exc_ex(uint *param_1,uint *param_2,uint param_3,int param_4,uint *param_5,uint *param_6,
              int param_7)

{
  uint *puVar1;
  uint *puVar2;
  uint uVar3;
  
  uVar3 = param_3;
  puVar1 = param_2;
  param_1[1] = 0;
  param_1[2] = 0;
  param_1[3] = 0;
  if ((param_3 & 0x10) != 0) {
    param_1[1] = param_1[1] | 1;
    param_3 = 0xc000008f;
  }
  if ((uVar3 & 2) != 0) {
    param_1[1] = param_1[1] | 2;
    param_3 = 0xc0000093;
  }
  if ((uVar3 & 1) != 0) {
    param_1[1] = param_1[1] | 4;
    param_3 = 0xc0000091;
  }
  if ((uVar3 & 4) != 0) {
    param_1[1] = param_1[1] | 8;
    param_3 = 0xc000008e;
  }
  if ((uVar3 & 8) != 0) {
    param_1[1] = param_1[1] | 0x10;
    param_3 = 0xc0000090;
  }
  param_1[2] = param_1[2] ^ (~(*param_2 << 4) ^ param_1[2]) & 0x10;
  param_1[2] = param_1[2] ^ (~(*param_2 * 2) ^ param_1[2]) & 8;
  param_1[2] = param_1[2] ^ (~(*param_2 >> 1) ^ param_1[2]) & 4;
  param_1[2] = param_1[2] ^ (~(*param_2 >> 3) ^ param_1[2]) & 2;
  param_1[2] = param_1[2] ^ (~(*param_2 >> 5) ^ param_1[2]) & 1;
  uVar3 = __statfp();
  puVar2 = param_6;
  if ((uVar3 & 1) != 0) {
    param_1[3] = param_1[3] | 0x10;
  }
  if ((uVar3 & 4) != 0) {
    param_1[3] = param_1[3] | 8;
  }
  if ((uVar3 & 8) != 0) {
    param_1[3] = param_1[3] | 4;
  }
  if ((uVar3 & 0x10) != 0) {
    param_1[3] = param_1[3] | 2;
  }
  if ((uVar3 & 0x20) != 0) {
    param_1[3] = param_1[3] | 1;
  }
  uVar3 = *puVar1 & 0xc00;
  if (uVar3 == 0) {
    *param_1 = *param_1 & 0xfffffffc;
  }
  else {
    if (uVar3 == 0x400) {
      uVar3 = *param_1 & 0xfffffffd | 1;
    }
    else {
      if (uVar3 != 0x800) {
        if (uVar3 == 0xc00) {
          *param_1 = *param_1 | 3;
        }
        goto LAB_10023475;
      }
      uVar3 = *param_1 & 0xfffffffe | 2;
    }
    *param_1 = uVar3;
  }
LAB_10023475:
  uVar3 = *puVar1 & 0x300;
  if (uVar3 == 0) {
    uVar3 = *param_1 & 0xffffffeb | 8;
LAB_100234ab:
    *param_1 = uVar3;
  }
  else {
    if (uVar3 == 0x200) {
      uVar3 = *param_1 & 0xffffffe7 | 4;
      goto LAB_100234ab;
    }
    if (uVar3 == 0x300) {
      *param_1 = *param_1 & 0xffffffe3;
    }
  }
  *param_1 = *param_1 ^ (param_4 << 5 ^ *param_1) & 0x1ffe0;
  param_1[8] = param_1[8] | 1;
  if (param_7 == 0) {
    param_1[8] = param_1[8] & 0xffffffe3 | 2;
    *(undefined8 *)(param_1 + 4) = *(undefined8 *)param_5;
    param_1[0x18] = param_1[0x18] | 1;
    param_1[0x18] = param_1[0x18] & 0xffffffe3 | 2;
    *(undefined8 *)(param_1 + 0x14) = *(undefined8 *)param_6;
  }
  else {
    param_1[8] = param_1[8] & 0xffffffe1;
    param_1[4] = *param_5;
    param_1[0x18] = param_1[0x18] | 1;
    param_1[0x18] = param_1[0x18] & 0xffffffe1;
    param_1[0x14] = *param_6;
  }
  __clrfp();
  RaiseException(param_3,0,1,(ULONG_PTR *)&param_1);
  if ((param_1[2] & 0x10) != 0) {
    *puVar1 = *puVar1 & 0xfffffffe;
  }
  if ((param_1[2] & 8) != 0) {
    *puVar1 = *puVar1 & 0xfffffffb;
  }
  if ((param_1[2] & 4) != 0) {
    *puVar1 = *puVar1 & 0xfffffff7;
  }
  if ((param_1[2] & 2) != 0) {
    *puVar1 = *puVar1 & 0xffffffef;
  }
  if ((param_1[2] & 1) != 0) {
    *puVar1 = *puVar1 & 0xffffffdf;
  }
  uVar3 = *param_1 & 3;
  if (uVar3 == 0) {
    *puVar1 = *puVar1 & 0xfffff3ff;
  }
  else {
    if (uVar3 == 1) {
      uVar3 = *puVar1 & 0xfffff7ff | 0x400;
    }
    else {
      if (uVar3 != 2) {
        if (uVar3 == 3) {
          *puVar1 = *puVar1 | 0xc00;
        }
        goto LAB_100235b0;
      }
      uVar3 = *puVar1 & 0xfffffbff | 0x800;
    }
    *puVar1 = uVar3;
  }
LAB_100235b0:
  uVar3 = *param_1 >> 2 & 7;
  if (uVar3 == 0) {
    uVar3 = *puVar1 & 0xfffff3ff | 0x300;
  }
  else {
    if (uVar3 != 1) {
      if (uVar3 == 2) {
        *puVar1 = *puVar1 & 0xfffff3ff;
      }
      goto LAB_100235dc;
    }
    uVar3 = *puVar1 & 0xfffff3ff | 0x200;
  }
  *puVar1 = uVar3;
LAB_100235dc:
  if (param_7 == 0) {
    *(undefined8 *)puVar2 = *(undefined8 *)(param_1 + 0x14);
  }
  else {
    *puVar2 = param_1[0x14];
  }
  return;
}


// 100235f2 __raise_exc

/* Library Function - Single Match
    __raise_exc
   
   Library: Visual Studio 2005 Release */

void __cdecl
__raise_exc(uint *param_1,uint *param_2,uint param_3,int param_4,uint *param_5,uint *param_6)

{
  __raise_exc_ex(param_1,param_2,param_3,param_4,param_5,param_6,0);
  return;
}


// 10023613 __handle_exc

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __handle_exc
   
   Library: Visual Studio 2005 Release */

bool __cdecl __handle_exc(uint param_1,double *param_2,uint param_3)

{
  double dVar1;
  double dVar2;
  bool bVar3;
  ulonglong uVar4;
  double dVar5;
  uint uVar6;
  bool bVar7;
  float10 fVar8;
  undefined8 local_14;
  int local_c;
  uint local_8;
  
  uVar6 = param_1 & 0x1f;
  bVar3 = true;
  local_8 = uVar6;
  if (((param_1 & 8) != 0) && ((param_3 & 1) != 0)) {
    FUN_1001e9f8();
    uVar6 = param_1 & 0x17;
    goto LAB_100237d2;
  }
  if (((param_1 & 4) != 0) && ((param_3 & 4) != 0)) {
    FUN_1001e9f8();
    uVar6 = param_1 & 0x1b;
    goto LAB_100237d2;
  }
  if (((param_1 & 1) == 0) || ((param_3 & 8) == 0)) {
    if (((param_1 & 2) != 0) && ((param_3 & 0x10) != 0)) {
      bVar7 = (param_1 & 0x10) != 0;
      if (*param_2 != 0.0) {
        fVar8 = (float10)FUN_10023a8a(SUB84(*param_2,0),(uint)((ulonglong)*param_2 >> 0x20),&local_c
                                     );
        dVar2 = (double)fVar8;
        local_c = local_c + -0x600;
        if (local_c < -0x432) {
          local_14 = dVar2 * 0.0;
          bVar7 = bVar3;
        }
        else {
          local_14 = (double)((ulonglong)dVar2 & 0xfffffffffffff | 0x10000000000000);
          if (local_c < -0x3fd) {
            local_c = -0x3fd - local_c;
            do {
              if ((((ulonglong)local_14 & 1) != 0) && (!bVar7)) {
                bVar7 = bVar3;
              }
              uVar6 = (uint)local_14 >> 1;
              uVar4 = (ulonglong)local_14 & 0x100000000;
              local_14._0_4_ = uVar6;
              if (uVar4 != 0) {
                local_14._0_4_ = uVar6 | 0x80000000;
              }
              local_14 = (double)CONCAT44(local_14._4_4_ >> 1,(uint)local_14);
              local_c = local_c + -1;
            } while (local_c != 0);
          }
          if (dVar2 < 0.0) {
            local_14 = -local_14;
          }
        }
        *param_2 = local_14;
        bVar3 = bVar7;
      }
      if (bVar3) {
        FUN_1001e9f8();
      }
      uVar6 = local_8 & 0xfffffffd;
      local_8 = uVar6;
    }
    goto LAB_100237d2;
  }
  FUN_1001e9f8();
  uVar6 = param_3 & 0xc00;
  dVar2 = _DAT_10037978;
  dVar5 = _DAT_10037978;
  if (uVar6 == 0) {
    dVar1 = *param_2;
joined_r0x100236b5:
    if (dVar1 <= 0.0) {
      dVar2 = -dVar5;
    }
    *param_2 = dVar2;
  }
  else {
    if (uVar6 == 0x400) {
      dVar1 = *param_2;
      dVar2 = _DAT_10037988;
      goto joined_r0x100236b5;
    }
    dVar5 = _DAT_10037988;
    if (uVar6 == 0x800) {
      dVar1 = *param_2;
      goto joined_r0x100236b5;
    }
    if (uVar6 == 0xc00) {
      dVar1 = *param_2;
      dVar2 = _DAT_10037988;
      goto joined_r0x100236b5;
    }
  }
  uVar6 = param_1 & 0x1e;
LAB_100237d2:
  if (((param_1 & 0x10) != 0) && ((param_3 & 0x20) != 0)) {
    FUN_1001e9f8();
    uVar6 = uVar6 & 0xffffffef;
  }
  return uVar6 == 0;
}


// 100237f4 __set_errno_from_matherr

/* Library Function - Single Match
    __set_errno_from_matherr
   
   Library: Visual Studio 2005 Release */

void __cdecl __set_errno_from_matherr(int param_1)

{
  DWORD *pDVar1;
  
  if (param_1 == 1) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x21;
  }
  else if ((1 < param_1) && (param_1 < 4)) {
    pDVar1 = FUN_10011e63();
    *pDVar1 = 0x22;
    return;
  }
  return;
}


// 1002381c __errcode

/* Library Function - Single Match
    __errcode
   
   Library: Visual Studio 2005 Release */

char __cdecl __errcode(byte param_1)

{
  char cVar1;
  
  if ((param_1 & 0x20) == 0) {
    if ((param_1 & 8) != 0) {
      return '\x01';
    }
    if ((param_1 & 4) == 0) {
      if ((param_1 & 1) == 0) {
        return (param_1 & 2) * '\x02';
      }
      cVar1 = '\x03';
    }
    else {
      cVar1 = '\x02';
    }
  }
  else {
    cVar1 = '\x05';
  }
  return cVar1;
}


// 10023849 __umatherr

/* Library Function - Single Match
    __umatherr
   
   Library: Visual Studio 2005 Release */

float10 __cdecl __umatherr(int param_1,int param_2)

{
  int iVar1;
  undefined4 in_stack_0000001c;
  undefined4 in_stack_00000020;
  
  iVar1 = 0;
  do {
    if ((&DAT_10037890)[iVar1 * 2] == param_2) {
      iVar1 = *(int *)(iVar1 * 8 + 0x10037894);
      goto LAB_10023865;
    }
    iVar1 = iVar1 + 1;
  } while (iVar1 < 0x1d);
  iVar1 = 0;
LAB_10023865:
  if (iVar1 != 0) {
    __ctrlfp();
    iVar1 = FUN_10023315();
    if (iVar1 == 0) {
      __set_errno_from_matherr(param_1);
    }
    return (float10)(double)CONCAT44(in_stack_00000020,in_stack_0000001c);
  }
  __ctrlfp();
  __set_errno_from_matherr(param_1);
  return (float10)(double)CONCAT44(in_stack_00000020,in_stack_0000001c);
}


// 100238e7 __handle_qnan1

/* Library Function - Single Match
    __handle_qnan1
   
   Library: Visual Studio 2005 Release */

float10 __cdecl __handle_qnan1(int param_1,double param_2)

{
  DWORD *pDVar1;
  float10 fVar2;
  
  if (DAT_1003788c == 0) {
    fVar2 = __umatherr(1,param_1);
    return fVar2;
  }
  pDVar1 = FUN_10011e63();
  *pDVar1 = 0x21;
  __ctrlfp();
  return (float10)param_2;
}


// 1002393a __except1

/* Library Function - Single Match
    __except1
   
   Library: Visual Studio 2005 Release */

void __cdecl __except1(uint param_1,int param_2,undefined8 param_3,double param_4,uint param_5)

{
  bool bVar1;
  char cVar2;
  undefined3 extraout_var;
  undefined3 extraout_var_00;
  uint local_90 [16];
  uint local_50;
  uint local_14;
  int iVar3;
  
  local_14 = DAT_100366b8 ^ (uint)local_90;
  bVar1 = __handle_exc(param_1,&param_4,param_5);
  if (CONCAT31(extraout_var,bVar1) == 0) {
    local_50 = local_50 & 0xfffffffe;
    __raise_exc_ex(local_90,&param_5,param_1,param_2,(uint *)&param_3,(uint *)&param_4,0);
  }
  cVar2 = __errcode((byte)param_1);
  iVar3 = CONCAT31(extraout_var_00,cVar2);
  if ((DAT_1003788c == 0) && (iVar3 != 0)) {
    __umatherr(iVar3,param_2);
  }
  else {
    __set_errno_from_matherr(iVar3);
    __ctrlfp();
  }
  __security_check_cookie(local_14 ^ (uint)local_90);
  return;
}


// 100239f4 __frnd

/* Library Function - Single Match
    __frnd
   
   Library: Visual Studio 2005 Release */

float10 __cdecl __frnd(double param_1)

{
  return (float10)ROUND(param_1);
}


// 10023a05 __set_exp

/* Library Function - Single Match
    __set_exp
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release */

float10 __cdecl __set_exp(undefined8 param_1,short param_2)

{
  undefined8 local_c;
  
  local_c = (double)CONCAT26((param_2 + 0x3fe) * 0x10 | param_1._6_2_ & 0x800f,(int6)param_1);
  return (float10)local_c;
}


// 10023a2f __sptype

/* Library Function - Single Match
    __sptype
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __sptype(int param_1,uint param_2)

{
  undefined4 uStack_8;
  
  if (param_2 == 0x7ff00000) {
    if (param_1 == 0) {
      return 1;
    }
  }
  else if ((param_2 == 0xfff00000) && (param_1 == 0)) {
    return 2;
  }
  if ((param_2._2_2_ & 0x7ff8) == 0x7ff8) {
    uStack_8 = 3;
  }
  else {
    if (((param_2._2_2_ & 0x7ff8) != 0x7ff0) || (((param_2 & 0x7ffff) == 0 && (param_1 == 0)))) {
      return 0;
    }
    uStack_8 = 4;
  }
  return uStack_8;
}


// 10023a8a FUN_10023a8a

void __cdecl FUN_10023a8a(uint param_1,uint param_2,int *param_3)

{
  ushort uVar1;
  bool bVar2;
  int iVar3;
  int extraout_EDX;
  
  if ((double)CONCAT17(param_2._3_1_,CONCAT16(param_2._2_1_,CONCAT24((ushort)param_2,param_1))) ==
      0.0) {
    iVar3 = 0;
  }
  else if (((param_2 & 0x7ff00000) == 0) && (((param_2 & 0xfffff) != 0 || (param_1 != 0)))) {
    if (0.0 <= (double)CONCAT17(param_2._3_1_,
                                CONCAT16(param_2._2_1_,CONCAT24((ushort)param_2,param_1)))) {
      bVar2 = false;
    }
    else {
      bVar2 = true;
    }
    while ((param_2._2_1_ & 0x10) == 0) {
      iVar3 = CONCAT13(param_2._3_1_,CONCAT12(param_2._2_1_,(ushort)param_2)) << 1;
      param_2._0_2_ = (ushort)iVar3;
      param_2._2_1_ = (byte)((uint)iVar3 >> 0x10);
      param_2._3_1_ = (byte)((uint)iVar3 >> 0x18);
      if ((param_1 & 0x80000000) != 0) {
        param_2._0_2_ = (ushort)param_2 | 1;
      }
      param_1 = param_1 << 1;
    }
    uVar1 = CONCAT11(param_2._3_1_,param_2._2_1_) & 0xffef;
    param_2._2_1_ = (byte)uVar1;
    param_2._3_1_ = (byte)(uVar1 >> 8);
    if (bVar2) {
      param_2._3_1_ = param_2._3_1_ | 0x80;
    }
    __set_exp(CONCAT17(param_2._3_1_,CONCAT16(param_2._2_1_,CONCAT24((ushort)param_2,param_1))),0);
    iVar3 = extraout_EDX;
  }
  else {
    __set_exp(CONCAT17(param_2._3_1_,CONCAT16(param_2._2_1_,CONCAT24((ushort)param_2,param_1))),0);
    iVar3 = ((param_2 >> 0x10 & 0x7ff0) >> 4) - 0x3fe;
  }
  *param_3 = iVar3;
  return;
}


// 10023b39 __fpclass

/* Library Function - Single Match
    __fpclass
   
   Library: Visual Studio 2005 Release */

int __cdecl __fpclass(double _X)

{
  int iVar1;
  
  if ((_X._6_2_ & 0x7ff0) == 0x7ff0) {
    iVar1 = __sptype(_X._0_4_,(uint)((ulonglong)_X >> 0x20));
    if (iVar1 == 1) {
      return 0x200;
    }
    if (iVar1 == 2) {
      iVar1 = 4;
    }
    else {
      if (iVar1 != 3) {
        return 1;
      }
      iVar1 = 2;
    }
    return iVar1;
  }
  if ((((ulonglong)_X & 0x7ff0000000000000) == 0) &&
     ((((ulonglong)_X & 0xfffff00000000) != 0 || (_X._0_4_ != 0)))) {
    return (-(uint)(((ulonglong)_X & 0x8000000000000000) != 0) & 0xffffff90) + 0x80;
  }
  if (_X == 0.0) {
    return (-(uint)(((ulonglong)_X & 0x8000000000000000) != 0) & 0xffffffe0) + 0x40;
  }
  return (-(uint)(((ulonglong)_X & 0x8000000000000000) != 0) & 0xffffff08) + 0x100;
}


// 10023bd0 FUN_10023bd0

void FUN_10023bd0(void)

{
  FUN_100096b0(&DAT_10034054,0,0x20,1);
  DAT_1003408c = 0;
  DAT_10034090 = 0;
  DAT_10034094 = 0;
  DAT_10034098 = 0;
  DAT_1003409c = 0;
  FUN_100096b0(&DAT_100340a0,0,0x20,1);
  DAT_1003403c = 0;
  _atexit((_func_4879 *)&LAB_100242e0);
  return;
}


