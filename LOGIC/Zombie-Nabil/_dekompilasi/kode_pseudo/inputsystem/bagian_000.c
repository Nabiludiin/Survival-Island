// 10001040 FUN_10001040

void __fastcall FUN_10001040(int *param_1)

{
  if (param_1[3] != 0) {
    (**(code **)(*param_1 + 100))();
    if (param_1[2] != 0) {
      SetWindowLongA((HWND)param_1[3],-4,param_1[2]);
      param_1[2] = 0;
    }
    FUN_10003c70();
    param_1[3] = 0;
  }
  return;
}


// 10001120 FUN_10001120

int __thiscall FUN_10001120(void *this,int param_1,uint param_2)

{
  int iVar1;
  
  if (*(char *)((int)this + 0x1620) != '\0') {
    iVar1 = FUN_10003590(param_1,param_2);
    return iVar1;
  }
  return 0;
}


// 10001160 FUN_10001160

void FUN_10001160(undefined4 param_1,undefined4 param_2,undefined4 param_3)

{
  HWND hWnd;
  code *pcVar1;
  
  hWnd = GetFocus();
  pcVar1 = (code *)GetWindowLongA(hWnd,-4);
  if (pcVar1 != (code *)0x0) {
    (*pcVar1)(hWnd,param_1,param_2,param_3);
  }
  return;
}


// 100011a0 FUN_100011a0

void __fastcall FUN_100011a0(void *param_1)

{
  int iVar1;
  
  iVar1 = Plat_MSTime();
  *(int *)((int)param_1 + 0x160c) = iVar1 - *(int *)((int)param_1 + 0x1604);
  if (*(char *)((int)param_1 + 0x1620) == '\0') {
    FUN_10002ea0(param_1);
  }
  else {
    FUN_10004b40(param_1);
  }
  if (*(char *)((int)param_1 + 0x17d8) != '\0') {
    FUN_10003e20(param_1);
    return;
  }
  return;
}


// 100012c0 FUN_100012c0

void __thiscall FUN_100012c0(void *this,int param_1,int param_2)

{
  size_t _Size;
  int iVar1;
  
  iVar1 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
  _Size = (param_2 - param_1) * 4 + 4;
  _memset((void *)((int)this + param_1 * 4 + iVar1 + 0xa14),0,_Size);
  _memset((void *)((int)this + param_1 * 4 + iVar1 + 0xa84),0,_Size);
  return;
}


// 10001310 FUN_10001310

byte FUN_10001310(byte param_1)

{
  byte bVar1;
  
  bVar1 = (param_1 & 1) != 0;
  if ((param_1 & 2) != 0) {
    bVar1 = bVar1 | 2;
  }
  if ((param_1 & 0x10) != 0) {
    bVar1 = bVar1 | 4;
  }
  if ((param_1 & 0x20) != 0) {
    bVar1 = bVar1 | 8;
  }
  if ((param_1 & 0x40) != 0) {
    bVar1 = bVar1 | 0x10;
  }
  return bVar1;
}


// 10001380 FUN_10001380

void __thiscall FUN_10001380(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_100013db;
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
LAB_100013db:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 * 0x14);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 100014b0 FUN_100014b0

int __thiscall FUN_100014b0(void *this,int param_1,undefined4 *param_2)

{
  void *pvVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10001380(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar3 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar3) {
    pvVar1 = (void *)(*this + param_1 * 0x14);
    FUN_10005230((void *)((int)pvVar1 + 0x14),pvVar1,iVar3 * 0x14);
  }
                    /* WARNING: Load size is inaccurate */
  puVar2 = (undefined4 *)(*this + param_1 * 0x14);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_2;
    puVar2[1] = param_2[1];
    puVar2[2] = param_2[2];
    puVar2[3] = param_2[3];
    puVar2[4] = param_2[4];
  }
  return param_1;
}


// 10001540 FUN_10001540

int __thiscall FUN_10001540(void *this,int param_1)

{
  void *pvVar1;
  int iVar2;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10001380(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 0x10) = *this;
  iVar2 = (*(int *)((int)this + 0xc) - param_1) + -1;
  if (0 < iVar2) {
    pvVar1 = (void *)(*this + param_1 * 0x14);
    FUN_10005230((void *)((int)pvVar1 + 0x14),pvVar1,iVar2 * 0x14);
  }
  return param_1;
}


// 100015a0 FUN_100015a0

int __thiscall FUN_100015a0(void *this,int param_1,int param_2,undefined4 *param_3)

{
  undefined4 *puVar1;
  int iVar2;
  undefined4 *puVar3;
  int iVar4;
  
  if (param_2 != 0) {
    if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + param_2) {
      FUN_10001380(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + param_2);
    }
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
                    /* WARNING: Load size is inaccurate */
    iVar4 = *this;
    iVar2 = (*(int *)((int)this + 0xc) - param_1) - param_2;
    *(int *)((int)this + 0x10) = iVar4;
    if ((0 < iVar2) && (0 < param_2)) {
      FUN_10005230((void *)(iVar4 + (param_1 + param_2) * 0x14),(void *)(iVar4 + param_1 * 0x14),
                   iVar2 * 0x14);
    }
    if ((param_3 != (undefined4 *)0x0) && (0 < param_2)) {
      iVar4 = param_1 * 0x14;
      do {
                    /* WARNING: Load size is inaccurate */
        puVar3 = (undefined4 *)(*this + iVar4);
        *puVar3 = *param_3;
        puVar3[1] = param_3[1];
        puVar3[2] = param_3[2];
        puVar3[3] = param_3[3];
        puVar1 = param_3 + 4;
        param_3 = param_3 + 5;
        iVar4 = iVar4 + 0x14;
        param_2 = param_2 + -1;
        puVar3[4] = *puVar1;
      } while (param_2 != 0);
    }
    return param_1;
  }
  return param_1;
}


// 100017d0 FUN_100017d0

undefined4 __thiscall FUN_100017d0(void *this,undefined4 param_1)

{
  undefined4 uVar1;
  bool bVar2;
  undefined4 local_4;
  
  bVar2 = *(char *)((int)this + 4) == '\0';
  local_4 = param_1;
  uVar1 = param_1;
  if (!bVar2) {
    uVar1 = FUN_100052f0((int)&param_1,1);
    bVar2 = *(char *)((int)this + 4) == '\0';
  }
  if (!bVar2) {
    uVar1 = FUN_10004c40((int)&local_4,1);
  }
  return CONCAT31((int3)((uint)uVar1 >> 8),1);
}


// 10001810 FUN_10001810

void __fastcall FUN_10001810(int param_1)

{
  if (*(HANDLE *)(param_1 + 0x1da0) != (HANDLE)0x0) {
    CloseHandle(*(HANDLE *)(param_1 + 0x1da0));
    *(undefined4 *)(param_1 + 0x1da0) = 0;
  }
  if ((DAT_1001ac80 != 0) && (*(char *)(param_1 + 4) != '\0')) {
    FUN_10005430();
    return;
  }
  return;
}


// 10001850 FUN_10001850

void __fastcall FUN_10001850(int param_1)

{
  int *piVar1;
  int iVar2;
  
  piVar1 = (int *)(param_1 + 0xafc);
  iVar2 = 2;
  do {
    if (piVar1 != (int *)0xae8) {
      piVar1[-0x2ba] = 0;
      piVar1[-0x2b9] = 0;
      piVar1[-0x2b8] = 0;
      piVar1[-0x2b7] = 0;
      piVar1[-0x2b6] = 0;
      piVar1[-0x2b5] = 0;
      piVar1[-0x2b4] = 0;
      piVar1[-0x2b3] = 0;
      piVar1[-0x2b2] = 0;
      piVar1[-0x2b1] = 0;
    }
    _memset(piVar1 + -0x3a,0,0x70);
    _memset(piVar1 + -0x1e,0,0x70);
    _memset(piVar1 + -0x2b0,0,0x4ec);
    _memset(piVar1 + -0x175,0,0x4ec);
    piVar1[1] = 0;
    if (-1 < *piVar1) {
      if (piVar1[-2] != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar1[-2]);
        piVar1[-2] = 0;
      }
      piVar1[-1] = 0;
    }
    piVar1[2] = piVar1[-2];
    *(undefined1 *)(piVar1 + 3) = 0;
    piVar1 = piVar1 + 0x2be;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  _memset((void *)(param_1 + 0x1810),0,0x340);
  return;
}


// 10001950 FUN_10001950

void __fastcall FUN_10001950(undefined4 *param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  int *piVar4;
  
  *param_1 = &PTR_FUN_100122d4;
  if ((HMODULE)param_1[0x769] != (HMODULE)0x0) {
    FUN_10006470((HMODULE)param_1[0x769]);
    param_1[0x769] = 0;
  }
  if ((HMODULE)param_1[0x76a] != (HMODULE)0x0) {
    FUN_10006470((HMODULE)param_1[0x76a]);
    param_1[0x76a] = 0;
  }
  if ((HMODULE)param_1[0x76b] != (HMODULE)0x0) {
    FUN_10006470((HMODULE)param_1[0x76b]);
    param_1[0x76b] = 0;
  }
  iVar2 = 1;
  piVar3 = param_1 + 0x839;
  do {
    piVar4 = piVar3 + -0x2be;
    piVar3[-699] = 0;
    if (-1 < piVar3[-700]) {
      if (*piVar4 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar4);
        *piVar4 = 0;
      }
      piVar3[-0x2bd] = 0;
    }
    iVar1 = *piVar4;
    piVar3[-0x2ba] = iVar1;
    if (-1 < piVar3[-700]) {
      if (iVar1 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
        *piVar4 = 0;
      }
      piVar3[-0x2bd] = 0;
    }
    iVar2 = iVar2 + -1;
    piVar3 = piVar4;
  } while (-1 < iVar2);
  return;
}


// 10001a10 FUN_10001a10

void __fastcall FUN_10001a10(int param_1)

{
  bool bVar1;
  
  bVar1 = *(char *)(param_1 + 4) == '\0';
  if (!bVar1) {
    FUN_10004df0();
    bVar1 = *(char *)(param_1 + 4) == '\0';
  }
  if (!bVar1) {
    FUN_10005360();
    return;
  }
  return;
}


// 10001a30 FUN_10001a30

void __thiscall
FUN_10001a30(void *this,undefined4 param_1,undefined4 param_2,undefined4 param_3,undefined4 param_4,
            undefined4 param_5)

{
  int *this_00;
  undefined4 *puVar1;
  int iVar2;
  int iVar3;
  
  iVar2 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
  this_00 = (int *)((int)this + iVar2 + 0xaf4);
  iVar3 = FUN_10001540(this_00,*(int *)((int)this + iVar2 + 0xb00));
  puVar1 = (undefined4 *)(*this_00 + iVar3 * 0x14);
  *puVar1 = param_1;
  puVar1[1] = param_2;
  puVar1[2] = param_3;
  puVar1[3] = param_4;
  puVar1[4] = param_5;
  *(undefined1 *)((int)this + iVar2 + 0xb08) = 1;
  return;
}


// 10001a90 FUN_10001a90

void __thiscall
FUN_10001a90(void *this,undefined4 param_1,undefined4 param_2,int param_3,undefined4 param_4)

{
  uint *puVar1;
  int iVar2;
  uint uVar3;
  
  iVar2 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
  uVar3 = 1 << ((byte)param_3 & 0x1f);
  puVar1 = (uint *)((int)this + (param_3 >> 5) * 4 + iVar2 + 0x14);
  if ((*puVar1 & uVar3) == 0) {
    *puVar1 = *puVar1 | uVar3;
    *(undefined4 *)((int)this + param_3 * 4 + iVar2 + 0x3c) = param_2;
    FUN_10001a30(this,param_1,param_2,param_3,param_4,0);
  }
  return;
}


// 10001af0 FUN_10001af0

void __thiscall
FUN_10001af0(void *this,undefined4 param_1,undefined4 param_2,int param_3,undefined4 param_4)

{
  uint *puVar1;
  int iVar2;
  uint uVar3;
  
  iVar2 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
  uVar3 = 1 << ((byte)param_3 & 0x1f);
  puVar1 = (uint *)((int)this + (param_3 >> 5) * 4 + iVar2 + 0x14);
  if ((*puVar1 & uVar3) != 0) {
    *puVar1 = *puVar1 & ~uVar3;
    *(undefined4 *)((int)this + param_3 * 4 + iVar2 + 0x528) = param_2;
    FUN_10001a30(this,param_1,param_2,param_3,param_4,0);
  }
  return;
}


// 10001b50 FUN_10001b50

void __thiscall FUN_10001b50(void *this,int param_1,short param_2,short param_3)

{
  int iVar1;
  int iVar2;
  int iVar3;
  
  iVar3 = (int)param_2;
  iVar1 = *(int *)(param_1 + 0xa74);
  iVar2 = iVar3 - *(int *)(param_1 + 0xa70);
  *(int *)(param_1 + 0xa74) = (int)param_3;
  *(int *)(param_1 + 0xa70) = iVar3;
  *(int *)(param_1 + 0xa00) = iVar2;
  *(int *)(param_1 + 0xa04) = param_3 - iVar1;
  if (iVar2 != 0) {
    FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),0,iVar3,iVar2);
  }
  if (*(int *)(param_1 + 0xa04) != 0) {
    FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),1,*(undefined4 *)(param_1 + 0xa74),
                 *(int *)(param_1 + 0xa04));
  }
  if ((*(int *)(param_1 + 0xa00) != 0) || (*(int *)(param_1 + 0xa04) != 0)) {
    FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),2,*(undefined4 *)(param_1 + 0xa70),
                 *(undefined4 *)(param_1 + 0xa74));
  }
  return;
}


// 10001c10 FUN_10001c10

void __fastcall FUN_10001c10(void *param_1)

{
  int iVar1;
  tagMSG local_1c;
  
  if (*(char *)((int)param_1 + 0x11) != '\0') {
    iVar1 = PeekMessageA(&local_1c,(HWND)0x0,0,0,1);
    while (iVar1 != 0) {
      if (local_1c.message == 0x12) {
        FUN_10001a30(param_1,100,*(undefined4 *)((int)param_1 + 0x160c),0,0,0);
        return;
      }
      TranslateMessage(&local_1c);
      DispatchMessageA(&local_1c);
      iVar1 = PeekMessageA(&local_1c,(HWND)0x0,0,0,1);
    }
  }
  return;
}


// 10001cf0 FUN_10001cf0

void __thiscall FUN_10001cf0(void *this,int param_1,int param_2)

{
  int *piVar1;
  uint *puVar2;
  void *pvVar3;
  undefined4 *puVar4;
  int iVar5;
  undefined4 uVar6;
  uint uVar7;
  int iVar8;
  int iVar9;
  int iVar10;
  int *this_00;
  
  if (param_1 <= param_2) {
    iVar10 = param_1;
    param_1 = param_1 * 4 + 0x514;
    do {
      uVar6 = *(undefined4 *)((int)this + 0x160c);
      iVar5 = (uint)*(byte *)((int)this + 0x12) * 0xaf8 + 0x14;
      uVar7 = 1 << ((byte)iVar10 & 0x1f);
      puVar2 = (uint *)((int)this + (iVar10 >> 5) * 4 + iVar5);
      if ((*puVar2 & uVar7) != 0) {
        *puVar2 = *puVar2 & ~uVar7;
        *(undefined4 *)((int)this + param_1 + iVar5) = uVar6;
        iVar9 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
        iVar5 = *(int *)((int)this + iVar9 + 0xb00);
        iVar8 = *(int *)((int)this + iVar9 + 0xaf8);
        this_00 = (int *)((int)this + iVar9 + 0xaf4);
        if (iVar8 < iVar5 + 1) {
          FUN_10001380(this_00,(iVar5 - iVar8) + 1);
        }
        piVar1 = (int *)((int)this + iVar9 + 0xb00);
        *piVar1 = *piVar1 + 1;
        iVar8 = (*(int *)((int)this + iVar9 + 0xb00) - iVar5) + -1;
        *(int *)((int)this + iVar9 + 0xb04) = *this_00;
        if (0 < iVar8) {
          pvVar3 = (void *)(*this_00 + iVar5 * 0x14);
          FUN_10005230((void *)((int)pvVar3 + 0x14),pvVar3,iVar8 * 0x14);
        }
        puVar4 = (undefined4 *)(*this_00 + iVar5 * 0x14);
        *puVar4 = 1;
        puVar4[1] = uVar6;
        puVar4[2] = iVar10;
        puVar4[3] = iVar10;
        puVar4[4] = 0;
        *(undefined1 *)((int)this + iVar9 + 0xb08) = 1;
      }
      param_1 = param_1 + 4;
      iVar10 = iVar10 + 1;
    } while (iVar10 <= param_2);
  }
  return;
}


// 10001e20 FUN_10001e20

void __thiscall FUN_10001e20(void *this,uint param_1,uint param_2)

{
  byte bVar1;
  int iVar2;
  uint *puVar3;
  uint uVar4;
  int iVar5;
  uint uVar6;
  int local_8;
  uint local_4;
  
  local_8 = 0x6c0;
  uVar6 = 0x6b;
  do {
    bVar1 = (byte)uVar6;
    if ((param_1 & 1 << (bVar1 + 0x95 & 0x1f)) == 0) {
      local_4 = *(uint *)((int)this + 0x160c);
      iVar5 = (uint)*(byte *)((int)this + 0x12) * 0xaf8 + 0x14;
      uVar4 = 1 << (bVar1 & 0x1f);
      puVar3 = (uint *)((int)this + ((int)uVar6 >> 5) * 4 + iVar5);
      if ((*puVar3 & uVar4) != 0) {
        *puVar3 = *puVar3 & ~uVar4;
        *(uint *)((int)this + local_8 + iVar5) = local_4;
        iVar5 = (uint)*(byte *)((int)this + 0x12) * 0xaf8 + 0x14 + (int)this;
        iVar2 = FUN_10001540((int *)(iVar5 + 0xae0),*(int *)(iVar5 + 0xaec));
        puVar3 = (uint *)(*(int *)(iVar5 + 0xae0) + iVar2 * 0x14);
        *puVar3 = 1;
        goto LAB_10001f31;
      }
    }
    else {
      local_4 = *(uint *)((int)this + 0x160c);
      iVar5 = (uint)*(byte *)((int)this + 0x12) * 0xaf8 + 0x14;
      uVar4 = 1 << (bVar1 & 0x1f);
      puVar3 = (uint *)((int)this + ((int)uVar6 >> 5) * 4 + iVar5);
      if ((*puVar3 & uVar4) == 0) {
        *puVar3 = *puVar3 | uVar4;
        *(uint *)((int)this + local_8 + -0x4ec + iVar5) = local_4;
        iVar5 = (uint)*(byte *)((int)this + 0x12) * 0xaf8 + 0x14 + (int)this;
        iVar2 = FUN_10001540((int *)(iVar5 + 0xae0),*(int *)(iVar5 + 0xaec));
        puVar3 = (uint *)(*(int *)(iVar5 + 0xae0) + iVar2 * 0x14);
        *puVar3 = (uVar6 != param_2) - 1 & 2;
LAB_10001f31:
        puVar3[1] = local_4;
        puVar3[2] = uVar6;
        puVar3[3] = uVar6;
        puVar3[4] = 0;
        *(undefined1 *)(iVar5 + 0xaf4) = 1;
      }
    }
    local_8 = local_8 + 4;
    iVar5 = uVar6 - 0x6a;
    uVar6 = uVar6 + 1;
    if (4 < iVar5) {
      return;
    }
  } while( true );
}


// 10001f70 FUN_10001f70

void __thiscall FUN_10001f70(void *this,int param_1,int param_2)

{
  int iVar1;
  bool bVar2;
  bool bVar3;
  tagPOINT local_8;
  
  if (*(HWND *)((int)this + 0xc) != (HWND)0x0) {
    local_8.x = param_1;
    local_8.y = param_2;
    ClientToScreen(*(HWND *)((int)this + 0xc),&local_8);
    SetCursorPos(local_8.x,local_8.y);
    iVar1 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
    bVar2 = *(int *)(iVar1 + 0xa84 + (int)this) != param_1;
    bVar3 = *(int *)((int)this + iVar1 + 0xa88) != param_2;
    *(int *)((int)this + iVar1 + 0xa84) = param_1;
    *(int *)((int)this + iVar1 + 0xa88) = param_2;
    *(undefined4 *)((int)this + iVar1 + 0xa14) = 0;
    *(undefined4 *)((int)this + iVar1 + 0xa18) = 0;
    if (bVar2) {
      FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),0,param_1,0);
    }
    if (bVar3) {
      FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),1,
                   *(undefined4 *)((int)this + iVar1 + 0xa88),
                   *(undefined4 *)((int)this + iVar1 + 0xa18));
    }
    if ((bVar2) || (bVar3)) {
      FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),2,
                   *(undefined4 *)((int)this + iVar1 + 0xa84),
                   *(undefined4 *)((int)this + iVar1 + 0xa88));
    }
  }
  return;
}


// 10002070 FUN_10002070

LRESULT __thiscall FUN_10002070(void *this,HWND param_1,uint param_2,uint param_3,uint param_4)

{
  int iVar1;
  byte bVar2;
  ushort uVar3;
  LRESULT LVar4;
  int nVirtKey;
  undefined4 uVar5;
  undefined3 extraout_var;
  undefined3 extraout_var_00;
  undefined3 extraout_var_01;
  undefined3 extraout_var_02;
  undefined3 extraout_var_03;
  undefined3 extraout_var_04;
  undefined3 extraout_var_05;
  undefined3 extraout_var_06;
  uint uVar6;
  undefined3 extraout_var_07;
  int iVar7;
  uint uVar8;
  
  uVar8 = param_4;
  uVar6 = param_3;
  if (*(char *)((int)this + 0x10) == '\0') {
    if (*(WNDPROC *)((int)this + 8) == (WNDPROC)0x0) {
      return 0;
    }
    LVar4 = CallWindowProcA(*(WNDPROC *)((int)this + 8),param_1,param_2,param_3,param_4);
    return LVar4;
  }
  if (param_1 != *(HWND *)((int)this + 0xc)) {
    if (*(WNDPROC *)((int)this + 8) == (WNDPROC)0x0) {
      return 0;
    }
    LVar4 = CallWindowProcA(*(WNDPROC *)((int)this + 8),param_1,param_2,param_3,param_4);
    return LVar4;
  }
  iVar1 = (uint)*(byte *)((int)this + 0x12) * 0xaf8 + 0x14 + (int)this;
  bVar2 = (byte)param_3;
  if (param_2 < 0x201) {
    if (param_2 == 0x200) {
      FUN_10001b50(this,iVar1,(short)param_4,(short)(param_4 >> 0x10));
      uVar8 = 0xffffffff;
      bVar2 = FUN_10001310(bVar2);
      FUN_10001e20(this,CONCAT31(extraout_var,bVar2),uVar8);
    }
    else {
      switch(param_2) {
      case 0x1c:
        if (param_3 != 1) {
                    /* WARNING: Load size is inaccurate */
          (**(code **)(*this + 100))();
        }
        break;
      case 0xff:
        if (*(char *)((int)this + 0x1d94) != '\0') {
          param_3 = 0x28;
          (*DAT_10018164)(param_4,0x10000003,&DAT_10019f78,&param_3,0x10);
          if (DAT_10019f78 == 0) {
            *(int *)((int)this + 0x1d98) = *(int *)((int)this + 0x1d98) + DAT_10019f94;
            *(int *)((int)this + 0x1d9c) = *(int *)((int)this + 0x1d9c) + DAT_10019f98;
          }
        }
        break;
      case 0x100:
      case 0x104:
        if ((param_4 & 0x40000000) == 0) {
          param_3 = FUN_10003560(param_3);
          iVar7 = FUN_100037a0(uVar8);
          FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar7,param_3);
          if (((iVar7 == 0x44) || (iVar7 == 0x47)) || (iVar7 == 0x45)) {
            if (iVar7 == 0x45) {
              nVirtKey = 0x90;
              iVar7 = 0x69;
            }
            else if (iVar7 == 0x47) {
              nVirtKey = 0x91;
              iVar7 = 0x6a;
            }
            else {
              nVirtKey = 0x14;
              iVar7 = 0x68;
            }
            uVar3 = GetKeyState(nVirtKey);
            FUN_10001a90(this,(uint)((uVar3 & 1) == 0),*(undefined4 *)((int)this + 0x160c),iVar7,
                         iVar7);
          }
        }
        break;
      case 0x101:
      case 0x105:
        uVar5 = FUN_10003560(param_3);
        iVar7 = FUN_100037a0(param_4);
        FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar7,uVar5);
      }
    }
    goto switchD_1000210f_caseD_1d;
  }
  switch(param_2) {
  case 0x201:
    uVar8 = 0xffffffff;
    bVar2 = FUN_10001310(bVar2);
    FUN_10001e20(this,CONCAT31(extraout_var_00,bVar2),uVar8);
    break;
  case 0x202:
    uVar8 = 0xffffffff;
    bVar2 = FUN_10001310(bVar2);
    FUN_10001e20(this,CONCAT31(extraout_var_01,bVar2),uVar8);
    break;
  case 0x203:
    uVar8 = 0x6b;
    bVar2 = FUN_10001310(bVar2);
    FUN_10001e20(this,CONCAT31(extraout_var_04,bVar2),uVar8);
    break;
  case 0x204:
    uVar8 = 0xffffffff;
    bVar2 = FUN_10001310(bVar2);
    FUN_10001e20(this,CONCAT31(extraout_var_02,bVar2),uVar8);
    break;
  case 0x205:
    uVar8 = 0xffffffff;
    bVar2 = FUN_10001310(bVar2);
    FUN_10001e20(this,CONCAT31(extraout_var_03,bVar2),uVar8);
    break;
  case 0x206:
    uVar8 = 0x6c;
    bVar2 = FUN_10001310(bVar2);
    FUN_10001e20(this,CONCAT31(extraout_var_05,bVar2),uVar8);
    break;
  case 0x207:
    uVar8 = 0xffffffff;
    goto LAB_10002367;
  case 0x208:
    uVar8 = 0xffffffff;
    goto LAB_10002367;
  case 0x209:
    uVar8 = 0x6d;
LAB_10002367:
    bVar2 = FUN_10001310(bVar2);
    FUN_10001e20(this,CONCAT31(extraout_var_06,bVar2),uVar8);
    break;
  case 0x20a:
    uVar8 = (uint)((short)(param_3 >> 0x10) < 1);
    uVar5 = *(undefined4 *)((int)this + 0x160c);
    *(undefined4 *)(iVar1 + 0x6d4 + uVar8 * 4) = uVar5;
    *(undefined4 *)(iVar1 + 0x1e8 + uVar8 * 4) = uVar5;
    iVar7 = uVar8 + 0x70;
    param_3 = param_3 >> 0x10;
    FUN_10001a30(this,0,*(undefined4 *)((int)this + 0x160c),iVar7,iVar7,0);
    FUN_10001a30(this,1,*(undefined4 *)((int)this + 0x160c),iVar7,iVar7,0);
    iVar7 = (int)(short)param_3 / 0x78;
    *(int *)(iVar1 + 0xa7c) = *(int *)(iVar1 + 0xa7c) + iVar7;
    *(int *)(iVar1 + 0xa0c) = iVar7;
    FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),3,*(undefined4 *)(iVar1 + 0xa7c),iVar7);
    break;
  case 0x20b:
    uVar6 = 0xffffffff;
    goto LAB_10002396;
  case 0x20c:
    uVar6 = 0xffffffff;
    goto LAB_10002396;
  case 0x20d:
    uVar6 = (param_3 >> 0x10 != 1) + 0x6e;
LAB_10002396:
    bVar2 = FUN_10001310(bVar2);
    FUN_10001e20(this,CONCAT31(extraout_var_07,bVar2),uVar6);
    return 1;
  }
switchD_1000210f_caseD_1d:
  if (param_2 == *(uint *)((int)this + 0x1614)) {
    uVar5 = *(undefined4 *)((int)this + 0x160c);
    uVar8 = (uint)((int)uVar6 < 1);
    *(undefined4 *)(iVar1 + 0x6d4 + uVar8 * 4) = uVar5;
    *(undefined4 *)(iVar1 + 0x1e8 + uVar8 * 4) = uVar5;
    iVar7 = uVar8 + 0x70;
    FUN_10001a30(this,0,*(undefined4 *)((int)this + 0x160c),iVar7,iVar7,0);
    FUN_10001a30(this,1,*(undefined4 *)((int)this + 0x160c),iVar7,iVar7,0);
    iVar7 = (int)uVar6 / 0x78;
    *(int *)(iVar1 + 0xa7c) = *(int *)(iVar1 + 0xa7c) + iVar7;
    *(int *)(iVar1 + 0xa0c) = iVar7;
    FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),3,*(undefined4 *)(iVar1 + 0xa7c),iVar7);
  }
  if (*(WNDPROC *)((int)this + 8) == (WNDPROC)0x0) {
    return 0;
  }
  LVar4 = CallWindowProcA(*(WNDPROC *)((int)this + 8),param_1,param_2,uVar6,param_4);
  return LVar4;
}


// 10002630 FUN_10002630

undefined4 * __thiscall FUN_10002630(void *this,byte param_1)

{
  FUN_10001950((undefined4 *)this);
  if ((param_1 & 1) != 0) {
    FUN_10003a70(this);
  }
  return (undefined4 *)this;
}


// 10002660 FUN_10002660

void __thiscall FUN_10002660(void *this,HWND param_1)

{
  LONG LVar1;
  undefined2 uStack_c;
  undefined2 uStack_a;
  undefined4 uStack_8;
  undefined4 uStack_4;
  
  if (*(int *)((int)this + 0xc) != 0) {
    Warning("CInputSystem::AttachToWindow: Cannot attach to two windows at once!\n");
    return;
  }
  LVar1 = GetWindowLongW(param_1,-4);
  *(LONG *)((int)this + 8) = LVar1;
  SetWindowLongW(param_1,-4,0x10002650);
  *(HWND *)((int)this + 0xc) = param_1;
  if (*(char *)((int)this + 0x1d94) != '\0') {
    uStack_c = 1;
    uStack_a = 2;
    uStack_8 = 0x100;
    uStack_4 = DAT_1001817c;
    (*DAT_10018160)(&uStack_c,1,0xc);
  }
  FUN_10001850((int)this);
  return;
}


// 10002760 FUN_10002760

void FUN_10002760(void *param_1,void *param_2,char param_3)

{
  int iVar1;
  
  *(undefined4 *)((int)param_1 + 0xaec) = 0;
  *(undefined1 *)((int)param_1 + 0xaf4) = 0;
  if (*(char *)((int)param_2 + 0xaf4) != '\0') {
    _memcpy(param_1,param_2,0x28);
    *(undefined4 *)((int)param_1 + 0x28) = *(undefined4 *)((int)param_2 + 0x28);
    *(undefined4 *)((int)param_1 + 0x514) = *(undefined4 *)((int)param_2 + 0x514);
    *(undefined4 *)((int)param_1 + 0xa00) = *(undefined4 *)((int)param_2 + 0xa00);
    *(undefined4 *)((int)param_1 + 0xa70) = *(undefined4 *)((int)param_2 + 0xa70);
    if ((param_3 != '\0') && (iVar1 = *(int *)((int)param_2 + 0xaec), 0 < iVar1)) {
      if (*(int *)((int)param_1 + 0xaec) < iVar1) {
        FUN_100015a0((undefined4 *)((int)param_1 + 0xae0),*(int *)((int)param_1 + 0xaec),
                     iVar1 - *(int *)((int)param_1 + 0xaec),(undefined4 *)0x0);
      }
      _memcpy(*(void **)((int)param_1 + 0xae0),*(void **)((int)param_2 + 0xae0),
              *(int *)((int)param_2 + 0xaec) * 0x14);
    }
  }
  return;
}


// 10002810 FUN_10002810

void __fastcall FUN_10002810(int *param_1)

{
  param_1[0x584] = param_1[0x584] + 1;
  *(undefined1 *)((int)param_1 + 0x12) = 1;
  FUN_10002760(param_1 + 0x2c3,param_1 + 5,'\x01');
  (**(code **)(*param_1 + 0x58))();
  param_1[0x582] = param_1[0x583];
  FUN_10001c10(param_1);
  param_1[0x2c0] = 0;
  *(undefined1 *)(param_1 + 0x2c2) = 0;
  if ((char)param_1[0x580] != '\0') {
    _memcpy(param_1 + 5,param_1 + 0x2c3,0x28);
    param_1[0xf] = param_1[0x2cd];
    param_1[0x14a] = param_1[0x408];
    param_1[0x285] = param_1[0x543];
    param_1[0x2a1] = param_1[0x55f];
  }
  *(undefined1 *)((int)param_1 + 0x12) = 0;
  return;
}


// 100028b0 FUN_100028b0

void __fastcall FUN_100028b0(undefined4 *param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  int iVar3;
  
  puVar1 = param_1 + 5;
  *(undefined1 *)(param_1 + 1) = 1;
  *param_1 = &PTR_FUN_100122d4;
  puVar2 = param_1 + 0x2bf;
  iVar3 = 1;
  do {
    if (puVar1 != (undefined4 *)0x0) {
      *puVar1 = 0;
      puVar1[1] = 0;
      puVar1[2] = 0;
      puVar1[3] = 0;
      puVar1[4] = 0;
      puVar1[5] = 0;
      puVar1[6] = 0;
      puVar1[7] = 0;
      puVar1[8] = 0;
      puVar1[9] = 0;
    }
    puVar2[-2] = 0;
    puVar2[-1] = 0;
    *puVar2 = 0;
    puVar2[1] = 0;
    puVar2[2] = 0;
    puVar1 = puVar1 + 0x2be;
    puVar2 = puVar2 + 0x2be;
    iVar3 = iVar3 + -1;
  } while (-1 < iVar3);
  param_1[0x581] = 0;
  param_1[0x583] = 0;
  param_1[0x582] = 0;
  param_1[2] = 0;
  param_1[3] = 0;
  param_1[0x768] = 0;
  *(undefined1 *)(param_1 + 4) = 1;
  *(undefined1 *)((int)param_1 + 0x11) = 1;
  *(undefined1 *)((int)param_1 + 0x12) = 0;
  *(undefined2 *)(param_1 + 0x586) = 0;
  param_1[0x587] = 0;
  param_1[0x584] = 0;
  param_1[0x764] = 0xffffffff;
  param_1[0x585] = 0;
  *(undefined1 *)(param_1 + 0x588) = 0;
  *(undefined1 *)(param_1 + 0x765) = 0;
  param_1[0x769] = 0;
  param_1[0x76a] = 0;
  param_1[0x76b] = 0;
  return;
}


// 10002980 FUN_10002980

void __fastcall FUN_10002980(int *param_1)

