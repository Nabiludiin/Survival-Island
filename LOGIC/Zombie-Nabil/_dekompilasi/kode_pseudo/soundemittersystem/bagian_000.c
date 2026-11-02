// 10001000 FUN_10001000

undefined8 __cdecl FUN_10001000(char *param_1)

{
  char *pcVar1;
  double dVar2;
  float local_88;
  float local_84;
  char local_80 [128];
  
  local_88 = 0.0;
  local_84 = 0.0;
  FUN_10008ee0(local_80,param_1,0x80);
  pcVar1 = _strtok(local_80,",");
  if (pcVar1 != (char *)0x0) {
    dVar2 = _atof(pcVar1);
    local_88 = (float)dVar2;
    pcVar1 = _strtok((char *)0x0,",");
    if (pcVar1 != (char *)0x0) {
      dVar2 = _atof(pcVar1);
      local_84 = (float)dVar2 - local_88;
    }
  }
  return CONCAT44(local_84,local_88);
}


// 10001080 FUN_10001080

char * FUN_10001080(void)

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
    VirtualQuery(&DAT_10020e8c,&_Stack_1c,0x1c);
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


// 10001110 FUN_10001110

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10001110(undefined4 param_1)

{
  if ((_DAT_10020e94 & 1) == 0) {
    _DAT_10020e94 = _DAT_10020e94 | 1;
    DAT_10020e90 = FUN_10001080();
  }
  if (DAT_10020e90 != (char *)0x0) {
    (**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_10020e90,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  return;
}


// 10001170 FUN_10001170

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_10001170(size_t param_1)

{
  void *pvVar1;
  
  if ((_DAT_10020e94 & 1) == 0) {
    _DAT_10020e94 = _DAT_10020e94 | 1;
    DAT_10020e90 = FUN_10001080();
  }
  if (DAT_10020e90 != (char *)0x0) {
    pvVar1 = (void *)(**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_10020e90,0);
    _memset(pvVar1,0,param_1);
    return pvVar1;
  }
  pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  _memset(pvVar1,0,param_1);
  return pvVar1;
}


// 100011f0 FUN_100011f0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100011f0(undefined4 param_1,undefined4 param_2)

{
  if ((_DAT_10020e9c & 1) == 0) {
    _DAT_10020e9c = _DAT_10020e9c | 1;
    DAT_10020e98 = FUN_10001080();
  }
  if (DAT_10020e98 != (char *)0x0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 8))(param_1,param_2,DAT_10020e98,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(param_1,param_2);
  return;
}


// 10001250 FUN_10001250

void __cdecl FUN_10001250(int param_1,int param_2)

{
  FUN_10001170(param_1 * param_2);
  return;
}


// 10001280 FUN_10001280

void __cdecl FUN_10001280(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x20))(param_1);
  return;
}


// 100012a0 FUN_100012a0

bool FUN_100012a0(void)

{
  return *(int *)g_pMemAlloc_exref != 0;
}


// 100012b0 FUN_100012b0

void FUN_100012b0(void)

{
  return;
}


// 100012c0 FUN_100012c0

void __cdecl FUN_100012c0(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1);
  return;
}


// 100012e0 FUN_100012e0

int __cdecl FUN_100012e0(char *param_1)

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


// 10001370 FUN_10001370

float10 __cdecl FUN_10001370(uint param_1)

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


// 10001440 FUN_10001440

void __thiscall FUN_10001440(void *this,float param_1,float param_2)

{
  undefined2 local_4;
  
  local_4 = (undefined2)(int)ROUND(param_1);
  *(undefined2 *)((int)this + 0x10) = local_4;
  param_2._0_2_ = (undefined2)(int)ROUND(param_2);
  *(undefined2 *)((int)this + 0x12) = param_2._0_2_;
  return;
}


// 100014a0 FUN_100014a0

int __fastcall FUN_100014a0(int param_1)

{
  int iVar1;
  
  iVar1 = param_1 + 4;
  if (*(short *)(param_1 + 10) != 1) {
    iVar1 = *(int *)(param_1 + 4);
  }
  return iVar1;
}


// 100014b0 FUN_100014b0

undefined4 __thiscall FUN_100014b0(void *this,undefined4 *param_1)

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


// 10001510 FUN_10001510

undefined4 * __fastcall FUN_10001510(undefined4 *param_1)

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


// 10001590 FUN_10001590

void __cdecl FUN_10001590(char *param_1)

{
  char cVar1;
  
  for (; ((((cVar1 = *param_1, cVar1 == '*' || (cVar1 == '?')) || (cVar1 == '!')) ||
          ((((cVar1 == '#' || (cVar1 == '@')) ||
            ((cVar1 == '(' || ((cVar1 == '>' || (cVar1 == '<')))))) || (cVar1 == '^')))) ||
         (((cVar1 == ')' || (cVar1 == '}')) || (cVar1 == '$')))); param_1 = param_1 + 1) {
  }
  return;
}


// 10001610 FUN_10001610

void __thiscall
FUN_10001610(void *this,int param_1,int param_2,int param_3,char *param_4,int param_5)

{
  char *in_EAX;
  int iVar1;
  int iVar2;
  int iVar3;
  
  iVar1 = 0;
  iVar3 = 0;
                    /* WARNING: Load size is inaccurate */
  if (*this != '\0') {
    do {
      if (iVar1 == param_1) {
        iVar2 = param_2 + -1;
        if (-1 < iVar2) {
          iVar2 = iVar2 - param_2;
          this = (void *)((int)this + param_2);
        }
        *in_EAX = '\0';
        in_EAX = param_4;
        param_2 = iVar2;
        param_3 = param_5;
      }
      else if (iVar3 < param_3) {
                    /* WARNING: Load size is inaccurate */
        *in_EAX = *this;
        in_EAX = in_EAX + 1;
        this = (void *)((int)this + 1);
        iVar3 = iVar3 + 1;
      }
      else {
        this = (void *)((int)this + 1);
      }
      iVar1 = iVar1 + 1;
                    /* WARNING: Load size is inaccurate */
    } while (*this != '\0');
    *in_EAX = '\0';
    return;
  }
  *in_EAX = '\0';
  return;
}


// 10001bc0 FUN_10001bc0

void __thiscall FUN_10001bc0(void *this,undefined4 param_1,undefined4 param_2,undefined4 param_3)

{
  undefined4 uVar1;
  
                    /* WARNING: Load size is inaccurate */
  uVar1 = (**(code **)(*this + 0x88))(param_1);
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0x8c))(uVar1,param_1,param_2,param_3);
  return;
}


// 10001c00 FUN_10001c00

void __thiscall FUN_10001c00(void *this,undefined4 param_1,undefined4 param_2)

{
  undefined4 uVar1;
  undefined4 unaff_retaddr;
  
                    /* WARNING: Load size is inaccurate */
  uVar1 = (**(code **)(*this + 0x88))(param_2);
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0x3c))(unaff_retaddr,uVar1);
  return;
}


// 10001e90 FUN_10001e90

float10 FUN_10001e90(byte *param_1)

{
  int iVar1;
  
  if (param_1 != (byte *)0x0) {
    iVar1 = FUN_10008c70(param_1,(byte *)"ATTN_NONE");
    if (iVar1 == 0) {
      return (float10)0;
    }
    iVar1 = FUN_10008c70(param_1,(byte *)"ATTN_NORM");
    if (iVar1 != 0) {
      iVar1 = FUN_10008c70(param_1,(byte *)0x1001a344);
      if (iVar1 == 0) {
        return (float10)2.0;
      }
      iVar1 = FUN_10008c70(param_1,(byte *)0x1001a334);
      if (iVar1 == 0) {
        return (float10)1.25;
      }
      iVar1 = FUN_10008c70(param_1,(byte *)0x1001a320);
      if (iVar1 == 0) {
        return (float10)1.5;
      }
      iVar1 = FUN_10008c70(param_1,(byte *)0x1001a30c);
      if (iVar1 == 0) {
        return (float10)0.27;
      }
      DevMsg("CSoundEmitterSystem:  Unknown attenuation key %s\n",param_1);
    }
  }
  return (float10)0.8;
}


// 10001f90 FUN_10001f90

uint __thiscall FUN_10001f90(void *this,undefined4 param_1)

{
  uint uVar1;
  
                    /* WARNING: Load size is inaccurate */
  uVar1 = (**(code **)(*this + 0x1c))(param_1);
  if (-1 < (int)uVar1) {
                    /* WARNING: Load size is inaccurate */
    uVar1 = (**(code **)(*this + 0x58))(uVar1);
    if (uVar1 != 0) {
      return CONCAT31((int3)(uVar1 >> 8),*(byte *)(uVar1 + 0x1a) >> 2) & 0xffffff01;
    }
  }
  return uVar1 & 0xffffff00;
}


// 10001fe0 FUN_10001fe0

uint * FUN_10001fe0(uint *param_1)

{
  uint *puVar1;
  uint uVar2;
  
  puVar1 = param_1;
  uVar2 = FUN_10008a80((char *)param_1);
  FUN_100090e0(&param_1);
  FUN_10009100((uint *)&param_1,puVar1,uVar2);
  FUN_100090f0((uint *)&param_1);
  return param_1;
}


// 10002030 FUN_10002030

void __fastcall FUN_10002030(ushort *param_1)

{
  ushort uVar1;
  float10 fVar2;
  float10 fVar3;
  float10 fVar4;
  
  uVar1 = *param_1;
  fVar2 = FUN_10001370((uint)uVar1);
  fVar3 = FUN_10001370((uint)param_1[1]);
  fVar4 = FUN_10001370((uint)uVar1);
  RandomFloat((float)fVar4,(float)fVar3 + (float)fVar2);
  return;
}


// 10002080 FUN_10002080

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __fastcall FUN_10002080(undefined4 *param_1)

{
  DWORD DVar1;
  DWORD DVar2;
  DWORD DVar3;
  undefined4 *puVar4;
  int iVar5;
  
  if (param_1[2] != 0) goto LAB_1000213c;
  if ((_DAT_100210a8 & 1) == 0) {
    _DAT_100210a8 = _DAT_100210a8 | 1;
    DAT_100210a0 = 0;
    DAT_100210a4 = 0;
  }
  DVar2 = GetCurrentThreadId();
  if (DVar2 == DAT_100210a0) {
LAB_100020d4:
    DAT_100210a4 = DAT_100210a4 + 1;
  }
  else {
    DVar3 = 0;
    LOCK();
    DVar1 = DVar2;
    if (DAT_100210a0 != 0) {
      DVar3 = DAT_100210a0;
      DVar1 = DAT_100210a0;
    }
    DAT_100210a0 = DVar1;
    UNLOCK();
    if (DVar3 == 0) goto LAB_100020d4;
    CThreadFastMutex::Lock((CThreadFastMutex *)&DAT_100210a0,DVar2,0);
  }
  if (param_1[2] == 0) {
    puVar4 = (undefined4 *)FUN_10001110(0x3bce0);
    param_1[2] = puVar4;
    param_1[3] = puVar4 + 0xef38;
    iVar5 = 0x17ec;
    do {
      FUN_100014b0(param_1,puVar4);
      puVar4 = puVar4 + 10;
      iVar5 = iVar5 + -1;
    } while (iVar5 != 0);
  }
  DAT_100210a4 = DAT_100210a4 + -1;
  if (DAT_100210a4 == 0) {
    LOCK();
    DAT_100210a0 = 0;
    UNLOCK();
  }
LAB_1000213c:
  puVar4 = FUN_10001510(param_1);
  if (puVar4 == (undefined4 *)0x0) {
    FUN_10001110(0x28);
  }
  return;
}


// 10002180 FUN_10002180

void __thiscall FUN_10002180(void *this,int param_1)

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


// 10002270 FUN_10002270

void FUN_10002270(undefined4 param_1,undefined4 param_2)

{
  RandomInt(param_1,param_2);
  return;
}


// 100023f0 FUN_100023f0

undefined4 __thiscall FUN_100023f0(void *this,undefined4 param_1,int param_2,void *param_3)

{
  ushort uVar1;
  undefined4 *this_00;
  byte *pbVar2;
  int iVar3;
  int iVar4;
  void *this_01;
  undefined4 extraout_ECX;
  undefined4 extraout_ECX_00;
  undefined4 uVar5;
  undefined4 extraout_ECX_01;
  undefined4 extraout_ECX_02;
  undefined4 extraout_ECX_03;
  undefined4 extraout_EDX;
  undefined4 extraout_EDX_00;
  undefined4 uVar6;
  float10 fVar7;
  ulonglong uVar8;
  undefined8 uVar9;
  ulonglong uVar10;
  char *pcVar11;
  char *pcVar12;
  byte *pbVar13;
  float fVar14;
  
  this_00 = (undefined4 *)FUN_1000aab0(param_2);
  if (this_00 != (undefined4 *)0x0) {
    do {
      pcVar12 = "channel";
      pbVar2 = FUN_1000a8a0(this_00);
      iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
      if (iVar3 == 0) {
        pbVar2 = (byte *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
        FUN_10008520(param_3,pbVar2);
      }
      else {
        pcVar12 = "volume";
        pbVar2 = FUN_1000a8a0(this_00);
        iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
        if (iVar3 == 0) {
          pbVar2 = (byte *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
          FUN_10008650(param_3,pbVar2);
        }
        else {
          pcVar12 = "pitch";
          pbVar2 = FUN_1000a8a0(this_00);
          iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
          if (iVar3 == 0) {
            pbVar2 = (byte *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
            FUN_100086c0(param_3,pbVar2);
          }
          else {
            pbVar13 = &DAT_1001a654;
            pbVar2 = FUN_1000a8a0(this_00);
            iVar3 = FUN_10008c70(pbVar2,pbVar13);
            if (iVar3 == 0) {
                    /* WARNING: Load size is inaccurate */
              iVar3 = *this;
              iVar4 = FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
              (**(code **)(iVar3 + 0x84))(param_3,iVar4);
            }
            else {
              pcVar12 = "rndwave";
              pbVar2 = FUN_1000a8a0(this_00);
              iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
              if (iVar3 == 0) {
                for (this_01 = (void *)FUN_1000aab0((int)this_00); this_01 != (void *)0x0;
                    this_01 = (void *)FUN_1000aac0((int)this_01)) {
                    /* WARNING: Load size is inaccurate */
                  iVar3 = *this;
                  iVar4 = FUN_1000b210(this_01,(char *)0x0,0x1001a66b);
                  (**(code **)(iVar3 + 0x84))(param_3,iVar4);
                }
              }
              else {
                pcVar12 = "attenuation";
                pbVar2 = FUN_1000a8a0(this_00);
                iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
                if (iVar3 == 0) {
LAB_10002728:
                  if ((*(short *)((int)param_3 + 0x10) != 0x4b) ||
                     (*(short *)((int)param_3 + 0x12) != 0)) {
                    DevMsg("CSoundEmitterSystemBase::GetParametersForSound:  sound %s has multiple attenuation, CompatabilityAttenuation, and/or soundlevel entries.\n"
                           ,param_1);
                  }
                  iVar3 = 7;
                  pcVar11 = "SNDLVL_";
                  pcVar12 = (char *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
                  iVar3 = FUN_10008bf0(pcVar12,pcVar11,iVar3);
                  if (iVar3 == 0) {
                    iVar3 = FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
                    DevMsg("CSoundEmitterSystemBase::GetParametersForSound:  sound %s has \"attenuation\" with %s value!\n"
                           ,param_1,iVar3);
                  }
                  iVar3 = 5;
                  pcVar11 = "ATTN_";
                  pcVar12 = (char *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
                  iVar3 = FUN_10008bf0(pcVar12,pcVar11,iVar3);
                  if (iVar3 == 0) {
                    pbVar2 = (byte *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
                    fVar7 = FUN_10001e90(pbVar2);
                    uVar5 = extraout_ECX;
                    uVar6 = extraout_EDX;
                    if ((float10)0 != fVar7) {
                      pbVar2 = (byte *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
                      FUN_10001e90(pbVar2);
                      uVar5 = extraout_ECX_00;
                      uVar6 = extraout_EDX_00;
                    }
                    fVar14 = 0.0;
                    uVar8 = FUN_1000e380(uVar5,uVar6);
                    param_2 = (int)uVar8;
                  }
                  else {
                    pcVar12 = (char *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
                    uVar9 = FUN_10001000(pcVar12);
                    uVar8 = FUN_1000e380(extraout_ECX_01,(int)((ulonglong)uVar9 >> 0x20));
                    uVar10 = FUN_1000e380(extraout_ECX_02,(int)(uVar8 >> 0x20));
                    fVar14 = (float)((int)uVar10 - (int)uVar8);
                    uVar8 = FUN_1000e380(extraout_ECX_03,(int)(uVar10 >> 0x20));
                    param_2 = (int)uVar8;
                  }
                  FUN_10001440(param_3,(float)param_2,fVar14);
                  pcVar12 = "CompatibilityAttenuation";
                  pbVar2 = FUN_1000a8a0(this_00);
                  iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
                  if (iVar3 != 0) goto LAB_10002926;
                  if (*(short *)((int)param_3 + 0x12) != 0) {
                    Warning("CompatibilityAttenuation for sound %s must have same start and end values.\n"
                            ,param_1);
                  }
                  uVar1 = *(ushort *)((int)param_3 + 0x10);
                }
                else {
                  pcVar12 = "CompatibilityAttenuation";
                  pbVar2 = FUN_1000a8a0(this_00);
                  iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
                  if (iVar3 == 0) goto LAB_10002728;
                  pcVar12 = "soundlevel";
                  pbVar2 = FUN_1000a8a0(this_00);
                  iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
                  if (iVar3 != 0) {
                    pcVar12 = "CompatibilitySoundlevel";
                    pbVar2 = FUN_1000a8a0(this_00);
                    iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
                    if (iVar3 != 0) {
                      pcVar12 = "play_to_owner_only";
                      pbVar2 = FUN_1000a8a0(this_00);
                      iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
                      if (iVar3 == 0) {
                        iVar3 = FUN_1000ae00(this_00,(char *)0x0,0);
                        *(byte *)((int)param_3 + 0x1a) =
                             *(byte *)((int)param_3 + 0x1a) ^
                             (*(byte *)((int)param_3 + 0x1a) ^ iVar3 != 0) & 1;
                      }
                      else {
                        pcVar12 = "delay_msec";
                        pbVar2 = FUN_1000a8a0(this_00);
                        iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
                        if (iVar3 == 0) {
                          iVar3 = FUN_1000ae00(this_00,(char *)0x0,0);
                          if (iVar3 < 0) {
                            *(undefined2 *)((int)param_3 + 0x18) = 0;
                          }
                          else {
                            iVar3 = FUN_1000ae00(this_00,(char *)0x0,0);
                            *(short *)((int)param_3 + 0x18) = (short)iVar3;
                          }
                        }
                        else {
                          pcVar12 = "GameData";
                          pbVar2 = FUN_1000a8a0(this_00);
                          iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
                          if (iVar3 == 0) {
                            FUN_10008030(param_3,this_00);
                          }
                        }
                      }
                      goto LAB_10002926;
                    }
                  }
                  if ((*(short *)((int)param_3 + 0x10) != 0x4b) ||
                     (*(short *)((int)param_3 + 0x12) != 0)) {
                    DevMsg("CSoundEmitterSystemBase::GetParametersForSound:  sound %s has multiple attenuation, CompatabilityAttenuation, and/or soundlevel entries.\n"
                           ,param_1);
                  }
                  iVar3 = 5;
                  pcVar11 = "ATTN_";
                  pcVar12 = (char *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
                  iVar3 = FUN_10008bf0(pcVar12,pcVar11,iVar3);
                  if (iVar3 == 0) {
                    iVar3 = FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
                    DevMsg("CSoundEmitterSystemBase::GetParametersForSound:  sound %s has \"soundlevel\" with %s value!\n"
                           ,param_1,iVar3);
                  }
                  pbVar2 = (byte *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
                  FUN_10008760(param_3,pbVar2);
                  pcVar12 = "CompatibilitySoundlevel";
                  pbVar2 = FUN_1000a8a0(this_00);
                  iVar3 = FUN_10008c70(pbVar2,(byte *)pcVar12);
                  if (iVar3 != 0) goto LAB_10002926;
                  if (*(short *)((int)param_3 + 0x12) != 0) {
                    Warning("CompatibilitySoundlevel for sound %s must have same start and end values.\n"
                            ,param_1);
                  }
                  uVar1 = *(ushort *)((int)param_3 + 0x10);
                }
                param_2 = uVar1 + 0x100;
                FUN_10001440(param_3,(float)param_2,0.0);
              }
            }
          }
        }
      }
LAB_10002926:
      this_00 = (undefined4 *)FUN_1000aac0((int)this_00);
    } while (this_00 != (undefined4 *)0x0);
    this_00 = (undefined4 *)0x0;
  }
  return CONCAT31((int3)((uint)this_00 >> 8),1);
}


// 100029e0 FUN_100029e0

int __thiscall FUN_100029e0(void *this,undefined4 *param_1)

{
  int iVar1;
  code *pcVar2;
  char cVar3;
  int iVar4;
  byte *pbVar5;
  byte *pbVar6;
  uint uVar7;
  int iVar8;
  int local_8;
  
  if (*(char *)((int)this + 0x18) != '\0') {
    _SpewInfo(2,"c:\\buildworker\\l4d_rel_win32\\build\\src\\public\\tier1/UtlSortVector.h",0xf4);
    iVar4 = _SpewMessage(&DAT_1001a674,"Assertion Failed: !m_bNeedsSort");
    if (iVar4 == 0) {
      cVar3 = ShouldUseNewAssertDialog();
      if ((cVar3 == '\0') ||
         (cVar3 = DoNewAssertDialog("c:\\buildworker\\l4d_rel_win32\\build\\src\\public\\tier1/UtlSortVector.h"
                                    ,0xf4,"Assertion Failed: !m_bNeedsSort"), cVar3 != '\0')) {
        pcVar2 = (code *)swi(3);
        iVar4 = (*pcVar2)();
        return iVar4;
      }
      _ExitOnFatalAssert("c:\\buildworker\\l4d_rel_win32\\build\\src\\public\\tier1/UtlSortVector.h"
                         ,0xf4);
    }
  }
  iVar4 = *(int *)((int)this + 0xc) + -1;
  local_8 = 0;
  if (iVar4 < 0) {
    return iVar4;
  }
  do {
                    /* WARNING: Load size is inaccurate */
    iVar1 = *this;
    iVar8 = local_8 + iVar4 >> 1;
    pbVar5 = (byte *)FUN_1000a660((ushort *)*param_1);
    pbVar6 = (byte *)FUN_1000a660(*(ushort **)(iVar1 + iVar8 * 4));
    uVar7 = FUN_10008b10(pbVar6,pbVar5);
    if ((int)uVar7 < 0) {
      local_8 = iVar8 + 1;
    }
    else {
                    /* WARNING: Load size is inaccurate */
      pbVar5 = (byte *)FUN_1000a660(*(ushort **)(*this + iVar8 * 4));
      pbVar6 = (byte *)FUN_1000a660((ushort *)*param_1);
      uVar7 = FUN_10008b10(pbVar6,pbVar5);
      if (-1 < (int)uVar7) {
        return iVar8;
      }
      iVar4 = iVar8 + -1;
    }
  } while (local_8 <= iVar4);
  return iVar4;
}


// 10002b00 FUN_10002b00

void __thiscall FUN_10002b00(void *this,short *param_1,short *param_2)

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


// 10002b40 FUN_10002b40

void __thiscall FUN_10002b40(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_10002b9b;
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
LAB_10002b9b:
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


// 10002bd0 FUN_10002bd0

ushort __fastcall FUN_10002bd0(int param_1)

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
    FUN_10002180((void *)(param_1 + 4),1);
    uVar2 = *puVar1;
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)uVar2)
       ) {
      iVar4 = *(int *)(param_1 + 8);
      puVar3 = &local_2;
    }
    else {
      puVar3 = (ushort *)FUN_10002b00((void *)(param_1 + 4),(short *)&local_2,(short *)puVar1);
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


// 10002cf0 FUN_10002cf0

undefined2 * __thiscall FUN_10002cf0(void *this,ushort param_1)

{
  if ((DAT_100210f0 & 1) == 0) {
    DAT_100210f0 = DAT_100210f0 | 1;
    DAT_100210e8 = 0xffff;
    DAT_100210ea = 0xffff;
    DAT_100210ec = 0xffff;
    DAT_100210ee = 1;
  }
  if (param_1 != 0xffff) {
    return (undefined2 *)((uint)param_1 * 0x10 + *(int *)((int)this + 4));
  }
  return &DAT_100210e8;
}


// 10002d50 FUN_10002d50

void __thiscall FUN_10002d50(void *this,char *param_1,char *param_2)

{
  byte bVar1;
  
  bVar1 = *param_2 + 1;
  if (((bVar1 == 0) || (bVar1 != 0)) && ((int)(uint)bVar1 < *(int *)((int)this + 4))) {
    *param_1 = *param_2 + '\x01';
    return;
  }
  *param_1 = -1;
  return;
}


// 10002d90 FUN_10002d90

void __thiscall FUN_10002d90(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_10002deb;
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
LAB_10002deb:
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


// 10002e20 FUN_10002e20

void __thiscall FUN_10002e20(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_10002e7b;
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
LAB_10002e7b:
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


// 10002eb0 FUN_10002eb0

void __thiscall FUN_10002eb0(void *this,int param_1)

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
    if ((int)(uVar2 & 0xff) < (int)uVar4) {
      if (((char)uVar2 == '\0') && ((int)uVar4 < 0x100)) {
        uVar2 = uVar2 - 1;
      }
      else {
        if ((uVar4 & 0xff) != uVar4) {
          return;
        }
        do {
          uVar2 = (int)(uVar2 + uVar4) / 2;
        } while ((int)(uVar2 & 0xff) < (int)uVar4);
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


// 10003070 FUN_10003070

void __thiscall FUN_10003070(void *this,undefined4 param_1)

{
  byte *pbVar1;
  char cVar2;
  int iVar3;
  uint uVar4;
  
                    /* WARNING: Load size is inaccurate */
  iVar3 = (**(code **)(*this + 0x1c))(param_1);
                    /* WARNING: Load size is inaccurate */
  cVar2 = (**(code **)(*this + 0x20))(iVar3);
  if (cVar2 != '\0') {
    pbVar1 = (byte *)(*(int *)(*(int *)((int)this + 0x20) + iVar3 * 4) + 0x24);
    *pbVar1 = *pbVar1 | 1;
    uVar4 = (uint)*(ushort *)(*(int *)(*(int *)((int)this + 0x20) + iVar3 * 4) + 0x22);
    if ((int)uVar4 < *(int *)((int)this + 0x70)) {
      *(undefined1 *)(*(int *)((int)this + 100) + 4 + uVar4 * 8) = 1;
    }
    return;
  }
  Warning("Can\'t remove %s, no such sound!\n",param_1);
  return;
}


// 100030e0 FUN_100030e0

void __thiscall FUN_100030e0(void *this,undefined4 param_1)

{
  char cVar1;
  int iVar2;
  uint uVar3;
  undefined4 unaff_ESI;
  uint uVar4;
  undefined4 unaff_retaddr;
  
                    /* WARNING: Load size is inaccurate */
  iVar2 = (**(code **)(*this + 0x1c))(param_1);
                    /* WARNING: Load size is inaccurate */
  cVar1 = (**(code **)(*this + 0x20))(iVar2);
  if (cVar1 == '\0') {
    Warning("Can\'t move \'%s\', no such sound!\n",param_1);
    return;
  }
  uVar4 = (uint)*(ushort *)(*(int *)(*(int *)((int)this + 0x20) + iVar2 * 4) + 0x22);
  if ((int)uVar4 < *(int *)((int)this + 0x70)) {
                    /* WARNING: Load size is inaccurate */
    uVar3 = (**(code **)(*this + 0x7c))(unaff_retaddr);
    if (uVar3 == 0xffffffff) {
      Warning("CSoundEmitterSystemBase::MoveSound( \'%s\', \'%s\' ), script file not list in manifest \'%s\'\n"
              ,unaff_ESI,unaff_retaddr,"scripts/game_sounds_manifest.txt");
      return;
    }
    if (uVar4 != uVar3) {
      *(short *)(*(int *)(*(int *)((int)this + 0x20) + iVar2 * 4) + 0x22) = (short)uVar3;
      *(undefined1 *)(*(int *)((int)this + 100) + 4 + uVar4 * 8) = 1;
      *(undefined1 *)(*(int *)((int)this + 100) + 4 + uVar3 * 8) = 1;
    }
  }
  return;
}


// 10003200 FUN_10003200

void __thiscall FUN_10003200(void *this,undefined4 param_1)

{
  void *this_00;
  char cVar1;
  int iVar2;
  undefined4 uVar3;
  undefined4 *unaff_retaddr;
  
                    /* WARNING: Load size is inaccurate */
  iVar2 = (**(code **)(*this + 0x1c))(param_1);
                    /* WARNING: Load size is inaccurate */
  cVar1 = (**(code **)(*this + 0x20))(iVar2);
  if (cVar1 == '\0') {
    Msg("Can\'t UpdateSoundParameters %s, no such sound\n",param_1);
    return;
  }
  iVar2 = *(int *)(*(int *)((int)this + 0x20) + iVar2 * 4);
  this_00 = (void *)(iVar2 + 2);
  uVar3 = FUN_100081d0(this_00,unaff_retaddr);
  if ((char)uVar3 == '\0') {
    FUN_10008090(this_00,unaff_retaddr);
    *(undefined1 *)(*(int *)((int)this + 100) + 4 + (uint)*(ushort *)(iVar2 + 0x22) * 8) = 1;
  }
  return;
}


// 10003270 FUN_10003270

int __thiscall FUN_10003270(void *this,undefined4 *param_1)

{
  int iVar1;
  code *pcVar2;
  char cVar3;
  int iVar4;
  byte *pbVar5;
  byte *pbVar6;
  uint uVar7;
  int iVar8;
  int local_8;
  
  if (*(char *)((int)this + 0x18) != '\0') {
    _SpewInfo(2,"c:\\buildworker\\l4d_rel_win32\\build\\src\\public\\tier1/UtlSortVector.h",0xd5);
    iVar4 = _SpewMessage(&DAT_1001a674,"Assertion Failed: !m_bNeedsSort");
    if (iVar4 == 0) {
      cVar3 = ShouldUseNewAssertDialog();
      if ((cVar3 == '\0') ||
         (cVar3 = DoNewAssertDialog("c:\\buildworker\\l4d_rel_win32\\build\\src\\public\\tier1/UtlSortVector.h"
                                    ,0xd5,"Assertion Failed: !m_bNeedsSort"), cVar3 != '\0')) {
        pcVar2 = (code *)swi(3);
        iVar4 = (*pcVar2)();
        return iVar4;
      }
      _ExitOnFatalAssert("c:\\buildworker\\l4d_rel_win32\\build\\src\\public\\tier1/UtlSortVector.h"
                         ,0xd5);
    }
  }
  iVar4 = *(int *)((int)this + 0xc) + -1;
  local_8 = 0;
  if (iVar4 < 0) {
    return -1;
  }
  do {
                    /* WARNING: Load size is inaccurate */
    iVar1 = *this;
    iVar8 = local_8 + iVar4 >> 1;
    pbVar5 = (byte *)FUN_1000a660((ushort *)*param_1);
    pbVar6 = (byte *)FUN_1000a660(*(ushort **)(iVar1 + iVar8 * 4));
    uVar7 = FUN_10008b10(pbVar6,pbVar5);
    if ((int)uVar7 < 0) {
      local_8 = iVar8 + 1;
    }
    else {
                    /* WARNING: Load size is inaccurate */
      pbVar5 = (byte *)FUN_1000a660(*(ushort **)(*this + iVar8 * 4));
      pbVar6 = (byte *)FUN_1000a660((ushort *)*param_1);
      uVar7 = FUN_10008b10(pbVar6,pbVar5);
      if (-1 < (int)uVar7) {
        return iVar8;
      }
      iVar4 = iVar8 + -1;
    }
  } while (local_8 <= iVar4);
  return -1;
}


// 10003390 FUN_10003390

void __thiscall FUN_10003390(void *this,undefined1 param_1)

{
  undefined4 uVar1;
  
  uVar1 = FUN_1000c1f0(this,1);
  if ((char)uVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
    FUN_1000c240((int *)this);
  }
  return;
}


// 100033c0 FUN_100033c0

ushort * __thiscall FUN_100033c0(void *this,undefined4 param_1,ushort *param_2,undefined1 *param_3)

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
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_100210e8;
      }
      else {
        puVar3 = (ushort *)(*(int *)((int)this + 4) + iVar4);
      }
      uVar1 = puVar3[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_100210e8;
        uVar1 = DAT_100210e8;
      }
      else {
        puVar3 = (ushort *)(*(int *)((int)this + 4) + iVar4);
        uVar1 = *(ushort *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  }
  return puVar3;
}


// 100034d0 FUN_100034d0

undefined4 __thiscall FUN_100034d0(void *this,byte param_1)

{
  undefined1 *puVar1;
  
  if ((DAT_100210f8 & 1) == 0) {
    DAT_100210f8 = DAT_100210f8 | 1;
    DAT_100210f4 = 0xff;
    DAT_100210f5 = 0xff;
    DAT_100210f6 = 0xff;
    DAT_100210f7 = 1;
  }
  if (param_1 != 0xff) {
    puVar1 = (undefined1 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
    return CONCAT31((int3)((uint)puVar1 >> 8),*puVar1);
  }
  return CONCAT31(0x100210,DAT_100210f4);
}


// 10003520 FUN_10003520

undefined4 __thiscall FUN_10003520(void *this,byte param_1)

{
  int iVar1;
  
  if ((DAT_100210f8 & 1) == 0) {
    DAT_100210f8 = DAT_100210f8 | 1;
    DAT_100210f4 = 0xff;
    DAT_100210f5 = 0xff;
    DAT_100210f6 = 0xff;
    DAT_100210f7 = 1;
  }
  if (param_1 != 0xff) {
    iVar1 = *(int *)((int)this + 4) + (uint)param_1 * 0xc;
    return CONCAT31((int3)((uint)iVar1 >> 8),*(undefined1 *)(iVar1 + 1));
  }
  return CONCAT31(0x100210,DAT_100210f5);
}


// 10003580 FUN_10003580

void __fastcall FUN_10003580(int param_1)

{
  byte bVar1;
  byte bVar2;
  char *pcVar3;
  uint uVar4;
  
  uVar4 = DAT_100210f8;
  bVar1 = *(byte *)(param_1 + 0x10);
  while( true ) {
    bVar2 = bVar1;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_100210f4 = 0xff;
      DAT_100210f5 = 0xff;
      DAT_100210f6 = 0xff;
      DAT_100210f7 = 1;
      DAT_100210f8 = uVar4;
    }
    if (bVar2 == 0xff) {
      pcVar3 = (char *)&DAT_100210f4;
    }
    else {
      pcVar3 = (char *)(*(int *)(param_1 + 4) + (uint)bVar2 * 0xc);
    }
    if (*pcVar3 == -1) break;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_100210f4 = 0xff;
      DAT_100210f5 = 0xff;
      DAT_100210f6 = 0xff;
      DAT_100210f7 = 1;
      DAT_100210f8 = uVar4;
    }
    bVar1 = DAT_100210f4;
    if (bVar2 != 0xff) {
      bVar1 = *(byte *)(*(int *)(param_1 + 4) + (uint)bVar2 * 0xc);
    }
  }
  return;
}


// 10003620 FUN_10003620

undefined1 * __thiscall
FUN_10003620(void *this,undefined4 param_1,byte *param_2,undefined1 *param_3)

{
  byte bVar1;
  int iVar2;
  char cVar3;
  undefined1 *puVar4;
  
  bVar1 = *(byte *)((int)this + 0x10);
  *param_2 = 0xff;
  *param_3 = 0;
  puVar4 = param_3;
  while (bVar1 != 0xff) {
    *param_2 = bVar1;
    iVar2 = (uint)bVar1 * 0xc;
                    /* WARNING: Load size is inaccurate */
    cVar3 = (**this)(param_1,iVar2 + 4 + *(int *)((int)this + 4));
    if (cVar3 == '\0') {
      *param_3 = 0;
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      if (bVar1 == 0xff) {
        puVar4 = &DAT_100210f4;
      }
      else {
        puVar4 = (undefined1 *)(*(int *)((int)this + 4) + iVar2);
      }
      bVar1 = puVar4[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      if (bVar1 == 0xff) {
        puVar4 = &DAT_100210f4;
        bVar1 = DAT_100210f4;
      }
      else {
        puVar4 = (undefined1 *)(*(int *)((int)this + 4) + iVar2);
        bVar1 = *(byte *)(*(int *)((int)this + 4) + iVar2);
      }
    }
  }
  return puVar4;
}


// 10003710 FUN_10003710

undefined4 __thiscall FUN_10003710(void *this,byte param_1)

{
  int iVar1;
  
  if ((DAT_100210f8 & 1) == 0) {
    DAT_100210f8 = DAT_100210f8 | 1;
    DAT_100210f4 = 0xff;
    DAT_100210f5 = 0xff;
    DAT_100210f6 = 0xff;
    DAT_100210f7 = 1;
  }
  if (param_1 != 0xff) {
    iVar1 = *(int *)((int)this + 4) + (uint)param_1 * 0xc;
    return CONCAT31((int3)((uint)iVar1 >> 8),*(undefined1 *)(iVar1 + 2));
  }
  return CONCAT31(0x100210,DAT_100210f6);
}


// 10003770 FUN_10003770

byte __fastcall FUN_10003770(int param_1)

{
  byte *pbVar1;
  byte bVar2;
  byte *pbVar3;
  int iVar4;
  byte local_1;
  
  bVar2 = *(byte *)(param_1 + 0x12);
  if (bVar2 != 0xff) {
    *(undefined1 *)(param_1 + 0x12) = *(undefined1 *)(*(int *)(param_1 + 4) + 1 + (uint)bVar2 * 0xc)
    ;
    *(int *)(param_1 + 0x14) = *(int *)(param_1 + 4);
    return bVar2;
  }
  bVar2 = *(byte *)(param_1 + 0x13);
  pbVar1 = (byte *)(param_1 + 0x13);
  if (((bVar2 != 0) && (bVar2 == 0)) || (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)bVar2))
  {
    iVar4 = *(int *)(param_1 + 8);
    bVar2 = (0 < iVar4) - 1;
  }
  else {
    bVar2 = *pbVar1 + 1;
    if (((bVar2 == 0) || (bVar2 != 0)) && ((int)(uint)bVar2 < iVar4)) {
      bVar2 = *pbVar1 + 1;
    }
    else {
      bVar2 = 0xff;
    }
  }
  if (((bVar2 != 0) && (bVar2 == 0)) || (iVar4 <= (int)(uint)bVar2)) {
    FUN_10002eb0((void *)(param_1 + 4),1);
    bVar2 = *pbVar1;
    if (((bVar2 != 0) && (bVar2 == 0)) || (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)bVar2)
       ) {
      iVar4 = *(int *)(param_1 + 8);
      pbVar3 = &local_1;
    }
    else {
      pbVar3 = (byte *)FUN_10002d50((void *)(param_1 + 4),(char *)&local_1,(char *)pbVar1);
    }
    bVar2 = *pbVar3;
    if (((bVar2 != 0) && (bVar2 == 0)) || (iVar4 <= (int)(uint)bVar2)) {
      Error("CUtlRBTree overflow!\n");
    }
  }
  *pbVar1 = bVar2;
  *(undefined4 *)(param_1 + 0x14) = *(undefined4 *)(param_1 + 4);
  return bVar2;
}


// 10003870 FUN_10003870

void __thiscall FUN_10003870(void *this,ushort param_1)

{
  ushort uVar1;
  undefined2 *puVar2;
  short *psVar3;
  ushort *puVar4;
  
  puVar2 = FUN_10002cf0(this,param_1);
  uVar1 = puVar2[1];
  puVar2 = FUN_10002cf0(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + 2 + (uint)param_1 * 0x10) = *puVar2;
  psVar3 = FUN_10002cf0(this,uVar1);
  if (*psVar3 != -1) {
    puVar4 = FUN_10002cf0(this,uVar1);
    *(ushort *)((uint)*puVar4 * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar2 = FUN_10002cf0(this,param_1);
    *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar2[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar2 = FUN_10002cf0(this,param_1);
    puVar4 = FUN_10002cf0(this,puVar2[2]);
    if (*puVar4 == param_1) {
      puVar2 = FUN_10002cf0(this,param_1);
      *(ushort *)((uint)(ushort)puVar2[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
    }
    else {
      puVar2 = FUN_10002cf0(this,param_1);
      *(ushort *)((uint)(ushort)puVar2[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
    }
  }
  *(ushort *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x10) = uVar1;
  }
  return;
}


// 10003960 FUN_10003960

void __thiscall FUN_10003960(void *this,ushort param_1)

{
  ushort uVar1;
  ushort *puVar2;
  undefined2 *puVar3;
  
  puVar2 = FUN_10002cf0(this,param_1);
  uVar1 = *puVar2;
  puVar3 = FUN_10002cf0(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x10) = puVar3[1];
  puVar3 = FUN_10002cf0(this,uVar1);
  if (puVar3[1] != -1) {
    puVar3 = FUN_10002cf0(this,uVar1);
    *(ushort *)((uint)(ushort)puVar3[1] * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar3 = FUN_10002cf0(this,param_1);
    *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar3[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar3 = FUN_10002cf0(this,param_1);
    puVar3 = FUN_10002cf0(this,puVar3[2]);
    if (puVar3[1] == param_1) {
      puVar3 = FUN_10002cf0(this,param_1);
      *(ushort *)((uint)(ushort)puVar3[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
    }
    else {
      puVar3 = FUN_10002cf0(this,param_1);
      *(ushort *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
    }
  }
  *(ushort *)((uint)uVar1 * 0x10 + 2 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x10) = uVar1;
  }
  return;
}


// 10003a60 FUN_10003a60

void __thiscall FUN_10003a60(void *this,byte param_1)

{
  byte bVar1;
  uint uVar2;
  undefined4 uVar3;
  uint uVar4;
  
  uVar2 = FUN_10003520(this,param_1);
  bVar1 = (byte)uVar2;
  uVar3 = FUN_100034d0(this,bVar1);
  *(char *)(*(int *)((int)this + 4) + 1 + (uint)param_1 * 0xc) = (char)uVar3;
  uVar3 = FUN_100034d0(this,bVar1);
  if ((char)uVar3 != -1) {
    uVar4 = FUN_100034d0(this,bVar1);
    *(byte *)(*(int *)((int)this + 4) + 2 + (uVar4 & 0xff) * 0xc) = param_1;
  }
  if (bVar1 != 0xff) {
    uVar3 = FUN_10003710(this,param_1);
    *(char *)(*(int *)((int)this + 4) + 2 + (uVar2 & 0xff) * 0xc) = (char)uVar3;
  }
  if (param_1 == *(byte *)((int)this + 0x10)) {
    *(byte *)((int)this + 0x10) = bVar1;
  }
  else {
    uVar3 = FUN_10003710(this,param_1);
    uVar3 = FUN_100034d0(this,(byte)uVar3);
    if ((byte)uVar3 == param_1) {
      uVar4 = FUN_10003710(this,param_1);
      *(byte *)(*(int *)((int)this + 4) + (uVar4 & 0xff) * 0xc) = bVar1;
    }
    else {
      uVar4 = FUN_10003710(this,param_1);
      *(byte *)(*(int *)((int)this + 4) + 1 + (uVar4 & 0xff) * 0xc) = bVar1;
    }
  }
  *(byte *)(*(int *)((int)this + 4) + (uVar2 & 0xff) * 0xc) = param_1;
  if (param_1 != 0xff) {
    *(byte *)(*(int *)((int)this + 4) + 2 + (uint)param_1 * 0xc) = bVar1;
  }
  return;
}


// 10003b50 FUN_10003b50

void __thiscall FUN_10003b50(void *this,byte param_1)

{
  byte bVar1;
  uint uVar2;
  undefined4 uVar3;
  uint uVar4;
  
  uVar2 = FUN_100034d0(this,param_1);
  bVar1 = (byte)uVar2;
  uVar3 = FUN_10003520(this,bVar1);
  *(char *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc) = (char)uVar3;
  uVar3 = FUN_10003520(this,bVar1);
  if ((char)uVar3 != -1) {
    uVar4 = FUN_10003520(this,bVar1);
    *(byte *)(*(int *)((int)this + 4) + 2 + (uVar4 & 0xff) * 0xc) = param_1;
  }
  if (bVar1 != 0xff) {
    uVar3 = FUN_10003710(this,param_1);
    *(char *)(*(int *)((int)this + 4) + 2 + (uVar2 & 0xff) * 0xc) = (char)uVar3;
  }
  if (param_1 == *(byte *)((int)this + 0x10)) {
    *(byte *)((int)this + 0x10) = bVar1;
  }
  else {
    uVar3 = FUN_10003710(this,param_1);
    uVar3 = FUN_10003520(this,(byte)uVar3);
    if ((byte)uVar3 == param_1) {
      uVar4 = FUN_10003710(this,param_1);
      *(byte *)(*(int *)((int)this + 4) + 1 + (uVar4 & 0xff) * 0xc) = bVar1;
    }
    else {
      uVar4 = FUN_10003710(this,param_1);
      *(byte *)(*(int *)((int)this + 4) + (uVar4 & 0xff) * 0xc) = bVar1;
    }
  }
  *(byte *)(*(int *)((int)this + 4) + 1 + (uVar2 & 0xff) * 0xc) = param_1;
  if (param_1 != 0xff) {
    *(byte *)(*(int *)((int)this + 4) + 2 + (uint)param_1 * 0xc) = bVar1;
  }
  return;
}


// 10003c40 FUN_10003c40

void __fastcall FUN_10003c40(int *param_1)

{
  int iVar1;
  undefined4 uVar2;
  
  if ((*(byte *)((int)param_1 + 0x15) & 0x10) == 0) {
    iVar1 = param_1[6];
  }
  else {
    iVar1 = 0;
  }
  while (iVar1 = iVar1 + -1, -1 < iVar1) {
    uVar2 = FUN_1000c1f0(param_1,1);
    if ((char)uVar2 != '\0') {
      *(undefined1 *)((*param_1 - param_1[8]) + param_1[4]) = 9;
      param_1[4] = param_1[4] + 1;
      FUN_1000c240(param_1);
    }
  }
  return;
}


// 10003c90 FUN_10003c90

void __thiscall FUN_10003c90(void *this,undefined1 param_1)

{
  undefined4 uVar1;
  
                    /* WARNING: Load size is inaccurate */
  if ((((*(byte *)((int)this + 0x15) & 1) != 0) && (*(int *)((int)this + 0x10) != 0)) &&
     (*(char *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + -1 + *this) == '\n')) {
    FUN_10003c40((int *)this);
  }
  uVar1 = FUN_1000c1f0(this,1);
  if ((char)uVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
    FUN_1000c240((int *)this);
  }
  return;
}


// 10003d90 FUN_10003d90

int __thiscall FUN_10003d90(void *this,byte *param_1)

{
  int iVar1;
  undefined2 local_28;
  undefined4 local_26 [9];
  
  local_28 = 0xffff;
  FUN_100085f0(local_26);
  FUN_1000a610(&param_1,param_1);
  local_28 = param_1._0_2_;
  param_1 = (byte *)&local_28;
  iVar1 = FUN_10003270((void *)((int)this + 0x20),&param_1);
  FUN_10007fe0(local_26);
  if (iVar1 == -1) {
    return -1;
  }
  return iVar1;
}


// 10003e00 FUN_10003e00

int __thiscall FUN_10003e00(void *this,undefined4 *param_1)

{
  void *pvVar1;
  undefined4 *puVar2;
  code *pcVar3;
  char cVar4;
  int iVar5;
  int iVar6;
  
  if (*(char *)((int)this + 0x18) != '\0') {
    _SpewInfo(2,"c:\\buildworker\\l4d_rel_win32\\build\\src\\public\\tier1/UtlSortVector.h",0x94);
    iVar5 = _SpewMessage(&DAT_1001a674,"Assertion Failed: !m_bNeedsSort");
    if (iVar5 == 0) {
      cVar4 = ShouldUseNewAssertDialog();
      if (cVar4 != '\0') {
        cVar4 = DoNewAssertDialog("c:\\buildworker\\l4d_rel_win32\\build\\src\\public\\tier1/UtlSortVector.h"
                                  ,0x94,"Assertion Failed: !m_bNeedsSort");
        if (cVar4 == '\0') {
          _ExitOnFatalAssert("c:\\buildworker\\l4d_rel_win32\\build\\src\\public\\tier1/UtlSortVector.h"
                             ,0x94);
          goto LAB_10003e6e;
        }
      }
      pcVar3 = (code *)swi(3);
      iVar5 = (*pcVar3)();
      return iVar5;
    }
  }
LAB_10003e6e:
  iVar5 = FUN_100029e0(this,param_1);
  iVar5 = iVar5 + 1;
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10002b40(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar6 = (*(int *)((int)this + 0xc) - iVar5) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar6) {
    pvVar1 = (void *)(*this + iVar5 * 4);
    FUN_10008eb0((void *)((int)pvVar1 + 4),pvVar1,iVar6 * 4);
  }
                    /* WARNING: Load size is inaccurate */
  puVar2 = (undefined4 *)(*this + iVar5 * 4);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_1;
  }
  return iVar5;
}


// 10003ee0 FUN_10003ee0

void __thiscall FUN_10003ee0(void *this,int param_1)

{
  void *pvVar1;
  int iVar2;
  
  iVar2 = (*(int *)((int)this + 0xc) - param_1) + -1;
  if (0 < iVar2) {
                    /* WARNING: Load size is inaccurate */
    pvVar1 = (void *)(*this + param_1 * 4);
    FUN_10008eb0(pvVar1,(void *)((int)pvVar1 + 4),iVar2 * 4);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + -1;
  return;
}


// 10003f20 FUN_10003f20

ushort __thiscall FUN_10003f20(void *this,undefined4 param_1)

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
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      if (uVar3 == 0xffff) {
        puVar2 = &DAT_100210e8;
      }
      else {
        puVar2 = (undefined2 *)(*(int *)((int)this + 4) + iVar4);
      }
      uVar3 = puVar2[1];
    }
    else {
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      bVar5 = uVar3 != 0xffff;
      uVar3 = DAT_100210e8;
      if (bVar5) {
        uVar3 = *(ushort *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  } while (uVar3 != 0xffff);
  return 0xffff;
}


// 10004020 FUN_10004020

void __fastcall FUN_10004020(int *param_1)

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


// 10004090 FUN_10004090

int __thiscall FUN_10004090(void *this,int param_1,undefined4 *param_2)

{
  void *pvVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10002d90(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar3 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar3) {
    pvVar1 = (void *)(*this + param_1 * 8);
    FUN_10008eb0((void *)((int)pvVar1 + 8),pvVar1,iVar3 * 8);
  }
                    /* WARNING: Load size is inaccurate */
  puVar2 = (undefined4 *)(*this + param_1 * 8);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_2;
    puVar2[1] = param_2[1];
  }
  return param_1;
}


// 10004100 FUN_10004100

void __fastcall FUN_10004100(void *param_1)

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
         ((uVar2 <= uVar1 && (puVar3 = FUN_10002cf0(param_1,uVar2), *puVar3 != uVar2)))) {
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


// 100041d0 FUN_100041d0

int __thiscall FUN_100041d0(void *this,int param_1,undefined4 *param_2)

{
  void *pvVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10002b40(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar3 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar3) {
    pvVar1 = (void *)(*this + param_1 * 4);
    FUN_10008eb0((void *)((int)pvVar1 + 4),pvVar1,iVar3 * 4);
  }
                    /* WARNING: Load size is inaccurate */
  puVar2 = (undefined4 *)(*this + param_1 * 4);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_2;
  }
  return param_1;
}


// 10004240 FUN_10004240

byte __thiscall FUN_10004240(void *this,undefined4 param_1)

{
  int iVar1;
  char cVar2;
  undefined1 *puVar3;
  byte bVar4;
  bool bVar5;
  
  bVar4 = *(byte *)((int)this + 0x10);
  if (bVar4 == 0xff) {
    return 0xff;
  }
  do {
                    /* WARNING: Load size is inaccurate */
    iVar1 = (uint)bVar4 * 0xc;
    cVar2 = (**this)(param_1,*(int *)((int)this + 4) + 4 + iVar1);
    if (cVar2 == '\0') {
                    /* WARNING: Load size is inaccurate */
      cVar2 = (**this)(*(int *)((int)this + 4) + 4 + iVar1,param_1);
      if (cVar2 == '\0') {
        return bVar4;
      }
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      if (bVar4 == 0xff) {
        puVar3 = &DAT_100210f4;
      }
      else {
        puVar3 = (undefined1 *)(*(int *)((int)this + 4) + iVar1);
      }
      bVar4 = puVar3[1];
    }
    else {
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      bVar5 = bVar4 != 0xff;
      bVar4 = DAT_100210f4;
      if (bVar5) {
        bVar4 = *(byte *)(*(int *)((int)this + 4) + iVar1);
      }
    }
  } while (bVar4 != 0xff);
  return 0xff;
}


// 10004330 FUN_10004330

void __fastcall FUN_10004330(void *param_1)

{
  byte bVar1;
  int iVar2;
  byte bVar3;
  undefined4 uVar4;
  byte bVar5;
  
  bVar1 = *(byte *)((int)param_1 + 0x13);
  if (bVar1 != 0xff) {
    bVar5 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (bVar5 != 0xff) {
      if ((((bVar5 == 0) || (bVar5 != 0)) && ((int)(uint)bVar5 < *(int *)((int)param_1 + 8))) &&
         ((bVar5 <= bVar1 && (uVar4 = FUN_100034d0(param_1,bVar5), (byte)uVar4 != bVar5)))) {
        iVar2 = (uint)bVar5 * 0xc;
        *(undefined1 *)(*(int *)((int)param_1 + 4) + 1 + iVar2) =
             *(undefined1 *)((int)param_1 + 0x12);
        *(byte *)(iVar2 + *(int *)((int)param_1 + 4)) = bVar5;
        *(byte *)((int)param_1 + 0x12) = bVar5;
      }
      bVar1 = *(byte *)((int)param_1 + 0x13);
      if (bVar5 == bVar1) break;
      bVar3 = bVar5 + 1;
      if (((bVar3 == 0) || (bVar3 != 0)) && ((int)(uint)bVar3 < *(int *)((int)param_1 + 8))) {
        bVar5 = bVar5 + 1;
      }
      else {
        bVar5 = 0xff;
      }
    }
    *(undefined1 *)((int)param_1 + 0x10) = 0xff;
    *(undefined1 *)((int)param_1 + 0x11) = 0;
  }
  return;
}


// 100043e0 FUN_100043e0

void __thiscall FUN_100043e0(void *this,uint param_1)

{
  undefined4 uVar1;
  uint uVar2;
  char *pcVar3;
  undefined1 *puVar4;
  uint uVar5;
  byte bVar6;
  
  bVar6 = (byte)param_1;
  uVar1 = FUN_10003520(this,bVar6);
  if ((char)uVar1 == -1) {
    uVar2 = FUN_10003710(this,bVar6);
    uVar5 = DAT_100210f8;
    while( true ) {
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
        DAT_100210f8 = uVar5;
      }
      if ((char)param_1 == -1) {
        puVar4 = &DAT_100210f4;
      }
      else {
        puVar4 = (undefined1 *)(*(int *)((int)this + 4) + (param_1 & 0xff) * 0xc);
      }
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
        DAT_100210f8 = uVar5;
      }
      if (puVar4[2] == 0xff) {
        puVar4 = &DAT_100210f4;
      }
      else {
        puVar4 = (undefined1 *)(*(int *)((int)this + 4) + (uint)(byte)puVar4[2] * 0xc);
      }
      if ((puVar4[1] != (char)param_1) || (param_1 = uVar2 & 0xff, (char)uVar2 == -1)) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
        DAT_100210f8 = uVar5;
      }
      uVar2 = (uint)*(byte *)(*(int *)((int)this + 4) + (uVar2 & 0xff) * 0xc + 2);
    }
  }
  else {
    uVar2 = FUN_10003520(this,bVar6);
    uVar5 = DAT_100210f8;
    while( true ) {
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
        DAT_100210f8 = uVar5;
      }
      if ((char)uVar2 == -1) {
        pcVar3 = (char *)&DAT_100210f4;
      }
      else {
        pcVar3 = (char *)(*(int *)((int)this + 4) + (uVar2 & 0xff) * 0xc);
      }
      if (*pcVar3 == -1) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
        DAT_100210f8 = uVar5;
      }
      if ((char)uVar2 == -1) {
        uVar2 = (uint)DAT_100210f4;
      }
      else {
        uVar2 = (uint)*(byte *)(*(int *)((int)this + 4) + (uVar2 & 0xff) * 0xc);
      }
    }
  }
  return;
}


// 10004580 FUN_10004580

void __thiscall FUN_10004580(void *this,uint param_1)

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
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      sVar7 = (short)param_1;
      if (sVar7 == -1) {
        puVar3 = &DAT_100210e8;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      if (puVar3[2] == 0xffff) {
        puVar3 = &DAT_100210e8;
      }
      else {
        puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (puVar3[3] != 0) break;
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      if (sVar7 == -1) {
        puVar3 = &DAT_100210e8;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      uVar1 = puVar3[2];
      uVar8 = (uint)uVar1;
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_100210e8;
      }
      else {
        puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      }
      uVar5 = puVar3[2];
      uVar6 = (uint)uVar5;
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_100210e8;
      }
      else {
        puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_100210f0 & 1) == 0) {
        DAT_100210f0 = DAT_100210f0 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
      }
      if (puVar3[2] == 0xffff) {
        puVar4 = &DAT_100210e8;
      }
      else {
        puVar4 = (ushort *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (*puVar4 == uVar1) {
        if ((DAT_100210f0 & 1) == 0) {
          DAT_100210f0 = DAT_100210f0 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        uVar5 = puVar3[1];
        if ((DAT_100210f0 & 1) == 0) {
          DAT_100210f0 = DAT_100210f0 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
LAB_100049a0:
          *(undefined2 *)((uint)uVar1 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          param_1 = uVar6;
        }
        else {
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (sVar7 == -1) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (puVar3[2] == 0xffff) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[1] == sVar7) {
            param_1 = (uint)uVar1;
            FUN_10003870(this,uVar1);
            if ((DAT_100210f0 & 1) == 0) {
              DAT_100210f0 = DAT_100210f0 | 1;
              DAT_100210e8 = 0xffff;
              DAT_100210ea = 0xffff;
              DAT_100210ec = 0xffff;
              DAT_100210ee = 1;
            }
            if (uVar1 == 0xffff) {
              puVar3 = &DAT_100210e8;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
            }
            uVar1 = puVar3[2];
            uVar8 = (uint)uVar1;
            if ((DAT_100210f0 & 1) == 0) {
              DAT_100210f0 = DAT_100210f0 | 1;
              DAT_100210e8 = 0xffff;
              DAT_100210ea = 0xffff;
              DAT_100210ec = 0xffff;
              DAT_100210ee = 1;
            }
            if (uVar1 == 0xffff) {
              puVar3 = &DAT_100210e8;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
            }
            uVar6 = (uint)(ushort)puVar3[2];
          }
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_10003960(this,(ushort)uVar6);
        }
      }
      else {
        if ((DAT_100210f0 & 1) == 0) {
          DAT_100210f0 = DAT_100210f0 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
        }
        if (uVar5 == 0xffff) {
          puVar4 = &DAT_100210e8;
        }
        else {
          puVar4 = (ushort *)(uVar6 * 0x10 + *(int *)((int)this + 4));
        }
        uVar5 = *puVar4;
        if ((DAT_100210f0 & 1) == 0) {
          DAT_100210f0 = DAT_100210f0 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) goto LAB_100049a0;
        if ((DAT_100210f0 & 1) == 0) {
          DAT_100210f0 = DAT_100210f0 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
        }
        if (sVar7 == -1) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
        }
        if ((DAT_100210f0 & 1) == 0) {
          DAT_100210f0 = DAT_100210f0 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
        }
        if (puVar3[2] == 0xffff) {
          psVar2 = &DAT_100210e8;
        }
        else {
          psVar2 = (short *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
        }
        if (*psVar2 == sVar7) {
          param_1 = (uint)uVar1;
          FUN_10003960(this,uVar1);
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (uVar1 == 0xffff) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
          }
          uVar1 = puVar3[2];
          uVar8 = (uint)uVar1;
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (uVar1 == 0xffff) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
          }
          uVar6 = (uint)(ushort)puVar3[2];
        }
        *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
        FUN_10003870(this,(ushort)uVar6);
      }
    } while ((short)param_1 != *(short *)((int)this + 0x10));
  }
  *(undefined2 *)((uint)*(ushort *)((int)this + 0x10) * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
  return;
}


// 10004b60 FUN_10004b60

void __thiscall FUN_10004b60(void *this,uint param_1)

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
  
  uVar2 = DAT_100210f0;
  if ((short)param_1 != *(short *)((int)this + 0x10)) {
    do {
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
        DAT_100210f0 = uVar2;
      }
      sVar6 = (short)param_1;
      if (sVar6 == -1) {
        puVar3 = &DAT_100210e8;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      if (puVar3[3] != 1) break;
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
        DAT_100210f0 = uVar2;
      }
      if (sVar6 == -1) {
        puVar3 = &DAT_100210e8;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      uVar7 = puVar3[2];
      uVar9 = (uint)uVar7;
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_100210e8 = 0xffff;
        DAT_100210ea = 0xffff;
        DAT_100210ec = 0xffff;
        DAT_100210ee = 1;
        DAT_100210f0 = uVar2;
      }
      if (uVar7 == 0xffff) {
        psVar4 = &DAT_100210e8;
      }
      else {
        psVar4 = (short *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
      }
      if (sVar6 == *psVar4) {
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
        }
        uVar1 = puVar3[1];
        uVar8 = (uint)uVar1;
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (uVar1 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar7 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_10003870(this,uVar7);
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (sVar6 == -1) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          uVar7 = puVar3[2];
          uVar9 = (uint)uVar7;
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (uVar7 == 0xffff) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
          }
          uVar8 = (uint)(ushort)puVar3[1];
          uVar2 = DAT_100210f0;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        uVar7 = (ushort)uVar8;
        if (uVar7 == 0xffff) {
          puVar5 = &DAT_100210e8;
        }
        else {
          puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (*puVar5 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)*puVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] != 1) {
LAB_10004ead:
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
            DAT_100210f0 = uVar2;
          }
          if (uVar7 == 0xffff) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
          }
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
            DAT_100210f0 = uVar2;
          }
          if (puVar3[1] == 0xffff) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[3] == 1) {
            if ((uVar2 & 1) == 0) {
              DAT_100210f0 = uVar2 | 1;
              DAT_100210e8 = 0xffff;
              DAT_100210ea = 0xffff;
              DAT_100210ec = 0xffff;
              DAT_100210ee = 1;
            }
            if (uVar7 == 0xffff) {
              puVar5 = &DAT_100210e8;
            }
            else {
              puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
            }
            *(undefined2 *)((uint)*puVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
            *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
            FUN_10003960(this,uVar7);
            if ((DAT_100210f0 & 1) == 0) {
              DAT_100210f0 = DAT_100210f0 | 1;
              DAT_100210e8 = 0xffff;
              DAT_100210ea = 0xffff;
              DAT_100210ec = 0xffff;
              DAT_100210ee = 1;
            }
            if (sVar6 == -1) {
              puVar3 = &DAT_100210e8;
            }
            else {
              puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
            }
            uVar7 = puVar3[2];
            uVar9 = (uint)uVar7;
            if ((DAT_100210f0 & 1) == 0) {
              DAT_100210f0 = DAT_100210f0 | 1;
              DAT_100210e8 = 0xffff;
              DAT_100210ea = 0xffff;
              DAT_100210ec = 0xffff;
              DAT_100210ee = 1;
            }
            if (uVar7 == 0xffff) {
              puVar3 = &DAT_100210e8;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
            }
            uVar8 = (uint)(ushort)puVar3[1];
            uVar2 = DAT_100210f0;
          }
          if ((uVar2 & 1) == 0) {
            DAT_100210f0 = uVar2 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if ((ushort)uVar9 == 0xffff) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)(uVar9 * 0x10 + *(int *)((int)this + 4));
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar8 * 0x10) = puVar3[3];
          *(undefined2 *)(uVar9 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if ((short)uVar8 == -1) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)(*(int *)((int)this + 4) + uVar8 * 0x10);
          }
          *(undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          FUN_10003870(this,(ushort)uVar9);
          goto LAB_1000555d;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (puVar3[1] == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] != 1) goto LAB_10004ead;
        param_1 = uVar9;
        if (uVar7 != 0xffff) {
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          uVar2 = DAT_100210f0;
        }
      }
      else {
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar5 = &DAT_100210e8;
        }
        else {
          puVar5 = (ushort *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
        }
        uVar1 = *puVar5;
        uVar8 = (uint)uVar1;
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (uVar1 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar7 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_10003960(this,uVar7);
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (sVar6 == -1) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          uVar7 = puVar3[2];
          uVar9 = (uint)uVar7;
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (uVar7 == 0xffff) {
            puVar5 = &DAT_100210e8;
          }
          else {
            puVar5 = (ushort *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
          }
          uVar8 = (uint)*puVar5;
          uVar2 = DAT_100210f0;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        uVar7 = (ushort)uVar8;
        if (uVar7 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (puVar3[1] == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 1) {
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
            DAT_100210f0 = uVar2;
          }
          if (uVar7 == 0xffff) {
            puVar5 = &DAT_100210e8;
          }
          else {
            puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
          }
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
            DAT_100210f0 = uVar2;
          }
          if (*puVar5 == 0xffff) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((uint)*puVar5 * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[3] == 1) {
            param_1 = uVar9;
            if (uVar7 != 0xffff) {
              *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
              uVar2 = DAT_100210f0;
            }
            goto LAB_1000556a;
          }
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (uVar7 == 0xffff) {
          puVar5 = &DAT_100210e8;
        }
        else {
          puVar5 = (ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar2;
        }
        if (*puVar5 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)((uint)*puVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 1) {
          if ((uVar2 & 1) == 0) {
            DAT_100210f0 = uVar2 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (uVar7 == 0xffff) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)(uVar8 * 0x10 + *(int *)((int)this + 4));
          }
          *(undefined2 *)((uint)(ushort)puVar3[1] * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_10003870(this,uVar7);
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (sVar6 == -1) {
            puVar3 = &DAT_100210e8;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          uVar7 = puVar3[2];
          uVar9 = (uint)uVar7;
          if ((DAT_100210f0 & 1) == 0) {
            DAT_100210f0 = DAT_100210f0 | 1;
            DAT_100210e8 = 0xffff;
            DAT_100210ea = 0xffff;
            DAT_100210ec = 0xffff;
            DAT_100210ee = 1;
          }
          if (uVar7 == 0xffff) {
            puVar5 = &DAT_100210e8;
          }
          else {
            puVar5 = (ushort *)((uint)uVar7 * 0x10 + *(int *)((int)this + 4));
          }
          uVar8 = (uint)*puVar5;
          uVar2 = DAT_100210f0;
        }
        if ((uVar2 & 1) == 0) {
          DAT_100210f0 = uVar2 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
        }
        if ((ushort)uVar9 == 0xffff) {
          puVar3 = &DAT_100210e8;
        }
        else {
          puVar3 = (undefined2 *)(uVar9 * 0x10 + *(int *)((int)this + 4));
        }
        *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar8 * 0x10) = puVar3[3];
        *(undefined2 *)(uVar9 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        if ((DAT_100210f0 & 1) == 0) {
          DAT_100210f0 = DAT_100210f0 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
        }
        if ((short)uVar8 == -1) {
          puVar5 = &DAT_100210e8;
        }
        else {
          puVar5 = (ushort *)(*(int *)((int)this + 4) + uVar8 * 0x10);
        }
        *(undefined2 *)((uint)*puVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        FUN_10003960(this,(ushort)uVar9);
LAB_1000555d:
        uVar2 = DAT_100210f0;
        param_1 = (uint)*(ushort *)((int)this + 0x10);
      }
LAB_1000556a:
    } while ((short)param_1 != *(short *)((int)this + 0x10));
  }
  *(undefined2 *)((param_1 & 0xffff) * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
  return;
}


// 100055a0 FUN_100055a0

void __thiscall FUN_100055a0(void *this,byte param_1)

{
  byte bVar1;
  undefined1 *puVar2;
  byte *pbVar3;
  byte bVar4;
  byte bVar5;
  
  if (param_1 != *(byte *)((int)this + 0x10)) {
    do {
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      if (param_1 == 0xff) {
        puVar2 = &DAT_100210f4;
      }
      else {
        puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
      }
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      if (puVar2[2] == 0xff) {
        puVar2 = &DAT_100210f4;
      }
      else {
        puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)(byte)puVar2[2] * 0xc);
      }
      if (puVar2[3] != '\0') break;
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      if (param_1 == 0xff) {
        puVar2 = &DAT_100210f4;
      }
      else {
        puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
      }
      bVar1 = puVar2[2];
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      if (bVar1 == 0xff) {
        puVar2 = &DAT_100210f4;
      }
      else {
        puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)bVar1 * 0xc);
      }
      bVar4 = puVar2[2];
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      if (bVar1 == 0xff) {
        puVar2 = &DAT_100210f4;
      }
      else {
        puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)bVar1 * 0xc);
      }
      if ((DAT_100210f8 & 1) == 0) {
        DAT_100210f8 = DAT_100210f8 | 1;
        DAT_100210f4 = 0xff;
        DAT_100210f5 = 0xff;
        DAT_100210f6 = 0xff;
        DAT_100210f7 = 1;
      }
      if (puVar2[2] == 0xff) {
        pbVar3 = &DAT_100210f4;
      }
      else {
        pbVar3 = (byte *)(*(int *)((int)this + 4) + (uint)(byte)puVar2[2] * 0xc);
      }
      if (*pbVar3 == bVar1) {
        if ((DAT_100210f8 & 1) == 0) {
          DAT_100210f8 = DAT_100210f8 | 1;
          DAT_100210f4 = 0xff;
          DAT_100210f5 = 0xff;
          DAT_100210f6 = 0xff;
          DAT_100210f7 = 1;
        }
        if (bVar4 == 0xff) {
          puVar2 = &DAT_100210f4;
        }
        else {
          puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)bVar4 * 0xc);
        }
        bVar5 = puVar2[1];
        if ((DAT_100210f8 & 1) == 0) {
          DAT_100210f8 = DAT_100210f8 | 1;
          DAT_100210f4 = 0xff;
          DAT_100210f5 = 0xff;
          DAT_100210f6 = 0xff;
          DAT_100210f7 = 1;
        }
        if (bVar5 == 0xff) {
          puVar2 = &DAT_100210f4;
        }
        else {
          puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)bVar5 * 0xc);
        }
        if (puVar2[3] == '\0') {
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar1 * 0xc) = 1;
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar5 * 0xc) = 1;
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar4 * 0xc) = 0;
          param_1 = bVar4;
        }
        else {
          if ((DAT_100210f8 & 1) == 0) {
            DAT_100210f8 = DAT_100210f8 | 1;
            DAT_100210f4 = 0xff;
            DAT_100210f5 = 0xff;
            DAT_100210f6 = 0xff;
            DAT_100210f7 = 1;
          }
          if (param_1 == 0xff) {
            puVar2 = &DAT_100210f4;
          }
          else {
            puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
          }
          if ((DAT_100210f8 & 1) == 0) {
            DAT_100210f8 = DAT_100210f8 | 1;
            DAT_100210f4 = 0xff;
            DAT_100210f5 = 0xff;
            DAT_100210f6 = 0xff;
            DAT_100210f7 = 1;
          }
          if (puVar2[2] == 0xff) {
            puVar2 = &DAT_100210f4;
          }
          else {
            puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)(byte)puVar2[2] * 0xc);
          }
          bVar5 = bVar1;
          if (puVar2[1] == param_1) {
            FUN_10003a60(this,bVar1);
            if ((DAT_100210f8 & 1) == 0) {
              DAT_100210f8 = DAT_100210f8 | 1;
              DAT_100210f4 = 0xff;
              DAT_100210f5 = 0xff;
              DAT_100210f6 = 0xff;
              DAT_100210f7 = 1;
            }
            if (bVar1 == 0xff) {
              puVar2 = &DAT_100210f4;
            }
            else {
              puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)bVar1 * 0xc);
            }
            bVar5 = puVar2[2];
            if ((DAT_100210f8 & 1) == 0) {
              DAT_100210f8 = DAT_100210f8 | 1;
              DAT_100210f4 = 0xff;
              DAT_100210f5 = 0xff;
              DAT_100210f6 = 0xff;
              DAT_100210f7 = 1;
            }
            if (bVar5 == 0xff) {
              puVar2 = &DAT_100210f4;
            }
            else {
              puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)bVar5 * 0xc);
            }
            bVar4 = puVar2[2];
            param_1 = bVar1;
          }
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar5 * 0xc) = 1;
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar4 * 0xc) = 0;
          FUN_10003b50(this,bVar4);
        }
      }
      else {
        if ((DAT_100210f8 & 1) == 0) {
          DAT_100210f8 = DAT_100210f8 | 1;
          DAT_100210f4 = 0xff;
          DAT_100210f5 = 0xff;
          DAT_100210f6 = 0xff;
          DAT_100210f7 = 1;
        }
        if (bVar4 == 0xff) {
          pbVar3 = &DAT_100210f4;
        }
        else {
          pbVar3 = (byte *)(*(int *)((int)this + 4) + (uint)bVar4 * 0xc);
        }
        bVar5 = *pbVar3;
        if ((DAT_100210f8 & 1) == 0) {
          DAT_100210f8 = DAT_100210f8 | 1;
          DAT_100210f4 = 0xff;
          DAT_100210f5 = 0xff;
          DAT_100210f6 = 0xff;
          DAT_100210f7 = 1;
        }
        if (bVar5 == 0xff) {
          puVar2 = &DAT_100210f4;
        }
        else {
          puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)bVar5 * 0xc);
        }
        if (puVar2[3] == '\0') {
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar1 * 0xc) = 1;
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar5 * 0xc) = 1;
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar4 * 0xc) = 0;
          param_1 = bVar4;
        }
        else {
          if ((DAT_100210f8 & 1) == 0) {
            DAT_100210f8 = DAT_100210f8 | 1;
            DAT_100210f4 = 0xff;
            DAT_100210f5 = 0xff;
            DAT_100210f6 = 0xff;
            DAT_100210f7 = 1;
          }
          if (param_1 == 0xff) {
            puVar2 = &DAT_100210f4;
          }
          else {
            puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
          }
          if ((DAT_100210f8 & 1) == 0) {
            DAT_100210f8 = DAT_100210f8 | 1;
            DAT_100210f4 = 0xff;
            DAT_100210f5 = 0xff;
            DAT_100210f6 = 0xff;
            DAT_100210f7 = 1;
          }
          if (puVar2[2] == 0xff) {
            pbVar3 = &DAT_100210f4;
          }
          else {
            pbVar3 = (byte *)(*(int *)((int)this + 4) + (uint)(byte)puVar2[2] * 0xc);
          }
          bVar5 = bVar1;
          if (*pbVar3 == param_1) {
            FUN_10003b50(this,bVar1);
            if ((DAT_100210f8 & 1) == 0) {
              DAT_100210f8 = DAT_100210f8 | 1;
              DAT_100210f4 = 0xff;
              DAT_100210f5 = 0xff;
              DAT_100210f6 = 0xff;
              DAT_100210f7 = 1;
            }
            if (bVar1 == 0xff) {
              puVar2 = &DAT_100210f4;
            }
            else {
              puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)bVar1 * 0xc);
            }
            bVar5 = puVar2[2];
            if ((DAT_100210f8 & 1) == 0) {
              DAT_100210f8 = DAT_100210f8 | 1;
              DAT_100210f4 = 0xff;
              DAT_100210f5 = 0xff;
              DAT_100210f6 = 0xff;
              DAT_100210f7 = 1;
            }
            if (bVar5 == 0xff) {
              puVar2 = &DAT_100210f4;
            }
            else {
              puVar2 = (undefined1 *)(*(int *)((int)this + 4) + (uint)bVar5 * 0xc);
            }
            bVar4 = puVar2[2];
            param_1 = bVar1;
          }
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar5 * 0xc) = 1;
          *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)bVar4 * 0xc) = 0;
          FUN_10003a60(this,bVar4);
        }
      }
    } while (param_1 != *(byte *)((int)this + 0x10));
  }
  *(undefined1 *)(*(int *)((int)this + 4) + 3 + (uint)*(byte *)((int)this + 0x10) * 0xc) = 1;
  return;
}


// 10005be0 FUN_10005be0

void __thiscall FUN_10005be0(void *this,byte *param_1)

{
  int iVar1;
  char cVar2;
  int iVar3;
  int iVar4;
  char *pcVar5;
  byte *pbVar6;
  uint uVar7;
  undefined4 *puVar8;
  byte *pbVar9;
  undefined4 *puVar10;
  int iVar11;
  char *pcVar12;
  int iVar13;
  byte *pbStack_48;
  undefined1 auStack_44 [4];
  undefined4 uStack_40;
  undefined1 auStack_3c [32];
  int iStack_1c;
  
                    /* WARNING: Load size is inaccurate */
  pbStack_48 = (byte *)(**(code **)(*this + 0x74))();
  if (pbStack_48 == (byte *)0x0) {
    Msg("CSoundEmitterSystemBase::SaveChangesToSoundScript:  No script file for index %i\n",param_1)
    ;
    return;
  }
  cVar2 = (**(code **)(*(int *)(DAT_10021788 + 4) + 0x28))(pbStack_48,0);
  if ((cVar2 != '\0') &&
     (cVar2 = (**(code **)(*(int *)(DAT_10021788 + 4) + 0x2c))(pbStack_48,0), cVar2 == '\0')) {
    Warning("%s is not writable, can\'t save data to file\n",pbStack_48);
    return;
  }
  FUN_1000c740(auStack_3c,0,0,1);
  cVar2 = (**(code **)(*(int *)(DAT_10021788 + 4) + 0x28))("scripts/game_sounds_header.txt",0);
  pbVar9 = pbStack_48;
  if (cVar2 != '\0') {
    iVar3 = (**(code **)(*(int *)(DAT_10021788 + 4) + 8))
                      ("scripts/game_sounds_header.txt",&DAT_1001a93c,0);
    if (iVar3 != 0) {
      iVar4 = (**(code **)(*(int *)(DAT_10021788 + 4) + 0x1c))(iVar3);
      pcVar5 = (char *)FUN_10001110(iVar4 + 1);
      FUN_10008e50(pcVar5,0,iVar4 + 1);
      (*(code *)**(undefined4 **)(DAT_10021788 + 4))(pcVar5,iVar4,iVar3);
      (**(code **)(*(int *)(DAT_10021788 + 4) + 0xc))(iVar3);
      pcVar5[iVar4] = '\0';
      cVar2 = *pcVar5;
      pcVar12 = pcVar5;
      while (cVar2 != '\0') {
        if (*pcVar12 != '\r') {
          FUN_10003c90(auStack_44,*pcVar12);
        }
        pcVar12 = pcVar12 + 1;
        cVar2 = *pcVar12;
      }
      FUN_100012c0(pcVar5);
      pbVar9 = param_1;
    }
    FUN_1000c950(auStack_44,"\n");
  }
                    /* WARNING: Load size is inaccurate */
  iVar3 = (**(code **)(*this + 0x24))();
  iVar4 = 0;
  if (0 < iVar3) {
    do {
                    /* WARNING: Load size is inaccurate */
      pbVar6 = (byte *)(**(code **)(*this + 0x48))(iVar4);
      uVar7 = FUN_10008b10(pbVar9,pbVar6);
                    /* WARNING: Load size is inaccurate */
      if (((uVar7 == 0) &&
          ((*(byte *)(*(int *)(*(int *)((int)this + 0x20) + iVar4 * 4) + 0x24) & 1) == 0)) &&
         (puVar8 = (undefined4 *)(**(code **)(*this + 0x58))(iVar4), puVar8 != (undefined4 *)0x0)) {
                    /* WARNING: Load size is inaccurate */
        (**(code **)(*this + 0x28))(iVar4);
        FUN_1000c950(auStack_44,"\"%s\"\n{\n");
        FUN_10008420((int)puVar8);
        FUN_1000c950(auStack_44,"\t\"channel\"\t\t\"%s\"\n");
        FUN_100083b0((int)puVar8);
        FUN_1000c950(auStack_44,"\t\"volume\"\t\t\"%s\"\n");
        FUN_100084d0((int)puVar8);
        FUN_1000c950(auStack_44,"\t\"pitch\"\t\t\t\"%s\"\n");
        FUN_1000c950(auStack_44,"\n");
        FUN_10008460((int)puVar8);
        FUN_1000c950(auStack_44,"\t\"soundlevel\"\t\"%s\"\n");
        if ((*(byte *)((int)puVar8 + 0x1a) & 1) != 0) {
          FUN_1000c950(auStack_44,"\t\"play_to_owner_only\"\t\"1\"\n");
        }
        if (*(short *)(puVar8 + 6) != 0) {
          FUN_1000c950(auStack_44,"\t\"delay_msec\"\t\"%i\"\n");
        }
        pbStack_48 = (byte *)(uint)*(ushort *)(puVar8 + 2);
        pbVar9 = (byte *)(uint)*(ushort *)((int)puVar8 + 10);
        if (pbStack_48 != pbVar9 && -1 < (int)pbStack_48 - (int)pbVar9) {
          FUN_1000c950(auStack_44,"\n");
          if (pbStack_48 == (byte *)0x1) {
            if (*(short *)(puVar8 + 2) != 1) {
              puVar8 = (undefined4 *)*puVar8;
            }
                    /* WARNING: Load size is inaccurate */
            (**(code **)(*this + 0x30))(puVar8);
            FUN_1000c950(auStack_44,"\t\"wave\"\t\t\t\"%s\"\n");
          }
          else if (pbVar9 == (byte *)0x1) {
            if (*(short *)((int)puVar8 + 10) == 1) {
              puVar8 = puVar8 + 1;
            }
            else {
              puVar8 = (undefined4 *)puVar8[1];
            }
                    /* WARNING: Load size is inaccurate */
            (**(code **)(*this + 0x30))(puVar8);
            FUN_1000c950(auStack_44,"\t\"wave\"\t\t\t\"%s\"\n");
          }
          else {
            FUN_1000c950(auStack_44,"\t\"rndwave\"\n");
            FUN_1000c950(auStack_44,"\t{\n");
            iVar13 = 0;
            if (pbStack_48 != (byte *)0x0) {
              do {
                puVar10 = FUN_10007e30(puVar8);
                if (*(char *)((int)puVar10 + iVar13 * 4 + 2) == '\0') {
                    /* WARNING: Load size is inaccurate */
                  iVar1 = *this;
                  puVar10 = FUN_10007e30(puVar8);
                  (**(code **)(iVar1 + 0x30))(puVar10 + iVar13);
                  FUN_1000c950(auStack_44,"\t\t\"wave\"\t\"%s\"\n");
                }
                iVar13 = iVar13 + 1;
              } while (iVar13 < (int)pbStack_48);
            }
            iVar13 = 0;
            if (0 < (int)pbVar9) {
              do {
                    /* WARNING: Load size is inaccurate */
                iVar1 = *this;
                iVar11 = FUN_100014a0((int)puVar8);
                (**(code **)(iVar1 + 0x30))(iVar11 + iVar13 * 4);
                FUN_1000c950(auStack_44,"\t\t\"wave\"\t\"%s\"\n");
                iVar13 = iVar13 + 1;
              } while (iVar13 < (int)pbVar9);
            }
            FUN_1000c950(auStack_44,"\t}\n");
          }
        }
        FUN_1000c950(auStack_44,"}\n");
        pbVar9 = param_1;
        if (iVar4 != iVar3 + -1) {
          FUN_1000c950(auStack_44,"\n");
        }
      }
      iVar4 = iVar4 + 1;
    } while (iVar4 < iVar3);
  }
  iVar3 = (**(code **)(*(int *)(DAT_10021788 + 4) + 8))(pbVar9,&DAT_1001a874,0);
  if (iVar3 == 0) {
    Warning("SceneManager_SaveSoundsToScriptFile:  Unable to write file %s!!!\n",pbVar9);
  }
  else {
    (**(code **)(*(int *)(DAT_10021788 + 4) + 4))(iVar4,uStack_40,iVar3);
    (**(code **)(*(int *)(DAT_10021788 + 4) + 0xc))(iVar3);
    *(undefined1 *)(*(int *)((int)this + 100) + 4 + iStack_1c * 8) = 0;
  }
  if ((-1 < (int)pbStack_48) && (iVar4 != 0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar4);
  }
  return;
}


// 10006110 FUN_10006110

void __fastcall FUN_10006110(void *param_1)

{
  FUN_10004330(param_1);
  *(undefined1 *)((int)param_1 + 0x12) = 0xff;
  if (-1 < *(int *)((int)param_1 + 0xc)) {
    if (*(int *)((int)param_1 + 4) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)param_1 + 4));
      *(undefined4 *)((int)param_1 + 4) = 0;
    }
    *(undefined4 *)((int)param_1 + 8) = 0;
  }
  *(undefined1 *)((int)param_1 + 0x13) = 0xff;
  if (-1 < *(int *)((int)param_1 + 0xc)) {
    if (*(int *)((int)param_1 + 4) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)param_1 + 4));
      *(undefined4 *)((int)param_1 + 4) = 0;
    }
    *(undefined4 *)((int)param_1 + 8) = 0;
  }
  return;
}


// 10006170 FUN_10006170

void __thiscall FUN_10006170(void *this,uint param_1,ushort param_2,char param_3)

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
    FUN_10004580(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(undefined2 *)((uint)param_2 * 0x10 + *(int *)((int)this + 4)) = uVar2;
    FUN_10004580(this,param_1);
    return;
  }
  *(undefined2 *)((uint)param_2 * 0x10 + 2 + *(int *)((int)this + 4)) = uVar2;
  FUN_10004580(this,param_1);
  return;
}


// 100061e0 FUN_100061e0

void __thiscall FUN_100061e0(void *this,ushort param_1)

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
    psVar3 = FUN_10002cf0(this,param_1);
    if ((*psVar3 == -1) || (puVar4 = FUN_10002cf0(this,param_1), puVar4[1] == -1)) {
      uVar8 = (uint)param_1;
    }
    else {
      puVar4 = FUN_10002cf0(this,param_1);
      uVar8 = (uint)(ushort)puVar4[1];
      uVar6 = DAT_100210f0;
      while( true ) {
        if ((uVar6 & 1) == 0) {
          uVar6 = uVar6 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar6;
        }
        if ((short)uVar8 == -1) {
          psVar3 = (short *)&DAT_100210e8;
        }
        else {
          psVar3 = (short *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
        if (*psVar3 == -1) break;
        if ((uVar6 & 1) == 0) {
          uVar6 = uVar6 | 1;
          DAT_100210e8 = 0xffff;
          DAT_100210ea = 0xffff;
          DAT_100210ec = 0xffff;
          DAT_100210ee = 1;
          DAT_100210f0 = uVar6;
        }
        if ((short)uVar8 == -1) {
          uVar8 = (uint)DAT_100210e8;
        }
        else {
          uVar8 = (uint)*(ushort *)(uVar8 * 0x10 + *(int *)((int)this + 4));
        }
      }
    }
    uVar9 = (ushort)uVar8;
    psVar3 = FUN_10002cf0(this,uVar9);
    if (*psVar3 == -1) {
      puVar4 = FUN_10002cf0(this,uVar9);
      uVar1 = puVar4[1];
    }
    else {
      puVar5 = FUN_10002cf0(this,uVar9);
      uVar1 = *puVar5;
    }
    if (uVar1 != 0xffff) {
      puVar4 = FUN_10002cf0(this,uVar9);
      *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar4[2];
    }
    if (uVar9 == *(ushort *)((int)this + 0x10)) {
      *(ushort *)((int)this + 0x10) = uVar1;
    }
    else {
      puVar4 = FUN_10002cf0(this,uVar9);
      puVar5 = FUN_10002cf0(this,puVar4[2]);
      if (*puVar5 == uVar9) {
        puVar4 = FUN_10002cf0(this,uVar9);
        *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
      }
      else {
        puVar4 = FUN_10002cf0(this,uVar9);
        *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
      }
    }
    puVar4 = FUN_10002cf0(this,uVar9);
    sVar2 = puVar4[3];
    if (uVar9 != param_1) {
      puVar4 = FUN_10002cf0(this,param_1);
      iVar7 = uVar8 * 0x10;
      *(undefined2 *)(*(int *)((int)this + 4) + 4 + iVar7) = puVar4[2];
      puVar4 = FUN_10002cf0(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + 2 + iVar7) = puVar4[1];
      puVar4 = FUN_10002cf0(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + iVar7) = *puVar4;
      if (param_1 == *(ushort *)((int)this + 0x10)) {
        *(ushort *)((int)this + 0x10) = uVar9;
      }
      else {
        puVar4 = FUN_10002cf0(this,param_1);
        puVar5 = FUN_10002cf0(this,puVar4[2]);
        if (*puVar5 == param_1) {
          puVar4 = FUN_10002cf0(this,param_1);
          *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + *(int *)((int)this + 4)) = uVar9;
        }
        else {
          puVar4 = FUN_10002cf0(this,param_1);
          *(ushort *)((uint)(ushort)puVar4[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar9;
        }
      }
      psVar3 = FUN_10002cf0(this,uVar9);
      if (*psVar3 != -1) {
        puVar5 = FUN_10002cf0(this,uVar9);
        *(ushort *)((uint)*puVar5 * 0x10 + 4 + *(int *)((int)this + 4)) = uVar9;
      }
      puVar4 = FUN_10002cf0(this,uVar9);
      if (puVar4[1] != -1) {
        puVar4 = FUN_10002cf0(this,uVar9);
        *(ushort *)((uint)(ushort)puVar4[1] * 0x10 + 4 + *(int *)((int)this + 4)) = uVar9;
      }
      puVar4 = FUN_10002cf0(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + 6 + iVar7) = puVar4[3];
    }
    if ((uVar1 != 0xffff) && (sVar2 == 1)) {
      FUN_10004b60(this,(uint)uVar1);
    }
    return;
  }
  return;
}


// 100064b0 FUN_100064b0

void __thiscall FUN_100064b0(void *this,byte param_1,byte param_2,char param_3)

{
  undefined1 *puVar1;
  
  puVar1 = (undefined1 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
  puVar1[2] = param_2;
  puVar1[1] = 0xff;
  *puVar1 = 0xff;
  puVar1[3] = 0;
  if (param_2 == 0xff) {
    *(byte *)((int)this + 0x10) = param_1;
    FUN_100055a0(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(byte *)(*(int *)((int)this + 4) + (uint)param_2 * 0xc) = param_1;
    FUN_100055a0(this,param_1);
    return;
  }
  *(byte *)(*(int *)((int)this + 4) + 1 + (uint)param_2 * 0xc) = param_1;
  FUN_100055a0(this,param_1);
  return;
}


// 10006510 FUN_10006510

void FUN_10006510(int param_1,int param_2,uint param_3)

{
  bool bVar1;
  int iVar2;
  int iVar3;
  undefined1 *puVar4;
  int iVar5;
  int local_14 [5];
  
  iVar5 = param_2;
  if (0 < param_2) {
    iVar3 = 0;
    bVar1 = false;
    local_14[0] = 0;
    local_14[1] = 0;
    local_14[2] = 0;
    local_14[3] = 0;
    local_14[4] = 0;
    param_2 = 0;
    if (0 < iVar5) {
      do {
        iVar2 = param_2;
        if ((*(byte *)(param_1 + 2 + param_2 * 4) == param_3) &&
           (bVar1 = true, *(char *)(param_1 + 3 + param_2 * 4) != '\0')) {
          FUN_100041d0(local_14,iVar3,&param_2);
          iVar3 = local_14[3];
        }
        param_2 = iVar2 + 1;
      } while (param_2 < iVar5);
      if ((iVar3 == 0) && (bVar1)) {
        puVar4 = (undefined1 *)(param_1 + 3);
        do {
          if ((byte)puVar4[-1] == param_3) {
            *puVar4 = 1;
          }
          puVar4 = puVar4 + 4;
          iVar5 = iVar5 + -1;
        } while (iVar5 != 0);
      }
    }
    if ((-1 < local_14[2]) && (local_14[0] != 0)) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_14[0]);
    }
  }
  return;
}


// 100065d0 FUN_100065d0

undefined4 FUN_100065d0(int param_1,int param_2,uint param_3)

{
  uint uVar1;
  uint uVar2;
  int iVar3;
  undefined4 uVar4;
  int unaff_EBP;
  undefined4 local_14;
  undefined4 local_10;
  undefined4 local_c;
  int local_8;
  undefined4 local_4;
  
  uVar1 = param_3;
  FUN_10006510(param_1,param_2,param_3);
  if (param_2 < 1) {
    return 0xffffffff;
  }
  iVar3 = 0;
  local_14 = 0;
  local_10 = 0;
  local_c = 0;
  local_8 = 0;
  local_4 = 0;
  param_3 = 0;
  if (0 < param_2) {
    do {
      uVar2 = param_3;
      if ((*(byte *)(param_1 + 2 + param_3 * 4) == uVar1) &&
         (*(char *)(param_1 + 3 + param_3 * 4) != '\0')) {
        FUN_100041d0(&local_14,iVar3,&param_3);
        iVar3 = local_8;
      }
      param_3 = uVar2 + 1;
    } while ((int)param_3 < param_2);
    if (0 < iVar3) {
      iVar3 = (**(code **)(*(int *)PTR_PTR_1001f014 + 8))(0,iVar3 + -1);
      uVar4 = *(undefined4 *)(unaff_EBP + iVar3 * 4);
      FUN_10004020((int *)&stack0xffffffe4);
      return uVar4;
    }
  }
  uVar4 = (**(code **)(*(int *)PTR_PTR_1001f014 + 8))(0,param_2 + -1);
  FUN_10004020((int *)&stack0xffffffe4);
  return uVar4;
}


// 10006990 FUN_10006990

undefined4 __thiscall FUN_10006990(void *this,undefined4 param_1)

{
  ushort uVar1;
  undefined4 uVar2;
  undefined4 local_8 [2];
  
  local_8[0] = param_1;
  uVar1 = FUN_10003f20((void *)((int)this + 0xb8),local_8);
  if (uVar1 == 0xffff) {
    return 0;
  }
  uVar2 = FUN_1000a660(*(ushort **)((uint)uVar1 * 0x10 + 0xc + *(int *)((int)this + 0xbc)));
  return uVar2;
}


// 100069e0 FUN_100069e0

undefined4 __thiscall FUN_100069e0(void *this,undefined4 param_1)

{
  ushort uVar1;
  undefined2 extraout_var;
  undefined4 local_8 [2];
  
  local_8[0] = param_1;
  uVar1 = FUN_10003f20((void *)((int)this + 0xb8),local_8);
  return CONCAT31((int3)(CONCAT22(extraout_var,uVar1) >> 8),uVar1 != 0xffff);
}


// 10006a10 FUN_10006a10

void __fastcall FUN_10006a10(void *param_1)

{
  FUN_10004100(param_1);
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


// 10006a70 FUN_10006a70

void __fastcall FUN_10006a70(void *param_1)

{
  byte bVar1;
  uint local_4;
  
  bVar1 = FUN_10003580((int)param_1);
  local_4._1_3_ = (undefined3)((uint)param_1 >> 8);
  local_4 = CONCAT31(local_4._1_3_,bVar1);
  while (bVar1 != 0xff) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)(*(int *)((int)param_1 + 4) + 4 + (uint)bVar1 * 0xc));
    bVar1 = FUN_100043e0(param_1,local_4);
    local_4._1_3_ = (undefined3)(local_4 >> 8);
    local_4 = CONCAT31(local_4._1_3_,bVar1);
  }
  FUN_10004330(param_1);
  return;
}


// 10006b30 FUN_10006b30

undefined4 __thiscall FUN_10006b30(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  ushort uVar3;
  void *local_4;
  
  puVar2 = param_1;
  local_4 = this;
  FUN_100033c0(this,param_1,(ushort *)&local_4,(undefined1 *)&param_1);
  uVar3 = FUN_10002bd0((int)this);
  FUN_10006170(this,(uint)uVar3,(ushort)local_4,(char)param_1);
  *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + 1;
  puVar1 = (undefined4 *)((uint)uVar3 * 0x10 + 8 + *(int *)((int)this + 4));
  if (puVar1 != (undefined4 *)0x0) {
    *puVar1 = *puVar2;
    puVar1[1] = puVar2[1];
  }
  return CONCAT22((short)((uint)puVar1 >> 0x10),uVar3);
}


// 10006ba0 FUN_10006ba0

undefined4 __thiscall FUN_10006ba0(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  byte bVar3;
  byte local_8 [4];
  byte local_4;
  undefined3 uStack_3;
  
  puVar2 = param_1;
  FUN_10003620(this,param_1,local_8,(undefined1 *)&param_1);
  bVar3 = FUN_10003770((int)this);
  _local_4 = CONCAT31(uStack_3,bVar3);
  FUN_100064b0(this,bVar3,local_8[0],(char)param_1);
  *(char *)((int)this + 0x11) = *(char *)((int)this + 0x11) + '\x01';
  puVar1 = (undefined4 *)(*(int *)((int)this + 4) + 4 + (uint)bVar3 * 0xc);
  if (puVar1 != (undefined4 *)0x0) {
    *puVar1 = *puVar2;
    puVar1[1] = puVar2[1];
  }
  return CONCAT31((int3)((uint)puVar1 >> 8),bVar3);
}


// 10006d70 FUN_10006d70

uint __thiscall FUN_10006d70(void *this,undefined4 *param_1)

{
  ushort uVar1;
  undefined2 extraout_var;
  int iVar2;
  undefined4 local_8 [2];
  
  local_8[0] = *param_1;
  uVar1 = FUN_10003f20(this,local_8);
  if (uVar1 != 0xffff) {
    FUN_100061e0(this,uVar1);
    iVar2 = (uint)uVar1 * 0x10;
    *(ushort *)(iVar2 + *(int *)((int)this + 4)) = uVar1;
    *(undefined2 *)(*(int *)((int)this + 4) + 2 + iVar2) = *(undefined2 *)((int)this + 0x14);
    *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + -1;
    *(ushort *)((int)this + 0x14) = uVar1;
    return CONCAT31((int3)((uint)iVar2 >> 8),1);
  }
  return CONCAT22(extraout_var,uVar1) & 0xffffff00;
}


// 10006de0 FUN_10006de0

void __fastcall FUN_10006de0(undefined4 *param_1)

{
  *param_1 = &PTR_LAB_1001aab4;
  FUN_10006a10(param_1 + 0x2e);
  FUN_1000a380(param_1 + 0x20);
  FUN_10004020(param_1 + 0x19);
  FUN_10004020(param_1 + 0x14);
  FUN_10004020(param_1 + 0xf);
  FUN_10004020(param_1 + 8);
  FUN_10006a70(param_1 + 2);
  FUN_10006110(param_1 + 2);
  return;
}


// 10006e40 FUN_10006e40

undefined4 * __thiscall FUN_10006e40(void *this,byte param_1)

{
  FUN_10006de0((undefined4 *)this);
  if ((param_1 & 1) != 0) {
    FUN_100012c0(this);
  }
  return (undefined4 *)this;
}


// 10006e60 FUN_10006e60

undefined4 * __fastcall FUN_10006e60(undefined4 *param_1)

{
  *(undefined1 *)(param_1 + 1) = 1;
  *param_1 = &PTR_LAB_1001aab4;
  param_1[2] = 0;
  param_1[3] = 0;
  param_1[4] = 0;
  param_1[5] = 0;
  *(undefined1 *)(param_1 + 6) = 0xff;
  *(undefined1 *)((int)param_1 + 0x1a) = 0xff;
  *(undefined1 *)((int)param_1 + 0x19) = 0;
  *(undefined1 *)((int)param_1 + 0x1b) = 0xff;
  param_1[7] = param_1[3];
  if (param_1[2] == 0) {
    param_1[2] = &LAB_10001340;
  }
  param_1[8] = 0;
  param_1[9] = 0;
  param_1[10] = 0;
  param_1[0xb] = 0;
  param_1[0xc] = 0;
  param_1[0xd] = 0;
  *(undefined1 *)(param_1 + 0xe) = 0;
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
  FUN_1000a400(param_1 + 0x20,0,0x10,0);
  param_1[0x2e] = &LAB_10002160;
  param_1[0x2f] = 0;
  param_1[0x30] = 0;
  param_1[0x31] = 0;
  *(undefined2 *)(param_1 + 0x32) = 0xffff;
  *(undefined2 *)((int)param_1 + 0xca) = 0;
  *(undefined2 *)(param_1 + 0x33) = 0xffff;
  *(undefined2 *)((int)param_1 + 0xce) = 0xffff;
  param_1[0x34] = param_1[0x2f];
  return param_1;
}


// 10006f50 FUN_10006f50

void __thiscall FUN_10006f50(void *this,undefined4 param_1)

{
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0xb0))(param_1);
  FUN_10006b30((void *)((int)this + 0xb8),(undefined4 *)&stack0xfffffff4);
  return;
}


// 10006fa0 FUN_10006fa0

void __thiscall FUN_10006fa0(void *this,undefined4 param_1)

{
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0xb0))(param_1);
  FUN_10006d70((void *)((int)this + 0xb8),(undefined4 *)&stack0x00000000);
  return;
}


// 10006fd0 FUN_10006fd0

void __fastcall FUN_10006fd0(int param_1)

{
  char cVar1;
  byte bVar2;
  void *this;
  undefined4 *this_00;
  int *piVar3;
  undefined4 *this_01;
  undefined1 *puVar4;
  byte *pbVar5;
  uint uVar6;
  char *pcVar7;
  undefined4 uVar8;
  byte *pbVar9;
  int local_18;
  undefined4 local_14;
  undefined1 *local_10 [3];
  
  this = (void *)FUN_1000ac30(0x24);
  if (this == (void *)0x0) {
    this_00 = (undefined4 *)0x0;
  }
  else {
    this_00 = FUN_1000ac50(this,"allactors");
  }
  if (DAT_10021788 == 0) {
    piVar3 = (int *)0x0;
  }
  else {
    piVar3 = (int *)(DAT_10021788 + 4);
  }
  cVar1 = FUN_1000b810(this_00,piVar3,"scripts/global_actors.txt");
  if (cVar1 != '\0') {
    this_01 = (undefined4 *)FUN_1000aab0((int)this_00);
    if (this_01 != (undefined4 *)0x0) {
      do {
        puVar4 = FUN_1000a8a0(this_01);
        if (puVar4 == (undefined1 *)0x0) {
LAB_1000705b:
          if (*(char *)(param_1 + 0x19) == -2) {
            Warning("Exceeded max number of actors in scripts/global_actors.txt\n");
            break;
          }
          pbVar9 = &DAT_1001a244;
          uVar8 = 0;
          pbVar5 = (byte *)FUN_1000b210(this_01,(char *)0x0,0x1001a66b);
          uVar6 = FUN_10008b10(pbVar5,pbVar9);
          if (uVar6 == 0) {
            uVar8 = 1;
          }
          else {
            pcVar7 = "female";
            pbVar5 = (byte *)FUN_1000b210(this_01,(char *)0x0,0x1001a66b);
            uVar6 = FUN_10008b10(pbVar5,(byte *)pcVar7);
            if (uVar6 == 0) {
              uVar8 = 2;
            }
            else {
              pcVar7 = "namvet";
              pbVar5 = (byte *)FUN_1000b210(this_01,(char *)0x0,0x1001a66b);
              uVar6 = FUN_10008b10(pbVar5,(byte *)pcVar7);
              if (uVar6 == 0) {
                uVar8 = 3;
              }
              else {
                pcVar7 = "teengirl";
                pbVar5 = (byte *)FUN_1000b210(this_01,(char *)0x0,0x1001a66b);
                uVar6 = FUN_10008b10(pbVar5,(byte *)pcVar7);
                if (uVar6 == 0) {
                  uVar8 = 4;
                }
                else {
                  pcVar7 = "biker";
                  pbVar5 = (byte *)FUN_1000b210(this_01,(char *)0x0,0x1001a66b);
                  uVar6 = FUN_10008b10(pbVar5,(byte *)pcVar7);
                  if (uVar6 == 0) {
                    uVar8 = 5;
                  }
                  else {
                    pcVar7 = "manager";
                    pbVar5 = (byte *)FUN_1000b210(this_01,(char *)0x0,0x1001a66b);
                    uVar6 = FUN_10008b10(pbVar5,(byte *)pcVar7);
                    if (uVar6 == 0) {
                      uVar8 = 6;
                    }
                  }
                }
              }
            }
          }
          pcVar7 = FUN_1000a8a0(this_01);
          local_18 = FUN_100012e0(pcVar7);
          local_14 = uVar8;
          FUN_10006ba0((void *)(param_1 + 8),&local_18);
        }
        else {
          local_10[0] = puVar4;
          bVar2 = FUN_10004240((void *)(param_1 + 8),local_10);
          if (bVar2 == 0xff) goto LAB_1000705b;
        }
        this_01 = (undefined4 *)FUN_1000aac0((int)this_01);
        if (this_01 == (undefined4 *)0x0) {
          FUN_1000b0a0((int)this_00);
          return;
        }
      } while( true );
    }
  }
  FUN_1000b0a0((int)this_00);
  return;
}


// 100071c0 FUN_100071c0

void __thiscall FUN_100071c0(void *this,undefined4 param_1)

{
  int *this_00;
  char cVar1;
  int iVar2;
  void *pvVar3;
  int iVar4;
  ushort *puVar5;
  char *pcVar6;
  byte *pbVar7;
  undefined1 *puVar8;
  undefined4 uVar9;
  undefined4 *puVar10;
  int unaff_EBX;
  undefined4 *unaff_ESI;
  undefined4 *puVar11;
  ushort *puVar12;
  undefined4 uStack_44;
  int iStack_30;
  undefined4 *puStack_2c;
  int iStack_28;
  undefined4 uStack_1c;
  int iStack_18;
  undefined4 uStack_14;
  uint uStack_10;
  char cStack_c;
  char cStack_8;
  
  uStack_44 = param_1;
  uStack_14 = (**(code **)(*DAT_10021788 + 0x94))();
  uStack_10 = uStack_10 & 0xffffff00;
  iVar2 = FUN_10004090((int *)((int)this + 100),*(int *)((int)this + 0x70),&uStack_14);
  uStack_1c = 0;
  iStack_18 = iVar2;
  pvVar3 = (void *)FUN_1000ac30(0x24);
  if (pvVar3 == (void *)0x0) {
    puStack_2c = (undefined4 *)0x0;
  }
  else {
    puStack_2c = FUN_1000ac50(pvVar3,&DAT_1001a66b);
  }
  cVar1 = (**(code **)(*DAT_10021788 + 0x130))(puStack_2c,1,param_1,&DAT_1001a220);
  if (cVar1 == '\0') {
    if (cStack_8 == '\0') {
      Warning("CSoundEmitterSystem::AddSoundsFromFile:  No such file %s\n",param_1);
    }
    iVar4 = (*(int *)((int)this + 0x70) - iVar2) + -1;
    if (0 < iVar4) {
      pvVar3 = (void *)(*(int *)((int)this + 100) + iVar2 * 8);
      FUN_10008eb0(pvVar3,(void *)((int)pvVar3 + 8),iVar4 * 8);
    }
    *(int *)((int)this + 0x70) = *(int *)((int)this + 0x70) + -1;
    FUN_1000b0a0((int)unaff_ESI);
  }
  else {
    puVar10 = unaff_ESI;
    if (unaff_ESI != (undefined4 *)0x0) {
      do {
        iVar4 = FUN_1000aab0((int)puVar10);
        if (iVar4 != 0) {
          if (*(int *)((int)this + 0x2c) == -2) {
            Warning("Exceeded maximum number of sound emitter entries\n");
            break;
          }
          puVar5 = (ushort *)FUN_10002080(&DAT_10021548);
          if (puVar5 == (ushort *)0x0) {
            puVar5 = (ushort *)0x0;
          }
          else {
            *puVar5 = 0xffff;
            FUN_100085f0((undefined4 *)(puVar5 + 1));
          }
          pcVar6 = FUN_1000a8a0(puVar10);
          iVar4 = FUN_10008a80(pcVar6);
          if (iVar4 == 0) {
            Error("Syntax Error! Empty named KV block in %s\n",uStack_10);
          }
          pbVar7 = FUN_1000a8a0(puVar10);
          FUN_1000a610((void *)((int)&uStack_44 + 2),pbVar7);
          *puVar5 = uStack_44._2_2_;
          *(byte *)(puVar5 + 0x12) = (byte)puVar5[0x12] & 0xfe;
          puVar5[0x11] = (ushort)iVar2;
          *(byte *)(puVar5 + 0x12) =
               (byte)puVar5[0x12] ^ (cStack_8 * '\x02' ^ (byte)puVar5[0x12]) & 2;
          if (cStack_8 != '\0') {
            puStack_2c = (undefined4 *)((int)puStack_2c + 1);
          }
          this_00 = (int *)((int)this + 0x20);
          iVar2 = FUN_10003270(this_00,(undefined4 *)&stack0xffffffc8);
          if (iVar2 == -1) {
            puVar12 = puVar5 + 1;
            puVar11 = puVar10;
            puVar8 = FUN_1000a8a0(puVar10);
            FUN_100023f0(this,puVar8,(int)puVar11,puVar12);
            *(byte *)(puVar5 + 0xe) = (byte)puVar5[0xe] ^ (cStack_c * '\b' ^ (byte)puVar5[0xe]) & 8;
            iVar2 = FUN_10003e00(this_00,(undefined4 *)&stack0xffffffc8);
            uVar9 = FUN_1000a660(puVar5);
            iVar2 = *(int *)(*this_00 + iVar2 * 4);
                    /* WARNING: Load size is inaccurate */
            uStack_1c = (**(code **)(*this + 0xb0))(uVar9);
            iStack_18 = iVar2;
            FUN_10006b30((void *)((int)this + 0xb8),&uStack_1c);
          }
          else if (cStack_8 == '\0') {
            FUN_10007fe0((undefined4 *)(puVar5 + 1));
            if ((puVar5 < DAT_10021550) || (DAT_10021554 <= puVar5)) {
              FUN_100012c0(puVar5);
            }
            else {
              FUN_100014b0(&DAT_10021548,(undefined4 *)puVar5);
            }
          }
          else {
            puVar12 = *(ushort **)(*this_00 + iVar2 * 4);
            if ((puVar12[0x12] & 2) == 0) {
              FUN_100041d0((void *)((int)this + 0x3c),*(int *)((int)this + 0x48),
                           (undefined4 *)(*this_00 + iVar2 * 4));
            }
            else {
              if (puVar12 != (ushort *)0x0) {
                FUN_10007fe0((undefined4 *)(puVar12 + 1));
                if ((puVar12 < DAT_10021550) || (DAT_10021554 <= puVar12)) {
                  FUN_100012c0(puVar12);
                }
                else {
                  FUN_100014b0(&DAT_10021548,(undefined4 *)puVar12);
                }
              }
              unaff_EBX = unaff_EBX + 1;
            }
            puVar12 = puVar5 + 1;
            puVar11 = puVar10;
            puVar8 = FUN_1000a8a0(puVar10);
            FUN_100023f0(this,puVar8,(int)puVar11,puVar12);
            *(byte *)(puVar5 + 0xe) = (byte)puVar5[0xe] ^ (cStack_c * '\b' ^ (byte)puVar5[0xe]) & 8;
            iStack_30 = iStack_30 + 1;
            *(ushort **)(*this_00 + iVar2 * 4) = puVar5;
          }
        }
        puVar10 = (undefined4 *)FUN_1000aac0((int)puVar10);
        iVar2 = iStack_28;
      } while (puVar10 != (undefined4 *)0x0);
    }
    FUN_1000b0a0((int)unaff_ESI);
    if (cStack_8 != '\0') {
      Warning("SoundEmitter:  adding map sound overrides from %s [%i total, %i replacements, %i duplicated replacements]\n"
              ,uStack_10,puStack_2c,iStack_30,unaff_EBX);
      return;
    }
  }
  return;
}


// 10007ab0 FUN_10007ab0

undefined4 __fastcall FUN_10007ab0(void *param_1)

{
  char cVar1;
  undefined4 in_EAX;
  void *this;
  undefined4 *puVar2;
  uint uVar3;
  undefined4 *this_00;
  byte *pbVar4;
  uint *puVar5;
  int iVar6;
  undefined1 *puVar7;
  char *pcVar8;
  undefined *puVar9;
  undefined4 local_c [3];
  
  *(int *)((int)param_1 + 0x78) = *(int *)((int)param_1 + 0x78) + 1;
  if (*(int *)((int)param_1 + 0x78) < 2) {
    FUN_10006fd0((int)param_1);
    *(undefined4 *)((int)param_1 + 0x7c) = 0;
    FUN_100090e0(local_c);
    this = (void *)FUN_1000ac30(0x24);
    if (this == (void *)0x0) {
      puVar2 = (undefined4 *)0x0;
    }
    else {
      puVar2 = FUN_1000ac50(this,"scripts/game_sounds_manifest.txt");
    }
    puVar9 = &DAT_1001a220;
    cVar1 = (**(code **)(*DAT_10021788 + 0x130))(puVar2,1,"scripts/game_sounds_manifest.txt");
    if (cVar1 == '\0') {
      Error("Unable to load manifest file \'%s\'\n","scripts/game_sounds_manifest.txt");
    }
    else {
      (**(code **)(DAT_10021788[1] + 0x34))("scripts/game_sounds_manifest.txt",&DAT_1001a220);
      FUN_10009100((uint *)&stack0xffffffe4,(uint *)&stack0xffffffe8,4);
      uVar3 = FUN_10008a80("scripts/game_sounds_manifest.txt");
      FUN_10009100((uint *)&stack0xffffffe4,(uint *)"scripts/game_sounds_manifest.txt",uVar3);
      for (this_00 = (undefined4 *)FUN_1000aab0((int)puVar2); this_00 != (undefined4 *)0x0;
          this_00 = (undefined4 *)FUN_1000aac0((int)this_00)) {
        pcVar8 = "precache_file";
        pbVar4 = FUN_1000a8a0(this_00);
        uVar3 = FUN_10008b10(pbVar4,(byte *)pcVar8);
        if (uVar3 == 0) {
          puVar5 = (uint *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
          (**(code **)(DAT_10021788[1] + 0x34))(puVar5,&DAT_1001a220);
          FUN_10009100((uint *)&stack0xffffffe4,(uint *)&stack0xffffffe8,4);
          uVar3 = FUN_10008a80((char *)puVar5);
          FUN_10009100((uint *)&stack0xffffffe4,puVar5,uVar3);
LAB_10007bef:
          iVar6 = FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
          FUN_100071c0(param_1,iVar6);
        }
        else {
          pcVar8 = "preload_file";
          pbVar4 = FUN_1000a8a0(this_00);
          uVar3 = FUN_10008b10(pbVar4,(byte *)pcVar8);
          if (uVar3 == 0) {
            puVar5 = (uint *)FUN_1000b210(this_00,(char *)0x0,0x1001a66b);
            (**(code **)(DAT_10021788[1] + 0x34))(puVar5,&DAT_1001a220);
            FUN_10009100((uint *)&stack0xffffffe4,(uint *)&stack0xffffffec,4);
            uVar3 = FUN_10008a80((char *)puVar5);
            FUN_10009100((uint *)&stack0xffffffe4,puVar5,uVar3);
            goto LAB_10007bef;
          }
          puVar7 = FUN_1000a8a0(this_00);
          Warning("CSoundEmitterSystemBase::BaseInit:  Manifest \'%s\' with bogus file type \'%s\', expecting \'declare_file\' or \'precache_file\'\n"
                  ,"scripts/game_sounds_manifest.txt",puVar7);
        }
      }
    }
    FUN_1000b0a0((int)puVar2);
    FUN_100090f0((uint *)&stack0xffffffe4);
    *(undefined **)((int)param_1 + 0x7c) = puVar9;
    in_EAX = DevMsg(1,"CSoundEmitterSystem:  Registered %i sounds\n",
                    *(undefined4 *)((int)param_1 + 0x2c));
  }
  return CONCAT31((int3)((uint)in_EAX >> 8),1);
}


// 10007cf0 FUN_10007cf0

uint __cdecl FUN_10007cf0(float param_1)

{
  int iVar1;
  float fVar2;
  char cVar3;
  uint uVar4;
  uint uVar5;
  uint uVar6;
  
  fVar2 = 65504.0;
  if ((param_1 <= 65504.0) && (fVar2 = param_1, param_1 < -65504.0)) {
    fVar2 = -65504.0;
  }
  uVar5 = (uint)fVar2 & 0x7fff;
  uVar4 = ((uint)fVar2 >> 0x1f) << 0xf;
  uVar6 = (uint)fVar2 >> 0x17 & 0xff;
  if (((uVar6 == 0) && (((uint)fVar2 & 0x7fffff) == 0)) ||
     ((uVar6 == 0 && (((uint)fVar2 & 0x7fffff) != 0)))) {
    return uVar4;
  }
  if (uVar6 == 0xff) {
    if (((uint)fVar2 & 0x7fffff) == 0) {
      return uVar4 | (uint)fVar2 & 0x7bff | 0x7bff;
    }
    if (((uint)fVar2 & 0x7fffff) != 0) {
      return uVar4;
    }
  }
  iVar1 = uVar6 - 0x7f;
  if (iVar1 < -0x18) {
    uVar5 = 0;
  }
  if (iVar1 < -0xe) {
    cVar3 = (char)-uVar6;
    uVar4 = uVar4 | uVar5 & 0xffff83ff;
    if (-uVar6 + 0x70 < 10) {
      return (uint)(ushort)((ushort)uVar4 ^
                           ((short)(((uint)fVar2 & 0x7fffff) >> (cVar3 + 0x7eU & 0x1f)) +
                            (short)(1 << (10U - (cVar3 + 'q') & 0x1f)) ^ (ushort)uVar4) & 0x3ff);
    }
  }
  else {
    if (0xf < iVar1) {
      return uVar4 | uVar5 & 0xfbff | 0x7bff;
    }
    uVar4 = (uint)fVar2 >> 0xd & 0x3ff | (uVar6 - 0x70 & 0x1f) << 10 | uVar4;
  }
  return uVar4;
}


// 10007e30 FUN_10007e30

undefined4 * __fastcall FUN_10007e30(undefined4 *param_1)

{
  if (*(short *)(param_1 + 2) != 1) {
    param_1 = (undefined4 *)*param_1;
  }
  return param_1;
}


// 10007e40 FUN_10007e40

undefined * __cdecl FUN_10007e40(float param_1)

{
  float *pfVar1;
  
  pfVar1 = (float *)&DAT_1001f218;
  do {
    if (param_1 == *pfVar1) {
      return (undefined *)pfVar1[1];
    }
    pfVar1 = pfVar1 + 2;
  } while ((int)pfVar1 < 0x1001f220);
  FUN_10008f20(&DAT_10021688,0x20,"%.3f");
  return &DAT_10021688;
}


// 10007e90 FUN_10007e90

undefined * __cdecl FUN_10007e90(float param_1)

{
  float *pfVar1;
  
  pfVar1 = (float *)&DAT_1001f220;
  do {
    if (param_1 == *pfVar1) {
      return (undefined *)pfVar1[1];
    }
    pfVar1 = pfVar1 + 2;
  } while ((int)pfVar1 < 0x1001f238);
  FUN_10008f20(&DAT_100216a8,0x20,"%.3f");
  return &DAT_100216a8;
}


// 10007ee0 FUN_10007ee0

long __cdecl FUN_10007ee0(byte *param_1)

{
  int iVar1;
  long lVar2;
  long *plVar3;
  
  if (param_1 == (byte *)0x0) {
    return 0x4b;
  }
  plVar3 = &DAT_1001f238;
  do {
    iVar1 = FUN_10008c70(param_1,(byte *)plVar3[1]);
    if (iVar1 == 0) {
      return *plVar3;
    }
    plVar3 = plVar3 + 2;
  } while ((int)plVar3 < 0x1001f328);
  iVar1 = FUN_10008a80("SNDLVL_");
  iVar1 = FUN_10008ca0((char *)param_1,"SNDLVL_",iVar1);
  if (iVar1 == 0) {
    iVar1 = FUN_10008a80("SNDLVL_");
    lVar2 = _atol((char *)(param_1 + iVar1));
    if (lVar2 - 1U < 0xb4) {
      return lVar2;
    }
  }
  DevMsg("CSoundEmitterSystem:  Unknown sound level %s\n",param_1);
  return 0x4b;
}


// 10007f70 FUN_10007f70

long __cdecl FUN_10007f70(byte *param_1)

{
  int iVar1;
  long lVar2;
  int iVar3;
  
  if (param_1 == (byte *)0x0) {
    return 0;
  }
  iVar1 = FUN_10008bf0((char *)param_1,"chan_",5);
  if (iVar1 != 0) {
    lVar2 = _atol((char *)param_1);
    return lVar2;
  }
  iVar1 = 0;
  do {
    iVar3 = FUN_10008c70(param_1,(&PTR_s_CHAN_AUTO_1001f1e4)[iVar1 * 2]);
    if (iVar3 == 0) {
      return (&DAT_1001f1e0)[iVar1 * 2];
    }
    iVar1 = iVar1 + 1;
  } while (iVar1 < 7);
  DevMsg("CSoundEmitterSystem:  Warning, unknown channel type in sounds.txt (%s)\n",param_1);
  return 0;
}


// 10007fe0 FUN_10007fe0

void __fastcall FUN_10007fe0(undefined4 *param_1)

{
  if (1 < *(ushort *)(param_1 + 2)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
  }
  if (1 < *(ushort *)((int)param_1 + 10)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1[1]);
  }
  param_1[1] = 0;
  *param_1 = 0;
  *(undefined2 *)(param_1 + 2) = 0;
  *(undefined2 *)((int)param_1 + 10) = 0;
  return;
}


// 10008030 FUN_10008030

void __thiscall FUN_10008030(void *this,void *param_1)

{
  void *this_00;
  undefined4 *puVar1;
  
  if (*(int *)((int)this + 0x1c) != 0) {
    FUN_1000b0a0(*(int *)((int)this + 0x1c));
  }
  *(undefined4 *)((int)this + 0x1c) = 0;
  this_00 = (void *)FUN_1000ac30(0x24);
  if (this_00 != (void *)0x0) {
    puVar1 = FUN_1000ac50(this_00,"GameData");
    *(undefined4 **)((int)this + 0x1c) = puVar1;
    FUN_1000aad0(param_1,(int)puVar1);
    return;
  }
  *(undefined4 *)((int)this + 0x1c) = 0;
  FUN_1000aad0(param_1,0);
  return;
}


// 10008090 FUN_10008090

void __thiscall FUN_10008090(void *this,undefined4 *param_1)

{
  ushort uVar1;
  void *pvVar2;
  
  if (1 < *(ushort *)((int)this + 8)) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*this);
  }
  if (1 < *(ushort *)((int)this + 10)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(undefined4 *)((int)this + 4));
  }
  *(undefined2 *)((int)this + 0x16) = *(undefined2 *)((int)param_1 + 0x16);
  *(undefined4 *)((int)this + 0xc) = param_1[3];
  *(undefined2 *)((int)this + 0x14) = *(undefined2 *)(param_1 + 5);
  *(undefined4 *)((int)this + 0x10) = param_1[4];
  *(undefined2 *)((int)this + 0x18) = *(undefined2 *)(param_1 + 6);
  *(byte *)((int)this + 0x1a) =
       *(byte *)((int)this + 0x1a) ^
       (*(byte *)((int)this + 0x1a) ^ *(byte *)((int)param_1 + 0x1a)) & 1;
  uVar1 = *(ushort *)(param_1 + 2);
  *(ushort *)((int)this + 8) = uVar1;
  if (uVar1 == 0) {
    *(undefined4 *)this = 0;
  }
  else if (uVar1 < 2) {
    *(undefined4 *)this = *param_1;
  }
  else {
    pvVar2 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))((uint)uVar1 * 4);
    *(void **)this = pvVar2;
    _memcpy(pvVar2,(void *)*param_1,(uint)*(ushort *)((int)this + 8) * 4);
  }
  uVar1 = *(ushort *)((int)param_1 + 10);
  *(ushort *)((int)this + 10) = uVar1;
  if (uVar1 == 0) {
    *(undefined4 *)((int)this + 4) = 0;
  }
  else if (uVar1 < 2) {
    *(undefined4 *)((int)this + 4) = param_1[1];
  }
  else {
    pvVar2 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))((uint)uVar1 * 4);
    *(void **)((int)this + 4) = pvVar2;
    _memcpy(pvVar2,(void *)param_1[1],(uint)*(ushort *)((int)this + 10) * 4);
  }
  if ((void *)param_1[7] != (void *)0x0) {
    FUN_10008030(this,(void *)param_1[7]);
  }
  *(byte *)((int)this + 0x1a) =
       *(byte *)((int)this + 0x1a) ^
       (*(byte *)((int)this + 0x1a) ^ *(byte *)((int)param_1 + 0x1a)) & 2;
  *(byte *)((int)this + 0x1a) =
       (*(byte *)((int)param_1 + 0x1a) ^ *(byte *)((int)this + 0x1a)) & 4 ^
       *(byte *)((int)this + 0x1a);
  return;
}


// 100081d0 FUN_100081d0

undefined4 __thiscall FUN_100081d0(void *this,undefined4 *param_1)

{
  ushort uVar1;
  undefined4 in_EAX;
  byte *pbVar2;
  undefined4 *puVar3;
  undefined4 *puVar4;
  byte *pbVar5;
  uint uVar6;
  int iVar7;
  
  if ((undefined4 *)this == param_1) {
    return CONCAT31((int3)((uint)in_EAX >> 8),1);
  }
  if (*(short *)((int)this + 0x16) != *(short *)((int)param_1 + 0x16)) {
    return CONCAT22((short)((uint)in_EAX >> 0x10),*(short *)((int)this + 0x16)) & 0xffffff00;
  }
  uVar6 = 4;
  pbVar2 = (byte *)(param_1 + 3);
  pbVar5 = (byte *)((int)this + 0xc);
  do {
    if (*(int *)pbVar5 != *(int *)pbVar2) goto LAB_10008219;
    uVar6 = uVar6 - 4;
    pbVar2 = pbVar2 + 4;
    pbVar5 = pbVar5 + 4;
  } while (3 < uVar6);
  if (uVar6 == 0) {
LAB_10008276:
    uVar6 = 0;
  }
  else {
LAB_10008219:
    iVar7 = (uint)*pbVar5 - (uint)*pbVar2;
    if (iVar7 == 0) {
      if (uVar6 == 1) goto LAB_10008276;
      iVar7 = (uint)pbVar5[1] - (uint)pbVar2[1];
      if (iVar7 == 0) {
        if (uVar6 == 2) goto LAB_10008276;
        iVar7 = (uint)pbVar5[2] - (uint)pbVar2[2];
        if (iVar7 == 0) {
          if ((uVar6 == 3) || (iVar7 = (uint)pbVar5[3] - (uint)pbVar2[3], iVar7 == 0))
          goto LAB_10008276;
        }
      }
    }
    uVar6 = 1;
    if (iVar7 < 1) {
      uVar6 = 0xffffffff;
    }
  }
  if (uVar6 != 0) goto LAB_1000839f;
  uVar6 = (uint)*(byte *)((int)this + 0x14) - (uint)*(byte *)(param_1 + 5);
  if ((uVar6 == 0) &&
     (uVar6 = (uint)*(byte *)((int)this + 0x15) - (uint)*(byte *)((int)param_1 + 0x15), uVar6 == 0))
  {
    iVar7 = 0;
  }
  else {
    iVar7 = 1;
    if ((int)uVar6 < 1) {
      iVar7 = -1;
    }
  }
  if (iVar7 != 0) goto LAB_1000839f;
  uVar6 = 4;
  pbVar2 = (byte *)(param_1 + 4);
  pbVar5 = (byte *)((int)this + 0x10);
  do {
    if (*(int *)pbVar5 != *(int *)pbVar2) goto LAB_100082dc;
    uVar6 = uVar6 - 4;
    pbVar2 = pbVar2 + 4;
    pbVar5 = pbVar5 + 4;
  } while (3 < uVar6);
  if (uVar6 == 0) {
LAB_10008339:
    uVar6 = 0;
  }
  else {
LAB_100082dc:
    iVar7 = (uint)*pbVar5 - (uint)*pbVar2;
    if (iVar7 == 0) {
      if (uVar6 == 1) goto LAB_10008339;
      iVar7 = (uint)pbVar5[1] - (uint)pbVar2[1];
      if (iVar7 == 0) {
        if (uVar6 == 2) goto LAB_10008339;
        iVar7 = (uint)pbVar5[2] - (uint)pbVar2[2];
        if (iVar7 == 0) {
          if ((uVar6 == 3) || (iVar7 = (uint)pbVar5[3] - (uint)pbVar2[3], iVar7 == 0))
          goto LAB_10008339;
        }
      }
    }
    uVar6 = 1;
    if (iVar7 < 1) {
      uVar6 = 0xffffffff;
    }
  }
  if (((uVar6 == 0) && (*(short *)((int)this + 0x18) == *(short *)(param_1 + 6))) &&
     (((*(byte *)((int)param_1 + 0x1a) ^ *(byte *)((int)this + 0x1a)) & 1) == 0)) {
    uVar1 = *(ushort *)((int)this + 8);
    uVar6 = (uint)uVar1;
    if (uVar1 == *(ushort *)(param_1 + 2)) {
      iVar7 = 0;
      if (uVar1 == 0) {
        return CONCAT31((uint3)(byte)(uVar1 >> 8),1);
      }
      while( true ) {
        puVar3 = FUN_10007e30(param_1);
        puVar4 = FUN_10007e30((undefined4 *)this);
        uVar6 = CONCAT22((short)((uint)puVar4 >> 0x10),*(short *)(puVar4 + iVar7));
        if (*(short *)(puVar4 + iVar7) != *(short *)(puVar3 + iVar7)) break;
        iVar7 = iVar7 + 1;
        if ((int)(uint)uVar1 <= iVar7) {
          return CONCAT31((int3)(uVar6 >> 8),1);
        }
      }
    }
  }
LAB_1000839f:
  return uVar6 & 0xffffff00;
}


// 100083b0 FUN_100083b0

undefined * __fastcall FUN_100083b0(int param_1)

{
  ushort uVar1;
  ushort uVar2;
  undefined *puVar3;
  float10 fVar4;
  
  uVar1 = *(ushort *)(param_1 + 0xe);
  if (uVar1 == DAT_1002163c) {
    fVar4 = FUN_10001370((uint)*(ushort *)(param_1 + 0xc));
    puVar3 = FUN_10007e40((float)fVar4);
    return puVar3;
  }
  uVar2 = *(ushort *)(param_1 + 0xc);
  FUN_10001370((uint)uVar2);
  FUN_10001370((uint)uVar1);
  FUN_10001370((uint)uVar2);
  FUN_10008f20(&DAT_100216c8,0x40,"%.3f, %.3f");
  return &DAT_100216c8;
}


// 10008420 FUN_10008420

undefined * __fastcall FUN_10008420(int param_1)

{
  uint *puVar1;
  
  puVar1 = &DAT_1001f1e0;
  do {
    if (*puVar1 == (uint)*(ushort *)(param_1 + 0x16)) {
      return (undefined *)puVar1[1];
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x1001f218);
  FUN_10008f20(&DAT_10021668,0x20,"%i");
  return &DAT_10021668;
}


// 10008460 FUN_10008460

undefined * __fastcall FUN_10008460(int param_1)

{
  uint *puVar1;
  
  if (*(short *)(param_1 + 0x12) != 0) {
    FUN_10008f20(&DAT_10021708,0x40,"%i, %i");
    return &DAT_10021708;
  }
  puVar1 = &DAT_1001f238;
  do {
    if (*puVar1 == (uint)*(ushort *)(param_1 + 0x10)) {
      return (undefined *)puVar1[1];
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x1001f328);
  FUN_10008f20(&DAT_10021648,0x20,"%i");
  return &DAT_10021648;
}


// 100084d0 FUN_100084d0

undefined * __fastcall FUN_100084d0(int param_1)

{
  undefined *puVar1;
  
  if (*(char *)(param_1 + 0x15) == '\0') {
    puVar1 = FUN_10007e90((float)*(byte *)(param_1 + 0x14));
    return puVar1;
  }
  FUN_10008f20(&DAT_10021748,0x40,"%i, %i");
  return &DAT_10021748;
}


// 10008520 FUN_10008520

void __thiscall FUN_10008520(void *this,byte *param_1)

{
  long lVar1;
  
  lVar1 = FUN_10007f70(param_1);
  *(short *)((int)this + 0x16) = (short)lVar1;
  return;
}


// 10008540 FUN_10008540

void __thiscall FUN_10008540(void *this,float *param_1)

{
  undefined2 local_4;
  
  local_4 = (undefined2)(int)ROUND(*param_1);
  *(undefined2 *)this = local_4;
  local_4 = (undefined2)(int)ROUND(param_1[1]);
  *(undefined2 *)((int)this + 2) = local_4;
  return;
}


// 100085a0 FUN_100085a0

void __thiscall FUN_100085a0(void *this,float *param_1)

{
  undefined1 local_4;
  
  local_4 = (undefined1)(int)ROUND(*param_1);
  *(undefined1 *)this = local_4;
  local_4 = (undefined1)(int)ROUND(param_1[1]);
  *(undefined1 *)((int)this + 1) = local_4;
  return;
}


// 100085f0 FUN_100085f0

undefined4 * __fastcall FUN_100085f0(undefined4 *param_1)

{
  uint uVar1;
  
  *param_1 = 0;
  param_1[1] = 0;
  *(undefined2 *)(param_1 + 2) = 0;
  *(undefined2 *)((int)param_1 + 10) = 0;
  *(undefined2 *)((int)param_1 + 0x16) = 0;
  uVar1 = FUN_10007cf0(1.0);
  *(short *)(param_1 + 3) = (short)uVar1;
  uVar1 = FUN_10007cf0(0.0);
  *(short *)((int)param_1 + 0xe) = (short)uVar1;
  *(byte *)((int)param_1 + 0x1a) = *(byte *)((int)param_1 + 0x1a) & 0xf8;
  *(undefined1 *)((int)param_1 + 0x15) = 0;
  *(undefined2 *)((int)param_1 + 0x12) = 0;
  *(undefined2 *)(param_1 + 6) = 0;
  param_1[7] = 0;
  *(undefined1 *)(param_1 + 5) = 100;
  *(undefined2 *)(param_1 + 4) = 0x4b;
  return param_1;
}


// 10008650 FUN_10008650

void __thiscall FUN_10008650(void *this,byte *param_1)

{
  undefined2 uVar1;
  int iVar2;
  undefined8 uVar4;
  float local_4;
  uint uVar3;
  
  iVar2 = FUN_10008c70(param_1,(byte *)"VOL_NORM");
  if (iVar2 == 0) {
    uVar3 = FUN_10007cf0(1.0);
    uVar1 = (undefined2)uVar3;
    local_4 = 0.0;
  }
  else {
    uVar4 = FUN_10001000((char *)param_1);
    uVar3 = FUN_10007cf0((float)uVar4);
    local_4 = (float)((ulonglong)uVar4 >> 0x20);
    uVar1 = (undefined2)uVar3;
  }
  *(undefined2 *)((int)this + 0xc) = uVar1;
  uVar3 = FUN_10007cf0(local_4);
  *(short *)((int)this + 0xe) = (short)uVar3;
  return;
}


// 100086c0 FUN_100086c0

void __thiscall FUN_100086c0(void *this,byte *param_1)

{
  int iVar1;
  undefined8 local_8;
  
  iVar1 = FUN_10008c70(param_1,(byte *)"PITCH_NORM");
  if (iVar1 == 0) {
    *(undefined1 *)((int)this + 0x14) = 100;
    *(undefined1 *)((int)this + 0x15) = 0;
    return;
  }
  iVar1 = FUN_10008c70(param_1,(byte *)"PITCH_LOW");
  if (iVar1 == 0) {
    *(undefined1 *)((int)this + 0x14) = 0x5f;
    *(undefined1 *)((int)this + 0x15) = 0;
    return;
  }
  iVar1 = FUN_10008c70(param_1,(byte *)"PITCH_HIGH");
  if (iVar1 == 0) {
    *(undefined1 *)((int)this + 0x14) = 0x78;
    *(undefined1 *)((int)this + 0x15) = 0;
    return;
  }
  local_8 = FUN_10001000((char *)param_1);
  FUN_100085a0((void *)((int)this + 0x14),(float *)&local_8);
  return;
}


// 10008760 FUN_10008760

void __thiscall FUN_10008760(void *this,byte *param_1)

{
  int iVar1;
  long lVar2;
  undefined8 local_8;
  
  iVar1 = FUN_10008bf0((char *)param_1,"SNDLVL_",7);
  if (iVar1 == 0) {
    lVar2 = FUN_10007ee0(param_1);
    *(short *)((int)this + 0x10) = (short)lVar2;
    *(undefined2 *)((int)this + 0x12) = 0;
    return;
  }
  local_8 = FUN_10001000((char *)param_1);
  FUN_10008540((void *)((int)this + 0x10),(float *)&local_8);
  return;
}


// 100087c0 FUN_100087c0

void FUN_100087c0(int *param_1,ushort *param_2,undefined2 *param_3)

{
  undefined2 *puVar1;
  int iVar2;
  undefined1 uVar3;
  undefined2 uVar4;
  undefined1 unaff_retaddr;
  
  *param_2 = *param_2 + 1;
  if (*param_2 == 1) {
    *(undefined2 *)param_1 = *param_3;
    *(undefined1 *)((int)param_1 + 2) = *(undefined1 *)(param_3 + 1);
    *(undefined1 *)((int)param_1 + 3) = *(undefined1 *)((int)param_3 + 3);
    return;
  }
  uVar4 = 0xffff;
  uVar3 = 0;
  if (*param_2 == 2) {
    uVar4 = (undefined2)*param_1;
    uVar3 = *(undefined1 *)((int)param_1 + 2);
    *param_1 = 0;
  }
  iVar2 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*param_1,(uint)*param_2 * 4);
  *param_1 = iVar2;
  puVar1 = (undefined2 *)(iVar2 + -4 + (uint)*param_2 * 4);
  *puVar1 = (short)*param_1;
  *(undefined1 *)(puVar1 + 1) = *(undefined1 *)((int)param_1 + 2);
  *(undefined1 *)((int)puVar1 + 3) = *(undefined1 *)((int)param_1 + 3);
  if (*param_2 == 2) {
    puVar1 = (undefined2 *)*param_1;
    *puVar1 = uVar4;
    *(undefined1 *)(puVar1 + 1) = uVar3;
    *(undefined1 *)((int)puVar1 + 3) = unaff_retaddr;
  }
  return;
}


// 10008880 FUN_10008880

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10008880(int param_1,int param_2)

{
  int iVar1;
  
  iVar1 = 0;
  if (0 < param_2) {
    do {
      if (DAT_10021788 == 0) {
        DAT_10021788 = (**(code **)(param_1 + iVar1 * 4))("VFileSystem018",0);
      }
      if (DAT_1002178c == 0) {
        DAT_1002178c = (**(code **)(param_1 + iVar1 * 4))("VMaterialSystem080",0);
        _DAT_10021790 = DAT_1002178c;
      }
      if (DAT_10021794 == 0) {
        DAT_10021794 = (**(code **)(param_1 + iVar1 * 4))("InputSystemVersion001",0);
      }
      if (DAT_10021798 == 0) {
        DAT_10021798 = (**(code **)(param_1 + iVar1 * 4))("NetworkSystemVersion001",0);
      }
      if (DAT_1002179c == 0) {
        DAT_1002179c = (**(code **)(param_1 + iVar1 * 4))("MaterialSystemHardwareConfig013",0);
      }
      if (DAT_100217a0 == 0) {
        DAT_100217a0 = (**(code **)(param_1 + iVar1 * 4))("DebugTextureInfo001",0);
      }
      if (DAT_100217a4 == 0) {
        DAT_100217a4 = (**(code **)(param_1 + iVar1 * 4))("VBAllocTracker001",0);
      }
      if (DAT_100217a8 == 0) {
        DAT_100217a8 = (**(code **)(param_1 + iVar1 * 4))("COLORCORRECTION_VERSION_1",0);
      }
      if (DAT_100217ac == 0) {
        DAT_100217ac = (**(code **)(param_1 + iVar1 * 4))("VP4001",0);
      }
      if (DAT_100217b0 == 0) {
        DAT_100217b0 = (**(code **)(param_1 + iVar1 * 4))("VMDLLIB001",0);
      }
      if (DAT_100217b4 == 0) {
        DAT_100217b4 = (**(code **)(param_1 + iVar1 * 4))("QueuedLoaderVersion001",0);
      }
      if (DAT_100217b8 == 0) {
        DAT_100217b8 = (**(code **)(param_1 + iVar1 * 4))("VResourceAccessControl001",0);
      }
      if (DAT_100217bc == 0) {
        DAT_100217bc = (**(code **)(param_1 + iVar1 * 4))("MATCHFRAMEWORK_001",0);
      }
      iVar1 = iVar1 + 1;
    } while (iVar1 < param_2);
  }
  return;
}


// 10008a30 FUN_10008a30

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10008a30(void)

{
  DAT_10021788 = 0;
  _DAT_10021790 = 0;
  DAT_1002178c = 0;
  DAT_1002179c = 0;
  DAT_100217a0 = 0;
  DAT_10021794 = 0;
  DAT_10021798 = 0;
  DAT_100217a8 = 0;
  DAT_100217ac = 0;
  DAT_100217b0 = 0;
  DAT_100217b4 = 0;
  DAT_100217b8 = 0;
  DAT_100217bc = 0;
  return;
}


// 10008a80 FUN_10008a80

int __cdecl FUN_10008a80(char *param_1)

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


// 10008ab0 FUN_10008ab0

int __cdecl FUN_10008ab0(byte *param_1,byte *param_2)

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


// 10008b10 FUN_10008b10

uint __cdecl FUN_10008b10(byte *param_1,byte *param_2)

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
        if (uVar1 != uVar2) goto LAB_10008b8a;
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
LAB_10008b8a:
  return uVar1 - uVar2;
}


// 10008b90 FUN_10008b90

int __cdecl FUN_10008b90(char *param_1,char *param_2,int param_3)

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


// 10008bf0 FUN_10008bf0

int __cdecl FUN_10008bf0(char *param_1,char *param_2,int param_3)

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


// 10008c70 FUN_10008c70

void __cdecl FUN_10008c70(byte *param_1,byte *param_2)

{
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  FUN_10008b10(param_1,param_2);
  return;
}


// 10008ca0 FUN_10008ca0

void __cdecl FUN_10008ca0(char *param_1,char *param_2,int param_3)

{
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  FUN_10008bf0(param_1,param_2,param_3);
  return;
}


// 10008ce0 FUN_10008ce0

char * __cdecl FUN_10008ce0(char *param_1,char *param_2)

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


// 10008dc0 FUN_10008dc0

char * __cdecl FUN_10008dc0(char *param_1,char *param_2,uint param_3,size_t param_4)

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


// 10008e50 FUN_10008e50

void __cdecl FUN_10008e50(void *param_1,int param_2,size_t param_3)

{
  _AssertValidWritePtr(param_1,param_3);
  _memset(param_1,param_2,param_3);
  return;
}


// 10008e80 FUN_10008e80

void __cdecl FUN_10008e80(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  _memcpy(param_1,param_2,param_3);
  return;
}


// 10008eb0 FUN_10008eb0

void __cdecl FUN_10008eb0(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  _memmove(param_1,param_2,param_3);
  return;
}


// 10008ee0 FUN_10008ee0

void __cdecl FUN_10008ee0(char *param_1,char *param_2,size_t param_3)

{
  _AssertValidWritePtr(param_1,param_3);
  AssertValidStringPtr(param_2,0xffffff);
  _strncpy(param_1,param_2,param_3);
  if (0 < (int)param_3) {
    param_1[param_3 - 1] = '\0';
  }
  return;
}


// 10008f20 FUN_10008f20

size_t __cdecl FUN_10008f20(char *param_1,size_t param_2,char *param_3)

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


// 10008f70 FUN_10008f70

size_t __cdecl FUN_10008f70(char *param_1,size_t param_2,char *param_3,va_list param_4)

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


// 10008fc0 FUN_10008fc0

void __cdecl FUN_10008fc0(LPCWSTR param_1,LPSTR param_2,int param_3)

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


// 10009010 FUN_10009010

void __cdecl FUN_10009010(char *param_1,char *param_2,char *param_3)

{
  char cVar1;
  char *pcVar2;
  char *pcVar3;
  char *pcVar4;
  
  if ((param_1 == (char *)0x0) || (*param_1 == '\0')) {
    *param_2 = '\0';
    return;
  }
  AssertValidStringPtr(param_1,0xffffff);
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  pcVar2 = pcVar2 + (-1 - (int)(param_1 + 1));
  for (pcVar3 = pcVar2;
      (((pcVar3 != (char *)0x0 && (cVar1 = param_1[(int)pcVar3], cVar1 != '.')) && (cVar1 != '\\'))
      && (cVar1 != '/')); pcVar3 = pcVar3 + -1) {
  }
  pcVar4 = pcVar2;
  if (param_1[(int)pcVar3] == '.') {
    pcVar4 = pcVar3 + -1;
  }
  do {
    if ((int)pcVar2 < 0) {
LAB_10009092:
      pcVar2 = (char *)0x0;
LAB_10009094:
      pcVar3 = pcVar4 + (2 - (int)pcVar2);
      if ((int)param_3 <= (int)(pcVar4 + (2 - (int)pcVar2))) {
        pcVar3 = param_3;
      }
      FUN_10008ee0(param_2,param_1 + (int)pcVar2,(size_t)pcVar3);
      return;
    }
    if ((param_1[(int)pcVar2] == '\\') || (param_1[(int)pcVar2] == '/')) {
      if ((-1 < (int)pcVar2) && ((param_1[(int)pcVar2] == '\\' || (param_1[(int)pcVar2] == '/')))) {
        pcVar2 = pcVar2 + 1;
        goto LAB_10009094;
      }
      goto LAB_10009092;
    }
    pcVar2 = pcVar2 + -1;
  } while( true );
}


// 100090e0 FUN_100090e0

void __cdecl FUN_100090e0(undefined4 *param_1)

{
  *param_1 = 0xffffffff;
  return;
}


// 100090f0 FUN_100090f0

void __cdecl FUN_100090f0(uint *param_1)

{
  *param_1 = ~*param_1;
  return;
}


// 10009100 FUN_10009100

void __cdecl FUN_10009100(uint *param_1,uint *param_2,uint param_3)

{
  uint uVar1;
  uint uVar2;
  int iVar3;
  
  uVar1 = *param_1;
  do {
    switch(param_3) {
    case 3:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 2:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 1:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
    case 0:
      *param_1 = uVar1;
      return;
    case 7:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 6:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 5:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    case 4:
      uVar1 = (uVar1 ^ *param_2) >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uVar1 ^ *param_2) & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4);
      *param_1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4);
      return;
    }
    uVar2 = (uint)param_2 & 3;
    if (uVar2 == 1) {
LAB_1000915d:
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
      param_2 = (uint *)((int)param_2 + 1);
    }
    else {
      if (uVar2 == 2) {
LAB_10009148:
        uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
        param_2 = (uint *)((int)param_2 + 1);
        goto LAB_1000915d;
      }
      if (uVar2 == 3) {
        uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uint)(byte)*param_2 ^ uVar1 & 0xff) * 4);
        param_2 = (uint *)((int)param_2 + 1);
        goto LAB_10009148;
      }
    }
    for (iVar3 = (int)(param_3 - uVar2) >> 3; iVar3 != 0; iVar3 = iVar3 + -1) {
      uVar1 = (uVar1 ^ *param_2) >> 8 ^ *(uint *)(&DAT_1001b2d0 + ((uVar1 ^ *param_2) & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4);
      uVar1 = *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4) ^ param_2[1] ^ uVar1 >> 8;
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4);
      uVar1 = uVar1 >> 8 ^ *(uint *)(&DAT_1001b2d0 + (uVar1 & 0xff) * 4);
      param_2 = param_2 + 2;
    }
    param_3 = param_3 - uVar2 & 7;
  } while( true );
}


// 100092e0 FUN_100092e0

void __thiscall FUN_100092e0(void *this,int param_1)

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


// 10009390 FUN_10009390

ushort __fastcall FUN_10009390(int param_1)

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
    FUN_100092e0((void *)(param_1 + 4),1);
    uVar2 = *puVar1;
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)uVar2)
       ) {
      iVar4 = *(int *)(param_1 + 8);
      puVar3 = &local_2;
    }
    else {
      puVar3 = (ushort *)FUN_10002b00((void *)(param_1 + 4),(short *)&local_2,(short *)puVar1);
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


// 100094b0 FUN_100094b0

undefined2 * __thiscall FUN_100094b0(void *this,ushort param_1)

{
  if ((DAT_100217f8 & 1) == 0) {
    DAT_100217f8 = DAT_100217f8 | 1;
    DAT_100217f0 = 0xffff;
    DAT_100217f2 = 0xffff;
    DAT_100217f4 = 0xffff;
    DAT_100217f6 = 1;
  }
  if (param_1 != 0xffff) {
    return (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
  }
  return &DAT_100217f0;
}


// 10009510 FUN_10009510

bool __thiscall FUN_10009510(void *this,ushort *param_1,ushort *param_2)

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
      uVar5 = FUN_1000d960(pbVar6);
      uVar3 = (ushort)uVar5;
    }
    else {
      uVar5 = FUN_1000d9a0(pbVar6);
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
      uVar5 = FUN_1000d960((byte *)puVar7);
      uVar4 = (ushort)uVar5;
    }
    else {
      uVar5 = FUN_1000d9a0((byte *)puVar7);
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
    uVar5 = FUN_10008b10(pbVar6,(byte *)puVar7);
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


// 10009650 FUN_10009650

int __thiscall FUN_10009650(void *this,int param_1)

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


// 10009690 FUN_10009690

ushort * __thiscall FUN_10009690(void *this,ushort *param_1,ushort *param_2,undefined1 *param_3)

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
    bVar3 = FUN_10009510(this,param_1,(ushort *)(iVar2 + 8 + *(int *)((int)this + 4)));
    if (bVar3) {
      *param_3 = 1;
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100217f0;
        uVar1 = DAT_100217f0;
      }
      else {
        puVar4 = (ushort *)(*(int *)((int)this + 4) + iVar2);
        uVar1 = *(ushort *)(*(int *)((int)this + 4) + iVar2);
      }
    }
    else {
      *param_3 = 0;
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100217f0;
      }
      else {
        puVar4 = (ushort *)(*(int *)((int)this + 4) + iVar2);
      }
      uVar1 = puVar4[1];
    }
  }
  return puVar4;
}


// 100097a0 FUN_100097a0

void __thiscall FUN_100097a0(void *this,ushort param_1)

{
  ushort uVar1;
  undefined2 *puVar2;
  short *psVar3;
  ushort *puVar4;
  
  puVar2 = FUN_100094b0(this,param_1);
  uVar1 = puVar2[1];
  puVar2 = FUN_100094b0(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + 2 + (uint)param_1 * 0xc) = *puVar2;
  psVar3 = FUN_100094b0(this,uVar1);
  if (*psVar3 != -1) {
    puVar4 = FUN_100094b0(this,uVar1);
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)*puVar4 * 0xc) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar2 = FUN_100094b0(this,param_1);
    *(undefined2 *)(*(int *)((int)this + 4) + 4 + (uint)uVar1 * 0xc) = puVar2[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar2 = FUN_100094b0(this,param_1);
    puVar4 = FUN_100094b0(this,puVar2[2]);
    if (*puVar4 == param_1) {
      puVar2 = FUN_100094b0(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar2[2] * 0xc) = uVar1;
    }
    else {
      puVar2 = FUN_100094b0(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)(ushort)puVar2[2] * 0xc) = uVar1;
    }
  }
  *(ushort *)(*(int *)((int)this + 4) + (uint)uVar1 * 0xc) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0xc) = uVar1;
  }
  return;
}


// 100098a0 FUN_100098a0

void __thiscall FUN_100098a0(void *this,ushort param_1)

{
  ushort uVar1;
  ushort *puVar2;
  undefined2 *puVar3;
  
  puVar2 = FUN_100094b0(this,param_1);
  uVar1 = *puVar2;
  puVar3 = FUN_100094b0(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc) = puVar3[1];
  puVar3 = FUN_100094b0(this,uVar1);
  if (puVar3[1] != -1) {
    puVar3 = FUN_100094b0(this,uVar1);
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)(ushort)puVar3[1] * 0xc) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar3 = FUN_100094b0(this,param_1);
    *(undefined2 *)(*(int *)((int)this + 4) + 4 + (uint)uVar1 * 0xc) = puVar3[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar3 = FUN_100094b0(this,param_1);
    puVar3 = FUN_100094b0(this,puVar3[2]);
    if (puVar3[1] == param_1) {
      puVar3 = FUN_100094b0(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)(ushort)puVar3[2] * 0xc) = uVar1;
    }
    else {
      puVar3 = FUN_100094b0(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar3[2] * 0xc) = uVar1;
    }
  }
  *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)uVar1 * 0xc) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0xc) = uVar1;
  }
  return;
}


// 100099a0 FUN_100099a0

undefined1 * __thiscall FUN_100099a0(void *this,ushort param_1)

{
  ushort *puVar1;
  
  if (param_1 == 0xffff) {
    return &DAT_1001a66b;
  }
  puVar1 = (ushort *)(*(int *)((int)this + 4) + 8 + (uint)param_1 * 0xc);
  return (undefined1 *)
         (*(int *)(*(int *)((int)this + 0x24) + (uint)*puVar1 * 4) + 10 + (uint)puVar1[1]);
}


// 100099e0 FUN_100099e0

int __thiscall FUN_100099e0(void *this,undefined4 param_1,int param_2)

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


// 10009a40 FUN_10009a40

ushort __thiscall FUN_10009a40(void *this,ushort *param_1)

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
    bVar2 = FUN_10009510(this,param_1,(ushort *)(*(int *)((int)this + 4) + 8 + iVar1));
    if (bVar2) {
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      bVar2 = uVar4 != 0xffff;
      uVar4 = DAT_100217f0;
      if (bVar2) {
        uVar4 = *(ushort *)(*(int *)((int)this + 4) + iVar1);
      }
    }
    else {
      bVar2 = FUN_10009510(this,(ushort *)(*(int *)((int)this + 4) + 8 + iVar1),param_1);
      if (!bVar2) {
        return uVar4;
      }
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      if (uVar4 == 0xffff) {
        puVar3 = &DAT_100217f0;
      }
      else {
        puVar3 = (undefined2 *)(*(int *)((int)this + 4) + iVar1);
      }
      uVar4 = puVar3[1];
    }
  } while (uVar4 != 0xffff);
  return 0xffff;
}


// 10009b50 FUN_10009b50

void __fastcall FUN_10009b50(void *param_1)

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
         ((uVar3 <= uVar1 && (puVar5 = FUN_100094b0(param_1,uVar3), *puVar5 != uVar3)))) {
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


// 10009c20 FUN_10009c20

void __thiscall FUN_10009c20(void *this,ushort param_1)

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
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      if (param_1 == 0xffff) {
        puVar4 = &DAT_100217f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
      }
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      if (puVar4[2] == 0xffff) {
        puVar4 = &DAT_100217f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0xc);
      }
      if (puVar4[3] != 0) break;
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      if (param_1 == 0xffff) {
        puVar4 = &DAT_100217f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
      }
      uVar1 = puVar4[2];
      uVar7 = (uint)uVar1;
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100217f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar1 * 0xc);
      }
      uVar2 = puVar4[2];
      uVar6 = (uint)uVar2;
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100217f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar1 * 0xc);
      }
      if ((DAT_100217f8 & 1) == 0) {
        DAT_100217f8 = DAT_100217f8 | 1;
        DAT_100217f0 = 0xffff;
        DAT_100217f2 = 0xffff;
        DAT_100217f4 = 0xffff;
        DAT_100217f6 = 1;
      }
      if (puVar4[2] == 0xffff) {
        puVar5 = &DAT_100217f0;
      }
      else {
        puVar5 = (ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0xc);
      }
      if (*puVar5 == uVar1) {
        if ((DAT_100217f8 & 1) == 0) {
          DAT_100217f8 = DAT_100217f8 | 1;
          DAT_100217f0 = 0xffff;
          DAT_100217f2 = 0xffff;
          DAT_100217f4 = 0xffff;
          DAT_100217f6 = 1;
        }
        if (uVar2 == 0xffff) {
          puVar4 = &DAT_100217f0;
        }
        else {
          puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar2 * 0xc);
        }
        uVar3 = puVar4[1];
        if ((DAT_100217f8 & 1) == 0) {
          DAT_100217f8 = DAT_100217f8 | 1;
          DAT_100217f0 = 0xffff;
          DAT_100217f2 = 0xffff;
          DAT_100217f4 = 0xffff;
          DAT_100217f6 = 1;
        }
        if (uVar3 == 0xffff) {
          puVar4 = &DAT_100217f0;
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
          if ((DAT_100217f8 & 1) == 0) {
            DAT_100217f8 = DAT_100217f8 | 1;
            DAT_100217f0 = 0xffff;
            DAT_100217f2 = 0xffff;
            DAT_100217f4 = 0xffff;
            DAT_100217f6 = 1;
          }
          if (param_1 == 0xffff) {
            puVar4 = &DAT_100217f0;
          }
          else {
            puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
          }
          if ((DAT_100217f8 & 1) == 0) {
            DAT_100217f8 = DAT_100217f8 | 1;
            DAT_100217f0 = 0xffff;
            DAT_100217f2 = 0xffff;
            DAT_100217f4 = 0xffff;
            DAT_100217f6 = 1;
          }
          if (puVar4[2] == 0xffff) {
            puVar4 = &DAT_100217f0;
          }
          else {
            puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0xc);
          }
          if (puVar4[1] == param_1) {
            FUN_100097a0(this,uVar1);
            puVar4 = FUN_100094b0(this,uVar1);
            uVar7 = (uint)(ushort)puVar4[2];
            puVar4 = FUN_100094b0(this,puVar4[2]);
            uVar6 = (uint)(ushort)puVar4[2];
            param_1 = uVar1;
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar7 * 0xc) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0xc) = 0;
          FUN_100098a0(this,(ushort)uVar6);
        }
      }
      else {
        if ((DAT_100217f8 & 1) == 0) {
          DAT_100217f8 = DAT_100217f8 | 1;
          DAT_100217f0 = 0xffff;
          DAT_100217f2 = 0xffff;
          DAT_100217f4 = 0xffff;
          DAT_100217f6 = 1;
        }
        if (uVar2 == 0xffff) {
          puVar5 = &DAT_100217f0;
        }
        else {
          puVar5 = (ushort *)(*(int *)((int)this + 4) + (uint)uVar2 * 0xc);
        }
        uVar3 = *puVar5;
        if ((DAT_100217f8 & 1) == 0) {
          DAT_100217f8 = DAT_100217f8 | 1;
          DAT_100217f0 = 0xffff;
          DAT_100217f2 = 0xffff;
          DAT_100217f4 = 0xffff;
          DAT_100217f6 = 1;
        }
        if (uVar3 == 0xffff) {
          puVar4 = &DAT_100217f0;
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
          if ((DAT_100217f8 & 1) == 0) {
            DAT_100217f8 = DAT_100217f8 | 1;
            DAT_100217f0 = 0xffff;
            DAT_100217f2 = 0xffff;
            DAT_100217f4 = 0xffff;
            DAT_100217f6 = 1;
          }
          if (param_1 == 0xffff) {
            puVar4 = &DAT_100217f0;
          }
          else {
            puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
          }
          if ((DAT_100217f8 & 1) == 0) {
            DAT_100217f8 = DAT_100217f8 | 1;
            DAT_100217f0 = 0xffff;
            DAT_100217f2 = 0xffff;
            DAT_100217f4 = 0xffff;
            DAT_100217f6 = 1;
          }
          if (puVar4[2] == 0xffff) {
            puVar5 = &DAT_100217f0;
          }
          else {
            puVar5 = (ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0xc);
          }
          if (*puVar5 == param_1) {
            FUN_100098a0(this,uVar1);
            puVar4 = FUN_100094b0(this,uVar1);
            uVar7 = (uint)(ushort)puVar4[2];
            puVar4 = FUN_100094b0(this,puVar4[2]);
            uVar6 = (uint)(ushort)puVar4[2];
            param_1 = uVar1;
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar7 * 0xc) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0xc) = 0;
          FUN_100097a0(this,(ushort)uVar6);
        }
      }
    } while (param_1 != *(ushort *)((int)this + 0x10));
  }
  *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)*(ushort *)((int)this + 0x10) * 0xc) = 1;
  return;
}


// 1000a190 FUN_1000a190

undefined1 * __thiscall FUN_1000a190(void *this,ushort param_1)

{
  CThreadSpinRWLock *this_00;
  ushort uVar1;
  int iVar2;
  
  this_00 = (CThreadSpinRWLock *)((int)this + 0x38);
  CThreadSpinRWLock::LockForRead(this_00);
  if (param_1 == 0xffff) {
    CThreadSpinRWLock::UnlockRead(this_00);
    return &DAT_1001a66b;
  }
  uVar1 = *(ushort *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc + 10);
  iVar2 = *(int *)(*(int *)((int)this + 0x24) +
                  (uint)*(ushort *)(*(int *)((int)this + 4) + 8 + (uint)param_1 * 0xc) * 4);
  CThreadSpinRWLock::UnlockRead(this_00);
  return (undefined1 *)(iVar2 + 10 + (uint)uVar1);
}


// 1000a1f0 FUN_1000a1f0

ushort * __thiscall FUN_1000a1f0(void *this,ushort *param_1,int param_2)

{
  ushort uVar1;
  
  if (param_2 == 0) {
    *param_1 = 0xffff;
    return param_1;
  }
  *(int *)((int)this + 0x20) = param_2;
  param_2 = -1;
  uVar1 = FUN_10009a40(this,(ushort *)&param_2);
  *param_1 = uVar1;
  return param_1;
}


// 1000a230 FUN_1000a230

void __thiscall FUN_1000a230(void *this,ushort param_1,ushort param_2,char param_3)

{
  undefined2 *puVar1;
  
  puVar1 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0xc);
  puVar1[2] = param_2;
  puVar1[1] = 0xffff;
  *puVar1 = 0xffff;
  puVar1[3] = 0;
  if (param_2 == 0xffff) {
    *(ushort *)((int)this + 0x10) = param_1;
    FUN_10009c20(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(ushort *)(*(int *)((int)this + 4) + (uint)param_2 * 0xc) = param_1;
    FUN_10009c20(this,param_1);
    return;
  }
  *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)param_2 * 0xc) = param_1;
  FUN_10009c20(this,param_1);
  return;
}


// 1000a2a0 FUN_1000a2a0

void __fastcall FUN_1000a2a0(void *param_1)

{
  int iVar1;
  
  FUN_10009b50(param_1);
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


// 1000a320 FUN_1000a320

void __fastcall FUN_1000a320(void *param_1)

{
  FUN_10009b50(param_1);
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


// 1000a380 FUN_1000a380

void __fastcall FUN_1000a380(void *param_1)

{
  FUN_1000a2a0(param_1);
  FUN_10004020((int *)((int)param_1 + 0x24));
  FUN_1000a320(param_1);
  return;
}


// 1000a3a0 FUN_1000a3a0

undefined4 __thiscall FUN_1000a3a0(void *this,ushort *param_1)

{
  undefined4 *puVar1;
  ushort *puVar2;
  ushort uVar3;
  void *local_4;
  
  puVar2 = param_1;
  local_4 = this;
  FUN_10009690(this,param_1,(ushort *)&local_4,(undefined1 *)&param_1);
  uVar3 = FUN_10009390((int)this);
  FUN_1000a230(this,uVar3,(ushort)local_4,(char)param_1);
  *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + 1;
  puVar1 = (undefined4 *)(*(int *)((int)this + 4) + 8 + (uint)uVar3 * 0xc);
  if (puVar1 != (undefined4 *)0x0) {
    *puVar1 = *(undefined4 *)puVar2;
  }
  return CONCAT22((short)((uint)puVar1 >> 0x10),uVar3);
}


// 1000a400 FUN_1000a400

void * __thiscall FUN_1000a400(void *this,undefined4 param_1,int param_2,undefined1 param_3)

{
  FUN_100099e0(this,param_1,param_2);
  *(undefined1 *)((int)this + 0x1c) = param_3;
  *(undefined4 *)((int)this + 0x24) = 0;
  *(undefined4 *)((int)this + 0x28) = 0;
  *(undefined4 *)((int)this + 0x30) = 0;
  *(undefined4 *)((int)this + 0x34) = 0;
  *(undefined4 *)((int)this + 0x2c) = 8;
  return this;
}


// 1000a440 FUN_1000a440

ushort * __thiscall FUN_1000a440(void *this,ushort *param_1,byte *param_2)

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
  uVar5 = FUN_10009a40(this,(ushort *)&local_8);
  if (uVar5 == 0xffff) {
    pbVar7 = pbVar4;
    do {
      bVar2 = *pbVar7;
      pbVar7 = pbVar7 + 1;
    } while (bVar2 != 0);
    local_4 = pbVar7 + (1 - (int)(pbVar4 + 1));
    uVar10 = (uint)(pbVar7 + (4 - (int)(pbVar4 + 1))) & 0xfffffffe;
    iVar8 = FUN_10009650(this,uVar10);
    if (iVar8 == -1) {
      uVar11 = uVar10;
      if ((int)uVar10 < 0x801) {
        uVar11 = 0x800;
      }
      local_8 = (int *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(uVar11);
      *local_8 = uVar11 - 0xc;
      local_8[1] = 0;
      iVar8 = FUN_100041d0((void *)((int)this + 0x24),*(int *)((int)this + 0x30),&local_8);
    }
    if (*(char *)((int)this + 0x1c) == '\0') {
      uVar11 = FUN_1000d960(pbVar4);
      uVar6 = (undefined2)uVar11;
    }
    else {
      uVar11 = FUN_1000d9a0(pbVar4);
      uVar6 = (undefined2)uVar11;
    }
    iVar3 = *(int *)(*(int *)((int)this + 0x24) + iVar8 * 4);
    local_8 = (int *)(uint)*(ushort *)(iVar3 + 4);
    puVar1 = (undefined2 *)(iVar3 + 8 + *(int *)(iVar3 + 4));
    *puVar1 = uVar6;
    _memcpy(puVar1 + 1,param_2,(size_t)local_4);
    *(int *)(iVar3 + 4) = *(int *)(iVar3 + 4) + uVar10;
    param_2 = (byte *)CONCAT22(local_8._0_2_,(short)iVar8);
    uVar9 = FUN_1000a3a0(this,(ushort *)&param_2);
    uVar5 = (ushort)uVar9;
  }
  *param_1 = uVar5;
  return param_1;
}


// 1000a570 FUN_1000a570

void FUN_1000a570(void)

{
  int iVar1;
  undefined4 uVar2;
  
  if (DAT_100217fc == '\0') {
    iVar1 = FUN_10001110(0x48);
    if (iVar1 != 0) {
      *(undefined4 *)(iVar1 + 4) = 0;
      *(undefined4 *)(iVar1 + 8) = 0x20;
      *(undefined4 *)(iVar1 + 0xc) = 0;
      uVar2 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(0x180);
      *(undefined4 *)(iVar1 + 4) = uVar2;
      *(undefined2 *)(iVar1 + 0x12) = 0;
      *(undefined2 *)(iVar1 + 0x10) = 0xffff;
      *(undefined2 *)(iVar1 + 0x14) = 0xffff;
      *(undefined2 *)(iVar1 + 0x16) = 0xffff;
      *(undefined4 *)(iVar1 + 0x18) = *(undefined4 *)(iVar1 + 4);
      *(undefined1 *)(iVar1 + 0x1c) = 0;
      *(undefined4 *)(iVar1 + 0x24) = 0;
      *(undefined4 *)(iVar1 + 0x28) = 0;
      *(undefined4 *)(iVar1 + 0x2c) = 8;
      *(undefined4 *)(iVar1 + 0x30) = 0;
      *(undefined4 *)(iVar1 + 0x34) = 0;
      CThreadSpinRWLock::CThreadSpinRWLock((CThreadSpinRWLock *)(iVar1 + 0x38));
      DAT_100217e0 = iVar1;
      DAT_100217fc = '\x01';
      return;
    }
    DAT_100217e0 = 0;
    DAT_100217fc = '\x01';
  }
  return;
}


// 1000a610 FUN_1000a610

ushort * __thiscall FUN_1000a610(void *this,byte *param_1)

{
  CThreadSpinRWLock *this_00;
  void *this_01;
  
  FUN_1000a570();
  this_01 = DAT_100217e0;
  this_00 = (CThreadSpinRWLock *)((int)DAT_100217e0 + 0x38);
  CThreadSpinRWLock::LockForWrite(this_00);
  FUN_1000a440(this_01,(ushort *)&param_1,param_1);
  CThreadSpinRWLock::UnlockWrite(this_00);
  *(undefined2 *)this = param_1._0_2_;
  return (ushort *)this;
}


// 1000a660 FUN_1000a660

void __fastcall FUN_1000a660(ushort *param_1)

{
  ushort uVar1;
  
  uVar1 = *param_1;
  FUN_1000a570();
  FUN_1000a190(DAT_100217e0,uVar1);
  return;
}


// 1000a680 FUN_1000a680

void __thiscall FUN_1000a680(void *this,undefined4 param_1,undefined4 param_2)

{
  *(undefined4 *)((int)this + 4) = param_2;
  *(undefined4 *)this = param_1;
  *(void **)((int)this + 8) = DAT_10021800;
  DAT_10021800 = this;
  return;
}


// 1000a6a0 FUN_1000a6a0

undefined4 __cdecl FUN_1000a6a0(byte *param_1,undefined4 *param_2)

{
  int *piVar1;
  byte bVar2;
  byte *pbVar3;
  int iVar4;
  undefined4 uVar5;
  byte *pbVar6;
  undefined4 *puVar7;
  bool bVar8;
  
  puVar7 = DAT_10021800;
  if (DAT_10021800 != (undefined4 *)0x0) {
    do {
      pbVar3 = (byte *)puVar7[1];
      pbVar6 = param_1;
      do {
        bVar2 = *pbVar3;
        bVar8 = bVar2 < *pbVar6;
        if (bVar2 != *pbVar6) {
LAB_1000a6d5:
          iVar4 = (1 - (uint)bVar8) - (uint)(bVar8 != 0);
          goto LAB_1000a6da;
        }
        if (bVar2 == 0) break;
        bVar2 = pbVar3[1];
        bVar8 = bVar2 < pbVar6[1];
        if (bVar2 != pbVar6[1]) goto LAB_1000a6d5;
        pbVar3 = pbVar3 + 2;
        pbVar6 = pbVar6 + 2;
      } while (bVar2 != 0);
      iVar4 = 0;
LAB_1000a6da:
      if (iVar4 == 0) {
        if (param_2 != (undefined4 *)0x0) {
          *param_2 = 0;
        }
                    /* WARNING: Could not recover jumptable at 0x1000a70a. Too many branches */
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


// 1000a720 FUN_1000a720

undefined * FUN_1000a720(void)

{
  return FUN_1000a6a0;
}


// 1000a730 FUN_1000a730

void __thiscall FUN_1000a730(void *this,undefined4 param_1)

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
  Warning(&DAT_1001a938);
  return;
}


// 1000a810 FUN_1000a810

void __fastcall FUN_1000a810(int param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  
  iVar2 = *(int *)(param_1 + 0x18);
  while (iVar2 != 0) {
    iVar1 = *(int *)(iVar2 + 0x14);
    *(undefined4 *)(iVar2 + 0x14) = 0;
    FUN_1000a810(iVar2);
    piVar3 = (int *)KeyValuesSystem();
    (**(code **)(*piVar3 + 8))(iVar2);
    iVar2 = iVar1;
  }
  iVar2 = *(int *)(param_1 + 0x14);
  while ((iVar2 != 0 && (iVar2 != param_1))) {
    iVar1 = *(int *)(iVar2 + 0x14);
    *(undefined4 *)(iVar2 + 0x14) = 0;
    FUN_1000a810(iVar2);
    piVar3 = (int *)KeyValuesSystem();
    (**(code **)(*piVar3 + 8))(iVar2);
    iVar2 = iVar1;
  }
  FUN_100012c0(*(undefined4 *)(param_1 + 4));
  *(undefined4 *)(param_1 + 4) = 0;
  FUN_100012c0(*(undefined4 *)(param_1 + 8));
  *(undefined4 *)(param_1 + 8) = 0;
  return;
}


// 1000a8a0 FUN_1000a8a0

undefined1 * __fastcall FUN_1000a8a0(undefined4 *param_1)

{
  int *piVar1;
  undefined1 *puVar2;
  
  if (param_1 != (undefined4 *)0x0) {
    piVar1 = (int *)KeyValuesSystem();
    puVar2 = (undefined1 *)(**(code **)(*piVar1 + 0x10))(*param_1);
    return puVar2;
  }
  return &DAT_1001a66b;
}


// 1000a8d0 FUN_1000a8d0

undefined1 * __thiscall
FUN_1000a8d0(void *this,int *param_1,undefined1 *param_2,undefined1 *param_3)

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
    FUN_1000c3b0(param_1);
    cVar1 = (char)param_1[5];
    while (cVar1 == '\0') {
      uVar4 = FUN_1000c990(param_1);
      if ((char)uVar4 == '\0') {
        pcVar5 = (char *)FUN_1000c150(param_1,1,0);
        if (pcVar5 == (char *)0x0) {
          return (undefined1 *)0x0;
        }
        cVar1 = *pcVar5;
        if (cVar1 == '\"') {
          *param_2 = 1;
          if (*(char *)((int)this + 0x11) == '\0') {
            puVar6 = (undefined4 *)FUN_1000bf40();
          }
          else {
            puVar6 = (undefined4 *)FUN_1000bf30();
          }
          FUN_1000cb50(param_1,puVar6,&DAT_10021808,0x400);
          return &DAT_10021808;
        }
        if ((cVar1 == '{') || (cVar1 == '}')) {
          DAT_10021809 = 0;
          DAT_10021808 = cVar1;
          FUN_1000c190(param_1,1,1);
          return &DAT_10021808;
        }
        iVar8 = 0;
        bVar3 = false;
        bVar2 = false;
        pcVar5 = (char *)FUN_1000c150(param_1,1,0);
        goto joined_r0x1000a98e;
      }
      FUN_1000c3b0(param_1);
      cVar1 = (char)param_1[5];
    }
  }
  return (undefined1 *)0x0;
joined_r0x1000a98e:
  if ((((pcVar5 == (char *)0x0) || (cVar1 = *pcVar5, cVar1 == '\0')) || (cVar1 == '\"')) ||
     ((cVar1 == '{' || (cVar1 == '}')))) {
LAB_1000aa29:
    (&DAT_10021808)[iVar8] = 0;
    return &DAT_10021808;
  }
  if (cVar1 == '[') {
    bVar2 = true;
  }
  if ((cVar1 == ']') && (bVar2)) {
    bVar2 = false;
    *param_3 = 1;
  }
  iVar7 = _isspace((int)*pcVar5);
  if ((iVar7 != 0) && (!bVar2)) goto LAB_1000aa29;
  if (iVar8 < 0x3ff) {
    (&DAT_10021808)[iVar8] = *pcVar5;
    iVar8 = iVar8 + 1;
  }
  else if (!bVar3) {
    bVar3 = true;
    FUN_1000a730(&DAT_1001f408," ReadToken overflow");
  }
  FUN_1000c190(param_1,1,1);
  pcVar5 = (char *)FUN_1000c150(param_1,1,0);
  goto joined_r0x1000a98e;
}


// 1000aa60 FUN_1000aa60

void __thiscall FUN_1000aa60(void *this,int param_1)

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


// 1000aab0 FUN_1000aab0

undefined4 __fastcall FUN_1000aab0(int param_1)

{
  if (param_1 != 0) {
    return *(undefined4 *)(param_1 + 0x18);
  }
  return 0;
}


// 1000aac0 FUN_1000aac0

undefined4 __fastcall FUN_1000aac0(int param_1)

{
  if (param_1 != 0) {
    return *(undefined4 *)(param_1 + 0x14);
  }
  return 0;
}


// 1000aad0 FUN_1000aad0

void __thiscall FUN_1000aad0(void *this,int param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  
  puVar2 = (undefined4 *)0x0;
  for (puVar1 = *(undefined4 **)((int)this + 0x18); puVar1 != (undefined4 *)0x0;
      puVar1 = (undefined4 *)puVar1[5]) {
    puVar3 = FUN_1000aef0(puVar1);
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


// 1000ab10 FUN_1000ab10

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

bool FUN_1000ab10(void)

{
  char cVar1;
  int iVar2;
  char *pcVar3;
  uint uVar4;
  char *unaff_ESI;
  bool bVar5;
  byte local_40 [64];
  
  iVar2 = FUN_10008a80(unaff_ESI);
  if (iVar2 < 4) {
    bVar5 = false;
  }
  else {
    cVar1 = unaff_ESI[2];
    pcVar3 = unaff_ESI + 2;
    if (cVar1 == '!') {
      pcVar3 = unaff_ESI + 3;
    }
    FUN_10008ee0((char *)local_40,pcVar3,0x40);
    iVar2 = FUN_10008a80((char *)local_40);
    local_40[iVar2 + -1] = 0;
    if ((_DAT_10021cc4 & 1) == 0) {
      _DAT_10021cc4 = _DAT_10021cc4 | 1;
      FUN_1000d940(&DAT_10021cbc,"cl_language");
    }
    uVar4 = FUN_10008b10(local_40,*(byte **)(DAT_10021cc0 + 0x24));
    bVar5 = uVar4 == 0;
    if (cVar1 == '!') {
      return !bVar5;
    }
  }
  return bVar5;
}


// 1000abb0 FUN_1000abb0

bool FUN_1000abb0(void)

{
  byte *in_EAX;
  char *pcVar1;
  uint uVar2;
  bool bVar3;
  
  pcVar1 = FUN_10008ce0((char *)in_EAX,"$OSX");
  if (pcVar1 == (char *)0x0) {
    pcVar1 = FUN_10008ce0((char *)in_EAX,"$LINUX");
    if (pcVar1 == (char *)0x0) {
      pcVar1 = FUN_10008ce0((char *)in_EAX,"$POSIX");
      if (pcVar1 == (char *)0x0) {
        pcVar1 = FUN_10008ce0((char *)in_EAX,"$WINDOWS");
        if (pcVar1 != (char *)0x0) {
          return true;
        }
        uVar2 = FUN_10008b10((byte *)"[$WIN32]",in_EAX);
        bVar3 = uVar2 == 0;
        if (!bVar3) {
          bVar3 = FUN_1000ab10();
        }
        return bVar3;
      }
    }
  }
  return false;
}


// 1000ac30 FUN_1000ac30

void __cdecl FUN_1000ac30(undefined4 param_1)

{
  int *piVar1;
  
  piVar1 = (int *)KeyValuesSystem();
  (**(code **)(*piVar1 + 4))(param_1);
  return;
}


// 1000ac50 FUN_1000ac50

undefined4 * __thiscall FUN_1000ac50(void *this,undefined4 param_1)

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


// 1000aca0 FUN_1000aca0

int * __thiscall FUN_1000aca0(void *this,char *param_1,undefined4 param_2)

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
      FUN_10008ee0(&local_100,param_1,(size_t)pcVar1);
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
      if (*this == iVar3) goto LAB_1000adc5;
      piVar2 = (int *)this;
      piVar4 = *(int **)((int)this + 0x14);
    }
    if (*(void **)(unaff_EBP + 0x1c) != (void *)0x0) {
      this = FUN_1000aca0(*(void **)(unaff_EBP + 0x1c),param_1,0);
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
        this = FUN_1000ac50(this_00,pcVar5);
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
LAB_1000adc5:
    if (unaff_ESI != 0) {
      this = FUN_1000aca0(this,(char *)(unaff_ESI + 1),unaff_retaddr);
    }
  }
  return (int *)this;
}


// 1000ae00 FUN_1000ae00

int __thiscall FUN_1000ae00(void *this,char *param_1,int param_2)

{
  int *piVar1;
  long lVar2;
  int iVar3;
  undefined4 extraout_EDX;
  ulonglong uVar4;
  
  piVar1 = FUN_1000aca0(this,param_1,0);
  if (piVar1 != (int *)0x0) {
    iVar3 = (char)piVar1[4] + -1;
    switch(iVar3) {
    case 0:
      lVar2 = _atol((char *)piVar1[1]);
      return lVar2;
    default:
      return piVar1[3];
    case 2:
      uVar4 = FUN_1000e380(iVar3,extraout_EDX);
      return (int)uVar4;
    case 4:
      iVar3 = thunk_FUN_1000f25c((wchar_t *)piVar1[2]);
      return iVar3;
    case 6:
      return 0;
    }
  }
  return param_2;
}


// 1000ae80 FUN_1000ae80

void __thiscall FUN_1000ae80(void *this,char *param_1,char *param_2)

{
  int *piVar1;
  int iVar2;
  void *pvVar3;
  
  piVar1 = FUN_1000aca0(this,param_1,1);
  if (piVar1 != (int *)0x0) {
    FUN_100012c0(piVar1[1]);
    FUN_100012c0(piVar1[2]);
    piVar1[2] = 0;
    if (param_2 == (char *)0x0) {
      param_2 = "";
    }
    iVar2 = FUN_10008a80(param_2);
    pvVar3 = (void *)FUN_10001110(iVar2 + 1U);
    piVar1[1] = (int)pvVar3;
    FUN_10008e80(pvVar3,param_2,iVar2 + 1U);
    *(undefined1 *)(piVar1 + 4) = 1;
  }
  return;
}


// 1000aef0 FUN_1000aef0

undefined4 * __fastcall FUN_1000aef0(undefined4 *param_1)

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
    puVar7 = FUN_1000ac50(pvVar5,&DAT_1001a66b);
  }
  else {
    piVar4 = (int *)KeyValuesSystem();
    uVar6 = (**(code **)(*piVar4 + 0x10))(*param_1);
    puVar7 = FUN_1000ac50(pvVar5,uVar6);
  }
  *(undefined1 *)(puVar7 + 4) = *(undefined1 *)(param_1 + 4);
  switch(*(undefined1 *)(param_1 + 4)) {
  case 1:
    if ((char *)param_1[1] != (char *)0x0) {
      iVar9 = FUN_10008a80((char *)param_1[1]);
      pvVar5 = (void *)FUN_10001110(iVar9 + 1U);
      puVar7[1] = pvVar5;
      FUN_10008e80(pvVar5,(void *)param_1[1],iVar9 + 1U);
      FUN_1000aad0(param_1,(int)puVar7);
      return puVar7;
    }
    break;
  case 2:
    puVar7[3] = param_1[3];
    FUN_1000aad0(param_1,(int)puVar7);
    return puVar7;
  case 3:
    puVar7[3] = param_1[3];
    FUN_1000aad0(param_1,(int)puVar7);
    return puVar7;
  case 4:
    puVar7[3] = param_1[3];
    FUN_1000aad0(param_1,(int)puVar7);
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
      pvVar5 = (void *)FUN_10001110(-(uint)((int)((ulonglong)lVar3 >> 0x20) != 0) | (uint)lVar3);
      puVar7[2] = pvVar5;
      FUN_10008e80(pvVar5,(void *)param_1[2],iVar9 * 2 + 2);
      FUN_1000aad0(param_1,(int)puVar7);
      return puVar7;
    }
    break;
  case 6:
    *(undefined1 *)(puVar7 + 3) = *(undefined1 *)(param_1 + 3);
    *(undefined1 *)((int)puVar7 + 0xd) = *(undefined1 *)((int)param_1 + 0xd);
    *(undefined1 *)((int)puVar7 + 0xe) = *(undefined1 *)((int)param_1 + 0xe);
    *(undefined1 *)((int)puVar7 + 0xf) = *(undefined1 *)((int)param_1 + 0xf);
    FUN_1000aad0(param_1,(int)puVar7);
    return puVar7;
  case 7:
    pvVar5 = (void *)FUN_10001110(8);
    puVar7[1] = pvVar5;
    FUN_10008e80(pvVar5,(void *)param_1[1],8);
  }
  FUN_1000aad0(param_1,(int)puVar7);
  return puVar7;
}


// 1000b0a0 FUN_1000b0a0

void __fastcall FUN_1000b0a0(int param_1)

{
  int *piVar1;
  
  if (param_1 != 0) {
    FUN_1000a810(param_1);
    piVar1 = (int *)KeyValuesSystem();
    (**(code **)(*piVar1 + 8))(param_1);
  }
  return;
}


// 1000b0c0 FUN_1000b0c0

void __thiscall FUN_1000b0c0(void *this,int param_1)

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
        pbVar5 = &DAT_1001a66b;
      }
      else {
        piVar3 = (int *)KeyValuesSystem();
        pbVar5 = (byte *)(**(code **)(*piVar3 + 0x10))(*puVar1);
      }
      iVar6 = FUN_10008ab0(pbVar5,pbVar4);
      if (iVar6 == 0) {
        FUN_1000b0c0(puVar7,(int)puVar1);
        goto LAB_1000b162;
      }
    }
    puVar7 = FUN_1000aef0(puVar1);
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
LAB_1000b162:
    puVar1 = (undefined4 *)puVar1[5];
  } while( true );
}


// 1000b180 FUN_1000b180

int * __fastcall FUN_1000b180(int param_1)

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
    piVar2 = FUN_1000ac50(this,unaff_retaddr);
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
      FUN_1000a810((int)piVar2);
      piVar3 = (int *)KeyValuesSystem();
      (**(code **)(*piVar3 + 8))(piVar2);
    }
  }
  return (int *)0x0;
}


// 1000b210 FUN_1000b210

int __thiscall FUN_1000b210(void *this,char *param_1,int param_2)

{
  int *piVar1;
  int iVar2;
  char local_240 [64];
  CHAR local_200 [512];
  
  piVar1 = FUN_1000aca0(this,param_1,0);
  if (piVar1 != (int *)0x0) {
    switch((char)piVar1[4]) {
    case '\x01':
switchD_1000b244_caseD_1:
      return piVar1[1];
    case '\x02':
    case '\x04':
      FUN_10008f20(local_240,0x40,"%d");
      FUN_1000ae80(this,param_1,local_240);
      return piVar1[1];
    case '\x03':
      FUN_10008f20(local_240,0x40,"%f");
      FUN_1000ae80(this,param_1,local_240);
      return piVar1[1];
    case '\x05':
      iVar2 = FUN_10008fc0((LPCWSTR)piVar1[2],local_200,0x200);
      if (iVar2 != 0) {
        FUN_1000ae80(this,param_1,local_200);
        goto switchD_1000b244_caseD_1;
      }
      break;
    case '\a':
      FUN_10008f20(local_240,0x40,"%lld");
      FUN_1000ae80(this,param_1,local_240);
      return piVar1[1];
    }
  }
  return param_2;
}


// 1000b360 FUN_1000b360

void __thiscall FUN_1000b360(void *this,int *param_1)

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


// 1000b3a0 FUN_1000b3a0

void __thiscall FUN_1000b3a0(void *this,undefined4 param_1,int *param_2)

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
  if (DAT_1001f50c < 0x40) {
    *(undefined4 *)(&DAT_1001f408 + DAT_1001f50c * 4) = uVar6;
  }
  iVar2 = DAT_1001f50c + 1;
  if (DAT_1001f510 <= DAT_1001f50c) {
    DAT_1001f510 = DAT_1001f50c;
  }
  local_1c = this;
  if (100 < DAT_1001f50c) {
    DAT_1001f50c = iVar2;
    FUN_1000a730(&DAT_1001f408,"RecursiveLoadFromBuffer:  recursion overflow");
    DAT_1001f50c = DAT_1001f50c + -1;
    return;
  }
  if (iVar2 < 0x40) {
    DAT_1001f50c = iVar2;
    *(undefined4 *)(&DAT_1001f408 + iVar2 * 4) = 0xffffffff;
    iVar2 = DAT_1001f50c;
  }
  DAT_1001f50c = iVar2 + 1;
  if (DAT_1001f510 <= iVar2) {
    DAT_1001f510 = iVar2;
  }
  local_25 = '\x01';
  local_18 = iVar2;
  pcVar3 = FUN_1000a8d0(this,param_2,&local_26,&local_27);
  do {
    if (pcVar3 == (char *)0x0) {
      pcVar3 = "RecursiveLoadFromBuffer:  got EOF instead of keyname";
LAB_1000b7a0:
      FUN_1000a730(&DAT_1001f408,pcVar3);
LAB_1000b7aa:
      DAT_1001f50c = DAT_1001f50c + -2;
      return;
    }
    if (*pcVar3 == '\0') {
      pcVar3 = "RecursiveLoadFromBuffer:  got empty keyname";
      goto LAB_1000b7a0;
    }
    if ((*pcVar3 == '}') && (local_26 == '\0')) goto LAB_1000b7aa;
    this_00 = FUN_1000b180((int)this);
    local_10 = this_00;
    if (this_00 == (int *)0x0) {
LAB_1000b78d:
      pcVar3 = "RecursiveLoadFromBuffer:  got NULL key";
      goto LAB_1000b7a0;
    }
    if (iVar2 < 0x40) {
      *(int *)(&DAT_1001f408 + iVar2 * 4) = *this_00;
    }
    _Str = FUN_1000a8d0(this,param_2,&local_26,&local_27);
    if (local_27 != '\0') {
      local_20 = _Str;
      if (_Str == (byte *)0x0) goto LAB_1000b78d;
      local_25 = FUN_1000abb0();
      _Str = FUN_1000a8d0(this,param_2,&local_26,&local_27);
    }
    local_20 = _Str;
    if (_Str == (byte *)0x0) goto LAB_1000b78d;
    if ((*_Str == 0x7d) && (local_26 == '\0')) {
      pcVar3 = "RecursiveLoadFromBuffer:  got } in key";
      goto LAB_1000b7a0;
    }
    if ((*_Str == 0x7b) && (local_26 == '\0')) {
      if (local_18 < 0x40) {
        *(undefined4 *)(&DAT_1001f408 + local_18 * 4) = 0xffffffff;
      }
      FUN_1000b3a0(this_00,param_1,param_2);
    }
    else {
      if (local_27 != '\0') {
        pcVar3 = "RecursiveLoadFromBuffer:  got conditional between key and value";
        goto LAB_1000b7a0;
      }
      if (this_00[1] != 0) {
        FUN_100012c0(this_00[1]);
        this_00[1] = 0;
      }
      iVar2 = FUN_10008a80((char *)_Str);
      local_c = iVar2;
      local_14 = _strtol((char *)_Str,(char **)&local_4,10);
      fVar7 = (float10)FUN_1000f38d(_Str,&local_8);
      if (*_Str == 0) {
LAB_1000b58a:
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
        plVar5 = (longlong *)FUN_10001110(8);
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
        if (local_4 != _Str + iVar2) goto LAB_1000b58a;
        this_00[3] = local_14;
        *(undefined1 *)(this_00 + 4) = 2;
      }
      if ((char)this_00[4] == '\x01') {
        pvVar4 = (void *)FUN_10001110(iVar2 + 1U);
        this_00[1] = (int)pvVar4;
        FUN_10008e80(pvVar4,_Str,iVar2 + 1U);
      }
      iVar2 = param_2[3];
      FUN_1000a8d0(local_1c,param_2,&local_26,&local_27);
      if (local_27 == '\0') {
        FUN_1000c190(param_2,0,iVar2);
        this = local_1c;
      }
      else {
        local_25 = FUN_1000abb0();
        this = local_1c;
      }
    }
    if (local_25 == '\0') {
      FUN_1000aa60(this,(int)this_00);
      FUN_1000b0a0((int)this_00);
    }
    local_25 = '\x01';
    pcVar3 = FUN_1000a8d0(this,param_2,&local_26,&local_27);
    iVar2 = local_18;
  } while( true );
}


// 1000b7c0 FUN_1000b7c0

undefined1 __fastcall FUN_1000b7c0(int *param_1)

{
  undefined1 uVar1;
  char cVar2;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) != 0) {
    FUN_1000cfa0(param_1,"%c");
    return 0;
  }
  cVar2 = FUN_1000c080(param_1,1);
  if (cVar2 != '\0') {
    uVar1 = *(undefined1 *)((param_1[3] - param_1[8]) + *param_1);
    param_1[3] = param_1[3] + 1;
    return uVar1;
  }
  return 0;
}


// 1000b810 FUN_1000b810

undefined1 __thiscall FUN_1000b810(void *this,int *param_1,undefined *param_2)

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
    PTR_s_unknown_1001f3f0 = param_2;
    uStack_5c = 0x1000b858;
    pvStack_58 = this_00;
    iVar3 = (**(code **)(*param_1 + 0x1c))();
    uStack_5c = 0;
    uStack_60 = 0;
    piVar1 = param_1 + -1;
    puStack_64 = auStack_c;
    uVar4 = iVar3 + 2;
    auStack_70[1] = 0x1000b879;
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
      FUN_1000c740(auStack_70,0,0,0);
      iVar3 = FUN_10008a80(pcVar5);
      FUN_1000c7c0(auStack_70,(int)pcVar5,(int)this,iVar3,9);
      FUN_1000ba90(this_00,pcStack_38,(int *)auStack_70,(int)param_1,(void *)0x0);
      if ((-1 < (int)pvStack_68) && (auStack_70[0] != 0)) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(auStack_70[0]);
      }
    }
    (**(code **)(*piVar1 + 0x158))(pcVar5);
    return unaff_SI;
  }
  return 0;
}


// 1000b970 FUN_1000b970

void __thiscall FUN_1000b970(void *this,char *param_1,char *param_2,int param_3,void *param_4)

{
  char cVar1;
  int iVar2;
  int *piVar3;
  void *this_00;
  undefined4 *this_01;
  undefined1 auStack_204 [3];
  char acStack_201 [513];
  
  if (param_3 != 0) {
    FUN_10008ee0(acStack_201 + 1,param_1,0x200);
    for (iVar2 = FUN_10008a80(acStack_201 + 1);
        ((0 < iVar2 && (acStack_201[iVar2] != '\\')) && (acStack_201[iVar2] != '/'));
        iVar2 = iVar2 + -1) {
      acStack_201[iVar2] = '\0';
    }
    FUN_10008dc0(acStack_201 + 1,param_2,0x200,0xffffffff);
    piVar3 = (int *)KeyValuesSystem();
    this_00 = (void *)(**(code **)(*piVar3 + 4))(0x24);
    if (this_00 == (void *)0x0) {
      this_01 = (undefined4 *)0x0;
    }
    else {
      this_01 = FUN_1000ac50(this_00,auStack_204);
    }
    *(bool *)((int)this_01 + 0x11) = *(char *)((int)this + 0x11) != '\0';
    cVar1 = FUN_1000b810(this_01,(int *)param_3,auStack_204);
    if (cVar1 != '\0') {
      FUN_100041d0(param_4,*(int *)((int)param_4 + 0xc),(undefined4 *)&stack0xfffffdf8);
      return;
    }
    DevMsg("KeyValues::ParseIncludedKeys: Couldn\'t load included keyvalue file %s\n",auStack_204);
    FUN_1000a810((int)this_01);
    piVar3 = (int *)KeyValuesSystem();
    (**(code **)(*piVar3 + 8))(this_01);
  }
  return;
}


// 1000ba90 FUN_1000ba90

undefined4 __thiscall FUN_1000ba90(void *this,char *param_1,int *param_2,int param_3,void *param_4)

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
  if (DVar4 != DAT_10021c14) {
    DVar5 = 0;
    LOCK();
    DVar3 = DVar4;
    if (DAT_10021c14 != 0) {
      DVar5 = DAT_10021c14;
      DVar3 = DAT_10021c14;
    }
    DAT_10021c14 = DVar3;
    UNLOCK();
    if (DVar5 != 0) {
      CThreadFastMutex::Lock((CThreadFastMutex *)&DAT_10021c14,DVar4,0);
      goto LAB_1000bad7;
    }
  }
  DAT_10021c18 = DAT_10021c18 + 1;
LAB_1000bad7:
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
  PTR_DAT_1001f508 = param_1;
  DAT_1001f510 = 0;
  this_01 = (undefined4 *)this;
  do {
    cStack_2d = '\x01';
    pbVar6 = FUN_1000a8d0(this,param_2,&cStack_2f,&cStack_2e);
    if ((((char)param_2[5] != '\0') || (pbVar6 == (byte *)0x0)) ||
       ((cStack_2f == '\0' && (*pbVar6 == 0)))) break;
    uVar7 = FUN_10008b10(pbVar6,(byte *)"#include");
    puVar12 = this_01;
    if (uVar7 == 0) {
      pcVar8 = FUN_1000a8d0(this,param_2,&cStack_2f,&cStack_2e);
      if ((pcVar8 == (char *)0x0) || (*pcVar8 == '\0')) {
        FUN_1000a730(&DAT_1001f408,"#include is NULL ");
      }
      else {
        FUN_1000b970(this,param_1,pcVar8,param_3,param_4);
      }
    }
    else {
      uVar7 = FUN_10008b10(pbVar6,(byte *)"#base");
      if (uVar7 == 0) {
        pcVar8 = FUN_1000a8d0(this,param_2,&cStack_2f,&cStack_2e);
        if ((pcVar8 == (char *)0x0) || (*pcVar8 == '\0')) {
          FUN_1000a730(&DAT_1001f408,"#base is NULL ");
        }
        else {
          FUN_1000b970(this,param_1,pcVar8,param_3,param_4);
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
            this_01 = FUN_1000ac50(this_00,pbVar6);
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
        pcVar8 = FUN_1000a8d0(this,param_2,&cStack_2f,&cStack_2e);
        if (cStack_2e != '\0') {
          cStack_2d = FUN_1000abb0();
          pcVar8 = FUN_1000a8d0(this,param_2,&cStack_2f,&cStack_2e);
        }
        if (((pcVar8 == (char *)0x0) || (*pcVar8 != '{')) || (cStack_2f != '\0')) {
          FUN_1000a730(&DAT_1001f408,"LoadFromBuffer: missing {");
        }
        else {
          FUN_1000b3a0(this_01,param_1,param_2);
        }
        if (cStack_2d == '\0') {
          if (puStack_2c != (undefined4 *)0x0) {
            puStack_2c[5] = 0;
          }
          iVar1 = this_01[6];
          if (iVar1 != 0) {
            FUN_1000a810(iVar1);
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
  FUN_1000b360(this,(int *)&pDStack_28);
  iVar13 = iStack_14;
  iVar2 = iStack_8;
  iVar1 = iStack_1c;
  while (iVar1 = iVar1 + -1, iStack_14 = iVar13, iStack_8 = iVar2, 0 < iVar1) {
    DVar4 = pDStack_28[iVar1];
    if (DVar4 != 0) {
      FUN_1000a810(DVar4);
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
      FUN_1000b0c0(this,*(int *)(iVar13 + iVar11 * 4));
      iVar11 = iVar11 + 1;
      iVar1 = iStack_c;
    } while (iVar11 < iVar2);
  }
  while (iVar2 = iVar2 + -1, iStack_c = iVar1, -1 < iVar2) {
    iVar11 = *(int *)(iVar13 + iVar2 * 4);
    if (iVar11 != 0) {
      FUN_1000a810(iVar11);
      piVar10 = (int *)KeyValuesSystem();
      (**(code **)(*piVar10 + 8))(iVar11);
      iVar1 = iStack_c;
    }
  }
  PTR_DAT_1001f508 = &DAT_1001a66b;
  DAT_1001f510 = 0;
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
  DAT_10021c18 = DAT_10021c18 + -1;
  if (DAT_10021c18 == 0) {
    pDStack_28 = &DAT_10021c14;
    LOCK();
    DAT_10021c14 = 0;
    UNLOCK();
  }
  return CONCAT31((int3)((uint)pDStack_28 >> 8),1);
}


// 1000bea0 FUN_1000bea0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1000bea0(int param_1,int param_2)

{
  int iVar1;
  int iVar2;
  
  if (DAT_10021cd4 == '\0') {
    iVar2 = 0;
    DAT_10021cd4 = '\x01';
    iVar1 = DAT_10021ccc;
    if (0 < param_2) {
      do {
        if (iVar1 == 0) {
          iVar1 = (**(code **)(param_1 + iVar2 * 4))("VEngineCvar007",0);
          _DAT_10021cc8 = iVar1;
          DAT_10021ccc = iVar1;
        }
        if (DAT_10021cd0 == 0) {
          DAT_10021cd0 = (**(code **)(param_1 + iVar2 * 4))("VProcessUtils001",0);
          iVar1 = DAT_10021ccc;
        }
        iVar2 = iVar2 + 1;
      } while (iVar2 < param_2);
    }
  }
  return;
}


// 1000bf10 FUN_1000bf10

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_1000bf10(void)

{
  if (DAT_10021cd4 != '\0') {
    _DAT_10021cc8 = 0;
    DAT_10021ccc = 0;
    DAT_10021cd0 = 0;
    DAT_10021cd4 = '\0';
  }
  return;
}


// 1000bf30 FUN_1000bf30

undefined * FUN_1000bf30(void)

{
  return &DAT_10021ce8;
}


// 1000bf40 FUN_1000bf40

undefined * FUN_1000bf40(void)

{
  return &DAT_100226f8;
}


// 1000bf70 FUN_1000bf70

undefined4 * __thiscall
FUN_1000bf70(void *this,undefined1 param_1,char *param_2,int param_3,int param_4)

{
  int *piVar1;
  char cVar2;
  char *pcVar3;
  int iVar4;
  int iVar5;
  
  *(undefined1 *)((int)this + 4) = param_1;
  *(undefined ***)this = &PTR_FUN_1001b98c;
  *(char **)((int)this + 8) = param_2;
  *(int *)((int)this + 0x10) = param_3;
  iVar4 = FUN_10008a80(param_2);
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
      iVar4 = FUN_10008a80(pcVar3);
      *piVar1 = iVar4;
      if (*(int *)((int)this + 0x14) < iVar4) {
        *(int *)((int)this + 0x14) = iVar4;
      }
      iVar5 = iVar5 + 1;
    } while (iVar5 < param_3);
  }
  return (undefined4 *)this;
}


// 1000c000 FUN_1000c000

uint __thiscall FUN_1000c000(void *this,byte *param_1,undefined4 *param_2)

{
  undefined4 uVar1;
  uint in_EAX;
  int iVar2;
  
  iVar2 = 0;
  if (0 < *(int *)((int)this + 0x10)) {
    do {
      in_EAX = FUN_10008ab0(param_1,*(byte **)((int)this +
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


// 1000c060 FUN_1000c060

undefined1 FUN_1000c060(void)

{
  return 0;
}


// 1000c080 FUN_1000c080

undefined1 __thiscall FUN_1000c080(void *this,int param_1)

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


// 1000c0e0 FUN_1000c0e0

uint __thiscall FUN_1000c0e0(void *this,int param_1,int *param_2)

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
    FUN_1000c080(this,*param_2 + param_1);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
  }
  iVar2 = *(int *)((int)this + 0x1c) - *(int *)((int)this + 0xc);
  if (iVar2 < *param_2) {
    *param_2 = iVar2;
  }
  return (uint)(*param_2 != 0);
}


// 1000c150 FUN_1000c150

int __thiscall FUN_1000c150(void *this,int param_1,int param_2)

{
  char cVar1;
  
  if ((*(byte *)((int)this + 0x14) & 2) == 0) {
    cVar1 = FUN_1000c080(this,param_1 + param_2);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
    if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
      return (*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this + param_2;
    }
  }
  return 0;
}


// 1000c190 FUN_1000c190

void __thiscall FUN_1000c190(void *this,int param_1,int param_2)

{
  int iVar1;
  
  if (param_1 != 0) {
    if (param_1 == 1) {
      *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
      goto LAB_1000c1bc;
    }
    if (param_1 != 2) goto LAB_1000c1bc;
    param_2 = *(int *)((int)this + 0x1c) - param_2;
  }
  *(int *)((int)this + 0xc) = param_2;
LAB_1000c1bc:
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


// 1000c1f0 FUN_1000c1f0

uint __thiscall FUN_1000c1f0(void *this,int param_1)

{
  uint in_EAX;
  
  if (((*(byte *)((int)this + 0x14) & 1) == 0) && ((*(byte *)((int)this + 0x15) & 8) == 0)) {
    if ((*(int *)((int)this + 0x10) < *(int *)((int)this + 0x20)) ||
       (in_EAX = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1,
       *(int *)((int)this + 4) < (int)in_EAX)) {
      in_EAX = (**(code **)((int)this + 0x28))(param_1);
      if ((char)in_EAX == '\0') {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 1;
        goto LAB_1000c226;
      }
    }
    return CONCAT31((int3)(in_EAX >> 8),1);
  }
LAB_1000c226:
  return in_EAX & 0xffffff00;
}


// 1000c240 FUN_1000c240

void __fastcall FUN_1000c240(int *param_1)

{
  undefined4 uVar1;
  
  if (param_1[7] < param_1[4]) {
    if (((*(byte *)((int)param_1 + 0x15) & 8) == 0) && ((*(byte *)(param_1 + 5) & 1) == 0)) {
      uVar1 = FUN_1000c1f0(param_1,1);
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


// 1000c290 FUN_1000c290

void __thiscall FUN_1000c290(void *this,undefined4 param_1)

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


// 1000c2e0 FUN_1000c2e0

void __thiscall FUN_1000c2e0(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000c33b;
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
LAB_1000c33b:
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


// 1000c370 FUN_1000c370

void __thiscall FUN_1000c370(void *this,void *param_1,size_t param_2)

{
  char cVar1;
  
  cVar1 = FUN_1000c080(this,param_2);
  if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    _memcpy(param_1,(void *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this),
            param_2);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
  }
  return;
}


// 1000c3b0 FUN_1000c3b0

void __fastcall FUN_1000c3b0(int *param_1)

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


// 1000c420 FUN_1000c420

int __thiscall FUN_1000c420(void *this,int param_1)

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


// 1000c4c0 FUN_1000c4c0

int __fastcall FUN_1000c4c0(int *param_1)

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
    local_8 = FUN_1000c420(param_1,0);
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
      if (iVar6 < 1) goto LAB_1000c5c5;
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
LAB_1000c5c5:
      local_8 = local_8 + iVar6;
    }
  }
  if (local_8 != iVar1) {
    return (local_8 - iVar1) + 1;
  }
  return 0;
}


// 1000c620 FUN_1000c620

uint __thiscall FUN_1000c620(void *this,int param_1,char *param_2,int param_3)

{
  char cVar1;
  uint in_EAX;
  undefined3 extraout_var;
  int iVar2;
  
  if ((*(byte *)((int)this + 0x14) & 2) == 0) {
    cVar1 = FUN_1000c080(this,param_1 + param_3);
    in_EAX = CONCAT31(extraout_var,cVar1);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
    if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
      iVar2 = FUN_10008b90((char *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this
                                   + param_1),param_2,param_3);
      return (uint)(iVar2 == 0);
    }
  }
  return in_EAX & 0xffffff00;
}


// 1000c680 FUN_1000c680

void __thiscall FUN_1000c680(void *this,void *param_1,size_t param_2)

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
    FUN_1000c240((int *)this);
  }
  return;
}


// 1000c6f0 FUN_1000c6f0

uint __thiscall FUN_1000c6f0(void *this,int param_1)

{
  uint in_EAX;
  int iVar1;
  
  if (*(int *)((int)this + 8) < 0) {
    if ((*(byte *)((int)this + 0x15) & 2) == 0) {
      return in_EAX & 0xffffff00;
    }
    FUN_1000c290(this,0);
  }
  iVar1 = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1;
  if (*(int *)((int)this + 4) < iVar1) {
    do {
      iVar1 = FUN_1000c2e0(this,1);
    } while (*(int *)((int)this + 4) <
             (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1);
  }
  return CONCAT31((int3)((uint)iVar1 >> 8),1);
}


// 1000c740 FUN_1000c740

int * __thiscall FUN_1000c740(void *this,int param_1,int param_2,byte param_3)

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
    FUN_1000c240((int *)this);
  }
  *(code **)((int)this + 0x24) = FUN_1000c060;
  *(code **)((int)this + 0x28) = FUN_1000c6f0;
  return (int *)this;
}


// 1000c7c0 FUN_1000c7c0

void __thiscall FUN_1000c7c0(void *this,int param_1,int param_2,int param_3,byte param_4)

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
      uVar1 = FUN_1000c1f0(this,1);
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


// 1000c860 FUN_1000c860

void __thiscall FUN_1000c860(void *this,char *param_1)

{
  int iVar1;
  char *pcVar2;
  size_t sVar3;
  
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    if (param_1 != (char *)0x0) {
      iVar1 = FUN_10008a80(param_1);
      FUN_1000c680(this,param_1,iVar1 + 1);
      return;
    }
    FUN_10003390(this,0);
    return;
  }
  if (param_1 != (char *)0x0) {
    if (((*(byte *)((int)this + 0x15) & 0x10) == 0) && (0 < *(int *)((int)this + 0x18))) {
                    /* WARNING: Load size is inaccurate */
      if ((*(int *)((int)this + 0x10) != 0) &&
         (*(char *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + -1 + *this) == '\n')
         ) {
        FUN_10003c40((int *)this);
      }
      pcVar2 = _strchr(param_1,10);
      while (pcVar2 != (char *)0x0) {
        FUN_1000c680(this,param_1,(size_t)(pcVar2 + (1 - (int)param_1)));
        param_1 = pcVar2 + 1;
        if (pcVar2[1] == '\0') break;
        if ((*(byte *)((int)this + 0x15) & 0x10) == 0) {
          iVar1 = *(int *)((int)this + 0x18);
        }
        else {
          iVar1 = 0;
        }
        while (iVar1 = iVar1 + -1, -1 < iVar1) {
          FUN_10003390(this,9);
        }
        pcVar2 = _strchr(param_1,10);
      }
    }
    sVar3 = FUN_10008a80(param_1);
    if (sVar3 != 0) {
      FUN_1000c680(this,param_1,sVar3);
    }
  }
  return;
}


// 1000c950 FUN_1000c950

void __cdecl FUN_1000c950(void *param_1,char *param_2)

{
  char local_800 [2048];
  
  FUN_10008f70(local_800,0x800,param_2,&stack0x0000000c);
  FUN_1000c860(param_1,local_800);
  return;
}


// 1000c990 FUN_1000c990

uint __fastcall FUN_1000c990(int *param_1)

{
  undefined3 uVar1;
  char cVar2;
  char cVar3;
  char *in_EAX;
  undefined3 extraout_var;
  undefined3 extraout_var_00;
  undefined3 extraout_var_01;
  
  if (((*(byte *)((int)param_1 + 0x15) & 1) != 0) && ((char)param_1[5] == '\0')) {
    cVar2 = FUN_1000c080(param_1,2);
    in_EAX = (char *)CONCAT31(extraout_var,cVar2);
    *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) & 0xfd;
    if (cVar2 != '\0') {
      in_EAX = (char *)((param_1[3] - param_1[8]) + *param_1);
      if (((in_EAX != (char *)0x0) && (*in_EAX == '/')) && (in_EAX[1] == '/')) {
        param_1[3] = param_1[3] + 2;
        cVar3 = FUN_1000b7c0(param_1);
        cVar2 = (char)param_1[5];
        uVar1 = extraout_var_00;
        while ((cVar2 == '\0' && (cVar3 != '\n'))) {
          cVar3 = FUN_1000b7c0(param_1);
          cVar2 = (char)param_1[5];
          uVar1 = extraout_var_01;
        }
        return CONCAT31(uVar1,1);
      }
    }
  }
  return (uint)in_EAX & 0xffffff00;
}


// 1000ca00 FUN_1000ca00

void __thiscall FUN_1000ca00(void *this,undefined1 *param_1,int param_2)

{
  int iVar1;
  
  if (*(char *)((int)this + 0x14) != '\0') {
    *param_1 = 0;
    return;
  }
  if (param_2 == 0) {
    param_2 = 0x7fffffff;
  }
  iVar1 = FUN_1000c4c0((int *)this);
  if ((*(byte *)((int)this + 0x15) & 1) != 0) {
    FUN_1000c3b0((int *)this);
  }
  if (iVar1 == 0) {
    *param_1 = 0;
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
    return;
  }
  if (param_2 < iVar1) {
    FUN_1000c370(this,param_1,param_2 - 1);
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
    FUN_1000c370(this,param_1,iVar1 - 1);
    param_1[iVar1 + -1] = 0;
  }
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    FUN_1000b7c0((int *)this);
  }
  return;
}


// 1000cac0 FUN_1000cac0

uint __thiscall FUN_1000cac0(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  char cVar2;
  undefined3 extraout_var;
  uint uVar3;
  int iVar4;
  int unaff_EBX;
  
  cVar2 = FUN_1000b7c0((int *)this);
  puVar1 = param_1;
  iVar4 = CONCAT31(extraout_var,cVar2);
  if (cVar2 == *(char *)(param_1 + 1)) {
    param_1 = (undefined4 *)param_1[5];
    uVar3 = FUN_1000c0e0(this,0,(int *)&param_1);
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


// 1000cb50 FUN_1000cb50

void __thiscall FUN_1000cb50(void *this,undefined4 *param_1,undefined1 *param_2,int param_3)

{
  char cVar1;
  int iVar2;
  uint uVar3;
  int iVar4;
  
  if (((*(byte *)((int)this + 0x15) & 1) == 0) || (param_1 == (undefined4 *)0x0)) {
    FUN_1000ca00(this,param_2,param_3);
  }
  else {
    if (*(char *)((int)this + 0x14) != '\0') {
      *param_2 = 0;
      return;
    }
    if (param_3 == 0) {
      param_3 = 0x7fffffff;
    }
    FUN_1000c3b0((int *)this);
    uVar3 = FUN_1000c620(this,0,(char *)param_1[2],param_1[3]);
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
LAB_1000cc4a:
          if (param_3 <= iVar4) {
            iVar4 = param_3 + -1;
          }
          param_2[iVar4] = 0;
          return;
        }
        uVar3 = FUN_1000c620(this,0,(char *)param_1[2],param_1[3]);
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
          goto LAB_1000cc4a;
        }
        uVar3 = FUN_1000cac0(this,param_1);
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


// 1000cc80 FUN_1000cc80

int __thiscall FUN_1000cc80(void *this,char *param_1,int param_2)

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
        FUN_1000c3b0((int *)this);
        goto LAB_1000cefb;
      }
      if (cVar5 != '%') {
        if ((*(byte *)((int)this + 0x14) & 2) != 0) {
          return local_8;
        }
        cVar4 = FUN_1000c080(this,1);
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
        if (cVar4 == '\0') {
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        if (cVar5 != *(char *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this)) {
          return local_8;
        }
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
        goto LAB_1000cefb;
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
LAB_1000cf26:
          *puVar2 = 0;
          return local_8;
        }
        cVar5 = FUN_1000c080(this,1);
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
        if (cVar5 == '\0') goto LAB_1000cf26;
                    /* WARNING: Load size is inaccurate */
        *puVar2 = *(undefined1 *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc));
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
        local_8 = local_8 + 1;
        break;
      case 'd':
      case 'i':
        puVar9 = *(ulong **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000c0e0(this,0,&param_2);
        if ((char)uVar6 == '\0') {
LAB_1000cf37:
          *puVar9 = 0;
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        uVar7 = _strtol((char *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc)),
                        &local_4,10);
        goto LAB_1000cdc6;
      default:
        goto switchD_1000cd42_caseD_65;
      case 'f':
        pfVar3 = *(float **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000c0e0(this,0,&param_2);
        if ((char)uVar6 == '\0') {
          *pfVar3 = 0.0;
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        fVar11 = (float10)FUN_1000f38d((byte *)((*this - *(int *)((int)this + 0x20)) +
                                               *(int *)((int)this + 0xc)),&local_4);
        *pfVar3 = (float)fVar11;
        goto LAB_1000cdcc;
      case 's':
        puVar1 = (undefined4 *)(iVar10 + 4);
        iVar10 = iVar10 + 4;
        FUN_1000ca00(this,(undefined1 *)*puVar1,0);
        local_8 = local_8 + 1;
        break;
      case 'u':
        puVar9 = *(ulong **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000c0e0(this,0,&param_2);
        if ((char)uVar6 == '\0') goto LAB_1000cf37;
                    /* WARNING: Load size is inaccurate */
        uVar7 = _strtoul((char *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc)),
                         &local_4,10);
LAB_1000cdc6:
        *puVar9 = uVar7;
LAB_1000cdcc:
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
        uVar6 = FUN_1000c0e0(this,0,&param_2);
        if ((char)uVar6 == '\0') goto LAB_1000cf37;
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
LAB_1000cefb:
      cVar5 = *pcVar8;
      param_1 = pcVar8;
    } while (cVar5 != '\0');
  }
switchD_1000cd42_caseD_65:
  return local_8;
}


// 1000cfa0 FUN_1000cfa0

void __cdecl FUN_1000cfa0(void *param_1,char *param_2)

{
  FUN_1000cc80(param_1,param_2,(int)&stack0x0000000c);
  return;
}


// 1000cfc0 FUN_1000cfc0

void __cdecl FUN_1000cfc0(undefined4 param_1,undefined **param_2)

{
  int *piVar1;
  int *piVar2;
  
  if ((DAT_10021ccc != (int *)0x0) && (DAT_10023114 == '\0')) {
    DAT_10023110 = param_1;
    DAT_10023114 = '\x01';
    DAT_1001f5f4 = (**(code **)(*DAT_10021ccc + 0x14))();
    piVar2 = DAT_10023108;
    DAT_1002310c = param_2;
    if (param_2 == (undefined **)0x0) {
      DAT_1002310c = &PTR_PTR_1001f600;
    }
    while (piVar2 != (int *)0x0) {
      piVar1 = (int *)piVar2[1];
      (**(code **)(*piVar2 + 0xc))(DAT_10023110);
      (**(code **)(*piVar2 + 0x2c))();
      piVar2 = piVar1;
    }
    (**(code **)(*DAT_10021ccc + 0x70))();
    (**(code **)(*DAT_10021ccc + 0x94))();
    DAT_10023108 = (int *)0x0;
  }
  return;
}


// 1000d130 FUN_1000d130

void __fastcall FUN_1000d130(undefined4 param_1)

{
  if (DAT_1002310c != (undefined4 *)0x0) {
    (**(code **)*DAT_1002310c)(param_1);
  }
  return;
}


// 1000d240 FUN_1000d240

void __fastcall FUN_1000d240(int *param_1)

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
  if ((cVar1 != '\0') && (DAT_10021ccc != (int *)0x0)) {
    cVar1 = (**(code **)(*DAT_10021ccc + 0x80))();
    if (cVar1 == '\0') {
      (**(code **)(*DAT_10021ccc + 0x8c))(param_1,unaff_retaddr);
      return;
    }
  }
  aiStack_28[0] = param_1[0xb];
  piVar4 = unaff_retaddr;
  if (unaff_retaddr == (int *)0x0) {
    piVar4 = (int *)&DAT_1001a66b;
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
    FUN_10008f20((char *)aiStack_28,0x20,"%f");
    piVar4 = aiStack_28;
    uVar2 = extraout_ECX_00;
    uVar3 = extraout_EDX_00;
  }
  param_1[0xb] = 0xb00000;
  uVar5 = FUN_1000e380(uVar2,uVar3);
  param_1[0xc] = (int)uVar5;
  if ((param_1[5] & 0x1000U) == 0) {
    (**(code **)(*param_1 + 0x40))(piVar4,fVar7);
  }
  return;
}


// 1000d320 FUN_1000d320

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

void __thiscall FUN_1000d320(void *this,undefined4 param_1,undefined4 param_2)

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
  
  asStack_1c[2] = 0x1000d336;
  iVar2 = -(*(int *)((int)this + 0x28) + 0xfU & 0xfffffff0);
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x28);
  *(undefined4 *)((int)asStack_1c + iVar2 + 4) = uVar4;
  *(undefined1 **)((int)asStack_1c + iVar2) = &stack0xfffffff0 + iVar2;
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = 0x1000d343;
  _memcpy(*(void **)((int)asStack_1c + iVar2),*(void **)((int)asStack_1c + iVar2 + 4),
          *(size_t *)((int)asStack_1c + iVar2 + 8));
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = param_1;
  *(undefined4 *)((int)auStack_24 + iVar2) = 0x1000d34c;
  iVar3 = FUN_10008a80(*(char **)((int)auStack_24 + iVar2 + 4));
  iVar3 = iVar3 + 1;
  puVar6 = &stack0xfffffff0 + iVar2;
  if (*(int *)((int)this + 0x28) < iVar3) {
    if (*(int *)((int)this + 0x24) != 0) {
      *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x24);
      *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x1000d366;
      FUN_100012c0(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    }
    puVar6 = &stack0xfffffff0 + iVar2;
    *(int *)((int)asStack_1c + iVar2 + 8) = iVar3;
    *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x1000d36f;
    uVar4 = FUN_10001110(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    *(undefined4 *)((int)this + 0x24) = uVar4;
    *(int *)((int)this + 0x28) = iVar3;
  }
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)(puVar6 + -4) = iVar3;
  *(undefined4 *)(puVar6 + -8) = param_1;
  *(undefined4 *)(puVar6 + -0xc) = uVar4;
  *(undefined4 *)(puVar6 + -0x10) = 0x1000d386;
  _memcpy(*(void **)(puVar6 + -0xc),*(void **)(puVar6 + -8),*(size_t *)(puVar6 + -4));
  pcVar1 = *(code **)((int)this + 0x44);
  if (pcVar1 != (code *)0x0) {
    *(undefined4 *)(puVar6 + -4) = extraout_ECX;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(int *)(puVar6 + -0xc) = (int)this + 0x18;
    puVar5 = puVar6 + -0x10;
    *(undefined4 *)(puVar6 + -0x10) = 0x1000d39e;
    (*pcVar1)();
    puVar6 = puVar5 + 0xc;
  }
  if (DAT_10021ccc != (int *)0x0) {
    pcVar1 = *(code **)(*DAT_10021ccc + 0x48);
    *(int **)(puVar6 + -4) = DAT_10021ccc;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(void **)(puVar6 + -0xc) = this;
    *(undefined4 *)(puVar6 + -0x10) = 0x1000d3bb;
    (*pcVar1)();
  }
  return;
}


// 1000d420 FUN_1000d420

void __thiscall FUN_1000d420(void *this,float param_1)

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
    if (((cVar1 != '\0') && (DAT_10021ccc != (int *)0x0)) &&
       (cVar1 = (**(code **)(*DAT_10021ccc + 0x80))(), cVar1 == '\0')) {
      (**(code **)(*DAT_10021ccc + 0x84))();
      return;
    }
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x3c))(&stack0x00000000);
    uVar3 = *(undefined4 *)((int)this + 0x2c);
    *(undefined4 *)((int)this + 0x2c) = uStack_4;
    uVar2 = FUN_1000e380(extraout_ECX,extraout_EDX);
    *(int *)((int)this + 0x30) = (int)uVar2;
    if ((*(uint *)((int)this + 0x14) & 0x1000) == 0) {
      FUN_10008f20(&stack0xffffffd8,0x20,"%f");
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x40))(&stack0xffffffd8,uVar3);
      return;
    }
  }
  return;
}


// 1000d750 FUN_1000d750

undefined4 * __thiscall FUN_1000d750(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_1001ba1c;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_1001b9fc;
  if (*(int *)((int)this + 0x24) != 0) {
    FUN_100012c0(*(int *)((int)this + 0x24));
    *(undefined4 *)((int)this + 0x24) = 0;
  }
  *(undefined ***)this = &PTR_LAB_1001b9a8;
  if ((param_1 & 1) != 0) {
    FUN_100012c0(this);
  }
  return (undefined4 *)this;
}


// 1000d7a0 FUN_1000d7a0

int * __fastcall FUN_1000d7a0(int *param_1)

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
  param_1[6] = (int)&PTR_LAB_1001b9dc;
  param_1[0xe] = 0;
  param_1[0x10] = 0;
  *param_1 = (int)&PTR_FUN_1001ba1c;
  param_1[6] = (int)&PTR_LAB_1001b9fc;
  param_1[7] = (int)param_1;
  param_1[8] = (int)&DAT_1001badc;
  *(undefined1 *)(param_1 + 0xd) = 0;
  *(undefined1 *)(param_1 + 0xf) = 0;
  param_1[0x11] = 0;
  pcVar2 = "0";
  do {
    pcVar3 = pcVar2;
    pcVar2 = pcVar3 + 1;
  } while (*pcVar3 != '\0');
  param_1[10] = (int)(pcVar3 + -0x1001badb);
  _Dst = (void *)FUN_10001110(pcVar3 + -0x1001badb);
  param_1[9] = (int)_Dst;
  _memcpy(_Dst,(void *)param_1[8],param_1[10]);
  dVar5 = _atof((char *)param_1[9]);
  param_1[0xb] = (int)(float)dVar5;
  uVar4 = FUN_1000e380(extraout_ECX,extraout_EDX);
  puVar1 = PTR_DAT_1001f64c;
  param_1[0xc] = (int)uVar4;
  *(undefined1 *)(param_1 + 2) = 0;
  param_1[3] = (int)&DAT_1001a66b;
  param_1[4] = (int)puVar1;
  param_1[5] = 2;
  param_1[1] = (int)DAT_10023108;
  DAT_10023108 = param_1;
  if (DAT_1002310c != 0) {
    (**(code **)(*param_1 + 0x2c))();
  }
  *param_1 = (int)&PTR_FUN_1001ba94;
  param_1[6] = (int)&PTR_LAB_1001ba70;
  return param_1;
}


// 1000d8c0 FUN_1000d8c0

void __thiscall FUN_1000d8c0(void *this,undefined4 param_1,char param_2)

{
  int iVar1;
  undefined *puVar2;
  
  if (DAT_10021ccc == (int *)0x0) {
    puVar2 = &DAT_10023138;
  }
  else {
    iVar1 = (**(code **)(*DAT_10021ccc + 0x34))(param_1);
    if (iVar1 == 0) {
      puVar2 = (undefined *)0x0;
    }
    else {
      puVar2 = (undefined *)(iVar1 + 0x18);
    }
  }
  *(undefined **)this = puVar2;
  if (puVar2 == (undefined *)0x0) {
    *(undefined **)this = &DAT_10023138;
  }
                    /* WARNING: Load size is inaccurate */
  *(undefined **)((int)this + 4) = *this + -0x18;
  if ((*this == &DAT_10023138) && ((DAT_10021ccc != (int *)0x0 || (DAT_1001f6bf != '\0')))) {
    if (param_2 == '\0') {
      Warning("ConVarRef %s doesn\'t point to an existing ConVar\n",param_1);
    }
    DAT_1001f6bf = '\0';
  }
  return;
}


// 1000d940 FUN_1000d940

void * __thiscall FUN_1000d940(void *this,undefined4 param_1)

{
  FUN_1000d8c0(this,param_1,'\0');
  return this;
}


// 1000d960 FUN_1000d960

uint __fastcall FUN_1000d960(byte *param_1)

{
  byte *pbVar1;
  byte bVar2;
  uint uVar3;
  uint uVar4;
  
  uVar3 = 0;
  uVar4 = 0;
  bVar2 = *param_1;
  while (bVar2 != 0) {
    uVar3 = *(uint *)(&DAT_1001f6c8 + (bVar2 ^ uVar4) * 4);
    pbVar1 = param_1 + 1;
    param_1 = param_1 + 2;
    if (*pbVar1 == 0) break;
    uVar4 = *(uint *)(&DAT_1001f6c8 + (*pbVar1 ^ uVar3) * 4);
    bVar2 = *param_1;
  }
  return uVar3 << 8 | uVar4;
}


// 1000d9a0 FUN_1000d9a0

uint __fastcall FUN_1000d9a0(byte *param_1)

{
  uint uVar1;
  uint uVar2;
  uint uVar3;
  
  uVar3 = 0;
  uVar2 = 0;
  uVar1 = _toupper((uint)*param_1);
  while (uVar1 != 0) {
    uVar3 = *(uint *)(&DAT_1001f6c8 + (uVar1 ^ uVar2) * 4);
    uVar1 = _toupper((uint)param_1[1]);
    param_1 = param_1 + 2;
    if (uVar1 == 0) break;
    uVar2 = *(uint *)(&DAT_1001f6c8 + (uVar1 ^ uVar3) * 4);
    uVar1 = _toupper((uint)*param_1);
  }
  return uVar3 << 8 | uVar2;
}


// 1000da10 FUN_1000da10

void __fastcall FUN_1000da10(undefined4 *param_1)

{
  *param_1 = 0;
  return;
}


// 1000da20 FUN_1000da20

void FUN_1000da20(int *param_1)

{
  if ((int *)*param_1 != (int *)0x0) {
    FUN_1000da20((int *)*param_1);
    FUN_1000da20((int *)(*param_1 + 4));
    FUN_100012c0(*param_1);
    *param_1 = 0;
  }
  return;
}


// 1000da60 FUN_1000da60

void __fastcall FUN_1000da60(int *param_1)

{
  if ((int *)*param_1 != (int *)0x0) {
    FUN_1000da20((int *)*param_1);
    FUN_1000da20((int *)(*param_1 + 4));
    FUN_100012c0(*param_1);
    *param_1 = 0;
  }
  return;
}


// 1000da8f _LocaleUpdate::_LocaleUpdate

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
    if ((*(undefined **)this != PTR_DAT_10020118) && ((p_Var2->_ownlocale & DAT_10020034) == 0)) {
      ptVar3 = ___updatetlocinfo();
      *(pthreadlocinfo *)this = ptVar3;
    }
    if ((*(undefined **)(this + 4) != PTR_DAT_1001ff38) &&
       ((*(uint *)(*(int *)(this + 8) + 0x70) & DAT_10020034) == 0)) {
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


// 1000db11 __atof_l

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
    pDVar2 = FUN_10010b06();
    *pDVar2 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 1000dbba _atof

/* Library Function - Single Match
    _atof
   
   Library: Visual Studio 2005 Release */

double __cdecl _atof(char *_String)

{
  double dVar1;
  
  dVar1 = __atof_l(_String,(_locale_t)0x0);
  return dVar1;
}


// 1000dbc8 _strtok

/* Library Function - Single Match
    _strtok
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

char * __cdecl _strtok(char *_Str,char *_Delim)

{
  byte bVar1;
  _ptiddata p_Var2;
  char *pcVar3;
  int iVar4;
  byte *pbVar5;
  byte local_28 [32];
  uint local_8;
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  p_Var2 = __getptd();
  pbVar5 = local_28;
  for (iVar4 = 8; iVar4 != 0; iVar4 = iVar4 + -1) {
    pbVar5[0] = 0;
    pbVar5[1] = 0;
    pbVar5[2] = 0;
    pbVar5[3] = 0;
    pbVar5 = pbVar5 + 4;
  }
  do {
    bVar1 = *_Delim;
    local_28[bVar1 >> 3] = local_28[bVar1 >> 3] | '\x01' << (bVar1 & 7);
    _Delim = (char *)((byte *)_Delim + 1);
  } while (bVar1 != 0);
  if (_Str == (char *)0x0) {
    _Str = p_Var2->_token;
  }
  for (; (bVar1 = *_Str, (local_28[bVar1 >> 3] & (byte)(1 << (bVar1 & 7))) != 0 && (bVar1 != 0));
      _Str = (char *)((byte *)_Str + 1)) {
  }
  do {
    if (*_Str == 0) {
LAB_1000dc68:
      p_Var2->_token = _Str;
      pcVar3 = (char *)__security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
      return pcVar3;
    }
    if ((local_28[(byte)*_Str >> 3] & (byte)(1 << (*_Str & 7U))) != 0) {
      *_Str = 0;
      _Str = (char *)((byte *)_Str + 1);
      goto LAB_1000dc68;
    }
    _Str = (char *)((byte *)_Str + 1);
  } while( true );
}


// 1000dc87 FUN_1000dc87

void FUN_1000dc87(void)

{
  return;
}


// 1000dc88 __cfltcvt_init

/* Library Function - Single Match
    __cfltcvt_init
   
   Library: Visual Studio 2005 Release */

void __cfltcvt_init(void)

{
  PTR_LAB_100202a8 = __cfltcvt;
  PTR_LAB_100202ac = &LAB_100113a2;
  PTR_LAB_100202b0 = &LAB_10011360;
  PTR_LAB_100202b4 = &LAB_10011394;
  PTR_LAB_100202b8 = &LAB_1001130a;
  PTR_LAB_100202bc = __cfltcvt;
  PTR_LAB_100202c0 = __cfltcvt_l;
  PTR_LAB_100202c4 = __fassign_l;
  PTR_LAB_100202c8 = __cropzeros_l;
  PTR_LAB_100202cc = __forcdecpt_l;
  return;
}


// 1000dce8 __fpmath

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __fpmath
   
   Library: Visual Studio 2005 Release */

void __cdecl __fpmath(int param_1)

{
  __cfltcvt_init();
  _DAT_1002317c = __ms_p5_mp_test_fdiv();
  if (param_1 != 0) {
    __setdefaultprecision();
  }
  return;
}


// 1000dd10 _strrchr

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


// 1000dd40 _memset

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
  if ((((char)_Val == '\0') && (0xff < _Size)) && (DAT_10024d7c != 0)) {
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


// 1000ddba __vsnprintf_l

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
    pDVar1 = FUN_10010b06();
    *pDVar1 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
    iVar2 = FUN_1001206f(&local_24,(byte *)_Format,_Locale,(wchar_t *)_ArgList);
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
    pDVar1 = FUN_10010b06();
    *pDVar1 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  return iVar2;
}


// 1000de6b __vsnprintf

/* Library Function - Single Match
    __vsnprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl __vsnprintf(char *_Dest,size_t _Count,char *_Format,va_list _Args)

{
  int iVar1;
  
  iVar1 = __vsnprintf_l(_Dest,_Count,_Format,(_locale_t)0x0,_Args);
  return iVar1;
}


// 1000de90 _memcpy

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
          goto switchD_1000e073_caseD_2;
        case 3:
          goto switchD_1000e073_caseD_3;
        }
        goto switchD_1000e073_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_1000e073_caseD_0;
      case 1:
        goto switchD_1000e073_caseD_1;
      case 2:
        goto switchD_1000e073_caseD_2;
      case 3:
        goto switchD_1000e073_caseD_3;
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
              goto switchD_1000e073_caseD_2;
            case 3:
              goto switchD_1000e073_caseD_3;
            }
            goto switchD_1000e073_caseD_1;
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
              goto switchD_1000e073_caseD_2;
            case 3:
              goto switchD_1000e073_caseD_3;
            }
            goto switchD_1000e073_caseD_1;
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
              goto switchD_1000e073_caseD_2;
            case 3:
              goto switchD_1000e073_caseD_3;
            }
            goto switchD_1000e073_caseD_1;
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
switchD_1000e073_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_1000e073_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_1000e073_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_1000e073_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_10024d7c != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_1000deec_caseD_2;
      case 3:
        goto switchD_1000deec_caseD_3;
      }
      goto switchD_1000deec_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_1000deec_caseD_0;
    case 1:
      goto switchD_1000deec_caseD_1;
    case 2:
      goto switchD_1000deec_caseD_2;
    case 3:
      goto switchD_1000deec_caseD_3;
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
            goto switchD_1000deec_caseD_2;
          case 3:
            goto switchD_1000deec_caseD_3;
          }
          goto switchD_1000deec_caseD_1;
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
            goto switchD_1000deec_caseD_2;
          case 3:
            goto switchD_1000deec_caseD_3;
          }
          goto switchD_1000deec_caseD_1;
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
            goto switchD_1000deec_caseD_2;
          case 3:
            goto switchD_1000deec_caseD_3;
          }
          goto switchD_1000deec_caseD_1;
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
switchD_1000deec_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_1000deec_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_1000deec_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_1000deec_caseD_0:
  return _Dst;
}


// 1000e1f5 type_info::~type_info

/* Library Function - Single Match
    public: virtual __thiscall type_info::~type_info(void)
   
   Library: Visual Studio 2005 Release */

void __thiscall type_info::~type_info(type_info *this)

{
  *(undefined ***)this = &PTR__scalar_deleting_destructor__1001bb34;
  _Type_info_dtor(this);
  return;
}


// 1000e203 type_info::`scalar_deleting_destructor'

/* Library Function - Single Match
    public: virtual void * __thiscall type_info::`scalar deleting destructor'(unsigned int)
   
   Library: Visual Studio 2005 Release */

void * __thiscall type_info::_scalar_deleting_destructor_(type_info *this,uint param_1)

{
  ~type_info(this);
  if ((param_1 & 1) != 0) {
    FUN_100012c0(this);
  }
  return this;
}


// 1000e249 FUN_1000e249

int __cdecl FUN_1000e249(int param_1)

{
  int *piVar1;
  int *piVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  
  piVar1 = (int *)FUN_10010270(DAT_10024d6c);
  piVar2 = (int *)FUN_10010270(DAT_10024d68);
  if (piVar1 <= piVar2) {
    uVar4 = ((int)piVar2 - (int)piVar1) + 4;
    if (3 < uVar4) {
      uVar3 = FUN_10001280(piVar1);
      if (uVar3 < uVar4) {
        uVar4 = 0x800;
        if (uVar3 < 0x800) {
          uVar4 = uVar3;
        }
        if ((uVar4 + uVar3 < uVar3) ||
           (iVar5 = thunk_FUN_100011f0(piVar1,uVar4 + uVar3), iVar5 == 0)) {
          if (uVar3 + 0x10 < uVar3) {
            return 0;
          }
          iVar5 = thunk_FUN_100011f0(piVar1,uVar3 + 0x10);
          if (iVar5 == 0) {
            return 0;
          }
        }
        piVar2 = (int *)(iVar5 + ((int)piVar2 - (int)piVar1 >> 2) * 4);
        DAT_10024d6c = FUN_100101f9(iVar5);
      }
      iVar5 = FUN_100101f9(param_1);
      *piVar2 = iVar5;
      DAT_10024d68 = FUN_100101f9((int)(piVar2 + 1));
      return param_1;
    }
  }
  return 0;
}


// 1000e331 __onexit

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __onexit
   
   Library: Visual Studio 2005 Release */

_onexit_t __cdecl __onexit(_onexit_t _Func)

{
  _onexit_t p_Var1;
  
  FUN_10012fbb();
  p_Var1 = (_onexit_t)FUN_1000e249((int)_Func);
  FUN_1000e367();
  return p_Var1;
}


// 1000e367 FUN_1000e367

void FUN_1000e367(void)

{
  FUN_10012fc4();
  return;
}


// 1000e36d _atexit

/* Library Function - Single Match
    _atexit
   
   Library: Visual Studio 2005 Release */

int __cdecl _atexit(_func_4879 *param_1)

{
  _onexit_t p_Var1;
  
  p_Var1 = __onexit((_onexit_t)param_1);
  return (p_Var1 != (_onexit_t)0x0) - 1;
}


// 1000e380 FUN_1000e380

ulonglong __fastcall FUN_1000e380(undefined4 param_1,undefined4 param_2)

{
  ulonglong uVar1;
  uint uVar2;
  float fVar3;
  float10 in_ST0;
  uint local_20;
  float fStack_1c;
  
  if (DAT_10024d7c == 0) {
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


// 1000e42b _atol

/* Library Function - Single Match
    _atol
   
   Library: Visual Studio 2005 Release */

long __cdecl _atol(char *_Str)

{
  long lVar1;
  
  lVar1 = _strtol(_Str,(char **)0x0,10);
  return lVar1;
}


// 1000e441 __CRT_INIT@12

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
        _DAT_1002351c = DVar2 * 0x100 + DVar3;
        DAT_10023514 = DVar1;
        _DAT_10023518 = uVar10;
        DAT_10023520 = DVar2;
        _DAT_10023524 = DVar3;
        bVar4 = FUN_100012a0();
        if (CONCAT31(extraout_var,bVar4) != 0) {
          iVar7 = __mtinit();
          if (iVar7 != 0) {
            __RTC_Initialize();
            DAT_10024d80 = GetCommandLineA();
            DAT_10023184 = ___crtGetEnvironmentStringsA();
            iVar7 = __ioinit();
            if (-1 < iVar7) {
              iVar7 = __setargv();
              if (((-1 < iVar7) && (iVar7 = __setenvp(), -1 < iVar7)) &&
                 (iVar7 = __cinit(0), iVar7 == 0)) {
                DAT_10023180 = DAT_10023180 + 1;
                return 1;
              }
              __ioterm();
            }
            __mtterm();
          }
          FUN_100012b0();
        }
      }
    }
  }
  else if (param_2 == 0) {
    if (0 < DAT_10023180) {
      DAT_10023180 = DAT_10023180 + -1;
      if (DAT_10023550 == 0) {
        __cexit();
      }
      if (param_3 == 0) {
        __ioterm();
        __mtterm();
        FUN_100012b0();
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
    FUN_100102e7();
    pDVar8 = (DWORD *)FUN_10001250(1,0x214);
    if (pDVar8 != (DWORD *)0x0) {
      uVar12 = DAT_10020128;
      pDVar13 = pDVar8;
      pcVar9 = (code *)FUN_10010270(DAT_100231dc);
      iVar7 = (*pcVar9)(uVar12,pDVar13);
      if (iVar7 != 0) {
        FUN_10010356((int)pDVar8,0);
        DVar11 = GetCurrentThreadId();
        pDVar8[1] = 0xffffffff;
        *pDVar8 = DVar11;
        return 1;
      }
      FUN_100012c0(pDVar8);
    }
  }
  return 0;
}


// 1000e61a ___DllMainCRTStartup

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x1000e6ac) */
/* WARNING: Removing unreachable block (ram,0x1000e659) */
/* WARNING: Removing unreachable block (ram,0x1000e6d9) */
/* Library Function - Single Match
    ___DllMainCRTStartup
   
   Library: Visual Studio 2005 Release */

int __fastcall ___DllMainCRTStartup(int param_1,int param_2,undefined4 param_3)

{
  int iVar1;
  int local_20;
  
  if (((param_2 == 0) && (DAT_10023180 == 0)) ||
     (((param_2 == 1 || (param_2 == 2)) &&
      (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)))) {
    local_20 = 0;
  }
  else {
    local_20 = FUN_10013d04();
    if ((param_2 == 1) && (local_20 == 0)) {
      FUN_10013d04();
      __CRT_INIT_12(param_3,0,param_1);
    }
    if (((param_2 == 0) || (param_2 == 3)) &&
       (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)) {
      local_20 = 0;
    }
  }
  return local_20;
}


// 1000e710 entry

void entry(undefined4 param_1,int param_2,int param_3)

{
  if (param_2 == 1) {
    ___security_init_cookie();
  }
  ___DllMainCRTStartup(param_3,param_2,param_1);
  return;
}


// 1000e750 _strchr

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


// 1000e896 __freea

/* Library Function - Single Match
    __freea
   
   Library: Visual Studio 2005 Release */

void __cdecl __freea(void *_Memory)

{
  if ((_Memory != (void *)0x0) && (*(int *)((int)_Memory + -8) == 0xdddd)) {
    FUN_100012c0((int *)((int)_Memory + -8));
  }
  return;
}


// 1000e8b1 __tolower_l

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
LAB_1000e910:
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
      pDVar4 = FUN_10010b06();
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
    if (iVar3 == 0) goto LAB_1000e910;
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


// 1000e9c8 _tolower

/* Library Function - Single Match
    _tolower
   
   Library: Visual Studio 2005 Release */

int __cdecl _tolower(int _C)

{
  if (DAT_100231b0 == 0) {
    if (_C - 0x41U < 0x1a) {
      return _C + 0x20;
    }
  }
  else {
    _C = __tolower_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 1000e9f0 _strncat

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
      if ((byte)uVar4 == 0) goto LAB_1000ea4d;
      uVar4 = (uint)puVar5 & 3;
    }
    do {
      do {
        puVar6 = puVar5;
        puVar5 = puVar6 + 1;
      } while (((*puVar6 ^ 0xffffffff ^ *puVar6 + 0x7efefeff) & 0x81010100) == 0);
      uVar4 = *puVar6;
      if ((char)uVar4 == '\0') goto LAB_1000ea5f;
      if ((char)(uVar4 >> 8) == '\0') {
        puVar6 = (uint *)((int)puVar6 + 1);
        goto LAB_1000ea5f;
      }
      if ((uVar4 & 0xff0000) == 0) {
        puVar6 = (uint *)((int)puVar6 + 2);
        goto LAB_1000ea5f;
      }
    } while ((uVar4 & 0xff000000) != 0);
LAB_1000ea4d:
    puVar6 = (uint *)((int)puVar5 + -1);
LAB_1000ea5f:
    if (((uint)_Source & 3) == 0) {
      uVar3 = _Count >> 2;
    }
    else {
      do {
        bVar1 = (byte)*(uint *)_Source;
        uVar4 = (uint)bVar1;
        _Source = (char *)((int)_Source + 1);
        if (bVar1 == 0) goto LAB_1000eaba;
        *(byte *)puVar6 = bVar1;
        puVar6 = (uint *)((int)puVar6 + 1);
        _Count = _Count - 1;
        if (_Count == 0) goto LAB_1000eab0;
      } while (((uint)_Source & 3) != 0);
      uVar3 = _Count >> 2;
    }
    for (; uVar3 != 0; uVar3 = uVar3 - 1) {
      uVar2 = *(uint *)_Source;
      uVar4 = *(uint *)_Source;
      _Source = (char *)((int)_Source + 4);
      if (((uVar2 ^ 0xffffffff ^ uVar2 + 0x7efefeff) & 0x81010100) != 0) {
        if ((char)uVar4 == '\0') {
LAB_1000eaba:
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
LAB_1000eab0:
    *(byte *)puVar6 = (byte)_Count;
  }
  return _Dest;
}


// 1000eb30 _memmove

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
          goto switchD_1000ed13_caseD_2;
        case 3:
          goto switchD_1000ed13_caseD_3;
        }
        goto switchD_1000ed13_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_1000ed13_caseD_0;
      case 1:
        goto switchD_1000ed13_caseD_1;
      case 2:
        goto switchD_1000ed13_caseD_2;
      case 3:
        goto switchD_1000ed13_caseD_3;
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
              goto switchD_1000ed13_caseD_2;
            case 3:
              goto switchD_1000ed13_caseD_3;
            }
            goto switchD_1000ed13_caseD_1;
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
              goto switchD_1000ed13_caseD_2;
            case 3:
              goto switchD_1000ed13_caseD_3;
            }
            goto switchD_1000ed13_caseD_1;
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
              goto switchD_1000ed13_caseD_2;
            case 3:
              goto switchD_1000ed13_caseD_3;
            }
            goto switchD_1000ed13_caseD_1;
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
switchD_1000ed13_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_1000ed13_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_1000ed13_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_1000ed13_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_10024d7c != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_1000eb8c_caseD_2;
      case 3:
        goto switchD_1000eb8c_caseD_3;
      }
      goto switchD_1000eb8c_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_1000eb8c_caseD_0;
    case 1:
      goto switchD_1000eb8c_caseD_1;
    case 2:
      goto switchD_1000eb8c_caseD_2;
    case 3:
      goto switchD_1000eb8c_caseD_3;
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
            goto switchD_1000eb8c_caseD_2;
          case 3:
            goto switchD_1000eb8c_caseD_3;
          }
          goto switchD_1000eb8c_caseD_1;
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
            goto switchD_1000eb8c_caseD_2;
          case 3:
            goto switchD_1000eb8c_caseD_3;
          }
          goto switchD_1000eb8c_caseD_1;
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
            goto switchD_1000eb8c_caseD_2;
          case 3:
            goto switchD_1000eb8c_caseD_3;
          }
          goto switchD_1000eb8c_caseD_1;
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
switchD_1000eb8c_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_1000eb8c_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_1000eb8c_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_1000eb8c_caseD_0:
  return _Dst;
}


// 1000eea0 _strncpy

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
        goto joined_r0x1000eeec;
      }
    }
    do {
      if (((uint)puVar5 & 3) == 0) {
        uVar4 = _Count >> 2;
        cVar3 = '\0';
        if (uVar4 == 0) goto LAB_1000ef33;
        goto LAB_1000efa9;
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
joined_r0x1000efa5:
          while( true ) {
            uVar4 = uVar4 - 1;
            puVar5 = puVar5 + 1;
            if (uVar4 == 0) break;
LAB_1000efa9:
            *puVar5 = 0;
          }
          cVar3 = '\0';
          _Count = _Count & 3;
          if (_Count != 0) goto LAB_1000ef33;
          return _Dest;
        }
        if ((char)(uVar2 >> 8) == '\0') {
          *puVar5 = uVar2 & 0xff;
          goto joined_r0x1000efa5;
        }
        if ((uVar2 & 0xff0000) == 0) {
          *puVar5 = uVar2 & 0xffff;
          goto joined_r0x1000efa5;
        }
        if ((uVar2 & 0xff000000) == 0) {
          *puVar5 = uVar2;
          goto joined_r0x1000efa5;
        }
      }
      *puVar5 = uVar2;
      puVar5 = puVar5 + 1;
      uVar4 = uVar4 - 1;
joined_r0x1000eeec:
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
LAB_1000ef33:
        *(char *)puVar5 = cVar3;
        puVar5 = (uint *)((int)puVar5 + 1);
      }
      return _Dest;
    }
    _Count = _Count - 1;
  } while (_Count != 0);
  return _Dest;
}


// 1000f13a __isdigit_l

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


// 1000f189 _isdigit

/* Library Function - Single Match
    _isdigit
   
   Library: Visual Studio 2005 Release */

int __cdecl _isdigit(int _C)

{
  int iVar1;
  
  if (DAT_100231b0 == 0) {
    return (byte)PTR_DAT_10020108[_C * 2] & 4;
  }
  iVar1 = __isdigit_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 1000f1b2 __isspace_l

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


// 1000f201 _isspace

/* Library Function - Single Match
    _isspace
   
   Library: Visual Studio 2005 Release */

int __cdecl _isspace(int _C)

{
  int iVar1;
  
  if (DAT_100231b0 == 0) {
    return (byte)PTR_DAT_10020108[_C * 2] & 8;
  }
  iVar1 = __isspace_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 1000f230 __alloca_probe_16

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


// 1000f246 __alloca_probe_8

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


// 1000f25c FUN_1000f25c

void __cdecl FUN_1000f25c(wchar_t *param_1)

{
  _wcstol(param_1,(wchar_t **)0x0,10);
  return;
}


// 1000f272 FUN_1000f272

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 __cdecl FUN_1000f272(byte *param_1,undefined4 *param_2,localeinfo_struct *param_3)

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
    pDVar1 = FUN_10010b06();
    *pDVar1 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_1000f378;
      }
    }
    else {
      local_c = _DAT_10020590;
      if (*_Str == 0x2d) {
        local_c = -_DAT_10020590;
      }
    }
    pDVar1 = FUN_10010b06();
    *pDVar1 = 0x22;
  }
  else {
    local_c = 0.0;
    if (param_2 != (undefined4 *)0x0) {
      *param_2 = param_1;
    }
  }
LAB_1000f378:
  if (local_10 != '\0') {
    *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
  }
  return (float10)local_c;
}


// 1000f38d FUN_1000f38d

void __cdecl FUN_1000f38d(byte *param_1,undefined4 *param_2)

{
  FUN_1000f272(param_1,param_2,(localeinfo_struct *)0x0);
  return;
}


// 1000f3a0 strtoxl

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
    pDVar3 = FUN_10010b06();
    *pDVar3 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
LAB_1000f457:
    bVar8 = *pbVar9;
    pbVar9 = pbVar2 + 2;
  }
  else if (bVar8 == 0x2b) goto LAB_1000f457;
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
      goto LAB_1000f4bd;
    }
    if ((*pbVar9 != 0x78) && (*pbVar9 != 0x58)) {
      param_4 = 8;
      goto LAB_1000f4bd;
    }
    param_4 = 0x10;
  }
  else if ((param_4 != 0x10) || (bVar8 != 0x30)) goto LAB_1000f4bd;
  if ((*pbVar9 == 0x78) || (*pbVar9 == 0x58)) {
    bVar8 = pbVar9[1];
    pbVar9 = pbVar9 + 2;
  }
LAB_1000f4bd:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  do {
    uVar1 = *(ushort *)(ptVar5[1].lc_category[0].locale + (uint)bVar8 * 2);
    if ((uVar1 & 4) == 0) {
      if ((uVar1 & 0x103) == 0) {
LAB_1000f517:
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
          pDVar3 = FUN_10010b06();
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
    if ((uint)param_4 <= uVar6) goto LAB_1000f517;
    if ((local_8 < uVar4) ||
       ((local_8 == uVar4 && (uVar6 <= (uint)(0xffffffff % (ulonglong)(uint)param_4))))) {
      local_8 = local_8 * param_4 + uVar6;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (char **)0x0) goto LAB_1000f517;
    }
    bVar8 = *pbVar9;
    pbVar9 = pbVar9 + 1;
  } while( true );
}


// 1000f5cb _strtol

/* Library Function - Single Match
    _strtol
   
   Library: Visual Studio 2005 Release */

long __cdecl _strtol(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_100231b0 == 0) {
    ppuVar2 = &PTR_DAT_10020120;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 1000f5f4 _strtoul

/* Library Function - Single Match
    _strtoul
   
   Library: Visual Studio 2005 Release */

ulong __cdecl _strtoul(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_100231b0 == 0) {
    ppuVar2 = &PTR_DAT_10020120;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,1);
  return uVar1;
}


// 1000f620 __allmul

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


// 1000f660 __alloca_probe

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


// 1000f68b __toupper_l

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
LAB_1000f6e8:
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
      pDVar4 = FUN_10010b06();
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
    if (iVar3 == 0) goto LAB_1000f6e8;
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


// 1000f7a3 _toupper

/* Library Function - Single Match
    _toupper
   
   Library: Visual Studio 2005 Release */

int __cdecl _toupper(int _C)

{
  if (DAT_100231b0 == 0) {
    if (_C - 0x61U < 0x1a) {
      return _C + -0x20;
    }
  }
  else {
    _C = __toupper_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 1000f7ca FUN_1000f7ca

undefined4 FUN_1000f7ca(void)

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


// 1000f7f9 setSBCS

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
    *puVar1 = puVar1[(int)&DAT_1001fb10 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  puVar1 = (undefined1 *)(in_EAX + 0x11d);
  iVar2 = 0x100;
  do {
    *puVar1 = puVar1[(int)&DAT_1001fb10 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  return;
}


// 1000f84e setSBUpLow

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
  
  local_8 = DAT_100202a0 ^ (uint)local_4a0;
  BVar3 = GetCPInfo(*(UINT *)(unaff_ESI + 4),&local_51c);
  if (BVar3 == 0) {
    uVar4 = 0;
    do {
      pcVar2 = (char *)(unaff_ESI + 0x11d + uVar4);
      if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) + 0x20 < (char *)0x1a) {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        cVar6 = (char)uVar4 + ' ';
LAB_1000f9b5:
        *pcVar2 = cVar6;
      }
      else {
        if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) < (char *)0x1a) {
          pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
          *pbVar1 = *pbVar1 | 0x20;
          cVar6 = (char)uVar4 + -0x20;
          goto LAB_1000f9b5;
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
          goto LAB_1000f95c;
        }
        *(undefined1 *)(unaff_ESI + 0x11d + uVar4) = 0;
      }
      else {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        CVar5 = local_208[uVar4];
LAB_1000f95c:
        *(CHAR *)(unaff_ESI + 0x11d + uVar4) = CVar5;
      }
      uVar4 = uVar4 + 1;
    } while (uVar4 < 0x100);
  }
  __security_check_cookie(local_8 ^ (uint)local_4a0);
  return;
}


// 1000f9d8 ___updatetmbcinfo

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
  if (((p_Var1->_ownlocale & DAT_10020034) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xd);
    lpAddend = p_Var1->ptmbcinfo;
    if (lpAddend != (pthreadmbcinfo)PTR_DAT_1001ff38) {
      if (lpAddend != (pthreadmbcinfo)0x0) {
        LVar2 = InterlockedDecrement(&lpAddend->refcount);
        if ((LVar2 == 0) && (lpAddend != (pthreadmbcinfo)&DAT_1001fb10)) {
          FUN_100012c0(lpAddend);
        }
      }
      p_Var1->ptmbcinfo = (pthreadmbcinfo)PTR_DAT_1001ff38;
      lpAddend = (pthreadmbcinfo)PTR_DAT_1001ff38;
      InterlockedIncrement((LONG *)PTR_DAT_1001ff38);
    }
    FUN_1000fa73();
  }
  else {
    lpAddend = p_Var1->ptmbcinfo;
  }
  if (lpAddend == (pthreadmbcinfo)0x0) {
    __amsg_exit(0x20);
  }
  return lpAddend;
}


// 1000fa73 FUN_1000fa73

void FUN_1000fa73(void)

{
  FUN_10014265(0xd);
  return;
}


// 1000fa7c getSystemCP

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
  DAT_10023194 = 0;
  if (unaff_ESI == -2) {
    DAT_10023194 = 1;
    UVar1 = GetOEMCP();
  }
  else if (unaff_ESI == -3) {
    DAT_10023194 = 1;
    UVar1 = GetACP();
  }
  else {
    if (unaff_ESI != -4) {
      if (local_8 == '\0') {
        DAT_10023194 = 0;
        return unaff_ESI;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return unaff_ESI;
    }
    UVar1 = *(UINT *)(local_14[0] + 4);
    DAT_10023194 = 1;
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return UVar1;
}


// 1000faf6 FUN_1000faf6

void __cdecl FUN_1000faf6(undefined4 param_1,int param_2)

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  uVar4 = getSystemCP((int)unaff_EDI);
  if (uVar4 != 0) {
    local_20 = (byte *)0x0;
    uVar5 = 0;
LAB_1000fb32:
    if (*(uint *)((int)&DAT_1001ff40 + uVar5) != uVar4) goto code_r0x1000fb3e;
    _memset((void *)(param_2 + 0x1c),0,0x101);
    local_24 = 0;
    pbVar8 = &DAT_1001ff50 + (int)local_20 * 0x30;
    local_20 = pbVar8;
    do {
      for (; (*pbVar8 != 0 && (bVar3 = pbVar8[1], bVar3 != 0)); pbVar8 = pbVar8 + 2) {
        for (uVar5 = (uint)*pbVar8; uVar5 <= bVar3; uVar5 = uVar5 + 1) {
          pbVar2 = (byte *)(param_2 + 0x1d + uVar5);
          *pbVar2 = *pbVar2 | *(byte *)(local_24 + 0x1001ff3c);
          bVar3 = pbVar8[1];
        }
      }
      local_24 = local_24 + 1;
      pbVar8 = local_20 + 8;
      local_20 = pbVar8;
    } while (local_24 < 4);
    *(uint *)(param_2 + 4) = uVar4;
    *(undefined4 *)(param_2 + 8) = 1;
    uVar9 = FUN_1000f7ca();
    *(undefined4 *)(param_2 + 0xc) = uVar9;
    puVar7 = (undefined2 *)(param_2 + 0x10);
    puVar10 = (undefined2 *)(&DAT_1001ff44 + extraout_ECX);
    iVar11 = 6;
    do {
      *puVar7 = *puVar10;
      puVar10 = puVar10 + 1;
      puVar7 = puVar7 + 1;
      iVar11 = iVar11 + -1;
    } while (iVar11 != 0);
    goto LAB_1000fc63;
  }
LAB_1000fb1f:
  setSBCS(unaff_EDI);
LAB_1000fcc0:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
code_r0x1000fb3e:
  local_20 = (byte *)((int)local_20 + 1);
  uVar5 = uVar5 + 0x30;
  if (0xef < uVar5) goto code_r0x1000fb4b;
  goto LAB_1000fb32;
code_r0x1000fb4b:
  if (((uVar4 == 65000) || (uVar4 == 0xfde9)) ||
     (BVar6 = IsValidCodePage(uVar4 & 0xffff), BVar6 == 0)) goto LAB_1000fcc0;
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
      uVar9 = FUN_1000f7ca();
      *(undefined4 *)(param_2 + 0xc) = uVar9;
      *(undefined4 *)(param_2 + 8) = extraout_EDX;
    }
    *(undefined4 *)(param_2 + 0x10) = 0;
    *(undefined4 *)(param_2 + 0x14) = 0;
    *(undefined4 *)(param_2 + 0x18) = 0;
LAB_1000fc63:
    setSBUpLow(unaff_EDI);
    goto LAB_1000fcc0;
  }
  if (DAT_10023194 == 0) goto LAB_1000fcc0;
  goto LAB_1000fb1f;
}


// 1000fccf __setmbcp

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
    ptVar3 = (pthreadmbcinfo)FUN_10001110(0x220);
    if (ptVar3 != (pthreadmbcinfo)0x0) {
      ptVar7 = p_Var1->ptmbcinfo;
      ptVar8 = ptVar3;
      for (iVar6 = 0x88; iVar6 != 0; iVar6 = iVar6 + -1) {
        ptVar8->refcount = ptVar7->refcount;
        ptVar7 = (pthreadmbcinfo)&ptVar7->mbcodepage;
        ptVar8 = (pthreadmbcinfo)&ptVar8->mbcodepage;
      }
      ptVar3->refcount = 0;
      local_24 = FUN_1000faf6(iVar2,(int)ptVar3);
      if (local_24 == 0) {
        LVar4 = InterlockedDecrement(&p_Var1->ptmbcinfo->refcount);
        if ((LVar4 == 0) && (p_Var1->ptmbcinfo != (pthreadmbcinfo)&DAT_1001fb10)) {
          FUN_100012c0(p_Var1->ptmbcinfo);
        }
        p_Var1->ptmbcinfo = ptVar3;
        InterlockedIncrement((LONG *)ptVar3);
        if (((p_Var1->_ownlocale & 2) == 0) && (((byte)DAT_10020034 & 1) == 0)) {
          __lock(0xd);
          _DAT_100231a4 = ptVar3->mbcodepage;
          _DAT_100231a8 = ptVar3->ismbcodepage;
          _DAT_100231ac = *(undefined4 *)ptVar3->mbulinfo;
          for (iVar2 = 0; iVar2 < 5; iVar2 = iVar2 + 1) {
            (&DAT_10023198)[iVar2] = ptVar3->mbulinfo[iVar2 + 2];
          }
          for (iVar2 = 0; iVar2 < 0x101; iVar2 = iVar2 + 1) {
            (&DAT_1001fd30)[iVar2] = ptVar3->mbctype[iVar2 + 4];
          }
          for (iVar2 = 0; iVar2 < 0x100; iVar2 = iVar2 + 1) {
            (&DAT_1001fe38)[iVar2] = ptVar3->mbcasemap[iVar2 + 4];
          }
          LVar4 = InterlockedDecrement((LONG *)PTR_DAT_1001ff38);
          if ((LVar4 == 0) && (PTR_DAT_1001ff38 != &DAT_1001fb10)) {
            FUN_100012c0(PTR_DAT_1001ff38);
          }
          PTR_DAT_1001ff38 = (undefined *)ptVar3;
          InterlockedIncrement((LONG *)ptVar3);
          FUN_1000fe30();
        }
      }
      else if (local_24 == -1) {
        if (ptVar3 != (pthreadmbcinfo)&DAT_1001fb10) {
          FUN_100012c0(ptVar3);
        }
        pDVar5 = FUN_10010b06();
        *pDVar5 = 0x16;
      }
    }
  }
  return local_24;
}


// 1000fe30 FUN_1000fe30

void FUN_1000fe30(void)

{
  FUN_10014265(0xd);
  return;
}


// 1000fe69 ___initmbctable

/* Library Function - Single Match
    ___initmbctable
   
   Library: Visual Studio 2005 Release */

undefined4 ___initmbctable(void)

{
  if (DAT_10024d70 == 0) {
    __setmbcp(-3);
    DAT_10024d70 = 1;
  }
  return 0;
}


// 1000fe87 ___freetlocinfo

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
       (*(undefined ***)(param_1 + 0xbc) != &PTR_DAT_10020668)) &&
      (*(int **)(param_1 + 0xb0) != (int *)0x0)) && (**(int **)(param_1 + 0xb0) == 0)) {
    piVar4 = *(int **)(param_1 + 0xb8);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_100012c0(piVar4);
      ___free_lconv_mon(*(int *)(param_1 + 0xbc));
    }
    piVar4 = *(int **)(param_1 + 0xb4);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_100012c0(piVar4);
      ___free_lconv_num(*(int **)(param_1 + 0xbc));
    }
    FUN_100012c0(*(undefined4 *)(param_1 + 0xb0));
    FUN_100012c0(*(undefined4 *)(param_1 + 0xbc));
  }
  if ((*(int **)(param_1 + 0xc0) != (int *)0x0) && (**(int **)(param_1 + 0xc0) == 0)) {
    FUN_100012c0(*(int *)(param_1 + 0xc4) + -0xfe);
    FUN_100012c0(*(int *)(param_1 + 0xcc) + -0x80);
    FUN_100012c0(*(int *)(param_1 + 0xd0) + -0x80);
    FUN_100012c0(*(undefined4 *)(param_1 + 0xc0));
  }
  ppuVar1 = *(undefined ***)(param_1 + 0xd4);
  if ((ppuVar1 != &PTR_DAT_100205a8) && (ppuVar1[0x2d] == (undefined *)0x0)) {
    ___free_lc_time(ppuVar1);
    FUN_100012c0(*(undefined4 *)(param_1 + 0xd4));
  }
  piVar4 = (int *)(param_1 + 0x50);
  iVar3 = 6;
  do {
    if ((((undefined *)piVar4[-2] != &DAT_10020038) &&
        (piVar2 = (int *)*piVar4, piVar2 != (int *)0x0)) && (*piVar2 == 0)) {
      FUN_100012c0(piVar2);
    }
    if (((piVar4[-1] != 0) && (piVar2 = (int *)piVar4[1], piVar2 != (int *)0x0)) && (*piVar2 == 0))
    {
      FUN_100012c0(piVar2);
    }
    piVar4 = piVar4 + 4;
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  FUN_100012c0(param_1);
  return;
}


// 1000ffc7 ___addlocaleref

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
    if (((undefined *)pLVar1[-2] != &DAT_10020038) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 1001004d ___removelocaleref

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
      if (((undefined *)pLVar1[-2] != &DAT_10020038) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 100100d9 __updatetlocinfoEx_nolock

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
        if ((*pLVar1 == 0) && (pLVar1 != (LONG *)&DAT_10020040)) {
          ___freetlocinfo((int)pLVar1);
        }
      }
    }
    return unaff_EDI;
  }
  return (LONG *)0x0;
}


// 10010117 ___updatetlocinfo

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___updatetlocinfo
   
   Library: Visual Studio 2005 Release */

pthreadlocinfo __cdecl ___updatetlocinfo(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = __getptd();
  if (((p_Var1->_ownlocale & DAT_10020034) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xc);
    __updatetlocinfoEx_nolock();
    FUN_10010181();
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


// 10010181 FUN_10010181

void FUN_10010181(void)

{
  FUN_10014265(0xc);
  return;
}


// 1001018d FUN_1001018d

undefined4 FUN_1001018d(void)

{
  int iVar1;
  HMODULE pHVar2;
  int iVar3;
  uint uVar4;
  char *_Str2;
  int local_8;
  
  uVar4 = 0;
  local_8 = 0;
  FUN_1001303c(&local_8);
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


// 100101f9 FUN_100101f9

int __cdecl FUN_100101f9(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_1002012c);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10020128 != -1)) {
    iVar3 = DAT_10020128;
    pcVar2 = (code *)TlsGetValue(DAT_1002012c);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1f8);
      goto LAB_10010253;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_1001018d();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"EncodePointer");
LAB_10010253:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 10010267 __encoded_null

/* Library Function - Single Match
    __encoded_null
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __encoded_null(void)

{
  FUN_100101f9(0);
  return;
}


// 10010270 FUN_10010270

int __cdecl FUN_10010270(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_1002012c);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10020128 != -1)) {
    iVar3 = DAT_10020128;
    pcVar2 = (code *)TlsGetValue(DAT_1002012c);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1fc);
      goto LAB_100102ca;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_1001018d();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"DecodePointer");
LAB_100102ca:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 100102e7 FUN_100102e7

LPVOID FUN_100102e7(void)

{
  LPVOID lpTlsValue;
  
  lpTlsValue = TlsGetValue(DAT_1002012c);
  if (lpTlsValue == (LPVOID)0x0) {
    lpTlsValue = (LPVOID)FUN_10010270(DAT_100231d8);
    TlsSetValue(DAT_1002012c,lpTlsValue);
  }
  return lpTlsValue;
}


// 10010319 __mtterm

/* Library Function - Single Match
    __mtterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __mtterm(void)

{
  code *pcVar1;
  int iVar2;
  
  if (DAT_10020128 != -1) {
    iVar2 = DAT_10020128;
    pcVar1 = (code *)FUN_10010270(DAT_100231e0);
    (*pcVar1)(iVar2);
    DAT_10020128 = -1;
  }
  if (DAT_1002012c != 0xffffffff) {
    TlsFree(DAT_1002012c);
    DAT_1002012c = 0xffffffff;
  }
  __mtdeletelocks();
  return;
}


// 10010356 FUN_10010356

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

void __cdecl FUN_10010356(int param_1,int param_2)

{
  HMODULE hModule;
  int iVar1;
  FARPROC pFVar2;
  
  hModule = GetModuleHandleA("KERNEL32.DLL");
  *(undefined **)(param_1 + 0x5c) = &DAT_100203e0;
  *(undefined4 *)(param_1 + 0x14) = 1;
  if (hModule != (HMODULE)0x0) {
    iVar1 = FUN_1001018d();
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
  *(undefined **)(param_1 + 0x68) = &DAT_1001fb10;
  InterlockedIncrement((LONG *)&DAT_1001fb10);
  __lock(0xc);
  *(int *)(param_1 + 0x6c) = param_2;
  if (param_2 == 0) {
    *(undefined **)(param_1 + 0x6c) = PTR_DAT_10020118;
  }
  ___addlocaleref(*(LONG **)(param_1 + 0x6c));
  FUN_1001040c();
  return;
}


// 1001040c FUN_1001040c

void FUN_1001040c(void)

{
  FUN_10014265(0xc);
  return;
}


// 10010415 FUN_10010415

DWORD * FUN_10010415(void)

{
  DWORD dwErrCode;
  code *pcVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD DVar4;
  undefined4 uVar5;
  DWORD *pDVar6;
  
  dwErrCode = GetLastError();
  uVar5 = DAT_10020128;
  pcVar1 = (code *)FUN_100102e7();
  pDVar2 = (DWORD *)(*pcVar1)(uVar5);
  if (pDVar2 == (DWORD *)0x0) {
    pDVar2 = (DWORD *)FUN_10001250(1,0x214);
    if (pDVar2 != (DWORD *)0x0) {
      uVar5 = DAT_10020128;
      pDVar6 = pDVar2;
      pcVar1 = (code *)FUN_10010270(DAT_100231dc);
      iVar3 = (*pcVar1)(uVar5,pDVar6);
      if (iVar3 == 0) {
        FUN_100012c0(pDVar2);
        pDVar2 = (DWORD *)0x0;
      }
      else {
        FUN_10010356((int)pDVar2,0);
        DVar4 = GetCurrentThreadId();
        pDVar2[1] = 0xffffffff;
        *pDVar2 = DVar4;
      }
    }
  }
  SetLastError(dwErrCode);
  return pDVar2;
}


// 1001048c __getptd

/* Library Function - Single Match
    __getptd
   
   Library: Visual Studio 2005 Release */

_ptiddata __cdecl __getptd(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = (_ptiddata)FUN_10010415();
  if (p_Var1 == (_ptiddata)0x0) {
    __amsg_exit(0x10);
  }
  return p_Var1;
}


// 100104a4 __freefls@4

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
      FUN_100012c0(*(int *)(param_1 + 0x24));
    }
    if (*(int *)(param_1 + 0x2c) != 0) {
      FUN_100012c0(*(int *)(param_1 + 0x2c));
    }
    if (*(int *)(param_1 + 0x34) != 0) {
      FUN_100012c0(*(int *)(param_1 + 0x34));
    }
    if (*(int *)(param_1 + 0x3c) != 0) {
      FUN_100012c0(*(int *)(param_1 + 0x3c));
    }
    if (*(int *)(param_1 + 0x44) != 0) {
      FUN_100012c0(*(int *)(param_1 + 0x44));
    }
    if (*(int *)(param_1 + 0x48) != 0) {
      FUN_100012c0(*(int *)(param_1 + 0x48));
    }
    if (*(undefined **)(param_1 + 0x5c) != &DAT_100203e0) {
      FUN_100012c0(*(undefined **)(param_1 + 0x5c));
    }
    __lock(0xd);
    pLVar1 = *(LONG **)(param_1 + 0x68);
    if (pLVar1 != (LONG *)0x0) {
      LVar2 = InterlockedDecrement(pLVar1);
      if ((LVar2 == 0) && (pLVar1 != (LONG *)&DAT_1001fb10)) {
        FUN_100012c0(pLVar1);
      }
    }
    FUN_100105b0();
    __lock(0xc);
    pLVar1 = *(LONG **)(param_1 + 0x6c);
    if (pLVar1 != (LONG *)0x0) {
      ___removelocaleref(pLVar1);
      if (((pLVar1 != (LONG *)PTR_DAT_10020118) && (pLVar1 != (LONG *)&DAT_10020040)) &&
         (*pLVar1 == 0)) {
        ___freetlocinfo((int)pLVar1);
      }
    }
    FUN_100105bc();
    FUN_100012c0(param_1);
  }
  return;
}


// 100105b0 FUN_100105b0

void FUN_100105b0(void)

{
  FUN_10014265(0xd);
  return;
}


// 100105bc FUN_100105bc

void FUN_100105bc(void)

{
  FUN_10014265(0xc);
  return;
}


// 100105c5 __freeptd

/* Library Function - Single Match
    __freeptd
   
   Library: Visual Studio 2005 Release */

void __cdecl __freeptd(_ptiddata _Ptd)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  undefined4 uVar4;
  
  if (DAT_10020128 != -1) {
    if ((_Ptd == (_ptiddata)0x0) && (pvVar1 = TlsGetValue(DAT_1002012c), pvVar1 != (LPVOID)0x0)) {
      iVar3 = DAT_10020128;
      pcVar2 = (code *)TlsGetValue(DAT_1002012c);
      _Ptd = (_ptiddata)(*pcVar2)(iVar3);
    }
    uVar4 = 0;
    iVar3 = DAT_10020128;
    pcVar2 = (code *)FUN_10010270(DAT_100231dc);
    (*pcVar2)(iVar3,uVar4);
    __freefls_4((int)_Ptd);
  }
  if (DAT_1002012c != 0xffffffff) {
    TlsSetValue(DAT_1002012c,(LPVOID)0x0);
  }
  return;
}


// 1001062e __mtinit

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
  DAT_100231d4 = GetProcAddress(hModule,"FlsAlloc");
  DAT_100231d8 = GetProcAddress(hModule,"FlsGetValue");
  DAT_100231dc = GetProcAddress(hModule,"FlsSetValue");
  DAT_100231e0 = GetProcAddress(hModule,"FlsFree");
  if ((((DAT_100231d4 == (FARPROC)0x0) || (DAT_100231d8 == (FARPROC)0x0)) ||
      (DAT_100231dc == (FARPROC)0x0)) || (DAT_100231e0 == (FARPROC)0x0)) {
    DAT_100231d8 = TlsGetValue_exref;
    DAT_100231d4 = (FARPROC)&LAB_100102de;
    DAT_100231dc = TlsSetValue_exref;
    DAT_100231e0 = TlsFree_exref;
  }
  DAT_1002012c = TlsAlloc();
  if ((DAT_1002012c != 0xffffffff) && (BVar1 = TlsSetValue(DAT_1002012c,DAT_100231d8), BVar1 != 0))
  {
    __init_pointers();
    DAT_100231d4 = (FARPROC)FUN_100101f9((int)DAT_100231d4);
    DAT_100231d8 = (FARPROC)FUN_100101f9((int)DAT_100231d8);
    DAT_100231dc = (FARPROC)FUN_100101f9((int)DAT_100231dc);
    DAT_100231e0 = (FARPROC)FUN_100101f9((int)DAT_100231e0);
    iVar2 = __mtinitlocks();
    if (iVar2 != 0) {
      pcVar6 = __freefls_4;
      pcVar3 = (code *)FUN_10010270((int)DAT_100231d4);
      DAT_10020128 = (*pcVar3)(pcVar6);
      if ((DAT_10020128 != -1) && (pDVar4 = (DWORD *)FUN_10001250(1,0x214), pDVar4 != (DWORD *)0x0))
      {
        iVar2 = DAT_10020128;
        pDVar7 = pDVar4;
        pcVar3 = (code *)FUN_10010270((int)DAT_100231dc);
        iVar2 = (*pcVar3)(iVar2,pDVar7);
        if (iVar2 != 0) {
          FUN_10010356((int)pDVar4,0);
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


// 100107b2 __fltin2

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  local_24 = _Str;
  uVar3 = 0;
  local_18 = FUN_10010b4a((undefined2 *)&local_14,&local_28,_Str,0,0,0,0,in_stack_00000018);
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


// 10010860 _strlen

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
    if ((char)uVar1 == '\0') goto LAB_100108c3;
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
LAB_100108c3:
  return (size_t)((int)puVar2 + (-1 - (int)_Str));
}


// 100108eb __isctype_l

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


// 100109a1 FUN_100109a1

void __cdecl FUN_100109a1(undefined4 param_1)

{
  DAT_100231e4 = param_1;
  return;
}


// 100109ab __invoke_watson

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
  
  iVar1 = DAT_100202a0;
  local_2d4 = 0x10001;
  _memset(&local_32c,0,0x50);
  local_2dc.ExceptionRecord = &local_32c;
  local_2dc.ContextRecord = (PCONTEXT)&local_2d4;
  local_32c.ExceptionCode = 0xc000000d;
  BVar2 = IsDebuggerPresent();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  LVar3 = UnhandledExceptionFilter(&local_2dc);
  if ((LVar3 == 0) && (BVar2 == 0)) {
    FUN_10015704();
  }
  uExitCode = 0xc000000d;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  __security_check_cookie(iVar1);
  return;
}


// 10010aa7 FUN_10010aa7

void __cdecl
FUN_10010aa7(wchar_t *param_1,wchar_t *param_2,wchar_t *param_3,uint param_4,uintptr_t param_5)

{
  code *UNRECOVERED_JUMPTABLE;
  
  UNRECOVERED_JUMPTABLE = (code *)FUN_10010270(DAT_100231e4);
  if (UNRECOVERED_JUMPTABLE != (code *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x10010abb. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (*UNRECOVERED_JUMPTABLE)();
    return;
  }
  FUN_10015704();
                    /* WARNING: Subroutine does not return */
  __invoke_watson(param_1,param_2,param_3,param_4,param_5);
}


// 10010acb FUN_10010acb

int __cdecl FUN_10010acb(int param_1)

{
  uint uVar1;
  
  uVar1 = 0;
  do {
    if (param_1 == (&DAT_10020130)[uVar1 * 2]) {
      return *(int *)(uVar1 * 8 + 0x10020134);
    }
    uVar1 = uVar1 + 1;
  } while (uVar1 < 0x2d);
  if (param_1 - 0x13U < 0x12) {
    return 0xd;
  }
  return (-(uint)(0xe < param_1 - 0xbcU) & 0xe) + 8;
}


// 10010b06 FUN_10010b06

DWORD * FUN_10010b06(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_10010415();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10020298;
  }
  return pDVar1 + 2;
}


// 10010b19 FUN_10010b19

DWORD * FUN_10010b19(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_10010415();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_1002029c;
  }
  return pDVar1 + 3;
}


// 10010b2c FUN_10010b2c

void __cdecl FUN_10010b2c(DWORD param_1)

{
  DWORD *pDVar1;
  DWORD DVar2;
  
  pDVar1 = FUN_10010b19();
  *pDVar1 = param_1;
  DVar2 = FUN_10010acb(param_1);
  pDVar1 = FUN_10010b06();
  *pDVar1 = DVar2;
  return;
}


// 10010b4a FUN_10010b4a

void __cdecl
FUN_10010b4a(undefined2 *param_1,int *param_2,char *param_3,int param_4,int param_5,int param_6,
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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
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
LAB_10010bcf:
    cVar8 = *param_3;
    pcVar17 = param_3 + 1;
    switch(iVar14) {
    case 0:
      if ((byte)(cVar8 - 0x31U) < 9) {
LAB_10010bec:
        iVar14 = 3;
        goto LAB_10010bee;
      }
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) {
LAB_10010c03:
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
          if (cVar8 == '0') goto LAB_10010c1d;
          goto LAB_10010da8;
        }
        iVar14 = 2;
        uVar7 = 0x8000;
        param_3 = pcVar17;
      }
      goto LAB_10010bcf;
    case 1:
      bVar4 = true;
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_10010bec;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10010c54;
      if ((cVar8 == '+') || (cVar8 == '-')) goto LAB_10010c84;
      if (cVar8 == '0') goto LAB_10010c1d;
      goto LAB_10010c64;
    case 2:
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_10010bec;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10010c03;
      param_3 = local_58;
      if (cVar8 != '0') goto LAB_10010dd3;
LAB_10010c1d:
      iVar14 = 1;
      param_3 = pcVar17;
      goto LAB_10010bcf;
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
      if (cVar8 != *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10010cf7;
LAB_10010c54:
      bVar4 = true;
      iVar14 = 4;
      param_3 = pcVar17;
      goto LAB_10010bcf;
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
LAB_10010cf7:
      if ((cVar8 == '+') || (cVar8 == '-')) {
LAB_10010c84:
        bVar4 = true;
        iVar14 = 0xb;
        param_3 = pcVar17 + -1;
      }
      else {
LAB_10010c64:
        bVar4 = true;
        if ((cVar8 < 'D') || (('E' < cVar8 && ((cVar8 < 'd' || ('e' < cVar8)))))) goto LAB_10010da8;
        iVar14 = 6;
        param_3 = pcVar17;
      }
      goto LAB_10010bcf;
    case 5:
      bVar6 = true;
      param_3 = local_58;
      if ((byte)(cVar8 - 0x30U) < 10) {
        iVar14 = 4;
        goto LAB_10010bee;
      }
      goto LAB_10010dd3;
    case 6:
      local_58 = param_3 + -1;
      if (8 < (byte)(cVar8 - 0x31U)) {
        if (cVar8 == '+') goto LAB_10010d8f;
        if (cVar8 == '-') goto LAB_10010d83;
LAB_10010d76:
        param_3 = local_58;
        if (cVar8 != '0') goto LAB_10010dd3;
        iVar14 = 8;
        param_3 = pcVar17;
        goto LAB_10010bcf;
      }
      break;
    case 7:
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_10010d76;
      break;
    case 8:
      bVar5 = true;
      while (cVar8 == '0') {
        cVar8 = *pcVar17;
        pcVar17 = pcVar17 + 1;
      }
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_10010da8;
      break;
    case 9:
      bVar5 = true;
      local_68 = 0;
      goto LAB_10010e35;
    default:
      goto switchD_10010bdb_caseD_a;
    case 0xb:
      if (param_7 != 0) {
        local_58 = param_3;
        if (cVar8 == '+') {
LAB_10010d8f:
          iVar14 = 7;
          param_3 = pcVar17;
        }
        else {
          if (cVar8 != '-') goto LAB_10010dd3;
LAB_10010d83:
          local_6c = -1;
          iVar14 = 7;
          param_3 = pcVar17;
        }
        goto LAB_10010bcf;
      }
      iVar14 = 10;
      pcVar17 = param_3;
switchD_10010bdb_caseD_a:
      param_3 = pcVar17;
      if (iVar14 != 10) goto LAB_10010bcf;
      goto LAB_10010dd3;
    }
    iVar14 = 9;
LAB_10010bee:
    param_3 = pcVar17 + -1;
    goto LAB_10010bcf;
  }
  pDVar10 = FUN_10010b06();
  *pDVar10 = 0x16;
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
  goto LAB_100111cb;
LAB_10010e35:
  if ((cVar8 < '0') || ('9' < cVar8)) goto LAB_10010e50;
  local_68 = local_68 * 10 + -0x30 + (int)cVar8;
  if (local_68 < 0x1451) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
    goto LAB_10010e35;
  }
  local_68 = 0x1451;
LAB_10010e50:
  while (('/' < cVar8 && (cVar8 < ':'))) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
  }
LAB_10010da8:
  param_3 = pcVar17 + -1;
LAB_10010dd3:
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
    if (local_50 == 0) goto LAB_100111ad;
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
      if ((int)local_58 < -0x1450) goto LAB_100111ad;
      ppuVar20 = &PTR_DAT_10020680;
      if (local_58 != (char *)0x0) {
        if ((int)local_58 < 0) {
          local_58 = (char *)-(int)local_58;
          ppuVar20 = (undefined **)&DAT_100207e0;
        }
        if (param_4 == 0) {
          local_40._0_2_ = 0;
        }
        iVar14 = uStack_4a;
        uVar22 = uStack_2e;
        iVar1 = uStack_2a;
        uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
        iVar12 = local_3c;
joined_r0x10010edb:
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
LAB_10011091:
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
                    if ((short)uVar24 < 1) goto LAB_10011091;
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
                  if (0x7ffe < uVar24) goto LAB_10011150;
                  local_40 = uVar22;
                  local_3c = iVar1;
                  uStack_38 = uStack_26;
                  local_36 = uVar24 | uVar9;
                  uVar2 = uVar22;
                  iVar12 = iVar1;
                }
                goto joined_r0x10010edb;
              }
              iVar12 = 0;
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            else {
LAB_10011150:
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
          goto joined_r0x10010edb;
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
LAB_100111ad:
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
LAB_100111cb:
  local_40 = uVar2;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001120a __security_check_cookie

/* Library Function - Single Match
    @__security_check_cookie@4
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release
   __fastcall __security_check_cookie,4 */

void __fastcall __security_check_cookie(int param_1)

{
  if (param_1 == DAT_100202a0) {
    return;
  }
                    /* WARNING: Subroutine does not return */
  ___report_gsfailure();
}


// 10011219 __forcdecpt_l

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


// 1001128a __cropzeros_l

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


// 10011320 __fassign_l

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


// 10011377 __shift

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


// 100113b0 __cftoe2_l

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
    pDVar1 = FUN_10010b06();
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
      if (((DAT_10023ae4 & 1) != 0) && (_Dst[2] == '0')) {
        pcVar9 = (char *)0x3;
        _memmove(_Dst + 2,_Dst + 3,3);
      }
      if (local_8 == '\0') {
        return (DWORD)pcVar9;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return (DWORD)pcVar9;
    }
    pDVar1 = FUN_10010b06();
    DVar7 = 0x22;
  }
  DVar8 = 0;
  *pDVar1 = DVar7;
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return DVar8;
}


// 1001151d __cftoe_l

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_10010b06();
    *pDVar1 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100115eb __cftoe

/* Library Function - Single Match
    __cftoe
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __cftoe(double *_Value,char *_Buf,size_t _SizeInBytes,int _Dec,int _Caps)

{
  errno_t eVar1;
  
  eVar1 = __cftoe_l(_Value,_Buf,_SizeInBytes,_Dec,_Caps,(localeinfo_struct *)0x0);
  return eVar1;
}


// 10011609 __cftoa_l

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
    pDVar2 = FUN_10010b06();
    DVar15 = 0x16;
LAB_10011642:
    DVar16 = 0;
    *pDVar2 = DVar15;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_1c != '\0') {
      *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
    }
    return DVar16;
  }
  *param_2 = 0;
  if (param_3 <= param_4 + 0xb) {
    pDVar2 = FUN_10010b06();
    DVar15 = 0x22;
    goto LAB_10011642;
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
    goto LAB_10011966;
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
LAB_10011915:
    if ((-1 < (int)uVar8) && ((0 < (int)uVar8 || (99 < uVar7)))) goto LAB_10011920;
  }
  else {
    uVar14 = __alldvrm(uVar7,uVar8,1000,0);
    local_14 = (undefined4)((ulonglong)uVar14 >> 0x20);
    *pcVar10 = (char)uVar14 + '0';
    pcVar11 = pcVar4 + 3;
    uVar8 = 0;
    uVar7 = extraout_ECX;
    if (pcVar11 == pcVar10) goto LAB_10011915;
LAB_10011920:
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
LAB_10011966:
  if (local_1c == '\0') {
    return unaff_EBX;
  }
  *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  return unaff_EBX;
}


// 1001197a __cftof2_l

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
    pDVar2 = FUN_10010b06();
    uVar4 = 0x16;
    *pDVar2 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10011a6f __cftof_l

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_10010b06();
    *pDVar1 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10011b28 __cftog_l

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_34,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar2 = FUN_10010b06();
    *pDVar2 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10011c20 __cfltcvt_l

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


// 10011ca6 __cfltcvt

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


// 10011cc7 __initp_misc_cfltcvt_tab

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
    piVar1 = (int *)((int)&PTR_LAB_100202a8 + uVar3);
    iVar2 = FUN_100101f9(*piVar1);
    uVar3 = uVar3 + 4;
    *piVar1 = iVar2;
  } while (uVar3 < 0x28);
  return;
}


// 10011ce6 __setdefaultprecision

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


// 10011d0f __ms_p5_test_fdiv

/* WARNING: Removing unreachable block (ram,0x10011d42) */
/* Library Function - Single Match
    __ms_p5_test_fdiv
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

undefined4 __ms_p5_test_fdiv(void)

{
  return 0;
}


// 10011d4b __ms_p5_mp_test_fdiv

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


// 10011d74 fastzero_I

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


// 10011dcb __VEC_memzero

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


// 10011e6e __flsbuf

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
    pDVar2 = FUN_10010b06();
    *pDVar2 = 9;
LAB_10011e92:
    _File_00->_flag = _File_00->_flag | 0x20;
    return -1;
  }
  if ((uVar6 & 0x40) != 0) {
    pDVar2 = FUN_10010b06();
    *pDVar2 = 0x22;
    goto LAB_10011e92;
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
     (((ppuVar3 = FUN_1001683f(), _File_00 != (FILE *)(ppuVar3 + 8) &&
       (ppuVar3 = FUN_1001683f(), _File_00 != (FILE *)(ppuVar3 + 0x10))) ||
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
        puVar5 = &DAT_100203a8;
      }
      else {
        puVar5 = (undefined *)(((uint)_File & 0x1f) * 0x38 + (&DAT_10024c60)[(int)_File >> 5]);
      }
      if (((puVar5[4] & 0x20) != 0) &&
         (lVar7 = __lseeki64((int)_File,0x200000000,unaff_EDI), lVar7 == -1)) goto LAB_10011fb8;
    }
    else {
      local_8 = __write((int)_File,_Buf,uVar6);
    }
    *_File_00->_base = (char)_Ch;
  }
  if (local_8 == uVar6) {
    return _Ch & 0xff;
  }
LAB_10011fb8:
  _File_00->_flag = _File_00->_flag | 0x20;
  return -1;
}


// 10011fce write_char

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


// 10012001 write_multi_char

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


// 10012025 write_string

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
        pDVar1 = FUN_10010b06();
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


// 1001206f FUN_1001206f

/* WARNING: Type propagation algorithm not settling */

void __cdecl FUN_1001206f(FILE *param_1,byte *param_2,localeinfo_struct *param_3,wchar_t *param_4)

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
  
  local_8 = DAT_100202a0 ^ (uint)local_1fc;
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
        puVar7 = &DAT_100203a8;
      }
      else {
        iVar5 = __fileno(local_22c);
        uVar6 = __fileno(local_22c);
        puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_10024c60)[iVar5 >> 5]);
      }
      if ((puVar7[0x24] & 0x7f) == 0) {
        iVar5 = __fileno(local_22c);
        if ((iVar5 == -1) || (iVar5 = __fileno(local_22c), iVar5 == -2)) {
          puVar7 = &DAT_100203a8;
        }
        else {
          iVar5 = __fileno(local_22c);
          uVar6 = __fileno(local_22c);
          puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_10024c60)[iVar5 >> 5]);
        }
        if ((puVar7[0x24] & 0x80) == 0) goto LAB_100121ad;
      }
    }
    else {
LAB_100121ad:
      if (param_2 != (byte *)0x0) {
        local_215 = *param_2;
        local_230 = 0;
        local_224 = 0;
        local_250 = (wchar_t *)0x0;
        iVar5 = 0;
        while ((local_215 != 0 && (pbVar12 = param_2 + 1, local_244 = pbVar12, -1 < local_230))) {
          if ((byte)(local_215 - 0x20) < 0x59) {
            uVar6 = (byte)"KERNEL32"[(char)local_215 + 4] & 0xf;
          }
          else {
            uVar6 = 0;
          }
          local_270 = (int)(char)(&DAT_1001bc80)[uVar6 * 8 + iVar5] >> 4;
          switch(local_270) {
          case 0:
switchD_10012211_caseD_0:
            local_240 = 0;
            iVar5 = __isleadbyte_l((uint)local_215,&local_260);
            if (iVar5 != 0) {
              write_char();
              local_244 = param_2 + 2;
              if (*pbVar12 == 0) {
LAB_100129a4:
                pDVar4 = FUN_10010b06();
                *pDVar4 = 0x16;
                goto LAB_100120df;
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
                goto switchD_10012211_caseD_0;
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
LAB_100125f0:
                local_214 = local_214 | 0x40;
LAB_100125f4:
                local_224 = 10;
LAB_100125fb:
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
                  goto LAB_10012479;
                }
                if (local_215 == 0x41) {
LAB_10012425:
                  local_215 = local_215 + 0x20;
                  local_274 = 1;
LAB_10012432:
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
                      local_250 = (wchar_t *)FUN_10001110(sVar13);
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
                  pcVar9 = (code *)FUN_10010270((int)PTR_LAB_100202c0);
                  (*pcVar9)(puVar17,pwVar18,sVar13,iVar8,iVar5,uVar19,plVar20);
                  uVar6 = local_214 & 0x80;
                  if ((uVar6 != 0) && (local_21c == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_10010270((int)PTR_LAB_100202cc);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((local_215 == 0x67) && (uVar6 == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_10010270((int)PTR_LAB_100202c8);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((char)*pwVar14 == '-') {
                    local_214 = local_214 | 0x100;
                    local_220 = (wchar_t *)((int)pwVar14 + 1);
                    pwVar14 = local_220;
                  }
LAB_10012566:
                  local_224 = _strlen((char *)pwVar14);
                }
                else if (local_215 == 0x43) {
                  if ((local_214 & 0x830) == 0) {
                    local_214 = local_214 | 0x800;
                  }
LAB_100124d6:
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
                else if ((local_215 == 0x45) || (local_215 == 0x47)) goto LAB_10012425;
              }
              else {
                if (local_215 == 0x58) goto LAB_100126fb;
                if (local_215 == 0x5a) {
                  psVar2 = *(short **)param_4;
                  local_228 = param_4 + 2;
                  pwVar14 = (wchar_t *)PTR_s__null__100202d0;
                  local_220 = (wchar_t *)PTR_s__null__100202d0;
                  if ((psVar2 == (short *)0x0) ||
                     (pwVar18 = *(wchar_t **)(psVar2 + 2), pwVar18 == (wchar_t *)0x0))
                  goto LAB_10012566;
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
                  if (local_215 == 0x61) goto LAB_10012432;
                  if (local_215 == 99) goto LAB_100124d6;
                }
              }
LAB_10012873:
              if (local_24c == 0) {
                if ((local_214 & 0x40) != 0) {
                  if ((local_214 & 0x100) == 0) {
                    if ((local_214 & 1) == 0) {
                      if ((local_214 & 2) == 0) goto LAB_100128a9;
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
LAB_100128a9:
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
LAB_10012479:
                  iVar5 = local_21c;
                  if (local_21c == -1) {
                    iVar5 = 0x7fffffff;
                  }
                  local_228 = param_4 + 2;
                  local_220 = *(wchar_t **)param_4;
                  if ((local_214 & 0x810) == 0) {
                    pwVar14 = local_220;
                    if (local_220 == (wchar_t *)0x0) {
                      pwVar14 = (wchar_t *)PTR_s__null__100202d0;
                      local_220 = (wchar_t *)PTR_s__null__100202d0;
                    }
                    for (; (iVar5 != 0 && (iVar5 = iVar5 + -1, (char)*pwVar14 != '\0'));
                        pwVar14 = (wchar_t *)((int)pwVar14 + 1)) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220;
                  }
                  else {
                    if (local_220 == (wchar_t *)0x0) {
                      local_220 = (wchar_t *)PTR_u__null__100202d4;
                    }
                    local_240 = 1;
                    for (pwVar14 = local_220;
                        (iVar5 != 0 && (iVar5 = iVar5 + -1, *pwVar14 != L'\0'));
                        pwVar14 = pwVar14 + 1) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220 >> 1;
                  }
                  goto LAB_10012873;
                }
                if (local_215 == 0x75) goto LAB_100125f4;
                if (local_215 != 0x78) goto LAB_10012873;
                local_248 = 0x27;
LAB_10012721:
                local_224 = 0x10;
                if ((local_214 & 0x80) != 0) {
                  local_233 = (char)local_248 + 'Q';
                  local_234 = 0x30;
                  local_238 = 2;
                }
                goto LAB_100125fb;
              }
              if (local_215 == 0x70) {
                local_21c = 8;
LAB_100126fb:
                local_248 = 7;
                goto LAB_10012721;
              }
              if ((char)local_215 < 'e') goto LAB_10012873;
              if ((char)local_215 < 'h') goto LAB_10012432;
              if (local_215 == 0x69) goto LAB_100125f0;
              if (local_215 != 0x6e) {
                if (local_215 != 0x6f) goto LAB_10012873;
                local_224 = 8;
                if ((local_214 & 0x80) != 0) {
                  local_214 = local_214 | 0x200;
                }
                goto LAB_100125fb;
              }
              piVar3 = *(int **)param_4;
              local_228 = param_4 + 2;
              bVar15 = FUN_10016a0d();
              if (CONCAT31(extraout_var,bVar15) == 0) goto LAB_100129a4;
              if ((local_214 & 0x20) == 0) {
                *piVar3 = local_230;
              }
              else {
                *(undefined2 *)piVar3 = (undefined2)local_230;
              }
              local_24c = 1;
            }
            if (local_250 != (wchar_t *)0x0) {
              FUN_100012c0(local_250);
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
        goto LAB_100129cb;
      }
    }
  }
  pDVar4 = FUN_10010b06();
  *pDVar4 = 0x16;
LAB_100120df:
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_254 != '\0') {
    *(uint *)(local_258 + 0x70) = *(uint *)(local_258 + 0x70) & 0xfffffffd;
  }
LAB_100129cb:
  __security_check_cookie(local_8 ^ (uint)local_1fc);
  return;
}


// 10012a03 FUN_10012a03

void __cdecl FUN_10012a03(undefined4 *param_1,undefined4 *param_2,uint param_3)

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


// 10012a8a __VEC_memcpy

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
      FUN_10012a03(param_1,param_2,param_3 - uVar3);
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


// 10012b6d type_info::_Type_info_dtor

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
  piVar1 = DAT_100231ec;
  if (*(int *)(param_1 + 4) != 0) {
    piVar2 = (int *)&DAT_100231e8;
    do {
      piVar3 = piVar2;
      if (DAT_100231ec == (int *)0x0) goto LAB_10012bb1;
      piVar2 = DAT_100231ec;
    } while (*DAT_100231ec != *(int *)(param_1 + 4));
    piVar3[1] = DAT_100231ec[1];
    FUN_100012c0(piVar1);
LAB_10012bb1:
    FUN_100012c0(*(undefined4 *)(param_1 + 4));
    *(undefined4 *)(param_1 + 4) = 0;
  }
  FUN_10012bd4();
  return;
}


// 10012bd4 FUN_10012bd4

void FUN_10012bd4(void)

{
  FUN_10014265(0xe);
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
  
  if (((byte)DAT_100202e0 & 1) != 0) {
    FUN_10012d79(10);
  }
  p_Var1 = ___get_sigabrt();
  if (p_Var1 != (_PHNDLR)0x0) {
    _raise(0x16);
  }
  if (((byte)DAT_100202e0 & 2) != 0) {
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
  DAT_100202e0 = ~param_2 & DAT_100202e0 | param_1 & param_2;
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
  HANDLE hFile;
  uint uVar6;
  DWORD *lpNumberOfBytesWritten;
  LPOVERLAPPED lpOverlapped;
  DWORD DStack_4;
  
  uVar6 = 0;
  do {
    if (param_1 == (&DAT_100202e8)[uVar6 * 2]) break;
    uVar6 = uVar6 + 1;
  } while (uVar6 < 0x17);
  if (uVar6 < 0x17) {
    iVar2 = __set_error_mode(3);
    if ((iVar2 == 1) || ((iVar2 = __set_error_mode(3), iVar2 == 0 && (DAT_10023190 == 1)))) {
      hFile = GetStdHandle(0xfffffff4);
      if ((hFile != (HANDLE)0x0) && (hFile != (HANDLE)0xffffffff)) {
        lpOverlapped = (LPOVERLAPPED)0x0;
        lpNumberOfBytesWritten = &DStack_4;
        puVar1 = (undefined4 *)(uVar6 * 8 + 0x100202ec);
        sVar5 = _strlen((char *)*puVar1);
        WriteFile(hFile,(LPCVOID)*puVar1,sVar5,lpNumberOfBytesWritten,lpOverlapped);
      }
    }
    else if (param_1 != 0xfc) {
      eVar3 = _strcpy_s(&DAT_100231f0,0x314,"Runtime Error!\n\nProgram: ");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      DAT_1002330d = 0;
      DVar4 = GetModuleFileNameA((HMODULE)0x0,&DAT_10023209,0x104);
      if ((DVar4 == 0) &&
         (eVar3 = _strcpy_s(&DAT_10023209,0x2fb,"<program name unknown>"), eVar3 != 0)) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      sVar5 = _strlen(&DAT_10023209);
      if (0x3c < sVar5 + 1) {
        sVar5 = _strlen(&DAT_10023209);
        eVar3 = _strncpy_s((char *)(sVar5 + 0x100231ce),
                           (int)&DAT_10023504 - (int)(sVar5 + 0x100231ce),"...",3);
        if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
      }
      eVar3 = _strcat_s(&DAT_100231f0,0x314,"\n\n");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      eVar3 = _strcat_s(&DAT_100231f0,0x314,*(char **)(uVar6 * 8 + 0x100202ec));
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      FUN_10016e3f(&DAT_100231f0,"Microsoft Visual C++ Runtime Library",0x12010);
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
    if (DAT_10023190 != 1) {
      return;
    }
  }
  FUN_10012d79(0xfc);
  FUN_10012d79(0xff);
  return;
}


// 10012f52 FUN_10012f52

void __cdecl FUN_10012f52(undefined4 param_1)

{
  DAT_1002350c = param_1;
  return;
}


// 10012f5c __amsg_exit

/* Library Function - Single Match
    __amsg_exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __amsg_exit(int param_1)

{
  code *pcVar1;
  
  __FF_MSGBANNER();
  FUN_10012d79(param_1);
  pcVar1 = (code *)FUN_10010270((int)PTR___exit_100203a0);
  (*pcVar1)(0xff);
  return;
}


// 10012f80 FUN_10012f80

void __cdecl FUN_10012f80(undefined4 param_1)

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


// 10012fa6 ___crtExitProcess

/* Library Function - Single Match
    ___crtExitProcess
   
   Library: Visual Studio 2005 Release */

void __cdecl ___crtExitProcess(int param_1)

{
  FUN_10012f80(param_1);
                    /* WARNING: Subroutine does not return */
  ExitProcess(param_1);
}


// 10012fbb FUN_10012fbb

void FUN_10012fbb(void)

{
  __lock(8);
  return;
}


// 10012fc4 FUN_10012fc4

void FUN_10012fc4(void)

{
  FUN_10014265(8);
  return;
}


// 10012fcd FUN_10012fcd

void __cdecl FUN_10012fcd(undefined4 *param_1)

{
  undefined4 *in_EAX;
  
  for (; in_EAX < param_1; in_EAX = in_EAX + 1) {
    if ((code *)*in_EAX != (code *)0x0) {
      (*(code *)*in_EAX)();
    }
  }
  return;
}


// 10012fe5 __initterm_e

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


// 10013005 __get_osplatform

/* Library Function - Single Match
    __get_osplatform
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __get_osplatform(int *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (int *)0x0) && (DAT_10023514 != 0)) {
    *param_1 = DAT_10023514;
    return 0;
  }
  pDVar1 = FUN_10010b06();
  *pDVar1 = 0x16;
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 1001303c FUN_1001303c

undefined4 __cdecl FUN_1001303c(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (undefined4 *)0x0) && (DAT_10023514 != 0)) {
    *param_1 = DAT_10023520;
    return 0;
  }
  pDVar1 = FUN_10010b06();
  *pDVar1 = 0x16;
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 10013078 __cinit

/* Library Function - Single Match
    __cinit
   
   Library: Visual Studio 2005 Release */

int __cdecl __cinit(int param_1)

{
  BOOL BVar1;
  int iVar2;
  undefined4 *puVar3;
  
  BVar1 = __IsNonwritableInCurrentImage((PBYTE)&PTR___fpmath_1001bb14);
  if (BVar1 != 0) {
    __fpmath(param_1);
  }
  __initp_misc_cfltcvt_tab();
  iVar2 = __initterm_e((undefined4 *)&DAT_1001a19c,(undefined4 *)&DAT_1001a1b4);
  if (iVar2 == 0) {
    _atexit((_func_4879 *)&LAB_10013b67);
    puVar3 = &DAT_1001a170;
    do {
      if ((code *)*puVar3 != (code *)0x0) {
        (*(code *)*puVar3)();
      }
      puVar3 = puVar3 + 1;
    } while (puVar3 < &DAT_1001a198);
    if ((DAT_10024d74 != (code *)0x0) &&
       (BVar1 = __IsNonwritableInCurrentImage((PBYTE)&DAT_10024d74), BVar1 != 0)) {
      (*DAT_10024d74)(0,2,0);
    }
    iVar2 = 0;
  }
  return iVar2;
}


// 1001310a FUN_1001310a

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x100131dd) */

void __cdecl FUN_1001310a(int param_1,int param_2,int param_3)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  int iVar4;
  code *pcVar5;
  
  __lock(8);
  if (DAT_10023554 != 1) {
    DAT_10023550 = 1;
    DAT_1002354c = (undefined1)param_3;
    if (param_2 == 0) {
      piVar2 = (int *)FUN_10010270(DAT_10024d6c);
      piVar3 = (int *)FUN_10010270(DAT_10024d68);
      if (piVar2 != (int *)0x0) {
        while (piVar3 = piVar3 + -1, piVar2 <= piVar3) {
          if (*piVar3 != 0) {
            iVar1 = *piVar3;
            iVar4 = __encoded_null();
            if (iVar1 != iVar4) {
              pcVar5 = (code *)FUN_10010270(iVar1);
              (*pcVar5)();
            }
          }
        }
      }
      FUN_10012fcd((undefined4 *)&DAT_1001a1c4);
    }
    FUN_10012fcd((undefined4 *)&DAT_1001a1cc);
  }
  FUN_100131d7();
  if (param_3 != 0) {
    return;
  }
  DAT_10023554 = 1;
  FUN_10014265(8);
  ___crtExitProcess(param_1);
  return;
}


// 100131d7 FUN_100131d7

void FUN_100131d7(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + 0x10) != 0) {
    FUN_10014265(8);
  }
  return;
}


// 100131ec __exit

/* Library Function - Single Match
    __exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __exit(int _Code)

{
  FUN_1001310a(_Code,1,0);
  return;
}


// 100131fd __cexit

/* Library Function - Single Match
    __cexit
   
   Library: Visual Studio 2005 Release */

void __cdecl __cexit(void)

{
  FUN_1001310a(0,0,1);
  return;
}


// 1001320c __init_pointers

/* Library Function - Single Match
    __init_pointers
   
   Library: Visual Studio 2005 Release */

void __cdecl __init_pointers(void)

{
  undefined4 uVar1;
  
  uVar1 = __encoded_null();
  FUN_100172be(uVar1);
  FUN_100171df(uVar1);
  FUN_100109a1(uVar1);
  FUN_10012f52(uVar1);
  FUN_100171d5(uVar1);
  __initp_misc_winsig(uVar1);
  FUN_1000dc87();
  __initp_eh_hooks();
  PTR___exit_100203a0 = (undefined *)FUN_100101f9(0x100131ec);
  return;
}


// 10013258 __SEH_prolog4

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
  *(uint *)((int)auStack_1c + iVar1 + 4) = DAT_100202a0 ^ (uint)&param_2;
  *(undefined4 *)((int)auStack_1c + iVar1) = unaff_retaddr;
  *unaff_FS_OFFSET = (int)local_8;
  return;
}


// 1001329d __SEH_epilog4

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


// 100132c0 __except_handler4

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
  
  piVar8 = (int *)(*(uint *)((int)param_2 + 8) ^ DAT_100202a0);
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
          goto LAB_1001336b;
        }
        if (0 < iVar4) {
          if (((*param_1 == -0x1f928c9d) && (DAT_10024d60 != (code *)0x0)) &&
             (BVar5 = __IsNonwritableInCurrentImage((PBYTE)&DAT_10024d60), BVar5 != 0)) {
            (*DAT_10024d60)(param_1,1);
          }
          _EH4_GlobalUnwind(param_2);
          if (*(uint *)((int)param_2 + 0xc) != uVar7) {
            _EH4_LocalUnwind((int)param_2,uVar7,iVar1,&DAT_100202a0);
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
    _EH4_LocalUnwind((int)param_2,0xfffffffe,iVar1,&DAT_100202a0);
  }
LAB_1001336b:
  if (*piVar8 != -2) {
    __security_check_cookie(piVar8[1] + iVar1 ^ *(uint *)(*piVar8 + iVar1));
  }
  __security_check_cookie(piVar8[3] + iVar1 ^ *(uint *)(piVar8[2] + iVar1));
  return local_c;
}


// 10013456 __ioinit

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
  
  uStack_c = 0x10013462;
  local_8 = 0;
  GetStartupInfoA(&local_68);
  local_8 = 0xfffffffe;
  puVar2 = (undefined4 *)FUN_10001250(0x20,0x38);
  if (puVar2 == (undefined4 *)0x0) {
LAB_1001368d:
    iVar7 = -1;
  }
  else {
    DAT_10024c48 = 0x20;
    DAT_10024c60 = puVar2;
    for (; puVar2 < DAT_10024c60 + 0x1c0; puVar2 = puVar2 + 0xe) {
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
      while ((UVar10 = UVar9, (int)DAT_10024c48 < (int)UVar9 &&
             (puVar2 = (undefined4 *)FUN_10001250(0x20,0x38), UVar10 = DAT_10024c48,
             puVar2 != (undefined4 *)0x0))) {
        (&DAT_10024c60)[iVar7] = puVar2;
        DAT_10024c48 = DAT_10024c48 + 0x20;
        puVar1 = puVar2;
        for (; puVar2 < puVar1 + 0x1c0; puVar2 = puVar2 + 0xe) {
          *(undefined1 *)(puVar2 + 1) = 0;
          *puVar2 = 0xffffffff;
          *(undefined1 *)((int)puVar2 + 5) = 10;
          puVar2[2] = 0;
          *(byte *)(puVar2 + 9) = *(byte *)(puVar2 + 9) & 0x80;
          *(undefined1 *)((int)puVar2 + 0x25) = 10;
          *(undefined1 *)((int)puVar2 + 0x26) = 10;
          puVar1 = (&DAT_10024c60)[iVar7];
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
                     ((local_24 & 0x1f) * 0x38 + (int)(&DAT_10024c60)[(int)local_24 >> 5]);
            *puVar2 = *(undefined4 *)local_20;
            *(byte *)(puVar2 + 1) = (byte)*pUVar6;
            iVar7 = ___crtInitCritSecAndSpinCount(puVar2 + 3,4000);
            if (iVar7 == 0) goto LAB_1001368d;
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
      piVar8 = DAT_10024c60 + iVar7 * 0xe;
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
          if (iVar5 == 0) goto LAB_1001368d;
          piVar8[2] = piVar8[2] + 1;
        }
      }
      else {
        *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 0x80;
      }
      iVar7 = iVar7 + 1;
    } while (iVar7 < 3);
    SetHandleCount(DAT_10024c48);
    iVar7 = 0;
  }
  return iVar7;
}


// 10013696 __ioterm

/* Library Function - Single Match
    __ioterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __ioterm(void)

{
  uint uVar1;
  uint *puVar2;
  uint uVar3;
  
  puVar2 = &DAT_10024c60;
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
      FUN_100012c0(*puVar2);
      *puVar2 = 0;
    }
    puVar2 = puVar2 + 1;
  } while ((int)puVar2 < 0x10024d60);
  return;
}


// 100136e2 __setenvp

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
  
  if (DAT_10024d70 == 0) {
    ___initmbctable();
  }
  iVar5 = 0;
  pcVar4 = DAT_10023184;
  if (DAT_10023184 != (char *)0x0) {
    for (; *pcVar4 != '\0'; pcVar4 = pcVar4 + sVar2 + 1) {
      if (*pcVar4 != '=') {
        iVar5 = iVar5 + 1;
      }
      sVar2 = _strlen(pcVar4);
    }
    puVar1 = (undefined4 *)FUN_10001250(iVar5 + 1,4);
    pcVar4 = DAT_10023184;
    DAT_10023534 = puVar1;
    if (puVar1 != (undefined4 *)0x0) {
      do {
        if (*pcVar4 == '\0') {
          FUN_100012c0(DAT_10023184);
          DAT_10023184 = (char *)0x0;
          *puVar1 = 0;
          _DAT_10024d64 = 1;
          return 0;
        }
        sVar2 = _strlen(pcVar4);
        _SizeInBytes = sVar2 + 1;
        if (*pcVar4 != '=') {
          _Dst = (char *)FUN_10001250(_SizeInBytes,1);
          *puVar1 = _Dst;
          if (_Dst == (char *)0x0) {
            FUN_100012c0(DAT_10023534);
            DAT_10023534 = (undefined4 *)0x0;
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


// 100137bd parse_cmdline

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
        goto LAB_1001384f;
      }
    }
    in_EDX = pbVar7;
  } while ((bVar2) || ((bVar6 != 0x20 && (bVar6 != 9))));
  if (pbVar5 != (byte *)0x0) {
    pbVar5[-1] = 0;
  }
LAB_1001384f:
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


// 10013955 __setargv

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
  
  if (DAT_10024d70 == 0) {
    ___initmbctable();
  }
  DAT_1002365c = 0;
  GetModuleFileNameA((HMODULE)0x0,&DAT_10023558,0x104);
  _DAT_10023544 = &DAT_10023558;
  if ((DAT_10024d80 == (char *)0x0) || (local_8 = DAT_10024d80, *DAT_10024d80 == '\0')) {
    local_8 = &DAT_10023558;
  }
  parse_cmdline((undefined4 *)0x0,(byte *)0x0,(int *)&local_c);
  uVar2 = local_c;
  if ((local_c < 0x3fffffff) && (local_10 != 0xffffffff)) {
    uVar1 = local_c * 4 + local_10;
    if ((local_10 <= uVar1) &&
       (puVar3 = (undefined4 *)FUN_10001110(uVar1), puVar3 != (undefined4 *)0x0)) {
      parse_cmdline(puVar3,(byte *)(puVar3 + uVar2),(int *)&local_c);
      _DAT_10023528 = local_c - 1;
      _DAT_1002352c = puVar3;
      return 0;
    }
  }
  return -1;
}


// 10013a0e ___crtGetEnvironmentStringsA

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
  if (DAT_10023660 == 0) {
    lpWideCharStr = GetEnvironmentStringsW();
    if (lpWideCharStr != (LPWCH)0x0) {
      DAT_10023660 = 1;
      goto LAB_10013a60;
    }
    DVar3 = GetLastError();
    if (DVar3 == 0x78) {
      DAT_10023660 = 2;
    }
  }
  if (DAT_10023660 != 1) {
    if ((DAT_10023660 != 2) && (DAT_10023660 != 0)) {
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
    _Dst = (void *)FUN_10001110(pcVar7 + (1 - (int)_Src));
    if (_Dst == (void *)0x0) {
      FreeEnvironmentStringsA(_Src);
      return (LPVOID)0x0;
    }
    _memcpy(_Dst,_Src,(size_t)(pcVar7 + (1 - (int)_Src)));
    FreeEnvironmentStringsA(_Src);
    return _Dst;
  }
LAB_10013a60:
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
      (lpMultiByteStr = (LPSTR)FUN_10001110(cbMultiByte), lpMultiByteStr != (LPSTR)0x0)) &&
     (iVar6 = WideCharToMultiByte(0,0,lpWideCharStr,iVar6,lpMultiByteStr,cbMultiByte,(LPCSTR)0x0,
                                  (LPBOOL)0x0), local_8 = lpMultiByteStr, iVar6 == 0)) {
    FUN_100012c0(lpMultiByteStr);
    local_8 = (LPSTR)0x0;
  }
  FreeEnvironmentStringsW(lpWideCharStr);
  return local_8;
}


// 10013b43 __RTC_Initialize

/* WARNING: Removing unreachable block (ram,0x10013b55) */
/* WARNING: Removing unreachable block (ram,0x10013b5b) */
/* WARNING: Removing unreachable block (ram,0x10013b5d) */
/* Library Function - Single Match
    __RTC_Initialize
   
   Library: Visual Studio 2005 Release */

void __RTC_Initialize(void)

{
  return;
}


// 10013b8b FUN_10013b8b

DWORD * __cdecl FUN_10013b8b(int param_1,DWORD param_2)

{
  int *piVar1;
  code *pcVar2;
  DWORD DVar3;
  DWORD DVar4;
  DWORD *pDVar5;
  int *piVar6;
  int iVar7;
  int iVar8;
  
  pDVar5 = FUN_10010415();
  if (pDVar5 != (DWORD *)0x0) {
    piVar1 = (int *)pDVar5[0x17];
    piVar6 = piVar1;
    do {
      if (*piVar6 == param_1) break;
      piVar6 = piVar6 + 3;
    } while (piVar6 < piVar1 + DAT_10020464 * 3);
    if ((piVar1 + DAT_10020464 * 3 <= piVar6) || (*piVar6 != param_1)) {
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
          if (DAT_10020458 < DAT_1002045c + DAT_10020458) {
            iVar7 = DAT_10020458 * 0xc;
            iVar8 = DAT_10020458;
            do {
              *(undefined4 *)(iVar7 + 8 + pDVar5[0x17]) = 0;
              iVar8 = iVar8 + 1;
              iVar7 = iVar7 + 0xc;
            } while (iVar8 < DAT_1002045c + DAT_10020458);
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


// 10013d04 FUN_10013d04

undefined4 FUN_10013d04(void)

{
  return 1;
}


// 10013d0a ___security_init_cookie

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
  if ((DAT_100202a0 == 0xbb40e64e) || ((DAT_100202a0 & 0xffff0000) == 0)) {
    GetSystemTimeAsFileTime(&local_c);
    uVar4 = local_c.dwHighDateTime ^ local_c.dwLowDateTime;
    DVar1 = GetCurrentProcessId();
    DVar2 = GetCurrentThreadId();
    DVar3 = GetTickCount();
    QueryPerformanceCounter(&local_14);
    DAT_100202a0 = uVar4 ^ DVar1 ^ DVar2 ^ DVar3 ^ local_14.s.HighPart ^ local_14.s.LowPart;
    if (DAT_100202a0 == 0xbb40e64e) {
      DAT_100202a0 = 0xbb40e64f;
    }
    else if ((DAT_100202a0 & 0xffff0000) == 0) {
      DAT_100202a0 = DAT_100202a0 | DAT_100202a0 << 0x10;
    }
    DAT_100202a4 = ~DAT_100202a0;
  }
  else {
    DAT_100202a4 = ~DAT_100202a0;
  }
  return;
}


// 10013d9e __crtLCMapStringA_stat

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  if (DAT_10023664 == 0) {
    iVar3 = LCMapStringW(0,0x100,L"",1,(LPWSTR)0x0,0);
    if (iVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_10023664 = 2;
      }
    }
    else {
      DAT_10023664 = 1;
    }
  }
  pcVar5 = (char *)param_3;
  pcVar8 = param_4;
  if (0 < (int)param_4) {
    do {
      pcVar8 = pcVar8 + -1;
      if (*pcVar5 == '\0') goto LAB_10013e0d;
      pcVar5 = pcVar5 + 1;
    } while (pcVar8 != (char *)0x0);
    pcVar8 = (char *)0xffffffff;
LAB_10013e0d:
    pcVar5 = param_4 + -(int)pcVar8;
    bVar2 = (int)(pcVar5 + -1) < (int)param_4;
    param_4 = pcVar5 + -1;
    if (bVar2) {
      param_4 = pcVar5;
    }
  }
  if ((DAT_10023664 == 2) || (DAT_10023664 == 0)) {
    local_10 = (LPCWSTR)0x0;
    local_14 = 0;
    if (param_1 == (localeinfo_struct *)0x0) {
      param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
    }
    if (param_7 == 0) {
      param_7 = *(int *)(*in_ECX + 4);
    }
    UVar7 = ___ansicp((LCID)param_1);
    if (UVar7 == 0xffffffff) goto LAB_1001412e;
    if (UVar7 == param_7) {
      LCMapStringA((LCID)param_1,param_2,(LPCSTR)param_3,(int)param_4,(LPSTR)param_5,(int)param_6);
    }
    else {
      local_10 = (LPCWSTR)___convertcp(param_7,UVar7,(char *)param_3,(uint *)&param_4,(LPSTR)0x0,0);
      if (local_10 == (LPCWSTR)0x0) goto LAB_1001412e;
      local_c = LCMapStringA((LCID)param_1,param_2,(LPCSTR)local_10,(int)param_4,(LPSTR)0x0,0);
      if (local_c != 0) {
        if (((int)local_c < 1) || (0xffffffe0 < local_c)) {
          pcVar5 = (char *)0x0;
        }
        else if (local_c + 8 < 0x401) {
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_1001410b;
          pcVar5 = &stack0xffffffe4;
        }
        else {
          pcVar5 = (char *)FUN_10001110(local_c + 8);
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
LAB_1001410b:
    if (local_10 != (LPCWSTR)0x0) {
      FUN_100012c0(local_10);
    }
    if ((local_14 != 0) && (param_5 != local_14)) {
      FUN_100012c0(local_14);
    }
    goto LAB_1001412e;
  }
  if (DAT_10023664 != 1) goto LAB_1001412e;
  local_c = 0;
  if (param_7 == 0) {
    param_7 = *(int *)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar(param_7,(uint)(param_8 != 0) * 8 + 1,(LPCSTR)param_3,
                                    (int)param_4,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_1001412e;
  if (((int)cchWideChar < 1) || (0xffffffe0 / cchWideChar < 2)) {
    local_10 = (LPCWSTR)0x0;
  }
  else {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar6 = (LPCWSTR)&stack0xffffffdc;
      local_10 = (LPCWSTR)&stack0xffffffdc;
      if (&stack0x00000000 != (undefined1 *)0x24) {
LAB_10013eb5:
        local_10 = pWVar6 + 4;
      }
    }
    else {
      pWVar6 = (LPCWSTR)FUN_10001110(uVar1);
      local_10 = pWVar6;
      if (pWVar6 != (LPCWSTR)0x0) {
        pWVar6[0] = L'\xdddd';
        pWVar6[1] = L'\0';
        goto LAB_10013eb5;
      }
    }
  }
  if (local_10 == (LPCWSTR)0x0) goto LAB_1001412e;
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
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_10013fc4;
          pWVar6 = (LPCWSTR)&stack0xffffffe4;
        }
        else {
          pWVar6 = (LPCWSTR)FUN_10001110(uVar1);
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
LAB_10013fc4:
  __freea(local_10);
LAB_1001412e:
  iVar3 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar3;
}


// 10014140 ___crtLCMapStringA

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


// 10014183 __isleadbyte_l

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


// 100141b9 _isleadbyte

/* Library Function - Single Match
    _isleadbyte
   
   Library: Visual Studio 2005 Release */

int __cdecl _isleadbyte(int _C)

{
  int iVar1;
  
  iVar1 = __isleadbyte_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 100141c7 __mtinitlocks

/* Library Function - Single Match
    __mtinitlocks
   
   Library: Visual Studio 2005 Release */

int __cdecl __mtinitlocks(void)

{
  int iVar1;
  int iVar2;
  undefined *puVar3;
  
  iVar2 = 0;
  puVar3 = &DAT_10023668;
  do {
    if ((&DAT_1002046c)[iVar2 * 2] == 1) {
      (&DAT_10020468)[iVar2 * 2] = puVar3;
      puVar3 = puVar3 + 0x18;
      iVar1 = ___crtInitCritSecAndSpinCount((&DAT_10020468)[iVar2 * 2],4000);
      if (iVar1 == 0) {
        (&DAT_10020468)[iVar2 * 2] = 0;
        return 0;
      }
    }
    iVar2 = iVar2 + 1;
  } while (iVar2 < 0x24);
  return 1;
}


// 10014210 __mtdeletelocks

/* Library Function - Single Match
    __mtdeletelocks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __cdecl __mtdeletelocks(void)

{
  LPCRITICAL_SECTION lpCriticalSection;
  undefined4 *puVar1;
  
  puVar1 = &DAT_10020468;
  do {
    lpCriticalSection = (LPCRITICAL_SECTION)*puVar1;
    if ((lpCriticalSection != (LPCRITICAL_SECTION)0x0) && (puVar1[1] != 1)) {
      DeleteCriticalSection(lpCriticalSection);
      FUN_100012c0(lpCriticalSection);
      *puVar1 = 0;
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10020588);
  puVar1 = &DAT_10020468;
  do {
    if (((LPCRITICAL_SECTION)*puVar1 != (LPCRITICAL_SECTION)0x0) && (puVar1[1] == 1)) {
      DeleteCriticalSection((LPCRITICAL_SECTION)*puVar1);
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10020588);
  return;
}


// 10014265 FUN_10014265

void __cdecl FUN_10014265(int param_1)

{
  LeaveCriticalSection((LPCRITICAL_SECTION)(&DAT_10020468)[param_1 * 2]);
  return;
}


// 1001427a __mtinitlocknum

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
  if (DAT_1001f004 == 0) {
    __FF_MSGBANNER();
    FUN_10012d79(0x1e);
    ___crtExitProcess(0xff);
  }
  piVar1 = &DAT_10020468 + _LockNum * 2;
  if (*piVar1 == 0) {
    iVar4 = FUN_10001110(0x18);
    if (iVar4 == 0) {
      pDVar2 = FUN_10010b06();
      *pDVar2 = 0xc;
      iVar4 = 0;
    }
    else {
      __lock(10);
      if (*piVar1 == 0) {
        iVar3 = ___crtInitCritSecAndSpinCount(iVar4,4000);
        if (iVar3 == 0) {
          FUN_100012c0(iVar4);
          pDVar2 = FUN_10010b06();
          *pDVar2 = 0xc;
          local_20 = 0;
        }
        else {
          *piVar1 = iVar4;
        }
      }
      else {
        FUN_100012c0(iVar4);
      }
      FUN_10014334();
      iVar4 = local_20;
    }
  }
  return iVar4;
}


// 10014334 FUN_10014334

void FUN_10014334(void)

{
  FUN_10014265(10);
  return;
}


// 1001433d __lock

/* Library Function - Single Match
    __lock
   
   Library: Visual Studio 2005 Release */

void __cdecl __lock(int _File)

{
  int iVar1;
  
  if ((&DAT_10020468)[_File * 2] == 0) {
    iVar1 = __mtinitlocknum(_File);
    if (iVar1 == 0) {
      __amsg_exit(0x11);
    }
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(&DAT_10020468)[_File * 2]);
  return;
}


// 1001436e wcstoxl

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
    pDVar2 = FUN_10010b06();
    *pDVar2 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
LAB_10014405:
    _C = *pwVar7;
    pwVar7 = pwVar1 + 2;
  }
  else if (_C == L'+') goto LAB_10014405;
  uVar6 = (uint)(ushort)_C;
  if (param_4 == 0) {
    iVar3 = __wchartodigit(_C);
    if (iVar3 != 0) {
      param_4 = 10;
      goto LAB_10014469;
    }
    if ((*pwVar7 != L'x') && (*pwVar7 != L'X')) {
      param_4 = 8;
      goto LAB_10014469;
    }
    param_4 = 0x10;
  }
  if (((param_4 == 0x10) && (iVar3 = __wchartodigit(_C), iVar3 == 0)) &&
     ((*pwVar7 == L'x' || (*pwVar7 == L'X')))) {
    uVar6 = (uint)(ushort)pwVar7[1];
    pwVar7 = pwVar7 + 2;
  }
LAB_10014469:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  local_c = (uint)(0xffffffff % (ulonglong)(uint)param_4);
  do {
    uVar8 = (ushort)uVar6;
    uVar5 = __wchartodigit(uVar8);
    if (uVar5 == 0xffffffff) {
      if (((uVar8 < 0x41) || (0x5a < uVar8)) && (0x19 < (ushort)(uVar8 - 0x61))) {
LAB_100144c8:
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
          pDVar2 = FUN_10010b06();
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
    if ((uint)param_4 <= uVar5) goto LAB_100144c8;
    if ((uVar9 < uVar4) || ((uVar9 == uVar4 && (uVar5 <= local_c)))) {
      uVar9 = uVar9 * param_4 + uVar5;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (wchar_t **)0x0) goto LAB_100144c8;
    }
    uVar6 = (uint)(ushort)*pwVar7;
    pwVar7 = pwVar7 + 1;
  } while( true );
}


// 10014565 _wcstol

/* Library Function - Single Match
    _wcstol
   
   Library: Visual Studio 2005 Release */

long __cdecl _wcstol(wchar_t *_Str,wchar_t **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_100231b0 == 0) {
    ppuVar2 = &PTR_DAT_10020120;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = wcstoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 1001458e __iswctype_l

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
    local_8[0] = *(ushort *)(PTR_DAT_100205a0 + (uint)_C * 2) & _Type;
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


// 10014610 __crtGetStringTypeA_stat

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  local_c = in_ECX;
  if (DAT_100237b8 == 0) {
    BVar2 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_100237b8 = 2;
      }
      goto LAB_10014669;
    }
    DAT_100237b8 = 1;
  }
  else {
LAB_10014669:
    if ((DAT_100237b8 == 2) || (DAT_100237b8 == 0)) {
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
        FUN_100012c0(uVar7);
      }
      goto LAB_100147b6;
    }
    if (DAT_100237b8 != 1) goto LAB_100147b6;
  }
  local_c = (int *)0x0;
  if (param_5 == (ushort *)0x0) {
    param_5 = *(ushort **)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar((UINT)param_5,(uint)(param_7 != 0) * 8 + 1,(LPCSTR)param_2,
                                    (int)param_3,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_100147b6;
  lpWideCharStr = (LPCWSTR)0x0;
  if ((0 < (int)cchWideChar) && (cchWideChar < 0x7ffffff1)) {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar4 = (LPCWSTR)&stack0xffffffe8;
      lpWideCharStr = (LPCWSTR)&stack0xffffffe8;
      if (&stack0x00000000 != (undefined1 *)0x18) {
LAB_100146f9:
        lpWideCharStr = pWVar4 + 4;
      }
    }
    else {
      pWVar4 = (LPCWSTR)FUN_10001110(uVar1);
      lpWideCharStr = pWVar4;
      if (pWVar4 != (LPCWSTR)0x0) {
        pWVar4[0] = L'\xdddd';
        pWVar4[1] = L'\0';
        goto LAB_100146f9;
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
LAB_100147b6:
  iVar5 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar5;
}


// 100147c8 ___crtGetStringTypeA

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


// 10014808 ___free_lc_time

/* Library Function - Single Match
    ___free_lc_time
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lc_time(undefined4 *param_1)

{
  if (param_1 != (undefined4 *)0x0) {
    FUN_100012c0(param_1[1]);
    FUN_100012c0(param_1[2]);
    FUN_100012c0(param_1[3]);
    FUN_100012c0(param_1[4]);
    FUN_100012c0(param_1[5]);
    FUN_100012c0(param_1[6]);
    FUN_100012c0(*param_1);
    FUN_100012c0(param_1[8]);
    FUN_100012c0(param_1[9]);
    FUN_100012c0(param_1[10]);
    FUN_100012c0(param_1[0xb]);
    FUN_100012c0(param_1[0xc]);
    FUN_100012c0(param_1[0xd]);
    FUN_100012c0(param_1[7]);
    FUN_100012c0(param_1[0xe]);
    FUN_100012c0(param_1[0xf]);
    FUN_100012c0(param_1[0x10]);
    FUN_100012c0(param_1[0x11]);
    FUN_100012c0(param_1[0x12]);
    FUN_100012c0(param_1[0x13]);
    FUN_100012c0(param_1[0x14]);
    FUN_100012c0(param_1[0x15]);
    FUN_100012c0(param_1[0x16]);
    FUN_100012c0(param_1[0x17]);
    FUN_100012c0(param_1[0x18]);
    FUN_100012c0(param_1[0x19]);
    FUN_100012c0(param_1[0x1a]);
    FUN_100012c0(param_1[0x1b]);
    FUN_100012c0(param_1[0x1c]);
    FUN_100012c0(param_1[0x1d]);
    FUN_100012c0(param_1[0x1e]);
    FUN_100012c0(param_1[0x1f]);
    FUN_100012c0(param_1[0x20]);
    FUN_100012c0(param_1[0x21]);
    FUN_100012c0(param_1[0x22]);
    FUN_100012c0(param_1[0x23]);
    FUN_100012c0(param_1[0x24]);
    FUN_100012c0(param_1[0x25]);
    FUN_100012c0(param_1[0x26]);
    FUN_100012c0(param_1[0x27]);
    FUN_100012c0(param_1[0x28]);
    FUN_100012c0(param_1[0x29]);
    FUN_100012c0(param_1[0x2a]);
  }
  return;
}


// 10014998 ___free_lconv_num

/* Library Function - Single Match
    ___free_lconv_num
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_num(int *param_1)

{
  if (param_1 != (int *)0x0) {
    if ((undefined *)*param_1 != PTR_DAT_10020668) {
      FUN_100012c0((undefined *)*param_1);
    }
    if ((undefined *)param_1[1] != PTR_DAT_1002066c) {
      FUN_100012c0((undefined *)param_1[1]);
    }
    if ((undefined *)param_1[2] != PTR_DAT_10020670) {
      FUN_100012c0((undefined *)param_1[2]);
    }
  }
  return;
}


// 100149d8 ___free_lconv_mon

/* Library Function - Single Match
    ___free_lconv_mon
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_mon(int param_1)

{
  if (param_1 != 0) {
    if (*(undefined **)(param_1 + 0xc) != PTR_DAT_10020674) {
      FUN_100012c0(*(undefined **)(param_1 + 0xc));
    }
    if (*(undefined **)(param_1 + 0x10) != PTR_DAT_10020678) {
      FUN_100012c0(*(undefined **)(param_1 + 0x10));
    }
    if (*(undefined **)(param_1 + 0x14) != PTR_DAT_1002067c) {
      FUN_100012c0(*(undefined **)(param_1 + 0x14));
    }
    if (*(undefined **)(param_1 + 0x18) != PTR_DAT_10020680) {
      FUN_100012c0(*(undefined **)(param_1 + 0x18));
    }
    if (*(undefined **)(param_1 + 0x1c) != PTR_DAT_10020684) {
      FUN_100012c0(*(undefined **)(param_1 + 0x1c));
    }
    if (*(undefined **)(param_1 + 0x20) != PTR_DAT_10020688) {
      FUN_100012c0(*(undefined **)(param_1 + 0x20));
    }
    if (*(undefined **)(param_1 + 0x24) != PTR_DAT_1002068c) {
      FUN_100012c0(*(undefined **)(param_1 + 0x24));
    }
  }
  return;
}


// 10014a61 _strcat_s

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
        pDVar2 = FUN_10010b06();
        eVar4 = 0x22;
        *pDVar2 = 0x22;
        goto LAB_10014a80;
      }
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_10010b06();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_10014a80:
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 10014ae0 _strcspn

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


// 10014b26 _strncpy_s

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
LAB_10014b4a:
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
          pDVar2 = FUN_10010b06();
          eVar5 = 0x22;
          *pDVar2 = 0x22;
          goto LAB_10014b5b;
        }
        *_Dst = '\0';
      }
    }
  }
  else if (_Dst != (char *)0x0) goto LAB_10014b4a;
  pDVar2 = FUN_10010b06();
  eVar5 = 0x16;
  *pDVar2 = 0x16;
LAB_10014b5b:
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar5;
}


// 10014bd9 _strcpy_s

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
      pDVar2 = FUN_10010b06();
      eVar4 = 0x22;
      *pDVar2 = 0x22;
      goto LAB_10014bf8;
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_10010b06();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_10014bf8:
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 10014c40 _strpbrk

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


// 10014c80 __ld12tod

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
        goto LAB_1001517f;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_100206b8 - 1;
    iVar4 = (int)(DAT_100206b8 + ((int)DAT_100206b8 >> 0x1f & 0x1fU)) >> 5;
    uVar8 = DAT_100206b8 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar6) goto LAB_10014db3;
          bVar14 = _Ifp < p_Var9;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_10014dba;
LAB_10014db3:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_10014dba:
              iVar4 = iVar4 + -1;
              *puVar6 = (uint)_Ifp;
              if ((iVar4 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_10014dc8;
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
LAB_10014dc8:
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
    if (iVar12 < (int)(DAT_100206b4 - DAT_100206b8)) {
      local_24[0] = 0;
      local_24[1] = 0;
    }
    else {
      if (DAT_100206b4 < iVar12) {
        if (iVar12 < DAT_100206b0) {
          local_24[0] = local_24[0] & 0x7fffffff;
          iVar12 = iVar12 + DAT_100206c4;
          iVar2 = (int)(DAT_100206bc + ((int)DAT_100206bc >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_100206bc & 0x8000001f;
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
          iVar12 = (int)(DAT_100206bc + ((int)DAT_100206bc >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_100206bc & 0x8000001f;
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
          iVar12 = DAT_100206c4 + DAT_100206b0;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_1001517f;
      }
      local_14 = DAT_100206b4 - local_14;
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
      iVar2 = DAT_100206b8 - 1;
      iVar12 = (int)(DAT_100206b8 + ((int)DAT_100206b8 >> 0x1f & 0x1fU)) >> 5;
      uVar10 = DAT_100206b8 & 0x8000001f;
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
          if (2 < iVar12) goto LAB_10014f6b;
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
LAB_10014f6b:
      *puVar7 = *puVar7 & -1 << ((byte)local_14 & 0x1f);
      iVar12 = local_10 + 1;
      if (iVar12 < 3) {
        puVar7 = local_24 + iVar12;
        for (iVar2 = 3 - iVar12; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar7 = 0;
          puVar7 = puVar7 + 1;
        }
      }
      uVar10 = DAT_100206bc + 1;
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
LAB_1001517f:
  local_24[0] = iVar12 << (0x1fU - (char)DAT_100206bc & 0x1f) |
                -(uint)(local_24[3] != 0) & 0x80000000 | local_24[0];
  if (DAT_100206c0 == 0x40) {
    *(uint *)((int)&_D->x + 4) = local_24[0];
    *(uint *)&_D->x = local_24[1];
  }
  else if (DAT_100206c0 == 0x20) {
    *(uint *)&_D->x = local_24[0];
  }
  return IVar3;
}


// 100151c2 __ld12tof

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
        goto LAB_100156c1;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_100206d0 - 1;
    iVar2 = (int)(DAT_100206d0 + ((int)DAT_100206d0 >> 0x1f & 0x1fU)) >> 5;
    uVar7 = DAT_100206d0 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar5) goto LAB_100152f5;
          bVar14 = _Ifp < p_Var8;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_100152fc;
LAB_100152f5:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_100152fc:
              iVar2 = iVar2 + -1;
              *puVar5 = (uint)_Ifp;
              if ((iVar2 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_1001530a;
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
LAB_1001530a:
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
    if (iVar11 < (int)(DAT_100206cc - DAT_100206d0)) {
      local_24 = 0;
      local_20 = 0.0;
    }
    else {
      if (DAT_100206cc < iVar11) {
        if (iVar11 < DAT_100206c8) {
          local_24 = local_24 & 0x7fffffff;
          iVar11 = iVar11 + DAT_100206dc;
          iVar2 = (int)(DAT_100206d4 + ((int)DAT_100206d4 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_100206d4 & 0x8000001f;
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
          iVar11 = (int)(DAT_100206d4 + ((int)DAT_100206d4 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_100206d4 & 0x8000001f;
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
          iVar11 = DAT_100206dc + DAT_100206c8;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_100156c1;
      }
      local_1c[2] = DAT_100206cc - local_1c[2];
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
      iVar2 = DAT_100206d0 - 1;
      iVar11 = (int)(DAT_100206d0 + ((int)DAT_100206d0 >> 0x1f & 0x1fU)) >> 5;
      uVar9 = DAT_100206d0 & 0x8000001f;
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
          if (2 < iVar11) goto LAB_100154ad;
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
LAB_100154ad:
      *puVar6 = *puVar6 & -1 << ((byte)local_1c[2] & 0x1f);
      iVar11 = local_1c[3] + 1;
      if (iVar11 < 3) {
        puVar6 = &local_24 + iVar11;
        for (iVar2 = 3 - iVar11; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar6 = 0;
          puVar6 = puVar6 + 1;
        }
      }
      uVar9 = DAT_100206d4 + 1;
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
LAB_100156c1:
  fVar12 = (float)(iVar11 << (0x1fU - (char)DAT_100206d4 & 0x1f) |
                   -(uint)(local_1c[1] != 0) & 0x80000000 | local_24);
  if (DAT_100206d8 == 0x40) {
    _F[1].f = fVar12;
    _F->f = local_20;
  }
  else if (DAT_100206d8 == 0x20) {
    _F->f = fVar12;
  }
  return IVar3;
}


// 10015704 FUN_10015704

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10015704(void)

{
  _DAT_10024c44 = 0;
  return;
}


// 1001570c ___mtold12

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
  
  uVar7 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
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


// 100158d8 ___report_gsfailure

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
  
  _DAT_100238d8 =
       (uint)(in_NT & 1) * 0x4000 | (uint)SBORROW4((int)&stack0xfffffffc,0x328) * 0x800 |
       (uint)(in_IF & 1) * 0x200 | (uint)(in_TF & 1) * 0x100 | (uint)((int)&local_32c < 0) * 0x80 |
       (uint)(&stack0x00000000 == (undefined1 *)0x32c) * 0x40 | (uint)(in_AF & 1) * 0x10 |
       (uint)((POPCOUNT((uint)&local_32c & 0xff) & 1U) == 0) * 4 |
       (uint)(&stack0xfffffffc < (undefined1 *)0x328) | (uint)(in_ID & 1) * 0x200000 |
       (uint)(in_VIP & 1) * 0x100000 | (uint)(in_VIF & 1) * 0x80000 | (uint)(in_AC & 1) * 0x40000;
  _DAT_100238dc = &stack0x00000004;
  _DAT_10023818 = 0x10001;
  _DAT_100237c0 = 0xc0000409;
  _DAT_100237c4 = 1;
  local_32c = DAT_100202a0;
  local_328 = DAT_100202a4;
  _DAT_100237cc = unaff_retaddr;
  _DAT_100238a4 = in_GS;
  _DAT_100238a8 = in_FS;
  _DAT_100238ac = in_ES;
  _DAT_100238b0 = in_DS;
  _DAT_100238b4 = unaff_EDI;
  _DAT_100238b8 = unaff_ESI;
  _DAT_100238bc = unaff_EBX;
  _DAT_100238c0 = in_EDX;
  _DAT_100238c4 = in_ECX;
  _DAT_100238c8 = in_EAX;
  _DAT_100238cc = unaff_EBP;
  DAT_100238d0 = unaff_retaddr;
  _DAT_100238d4 = in_CS;
  _DAT_100238e0 = in_SS;
  DAT_10023810 = IsDebuggerPresent();
  FUN_10015704();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  UnhandledExceptionFilter((_EXCEPTION_POINTERS *)&PTR_DAT_1001cbf4);
  if (DAT_10023810 == 0) {
    FUN_10015704();
  }
  uExitCode = 0xc0000409;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  return;
}


// 100159dc FID_conflict:__atoflt_l

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_10010b4a((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tod(&local_14,(_CRT_DOUBLE *)_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_10015a33:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10015a73;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_10015a65:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10015a73;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_10015a65;
    goto LAB_10015a33;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_10015a73:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 10015a82 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_10010b4a((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tof(&local_14,_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_10015ad9:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10015b19;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_10015b0b:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10015b19;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_10015b0b;
    goto LAB_10015ad9;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_10015b19:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 10015b28 __fptostr

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
    pDVar1 = FUN_10010b06();
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
    pDVar1 = FUN_10010b06();
    eVar7 = 0x22;
    *pDVar1 = 0x22;
  }
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar7;
}


// 10015be5 ___dtold

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
      goto LAB_10015c97;
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
LAB_10015c97:
  *(ushort *)(param_1 + 2) = uVar4;
  return;
}


// 10015ca0 __fltout2

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  ___dtold(&local_14,(uint *)&_Dbl);
  iVar1 = FUN_10017b86(local_14,uStack_10,uStack_c,0x11,0,&local_30);
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


// 10015d30 __alldvrm

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


// 10015e10 __aullshr

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


// 10015e38 __controlfp_s

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
    pDVar2 = FUN_10010b06();
    eVar3 = 0x16;
    *pDVar2 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return eVar3;
}


// 10015ea2 FUN_10015ea2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

undefined4 FUN_10015ea2(void)

{
  return 1;
}


// 10015ef2 __get_sse2_info

/* WARNING: Removing unreachable block (ram,0x10015f2d) */
/* WARNING: Removing unreachable block (ram,0x10015f1a) */
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
  if (((local_8 & 0x4000000) == 0) || (iVar2 = FUN_10015ea2(), iVar2 == 0)) {
    uVar3 = 0;
  }
  else {
    uVar3 = 1;
  }
  return uVar3;
}


// 10015f5f __lseeki64_nolock

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
    pDVar2 = FUN_10010b06();
    *pDVar2 = 9;
LAB_10015f8e:
    DVar3 = 0xffffffff;
    local_8 = -1;
  }
  else {
    DVar3 = SetFilePointer(hFile,in_stack_00000008,&local_8,_Offset._4_4_);
    if (DVar3 == 0xffffffff) {
      DVar4 = GetLastError();
      if (DVar4 != 0) {
        FUN_10010b2c(DVar4);
        goto LAB_10015f8e;
      }
    }
    pbVar1 = (byte *)((&DAT_10024c60)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38);
    *pbVar1 = *pbVar1 & 0xfd;
  }
  return CONCAT44(local_8,DVar3);
}


// 10015fe2 __lseeki64

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
    pDVar1 = FUN_10010b19();
    *pDVar1 = 0;
    pDVar1 = FUN_10010b06();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10024c48)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_10024c60)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
        pDVar1 = FUN_10010b19();
        *pDVar1 = 0;
        pDVar1 = FUN_10010b06();
        *pDVar1 = 9;
        FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        local_28._4_4_ = 0xffffffff;
        local_28._0_4_ = 0xffffffff;
      }
      else {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_10024c60)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_10010b06();
          *pDVar1 = 9;
          pDVar1 = FUN_10010b19();
          *pDVar1 = 0;
          local_28 = -1;
        }
        else {
          local_28 = __lseeki64_nolock(_FileHandle,_Offset,in_stack_ffffffc8);
        }
        FUN_100160f1();
      }
      goto LAB_100160eb;
    }
    pDVar1 = FUN_10010b19();
    *pDVar1 = 0;
    pDVar1 = FUN_10010b06();
    *pDVar1 = 9;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  local_28._0_4_ = 0xffffffff;
  local_28._4_4_ = 0xffffffff;
LAB_100160eb:
  return CONCAT44(local_28._4_4_,(undefined4)local_28);
}


// 100160f1 FUN_100160f1

void FUN_100160f1(void)

{
  int unaff_EBP;
  
  FUN_10018a0f(*(uint *)(unaff_EBP + 8));
  return;
}


// 100160fb __write_nolock

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
  
  local_8 = DAT_100202a0 ^ (uint)local_51c;
  local_580 = (WCHAR *)_Buf;
  local_57c = (char *)0x0;
  local_584 = 0;
  if (_MaxCharCount == 0) goto LAB_100166ab;
  if (_Buf == (void *)0x0) {
    pDVar6 = FUN_10010b19();
    *pDVar6 = 0;
    pDVar6 = FUN_10010b06();
    *pDVar6 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_100166ab;
  }
  iVar11 = (_FileHandle & 0x1fU) * 0x38;
  piVar16 = &DAT_10024c60 + (_FileHandle >> 5);
  local_571 = (char)(*(char *)(*piVar16 + iVar11 + 0x24) * '\x02') >> 1;
  local_58c = piVar16;
  if (((local_571 == '\x02') || (local_571 == '\x01')) && ((~_MaxCharCount & 1) == 0)) {
    pDVar6 = FUN_10010b19();
    *pDVar6 = 0;
    pDVar6 = FUN_10010b06();
    *pDVar6 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_100166ab;
  }
  if ((*(byte *)(*piVar16 + iVar11 + 4) & 0x20) != 0) {
    __lseeki64_nolock(_FileHandle,0x200000000,unaff_EDI);
  }
  iVar7 = __isatty(_FileHandle);
  if ((iVar7 == 0) || ((*(byte *)(iVar11 + 4 + *piVar16) & 0x80) == 0)) {
LAB_100163e6:
    if ((*(byte *)((undefined4 *)(*piVar16 + iVar11) + 1) & 0x80) == 0) {
      BVar9 = WriteFile(*(HANDLE *)(*piVar16 + iVar11),local_580,_MaxCharCount,(LPDWORD)&local_588,
                        (LPOVERLAPPED)0x0);
      if (BVar9 == 0) {
LAB_10016640:
        local_570 = GetLastError();
      }
      else {
        local_570 = 0;
        local_57c = local_588;
      }
LAB_10016649:
      piVar16 = local_58c;
      if (local_57c != (char *)0x0) goto LAB_100166ab;
      goto LAB_10016653;
    }
    local_570 = 0;
    if (local_571 == '\0') {
      local_56c = local_580;
      if (_MaxCharCount == 0) goto LAB_1001667a;
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
        if (BVar9 == 0) goto LAB_10016640;
        local_57c = local_57c + (int)local_588;
      } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
              ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
      goto LAB_10016649;
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
          if (BVar9 == 0) goto LAB_10016640;
          local_57c = local_57c + (int)local_588;
        } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
                ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
        goto LAB_10016649;
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
          if (iVar7 == 0) goto LAB_10016640;
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
        goto LAB_10016649;
      }
    }
LAB_1001667a:
    DVar10 = 0;
    if (((*(byte *)(iVar11 + 4 + *piVar16) & 0x40) != 0) && ((char)*local_580 == '\x1a'))
    goto LAB_100166ab;
    pDVar6 = FUN_10010b06();
    *pDVar6 = 0x1c;
  }
  else {
    p_Var8 = __getptd();
    pwVar3 = p_Var8->ptlocinfo->lc_category[0].wlocale;
    BVar9 = GetConsoleMode(*(HANDLE *)(iVar11 + *piVar16),&local_598);
    if ((BVar9 == 0) || ((pwVar3 == (wchar_t *)0x0 && (local_571 == '\0')))) goto LAB_100163e6;
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
          if (BVar9 == 0) goto LAB_10016640;
          local_57c = local_57c + (int)local_56c;
          if ((int)local_56c < (int)DVar10) break;
          if (local_594 != 0) {
            local_10[0] = '\r';
            BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_10,1,(LPDWORD)&local_56c,
                              (LPOVERLAPPED)0x0);
            if (BVar9 == 0) goto LAB_10016640;
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
            if (wVar5 != (wchar_t)local_570) goto LAB_10016640;
            local_57c = local_57c + 1;
            if (local_594 != 0) {
              local_570 = 0xd;
              wVar5 = __putwch_nolock(L'\r');
              if (wVar5 != (wchar_t)local_570) goto LAB_10016640;
              local_57c = local_57c + 1;
              local_584 = local_584 + 1;
            }
          }
        }
      } while (local_578 < _MaxCharCount);
      goto LAB_10016649;
    }
LAB_10016653:
    if (local_570 == 0) goto LAB_1001667a;
    DVar10 = 5;
    if (local_570 != 5) {
      FUN_10010b2c(local_570);
      goto LAB_100166ab;
    }
    pDVar6 = FUN_10010b06();
    *pDVar6 = 9;
  }
  pDVar6 = FUN_10010b19();
  *pDVar6 = DVar10;
LAB_100166ab:
  iVar11 = __security_check_cookie(local_8 ^ (uint)local_51c);
  return iVar11;
}


// 100166c1 __write

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
    pDVar1 = FUN_10010b19();
    *pDVar1 = 0;
    pDVar1 = FUN_10010b06();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10024c48)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_10024c60)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_10024c60)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_10010b06();
          *pDVar1 = 9;
          pDVar1 = FUN_10010b19();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = __write_nolock(_FileHandle,_Buf,_MaxCharCount);
        }
        FUN_10016793();
        return local_20;
      }
    }
    pDVar1 = FUN_10010b19();
    *pDVar1 = 0;
    pDVar1 = FUN_10010b06();
    *pDVar1 = 9;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 10016793 FUN_10016793

void FUN_10016793(void)

{
  int unaff_EBP;
  
  FUN_10018a0f(*(uint *)(unaff_EBP + 8));
  return;
}


// 1001679d __getbuf

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __getbuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __getbuf(FILE *_File)

{
  char *pcVar1;
  
  _DAT_10023ae8 = _DAT_10023ae8 + 1;
  pcVar1 = (char *)FUN_10001110(0x1000);
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


// 100167e1 __isatty

/* Library Function - Single Match
    __isatty
   
   Library: Visual Studio 2005 Release */

int __cdecl __isatty(int _FileHandle)

{
  DWORD *pDVar1;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_10010b06();
    *pDVar1 = 9;
    return 0;
  }
  if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10024c48)) {
    return *(byte *)((&DAT_10024c60)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38) & 0x40;
  }
  pDVar1 = FUN_10010b06();
  *pDVar1 = 9;
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0;
}


// 1001683f FUN_1001683f

undefined ** FUN_1001683f(void)

{
  return &PTR_DAT_100209a0;
}


// 100169e0 __fileno

/* Library Function - Single Match
    __fileno
   
   Library: Visual Studio 2005 Release */

int __cdecl __fileno(FILE *_File)

{
  DWORD *pDVar1;
  
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_10010b06();
    *pDVar1 = 0x16;
    FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  return _File->_file;
}


// 10016a0d FUN_10016a0d

bool FUN_10016a0d(void)

{
  return DAT_10023aec == (DAT_100202a0 | 1);
}


// 10016a23 __wctomb_s_l

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
LAB_10016a45:
    DVar1 = 0;
  }
  else {
    if (_SizeConverted != (int *)0x0) {
      *_SizeConverted = -1;
    }
    if (0x7fffffff < _SizeInBytes) {
      pDVar2 = FUN_10010b06();
      *pDVar2 = 0x16;
      FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return 0x16;
    }
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
    if (*(int *)(local_14[0] + 0x14) == 0) {
      if ((ushort)_WCh < 0x100) {
        if (lpMultiByteStr != (char *)0x0) {
          if (_Size == 0) goto LAB_10016ad5;
          *lpMultiByteStr = (char)_WCh;
        }
        if (_SizeConverted != (int *)0x0) {
          *_SizeConverted = 1;
        }
LAB_10016b0c:
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        goto LAB_10016a45;
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
LAB_10016ad5:
          pDVar2 = FUN_10010b06();
          *pDVar2 = 0x22;
          FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_10016b0c;
      }
    }
    pDVar2 = FUN_10010b06();
    *pDVar2 = 0x2a;
    pDVar2 = FUN_10010b06();
    DVar1 = *pDVar2;
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return DVar1;
}


// 10016b82 _wctomb_s

/* Library Function - Single Match
    _wctomb_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _wctomb_s(int *_SizeConverted,char *_MbCh,rsize_t _SizeInBytes,wchar_t _WCh)

{
  errno_t eVar1;
  
  eVar1 = __wctomb_s_l(_SizeConverted,_MbCh,_SizeInBytes,_WCh,(_locale_t)0x0);
  return eVar1;
}


// 10016ba0 __aulldvrm

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


// 10016c35 __initp_misc_winsig

/* Library Function - Single Match
    __initp_misc_winsig
   
   Library: Visual Studio 2005 Release */

void __cdecl __initp_misc_winsig(undefined4 param_1)

{
  DAT_10023b2c = param_1;
  DAT_10023b30 = param_1;
  DAT_10023b34 = param_1;
  DAT_10023b38 = param_1;
  return;
}


// 10016c4e siglookup

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
  } while (uVar1 < DAT_10020464 * 0xc + param_1);
  if ((DAT_10020464 * 0xc + param_1 <= uVar1) || (*(int *)(uVar1 + 4) != in_EDX)) {
    uVar1 = 0;
  }
  return uVar1;
}


// 10016c82 ___get_sigabrt

/* Library Function - Single Match
    ___get_sigabrt
   
   Library: Visual Studio 2005 Release */

_PHNDLR __cdecl ___get_sigabrt(void)

{
  _PHNDLR p_Var1;
  
  p_Var1 = (_PHNDLR)FUN_10010270(DAT_10023b34);
  return p_Var1;
}


// 10016c8f _raise

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
        puVar6 = &DAT_10023b2c;
        iVar3 = DAT_10023b2c;
        goto LAB_10016d44;
      }
      if (_SigNum != 4) {
        if (_SigNum == 6) goto LAB_10016d22;
        if (_SigNum != 8) goto LAB_10016d06;
      }
    }
    pDVar7 = FUN_10010415();
    if (pDVar7 == (DWORD *)0x0) {
      return -1;
    }
    uVar2 = siglookup(pDVar7[0x17]);
    puVar6 = (undefined4 *)(uVar2 + 8);
    pcVar4 = (code *)*puVar6;
  }
  else {
    if (_SigNum == 0xf) {
      puVar6 = &DAT_10023b38;
      iVar3 = DAT_10023b38;
    }
    else if (_SigNum == 0x15) {
      puVar6 = &DAT_10023b30;
      iVar3 = DAT_10023b30;
    }
    else {
      if (_SigNum != 0x16) {
LAB_10016d06:
        pDVar7 = FUN_10010b06();
        *pDVar7 = 0x16;
        FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return -1;
      }
LAB_10016d22:
      puVar6 = &DAT_10023b34;
      iVar3 = DAT_10023b34;
    }
LAB_10016d44:
    bVar1 = true;
    pcVar4 = (code *)FUN_10010270(iVar3);
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
      goto LAB_10016da8;
    }
  }
  else {
LAB_10016da8:
    if (_SigNum == 8) {
      for (local_28 = DAT_10020458; local_28 < DAT_1002045c + DAT_10020458; local_28 = local_28 + 1)
      {
        *(undefined4 *)(local_28 * 0xc + 8 + pDVar7[0x17]) = 0;
      }
      goto LAB_10016de2;
    }
  }
  uVar5 = __encoded_null();
  *puVar6 = uVar5;
LAB_10016de2:
  FUN_10016e03();
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


// 10016e03 FUN_10016e03

void FUN_10016e03(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + -0x1c) != 0) {
    FUN_10014265(0);
  }
  return;
}


// 10016e3f FUN_10016e3f

undefined4 __cdecl FUN_10016e3f(undefined4 param_1,undefined4 param_2,uint param_3)

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
  if (DAT_10023b40 == 0) {
    hModule = LoadLibraryA("USER32.DLL");
    if (hModule == (HMODULE)0x0) {
      return 0;
    }
    pFVar1 = GetProcAddress(hModule,"MessageBoxA");
    if (pFVar1 == (FARPROC)0x0) {
      return 0;
    }
    DAT_10023b40 = FUN_100101f9((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetActiveWindow");
    DAT_10023b44 = FUN_100101f9((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetLastActivePopup");
    DAT_10023b48 = FUN_100101f9((int)pFVar1);
    iVar2 = __get_osplatform(&local_10);
    if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
    if (local_10 == 2) {
      pFVar1 = GetProcAddress(hModule,"GetUserObjectInformationA");
      DAT_10023b50 = FUN_100101f9((int)pFVar1);
      if (DAT_10023b50 != 0) {
        pFVar1 = GetProcAddress(hModule,"GetProcessWindowStation");
        DAT_10023b4c = FUN_100101f9((int)pFVar1);
      }
    }
  }
  if ((DAT_10023b4c != local_c) && (DAT_10023b50 != local_c)) {
    pcVar3 = (code *)FUN_10010270(DAT_10023b4c);
    pcVar4 = (code *)FUN_10010270(DAT_10023b50);
    if (((pcVar3 != (code *)0x0) && (pcVar4 != (code *)0x0)) &&
       (((iVar2 = (*pcVar3)(), iVar2 == 0 ||
         (iVar2 = (*pcVar4)(iVar2,1,local_24,0xc,local_18), iVar2 == 0)) || ((local_1c & 1) == 0))))
    {
      iVar2 = FUN_1001303c(&local_14);
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
      goto LAB_10016fd5;
    }
  }
  if ((((DAT_10023b44 != local_c) &&
       (pcVar3 = (code *)FUN_10010270(DAT_10023b44), pcVar3 != (code *)0x0)) &&
      (local_8 = (*pcVar3)(), local_8 != 0)) &&
     ((DAT_10023b48 != local_c &&
      (pcVar3 = (code *)FUN_10010270(DAT_10023b48), pcVar3 != (code *)0x0)))) {
    local_8 = (*pcVar3)(local_8);
  }
LAB_10016fd5:
  pcVar3 = (code *)FUN_10010270(DAT_10023b40);
  if (pcVar3 == (code *)0x0) {
    return 0;
  }
  uVar5 = (*pcVar3)(local_8,param_1,param_2,param_3);
  return uVar5;
}


// 10016ffc __set_error_mode

/* Library Function - Single Match
    __set_error_mode
   
   Library: Visual Studio 2005 Release */

int __cdecl __set_error_mode(int _Mode)

{
  int iVar1;
  DWORD *pDVar2;
  
  if (-1 < _Mode) {
    if (_Mode < 3) {
      iVar1 = DAT_1002318c;
      DAT_1002318c = _Mode;
      return iVar1;
    }
    if (_Mode == 3) {
      return DAT_1002318c;
    }
  }
  pDVar2 = FUN_10010b06();
  *pDVar2 = 0x16;
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 10017050 __ValidateImageBase

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


// 10017080 __FindPESection

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


// 100170d0 __IsNonwritableInCurrentImage

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
  local_c = DAT_100202a0 ^ 0x1001dcb8;
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


// 100171c4 __initp_eh_hooks

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __initp_eh_hooks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __initp_eh_hooks(void)

{
  _DAT_10023b54 = FUN_100101f9(0x1001718b);
  return;
}


// 100171d5 FUN_100171d5

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100171d5(undefined4 param_1)

{
  _DAT_10023b58 = param_1;
  return;
}


// 100171df FUN_100171df

void __cdecl FUN_100171df(undefined4 param_1)

{
  DAT_10023b5c = param_1;
  return;
}


// 100171e9 ___crtInitCritSecNoSpinCount@8

/* Library Function - Single Match
    ___crtInitCritSecNoSpinCount@8
   
   Library: Visual Studio 2005 Release */

undefined4 ___crtInitCritSecNoSpinCount_8(LPCRITICAL_SECTION param_1)

{
  InitializeCriticalSection(param_1);
  return 1;
}


// 100171f9 ___crtInitCritSecAndSpinCount

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
  
  local_8 = &DAT_1001dcf8;
  uStack_c = 0x10017205;
  local_20[0] = 0;
  pcVar1 = (code *)FUN_10010270(DAT_10023b5c);
  if (pcVar1 != (FARPROC)0x0) goto LAB_1001726f;
  iVar2 = __get_osplatform(local_20);
  if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  if (local_20[0] == 1) {
LAB_1001725e:
    pcVar1 = ___crtInitCritSecNoSpinCount_8;
  }
  else {
    hModule = GetModuleHandleA("kernel32.dll");
    if (hModule == (HMODULE)0x0) goto LAB_1001725e;
    pcVar1 = GetProcAddress(hModule,"InitializeCriticalSectionAndSpinCount");
    if (pcVar1 == (FARPROC)0x0) goto LAB_1001725e;
  }
  DAT_10023b5c = FUN_100101f9((int)pcVar1);
LAB_1001726f:
  local_8 = (undefined *)0x0;
  iVar2 = (*pcVar1)(param_1,param_2);
  return iVar2;
}


// 100172be FUN_100172be

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100172be(undefined4 param_1)

{
  _DAT_10023b60 = param_1;
  return;
}


// 100172c8 __local_unwind4

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
  puStack_24 = &LAB_10017358;
  uStack_28 = *unaff_FS_OFFSET;
  local_20 = DAT_100202a0 ^ (uint)&uStack_28;
  *unaff_FS_OFFSET = &uStack_28;
  while( true ) {
    uVar2 = *(uint *)(param_2 + 0xc);
    if ((uVar2 == 0xfffffffe) || ((param_3 != 0xfffffffe && (uVar2 <= param_3)))) break;
    puVar1 = (undefined4 *)((*(uint *)(param_2 + 8) ^ *param_1) + 0x10 + uVar2 * 0xc);
    *(undefined4 *)(param_2 + 0xc) = *puVar1;
    if (puVar1[1] == 0) {
      __NLG_Notify(0x101);
      FUN_10018e50();
    }
  }
  *unaff_FS_OFFSET = uStack_28;
  return;
}


// 1001739e FUN_1001739e

void FUN_1001739e(int param_1)

{
  __local_unwind4(*(uint **)(param_1 + 0x28),*(int *)(param_1 + 0x18),*(uint *)(param_1 + 0x1c));
  return;
}


// 100173ba _EH4_CallFilterFunc

/* Library Function - Single Match
    @_EH4_CallFilterFunc@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_CallFilterFunc,8 */

void __fastcall _EH4_CallFilterFunc(undefined *param_1)

{
  (*(code *)param_1)();
  return;
}


// 100173d1 _EH4_TransferToHandler

/* Library Function - Single Match
    @_EH4_TransferToHandler@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_TransferToHandler,8 */

void __fastcall _EH4_TransferToHandler(undefined *UNRECOVERED_JUMPTABLE)

{
  __NLG_Notify(1);
                    /* WARNING: Could not recover jumptable at 0x100173e8. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (*(code *)UNRECOVERED_JUMPTABLE)();
  return;
}


// 100173ea _EH4_GlobalUnwind

/* Library Function - Single Match
    @_EH4_GlobalUnwind@4
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_GlobalUnwind,4 */

void __fastcall _EH4_GlobalUnwind(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x100173ff,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 10017404 _EH4_LocalUnwind

/* Library Function - Single Match
    @_EH4_LocalUnwind@16
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_LocalUnwind,16 */

void __fastcall _EH4_LocalUnwind(int param_1,uint param_2,undefined4 param_3,uint *param_4)

{
  __local_unwind4(param_4,param_1,param_2);
  return;
}


// 1001741b x_ismbbtype_l

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
    if (uVar1 == 0) goto LAB_1001745d;
  }
  uVar1 = 1;
LAB_1001745d:
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 1001746c __ismbblead

/* Library Function - Single Match
    __ismbblead
   
   Library: Visual Studio 2005 Release */

int __cdecl __ismbblead(uint _C)

{
  int iVar1;
  
  iVar1 = x_ismbbtype_l((localeinfo_struct *)0x0,_C,0,4);
  return iVar1;
}


// 1001747f ___ansicp

/* Library Function - Single Match
    ___ansicp
   
   Library: Visual Studio 2005 Release */

void __cdecl ___ansicp(LCID param_1)

{
  int iVar1;
  CHAR local_10 [6];
  undefined1 local_a;
  uint local_8;
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  local_a = 0;
  iVar1 = GetLocaleInfoA(param_1,0x1004,local_10,6);
  if (iVar1 != 0) {
    _atol(local_10);
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 100174c6 ___convertcp

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  cbMultiByte = *param_4;
  bVar2 = false;
  if (param_1 == param_2) goto LAB_10017666;
  BVar3 = GetCPInfo(param_1,&local_1c);
  if ((((BVar3 == 0) || (local_1c.MaxCharSize != 1)) ||
      (BVar3 = GetCPInfo(param_2,&local_1c), BVar3 == 0)) || (local_1c.MaxCharSize != 1)) {
    uVar7 = MultiByteToWideChar(param_1,1,param_3,cbMultiByte,(LPWSTR)0x0,0);
    bVar8 = uVar7 == 0;
    if (bVar8) goto LAB_10017666;
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
LAB_100175a6:
        local_20 = pWVar5 + 4;
      }
    }
    else {
      pWVar5 = (LPCWSTR)FUN_10001110(uVar1);
      local_20 = pWVar5;
      if (pWVar5 != (LPCWSTR)0x0) {
        pWVar5[0] = L'\xdddd';
        pWVar5[1] = L'\0';
        goto LAB_100175a6;
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
           (lpMultiByteStr = (LPSTR)FUN_10001250(1,uVar7), lpMultiByteStr != (LPSTR)0x0)) {
          uVar7 = WideCharToMultiByte(param_2,0,local_20,uVar7,lpMultiByteStr,uVar7,(LPCSTR)0x0,
                                      (LPBOOL)0x0);
          if (uVar7 == 0) {
            FUN_100012c0(lpMultiByteStr);
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
LAB_10017666:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10017678 __mbtowc_l

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
          if (iVar2 != 0) goto LAB_100176c3;
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
        pDVar3 = FUN_10010b06();
        *pDVar3 = 0x2a;
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        return -1;
      }
      if (_DstCh != (wchar_t *)0x0) {
        *_DstCh = (ushort)(byte)*_SrcCh;
      }
LAB_100176c3:
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


// 1001778b _mbtowc

/* Library Function - Single Match
    _mbtowc
   
   Library: Visual Studio 2005 Release */

int __cdecl _mbtowc(wchar_t *_DstCh,char *_SrcCh,size_t _SrcSizeInBytes)

{
  int iVar1;
  
  iVar1 = __mbtowc_l(_DstCh,_SrcCh,_SrcSizeInBytes,(_locale_t)0x0);
  return iVar1;
}


// 100177a2 __wchartodigit

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
      if (param_1 < 0x66a) goto LAB_100177e0;
      iVar1 = 0x6f0;
      if (param_1 < 0x6f0) {
        return -1;
      }
      if (param_1 < 0x6fa) goto LAB_100177e0;
      iVar1 = 0x966;
      if (param_1 < 0x966) {
        return -1;
      }
      if (param_1 < 0x970) goto LAB_100177e0;
      iVar1 = 0x9e6;
      if (param_1 < 0x9e6) {
        return -1;
      }
      if (param_1 < 0x9f0) goto LAB_100177e0;
      iVar1 = 0xa66;
      if (param_1 < 0xa66) {
        return -1;
      }
      if (param_1 < 0xa70) goto LAB_100177e0;
      iVar1 = 0xae6;
      if (param_1 < 0xae6) {
        return -1;
      }
      if (param_1 < 0xaf0) goto LAB_100177e0;
      iVar1 = 0xb66;
      if (param_1 < 0xb66) {
        return -1;
      }
      if (param_1 < 0xb70) goto LAB_100177e0;
      iVar1 = 0xc66;
      if (param_1 < 0xc66) {
        return -1;
      }
      if (param_1 < 0xc70) goto LAB_100177e0;
      iVar1 = 0xce6;
      if (param_1 < 0xce6) {
        return -1;
      }
      if (param_1 < 0xcf0) goto LAB_100177e0;
      iVar1 = 0xd66;
      if (param_1 < 0xd66) {
        return -1;
      }
      if (param_1 < 0xd70) goto LAB_100177e0;
      iVar1 = 0xe50;
      if (param_1 < 0xe50) {
        return -1;
      }
      if (param_1 < 0xe5a) goto LAB_100177e0;
      iVar1 = 0xed0;
      if (param_1 < 0xed0) {
        return -1;
      }
      if (param_1 < 0xeda) goto LAB_100177e0;
      iVar1 = 0xf20;
      if (param_1 < 0xf20) {
        return -1;
      }
      if (param_1 < 0xf2a) goto LAB_100177e0;
      iVar1 = 0x1040;
      if (param_1 < 0x1040) {
        return -1;
      }
      if (param_1 < 0x104a) goto LAB_100177e0;
      iVar1 = 0x17e0;
      if (param_1 < 0x17e0) {
        return -1;
      }
      if (param_1 < 0x17ea) goto LAB_100177e0;
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
LAB_100177e0:
      return (uint)param_1 - iVar1;
    }
  }
  return -1;
}


// 10017924 __crtGetStringTypeW_stat

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  if (param_4 < -1) goto LAB_10017b36;
  if (DAT_10023b64 == 0) {
    BVar3 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_10023b64 = 2;
      }
      goto LAB_1001798b;
    }
    DAT_10023b64 = 1;
  }
  else {
LAB_1001798b:
    if (DAT_10023b64 != 1) {
      if ((DAT_10023b64 != 2) && (DAT_10023b64 != 0)) goto LAB_10017b36;
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
      if (_Size == 0) goto LAB_10017b36;
      if (((int)_Size < 1) || (0xffffffe0 < _Size)) {
        local_c = (LPSTR)0x0;
      }
      else if (_Size + 8 < 0x401) {
        pCVar5 = &stack0xffffffe0;
        local_c = &stack0xffffffe0;
        if (&stack0x00000000 != (undefined1 *)0x20) {
LAB_10017a3c:
          local_c = pCVar5 + 8;
        }
      }
      else {
        pCVar5 = (LPSTR)FUN_10001110(_Size + 8);
        local_c = pCVar5;
        if (pCVar5 != (LPSTR)0x0) {
          pCVar5[0] = -0x23;
          pCVar5[1] = -0x23;
          pCVar5[2] = '\0';
          pCVar5[3] = '\0';
          goto LAB_10017a3c;
        }
      }
      if (local_c == (LPSTR)0x0) goto LAB_10017b36;
      _memset(local_c,0,_Size);
      iVar6 = WideCharToMultiByte(param_6,0,param_3,param_4,local_c,_Size,(LPCSTR)0x0,(LPBOOL)0x0);
      if (iVar6 != 0) {
        if (((int)(_Size + 1) < 1) || (0x7ffffff0 < _Size + 1)) {
          lpCharType = (LPWORD)0x0;
        }
        else {
          uVar2 = _Size * 2 + 10;
          if (uVar2 < 0x401) {
            if (&stack0x00000000 == (undefined1 *)0x20) goto LAB_10017b2a;
            lpCharType = (LPWORD)&stack0xffffffe8;
          }
          else {
            lpCharType = (LPWORD)FUN_10001110(uVar2);
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
LAB_10017b2a:
      __freea(local_c);
      goto LAB_10017b36;
    }
  }
  GetStringTypeW(param_2,param_3,param_4,param_5);
LAB_10017b36:
  iVar6 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar6;
}


// 10017b48 ___crtGetStringTypeW

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


// 10017b86 FUN_10017b86

/* WARNING: Removing unreachable block (ram,0x10018077) */
/* WARNING: Removing unreachable block (ram,0x10018081) */
/* WARNING: Removing unreachable block (ram,0x10018086) */

void __cdecl
FUN_10017b86(int param_1,uint param_2,ushort param_3,int param_4,byte param_5,short *param_6)

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
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
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
    goto LAB_10018404;
  }
  if ((short)uVar14 == 0x7fff) {
    *param_6 = 1;
    if (((param_2 == 0x80000000) && (param_1 == 0)) || ((param_2 & 0x40000000) != 0)) {
      if ((uVar8 == 0) || (param_2 != 0xc0000000)) {
        if ((param_2 != 0x80000000) || (param_1 != 0)) goto LAB_10017cb5;
        pcVar25 = "1#INF";
      }
      else {
        if (param_1 != 0) {
LAB_10017cb5:
          pcVar25 = "1#QNAN";
          goto LAB_10017cba;
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
LAB_10017cba:
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
    goto LAB_10018404;
  }
  local_50 = (short)(((uVar14 >> 8) + (param_2 >> 0x18) * 2) * 0x4d + -0x134312f4 + uVar14 * 0x4d10
                    >> 0x10);
  uVar16 = (uint)local_50;
  local_1a = (undefined1)uVar14;
  bStack_19 = (byte)(uVar14 >> 8);
  local_24._2_2_ = (ushort)param_1;
  local_24._0_2_ = 0;
  local_6c = &PTR_DAT_10020680;
  uStack_20._0_2_ = uStack_12;
  uStack_20._2_2_ = (ushort)local_10;
  uStack_1c = local_10._2_2_;
  if (uVar16 != 0) {
    iVar20 = param_1;
    uVar14 = -uVar16;
    if (0 < (int)uVar16) {
      local_6c = (undefined **)&DAT_100207e0;
      uVar14 = uVar16;
    }
    while (uVar14 != 0) {
      uStack_20._0_2_ = (ushort)((uint)iVar20 >> 0x10);
      local_24._2_2_ = (ushort)iVar20;
      local_6c = local_6c + 0x15;
      iVar3 = CONCAT22(local_c,local_10._2_2_);
      if ((uVar14 & 7) == 0) goto LAB_10017fdb;
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
LAB_10017eff:
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
            if ((short)uVar24 < 1) goto LAB_10017eff;
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
          if (0x7ffe < uVar24) goto LAB_10017fbf;
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
LAB_10017fbf:
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
LAB_10017fdb:
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
          goto LAB_10018243;
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
LAB_10018151:
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
          if ((short)uVar24 < 1) goto LAB_10018151;
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
        goto LAB_10018243;
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
LAB_10018243:
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
        goto LAB_10018404;
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
LAB_10018404:
  local_24 = uVar16;
  local_10 = param_2;
  uStack_3e = iVar1;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001844c __hw_cw

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


// 100184da ___hw_cw_sse2

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


// 1001857a __control87

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
  if (DAT_10024d7c != 0) {
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


// 100188fe __get_osfhandle

/* Library Function - Single Match
    __get_osfhandle
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __get_osfhandle(int _FileHandle)

{
  DWORD *pDVar1;
  intptr_t *piVar2;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_10010b19();
    *pDVar1 = 0;
    pDVar1 = FUN_10010b06();
    *pDVar1 = 9;
    return -1;
  }
  if (((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10024c48)) &&
     (piVar2 = (intptr_t *)((_FileHandle & 0x1fU) * 0x38 + (&DAT_10024c60)[_FileHandle >> 5]),
     (*(byte *)(piVar2 + 1) & 1) != 0)) {
    return *piVar2;
  }
  pDVar1 = FUN_10010b19();
  *pDVar1 = 0;
  pDVar1 = FUN_10010b06();
  *pDVar1 = 9;
  FUN_10010aa7((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1001896f ___lock_fhandle

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
  
  iVar2 = (_Filehandle & 0x1fU) * 0x38 + (&DAT_10024c60)[_Filehandle >> 5];
  local_20 = 1;
  if (*(int *)(iVar2 + 8) == 0) {
    __lock(10);
    if (*(int *)(iVar2 + 8) == 0) {
      iVar1 = ___crtInitCritSecAndSpinCount(iVar2 + 0xc,4000);
      local_20 = (uint)(iVar1 != 0);
      *(int *)(iVar2 + 8) = *(int *)(iVar2 + 8) + 1;
    }
    FUN_10018a06();
  }
  if (local_20 != 0) {
    EnterCriticalSection
              ((LPCRITICAL_SECTION)
               ((&DAT_10024c60)[_Filehandle >> 5] + 0xc + (_Filehandle & 0x1fU) * 0x38));
  }
  return local_20;
}


// 10018a06 FUN_10018a06

void FUN_10018a06(void)

{
  FUN_10014265(10);
  return;
}


// 10018a0f FUN_10018a0f

void __cdecl FUN_10018a0f(uint param_1)

{
  LeaveCriticalSection
            ((LPCRITICAL_SECTION)
             ((&DAT_10024c60)[(int)param_1 >> 5] + 0xc + (param_1 & 0x1f) * 0x38));
  return;
}


// 10018a31 __putwch_nolock

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
  
  local_8 = DAT_100202a0 ^ (uint)&stack0xfffffffc;
  if (DAT_10020cd0 != 0) {
    if (DAT_10020e70 == (HANDLE)0xfffffffe) {
      ___initconout();
    }
    if (DAT_10020e70 == (HANDLE)0xffffffff) goto LAB_10018ada;
    BVar2 = WriteConsoleW(DAT_10020e70,&_WCh,1,&local_14,(LPVOID)0x0);
    if (BVar2 != 0) {
      DAT_10020cd0 = 1;
      goto LAB_10018ada;
    }
    if ((DAT_10020cd0 != 2) || (DVar3 = GetLastError(), DVar3 != 0x78)) goto LAB_10018ada;
    DAT_10020cd0 = 0;
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
  if (DAT_10020e70 != (HANDLE)0xffffffff) {
    WriteConsoleA(DAT_10020e70,local_10,DVar3,&local_14,(LPVOID)0x0);
  }
LAB_10018ada:
  wVar1 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return wVar1;
}


// 10018d1c __global_unwind2

/* Library Function - Single Match
    __global_unwind2
   
   Library: Visual Studio */

void __cdecl __global_unwind2(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x10018d34,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 10018d81 __local_unwind2

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
  puStack_1c = &LAB_10018d3c;
  local_20 = *unaff_FS_OFFSET;
  *unaff_FS_OFFSET = &local_20;
  while( true ) {
    uVar1 = *(uint *)(param_1 + 0xc);
    if ((uVar1 == 0xffffffff) || ((param_2 != 0xffffffff && (uVar1 <= param_2)))) break;
    local_18 = *(undefined4 *)(*(int *)(param_1 + 8) + uVar1 * 0xc);
    *(undefined4 *)(param_1 + 0xc) = local_18;
    if (*(int *)(*(int *)(param_1 + 8) + 4 + uVar1 * 0xc) == 0) {
      __NLG_Notify(0x101);
      FUN_10018e50();
    }
  }
  *unaff_FS_OFFSET = local_20;
  return;
}


// 10018e31 __NLG_Notify

/* Library Function - Single Match
    __NLG_Notify
   
   Libraries: Visual Studio 2017 Debug, Visual Studio 2017 Release, Visual Studio 2019 Debug, Visual
   Studio 2019 Release */

void __NLG_Notify(ulong param_1)

{
  undefined4 in_EAX;
  undefined4 unaff_EBP;
  
  DAT_10020cdc = param_1;
  DAT_10020cd8 = in_EAX;
  DAT_10020ce0 = unaff_EBP;
  return;
}


// 10018e50 FUN_10018e50

void FUN_10018e50(void)

{
  code *in_EAX;
  
  (*in_EAX)();
  return;
}


// 10018e60 ___ascii_strnicmp

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
      if (bVar2 != (byte)uVar3) goto LAB_10018eb1;
      _MaxCount = _MaxCount - 1;
    } while (_MaxCount != 0);
    _MaxCount = 0;
    bVar2 = (byte)(uVar3 >> 8);
    bVar5 = bVar2 < (byte)uVar3;
    if (bVar2 != (byte)uVar3) {
LAB_10018eb1:
      _MaxCount = 0xffffffff;
      if (!bVar5) {
        _MaxCount = 1;
      }
    }
  }
  return _MaxCount;
}


// 10018ec1 ___set_fpsr_sse2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___set_fpsr_sse2
   
   Library: Visual Studio 2005 Release */

void __cdecl ___set_fpsr_sse2(uint param_1)

{
  if (DAT_10024d7c != 0) {
    if (((param_1 & 0x40) == 0) || (DAT_10020e68 == 0)) {
      MXCSR = param_1 & 0xffffffbf;
    }
    else {
      MXCSR = param_1;
    }
  }
  return;
}


// 10018f33 ___initconout

/* Library Function - Single Match
    ___initconout
   
   Library: Visual Studio 2005 Release */

void __cdecl ___initconout(void)

{
  DAT_10020e70 = CreateFileA("CONOUT$",0x40000000,3,(LPSECURITY_ATTRIBUTES)0x0,3,0,(HANDLE)0x0);
  return;
}


// 100192f0 FUN_100192f0

/* WARNING: Removing unreachable block (ram,0x100192f9) */

void FUN_100192f0(void)

{
  DAT_10021548 = 0;
  DAT_1002154c = 0;
  DAT_10021554 = 0;
  DAT_10021550 = 0;
  _atexit((_func_4879 *)&LAB_10019480);
  return;
}


