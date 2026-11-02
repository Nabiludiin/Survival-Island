// 10001000 FUN_10001000

undefined4 __thiscall FUN_10001000(void *this,undefined4 *param_1)

{
  int iVar1;
  undefined4 uVar2;
  char cVar3;
  
  iVar1 = *(int *)((int)this + 4);
                    /* WARNING: Load size is inaccurate */
  uVar2 = *this;
  *param_1 = uVar2;
  cVar3 = ThreadInterlockedAssignIf64(this,param_1,iVar1 + 0x10001,uVar2,iVar1);
  while (cVar3 == '\0') {
    iVar1 = *(int *)((int)this + 4);
                    /* WARNING: Load size is inaccurate */
    uVar2 = *this;
    *param_1 = uVar2;
    cVar3 = ThreadInterlockedAssignIf64(this,param_1,iVar1 + 0x10001,uVar2,iVar1);
  }
  return uVar2;
}


// 10001060 FUN_10001060

undefined4 * __fastcall FUN_10001060(undefined4 *param_1)

{
  undefined4 *puVar1;
  int iVar2;
  char cVar3;
  
  puVar1 = (undefined4 *)*param_1;
  iVar2 = param_1[1];
  while( true ) {
    if (puVar1 == (undefined4 *)0x0) {
      return (undefined4 *)0x0;
    }
    cVar3 = ThreadInterlockedAssignIf64(param_1,*puVar1,iVar2 + -1,puVar1,iVar2);
    if (cVar3 != '\0') break;
    puVar1 = (undefined4 *)*param_1;
    iVar2 = param_1[1];
  }
  return puVar1;
}


// 100010a0 FUN_100010a0

int __fastcall FUN_100010a0(int *param_1)

{
  int iVar1;
  char cVar2;
  
  do {
    iVar1 = *param_1;
    if (iVar1 == 0) {
      return 0;
    }
    cVar2 = ThreadInterlockedAssignIf64(param_1,0,param_1[1] & 0xffff0000,iVar1,param_1[1]);
  } while (cVar2 == '\0');
  return iVar1;
}


// 100011d0 FUN_100011d0

void __thiscall FUN_100011d0(void *this,undefined4 param_1)

{
  code *pcVar1;
  int iVar2;
  
                    /* WARNING: Load size is inaccurate */
  pcVar1 = *(code **)(*this + 0x80);
  *(int *)((int)this + 0x9c) = *(int *)((int)this + 0x9c) + 1;
  iVar2 = (*pcVar1)(param_1);
  if (iVar2 != 0) {
    *(int *)((int)this + 0xa0) = *(int *)((int)this + 0xa0) + 1;
  }
  return;
}


// 100012d0 FUN_100012d0

void __thiscall FUN_100012d0(void *this,undefined4 param_1)

{
  int iVar1;
  undefined4 uVar2;
  
                    /* WARNING: Load size is inaccurate */
  iVar1 = **(int **)((int)this + 0xbc);
  uVar2 = (**(code **)(*this + 4))();
  (**(code **)(iVar1 + 0x38))(param_1,uVar2);
  return;
}


// 10001300 FUN_10001300

uint __thiscall FUN_10001300(void *this,undefined4 *param_1,int param_2)

{
  int iVar1;
  uint in_EAX;
  uint uVar2;
  int *piVar3;
  int *piVar4;
  int local_14;
  undefined4 uStack_10;
  undefined4 uStack_c;
  undefined4 uStack_8;
  undefined4 uStack_4;
  
  if (param_1 == (undefined4 *)0x0) {
    uVar2 = in_EAX & 0xffffff00;
  }
  else {
    if (param_2 == 0) {
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x84))(param_1[2]);
      param_1[3] = 0;
      *param_1 = 0;
      param_1[2] = 0;
      iVar1 = param_1[1];
      *(int *)((int)this + 0x8c) = *(int *)((int)this + 0x8c) - iVar1;
      *(int *)((int)this + 0x90) = *(int *)((int)this + 0x90) + -1;
      piVar3 = (int *)(*(int *)((int)this + 0xbc) + 0x5c);
      LOCK();
      *piVar3 = *piVar3 - iVar1;
      UNLOCK();
      piVar4 = (int *)(*(int *)((int)this + 0xbc) + 0x60);
      LOCK();
      *piVar4 = *piVar4 + -1;
      UNLOCK();
      return CONCAT31((int3)((uint)piVar3 >> 8),1);
    }
    if ((param_2 == 1) && (*(char *)(*(int *)((int)this + 0xbc) + 0x88) != '\0')) {
      param_2 = 2;
    }
                    /* WARNING: Load size is inaccurate */
    local_14 = param_2;
    uStack_10 = (**(code **)(*this + 4))();
    uStack_c = param_1[2];
    uStack_8 = *param_1;
    uStack_4 = param_1[1];
    uVar2 = (**(code **)**(undefined4 **)((int)this + 0xb4))(&local_14);
    if ((char)uVar2 != '\0') {
      iVar1 = param_1[1];
      *(int *)((int)this + 0x8c) = *(int *)((int)this + 0x8c) - iVar1;
      *(int *)((int)this + 0x90) = *(int *)((int)this + 0x90) + -1;
      piVar3 = (int *)(*(int *)((int)this + 0xbc) + 0x5c);
      LOCK();
      *piVar3 = *piVar3 - iVar1;
      UNLOCK();
      piVar3 = (int *)(*(int *)((int)this + 0xbc) + 0x60);
      LOCK();
      *piVar3 = *piVar3 + -1;
      UNLOCK();
      return uVar2;
    }
  }
  return uVar2;
}


// 100014b0 FUN_100014b0

int __thiscall FUN_100014b0(void *this,char param_1)

{
  int iVar1;
  
  if (*(char *)((int)this + 0x88) != '\0') {
    return 0;
  }
  *(undefined1 *)((int)this + 0x88) = 1;
  if (param_1 != '\0') {
    iVar1 = FUN_100155e0((int *)((int)this + 8));
    *(undefined1 *)((int)this + 0x88) = 0;
    return iVar1;
  }
  iVar1 = FUN_10015730((int *)((int)this + 8));
  *(undefined1 *)((int)this + 0x88) = 0;
  return iVar1;
}


// 10001540 FUN_10001540

uint __fastcall FUN_10001540(int param_1)

{
  DWORD DVar1;
  DWORD DVar2;
  
  DVar1 = GetCurrentThreadId();
  if (DVar1 != *(DWORD *)(param_1 + 0x4c)) {
    LOCK();
    DVar2 = *(DWORD *)(param_1 + 0x4c);
    if (DVar2 == 0) {
      *(DWORD *)(param_1 + 0x4c) = DVar1;
      DVar2 = 0;
    }
    UNLOCK();
    if (DVar2 != 0) {
      return DVar2 & 0xffffff00;
    }
  }
  *(int *)(param_1 + 0x50) = *(int *)(param_1 + 0x50) + 1;
  return 1;
}


// 100015a0 FUN_100015a0

int * __thiscall FUN_100015a0(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_LAB_100253d8;
  FUN_10015730((int *)this);
  *(ushort *)((int)this + 0x4a) = *(ushort *)((int)this + 0x4a) | 1;
  FUN_10015180((undefined4 *)this);
  if ((param_1 & 1) != 0) {
    FUN_1000e140(this);
  }
  return (int *)this;
}


// 100015d0 FUN_100015d0

void __thiscall FUN_100015d0(void *this,int param_1)

{
  int *piVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (0 < param_1) {
    iVar3 = *(int *)((int)this + 8);
    if ((iVar3 == 0) && (iVar3 = *(int *)((int)this + 4), iVar3 == 0)) {
      iVar3 = 2;
    }
    if (iVar3 < param_1) {
      iVar3 = iVar3 * ((iVar3 + -1 + param_1) / iVar3);
    }
    *(int *)((int)this + 4) = *(int *)((int)this + 4) + iVar3;
    puVar2 = (undefined4 *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar3 * 0x10 + 8);
    if (puVar2 == (undefined4 *)0x0) {
      Error("CUtlFixedMemory overflow!\n");
    }
                    /* WARNING: Load size is inaccurate */
    piVar1 = *this;
    *puVar2 = 0;
    puVar2[1] = iVar3;
    if (piVar1 == (int *)0x0) {
      *(undefined4 **)this = puVar2;
      return;
    }
    iVar3 = *piVar1;
    while (iVar3 != 0) {
      piVar1 = (int *)*piVar1;
      iVar3 = *piVar1;
    }
    *piVar1 = (int)puVar2;
  }
  return;
}


// 10001660 FUN_10001660

void __thiscall FUN_10001660(void *this,uint param_1)

{
  CThreadFastMutex *this_00;
  int *piVar1;
  DWORD DVar2;
  int iVar3;
  
  this_00 = (CThreadFastMutex *)((int)this + 0x30);
  DVar2 = GetCurrentThreadId();
  if (DVar2 != *(DWORD *)this_00) {
    LOCK();
    iVar3 = *(int *)this_00;
    if (iVar3 == 0) {
      *(DWORD *)this_00 = DVar2;
      iVar3 = 0;
    }
    UNLOCK();
    if (iVar3 != 0) {
      CThreadFastMutex::Lock(this_00,DVar2,0);
      goto LAB_10001698;
    }
  }
  *(int *)((int)this + 0x34) = *(int *)((int)this + 0x34) + 1;
LAB_10001698:
  FUN_10016050(this,param_1);
  piVar1 = (int *)((int)this + 0x34);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return;
}


// 100016c0 FUN_100016c0

void __thiscall FUN_100016c0(void *this,undefined4 *param_1)

{
  CThreadFastMutex *this_00;
  int *piVar1;
  DWORD DVar2;
  int iVar3;
  
  this_00 = (CThreadFastMutex *)((int)this + 0x30);
  DVar2 = GetCurrentThreadId();
  if (DVar2 != *(DWORD *)this_00) {
    LOCK();
    iVar3 = *(int *)this_00;
    if (iVar3 == 0) {
      *(DWORD *)this_00 = DVar2;
      iVar3 = 0;
    }
    UNLOCK();
    if (iVar3 != 0) {
      CThreadFastMutex::Lock(this_00,DVar2,0);
      goto LAB_100016f8;
    }
  }
  *(int *)((int)this + 0x34) = *(int *)((int)this + 0x34) + 1;
LAB_100016f8:
  FUN_10015f60(this,param_1);
  piVar1 = (int *)((int)this + 0x34);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return;
}


// 10001720 FUN_10001720

void __fastcall FUN_10001720(undefined4 *param_1)

{
  int *piVar1;
  undefined4 *puVar2;
  int iVar3;
  char cVar4;
  
  *param_1 = &PTR_LAB_10025414;
  piVar1 = param_1 + 0x34;
  while( true ) {
    puVar2 = (undefined4 *)*piVar1;
    iVar3 = param_1[0x35];
    while( true ) {
      if (puVar2 == (undefined4 *)0x0) goto LAB_1000175e;
      cVar4 = ThreadInterlockedAssignIf64(piVar1,*puVar2,iVar3 + -1,puVar2,iVar3);
      if (cVar4 != '\0') break;
      puVar2 = (undefined4 *)*piVar1;
      iVar3 = param_1[0x35];
    }
    if (puVar2 == (undefined4 *)0x0) break;
    FUN_1000e140(puVar2);
  }
LAB_1000175e:
  FUN_100010a0(piVar1);
  return;
}


// 100017b0 FUN_100017b0

void __thiscall FUN_100017b0(void *this,char param_1)

{
  char *pcVar1;
  uint uVar2;
  
  if ((param_1 != '\0') && (*(int *)(DAT_10037434 + 0x30) != 0)) {
    if (((DAT_1003731c & 0x1000) == 0) &&
       ((*(char **)(DAT_10037324 + 0x24) == (char *)0x0 ||
        (**(char **)(DAT_10037324 + 0x24) == '\0')))) {
      (**(code **)(**(int **)((int)this + 0xbc) + 0x34))(1,1);
      return;
    }
    if ((DAT_1003731c & 0x1000) == 0) {
      pcVar1 = *(char **)(DAT_10037324 + 0x24);
      if ((byte *)pcVar1 == (byte *)0x0) {
        pcVar1 = &DAT_1002536c;
      }
    }
    else {
      pcVar1 = "FCVAR_NEVER_AS_STRING";
    }
    uVar2 = FUN_10016260((byte *)((int)this + 0xc0),(byte *)pcVar1);
    if (uVar2 == 0) {
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x5c))(1,1);
    }
  }
  return;
}


// 10001900 FUN_10001900

void FUN_10001900(undefined4 *param_1)

{
  if ((void *)param_1[3] != (void *)0x0) {
    FUN_10001300((void *)param_1[3],param_1,1);
  }
  FUN_100016c0(&DAT_10037350,param_1);
  return;
}


// 10001940 FUN_10001940

undefined4 * __cdecl FUN_10001940(undefined4 *param_1,char *param_2)

{
  char *pcVar1;
  undefined4 *puVar2;
  int iVar3;
  
  puVar2 = param_1;
  pcVar1 = (char *)((int)param_1 + 5);
  *param_1 = &PTR_LAB_100254a0;
  *(undefined1 *)(param_1 + 1) = 1;
  param_1 = (undefined4 *)((uint)param_1 & 0xffffff00);
  FUN_100166f0(pcVar1,0xff,param_2,&stack0x0000000c,(int)&param_1);
  *(undefined1 *)(puVar2 + 0x41) = 0;
  if ((((char)param_1 != '\0') && (*(char *)(puVar2 + 1) == '\0')) && (DAT_10037460 < 5)) {
    DAT_10037460 = DAT_10037460 + 1;
  }
  iVar3 = FUN_100161d0(pcVar1);
  puVar2[0x42] = iVar3;
  return puVar2;
}


// 100019b0 FUN_100019b0

void __thiscall FUN_100019b0(void *this,int param_1,int param_2)

{
  int *piVar1;
  int *piVar2;
  
                    /* WARNING: Load size is inaccurate */
  piVar2 = *this;
  if (piVar2 != (int *)0x0) {
    do {
      piVar1 = (int *)*piVar2;
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar2);
      piVar2 = piVar1;
    } while (piVar1 != (int *)0x0);
    *(undefined4 *)this = 0;
    *(undefined4 *)((int)this + 4) = 0;
  }
  *(int *)((int)this + 8) = param_1;
  FUN_100015d0(this,param_2);
  return;
}


// 10001a00 FUN_10001a00

void __thiscall FUN_10001a00(void *this,int *param_1)

{
  int local_8;
  int local_4;
  
                    /* WARNING: Load size is inaccurate */
  local_8 = *this;
  if (local_8 == 0) {
    local_8 = 0;
    local_4 = -1;
  }
  else {
    local_4 = 0;
  }
  *param_1 = local_8;
  param_1[1] = local_4;
  return;
}


// 10001a40 FUN_10001a40

undefined2 __thiscall FUN_10001a40(void *this,int param_1)

{
  int iVar1;
  undefined2 *puVar2;
  short sVar3;
  uint uVar4;
  undefined2 uVar5;
  int iVar6;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar4 = param_1 - 1U & 0xffff;
  uVar5 = 0;
  if ((int)uVar4 < *(int *)((int)this + 0x3c)) {
    iVar1 = uVar4 * 0xc;
    iVar6 = *(int *)((int)this + 0xc) + iVar1;
    sVar3 = (short)(param_1 - 1U);
    if ((((*(short *)(iVar6 + 8) != sVar3) || (*(short *)(iVar6 + 10) == sVar3)) &&
        (puVar2 = (undefined2 *)(iVar1 + *(int *)((int)this + 0xc)),
        puVar2[1] == (short)((uint)param_1 >> 0x10))) && (sVar3 != -1)) {
      uVar5 = *puVar2;
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return uVar5;
}


// 10001ab0 FUN_10001ab0

undefined4 * __thiscall FUN_10001ab0(void *this,int param_1,undefined4 param_2,char *param_3)

{
  code *pcVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  int iVar4;
  
  *(int *)((int)this + 8) = param_1 + 8;
  iVar4 = 0;
  *(undefined ***)this = &PTR_LAB_10025414;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined4 *)((int)this + 0xa4) = 0xffffffff;
  *(undefined4 *)((int)this + 0xa8) = 0xffffffff;
  *(undefined4 *)((int)this + 0xac) = 0;
  *(undefined4 *)((int)this + 0xb0) = 0;
  puVar2 = (undefined4 *)((int)this + 0xd0);
  *(undefined4 *)((int)this + 0xb4) = param_2;
  *(undefined4 *)((int)this + 0xb8) = 0;
  *(int *)((int)this + 0xbc) = param_1;
  if (((uint)puVar2 & 7) != 0) {
    Error("CTSListBase: Misaligned list\n");
    pcVar1 = (code *)swi(3);
    puVar2 = (undefined4 *)(*pcVar1)();
    return puVar2;
  }
  *puVar2 = 0;
  *(undefined4 *)((int)this + 0xd4) = 0;
  *(undefined4 *)((int)this + 0xd8) = *(undefined4 *)(param_1 + 0x8c);
  _memset((void *)((int)this + 0xc),0,0x80);
  *(undefined4 *)((int)this + 0x8c) = 0;
  *(undefined4 *)((int)this + 0x90) = 0;
  *(undefined4 *)((int)this + 0x94) = 0;
  *(undefined4 *)((int)this + 0x98) = 0;
  *(undefined4 *)((int)this + 0x9c) = 0;
  *(undefined4 *)((int)this + 0xa0) = 0;
  FUN_10016660((char *)((int)this + 0xc0),param_3,0x10);
  do {
    puVar3 = (undefined4 *)FUN_1000dfa0(0xc);
    puVar3[2] = iVar4;
    FUN_10001000(puVar2,puVar3);
    iVar4 = iVar4 + 1;
  } while (iVar4 < 6);
  return (undefined4 *)this;
}


// 10001dd0 FUN_10001dd0

undefined4 __thiscall FUN_10001dd0(void *this,int param_1,char param_2)

{
  CThreadFastMutex *pCVar1;
  CThreadFastMutex *this_00;
  char cVar2;
  undefined4 uVar3;
  DWORD DVar4;
  int iVar5;
  undefined4 *puVar6;
  
  if (param_1 == 0) {
    return 0;
  }
                    /* WARNING: Load size is inaccurate */
  if ((param_2 != '\0') && (cVar2 = (**(code **)(*this + 0x3c))(), cVar2 != '\0')) {
                    /* WARNING: Load size is inaccurate */
    uVar3 = (**(code **)(*this + 0x40))(param_1);
    return uVar3;
  }
  this_00 = *(CThreadFastMutex **)((int)this + 0xd8);
  DVar4 = GetCurrentThreadId();
  if (DVar4 != *(DWORD *)this_00) {
    LOCK();
    iVar5 = *(int *)this_00;
    if (iVar5 == 0) {
      *(DWORD *)this_00 = DVar4;
      iVar5 = 0;
    }
    UNLOCK();
    if (iVar5 != 0) {
      CThreadFastMutex::Lock(this_00,DVar4,0);
      goto LAB_10001e3b;
    }
  }
  *(int *)(this_00 + 4) = *(int *)(this_00 + 4) + 1;
LAB_10001e3b:
  puVar6 = (undefined4 *)FUN_10014970(*(void **)((int)this + 8),param_1);
  if (puVar6 == (undefined4 *)0x0) {
    pCVar1 = this_00 + 4;
    *(int *)pCVar1 = *(int *)pCVar1 + -1;
    if (*(int *)pCVar1 == 0) {
      LOCK();
      *(undefined4 *)this_00 = 0;
      UNLOCK();
    }
    return 0;
  }
  uVar3 = *puVar6;
  pCVar1 = this_00 + 4;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
    return uVar3;
  }
  return uVar3;
}


// 10001ef0 FUN_10001ef0

bool __fastcall FUN_10001ef0(int param_1)

{
  void *pvVar1;
  
  pvVar1 = GenericThreadLocals::CThreadLocalBase::Get((CThreadLocalBase *)g_nThreadID_exref);
  return *(int *)(param_1 + 0xc + (int)pvVar1 * 4) != 0;
}


// 10001fa0 FUN_10001fa0

int __fastcall FUN_10001fa0(int *param_1)

{
  CThreadFastMutex *pCVar1;
  int *piVar2;
  CThreadFastMutex *this;
  int iVar3;
  int iVar4;
  void *pvVar5;
  DWORD DVar6;
  int iVar7;
  
  pvVar5 = GenericThreadLocals::CThreadLocalBase::Get((CThreadLocalBase *)g_nThreadID_exref);
  piVar2 = (int *)param_1[(int)pvVar5 + 3];
  if (*piVar2 != 1) {
    iVar7 = *piVar2 + -1;
    *piVar2 = iVar7;
    return iVar7;
  }
  if (piVar2[1] == 0) goto LAB_10002044;
  this = (CThreadFastMutex *)param_1[0x36];
  DVar6 = GetCurrentThreadId();
  if (DVar6 == *(DWORD *)this) {
LAB_10001ff5:
    *(int *)(this + 4) = *(int *)(this + 4) + 1;
  }
  else {
    LOCK();
    iVar7 = *(int *)this;
    if (iVar7 == 0) {
      *(DWORD *)this = DVar6;
      iVar7 = 0;
    }
    UNLOCK();
    if (iVar7 == 0) goto LAB_10001ff5;
    CThreadFastMutex::Lock(this,DVar6,0);
  }
  iVar7 = piVar2[2];
  iVar4 = piVar2[1];
  while (iVar4 != 0) {
    iVar3 = *(int *)(iVar4 + 0x14 + iVar7 * 4);
    *(undefined4 *)(iVar4 + 0x14 + iVar7 * 4) = 0xffffffff;
    (**(code **)(*param_1 + 0x2c))(*(undefined4 *)(iVar4 + 0x10));
    iVar4 = iVar3;
  }
  pCVar1 = this + 4;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
LAB_10002044:
  FUN_10001000(param_1 + 0x34,piVar2);
  param_1[(int)pvVar5 + 3] = 0;
  return 0;
}


// 10002110 FUN_10002110

undefined4 __thiscall FUN_10002110(void *this,int param_1,int param_2)

{
  int iVar1;
  int iVar2;
  int iVar3;
  undefined4 *puVar4;
  undefined4 *puVar5;
  undefined2 uVar6;
  undefined4 *puVar7;
  uint uVar8;
  undefined2 extraout_var;
  void *pvVar9;
  undefined4 uVar10;
  int *piVar11;
  
  puVar7 = (undefined4 *)FUN_10014970((void *)(*(int *)((int)this + 0xbc) + 8),param_1);
  uVar8 = FUN_10001300(this,puVar7,param_2);
  if ((char)uVar8 == '\0') {
    return uVar8 & 0xffffff00;
  }
  uVar6 = FUN_10001a40(*(void **)((int)this + 8),param_1);
  if (CONCAT22(extraout_var,uVar6) != 0) {
    FUN_100153a0(*(void **)((int)this + 8),param_1);
    iVar1 = puVar7[1];
    *(int *)((int)this + 0x94) = *(int *)((int)this + 0x94) - iVar1;
    *(int *)((int)this + 0x98) = *(int *)((int)this + 0x98) + -1;
    piVar11 = (int *)(*(int *)((int)this + 0xbc) + 100);
    LOCK();
    *piVar11 = *piVar11 - iVar1;
    UNLOCK();
    piVar11 = (int *)(*(int *)((int)this + 0xbc) + 0x68);
    LOCK();
    *piVar11 = *piVar11 + -1;
    UNLOCK();
    *(int *)((int)this + 4) = *(int *)((int)this + 4) + 1;
  }
  pvVar9 = GenericThreadLocals::CThreadLocalBase::Get((CThreadLocalBase *)g_nThreadID_exref);
  iVar1 = *(int *)((int)this + (int)pvVar9 * 4 + 0xc);
  if (iVar1 != 0) {
    iVar2 = *(int *)(iVar1 + 8);
    iVar3 = puVar7[iVar2 + 5];
    if (iVar3 != -1) {
      puVar5 = *(undefined4 **)(iVar1 + 4);
      if (*(undefined4 **)(iVar1 + 4) == puVar7) {
        *(int *)(iVar1 + 4) = iVar3;
      }
      else {
        do {
          puVar4 = puVar5;
          if (puVar4 == (undefined4 *)0x0) goto LAB_100021d4;
          puVar5 = (undefined4 *)puVar4[iVar2 + 5];
        } while ((undefined4 *)puVar4[iVar2 + 5] != puVar7);
        puVar4[iVar2 + 5] = iVar3;
      }
LAB_100021d4:
      puVar7[iVar2 + 5] = 0xffffffff;
    }
  }
  puVar7[3] = 0;
  uVar10 = FUN_10015960(*(void **)((int)this + 8),param_1);
  return CONCAT31((int3)((uint)uVar10 >> 8),1);
}


// 10002200 FUN_10002200

void __thiscall FUN_10002200(void *this,undefined4 param_1)

{
  int iVar1;
  
  if (*(int *)((int)this + 0x80) != 0) {
    iVar1 = 0;
    do {
      (**(code **)(**(int **)(iVar1 + *(int *)((int)this + 0x74)) + 0xc))(param_1);
      iVar1 = iVar1 + 4;
    } while (*(int *)((int)this + 0x80) != 0);
  }
  return;
}


// 10002240 FUN_10002240

longlong __thiscall FUN_10002240(void *this,int param_1)

{
  void *this_00;
  CThreadFastMutex *pCVar1;
  uint uVar2;
  CThreadFastMutex *this_01;
  int *piVar3;
  float fVar4;
  undefined2 uVar5;
  DWORD DVar6;
  int iVar7;
  undefined4 *puVar8;
  undefined2 extraout_var;
  undefined4 *puVar9;
  undefined1 *puVar10;
  undefined4 uVar11;
  char *pcVar12;
  undefined4 extraout_EDX;
  undefined4 uVar13;
  char unaff_SI;
  undefined1 auStack_30c [496];
  undefined4 auStack_11c [68];
  int iStack_c;
  
  this_01 = *(CThreadFastMutex **)((int)this + 0x8c);
  DVar6 = GetCurrentThreadId();
  if (DVar6 != *(DWORD *)this_01) {
    LOCK();
    iVar7 = *(int *)this_01;
    if (iVar7 == 0) {
      *(DWORD *)this_01 = DVar6;
      iVar7 = 0;
    }
    UNLOCK();
    if (iVar7 != 0) {
      CThreadFastMutex::Lock(this_01,DVar6,0);
      goto LAB_10002281;
    }
  }
  *(int *)(this_01 + 4) = *(int *)(this_01 + 4) + 1;
LAB_10002281:
  this_00 = (void *)((int)this + 8);
  puVar8 = (undefined4 *)FUN_10014970(this_00,param_1);
  if (puVar8 == (undefined4 *)0x0) {
    pCVar1 = this_01 + 4;
    *(int *)pCVar1 = *(int *)pCVar1 + -1;
    uVar11 = 0;
    uVar13 = extraout_EDX;
    if (*(int *)pCVar1 == 0) {
      LOCK();
      uVar2 = *(uint *)this_01;
      *(undefined4 *)this_01 = 0;
      UNLOCK();
      return (ulonglong)uVar2 << 0x20;
    }
  }
  else {
    piVar3 = (int *)puVar8[3];
    auStack_30c[0] = 0;
    (**(code **)(*(int *)piVar3[0x2d] + 4))(puVar8[2],*puVar8,auStack_30c,0x1ff);
    uVar5 = FUN_10001a40(this_00,iStack_c);
    if (CONCAT22(extraout_var,uVar5) == 0) {
      puVar10 = &DAT_1002536c;
    }
    else {
      FUN_10001a40(this_00,iStack_c);
      puVar9 = FUN_10001940(auStack_11c,"Locked %d");
      puVar10 = (undefined1 *)((int)puVar9 + 5);
    }
    pcVar12 = &stack0xfffffce4;
    if (unaff_SI == '\0') {
      pcVar12 = "unknown";
    }
    uVar11 = (**(code **)(*piVar3 + 4))(puVar8[2],*puVar8,iStack_c,pcVar12,puVar10);
    fVar4 = (float)(int)puVar8[1];
    if ((int)puVar8[1] < 0) {
      fVar4 = fVar4 + 4.2949673e+09;
    }
    pcVar12 = FUN_10016750(fVar4,2,'\x01');
    uVar11 = Msg("\t%16.16s : %12s : 0x%08x, 0x%08x, 0x%08x : %s : %s\n",pcVar12,uVar11);
    pCVar1 = this_01 + 4;
    *(int *)pCVar1 = *(int *)pCVar1 + -1;
    uVar13 = *(undefined4 *)(this_01 + 4);
    if (*(int *)pCVar1 == 0) {
      LOCK();
      uVar11 = *(undefined4 *)this_01;
      *(undefined4 *)this_01 = 0;
      UNLOCK();
    }
  }
  return CONCAT44(uVar13,uVar11);
}


// 100023a0 FUN_100023a0

int __thiscall FUN_100023a0(void *this,byte *param_1)

{
  byte *pbVar1;
  uint uVar2;
  int iVar3;
  byte *pbVar4;
  
  iVar3 = 0;
  if (0 < *(int *)((int)this + 0x80)) {
    do {
      pbVar4 = param_1;
      pbVar1 = (byte *)(**(code **)(**(int **)(*(int *)((int)this + 0x74) + iVar3 * 4) + 4))();
      uVar2 = FUN_10016260(pbVar1,pbVar4);
      if (uVar2 == 0) {
        return iVar3;
      }
      iVar3 = iVar3 + 1;
    } while (iVar3 < *(int *)((int)this + 0x80));
  }
  return -1;
}


// 10002460 FUN_10002460

void __thiscall FUN_10002460(void *this,undefined4 *param_1,char param_2)

{
  uint uVar1;
  undefined4 *puVar2;
  
  FUN_10015bc0(this,param_1[1]);
  uVar1 = FUN_10015b30(this,param_2);
  puVar2 = (undefined4 *)FUN_10001660(&DAT_10037350,0x2c);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_1;
    puVar2[1] = param_1[1];
    puVar2[2] = param_1[2];
    puVar2[3] = param_1[3];
    puVar2[4] = 0xffffffff;
    puVar2[5] = 0xffffffff;
    puVar2[6] = 0xffffffff;
    puVar2[7] = 0xffffffff;
    puVar2[8] = 0xffffffff;
    puVar2[9] = 0xffffffff;
    puVar2[10] = 0xffffffff;
    FUN_10014e30(this,uVar1 & 0xffff,puVar2,puVar2[1]);
    return;
  }
  FUN_10014e30(this,uVar1 & 0xffff,0,iRam00000004);
  return;
}


// 100024f0 FUN_100024f0

undefined4 __thiscall FUN_100024f0(void *this,int param_1)

{
  if ((DAT_10037474 & 1) == 0) {
    DAT_10037474 = DAT_10037474 | 1;
    DAT_10037464 = 0xffffffff;
    DAT_10037468 = 0xffffffff;
    DAT_1003746c = 0xffffffff;
    DAT_10037470 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14 + 8);
  }
  return DAT_1003746c;
}


// 10002540 FUN_10002540

undefined4 __thiscall FUN_10002540(void *this,int param_1)

{
  if ((DAT_10037474 & 1) == 0) {
    DAT_10037474 = DAT_10037474 | 1;
    DAT_10037464 = 0xffffffff;
    DAT_10037468 = 0xffffffff;
    DAT_1003746c = 0xffffffff;
    DAT_10037470 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
  }
  return DAT_10037464;
}


// 10002590 FUN_10002590

undefined4 __thiscall FUN_10002590(void *this,int param_1)

{
  if ((DAT_10037474 & 1) == 0) {
    DAT_10037474 = DAT_10037474 | 1;
    DAT_10037464 = 0xffffffff;
    DAT_10037468 = 0xffffffff;
    DAT_1003746c = 0xffffffff;
    DAT_10037470 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14 + 4);
  }
  return DAT_10037468;
}


// 100025e0 FUN_100025e0

int * __thiscall FUN_100025e0(void *this,undefined4 param_1,int *param_2,undefined1 *param_3)

{
  int iVar1;
  int iVar2;
  char cVar3;
  int *piVar4;
  
  iVar1 = *(int *)((int)this + 0x10);
  *param_2 = -1;
  *param_3 = 0;
  piVar4 = param_2;
  while (iVar1 != -1) {
    *param_2 = iVar1;
                    /* WARNING: Load size is inaccurate */
    iVar2 = iVar1 * 0x14;
    cVar3 = (**this)(param_1,*(int *)((int)this + 4) + 0x10 + iVar2);
    if (cVar3 == '\0') {
      *param_3 = 0;
      if ((DAT_10037474 & 1) == 0) {
        DAT_10037474 = DAT_10037474 | 1;
        DAT_10037464 = -1;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
      }
      if (iVar1 == -1) {
        piVar4 = &DAT_10037464;
      }
      else {
        piVar4 = (int *)(*(int *)((int)this + 4) + iVar2);
      }
      iVar1 = piVar4[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_10037474 & 1) == 0) {
        DAT_10037474 = DAT_10037474 | 1;
        DAT_10037464 = -1;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
      }
      if (iVar1 == -1) {
        piVar4 = &DAT_10037464;
        iVar1 = DAT_10037464;
      }
      else {
        piVar4 = (int *)(*(int *)((int)this + 4) + iVar2);
        iVar1 = *(int *)(*(int *)((int)this + 4) + iVar2);
      }
    }
  }
  return piVar4;
}


// 100026d0 FUN_100026d0

int __thiscall FUN_100026d0(void *this,char param_1)

{
  int *piVar1;
  int *piVar2;
  int iVar3;
  int iVar4;
  int local_18;
  undefined4 local_14;
  int local_10;
  undefined4 local_c;
  int local_8 [2];
  
  iVar4 = *(int *)((int)this + 0x14);
  if (iVar4 == 0) {
    piVar1 = (int *)((int)this + 0x20);
    if (((*(int *)((int)this + 0x20) == 0) || (*(int *)((int)this + 0x24) < 0)) ||
       (*(int *)(*(int *)((int)this + 0x20) + 4) <= *(int *)((int)this + 0x24))) {
                    /* WARNING: Load size is inaccurate */
      local_18 = *this;
      if (local_18 == 0) {
        local_18 = 0;
        local_14 = 0xffffffff;
      }
      else {
        local_14 = 0;
      }
      local_10 = local_18;
      local_c = local_14;
      piVar2 = &local_10;
    }
    else {
      piVar2 = (int *)FUN_10005e60(&local_10,piVar1);
    }
    iVar4 = *piVar2;
    iVar3 = piVar2[1];
    if (((iVar4 == 0) || (iVar3 < 0)) || (*(int *)(iVar4 + 4) <= iVar3)) {
      FUN_100015d0(this,1);
      *(undefined4 *)((int)this + 0x28) = 0;
      if (((*piVar1 == 0) || (*(int *)((int)this + 0x24) < 0)) ||
         (*(int *)(*piVar1 + 4) <= *(int *)((int)this + 0x24))) {
        piVar2 = (int *)FUN_10001a00(this,local_8);
      }
      else {
        piVar2 = (int *)FUN_10005e60(&local_18,piVar1);
      }
      iVar4 = *piVar2;
      iVar3 = piVar2[1];
      if (((iVar4 == 0) || (iVar3 < 0)) || (*(int *)(iVar4 + 4) <= iVar3)) {
        if (DAT_1003747c < 10) {
          Warning("CUtlLinkedList overflow! (exhausted memory allocator)\n");
          DAT_1003747c = DAT_1003747c + 1;
        }
        return 0;
      }
    }
    if (iVar3 * 0x10 + 8 + iVar4 == 0) {
      if (DAT_10037478 < 10) {
        Warning("CUtlLinkedList overflow! (exhausted index range)\n");
        DAT_10037478 = DAT_10037478 + 1;
      }
      return 0;
    }
    *(int *)((int)this + 0x24) = iVar3;
    *piVar1 = iVar4;
    if (((iVar4 == 0) || (iVar3 = *(int *)((int)this + 0x24), iVar3 < 0)) ||
       (*(int *)(iVar4 + 4) <= iVar3)) {
      iVar4 = 0;
      *(int *)((int)this + 0x1c) = *(int *)((int)this + 0x1c) + 1;
    }
    else {
      *(int *)((int)this + 0x1c) = *(int *)((int)this + 0x1c) + 1;
      iVar4 = iVar3 * 0x10 + 8 + iVar4;
    }
  }
  else {
    *(undefined4 *)((int)this + 0x14) = *(undefined4 *)(iVar4 + 0xc);
  }
  if (param_1 == '\0') {
    *(int *)(iVar4 + 0xc) = iVar4;
    *(int *)(iVar4 + 8) = iVar4;
    return iVar4;
  }
  *(undefined4 *)(iVar4 + 0xc) = 0;
  *(undefined4 *)(iVar4 + 8) = 0;
  return iVar4;
}


// 10002850 FUN_10002850

int __fastcall FUN_10002850(int param_1)

{
  int iVar1;
  int iVar2;
  
  iVar1 = *(int *)(param_1 + 0x18);
  if (iVar1 == -1) {
    iVar1 = *(int *)(param_1 + 0x1c);
    if (((iVar1 == 0) || (0 < iVar1)) && (iVar2 = *(int *)(param_1 + 8), iVar1 < iVar2)) {
      iVar1 = iVar1 + 1;
      if (((iVar1 != 0) && (iVar1 < 1)) || (iVar2 <= iVar1)) {
        iVar1 = -1;
      }
    }
    else {
      iVar2 = *(int *)(param_1 + 8);
      iVar1 = (0 < iVar2) - 1;
    }
    if (((iVar1 != 0) && (iVar1 < 1)) || (iVar2 <= iVar1)) {
      FUN_1000e7e0((void *)(param_1 + 4),1);
      iVar1 = *(int *)(param_1 + 0x1c);
      if (((iVar1 == 0) || (0 < iVar1)) && (iVar2 = *(int *)(param_1 + 8), iVar1 < iVar2)) {
        iVar1 = iVar1 + 1;
        if (((iVar1 != 0) && (iVar1 < 1)) || (iVar2 <= iVar1)) {
          iVar1 = -1;
        }
      }
      else {
        iVar2 = *(int *)(param_1 + 8);
        iVar1 = (0 < iVar2) - 1;
      }
      if (((iVar1 != 0) && (iVar1 < 1)) || (iVar2 <= iVar1)) {
        Error("CUtlRBTree overflow!\n");
      }
    }
    *(int *)(param_1 + 0x1c) = iVar1;
    *(undefined4 *)(param_1 + 0x20) = *(undefined4 *)(param_1 + 4);
    return iVar1;
  }
  *(undefined4 *)(param_1 + 0x18) = *(undefined4 *)(*(int *)(param_1 + 4) + 4 + iVar1 * 0x14);
  *(int *)(param_1 + 0x20) = *(int *)(param_1 + 4);
  return iVar1;
}


// 10002930 FUN_10002930

uint FUN_10002930(undefined4 *param_1,uint *param_2)

{
  undefined4 *puVar1;
  uint uVar2;
  
  puVar1 = (undefined4 *)*param_2;
  if (((puVar1 != (undefined4 *)0x0) && (uVar2 = param_2[1], -1 < (int)uVar2)) &&
     ((int)uVar2 < (int)puVar1[1])) {
    if ((puVar1 + 2 <= param_1) && (param_1 < puVar1 + 2 + puVar1[1] * 4)) {
      return CONCAT31((int3)((uint)(puVar1 + uVar2 * 4 + 2) >> 8),
                      (int)(puVar1 + uVar2 * 4 + 2) < (int)param_1);
    }
    puVar1 = (undefined4 *)*puVar1;
    while( true ) {
      if (puVar1 == (undefined4 *)0x0) {
        return 0;
      }
      if ((puVar1 + 2 <= param_1) && (param_1 < puVar1 + 2 + puVar1[1] * 4)) break;
      puVar1 = (undefined4 *)*puVar1;
    }
    return CONCAT31((int3)((uint)puVar1 >> 8),1);
  }
  return (uint)puVar1 & 0xffffff00;
}


// 100029b0 FUN_100029b0

undefined4 __thiscall FUN_100029b0(void *this,int param_1)

{
  int *piVar1;
  int iVar2;
  
  iVar2 = FUN_100024f0(this,param_1);
  if ((DAT_10037474 & 1) == 0) {
    DAT_10037474 = DAT_10037474 | 1;
    DAT_10037464 = -1;
    DAT_10037468 = 0xffffffff;
    DAT_1003746c = 0xffffffff;
    DAT_10037470 = 1;
  }
  if (iVar2 != -1) {
    piVar1 = (int *)(*(int *)((int)this + 4) + iVar2 * 0x14);
    return CONCAT31((int3)((uint)piVar1 >> 8),*piVar1 == param_1);
  }
  return CONCAT31(0x100374,DAT_10037464 == param_1);
}


// 10002a20 FUN_10002a20

void __thiscall FUN_10002a20(void *this,int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  int *piVar3;
  int iVar4;
  undefined4 uVar5;
  
  iVar1 = FUN_10002590(this,param_1);
  if ((DAT_10037474 & 1) == 0) {
    DAT_10037474 = DAT_10037474 | 1;
    DAT_10037464 = 0xffffffff;
    DAT_10037468 = 0xffffffff;
    DAT_1003746c = 0xffffffff;
    DAT_10037470 = 1;
  }
  if (iVar1 == -1) {
    puVar2 = &DAT_10037464;
  }
  else {
    puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
  }
  *(undefined4 *)(*(int *)((int)this + 4) + 4 + param_1 * 0x14) = *puVar2;
  if ((DAT_10037474 & 1) == 0) {
    DAT_10037474 = DAT_10037474 | 1;
    DAT_10037464 = 0xffffffff;
    DAT_10037468 = 0xffffffff;
    DAT_1003746c = 0xffffffff;
    DAT_10037470 = 1;
  }
  if (iVar1 == -1) {
    piVar3 = &DAT_10037464;
  }
  else {
    piVar3 = (int *)(*(int *)((int)this + 4) + iVar1 * 0x14);
  }
  if (*piVar3 != -1) {
    iVar4 = FUN_10002540(this,iVar1);
    *(int *)(*(int *)((int)this + 4) + 8 + iVar4 * 0x14) = param_1;
  }
  if (iVar1 != -1) {
    uVar5 = FUN_100024f0(this,param_1);
    *(undefined4 *)(*(int *)((int)this + 4) + 8 + iVar1 * 0x14) = uVar5;
  }
  if (param_1 == *(int *)((int)this + 0x10)) {
    *(int *)((int)this + 0x10) = iVar1;
  }
  else {
    uVar5 = FUN_100029b0(this,param_1);
    if ((char)uVar5 == '\0') {
      iVar4 = FUN_100024f0(this,param_1);
      *(int *)(*(int *)((int)this + 4) + 4 + iVar4 * 0x14) = iVar1;
    }
    else {
      iVar4 = FUN_100024f0(this,param_1);
      *(int *)(*(int *)((int)this + 4) + iVar4 * 0x14) = iVar1;
    }
  }
  *(int *)(*(int *)((int)this + 4) + iVar1 * 0x14) = param_1;
  if (param_1 != -1) {
    *(int *)(*(int *)((int)this + 4) + 8 + param_1 * 0x14) = iVar1;
  }
  return;
}


// 10002b50 FUN_10002b50

void __thiscall FUN_10002b50(void *this,int param_1)

{
  int iVar1;
  int *piVar2;
  undefined4 uVar3;
  int iVar4;
  
  if ((DAT_10037474 & 1) == 0) {
    DAT_10037474 = DAT_10037474 | 1;
    DAT_10037464 = 0xffffffff;
    DAT_10037468 = 0xffffffff;
    DAT_1003746c = 0xffffffff;
    DAT_10037470 = 1;
  }
  if (param_1 == -1) {
    piVar2 = &DAT_10037464;
  }
  else {
    piVar2 = (int *)(*(int *)((int)this + 4) + param_1 * 0x14);
  }
  iVar1 = *piVar2;
  uVar3 = FUN_10002590(this,iVar1);
  *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14) = uVar3;
  iVar4 = FUN_10002590(this,iVar1);
  if (iVar4 != -1) {
    iVar4 = FUN_10002590(this,iVar1);
    *(int *)(*(int *)((int)this + 4) + 8 + iVar4 * 0x14) = param_1;
  }
  if (iVar1 != -1) {
    uVar3 = FUN_100024f0(this,param_1);
    *(undefined4 *)(*(int *)((int)this + 4) + 8 + iVar1 * 0x14) = uVar3;
  }
  if (param_1 == *(int *)((int)this + 0x10)) {
    *(int *)((int)this + 0x10) = iVar1;
  }
  else {
    iVar4 = FUN_100024f0(this,param_1);
    iVar4 = FUN_10002590(this,iVar4);
    if (iVar4 == param_1) {
      iVar4 = FUN_100024f0(this,param_1);
      *(int *)(*(int *)((int)this + 4) + 4 + iVar4 * 0x14) = iVar1;
    }
    else {
      iVar4 = FUN_100024f0(this,param_1);
      *(int *)(*(int *)((int)this + 4) + iVar4 * 0x14) = iVar1;
    }
  }
  *(int *)(*(int *)((int)this + 4) + 4 + iVar1 * 0x14) = param_1;
  if (param_1 != -1) {
    *(int *)(*(int *)((int)this + 4) + 8 + param_1 * 0x14) = iVar1;
  }
  return;
}


// 10002c50 FUN_10002c50

uint __thiscall FUN_10002c50(void *this,undefined4 param_1,undefined4 param_2,int *param_3)

{
  undefined2 uVar1;
  int iVar2;
  uint uVar3;
  int iVar4;
  undefined2 extraout_var;
  int *piVar5;
  uint unaff_retaddr;
  
  FUN_100017b0(this,'\x01');
  if ((*(byte *)((int)this + 0xb8) & 8) != 0) {
                    /* WARNING: Load size is inaccurate */
    iVar2 = (**(code **)(*this + 0x1c))(param_1);
    if (iVar2 != 0) {
      uVar3 = Error("Duplicate add to data cache\n");
      return uVar3 & 0xffffff00;
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0x14))(param_3,1);
  iVar2 = FUN_10002460(*(void **)((int)this + 8),(undefined4 *)&stack0xffffffe8,'\x01');
  iVar4 = FUN_10014970((void *)(*(int *)((int)this + 0xbc) + 8),iVar2);
  *(int *)(iVar4 + 0x10) = iVar2;
  if (param_3 != (int *)0x0) {
    *param_3 = iVar2;
  }
  *(int *)((int)this + 0x8c) = *(int *)((int)this + 0x8c) + (int)param_3;
  *(int *)((int)this + 0x90) = *(int *)((int)this + 0x90) + 1;
  piVar5 = (int *)(*(int *)((int)this + 0xbc) + 0x5c);
  LOCK();
  *piVar5 = *piVar5 + (int)param_3;
  UNLOCK();
  piVar5 = (int *)(*(int *)((int)this + 0xbc) + 0x60);
  LOCK();
  *piVar5 = *piVar5 + 1;
  UNLOCK();
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0x7c))(param_1,iVar2);
  DAT_10037260 = DAT_10037260 + 1;
  if ((unaff_retaddr & 1) != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x28))(iVar2);
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0x40))(iVar2);
  DAT_10037260 = DAT_10037260 + -1;
  uVar1 = FUN_10015250(*(void **)((int)this + 8),iVar2);
  return CONCAT31((int3)(CONCAT22(extraout_var,uVar1) >> 8),1);
}


// 10002d60 FUN_10002d60

undefined4 __thiscall
FUN_10002d60(void *this,int param_1,undefined4 *param_2,undefined4 *param_3,char param_4)

{
  CThreadFastMutex *pCVar1;
  CThreadFastMutex *this_00;
  undefined2 uVar2;
  short extraout_var;
  DWORD DVar3;
  int iVar4;
  undefined4 *puVar5;
  
  if (param_1 == 0) {
    return 1;
  }
  uVar2 = FUN_10001a40(*(void **)((int)this + 8),param_1);
  if (CONCAT22(extraout_var,uVar2) != 0 && -1 < extraout_var) {
    return 2;
  }
  this_00 = *(CThreadFastMutex **)((int)this + 0xd8);
  DVar3 = GetCurrentThreadId();
  if (DVar3 != *(DWORD *)this_00) {
    LOCK();
    iVar4 = *(int *)this_00;
    if (iVar4 == 0) {
      *(DWORD *)this_00 = DVar3;
      iVar4 = 0;
    }
    UNLOCK();
    if (iVar4 != 0) {
      CThreadFastMutex::Lock(this_00,DVar3,0);
      goto LAB_10002dc1;
    }
  }
  *(int *)(this_00 + 4) = *(int *)(this_00 + 4) + 1;
LAB_10002dc1:
  puVar5 = (undefined4 *)FUN_10014970((void *)(*(int *)((int)this + 0xbc) + 8),param_1);
  if (puVar5 == (undefined4 *)0x0) {
    pCVar1 = this_00 + 4;
    *(int *)pCVar1 = *(int *)pCVar1 + -1;
    if (*(int *)pCVar1 == 0) {
      LOCK();
      *(undefined4 *)this_00 = 0;
      UNLOCK();
    }
    return 1;
  }
  if (param_2 != (undefined4 *)0x0) {
    *param_2 = *puVar5;
  }
  if (param_3 != (undefined4 *)0x0) {
    *param_3 = puVar5[1];
  }
  FUN_10002110(this,param_1,-(uint)(param_4 != '\0') & 3);
  pCVar1 = this_00 + 4;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return 0;
}


// 10002e30 FUN_10002e30

void __fastcall FUN_10002e30(int *param_1)

{
  (**(code **)(*param_1 + 0x34))(0,0);
  if ((DAT_10038854 != 0) && ((char)param_1[1] != '\0')) {
    FUN_10016af0();
    return;
  }
  return;
}


// 10002f00 FUN_10002f00

uint __thiscall FUN_10002f00(void *this,int param_1)

{
  ushort uVar1;
  short sVar2;
  uint uVar3;
  int iVar4;
  
  if (param_1 == -1) {
    return 0xffffffff;
  }
  uVar3 = param_1 - 1U & 0xffff;
  if ((int)uVar3 < *(int *)((int)this + 0x3c)) {
    iVar4 = *(int *)((int)this + 0xc) + uVar3 * 0xc;
    sVar2 = (short)(param_1 - 1U);
    if (((*(short *)(iVar4 + 8) != sVar2) || (*(short *)(iVar4 + 10) == sVar2)) &&
       (*(short *)(*(int *)((int)this + 0xc) + 2 + uVar3 * 0xc) == (short)((uint)param_1 >> 0x10)))
    goto LAB_10002f4e;
  }
  uVar3 = 0xffff;
LAB_10002f4e:
  uVar1 = *(ushort *)(*(int *)((int)this + 0xc) + 10 + uVar3 * 0xc);
  if (uVar1 == 0xffff) {
    return 0xffffffff;
  }
  uVar3 = FUN_100149e0(this,(uint)uVar1);
  return uVar3;
}


// 10002f70 FUN_10002f70

void __fastcall FUN_10002f70(int *param_1)

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


// 10002fe0 FUN_10002fe0

void __fastcall FUN_10002fe0(int param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  uint uVar4;
  
  iVar1 = *(int *)(param_1 + 0x10);
  uVar4 = DAT_10037474;
  while( true ) {
    iVar2 = iVar1;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_10037464 = -1;
      DAT_10037468 = 0xffffffff;
      DAT_1003746c = 0xffffffff;
      DAT_10037470 = 1;
      DAT_10037474 = uVar4;
    }
    if (iVar2 == -1) {
      piVar3 = &DAT_10037464;
    }
    else {
      piVar3 = (int *)(*(int *)(param_1 + 4) + iVar2 * 0x14);
    }
    if (*piVar3 == -1) break;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_10037464 = -1;
      DAT_10037468 = 0xffffffff;
      DAT_1003746c = 0xffffffff;
      DAT_10037470 = 1;
      DAT_10037474 = uVar4;
    }
    iVar1 = DAT_10037464;
    if (iVar2 != -1) {
      iVar1 = *(int *)(*(int *)(param_1 + 4) + iVar2 * 0x14);
    }
  }
  return;
}


// 10003080 FUN_10003080

void __thiscall FUN_10003080(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  undefined4 *puVar4;
  uint uVar5;
  
  iVar1 = FUN_10002590(this,param_1);
  if (iVar1 == -1) {
    iVar1 = FUN_100024f0(this,param_1);
    uVar5 = DAT_10037474;
    while( true ) {
      iVar2 = iVar1;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037464 = -1;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
        DAT_10037474 = uVar5;
      }
      if (param_1 == -1) {
        puVar4 = &DAT_10037464;
      }
      else {
        puVar4 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
      }
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037464 = -1;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
        DAT_10037474 = uVar5;
      }
      if (puVar4[2] == -1) {
        puVar4 = &DAT_10037464;
      }
      else {
        puVar4 = (undefined4 *)(*(int *)((int)this + 4) + puVar4[2] * 0x14);
      }
      if ((puVar4[1] != param_1) || (iVar2 == -1)) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037464 = -1;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
        DAT_10037474 = uVar5;
      }
      iVar1 = *(int *)(*(int *)((int)this + 4) + iVar2 * 0x14 + 8);
      param_1 = iVar2;
    }
  }
  else {
    iVar1 = FUN_10002590(this,param_1);
    uVar5 = DAT_10037474;
    while( true ) {
      iVar2 = iVar1;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037464 = -1;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
        DAT_10037474 = uVar5;
      }
      if (iVar2 == -1) {
        piVar3 = &DAT_10037464;
      }
      else {
        piVar3 = (int *)(*(int *)((int)this + 4) + iVar2 * 0x14);
      }
      if (*piVar3 == -1) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037464 = -1;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
        DAT_10037474 = uVar5;
      }
      iVar1 = DAT_10037464;
      if (iVar2 != -1) {
        iVar1 = *(int *)(*(int *)((int)this + 4) + iVar2 * 0x14);
      }
    }
  }
  return;
}


// 10003210 FUN_10003210

int __thiscall FUN_10003210(void *this,int param_1,undefined4 *param_2)

{
  void *pvVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10014830(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar3 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar3) {
    pvVar1 = (void *)(*this + param_1 * 4);
    FUN_10016630((void *)((int)pvVar1 + 4),pvVar1,iVar3 * 4);
  }
                    /* WARNING: Load size is inaccurate */
  puVar2 = (undefined4 *)(*this + param_1 * 4);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_2;
  }
  return param_1;
}


// 10003280 FUN_10003280

void __fastcall FUN_10003280(int *param_1)

{
  undefined4 *puVar1;
  int iVar2;
  int iVar3;
  uint uVar4;
  int *piVar5;
  int local_18;
  int local_14;
  int local_10 [3];
  
  if ((param_1[8] == 0) && (param_1[9] == -1)) {
    return;
  }
  if (*param_1 == 0) {
    local_10[0] = 0;
    local_10[1] = 0xffffffff;
  }
  else {
    local_10[0] = *param_1;
    local_10[1] = 0;
  }
  piVar5 = local_10;
  do {
    iVar2 = *piVar5;
    iVar3 = piVar5[1];
    local_18 = iVar2;
    local_14 = iVar3;
    if (iVar2 == 0) {
      if (iVar3 == -1) {
LAB_1000333a:
        param_1[3] = 0;
        param_1[4] = 0;
        param_1[6] = 0;
        return;
      }
    }
    else if (((((-1 < iVar3) && (iVar3 < *(int *)(iVar2 + 4))) &&
              (puVar1 = (undefined4 *)(iVar3 * 0x10 + 8 + iVar2), puVar1 != (undefined4 *)0x0)) &&
             (uVar4 = FUN_10002930(puVar1,(uint *)(param_1 + 8)), (char)uVar4 == '\0')) &&
            (((undefined4 *)puVar1[2] != puVar1 || ((undefined4 *)puVar1[3] == puVar1)))) {
      puVar1[2] = puVar1;
      puVar1[3] = param_1[5];
      param_1[5] = (int)puVar1;
    }
    if ((iVar2 == param_1[8]) && (iVar3 == param_1[9])) goto LAB_1000333a;
    piVar5 = (int *)FUN_10005e60(local_10,&local_18);
  } while( true );
}


// 10003350 FUN_10003350

void __fastcall FUN_10003350(void *param_1)

{
  int iVar1;
  int iVar2;
  
  iVar2 = *(int *)((int)param_1 + 0x1c);
  if (iVar2 != -1) {
    iVar1 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (iVar1 != -1) {
      if ((((iVar1 == 0) || (0 < iVar1)) && (iVar1 < *(int *)((int)param_1 + 8))) &&
         ((iVar1 <= iVar2 && (iVar2 = FUN_10002540(param_1,iVar1), iVar2 != iVar1)))) {
        *(undefined4 *)(iVar1 * 0x14 + 4 + *(int *)((int)param_1 + 4)) =
             *(undefined4 *)((int)param_1 + 0x18);
        *(int *)(iVar1 * 0x14 + *(int *)((int)param_1 + 4)) = iVar1;
        *(int *)((int)param_1 + 0x18) = iVar1;
      }
      iVar2 = *(int *)((int)param_1 + 0x1c);
      if (iVar1 == iVar2) break;
      iVar1 = iVar1 + 1;
      if (((iVar1 != 0) && (iVar1 < 1)) || (*(int *)((int)param_1 + 8) <= iVar1)) {
        iVar1 = -1;
      }
    }
    *(undefined4 *)((int)param_1 + 0x10) = 0xffffffff;
    *(undefined4 *)((int)param_1 + 0x14) = 0;
  }
  return;
}


// 100033e0 FUN_100033e0

void __thiscall FUN_100033e0(void *this,int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 uVar3;
  int *piVar4;
  int iVar5;
  int iVar6;
  
  if (param_1 != *(int *)((int)this + 0x10)) {
    do {
      if ((DAT_10037474 & 1) == 0) {
        DAT_10037474 = DAT_10037474 | 1;
        DAT_10037464 = 0xffffffff;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
      }
      if (param_1 == -1) {
        puVar2 = &DAT_10037464;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
      }
      if ((DAT_10037474 & 1) == 0) {
        DAT_10037474 = DAT_10037474 | 1;
        DAT_10037464 = 0xffffffff;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
      }
      if (puVar2[2] == -1) {
        puVar2 = &DAT_10037464;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + puVar2[2] * 0x14);
      }
      if (puVar2[3] != 0) break;
      if ((DAT_10037474 & 1) == 0) {
        DAT_10037474 = DAT_10037474 | 1;
        DAT_10037464 = 0xffffffff;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
      }
      if (param_1 == -1) {
        puVar2 = &DAT_10037464;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
      }
      iVar1 = puVar2[2];
      if ((DAT_10037474 & 1) == 0) {
        DAT_10037474 = DAT_10037474 | 1;
        DAT_10037464 = 0xffffffff;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
      }
      if (iVar1 == -1) {
        puVar2 = &DAT_10037464;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
      }
      iVar5 = puVar2[2];
      if ((DAT_10037474 & 1) == 0) {
        DAT_10037474 = DAT_10037474 | 1;
        DAT_10037464 = 0xffffffff;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
      }
      if (iVar1 == -1) {
        puVar2 = &DAT_10037464;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
      }
      if ((DAT_10037474 & 1) == 0) {
        DAT_10037474 = DAT_10037474 | 1;
        DAT_10037464 = 0xffffffff;
        DAT_10037468 = 0xffffffff;
        DAT_1003746c = 0xffffffff;
        DAT_10037470 = 1;
      }
      if (puVar2[2] == -1) {
        piVar4 = &DAT_10037464;
      }
      else {
        piVar4 = (int *)(*(int *)((int)this + 4) + puVar2[2] * 0x14);
      }
      if (*piVar4 == iVar1) {
        if ((DAT_10037474 & 1) == 0) {
          DAT_10037474 = DAT_10037474 | 1;
          DAT_10037464 = 0xffffffff;
          DAT_10037468 = 0xffffffff;
          DAT_1003746c = 0xffffffff;
          DAT_10037470 = 1;
        }
        if (iVar5 == -1) {
          puVar2 = &DAT_10037464;
        }
        else {
          puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar5 * 0x14);
        }
        iVar6 = puVar2[1];
        if ((DAT_10037474 & 1) == 0) {
          DAT_10037474 = DAT_10037474 | 1;
          DAT_10037464 = 0xffffffff;
          DAT_10037468 = 0xffffffff;
          DAT_1003746c = 0xffffffff;
          DAT_10037470 = 1;
        }
        if (iVar6 == -1) {
          puVar2 = &DAT_10037464;
        }
        else {
          puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar6 * 0x14);
        }
        if (puVar2[3] == 0) {
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar1 * 0x14) = 1;
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar6 * 0x14) = 1;
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar5 * 0x14) = 0;
          param_1 = iVar5;
        }
        else {
          if ((DAT_10037474 & 1) == 0) {
            DAT_10037474 = DAT_10037474 | 1;
            DAT_10037464 = 0xffffffff;
            DAT_10037468 = 0xffffffff;
            DAT_1003746c = 0xffffffff;
            DAT_10037470 = 1;
          }
          if (param_1 == -1) {
            puVar2 = &DAT_10037464;
          }
          else {
            puVar2 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
          }
          if ((DAT_10037474 & 1) == 0) {
            DAT_10037474 = DAT_10037474 | 1;
            DAT_10037464 = 0xffffffff;
            DAT_10037468 = 0xffffffff;
            DAT_1003746c = 0xffffffff;
            DAT_10037470 = 1;
          }
          if (puVar2[2] == -1) {
            puVar2 = &DAT_10037464;
          }
          else {
            puVar2 = (undefined4 *)(*(int *)((int)this + 4) + puVar2[2] * 0x14);
          }
          iVar6 = iVar1;
          if (puVar2[1] == param_1) {
            FUN_10002a20(this,iVar1);
            if ((DAT_10037474 & 1) == 0) {
              DAT_10037474 = DAT_10037474 | 1;
              DAT_10037464 = 0xffffffff;
              DAT_10037468 = 0xffffffff;
              DAT_1003746c = 0xffffffff;
              DAT_10037470 = 1;
            }
            if (iVar1 == -1) {
              puVar2 = &DAT_10037464;
            }
            else {
              puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
            }
            iVar6 = puVar2[2];
            if ((DAT_10037474 & 1) == 0) {
              DAT_10037474 = DAT_10037474 | 1;
              DAT_10037464 = 0xffffffff;
              DAT_10037468 = 0xffffffff;
              DAT_1003746c = 0xffffffff;
              DAT_10037470 = 1;
            }
            if (iVar6 == -1) {
              puVar2 = &DAT_10037464;
            }
            else {
              puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar6 * 0x14);
            }
            iVar5 = puVar2[2];
            param_1 = iVar1;
          }
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar6 * 0x14) = 1;
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar5 * 0x14) = 0;
          FUN_10002b50(this,iVar5);
        }
      }
      else {
        if ((DAT_10037474 & 1) == 0) {
          DAT_10037474 = DAT_10037474 | 1;
          DAT_10037464 = 0xffffffff;
          DAT_10037468 = 0xffffffff;
          DAT_1003746c = 0xffffffff;
          DAT_10037470 = 1;
        }
        if (iVar5 == -1) {
          piVar4 = &DAT_10037464;
        }
        else {
          piVar4 = (int *)(*(int *)((int)this + 4) + iVar5 * 0x14);
        }
        iVar6 = *piVar4;
        if ((DAT_10037474 & 1) == 0) {
          DAT_10037474 = DAT_10037474 | 1;
          DAT_10037464 = 0xffffffff;
          DAT_10037468 = 0xffffffff;
          DAT_1003746c = 0xffffffff;
          DAT_10037470 = 1;
        }
        if (iVar6 == -1) {
          puVar2 = &DAT_10037464;
        }
        else {
          puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar6 * 0x14);
        }
        if (puVar2[3] == 0) {
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar1 * 0x14) = 1;
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar6 * 0x14) = 1;
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar5 * 0x14) = 0;
          param_1 = iVar5;
        }
        else {
          uVar3 = FUN_100029b0(this,param_1);
          iVar6 = iVar1;
          if ((char)uVar3 != '\0') {
            FUN_10002b50(this,iVar1);
            if ((DAT_10037474 & 1) == 0) {
              DAT_10037474 = DAT_10037474 | 1;
              DAT_10037464 = 0xffffffff;
              DAT_10037468 = 0xffffffff;
              DAT_1003746c = 0xffffffff;
              DAT_10037470 = 1;
            }
            if (iVar1 == -1) {
              puVar2 = &DAT_10037464;
            }
            else {
              puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
            }
            iVar6 = puVar2[2];
            if ((DAT_10037474 & 1) == 0) {
              DAT_10037474 = DAT_10037474 | 1;
              DAT_10037464 = 0xffffffff;
              DAT_10037468 = 0xffffffff;
              DAT_1003746c = 0xffffffff;
              DAT_10037470 = 1;
            }
            if (iVar6 == -1) {
              puVar2 = &DAT_10037464;
            }
            else {
              puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar6 * 0x14);
            }
            iVar5 = puVar2[2];
            param_1 = iVar1;
          }
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar6 * 0x14) = 1;
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar5 * 0x14) = 0;
          FUN_10002a20(this,iVar5);
        }
      }
    } while (param_1 != *(int *)((int)this + 0x10));
  }
  *(undefined4 *)(*(int *)((int)this + 4) + 0xc + *(int *)((int)this + 0x10) * 0x14) = 1;
  return;
}


// 10003920 FUN_10003920

int __thiscall FUN_10003920(void *this,int param_1,int param_2,int param_3)

{
  int iVar1;
  int iVar2;
  
  if (param_2 != 0) {
    if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + param_2) {
      FUN_10014830(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + param_2);
    }
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
                    /* WARNING: Load size is inaccurate */
    iVar2 = *this;
    iVar1 = (*(int *)((int)this + 0xc) - param_1) - param_2;
    *(int *)((int)this + 0x10) = iVar2;
    if ((0 < iVar1) && (0 < param_2)) {
      FUN_10016630((void *)(iVar2 + (param_1 + param_2) * 4),(void *)(iVar2 + param_1 * 4),iVar1 * 4
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


// 100039c0 FUN_100039c0

uint __fastcall FUN_100039c0(int param_1)

{
  ushort uVar1;
  void *pvVar2;
  uint uVar3;
  int iVar4;
  short sVar5;
  uint uVar6;
  
  pvVar2 = *(void **)(param_1 + 8);
  uVar3 = (uint)*(ushort *)
                 (*(int *)((int)pvVar2 + 0x18) + (uint)*(ushort *)((int)pvVar2 + 0x44) * 10);
  if (uVar3 != 0xffff) {
    uVar3 = FUN_100149e0(pvVar2,uVar3);
    while (uVar3 != 0xffffffff) {
      iVar4 = FUN_10014970((void *)(*(int *)(param_1 + 0xbc) + 8),uVar3);
      if (*(int *)(iVar4 + 0xc) == param_1) {
        return uVar3;
      }
      pvVar2 = *(void **)(param_1 + 8);
      uVar6 = uVar3 - 1 & 0xffff;
      if ((int)uVar6 < *(int *)((int)pvVar2 + 0x3c)) {
        iVar4 = *(int *)((int)pvVar2 + 0xc) + uVar6 * 0xc;
        sVar5 = (short)(uVar3 - 1);
        if (((*(short *)(iVar4 + 8) == sVar5) && (*(short *)(iVar4 + 10) != sVar5)) ||
           (*(short *)(uVar6 * 0xc + 2 + *(int *)((int)pvVar2 + 0xc)) != (short)(uVar3 >> 0x10)))
        goto LAB_10003a3f;
      }
      else {
LAB_10003a3f:
        uVar6 = 0xffff;
      }
      uVar1 = *(ushort *)(*(int *)((int)pvVar2 + 0xc) + 10 + uVar6 * 0xc);
      if (uVar1 == 0xffff) {
        return 0xffffffff;
      }
      uVar3 = FUN_100149e0(pvVar2,(uint)uVar1);
    }
  }
  return 0xffffffff;
}


// 10003a80 FUN_10003a80

uint __fastcall FUN_10003a80(int param_1)

{
  ushort uVar1;
  void *pvVar2;
  uint uVar3;
  int iVar4;
  short sVar5;
  uint uVar6;
  
  pvVar2 = *(void **)(param_1 + 8);
  uVar3 = (uint)*(ushort *)
                 (*(int *)((int)pvVar2 + 0x18) + (uint)*(ushort *)((int)pvVar2 + 0x46) * 10);
  if (uVar3 != 0xffff) {
    uVar3 = FUN_100149e0(pvVar2,uVar3);
    while (uVar3 != 0xffffffff) {
      iVar4 = FUN_10014970((void *)(*(int *)(param_1 + 0xbc) + 8),uVar3);
      if (*(int *)(iVar4 + 0xc) == param_1) {
        return uVar3;
      }
      pvVar2 = *(void **)(param_1 + 8);
      uVar6 = uVar3 - 1 & 0xffff;
      if ((int)uVar6 < *(int *)((int)pvVar2 + 0x3c)) {
        iVar4 = *(int *)((int)pvVar2 + 0xc) + uVar6 * 0xc;
        sVar5 = (short)(uVar3 - 1);
        if (((*(short *)(iVar4 + 8) == sVar5) && (*(short *)(iVar4 + 10) != sVar5)) ||
           (*(short *)(uVar6 * 0xc + 2 + *(int *)((int)pvVar2 + 0xc)) != (short)(uVar3 >> 0x10)))
        goto LAB_10003aff;
      }
      else {
LAB_10003aff:
        uVar6 = 0xffff;
      }
      uVar1 = *(ushort *)(*(int *)((int)pvVar2 + 0xc) + 10 + uVar6 * 0xc);
      if (uVar1 == 0xffff) {
        return 0xffffffff;
      }
      uVar3 = FUN_100149e0(pvVar2,(uint)uVar1);
    }
  }
  return 0xffffffff;
}


// 10003b40 FUN_10003b40

uint __thiscall FUN_10003b40(void *this,int param_1)

{
  ushort uVar1;
  void *this_00;
  uint uVar2;
  int iVar3;
  short sVar4;
  uint uVar5;
  
  uVar2 = FUN_10002f00(*(void **)((int)this + 8),param_1);
  do {
    if (uVar2 == 0xffffffff) {
      return 0xffffffff;
    }
    iVar3 = FUN_10014970((void *)(*(int *)((int)this + 0xbc) + 8),uVar2);
    if (*(void **)(iVar3 + 0xc) == this) {
      return uVar2;
    }
    this_00 = *(void **)((int)this + 8);
    uVar5 = uVar2 - 1 & 0xffff;
    if ((int)uVar5 < *(int *)((int)this_00 + 0x3c)) {
      iVar3 = *(int *)((int)this_00 + 0xc) + uVar5 * 0xc;
      sVar4 = (short)(uVar2 - 1);
      if (((*(short *)(iVar3 + 8) == sVar4) && (*(short *)(iVar3 + 10) != sVar4)) ||
         (*(short *)(uVar5 * 0xc + 2 + *(int *)((int)this_00 + 0xc)) != (short)(uVar2 >> 0x10)))
      goto LAB_10003baf;
    }
    else {
LAB_10003baf:
      uVar5 = 0xffff;
    }
    uVar1 = *(ushort *)(*(int *)((int)this_00 + 0xc) + 10 + uVar5 * 0xc);
    if (uVar1 == 0xffff) {
      return 0xffffffff;
    }
    uVar2 = FUN_100149e0(this_00,(uint)uVar1);
  } while( true );
}


// 10003bf0 FUN_10003bf0

undefined8 __fastcall FUN_10003bf0(int param_1)

{
  CThreadFastMutex *pCVar1;
  undefined4 uVar2;
  CThreadFastMutex *this;
  uint *puVar3;
  DWORD DVar4;
  int iVar5;
  uint uVar6;
  
  this = *(CThreadFastMutex **)(param_1 + 0xd8);
  DVar4 = GetCurrentThreadId();
  if (DVar4 != *(DWORD *)this) {
    LOCK();
    iVar5 = *(int *)this;
    if (iVar5 == 0) {
      *(DWORD *)this = DVar4;
      iVar5 = 0;
    }
    UNLOCK();
    if (iVar5 != 0) {
      CThreadFastMutex::Lock(this,DVar4,0);
      goto LAB_10003c2b;
    }
  }
  *(int *)(this + 4) = *(int *)(this + 4) + 1;
LAB_10003c2b:
  uVar6 = FUN_10017580((uint *)&stack0x00000004);
  puVar3 = *(uint **)(*(int *)(param_1 + 0xe4) + (*(uint *)(param_1 + 0xe0) & uVar6) * 4);
  do {
    if (puVar3 == (uint *)0x0) {
LAB_10003c5b:
      pCVar1 = this + 4;
      *(int *)pCVar1 = *(int *)pCVar1 + -1;
      if (*(int *)pCVar1 == 0) {
        LOCK();
        *(undefined4 *)this = 0;
        UNLOCK();
      }
      uVar6 = 0;
LAB_10003c69:
      return CONCAT44(0xffffffff,uVar6);
    }
    if (*puVar3 == uVar6) {
      if (puVar3 != (uint *)0xffffffff) {
        uVar6 = puVar3[1];
        pCVar1 = this + 4;
        *(int *)pCVar1 = *(int *)pCVar1 + -1;
        if (*(int *)pCVar1 == 0) {
          LOCK();
          uVar2 = *(undefined4 *)this;
          *(undefined4 *)this = 0;
          UNLOCK();
          return CONCAT44(uVar2,uVar6);
        }
        goto LAB_10003c69;
      }
      goto LAB_10003c5b;
    }
    puVar3 = (uint *)puVar3[3];
  } while( true );
}


// 10003ce0 FUN_10003ce0

void __fastcall FUN_10003ce0(int param_1)

{
  bool bVar1;
  
  DAT_100384d4 = 0;
  bVar1 = *(char *)(param_1 + 4) == '\0';
  if (!bVar1) {
    FUN_100144c0();
    bVar1 = *(char *)(param_1 + 4) == '\0';
    if (!bVar1) {
      FUN_100146e0();
      bVar1 = *(char *)(param_1 + 4) == '\0';
    }
  }
  if (!bVar1) {
    FUN_10017560();
    return;
  }
  return;
}


// 10003d10 FUN_10003d10

void __thiscall FUN_10003d10(void *this,undefined4 *param_1)

{
  int iVar1;
  int iVar2;
  uint uVar3;
  
  if (param_1 != (undefined4 *)0x0) {
    uVar3 = FUN_10002930(param_1,(uint *)((int)this + 0x20));
    if (((char)uVar3 == '\0') && ((undefined4 *)param_1[2] != param_1)) {
      iVar1 = param_1[2];
      iVar2 = param_1[3];
      if (iVar1 == 0) {
        *(int *)((int)this + 0xc) = iVar2;
      }
      else {
        *(int *)(iVar1 + 0xc) = iVar2;
      }
      if (iVar2 != 0) {
        *(int *)(iVar2 + 8) = iVar1;
        *(int *)((int)this + 0x18) = *(int *)((int)this + 0x18) + -1;
        param_1[3] = param_1;
        param_1[2] = param_1;
        return;
      }
      *(int *)((int)this + 0x18) = *(int *)((int)this + 0x18) + -1;
      *(int *)((int)this + 0x10) = iVar1;
      param_1[3] = param_1;
      param_1[2] = param_1;
    }
  }
  return;
}


// 10003d70 FUN_10003d70

void __fastcall FUN_10003d70(int *param_1)

{
  int *piVar1;
  int *piVar2;
  
  FUN_10003280(param_1);
  piVar2 = (int *)*param_1;
  if ((int *)*param_1 != (int *)0x0) {
    do {
      piVar1 = (int *)*piVar2;
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar2);
      piVar2 = piVar1;
    } while (piVar1 != (int *)0x0);
    *param_1 = 0;
    param_1[1] = 0;
  }
  param_1[5] = 0;
  param_1[7] = 0;
  param_1[10] = 0;
  param_1[8] = 0;
  param_1[9] = -1;
  return;
}


// 10003dc0 FUN_10003dc0

void __thiscall FUN_10003dc0(void *this,int param_1,int param_2,char param_3)

{
  undefined4 *puVar1;
  
  puVar1 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
  puVar1[2] = param_2;
  puVar1[1] = 0xffffffff;
  *puVar1 = 0xffffffff;
  puVar1[3] = 0;
  if (param_2 == -1) {
    *(int *)((int)this + 0x10) = param_1;
    FUN_100033e0(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(int *)(*(int *)((int)this + 4) + param_2 * 0x14) = param_1;
    FUN_100033e0(this,param_1);
    return;
  }
  *(int *)(*(int *)((int)this + 4) + 4 + param_2 * 0x14) = param_1;
  FUN_100033e0(this,param_1);
  return;
}


// 10003e30 FUN_10003e30

uint __thiscall FUN_10003e30(void *this,int param_1)

{
  CThreadFastMutex *pCVar1;
  CThreadFastMutex *this_00;
  DWORD DVar2;
  uint uVar3;
  int iVar4;
  
  this_00 = *(CThreadFastMutex **)((int)this + 0xd8);
  DVar2 = GetCurrentThreadId();
  if (DVar2 != *(DWORD *)this_00) {
    LOCK();
    iVar4 = *(int *)this_00;
    if (iVar4 == 0) {
      *(DWORD *)this_00 = DVar2;
      iVar4 = 0;
    }
    UNLOCK();
    if (iVar4 != 0) {
      CThreadFastMutex::Lock(this_00,DVar2,0);
      goto LAB_10003e6b;
    }
  }
  *(int *)(this_00 + 4) = *(int *)(this_00 + 4) + 1;
LAB_10003e6b:
  for (uVar3 = FUN_100039c0((int)this); uVar3 != 0xffffffff; uVar3 = FUN_10003b40(this,uVar3)) {
    iVar4 = FUN_10014970((void *)(*(int *)((int)this + 0xbc) + 8),uVar3);
    if (*(int *)(iVar4 + 8) == param_1) {
      *(int *)((int)this + 0xa0) = *(int *)((int)this + 0xa0) + 1;
      pCVar1 = this_00 + 4;
      *(int *)pCVar1 = *(int *)pCVar1 + -1;
      if (*(int *)pCVar1 == 0) {
        LOCK();
        *(undefined4 *)this_00 = 0;
        UNLOCK();
      }
      return uVar3;
    }
  }
  uVar3 = FUN_10003a80((int)this);
  while( true ) {
    if (uVar3 == 0xffffffff) {
      pCVar1 = this_00 + 4;
      *(int *)pCVar1 = *(int *)pCVar1 + -1;
      if (*(int *)pCVar1 == 0) {
        LOCK();
        *(undefined4 *)this_00 = 0;
        UNLOCK();
      }
      return 0;
    }
    iVar4 = FUN_10014970((void *)(*(int *)((int)this + 0xbc) + 8),uVar3);
    if (*(int *)(iVar4 + 8) == param_1) break;
    uVar3 = FUN_10003b40(this,uVar3);
  }
  *(int *)((int)this + 0xa0) = *(int *)((int)this + 0xa0) + 1;
  pCVar1 = this_00 + 4;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return uVar3;
}


// 10003f30 FUN_10003f30

undefined8 __thiscall FUN_10003f30(void *this,char param_1,char param_2)

{
  CThreadFastMutex *pCVar1;
  CThreadFastMutex *this_00;
  DWORD DVar2;
  uint uVar3;
  uint uVar4;
  uint uVar5;
  int iVar6;
  undefined4 uVar7;
  bool bVar8;
  
  this_00 = *(CThreadFastMutex **)((int)this + 0xd8);
  DVar2 = GetCurrentThreadId();
  if (DVar2 != *(DWORD *)this_00) {
    LOCK();
    iVar6 = *(int *)this_00;
    if (iVar6 == 0) {
      *(DWORD *)this_00 = DVar2;
      iVar6 = 0;
    }
    UNLOCK();
    if (iVar6 != 0) {
      CThreadFastMutex::Lock(this_00,DVar2,0);
      goto LAB_10003f71;
    }
  }
  *(int *)(this_00 + 4) = *(int *)(this_00 + 4) + 1;
LAB_10003f71:
  bVar8 = param_2 != '\0';
  _param_2 = 0;
  uVar3 = -(uint)bVar8 & 2;
  uVar4 = FUN_100039c0((int)this);
  while (uVar4 != 0xffffffff) {
    uVar5 = FUN_10003b40(this,uVar4);
    iVar6 = FUN_10014970((void *)(*(int *)((int)this + 0xbc) + 8),uVar4);
    iVar6 = *(int *)(iVar6 + 4);
    uVar7 = FUN_10002110(this,uVar4,uVar3);
    uVar4 = uVar5;
    if ((char)uVar7 != '\0') {
      _param_2 = _param_2 + iVar6;
    }
  }
  if (param_1 == '\0') {
    uVar4 = FUN_10003a80((int)this);
    while (uVar4 != 0xffffffff) {
      uVar5 = FUN_10003b40(this,uVar4);
      iVar6 = FUN_10014970((void *)(*(int *)((int)this + 0xbc) + 8),uVar4);
      iVar6 = *(int *)(iVar6 + 4);
      uVar7 = FUN_10002110(this,uVar4,uVar3);
      uVar4 = uVar5;
      if ((char)uVar7 != '\0') {
        _param_2 = _param_2 + iVar6;
      }
    }
  }
  pCVar1 = this_00 + 4;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return CONCAT44(*(undefined4 *)(this_00 + 4),_param_2);
}


// 10004050 FUN_10004050

int __thiscall FUN_10004050(void *this,uint param_1)

{
  CThreadFastMutex *pCVar1;
  CThreadFastMutex *this_00;
  DWORD DVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  undefined4 uVar6;
  uint uVar7;
  int iStack_8;
  
  this_00 = *(CThreadFastMutex **)((int)this + 0xd8);
  DVar2 = GetCurrentThreadId();
  if (DVar2 != *(DWORD *)this_00) {
    LOCK();
    iVar5 = *(int *)this_00;
    if (iVar5 == 0) {
      *(DWORD *)this_00 = DVar2;
      iVar5 = 0;
    }
    UNLOCK();
    if (iVar5 != 0) {
      CThreadFastMutex::Lock(this_00,DVar2,0);
      goto LAB_10004091;
    }
  }
  *(int *)(this_00 + 4) = *(int *)(this_00 + 4) + 1;
LAB_10004091:
  iStack_8 = 0;
  uVar3 = FUN_100039c0((int)this);
  while ((uVar3 != 0xffffffff && (param_1 != 0))) {
    uVar4 = FUN_10003b40(this,uVar3);
    iVar5 = FUN_10014970((void *)(*(int *)((int)this + 0xbc) + 8),uVar3);
    uVar7 = *(uint *)(iVar5 + 4);
    uVar6 = FUN_10002110(this,uVar3,2);
    uVar3 = uVar4;
    if ((char)uVar6 != '\0') {
      iStack_8 = iStack_8 + uVar7;
      if (param_1 <= uVar7) {
        uVar7 = param_1;
      }
      param_1 = param_1 - uVar7;
    }
  }
  pCVar1 = this_00 + 4;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return iStack_8;
}


// 10004120 FUN_10004120

int __thiscall FUN_10004120(void *this,int param_1)

{
  CThreadFastMutex *pCVar1;
  CThreadFastMutex *this_00;
  DWORD DVar2;
  int iVar3;
  uint uVar4;
  uint uVar5;
  undefined4 uVar6;
  int iStack_4;
  
  this_00 = *(CThreadFastMutex **)((int)this + 0xd8);
  DVar2 = GetCurrentThreadId();
  if (DVar2 != *(DWORD *)this_00) {
    LOCK();
    iVar3 = *(int *)this_00;
    if (iVar3 == 0) {
      *(DWORD *)this_00 = DVar2;
      iVar3 = 0;
    }
    UNLOCK();
    if (iVar3 != 0) {
      CThreadFastMutex::Lock(this_00,DVar2,0);
      goto LAB_1000415c;
    }
  }
  *(int *)(this_00 + 4) = *(int *)(this_00 + 4) + 1;
LAB_1000415c:
  iStack_4 = 0;
  uVar4 = FUN_100039c0((int)this);
  while ((uVar4 != 0xffffffff && (param_1 != 0))) {
    uVar5 = FUN_10003b40(this,uVar4);
    uVar6 = FUN_10002110(this,uVar4,2);
    uVar4 = uVar5;
    if ((char)uVar6 != '\0') {
      param_1 = param_1 + -1;
      iStack_4 = iStack_4 + 1;
    }
  }
  pCVar1 = this_00 + 4;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return iStack_4;
}


// 100041c0 FUN_100041c0

void __thiscall FUN_100041c0(void *this,uint param_1)

{
  uint uVar1;
  
  if ((*(int *)((int)this + 0xa8) != -1) || (*(int *)((int)this + 0xa4) != -1)) {
    uVar1 = *(int *)((int)this + 0x8c) + param_1;
    if (*(uint *)((int)this + 0xa4) < uVar1) {
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x60))(uVar1 - *(uint *)((int)this + 0xa4));
    }
    if (*(uint *)((int)this + 0xa8) <= *(uint *)((int)this + 0x90)) {
      FUN_10004120(this,(*(uint *)((int)this + 0x90) - *(uint *)((int)this + 0xa8)) + 1);
    }
  }
  FUN_10015bc0((void *)(*(int *)((int)this + 0xbc) + 8),param_1);
  return;
}


// 10004230 FUN_10004230

undefined4 * __fastcall FUN_10004230(undefined4 *param_1)

{
  *(undefined1 *)(param_1 + 1) = 1;
  *param_1 = &PTR_LAB_1002559c;
  FUN_10015de0(param_1 + 2,0xffffffff);
  param_1[2] = &PTR_LAB_100253d8;
  param_1[0x15] = 0;
  param_1[0x16] = 0;
  param_1[0x1d] = 0;
  param_1[0x1e] = 0;
  param_1[0x1f] = 0;
  param_1[0x20] = 0;
  param_1[0x21] = 0;
  param_1[0x23] = param_1 + 0x15;
  param_1[0x17] = 0;
  param_1[0x18] = 0;
  param_1[0x19] = 0;
  param_1[0x1a] = 0;
  param_1[0x1b] = 0;
  param_1[0x1c] = 0;
  *(undefined1 *)(param_1 + 0x22) = 0;
  return param_1;
}


// 100042a0 FUN_100042a0

int __fastcall FUN_100042a0(int param_1)

{
  int *this;
  
  this = (int *)(param_1 + 0x18);
  *(undefined4 *)(param_1 + 4) = 0;
  *(undefined4 *)(param_1 + 8) = 0;
  *(undefined4 *)(param_1 + 0xc) = 0;
  *(undefined4 *)(param_1 + 0x10) = 0;
  *(undefined4 *)(param_1 + 0x14) = 0;
  *this = 0;
  *(undefined4 *)(param_1 + 0x1c) = 0;
  *(undefined4 *)(param_1 + 0x20) = 0;
  FUN_100019b0(this,0,0);
  *(undefined4 *)(param_1 + 0x38) = 0;
  *(undefined4 *)(param_1 + 0x3c) = 0xffffffff;
  *(undefined4 *)(param_1 + 0x24) = 0;
  *(undefined4 *)(param_1 + 0x28) = 0;
  *(undefined4 *)(param_1 + 0x2c) = 0;
  *(undefined4 *)(param_1 + 0x30) = 0;
  *(undefined4 *)(param_1 + 0x34) = 0;
  *(undefined4 *)(param_1 + 0x40) = 0;
  *(undefined4 *)(param_1 + 0x10) = 0;
  if (-1 < *(int *)(param_1 + 0xc)) {
    if (*(int *)(param_1 + 4) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 4));
      *(undefined4 *)(param_1 + 4) = 0;
    }
    *(undefined4 *)(param_1 + 8) = 0;
  }
  *(undefined4 *)(param_1 + 0x14) = *(undefined4 *)(param_1 + 4);
  FUN_10003d70(this);
  return param_1;
}


// 10004320 FUN_10004320

uint * __thiscall FUN_10004320(void *this,uint param_1,uint *param_2)

{
  uint uVar1;
  uint uVar2;
  uint *puVar3;
  uint uVar4;
  
  puVar3 = (uint *)FUN_100026d0((void *)((int)this + 0x18),'\x01');
  if (puVar3 == (uint *)0x0) {
    return (uint *)0xffffffff;
  }
  *puVar3 = param_1;
  puVar3[1] = *param_2;
                    /* WARNING: Load size is inaccurate */
  uVar1 = *this;
  uVar2 = *(uint *)(*(int *)((int)this + 4) + (uVar1 & param_1) * 4);
  FUN_10003d10((void *)((int)this + 0x18),puVar3);
  puVar3[3] = uVar2;
  if (uVar2 == 0) {
    uVar4 = *(uint *)((int)this + 0x28);
    puVar3[2] = uVar4;
    *(uint **)((int)this + 0x28) = puVar3;
  }
  else {
    uVar4 = *(uint *)(uVar2 + 8);
    puVar3[2] = uVar4;
    *(uint **)(uVar2 + 8) = puVar3;
  }
  if (uVar4 == 0) {
    *(uint **)((int)this + 0x24) = puVar3;
  }
  else {
    *(uint **)(uVar4 + 0xc) = puVar3;
  }
  *(int *)((int)this + 0x30) = *(int *)((int)this + 0x30) + 1;
  *(uint **)(*(int *)((int)this + 4) + (uVar1 & param_1) * 4) = puVar3;
  return puVar3;
}


// 100043b0 FUN_100043b0

void __fastcall FUN_100043b0(int param_1)

{
  CThreadFastMutex *pCVar1;
  CThreadFastMutex *this;
  DWORD DVar2;
  int iVar3;
  uint uVar4;
  uint *puVar5;
  
  this = *(CThreadFastMutex **)(param_1 + 0xd8);
  DVar2 = GetCurrentThreadId();
  if (DVar2 != *(DWORD *)this) {
    LOCK();
    iVar3 = *(int *)this;
    if (iVar3 == 0) {
      *(DWORD *)this = DVar2;
      iVar3 = 0;
    }
    UNLOCK();
    if (iVar3 != 0) {
      CThreadFastMutex::Lock(this,DVar2,0);
      goto LAB_100043eb;
    }
  }
  *(int *)(this + 4) = *(int *)(this + 4) + 1;
LAB_100043eb:
  puVar5 = (uint *)&stack0x00000008;
  uVar4 = FUN_10017580((uint *)&stack0x00000004);
  FUN_10004320((void *)(param_1 + 0xe0),uVar4,puVar5);
  pCVar1 = this + 4;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return;
}


// 100044f0 FUN_100044f0

void __thiscall FUN_100044f0(void *this,int param_1,int param_2)

{
  void *pvVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  CThreadFastMutex *pCVar4;
  DWORD DVar5;
  int iVar6;
  int iVar7;
  int iVar8;
  undefined4 uVar9;
  char *pcVar10;
  char *pcVar11;
  int iVar12;
  float fVar13;
  float fVar14;
  double dVar15;
  char cStack_71;
  int iStack_70;
  void *pvStack_6c;
  float fStack_68;
  void *pvStack_64;
  CThreadFastMutex *local_60;
  undefined1 auStack_5c [4];
  int iStack_58;
  int iStack_54;
  float fStack_50;
  int iStack_4c;
  undefined4 uStack_48;
  int iStack_44;
  int iStack_40;
  int iStack_3c;
  int iStack_38;
  undefined4 uStack_34;
  int iStack_30;
  int iStack_2c;
  int iStack_28;
  undefined1 *puStack_24;
  int iStack_20;
  undefined4 uStack_1c;
  int iStack_18;
  int iStack_14;
  int iStack_10;
  undefined4 uStack_c;
  undefined4 uStack_8;
  undefined4 uStack_4;
  
  pCVar4 = *(CThreadFastMutex **)((int)this + 0x8c);
  local_60 = pCVar4;
  DVar5 = GetCurrentThreadId();
  if (DVar5 != *(DWORD *)pCVar4) {
    LOCK();
    iVar7 = *(int *)pCVar4;
    if (iVar7 == 0) {
      *(DWORD *)pCVar4 = DVar5;
      iVar7 = 0;
    }
    UNLOCK();
    if (iVar7 != 0) {
      CThreadFastMutex::Lock(pCVar4,DVar5,0);
      goto LAB_10004535;
    }
  }
  *(int *)(pCVar4 + 4) = *(int *)(pCVar4 + 4) + 1;
LAB_10004535:
  iVar7 = 0;
  pvVar1 = (void *)((int)this + 8);
  pvStack_6c = pvVar1;
  fStack_68 = (float)FUN_10014780((int)pvVar1);
  pvStack_64 = (void *)FUN_10014770((int)pvVar1);
  fStack_50 = (float)(int)fStack_68;
  iStack_38 = 0;
  uStack_34 = 0;
  fStack_68 = (fStack_50 * 100.0) / (float)(int)pvStack_64;
  iStack_30 = 0;
  iStack_2c = 0;
  iStack_28 = 0;
  iStack_4c = 0;
  uStack_48 = 0;
  iStack_44 = 0;
  iStack_40 = 0;
  iStack_3c = 0;
  FUN_10015d10(pvVar1,&iStack_4c);
  FUN_10015c40(pvVar1,&iStack_38);
  iStack_70 = 0;
                    /* WARNING: Load size is inaccurate */
  if ((param_2 == 0) || (iStack_70 = (**(code **)(*this + 0x2c))(param_2), iStack_70 != 0)) {
    iVar12 = iStack_70;
    if (param_1 == 1) {
      iVar12 = 0;
      puStack_24 = &LAB_100023f0;
      iStack_20 = 0;
      uStack_1c = 0;
      iStack_18 = 0;
      iStack_14 = -1;
      iStack_10 = 0;
      uStack_c = 0xffffffff;
      uStack_8 = 0xffffffff;
      uStack_4 = 0;
      iVar7 = iStack_70;
      if (0 < iStack_40) {
        do {
          if ((iVar7 == 0) ||
             (iVar6 = FUN_10014970(pvStack_6c,*(int *)(iStack_4c + iVar12 * 4)),
             *(int *)(iVar6 + 0xc) == iVar7)) {
            puVar2 = (undefined4 *)(iStack_4c + iVar12 * 4);
            FUN_100025e0(&puStack_24,puVar2,&iStack_58,auStack_5c);
            iVar7 = FUN_10002850((int)&puStack_24);
            FUN_10003dc0(&puStack_24,iVar7,iStack_58,auStack_5c[0]);
            iStack_10 = iStack_10 + 1;
            puVar3 = (undefined4 *)(iStack_20 + 0x10 + iVar7 * 0x14);
            iVar7 = iStack_70;
            if (puVar3 != (undefined4 *)0x0) {
              *puVar3 = *puVar2;
            }
          }
          iVar12 = iVar12 + 1;
        } while (iVar12 < iStack_40);
      }
      iStack_70 = 0;
      if (0 < iStack_2c) {
        do {
          if ((iVar7 == 0) ||
             (iVar12 = FUN_10014970(pvStack_6c,*(int *)(iStack_38 + iStack_70 * 4)),
             *(int *)(iVar12 + 0xc) == iVar7)) {
            puVar2 = (undefined4 *)(iStack_38 + iStack_70 * 4);
            FUN_100025e0(&puStack_24,puVar2,&iStack_54,&cStack_71);
            iVar8 = FUN_10002850((int)&puStack_24);
            iVar12 = iVar8 * 0x14;
            *(int *)(iVar12 + 8 + iStack_20) = iStack_54;
            *(undefined4 *)(iVar12 + 4 + iStack_20) = 0xffffffff;
            *(undefined4 *)(iVar12 + iStack_20) = 0xffffffff;
            *(undefined4 *)(iVar12 + 0xc + iStack_20) = 0;
            iVar6 = iVar8;
            if (iStack_54 != -1) {
              iVar6 = iStack_14;
              if (cStack_71 == '\0') {
                *(int *)(iStack_20 + 4 + iStack_54 * 0x14) = iVar8;
              }
              else {
                *(int *)(iStack_20 + iStack_54 * 0x14) = iVar8;
              }
            }
            iStack_14 = iVar6;
            FUN_100033e0(&puStack_24,iVar8);
            iStack_10 = iStack_10 + 1;
            puVar3 = (undefined4 *)(iVar12 + 0x10 + iStack_20);
            if (puVar3 != (undefined4 *)0x0) {
              *puVar3 = *puVar2;
            }
          }
          iStack_70 = iStack_70 + 1;
        } while (iStack_70 < iStack_2c);
      }
      iVar7 = FUN_10002fe0((int)&puStack_24);
      iVar12 = iStack_20;
      for (; iVar7 != -1; iVar7 = FUN_10003080(&puStack_24,iVar7)) {
        FUN_10002240(this,*(int *)(iVar12 + 0x10 + iVar7 * 0x14));
      }
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x38))(0,param_2);
      FUN_10003350(&puStack_24);
      iVar7 = iStack_18;
      uStack_c = 0xffffffff;
      if (-1 < iStack_18) {
        if (iStack_20 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iStack_20);
          iStack_20 = 0;
        }
        uStack_1c = 0;
      }
      uStack_8 = 0xffffffff;
      if (-1 < iVar7) {
        if (iStack_20 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iStack_20);
          iStack_20 = 0;
        }
        uStack_1c = 0;
      }
    }
    else if (param_1 == 2) {
      if (0 < iStack_40) {
        do {
          if ((iVar12 == 0) ||
             (iVar6 = FUN_10014970(pvVar1,*(int *)(iStack_4c + iVar7 * 4)),
             *(int *)(iVar6 + 0xc) == iVar12)) {
            FUN_10002240(this,*(int *)(iStack_4c + iVar7 * 4));
          }
          iVar7 = iVar7 + 1;
        } while (iVar7 < iStack_40);
      }
      iVar7 = 0;
      if (0 < iStack_2c) {
        do {
          if ((iVar12 == 0) ||
             (iVar6 = FUN_10014970(pvVar1,*(int *)(iStack_38 + iVar7 * 4)),
             *(int *)(iVar6 + 0xc) == iVar12)) {
            FUN_10002240(this,*(int *)(iStack_38 + iVar7 * 4));
          }
          iVar7 = iVar7 + 1;
        } while (iVar7 < iStack_2c);
      }
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x38))(0,param_2);
    }
    else if (param_1 == 0) {
      iVar7 = 0;
      if (param_2 == 0) {
        if (0 < *(int *)((int)this + 0x80)) {
          do {
            iVar12 = (**(code **)(**(int **)(*(int *)((int)this + 0x74) + iVar7 * 4) + 4))();
            if (iVar12 != 0) {
                    /* WARNING: Load size is inaccurate */
              iVar12 = *this;
              uVar9 = (**(code **)(**(int **)(*(int *)((int)this + 0x74) + iVar7 * 4) + 4))();
              (**(code **)(iVar12 + 0x38))(0,uVar9);
            }
            iVar7 = iVar7 + 1;
          } while (iVar7 < *(int *)((int)this + 0x80));
        }
        iVar12 = iStack_2c;
        iVar7 = iStack_40;
        dVar15 = (double)fStack_68;
        pcVar10 = FUN_10016750(fStack_50,2,'\x01');
        Msg("Summary: %i resources total %s, %.2f %% of capacity\n",iVar12 + iVar7,pcVar10,dVar15);
      }
      else {
        fVar13 = 0.0;
        iVar6 = 0;
        fStack_68 = 0.0;
        fVar14 = fStack_68;
        if (0 < iStack_40) {
          do {
            iVar8 = FUN_10014970(pvStack_6c,*(int *)(iStack_4c + iVar7 * 4));
            if (*(int *)(iVar8 + 0xc) == iVar12) {
              iVar8 = FUN_10014970(&DAT_10037390,*(int *)(iStack_4c + iVar7 * 4));
              fVar13 = (float)((int)fVar13 + *(int *)(iVar8 + 4));
              iVar6 = iVar6 + 1;
            }
            iVar7 = iVar7 + 1;
            fVar14 = fVar13;
          } while (iVar7 < iStack_40);
        }
        fStack_68 = fVar14;
        iVar7 = 0;
        fVar14 = fStack_68;
        if (0 < iStack_2c) {
          do {
            iVar8 = FUN_10014970(pvStack_6c,*(int *)(iStack_38 + iVar7 * 4));
            if (*(int *)(iVar8 + 0xc) == iVar12) {
              iVar8 = FUN_10014970(&DAT_10037390,*(int *)(iStack_38 + iVar7 * 4));
              fVar14 = (float)((int)fVar14 + *(int *)(iVar8 + 4));
              iVar6 = iVar6 + 1;
            }
            iVar7 = iVar7 + 1;
          } while (iVar7 < iStack_2c);
        }
        fStack_68 = fVar14;
        pvVar1 = *(void **)(iVar12 + 0xa4);
        pvStack_6c = (void *)0x1;
        if (pvVar1 == (void *)0xffffffff) {
          pvStack_6c = pvStack_64;
        }
        else if (pvVar1 != (void *)0x0) {
          pvStack_6c = pvVar1;
        }
        pvStack_64 = (void *)(float)(int)pvStack_6c;
        fStack_50 = (float)(int)fStack_68;
        pcVar10 = FUN_10016750((float)pvStack_64,2,'\x01');
        dVar15 = (double)((fStack_50 * 100.0) / (float)pvStack_64);
        pcVar11 = FUN_10016750(fStack_50,2,'\x01');
        Msg("Section [%s]: %i resources total %s, %.2f %% of limit (%s)\n",param_2,iVar6,pcVar11,
            dVar15,pcVar10);
      }
    }
    iStack_40 = 0;
    if (-1 < iStack_44) {
      if (iStack_4c != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iStack_4c);
        iStack_4c = 0;
      }
      uStack_48 = 0;
    }
    iStack_3c = iStack_4c;
    if (-1 < iStack_44) {
      if (iStack_4c != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iStack_4c);
        iStack_4c = 0;
      }
      uStack_48 = 0;
    }
    iStack_2c = 0;
    if (-1 < iStack_30) {
      if (iStack_38 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iStack_38);
        iStack_38 = 0;
      }
      uStack_34 = 0;
    }
    if ((-1 < iStack_30) && (iStack_38 != 0)) {
      iStack_28 = iStack_38;
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iStack_38);
    }
  }
  else {
    Msg("Unknown cache section %s\n",param_2);
    FUN_10002f70(&iStack_4c);
    FUN_10002f70(&iStack_38);
  }
  pCVar4 = local_60 + 4;
  *(int *)pCVar4 = *(int *)pCVar4 + -1;
  if (*(int *)pCVar4 == 0) {
    LOCK();
    *(undefined4 *)local_60 = 0;
    UNLOCK();
  }
  return;
}


// 10004b60 FUN_10004b60

uint __thiscall FUN_10004b60(void *this,uint param_1)

{
  int *piVar1;
  int *piVar2;
  int iVar3;
  undefined4 uVar4;
  int *piVar5;
  
  if ((param_1 & param_1 - 1) != 0) {
    return param_1 - 1 & 0xffffff00;
  }
  *(undefined4 *)((int)this + 0x10) = 0;
  FUN_10003920((int *)((int)this + 4),0,param_1,0);
  iVar3 = 0;
  if (0 < (int)param_1) {
    do {
      *(undefined4 *)(*(int *)((int)this + 4) + iVar3 * 4) = 0;
      iVar3 = iVar3 + 1;
    } while (iVar3 < (int)param_1);
  }
  piVar1 = (int *)((int)this + 0x18);
  *(uint *)this = param_1 - 1;
  uVar4 = FUN_10003280(piVar1);
  piVar5 = (int *)*piVar1;
  if ((int *)*piVar1 != (int *)0x0) {
    do {
      piVar2 = (int *)*piVar5;
      uVar4 = (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar5);
      piVar5 = piVar2;
    } while (piVar2 != (int *)0x0);
    *piVar1 = 0;
    *(undefined4 *)((int)this + 0x1c) = 0;
  }
  *(uint *)((int)this + 0x20) = param_1 * 2;
  *(undefined4 *)((int)this + 0x40) = 0;
  return CONCAT31((int3)((uint)uVar4 >> 8),1);
}


// 10004c00 FUN_10004c00

undefined4 * __thiscall FUN_10004c00(void *this,int param_1,undefined4 param_2,char *param_3)

{
  FUN_10001ab0(this,param_1,param_2,param_3);
  *(undefined ***)this = &PTR_LAB_1002567c;
  FUN_100042a0((int)this + 0xe0);
  FUN_10004b60((void *)((int)this + 0xe0),0x400);
  return (undefined4 *)this;
}


// 10004c40 FUN_10004c40

int * __thiscall FUN_10004c40(void *this,undefined4 param_1,char *param_2,int *param_3)

{
  char *pcVar1;
  int *piVar2;
  void *pvVar3;
  undefined4 unaff_retaddr;
  
  pcVar1 = param_2;
                    /* WARNING: Load size is inaccurate */
  piVar2 = (int *)(**(code **)(*this + 0x2c))(param_2);
  if (piVar2 != (int *)0x0) {
    return piVar2;
  }
  if ((char)param_3 == '\0') {
    pvVar3 = (void *)FUN_1000dfa0(0xe0);
    if (pvVar3 != (void *)0x0) {
      piVar2 = FUN_10001ab0(pvVar3,(int)this,unaff_retaddr,pcVar1);
      goto LAB_10004ca2;
    }
  }
  else {
    pvVar3 = (void *)FUN_1000dfa0(0x128);
    if (pvVar3 != (void *)0x0) {
      piVar2 = FUN_10004c00(pvVar3,(int)this,unaff_retaddr,pcVar1);
      goto LAB_10004ca2;
    }
  }
  piVar2 = (int *)0x0;
LAB_10004ca2:
  param_3 = piVar2;
  (**(code **)(*piVar2 + 8))(param_2);
  FUN_10003210((void *)((int)this + 0x74),*(int *)((int)this + 0x80),&param_2);
  return piVar2;
}


// 10004cd0 FUN_10004cd0

int __cdecl FUN_10004cd0(char *param_1)

{
  char cVar1;
  char *pcVar2;
  int iVar3;
  int iVar4;
  
  if (param_1 != (char *)0x0) {
    pcVar2 = param_1;
    do {
      cVar1 = *pcVar2;
      pcVar2 = pcVar2 + 1;
    } while (cVar1 != '\0');
    iVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(pcVar2 + (1 - (int)(param_1 + 1)));
    if (iVar3 != 0) {
      iVar4 = iVar3 - (int)param_1;
      do {
        cVar1 = *param_1;
        param_1[iVar4] = cVar1;
        param_1 = param_1 + 1;
      } while (cVar1 != '\0');
      return iVar3;
    }
  }
  return 0;
}


// 10004d60 FUN_10004d60

void __fastcall FUN_10004d60(int *param_1)

{
  param_1[3] = 0;
  param_1[4] = 0;
  *(undefined1 *)(param_1 + 5) = 0;
  param_1[8] = 0;
  param_1[7] = -1;
  FUN_100177a0(param_1);
  return;
}


// 10004d80 FUN_10004d80

float10 __cdecl FUN_10004d80(uint param_1)

{
  uint uVar1;
  float10 fVar2;
  
  if (((ushort)(param_1 >> 10) & 0x1f) == 0x1f) {
    if ((param_1 & 0x3ff) == 0) {
      if ((param_1 & 0x8000) == 0) {
        return (float10)1 * (float10)65504.0;
      }
      return (float10)-1.0 * (float10)65504.0;
    }
    if ((param_1 & 0x3ff) != 0) {
      return (float10)0;
    }
  }
  if (((param_1 >> 10 & 0x1f) == 0) && ((param_1 & 0x3ff) != 0)) {
    fVar2 = (float10)(param_1 & 0x3ff) * (float10)0.0009765625;
    if ((param_1 & 0x8000) == 0) {
      return (float10)1 * fVar2 * (float10)6.1035156e-05;
    }
    return (float10)-1.0 * fVar2 * (float10)6.1035156e-05;
  }
  uVar1 = (param_1 & 0xffff) >> 10 & 0x1f;
  return (float10)(float)((((uint)(uVar1 != 0) * (uVar1 + 0x70) * 0x80 | param_1 & 0x8000) * 8 |
                          param_1 & 0x3ff) << 0xd);
}


// 10004e50 FUN_10004e50

undefined1 __cdecl FUN_10004e50(int param_1)

{
  uint uVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  int iVar5;
  
  iVar2 = param_1;
  iVar5 = *(int *)(param_1 + 4);
  if (iVar5 == 0x31) {
    return 1;
  }
  param_1 = 1;
  if ((iVar5 < 0x2e) && (iVar4 = 0, 0 < *(int *)(iVar2 + 0xb4))) {
    do {
      if ((iVar4 < 0) || (iVar3 = iVar4, *(int *)(iVar2 + 0xb4) <= iVar4)) {
        iVar3 = 0;
      }
      iVar3 = iVar3 * 100 + iVar2 + *(int *)(iVar2 + 0xb8);
      if (*(int *)(iVar3 + 0x54) != 0) {
        _memset((undefined4 *)(iVar3 + 0x10),0,0x54);
        *(undefined4 *)(iVar3 + 0x10) = 1;
        *(undefined4 *)(iVar3 + 0x34) = 0xffffffff;
        param_1 = 0;
      }
      iVar4 = iVar4 + 1;
    } while (iVar4 < *(int *)(iVar2 + 0xb4));
  }
  if (iVar5 < 0x2f) {
    if (*(int *)(iVar2 + 0x17c) != 0) {
      *(undefined4 *)(iVar2 + 0x17c) = 0;
      param_1 = 0;
    }
    iVar4 = 0;
    if (0 < *(int *)(iVar2 + 0xb4)) {
      do {
        if ((iVar4 < 0) || (iVar3 = iVar4, *(int *)(iVar2 + 0xb4) <= iVar4)) {
          iVar3 = 0;
        }
        iVar3 = iVar3 * 100 + iVar2 + *(int *)(iVar2 + 0xb8);
        iVar4 = iVar4 + 1;
        *(undefined4 *)(iVar3 + 0x5c) = 0;
        *(undefined2 *)(iVar3 + 0x58) = 0;
      } while (iVar4 < *(int *)(iVar2 + 0xb4));
    }
LAB_10004f2d:
    if (0x30 < iVar5) goto LAB_10004f79;
  }
  else {
    if (iVar5 != 0x2f) goto LAB_10004f2d;
    iVar5 = 0;
    if (0 < *(int *)(iVar2 + 0xb4)) {
      do {
        if ((iVar5 < 0) || (iVar4 = iVar5, *(int *)(iVar2 + 0xb4) <= iVar5)) {
          iVar4 = 0;
        }
        iVar4 = iVar4 * 100 + iVar2 + *(int *)(iVar2 + 0xb8);
        if (*(int *)(iVar4 + 0x5c) != 0) {
          *(undefined4 *)(iVar4 + 0x5c) = 0;
          *(undefined2 *)(iVar4 + 0x58) = 0;
          param_1 = 0;
        }
        iVar5 = iVar5 + 1;
      } while (iVar5 < *(int *)(iVar2 + 0xb4));
    }
  }
  iVar5 = 0;
  if (0 < *(int *)(iVar2 + 0xb4)) {
    do {
      if ((iVar5 < 0) || (iVar4 = iVar5, *(int *)(iVar2 + 0xb4) <= iVar5)) {
        iVar4 = 0;
      }
      iVar4 = iVar4 * 100 + iVar2 + *(int *)(iVar2 + 0xb8);
      uVar1 = *(uint *)(iVar4 + 0xc);
      if ((uVar1 & 0x40) != 0) {
        *(uint *)(iVar4 + 0xc) = uVar1 & 0xffffffbf;
        param_1 = 0;
      }
      iVar5 = iVar5 + 1;
    } while (iVar5 < *(int *)(iVar2 + 0xb4));
  }
LAB_10004f79:
  *(undefined4 *)(iVar2 + 4) = 0x31;
  return (undefined1)param_1;
}


// 10004fe0 FUN_10004fe0

void __cdecl FUN_10004fe0(int param_1,int param_2)

{
  byte bVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  int iVar5;
  int iVar6;
  int iVar7;
  int iVar8;
  int local_18;
  int local_14;
  int local_10;
  int local_c;
  int local_8;
  int local_4;
  
  bVar1 = *(byte *)(param_1 + 0x17a);
  if ((bVar1 != 0) && ((int)(uint)bVar1 <= param_2)) {
    param_2 = bVar1 - 1;
  }
  local_10 = 0;
  local_c = 0;
  local_4 = 0;
  if (*(int *)(param_1 + 0xe8) < 1) {
    *(undefined1 *)(param_1 + 0x179) = (undefined1)param_2;
    return;
  }
  local_14 = 0;
  do {
    local_8 = 0;
    iVar5 = *(int *)(param_1 + 0xec) + local_14 + param_1;
    if (0 < *(int *)(iVar5 + 4)) {
      local_18 = 0;
      do {
        iVar6 = 0;
        iVar4 = 0;
        iVar7 = *(int *)(iVar5 + 0xc) + local_18 + iVar5;
        iVar8 = 0;
        if (0 < *(int *)(iVar7 + 0x48)) {
          do {
            iVar3 = iVar7 + iVar6 + *(int *)(iVar7 + 0x4c);
            iVar8 = iVar8 + 1;
            iVar2 = *(int *)(iVar3 + 0x34 + param_2 * 4);
            *(int *)(iVar3 + 0xc) = iVar4;
            *(int *)(iVar3 + 8) = iVar2;
            iVar4 = iVar4 + iVar2;
            iVar6 = iVar6 + 0x74;
          } while (iVar8 < *(int *)(iVar7 + 0x48));
        }
        local_18 = local_18 + 0x94;
        *(int *)(iVar7 + 0x54) = local_10;
        *(int *)(iVar7 + 0x50) = iVar4;
        *(int *)(iVar7 + 0x58) = local_c;
        local_10 = local_10 + iVar4 * 0x30;
        local_8 = local_8 + 1;
        local_c = local_c + iVar4 * 0x10;
      } while (local_8 < *(int *)(iVar5 + 4));
    }
    local_14 = local_14 + 0x10;
    local_4 = local_4 + 1;
  } while (local_4 < *(int *)(param_1 + 0xe8));
  *(undefined1 *)(param_1 + 0x179) = (undefined1)param_2;
  return;
}


// 10005110 FUN_10005110

int __cdecl FUN_10005110(int param_1,int param_2,char param_3)

{
  int iVar1;
  int iVar2;
  
  iVar2 = *(int *)(param_1 + 0x10 + param_2 * 4) + 1;
  iVar1 = iVar2 * 0x30 + *(int *)(param_1 + 0x38);
  if (param_3 != '\0') {
    iVar1 = iVar1 + iVar2 * 0x10;
  }
  return iVar1;
}


// 10005140 FUN_10005140

int __cdecl FUN_10005140(void *param_1,void *param_2,int param_3,char param_4)

{
  int *piVar1;
  int iVar2;
  void *pvVar3;
  undefined4 *puVar4;
  int iVar5;
  int *piVar6;
  
  pvVar3 = param_1;
  iVar2 = *(int *)((int)param_1 + param_3 * 4 + 0x10);
  _memcpy(param_2,param_1,*(size_t *)((int)param_1 + 0x38));
  if (0 < param_3) {
    puVar4 = (undefined4 *)((int)param_2 + 0x10);
    iVar5 = param_3;
    do {
      *puVar4 = *(undefined4 *)((int)param_2 + param_3 * 4 + 0x10);
      puVar4 = puVar4 + 1;
      iVar5 = iVar5 + -1;
    } while (iVar5 != 0);
  }
  if (param_4 == '\0') {
    *(undefined4 *)((int)param_2 + 0x3c) = 0;
  }
  else {
    *(int *)((int)param_2 + 0x3c) = iVar2 * 0x30 + *(int *)((int)param_2 + 0x38);
  }
  if (*(int *)((int)param_2 + 0x30) == 0) {
    _memcpy((void *)(*(int *)((int)param_2 + 0x38) + (int)param_2),
            (void *)(*(int *)((int)param_1 + 0x38) + (int)param_1),iVar2 * 0x30);
    if (param_4 != '\0') {
      _memcpy((void *)(*(int *)((int)param_2 + 0x3c) + (int)param_2),
              (void *)(*(int *)((int)param_1 + 0x3c) + (int)param_1),iVar2 << 4);
    }
    return iVar2;
  }
  piVar6 = (int *)((int)param_1 + 0x34);
  iVar5 = 0;
  piVar1 = (int *)((int)param_1 + 0x30);
  param_1 = (void *)0x0;
  if (0 < *piVar1) {
    piVar6 = (int *)((int)pvVar3 + *piVar6 + 8);
    do {
      if (param_3 <= piVar6[-2]) {
        if ((*piVar6 < 0) || (iVar2 < *piVar6 + iVar5)) {
          Error("Unable to load corrupted map.");
        }
        _memcpy((void *)(iVar5 * 0x30 + *(int *)((int)param_2 + 0x38) + (int)param_2),
                (void *)(piVar6[-1] * 0x30 + *(int *)((int)pvVar3 + 0x38) + (int)pvVar3),
                *piVar6 * 0x30);
        if (param_4 != '\0') {
          _memcpy((void *)((int)param_2 + *(int *)((int)param_2 + 0x3c) + iVar5 * 0x10),
                  (void *)(piVar6[-1] * 0x10 + *(int *)((int)pvVar3 + 0x3c) + (int)pvVar3),
                  *piVar6 << 4);
        }
        iVar5 = iVar5 + *piVar6;
      }
      param_1 = (void *)((int)param_1 + 1);
      piVar6 = piVar6 + 3;
    } while ((int)param_1 < *(int *)((int)pvVar3 + 0x30));
  }
  *(undefined4 *)((int)param_2 + 0x30) = 0;
  return iVar5;
}


// 100052b0 FUN_100052b0

void __fastcall FUN_100052b0(int param_1)

{
  char *unaff_ESI;
  char local_104 [260];
  
  FUN_100168e0((char *)(param_1 + 0xc),local_104,0x104);
  FUN_100166a0(unaff_ESI,0x104,"models/%s%s");
  FUN_100163d0(unaff_ESI,'\\');
  return;
}


// 10005300 FUN_10005300

void FUN_10005300(void)

{
  undefined4 uVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  int unaff_EDI;
  
  uVar1 = (**(code **)(*DAT_10037480 + 0xc))(*(int *)(unaff_EDI + 0x134) + unaff_EDI);
  iVar2 = 0;
  *(undefined4 *)(unaff_EDI + 0x18c) = uVar1;
  if (0 < *(int *)(unaff_EDI + 0x9c)) {
    iVar3 = 0;
    do {
      iVar4 = unaff_EDI + iVar3 + *(int *)(unaff_EDI + 0xa0);
      uVar1 = (**(code **)(*DAT_10037480 + 0xc))(*(int *)(iVar4 + 0xb0) + iVar4);
      iVar2 = iVar2 + 1;
      *(undefined4 *)(iVar4 + 0xb8) = uVar1;
      iVar3 = iVar3 + 0xd8;
    } while (iVar2 < *(int *)(unaff_EDI + 0x9c));
  }
  return;
}


// 10005370 FUN_10005370

void __fastcall FUN_10005370(undefined4 param_1,int param_2)

{
  int iVar1;
  int iVar2;
  int iVar3;
  
  iVar1 = *(int *)(param_2 + 0xbc);
  iVar3 = 0;
  if (0 < iVar1) {
    do {
      if ((iVar3 < 0) || (iVar2 = iVar3, iVar1 <= iVar3)) {
        iVar2 = 0;
      }
      if (*(int *)(iVar2 * 0xd4 + *(int *)(param_2 + 0xc0) + 0x18 + param_2) != 0) {
        return;
      }
      iVar3 = iVar3 + 1;
    } while (iVar3 < iVar1);
  }
  *(uint *)(param_2 + 0x98) = *(uint *)(param_2 + 0x98) | 0x80000;
  return;
}


// 100053c0 FUN_100053c0

undefined4 __thiscall FUN_100053c0(void *this,int param_1)

{
  if (param_1 != 1) {
    if (param_1 == 3) {
      return *(undefined4 *)((int)this + 0x18);
    }
    if (param_1 != 5) {
      return *(undefined4 *)((int)this + 0x10);
    }
  }
  return *(undefined4 *)((int)this + 0x14);
}


// 10005490 FUN_10005490

void FUN_10005490(int param_1,uint param_2)

{
  longlong lVar1;
  void *pvVar2;
  
  *(uint *)(param_1 + 0x20) = param_2;
  pvVar2 = (void *)FUN_1000dfa0(-(uint)((int)((ulonglong)param_2 * 4 >> 0x20) != 0) |
                                (uint)((ulonglong)param_2 * 4));
  *(void **)(param_1 + 0x24) = pvVar2;
  _memset(pvVar2,0,*(int *)(param_1 + 0x20) * 4);
  lVar1 = (ulonglong)*(uint *)(param_1 + 0x20) * 4;
  pvVar2 = (void *)FUN_1000dfa0(-(uint)((int)((ulonglong)lVar1 >> 0x20) != 0) | (uint)lVar1);
  *(void **)(param_1 + 0x28) = pvVar2;
  _memset(pvVar2,0,*(int *)(param_1 + 0x20) * 4);
  return;
}


// 10005520 FUN_10005520

uint __thiscall FUN_10005520(void *this,char *param_1,char *param_2,size_t param_3)

{
  uint in_EAX;
  
  if (param_2 != param_1) {
    (**(code **)(*(int *)((int)this + -0xc) + 0x5c))((uint)param_1 >> 0x10);
    in_EAX = FUN_100166a0(param_2,param_3,"%s - %s");
  }
  return in_EAX & 0xffffff00;
}


// 10005570 FUN_10005570

void __fastcall FUN_10005570(int param_1)

{
  (**(code **)(**(int **)(param_1 + 0x10) + 0x38))();
  (**(code **)(**(int **)(param_1 + 0x14) + 0x38))();
                    /* WARNING: Could not recover jumptable at 0x10005590. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(**(int **)(param_1 + 0x18) + 0x38))();
  return;
}


// 100055a0 FUN_100055a0

void __fastcall FUN_100055a0(int param_1)

{
  (**(code **)(**(int **)(param_1 + 0x10) + 0x44))();
  (**(code **)(**(int **)(param_1 + 0x14) + 0x44))();
                    /* WARNING: Could not recover jumptable at 0x100055c0. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(**(int **)(param_1 + 0x18) + 0x44))();
  return;
}


// 100055d0 FUN_100055d0

void __fastcall FUN_100055d0(int param_1)

{
  int iVar1;
  
  iVar1 = *(int *)(param_1 + 0x1c);
  while (iVar1 != 0) {
    (**(code **)(**(int **)(param_1 + 0x10) + 0x38))();
    *(int *)(param_1 + 0x1c) = *(int *)(param_1 + 0x1c) + -1;
    iVar1 = *(int *)(param_1 + 0x1c);
  }
  iVar1 = *(int *)(param_1 + 0x20);
  while (iVar1 != 0) {
    (**(code **)(**(int **)(param_1 + 0x14) + 0x38))();
    *(int *)(param_1 + 0x20) = *(int *)(param_1 + 0x20) + -1;
    iVar1 = *(int *)(param_1 + 0x20);
  }
  iVar1 = *(int *)(param_1 + 0x24);
  while (iVar1 != 0) {
    (**(code **)(**(int **)(param_1 + 0x18) + 0x38))();
    *(int *)(param_1 + 0x24) = *(int *)(param_1 + 0x24) + -1;
    iVar1 = *(int *)(param_1 + 0x24);
  }
  return;
}


// 10005690 FUN_10005690

undefined4
FUN_10005690(undefined4 param_1,undefined4 param_2,int param_3,undefined4 param_4,undefined4 param_5
            ,char param_6,int *param_7)

{
  undefined4 uVar1;
  undefined4 local_30;
  int local_2c;
  undefined4 local_28;
  undefined4 local_24;
  uint local_14;
  undefined4 local_10;
  undefined4 local_8;
  
  if (*param_7 == 0) {
    _memset(&local_30,0,0x30);
    local_30 = param_1;
    local_10 = param_2;
    local_8 = 0xffff;
    local_2c = param_3;
    local_24 = param_4;
    local_28 = param_5;
    if (param_3 == 0) {
      local_14 = 1;
    }
    if (param_6 == '\0') {
      local_14 = local_14 | 4;
    }
    uVar1 = (**(code **)(*DAT_10038510 + 0x9c))(&local_30,1,param_7);
    return uVar1;
  }
  return 0xfffffffb;
}


// 10005730 FUN_10005730

uint FUN_10005730(undefined4 param_1,int param_2)

{
  int iVar1;
  uint uVar2;
  
  iVar1 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_2 + 0x23);
  if (iVar1 != 0) {
    uVar2 = iVar1 + 0x23U & 0xffffffe0;
    *(int *)(uVar2 - 4) = iVar1;
    return uVar2;
  }
  Error("CMDLCache:: Out of memory");
  return 0;
}


// 10005770 FUN_10005770

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __fastcall FUN_10005770(undefined4 *param_1)

{
  DWORD DVar1;
  DWORD DVar2;
  DWORD DVar3;
  undefined4 *puVar4;
  int iVar5;
  
  if (param_1[2] != 0) goto LAB_1000582f;
  if ((_DAT_10037988 & 1) == 0) {
    _DAT_10037988 = _DAT_10037988 | 1;
    DAT_10037980 = 0;
    DAT_10037984 = 0;
  }
  DVar2 = GetCurrentThreadId();
  if (DVar2 == DAT_10037980) {
LAB_100057c4:
    DAT_10037984 = DAT_10037984 + 1;
  }
  else {
    DVar3 = 0;
    LOCK();
    DVar1 = DVar2;
    if (DAT_10037980 != 0) {
      DVar3 = DAT_10037980;
      DVar1 = DAT_10037980;
    }
    DAT_10037980 = DVar1;
    UNLOCK();
    if (DVar3 == 0) goto LAB_100057c4;
    CThreadFastMutex::Lock((CThreadFastMutex *)&DAT_10037980,DVar2,0);
  }
  if (param_1[2] == 0) {
    puVar4 = (undefined4 *)FUN_1000dfa0(0x898000);
    param_1[2] = puVar4;
    param_1[3] = puVar4 + 0x226000;
    iVar5 = 0x113;
    do {
      FUN_10001000(param_1,puVar4);
      puVar4 = puVar4 + 0x2000;
      iVar5 = iVar5 + -1;
    } while (iVar5 != 0);
  }
  DAT_10037984 = DAT_10037984 + -1;
  if (DAT_10037984 == 0) {
    LOCK();
    DAT_10037980 = 0;
    UNLOCK();
  }
LAB_1000582f:
  puVar4 = FUN_10001060(param_1);
  if (puVar4 == (undefined4 *)0x0) {
    FUN_1000dfa0(0x8000);
  }
  return;
}


// 10005870 FUN_10005870

void __thiscall FUN_10005870(void *this,short *param_1,short *param_2)

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


// 100058b0 FUN_100058b0

void __thiscall FUN_100058b0(void *this,int param_1)

{
  int *piVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (0 < param_1) {
    iVar3 = *(int *)((int)this + 8);
    if ((iVar3 == 0) && (iVar3 = *(int *)((int)this + 4), iVar3 == 0)) {
      iVar3 = 2;
    }
    if (iVar3 < param_1) {
      iVar3 = iVar3 * ((iVar3 + -1 + param_1) / iVar3);
    }
    *(int *)((int)this + 4) = *(int *)((int)this + 4) + iVar3;
    puVar2 = (undefined4 *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar3 * 0x18 + 8);
    if (puVar2 == (undefined4 *)0x0) {
      Error("CUtlFixedMemory overflow!\n");
    }
                    /* WARNING: Load size is inaccurate */
    piVar1 = *this;
    *puVar2 = 0;
    puVar2[1] = iVar3;
    if (piVar1 == (int *)0x0) {
      *(undefined4 **)this = puVar2;
      return;
    }
    iVar3 = *piVar1;
    while (iVar3 != 0) {
      piVar1 = (int *)*piVar1;
      iVar3 = *piVar1;
    }
    *piVar1 = (int)puVar2;
  }
  return;
}


// 10005940 FUN_10005940

void __cdecl
FUN_10005940(int param_1,int param_2,int param_3,undefined4 param_4,undefined4 param_5,
            undefined4 param_6,undefined4 param_7,undefined *param_8)

{
  char cVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  int iVar4;
  
  if (param_3 < param_2) {
    do {
      iVar4 = (param_2 + -1) / 2;
      puVar3 = (undefined4 *)(iVar4 * 0x10 + param_1);
      cVar1 = (*(code *)param_8)(puVar3,&param_4);
      if (cVar1 == '\0') break;
      puVar2 = (undefined4 *)(param_2 * 0x10 + param_1);
      *puVar2 = *puVar3;
      puVar2[1] = puVar3[1];
      puVar2[2] = puVar3[2];
      puVar2[3] = puVar3[3];
      param_2 = iVar4;
    } while (param_3 < iVar4);
  }
  puVar3 = (undefined4 *)(param_2 * 0x10 + param_1);
  *puVar3 = param_4;
  puVar3[1] = param_5;
  puVar3[2] = param_6;
  puVar3[3] = param_7;
  return;
}


// 100059d0 FUN_100059d0

void __cdecl
FUN_100059d0(int param_1,int param_2,int param_3,undefined4 param_4,undefined4 param_5,
            undefined4 param_6,undefined4 param_7,undefined2 param_8,undefined *param_9)

{
  undefined4 *puVar1;
  int iVar2;
  char cVar3;
  int iVar4;
  
  if (param_3 < param_2) {
    do {
      iVar4 = (param_2 + -1) / 2;
      puVar1 = (undefined4 *)(param_1 + iVar4 * 0x12);
      cVar3 = (*(code *)param_9)(puVar1,&param_4);
      if (cVar3 == '\0') break;
      *(undefined4 *)(param_1 + param_2 * 0x12) = *puVar1;
      iVar2 = param_1 + param_2 * 0x12;
      *(undefined4 *)(iVar2 + 4) = puVar1[1];
      *(undefined4 *)(iVar2 + 8) = puVar1[2];
      *(undefined4 *)(iVar2 + 0xc) = puVar1[3];
      *(undefined2 *)(iVar2 + 0x10) = *(undefined2 *)(puVar1 + 4);
      param_2 = iVar4;
    } while (param_3 < iVar4);
  }
  puVar1 = (undefined4 *)(param_1 + param_2 * 0x12);
  *puVar1 = param_4;
  puVar1[1] = param_5;
  puVar1[2] = param_6;
  puVar1[3] = param_7;
  *(undefined2 *)(puVar1 + 4) = param_8;
  return;
}


// 10005a80 FUN_10005a80

void __fastcall FUN_10005a80(int *param_1)

{
  if (*param_1 != 0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
    *param_1 = 0;
  }
  return;
}


// 10005ab0 FUN_10005ab0

void __cdecl FUN_10005ab0(undefined4 *param_1)

{
  if ((DAT_10037788 <= param_1) && (param_1 < DAT_1003778c)) {
    FUN_10001000(&DAT_10037780,param_1);
    return;
  }
  (**(code **)(*DAT_10038510 + 0x158))(param_1);
  return;
}


// 10005af0 FUN_10005af0

void FUN_10005af0(undefined4 param_1,undefined4 param_2,char param_3,int *param_4)

{
  FUN_10005690(param_1,param_2,0,0,0,param_3,param_4);
  return;
}


// 10005b30 FUN_10005b30

void FUN_10005b30(int param_1,uint param_2)

{
  undefined4 uVar1;
  
  if (*(int *)(param_1 + 0x38) != 0) {
    FUN_1000e140(*(int *)(param_1 + 0x38));
    *(undefined4 *)(param_1 + 0x38) = 0;
  }
  *(uint *)(param_1 + 0x34) = param_2;
  uVar1 = FUN_1000dfa0(-(uint)((int)((ulonglong)param_2 * 2 >> 0x20) != 0) |
                       (uint)((ulonglong)param_2 * 2));
  *(undefined4 *)(param_1 + 0x38) = uVar1;
  return;
}


// 10005bd0 FUN_10005bd0

uint __thiscall FUN_10005bd0(void *this,undefined4 param_1,int param_2,int param_3,int *param_4)

{
  char cVar1;
  undefined4 uVar2;
  undefined4 unaff_EBP;
  char *pcVar3;
  int iVar4;
  int iVar5;
  
  if (param_4 != (int *)0x0) {
    if (*(int *)(DAT_1003758c + 0x30) != 0) {
      Msg("MDLCache: Alloc VTX %s\n",param_3 + 0xc);
    }
    iVar4 = *param_4;
    if (iVar4 == 7) {
      iVar4 = param_4[4];
      iVar5 = *(int *)(param_3 + 8);
      if (iVar4 == iVar5) {
        if (*(int *)(DAT_1003758c + 0x30) != 0) {
          Msg("MDLCache: Load studiomdl %s\n",param_3 + 0xc);
        }
                    /* WARNING: Load size is inaccurate */
        (**(code **)(*this + 0xb4))();
                    /* WARNING: Load size is inaccurate */
        (**(code **)(*this + 100))();
        cVar1 = (**(code **)(*DAT_100384ac + 0x28))(param_3,param_4,param_2 + 8);
                    /* WARNING: Load size is inaccurate */
        (**(code **)(*this + 0x68))();
                    /* WARNING: Load size is inaccurate */
        uVar2 = (**(code **)(*this + 0xb8))();
        if (cVar1 == '\0') {
          *(ushort *)(param_2 + 0x18) = *(ushort *)(param_2 + 0x18) | 8;
        }
        else {
          *(ushort *)(param_2 + 0x18) = *(ushort *)(param_2 + 0x18) | 1;
        }
        if (*(int *)((int)this + 0x44) != 0) {
          uVar2 = (**(code **)**(undefined4 **)((int)this + 0x44))(1,unaff_EBP);
        }
        return CONCAT31((int3)((uint)uVar2 >> 8),1);
      }
      pcVar3 = "Error Index File for \'%s\' checksum %ld should be %ld\n";
    }
    else {
      iVar5 = 7;
      pcVar3 = "Error Index File for \'%s\' version %d should be %d\n";
    }
    Warning(pcVar3,param_3 + 0xc,iVar4,iVar5);
  }
  *(ushort *)(param_2 + 0x18) = *(ushort *)(param_2 + 0x18) | 8;
  return param_2 & 0xffffff00;
}


// 10005e00 FUN_10005e00

void FUN_10005e00(int param_1,undefined4 *param_2)

{
  if (param_1 == 3) {
    if ((DAT_10037788 <= param_2) && (param_2 < DAT_1003778c)) {
      FUN_10001000(&DAT_10037780,param_2);
      return;
    }
    (**(code **)(*DAT_10038510 + 0x158))(param_2);
  }
  else if (param_2 != (undefined4 *)0x0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)(((uint)param_2 & 0xfffffffc) - 4));
    return;
  }
  return;
}


// 10005e60 FUN_10005e60

void FUN_10005e60(int *param_1,int *param_2)

{
  int *piVar1;
  int iVar2;
  int local_8;
  int local_4;
  
  piVar1 = (int *)*param_2;
  if (((piVar1 != (int *)0x0) && (iVar2 = param_2[1], -1 < iVar2)) && (iVar2 < piVar1[1])) {
    if (iVar2 + 1 < piVar1[1]) {
      *param_1 = (int)piVar1;
      param_1[1] = iVar2 + 1;
      return;
    }
    local_8 = *piVar1;
    if (local_8 == 0) {
      local_8 = 0;
      local_4 = -1;
    }
    else {
      local_4 = 0;
    }
    *param_1 = local_8;
    param_1[1] = local_4;
    return;
  }
  *param_1 = 0;
  param_1[1] = -1;
  return;
}


// 10005ee0 FUN_10005ee0

undefined2 * __thiscall FUN_10005ee0(void *this,ushort param_1)

{
  if ((DAT_10037994 & 1) == 0) {
    DAT_10037994 = DAT_10037994 | 1;
    DAT_1003798c = 0xffff;
    DAT_1003798e = 0xffff;
    DAT_10037990 = 0xffff;
    DAT_10037992 = 1;
  }
  if (param_1 != 0xffff) {
    return (undefined2 *)((uint)param_1 * 0x10 + *(int *)((int)this + 4));
  }
  return &DAT_1003798c;
}


// 10005f40 FUN_10005f40

void __thiscall FUN_10005f40(void *this,int param_1,int param_2)

{
  int *piVar1;
  int *piVar2;
  
                    /* WARNING: Load size is inaccurate */
  piVar2 = *this;
  if (piVar2 != (int *)0x0) {
    do {
      piVar1 = (int *)*piVar2;
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar2);
      piVar2 = piVar1;
    } while (piVar1 != (int *)0x0);
    *(undefined4 *)this = 0;
    *(undefined4 *)((int)this + 4) = 0;
  }
  *(int *)((int)this + 8) = param_1;
  FUN_100058b0(this,param_2);
  return;
}


// 10005f90 FUN_10005f90

void __cdecl
FUN_10005f90(int param_1,int param_2,int param_3,undefined4 param_4,undefined4 param_5,
            undefined4 param_6,undefined4 param_7,undefined *param_8)

{
  char cVar1;
  int iVar2;
  int iVar3;
  undefined4 *puVar4;
  int iVar5;
  int iVar6;
  
  iVar3 = param_2 * 2;
  iVar6 = param_2;
  while (iVar5 = iVar3 + 2, iVar5 < param_3) {
    iVar2 = iVar5 * 0x10 + param_1;
    cVar1 = (*(code *)param_8)(iVar2,iVar2 + -0x10);
    if (cVar1 != '\0') {
      iVar5 = iVar3 + 1;
    }
    iVar3 = iVar5 * 0x10 + param_1;
    puVar4 = (undefined4 *)(iVar6 * 0x10 + param_1);
    *puVar4 = *(undefined4 *)(iVar5 * 0x10 + param_1);
    puVar4[1] = *(undefined4 *)(iVar3 + 4);
    puVar4[2] = *(undefined4 *)(iVar3 + 8);
    puVar4[3] = *(undefined4 *)(iVar3 + 0xc);
    iVar6 = iVar5;
    iVar3 = iVar5 * 2;
  }
  if (iVar5 == param_3) {
    iVar3 = param_3 * 0x10 + -0x10 + param_1;
    puVar4 = (undefined4 *)(iVar6 * 0x10 + param_1);
    *puVar4 = *(undefined4 *)(param_3 * 0x10 + -0x10 + param_1);
    puVar4[1] = *(undefined4 *)(iVar3 + 4);
    puVar4[2] = *(undefined4 *)(iVar3 + 8);
    puVar4[3] = *(undefined4 *)(iVar3 + 0xc);
    iVar6 = param_3 + -1;
  }
  FUN_10005940(param_1,iVar6,param_2,param_4,param_5,param_6,param_7,param_8);
  return;
}


// 10006060 FUN_10006060

void __cdecl
FUN_10006060(int param_1,int param_2,int param_3,undefined4 param_4,undefined4 param_5,
            undefined4 param_6,undefined4 param_7,undefined2 param_8,undefined *param_9)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  int iVar4;
  char cVar5;
  int iVar6;
  int iVar7;
  
  iVar4 = param_2 * 2;
  iVar7 = param_2;
  while (iVar6 = iVar4 + 2, iVar6 < param_3) {
    iVar1 = param_1 + iVar6 * 0x12;
    cVar5 = (*(code *)param_9)(iVar1,iVar1 + -0x12);
    if (cVar5 != '\0') {
      iVar6 = iVar4 + 1;
    }
    puVar2 = (undefined4 *)(param_1 + iVar6 * 0x12);
    *(undefined4 *)(param_1 + iVar7 * 0x12) = *puVar2;
    iVar7 = param_1 + iVar7 * 0x12;
    *(undefined4 *)(iVar7 + 4) = puVar2[1];
    *(undefined4 *)(iVar7 + 8) = puVar2[2];
    *(undefined4 *)(iVar7 + 0xc) = puVar2[3];
    *(undefined2 *)(iVar7 + 0x10) = *(undefined2 *)(puVar2 + 4);
    iVar7 = iVar6;
    iVar4 = iVar6 * 2;
  }
  if (iVar6 == param_3) {
    puVar3 = (undefined4 *)(param_1 + -0x12 + param_3 * 0x12);
    puVar2 = (undefined4 *)(param_1 + iVar7 * 0x12);
    *puVar2 = *puVar3;
    puVar2[1] = puVar3[1];
    puVar2[2] = puVar3[2];
    puVar2[3] = puVar3[3];
    *(undefined2 *)(puVar2 + 4) = *(undefined2 *)(puVar3 + 4);
    iVar7 = param_3 + -1;
  }
  FUN_100059d0(param_1,iVar7,param_2,param_4,param_5,param_6,param_7,param_8,param_9);
  return;
}


// 10006150 FUN_10006150

uint FUN_10006150(undefined4 *param_1,uint *param_2)

{
  undefined4 *puVar1;
  uint uVar2;
  
  puVar1 = (undefined4 *)*param_2;
  if (((puVar1 != (undefined4 *)0x0) && (uVar2 = param_2[1], -1 < (int)uVar2)) &&
     ((int)uVar2 < (int)puVar1[1])) {
    if ((puVar1 + 2 <= param_1) && (param_1 < puVar1 + 2 + puVar1[1] * 6)) {
      return CONCAT31((int3)((uint)(puVar1 + uVar2 * 6 + 2) >> 8),
                      (int)(puVar1 + uVar2 * 6 + 2) < (int)param_1);
    }
    puVar1 = (undefined4 *)*puVar1;
    while( true ) {
      if (puVar1 == (undefined4 *)0x0) {
        return 0;
      }
      if ((puVar1 + 2 <= param_1) && (param_1 < puVar1 + 2 + puVar1[1] * 6)) break;
      puVar1 = (undefined4 *)*puVar1;
    }
    return CONCAT31((int3)((uint)puVar1 >> 8),1);
  }
  return (uint)puVar1 & 0xffffff00;
}


// 100061d0 FUN_100061d0

void __thiscall FUN_100061d0(void *this,int param_1)

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
      iVar2 = 1;
    }
    if ((int)uVar4 <= iVar2) goto LAB_1000622b;
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
LAB_1000622b:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 * 0x90);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 10006260 FUN_10006260

void __thiscall FUN_10006260(void *this,int param_1)

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
        uVar2 = 2;
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
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(uVar2 << 4);
    *(undefined4 *)this = uVar3;
  }
  return;
}


// 10006300 FUN_10006300

undefined2 * __thiscall FUN_10006300(void *this,ushort param_1)

{
  if ((DAT_100379a0 & 1) == 0) {
    DAT_100379a0 = DAT_100379a0 | 1;
    DAT_10037998 = 0xffff;
    DAT_1003799a = 0xffff;
    DAT_1003799c = 0xffff;
    DAT_1003799e = 1;
  }
  if (param_1 != 0xffff) {
    return (undefined2 *)((uint)param_1 * 0x10 + *(int *)((int)this + 4));
  }
  return &DAT_10037998;
}


// 10006360 FUN_10006360

void __cdecl
FUN_10006360(undefined4 *param_1,int param_2,undefined4 *param_3,undefined4 param_4,
            undefined4 param_5,undefined4 param_6,undefined4 param_7,undefined2 param_8,
            undefined *param_9)

{
  *param_3 = *param_1;
  param_3[1] = param_1[1];
  param_3[2] = param_1[2];
  param_3[3] = param_1[3];
  *(undefined2 *)(param_3 + 4) = *(undefined2 *)(param_1 + 4);
  FUN_10006060((int)param_1,0,(param_2 - (int)param_1) / 0x12,param_4,param_5,param_6,param_7,
               param_8,param_9);
  return;
}


// 100063e0 FUN_100063e0

undefined4 * __fastcall FUN_100063e0(undefined4 *param_1)

{
  byte unaff_retaddr;
  
  *param_1 = &PTR_FUN_100259b0;
  (**(code **)(*DAT_100384f0 + 0x94))(param_1 + 2);
  if ((unaff_retaddr & 1) != 0) {
    FUN_1000e140(param_1);
  }
  return param_1;
}


// 10006420 FUN_10006420

ushort * __thiscall FUN_10006420(void *this,undefined4 param_1,ushort *param_2,undefined1 *param_3)

{
  ushort uVar1;
  char cVar2;
  ushort *puVar3;
  int iVar4;
  
  uVar1 = *(ushort *)((int)this + 0x10);
  *param_2 = 0xffff;
  *param_3 = 0;
  puVar3 = param_2;
  while (uVar1 != 0xffff) {
    *param_2 = uVar1;
                    /* WARNING: Load size is inaccurate */
    iVar4 = (uint)uVar1 * 0x10;
    cVar2 = (**this)(param_1,iVar4 + 8 + *(int *)((int)this + 4));
    if (cVar2 == '\0') {
      *param_3 = 0;
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_1003798c;
      }
      else {
        puVar3 = (ushort *)(*(int *)((int)this + 4) + iVar4);
      }
      uVar1 = puVar3[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_1003798c;
        uVar1 = DAT_1003798c;
      }
      else {
        puVar3 = (ushort *)(*(int *)((int)this + 4) + iVar4);
        uVar1 = *(ushort *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  }
  return puVar3;
}


// 10006530 FUN_10006530

ushort __fastcall FUN_10006530(int param_1)

{
  ushort *puVar1;
  ushort uVar2;
  ushort *puVar3;
  int iVar4;
  ushort local_2;
  
  uVar2 = *(ushort *)(param_1 + 0x14);
  if (uVar2 != 0xffff) {
    *(undefined2 *)(param_1 + 0x14) =
         *(undefined2 *)((uint)uVar2 * 0x10 + 2 + *(int *)(param_1 + 4));
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
    FUN_10006260((void *)(param_1 + 4),1);
    uVar2 = *puVar1;
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)uVar2)
       ) {
      iVar4 = *(int *)(param_1 + 8);
      puVar3 = &local_2;
    }
    else {
      puVar3 = (ushort *)FUN_10005870((void *)(param_1 + 4),(short *)&local_2,(short *)puVar1);
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


// 10006650 FUN_10006650

int __thiscall FUN_10006650(void *this,char param_1)

{
  int *piVar1;
  int *piVar2;
  int iVar3;
  int iVar4;
  int local_18;
  undefined4 local_14;
  int local_10;
  undefined4 local_c;
  int local_8 [2];
  
  iVar4 = *(int *)((int)this + 0x14);
  if (iVar4 == 0) {
    piVar1 = (int *)((int)this + 0x20);
    if (((*(int *)((int)this + 0x20) == 0) || (*(int *)((int)this + 0x24) < 0)) ||
       (*(int *)(*(int *)((int)this + 0x20) + 4) <= *(int *)((int)this + 0x24))) {
                    /* WARNING: Load size is inaccurate */
      local_18 = *this;
      if (local_18 == 0) {
        local_18 = 0;
        local_14 = 0xffffffff;
      }
      else {
        local_14 = 0;
      }
      local_10 = local_18;
      local_c = local_14;
      piVar2 = &local_10;
    }
    else {
      piVar2 = (int *)FUN_10005e60(&local_10,piVar1);
    }
    iVar4 = *piVar2;
    iVar3 = piVar2[1];
    if (((iVar4 == 0) || (iVar3 < 0)) || (*(int *)(iVar4 + 4) <= iVar3)) {
      FUN_100058b0(this,1);
      *(undefined4 *)((int)this + 0x28) = 0;
      if (((*piVar1 == 0) || (*(int *)((int)this + 0x24) < 0)) ||
         (*(int *)(*piVar1 + 4) <= *(int *)((int)this + 0x24))) {
        piVar2 = (int *)FUN_10001a00(this,local_8);
      }
      else {
        piVar2 = (int *)FUN_10005e60(&local_18,piVar1);
      }
      iVar4 = *piVar2;
      iVar3 = piVar2[1];
      if (((iVar4 == 0) || (iVar3 < 0)) || (*(int *)(iVar4 + 4) <= iVar3)) {
        if (DAT_100379a8 < 10) {
          Warning("CUtlLinkedList overflow! (exhausted memory allocator)\n");
          DAT_100379a8 = DAT_100379a8 + 1;
        }
        return 0;
      }
    }
    if (iVar4 + 8 + iVar3 * 0x18 == 0) {
      if (DAT_100379a4 < 10) {
        Warning("CUtlLinkedList overflow! (exhausted index range)\n");
        DAT_100379a4 = DAT_100379a4 + 1;
      }
      return 0;
    }
    *(int *)((int)this + 0x24) = iVar3;
    *piVar1 = iVar4;
    if (((iVar4 == 0) || (iVar3 = *(int *)((int)this + 0x24), iVar3 < 0)) ||
       (*(int *)(iVar4 + 4) <= iVar3)) {
      iVar4 = 0;
      *(int *)((int)this + 0x1c) = *(int *)((int)this + 0x1c) + 1;
    }
    else {
      *(int *)((int)this + 0x1c) = *(int *)((int)this + 0x1c) + 1;
      iVar4 = iVar4 + 8 + iVar3 * 0x18;
    }
  }
  else {
    *(undefined4 *)((int)this + 0x14) = *(undefined4 *)(iVar4 + 0x14);
  }
  if (param_1 == '\0') {
    *(int *)(iVar4 + 0x14) = iVar4;
    *(int *)(iVar4 + 0x10) = iVar4;
    return iVar4;
  }
  *(undefined4 *)(iVar4 + 0x14) = 0;
  *(undefined4 *)(iVar4 + 0x10) = 0;
  return iVar4;
}


// 100067d0 FUN_100067d0

ushort * __thiscall FUN_100067d0(void *this,undefined4 param_1,ushort *param_2,undefined1 *param_3)

{
  ushort uVar1;
  char cVar2;
  ushort *puVar3;
  int iVar4;
  
  uVar1 = *(ushort *)((int)this + 0x10);
  *param_2 = 0xffff;
  *param_3 = 0;
  puVar3 = param_2;
  while (uVar1 != 0xffff) {
    *param_2 = uVar1;
                    /* WARNING: Load size is inaccurate */
    iVar4 = (uint)uVar1 * 0x10;
    cVar2 = (**this)(param_1,iVar4 + 8 + *(int *)((int)this + 4));
    if (cVar2 == '\0') {
      *param_3 = 0;
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (ushort *)(*(int *)((int)this + 4) + iVar4);
      }
      uVar1 = puVar3[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_10037998;
        uVar1 = DAT_10037998;
      }
      else {
        puVar3 = (ushort *)(*(int *)((int)this + 4) + iVar4);
        uVar1 = *(ushort *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  }
  return puVar3;
}


// 100068e0 FUN_100068e0

void __thiscall FUN_100068e0(void *this,ushort param_1)

{
  ushort uVar1;
  undefined2 *puVar2;
  short *psVar3;
  ushort *puVar4;
  
  puVar2 = FUN_10005ee0(this,param_1);
  uVar1 = puVar2[1];
  puVar2 = FUN_10005ee0(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + 2 + (uint)param_1 * 0x10) = *puVar2;
  psVar3 = FUN_10005ee0(this,uVar1);
  if (*psVar3 != -1) {
    puVar4 = FUN_10005ee0(this,uVar1);
    *(ushort *)((uint)*puVar4 * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar2 = FUN_10005ee0(this,param_1);
    *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar2[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar2 = FUN_10005ee0(this,param_1);
    puVar4 = FUN_10005ee0(this,puVar2[2]);
    if (*puVar4 == param_1) {
      puVar2 = FUN_10005ee0(this,param_1);
      *(ushort *)((uint)(ushort)puVar2[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
    }
    else {
      puVar2 = FUN_10005ee0(this,param_1);
      *(ushort *)((uint)(ushort)puVar2[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
    }
  }
  *(ushort *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x10) = uVar1;
  }
  return;
}


// 100069d0 FUN_100069d0

void __thiscall FUN_100069d0(void *this,ushort param_1)

{
  ushort uVar1;
  ushort *puVar2;
  undefined2 *puVar3;
  
  puVar2 = FUN_10005ee0(this,param_1);
  uVar1 = *puVar2;
  puVar3 = FUN_10005ee0(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x10) = puVar3[1];
  puVar3 = FUN_10005ee0(this,uVar1);
  if (puVar3[1] != -1) {
    puVar3 = FUN_10005ee0(this,uVar1);
    *(ushort *)((uint)(ushort)puVar3[1] * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar3 = FUN_10005ee0(this,param_1);
    *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar3[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar3 = FUN_10005ee0(this,param_1);
    puVar3 = FUN_10005ee0(this,puVar3[2]);
    if (puVar3[1] == param_1) {
      puVar3 = FUN_10005ee0(this,param_1);
      *(ushort *)((uint)(ushort)puVar3[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
    }
    else {
      puVar3 = FUN_10005ee0(this,param_1);
      *(ushort *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
    }
  }
  *(ushort *)((uint)uVar1 * 0x10 + 2 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x10) = uVar1;
  }
  return;
}


// 10006ad0 FUN_10006ad0

void __thiscall FUN_10006ad0(void *this,ushort param_1)

{
  ushort uVar1;
  undefined2 *puVar2;
  short *psVar3;
  ushort *puVar4;
  
  puVar2 = FUN_10006300(this,param_1);
  uVar1 = puVar2[1];
  puVar2 = FUN_10006300(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + 2 + (uint)param_1 * 0x10) = *puVar2;
  psVar3 = FUN_10006300(this,uVar1);
  if (*psVar3 != -1) {
    puVar4 = FUN_10006300(this,uVar1);
    *(ushort *)((uint)*puVar4 * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar2 = FUN_10006300(this,param_1);
    *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar2[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar2 = FUN_10006300(this,param_1);
    puVar4 = FUN_10006300(this,puVar2[2]);
    if (*puVar4 == param_1) {
      puVar2 = FUN_10006300(this,param_1);
      *(ushort *)((uint)(ushort)puVar2[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
    }
    else {
      puVar2 = FUN_10006300(this,param_1);
      *(ushort *)((uint)(ushort)puVar2[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
    }
  }
  *(ushort *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x10) = uVar1;
  }
  return;
}


// 10006bc0 FUN_10006bc0

void __thiscall FUN_10006bc0(void *this,ushort param_1)

{
  ushort uVar1;
  ushort *puVar2;
  undefined2 *puVar3;
  
  puVar2 = FUN_10006300(this,param_1);
  uVar1 = *puVar2;
  puVar3 = FUN_10006300(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x10) = puVar3[1];
  puVar3 = FUN_10006300(this,uVar1);
  if (puVar3[1] != -1) {
    puVar3 = FUN_10006300(this,uVar1);
    *(ushort *)((uint)(ushort)puVar3[1] * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar3 = FUN_10006300(this,param_1);
    *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar3[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar3 = FUN_10006300(this,param_1);
    puVar3 = FUN_10006300(this,puVar3[2]);
    if (puVar3[1] == param_1) {
      puVar3 = FUN_10006300(this,param_1);
      *(ushort *)((uint)(ushort)puVar3[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
    }
    else {
      puVar3 = FUN_10006300(this,param_1);
      *(ushort *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
    }
  }
  *(ushort *)((uint)uVar1 * 0x10 + 2 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x10) = uVar1;
  }
  return;
}


// 10006cc0 FUN_10006cc0

void __thiscall FUN_10006cc0(void *this,uint param_1)

{
  ushort uVar1;
  short *psVar2;
  undefined2 *puVar3;
  ushort *puVar4;
  ushort uVar5;
  uint uVar6;
  short sVar7;
  uint uVar8;
  
  if ((short)param_1 != *(short *)((int)this + 0x10)) {
    do {
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      sVar7 = (short)param_1;
      if (sVar7 == -1) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      if (puVar3[2] == 0xffff) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (puVar3[3] != 0) break;
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      if (sVar7 == -1) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      uVar1 = puVar3[2];
      uVar8 = (uint)uVar1;
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      }
      uVar5 = puVar3[2];
      uVar6 = (uint)uVar5;
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      if (puVar3[2] == 0xffff) {
        puVar4 = &DAT_10037998;
      }
      else {
        puVar4 = (ushort *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (*puVar4 == uVar1) {
        if ((DAT_100379a0 & 1) == 0) {
          DAT_100379a0 = DAT_100379a0 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        uVar5 = puVar3[1];
        if ((DAT_100379a0 & 1) == 0) {
          DAT_100379a0 = DAT_100379a0 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
LAB_100070e0:
          *(undefined2 *)((uint)uVar1 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          param_1 = uVar6;
        }
        else {
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (sVar7 == -1) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (puVar3[2] == 0xffff) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[1] == sVar7) {
            param_1 = (uint)uVar1;
            FUN_10006ad0(this,uVar1);
            if ((DAT_100379a0 & 1) == 0) {
              DAT_100379a0 = DAT_100379a0 | 1;
              DAT_10037998 = 0xffff;
              DAT_1003799a = 0xffff;
              DAT_1003799c = 0xffff;
              DAT_1003799e = 1;
            }
            if (uVar1 == 0xffff) {
              puVar3 = &DAT_10037998;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
            }
            uVar1 = puVar3[2];
            uVar8 = (uint)uVar1;
            if ((DAT_100379a0 & 1) == 0) {
              DAT_100379a0 = DAT_100379a0 | 1;
              DAT_10037998 = 0xffff;
              DAT_1003799a = 0xffff;
              DAT_1003799c = 0xffff;
              DAT_1003799e = 1;
            }
            if (uVar1 == 0xffff) {
              puVar3 = &DAT_10037998;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
            }
            uVar6 = (uint)(ushort)puVar3[2];
          }
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_10006bc0(this,(ushort)uVar6);
        }
      }
      else {
        if ((DAT_100379a0 & 1) == 0) {
          DAT_100379a0 = DAT_100379a0 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
        }
        if (uVar5 == 0xffff) {
          puVar4 = &DAT_10037998;
        }
        else {
          puVar4 = (ushort *)(uVar6 * 0x10 + *(int *)((int)this + 4));
        }
        uVar5 = *puVar4;
        if ((DAT_100379a0 & 1) == 0) {
          DAT_100379a0 = DAT_100379a0 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) goto LAB_100070e0;
        if ((DAT_100379a0 & 1) == 0) {
          DAT_100379a0 = DAT_100379a0 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
        }
        if (sVar7 == -1) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
        }
        if ((DAT_100379a0 & 1) == 0) {
          DAT_100379a0 = DAT_100379a0 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
        }
        if (puVar3[2] == 0xffff) {
          psVar2 = &DAT_10037998;
        }
        else {
          psVar2 = (short *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
        }
        if (*psVar2 == sVar7) {
          param_1 = (uint)uVar1;
          FUN_10006bc0(this,uVar1);
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (uVar1 == 0xffff) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
          }
          uVar1 = puVar3[2];
          uVar8 = (uint)uVar1;
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (uVar1 == 0xffff) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
          }
          uVar6 = (uint)(ushort)puVar3[2];
        }
        *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
        FUN_10006ad0(this,(ushort)uVar6);
      }
    } while ((short)param_1 != *(short *)((int)this + 0x10));
  }
  *(undefined2 *)((uint)*(ushort *)((int)this + 0x10) * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
  return;
}


// 100072a0 FUN_100072a0

void __fastcall FUN_100072a0(int *param_1)

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


// 100072d0 FUN_100072d0

undefined4 __fastcall FUN_100072d0(int param_1)

{
  int iVar1;
  undefined4 uVar2;
  int *piVar3;
  uint uVar4;
  undefined4 local_10;
  undefined4 local_c;
  undefined4 local_8;
  undefined4 local_4;
  
  if ((*(byte *)(param_1 + 0x84) & 4) == 0) {
    if ((DAT_10038854 != 0) && (*(char *)(param_1 + 4) != '\0')) {
      FUN_10016a40(0,(undefined **)0x0);
    }
    if (*(int *)(param_1 + 0x10) == 0) {
      local_10 = 0xffffffff;
      local_c = 0xffffffff;
      local_8 = 0;
      local_4 = 0;
      uVar2 = (**(code **)(*DAT_100384d4 + 0x24))(param_1 + 0xc,"ModelData",&local_10,0);
      *(undefined4 *)(param_1 + 0x10) = uVar2;
    }
    if (*(int *)(param_1 + 0x14) == 0) {
      local_10 = 0xffffffff;
      local_c = 0xffffffff;
      local_8 = 0;
      local_4 = 0;
      piVar3 = (int *)(**(code **)(*DAT_100384d4 + 0x24))(param_1 + 0xc,"ModelMesh",&local_10,0);
      *(int **)(param_1 + 0x14) = piVar3;
      iVar1 = *piVar3;
      uVar4 = (**(code **)(iVar1 + 0x78))();
      (**(code **)(iVar1 + 0xc))(uVar4 | 0x10);
    }
    if (*(int *)(param_1 + 0x18) == 0) {
      local_10 = 0xffffffff;
      local_c = 0xffffffff;
      local_8 = 0;
      local_4 = 0;
      uVar2 = (**(code **)(*DAT_100384d4 + 0x24))(param_1 + 0xc,"AnimBlock",&local_10,0);
      *(undefined4 *)(param_1 + 0x18) = uVar2;
    }
    *(byte *)(param_1 + 0x84) = *(byte *)(param_1 + 0x84) & 0xfe | 4;
  }
  return 1;
}


// 100073e0 FUN_100073e0

void FUN_100073e0(undefined4 param_1,undefined4 param_2,undefined4 param_3,undefined4 param_4)

{
  (**(code **)(*(int *)(DAT_10038510 + 4) + 0x38))(param_1,param_2,param_3,param_4,0,0);
  return;
}


// 10007410 FUN_10007410

uint __thiscall FUN_10007410(void *this,undefined4 param_1,char *param_2,int *param_3)

{
  char cVar1;
  uint extraout_EAX;
  int *piVar2;
  uint extraout_EAX_00;
  undefined4 uVar3;
  uint extraout_EAX_01;
  char local_104 [240];
  ushort uStack_14;
  
  FUN_10016660(local_104,param_2,0x104);
  FUN_100163d0(local_104,'\\');
  if (*(int *)(DAT_1003758c + 0x30) != 0) {
    Msg("MDLCache: Load studiohdr %s\n",local_104);
  }
  cVar1 = (**(code **)(*(int *)(DAT_10038510 + 4) + 0x38))(local_104,&DAT_10025a40,param_3,0,0,0);
  if (cVar1 == '\0') {
    DevWarning("Failed to load %s!\n",param_2);
    return extraout_EAX & 0xffffff00;
  }
  piVar2 = (int *)((param_3[3] - param_3[8]) + *param_3);
  if (*piVar2 != 0x54534449) {
    DevWarning("Model %s not a .MDL format file!\n",param_2);
    return extraout_EAX_00 & 0xffffff00;
  }
  piVar2[0x56] = (uint)uStack_14;
  uVar3 = (**(code **)(*(int *)((int)this + 8) + 0x14))(piVar2);
  if ((char)uVar3 == '\0') {
    DevWarning("Model %s has mismatched .vvd + .vtx files!\n",param_2);
    return extraout_EAX_01 & 0xffffff00;
  }
  return CONCAT31((int3)((uint)uVar3 >> 8),1);
}


// 100076c0 FUN_100076c0

undefined4 __thiscall FUN_100076c0(void *this,undefined4 param_1)

{
  undefined4 uVar1;
  bool bVar2;
  undefined4 local_8;
  undefined4 local_4;
  
  bVar2 = *(char *)((int)this + 4) == '\0';
  local_4 = param_1;
  local_8 = param_1;
  uVar1 = param_1;
  if (!bVar2) {
    uVar1 = FUN_100174f0((int)&local_8,1);
    bVar2 = *(char *)((int)this + 4) == '\0';
    if (!bVar2) {
      uVar1 = FUN_10014530((int)&local_4,1);
      bVar2 = *(char *)((int)this + 4) == '\0';
    }
  }
  if (!bVar2) {
    uVar1 = FUN_100142c0((int)&param_1,1);
  }
  return CONCAT31((int3)((uint)uVar1 >> 8),1);
}


// 10007720 FUN_10007720

ushort __thiscall FUN_10007720(void *this,undefined4 param_1)

{
  char cVar1;
  undefined2 *puVar2;
  ushort uVar3;
  int iVar4;
  bool bVar5;
  
  uVar3 = *(ushort *)((int)this + 0x10);
  if (uVar3 == 0xffff) {
    return 0xffff;
  }
  do {
                    /* WARNING: Load size is inaccurate */
    iVar4 = (uint)uVar3 * 0x10;
    cVar1 = (**this)(param_1,*(int *)((int)this + 4) + 8 + iVar4);
    if (cVar1 == '\0') {
                    /* WARNING: Load size is inaccurate */
      cVar1 = (**this)(*(int *)((int)this + 4) + 8 + iVar4,param_1);
      if (cVar1 == '\0') {
        return uVar3;
      }
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      if (uVar3 == 0xffff) {
        puVar2 = &DAT_1003798c;
      }
      else {
        puVar2 = (undefined2 *)(*(int *)((int)this + 4) + iVar4);
      }
      uVar3 = puVar2[1];
    }
    else {
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      bVar5 = uVar3 != 0xffff;
      uVar3 = DAT_1003798c;
      if (bVar5) {
        uVar3 = *(ushort *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  } while (uVar3 != 0xffff);
  return 0xffff;
}


// 10007820 FUN_10007820

void __fastcall FUN_10007820(int *param_1)

{
  undefined4 *puVar1;
  int iVar2;
  int iVar3;
  uint uVar4;
  int *piVar5;
  int local_18;
  int local_14;
  int local_10 [3];
  
  if ((param_1[8] == 0) && (param_1[9] == -1)) {
    return;
  }
  if (*param_1 == 0) {
    local_10[0] = 0;
    local_10[1] = 0xffffffff;
  }
  else {
    local_10[0] = *param_1;
    local_10[1] = 0;
  }
  piVar5 = local_10;
  do {
    iVar2 = *piVar5;
    iVar3 = piVar5[1];
    local_18 = iVar2;
    local_14 = iVar3;
    if (iVar2 == 0) {
      if (iVar3 == -1) {
LAB_100078d8:
        param_1[3] = 0;
        param_1[4] = 0;
        param_1[6] = 0;
        return;
      }
    }
    else if (((((-1 < iVar3) && (iVar3 < *(int *)(iVar2 + 4))) &&
              (puVar1 = (undefined4 *)(iVar2 + 8 + iVar3 * 0x18), puVar1 != (undefined4 *)0x0)) &&
             (uVar4 = FUN_10006150(puVar1,(uint *)(param_1 + 8)), (char)uVar4 == '\0')) &&
            (((undefined4 *)puVar1[4] != puVar1 || ((undefined4 *)puVar1[5] == puVar1)))) {
      puVar1[4] = puVar1;
      puVar1[5] = param_1[5];
      param_1[5] = (int)puVar1;
    }
    if ((iVar2 == param_1[8]) && (iVar3 == param_1[9])) goto LAB_100078d8;
    piVar5 = (int *)FUN_10005e60(local_10,&local_18);
  } while( true );
}


// 100078f0 FUN_100078f0

ushort __thiscall FUN_100078f0(void *this,undefined4 param_1)

{
  char cVar1;
  undefined2 *puVar2;
  ushort uVar3;
  int iVar4;
  bool bVar5;
  
  uVar3 = *(ushort *)((int)this + 0x10);
  if (uVar3 == 0xffff) {
    return 0xffff;
  }
  do {
                    /* WARNING: Load size is inaccurate */
    iVar4 = (uint)uVar3 * 0x10;
    cVar1 = (**this)(param_1,*(int *)((int)this + 4) + 8 + iVar4);
    if (cVar1 == '\0') {
                    /* WARNING: Load size is inaccurate */
      cVar1 = (**this)(*(int *)((int)this + 4) + 8 + iVar4,param_1);
      if (cVar1 == '\0') {
        return uVar3;
      }
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      if (uVar3 == 0xffff) {
        puVar2 = &DAT_10037998;
      }
      else {
        puVar2 = (undefined2 *)(*(int *)((int)this + 4) + iVar4);
      }
      uVar3 = puVar2[1];
    }
    else {
      if ((DAT_100379a0 & 1) == 0) {
        DAT_100379a0 = DAT_100379a0 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
      }
      bVar5 = uVar3 != 0xffff;
      uVar3 = DAT_10037998;
      if (bVar5) {
        uVar3 = *(ushort *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  } while (uVar3 != 0xffff);
  return 0xffff;
}


// 100079f0 FUN_100079f0

void __fastcall FUN_100079f0(int param_1)

{
  ushort uVar1;
  ushort uVar2;
  short *psVar3;
  uint uVar4;
  
  uVar4 = DAT_100379a0;
  uVar1 = *(ushort *)(param_1 + 0x10);
  while( true ) {
    uVar2 = uVar1;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_10037998 = 0xffff;
      DAT_1003799a = 0xffff;
      DAT_1003799c = 0xffff;
      DAT_1003799e = 1;
      DAT_100379a0 = uVar4;
    }
    if (uVar2 == 0xffff) {
      psVar3 = (short *)&DAT_10037998;
    }
    else {
      psVar3 = (short *)((uint)uVar2 * 0x10 + *(int *)(param_1 + 4));
    }
    if (*psVar3 == -1) break;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_10037998 = 0xffff;
      DAT_1003799a = 0xffff;
      DAT_1003799c = 0xffff;
      DAT_1003799e = 1;
      DAT_100379a0 = uVar4;
    }
    uVar1 = DAT_10037998;
    if (uVar2 != 0xffff) {
      uVar1 = *(ushort *)((uint)uVar2 * 0x10 + *(int *)(param_1 + 4));
    }
  }
  return;
}


// 10007aa0 FUN_10007aa0

void __thiscall FUN_10007aa0(void *this,ushort param_1)

{
  ushort uVar1;
  ushort uVar2;
  undefined2 *puVar3;
  short *psVar4;
  uint uVar5;
  
  puVar3 = FUN_10006300(this,param_1);
  if (puVar3[1] == -1) {
    puVar3 = FUN_10006300(this,param_1);
    uVar5 = DAT_100379a0;
    while( true ) {
      uVar1 = puVar3[2];
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
        DAT_100379a0 = uVar5;
      }
      if (param_1 == 0xffff) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (undefined2 *)((uint)param_1 * 0x10 + *(int *)((int)this + 4));
      }
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
        DAT_100379a0 = uVar5;
      }
      if (puVar3[2] == 0xffff) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if ((puVar3[1] != param_1) || (uVar1 == 0xffff)) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
        DAT_100379a0 = uVar5;
      }
      puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      param_1 = uVar1;
    }
  }
  else {
    puVar3 = FUN_10006300(this,param_1);
    uVar5 = DAT_100379a0;
    uVar1 = puVar3[1];
    while( true ) {
      uVar2 = uVar1;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
        DAT_100379a0 = uVar5;
      }
      if (uVar2 == 0xffff) {
        psVar4 = (short *)&DAT_10037998;
      }
      else {
        psVar4 = (short *)((uint)uVar2 * 0x10 + *(int *)((int)this + 4));
      }
      if (*psVar4 == -1) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
        DAT_100379a0 = uVar5;
      }
      uVar1 = DAT_10037998;
      if (uVar2 != 0xffff) {
        uVar1 = *(ushort *)((uint)uVar2 * 0x10 + *(int *)((int)this + 4));
      }
    }
  }
  return;
}


// 10007c50 FUN_10007c50

void __fastcall FUN_10007c50(void *param_1)

{
  ushort uVar1;
  ushort uVar2;
  ushort *puVar3;
  ushort uVar4;
  int iVar5;
  
  uVar1 = *(ushort *)((int)param_1 + 0x16);
  if (uVar1 != 0xffff) {
    uVar2 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (uVar2 != 0xffff) {
      if ((((uVar2 == 0) || (uVar2 != 0)) && ((int)(uint)uVar2 < *(int *)((int)param_1 + 8))) &&
         ((uVar2 <= uVar1 && (puVar3 = FUN_10005ee0(param_1,uVar2), *puVar3 != uVar2)))) {
        iVar5 = (uint)uVar2 * 0x10;
        *(undefined2 *)(iVar5 + 2 + *(int *)((int)param_1 + 4)) =
             *(undefined2 *)((int)param_1 + 0x14);
        *(ushort *)(iVar5 + *(int *)((int)param_1 + 4)) = uVar2;
        *(ushort *)((int)param_1 + 0x14) = uVar2;
      }
      uVar1 = *(ushort *)((int)param_1 + 0x16);
      if (uVar2 == uVar1) break;
      uVar4 = uVar2 + 1;
      if (((uVar4 != 0) && (uVar4 == 0)) || (*(int *)((int)param_1 + 8) <= (int)(uint)uVar4)) {
        uVar2 = 0xffff;
      }
      else {
        uVar2 = uVar2 + 1;
      }
    }
    *(undefined2 *)((int)param_1 + 0x10) = 0xffff;
    *(undefined2 *)((int)param_1 + 0x12) = 0;
  }
  return;
}


// 10007d20 FUN_10007d20

void __thiscall FUN_10007d20(void *this,undefined4 *param_1)

{
  int iVar1;
  int iVar2;
  uint uVar3;
  
  if (param_1 != (undefined4 *)0x0) {
    uVar3 = FUN_10006150(param_1,(uint *)((int)this + 0x20));
    if (((char)uVar3 == '\0') && ((undefined4 *)param_1[4] != param_1)) {
      iVar1 = param_1[4];
      iVar2 = param_1[5];
      if (iVar1 == 0) {
        *(int *)((int)this + 0xc) = iVar2;
      }
      else {
        *(int *)(iVar1 + 0x14) = iVar2;
      }
      if (iVar2 != 0) {
        *(int *)(iVar2 + 0x10) = iVar1;
        *(int *)((int)this + 0x18) = *(int *)((int)this + 0x18) + -1;
        param_1[5] = param_1;
        param_1[4] = param_1;
        return;
      }
      *(int *)((int)this + 0x18) = *(int *)((int)this + 0x18) + -1;
      *(int *)((int)this + 0x10) = iVar1;
      param_1[5] = param_1;
      param_1[4] = param_1;
    }
  }
  return;
}


// 10007d80 FUN_10007d80

void __fastcall FUN_10007d80(void *param_1)

{
  ushort uVar1;
  ushort uVar2;
  ushort *puVar3;
  ushort uVar4;
  int iVar5;
  
  uVar1 = *(ushort *)((int)param_1 + 0x16);
  if (uVar1 != 0xffff) {
    uVar2 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (uVar2 != 0xffff) {
      if ((((uVar2 == 0) || (uVar2 != 0)) && ((int)(uint)uVar2 < *(int *)((int)param_1 + 8))) &&
         ((uVar2 <= uVar1 && (puVar3 = FUN_10006300(param_1,uVar2), *puVar3 != uVar2)))) {
        iVar5 = (uint)uVar2 * 0x10;
        *(undefined2 *)(iVar5 + 2 + *(int *)((int)param_1 + 4)) =
             *(undefined2 *)((int)param_1 + 0x14);
        *(ushort *)(iVar5 + *(int *)((int)param_1 + 4)) = uVar2;
        *(ushort *)((int)param_1 + 0x14) = uVar2;
      }
      uVar1 = *(ushort *)((int)param_1 + 0x16);
      if (uVar2 == uVar1) break;
      uVar4 = uVar2 + 1;
      if (((uVar4 != 0) && (uVar4 == 0)) || (*(int *)((int)param_1 + 8) <= (int)(uint)uVar4)) {
        uVar2 = 0xffff;
      }
      else {
        uVar2 = uVar2 + 1;
      }
    }
    *(undefined2 *)((int)param_1 + 0x10) = 0xffff;
    *(undefined2 *)((int)param_1 + 0x12) = 0;
  }
  return;
}


// 10007e50 FUN_10007e50

void __thiscall FUN_10007e50(void *this,uint param_1)

{
  ushort uVar1;
  short *psVar2;
  undefined2 *puVar3;
  ushort *puVar4;
  ushort uVar5;
  uint uVar6;
  short sVar7;
  uint uVar8;
  
  if ((short)param_1 != *(short *)((int)this + 0x10)) {
    do {
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      sVar7 = (short)param_1;
      if (sVar7 == -1) {
        puVar3 = &DAT_1003798c;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      if (puVar3[2] == 0xffff) {
        puVar3 = &DAT_1003798c;
      }
      else {
        puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (puVar3[3] != 0) break;
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      if (sVar7 == -1) {
        puVar3 = &DAT_1003798c;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      uVar1 = puVar3[2];
      uVar8 = (uint)uVar1;
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_1003798c;
      }
      else {
        puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      }
      uVar5 = puVar3[2];
      uVar6 = (uint)uVar5;
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_1003798c;
      }
      else {
        puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_10037994 & 1) == 0) {
        DAT_10037994 = DAT_10037994 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
      }
      if (puVar3[2] == 0xffff) {
        puVar4 = &DAT_1003798c;
      }
      else {
        puVar4 = (ushort *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (*puVar4 == uVar1) {
        if ((DAT_10037994 & 1) == 0) {
          DAT_10037994 = DAT_10037994 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        uVar5 = puVar3[1];
        if ((DAT_10037994 & 1) == 0) {
          DAT_10037994 = DAT_10037994 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
LAB_10008270:
          *(undefined2 *)((uint)uVar1 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          param_1 = uVar6;
        }
        else {
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (sVar7 == -1) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (puVar3[2] == 0xffff) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[1] == sVar7) {
            param_1 = (uint)uVar1;
            FUN_100068e0(this,uVar1);
            if ((DAT_10037994 & 1) == 0) {
              DAT_10037994 = DAT_10037994 | 1;
              DAT_1003798c = 0xffff;
              DAT_1003798e = 0xffff;
              DAT_10037990 = 0xffff;
              DAT_10037992 = 1;
            }
            if (uVar1 == 0xffff) {
              puVar3 = &DAT_1003798c;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
            }
            uVar1 = puVar3[2];
            uVar8 = (uint)uVar1;
            if ((DAT_10037994 & 1) == 0) {
              DAT_10037994 = DAT_10037994 | 1;
              DAT_1003798c = 0xffff;
              DAT_1003798e = 0xffff;
              DAT_10037990 = 0xffff;
              DAT_10037992 = 1;
            }
            if (uVar1 == 0xffff) {
              puVar3 = &DAT_1003798c;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
            }
            uVar6 = (uint)(ushort)puVar3[2];
          }
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_100069d0(this,(ushort)uVar6);
        }
      }
      else {
        if ((DAT_10037994 & 1) == 0) {
          DAT_10037994 = DAT_10037994 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
        }
        if (uVar5 == 0xffff) {
          puVar4 = &DAT_1003798c;
        }
        else {
          puVar4 = (ushort *)(uVar6 * 0x10 + *(int *)((int)this + 4));
        }
        uVar5 = *puVar4;
        if ((DAT_10037994 & 1) == 0) {
          DAT_10037994 = DAT_10037994 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) goto LAB_10008270;
        if ((DAT_10037994 & 1) == 0) {
          DAT_10037994 = DAT_10037994 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
        }
        if (sVar7 == -1) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
        }
        if ((DAT_10037994 & 1) == 0) {
          DAT_10037994 = DAT_10037994 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
        }
        if (puVar3[2] == 0xffff) {
          psVar2 = &DAT_1003798c;
        }
        else {
          psVar2 = (short *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
        }
        if (*psVar2 == sVar7) {
          param_1 = (uint)uVar1;
          FUN_100069d0(this,uVar1);
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (uVar1 == 0xffff) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
          }
          uVar1 = puVar3[2];
          uVar8 = (uint)uVar1;
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (uVar1 == 0xffff) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
          }
          uVar6 = (uint)(ushort)puVar3[2];
        }
        *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
        FUN_100068e0(this,(ushort)uVar6);
      }
    } while ((short)param_1 != *(short *)((int)this + 0x10));
  }
  *(undefined2 *)((uint)*(ushort *)((int)this + 0x10) * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
  return;
}


// 10008430 FUN_10008430

void __thiscall FUN_10008430(void *this,uint param_1)

{
  ushort uVar1;
  uint uVar2;
  undefined2 *puVar3;
  short *psVar4;
  ushort *puVar5;
  short sVar6;
  ushort uVar7;
  uint uVar8;
  uint uVar9;
  
  uVar2 = DAT_10037994;
  if ((short)param_1 != *(short *)((int)this + 0x10)) {
    do {
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
        DAT_10037994 = uVar2;
      }
      sVar6 = (short)param_1;
      if (sVar6 == -1) {
        puVar3 = &DAT_1003798c;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      if (puVar3[3] != 1) break;
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
        DAT_10037994 = uVar2;
      }
      if (sVar6 == -1) {
        puVar3 = &DAT_1003798c;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      uVar7 = puVar3[2];
      uVar9 = (uint)uVar7;
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_1003798c = 0xffff;
        DAT_1003798e = 0xffff;
        DAT_10037990 = 0xffff;
        DAT_10037992 = 1;
        DAT_10037994 = uVar2;
      }
      if (uVar7 == 0xffff) {
        psVar4 = &DAT_1003798c;
      }
      else {
        psVar4 = (short *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
      }
      if (sVar6 == *psVar4) {
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
        }
        uVar1 = puVar3[1];
        uVar8 = (uint)uVar1;
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (uVar1 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar7 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_100068e0(this,uVar7);
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (sVar6 == -1) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          uVar7 = puVar3[2];
          uVar9 = (uint)uVar7;
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (uVar7 == 0xffff) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
          }
          uVar8 = (uint)(ushort)puVar3[1];
          uVar2 = DAT_10037994;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        uVar7 = (ushort)uVar8;
        if (uVar7 == 0xffff) {
          puVar5 = &DAT_1003798c;
        }
        else {
          puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (*puVar5 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)*puVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] != 1) {
LAB_1000877d:
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
            DAT_10037994 = uVar2;
          }
          if (uVar7 == 0xffff) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
          }
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
            DAT_10037994 = uVar2;
          }
          if (puVar3[1] == 0xffff) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[3] == 1) {
            if ((uVar2 & 1) == 0) {
              DAT_10037994 = uVar2 | 1;
              DAT_1003798c = 0xffff;
              DAT_1003798e = 0xffff;
              DAT_10037990 = 0xffff;
              DAT_10037992 = 1;
            }
            if (uVar7 == 0xffff) {
              puVar5 = &DAT_1003798c;
            }
            else {
              puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
            }
            *(undefined2 *)((uint)*puVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
            *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
            FUN_100069d0(this,uVar7);
            if ((DAT_10037994 & 1) == 0) {
              DAT_10037994 = DAT_10037994 | 1;
              DAT_1003798c = 0xffff;
              DAT_1003798e = 0xffff;
              DAT_10037990 = 0xffff;
              DAT_10037992 = 1;
            }
            if (sVar6 == -1) {
              puVar3 = &DAT_1003798c;
            }
            else {
              puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
            }
            uVar7 = puVar3[2];
            uVar9 = (uint)uVar7;
            if ((DAT_10037994 & 1) == 0) {
              DAT_10037994 = DAT_10037994 | 1;
              DAT_1003798c = 0xffff;
              DAT_1003798e = 0xffff;
              DAT_10037990 = 0xffff;
              DAT_10037992 = 1;
            }
            if (uVar7 == 0xffff) {
              puVar3 = &DAT_1003798c;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
            }
            uVar8 = (uint)(ushort)puVar3[1];
            uVar2 = DAT_10037994;
          }
          if ((uVar2 & 1) == 0) {
            DAT_10037994 = uVar2 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if ((ushort)uVar9 == 0xffff) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)(uVar9 * 0x10 + *(int *)((int)this + 4));
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar8 * 0x10) = puVar3[3];
          *(undefined2 *)(uVar9 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if ((short)uVar8 == -1) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)(*(int *)((int)this + 4) + uVar8 * 0x10);
          }
          *(undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          FUN_100068e0(this,(ushort)uVar9);
          goto LAB_10008e2d;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (puVar3[1] == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] != 1) goto LAB_1000877d;
        param_1 = uVar9;
        if (uVar7 != 0xffff) {
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          uVar2 = DAT_10037994;
        }
      }
      else {
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar5 = &DAT_1003798c;
        }
        else {
          puVar5 = (ushort *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
        }
        uVar1 = *puVar5;
        uVar8 = (uint)uVar1;
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (uVar1 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar7 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_100069d0(this,uVar7);
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (sVar6 == -1) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          uVar7 = puVar3[2];
          uVar9 = (uint)uVar7;
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (uVar7 == 0xffff) {
            puVar5 = &DAT_1003798c;
          }
          else {
            puVar5 = (ushort *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
          }
          uVar8 = (uint)*puVar5;
          uVar2 = DAT_10037994;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        uVar7 = (ushort)uVar8;
        if (uVar7 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (puVar3[1] == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 1) {
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
            DAT_10037994 = uVar2;
          }
          if (uVar7 == 0xffff) {
            puVar5 = &DAT_1003798c;
          }
          else {
            puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
          }
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
            DAT_10037994 = uVar2;
          }
          if (*puVar5 == 0xffff) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((uint)*puVar5 * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[3] == 1) {
            param_1 = uVar9;
            if (uVar7 != 0xffff) {
              *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
              uVar2 = DAT_10037994;
            }
            goto LAB_10008e3a;
          }
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar5 = &DAT_1003798c;
        }
        else {
          puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar2;
        }
        if (*puVar5 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)((uint)*puVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 1) {
          if ((uVar2 & 1) == 0) {
            DAT_10037994 = uVar2 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (uVar7 == 0xffff) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
          }
          *(undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_100068e0(this,uVar7);
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (sVar6 == -1) {
            puVar3 = &DAT_1003798c;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          uVar7 = puVar3[2];
          uVar9 = (uint)uVar7;
          if ((DAT_10037994 & 1) == 0) {
            DAT_10037994 = DAT_10037994 | 1;
            DAT_1003798c = 0xffff;
            DAT_1003798e = 0xffff;
            DAT_10037990 = 0xffff;
            DAT_10037992 = 1;
          }
          if (uVar7 == 0xffff) {
            puVar5 = &DAT_1003798c;
          }
          else {
            puVar5 = (ushort *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
          }
          uVar8 = (uint)*puVar5;
          uVar2 = DAT_10037994;
        }
        if ((uVar2 & 1) == 0) {
          DAT_10037994 = uVar2 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
        }
        if ((ushort)uVar9 == 0xffff) {
          puVar3 = &DAT_1003798c;
        }
        else {
          puVar3 = (undefined2 *)(uVar9 * 0x10 + *(int *)((int)this + 4));
        }
        *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar8 * 0x10) = puVar3[3];
        *(undefined2 *)(uVar9 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        if ((DAT_10037994 & 1) == 0) {
          DAT_10037994 = DAT_10037994 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
        }
        if ((short)uVar8 == -1) {
          puVar5 = &DAT_1003798c;
        }
        else {
          puVar5 = (ushort *)(*(int *)((int)this + 4) + uVar8 * 0x10);
        }
        *(undefined2 *)((uint)*puVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        FUN_100069d0(this,(ushort)uVar9);
LAB_10008e2d:
        uVar2 = DAT_10037994;
        param_1 = (uint)*(ushort *)((int)this + 0x10);
      }
LAB_10008e3a:
    } while ((short)param_1 != *(short *)((int)this + 0x10));
  }
  *(undefined2 *)((param_1 & 0xffff) * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
  return;
}


// 10008e70 FUN_10008e70

void __thiscall FUN_10008e70(void *this,uint param_1,ushort param_2,char param_3)

{
  undefined2 *puVar1;
  undefined2 uVar2;
  
  puVar1 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
  puVar1[2] = param_2;
  puVar1[1] = 0xffff;
  *puVar1 = 0xffff;
  puVar1[3] = 0;
  uVar2 = (undefined2)param_1;
  if (param_2 == 0xffff) {
    *(undefined2 *)((int)this + 0x10) = uVar2;
    FUN_10006cc0(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(undefined2 *)((uint)param_2 * 0x10 + *(int *)((int)this + 4)) = uVar2;
    FUN_10006cc0(this,param_1);
    return;
  }
  *(undefined2 *)((uint)param_2 * 0x10 + 2 + *(int *)((int)this + 4)) = uVar2;
  FUN_10006cc0(this,param_1);
  return;
}


// 10008ee0 FUN_10008ee0

void __thiscall FUN_10008ee0(void *this,uint param_1)

{
  ushort uVar1;
  uint uVar2;
  undefined2 *puVar3;
  short *psVar4;
  ushort *puVar5;
  short sVar6;
  ushort uVar7;
  uint uVar8;
  uint uVar9;
  
  uVar2 = DAT_100379a0;
  if ((short)param_1 != *(short *)((int)this + 0x10)) {
    do {
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
        DAT_100379a0 = uVar2;
      }
      sVar6 = (short)param_1;
      if (sVar6 == -1) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      if (puVar3[3] != 1) break;
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
        DAT_100379a0 = uVar2;
      }
      if (sVar6 == -1) {
        puVar3 = &DAT_10037998;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      uVar7 = puVar3[2];
      uVar9 = (uint)uVar7;
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_10037998 = 0xffff;
        DAT_1003799a = 0xffff;
        DAT_1003799c = 0xffff;
        DAT_1003799e = 1;
        DAT_100379a0 = uVar2;
      }
      if (uVar7 == 0xffff) {
        psVar4 = &DAT_10037998;
      }
      else {
        psVar4 = (short *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
      }
      if (sVar6 == *psVar4) {
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
        }
        uVar1 = puVar3[1];
        uVar8 = (uint)uVar1;
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (uVar1 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar7 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_10006ad0(this,uVar7);
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (sVar6 == -1) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          uVar7 = puVar3[2];
          uVar9 = (uint)uVar7;
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (uVar7 == 0xffff) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
          }
          uVar8 = (uint)(ushort)puVar3[1];
          uVar2 = DAT_100379a0;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        uVar7 = (ushort)uVar8;
        if (uVar7 == 0xffff) {
          puVar5 = &DAT_10037998;
        }
        else {
          puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (*puVar5 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)*puVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] != 1) {
LAB_1000922d:
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
            DAT_100379a0 = uVar2;
          }
          if (uVar7 == 0xffff) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
          }
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
            DAT_100379a0 = uVar2;
          }
          if (puVar3[1] == 0xffff) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[3] == 1) {
            if ((uVar2 & 1) == 0) {
              DAT_100379a0 = uVar2 | 1;
              DAT_10037998 = 0xffff;
              DAT_1003799a = 0xffff;
              DAT_1003799c = 0xffff;
              DAT_1003799e = 1;
            }
            if (uVar7 == 0xffff) {
              puVar5 = &DAT_10037998;
            }
            else {
              puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
            }
            *(undefined2 *)((uint)*puVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
            *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
            FUN_10006bc0(this,uVar7);
            if ((DAT_100379a0 & 1) == 0) {
              DAT_100379a0 = DAT_100379a0 | 1;
              DAT_10037998 = 0xffff;
              DAT_1003799a = 0xffff;
              DAT_1003799c = 0xffff;
              DAT_1003799e = 1;
            }
            if (sVar6 == -1) {
              puVar3 = &DAT_10037998;
            }
            else {
              puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
            }
            uVar7 = puVar3[2];
            uVar9 = (uint)uVar7;
            if ((DAT_100379a0 & 1) == 0) {
              DAT_100379a0 = DAT_100379a0 | 1;
              DAT_10037998 = 0xffff;
              DAT_1003799a = 0xffff;
              DAT_1003799c = 0xffff;
              DAT_1003799e = 1;
            }
            if (uVar7 == 0xffff) {
              puVar3 = &DAT_10037998;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
            }
            uVar8 = (uint)(ushort)puVar3[1];
            uVar2 = DAT_100379a0;
          }
          if ((uVar2 & 1) == 0) {
            DAT_100379a0 = uVar2 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if ((ushort)uVar9 == 0xffff) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)(uVar9 * 0x10 + *(int *)((int)this + 4));
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar8 * 0x10) = puVar3[3];
          *(undefined2 *)(uVar9 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if ((short)uVar8 == -1) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)(*(int *)((int)this + 4) + uVar8 * 0x10);
          }
          *(undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          FUN_10006ad0(this,(ushort)uVar9);
          goto LAB_100098dd;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (puVar3[1] == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] != 1) goto LAB_1000922d;
        param_1 = uVar9;
        if (uVar7 != 0xffff) {
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          uVar2 = DAT_100379a0;
        }
      }
      else {
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar5 = &DAT_10037998;
        }
        else {
          puVar5 = (ushort *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
        }
        uVar1 = *puVar5;
        uVar8 = (uint)uVar1;
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (uVar1 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar7 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_10006bc0(this,uVar7);
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (sVar6 == -1) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          uVar7 = puVar3[2];
          uVar9 = (uint)uVar7;
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (uVar7 == 0xffff) {
            puVar5 = &DAT_10037998;
          }
          else {
            puVar5 = (ushort *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
          }
          uVar8 = (uint)*puVar5;
          uVar2 = DAT_100379a0;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        uVar7 = (ushort)uVar8;
        if (uVar7 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (puVar3[1] == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 1) {
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
            DAT_100379a0 = uVar2;
          }
          if (uVar7 == 0xffff) {
            puVar5 = &DAT_10037998;
          }
          else {
            puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
          }
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
            DAT_100379a0 = uVar2;
          }
          if (*puVar5 == 0xffff) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((uint)*puVar5 * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[3] == 1) {
            param_1 = uVar9;
            if (uVar7 != 0xffff) {
              *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
              uVar2 = DAT_100379a0;
            }
            goto LAB_100098ea;
          }
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar5 = &DAT_10037998;
        }
        else {
          puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar2;
        }
        if (*puVar5 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)((uint)*puVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 1) {
          if ((uVar2 & 1) == 0) {
            DAT_100379a0 = uVar2 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (uVar7 == 0xffff) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
          }
          *(undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_10006ad0(this,uVar7);
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (sVar6 == -1) {
            puVar3 = &DAT_10037998;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          uVar7 = puVar3[2];
          uVar9 = (uint)uVar7;
          if ((DAT_100379a0 & 1) == 0) {
            DAT_100379a0 = DAT_100379a0 | 1;
            DAT_10037998 = 0xffff;
            DAT_1003799a = 0xffff;
            DAT_1003799c = 0xffff;
            DAT_1003799e = 1;
          }
          if (uVar7 == 0xffff) {
            puVar5 = &DAT_10037998;
          }
          else {
            puVar5 = (ushort *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
          }
          uVar8 = (uint)*puVar5;
          uVar2 = DAT_100379a0;
        }
        if ((uVar2 & 1) == 0) {
          DAT_100379a0 = uVar2 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
        }
        if ((ushort)uVar9 == 0xffff) {
          puVar3 = &DAT_10037998;
        }
        else {
          puVar3 = (undefined2 *)(uVar9 * 0x10 + *(int *)((int)this + 4));
        }
        *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar8 * 0x10) = puVar3[3];
        *(undefined2 *)(uVar9 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        if ((DAT_100379a0 & 1) == 0) {
          DAT_100379a0 = DAT_100379a0 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
        }
        if ((short)uVar8 == -1) {
          puVar5 = &DAT_10037998;
        }
        else {
          puVar5 = (ushort *)(*(int *)((int)this + 4) + uVar8 * 0x10);
        }
        *(undefined2 *)((uint)*puVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        FUN_10006bc0(this,(ushort)uVar9);
LAB_100098dd:
        uVar2 = DAT_100379a0;
        param_1 = (uint)*(ushort *)((int)this + 0x10);
      }
LAB_100098ea:
    } while ((short)param_1 != *(short *)((int)this + 0x10));
  }
  *(undefined2 *)((param_1 & 0xffff) * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
  return;
}


// 10009920 FUN_10009920

void __fastcall FUN_10009920(undefined4 *param_1)

{
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
  param_1[0x10] = 0;
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
  param_1[0x20] = 0;
  param_1[0x21] = 0;
  param_1[0x22] = 0;
  param_1[0x23] = 0;
  *param_1 = 0;
  return;
}


// 100099a0 FUN_100099a0

void __thiscall FUN_100099a0(void *this,uint param_1)

{
  int iVar1;
  
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0x54))(param_1,0xffffffff);
  iVar1 = (param_1 & 0xffff) * 0x10;
  FUN_100016c0(&DAT_1003749c,*(undefined4 **)(*(int *)((int)this + 0x2c) + 0xc + iVar1));
  *(undefined4 *)(*(int *)((int)this + 0x2c) + 0xc + iVar1) = 0;
  return;
}


// 10009a10 FUN_10009a10

char * __thiscall FUN_10009a10(void *this,ushort param_1)

{
  if ((param_1 != 0xffff) &&
     ((*(byte *)(*(int *)((uint)param_1 * 0x10 + 0xc + *(int *)((int)this + 0x2c)) + 0x18) & 4) == 0
     )) {
    return *(char **)((uint)param_1 * 0x10 + *(int *)((int)this + 0x2c) + 8);
  }
  return "models/error.mdl";
}


// 10009a90 FUN_10009a90

void __thiscall FUN_10009a90(void *this,uint param_1)

{
  int *piVar1;
  int iVar2;
  int *piVar3;
  char cVar4;
  undefined4 uVar5;
  
  iVar2 = *(int *)((param_1 & 0xffff) * 0x10 + 0xc + *(int *)((int)this + 0x2c));
  if (((*(ushort *)(iVar2 + 0x18) & 2) != 0) &&
     (*(ushort *)(iVar2 + 0x18) = *(ushort *)(iVar2 + 0x18) & 0xfffd, *(int *)(iVar2 + 4) != 0)) {
    if (*(int *)((int)this + 0x44) != 0) {
      (**(code **)(**(int **)((int)this + 0x44) + 4))(2,param_1);
    }
    if (*(int *)(DAT_1003758c + 0x30) != 0) {
                    /* WARNING: Load size is inaccurate */
      uVar5 = (**(code **)(*this + 0x5c))(param_1);
      Msg("MDLCache: Unload vcollide %s\n",uVar5);
    }
    piVar3 = *(int **)(iVar2 + 4);
    piVar1 = piVar3 + 1;
    LOCK();
    iVar2 = *piVar1;
    *piVar1 = *piVar1 + -1;
    UNLOCK();
    if (iVar2 == 1) {
      cVar4 = (**(code **)(*piVar3 + 4))();
      if (cVar4 != '\0') {
        (**(code **)*piVar3)(1);
      }
    }
  }
  return;
}


// 10009ba0 FUN_10009ba0

undefined4 __thiscall FUN_10009ba0(void *this,uint param_1,undefined4 *param_2)

{
  int iVar1;
  
  if (param_2 != (undefined4 *)0x0) {
    *param_2 = 0;
  }
  if ((short)param_1 == -1) {
    return 0;
  }
                    /* WARNING: Load size is inaccurate */
  iVar1 = (**(code **)(*this + 0x38))(param_1);
  if (iVar1 != 0) {
    if ((param_2 != (undefined4 *)0x0) && (*(int *)(iVar1 + 0xa0) != 0)) {
      *param_2 = *(undefined4 *)(iVar1 + 0x94);
    }
    return *(undefined4 *)(iVar1 + 0xa0);
  }
  iVar1 = *(int *)((param_1 & 0xffff) * 0x10 + 0xc + *(int *)((int)this + 0x2c));
  if (param_2 != (undefined4 *)0x0) {
    *param_2 = *(undefined4 *)(iVar1 + 0x38);
  }
  return *(undefined4 *)(iVar1 + 0x34);
}


// 10009c20 FUN_10009c20

void __thiscall FUN_10009c20(void *this,uint param_1)

{
  int iVar1;
  undefined4 uVar2;
  
  if (((short)param_1 != -1) &&
     (iVar1 = *(int *)((param_1 & 0xffff) * 0x10 + 0xc + *(int *)((int)this + 0x2c)),
     (*(byte *)(iVar1 + 0x18) & 1) != 0)) {
    if (*(int *)((int)this + 0x44) != 0) {
      (**(code **)(**(int **)((int)this + 0x44) + 4))(1,param_1);
    }
    if (*(int *)(DAT_1003758c + 0x30) != 0) {
                    /* WARNING: Load size is inaccurate */
      uVar2 = (**(code **)(*this + 0x5c))(param_1);
      Msg("MDLCache: Unload studiomdl %s\n",uVar2);
    }
    (**(code **)(*DAT_100384ac + 0x2c))((undefined4 *)(iVar1 + 8));
    *(undefined4 *)(iVar1 + 8) = 0;
    *(undefined4 *)(iVar1 + 0xc) = 0;
    *(undefined4 *)(iVar1 + 0x10) = 0;
    *(undefined4 *)(iVar1 + 0x14) = 0;
    *(ushort *)(iVar1 + 0x18) = *(ushort *)(iVar1 + 0x18) & 0xfffe;
  }
  return;
}


// 10009cb0 FUN_10009cb0

longlong __fastcall FUN_10009cb0(int *param_1,uint param_2,uint param_3)

{
  int *piVar1;
  int iVar2;
  undefined4 uVar3;
  int iVar4;
  uint extraout_EDX;
  undefined4 extraout_EDX_00;
  undefined4 extraout_EDX_01;
  int iVar5;
  
  if ((short)param_3 == -1) {
    return (ulonglong)param_2 << 0x20;
  }
  iVar5 = (param_3 & 0xffff) * 0x10;
  iVar2 = *(int *)(param_1[0xb] + 0xc + iVar5);
  if (*(int *)(iVar2 + 0x40) != 0) {
    piVar1 = (int *)(iVar2 + 0x48);
    LOCK();
    iVar5 = *piVar1;
    *piVar1 = *piVar1 + 1;
    UNLOCK();
    return CONCAT44(iVar5,*(undefined4 *)(iVar2 + 0x40));
  }
  (**(code **)(*param_1 + 100))();
  iVar4 = (**(code **)(*param_1 + 0x28))(param_3);
  if (iVar4 == 0) {
    (**(code **)(*param_1 + 0x68))();
    return (ulonglong)extraout_EDX << 0x20;
  }
  if (*(int *)(iVar2 + 0x40) != 0) {
    LOCK();
    *(int *)(iVar2 + 0x48) = *(int *)(iVar2 + 0x48) + 1;
    UNLOCK();
    uVar3 = *(undefined4 *)(iVar2 + 0x40);
    (**(code **)(*param_1 + 0x68))();
    return CONCAT44(extraout_EDX_00,uVar3);
  }
  (**(code **)(*(int *)param_1[4] + 0x28))(**(undefined4 **)(param_1[0xb] + 0xc + iVar5));
  (**(code **)(*param_1 + 0x68))();
  return CONCAT44(extraout_EDX_01,iVar4);
}


// 10009e40 FUN_10009e40

undefined4 __thiscall FUN_10009e40(void *this,int *param_1)

{
  undefined4 *puVar1;
  undefined4 uVar2;
  
  if (2 < *param_1 - 1U) {
    return *param_1 - 1U & 0xffffff00;
  }
  if (*(int *)(DAT_1003758c + 0x30) != 0) {
    uVar2 = (**(code **)(*(int *)((int)this + -0xc) + 0x5c))(*(undefined2 *)((int)param_1 + 10));
    Msg("MDLCache: Data cache discard %s %s\n",(&PTR_s_studiohdr_1002c274)[param_1[2] & 0xffff],
        uVar2);
  }
  puVar1 = (undefined4 *)param_1[2];
  if (((undefined4 *)param_1[3] != puVar1) && (((uint)puVar1 & 0xffff) == 1)) {
    uVar2 = FUN_10009c20((void *)((int)this + -0xc),(uint)puVar1 >> 0x10);
    return CONCAT31((int3)((uint)uVar2 >> 8),1);
  }
  uVar2 = FUN_10005e00((uint)puVar1 & 0xffff,(undefined4 *)param_1[3]);
  return CONCAT31((int3)((uint)uVar2 >> 8),1);
}


// 10009ee0 FUN_10009ee0

void * __thiscall FUN_10009ee0(void *this,uint param_1,int *param_2)

{
  byte *pbVar1;
  uint uVar2;
  int iVar3;
  undefined4 uVar4;
  void *pvVar5;
  void *pvVar6;
  int iStack_4;
  
  uVar2 = (uint)*(ushort *)(param_1 + 0x158);
  if (*(int *)(DAT_1003758c + 0x30) != 0) {
    Msg("MDLCache: Load VVD for %s\n",param_1 + 0xc);
  }
  if (*param_2 == 0x56534449) {
    if (param_2[1] != 4) {
      Warning("Error Vertex File for \'%s\' version %d should be %d\n",param_1 + 0xc,param_2[1],4);
      return (void *)0x0;
    }
    if (param_2[2] != *(int *)(param_1 + 8)) {
      Warning("Error Vertex File for \'%s\' checksum %ld should be %ld\n",param_1 + 0xc,param_2[2],
              *(int *)(param_1 + 8));
      return (void *)0x0;
    }
    if (param_2[3] != 0) {
      pbVar1 = (byte *)(param_1 + 0x179);
      iStack_4 = 0;
      param_1 = (uint)*pbVar1;
      if ((int)(param_2[3] - 1U) <= (int)(uint)*pbVar1) {
        param_1 = param_2[3] - 1U;
      }
      iVar3 = FUN_10005110((int)param_2,param_1,'\x01');
      if (*(int *)(DAT_1003758c + 0x30) != 0) {
                    /* WARNING: Load size is inaccurate */
        uVar4 = (**(code **)(*this + 0x5c))(uVar2);
        Msg("MDLCache: Alloc VVD %s\n",uVar4);
      }
      pvVar5 = (void *)FUN_10005730(5,iVar3);
      (**(code **)(**(int **)((int)this + 0x14) + 0x38))();
      pvVar6 = (void *)(uVar2 << 0x10 | 5);
      if (pvVar5 != (void *)0x0) {
        if (pvVar6 == (void *)0xffffffff) {
          pvVar6 = pvVar5;
        }
        (**(code **)(**(int **)((int)this + 0x14) + 0x18))
                  (pvVar6,pvVar5,iVar3,
                   *(int *)(uVar2 * 0x10 + 0xc + *(int *)((int)this + 0x2c)) + 0x2c);
      }
      FUN_10005140(param_2,pvVar5,param_1,'\x01');
      (**(code **)(**(int **)((int)this + 0x14) + 0x44))();
      FUN_10005a80(&iStack_4);
      return pvVar5;
    }
  }
  else {
    Warning("Error Vertex File for \'%s\' id %d should be %d\n",param_1 + 0xc,*param_2,0x56534449);
  }
  return (void *)0x0;
}


// 1000a110 FUN_1000a110

void __cdecl FUN_1000a110(undefined4 *param_1,int param_2,undefined *param_3)

{
  undefined4 uVar1;
  undefined4 uVar2;
  undefined4 uVar3;
  undefined4 uVar4;
  int iVar5;
  
  for (iVar5 = param_2 - (int)param_1; 1 < iVar5 >> 4; iVar5 = iVar5 + -0x10) {
    uVar1 = *(undefined4 *)((int)param_1 + iVar5 + -0x10);
    uVar2 = *(undefined4 *)((int)param_1 + iVar5 + -0xc);
    uVar3 = *(undefined4 *)((int)param_1 + iVar5 + -8);
    uVar4 = *(undefined4 *)((int)param_1 + iVar5 + -4);
    *(undefined4 *)((int)param_1 + iVar5 + -0x10) = *param_1;
    *(undefined4 *)((int)param_1 + iVar5 + -0xc) = param_1[1];
    *(undefined4 *)((int)param_1 + iVar5 + -8) = param_1[2];
    *(undefined4 *)((int)param_1 + iVar5 + -4) = param_1[3];
    FUN_10005f90((int)param_1,0,iVar5 + -0x10 >> 4,uVar1,uVar2,uVar3,uVar4,param_3);
  }
  return;
}


// 1000a190 FUN_1000a190

void __cdecl FUN_1000a190(undefined4 *param_1,int param_2,undefined *param_3)

{
  int iVar1;
  undefined4 *puVar2;
  
  iVar1 = (param_2 - (int)param_1) / 0x12;
  if (1 < iVar1) {
    puVar2 = (undefined4 *)(param_2 + -0x12);
    if (iVar1 < 2) goto LAB_1000a1fa;
    do {
      FUN_10006360(param_1,(int)puVar2,puVar2,*puVar2,puVar2[1],puVar2[2],puVar2[3],
                   *(undefined2 *)(puVar2 + 4),param_3);
LAB_1000a1fa:
      puVar2 = (undefined4 *)((int)puVar2 + -0x12);
    } while (1 < ((0x12 - (int)param_1) + (int)puVar2) / 0x12);
  }
  return;
}


// 1000a220 FUN_1000a220

void __fastcall FUN_1000a220(void *param_1)

{
  FUN_10007d80(param_1);
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


// 1000a280 FUN_1000a280

void __thiscall FUN_1000a280(void *this,uint param_1,ushort param_2,char param_3)

{
  undefined2 *puVar1;
  undefined2 uVar2;
  
  puVar1 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
  puVar1[2] = param_2;
  puVar1[1] = 0xffff;
  *puVar1 = 0xffff;
  puVar1[3] = 0;
  uVar2 = (undefined2)param_1;
  if (param_2 == 0xffff) {
    *(undefined2 *)((int)this + 0x10) = uVar2;
    FUN_10007e50(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(undefined2 *)((uint)param_2 * 0x10 + *(int *)((int)this + 4)) = uVar2;
    FUN_10007e50(this,param_1);
    return;
  }
  *(undefined2 *)((uint)param_2 * 0x10 + 2 + *(int *)((int)this + 4)) = uVar2;
  FUN_10007e50(this,param_1);
  return;
}


// 1000a2f0 FUN_1000a2f0

void __thiscall FUN_1000a2f0(void *this,ushort param_1)

{
  ushort uVar1;
  short sVar2;
  short *psVar3;
  undefined2 *puVar4;
  ushort *puVar5;
  uint uVar6;
  int iVar7;
  uint uVar8;
  ushort uVar9;
  
  if (param_1 != 0xffff) {
    psVar3 = FUN_10005ee0(this,param_1);
    if ((*psVar3 == -1) || (puVar4 = FUN_10005ee0(this,param_1), puVar4[1] == -1)) {
      uVar8 = (uint)param_1;
    }
    else {
      puVar4 = FUN_10005ee0(this,param_1);
      uVar8 = (uint)(ushort)puVar4[1];
      uVar6 = DAT_10037994;
      while( true ) {
        if ((uVar6 & 1) == 0) {
          uVar6 = uVar6 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar6;
        }
        if ((short)uVar8 == -1) {
          psVar3 = (short *)&DAT_1003798c;
        }
        else {
          psVar3 = (short *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if (*psVar3 == -1) break;
        if ((uVar6 & 1) == 0) {
          uVar6 = uVar6 | 1;
          DAT_1003798c = 0xffff;
          DAT_1003798e = 0xffff;
          DAT_10037990 = 0xffff;
          DAT_10037992 = 1;
          DAT_10037994 = uVar6;
        }
        if ((short)uVar8 == -1) {
          uVar8 = (uint)DAT_1003798c;
        }
        else {
          uVar8 = (uint)*(ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
      }
    }
    uVar9 = (ushort)uVar8;
    psVar3 = FUN_10005ee0(this,uVar9);
    if (*psVar3 == -1) {
      puVar4 = FUN_10005ee0(this,uVar9);
      uVar1 = puVar4[1];
    }
    else {
      puVar5 = FUN_10005ee0(this,uVar9);
      uVar1 = *puVar5;
    }
    if (uVar1 != 0xffff) {
      puVar4 = FUN_10005ee0(this,uVar9);
      *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar4[2];
    }
    if (uVar9 == *(ushort *)((int)this + 0x10)) {
      *(ushort *)((int)this + 0x10) = uVar1;
    }
    else {
      puVar4 = FUN_10005ee0(this,uVar9);
      puVar5 = FUN_10005ee0(this,puVar4[2]);
      if (*puVar5 == uVar9) {
        puVar4 = FUN_10005ee0(this,uVar9);
        *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
      }
      else {
        puVar4 = FUN_10005ee0(this,uVar9);
        *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
      }
    }
    puVar4 = FUN_10005ee0(this,uVar9);
    sVar2 = puVar4[3];
    if (uVar9 != param_1) {
      puVar4 = FUN_10005ee0(this,param_1);
      iVar7 = uVar8 * 0x10;
      *(undefined2 *)(*(int *)((int)this + 4) + 4 + iVar7) = puVar4[2];
      puVar4 = FUN_10005ee0(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + 2 + iVar7) = puVar4[1];
      puVar4 = FUN_10005ee0(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + iVar7) = *puVar4;
      if (param_1 == *(ushort *)((int)this + 0x10)) {
        *(ushort *)((int)this + 0x10) = uVar9;
      }
      else {
        puVar4 = FUN_10005ee0(this,param_1);
        puVar5 = FUN_10005ee0(this,puVar4[2]);
        if (*puVar5 == param_1) {
          puVar4 = FUN_10005ee0(this,param_1);
          *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + *(int *)((int)this + 4)) = uVar9;
        }
        else {
          puVar4 = FUN_10005ee0(this,param_1);
          *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar9;
        }
      }
      psVar3 = FUN_10005ee0(this,uVar9);
      if (*psVar3 != -1) {
        puVar5 = FUN_10005ee0(this,uVar9);
        *(ushort *)((uint)*puVar5 * 0x10 + 4 + *(int *)((int)this + 4)) = uVar9;
      }
      puVar4 = FUN_10005ee0(this,uVar9);
      if (puVar4[1] != -1) {
        puVar4 = FUN_10005ee0(this,uVar9);
        *(ushort *)((uint)(ushort)puVar4[1] * 0x10 + 4 + *(int *)((int)this + 4)) = uVar9;
      }
      puVar4 = FUN_10005ee0(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + 6 + iVar7) = puVar4[3];
    }
    if ((uVar1 != 0xffff) && (sVar2 == 1)) {
      FUN_10008430(this,(uint)uVar1);
    }
    return;
  }
  return;
}


// 1000a5c0 FUN_1000a5c0

void __thiscall FUN_1000a5c0(void *this,ushort param_1)

{
  ushort uVar1;
  short sVar2;
  short *psVar3;
  undefined2 *puVar4;
  ushort *puVar5;
  uint uVar6;
  int iVar7;
  uint uVar8;
  ushort uVar9;
  
  if (param_1 != 0xffff) {
    psVar3 = FUN_10006300(this,param_1);
    if ((*psVar3 == -1) || (puVar4 = FUN_10006300(this,param_1), puVar4[1] == -1)) {
      uVar8 = (uint)param_1;
    }
    else {
      puVar4 = FUN_10006300(this,param_1);
      uVar8 = (uint)(ushort)puVar4[1];
      uVar6 = DAT_100379a0;
      while( true ) {
        if ((uVar6 & 1) == 0) {
          uVar6 = uVar6 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar6;
        }
        if ((short)uVar8 == -1) {
          psVar3 = (short *)&DAT_10037998;
        }
        else {
          psVar3 = (short *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if (*psVar3 == -1) break;
        if ((uVar6 & 1) == 0) {
          uVar6 = uVar6 | 1;
          DAT_10037998 = 0xffff;
          DAT_1003799a = 0xffff;
          DAT_1003799c = 0xffff;
          DAT_1003799e = 1;
          DAT_100379a0 = uVar6;
        }
        if ((short)uVar8 == -1) {
          uVar8 = (uint)DAT_10037998;
        }
        else {
          uVar8 = (uint)*(ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
      }
    }
    uVar9 = (ushort)uVar8;
    psVar3 = FUN_10006300(this,uVar9);
    if (*psVar3 == -1) {
      puVar4 = FUN_10006300(this,uVar9);
      uVar1 = puVar4[1];
    }
    else {
      puVar5 = FUN_10006300(this,uVar9);
      uVar1 = *puVar5;
    }
    if (uVar1 != 0xffff) {
      puVar4 = FUN_10006300(this,uVar9);
      *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar4[2];
    }
    if (uVar9 == *(ushort *)((int)this + 0x10)) {
      *(ushort *)((int)this + 0x10) = uVar1;
    }
    else {
      puVar4 = FUN_10006300(this,uVar9);
      puVar5 = FUN_10006300(this,puVar4[2]);
      if (*puVar5 == uVar9) {
        puVar4 = FUN_10006300(this,uVar9);
        *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
      }
      else {
        puVar4 = FUN_10006300(this,uVar9);
        *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
      }
    }
    puVar4 = FUN_10006300(this,uVar9);
    sVar2 = puVar4[3];
    if (uVar9 != param_1) {
      puVar4 = FUN_10006300(this,param_1);
      iVar7 = uVar8 * 0x10;
      *(undefined2 *)(*(int *)((int)this + 4) + 4 + iVar7) = puVar4[2];
      puVar4 = FUN_10006300(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + 2 + iVar7) = puVar4[1];
      puVar4 = FUN_10006300(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + iVar7) = *puVar4;
      if (param_1 == *(ushort *)((int)this + 0x10)) {
        *(ushort *)((int)this + 0x10) = uVar9;
      }
      else {
        puVar4 = FUN_10006300(this,param_1);
        puVar5 = FUN_10006300(this,puVar4[2]);
        if (*puVar5 == param_1) {
          puVar4 = FUN_10006300(this,param_1);
          *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + *(int *)((int)this + 4)) = uVar9;
        }
        else {
          puVar4 = FUN_10006300(this,param_1);
          *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar9;
        }
      }
      psVar3 = FUN_10006300(this,uVar9);
      if (*psVar3 != -1) {
        puVar5 = FUN_10006300(this,uVar9);
        *(ushort *)((uint)*puVar5 * 0x10 + 4 + *(int *)((int)this + 4)) = uVar9;
      }
      puVar4 = FUN_10006300(this,uVar9);
      if (puVar4[1] != -1) {
        puVar4 = FUN_10006300(this,uVar9);
        *(ushort *)((uint)(ushort)puVar4[1] * 0x10 + 4 + *(int *)((int)this + 4)) = uVar9;
      }
      puVar4 = FUN_10006300(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + 6 + iVar7) = puVar4[3];
    }
    if ((uVar1 != 0xffff) && (sVar2 == 1)) {
      FUN_10008ee0(this,(uint)uVar1);
    }
    return;
  }
  return;
}


// 1000a890 FUN_1000a890

undefined4 __cdecl FUN_1000a890(ushort param_1,uint param_2,uint param_3)

{
  ushort uVar1;
  uint local_8 [2];
  
  local_8[0] = ((uint)param_1 * 8 | param_2) << 0xd | param_3;
  uVar1 = FUN_10007720(&PTR_LAB_1002c2ac,local_8);
  if (uVar1 == 0xffff) {
    return DAT_1002c2a8;
  }
  return *(undefined4 *)((uint)uVar1 * 0x10 + 0xc + DAT_1002c2b0);
}


// 1000a8f0 FUN_1000a8f0

void __fastcall FUN_1000a8f0(void *param_1)

{
  FUN_10007c50(param_1);
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


// 1000a950 FUN_1000a950

int __thiscall FUN_1000a950(void *this,int param_1)

{
  int iVar1;
  void *pvVar2;
  undefined4 *puVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_100061d0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar2 = (void *)(*this + param_1 * 0x90);
    FUN_10016630((void *)((int)pvVar2 + 0x90),pvVar2,iVar1 * 0x90);
  }
                    /* WARNING: Load size is inaccurate */
  puVar3 = (undefined4 *)(param_1 * 0x90 + *this);
  if (puVar3 != (undefined4 *)0x0) {
    FUN_10009920(puVar3);
  }
  return param_1;
}


// 1000a9c0 FUN_1000a9c0

void __fastcall FUN_1000a9c0(void *param_1)

{
  ushort uVar1;
  
  for (uVar1 = FUN_100079f0((int)param_1); uVar1 != 0xffff; uVar1 = FUN_10007aa0(param_1,uVar1)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)((uint)uVar1 * 0x10 + 8 + *(int *)((int)param_1 + 4)));
  }
  FUN_10007d80(param_1);
  return;
}


// 1000aa10 FUN_1000aa10

undefined4 __thiscall FUN_1000aa10(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  ushort uVar3;
  void *local_4;
  
  puVar2 = param_1;
  local_4 = this;
  FUN_100067d0(this,param_1,(ushort *)&local_4,(undefined1 *)&param_1);
  uVar3 = FUN_10006530((int)this);
  FUN_10008e70(this,(uint)uVar3,(ushort)local_4,(char)param_1);
  *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + 1;
  puVar1 = (undefined4 *)((uint)uVar3 * 0x10 + 8 + *(int *)((int)this + 4));
  if (puVar1 != (undefined4 *)0x0) {
    *puVar1 = *puVar2;
    puVar1[1] = puVar2[1];
  }
  return CONCAT22((short)((uint)puVar1 >> 0x10),uVar3);
}


// 1000aa80 FUN_1000aa80

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_1000aa80(int param_1)

{
  int *piVar1;
  char cVar2;
  undefined8 uVar3;
  undefined8 uVar4;
  int iVar5;
  int iVar6;
  undefined4 extraout_ECX;
  undefined4 extraout_ECX_00;
  undefined4 extraout_ECX_01;
  undefined4 extraout_ECX_02;
  undefined4 extraout_ECX_03;
  undefined4 extraout_ECX_04;
  int iVar7;
  undefined4 extraout_EDX;
  undefined4 extraout_EDX_00;
  int iVar8;
  int iVar9;
  undefined4 *puVar10;
  int iVar11;
  int iVar12;
  undefined4 *puVar13;
  int iVar14;
  ulonglong uVar15;
  int local_58;
  int local_50;
  int local_4c;
  int local_48;
  int local_44;
  int local_40;
  int local_3c;
  int local_38;
  int local_34;
  
  local_38 = 0;
  if (0 < *(int *)(param_1 + 0xe8)) {
    local_34 = 0;
    do {
      iVar7 = local_34 + param_1 + *(int *)(param_1 + 0xec);
      local_3c = 0;
      if (0 < *(int *)(iVar7 + 4)) {
        local_48 = 0;
        do {
          local_40 = 0;
          iVar5 = *(int *)(iVar7 + 0xc) + local_48 + iVar7;
          if (0 < *(int *)(iVar5 + 0x48)) {
            local_4c = 0;
            do {
              local_44 = 0;
              iVar6 = *(int *)(iVar5 + 0x4c) + local_4c + iVar5;
              if (0 < *(int *)(iVar6 + 0x10)) {
                local_50 = 0;
                do {
                  iVar8 = 0;
                  iVar14 = *(int *)(iVar6 + 0x14) + local_50 + iVar6;
                  cVar2 = *(char *)(iVar14 + 0x20);
                  iVar9 = 0;
                  if (0 < *(int *)(iVar14 + 0x14)) {
                    local_58 = 0;
                    do {
                      iVar11 = local_58;
                      if (cVar2 == '\x01') {
                        iVar11 = iVar8;
                      }
                      iVar11 = *(int *)(iVar14 + 0x18) + iVar11;
                      iVar12 = iVar11 + iVar14;
                      FUN_10004d80((uint)*(ushort *)(iVar11 + 4 + iVar14));
                      FUN_10004d80((uint)*(ushort *)(iVar12 + 6));
                      FUN_10004d80((uint)*(ushort *)(iVar12 + 8));
                      uVar15 = FUN_10017cc0(extraout_ECX,extraout_EDX);
                      *(short *)(iVar12 + 4) = (short)uVar15;
                      uVar15 = FUN_10017cc0(extraout_ECX_00,(int)(uVar15 >> 0x20));
                      *(short *)(iVar12 + 6) = (short)uVar15;
                      uVar15 = FUN_10017cc0(extraout_ECX_01,(int)(uVar15 >> 0x20));
                      *(short *)(iVar12 + 8) = (short)uVar15;
                      FUN_10004d80((uint)*(ushort *)(iVar12 + 10));
                      FUN_10004d80((uint)*(ushort *)(iVar12 + 0xc));
                      FUN_10004d80((uint)*(ushort *)(iVar12 + 0xe));
                      uVar15 = FUN_10017cc0(extraout_ECX_02,extraout_EDX_00);
                      *(short *)(iVar12 + 10) = (short)uVar15;
                      uVar15 = FUN_10017cc0(extraout_ECX_03,(int)(uVar15 >> 0x20));
                      *(short *)(iVar12 + 0xc) = (short)uVar15;
                      uVar15 = FUN_10017cc0(extraout_ECX_04,(int)(uVar15 >> 0x20));
                      local_58 = local_58 + 0x10;
                      iVar9 = iVar9 + 1;
                      *(short *)(iVar12 + 0xe) = (short)uVar15;
                      iVar8 = iVar8 + 0x12;
                    } while (iVar9 < *(int *)(iVar14 + 0x14));
                  }
                  uVar3 = rdtsc();
                  iVar8 = *(int *)g_pLastMiniProfiler_exref;
                  *(undefined **)g_pLastMiniProfiler_exref = &DAT_100375b8;
                  if (*(char *)(iVar14 + 0x20) == '\0') {
                    iVar9 = *(int *)(iVar14 + 0x14);
                    puVar10 = (undefined4 *)(*(int *)(iVar14 + 0x18) + iVar14);
                    iVar14 = (int)(puVar10 + iVar9 * 4) - (int)puVar10;
                    iVar11 = iVar14 >> 4;
                    if ((1 < iVar11) && (iVar14 = iVar11 - (iVar14 >> 0x1f) >> 1, 0 < iVar14)) {
                      puVar13 = puVar10 + iVar14 * 4;
                      do {
                        iVar14 = iVar14 + -1;
                        FUN_10005f90((int)puVar10,iVar14,iVar11,puVar13[-4],puVar13[-3],puVar13[-2],
                                     puVar13[-1],&LAB_10005500);
                        puVar13 = puVar13 + -4;
                      } while (0 < iVar14);
                    }
                    FUN_1000a110(puVar10,(int)(puVar10 + iVar9 * 4),&LAB_10005500);
                  }
                  else if (*(char *)(iVar14 + 0x20) == '\x01') {
                    puVar10 = (undefined4 *)(*(int *)(iVar14 + 0x18) + iVar14);
                    iVar9 = (int)puVar10 + *(int *)(iVar14 + 0x14) * 0x12;
                    iVar14 = (iVar9 - (int)puVar10) / 0x12;
                    if ((1 < iVar14) && (iVar11 = iVar14 / 2, 0 < iVar11)) {
                      iVar12 = (int)puVar10 + iVar11 * 0x12;
                      do {
                        iVar11 = iVar11 + -1;
                        FUN_10006060((int)puVar10,iVar11,iVar14,*(undefined4 *)(iVar12 + -0x12),
                                     *(undefined4 *)(iVar12 + -0xe),*(undefined4 *)(iVar12 + -10),
                                     *(undefined4 *)(iVar12 + -6),*(undefined2 *)(iVar12 + -2),
                                     &LAB_10005500);
                        iVar12 = iVar12 + -0x12;
                      } while (0 < iVar11);
                    }
                    FUN_1000a190(puVar10,iVar9,&LAB_10005500);
                  }
                  _DAT_100375bc = _DAT_100375bc + 1;
                  local_50 = local_50 + 0x3c;
                  uVar4 = rdtsc();
                  iVar9 = (int)uVar4 - (int)uVar3;
                  _DAT_100375b8 = _DAT_100375b8 + iVar9;
                  piVar1 = (int *)(iVar8 + 8);
                  *piVar1 = *piVar1 + iVar9;
                  *(int *)g_pLastMiniProfiler_exref = iVar8;
                  local_44 = local_44 + 1;
                } while (local_44 < *(int *)(iVar6 + 0x10));
              }
              local_4c = local_4c + 0x74;
              local_40 = local_40 + 1;
            } while (local_40 < *(int *)(iVar5 + 0x48));
          }
          local_48 = local_48 + 0x94;
          local_3c = local_3c + 1;
        } while (local_3c < *(int *)(iVar7 + 4));
      }
      local_38 = local_38 + 1;
      local_34 = local_34 + 0x10;
    } while (local_38 < *(int *)(param_1 + 0xe8));
  }
  return;
}


// 1000ae00 FUN_1000ae00

void __fastcall FUN_1000ae00(void *param_1)

{
  char cVar1;
  ushort uVar2;
  int iVar3;
  
  *(byte *)((int)param_1 + 0x84) = *(byte *)((int)param_1 + 0x84) | 1;
  cVar1 = (**(code **)(**(int **)((int)param_1 + 0x14) + 0x3c))();
  if (cVar1 != '\0') {
    *(undefined4 *)((int)param_1 + 0x20) = 0;
    do {
      *(int *)((int)param_1 + 0x20) = *(int *)((int)param_1 + 0x20) + 1;
      iVar3 = (**(code **)(**(int **)((int)param_1 + 0x14) + 0x44))();
    } while (iVar3 != 0);
  }
  cVar1 = (**(code **)(**(int **)((int)param_1 + 0x18) + 0x3c))();
  if (cVar1 != '\0') {
    *(undefined4 *)((int)param_1 + 0x24) = 0;
    do {
      *(int *)((int)param_1 + 0x24) = *(int *)((int)param_1 + 0x24) + 1;
      iVar3 = (**(code **)(**(int **)((int)param_1 + 0x18) + 0x44))();
    } while (iVar3 != 0);
  }
  for (uVar2 = FUN_100079f0((int)param_1 + 0x28); uVar2 != 0xffff;
      uVar2 = FUN_10007aa0((void *)((int)param_1 + 0x28),uVar2)) {
    FUN_10009c20(param_1,(uint)uVar2);
  }
  FUN_100055d0((int)param_1);
  return;
}


// 1000aea0 FUN_1000aea0

void * __thiscall FUN_1000aea0(void *this,uint param_1,int param_2)

{
  int *piVar1;
  undefined4 uVar2;
  int iVar3;
  undefined4 extraout_ECX;
  uint uVar4;
  void *pvVar5;
  undefined4 *unaff_ESI;
  void *unaff_EDI;
  int iVar6;
  void *_Dst;
  int unaff_retaddr;
  
  if (0 < *(int *)(DAT_1003770c + 0x30)) {
    FUN_10004fe0(param_2,*(int *)(DAT_1003770c + 0x30));
  }
  FUN_10005300();
  if (*(int *)(param_2 + 0x150) == 0) {
    FUN_10005370(extraout_ECX,param_2);
  }
  uVar4 = param_1 & 0xffff;
  *(uint *)(param_2 + 0x158) = uVar4;
  if (*(int *)(DAT_1003758c + 0x30) != 0) {
                    /* WARNING: Load size is inaccurate */
    uVar2 = (**(code **)(*this + 0x5c))(param_1);
    Msg("MDLCache: Alloc studiohdr %s\n",uVar2);
  }
  iVar6 = *(int *)(param_2 + 0x4c) + 0x23;
  iVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar6);
  if (iVar3 != 0) {
    _Dst = (void *)(iVar3 + 0x23U & 0xffffffe0);
    *(int *)((int)_Dst + -4) = iVar3;
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x5c))(unaff_retaddr);
    pvVar5 = (void *)(uVar4 * 0x10000);
    if ((void *)(uVar4 * 0x10000) == (void *)0xffffffff) {
      pvVar5 = _Dst;
    }
    (**(code **)(**(int **)((int)this + 0x10) + 0x18))
              (pvVar5,_Dst,*(undefined4 *)(unaff_retaddr + 0x4c),*unaff_ESI);
    if (*(int *)(DAT_1003783c + 0x30) != 0) {
      piVar1 = (int *)(*(int *)((int)this + 0x2c) + 0xc + uVar4 * 0x10);
      uVar2 = (**(code **)(**(int **)((int)this + 0x10) + 0x28))(*(undefined4 *)*piVar1);
      *(undefined4 *)(*piVar1 + 0x40) = uVar2;
    }
    _memcpy(_Dst,unaff_EDI,*(size_t *)((int)unaff_EDI + 0x4c));
    if ((*(uint *)((int)_Dst + 0x98) & 0x4000) == 0) {
      FUN_1000aa80((int)_Dst);
      *(uint *)((int)_Dst + 0x98) = *(uint *)((int)_Dst + 0x98) | 0x4000;
    }
    if (*(int *)((int)this + 0x44) != 0) {
      (**(code **)**(undefined4 **)((int)this + 0x44))(0,iVar6);
    }
    return _Dst;
  }
  Error("CMDLCache:: Out of memory");
  return (void *)0x0;
}


// 1000b010 FUN_1000b010

void __thiscall FUN_1000b010(void *this,undefined4 param_1)

{
  ushort uVar1;
  
  for (uVar1 = FUN_100079f0((int)this + 0x28); uVar1 != 0xffff;
      uVar1 = FUN_10007aa0((void *)((int)this + 0x28),uVar1)) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x54))(uVar1,param_1);
  }
  return;
}


// 1000b060 FUN_1000b060

void __fastcall FUN_1000b060(void *param_1)

{
  undefined4 *puVar1;
  char cVar2;
  ushort uVar3;
  int iVar4;
  int *piVar5;
  int *piVar6;
  
  cVar2 = (**(code **)(**(int **)((int)param_1 + 0x10) + 0x3c))();
  if (cVar2 != '\0') {
    *(undefined4 *)((int)param_1 + 0x1c) = 0;
    do {
      *(int *)((int)param_1 + 0x1c) = *(int *)((int)param_1 + 0x1c) + 1;
      iVar4 = (**(code **)(**(int **)((int)param_1 + 0x10) + 0x44))();
    } while (iVar4 != 0);
  }
  cVar2 = (**(code **)(**(int **)((int)param_1 + 0x14) + 0x3c))();
  if (cVar2 != '\0') {
    *(undefined4 *)((int)param_1 + 0x20) = 0;
    do {
      *(int *)((int)param_1 + 0x20) = *(int *)((int)param_1 + 0x20) + 1;
      iVar4 = (**(code **)(**(int **)((int)param_1 + 0x14) + 0x44))();
    } while (iVar4 != 0);
  }
  cVar2 = (**(code **)(**(int **)((int)param_1 + 0x18) + 0x3c))();
  if (cVar2 != '\0') {
    *(undefined4 *)((int)param_1 + 0x24) = 0;
    do {
      *(int *)((int)param_1 + 0x24) = *(int *)((int)param_1 + 0x24) + 1;
      iVar4 = (**(code **)(**(int **)((int)param_1 + 0x18) + 0x44))();
    } while (iVar4 != 0);
  }
  for (uVar3 = FUN_100079f0((int)param_1 + 0x28); uVar3 != 0xffff;
      uVar3 = FUN_10007aa0((void *)((int)param_1 + 0x28),uVar3)) {
    puVar1 = *(undefined4 **)((uint)uVar3 * 0x10 + 0xc + *(int *)((int)param_1 + 0x2c));
    if (puVar1[0x10] != 0) {
      piVar6 = puVar1 + 0x12;
      iVar4 = puVar1[0x12];
      while (0 < iVar4) {
        LOCK();
        *piVar6 = *piVar6 + -1;
        UNLOCK();
        piVar5 = (int *)FUN_100053c0(param_1,0);
        (**(code **)(*piVar5 + 0x28))(*puVar1);
        iVar4 = *piVar6;
      }
      iVar4 = *piVar6;
      while (iVar4 < 0) {
        LOCK();
        *piVar6 = *piVar6 + 1;
        UNLOCK();
        piVar5 = (int *)FUN_100053c0(param_1,0);
        (**(code **)(*piVar5 + 0x2c))(*puVar1);
        iVar4 = *piVar6;
      }
      piVar6 = (int *)FUN_100053c0(param_1,0);
      (**(code **)(*piVar6 + 0x2c))(*puVar1);
      puVar1[0x10] = 0;
    }
    if (puVar1[0x11] != 0) {
      piVar6 = (int *)FUN_100053c0(param_1,5);
      (**(code **)(*piVar6 + 0x2c))(puVar1[0xb]);
      puVar1[0x11] = 0;
    }
  }
  return;
}


// 1000b3b0 FUN_1000b3b0

undefined4 __thiscall FUN_1000b3b0(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  ushort uVar3;
  void *local_4;
  
  puVar2 = param_1;
  local_4 = this;
  FUN_10006420(this,param_1,(ushort *)&local_4,(undefined1 *)&param_1);
  uVar3 = FUN_10006530((int)this);
  FUN_1000a280(this,(uint)uVar3,(ushort)local_4,(char)param_1);
  *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + 1;
  puVar1 = (undefined4 *)((uint)uVar3 * 0x10 + 8 + *(int *)((int)this + 4));
  if (puVar1 != (undefined4 *)0x0) {
    *puVar1 = *puVar2;
    puVar1[1] = puVar2[1];
  }
  return CONCAT22((short)((uint)puVar1 >> 0x10),uVar3);
}


// 1000b420 FUN_1000b420

undefined4 * __thiscall FUN_1000b420(void *this,undefined4 *param_1)

{
  int iVar1;
  undefined4 *puVar2;
  
  puVar2 = (undefined4 *)FUN_10006650(this,'\0');
  if (puVar2 == (undefined4 *)0x0) {
    return (undefined4 *)0x0;
  }
  FUN_10007d20(this,puVar2);
  puVar2[5] = 0;
  iVar1 = *(int *)((int)this + 0x10);
  puVar2[4] = iVar1;
  *(undefined4 **)((int)this + 0x10) = puVar2;
  if (iVar1 == 0) {
    *(undefined4 **)((int)this + 0xc) = puVar2;
  }
  else {
    *(undefined4 **)(iVar1 + 0x14) = puVar2;
  }
  *(int *)((int)this + 0x18) = *(int *)((int)this + 0x18) + 1;
  *puVar2 = *param_1;
  puVar2[1] = param_1[1];
  puVar2[2] = param_1[2];
  puVar2[3] = param_1[3];
  return puVar2;
}


// 1000b480 FUN_1000b480

void __fastcall FUN_1000b480(int *param_1)

{
  int iVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  
  iVar2 = param_1[3] + -1;
  if (-1 < iVar2) {
    iVar3 = iVar2 * 0x90;
    do {
      iVar4 = *param_1 + iVar3;
      *(undefined4 *)(iVar4 + 0x88) = 0;
      if (-1 < *(int *)(iVar4 + 0x84)) {
        if (*(int *)(iVar4 + 0x7c) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(iVar4 + 0x7c));
          *(undefined4 *)(iVar4 + 0x7c) = 0;
        }
        *(undefined4 *)(iVar4 + 0x80) = 0;
      }
      iVar1 = *(int *)(iVar4 + 0x7c);
      *(int *)(iVar4 + 0x8c) = iVar1;
      if (-1 < *(int *)(iVar4 + 0x84)) {
        if (iVar1 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
          *(undefined4 *)(iVar4 + 0x7c) = 0;
        }
        *(undefined4 *)(iVar4 + 0x80) = 0;
      }
      *(undefined4 *)(iVar4 + 0x74) = 0;
      if (-1 < *(int *)(iVar4 + 0x70)) {
        if (*(int *)(iVar4 + 0x68) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(iVar4 + 0x68));
          *(undefined4 *)(iVar4 + 0x68) = 0;
        }
        *(undefined4 *)(iVar4 + 0x6c) = 0;
      }
      iVar1 = *(int *)(iVar4 + 0x68);
      *(int *)(iVar4 + 0x78) = iVar1;
      if (-1 < *(int *)(iVar4 + 0x70)) {
        if (iVar1 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
          *(undefined4 *)(iVar4 + 0x68) = 0;
        }
        *(undefined4 *)(iVar4 + 0x6c) = 0;
      }
      *(undefined4 *)(iVar4 + 0x60) = 0;
      if (-1 < *(int *)(iVar4 + 0x5c)) {
        if (*(int *)(iVar4 + 0x54) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(iVar4 + 0x54));
          *(undefined4 *)(iVar4 + 0x54) = 0;
        }
        *(undefined4 *)(iVar4 + 0x58) = 0;
      }
      iVar1 = *(int *)(iVar4 + 0x54);
      *(int *)(iVar4 + 100) = iVar1;
      if (-1 < *(int *)(iVar4 + 0x5c)) {
        if (iVar1 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
          *(undefined4 *)(iVar4 + 0x54) = 0;
        }
        *(undefined4 *)(iVar4 + 0x58) = 0;
      }
      *(undefined4 *)(iVar4 + 0x4c) = 0;
      if (-1 < *(int *)(iVar4 + 0x48)) {
        if (*(int *)(iVar4 + 0x40) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(iVar4 + 0x40));
          *(undefined4 *)(iVar4 + 0x40) = 0;
        }
        *(undefined4 *)(iVar4 + 0x44) = 0;
      }
      iVar1 = *(int *)(iVar4 + 0x40);
      *(int *)(iVar4 + 0x50) = iVar1;
      if (-1 < *(int *)(iVar4 + 0x48)) {
        if (iVar1 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
          *(undefined4 *)(iVar4 + 0x40) = 0;
        }
        *(undefined4 *)(iVar4 + 0x44) = 0;
      }
      *(undefined4 *)(iVar4 + 0x38) = 0;
      if (-1 < *(int *)(iVar4 + 0x34)) {
        if (*(int *)(iVar4 + 0x2c) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(iVar4 + 0x2c));
          *(undefined4 *)(iVar4 + 0x2c) = 0;
        }
        *(undefined4 *)(iVar4 + 0x30) = 0;
      }
      iVar1 = *(int *)(iVar4 + 0x2c);
      *(int *)(iVar4 + 0x3c) = iVar1;
      if (-1 < *(int *)(iVar4 + 0x34)) {
        if (iVar1 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
          *(undefined4 *)(iVar4 + 0x2c) = 0;
        }
        *(undefined4 *)(iVar4 + 0x30) = 0;
      }
      *(undefined4 *)(iVar4 + 0x24) = 0;
      if (-1 < *(int *)(iVar4 + 0x20)) {
        if (*(int *)(iVar4 + 0x18) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(iVar4 + 0x18));
          *(undefined4 *)(iVar4 + 0x18) = 0;
        }
        *(undefined4 *)(iVar4 + 0x1c) = 0;
      }
      iVar1 = *(int *)(iVar4 + 0x18);
      *(int *)(iVar4 + 0x28) = iVar1;
      if (-1 < *(int *)(iVar4 + 0x20)) {
        if (iVar1 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
          *(undefined4 *)(iVar4 + 0x18) = 0;
        }
        *(undefined4 *)(iVar4 + 0x1c) = 0;
      }
      *(undefined4 *)(iVar4 + 0x10) = 0;
      if (-1 < *(int *)(iVar4 + 0xc)) {
        if (*(int *)(iVar4 + 4) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(iVar4 + 4));
          *(undefined4 *)(iVar4 + 4) = 0;
        }
        *(undefined4 *)(iVar4 + 8) = 0;
      }
      iVar1 = *(int *)(iVar4 + 4);
      *(int *)(iVar4 + 0x14) = iVar1;
      if (-1 < *(int *)(iVar4 + 0xc)) {
        if (iVar1 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
          *(undefined4 *)(iVar4 + 4) = 0;
        }
        *(undefined4 *)(iVar4 + 8) = 0;
      }
      iVar2 = iVar2 + -1;
      iVar3 = iVar3 + -0x90;
    } while (-1 < iVar2);
    param_1[3] = 0;
    return;
  }
  param_1[3] = 0;
  return;
}


// 1000b870 FUN_1000b870

uint __thiscall FUN_1000b870(void *this,undefined4 *param_1)

{
  ushort uVar1;
  undefined2 extraout_var;
  int iVar2;
  undefined4 local_8 [2];
  
  local_8[0] = *param_1;
  uVar1 = FUN_10007720(this,local_8);
  if (uVar1 != 0xffff) {
    FUN_1000a2f0(this,uVar1);
    iVar2 = (uint)uVar1 * 0x10;
    *(ushort *)(iVar2 + *(int *)((int)this + 4)) = uVar1;
    *(undefined2 *)(*(int *)((int)this + 4) + 2 + iVar2) = *(undefined2 *)((int)this + 0x14);
    *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + -1;
    *(ushort *)((int)this + 0x14) = uVar1;
    return CONCAT31((int3)((uint)iVar2 >> 8),1);
  }
  return CONCAT22(extraout_var,uVar1) & 0xffffff00;
}


// 1000b8e0 FUN_1000b8e0

void __thiscall FUN_1000b8e0(void *this,ushort param_1)

{
  int iVar1;
  
  iVar1 = (uint)param_1 * 0x10;
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
            (*(undefined4 *)(*(int *)((int)this + 4) + 8 + iVar1));
  if (param_1 != 0xffff) {
    FUN_1000a5c0(this,param_1);
    *(ushort *)(iVar1 + *(int *)((int)this + 4)) = param_1;
    *(undefined2 *)(*(int *)((int)this + 4) + 2 + iVar1) = *(undefined2 *)((int)this + 0x14);
    *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + -1;
    *(ushort *)((int)this + 0x14) = param_1;
  }
  return;
}


// 1000b940 FUN_1000b940

int __cdecl FUN_1000b940(ushort param_1,uint param_2,uint param_3,int param_4)

{
  int iVar1;
  uint local_8;
  int local_4;
  
  iVar1 = param_4;
  _param_1 = ((uint)param_1 * 8 | param_2) << 0xd | param_3;
  if (param_4 == DAT_1002c2a8) {
    FUN_1000b870(&PTR_LAB_1002c2ac,(undefined4 *)&param_1);
    return iVar1;
  }
  local_4 = param_4;
  local_8 = _param_1;
  FUN_1000b3b0(&PTR_LAB_1002c2ac,&local_8);
  return iVar1;
}


// 1000b9a0 FUN_1000b9a0

void __cdecl FUN_1000b9a0(ushort param_1,uint param_2,int param_3)

{
  FUN_1000b940(param_1,param_2,0,param_3);
  return;
}


// 1000ba10 FUN_1000ba10

undefined4 * __fastcall FUN_1000ba10(undefined4 *param_1)

{
  *(undefined1 *)(param_1 + 1) = 0;
  param_1[2] = &PTR_LAB_100258e0;
  param_1[3] = &PTR_LAB_10025900;
  *param_1 = &PTR_FUN_10025c4c;
  param_1[2] = &PTR_LAB_10025c28;
  param_1[3] = &PTR_FUN_10025c1c;
  param_1[10] = 0;
  param_1[0xb] = 0;
  param_1[0xc] = 0;
  param_1[0xd] = 0;
  *(undefined2 *)(param_1 + 0xe) = 0xffff;
  *(undefined2 *)(param_1 + 0xf) = 0xffff;
  *(undefined2 *)((int)param_1 + 0x3a) = 0;
  *(undefined2 *)((int)param_1 + 0x3e) = 0xffff;
  param_1[0x10] = param_1[0xb];
  if (param_1[10] == 0) {
    param_1[10] = &LAB_10004d30;
  }
  param_1[0x12] = 0;
  param_1[0x13] = 0;
  param_1[0x14] = 0;
  FUN_10005f40(param_1 + 0x12,0,0);
  param_1[0x1a] = 0;
  param_1[0x1b] = 0xffffffff;
  param_1[0x15] = 0;
  param_1[0x16] = 0;
  param_1[0x17] = 0;
  param_1[0x18] = 0;
  param_1[0x19] = 0;
  param_1[0x1c] = 0;
  param_1[0x1d] = 0;
  param_1[0x1e] = 0;
  param_1[0x1f] = 0;
  param_1[0x20] = 0;
  *(byte *)(param_1 + 0x21) = *(byte *)(param_1 + 0x21) & 0xf8;
  param_1[0x11] = 0;
  param_1[4] = 0;
  param_1[5] = 0;
  param_1[6] = 0;
  param_1[7] = 0;
  param_1[8] = 0;
  param_1[9] = 0;
  return param_1;
}


// 1000bb20 FUN_1000bb20

uint __thiscall FUN_1000bb20(void *this,char *param_1)

{
  char cVar1;
  ushort uVar2;
  void *_Dst;
  undefined2 extraout_var;
  uint uVar3;
  int aiStack_114 [2];
  char *local_10c [2];
  char local_104 [260];
  
  FUN_10016660(local_104,param_1,0x104);
  FUN_10016400(local_104,'/');
  if (DAT_10038540 != (int *)0x0) {
    cVar1 = (**(code **)(*DAT_10038540 + 0x24))(2,local_104);
    if (cVar1 == '\0') {
      FUN_10016660(local_104,"models/error.mdl",0x104);
    }
  }
  local_10c[0] = local_104;
  uVar2 = FUN_100078f0((void *)((int)this + 0x28),local_10c);
  uVar3 = (uint)uVar2;
  if (uVar2 == 0xffff) {
    aiStack_114[0] = FUN_10004cd0(local_104);
    aiStack_114[1] = 0;
    uVar3 = FUN_1000aa10((void *)((int)this + 0x28),aiStack_114);
    uVar3 = uVar3 & 0xffff;
    _Dst = (void *)FUN_10001660(&DAT_1003749c,0x54);
    if (_Dst == (void *)0x0) {
      _Dst = (void *)0x0;
    }
    else {
      *(undefined4 *)((int)_Dst + 0x48) = 0;
      *(undefined4 *)((int)_Dst + 0x4c) = 0;
      *(undefined4 *)((int)_Dst + 0x50) = 0;
    }
    *(void **)(uVar3 * 0x10 + 0xc + *(int *)((int)this + 0x2c)) = _Dst;
    _memset(_Dst,0,0x54);
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0x1c))(uVar3);
  return CONCAT22(extraout_var,(short)uVar3);
}


// 1000bc80 FUN_1000bc80

uint __thiscall FUN_1000bc80(void *this,ushort param_1,uint param_2,uint param_3,char param_4)

{
  int *piVar1;
  void *pvVar2;
  int *piVar3;
  DWORD DVar4;
  int iVar5;
  CThreadFastMutex *this_00;
  undefined4 *puStack_c;
  void *local_8 [2];
  
  local_8[0] = this;
  piVar3 = (int *)FUN_1000a890(param_1,param_2,param_3);
  if (piVar3 == DAT_1002c2a8) {
    return (uint)piVar3 & 0xffffff00;
  }
  this_00 = (CThreadFastMutex *)((int)this + 0x7c);
  DVar4 = GetCurrentThreadId();
  if (DVar4 == *(DWORD *)this_00) {
LAB_1000bcd3:
    *(int *)((int)this + 0x80) = *(int *)((int)this + 0x80) + 1;
  }
  else {
    LOCK();
    iVar5 = *(int *)this_00;
    if (iVar5 == 0) {
      *(DWORD *)this_00 = DVar4;
      iVar5 = 0;
    }
    UNLOCK();
    if (iVar5 == 0) goto LAB_1000bcd3;
    CThreadFastMutex::Lock(this_00,DVar4,0);
  }
  piVar1 = (int *)((int)this + 0x80);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  if (*piVar3 != 0) {
    if (param_4 != '\0') {
      (**(code **)(*DAT_10038510 + 0xcc))(*piVar3);
      iVar5 = (**(code **)(*DAT_10038510 + 200))(*piVar3,&stack0xfffffff0,local_8);
      if (iVar5 == 0) {
        if (param_2 == 3) {
          FUN_10005ab0(puStack_c);
        }
        else {
          (**(code **)(*DAT_10038510 + 0x158))(puStack_c);
        }
      }
    }
    (**(code **)(*DAT_10038510 + 0xdc))(*piVar3);
    *piVar3 = 0;
  }
  FUN_1000b940(param_1,param_2,param_3,(int)DAT_1002c2a8);
  DVar4 = GetCurrentThreadId();
  if (DVar4 != *(DWORD *)this_00) {
    LOCK();
    iVar5 = *(int *)this_00;
    if (iVar5 == 0) {
      *(DWORD *)this_00 = DVar4;
      iVar5 = 0;
    }
    UNLOCK();
    if (iVar5 != 0) {
      CThreadFastMutex::Lock(this_00,DVar4,0);
      goto LAB_1000bdc3;
    }
  }
  *(int *)((int)this + 0x80) = *(int *)((int)this + 0x80) + 1;
LAB_1000bdc3:
  pvVar2 = local_8[0];
  FUN_10007d20((void *)((int)local_8[0] + 0x48),piVar3);
  iVar5 = *(int *)((int)pvVar2 + 0x5c);
  piVar3[5] = iVar5;
  *(int **)((int)pvVar2 + 0x5c) = piVar3;
  piVar3 = (int *)((int)this + 0x80);
  *piVar3 = *piVar3 + -1;
  if (*piVar3 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return CONCAT31((int3)((uint)iVar5 >> 8),1);
}


// 1000be00 FUN_1000be00

void __fastcall FUN_1000be00(int *param_1)

{
  int iVar1;
  
  FUN_1000b480(param_1);
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


// 1000be60 FUN_1000be60

void __thiscall FUN_1000be60(void *this,ushort param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  void *this_00;
  char cVar4;
  uint uVar5;
  void *local_8;
  undefined4 *puStack_4;
  
  iVar1 = *(int *)((uint)param_1 * 0x10 + 0xc + *(int *)((int)this + 0x2c));
  uVar5 = 0;
  if (*(int *)(iVar1 + 0x24) == 0) {
    *(undefined4 *)(iVar1 + 0x20) = 0;
    return;
  }
  local_8 = this;
  if (0 < *(int *)(iVar1 + 0x20)) {
    do {
      this_00 = local_8;
      if (*(int *)(DAT_1003758c + 0x30) != 0) {
        Msg("MDLCache: Free Anim block: %d\n",uVar5);
      }
      FUN_1000bc80(this_00,param_1,3,uVar5,'\x01');
      if ((*(int *)(*(int *)(iVar1 + 0x24) + uVar5 * 4) != 0) &&
         (iVar2 = *(int *)(*(int *)(iVar1 + 0x24) + uVar5 * 4), iVar2 != 0)) {
        piVar3 = *(int **)((int)this_00 + 0x18);
        cVar4 = (**(code **)(*piVar3 + 0x24))(iVar2);
        if (cVar4 != '\0') {
          (**(code **)(*piVar3 + 0x50))(iVar2);
          (**(code **)(*piVar3 + 0x20))(iVar2,&local_8,0,0);
          if ((puStack_4 < DAT_10037788) || (DAT_1003778c <= puStack_4)) {
            (**(code **)(*DAT_10038510 + 0x158))(puStack_4);
          }
          else {
            FUN_10001000(&DAT_10037780,puStack_4);
          }
        }
      }
      uVar5 = uVar5 + 1;
    } while ((int)uVar5 < *(int *)(iVar1 + 0x20));
  }
  FUN_1000e140(*(undefined4 *)(iVar1 + 0x24));
  *(undefined4 *)(iVar1 + 0x24) = 0;
  FUN_1000e140(*(undefined4 *)(iVar1 + 0x28));
  *(undefined4 *)(iVar1 + 0x28) = 0;
  *(undefined4 *)(iVar1 + 0x20) = 0;
  return;
}


// 1000bf80 FUN_1000bf80

void __fastcall FUN_1000bf80(undefined4 *param_1)

{
  *param_1 = 0;
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
  param_1[0x10] = 0;
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
  param_1[0x20] = 0;
  param_1[0x21] = 0;
  param_1[0x22] = 0;
  param_1[0x23] = 0;
  param_1[0x24] = 0;
  param_1[0x25] = 0;
  param_1[0x26] = 0;
  param_1[0x27] = 0;
  param_1[0x28] = 0;
  param_1[0x29] = 0;
  return;
}


// 1000c020 FUN_1000c020

void __thiscall FUN_1000c020(void *this,uint param_1)

{
  undefined4 *puVar1;
  char cVar2;
  ushort uVar3;
  int iVar4;
  int *piVar5;
  uint uVar6;
  
  *(byte *)((int)this + 0x84) = *(byte *)((int)this + 0x84) & 0xfe;
  cVar2 = (**(code **)(**(int **)((int)this + 0x14) + 0x3c))();
  if (cVar2 != '\0') {
    *(undefined4 *)((int)this + 0x20) = 0;
    do {
      *(int *)((int)this + 0x20) = *(int *)((int)this + 0x20) + 1;
      iVar4 = (**(code **)(**(int **)((int)this + 0x14) + 0x44))();
    } while (iVar4 != 0);
  }
  cVar2 = (**(code **)(**(int **)((int)this + 0x18) + 0x3c))();
  if (cVar2 != '\0') {
    *(undefined4 *)((int)this + 0x24) = 0;
    do {
      *(int *)((int)this + 0x24) = *(int *)((int)this + 0x24) + 1;
      iVar4 = (**(code **)(**(int **)((int)this + 0x18) + 0x44))();
    } while (iVar4 != 0);
  }
  uVar3 = FUN_100079f0((int)this + 0x28);
  if (uVar3 != 0xffff) {
    do {
      uVar6 = (uint)uVar3;
      puVar1 = *(undefined4 **)(uVar6 * 0x10 + 0xc + *(int *)((int)this + 0x2c));
      piVar5 = (int *)FUN_100053c0(this,0);
      cVar2 = (**(code **)(*piVar5 + 0x24))(*puVar1);
      if ((param_1 & 1) != 0) {
        if (*(int *)(DAT_1003758c + 0x30) != 0) {
          Msg("MDLCache: Free studiohdr\n");
        }
        if (*(int *)(DAT_1003758c + 0x30) != 0) {
          Msg("MDLCache: Free VVD\n");
        }
        if (*(int *)(DAT_1003758c + 0x30) != 0) {
          Msg("MDLCache: Free VTX\n");
        }
        FUN_1000bc80(this,uVar3,1,0,'\x01');
                    /* WARNING: Load size is inaccurate */
        (**(code **)(*this + 0x54))(uVar6,0x40);
      }
      if (cVar2 != '\0') {
                    /* WARNING: Load size is inaccurate */
        (**(code **)(*this + 0x2c))(uVar6);
      }
      uVar3 = FUN_10007aa0((void *)((int)this + 0x28),uVar3);
    } while (uVar3 != 0xffff);
  }
  FUN_100055d0((int)this);
  return;
}


// 1000c160 FUN_1000c160

undefined4 __thiscall FUN_1000c160(void *this,undefined4 *param_1)

{
  CThreadFastMutex *this_00;
  int *piVar1;
  uint uVar2;
  uint uVar3;
  uint uVar4;
  char cVar5;
  DWORD DVar6;
  int iVar7;
  undefined4 uVar8;
  int unaff_EBX;
  int *unaff_ESI;
  undefined4 uStack_18;
  undefined4 auStack_14 [3];
  int iStack_8;
  
  cVar5 = ThreadInMainThread();
  if (cVar5 == '\0') {
    return 0xffffffff;
  }
  uStack_18 = 0;
  auStack_14[0] = 0;
  this_00 = (CThreadFastMutex *)((int)this + 0x7c);
  DVar6 = GetCurrentThreadId();
  if (DVar6 != *(DWORD *)this_00) {
    LOCK();
    iVar7 = *(int *)this_00;
    if (iVar7 == 0) {
      *(DWORD *)this_00 = DVar6;
      iVar7 = 0;
    }
    UNLOCK();
    if (iVar7 != 0) {
      CThreadFastMutex::Lock(this_00,DVar6,0);
      goto LAB_1000c1b8;
    }
  }
  *(int *)((int)this + 0x80) = *(int *)((int)this + 0x80) + 1;
LAB_1000c1b8:
  piVar1 = (int *)((int)this + 0x80);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  iStack_8 = (**(code **)(*DAT_10038510 + 200))(*param_1,&uStack_18,auStack_14);
  if (iStack_8 == 1) {
    return 0;
  }
  uVar2 = param_1[1];
  uVar3 = param_1[2];
  uVar4 = param_1[3];
  FUN_1000bc80(this,(ushort)uVar2,uVar3,uVar4,'\0');
  switch(uVar3) {
  case 1:
  case 2:
  case 5:
    FUN_1000d4e0(this,uVar2,uVar3,0,unaff_ESI,unaff_EBX,iStack_8 == 0);
    (**(code **)(*DAT_10038510 + 0x158))(unaff_ESI);
    return 1;
  case 3:
    uVar8 = FUN_1000d4e0(this,uVar2,3,uVar4,unaff_ESI,unaff_EBX,iStack_8 == 0);
    if ((char)uVar8 == '\0') {
      FUN_10005ab0(unaff_ESI);
    }
  }
  return 1;
}


// 1000c17a FUN_1000c17a

undefined4 FUN_1000c17a(void)

{
  CThreadFastMutex *this;
  int *piVar1;
  uint uVar2;
  uint uVar3;
  uint uVar4;
  DWORD DVar5;
  int iVar6;
  undefined4 uVar7;
  int unaff_EBX;
  void *unaff_EBP;
  int *unaff_ESI;
  undefined4 uStack00000004;
  undefined4 uStack00000008;
  int iStack00000014;
  undefined4 *in_stack_00000020;
  
  uStack00000004 = 0;
  uStack00000008 = 0;
  this = (CThreadFastMutex *)((int)unaff_EBP + 0x7c);
  DVar5 = GetCurrentThreadId();
  if (DVar5 != *(DWORD *)this) {
    LOCK();
    iVar6 = *(int *)this;
    if (iVar6 == 0) {
      *(DWORD *)this = DVar5;
      iVar6 = 0;
    }
    UNLOCK();
    if (iVar6 != 0) {
      CThreadFastMutex::Lock(this,DVar5,0);
      goto LAB_1000c1b8;
    }
  }
  *(int *)((int)unaff_EBP + 0x80) = *(int *)((int)unaff_EBP + 0x80) + 1;
LAB_1000c1b8:
  piVar1 = (int *)((int)unaff_EBP + 0x80);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  iStack00000014 =
       (**(code **)(*DAT_10038510 + 200))(*in_stack_00000020,&stack0x00000004,&stack0x00000008);
  if (iStack00000014 == 1) {
    return 0;
  }
  uVar2 = in_stack_00000020[1];
  uVar3 = in_stack_00000020[2];
  uVar4 = in_stack_00000020[3];
  FUN_1000bc80(unaff_EBP,(ushort)uVar2,uVar3,uVar4,'\0');
  switch(uVar3) {
  case 1:
  case 2:
  case 5:
    FUN_1000d4e0(unaff_EBP,uVar2,uVar3,0,unaff_ESI,unaff_EBX,iStack00000014 == 0);
    (**(code **)(*DAT_10038510 + 0x158))(unaff_ESI);
    return 1;
  case 3:
    uVar7 = FUN_1000d4e0(unaff_EBP,uVar2,3,uVar4,unaff_ESI,unaff_EBX,iStack00000014 == 0);
    if ((char)uVar7 == '\0') {
      FUN_10005ab0(unaff_ESI);
    }
  }
  return 1;
}


// 1000c2c0 FUN_1000c2c0

undefined4 FUN_1000c2c0(int param_1)

{
  CThreadFastMutex *this;
  int *piVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  undefined4 uVar4;
  DWORD DVar5;
  int iVar6;
  void *in_ECX;
  
  uVar4 = ThreadInMainThread();
  if ((char)uVar4 == '\0') {
    return uVar4;
  }
  if (*(int *)((int)in_ECX + 0x60) == 0) {
    return uVar4;
  }
  if (DAT_100379ac != '\0') {
    return uVar4;
  }
  DAT_100379ac = '\x01';
  this = (CThreadFastMutex *)((int)in_ECX + 0x7c);
  DVar5 = GetCurrentThreadId();
  if (DVar5 != *(DWORD *)this) {
    LOCK();
    iVar6 = *(int *)this;
    if (iVar6 == 0) {
      *(DWORD *)this = DVar5;
      iVar6 = 0;
    }
    UNLOCK();
    if (iVar6 != 0) {
      CThreadFastMutex::Lock(this,DVar5,0);
      goto LAB_1000c327;
    }
  }
  *(int *)((int)in_ECX + 0x80) = *(int *)((int)in_ECX + 0x80) + 1;
LAB_1000c327:
  puVar2 = *(undefined4 **)((int)in_ECX + 0x54);
  do {
    puVar3 = puVar2;
    if (puVar3 == (undefined4 *)0x0) break;
    puVar2 = (undefined4 *)puVar3[5];
  } while (((param_1 != -1) && (puVar3[2] != param_1)) ||
          (iVar6 = FUN_1000c160(in_ECX,puVar3), 0 < iVar6));
  DAT_100379ac = '\0';
  piVar1 = (int *)((int)in_ECX + 0x80);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return *(undefined4 *)((int)in_ECX + 0x80);
}


// 1000c370 FUN_1000c370

undefined4 __thiscall FUN_1000c370(void *this,int param_1)

{
  CThreadFastMutex *this_00;
  int *piVar1;
  ushort uVar2;
  undefined4 *puVar3;
  DWORD DVar4;
  int iVar5;
  undefined4 uVar6;
  int iVar7;
  undefined4 local_144;
  ushort local_140;
  undefined4 local_13c;
  undefined4 local_138;
  undefined1 *local_134;
  undefined4 uStack_130;
  undefined4 uStack_12c;
  undefined4 uStack_128;
  undefined4 uStack_118;
  undefined *puStack_114;
  undefined4 uStack_10c;
  undefined1 local_104 [260];
  
  uVar2 = *(ushort *)(param_1 + 0x158);
  iVar7 = (uint)uVar2 * 0x10;
  if ((*(byte *)(*(int *)(*(int *)((int)this + 0x2c) + 0xc + iVar7) + 0x18) & 0x10) != 0) {
    return 0;
  }
  puVar3 = (undefined4 *)FUN_1000a890(uVar2,5,0);
  if (puVar3 != DAT_1002c2a8) goto LAB_1000c4de;
  FUN_100052b0(param_1);
  if (*(int *)(DAT_1003758c + 0x30) != 0) {
    Msg("MDLCache: Begin load VVD %s\n",local_104);
  }
  local_13c = 5;
  local_138 = 0;
  local_144 = 0;
  local_140 = uVar2;
  _memset(&local_134,0,0x30);
  local_134 = local_104;
  uStack_10c = 0xffff;
  puStack_114 = &DAT_10025a40;
  uStack_130 = 0;
  uStack_128 = 0;
  uStack_12c = 0;
  uStack_118 = 1;
  if (*(int *)(DAT_100377ac + 0x30) == 0) {
    uStack_118 = 5;
  }
  (**(code **)(*DAT_10038510 + 0x9c))(&local_134,1,&local_144);
  this_00 = (CThreadFastMutex *)((int)this + 0x7c);
  DVar4 = GetCurrentThreadId();
  if (DVar4 == *(DWORD *)this_00) {
LAB_1000c49f:
    *(int *)((int)this + 0x80) = *(int *)((int)this + 0x80) + 1;
  }
  else {
    LOCK();
    iVar5 = *(int *)this_00;
    if (iVar5 == 0) {
      *(DWORD *)this_00 = DVar4;
      iVar5 = 0;
    }
    UNLOCK();
    if (iVar5 == 0) goto LAB_1000c49f;
    CThreadFastMutex::Lock(this_00,DVar4,0);
  }
  puVar3 = FUN_1000b420((void *)((int)this + 0x48),&local_144);
  puVar3 = (undefined4 *)FUN_1000b940(uVar2,5,0,(int)puVar3);
  piVar1 = (int *)((int)this + 0x80);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
LAB_1000c4de:
  FUN_1000c160(this,puVar3);
  if ((*(int *)(DAT_100377ac + 0x30) == 0) && (*(int *)(DAT_10037954 + 0x30) != 0)) {
    piVar1 = (int *)(*(int *)((int)this + 0x2c) + 0xc + iVar7);
    uVar6 = (**(code **)(**(int **)((int)this + 0x14) + 0x28))(*(undefined4 *)(*piVar1 + 0x2c));
    *(undefined4 *)(*piVar1 + 0x44) = uVar6;
  }
  uVar6 = (**(code **)(**(int **)((int)this + 0x14) + 0x30))
                    (*(undefined4 *)(*(int *)(*(int *)((int)this + 0x2c) + 0xc + iVar7) + 0x2c),1);
  return uVar6;
}


// 1000c560 FUN_1000c560

undefined4 __thiscall FUN_1000c560(void *this,undefined *param_1)

{
  undefined4 in_EAX;
  uint uVar1;
  undefined4 uVar2;
  
  if ((*(byte *)((int)this + 0x84) & 2) != 0) {
    return CONCAT31((int3)((uint)in_EAX >> 8),1);
  }
  uVar1 = FUN_100076c0(this,param_1);
  if ((char)uVar1 != '\0') {
    uVar1 = (*(code *)param_1)("VPhysicsSurfaceProps001",0);
    DAT_10037480 = uVar1;
    if ((((uVar1 != 0) && (DAT_10038524 != 0)) && (DAT_100384f0 != 0)) &&
       ((DAT_100384ac != 0 && (DAT_10038518 != (int *)0x0)))) {
      *(byte *)((int)this + 0x84) = *(byte *)((int)this + 0x84) | 2;
      (**(code **)(*DAT_10038518 + 0xa8))(&LAB_1000b6f0);
      uVar2 = (**(code **)(*DAT_10038518 + 0xb0))(&LAB_1000c550);
      return CONCAT31((int3)((uint)uVar2 >> 8),1);
    }
  }
  return uVar1 & 0xffffff00;
}


// 1000c600 FUN_1000c600

void __fastcall FUN_1000c600(int param_1)

{
  bool bVar1;
  
  if ((DAT_10038518 != (int *)0x0) && ((*(byte *)(param_1 + 0x84) & 2) != 0)) {
    (**(code **)(*DAT_10038518 + 0xac))(&LAB_1000b6f0);
    (**(code **)(*DAT_10038518 + 0xb4))(&LAB_1000c550);
    *(byte *)(param_1 + 0x84) = *(byte *)(param_1 + 0x84) & 0xfd;
  }
  bVar1 = *(char *)(param_1 + 4) == '\0';
  if (!bVar1) {
    FUN_100144c0();
    bVar1 = *(char *)(param_1 + 4) == '\0';
    if (!bVar1) {
      FUN_100146e0();
      bVar1 = *(char *)(param_1 + 4) == '\0';
    }
  }
  if (!bVar1) {
    FUN_10017560();
    return;
  }
  return;
}


// 1000c670 FUN_1000c670

void __thiscall FUN_1000c670(void *this,uint param_1,char param_2)

{
  CThreadFastMutex *pCVar1;
  int iVar2;
  bool bVar3;
  ushort uVar4;
  int iVar5;
  uint uVar6;
  undefined4 uVar7;
  DWORD DVar8;
  int iVar9;
  int *piVar10;
  char *pcVar11;
  undefined4 unaff_retaddr;
  int iVar12;
  int *local_160;
  int *local_15c;
  int *piStack_158;
  int *piStack_154;
  int *local_150;
  short asStack_14c [2];
  undefined4 uStack_148;
  undefined4 uStack_144;
  int *local_13c;
  char *pcStack_138;
  undefined4 uStack_134;
  undefined4 uStack_130;
  undefined4 uStack_12c;
  undefined4 uStack_11c;
  undefined *puStack_118;
  undefined4 uStack_110;
  char acStack_108 [264];
  
  piVar10 = (int *)((param_1 & 0xffff) * 0x10);
  iVar9 = *(int *)(*(int *)((int)this + 0x2c) + 0xc + (int)piVar10);
  local_150 = (int *)((param_1 & 0xffff) << 0x10 | 0x4000);
  local_15c = piVar10;
  local_13c = local_150;
  uVar4 = FUN_10007720(&PTR_LAB_1002c2ac,&local_13c);
  if ((uVar4 != 0xffff) &&
     (local_160 = *(int **)((uint)uVar4 * 0x10 + 0xc + DAT_1002c2b0), local_160 != DAT_1002c2a8)) {
    if (param_2 == '\0') goto LAB_1000c9c2;
    pCVar1 = (CThreadFastMutex *)((int)this + 0x7c);
    DVar8 = GetCurrentThreadId();
    if (DVar8 == *(DWORD *)pCVar1) {
LAB_1000c985:
      *(int *)((int)this + 0x80) = *(int *)((int)this + 0x80) + 1;
    }
    else {
      LOCK();
      iVar9 = *(int *)pCVar1;
      if (iVar9 == 0) {
        *(DWORD *)pCVar1 = DVar8;
        iVar9 = 0;
      }
      UNLOCK();
      if (iVar9 == 0) goto LAB_1000c985;
      CThreadFastMutex::Lock(pCVar1,DVar8,0);
    }
    piVar10 = (int *)((int)this + 0x80);
    *piVar10 = *piVar10 + -1;
    if (*piVar10 == 0) {
      LOCK();
      *(undefined4 *)pCVar1 = 0;
      UNLOCK();
    }
    if (*local_160 != 0) {
      (**(code **)(*DAT_10038510 + 0xc4))(*local_160,1);
    }
    goto LAB_1000c9c2;
  }
  *(ushort *)(iVar9 + 0x18) = *(ushort *)(iVar9 + 0x18) & 0xfffd | 0x80;
  *(undefined4 *)(iVar9 + 4) = 0;
                    /* WARNING: Load size is inaccurate */
  iVar5 = (**(code **)(*this + 0x38))(param_1);
  if ((iVar5 != 0) && (local_15c = (int *)0x1, 1 < *(int *)(iVar5 + 100))) {
    iVar12 = 0x90;
    do {
      uVar6 = (uint)*(ushort *)(iVar12 + *(int *)(iVar5 + 0x58));
      iVar2 = *(int *)(uVar6 * 0x10 + 0xc + *(int *)((int)this + 0x2c));
      if ((*(byte *)(iVar2 + 0x18) & 2) == 0) {
        FUN_1000c670(this,uVar6,(char)param_1);
      }
      if (*(int *)(iVar2 + 4) != 0) {
        piVar10 = (int *)(*(int *)(iVar2 + 4) + 4);
        LOCK();
        *piVar10 = *piVar10 + 1;
        UNLOCK();
        uVar7 = *(undefined4 *)(iVar2 + 4);
        *(ushort *)(iVar9 + 0x18) = *(ushort *)(iVar9 + 0x18) | 2;
        *(undefined4 *)(iVar9 + 4) = uVar7;
        return;
      }
      iVar12 = iVar12 + 0x90;
      local_15c = (int *)((int)local_15c + 1);
      piVar10 = local_160;
    } while ((int)local_15c < *(int *)(iVar5 + 100));
  }
  if ((short)unaff_retaddr == -1) {
    pcVar11 = "models/error.mdl";
  }
  else if ((*(byte *)(*(int *)((int)piVar10 + *(int *)((int)this + 0x2c) + 0xc) + 0x18) & 4) == 0) {
    pcVar11 = *(char **)((int)piVar10 + *(int *)((int)this + 0x2c) + 8);
  }
  else {
    pcVar11 = "models/error.mdl";
  }
  FUN_10016660(acStack_108,pcVar11,0x104);
  FUN_100169a0(acStack_108,".phy",0x104);
  FUN_100163d0(acStack_108,'\\');
  if ((*(int *)(DAT_10037754 + 0x30) == 0) || ((char)param_1 != '\0')) {
    bVar3 = false;
  }
  else {
    bVar3 = true;
  }
  if (*(int *)(DAT_1003758c + 0x30) != 0) {
    pcVar11 = "Async";
    if (!bVar3) {
      pcVar11 = "Sync";
    }
                    /* WARNING: Load size is inaccurate */
    uVar7 = (**(code **)(*this + 0x5c))(unaff_retaddr);
    Msg("MDLCache: %s load vcollide %s\n",pcVar11,uVar7);
  }
  uStack_148 = 2;
  uStack_144 = 0;
  local_150 = (int *)0x0;
  asStack_14c[0] = (short)unaff_retaddr;
  _memset(&pcStack_138,0,0x30);
  pcStack_138 = acStack_108;
  uStack_110 = 0xffff;
  puStack_118 = &DAT_10025a40;
  uStack_134 = 0;
  uStack_12c = 0;
  uStack_130 = 0;
  uStack_11c = 1;
  if (!bVar3) {
    uStack_11c = 5;
  }
  (**(code **)(*DAT_10038510 + 0x9c))(&pcStack_138,1,&local_150);
  pCVar1 = (CThreadFastMutex *)((int)this + 0x7c);
  DVar8 = GetCurrentThreadId();
  if (DVar8 == *(DWORD *)pCVar1) {
LAB_1000c8f0:
    *(int *)((int)this + 0x80) = *(int *)((int)this + 0x80) + 1;
  }
  else {
    LOCK();
    iVar9 = *(int *)pCVar1;
    if (iVar9 == 0) {
      *(DWORD *)pCVar1 = DVar8;
      iVar9 = 0;
    }
    UNLOCK();
    if (iVar9 == 0) goto LAB_1000c8f0;
    CThreadFastMutex::Lock(pCVar1,DVar8,0);
  }
  local_160 = FUN_1000b420((void *)((int)this + 0x48),(undefined4 *)asStack_14c);
  local_15c = local_150;
  if (local_160 == DAT_1002c2a8) {
    FUN_1000b870(&PTR_LAB_1002c2ac,&local_15c);
  }
  else {
    piStack_158 = local_150;
    piStack_154 = local_160;
    FUN_1000b3b0(&PTR_LAB_1002c2ac,&piStack_158);
  }
  piVar10 = (int *)((int)this + 0x80);
  *piVar10 = *piVar10 + -1;
  if (*piVar10 == 0) {
    LOCK();
    *(undefined4 *)pCVar1 = 0;
    UNLOCK();
  }
LAB_1000c9c2:
  FUN_1000c160(this,local_160);
  return;
}


// 1000cab0 FUN_1000cab0

undefined4 __thiscall FUN_1000cab0(void *this,uint param_1,uint param_2)

{
  int iVar1;
  undefined4 *puVar2;
  int iVar3;
  undefined4 uVar4;
  int iVar5;
  int *piVar6;
  int iVar7;
  ushort uVar8;
  int iStack_114;
  ushort uStack_110;
  undefined4 uStack_10c;
  uint uStack_108;
  char acStack_104 [260];
  
  iVar1 = *(int *)((param_1 & 0xffff) * 0x10 + 0xc + *(int *)((int)this + 0x2c));
  uVar8 = (ushort)param_1;
  puVar2 = (undefined4 *)FUN_1000a890(uVar8,3,param_2);
  if (puVar2 == DAT_1002c2a8) {
                    /* WARNING: Load size is inaccurate */
    iVar3 = (**(code **)(*this + 0x28))(param_1);
    iVar5 = *(int *)(iVar3 + 0x15c);
    iVar7 = *(int *)(iVar3 + 0x164) + param_2 * 8;
    piVar6 = (int *)(iVar7 + iVar3);
    iVar7 = *(int *)(iVar7 + 4 + iVar3) - *piVar6;
    if (iVar7 == 0) {
      return 0;
    }
    *(undefined4 *)(*(int *)(iVar1 + 0x24) + param_2 * 4) = 0;
    FUN_10016660(acStack_104,(char *)(iVar5 + iVar3),0x104);
    FUN_100163d0(acStack_104,'\\');
    if (*(int *)(DAT_1003758c + 0x30) != 0) {
                    /* WARNING: Load size is inaccurate */
      uVar4 = (**(code **)(*this + 0x5c))(param_1,param_2,iVar7);
      Msg("MDLCache: Begin load Anim Block %s (block %i, bytes %d)\n",uVar4);
    }
    iVar5 = 0;
    uStack_10c = 3;
    uStack_108 = param_2;
    iStack_114 = 0;
    uStack_110 = uVar8;
    if (iVar7 < 0x8001) {
      iVar5 = FUN_10005770((undefined4 *)&DAT_10037780);
    }
    FUN_10005690(acStack_104,&DAT_10025a40,iVar5,iVar7,*piVar6,*(int *)(DAT_100377f4 + 0x30) != 0,
                 &iStack_114);
    puVar2 = FUN_1000b420((void *)((int)this + 0x48),&iStack_114);
    puVar2 = (undefined4 *)FUN_1000b940(uVar8,3,param_2,(int)puVar2);
  }
  FUN_1000c160(this,puVar2);
  uVar4 = (**(code **)(**(int **)((int)this + 0x18) + 0x30))
                    (*(undefined4 *)(*(int *)(iVar1 + 0x24) + param_2 * 4),1);
  return uVar4;
}


// 1000cc30 FUN_1000cc30

int __thiscall FUN_1000cc30(void *this,uint param_1,uint param_2)

{
  CThreadFastMutex *pCVar1;
  int *piVar2;
  uint *puVar3;
  int iVar4;
  uint uVar5;
  DWORD DVar6;
  int iVar7;
  uint uVar8;
  
  if ((*(int *)(DAT_100374fc + 0x30) != 0) || ((short)param_1 == -1)) {
    return 0;
  }
  iVar4 = *(int *)((param_1 & 0xffff) * 0x10 + 0xc + *(int *)((int)this + 0x2c));
  if (*(int *)(iVar4 + 0x24) == 0) {
    pCVar1 = (CThreadFastMutex *)((int)this + 0x7c);
    DVar6 = GetCurrentThreadId();
    if (DVar6 == *(DWORD *)pCVar1) {
LAB_1000cc86:
      *(int *)((int)this + 0x80) = *(int *)((int)this + 0x80) + 1;
    }
    else {
      LOCK();
      iVar7 = *(int *)pCVar1;
      if (iVar7 == 0) {
        *(DWORD *)pCVar1 = DVar6;
        iVar7 = 0;
      }
      UNLOCK();
      if (iVar7 == 0) goto LAB_1000cc86;
      CThreadFastMutex::Lock(pCVar1,DVar6,0);
    }
    if (*(int *)(iVar4 + 0x24) == 0) {
                    /* WARNING: Load size is inaccurate */
      iVar7 = (**(code **)(*this + 0x28))(param_1);
      FUN_10005490(iVar4,*(uint *)(iVar7 + 0x160));
    }
    piVar2 = (int *)((int)this + 0x80);
    *piVar2 = *piVar2 + -1;
    if (*piVar2 == 0) {
      LOCK();
      *(undefined4 *)pCVar1 = 0;
      UNLOCK();
    }
  }
  if ((int)param_2 < 0) {
    return 0;
  }
  if (*(int *)(iVar4 + 0x20) <= (int)param_2) {
    return 0;
  }
  iVar7 = (**(code **)(**(int **)((int)this + 0x18) + 0x30))
                    (*(undefined4 *)(*(int *)(iVar4 + 0x24) + param_2 * 4),1);
  if (iVar7 != 0) goto LAB_1000cd71;
  pCVar1 = (CThreadFastMutex *)((int)this + 0x7c);
  DVar6 = GetCurrentThreadId();
  if (DVar6 == *(DWORD *)pCVar1) {
LAB_1000cd1d:
    *(int *)((int)this + 0x80) = *(int *)((int)this + 0x80) + 1;
  }
  else {
    LOCK();
    iVar7 = *(int *)pCVar1;
    if (iVar7 == 0) {
      *(DWORD *)pCVar1 = DVar6;
      iVar7 = 0;
    }
    UNLOCK();
    if (iVar7 == 0) goto LAB_1000cd1d;
    CThreadFastMutex::Lock(pCVar1,DVar6,0);
  }
  iVar7 = (**(code **)(**(int **)((int)this + 0x18) + 0x30))
                    (*(undefined4 *)(*(int *)(iVar4 + 0x24) + param_2 * 4),1);
  if (iVar7 == 0) {
    *(undefined4 *)(*(int *)(iVar4 + 0x24) + param_2 * 4) = 0;
    iVar7 = FUN_1000cab0(this,(uint)this,param_2);
  }
  piVar2 = (int *)((int)this + 0x80);
  *piVar2 = *piVar2 + -1;
  if (*piVar2 == 0) {
    LOCK();
    *(undefined4 *)pCVar1 = 0;
    UNLOCK();
  }
LAB_1000cd71:
  if (*(int *)(DAT_10037634 + 0x30) != 0) {
    uVar8 = Plat_MSTime();
    puVar3 = (uint *)(*(int *)(iVar4 + 0x28) + param_2 * 4);
    uVar5 = *puVar3;
    if ((uVar5 == 0) || (uVar8 < uVar5)) {
      *puVar3 = uVar8;
    }
    if ((int)(uVar8 - *(int *)(*(int *)(iVar4 + 0x28) + param_2 * 4)) <
        *(int *)(DAT_10037634 + 0x30)) {
      return 0;
    }
  }
  return iVar7;
}


// 1000cdc0 FUN_1000cdc0

void __thiscall FUN_1000cdc0(void *this,ushort param_1)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 uVar3;
  
  iVar1 = *(int *)((uint)param_1 * 0x10 + 0xc + *(int *)((int)this + 0x2c));
  puVar2 = (undefined4 *)FUN_1000dfa0(0xa8);
  if (puVar2 != (undefined4 *)0x0) {
    uVar3 = FUN_1000bf80(puVar2);
    *(undefined4 *)(iVar1 + 0x1c) = uVar3;
    return;
  }
  *(undefined4 *)(iVar1 + 0x1c) = 0;
  return;
}


// 1000ce00 FUN_1000ce00

undefined4 __thiscall FUN_1000ce00(void *this,void *param_1,ushort param_2)

{
  int iVar1;
  int iVar2;
  
  if (*(int *)((int)param_1 + 0x150) == 0) {
    return 0;
  }
  iVar1 = *(int *)((uint)param_2 * 0x10 + 0xc + *(int *)((int)this + 0x2c));
  if (iVar1 == 0) {
    return 0;
  }
  if (*(int *)(iVar1 + 0x1c) == 0) {
    DevMsg(2,"Loading virtual model for %s\n",(int)param_1 + 0xc);
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 100))();
    FUN_1000cdc0(this,param_2);
    iVar2 = FUN_1000a950((void *)(*(int *)(iVar1 + 0x1c) + 0x58),
                         *(int *)(*(int *)(iVar1 + 0x1c) + 100));
    *(uint *)(iVar2 * 0x90 + *(int *)(*(int *)(iVar1 + 0x1c) + 0x58)) = (uint)param_2;
    FUN_10011c50(*(void **)(iVar1 + 0x1c),0,param_1);
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x68))();
  }
  return *(undefined4 *)(iVar1 + 0x1c);
}


// 1000ceb0 FUN_1000ceb0

bool __thiscall FUN_1000ceb0(void *this,uint param_1)

{
  CThreadFastMutex *this_00;
  int *piVar1;
  int iVar2;
  undefined4 *puVar3;
  undefined4 uVar4;
  int iVar5;
  DWORD DVar6;
  code *pcVar7;
  int unaff_EBX;
  int iVar8;
  ushort uVar9;
  undefined1 auStack_10c [268];
  
  if ((*(byte *)((int)this + 0x84) & 1) != 0) {
    return false;
  }
                    /* WARNING: Load size is inaccurate */
  iVar8 = *(int *)((param_1 & 0xffff) * 0x10 + 0xc + *(int *)((int)this + 0x2c));
  (**(code **)(*this + 100))();
                    /* WARNING: Load size is inaccurate */
  iVar2 = (**(code **)(*this + 0x28))(param_1);
  if ((iVar2 == 0) || (*(int *)(iVar2 + 0xe8) == 0)) {
    *(ushort *)(iVar8 + 0x18) = *(ushort *)(iVar8 + 0x18) | 8;
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x68))();
    return true;
  }
  if ((*(byte *)(iVar8 + 0x18) & 8) != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x68))();
    return false;
  }
  uVar9 = (ushort)param_1;
  if ((((*(int *)(DAT_1003758c + 0x30) != 0) &&
       (puVar3 = (undefined4 *)FUN_1000a890(uVar9,1,0), puVar3 == DAT_1002c2a8)) &&
      (puVar3 = (undefined4 *)FUN_1000a890(uVar9,5,0), puVar3 == DAT_1002c2a8)) &&
     (*(int *)(DAT_1003758c + 0x30) != 0)) {
                    /* WARNING: Load size is inaccurate */
    uVar4 = (**(code **)(*this + 0x5c))(param_1);
    Msg("MDLCache: Begin load studiomdl %s\n",uVar4);
  }
                    /* WARNING: Load size is inaccurate */
  iVar5 = (**(code **)(*this + 0x40))(param_1);
  if (iVar5 == 0) {
    if ((*(ushort *)(iVar8 + 0x18) & 0x10) != 0) {
      *(ushort *)(iVar8 + 0x18) = *(ushort *)(iVar8 + 0x18) | 8;
    }
                    /* WARNING: Load size is inaccurate */
    pcVar7 = *(code **)(*this + 0x68);
    goto LAB_1000cfb0;
  }
  puVar3 = (undefined4 *)FUN_1000a890(uVar9,1,0);
  if (puVar3 == DAT_1002c2a8) {
    (**(code **)(**(int **)((int)this + 0x14) + 0x28))(*(undefined4 *)(iVar8 + 0x2c));
    FUN_100052b0(iVar2);
    if (*(int *)(DAT_1003758c + 0x30) != 0) {
                    /* WARNING: Load size is inaccurate */
      uVar4 = (**(code **)(*this + 0x5c))(param_1);
      Msg("MDLCache: Begin load VTX %s\n",uVar4);
    }
    FUN_10005af0(auStack_10c,&DAT_10025a40,*(int *)(DAT_100377ac + 0x30) != 0,
                 (int *)&stack0xfffffee4);
    this_00 = (CThreadFastMutex *)((int)this + 0x7c);
    DVar6 = GetCurrentThreadId();
    if (DVar6 == *(DWORD *)this_00) {
LAB_1000d07c:
      unaff_EBX = iVar8;
      *(int *)((int)this + 0x80) = *(int *)((int)this + 0x80) + 1;
    }
    else {
      LOCK();
      iVar2 = *(int *)this_00;
      if (iVar2 == 0) {
        *(DWORD *)this_00 = DVar6;
        iVar2 = 0;
      }
      UNLOCK();
      iVar8 = unaff_EBX;
      if (iVar2 == 0) goto LAB_1000d07c;
      CThreadFastMutex::Lock(this_00,DVar6,0);
    }
    puVar3 = FUN_1000b420((void *)((int)this + 0x48),(undefined4 *)&stack0xfffffee4);
    puVar3 = (undefined4 *)FUN_1000b9a0(uVar9,1,(int)puVar3);
    piVar1 = (int *)((int)this + 0x80);
    *piVar1 = *piVar1 + -1;
    iVar8 = unaff_EBX;
    if (*piVar1 == 0) {
      LOCK();
      *(undefined4 *)this_00 = 0;
      UNLOCK();
    }
  }
  iVar2 = FUN_1000c160(this,puVar3);
                    /* WARNING: Load size is inaccurate */
  pcVar7 = *(code **)(*this + 0x68);
  if ((0 < iVar2) && ((*(byte *)(iVar8 + 0x18) & 8) == 0)) {
    iVar8 = *(int *)(iVar8 + 0x14);
    (*pcVar7)();
    return iVar8 != 0;
  }
LAB_1000cfb0:
  (*pcVar7)();
  return false;
}


// 1000d190 FUN_1000d190

void __fastcall FUN_1000d190(int param_1)

{
  CThreadFastMutex *this;
  int *piVar1;
  char cVar2;
  DWORD DVar3;
  int iVar4;
  
  cVar2 = ThreadInMainThread();
  if (cVar2 == '\0') {
    return;
  }
  this = (CThreadFastMutex *)(param_1 + 0x7c);
  DVar3 = GetCurrentThreadId();
  if (DVar3 != *(DWORD *)this) {
    LOCK();
    iVar4 = *(int *)this;
    if (iVar4 == 0) {
      *(DWORD *)this = DVar3;
      iVar4 = 0;
    }
    UNLOCK();
    if (iVar4 != 0) {
      CThreadFastMutex::Lock(this,DVar3,0);
      goto LAB_1000d1d1;
    }
  }
  *(int *)(param_1 + 0x80) = *(int *)(param_1 + 0x80) + 1;
LAB_1000d1d1:
  for (piVar1 = *(int **)(param_1 + 0x54); piVar1 != (int *)0x0; piVar1 = (int *)piVar1[5]) {
    if (*piVar1 != 0) {
      (**(code **)(*DAT_10038510 + 0xc4))(*piVar1,1);
    }
  }
  FUN_1000c2c0(0xffffffff);
  piVar1 = (int *)(param_1 + 0x80);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return;
}


// 1000d220 FUN_1000d220

void __thiscall FUN_1000d220(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  
  iVar2 = (uint)*(ushort *)(param_1 + 0x158) * 0x10;
  iVar1 = *(int *)(*(int *)((int)this + 0x24) + 0xc + iVar2);
  if (*(int *)(iVar1 + 0x44) == 0) {
    iVar1 = (**(code **)(**(int **)((int)this + 0xc) + 0x30))(*(undefined4 *)(iVar1 + 0x2c),1);
    if (iVar1 == 0) {
      *(undefined4 *)(*(int *)(*(int *)((int)this + 0x24) + 0xc + iVar2) + 0x2c) = 0;
      FUN_1000c370((void *)((int)this + -8),param_1);
    }
  }
  return;
}


// 1000d280 FUN_1000d280

undefined4 __fastcall FUN_1000d280(void *param_1)

{
  undefined4 uVar1;
  
  if (*(int *)((int)param_1 + 0x150) == 0) {
    return 0;
  }
  uVar1 = FUN_1000ce00(&DAT_10037868,param_1,*(ushort *)((int)param_1 + 0x158));
  return uVar1;
}


// 1000d2c0 FUN_1000d2c0

void __thiscall FUN_1000d2c0(void *this,uint param_1)

{
  int iVar1;
  int iVar2;
  uint uVar3;
  
  iVar1 = *(int *)((param_1 & 0xffff) * 0x10 + 0xc + *(int *)((int)this + 0x2c));
  if ((iVar1 != 0) && (*(int *)(iVar1 + 0x1c) != 0)) {
    iVar2 = *(int *)(*(int *)(iVar1 + 0x1c) + 100);
    if (1 < iVar2) {
      param_1 = iVar2 - 1;
      iVar2 = 0x90;
      do {
        uVar3 = (uint)*(ushort *)(iVar2 + *(int *)(*(int *)(iVar1 + 0x1c) + 0x58));
        FUN_1000d2c0(this,uVar3);
                    /* WARNING: Load size is inaccurate */
        (**(code **)(*this + 0x20))(uVar3);
        iVar2 = iVar2 + 0x90;
        param_1 = param_1 - 1;
      } while (param_1 != 0);
    }
    iVar2 = *(int *)(iVar1 + 0x1c);
    if (iVar2 != 0) {
      FUN_10002f70((int *)(iVar2 + 0x94));
      FUN_10002f70((int *)(iVar2 + 0x80));
      FUN_10002f70((int *)(iVar2 + 0x6c));
      FUN_1000be00((int *)(iVar2 + 0x58));
      FUN_10002f70((int *)(iVar2 + 0x44));
      FUN_10002f70((int *)(iVar2 + 0x30));
      FUN_10002f70((int *)(iVar2 + 0x1c));
      FUN_10002f70((int *)(iVar2 + 8));
      FUN_1000e140(iVar2);
    }
    *(undefined4 *)(iVar1 + 0x1c) = 0;
  }
  return;
}


// 1000d390 FUN_1000d390

void __thiscall FUN_1000d390(void *this,uint param_1)

{
  int iVar1;
  int iVar2;
  
  if ((short)param_1 != -1) {
    FUN_1000d2c0(this,param_1);
    if (*(int *)(DAT_1003767c + 0x30) != 0) {
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x38))(param_1);
                    /* WARNING: Load size is inaccurate */
      iVar1 = (**(code **)(*this + 0x28))(param_1);
      iVar2 = 1;
      if (1 < *(int *)(iVar1 + 0x160)) {
        do {
                    /* WARNING: Load size is inaccurate */
          (**(code **)(*this + 0x34))(param_1,iVar2);
          iVar2 = iVar2 + 1;
        } while (iVar2 < *(int *)(iVar1 + 0x160));
      }
      FUN_1000c2c0(3);
    }
  }
  return;
}


// 1000d4e0 FUN_1000d4e0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __thiscall
FUN_1000d4e0(void *this,uint param_1,int param_2,int param_3,int *param_4,int param_5,char param_6)

{
  char cVar1;
  void *pvVar2;
  uint uVar3;
  int iVar4;
  undefined4 uVar5;
  undefined4 *puVar6;
  int iVar7;
  int *piVar8;
  int *local_14c;
  int iStack_148;
  int aiStack_144 [2];
  int iStack_13c;
  int aiStack_134 [3];
  int iStack_128;
  int iStack_118;
  int iStack_114;
  char acStack_104 [260];
  
  local_14c = (int *)0x0;
                    /* WARNING: Load size is inaccurate */
  if ((param_2 != 0) &&
     (local_14c = (int *)(**(code **)(*this + 0x28))(param_1), local_14c == (int *)0x0)) {
    return 0;
  }
  iVar7 = (param_1 & 0xffff) * 0x10;
  iVar4 = *(int *)(*(int *)((int)this + 0x2c) + 0xc + iVar7);
  if (iVar4 != 0) {
    switch(param_2) {
    case 0:
      local_14c = (int *)FUN_1000aea0(this,param_1,(int)param_4);
      if (local_14c == (int *)0x0) goto LAB_1000d938;
      cVar1 = FUN_10004e50((int)local_14c);
      if (cVar1 == '\0') {
        Warning("MDLCache: %s needs to be recompiled\n",local_14c + 3);
      }
      if ((local_14c[0x54] == 0) && (uVar3 = FUN_1000e990(local_14c), uVar3 != 0)) {
        FUN_10005b30(*(int *)(*(int *)((int)this + 0x2c) + 0xc + iVar7),uVar3);
        FUN_1000e9e0(local_14c,*(int *)(*(int *)(*(int *)((int)this + 0x2c) + 0xc + iVar7) + 0x38),
                     uVar3);
      }
      local_14c = (int *)FUN_1000d390(this,param_1);
      break;
    case 1:
      if (param_6 == '\0') {
        *(ushort *)(iVar4 + 0x18) = *(ushort *)(iVar4 + 0x18) | 8;
        if (local_14c[0x3a] != 0) {
          uVar3 = Warning("MDLCache: Failed load of .VTX data for %s\n",local_14c + 3);
          return uVar3 & 0xffffff00;
        }
      }
      else {
        FUN_10005bd0(this,param_1,iVar4,(int)local_14c,param_4);
      }
      (**(code **)(**(int **)((int)this + 0x14) + 0x2c))(*(undefined4 *)(iVar4 + 0x2c));
      local_14c = (int *)(**(code **)(**(int **)((int)this + 0x14) + 0x58))
                                   (*(undefined4 *)(iVar4 + 0x2c));
      if ((_DAT_100379b0 & 1) == 0) {
        _DAT_100379b0 = _DAT_100379b0 | 1;
        piVar8 = (int *)CommandLine_Tier0();
        iVar4 = (**(code **)(*piVar8 + 0xc))("-no_compressed_vvds",0);
        DAT_100379ad = iVar4 == 0;
        return CONCAT31((int3)((uint)iVar4 >> 8),1);
      }
      break;
    case 2:
      *(ushort *)(iVar4 + 0x18) = *(ushort *)(iVar4 + 0x18) | 2;
      if (param_6 != '\0') {
        if (*(int *)(DAT_1003758c + 0x30) != 0) {
          Msg("MDLCache: Finish load vcollide for %s\n",local_14c + 3);
        }
        iStack_148 = 0;
        FUN_10017a60(aiStack_134,(int)param_4,param_5,8);
        FUN_10017960(aiStack_134,0,param_5);
        FUN_100178d0(aiStack_134,aiStack_144,0x10);
        if ((aiStack_144[0] == 0x10) && (0 < iStack_13c)) {
          iVar7 = iStack_118 - iStack_128;
          puVar6 = (undefined4 *)FUN_1000dfa0(0x18);
          if (puVar6 == (undefined4 *)0x0) {
            puVar6 = (undefined4 *)0x0;
          }
          else {
            puVar6[1] = 1;
            *puVar6 = &PTR_FUN_100259b0;
          }
          *(undefined4 **)(iVar4 + 4) = puVar6;
          (**(code **)(*DAT_100384f0 + 0x90))
                    (puVar6 + 2,iStack_13c,(aiStack_134[0] - iStack_114) + iStack_128,iVar7,0);
          if (*(int *)(DAT_100375ec + 0x30) != 0) {
            (**(code **)(*DAT_100384f0 + 0xe0))(puVar6 + 2,local_14c + 3);
          }
          if (*(int *)((int)this + 0x44) != 0) {
            (**(code **)**(undefined4 **)((int)this + 0x44))(2,param_1);
          }
        }
        FUN_100072a0(aiStack_134);
        uVar5 = FUN_10005a80(&iStack_148);
        return CONCAT31((int3)((uint)uVar5 >> 8),1);
      }
      if (*(int *)(DAT_1003758c + 0x30) != 0) {
        local_14c = (int *)Warning("MDLCache: Failed load of .PHY data for %s\n",local_14c + 3);
      }
      goto LAB_1000d938;
    case 3:
      if (param_6 == '\0') {
        if (*(int *)(DAT_1003758c + 0x30) != 0) {
          local_14c = (int *)Msg("MDLCache: Failed load anim block %s (block %i)\n",local_14c + 3,
                                 param_3);
        }
        if (*(int *)(iVar4 + 0x24) != 0) {
          *(undefined4 *)(*(int *)(iVar4 + 0x24) + param_3 * 4) = 0;
        }
        return (uint)local_14c & 0xffffff00;
      }
      if (*(int *)(DAT_1003758c + 0x30) != 0) {
        Msg("MDLCache: Finish load anim block %s (block %i)\n",local_14c + 3,param_3);
      }
      FUN_100166a0(acStack_104,0x104,"%s (block %i)");
      piVar8 = (int *)((param_1 & 0xffff) << 0x10 | 3);
      local_14c = param_4;
      if (param_4 != (int *)0x0) {
        if (piVar8 == (int *)0xffffffff) {
          piVar8 = param_4;
        }
        uVar5 = (**(code **)(**(int **)((int)this + 0x18) + 0x18))
                          (piVar8,param_4,param_5,*(int *)(iVar4 + 0x24) + param_3 * 4);
        return CONCAT31((int3)((uint)uVar5 >> 8),1);
      }
      break;
    case 5:
      if (param_6 != '\0') {
        pvVar2 = FUN_10009ee0(this,(uint)local_14c,param_4);
        return CONCAT31((int3)((uint)pvVar2 >> 8),1);
      }
      *(ushort *)(iVar4 + 0x18) = *(ushort *)(iVar4 + 0x18) | 0x10;
      if (local_14c[0x3a] != 0) {
        uVar3 = Warning("MDLCache: Failed load of .VVD data for %s\n",local_14c + 3);
        return uVar3 & 0xffffff00;
      }
    }
    return CONCAT31((int3)((uint)local_14c >> 8),1);
  }
LAB_1000d938:
  return (uint)local_14c & 0xffffff00;
}


// 1000dbc0 FUN_1000dbc0

longlong __fastcall FUN_1000dbc0(int *param_1,uint param_2,uint param_3)

{
  int *piVar1;
  ushort *puVar2;
  int iVar3;
  DWORD DVar4;
  int iVar5;
  char *pcVar6;
  undefined4 uVar7;
  uint extraout_EDX;
  CThreadFastMutex *this;
  int *unaff_EBP;
  int iVar8;
  int iStack_1c;
  int iStack_4;
  
  if ((ushort)param_3 == 0xffff) {
    return (ulonglong)param_2 << 0x20;
  }
  iVar8 = (param_3 & 0xffff) * 0x10;
  iVar3 = *(int *)(param_1[0xb] + 0xc + iVar8);
  iVar5 = *(int *)(iVar3 + 0x40);
  if (iVar5 != 0) goto LAB_1000de04;
  if (*(int *)(DAT_1003783c + 0x30) != 0) {
    this = (CThreadFastMutex *)(iVar3 + 0x4c);
    DVar4 = GetCurrentThreadId();
    if (DVar4 == *(DWORD *)this) {
LAB_1000dc26:
      *(int *)(iVar3 + 0x50) = *(int *)(iVar3 + 0x50) + 1;
    }
    else {
      LOCK();
      iVar5 = *(int *)this;
      if (iVar5 == 0) {
        *(DWORD *)this = DVar4;
        iVar5 = 0;
      }
      UNLOCK();
      if (iVar5 == 0) goto LAB_1000dc26;
      CThreadFastMutex::Lock(this,DVar4,0);
    }
    if (*(int *)(iVar3 + 0x40) != 0) {
      piVar1 = (int *)(iVar3 + 0x50);
      *piVar1 = *piVar1 + -1;
      if (*piVar1 == 0) {
        LOCK();
        *(undefined4 *)this = 0;
        UNLOCK();
      }
      return CONCAT44(*(undefined4 *)(iVar3 + 0x50),*(undefined4 *)(iVar3 + 0x40));
    }
  }
  iVar5 = (**(code **)(*(int *)param_1[4] + 0x30))(**(undefined4 **)(param_1[0xb] + 0xc + iVar8),1);
  if (iVar5 == 0) {
    **(undefined4 **)(param_1[0xb] + 0xc + iVar8) = 0;
    (**(code **)(*param_1 + 100))();
    pcVar6 = FUN_10009a10(param_1,(ushort)param_3);
    if (1 < *(int *)(DAT_10037294 + 0x30)) {
      DevMsg("Loading %s\n",pcVar6);
    }
    FUN_100179e0(&stack0xffffffc8,0,0,0);
    uVar7 = FUN_10007410(param_1,param_3,pcVar6,(int *)&stack0xffffffc8);
    if ((char)uVar7 == '\0') {
      if ((*(byte *)(*(int *)(param_1[0xb] + 0xc + iVar8) + 0x18) & 4) != 0) {
LAB_1000dd1b:
        Error("Model %s not found and %s couldn\'t be loaded",pcVar6,"models/error.mdl");
        if (*(int *)(DAT_1003783c + 0x30) != 0) {
          piVar1 = (int *)(iStack_4 + 0x50);
          *piVar1 = *piVar1 + -1;
          if (*piVar1 == 0) {
            LOCK();
            *(undefined4 *)(iStack_4 + 0x4c) = 0;
            UNLOCK();
          }
        }
        FUN_100072a0((int *)&stack0xffffffc8);
        (**(code **)(*param_1 + 0x68))();
        return (ulonglong)extraout_EDX << 0x20;
      }
      FUN_10004d60((int *)&stack0xffffffc8);
      puVar2 = (ushort *)(*(int *)(param_1[0xb] + 0xc + iVar8) + 0x18);
      *puVar2 = *puVar2 | 4;
      uVar7 = FUN_10007410(param_1,param_3,"models/error.mdl",(int *)&stack0xffffffc8);
      if ((char)uVar7 == '\0') goto LAB_1000dd1b;
    }
    uVar7 = FUN_1000d4e0(param_1,param_3,0,0,unaff_EBP,iStack_1c,'\x01');
    if ((char)uVar7 != '\0') {
      iVar5 = (**(code **)(*(int *)param_1[4] + 0x30))
                        (**(undefined4 **)(param_1[0xb] + 0xc + iVar8),1);
    }
    FUN_100072a0((int *)&stack0xffffffc8);
    (**(code **)(*param_1 + 0x68))();
  }
  else if (*(int *)(DAT_1003783c + 0x30) != 0) {
    (**(code **)(*(int *)param_1[4] + 0x28))(**(undefined4 **)(param_1[0xb] + 0xc + iVar8));
    *(int *)(*(int *)(param_1[0xb] + 0xc + iVar8) + 0x40) = iVar5;
  }
  param_2 = DAT_1003783c;
  if (*(int *)(DAT_1003783c + 0x30) != 0) {
    piVar1 = (int *)(iStack_4 + 0x50);
    *piVar1 = *piVar1 + -1;
    if (*piVar1 == 0) {
      LOCK();
      param_2 = *(uint *)(iStack_4 + 0x4c);
      *(uint *)(iStack_4 + 0x4c) = 0;
      UNLOCK();
    }
  }
LAB_1000de04:
  return CONCAT44(param_2,iVar5);
}


// 1000de10 FUN_1000de10

longlong FUN_1000de10(uint *param_1,char *param_2)

{
  uint uVar1;
  longlong lVar2;
  
  uVar1 = FUN_1000bb20(&DAT_10037868,param_2);
  *param_1 = uVar1 & 0xffff;
  lVar2 = FUN_1000dbc0((int *)&DAT_10037868,(uint)param_1,uVar1 & 0xffff);
  return lVar2;
}


// 1000de40 FUN_1000de40

longlong __fastcall FUN_1000de40(ushort *param_1,uint param_2)

{
  longlong lVar1;
  
  lVar1 = FUN_1000dbc0((int *)&DAT_10037868,param_2,(uint)*param_1);
  return lVar1;
}


// 1000df10 FUN_1000df10

char * FUN_1000df10(void)

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
    VirtualQuery(&DAT_100379c0,&_Stack_1c,0x1c);
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


// 1000dfa0 FUN_1000dfa0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1000dfa0(undefined4 param_1)

{
  if ((_DAT_100379c8 & 1) == 0) {
    _DAT_100379c8 = _DAT_100379c8 | 1;
    DAT_100379c4 = FUN_1000df10();
  }
  if (DAT_100379c4 != (char *)0x0) {
    (**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_100379c4,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  return;
}


// 1000e000 FUN_1000e000

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_1000e000(size_t param_1)

{
  void *pvVar1;
  
  if ((_DAT_100379c8 & 1) == 0) {
    _DAT_100379c8 = _DAT_100379c8 | 1;
    DAT_100379c4 = FUN_1000df10();
  }
  if (DAT_100379c4 != (char *)0x0) {
    pvVar1 = (void *)(**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_100379c4,0);
    _memset(pvVar1,0,param_1);
    return pvVar1;
  }
  pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  _memset(pvVar1,0,param_1);
  return pvVar1;
}


// 1000e080 FUN_1000e080

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1000e080(undefined4 param_1,undefined4 param_2)

{
  if ((_DAT_100379d0 & 1) == 0) {
    _DAT_100379d0 = _DAT_100379d0 | 1;
    DAT_100379cc = FUN_1000df10();
  }
  if (DAT_100379cc != (char *)0x0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 8))(param_1,param_2,DAT_100379cc,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(param_1,param_2);
  return;
}


// 1000e0e0 FUN_1000e0e0

void __cdecl FUN_1000e0e0(int param_1,int param_2)

{
  FUN_1000e000(param_1 * param_2);
  return;
}


// 1000e110 FUN_1000e110

void __cdecl FUN_1000e110(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x20))(param_1);
  return;
}


// 1000e130 FUN_1000e130

bool FUN_1000e130(void)

{
  return *(int *)g_pMemAlloc_exref != 0;
}


// 1000e140 FUN_1000e140

void __cdecl FUN_1000e140(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1);
  return;
}


// 1000e170 FUN_1000e170

int __fastcall FUN_1000e170(int param_1)

{
  DWORD DVar1;
  int iVar2;
  DWORD *pDVar3;
  
  DVar1 = GetCurrentThreadId();
  iVar2 = 0;
  if (0 < *(int *)(param_1 + 0x9c)) {
    pDVar3 = (DWORD *)(param_1 + 0x5c);
    do {
      if (*pDVar3 == DVar1) {
        return iVar2;
      }
      iVar2 = iVar2 + 1;
      pDVar3 = pDVar3 + 1;
    } while (iVar2 < *(int *)(param_1 + 0x9c));
  }
  CThreadMutex::Lock((CThreadMutex *)(param_1 + 0xa0));
  iVar2 = *(int *)(param_1 + 0x9c);
  *(DWORD *)(param_1 + 0x5c + iVar2 * 4) = DVar1;
  *(int *)(param_1 + 0x9c) = *(int *)(param_1 + 0x9c) + 1;
  LeaveCriticalSection((LPCRITICAL_SECTION)(param_1 + 0xa0));
  return iVar2;
}


// 1000e220 FUN_1000e220

void __thiscall FUN_1000e220(void *this,int param_1)

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
      iVar2 = 1;
    }
    if ((int)uVar4 <= iVar2) goto LAB_1000e27b;
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
LAB_1000e27b:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 * 0x74);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 1000e2b0 FUN_1000e2b0

void __fastcall FUN_1000e2b0(int *param_1)

{
  (**(code **)(*param_1 + 0x18))();
  _memset(param_1 + 7,0xff,0x40);
  if ((DAT_10038854 != 0) && ((char)param_1[1] != '\0')) {
    FUN_10016af0();
    return;
  }
  return;
}


// 1000e470 FUN_1000e470

int __thiscall FUN_1000e470(void *this,int param_1)

{
  int iVar1;
  void *pvVar2;
  undefined4 *puVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_1000efa0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar2 = (void *)(*this + param_1 * 0x10);
    FUN_10016630((void *)((int)pvVar2 + 0x10),pvVar2,iVar1 * 0x10);
  }
                    /* WARNING: Load size is inaccurate */
  puVar3 = (undefined4 *)(param_1 * 0x10 + *this);
  if (puVar3 != (undefined4 *)0x0) {
    FUN_10017bd0(puVar3);
  }
  return param_1;
}


// 1000e4e0 FUN_1000e4e0

void __fastcall FUN_1000e4e0(int *param_1)

{
  int iVar1;
  int *piVar2;
  int iVar3;
  
  iVar3 = param_1[3] + -1;
  if (-1 < iVar3) {
    iVar1 = iVar3 * 0x10;
    do {
      piVar2 = (int *)(*param_1 + iVar1);
      if (-1 < piVar2[2]) {
        if (*piVar2 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar2);
          *piVar2 = 0;
        }
        piVar2[1] = 0;
      }
      iVar3 = iVar3 + -1;
      iVar1 = iVar1 + -0x10;
    } while (-1 < iVar3);
  }
  param_1[3] = 0;
  return;
}


// 1000e5d0 FUN_1000e5d0

undefined4 * __fastcall FUN_1000e5d0(undefined4 *param_1)

{
  undefined4 *puVar1;
  int iVar2;
  
  FUN_10017bd0(param_1);
  puVar1 = param_1 + 4;
  iVar2 = 4;
  do {
    *puVar1 = 0;
    puVar1[1] = 0;
    puVar1[2] = 0;
    puVar1[3] = 0;
    puVar1[4] = 0;
    puVar1 = puVar1 + 5;
    iVar2 = iVar2 + -1;
  } while (-1 < iVar2);
  return param_1;
}


// 1000e600 FUN_1000e600

void __fastcall FUN_1000e600(int *param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  int *piVar4;
  
  iVar2 = 4;
  piVar3 = param_1 + 0x1d;
  do {
    piVar4 = piVar3 + -5;
    FUN_1000e4e0(piVar4);
    if (-1 < piVar3[-3]) {
      if (*piVar4 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar4);
        *piVar4 = 0;
      }
      piVar3[-4] = 0;
    }
    iVar1 = *piVar4;
    piVar3[-1] = iVar1;
    if (-1 < piVar3[-3]) {
      if (iVar1 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
        *piVar4 = 0;
      }
      piVar3[-4] = 0;
    }
    iVar2 = iVar2 + -1;
    piVar3 = piVar4;
  } while (-1 < iVar2);
  if (-1 < param_1[2]) {
    if (*param_1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
      *param_1 = 0;
    }
    param_1[1] = 0;
  }
  return;
}


// 1000e690 FUN_1000e690

int __thiscall FUN_1000e690(void *this,int param_1)

{
  int iVar1;
  void *pvVar2;
  undefined4 *puVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_1000e220(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar2 = (void *)(*this + param_1 * 0x74);
    FUN_10016630((void *)((int)pvVar2 + 0x74),pvVar2,iVar1 * 0x74);
  }
                    /* WARNING: Load size is inaccurate */
  puVar3 = (undefined4 *)(param_1 * 0x74 + *this);
  if (puVar3 != (undefined4 *)0x0) {
    FUN_1000e5d0(puVar3);
  }
  return param_1;
}


// 1000e730 FUN_1000e730

void __fastcall FUN_1000e730(int *param_1)

{
  int iVar1;
  int iVar2;
  
  iVar2 = param_1[3] + -1;
  if (-1 < iVar2) {
    iVar1 = iVar2 * 0x74;
    do {
      FUN_1000e600((int *)(*param_1 + iVar1));
      iVar2 = iVar2 + -1;
      iVar1 = iVar1 + -0x74;
    } while (-1 < iVar2);
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


// 1000e7a0 FUN_1000e7a0

void __fastcall FUN_1000e7a0(int param_1)

{
  FUN_1000e730((int *)(param_1 + 8));
  _memset((void *)(param_1 + 0x1c),0xff,0x40);
  return;
}


// 1000e7e0 FUN_1000e7e0

void __thiscall FUN_1000e7e0(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000e83b;
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
LAB_1000e83b:
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


// 1000e880 FUN_1000e880

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * __thiscall FUN_1000e880(void *this,ushort param_1)

{
  if ((_DAT_10037a40 & 1) == 0) {
    _DAT_10037a40 = _DAT_10037a40 | 1;
    _DAT_10037a38 = 0xffff;
    _DAT_10037a3a = 0xffff;
    _DAT_10037a3c = 0xffff;
    _DAT_10037a3e = 1;
  }
  if (param_1 != 0xffff) {
    return (undefined *)((uint)param_1 * 0x58 + *(int *)((int)this + 4));
  }
  return &DAT_10037a38;
}


// 1000e8e0 FUN_1000e8e0

int __thiscall FUN_1000e8e0(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  uint extraout_EDX;
  longlong lVar3;
  
  if (*(int *)((int)this + 0x150) == 0) {
    if ((param_1 < 0) || (*(int *)((int)this + 0xbc) <= param_1)) {
      param_1 = 0;
    }
    return param_1 * 0xd4 + *(int *)((int)this + 0xc0) + (int)this;
  }
  iVar1 = FUN_1000d280(this);
  if (iVar1 == 0) {
    if ((param_1 < 0) || (*(int *)((int)this + 0xbc) <= param_1)) {
      param_1 = 0;
    }
    return param_1 * 0xd4 + *(int *)((int)this + 0xc0) + (int)this;
  }
  lVar3 = FUN_1000de40((ushort *)
                       (*(int *)(*(int *)(iVar1 + 8) + 8 + param_1 * 0x10) * 0x90 +
                       *(int *)(iVar1 + 0x58)),extraout_EDX);
  iVar2 = (int)lVar3;
  iVar1 = *(int *)(*(int *)(iVar1 + 8) + 0xc + param_1 * 0x10);
  if ((iVar1 < 0) || (*(int *)(iVar2 + 0xbc) <= iVar1)) {
    iVar1 = 0;
  }
  return iVar1 * 0xd4 + *(int *)(iVar2 + 0xc0) + iVar2;
}


// 1000e990 FUN_1000e990

int __fastcall FUN_1000e990(void *param_1)

{
  int iVar1;
  int iVar2;
  int iVar3;
  
  iVar2 = 0;
  iVar3 = 0;
  while( true ) {
    if (*(int *)((int)param_1 + 0x150) == 0) {
      iVar1 = *(int *)((int)param_1 + 0xbc);
    }
    else {
      iVar1 = FUN_1000d280(param_1);
      iVar1 = *(int *)(iVar1 + 0x14);
    }
    if (iVar1 <= iVar3) break;
    iVar1 = FUN_1000e8e0(param_1,iVar3);
    if ((*(byte *)(iVar1 + 0xc) & 8) != 0) {
      iVar2 = iVar2 + 1;
    }
    iVar3 = iVar3 + 1;
  }
  return iVar2;
}


// 1000e9e0 FUN_1000e9e0

int __thiscall FUN_1000e9e0(void *this,int param_1,int param_2)

{
  int iVar1;
  int iVar2;
  int iVar3;
  
  iVar2 = 0;
  iVar3 = 0;
  while( true ) {
    if (*(int *)((int)this + 0x150) == 0) {
      iVar1 = *(int *)((int)this + 0xbc);
    }
    else {
      iVar1 = FUN_1000d280(this);
      iVar1 = *(int *)(iVar1 + 0x14);
    }
    if ((iVar1 <= iVar3) || (param_2 <= iVar2)) break;
    iVar1 = FUN_1000e8e0(this,iVar3);
    if ((*(byte *)(iVar1 + 0xc) & 8) != 0) {
      *(short *)(param_1 + iVar2 * 2) = (short)iVar3;
      iVar2 = iVar2 + 1;
    }
    iVar3 = iVar3 + 1;
  }
  return iVar2;
}


// 1000ea40 FUN_1000ea40

int __thiscall FUN_1000ea40(void *this,int param_1,int param_2,undefined4 *param_3)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  int iVar4;
  
  if (param_2 == 0) {
    return param_1;
  }
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + param_2) {
    FUN_1000efa0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + param_2);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
                    /* WARNING: Load size is inaccurate */
  iVar4 = *this;
  iVar1 = (*(int *)((int)this + 0xc) - param_1) - param_2;
  *(int *)((int)this + 0x10) = iVar4;
  if (0 < iVar1) {
    if (param_2 < 1) goto LAB_1000eacf;
    FUN_10016630((void *)((param_1 + param_2) * 0x10 + iVar4),(void *)(param_1 * 0x10 + iVar4),
                 iVar1 * 0x10);
  }
  if (0 < param_2) {
    iVar1 = param_1 << 4;
    iVar4 = param_2;
    do {
                    /* WARNING: Load size is inaccurate */
      puVar2 = (undefined4 *)(*this + iVar1);
      if (puVar2 != (undefined4 *)0x0) {
        *puVar2 = 0xffffffff;
        puVar2[1] = 0xffffffff;
        puVar2[2] = 0xffffffff;
        puVar2[3] = 0xffffffff;
      }
      iVar1 = iVar1 + 0x10;
      iVar4 = iVar4 + -1;
    } while (iVar4 != 0);
  }
LAB_1000eacf:
  if ((param_3 != (undefined4 *)0x0) && (0 < param_2)) {
    iVar4 = param_1 << 4;
    do {
                    /* WARNING: Load size is inaccurate */
      puVar3 = (undefined4 *)(*this + iVar4);
      *puVar3 = *param_3;
      puVar3[1] = param_3[1];
      puVar3[2] = param_3[2];
      puVar2 = param_3 + 3;
      param_3 = param_3 + 4;
      iVar4 = iVar4 + 0x10;
      param_2 = param_2 + -1;
      puVar3[3] = *puVar2;
    } while (param_2 != 0);
  }
  return param_1;
}


// 1000eb10 FUN_1000eb10

void __fastcall FUN_1000eb10(void *param_1)

{
  ushort uVar1;
  ushort uVar2;
  ushort uVar3;
  ushort *puVar4;
  int iVar5;
  
  uVar1 = *(ushort *)((int)param_1 + 0x16);
  if (uVar1 != 0xffff) {
    uVar2 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (uVar2 != 0xffff) {
      if ((((uVar2 == 0) || (uVar2 != 0)) && ((int)(uint)uVar2 < *(int *)((int)param_1 + 8))) &&
         ((uVar2 <= uVar1 && (puVar4 = (ushort *)FUN_1000e880(param_1,uVar2), *puVar4 != uVar2)))) {
        iVar5 = (uint)uVar2 * 0x58;
        *(undefined2 *)(iVar5 + 2 + *(int *)((int)param_1 + 4)) =
             *(undefined2 *)((int)param_1 + 0x14);
        *(ushort *)(iVar5 + *(int *)((int)param_1 + 4)) = uVar2;
        *(ushort *)((int)param_1 + 0x14) = uVar2;
      }
      uVar1 = *(ushort *)((int)param_1 + 0x16);
      if (uVar2 == uVar1) break;
      uVar3 = uVar2 + 1;
      if (((uVar3 != 0) && (uVar3 == 0)) || (*(int *)((int)param_1 + 8) <= (int)(uint)uVar3)) {
        uVar2 = 0xffff;
      }
      else {
        uVar2 = uVar2 + 1;
      }
    }
    *(undefined2 *)((int)param_1 + 0x10) = 0xffff;
    *(undefined2 *)((int)param_1 + 0x12) = 0;
  }
  return;
}


// 1000ebe0 FUN_1000ebe0

void __fastcall FUN_1000ebe0(void *param_1)

{
  FUN_1000eb10(param_1);
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


// 1000ec40 FUN_1000ec40

void __fastcall FUN_1000ec40(int *param_1)

{
  int iVar1;
  int *piVar2;
  int iVar3;
  
  iVar1 = param_1[3];
  if (0 < iVar1) {
    iVar3 = 0;
    do {
      piVar2 = (int *)(*param_1 + iVar3);
      piVar2[3] = 0;
      if (-1 < piVar2[2]) {
        if (*piVar2 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar2);
          *piVar2 = 0;
        }
        piVar2[1] = 0;
      }
      iVar3 = iVar3 + 0x14;
      iVar1 = iVar1 + -1;
      piVar2[4] = *piVar2;
    } while (iVar1 != 0);
  }
  return;
}


// 1000eca0 FUN_1000eca0

int * __thiscall FUN_1000eca0(void *this,int *param_1)

{
  int iVar1;
  undefined4 *puVar2;
  int iVar3;
  int iVar4;
  
  iVar4 = param_1[3];
  *(undefined4 *)((int)this + 0xc) = 0;
  FUN_1000ea40(this,0,iVar4,(undefined4 *)0x0);
  if (0 < iVar4) {
    iVar3 = 0;
    do {
                    /* WARNING: Load size is inaccurate */
      iVar1 = *param_1 + iVar3;
      puVar2 = (undefined4 *)(*this + iVar3);
      *puVar2 = *(undefined4 *)(*param_1 + iVar3);
      puVar2[1] = *(undefined4 *)(iVar1 + 4);
      puVar2[2] = *(undefined4 *)(iVar1 + 8);
      iVar3 = iVar3 + 0x10;
      iVar4 = iVar4 + -1;
      puVar2[3] = *(undefined4 *)(iVar1 + 0xc);
    } while (iVar4 != 0);
  }
  return (int *)this;
}


// 1000ed00 FUN_1000ed00

void __fastcall FUN_1000ed00(int *param_1)

{
  int iVar1;
  int iVar2;
  int iVar3;
  int *piVar4;
  
  iVar2 = param_1[3] + -1;
  if (-1 < iVar2) {
    iVar3 = iVar2 * 0x14;
    do {
      piVar4 = (int *)(*param_1 + iVar3);
      piVar4[3] = 0;
      if (-1 < piVar4[2]) {
        if (*piVar4 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar4);
          *piVar4 = 0;
        }
        piVar4[1] = 0;
      }
      iVar1 = *piVar4;
      piVar4[4] = iVar1;
      if (-1 < piVar4[2]) {
        if (iVar1 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
          *piVar4 = 0;
        }
        piVar4[1] = 0;
      }
      iVar2 = iVar2 + -1;
      iVar3 = iVar3 + -0x14;
    } while (-1 < iVar2);
    param_1[3] = 0;
    return;
  }
  param_1[3] = 0;
  return;
}


// 1000ed90 FUN_1000ed90

int __thiscall FUN_1000ed90(void *this,int param_1,int param_2,int *param_3)

{
  int iVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (param_2 == 0) {
    return param_1;
  }
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + param_2) {
    FUN_1000e7e0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + param_2);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
                    /* WARNING: Load size is inaccurate */
  iVar3 = *this;
  iVar1 = (*(int *)((int)this + 0xc) - param_1) - param_2;
  *(int *)((int)this + 0x10) = iVar3;
  if (0 < iVar1) {
    if (param_2 < 1) goto LAB_1000ee2c;
    FUN_10016630((void *)(iVar3 + (param_1 + param_2) * 0x14),(void *)(iVar3 + param_1 * 0x14),
                 iVar1 * 0x14);
  }
  if (0 < param_2) {
    iVar3 = param_1 * 0x14;
    iVar1 = param_2;
    do {
                    /* WARNING: Load size is inaccurate */
      puVar2 = (undefined4 *)(*this + iVar3);
      if (puVar2 != (undefined4 *)0x0) {
        *puVar2 = 0;
        puVar2[1] = 0;
        puVar2[2] = 0;
        puVar2[3] = 0;
        puVar2[4] = 0;
      }
      iVar3 = iVar3 + 0x14;
      iVar1 = iVar1 + -1;
    } while (iVar1 != 0);
  }
LAB_1000ee2c:
  if ((param_3 != (int *)0x0) && (0 < param_2)) {
    iVar3 = param_1 * 0x14;
    do {
                    /* WARNING: Load size is inaccurate */
      FUN_1000eca0((void *)(*this + iVar3),param_3);
      param_3 = param_3 + 5;
      iVar3 = iVar3 + 0x14;
      param_2 = param_2 + -1;
    } while (param_2 != 0);
  }
  return param_1;
}


// 1000ee70 FUN_1000ee70

void __fastcall FUN_1000ee70(int *param_1)

{
  int iVar1;
  
  FUN_1000ed00(param_1);
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


// 1000eed0 FUN_1000eed0

int * __thiscall FUN_1000eed0(void *this,uint param_1,undefined4 param_2,int param_3)

{
  int iVar1;
  uint uVar2;
  int iVar3;
  bool bVar4;
  
  iVar3 = 0;
  *(undefined4 *)this = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined4 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  FUN_1000ed00((int *)this);
  FUN_1000ed90(this,*(int *)((int)this + 0xc),param_1,(int *)0x0);
  uVar2 = param_1;
  if (0 < (int)param_1) {
    do {
                    /* WARNING: Load size is inaccurate */
      iVar1 = *this;
      *(undefined4 *)(iVar1 + iVar3 + 0xc) = 0;
      FUN_1000ea40((void *)(iVar1 + iVar3),0,param_3,(undefined4 *)0x0);
                    /* WARNING: Load size is inaccurate */
      *(undefined4 *)(iVar3 + 8 + *this) = param_2;
      iVar3 = iVar3 + 0x14;
      uVar2 = uVar2 - 1;
    } while (uVar2 != 0);
  }
  bVar4 = (param_1 & param_1 - 1) == 0;
  *(bool *)((int)this + 0x16) = bVar4;
  *(uint *)((int)this + 0x18) = -(uint)bVar4 & param_1 - 1;
  return (int *)this;
}


// 1000ef50 FUN_1000ef50

void __fastcall FUN_1000ef50(DWORD *param_1)

{
  code *pcVar1;
  DWORD DVar2;
  DWORD DVar3;
  
  DVar2 = GetCurrentThreadId();
  if (DVar2 != *param_1) {
    LOCK();
    DVar3 = *param_1;
    if (DVar3 == 0) {
      *param_1 = DVar2;
      DVar3 = 0;
    }
    UNLOCK();
    if (DVar3 != 0) {
      pcVar1 = (code *)swi(3);
      (*pcVar1)();
      return;
    }
  }
  param_1[1] = param_1[1] + 1;
  return;
}


// 1000efa0 FUN_1000efa0

void __thiscall FUN_1000efa0(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000effb;
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
LAB_1000effb:
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


// 1000f030 FUN_1000f030

void __thiscall FUN_1000f030(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000f08b;
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
LAB_1000f08b:
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


// 1000f0c0 FUN_1000f0c0

void __thiscall FUN_1000f0c0(void *this,int param_1)

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
      iVar2 = 1;
    }
    if ((int)uVar4 <= iVar2) goto LAB_1000f11b;
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
LAB_1000f11b:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 * 0x38);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this,iVar2 * 0x38);
  *(undefined4 *)this = uVar3;
  return;
}


// 1000f170 FUN_1000f170

undefined2 * __thiscall FUN_1000f170(void *this,short param_1)

{
  if ((DAT_10037ae4 & 1) == 0) {
    DAT_10037ae4 = DAT_10037ae4 | 1;
    DAT_10037adc = 0xffff;
    DAT_10037ade = 0xffff;
    DAT_10037ae0 = 0xffff;
    DAT_10037ae2 = 1;
  }
  if (param_1 != -1) {
    return (undefined2 *)(param_1 * 0x10 + *(int *)((int)this + 4));
  }
  return &DAT_10037adc;
}


// 1000f1c0 FUN_1000f1c0

void __thiscall FUN_1000f1c0(void *this,short *param_1,short *param_2)

{
  short sVar1;
  
  sVar1 = *param_2 + 1;
  if (((sVar1 == 0) || (0 < sVar1)) && ((int)sVar1 < *(int *)((int)this + 4))) {
    *param_1 = *param_2 + 1;
    return;
  }
  *param_1 = -1;
  return;
}


// 1000f200 FUN_1000f200

void __thiscall FUN_1000f200(void *this,int param_1)

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
      iVar2 = 0x10;
    }
    if ((int)uVar4 <= iVar2) goto LAB_1000f25b;
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
LAB_1000f25b:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 * 2);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 1000f290 FUN_1000f290

void __thiscall FUN_1000f290(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  undefined4 uVar3;
  uint uVar4;
  
  iVar1 = *(int *)((int)this + 8);
  if (-1 < iVar1) {
    iVar2 = *(int *)((int)this + 4);
    uVar4 = param_1 + iVar2;
    if (iVar1 == 0) {
      if (iVar2 == 0) {
        iVar2 = 2;
      }
      for (; iVar2 < (int)uVar4; iVar2 = iVar2 * 2) {
      }
    }
    else {
      iVar2 = ((int)(uVar4 - 1) / iVar1 + 1) * iVar1;
    }
    if ((int)(short)iVar2 < (int)uVar4) {
      if (((short)iVar2 == 0) && (0x7fffffff < uVar4)) {
        iVar2 = iVar2 + -1;
      }
      else {
        if ((int)(short)uVar4 != uVar4) {
          return;
        }
        do {
          iVar2 = (int)(iVar2 + uVar4) / 2;
        } while ((int)(short)iVar2 < (int)uVar4);
      }
    }
                    /* WARNING: Load size is inaccurate */
    *(int *)((int)this + 4) = iVar2;
    if (*this != 0) {
      uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
      *(undefined4 *)this = uVar3;
      return;
    }
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 << 4);
    *(undefined4 *)this = uVar3;
  }
  return;
}


// 1000f330 FUN_1000f330

short * __thiscall FUN_1000f330(void *this,undefined4 param_1,short *param_2,undefined1 *param_3)

{
  short sVar1;
  char cVar2;
  short *psVar3;
  int iVar4;
  
  sVar1 = *(short *)((int)this + 0x10);
  *param_2 = -1;
  *param_3 = 0;
  psVar3 = param_2;
  while (sVar1 != -1) {
    *param_2 = sVar1;
                    /* WARNING: Load size is inaccurate */
    iVar4 = sVar1 * 0x10;
    cVar2 = (**this)(param_1,iVar4 + 8 + *(int *)((int)this + 4));
    if (cVar2 == '\0') {
      *param_3 = 0;
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = -1;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      if (sVar1 == -1) {
        psVar3 = &DAT_10037adc;
      }
      else {
        psVar3 = (short *)(*(int *)((int)this + 4) + iVar4);
      }
      sVar1 = psVar3[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = -1;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      if (sVar1 == -1) {
        psVar3 = &DAT_10037adc;
        sVar1 = DAT_10037adc;
      }
      else {
        psVar3 = (short *)(*(int *)((int)this + 4) + iVar4);
        sVar1 = *(short *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  }
  return psVar3;
}


// 1000f430 FUN_1000f430

short __fastcall FUN_1000f430(int param_1)

{
  short *psVar1;
  short sVar2;
  short *psVar3;
  int iVar4;
  short local_2;
  
  sVar2 = *(short *)(param_1 + 0x14);
  if (sVar2 != -1) {
    *(undefined2 *)(param_1 + 0x14) = *(undefined2 *)(sVar2 * 0x10 + 2 + *(int *)(param_1 + 4));
    *(int *)(param_1 + 0x18) = *(int *)(param_1 + 4);
    return sVar2;
  }
  sVar2 = *(short *)(param_1 + 0x16);
  psVar1 = (short *)(param_1 + 0x16);
  if (((sVar2 == 0) || (0 < sVar2)) && (iVar4 = *(int *)(param_1 + 8), sVar2 < iVar4)) {
    sVar2 = *psVar1 + 1;
    if (((sVar2 == 0) || (0 < sVar2)) && (sVar2 < iVar4)) {
      sVar2 = *psVar1 + 1;
    }
    else {
      sVar2 = -1;
    }
  }
  else {
    iVar4 = *(int *)(param_1 + 8);
    sVar2 = (0 < iVar4) - 1;
  }
  if (((sVar2 != 0) && (sVar2 < 1)) || (iVar4 <= sVar2)) {
    FUN_1000f290((void *)(param_1 + 4),1);
    sVar2 = *psVar1;
    if (((sVar2 == 0) || (0 < sVar2)) && (iVar4 = *(int *)(param_1 + 8), sVar2 < iVar4)) {
      psVar3 = (short *)FUN_1000f1c0((void *)(param_1 + 4),&local_2,psVar1);
    }
    else {
      iVar4 = *(int *)(param_1 + 8);
      psVar3 = &local_2;
    }
    sVar2 = *psVar3;
    if (((sVar2 != 0) && (sVar2 < 1)) || (iVar4 <= sVar2)) {
      Error("CUtlRBTree overflow!\n");
    }
  }
  *psVar1 = sVar2;
  *(undefined4 *)(param_1 + 0x18) = *(undefined4 *)(param_1 + 4);
  return sVar2;
}


// 1000f540 FUN_1000f540

int __thiscall FUN_1000f540(void *this,int param_1,int param_2,undefined4 *param_3)

{
  undefined4 *puVar1;
  int iVar2;
  undefined4 *puVar3;
  int iVar4;
  
  if (param_2 != 0) {
    if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + param_2) {
      FUN_1000efa0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + param_2);
    }
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
                    /* WARNING: Load size is inaccurate */
    iVar4 = *this;
    iVar2 = (*(int *)((int)this + 0xc) - param_1) - param_2;
    *(int *)((int)this + 0x10) = iVar4;
    if ((0 < iVar2) && (0 < param_2)) {
      FUN_10016630((void *)((param_1 + param_2) * 0x10 + iVar4),(void *)(param_1 * 0x10 + iVar4),
                   iVar2 * 0x10);
    }
    if ((param_3 != (undefined4 *)0x0) && (0 < param_2)) {
      iVar4 = param_1 << 4;
      do {
                    /* WARNING: Load size is inaccurate */
        puVar3 = (undefined4 *)(*this + iVar4);
        *puVar3 = *param_3;
        puVar3[1] = param_3[1];
        puVar3[2] = param_3[2];
        puVar1 = param_3 + 3;
        param_3 = param_3 + 4;
        iVar4 = iVar4 + 0x10;
        param_2 = param_2 + -1;
        puVar3[3] = *puVar1;
      } while (param_2 != 0);
    }
    return param_1;
  }
  return param_1;
}


// 1000f5f0 FUN_1000f5f0

int __thiscall FUN_1000f5f0(void *this,int param_1,int param_2,undefined4 *param_3)

{
  int iVar1;
  int iVar2;
  
  if (param_2 != 0) {
    if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + param_2) {
      FUN_1000f030(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + param_2);
    }
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
                    /* WARNING: Load size is inaccurate */
    iVar2 = *this;
    iVar1 = (*(int *)((int)this + 0xc) - param_1) - param_2;
    *(int *)((int)this + 0x10) = iVar2;
    if ((0 < iVar1) && (0 < param_2)) {
      FUN_10016630((void *)(iVar2 + (param_1 + param_2) * 8),(void *)(iVar2 + param_1 * 8),iVar1 * 8
                  );
    }
    if ((param_3 != (undefined4 *)0x0) && (0 < param_2)) {
      iVar2 = param_1 * 8;
      do {
                    /* WARNING: Load size is inaccurate */
        iVar1 = *this;
        *(undefined4 *)(iVar2 + iVar1) = *param_3;
        *(undefined4 *)(iVar2 + 4 + iVar1) = param_3[1];
        param_3 = param_3 + 2;
        iVar2 = iVar2 + 8;
        param_2 = param_2 + -1;
      } while (param_2 != 0);
    }
    return param_1;
  }
  return param_1;
}


// 1000f690 FUN_1000f690

void __fastcall FUN_1000f690(int param_1)

{
  short sVar1;
  short sVar2;
  short *psVar3;
  uint uVar4;
  
  uVar4 = DAT_10037ae4;
  sVar1 = *(short *)(param_1 + 0x10);
  while( true ) {
    sVar2 = sVar1;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_10037adc = -1;
      DAT_10037ade = 0xffff;
      DAT_10037ae0 = 0xffff;
      DAT_10037ae2 = 1;
      DAT_10037ae4 = uVar4;
    }
    if (sVar2 == -1) {
      psVar3 = &DAT_10037adc;
    }
    else {
      psVar3 = (short *)(sVar2 * 0x10 + *(int *)(param_1 + 4));
    }
    if (*psVar3 == -1) break;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_10037adc = -1;
      DAT_10037ade = 0xffff;
      DAT_10037ae0 = 0xffff;
      DAT_10037ae2 = 1;
      DAT_10037ae4 = uVar4;
    }
    sVar1 = DAT_10037adc;
    if (sVar2 != -1) {
      sVar1 = *(short *)(sVar2 * 0x10 + *(int *)(param_1 + 4));
    }
  }
  return;
}


// 1000f740 FUN_1000f740

void __thiscall FUN_1000f740(void *this,short param_1)

{
  short sVar1;
  short sVar2;
  undefined2 *puVar3;
  short *psVar4;
  uint uVar5;
  
  puVar3 = FUN_1000f170(this,param_1);
  if (puVar3[1] == -1) {
    puVar3 = FUN_1000f170(this,param_1);
    uVar5 = DAT_10037ae4;
    while( true ) {
      sVar1 = puVar3[2];
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037adc = -1;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
        DAT_10037ae4 = uVar5;
      }
      if (param_1 == -1) {
        puVar3 = &DAT_10037adc;
      }
      else {
        puVar3 = (undefined2 *)(param_1 * 0x10 + *(int *)((int)this + 4));
      }
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037adc = -1;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
        DAT_10037ae4 = uVar5;
      }
      if (puVar3[2] == -1) {
        puVar3 = &DAT_10037adc;
      }
      else {
        puVar3 = (undefined2 *)((short)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if ((puVar3[1] != param_1) || (sVar1 == -1)) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037adc = -1;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
        DAT_10037ae4 = uVar5;
      }
      puVar3 = (undefined2 *)(sVar1 * 0x10 + *(int *)((int)this + 4));
      param_1 = sVar1;
    }
  }
  else {
    puVar3 = FUN_1000f170(this,param_1);
    uVar5 = DAT_10037ae4;
    sVar1 = puVar3[1];
    while( true ) {
      sVar2 = sVar1;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037adc = -1;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
        DAT_10037ae4 = uVar5;
      }
      if (sVar2 == -1) {
        psVar4 = &DAT_10037adc;
      }
      else {
        psVar4 = (short *)(sVar2 * 0x10 + *(int *)((int)this + 4));
      }
      if (*psVar4 == -1) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10037adc = -1;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
        DAT_10037ae4 = uVar5;
      }
      sVar1 = DAT_10037adc;
      if (sVar2 != -1) {
        sVar1 = *(short *)(sVar2 * 0x10 + *(int *)((int)this + 4));
      }
    }
  }
  return;
}


// 1000f8f0 FUN_1000f8f0

void __thiscall FUN_1000f8f0(void *this,short param_1)

{
  short sVar1;
  undefined2 *puVar2;
  short *psVar3;
  
  puVar2 = FUN_1000f170(this,param_1);
  sVar1 = puVar2[1];
  puVar2 = FUN_1000f170(this,sVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + 2 + param_1 * 0x10) = *puVar2;
  psVar3 = FUN_1000f170(this,sVar1);
  if (*psVar3 != -1) {
    psVar3 = FUN_1000f170(this,sVar1);
    *(short *)(*psVar3 * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (sVar1 != -1) {
    puVar2 = FUN_1000f170(this,param_1);
    *(undefined2 *)(sVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar2[2];
  }
  if (param_1 == *(short *)((int)this + 0x10)) {
    *(short *)((int)this + 0x10) = sVar1;
  }
  else {
    puVar2 = FUN_1000f170(this,param_1);
    psVar3 = FUN_1000f170(this,puVar2[2]);
    if (*psVar3 == param_1) {
      puVar2 = FUN_1000f170(this,param_1);
      *(short *)((short)puVar2[2] * 0x10 + *(int *)((int)this + 4)) = sVar1;
    }
    else {
      puVar2 = FUN_1000f170(this,param_1);
      *(short *)((short)puVar2[2] * 0x10 + 2 + *(int *)((int)this + 4)) = sVar1;
    }
  }
  *(short *)(sVar1 * 0x10 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != -1) {
    *(short *)(*(int *)((int)this + 4) + 4 + param_1 * 0x10) = sVar1;
  }
  return;
}


// 1000f9e0 FUN_1000f9e0

void __thiscall FUN_1000f9e0(void *this,short param_1)

{
  short sVar1;
  short *psVar2;
  undefined2 *puVar3;
  
  psVar2 = FUN_1000f170(this,param_1);
  sVar1 = *psVar2;
  puVar3 = FUN_1000f170(this,sVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + param_1 * 0x10) = puVar3[1];
  puVar3 = FUN_1000f170(this,sVar1);
  if (puVar3[1] != -1) {
    puVar3 = FUN_1000f170(this,sVar1);
    *(short *)((short)puVar3[1] * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (sVar1 != -1) {
    puVar3 = FUN_1000f170(this,param_1);
    *(undefined2 *)(sVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar3[2];
  }
  if (param_1 == *(short *)((int)this + 0x10)) {
    *(short *)((int)this + 0x10) = sVar1;
  }
  else {
    puVar3 = FUN_1000f170(this,param_1);
    puVar3 = FUN_1000f170(this,puVar3[2]);
    if (puVar3[1] == param_1) {
      puVar3 = FUN_1000f170(this,param_1);
      *(short *)((short)puVar3[2] * 0x10 + 2 + *(int *)((int)this + 4)) = sVar1;
    }
    else {
      puVar3 = FUN_1000f170(this,param_1);
      *(short *)((short)puVar3[2] * 0x10 + *(int *)((int)this + 4)) = sVar1;
    }
  }
  *(short *)(sVar1 * 0x10 + 2 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != -1) {
    *(short *)(*(int *)((int)this + 4) + 4 + param_1 * 0x10) = sVar1;
  }
  return;
}


// 1000fad0 FUN_1000fad0

int __thiscall FUN_1000fad0(void *this,int param_1,undefined4 *param_2)

{
  int iVar1;
  undefined4 *puVar2;
  void *pvVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_1000efa0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar3 = (void *)(*this + param_1 * 0x10);
    FUN_10016630((void *)((int)pvVar3 + 0x10),pvVar3,iVar1 * 0x10);
  }
                    /* WARNING: Load size is inaccurate */
  puVar2 = (undefined4 *)(param_1 * 0x10 + *this);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_2;
    puVar2[1] = param_2[1];
    puVar2[2] = param_2[2];
    puVar2[3] = param_2[3];
  }
  return param_1;
}


// 1000fb50 FUN_1000fb50

short __thiscall FUN_1000fb50(void *this,undefined4 param_1)

{
  char cVar1;
  undefined2 *puVar2;
  short sVar3;
  int iVar4;
  bool bVar5;
  
  sVar3 = *(short *)((int)this + 0x10);
  if (sVar3 == -1) {
    return -1;
  }
  do {
                    /* WARNING: Load size is inaccurate */
    iVar4 = sVar3 * 0x10;
    cVar1 = (**this)(param_1,*(int *)((int)this + 4) + 8 + iVar4);
    if (cVar1 == '\0') {
                    /* WARNING: Load size is inaccurate */
      cVar1 = (**this)(*(int *)((int)this + 4) + 8 + iVar4,param_1);
      if (cVar1 == '\0') {
        return sVar3;
      }
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = -1;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      if (sVar3 == -1) {
        puVar2 = &DAT_10037adc;
      }
      else {
        puVar2 = (undefined2 *)(*(int *)((int)this + 4) + iVar4);
      }
      sVar3 = puVar2[1];
    }
    else {
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = -1;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      bVar5 = sVar3 != -1;
      sVar3 = DAT_10037adc;
      if (bVar5) {
        sVar3 = *(short *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  } while (sVar3 != -1);
  return -1;
}


// 1000fc50 FUN_1000fc50

void __thiscall FUN_1000fc50(void *this,short param_1)

{
  short sVar1;
  undefined2 *puVar2;
  short *psVar3;
  short sVar4;
  short sVar5;
  
  if (param_1 != *(short *)((int)this + 0x10)) {
    do {
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = 0xffff;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      if (param_1 == -1) {
        puVar2 = &DAT_10037adc;
      }
      else {
        puVar2 = (undefined2 *)(param_1 * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = 0xffff;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      if (puVar2[2] == -1) {
        puVar2 = &DAT_10037adc;
      }
      else {
        puVar2 = (undefined2 *)((short)puVar2[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (puVar2[3] != 0) break;
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = 0xffff;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      if (param_1 == -1) {
        puVar2 = &DAT_10037adc;
      }
      else {
        puVar2 = (undefined2 *)(param_1 * 0x10 + *(int *)((int)this + 4));
      }
      sVar1 = puVar2[2];
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = 0xffff;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      if (sVar1 == -1) {
        puVar2 = &DAT_10037adc;
      }
      else {
        puVar2 = (undefined2 *)(sVar1 * 0x10 + *(int *)((int)this + 4));
      }
      sVar5 = puVar2[2];
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = 0xffff;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      if (sVar1 == -1) {
        puVar2 = &DAT_10037adc;
      }
      else {
        puVar2 = (undefined2 *)(sVar1 * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_10037ae4 & 1) == 0) {
        DAT_10037ae4 = DAT_10037ae4 | 1;
        DAT_10037adc = 0xffff;
        DAT_10037ade = 0xffff;
        DAT_10037ae0 = 0xffff;
        DAT_10037ae2 = 1;
      }
      if (puVar2[2] == -1) {
        psVar3 = &DAT_10037adc;
      }
      else {
        psVar3 = (short *)((short)puVar2[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (*psVar3 == sVar1) {
        if ((DAT_10037ae4 & 1) == 0) {
          DAT_10037ae4 = DAT_10037ae4 | 1;
          DAT_10037adc = 0xffff;
          DAT_10037ade = 0xffff;
          DAT_10037ae0 = 0xffff;
          DAT_10037ae2 = 1;
        }
        if (sVar5 == -1) {
          puVar2 = &DAT_10037adc;
        }
        else {
          puVar2 = (undefined2 *)(sVar5 * 0x10 + *(int *)((int)this + 4));
        }
        sVar4 = puVar2[1];
        if ((DAT_10037ae4 & 1) == 0) {
          DAT_10037ae4 = DAT_10037ae4 | 1;
          DAT_10037adc = 0xffff;
          DAT_10037ade = 0xffff;
          DAT_10037ae0 = 0xffff;
          DAT_10037ae2 = 1;
        }
        if (sVar4 == -1) {
          puVar2 = &DAT_10037adc;
        }
        else {
          puVar2 = (undefined2 *)(sVar4 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar2[3] == 0) {
LAB_10010070:
          *(undefined2 *)(sVar1 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(sVar4 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(sVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          param_1 = sVar5;
        }
        else {
          if ((DAT_10037ae4 & 1) == 0) {
            DAT_10037ae4 = DAT_10037ae4 | 1;
            DAT_10037adc = 0xffff;
            DAT_10037ade = 0xffff;
            DAT_10037ae0 = 0xffff;
            DAT_10037ae2 = 1;
          }
          if (param_1 == -1) {
            puVar2 = &DAT_10037adc;
          }
          else {
            puVar2 = (undefined2 *)(param_1 * 0x10 + *(int *)((int)this + 4));
          }
          if ((DAT_10037ae4 & 1) == 0) {
            DAT_10037ae4 = DAT_10037ae4 | 1;
            DAT_10037adc = 0xffff;
            DAT_10037ade = 0xffff;
            DAT_10037ae0 = 0xffff;
            DAT_10037ae2 = 1;
          }
          if (puVar2[2] == -1) {
            puVar2 = &DAT_10037adc;
          }
          else {
            puVar2 = (undefined2 *)((short)puVar2[2] * 0x10 + *(int *)((int)this + 4));
          }
          sVar4 = sVar1;
          if (puVar2[1] == param_1) {
            FUN_1000f8f0(this,sVar1);
            if ((DAT_10037ae4 & 1) == 0) {
              DAT_10037ae4 = DAT_10037ae4 | 1;
              DAT_10037adc = 0xffff;
              DAT_10037ade = 0xffff;
              DAT_10037ae0 = 0xffff;
              DAT_10037ae2 = 1;
            }
            if (sVar1 == -1) {
              puVar2 = &DAT_10037adc;
            }
            else {
              puVar2 = (undefined2 *)(sVar1 * 0x10 + *(int *)((int)this + 4));
            }
            sVar4 = puVar2[2];
            if ((DAT_10037ae4 & 1) == 0) {
              DAT_10037ae4 = DAT_10037ae4 | 1;
              DAT_10037adc = 0xffff;
              DAT_10037ade = 0xffff;
              DAT_10037ae0 = 0xffff;
              DAT_10037ae2 = 1;
            }
            if (sVar4 == -1) {
              puVar2 = &DAT_10037adc;
            }
            else {
              puVar2 = (undefined2 *)(sVar4 * 0x10 + *(int *)((int)this + 4));
            }
            sVar5 = puVar2[2];
            param_1 = sVar1;
          }
          *(undefined2 *)(sVar4 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(sVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_1000f9e0(this,sVar5);
        }
      }
      else {
        if ((DAT_10037ae4 & 1) == 0) {
          DAT_10037ae4 = DAT_10037ae4 | 1;
          DAT_10037adc = 0xffff;
          DAT_10037ade = 0xffff;
          DAT_10037ae0 = 0xffff;
          DAT_10037ae2 = 1;
        }
        if (sVar5 == -1) {
          psVar3 = &DAT_10037adc;
        }
        else {
          psVar3 = (short *)(sVar5 * 0x10 + *(int *)((int)this + 4));
        }
        sVar4 = *psVar3;
        if ((DAT_10037ae4 & 1) == 0) {
          DAT_10037ae4 = DAT_10037ae4 | 1;
          DAT_10037adc = 0xffff;
          DAT_10037ade = 0xffff;
          DAT_10037ae0 = 0xffff;
          DAT_10037ae2 = 1;
        }
        if (sVar4 == -1) {
          puVar2 = &DAT_10037adc;
        }
        else {
          puVar2 = (undefined2 *)(sVar4 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar2[3] == 0) goto LAB_10010070;
        if ((DAT_10037ae4 & 1) == 0) {
          DAT_10037ae4 = DAT_10037ae4 | 1;
          DAT_10037adc = 0xffff;
          DAT_10037ade = 0xffff;
          DAT_10037ae0 = 0xffff;
          DAT_10037ae2 = 1;
        }
        if (param_1 == -1) {
          puVar2 = &DAT_10037adc;
        }
        else {
          puVar2 = (undefined2 *)(param_1 * 0x10 + *(int *)((int)this + 4));
        }
        if ((DAT_10037ae4 & 1) == 0) {
          DAT_10037ae4 = DAT_10037ae4 | 1;
          DAT_10037adc = 0xffff;
          DAT_10037ade = 0xffff;
          DAT_10037ae0 = 0xffff;
          DAT_10037ae2 = 1;
        }
        if (puVar2[2] == -1) {
          psVar3 = &DAT_10037adc;
        }
        else {
          psVar3 = (short *)((short)puVar2[2] * 0x10 + *(int *)((int)this + 4));
        }
        sVar4 = sVar1;
        if (*psVar3 == param_1) {
          FUN_1000f9e0(this,sVar1);
          if ((DAT_10037ae4 & 1) == 0) {
            DAT_10037ae4 = DAT_10037ae4 | 1;
            DAT_10037adc = 0xffff;
            DAT_10037ade = 0xffff;
            DAT_10037ae0 = 0xffff;
            DAT_10037ae2 = 1;
          }
          if (sVar1 == -1) {
            puVar2 = &DAT_10037adc;
          }
          else {
            puVar2 = (undefined2 *)(sVar1 * 0x10 + *(int *)((int)this + 4));
          }
          sVar4 = puVar2[2];
          if ((DAT_10037ae4 & 1) == 0) {
            DAT_10037ae4 = DAT_10037ae4 | 1;
            DAT_10037adc = 0xffff;
            DAT_10037ade = 0xffff;
            DAT_10037ae0 = 0xffff;
            DAT_10037ae2 = 1;
          }
          if (sVar4 == -1) {
            puVar2 = &DAT_10037adc;
          }
          else {
            puVar2 = (undefined2 *)(sVar4 * 0x10 + *(int *)((int)this + 4));
          }
          sVar5 = puVar2[2];
          param_1 = sVar1;
        }
        *(undefined2 *)(sVar4 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        *(undefined2 *)(sVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
        FUN_1000f8f0(this,sVar5);
      }
    } while (param_1 != *(short *)((int)this + 0x10));
  }
  *(undefined2 *)(*(short *)((int)this + 0x10) * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
  return;
}


// 10010230 FUN_10010230

void __fastcall FUN_10010230(void *param_1)

{
  short sVar1;
  short sVar2;
  short sVar3;
  short *psVar4;
  int iVar5;
  
  sVar1 = *(short *)((int)param_1 + 0x16);
  if (sVar1 != -1) {
    sVar3 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (sVar3 != -1) {
      if ((((sVar3 == 0) || (0 < sVar3)) && ((int)sVar3 < *(int *)((int)param_1 + 8))) &&
         ((sVar3 <= sVar1 && (psVar4 = FUN_1000f170(param_1,sVar3), *psVar4 != sVar3)))) {
        iVar5 = sVar3 * 0x10;
        *(undefined2 *)(iVar5 + 2 + *(int *)((int)param_1 + 4)) =
             *(undefined2 *)((int)param_1 + 0x14);
        *(short *)(iVar5 + *(int *)((int)param_1 + 4)) = sVar3;
        *(short *)((int)param_1 + 0x14) = sVar3;
      }
      sVar1 = *(short *)((int)param_1 + 0x16);
      if (sVar3 == sVar1) break;
      sVar2 = sVar3 + 1;
      if (((sVar2 == 0) || (0 < sVar2)) && ((int)sVar2 < *(int *)((int)param_1 + 8))) {
        sVar3 = sVar3 + 1;
      }
      else {
        sVar3 = -1;
      }
    }
    *(undefined2 *)((int)param_1 + 0x10) = 0xffff;
    *(undefined2 *)((int)param_1 + 0x12) = 0;
  }
  return;
}


// 100102f0 FUN_100102f0

void __fastcall FUN_100102f0(void *param_1)

{
  short sVar1;
  
  for (sVar1 = FUN_1000f690((int)param_1); sVar1 != -1; sVar1 = FUN_1000f740(param_1,sVar1)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)(sVar1 * 0x10 + 8 + *(int *)((int)param_1 + 4)));
  }
  FUN_10010230(param_1);
  return;
}


// 10010340 FUN_10010340

DWORD __thiscall FUN_10010340(void *this,int param_1,int param_2)

{
  void *pvVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  int iVar5;
  int *piVar6;
  int iVar7;
  uint uVar8;
  uint extraout_EDX;
  int iVar9;
  bool bVar10;
  longlong lVar11;
  int local_8;
  
  iVar3 = param_2;
  FUN_1000ef50((DWORD *)this);
  lVar11 = FUN_1000de40(*(ushort **)((int)this + 0x58),extraout_EDX);
  iVar4 = (int)lVar11;
  iVar5 = *(int *)(iVar4 + 0x9c);
  iVar9 = param_1 * 0x90;
  pvVar1 = (void *)(iVar9 + 4 + *(int *)((int)this + 0x58));
  *(undefined4 *)((int)pvVar1 + 0xc) = 0;
  FUN_10003920(pvVar1,0,iVar5,0);
  iVar5 = *(int *)(param_2 + 0x9c);
  pvVar1 = (void *)(iVar9 + 0x18 + *(int *)((int)this + 0x58));
  *(undefined4 *)((int)pvVar1 + 0xc) = 0;
  FUN_10003920(pvVar1,0,iVar5,0);
  iVar5 = 0;
  if (param_1 == 0) {
    if (0 < *(int *)(param_2 + 0x9c)) {
      do {
        *(int *)(*(int *)(*(int *)((int)this + 0x58) + 4) + iVar5 * 4) = iVar5;
        *(int *)(*(int *)(*(int *)((int)this + 0x58) + 0x18) + iVar5 * 4) = iVar5;
        iVar5 = iVar5 + 1;
      } while (iVar5 < *(int *)(param_2 + 0x9c));
    }
  }
  else {
    if (0 < *(int *)(iVar4 + 0x9c)) {
      do {
        *(undefined4 *)(*(int *)(iVar9 + 4 + *(int *)((int)this + 0x58)) + iVar5 * 4) = 0xffffffff;
        iVar5 = iVar5 + 1;
      } while (iVar5 < *(int *)(iVar4 + 0x9c));
    }
    param_1 = 0;
    if (0 < *(int *)(param_2 + 0x9c)) {
      param_2 = 0;
      do {
        iVar5 = 0;
        if (0 < *(int *)(iVar4 + 0x9c)) {
          local_8 = 0;
          do {
            piVar6 = (int *)(local_8 + iVar4 + *(int *)(iVar4 + 0xa0));
            iVar7 = *(int *)(iVar3 + 0xa0) + param_2;
            uVar8 = FUN_10016260((byte *)(*(int *)(iVar7 + iVar3) + iVar7 + iVar3),
                                 (byte *)(*piVar6 + (int)piVar6));
            if (uVar8 == 0) break;
            local_8 = local_8 + 0xd8;
            iVar5 = iVar5 + 1;
          } while (iVar5 < *(int *)(iVar4 + 0x9c));
        }
        if (iVar5 < *(int *)(iVar4 + 0x9c)) {
          *(int *)(*(int *)(iVar9 + 0x18 + *(int *)((int)this + 0x58)) + param_1 * 4) = iVar5;
          *(int *)(*(int *)(iVar9 + 4 + *(int *)((int)this + 0x58)) + iVar5 * 4) = param_1;
          piVar6 = (int *)(*(int *)(iVar3 + 0xa0) + param_2 + iVar3);
          iVar7 = piVar6[1];
          if (iVar7 == -1) {
LAB_100104e9:
            bVar10 = *(int *)(iVar5 * 0xd8 + *(int *)(iVar4 + 0xa0) + 4 + iVar4) == -1;
LAB_100104fa:
            if (bVar10) goto LAB_1001052b;
          }
          else {
            iVar2 = *(int *)(iVar5 * 0xd8 + *(int *)(iVar4 + 0xa0) + 4 + iVar4);
            if (iVar2 != -1) {
              bVar10 = *(int *)(*(int *)(iVar9 + 0x18 + *(int *)((int)this + 0x58)) + iVar7 * 4) ==
                       *(int *)(*(int *)(*(int *)((int)this + 0x58) + 0x18) + iVar2 * 4);
              goto LAB_100104fa;
            }
            if (iVar7 == -1) goto LAB_100104e9;
          }
          Warning("%s/%s : missmatched parent bones on \"%s\"\n",iVar4 + 0xc,iVar3 + 0xc,
                  *piVar6 + (int)piVar6);
        }
        else {
          *(undefined4 *)(*(int *)(iVar9 + 0x18 + *(int *)((int)this + 0x58)) + param_1 * 4) =
               0xffffffff;
        }
LAB_1001052b:
        param_2 = param_2 + 0xd8;
        param_1 = param_1 + 1;
      } while (param_1 < *(int *)(iVar3 + 0x9c));
    }
  }
  piVar6 = (int *)((int)this + 4);
  *piVar6 = *piVar6 + -1;
  if (*piVar6 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return *(DWORD *)((int)this + 4);
}


// 10010570 FUN_10010570

int * __thiscall FUN_10010570(void *this,int *param_1)

{
  int iVar1;
  undefined4 *puVar2;
  int iVar3;
  int iVar4;
  
  iVar4 = param_1[3];
  *(undefined4 *)((int)this + 0xc) = 0;
  FUN_1000f540(this,0,iVar4,(undefined4 *)0x0);
  if (0 < iVar4) {
    iVar3 = 0;
    do {
                    /* WARNING: Load size is inaccurate */
      iVar1 = *param_1 + iVar3;
      puVar2 = (undefined4 *)(*this + iVar3);
      *puVar2 = *(undefined4 *)(*param_1 + iVar3);
      puVar2[1] = *(undefined4 *)(iVar1 + 4);
      puVar2[2] = *(undefined4 *)(iVar1 + 8);
      iVar3 = iVar3 + 0x10;
      iVar4 = iVar4 + -1;
      puVar2[3] = *(undefined4 *)(iVar1 + 0xc);
    } while (iVar4 != 0);
  }
  return (int *)this;
}


// 100105d0 FUN_100105d0

int * __thiscall FUN_100105d0(void *this,int *param_1)

{
  int iVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  
  iVar1 = param_1[3];
  *(undefined4 *)((int)this + 0xc) = 0;
  FUN_1000f5f0(this,0,iVar1,(undefined4 *)0x0);
  iVar4 = 0;
  if (0 < iVar1) {
    do {
      iVar2 = *param_1;
                    /* WARNING: Load size is inaccurate */
      iVar3 = *this;
      *(undefined4 *)(iVar3 + iVar4 * 8) = *(undefined4 *)(iVar2 + iVar4 * 8);
      *(undefined4 *)(iVar3 + 4 + iVar4 * 8) = *(undefined4 *)(iVar2 + 4 + iVar4 * 8);
      iVar4 = iVar4 + 1;
    } while (iVar4 < iVar1);
  }
  return (int *)this;
}


// 10010620 FUN_10010620

void __thiscall FUN_10010620(void *this,int param_1,int param_2)

{
  int *piVar1;
  void *pvVar2;
  int iVar3;
  int iVar4;
  uint uVar5;
  uint uVar6;
  int iVar7;
  uint *puVar8;
  int *piVar9;
  uint extraout_EDX;
  uint extraout_EDX_00;
  int iVar10;
  int iVar11;
  longlong lVar12;
  uint local_28;
  uint local_24;
  int local_18;
  uint local_14;
  int local_10;
  int local_c;
  int local_8;
  uint local_4;
  
  FUN_1000ef50((DWORD *)this);
  iVar3 = *(int *)((int)this + 0x3c);
  local_14 = 0;
  local_10 = 0;
  local_c = 0;
  local_8 = 0;
  local_4 = 0;
  FUN_100105d0(&local_14,(int *)((int)this + 0x30));
  iVar7 = *(int *)(param_2 + 0xf0);
  iVar11 = param_1 * 0x90;
  pvVar2 = (void *)(*(int *)((int)this + 0x58) + 0x54 + iVar11);
  *(undefined4 *)((int)pvVar2 + 0xc) = 0;
  FUN_10003920(pvVar2,0,iVar7,0);
  local_24 = 0;
  if (0 < *(int *)(param_2 + 0xf0)) {
    local_18 = 0;
    do {
      piVar1 = (int *)(local_18 + param_2 + *(int *)(param_2 + 0xf4));
      local_28 = *(uint *)(*(int *)(*(int *)((int)this + 0x58) + 0x18 + iVar11) +
                          *(int *)(local_18 + param_2 + 8 + *(int *)(param_2 + 0xf4)) * 4);
      if (local_28 != 0xffffffff) {
        iVar7 = *piVar1;
        iVar10 = 0;
        if (0 < iVar3) {
          do {
            iVar4 = *(int *)(local_14 + 4 + iVar10 * 8);
            lVar12 = FUN_1000de40((ushort *)
                                  (*(int *)(local_14 + iVar10 * 8) * 0x90 +
                                  *(int *)((int)this + 0x58)),local_14);
            piVar9 = (int *)(*(int *)((int)lVar12 + 0xf4) + iVar4 * 0x5c + (int)lVar12);
            uVar6 = FUN_10016260((byte *)(iVar7 + (int)piVar1),(byte *)(*piVar9 + (int)piVar9));
            if (uVar6 == 0) break;
            iVar10 = iVar10 + 1;
          } while (iVar10 < iVar3);
        }
        iVar7 = local_8;
        if (iVar10 == iVar3) {
          uVar6 = local_8 + 1;
          if (local_10 < (int)uVar6) {
            FUN_1000f030(&local_14,(local_8 - local_10) + 1);
            uVar6 = extraout_EDX;
          }
          uVar5 = local_14;
          local_8 = local_8 + 1;
          iVar10 = (local_8 - iVar7) + -1;
          local_4 = local_14;
          if (0 < iVar10) {
            pvVar2 = (void *)(local_14 + iVar7 * 8);
            FUN_10016630((void *)((int)pvVar2 + 8),pvVar2,iVar10 * 8);
            uVar6 = extraout_EDX_00;
          }
          piVar1 = (int *)(uVar5 + iVar7 * 8);
          if (piVar1 != (int *)0x0) {
            *piVar1 = param_1;
            piVar1[1] = local_24;
            uVar6 = local_24;
          }
          lVar12 = FUN_1000de40(*(ushort **)((int)this + 0x58),uVar6);
          uVar6 = (uint)((ulonglong)lVar12 >> 0x20);
          iVar10 = iVar7;
          if ((*(uint *)(local_28 * 0xd8 + *(int *)((int)lVar12 + 0xa0) + 0xa0 + (int)lVar12) &
              0x200) == 0) {
            do {
              lVar12 = FUN_1000de40(*(ushort **)((int)this + 0x58),uVar6);
              uVar6 = *(int *)((int)lVar12 + 0xa0) + local_28 * 0xd8;
              puVar8 = (uint *)(uVar6 + 0xa0 + (int)lVar12);
              *puVar8 = *puVar8 | 0x200;
              lVar12 = FUN_1000de40(*(ushort **)((int)this + 0x58),uVar6);
              uVar6 = (uint)((ulonglong)lVar12 >> 0x20);
              iVar7 = *(int *)((int)lVar12 + 400);
              if (iVar7 != 0) {
                iVar7 = (int)lVar12 + iVar7;
                iVar10 = *(int *)(iVar7 + 0x10);
                if ((iVar10 != 0) && (iVar7 + iVar10 != 0)) {
                  lVar12 = FUN_1000de40(*(ushort **)((int)this + 0x58),uVar6);
                  iVar7 = *(int *)((int)lVar12 + 400);
                  if (iVar7 == 0) {
LAB_1001093a:
                    iVar7 = 0;
                  }
                  else {
                    iVar7 = (int)lVar12 + iVar7;
                    iVar10 = *(int *)(iVar7 + 0x10);
                    if (iVar10 == 0) goto LAB_1001093a;
                    iVar7 = iVar7 + iVar10;
                  }
                  puVar8 = (uint *)(iVar7 + *(int *)(iVar7 + 4) + local_28 * 4);
                  *puVar8 = *puVar8 | 0x200;
                  uVar6 = local_28;
                }
              }
              lVar12 = FUN_1000de40(*(ushort **)((int)this + 0x58),uVar6);
              uVar6 = *(int *)((int)lVar12 + 0xa0) + local_28 * 0xd8;
              local_28 = *(uint *)(uVar6 + 4 + (int)lVar12);
            } while (local_28 != 0xffffffff);
            goto LAB_100107c9;
          }
        }
        *(int *)(*(int *)(iVar11 + 0x54 + *(int *)((int)this + 0x58)) + local_24 * 4) = iVar10;
      }
LAB_100107c9:
      local_24 = local_24 + 1;
      local_18 = local_18 + 0x5c;
    } while ((int)local_24 < *(int *)(param_2 + 0xf0));
  }
  iVar3 = local_8;
  piVar1 = (int *)((int)this + 0x30);
  *(undefined4 *)((int)this + 0x3c) = 0;
  if (local_8 != 0) {
    if (*(int *)((int)this + 0x34) < local_8) {
      FUN_1000f030(piVar1,local_8 - *(int *)((int)this + 0x34));
    }
    *(int *)((int)this + 0x3c) = *(int *)((int)this + 0x3c) + iVar3;
    pvVar2 = (void *)*piVar1;
    iVar7 = *(int *)((int)this + 0x3c) - iVar3;
    *(void **)((int)this + 0x40) = pvVar2;
    if ((0 < iVar7) && (0 < iVar3)) {
      FUN_10016630((void *)((int)pvVar2 + iVar3 * 8),pvVar2,iVar7 * 8);
    }
  }
  iVar7 = local_c;
  iVar11 = 0;
  if (0 < iVar3) {
    do {
      iVar10 = *piVar1;
      *(undefined4 *)(iVar10 + iVar11 * 8) = *(undefined4 *)(local_14 + iVar11 * 8);
      *(undefined4 *)(iVar10 + 4 + iVar11 * 8) = *(undefined4 *)(local_14 + 4 + iVar11 * 8);
      iVar11 = iVar11 + 1;
    } while (iVar11 < iVar3);
  }
  local_8 = 0;
  if (-1 < local_c) {
    if (local_14 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_14);
      local_14 = 0;
    }
    local_10 = 0;
  }
  if ((-1 < iVar7) && (local_14 != 0)) {
    local_4 = local_14;
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_14);
  }
  piVar1 = (int *)((int)this + 4);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return;
}


// 10010980 FUN_10010980

void __thiscall FUN_10010980(void *this,int param_1,int param_2)

{
  void *pvVar1;
  float fVar2;
  undefined4 uVar3;
  undefined4 uVar4;
  int iVar5;
  uint uVar6;
  int *piVar7;
  uint uVar8;
  int iVar9;
  int iVar10;
  int *piVar11;
  int iVar12;
  uint extraout_EDX;
  uint extraout_EDX_00;
  int iVar13;
  int iVar14;
  int iVar15;
  longlong lVar16;
  int local_20;
  int local_1c;
  int local_14;
  int local_10;
  int local_c;
  int local_8;
  int local_4;
  
  FUN_1000ef50((DWORD *)this);
  iVar5 = *(int *)((int)this + 0x50);
  local_14 = 0;
  local_10 = 0;
  local_c = 0;
  local_8 = 0;
  local_4 = 0;
  FUN_100105d0(&local_14,(int *)((int)this + 0x44));
  iVar13 = *(int *)(param_2 + 300);
  uVar6 = param_1 * 0x90;
  pvVar1 = (void *)(uVar6 + 0x68 + *(int *)((int)this + 0x58));
  *(undefined4 *)((int)pvVar1 + 0xc) = 0;
  FUN_10003920(pvVar1,0,iVar13,0);
  local_20 = 0;
  iVar13 = local_14;
  if (0 < *(int *)(param_2 + 300)) {
    local_1c = 0;
    uVar8 = extraout_EDX;
    do {
      piVar7 = (int *)(param_2 + local_1c + *(int *)(param_2 + 0x130));
      iVar14 = 0;
      iVar10 = *piVar7;
      if (0 < iVar5) {
        do {
          iVar9 = *(int *)(iVar13 + 4 + iVar14 * 8);
          lVar16 = FUN_1000de40((ushort *)
                                (*(int *)(iVar13 + iVar14 * 8) * 0x90 + *(int *)((int)this + 0x58)),
                                uVar8);
          piVar11 = (int *)(*(int *)((int)lVar16 + 0x130) + iVar9 * 0x14 + (int)lVar16);
          uVar8 = FUN_10016260((byte *)(iVar10 + (int)piVar7),(byte *)(*piVar11 + (int)piVar11));
          if (uVar8 == 0) break;
          iVar14 = iVar14 + 1;
          uVar8 = extraout_EDX_00;
        } while (iVar14 < iVar5);
      }
      iVar10 = local_8;
      if (iVar14 == iVar5) {
        if (local_10 < local_8 + 1) {
          FUN_1000f030(&local_14,(local_8 - local_10) + 1);
          iVar13 = local_14;
        }
        local_8 = local_8 + 1;
        iVar14 = (local_8 - iVar10) + -1;
        local_4 = iVar13;
        if (0 < iVar14) {
          pvVar1 = (void *)(iVar13 + iVar10 * 8);
          FUN_10016630((void *)((int)pvVar1 + 8),pvVar1,iVar14 * 8);
        }
        piVar7 = (int *)(iVar13 + iVar10 * 8);
        iVar14 = iVar10;
        if (piVar7 != (int *)0x0) {
          *piVar7 = param_1;
          piVar7[1] = local_20;
        }
      }
      else {
        iVar10 = *(int *)(iVar13 + 4 + iVar14 * 8);
        iVar15 = param_2 + local_1c + *(int *)(param_2 + 0x130);
        lVar16 = FUN_1000de40((ushort *)
                              (*(int *)(iVar13 + iVar14 * 8) * 0x90 + *(int *)((int)this + 0x58)),
                              (uint)this);
        iVar9 = (int)lVar16;
        iVar10 = *(int *)(iVar9 + 0x130) + iVar10 * 0x14;
        iVar12 = iVar10 + iVar9;
        if (*(float *)(iVar15 + 8) <= *(float *)(iVar10 + 8 + iVar9)) {
          fVar2 = *(float *)(iVar15 + 8);
        }
        else {
          fVar2 = *(float *)(iVar12 + 8);
        }
        if (fVar2 <= *(float *)(iVar15 + 0xc)) {
          if (*(float *)(iVar15 + 8) <= *(float *)(iVar12 + 8)) {
            fVar2 = *(float *)(iVar15 + 8);
          }
          else {
            fVar2 = *(float *)(iVar12 + 8);
          }
        }
        else {
          fVar2 = *(float *)(iVar15 + 0xc);
        }
        if (fVar2 <= *(float *)(iVar12 + 0xc)) {
          if (*(float *)(iVar15 + 8) <= *(float *)(iVar12 + 8)) {
            fVar2 = *(float *)(iVar15 + 8);
          }
          else {
            fVar2 = *(float *)(iVar12 + 8);
          }
          if (fVar2 <= *(float *)(iVar15 + 0xc)) {
            if (*(float *)(iVar15 + 8) <= *(float *)(iVar12 + 8)) {
              uVar3 = *(undefined4 *)(iVar15 + 8);
            }
            else {
              uVar3 = *(undefined4 *)(iVar12 + 8);
            }
          }
          else {
            uVar3 = *(undefined4 *)(iVar15 + 0xc);
          }
        }
        else {
          uVar3 = *(undefined4 *)(iVar12 + 0xc);
        }
        if (*(float *)(iVar12 + 8) <= *(float *)(iVar15 + 8)) {
          fVar2 = *(float *)(iVar15 + 8);
        }
        else {
          fVar2 = *(float *)(iVar12 + 8);
        }
        if (*(float *)(iVar15 + 0xc) <= fVar2) {
          if (*(float *)(iVar12 + 8) <= *(float *)(iVar15 + 8)) {
            fVar2 = *(float *)(iVar15 + 8);
          }
          else {
            fVar2 = *(float *)(iVar12 + 8);
          }
        }
        else {
          fVar2 = *(float *)(iVar15 + 0xc);
        }
        if (*(float *)(iVar12 + 0xc) <= fVar2) {
          if (*(float *)(iVar12 + 8) <= *(float *)(iVar15 + 8)) {
            fVar2 = *(float *)(iVar15 + 8);
          }
          else {
            fVar2 = *(float *)(iVar12 + 8);
          }
          if (*(float *)(iVar15 + 0xc) <= fVar2) {
            if (*(float *)(iVar12 + 8) <= *(float *)(iVar15 + 8)) {
              uVar4 = *(undefined4 *)(iVar15 + 8);
            }
            else {
              uVar4 = *(undefined4 *)(iVar12 + 8);
            }
          }
          else {
            uVar4 = *(undefined4 *)(iVar15 + 0xc);
          }
        }
        else {
          uVar4 = *(undefined4 *)(iVar12 + 0xc);
        }
        *(undefined4 *)(iVar12 + 8) = uVar3;
        *(undefined4 *)(iVar12 + 0xc) = uVar4;
      }
      *(int *)(*(int *)(uVar6 + 0x68 + *(int *)((int)this + 0x58)) + local_20 * 4) = iVar14;
      local_20 = local_20 + 1;
      local_1c = local_1c + 0x14;
      uVar8 = uVar6;
    } while (local_20 < *(int *)(param_2 + 300));
  }
  iVar5 = local_8;
  piVar7 = (int *)((int)this + 0x44);
  *(undefined4 *)((int)this + 0x50) = 0;
  if (local_8 != 0) {
    if (*(int *)((int)this + 0x48) < local_8) {
      FUN_1000f030(piVar7,local_8 - *(int *)((int)this + 0x48));
    }
    *(int *)((int)this + 0x50) = *(int *)((int)this + 0x50) + iVar5;
    pvVar1 = (void *)*piVar7;
    iVar10 = *(int *)((int)this + 0x50) - iVar5;
    *(void **)((int)this + 0x54) = pvVar1;
    if ((0 < iVar10) && (0 < iVar5)) {
      FUN_10016630((void *)((int)pvVar1 + iVar5 * 8),pvVar1,iVar10 * 8);
    }
  }
  iVar10 = local_c;
  iVar14 = 0;
  if (0 < iVar5) {
    do {
      iVar9 = *piVar7;
      *(undefined4 *)(iVar9 + iVar14 * 8) = *(undefined4 *)(iVar13 + iVar14 * 8);
      *(undefined4 *)(iVar9 + 4 + iVar14 * 8) = *(undefined4 *)(iVar13 + 4 + iVar14 * 8);
      iVar14 = iVar14 + 1;
    } while (iVar14 < iVar5);
  }
  local_8 = 0;
  if (-1 < local_c) {
    if (iVar13 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar13);
      iVar13 = 0;
      local_14 = 0;
    }
    local_10 = 0;
  }
  if ((-1 < iVar10) && (iVar13 != 0)) {
    local_4 = iVar13;
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar13);
  }
  piVar7 = (int *)((int)this + 4);
  *piVar7 = *piVar7 + -1;
  if (*piVar7 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return;
}


// 10010d30 FUN_10010d30

void __thiscall FUN_10010d30(void *this,int param_1,int param_2)

{
  int *piVar1;
  void *pvVar2;
  int iVar3;
  int iVar4;
  int iVar5;
  uint uVar6;
  int iVar7;
  uint extraout_EDX;
  int iVar8;
  int iVar9;
  longlong lVar10;
  int local_24;
  int local_14;
  int local_10;
  int local_c;
  int local_8;
  int local_4;
  
  FUN_1000ef50((DWORD *)this);
  iVar3 = *(int *)((int)this + 0x78);
  local_14 = 0;
  local_10 = 0;
  local_c = 0;
  local_8 = 0;
  local_4 = 0;
  FUN_100105d0(&local_14,(int *)((int)this + 0x6c));
  iVar9 = *(int *)(param_2 + 0xf8);
  pvVar2 = (void *)(param_1 * 0x90 + 0x7c + *(int *)((int)this + 0x58));
  *(undefined4 *)((int)pvVar2 + 0xc) = 0;
  FUN_10003920(pvVar2,0,iVar9,0);
  local_24 = 0;
  iVar9 = local_14;
  if (0 < *(int *)(param_2 + 0xf8)) {
    do {
      uVar6 = param_2 + local_24 * 4;
      iVar7 = *(int *)(uVar6 + *(int *)(param_2 + 0x100));
      iVar8 = 0;
      if (0 < iVar3) {
        do {
          iVar4 = *(int *)(iVar9 + 4 + iVar8 * 8);
          lVar10 = FUN_1000de40((ushort *)
                                (*(int *)(iVar9 + iVar8 * 8) * 0x90 + *(int *)((int)this + 0x58)),
                                uVar6);
          iVar5 = (int)lVar10;
          uVar6 = FUN_10016260((byte *)(iVar7 + param_2),
                               (byte *)(*(int *)(*(int *)(iVar5 + 0x100) + iVar4 * 4 + iVar5) +
                                       iVar5));
          if (uVar6 == 0) break;
          iVar8 = iVar8 + 1;
          uVar6 = extraout_EDX;
        } while (iVar8 < iVar3);
      }
      iVar7 = local_8;
      if (iVar8 == iVar3) {
        if (local_10 < local_8 + 1) {
          FUN_1000f030(&local_14,(local_8 - local_10) + 1);
          iVar9 = local_14;
        }
        local_8 = local_8 + 1;
        iVar8 = (local_8 - iVar7) + -1;
        local_4 = iVar9;
        if (0 < iVar8) {
          pvVar2 = (void *)(iVar9 + iVar7 * 8);
          FUN_10016630((void *)((int)pvVar2 + 8),pvVar2,iVar8 * 8);
        }
        piVar1 = (int *)(iVar9 + iVar7 * 8);
        iVar8 = iVar7;
        if (piVar1 != (int *)0x0) {
          *piVar1 = param_1;
          piVar1[1] = local_24;
        }
      }
      *(int *)(*(int *)(param_1 * 0x90 + 0x7c + *(int *)((int)this + 0x58)) + local_24 * 4) = iVar8;
      local_24 = local_24 + 1;
    } while (local_24 < *(int *)(param_2 + 0xf8));
  }
  iVar3 = local_8;
  piVar1 = (int *)((int)this + 0x6c);
  *(undefined4 *)((int)this + 0x78) = 0;
  if (local_8 != 0) {
    if (*(int *)((int)this + 0x70) < local_8) {
      FUN_1000f030(piVar1,local_8 - *(int *)((int)this + 0x70));
    }
    *(int *)((int)this + 0x78) = *(int *)((int)this + 0x78) + iVar3;
    pvVar2 = (void *)*piVar1;
    iVar7 = *(int *)((int)this + 0x78) - iVar3;
    *(void **)((int)this + 0x7c) = pvVar2;
    if ((0 < iVar7) && (0 < iVar3)) {
      FUN_10016630((void *)((int)pvVar2 + iVar3 * 8),pvVar2,iVar7 * 8);
    }
  }
  iVar7 = local_c;
  iVar8 = 0;
  if (0 < iVar3) {
    do {
      iVar4 = *piVar1;
      *(undefined4 *)(iVar4 + iVar8 * 8) = *(undefined4 *)(iVar9 + iVar8 * 8);
      *(undefined4 *)(iVar4 + 4 + iVar8 * 8) = *(undefined4 *)(iVar9 + 4 + iVar8 * 8);
      iVar8 = iVar8 + 1;
    } while (iVar8 < iVar3);
  }
  local_8 = 0;
  if (-1 < local_c) {
    if (iVar9 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar9);
      iVar9 = 0;
      local_14 = 0;
    }
    local_10 = 0;
  }
  if ((-1 < iVar7) && (iVar9 != 0)) {
    local_4 = iVar9;
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar9);
  }
  piVar1 = (int *)((int)this + 4);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return;
}


// 10010f90 FUN_10010f90

void __thiscall FUN_10010f90(void *this,undefined4 param_1,int param_2)

{
  void *pvVar1;
  undefined4 *puVar2;
  uint uVar3;
  int iVar4;
  int iVar5;
  uint uVar6;
  int iVar7;
  int iVar8;
  int iVar9;
  int *piVar10;
  longlong lVar11;
  int local_24;
  int local_20;
  int local_14;
  int local_10;
  int local_c;
  int local_8;
  int local_4;
  
  FUN_1000ef50((DWORD *)this);
  iVar4 = *(int *)((int)this + 0x8c);
  local_14 = 0;
  local_10 = 0;
  local_c = 0;
  local_8 = 0;
  local_4 = 0;
  FUN_100105d0(&local_14,(int *)((int)this + 0x80));
  local_24 = 0;
  iVar7 = local_14;
  iVar8 = local_8;
  if (0 < *(int *)(param_2 + 0x140)) {
    local_20 = 0;
    iVar5 = local_8;
    do {
      iVar9 = 0;
      uVar3 = *(uint *)(*(int *)(param_2 + 0x144) + local_20 + param_2);
      uVar6 = uVar3;
      if (0 < iVar4) {
        do {
          iVar8 = *(int *)(iVar7 + 4 + iVar9 * 8);
          lVar11 = FUN_1000de40((ushort *)
                                (*(int *)(iVar7 + iVar9 * 8) * 0x90 + *(int *)((int)this + 0x58)),
                                uVar6);
          if (uVar3 == *(uint *)(iVar8 * 0x20 + *(int *)((int)lVar11 + 0x144) + (int)lVar11)) break;
          iVar9 = iVar9 + 1;
          uVar6 = (uint)((ulonglong)lVar11 >> 0x20);
        } while (iVar9 < iVar4);
      }
      iVar8 = iVar5;
      if (iVar9 == iVar4) {
        if (local_10 < iVar5 + 1) {
          FUN_1000f030(&local_14,(iVar5 - local_10) + 1);
          iVar7 = local_14;
          iVar8 = local_8;
        }
        iVar8 = iVar8 + 1;
        iVar9 = (iVar8 - iVar5) + -1;
        local_8 = iVar8;
        local_4 = iVar7;
        if (0 < iVar9) {
          pvVar1 = (void *)(iVar7 + iVar5 * 8);
          FUN_10016630((void *)((int)pvVar1 + 8),pvVar1,iVar9 * 8);
        }
        puVar2 = (undefined4 *)(iVar7 + iVar5 * 8);
        if (puVar2 != (undefined4 *)0x0) {
          *puVar2 = param_1;
          puVar2[1] = local_24;
        }
      }
      local_20 = local_20 + 0x20;
      local_24 = local_24 + 1;
      iVar5 = iVar8;
    } while (local_24 < *(int *)(param_2 + 0x140));
  }
  piVar10 = (int *)((int)this + 0x80);
  *(undefined4 *)((int)this + 0x8c) = 0;
  if (iVar8 != 0) {
    if (*(int *)((int)this + 0x84) < iVar8) {
      FUN_1000f030(piVar10,iVar8 - *(int *)((int)this + 0x84));
    }
    *(int *)((int)this + 0x8c) = *(int *)((int)this + 0x8c) + iVar8;
    pvVar1 = (void *)*piVar10;
    iVar4 = *(int *)((int)this + 0x8c) - iVar8;
    *(void **)((int)this + 0x90) = pvVar1;
    if ((0 < iVar4) && (0 < iVar8)) {
      FUN_10016630((void *)((int)pvVar1 + iVar8 * 8),pvVar1,iVar4 * 8);
    }
  }
  iVar4 = local_c;
  iVar5 = 0;
  if (0 < iVar8) {
    do {
      iVar9 = *piVar10;
      *(undefined4 *)(iVar9 + iVar5 * 8) = *(undefined4 *)(iVar7 + iVar5 * 8);
      *(undefined4 *)(iVar9 + 4 + iVar5 * 8) = *(undefined4 *)(iVar7 + 4 + iVar5 * 8);
      iVar5 = iVar5 + 1;
    } while (iVar5 < iVar8);
  }
  local_8 = 0;
  if (-1 < local_c) {
    if (iVar7 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar7);
      iVar7 = 0;
      local_14 = 0;
    }
    local_10 = 0;
  }
  if ((-1 < iVar4) && (iVar7 != 0)) {
    local_4 = iVar7;
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar7);
  }
  piVar10 = (int *)((int)this + 4);
  *piVar10 = *piVar10 + -1;
  if (*piVar10 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return;
}


// 100111b0 FUN_100111b0

void __fastcall FUN_100111b0(int *param_1)

{
  *param_1 = 0;
  param_1[1] = 0;
  param_1[2] = 0;
  param_1[3] = 0;
  *(undefined2 *)(param_1 + 4) = 0xffff;
  *(undefined2 *)((int)param_1 + 0x12) = 0;
  *(undefined2 *)(param_1 + 5) = 0xffff;
  *(undefined2 *)((int)param_1 + 0x16) = 0xffff;
  param_1[6] = param_1[1];
  if (*param_1 == 0) {
    *param_1 = (int)&LAB_10004d30;
  }
  param_1[7] = 0;
  param_1[8] = 0;
  param_1[9] = 0;
  param_1[10] = 0;
  *(undefined2 *)(param_1 + 0xb) = 0xffff;
  *(undefined2 *)(param_1 + 0xc) = 0xffff;
  *(undefined2 *)((int)param_1 + 0x2e) = 0;
  *(undefined2 *)((int)param_1 + 0x32) = 0xffff;
  param_1[0xd] = param_1[8];
  if (param_1[7] == 0) {
    param_1[7] = (int)&LAB_10004d30;
  }
  return;
}


// 10011220 FUN_10011220

void __thiscall FUN_10011220(void *this,int param_1,uint param_2)

{
  undefined4 *puVar1;
  void *pvVar2;
  uint *puVar3;
  char cVar4;
  undefined4 uVar5;
  short sVar6;
  int iVar7;
  uint uVar8;
  uint uVar9;
  char *pcVar10;
  undefined2 *puVar11;
  uint extraout_EDX;
  uint extraout_EDX_00;
  uint extraout_EDX_01;
  int iVar12;
  int iVar13;
  int iVar14;
  int *piVar15;
  byte *pbVar16;
  int iVar17;
  char *pcVar18;
  longlong lVar19;
  uint local_34;
  undefined2 local_30;
  byte *local_2c [2];
  undefined4 local_24;
  undefined4 local_20;
  int local_1c;
  int local_18;
  uint local_14 [5];
  
  FUN_1000ef50((DWORD *)this);
  iVar13 = *(int *)((int)this + 0x14);
  local_14[0] = 0;
  local_14[1] = 0;
  local_14[2] = 0;
  local_14[3] = 0;
  local_14[4] = 0;
  FUN_10010570(local_14,(int *)((int)this + 8));
  uVar8 = param_2;
  iVar17 = *(int *)(param_2 + 0xbc);
  local_34 = param_1 * 0x90;
  pvVar2 = (void *)(local_34 + 0x2c + *(int *)((int)this + 0x58));
  *(undefined4 *)((int)pvVar2 + 0xc) = 0;
  FUN_10003920(pvVar2,0,iVar17,0);
  iVar17 = *(int *)(uVar8 + 0xbc);
  iVar12 = 0;
  uVar9 = extraout_EDX;
  if (0 < iVar17) {
    do {
      iVar14 = 0;
      if ((iVar12 < 0) || (iVar7 = iVar12, iVar17 <= iVar12)) {
        iVar7 = 0;
      }
      uVar8 = iVar7 * 0xd4 + *(int *)(uVar8 + 0xc0) + uVar8;
      pbVar16 = (byte *)(*(int *)(uVar8 + 4) + uVar8);
      if (DAT_1002c728 < 0) {
        if (0 < iVar13) {
          piVar15 = (int *)(local_14[0] + 0xc);
          uVar9 = uVar8;
          do {
            lVar19 = FUN_1000de40((ushort *)(piVar15[-1] * 0x90 + *(int *)((int)this + 0x58)),uVar9)
            ;
            iVar7 = (int)lVar19;
            iVar17 = *piVar15;
            if ((iVar17 < 0) || (*(int *)(iVar7 + 0xbc) <= iVar17)) {
              iVar17 = 0;
            }
            iVar7 = iVar17 * 0xd4 + *(int *)(iVar7 + 0xc0) + iVar7;
            uVar9 = FUN_10016260(pbVar16,(byte *)(*(int *)(iVar7 + 4) + iVar7));
            if (uVar9 == 0) break;
            iVar14 = iVar14 + 1;
            piVar15 = piVar15 + 4;
            uVar9 = extraout_EDX_00;
          } while (iVar14 < iVar13);
        }
LAB_1001138a:
        if (iVar14 == iVar13) goto LAB_10011451;
        iVar7 = iVar14 * 0x10;
        iVar17 = *(int *)(iVar7 + 0xc + local_14[0]);
        puVar1 = (undefined4 *)(iVar7 + local_14[0]);
        lVar19 = FUN_1000de40((ushort *)
                              (*(int *)(iVar7 + 8 + local_14[0]) * 0x90 + *(int *)((int)this + 0x58)
                              ),local_14[0]);
        iVar7 = (int)lVar19;
        if ((iVar17 < 0) || (*(int *)(iVar7 + 0xbc) <= iVar17)) {
          iVar17 = 0;
        }
        if ((*(uint *)(iVar17 * 0xd4 + *(int *)(iVar7 + 0xc0) + 0xc + iVar7) & 0x800) != 0) {
          uVar5 = *(undefined4 *)(uVar8 + 0x10);
          *puVar1 = *(undefined4 *)(uVar8 + 0xc);
          puVar1[1] = uVar5;
          puVar1[2] = param_1;
          puVar1[3] = iVar12;
        }
      }
      else {
        if ((pbVar16 != (byte *)0x0) &&
           (local_2c[0] = pbVar16,
           sVar6 = FUN_1000fb50((void *)(DAT_10037b00 + DAT_1002c728 * 0x38),local_2c), sVar6 != -1)
           ) {
          iVar14 = (int)*(short *)(*(int *)(DAT_10037b00 + 4 + DAT_1002c728 * 0x38) + 0xc +
                                  sVar6 * 0x10);
          goto LAB_1001138a;
        }
LAB_10011451:
        local_24 = *(undefined4 *)(uVar8 + 0xc);
        local_1c = param_1;
        local_20 = *(undefined4 *)(uVar8 + 0x10);
        local_18 = iVar12;
        iVar14 = FUN_1000fad0(local_14,local_14[3],&local_24);
      }
      *(int *)(*(int *)(local_34 + 0x2c + *(int *)((int)this + 0x58)) + iVar12 * 4) = iVar14;
      iVar17 = *(int *)(param_2 + 0xbc);
      iVar12 = iVar12 + 1;
      uVar9 = param_2;
      uVar8 = param_2;
    } while (iVar12 < iVar17);
  }
  if ((-1 < DAT_1002c728) && (iVar13 < (int)local_14[3])) {
    piVar15 = (int *)(iVar13 * 0x10 + 0xc + local_14[0]);
    do {
      lVar19 = FUN_1000de40((ushort *)(piVar15[-1] * 0x90 + *(int *)((int)this + 0x58)),uVar9);
      iVar12 = (int)lVar19;
      iVar17 = *piVar15;
      if ((iVar17 < 0) || (*(int *)(iVar12 + 0xbc) <= iVar17)) {
        iVar17 = 0;
      }
      iVar17 = iVar17 * 0xd4 + *(int *)(iVar12 + 0xc0);
      pcVar18 = (char *)(*(int *)(iVar17 + 4 + iVar12) + iVar17 + iVar12);
      pvVar2 = (void *)(DAT_10037b00 + DAT_1002c728 * 0x38);
      if (pcVar18 == (char *)0x0) {
LAB_10011516:
        local_34 = 0;
      }
      else {
        pcVar10 = pcVar18;
        do {
          cVar4 = *pcVar10;
          pcVar10 = pcVar10 + 1;
        } while (cVar4 != '\0');
        local_34 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))
                             (pcVar10 + (1 - (int)(pcVar18 + 1)));
        if (local_34 == 0) goto LAB_10011516;
        iVar17 = local_34 - (int)pcVar18;
        do {
          cVar4 = *pcVar18;
          pcVar18[iVar17] = cVar4;
          pcVar18 = pcVar18 + 1;
        } while (cVar4 != '\0');
      }
      local_30 = (undefined2)iVar13;
      FUN_1000f330(pvVar2,&local_34,(short *)&param_2,(undefined1 *)&param_1);
      sVar6 = FUN_1000f430((int)pvVar2);
      puVar11 = (undefined2 *)(*(int *)((int)pvVar2 + 4) + sVar6 * 0x10);
      puVar11[2] = (short)param_2;
      puVar11[1] = 0xffff;
      *puVar11 = 0xffff;
      puVar11[3] = 0;
      if ((short)param_2 == -1) {
        *(short *)((int)pvVar2 + 0x10) = sVar6;
      }
      else if ((char)param_1 == '\0') {
        *(short *)((short)param_2 * 0x10 + 2 + *(int *)((int)pvVar2 + 4)) = sVar6;
      }
      else {
        *(short *)((short)param_2 * 0x10 + *(int *)((int)pvVar2 + 4)) = sVar6;
      }
      FUN_1000fc50(pvVar2,sVar6);
      *(short *)((int)pvVar2 + 0x12) = *(short *)((int)pvVar2 + 0x12) + 1;
      puVar3 = (uint *)(sVar6 * 0x10 + 8 + *(int *)((int)pvVar2 + 4));
      uVar9 = extraout_EDX_01;
      if (puVar3 != (uint *)0x0) {
        *puVar3 = local_34;
        *(undefined2 *)(puVar3 + 1) = local_30;
        uVar9 = local_34;
      }
      piVar15 = piVar15 + 4;
      iVar13 = iVar13 + 1;
    } while (iVar13 < (int)local_14[3]);
  }
  FUN_10010570((void *)((int)this + 8),(int *)local_14);
  uVar8 = local_14[2];
  local_14[3] = 0;
  if (-1 < (int)local_14[2]) {
    if (local_14[0] != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_14[0]);
      local_14[0] = 0;
    }
    local_14[1] = 0;
  }
  if ((-1 < (int)uVar8) && (local_14[0] != 0)) {
    local_14[4] = local_14[0];
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_14[0]);
  }
  piVar15 = (int *)((int)this + 4);
  *piVar15 = *piVar15 + -1;
  if (*piVar15 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return;
}


// 10011650 FUN_10011650

void __thiscall FUN_10011650(void *this,int param_1,int param_2)

{
  int *piVar1;
  void *pvVar2;
  uint *puVar3;
  char cVar4;
  uint uVar5;
  int iVar6;
  short sVar7;
  uint uVar8;
  int iVar9;
  uint uVar10;
  char *pcVar11;
  undefined2 *puVar12;
  int iVar13;
  uint extraout_EDX;
  uint extraout_EDX_00;
  uint extraout_EDX_01;
  int iVar14;
  byte *pbVar15;
  char *pcVar16;
  longlong lVar17;
  uint local_30;
  uint local_24;
  undefined2 local_20;
  byte *local_1c [2];
  int local_14;
  int local_10;
  int local_c;
  uint local_8;
  int local_4;
  
  FUN_1000ef50((DWORD *)this);
  uVar5 = *(uint *)((int)this + 0x28);
  local_14 = 0;
  local_10 = 0;
  local_c = 0;
  local_8 = 0;
  local_4 = 0;
  FUN_100105d0(&local_14,(int *)((int)this + 0x1c));
  iVar14 = param_2;
  iVar9 = *(int *)(param_2 + 0xb4);
  local_24 = param_1 * 0x90;
  pvVar2 = (void *)(local_24 + 0x40 + *(int *)((int)this + 0x58));
  *(undefined4 *)((int)pvVar2 + 0xc) = 0;
  FUN_10003920(pvVar2,0,iVar9,0);
  iVar9 = *(int *)(iVar14 + 0xb4);
  local_30 = 0;
  uVar10 = extraout_EDX;
  iVar14 = local_14;
  if (0 < iVar9) {
    do {
      if (((int)local_30 < 0) || (uVar8 = local_30, iVar9 <= (int)local_30)) {
        uVar8 = 0;
      }
      iVar9 = uVar8 * 100 + *(int *)(param_2 + 0xb8);
      pbVar15 = (byte *)(*(int *)(iVar9 + 4 + param_2) + iVar9 + param_2);
      if (DAT_1002c728 < 0) {
        uVar8 = 0;
        if (0 < (int)uVar5) {
          uVar8 = 0;
          do {
            iVar9 = *(int *)(iVar14 + 4 + uVar8 * 8);
            lVar17 = FUN_1000de40((ushort *)
                                  (*(int *)(iVar14 + uVar8 * 8) * 0x90 + *(int *)((int)this + 0x58))
                                  ,uVar10);
            iVar13 = (int)lVar17;
            if ((iVar9 < 0) || (*(int *)(iVar13 + 0xb4) <= iVar9)) {
              iVar9 = 0;
            }
            iVar13 = iVar9 * 100 + *(int *)(iVar13 + 0xb8) + iVar13;
            uVar10 = FUN_10016260(pbVar15,(byte *)(*(int *)(iVar13 + 4) + iVar13));
          } while ((uVar10 != 0) &&
                  (uVar8 = uVar8 + 1, uVar10 = extraout_EDX_00, (int)uVar8 < (int)uVar5));
        }
LAB_100117b2:
        if (uVar8 == uVar5) goto LAB_100117b8;
      }
      else {
        if ((pbVar15 != (byte *)0x0) &&
           (local_1c[0] = pbVar15,
           sVar7 = FUN_1000fb50((void *)(DAT_10037b00 + 0x1c + DAT_1002c728 * 0x38),local_1c),
           sVar7 != -1)) {
          uVar8 = (uint)*(short *)(*(int *)(DAT_10037b00 + 0x20 + DAT_1002c728 * 0x38) + 0xc +
                                  sVar7 * 0x10);
          goto LAB_100117b2;
        }
LAB_100117b8:
        uVar8 = local_8;
        if (local_10 < (int)(local_8 + 1)) {
          FUN_1000f030(&local_14,(local_8 - local_10) + 1);
          iVar14 = local_14;
        }
        local_8 = local_8 + 1;
        iVar9 = (local_8 - uVar8) + -1;
        local_4 = iVar14;
        if (0 < iVar9) {
          pvVar2 = (void *)(iVar14 + uVar8 * 8);
          FUN_10016630((void *)((int)pvVar2 + 8),pvVar2,iVar9 * 8);
        }
        piVar1 = (int *)(iVar14 + uVar8 * 8);
        if (piVar1 != (int *)0x0) {
          *piVar1 = param_1;
          piVar1[1] = local_30;
        }
      }
      uVar10 = *(uint *)(local_24 + 0x40 + *(int *)((int)this + 0x58));
      *(uint *)(uVar10 + local_30 * 4) = uVar8;
      iVar9 = *(int *)(param_2 + 0xb4);
      local_30 = local_30 + 1;
    } while ((int)local_30 < iVar9);
  }
  if ((-1 < DAT_1002c728) && (uVar10 = local_8, local_30 = uVar5, (int)uVar5 < (int)local_8)) {
    do {
      iVar9 = *(int *)(iVar14 + 4 + local_30 * 8);
      lVar17 = FUN_1000de40((ushort *)
                            (*(int *)(iVar14 + local_30 * 8) * 0x90 + *(int *)((int)this + 0x58)),
                            uVar10);
      iVar13 = (int)lVar17;
      if ((iVar9 < 0) || (*(int *)(iVar13 + 0xb4) <= iVar9)) {
        iVar9 = 0;
      }
      iVar9 = iVar9 * 100 + *(int *)(iVar13 + 0xb8);
      pcVar16 = (char *)(*(int *)(iVar9 + 4 + iVar13) + iVar9 + iVar13);
      pvVar2 = (void *)(DAT_10037b00 + 0x1c + DAT_1002c728 * 0x38);
      if (pcVar16 == (char *)0x0) {
LAB_10011906:
        local_24 = 0;
      }
      else {
        pcVar11 = pcVar16;
        do {
          cVar4 = *pcVar11;
          pcVar11 = pcVar11 + 1;
        } while (cVar4 != '\0');
        local_24 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))
                             (pcVar11 + (1 - (int)(pcVar16 + 1)));
        if (local_24 == 0) goto LAB_10011906;
        iVar9 = local_24 - (int)pcVar16;
        do {
          cVar4 = *pcVar16;
          pcVar16[iVar9] = cVar4;
          pcVar16 = pcVar16 + 1;
        } while (cVar4 != '\0');
      }
      local_20 = (undefined2)local_30;
      FUN_1000f330(pvVar2,&local_24,(short *)&param_1,(undefined1 *)&param_2);
      sVar7 = FUN_1000f430((int)pvVar2);
      puVar12 = (undefined2 *)(*(int *)((int)pvVar2 + 4) + sVar7 * 0x10);
      puVar12[2] = (short)param_1;
      puVar12[1] = 0xffff;
      *puVar12 = 0xffff;
      puVar12[3] = 0;
      if ((short)param_1 == -1) {
        *(short *)((int)pvVar2 + 0x10) = sVar7;
      }
      else if ((char)param_2 == '\0') {
        *(short *)((short)param_1 * 0x10 + 2 + *(int *)((int)pvVar2 + 4)) = sVar7;
      }
      else {
        *(short *)((short)param_1 * 0x10 + *(int *)((int)pvVar2 + 4)) = sVar7;
      }
      FUN_1000fc50(pvVar2,sVar7);
      *(short *)((int)pvVar2 + 0x12) = *(short *)((int)pvVar2 + 0x12) + 1;
      puVar3 = (uint *)(sVar7 * 0x10 + 8 + *(int *)((int)pvVar2 + 4));
      uVar10 = extraout_EDX_01;
      if (puVar3 != (uint *)0x0) {
        *puVar3 = local_24;
        *(undefined2 *)(puVar3 + 1) = local_20;
        uVar10 = local_24;
      }
      local_30 = local_30 + 1;
    } while ((int)local_30 < (int)local_8);
  }
  piVar1 = (int *)((int)this + 0x1c);
  *(undefined4 *)((int)this + 0x28) = 0;
  if (local_8 != 0) {
    if (*(int *)((int)this + 0x20) < (int)local_8) {
      FUN_1000f030(piVar1,local_8 - *(int *)((int)this + 0x20));
    }
    *(int *)((int)this + 0x28) = *(int *)((int)this + 0x28) + local_8;
    pvVar2 = (void *)*piVar1;
    iVar9 = *(int *)((int)this + 0x28) - local_8;
    *(void **)((int)this + 0x2c) = pvVar2;
    if ((0 < iVar9) && (0 < (int)local_8)) {
      FUN_10016630((void *)((int)pvVar2 + local_8 * 8),pvVar2,iVar9 * 8);
    }
  }
  iVar9 = local_c;
  iVar13 = 0;
  if (0 < (int)local_8) {
    do {
      iVar6 = *piVar1;
      *(undefined4 *)(iVar6 + iVar13 * 8) = *(undefined4 *)(iVar14 + iVar13 * 8);
      *(undefined4 *)(iVar6 + 4 + iVar13 * 8) = *(undefined4 *)(iVar14 + 4 + iVar13 * 8);
      iVar13 = iVar13 + 1;
    } while (iVar13 < (int)local_8);
  }
  local_8 = 0;
  if (-1 < local_c) {
    if (iVar14 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar14);
      iVar14 = 0;
      local_14 = 0;
    }
    local_10 = 0;
  }
  if ((-1 < iVar9) && (iVar14 != 0)) {
    local_4 = iVar14;
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar14);
  }
  piVar1 = (int *)((int)this + 4);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return;
}


// 10011aa0 FUN_10011aa0

int __thiscall FUN_10011aa0(void *this,int param_1)

{
  void *pvVar1;
  int *piVar2;
  int iVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_1000f0c0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar3 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar3) {
    pvVar1 = (void *)(*this + param_1 * 0x38);
    FUN_10016630((void *)((int)pvVar1 + 0x38),pvVar1,iVar3 * 0x38);
  }
                    /* WARNING: Load size is inaccurate */
  piVar2 = (int *)(*this + param_1 * 0x38);
  if (piVar2 != (int *)0x0) {
    FUN_100111b0(piVar2);
  }
  return param_1;
}


// 10011b20 FUN_10011b20

void __fastcall FUN_10011b20(int *param_1)

{
  int iVar1;
  void *pvVar2;
  int local_8;
  
  local_8 = param_1[3] + -1;
  if (-1 < local_8) {
    iVar1 = local_8 * 0x38;
    do {
      pvVar2 = (void *)(*param_1 + iVar1);
      FUN_100102f0((void *)((int)pvVar2 + 0x1c));
      FUN_10010230((void *)((int)pvVar2 + 0x1c));
      *(undefined2 *)((int)pvVar2 + 0x30) = 0xffff;
      if (-1 < *(int *)((int)pvVar2 + 0x28)) {
        if (*(int *)((int)pvVar2 + 0x20) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)pvVar2 + 0x20));
          *(undefined4 *)((int)pvVar2 + 0x20) = 0;
        }
        *(undefined4 *)((int)pvVar2 + 0x24) = 0;
      }
      *(undefined2 *)((int)pvVar2 + 0x32) = 0xffff;
      if (-1 < *(int *)((int)pvVar2 + 0x28)) {
        if (*(int *)((int)pvVar2 + 0x20) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)pvVar2 + 0x20));
          *(undefined4 *)((int)pvVar2 + 0x20) = 0;
        }
        *(undefined4 *)((int)pvVar2 + 0x24) = 0;
      }
      FUN_100102f0(pvVar2);
      FUN_10010230(pvVar2);
      *(undefined2 *)((int)pvVar2 + 0x14) = 0xffff;
      if (-1 < *(int *)((int)pvVar2 + 0xc)) {
        if (*(int *)((int)pvVar2 + 4) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)pvVar2 + 4));
          *(undefined4 *)((int)pvVar2 + 4) = 0;
        }
        *(undefined4 *)((int)pvVar2 + 8) = 0;
      }
      *(undefined2 *)((int)pvVar2 + 0x16) = 0xffff;
      if (-1 < *(int *)((int)pvVar2 + 0xc)) {
        if (*(int *)((int)pvVar2 + 4) != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)pvVar2 + 4));
          *(undefined4 *)((int)pvVar2 + 4) = 0;
        }
        *(undefined4 *)((int)pvVar2 + 8) = 0;
      }
      local_8 = local_8 + -1;
      iVar1 = iVar1 + -0x38;
    } while (-1 < local_8);
    param_1[3] = 0;
    return;
  }
  param_1[3] = 0;
  return;
}


// 10011c50 FUN_10011c50

DWORD __thiscall FUN_10011c50(void *this,int param_1,void *param_2)

{
  int *this_00;
  DWORD *pDVar1;
  code *pcVar2;
  int iVar3;
  DWORD DVar4;
  DWORD DVar5;
  void *pvVar6;
  undefined4 *puVar7;
  DWORD *pDVar8;
  int iVar9;
  uint uVar10;
  undefined4 *puVar11;
  longlong lVar12;
  int local_214;
  int local_20c;
  uint local_208;
  DWORD *local_204;
  uint auStack_200 [128];
  
  local_204 = (DWORD *)this;
  FUN_1000ef50((DWORD *)this);
  local_214 = -1;
  if ((param_1 == 0) && (*(int *)((int)param_2 + 0x150) != 0)) {
    local_214 = FUN_10011aa0(&DAT_10037b00,DAT_10037b0c);
    DAT_1002c728 = DAT_10037b0c + -1;
  }
  FUN_10011220(this,param_1,(uint)param_2);
  FUN_10011650(this,param_1,(int)param_2);
  FUN_10010340(this,param_1,(int)param_2);
  FUN_10010620(this,param_1,(int)param_2);
  FUN_10010980(this,param_1,(int)param_2);
  FUN_10010d30(this,param_1,(int)param_2);
  FUN_10010f90(this,param_1,(int)param_2);
  iVar9 = 0;
  uVar10 = 0;
  pDVar8 = (DWORD *)this;
  if (0 < *(int *)((int)param_2 + 0x150)) {
    do {
      iVar3 = *(int *)((int)param_2 + 0x154) + iVar9 * 8;
      local_208 = 0;
      lVar12 = FUN_1000de10(&local_208,
                            (char *)((int)param_2 + *(int *)((int)param_2 + iVar3 + 4) + iVar3));
      if ((uint)lVar12 != 0) {
        if (0x3f < uVar10) break;
        auStack_200[uVar10 * 2] = local_208;
        auStack_200[uVar10 * 2 + 1] = (uint)lVar12;
        uVar10 = uVar10 + 1;
      }
      iVar9 = iVar9 + 1;
    } while (iVar9 < *(int *)((int)param_2 + 0x150));
    if (uVar10 != 0) {
      iVar9 = *(int *)((int)this + 100) + uVar10;
      this_00 = (int *)((int)this + 0x58);
      if ((*(int *)((int)this + 0x5c) < iVar9) && (-1 < *(int *)((int)this + 0x60))) {
        *(int *)((int)this + 0x5c) = iVar9;
        if (*this_00 == 0) {
          iVar9 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar9 * 0x90);
        }
        else {
          iVar9 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this_00);
        }
        *this_00 = iVar9;
      }
      *(int *)((int)this + 0x68) = *this_00;
      local_20c = 0;
      if (0 < (int)uVar10) {
        do {
          iVar9 = *(int *)((int)this + 100);
          if (*(int *)((int)this + 0x5c) < iVar9 + 1) {
            FUN_100061d0(this_00,(iVar9 - *(int *)((int)this + 0x5c)) + 1);
          }
          *(int *)((int)this + 100) = *(int *)((int)this + 100) + 1;
          iVar3 = (*(int *)((int)this + 100) - iVar9) + -1;
          *(int *)((int)this + 0x68) = *this_00;
          if (0 < iVar3) {
            pvVar6 = (void *)(*this_00 + iVar9 * 0x90);
            FUN_10016630((void *)((int)pvVar6 + 0x90),pvVar6,iVar3 * 0x90);
          }
          puVar7 = (undefined4 *)(*this_00 + iVar9 * 0x90);
          if (puVar7 != (undefined4 *)0x0) {
            FUN_10009920(puVar7);
          }
          pvVar6 = (void *)auStack_200[local_20c * 2 + 1];
          *(uint *)(iVar9 * 0x90 + *this_00) = auStack_200[local_20c * 2];
          FUN_10011c50(local_204,iVar9,pvVar6);
          local_20c = local_20c + 1;
          pDVar8 = local_204;
        } while (local_20c < (int)uVar10);
      }
    }
  }
  DVar4 = GetCurrentThreadId();
  if (DVar4 != *pDVar8) {
    LOCK();
    DVar5 = *pDVar8;
    if (DVar5 == 0) {
      *pDVar8 = DVar4;
      DVar5 = 0;
    }
    UNLOCK();
    if (DVar5 != 0) {
      pcVar2 = (code *)swi(3);
      DVar4 = (*pcVar2)();
      return DVar4;
    }
  }
  pDVar8[1] = pDVar8[1] + 1;
  iVar9 = FUN_1000e990(param_2);
  pDVar1 = pDVar8 + 0x25;
  pDVar8[0x28] = 0;
  if (iVar9 != 0) {
    if ((int)pDVar8[0x26] < iVar9) {
      FUN_1000f200(pDVar1,iVar9 - pDVar8[0x26]);
    }
    pDVar8[0x28] = pDVar8[0x28] + iVar9;
    pvVar6 = (void *)*pDVar1;
    pDVar8[0x29] = (DWORD)pvVar6;
    if ((0 < (int)(pDVar8[0x28] - iVar9)) && (0 < iVar9)) {
      FUN_10016630((void *)((int)pvVar6 + iVar9 * 2),pvVar6,(pDVar8[0x28] - iVar9) * 2);
    }
  }
  FUN_1000e9e0(param_2,*pDVar1,iVar9);
  pDVar1 = pDVar8 + 1;
  *pDVar1 = *pDVar1 - 1;
  if (*pDVar1 == 0) {
    LOCK();
    *pDVar8 = 0;
    UNLOCK();
  }
  if (-1 < local_214) {
    pvVar6 = (void *)(DAT_10037b00 + local_214 * 0x38);
    FUN_100102f0((void *)((int)pvVar6 + 0x1c));
    FUN_10010230((void *)((int)pvVar6 + 0x1c));
    *(undefined2 *)((int)pvVar6 + 0x30) = 0xffff;
    if (-1 < *(int *)((int)pvVar6 + 0x28)) {
      if (*(int *)((int)pvVar6 + 0x20) != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)pvVar6 + 0x20));
        *(undefined4 *)((int)pvVar6 + 0x20) = 0;
      }
      *(undefined4 *)((int)pvVar6 + 0x24) = 0;
    }
    *(undefined2 *)((int)pvVar6 + 0x32) = 0xffff;
    if (-1 < *(int *)((int)pvVar6 + 0x28)) {
      if (*(int *)((int)pvVar6 + 0x20) != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)pvVar6 + 0x20));
        *(undefined4 *)((int)pvVar6 + 0x20) = 0;
      }
      *(undefined4 *)((int)pvVar6 + 0x24) = 0;
    }
    FUN_100102f0(pvVar6);
    FUN_10010230(pvVar6);
    *(undefined2 *)((int)pvVar6 + 0x14) = 0xffff;
    if (-1 < *(int *)((int)pvVar6 + 0xc)) {
      if (*(int *)((int)pvVar6 + 4) != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)pvVar6 + 4));
        *(undefined4 *)((int)pvVar6 + 4) = 0;
      }
      *(undefined4 *)((int)pvVar6 + 8) = 0;
    }
    *(undefined2 *)((int)pvVar6 + 0x16) = 0xffff;
    if (-1 < *(int *)((int)pvVar6 + 0xc)) {
      if (*(int *)((int)pvVar6 + 4) != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)pvVar6 + 4));
        *(undefined4 *)((int)pvVar6 + 4) = 0;
      }
      *(undefined4 *)((int)pvVar6 + 8) = 0;
    }
    if (0 < DAT_10037b0c) {
      if (local_214 != DAT_10037b0c + -1) {
        puVar7 = (undefined4 *)(DAT_10037b00 + -0x38 + DAT_10037b0c * 0x38);
        puVar11 = (undefined4 *)(local_214 * 0x38 + DAT_10037b00);
        for (iVar9 = 0xe; iVar9 != 0; iVar9 = iVar9 + -1) {
          *puVar11 = *puVar7;
          puVar7 = puVar7 + 1;
          puVar11 = puVar11 + 1;
        }
      }
      DAT_10037b0c = DAT_10037b0c + -1;
    }
    DAT_1002c728 = DAT_10037b0c + -1;
  }
  pDVar1 = pDVar8 + 1;
  *pDVar1 = *pDVar1 - 1;
  if (*pDVar1 == 0) {
    LOCK();
    *pDVar8 = 0;
    UNLOCK();
  }
  return pDVar8[1];
}


// 10012060 FUN_10012060

void __fastcall FUN_10012060(int *param_1)

{
  int iVar1;
  
  FUN_10011b20(param_1);
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


// 100120c0 FUN_100120c0

void __fastcall FUN_100120c0(int param_1)

{
  int iVar1;
  
  if (0 < *(int *)(param_1 + 0x14)) {
    iVar1 = 0;
    do {
      FUN_1000e140(*(undefined4 *)(*(int *)(param_1 + 8) + iVar1 * 4));
      iVar1 = iVar1 + 1;
    } while (iVar1 < *(int *)(param_1 + 0x14));
  }
  *(undefined4 *)(param_1 + 0x14) = 0;
  if (-1 < *(int *)(param_1 + 0x10)) {
    if (*(int *)(param_1 + 8) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 8));
      *(undefined4 *)(param_1 + 8) = 0;
    }
    *(undefined4 *)(param_1 + 0xc) = 0;
  }
  iVar1 = *(int *)(param_1 + 8);
  *(int *)(param_1 + 0x18) = iVar1;
  if (-1 < *(int *)(param_1 + 0x10)) {
    if (iVar1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
      *(undefined4 *)(param_1 + 8) = 0;
    }
    *(undefined4 *)(param_1 + 0xc) = 0;
  }
  return;
}


// 10012140 FUN_10012140

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012140(void)

{
  char *pcVar1;
  
  if ((_DAT_10037c68 & 1) == 0) {
    _DAT_10037c68 = _DAT_10037c68 | 1;
    _DAT_10037c4c = "swapcompactsurfaceheader_t";
    _DAT_10037c54 = 0;
    _DAT_10037c58 = 0;
    _DAT_10037c5c = 0;
    _DAT_10037c60 = 0;
    _DAT_10037c64 = 0;
    pcVar1 = "swapcompactsurfaceheader_t";
    do {
      _DAT_10037c50 = pcVar1;
      pcVar1 = _DAT_10037c50 + 1;
    } while (*_DAT_10037c50 != '\0');
    _DAT_10037c50 = _DAT_10037c50 + -0x10026934;
    _atexit((_func_4879 *)&LAB_100244d0);
  }
  _DAT_1002c758 = 0;
  _DAT_1002c750 = 7;
  _DAT_1002c74c = &DAT_1002cdec;
  return &DAT_1002c74c;
}


// 100121c0 FUN_100121c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100121c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037c88 & 1) == 0) {
    _DAT_10037c88 = _DAT_10037c88 | 1;
    _DAT_10037c6c = "legacysurfaceheader_t";
    _DAT_10037c74 = 0;
    _DAT_10037c78 = 0;
    _DAT_10037c7c = 0;
    _DAT_10037c80 = 0;
    _DAT_10037c84 = 0;
    pcVar1 = "legacysurfaceheader_t";
    do {
      _DAT_10037c70 = pcVar1;
      pcVar1 = _DAT_10037c70 + 1;
    } while (*_DAT_10037c70 != '\0');
    _DAT_10037c70 = _DAT_10037c70 + -0x1002691c;
    _atexit((_func_4879 *)&LAB_100244e0);
  }
  _DAT_1002c770 = 0;
  _DAT_1002c768 = 7;
  _DAT_1002c764 = &DAT_1002cfcc;
  return &DAT_1002c764;
}


// 10012240 FUN_10012240

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012240(void)

{
  char *pcVar1;
  
  if ((_DAT_10037ca8 & 1) == 0) {
    _DAT_10037ca8 = _DAT_10037ca8 | 1;
    _DAT_10037c8c = "studiohdr_t";
    _DAT_10037c94 = 0;
    _DAT_10037c98 = 0;
    _DAT_10037c9c = 0;
    _DAT_10037ca0 = 0;
    _DAT_10037ca4 = 0;
    pcVar1 = "studiohdr_t";
    do {
      _DAT_10037c90 = pcVar1;
      pcVar1 = _DAT_10037c90 + 1;
    } while (*_DAT_10037c90 != '\0');
    _DAT_10037c90 = _DAT_10037c90 + -0x10026910;
    _atexit((_func_4879 *)&LAB_100244f0);
  }
  _DAT_1002c788 = 0;
  _DAT_1002c780 = 0x4e;
  _DAT_1002c77c = &DAT_1002d1ac;
  return &DAT_1002c77c;
}


// 100122c0 FUN_100122c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100122c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037cc8 & 1) == 0) {
    _DAT_10037cc8 = _DAT_10037cc8 | 1;
    _DAT_10037cac = "studiohdr2_t";
    _DAT_10037cb4 = 0;
    _DAT_10037cb8 = 0;
    _DAT_10037cbc = 0;
    _DAT_10037cc0 = 0;
    _DAT_10037cc4 = 0;
    pcVar1 = "studiohdr2_t";
    do {
      _DAT_10037cb0 = pcVar1;
      pcVar1 = _DAT_10037cb0 + 1;
    } while (*_DAT_10037cb0 != '\0');
    _DAT_10037cb0 = _DAT_10037cb0 + -0x10026900;
    _atexit((_func_4879 *)&LAB_10024500);
  }
  _DAT_1002c7a0 = 0;
  _DAT_1002c798 = 6;
  _DAT_1002c794 = &DAT_1002e434;
  return &DAT_1002c794;
}


// 10012340 FUN_10012340

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012340(void)

{
  char *pcVar1;
  
  if ((_DAT_10037ce8 & 1) == 0) {
    _DAT_10037ce8 = _DAT_10037ce8 | 1;
    _DAT_10037ccc = "mstudiobone_t";
    _DAT_10037cd4 = 0;
    _DAT_10037cd8 = 0;
    _DAT_10037cdc = 0;
    _DAT_10037ce0 = 0;
    _DAT_10037ce4 = 0;
    pcVar1 = "mstudiobone_t";
    do {
      _DAT_10037cd0 = pcVar1;
      pcVar1 = _DAT_10037cd0 + 1;
    } while (*_DAT_10037cd0 != '\0');
    _DAT_10037cd0 = _DAT_10037cd0 + -0x100268f0;
    _atexit((_func_4879 *)&LAB_10024510);
  }
  _DAT_1002c7b8 = 0;
  _DAT_1002c7b0 = 0x12;
  _DAT_1002c7ac = &DAT_1002e5dc;
  return &DAT_1002c7ac;
}


// 100123c0 FUN_100123c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100123c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037d08 & 1) == 0) {
    _DAT_10037d08 = _DAT_10037d08 | 1;
    _DAT_10037cec = "mstudiolinearbone_t";
    _DAT_10037cf4 = 0;
    _DAT_10037cf8 = 0;
    _DAT_10037cfc = 0;
    _DAT_10037d00 = 0;
    _DAT_10037d04 = 0;
    pcVar1 = "mstudiolinearbone_t";
    do {
      _DAT_10037cf0 = pcVar1;
      pcVar1 = _DAT_10037cf0 + 1;
    } while (*_DAT_10037cf0 != '\0');
    _DAT_10037cf0 = _DAT_10037cf0 + -0x100268dc;
    _atexit((_func_4879 *)&LAB_10024520);
  }
  _DAT_1002c7d0 = 0;
  _DAT_1002c7c8 = 0xb;
  _DAT_1002c7c4 = &DAT_1002ea54;
  return &DAT_1002c7c4;
}


// 10012440 FUN_10012440

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012440(void)

{
  char *pcVar1;
  
  if ((_DAT_10037d28 & 1) == 0) {
    _DAT_10037d28 = _DAT_10037d28 | 1;
    _DAT_10037d0c = "mstudioaxisinterpbone_t";
    _DAT_10037d14 = 0;
    _DAT_10037d18 = 0;
    _DAT_10037d1c = 0;
    _DAT_10037d20 = 0;
    _DAT_10037d24 = 0;
    pcVar1 = "mstudioaxisinterpbone_t";
    do {
      _DAT_10037d10 = pcVar1;
      pcVar1 = _DAT_10037d10 + 1;
    } while (*_DAT_10037d10 != '\0');
    _DAT_10037d10 = _DAT_10037d10 + -0x100268c4;
    _atexit((_func_4879 *)&LAB_10024530);
  }
  _DAT_1002c7e8 = 0;
  _DAT_1002c7e0 = 4;
  _DAT_1002c7dc = &DAT_1002ed24;
  return &DAT_1002c7dc;
}


// 100124c0 FUN_100124c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100124c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037d48 & 1) == 0) {
    _DAT_10037d48 = _DAT_10037d48 | 1;
    _DAT_10037d2c = "mstudioquatinterpbone_t";
    _DAT_10037d34 = 0;
    _DAT_10037d38 = 0;
    _DAT_10037d3c = 0;
    _DAT_10037d40 = 0;
    _DAT_10037d44 = 0;
    pcVar1 = "mstudioquatinterpbone_t";
    do {
      _DAT_10037d30 = pcVar1;
      pcVar1 = _DAT_10037d30 + 1;
    } while (*_DAT_10037d30 != '\0');
    _DAT_10037d30 = _DAT_10037d30 + -0x100268ac;
    _atexit((_func_4879 *)&LAB_10024540);
  }
  _DAT_1002c800 = 0;
  _DAT_1002c7f8 = 3;
  _DAT_1002c7f4 = &DAT_1002ee54;
  return &DAT_1002c7f4;
}


// 10012540 FUN_10012540

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012540(void)

{
  char *pcVar1;
  
  if ((_DAT_10037d68 & 1) == 0) {
    _DAT_10037d68 = _DAT_10037d68 | 1;
    _DAT_10037d4c = "mstudiojigglebone_t";
    _DAT_10037d54 = 0;
    _DAT_10037d58 = 0;
    _DAT_10037d5c = 0;
    _DAT_10037d60 = 0;
    _DAT_10037d64 = 0;
    pcVar1 = "mstudiojigglebone_t";
    do {
      _DAT_10037d50 = pcVar1;
      pcVar1 = _DAT_10037d50 + 1;
    } while (*_DAT_10037d50 != '\0');
    _DAT_10037d50 = _DAT_10037d50 + -0x10026898;
    _atexit((_func_4879 *)&LAB_10024550);
  }
  _DAT_1002c818 = 0;
  _DAT_1002c810 = 0x1e;
  _DAT_1002c80c = &DAT_1002ef44;
  return &DAT_1002c80c;
}


// 100125c0 FUN_100125c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100125c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037d88 & 1) == 0) {
    _DAT_10037d88 = _DAT_10037d88 | 1;
    _DAT_10037d6c = "mstudioaimatbone_t";
    _DAT_10037d74 = 0;
    _DAT_10037d78 = 0;
    _DAT_10037d7c = 0;
    _DAT_10037d80 = 0;
    _DAT_10037d84 = 0;
    pcVar1 = "mstudioaimatbone_t";
    do {
      _DAT_10037d70 = pcVar1;
      pcVar1 = _DAT_10037d70 + 1;
    } while (*_DAT_10037d70 != '\0');
    _DAT_10037d70 = _DAT_10037d70 + -0x10026884;
    _atexit((_func_4879 *)&LAB_10024560);
  }
  _DAT_1002c830 = 0;
  _DAT_1002c828 = 5;
  _DAT_1002c824 = &DAT_1002f68c;
  return &DAT_1002c824;
}


// 10012640 FUN_10012640

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012640(void)

{
  char *pcVar1;
  
  if ((_DAT_10037da8 & 1) == 0) {
    _DAT_10037da8 = _DAT_10037da8 | 1;
    _DAT_10037d8c = "mstudioquatinterpinfo_t";
    _DAT_10037d94 = 0;
    _DAT_10037d98 = 0;
    _DAT_10037d9c = 0;
    _DAT_10037da0 = 0;
    _DAT_10037da4 = 0;
    pcVar1 = "mstudioquatinterpinfo_t";
    do {
      _DAT_10037d90 = pcVar1;
      pcVar1 = _DAT_10037d90 + 1;
    } while (*_DAT_10037d90 != '\0');
    _DAT_10037d90 = _DAT_10037d90 + -0x1002686c;
    _atexit((_func_4879 *)&LAB_10024570);
  }
  _DAT_1002c848 = 0;
  _DAT_1002c840 = 4;
  _DAT_1002c83c = &DAT_1002f7f4;
  return &DAT_1002c83c;
}


// 100126c0 FUN_100126c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100126c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037dc8 & 1) == 0) {
    _DAT_10037dc8 = _DAT_10037dc8 | 1;
    _DAT_10037dac = "mstudiobonecontroller_t";
    _DAT_10037db4 = 0;
    _DAT_10037db8 = 0;
    _DAT_10037dbc = 0;
    _DAT_10037dc0 = 0;
    _DAT_10037dc4 = 0;
    pcVar1 = "mstudiobonecontroller_t";
    do {
      _DAT_10037db0 = pcVar1;
      pcVar1 = _DAT_10037db0 + 1;
    } while (*_DAT_10037db0 != '\0');
    _DAT_10037db0 = _DAT_10037db0 + -0x10026854;
    _atexit((_func_4879 *)&LAB_10024580);
  }
  _DAT_1002c860 = 0;
  _DAT_1002c858 = 7;
  _DAT_1002c854 = &DAT_1002f924;
  return &DAT_1002c854;
}


// 10012740 FUN_10012740

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012740(void)

{
  char *pcVar1;
  
  if ((_DAT_10037de8 & 1) == 0) {
    _DAT_10037de8 = _DAT_10037de8 | 1;
    _DAT_10037dcc = "mstudioattachment_t";
    _DAT_10037dd4 = 0;
    _DAT_10037dd8 = 0;
    _DAT_10037ddc = 0;
    _DAT_10037de0 = 0;
    _DAT_10037de4 = 0;
    pcVar1 = "mstudioattachment_t";
    do {
      _DAT_10037dd0 = pcVar1;
      pcVar1 = _DAT_10037dd0 + 1;
    } while (*_DAT_10037dd0 != '\0');
    _DAT_10037dd0 = _DAT_10037dd0 + -0x10026840;
    _atexit((_func_4879 *)&LAB_10024590);
  }
  _DAT_1002c878 = 0;
  _DAT_1002c870 = 5;
  _DAT_1002c86c = &DAT_1002fb04;
  return &DAT_1002c86c;
}


// 100127c0 FUN_100127c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100127c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037e08 & 1) == 0) {
    _DAT_10037e08 = _DAT_10037e08 | 1;
    _DAT_10037dec = "mstudiohitboxset_t";
    _DAT_10037df4 = 0;
    _DAT_10037df8 = 0;
    _DAT_10037dfc = 0;
    _DAT_10037e00 = 0;
    _DAT_10037e04 = 0;
    pcVar1 = "mstudiohitboxset_t";
    do {
      _DAT_10037df0 = pcVar1;
      pcVar1 = _DAT_10037df0 + 1;
    } while (*_DAT_10037df0 != '\0');
    _DAT_10037df0 = _DAT_10037df0 + -0x1002682c;
    _atexit((_func_4879 *)&LAB_100245a0);
  }
  _DAT_1002c890 = 0;
  _DAT_1002c888 = 3;
  _DAT_1002c884 = &DAT_1002fc6c;
  return &DAT_1002c884;
}


// 10012840 FUN_10012840

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012840(void)

{
  char *pcVar1;
  
  if ((_DAT_10037e28 & 1) == 0) {
    _DAT_10037e28 = _DAT_10037e28 | 1;
    _DAT_10037e0c = "mstudiosrcbonetransform_t";
    _DAT_10037e14 = 0;
    _DAT_10037e18 = 0;
    _DAT_10037e1c = 0;
    _DAT_10037e20 = 0;
    _DAT_10037e24 = 0;
    pcVar1 = "mstudiosrcbonetransform_t";
    do {
      _DAT_10037e10 = pcVar1;
      pcVar1 = _DAT_10037e10 + 1;
    } while (*_DAT_10037e10 != '\0');
    _DAT_10037e10 = _DAT_10037e10 + -0x10026810;
    _atexit((_func_4879 *)&LAB_100245b0);
  }
  _DAT_1002c8a8 = 0;
  _DAT_1002c8a0 = 3;
  _DAT_1002c89c = &DAT_1002fd5c;
  return &DAT_1002c89c;
}


// 100128c0 FUN_100128c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100128c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037e48 & 1) == 0) {
    _DAT_10037e48 = _DAT_10037e48 | 1;
    _DAT_10037e2c = "mstudiobbox_t";
    _DAT_10037e34 = 0;
    _DAT_10037e38 = 0;
    _DAT_10037e3c = 0;
    _DAT_10037e40 = 0;
    _DAT_10037e44 = 0;
    pcVar1 = "mstudiobbox_t";
    do {
      _DAT_10037e30 = pcVar1;
      pcVar1 = _DAT_10037e30 + 1;
    } while (*_DAT_10037e30 != '\0');
    _DAT_10037e30 = _DAT_10037e30 + -0x10026800;
    _atexit((_func_4879 *)&LAB_100245c0);
  }
  _DAT_1002c8c0 = 0;
  _DAT_1002c8b8 = 6;
  _DAT_1002c8b4 = &DAT_1002fe4c;
  return &DAT_1002c8b4;
}


// 10012940 FUN_10012940

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012940(void)

{
  char *pcVar1;
  
  if ((_DAT_10037e68 & 1) == 0) {
    _DAT_10037e68 = _DAT_10037e68 | 1;
    _DAT_10037e4c = "mstudioanim_valueptr_t";
    _DAT_10037e54 = 0;
    _DAT_10037e58 = 0;
    _DAT_10037e5c = 0;
    _DAT_10037e60 = 0;
    _DAT_10037e64 = 0;
    pcVar1 = "mstudioanim_valueptr_t";
    do {
      _DAT_10037e50 = pcVar1;
      pcVar1 = _DAT_10037e50 + 1;
    } while (*_DAT_10037e50 != '\0');
    _DAT_10037e50 = _DAT_10037e50 + -0x100267e8;
    _atexit((_func_4879 *)&LAB_100245d0);
  }
  _DAT_1002c8d8 = 0;
  _DAT_1002c8d0 = 1;
  _DAT_1002c8cc = &DAT_1002fff4;
  return &DAT_1002c8cc;
}


// 100129c0 FUN_100129c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100129c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037e88 & 1) == 0) {
    _DAT_10037e88 = _DAT_10037e88 | 1;
    _DAT_10037e6c = "mstudiolocalhierarchy_t";
    _DAT_10037e74 = 0;
    _DAT_10037e78 = 0;
    _DAT_10037e7c = 0;
    _DAT_10037e80 = 0;
    _DAT_10037e84 = 0;
    pcVar1 = "mstudiolocalhierarchy_t";
    do {
      _DAT_10037e70 = pcVar1;
      pcVar1 = _DAT_10037e70 + 1;
    } while (*_DAT_10037e70 != '\0');
    _DAT_10037e70 = _DAT_10037e70 + -0x100267d0;
    _atexit((_func_4879 *)&LAB_100245e0);
  }
  _DAT_1002c8f0 = 0;
  _DAT_1002c8e8 = 9;
  _DAT_1002c8e4 = &DAT_1003006c;
  return &DAT_1002c8e4;
}


// 10012a40 FUN_10012a40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012a40(void)

{
  char *pcVar1;
  
  if ((_DAT_10037ea8 & 1) == 0) {
    _DAT_10037ea8 = _DAT_10037ea8 | 1;
    _DAT_10037e8c = "mstudioanimsections_t";
    _DAT_10037e94 = 0;
    _DAT_10037e98 = 0;
    _DAT_10037e9c = 0;
    _DAT_10037ea0 = 0;
    _DAT_10037ea4 = 0;
    pcVar1 = "mstudioanimsections_t";
    do {
      _DAT_10037e90 = pcVar1;
      pcVar1 = _DAT_10037e90 + 1;
    } while (*_DAT_10037e90 != '\0');
    _DAT_10037e90 = _DAT_10037e90 + -0x100267b8;
    _atexit((_func_4879 *)&LAB_100245f0);
  }
  _DAT_1002c908 = 0;
  _DAT_1002c900 = 2;
  _DAT_1002c8fc = &DAT_100302c4;
  return &DAT_1002c8fc;
}


// 10012ac0 FUN_10012ac0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012ac0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037ec8 & 1) == 0) {
    _DAT_10037ec8 = _DAT_10037ec8 | 1;
    _DAT_10037eac = "mstudioanimdesc_t";
    _DAT_10037eb4 = 0;
    _DAT_10037eb8 = 0;
    _DAT_10037ebc = 0;
    _DAT_10037ec0 = 0;
    _DAT_10037ec4 = 0;
    pcVar1 = "mstudioanimdesc_t";
    do {
      _DAT_10037eb0 = pcVar1;
      pcVar1 = _DAT_10037eb0 + 1;
    } while (*_DAT_10037eb0 != '\0');
    _DAT_10037eb0 = _DAT_10037eb0 + -0x100267a4;
    _atexit((_func_4879 *)&LAB_10024600);
  }
  _DAT_1002c920 = 0;
  _DAT_1002c918 = 0x16;
  _DAT_1002c914 = &DAT_1003037c;
  return &DAT_1002c914;
}


// 10012b40 FUN_10012b40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012b40(void)

{
  char *pcVar1;
  
  if ((_DAT_10037ee8 & 1) == 0) {
    _DAT_10037ee8 = _DAT_10037ee8 | 1;
    _DAT_10037ecc = "mstudio_rle_anim_t";
    _DAT_10037ed4 = 0;
    _DAT_10037ed8 = 0;
    _DAT_10037edc = 0;
    _DAT_10037ee0 = 0;
    _DAT_10037ee4 = 0;
    pcVar1 = "mstudio_rle_anim_t";
    do {
      _DAT_10037ed0 = pcVar1;
      pcVar1 = _DAT_10037ed0 + 1;
    } while (*_DAT_10037ed0 != '\0');
    _DAT_10037ed0 = _DAT_10037ed0 + -0x10026790;
    _atexit((_func_4879 *)&LAB_10024610);
  }
  _DAT_1002c938 = 0;
  _DAT_1002c930 = 3;
  _DAT_1002c92c = &DAT_100308e4;
  return &DAT_1002c92c;
}


// 10012bc0 FUN_10012bc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012bc0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037f08 & 1) == 0) {
    _DAT_10037f08 = _DAT_10037f08 | 1;
    _DAT_10037eec = "mstudio_frame_anim_t";
    _DAT_10037ef4 = 0;
    _DAT_10037ef8 = 0;
    _DAT_10037efc = 0;
    _DAT_10037f00 = 0;
    _DAT_10037f04 = 0;
    pcVar1 = "mstudio_frame_anim_t";
    do {
      _DAT_10037ef0 = pcVar1;
      pcVar1 = _DAT_10037ef0 + 1;
    } while (*_DAT_10037ef0 != '\0');
    _DAT_10037ef0 = _DAT_10037ef0 + -0x10026778;
    _atexit((_func_4879 *)&LAB_10024620);
  }
  _DAT_1002c950 = 0;
  _DAT_1002c948 = 4;
  _DAT_1002c944 = &DAT_100309d4;
  return &DAT_1002c944;
}


// 10012c40 FUN_10012c40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012c40(void)

{
  char *pcVar1;
  
  if ((_DAT_10037f28 & 1) == 0) {
    _DAT_10037f28 = _DAT_10037f28 | 1;
    _DAT_10037f0c = "mstudioikerror_t";
    _DAT_10037f14 = 0;
    _DAT_10037f18 = 0;
    _DAT_10037f1c = 0;
    _DAT_10037f20 = 0;
    _DAT_10037f24 = 0;
    pcVar1 = "mstudioikerror_t";
    do {
      _DAT_10037f10 = pcVar1;
      pcVar1 = _DAT_10037f10 + 1;
    } while (*_DAT_10037f10 != '\0');
    _DAT_10037f10 = _DAT_10037f10 + -0x10026764;
    _atexit((_func_4879 *)&LAB_10024630);
  }
  _DAT_1002c968 = 0;
  _DAT_1002c960 = 2;
  _DAT_1002c95c = &DAT_10030b04;
  return &DAT_1002c95c;
}


// 10012cc0 FUN_10012cc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012cc0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037f48 & 1) == 0) {
    _DAT_10037f48 = _DAT_10037f48 | 1;
    _DAT_10037f2c = "mstudiocompressedikerror_t";
    _DAT_10037f34 = 0;
    _DAT_10037f38 = 0;
    _DAT_10037f3c = 0;
    _DAT_10037f40 = 0;
    _DAT_10037f44 = 0;
    pcVar1 = "mstudiocompressedikerror_t";
    do {
      _DAT_10037f30 = pcVar1;
      pcVar1 = _DAT_10037f30 + 1;
    } while (*_DAT_10037f30 != '\0');
    _DAT_10037f30 = _DAT_10037f30 + -0x10026748;
    _atexit((_func_4879 *)&LAB_10024640);
  }
  _DAT_1002c980 = 0;
  _DAT_1002c978 = 2;
  _DAT_1002c974 = &DAT_10030bbc;
  return &DAT_1002c974;
}


// 10012d40 FUN_10012d40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012d40(void)

{
  char *pcVar1;
  
  if ((_DAT_10037f68 & 1) == 0) {
    _DAT_10037f68 = _DAT_10037f68 | 1;
    _DAT_10037f4c = "mstudioikrule_t";
    _DAT_10037f54 = 0;
    _DAT_10037f58 = 0;
    _DAT_10037f5c = 0;
    _DAT_10037f60 = 0;
    _DAT_10037f64 = 0;
    pcVar1 = "mstudioikrule_t";
    do {
      _DAT_10037f50 = pcVar1;
      pcVar1 = _DAT_10037f50 + 1;
    } while (*_DAT_10037f50 != '\0');
    _DAT_10037f50 = _DAT_10037f50 + -0x10026738;
    _atexit((_func_4879 *)&LAB_10024650);
  }
  _DAT_1002c998 = 0;
  _DAT_1002c990 = 0x1b;
  _DAT_1002c98c = &DAT_10030c74;
  return &DAT_1002c98c;
}


// 10012dc0 FUN_10012dc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012dc0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037f88 & 1) == 0) {
    _DAT_10037f88 = _DAT_10037f88 | 1;
    _DAT_10037f6c = "mstudiomovement_t";
    _DAT_10037f74 = 0;
    _DAT_10037f78 = 0;
    _DAT_10037f7c = 0;
    _DAT_10037f80 = 0;
    _DAT_10037f84 = 0;
    pcVar1 = "mstudiomovement_t";
    do {
      _DAT_10037f70 = pcVar1;
      pcVar1 = _DAT_10037f70 + 1;
    } while (*_DAT_10037f70 != '\0');
    _DAT_10037f70 = _DAT_10037f70 + -0x10026724;
    _atexit((_func_4879 *)&LAB_10024660);
  }
  _DAT_1002c9b0 = 0;
  _DAT_1002c9a8 = 7;
  _DAT_1002c9a4 = &DAT_10031304;
  return &DAT_1002c9a4;
}


// 10012e40 FUN_10012e40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012e40(void)

{
  char *pcVar1;
  
  if ((_DAT_10037fa8 & 1) == 0) {
    _DAT_10037fa8 = _DAT_10037fa8 | 1;
    _DAT_10037f8c = "mstudioseqdesc_t";
    _DAT_10037f94 = 0;
    _DAT_10037f98 = 0;
    _DAT_10037f9c = 0;
    _DAT_10037fa0 = 0;
    _DAT_10037fa4 = 0;
    pcVar1 = "mstudioseqdesc_t";
    do {
      _DAT_10037f90 = pcVar1;
      pcVar1 = _DAT_10037f90 + 1;
    } while (*_DAT_10037f90 != '\0');
    _DAT_10037f90 = _DAT_10037f90 + -0x10026710;
    _atexit((_func_4879 *)&LAB_10024670);
  }
  _DAT_1002c9c8 = 0;
  _DAT_1002c9c0 = 0x27;
  _DAT_1002c9bc = &DAT_100314e4;
  return &DAT_1002c9bc;
}


// 10012ec0 FUN_10012ec0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012ec0(void)

{
  char *pcVar1;
  
  if ((_DAT_10037fc8 & 1) == 0) {
    _DAT_10037fc8 = _DAT_10037fc8 | 1;
    _DAT_10037fac = "mstudioevent_t";
    _DAT_10037fb4 = 0;
    _DAT_10037fb8 = 0;
    _DAT_10037fbc = 0;
    _DAT_10037fc0 = 0;
    _DAT_10037fc4 = 0;
    pcVar1 = "mstudioevent_t";
    do {
      _DAT_10037fb0 = pcVar1;
      pcVar1 = _DAT_10037fb0 + 1;
    } while (*_DAT_10037fb0 != '\0');
    _DAT_10037fb0 = _DAT_10037fb0 + -0x10026700;
    _atexit((_func_4879 *)&LAB_10024680);
  }
  _DAT_1002c9e0 = 0;
  _DAT_1002c9d8 = 5;
  _DAT_1002c9d4 = &DAT_10031e44;
  return &DAT_1002c9d4;
}


// 10012f40 FUN_10012f40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012f40(void)

{
  char *pcVar1;
  
  if ((_DAT_10037fe8 & 1) == 0) {
    _DAT_10037fe8 = _DAT_10037fe8 | 1;
    _DAT_10037fcc = "mstudioautolayer_t";
    _DAT_10037fd4 = 0;
    _DAT_10037fd8 = 0;
    _DAT_10037fdc = 0;
    _DAT_10037fe0 = 0;
    _DAT_10037fe4 = 0;
    pcVar1 = "mstudioautolayer_t";
    do {
      _DAT_10037fd0 = pcVar1;
      pcVar1 = _DAT_10037fd0 + 1;
    } while (*_DAT_10037fd0 != '\0');
    _DAT_10037fd0 = _DAT_10037fd0 + -0x100266ec;
    _atexit((_func_4879 *)&LAB_10024690);
  }
  _DAT_1002c9f8 = 0;
  _DAT_1002c9f0 = 7;
  _DAT_1002c9ec = &DAT_10031fac;
  return &DAT_1002c9ec;
}


// 10012fc0 FUN_10012fc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10012fc0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038008 & 1) == 0) {
    _DAT_10038008 = _DAT_10038008 | 1;
    _DAT_10037fec = "mstudioiklock_t";
    _DAT_10037ff4 = 0;
    _DAT_10037ff8 = 0;
    _DAT_10037ffc = 0;
    _DAT_10038000 = 0;
    _DAT_10038004 = 0;
    pcVar1 = "mstudioiklock_t";
    do {
      _DAT_10037ff0 = pcVar1;
      pcVar1 = _DAT_10037ff0 + 1;
    } while (*_DAT_10037ff0 != '\0');
    _DAT_10037ff0 = _DAT_10037ff0 + -0x100266dc;
    _atexit((_func_4879 *)&LAB_100246a0);
  }
  _DAT_1002ca10 = 0;
  _DAT_1002ca08 = 5;
  _DAT_1002ca04 = &DAT_1003218c;
  return &DAT_1002ca04;
}


// 10013040 FUN_10013040

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013040(void)

{
  char *pcVar1;
  
  if ((_DAT_10038028 & 1) == 0) {
    _DAT_10038028 = _DAT_10038028 | 1;
    _DAT_1003800c = "mstudiobodyparts_t";
    _DAT_10038014 = 0;
    _DAT_10038018 = 0;
    _DAT_1003801c = 0;
    _DAT_10038020 = 0;
    _DAT_10038024 = 0;
    pcVar1 = "mstudiobodyparts_t";
    do {
      _DAT_10038010 = pcVar1;
      pcVar1 = _DAT_10038010 + 1;
    } while (*_DAT_10038010 != '\0');
    _DAT_10038010 = _DAT_10038010 + -0x100266c8;
    _atexit((_func_4879 *)&LAB_100246b0);
  }
  _DAT_1002ca28 = 0;
  _DAT_1002ca20 = 4;
  _DAT_1002ca1c = &DAT_100322f4;
  return &DAT_1002ca1c;
}


// 100130c0 FUN_100130c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100130c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038048 & 1) == 0) {
    _DAT_10038048 = _DAT_10038048 | 1;
    _DAT_1003802c = "mstudiomodel_t";
    _DAT_10038034 = 0;
    _DAT_10038038 = 0;
    _DAT_1003803c = 0;
    _DAT_10038040 = 0;
    _DAT_10038044 = 0;
    pcVar1 = "mstudiomodel_t";
    do {
      _DAT_10038030 = pcVar1;
      pcVar1 = _DAT_10038030 + 1;
    } while (*_DAT_10038030 != '\0');
    _DAT_10038030 = _DAT_10038030 + -0x100266b8;
    _atexit((_func_4879 *)&LAB_100246c0);
  }
  _DAT_1002ca40 = 0;
  _DAT_1002ca38 = 0xe;
  _DAT_1002ca34 = &DAT_10032424;
  return &DAT_1002ca34;
}


// 10013140 FUN_10013140

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013140(void)

{
  char *pcVar1;
  
  if ((_DAT_10038068 & 1) == 0) {
    _DAT_10038068 = _DAT_10038068 | 1;
    _DAT_1003804c = "mstudio_modelvertexdata_t";
    _DAT_10038054 = 0;
    _DAT_10038058 = 0;
    _DAT_1003805c = 0;
    _DAT_10038060 = 0;
    _DAT_10038064 = 0;
    pcVar1 = "mstudio_modelvertexdata_t";
    do {
      _DAT_10038050 = pcVar1;
      pcVar1 = _DAT_10038050 + 1;
    } while (*_DAT_10038050 != '\0');
    _DAT_10038050 = _DAT_10038050 + -0x1002669c;
    _atexit((_func_4879 *)&LAB_100246d0);
  }
  _DAT_1002ca58 = 0;
  _DAT_1002ca50 = 2;
  _DAT_1002ca4c = &DAT_100327ac;
  return &DAT_1002ca4c;
}


// 100131c0 FUN_100131c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100131c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038088 & 1) == 0) {
    _DAT_10038088 = _DAT_10038088 | 1;
    _DAT_1003806c = "mstudioflexdesc_t";
    _DAT_10038074 = 0;
    _DAT_10038078 = 0;
    _DAT_1003807c = 0;
    _DAT_10038080 = 0;
    _DAT_10038084 = 0;
    pcVar1 = "mstudioflexdesc_t";
    do {
      _DAT_10038070 = pcVar1;
      pcVar1 = _DAT_10038070 + 1;
    } while (*_DAT_10038070 != '\0');
    _DAT_10038070 = _DAT_10038070 + -0x10026688;
    _atexit((_func_4879 *)&LAB_100246e0);
  }
  _DAT_1002ca70 = 0;
  _DAT_1002ca68 = 1;
  _DAT_1002ca64 = &DAT_10032864;
  return &DAT_1002ca64;
}


// 10013240 FUN_10013240

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013240(void)

{
  char *pcVar1;
  
  if ((_DAT_100380a8 & 1) == 0) {
    _DAT_100380a8 = _DAT_100380a8 | 1;
    _DAT_1003808c = "mstudioflexcontroller_t";
    _DAT_10038094 = 0;
    _DAT_10038098 = 0;
    _DAT_1003809c = 0;
    _DAT_100380a0 = 0;
    _DAT_100380a4 = 0;
    pcVar1 = "mstudioflexcontroller_t";
    do {
      _DAT_10038090 = pcVar1;
      pcVar1 = _DAT_10038090 + 1;
    } while (*_DAT_10038090 != '\0');
    _DAT_10038090 = _DAT_10038090 + -0x10026670;
    _atexit((_func_4879 *)&LAB_100246f0);
  }
  _DAT_1002ca88 = 0;
  _DAT_1002ca80 = 5;
  _DAT_1002ca7c = &DAT_100328dc;
  return &DAT_1002ca7c;
}


// 100132c0 FUN_100132c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100132c0(void)

{
  char *pcVar1;
  
  if ((_DAT_100380c8 & 1) == 0) {
    _DAT_100380c8 = _DAT_100380c8 | 1;
    _DAT_100380ac = "mstudioflexcontrollerui_t";
    _DAT_100380b4 = 0;
    _DAT_100380b8 = 0;
    _DAT_100380bc = 0;
    _DAT_100380c0 = 0;
    _DAT_100380c4 = 0;
    pcVar1 = "mstudioflexcontrollerui_t";
    do {
      _DAT_100380b0 = pcVar1;
      pcVar1 = _DAT_100380b0 + 1;
    } while (*_DAT_100380b0 != '\0');
    _DAT_100380b0 = _DAT_100380b0 + -0x10026654;
    _atexit((_func_4879 *)&LAB_10024700);
  }
  _DAT_1002caa0 = 0;
  _DAT_1002ca98 = 7;
  _DAT_1002ca94 = &DAT_10032a44;
  return &DAT_1002ca94;
}


// 10013340 FUN_10013340

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013340(void)

{
  char *pcVar1;
  
  if ((_DAT_100380e8 & 1) == 0) {
    _DAT_100380e8 = _DAT_100380e8 | 1;
    _DAT_100380cc = "mstudioflexrule_t";
    _DAT_100380d4 = 0;
    _DAT_100380d8 = 0;
    _DAT_100380dc = 0;
    _DAT_100380e0 = 0;
    _DAT_100380e4 = 0;
    pcVar1 = "mstudioflexrule_t";
    do {
      _DAT_100380d0 = pcVar1;
      pcVar1 = _DAT_100380d0 + 1;
    } while (*_DAT_100380d0 != '\0');
    _DAT_100380d0 = _DAT_100380d0 + -0x10026640;
    _atexit((_func_4879 *)&LAB_10024710);
  }
  _DAT_1002cab8 = 0;
  _DAT_1002cab0 = 3;
  _DAT_1002caac = &DAT_10032c24;
  return &DAT_1002caac;
}


// 100133c0 FUN_100133c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100133c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038108 & 1) == 0) {
    _DAT_10038108 = _DAT_10038108 | 1;
    _DAT_100380ec = "mstudioflexop_t";
    _DAT_100380f4 = 0;
    _DAT_100380f8 = 0;
    _DAT_100380fc = 0;
    _DAT_10038100 = 0;
    _DAT_10038104 = 0;
    pcVar1 = "mstudioflexop_t";
    do {
      _DAT_100380f0 = pcVar1;
      pcVar1 = _DAT_100380f0 + 1;
    } while (*_DAT_100380f0 != '\0');
    _DAT_100380f0 = _DAT_100380f0 + -0x10026630;
    _atexit((_func_4879 *)&LAB_10024720);
  }
  _DAT_1002cad0 = 0;
  _DAT_1002cac8 = 2;
  _DAT_1002cac4 = &DAT_10032d14;
  return &DAT_1002cac4;
}


// 10013440 FUN_10013440

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013440(void)

{
  char *pcVar1;
  
  if ((_DAT_10038128 & 1) == 0) {
    _DAT_10038128 = _DAT_10038128 | 1;
    _DAT_1003810c = "mstudioikchain_t";
    _DAT_10038114 = 0;
    _DAT_10038118 = 0;
    _DAT_1003811c = 0;
    _DAT_10038120 = 0;
    _DAT_10038124 = 0;
    pcVar1 = "mstudioikchain_t";
    do {
      _DAT_10038110 = pcVar1;
      pcVar1 = _DAT_10038110 + 1;
    } while (*_DAT_10038110 != '\0');
    _DAT_10038110 = _DAT_10038110 + -0x1002661c;
    _atexit((_func_4879 *)&LAB_10024730);
  }
  _DAT_1002cae8 = 0;
  _DAT_1002cae0 = 4;
  _DAT_1002cadc = &DAT_10032dcc;
  return &DAT_1002cadc;
}


// 100134c0 FUN_100134c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100134c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038148 & 1) == 0) {
    _DAT_10038148 = _DAT_10038148 | 1;
    _DAT_1003812c = "mstudioiklink_t";
    _DAT_10038134 = 0;
    _DAT_10038138 = 0;
    _DAT_1003813c = 0;
    _DAT_10038140 = 0;
    _DAT_10038144 = 0;
    pcVar1 = "mstudioiklink_t";
    do {
      _DAT_10038130 = pcVar1;
      pcVar1 = _DAT_10038130 + 1;
    } while (*_DAT_10038130 != '\0');
    _DAT_10038130 = _DAT_10038130 + -0x1002660c;
    _atexit((_func_4879 *)&LAB_10024740);
  }
  _DAT_1002cb00 = 0;
  _DAT_1002caf8 = 3;
  _DAT_1002caf4 = &DAT_10032efc;
  return &DAT_1002caf4;
}


// 10013540 FUN_10013540

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013540(void)

{
  char *pcVar1;
  
  if ((_DAT_10038168 & 1) == 0) {
    _DAT_10038168 = _DAT_10038168 | 1;
    _DAT_1003814c = "mstudiomouth_t";
    _DAT_10038154 = 0;
    _DAT_10038158 = 0;
    _DAT_1003815c = 0;
    _DAT_10038160 = 0;
    _DAT_10038164 = 0;
    pcVar1 = "mstudiomouth_t";
    do {
      _DAT_10038150 = pcVar1;
      pcVar1 = _DAT_10038150 + 1;
    } while (*_DAT_10038150 != '\0');
    _DAT_10038150 = _DAT_10038150 + -0x100265fc;
    _atexit((_func_4879 *)&LAB_10024750);
  }
  _DAT_1002cb18 = 0;
  _DAT_1002cb10 = 3;
  _DAT_1002cb0c = &DAT_10032fec;
  return &DAT_1002cb0c;
}


// 100135c0 FUN_100135c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100135c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038188 & 1) == 0) {
    _DAT_10038188 = _DAT_10038188 | 1;
    _DAT_1003816c = "mstudioposeparamdesc_t";
    _DAT_10038174 = 0;
    _DAT_10038178 = 0;
    _DAT_1003817c = 0;
    _DAT_10038180 = 0;
    _DAT_10038184 = 0;
    pcVar1 = "mstudioposeparamdesc_t";
    do {
      _DAT_10038170 = pcVar1;
      pcVar1 = _DAT_10038170 + 1;
    } while (*_DAT_10038170 != '\0');
    _DAT_10038170 = _DAT_10038170 + -0x100265e4;
    _atexit((_func_4879 *)&LAB_10024760);
  }
  _DAT_1002cb30 = 0;
  _DAT_1002cb28 = 5;
  _DAT_1002cb24 = &DAT_100330dc;
  return &DAT_1002cb24;
}


// 10013640 FUN_10013640

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013640(void)

{
  char *pcVar1;
  
  if ((_DAT_100381a8 & 1) == 0) {
    _DAT_100381a8 = _DAT_100381a8 | 1;
    _DAT_1003818c = "mstudiomesh_t";
    _DAT_10038194 = 0;
    _DAT_10038198 = 0;
    _DAT_1003819c = 0;
    _DAT_100381a0 = 0;
    _DAT_100381a4 = 0;
    pcVar1 = "mstudiomesh_t";
    do {
      _DAT_10038190 = pcVar1;
      pcVar1 = _DAT_10038190 + 1;
    } while (*_DAT_10038190 != '\0');
    _DAT_10038190 = _DAT_10038190 + -0x100265d4;
    _atexit((_func_4879 *)&LAB_10024770);
  }
  _DAT_1002cb48 = 0;
  _DAT_1002cb40 = 0xc;
  _DAT_1002cb3c = &DAT_10033244;
  return &DAT_1002cb3c;
}


// 100136c0 FUN_100136c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100136c0(void)

{
  char *pcVar1;
  
  if ((_DAT_100381c8 & 1) == 0) {
    _DAT_100381c8 = _DAT_100381c8 | 1;
    _DAT_100381ac = "mstudio_meshvertexdata_t";
    _DAT_100381b4 = 0;
    _DAT_100381b8 = 0;
    _DAT_100381bc = 0;
    _DAT_100381c0 = 0;
    _DAT_100381c4 = 0;
    pcVar1 = "mstudio_meshvertexdata_t";
    do {
      _DAT_100381b0 = pcVar1;
      pcVar1 = _DAT_100381b0 + 1;
    } while (*_DAT_100381b0 != '\0');
    _DAT_100381b0 = _DAT_100381b0 + -0x100265b8;
    _atexit((_func_4879 *)&LAB_10024780);
  }
  _DAT_1002cb60 = 0;
  _DAT_1002cb58 = 2;
  _DAT_1002cb54 = &DAT_10033554;
  return &DAT_1002cb54;
}


// 10013740 FUN_10013740

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013740(void)

{
  char *pcVar1;
  
  if ((_DAT_100381e8 & 1) == 0) {
    _DAT_100381e8 = _DAT_100381e8 | 1;
    _DAT_100381cc = "mstudioeyeball_t";
    _DAT_100381d4 = 0;
    _DAT_100381d8 = 0;
    _DAT_100381dc = 0;
    _DAT_100381e0 = 0;
    _DAT_100381e4 = 0;
    pcVar1 = "mstudioeyeball_t";
    do {
      _DAT_100381d0 = pcVar1;
      pcVar1 = _DAT_100381d0 + 1;
    } while (*_DAT_100381d0 != '\0');
    _DAT_100381d0 = _DAT_100381d0 + -0x100265a4;
    _atexit((_func_4879 *)&LAB_10024790);
  }
  _DAT_1002cb78 = 0;
  _DAT_1002cb70 = 0x15;
  _DAT_1002cb6c = &DAT_1003360c;
  return &DAT_1002cb6c;
}


// 100137c0 FUN_100137c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100137c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038208 & 1) == 0) {
    _DAT_10038208 = _DAT_10038208 | 1;
    _DAT_100381ec = "mstudioflex_t";
    _DAT_100381f4 = 0;
    _DAT_100381f8 = 0;
    _DAT_100381fc = 0;
    _DAT_10038200 = 0;
    _DAT_10038204 = 0;
    pcVar1 = "mstudioflex_t";
    do {
      _DAT_100381f0 = pcVar1;
      pcVar1 = _DAT_100381f0 + 1;
    } while (*_DAT_100381f0 != '\0');
    _DAT_100381f0 = _DAT_100381f0 + -0x10026594;
    _atexit((_func_4879 *)&LAB_100247a0);
  }
  _DAT_1002cb90 = 0;
  _DAT_1002cb88 = 0xb;
  _DAT_1002cb84 = &DAT_10033b34;
  return &DAT_1002cb84;
}


// 10013840 FUN_10013840

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013840(void)

{
  char *pcVar1;
  
  if ((_DAT_10038228 & 1) == 0) {
    _DAT_10038228 = _DAT_10038228 | 1;
    _DAT_1003820c = "mstudiovertanim_t";
    _DAT_10038214 = 0;
    _DAT_10038218 = 0;
    _DAT_1003821c = 0;
    _DAT_10038220 = 0;
    _DAT_10038224 = 0;
    pcVar1 = "mstudiovertanim_t";
    do {
      _DAT_10038210 = pcVar1;
      pcVar1 = _DAT_10038210 + 1;
    } while (*_DAT_10038210 != '\0');
    _DAT_10038210 = _DAT_10038210 + -0x10026580;
    _atexit((_func_4879 *)&LAB_100247b0);
  }
  _DAT_1002cba8 = 0;
  _DAT_1002cba0 = 5;
  _DAT_1002cb9c = &DAT_10033e04;
  return &DAT_1002cb9c;
}


// 100138c0 FUN_100138c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100138c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038248 & 1) == 0) {
    _DAT_10038248 = _DAT_10038248 | 1;
    _DAT_1003822c = "mstudiovertanim_wrinkle_t";
    _DAT_10038234 = 0;
    _DAT_10038238 = 0;
    _DAT_1003823c = 0;
    _DAT_10038240 = 0;
    _DAT_10038244 = 0;
    pcVar1 = "mstudiovertanim_wrinkle_t";
    do {
      _DAT_10038230 = pcVar1;
      pcVar1 = _DAT_10038230 + 1;
    } while (*_DAT_10038230 != '\0');
    _DAT_10038230 = _DAT_10038230 + -0x10026564;
    _atexit((_func_4879 *)&LAB_100247c0);
  }
  _DAT_1002cbc0 = 0;
  _DAT_1002cbb8 = 6;
  _DAT_1002cbb4 = &DAT_10033f6c;
  return &DAT_1002cbb4;
}


// 10013940 FUN_10013940

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013940(void)

{
  char *pcVar1;
  
  if ((_DAT_10038268 & 1) == 0) {
    _DAT_10038268 = _DAT_10038268 | 1;
    _DAT_1003824c = "mstudiomodelgroup_t";
    _DAT_10038254 = 0;
    _DAT_10038258 = 0;
    _DAT_1003825c = 0;
    _DAT_10038260 = 0;
    _DAT_10038264 = 0;
    pcVar1 = "mstudiomodelgroup_t";
    do {
      _DAT_10038250 = pcVar1;
      pcVar1 = _DAT_10038250 + 1;
    } while (*_DAT_10038250 != '\0');
    _DAT_10038250 = _DAT_10038250 + -0x10026550;
    _atexit((_func_4879 *)&LAB_100247d0);
  }
  _DAT_1002cbd8 = 0;
  _DAT_1002cbd0 = 2;
  _DAT_1002cbcc = &DAT_10034114;
  return &DAT_1002cbcc;
}


// 100139c0 FUN_100139c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100139c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038288 & 1) == 0) {
    _DAT_10038288 = _DAT_10038288 | 1;
    _DAT_1003826c = "mstudioanimblock_t";
    _DAT_10038274 = 0;
    _DAT_10038278 = 0;
    _DAT_1003827c = 0;
    _DAT_10038280 = 0;
    _DAT_10038284 = 0;
    pcVar1 = "mstudioanimblock_t";
    do {
      _DAT_10038270 = pcVar1;
      pcVar1 = _DAT_10038270 + 1;
    } while (*_DAT_10038270 != '\0');
    _DAT_10038270 = _DAT_10038270 + -0x1002653c;
    _atexit((_func_4879 *)&LAB_100247e0);
  }
  _DAT_1002cbf0 = 0;
  _DAT_1002cbe8 = 2;
  _DAT_1002cbe4 = &DAT_100341cc;
  return &DAT_1002cbe4;
}


// 10013a40 FUN_10013a40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013a40(void)

{
  char *pcVar1;
  
  if ((_DAT_100382a8 & 1) == 0) {
    _DAT_100382a8 = _DAT_100382a8 | 1;
    _DAT_1003828c = "mstudiotexture_t";
    _DAT_10038294 = 0;
    _DAT_10038298 = 0;
    _DAT_1003829c = 0;
    _DAT_100382a0 = 0;
    _DAT_100382a4 = 0;
    pcVar1 = "mstudiotexture_t";
    do {
      _DAT_10038290 = pcVar1;
      pcVar1 = _DAT_10038290 + 1;
    } while (*_DAT_10038290 != '\0');
    _DAT_10038290 = _DAT_10038290 + -0x10026528;
    _atexit((_func_4879 *)&LAB_100247f0);
  }
  _DAT_1002cc08 = 0;
  _DAT_1002cc00 = 7;
  _DAT_1002cbfc = &DAT_10034284;
  return &DAT_1002cbfc;
}


// 10013ac0 FUN_10013ac0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013ac0(void)

{
  char *pcVar1;
  
  if ((_DAT_100382c8 & 1) == 0) {
    _DAT_100382c8 = _DAT_100382c8 | 1;
    _DAT_100382ac = "vertexFileHeader_t";
    _DAT_100382b4 = 0;
    _DAT_100382b8 = 0;
    _DAT_100382bc = 0;
    _DAT_100382c0 = 0;
    _DAT_100382c4 = 0;
    pcVar1 = "vertexFileHeader_t";
    do {
      _DAT_100382b0 = pcVar1;
      pcVar1 = _DAT_100382b0 + 1;
    } while (*_DAT_100382b0 != '\0');
    _DAT_100382b0 = _DAT_100382b0 + -0x10026514;
    _atexit((_func_4879 *)&LAB_10024800);
  }
  _DAT_1002cc20 = 0;
  _DAT_1002cc18 = 9;
  _DAT_1002cc14 = &DAT_10034464;
  return &DAT_1002cc14;
}


// 10013b40 FUN_10013b40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013b40(void)

{
  char *pcVar1;
  
  if ((_DAT_100382e8 & 1) == 0) {
    _DAT_100382e8 = _DAT_100382e8 | 1;
    _DAT_100382cc = "vertexFileFixup_t";
    _DAT_100382d4 = 0;
    _DAT_100382d8 = 0;
    _DAT_100382dc = 0;
    _DAT_100382e0 = 0;
    _DAT_100382e4 = 0;
    pcVar1 = "vertexFileFixup_t";
    do {
      _DAT_100382d0 = pcVar1;
      pcVar1 = _DAT_100382d0 + 1;
    } while (*_DAT_100382d0 != '\0');
    _DAT_100382d0 = _DAT_100382d0 + -0x10026500;
    _atexit((_func_4879 *)&LAB_10024810);
  }
  _DAT_1002cc38 = 0;
  _DAT_1002cc30 = 3;
  _DAT_1002cc2c = &DAT_100346bc;
  return &DAT_1002cc2c;
}


// 10013bc0 FUN_10013bc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013bc0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038308 & 1) == 0) {
    _DAT_10038308 = _DAT_10038308 | 1;
    _DAT_100382ec = "mstudioboneweight_t";
    _DAT_100382f4 = 0;
    _DAT_100382f8 = 0;
    _DAT_100382fc = 0;
    _DAT_10038300 = 0;
    _DAT_10038304 = 0;
    pcVar1 = "mstudioboneweight_t";
    do {
      _DAT_100382f0 = pcVar1;
      pcVar1 = _DAT_100382f0 + 1;
    } while (*_DAT_100382f0 != '\0');
    _DAT_100382f0 = _DAT_100382f0 + -0x100264ec;
    _atexit((_func_4879 *)&LAB_10024820);
  }
  _DAT_1002cc50 = 0;
  _DAT_1002cc48 = 3;
  _DAT_1002cc44 = &DAT_100347ac;
  return &DAT_1002cc44;
}


// 10013c40 FUN_10013c40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013c40(void)

{
  char *pcVar1;
  
  if ((_DAT_10038328 & 1) == 0) {
    _DAT_10038328 = _DAT_10038328 | 1;
    _DAT_1003830c = "mstudiovertex_t";
    _DAT_10038314 = 0;
    _DAT_10038318 = 0;
    _DAT_1003831c = 0;
    _DAT_10038320 = 0;
    _DAT_10038324 = 0;
    pcVar1 = "mstudiovertex_t";
    do {
      _DAT_10038310 = pcVar1;
      pcVar1 = _DAT_10038310 + 1;
    } while (*_DAT_10038310 != '\0');
    _DAT_10038310 = _DAT_10038310 + -0x100264dc;
    _atexit((_func_4879 *)&LAB_10024830);
  }
  _DAT_1002cc68 = 0;
  _DAT_1002cc60 = 4;
  _DAT_1002cc5c = &DAT_1003489c;
  return &DAT_1002cc5c;
}


// 10013cc0 FUN_10013cc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013cc0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038348 & 1) == 0) {
    _DAT_10038348 = _DAT_10038348 | 1;
    _DAT_1003832c = "BoneStateChangeHeader_t";
    _DAT_10038334 = 0;
    _DAT_10038338 = 0;
    _DAT_1003833c = 0;
    _DAT_10038340 = 0;
    _DAT_10038344 = 0;
    pcVar1 = "BoneStateChangeHeader_t";
    do {
      _DAT_10038330 = pcVar1;
      pcVar1 = _DAT_10038330 + 1;
    } while (*_DAT_10038330 != '\0');
    _DAT_10038330 = _DAT_10038330 + -0x100264c4;
    _atexit((_func_4879 *)&LAB_10024840);
  }
  _DAT_1002cc80 = 0;
  _DAT_1002cc78 = 2;
  _DAT_1002cc74 = &DAT_100349cc;
  return &DAT_1002cc74;
}


// 10013d40 FUN_10013d40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013d40(void)

{
  char *pcVar1;
  
  if ((_DAT_10038368 & 1) == 0) {
    _DAT_10038368 = _DAT_10038368 | 1;
    _DAT_1003834c = "Vertex_t";
    _DAT_10038354 = 0;
    _DAT_10038358 = 0;
    _DAT_1003835c = 0;
    _DAT_10038360 = 0;
    _DAT_10038364 = 0;
    pcVar1 = "Vertex_t";
    do {
      _DAT_10038350 = pcVar1;
      pcVar1 = _DAT_10038350 + 1;
    } while (*_DAT_10038350 != '\0');
    _DAT_10038350 = _DAT_10038350 + -0x100264b8;
    _atexit((_func_4879 *)&LAB_10024850);
  }
  _DAT_1002cc98 = 0;
  _DAT_1002cc90 = 4;
  _DAT_1002cc8c = &DAT_10034a84;
  return &DAT_1002cc8c;
}


// 10013dc0 FUN_10013dc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013dc0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038388 & 1) == 0) {
    _DAT_10038388 = _DAT_10038388 | 1;
    _DAT_1003836c = "StripHeader_t";
    _DAT_10038374 = 0;
    _DAT_10038378 = 0;
    _DAT_1003837c = 0;
    _DAT_10038380 = 0;
    _DAT_10038384 = 0;
    pcVar1 = "StripHeader_t";
    do {
      _DAT_10038370 = pcVar1;
      pcVar1 = _DAT_10038370 + 1;
    } while (*_DAT_10038370 != '\0');
    _DAT_10038370 = _DAT_10038370 + -0x100264a8;
    _atexit((_func_4879 *)&LAB_10024860);
  }
  _DAT_1002ccb0 = 0;
  _DAT_1002cca8 = 8;
  _DAT_1002cca4 = &DAT_10034bb4;
  return &DAT_1002cca4;
}


// 10013e40 FUN_10013e40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013e40(void)

{
  char *pcVar1;
  
  if ((_DAT_100383a8 & 1) == 0) {
    _DAT_100383a8 = _DAT_100383a8 | 1;
    _DAT_1003838c = "StripGroupHeader_t";
    _DAT_10038394 = 0;
    _DAT_10038398 = 0;
    _DAT_1003839c = 0;
    _DAT_100383a0 = 0;
    _DAT_100383a4 = 0;
    pcVar1 = "StripGroupHeader_t";
    do {
      _DAT_10038390 = pcVar1;
      pcVar1 = _DAT_10038390 + 1;
    } while (*_DAT_10038390 != '\0');
    _DAT_10038390 = _DAT_10038390 + -0x10026494;
    _atexit((_func_4879 *)&LAB_10024870);
  }
  _DAT_1002ccc8 = 0;
  _DAT_1002ccc0 = 7;
  _DAT_1002ccbc = &DAT_10034dd4;
  return &DAT_1002ccbc;
}


// 10013ec0 FUN_10013ec0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013ec0(void)

{
  char *pcVar1;
  
  if ((_DAT_100383c8 & 1) == 0) {
    _DAT_100383c8 = _DAT_100383c8 | 1;
    _DAT_100383ac = "MeshHeader_t";
    _DAT_100383b4 = 0;
    _DAT_100383b8 = 0;
    _DAT_100383bc = 0;
    _DAT_100383c0 = 0;
    _DAT_100383c4 = 0;
    pcVar1 = "MeshHeader_t";
    do {
      _DAT_100383b0 = pcVar1;
      pcVar1 = _DAT_100383b0 + 1;
    } while (*_DAT_100383b0 != '\0');
    _DAT_100383b0 = _DAT_100383b0 + -0x10026484;
    _atexit((_func_4879 *)&LAB_10024880);
  }
  _DAT_1002cce0 = 0;
  _DAT_1002ccd8 = 3;
  _DAT_1002ccd4 = &DAT_10034fb4;
  return &DAT_1002ccd4;
}


// 10013f40 FUN_10013f40

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013f40(void)

{
  char *pcVar1;
  
  if ((_DAT_100383e8 & 1) == 0) {
    _DAT_100383e8 = _DAT_100383e8 | 1;
    _DAT_100383cc = "ModelLODHeader_t";
    _DAT_100383d4 = 0;
    _DAT_100383d8 = 0;
    _DAT_100383dc = 0;
    _DAT_100383e0 = 0;
    _DAT_100383e4 = 0;
    pcVar1 = "ModelLODHeader_t";
    do {
      _DAT_100383d0 = pcVar1;
      pcVar1 = _DAT_100383d0 + 1;
    } while (*_DAT_100383d0 != '\0');
    _DAT_100383d0 = _DAT_100383d0 + -0x10026470;
    _atexit((_func_4879 *)&LAB_10024890);
  }
  _DAT_1002ccf8 = 0;
  _DAT_1002ccf0 = 3;
  _DAT_1002ccec = &DAT_100350a4;
  return &DAT_1002ccec;
}


// 10013fc0 FUN_10013fc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10013fc0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038408 & 1) == 0) {
    _DAT_10038408 = _DAT_10038408 | 1;
    _DAT_100383ec = "ModelHeader_t";
    _DAT_100383f4 = 0;
    _DAT_100383f8 = 0;
    _DAT_100383fc = 0;
    _DAT_10038400 = 0;
    _DAT_10038404 = 0;
    pcVar1 = "ModelHeader_t";
    do {
      _DAT_100383f0 = pcVar1;
      pcVar1 = _DAT_100383f0 + 1;
    } while (*_DAT_100383f0 != '\0');
    _DAT_100383f0 = _DAT_100383f0 + -0x10026460;
    _atexit((_func_4879 *)&LAB_100248a0);
  }
  _DAT_1002cd10 = 0;
  _DAT_1002cd08 = 2;
  _DAT_1002cd04 = &DAT_10035194;
  return &DAT_1002cd04;
}


// 10014040 FUN_10014040

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10014040(void)

{
  char *pcVar1;
  
  if ((_DAT_10038428 & 1) == 0) {
    _DAT_10038428 = _DAT_10038428 | 1;
    _DAT_1003840c = "BodyPartHeader_t";
    _DAT_10038414 = 0;
    _DAT_10038418 = 0;
    _DAT_1003841c = 0;
    _DAT_10038420 = 0;
    _DAT_10038424 = 0;
    pcVar1 = "BodyPartHeader_t";
    do {
      _DAT_10038410 = pcVar1;
      pcVar1 = _DAT_10038410 + 1;
    } while (*_DAT_10038410 != '\0');
    _DAT_10038410 = _DAT_10038410 + -0x1002644c;
    _atexit((_func_4879 *)&LAB_100248b0);
  }
  _DAT_1002cd28 = 0;
  _DAT_1002cd20 = 2;
  _DAT_1002cd1c = &DAT_1003524c;
  return &DAT_1002cd1c;
}


// 100140c0 FUN_100140c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100140c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038448 & 1) == 0) {
    _DAT_10038448 = _DAT_10038448 | 1;
    _DAT_1003842c = "MaterialReplacementHeader_t";
    _DAT_10038434 = 0;
    _DAT_10038438 = 0;
    _DAT_1003843c = 0;
    _DAT_10038440 = 0;
    _DAT_10038444 = 0;
    pcVar1 = "MaterialReplacementHeader_t";
    do {
      _DAT_10038430 = pcVar1;
      pcVar1 = _DAT_10038430 + 1;
    } while (*_DAT_10038430 != '\0');
    _DAT_10038430 = _DAT_10038430 + -0x10026430;
    _atexit((_func_4879 *)&LAB_100248c0);
  }
  _DAT_1002cd40 = 0;
  _DAT_1002cd38 = 2;
  _DAT_1002cd34 = &DAT_10035304;
  return &DAT_1002cd34;
}


// 10014140 FUN_10014140

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10014140(void)

{
  char *pcVar1;
  
  if ((_DAT_10038468 & 1) == 0) {
    _DAT_10038468 = _DAT_10038468 | 1;
    _DAT_1003844c = "MaterialReplacementListHeader_t";
    _DAT_10038454 = 0;
    _DAT_10038458 = 0;
    _DAT_1003845c = 0;
    _DAT_10038460 = 0;
    _DAT_10038464 = 0;
    pcVar1 = "MaterialReplacementListHeader_t";
    do {
      _DAT_10038450 = pcVar1;
      pcVar1 = _DAT_10038450 + 1;
    } while (*_DAT_10038450 != '\0');
    _DAT_10038450 = _DAT_10038450 + -0x10026410;
    _atexit((_func_4879 *)&LAB_100248d0);
  }
  _DAT_1002cd58 = 0;
  _DAT_1002cd50 = 2;
  _DAT_1002cd4c = &DAT_100353bc;
  return &DAT_1002cd4c;
}


// 100141c0 FUN_100141c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100141c0(void)

{
  char *pcVar1;
  
  if ((_DAT_10038488 & 1) == 0) {
    _DAT_10038488 = _DAT_10038488 | 1;
    _DAT_1003846c = "FileHeader_t";
    _DAT_10038474 = 0;
    _DAT_10038478 = 0;
    _DAT_1003847c = 0;
    _DAT_10038480 = 0;
    _DAT_10038484 = 0;
    pcVar1 = "FileHeader_t";
    do {
      _DAT_10038470 = pcVar1;
      pcVar1 = _DAT_10038470 + 1;
    } while (*_DAT_10038470 != '\0');
    _DAT_10038470 = _DAT_10038470 + -0x10026400;
    _atexit((_func_4879 *)&LAB_100248e0);
  }
  _DAT_1002cd70 = 0;
  _DAT_1002cd68 = 10;
  _DAT_1002cd64 = &DAT_10035474;
  return &DAT_1002cd64;
}


// 10014240 FUN_10014240

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_10014240(void)

{
  char *pcVar1;
  
  if ((_DAT_100384a8 & 1) == 0) {
    _DAT_100384a8 = _DAT_100384a8 | 1;
    _DAT_1003848c = "phyheader_t";
    _DAT_10038494 = 0;
    _DAT_10038498 = 0;
    _DAT_1003849c = 0;
    _DAT_100384a0 = 0;
    _DAT_100384a4 = 0;
    pcVar1 = "phyheader_t";
    do {
      _DAT_10038490 = pcVar1;
      pcVar1 = _DAT_10038490 + 1;
    } while (*_DAT_10038490 != '\0');
    _DAT_10038490 = _DAT_10038490 + -0x100263f4;
    _atexit((_func_4879 *)&LAB_100248f0);
  }
  _DAT_1002cd88 = 0;
  _DAT_1002cd80 = 4;
  _DAT_1002cd7c = &DAT_1003570c;
  return &DAT_1002cd7c;
}


// 100142c0 FUN_100142c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100142c0(int param_1,int param_2)

{
  int iVar1;
  
  iVar1 = 0;
  if (0 < param_2) {
    do {
      if (DAT_100384ac == 0) {
        DAT_100384ac = (**(code **)(param_1 + iVar1 * 4))("VStudioRender026",0);
        _DAT_100384b0 = DAT_100384ac;
      }
      if (DAT_100384c4 == 0) {
        DAT_100384c4 = (**(code **)(param_1 + iVar1 * 4))("VGUI_ivgui008",0);
      }
      if (DAT_100384b8 == 0) {
        DAT_100384b8 = (**(code **)(param_1 + iVar1 * 4))("VGUI_Input005",0);
      }
      if (DAT_100384c0 == 0) {
        DAT_100384c0 = (**(code **)(param_1 + iVar1 * 4))("VGUI_Panel009",0);
      }
      if (DAT_100384bc == 0) {
        DAT_100384bc = (**(code **)(param_1 + iVar1 * 4))("VGUI_Surface030",0);
      }
      if (DAT_100384cc == 0) {
        DAT_100384cc = (**(code **)(param_1 + iVar1 * 4))("VGUI_Scheme010",0);
      }
      if (DAT_100384d0 == 0) {
        DAT_100384d0 = (**(code **)(param_1 + iVar1 * 4))("VGUI_System010",0);
      }
      if (DAT_100384c8 == 0) {
        DAT_100384c8 = (**(code **)(param_1 + iVar1 * 4))("VGUI_Localize005",0);
      }
      if (DAT_100384b4 == 0) {
        DAT_100384b4 = (**(code **)(param_1 + iVar1 * 4))("MatSystemSurface006",0);
      }
      if (DAT_100384d4 == 0) {
        DAT_100384d4 = (**(code **)(param_1 + iVar1 * 4))("VDataCache003",0);
      }
      if (DAT_100384d8 == 0) {
        DAT_100384d8 = (**(code **)(param_1 + iVar1 * 4))("MDLCache004",0);
        _DAT_100384dc = DAT_100384d8;
      }
      if (DAT_100384e0 == 0) {
        DAT_100384e0 = (**(code **)(param_1 + iVar1 * 4))("VAvi001",0);
      }
      if (DAT_100384e4 == 0) {
        DAT_100384e4 = (**(code **)(param_1 + iVar1 * 4))("VBik001",0);
      }
      if (DAT_100384e8 == 0) {
        DAT_100384e8 = (**(code **)(param_1 + iVar1 * 4))("IQuickTime001",0);
      }
      if (DAT_100384f0 == 0) {
        DAT_100384f0 = (**(code **)(param_1 + iVar1 * 4))("VPhysicsCollision007",0);
      }
      if (DAT_100384f4 == 0) {
        DAT_100384f4 = (**(code **)(param_1 + iVar1 * 4))("VSoundEmitter003",0);
      }
      iVar1 = iVar1 + 1;
    } while (iVar1 < param_2);
  }
  return;
}


// 100144c0 FUN_100144c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_100144c0(void)

{
  DAT_100384ac = 0;
  _DAT_100384b0 = 0;
  DAT_100384c4 = 0;
  DAT_100384b8 = 0;
  DAT_100384c0 = 0;
  DAT_100384bc = 0;
  DAT_100384c8 = 0;
  DAT_100384cc = 0;
  DAT_100384d0 = 0;
  DAT_100384b4 = 0;
  DAT_100384d4 = 0;
  DAT_100384d8 = 0;
  _DAT_100384dc = 0;
  DAT_100384e0 = 0;
  DAT_100384e4 = 0;
  DAT_100384e8 = 0;
  DAT_100384f0 = 0;
  _DAT_100384ec = 0;
  DAT_100384f4 = 0;
  return;
}


// 10014530 FUN_10014530

void __cdecl FUN_10014530(int param_1,int param_2)

{
  int iVar1;
  
  iVar1 = 0;
  if (0 < param_2) {
    do {
      if (DAT_10038510 == 0) {
        DAT_10038510 = (**(code **)(param_1 + iVar1 * 4))("VFileSystem018",0);
      }
      if (DAT_10038514 == 0) {
        DAT_10038514 = (**(code **)(param_1 + iVar1 * 4))("VMaterialSystem080",0);
        DAT_10038518 = DAT_10038514;
      }
      if (DAT_1003851c == 0) {
        DAT_1003851c = (**(code **)(param_1 + iVar1 * 4))("InputSystemVersion001",0);
      }
      if (DAT_10038520 == 0) {
        DAT_10038520 = (**(code **)(param_1 + iVar1 * 4))("NetworkSystemVersion001",0);
      }
      if (DAT_10038524 == 0) {
        DAT_10038524 = (**(code **)(param_1 + iVar1 * 4))("MaterialSystemHardwareConfig013",0);
      }
      if (DAT_10038528 == 0) {
        DAT_10038528 = (**(code **)(param_1 + iVar1 * 4))("DebugTextureInfo001",0);
      }
      if (DAT_1003852c == 0) {
        DAT_1003852c = (**(code **)(param_1 + iVar1 * 4))("VBAllocTracker001",0);
      }
      if (DAT_10038530 == 0) {
        DAT_10038530 = (**(code **)(param_1 + iVar1 * 4))("COLORCORRECTION_VERSION_1",0);
      }
      if (DAT_10038534 == 0) {
        DAT_10038534 = (**(code **)(param_1 + iVar1 * 4))("VP4001",0);
      }
      if (DAT_10038538 == 0) {
        DAT_10038538 = (**(code **)(param_1 + iVar1 * 4))("VMDLLIB001",0);
      }
      if (DAT_1003853c == 0) {
        DAT_1003853c = (**(code **)(param_1 + iVar1 * 4))("QueuedLoaderVersion001",0);
      }
      if (DAT_10038540 == 0) {
        DAT_10038540 = (**(code **)(param_1 + iVar1 * 4))("VResourceAccessControl001",0);
      }
      if (DAT_10038544 == 0) {
        DAT_10038544 = (**(code **)(param_1 + iVar1 * 4))("MATCHFRAMEWORK_001",0);
      }
      iVar1 = iVar1 + 1;
    } while (iVar1 < param_2);
  }
  return;
}


// 100146e0 FUN_100146e0

void FUN_100146e0(void)

{
  DAT_10038510 = 0;
  DAT_10038518 = 0;
  DAT_10038514 = 0;
  DAT_10038524 = 0;
  DAT_10038528 = 0;
  DAT_1003851c = 0;
  DAT_10038520 = 0;
  DAT_10038530 = 0;
  DAT_10038534 = 0;
  DAT_10038538 = 0;
  DAT_1003853c = 0;
  DAT_10038540 = 0;
  DAT_10038544 = 0;
  return;
}


// 10014730 FUN_10014730

void __thiscall FUN_10014730(void *this,undefined4 param_1,int param_2,int param_3)

{
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 8) = *(int *)((int)this + 8) + (param_3 - param_2);
  (**(code **)(*this + 8))();
  return;
}


// 10014760 FUN_10014760

void __thiscall FUN_10014760(void *this,undefined4 param_1)

{
  *(undefined4 *)((int)this + 4) = param_1;
  return;
}


// 10014770 FUN_10014770

undefined4 __fastcall FUN_10014770(int param_1)

{
  return *(undefined4 *)(param_1 + 4);
}


// 10014780 FUN_10014780

undefined4 __fastcall FUN_10014780(int param_1)

{
  return *(undefined4 *)(param_1 + 8);
}


// 10014790 FUN_10014790

void __thiscall FUN_10014790(void *this,int param_1)

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
      iVar2 = 3;
    }
    if ((int)uVar4 <= iVar2) goto LAB_100147eb;
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
LAB_100147eb:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 * 0xc);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 10014830 FUN_10014830

void __thiscall FUN_10014830(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1001488b;
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
LAB_1001488b:
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


// 100148c0 FUN_100148c0

void __thiscall FUN_100148c0(void *this,int param_1)

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
        uVar2 = 4;
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
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(uVar2 * 10);
    *(undefined4 *)this = uVar3;
  }
  return;
}


// 10014970 FUN_10014970

undefined4 __thiscall FUN_10014970(void *this,int param_1)

{
  undefined4 uVar1;
  int iVar2;
  short sVar3;
  uint uVar4;
  int iVar5;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar4 = param_1 - 1U & 0xffff;
  if ((int)uVar4 < *(int *)((int)this + 0x3c)) {
    iVar2 = uVar4 * 0xc;
    iVar5 = *(int *)((int)this + 0xc) + iVar2;
    sVar3 = (short)(param_1 - 1U);
    if ((((*(short *)(iVar5 + 8) != sVar3) || (*(short *)(iVar5 + 10) == sVar3)) &&
        (iVar2 = iVar2 + *(int *)((int)this + 0xc),
        *(short *)(iVar2 + 2) == (short)((uint)param_1 >> 0x10))) && (sVar3 != -1)) {
      uVar1 = *(undefined4 *)(iVar2 + 4);
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 8))();
      return uVar1;
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return 0;
}


// 100149e0 FUN_100149e0

uint __thiscall FUN_100149e0(void *this,uint param_1)

{
  return (uint)*(ushort *)(*(int *)((int)this + 0xc) + 2 + (param_1 & 0xffff) * 0xc) << 0x10 |
         param_1 + 1 & 0xffff;
}


// 10014a00 FUN_10014a00

void __fastcall FUN_10014a00(int *param_1)

{
  uint uVar1;
  int iVar2;
  ushort uVar3;
  undefined2 uVar4;
  int iVar5;
  
  uVar1 = param_1[0xc];
  if (uVar1 != 0) {
    uVar4 = 0xffff;
    while (uVar1 = uVar1 - 1, -1 < (int)uVar1) {
      iVar5 = (uVar1 & 0xffff) * 0xc;
      *(undefined2 *)(iVar5 + 10 + *param_1) = uVar4;
      uVar4 = (undefined2)uVar1;
      *(undefined2 *)(iVar5 + 8 + *param_1) = uVar4;
    }
    uVar3 = *(ushort *)(param_1 + 6);
    *(undefined2 *)(param_1 + 0xb) = 0;
    if (uVar3 != 0xffff) {
      iVar5 = param_1[3];
      do {
        iVar2 = (uint)uVar3 * 10;
        *(undefined2 *)(iVar5 + iVar2) = 0xffff;
        *(undefined2 *)(param_1[3] + 2 + iVar2) = 0xffff;
        *(undefined2 *)(param_1[3] + 4 + iVar2) = 0;
        iVar5 = param_1[3];
        uVar3 = *(ushort *)(iVar5 + 8 + iVar2);
      } while (uVar3 != 0xffff);
    }
    *(undefined2 *)((int)param_1 + 0x2e) = 0;
  }
  return;
}


// 10014aa0 FUN_10014aa0

uint __fastcall FUN_10014aa0(int *param_1)

{
  ushort uVar1;
  int iVar2;
  uint uVar3;
  undefined2 extraout_var;
  undefined2 extraout_var_00;
  undefined2 uVar4;
  uint uVar5;
  undefined2 *puVar6;
  
  uVar1 = *(ushort *)(param_1 + 0xb);
  if (uVar1 == 0xffff) {
    uVar5 = param_1[0xc];
    uVar3 = uVar5 & 0xffff;
    if ((uVar3 != uVar5) || ((short)uVar5 == -1)) {
      if (DAT_100385b4 < 10) {
        uVar3 = Warning("CUtlMultiList overflow! (exhausted index range)\n");
        DAT_100385b4 = DAT_100385b4 + 1;
      }
      return CONCAT22((short)(uVar3 >> 0x10),0xffff);
    }
    if (uVar5 == param_1[1]) {
      FUN_10014790(param_1,1);
      param_1[0xd] = *param_1;
      uVar5 = param_1[0xc];
      if (param_1[1] <= (int)uVar5) {
        uVar4 = extraout_var;
        if (DAT_100385b0 < 10) {
          Warning("CUtlMultiList overflow! (exhausted memory allocator)\n");
          DAT_100385b0 = DAT_100385b0 + 1;
          uVar4 = extraout_var_00;
        }
        return CONCAT22(uVar4,0xffff);
      }
    }
    uVar1 = *(ushort *)(param_1 + 0xc);
    param_1[0xc] = uVar5 + 1;
  }
  else {
    *(undefined2 *)(param_1 + 0xb) = *(undefined2 *)(*param_1 + 10 + (uint)uVar1 * 0xc);
  }
  iVar2 = (uint)uVar1 * 0xc;
  *(ushort *)(iVar2 + 8 + *param_1) = uVar1;
  *(ushort *)(iVar2 + 10 + *param_1) = uVar1;
  *(short *)((int)param_1 + 0x2e) = *(short *)((int)param_1 + 0x2e) + 1;
  puVar6 = (undefined2 *)(*param_1 + iVar2);
  if (puVar6 != (undefined2 *)0x0) {
    *puVar6 = 0;
    puVar6[1] = 1;
    *(undefined4 *)(puVar6 + 2) = 0;
  }
  return (uint)uVar1;
}


// 10014b80 FUN_10014b80

void __fastcall FUN_10014b80(int *param_1)

{
  int iVar1;
  ushort uVar2;
  ushort uVar3;
  
  if (*(short *)((int)param_1 + 0x16) != -1) {
    uVar3 = *(ushort *)(param_1 + 3);
    while (uVar3 != 0xffff) {
      iVar1 = *param_1 + (uint)uVar3 * 10;
      uVar2 = *(ushort *)(iVar1 + 8);
      *(ushort *)(iVar1 + 6) = uVar3;
      uVar3 = uVar2;
      if (uVar2 == 0xffff) {
        uVar3 = *(ushort *)(param_1 + 4);
      }
      *(ushort *)(iVar1 + 8) = uVar3;
      uVar3 = uVar2;
    }
    if ((short)param_1[3] != -1) {
      *(short *)(param_1 + 4) = (short)param_1[3];
    }
    *(undefined2 *)(param_1 + 3) = 0xffff;
    *(undefined2 *)((int)param_1 + 0xe) = 0xffff;
    *(undefined2 *)((int)param_1 + 0x12) = 0;
  }
  return;
}


// 10014bf0 FUN_10014bf0

uint __thiscall FUN_10014bf0(void *this,char param_1)

{
  ushort *puVar1;
  ushort uVar2;
  undefined2 uVar5;
  ushort *puVar3;
  undefined2 extraout_var;
  undefined2 extraout_var_00;
  uint uVar4;
  int iVar6;
  ushort local_2;
  
  uVar2 = *(ushort *)((int)this + 0x10);
  if (uVar2 == 0xffff) {
    uVar2 = *(ushort *)((int)this + 0x16);
    puVar1 = (ushort *)((int)this + 0x16);
    if (((uVar2 != 0) && (uVar2 == 0)) ||
       (iVar6 = *(int *)((int)this + 4), iVar6 <= (int)(uint)uVar2)) {
      iVar6 = *(int *)((int)this + 4);
      uVar2 = (0 < iVar6) - 1;
    }
    else {
      uVar2 = *puVar1 + 1;
      if (((uVar2 != 0) && (uVar2 == 0)) || (iVar6 <= (int)(uint)uVar2)) {
        uVar2 = 0xffff;
      }
      else {
        uVar2 = *puVar1 + 1;
      }
    }
    uVar5 = (undefined2)((uint)&local_2 >> 0x10);
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar6 <= (int)(uint)uVar2)) {
      FUN_100148c0(this,1);
                    /* WARNING: Load size is inaccurate */
      *(undefined4 *)((int)this + 0x18) = *this;
      uVar2 = *puVar1;
      if (((uVar2 != 0) && (uVar2 == 0)) ||
         (iVar6 = *(int *)((int)this + 4), iVar6 <= (int)(uint)uVar2)) {
        iVar6 = *(int *)((int)this + 4);
        puVar3 = &local_2;
      }
      else {
        puVar3 = (ushort *)FUN_10005870(this,(short *)&local_2,(short *)puVar1);
      }
      uVar2 = *puVar3;
      uVar5 = (undefined2)((uint)puVar3 >> 0x10);
      if (((uVar2 != 0) && (uVar2 == 0)) || (iVar6 <= (int)(uint)uVar2)) {
        if (DAT_100385bc < 10) {
          Warning("CUtlLinkedList overflow! (exhausted memory allocator)\n");
          DAT_100385bc = DAT_100385bc + 1;
          uVar5 = extraout_var;
        }
        return CONCAT22(uVar5,0xffff);
      }
    }
    if (uVar2 == 0xffff) {
      if (DAT_100385b8 < 10) {
        Warning("CUtlLinkedList overflow! (exhausted index range)\n");
        DAT_100385b8 = DAT_100385b8 + 1;
        uVar5 = extraout_var_00;
      }
      return CONCAT22(uVar5,0xffff);
    }
    *(short *)((int)this + 0x14) = *(short *)((int)this + 0x14) + 1;
    *puVar1 = uVar2;
  }
  else {
                    /* WARNING: Load size is inaccurate */
    *(undefined2 *)((int)this + 0x10) = *(undefined2 *)(*this + 8 + (uint)uVar2 * 10);
  }
  uVar4 = (uint)uVar2;
                    /* WARNING: Load size is inaccurate */
  iVar6 = uVar4 * 10;
  if (param_1 == '\0') {
    *(ushort *)(iVar6 + 8 + *this) = uVar2;
                    /* WARNING: Load size is inaccurate */
    *(ushort *)(iVar6 + 6 + *this) = uVar2;
    return uVar4;
  }
  *(undefined2 *)(iVar6 + 8 + *this) = 0xffff;
                    /* WARNING: Load size is inaccurate */
  *(undefined2 *)(iVar6 + 6 + *this) = 0xffff;
  return uVar4;
}


// 10014d90 FUN_10014d90

void __thiscall FUN_10014d90(void *this,ushort param_1)

{
  int iVar1;
  ushort uVar2;
  ushort uVar3;
  int iVar4;
  uint uVar5;
  
  if ((((param_1 == 0) || (param_1 != 0)) &&
      (uVar5 = (uint)param_1, (int)uVar5 < *(int *)((int)this + 4))) &&
     (param_1 <= *(ushort *)((int)this + 0x16))) {
                    /* WARNING: Load size is inaccurate */
    iVar4 = *this;
    iVar1 = iVar4 + uVar5 * 10;
    if (*(ushort *)(iVar4 + 6 + uVar5 * 10) != param_1) {
      uVar2 = *(ushort *)(iVar1 + 8);
      uVar3 = *(ushort *)(iVar1 + 6);
      if (uVar3 == 0xffff) {
        *(ushort *)((int)this + 0xc) = uVar2;
      }
      else {
        *(ushort *)(iVar4 + 8 + (uint)uVar3 * 10) = uVar2;
      }
      if (uVar2 != 0xffff) {
                    /* WARNING: Load size is inaccurate */
        *(ushort *)(*this + 6 + (uint)uVar2 * 10) = uVar3;
        *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + -1;
        *(ushort *)(iVar1 + 8) = param_1;
        *(ushort *)(iVar1 + 6) = param_1;
        return;
      }
      *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + -1;
      *(ushort *)((int)this + 0xe) = uVar3;
      *(ushort *)(iVar1 + 8) = param_1;
      *(ushort *)(iVar1 + 6) = param_1;
    }
  }
  return;
}


// 10014e30 FUN_10014e30

uint __thiscall FUN_10014e30(void *this,uint param_1,undefined4 param_2,int param_3)

{
  ushort uVar1;
  int iVar2;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  iVar2 = (param_1 & 0xffff) * 0xc;
  *(undefined4 *)(*(int *)((int)this + 0xc) + 4 + iVar2) = param_2;
  *(int *)((int)this + 8) = *(int *)((int)this + 8) + param_3;
  uVar1 = *(ushort *)(*(int *)((int)this + 0xc) + 2 + iVar2);
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return param_1 + 1 & 0xffff | (uint)uVar1 << 0x10;
}


// 10014e80 FUN_10014e80

uint __thiscall FUN_10014e80(void *this,ushort param_1,ushort param_2)

{
  short *psVar1;
  int iVar2;
  uint uVar3;
  uint uVar4;
  
  uVar3 = (uint)param_2;
  uVar4 = uVar3;
  if ((int)uVar3 < *(int *)((int)this + 0x30)) {
                    /* WARNING: Load size is inaccurate */
    iVar2 = *this;
    uVar4 = iVar2 + uVar3 * 0xc;
    if (*(ushort *)(iVar2 + 8 + uVar3 * 0xc) != param_2) {
      if (*(ushort *)(uVar4 + 8) == 0xffff) {
        *(undefined2 *)(*(int *)((int)this + 0xc) + (uint)param_1 * 10) =
             *(undefined2 *)(uVar4 + 10);
      }
      else {
        *(undefined2 *)(iVar2 + 10 + (uint)*(ushort *)(uVar4 + 8) * 0xc) =
             *(undefined2 *)(uVar4 + 10);
      }
      if (*(ushort *)(uVar4 + 10) == 0xffff) {
        *(undefined2 *)(*(int *)((int)this + 0xc) + 2 + (uint)param_1 * 10) =
             *(undefined2 *)(uVar4 + 8);
      }
      else {
                    /* WARNING: Load size is inaccurate */
        *(undefined2 *)(*this + 8 + (uint)*(ushort *)(uVar4 + 10) * 0xc) =
             *(undefined2 *)(uVar4 + 8);
      }
      *(ushort *)(uVar4 + 10) = param_2;
      *(ushort *)(uVar4 + 8) = param_2;
      iVar2 = *(int *)((int)this + 0xc);
      psVar1 = (short *)(iVar2 + 4 + (uint)param_1 * 10);
      *psVar1 = *psVar1 + -1;
      uVar4 = iVar2 + 4 + (uint)param_1 * 10;
      if (*(int *)((int)this + 0x28) != 0) {
        *(undefined2 *)(*(int *)((int)this + 0x28) + uVar3 * 2) = 0xffff;
      }
    }
  }
  return uVar4;
}


// 10014f40 FUN_10014f40

int __thiscall FUN_10014f40(void *this,ushort param_1,ushort param_2,ushort param_3)

{
  int iVar1;
  short *psVar2;
  int iVar3;
  
  FUN_10014e80(this,param_1,param_3);
                    /* WARNING: Load size is inaccurate */
  iVar1 = *this + (uint)param_3 * 0xc;
  *(ushort *)(iVar1 + 10) = param_2;
  if (param_2 == 0xffff) {
    *(undefined2 *)(iVar1 + 8) = *(undefined2 *)((uint)param_1 * 10 + 2 + *(int *)((int)this + 0xc))
    ;
    *(ushort *)((uint)param_1 * 10 + 2 + *(int *)((int)this + 0xc)) = param_3;
  }
  else {
                    /* WARNING: Load size is inaccurate */
    iVar3 = *this + (uint)param_2 * 0xc;
    *(undefined2 *)(iVar1 + 8) = *(undefined2 *)(iVar3 + 8);
    *(ushort *)(iVar3 + 8) = param_3;
  }
  if (*(ushort *)(iVar1 + 8) == 0xffff) {
    *(ushort *)(*(int *)((int)this + 0xc) + (uint)param_1 * 10) = param_3;
  }
  else {
                    /* WARNING: Load size is inaccurate */
    *(ushort *)(*this + 10 + (uint)*(ushort *)(iVar1 + 8) * 0xc) = param_3;
  }
  iVar1 = *(int *)((int)this + 0xc);
  psVar2 = (short *)(iVar1 + 4 + (uint)param_1 * 10);
  *psVar2 = *psVar2 + 1;
  if (*(int *)((int)this + 0x28) != 0) {
    *(ushort *)(*(int *)((int)this + 0x28) + (uint)param_3 * 2) = param_1;
  }
  return iVar1 + 4 + (uint)param_1 * 10;
}


// 10014ff0 FUN_10014ff0

int __thiscall FUN_10014ff0(void *this,ushort param_1,ushort param_2,ushort param_3)

{
  int iVar1;
  short *psVar2;
  int iVar3;
  
  FUN_10014e80(this,param_1,param_3);
                    /* WARNING: Load size is inaccurate */
  iVar1 = *this + (uint)param_3 * 0xc;
  *(ushort *)(iVar1 + 8) = param_2;
  if (param_2 == 0xffff) {
    *(undefined2 *)(iVar1 + 10) = *(undefined2 *)((uint)param_1 * 10 + *(int *)((int)this + 0xc));
    *(ushort *)((uint)param_1 * 10 + *(int *)((int)this + 0xc)) = param_3;
  }
  else {
                    /* WARNING: Load size is inaccurate */
    iVar3 = *this + (uint)param_2 * 0xc;
    *(undefined2 *)(iVar1 + 10) = *(undefined2 *)(iVar3 + 10);
    *(ushort *)(iVar3 + 10) = param_3;
  }
  if (*(ushort *)(iVar1 + 10) == 0xffff) {
    *(ushort *)(*(int *)((int)this + 0xc) + 2 + (uint)param_1 * 10) = param_3;
  }
  else {
                    /* WARNING: Load size is inaccurate */
    *(ushort *)(*this + 8 + (uint)*(ushort *)(iVar1 + 10) * 0xc) = param_3;
  }
  iVar1 = *(int *)((int)this + 0xc);
  psVar2 = (short *)(iVar1 + 4 + (uint)param_1 * 10);
  *psVar2 = *psVar2 + 1;
  if (*(int *)((int)this + 0x28) != 0) {
    *(ushort *)(*(int *)((int)this + 0x28) + (uint)param_3 * 2) = param_1;
  }
  return iVar1 + 4 + (uint)param_1 * 10;
}


// 100150a0 FUN_100150a0

void __fastcall FUN_100150a0(int *param_1)

{
  int *piVar1;
  
  FUN_10014a00(param_1);
  if (param_1[10] != 0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1[10]);
  }
  piVar1 = param_1 + 3;
  FUN_10014b80(piVar1);
  if (-1 < param_1[5]) {
    if (*piVar1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar1);
      *piVar1 = 0;
    }
    param_1[4] = 0;
  }
  if (-1 < param_1[2]) {
    if (*param_1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
      *param_1 = 0;
    }
    param_1[1] = 0;
  }
  return;
}


// 10015120 FUN_10015120

uint __thiscall FUN_10015120(void *this,ushort param_1,ushort param_2)

{
  undefined2 *puVar1;
  uint uVar2;
  ushort uVar3;
  
  uVar2 = FUN_10014aa0((int *)this);
  uVar3 = (ushort)uVar2;
  if (uVar3 == 0xffff) {
    return uVar2;
  }
  FUN_10014f40(this,param_1,param_2,uVar3);
                    /* WARNING: Load size is inaccurate */
  puVar1 = (undefined2 *)(*this + (uVar2 & 0xffff) * 0xc);
  if (puVar1 != (undefined2 *)0x0) {
    *puVar1 = 0;
    puVar1[1] = 1;
    *(undefined4 *)(puVar1 + 2) = 0;
  }
  return CONCAT22((short)((uint)puVar1 >> 0x10),uVar3);
}


// 10015180 FUN_10015180

void __fastcall FUN_10015180(undefined4 *param_1)

{
  *param_1 = &PTR_FUN_10027ff0;
  FUN_100150a0(param_1 + 3);
  return;
}


// 10015190 FUN_10015190

void FUN_10015190(void)

{
  return;
}


// 100151a0 FUN_100151a0

undefined4 __thiscall FUN_100151a0(void *this,int param_1)

{
  int *this_00;
  undefined4 uVar1;
  int iVar2;
  ushort uVar3;
  uint uVar4;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar4 = param_1 - 1U & 0xffff;
  this_00 = (int *)((int)this + 0xc);
  if ((int)uVar4 < *(int *)((int)this + 0x3c)) {
    iVar2 = uVar4 * 0xc;
    uVar3 = (ushort)(param_1 - 1U);
    if ((((*(ushort *)(*this_00 + iVar2 + 8) != uVar3) ||
         (*(ushort *)(*this_00 + iVar2 + 10) == uVar3)) &&
        (*(short *)(*this_00 + 2 + iVar2) == (short)((uint)param_1 >> 0x10))) && (uVar3 != 0xffff))
    {
      if (*(short *)(*this_00 + iVar2) == 0) {
        FUN_10014e80(this_00,*(ushort *)((int)this + 0x44),uVar3);
        FUN_10014f40(this_00,*(ushort *)((int)this + 0x46),0xffff,uVar3);
      }
      *(short *)(*this_00 + iVar2) = *(short *)(*this_00 + iVar2) + 1;
                    /* WARNING: Load size is inaccurate */
      uVar1 = *(undefined4 *)(*this_00 + 4 + iVar2);
      (**(code **)(*this + 8))();
      return uVar1;
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return 0;
}


// 10015250 FUN_10015250

undefined2 __thiscall FUN_10015250(void *this,int param_1)

{
  int *this_00;
  undefined2 uVar1;
  int iVar2;
  int iVar3;
  ushort uVar4;
  uint uVar5;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar5 = param_1 - 1U & 0xffff;
  this_00 = (int *)((int)this + 0xc);
  if ((int)uVar5 < *(int *)((int)this + 0x3c)) {
    iVar3 = uVar5 * 0xc;
    uVar4 = (ushort)(param_1 - 1U);
    if ((*(ushort *)(*this_00 + iVar3 + 8) != uVar4) ||
       (*(ushort *)(*this_00 + iVar3 + 10) == uVar4)) {
      iVar2 = *this_00;
      if ((*(short *)(iVar2 + 2 + iVar3) == (short)((uint)param_1 >> 0x10)) && (uVar4 != 0xffff)) {
        if ((*(short *)(iVar2 + iVar3) != 0) &&
           (*(short *)(iVar2 + iVar3) = *(short *)(iVar2 + iVar3) + -1,
           *(short *)(iVar3 + *this_00) == 0)) {
          FUN_10014e80(this_00,*(ushort *)((int)this + 0x46),uVar4);
          FUN_10014f40(this_00,*(ushort *)((int)this + 0x44),0xffff,uVar4);
        }
                    /* WARNING: Load size is inaccurate */
        uVar1 = *(undefined2 *)(*this_00 + iVar3);
        (**(code **)(*this + 8))();
        return uVar1;
      }
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return 0;
}


// 10015310 FUN_10015310

void __thiscall FUN_10015310(void *this,int param_1)

{
  int *this_00;
  int iVar1;
  short *psVar2;
  int iVar3;
  ushort uVar4;
  uint uVar5;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar5 = param_1 - 1U & 0xffff;
  this_00 = (int *)((int)this + 0xc);
  if ((int)uVar5 < *(int *)((int)this + 0x3c)) {
    iVar1 = uVar5 * 0xc;
    iVar3 = *this_00 + iVar1;
    uVar4 = (ushort)(param_1 - 1U);
    if ((((*(ushort *)(iVar3 + 8) != uVar4) || (*(ushort *)(iVar3 + 10) == uVar4)) &&
        (psVar2 = (short *)(iVar1 + *this_00), psVar2[1] == (short)((uint)param_1 >> 0x10))) &&
       ((uVar4 != 0xffff && (*psVar2 == 0)))) {
      FUN_10014e80(this_00,*(ushort *)((int)this + 0x44),uVar4);
      FUN_10014ff0(this_00,*(ushort *)((int)this + 0x44),0xffff,uVar4);
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return;
}


// 100153a0 FUN_100153a0

undefined2 __thiscall FUN_100153a0(void *this,int param_1)

{
  int *this_00;
  undefined2 uVar1;
  int iVar2;
  ushort uVar3;
  uint uVar4;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar4 = param_1 - 1U & 0xffff;
  this_00 = (int *)((int)this + 0xc);
  if ((int)uVar4 < *(int *)((int)this + 0x3c)) {
    iVar2 = uVar4 * 0xc;
    uVar3 = (ushort)(param_1 - 1U);
    if ((((*(ushort *)(*this_00 + iVar2 + 8) != uVar3) ||
         (*(ushort *)(*this_00 + iVar2 + 10) == uVar3)) &&
        (((short *)(*this_00 + iVar2))[1] == (short)((uint)param_1 >> 0x10))) &&
       ((uVar3 != 0xffff && (*(short *)(*this_00 + iVar2) != 0)))) {
      uVar1 = *(undefined2 *)(iVar2 + *this_00);
      *(undefined2 *)(iVar2 + *this_00) = 0;
      FUN_10014e80(this_00,*(ushort *)((int)this + 0x46),uVar3);
      FUN_10014f40(this_00,*(ushort *)((int)this + 0x44),0xffff,uVar3);
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 8))();
      return uVar1;
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return 0;
}


// 10015450 FUN_10015450

void __thiscall FUN_10015450(void *this,ushort param_1)

{
  if ((param_1 != 0xffff) && (*(short *)(*(int *)((int)this + 0xc) + (uint)param_1 * 0xc) == 0)) {
    FUN_10014e80((void *)((int)this + 0xc),*(ushort *)((int)this + 0x44),param_1);
    FUN_10014f40((void *)((int)this + 0xc),*(ushort *)((int)this + 0x44),0xffff,param_1);
  }
  return;
}


// 100154a0 FUN_100154a0

undefined4 __thiscall FUN_100154a0(void *this,ushort param_1)

{
  int iVar1;
  undefined4 uVar2;
  uint uVar3;
  uint uVar4;
  ushort unaff_retaddr;
  
  uVar2 = 0;
  if (param_1 != 0xffff) {
                    /* WARNING: Load size is inaccurate */
    iVar1 = *(int *)((int)this + 0xc) + (uint)param_1 * 0xc;
    uVar3 = (**(code **)(*this + 0x14))(*(undefined4 *)(iVar1 + 4));
    uVar4 = *(uint *)((int)this + 8);
    if (*(uint *)((int)this + 8) < uVar3) {
      if (DAT_100385c0 < 1) {
        Warning("Data manager \'used\' memory incorrect\n");
        DAT_100385c0 = DAT_100385c0 + 1;
      }
      uVar3 = *(uint *)((int)this + 8);
      uVar4 = uVar3;
    }
    *(uint *)((int)this + 8) = uVar4 - uVar3;
    uVar2 = *(undefined4 *)(iVar1 + 4);
    *(short *)(iVar1 + 2) = *(short *)(iVar1 + 2) + 1;
    *(undefined4 *)(iVar1 + 4) = 0;
    FUN_10014f40((void *)((int)this + 0xc),*(ushort *)((int)this + 0x48),0xffff,unaff_retaddr);
  }
  return uVar2;
}


// 10015530 FUN_10015530

uint __fastcall FUN_10015530(int *param_1)

{
  int iVar1;
  ushort uVar2;
  uint uVar3;
  ushort uVar4;
  
  uVar3 = FUN_10014bf0(param_1,'\0');
  uVar4 = (ushort)uVar3;
  if (uVar4 == 0xffff) {
    return 0xffff;
  }
  FUN_10014d90(param_1,uVar4);
  iVar1 = *param_1 + (uVar3 & 0xffff) * 10;
  *(undefined2 *)(iVar1 + 8) = 0xffff;
  uVar2 = *(ushort *)((int)param_1 + 0xe);
  *(ushort *)(iVar1 + 6) = uVar2;
  *(ushort *)((int)param_1 + 0xe) = uVar4;
  if (uVar2 == 0xffff) {
    *(short *)((int)param_1 + 0x12) = *(short *)((int)param_1 + 0x12) + 1;
    *(ushort *)(param_1 + 3) = uVar4;
    return uVar3 & 0xffff;
  }
  *(ushort *)(*param_1 + 8 + (uint)uVar2 * 10) = uVar4;
  *(short *)((int)param_1 + 0x12) = *(short *)((int)param_1 + 0x12) + 1;
  return uVar3 & 0xffff;
}


// 100155b0 FUN_100155b0

undefined4 * __thiscall FUN_100155b0(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_10027ff0;
  FUN_100150a0((int *)((int)this + 0xc));
  if ((param_1 & 1) != 0) {
    FUN_1000e140(this);
  }
  return (undefined4 *)this;
}


// 100155e0 FUN_100155e0

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

int __fastcall FUN_100155e0(int *param_1)

{
  undefined1 *puVar1;
  ushort uVar2;
  ushort uVar3;
  code *pcVar4;
  int iVar5;
  uint uVar6;
  int iVar7;
  int iVar8;
  undefined1 *puVar9;
  undefined4 *puVar10;
  int iVar11;
  int aiStack_44 [2];
  uint uStack_3c;
  uint uStack_38;
  undefined1 *local_28;
  uint local_24;
  int local_20;
  int local_1c;
  undefined1 *local_18;
  int local_14;
  uint local_10;
  uint local_c;
  undefined4 local_8;
  
  uStack_38 = 0x100155f1;
  (**(code **)*param_1)();
  local_10 = (uint)*(ushort *)(param_1[6] + 4 + (uint)*(ushort *)(param_1 + 0x11) * 10);
  uStack_38 = 0x10015615;
  iVar11 = -(local_10 * 4 + 0xf & 0xfffffff0);
  local_24 = local_10;
  local_28 = &stack0xffffffcc + iVar11;
  local_20 = -1;
  local_1c = 0;
  local_18 = &stack0xffffffcc + iVar11;
  local_14 = param_1[2];
  uVar6 = (uint)*(ushort *)(param_1[6] + (uint)*(ushort *)(param_1 + 0x11) * 10);
  iVar5 = 0;
  while (uVar6 != 0xffff) {
    uVar2 = *(ushort *)(param_1[3] + 10 + (uVar6 & 0xffff) * 0xc);
    uVar3 = *(ushort *)(param_1 + 0x11);
    *(uint *)((int)&uStack_38 + iVar11) = uVar6;
    *(uint *)((int)&uStack_3c + iVar11) = (uint)uVar3;
    local_c = (uint)uVar2;
    *(undefined4 *)((int)aiStack_44 + iVar11 + 4) = 0x10015660;
    FUN_10014e80(param_1 + 3,*(ushort *)((int)&uStack_3c + iVar11),
                 *(ushort *)((int)&uStack_38 + iVar11));
    *(uint *)((int)&uStack_38 + iVar11) = uVar6;
    *(undefined4 *)((int)&uStack_3c + iVar11) = 0x10015668;
    local_8 = FUN_100154a0(param_1,*(ushort *)((int)&uStack_38 + iVar11));
    iVar8 = iVar5;
    if ((int)local_24 < iVar5 + 1) {
      *(uint *)((int)&uStack_38 + iVar11) = (iVar5 - local_24) + 1;
      *(undefined4 *)((int)&uStack_3c + iVar11) = 0x10015685;
      FUN_10014830(&local_28,*(int *)((int)&uStack_38 + iVar11));
      iVar8 = local_1c;
    }
    iVar8 = iVar8 + 1;
    iVar7 = (iVar8 - iVar5) + -1;
    local_1c = iVar8;
    local_18 = local_28;
    if (0 < iVar7) {
      puVar1 = local_28 + iVar5 * 4;
      *(int *)((int)&uStack_38 + iVar11) = iVar7 * 4;
      *(undefined1 **)((int)&uStack_3c + iVar11) = puVar1;
      *(undefined1 **)((int)aiStack_44 + iVar11 + 4) = puVar1 + 4;
      *(undefined4 *)((int)aiStack_44 + iVar11) = 0x100156b4;
      FUN_10016630(*(void **)((int)aiStack_44 + iVar11 + 4),*(void **)((int)&uStack_3c + iVar11),
                   *(size_t *)((int)&uStack_38 + iVar11));
    }
    iVar7 = iVar5 * 4;
    uVar6 = local_c;
    iVar5 = iVar8;
    if ((undefined4 *)(local_28 + iVar7) != (undefined4 *)0x0) {
      *(undefined4 *)(local_28 + iVar7) = local_8;
    }
  }
  pcVar4 = *(code **)(*param_1 + 8);
  puVar9 = (undefined1 *)((int)&uStack_38 + iVar11);
  *(undefined4 *)((int)&uStack_38 + iVar11) = 0x100156de;
  (*pcVar4)();
  uVar6 = local_10;
  iVar11 = 0;
  if (0 < (int)local_10) {
    do {
      pcVar4 = *(code **)(*param_1 + 0x10);
      *(undefined4 *)(puVar9 + -4) = *(undefined4 *)(local_28 + iVar11 * 4);
      puVar10 = (undefined4 *)(puVar9 + -8);
      puVar9 = puVar9 + -8;
      *puVar10 = 0x100156f7;
      (*pcVar4)();
      iVar11 = iVar11 + 1;
    } while (iVar11 < (int)uVar6);
  }
  iVar11 = local_14 - param_1[2];
  if ((-1 < local_20) && (local_28 != (undefined1 *)0x0)) {
    iVar5 = **(int **)g_pMemAlloc_exref;
    *(undefined1 **)(puVar9 + -4) = local_28;
    pcVar4 = *(code **)(iVar5 + 0x14);
    *(undefined4 *)(puVar9 + -8) = 0x10015723;
    (*pcVar4)();
  }
  return iVar11;
}


// 10015730 FUN_10015730

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

int __fastcall FUN_10015730(int *param_1)

{
  ushort *puVar1;
  undefined1 *puVar2;
  ushort uVar3;
  ushort uVar4;
  code *pcVar5;
  int iVar6;
  int iVar7;
  uint uVar8;
  undefined1 *puVar9;
  undefined4 *puVar10;
  int iVar11;
  int iVar12;
  int aiStack_44 [2];
  uint uStack_3c;
  uint uStack_38;
  undefined1 *local_28;
  int local_24;
  int local_20;
  int local_1c;
  undefined1 *local_18;
  int local_14;
  int local_10;
  uint local_c;
  uint local_8;
  
  uStack_38 = 0x10015741;
  (**(code **)*param_1)();
  puVar1 = (ushort *)(param_1[6] + (uint)*(ushort *)(param_1 + 0x11) * 10);
  local_14 = (uint)*(ushort *)(param_1[6] + 4 + (uint)*(ushort *)((int)param_1 + 0x46) * 10) +
             (uint)puVar1[2];
  uStack_38 = 0x10015772;
  iVar6 = -(local_14 * 4 + 0xfU & 0xfffffff0);
  uVar8 = (uint)*puVar1;
  local_24 = local_14;
  iVar11 = 0;
  local_28 = &stack0xffffffcc + iVar6;
  local_20 = -1;
  local_1c = 0;
  local_18 = &stack0xffffffcc + iVar6;
  local_10 = param_1[2];
  if (uVar8 != 0xffff) {
    iVar12 = 0;
    do {
      uVar3 = *(ushort *)(param_1[3] + 10 + (uVar8 & 0xffff) * 0xc);
      uVar4 = *(ushort *)(param_1 + 0x11);
      *(uint *)((int)&uStack_38 + iVar6) = uVar8;
      *(uint *)((int)&uStack_3c + iVar6) = (uint)uVar4;
      local_8 = (uint)uVar3;
      *(undefined4 *)((int)aiStack_44 + iVar6 + 4) = 0x100157bf;
      FUN_10014e80(param_1 + 3,*(ushort *)((int)&uStack_3c + iVar6),
                   *(ushort *)((int)&uStack_38 + iVar6));
      *(uint *)((int)&uStack_38 + iVar6) = uVar8;
      *(undefined4 *)((int)&uStack_3c + iVar6) = 0x100157c7;
      local_c = FUN_100154a0(param_1,*(ushort *)((int)&uStack_38 + iVar6));
      iVar11 = iVar12;
      if (local_24 < iVar12 + 1) {
        *(int *)((int)&uStack_38 + iVar6) = (iVar12 - local_24) + 1;
        *(undefined4 *)((int)&uStack_3c + iVar6) = 0x100157e4;
        FUN_10014830(&local_28,*(int *)((int)&uStack_38 + iVar6));
        iVar11 = local_1c;
      }
      iVar11 = iVar11 + 1;
      iVar7 = (iVar11 - iVar12) + -1;
      local_1c = iVar11;
      local_18 = local_28;
      if (0 < iVar7) {
        puVar2 = local_28 + iVar12 * 4;
        *(int *)((int)&uStack_38 + iVar6) = iVar7 * 4;
        *(undefined1 **)((int)&uStack_3c + iVar6) = puVar2;
        *(undefined1 **)((int)aiStack_44 + iVar6 + 4) = puVar2 + 4;
        *(undefined4 *)((int)aiStack_44 + iVar6) = 0x10015813;
        FUN_10016630(*(void **)((int)aiStack_44 + iVar6 + 4),*(void **)((int)&uStack_3c + iVar6),
                     *(size_t *)((int)&uStack_38 + iVar6));
      }
      if ((undefined4 *)(local_28 + iVar12 * 4) != (undefined4 *)0x0) {
        *(uint *)(local_28 + iVar12 * 4) = local_c;
      }
      uVar8 = local_8;
      iVar12 = iVar11;
    } while (local_8 != 0xffff);
  }
  local_c = (uint)*(ushort *)(param_1[6] + (uint)*(ushort *)((int)param_1 + 0x46) * 10);
  while (local_c != 0xffff) {
    iVar12 = param_1[3];
    uVar8 = local_c & 0xffff;
    *(uint *)((int)&uStack_38 + iVar6) = local_c;
    iVar7 = uVar8 * 0xc;
    uVar3 = *(ushort *)(iVar7 + 10 + iVar12);
    *(uint *)((int)&uStack_3c + iVar6) = (uint)*(ushort *)((int)param_1 + 0x46);
    local_8 = (uint)uVar3;
    *(undefined4 *)((int)aiStack_44 + iVar6 + 4) = 0x10015878;
    FUN_10014e80(param_1 + 3,*(ushort *)((int)&uStack_3c + iVar6),
                 *(ushort *)((int)&uStack_38 + iVar6));
    *(undefined2 *)(iVar7 + param_1[3]) = 0;
    *(uint *)((int)&uStack_38 + iVar6) = local_c;
    *(undefined4 *)((int)&uStack_3c + iVar6) = 0x1001588c;
    local_c = FUN_100154a0(param_1,*(ushort *)((int)&uStack_38 + iVar6));
    iVar12 = iVar11;
    if (local_24 < iVar11 + 1) {
      *(int *)((int)&uStack_38 + iVar6) = (iVar11 - local_24) + 1;
      *(undefined4 *)((int)&uStack_3c + iVar6) = 0x100158a9;
      FUN_10014830(&local_28,*(int *)((int)&uStack_38 + iVar6));
      iVar12 = local_1c;
    }
    iVar12 = iVar12 + 1;
    iVar7 = (iVar12 - iVar11) + -1;
    local_1c = iVar12;
    local_18 = local_28;
    if (0 < iVar7) {
      puVar2 = local_28 + iVar11 * 4;
      *(int *)((int)&uStack_38 + iVar6) = iVar7 * 4;
      *(undefined1 **)((int)&uStack_3c + iVar6) = puVar2;
      *(undefined1 **)((int)aiStack_44 + iVar6 + 4) = puVar2 + 4;
      *(undefined4 *)((int)aiStack_44 + iVar6) = 0x100158d8;
      FUN_10016630(*(void **)((int)aiStack_44 + iVar6 + 4),*(void **)((int)&uStack_3c + iVar6),
                   *(size_t *)((int)&uStack_38 + iVar6));
    }
    if ((undefined4 *)(local_28 + iVar11 * 4) != (undefined4 *)0x0) {
      *(uint *)(local_28 + iVar11 * 4) = local_c;
    }
    local_c = local_8;
    iVar11 = iVar12;
  }
  pcVar5 = *(code **)(*param_1 + 8);
  *(ushort *)((int)param_1 + 0x4a) = *(ushort *)((int)param_1 + 0x4a) & 0xfffe;
  puVar9 = (undefined1 *)((int)&uStack_38 + iVar6);
  *(undefined4 *)((int)&uStack_38 + iVar6) = 0x1001590a;
  (*pcVar5)();
  iVar6 = local_14;
  iVar11 = 0;
  if (0 < local_14) {
    do {
      pcVar5 = *(code **)(*param_1 + 0x10);
      *(undefined4 *)(puVar9 + -4) = *(undefined4 *)(local_28 + iVar11 * 4);
      puVar10 = (undefined4 *)(puVar9 + -8);
      puVar9 = puVar9 + -8;
      *puVar10 = 0x10015923;
      (*pcVar5)();
      iVar11 = iVar11 + 1;
    } while (iVar11 < iVar6);
  }
  if ((-1 < local_20) && (local_28 != (undefined1 *)0x0)) {
    iVar6 = **(int **)g_pMemAlloc_exref;
    *(undefined1 **)(puVar9 + -4) = local_28;
    pcVar5 = *(code **)(iVar6 + 0x14);
    *(undefined4 *)(puVar9 + -8) = 0x10015947;
    (*pcVar5)();
  }
  return local_10;
}


// 10015960 FUN_10015960

void __thiscall FUN_10015960(void *this,int param_1)

{
  int *this_00;
  undefined4 uVar1;
  short sVar2;
  uint uVar3;
  ushort uVar4;
  int iVar5;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar3 = param_1 - 1U & 0xffff;
  this_00 = (int *)((int)this + 0xc);
  if ((int)uVar3 < *(int *)((int)this + 0x3c)) {
    iVar5 = *this_00 + uVar3 * 0xc;
    sVar2 = (short)(param_1 - 1U);
    if (((*(short *)(iVar5 + 8) != sVar2) || (*(short *)(iVar5 + 10) == sVar2)) &&
       (*(short *)(*this_00 + 2 + uVar3 * 0xc) == (short)((uint)param_1 >> 0x10)))
    goto LAB_100159ae;
  }
  uVar3 = 0xffff;
LAB_100159ae:
  if ((int)uVar3 < *(int *)((int)this + 0x3c)) {
    iVar5 = *this_00 + uVar3 * 0xc;
    uVar4 = (ushort)uVar3;
    if ((*(ushort *)(iVar5 + 8) != uVar4) || (*(ushort *)(iVar5 + 10) == uVar4)) {
      if (*(short *)(uVar3 * 0xc + *this_00) != 0) {
        FUN_100153a0(this,param_1);
      }
      FUN_10014e80(this_00,*(ushort *)((int)this + 0x44),uVar4);
      uVar1 = FUN_100154a0(this,uVar4);
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 8))();
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x10))(uVar1);
      return;
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return;
}


// 10015a20 FUN_10015a20

undefined4 __thiscall FUN_10015a20(void *this,int param_1)

{
  undefined4 uVar1;
  int iVar2;
  ushort uVar3;
  uint uVar4;
  int iVar5;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar4 = param_1 - 1U & 0xffff;
  if ((int)uVar4 < *(int *)((int)this + 0x3c)) {
    iVar2 = uVar4 * 0xc;
    iVar5 = *(int *)((int)this + 0xc) + iVar2;
    uVar3 = (ushort)(param_1 - 1U);
    if ((((*(ushort *)(iVar5 + 8) != uVar3) || (*(ushort *)(iVar5 + 10) == uVar3)) &&
        (*(short *)(*(int *)((int)this + 0xc) + 2 + iVar2) == (short)((uint)param_1 >> 0x10))) &&
       (uVar3 != 0xffff)) {
      FUN_10015450(this,uVar3);
                    /* WARNING: Load size is inaccurate */
      uVar1 = *(undefined4 *)(*(int *)((int)this + 0xc) + 4 + iVar2);
      (**(code **)(*this + 8))();
      return uVar1;
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return 0;
}


// 10015aa0 FUN_10015aa0

void __thiscall FUN_10015aa0(void *this,int param_1)

{
  int *this_00;
  int iVar1;
  short *psVar2;
  int iVar3;
  ushort uVar4;
  uint uVar5;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar5 = param_1 - 1U & 0xffff;
  this_00 = (int *)((int)this + 0xc);
  if ((int)uVar5 < *(int *)((int)this + 0x3c)) {
    iVar1 = uVar5 * 0xc;
    iVar3 = *this_00 + iVar1;
    uVar4 = (ushort)(param_1 - 1U);
    if ((((*(ushort *)(iVar3 + 8) != uVar4) || (*(ushort *)(iVar3 + 10) == uVar4)) &&
        (psVar2 = (short *)(iVar1 + *this_00), psVar2[1] == (short)((uint)param_1 >> 0x10))) &&
       ((uVar4 != 0xffff && (*psVar2 == 0)))) {
      FUN_10014e80(this_00,*(ushort *)((int)this + 0x44),uVar4);
      FUN_10014f40(this_00,*(ushort *)((int)this + 0x44),0xffff,uVar4);
    }
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return;
}


// 10015b30 FUN_10015b30

uint __thiscall FUN_10015b30(void *this,char param_1)

{
  int *this_00;
  short *psVar1;
  ushort uVar2;
  ushort uVar3;
  undefined2 extraout_var;
  uint uVar4;
  
                    /* WARNING: Load size is inaccurate */
  (*(code *)**this)();
  uVar2 = *(ushort *)(*(int *)((int)this + 0x18) + (uint)*(ushort *)((int)this + 0x48) * 10);
  uVar4 = (uint)uVar2;
  if (param_1 == '\0') {
    uVar3 = *(ushort *)((int)this + 0x44);
  }
  else {
    uVar3 = *(ushort *)((int)this + 0x46);
  }
  this_00 = (int *)((int)this + 0xc);
  if (uVar4 == 0xffff) {
    uVar4 = FUN_10015120(this_00,uVar3,0xffff);
    uVar4 = uVar4 & 0xffff;
  }
  else {
    FUN_10014e80(this_00,*(ushort *)((int)this + 0x48),uVar2);
    FUN_10014f40(this_00,uVar3,0xffff,uVar2);
  }
  if (param_1 != '\0') {
    psVar1 = (short *)(*this_00 + uVar4 * 0xc);
    *psVar1 = *psVar1 + 1;
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
  return CONCAT22(extraout_var,(short)uVar4);
}


// 10015bc0 FUN_10015bc0

int __thiscall FUN_10015bc0(void *this,uint param_1)

{
  ushort uVar1;
  int iVar2;
  undefined4 uVar3;
  
  iVar2 = *(int *)((int)this + 8);
  while( true ) {
    if ((*(uint *)((int)this + 8) <= *(uint *)((int)this + 4)) &&
       (param_1 <= *(uint *)((int)this + 4) - *(uint *)((int)this + 8))) goto LAB_10015c33;
                    /* WARNING: Load size is inaccurate */
    (*(code *)**this)();
    uVar1 = *(ushort *)(*(int *)((int)this + 0x18) + (uint)*(ushort *)((int)this + 0x44) * 10);
    if (uVar1 == 0xffff) break;
    FUN_10014e80((void *)((int)this + 0xc),*(ushort *)((int)this + 0x44),uVar1);
    uVar3 = FUN_100154a0(this,uVar1);
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 8))();
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x10))(uVar3);
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 8))();
LAB_10015c33:
  return iVar2 - *(int *)((int)this + 8);
}


// 10015c40 FUN_10015c40

void __thiscall FUN_10015c40(void *this,int *param_1)

{
  void *pvVar1;
  uint *puVar2;
  ushort uVar3;
  int iVar4;
  uint uVar5;
  int iVar6;
  
  uVar3 = *(ushort *)(*(int *)((int)this + 0x18) + 2 + (uint)*(ushort *)((int)this + 0x44) * 10);
  while (uVar5 = (uint)uVar3, uVar5 != 0xffff) {
    iVar4 = param_1[3];
    uVar3 = *(ushort *)(uVar5 * 0xc + 2 + *(int *)((int)this + 0xc));
    if (param_1[1] < iVar4 + 1) {
      FUN_10014830(param_1,(iVar4 - param_1[1]) + 1);
    }
    param_1[3] = param_1[3] + 1;
    iVar6 = (param_1[3] - iVar4) + -1;
    param_1[4] = *param_1;
    if (0 < iVar6) {
      pvVar1 = (void *)(*param_1 + iVar4 * 4);
      FUN_10016630((void *)((int)pvVar1 + 4),pvVar1,iVar6 * 4);
    }
    puVar2 = (uint *)(*param_1 + iVar4 * 4);
    if (puVar2 != (uint *)0x0) {
      *puVar2 = uVar5 + 1 & 0xffff | (uint)uVar3 << 0x10;
    }
    uVar3 = *(ushort *)(uVar5 * 0xc + 8 + *(int *)((int)this + 0xc));
  }
  return;
}


// 10015d10 FUN_10015d10

void __thiscall FUN_10015d10(void *this,int *param_1)

{
  void *pvVar1;
  uint *puVar2;
  ushort uVar3;
  int iVar4;
  uint uVar5;
  int iVar6;
  
  uVar3 = *(ushort *)(*(int *)((int)this + 0x18) + (uint)*(ushort *)((int)this + 0x46) * 10);
  while (uVar5 = (uint)uVar3, uVar5 != 0xffff) {
    iVar4 = param_1[3];
    uVar3 = *(ushort *)(*(int *)((int)this + 0xc) + 2 + uVar5 * 0xc);
    if (param_1[1] < iVar4 + 1) {
      FUN_10014830(param_1,(iVar4 - param_1[1]) + 1);
    }
    param_1[3] = param_1[3] + 1;
    iVar6 = (param_1[3] - iVar4) + -1;
    param_1[4] = *param_1;
    if (0 < iVar6) {
      pvVar1 = (void *)(*param_1 + iVar4 * 4);
      FUN_10016630((void *)((int)pvVar1 + 4),pvVar1,iVar6 * 4);
    }
    puVar2 = (uint *)(*param_1 + iVar4 * 4);
    if (puVar2 != (uint *)0x0) {
      *puVar2 = uVar5 + 1 & 0xffff | (uint)uVar3 << 0x10;
    }
    uVar3 = *(ushort *)(*(int *)((int)this + 0xc) + 10 + uVar5 * 0xc);
  }
  return;
}


// 10015de0 FUN_10015de0

undefined4 * __thiscall FUN_10015de0(void *this,undefined4 param_1)

{
  int *piVar1;
  int iVar2;
  int iVar3;
  uint uVar4;
  
  *(undefined ***)this = &PTR_FUN_10027ff0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 0x18) = 0;
  *(undefined4 *)((int)this + 0x1c) = 0;
  *(undefined4 *)((int)this + 0x20) = 0;
  *(undefined2 *)((int)this + 0x2e) = 0xffff;
  *(undefined2 *)((int)this + 0x24) = 0xffff;
  *(undefined2 *)((int)this + 0x26) = 0xffff;
  *(undefined2 *)((int)this + 0x28) = 0xffff;
  *(undefined2 *)((int)this + 0x2a) = 0;
  *(undefined2 *)((int)this + 0x2c) = 0;
  *(undefined4 *)((int)this + 0x30) = *(undefined4 *)((int)this + 0x18);
  *(undefined4 *)((int)this + 0x40) = *(undefined4 *)((int)this + 0xc);
  *(undefined4 *)((int)this + 0x34) = 0;
  *(undefined2 *)((int)this + 0x38) = 0xffff;
  *(undefined2 *)((int)this + 0x3a) = 0;
  *(undefined4 *)((int)this + 0x3c) = 0;
  piVar1 = (int *)((int)this + 0x18);
  *(undefined4 *)((int)this + 4) = param_1;
  *(undefined4 *)((int)this + 8) = 0;
  uVar4 = FUN_10015530(piVar1);
  iVar2 = *piVar1;
  iVar3 = (uVar4 & 0xffff) * 10;
  *(undefined2 *)(iVar3 + 2 + iVar2) = 0xffff;
  *(undefined2 *)(iVar3 + *piVar1) = *(undefined2 *)(iVar3 + 2 + iVar2);
  *(undefined2 *)(iVar3 + 4 + *piVar1) = 0;
  *(short *)((int)this + 0x44) = (short)uVar4;
  piVar1 = (int *)((int)this + 0x18);
  uVar4 = FUN_10015530(piVar1);
  iVar2 = *piVar1;
  iVar3 = (uVar4 & 0xffff) * 10;
  *(undefined2 *)(iVar2 + 2 + iVar3) = 0xffff;
  *(undefined2 *)(iVar3 + *piVar1) = *(undefined2 *)(iVar2 + 2 + iVar3);
  *(undefined2 *)(*piVar1 + 4 + iVar3) = 0;
  *(short *)((int)this + 0x46) = (short)uVar4;
  piVar1 = (int *)((int)this + 0x18);
  uVar4 = FUN_10015530(piVar1);
  iVar2 = *piVar1;
  iVar3 = (uVar4 & 0xffff) * 10;
  *(undefined2 *)(iVar3 + 2 + iVar2) = 0xffff;
  *(undefined2 *)(iVar3 + *piVar1) = *(undefined2 *)(iVar3 + 2 + iVar2);
  *(undefined2 *)(iVar3 + 4 + *piVar1) = 0;
  *(ushort *)((int)this + 0x4a) = *(ushort *)((int)this + 0x4a) & 0xfffe;
  *(short *)((int)this + 0x48) = (short)uVar4;
  return (undefined4 *)this;
}


// 10015ef0 FUN_10015ef0

void __fastcall FUN_10015ef0(void *param_1)

{
  FUN_10015bc0(param_1,0);
  return;
}


// 10015f20 FUN_10015f20

void __fastcall FUN_10015f20(int param_1)

{
  int iVar1;
  int iVar2;
  int iVar3;
  
  iVar1 = param_1 + 0x20;
  iVar3 = *(int *)(param_1 + 0x24);
  while (iVar3 != iVar1) {
    iVar2 = *(int *)(iVar3 + 4);
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar3);
    iVar3 = iVar2;
  }
  *(int *)(param_1 + 0x24) = iVar1;
  *(int *)iVar1 = iVar1;
  *(undefined2 *)(param_1 + 0x1a) = 0;
  *(undefined4 *)(param_1 + 0x10) = 0;
  *(undefined4 *)(param_1 + 0xc) = 0;
  return;
}


// 10015f60 FUN_10015f60

void __thiscall FUN_10015f60(void *this,undefined4 *param_1)

{
  if (param_1 != (undefined4 *)0x0) {
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + -1;
    *param_1 = *(undefined4 *)((int)this + 0xc);
    *(undefined4 **)((int)this + 0xc) = param_1;
  }
  return;
}


// 10015f80 FUN_10015f80

void __fastcall FUN_10015f80(int param_1)

{
  if ((0 < *(int *)(param_1 + 0x10)) && (DAT_100385cc != (code *)0x0)) {
    (*DAT_100385cc)("Memory leak: mempool blocks left in memory: %d\n",*(int *)(param_1 + 0x10));
  }
  FUN_10015f20(param_1);
  return;
}


// 10015fb0 FUN_10015fb0

void __fastcall FUN_10015fb0(int *param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  undefined4 *puVar4;
  undefined4 *puVar5;
  int iVar6;
  
  if (param_1[2] == 2) {
    iVar2 = 1;
  }
  else {
    if ((param_1[2] == 0) && (*(short *)((int)param_1 + 0x1a) != 0)) {
      return;
    }
    iVar2 = *(ushort *)((int)param_1 + 0x1a) + 1;
  }
  iVar1 = param_1[1];
  iVar6 = *param_1 * iVar1 * iVar2;
  piVar3 = (int *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))
                            (*(ushort *)(param_1 + 6) + 0xe + iVar6);
  piVar3[2] = iVar6;
  piVar3[1] = (int)(param_1 + 8);
  iVar6 = param_1[8];
  *piVar3 = iVar6;
  *(int **)(iVar6 + 4) = piVar3;
  *(int **)piVar3[1] = piVar3;
  puVar4 = (undefined4 *)
           (*(ushort *)(param_1 + 6) + 0xb + (int)piVar3 & ~(*(ushort *)(param_1 + 6) - 1));
  iVar2 = iVar1 * iVar2 + -1;
  param_1[3] = (int)puVar4;
  puVar5 = puVar4;
  if (0 < iVar2) {
    do {
      puVar4 = (undefined4 *)(*param_1 + (int)puVar5);
      iVar2 = iVar2 + -1;
      *puVar5 = puVar4;
      puVar5 = puVar4;
    } while (iVar2 != 0);
  }
  *puVar4 = 0;
  *(short *)((int)param_1 + 0x1a) = *(short *)((int)param_1 + 0x1a) + 1;
  return;
}


// 10016050 FUN_10016050

uint * __thiscall FUN_10016050(void *this,uint param_1)

{
  uint *puVar1;
  int iVar2;
  
                    /* WARNING: Load size is inaccurate */
  if (param_1 <= *this) {
    puVar1 = *(uint **)((int)this + 0xc);
    if (puVar1 != (uint *)0x0) {
LAB_1001607f:
      *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
      iVar2 = *(int *)((int)this + 0x10);
      if (*(int *)((int)this + 0x10) < *(int *)((int)this + 0x14)) {
        iVar2 = *(int *)((int)this + 0x14);
      }
      *(int *)((int)this + 0x14) = iVar2;
      *(uint *)((int)this + 0xc) = *puVar1;
      return puVar1;
    }
    if ((*(int *)((int)this + 8) != 0) || (*(short *)((int)this + 0x1a) == 0)) {
      FUN_10015fb0((int *)this);
      puVar1 = *(uint **)((int)this + 0xc);
      if (puVar1 != (uint *)0x0) goto LAB_1001607f;
    }
  }
  return (uint *)0x0;
}


// 100160a0 FUN_100160a0

uint * __thiscall
FUN_100160a0(void *this,uint param_1,uint param_2,uint param_3,char *param_4,uint param_5)

{
  int iVar1;
  
  if (param_5 == 0) {
    param_5 = 1;
  }
  *(short *)((int)this + 0x18) = (short)param_5;
  if (param_1 < 4) {
    param_1 = 4;
  }
  *(uint *)((int)this + 4) = param_2;
  *(uint *)this = (param_1 - 1) + (param_5 & 0xffff) & ~((param_5 & 0xffff) - 1);
  *(undefined4 *)((int)this + 0x14) = 0;
  *(uint *)((int)this + 8) = param_3;
  if (param_4 == (char *)0x0) {
    param_4 = ".\\mempool.cpp";
  }
  *(char **)((int)this + 0x1c) = param_4;
  iVar1 = (int)this + 0x20;
  *(undefined2 *)((int)this + 0x1a) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(int *)iVar1 = iVar1;
  *(int *)((int)this + 0x24) = iVar1;
  FUN_10015fb0((int *)this);
  return (uint *)this;
}


// 10016120 FUN_10016120

void __thiscall FUN_10016120(void *this,undefined4 param_1,undefined4 param_2)

{
  *(undefined4 *)((int)this + 4) = param_2;
  *(undefined4 *)this = param_1;
  *(void **)((int)this + 8) = DAT_100385d8;
  DAT_100385d8 = this;
  return;
}


// 10016140 FUN_10016140

undefined4 __cdecl FUN_10016140(byte *param_1,undefined4 *param_2)

{
  int *piVar1;
  byte bVar2;
  byte *pbVar3;
  int iVar4;
  undefined4 uVar5;
  byte *pbVar6;
  undefined4 *puVar7;
  bool bVar8;
  
  puVar7 = DAT_100385d8;
  if (DAT_100385d8 != (undefined4 *)0x0) {
    do {
      pbVar3 = (byte *)puVar7[1];
      pbVar6 = param_1;
      do {
        bVar2 = *pbVar3;
        bVar8 = bVar2 < *pbVar6;
        if (bVar2 != *pbVar6) {
LAB_10016175:
          iVar4 = (1 - (uint)bVar8) - (uint)(bVar8 != 0);
          goto LAB_1001617a;
        }
        if (bVar2 == 0) break;
        bVar2 = pbVar3[1];
        bVar8 = bVar2 < pbVar6[1];
        if (bVar2 != pbVar6[1]) goto LAB_10016175;
        pbVar3 = pbVar3 + 2;
        pbVar6 = pbVar6 + 2;
      } while (bVar2 != 0);
      iVar4 = 0;
LAB_1001617a:
      if (iVar4 == 0) {
        if (param_2 != (undefined4 *)0x0) {
          *param_2 = 0;
        }
                    /* WARNING: Could not recover jumptable at 0x100161aa. Too many branches */
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


// 100161c0 FUN_100161c0

undefined * FUN_100161c0(void)

{
  return FUN_10016140;
}


// 100161d0 FUN_100161d0

int __cdecl FUN_100161d0(char *param_1)

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


// 10016200 FUN_10016200

int __cdecl FUN_10016200(byte *param_1,byte *param_2)

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


// 10016260 FUN_10016260

uint __cdecl FUN_10016260(byte *param_1,byte *param_2)

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
        if (uVar1 != uVar2) goto LAB_100162da;
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
LAB_100162da:
  return uVar1 - uVar2;
}


// 100162e0 FUN_100162e0

int __cdecl FUN_100162e0(char *param_1,char *param_2,int param_3)

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


// 10016340 FUN_10016340

char * __cdecl FUN_10016340(char *param_1,char *param_2,uint param_3,size_t param_4)

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


// 100163d0 FUN_100163d0

void __cdecl FUN_100163d0(char *param_1,char param_2)

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


// 10016400 FUN_10016400

undefined4 __cdecl FUN_10016400(char *param_1,char param_2)

{
  char cVar1;
  char *pcVar2;
  char *_Dst;
  int iVar3;
  int iVar4;
  int iVar5;
  char *_Src;
  
  cVar1 = *param_1;
  pcVar2 = param_1;
  while (cVar1 != '\0') {
    if ((*pcVar2 == '/') || (*pcVar2 == '\\')) {
      *pcVar2 = param_2;
    }
    pcVar2 = pcVar2 + 1;
    cVar1 = *pcVar2;
  }
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  iVar5 = (int)pcVar2 - (int)(param_1 + 1);
  iVar4 = 1;
  iVar3 = iVar5 + -2;
  if (0 < iVar3) {
    do {
      if ((param_1[iVar4] == param_2) && (param_1[iVar4 + 1] == param_2)) {
        _memmove(param_1 + iVar4,param_1 + iVar4 + 1,iVar5 - iVar4);
        iVar5 = iVar5 + -1;
        iVar3 = iVar3 + -1;
      }
      else {
        iVar4 = iVar4 + 1;
      }
    } while (iVar4 <= iVar3);
  }
  iVar3 = iVar5 + -3;
  iVar4 = 0;
  if (-1 < iVar3) {
    do {
      if (((param_1[iVar4] == param_2) && (param_1[iVar4 + 1] == '.')) &&
         (param_1[iVar4 + 2] == param_2)) {
        _memmove(param_1 + iVar4,param_1 + iVar4 + 2,(iVar5 - iVar4) - 1);
        iVar5 = iVar5 + -2;
        iVar3 = iVar3 + -2;
      }
      else {
        iVar4 = iVar4 + 1;
      }
    } while (iVar4 <= iVar3);
  }
  iVar3 = iVar5 + -2;
  iVar4 = 0;
  if (-1 < iVar3) {
    do {
      if (iVar4 == 0) {
        if ((*param_1 != '.') || (param_1[1] != param_2)) goto LAB_1001654e;
        _memmove(param_1,param_1 + 2,iVar5 - 1);
        iVar5 = iVar5 + -2;
        iVar3 = iVar3 + -2;
      }
      else {
        if (((param_1[iVar4 + -1] != '.') && (param_1[iVar4] == '.')) &&
           (param_1[iVar4 + 1] == param_2)) {
          _memmove(param_1 + iVar4,param_1 + iVar4 + 1,iVar5 - iVar4);
          iVar5 = iVar5 + -1;
          iVar3 = iVar3 + -1;
        }
LAB_1001654e:
        iVar4 = iVar4 + 1;
      }
    } while (iVar4 <= iVar3);
  }
  if (((2 < iVar5) && (param_1[iVar5 + -1] == '.')) && (param_2 == param_1[iVar5 + -2])) {
    param_1[iVar5 + -2] = '\0';
    iVar5 = iVar5 + -2;
  }
  if (((1 < iVar5) && (*param_1 == '.')) && (param_1[1] == param_2)) {
    _memmove(param_1,param_1 + 2,iVar5 - 1);
  }
  cVar1 = *param_1;
  pcVar2 = param_1;
  do {
    if (cVar1 == '\0') {
      return CONCAT31((int3)((uint)pcVar2 >> 8),1);
    }
    if (((*pcVar2 == '.') && (pcVar2[1] == '.')) && ((pcVar2 == param_1 || (param_2 == pcVar2[-1])))
       ) {
      _Src = pcVar2 + 2;
      if ((pcVar2[2] != '\0') && (param_2 != pcVar2[2])) goto LAB_10016611;
      if ((pcVar2 == param_1) || (pcVar2 + -1 == param_1)) {
        return (uint)pcVar2 & 0xffffff00;
      }
      for (_Dst = pcVar2 + -2; (param_1 < _Dst && (param_2 != *_Dst)); _Dst = _Dst + -1) {
      }
      if ((_Dst == param_1) && (*_Src != '\0')) {
        _Src = pcVar2 + 3;
      }
      pcVar2 = _Src;
      do {
        cVar1 = *pcVar2;
        pcVar2 = pcVar2 + 1;
      } while (cVar1 != '\0');
      _memmove(_Dst,_Src,(size_t)(pcVar2 + (1 - (int)(_Src + 1))));
      pcVar2 = param_1;
    }
    else {
LAB_10016611:
      pcVar2 = pcVar2 + 1;
    }
    cVar1 = *pcVar2;
  } while( true );
}


// 10016630 FUN_10016630

void __cdecl FUN_10016630(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  _memmove(param_1,param_2,param_3);
  return;
}


// 10016660 FUN_10016660

void __cdecl FUN_10016660(char *param_1,char *param_2,size_t param_3)

{
  _AssertValidWritePtr(param_1,param_3);
  AssertValidStringPtr(param_2,0xffffff);
  _strncpy(param_1,param_2,param_3);
  if (0 < (int)param_3) {
    param_1[param_3 - 1] = '\0';
  }
  return;
}


// 100166a0 FUN_100166a0

size_t __cdecl FUN_100166a0(char *param_1,size_t param_2,char *param_3)

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


// 100166f0 FUN_100166f0

size_t __cdecl FUN_100166f0(char *param_1,size_t param_2,char *param_3,va_list param_4,int param_5)

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


// 10016750 FUN_10016750

char * __cdecl FUN_10016750(float param_1,undefined4 param_2,char param_3)

{
  float fVar1;
  uint *puVar2;
  int iVar3;
  undefined4 extraout_ECX;
  undefined4 extraout_EDX;
  char *pcVar4;
  uint *puVar5;
  float10 extraout_ST0;
  ulonglong uVar6;
  char *pcVar7;
  char local_48 [8];
  undefined4 local_40;
  char local_20 [32];
  
  if (param_3 == '\0') {
    fVar1 = 1000.0;
  }
  else {
    fVar1 = 1024.0;
  }
  pcVar4 = &DAT_100385e0 + DAT_100386e0 * 0x20;
  if (param_1 <= fVar1 * fVar1) {
    if (param_1 <= fVar1) {
      pcVar7 = " bytes";
    }
    else {
      pcVar7 = " KB";
    }
  }
  else {
    pcVar7 = " MB";
  }
  DAT_100386e0 = DAT_100386e0 + 1 & 7;
  FUN_100166a0(local_48,8,pcVar7);
  uVar6 = FUN_10017cc0(extraout_ECX,extraout_EDX);
  if ((float10)1e-05 <= ABS(extraout_ST0 - (float10)(int)uVar6)) {
    FUN_100166a0(local_20,0x20,"%%.%if%s");
    pcVar7 = local_20;
  }
  else {
    pcVar7 = "%i%s";
  }
  FUN_100166a0((char *)&local_40,0x20,pcVar7);
  puVar5 = &local_40;
  puVar2 = FUN_10018c20(puVar5,".");
  if (puVar2 == (uint *)0x0) {
    puVar2 = FUN_10018c20(puVar5," ");
  }
  iVar3 = (int)puVar2 + (-3 - (int)&local_40);
  pcVar7 = pcVar4;
  while ((char)local_40 != '\0') {
    if (((-1 < iVar3) && (iVar3 == (iVar3 / 3) * 3)) && (pcVar7 != pcVar4)) {
      *pcVar7 = ',';
      pcVar7 = pcVar7 + 1;
    }
    puVar5 = (uint *)((int)puVar5 + 1);
    *pcVar7 = (char)local_40;
    iVar3 = iVar3 + -1;
    pcVar7 = pcVar7 + 1;
    local_40._0_1_ = *(char *)puVar5;
  }
  *pcVar7 = '\0';
  return pcVar4;
}


// 100168e0 FUN_100168e0

void __cdecl FUN_100168e0(char *param_1,char *param_2,size_t param_3)

{
  char cVar1;
  char *pcVar2;
  size_t sVar3;
  size_t _Size;
  
  AssertValidStringPtr(param_1,0xffffff);
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  sVar3 = (int)pcVar2 - (int)(param_1 + 1);
  do {
    sVar3 = sVar3 - 1;
    if ((int)sVar3 < 1) goto LAB_1001692f;
    cVar1 = param_1[sVar3];
  } while (((cVar1 != '.') && (cVar1 != '\\')) && (cVar1 != '/'));
  if (((0 < (int)sVar3) && (param_1[sVar3] != '\\')) &&
     ((param_1[sVar3] != '/' && ((int)sVar3 < (int)param_3)))) {
    _Size = param_3 - 1;
    if ((int)sVar3 < (int)(param_3 - 1)) {
      _Size = sVar3;
    }
    if (param_2 != param_1) {
      _memcpy(param_2,param_1,_Size);
    }
    param_2[_Size] = '\0';
    return;
  }
LAB_1001692f:
  if (param_2 != param_1) {
    _AssertValidWritePtr(param_2,param_3);
    AssertValidStringPtr(param_1,0xffffff);
    _strncpy(param_2,param_1,param_3);
    if (0 < (int)param_3) {
      param_2[param_3 - 1] = '\0';
    }
  }
  return;
}


// 100169a0 FUN_100169a0

void __cdecl FUN_100169a0(char *param_1,char *param_2,uint param_3)

{
  char cVar1;
  char *pcVar2;
  char *pcVar3;
  size_t _Count;
  
  FUN_100168e0(param_1,param_1,param_3);
  if (*param_2 != '.') {
    FUN_10016340(param_1,".",param_3,0xffffffff);
  }
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
  if (param_3 <= _Count + ((int)pcVar2 - (int)(param_1 + 1))) {
    _Count = (param_3 - ((int)pcVar2 - (int)(param_1 + 1))) - 1;
  }
  if (_Count != 0) {
    pcVar2 = _strncat(param_1,param_2,_Count);
    pcVar2[param_3 - 1] = '\0';
  }
  return;
}


// 10016a40 FUN_10016a40

void __cdecl FUN_10016a40(undefined4 param_1,undefined **param_2)

{
  int *piVar1;
  int *piVar2;
  
  if ((DAT_10038854 != (int *)0x0) && (DAT_100387fc == '\0')) {
    DAT_100387f8 = param_1;
    DAT_100387fc = '\x01';
    DAT_100358e4 = (**(code **)(*DAT_10038854 + 0x14))();
    piVar2 = DAT_100387f0;
    DAT_100387f4 = param_2;
    if (param_2 == (undefined **)0x0) {
      DAT_100387f4 = &PTR_PTR_100358f0;
    }
    while (piVar2 != (int *)0x0) {
      piVar1 = (int *)piVar2[1];
      (**(code **)(*piVar2 + 0xc))(DAT_100387f8);
      (**(code **)(*piVar2 + 0x2c))();
      piVar2 = piVar1;
    }
    (**(code **)(*DAT_10038854 + 0x70))();
    (**(code **)(*DAT_10038854 + 0x94))();
    DAT_100387f0 = (int *)0x0;
  }
  return;
}


// 10016af0 FUN_10016af0

void FUN_10016af0(void)

{
  if ((DAT_10038854 != (int *)0x0) && (DAT_100387fc != '\0')) {
    (**(code **)(*DAT_10038854 + 0x74))(DAT_100358e4);
    (**(code **)(*DAT_10038854 + 0x20))(DAT_100358e4);
    DAT_100358e4 = 0xffffffff;
    DAT_100387fc = '\0';
  }
  return;
}


// 10016bb0 FUN_10016bb0

void __fastcall FUN_10016bb0(undefined4 param_1)

{
  if (DAT_100387f4 != (undefined4 *)0x0) {
    (**(code **)*DAT_100387f4)(param_1);
  }
  return;
}


// 10016c40 FUN_10016c40

void __fastcall FUN_10016c40(undefined4 *param_1)

{
  *param_1 = &PTR_FUN_1002812c;
  param_1[6] = &PTR_FUN_10028108;
  if (param_1[9] != 0) {
    FUN_1000e140(param_1[9]);
    param_1[9] = 0;
  }
  *param_1 = &PTR_LAB_100280b4;
  return;
}


// 10016c70 FUN_10016c70

undefined4 __thiscall FUN_10016c70(void *this,uint param_1)

{
  return CONCAT31((int3)((uint)*(int *)((int)this + 0x1c) >> 8),
                  (*(uint *)(*(int *)((int)this + 0x1c) + 0x14) & param_1) != 0);
}


// 10016cc0 FUN_10016cc0

undefined4 __fastcall FUN_10016cc0(int param_1)

{
  return *(undefined4 *)(*(int *)(param_1 + 0x1c) + 0xc);
}


// 10016d00 FUN_10016d00

void __fastcall FUN_10016d00(int *param_1)

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
  if ((cVar1 != '\0') && (DAT_10038854 != (int *)0x0)) {
    cVar1 = (**(code **)(*DAT_10038854 + 0x80))();
    if (cVar1 == '\0') {
      (**(code **)(*DAT_10038854 + 0x8c))(param_1,unaff_retaddr);
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
    FUN_100166a0((char *)aiStack_28,0x20,"%f");
    piVar4 = aiStack_28;
    uVar2 = extraout_ECX_00;
    uVar3 = extraout_EDX_00;
  }
  param_1[0xb] = 0xb00000;
  uVar5 = FUN_10017cc0(uVar2,uVar3);
  param_1[0xc] = (int)uVar5;
  if ((param_1[5] & 0x1000U) == 0) {
    (**(code **)(*param_1 + 0x40))(piVar4,fVar7);
  }
  return;
}


// 10016de0 FUN_10016de0

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

void __thiscall FUN_10016de0(void *this,undefined4 param_1,undefined4 param_2)

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
  
  asStack_1c[2] = 0x10016df6;
  iVar2 = -(*(int *)((int)this + 0x28) + 0xfU & 0xfffffff0);
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x28);
  *(undefined4 *)((int)asStack_1c + iVar2 + 4) = uVar4;
  *(undefined1 **)((int)asStack_1c + iVar2) = &stack0xfffffff0 + iVar2;
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = 0x10016e03;
  _memcpy(*(void **)((int)asStack_1c + iVar2),*(void **)((int)asStack_1c + iVar2 + 4),
          *(size_t *)((int)asStack_1c + iVar2 + 8));
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = param_1;
  *(undefined4 *)((int)auStack_24 + iVar2) = 0x10016e0c;
  iVar3 = FUN_100161d0(*(char **)((int)auStack_24 + iVar2 + 4));
  iVar3 = iVar3 + 1;
  puVar6 = &stack0xfffffff0 + iVar2;
  if (*(int *)((int)this + 0x28) < iVar3) {
    if (*(int *)((int)this + 0x24) != 0) {
      *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x24);
      *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x10016e26;
      FUN_1000e140(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    }
    puVar6 = &stack0xfffffff0 + iVar2;
    *(int *)((int)asStack_1c + iVar2 + 8) = iVar3;
    *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x10016e2f;
    uVar4 = FUN_1000dfa0(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    *(undefined4 *)((int)this + 0x24) = uVar4;
    *(int *)((int)this + 0x28) = iVar3;
  }
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)(puVar6 + -4) = iVar3;
  *(undefined4 *)(puVar6 + -8) = param_1;
  *(undefined4 *)(puVar6 + -0xc) = uVar4;
  *(undefined4 *)(puVar6 + -0x10) = 0x10016e46;
  _memcpy(*(void **)(puVar6 + -0xc),*(void **)(puVar6 + -8),*(size_t *)(puVar6 + -4));
  pcVar1 = *(code **)((int)this + 0x44);
  if (pcVar1 != (code *)0x0) {
    *(undefined4 *)(puVar6 + -4) = extraout_ECX;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(int *)(puVar6 + -0xc) = (int)this + 0x18;
    puVar5 = puVar6 + -0x10;
    *(undefined4 *)(puVar6 + -0x10) = 0x10016e5e;
    (*pcVar1)();
    puVar6 = puVar5 + 0xc;
  }
  if (DAT_10038854 != (int *)0x0) {
    pcVar1 = *(code **)(*DAT_10038854 + 0x48);
    *(int **)(puVar6 + -4) = DAT_10038854;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(void **)(puVar6 + -0xc) = this;
    *(undefined4 *)(puVar6 + -0x10) = 0x10016e7b;
    (*pcVar1)();
  }
  return;
}


// 10016ee0 FUN_10016ee0

void __thiscall FUN_10016ee0(void *this,float param_1)

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
    if (((cVar1 != '\0') && (DAT_10038854 != (int *)0x0)) &&
       (cVar1 = (**(code **)(*DAT_10038854 + 0x80))(), cVar1 == '\0')) {
      (**(code **)(*DAT_10038854 + 0x84))();
      return;
    }
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x3c))(&stack0x00000000);
    uVar3 = *(undefined4 *)((int)this + 0x2c);
    *(undefined4 *)((int)this + 0x2c) = uStack_4;
    uVar2 = FUN_10017cc0(extraout_ECX,extraout_EDX);
    *(int *)((int)this + 0x30) = (int)uVar2;
    if ((*(uint *)((int)this + 0x14) & 0x1000) == 0) {
      FUN_100166a0(&stack0xffffffd8,0x20,"%f");
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x40))(&stack0xffffffd8,uVar3);
      return;
    }
  }
  return;
}


// 10017080 FUN_10017080

void __thiscall
FUN_10017080(void *this,int param_1,char *param_2,uint param_3,undefined *param_4,undefined1 param_5
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
    param_2 = PTR_DAT_10035970;
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
  _Dst = (void *)FUN_1000dfa0(param_2 + (1 - (int)pcVar1));
  *(void **)((int)this + 0x24) = _Dst;
  _memcpy(_Dst,*(void **)((int)this + 0x20),*(size_t *)((int)this + 0x28));
  dVar4 = _atof(*(char **)((int)this + 0x24));
  *(float *)((int)this + 0x2c) = (float)dVar4;
  uVar3 = FUN_10017cc0(extraout_ECX,extraout_EDX);
  *(int *)((int)this + 0x30) = (int)uVar3;
  if (((param_3 & 0x84200) == 0) && (-1 < (char)param_3)) {
    param_3 = param_3 | 2;
  }
  *(int *)((int)this + 0xc) = param_1;
  *(undefined1 *)((int)this + 8) = 0;
  if (param_4 == (undefined *)0x0) {
    param_4 = PTR_DAT_1003593c;
  }
  *(undefined **)((int)this + 0x10) = param_4;
  *(uint *)((int)this + 0x14) = param_3;
  if ((param_3 & 1) == 0) {
    *(void **)((int)this + 4) = DAT_100387f0;
    DAT_100387f0 = this;
  }
  else {
    *(undefined4 *)((int)this + 4) = 0;
  }
  if (DAT_100387f4 != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x2c))();
  }
  return;
}


// 100171a0 FUN_100171a0

void __fastcall FUN_100171a0(int param_1)

{
                    /* WARNING: Could not recover jumptable at 0x100171a8. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(**(int **)(param_1 + 4) + 0x38))();
  return;
}


// 100171b0 FUN_100171b0

void __thiscall FUN_100171b0(void *this,undefined *param_1)

{
  if (param_1 == (undefined *)0x0) {
    param_1 = &DAT_10038820;
  }
  *(undefined **)this = param_1;
  if (param_1 != (undefined *)0x0) {
    *(undefined **)((int)this + 4) = param_1 + -0x18;
    return;
  }
  *(undefined4 *)((int)this + 4) = 0;
  return;
}


// 10017240 FUN_10017240

undefined4 * __thiscall FUN_10017240(void *this,int param_1,char *param_2,uint param_3)

{
  *(undefined1 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_100280e8;
  *(undefined ***)this = &PTR_FUN_1002812c;
  *(undefined ***)((int)this + 0x18) = &PTR_FUN_10028108;
  FUN_10017080(this,param_1,param_2,param_3,(undefined *)0x0,0,0,0,0,0);
  return (undefined4 *)this;
}


// 100172a0 FUN_100172a0

undefined4 * __thiscall FUN_100172a0(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_1002812c;
  *(undefined ***)((int)this + 0x18) = &PTR_FUN_10028108;
  if (*(int *)((int)this + 0x24) != 0) {
    FUN_1000e140(*(int *)((int)this + 0x24));
    *(undefined4 *)((int)this + 0x24) = 0;
  }
  *(undefined ***)this = &PTR_LAB_100280b4;
  if ((param_1 & 1) != 0) {
    FUN_1000e140(this);
  }
  return (undefined4 *)this;
}


// 100172f0 FUN_100172f0

undefined4 * __thiscall
FUN_100172f0(void *this,int param_1,char *param_2,uint param_3,undefined *param_4)

{
  *(undefined1 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_100280e8;
  *(undefined ***)this = &PTR_FUN_1002812c;
  *(undefined ***)((int)this + 0x18) = &PTR_FUN_10028108;
  FUN_10017080(this,param_1,param_2,param_3,param_4,0,0,0,0,0);
  return (undefined4 *)this;
}


// 10017350 FUN_10017350

undefined4 * __thiscall
FUN_10017350(void *this,int param_1,char *param_2,uint param_3,undefined *param_4,undefined1 param_5
            ,int param_6,undefined1 param_7,int param_8,int param_9)

{
  *(undefined1 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_100280e8;
  *(undefined ***)this = &PTR_FUN_1002812c;
  *(undefined ***)((int)this + 0x18) = &PTR_FUN_10028108;
  FUN_10017080(this,param_1,param_2,param_3,param_4,param_5,param_6,param_7,param_8,param_9);
  return (undefined4 *)this;
}


// 100173c0 FUN_100173c0

int * __fastcall FUN_100173c0(int *param_1)

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
  param_1[6] = (int)&PTR_LAB_100280e8;
  param_1[0xe] = 0;
  param_1[0x10] = 0;
  *param_1 = (int)&PTR_FUN_1002812c;
  param_1[6] = (int)&PTR_FUN_10028108;
  param_1[7] = (int)param_1;
  param_1[8] = (int)&DAT_10025720;
  *(undefined1 *)(param_1 + 0xd) = 0;
  *(undefined1 *)(param_1 + 0xf) = 0;
  param_1[0x11] = 0;
  pcVar2 = "0";
  do {
    pcVar3 = pcVar2;
    pcVar2 = pcVar3 + 1;
  } while (*pcVar3 != '\0');
  param_1[10] = (int)(pcVar3 + -0x1002571f);
  _Dst = (void *)FUN_1000dfa0(pcVar3 + -0x1002571f);
  param_1[9] = (int)_Dst;
  _memcpy(_Dst,(void *)param_1[8],param_1[10]);
  dVar5 = _atof((char *)param_1[9]);
  param_1[0xb] = (int)(float)dVar5;
  uVar4 = FUN_10017cc0(extraout_ECX,extraout_EDX);
  puVar1 = PTR_DAT_1003593c;
  param_1[0xc] = (int)uVar4;
  *(undefined1 *)(param_1 + 2) = 0;
  param_1[3] = (int)&DAT_1002536c;
  param_1[4] = (int)puVar1;
  param_1[5] = 2;
  param_1[1] = (int)DAT_100387f0;
  DAT_100387f0 = param_1;
  if (DAT_100387f4 != 0) {
    (**(code **)(*param_1 + 0x2c))();
  }
  *param_1 = (int)&PTR_FUN_100281a4;
  param_1[6] = (int)&PTR_LAB_10028184;
  return param_1;
}


// 100174f0 FUN_100174f0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100174f0(int param_1,int param_2)

{
  int iVar1;
  int iVar2;
  
  if (DAT_1003885c == '\0') {
    iVar2 = 0;
    DAT_1003885c = '\x01';
    iVar1 = DAT_10038854;
    if (0 < param_2) {
      do {
        if (iVar1 == 0) {
          iVar1 = (**(code **)(param_1 + iVar2 * 4))("VEngineCvar007",0);
          _DAT_10038850 = iVar1;
          DAT_10038854 = iVar1;
        }
        if (DAT_10038858 == 0) {
          DAT_10038858 = (**(code **)(param_1 + iVar2 * 4))("VProcessUtils001",0);
          iVar1 = DAT_10038854;
        }
        iVar2 = iVar2 + 1;
      } while (iVar2 < param_2);
    }
  }
  return;
}


// 10017560 FUN_10017560

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10017560(void)

{
  if (DAT_1003885c != '\0') {
    _DAT_10038850 = 0;
    DAT_10038854 = 0;
    DAT_10038858 = 0;
    DAT_1003885c = '\0';
  }
  return;
}


// 10017580 FUN_10017580

uint __fastcall FUN_10017580(uint *param_1)

{
  uint uVar1;
  uint uVar2;
  
  uVar1 = *param_1;
  uVar2 = uVar1 >> 8 & 0xff;
  return *(uint *)(&DAT_100359b8 +
                  (*(uint *)(&DAT_100359b8 +
                            (uVar1 >> 0x10 & 0xff ^
                            *(uint *)(&DAT_100359b8 +
                                     (uVar1 >> 0x18 ^ *(uint *)(&DAT_100359b8 + uVar2 * 4)) * 4)) *
                            4) ^ uVar2) * 4) << 8 |
         *(uint *)(&DAT_100359b8 +
                  (uVar1 & 0xff ^
                  *(uint *)(&DAT_100359b8 +
                           (*(uint *)(&DAT_100359b8 +
                                     (uVar1 >> 0x10 & 0xff ^
                                     *(uint *)(&DAT_100359b8 +
                                              (uVar1 >> 0x18 ^ *(uint *)(&DAT_100359b8 + uVar2 * 4))
                                              * 4)) * 4) ^ uVar2) * 4)) * 4);
}


// 100175f0 FUN_100175f0

undefined4 * __thiscall
FUN_100175f0(void *this,undefined1 param_1,char *param_2,int param_3,int param_4)

{
  int *piVar1;
  char cVar2;
  char *pcVar3;
  int iVar4;
  int iVar5;
  
  *(undefined1 *)((int)this + 4) = param_1;
  *(undefined ***)this = &PTR_FUN_1002823c;
  *(char **)((int)this + 8) = param_2;
  *(int *)((int)this + 0x10) = param_3;
  iVar4 = FUN_100161d0(param_2);
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
      iVar4 = FUN_100161d0(pcVar3);
      *piVar1 = iVar4;
      if (*(int *)((int)this + 0x14) < iVar4) {
        *(int *)((int)this + 0x14) = iVar4;
      }
      iVar5 = iVar5 + 1;
    } while (iVar5 < param_3);
  }
  return (undefined4 *)this;
}


// 10017680 FUN_10017680

uint __thiscall FUN_10017680(void *this,byte *param_1,undefined4 *param_2)

{
  undefined4 uVar1;
  uint in_EAX;
  int iVar2;
  
  iVar2 = 0;
  if (0 < *(int *)((int)this + 0x10)) {
    do {
      in_EAX = FUN_10016200(param_1,*(byte **)((int)this +
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


// 100176f0 FUN_100176f0

undefined1 __thiscall FUN_100176f0(void *this,int param_1)

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


// 10017750 FUN_10017750

uint __thiscall FUN_10017750(void *this,int param_1)

{
  uint in_EAX;
  
  if (((*(byte *)((int)this + 0x14) & 1) == 0) && ((*(byte *)((int)this + 0x15) & 8) == 0)) {
    if ((*(int *)((int)this + 0x10) < *(int *)((int)this + 0x20)) ||
       (in_EAX = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1,
       *(int *)((int)this + 4) < (int)in_EAX)) {
      in_EAX = (**(code **)((int)this + 0x28))(param_1);
      if ((char)in_EAX == '\0') {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 1;
        goto LAB_10017786;
      }
    }
    return CONCAT31((int3)(in_EAX >> 8),1);
  }
LAB_10017786:
  return in_EAX & 0xffffff00;
}


// 100177a0 FUN_100177a0

void __fastcall FUN_100177a0(int *param_1)

{
  undefined4 uVar1;
  
  if (param_1[7] < param_1[4]) {
    if (((*(byte *)((int)param_1 + 0x15) & 8) == 0) && ((*(byte *)(param_1 + 5) & 1) == 0)) {
      uVar1 = FUN_10017750(param_1,1);
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


// 100177f0 FUN_100177f0

void __thiscall FUN_100177f0(void *this,undefined4 param_1)

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


// 10017840 FUN_10017840

void __thiscall FUN_10017840(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1001789b;
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
LAB_1001789b:
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


// 100178d0 FUN_100178d0

void __thiscall FUN_100178d0(void *this,void *param_1,size_t param_2)

{
  char cVar1;
  
  cVar1 = FUN_100176f0(this,param_2);
  if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    _memcpy(param_1,(void *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this),
            param_2);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
  }
  return;
}


// 10017910 FUN_10017910

uint __thiscall FUN_10017910(void *this,int param_1)

{
  uint in_EAX;
  int iVar1;
  
  if (*(int *)((int)this + 8) < 0) {
    if ((*(byte *)((int)this + 0x15) & 2) == 0) {
      return in_EAX & 0xffffff00;
    }
    FUN_100177f0(this,0);
  }
  iVar1 = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1;
  if (*(int *)((int)this + 4) < iVar1) {
    do {
      iVar1 = FUN_10017840(this,1);
    } while (*(int *)((int)this + 4) <
             (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1);
  }
  return CONCAT31((int3)((uint)iVar1 >> 8),1);
}


// 10017960 FUN_10017960

void __thiscall FUN_10017960(void *this,int param_1,int param_2)

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
      uVar1 = FUN_10017750(this,1);
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


// 100179e0 FUN_100179e0

int * __thiscall FUN_100179e0(void *this,int param_1,int param_2,byte param_3)

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
    FUN_100177a0((int *)this);
  }
  *(undefined1 **)((int)this + 0x24) = &LAB_100174c0;
  *(code **)((int)this + 0x28) = FUN_10017910;
  return (int *)this;
}


// 10017a60 FUN_10017a60

int * __thiscall FUN_10017a60(void *this,int param_1,int param_2,byte param_3)

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
      uVar1 = FUN_10017750(this,1);
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
  *(undefined1 **)((int)this + 0x24) = &LAB_100174c0;
  *(code **)((int)this + 0x28) = FUN_10017910;
  return (int *)this;
}


// 10017ae0 FUN_10017ae0

void __thiscall FUN_10017ae0(void *this,void *param_1,int param_2)

{
  size_t _Size;
  void *_Dst;
  
  if (param_1 == (void *)0x0) {
    param_2 = 0;
  }
  *(int *)((int)this + 0xc) = param_2;
  if (*(int *)((int)this + 4) < param_2) {
    FUN_10017840(this,param_2 - *(int *)((int)this + 4));
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


// 10017b50 FUN_10017b50

void __thiscall FUN_10017b50(void *this,char *param_1)

{
  int iVar1;
  
  if (param_1 != (char *)0x0) {
    iVar1 = FUN_100161d0(param_1);
    FUN_10017ae0(this,param_1,iVar1 + 1);
    return;
  }
  FUN_10017ae0(this,(void *)0x0,0);
  return;
}


// 10017b90 FUN_10017b90

undefined4 __fastcall FUN_10017b90(undefined4 *param_1)

{
  if (param_1[3] == 0) {
    param_1[3] = 1;
    if ((int)param_1[1] < 1) {
      FUN_10017840(param_1,1 - param_1[1]);
      if ((int)param_1[1] < 1) {
        param_1[3] = param_1[1];
      }
    }
    *(undefined1 *)*param_1 = 0;
  }
  return *param_1;
}


// 10017bd0 FUN_10017bd0

void __fastcall FUN_10017bd0(undefined4 *param_1)

{
  *param_1 = 0;
  param_1[1] = 0;
  param_1[2] = 0;
  param_1[3] = 0;
  return;
}


// 10017be0 FUN_10017be0

void FUN_10017be0(void)

{
  return;
}


// 10017be1 __cfltcvt_init

/* Library Function - Single Match
    __cfltcvt_init
   
   Library: Visual Studio 2005 Release */

void __cfltcvt_init(void)

{
  PTR_LAB_10035ed0 = __cfltcvt;
  PTR_LAB_10035ed4 = &LAB_100197c0;
  PTR_LAB_10035ed8 = &LAB_1001977e;
  PTR_LAB_10035edc = &LAB_100197b2;
  PTR_LAB_10035ee0 = &LAB_10019728;
  PTR_LAB_10035ee4 = __cfltcvt;
  PTR_LAB_10035ee8 = __cfltcvt_l;
  PTR_LAB_10035eec = __fassign_l;
  PTR_LAB_10035ef0 = __cropzeros_l;
  PTR_LAB_10035ef4 = __forcdecpt_l;
  return;
}


// 10017c41 __fpmath

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __fpmath
   
   Library: Visual Studio 2005 Release */

void __cdecl __fpmath(int param_1)

{
  __cfltcvt_init();
  _DAT_10039b9c = __ms_p5_mp_test_fdiv();
  if (param_1 != 0) {
    __setdefaultprecision();
  }
  return;
}


// 10017c5f type_info::~type_info

/* Library Function - Single Match
    public: virtual __thiscall type_info::~type_info(void)
   
   Library: Visual Studio 2005 Release */

void __thiscall type_info::~type_info(type_info *this)

{
  *(undefined ***)this = &PTR__scalar_deleting_destructor__10028264;
  _Type_info_dtor(this);
  return;
}


// 10017c6d type_info::`scalar_deleting_destructor'

/* Library Function - Single Match
    public: virtual void * __thiscall type_info::`scalar deleting destructor'(unsigned int)
   
   Library: Visual Studio 2005 Release */

void * __thiscall type_info::_scalar_deleting_destructor_(type_info *this,uint param_1)

{
  ~type_info(this);
  if ((param_1 & 1) != 0) {
    FUN_1000e140(this);
  }
  return this;
}


// 10017cc0 FUN_10017cc0

ulonglong __fastcall FUN_10017cc0(undefined4 param_1,undefined4 param_2)

{
  ulonglong uVar1;
  uint uVar2;
  float fVar3;
  float10 in_ST0;
  uint local_20;
  float fStack_1c;
  
  if (DAT_1003b7a0 == 0) {
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


// 10017d70 _memset

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
  if ((((char)_Val == '\0') && (0xff < _Size)) && (DAT_1003b7a0 != 0)) {
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


// 10017dea FUN_10017dea

int __cdecl FUN_10017dea(int param_1)

{
  int *piVar1;
  int *piVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  
  piVar1 = (int *)FUN_1001a665(DAT_1003b790);
  piVar2 = (int *)FUN_1001a665(DAT_1003b78c);
  if (piVar1 <= piVar2) {
    uVar4 = ((int)piVar2 - (int)piVar1) + 4;
    if (3 < uVar4) {
      uVar3 = FUN_1000e110(piVar1);
      if (uVar3 < uVar4) {
        uVar4 = 0x800;
        if (uVar3 < 0x800) {
          uVar4 = uVar3;
        }
        if ((uVar4 + uVar3 < uVar3) ||
           (iVar5 = thunk_FUN_1000e080(piVar1,uVar4 + uVar3), iVar5 == 0)) {
          if (uVar3 + 0x10 < uVar3) {
            return 0;
          }
          iVar5 = thunk_FUN_1000e080(piVar1,uVar3 + 0x10);
          if (iVar5 == 0) {
            return 0;
          }
        }
        piVar2 = (int *)(iVar5 + ((int)piVar2 - (int)piVar1 >> 2) * 4);
        DAT_1003b790 = FUN_1001a5ee(iVar5);
      }
      iVar5 = FUN_1001a5ee(param_1);
      *piVar2 = iVar5;
      DAT_1003b78c = FUN_1001a5ee((int)(piVar2 + 1));
      return param_1;
    }
  }
  return 0;
}


// 10017ed2 __onexit

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __onexit
   
   Library: Visual Studio 2005 Release */

_onexit_t __cdecl __onexit(_onexit_t _Func)

{
  _onexit_t p_Var1;
  
  FUN_1001ad0a();
  p_Var1 = (_onexit_t)FUN_10017dea((int)_Func);
  FUN_10017f08();
  return p_Var1;
}


// 10017f08 FUN_10017f08

void FUN_10017f08(void)

{
  FUN_1001ad13();
  return;
}


// 10017f0e _atexit

/* Library Function - Single Match
    _atexit
   
   Library: Visual Studio 2005 Release */

int __cdecl _atexit(_func_4879 *param_1)

{
  _onexit_t p_Var1;
  
  p_Var1 = __onexit((_onexit_t)param_1);
  return (p_Var1 != (_onexit_t)0x0) - 1;
}


// 10017f20 _memcpy

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
          goto switchD_10018103_caseD_2;
        case 3:
          goto switchD_10018103_caseD_3;
        }
        goto switchD_10018103_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_10018103_caseD_0;
      case 1:
        goto switchD_10018103_caseD_1;
      case 2:
        goto switchD_10018103_caseD_2;
      case 3:
        goto switchD_10018103_caseD_3;
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
              goto switchD_10018103_caseD_2;
            case 3:
              goto switchD_10018103_caseD_3;
            }
            goto switchD_10018103_caseD_1;
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
              goto switchD_10018103_caseD_2;
            case 3:
              goto switchD_10018103_caseD_3;
            }
            goto switchD_10018103_caseD_1;
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
              goto switchD_10018103_caseD_2;
            case 3:
              goto switchD_10018103_caseD_3;
            }
            goto switchD_10018103_caseD_1;
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
switchD_10018103_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_10018103_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_10018103_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_10018103_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_1003b7a0 != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_10017f7c_caseD_2;
      case 3:
        goto switchD_10017f7c_caseD_3;
      }
      goto switchD_10017f7c_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_10017f7c_caseD_0;
    case 1:
      goto switchD_10017f7c_caseD_1;
    case 2:
      goto switchD_10017f7c_caseD_2;
    case 3:
      goto switchD_10017f7c_caseD_3;
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
            goto switchD_10017f7c_caseD_2;
          case 3:
            goto switchD_10017f7c_caseD_3;
          }
          goto switchD_10017f7c_caseD_1;
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
            goto switchD_10017f7c_caseD_2;
          case 3:
            goto switchD_10017f7c_caseD_3;
          }
          goto switchD_10017f7c_caseD_1;
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
            goto switchD_10017f7c_caseD_2;
          case 3:
            goto switchD_10017f7c_caseD_3;
          }
          goto switchD_10017f7c_caseD_1;
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
switchD_10017f7c_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_10017f7c_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_10017f7c_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_10017f7c_caseD_0:
  return _Dst;
}


// 10018290 _strrchr

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


// 100182bd __vsnprintf_l

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
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
    iVar2 = FUN_1001b511(&local_24,(byte *)_Format,_Locale,(wchar_t *)_ArgList);
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
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  return iVar2;
}


// 1001836e __vsnprintf

/* Library Function - Single Match
    __vsnprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl __vsnprintf(char *_Dest,size_t _Count,char *_Format,va_list _Args)

{
  int iVar1;
  
  iVar1 = __vsnprintf_l(_Dest,_Count,_Format,(_locale_t)0x0,_Args);
  return iVar1;
}


// 10018390 __alloca_probe_16

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


// 100183a6 __alloca_probe_8

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


// 100184e1 __CRT_INIT@12

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
        _DAT_10039efc = DVar2 * 0x100 + DVar3;
        DAT_10039ef4 = DVar1;
        _DAT_10039ef8 = uVar10;
        DAT_10039f00 = DVar2;
        _DAT_10039f04 = DVar3;
        bVar4 = FUN_1000e130();
        if (CONCAT31(extraout_var,bVar4) != 0) {
          iVar7 = __mtinit();
          if (iVar7 != 0) {
            __RTC_Initialize();
            DAT_1003b7a4 = GetCommandLineA();
            DAT_10039ba4 = ___crtGetEnvironmentStringsA();
            iVar7 = __ioinit();
            if (-1 < iVar7) {
              iVar7 = __setargv();
              if (((-1 < iVar7) && (iVar7 = __setenvp(), -1 < iVar7)) &&
                 (iVar7 = __cinit(0), iVar7 == 0)) {
                DAT_10039ba0 = DAT_10039ba0 + 1;
                return 1;
              }
              __ioterm();
            }
            __mtterm();
          }
          FUN_10015190();
        }
      }
    }
  }
  else if (param_2 == 0) {
    if (0 < DAT_10039ba0) {
      DAT_10039ba0 = DAT_10039ba0 + -1;
      if (DAT_10039f30 == 0) {
        __cexit();
      }
      if (param_3 == 0) {
        __ioterm();
        __mtterm();
        FUN_10015190();
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
    FUN_1001a6dc();
    pDVar8 = (DWORD *)FUN_1000e0e0(1,0x214);
    if (pDVar8 != (DWORD *)0x0) {
      uVar12 = DAT_10035fc0;
      pDVar13 = pDVar8;
      pcVar9 = (code *)FUN_1001a665(DAT_10039edc);
      iVar7 = (*pcVar9)(uVar12,pDVar13);
      if (iVar7 != 0) {
        FUN_1001a74b((int)pDVar8,0);
        DVar11 = GetCurrentThreadId();
        pDVar8[1] = 0xffffffff;
        *pDVar8 = DVar11;
        return 1;
      }
      FUN_1000e140(pDVar8);
    }
  }
  return 0;
}


// 100186ba ___DllMainCRTStartup

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x1001874c) */
/* WARNING: Removing unreachable block (ram,0x100186f9) */
/* WARNING: Removing unreachable block (ram,0x10018779) */
/* Library Function - Single Match
    ___DllMainCRTStartup
   
   Library: Visual Studio 2005 Release */

int __fastcall ___DllMainCRTStartup(int param_1,int param_2,undefined4 param_3)

{
  int iVar1;
  int local_20;
  
  if (((param_2 == 0) && (DAT_10039ba0 == 0)) ||
     (((param_2 == 1 || (param_2 == 2)) &&
      (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)))) {
    local_20 = 0;
  }
  else {
    local_20 = FUN_1001cc7d();
    if ((param_2 == 1) && (local_20 == 0)) {
      FUN_1001cc7d();
      __CRT_INIT_12(param_3,0,param_1);
    }
    if (((param_2 == 0) || (param_2 == 3)) &&
       (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)) {
      local_20 = 0;
    }
  }
  return local_20;
}


// 100187b0 entry

void entry(undefined4 param_1,int param_2,int param_3)

{
  if (param_2 == 1) {
    ___security_init_cookie();
  }
  ___DllMainCRTStartup(param_3,param_2,param_1);
  return;
}


// 100187f6 FUN_100187f6

uint * __cdecl FUN_100187f6(uint *param_1)

{
  uint uVar1;
  char cVar2;
  uint in_EAX;
  uint uVar3;
  uint uVar4;
  uint *puVar5;
  char cVar6;
  uint uVar7;
  
  cVar6 = (char)in_EAX;
  uVar7 = (uint)param_1 & 3;
  while (uVar7 != 0) {
    if ((char)*param_1 == cVar6) {
      return param_1;
    }
    if ((char)*param_1 == '\0') {
      return (uint *)0x0;
    }
    uVar7 = (uint)((int)param_1 + 1) & 3;
    param_1 = (uint *)((int)param_1 + 1);
  }
  uVar7 = in_EAX | in_EAX << 8;
  while( true ) {
    while( true ) {
      uVar1 = *param_1;
      uVar4 = uVar1 ^ (uVar7 << 0x10 | uVar7);
      uVar3 = uVar1 ^ 0xffffffff ^ uVar1 + 0x7efefeff;
      puVar5 = param_1 + 1;
      if (((uVar4 ^ 0xffffffff ^ uVar4 + 0x7efefeff) & 0x81010100) != 0) break;
      param_1 = puVar5;
      if ((uVar3 & 0x81010100) != 0) {
        if ((uVar3 & 0x1010100) != 0) {
          return (uint *)0x0;
        }
        if ((uVar1 + 0x7efefeff & 0x80000000) == 0) {
          return (uint *)0x0;
        }
      }
    }
    uVar1 = *param_1;
    if ((char)uVar1 == cVar6) {
      return param_1;
    }
    if ((char)uVar1 == '\0') {
      return (uint *)0x0;
    }
    cVar2 = (char)(uVar1 >> 8);
    if (cVar2 == cVar6) {
      return (uint *)((int)param_1 + 1);
    }
    if (cVar2 == '\0') break;
    cVar2 = (char)(uVar1 >> 0x10);
    if (cVar2 == cVar6) {
      return (uint *)((int)param_1 + 2);
    }
    if (cVar2 == '\0') {
      return (uint *)0x0;
    }
    cVar2 = (char)(uVar1 >> 0x18);
    if (cVar2 == cVar6) {
      return (uint *)((int)param_1 + 3);
    }
    param_1 = puVar5;
    if (cVar2 == '\0') {
      return (uint *)0x0;
    }
  }
  return (uint *)0x0;
}


// 100188b0 _memmove

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
          goto switchD_10018a93_caseD_2;
        case 3:
          goto switchD_10018a93_caseD_3;
        }
        goto switchD_10018a93_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_10018a93_caseD_0;
      case 1:
        goto switchD_10018a93_caseD_1;
      case 2:
        goto switchD_10018a93_caseD_2;
      case 3:
        goto switchD_10018a93_caseD_3;
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
              goto switchD_10018a93_caseD_2;
            case 3:
              goto switchD_10018a93_caseD_3;
            }
            goto switchD_10018a93_caseD_1;
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
              goto switchD_10018a93_caseD_2;
            case 3:
              goto switchD_10018a93_caseD_3;
            }
            goto switchD_10018a93_caseD_1;
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
              goto switchD_10018a93_caseD_2;
            case 3:
              goto switchD_10018a93_caseD_3;
            }
            goto switchD_10018a93_caseD_1;
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
switchD_10018a93_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_10018a93_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_10018a93_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_10018a93_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_1003b7a0 != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_1001890c_caseD_2;
      case 3:
        goto switchD_1001890c_caseD_3;
      }
      goto switchD_1001890c_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_1001890c_caseD_0;
    case 1:
      goto switchD_1001890c_caseD_1;
    case 2:
      goto switchD_1001890c_caseD_2;
    case 3:
      goto switchD_1001890c_caseD_3;
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
            goto switchD_1001890c_caseD_2;
          case 3:
            goto switchD_1001890c_caseD_3;
          }
          goto switchD_1001890c_caseD_1;
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
            goto switchD_1001890c_caseD_2;
          case 3:
            goto switchD_1001890c_caseD_3;
          }
          goto switchD_1001890c_caseD_1;
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
            goto switchD_1001890c_caseD_2;
          case 3:
            goto switchD_1001890c_caseD_3;
          }
          goto switchD_1001890c_caseD_1;
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
switchD_1001890c_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_1001890c_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_1001890c_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_1001890c_caseD_0:
  return _Dst;
}


// 10018c20 FUN_10018c20

uint * __cdecl FUN_10018c20(uint *param_1,char *param_2)

{
  char *pcVar1;
  char *pcVar2;
  char cVar3;
  uint *puVar4;
  uint *puVar5;
  char *pcVar6;
  
  if (*param_2 == '\0') {
    return param_1;
  }
  if (param_2[1] == '\0') {
    puVar5 = FUN_100187f6(param_1);
    return puVar5;
  }
  do {
    cVar3 = (char)*param_1;
    do {
      while (puVar5 = param_1, param_1 = (uint *)((int)puVar5 + 1), cVar3 != *param_2) {
        if (cVar3 == '\0') {
          return (uint *)0x0;
        }
        cVar3 = *(char *)param_1;
      }
      cVar3 = *(char *)param_1;
      pcVar6 = param_2;
      puVar4 = puVar5;
    } while (cVar3 != param_2[1]);
    do {
      if (pcVar6[2] == '\0') {
        return puVar5;
      }
      if (*(char *)((int)puVar4 + 2) != pcVar6[2]) break;
      pcVar1 = pcVar6 + 3;
      if (*pcVar1 == '\0') {
        return puVar5;
      }
      pcVar2 = (char *)((int)puVar4 + 3);
      pcVar6 = pcVar6 + 2;
      puVar4 = (uint *)((int)puVar4 + 2);
    } while (*pcVar1 == *pcVar2);
  } while( true );
}


// 10018ca6 __freea

/* Library Function - Single Match
    __freea
   
   Library: Visual Studio 2005 Release */

void __cdecl __freea(void *_Memory)

{
  if ((_Memory != (void *)0x0) && (*(int *)((int)_Memory + -8) == 0xdddd)) {
    FUN_1000e140((int *)((int)_Memory + -8));
  }
  return;
}


// 10018cc1 _LocaleUpdate::_LocaleUpdate

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
    if ((*(undefined **)this != PTR_DAT_10036938) && ((p_Var2->_ownlocale & DAT_10036854) == 0)) {
      ptVar3 = ___updatetlocinfo();
      *(pthreadlocinfo *)this = ptVar3;
    }
    if ((*(undefined **)(this + 4) != PTR_DAT_10036758) &&
       ((*(uint *)(*(int *)(this + 8) + 0x70) & DAT_10036854) == 0)) {
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


// 10018d43 __tolower_l

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
LAB_10018da2:
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
      pDVar4 = FUN_1001c00a();
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
    if (iVar3 == 0) goto LAB_10018da2;
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


// 10018e5a _tolower

/* Library Function - Single Match
    _tolower
   
   Library: Visual Studio 2005 Release */

int __cdecl _tolower(int _C)

{
  if (DAT_1003a1bc == 0) {
    if (_C - 0x41U < 0x1a) {
      return _C + 0x20;
    }
  }
  else {
    _C = __tolower_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 10018e90 _strncat

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
      if ((byte)uVar4 == 0) goto LAB_10018eed;
      uVar4 = (uint)puVar5 & 3;
    }
    do {
      do {
        puVar6 = puVar5;
        puVar5 = puVar6 + 1;
      } while (((*puVar6 ^ 0xffffffff ^ *puVar6 + 0x7efefeff) & 0x81010100) == 0);
      uVar4 = *puVar6;
      if ((char)uVar4 == '\0') goto LAB_10018eff;
      if ((char)(uVar4 >> 8) == '\0') {
        puVar6 = (uint *)((int)puVar6 + 1);
        goto LAB_10018eff;
      }
      if ((uVar4 & 0xff0000) == 0) {
        puVar6 = (uint *)((int)puVar6 + 2);
        goto LAB_10018eff;
      }
    } while ((uVar4 & 0xff000000) != 0);
LAB_10018eed:
    puVar6 = (uint *)((int)puVar5 + -1);
LAB_10018eff:
    if (((uint)_Source & 3) == 0) {
      uVar3 = _Count >> 2;
    }
    else {
      do {
        bVar1 = (byte)*(uint *)_Source;
        uVar4 = (uint)bVar1;
        _Source = (char *)((int)_Source + 1);
        if (bVar1 == 0) goto LAB_10018f5a;
        *(byte *)puVar6 = bVar1;
        puVar6 = (uint *)((int)puVar6 + 1);
        _Count = _Count - 1;
        if (_Count == 0) goto LAB_10018f50;
      } while (((uint)_Source & 3) != 0);
      uVar3 = _Count >> 2;
    }
    for (; uVar3 != 0; uVar3 = uVar3 - 1) {
      uVar2 = *(uint *)_Source;
      uVar4 = *(uint *)_Source;
      _Source = (char *)((int)_Source + 4);
      if (((uVar2 ^ 0xffffffff ^ uVar2 + 0x7efefeff) & 0x81010100) != 0) {
        if ((char)uVar4 == '\0') {
LAB_10018f5a:
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
LAB_10018f50:
    *(byte *)puVar6 = (byte)_Count;
  }
  return _Dest;
}


// 10018fd0 _strncpy

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
        goto joined_r0x1001901c;
      }
    }
    do {
      if (((uint)puVar5 & 3) == 0) {
        uVar4 = _Count >> 2;
        cVar3 = '\0';
        if (uVar4 == 0) goto LAB_10019063;
        goto LAB_100190d9;
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
joined_r0x100190d5:
          while( true ) {
            uVar4 = uVar4 - 1;
            puVar5 = puVar5 + 1;
            if (uVar4 == 0) break;
LAB_100190d9:
            *puVar5 = 0;
          }
          cVar3 = '\0';
          _Count = _Count & 3;
          if (_Count != 0) goto LAB_10019063;
          return _Dest;
        }
        if ((char)(uVar2 >> 8) == '\0') {
          *puVar5 = uVar2 & 0xff;
          goto joined_r0x100190d5;
        }
        if ((uVar2 & 0xff0000) == 0) {
          *puVar5 = uVar2 & 0xffff;
          goto joined_r0x100190d5;
        }
        if ((uVar2 & 0xff000000) == 0) {
          *puVar5 = uVar2;
          goto joined_r0x100190d5;
        }
      }
      *puVar5 = uVar2;
      puVar5 = puVar5 + 1;
      uVar4 = uVar4 - 1;
joined_r0x1001901c:
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
LAB_10019063:
        *(char *)puVar5 = cVar3;
        puVar5 = (uint *)((int)puVar5 + 1);
      }
      return _Dest;
    }
    _Count = _Count - 1;
  } while (_Count != 0);
  return _Dest;
}


// 1001926a _atol

/* Library Function - Single Match
    _atol
   
   Library: Visual Studio 2005 Release */

long __cdecl _atol(char *_Str)

{
  long lVar1;
  
  lVar1 = _strtol(_Str,(char **)0x0,10);
  return lVar1;
}


// 1001927b __atof_l

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
    pDVar2 = FUN_1001c00a();
    *pDVar2 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10019324 _atof

/* Library Function - Single Match
    _atof
   
   Library: Visual Studio 2005 Release */

double __cdecl _atof(char *_String)

{
  double dVar1;
  
  dVar1 = __atof_l(_String,(_locale_t)0x0);
  return dVar1;
}


// 10019340 __alloca_probe

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


// 1001936b __isdigit_l

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


// 100193ba _isdigit

/* Library Function - Single Match
    _isdigit
   
   Library: Visual Studio 2005 Release */

int __cdecl _isdigit(int _C)

{
  int iVar1;
  
  if (DAT_1003a1bc == 0) {
    return (byte)PTR_DAT_10036928[_C * 2] & 4;
  }
  iVar1 = __isdigit_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 100193e3 strtoxl

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
    pDVar3 = FUN_1001c00a();
    *pDVar3 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
LAB_1001949a:
    bVar8 = *pbVar9;
    pbVar9 = pbVar2 + 2;
  }
  else if (bVar8 == 0x2b) goto LAB_1001949a;
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
      goto LAB_10019500;
    }
    if ((*pbVar9 != 0x78) && (*pbVar9 != 0x58)) {
      param_4 = 8;
      goto LAB_10019500;
    }
    param_4 = 0x10;
  }
  else if ((param_4 != 0x10) || (bVar8 != 0x30)) goto LAB_10019500;
  if ((*pbVar9 == 0x78) || (*pbVar9 == 0x58)) {
    bVar8 = pbVar9[1];
    pbVar9 = pbVar9 + 2;
  }
LAB_10019500:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  do {
    uVar1 = *(ushort *)(ptVar5[1].lc_category[0].locale + (uint)bVar8 * 2);
    if ((uVar1 & 4) == 0) {
      if ((uVar1 & 0x103) == 0) {
LAB_1001955a:
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
          pDVar3 = FUN_1001c00a();
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
    if ((uint)param_4 <= uVar6) goto LAB_1001955a;
    if ((local_8 < uVar4) ||
       ((local_8 == uVar4 && (uVar6 <= (uint)(0xffffffff % (ulonglong)(uint)param_4))))) {
      local_8 = local_8 * param_4 + uVar6;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (char **)0x0) goto LAB_1001955a;
    }
    bVar8 = *pbVar9;
    pbVar9 = pbVar9 + 1;
  } while( true );
}


// 1001960e _strtol

/* Library Function - Single Match
    _strtol
   
   Library: Visual Studio 2005 Release */

long __cdecl _strtol(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_1003a1bc == 0) {
    ppuVar2 = &PTR_DAT_10036940;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 10019637 __forcdecpt_l

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


// 100196a8 __cropzeros_l

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


// 1001973e __fassign_l

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


// 10019795 __shift

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


// 100197ce __cftoe2_l

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
    pDVar1 = FUN_1001c00a();
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
      if (((DAT_1003a1e4 & 1) != 0) && (_Dst[2] == '0')) {
        pcVar9 = (char *)0x3;
        _memmove(_Dst + 2,_Dst + 3,3);
      }
      if (local_8 == '\0') {
        return (DWORD)pcVar9;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return (DWORD)pcVar9;
    }
    pDVar1 = FUN_1001c00a();
    DVar7 = 0x22;
  }
  DVar8 = 0;
  *pDVar1 = DVar7;
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return DVar8;
}


// 1001993b __cftoe_l

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10019a09 __cftoe

/* Library Function - Single Match
    __cftoe
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __cftoe(double *_Value,char *_Buf,size_t _SizeInBytes,int _Dec,int _Caps)

{
  errno_t eVar1;
  
  eVar1 = __cftoe_l(_Value,_Buf,_SizeInBytes,_Dec,_Caps,(localeinfo_struct *)0x0);
  return eVar1;
}


// 10019a27 __cftoa_l

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
    pDVar2 = FUN_1001c00a();
    DVar15 = 0x16;
LAB_10019a60:
    DVar16 = 0;
    *pDVar2 = DVar15;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_1c != '\0') {
      *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
    }
    return DVar16;
  }
  *param_2 = 0;
  if (param_3 <= param_4 + 0xb) {
    pDVar2 = FUN_1001c00a();
    DVar15 = 0x22;
    goto LAB_10019a60;
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
    goto LAB_10019d84;
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
LAB_10019d33:
    if ((-1 < (int)uVar8) && ((0 < (int)uVar8 || (99 < uVar7)))) goto LAB_10019d3e;
  }
  else {
    uVar14 = __alldvrm(uVar7,uVar8,1000,0);
    local_14 = (undefined4)((ulonglong)uVar14 >> 0x20);
    *pcVar10 = (char)uVar14 + '0';
    pcVar11 = pcVar4 + 3;
    uVar8 = 0;
    uVar7 = extraout_ECX;
    if (pcVar11 == pcVar10) goto LAB_10019d33;
LAB_10019d3e:
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
LAB_10019d84:
  if (local_1c == '\0') {
    return unaff_EBX;
  }
  *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  return unaff_EBX;
}


// 10019d98 __cftof2_l

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
    pDVar2 = FUN_1001c00a();
    uVar4 = 0x16;
    *pDVar2 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10019e8d __cftof_l

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10019f46 __cftog_l

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_34,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar2 = FUN_1001c00a();
    *pDVar2 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 1001a03e __cfltcvt_l

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


// 1001a0c4 __cfltcvt

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


// 1001a0e5 __initp_misc_cfltcvt_tab

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
    piVar1 = (int *)((int)&PTR_LAB_10035ed0 + uVar3);
    iVar2 = FUN_1001a5ee(*piVar1);
    uVar3 = uVar3 + 4;
    *piVar1 = iVar2;
  } while (uVar3 < 0x28);
  return;
}


// 1001a104 __setdefaultprecision

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


// 1001a12d __ms_p5_test_fdiv

/* WARNING: Removing unreachable block (ram,0x1001a160) */
/* Library Function - Single Match
    __ms_p5_test_fdiv
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

undefined4 __ms_p5_test_fdiv(void)

{
  return 0;
}


// 1001a169 __ms_p5_mp_test_fdiv

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


// 1001a192 type_info::_Type_info_dtor

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
  piVar1 = DAT_10039bb8;
  if (*(int *)(param_1 + 4) != 0) {
    piVar2 = (int *)&DAT_10039bb4;
    do {
      piVar3 = piVar2;
      if (DAT_10039bb8 == (int *)0x0) goto LAB_1001a1d6;
      piVar2 = DAT_10039bb8;
    } while (*DAT_10039bb8 != *(int *)(param_1 + 4));
    piVar3[1] = DAT_10039bb8[1];
    FUN_1000e140(piVar1);
LAB_1001a1d6:
    FUN_1000e140(*(undefined4 *)(param_1 + 4));
    *(undefined4 *)(param_1 + 4) = 0;
  }
  FUN_1001a1f9();
  return;
}


// 1001a1f9 FUN_1001a1f9

void FUN_1001a1f9(void)

{
  FUN_1001cdb5(0xe);
  return;
}


// 1001a210 _strcmp

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
      if (bVar4 != *_Str2) goto LAB_1001a254;
      _Str2 = _Str2 + 1;
      if (bVar4 == 0) {
        return 0;
      }
      if (((uint)_Str1 & 2) == 0) goto LAB_1001a220;
    }
    uVar1 = *(undefined2 *)_Str1;
    _Str1 = _Str1 + 2;
    bVar4 = (byte)uVar1;
    bVar5 = bVar4 < (byte)*_Str2;
    if (bVar4 != *_Str2) goto LAB_1001a254;
    if (bVar4 == 0) {
      return 0;
    }
    bVar4 = (byte)((ushort)uVar1 >> 8);
    bVar5 = bVar4 < ((byte *)_Str2)[1];
    if (bVar4 != ((byte *)_Str2)[1]) goto LAB_1001a254;
    if (bVar4 == 0) {
      return 0;
    }
    _Str2 = (char *)((byte *)_Str2 + 2);
  }
LAB_1001a220:
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
LAB_1001a254:
  return (uint)bVar5 * -2 + 1;
}


// 1001a298 _abort

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
    FUN_1001a3a9(10);
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


// 1001a38b FUN_1001a38b

void __cdecl FUN_1001a38b(uint param_1,uint param_2)

{
  DAT_10035f00 = ~param_2 & DAT_10035f00 | param_1 & param_2;
  return;
}


// 1001a3a9 FUN_1001a3a9

void __cdecl FUN_1001a3a9(int param_1)

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
    if (param_1 == (&DAT_10035f08)[uVar6 * 2]) break;
    uVar6 = uVar6 + 1;
  } while (uVar6 < 0x17);
  if (uVar6 < 0x17) {
    iVar2 = __set_error_mode(3);
    if ((iVar2 == 1) || ((iVar2 = __set_error_mode(3), iVar2 == 0 && (DAT_10039bb0 == 1)))) {
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
      eVar3 = _strcpy_s(&DAT_10039bc0,0x314,"Runtime Error!\n\nProgram: ");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      DAT_10039cdd = 0;
      DVar4 = GetModuleFileNameA((HMODULE)0x0,&DAT_10039bd9,0x104);
      if ((DVar4 == 0) &&
         (eVar3 = _strcpy_s(&DAT_10039bd9,0x2fb,"<program name unknown>"), eVar3 != 0)) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      sVar5 = _strlen(&DAT_10039bd9);
      if (0x3c < sVar5 + 1) {
        sVar5 = _strlen(&DAT_10039bd9);
        eVar3 = _strncpy_s((char *)(sVar5 + 0x10039b9e),
                           (int)&DAT_10039ed4 - (int)(sVar5 + 0x10039b9e),"...",3);
        if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
      }
      eVar3 = _strcat_s(&DAT_10039bc0,0x314,"\n\n");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      eVar3 = _strcat_s(&DAT_10039bc0,0x314,*(char **)(uVar6 * 8 + 0x10035f0c));
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      FUN_1001ecac(&DAT_10039bc0,"Microsoft Visual C++ Runtime Library",0x12010);
    }
  }
  return;
}


// 1001a549 __FF_MSGBANNER

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
    if (DAT_10039bb0 != 1) {
      return;
    }
  }
  FUN_1001a3a9(0xfc);
  FUN_1001a3a9(0xff);
  return;
}


// 1001a582 FUN_1001a582

undefined4 FUN_1001a582(void)

{
  int iVar1;
  HMODULE pHVar2;
  int iVar3;
  uint uVar4;
  char *_Str2;
  int local_8;
  
  uVar4 = 0;
  local_8 = 0;
  FUN_1001ad8b(&local_8);
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


// 1001a5ee FUN_1001a5ee

int __cdecl FUN_1001a5ee(int param_1)

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
      goto LAB_1001a648;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_1001a582();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"EncodePointer");
LAB_1001a648:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 1001a65c __encoded_null

/* Library Function - Single Match
    __encoded_null
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __encoded_null(void)

{
  FUN_1001a5ee(0);
  return;
}


// 1001a665 FUN_1001a665

int __cdecl FUN_1001a665(int param_1)

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
      goto LAB_1001a6bf;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_1001a582();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"DecodePointer");
LAB_1001a6bf:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 1001a6dc FUN_1001a6dc

LPVOID FUN_1001a6dc(void)

{
  LPVOID lpTlsValue;
  
  lpTlsValue = TlsGetValue(DAT_10035fc4);
  if (lpTlsValue == (LPVOID)0x0) {
    lpTlsValue = (LPVOID)FUN_1001a665(DAT_10039ed8);
    TlsSetValue(DAT_10035fc4,lpTlsValue);
  }
  return lpTlsValue;
}


// 1001a70e __mtterm

/* Library Function - Single Match
    __mtterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __mtterm(void)

{
  code *pcVar1;
  int iVar2;
  
  if (DAT_10035fc0 != -1) {
    iVar2 = DAT_10035fc0;
    pcVar1 = (code *)FUN_1001a665(DAT_10039ee0);
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


// 1001a74b FUN_1001a74b

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

void __cdecl FUN_1001a74b(int param_1,int param_2)

{
  HMODULE hModule;
  int iVar1;
  FARPROC pFVar2;
  
  hModule = GetModuleHandleA("KERNEL32.DLL");
  *(undefined **)(param_1 + 0x5c) = &DAT_10036188;
  *(undefined4 *)(param_1 + 0x14) = 1;
  if (hModule != (HMODULE)0x0) {
    iVar1 = FUN_1001a582();
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
  *(undefined **)(param_1 + 0x68) = &DAT_10036330;
  InterlockedIncrement((LONG *)&DAT_10036330);
  __lock(0xc);
  *(int *)(param_1 + 0x6c) = param_2;
  if (param_2 == 0) {
    *(undefined **)(param_1 + 0x6c) = PTR_DAT_10036938;
  }
  ___addlocaleref(*(LONG **)(param_1 + 0x6c));
  FUN_1001a801();
  return;
}


// 1001a801 FUN_1001a801

void FUN_1001a801(void)

{
  FUN_1001cdb5(0xc);
  return;
}


// 1001a80a FUN_1001a80a

DWORD * FUN_1001a80a(void)

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
  pcVar1 = (code *)FUN_1001a6dc();
  pDVar2 = (DWORD *)(*pcVar1)(uVar5);
  if (pDVar2 == (DWORD *)0x0) {
    pDVar2 = (DWORD *)FUN_1000e0e0(1,0x214);
    if (pDVar2 != (DWORD *)0x0) {
      uVar5 = DAT_10035fc0;
      pDVar6 = pDVar2;
      pcVar1 = (code *)FUN_1001a665(DAT_10039edc);
      iVar3 = (*pcVar1)(uVar5,pDVar6);
      if (iVar3 == 0) {
        FUN_1000e140(pDVar2);
        pDVar2 = (DWORD *)0x0;
      }
      else {
        FUN_1001a74b((int)pDVar2,0);
        DVar4 = GetCurrentThreadId();
        pDVar2[1] = 0xffffffff;
        *pDVar2 = DVar4;
      }
    }
  }
  SetLastError(dwErrCode);
  return pDVar2;
}


// 1001a881 __getptd

/* Library Function - Single Match
    __getptd
   
   Library: Visual Studio 2005 Release */

_ptiddata __cdecl __getptd(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = (_ptiddata)FUN_1001a80a();
  if (p_Var1 == (_ptiddata)0x0) {
    __amsg_exit(0x10);
  }
  return p_Var1;
}


// 1001a899 __freefls@4

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
      FUN_1000e140(*(int *)(param_1 + 0x24));
    }
    if (*(int *)(param_1 + 0x2c) != 0) {
      FUN_1000e140(*(int *)(param_1 + 0x2c));
    }
    if (*(int *)(param_1 + 0x34) != 0) {
      FUN_1000e140(*(int *)(param_1 + 0x34));
    }
    if (*(int *)(param_1 + 0x3c) != 0) {
      FUN_1000e140(*(int *)(param_1 + 0x3c));
    }
    if (*(int *)(param_1 + 0x44) != 0) {
      FUN_1000e140(*(int *)(param_1 + 0x44));
    }
    if (*(int *)(param_1 + 0x48) != 0) {
      FUN_1000e140(*(int *)(param_1 + 0x48));
    }
    if (*(undefined **)(param_1 + 0x5c) != &DAT_10036188) {
      FUN_1000e140(*(undefined **)(param_1 + 0x5c));
    }
    __lock(0xd);
    pLVar1 = *(LONG **)(param_1 + 0x68);
    if (pLVar1 != (LONG *)0x0) {
      LVar2 = InterlockedDecrement(pLVar1);
      if ((LVar2 == 0) && (pLVar1 != (LONG *)&DAT_10036330)) {
        FUN_1000e140(pLVar1);
      }
    }
    FUN_1001a9a5();
    __lock(0xc);
    pLVar1 = *(LONG **)(param_1 + 0x6c);
    if (pLVar1 != (LONG *)0x0) {
      ___removelocaleref(pLVar1);
      if (((pLVar1 != (LONG *)PTR_DAT_10036938) && (pLVar1 != (LONG *)&DAT_10036860)) &&
         (*pLVar1 == 0)) {
        ___freetlocinfo((int)pLVar1);
      }
    }
    FUN_1001a9b1();
    FUN_1000e140(param_1);
  }
  return;
}


// 1001a9a5 FUN_1001a9a5

void FUN_1001a9a5(void)

{
  FUN_1001cdb5(0xd);
  return;
}


// 1001a9b1 FUN_1001a9b1

void FUN_1001a9b1(void)

{
  FUN_1001cdb5(0xc);
  return;
}


// 1001a9ba __freeptd

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
    pcVar2 = (code *)FUN_1001a665(DAT_10039edc);
    (*pcVar2)(iVar3,uVar4);
    __freefls_4((int)_Ptd);
  }
  if (DAT_10035fc4 != 0xffffffff) {
    TlsSetValue(DAT_10035fc4,(LPVOID)0x0);
  }
  return;
}


// 1001aa23 __mtinit

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
  DAT_10039ed4 = GetProcAddress(hModule,"FlsAlloc");
  DAT_10039ed8 = GetProcAddress(hModule,"FlsGetValue");
  DAT_10039edc = GetProcAddress(hModule,"FlsSetValue");
  DAT_10039ee0 = GetProcAddress(hModule,"FlsFree");
  if ((((DAT_10039ed4 == (FARPROC)0x0) || (DAT_10039ed8 == (FARPROC)0x0)) ||
      (DAT_10039edc == (FARPROC)0x0)) || (DAT_10039ee0 == (FARPROC)0x0)) {
    DAT_10039ed8 = TlsGetValue_exref;
    DAT_10039ed4 = (FARPROC)&LAB_1001a6d3;
    DAT_10039edc = TlsSetValue_exref;
    DAT_10039ee0 = TlsFree_exref;
  }
  DAT_10035fc4 = TlsAlloc();
  if ((DAT_10035fc4 != 0xffffffff) && (BVar1 = TlsSetValue(DAT_10035fc4,DAT_10039ed8), BVar1 != 0))
  {
    __init_pointers();
    DAT_10039ed4 = (FARPROC)FUN_1001a5ee((int)DAT_10039ed4);
    DAT_10039ed8 = (FARPROC)FUN_1001a5ee((int)DAT_10039ed8);
    DAT_10039edc = (FARPROC)FUN_1001a5ee((int)DAT_10039edc);
    DAT_10039ee0 = (FARPROC)FUN_1001a5ee((int)DAT_10039ee0);
    iVar2 = __mtinitlocks();
    if (iVar2 != 0) {
      pcVar6 = __freefls_4;
      pcVar3 = (code *)FUN_1001a665((int)DAT_10039ed4);
      DAT_10035fc0 = (*pcVar3)(pcVar6);
      if ((DAT_10035fc0 != -1) && (pDVar4 = (DWORD *)FUN_1000e0e0(1,0x214), pDVar4 != (DWORD *)0x0))
      {
        iVar2 = DAT_10035fc0;
        pDVar7 = pDVar4;
        pcVar3 = (code *)FUN_1001a665((int)DAT_10039edc);
        iVar2 = (*pcVar3)(iVar2,pDVar7);
        if (iVar2 != 0) {
          FUN_1001a74b((int)pDVar4,0);
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


// 1001aba7 FUN_1001aba7

void __cdecl FUN_1001aba7(undefined4 param_1)

{
  DAT_10039eec = param_1;
  return;
}


// 1001abc5 fastzero_I

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


// 1001ac1c __VEC_memzero

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


// 1001acab __amsg_exit

/* Library Function - Single Match
    __amsg_exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __amsg_exit(int param_1)

{
  code *pcVar1;
  
  __FF_MSGBANNER();
  FUN_1001a3a9(param_1);
  pcVar1 = (code *)FUN_1001a665((int)PTR___exit_10035fc8);
  (*pcVar1)(0xff);
  return;
}


// 1001accf FUN_1001accf

void __cdecl FUN_1001accf(undefined4 param_1)

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


// 1001acf5 ___crtExitProcess

/* Library Function - Single Match
    ___crtExitProcess
   
   Library: Visual Studio 2005 Release */

void __cdecl ___crtExitProcess(int param_1)

{
  FUN_1001accf(param_1);
                    /* WARNING: Subroutine does not return */
  ExitProcess(param_1);
}


// 1001ad0a FUN_1001ad0a

void FUN_1001ad0a(void)

{
  __lock(8);
  return;
}


// 1001ad13 FUN_1001ad13

void FUN_1001ad13(void)

{
  FUN_1001cdb5(8);
  return;
}


// 1001ad1c FUN_1001ad1c

void __cdecl FUN_1001ad1c(undefined4 *param_1)

{
  undefined4 *in_EAX;
  
  for (; in_EAX < param_1; in_EAX = in_EAX + 1) {
    if ((code *)*in_EAX != (code *)0x0) {
      (*(code *)*in_EAX)();
    }
  }
  return;
}


// 1001ad34 __initterm_e

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


// 1001ad54 __get_osplatform

/* Library Function - Single Match
    __get_osplatform
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __get_osplatform(int *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (int *)0x0) && (DAT_10039ef4 != 0)) {
    *param_1 = DAT_10039ef4;
    return 0;
  }
  pDVar1 = FUN_1001c00a();
  *pDVar1 = 0x16;
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 1001ad8b FUN_1001ad8b

undefined4 __cdecl FUN_1001ad8b(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (undefined4 *)0x0) && (DAT_10039ef4 != 0)) {
    *param_1 = DAT_10039f00;
    return 0;
  }
  pDVar1 = FUN_1001c00a();
  *pDVar1 = 0x16;
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 1001adc7 __cinit

/* Library Function - Single Match
    __cinit
   
   Library: Visual Studio 2005 Release */

int __cdecl __cinit(int param_1)

{
  BOOL BVar1;
  int iVar2;
  undefined4 *puVar3;
  
  BVar1 = __IsNonwritableInCurrentImage((PBYTE)&PTR___fpmath_10028254);
  if (BVar1 != 0) {
    __fpmath(param_1);
  }
  __initp_misc_cfltcvt_tab();
  iVar2 = __initterm_e((undefined4 *)&DAT_10025318,(undefined4 *)&DAT_10025330);
  if (iVar2 == 0) {
    _atexit((_func_4879 *)&LAB_1001cae0);
    puVar3 = &DAT_10025164;
    do {
      if ((code *)*puVar3 != (code *)0x0) {
        (*(code *)*puVar3)();
      }
      puVar3 = puVar3 + 1;
    } while (puVar3 < &DAT_10025314);
    if ((DAT_1003b798 != (code *)0x0) &&
       (BVar1 = __IsNonwritableInCurrentImage((PBYTE)&DAT_1003b798), BVar1 != 0)) {
      (*DAT_1003b798)(0,2,0);
    }
    iVar2 = 0;
  }
  return iVar2;
}


// 1001ae59 FUN_1001ae59

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x1001af2c) */

void __cdecl FUN_1001ae59(int param_1,int param_2,int param_3)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  int iVar4;
  code *pcVar5;
  
  __lock(8);
  if (DAT_10039f34 != 1) {
    DAT_10039f30 = 1;
    DAT_10039f2c = (undefined1)param_3;
    if (param_2 == 0) {
      piVar2 = (int *)FUN_1001a665(DAT_1003b790);
      piVar3 = (int *)FUN_1001a665(DAT_1003b78c);
      if (piVar2 != (int *)0x0) {
        while (piVar3 = piVar3 + -1, piVar2 <= piVar3) {
          if (*piVar3 != 0) {
            iVar1 = *piVar3;
            iVar4 = __encoded_null();
            if (iVar1 != iVar4) {
              pcVar5 = (code *)FUN_1001a665(iVar1);
              (*pcVar5)();
            }
          }
        }
      }
      FUN_1001ad1c((undefined4 *)&DAT_10025340);
    }
    FUN_1001ad1c((undefined4 *)&DAT_10025348);
  }
  FUN_1001af26();
  if (param_3 != 0) {
    return;
  }
  DAT_10039f34 = 1;
  FUN_1001cdb5(8);
  ___crtExitProcess(param_1);
  return;
}


// 1001af26 FUN_1001af26

void FUN_1001af26(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + 0x10) != 0) {
    FUN_1001cdb5(8);
  }
  return;
}


// 1001af3b __exit

/* Library Function - Single Match
    __exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __exit(int _Code)

{
  FUN_1001ae59(_Code,1,0);
  return;
}


// 1001af4c __cexit

/* Library Function - Single Match
    __cexit
   
   Library: Visual Studio 2005 Release */

void __cdecl __cexit(void)

{
  FUN_1001ae59(0,0,1);
  return;
}


// 1001af5b __init_pointers

/* Library Function - Single Match
    __init_pointers
   
   Library: Visual Studio 2005 Release */

void __cdecl __init_pointers(void)

{
  undefined4 uVar1;
  
  uVar1 = __encoded_null();
  FUN_1001f2fe(uVar1);
  FUN_1001f21f(uVar1);
  FUN_1001bea5(uVar1);
  FUN_1001aba7(uVar1);
  FUN_1001f215(uVar1);
  __initp_misc_winsig(uVar1);
  FUN_10017be0();
  __initp_eh_hooks();
  PTR___exit_10035fc8 = (undefined *)FUN_1001a5ee(0x1001af3b);
  return;
}


// 1001afa8 __SEH_prolog4

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
  *(uint *)((int)auStack_1c + iVar1 + 4) = DAT_10036948 ^ (uint)&param_2;
  *(undefined4 *)((int)auStack_1c + iVar1) = unaff_retaddr;
  *unaff_FS_OFFSET = (int)local_8;
  return;
}


// 1001afed __SEH_epilog4

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


// 1001b010 __except_handler4

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
  
  piVar8 = (int *)(*(uint *)((int)param_2 + 8) ^ DAT_10036948);
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
          goto LAB_1001b0bb;
        }
        if (0 < iVar4) {
          if (((*param_1 == -0x1f928c9d) && (DAT_1003b784 != (code *)0x0)) &&
             (BVar5 = __IsNonwritableInCurrentImage((PBYTE)&DAT_1003b784), BVar5 != 0)) {
            (*DAT_1003b784)(param_1,1);
          }
          _EH4_GlobalUnwind(param_2);
          if (*(uint *)((int)param_2 + 0xc) != uVar7) {
            _EH4_LocalUnwind((int)param_2,uVar7,iVar1,&DAT_10036948);
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
    _EH4_LocalUnwind((int)param_2,0xfffffffe,iVar1,&DAT_10036948);
  }
LAB_1001b0bb:
  if (*piVar8 != -2) {
    __security_check_cookie(piVar8[1] + iVar1 ^ *(uint *)(*piVar8 + iVar1));
  }
  __security_check_cookie(piVar8[3] + iVar1 ^ *(uint *)(piVar8[2] + iVar1));
  return local_c;
}


// 1001b1a6 FUN_1001b1a6

void __cdecl FUN_1001b1a6(undefined4 *param_1,undefined4 *param_2,uint param_3)

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


// 1001b22d __VEC_memcpy

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
      FUN_1001b1a6(param_1,param_2,param_3 - uVar3);
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


// 1001b310 __flsbuf

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
    pDVar2 = FUN_1001c00a();
    *pDVar2 = 9;
LAB_1001b334:
    _File_00->_flag = _File_00->_flag | 0x20;
    return -1;
  }
  if ((uVar6 & 0x40) != 0) {
    pDVar2 = FUN_1001c00a();
    *pDVar2 = 0x22;
    goto LAB_1001b334;
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
     (((ppuVar3 = FUN_1001fd3b(), _File_00 != (FILE *)(ppuVar3 + 8) &&
       (ppuVar3 = FUN_1001fd3b(), _File_00 != (FILE *)(ppuVar3 + 0x10))) ||
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
        puVar5 = &DAT_10036150;
      }
      else {
        puVar5 = (undefined *)(((uint)_File & 0x1f) * 0x38 + (&DAT_1003b680)[(int)_File >> 5]);
      }
      if (((puVar5[4] & 0x20) != 0) &&
         (lVar7 = __lseeki64((int)_File,0x200000000,unaff_EDI), lVar7 == -1)) goto LAB_1001b45a;
    }
    else {
      local_8 = __write((int)_File,_Buf,uVar6);
    }
    *_File_00->_base = (char)_Ch;
  }
  if (local_8 == uVar6) {
    return _Ch & 0xff;
  }
LAB_1001b45a:
  _File_00->_flag = _File_00->_flag | 0x20;
  return -1;
}


// 1001b470 write_char

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


// 1001b4a3 write_multi_char

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


// 1001b4c7 write_string

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
        pDVar1 = FUN_1001c00a();
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


// 1001b511 FUN_1001b511

/* WARNING: Type propagation algorithm not settling */

void __cdecl FUN_1001b511(FILE *param_1,byte *param_2,localeinfo_struct *param_3,wchar_t *param_4)

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
  
  local_8 = DAT_10036948 ^ (uint)local_1fc;
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
        puVar7 = &DAT_10036150;
      }
      else {
        iVar5 = __fileno(local_22c);
        uVar6 = __fileno(local_22c);
        puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_1003b680)[iVar5 >> 5]);
      }
      if ((puVar7[0x24] & 0x7f) == 0) {
        iVar5 = __fileno(local_22c);
        if ((iVar5 == -1) || (iVar5 = __fileno(local_22c), iVar5 == -2)) {
          puVar7 = &DAT_10036150;
        }
        else {
          iVar5 = __fileno(local_22c);
          uVar6 = __fileno(local_22c);
          puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_1003b680)[iVar5 >> 5]);
        }
        if ((puVar7[0x24] & 0x80) == 0) goto LAB_1001b64f;
      }
    }
    else {
LAB_1001b64f:
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
          local_270 = (int)(char)(&DAT_10028960)[uVar6 * 8 + iVar5] >> 4;
          switch(local_270) {
          case 0:
switchD_1001b6b3_caseD_0:
            local_240 = 0;
            iVar5 = __isleadbyte_l((uint)local_215,&local_260);
            if (iVar5 != 0) {
              write_char();
              local_244 = param_2 + 2;
              if (*pbVar12 == 0) {
LAB_1001be46:
                pDVar4 = FUN_1001c00a();
                *pDVar4 = 0x16;
                goto LAB_1001b581;
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
                goto switchD_1001b6b3_caseD_0;
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
LAB_1001ba92:
                local_214 = local_214 | 0x40;
LAB_1001ba96:
                local_224 = 10;
LAB_1001ba9d:
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
                  goto LAB_1001b91b;
                }
                if (local_215 == 0x41) {
LAB_1001b8c7:
                  local_215 = local_215 + 0x20;
                  local_274 = 1;
LAB_1001b8d4:
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
                      local_250 = (wchar_t *)FUN_1000dfa0(sVar13);
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
                  pcVar9 = (code *)FUN_1001a665((int)PTR_LAB_10035ee8);
                  (*pcVar9)(puVar17,pwVar18,sVar13,iVar8,iVar5,uVar19,plVar20);
                  uVar6 = local_214 & 0x80;
                  if ((uVar6 != 0) && (local_21c == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_1001a665((int)PTR_LAB_10035ef4);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((local_215 == 0x67) && (uVar6 == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_1001a665((int)PTR_LAB_10035ef0);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((char)*pwVar14 == '-') {
                    local_214 = local_214 | 0x100;
                    local_220 = (wchar_t *)((int)pwVar14 + 1);
                    pwVar14 = local_220;
                  }
LAB_1001ba08:
                  local_224 = _strlen((char *)pwVar14);
                }
                else if (local_215 == 0x43) {
                  if ((local_214 & 0x830) == 0) {
                    local_214 = local_214 | 0x800;
                  }
LAB_1001b978:
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
                else if ((local_215 == 0x45) || (local_215 == 0x47)) goto LAB_1001b8c7;
              }
              else {
                if (local_215 == 0x58) goto LAB_1001bb9d;
                if (local_215 == 0x5a) {
                  psVar2 = *(short **)param_4;
                  local_228 = param_4 + 2;
                  pwVar14 = (wchar_t *)PTR_s__null__10035fcc;
                  local_220 = (wchar_t *)PTR_s__null__10035fcc;
                  if ((psVar2 == (short *)0x0) ||
                     (pwVar18 = *(wchar_t **)(psVar2 + 2), pwVar18 == (wchar_t *)0x0))
                  goto LAB_1001ba08;
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
                  if (local_215 == 0x61) goto LAB_1001b8d4;
                  if (local_215 == 99) goto LAB_1001b978;
                }
              }
LAB_1001bd15:
              if (local_24c == 0) {
                if ((local_214 & 0x40) != 0) {
                  if ((local_214 & 0x100) == 0) {
                    if ((local_214 & 1) == 0) {
                      if ((local_214 & 2) == 0) goto LAB_1001bd4b;
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
LAB_1001bd4b:
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
LAB_1001b91b:
                  iVar5 = local_21c;
                  if (local_21c == -1) {
                    iVar5 = 0x7fffffff;
                  }
                  local_228 = param_4 + 2;
                  local_220 = *(wchar_t **)param_4;
                  if ((local_214 & 0x810) == 0) {
                    pwVar14 = local_220;
                    if (local_220 == (wchar_t *)0x0) {
                      pwVar14 = (wchar_t *)PTR_s__null__10035fcc;
                      local_220 = (wchar_t *)PTR_s__null__10035fcc;
                    }
                    for (; (iVar5 != 0 && (iVar5 = iVar5 + -1, (char)*pwVar14 != '\0'));
                        pwVar14 = (wchar_t *)((int)pwVar14 + 1)) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220;
                  }
                  else {
                    if (local_220 == (wchar_t *)0x0) {
                      local_220 = (wchar_t *)PTR_u__null__10035fd0;
                    }
                    local_240 = 1;
                    for (pwVar14 = local_220;
                        (iVar5 != 0 && (iVar5 = iVar5 + -1, *pwVar14 != L'\0'));
                        pwVar14 = pwVar14 + 1) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220 >> 1;
                  }
                  goto LAB_1001bd15;
                }
                if (local_215 == 0x75) goto LAB_1001ba96;
                if (local_215 != 0x78) goto LAB_1001bd15;
                local_248 = 0x27;
LAB_1001bbc3:
                local_224 = 0x10;
                if ((local_214 & 0x80) != 0) {
                  local_233 = (char)local_248 + 'Q';
                  local_234 = 0x30;
                  local_238 = 2;
                }
                goto LAB_1001ba9d;
              }
              if (local_215 == 0x70) {
                local_21c = 8;
LAB_1001bb9d:
                local_248 = 7;
                goto LAB_1001bbc3;
              }
              if ((char)local_215 < 'e') goto LAB_1001bd15;
              if ((char)local_215 < 'h') goto LAB_1001b8d4;
              if (local_215 == 0x69) goto LAB_1001ba92;
              if (local_215 != 0x6e) {
                if (local_215 != 0x6f) goto LAB_1001bd15;
                local_224 = 8;
                if ((local_214 & 0x80) != 0) {
                  local_214 = local_214 | 0x200;
                }
                goto LAB_1001ba9d;
              }
              piVar3 = *(int **)param_4;
              local_228 = param_4 + 2;
              bVar15 = FUN_1001ff09();
              if (CONCAT31(extraout_var,bVar15) == 0) goto LAB_1001be46;
              if ((local_214 & 0x20) == 0) {
                *piVar3 = local_230;
              }
              else {
                *(undefined2 *)piVar3 = (undefined2)local_230;
              }
              local_24c = 1;
            }
            if (local_250 != (wchar_t *)0x0) {
              FUN_1000e140(local_250);
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
        goto LAB_1001be6d;
      }
    }
  }
  pDVar4 = FUN_1001c00a();
  *pDVar4 = 0x16;
LAB_1001b581:
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_254 != '\0') {
    *(uint *)(local_258 + 0x70) = *(uint *)(local_258 + 0x70) & 0xfffffffd;
  }
LAB_1001be6d:
  __security_check_cookie(local_8 ^ (uint)local_1fc);
  return;
}


// 1001bea5 FUN_1001bea5

void __cdecl FUN_1001bea5(undefined4 param_1)

{
  DAT_10039f38 = param_1;
  return;
}


// 1001beaf __invoke_watson

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
  
  iVar1 = DAT_10036948;
  local_2d4 = 0x10001;
  _memset(&local_32c,0,0x50);
  local_2dc.ExceptionRecord = &local_32c;
  local_2dc.ContextRecord = (PCONTEXT)&local_2d4;
  local_32c.ExceptionCode = 0xc000000d;
  BVar2 = IsDebuggerPresent();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  LVar3 = UnhandledExceptionFilter(&local_2dc);
  if ((LVar3 == 0) && (BVar2 == 0)) {
    FUN_10020135();
  }
  uExitCode = 0xc000000d;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  __security_check_cookie(iVar1);
  return;
}


// 1001bfab FUN_1001bfab

void __cdecl
FUN_1001bfab(wchar_t *param_1,wchar_t *param_2,wchar_t *param_3,uint param_4,uintptr_t param_5)

{
  code *UNRECOVERED_JUMPTABLE;
  
  UNRECOVERED_JUMPTABLE = (code *)FUN_1001a665(DAT_10039f38);
  if (UNRECOVERED_JUMPTABLE != (code *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x1001bfbf. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (*UNRECOVERED_JUMPTABLE)();
    return;
  }
  FUN_10020135();
                    /* WARNING: Subroutine does not return */
  __invoke_watson(param_1,param_2,param_3,param_4,param_5);
}


// 1001bfcf FUN_1001bfcf

int __cdecl FUN_1001bfcf(int param_1)

{
  uint uVar1;
  
  uVar1 = 0;
  do {
    if (param_1 == (&DAT_10035fd8)[uVar1 * 2]) {
      return *(int *)(uVar1 * 8 + 0x10035fdc);
    }
    uVar1 = uVar1 + 1;
  } while (uVar1 < 0x2d);
  if (param_1 - 0x13U < 0x12) {
    return 0xd;
  }
  return (-(uint)(0xe < param_1 - 0xbcU) & 0xe) + 8;
}


// 1001c00a FUN_1001c00a

DWORD * FUN_1001c00a(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_1001a80a();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10036140;
  }
  return pDVar1 + 2;
}


// 1001c01d FUN_1001c01d

DWORD * FUN_1001c01d(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_1001a80a();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10036144;
  }
  return pDVar1 + 3;
}


// 1001c030 FUN_1001c030

void __cdecl FUN_1001c030(DWORD param_1)

{
  DWORD *pDVar1;
  DWORD DVar2;
  
  pDVar1 = FUN_1001c01d();
  *pDVar1 = param_1;
  DVar2 = FUN_1001bfcf(param_1);
  pDVar1 = FUN_1001c00a();
  *pDVar1 = DVar2;
  return;
}


// 1001c04e ___libm_error_support

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
  if (DAT_10039f3c == 0) {
    pcVar1 = FUN_1002013d;
  }
  else {
    pcVar1 = (code *)FUN_1001a665(DAT_1003b780);
  }
  if (param_4 < 0xa7) {
    if (param_4 == 0xa6) {
      local_2c = 3;
      local_28 = "exp10";
LAB_1001c0fd:
      local_24 = *param_1;
      local_1c = *param_2;
      local_14 = *param_3;
      iVar2 = (*pcVar1)(&local_2c);
      if (iVar2 == 0) {
        pDVar3 = FUN_1001c00a();
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
            goto LAB_1001c16b;
          }
          if (param_4 == 8) {
            local_28 = "log10";
          }
          else {
            if (param_4 == 9) {
              local_28 = "log10";
              goto LAB_1001c16b;
            }
            if (param_4 != 0xe) {
              if (param_4 != 0xf) {
                if (param_4 != 0x18) {
                  return;
                }
                local_2c = 3;
                goto LAB_1001c0f6;
              }
              local_28 = "exp";
              goto LAB_1001c132;
            }
            local_2c = 3;
            local_28 = "exp";
          }
        }
        goto LAB_1001c0fd;
      }
      local_28 = "pow";
LAB_1001c132:
      local_24 = *param_1;
      local_1c = *param_2;
      local_2c = 4;
      local_14 = *param_3;
      (*pcVar1)(&local_2c);
    }
    else {
      if (param_4 != 0x1a) {
        if (param_4 != 0x1b) {
          if (param_4 == 0x1c) goto switchD_1001c228_caseD_3ee;
          if (param_4 != 0x1d) {
            if (param_4 != 0x3a) {
              if (param_4 != 0x3d) {
                return;
              }
              goto switchD_1001c228_caseD_3f1;
            }
            goto switchD_1001c228_caseD_3f0;
          }
          local_28 = "pow";
          goto LAB_1001c1e1;
        }
        local_2c = 2;
LAB_1001c0f6:
        local_28 = "pow";
        goto LAB_1001c0fd;
      }
      local_14 = 1.0;
    }
    goto LAB_1001c2c1;
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
switchD_1001c228_caseD_3ee:
    local_28 = "pow";
    goto LAB_1001c16b;
  case 0x3ef:
    local_28 = "modf";
    break;
  case 0x3f0:
switchD_1001c228_caseD_3f0:
    local_28 = "acos";
    goto LAB_1001c16b;
  case 0x3f1:
switchD_1001c228_caseD_3f1:
    local_28 = "asin";
    goto LAB_1001c16b;
  case 0x3f2:
    local_28 = "sin";
    goto LAB_1001c28d;
  case 0x3f3:
    local_28 = "cos";
    goto LAB_1001c28d;
  case 0x3f4:
    local_28 = "tan";
LAB_1001c28d:
    local_14 = *param_1 *
               (double)CONCAT17(uStack_5,CONCAT16(uStack_6,CONCAT15(uStack_7,CONCAT14(uStack_8,
                                                  CONCAT13(uStack_9,CONCAT12(uStack_a,CONCAT11(
                                                  uStack_b,local_c)))))));
    *param_3 = local_14;
    local_24 = *param_1;
    local_1c = *param_2;
    goto LAB_1001c29e;
  default:
    goto switchD_1001c228_default;
  }
LAB_1001c1e1:
  *param_3 = *param_1;
LAB_1001c16b:
  local_24 = *param_1;
  local_1c = *param_2;
  local_14 = *param_3;
LAB_1001c29e:
  local_2c = 1;
  iVar2 = (*pcVar1)(&local_2c);
  if (iVar2 == 0) {
    pDVar3 = FUN_1001c00a();
    *pDVar3 = 0x21;
  }
LAB_1001c2c1:
  *param_3 = local_14;
switchD_1001c228_default:
  return;
}


// 1001c2fe FUN_1001c2fe

float10 __cdecl FUN_1001c2fe(double param_1)

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
    fVar4 = FUN_100207fb(param_1);
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


// 1001c3cf __ioinit

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
  
  uStack_c = 0x1001c3db;
  local_8 = 0;
  GetStartupInfoA(&local_68);
  local_8 = 0xfffffffe;
  puVar2 = (undefined4 *)FUN_1000e0e0(0x20,0x38);
  if (puVar2 == (undefined4 *)0x0) {
LAB_1001c606:
    iVar7 = -1;
  }
  else {
    DAT_1003b664 = 0x20;
    DAT_1003b680 = puVar2;
    for (; puVar2 < DAT_1003b680 + 0x1c0; puVar2 = puVar2 + 0xe) {
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
      while ((UVar10 = UVar9, (int)DAT_1003b664 < (int)UVar9 &&
             (puVar2 = (undefined4 *)FUN_1000e0e0(0x20,0x38), UVar10 = DAT_1003b664,
             puVar2 != (undefined4 *)0x0))) {
        (&DAT_1003b680)[iVar7] = puVar2;
        DAT_1003b664 = DAT_1003b664 + 0x20;
        puVar1 = puVar2;
        for (; puVar2 < puVar1 + 0x1c0; puVar2 = puVar2 + 0xe) {
          *(undefined1 *)(puVar2 + 1) = 0;
          *puVar2 = 0xffffffff;
          *(undefined1 *)((int)puVar2 + 5) = 10;
          puVar2[2] = 0;
          *(byte *)(puVar2 + 9) = *(byte *)(puVar2 + 9) & 0x80;
          *(undefined1 *)((int)puVar2 + 0x25) = 10;
          *(undefined1 *)((int)puVar2 + 0x26) = 10;
          puVar1 = (&DAT_1003b680)[iVar7];
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
                     ((local_24 & 0x1f) * 0x38 + (int)(&DAT_1003b680)[(int)local_24 >> 5]);
            *puVar2 = *(undefined4 *)local_20;
            *(byte *)(puVar2 + 1) = (byte)*pUVar6;
            iVar7 = ___crtInitCritSecAndSpinCount(puVar2 + 3,4000);
            if (iVar7 == 0) goto LAB_1001c606;
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
      piVar8 = DAT_1003b680 + iVar7 * 0xe;
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
          if (iVar5 == 0) goto LAB_1001c606;
          piVar8[2] = piVar8[2] + 1;
        }
      }
      else {
        *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 0x80;
      }
      iVar7 = iVar7 + 1;
    } while (iVar7 < 3);
    SetHandleCount(DAT_1003b664);
    iVar7 = 0;
  }
  return iVar7;
}


// 1001c60f __ioterm

/* Library Function - Single Match
    __ioterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __ioterm(void)

{
  uint uVar1;
  uint *puVar2;
  uint uVar3;
  
  puVar2 = &DAT_1003b680;
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
      FUN_1000e140(*puVar2);
      *puVar2 = 0;
    }
    puVar2 = puVar2 + 1;
  } while ((int)puVar2 < 0x1003b780);
  return;
}


// 1001c65b __setenvp

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
  
  if (DAT_1003b794 == 0) {
    ___initmbctable();
  }
  iVar5 = 0;
  pcVar4 = DAT_10039ba4;
  if (DAT_10039ba4 != (char *)0x0) {
    for (; *pcVar4 != '\0'; pcVar4 = pcVar4 + sVar2 + 1) {
      if (*pcVar4 != '=') {
        iVar5 = iVar5 + 1;
      }
      sVar2 = _strlen(pcVar4);
    }
    puVar1 = (undefined4 *)FUN_1000e0e0(iVar5 + 1,4);
    pcVar4 = DAT_10039ba4;
    DAT_10039f14 = puVar1;
    if (puVar1 != (undefined4 *)0x0) {
      do {
        if (*pcVar4 == '\0') {
          FUN_1000e140(DAT_10039ba4);
          DAT_10039ba4 = (char *)0x0;
          *puVar1 = 0;
          _DAT_1003b788 = 1;
          return 0;
        }
        sVar2 = _strlen(pcVar4);
        _SizeInBytes = sVar2 + 1;
        if (*pcVar4 != '=') {
          _Dst = (char *)FUN_1000e0e0(_SizeInBytes,1);
          *puVar1 = _Dst;
          if (_Dst == (char *)0x0) {
            FUN_1000e140(DAT_10039f14);
            DAT_10039f14 = (undefined4 *)0x0;
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


// 1001c736 parse_cmdline

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
        goto LAB_1001c7c8;
      }
    }
    in_EDX = pbVar7;
  } while ((bVar2) || ((bVar6 != 0x20 && (bVar6 != 9))));
  if (pbVar5 != (byte *)0x0) {
    pbVar5[-1] = 0;
  }
LAB_1001c7c8:
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


// 1001c8ce __setargv

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
  
  if (DAT_1003b794 == 0) {
    ___initmbctable();
  }
  DAT_1003a044 = 0;
  GetModuleFileNameA((HMODULE)0x0,&DAT_10039f40,0x104);
  _DAT_10039f24 = &DAT_10039f40;
  if ((DAT_1003b7a4 == (char *)0x0) || (local_8 = DAT_1003b7a4, *DAT_1003b7a4 == '\0')) {
    local_8 = &DAT_10039f40;
  }
  parse_cmdline((undefined4 *)0x0,(byte *)0x0,(int *)&local_c);
  uVar2 = local_c;
  if ((local_c < 0x3fffffff) && (local_10 != 0xffffffff)) {
    uVar1 = local_c * 4 + local_10;
    if ((local_10 <= uVar1) &&
       (puVar3 = (undefined4 *)FUN_1000dfa0(uVar1), puVar3 != (undefined4 *)0x0)) {
      parse_cmdline(puVar3,(byte *)(puVar3 + uVar2),(int *)&local_c);
      _DAT_10039f08 = local_c - 1;
      _DAT_10039f0c = puVar3;
      return 0;
    }
  }
  return -1;
}


// 1001c987 ___crtGetEnvironmentStringsA

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
  if (DAT_1003a048 == 0) {
    lpWideCharStr = GetEnvironmentStringsW();
    if (lpWideCharStr != (LPWCH)0x0) {
      DAT_1003a048 = 1;
      goto LAB_1001c9d9;
    }
    DVar3 = GetLastError();
    if (DVar3 == 0x78) {
      DAT_1003a048 = 2;
    }
  }
  if (DAT_1003a048 != 1) {
    if ((DAT_1003a048 != 2) && (DAT_1003a048 != 0)) {
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
    _Dst = (void *)FUN_1000dfa0(pcVar7 + (1 - (int)_Src));
    if (_Dst == (void *)0x0) {
      FreeEnvironmentStringsA(_Src);
      return (LPVOID)0x0;
    }
    _memcpy(_Dst,_Src,(size_t)(pcVar7 + (1 - (int)_Src)));
    FreeEnvironmentStringsA(_Src);
    return _Dst;
  }
LAB_1001c9d9:
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
      (lpMultiByteStr = (LPSTR)FUN_1000dfa0(cbMultiByte), lpMultiByteStr != (LPSTR)0x0)) &&
     (iVar6 = WideCharToMultiByte(0,0,lpWideCharStr,iVar6,lpMultiByteStr,cbMultiByte,(LPCSTR)0x0,
                                  (LPBOOL)0x0), local_8 = lpMultiByteStr, iVar6 == 0)) {
    FUN_1000e140(lpMultiByteStr);
    local_8 = (LPSTR)0x0;
  }
  FreeEnvironmentStringsW(lpWideCharStr);
  return local_8;
}


// 1001cabc __RTC_Initialize

/* WARNING: Removing unreachable block (ram,0x1001cace) */
/* WARNING: Removing unreachable block (ram,0x1001cad4) */
/* WARNING: Removing unreachable block (ram,0x1001cad6) */
/* Library Function - Single Match
    __RTC_Initialize
   
   Library: Visual Studio 2005 Release */

void __RTC_Initialize(void)

{
  return;
}


// 1001cb04 FUN_1001cb04

DWORD * __cdecl FUN_1001cb04(int param_1,DWORD param_2)

{
  int *piVar1;
  code *pcVar2;
  DWORD DVar3;
  DWORD DVar4;
  DWORD *pDVar5;
  int *piVar6;
  int iVar7;
  int iVar8;
  
  pDVar5 = FUN_1001a80a();
  if (pDVar5 != (DWORD *)0x0) {
    piVar1 = (int *)pDVar5[0x17];
    piVar6 = piVar1;
    do {
      if (*piVar6 == param_1) break;
      piVar6 = piVar6 + 3;
    } while (piVar6 < piVar1 + DAT_1003620c * 3);
    if ((piVar1 + DAT_1003620c * 3 <= piVar6) || (*piVar6 != param_1)) {
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
          if (DAT_10036200 < DAT_10036204 + DAT_10036200) {
            iVar7 = DAT_10036200 * 0xc;
            iVar8 = DAT_10036200;
            do {
              *(undefined4 *)(iVar7 + 8 + pDVar5[0x17]) = 0;
              iVar8 = iVar8 + 1;
              iVar7 = iVar7 + 0xc;
            } while (iVar8 < DAT_10036204 + DAT_10036200);
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


// 1001cc7d FUN_1001cc7d

undefined4 FUN_1001cc7d(void)

{
  return 1;
}


// 1001cc83 ___security_init_cookie

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
  if ((DAT_10036948 == 0xbb40e64e) || ((DAT_10036948 & 0xffff0000) == 0)) {
    GetSystemTimeAsFileTime(&local_c);
    uVar4 = local_c.dwHighDateTime ^ local_c.dwLowDateTime;
    DVar1 = GetCurrentProcessId();
    DVar2 = GetCurrentThreadId();
    DVar3 = GetTickCount();
    QueryPerformanceCounter(&local_14);
    DAT_10036948 = uVar4 ^ DVar1 ^ DVar2 ^ DVar3 ^ local_14.s.HighPart ^ local_14.s.LowPart;
    if (DAT_10036948 == 0xbb40e64e) {
      DAT_10036948 = 0xbb40e64f;
    }
    else if ((DAT_10036948 & 0xffff0000) == 0) {
      DAT_10036948 = DAT_10036948 | DAT_10036948 << 0x10;
    }
    DAT_1003694c = ~DAT_10036948;
  }
  else {
    DAT_1003694c = ~DAT_10036948;
  }
  return;
}


// 1001cd17 __mtinitlocks

/* Library Function - Single Match
    __mtinitlocks
   
   Library: Visual Studio 2005 Release */

int __cdecl __mtinitlocks(void)

{
  int iVar1;
  int iVar2;
  undefined *puVar3;
  
  iVar2 = 0;
  puVar3 = &DAT_1003a050;
  do {
    if ((&DAT_10036214)[iVar2 * 2] == 1) {
      (&DAT_10036210)[iVar2 * 2] = puVar3;
      puVar3 = puVar3 + 0x18;
      iVar1 = ___crtInitCritSecAndSpinCount((&DAT_10036210)[iVar2 * 2],4000);
      if (iVar1 == 0) {
        (&DAT_10036210)[iVar2 * 2] = 0;
        return 0;
      }
    }
    iVar2 = iVar2 + 1;
  } while (iVar2 < 0x24);
  return 1;
}


// 1001cd60 __mtdeletelocks

/* Library Function - Single Match
    __mtdeletelocks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __cdecl __mtdeletelocks(void)

{
  LPCRITICAL_SECTION lpCriticalSection;
  undefined4 *puVar1;
  
  puVar1 = &DAT_10036210;
  do {
    lpCriticalSection = (LPCRITICAL_SECTION)*puVar1;
    if ((lpCriticalSection != (LPCRITICAL_SECTION)0x0) && (puVar1[1] != 1)) {
      DeleteCriticalSection(lpCriticalSection);
      FUN_1000e140(lpCriticalSection);
      *puVar1 = 0;
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10036330);
  puVar1 = &DAT_10036210;
  do {
    if (((LPCRITICAL_SECTION)*puVar1 != (LPCRITICAL_SECTION)0x0) && (puVar1[1] == 1)) {
      DeleteCriticalSection((LPCRITICAL_SECTION)*puVar1);
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10036330);
  return;
}


// 1001cdb5 FUN_1001cdb5

void __cdecl FUN_1001cdb5(int param_1)

{
  LeaveCriticalSection((LPCRITICAL_SECTION)(&DAT_10036210)[param_1 * 2]);
  return;
}


// 1001cdca __mtinitlocknum

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
  if (DAT_1002c4f8 == 0) {
    __FF_MSGBANNER();
    FUN_1001a3a9(0x1e);
    ___crtExitProcess(0xff);
  }
  piVar1 = &DAT_10036210 + _LockNum * 2;
  if (*piVar1 == 0) {
    iVar4 = FUN_1000dfa0(0x18);
    if (iVar4 == 0) {
      pDVar2 = FUN_1001c00a();
      *pDVar2 = 0xc;
      iVar4 = 0;
    }
    else {
      __lock(10);
      if (*piVar1 == 0) {
        iVar3 = ___crtInitCritSecAndSpinCount(iVar4,4000);
        if (iVar3 == 0) {
          FUN_1000e140(iVar4);
          pDVar2 = FUN_1001c00a();
          *pDVar2 = 0xc;
          local_20 = 0;
        }
        else {
          *piVar1 = iVar4;
        }
      }
      else {
        FUN_1000e140(iVar4);
      }
      FUN_1001ce84();
      iVar4 = local_20;
    }
  }
  return iVar4;
}


// 1001ce84 FUN_1001ce84

void FUN_1001ce84(void)

{
  FUN_1001cdb5(10);
  return;
}


// 1001ce8d __lock

/* Library Function - Single Match
    __lock
   
   Library: Visual Studio 2005 Release */

void __cdecl __lock(int _File)

{
  int iVar1;
  
  if ((&DAT_10036210)[_File * 2] == 0) {
    iVar1 = __mtinitlocknum(_File);
    if (iVar1 == 0) {
      __amsg_exit(0x11);
    }
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(&DAT_10036210)[_File * 2]);
  return;
}


// 1001cebe FUN_1001cebe

undefined4 FUN_1001cebe(void)

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


// 1001ceed setSBCS

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
    *puVar1 = puVar1[(int)&DAT_10036330 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  puVar1 = (undefined1 *)(in_EAX + 0x11d);
  iVar2 = 0x100;
  do {
    *puVar1 = puVar1[(int)&DAT_10036330 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  return;
}


// 1001cf42 setSBUpLow

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
  
  local_8 = DAT_10036948 ^ (uint)local_4a0;
  BVar3 = GetCPInfo(*(UINT *)(unaff_ESI + 4),&local_51c);
  if (BVar3 == 0) {
    uVar4 = 0;
    do {
      pcVar2 = (char *)(unaff_ESI + 0x11d + uVar4);
      if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) + 0x20 < (char *)0x1a) {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        cVar6 = (char)uVar4 + ' ';
LAB_1001d0a9:
        *pcVar2 = cVar6;
      }
      else {
        if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) < (char *)0x1a) {
          pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
          *pbVar1 = *pbVar1 | 0x20;
          cVar6 = (char)uVar4 + -0x20;
          goto LAB_1001d0a9;
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
          goto LAB_1001d050;
        }
        *(undefined1 *)(unaff_ESI + 0x11d + uVar4) = 0;
      }
      else {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        CVar5 = local_208[uVar4];
LAB_1001d050:
        *(CHAR *)(unaff_ESI + 0x11d + uVar4) = CVar5;
      }
      uVar4 = uVar4 + 1;
    } while (uVar4 < 0x100);
  }
  __security_check_cookie(local_8 ^ (uint)local_4a0);
  return;
}


// 1001d0cc ___updatetmbcinfo

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
  if (((p_Var1->_ownlocale & DAT_10036854) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xd);
    lpAddend = p_Var1->ptmbcinfo;
    if (lpAddend != (pthreadmbcinfo)PTR_DAT_10036758) {
      if (lpAddend != (pthreadmbcinfo)0x0) {
        LVar2 = InterlockedDecrement(&lpAddend->refcount);
        if ((LVar2 == 0) && (lpAddend != (pthreadmbcinfo)&DAT_10036330)) {
          FUN_1000e140(lpAddend);
        }
      }
      p_Var1->ptmbcinfo = (pthreadmbcinfo)PTR_DAT_10036758;
      lpAddend = (pthreadmbcinfo)PTR_DAT_10036758;
      InterlockedIncrement((LONG *)PTR_DAT_10036758);
    }
    FUN_1001d167();
  }
  else {
    lpAddend = p_Var1->ptmbcinfo;
  }
  if (lpAddend == (pthreadmbcinfo)0x0) {
    __amsg_exit(0x20);
  }
  return lpAddend;
}


// 1001d167 FUN_1001d167

void FUN_1001d167(void)

{
  FUN_1001cdb5(0xd);
  return;
}


// 1001d170 getSystemCP

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
  DAT_1003a1a0 = 0;
  if (unaff_ESI == -2) {
    DAT_1003a1a0 = 1;
    UVar1 = GetOEMCP();
  }
  else if (unaff_ESI == -3) {
    DAT_1003a1a0 = 1;
    UVar1 = GetACP();
  }
  else {
    if (unaff_ESI != -4) {
      if (local_8 == '\0') {
        DAT_1003a1a0 = 0;
        return unaff_ESI;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return unaff_ESI;
    }
    UVar1 = *(UINT *)(local_14[0] + 4);
    DAT_1003a1a0 = 1;
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return UVar1;
}


// 1001d1ea FUN_1001d1ea

void __cdecl FUN_1001d1ea(undefined4 param_1,int param_2)

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  uVar4 = getSystemCP((int)unaff_EDI);
  if (uVar4 != 0) {
    local_20 = (byte *)0x0;
    uVar5 = 0;
LAB_1001d226:
    if (*(uint *)((int)&DAT_10036760 + uVar5) != uVar4) goto code_r0x1001d232;
    _memset((void *)(param_2 + 0x1c),0,0x101);
    local_24 = 0;
    pbVar8 = &DAT_10036770 + (int)local_20 * 0x30;
    local_20 = pbVar8;
    do {
      for (; (*pbVar8 != 0 && (bVar3 = pbVar8[1], bVar3 != 0)); pbVar8 = pbVar8 + 2) {
        for (uVar5 = (uint)*pbVar8; uVar5 <= bVar3; uVar5 = uVar5 + 1) {
          pbVar2 = (byte *)(param_2 + 0x1d + uVar5);
          *pbVar2 = *pbVar2 | *(byte *)(local_24 + 0x1003675c);
          bVar3 = pbVar8[1];
        }
      }
      local_24 = local_24 + 1;
      pbVar8 = local_20 + 8;
      local_20 = pbVar8;
    } while (local_24 < 4);
    *(uint *)(param_2 + 4) = uVar4;
    *(undefined4 *)(param_2 + 8) = 1;
    uVar9 = FUN_1001cebe();
    *(undefined4 *)(param_2 + 0xc) = uVar9;
    puVar7 = (undefined2 *)(param_2 + 0x10);
    puVar10 = (undefined2 *)(&DAT_10036764 + extraout_ECX);
    iVar11 = 6;
    do {
      *puVar7 = *puVar10;
      puVar10 = puVar10 + 1;
      puVar7 = puVar7 + 1;
      iVar11 = iVar11 + -1;
    } while (iVar11 != 0);
    goto LAB_1001d357;
  }
LAB_1001d213:
  setSBCS(unaff_EDI);
LAB_1001d3b4:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
code_r0x1001d232:
  local_20 = (byte *)((int)local_20 + 1);
  uVar5 = uVar5 + 0x30;
  if (0xef < uVar5) goto code_r0x1001d23f;
  goto LAB_1001d226;
code_r0x1001d23f:
  if (((uVar4 == 65000) || (uVar4 == 0xfde9)) ||
     (BVar6 = IsValidCodePage(uVar4 & 0xffff), BVar6 == 0)) goto LAB_1001d3b4;
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
      uVar9 = FUN_1001cebe();
      *(undefined4 *)(param_2 + 0xc) = uVar9;
      *(undefined4 *)(param_2 + 8) = extraout_EDX;
    }
    *(undefined4 *)(param_2 + 0x10) = 0;
    *(undefined4 *)(param_2 + 0x14) = 0;
    *(undefined4 *)(param_2 + 0x18) = 0;
LAB_1001d357:
    setSBUpLow(unaff_EDI);
    goto LAB_1001d3b4;
  }
  if (DAT_1003a1a0 == 0) goto LAB_1001d3b4;
  goto LAB_1001d213;
}


// 1001d3c3 __setmbcp

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
    ptVar3 = (pthreadmbcinfo)FUN_1000dfa0(0x220);
    if (ptVar3 != (pthreadmbcinfo)0x0) {
      ptVar7 = p_Var1->ptmbcinfo;
      ptVar8 = ptVar3;
      for (iVar6 = 0x88; iVar6 != 0; iVar6 = iVar6 + -1) {
        ptVar8->refcount = ptVar7->refcount;
        ptVar7 = (pthreadmbcinfo)&ptVar7->mbcodepage;
        ptVar8 = (pthreadmbcinfo)&ptVar8->mbcodepage;
      }
      ptVar3->refcount = 0;
      local_24 = FUN_1001d1ea(iVar2,(int)ptVar3);
      if (local_24 == 0) {
        LVar4 = InterlockedDecrement(&p_Var1->ptmbcinfo->refcount);
        if ((LVar4 == 0) && (p_Var1->ptmbcinfo != (pthreadmbcinfo)&DAT_10036330)) {
          FUN_1000e140(p_Var1->ptmbcinfo);
        }
        p_Var1->ptmbcinfo = ptVar3;
        InterlockedIncrement((LONG *)ptVar3);
        if (((p_Var1->_ownlocale & 2) == 0) && (((byte)DAT_10036854 & 1) == 0)) {
          __lock(0xd);
          _DAT_1003a1b0 = ptVar3->mbcodepage;
          _DAT_1003a1b4 = ptVar3->ismbcodepage;
          _DAT_1003a1b8 = *(undefined4 *)ptVar3->mbulinfo;
          for (iVar2 = 0; iVar2 < 5; iVar2 = iVar2 + 1) {
            (&DAT_1003a1a4)[iVar2] = ptVar3->mbulinfo[iVar2 + 2];
          }
          for (iVar2 = 0; iVar2 < 0x101; iVar2 = iVar2 + 1) {
            (&DAT_10036550)[iVar2] = ptVar3->mbctype[iVar2 + 4];
          }
          for (iVar2 = 0; iVar2 < 0x100; iVar2 = iVar2 + 1) {
            (&DAT_10036658)[iVar2] = ptVar3->mbcasemap[iVar2 + 4];
          }
          LVar4 = InterlockedDecrement((LONG *)PTR_DAT_10036758);
          if ((LVar4 == 0) && (PTR_DAT_10036758 != &DAT_10036330)) {
            FUN_1000e140(PTR_DAT_10036758);
          }
          PTR_DAT_10036758 = (undefined *)ptVar3;
          InterlockedIncrement((LONG *)ptVar3);
          FUN_1001d524();
        }
      }
      else if (local_24 == -1) {
        if (ptVar3 != (pthreadmbcinfo)&DAT_10036330) {
          FUN_1000e140(ptVar3);
        }
        pDVar5 = FUN_1001c00a();
        *pDVar5 = 0x16;
      }
    }
  }
  return local_24;
}


// 1001d524 FUN_1001d524

void FUN_1001d524(void)

{
  FUN_1001cdb5(0xd);
  return;
}


// 1001d55d ___initmbctable

/* Library Function - Single Match
    ___initmbctable
   
   Library: Visual Studio 2005 Release */

undefined4 ___initmbctable(void)

{
  if (DAT_1003b794 == 0) {
    __setmbcp(-3);
    DAT_1003b794 = 1;
  }
  return 0;
}


// 1001d57b ___freetlocinfo

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
       (*(undefined ***)(param_1 + 0xbc) != &PTR_DAT_10036df0)) &&
      (*(int **)(param_1 + 0xb0) != (int *)0x0)) && (**(int **)(param_1 + 0xb0) == 0)) {
    piVar4 = *(int **)(param_1 + 0xb8);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_1000e140(piVar4);
      ___free_lconv_mon(*(int *)(param_1 + 0xbc));
    }
    piVar4 = *(int **)(param_1 + 0xb4);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_1000e140(piVar4);
      ___free_lconv_num(*(int **)(param_1 + 0xbc));
    }
    FUN_1000e140(*(undefined4 *)(param_1 + 0xb0));
    FUN_1000e140(*(undefined4 *)(param_1 + 0xbc));
  }
  if ((*(int **)(param_1 + 0xc0) != (int *)0x0) && (**(int **)(param_1 + 0xc0) == 0)) {
    FUN_1000e140(*(int *)(param_1 + 0xc4) + -0xfe);
    FUN_1000e140(*(int *)(param_1 + 0xcc) + -0x80);
    FUN_1000e140(*(int *)(param_1 + 0xd0) + -0x80);
    FUN_1000e140(*(undefined4 *)(param_1 + 0xc0));
  }
  ppuVar1 = *(undefined ***)(param_1 + 0xd4);
  if ((ppuVar1 != &PTR_DAT_10036d30) && (ppuVar1[0x2d] == (undefined *)0x0)) {
    ___free_lc_time(ppuVar1);
    FUN_1000e140(*(undefined4 *)(param_1 + 0xd4));
  }
  piVar4 = (int *)(param_1 + 0x50);
  iVar3 = 6;
  do {
    if ((((undefined *)piVar4[-2] != &DAT_10036858) &&
        (piVar2 = (int *)*piVar4, piVar2 != (int *)0x0)) && (*piVar2 == 0)) {
      FUN_1000e140(piVar2);
    }
    if (((piVar4[-1] != 0) && (piVar2 = (int *)piVar4[1], piVar2 != (int *)0x0)) && (*piVar2 == 0))
    {
      FUN_1000e140(piVar2);
    }
    piVar4 = piVar4 + 4;
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  FUN_1000e140(param_1);
  return;
}


// 1001d6bb ___addlocaleref

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
    if (((undefined *)pLVar1[-2] != &DAT_10036858) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 1001d741 ___removelocaleref

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
      if (((undefined *)pLVar1[-2] != &DAT_10036858) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 1001d7cd __updatetlocinfoEx_nolock

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
        if ((*pLVar1 == 0) && (pLVar1 != (LONG *)&DAT_10036860)) {
          ___freetlocinfo((int)pLVar1);
        }
      }
    }
    return unaff_EDI;
  }
  return (LONG *)0x0;
}


// 1001d80b ___updatetlocinfo

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___updatetlocinfo
   
   Library: Visual Studio 2005 Release */

pthreadlocinfo __cdecl ___updatetlocinfo(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = __getptd();
  if (((p_Var1->_ownlocale & DAT_10036854) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xc);
    __updatetlocinfoEx_nolock();
    FUN_1001d875();
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


// 1001d875 FUN_1001d875

void FUN_1001d875(void)

{
  FUN_1001cdb5(0xc);
  return;
}


// 1001d881 __security_check_cookie

/* Library Function - Single Match
    @__security_check_cookie@4
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release
   __fastcall __security_check_cookie,4 */

void __fastcall __security_check_cookie(int param_1)

{
  if (param_1 == DAT_10036948) {
    return;
  }
                    /* WARNING: Subroutine does not return */
  ___report_gsfailure();
}


// 1001d890 __crtLCMapStringA_stat

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  if (DAT_1003a1e0 == 0) {
    iVar3 = LCMapStringW(0,0x100,L"",1,(LPWSTR)0x0,0);
    if (iVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_1003a1e0 = 2;
      }
    }
    else {
      DAT_1003a1e0 = 1;
    }
  }
  pcVar5 = (char *)param_3;
  pcVar8 = param_4;
  if (0 < (int)param_4) {
    do {
      pcVar8 = pcVar8 + -1;
      if (*pcVar5 == '\0') goto LAB_1001d8ff;
      pcVar5 = pcVar5 + 1;
    } while (pcVar8 != (char *)0x0);
    pcVar8 = (char *)0xffffffff;
LAB_1001d8ff:
    pcVar5 = param_4 + -(int)pcVar8;
    bVar2 = (int)(pcVar5 + -1) < (int)param_4;
    param_4 = pcVar5 + -1;
    if (bVar2) {
      param_4 = pcVar5;
    }
  }
  if ((DAT_1003a1e0 == 2) || (DAT_1003a1e0 == 0)) {
    local_10 = (LPCWSTR)0x0;
    local_14 = 0;
    if (param_1 == (localeinfo_struct *)0x0) {
      param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
    }
    if (param_7 == 0) {
      param_7 = *(int *)(*in_ECX + 4);
    }
    UVar7 = ___ansicp((LCID)param_1);
    if (UVar7 == 0xffffffff) goto LAB_1001dc20;
    if (UVar7 == param_7) {
      LCMapStringA((LCID)param_1,param_2,(LPCSTR)param_3,(int)param_4,(LPSTR)param_5,(int)param_6);
    }
    else {
      local_10 = (LPCWSTR)___convertcp(param_7,UVar7,(char *)param_3,(uint *)&param_4,(LPSTR)0x0,0);
      if (local_10 == (LPCWSTR)0x0) goto LAB_1001dc20;
      local_c = LCMapStringA((LCID)param_1,param_2,(LPCSTR)local_10,(int)param_4,(LPSTR)0x0,0);
      if (local_c != 0) {
        if (((int)local_c < 1) || (0xffffffe0 < local_c)) {
          pcVar5 = (char *)0x0;
        }
        else if (local_c + 8 < 0x401) {
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_1001dbfd;
          pcVar5 = &stack0xffffffe4;
        }
        else {
          pcVar5 = (char *)FUN_1000dfa0(local_c + 8);
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
LAB_1001dbfd:
    if (local_10 != (LPCWSTR)0x0) {
      FUN_1000e140(local_10);
    }
    if ((local_14 != 0) && (param_5 != local_14)) {
      FUN_1000e140(local_14);
    }
    goto LAB_1001dc20;
  }
  if (DAT_1003a1e0 != 1) goto LAB_1001dc20;
  local_c = 0;
  if (param_7 == 0) {
    param_7 = *(int *)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar(param_7,(uint)(param_8 != 0) * 8 + 1,(LPCSTR)param_3,
                                    (int)param_4,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_1001dc20;
  if (((int)cchWideChar < 1) || (0xffffffe0 / cchWideChar < 2)) {
    local_10 = (LPCWSTR)0x0;
  }
  else {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar6 = (LPCWSTR)&stack0xffffffdc;
      local_10 = (LPCWSTR)&stack0xffffffdc;
      if (&stack0x00000000 != (undefined1 *)0x24) {
LAB_1001d9a7:
        local_10 = pWVar6 + 4;
      }
    }
    else {
      pWVar6 = (LPCWSTR)FUN_1000dfa0(uVar1);
      local_10 = pWVar6;
      if (pWVar6 != (LPCWSTR)0x0) {
        pWVar6[0] = L'\xdddd';
        pWVar6[1] = L'\0';
        goto LAB_1001d9a7;
      }
    }
  }
  if (local_10 == (LPCWSTR)0x0) goto LAB_1001dc20;
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
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_1001dab6;
          pWVar6 = (LPCWSTR)&stack0xffffffe4;
        }
        else {
          pWVar6 = (LPCWSTR)FUN_1000dfa0(uVar1);
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
LAB_1001dab6:
  __freea(local_10);
LAB_1001dc20:
  iVar3 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar3;
}


// 1001dc32 ___crtLCMapStringA

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


// 1001dc75 __isleadbyte_l

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


// 1001dcab _isleadbyte

/* Library Function - Single Match
    _isleadbyte
   
   Library: Visual Studio 2005 Release */

int __cdecl _isleadbyte(int _C)

{
  int iVar1;
  
  iVar1 = __isleadbyte_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 1001dcb9 __isctype_l

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


// 1001dd6f __fltin2

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  local_24 = _Str;
  uVar3 = 0;
  local_18 = FUN_1001deab((undefined2 *)&local_14,&local_28,_Str,0,0,0,0,in_stack_00000018);
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


// 1001de20 _strlen

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
    if ((char)uVar1 == '\0') goto LAB_1001de83;
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
LAB_1001de83:
  return (size_t)((int)puVar2 + (-1 - (int)_Str));
}


// 1001deab FUN_1001deab

void __cdecl
FUN_1001deab(undefined2 *param_1,int *param_2,char *param_3,int param_4,int param_5,int param_6,
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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
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
LAB_1001df30:
    cVar8 = *param_3;
    pcVar17 = param_3 + 1;
    switch(iVar14) {
    case 0:
      if ((byte)(cVar8 - 0x31U) < 9) {
LAB_1001df4d:
        iVar14 = 3;
        goto LAB_1001df4f;
      }
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) {
LAB_1001df64:
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
          if (cVar8 == '0') goto LAB_1001df7e;
          goto LAB_1001e109;
        }
        iVar14 = 2;
        uVar7 = 0x8000;
        param_3 = pcVar17;
      }
      goto LAB_1001df30;
    case 1:
      bVar4 = true;
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_1001df4d;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_1001dfb5;
      if ((cVar8 == '+') || (cVar8 == '-')) goto LAB_1001dfe5;
      if (cVar8 == '0') goto LAB_1001df7e;
      goto LAB_1001dfc5;
    case 2:
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_1001df4d;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_1001df64;
      param_3 = local_58;
      if (cVar8 != '0') goto LAB_1001e134;
LAB_1001df7e:
      iVar14 = 1;
      param_3 = pcVar17;
      goto LAB_1001df30;
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
      if (cVar8 != *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_1001e058;
LAB_1001dfb5:
      bVar4 = true;
      iVar14 = 4;
      param_3 = pcVar17;
      goto LAB_1001df30;
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
LAB_1001e058:
      if ((cVar8 == '+') || (cVar8 == '-')) {
LAB_1001dfe5:
        bVar4 = true;
        iVar14 = 0xb;
        param_3 = pcVar17 + -1;
      }
      else {
LAB_1001dfc5:
        bVar4 = true;
        if ((cVar8 < 'D') || (('E' < cVar8 && ((cVar8 < 'd' || ('e' < cVar8)))))) goto LAB_1001e109;
        iVar14 = 6;
        param_3 = pcVar17;
      }
      goto LAB_1001df30;
    case 5:
      bVar6 = true;
      param_3 = local_58;
      if ((byte)(cVar8 - 0x30U) < 10) {
        iVar14 = 4;
        goto LAB_1001df4f;
      }
      goto LAB_1001e134;
    case 6:
      local_58 = param_3 + -1;
      if (8 < (byte)(cVar8 - 0x31U)) {
        if (cVar8 == '+') goto LAB_1001e0f0;
        if (cVar8 == '-') goto LAB_1001e0e4;
LAB_1001e0d7:
        param_3 = local_58;
        if (cVar8 != '0') goto LAB_1001e134;
        iVar14 = 8;
        param_3 = pcVar17;
        goto LAB_1001df30;
      }
      break;
    case 7:
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_1001e0d7;
      break;
    case 8:
      bVar5 = true;
      while (cVar8 == '0') {
        cVar8 = *pcVar17;
        pcVar17 = pcVar17 + 1;
      }
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_1001e109;
      break;
    case 9:
      bVar5 = true;
      local_68 = 0;
      goto LAB_1001e196;
    default:
      goto switchD_1001df3c_caseD_a;
    case 0xb:
      if (param_7 != 0) {
        local_58 = param_3;
        if (cVar8 == '+') {
LAB_1001e0f0:
          iVar14 = 7;
          param_3 = pcVar17;
        }
        else {
          if (cVar8 != '-') goto LAB_1001e134;
LAB_1001e0e4:
          local_6c = -1;
          iVar14 = 7;
          param_3 = pcVar17;
        }
        goto LAB_1001df30;
      }
      iVar14 = 10;
      pcVar17 = param_3;
switchD_1001df3c_caseD_a:
      param_3 = pcVar17;
      if (iVar14 != 10) goto LAB_1001df30;
      goto LAB_1001e134;
    }
    iVar14 = 9;
LAB_1001df4f:
    param_3 = pcVar17 + -1;
    goto LAB_1001df30;
  }
  pDVar10 = FUN_1001c00a();
  *pDVar10 = 0x16;
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
  goto LAB_1001e52c;
LAB_1001e196:
  if ((cVar8 < '0') || ('9' < cVar8)) goto LAB_1001e1b1;
  local_68 = local_68 * 10 + -0x30 + (int)cVar8;
  if (local_68 < 0x1451) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
    goto LAB_1001e196;
  }
  local_68 = 0x1451;
LAB_1001e1b1:
  while (('/' < cVar8 && (cVar8 < ':'))) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
  }
LAB_1001e109:
  param_3 = pcVar17 + -1;
LAB_1001e134:
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
    if (local_50 == 0) goto LAB_1001e50e;
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
      if ((int)local_58 < -0x1450) goto LAB_1001e50e;
      ppuVar20 = &PTR_DAT_10036e00;
      if (local_58 != (char *)0x0) {
        if ((int)local_58 < 0) {
          local_58 = (char *)-(int)local_58;
          ppuVar20 = (undefined **)&DAT_10036f60;
        }
        if (param_4 == 0) {
          local_40._0_2_ = 0;
        }
        iVar14 = uStack_4a;
        uVar22 = uStack_2e;
        iVar1 = uStack_2a;
        uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
        iVar12 = local_3c;
joined_r0x1001e23c:
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
LAB_1001e3f2:
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
                    if ((short)uVar24 < 1) goto LAB_1001e3f2;
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
                  if (0x7ffe < uVar24) goto LAB_1001e4b1;
                  local_40 = uVar22;
                  local_3c = iVar1;
                  uStack_38 = uStack_26;
                  local_36 = uVar24 | uVar9;
                  uVar2 = uVar22;
                  iVar12 = iVar1;
                }
                goto joined_r0x1001e23c;
              }
              iVar12 = 0;
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            else {
LAB_1001e4b1:
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
          goto joined_r0x1001e23c;
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
LAB_1001e50e:
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
LAB_1001e52c:
  local_40 = uVar2;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001e570 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_1001deab((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tod(&local_14,(_CRT_DOUBLE *)_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_1001e5c7:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1001e607;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_1001e5f9:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1001e607;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_1001e5f9;
    goto LAB_1001e5c7;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_1001e607:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 1001e616 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_1001deab((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tof(&local_14,_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_1001e66d:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1001e6ad;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_1001e69f:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1001e6ad;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_1001e69f;
    goto LAB_1001e66d;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_1001e6ad:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 1001e6bc _strcpy_s

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
      pDVar2 = FUN_1001c00a();
      eVar4 = 0x22;
      *pDVar2 = 0x22;
      goto LAB_1001e6db;
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_1001c00a();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_1001e6db:
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 1001e721 __fptostr

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
    pDVar1 = FUN_1001c00a();
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
    pDVar1 = FUN_1001c00a();
    eVar7 = 0x22;
    *pDVar1 = 0x22;
  }
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar7;
}


// 1001e7de ___dtold

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
      goto LAB_1001e890;
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
LAB_1001e890:
  *(ushort *)(param_1 + 2) = uVar4;
  return;
}


// 1001e899 __fltout2

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  ___dtold(&local_14,(uint *)&_Dbl);
  iVar1 = FUN_10022044(local_14,uStack_10,uStack_c,0x11,0,&local_30);
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


// 1001e930 __alldvrm

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


// 1001ea10 __aullshr

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


// 1001ea38 __controlfp_s

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
    pDVar2 = FUN_1001c00a();
    eVar3 = 0x16;
    *pDVar2 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return eVar3;
}


// 1001eaa2 __initp_misc_winsig

/* Library Function - Single Match
    __initp_misc_winsig
   
   Library: Visual Studio 2005 Release */

void __cdecl __initp_misc_winsig(undefined4 param_1)

{
  DAT_1003a224 = param_1;
  DAT_1003a228 = param_1;
  DAT_1003a22c = param_1;
  DAT_1003a230 = param_1;
  return;
}


// 1001eabb siglookup

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
  } while (uVar1 < DAT_1003620c * 0xc + param_1);
  if ((DAT_1003620c * 0xc + param_1 <= uVar1) || (*(int *)(uVar1 + 4) != in_EDX)) {
    uVar1 = 0;
  }
  return uVar1;
}


// 1001eaef ___get_sigabrt

/* Library Function - Single Match
    ___get_sigabrt
   
   Library: Visual Studio 2005 Release */

_PHNDLR __cdecl ___get_sigabrt(void)

{
  _PHNDLR p_Var1;
  
  p_Var1 = (_PHNDLR)FUN_1001a665(DAT_1003a22c);
  return p_Var1;
}


// 1001eafc _raise

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
        puVar6 = &DAT_1003a224;
        iVar3 = DAT_1003a224;
        goto LAB_1001ebb1;
      }
      if (_SigNum != 4) {
        if (_SigNum == 6) goto LAB_1001eb8f;
        if (_SigNum != 8) goto LAB_1001eb73;
      }
    }
    pDVar7 = FUN_1001a80a();
    if (pDVar7 == (DWORD *)0x0) {
      return -1;
    }
    uVar2 = siglookup(pDVar7[0x17]);
    puVar6 = (undefined4 *)(uVar2 + 8);
    pcVar4 = (code *)*puVar6;
  }
  else {
    if (_SigNum == 0xf) {
      puVar6 = &DAT_1003a230;
      iVar3 = DAT_1003a230;
    }
    else if (_SigNum == 0x15) {
      puVar6 = &DAT_1003a228;
      iVar3 = DAT_1003a228;
    }
    else {
      if (_SigNum != 0x16) {
LAB_1001eb73:
        pDVar7 = FUN_1001c00a();
        *pDVar7 = 0x16;
        FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return -1;
      }
LAB_1001eb8f:
      puVar6 = &DAT_1003a22c;
      iVar3 = DAT_1003a22c;
    }
LAB_1001ebb1:
    bVar1 = true;
    pcVar4 = (code *)FUN_1001a665(iVar3);
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
      goto LAB_1001ec15;
    }
  }
  else {
LAB_1001ec15:
    if (_SigNum == 8) {
      for (local_28 = DAT_10036200; local_28 < DAT_10036204 + DAT_10036200; local_28 = local_28 + 1)
      {
        *(undefined4 *)(local_28 * 0xc + 8 + pDVar7[0x17]) = 0;
      }
      goto LAB_1001ec4f;
    }
  }
  uVar5 = __encoded_null();
  *puVar6 = uVar5;
LAB_1001ec4f:
  FUN_1001ec70();
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


// 1001ec70 FUN_1001ec70

void FUN_1001ec70(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + -0x1c) != 0) {
    FUN_1001cdb5(0);
  }
  return;
}


// 1001ecac FUN_1001ecac

undefined4 __cdecl FUN_1001ecac(undefined4 param_1,undefined4 param_2,uint param_3)

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
  if (DAT_1003a238 == 0) {
    hModule = LoadLibraryA("USER32.DLL");
    if (hModule == (HMODULE)0x0) {
      return 0;
    }
    pFVar1 = GetProcAddress(hModule,"MessageBoxA");
    if (pFVar1 == (FARPROC)0x0) {
      return 0;
    }
    DAT_1003a238 = FUN_1001a5ee((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetActiveWindow");
    DAT_1003a23c = FUN_1001a5ee((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetLastActivePopup");
    DAT_1003a240 = FUN_1001a5ee((int)pFVar1);
    iVar2 = __get_osplatform(&local_10);
    if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
    if (local_10 == 2) {
      pFVar1 = GetProcAddress(hModule,"GetUserObjectInformationA");
      DAT_1003a248 = FUN_1001a5ee((int)pFVar1);
      if (DAT_1003a248 != 0) {
        pFVar1 = GetProcAddress(hModule,"GetProcessWindowStation");
        DAT_1003a244 = FUN_1001a5ee((int)pFVar1);
      }
    }
  }
  if ((DAT_1003a244 != local_c) && (DAT_1003a248 != local_c)) {
    pcVar3 = (code *)FUN_1001a665(DAT_1003a244);
    pcVar4 = (code *)FUN_1001a665(DAT_1003a248);
    if (((pcVar3 != (code *)0x0) && (pcVar4 != (code *)0x0)) &&
       (((iVar2 = (*pcVar3)(), iVar2 == 0 ||
         (iVar2 = (*pcVar4)(iVar2,1,local_24,0xc,local_18), iVar2 == 0)) || ((local_1c & 1) == 0))))
    {
      iVar2 = FUN_1001ad8b(&local_14);
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
      goto LAB_1001ee42;
    }
  }
  if ((((DAT_1003a23c != local_c) &&
       (pcVar3 = (code *)FUN_1001a665(DAT_1003a23c), pcVar3 != (code *)0x0)) &&
      (local_8 = (*pcVar3)(), local_8 != 0)) &&
     ((DAT_1003a240 != local_c &&
      (pcVar3 = (code *)FUN_1001a665(DAT_1003a240), pcVar3 != (code *)0x0)))) {
    local_8 = (*pcVar3)(local_8);
  }
LAB_1001ee42:
  pcVar3 = (code *)FUN_1001a665(DAT_1003a238);
  if (pcVar3 == (code *)0x0) {
    return 0;
  }
  uVar5 = (*pcVar3)(local_8,param_1,param_2,param_3);
  return uVar5;
}


// 1001ee69 _strcat_s

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
        pDVar2 = FUN_1001c00a();
        eVar4 = 0x22;
        *pDVar2 = 0x22;
        goto LAB_1001ee88;
      }
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_1001c00a();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_1001ee88:
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 1001eeda _strncpy_s

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
LAB_1001eefe:
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
          pDVar2 = FUN_1001c00a();
          eVar5 = 0x22;
          *pDVar2 = 0x22;
          goto LAB_1001ef0f;
        }
        *_Dst = '\0';
      }
    }
  }
  else if (_Dst != (char *)0x0) goto LAB_1001eefe;
  pDVar2 = FUN_1001c00a();
  eVar5 = 0x16;
  *pDVar2 = 0x16;
LAB_1001ef0f:
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar5;
}


// 1001ef8d __set_error_mode

/* Library Function - Single Match
    __set_error_mode
   
   Library: Visual Studio 2005 Release */

int __cdecl __set_error_mode(int _Mode)

{
  int iVar1;
  DWORD *pDVar2;
  
  if (-1 < _Mode) {
    if (_Mode < 3) {
      iVar1 = DAT_10039bac;
      DAT_10039bac = _Mode;
      return iVar1;
    }
    if (_Mode == 3) {
      return DAT_10039bac;
    }
  }
  pDVar2 = FUN_1001c00a();
  *pDVar2 = 0x16;
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1001efd3 FUN_1001efd3

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

undefined4 FUN_1001efd3(void)

{
  return 1;
}


// 1001f023 __get_sse2_info

/* WARNING: Removing unreachable block (ram,0x1001f05e) */
/* WARNING: Removing unreachable block (ram,0x1001f04b) */
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
  if (((local_8 & 0x4000000) == 0) || (iVar2 = FUN_1001efd3(), iVar2 == 0)) {
    uVar3 = 0;
  }
  else {
    uVar3 = 1;
  }
  return uVar3;
}


// 1001f090 __ValidateImageBase

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


// 1001f0c0 __FindPESection

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


// 1001f110 __IsNonwritableInCurrentImage

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
  local_c = DAT_10036948 ^ 0x1002ac38;
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


// 1001f204 __initp_eh_hooks

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __initp_eh_hooks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __initp_eh_hooks(void)

{
  _DAT_1003a24c = FUN_1001a5ee(0x1001f1cb);
  return;
}


// 1001f215 FUN_1001f215

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1001f215(undefined4 param_1)

{
  _DAT_1003a250 = param_1;
  return;
}


// 1001f21f FUN_1001f21f

void __cdecl FUN_1001f21f(undefined4 param_1)

{
  DAT_1003a254 = param_1;
  return;
}


// 1001f229 ___crtInitCritSecNoSpinCount@8

/* Library Function - Single Match
    ___crtInitCritSecNoSpinCount@8
   
   Library: Visual Studio 2005 Release */

undefined4 ___crtInitCritSecNoSpinCount_8(LPCRITICAL_SECTION param_1)

{
  InitializeCriticalSection(param_1);
  return 1;
}


// 1001f239 ___crtInitCritSecAndSpinCount

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
  
  local_8 = &DAT_1002ac78;
  uStack_c = 0x1001f245;
  local_20[0] = 0;
  pcVar1 = (code *)FUN_1001a665(DAT_1003a254);
  if (pcVar1 != (FARPROC)0x0) goto LAB_1001f2af;
  iVar2 = __get_osplatform(local_20);
  if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  if (local_20[0] == 1) {
LAB_1001f29e:
    pcVar1 = ___crtInitCritSecNoSpinCount_8;
  }
  else {
    hModule = GetModuleHandleA("kernel32.dll");
    if (hModule == (HMODULE)0x0) goto LAB_1001f29e;
    pcVar1 = GetProcAddress(hModule,"InitializeCriticalSectionAndSpinCount");
    if (pcVar1 == (FARPROC)0x0) goto LAB_1001f29e;
  }
  DAT_1003a254 = FUN_1001a5ee((int)pcVar1);
LAB_1001f2af:
  local_8 = (undefined *)0x0;
  iVar2 = (*pcVar1)(param_1,param_2);
  return iVar2;
}


// 1001f2fe FUN_1001f2fe

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1001f2fe(undefined4 param_1)

{
  _DAT_1003a258 = param_1;
  return;
}


// 1001f308 __local_unwind4

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
  puStack_24 = &LAB_1001f398;
  uStack_28 = *unaff_FS_OFFSET;
  local_20 = DAT_10036948 ^ (uint)&uStack_28;
  *unaff_FS_OFFSET = &uStack_28;
  while( true ) {
    uVar2 = *(uint *)(param_2 + 0xc);
    if ((uVar2 == 0xfffffffe) || ((param_3 != 0xfffffffe && (uVar2 <= param_3)))) break;
    puVar1 = (undefined4 *)((*(uint *)(param_2 + 8) ^ *param_1) + 0x10 + uVar2 * 0xc);
    *(undefined4 *)(param_2 + 0xc) = *puVar1;
    if (puVar1[1] == 0) {
      __NLG_Notify(0x101);
      FUN_10022e70();
    }
  }
  *unaff_FS_OFFSET = uStack_28;
  return;
}


// 1001f3de FUN_1001f3de

void FUN_1001f3de(int param_1)

{
  __local_unwind4(*(uint **)(param_1 + 0x28),*(int *)(param_1 + 0x18),*(uint *)(param_1 + 0x1c));
  return;
}


// 1001f3fa _EH4_CallFilterFunc

/* Library Function - Single Match
    @_EH4_CallFilterFunc@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_CallFilterFunc,8 */

void __fastcall _EH4_CallFilterFunc(undefined *param_1)

{
  (*(code *)param_1)();
  return;
}


// 1001f411 _EH4_TransferToHandler

/* Library Function - Single Match
    @_EH4_TransferToHandler@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_TransferToHandler,8 */

void __fastcall _EH4_TransferToHandler(undefined *UNRECOVERED_JUMPTABLE)

{
  __NLG_Notify(1);
                    /* WARNING: Could not recover jumptable at 0x1001f428. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (*(code *)UNRECOVERED_JUMPTABLE)();
  return;
}


// 1001f42a _EH4_GlobalUnwind

/* Library Function - Single Match
    @_EH4_GlobalUnwind@4
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_GlobalUnwind,4 */

void __fastcall _EH4_GlobalUnwind(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x1001f43f,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 1001f444 _EH4_LocalUnwind

/* Library Function - Single Match
    @_EH4_LocalUnwind@16
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_LocalUnwind,16 */

void __fastcall _EH4_LocalUnwind(int param_1,uint param_2,undefined4 param_3,uint *param_4)

{
  __local_unwind4(param_4,param_1,param_2);
  return;
}


// 1001f45b __lseeki64_nolock

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
    pDVar2 = FUN_1001c00a();
    *pDVar2 = 9;
LAB_1001f48a:
    DVar3 = 0xffffffff;
    local_8 = -1;
  }
  else {
    DVar3 = SetFilePointer(hFile,in_stack_00000008,&local_8,_Offset._4_4_);
    if (DVar3 == 0xffffffff) {
      DVar4 = GetLastError();
      if (DVar4 != 0) {
        FUN_1001c030(DVar4);
        goto LAB_1001f48a;
      }
    }
    pbVar1 = (byte *)((&DAT_1003b680)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38);
    *pbVar1 = *pbVar1 & 0xfd;
  }
  return CONCAT44(local_8,DVar3);
}


// 1001f4de __lseeki64

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
    pDVar1 = FUN_1001c01d();
    *pDVar1 = 0;
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1003b664)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_1003b680)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
        pDVar1 = FUN_1001c01d();
        *pDVar1 = 0;
        pDVar1 = FUN_1001c00a();
        *pDVar1 = 9;
        FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        local_28._4_4_ = 0xffffffff;
        local_28._0_4_ = 0xffffffff;
      }
      else {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_1003b680)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_1001c00a();
          *pDVar1 = 9;
          pDVar1 = FUN_1001c01d();
          *pDVar1 = 0;
          local_28 = -1;
        }
        else {
          local_28 = __lseeki64_nolock(_FileHandle,_Offset,in_stack_ffffffc8);
        }
        FUN_1001f5ed();
      }
      goto LAB_1001f5e7;
    }
    pDVar1 = FUN_1001c01d();
    *pDVar1 = 0;
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 9;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  local_28._0_4_ = 0xffffffff;
  local_28._4_4_ = 0xffffffff;
LAB_1001f5e7:
  return CONCAT44(local_28._4_4_,(undefined4)local_28);
}


// 1001f5ed FUN_1001f5ed

void FUN_1001f5ed(void)

{
  int unaff_EBP;
  
  FUN_10023005(*(uint *)(unaff_EBP + 8));
  return;
}


// 1001f5f7 __write_nolock

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
  
  local_8 = DAT_10036948 ^ (uint)local_51c;
  local_580 = (WCHAR *)_Buf;
  local_57c = (char *)0x0;
  local_584 = 0;
  if (_MaxCharCount == 0) goto LAB_1001fba7;
  if (_Buf == (void *)0x0) {
    pDVar6 = FUN_1001c01d();
    *pDVar6 = 0;
    pDVar6 = FUN_1001c00a();
    *pDVar6 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_1001fba7;
  }
  iVar11 = (_FileHandle & 0x1fU) * 0x38;
  piVar16 = &DAT_1003b680 + (_FileHandle >> 5);
  local_571 = (char)(*(char *)(*piVar16 + iVar11 + 0x24) * '\x02') >> 1;
  local_58c = piVar16;
  if (((local_571 == '\x02') || (local_571 == '\x01')) && ((~_MaxCharCount & 1) == 0)) {
    pDVar6 = FUN_1001c01d();
    *pDVar6 = 0;
    pDVar6 = FUN_1001c00a();
    *pDVar6 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_1001fba7;
  }
  if ((*(byte *)(*piVar16 + iVar11 + 4) & 0x20) != 0) {
    __lseeki64_nolock(_FileHandle,0x200000000,unaff_EDI);
  }
  iVar7 = __isatty(_FileHandle);
  if ((iVar7 == 0) || ((*(byte *)(iVar11 + 4 + *piVar16) & 0x80) == 0)) {
LAB_1001f8e2:
    if ((*(byte *)((undefined4 *)(*piVar16 + iVar11) + 1) & 0x80) == 0) {
      BVar9 = WriteFile(*(HANDLE *)(*piVar16 + iVar11),local_580,_MaxCharCount,(LPDWORD)&local_588,
                        (LPOVERLAPPED)0x0);
      if (BVar9 == 0) {
LAB_1001fb3c:
        local_570 = GetLastError();
      }
      else {
        local_570 = 0;
        local_57c = local_588;
      }
LAB_1001fb45:
      piVar16 = local_58c;
      if (local_57c != (char *)0x0) goto LAB_1001fba7;
      goto LAB_1001fb4f;
    }
    local_570 = 0;
    if (local_571 == '\0') {
      local_56c = local_580;
      if (_MaxCharCount == 0) goto LAB_1001fb76;
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
        if (BVar9 == 0) goto LAB_1001fb3c;
        local_57c = local_57c + (int)local_588;
      } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
              ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
      goto LAB_1001fb45;
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
          if (BVar9 == 0) goto LAB_1001fb3c;
          local_57c = local_57c + (int)local_588;
        } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
                ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
        goto LAB_1001fb45;
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
          if (iVar7 == 0) goto LAB_1001fb3c;
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
        goto LAB_1001fb45;
      }
    }
LAB_1001fb76:
    DVar10 = 0;
    if (((*(byte *)(iVar11 + 4 + *piVar16) & 0x40) != 0) && ((char)*local_580 == '\x1a'))
    goto LAB_1001fba7;
    pDVar6 = FUN_1001c00a();
    *pDVar6 = 0x1c;
  }
  else {
    p_Var8 = __getptd();
    pwVar3 = p_Var8->ptlocinfo->lc_category[0].wlocale;
    BVar9 = GetConsoleMode(*(HANDLE *)(iVar11 + *piVar16),&local_598);
    if ((BVar9 == 0) || ((pwVar3 == (wchar_t *)0x0 && (local_571 == '\0')))) goto LAB_1001f8e2;
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
          if (BVar9 == 0) goto LAB_1001fb3c;
          local_57c = local_57c + (int)local_56c;
          if ((int)local_56c < (int)DVar10) break;
          if (local_594 != 0) {
            local_10[0] = '\r';
            BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_10,1,(LPDWORD)&local_56c,
                              (LPOVERLAPPED)0x0);
            if (BVar9 == 0) goto LAB_1001fb3c;
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
            if (wVar5 != (wchar_t)local_570) goto LAB_1001fb3c;
            local_57c = local_57c + 1;
            if (local_594 != 0) {
              local_570 = 0xd;
              wVar5 = __putwch_nolock(L'\r');
              if (wVar5 != (wchar_t)local_570) goto LAB_1001fb3c;
              local_57c = local_57c + 1;
              local_584 = local_584 + 1;
            }
          }
        }
      } while (local_578 < _MaxCharCount);
      goto LAB_1001fb45;
    }
LAB_1001fb4f:
    if (local_570 == 0) goto LAB_1001fb76;
    DVar10 = 5;
    if (local_570 != 5) {
      FUN_1001c030(local_570);
      goto LAB_1001fba7;
    }
    pDVar6 = FUN_1001c00a();
    *pDVar6 = 9;
  }
  pDVar6 = FUN_1001c01d();
  *pDVar6 = DVar10;
LAB_1001fba7:
  iVar11 = __security_check_cookie(local_8 ^ (uint)local_51c);
  return iVar11;
}


// 1001fbbd __write

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
    pDVar1 = FUN_1001c01d();
    *pDVar1 = 0;
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1003b664)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_1003b680)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_1003b680)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_1001c00a();
          *pDVar1 = 9;
          pDVar1 = FUN_1001c01d();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = __write_nolock(_FileHandle,_Buf,_MaxCharCount);
        }
        FUN_1001fc8f();
        return local_20;
      }
    }
    pDVar1 = FUN_1001c01d();
    *pDVar1 = 0;
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 9;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 1001fc8f FUN_1001fc8f

void FUN_1001fc8f(void)

{
  int unaff_EBP;
  
  FUN_10023005(*(uint *)(unaff_EBP + 8));
  return;
}


// 1001fc99 __getbuf

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __getbuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __getbuf(FILE *_File)

{
  char *pcVar1;
  
  _DAT_1003a25c = _DAT_1003a25c + 1;
  pcVar1 = (char *)FUN_1000dfa0(0x1000);
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


// 1001fcdd __isatty

/* Library Function - Single Match
    __isatty
   
   Library: Visual Studio 2005 Release */

int __cdecl __isatty(int _FileHandle)

{
  DWORD *pDVar1;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 9;
    return 0;
  }
  if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1003b664)) {
    return *(byte *)((&DAT_1003b680)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38) & 0x40;
  }
  pDVar1 = FUN_1001c00a();
  *pDVar1 = 9;
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0;
}


// 1001fd3b FUN_1001fd3b

undefined ** FUN_1001fd3b(void)

{
  return &PTR_DAT_10036970;
}


// 1001fedc __fileno

/* Library Function - Single Match
    __fileno
   
   Library: Visual Studio 2005 Release */

int __cdecl __fileno(FILE *_File)

{
  DWORD *pDVar1;
  
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 0x16;
    FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  return _File->_file;
}


// 1001ff09 FUN_1001ff09

bool FUN_1001ff09(void)

{
  return DAT_1003a260 == (DAT_10036948 | 1);
}


// 1001ff1f __wctomb_s_l

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
LAB_1001ff41:
    DVar1 = 0;
  }
  else {
    if (_SizeConverted != (int *)0x0) {
      *_SizeConverted = -1;
    }
    if (0x7fffffff < _SizeInBytes) {
      pDVar2 = FUN_1001c00a();
      *pDVar2 = 0x16;
      FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return 0x16;
    }
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
    if (*(int *)(local_14[0] + 0x14) == 0) {
      if ((ushort)_WCh < 0x100) {
        if (lpMultiByteStr != (char *)0x0) {
          if (_Size == 0) goto LAB_1001ffd1;
          *lpMultiByteStr = (char)_WCh;
        }
        if (_SizeConverted != (int *)0x0) {
          *_SizeConverted = 1;
        }
LAB_10020008:
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        goto LAB_1001ff41;
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
LAB_1001ffd1:
          pDVar2 = FUN_1001c00a();
          *pDVar2 = 0x22;
          FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_10020008;
      }
    }
    pDVar2 = FUN_1001c00a();
    *pDVar2 = 0x2a;
    pDVar2 = FUN_1001c00a();
    DVar1 = *pDVar2;
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return DVar1;
}


// 1002007e _wctomb_s

/* Library Function - Single Match
    _wctomb_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _wctomb_s(int *_SizeConverted,char *_MbCh,rsize_t _SizeInBytes,wchar_t _WCh)

{
  errno_t eVar1;
  
  eVar1 = __wctomb_s_l(_SizeConverted,_MbCh,_SizeInBytes,_WCh,(_locale_t)0x0);
  return eVar1;
}


// 100200a0 __aulldvrm

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


// 10020135 FUN_10020135

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10020135(void)

{
  _DAT_1003a658 = 0;
  return;
}


// 1002013d FUN_1002013d

undefined4 FUN_1002013d(void)

{
  return 0;
}


// 10020140 __raise_exc_ex

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
        goto LAB_1002029d;
      }
      uVar3 = *param_1 & 0xfffffffe | 2;
    }
    *param_1 = uVar3;
  }
LAB_1002029d:
  uVar3 = *puVar1 & 0x300;
  if (uVar3 == 0) {
    uVar3 = *param_1 & 0xffffffeb | 8;
LAB_100202d3:
    *param_1 = uVar3;
  }
  else {
    if (uVar3 == 0x200) {
      uVar3 = *param_1 & 0xffffffe7 | 4;
      goto LAB_100202d3;
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
        goto LAB_100203d8;
      }
      uVar3 = *puVar1 & 0xfffffbff | 0x800;
    }
    *puVar1 = uVar3;
  }
LAB_100203d8:
  uVar3 = *param_1 >> 2 & 7;
  if (uVar3 == 0) {
    uVar3 = *puVar1 & 0xfffff3ff | 0x300;
  }
  else {
    if (uVar3 != 1) {
      if (uVar3 == 2) {
        *puVar1 = *puVar1 & 0xfffff3ff;
      }
      goto LAB_10020404;
    }
    uVar3 = *puVar1 & 0xfffff3ff | 0x200;
  }
  *puVar1 = uVar3;
LAB_10020404:
  if (param_7 == 0) {
    *(undefined8 *)puVar2 = *(undefined8 *)(param_1 + 0x14);
  }
  else {
    *puVar2 = param_1[0x14];
  }
  return;
}


// 1002041a __handle_exc

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
    FUN_1002097e();
    uVar6 = param_1 & 0x17;
    goto LAB_100205d9;
  }
  if (((param_1 & 4) != 0) && ((param_3 & 4) != 0)) {
    FUN_1002097e();
    uVar6 = param_1 & 0x1b;
    goto LAB_100205d9;
  }
  if (((param_1 & 1) == 0) || ((param_3 & 8) == 0)) {
    if (((param_1 & 2) != 0) && ((param_3 & 0x10) != 0)) {
      bVar7 = (param_1 & 0x10) != 0;
      if (*param_2 != 0.0) {
        fVar8 = (float10)FUN_10020891(SUB84(*param_2,0),(uint)((ulonglong)*param_2 >> 0x20),&local_c
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
        FUN_1002097e();
      }
      uVar6 = local_8 & 0xfffffffd;
      local_8 = uVar6;
    }
    goto LAB_100205d9;
  }
  FUN_1002097e();
  uVar6 = param_3 & 0xc00;
  dVar2 = _DAT_10036ce0;
  dVar5 = _DAT_10036ce0;
  if (uVar6 == 0) {
    dVar1 = *param_2;
joined_r0x100204bc:
    if (dVar1 <= 0.0) {
      dVar2 = -dVar5;
    }
    *param_2 = dVar2;
  }
  else {
    if (uVar6 == 0x400) {
      dVar1 = *param_2;
      dVar2 = _DAT_10036cf0;
      goto joined_r0x100204bc;
    }
    dVar5 = _DAT_10036cf0;
    if (uVar6 == 0x800) {
      dVar1 = *param_2;
      goto joined_r0x100204bc;
    }
    if (uVar6 == 0xc00) {
      dVar1 = *param_2;
      dVar2 = _DAT_10036cf0;
      goto joined_r0x100204bc;
    }
  }
  uVar6 = param_1 & 0x1e;
LAB_100205d9:
  if (((param_1 & 0x10) != 0) && ((param_3 & 0x20) != 0)) {
    FUN_1002097e();
    uVar6 = uVar6 & 0xffffffef;
  }
  return uVar6 == 0;
}


// 100205fb __set_errno_from_matherr

/* Library Function - Single Match
    __set_errno_from_matherr
   
   Library: Visual Studio 2005 Release */

void __cdecl __set_errno_from_matherr(int param_1)

{
  DWORD *pDVar1;
  
  if (param_1 == 1) {
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 0x21;
  }
  else if ((1 < param_1) && (param_1 < 4)) {
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 0x22;
    return;
  }
  return;
}


// 10020623 __errcode

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


// 10020650 __umatherr

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
    if ((&DAT_10036bf8)[iVar1 * 2] == param_2) {
      iVar1 = *(int *)(iVar1 * 8 + 0x10036bfc);
      goto LAB_1002066c;
    }
    iVar1 = iVar1 + 1;
  } while (iVar1 < 0x1d);
  iVar1 = 0;
LAB_1002066c:
  if (iVar1 != 0) {
    __ctrlfp();
    iVar1 = FUN_1002013d();
    if (iVar1 == 0) {
      __set_errno_from_matherr(param_1);
    }
    return (float10)(double)CONCAT44(in_stack_00000020,in_stack_0000001c);
  }
  __ctrlfp();
  __set_errno_from_matherr(param_1);
  return (float10)(double)CONCAT44(in_stack_00000020,in_stack_0000001c);
}


// 100206ee __handle_qnan1

/* Library Function - Single Match
    __handle_qnan1
   
   Library: Visual Studio 2005 Release */

float10 __cdecl __handle_qnan1(int param_1,double param_2)

{
  DWORD *pDVar1;
  float10 fVar2;
  
  if (DAT_10036bf0 == 0) {
    fVar2 = __umatherr(1,param_1);
    return fVar2;
  }
  pDVar1 = FUN_1001c00a();
  *pDVar1 = 0x21;
  __ctrlfp();
  return (float10)param_2;
}


// 10020741 __except1

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
  
  local_14 = DAT_10036948 ^ (uint)local_90;
  bVar1 = __handle_exc(param_1,&param_4,param_5);
  if (CONCAT31(extraout_var,bVar1) == 0) {
    local_50 = local_50 & 0xfffffffe;
    __raise_exc_ex(local_90,&param_5,param_1,param_2,(uint *)&param_3,(uint *)&param_4,0);
  }
  cVar2 = __errcode((byte)param_1);
  iVar3 = CONCAT31(extraout_var_00,cVar2);
  if ((DAT_10036bf0 == 0) && (iVar3 != 0)) {
    __umatherr(iVar3,param_2);
  }
  else {
    __set_errno_from_matherr(iVar3);
    __ctrlfp();
  }
  __security_check_cookie(local_14 ^ (uint)local_90);
  return;
}


// 100207fb FUN_100207fb

float10 __cdecl FUN_100207fb(double param_1)

{
  return (float10)ROUND(param_1);
}


// 1002080c __set_exp

/* Library Function - Single Match
    __set_exp
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release */

float10 __cdecl __set_exp(undefined8 param_1,short param_2)

{
  undefined8 local_c;
  
  local_c = (double)CONCAT26((param_2 + 0x3fe) * 0x10 | param_1._6_2_ & 0x800f,(int6)param_1);
  return (float10)local_c;
}


// 10020836 __sptype

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


// 10020891 FUN_10020891

void __cdecl FUN_10020891(uint param_1,uint param_2,int *param_3)

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


// 10020940 __statfp

/* Library Function - Single Match
    __statfp
   
   Library: Visual Studio 2005 Release */

int __statfp(void)

{
  short in_FPUStatusWord;
  
  return (int)in_FPUStatusWord;
}


// 1002094b __clrfp

/* Library Function - Single Match
    __clrfp
   
   Library: Visual Studio 2005 Release */

int __clrfp(void)

{
  short in_FPUStatusWord;
  
  return (int)in_FPUStatusWord;
}


// 10020957 __ctrlfp

/* Library Function - Single Match
    __ctrlfp
   
   Library: Visual Studio 2005 Release */

int __ctrlfp(void)

{
  short in_FPUControlWord;
  
  return (int)in_FPUControlWord;
}


// 1002097e FUN_1002097e

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_1002097e(void)

{
  return;
}


// 100209d4 ___set_fpsr_sse2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___set_fpsr_sse2
   
   Library: Visual Studio 2005 Release */

void __cdecl ___set_fpsr_sse2(uint param_1)

{
  if (DAT_1003b7a0 != 0) {
    if (((param_1 & 0x40) == 0) || (DAT_10036d20 == 0)) {
      MXCSR = param_1 & 0xffffffbf;
    }
    else {
      MXCSR = param_1;
    }
  }
  return;
}


// 10020a46 x_ismbbtype_l

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
    if (uVar1 == 0) goto LAB_10020a88;
  }
  uVar1 = 1;
LAB_10020a88:
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 10020a97 __ismbblead

/* Library Function - Single Match
    __ismbblead
   
   Library: Visual Studio 2005 Release */

int __cdecl __ismbblead(uint _C)

{
  int iVar1;
  
  iVar1 = x_ismbbtype_l((localeinfo_struct *)0x0,_C,0,4);
  return iVar1;
}


// 10020aaa __crtGetStringTypeA_stat

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  local_c = in_ECX;
  if (DAT_1003a264 == 0) {
    BVar2 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_1003a264 = 2;
      }
      goto LAB_10020b03;
    }
    DAT_1003a264 = 1;
  }
  else {
LAB_10020b03:
    if ((DAT_1003a264 == 2) || (DAT_1003a264 == 0)) {
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
        FUN_1000e140(uVar7);
      }
      goto LAB_10020c50;
    }
    if (DAT_1003a264 != 1) goto LAB_10020c50;
  }
  local_c = (int *)0x0;
  if (param_5 == (ushort *)0x0) {
    param_5 = *(ushort **)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar((UINT)param_5,(uint)(param_7 != 0) * 8 + 1,(LPCSTR)param_2,
                                    (int)param_3,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_10020c50;
  lpWideCharStr = (LPCWSTR)0x0;
  if ((0 < (int)cchWideChar) && (cchWideChar < 0x7ffffff1)) {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar4 = (LPCWSTR)&stack0xffffffe8;
      lpWideCharStr = (LPCWSTR)&stack0xffffffe8;
      if (&stack0x00000000 != (undefined1 *)0x18) {
LAB_10020b93:
        lpWideCharStr = pWVar4 + 4;
      }
    }
    else {
      pWVar4 = (LPCWSTR)FUN_1000dfa0(uVar1);
      lpWideCharStr = pWVar4;
      if (pWVar4 != (LPCWSTR)0x0) {
        pWVar4[0] = L'\xdddd';
        pWVar4[1] = L'\0';
        goto LAB_10020b93;
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
LAB_10020c50:
  iVar5 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar5;
}


// 10020c62 ___crtGetStringTypeA

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


// 10020ca2 ___free_lc_time

/* Library Function - Single Match
    ___free_lc_time
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lc_time(undefined4 *param_1)

{
  if (param_1 != (undefined4 *)0x0) {
    FUN_1000e140(param_1[1]);
    FUN_1000e140(param_1[2]);
    FUN_1000e140(param_1[3]);
    FUN_1000e140(param_1[4]);
    FUN_1000e140(param_1[5]);
    FUN_1000e140(param_1[6]);
    FUN_1000e140(*param_1);
    FUN_1000e140(param_1[8]);
    FUN_1000e140(param_1[9]);
    FUN_1000e140(param_1[10]);
    FUN_1000e140(param_1[0xb]);
    FUN_1000e140(param_1[0xc]);
    FUN_1000e140(param_1[0xd]);
    FUN_1000e140(param_1[7]);
    FUN_1000e140(param_1[0xe]);
    FUN_1000e140(param_1[0xf]);
    FUN_1000e140(param_1[0x10]);
    FUN_1000e140(param_1[0x11]);
    FUN_1000e140(param_1[0x12]);
    FUN_1000e140(param_1[0x13]);
    FUN_1000e140(param_1[0x14]);
    FUN_1000e140(param_1[0x15]);
    FUN_1000e140(param_1[0x16]);
    FUN_1000e140(param_1[0x17]);
    FUN_1000e140(param_1[0x18]);
    FUN_1000e140(param_1[0x19]);
    FUN_1000e140(param_1[0x1a]);
    FUN_1000e140(param_1[0x1b]);
    FUN_1000e140(param_1[0x1c]);
    FUN_1000e140(param_1[0x1d]);
    FUN_1000e140(param_1[0x1e]);
    FUN_1000e140(param_1[0x1f]);
    FUN_1000e140(param_1[0x20]);
    FUN_1000e140(param_1[0x21]);
    FUN_1000e140(param_1[0x22]);
    FUN_1000e140(param_1[0x23]);
    FUN_1000e140(param_1[0x24]);
    FUN_1000e140(param_1[0x25]);
    FUN_1000e140(param_1[0x26]);
    FUN_1000e140(param_1[0x27]);
    FUN_1000e140(param_1[0x28]);
    FUN_1000e140(param_1[0x29]);
    FUN_1000e140(param_1[0x2a]);
  }
  return;
}


// 10020e32 ___free_lconv_num

/* Library Function - Single Match
    ___free_lconv_num
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_num(int *param_1)

{
  if (param_1 != (int *)0x0) {
    if ((undefined *)*param_1 != PTR_DAT_10036df0) {
      FUN_1000e140((undefined *)*param_1);
    }
    if ((undefined *)param_1[1] != PTR_DAT_10036df4) {
      FUN_1000e140((undefined *)param_1[1]);
    }
    if ((undefined *)param_1[2] != PTR_DAT_10036df8) {
      FUN_1000e140((undefined *)param_1[2]);
    }
  }
  return;
}


// 10020e72 ___free_lconv_mon

/* Library Function - Single Match
    ___free_lconv_mon
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_mon(int param_1)

{
  if (param_1 != 0) {
    if (*(undefined **)(param_1 + 0xc) != PTR_DAT_10036dfc) {
      FUN_1000e140(*(undefined **)(param_1 + 0xc));
    }
    if (*(undefined **)(param_1 + 0x10) != PTR_DAT_10036e00) {
      FUN_1000e140(*(undefined **)(param_1 + 0x10));
    }
    if (*(undefined **)(param_1 + 0x14) != PTR_DAT_10036e04) {
      FUN_1000e140(*(undefined **)(param_1 + 0x14));
    }
    if (*(undefined **)(param_1 + 0x18) != PTR_DAT_10036e08) {
      FUN_1000e140(*(undefined **)(param_1 + 0x18));
    }
    if (*(undefined **)(param_1 + 0x1c) != PTR_DAT_10036e0c) {
      FUN_1000e140(*(undefined **)(param_1 + 0x1c));
    }
    if (*(undefined **)(param_1 + 0x20) != PTR_DAT_10036e10) {
      FUN_1000e140(*(undefined **)(param_1 + 0x20));
    }
    if (*(undefined **)(param_1 + 0x24) != PTR_DAT_10036e14) {
      FUN_1000e140(*(undefined **)(param_1 + 0x24));
    }
  }
  return;
}


// 10020f00 _strcspn

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


// 10020f50 _strpbrk

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


// 10020f90 ___ansicp

/* Library Function - Single Match
    ___ansicp
   
   Library: Visual Studio 2005 Release */

void __cdecl ___ansicp(LCID param_1)

{
  int iVar1;
  CHAR local_10 [6];
  undefined1 local_a;
  uint local_8;
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  local_a = 0;
  iVar1 = GetLocaleInfoA(param_1,0x1004,local_10,6);
  if (iVar1 != 0) {
    _atol(local_10);
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10020fd7 ___convertcp

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  cbMultiByte = *param_4;
  bVar2 = false;
  if (param_1 == param_2) goto LAB_10021177;
  BVar3 = GetCPInfo(param_1,&local_1c);
  if ((((BVar3 == 0) || (local_1c.MaxCharSize != 1)) ||
      (BVar3 = GetCPInfo(param_2,&local_1c), BVar3 == 0)) || (local_1c.MaxCharSize != 1)) {
    uVar7 = MultiByteToWideChar(param_1,1,param_3,cbMultiByte,(LPWSTR)0x0,0);
    bVar8 = uVar7 == 0;
    if (bVar8) goto LAB_10021177;
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
LAB_100210b7:
        local_20 = pWVar5 + 4;
      }
    }
    else {
      pWVar5 = (LPCWSTR)FUN_1000dfa0(uVar1);
      local_20 = pWVar5;
      if (pWVar5 != (LPCWSTR)0x0) {
        pWVar5[0] = L'\xdddd';
        pWVar5[1] = L'\0';
        goto LAB_100210b7;
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
           (lpMultiByteStr = (LPSTR)FUN_1000e0e0(1,uVar7), lpMultiByteStr != (LPSTR)0x0)) {
          uVar7 = WideCharToMultiByte(param_2,0,local_20,uVar7,lpMultiByteStr,uVar7,(LPCSTR)0x0,
                                      (LPBOOL)0x0);
          if (uVar7 == 0) {
            FUN_1000e140(lpMultiByteStr);
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
LAB_10021177:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10021189 ___report_gsfailure

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
  
  _DAT_1003a388 =
       (uint)(in_NT & 1) * 0x4000 | (uint)SBORROW4((int)&stack0xfffffffc,0x328) * 0x800 |
       (uint)(in_IF & 1) * 0x200 | (uint)(in_TF & 1) * 0x100 | (uint)((int)&local_32c < 0) * 0x80 |
       (uint)(&stack0x00000000 == (undefined1 *)0x32c) * 0x40 | (uint)(in_AF & 1) * 0x10 |
       (uint)((POPCOUNT((uint)&local_32c & 0xff) & 1U) == 0) * 4 |
       (uint)(&stack0xfffffffc < (undefined1 *)0x328) | (uint)(in_ID & 1) * 0x200000 |
       (uint)(in_VIP & 1) * 0x100000 | (uint)(in_VIF & 1) * 0x80000 | (uint)(in_AC & 1) * 0x40000;
  _DAT_1003a38c = &stack0x00000004;
  _DAT_1003a2c8 = 0x10001;
  _DAT_1003a270 = 0xc0000409;
  _DAT_1003a274 = 1;
  local_32c = DAT_10036948;
  local_328 = DAT_1003694c;
  _DAT_1003a27c = unaff_retaddr;
  _DAT_1003a354 = in_GS;
  _DAT_1003a358 = in_FS;
  _DAT_1003a35c = in_ES;
  _DAT_1003a360 = in_DS;
  _DAT_1003a364 = unaff_EDI;
  _DAT_1003a368 = unaff_ESI;
  _DAT_1003a36c = unaff_EBX;
  _DAT_1003a370 = in_EDX;
  _DAT_1003a374 = in_ECX;
  _DAT_1003a378 = in_EAX;
  _DAT_1003a37c = unaff_EBP;
  DAT_1003a380 = unaff_retaddr;
  _DAT_1003a384 = in_CS;
  _DAT_1003a390 = in_SS;
  DAT_1003a2c0 = IsDebuggerPresent();
  FUN_10020135();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  UnhandledExceptionFilter((_EXCEPTION_POINTERS *)&PTR_DAT_10029ccc);
  if (DAT_1003a2c0 == 0) {
    FUN_10020135();
  }
  uExitCode = 0xc0000409;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  return;
}


// 1002128d __mbtowc_l

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
          if (iVar2 != 0) goto LAB_100212d8;
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
        pDVar3 = FUN_1001c00a();
        *pDVar3 = 0x2a;
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        return -1;
      }
      if (_DstCh != (wchar_t *)0x0) {
        *_DstCh = (ushort)(byte)*_SrcCh;
      }
LAB_100212d8:
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


// 100213a0 _mbtowc

/* Library Function - Single Match
    _mbtowc
   
   Library: Visual Studio 2005 Release */

int __cdecl _mbtowc(wchar_t *_DstCh,char *_SrcCh,size_t _SrcSizeInBytes)

{
  int iVar1;
  
  iVar1 = __mbtowc_l(_DstCh,_SrcCh,_SrcSizeInBytes,(_locale_t)0x0);
  return iVar1;
}


// 100213f4 __ld12tod

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
        goto LAB_100218f3;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10036e38 - 1;
    iVar4 = (int)(DAT_10036e38 + ((int)DAT_10036e38 >> 0x1f & 0x1fU)) >> 5;
    uVar8 = DAT_10036e38 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar6) goto LAB_10021527;
          bVar14 = _Ifp < p_Var9;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_1002152e;
LAB_10021527:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_1002152e:
              iVar4 = iVar4 + -1;
              *puVar6 = (uint)_Ifp;
              if ((iVar4 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_1002153c;
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
LAB_1002153c:
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
    if (iVar12 < (int)(DAT_10036e34 - DAT_10036e38)) {
      local_24[0] = 0;
      local_24[1] = 0;
    }
    else {
      if (DAT_10036e34 < iVar12) {
        if (iVar12 < DAT_10036e30) {
          local_24[0] = local_24[0] & 0x7fffffff;
          iVar12 = iVar12 + DAT_10036e44;
          iVar2 = (int)(DAT_10036e3c + ((int)DAT_10036e3c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_10036e3c & 0x8000001f;
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
          iVar12 = (int)(DAT_10036e3c + ((int)DAT_10036e3c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_10036e3c & 0x8000001f;
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
          iVar12 = DAT_10036e44 + DAT_10036e30;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_100218f3;
      }
      local_14 = DAT_10036e34 - local_14;
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
      iVar2 = DAT_10036e38 - 1;
      iVar12 = (int)(DAT_10036e38 + ((int)DAT_10036e38 >> 0x1f & 0x1fU)) >> 5;
      uVar10 = DAT_10036e38 & 0x8000001f;
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
          if (2 < iVar12) goto LAB_100216df;
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
LAB_100216df:
      *puVar7 = *puVar7 & -1 << ((byte)local_14 & 0x1f);
      iVar12 = local_10 + 1;
      if (iVar12 < 3) {
        puVar7 = local_24 + iVar12;
        for (iVar2 = 3 - iVar12; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar7 = 0;
          puVar7 = puVar7 + 1;
        }
      }
      uVar10 = DAT_10036e3c + 1;
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
LAB_100218f3:
  local_24[0] = iVar12 << (0x1fU - (char)DAT_10036e3c & 0x1f) |
                -(uint)(local_24[3] != 0) & 0x80000000 | local_24[0];
  if (DAT_10036e40 == 0x40) {
    *(uint *)((int)&_D->x + 4) = local_24[0];
    *(uint *)&_D->x = local_24[1];
  }
  else if (DAT_10036e40 == 0x20) {
    *(uint *)&_D->x = local_24[0];
  }
  return IVar3;
}


// 10021936 __ld12tof

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
        goto LAB_10021e35;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10036e50 - 1;
    iVar2 = (int)(DAT_10036e50 + ((int)DAT_10036e50 >> 0x1f & 0x1fU)) >> 5;
    uVar7 = DAT_10036e50 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar5) goto LAB_10021a69;
          bVar14 = _Ifp < p_Var8;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_10021a70;
LAB_10021a69:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_10021a70:
              iVar2 = iVar2 + -1;
              *puVar5 = (uint)_Ifp;
              if ((iVar2 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_10021a7e;
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
LAB_10021a7e:
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
    if (iVar11 < (int)(DAT_10036e4c - DAT_10036e50)) {
      local_24 = 0;
      local_20 = 0.0;
    }
    else {
      if (DAT_10036e4c < iVar11) {
        if (iVar11 < DAT_10036e48) {
          local_24 = local_24 & 0x7fffffff;
          iVar11 = iVar11 + DAT_10036e5c;
          iVar2 = (int)(DAT_10036e54 + ((int)DAT_10036e54 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10036e54 & 0x8000001f;
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
          iVar11 = (int)(DAT_10036e54 + ((int)DAT_10036e54 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10036e54 & 0x8000001f;
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
          iVar11 = DAT_10036e5c + DAT_10036e48;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_10021e35;
      }
      local_1c[2] = DAT_10036e4c - local_1c[2];
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
      iVar2 = DAT_10036e50 - 1;
      iVar11 = (int)(DAT_10036e50 + ((int)DAT_10036e50 >> 0x1f & 0x1fU)) >> 5;
      uVar9 = DAT_10036e50 & 0x8000001f;
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
          if (2 < iVar11) goto LAB_10021c21;
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
LAB_10021c21:
      *puVar6 = *puVar6 & -1 << ((byte)local_1c[2] & 0x1f);
      iVar11 = local_1c[3] + 1;
      if (iVar11 < 3) {
        puVar6 = &local_24 + iVar11;
        for (iVar2 = 3 - iVar11; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar6 = 0;
          puVar6 = puVar6 + 1;
        }
      }
      uVar9 = DAT_10036e54 + 1;
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
LAB_10021e35:
  fVar12 = (float)(iVar11 << (0x1fU - (char)DAT_10036e54 & 0x1f) |
                   -(uint)(local_1c[1] != 0) & 0x80000000 | local_24);
  if (DAT_10036e58 == 0x40) {
    _F[1].f = fVar12;
    _F->f = local_20;
  }
  else if (DAT_10036e58 == 0x20) {
    _F->f = fVar12;
  }
  return IVar3;
}


// 10021e78 ___mtold12

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
  
  uVar7 = DAT_10036948 ^ (uint)&stack0xfffffffc;
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


// 10022044 FUN_10022044

/* WARNING: Removing unreachable block (ram,0x10022535) */
/* WARNING: Removing unreachable block (ram,0x1002253f) */
/* WARNING: Removing unreachable block (ram,0x10022544) */

void __cdecl
FUN_10022044(int param_1,uint param_2,ushort param_3,int param_4,byte param_5,short *param_6)

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
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
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
    goto LAB_100228c2;
  }
  if ((short)uVar14 == 0x7fff) {
    *param_6 = 1;
    if (((param_2 == 0x80000000) && (param_1 == 0)) || ((param_2 & 0x40000000) != 0)) {
      if ((uVar8 == 0) || (param_2 != 0xc0000000)) {
        if ((param_2 != 0x80000000) || (param_1 != 0)) goto LAB_10022173;
        pcVar25 = "1#INF";
      }
      else {
        if (param_1 != 0) {
LAB_10022173:
          pcVar25 = "1#QNAN";
          goto LAB_10022178;
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
LAB_10022178:
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
    goto LAB_100228c2;
  }
  local_50 = (short)(((uVar14 >> 8) + (param_2 >> 0x18) * 2) * 0x4d + -0x134312f4 + uVar14 * 0x4d10
                    >> 0x10);
  uVar16 = (uint)local_50;
  local_1a = (undefined1)uVar14;
  bStack_19 = (byte)(uVar14 >> 8);
  local_24._2_2_ = (ushort)param_1;
  local_24._0_2_ = 0;
  local_6c = &PTR_DAT_10036e00;
  uStack_20._0_2_ = uStack_12;
  uStack_20._2_2_ = (ushort)local_10;
  uStack_1c = local_10._2_2_;
  if (uVar16 != 0) {
    iVar20 = param_1;
    uVar14 = -uVar16;
    if (0 < (int)uVar16) {
      local_6c = (undefined **)&DAT_10036f60;
      uVar14 = uVar16;
    }
    while (uVar14 != 0) {
      uStack_20._0_2_ = (ushort)((uint)iVar20 >> 0x10);
      local_24._2_2_ = (ushort)iVar20;
      local_6c = local_6c + 0x15;
      iVar3 = CONCAT22(local_c,local_10._2_2_);
      if ((uVar14 & 7) == 0) goto LAB_10022499;
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
LAB_100223bd:
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
            if ((short)uVar24 < 1) goto LAB_100223bd;
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
          if (0x7ffe < uVar24) goto LAB_1002247d;
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
LAB_1002247d:
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
LAB_10022499:
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
          goto LAB_10022701;
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
LAB_1002260f:
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
          if ((short)uVar24 < 1) goto LAB_1002260f;
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
        goto LAB_10022701;
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
LAB_10022701:
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
        goto LAB_100228c2;
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
LAB_100228c2:
  local_24 = uVar16;
  local_10 = param_2;
  uStack_3e = iVar1;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1002290a __hw_cw

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


// 10022998 ___hw_cw_sse2

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


// 10022a38 __control87

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
  if (DAT_1003b7a0 != 0) {
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


// 10022d3c __global_unwind2

/* Library Function - Single Match
    __global_unwind2
   
   Library: Visual Studio */

void __cdecl __global_unwind2(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x10022d54,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 10022da1 __local_unwind2

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
  puStack_1c = &LAB_10022d5c;
  local_20 = *unaff_FS_OFFSET;
  *unaff_FS_OFFSET = &local_20;
  while( true ) {
    uVar1 = *(uint *)(param_1 + 0xc);
    if ((uVar1 == 0xffffffff) || ((param_2 != 0xffffffff && (uVar1 <= param_2)))) break;
    local_18 = *(undefined4 *)(*(int *)(param_1 + 8) + uVar1 * 0xc);
    *(undefined4 *)(param_1 + 0xc) = local_18;
    if (*(int *)(*(int *)(param_1 + 8) + 4 + uVar1 * 0xc) == 0) {
      __NLG_Notify(0x101);
      FUN_10022e70();
    }
  }
  *unaff_FS_OFFSET = local_20;
  return;
}


// 10022e51 __NLG_Notify

/* Library Function - Single Match
    __NLG_Notify
   
   Libraries: Visual Studio 2017 Debug, Visual Studio 2017 Release, Visual Studio 2019 Debug, Visual
   Studio 2019 Release */

void __NLG_Notify(ulong param_1)

{
  undefined4 in_EAX;
  undefined4 unaff_EBP;
  
  DAT_10037124 = param_1;
  DAT_10037120 = in_EAX;
  DAT_10037128 = unaff_EBP;
  return;
}


// 10022e70 FUN_10022e70

void FUN_10022e70(void)

{
  code *in_EAX;
  
  (*in_EAX)();
  return;
}


// 10022e73 FUN_10022e73

undefined4 __cdecl FUN_10022e73(uint param_1)

{
  int *piVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD nStdHandle;
  
  if ((-1 < (int)param_1) && (param_1 < DAT_1003b664)) {
    iVar3 = (param_1 & 0x1f) * 0x38;
    piVar1 = (int *)((&DAT_1003b680)[(int)param_1 >> 5] + iVar3);
    if (((*(byte *)(piVar1 + 1) & 1) != 0) && (*piVar1 != -1)) {
      if (DAT_10039bb0 == 1) {
        if (param_1 == 0) {
          nStdHandle = 0xfffffff6;
        }
        else if (param_1 == 1) {
          nStdHandle = 0xfffffff5;
        }
        else {
          if (param_1 != 2) goto LAB_10022ed1;
          nStdHandle = 0xfffffff4;
        }
        SetStdHandle(nStdHandle,(HANDLE)0x0);
      }
LAB_10022ed1:
      *(undefined4 *)(iVar3 + (&DAT_1003b680)[(int)param_1 >> 5]) = 0xffffffff;
      return 0;
    }
  }
  pDVar2 = FUN_1001c00a();
  *pDVar2 = 9;
  pDVar2 = FUN_1001c01d();
  *pDVar2 = 0;
  return 0xffffffff;
}


// 10022ef4 __get_osfhandle

/* Library Function - Single Match
    __get_osfhandle
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __get_osfhandle(int _FileHandle)

{
  DWORD *pDVar1;
  intptr_t *piVar2;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_1001c01d();
    *pDVar1 = 0;
    pDVar1 = FUN_1001c00a();
    *pDVar1 = 9;
    return -1;
  }
  if (((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1003b664)) &&
     (piVar2 = (intptr_t *)((_FileHandle & 0x1fU) * 0x38 + (&DAT_1003b680)[_FileHandle >> 5]),
     (*(byte *)(piVar2 + 1) & 1) != 0)) {
    return *piVar2;
  }
  pDVar1 = FUN_1001c01d();
  *pDVar1 = 0;
  pDVar1 = FUN_1001c00a();
  *pDVar1 = 9;
  FUN_1001bfab((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 10022f65 ___lock_fhandle

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
  
  iVar2 = (_Filehandle & 0x1fU) * 0x38 + (&DAT_1003b680)[_Filehandle >> 5];
  local_20 = 1;
  if (*(int *)(iVar2 + 8) == 0) {
    __lock(10);
    if (*(int *)(iVar2 + 8) == 0) {
      iVar1 = ___crtInitCritSecAndSpinCount(iVar2 + 0xc,4000);
      local_20 = (uint)(iVar1 != 0);
      *(int *)(iVar2 + 8) = *(int *)(iVar2 + 8) + 1;
    }
    FUN_10022ffc();
  }
  if (local_20 != 0) {
    EnterCriticalSection
              ((LPCRITICAL_SECTION)
               ((&DAT_1003b680)[_Filehandle >> 5] + 0xc + (_Filehandle & 0x1fU) * 0x38));
  }
  return local_20;
}


// 10022ffc FUN_10022ffc

void FUN_10022ffc(void)

{
  FUN_1001cdb5(10);
  return;
}


// 10023005 FUN_10023005

void __cdecl FUN_10023005(uint param_1)

{
  LeaveCriticalSection
            ((LPCRITICAL_SECTION)
             ((&DAT_1003b680)[(int)param_1 >> 5] + 0xc + (param_1 & 0x1f) * 0x38));
  return;
}


// 10023027 __putwch_nolock

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
  
  local_8 = DAT_10036948 ^ (uint)&stack0xfffffffc;
  if (DAT_1003712c != 0) {
    if (DAT_100371e4 == (HANDLE)0xfffffffe) {
      ___initconout();
    }
    if (DAT_100371e4 == (HANDLE)0xffffffff) goto LAB_100230d0;
    BVar2 = WriteConsoleW(DAT_100371e4,&_WCh,1,&local_14,(LPVOID)0x0);
    if (BVar2 != 0) {
      DAT_1003712c = 1;
      goto LAB_100230d0;
    }
    if ((DAT_1003712c != 2) || (DVar3 = GetLastError(), DVar3 != 0x78)) goto LAB_100230d0;
    DAT_1003712c = 0;
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
  if (DAT_100371e4 != (HANDLE)0xffffffff) {
    WriteConsoleA(DAT_100371e4,local_10,DVar3,&local_14,(LPVOID)0x0);
  }
LAB_100230d0:
  wVar1 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return wVar1;
}


// 1002330f ___initconout

/* Library Function - Single Match
    ___initconout
   
   Library: Visual Studio 2005 Release */

void __cdecl ___initconout(void)

{
  DAT_100371e4 = CreateFileA("CONOUT$",0x40000000,3,(LPSECURITY_ATTRIBUTES)0x0,3,0,(HANDLE)0x0);
  return;
}


// 10023530 ___ascii_strnicmp

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
      if (bVar2 != (byte)uVar3) goto LAB_10023581;
      _MaxCount = _MaxCount - 1;
    } while (_MaxCount != 0);
    _MaxCount = 0;
    bVar2 = (byte)(uVar3 >> 8);
    bVar5 = bVar2 < (byte)uVar3;
    if (bVar2 != (byte)uVar3) {
LAB_10023581:
      _MaxCount = 0xffffffff;
      if (!bVar5) {
        _MaxCount = 1;
      }
    }
  }
  return _MaxCount;
}


// 10023591 FUN_10023591

undefined4 __cdecl FUN_10023591(uint param_1)

{
  intptr_t iVar1;
  intptr_t iVar2;
  HANDLE hObject;
  BOOL BVar3;
  DWORD DVar4;
  undefined4 uVar5;
  
  iVar1 = __get_osfhandle(param_1);
  if (iVar1 != -1) {
    if (((param_1 == 1) && ((*(byte *)(DAT_1003b680 + 0x74) & 1) != 0)) ||
       ((param_1 == 2 && ((*(byte *)(DAT_1003b680 + 0x3c) & 1) != 0)))) {
      iVar1 = __get_osfhandle(2);
      iVar2 = __get_osfhandle(1);
      if (iVar2 == iVar1) goto LAB_100235f0;
    }
    hObject = (HANDLE)__get_osfhandle(param_1);
    BVar3 = CloseHandle(hObject);
    if (BVar3 == 0) {
      DVar4 = GetLastError();
      goto LAB_100235f2;
    }
  }
LAB_100235f0:
  DVar4 = 0;
LAB_100235f2:
  FUN_10022e73(param_1);
  *(undefined1 *)((&DAT_1003b680)[(int)param_1 >> 5] + 4 + (param_1 & 0x1f) * 0x38) = 0;
  if (DVar4 == 0) {
    uVar5 = 0;
  }
  else {
    FUN_1001c030(DVar4);
    uVar5 = 0xffffffff;
  }
  return uVar5;
}


// 10023730 FUN_10023730

void FUN_10023730(void)

{
  FUN_10004230((undefined4 *)&DAT_10037388);
  _atexit((_func_4879 *)&LAB_100242c0);
  return;
}