{
  DWORD *pDVar1;
  code *pcVar2;
  MMRESULT MVar3;
  int iVar4;
  UINT uJoyID;
  joyinfoex_tag *pjVar5;
  DWORD *pDVar6;
  UINT UStack_1cc;
  joyinfoex_tag jStack_1c8;
  tagJOYCAPSA tStack_194;
  
  uJoyID = 0;
  param_1[0x587] = 0;
  UStack_1cc = joyGetNumDevs();
  if ((int)UStack_1cc < 5) {
    if ((int)UStack_1cc < 1) {
      DevMsg(1,"joystick not found -- driver not present\n");
      return;
    }
  }
  else {
    UStack_1cc = 4;
  }
  do {
    FUN_10005200(&jStack_1c8,0,0x34);
    jStack_1c8.dwSize = 0x34;
    jStack_1c8.dwFlags = 0x400;
    MVar3 = joyGetPosEx(uJoyID,&jStack_1c8);
    if (MVar3 == 0) {
      FUN_10005200(&tStack_194,0,0x194);
      MVar3 = joyGetDevCapsA(uJoyID,&tStack_194,0x194);
      if (MVar3 == 0) {
        pDVar1 = (DWORD *)(param_1 + param_1[0x587] * 0x1b + 0x589);
        pDVar1[0xf] = uJoyID;
        pjVar5 = &jStack_1c8;
        pDVar6 = pDVar1;
        for (iVar4 = 0xd; iVar4 != 0; iVar4 = iVar4 + -1) {
          *pDVar6 = pjVar5->dwSize;
          pjVar5 = (joyinfoex_tag *)&pjVar5->dwFlags;
          pDVar6 = pDVar6 + 1;
        }
        pDVar1[0xd] = tStack_194.wNumButtons;
        *(byte *)(pDVar1 + 0x10) = (byte)(tStack_194.wCaps >> 4) & 1;
        *(undefined1 *)((int)pDVar1 + 0x41) = 0;
        pDVar1[0x11] = 0x483;
        pDVar1[0xe] = 0;
        if (1 < tStack_194.wNumAxes) {
          pDVar1[0xe] = 3;
        }
        if ((char)pDVar1[0x10] != '\0') {
          pDVar1[0x11] = 0x4c3;
        }
        if ((tStack_194.wCaps & 1) != 0) {
          pDVar1[0x11] = pDVar1[0x11] | 4;
          pDVar1[0xe] = pDVar1[0xe] | 4;
        }
        if ((tStack_194.wCaps & 2) != 0) {
          pDVar1[0x11] = pDVar1[0x11] | 8;
          pDVar1[0xe] = pDVar1[0xe] | 8;
        }
        if ((tStack_194.wCaps & 4) != 0) {
          pDVar1[0x11] = pDVar1[0x11] | 0x10;
          pDVar1[0xe] = pDVar1[0xe] | 0x10;
        }
        if ((tStack_194.wCaps & 8) != 0) {
          pDVar1[0x11] = pDVar1[0x11] | 0x20;
          pDVar1[0xe] = pDVar1[0xe] | 0x20;
        }
        pDVar1[0x12] = 0;
        pDVar1[0x13] = 0;
        pDVar1[0x14] = 0;
        pDVar1[0x15] = 0;
        pDVar1[0x16] = 0;
        pDVar1[0x17] = 0;
        pDVar1[0x18] = 0;
        pDVar1[0x19] = 0;
        pDVar1[0x1a] = 0;
        pcVar2 = *(code **)(*param_1 + 0x50);
        param_1[0x587] = param_1[0x587] + 1;
        (*pcVar2)(uJoyID,1);
      }
    }
    uJoyID = uJoyID + 1;
  } while ((int)uJoyID < (int)UStack_1cc);
  return;
}


// 10002b50 FUN_10002b50

void __thiscall FUN_10002b50(void *this,int param_1)

{
  float fVar1;
  float fVar2;
  float fVar3;
  int iVar4;
  uint uVar5;
  int iVar6;
  uint uVar7;
  int iVar8;
  int iVar9;
  int local_8;
  
  iVar4 = param_1 * 0x6c;
  uVar7 = *(uint *)(iVar4 + 0x166c + (int)this) ^ *(uint *)(iVar4 + 0x1644 + (int)this);
  if (uVar7 != 0) {
    iVar9 = 0;
    if (0 < *(int *)((int)this + iVar4 + 0x1658)) {
      do {
        uVar5 = 1 << ((byte)iVar9 & 0x1f) & uVar7;
        if (uVar5 != 0) {
          iVar8 = param_1 * 0x20 + 0x72 + iVar9;
          if ((*(uint *)((int)this + iVar4 + 0x1644) & uVar5) == 0) {
            FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar8,iVar8);
          }
          else {
            FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar8,iVar8);
          }
        }
        iVar9 = iVar9 + 1;
      } while (iVar9 < *(int *)((int)this + iVar4 + 0x1658));
    }
    *(undefined4 *)((int)this + iVar4 + 0x166c) = *(undefined4 *)((int)this + iVar4 + 0x1644);
  }
  fVar1 = *(float *)(DAT_10019fc4 + 0x2c) * 32768.0;
  local_8 = 0;
  do {
    if ((*(uint *)((int)this + iVar4 + 0x165c) & 1 << ((byte)local_8 & 0x1f)) != 0) {
                    /* WARNING: Load size is inaccurate */
      uVar7 = 1 << ((byte)local_8 * '\x02' & 0x1f);
      iVar8 = (param_1 * 6 + local_8) * 2;
      iVar9 = iVar8 + 0x102;
      iVar6 = (**(code **)(*this + 0x38))(param_1 * 6 + 4 + local_8);
      fVar2 = (float)iVar6;
      if ((fVar1 < fVar2) && (uVar5 = *(uint *)((int)this + iVar4 + 0x1670), (uVar7 & uVar5) == 0))
      {
        *(uint *)((int)this + iVar4 + 0x1670) = uVar5 | uVar7;
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar9,iVar9);
      }
      if ((fVar2 <= fVar1) && (uVar5 = *(uint *)((int)this + iVar4 + 0x1670), (uVar7 & uVar5) != 0))
      {
        *(uint *)((int)this + iVar4 + 0x1670) = ~uVar7 & uVar5;
        FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar9,iVar9);
      }
      uVar7 = uVar7 * 2;
      fVar3 = -fVar1;
      iVar8 = iVar8 + 0x103;
      if ((fVar2 < fVar3) && (uVar5 = *(uint *)((int)this + iVar4 + 0x1670), (uVar7 & uVar5) == 0))
      {
        *(uint *)((int)this + iVar4 + 0x1670) = uVar5 | uVar7;
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar8,iVar8);
      }
      if ((fVar3 < fVar2 != (fVar3 == fVar2)) &&
         (uVar5 = *(uint *)((int)this + iVar4 + 0x1670), (uVar7 & uVar5) != 0)) {
        *(uint *)((int)this + iVar4 + 0x1670) = ~uVar7 & uVar5;
        FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar8,iVar8);
      }
    }
    local_8 = local_8 + 1;
  } while (local_8 < 6);
  return;
}


// 10002d40 FUN_10002d40

undefined4 FUN_10002d40(undefined4 param_1,int param_2)

{
  switch(param_1) {
  default:
    return *(undefined4 *)(param_2 + 8);
  case 1:
    return *(undefined4 *)(param_2 + 0xc);
  case 2:
    return *(undefined4 *)(param_2 + 0x10);
  case 3:
    return *(undefined4 *)(param_2 + 0x14);
  case 4:
    return *(undefined4 *)(param_2 + 0x18);
  case 5:
    return *(undefined4 *)(param_2 + 0x1c);
  }
}


// 10002db0 FUN_10002db0

void __thiscall FUN_10002db0(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  uint uVar6;
  
  iVar2 = param_1 * 0x6c;
  if (*(char *)(iVar2 + 0x1664 + (int)this) != '\0') {
    iVar5 = *(int *)((int)this + iVar2 + 0x164c);
    uVar6 = 0;
    if (iVar5 != 0xffff) {
      uVar6 = (uint)(iVar5 == 0);
      if (iVar5 == 9000) {
        uVar6 = uVar6 | 2;
      }
      if (iVar5 == 18000) {
        uVar6 = uVar6 | 4;
      }
      if (iVar5 == 27000) {
        uVar6 = uVar6 | 8;
      }
      if (*(char *)((int)this + iVar2 + 0x1665) != '\0') {
        if (iVar5 == 0x1194) {
          uVar6 = uVar6 | 3;
        }
        if (iVar5 == 0x34bc) {
          uVar6 = uVar6 | 6;
        }
        if (iVar5 == 0x57e4) {
          uVar6 = uVar6 | 0xc;
        }
        if (iVar5 == 0x34bc) {
          uVar6 = uVar6 | 9;
        }
      }
    }
    uVar4 = *(uint *)((int)this + iVar2 + 0x1674) ^ uVar6;
    if (uVar4 != 0) {
      iVar5 = 0;
      do {
        uVar3 = 1 << ((byte)iVar5 & 0x1f) & uVar4;
        if (uVar3 != 0) {
          iVar1 = iVar5 + 0xf2 + param_1 * 4;
          if ((uVar6 & uVar3) == 0) {
            FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar1,iVar1);
          }
          else {
            FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar1,iVar1);
          }
        }
        iVar5 = iVar5 + 1;
      } while (iVar5 < 4);
      *(uint *)((int)this + iVar2 + 0x1674) = uVar6;
    }
  }
  return;
}


// 10002ea0 FUN_10002ea0

void __fastcall FUN_10002ea0(void *param_1)

{
  LPJOYINFOEX pji;
  int iVar1;
  UINT uJoyID;
  undefined4 uVar2;
  int iVar3;
  MMRESULT MVar4;
  int iVar5;
  int iVar6;
  int iVar7;
  undefined4 *puVar8;
  int local_8;
  int local_4;
  
  if (*(short *)((int)param_1 + 0x1618) != 0) {
    iVar3 = (uint)*(byte *)((int)param_1 + 0x12) * 0xaf8;
    local_8 = 0;
    if (0 < *(int *)((int)param_1 + 0x161c)) {
      puVar8 = (undefined4 *)((int)param_1 + 0x1628);
      local_4 = 4;
      do {
        if (((uint)*(ushort *)((int)param_1 + 0x1618) & 1 << ((byte)local_8 & 0x1f)) != 0) {
          pji = (LPJOYINFOEX)(puVar8 + -1);
          FUN_10005200(pji,0,0x34);
          uJoyID = puVar8[0xe];
          uVar2 = puVar8[0x10];
          pji->dwSize = 0x34;
          *puVar8 = uVar2;
          MVar4 = joyGetPosEx(uJoyID,pji);
          if (MVar4 == 0) {
            if (*(int *)(DAT_1001a00c + 0x30) != 0) {
              puVar8[5] = puVar8[5] + 100;
            }
            iVar7 = 0;
            do {
              if ((puVar8[0xd] & 1 << ((byte)iVar7 & 0x1f)) != 0) {
                iVar1 = local_4 + iVar7;
                iVar5 = FUN_10002d40(iVar7,(int)(puVar8 + -1));
                iVar5 = iVar5 + -0x8000;
                iVar6 = iVar5 - *(int *)((int)param_1 + iVar1 * 4 + iVar3 + 0xa84);
                *(int *)((int)param_1 + iVar1 * 4 + iVar3 + 0xa84) = iVar5;
                *(int *)((int)param_1 + iVar1 * 4 + iVar3 + 0xa14) = iVar6;
                if (iVar6 != 0) {
                  FUN_10001a30(param_1,3,*(undefined4 *)((int)param_1 + 0x160c),iVar1,iVar5,iVar6);
                }
              }
              iVar7 = iVar7 + 1;
            } while (iVar7 < 6);
            FUN_10002b50(param_1,local_8);
            FUN_10002db0(param_1,local_8);
          }
        }
        local_4 = local_4 + 6;
        local_8 = local_8 + 1;
        puVar8 = puVar8 + 0x1b;
      } while (local_8 < *(int *)((int)param_1 + 0x161c));
    }
  }
  return;
}


// 10002ff0 FUN_10002ff0

undefined4 __cdecl FUN_10002ff0(int param_1)

{
  if ((((param_1 - 1U < 0x41) && (0x24 < param_1)) && (param_1 != 0x40)) &&
     (((param_1 != 0x43 && (param_1 != 0x41)) && ((param_1 < 0x6b || (0x34 < param_1)))))) {
    return 1;
  }
  return 0;
}


// 10003030 FUN_10003030

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10003030(void)

{
  uint uVar1;
  
  _memset(&DAT_1001a038,0,0x400);
  _DAT_1001a0f8 = 1;
  _DAT_1001a0fc = 2;
  _DAT_1001a100 = 3;
  _DAT_1001a104 = 4;
  _DAT_1001a108 = 5;
  _DAT_1001a10c = 6;
  _DAT_1001a110 = 7;
  _DAT_1001a114 = 8;
  _DAT_1001a118 = 9;
  _DAT_1001a11c = 10;
  _DAT_1001a13c = 0xb;
  _DAT_1001a140 = 0xc;
  _DAT_1001a144 = 0xd;
  _DAT_1001a148 = 0xe;
  _DAT_1001a14c = 0xf;
  _DAT_1001a150 = 0x10;
  _DAT_1001a154 = 0x11;
  _DAT_1001a158 = 0x12;
  _DAT_1001a15c = 0x13;
  _DAT_1001a160 = 0x14;
  _DAT_1001a164 = 0x15;
  _DAT_1001a168 = 0x16;
  _DAT_1001a16c = 0x17;
  _DAT_1001a170 = 0x18;
  _DAT_1001a174 = 0x19;
  _DAT_1001a178 = 0x1a;
  _DAT_1001a17c = 0x1b;
  _DAT_1001a180 = 0x1c;
  _DAT_1001a184 = 0x1d;
  _DAT_1001a188 = 0x1e;
  _DAT_1001a18c = 0x1f;
  _DAT_1001a190 = 0x20;
  _DAT_1001a194 = 0x21;
  _DAT_1001a198 = 0x22;
  _DAT_1001a19c = 0x23;
  _DAT_1001a1a0 = 0x24;
  _DAT_1001a1b8 = 0x25;
  _DAT_1001a1bc = 0x26;
  _DAT_1001a1c0 = 0x27;
  _DAT_1001a1c4 = 0x28;
  _DAT_1001a1c8 = 0x29;
  _DAT_1001a1cc = 0x2a;
  _DAT_1001a1d0 = 0x2b;
  _DAT_1001a1d4 = 0x2c;
  _DAT_1001a1d8 = 0x2d;
  _DAT_1001a1dc = 0x2e;
  _DAT_1001a1f4 = 0x2f;
  _DAT_1001a1e0 = 0x30;
  _DAT_1001a1ec = 0x31;
  _DAT_1001a1e4 = 0x32;
  _DAT_1001a1f0 = 0x34;
  _DAT_1001a3a4 = 0x35;
  _DAT_1001a3ac = 0x36;
  _DAT_1001a320 = 0x37;
  _DAT_1001a3b0 = 0x38;
  _DAT_1001a338 = 0x39;
  _DAT_1001a328 = 0x3a;
  _DAT_1001a330 = 0x3b;
  _DAT_1001a334 = 0x3c;
  _DAT_1001a3a8 = 0x3d;
  _DAT_1001a32c = 0x3e;
  _DAT_1001a324 = 0x3f;
  _DAT_1001a06c = 0x40;
  _DAT_1001a0b8 = 0x41;
  _DAT_1001a058 = 0x42;
  _DAT_1001a05c = 0x43;
  _DAT_1001a088 = 0x44;
  _DAT_1001a278 = 0x45;
  _DAT_1001a0a4 = 0x46;
  _DAT_1001a27c = 0x47;
  _DAT_1001a0ec = 0x48;
  _DAT_1001a0f0 = 0x49;
  _DAT_1001a0c8 = 0x4a;
  _DAT_1001a0c4 = 0x4b;
  _DAT_1001a0bc = 0x4c;
  _DAT_1001a0c0 = 0x4d;
  _DAT_1001a084 = 0x4e;
  _DAT_1001a078 = 0x4f;
  _DAT_1001a080 = 0x51;
  _DAT_1001a07c = 0x53;
  _DAT_1001a1a4 = 0x55;
  _DAT_1001a1a8 = 0x56;
  _DAT_1001a1ac = 0x57;
  _DAT_1001a0d0 = 0x58;
  _DAT_1001a0cc = 0x59;
  _DAT_1001a0d8 = 0x5a;
  _DAT_1001a0d4 = 0x5b;
  _DAT_1001a1f8 = 0x5c;
  _DAT_1001a1fc = 0x5d;
  _DAT_1001a200 = 0x5e;
  _DAT_1001a204 = 0x5f;
  _DAT_1001a208 = 0x60;
  _DAT_1001a20c = 0x61;
  _DAT_1001a210 = 0x62;
  _DAT_1001a214 = 99;
  _DAT_1001a218 = 100;
  _DAT_1001a21c = 0x65;
  _DAT_1001a220 = 0x66;
  _DAT_1001a224 = 0x67;
  _DAT_1001ab28 = 0;
  _DAT_1001ab2c = 0xf2;
  _DAT_1001ab30 = 0xf4;
  _DAT_1001ab34 = 0xf5;
  _DAT_1001ab38 = 0xf3;
  _DAT_1001ab3c = 0x79;
  _DAT_1001ab40 = 0x78;
  _DAT_1001ab44 = 0x7a;
  _DAT_1001ab48 = 0x7b;
  _DAT_1001ab4c = 0x72;
  _DAT_1001ab50 = 0x73;
  _DAT_1001ab54 = 0x74;
  _DAT_1001ab58 = 0x75;
  _DAT_1001ab5c = 0x76;
  _DAT_1001ab60 = 0x77;
  _DAT_1001ab64 = 0x106;
  _DAT_1001ab68 = 0x107;
  _DAT_1001ab6c = 0x105;
  _DAT_1001ab70 = 0x104;
  _DAT_1001ab74 = 0x103;
  _DAT_1001ab78 = 0x102;
  _DAT_1001ab7c = 0x10b;
  _DAT_1001ab80 = 0x10a;
  _DAT_1001ab84 = 0x109;
  _DAT_1001ab88 = 0x108;
  _DAT_1001ab8c = 0x7c;
  uVar1 = 0;
  do {
    *(uint *)(&DAT_1001a438 + (&DAT_1001a038)[uVar1] * 4) = uVar1;
    uVar1 = uVar1 + 1;
  } while (uVar1 < 0x100);
  _DAT_1001a438 = 0;
  return;
}


// 10003560 FUN_10003560

undefined4 __cdecl FUN_10003560(uint param_1)

{
  if ((-1 < (int)param_1) && (param_1 < 0x100)) {
    return (&DAT_1001a038)[param_1];
  }
  return 0;
}


// 10003580 FUN_10003580

undefined4 __cdecl FUN_10003580(int param_1)

{
  return *(undefined4 *)(&DAT_1001a438 + param_1 * 4);
}


// 10003590 FUN_10003590

int __cdecl FUN_10003590(int param_1,uint param_2)

{
  int iVar1;
  
  if (((int)param_2 < 0) || (0x19 < param_2)) {
    return 0;
  }
  iVar1 = *(int *)(&DAT_1001ab28 + param_2 * 4);
  if (((iVar1 < 0x72) || (0xf1 < iVar1)) && (7 < iVar1 - 0x133U)) {
    if (iVar1 - 0xf2U < 0x10) {
      return iVar1 + param_1 * 4;
    }
    if (iVar1 - 0x102U < 0x30) {
      return iVar1 + param_1 * 0xc;
    }
    return iVar1;
  }
  return param_1 * 0x20 + iVar1;
}


// 10003600 FUN_10003600

undefined * __cdecl FUN_10003600(int param_1,char param_2)

{
  uint uVar1;
  
  if (param_2 != '\0') {
    if (((0x71 < param_1) && (param_1 < 0xf2)) || (param_1 - 0x133U < 8)) {
      uVar1 = param_1 - 0x72U & 0x8000001f;
      if ((int)uVar1 < 0) {
        uVar1 = (uVar1 - 1 | 0xffffffe0) + 1;
      }
      return (&PTR_s_A_BUTTON_10016690)[uVar1];
    }
    if (param_1 - 0xf2U < 0x10) {
      uVar1 = param_1 - 0xf2U & 0x80000003;
      if ((int)uVar1 < 0) {
        uVar1 = (uVar1 - 1 | 0xfffffffc) + 1;
      }
      return (&PTR_DAT_10016890)[uVar1];
    }
    if (param_1 - 0x102U < 0x30) {
      return (&PTR_s_S1_RIGHT_100168d0)[(param_1 + -0x102) % 0xc];
    }
  }
  return (&PTR_DAT_10016130)[param_1];
}


// 10003690 FUN_10003690

undefined * __cdecl FUN_10003690(int param_1)

{
  return (&PTR_s_MOUSE_X_10016620)[param_1];
}


// 100036a0 FUN_100036a0

int __cdecl FUN_100036a0(byte *param_1,char param_2)

{
  int iVar1;
  long lVar2;
  uint uVar3;
  uint uVar4;
  
  if ((param_1 != (byte *)0x0) && (*param_1 != 0)) {
    iVar1 = FUN_10004fd0((char *)param_1,"aux",3);
    if (iVar1 != 0) {
      iVar1 = 0;
      do {
        uVar3 = FUN_10004ed0((&PTR_DAT_10016130)[iVar1],param_1);
        if (uVar3 == 0) {
          return iVar1;
        }
        iVar1 = iVar1 + 1;
      } while (iVar1 < 0x13b);
      if (param_2 != '\0') {
        uVar3 = 0;
        do {
          uVar4 = FUN_10004ed0((&PTR_s_A_BUTTON_10016690)[uVar3],param_1);
          if (uVar4 == 0) {
            return uVar3 + 0x72;
          }
          uVar3 = uVar3 + 1;
        } while (uVar3 < 0xc0);
      }
      return -1;
    }
    lVar2 = _atol((char *)(param_1 + 3));
    if (lVar2 < 0x1d) {
      return lVar2 + 0x72;
    }
    if (lVar2 < 0x21) {
      return lVar2 + 0xd5;
    }
  }
  return -1;
}


// 10003760 FUN_10003760

int __cdecl FUN_10003760(byte *param_1)

{
  uint uVar1;
  int iVar2;
  
  if ((param_1 != (byte *)0x0) && (*param_1 != 0)) {
    iVar2 = 0;
    do {
      uVar1 = FUN_10004ed0((&PTR_s_MOUSE_X_10016620)[iVar2],param_1);
      if (uVar1 == 0) {
        return iVar2;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 0x1c);
    return -1;
  }
  return -1;
}


// 100037a0 FUN_100037a0

undefined4 __cdecl FUN_100037a0(uint param_1)

{
  uint uVar1;
  undefined4 uVar2;
  
  uVar1 = (int)param_1 >> 0x10 & 0xff;
  if (0x7f < uVar1) {
    return 0;
  }
  uVar2 = (&DAT_1001a928)[uVar1];
  if ((param_1 >> 0x18 & 1) == 0) {
    switch(uVar2) {
    case 0x48:
      return 0x25;
    case 0x49:
      return 0x34;
    case 0x4a:
      return 0x2c;
    case 0x4b:
      return 0x26;
    case 0x4c:
      return 0x2e;
    case 0x4d:
      return 0x28;
    case 0x58:
      return 0x2d;
    case 0x59:
      return 0x29;
    case 0x5a:
      return 0x27;
    case 0x5b:
      return 0x2b;
    }
  }
  else {
    switch(uVar2) {
    case 0x3c:
      return 0x2f;
    case 0x40:
      return 0x33;
    case 0x44:
      uVar2 = 0x32;
      break;
    case 0x51:
      return 0x52;
    case 0x53:
      return 0x54;
    }
  }
  return uVar2;
}


// 100038c0 FUN_100038c0

void FUN_100038c0(void)

{
  HKL dwhkl;
  HKL dwhkl_00;
  undefined4 uVar1;
  UINT UVar2;
  int iVar3;
  undefined4 *puVar4;
  undefined4 *puVar5;
  UINT uCode;
  
  puVar4 = &DAT_10016990;
  puVar5 = &DAT_1001a928;
  for (iVar3 = 0x80; iVar3 != 0; iVar3 = iVar3 + -1) {
    *puVar5 = *puVar4;
    puVar4 = puVar4 + 1;
    puVar5 = puVar5 + 1;
  }
  uCode = 0;
  dwhkl = GetKeyboardLayout(0);
  dwhkl_00 = LoadKeyboardLayoutA("00000409",0);
  if ((dwhkl_00 != (HKL)0x0) && (dwhkl_00 != dwhkl)) {
    do {
      iVar3 = (&DAT_10016990)[uCode];
      if (((iVar3 != 0) && (iVar3 != 0x39)) &&
         ((iVar3 - 1U < 0x24 || (uVar1 = FUN_10002ff0(iVar3), (char)uVar1 != '\0')))) {
        UVar2 = MapVirtualKeyExA(uCode,1,dwhkl_00);
        UVar2 = MapVirtualKeyExA(UVar2,0,dwhkl);
        (&DAT_1001a928)[UVar2 & 0x7f] = iVar3;
      }
      uCode = uCode + 1;
    } while (uCode < 0x80);
    DAT_1001a928 = 0;
    return;
  }
  DAT_1001a928 = 0;
  return;
}


// 10003980 FUN_10003980

char * FUN_10003980(void)

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
    VirtualQuery(&DAT_1001abc8,&_Stack_1c,0x1c);
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


// 10003a10 FUN_10003a10

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10003a10(undefined4 param_1)

{
  if ((_DAT_1001abd0 & 1) == 0) {
    _DAT_1001abd0 = _DAT_1001abd0 | 1;
    DAT_1001abcc = FUN_10003980();
  }
  if (DAT_1001abcc != (char *)0x0) {
    (**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_1001abcc,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  return;
}


// 10003a70 FUN_10003a70

void __cdecl FUN_10003a70(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1);
  return;
}


// 10003a90 FUN_10003a90

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_10003a90(int param_1,int param_2)

{
  void *pvVar1;
  size_t _Size;
  
  _Size = param_1 * param_2;
  if ((_DAT_1001abd0 & 1) == 0) {
    _DAT_1001abd0 = _DAT_1001abd0 | 1;
    DAT_1001abcc = FUN_10003980();
  }
  if (DAT_1001abcc != (char *)0x0) {
    pvVar1 = (void *)(**(code **)**(undefined4 **)g_pMemAlloc_exref)(_Size,DAT_1001abcc,0);
    _memset(pvVar1,0,_Size);
    return pvVar1;
  }
  pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(_Size);
  _memset(pvVar1,0,_Size);
  return pvVar1;
}


// 10003b10 FUN_10003b10

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_10003b10(size_t param_1)

{
  void *pvVar1;
  
  if ((_DAT_1001abd0 & 1) == 0) {
    _DAT_1001abd0 = _DAT_1001abd0 | 1;
    DAT_1001abcc = FUN_10003980();
  }
  if (DAT_1001abcc != (char *)0x0) {
    pvVar1 = (void *)(**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_1001abcc,0);
    _memset(pvVar1,0,param_1);
    return pvVar1;
  }
  pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  _memset(pvVar1,0,param_1);
  return pvVar1;
}


// 10003b90 FUN_10003b90

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10003b90(undefined4 param_1,undefined4 param_2)

{
  if ((_DAT_1001abd8 & 1) == 0) {
    _DAT_1001abd8 = _DAT_1001abd8 | 1;
    DAT_1001abd4 = FUN_10003980();
  }
  if (DAT_1001abd4 != (char *)0x0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 8))(param_1,param_2,DAT_1001abd4,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(param_1,param_2);
  return;
}


// 10003bf0 FUN_10003bf0

void __cdecl FUN_10003bf0(int param_1,int param_2)

{
  FUN_10003b10(param_1 * param_2);
  return;
}


// 10003c20 FUN_10003c20

void __cdecl FUN_10003c20(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x20))(param_1);
  return;
}


// 10003c40 FUN_10003c40

bool FUN_10003c40(void)

{
  return *(int *)g_pMemAlloc_exref != 0;
}


// 10003c50 FUN_10003c50

void FUN_10003c50(void)

{
  return;
}


// 10003c70 FUN_10003c70

void FUN_10003c70(void)

{
  if (DAT_1001abe4 != (code *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x10003c79. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (*DAT_1001abe4)();
    return;
  }
  return;
}


// 10003c80 FUN_10003c80

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __fastcall FUN_10003c80(int param_1)

{
  int iVar1;
  
  _DAT_1001ac00 = param_1;
  *(undefined1 *)(param_1 + 0x17d8) = 0;
  *(undefined4 *)(param_1 + 0x17d4) = 0;
  if (*(HMODULE *)(param_1 + 0x1dac) != (HMODULE)0x0) {
    DAT_1001abdc = GetProcAddress(*(HMODULE *)(param_1 + 0x1dac),"NovintGetIHaptics");
    _DAT_1001abe0 = GetProcAddress(*(HMODULE *)(param_1 + 0x1dac),"NovintAttemptHWND");
    DAT_1001abe4 = GetProcAddress(*(HMODULE *)(param_1 + 0x1dac),"NovintDisableHWND");
    DAT_1001abe8 = GetProcAddress(*(HMODULE *)(param_1 + 0x1dac),"NovintPollDevices");
    DAT_1001abec = GetProcAddress(*(HMODULE *)(param_1 + 0x1dac),"NovintButtonState");
    DAT_1001abf0 = GetProcAddress(*(HMODULE *)(param_1 + 0x1dac),"NovintDeviceCount");
    _DAT_1001abf4 = GetProcAddress(*(HMODULE *)(param_1 + 0x1dac),"NovintButtonCount");
    _DAT_1001abfc = GetProcAddress(*(HMODULE *)(param_1 + 0x1dac),"NovintGetDeviceID");
    DAT_1001abf8 = GetProcAddress(*(HMODULE *)(param_1 + 0x1dac),"NovintInputActive");
    iVar1 = (*DAT_1001abf0)();
    *(int *)(param_1 + 0x17d4) = iVar1;
    if (0 < iVar1) {
      *(undefined1 *)(param_1 + 0x17d8) = 1;
    }
  }
  return;
}


// 10003d70 FUN_10003d70

void __thiscall FUN_10003d70(void *this,uint param_1)

{
  char cVar1;
  uint uVar2;
  int iVar3;
  int iVar4;
  uint local_8;
  undefined1 local_4 [4];
  
  uVar2 = param_1;
  cVar1 = (*DAT_1001abec)(param_1,local_4,&param_1,&local_8);
  if (cVar1 != '\0') {
    iVar3 = 0;
    iVar4 = uVar2 * 4 + 0x133;
    do {
      uVar2 = 1 << ((byte)iVar3 & 0x1f);
      if ((param_1 & uVar2) != 0) {
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar4,0);
      }
      if ((local_8 & uVar2) != 0) {
        FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar4,0);
      }
      iVar3 = iVar3 + 1;
      iVar4 = iVar4 + 1;
    } while (iVar3 < 4);
  }
  return;
}


// 10003e20 FUN_10003e20

void __fastcall FUN_10003e20(void *param_1)

{
  uint uVar1;
  
  if (DAT_1001abe8 != (code *)0x0) {
    (*DAT_1001abe8)();
  }
  uVar1 = 0;
  if (0 < *(int *)((int)param_1 + 0x17d4)) {
    do {
      FUN_10003d70(param_1,uVar1);
      uVar1 = uVar1 + 1;
    } while ((int)uVar1 < *(int *)((int)param_1 + 0x17d4));
  }
  return;
}


// 10003e60 FUN_10003e60

void __thiscall FUN_10003e60(void *this,undefined4 *param_1,undefined4 param_2)

{
  int iVar1;
  undefined4 unaff_ESI;
  undefined1 local_14 [20];
  
  *param_1 = 0xffffffff;
  *(undefined1 *)(param_1 + 2) = 0;
  iVar1 = (*DAT_1001ac18)(param_2,1,local_14);
  if ((iVar1 == 0) && ((char)((uint)unaff_ESI >> 8) == '\x01')) {
    param_1[0x18] = 0x3fa85148;
    param_1[0x19] = 0x3fa851b2;
    param_1[0x10] = 0x1ea9;
    param_1[0x11] = 0x1ea9;
    param_1[0x1a] = 0x3fa85148;
    param_1[0x12] = 0x1ea9;
    param_1[0x13] = 0x1ea9;
    param_1[0x1b] = 0x3fa851b2;
    *(char *)(param_1 + 1) = (char)unaff_ESI;
    param_1[0x1c] = 0x3fae3076;
    *(undefined1 *)((int)param_1 + 5) = 1;
    *(short *)((int)param_1 + 6) = (short)((uint)unaff_ESI >> 0x10);
    param_1[0x1d] = 0x3fae30f3;
    *param_1 = param_2;
    *(undefined1 *)(param_1 + 2) = 1;
    param_1[0x1e] = 0x3fae3076;
    param_1[0x20] = 0;
    param_1[0x21] = 0;
    param_1[0x1f] = 0x3fae30f3;
    param_1[0x14] = 0x21f1;
    param_1[0x15] = 0x21f1;
    param_1[0x16] = 0x21f1;
    param_1[0x17] = 0x21f1;
    *(undefined2 *)(param_1 + 0x22) = 0;
    *(undefined2 *)((int)param_1 + 0x8a) = 0;
    *(undefined1 *)(param_1 + 0x23) = 0;
    *(int *)((int)this + 0x161c) = *(int *)((int)this + 0x161c) + 1;
  }
  return;
}


// 10003f40 FUN_10003f40

void __fastcall FUN_10003f40(void *param_1,int param_2,int param_3,uint param_4,int param_5)

{
  int *piVar1;
  int iVar2;
  int iVar3;
  undefined4 extraout_ECX;
  int iVar4;
  ulonglong uVar5;
  int local_4;
  
  local_4 = 0;
  iVar3 = param_5;
  switch(param_4) {
  case 0x11:
  case 0x12:
    iVar4 = param_3 * 6 + 5;
    if (param_4 == 0x11) {
      iVar3 = param_2;
    }
    break;
  case 0x13:
  case 0x14:
    iVar4 = param_3 * 6 + 4;
    if (param_4 == 0x13) {
      iVar3 = param_2;
    }
    break;
  case 0x15:
  case 0x16:
    iVar4 = param_3 * 6 + 7;
    if (param_4 == 0x15) {
      iVar3 = param_2;
    }
    break;
  case 0x17:
  case 0x18:
    iVar4 = param_3 * 6 + 8;
    iVar3 = param_5;
    if (param_4 == 0x17) {
      iVar3 = param_2;
    }
    break;
  default:
    goto switchD_10003f62_default;
  }
  local_4 = 0x3333;
  if (iVar4 != 0x1c) {
    iVar2 = (uint)*(byte *)((int)param_1 + 0x12) * 0xaf8;
    uVar5 = FUN_10006e40(param_4,iVar3);
    *(int *)((int)param_1 + iVar4 * 4 + iVar2 + 0xa14) = (int)uVar5;
    uVar5 = FUN_10006e40(extraout_ECX,(int)(uVar5 >> 0x20));
    iVar3 = *(int *)((int)param_1 + iVar4 * 4 + iVar2 + 0xa14);
    *(int *)((int)param_1 + iVar4 * 4 + iVar2 + 0xa84) = (int)uVar5;
    if (iVar3 != 0) {
      FUN_10001a30(param_1,3,*(undefined4 *)((int)param_1 + 0x160c),iVar4,(int)uVar5,0);
    }
  }
switchD_10003f62_default:
  iVar3 = param_3 * 0x1a + param_4;
  *(int *)((int)param_1 + iVar3 * 8 + 0x1814) = param_5;
  if (local_4 < param_5) {
    piVar1 = (int *)((int)param_1 + iVar3 * 8 + 0x1810);
    *piVar1 = *piVar1 + 1;
  }
  else {
    *(undefined4 *)((int)param_1 + iVar3 * 8 + 0x1810) = 0;
    param_5 = 0;
  }
  if (*(int *)((int)param_1 + iVar3 * 8 + 0x1810) < 2) {
    iVar3 = FUN_10001120(param_1,param_3,param_4);
    if (param_5 != 0) {
      FUN_10001a90(param_1,0,*(undefined4 *)((int)param_1 + 0x160c),iVar3,iVar3);
      return;
    }
    FUN_10001af0(param_1,1,*(undefined4 *)((int)param_1 + 0x160c),iVar3,iVar3);
  }
  return;
}


// 10004120 FUN_10004120

void __thiscall FUN_10004120(void *this,int *param_1)

{
  int *piVar1;
  int *piVar2;
  ushort uVar3;
  DWORD DVar4;
  uint uVar5;
  int iVar6;
  int iVar7;
  int *piVar8;
  int iVar9;
  undefined4 extraout_ECX;
  undefined4 extraout_ECX_00;
  undefined4 extraout_ECX_01;
  undefined4 extraout_ECX_02;
  undefined4 extraout_ECX_03;
  undefined4 extraout_ECX_04;
  undefined4 extraout_ECX_05;
  undefined4 extraout_ECX_06;
  undefined4 extraout_ECX_07;
  int *piVar10;
  int extraout_EDX;
  int extraout_EDX_00;
  undefined4 uVar11;
  undefined4 extraout_EDX_01;
  int extraout_EDX_02;
  int extraout_EDX_03;
  int extraout_EDX_04;
  int extraout_EDX_05;
  undefined4 extraout_EDX_06;
  int extraout_EDX_07;
  int extraout_EDX_08;
  int extraout_EDX_09;
  int extraout_EDX_10;
  int extraout_EDX_11;
  int extraout_EDX_12;
  int extraout_EDX_13;
  uint uVar12;
  int iVar13;
  int iVar14;
  uint uVar15;
  ulonglong uVar16;
  ulonglong uVar17;
  
  uVar12 = param_1[0xb];
  piVar2 = param_1 + uVar12 * 4 + 3;
  piVar1 = param_1 + (uVar12 ^ 1) * 4 + 3;
  if ((short)param_1[uVar12 * 4 + 4] == 0x30) {
    if (param_1[0x20] == 0) {
      DVar4 = GetTickCount();
      param_1[0x20] = DVar4;
    }
    else {
      DVar4 = GetTickCount();
      if (2000 < DVar4 - param_1[0x20]) {
        param_1[0x20] = 0;
        FUN_10001160(0x464,0,"quit_x360");
      }
    }
  }
  else {
    param_1[0x20] = 0;
  }
  if (*piVar1 == *piVar2) {
    return;
  }
  uVar12 = (uint)(*(ushort *)(piVar1 + 1) ^ *(ushort *)(piVar2 + 1));
  if (uVar12 != 0) {
    uVar3 = *(ushort *)(piVar2 + 1) & 0xf;
    uVar15 = 0;
    do {
      piVar8 = (int *)(*(uint *)((int)&DAT_10016bb8 + uVar15) & uVar12);
      if (piVar8 != (int *)0x0) {
        uVar5 = -(uint)(((uint)*(ushort *)(piVar2 + 1) & (uint)piVar8) != 0) & 0x8000;
        piVar10 = piVar2;
        if (((uint)piVar8 & 0xf) != 0) {
          if ((uVar3 & uVar3 - 1) == 0) {
            if (uVar5 != 0) {
              param_1[0x21] = (int)piVar8;
            }
          }
          else {
            piVar10 = (int *)param_1[0x21];
            if ((piVar10 != (int *)0x0) && (piVar10 != piVar8)) goto LAB_10004225;
          }
        }
        FUN_10003f40(this,(int)piVar10,*param_1,*(uint *)((int)&DAT_10016bbc + uVar15),uVar5);
      }
LAB_10004225:
      uVar15 = uVar15 + 8;
    } while (uVar15 < 0x70);
  }
  if (*(byte *)((int)piVar2 + 6) < 0x1f) {
    *(undefined1 *)((int)piVar2 + 6) = 0;
  }
  if ((uint)*(byte *)((int)piVar2 + 6) != (uint)*(byte *)((int)piVar1 + 6)) {
    uVar16 = FUN_10006e40((uint)*(byte *)((int)piVar1 + 6),piVar1);
    iVar7 = (int)uVar16;
    iVar6 = *param_1;
    *(int *)((int)this + iVar6 * 0xd0 + 0x188c) = iVar7;
    if (iVar7 < 1) {
      *(undefined4 *)((int)this + iVar6 * 0xd0 + 0x1888) = 0;
      iVar7 = 0;
    }
    else {
      piVar8 = (int *)((int)this + iVar6 * 0xd0 + 0x1888);
      *piVar8 = *piVar8 + 1;
    }
    if (*(int *)((int)this + iVar6 * 0xd0 + 0x1888) < 2) {
      iVar6 = FUN_10001120(this,iVar6,0xf);
      if (iVar7 == 0) {
        FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      }
      else {
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      }
    }
  }
  if (*(byte *)((int)piVar2 + 7) < 0x1f) {
    *(undefined1 *)((int)piVar2 + 7) = 0;
  }
  if ((uint)*(byte *)((int)piVar2 + 7) != (uint)*(byte *)((int)piVar1 + 7)) {
    uVar16 = FUN_10006e40((uint)*(byte *)((int)piVar1 + 7),piVar1);
    iVar7 = (int)uVar16;
    iVar6 = *param_1;
    *(int *)((int)this + iVar6 * 0xd0 + 0x1894) = iVar7;
    if (iVar7 < 1) {
      iVar7 = 0;
      *(undefined4 *)((int)this + iVar6 * 0xd0 + 0x1890) = 0;
    }
    else {
      piVar1 = (int *)((int)this + iVar6 * 0xd0 + 0x1890);
      *piVar1 = *piVar1 + 1;
    }
    if (*(int *)((int)this + iVar6 * 0xd0 + 0x1890) < 2) {
      iVar6 = FUN_10001120(this,iVar6,0x10);
      if (iVar7 == 0) {
        FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      }
      else {
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      }
    }
  }
  iVar7 = (int)(short)piVar2[2];
  iVar6 = -param_1[0x10];
  if (iVar7 < iVar6) {
    uVar16 = FUN_10006e40(-(param_1[0x10] + iVar7),iVar6);
    iVar13 = (int)uVar16;
    iVar7 = *param_1;
    iVar6 = iVar7 * 6 + 4;
    if (iVar6 != 0x1c) {
      iVar9 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
      iVar14 = iVar9 + 0x14 + (int)this;
      uVar16 = FUN_10006e40(iVar9,(int)(uVar16 >> 0x20));
      *(int *)(iVar14 + 0xa00 + iVar6 * 4) = (int)uVar16;
      uVar16 = FUN_10006e40(extraout_ECX,iVar14);
      *(int *)(iVar14 + 0xa70 + iVar6 * 4) = (int)uVar16;
      if (*(int *)(iVar14 + 0xa00 + iVar6 * 4) != 0) {
        FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),iVar6,(int)uVar16,0);
      }
    }
    iVar6 = iVar7 * 0xd0;
    *(int *)((int)this + iVar6 + 0x18ac) = iVar13;
    if (iVar13 < 0x3334) {
      iVar13 = 0;
      *(undefined4 *)((int)this + iVar6 + 0x18a8) = 0;
    }
    else {
      piVar1 = (int *)((int)this + iVar6 + 0x18a8);
      *piVar1 = *piVar1 + 1;
    }
    if (*(int *)((int)this + iVar6 + 0x18a8) < 2) {
      iVar6 = FUN_10001120(this,iVar7,0x13);
      if (iVar13 != 0) {
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
        uVar12 = 0x13;
        iVar6 = extraout_EDX;
        goto LAB_1000457e;
      }
      FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      iVar6 = extraout_EDX_00;
    }
    uVar12 = 0x13;
  }
  else if (param_1[0x11] < iVar7) {
    uVar16 = FUN_10006e40(param_1[0x11],iVar6);
    iVar7 = *param_1;
    iVar6 = iVar7 * 6 + 4;
    if (iVar6 != 0x1c) {
      iVar14 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
      uVar17 = FUN_10006e40(extraout_ECX_00,iVar14);
      *(int *)((int)this + iVar6 * 4 + iVar14 + 0xa14) = (int)uVar17;
      uVar17 = FUN_10006e40(extraout_ECX_01,(int)(uVar17 >> 0x20));
      uVar11 = (undefined4)(uVar17 >> 0x20);
      iVar13 = *(int *)((int)this + iVar6 * 4 + iVar14 + 0xa14);
      *(int *)((int)this + iVar6 * 4 + iVar14 + 0xa84) = (int)uVar17;
      if (iVar13 != 0) {
        FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),iVar6,(int)uVar17,0);
        uVar11 = extraout_EDX_01;
      }
      uVar16 = CONCAT44(uVar11,(int)uVar16);
    }
    iVar6 = (int)(uVar16 >> 0x20);
    iVar13 = (int)uVar16;
    *(int *)((int)this + iVar7 * 0xd0 + 0x18b4) = iVar13;
    if (iVar13 < 0x3334) {
      iVar13 = 0;
      *(undefined4 *)((int)this + iVar7 * 0xd0 + 0x18b0) = 0;
    }
    else {
      piVar1 = (int *)((int)this + iVar7 * 0xd0 + 0x18b0);
      *piVar1 = *piVar1 + 1;
    }
    if (*(int *)((int)this + iVar7 * 0xd0 + 0x18b0) < 2) {
      iVar6 = FUN_10001120(this,iVar7,0x14);
      if (iVar13 != 0) {
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
        uVar12 = 0x14;
        iVar6 = extraout_EDX_02;
        goto LAB_1000457e;
      }
      FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      iVar6 = extraout_EDX_03;
    }
    uVar12 = 0x14;
  }
  else {
    uVar12 = 0;
  }
LAB_1000457e:
  uVar15 = param_1[0xc];
  if ((uVar15 != 0) && (uVar15 != uVar12)) {
    FUN_10003f40(this,iVar6,*param_1,uVar15,0);
  }
  param_1[0xc] = uVar12;
  iVar6 = (int)*(short *)((int)piVar2 + 10);
  iVar7 = -param_1[0x12];
  if (iVar6 < iVar7) {
    uVar16 = FUN_10006e40(-(param_1[0x12] + iVar6),iVar7);
    FUN_10003f40(this,(int)(uVar16 >> 0x20),*param_1,0x12,(int)uVar16);
    uVar12 = 0x12;
  }
  else if (param_1[0x13] < iVar6) {
    uVar16 = FUN_10006e40(param_1[0x13],iVar7);
    FUN_10003f40(this,(int)(uVar16 >> 0x20),*param_1,0x11,(int)uVar16);
    uVar12 = 0x11;
  }
  else {
    uVar12 = 0;
  }
  uVar15 = param_1[0xd];
  if ((uVar15 != 0) && (uVar15 != uVar12)) {
    FUN_10003f40(this,*param_1,*param_1,uVar15,0);
  }
  param_1[0xd] = uVar12;
  iVar7 = (int)(short)piVar2[3];
  iVar6 = -param_1[0x14];
  if (iVar7 < iVar6) {
    uVar16 = FUN_10006e40(-(param_1[0x14] + iVar7),iVar6);
    iVar13 = (int)uVar16;
    iVar7 = *param_1;
    iVar6 = iVar7 * 6 + 8;
    if (iVar6 != 0x1c) {
      iVar9 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
      iVar14 = iVar9 + 0x14 + (int)this;
      uVar16 = FUN_10006e40(iVar9,(int)(uVar16 >> 0x20));
      *(int *)(iVar14 + 0xa00 + iVar6 * 4) = (int)uVar16;
      uVar16 = FUN_10006e40(extraout_ECX_02,iVar14);
      *(int *)(iVar14 + 0xa70 + iVar6 * 4) = (int)uVar16;
      if (*(int *)(iVar14 + 0xa00 + iVar6 * 4) != 0) {
        FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),iVar6,(int)uVar16,0);
      }
    }
    iVar6 = iVar7 * 0xd0;
    *(int *)((int)this + iVar6 + 0x18cc) = iVar13;
    if (iVar13 < 0x3334) {
      iVar13 = 0;
      *(undefined4 *)((int)this + iVar6 + 0x18c8) = 0;
    }
    else {
      piVar1 = (int *)((int)this + iVar6 + 0x18c8);
      *piVar1 = *piVar1 + 1;
    }
    if (*(int *)((int)this + iVar6 + 0x18c8) < 2) {
      iVar6 = FUN_10001120(this,iVar7,0x17);
      if (iVar13 != 0) {
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
        uVar12 = 0x17;
        iVar6 = extraout_EDX_04;
        goto LAB_1000483f;
      }
      FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      iVar6 = extraout_EDX_05;
    }
    uVar12 = 0x17;
  }
  else if (param_1[0x15] < iVar7) {
    uVar16 = FUN_10006e40(param_1[0x15],iVar6);
    iVar7 = *param_1;
    iVar6 = iVar7 * 6 + 8;
    if (iVar6 != 0x1c) {
      iVar14 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
      uVar17 = FUN_10006e40(extraout_ECX_03,iVar14);
      *(int *)((int)this + iVar6 * 4 + iVar14 + 0xa14) = (int)uVar17;
      uVar17 = FUN_10006e40(extraout_ECX_04,(int)(uVar17 >> 0x20));
      uVar11 = (undefined4)(uVar17 >> 0x20);
      iVar13 = *(int *)((int)this + iVar6 * 4 + iVar14 + 0xa14);
      *(int *)((int)this + iVar6 * 4 + iVar14 + 0xa84) = (int)uVar17;
      if (iVar13 != 0) {
        FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),iVar6,(int)uVar17,0);
        uVar11 = extraout_EDX_06;
      }
      uVar16 = CONCAT44(uVar11,(int)uVar16);
    }
    iVar6 = (int)(uVar16 >> 0x20);
    iVar13 = (int)uVar16;
    *(int *)((int)this + iVar7 * 0xd0 + 0x18d4) = iVar13;
    if (iVar13 < 0x3334) {
      iVar13 = 0;
      *(undefined4 *)((int)this + iVar7 * 0xd0 + 0x18d0) = 0;
    }
    else {
      piVar1 = (int *)((int)this + iVar7 * 0xd0 + 0x18d0);
      *piVar1 = *piVar1 + 1;
    }
    if (*(int *)((int)this + iVar7 * 0xd0 + 0x18d0) < 2) {
      iVar6 = FUN_10001120(this,iVar7,0x18);
      if (iVar13 != 0) {
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
        uVar12 = 0x18;
        iVar6 = extraout_EDX_07;
        goto LAB_1000483f;
      }
      FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      iVar6 = extraout_EDX_08;
    }
    uVar12 = 0x18;
  }
  else {
    uVar12 = 0;
  }
LAB_1000483f:
  uVar15 = param_1[0xe];
  if ((uVar15 != 0) && (uVar15 != uVar12)) {
    FUN_10003f40(this,iVar6,*param_1,uVar15,0);
  }
  param_1[0xe] = uVar12;
  iVar7 = (int)*(short *)((int)piVar2 + 0xe);
  iVar6 = -param_1[0x16];
  if (iVar7 < iVar6) {
    uVar16 = FUN_10006e40(-(param_1[0x16] + iVar7),iVar6);
    iVar13 = (int)uVar16;
    iVar7 = *param_1;
    iVar6 = iVar7 * 6 + 7;
    if (iVar6 != 0x1c) {
      iVar9 = (uint)*(byte *)((int)this + 0x12) * 0xaf8;
      uVar16 = FUN_10006e40(extraout_ECX_05,(int)(uVar16 >> 0x20));
      *(int *)((int)this + iVar6 * 4 + iVar9 + 0xa14) = (int)uVar16;
      uVar16 = FUN_10006e40(extraout_ECX_06,(int)(uVar16 >> 0x20));
      iVar14 = *(int *)((int)this + iVar6 * 4 + iVar9 + 0xa14);
      *(int *)((int)this + iVar6 * 4 + iVar9 + 0xa84) = (int)uVar16;
      if (iVar14 != 0) {
        FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),iVar6,(int)uVar16,0);
      }
    }
    iVar6 = iVar7 * 0xd0;
    *(int *)((int)this + iVar6 + 0x18c4) = iVar13;
    if (iVar13 < 0x3334) {
      iVar13 = 0;
      *(undefined4 *)((int)this + iVar6 + 0x18c0) = 0;
    }
    else {
      piVar2 = (int *)((int)this + iVar6 + 0x18c0);
      *piVar2 = *piVar2 + 1;
    }
    if (*(int *)((int)this + iVar6 + 0x18c0) < 2) {
      iVar6 = FUN_10001120(this,iVar7,0x16);
      if (iVar13 != 0) {
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
        uVar12 = 0x16;
        iVar6 = extraout_EDX_09;
        goto LAB_10004a81;
      }
      FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      iVar6 = extraout_EDX_10;
    }
    uVar12 = 0x16;
  }
  else if (param_1[0x17] < iVar7) {
    uVar16 = FUN_10006e40(param_1[0x17],iVar6);
    iVar14 = (int)uVar16;
    iVar13 = *param_1;
    iVar6 = -iVar14;
    iVar7 = iVar13 * 6 + 7;
    if (iVar7 != 0x1c) {
      iVar9 = (uint)*(byte *)((int)this + 0x12) * 0xaf8 + 0x14 + (int)this;
      uVar16 = FUN_10006e40(extraout_ECX_07,iVar6);
      *(int *)(iVar9 + 0xa00 + iVar7 * 4) = (int)uVar16;
      uVar16 = FUN_10006e40(iVar9,(int)(uVar16 >> 0x20));
      iVar6 = (int)(uVar16 >> 0x20);
      *(int *)(iVar9 + 0xa70 + iVar7 * 4) = (int)uVar16;
      if (*(int *)(iVar9 + 0xa00 + iVar7 * 4) != 0) {
        FUN_10001a30(this,3,*(undefined4 *)((int)this + 0x160c),iVar7,(int)uVar16,0);
        iVar6 = extraout_EDX_11;
      }
    }
    *(int *)((int)this + iVar13 * 0xd0 + 0x18bc) = iVar14;
    if (iVar14 < 0x3334) {
      iVar14 = 0;
      *(undefined4 *)((int)this + iVar13 * 0xd0 + 0x18b8) = 0;
    }
    else {
      piVar2 = (int *)((int)this + iVar13 * 0xd0 + 0x18b8);
      *piVar2 = *piVar2 + 1;
    }
    if (*(int *)((int)this + iVar13 * 0xd0 + 0x18b8) < 2) {
      iVar6 = FUN_10001120(this,iVar13,0x15);
      if (iVar14 != 0) {
        FUN_10001a90(this,0,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
        uVar12 = 0x15;
        iVar6 = extraout_EDX_12;
        goto LAB_10004a81;
      }
      FUN_10001af0(this,1,*(undefined4 *)((int)this + 0x160c),iVar6,iVar6);
      iVar6 = extraout_EDX_13;
    }
    uVar12 = 0x15;
  }
  else {
    uVar12 = 0;
  }
LAB_10004a81:
  uVar15 = param_1[0xf];
  if ((uVar15 != 0) && (uVar15 != uVar12)) {
    FUN_10003f40(this,iVar6,*param_1,uVar15,0);
  }
  param_1[0xf] = uVar12;
  param_1[0xb] = param_1[0xb] ^ 1;
  return;
}


// 10004ab0 FUN_10004ab0

void __fastcall FUN_10004ab0(void *param_1)

{
  undefined4 *puVar1;
  int iVar2;
  
  *(undefined4 *)((int)param_1 + 0x161c) = 0;
  DAT_1001ac10 = GetProcAddress(*(HMODULE *)((int)param_1 + 0x1da4),"XInputGetState");
  DAT_1001ac14 = GetProcAddress(*(HMODULE *)((int)param_1 + 0x1da4),"XInputSetState");
  DAT_1001ac18 = GetProcAddress(*(HMODULE *)((int)param_1 + 0x1da4),"XInputGetCapabilities");
  if (((DAT_1001ac10 != (FARPROC)0x0) && (DAT_1001ac14 != (FARPROC)0x0)) &&
     (DAT_1001ac18 != (FARPROC)0x0)) {
    puVar1 = (undefined4 *)((int)param_1 + 0x1b50);
    iVar2 = 0;
    do {
      FUN_10003e60(param_1,puVar1,iVar2);
      iVar2 = iVar2 + 1;
      puVar1 = puVar1 + 0x24;
    } while (iVar2 < 4);
  }
  return;
}


// 10004b40 FUN_10004b40

void __fastcall FUN_10004b40(void *param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  
  piVar3 = (int *)((int)param_1 + 0x1b50);
  iVar2 = 0;
  do {
    iVar1 = (*DAT_1001ac10)(iVar2,piVar3 + piVar3[0xb] * 4 + 3);
    if (iVar1 == 0) {
      if ((char)piVar3[2] == '\0') {
        FUN_10003e60(param_1,piVar3,iVar2);
        FUN_10001a30(param_1,0x65,*(undefined4 *)((int)param_1 + 0x160c),iVar2,0,0);
      }
      FUN_10004120(param_1,piVar3);
      if ((char)piVar3[0x23] != '\0') {
        (*DAT_1001ac14)(*piVar3,piVar3 + 0x22);
        *(undefined1 *)(piVar3 + 0x23) = 0;
      }
    }
    else if ((iVar1 == 0x48f) && ((char)piVar3[2] != '\0')) {
      iVar1 = *piVar3;
      *piVar3 = -1;
      *(undefined1 *)(piVar3 + 2) = 0;
      *(int *)((int)param_1 + 0x161c) = *(int *)((int)param_1 + 0x161c) + -1;
      FUN_10001cf0(param_1,0x72,0x131);
      FUN_100012c0(param_1,4,0x1b);
      _memset((void *)((int)param_1 + 0x1810),0,0x340);
      FUN_10001a30(param_1,0x66,*(undefined4 *)((int)param_1 + 0x160c),iVar1,0,0);
    }
    iVar2 = iVar2 + 1;
    piVar3 = piVar3 + 0x24;
  } while (iVar2 < 4);
  return;
}


// 10004c40 FUN_10004c40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10004c40(int param_1,int param_2)

{
  int iVar1;
  
  iVar1 = 0;
  if (0 < param_2) {
    do {
      if (DAT_1001ac24 == 0) {
        DAT_1001ac24 = (**(code **)(param_1 + iVar1 * 4))("VFileSystem018",0);
      }
      if (DAT_1001ac28 == 0) {
        DAT_1001ac28 = (**(code **)(param_1 + iVar1 * 4))("VMaterialSystem080",0);
        _DAT_1001ac2c = DAT_1001ac28;
      }
      if (DAT_1001ac30 == 0) {
        DAT_1001ac30 = (**(code **)(param_1 + iVar1 * 4))("InputSystemVersion001",0);
      }
      if (DAT_1001ac34 == 0) {
        DAT_1001ac34 = (**(code **)(param_1 + iVar1 * 4))("NetworkSystemVersion001",0);
      }
      if (DAT_1001ac38 == 0) {
        DAT_1001ac38 = (**(code **)(param_1 + iVar1 * 4))("MaterialSystemHardwareConfig013",0);
      }
      if (DAT_1001ac3c == 0) {
        DAT_1001ac3c = (**(code **)(param_1 + iVar1 * 4))("DebugTextureInfo001",0);
      }
      if (DAT_1001ac40 == 0) {
        DAT_1001ac40 = (**(code **)(param_1 + iVar1 * 4))("VBAllocTracker001",0);
      }
      if (DAT_1001ac44 == 0) {
        DAT_1001ac44 = (**(code **)(param_1 + iVar1 * 4))("COLORCORRECTION_VERSION_1",0);
      }
      if (DAT_1001ac48 == 0) {
        DAT_1001ac48 = (**(code **)(param_1 + iVar1 * 4))("VP4001",0);
      }
      if (DAT_1001ac4c == 0) {
        DAT_1001ac4c = (**(code **)(param_1 + iVar1 * 4))("VMDLLIB001",0);
      }
      if (DAT_1001ac50 == 0) {
        DAT_1001ac50 = (**(code **)(param_1 + iVar1 * 4))("QueuedLoaderVersion001",0);
      }
      if (DAT_1001ac54 == 0) {
        DAT_1001ac54 = (**(code **)(param_1 + iVar1 * 4))("VResourceAccessControl001",0);
      }
      if (DAT_1001ac58 == 0) {
        DAT_1001ac58 = (**(code **)(param_1 + iVar1 * 4))("MATCHFRAMEWORK_001",0);
      }
      iVar1 = iVar1 + 1;
    } while (iVar1 < param_2);
  }
  return;
}


// 10004df0 FUN_10004df0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10004df0(void)

{
  DAT_1001ac24 = 0;
  _DAT_1001ac2c = 0;
  DAT_1001ac28 = 0;
  DAT_1001ac38 = 0;
  DAT_1001ac3c = 0;
  DAT_1001ac30 = 0;
  DAT_1001ac34 = 0;
  DAT_1001ac44 = 0;
  DAT_1001ac48 = 0;
  DAT_1001ac4c = 0;
  DAT_1001ac50 = 0;
  DAT_1001ac54 = 0;
  DAT_1001ac58 = 0;
  return;
}


// 10004e40 FUN_10004e40

int __cdecl FUN_10004e40(char *param_1)

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


// 10004e70 FUN_10004e70

int __cdecl FUN_10004e70(byte *param_1,byte *param_2)

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


// 10004ed0 FUN_10004ed0

uint __cdecl FUN_10004ed0(byte *param_1,byte *param_2)

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
        if (uVar1 != uVar2) goto LAB_10004f4a;
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
LAB_10004f4a:
  return uVar1 - uVar2;
}


// 10004f50 FUN_10004f50

int __cdecl FUN_10004f50(char *param_1,char *param_2,int param_3)

{
  int iVar1;
  int iVar2;
  
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  if (0 < param_3) {
    do {
      iVar1 = (int)*param_1;
      iVar2 = (int)*param_2;
      param_3 = param_3 + -1;
      param_1 = param_1 + 1;
      param_2 = param_2 + 1;
      if (iVar1 != iVar2) {
        if (iVar1 - 0x61U < 0x1a) {
          iVar1 = iVar1 + -0x20;
        }
        if (iVar2 - 0x61U < 0x1a) {
          iVar2 = iVar2 + -0x20;
        }
        if (iVar1 != iVar2) {
          return (uint)(iVar2 <= iVar1) * 2 + -1;
        }
      }
    } while ((iVar1 != 0) && (0 < param_3));
  }
  return 0;
}


// 10004fd0 FUN_10004fd0

void __cdecl FUN_10004fd0(char *param_1,char *param_2,int param_3)

{
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  FUN_10004f50(param_1,param_2,param_3);
  return;
}


// 10005010 FUN_10005010

char * __cdecl FUN_10005010(char *param_1,char *param_2)

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


// 100050f0 FUN_100050f0

void __cdecl FUN_100050f0(char *param_1,char *param_2)

{
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  FUN_10005010(param_1,param_2);
  return;
}


// 10005120 FUN_10005120

char * __cdecl FUN_10005120(char *param_1,char *param_2,uint param_3,size_t param_4)

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


// 100051b0 FUN_100051b0

void __cdecl FUN_100051b0(char *param_1,char param_2)

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


// 100051e0 FUN_100051e0

undefined1 __cdecl FUN_100051e0(char *param_1)

{
  char cVar1;
  
  cVar1 = *param_1;
  if ((((cVar1 == '\0') || (param_1[1] != ':')) && (cVar1 != '/')) && (cVar1 != '\\')) {
    return 0;
  }
  return 1;
}


// 10005200 FUN_10005200

void __cdecl FUN_10005200(void *param_1,int param_2,size_t param_3)

{
  _AssertValidWritePtr(param_1,param_3);
  _memset(param_1,param_2,param_3);
  return;
}


// 10005230 FUN_10005230

void __cdecl FUN_10005230(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  _memmove(param_1,param_2,param_3);
  return;
}


// 10005260 FUN_10005260

void __cdecl FUN_10005260(char *param_1,char *param_2,size_t param_3)

{
  _AssertValidWritePtr(param_1,param_3);
  AssertValidStringPtr(param_2,0xffffff);
  _strncpy(param_1,param_2,param_3);
  if (0 < (int)param_3) {
    param_1[param_3 - 1] = '\0';
  }
  return;
}


// 100052a0 FUN_100052a0

size_t __cdecl FUN_100052a0(char *param_1,size_t param_2,char *param_3)

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


// 100052f0 FUN_100052f0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100052f0(int param_1,int param_2)

{
  int iVar1;
  int iVar2;
  
  if (DAT_1001ac88 == '\0') {
    iVar2 = 0;
    DAT_1001ac88 = '\x01';
    iVar1 = DAT_1001ac80;
    if (0 < param_2) {
      do {
        if (iVar1 == 0) {
          iVar1 = (**(code **)(param_1 + iVar2 * 4))("VEngineCvar007",0);
          _DAT_1001ac7c = iVar1;
          DAT_1001ac80 = iVar1;
        }
        if (DAT_1001ac84 == 0) {
          DAT_1001ac84 = (**(code **)(param_1 + iVar2 * 4))("VProcessUtils001",0);
          iVar1 = DAT_1001ac80;
        }
        iVar2 = iVar2 + 1;
      } while (iVar2 < param_2);
    }
  }
  return;
}


// 10005360 FUN_10005360

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10005360(void)

{
  if (DAT_1001ac88 != '\0') {
    _DAT_1001ac7c = 0;
    DAT_1001ac80 = 0;
    DAT_1001ac84 = 0;
    DAT_1001ac88 = '\0';
  }
  return;
}


// 10005380 FUN_10005380

void __cdecl FUN_10005380(undefined4 param_1,undefined **param_2)

{
  int *piVar1;
  int *piVar2;
  
  if ((DAT_1001ac80 != (int *)0x0) && (DAT_1001ada4 == '\0')) {
    DAT_1001ada0 = param_1;
    DAT_1001ada4 = '\x01';
    DAT_10016cec = (**(code **)(*DAT_1001ac80 + 0x14))();
    piVar2 = DAT_1001ad98;
    DAT_1001ad9c = param_2;
    if (param_2 == (undefined **)0x0) {
      DAT_1001ad9c = &PTR_PTR_10016cf8;
    }
    while (piVar2 != (int *)0x0) {
      piVar1 = (int *)piVar2[1];
      (**(code **)(*piVar2 + 0xc))(DAT_1001ada0);
      (**(code **)(*piVar2 + 0x2c))();
      piVar2 = piVar1;
    }
    (**(code **)(*DAT_1001ac80 + 0x70))();
    (**(code **)(*DAT_1001ac80 + 0x94))();
    DAT_1001ad98 = (int *)0x0;
  }
  return;
}


// 10005430 FUN_10005430

void FUN_10005430(void)

{
  if ((DAT_1001ac80 != (int *)0x0) && (DAT_1001ada4 != '\0')) {
    (**(code **)(*DAT_1001ac80 + 0x74))(DAT_10016cec);
    (**(code **)(*DAT_1001ac80 + 0x20))(DAT_10016cec);
    DAT_10016cec = 0xffffffff;
    DAT_1001ada4 = '\0';
  }
  return;
}


// 10005560 FUN_10005560

int * __thiscall
FUN_10005560(void *this,int param_1,int param_2,undefined *param_3,uint param_4,undefined1 *param_5)

{
  undefined1 *puVar1;
  
  *(byte *)((int)this + 0x20) = *(byte *)((int)this + 0x20) | 2;
  *(int *)((int)this + 0x18) = param_2;
  *(undefined1 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)this = &PTR_LAB_10012e24;
  puVar1 = param_5;
  if (param_5 == (undefined1 *)0x0) {
    puVar1 = &LAB_10005550;
  }
  *(undefined1 **)((int)this + 0x1c) = puVar1;
  *(byte *)((int)this + 0x20) = param_5 != (undefined1 *)0x0 | *(byte *)((int)this + 0x20) & 0xfa;
  *(undefined1 *)((int)this + 8) = 0;
  *(int *)((int)this + 0xc) = param_1;
  if (param_3 == (undefined *)0x0) {
    param_3 = PTR_DAT_10016d44;
  }
  *(undefined **)((int)this + 0x10) = param_3;
  *(uint *)((int)this + 0x14) = param_4;
  if ((param_4 & 1) == 0) {
    *(void **)((int)this + 4) = DAT_1001ad98;
    DAT_1001ad98 = this;
  }
  else {
    *(undefined4 *)((int)this + 4) = 0;
  }
  if (DAT_1001ad9c != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x2c))();
  }
  return (int *)this;
}


// 10005670 FUN_10005670

void __fastcall FUN_10005670(undefined4 *param_1)

{
  *param_1 = &PTR_FUN_10012ea4;
  param_1[6] = &PTR_FUN_10012e84;
  if (param_1[9] != 0) {
    FUN_10003a70(param_1[9]);
    param_1[9] = 0;
  }
  *param_1 = &PTR_LAB_10012dec;
  return;
}


// 100056a0 FUN_100056a0

undefined4 __thiscall FUN_100056a0(void *this,uint param_1)

{
  return CONCAT31((int3)((uint)*(int *)((int)this + 0x1c) >> 8),
                  (*(uint *)(*(int *)((int)this + 0x1c) + 0x14) & param_1) != 0);
}


// 100056f0 FUN_100056f0

undefined4 __fastcall FUN_100056f0(int param_1)

{
  return *(undefined4 *)(*(int *)(param_1 + 0x1c) + 0xc);
}


// 10005720 FUN_10005720

void __fastcall FUN_10005720(undefined4 param_1)

{
  if (DAT_1001ad9c != (undefined4 *)0x0) {
    (**(code **)*DAT_1001ad9c)(param_1);
  }
  return;
}


// 10005740 FUN_10005740

void __fastcall FUN_10005740(int *param_1)

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
  if ((cVar1 != '\0') && (DAT_1001ac80 != (int *)0x0)) {
    cVar1 = (**(code **)(*DAT_1001ac80 + 0x80))();
    if (cVar1 == '\0') {
      (**(code **)(*DAT_1001ac80 + 0x8c))(param_1,unaff_retaddr);
      return;
    }
  }
  aiStack_28[0] = param_1[0xb];
  piVar4 = unaff_retaddr;
  if (unaff_retaddr == (int *)0x0) {
    piVar4 = (int *)&DAT_10012b4a;
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
    FUN_100052a0((char *)aiStack_28,0x20,"%f");
    piVar4 = aiStack_28;
    uVar2 = extraout_ECX_00;
    uVar3 = extraout_EDX_00;
  }
  param_1[0xb] = 0xb00000;
  uVar5 = FUN_10006e40(uVar2,uVar3);
  param_1[0xc] = (int)uVar5;
  if ((param_1[5] & 0x1000U) == 0) {
    (**(code **)(*param_1 + 0x40))(piVar4,fVar7);
  }
  return;
}


// 10005820 FUN_10005820

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

void __thiscall FUN_10005820(void *this,undefined4 param_1,undefined4 param_2)

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
  
  asStack_1c[2] = 0x10005836;
  iVar2 = -(*(int *)((int)this + 0x28) + 0xfU & 0xfffffff0);
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x28);
  *(undefined4 *)((int)asStack_1c + iVar2 + 4) = uVar4;
  *(undefined1 **)((int)asStack_1c + iVar2) = &stack0xfffffff0 + iVar2;
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = 0x10005843;
  _memcpy(*(void **)((int)asStack_1c + iVar2),*(void **)((int)asStack_1c + iVar2 + 4),
          *(size_t *)((int)asStack_1c + iVar2 + 8));
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = param_1;
  *(undefined4 *)((int)auStack_24 + iVar2) = 0x1000584c;
  iVar3 = FUN_10004e40(*(char **)((int)auStack_24 + iVar2 + 4));
  iVar3 = iVar3 + 1;
  puVar6 = &stack0xfffffff0 + iVar2;
  if (*(int *)((int)this + 0x28) < iVar3) {
    if (*(int *)((int)this + 0x24) != 0) {
      *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x24);
      *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x10005866;
      FUN_10003a70(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    }
    puVar6 = &stack0xfffffff0 + iVar2;
    *(int *)((int)asStack_1c + iVar2 + 8) = iVar3;
    *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x1000586f;
    uVar4 = FUN_10003a10(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    *(undefined4 *)((int)this + 0x24) = uVar4;
    *(int *)((int)this + 0x28) = iVar3;
  }
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)(puVar6 + -4) = iVar3;
  *(undefined4 *)(puVar6 + -8) = param_1;
  *(undefined4 *)(puVar6 + -0xc) = uVar4;
  *(undefined4 *)(puVar6 + -0x10) = 0x10005886;
  _memcpy(*(void **)(puVar6 + -0xc),*(void **)(puVar6 + -8),*(size_t *)(puVar6 + -4));
  pcVar1 = *(code **)((int)this + 0x44);
  if (pcVar1 != (code *)0x0) {
    *(undefined4 *)(puVar6 + -4) = extraout_ECX;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(int *)(puVar6 + -0xc) = (int)this + 0x18;
    puVar5 = puVar6 + -0x10;
    *(undefined4 *)(puVar6 + -0x10) = 0x1000589e;
    (*pcVar1)();
    puVar6 = puVar5 + 0xc;
  }
  if (DAT_1001ac80 != (int *)0x0) {
    pcVar1 = *(code **)(*DAT_1001ac80 + 0x48);
    *(int **)(puVar6 + -4) = DAT_1001ac80;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(void **)(puVar6 + -0xc) = this;
    *(undefined4 *)(puVar6 + -0x10) = 0x100058bb;
    (*pcVar1)();
  }
  return;
}


// 10005920 FUN_10005920

void __thiscall FUN_10005920(void *this,float param_1)

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
    if (((cVar1 != '\0') && (DAT_1001ac80 != (int *)0x0)) &&
       (cVar1 = (**(code **)(*DAT_1001ac80 + 0x80))(), cVar1 == '\0')) {
      (**(code **)(*DAT_1001ac80 + 0x84))();
      return;
    }
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x3c))(&stack0x00000000);
    uVar3 = *(undefined4 *)((int)this + 0x2c);
    *(undefined4 *)((int)this + 0x2c) = uStack_4;
    uVar2 = FUN_10006e40(extraout_ECX,extraout_EDX);
    *(int *)((int)this + 0x30) = (int)uVar2;
    if ((*(uint *)((int)this + 0x14) & 0x1000) == 0) {
      FUN_100052a0(&stack0xffffffd8,0x20,"%f");
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x40))(&stack0xffffffd8,uVar3);
      return;
    }
  }
  return;
}


// 10005ac0 FUN_10005ac0

void __thiscall
FUN_10005ac0(void *this,int param_1,char *param_2,uint param_3,undefined *param_4,undefined1 param_5
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
    param_2 = PTR_DAT_10016d94;
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
  _Dst = (void *)FUN_10003a10(param_2 + (1 - (int)pcVar1));
  *(void **)((int)this + 0x24) = _Dst;
  _memcpy(_Dst,*(void **)((int)this + 0x20),*(size_t *)((int)this + 0x28));
  dVar4 = _atof(*(char **)((int)this + 0x24));
  *(float *)((int)this + 0x2c) = (float)dVar4;
  uVar3 = FUN_10006e40(extraout_ECX,extraout_EDX);
  *(int *)((int)this + 0x30) = (int)uVar3;
  if (((param_3 & 0x84200) == 0) && (-1 < (char)param_3)) {
    param_3 = param_3 | 2;
  }
  *(int *)((int)this + 0xc) = param_1;
  *(undefined1 *)((int)this + 8) = 0;
  if (param_4 == (undefined *)0x0) {
    param_4 = PTR_DAT_10016d44;
  }
  *(undefined **)((int)this + 0x10) = param_4;
  *(uint *)((int)this + 0x14) = param_3;
  if ((param_3 & 1) == 0) {
    *(void **)((int)this + 4) = DAT_1001ad98;
    DAT_1001ad98 = this;
  }
  else {
    *(undefined4 *)((int)this + 4) = 0;
  }
  if (DAT_1001ad9c != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x2c))();
  }
  return;
}


// 10005be0 FUN_10005be0

void __fastcall FUN_10005be0(int param_1)

{
                    /* WARNING: Could not recover jumptable at 0x10005be8. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(**(int **)(param_1 + 4) + 0x38))();
  return;
}


// 10005c50 FUN_10005c50

undefined4 * __thiscall
FUN_10005c50(void *this,int param_1,char *param_2,uint param_3,undefined *param_4)

{
  *(undefined1 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_10012e64;
  *(undefined ***)this = &PTR_FUN_10012ea4;
  *(undefined ***)((int)this + 0x18) = &PTR_FUN_10012e84;
  FUN_10005ac0(this,param_1,param_2,param_3,param_4,0,0,0,0,0);
  return (undefined4 *)this;
}


// 10005cb0 FUN_10005cb0

int * __fastcall FUN_10005cb0(int *param_1)

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
  param_1[6] = (int)&PTR_LAB_10012e64;
  param_1[0xe] = 0;
  param_1[0x10] = 0;
  *param_1 = (int)&PTR_FUN_10012ea4;
  param_1[6] = (int)&PTR_FUN_10012e84;
  param_1[7] = (int)param_1;
  param_1[8] = (int)&DAT_10012480;
  *(undefined1 *)(param_1 + 0xd) = 0;
  *(undefined1 *)(param_1 + 0xf) = 0;
  param_1[0x11] = 0;
  pcVar2 = "0";
  do {
    pcVar3 = pcVar2;
    pcVar2 = pcVar3 + 1;
  } while (*pcVar3 != '\0');
  param_1[10] = (int)(pcVar3 + -0x1001247f);
  _Dst = (void *)FUN_10003a10(pcVar3 + -0x1001247f);
  param_1[9] = (int)_Dst;
  _memcpy(_Dst,(void *)param_1[8],param_1[10]);
  dVar5 = _atof((char *)param_1[9]);
  param_1[0xb] = (int)(float)dVar5;
  uVar4 = FUN_10006e40(extraout_ECX,extraout_EDX);
  puVar1 = PTR_DAT_10016d44;
  param_1[0xc] = (int)uVar4;
  *(undefined1 *)(param_1 + 2) = 0;
  param_1[3] = (int)&DAT_10012b4a;
  param_1[4] = (int)puVar1;
  param_1[5] = 2;
  param_1[1] = (int)DAT_1001ad98;
  DAT_1001ad98 = param_1;
  if (DAT_1001ad9c != 0) {
    (**(code **)(*param_1 + 0x2c))();
  }
  *param_1 = (int)&PTR_FUN_10012f1c;
  param_1[6] = (int)&PTR_LAB_10012efc;
  return param_1;
}


// 10005de0 FUN_10005de0

undefined4 * __thiscall FUN_10005de0(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_10012ea4;
  *(undefined ***)((int)this + 0x18) = &PTR_FUN_10012e84;
  if (*(int *)((int)this + 0x24) != 0) {
    FUN_10003a70(*(int *)((int)this + 0x24));
    *(undefined4 *)((int)this + 0x24) = 0;
  }
  *(undefined ***)this = &PTR_LAB_10012dec;
  if ((param_1 & 1) != 0) {
    FUN_10003a70(this);
  }
  return (undefined4 *)this;
}


// 10005e30 FUN_10005e30

void __thiscall FUN_10005e30(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_10005e8b;
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
LAB_10005e8b:
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


// 10005ec0 FUN_10005ec0

int __thiscall FUN_10005ec0(void *this,int param_1,int *param_2)

{
  int iVar1;
  void *pvVar2;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10005e30(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar2 = (void *)(*this + param_1 * 0x10);
    FUN_10005230((void *)((int)pvVar2 + 0x10),pvVar2,iVar1 * 0x10);
  }
                    /* WARNING: Load size is inaccurate */
  pvVar2 = (void *)(param_1 * 0x10 + *this);
  if (pvVar2 != (void *)0x0) {
    FUN_100065c0(pvVar2,param_2);
  }
  return param_1;
}


// 10005f30 FUN_10005f30

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

int __thiscall FUN_10005f30(void *this,undefined4 param_1,void *param_2)

{
  int iVar1;
  char *pcVar2;
  int iVar3;
  int aiStack_1010 [4];
  char local_1000 [4092];
  undefined4 uStack_4;
  
  uStack_4 = 0x10005f3a;
  if ((*(byte *)((int)this + 0x20) & 4) == 0) {
    if (*(code **)((int)this + 0x1c) != (code *)0x0) {
      iVar1 = (**(code **)((int)this + 0x1c))(param_1,local_1000);
      if (0 < iVar1) {
        pcVar2 = local_1000;
        iVar3 = iVar1;
        do {
          FUN_10006580(aiStack_1010,pcVar2);
          FUN_10005ec0(param_2,*(int *)((int)param_2 + 0xc),aiStack_1010);
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


// 10006010 FUN_10006010

void __thiscall FUN_10006010(void *this,undefined4 param_1,undefined4 param_2)

{
  *(undefined4 *)((int)this + 4) = param_2;
  *(undefined4 *)this = param_1;
  *(void **)((int)this + 8) = DAT_1001adf8;
  DAT_1001adf8 = this;
  return;
}


// 10006030 FUN_10006030

undefined4 __cdecl FUN_10006030(byte *param_1,undefined4 *param_2)

{
  int *piVar1;
  byte bVar2;
  byte *pbVar3;
  int iVar4;
  undefined4 uVar5;
  byte *pbVar6;
  undefined4 *puVar7;
  bool bVar8;
  
  puVar7 = DAT_1001adf8;
  if (DAT_1001adf8 != (undefined4 *)0x0) {
    do {
      pbVar3 = (byte *)puVar7[1];
      pbVar6 = param_1;
      do {
        bVar2 = *pbVar3;
        bVar8 = bVar2 < *pbVar6;
        if (bVar2 != *pbVar6) {
LAB_10006065:
          iVar4 = (1 - (uint)bVar8) - (uint)(bVar8 != 0);
          goto LAB_1000606a;
        }
        if (bVar2 == 0) break;
        bVar2 = pbVar3[1];
        bVar8 = bVar2 < pbVar6[1];
        if (bVar2 != pbVar6[1]) goto LAB_10006065;
        pbVar3 = pbVar3 + 2;
        pbVar6 = pbVar6 + 2;
      } while (bVar2 != 0);
      iVar4 = 0;
LAB_1000606a:
      if (iVar4 == 0) {
        if (param_2 != (undefined4 *)0x0) {
          *param_2 = 0;
        }
                    /* WARNING: Could not recover jumptable at 0x1000609a. Too many branches */
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


// 100060b0 FUN_100060b0

undefined4 __cdecl FUN_100060b0(undefined4 *param_1)

{
  HMODULE pHVar1;
  
  pHVar1 = LoadLibraryExA((LPCSTR)*param_1,(HANDLE)0x0,8);
  param_1[1] = pHVar1;
  return 0;
}


// 100060d0 FUN_100060d0

HMODULE FUN_100060d0(void)

{
  char *in_EAX;
  int iVar1;
  code *pcVar2;
  HMODULE pHVar3;
  undefined4 uVar4;
  undefined4 uVar5;
  undefined4 uStack_40c;
  char *pcStack_408;
  HMODULE pHStack_404;
  char local_400 [1024];
  
  FUN_10005260(local_400,in_EAX,0x400);
  iVar1 = FUN_100050f0(local_400,".dll");
  if (iVar1 == 0) {
    FUN_10005120(local_400,".dll",0x400,0xffffffff);
  }
  FUN_100051b0(local_400,'\\');
  pcVar2 = (code *)GetThreadedLoadLibraryFunc();
  if (pcVar2 != (code *)0x0) {
    pcStack_408 = local_400;
    pHStack_404 = (HMODULE)0x0;
    uVar4 = CreateSimpleThread(FUN_100060b0,&pcStack_408,0);
    uStack_40c = uVar4;
    iVar1 = ThreadWaitForObjects(1,&uStack_40c,1,0);
    while (iVar1 == 0x102) {
      uVar5 = (*pcVar2)();
      uStack_40c = uVar4;
      iVar1 = ThreadWaitForObjects(1,&uStack_40c,1,uVar5);
    }
    ReleaseThreadHandle(uVar4);
    if (pHStack_404 != (HMODULE)0x0) {
      StackToolsNotify_LoadedLibrary(local_400);
    }
    return pHStack_404;
  }
  pHVar3 = LoadLibraryExA(local_400,(HANDLE)0x0,8);
  if (pHVar3 != (HMODULE)0x0) {
    StackToolsNotify_LoadedLibrary(local_400);
  }
  return pHVar3;
}


// 100061f0 FUN_100061f0

void FUN_100061f0(void)

{
  char cVar1;
  int *piVar2;
  int iVar3;
  char *pcVar4;
  HMODULE pHVar5;
  char *pcVar6;
  char *unaff_EDI;
  
  piVar2 = (int *)CommandLine_Tier0();
  iVar3 = (**(code **)(*piVar2 + 0x28))("-xlsp");
  if (iVar3 == 0) {
    piVar2 = (int *)CommandLine_Tier0();
    iVar3 = (**(code **)(*piVar2 + 0x28))(&DAT_10012f70);
    if (iVar3 == 0) goto LAB_10006284;
    pcVar6 = "_ds";
  }
  else {
    pcVar6 = "_xlsp";
  }
  iVar3 = -(int)unaff_EDI;
  do {
    cVar1 = *unaff_EDI;
    unaff_EDI[(int)(&stack0xfffffef8 + iVar3)] = cVar1;
    unaff_EDI = unaff_EDI + 1;
  } while (cVar1 != '\0');
  pcVar4 = _strchr(&stack0xfffffef8,0x2e);
  if (pcVar4 != (char *)0x0) {
    *pcVar4 = '\0';
  }
  FUN_10005120(&stack0xfffffef8,pcVar6,0x104,0xffffffff);
  pHVar5 = FUN_100060d0();
  if (pHVar5 != (HMODULE)0x0) {
    return;
  }
LAB_10006284:
  FUN_100060d0();
  return;
}


// 100062a0 FUN_100062a0

HMODULE __cdecl FUN_100062a0(char *param_1)

{
  char cVar1;
  char *pcVar2;
  char *pcVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  int *piVar5;
  int iVar6;
  char *pcVar7;
  char local_904 [1024];
  char acStack_504 [260];
  char local_400 [1024];
  
  cVar1 = FUN_100051e0(param_1);
  if (cVar1 == '\0') {
    __getcwd(local_904,0x400);
    pcVar7 = local_904;
    do {
      pcVar2 = pcVar7;
      pcVar7 = pcVar2 + 1;
    } while (*pcVar2 != '\0');
    if (pcVar2[-1] == '/') {
LAB_10006312:
      pcVar7 = local_904;
      do {
        pcVar2 = pcVar7;
        pcVar7 = pcVar2 + 1;
      } while (*pcVar2 != '\0');
      pcVar2[-1] = '\0';
    }
    else {
      pcVar7 = local_904;
      do {
        pcVar2 = pcVar7;
        pcVar7 = pcVar2 + 1;
      } while (*pcVar2 != '\0');
      if (pcVar2[-1] == '\\') goto LAB_10006312;
    }
    pcVar2 = local_904;
    pcVar7 = local_904 + 1;
    do {
      cVar1 = *pcVar2;
      pcVar2 = pcVar2 + 1;
    } while (cVar1 != '\0');
    pcVar3 = _strstr(param_1,"bin/");
    if ((pcVar3 == param_1) ||
       (((pcVar2[(int)(local_904 + (-1 - (int)pcVar7))] == 'n' &&
         (pcVar2[(int)(local_904 + (-2 - (int)pcVar7))] == 'i')) &&
        (pcVar2[(int)(local_904 + (-3 - (int)pcVar7))] == 'b')))) {
      pcVar7 = "%s/%s";
    }
    else {
      pcVar7 = "%s/bin/%s";
    }
    FUN_100052a0(local_400,0x400,pcVar7);
    hModule = (HMODULE)FUN_100061f0();
    if (hModule != (HMODULE)0x0) goto LAB_100063c1;
  }
  hModule = (HMODULE)FUN_100061f0();
LAB_100063c1:
  pFVar4 = GetProcAddress(hModule,"BuiltDebug");
  if (pFVar4 != (FARPROC)0x0) {
    if (hModule != (HMODULE)0x0) {
      piVar5 = (int *)CommandLine_Tier0();
      iVar6 = (**(code **)(*piVar5 + 0x28))();
      if (iVar6 == 0) {
        cVar1 = Plat_IsInDebugSession();
        if (cVar1 == '\0') {
          Error();
        }
      }
    }
    DevWarning("Module %s is a debug build\n");
    if (DAT_1001adfc == '\0') {
      DAT_1001adfc = '\x01';
      GetCurrentProcessId();
      _sprintf(acStack_504,"VALVE-MODULE-DEBUG-%08X");
      CreateFileMappingA((HANDLE)0xffffffff,(LPSECURITY_ATTRIBUTES)0x0,4,0,0x400,acStack_504);
    }
  }
  return hModule;
}


// 10006470 FUN_10006470

void __cdecl FUN_10006470(HMODULE param_1)

{
  if (param_1 != (HMODULE)0x0) {
    FreeLibrary(param_1);
  }
  return;
}


// 10006480 FUN_10006480

void __thiscall FUN_10006480(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_100064db;
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
LAB_100064db:
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


// 10006510 FUN_10006510

void __thiscall FUN_10006510(void *this,void *param_1,int param_2)

{
  size_t _Size;
  void *_Dst;
  
  if (param_1 == (void *)0x0) {
    param_2 = 0;
  }
  *(int *)((int)this + 0xc) = param_2;
  if (*(int *)((int)this + 4) < param_2) {
    FUN_10006480(this,param_2 - *(int *)((int)this + 4));
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


// 10006580 FUN_10006580

undefined4 * __thiscall FUN_10006580(void *this,char *param_1)

{
  int iVar1;
  
  iVar1 = 0;
  *(undefined4 *)this = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined4 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  if (param_1 != (char *)0x0) {
    iVar1 = FUN_10004e40(param_1);
    iVar1 = iVar1 + 1;
  }
  FUN_10006510(this,param_1,iVar1);
  return (undefined4 *)this;
}


// 100065c0 FUN_100065c0

undefined4 * __thiscall FUN_100065c0(void *this,int *param_1)

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
    if (pcVar2 == (char *)0x0) goto LAB_100065f3;
  }
  iVar1 = FUN_10004e40(pcVar2);
  iVar1 = iVar1 + 1;
LAB_100065f3:
  FUN_10006510(this,pcVar2,iVar1);
  return (undefined4 *)this;
}


// 10006630 FUN_10006630

undefined4 * __thiscall
FUN_10006630(void *this,undefined1 param_1,char *param_2,int param_3,int param_4)

{
  int *piVar1;
  char cVar2;
  char *pcVar3;
  int iVar4;
  int iVar5;
  
  *(undefined1 *)((int)this + 4) = param_1;
  *(undefined ***)this = &PTR_FUN_10012ff8;
  *(char **)((int)this + 8) = param_2;
  *(int *)((int)this + 0x10) = param_3;
  iVar4 = FUN_10004e40(param_2);
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
      iVar4 = FUN_10004e40(pcVar3);
      *piVar1 = iVar4;
      if (*(int *)((int)this + 0x14) < iVar4) {
        *(int *)((int)this + 0x14) = iVar4;
      }
      iVar5 = iVar5 + 1;
    } while (iVar5 < param_3);
  }
  return (undefined4 *)this;
}


// 100066c0 FUN_100066c0

uint __thiscall FUN_100066c0(void *this,byte *param_1,undefined4 *param_2)

{
  undefined4 uVar1;
  uint in_EAX;
  int iVar2;
  
  iVar2 = 0;
  if (0 < *(int *)((int)this + 0x10)) {
    do {
      in_EAX = FUN_10004e70(param_1,*(byte **)((int)this +
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


// 1000672f __cfltcvt_init

/* Library Function - Single Match
    __cfltcvt_init
   
   Library: Visual Studio 2005 Release */

void __cfltcvt_init(void)

{
  PTR_LAB_10016ef0 = __cfltcvt;
  PTR_LAB_10016ef4 = &LAB_1000840b;
  PTR_LAB_10016ef8 = &LAB_100083c9;
  PTR_LAB_10016efc = &LAB_100083fd;
  PTR_LAB_10016f00 = &LAB_10008373;
  PTR_LAB_10016f04 = __cfltcvt;
  PTR_LAB_10016f08 = __cfltcvt_l;
  PTR_LAB_10016f0c = __fassign_l;
  PTR_LAB_10016f10 = __cropzeros_l;
  PTR_LAB_10016f14 = __forcdecpt_l;
  return;
}


// 1000678f __fpmath

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __fpmath
   
   Library: Visual Studio 2005 Release */

void __cdecl __fpmath(int param_1)

{
  __cfltcvt_init();
  _DAT_1001c12c = __ms_p5_mp_test_fdiv();
  if (param_1 != 0) {
    __setdefaultprecision();
  }
  return;
}


// 100067b0 _memset

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
  if ((((char)_Val == '\0') && (0xff < _Size)) && (DAT_1001dd3c != 0)) {
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


// 1000682a type_info::~type_info

/* Library Function - Single Match
    public: virtual __thiscall type_info::~type_info(void)
   
   Library: Visual Studio 2005 Release */

void __thiscall type_info::~type_info(type_info *this)

{
  *(undefined ***)this = &PTR__scalar_deleting_destructor__10013030;
  _Type_info_dtor(this);
  return;
}


// 10006838 type_info::`scalar_deleting_destructor'

/* Library Function - Single Match
    public: virtual void * __thiscall type_info::`scalar deleting destructor'(unsigned int)
   
   Library: Visual Studio 2005 Release */

void * __thiscall type_info::_scalar_deleting_destructor_(type_info *this,uint param_1)

{
  ~type_info(this);
  if ((param_1 & 1) != 0) {
    FUN_10003a70(this);
  }
  return this;
}


// 10006880 _memcpy

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
          goto switchD_10006a63_caseD_2;
        case 3:
          goto switchD_10006a63_caseD_3;
        }
        goto switchD_10006a63_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_10006a63_caseD_0;
      case 1:
        goto switchD_10006a63_caseD_1;
      case 2:
        goto switchD_10006a63_caseD_2;
      case 3:
        goto switchD_10006a63_caseD_3;
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
              goto switchD_10006a63_caseD_2;
            case 3:
              goto switchD_10006a63_caseD_3;
            }
            goto switchD_10006a63_caseD_1;
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
              goto switchD_10006a63_caseD_2;
            case 3:
              goto switchD_10006a63_caseD_3;
            }
            goto switchD_10006a63_caseD_1;
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
              goto switchD_10006a63_caseD_2;
            case 3:
              goto switchD_10006a63_caseD_3;
            }
            goto switchD_10006a63_caseD_1;
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
switchD_10006a63_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_10006a63_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_10006a63_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_10006a63_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_1001dd3c != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_100068dc_caseD_2;
      case 3:
        goto switchD_100068dc_caseD_3;
      }
      goto switchD_100068dc_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_100068dc_caseD_0;
    case 1:
      goto switchD_100068dc_caseD_1;
    case 2:
      goto switchD_100068dc_caseD_2;
    case 3:
      goto switchD_100068dc_caseD_3;
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
            goto switchD_100068dc_caseD_2;
          case 3:
            goto switchD_100068dc_caseD_3;
          }
          goto switchD_100068dc_caseD_1;
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
            goto switchD_100068dc_caseD_2;
          case 3:
            goto switchD_100068dc_caseD_3;
          }
          goto switchD_100068dc_caseD_1;
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
            goto switchD_100068dc_caseD_2;
          case 3:
            goto switchD_100068dc_caseD_3;
          }
          goto switchD_100068dc_caseD_1;
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
switchD_100068dc_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_100068dc_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_100068dc_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_100068dc_caseD_0:
  return _Dst;
}


// 10006be5 FUN_10006be5

int __cdecl FUN_10006be5(int param_1)

{
  int *piVar1;
  int *piVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  
  piVar1 = (int *)FUN_100093a5(DAT_1001dd2c);
  piVar2 = (int *)FUN_100093a5(DAT_1001dd28);
  if (piVar1 <= piVar2) {
    uVar4 = ((int)piVar2 - (int)piVar1) + 4;
    if (3 < uVar4) {
      uVar3 = FUN_10003c20(piVar1);
      if (uVar3 < uVar4) {
        uVar4 = 0x800;
        if (uVar3 < 0x800) {
          uVar4 = uVar3;
        }
        if ((uVar4 + uVar3 < uVar3) ||
           (iVar5 = thunk_FUN_10003b90(piVar1,uVar4 + uVar3), iVar5 == 0)) {
          if (uVar3 + 0x10 < uVar3) {
            return 0;
          }
          iVar5 = thunk_FUN_10003b90(piVar1,uVar3 + 0x10);
          if (iVar5 == 0) {
            return 0;
          }
        }
        piVar2 = (int *)(iVar5 + ((int)piVar2 - (int)piVar1 >> 2) * 4);
        DAT_1001dd2c = FUN_1000932e(iVar5);
      }
      iVar5 = FUN_1000932e(param_1);
      *piVar2 = iVar5;
      DAT_1001dd28 = FUN_1000932e((int)(piVar2 + 1));
      return param_1;
    }
  }
  return 0;
}


// 10006ccd __onexit

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __onexit
   
   Library: Visual Studio 2005 Release */

_onexit_t __cdecl __onexit(_onexit_t _Func)

{
  _onexit_t p_Var1;
  
  FUN_10009aba();
  p_Var1 = (_onexit_t)FUN_10006be5((int)_Func);
  FUN_10006d03();
  return p_Var1;
}


// 10006d03 FUN_10006d03

void FUN_10006d03(void)

{
  FUN_10009ac3();
  return;
}


// 10006d09 _atexit

/* Library Function - Single Match
    _atexit
   
   Library: Visual Studio 2005 Release */

int __cdecl _atexit(_func_4879 *param_1)

{
  _onexit_t p_Var1;
  
  p_Var1 = __onexit((_onexit_t)param_1);
  return (p_Var1 != (_onexit_t)0x0) - 1;
}


// 10006d1b _atol

/* Library Function - Single Match
    _atol
   
   Library: Visual Studio 2005 Release */

long __cdecl _atol(char *_Str)

{
  long lVar1;
  
  lVar1 = _strtol(_Str,(char **)0x0,10);
  return lVar1;
}


// 10006d40 _strrchr

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


// 10006d6d __vsnprintf_l

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
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
    iVar2 = FUN_1000a157(&local_24,(byte *)_Format,_Locale,(wchar_t *)_ArgList);
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
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  return iVar2;
}


// 10006e1e __vsnprintf

/* Library Function - Single Match
    __vsnprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl __vsnprintf(char *_Dest,size_t _Count,char *_Format,va_list _Args)

{
  int iVar1;
  
  iVar1 = __vsnprintf_l(_Dest,_Count,_Format,(_locale_t)0x0,_Args);
  return iVar1;
}


// 10006e40 FUN_10006e40

ulonglong __fastcall FUN_10006e40(undefined4 param_1,undefined4 param_2)

{
  ulonglong uVar1;
  uint uVar2;
  float fVar3;
  float10 in_ST0;
  uint local_20;
  float fStack_1c;
  
  if (DAT_1001dd3c == 0) {
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


// 10006eeb __CRT_INIT@12

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
        _DAT_1001c48c = DVar2 * 0x100 + DVar3;
        DAT_1001c484 = DVar1;
        _DAT_1001c488 = uVar10;
        DAT_1001c490 = DVar2;
        _DAT_1001c494 = DVar3;
        bVar4 = FUN_10003c40();
        if (CONCAT31(extraout_var,bVar4) != 0) {
          iVar7 = __mtinit();
          if (iVar7 != 0) {
            __RTC_Initialize();
            DAT_1001dd40 = GetCommandLineA();
            DAT_1001c134 = ___crtGetEnvironmentStringsA();
            iVar7 = __ioinit();
            if (-1 < iVar7) {
              iVar7 = __setargv();
              if (((-1 < iVar7) && (iVar7 = __setenvp(), -1 < iVar7)) &&
                 (iVar7 = __cinit(0), iVar7 == 0)) {
                DAT_1001c130 = DAT_1001c130 + 1;
                return 1;
              }
              __ioterm();
            }
            __mtterm();
          }
          FUN_10003c50();
        }
      }
    }
  }
  else if (param_2 == 0) {
    if (0 < DAT_1001c130) {
      DAT_1001c130 = DAT_1001c130 + -1;
      if (DAT_1001c4c0 == 0) {
        __cexit();
      }
      if (param_3 == 0) {
        __ioterm();
        __mtterm();
        FUN_10003c50();
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
    FUN_1000941c();
    pDVar8 = (DWORD *)FUN_10003bf0(1,0x214);
    if (pDVar8 != (DWORD *)0x0) {
      uVar12 = DAT_10016fe0;
      pDVar13 = pDVar8;
      pcVar9 = (code *)FUN_100093a5(DAT_1001c46c);
      iVar7 = (*pcVar9)(uVar12,pDVar13);
      if (iVar7 != 0) {
        FUN_1000948b((int)pDVar8,0);
        DVar11 = GetCurrentThreadId();
        pDVar8[1] = 0xffffffff;
        *pDVar8 = DVar11;
        return 1;
      }
      FUN_10003a70(pDVar8);
    }
  }
  return 0;
}


// 100070c4 ___DllMainCRTStartup

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x10007156) */
/* WARNING: Removing unreachable block (ram,0x10007103) */
/* WARNING: Removing unreachable block (ram,0x10007183) */
/* Library Function - Single Match
    ___DllMainCRTStartup
   
   Library: Visual Studio 2005 Release */

int __fastcall ___DllMainCRTStartup(int param_1,int param_2,undefined4 param_3)

{
  int iVar1;
  int local_20;
  
  if (((param_2 == 0) && (DAT_1001c130 == 0)) ||
     (((param_2 == 1 || (param_2 == 2)) &&
      (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)))) {
    local_20 = 0;
  }
  else {
    local_20 = FUN_1000b543();
    if ((param_2 == 1) && (local_20 == 0)) {
      FUN_1000b543();
      __CRT_INIT_12(param_3,0,param_1);
    }
    if (((param_2 == 0) || (param_2 == 3)) &&
       (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)) {
      local_20 = 0;
    }
  }
  return local_20;
}


// 100071ba entry

void entry(undefined4 param_1,int param_2,int param_3)

{
  if (param_2 == 1) {
    ___security_init_cookie();
  }
  ___DllMainCRTStartup(param_3,param_2,param_1);
  return;
}


// 100071f0 _strchr

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


// 100072b0 _strstr

/* Library Function - Single Match
    _strstr
   
   Libraries: Visual Studio 2005, Visual Studio 2008, Visual Studio 2010, Visual Studio 2019 */

char * __cdecl _strstr(char *_Str,char *_SubStr)

{
  char *pcVar1;
  char *pcVar2;
  char cVar3;
  uint uVar4;
  char cVar5;
  uint uVar6;
  uint uVar7;
  char *pcVar8;
  uint *puVar9;
  char *pcVar10;
  
  cVar3 = *_SubStr;
  if (cVar3 == '\0') {
    return _Str;
  }
  if (_SubStr[1] == '\0') {
    uVar4 = (uint)_Str & 3;
    while (uVar4 != 0) {
      if ((char)*(uint *)_Str == cVar3) {
        return (char *)(uint *)_Str;
      }
      if ((char)*(uint *)_Str == '\0') {
        return (char *)0x0;
      }
      uVar4 = (uint)((int)_Str + 1) & 3;
      _Str = (char *)((int)_Str + 1);
    }
    while( true ) {
      while( true ) {
        uVar4 = *(uint *)_Str;
        uVar7 = uVar4 ^ CONCAT22(CONCAT11(cVar3,cVar3),CONCAT11(cVar3,cVar3));
        uVar6 = uVar4 ^ 0xffffffff ^ uVar4 + 0x7efefeff;
        puVar9 = (uint *)((int)_Str + 4);
        if (((uVar7 ^ 0xffffffff ^ uVar7 + 0x7efefeff) & 0x81010100) != 0) break;
        _Str = (char *)puVar9;
        if ((uVar6 & 0x81010100) != 0) {
          if ((uVar6 & 0x1010100) != 0) {
            return (char *)0x0;
          }
          if ((uVar4 + 0x7efefeff & 0x80000000) == 0) {
            return (char *)0x0;
          }
        }
      }
      uVar4 = *(uint *)_Str;
      if ((char)uVar4 == cVar3) {
        return (char *)(uint *)_Str;
      }
      if ((char)uVar4 == '\0') {
        return (char *)0x0;
      }
      cVar5 = (char)(uVar4 >> 8);
      if (cVar5 == cVar3) {
        return (char *)((int)_Str + 1);
      }
      if (cVar5 == '\0') break;
      cVar5 = (char)(uVar4 >> 0x10);
      if (cVar5 == cVar3) {
        return (char *)((int)_Str + 2);
      }
      if (cVar5 == '\0') {
        return (char *)0x0;
      }
      cVar5 = (char)(uVar4 >> 0x18);
      if (cVar5 == cVar3) {
        return (char *)((int)_Str + 3);
      }
      _Str = (char *)puVar9;
      if (cVar5 == '\0') {
        return (char *)0x0;
      }
    }
    return (char *)0x0;
  }
  do {
    cVar5 = *_Str;
    do {
      while (_Str = _Str + 1, cVar5 != cVar3) {
        if (cVar5 == '\0') {
          return (char *)0x0;
        }
        cVar5 = *_Str;
      }
      cVar5 = *_Str;
      pcVar10 = _Str + 1;
      pcVar8 = _SubStr;
    } while (cVar5 != _SubStr[1]);
    do {
      if (pcVar8[2] == '\0') {
LAB_10007329:
        return _Str + -1;
      }
      if (*pcVar10 != pcVar8[2]) break;
      pcVar1 = pcVar8 + 3;
      if (*pcVar1 == '\0') goto LAB_10007329;
      pcVar2 = pcVar10 + 1;
      pcVar8 = pcVar8 + 2;
      pcVar10 = pcVar10 + 2;
    } while (*pcVar1 == *pcVar2);
  } while( true );
}


// 10007336 __freea

/* Library Function - Single Match
    __freea
   
   Library: Visual Studio 2005 Release */

void __cdecl __freea(void *_Memory)

{
  if ((_Memory != (void *)0x0) && (*(int *)((int)_Memory + -8) == 0xdddd)) {
    FUN_10003a70((int *)((int)_Memory + -8));
  }
  return;
}


// 10007351 _LocaleUpdate::_LocaleUpdate

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
    if ((*(undefined **)this != PTR_DAT_10017830) && ((p_Var2->_ownlocale & DAT_1001774c) == 0)) {
      ptVar3 = ___updatetlocinfo();
      *(pthreadlocinfo *)this = ptVar3;
    }
    if ((*(undefined **)(this + 4) != PTR_DAT_10017650) &&
       ((*(uint *)(*(int *)(this + 8) + 0x70) & DAT_1001774c) == 0)) {
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


// 100073d3 __tolower_l

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
LAB_10007432:
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
      pDVar4 = FUN_1000ac50();
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
    if (iVar3 == 0) goto LAB_10007432;
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


// 100074ea _tolower

/* Library Function - Single Match
    _tolower
   
   Library: Visual Studio 2005 Release */

int __cdecl _tolower(int _C)

{
  if (DAT_1001c5f8 == 0) {
    if (_C - 0x41U < 0x1a) {
      return _C + 0x20;
    }
  }
  else {
    _C = __tolower_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 10007520 _strncat

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
      if ((byte)uVar4 == 0) goto LAB_1000757d;
      uVar4 = (uint)puVar5 & 3;
    }
    do {
      do {
        puVar6 = puVar5;
        puVar5 = puVar6 + 1;
      } while (((*puVar6 ^ 0xffffffff ^ *puVar6 + 0x7efefeff) & 0x81010100) == 0);
      uVar4 = *puVar6;
      if ((char)uVar4 == '\0') goto LAB_1000758f;
      if ((char)(uVar4 >> 8) == '\0') {
        puVar6 = (uint *)((int)puVar6 + 1);
        goto LAB_1000758f;
      }
      if ((uVar4 & 0xff0000) == 0) {
        puVar6 = (uint *)((int)puVar6 + 2);
        goto LAB_1000758f;
      }
    } while ((uVar4 & 0xff000000) != 0);
LAB_1000757d:
    puVar6 = (uint *)((int)puVar5 + -1);
LAB_1000758f:
    if (((uint)_Source & 3) == 0) {
      uVar3 = _Count >> 2;
    }
    else {
      do {
        bVar1 = (byte)*(uint *)_Source;
        uVar4 = (uint)bVar1;
        _Source = (char *)((int)_Source + 1);
        if (bVar1 == 0) goto LAB_100075ea;
        *(byte *)puVar6 = bVar1;
        puVar6 = (uint *)((int)puVar6 + 1);
        _Count = _Count - 1;
        if (_Count == 0) goto LAB_100075e0;
      } while (((uint)_Source & 3) != 0);
      uVar3 = _Count >> 2;
    }
    for (; uVar3 != 0; uVar3 = uVar3 - 1) {
      uVar2 = *(uint *)_Source;
      uVar4 = *(uint *)_Source;
      _Source = (char *)((int)_Source + 4);
      if (((uVar2 ^ 0xffffffff ^ uVar2 + 0x7efefeff) & 0x81010100) != 0) {
        if ((char)uVar4 == '\0') {
LAB_100075ea:
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
LAB_100075e0:
    *(byte *)puVar6 = (byte)_Count;
  }
  return _Dest;
}


// 10007660 _memmove

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
          goto switchD_10007843_caseD_2;
        case 3:
          goto switchD_10007843_caseD_3;
        }
        goto switchD_10007843_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_10007843_caseD_0;
      case 1:
        goto switchD_10007843_caseD_1;
      case 2:
        goto switchD_10007843_caseD_2;
      case 3:
        goto switchD_10007843_caseD_3;
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
              goto switchD_10007843_caseD_2;
            case 3:
              goto switchD_10007843_caseD_3;
            }
            goto switchD_10007843_caseD_1;
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
              goto switchD_10007843_caseD_2;
            case 3:
              goto switchD_10007843_caseD_3;
            }
            goto switchD_10007843_caseD_1;
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
              goto switchD_10007843_caseD_2;
            case 3:
              goto switchD_10007843_caseD_3;
            }
            goto switchD_10007843_caseD_1;
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
switchD_10007843_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_10007843_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_10007843_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_10007843_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_1001dd3c != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_100076bc_caseD_2;
      case 3:
        goto switchD_100076bc_caseD_3;
      }
      goto switchD_100076bc_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_100076bc_caseD_0;
    case 1:
      goto switchD_100076bc_caseD_1;
    case 2:
      goto switchD_100076bc_caseD_2;
    case 3:
      goto switchD_100076bc_caseD_3;
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
            goto switchD_100076bc_caseD_2;
          case 3:
            goto switchD_100076bc_caseD_3;
          }
          goto switchD_100076bc_caseD_1;
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
            goto switchD_100076bc_caseD_2;
          case 3:
            goto switchD_100076bc_caseD_3;
          }
          goto switchD_100076bc_caseD_1;
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
            goto switchD_100076bc_caseD_2;
          case 3:
            goto switchD_100076bc_caseD_3;
          }
          goto switchD_100076bc_caseD_1;
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
switchD_100076bc_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_100076bc_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_100076bc_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_100076bc_caseD_0:
  return _Dst;
}


// 100079c5 __validdrive

/* Library Function - Single Match
    __validdrive
   
   Library: Visual Studio 2005 Release */

int __cdecl __validdrive(uint param_1)

{
  char cVar1;
  UINT UVar2;
  
  if (param_1 != 0) {
    cVar1 = (char)param_1;
    param_1 = (uint)CONCAT12(0x5c,CONCAT11(0x3a,cVar1 + '@'));
    UVar2 = GetDriveTypeA((LPCSTR)&param_1);
    if ((UVar2 == 0) || (UVar2 == 1)) {
      return 0;
    }
  }
  return 1;
}


// 100079fc __getdcwd_nolock

/* Library Function - Single Match
    __getdcwd_nolock
   
   Library: Visual Studio 2005 Release */

LPSTR __cdecl __getdcwd_nolock(uint param_1,LPSTR param_2,DWORD param_3)

{
  int iVar1;
  DWORD *pDVar2;
  uint uVar3;
  DWORD DVar4;
  LPSTR lpBuffer;
  LPSTR local_c;
  undefined4 local_8;
  
  if (param_1 == 0) {
    uVar3 = __getdrive();
  }
  else {
    iVar1 = __validdrive(param_1);
    uVar3 = param_1;
    if (iVar1 == 0) {
      pDVar2 = FUN_1000ac63();
      *pDVar2 = 0xf;
      pDVar2 = FUN_1000ac50();
      *pDVar2 = 0xd;
      FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return (LPSTR)0x0;
    }
  }
  if (param_2 == (LPSTR)0x0) {
    param_1 = 0;
  }
  else {
    if ((int)param_3 < 1) {
      pDVar2 = FUN_1000ac50();
      *pDVar2 = 0x16;
      FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return (LPSTR)0x0;
    }
    param_1 = param_3;
    *param_2 = '\0';
  }
  if (uVar3 == 0) {
    local_8 = CONCAT22(local_8._2_2_,0x2e);
  }
  else {
    local_8 = (uint)CONCAT12(0x2e,CONCAT11(0x3a,(char)uVar3 + '@'));
  }
  DVar4 = GetFullPathNameA((LPCSTR)&local_8,param_1,param_2,&local_c);
  if (DVar4 != 0) {
    if (param_2 != (LPSTR)0x0) {
      if ((int)param_1 <= (int)DVar4) {
        pDVar2 = FUN_1000ac50();
        *pDVar2 = 0x22;
        *param_2 = '\0';
        return (LPSTR)0x0;
      }
      return param_2;
    }
    if ((int)param_3 < (int)DVar4) {
      param_3 = DVar4;
    }
    lpBuffer = (LPSTR)FUN_10003a90(param_3,1);
    if (lpBuffer == (LPSTR)0x0) {
      pDVar2 = FUN_1000ac50();
      *pDVar2 = 0xc;
      pDVar2 = FUN_1000ac63();
      *pDVar2 = 8;
      return (LPSTR)0x0;
    }
    DVar4 = GetFullPathNameA((LPCSTR)&local_8,param_3,lpBuffer,&local_c);
    if ((DVar4 != 0) && ((int)DVar4 < (int)param_3)) {
      return lpBuffer;
    }
  }
  DVar4 = GetLastError();
  FUN_1000ac76(DVar4);
  return (LPSTR)0x0;
}


// 10007b31 __getcwd

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __getcwd
   
   Library: Visual Studio 2005 Release */

char * __cdecl __getcwd(char *_DstBuf,int _SizeInBytes)

{
  LPSTR pCVar1;
  
  __lock(7);
  pCVar1 = __getdcwd_nolock(0,_DstBuf,_SizeInBytes);
  FUN_10007b71();
  return pCVar1;
}


// 10007b71 FUN_10007b71

void FUN_10007b71(void)

{
  FUN_1000c5f2(7);
  return;
}


// 10007b80 _strncpy

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
        goto joined_r0x10007bcc;
      }
    }
    do {
      if (((uint)puVar5 & 3) == 0) {
        uVar4 = _Count >> 2;
        cVar3 = '\0';
        if (uVar4 == 0) goto LAB_10007c13;
        goto LAB_10007c89;
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
joined_r0x10007c85:
          while( true ) {
            uVar4 = uVar4 - 1;
            puVar5 = puVar5 + 1;
            if (uVar4 == 0) break;
LAB_10007c89:
            *puVar5 = 0;
          }
          cVar3 = '\0';
          _Count = _Count & 3;
          if (_Count != 0) goto LAB_10007c13;
          return _Dest;
        }
        if ((char)(uVar2 >> 8) == '\0') {
          *puVar5 = uVar2 & 0xff;
          goto joined_r0x10007c85;
        }
        if ((uVar2 & 0xff0000) == 0) {
          *puVar5 = uVar2 & 0xffff;
          goto joined_r0x10007c85;
        }
        if ((uVar2 & 0xff000000) == 0) {
          *puVar5 = uVar2;
          goto joined_r0x10007c85;
        }
      }
      *puVar5 = uVar2;
      puVar5 = puVar5 + 1;
      uVar4 = uVar4 - 1;
joined_r0x10007bcc:
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
LAB_10007c13:
        *(char *)puVar5 = cVar3;
        puVar5 = (uint *)((int)puVar5 + 1);
      }
      return _Dest;
    }
    _Count = _Count - 1;
  } while (_Count != 0);
  return _Dest;
}


// 10007e1a __atof_l

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
    pDVar2 = FUN_1000ac50();
    *pDVar2 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10007ec3 _atof

/* Library Function - Single Match
    _atof
   
   Library: Visual Studio 2005 Release */

double __cdecl _atof(char *_String)

{
  double dVar1;
  
  dVar1 = __atof_l(_String,(_locale_t)0x0);
  return dVar1;
}


// 10007ee0 __alloca_probe_16

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


// 10007ef6 __alloca_probe_8

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


// 10007f10 __alloca_probe

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


// 10007f3b _sprintf

/* Library Function - Single Match
    _sprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl _sprintf(char *_Dest,char *_Format,...)

{
  DWORD *pDVar1;
  int iVar2;
  FILE local_24;
  
  if ((_Format == (char *)0x0) || (_Dest == (char *)0x0)) {
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  else {
    local_24._base = _Dest;
    local_24._ptr = _Dest;
    local_24._cnt = 0x7fffffff;
    local_24._flag = 0x42;
    iVar2 = FUN_1000a157(&local_24,(byte *)_Format,(localeinfo_struct *)0x0,
                         (wchar_t *)&stack0x0000000c);
    local_24._cnt = local_24._cnt + -1;
    if (local_24._cnt < 0) {
      __flsbuf(0,&local_24);
    }
    else {
      *local_24._ptr = '\0';
    }
  }
  return iVar2;
}


// 10007fb6 FID_conflict:__isdigit_l

/* Library Function - Multiple Matches With Different Base Names
    __iscntrl_l
    __isdigit_l
    __islower_l
    __ispunct_l
     6 names - too many to list
   
   Library: Visual Studio 2005 Release */

int __cdecl FID_conflict___isdigit_l(int _C,_locale_t _Locale)

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


// 10008005 _isdigit

/* Library Function - Single Match
    _isdigit
   
   Library: Visual Studio 2005 Release */

int __cdecl _isdigit(int _C)

{
  int iVar1;
  
  if (DAT_1001c5f8 == 0) {
    return (byte)PTR_DAT_10017820[_C * 2] & 4;
  }
  iVar1 = FID_conflict___isdigit_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 1000802e strtoxl

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
    pDVar3 = FUN_1000ac50();
    *pDVar3 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
LAB_100080e5:
    bVar8 = *pbVar9;
    pbVar9 = pbVar2 + 2;
  }
  else if (bVar8 == 0x2b) goto LAB_100080e5;
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
      goto LAB_1000814b;
    }
    if ((*pbVar9 != 0x78) && (*pbVar9 != 0x58)) {
      param_4 = 8;
      goto LAB_1000814b;
    }
    param_4 = 0x10;
  }
  else if ((param_4 != 0x10) || (bVar8 != 0x30)) goto LAB_1000814b;
  if ((*pbVar9 == 0x78) || (*pbVar9 == 0x58)) {
    bVar8 = pbVar9[1];
    pbVar9 = pbVar9 + 2;
  }
LAB_1000814b:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  do {
    uVar1 = *(ushort *)(ptVar5[1].lc_category[0].locale + (uint)bVar8 * 2);
    if ((uVar1 & 4) == 0) {
      if ((uVar1 & 0x103) == 0) {
LAB_100081a5:
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
          pDVar3 = FUN_1000ac50();
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
    if ((uint)param_4 <= uVar6) goto LAB_100081a5;
    if ((local_8 < uVar4) ||
       ((local_8 == uVar4 && (uVar6 <= (uint)(0xffffffff % (ulonglong)(uint)param_4))))) {
      local_8 = local_8 * param_4 + uVar6;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (char **)0x0) goto LAB_100081a5;
    }
    bVar8 = *pbVar9;
    pbVar9 = pbVar9 + 1;
  } while( true );
}


// 10008259 _strtol

/* Library Function - Single Match
    _strtol
   
   Library: Visual Studio 2005 Release */

long __cdecl _strtol(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_1001c5f8 == 0) {
    ppuVar2 = &PTR_DAT_10017838;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 10008282 __forcdecpt_l

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


// 100082f3 __cropzeros_l

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


// 10008389 __fassign_l

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


// 100083e0 __shift

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


// 10008419 __cftoe2_l

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
    pDVar1 = FUN_1000ac50();
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
      if (((DAT_1001c770 & 1) != 0) && (_Dst[2] == '0')) {
        pcVar9 = (char *)0x3;
        _memmove(_Dst + 2,_Dst + 3,3);
      }
      if (local_8 == '\0') {
        return (DWORD)pcVar9;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return (DWORD)pcVar9;
    }
    pDVar1 = FUN_1000ac50();
    DVar7 = 0x22;
  }
  DVar8 = 0;
  *pDVar1 = DVar7;
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return DVar8;
}


// 10008586 __cftoe_l

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10008654 __cftoe

/* Library Function - Single Match
    __cftoe
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __cftoe(double *_Value,char *_Buf,size_t _SizeInBytes,int _Dec,int _Caps)

{
  errno_t eVar1;
  
  eVar1 = __cftoe_l(_Value,_Buf,_SizeInBytes,_Dec,_Caps,(localeinfo_struct *)0x0);
  return eVar1;
}


// 10008672 __cftoa_l

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
    pDVar2 = FUN_1000ac50();
    DVar15 = 0x16;
LAB_100086ab:
    DVar16 = 0;
    *pDVar2 = DVar15;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_1c != '\0') {
      *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
    }
    return DVar16;
  }
  *param_2 = 0;
  if (param_3 <= param_4 + 0xb) {
    pDVar2 = FUN_1000ac50();
    DVar15 = 0x22;
    goto LAB_100086ab;
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
    goto LAB_100089cf;
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
LAB_1000897e:
    if ((-1 < (int)uVar8) && ((0 < (int)uVar8 || (99 < uVar7)))) goto LAB_10008989;
  }
  else {
    uVar14 = __alldvrm(uVar7,uVar8,1000,0);
    local_14 = (undefined4)((ulonglong)uVar14 >> 0x20);
    *pcVar10 = (char)uVar14 + '0';
    pcVar11 = pcVar4 + 3;
    uVar8 = 0;
    uVar7 = extraout_ECX;
    if (pcVar11 == pcVar10) goto LAB_1000897e;
LAB_10008989:
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
LAB_100089cf:
  if (local_1c == '\0') {
    return unaff_EBX;
  }
  *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  return unaff_EBX;
}


// 100089e3 __cftof2_l

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
    pDVar2 = FUN_1000ac50();
    uVar4 = 0x16;
    *pDVar2 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10008ad8 __cftof_l

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10008b91 __cftog_l

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_34,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar2 = FUN_1000ac50();
    *pDVar2 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10008c89 __cfltcvt_l

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


// 10008d0f __cfltcvt

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


// 10008d30 __initp_misc_cfltcvt_tab

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
    piVar1 = (int *)((int)&PTR_LAB_10016ef0 + uVar3);
    iVar2 = FUN_1000932e(*piVar1);
    uVar3 = uVar3 + 4;
    *piVar1 = iVar2;
  } while (uVar3 < 0x28);
  return;
}


// 10008d4f __setdefaultprecision

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


// 10008d78 __ms_p5_test_fdiv

/* WARNING: Removing unreachable block (ram,0x10008dab) */
/* Library Function - Single Match
    __ms_p5_test_fdiv
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

undefined4 __ms_p5_test_fdiv(void)

{
  return 0;
}


// 10008db4 __ms_p5_mp_test_fdiv

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


// 10008ddd fastzero_I

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


// 10008e34 __VEC_memzero

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


// 10008ed7 type_info::_Type_info_dtor

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
  piVar1 = DAT_1001c148;
  if (*(int *)(param_1 + 4) != 0) {
    piVar2 = (int *)&DAT_1001c144;
    do {
      piVar3 = piVar2;
      if (DAT_1001c148 == (int *)0x0) goto LAB_10008f1b;
      piVar2 = DAT_1001c148;
    } while (*DAT_1001c148 != *(int *)(param_1 + 4));
    piVar3[1] = DAT_1001c148[1];
    FUN_10003a70(piVar1);
LAB_10008f1b:
    FUN_10003a70(*(undefined4 *)(param_1 + 4));
    *(undefined4 *)(param_1 + 4) = 0;
  }
  FUN_10008f3e();
  return;
}


// 10008f3e FUN_10008f3e

void FUN_10008f3e(void)

{
  FUN_1000c5f2(0xe);
  return;
}


// 10008f50 _strcmp

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
      if (bVar4 != *_Str2) goto LAB_10008f94;
      _Str2 = _Str2 + 1;
      if (bVar4 == 0) {
        return 0;
      }
      if (((uint)_Str1 & 2) == 0) goto LAB_10008f60;
    }
    uVar1 = *(undefined2 *)_Str1;
    _Str1 = _Str1 + 2;
    bVar4 = (byte)uVar1;
    bVar5 = bVar4 < (byte)*_Str2;
    if (bVar4 != *_Str2) goto LAB_10008f94;
    if (bVar4 == 0) {
      return 0;
    }
    bVar4 = (byte)((ushort)uVar1 >> 8);
    bVar5 = bVar4 < ((byte *)_Str2)[1];
    if (bVar4 != ((byte *)_Str2)[1]) goto LAB_10008f94;
    if (bVar4 == 0) {
      return 0;
    }
    _Str2 = (char *)((byte *)_Str2 + 2);
  }
LAB_10008f60:
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
LAB_10008f94:
  return (uint)bVar5 * -2 + 1;
}


// 10008fd8 _abort

/* Library Function - Single Match
    _abort
   
   Library: Visual Studio 2005 Release */

void __cdecl _abort(void)

{
  _PHNDLR p_Var1;
  EXCEPTION_RECORD local_32c;
  _EXCEPTION_POINTERS local_2dc;
  undefined4 local_2d4;
  
  if (((byte)DAT_10016f20 & 1) != 0) {
    FUN_100090e9(10);
  }
  p_Var1 = ___get_sigabrt();
  if (p_Var1 != (_PHNDLR)0x0) {
    _raise(0x16);
  }
  if (((byte)DAT_10016f20 & 2) != 0) {
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


// 100090cb FUN_100090cb

void __cdecl FUN_100090cb(uint param_1,uint param_2)

{
  DAT_10016f20 = ~param_2 & DAT_10016f20 | param_1 & param_2;
  return;
}


// 100090e9 FUN_100090e9

void __cdecl FUN_100090e9(int param_1)

{
  undefined4 *puVar1;
  int iVar2;
  errno_t eVar3;
  DWORD DVar4;
  size_t sVar5;
  HANDLE hFile;
  uint uVar6;
  DWORD *lpNumberOfBytesWritten;
  LPOVERLAPPED lpOverlapped;
  DWORD DStack_4;
  
  uVar6 = 0;
  do {
    if (param_1 == (&DAT_10016f28)[uVar6 * 2]) break;
    uVar6 = uVar6 + 1;
  } while (uVar6 < 0x17);
  if (uVar6 < 0x17) {
    iVar2 = __set_error_mode(3);
    if ((iVar2 == 1) || ((iVar2 = __set_error_mode(3), iVar2 == 0 && (DAT_1001c140 == 1)))) {
      hFile = GetStdHandle(0xfffffff4);
      if ((hFile != (HANDLE)0x0) && (hFile != (HANDLE)0xffffffff)) {
        lpOverlapped = (LPOVERLAPPED)0x0;
        lpNumberOfBytesWritten = &DStack_4;
        puVar1 = (undefined4 *)(uVar6 * 8 + 0x10016f2c);
        sVar5 = _strlen((char *)*puVar1);
        WriteFile(hFile,(LPCVOID)*puVar1,sVar5,lpNumberOfBytesWritten,lpOverlapped);
      }
    }
    else if (param_1 != 0xfc) {
      eVar3 = _strcpy_s(&DAT_1001c150,0x314,"Runtime Error!\n\nProgram: ");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      DAT_1001c26d = 0;
      DVar4 = GetModuleFileNameA((HMODULE)0x0,&DAT_1001c169,0x104);
      if ((DVar4 == 0) &&
         (eVar3 = _strcpy_s(&DAT_1001c169,0x2fb,"<program name unknown>"), eVar3 != 0)) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      sVar5 = _strlen(&DAT_1001c169);
      if (0x3c < sVar5 + 1) {
        sVar5 = _strlen(&DAT_1001c169);
        eVar3 = _strncpy_s((char *)(sVar5 + 0x1001c12e),
                           (int)&DAT_1001c464 - (int)(sVar5 + 0x1001c12e),"...",3);
        if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
      }
      eVar3 = _strcat_s(&DAT_1001c150,0x314,"\n\n");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      eVar3 = _strcat_s(&DAT_1001c150,0x314,*(char **)(uVar6 * 8 + 0x10016f2c));
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      FUN_1000d6e9(&DAT_1001c150,"Microsoft Visual C++ Runtime Library",0x12010);
    }
  }
  return;
}


// 10009289 __FF_MSGBANNER

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
    if (DAT_1001c140 != 1) {
      return;
    }
  }
  FUN_100090e9(0xfc);
  FUN_100090e9(0xff);
  return;
}


// 100092c2 FUN_100092c2

undefined4 FUN_100092c2(void)

{
  int iVar1;
  HMODULE pHVar2;
  int iVar3;
  uint uVar4;
  char *_Str2;
  int local_8;
  
  uVar4 = 0;
  local_8 = 0;
  FUN_10009b3b(&local_8);
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


// 1000932e FUN_1000932e

int __cdecl FUN_1000932e(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_10016fe4);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10016fe0 != -1)) {
    iVar3 = DAT_10016fe0;
    pcVar2 = (code *)TlsGetValue(DAT_10016fe4);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1f8);
      goto LAB_10009388;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_100092c2();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"EncodePointer");
LAB_10009388:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 1000939c __encoded_null

/* Library Function - Single Match
    __encoded_null
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __encoded_null(void)

{
  FUN_1000932e(0);
  return;
}


// 100093a5 FUN_100093a5

int __cdecl FUN_100093a5(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_10016fe4);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10016fe0 != -1)) {
    iVar3 = DAT_10016fe0;
    pcVar2 = (code *)TlsGetValue(DAT_10016fe4);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1fc);
      goto LAB_100093ff;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_100092c2();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"DecodePointer");
LAB_100093ff:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 1000941c FUN_1000941c

LPVOID FUN_1000941c(void)

{
  LPVOID lpTlsValue;
  
  lpTlsValue = TlsGetValue(DAT_10016fe4);
  if (lpTlsValue == (LPVOID)0x0) {
    lpTlsValue = (LPVOID)FUN_100093a5(DAT_1001c468);
    TlsSetValue(DAT_10016fe4,lpTlsValue);
  }
  return lpTlsValue;
}


// 1000944e __mtterm

/* Library Function - Single Match
    __mtterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __mtterm(void)

{
  code *pcVar1;
  int iVar2;
  
  if (DAT_10016fe0 != -1) {
    iVar2 = DAT_10016fe0;
    pcVar1 = (code *)FUN_100093a5(DAT_1001c470);
    (*pcVar1)(iVar2);
    DAT_10016fe0 = -1;
  }
  if (DAT_10016fe4 != 0xffffffff) {
    TlsFree(DAT_10016fe4);
    DAT_10016fe4 = 0xffffffff;
  }
  __mtdeletelocks();
  return;
}


// 1000948b FUN_1000948b

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

void __cdecl FUN_1000948b(int param_1,int param_2)

{
  HMODULE hModule;
  int iVar1;
  FARPROC pFVar2;
  
  hModule = GetModuleHandleA("KERNEL32.DLL");
  *(undefined **)(param_1 + 0x5c) = &DAT_100171a0;
  *(undefined4 *)(param_1 + 0x14) = 1;
  if (hModule != (HMODULE)0x0) {
    iVar1 = FUN_100092c2();
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
  *(undefined **)(param_1 + 0x68) = &DAT_10017228;
  InterlockedIncrement((LONG *)&DAT_10017228);
  __lock(0xc);
  *(int *)(param_1 + 0x6c) = param_2;
  if (param_2 == 0) {
    *(undefined **)(param_1 + 0x6c) = PTR_DAT_10017830;
  }
  ___addlocaleref(*(LONG **)(param_1 + 0x6c));
  FUN_10009541();
  return;
}


// 10009541 FUN_10009541

void FUN_10009541(void)

{
  FUN_1000c5f2(0xc);
  return;
}


// 1000954a FUN_1000954a

DWORD * FUN_1000954a(void)

{
  DWORD dwErrCode;
  code *pcVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD DVar4;
  undefined4 uVar5;
  DWORD *pDVar6;
  
  dwErrCode = GetLastError();
  uVar5 = DAT_10016fe0;
  pcVar1 = (code *)FUN_1000941c();
  pDVar2 = (DWORD *)(*pcVar1)(uVar5);
  if (pDVar2 == (DWORD *)0x0) {
    pDVar2 = (DWORD *)FUN_10003bf0(1,0x214);
    if (pDVar2 != (DWORD *)0x0) {
      uVar5 = DAT_10016fe0;
      pDVar6 = pDVar2;
      pcVar1 = (code *)FUN_100093a5(DAT_1001c46c);
      iVar3 = (*pcVar1)(uVar5,pDVar6);
      if (iVar3 == 0) {
        FUN_10003a70(pDVar2);
        pDVar2 = (DWORD *)0x0;
      }
      else {
        FUN_1000948b((int)pDVar2,0);
        DVar4 = GetCurrentThreadId();
        pDVar2[1] = 0xffffffff;
        *pDVar2 = DVar4;
      }
    }
  }
  SetLastError(dwErrCode);
  return pDVar2;
}


// 100095c1 __getptd

/* Library Function - Single Match
    __getptd
   
   Library: Visual Studio 2005 Release */

_ptiddata __cdecl __getptd(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = (_ptiddata)FUN_1000954a();
  if (p_Var1 == (_ptiddata)0x0) {
    __amsg_exit(0x10);
  }
  return p_Var1;
}


// 100095d9 __freefls@4

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
      FUN_10003a70(*(int *)(param_1 + 0x24));
    }
    if (*(int *)(param_1 + 0x2c) != 0) {
      FUN_10003a70(*(int *)(param_1 + 0x2c));
    }
    if (*(int *)(param_1 + 0x34) != 0) {
      FUN_10003a70(*(int *)(param_1 + 0x34));
    }
    if (*(int *)(param_1 + 0x3c) != 0) {
      FUN_10003a70(*(int *)(param_1 + 0x3c));
    }
    if (*(int *)(param_1 + 0x44) != 0) {
      FUN_10003a70(*(int *)(param_1 + 0x44));
    }
    if (*(int *)(param_1 + 0x48) != 0) {
      FUN_10003a70(*(int *)(param_1 + 0x48));
    }
    if (*(undefined **)(param_1 + 0x5c) != &DAT_100171a0) {
      FUN_10003a70(*(undefined **)(param_1 + 0x5c));
    }
    __lock(0xd);
    pLVar1 = *(LONG **)(param_1 + 0x68);
    if (pLVar1 != (LONG *)0x0) {
      LVar2 = InterlockedDecrement(pLVar1);
      if ((LVar2 == 0) && (pLVar1 != (LONG *)&DAT_10017228)) {
        FUN_10003a70(pLVar1);
      }
    }
    FUN_100096e5();
    __lock(0xc);
    pLVar1 = *(LONG **)(param_1 + 0x6c);
    if (pLVar1 != (LONG *)0x0) {
      ___removelocaleref(pLVar1);
      if (((pLVar1 != (LONG *)PTR_DAT_10017830) && (pLVar1 != (LONG *)&DAT_10017758)) &&
         (*pLVar1 == 0)) {
        ___freetlocinfo((int)pLVar1);
      }
    }
    FUN_100096f1();
    FUN_10003a70(param_1);
  }
  return;
}


// 100096e5 FUN_100096e5

void FUN_100096e5(void)

{
  FUN_1000c5f2(0xd);
  return;
}


// 100096f1 FUN_100096f1

void FUN_100096f1(void)

{
  FUN_1000c5f2(0xc);
  return;
}


// 100096fa __freeptd

/* Library Function - Single Match
    __freeptd
   
   Library: Visual Studio 2005 Release */

void __cdecl __freeptd(_ptiddata _Ptd)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  undefined4 uVar4;
  
  if (DAT_10016fe0 != -1) {
    if ((_Ptd == (_ptiddata)0x0) && (pvVar1 = TlsGetValue(DAT_10016fe4), pvVar1 != (LPVOID)0x0)) {
      iVar3 = DAT_10016fe0;
      pcVar2 = (code *)TlsGetValue(DAT_10016fe4);
      _Ptd = (_ptiddata)(*pcVar2)(iVar3);
    }
    uVar4 = 0;
    iVar3 = DAT_10016fe0;
    pcVar2 = (code *)FUN_100093a5(DAT_1001c46c);
    (*pcVar2)(iVar3,uVar4);
    __freefls_4((int)_Ptd);
  }
  if (DAT_10016fe4 != 0xffffffff) {
    TlsSetValue(DAT_10016fe4,(LPVOID)0x0);
  }
  return;
}


// 10009763 __mtinit

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
  DAT_1001c464 = GetProcAddress(hModule,"FlsAlloc");
  DAT_1001c468 = GetProcAddress(hModule,"FlsGetValue");
  DAT_1001c46c = GetProcAddress(hModule,"FlsSetValue");
  DAT_1001c470 = GetProcAddress(hModule,"FlsFree");
  if ((((DAT_1001c464 == (FARPROC)0x0) || (DAT_1001c468 == (FARPROC)0x0)) ||
      (DAT_1001c46c == (FARPROC)0x0)) || (DAT_1001c470 == (FARPROC)0x0)) {
    DAT_1001c468 = TlsGetValue_exref;
    DAT_1001c464 = (FARPROC)&LAB_10009413;
    DAT_1001c46c = TlsSetValue_exref;
    DAT_1001c470 = TlsFree_exref;
  }
  DAT_10016fe4 = TlsAlloc();
  if ((DAT_10016fe4 != 0xffffffff) && (BVar1 = TlsSetValue(DAT_10016fe4,DAT_1001c468), BVar1 != 0))
  {
    __init_pointers();
    DAT_1001c464 = (FARPROC)FUN_1000932e((int)DAT_1001c464);
    DAT_1001c468 = (FARPROC)FUN_1000932e((int)DAT_1001c468);
    DAT_1001c46c = (FARPROC)FUN_1000932e((int)DAT_1001c46c);
    DAT_1001c470 = (FARPROC)FUN_1000932e((int)DAT_1001c470);
    iVar2 = __mtinitlocks();
    if (iVar2 != 0) {
      pcVar6 = __freefls_4;
      pcVar3 = (code *)FUN_100093a5((int)DAT_1001c464);
      DAT_10016fe0 = (*pcVar3)(pcVar6);
      if ((DAT_10016fe0 != -1) && (pDVar4 = (DWORD *)FUN_10003bf0(1,0x214), pDVar4 != (DWORD *)0x0))
      {
        iVar2 = DAT_10016fe0;
        pDVar7 = pDVar4;
        pcVar3 = (code *)FUN_100093a5((int)DAT_1001c46c);
        iVar2 = (*pcVar3)(iVar2,pDVar7);
        if (iVar2 != 0) {
          FUN_1000948b((int)pDVar4,0);
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


// 100098e7 FUN_100098e7

void __cdecl FUN_100098e7(undefined4 param_1)

{
  DAT_1001c47c = param_1;
  return;
}


// 100098f1 FUN_100098f1

void __cdecl FUN_100098f1(undefined4 *param_1,undefined4 *param_2,uint param_3)

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


// 10009978 __VEC_memcpy

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
      FUN_100098f1(param_1,param_2,param_3 - uVar3);
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


// 10009a5b __amsg_exit

/* Library Function - Single Match
    __amsg_exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __amsg_exit(int param_1)

{
  code *pcVar1;
  
  __FF_MSGBANNER();
  FUN_100090e9(param_1);
  pcVar1 = (code *)FUN_100093a5((int)PTR___exit_10016fe8);
  (*pcVar1)(0xff);
  return;
}


// 10009a7f FUN_10009a7f

void __cdecl FUN_10009a7f(undefined4 param_1)

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


// 10009aa5 ___crtExitProcess

/* Library Function - Single Match
    ___crtExitProcess
   
   Library: Visual Studio 2005 Release */

void __cdecl ___crtExitProcess(int param_1)

{
  FUN_10009a7f(param_1);
                    /* WARNING: Subroutine does not return */
  ExitProcess(param_1);
}


// 10009aba FUN_10009aba

void FUN_10009aba(void)

{
  __lock(8);
  return;
}


// 10009ac3 FUN_10009ac3

void FUN_10009ac3(void)

{
  FUN_1000c5f2(8);
  return;
}


// 10009acc FUN_10009acc

void __cdecl FUN_10009acc(undefined4 *param_1)

{
  undefined4 *in_EAX;
  
  for (; in_EAX < param_1; in_EAX = in_EAX + 1) {
    if ((code *)*in_EAX != (code *)0x0) {
      (*(code *)*in_EAX)();
    }
  }
  return;
}


// 10009ae4 __initterm_e

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


// 10009b04 __get_osplatform

/* Library Function - Single Match
    __get_osplatform
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __get_osplatform(int *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (int *)0x0) && (DAT_1001c484 != 0)) {
    *param_1 = DAT_1001c484;
    return 0;
  }
  pDVar1 = FUN_1000ac50();
  *pDVar1 = 0x16;
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 10009b3b FUN_10009b3b

undefined4 __cdecl FUN_10009b3b(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (undefined4 *)0x0) && (DAT_1001c484 != 0)) {
    *param_1 = DAT_1001c490;
    return 0;
  }
  pDVar1 = FUN_1000ac50();
  *pDVar1 = 0x16;
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 10009b77 __cinit

/* Library Function - Single Match
    __cinit
   
   Library: Visual Studio 2005 Release */

int __cdecl __cinit(int param_1)

{
  BOOL BVar1;
  int iVar2;
  undefined4 *puVar3;
  
  BVar1 = __IsNonwritableInCurrentImage((PBYTE)&PTR___fpmath_10013010);
  if (BVar1 != 0) {
    __fpmath(param_1);
  }
  __initp_misc_cfltcvt_tab();
  iVar2 = __initterm_e((undefined4 *)&DAT_100121ec,(undefined4 *)&DAT_10012204);
  if (iVar2 == 0) {
    _atexit((_func_4879 *)&LAB_1000b3a5);
    puVar3 = &DAT_100121c0;
    do {
      if ((code *)*puVar3 != (code *)0x0) {
        (*(code *)*puVar3)();
      }
      puVar3 = puVar3 + 1;
    } while (puVar3 < &DAT_100121e8);
    if ((DAT_1001dd34 != (code *)0x0) &&
       (BVar1 = __IsNonwritableInCurrentImage((PBYTE)&DAT_1001dd34), BVar1 != 0)) {
      (*DAT_1001dd34)(0,2,0);
    }
    iVar2 = 0;
  }
  return iVar2;
}


// 10009c09 FUN_10009c09

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x10009cdc) */

void __cdecl FUN_10009c09(int param_1,int param_2,int param_3)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  int iVar4;
  code *pcVar5;
  
  __lock(8);
  if (DAT_1001c4c4 != 1) {
    DAT_1001c4c0 = 1;
    DAT_1001c4bc = (undefined1)param_3;
    if (param_2 == 0) {
      piVar2 = (int *)FUN_100093a5(DAT_1001dd2c);
      piVar3 = (int *)FUN_100093a5(DAT_1001dd28);
      if (piVar2 != (int *)0x0) {
        while (piVar3 = piVar3 + -1, piVar2 <= piVar3) {
          if (*piVar3 != 0) {
            iVar1 = *piVar3;
            iVar4 = __encoded_null();
            if (iVar1 != iVar4) {
              pcVar5 = (code *)FUN_100093a5(iVar1);
              (*pcVar5)();
            }
          }
        }
      }
      FUN_10009acc((undefined4 *)&DAT_10012214);
    }
    FUN_10009acc((undefined4 *)&DAT_1001221c);
  }
  FUN_10009cd6();
  if (param_3 != 0) {
    return;
  }
  DAT_1001c4c4 = 1;
  FUN_1000c5f2(8);
  ___crtExitProcess(param_1);
  return;
}


// 10009cd6 FUN_10009cd6

void FUN_10009cd6(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + 0x10) != 0) {
    FUN_1000c5f2(8);
  }
  return;
}


// 10009ceb __exit

/* Library Function - Single Match
    __exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __exit(int _Code)

{
  FUN_10009c09(_Code,1,0);
  return;
}


// 10009cfc __cexit

/* Library Function - Single Match
    __cexit
   
   Library: Visual Studio 2005 Release */

void __cdecl __cexit(void)

{
  FUN_10009c09(0,0,1);
  return;
}


// 10009d0b __init_pointers

/* Library Function - Single Match
    __init_pointers
   
   Library: Visual Studio 2005 Release */

void __cdecl __init_pointers(void)

{
  undefined4 uVar1;
  
  uVar1 = __encoded_null();
  FUN_1000dc7e(uVar1);
  FUN_1000db9f(uVar1);
  FUN_1000aaeb(uVar1);
  FUN_100098e7(uVar1);
  FUN_1000db95(uVar1);
  __initp_misc_winsig(uVar1);
  FUN_1000b3c9();
  __initp_eh_hooks();
  PTR___exit_10016fe8 = (undefined *)FUN_1000932e(0x10009ceb);
  return;
}


// 10009d58 __SEH_prolog4

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
  *(uint *)((int)auStack_1c + iVar1 + 4) = DAT_10017840 ^ (uint)&param_2;
  *(undefined4 *)((int)auStack_1c + iVar1) = unaff_retaddr;
  *unaff_FS_OFFSET = (int)local_8;
  return;
}


// 10009d9d __SEH_epilog4

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


// 10009dc0 __except_handler4

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
  
  piVar8 = (int *)(*(uint *)((int)param_2 + 8) ^ DAT_10017840);
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
          goto LAB_10009e6b;
        }
        if (0 < iVar4) {
          if (((*param_1 == -0x1f928c9d) && (DAT_1001dd20 != (code *)0x0)) &&
             (BVar5 = __IsNonwritableInCurrentImage((PBYTE)&DAT_1001dd20), BVar5 != 0)) {
            (*DAT_1001dd20)(param_1,1);
          }
          _EH4_GlobalUnwind(param_2);
          if (*(uint *)((int)param_2 + 0xc) != uVar7) {
            _EH4_LocalUnwind((int)param_2,uVar7,iVar1,&DAT_10017840);
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
    _EH4_LocalUnwind((int)param_2,0xfffffffe,iVar1,&DAT_10017840);
  }
LAB_10009e6b:
  if (*piVar8 != -2) {
    __security_check_cookie(piVar8[1] + iVar1 ^ *(uint *)(*piVar8 + iVar1));
  }
  __security_check_cookie(piVar8[3] + iVar1 ^ *(uint *)(piVar8[2] + iVar1));
  return local_c;
}


// 10009f56 __flsbuf

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
    pDVar2 = FUN_1000ac50();
    *pDVar2 = 9;
LAB_10009f7a:
    _File_00->_flag = _File_00->_flag | 0x20;
    return -1;
  }
  if ((uVar6 & 0x40) != 0) {
    pDVar2 = FUN_1000ac50();
    *pDVar2 = 0x22;
    goto LAB_10009f7a;
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
     (((ppuVar3 = FUN_1000e795(), _File_00 != (FILE *)(ppuVar3 + 8) &&
       (ppuVar3 = FUN_1000e795(), _File_00 != (FILE *)(ppuVar3 + 0x10))) ||
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
        puVar5 = &DAT_10017168;
      }
      else {
        puVar5 = (undefined *)(((uint)_File & 0x1f) * 0x38 + (&DAT_1001dc20)[(int)_File >> 5]);
      }
      if (((puVar5[4] & 0x20) != 0) &&
         (lVar7 = __lseeki64((int)_File,0x200000000,unaff_EDI), lVar7 == -1)) goto LAB_1000a0a0;
    }
    else {
      local_8 = __write((int)_File,_Buf,uVar6);
    }
    *_File_00->_base = (char)_Ch;
  }
  if (local_8 == uVar6) {
    return _Ch & 0xff;
  }
LAB_1000a0a0:
  _File_00->_flag = _File_00->_flag | 0x20;
  return -1;
}


// 1000a0b6 write_char

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


// 1000a0e9 write_multi_char

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


// 1000a10d write_string

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
        pDVar1 = FUN_1000ac50();
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


// 1000a157 FUN_1000a157

/* WARNING: Type propagation algorithm not settling */

void __cdecl FUN_1000a157(FILE *param_1,byte *param_2,localeinfo_struct *param_3,wchar_t *param_4)

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
  
  local_8 = DAT_10017840 ^ (uint)local_1fc;
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
        puVar7 = &DAT_10017168;
      }
      else {
        iVar5 = __fileno(local_22c);
        uVar6 = __fileno(local_22c);
        puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_1001dc20)[iVar5 >> 5]);
      }
      if ((puVar7[0x24] & 0x7f) == 0) {
        iVar5 = __fileno(local_22c);
        if ((iVar5 == -1) || (iVar5 = __fileno(local_22c), iVar5 == -2)) {
          puVar7 = &DAT_10017168;
        }
        else {
          iVar5 = __fileno(local_22c);
          uVar6 = __fileno(local_22c);
          puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_1001dc20)[iVar5 >> 5]);
        }
        if ((puVar7[0x24] & 0x80) == 0) goto LAB_1000a295;
      }
    }
    else {
LAB_1000a295:
      if (param_2 != (byte *)0x0) {
        local_215 = *param_2;
        local_230 = 0;
        local_224 = 0;
        local_250 = (wchar_t *)0x0;
        iVar5 = 0;
        while ((local_215 != 0 && (pbVar12 = param_2 + 1, local_244 = pbVar12, -1 < local_230))) {
          if ((byte)(local_215 - 0x20) < 0x59) {
            uVar6 = (byte)"mscoree.dll"[(char)local_215 + 8] & 0xf;
          }
          else {
            uVar6 = 0;
          }
          local_270 = (int)(char)(&DAT_100136d0)[uVar6 * 8 + iVar5] >> 4;
          switch(local_270) {
          case 0:
switchD_1000a2f9_caseD_0:
            local_240 = 0;
            iVar5 = __isleadbyte_l((uint)local_215,&local_260);
            if (iVar5 != 0) {
              write_char();
              local_244 = param_2 + 2;
              if (*pbVar12 == 0) {
LAB_1000aa8c:
                pDVar4 = FUN_1000ac50();
                *pDVar4 = 0x16;
                goto LAB_1000a1c7;
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
                goto switchD_1000a2f9_caseD_0;
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
LAB_1000a6d8:
                local_214 = local_214 | 0x40;
LAB_1000a6dc:
                local_224 = 10;
LAB_1000a6e3:
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
                  goto LAB_1000a561;
                }
                if (local_215 == 0x41) {
LAB_1000a50d:
                  local_215 = local_215 + 0x20;
                  local_274 = 1;
LAB_1000a51a:
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
                      local_250 = (wchar_t *)FUN_10003a10(sVar13);
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
                  pcVar9 = (code *)FUN_100093a5((int)PTR_LAB_10016f08);
                  (*pcVar9)(puVar17,pwVar18,sVar13,iVar8,iVar5,uVar19,plVar20);
                  uVar6 = local_214 & 0x80;
                  if ((uVar6 != 0) && (local_21c == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_100093a5((int)PTR_LAB_10016f14);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((local_215 == 0x67) && (uVar6 == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_100093a5((int)PTR_LAB_10016f10);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((char)*pwVar14 == '-') {
                    local_214 = local_214 | 0x100;
                    local_220 = (wchar_t *)((int)pwVar14 + 1);
                    pwVar14 = local_220;
                  }
LAB_1000a64e:
                  local_224 = _strlen((char *)pwVar14);
                }
                else if (local_215 == 0x43) {
                  if ((local_214 & 0x830) == 0) {
                    local_214 = local_214 | 0x800;
                  }
LAB_1000a5be:
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
                else if ((local_215 == 0x45) || (local_215 == 0x47)) goto LAB_1000a50d;
              }
              else {
                if (local_215 == 0x58) goto LAB_1000a7e3;
                if (local_215 == 0x5a) {
                  psVar2 = *(short **)param_4;
                  local_228 = param_4 + 2;
                  pwVar14 = (wchar_t *)PTR_s__null__10016fec;
                  local_220 = (wchar_t *)PTR_s__null__10016fec;
                  if ((psVar2 == (short *)0x0) ||
                     (pwVar18 = *(wchar_t **)(psVar2 + 2), pwVar18 == (wchar_t *)0x0))
                  goto LAB_1000a64e;
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
                  if (local_215 == 0x61) goto LAB_1000a51a;
                  if (local_215 == 99) goto LAB_1000a5be;
                }
              }
LAB_1000a95b:
              if (local_24c == 0) {
                if ((local_214 & 0x40) != 0) {
                  if ((local_214 & 0x100) == 0) {
                    if ((local_214 & 1) == 0) {
                      if ((local_214 & 2) == 0) goto LAB_1000a991;
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
LAB_1000a991:
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
LAB_1000a561:
                  iVar5 = local_21c;
                  if (local_21c == -1) {
                    iVar5 = 0x7fffffff;
                  }
                  local_228 = param_4 + 2;
                  local_220 = *(wchar_t **)param_4;
                  if ((local_214 & 0x810) == 0) {
                    pwVar14 = local_220;
                    if (local_220 == (wchar_t *)0x0) {
                      pwVar14 = (wchar_t *)PTR_s__null__10016fec;
                      local_220 = (wchar_t *)PTR_s__null__10016fec;
                    }
                    for (; (iVar5 != 0 && (iVar5 = iVar5 + -1, (char)*pwVar14 != '\0'));
                        pwVar14 = (wchar_t *)((int)pwVar14 + 1)) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220;
                  }
                  else {
                    if (local_220 == (wchar_t *)0x0) {
                      local_220 = (wchar_t *)PTR_u__null__10016ff0;
                    }
                    local_240 = 1;
                    for (pwVar14 = local_220;
                        (iVar5 != 0 && (iVar5 = iVar5 + -1, *pwVar14 != L'\0'));
                        pwVar14 = pwVar14 + 1) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220 >> 1;
                  }
                  goto LAB_1000a95b;
                }
                if (local_215 == 0x75) goto LAB_1000a6dc;
                if (local_215 != 0x78) goto LAB_1000a95b;
                local_248 = 0x27;
LAB_1000a809:
                local_224 = 0x10;
                if ((local_214 & 0x80) != 0) {
                  local_233 = (char)local_248 + 'Q';
                  local_234 = 0x30;
                  local_238 = 2;
                }
                goto LAB_1000a6e3;
              }
              if (local_215 == 0x70) {
                local_21c = 8;
LAB_1000a7e3:
                local_248 = 7;
                goto LAB_1000a809;
              }
              if ((char)local_215 < 'e') goto LAB_1000a95b;
              if ((char)local_215 < 'h') goto LAB_1000a51a;
              if (local_215 == 0x69) goto LAB_1000a6d8;
              if (local_215 != 0x6e) {
                if (local_215 != 0x6f) goto LAB_1000a95b;
                local_224 = 8;
                if ((local_214 & 0x80) != 0) {
                  local_214 = local_214 | 0x200;
                }
                goto LAB_1000a6e3;
              }
              piVar3 = *(int **)param_4;
              local_228 = param_4 + 2;
              bVar15 = FUN_1000e963();
              if (CONCAT31(extraout_var,bVar15) == 0) goto LAB_1000aa8c;
              if ((local_214 & 0x20) == 0) {
                *piVar3 = local_230;
              }
              else {
                *(undefined2 *)piVar3 = (undefined2)local_230;
              }
              local_24c = 1;
            }
            if (local_250 != (wchar_t *)0x0) {
              FUN_10003a70(local_250);
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
        goto LAB_1000aab3;
      }
    }
  }
  pDVar4 = FUN_1000ac50();
  *pDVar4 = 0x16;
LAB_1000a1c7:
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_254 != '\0') {
    *(uint *)(local_258 + 0x70) = *(uint *)(local_258 + 0x70) & 0xfffffffd;
  }
LAB_1000aab3:
  __security_check_cookie(local_8 ^ (uint)local_1fc);
  return;
}


// 1000aaeb FUN_1000aaeb

void __cdecl FUN_1000aaeb(undefined4 param_1)

{
  DAT_1001c4c8 = param_1;
  return;
}


// 1000aaf5 __invoke_watson

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
  
  iVar1 = DAT_10017840;
  local_2d4 = 0x10001;
  _memset(&local_32c,0,0x50);
  local_2dc.ExceptionRecord = &local_32c;
  local_2dc.ContextRecord = (PCONTEXT)&local_2d4;
  local_32c.ExceptionCode = 0xc000000d;
  BVar2 = IsDebuggerPresent();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  LVar3 = UnhandledExceptionFilter(&local_2dc);
  if ((LVar3 == 0) && (BVar2 == 0)) {
    FUN_1000eaf3();
  }
  uExitCode = 0xc000000d;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  __security_check_cookie(iVar1);
  return;
}


// 1000abf1 FUN_1000abf1

void __cdecl
FUN_1000abf1(wchar_t *param_1,wchar_t *param_2,wchar_t *param_3,uint param_4,uintptr_t param_5)

{
  code *UNRECOVERED_JUMPTABLE;
  
  UNRECOVERED_JUMPTABLE = (code *)FUN_100093a5(DAT_1001c4c8);
  if (UNRECOVERED_JUMPTABLE != (code *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x1000ac05. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (*UNRECOVERED_JUMPTABLE)();
    return;
  }
  FUN_1000eaf3();
                    /* WARNING: Subroutine does not return */
  __invoke_watson(param_1,param_2,param_3,param_4,param_5);
}


// 1000ac15 FUN_1000ac15

int __cdecl FUN_1000ac15(int param_1)

{
  uint uVar1;
  
  uVar1 = 0;
  do {
    if (param_1 == (&DAT_10016ff8)[uVar1 * 2]) {
      return *(int *)(uVar1 * 8 + 0x10016ffc);
    }
    uVar1 = uVar1 + 1;
  } while (uVar1 < 0x2d);
  if (param_1 - 0x13U < 0x12) {
    return 0xd;
  }
  return (-(uint)(0xe < param_1 - 0xbcU) & 0xe) + 8;
}


// 1000ac50 FUN_1000ac50

DWORD * FUN_1000ac50(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_1000954a();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10017160;
  }
  return pDVar1 + 2;
}


// 1000ac63 FUN_1000ac63

DWORD * FUN_1000ac63(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_1000954a();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10017164;
  }
  return pDVar1 + 3;
}


// 1000ac76 FUN_1000ac76

void __cdecl FUN_1000ac76(DWORD param_1)

{
  DWORD *pDVar1;
  DWORD DVar2;
  
  pDVar1 = FUN_1000ac63();
  *pDVar1 = param_1;
  DVar2 = FUN_1000ac15(param_1);
  pDVar1 = FUN_1000ac50();
  *pDVar1 = DVar2;
  return;
}


// 1000ac94 __ioinit

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
  
  uStack_c = 0x1000aca0;
  local_8 = 0;
  GetStartupInfoA(&local_68);
  local_8 = 0xfffffffe;
  puVar2 = (undefined4 *)FUN_10003bf0(0x20,0x38);
  if (puVar2 == (undefined4 *)0x0) {
LAB_1000aecb:
    iVar7 = -1;
  }
  else {
    DAT_1001dc04 = 0x20;
    DAT_1001dc20 = puVar2;
    for (; puVar2 < DAT_1001dc20 + 0x1c0; puVar2 = puVar2 + 0xe) {
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
      while ((UVar10 = UVar9, (int)DAT_1001dc04 < (int)UVar9 &&
             (puVar2 = (undefined4 *)FUN_10003bf0(0x20,0x38), UVar10 = DAT_1001dc04,
             puVar2 != (undefined4 *)0x0))) {
        (&DAT_1001dc20)[iVar7] = puVar2;
        DAT_1001dc04 = DAT_1001dc04 + 0x20;
        puVar1 = puVar2;
        for (; puVar2 < puVar1 + 0x1c0; puVar2 = puVar2 + 0xe) {
          *(undefined1 *)(puVar2 + 1) = 0;
          *puVar2 = 0xffffffff;
          *(undefined1 *)((int)puVar2 + 5) = 10;
          puVar2[2] = 0;
          *(byte *)(puVar2 + 9) = *(byte *)(puVar2 + 9) & 0x80;
          *(undefined1 *)((int)puVar2 + 0x25) = 10;
          *(undefined1 *)((int)puVar2 + 0x26) = 10;
          puVar1 = (&DAT_1001dc20)[iVar7];
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
                     ((local_24 & 0x1f) * 0x38 + (int)(&DAT_1001dc20)[(int)local_24 >> 5]);
            *puVar2 = *(undefined4 *)local_20;
            *(byte *)(puVar2 + 1) = (byte)*pUVar6;
            iVar7 = ___crtInitCritSecAndSpinCount(puVar2 + 3,4000);
            if (iVar7 == 0) goto LAB_1000aecb;
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
      piVar8 = DAT_1001dc20 + iVar7 * 0xe;
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
          if (iVar5 == 0) goto LAB_1000aecb;
          piVar8[2] = piVar8[2] + 1;
        }
      }
      else {
        *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 0x80;
      }
      iVar7 = iVar7 + 1;
    } while (iVar7 < 3);
    SetHandleCount(DAT_1001dc04);
    iVar7 = 0;
  }
  return iVar7;
}


// 1000aed4 __ioterm

/* Library Function - Single Match
    __ioterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __ioterm(void)

{
  uint uVar1;
  uint *puVar2;
  uint uVar3;
  
  puVar2 = &DAT_1001dc20;
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
      FUN_10003a70(*puVar2);
      *puVar2 = 0;
    }
    puVar2 = puVar2 + 1;
  } while ((int)puVar2 < 0x1001dd20);
  return;
}


// 1000af20 __setenvp

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
  
  if (DAT_1001dd30 == 0) {
    ___initmbctable();
  }
  iVar5 = 0;
  pcVar4 = DAT_1001c134;
  if (DAT_1001c134 != (char *)0x0) {
    for (; *pcVar4 != '\0'; pcVar4 = pcVar4 + sVar2 + 1) {
      if (*pcVar4 != '=') {
        iVar5 = iVar5 + 1;
      }
      sVar2 = _strlen(pcVar4);
    }
    puVar1 = (undefined4 *)FUN_10003bf0(iVar5 + 1,4);
    pcVar4 = DAT_1001c134;
    DAT_1001c4a4 = puVar1;
    if (puVar1 != (undefined4 *)0x0) {
      do {
        if (*pcVar4 == '\0') {
          FUN_10003a70(DAT_1001c134);
          DAT_1001c134 = (char *)0x0;
          *puVar1 = 0;
          _DAT_1001dd24 = 1;
          return 0;
        }
        sVar2 = _strlen(pcVar4);
        _SizeInBytes = sVar2 + 1;
        if (*pcVar4 != '=') {
          _Dst = (char *)FUN_10003bf0(_SizeInBytes,1);
          *puVar1 = _Dst;
          if (_Dst == (char *)0x0) {
            FUN_10003a70(DAT_1001c4a4);
            DAT_1001c4a4 = (undefined4 *)0x0;
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


// 1000affb parse_cmdline

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
        goto LAB_1000b08d;
      }
    }
    in_EDX = pbVar7;
  } while ((bVar2) || ((bVar6 != 0x20 && (bVar6 != 9))));
  if (pbVar5 != (byte *)0x0) {
    pbVar5[-1] = 0;
  }
LAB_1000b08d:
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


// 1000b193 __setargv

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
  
  if (DAT_1001dd30 == 0) {
    ___initmbctable();
  }
  DAT_1001c5d4 = 0;
  GetModuleFileNameA((HMODULE)0x0,&DAT_1001c4d0,0x104);
  _DAT_1001c4b4 = &DAT_1001c4d0;
  if ((DAT_1001dd40 == (char *)0x0) || (local_8 = DAT_1001dd40, *DAT_1001dd40 == '\0')) {
    local_8 = &DAT_1001c4d0;
  }
  parse_cmdline((undefined4 *)0x0,(byte *)0x0,(int *)&local_c);
  uVar2 = local_c;
  if ((local_c < 0x3fffffff) && (local_10 != 0xffffffff)) {
    uVar1 = local_c * 4 + local_10;
    if ((local_10 <= uVar1) &&
       (puVar3 = (undefined4 *)FUN_10003a10(uVar1), puVar3 != (undefined4 *)0x0)) {
      parse_cmdline(puVar3,(byte *)(puVar3 + uVar2),(int *)&local_c);
      _DAT_1001c498 = local_c - 1;
      _DAT_1001c49c = puVar3;
      return 0;
    }
  }
  return -1;
}


// 1000b24c ___crtGetEnvironmentStringsA

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
  if (DAT_1001c5d8 == 0) {
    lpWideCharStr = GetEnvironmentStringsW();
    if (lpWideCharStr != (LPWCH)0x0) {
      DAT_1001c5d8 = 1;
      goto LAB_1000b29e;
    }
    DVar3 = GetLastError();
    if (DVar3 == 0x78) {
      DAT_1001c5d8 = 2;
    }
  }
  if (DAT_1001c5d8 != 1) {
    if ((DAT_1001c5d8 != 2) && (DAT_1001c5d8 != 0)) {
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
    _Dst = (void *)FUN_10003a10(pcVar7 + (1 - (int)_Src));
    if (_Dst == (void *)0x0) {
      FreeEnvironmentStringsA(_Src);
      return (LPVOID)0x0;
    }
    _memcpy(_Dst,_Src,(size_t)(pcVar7 + (1 - (int)_Src)));
    FreeEnvironmentStringsA(_Src);
    return _Dst;
  }
LAB_1000b29e:
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
      (lpMultiByteStr = (LPSTR)FUN_10003a10(cbMultiByte), lpMultiByteStr != (LPSTR)0x0)) &&
     (iVar6 = WideCharToMultiByte(0,0,lpWideCharStr,iVar6,lpMultiByteStr,cbMultiByte,(LPCSTR)0x0,
                                  (LPBOOL)0x0), local_8 = lpMultiByteStr, iVar6 == 0)) {
    FUN_10003a70(lpMultiByteStr);
    local_8 = (LPSTR)0x0;
  }
  FreeEnvironmentStringsW(lpWideCharStr);
  return local_8;
}


// 1000b381 __RTC_Initialize

/* WARNING: Removing unreachable block (ram,0x1000b393) */
/* WARNING: Removing unreachable block (ram,0x1000b399) */
/* WARNING: Removing unreachable block (ram,0x1000b39b) */
/* Library Function - Single Match
    __RTC_Initialize
   
   Library: Visual Studio 2005 Release */

void __RTC_Initialize(void)

{
  return;
}


// 1000b3c9 FUN_1000b3c9

void FUN_1000b3c9(void)

{
  return;
}


// 1000b3ca FUN_1000b3ca

DWORD * __cdecl FUN_1000b3ca(int param_1,DWORD param_2)

{
  int *piVar1;
  code *pcVar2;
  DWORD DVar3;
  DWORD DVar4;
  DWORD *pDVar5;
  int *piVar6;
  int iVar7;
  int iVar8;
  
  pDVar5 = FUN_1000954a();
  if (pDVar5 != (DWORD *)0x0) {
    piVar1 = (int *)pDVar5[0x17];
    piVar6 = piVar1;
    do {
      if (*piVar6 == param_1) break;
      piVar6 = piVar6 + 3;
    } while (piVar6 < piVar1 + DAT_10017224 * 3);
    if ((piVar1 + DAT_10017224 * 3 <= piVar6) || (*piVar6 != param_1)) {
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
          if (DAT_10017218 < DAT_1001721c + DAT_10017218) {
            iVar7 = DAT_10017218 * 0xc;
            iVar8 = DAT_10017218;
            do {
              *(undefined4 *)(iVar7 + 8 + pDVar5[0x17]) = 0;
              iVar8 = iVar8 + 1;
              iVar7 = iVar7 + 0xc;
            } while (iVar8 < DAT_1001721c + DAT_10017218);
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


// 1000b543 FUN_1000b543

undefined4 FUN_1000b543(void)

{
  return 1;
}


// 1000b549 ___security_init_cookie

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
  if ((DAT_10017840 == 0xbb40e64e) || ((DAT_10017840 & 0xffff0000) == 0)) {
    GetSystemTimeAsFileTime(&local_c);
    uVar4 = local_c.dwHighDateTime ^ local_c.dwLowDateTime;
    DVar1 = GetCurrentProcessId();
    DVar2 = GetCurrentThreadId();
    DVar3 = GetTickCount();
    QueryPerformanceCounter(&local_14);
    DAT_10017840 = uVar4 ^ DVar1 ^ DVar2 ^ DVar3 ^ local_14.s.HighPart ^ local_14.s.LowPart;
    if (DAT_10017840 == 0xbb40e64e) {
      DAT_10017840 = 0xbb40e64f;
    }
    else if ((DAT_10017840 & 0xffff0000) == 0) {
      DAT_10017840 = DAT_10017840 | DAT_10017840 << 0x10;
    }
    DAT_10017844 = ~DAT_10017840;
  }
  else {
    DAT_10017844 = ~DAT_10017840;
  }
  return;
}


// 1000b5dd FUN_1000b5dd

undefined4 FUN_1000b5dd(void)

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


// 1000b60c setSBCS

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
    *puVar1 = puVar1[(int)&DAT_10017228 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  puVar1 = (undefined1 *)(in_EAX + 0x11d);
  iVar2 = 0x100;
  do {
    *puVar1 = puVar1[(int)&DAT_10017228 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  return;
}


// 1000b661 setSBUpLow

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
  
  local_8 = DAT_10017840 ^ (uint)local_4a0;
  BVar3 = GetCPInfo(*(UINT *)(unaff_ESI + 4),&local_51c);
  if (BVar3 == 0) {
    uVar4 = 0;
    do {
      pcVar2 = (char *)(unaff_ESI + 0x11d + uVar4);
      if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) + 0x20 < (char *)0x1a) {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        cVar6 = (char)uVar4 + ' ';
LAB_1000b7c8:
        *pcVar2 = cVar6;
      }
      else {
        if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) < (char *)0x1a) {
          pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
          *pbVar1 = *pbVar1 | 0x20;
          cVar6 = (char)uVar4 + -0x20;
          goto LAB_1000b7c8;
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
          goto LAB_1000b76f;
        }
        *(undefined1 *)(unaff_ESI + 0x11d + uVar4) = 0;
      }
      else {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        CVar5 = local_208[uVar4];
LAB_1000b76f:
        *(CHAR *)(unaff_ESI + 0x11d + uVar4) = CVar5;
      }
      uVar4 = uVar4 + 1;
    } while (uVar4 < 0x100);
  }
  __security_check_cookie(local_8 ^ (uint)local_4a0);
  return;
}


// 1000b7eb ___updatetmbcinfo

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
  if (((p_Var1->_ownlocale & DAT_1001774c) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xd);
    lpAddend = p_Var1->ptmbcinfo;
    if (lpAddend != (pthreadmbcinfo)PTR_DAT_10017650) {
      if (lpAddend != (pthreadmbcinfo)0x0) {
        LVar2 = InterlockedDecrement(&lpAddend->refcount);
        if ((LVar2 == 0) && (lpAddend != (pthreadmbcinfo)&DAT_10017228)) {
          FUN_10003a70(lpAddend);
        }
      }
      p_Var1->ptmbcinfo = (pthreadmbcinfo)PTR_DAT_10017650;
      lpAddend = (pthreadmbcinfo)PTR_DAT_10017650;
      InterlockedIncrement((LONG *)PTR_DAT_10017650);
    }
    FUN_1000b886();
  }
  else {
    lpAddend = p_Var1->ptmbcinfo;
  }
  if (lpAddend == (pthreadmbcinfo)0x0) {
    __amsg_exit(0x20);
  }
  return lpAddend;
}


// 1000b886 FUN_1000b886

void FUN_1000b886(void)

{
  FUN_1000c5f2(0xd);
  return;
}


// 1000b88f getSystemCP

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
  DAT_1001c5dc = 0;
  if (unaff_ESI == -2) {
    DAT_1001c5dc = 1;
    UVar1 = GetOEMCP();
  }
  else if (unaff_ESI == -3) {
    DAT_1001c5dc = 1;
    UVar1 = GetACP();
  }
  else {
    if (unaff_ESI != -4) {
      if (local_8 == '\0') {
        DAT_1001c5dc = 0;
        return unaff_ESI;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return unaff_ESI;
    }
    UVar1 = *(UINT *)(local_14[0] + 4);
    DAT_1001c5dc = 1;
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return UVar1;
}


// 1000b909 FUN_1000b909

void __cdecl FUN_1000b909(undefined4 param_1,int param_2)

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  uVar4 = getSystemCP((int)unaff_EDI);
  if (uVar4 != 0) {
    local_20 = (byte *)0x0;
    uVar5 = 0;
LAB_1000b945:
    if (*(uint *)((int)&DAT_10017658 + uVar5) != uVar4) goto code_r0x1000b951;
    _memset((void *)(param_2 + 0x1c),0,0x101);
    local_24 = 0;
    pbVar8 = &DAT_10017668 + (int)local_20 * 0x30;
    local_20 = pbVar8;
    do {
      for (; (*pbVar8 != 0 && (bVar3 = pbVar8[1], bVar3 != 0)); pbVar8 = pbVar8 + 2) {
        for (uVar5 = (uint)*pbVar8; uVar5 <= bVar3; uVar5 = uVar5 + 1) {
          pbVar2 = (byte *)(param_2 + 0x1d + uVar5);
          *pbVar2 = *pbVar2 | *(byte *)(local_24 + 0x10017654);
          bVar3 = pbVar8[1];
        }
      }
      local_24 = local_24 + 1;
      pbVar8 = local_20 + 8;
      local_20 = pbVar8;
    } while (local_24 < 4);
    *(uint *)(param_2 + 4) = uVar4;
    *(undefined4 *)(param_2 + 8) = 1;
    uVar9 = FUN_1000b5dd();
    *(undefined4 *)(param_2 + 0xc) = uVar9;
    puVar7 = (undefined2 *)(param_2 + 0x10);
    puVar10 = (undefined2 *)(&DAT_1001765c + extraout_ECX);
    iVar11 = 6;
    do {
      *puVar7 = *puVar10;
      puVar10 = puVar10 + 1;
      puVar7 = puVar7 + 1;
      iVar11 = iVar11 + -1;
    } while (iVar11 != 0);
    goto LAB_1000ba76;
  }
LAB_1000b932:
  setSBCS(unaff_EDI);
LAB_1000bad3:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
code_r0x1000b951:
  local_20 = (byte *)((int)local_20 + 1);
  uVar5 = uVar5 + 0x30;
  if (0xef < uVar5) goto code_r0x1000b95e;
  goto LAB_1000b945;
code_r0x1000b95e:
  if (((uVar4 == 65000) || (uVar4 == 0xfde9)) ||
     (BVar6 = IsValidCodePage(uVar4 & 0xffff), BVar6 == 0)) goto LAB_1000bad3;
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
      uVar9 = FUN_1000b5dd();
      *(undefined4 *)(param_2 + 0xc) = uVar9;
      *(undefined4 *)(param_2 + 8) = extraout_EDX;
    }
    *(undefined4 *)(param_2 + 0x10) = 0;
    *(undefined4 *)(param_2 + 0x14) = 0;
    *(undefined4 *)(param_2 + 0x18) = 0;
LAB_1000ba76:
    setSBUpLow(unaff_EDI);
    goto LAB_1000bad3;
  }
  if (DAT_1001c5dc == 0) goto LAB_1000bad3;
  goto LAB_1000b932;
}


// 1000bae2 __setmbcp

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
    ptVar3 = (pthreadmbcinfo)FUN_10003a10(0x220);
    if (ptVar3 != (pthreadmbcinfo)0x0) {
      ptVar7 = p_Var1->ptmbcinfo;
      ptVar8 = ptVar3;
      for (iVar6 = 0x88; iVar6 != 0; iVar6 = iVar6 + -1) {
        ptVar8->refcount = ptVar7->refcount;
        ptVar7 = (pthreadmbcinfo)&ptVar7->mbcodepage;
        ptVar8 = (pthreadmbcinfo)&ptVar8->mbcodepage;
      }
      ptVar3->refcount = 0;
      local_24 = FUN_1000b909(iVar2,(int)ptVar3);
      if (local_24 == 0) {
        LVar4 = InterlockedDecrement(&p_Var1->ptmbcinfo->refcount);
        if ((LVar4 == 0) && (p_Var1->ptmbcinfo != (pthreadmbcinfo)&DAT_10017228)) {
          FUN_10003a70(p_Var1->ptmbcinfo);
        }
        p_Var1->ptmbcinfo = ptVar3;
        InterlockedIncrement((LONG *)ptVar3);
        if (((p_Var1->_ownlocale & 2) == 0) && (((byte)DAT_1001774c & 1) == 0)) {
          __lock(0xd);
          _DAT_1001c5ec = ptVar3->mbcodepage;
          _DAT_1001c5f0 = ptVar3->ismbcodepage;
          _DAT_1001c5f4 = *(undefined4 *)ptVar3->mbulinfo;
          for (iVar2 = 0; iVar2 < 5; iVar2 = iVar2 + 1) {
            (&DAT_1001c5e0)[iVar2] = ptVar3->mbulinfo[iVar2 + 2];
          }
          for (iVar2 = 0; iVar2 < 0x101; iVar2 = iVar2 + 1) {
            (&DAT_10017448)[iVar2] = ptVar3->mbctype[iVar2 + 4];
          }
          for (iVar2 = 0; iVar2 < 0x100; iVar2 = iVar2 + 1) {
            (&DAT_10017550)[iVar2] = ptVar3->mbcasemap[iVar2 + 4];
          }
          LVar4 = InterlockedDecrement((LONG *)PTR_DAT_10017650);
          if ((LVar4 == 0) && (PTR_DAT_10017650 != &DAT_10017228)) {
            FUN_10003a70(PTR_DAT_10017650);
          }
          PTR_DAT_10017650 = (undefined *)ptVar3;
          InterlockedIncrement((LONG *)ptVar3);
          FUN_1000bc43();
        }
      }
      else if (local_24 == -1) {
        if (ptVar3 != (pthreadmbcinfo)&DAT_10017228) {
          FUN_10003a70(ptVar3);
        }
        pDVar5 = FUN_1000ac50();
        *pDVar5 = 0x16;
      }
    }
  }
  return local_24;
}


// 1000bc43 FUN_1000bc43

void FUN_1000bc43(void)

{
  FUN_1000c5f2(0xd);
  return;
}


// 1000bc7c ___initmbctable

/* Library Function - Single Match
    ___initmbctable
   
   Library: Visual Studio 2005 Release */

undefined4 ___initmbctable(void)

{
  if (DAT_1001dd30 == 0) {
    __setmbcp(-3);
    DAT_1001dd30 = 1;
  }
  return 0;
}


// 1000bc9a ___freetlocinfo

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
       (*(undefined ***)(param_1 + 0xbc) != &PTR_DAT_10017cd8)) &&
      (*(int **)(param_1 + 0xb0) != (int *)0x0)) && (**(int **)(param_1 + 0xb0) == 0)) {
    piVar4 = *(int **)(param_1 + 0xb8);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_10003a70(piVar4);
      ___free_lconv_mon(*(int *)(param_1 + 0xbc));
    }
    piVar4 = *(int **)(param_1 + 0xb4);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_10003a70(piVar4);
      ___free_lconv_num(*(int **)(param_1 + 0xbc));
    }
    FUN_10003a70(*(undefined4 *)(param_1 + 0xb0));
    FUN_10003a70(*(undefined4 *)(param_1 + 0xbc));
  }
  if ((*(int **)(param_1 + 0xc0) != (int *)0x0) && (**(int **)(param_1 + 0xc0) == 0)) {
    FUN_10003a70(*(int *)(param_1 + 0xc4) + -0xfe);
    FUN_10003a70(*(int *)(param_1 + 0xcc) + -0x80);
    FUN_10003a70(*(int *)(param_1 + 0xd0) + -0x80);
    FUN_10003a70(*(undefined4 *)(param_1 + 0xc0));
  }
  ppuVar1 = *(undefined ***)(param_1 + 0xd4);
  if ((ppuVar1 != &PTR_DAT_10017c18) && (ppuVar1[0x2d] == (undefined *)0x0)) {
    ___free_lc_time(ppuVar1);
    FUN_10003a70(*(undefined4 *)(param_1 + 0xd4));
  }
  piVar4 = (int *)(param_1 + 0x50);
  iVar3 = 6;
  do {
    if ((((undefined *)piVar4[-2] != &DAT_10017750) &&
        (piVar2 = (int *)*piVar4, piVar2 != (int *)0x0)) && (*piVar2 == 0)) {
      FUN_10003a70(piVar2);
    }
    if (((piVar4[-1] != 0) && (piVar2 = (int *)piVar4[1], piVar2 != (int *)0x0)) && (*piVar2 == 0))
    {
      FUN_10003a70(piVar2);
    }
    piVar4 = piVar4 + 4;
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  FUN_10003a70(param_1);
  return;
}


// 1000bdda ___addlocaleref

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
    if (((undefined *)pLVar1[-2] != &DAT_10017750) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 1000be60 ___removelocaleref

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
      if (((undefined *)pLVar1[-2] != &DAT_10017750) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 1000beec __updatetlocinfoEx_nolock

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
        if ((*pLVar1 == 0) && (pLVar1 != (LONG *)&DAT_10017758)) {
          ___freetlocinfo((int)pLVar1);
        }
      }
    }
    return unaff_EDI;
  }
  return (LONG *)0x0;
}


// 1000bf2a ___updatetlocinfo

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___updatetlocinfo
   
   Library: Visual Studio 2005 Release */

pthreadlocinfo __cdecl ___updatetlocinfo(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = __getptd();
  if (((p_Var1->_ownlocale & DAT_1001774c) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xc);
    __updatetlocinfoEx_nolock();
    FUN_1000bf94();
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


// 1000bf94 FUN_1000bf94

void FUN_1000bf94(void)

{
  FUN_1000c5f2(0xc);
  return;
}


// 1000bfa0 __security_check_cookie

/* Library Function - Single Match
    @__security_check_cookie@4
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release
   __fastcall __security_check_cookie,4 */

void __fastcall __security_check_cookie(int param_1)

{
  if (param_1 == DAT_10017840) {
    return;
  }
                    /* WARNING: Subroutine does not return */
  ___report_gsfailure();
}


// 1000bfaf __crtLCMapStringA_stat

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  if (DAT_1001c61c == 0) {
    iVar3 = LCMapStringW(0,0x100,L"",1,(LPWSTR)0x0,0);
    if (iVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_1001c61c = 2;
      }
    }
    else {
      DAT_1001c61c = 1;
    }
  }
  pcVar5 = (char *)param_3;
  pcVar8 = param_4;
  if (0 < (int)param_4) {
    do {
      pcVar8 = pcVar8 + -1;
      if (*pcVar5 == '\0') goto LAB_1000c01e;
      pcVar5 = pcVar5 + 1;
    } while (pcVar8 != (char *)0x0);
    pcVar8 = (char *)0xffffffff;
LAB_1000c01e:
    pcVar5 = param_4 + -(int)pcVar8;
    bVar2 = (int)(pcVar5 + -1) < (int)param_4;
    param_4 = pcVar5 + -1;
    if (bVar2) {
      param_4 = pcVar5;
    }
  }
  if ((DAT_1001c61c == 2) || (DAT_1001c61c == 0)) {
    local_10 = (LPCWSTR)0x0;
    local_14 = 0;
    if (param_1 == (localeinfo_struct *)0x0) {
      param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
    }
    if (param_7 == 0) {
      param_7 = *(int *)(*in_ECX + 4);
    }
    UVar7 = ___ansicp((LCID)param_1);
    if (UVar7 == 0xffffffff) goto LAB_1000c33f;
    if (UVar7 == param_7) {
      LCMapStringA((LCID)param_1,param_2,(LPCSTR)param_3,(int)param_4,(LPSTR)param_5,(int)param_6);
    }
    else {
      local_10 = (LPCWSTR)___convertcp(param_7,UVar7,(char *)param_3,(uint *)&param_4,(LPSTR)0x0,0);
      if (local_10 == (LPCWSTR)0x0) goto LAB_1000c33f;
      local_c = LCMapStringA((LCID)param_1,param_2,(LPCSTR)local_10,(int)param_4,(LPSTR)0x0,0);
      if (local_c != 0) {
        if (((int)local_c < 1) || (0xffffffe0 < local_c)) {
          pcVar5 = (char *)0x0;
        }
        else if (local_c + 8 < 0x401) {
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_1000c31c;
          pcVar5 = &stack0xffffffe4;
        }
        else {
          pcVar5 = (char *)FUN_10003a10(local_c + 8);
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
LAB_1000c31c:
    if (local_10 != (LPCWSTR)0x0) {
      FUN_10003a70(local_10);
    }
    if ((local_14 != 0) && (param_5 != local_14)) {
      FUN_10003a70(local_14);
    }
    goto LAB_1000c33f;
  }
  if (DAT_1001c61c != 1) goto LAB_1000c33f;
  local_c = 0;
  if (param_7 == 0) {
    param_7 = *(int *)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar(param_7,(uint)(param_8 != 0) * 8 + 1,(LPCSTR)param_3,
                                    (int)param_4,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_1000c33f;
  if (((int)cchWideChar < 1) || (0xffffffe0 / cchWideChar < 2)) {
    local_10 = (LPCWSTR)0x0;
  }
  else {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar6 = (LPCWSTR)&stack0xffffffdc;
      local_10 = (LPCWSTR)&stack0xffffffdc;
      if (&stack0x00000000 != (undefined1 *)0x24) {
LAB_1000c0c6:
        local_10 = pWVar6 + 4;
      }
    }
    else {
      pWVar6 = (LPCWSTR)FUN_10003a10(uVar1);
      local_10 = pWVar6;
      if (pWVar6 != (LPCWSTR)0x0) {
        pWVar6[0] = L'\xdddd';
        pWVar6[1] = L'\0';
        goto LAB_1000c0c6;
      }
    }
  }
  if (local_10 == (LPCWSTR)0x0) goto LAB_1000c33f;
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
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_1000c1d5;
          pWVar6 = (LPCWSTR)&stack0xffffffe4;
        }
        else {
          pWVar6 = (LPCWSTR)FUN_10003a10(uVar1);
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
LAB_1000c1d5:
  __freea(local_10);
LAB_1000c33f:
  iVar3 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar3;
}


// 1000c351 ___crtLCMapStringA

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


// 1000c394 __isleadbyte_l

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


// 1000c3ca _isleadbyte

/* Library Function - Single Match
    _isleadbyte
   
   Library: Visual Studio 2005 Release */

int __cdecl _isleadbyte(int _C)

{
  int iVar1;
  
  iVar1 = __isleadbyte_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 1000c3d8 __isctype_l

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


// 1000c48e __getdrive

/* Library Function - Single Match
    __getdrive
   
   Library: Visual Studio 2005 Release */

int __cdecl __getdrive(void)

{
  bool bVar1;
  DWORD DVar2;
  byte *lpBuffer;
  DWORD *pDVar3;
  int iVar4;
  byte local_110 [120];
  undefined1 local_98 [144];
  uint local_8;
  
  local_8 = DAT_10017840 ^ (uint)local_98;
  bVar1 = false;
  lpBuffer = local_110;
  DVar2 = GetCurrentDirectoryA(0x105,(LPSTR)lpBuffer);
  if (0x104 < (int)DVar2) {
    lpBuffer = (byte *)FUN_10003bf0(DVar2 + 1,1);
    if (lpBuffer == (byte *)0x0) {
      pDVar3 = FUN_1000ac50();
      *pDVar3 = 0xc;
      DVar2 = 0;
    }
    else {
      bVar1 = true;
      if (DVar2 != 0) {
        DVar2 = GetCurrentDirectoryA(DVar2 + 1,(LPSTR)lpBuffer);
      }
    }
  }
  if (DVar2 == 0) {
    pDVar3 = FUN_1000ac50();
    *pDVar3 = 0xc;
  }
  else if (lpBuffer[1] == 0x3a) {
    _toupper((uint)*lpBuffer);
  }
  if (bVar1) {
    FUN_10003a70(lpBuffer);
  }
  iVar4 = __security_check_cookie(local_8 ^ (uint)local_98);
  return iVar4;
}


// 1000c554 __mtinitlocks

/* Library Function - Single Match
    __mtinitlocks
   
   Library: Visual Studio 2005 Release */

int __cdecl __mtinitlocks(void)

{
  int iVar1;
  int iVar2;
  undefined *puVar3;
  
  iVar2 = 0;
  puVar3 = &DAT_1001c620;
  do {
    if ((&DAT_1001784c)[iVar2 * 2] == 1) {
      (&DAT_10017848)[iVar2 * 2] = puVar3;
      puVar3 = puVar3 + 0x18;
      iVar1 = ___crtInitCritSecAndSpinCount((&DAT_10017848)[iVar2 * 2],4000);
      if (iVar1 == 0) {
        (&DAT_10017848)[iVar2 * 2] = 0;
        return 0;
      }
    }
    iVar2 = iVar2 + 1;
  } while (iVar2 < 0x24);
  return 1;
}


// 1000c59d __mtdeletelocks

/* Library Function - Single Match
    __mtdeletelocks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __cdecl __mtdeletelocks(void)

{
  LPCRITICAL_SECTION lpCriticalSection;
  undefined4 *puVar1;
  
  puVar1 = &DAT_10017848;
  do {
    lpCriticalSection = (LPCRITICAL_SECTION)*puVar1;
    if ((lpCriticalSection != (LPCRITICAL_SECTION)0x0) && (puVar1[1] != 1)) {
      DeleteCriticalSection(lpCriticalSection);
      FUN_10003a70(lpCriticalSection);
      *puVar1 = 0;
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10017968);
  puVar1 = &DAT_10017848;
  do {
    if (((LPCRITICAL_SECTION)*puVar1 != (LPCRITICAL_SECTION)0x0) && (puVar1[1] == 1)) {
      DeleteCriticalSection((LPCRITICAL_SECTION)*puVar1);
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10017968);
  return;
}


// 1000c5f2 FUN_1000c5f2

void __cdecl FUN_1000c5f2(int param_1)

{
  LeaveCriticalSection((LPCRITICAL_SECTION)(&DAT_10017848)[param_1 * 2]);
  return;
}


// 1000c607 __mtinitlocknum

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
  if (DAT_10016b9c == 0) {
    __FF_MSGBANNER();
    FUN_100090e9(0x1e);
    ___crtExitProcess(0xff);
  }
  piVar1 = &DAT_10017848 + _LockNum * 2;
  if (*piVar1 == 0) {
    iVar4 = FUN_10003a10(0x18);
    if (iVar4 == 0) {
      pDVar2 = FUN_1000ac50();
      *pDVar2 = 0xc;
      iVar4 = 0;
    }
    else {
      __lock(10);
      if (*piVar1 == 0) {
        iVar3 = ___crtInitCritSecAndSpinCount(iVar4,4000);
        if (iVar3 == 0) {
          FUN_10003a70(iVar4);
          pDVar2 = FUN_1000ac50();
          *pDVar2 = 0xc;
          local_20 = 0;
        }
        else {
          *piVar1 = iVar4;
        }
      }
      else {
        FUN_10003a70(iVar4);
      }
      FUN_1000c6c1();
      iVar4 = local_20;
    }
  }
  return iVar4;
}


// 1000c6c1 FUN_1000c6c1

void FUN_1000c6c1(void)

{
  FUN_1000c5f2(10);
  return;
}


// 1000c6ca __lock

/* Library Function - Single Match
    __lock
   
   Library: Visual Studio 2005 Release */

void __cdecl __lock(int _File)

{
  int iVar1;
  
  if ((&DAT_10017848)[_File * 2] == 0) {
    iVar1 = __mtinitlocknum(_File);
    if (iVar1 == 0) {
      __amsg_exit(0x11);
    }
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(&DAT_10017848)[_File * 2]);
  return;
}


// 1000c6fb __fltin2

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  local_24 = _Str;
  uVar3 = 0;
  local_18 = FUN_1000c82b((undefined2 *)&local_14,&local_28,_Str,0,0,0,0,in_stack_00000018);
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


// 1000c7a0 _strlen

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
    if ((char)uVar1 == '\0') goto LAB_1000c803;
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
LAB_1000c803:
  return (size_t)((int)puVar2 + (-1 - (int)_Str));
}


// 1000c82b FUN_1000c82b

void __cdecl
FUN_1000c82b(undefined2 *param_1,int *param_2,char *param_3,int param_4,int param_5,int param_6,
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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
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
LAB_1000c8b0:
    cVar8 = *param_3;
    pcVar17 = param_3 + 1;
    switch(iVar14) {
    case 0:
      if ((byte)(cVar8 - 0x31U) < 9) {
LAB_1000c8cd:
        iVar14 = 3;
        goto LAB_1000c8cf;
      }
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) {
LAB_1000c8e4:
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
          if (cVar8 == '0') goto LAB_1000c8fe;
          goto LAB_1000ca89;
        }
        iVar14 = 2;
        uVar7 = 0x8000;
        param_3 = pcVar17;
      }
      goto LAB_1000c8b0;
    case 1:
      bVar4 = true;
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_1000c8cd;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_1000c935;
      if ((cVar8 == '+') || (cVar8 == '-')) goto LAB_1000c965;
      if (cVar8 == '0') goto LAB_1000c8fe;
      goto LAB_1000c945;
    case 2:
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_1000c8cd;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_1000c8e4;
      param_3 = local_58;
      if (cVar8 != '0') goto LAB_1000cab4;
LAB_1000c8fe:
      iVar14 = 1;
      param_3 = pcVar17;
      goto LAB_1000c8b0;
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
      if (cVar8 != *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_1000c9d8;
LAB_1000c935:
      bVar4 = true;
      iVar14 = 4;
      param_3 = pcVar17;
      goto LAB_1000c8b0;
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
LAB_1000c9d8:
      if ((cVar8 == '+') || (cVar8 == '-')) {
LAB_1000c965:
        bVar4 = true;
        iVar14 = 0xb;
        param_3 = pcVar17 + -1;
      }
      else {
LAB_1000c945:
        bVar4 = true;
        if ((cVar8 < 'D') || (('E' < cVar8 && ((cVar8 < 'd' || ('e' < cVar8)))))) goto LAB_1000ca89;
        iVar14 = 6;
        param_3 = pcVar17;
      }
      goto LAB_1000c8b0;
    case 5:
      bVar6 = true;
      param_3 = local_58;
      if ((byte)(cVar8 - 0x30U) < 10) {
        iVar14 = 4;
        goto LAB_1000c8cf;
      }
      goto LAB_1000cab4;
    case 6:
      local_58 = param_3 + -1;
      if (8 < (byte)(cVar8 - 0x31U)) {
        if (cVar8 == '+') goto LAB_1000ca70;
        if (cVar8 == '-') goto LAB_1000ca64;
LAB_1000ca57:
        param_3 = local_58;
        if (cVar8 != '0') goto LAB_1000cab4;
        iVar14 = 8;
        param_3 = pcVar17;
        goto LAB_1000c8b0;
      }
      break;
    case 7:
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_1000ca57;
      break;
    case 8:
      bVar5 = true;
      while (cVar8 == '0') {
        cVar8 = *pcVar17;
        pcVar17 = pcVar17 + 1;
      }
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_1000ca89;
      break;
    case 9:
      bVar5 = true;
      local_68 = 0;
      goto LAB_1000cb16;
    default:
      goto switchD_1000c8bc_caseD_a;
    case 0xb:
      if (param_7 != 0) {
        local_58 = param_3;
        if (cVar8 == '+') {
LAB_1000ca70:
          iVar14 = 7;
          param_3 = pcVar17;
        }
        else {
          if (cVar8 != '-') goto LAB_1000cab4;
LAB_1000ca64:
          local_6c = -1;
          iVar14 = 7;
          param_3 = pcVar17;
        }
        goto LAB_1000c8b0;
      }
      iVar14 = 10;
      pcVar17 = param_3;
switchD_1000c8bc_caseD_a:
      param_3 = pcVar17;
      if (iVar14 != 10) goto LAB_1000c8b0;
      goto LAB_1000cab4;
    }
    iVar14 = 9;
LAB_1000c8cf:
    param_3 = pcVar17 + -1;
    goto LAB_1000c8b0;
  }
  pDVar10 = FUN_1000ac50();
  *pDVar10 = 0x16;
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
  goto LAB_1000ceac;
LAB_1000cb16:
  if ((cVar8 < '0') || ('9' < cVar8)) goto LAB_1000cb31;
  local_68 = local_68 * 10 + -0x30 + (int)cVar8;
  if (local_68 < 0x1451) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
    goto LAB_1000cb16;
  }
  local_68 = 0x1451;
LAB_1000cb31:
  while (('/' < cVar8 && (cVar8 < ':'))) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
  }
LAB_1000ca89:
  param_3 = pcVar17 + -1;
LAB_1000cab4:
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
    if (local_50 == 0) goto LAB_1000ce8e;
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
      if ((int)local_58 < -0x1450) goto LAB_1000ce8e;
      ppuVar20 = &PTR_DAT_10017cf0;
      if (local_58 != (char *)0x0) {
        if ((int)local_58 < 0) {
          local_58 = (char *)-(int)local_58;
          ppuVar20 = (undefined **)&DAT_10017e50;
        }
        if (param_4 == 0) {
          local_40._0_2_ = 0;
        }
        iVar14 = uStack_4a;
        uVar22 = uStack_2e;
        iVar1 = uStack_2a;
        uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
        iVar12 = local_3c;
joined_r0x1000cbbc:
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
LAB_1000cd72:
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
                    if ((short)uVar24 < 1) goto LAB_1000cd72;
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
                  if (0x7ffe < uVar24) goto LAB_1000ce31;
                  local_40 = uVar22;
                  local_3c = iVar1;
                  uStack_38 = uStack_26;
                  local_36 = uVar24 | uVar9;
                  uVar2 = uVar22;
                  iVar12 = iVar1;
                }
                goto joined_r0x1000cbbc;
              }
              iVar12 = 0;
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            else {
LAB_1000ce31:
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
          goto joined_r0x1000cbbc;
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
LAB_1000ce8e:
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
LAB_1000ceac:
  local_40 = uVar2;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1000cef0 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_1000c82b((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tod(&local_14,(_CRT_DOUBLE *)_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_1000cf47:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1000cf87;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_1000cf79:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1000cf87;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_1000cf79;
    goto LAB_1000cf47;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_1000cf87:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 1000cf96 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_1000c82b((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tof(&local_14,_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_1000cfed:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1000d02d;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_1000d01f:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1000d02d;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_1000d01f;
    goto LAB_1000cfed;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_1000d02d:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 1000d03c _strcpy_s

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
      pDVar2 = FUN_1000ac50();
      eVar4 = 0x22;
      *pDVar2 = 0x22;
      goto LAB_1000d05b;
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_1000ac50();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_1000d05b:
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 1000d0a1 __fptostr

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
    pDVar1 = FUN_1000ac50();
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
    pDVar1 = FUN_1000ac50();
    eVar7 = 0x22;
    *pDVar1 = 0x22;
  }
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar7;
}


// 1000d15e ___dtold

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
      goto LAB_1000d210;
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
LAB_1000d210:
  *(ushort *)(param_1 + 2) = uVar4;
  return;
}


// 1000d219 __fltout2

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  ___dtold(&local_14,(uint *)&_Dbl);
  iVar1 = FUN_100101f6(local_14,uStack_10,uStack_c,0x11,0,&local_30);
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


// 1000d2b0 __alldvrm

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


// 1000d390 __aullshr

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


// 1000d3b8 __controlfp_s

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
    pDVar2 = FUN_1000ac50();
    eVar3 = 0x16;
    *pDVar2 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return eVar3;
}


// 1000d422 FUN_1000d422

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

undefined4 FUN_1000d422(void)

{
  return 1;
}


// 1000d472 __get_sse2_info

/* WARNING: Removing unreachable block (ram,0x1000d4ad) */
/* WARNING: Removing unreachable block (ram,0x1000d49a) */
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
  if (((local_8 & 0x4000000) == 0) || (iVar2 = FUN_1000d422(), iVar2 == 0)) {
    uVar3 = 0;
  }
  else {
    uVar3 = 1;
  }
  return uVar3;
}


// 1000d4df __initp_misc_winsig

/* Library Function - Single Match
    __initp_misc_winsig
   
   Library: Visual Studio 2005 Release */

void __cdecl __initp_misc_winsig(undefined4 param_1)

{
  DAT_1001c7b0 = param_1;
  DAT_1001c7b4 = param_1;
  DAT_1001c7b8 = param_1;
  DAT_1001c7bc = param_1;
  return;
}


// 1000d4f8 siglookup

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
  } while (uVar1 < DAT_10017224 * 0xc + param_1);
  if ((DAT_10017224 * 0xc + param_1 <= uVar1) || (*(int *)(uVar1 + 4) != in_EDX)) {
    uVar1 = 0;
  }
  return uVar1;
}


// 1000d52c ___get_sigabrt

/* Library Function - Single Match
    ___get_sigabrt
   
   Library: Visual Studio 2005 Release */

_PHNDLR __cdecl ___get_sigabrt(void)

{
  _PHNDLR p_Var1;
  
  p_Var1 = (_PHNDLR)FUN_100093a5(DAT_1001c7b8);
  return p_Var1;
}


// 1000d539 _raise

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
        puVar6 = &DAT_1001c7b0;
        iVar3 = DAT_1001c7b0;
        goto LAB_1000d5ee;
      }
      if (_SigNum != 4) {
        if (_SigNum == 6) goto LAB_1000d5cc;
        if (_SigNum != 8) goto LAB_1000d5b0;
      }
    }
    pDVar7 = FUN_1000954a();
    if (pDVar7 == (DWORD *)0x0) {
      return -1;
    }
    uVar2 = siglookup(pDVar7[0x17]);
    puVar6 = (undefined4 *)(uVar2 + 8);
    pcVar4 = (code *)*puVar6;
  }
  else {
    if (_SigNum == 0xf) {
      puVar6 = &DAT_1001c7bc;
      iVar3 = DAT_1001c7bc;
    }
    else if (_SigNum == 0x15) {
      puVar6 = &DAT_1001c7b4;
      iVar3 = DAT_1001c7b4;
    }
    else {
      if (_SigNum != 0x16) {
LAB_1000d5b0:
        pDVar7 = FUN_1000ac50();
        *pDVar7 = 0x16;
        FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return -1;
      }
LAB_1000d5cc:
      puVar6 = &DAT_1001c7b8;
      iVar3 = DAT_1001c7b8;
    }
LAB_1000d5ee:
    bVar1 = true;
    pcVar4 = (code *)FUN_100093a5(iVar3);
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
      goto LAB_1000d652;
    }
  }
  else {
LAB_1000d652:
    if (_SigNum == 8) {
      for (local_28 = DAT_10017218; local_28 < DAT_1001721c + DAT_10017218; local_28 = local_28 + 1)
      {
        *(undefined4 *)(local_28 * 0xc + 8 + pDVar7[0x17]) = 0;
      }
      goto LAB_1000d68c;
    }
  }
  uVar5 = __encoded_null();
  *puVar6 = uVar5;
LAB_1000d68c:
  FUN_1000d6ad();
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


// 1000d6ad FUN_1000d6ad

void FUN_1000d6ad(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + -0x1c) != 0) {
    FUN_1000c5f2(0);
  }
  return;
}


// 1000d6e9 FUN_1000d6e9

undefined4 __cdecl FUN_1000d6e9(undefined4 param_1,undefined4 param_2,uint param_3)

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
  if (DAT_1001c7c4 == 0) {
    hModule = LoadLibraryA("USER32.DLL");
    if (hModule == (HMODULE)0x0) {
      return 0;
    }
    pFVar1 = GetProcAddress(hModule,"MessageBoxA");
    if (pFVar1 == (FARPROC)0x0) {
      return 0;
    }
    DAT_1001c7c4 = FUN_1000932e((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetActiveWindow");
    DAT_1001c7c8 = FUN_1000932e((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetLastActivePopup");
    DAT_1001c7cc = FUN_1000932e((int)pFVar1);
    iVar2 = __get_osplatform(&local_10);
    if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
    if (local_10 == 2) {
      pFVar1 = GetProcAddress(hModule,"GetUserObjectInformationA");
      DAT_1001c7d4 = FUN_1000932e((int)pFVar1);
      if (DAT_1001c7d4 != 0) {
        pFVar1 = GetProcAddress(hModule,"GetProcessWindowStation");
        DAT_1001c7d0 = FUN_1000932e((int)pFVar1);
      }
    }
  }
  if ((DAT_1001c7d0 != local_c) && (DAT_1001c7d4 != local_c)) {
    pcVar3 = (code *)FUN_100093a5(DAT_1001c7d0);
    pcVar4 = (code *)FUN_100093a5(DAT_1001c7d4);
    if (((pcVar3 != (code *)0x0) && (pcVar4 != (code *)0x0)) &&
       (((iVar2 = (*pcVar3)(), iVar2 == 0 ||
         (iVar2 = (*pcVar4)(iVar2,1,local_24,0xc,local_18), iVar2 == 0)) || ((local_1c & 1) == 0))))
    {
      iVar2 = FUN_10009b3b(&local_14);
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
      goto LAB_1000d87f;
    }
  }
  if ((((DAT_1001c7c8 != local_c) &&
       (pcVar3 = (code *)FUN_100093a5(DAT_1001c7c8), pcVar3 != (code *)0x0)) &&
      (local_8 = (*pcVar3)(), local_8 != 0)) &&
     ((DAT_1001c7cc != local_c &&
      (pcVar3 = (code *)FUN_100093a5(DAT_1001c7cc), pcVar3 != (code *)0x0)))) {
    local_8 = (*pcVar3)(local_8);
  }
LAB_1000d87f:
  pcVar3 = (code *)FUN_100093a5(DAT_1001c7c4);
  if (pcVar3 == (code *)0x0) {
    return 0;
  }
  uVar5 = (*pcVar3)(local_8,param_1,param_2,param_3);
  return uVar5;
}


// 1000d8a6 _strcat_s

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
        pDVar2 = FUN_1000ac50();
        eVar4 = 0x22;
        *pDVar2 = 0x22;
        goto LAB_1000d8c5;
      }
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_1000ac50();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_1000d8c5:
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 1000d917 _strncpy_s

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
LAB_1000d93b:
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
          pDVar2 = FUN_1000ac50();
          eVar5 = 0x22;
          *pDVar2 = 0x22;
          goto LAB_1000d94c;
        }
        *_Dst = '\0';
      }
    }
  }
  else if (_Dst != (char *)0x0) goto LAB_1000d93b;
  pDVar2 = FUN_1000ac50();
  eVar5 = 0x16;
  *pDVar2 = 0x16;
LAB_1000d94c:
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar5;
}


// 1000d9ca __set_error_mode

/* Library Function - Single Match
    __set_error_mode
   
   Library: Visual Studio 2005 Release */

int __cdecl __set_error_mode(int _Mode)

{
  int iVar1;
  DWORD *pDVar2;
  
  if (-1 < _Mode) {
    if (_Mode < 3) {
      iVar1 = DAT_1001c13c;
      DAT_1001c13c = _Mode;
      return iVar1;
    }
    if (_Mode == 3) {
      return DAT_1001c13c;
    }
  }
  pDVar2 = FUN_1000ac50();
  *pDVar2 = 0x16;
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1000da10 __ValidateImageBase

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


// 1000da40 __FindPESection

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


// 1000da90 __IsNonwritableInCurrentImage

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
  local_c = DAT_10017840 ^ 0x10015148;
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


// 1000db84 __initp_eh_hooks

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __initp_eh_hooks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __initp_eh_hooks(void)

{
  _DAT_1001c7d8 = FUN_1000932e(0x1000db4b);
  return;
}


// 1000db95 FUN_1000db95

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1000db95(undefined4 param_1)

{
  _DAT_1001c7dc = param_1;
  return;
}


// 1000db9f FUN_1000db9f

void __cdecl FUN_1000db9f(undefined4 param_1)

{
  DAT_1001c7e0 = param_1;
  return;
}


// 1000dba9 ___crtInitCritSecNoSpinCount@8

/* Library Function - Single Match
    ___crtInitCritSecNoSpinCount@8
   
   Library: Visual Studio 2005 Release */

undefined4 ___crtInitCritSecNoSpinCount_8(LPCRITICAL_SECTION param_1)

{
  InitializeCriticalSection(param_1);
  return 1;
}


// 1000dbb9 ___crtInitCritSecAndSpinCount

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
  
  local_8 = &DAT_10015188;
  uStack_c = 0x1000dbc5;
  local_20[0] = 0;
  pcVar1 = (code *)FUN_100093a5(DAT_1001c7e0);
  if (pcVar1 != (FARPROC)0x0) goto LAB_1000dc2f;
  iVar2 = __get_osplatform(local_20);
  if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  if (local_20[0] == 1) {
LAB_1000dc1e:
    pcVar1 = ___crtInitCritSecNoSpinCount_8;
  }
  else {
    hModule = GetModuleHandleA("kernel32.dll");
    if (hModule == (HMODULE)0x0) goto LAB_1000dc1e;
    pcVar1 = GetProcAddress(hModule,"InitializeCriticalSectionAndSpinCount");
    if (pcVar1 == (FARPROC)0x0) goto LAB_1000dc1e;
  }
  DAT_1001c7e0 = FUN_1000932e((int)pcVar1);
LAB_1000dc2f:
  local_8 = (undefined *)0x0;
  iVar2 = (*pcVar1)(param_1,param_2);
  return iVar2;
}


// 1000dc7e FUN_1000dc7e

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1000dc7e(undefined4 param_1)

{
  _DAT_1001c7e4 = param_1;
  return;
}


// 1000dc88 __local_unwind4

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
  puStack_24 = &LAB_1000dd18;
  uStack_28 = *unaff_FS_OFFSET;
  local_20 = DAT_10017840 ^ (uint)&uStack_28;
  *unaff_FS_OFFSET = &uStack_28;
  while( true ) {
    uVar2 = *(uint *)(param_2 + 0xc);
    if ((uVar2 == 0xfffffffe) || ((param_3 != 0xfffffffe && (uVar2 <= param_3)))) break;
    puVar1 = (undefined4 *)((*(uint *)(param_2 + 8) ^ *param_1) + 0x10 + uVar2 * 0xc);
    *(undefined4 *)(param_2 + 0xc) = *puVar1;
    if (puVar1[1] == 0) {
      __NLG_Notify(0x101);
      FUN_10011024();
    }
  }
  *unaff_FS_OFFSET = uStack_28;
  return;
}


// 1000dd5e FUN_1000dd5e

void FUN_1000dd5e(int param_1)

{
  __local_unwind4(*(uint **)(param_1 + 0x28),*(int *)(param_1 + 0x18),*(uint *)(param_1 + 0x1c));
  return;
}


// 1000dd7a _EH4_CallFilterFunc

/* Library Function - Single Match
    @_EH4_CallFilterFunc@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_CallFilterFunc,8 */

void __fastcall _EH4_CallFilterFunc(undefined *param_1)

{
  (*(code *)param_1)();
  return;
}


// 1000dd91 _EH4_TransferToHandler

/* Library Function - Single Match
    @_EH4_TransferToHandler@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_TransferToHandler,8 */

void __fastcall _EH4_TransferToHandler(undefined *UNRECOVERED_JUMPTABLE)

{
  __NLG_Notify(1);
                    /* WARNING: Could not recover jumptable at 0x1000dda8. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (*(code *)UNRECOVERED_JUMPTABLE)();
  return;
}


// 1000ddaa _EH4_GlobalUnwind

/* Library Function - Single Match
    @_EH4_GlobalUnwind@4
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_GlobalUnwind,4 */

void __fastcall _EH4_GlobalUnwind(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x1000ddbf,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 1000ddc4 _EH4_LocalUnwind

/* Library Function - Single Match
    @_EH4_LocalUnwind@16
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_LocalUnwind,16 */

void __fastcall _EH4_LocalUnwind(int param_1,uint param_2,undefined4 param_3,uint *param_4)

{
  __local_unwind4(param_4,param_1,param_2);
  return;
}


// 1000de20 __aulldvrm

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


// 1000deb5 __lseeki64_nolock

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
    pDVar2 = FUN_1000ac50();
    *pDVar2 = 9;
LAB_1000dee4:
    DVar3 = 0xffffffff;
    local_8 = -1;
  }
  else {
    DVar3 = SetFilePointer(hFile,in_stack_00000008,&local_8,_Offset._4_4_);
    if (DVar3 == 0xffffffff) {
      DVar4 = GetLastError();
      if (DVar4 != 0) {
        FUN_1000ac76(DVar4);
        goto LAB_1000dee4;
      }
    }
    pbVar1 = (byte *)((&DAT_1001dc20)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38);
    *pbVar1 = *pbVar1 & 0xfd;
  }
  return CONCAT44(local_8,DVar3);
}


// 1000df38 __lseeki64

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
    pDVar1 = FUN_1000ac63();
    *pDVar1 = 0;
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1001dc04)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_1001dc20)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
        pDVar1 = FUN_1000ac63();
        *pDVar1 = 0;
        pDVar1 = FUN_1000ac50();
        *pDVar1 = 9;
        FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        local_28._4_4_ = 0xffffffff;
        local_28._0_4_ = 0xffffffff;
      }
      else {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_1001dc20)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_1000ac50();
          *pDVar1 = 9;
          pDVar1 = FUN_1000ac63();
          *pDVar1 = 0;
          local_28 = -1;
        }
        else {
          local_28 = __lseeki64_nolock(_FileHandle,_Offset,in_stack_ffffffc8);
        }
        FUN_1000e047();
      }
      goto LAB_1000e041;
    }
    pDVar1 = FUN_1000ac63();
    *pDVar1 = 0;
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 9;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  local_28._0_4_ = 0xffffffff;
  local_28._4_4_ = 0xffffffff;
LAB_1000e041:
  return CONCAT44(local_28._4_4_,(undefined4)local_28);
}


// 1000e047 FUN_1000e047

void FUN_1000e047(void)

{
  int unaff_EBP;
  
  FUN_100111b9(*(uint *)(unaff_EBP + 8));
  return;
}


// 1000e051 __write_nolock

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
  
  local_8 = DAT_10017840 ^ (uint)local_51c;
  local_580 = (WCHAR *)_Buf;
  local_57c = (char *)0x0;
  local_584 = 0;
  if (_MaxCharCount == 0) goto LAB_1000e601;
  if (_Buf == (void *)0x0) {
    pDVar6 = FUN_1000ac63();
    *pDVar6 = 0;
    pDVar6 = FUN_1000ac50();
    *pDVar6 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_1000e601;
  }
  iVar11 = (_FileHandle & 0x1fU) * 0x38;
  piVar16 = &DAT_1001dc20 + (_FileHandle >> 5);
  local_571 = (char)(*(char *)(*piVar16 + iVar11 + 0x24) * '\x02') >> 1;
  local_58c = piVar16;
  if (((local_571 == '\x02') || (local_571 == '\x01')) && ((~_MaxCharCount & 1) == 0)) {
    pDVar6 = FUN_1000ac63();
    *pDVar6 = 0;
    pDVar6 = FUN_1000ac50();
    *pDVar6 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_1000e601;
  }
  if ((*(byte *)(*piVar16 + iVar11 + 4) & 0x20) != 0) {
    __lseeki64_nolock(_FileHandle,0x200000000,unaff_EDI);
  }
  iVar7 = __isatty(_FileHandle);
  if ((iVar7 == 0) || ((*(byte *)(iVar11 + 4 + *piVar16) & 0x80) == 0)) {
LAB_1000e33c:
    if ((*(byte *)((undefined4 *)(*piVar16 + iVar11) + 1) & 0x80) == 0) {
      BVar9 = WriteFile(*(HANDLE *)(*piVar16 + iVar11),local_580,_MaxCharCount,(LPDWORD)&local_588,
                        (LPOVERLAPPED)0x0);
      if (BVar9 == 0) {
LAB_1000e596:
        local_570 = GetLastError();
      }
      else {
        local_570 = 0;
        local_57c = local_588;
      }
LAB_1000e59f:
      piVar16 = local_58c;
      if (local_57c != (char *)0x0) goto LAB_1000e601;
      goto LAB_1000e5a9;
    }
    local_570 = 0;
    if (local_571 == '\0') {
      local_56c = local_580;
      if (_MaxCharCount == 0) goto LAB_1000e5d0;
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
        if (BVar9 == 0) goto LAB_1000e596;
        local_57c = local_57c + (int)local_588;
      } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
              ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
      goto LAB_1000e59f;
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
          if (BVar9 == 0) goto LAB_1000e596;
          local_57c = local_57c + (int)local_588;
        } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
                ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
        goto LAB_1000e59f;
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
          if (iVar7 == 0) goto LAB_1000e596;
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
        goto LAB_1000e59f;
      }
    }
LAB_1000e5d0:
    DVar10 = 0;
    if (((*(byte *)(iVar11 + 4 + *piVar16) & 0x40) != 0) && ((char)*local_580 == '\x1a'))
    goto LAB_1000e601;
    pDVar6 = FUN_1000ac50();
    *pDVar6 = 0x1c;
  }
  else {
    p_Var8 = __getptd();
    pwVar3 = p_Var8->ptlocinfo->lc_category[0].wlocale;
    BVar9 = GetConsoleMode(*(HANDLE *)(iVar11 + *piVar16),&local_598);
    if ((BVar9 == 0) || ((pwVar3 == (wchar_t *)0x0 && (local_571 == '\0')))) goto LAB_1000e33c;
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
          if (BVar9 == 0) goto LAB_1000e596;
          local_57c = local_57c + (int)local_56c;
          if ((int)local_56c < (int)DVar10) break;
          if (local_594 != 0) {
            local_10[0] = '\r';
            BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_10,1,(LPDWORD)&local_56c,
                              (LPOVERLAPPED)0x0);
            if (BVar9 == 0) goto LAB_1000e596;
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
            if (wVar5 != (wchar_t)local_570) goto LAB_1000e596;
            local_57c = local_57c + 1;
            if (local_594 != 0) {
              local_570 = 0xd;
              wVar5 = __putwch_nolock(L'\r');
              if (wVar5 != (wchar_t)local_570) goto LAB_1000e596;
              local_57c = local_57c + 1;
              local_584 = local_584 + 1;
            }
          }
        }
      } while (local_578 < _MaxCharCount);
      goto LAB_1000e59f;
    }
LAB_1000e5a9:
    if (local_570 == 0) goto LAB_1000e5d0;
    DVar10 = 5;
    if (local_570 != 5) {
      FUN_1000ac76(local_570);
      goto LAB_1000e601;
    }
    pDVar6 = FUN_1000ac50();
    *pDVar6 = 9;
  }
  pDVar6 = FUN_1000ac63();
  *pDVar6 = DVar10;
LAB_1000e601:
  iVar11 = __security_check_cookie(local_8 ^ (uint)local_51c);
  return iVar11;
}


// 1000e617 __write

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
    pDVar1 = FUN_1000ac63();
    *pDVar1 = 0;
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1001dc04)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_1001dc20)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_1001dc20)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_1000ac50();
          *pDVar1 = 9;
          pDVar1 = FUN_1000ac63();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = __write_nolock(_FileHandle,_Buf,_MaxCharCount);
        }
        FUN_1000e6e9();
        return local_20;
      }
    }
    pDVar1 = FUN_1000ac63();
    *pDVar1 = 0;
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 9;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 1000e6e9 FUN_1000e6e9

void FUN_1000e6e9(void)

{
  int unaff_EBP;
  
  FUN_100111b9(*(uint *)(unaff_EBP + 8));
  return;
}


// 1000e6f3 __getbuf

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __getbuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __getbuf(FILE *_File)

{
  char *pcVar1;
  
  _DAT_1001c7e8 = _DAT_1001c7e8 + 1;
  pcVar1 = (char *)FUN_10003a10(0x1000);
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


// 1000e737 __isatty

/* Library Function - Single Match
    __isatty
   
   Library: Visual Studio 2005 Release */

int __cdecl __isatty(int _FileHandle)

{
  DWORD *pDVar1;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 9;
    return 0;
  }
  if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1001dc04)) {
    return *(byte *)((&DAT_1001dc20)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38) & 0x40;
  }
  pDVar1 = FUN_1000ac50();
  *pDVar1 = 9;
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0;
}


// 1000e795 FUN_1000e795

undefined ** FUN_1000e795(void)

{
  return &PTR_DAT_10017990;
}


// 1000e936 __fileno

/* Library Function - Single Match
    __fileno
   
   Library: Visual Studio 2005 Release */

int __cdecl __fileno(FILE *_File)

{
  DWORD *pDVar1;
  
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 0x16;
    FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  return _File->_file;
}


// 1000e963 FUN_1000e963

bool FUN_1000e963(void)

{
  return DAT_1001c7ec == (DAT_10017840 | 1);
}


// 1000e979 __wctomb_s_l

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
LAB_1000e99b:
    DVar1 = 0;
  }
  else {
    if (_SizeConverted != (int *)0x0) {
      *_SizeConverted = -1;
    }
    if (0x7fffffff < _SizeInBytes) {
      pDVar2 = FUN_1000ac50();
      *pDVar2 = 0x16;
      FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return 0x16;
    }
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
    if (*(int *)(local_14[0] + 0x14) == 0) {
      if ((ushort)_WCh < 0x100) {
        if (lpMultiByteStr != (char *)0x0) {
          if (_Size == 0) goto LAB_1000ea2b;
          *lpMultiByteStr = (char)_WCh;
        }
        if (_SizeConverted != (int *)0x0) {
          *_SizeConverted = 1;
        }
LAB_1000ea62:
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        goto LAB_1000e99b;
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
LAB_1000ea2b:
          pDVar2 = FUN_1000ac50();
          *pDVar2 = 0x22;
          FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_1000ea62;
      }
    }
    pDVar2 = FUN_1000ac50();
    *pDVar2 = 0x2a;
    pDVar2 = FUN_1000ac50();
    DVar1 = *pDVar2;
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return DVar1;
}


// 1000ead8 _wctomb_s

/* Library Function - Single Match
    _wctomb_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _wctomb_s(int *_SizeConverted,char *_MbCh,rsize_t _SizeInBytes,wchar_t _WCh)

{
  errno_t eVar1;
  
  eVar1 = __wctomb_s_l(_SizeConverted,_MbCh,_SizeInBytes,_WCh,(_locale_t)0x0);
  return eVar1;
}


// 1000eaf3 FUN_1000eaf3

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_1000eaf3(void)

{
  _DAT_1001cbe0 = 0;
  return;
}


// 1000eafb x_ismbbtype_l

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
    if (uVar1 == 0) goto LAB_1000eb3d;
  }
  uVar1 = 1;
LAB_1000eb3d:
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 1000eb4c __ismbblead

/* Library Function - Single Match
    __ismbblead
   
   Library: Visual Studio 2005 Release */

int __cdecl __ismbblead(uint _C)

{
  int iVar1;
  
  iVar1 = x_ismbbtype_l((localeinfo_struct *)0x0,_C,0,4);
  return iVar1;
}


// 1000eb5f __crtGetStringTypeA_stat

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  local_c = in_ECX;
  if (DAT_1001c7f0 == 0) {
    BVar2 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_1001c7f0 = 2;
      }
      goto LAB_1000ebb8;
    }
    DAT_1001c7f0 = 1;
  }
  else {
LAB_1000ebb8:
    if ((DAT_1001c7f0 == 2) || (DAT_1001c7f0 == 0)) {
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
        FUN_10003a70(uVar7);
      }
      goto LAB_1000ed05;
    }
    if (DAT_1001c7f0 != 1) goto LAB_1000ed05;
  }
  local_c = (int *)0x0;
  if (param_5 == (ushort *)0x0) {
    param_5 = *(ushort **)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar((UINT)param_5,(uint)(param_7 != 0) * 8 + 1,(LPCSTR)param_2,
                                    (int)param_3,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_1000ed05;
  lpWideCharStr = (LPCWSTR)0x0;
  if ((0 < (int)cchWideChar) && (cchWideChar < 0x7ffffff1)) {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar4 = (LPCWSTR)&stack0xffffffe8;
      lpWideCharStr = (LPCWSTR)&stack0xffffffe8;
      if (&stack0x00000000 != (undefined1 *)0x18) {
LAB_1000ec48:
        lpWideCharStr = pWVar4 + 4;
      }
    }
    else {
      pWVar4 = (LPCWSTR)FUN_10003a10(uVar1);
      lpWideCharStr = pWVar4;
      if (pWVar4 != (LPCWSTR)0x0) {
        pWVar4[0] = L'\xdddd';
        pWVar4[1] = L'\0';
        goto LAB_1000ec48;
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
LAB_1000ed05:
  iVar5 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar5;
}


// 1000ed17 ___crtGetStringTypeA

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


// 1000ed57 ___free_lc_time

/* Library Function - Single Match
    ___free_lc_time
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lc_time(undefined4 *param_1)

{
  if (param_1 != (undefined4 *)0x0) {
    FUN_10003a70(param_1[1]);
    FUN_10003a70(param_1[2]);
    FUN_10003a70(param_1[3]);
    FUN_10003a70(param_1[4]);
    FUN_10003a70(param_1[5]);
    FUN_10003a70(param_1[6]);
    FUN_10003a70(*param_1);
    FUN_10003a70(param_1[8]);
    FUN_10003a70(param_1[9]);
    FUN_10003a70(param_1[10]);
    FUN_10003a70(param_1[0xb]);
    FUN_10003a70(param_1[0xc]);
    FUN_10003a70(param_1[0xd]);
    FUN_10003a70(param_1[7]);
    FUN_10003a70(param_1[0xe]);
    FUN_10003a70(param_1[0xf]);
    FUN_10003a70(param_1[0x10]);
    FUN_10003a70(param_1[0x11]);
    FUN_10003a70(param_1[0x12]);
    FUN_10003a70(param_1[0x13]);
    FUN_10003a70(param_1[0x14]);
    FUN_10003a70(param_1[0x15]);
    FUN_10003a70(param_1[0x16]);
    FUN_10003a70(param_1[0x17]);
    FUN_10003a70(param_1[0x18]);
    FUN_10003a70(param_1[0x19]);
    FUN_10003a70(param_1[0x1a]);
    FUN_10003a70(param_1[0x1b]);
    FUN_10003a70(param_1[0x1c]);
    FUN_10003a70(param_1[0x1d]);
    FUN_10003a70(param_1[0x1e]);
    FUN_10003a70(param_1[0x1f]);
    FUN_10003a70(param_1[0x20]);
    FUN_10003a70(param_1[0x21]);
    FUN_10003a70(param_1[0x22]);
    FUN_10003a70(param_1[0x23]);
    FUN_10003a70(param_1[0x24]);
    FUN_10003a70(param_1[0x25]);
    FUN_10003a70(param_1[0x26]);
    FUN_10003a70(param_1[0x27]);
    FUN_10003a70(param_1[0x28]);
    FUN_10003a70(param_1[0x29]);
    FUN_10003a70(param_1[0x2a]);
  }
  return;
}


// 1000eee7 ___free_lconv_num

/* Library Function - Single Match
    ___free_lconv_num
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_num(int *param_1)

{
  if (param_1 != (int *)0x0) {
    if ((undefined *)*param_1 != PTR_DAT_10017cd8) {
      FUN_10003a70((undefined *)*param_1);
    }
    if ((undefined *)param_1[1] != PTR_DAT_10017cdc) {
      FUN_10003a70((undefined *)param_1[1]);
    }
    if ((undefined *)param_1[2] != PTR_DAT_10017ce0) {
      FUN_10003a70((undefined *)param_1[2]);
    }
  }
  return;
}


// 1000ef27 ___free_lconv_mon

/* Library Function - Single Match
    ___free_lconv_mon
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_mon(int param_1)

{
  if (param_1 != 0) {
    if (*(undefined **)(param_1 + 0xc) != PTR_DAT_10017ce4) {
      FUN_10003a70(*(undefined **)(param_1 + 0xc));
    }
    if (*(undefined **)(param_1 + 0x10) != PTR_DAT_10017ce8) {
      FUN_10003a70(*(undefined **)(param_1 + 0x10));
    }
    if (*(undefined **)(param_1 + 0x14) != PTR_DAT_10017cec) {
      FUN_10003a70(*(undefined **)(param_1 + 0x14));
    }
    if (*(undefined **)(param_1 + 0x18) != PTR_DAT_10017cf0) {
      FUN_10003a70(*(undefined **)(param_1 + 0x18));
    }
    if (*(undefined **)(param_1 + 0x1c) != PTR_DAT_10017cf4) {
      FUN_10003a70(*(undefined **)(param_1 + 0x1c));
    }
    if (*(undefined **)(param_1 + 0x20) != PTR_DAT_10017cf8) {
      FUN_10003a70(*(undefined **)(param_1 + 0x20));
    }
    if (*(undefined **)(param_1 + 0x24) != PTR_DAT_10017cfc) {
      FUN_10003a70(*(undefined **)(param_1 + 0x24));
    }
  }
  return;
}


// 1000efb0 _strcspn

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


// 1000f000 _strpbrk

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


// 1000f040 ___ansicp

/* Library Function - Single Match
    ___ansicp
   
   Library: Visual Studio 2005 Release */

void __cdecl ___ansicp(LCID param_1)

{
  int iVar1;
  CHAR local_10 [6];
  undefined1 local_a;
  uint local_8;
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  local_a = 0;
  iVar1 = GetLocaleInfoA(param_1,0x1004,local_10,6);
  if (iVar1 != 0) {
    _atol(local_10);
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1000f087 ___convertcp

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  cbMultiByte = *param_4;
  bVar2 = false;
  if (param_1 == param_2) goto LAB_1000f227;
  BVar3 = GetCPInfo(param_1,&local_1c);
  if ((((BVar3 == 0) || (local_1c.MaxCharSize != 1)) ||
      (BVar3 = GetCPInfo(param_2,&local_1c), BVar3 == 0)) || (local_1c.MaxCharSize != 1)) {
    uVar7 = MultiByteToWideChar(param_1,1,param_3,cbMultiByte,(LPWSTR)0x0,0);
    bVar8 = uVar7 == 0;
    if (bVar8) goto LAB_1000f227;
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
LAB_1000f167:
        local_20 = pWVar5 + 4;
      }
    }
    else {
      pWVar5 = (LPCWSTR)FUN_10003a10(uVar1);
      local_20 = pWVar5;
      if (pWVar5 != (LPCWSTR)0x0) {
        pWVar5[0] = L'\xdddd';
        pWVar5[1] = L'\0';
        goto LAB_1000f167;
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
           (lpMultiByteStr = (LPSTR)FUN_10003bf0(1,uVar7), lpMultiByteStr != (LPSTR)0x0)) {
          uVar7 = WideCharToMultiByte(param_2,0,local_20,uVar7,lpMultiByteStr,uVar7,(LPCSTR)0x0,
                                      (LPBOOL)0x0);
          if (uVar7 == 0) {
            FUN_10003a70(lpMultiByteStr);
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
LAB_1000f227:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1000f239 ___report_gsfailure

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
  
  _DAT_1001c910 =
       (uint)(in_NT & 1) * 0x4000 | (uint)SBORROW4((int)&stack0xfffffffc,0x328) * 0x800 |
       (uint)(in_IF & 1) * 0x200 | (uint)(in_TF & 1) * 0x100 | (uint)((int)&local_32c < 0) * 0x80 |
       (uint)(&stack0x00000000 == (undefined1 *)0x32c) * 0x40 | (uint)(in_AF & 1) * 0x10 |
       (uint)((POPCOUNT((uint)&local_32c & 0xff) & 1U) == 0) * 4 |
       (uint)(&stack0xfffffffc < (undefined1 *)0x328) | (uint)(in_ID & 1) * 0x200000 |
       (uint)(in_VIP & 1) * 0x100000 | (uint)(in_VIF & 1) * 0x80000 | (uint)(in_AC & 1) * 0x40000;
  _DAT_1001c914 = &stack0x00000004;
  _DAT_1001c850 = 0x10001;
  _DAT_1001c7f8 = 0xc0000409;
  _DAT_1001c7fc = 1;
  local_32c = DAT_10017840;
  local_328 = DAT_10017844;
  _DAT_1001c804 = unaff_retaddr;
  _DAT_1001c8dc = in_GS;
  _DAT_1001c8e0 = in_FS;
  _DAT_1001c8e4 = in_ES;
  _DAT_1001c8e8 = in_DS;
  _DAT_1001c8ec = unaff_EDI;
  _DAT_1001c8f0 = unaff_ESI;
  _DAT_1001c8f4 = unaff_EBX;
  _DAT_1001c8f8 = in_EDX;
  _DAT_1001c8fc = in_ECX;
  _DAT_1001c900 = in_EAX;
  _DAT_1001c904 = unaff_EBP;
  DAT_1001c908 = unaff_retaddr;
  _DAT_1001c90c = in_CS;
  _DAT_1001c918 = in_SS;
  DAT_1001c848 = IsDebuggerPresent();
  FUN_1000eaf3();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  UnhandledExceptionFilter((_EXCEPTION_POINTERS *)&PTR_DAT_1001489c);
  if (DAT_1001c848 == 0) {
    FUN_1000eaf3();
  }
  uExitCode = 0xc0000409;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  return;
}


// 1000f33d __toupper_l

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
LAB_1000f39a:
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
      pDVar4 = FUN_1000ac50();
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
    if (iVar3 == 0) goto LAB_1000f39a;
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


// 1000f455 _toupper

/* Library Function - Single Match
    _toupper
   
   Library: Visual Studio 2005 Release */

int __cdecl _toupper(int _C)

{
  if (DAT_1001c5f8 == 0) {
    if (_C - 0x61U < 0x1a) {
      return _C + -0x20;
    }
  }
  else {
    _C = __toupper_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 1000f47c __mbtowc_l

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
          if (iVar2 != 0) goto LAB_1000f4c7;
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
        pDVar3 = FUN_1000ac50();
        *pDVar3 = 0x2a;
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        return -1;
      }
      if (_DstCh != (wchar_t *)0x0) {
        *_DstCh = (ushort)(byte)*_SrcCh;
      }
LAB_1000f4c7:
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


// 1000f58f _mbtowc

/* Library Function - Single Match
    _mbtowc
   
   Library: Visual Studio 2005 Release */

int __cdecl _mbtowc(wchar_t *_DstCh,char *_SrcCh,size_t _SrcSizeInBytes)

{
  int iVar1;
  
  iVar1 = __mbtowc_l(_DstCh,_SrcCh,_SrcSizeInBytes,(_locale_t)0x0);
  return iVar1;
}


// 1000f5a6 __ld12tod

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
        goto LAB_1000faa5;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10017d28 - 1;
    iVar4 = (int)(DAT_10017d28 + ((int)DAT_10017d28 >> 0x1f & 0x1fU)) >> 5;
    uVar8 = DAT_10017d28 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar6) goto LAB_1000f6d9;
          bVar14 = _Ifp < p_Var9;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_1000f6e0;
LAB_1000f6d9:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_1000f6e0:
              iVar4 = iVar4 + -1;
              *puVar6 = (uint)_Ifp;
              if ((iVar4 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_1000f6ee;
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
LAB_1000f6ee:
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
    if (iVar12 < (int)(DAT_10017d24 - DAT_10017d28)) {
      local_24[0] = 0;
      local_24[1] = 0;
    }
    else {
      if (DAT_10017d24 < iVar12) {
        if (iVar12 < DAT_10017d20) {
          local_24[0] = local_24[0] & 0x7fffffff;
          iVar12 = iVar12 + DAT_10017d34;
          iVar2 = (int)(DAT_10017d2c + ((int)DAT_10017d2c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_10017d2c & 0x8000001f;
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
          iVar12 = (int)(DAT_10017d2c + ((int)DAT_10017d2c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_10017d2c & 0x8000001f;
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
          iVar12 = DAT_10017d34 + DAT_10017d20;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_1000faa5;
      }
      local_14 = DAT_10017d24 - local_14;
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
      iVar2 = DAT_10017d28 - 1;
      iVar12 = (int)(DAT_10017d28 + ((int)DAT_10017d28 >> 0x1f & 0x1fU)) >> 5;
      uVar10 = DAT_10017d28 & 0x8000001f;
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
          if (2 < iVar12) goto LAB_1000f891;
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
LAB_1000f891:
      *puVar7 = *puVar7 & -1 << ((byte)local_14 & 0x1f);
      iVar12 = local_10 + 1;
      if (iVar12 < 3) {
        puVar7 = local_24 + iVar12;
        for (iVar2 = 3 - iVar12; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar7 = 0;
          puVar7 = puVar7 + 1;
        }
      }
      uVar10 = DAT_10017d2c + 1;
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
LAB_1000faa5:
  local_24[0] = iVar12 << (0x1fU - (char)DAT_10017d2c & 0x1f) |
                -(uint)(local_24[3] != 0) & 0x80000000 | local_24[0];
  if (DAT_10017d30 == 0x40) {
    *(uint *)((int)&_D->x + 4) = local_24[0];
    *(uint *)&_D->x = local_24[1];
  }
  else if (DAT_10017d30 == 0x20) {
    *(uint *)&_D->x = local_24[0];
  }
  return IVar3;
}


// 1000fae8 __ld12tof

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
        goto LAB_1000ffe7;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10017d40 - 1;
    iVar2 = (int)(DAT_10017d40 + ((int)DAT_10017d40 >> 0x1f & 0x1fU)) >> 5;
    uVar7 = DAT_10017d40 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar5) goto LAB_1000fc1b;
          bVar14 = _Ifp < p_Var8;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_1000fc22;
LAB_1000fc1b:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_1000fc22:
              iVar2 = iVar2 + -1;
              *puVar5 = (uint)_Ifp;
              if ((iVar2 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_1000fc30;
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
LAB_1000fc30:
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
    if (iVar11 < (int)(DAT_10017d3c - DAT_10017d40)) {
      local_24 = 0;
      local_20 = 0.0;
    }
    else {
      if (DAT_10017d3c < iVar11) {
        if (iVar11 < DAT_10017d38) {
          local_24 = local_24 & 0x7fffffff;
          iVar11 = iVar11 + DAT_10017d4c;
          iVar2 = (int)(DAT_10017d44 + ((int)DAT_10017d44 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10017d44 & 0x8000001f;
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
          iVar11 = (int)(DAT_10017d44 + ((int)DAT_10017d44 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10017d44 & 0x8000001f;
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
          iVar11 = DAT_10017d4c + DAT_10017d38;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_1000ffe7;
      }
      local_1c[2] = DAT_10017d3c - local_1c[2];
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
      iVar2 = DAT_10017d40 - 1;
      iVar11 = (int)(DAT_10017d40 + ((int)DAT_10017d40 >> 0x1f & 0x1fU)) >> 5;
      uVar9 = DAT_10017d40 & 0x8000001f;
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
          if (2 < iVar11) goto LAB_1000fdd3;
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
LAB_1000fdd3:
      *puVar6 = *puVar6 & -1 << ((byte)local_1c[2] & 0x1f);
      iVar11 = local_1c[3] + 1;
      if (iVar11 < 3) {
        puVar6 = &local_24 + iVar11;
        for (iVar2 = 3 - iVar11; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar6 = 0;
          puVar6 = puVar6 + 1;
        }
      }
      uVar9 = DAT_10017d44 + 1;
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
LAB_1000ffe7:
  fVar12 = (float)(iVar11 << (0x1fU - (char)DAT_10017d44 & 0x1f) |
                   -(uint)(local_1c[1] != 0) & 0x80000000 | local_24);
  if (DAT_10017d48 == 0x40) {
    _F[1].f = fVar12;
    _F->f = local_20;
  }
  else if (DAT_10017d48 == 0x20) {
    _F->f = fVar12;
  }
  return IVar3;
}


// 1001002a ___mtold12

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
  
  uVar7 = DAT_10017840 ^ (uint)&stack0xfffffffc;
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


// 100101f6 FUN_100101f6

/* WARNING: Removing unreachable block (ram,0x100106e7) */
/* WARNING: Removing unreachable block (ram,0x100106f1) */
/* WARNING: Removing unreachable block (ram,0x100106f6) */

void __cdecl
FUN_100101f6(int param_1,uint param_2,ushort param_3,int param_4,byte param_5,short *param_6)

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
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
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
    goto LAB_10010a74;
  }
  if ((short)uVar14 == 0x7fff) {
    *param_6 = 1;
    if (((param_2 == 0x80000000) && (param_1 == 0)) || ((param_2 & 0x40000000) != 0)) {
      if ((uVar8 == 0) || (param_2 != 0xc0000000)) {
        if ((param_2 != 0x80000000) || (param_1 != 0)) goto LAB_10010325;
        pcVar25 = "1#INF";
      }
      else {
        if (param_1 != 0) {
LAB_10010325:
          pcVar25 = "1#QNAN";
          goto LAB_1001032a;
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
LAB_1001032a:
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
    goto LAB_10010a74;
  }
  local_50 = (short)(((uVar14 >> 8) + (param_2 >> 0x18) * 2) * 0x4d + -0x134312f4 + uVar14 * 0x4d10
                    >> 0x10);
  uVar16 = (uint)local_50;
  local_1a = (undefined1)uVar14;
  bStack_19 = (byte)(uVar14 >> 8);
  local_24._2_2_ = (ushort)param_1;
  local_24._0_2_ = 0;
  local_6c = &PTR_DAT_10017cf0;
  uStack_20._0_2_ = uStack_12;
  uStack_20._2_2_ = (ushort)local_10;
  uStack_1c = local_10._2_2_;
  if (uVar16 != 0) {
    iVar20 = param_1;
    uVar14 = -uVar16;
    if (0 < (int)uVar16) {
      local_6c = (undefined **)&DAT_10017e50;
      uVar14 = uVar16;
    }
    while (uVar14 != 0) {
      uStack_20._0_2_ = (ushort)((uint)iVar20 >> 0x10);
      local_24._2_2_ = (ushort)iVar20;
      local_6c = local_6c + 0x15;
      iVar3 = CONCAT22(local_c,local_10._2_2_);
      if ((uVar14 & 7) == 0) goto LAB_1001064b;
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
LAB_1001056f:
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
            if ((short)uVar24 < 1) goto LAB_1001056f;
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
          if (0x7ffe < uVar24) goto LAB_1001062f;
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
LAB_1001062f:
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
LAB_1001064b:
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
          goto LAB_100108b3;
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
LAB_100107c1:
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
          if ((short)uVar24 < 1) goto LAB_100107c1;
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
        goto LAB_100108b3;
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
LAB_100108b3:
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
        goto LAB_10010a74;
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
LAB_10010a74:
  local_24 = uVar16;
  local_10 = param_2;
  uStack_3e = iVar1;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10010abc __hw_cw

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


// 10010b4a ___hw_cw_sse2

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


// 10010bea __control87

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
  if (DAT_1001dd3c != 0) {
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


// 10010ef0 __global_unwind2

/* Library Function - Single Match
    __global_unwind2
   
   Library: Visual Studio */

void __cdecl __global_unwind2(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x10010f08,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 10010f55 __local_unwind2

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
  puStack_1c = &LAB_10010f10;
  local_20 = *unaff_FS_OFFSET;
  *unaff_FS_OFFSET = &local_20;
  while( true ) {
    uVar1 = *(uint *)(param_1 + 0xc);
    if ((uVar1 == 0xffffffff) || ((param_2 != 0xffffffff && (uVar1 <= param_2)))) break;
    local_18 = *(undefined4 *)(*(int *)(param_1 + 8) + uVar1 * 0xc);
    *(undefined4 *)(param_1 + 0xc) = local_18;
    if (*(int *)(*(int *)(param_1 + 8) + 4 + uVar1 * 0xc) == 0) {
      __NLG_Notify(0x101);
      FUN_10011024();
    }
  }
  *unaff_FS_OFFSET = local_20;
  return;
}


// 10011005 __NLG_Notify

/* Library Function - Single Match
    __NLG_Notify
   
   Libraries: Visual Studio 2017 Debug, Visual Studio 2017 Release, Visual Studio 2019 Debug, Visual
   Studio 2019 Release */

void __NLG_Notify(ulong param_1)

{
  undefined4 in_EAX;
  undefined4 unaff_EBP;
  
  DAT_10018014 = param_1;
  DAT_10018010 = in_EAX;
  DAT_10018018 = unaff_EBP;
  return;
}


// 10011024 FUN_10011024

void FUN_10011024(void)

{
  code *in_EAX;
  
  (*in_EAX)();
  return;
}


// 10011027 FUN_10011027

undefined4 __cdecl FUN_10011027(uint param_1)

{
  int *piVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD nStdHandle;
  
  if ((-1 < (int)param_1) && (param_1 < DAT_1001dc04)) {
    iVar3 = (param_1 & 0x1f) * 0x38;
    piVar1 = (int *)((&DAT_1001dc20)[(int)param_1 >> 5] + iVar3);
    if (((*(byte *)(piVar1 + 1) & 1) != 0) && (*piVar1 != -1)) {
      if (DAT_1001c140 == 1) {
        if (param_1 == 0) {
          nStdHandle = 0xfffffff6;
        }
        else if (param_1 == 1) {
          nStdHandle = 0xfffffff5;
        }
        else {
          if (param_1 != 2) goto LAB_10011085;
          nStdHandle = 0xfffffff4;
        }
        SetStdHandle(nStdHandle,(HANDLE)0x0);
      }
LAB_10011085:
      *(undefined4 *)(iVar3 + (&DAT_1001dc20)[(int)param_1 >> 5]) = 0xffffffff;
      return 0;
    }
  }
  pDVar2 = FUN_1000ac50();
  *pDVar2 = 9;
  pDVar2 = FUN_1000ac63();
  *pDVar2 = 0;
  return 0xffffffff;
}


// 100110a8 __get_osfhandle

/* Library Function - Single Match
    __get_osfhandle
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __get_osfhandle(int _FileHandle)

{
  DWORD *pDVar1;
  intptr_t *piVar2;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_1000ac63();
    *pDVar1 = 0;
    pDVar1 = FUN_1000ac50();
    *pDVar1 = 9;
    return -1;
  }
  if (((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1001dc04)) &&
     (piVar2 = (intptr_t *)((_FileHandle & 0x1fU) * 0x38 + (&DAT_1001dc20)[_FileHandle >> 5]),
     (*(byte *)(piVar2 + 1) & 1) != 0)) {
    return *piVar2;
  }
  pDVar1 = FUN_1000ac63();
  *pDVar1 = 0;
  pDVar1 = FUN_1000ac50();
  *pDVar1 = 9;
  FUN_1000abf1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 10011119 ___lock_fhandle

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
  
  iVar2 = (_Filehandle & 0x1fU) * 0x38 + (&DAT_1001dc20)[_Filehandle >> 5];
  local_20 = 1;
  if (*(int *)(iVar2 + 8) == 0) {
    __lock(10);
    if (*(int *)(iVar2 + 8) == 0) {
      iVar1 = ___crtInitCritSecAndSpinCount(iVar2 + 0xc,4000);
      local_20 = (uint)(iVar1 != 0);
      *(int *)(iVar2 + 8) = *(int *)(iVar2 + 8) + 1;
    }
    FUN_100111b0();
  }
  if (local_20 != 0) {
    EnterCriticalSection
              ((LPCRITICAL_SECTION)
               ((&DAT_1001dc20)[_Filehandle >> 5] + 0xc + (_Filehandle & 0x1fU) * 0x38));
  }
  return local_20;
}


// 100111b0 FUN_100111b0

void FUN_100111b0(void)

{
  FUN_1000c5f2(10);
  return;
}


// 100111b9 FUN_100111b9

void __cdecl FUN_100111b9(uint param_1)

{
  LeaveCriticalSection
            ((LPCRITICAL_SECTION)
             ((&DAT_1001dc20)[(int)param_1 >> 5] + 0xc + (param_1 & 0x1f) * 0x38));
  return;
}


// 100111db __putwch_nolock

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
  
  local_8 = DAT_10017840 ^ (uint)&stack0xfffffffc;
  if (DAT_1001801c != 0) {
    if (DAT_100180f0 == (HANDLE)0xfffffffe) {
      ___initconout();
    }
    if (DAT_100180f0 == (HANDLE)0xffffffff) goto LAB_10011284;
    BVar2 = WriteConsoleW(DAT_100180f0,&_WCh,1,&local_14,(LPVOID)0x0);
    if (BVar2 != 0) {
      DAT_1001801c = 1;
      goto LAB_10011284;
    }
    if ((DAT_1001801c != 2) || (DVar3 = GetLastError(), DVar3 != 0x78)) goto LAB_10011284;
    DAT_1001801c = 0;
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
  if (DAT_100180f0 != (HANDLE)0xffffffff) {
    WriteConsoleA(DAT_100180f0,local_10,DVar3,&local_14,(LPVOID)0x0);
  }
LAB_10011284:
  wVar1 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return wVar1;
}


// 100114c3 ___set_fpsr_sse2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___set_fpsr_sse2
   
   Library: Visual Studio 2005 Release */

void __cdecl ___set_fpsr_sse2(uint param_1)

{
  if (DAT_1001dd3c != 0) {
    if (((param_1 & 0x40) == 0) || (DAT_100180e8 == 0)) {
      MXCSR = param_1 & 0xffffffbf;
    }
    else {
      MXCSR = param_1;
    }
  }
  return;
}


// 10011535 ___initconout

/* Library Function - Single Match
    ___initconout
   
   Library: Visual Studio 2005 Release */

void __cdecl ___initconout(void)

{
  DAT_100180f0 = CreateFileA("CONOUT$",0x40000000,3,(LPSECURITY_ATTRIBUTES)0x0,3,0,(HANDLE)0x0);
  return;
}


// 10011750 ___ascii_strnicmp

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
      if (bVar2 != (byte)uVar3) goto LAB_100117a1;
      _MaxCount = _MaxCount - 1;
    } while (_MaxCount != 0);
    _MaxCount = 0;
    bVar2 = (byte)(uVar3 >> 8);
    bVar5 = bVar2 < (byte)uVar3;
    if (bVar2 != (byte)uVar3) {
LAB_100117a1:
      _MaxCount = 0xffffffff;
      if (!bVar5) {
        _MaxCount = 1;
      }
    }
  }
  return _MaxCount;
}


// 100117b1 FUN_100117b1

undefined4 __cdecl FUN_100117b1(uint param_1)

{
  intptr_t iVar1;
  intptr_t iVar2;
  HANDLE hObject;
  BOOL BVar3;
  DWORD DVar4;
  undefined4 uVar5;
  
  iVar1 = __get_osfhandle(param_1);
  if (iVar1 != -1) {
    if (((param_1 == 1) && ((*(byte *)(DAT_1001dc20 + 0x74) & 1) != 0)) ||
       ((param_1 == 2 && ((*(byte *)(DAT_1001dc20 + 0x3c) & 1) != 0)))) {
      iVar1 = __get_osfhandle(2);
      iVar2 = __get_osfhandle(1);
      if (iVar2 == iVar1) goto LAB_10011810;
    }
    hObject = (HANDLE)__get_osfhandle(param_1);
    BVar3 = CloseHandle(hObject);
    if (BVar3 == 0) {
      DVar4 = GetLastError();
      goto LAB_10011812;
    }
  }
LAB_10011810:
  DVar4 = 0;
LAB_10011812:
  FUN_10011027(param_1);
  *(undefined1 *)((&DAT_1001dc20)[(int)param_1 >> 5] + 4 + (param_1 & 0x1f) * 0x38) = 0;
  if (DVar4 == 0) {
    uVar5 = 0;
  }
  else {
    FUN_1000ac76(DVar4);
    uVar5 = 0xffffffff;
  }
  return uVar5;
}


// 10011950 FUN_10011950

void FUN_10011950(void)

{
  FUN_10005c50(&DAT_10019f30,0x100123c0,"1",0,
               "Automatically set to 1 if an xcontroller has been detected.");
  _atexit((_func_4879 *)&LAB_10011b00);
  return;
}


