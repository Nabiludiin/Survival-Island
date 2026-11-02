// 10001000 FUN_10001000

undefined4 * __thiscall FUN_10001000(void *this,undefined4 param_1,int param_2,int *param_3)

{
  undefined4 uVar1;
  undefined *puVar2;
  uint uVar3;
  char *pcVar4;
  float10 fVar5;
  undefined1 uVar6;
  float fVar7;
  undefined1 uVar8;
  float fVar9;
  int iVar10;
  
  iVar10 = *(int *)(param_3[7] + 0x44);
  fVar5 = FUN_1000bdc0((int)param_3);
  fVar9 = (float)fVar5;
  uVar1 = FUN_1000bde0((int)param_3);
  uVar8 = (undefined1)uVar1;
  fVar5 = FUN_1000bdb0((int)param_3);
  fVar7 = (float)fVar5;
  uVar1 = FUN_1000bdd0((int)param_3);
  uVar6 = (undefined1)uVar1;
  puVar2 = (undefined *)(**(code **)(*param_3 + 0x1c))();
  uVar3 = (**(code **)(*param_3 + 0x14))();
  uVar3 = uVar3 & 0xffff7fff | 0x40000;
  pcVar4 = (char *)FUN_1000bdf0((int)param_3);
  FUN_1000c260(this,param_2,pcVar4,uVar3,puVar2,uVar6,(int)fVar7,uVar8,(int)fVar9,iVar10);
  *(int **)((int)this + 0x48) = param_3;
  *(undefined ***)this = &PTR_FUN_1001d2ec;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_1001d2cc;
  *(undefined4 *)((int)this + 0x4c) = param_1;
  return (undefined4 *)this;
}


// 10001130 FUN_10001130

void FUN_10001130(void)

{
  return;
}


// 10001260 FUN_10001260

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

void FUN_10001260(char *param_1)

{
  code *pcVar1;
  int iVar2;
  int iVar3;
  int *piVar4;
  undefined1 *puVar5;
  int aiStackY_20 [3];
  
  aiStackY_20[2] = 0x1000126f;
  iVar3 = FUN_1000b1a0(param_1);
  iVar2 = -(iVar3 + 0x11U & 0xfffffff0);
  *(int *)(&stack0xffffffec + iVar2) = iVar3 + 1;
  *(char **)((int)aiStackY_20 + iVar2 + 8) = param_1;
  *(undefined1 **)((int)aiStackY_20 + iVar2 + 4) = &stack0xfffffff1 + iVar2;
  (&stack0xfffffff0)[iVar2] = 0x2b;
  *(undefined4 *)((int)aiStackY_20 + iVar2) = 0x10001292;
  _memcpy(*(void **)((int)aiStackY_20 + iVar2 + 4),*(void **)((int)aiStackY_20 + iVar2 + 8),
          *(size_t *)(&stack0xffffffec + iVar2));
  puVar5 = &stack0xffffffec + iVar2;
  *(undefined4 *)(&stack0xffffffec + iVar2) = 0x1000129b;
  piVar4 = (int *)CommandLine_Tier0();
  iVar3 = *piVar4;
  *(undefined4 *)(puVar5 + -4) = 0;
  pcVar1 = *(code **)(iVar3 + 0x20);
  *(undefined1 **)(puVar5 + -8) = &stack0xfffffff0 + iVar2;
  *(undefined4 *)(puVar5 + -0xc) = 0x100012a7;
  (*pcVar1)();
  return;
}


// 10001390 FUN_10001390

undefined4 * __thiscall FUN_10001390(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_1001d36c;
  *(undefined ***)((int)this + 0x24) = &PTR_LAB_1001d360;
  *(undefined ***)((int)this + 0x28) = &PTR_LAB_1001d358;
  FUN_1000b690(this);
  FUN_1000b7b0((undefined4 *)this);
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (undefined4 *)this;
}


// 100013d0 FUN_100013d0

void FUN_100013d0(undefined4 param_1,int param_2,int param_3,int param_4,undefined *param_5)

{
  int iVar1;
  
  for (iVar1 = param_4 + -1; -1 < iVar1; iVar1 = iVar1 + -1) {
    (*(code *)param_5)(param_2);
    param_2 = param_2 + param_3;
  }
  return;
}


// 10001410 FUN_10001410

void __thiscall FUN_10001410(void *this,int param_1)

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


// 100014c0 FUN_100014c0

undefined4 * __thiscall FUN_100014c0(void *this,byte param_1)

{
  FUN_1000b810((undefined4 *)this);
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (undefined4 *)this;
}


// 10001530 FUN_10001530

undefined4 * __cdecl FUN_10001530(undefined4 *param_1,char *param_2)

{
  char *pcVar1;
  undefined4 *puVar2;
  int iVar3;
  
  puVar2 = param_1;
  pcVar1 = (char *)((int)param_1 + 5);
  *param_1 = &PTR_LAB_1001d3ec;
  *(undefined1 *)(param_1 + 1) = 1;
  param_1 = (undefined4 *)((uint)param_1 & 0xffffff00);
  FUN_1000b4d0(pcVar1,0xff,param_2,&stack0x0000000c,(int)&param_1);
  *(undefined1 *)(puVar2 + 0x41) = 0;
  if ((((char)param_1 != '\0') && (*(char *)(puVar2 + 1) == '\0')) && (DAT_100293ec < 5)) {
    DAT_100293ec = DAT_100293ec + 1;
  }
  iVar3 = FUN_1000b1a0(pcVar1);
  puVar2[0x42] = iVar3;
  return puVar2;
}


// 100015a0 FUN_100015a0

void __thiscall FUN_100015a0(void *this,int param_1,int param_2)

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
  FUN_10001410(this,param_2);
  return;
}


// 100015f0 FUN_100015f0

uint FUN_100015f0(undefined4 *param_1,uint *param_2)

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


// 10001670 FUN_10001670

void __thiscall FUN_10001670(void *this,void *param_1,void *param_2,int param_3)

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
        FUN_1000b390(param_1,&param_3,4);
        param_1 = (void *)((int)param_1 + 4);
        puVar2 = puVar2 + 4;
        iVar1 = iVar1 + -1;
      } while (iVar1 != 0);
      return;
    }
  }
  return;
}


// 10001710 FUN_10001710

undefined2 * __thiscall FUN_10001710(void *this,ushort param_1)

{
  if ((DAT_100293f8 & 1) == 0) {
    DAT_100293f8 = DAT_100293f8 | 1;
    DAT_100293f0 = 0xffff;
    DAT_100293f2 = 0xffff;
    DAT_100293f4 = 0xffff;
    DAT_100293f6 = 1;
  }
  if (param_1 != 0xffff) {
    return (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x48);
  }
  return &DAT_100293f0;
}


// 10001770 FUN_10001770

void __thiscall FUN_10001770(void *this,short *param_1,short *param_2)

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


// 100017b0 FUN_100017b0

void __thiscall FUN_100017b0(void *this,int param_1)

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
        uVar2 = 1;
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
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(uVar2 * 0x48);
    *(undefined4 *)this = uVar3;
  }
  return;
}


// 10001860 FUN_10001860

void __thiscall FUN_10001860(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_100018bb;
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
LAB_100018bb:
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 4) = iVar2;
  if (*this == 0) {
    uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar2 << 5);
    *(undefined4 *)this = uVar3;
    return;
  }
  uVar3 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(*this);
  *(undefined4 *)this = uVar3;
  return;
}


// 100018f0 FUN_100018f0

void __thiscall FUN_100018f0(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000194b;
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
LAB_1000194b:
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


// 10001990 FUN_10001990

void __fastcall FUN_10001990(int *param_1)

{
  param_1[3] = 0;
  param_1[4] = 0;
  param_1[8] = 0;
  param_1[7] = 0;
  *(undefined1 *)(param_1 + 5) = 0;
  if (-1 < param_1[2]) {
    if (*param_1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
      *param_1 = 0;
    }
    param_1[1] = 0;
  }
  return;
}


// 100019f0 FUN_100019f0

void __thiscall FUN_100019f0(void *this,int *param_1)

{
  int iVar1;
  int *piVar2;
  int iVar3;
  
  iVar3 = 0;
  if (0 < *(int *)((int)this + 0xc)) {
                    /* WARNING: Load size is inaccurate */
    piVar2 = *this;
    do {
      iVar1 = *piVar2;
      if (iVar1 != 0) {
        *param_1 = iVar3;
        param_1[1] = iVar1;
        return;
      }
      iVar3 = iVar3 + 1;
      piVar2 = piVar2 + 1;
    } while (iVar3 < *(int *)((int)this + 0xc));
  }
  *param_1 = -1;
  param_1[1] = 0;
  return;
}


// 10001a40 FUN_10001a40

void __thiscall FUN_10001a40(void *this,int *param_1,int *param_2)

{
  int iVar1;
  int *piVar2;
  int iVar3;
  
  iVar3 = *(int *)(param_2[1] + 0xc);
  if (iVar3 != 0) {
    *param_1 = *param_2;
    param_1[1] = iVar3;
    return;
  }
  iVar3 = *param_2 + 1;
  if (iVar3 < *(int *)((int)this + 0xc)) {
                    /* WARNING: Load size is inaccurate */
    piVar2 = (int *)(*this + iVar3 * 4);
    do {
      iVar1 = *piVar2;
      if (iVar1 != 0) {
        *param_1 = iVar3;
        param_1[1] = iVar1;
        return;
      }
      iVar3 = iVar3 + 1;
      piVar2 = piVar2 + 1;
    } while (iVar3 < *(int *)((int)this + 0xc));
  }
  *param_1 = -1;
  param_1[1] = 0;
  return;
}


// 10001ab0 FUN_10001ab0

void __fastcall FUN_10001ab0(int param_1)

{
  undefined4 *puVar1;
  int local_8 [2];
  
  puVar1 = (undefined4 *)FUN_100019f0(*(void **)(param_1 + 8),local_8);
  *(undefined4 *)(param_1 + 0xc) = *puVar1;
  *(undefined4 *)(param_1 + 0x10) = puVar1[1];
  return;
}


// 10001ae0 FUN_10001ae0

void __fastcall FUN_10001ae0(int param_1)

{
  int *piVar1;
  int local_8 [2];
  
  piVar1 = (int *)FUN_10001a40(*(void **)(param_1 + 8),local_8,(int *)(param_1 + 0xc));
  *(int *)(param_1 + 0xc) = *piVar1;
  *(int *)(param_1 + 0x10) = piVar1[1];
  return;
}


// 10001b30 FUN_10001b30

void __thiscall FUN_10001b30(void *this,int param_1,undefined4 param_2,undefined4 param_3)

{
  int iVar1;
  int iVar2;
  int iVar3;
  
  iVar1 = *(int *)((int)this + 0x10);
  iVar3 = 0;
  if (0 < iVar1) {
    do {
      if (param_1 == 0) {
        iVar2 = 0;
      }
      else {
        iVar2 = param_1 + 0x18;
      }
      (**(code **)(*(int *)((int)this + 4) + iVar3 * 4))(iVar2,param_2,param_3);
      iVar3 = iVar3 + 1;
    } while (iVar3 < iVar1);
  }
  return;
}


// 10001ba0 FUN_10001ba0

uint * __thiscall FUN_10001ba0(void *this,byte *param_1,uint param_2)

{
  uint *puVar1;
  byte *pbVar2;
  uint uVar3;
  
                    /* WARNING: Load size is inaccurate */
  puVar1 = *(uint **)(*this + (param_2 & 0xff) * 4);
  do {
    if (puVar1 == (uint *)0x0) {
      return (uint *)0xffffffff;
    }
    if (*puVar1 == param_2) {
      pbVar2 = (byte *)(**(code **)(*(int *)puVar1[1] + 0x18))();
      uVar3 = FUN_1000b230(param_1,pbVar2);
      if (uVar3 == 0) {
        return puVar1;
      }
    }
    puVar1 = (uint *)puVar1[3];
  } while( true );
}


// 10001c00 FUN_10001c00

uint * __thiscall FUN_10001c00(void *this,int *param_1)

{
  uint *puVar1;
  byte *pbVar2;
  uint uVar3;
  
  pbVar2 = (byte *)(**(code **)(*param_1 + 0x18))();
  uVar3 = FUN_1000c620(pbVar2);
                    /* WARNING: Load size is inaccurate */
  puVar1 = *(uint **)(*this + (uVar3 & 0xff) * 4);
  while( true ) {
    if (puVar1 == (uint *)0x0) {
      return (uint *)0xffffffff;
    }
    if ((*puVar1 == uVar3) && ((int *)puVar1[1] == param_1)) break;
    puVar1 = (uint *)puVar1[3];
  }
  return puVar1;
}


// 10001c40 FUN_10001c40

void __thiscall FUN_10001c40(void *this,undefined1 param_1)

{
  undefined4 uVar1;
  
  uVar1 = FUN_1000cd00(this,1);
  if ((char)uVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
    FUN_1000cd50((int *)this);
  }
  return;
}


// 10001c70 FUN_10001c70

void __thiscall FUN_10001c70(void *this,int *param_1)

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


// 10001cb0 FUN_10001cb0

void __thiscall FUN_10001cb0(void *this,undefined4 *param_1)

{
  char cVar1;
  int iVar2;
  
  cVar1 = FUN_1000cc30(this,4);
  if (cVar1 == '\0') {
    *param_1 = 0;
    return;
  }
  iVar2 = *(int *)((int)this + 0xc) - *(int *)((int)this + 0x20);
  if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
                    /* WARNING: Load size is inaccurate */
    FUN_10001670((void *)((int)this + 0x2c),param_1,(void *)(iVar2 + *this),1);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 4;
    return;
  }
                    /* WARNING: Load size is inaccurate */
  *param_1 = *(undefined4 *)(iVar2 + *this);
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 4;
  return;
}


// 10001d10 FUN_10001d10

void __thiscall FUN_10001d10(void *this,undefined4 param_1)

{
  undefined4 uVar1;
  
  uVar1 = FUN_1000cd00(this,4);
  if ((char)uVar1 != '\0') {
    if ((*(byte *)((int)this + 0x2c) & 1) != 0) {
                    /* WARNING: Load size is inaccurate */
      FUN_10001670((void *)((int)this + 0x2c),
                   (void *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this),
                   &param_1,1);
      *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 4;
      FUN_1000cd50((int *)this);
      return;
    }
                    /* WARNING: Load size is inaccurate */
    *(undefined4 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 4;
    FUN_1000cd50((int *)this);
  }
  return;
}


// 10001d70 FUN_10001d70

int __thiscall FUN_10001d70(void *this,char param_1)

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
      piVar2 = (int *)FUN_10009c10(&local_10,piVar1);
    }
    iVar4 = *piVar2;
    iVar3 = piVar2[1];
    if (((iVar4 == 0) || (iVar3 < 0)) || (*(int *)(iVar4 + 4) <= iVar3)) {
      FUN_10001410(this,1);
      *(undefined4 *)((int)this + 0x28) = 0;
      if (((*piVar1 == 0) || (*(int *)((int)this + 0x24) < 0)) ||
         (*(int *)(*piVar1 + 4) <= *(int *)((int)this + 0x24))) {
        piVar2 = (int *)FUN_10001c70(this,local_8);
      }
      else {
        piVar2 = (int *)FUN_10009c10(&local_18,piVar1);
      }
      iVar4 = *piVar2;
      iVar3 = piVar2[1];
      if (((iVar4 == 0) || (iVar3 < 0)) || (*(int *)(iVar4 + 4) <= iVar3)) {
        if (DAT_10029414 < 10) {
          Warning("CUtlLinkedList overflow! (exhausted memory allocator)\n");
          DAT_10029414 = DAT_10029414 + 1;
        }
        return 0;
      }
    }
    if (iVar3 * 0x10 + 8 + iVar4 == 0) {
      if (DAT_10029410 < 10) {
        Warning("CUtlLinkedList overflow! (exhausted index range)\n");
        DAT_10029410 = DAT_10029410 + 1;
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


// 10001ef0 FUN_10001ef0

ushort * __thiscall FUN_10001ef0(void *this,undefined4 param_1,ushort *param_2,undefined1 *param_3)

{
  ushort uVar1;
  int iVar2;
  char cVar3;
  ushort *puVar4;
  
  uVar1 = *(ushort *)((int)this + 0x10);
  *param_2 = 0xffff;
  *param_3 = 0;
  puVar4 = param_2;
  while (uVar1 != 0xffff) {
    *param_2 = uVar1;
                    /* WARNING: Load size is inaccurate */
    iVar2 = (uint)uVar1 * 0x48;
    cVar3 = (**this)(param_1,iVar2 + 8 + *(int *)((int)this + 4));
    if (cVar3 == '\0') {
      *param_3 = 0;
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100293f0;
      }
      else {
        puVar4 = (ushort *)(*(int *)((int)this + 4) + iVar2);
      }
      uVar1 = puVar4[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100293f0;
        uVar1 = DAT_100293f0;
      }
      else {
        puVar4 = (ushort *)(*(int *)((int)this + 4) + iVar2);
        uVar1 = *(ushort *)(*(int *)((int)this + 4) + iVar2);
      }
    }
  }
  return puVar4;
}


// 10002000 FUN_10002000

undefined4 __thiscall FUN_10002000(void *this,int param_1)

{
  if ((DAT_1002940c & 1) == 0) {
    DAT_1002940c = DAT_1002940c | 1;
    DAT_100293fc = 0xffffffff;
    DAT_10029400 = 0xffffffff;
    DAT_10029404 = 0xffffffff;
    DAT_10029408 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14 + 8);
  }
  return DAT_10029404;
}


// 10002050 FUN_10002050

undefined4 __thiscall FUN_10002050(void *this,int param_1)

{
  if ((DAT_1002940c & 1) == 0) {
    DAT_1002940c = DAT_1002940c | 1;
    DAT_100293fc = 0xffffffff;
    DAT_10029400 = 0xffffffff;
    DAT_10029404 = 0xffffffff;
    DAT_10029408 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
  }
  return DAT_100293fc;
}


// 100020a0 FUN_100020a0

undefined4 __thiscall FUN_100020a0(void *this,int param_1)

{
  if ((DAT_1002940c & 1) == 0) {
    DAT_1002940c = DAT_1002940c | 1;
    DAT_100293fc = 0xffffffff;
    DAT_10029400 = 0xffffffff;
    DAT_10029404 = 0xffffffff;
    DAT_10029408 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14 + 4);
  }
  return DAT_10029400;
}


// 100020f0 FUN_100020f0

int * __thiscall FUN_100020f0(void *this,undefined4 param_1,int *param_2,undefined1 *param_3)

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
      if ((DAT_1002940c & 1) == 0) {
        DAT_1002940c = DAT_1002940c | 1;
        DAT_100293fc = -1;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
      }
      if (iVar1 == -1) {
        piVar4 = &DAT_100293fc;
      }
      else {
        piVar4 = (int *)(*(int *)((int)this + 4) + iVar2);
      }
      iVar1 = piVar4[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_1002940c & 1) == 0) {
        DAT_1002940c = DAT_1002940c | 1;
        DAT_100293fc = -1;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
      }
      if (iVar1 == -1) {
        piVar4 = &DAT_100293fc;
        iVar1 = DAT_100293fc;
      }
      else {
        piVar4 = (int *)(*(int *)((int)this + 4) + iVar2);
        iVar1 = *(int *)(*(int *)((int)this + 4) + iVar2);
      }
    }
  }
  return piVar4;
}


// 100021e0 FUN_100021e0

int __fastcall FUN_100021e0(int param_1)

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
      FUN_100018f0((void *)(param_1 + 4),1);
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


// 100022c0 FUN_100022c0

void __thiscall FUN_100022c0(void *this,ushort param_1)

{
  ushort uVar1;
  undefined2 *puVar2;
  short *psVar3;
  ushort *puVar4;
  
  puVar2 = FUN_10001710(this,param_1);
  uVar1 = puVar2[1];
  puVar2 = FUN_10001710(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + 2 + (uint)param_1 * 0x48) = *puVar2;
  psVar3 = FUN_10001710(this,uVar1);
  if (*psVar3 != -1) {
    puVar4 = FUN_10001710(this,uVar1);
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)*puVar4 * 0x48) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar2 = FUN_10001710(this,param_1);
    *(undefined2 *)(*(int *)((int)this + 4) + 4 + (uint)uVar1 * 0x48) = puVar2[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar2 = FUN_10001710(this,param_1);
    puVar4 = FUN_10001710(this,puVar2[2]);
    if (*puVar4 == param_1) {
      puVar2 = FUN_10001710(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar2[2] * 0x48) = uVar1;
    }
    else {
      puVar2 = FUN_10001710(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)(ushort)puVar2[2] * 0x48) = uVar1;
    }
  }
  *(ushort *)(*(int *)((int)this + 4) + (uint)uVar1 * 0x48) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x48) = uVar1;
  }
  return;
}


// 100023c0 FUN_100023c0

void __thiscall FUN_100023c0(void *this,ushort param_1)

{
  ushort uVar1;
  ushort *puVar2;
  undefined2 *puVar3;
  
  puVar2 = FUN_10001710(this,param_1);
  uVar1 = *puVar2;
  puVar3 = FUN_10001710(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x48) = puVar3[1];
  puVar3 = FUN_10001710(this,uVar1);
  if (puVar3[1] != -1) {
    puVar3 = FUN_10001710(this,uVar1);
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)(ushort)puVar3[1] * 0x48) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar3 = FUN_10001710(this,param_1);
    *(undefined2 *)(*(int *)((int)this + 4) + 4 + (uint)uVar1 * 0x48) = puVar3[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar3 = FUN_10001710(this,param_1);
    puVar3 = FUN_10001710(this,puVar3[2]);
    if (puVar3[1] == param_1) {
      puVar3 = FUN_10001710(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)(ushort)puVar3[2] * 0x48) = uVar1;
    }
    else {
      puVar3 = FUN_10001710(this,param_1);
      *(ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar3[2] * 0x48) = uVar1;
    }
  }
  *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)uVar1 * 0x48) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x48) = uVar1;
  }
  return;
}


// 100024c0 FUN_100024c0

undefined4 __thiscall FUN_100024c0(void *this,int param_1)

{
  int *piVar1;
  int iVar2;
  
  iVar2 = FUN_10002000(this,param_1);
  if ((DAT_1002940c & 1) == 0) {
    DAT_1002940c = DAT_1002940c | 1;
    DAT_100293fc = -1;
    DAT_10029400 = 0xffffffff;
    DAT_10029404 = 0xffffffff;
    DAT_10029408 = 1;
  }
  if (iVar2 != -1) {
    piVar1 = (int *)(*(int *)((int)this + 4) + iVar2 * 0x14);
    return CONCAT31((int3)((uint)piVar1 >> 8),*piVar1 == param_1);
  }
  return CONCAT31(0x100293,DAT_100293fc == param_1);
}


// 10002530 FUN_10002530

void __thiscall FUN_10002530(void *this,int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  int *piVar3;
  int iVar4;
  undefined4 uVar5;
  
  iVar1 = FUN_100020a0(this,param_1);
  if ((DAT_1002940c & 1) == 0) {
    DAT_1002940c = DAT_1002940c | 1;
    DAT_100293fc = 0xffffffff;
    DAT_10029400 = 0xffffffff;
    DAT_10029404 = 0xffffffff;
    DAT_10029408 = 1;
  }
  if (iVar1 == -1) {
    puVar2 = &DAT_100293fc;
  }
  else {
    puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
  }
  *(undefined4 *)(*(int *)((int)this + 4) + 4 + param_1 * 0x14) = *puVar2;
  if ((DAT_1002940c & 1) == 0) {
    DAT_1002940c = DAT_1002940c | 1;
    DAT_100293fc = 0xffffffff;
    DAT_10029400 = 0xffffffff;
    DAT_10029404 = 0xffffffff;
    DAT_10029408 = 1;
  }
  if (iVar1 == -1) {
    piVar3 = &DAT_100293fc;
  }
  else {
    piVar3 = (int *)(*(int *)((int)this + 4) + iVar1 * 0x14);
  }
  if (*piVar3 != -1) {
    iVar4 = FUN_10002050(this,iVar1);
    *(int *)(*(int *)((int)this + 4) + 8 + iVar4 * 0x14) = param_1;
  }
  if (iVar1 != -1) {
    uVar5 = FUN_10002000(this,param_1);
    *(undefined4 *)(*(int *)((int)this + 4) + 8 + iVar1 * 0x14) = uVar5;
  }
  if (param_1 == *(int *)((int)this + 0x10)) {
    *(int *)((int)this + 0x10) = iVar1;
  }
  else {
    uVar5 = FUN_100024c0(this,param_1);
    if ((char)uVar5 == '\0') {
      iVar4 = FUN_10002000(this,param_1);
      *(int *)(*(int *)((int)this + 4) + 4 + iVar4 * 0x14) = iVar1;
    }
    else {
      iVar4 = FUN_10002000(this,param_1);
      *(int *)(*(int *)((int)this + 4) + iVar4 * 0x14) = iVar1;
    }
  }
  *(int *)(*(int *)((int)this + 4) + iVar1 * 0x14) = param_1;
  if (param_1 != -1) {
    *(int *)(*(int *)((int)this + 4) + 8 + param_1 * 0x14) = iVar1;
  }
  return;
}


// 10002660 FUN_10002660

void __thiscall FUN_10002660(void *this,int param_1)

{
  int iVar1;
  int *piVar2;
  undefined4 uVar3;
  int iVar4;
  
  if ((DAT_1002940c & 1) == 0) {
    DAT_1002940c = DAT_1002940c | 1;
    DAT_100293fc = 0xffffffff;
    DAT_10029400 = 0xffffffff;
    DAT_10029404 = 0xffffffff;
    DAT_10029408 = 1;
  }
  if (param_1 == -1) {
    piVar2 = &DAT_100293fc;
  }
  else {
    piVar2 = (int *)(*(int *)((int)this + 4) + param_1 * 0x14);
  }
  iVar1 = *piVar2;
  uVar3 = FUN_100020a0(this,iVar1);
  *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14) = uVar3;
  iVar4 = FUN_100020a0(this,iVar1);
  if (iVar4 != -1) {
    iVar4 = FUN_100020a0(this,iVar1);
    *(int *)(*(int *)((int)this + 4) + 8 + iVar4 * 0x14) = param_1;
  }
  if (iVar1 != -1) {
    uVar3 = FUN_10002000(this,param_1);
    *(undefined4 *)(*(int *)((int)this + 4) + 8 + iVar1 * 0x14) = uVar3;
  }
  if (param_1 == *(int *)((int)this + 0x10)) {
    *(int *)((int)this + 0x10) = iVar1;
  }
  else {
    iVar4 = FUN_10002000(this,param_1);
    iVar4 = FUN_100020a0(this,iVar4);
    if (iVar4 == param_1) {
      iVar4 = FUN_10002000(this,param_1);
      *(int *)(*(int *)((int)this + 4) + 4 + iVar4 * 0x14) = iVar1;
    }
    else {
      iVar4 = FUN_10002000(this,param_1);
      *(int *)(*(int *)((int)this + 4) + iVar4 * 0x14) = iVar1;
    }
  }
  *(int *)(*(int *)((int)this + 4) + 4 + iVar1 * 0x14) = param_1;
  if (param_1 != -1) {
    *(int *)(*(int *)((int)this + 4) + 8 + param_1 * 0x14) = iVar1;
  }
  return;
}


// 10002760 FUN_10002760

void __fastcall FUN_10002760(int *param_1)

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
    uVar2 = FUN_1000cd00(param_1,1);
    if ((char)uVar2 != '\0') {
      *(undefined1 *)((*param_1 - param_1[8]) + param_1[4]) = 9;
      param_1[4] = param_1[4] + 1;
      FUN_1000cd50(param_1);
    }
  }
  return;
}


// 100027b0 FUN_100027b0

void __thiscall FUN_100027b0(void *this,undefined1 param_1)

{
  undefined4 uVar1;
  
                    /* WARNING: Load size is inaccurate */
  if ((((*(byte *)((int)this + 0x15) & 1) != 0) && (*(int *)((int)this + 0x10) != 0)) &&
     (*(char *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + -1 + *this) == '\n')) {
    FUN_10002760((int *)this);
  }
  uVar1 = FUN_1000cd00(this,1);
  if ((char)uVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
    FUN_1000cd50((int *)this);
  }
  return;
}


// 100029c0 FUN_100029c0

void __thiscall FUN_100029c0(void *this,undefined4 param_1)

{
  char cVar1;
  char *pcVar2;
  byte *pbVar3;
  uint uVar4;
  int *piVar5;
  int local_18;
  int local_14;
  int aiStack_10 [3];
  
  FUN_100019f0((void *)((int)this + 0x34),&local_18);
  while (((-1 < local_18 && (local_14 != 0)) &&
         ((*(int *)(local_14 + 8) != local_14 || (*(int *)(local_14 + 0xc) == local_14))))) {
    piVar5 = *(int **)(local_14 + 4);
    cVar1 = (**(code **)(*piVar5 + 4))();
    if (cVar1 == '\0') {
      cVar1 = (**(code **)(*piVar5 + 8))(param_1);
      if (cVar1 != '\0') {
        if ((piVar5[5] & 0x1000U) == 0) {
          pcVar2 = *(char **)(piVar5[7] + 0x24);
          if ((byte *)pcVar2 == (byte *)0x0) {
            pcVar2 = &DAT_1001d2ac;
          }
        }
        else {
          pcVar2 = "FCVAR_NEVER_AS_STRING";
        }
        pbVar3 = (byte *)FUN_1000bdf0((int)piVar5);
        uVar4 = FUN_1000b230(pbVar3,(byte *)pcVar2);
        if (uVar4 != 0) {
          FUN_1000bd90((int)piVar5);
        }
      }
    }
    piVar5 = (int *)FUN_10001a40((void *)((int)this + 0x34),aiStack_10,&local_18);
    local_18 = *piVar5;
    local_14 = piVar5[1];
  }
  return;
}


// 10002a90 FUN_10002a90

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

void __cdecl FUN_10002a90(int param_1,char *param_2)

{
  int iVar1;
  int iVar2;
  char local_2000 [8188];
  uint uStack_4;
  
  uStack_4 = 0x10002a9a;
  __vsnprintf(local_2000,0x1fff,param_2,&stack0x0000000c);
  iVar1 = *(int *)(param_1 + 0x24);
  uStack_4 = uStack_4 & 0xffffff;
  if (iVar1 != 0) {
    iVar2 = 0;
    if (0 < iVar1) {
      do {
        (**(code **)(**(int **)(*(int *)(param_1 + 0x18) + iVar2 * 4) + 4))(local_2000);
        iVar2 = iVar2 + 1;
      } while (iVar2 < iVar1);
    }
    return;
  }
  FUN_100027b0((void *)(param_1 + 0x74),1);
  FUN_1000d2c0((void *)(param_1 + 0x74),local_2000);
  return;
}


// 10002b30 FUN_10002b30

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

void __cdecl FUN_10002b30(int param_1,char *param_2)

{
  int iVar1;
  int iVar2;
  char local_2000 [8188];
  uint uStack_4;
  
  uStack_4 = 0x10002b3a;
  __vsnprintf(local_2000,0x1fff,param_2,&stack0x0000000c);
  iVar1 = *(int *)(param_1 + 0x24);
  uStack_4 = uStack_4 & 0xffffff;
  if (iVar1 != 0) {
    iVar2 = 0;
    if (0 < iVar1) {
      do {
        (**(code **)(**(int **)(*(int *)(param_1 + 0x18) + iVar2 * 4) + 8))(local_2000);
        iVar2 = iVar2 + 1;
      } while (iVar2 < iVar1);
    }
    return;
  }
  FUN_100027b0((void *)(param_1 + 0x74),2);
  FUN_1000d2c0((void *)(param_1 + 0x74),local_2000);
  return;
}


// 10002bd0 FUN_10002bd0

void __fastcall FUN_10002bd0(int *param_1)

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
LAB_10002c8a:
        param_1[3] = 0;
        param_1[4] = 0;
        param_1[6] = 0;
        return;
      }
    }
    else if (((((-1 < iVar3) && (iVar3 < *(int *)(iVar2 + 4))) &&
              (puVar1 = (undefined4 *)(iVar3 * 0x10 + 8 + iVar2), puVar1 != (undefined4 *)0x0)) &&
             (uVar4 = FUN_100015f0(puVar1,(uint *)(param_1 + 8)), (char)uVar4 == '\0')) &&
            (((undefined4 *)puVar1[2] != puVar1 || ((undefined4 *)puVar1[3] == puVar1)))) {
      puVar1[2] = puVar1;
      puVar1[3] = param_1[5];
      param_1[5] = (int)puVar1;
    }
    if ((iVar2 == param_1[8]) && (iVar3 == param_1[9])) goto LAB_10002c8a;
    piVar5 = (int *)FUN_10009c10(local_10,&local_18);
  } while( true );
}


// 10002ca0 FUN_10002ca0

void __thiscall FUN_10002ca0(void *this,undefined4 *param_1)

{
  int iVar1;
  int iVar2;
  uint uVar3;
  
  if (param_1 != (undefined4 *)0x0) {
    uVar3 = FUN_100015f0(param_1,(uint *)((int)this + 0x20));
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


// 10002d00 FUN_10002d00

uint __thiscall FUN_10002d00(void *this,int *param_1)

{
  uint uVar1;
  int *piVar2;
  int iVar3;
  
  iVar3 = *(int *)((int)this + 0xc);
  uVar1 = 0;
  if (0 < iVar3) {
                    /* WARNING: Load size is inaccurate */
    piVar2 = *this;
    do {
      if (*piVar2 == *param_1) {
        if (uVar1 != 0xffffffff) {
          iVar3 = (iVar3 - uVar1) + -1;
          if (0 < iVar3) {
            piVar2 = *this + uVar1;
            uVar1 = FUN_1000b3c0(piVar2,piVar2 + 1,iVar3 * 4);
          }
          *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + -1;
          return CONCAT31((int3)(uVar1 >> 8),1);
        }
        break;
      }
      uVar1 = uVar1 + 1;
      piVar2 = piVar2 + 1;
    } while ((int)uVar1 < iVar3);
  }
  return uVar1 & 0xffffff00;
}


// 10002d60 FUN_10002d60

ushort __thiscall FUN_10002d60(void *this,undefined4 param_1)

{
  int iVar1;
  char cVar2;
  undefined2 *puVar3;
  ushort uVar4;
  bool bVar5;
  
  uVar4 = *(ushort *)((int)this + 0x10);
  if (uVar4 == 0xffff) {
    return 0xffff;
  }
  do {
                    /* WARNING: Load size is inaccurate */
    iVar1 = (uint)uVar4 * 0x48;
    cVar2 = (**this)(param_1,*(int *)((int)this + 4) + 8 + iVar1);
    if (cVar2 == '\0') {
                    /* WARNING: Load size is inaccurate */
      cVar2 = (**this)(*(int *)((int)this + 4) + 8 + iVar1,param_1);
      if (cVar2 == '\0') {
        return uVar4;
      }
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      if (uVar4 == 0xffff) {
        puVar3 = &DAT_100293f0;
      }
      else {
        puVar3 = (undefined2 *)(*(int *)((int)this + 4) + iVar1);
      }
      uVar4 = puVar3[1];
    }
    else {
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      bVar5 = uVar4 != 0xffff;
      uVar4 = DAT_100293f0;
      if (bVar5) {
        uVar4 = *(ushort *)(*(int *)((int)this + 4) + iVar1);
      }
    }
  } while (uVar4 != 0xffff);
  return 0xffff;
}


// 10002e70 FUN_10002e70

void __fastcall FUN_10002e70(int param_1)

{
  ushort uVar1;
  ushort uVar2;
  short *psVar3;
  uint uVar4;
  
  uVar4 = DAT_100293f8;
  uVar1 = *(ushort *)(param_1 + 0x10);
  while( true ) {
    uVar2 = uVar1;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_100293f0 = 0xffff;
      DAT_100293f2 = 0xffff;
      DAT_100293f4 = 0xffff;
      DAT_100293f6 = 1;
      DAT_100293f8 = uVar4;
    }
    if (uVar2 == 0xffff) {
      psVar3 = (short *)&DAT_100293f0;
    }
    else {
      psVar3 = (short *)(*(int *)(param_1 + 4) + (uint)uVar2 * 0x48);
    }
    if (*psVar3 == -1) break;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_100293f0 = 0xffff;
      DAT_100293f2 = 0xffff;
      DAT_100293f4 = 0xffff;
      DAT_100293f6 = 1;
      DAT_100293f8 = uVar4;
    }
    uVar1 = DAT_100293f0;
    if (uVar2 != 0xffff) {
      uVar1 = *(ushort *)(*(int *)(param_1 + 4) + (uint)uVar2 * 0x48);
    }
  }
  return;
}


// 10002f30 FUN_10002f30

void __thiscall FUN_10002f30(void *this,ushort param_1)

{
  ushort uVar1;
  ushort uVar2;
  undefined2 *puVar3;
  short *psVar4;
  uint uVar5;
  
  puVar3 = FUN_10001710(this,param_1);
  if (puVar3[1] == -1) {
    puVar3 = FUN_10001710(this,param_1);
    uVar5 = DAT_100293f8;
    while( true ) {
      uVar1 = puVar3[2];
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
        DAT_100293f8 = uVar5;
      }
      if (param_1 == 0xffff) {
        puVar3 = &DAT_100293f0;
      }
      else {
        puVar3 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x48);
      }
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
        DAT_100293f8 = uVar5;
      }
      if (puVar3[2] == 0xffff) {
        puVar3 = &DAT_100293f0;
      }
      else {
        puVar3 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar3[2] * 0x48);
      }
      if ((puVar3[1] != param_1) || (uVar1 == 0xffff)) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
        DAT_100293f8 = uVar5;
      }
      puVar3 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar1 * 0x48);
      param_1 = uVar1;
    }
  }
  else {
    puVar3 = FUN_10001710(this,param_1);
    uVar5 = DAT_100293f8;
    uVar1 = puVar3[1];
    while( true ) {
      uVar2 = uVar1;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
        DAT_100293f8 = uVar5;
      }
      if (uVar2 == 0xffff) {
        psVar4 = (short *)&DAT_100293f0;
      }
      else {
        psVar4 = (short *)(*(int *)((int)this + 4) + (uint)uVar2 * 0x48);
      }
      if (*psVar4 == -1) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
        DAT_100293f8 = uVar5;
      }
      uVar1 = DAT_100293f0;
      if (uVar2 != 0xffff) {
        uVar1 = *(ushort *)(*(int *)((int)this + 4) + (uint)uVar2 * 0x48);
      }
    }
  }
  return;
}


// 100030f0 FUN_100030f0

void __fastcall FUN_100030f0(int param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  uint uVar4;
  
  iVar1 = *(int *)(param_1 + 0x10);
  uVar4 = DAT_1002940c;
  while( true ) {
    iVar2 = iVar1;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_100293fc = -1;
      DAT_10029400 = 0xffffffff;
      DAT_10029404 = 0xffffffff;
      DAT_10029408 = 1;
      DAT_1002940c = uVar4;
    }
    if (iVar2 == -1) {
      piVar3 = &DAT_100293fc;
    }
    else {
      piVar3 = (int *)(*(int *)(param_1 + 4) + iVar2 * 0x14);
    }
    if (*piVar3 == -1) break;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_100293fc = -1;
      DAT_10029400 = 0xffffffff;
      DAT_10029404 = 0xffffffff;
      DAT_10029408 = 1;
      DAT_1002940c = uVar4;
    }
    iVar1 = DAT_100293fc;
    if (iVar2 != -1) {
      iVar1 = *(int *)(*(int *)(param_1 + 4) + iVar2 * 0x14);
    }
  }
  return;
}


// 10003190 FUN_10003190

void __thiscall FUN_10003190(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  undefined4 *puVar4;
  uint uVar5;
  
  iVar1 = FUN_100020a0(this,param_1);
  if (iVar1 == -1) {
    iVar1 = FUN_10002000(this,param_1);
    uVar5 = DAT_1002940c;
    while( true ) {
      iVar2 = iVar1;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293fc = -1;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
        DAT_1002940c = uVar5;
      }
      if (param_1 == -1) {
        puVar4 = &DAT_100293fc;
      }
      else {
        puVar4 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
      }
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293fc = -1;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
        DAT_1002940c = uVar5;
      }
      if (puVar4[2] == -1) {
        puVar4 = &DAT_100293fc;
      }
      else {
        puVar4 = (undefined4 *)(*(int *)((int)this + 4) + puVar4[2] * 0x14);
      }
      if ((puVar4[1] != param_1) || (iVar2 == -1)) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293fc = -1;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
        DAT_1002940c = uVar5;
      }
      iVar1 = *(int *)(*(int *)((int)this + 4) + iVar2 * 0x14 + 8);
      param_1 = iVar2;
    }
  }
  else {
    iVar1 = FUN_100020a0(this,param_1);
    uVar5 = DAT_1002940c;
    while( true ) {
      iVar2 = iVar1;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293fc = -1;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
        DAT_1002940c = uVar5;
      }
      if (iVar2 == -1) {
        piVar3 = &DAT_100293fc;
      }
      else {
        piVar3 = (int *)(*(int *)((int)this + 4) + iVar2 * 0x14);
      }
      if (*piVar3 == -1) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_100293fc = -1;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
        DAT_1002940c = uVar5;
      }
      iVar1 = DAT_100293fc;
      if (iVar2 != -1) {
        iVar1 = *(int *)(*(int *)((int)this + 4) + iVar2 * 0x14);
      }
    }
  }
  return;
}


// 10003320 FUN_10003320

int __thiscall FUN_10003320(void *this,int param_1,undefined4 *param_2)

{
  void *pvVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10006730(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar3 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar3) {
    pvVar1 = (void *)(*this + param_1 * 4);
    FUN_1000b3c0((void *)((int)pvVar1 + 4),pvVar1,iVar3 * 4);
  }
                    /* WARNING: Load size is inaccurate */
  puVar2 = (undefined4 *)(*this + param_1 * 4);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_2;
  }
  return param_1;
}


// 10003390 FUN_10003390

void __fastcall FUN_10003390(void *param_1)

{
  int iVar1;
  int iVar2;
  
  iVar2 = *(int *)((int)param_1 + 0x1c);
  if (iVar2 != -1) {
    iVar1 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (iVar1 != -1) {
      if ((((iVar1 == 0) || (0 < iVar1)) && (iVar1 < *(int *)((int)param_1 + 8))) &&
         ((iVar1 <= iVar2 && (iVar2 = FUN_10002050(param_1,iVar1), iVar2 != iVar1)))) {
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


// 10003420 FUN_10003420

void __thiscall FUN_10003420(void *this,ushort param_1)

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
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      if (param_1 == 0xffff) {
        puVar4 = &DAT_100293f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x48);
      }
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      if (puVar4[2] == 0xffff) {
        puVar4 = &DAT_100293f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0x48);
      }
      if (puVar4[3] != 0) break;
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      if (param_1 == 0xffff) {
        puVar4 = &DAT_100293f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x48);
      }
      uVar1 = puVar4[2];
      uVar7 = (uint)uVar1;
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100293f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar1 * 0x48);
      }
      uVar2 = puVar4[2];
      uVar6 = (uint)uVar2;
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar4 = &DAT_100293f0;
      }
      else {
        puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar1 * 0x48);
      }
      if ((DAT_100293f8 & 1) == 0) {
        DAT_100293f8 = DAT_100293f8 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
      }
      if (puVar4[2] == 0xffff) {
        puVar5 = &DAT_100293f0;
      }
      else {
        puVar5 = (ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0x48);
      }
      if (*puVar5 == uVar1) {
        if ((DAT_100293f8 & 1) == 0) {
          DAT_100293f8 = DAT_100293f8 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
        }
        if (uVar2 == 0xffff) {
          puVar4 = &DAT_100293f0;
        }
        else {
          puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar2 * 0x48);
        }
        uVar3 = puVar4[1];
        if ((DAT_100293f8 & 1) == 0) {
          DAT_100293f8 = DAT_100293f8 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
        }
        if (uVar3 == 0xffff) {
          puVar4 = &DAT_100293f0;
        }
        else {
          puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar3 * 0x48);
        }
        if (puVar4[3] == 0) {
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar1 * 0x48) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar3 * 0x48) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = 0;
          param_1 = uVar2;
        }
        else {
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (param_1 == 0xffff) {
            puVar4 = &DAT_100293f0;
          }
          else {
            puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x48);
          }
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (puVar4[2] == 0xffff) {
            puVar4 = &DAT_100293f0;
          }
          else {
            puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0x48);
          }
          if (puVar4[1] == param_1) {
            FUN_100022c0(this,uVar1);
            puVar4 = FUN_10001710(this,uVar1);
            uVar7 = (uint)(ushort)puVar4[2];
            puVar4 = FUN_10001710(this,puVar4[2]);
            uVar6 = (uint)(ushort)puVar4[2];
            param_1 = uVar1;
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar7 * 0x48) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = 0;
          FUN_100023c0(this,(ushort)uVar6);
        }
      }
      else {
        if ((DAT_100293f8 & 1) == 0) {
          DAT_100293f8 = DAT_100293f8 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
        }
        if (uVar2 == 0xffff) {
          puVar5 = &DAT_100293f0;
        }
        else {
          puVar5 = (ushort *)(*(int *)((int)this + 4) + (uint)uVar2 * 0x48);
        }
        uVar3 = *puVar5;
        if ((DAT_100293f8 & 1) == 0) {
          DAT_100293f8 = DAT_100293f8 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
        }
        if (uVar3 == 0xffff) {
          puVar4 = &DAT_100293f0;
        }
        else {
          puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar3 * 0x48);
        }
        if (puVar4[3] == 0) {
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar1 * 0x48) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar3 * 0x48) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar2 * 0x48) = 0;
          param_1 = uVar2;
        }
        else {
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (param_1 == 0xffff) {
            puVar4 = &DAT_100293f0;
          }
          else {
            puVar4 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x48);
          }
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (puVar4[2] == 0xffff) {
            puVar5 = &DAT_100293f0;
          }
          else {
            puVar5 = (ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar4[2] * 0x48);
          }
          if (*puVar5 == param_1) {
            FUN_100023c0(this,uVar1);
            puVar4 = FUN_10001710(this,uVar1);
            uVar7 = (uint)(ushort)puVar4[2];
            puVar4 = FUN_10001710(this,puVar4[2]);
            uVar6 = (uint)(ushort)puVar4[2];
            param_1 = uVar1;
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar7 * 0x48) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = 0;
          FUN_100022c0(this,(ushort)uVar6);
        }
      }
    } while (param_1 != *(ushort *)((int)this + 0x10));
  }
  *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)*(ushort *)((int)this + 0x10) * 0x48) = 1;
  return;
}


// 10003990 FUN_10003990

void __thiscall FUN_10003990(void *this,uint param_1)

{
  ushort uVar1;
  uint uVar2;
  ushort *puVar3;
  ushort uVar4;
  short *psVar5;
  uint uVar6;
  undefined2 *puVar7;
  short sVar8;
  uint uVar9;
  
  uVar2 = DAT_100293f8;
  if ((short)param_1 != *(short *)((int)this + 0x10)) {
    do {
      sVar8 = (short)param_1;
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
        DAT_100293f8 = uVar2;
      }
      if (sVar8 == -1) {
        puVar7 = &DAT_100293f0;
      }
      else {
        puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (param_1 & 0xffff) * 0x48);
      }
      if (puVar7[3] != 1) break;
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
        DAT_100293f8 = uVar2;
      }
      if (sVar8 == -1) {
        puVar7 = &DAT_100293f0;
      }
      else {
        puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (param_1 & 0xffff) * 0x48);
      }
      uVar4 = puVar7[2];
      uVar9 = (uint)uVar4;
      if ((uVar2 & 1) == 0) {
        uVar2 = uVar2 | 1;
        DAT_100293f0 = 0xffff;
        DAT_100293f2 = 0xffff;
        DAT_100293f4 = 0xffff;
        DAT_100293f6 = 1;
        DAT_100293f8 = uVar2;
      }
      if (uVar4 == 0xffff) {
        psVar5 = &DAT_100293f0;
      }
      else {
        psVar5 = (short *)(*(int *)((int)this + 4) + (uint)uVar4 * 0x48);
      }
      if (sVar8 == *psVar5) {
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (uVar4 == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar4 * 0x48);
        }
        uVar1 = puVar7[1];
        uVar6 = (uint)uVar1;
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (uVar1 == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar1 * 0x48);
        }
        if (puVar7[3] == 0) {
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar4 * 0x48) = 0;
          FUN_100022c0(this,uVar4);
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (sVar8 == -1) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (param_1 & 0xffff) * 0x48);
          }
          uVar4 = puVar7[2];
          uVar9 = (uint)uVar4;
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (uVar4 == 0xffff) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar4 * 0x48);
          }
          uVar6 = (uint)(ushort)puVar7[1];
          uVar2 = DAT_100293f8;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        uVar4 = (ushort)uVar6;
        if (uVar4 == 0xffff) {
          puVar3 = &DAT_100293f0;
        }
        else {
          puVar3 = (ushort *)(*(int *)((int)this + 4) + uVar6 * 0x48);
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (*puVar3 == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)*puVar3 * 0x48);
        }
        if (puVar7[3] != 1) {
LAB_10003d1b:
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
            DAT_100293f8 = uVar2;
          }
          if (uVar4 == 0xffff) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + uVar6 * 0x48);
          }
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
            DAT_100293f8 = uVar2;
          }
          if (puVar7[1] == 0xffff) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar7[1] * 0x48);
          }
          if (puVar7[3] == 1) {
            if ((uVar2 & 1) == 0) {
              DAT_100293f8 = uVar2 | 1;
              DAT_100293f0 = 0xffff;
              DAT_100293f2 = 0xffff;
              DAT_100293f4 = 0xffff;
              DAT_100293f6 = 1;
            }
            if (uVar4 == 0xffff) {
              puVar3 = &DAT_100293f0;
            }
            else {
              puVar3 = (ushort *)(*(int *)((int)this + 4) + uVar6 * 0x48);
            }
            *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)*puVar3 * 0x48) = 1;
            *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = 0;
            FUN_100023c0(this,uVar4);
            if ((DAT_100293f8 & 1) == 0) {
              DAT_100293f8 = DAT_100293f8 | 1;
              DAT_100293f0 = 0xffff;
              DAT_100293f2 = 0xffff;
              DAT_100293f4 = 0xffff;
              DAT_100293f6 = 1;
            }
            if (sVar8 == -1) {
              puVar7 = &DAT_100293f0;
            }
            else {
              puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (param_1 & 0xffff) * 0x48);
            }
            uVar4 = puVar7[2];
            uVar9 = (uint)uVar4;
            if ((DAT_100293f8 & 1) == 0) {
              DAT_100293f8 = DAT_100293f8 | 1;
              DAT_100293f0 = 0xffff;
              DAT_100293f2 = 0xffff;
              DAT_100293f4 = 0xffff;
              DAT_100293f6 = 1;
            }
            if (uVar4 == 0xffff) {
              puVar7 = &DAT_100293f0;
            }
            else {
              puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar4 * 0x48);
            }
            uVar6 = (uint)(ushort)puVar7[1];
            uVar2 = DAT_100293f8;
          }
          if ((uVar2 & 1) == 0) {
            DAT_100293f8 = uVar2 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if ((ushort)uVar9 == 0xffff) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + uVar9 * 0x48);
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = puVar7[3];
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar9 * 0x48) = 1;
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if ((short)uVar6 == -1) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + uVar6 * 0x48);
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)(ushort)puVar7[1] * 0x48) = 1;
          FUN_100022c0(this,(ushort)uVar9);
          goto LAB_1000441e;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (uVar4 == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + uVar6 * 0x48);
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (puVar7[1] == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar7[1] * 0x48);
        }
        if (puVar7[3] != 1) goto LAB_10003d1b;
LAB_10003cf9:
        param_1 = uVar9;
        if ((short)uVar6 != -1) {
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = 0;
          uVar2 = DAT_100293f8;
        }
      }
      else {
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (uVar4 == 0xffff) {
          puVar3 = &DAT_100293f0;
        }
        else {
          puVar3 = (ushort *)(*(int *)((int)this + 4) + (uint)uVar4 * 0x48);
        }
        uVar1 = *puVar3;
        uVar6 = (uint)uVar1;
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (uVar1 == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)uVar1 * 0x48);
        }
        if (puVar7[3] == 0) {
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)uVar4 * 0x48) = 0;
          FUN_100023c0(this,uVar4);
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (sVar8 == -1) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (param_1 & 0xffff) * 0x48);
          }
          uVar4 = puVar7[2];
          uVar9 = (uint)uVar4;
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (uVar4 == 0xffff) {
            puVar3 = &DAT_100293f0;
          }
          else {
            puVar3 = (ushort *)(*(int *)((int)this + 4) + (uint)uVar4 * 0x48);
          }
          uVar6 = (uint)*puVar3;
          uVar2 = DAT_100293f8;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        uVar4 = (ushort)uVar6;
        if (uVar4 == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + uVar6 * 0x48);
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (puVar7[1] == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)(ushort)puVar7[1] * 0x48);
        }
        if (puVar7[3] == 1) {
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
            DAT_100293f8 = uVar2;
          }
          if (uVar4 == 0xffff) {
            puVar3 = &DAT_100293f0;
          }
          else {
            puVar3 = (ushort *)(*(int *)((int)this + 4) + uVar6 * 0x48);
          }
          if ((uVar2 & 1) == 0) {
            uVar2 = uVar2 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
            DAT_100293f8 = uVar2;
          }
          if (*puVar3 == 0xffff) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)*puVar3 * 0x48);
          }
          if (puVar7[3] == 1) goto LAB_10003cf9;
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (uVar4 == 0xffff) {
          puVar3 = &DAT_100293f0;
        }
        else {
          puVar3 = (ushort *)(*(int *)((int)this + 4) + uVar6 * 0x48);
        }
        if ((uVar2 & 1) == 0) {
          uVar2 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
          DAT_100293f8 = uVar2;
        }
        if (*puVar3 == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (uint)*puVar3 * 0x48);
        }
        if (puVar7[3] == 1) {
          if ((uVar2 & 1) == 0) {
            DAT_100293f8 = uVar2 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (uVar4 == 0xffff) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + uVar6 * 0x48);
          }
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)(ushort)puVar7[1] * 0x48) = 1;
          *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = 0;
          FUN_100022c0(this,uVar4);
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (sVar8 == -1) {
            puVar7 = &DAT_100293f0;
          }
          else {
            puVar7 = (undefined2 *)(*(int *)((int)this + 4) + (param_1 & 0xffff) * 0x48);
          }
          uVar4 = puVar7[2];
          uVar9 = (uint)uVar4;
          if ((DAT_100293f8 & 1) == 0) {
            DAT_100293f8 = DAT_100293f8 | 1;
            DAT_100293f0 = 0xffff;
            DAT_100293f2 = 0xffff;
            DAT_100293f4 = 0xffff;
            DAT_100293f6 = 1;
          }
          if (uVar4 == 0xffff) {
            puVar3 = &DAT_100293f0;
          }
          else {
            puVar3 = (ushort *)(*(int *)((int)this + 4) + (uint)uVar4 * 0x48);
          }
          uVar6 = (uint)*puVar3;
          uVar2 = DAT_100293f8;
        }
        if ((uVar2 & 1) == 0) {
          DAT_100293f8 = uVar2 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
        }
        if ((ushort)uVar9 == 0xffff) {
          puVar7 = &DAT_100293f0;
        }
        else {
          puVar7 = (undefined2 *)(*(int *)((int)this + 4) + uVar9 * 0x48);
        }
        *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar6 * 0x48) = puVar7[3];
        *(undefined2 *)(*(int *)((int)this + 4) + 6 + uVar9 * 0x48) = 1;
        if ((DAT_100293f8 & 1) == 0) {
          DAT_100293f8 = DAT_100293f8 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
        }
        if ((short)uVar6 == -1) {
          puVar3 = &DAT_100293f0;
        }
        else {
          puVar3 = (ushort *)(*(int *)((int)this + 4) + uVar6 * 0x48);
        }
        *(undefined2 *)(*(int *)((int)this + 4) + 6 + (uint)*puVar3 * 0x48) = 1;
        FUN_100023c0(this,(ushort)uVar9);
LAB_1000441e:
        param_1 = (uint)*(ushort *)((int)this + 0x10);
        uVar2 = DAT_100293f8;
      }
    } while ((short)param_1 != *(short *)((int)this + 0x10));
  }
  *(undefined2 *)(*(int *)((int)this + 4) + 6 + (param_1 & 0xffff) * 0x48) = 1;
  return;
}


// 10004450 FUN_10004450

void __thiscall FUN_10004450(void *this,int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 uVar3;
  int *piVar4;
  int iVar5;
  int iVar6;
  
  if (param_1 != *(int *)((int)this + 0x10)) {
    do {
      if ((DAT_1002940c & 1) == 0) {
        DAT_1002940c = DAT_1002940c | 1;
        DAT_100293fc = 0xffffffff;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
      }
      if (param_1 == -1) {
        puVar2 = &DAT_100293fc;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
      }
      if ((DAT_1002940c & 1) == 0) {
        DAT_1002940c = DAT_1002940c | 1;
        DAT_100293fc = 0xffffffff;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
      }
      if (puVar2[2] == -1) {
        puVar2 = &DAT_100293fc;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + puVar2[2] * 0x14);
      }
      if (puVar2[3] != 0) break;
      if ((DAT_1002940c & 1) == 0) {
        DAT_1002940c = DAT_1002940c | 1;
        DAT_100293fc = 0xffffffff;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
      }
      if (param_1 == -1) {
        puVar2 = &DAT_100293fc;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
      }
      iVar1 = puVar2[2];
      if ((DAT_1002940c & 1) == 0) {
        DAT_1002940c = DAT_1002940c | 1;
        DAT_100293fc = 0xffffffff;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
      }
      if (iVar1 == -1) {
        puVar2 = &DAT_100293fc;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
      }
      iVar5 = puVar2[2];
      if ((DAT_1002940c & 1) == 0) {
        DAT_1002940c = DAT_1002940c | 1;
        DAT_100293fc = 0xffffffff;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
      }
      if (iVar1 == -1) {
        puVar2 = &DAT_100293fc;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
      }
      if ((DAT_1002940c & 1) == 0) {
        DAT_1002940c = DAT_1002940c | 1;
        DAT_100293fc = 0xffffffff;
        DAT_10029400 = 0xffffffff;
        DAT_10029404 = 0xffffffff;
        DAT_10029408 = 1;
      }
      if (puVar2[2] == -1) {
        piVar4 = &DAT_100293fc;
      }
      else {
        piVar4 = (int *)(*(int *)((int)this + 4) + puVar2[2] * 0x14);
      }
      if (*piVar4 == iVar1) {
        if ((DAT_1002940c & 1) == 0) {
          DAT_1002940c = DAT_1002940c | 1;
          DAT_100293fc = 0xffffffff;
          DAT_10029400 = 0xffffffff;
          DAT_10029404 = 0xffffffff;
          DAT_10029408 = 1;
        }
        if (iVar5 == -1) {
          puVar2 = &DAT_100293fc;
        }
        else {
          puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar5 * 0x14);
        }
        iVar6 = puVar2[1];
        if ((DAT_1002940c & 1) == 0) {
          DAT_1002940c = DAT_1002940c | 1;
          DAT_100293fc = 0xffffffff;
          DAT_10029400 = 0xffffffff;
          DAT_10029404 = 0xffffffff;
          DAT_10029408 = 1;
        }
        if (iVar6 == -1) {
          puVar2 = &DAT_100293fc;
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
          if ((DAT_1002940c & 1) == 0) {
            DAT_1002940c = DAT_1002940c | 1;
            DAT_100293fc = 0xffffffff;
            DAT_10029400 = 0xffffffff;
            DAT_10029404 = 0xffffffff;
            DAT_10029408 = 1;
          }
          if (param_1 == -1) {
            puVar2 = &DAT_100293fc;
          }
          else {
            puVar2 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
          }
          if ((DAT_1002940c & 1) == 0) {
            DAT_1002940c = DAT_1002940c | 1;
            DAT_100293fc = 0xffffffff;
            DAT_10029400 = 0xffffffff;
            DAT_10029404 = 0xffffffff;
            DAT_10029408 = 1;
          }
          if (puVar2[2] == -1) {
            puVar2 = &DAT_100293fc;
          }
          else {
            puVar2 = (undefined4 *)(*(int *)((int)this + 4) + puVar2[2] * 0x14);
          }
          iVar6 = iVar1;
          if (puVar2[1] == param_1) {
            FUN_10002530(this,iVar1);
            if ((DAT_1002940c & 1) == 0) {
              DAT_1002940c = DAT_1002940c | 1;
              DAT_100293fc = 0xffffffff;
              DAT_10029400 = 0xffffffff;
              DAT_10029404 = 0xffffffff;
              DAT_10029408 = 1;
            }
            if (iVar1 == -1) {
              puVar2 = &DAT_100293fc;
            }
            else {
              puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
            }
            iVar6 = puVar2[2];
            if ((DAT_1002940c & 1) == 0) {
              DAT_1002940c = DAT_1002940c | 1;
              DAT_100293fc = 0xffffffff;
              DAT_10029400 = 0xffffffff;
              DAT_10029404 = 0xffffffff;
              DAT_10029408 = 1;
            }
            if (iVar6 == -1) {
              puVar2 = &DAT_100293fc;
            }
            else {
              puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar6 * 0x14);
            }
            iVar5 = puVar2[2];
            param_1 = iVar1;
          }
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar6 * 0x14) = 1;
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar5 * 0x14) = 0;
          FUN_10002660(this,iVar5);
        }
      }
      else {
        if ((DAT_1002940c & 1) == 0) {
          DAT_1002940c = DAT_1002940c | 1;
          DAT_100293fc = 0xffffffff;
          DAT_10029400 = 0xffffffff;
          DAT_10029404 = 0xffffffff;
          DAT_10029408 = 1;
        }
        if (iVar5 == -1) {
          piVar4 = &DAT_100293fc;
        }
        else {
          piVar4 = (int *)(*(int *)((int)this + 4) + iVar5 * 0x14);
        }
        iVar6 = *piVar4;
        if ((DAT_1002940c & 1) == 0) {
          DAT_1002940c = DAT_1002940c | 1;
          DAT_100293fc = 0xffffffff;
          DAT_10029400 = 0xffffffff;
          DAT_10029404 = 0xffffffff;
          DAT_10029408 = 1;
        }
        if (iVar6 == -1) {
          puVar2 = &DAT_100293fc;
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
          uVar3 = FUN_100024c0(this,param_1);
          iVar6 = iVar1;
          if ((char)uVar3 != '\0') {
            FUN_10002660(this,iVar1);
            if ((DAT_1002940c & 1) == 0) {
              DAT_1002940c = DAT_1002940c | 1;
              DAT_100293fc = 0xffffffff;
              DAT_10029400 = 0xffffffff;
              DAT_10029404 = 0xffffffff;
              DAT_10029408 = 1;
            }
            if (iVar1 == -1) {
              puVar2 = &DAT_100293fc;
            }
            else {
              puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar1 * 0x14);
            }
            iVar6 = puVar2[2];
            if ((DAT_1002940c & 1) == 0) {
              DAT_1002940c = DAT_1002940c | 1;
              DAT_100293fc = 0xffffffff;
              DAT_10029400 = 0xffffffff;
              DAT_10029404 = 0xffffffff;
              DAT_10029408 = 1;
            }
            if (iVar6 == -1) {
              puVar2 = &DAT_100293fc;
            }
            else {
              puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar6 * 0x14);
            }
            iVar5 = puVar2[2];
            param_1 = iVar1;
          }
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar6 * 0x14) = 1;
          *(undefined4 *)(*(int *)((int)this + 4) + 0xc + iVar5 * 0x14) = 0;
          FUN_10002530(this,iVar5);
        }
      }
    } while (param_1 != *(int *)((int)this + 0x10));
  }
  *(undefined4 *)(*(int *)((int)this + 4) + 0xc + *(int *)((int)this + 0x10) * 0x14) = 1;
  return;
}


// 10004990 FUN_10004990

undefined1 __fastcall FUN_10004990(int *param_1)

{
  undefined1 uVar1;
  char cVar2;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) != 0) {
    FUN_1000d7d0(param_1,"%c");
    return 0;
  }
  cVar2 = FUN_1000cc30(param_1,1);
  if (cVar2 != '\0') {
    uVar1 = *(undefined1 *)((param_1[3] - param_1[8]) + *param_1);
    param_1[3] = param_1[3] + 1;
    return uVar1;
  }
  return 0;
}


// 100049f0 FUN_100049f0

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */

void __fastcall FUN_100049f0(int *param_1)

{
  int *this;
  char cVar1;
  undefined4 local_10;
  undefined4 local_c;
  char local_5;
  
  if (param_1[0x21] != 0) {
    local_10 = 0;
    if ((char)param_1[0x22] == '\0') {
      this = param_1 + 0x1d;
      do {
        if ((*(byte *)((int)param_1 + 0x89) & 1) == 0) {
          cVar1 = FUN_1000cc30(this,1);
          if (cVar1 == '\0') {
            local_5 = '\0';
          }
          else {
            local_5 = *(char *)((*this - param_1[0x25]) + -1 + param_1[0x20] + 1);
            param_1[0x20] = param_1[0x20] + 1;
          }
        }
        else {
          local_5 = '\0';
          FUN_1000d7d0(this,"%c");
        }
        cVar1 = local_5;
        if (local_5 == '\0') {
          if ((*(byte *)((int)param_1 + 0x89) & 1) == 0) {
            FUN_10001cb0(this,&local_c);
          }
          else {
            local_c = 0;
            FUN_1000d7d0(this,"%d");
          }
          local_10 = local_c;
        }
        FUN_1000cfa0(this);
        FUN_1000d3f0(this,&stack0xffffffe0,0);
        if (cVar1 == '\0') {
          (**(code **)(*param_1 + 0x54))(param_1,&local_10,&stack0xffffffe0);
        }
        else if (cVar1 == '\x01') {
          (**(code **)(*param_1 + 0x58))(param_1,&stack0xffffffe0);
        }
        else if (cVar1 == '\x02') {
          (**(code **)(*param_1 + 0x5c))(param_1,&stack0xffffffe0);
        }
      } while ((char)param_1[0x22] == '\0');
    }
    param_1[0x20] = 0;
    param_1[0x21] = 0;
    param_1[0x25] = 0;
    param_1[0x24] = 0;
    *(undefined1 *)(param_1 + 0x22) = 0;
    if (-1 < param_1[0x1f]) {
      if (param_1[0x1d] != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1[0x1d]);
        param_1[0x1d] = 0;
      }
      param_1[0x1e] = 0;
    }
  }
  return;
}


// 10004b70 FUN_10004b70

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

void __cdecl FUN_10004b70(int param_1,undefined4 *param_2,char *param_3)

{
  void *this;
  int iVar1;
  int iVar2;
  char local_2000 [8188];
  uint uStack_4;
  
  uStack_4 = 0x10004b7a;
  __vsnprintf(local_2000,0x1fff,param_3,&stack0x00000010);
  iVar1 = *(int *)(param_1 + 0x24);
  uStack_4 = uStack_4 & 0xffffff;
  if (iVar1 != 0) {
    iVar2 = 0;
    if (0 < iVar1) {
      do {
        (**(code **)**(undefined4 **)(*(int *)(param_1 + 0x18) + iVar2 * 4))(param_2,local_2000);
        iVar2 = iVar2 + 1;
      } while (iVar2 < iVar1);
    }
    return;
  }
  this = (void *)(param_1 + 0x74);
  FUN_100027b0(this,0);
  if ((*(byte *)(param_1 + 0x89) & 1) != 0) {
    FUN_1000d3b0(this,"%d");
    FUN_1000d2c0(this,local_2000);
    return;
  }
  FUN_10001d10(this,*param_2);
  FUN_1000d2c0(this,local_2000);
  return;
}


// 10004c50 FUN_10004c50

void __fastcall FUN_10004c50(int param_1)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  
  iVar1 = 2;
  piVar2 = (int *)(param_1 + 0x3c);
  do {
    piVar3 = piVar2 + -5;
    if (-1 < piVar2[-3]) {
      if (*piVar3 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar3);
        *piVar3 = 0;
      }
      piVar2[-4] = 0;
    }
    iVar1 = iVar1 + -1;
    piVar2 = piVar3;
  } while (-1 < iVar1);
  return;
}


// 10004ca0 FUN_10004ca0

void __fastcall FUN_10004ca0(int *param_1)

{
  int *piVar1;
  int *piVar2;
  
  FUN_10002bd0(param_1);
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


// 10004d10 FUN_10004d10

void __thiscall FUN_10004d10(void *this,ushort param_1,ushort param_2,char param_3)

{
  undefined2 *puVar1;
  
  puVar1 = (undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x48);
  puVar1[2] = param_2;
  puVar1[1] = 0xffff;
  *puVar1 = 0xffff;
  puVar1[3] = 0;
  if (param_2 == 0xffff) {
    *(ushort *)((int)this + 0x10) = param_1;
    FUN_10003420(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(ushort *)(*(int *)((int)this + 4) + (uint)param_2 * 0x48) = param_1;
    FUN_10003420(this,param_1);
    return;
  }
  *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)param_2 * 0x48) = param_1;
  FUN_10003420(this,param_1);
  return;
}


// 10004d80 FUN_10004d80

void __thiscall FUN_10004d80(void *this,ushort param_1)

{
  ushort uVar1;
  short sVar2;
  int iVar3;
  short *psVar4;
  undefined2 *puVar5;
  ushort *puVar6;
  uint uVar7;
  ushort uVar8;
  
  if (param_1 != 0xffff) {
    psVar4 = FUN_10001710(this,param_1);
    if ((*psVar4 == -1) || (puVar5 = FUN_10001710(this,param_1), puVar5[1] == -1)) {
      uVar7 = (uint)param_1;
    }
    else {
      puVar5 = FUN_10001710(this,param_1);
      uVar8 = puVar5[1];
      while( true ) {
        uVar7 = (uint)uVar8;
        if ((DAT_100293f8 & 1) == 0) {
          DAT_100293f8 = DAT_100293f8 | 1;
          DAT_100293f0 = 0xffff;
          DAT_100293f2 = 0xffff;
          DAT_100293f4 = 0xffff;
          DAT_100293f6 = 1;
        }
        if (uVar8 == 0xffff) {
          psVar4 = &DAT_100293f0;
        }
        else {
          psVar4 = (short *)(*(int *)((int)this + 4) + uVar7 * 0x48);
        }
        if (*psVar4 == -1) break;
        puVar6 = FUN_10001710(this,uVar8);
        uVar8 = *puVar6;
      }
    }
    uVar8 = (ushort)uVar7;
    psVar4 = FUN_10001710(this,uVar8);
    if (*psVar4 == -1) {
      puVar5 = FUN_10001710(this,uVar8);
      uVar1 = puVar5[1];
    }
    else {
      puVar6 = FUN_10001710(this,uVar8);
      uVar1 = *puVar6;
    }
    if (uVar1 != 0xffff) {
      puVar5 = FUN_10001710(this,uVar8);
      *(undefined2 *)(*(int *)((int)this + 4) + 4 + (uint)uVar1 * 0x48) = puVar5[2];
    }
    if (uVar8 == *(ushort *)((int)this + 0x10)) {
      *(ushort *)((int)this + 0x10) = uVar1;
    }
    else {
      puVar5 = FUN_10001710(this,uVar8);
      puVar6 = FUN_10001710(this,puVar5[2]);
      if (*puVar6 == uVar8) {
        puVar5 = FUN_10001710(this,uVar8);
        *(ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar5[2] * 0x48) = uVar1;
      }
      else {
        puVar5 = FUN_10001710(this,uVar8);
        *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)(ushort)puVar5[2] * 0x48) = uVar1;
      }
    }
    puVar5 = FUN_10001710(this,uVar8);
    sVar2 = puVar5[3];
    if (uVar8 != param_1) {
      puVar5 = FUN_10001710(this,param_1);
      iVar3 = uVar7 * 0x48;
      *(undefined2 *)(*(int *)((int)this + 4) + 4 + iVar3) = puVar5[2];
      puVar5 = FUN_10001710(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + 2 + iVar3) = puVar5[1];
      puVar5 = FUN_10001710(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + iVar3) = *puVar5;
      if (param_1 == *(ushort *)((int)this + 0x10)) {
        *(ushort *)((int)this + 0x10) = uVar8;
      }
      else {
        puVar5 = FUN_10001710(this,param_1);
        puVar6 = FUN_10001710(this,puVar5[2]);
        if (*puVar6 == param_1) {
          puVar5 = FUN_10001710(this,param_1);
          *(ushort *)(*(int *)((int)this + 4) + (uint)(ushort)puVar5[2] * 0x48) = uVar8;
        }
        else {
          puVar5 = FUN_10001710(this,param_1);
          *(ushort *)(*(int *)((int)this + 4) + 2 + (uint)(ushort)puVar5[2] * 0x48) = uVar8;
        }
      }
      psVar4 = FUN_10001710(this,uVar8);
      if (*psVar4 != -1) {
        puVar6 = FUN_10001710(this,uVar8);
        *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)*puVar6 * 0x48) = uVar8;
      }
      puVar5 = FUN_10001710(this,uVar8);
      if (puVar5[1] != -1) {
        puVar5 = FUN_10001710(this,uVar8);
        *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)(ushort)puVar5[1] * 0x48) = uVar8;
      }
      puVar5 = FUN_10001710(this,param_1);
      *(undefined2 *)(*(int *)((int)this + 4) + 6 + iVar3) = puVar5[3];
    }
    if ((uVar1 != 0xffff) && (sVar2 == 1)) {
      FUN_10003990(this,(uint)uVar1);
    }
    return;
  }
  return;
}


// 10005000 FUN_10005000

void __thiscall FUN_10005000(void *this,int param_1,int param_2,char param_3)

{
  undefined4 *puVar1;
  
  puVar1 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x14);
  puVar1[2] = param_2;
  puVar1[1] = 0xffffffff;
  *puVar1 = 0xffffffff;
  puVar1[3] = 0;
  if (param_2 == -1) {
    *(int *)((int)this + 0x10) = param_1;
    FUN_10004450(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(int *)(*(int *)((int)this + 4) + param_2 * 0x14) = param_1;
    FUN_10004450(this,param_1);
    return;
  }
  *(int *)(*(int *)((int)this + 4) + 4 + param_2 * 0x14) = param_1;
  FUN_10004450(this,param_1);
  return;
}


// 10005070 FUN_10005070

undefined4 * __fastcall FUN_10005070(undefined4 *param_1)

{
  undefined4 *puVar1;
  int iVar2;
  
  iVar2 = 2;
  puVar1 = param_1;
  do {
    FUN_1000c9c0(puVar1);
    FUN_1000c830(puVar1,"");
    puVar1[4] = 0;
    puVar1 = puVar1 + 5;
    iVar2 = iVar2 + -1;
  } while (-1 < iVar2);
  iVar2 = 3;
  puVar1 = param_1;
  do {
    FUN_1000c830(puVar1,"");
    puVar1[4] = 0;
    puVar1 = puVar1 + 5;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  return param_1;
}


// 100050f0 FUN_100050f0

void __fastcall FUN_100050f0(int *param_1)

{
  FUN_10003320(param_1 + 6,param_1[9],(undefined4 *)&stack0x00000004);
  FUN_100049f0(param_1);
  return;
}


// 10005110 FUN_10005110

uint * __thiscall FUN_10005110(void *this,int *param_1)

{
  int iVar1;
  uint uVar2;
  uint *puVar3;
  byte *pbVar4;
  uint uVar5;
  uint uVar6;
  
  puVar3 = (uint *)FUN_10001d70((void *)((int)this + 0x14),'\x01');
  if (puVar3 == (uint *)0x0) {
    return (uint *)0xffffffff;
  }
  pbVar4 = (byte *)(**(code **)(*param_1 + 0x18))();
  uVar5 = FUN_1000c620(pbVar4);
                    /* WARNING: Load size is inaccurate */
  iVar1 = *this;
  *puVar3 = uVar5;
  puVar3[1] = (uint)param_1;
  uVar2 = *(uint *)(iVar1 + (uVar5 & 0xff) * 4);
  FUN_10002ca0((void *)((int)this + 0x14),puVar3);
  puVar3[3] = uVar2;
  if (uVar2 == 0) {
    uVar6 = *(uint *)((int)this + 0x24);
    puVar3[2] = uVar6;
    *(uint **)((int)this + 0x24) = puVar3;
  }
  else {
    uVar6 = *(uint *)(uVar2 + 8);
    puVar3[2] = uVar6;
    *(uint **)(uVar2 + 8) = puVar3;
  }
  if (uVar6 == 0) {
    *(uint **)((int)this + 0x20) = puVar3;
  }
  else {
    *(uint **)(uVar6 + 0xc) = puVar3;
  }
  *(int *)((int)this + 0x2c) = *(int *)((int)this + 0x2c) + 1;
                    /* WARNING: Load size is inaccurate */
  *(uint **)(*this + (uVar5 & 0xff) * 4) = puVar3;
  return puVar3;
}


// 10005270 FUN_10005270

void __thiscall FUN_10005270(void *this,int *param_1)

{
  uint *puVar1;
  
  puVar1 = FUN_10001c00(this,param_1);
  if (puVar1 == (uint *)0xffffffff) {
    FUN_10005110(this,param_1);
  }
  return;
}


// 10005290 FUN_10005290

int __thiscall FUN_10005290(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  int iVar3;
  void *local_4;
  
  puVar2 = param_1;
  local_4 = this;
  FUN_100020f0(this,param_1,(int *)&local_4,(undefined1 *)&param_1);
  iVar3 = FUN_100021e0((int)this);
  FUN_10005000(this,iVar3,(int)local_4,(char)param_1);
  *(int *)((int)this + 0x14) = *(int *)((int)this + 0x14) + 1;
  puVar1 = (undefined4 *)(*(int *)((int)this + 4) + 0x10 + iVar3 * 0x14);
  if (puVar1 != (undefined4 *)0x0) {
    *puVar1 = *puVar2;
  }
  return iVar3;
}


// 100052f0 FUN_100052f0

int __thiscall FUN_100052f0(void *this,int param_1)

{
  int iVar1;
  void *pvVar2;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10001860(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar2 = (void *)(*this + param_1 * 0x20);
    FUN_1000b3c0((void *)((int)pvVar2 + 0x20),pvVar2,iVar1 * 0x20);
  }
                    /* WARNING: Load size is inaccurate */
  iVar1 = param_1 * 0x20 + *this;
  if (iVar1 != 0) {
    FUN_1000c9c0((undefined4 *)(iVar1 + 0x10));
  }
  return param_1;
}


// 10005360 FUN_10005360

ushort __fastcall FUN_10005360(int param_1)

{
  ushort *puVar1;
  ushort uVar2;
  ushort *puVar3;
  int iVar4;
  undefined4 uStack_4;
  
  uVar2 = *(ushort *)(param_1 + 0x14);
  if (uVar2 == 0xffff) {
    uVar2 = *(ushort *)(param_1 + 0x16);
    puVar1 = (ushort *)(param_1 + 0x16);
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)uVar2)
       ) {
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
    uStack_4._0_2_ = (undefined2)param_1;
    uStack_4 = CONCAT22(uVar2,(undefined2)uStack_4);
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 <= (int)(uint)uVar2)) {
      FUN_100017b0((void *)(param_1 + 4),1);
      uVar2 = *puVar1;
      if (((uVar2 != 0) && (uVar2 == 0)) ||
         (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)uVar2)) {
        iVar4 = *(int *)(param_1 + 8);
        uStack_4 = CONCAT22((0 < iVar4) - 1,(undefined2)uStack_4);
        puVar3 = (ushort *)((int)&uStack_4 + 2);
      }
      else {
        puVar3 = (ushort *)
                 FUN_10001770((void *)(param_1 + 4),(short *)((int)&uStack_4 + 2),(short *)puVar1);
      }
      uVar2 = *puVar3;
      if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 <= (int)(uint)uVar2)) {
        Error("CUtlRBTree overflow!\n");
      }
    }
    *puVar1 = uVar2;
  }
  else {
    *(undefined2 *)(param_1 + 0x14) =
         *(undefined2 *)(*(int *)(param_1 + 4) + 2 + (uint)uVar2 * 0x48);
    uStack_4 = param_1;
  }
  iVar4 = *(int *)(param_1 + 4) + 8 + (uint)uVar2 * 0x48;
  if (iVar4 != 0) {
    FUN_10005070((undefined4 *)(iVar4 + 4));
  }
  *(undefined4 *)(param_1 + 0x18) = *(undefined4 *)(param_1 + 4);
  return uVar2;
}


// 10005760 FUN_10005760

/* WARNING: Removing unreachable block (ram,0x100058df) */

void __thiscall FUN_10005760(void *this,int *param_1)

{
  char *pcVar1;
  int iVar2;
  char cVar3;
  char *pcVar4;
  char *pcVar5;
  int *piVar6;
  int iVar7;
  int *local_40;
  int local_3c;
  int local_38;
  int aiStack_34 [2];
  undefined1 *local_2c;
  int local_28;
  undefined4 local_24;
  int local_20;
  undefined4 local_1c;
  undefined4 local_18;
  undefined4 local_14;
  undefined4 local_10;
  undefined4 local_c;
  
  if (*param_1 != 2) {
    ConMsg("Usage:  find <string>\n");
    return;
  }
  pcVar1 = (char *)param_1[0x103];
  local_1c = 0xffffffff;
  local_14 = 0xffffffff;
  local_10 = 0xffffffff;
  local_2c = &LAB_100012d0;
  local_28 = 0;
  local_24 = 0;
  local_20 = 0;
  local_18 = 0;
  local_c = 0;
  FUN_100019f0((void *)((int)this + 0x34),&local_3c);
  do {
    if (((local_3c < 0) || (local_38 == 0)) ||
       ((*(int *)(local_38 + 8) == local_38 && (*(int *)(local_38 + 0xc) != local_38)))) {
      iVar7 = FUN_100030f0((int)&local_2c);
      iVar2 = local_28;
      for (; iVar7 != -1; iVar7 = FUN_10003190(&local_2c,iVar7)) {
        FUN_1000bfb0(*(int **)(iVar2 + 0x10 + iVar7 * 0x14));
      }
      FUN_10003390(&local_2c);
      if ((-1 < local_20) && (local_28 != 0)) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_28);
      }
      return;
    }
    local_40 = *(int **)(local_38 + 4);
    cVar3 = (**(code **)(*local_40 + 8))(2);
    if ((cVar3 == '\0') && (cVar3 = (**(code **)(*local_40 + 8))(0x10), cVar3 == '\0')) {
      pcVar5 = pcVar1;
      pcVar4 = (char *)(**(code **)(*local_40 + 0x18))();
      pcVar5 = FUN_1000b2b0(pcVar4,pcVar5);
      if (pcVar5 == (char *)0x0) {
        pcVar5 = pcVar1;
        pcVar4 = (char *)(**(code **)(*local_40 + 0x1c))();
        pcVar5 = FUN_1000b2b0(pcVar4,pcVar5);
        if (pcVar5 == (char *)0x0) goto LAB_1000585a;
      }
      FUN_10005290(&local_2c,&local_40);
    }
LAB_1000585a:
    piVar6 = (int *)FUN_10001a40((void *)((int)this + 0x34),aiStack_34,&local_3c);
    local_3c = *piVar6;
    local_38 = piVar6[1];
  } while( true );
}


// 10005900 FUN_10005900

void __fastcall FUN_10005900(int *param_1)

{
  int *piVar1;
  void *pvVar2;
  int *piVar3;
  int iVar4;
  int *piVar5;
  
  param_1[3] = 0;
  if (param_1[1] < 0x100) {
    FUN_10006730(param_1,0x100 - param_1[1]);
  }
  param_1[3] = param_1[3] + 0x100;
  pvVar2 = (void *)*param_1;
  param_1[4] = (int)pvVar2;
  if (0 < param_1[3] + -0x100) {
    FUN_1000b3c0((void *)((int)pvVar2 + 0x400),pvVar2,(param_1[3] + -0x100) * 4);
  }
  iVar4 = 0;
  do {
    *(undefined4 *)(iVar4 + *param_1) = 0;
    iVar4 = iVar4 + 4;
  } while (iVar4 < 0x400);
  piVar1 = param_1 + 5;
  FUN_10002bd0(piVar1);
  piVar5 = (int *)*piVar1;
  if ((int *)*piVar1 != (int *)0x0) {
    do {
      piVar3 = (int *)*piVar5;
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar5);
      piVar5 = piVar3;
    } while (piVar3 != (int *)0x0);
    *piVar1 = 0;
    param_1[6] = 0;
  }
  param_1[0xf] = 0;
  param_1[7] = 0x400;
  return;
}


// 100059a0 FUN_100059a0

void __fastcall FUN_100059a0(int *param_1)

{
  int *piVar1;
  int iVar2;
  int iVar3;
  
  iVar3 = param_1[3] + -1;
  if (-1 < iVar3) {
    iVar2 = iVar3 * 0x20;
    do {
      piVar1 = (int *)(iVar2 + 0x10 + *param_1);
      if (-1 < *(int *)(iVar2 + 0x18 + *param_1)) {
        if (*piVar1 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar1);
          *piVar1 = 0;
        }
        piVar1[1] = 0;
      }
      iVar3 = iVar3 + -1;
      iVar2 = iVar2 + -0x20;
    } while (-1 < iVar3);
  }
  param_1[3] = 0;
  return;
}


// 10005a00 FUN_10005a00

void __fastcall FUN_10005a00(void *param_1)

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
         ((uVar3 <= uVar1 && (puVar5 = FUN_10001710(param_1,uVar3), *puVar5 != uVar3)))) {
        iVar2 = (uint)uVar3 * 0x48;
        FUN_10004c50(iVar2 + 0xc + *(int *)((int)param_1 + 4));
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


// 10005ae0 FUN_10005ae0

undefined4 * __fastcall FUN_10005ae0(undefined4 *param_1)

{
  undefined4 *puVar1;
  
  if (param_1 == (undefined4 *)0x0) {
    puVar1 = (undefined4 *)0x0;
  }
  else {
    puVar1 = param_1 + 9;
  }
  FUN_1000b720(param_1,0x1001d770,(int)puVar1,
               "Find concommands with the specified string in their name/help text.",0,0);
  param_1[9] = &PTR_LAB_1001d3ac;
  param_1[10] = &PTR_LAB_1001d3b4;
  param_1[0xb] = param_1 + -0x37;
  *param_1 = &PTR_FUN_1001d36c;
  param_1[9] = &PTR_LAB_1001d360;
  param_1[10] = &PTR_LAB_1001d358;
  param_1[0xc] = FUN_10005760;
  param_1[0xd] = 0;
  return param_1;
}


// 10005c00 FUN_10005c00

uint __fastcall FUN_10005c00(int param_1)

{
  int iVar1;
  undefined1 *puVar2;
  uint uVar3;
  code *pcVar4;
  int iVar5;
  int *piVar6;
  uint local_8;
  int local_4;
  
  local_4 = *(int *)(param_1 + 0xd0);
  iVar5 = 0;
  *(undefined1 *)(param_1 + 0xd8) = 1;
  local_8 = 0;
  if (0 < local_4) {
    do {
      iVar1 = *(int *)(*(int *)(param_1 + 0xc4) + 4 + iVar5);
      piVar6 = (int *)(*(int *)(param_1 + 0xc4) + iVar5);
      if (iVar1 == 0) {
        iVar1 = *(int *)(*piVar6 + 0x18);
        puVar2 = FUN_1000c6e0(piVar6 + 4);
        pcVar4 = *(code **)(iVar1 + 8);
LAB_10005c7d:
        (*pcVar4)(puVar2);
      }
      else if (iVar1 == 1) {
        (*(code *)**(undefined4 **)(*piVar6 + 0x18))(piVar6[2]);
      }
      else if (iVar1 == 2) {
        puVar2 = (undefined1 *)piVar6[3];
        pcVar4 = *(code **)(*(int *)(*piVar6 + 0x18) + 4);
        goto LAB_10005c7d;
      }
      uVar3 = (**(code **)(*(int *)*piVar6 + 0x14))();
      local_8 = local_8 | uVar3 & 0xb00000;
      iVar5 = iVar5 + 0x20;
      local_4 = local_4 + -1;
    } while (local_4 != 0);
  }
  FUN_100059a0((int *)(param_1 + 0xc4));
  *(undefined1 *)(param_1 + 0xd8) = 0;
  return local_8;
}


// 10005cc0 FUN_10005cc0

ushort __thiscall FUN_10005cc0(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  ushort uVar3;
  void *local_4;
  
  puVar2 = param_1;
  local_4 = this;
  FUN_10001ef0(this,param_1,(ushort *)&local_4,(undefined1 *)&param_1);
  uVar3 = FUN_10005360((int)this);
  FUN_10004d10(this,uVar3,(ushort)local_4,(char)param_1);
  *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + 1;
  puVar1 = (undefined4 *)(*(int *)((int)this + 4) + 8 + (uint)uVar3 * 0x48);
  if (puVar1 != (undefined4 *)0x0) {
    *puVar1 = *puVar2;
    FUN_100013d0(puVar1 + 1,(int)(puVar2 + 1),0x14,3,&LAB_10004cf0);
  }
  return uVar3;
}


// 10005d40 FUN_10005d40

void __fastcall FUN_10005d40(int *param_1)

{
  int iVar1;
  
  FUN_100059a0(param_1);
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


// 10005e90 FUN_10005e90

int * __fastcall FUN_10005e90(int *param_1)

{
  int *this;
  
  this = param_1 + 5;
  *param_1 = 0;
  param_1[1] = 0;
  param_1[2] = 0;
  param_1[3] = 0;
  param_1[4] = 0;
  *this = 0;
  param_1[6] = 0;
  param_1[7] = 0;
  FUN_100015a0(this,0,0);
  param_1[0xd] = 0;
  param_1[0xe] = -1;
  param_1[8] = 0;
  param_1[9] = 0;
  param_1[10] = 0;
  param_1[0xb] = 0;
  param_1[0xc] = 0;
  param_1[0xf] = 0;
  param_1[3] = 0;
  if (-1 < param_1[2]) {
    if (*param_1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
      *param_1 = 0;
    }
    param_1[1] = 0;
  }
  param_1[4] = *param_1;
  FUN_10004ca0(this);
  FUN_10005900(param_1);
  return param_1;
}


// 10005f20 FUN_10005f20

void __fastcall FUN_10005f20(int *param_1)

{
  int *piVar1;
  int *piVar2;
  int *piVar3;
  
  param_1[3] = 0;
  if (-1 < param_1[2]) {
    if (*param_1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
      *param_1 = 0;
    }
    param_1[1] = 0;
  }
  param_1[4] = *param_1;
  piVar1 = param_1 + 5;
  FUN_10004ca0(piVar1);
  FUN_10002bd0(piVar1);
  piVar3 = (int *)*piVar1;
  if ((int *)*piVar1 != (int *)0x0) {
    do {
      piVar2 = (int *)*piVar3;
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar3);
      piVar3 = piVar2;
    } while (piVar2 != (int *)0x0);
    *piVar1 = 0;
    param_1[6] = 0;
  }
  FUN_10007b50(param_1);
  return;
}


// 10005fa0 FUN_10005fa0

void __fastcall FUN_10005fa0(void *param_1)

{
  FUN_10005a00(param_1);
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


// 10006190 FUN_10006190

void __fastcall FUN_10006190(int param_1)

{
  undefined4 *puVar1;
  
  puVar1 = (undefined4 *)(param_1 + 0xdc);
  *puVar1 = &PTR_FUN_1001d36c;
  *(undefined ***)(param_1 + 0x100) = &PTR_LAB_1001d360;
  *(undefined ***)(param_1 + 0x104) = &PTR_LAB_1001d358;
  FUN_1000b690(puVar1);
  FUN_1000b7b0(puVar1);
  FUN_10005d40((int *)(param_1 + 0xc4));
  FUN_10005fa0((void *)(param_1 + 0xa8));
  if (-1 < *(int *)(param_1 + 0x7c)) {
    if (*(int *)(param_1 + 0x74) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x74));
      *(undefined4 *)(param_1 + 0x74) = 0;
    }
    *(undefined4 *)(param_1 + 0x78) = 0;
  }
  FUN_10005f20((int *)(param_1 + 0x34));
  FUN_10007b50((int *)(param_1 + 0x18));
  FUN_10007b50((int *)(param_1 + 4));
  return;
}


// 10006210 FUN_10006210

undefined4 * __fastcall FUN_10006210(undefined4 *param_1)

{
  *param_1 = &PTR_LAB_1001d7cc;
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
  FUN_10005e90(param_1 + 0xd);
  FUN_1000d240(param_1 + 0x1d,0,0x400,0);
  param_1[0x2a] = &LAB_10001360;
  param_1[0x2b] = 0;
  param_1[0x2c] = 0;
  param_1[0x2d] = 0;
  *(undefined2 *)((int)param_1 + 0xba) = 0;
  *(undefined2 *)(param_1 + 0x2e) = 0xffff;
  *(undefined2 *)(param_1 + 0x2f) = 0xffff;
  *(undefined2 *)((int)param_1 + 0xbe) = 0xffff;
  param_1[0x30] = param_1[0x2b];
  param_1[0x31] = 0;
  param_1[0x32] = 0;
  param_1[0x33] = 0;
  param_1[0x34] = 0;
  param_1[0x35] = 0;
  FUN_10005ae0(param_1 + 0x37);
  param_1[0xb] = 0;
  param_1[0xc] = 0;
  param_1[0x29] = 1;
  *(undefined1 *)(param_1 + 0x36) = 0;
  FUN_10005900(param_1 + 0xd);
  return param_1;
}


// 100062f0 FUN_100062f0

void __thiscall FUN_100062f0(void *this,int param_1)

{
  void *this_00;
  void *pvVar1;
  undefined4 *puVar2;
  int *piVar3;
  ushort uVar4;
  int iVar5;
  int iVar6;
  int iVar7;
  int *piVar8;
  int local_54;
  int local_50;
  int local_4c;
  int local_48;
  int local_44;
  undefined4 uStack_40;
  undefined4 local_3c [15];
  
  if (*(int *)((int)this + 0xa4) != 1) {
    this_00 = (void *)((int)this + 0xa8);
    local_54 = 0;
    local_50 = 0;
    local_4c = 0;
    local_48 = 0;
    local_44 = 0;
    for (uVar4 = FUN_10002e70((int)this_00); uVar4 != 0xffff; uVar4 = FUN_10002f30(this_00,uVar4)) {
      iVar7 = (uint)uVar4 * 0x48;
      piVar3 = *(int **)(*(int *)((int)this + 0xac) + 8 + iVar7);
      iVar5 = (**(code **)(*piVar3 + 0x24))();
      if (iVar5 == param_1) {
        iVar5 = 1;
        if (1 < *(int *)((int)this + 0xa4)) {
          piVar8 = (int *)(*(int *)((int)this + 0xac) + iVar7 + 0x1c);
          do {
            if (*piVar8 != 0) {
                    /* WARNING: Load size is inaccurate */
              (**(code **)(*this + 0x1c))(*piVar8);
              if ((undefined4 *)*piVar8 != (undefined4 *)0x0) {
                (*(code *)**(undefined4 **)*piVar8)(1);
              }
              *piVar8 = 0;
            }
            iVar5 = iVar5 + 1;
            piVar8 = piVar8 + 5;
          } while (iVar5 < *(int *)((int)this + 0xa4));
        }
        iVar7 = local_48;
        if (local_50 < local_48 + 1) {
          FUN_10006730(&local_54,(local_48 - local_50) + 1);
        }
        iVar5 = local_54;
        local_48 = local_48 + 1;
        iVar6 = (local_48 - iVar7) + -1;
        local_44 = local_54;
        if (0 < iVar6) {
          pvVar1 = (void *)(local_54 + iVar7 * 4);
          FUN_1000b3c0((void *)((int)pvVar1 + 4),pvVar1,iVar6 * 4);
        }
        puVar2 = (undefined4 *)(iVar5 + iVar7 * 4);
        if (puVar2 != (undefined4 *)0x0) {
          *puVar2 = piVar3;
        }
      }
    }
    iVar7 = 0;
    if (0 < local_48) {
      do {
        FUN_10005070(local_3c);
        uStack_40 = *(undefined4 *)(local_54 + iVar7 * 4);
        uVar4 = FUN_10002d60(this_00,&uStack_40);
        if (uVar4 != 0xffff) {
          FUN_10004d80(this_00,uVar4);
          iVar5 = (uint)uVar4 * 0x48;
          FUN_10004c50(*(int *)((int)this + 0xac) + 0xc + iVar5);
          *(ushort *)(iVar5 + *(int *)((int)this + 0xac)) = uVar4;
          *(undefined2 *)(*(int *)((int)this + 0xac) + 2 + iVar5) =
               *(undefined2 *)((int)this + 0xbc);
          *(short *)((int)this + 0xba) = *(short *)((int)this + 0xba) + -1;
          *(ushort *)((int)this + 0xbc) = uVar4;
        }
        FUN_10004c50((int)local_3c);
        iVar7 = iVar7 + 1;
      } while (iVar7 < local_48);
    }
    if ((-1 < local_4c) && (local_54 != 0)) {
                    /* WARNING: Could not recover jumptable at 0x100064e4. Too many branches */
                    /* WARNING: Treating indirect jump as call */
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))();
      return;
    }
  }
  return;
}


// 100064f0 FUN_100064f0

void __cdecl FUN_100064f0(uint param_1,int param_2,undefined4 param_3,undefined4 param_4)

{
  uint uVar1;
  uint uVar2;
  undefined4 local_100 [64];
  
  uVar1 = 0;
  while( true ) {
    uVar2 = param_1;
    if (0x3f < param_1) {
      uVar2 = 0x40;
    }
    if (uVar2 <= uVar1) break;
    local_100[uVar1] = **(undefined4 **)(param_2 + uVar1 * 4);
    uVar1 = uVar1 + 1;
  }
  ThreadWaitForObjects(param_1,local_100,param_3,param_4);
  return;
}


// 10006560 FUN_10006560

undefined4 __thiscall FUN_10006560(void *this,undefined4 *param_1)

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


// 100065c0 FUN_100065c0

undefined4 * __fastcall FUN_100065c0(undefined4 *param_1)

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


// 10006600 FUN_10006600

int __fastcall FUN_10006600(int *param_1)

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


// 10006690 CGaussianRandomStream::operator=

/* public: class CGaussianRandomStream & __thiscall CGaussianRandomStream::operator=(class
   CGaussianRandomStream const &) */

CGaussianRandomStream * __thiscall
CGaussianRandomStream::operator=(CGaussianRandomStream *this,CGaussianRandomStream *param_1)

{
                    /* 0x6690  4  ??4CGaussianRandomStream@@QAEAAV0@ABV0@@Z */
  *(undefined4 *)this = *(undefined4 *)param_1;
  *(undefined4 *)(this + 4) = *(undefined4 *)(param_1 + 4);
  *(undefined4 *)(this + 8) = *(undefined4 *)(param_1 + 8);
  *(undefined4 *)(this + 0xc) = *(undefined4 *)(param_1 + 0xc);
  *(undefined4 *)(this + 0x10) = *(undefined4 *)(param_1 + 0x10);
  return this;
}


// 100066c0 CGaussianRandomStream::`default_constructor_closure'

/* public: void __thiscall CGaussianRandomStream::`default constructor closure'(void) */

void __thiscall CGaussianRandomStream::_default_constructor_closure_(CGaussianRandomStream *this)

{
                    /* 0x66c0  7  ??_FCGaussianRandomStream@@QAEXXZ */
  CGaussianRandomStream(this,(IUniformRandomStream *)0x0);
  return;
}


// 100066d0 DestroyThreadPool

void __cdecl DestroyThreadPool(int *param_1)

{
                    /* 0x66d0  17  DestroyThreadPool */
  if (param_1 != (int *)0x0) {
    (**(code **)(*param_1 + 8))(1);
  }
  return;
}


// 10006730 FUN_10006730

void __thiscall FUN_10006730(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000678b;
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
LAB_1000678b:
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


// 100067c0 FUN_100067c0

int __fastcall FUN_100067c0(int *param_1)

{
  CThreadFastMutex *this;
  int *piVar1;
  code *pcVar2;
  DWORD DVar3;
  int iVar4;
  int unaff_EBX;
  
  iVar4 = param_1[3];
  if (((iVar4 != 1) && (iVar4 != 2)) && (iVar4 != 4)) {
    return unaff_EBX;
  }
  this = (CThreadFastMutex *)(param_1 + 5);
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
      goto LAB_1000680d;
    }
  }
  param_1[6] = param_1[6] + 1;
LAB_1000680d:
  (**(code **)*param_1)();
  switch(param_1[3]) {
  default:
    break;
  case 1:
  case 4:
    pcVar2 = *(code **)(*param_1 + 0xc);
    param_1[3] = 2;
    iVar4 = (*pcVar2)();
    param_1[3] = iVar4;
    (**(code **)(*param_1 + 0x14))();
    CThreadEvent::Set((CThreadEvent *)(param_1 + 10));
  }
  (**(code **)(*param_1 + 4))();
  piVar1 = param_1 + 6;
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return unaff_EBX;
}


// 10006880 FUN_10006880

int __thiscall FUN_10006880(int *param_1,undefined4 param_2)

{
  CThreadFastMutex *this;
  int *piVar1;
  DWORD DVar2;
  int iVar3;
  int unaff_EBP;
  
  iVar3 = param_1[3];
  if (((iVar3 != 1) && (iVar3 != 2)) && (iVar3 != 4)) {
    return unaff_EBP;
  }
  this = (CThreadFastMutex *)(param_1 + 5);
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
      goto LAB_100068ce;
    }
  }
  param_1[6] = param_1[6] + 1;
LAB_100068ce:
  (**(code **)*param_1)();
  switch(param_1[3]) {
  default:
    break;
  case 1:
  case 4:
    iVar3 = (**(code **)(*param_1 + 0x10))(param_2);
    param_1[3] = iVar3;
    if ((char)param_2 != '\0') {
      (**(code **)(*param_1 + 0x14))();
    }
    CThreadEvent::Set((CThreadEvent *)(param_1 + 10));
  }
  (**(code **)(*param_1 + 4))();
  piVar1 = param_1 + 6;
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return unaff_EBP;
}


// 10006950 CUniformRandomStream::CUniformRandomStream

/* public: __thiscall CUniformRandomStream::CUniformRandomStream(class CUniformRandomStream const &)
    */

void __thiscall
CUniformRandomStream::CUniformRandomStream(CUniformRandomStream *this,CUniformRandomStream *param_1)

{
  int iVar1;
  CUniformRandomStream *pCVar2;
  CUniformRandomStream *pCVar3;
  
                    /* 0x6950  2  ??0CUniformRandomStream@@QAE@ABV0@@Z */
  *(undefined ***)this = &_vftable_;
  *(undefined4 *)(this + 4) = *(undefined4 *)(param_1 + 4);
  *(undefined4 *)(this + 8) = *(undefined4 *)(param_1 + 8);
  pCVar2 = param_1 + 0xc;
  pCVar3 = this + 0xc;
  for (iVar1 = 0x20; iVar1 != 0; iVar1 = iVar1 + -1) {
    *(undefined4 *)pCVar3 = *(undefined4 *)pCVar2;
    pCVar2 = pCVar2 + 4;
    pCVar3 = pCVar3 + 4;
  }
  *(undefined4 *)(this + 0x8c) = *(undefined4 *)(param_1 + 0x8c);
  *(undefined4 *)(this + 0x90) = *(undefined4 *)(param_1 + 0x90);
  return;
}


// 100069a0 CUniformRandomStream::operator=

/* public: class CUniformRandomStream & __thiscall CUniformRandomStream::operator=(class
   CUniformRandomStream const &) */

CUniformRandomStream * __thiscall
CUniformRandomStream::operator=(CUniformRandomStream *this,CUniformRandomStream *param_1)

{
  CUniformRandomStream *pCVar1;
  int iVar2;
  
                    /* 0x69a0  5  ??4CUniformRandomStream@@QAEAAV0@ABV0@@Z */
  *(undefined4 *)(this + 4) = *(undefined4 *)(param_1 + 4);
  *(undefined4 *)(this + 8) = *(undefined4 *)(param_1 + 8);
  pCVar1 = this + 0xc;
  iVar2 = 0x20;
  do {
    *(undefined4 *)pCVar1 = *(undefined4 *)(pCVar1 + ((int)param_1 - (int)this));
    pCVar1 = pCVar1 + 4;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  *(undefined4 *)(this + 0x8c) = *(undefined4 *)(param_1 + 0x8c);
  *(undefined4 *)(this + 0x90) = *(undefined4 *)(param_1 + 0x90);
  return this;
}


// 10006a10 FUN_10006a10

/* WARNING: Removing unreachable block (ram,0x10006a5c) */

undefined4 * __fastcall FUN_10006a10(undefined4 *param_1)

{
  code *pcVar1;
  undefined4 *puVar2;
  
  param_1[4] = 0;
  if (((uint)(param_1 + 6) & 7) != 0) {
    Error("CTSListBase: Misaligned list\n");
    pcVar1 = (code *)swi(3);
    puVar2 = (undefined4 *)(*pcVar1)();
    return puVar2;
  }
  param_1[6] = 0;
  param_1[7] = 0;
  if (((uint)param_1 & 7) == 0) {
    LOCK();
    param_1[4] = 0;
    UNLOCK();
    param_1[3] = 0;
    param_1[1] = 0;
    puVar2 = (undefined4 *)FUN_100099a0(8);
    param_1[2] = puVar2;
    *param_1 = puVar2;
    *puVar2 = param_1;
    return param_1;
  }
  Error("CTSQueue: Misaligned queue\n");
  pcVar1 = (code *)swi(3);
  puVar2 = (undefined4 *)(*pcVar1)();
  return puVar2;
}


// 10006ad0 FUN_10006ad0

undefined4 * __thiscall FUN_10006ad0(void *this,char *param_1,char *param_2)

{
  *(undefined4 *)((int)this + 8) = 1;
  *(undefined4 *)((int)this + 0x10) = 1;
  *(undefined ***)this = &PTR_LAB_1001d928;
  *(undefined ***)((int)this + 4) = &PTR_LAB_1001d91c;
  *(undefined4 *)((int)this + 0xc) = 4;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 0x18) = 0;
  *(undefined1 *)((int)this + 0x1c) = 0;
  *(undefined1 *)((int)this + 0x1d) = 0xff;
  *(undefined4 *)((int)this + 0x20) = 0xffffffff;
  *(undefined4 *)((int)this + 0x24) = 0;
  CThreadEvent::CThreadEvent((CThreadEvent *)((int)this + 0x28),true);
  *(undefined ***)this = &PTR_LAB_1001d954;
  *(undefined ***)((int)this + 4) = &PTR_LAB_1001d948;
  *(char **)((int)this + 0x30) = param_1;
  if (param_2 != (char *)0x0) {
    FUN_1000b3f0((char *)((int)this + 0x34),param_2,0x10);
    return (undefined4 *)this;
  }
  *(undefined1 *)((int)this + 0x34) = 0;
  return (undefined4 *)this;
}


// 10006b80 FUN_10006b80

int __thiscall FUN_10006b80(void *this,byte param_1)

{
  if (*(int **)((int)this + 0x30) != (int *)0x0) {
    (**(code **)(**(int **)((int)this + 0x30) + 4))();
  }
  CThreadSyncObject::~CThreadSyncObject((CThreadSyncObject *)((int)this + 0x28));
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (int)this;
}


// 10006bb0 FUN_10006bb0

void __fastcall FUN_10006bb0(int param_1)

{
  int *piVar1;
  CThreadFastMutex *this;
  DWORD DVar2;
  uint uVar3;
  int iVar4;
  int iStack_8;
  
  this = (CThreadFastMutex *)(param_1 + 0xbc);
  DVar2 = GetCurrentThreadId();
  if (DVar2 != *(DWORD *)this) {
    LOCK();
    iVar4 = *(int *)this;
    if (iVar4 == 0) {
      *(DWORD *)this = DVar2;
      iVar4 = 0;
    }
    UNLOCK();
    if (iVar4 != 0) {
      CThreadFastMutex::Lock(this,DVar2,0);
      goto LAB_10006bf1;
    }
  }
  *(int *)(param_1 + 0xc0) = *(int *)(param_1 + 0xc0) + 1;
LAB_10006bf1:
  if (*(int *)(param_1 + 0xc4) == 0) {
    iVar4 = 0;
    if (0 < *(int *)(param_1 + 0xa0)) {
      do {
        CWorkerThread::CallWorker
                  (*(CWorkerThread **)(*(int *)(param_1 + 0x94) + iVar4 * 4),1,0,true);
        iVar4 = iVar4 + 1;
      } while (iVar4 < *(int *)(param_1 + 0xa0));
    }
    iVar4 = 0;
    if (0 < *(int *)(param_1 + 0xa0)) {
      do {
        CWorkerThread::WaitForReply
                  (*(CWorkerThread **)(*(int *)(param_1 + 0x94) + iVar4 * 4),0xffffffff);
        iVar4 = iVar4 + 1;
      } while (iVar4 < *(int *)(param_1 + 0xa0));
    }
    iStack_8 = 0;
    if (0 < *(int *)(param_1 + 0xa0)) {
      do {
        iVar4 = iStack_8 * 4;
        uVar3 = CThread::Suspend(*(CThread **)(*(int *)(param_1 + 0x94) + iVar4));
        while (uVar3 == 0) {
          CThread::Resume(*(CThread **)(iVar4 + *(int *)(param_1 + 0x94)));
          ThreadSleep(0);
          uVar3 = CThread::Suspend(*(CThread **)(*(int *)(param_1 + 0x94) + iVar4));
        }
        CThread::Resume(*(CThread **)(iVar4 + *(int *)(param_1 + 0x94)));
        iStack_8 = iStack_8 + 1;
      } while (iStack_8 < *(int *)(param_1 + 0xa0));
    }
  }
  *(int *)(param_1 + 0xc4) = *(int *)(param_1 + 0xc4) + 1;
  piVar1 = (int *)(param_1 + 0xc0);
  *piVar1 = *piVar1 + -1;
  if (*piVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return;
}


// 10006fb0 FUN_10006fb0

int * __fastcall FUN_10006fb0(int *param_1)

{
  param_1[2] = 1;
  *param_1 = (int)&PTR_LAB_1001d928;
  param_1[1] = (int)&PTR_LAB_1001d91c;
  param_1[3] = 4;
  param_1[4] = 1;
  param_1[5] = 0;
  param_1[6] = 0;
  *(undefined1 *)((int)param_1 + 0x1d) = 0xff;
  param_1[8] = -1;
  *(undefined1 *)(param_1 + 7) = 0;
  param_1[9] = 0;
  CThreadEvent::CThreadEvent((CThreadEvent *)(param_1 + 10),true);
  *param_1 = (int)&PTR_LAB_1001d9a4;
  param_1[1] = (int)&PTR_LAB_1001d998;
  FUN_100067c0(param_1);
  return param_1;
}


// 100070c0 FUN_100070c0

int __thiscall FUN_100070c0(void *this,byte param_1)

{
  CThreadSyncObject::~CThreadSyncObject((CThreadSyncObject *)((int)this + 0x28));
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (int)this;
}


// 10007200 FUN_10007200

int * __thiscall FUN_10007200(void *this,int *param_1)

{
  undefined4 *puVar1;
  int iVar2;
  int *piVar3;
  void *pvVar4;
  
  *param_1 = (int)this;
  iVar2 = *(int *)((int)this + 0xc);
  piVar3 = *(int **)((int)this + 8);
  puVar1 = (undefined4 *)((int)this + 8);
  LOCK();
  pvVar4 = (void *)*piVar3;
  if (this == pvVar4) {
    *piVar3 = (int)param_1;
    pvVar4 = this;
  }
  UNLOCK();
  while (pvVar4 != this) {
    ThreadInterlockedAssignIf64(puVar1,*piVar3,iVar2 + 1,piVar3,iVar2);
    iVar2 = *(int *)((int)this + 0xc);
    piVar3 = (int *)*puVar1;
    LOCK();
    pvVar4 = (void *)*piVar3;
    if (this == pvVar4) {
      *piVar3 = (int)param_1;
      pvVar4 = this;
    }
    UNLOCK();
  }
  ThreadInterlockedAssignIf64(puVar1,param_1,iVar2 + 1,piVar3,iVar2);
  LOCK();
  *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
  UNLOCK();
  return piVar3;
}


// 10007290 FUN_10007290

int __fastcall FUN_10007290(int *param_1)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  int iVar4;
  char cVar5;
  
  do {
    do {
      while( true ) {
        do {
          do {
            iVar1 = param_1[1];
            piVar2 = (int *)*param_1;
            piVar3 = (int *)*piVar2;
          } while (piVar3 == (int *)0x0);
        } while (iVar1 != param_1[1]);
        if (piVar2 != (int *)param_1[2]) break;
        if (piVar3 == param_1) {
          return 0;
        }
        ThreadInterlockedAssignIf64(param_1 + 2,piVar3,param_1[3] + 1,piVar2,param_1[3]);
      }
    } while (piVar3 == param_1);
    iVar4 = piVar3[1];
    cVar5 = ThreadInterlockedAssignIf64(param_1,piVar3,iVar1 + 1,piVar2,iVar1);
  } while (cVar5 == '\0');
  LOCK();
  param_1[4] = param_1[4] + -1;
  UNLOCK();
  piVar2[1] = iVar4;
  return (int)piVar2;
}


// 10007370 FUN_10007370

void __thiscall FUN_10007370(void *this,int param_1)

{
  int iVar1;
  undefined4 uVar2;
  
  if (param_1 <= *(int *)((int)this + 4)) {
                    /* WARNING: Load size is inaccurate */
    *(undefined4 *)((int)this + 0x10) = *this;
    return;
  }
                    /* WARNING: Load size is inaccurate */
  iVar1 = *this;
  if (-1 < *(int *)((int)this + 8)) {
    *(int *)((int)this + 4) = param_1;
    if (iVar1 != 0) {
      uVar2 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(iVar1,param_1 * 4);
      *(undefined4 *)this = uVar2;
      *(undefined4 *)((int)this + 0x10) = uVar2;
      return;
    }
    uVar2 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1 * 4);
    *(undefined4 *)this = uVar2;
    *(undefined4 *)((int)this + 0x10) = uVar2;
    return;
  }
  *(int *)((int)this + 0x10) = iVar1;
  return;
}


// 10007420 FUN_10007420

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */
/* WARNING: Removing unreachable block (ram,0x100074d9) */
/* WARNING: Removing unreachable block (ram,0x100074e7) */

void __cdecl FUN_10007420(char param_1,char param_2,char param_3,undefined1 param_4)

{
  undefined8 uVar1;
  double dVar2;
  undefined4 *puVar3;
  char *pcVar4;
  undefined4 *puVar5;
  undefined1 *puVar6;
  CThreadSyncObject *this;
  int iVar7;
  bool bVar8;
  int iStack_32e40;
  int iStack_32e3c;
  undefined4 uStack_32e38;
  int iStack_32e34;
  uint uStack_32e30;
  int iStack_32e2c;
  int iStack_32e28;
  int iStack_32e24;
  undefined4 uStack_32e20;
  undefined4 uStack_32e1c;
  undefined1 auStack_32e18 [12];
  int aiStack_32e0c [6];
  undefined4 *puStack_32df4;
  undefined4 uStack_32df0;
  undefined4 *puStack_32dec;
  undefined4 *puStack_32de8;
  undefined4 *puStack_32de4;
  undefined1 *puStack_32de0;
  int *piStack_32ddc;
  int iStack_32dd8;
  undefined4 *puStack_32dd4;
  undefined4 uStack_32dd0;
  undefined4 uStack_32dcc;
  byte bStack_32cc8;
  undefined4 auStack_32cc0 [7];
  char acStack_32ca3 [11];
  undefined1 auStack_32c98 [208000];
  CThreadSyncObject aCStack_18 [20];
  
  puStack_32de4 = &uStack_32e38;
  puStack_32de8 = (undefined4 *)(uint)(param_1 != '\0');
  piStack_32ddc = &iStack_32e40;
  puStack_32dec = &uStack_32e20;
  iStack_32e2c = 0;
  puStack_32de0 = auStack_32e18;
  do {
    DAT_10029430 = -10;
    do {
      Msg("ThreadPoolTest:         Testing! Sleep %d, interleave %d \n",DAT_10029430,iStack_32e2c);
      iStack_32e24 = 1;
      do {
        LOCK();
        DAT_10029458 = 0;
        UNLOCK();
        bStack_32cc8 = bStack_32cc8 & 0xfc;
        DAT_10029444 = 0;
        uStack_32dd0 = 0xffffffff;
        uStack_32dcc = 0xffff8000;
        puStack_32dd4 = puStack_32de8;
        iStack_32dd8 = iStack_32e24;
        (**(code **)(*DAT_1002943c + 0xc))(&iStack_32dd8,&DAT_1001dad8);
        if (iStack_32e34 == 0) {
          (**(code **)(*DAT_1002943c + 0x24))();
        }
        iVar7 = 3999;
        puVar5 = auStack_32cc0;
        do {
          *puVar5 = 1;
          puVar5[-2] = &PTR_LAB_1001d928;
          puVar5[-1] = &PTR_LAB_1001d91c;
          puVar5[1] = 4;
          puVar5[2] = 1;
          puVar5[3] = 0;
          puVar5[4] = 0;
          *(undefined1 *)(puVar5 + 5) = 0;
          *(undefined1 *)((int)puVar5 + 0x15) = 0xff;
          puVar5[6] = 0xffffffff;
          puVar5[7] = 0;
          CThreadEvent::CThreadEvent((CThreadEvent *)(puVar5 + 8),true);
          puVar3 = puStack_32dec;
          puVar5[-2] = &PTR_LAB_1001d9cc;
          puVar5[-1] = &PTR_LAB_1001d9c0;
          puVar5 = puVar5 + 0xd;
          iVar7 = iVar7 + -1;
        } while (-1 < iVar7);
        DAT_10029440 = 4000;
        iStack_32e40 = 0;
        iStack_32e3c = 0;
        uVar1 = rdtsc();
        *puStack_32dec = (int)uVar1;
        puVar3[1] = (int)((ulonglong)uVar1 >> 0x20);
        puVar5 = puStack_32de4;
        uVar1 = rdtsc();
        *puStack_32de4 = (int)uVar1;
        puVar5[1] = (int)((ulonglong)uVar1 >> 0x20);
        uStack_32e30 = 0;
        puVar6 = auStack_32c98;
        do {
          *puVar6 = param_4;
          puVar6[-0x14] = 8;
          (**(code **)(*DAT_1002943c + 0x38))(puVar6 + -0x30);
          if ((param_2 != '\0') && ((uStack_32e30 & 0xf) == 0)) {
            ThreadSleep(0);
          }
          uStack_32e30 = uStack_32e30 + 1;
          puVar6 = puVar6 + 0x34;
        } while (uStack_32e30 < 4000);
        if (iStack_32e34 == 0) {
          (**(code **)(*DAT_1002943c + 0x28))();
        }
        if ((param_3 != '\0') && (DAT_10029430 < 2)) {
          CThreadEvent::Wait((CThreadEvent *)&DAT_10029530,0xffffffff);
        }
        DAT_10029444 = DAT_10029458;
        uStack_32e20 = 0;
        uStack_32e1c = 0;
        uVar1 = rdtsc();
        *puStack_32de8 = (int)uVar1;
        puStack_32de8[1] = (int)((ulonglong)uVar1 >> 0x20);
        (**(code **)(*DAT_1002943c + 0x24))();
        iStack_32e28 = 0;
        iStack_32e24 = 0;
        uVar1 = rdtsc();
        *puStack_32df4 = (int)uVar1;
        puStack_32df4[1] = (int)((ulonglong)uVar1 >> 0x20);
        bVar8 = iStack_32e40 != 0;
        iStack_32e40 = -iStack_32e40;
        iStack_32e3c = -(uint)bVar8 - iStack_32e3c;
        (**(code **)(*DAT_1002943c + 0x28))();
        (**(code **)(*DAT_1002943c + 0x14))(0xffffffff);
        CThreadEvent::Reset((CThreadEvent *)&DAT_10029530);
        aiStack_32e0c[1] = 0;
        aiStack_32e0c[2] = 0;
        aiStack_32e0c[3] = 0;
        aiStack_32e0c[4] = 0;
        aiStack_32e0c[5] = 0;
        puStack_32df4 = (undefined4 *)0x0;
        uStack_32df0 = 0;
        aiStack_32e0c[0] = 0;
        pcVar4 = acStack_32ca3;
        iVar7 = 4000;
        do {
          if (*pcVar4 != -1) {
            aiStack_32e0c[*pcVar4] = aiStack_32e0c[*pcVar4] + 1;
            *pcVar4 = -1;
          }
          pcVar4 = pcVar4 + 0x34;
          iVar7 = iVar7 + -1;
        } while (iVar7 != 0);
        iStack_32e28 = DAT_10029458;
        dVar2 = *(double *)g_ClockSpeedMillisecondsMultiplier_exref *
                (double)CONCAT44(iStack_32e3c,iStack_32e40);
        Msg("ThreadPoolTest:         %d threads -- %d (%d) jobs processed in %fms, %fms to suspend (%f/%f) [%d, %d, %d, %d, %d, %d, %d, %d]\n"
            ,iStack_32e24,DAT_10029444,DAT_10029458,dVar2,
            *(double *)g_ClockSpeedMillisecondsMultiplier_exref *
            (double)CONCAT44(iStack_32e34,uStack_32e38) - dVar2,
            (1.0 / (double)DAT_10029458) *
            *(double *)g_ClockSpeedMillisecondsMultiplier_exref *
            (double)CONCAT44(iStack_32e3c,iStack_32e40),
            (1.0 / (double)DAT_10029444) *
            *(double *)g_ClockSpeedMillisecondsMultiplier_exref *
            (double)CONCAT44(iStack_32e34,uStack_32e38),aiStack_32e0c[0],aiStack_32e0c[1],
            aiStack_32e0c[2],aiStack_32e0c[3],aiStack_32e0c[4],aiStack_32e0c[5],puStack_32df4,
            uStack_32df0);
        iVar7 = 3999;
        this = aCStack_18;
        do {
          this = this + -0x34;
          CThreadSyncObject::~CThreadSyncObject(this);
          iVar7 = iVar7 + -1;
        } while (-1 < iVar7);
        iStack_32e24 = iStack_32e24 + 2;
      } while (iStack_32e24 < 9);
      DAT_10029430 = DAT_10029430 + 10;
    } while (DAT_10029430 < 0xb);
    iStack_32e2c = iStack_32e2c + 1;
  } while (iStack_32e2c < 2);
  return;
}


// 10007880 FUN_10007880

/* WARNING: Removing unreachable block (ram,0x100078e3) */
/* WARNING: Removing unreachable block (ram,0x100078ee) */
/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10007880(void)

{
  int *piVar1;
  int *piVar2;
  int unaff_EBP;
  undefined4 *puVar3;
  int iVar4;
  int *piVar5;
  int iStack_124;
  int iStack_120;
  int iStack_11c;
  int aiStack_114 [68];
  byte bStack_4;
  
  Msg("TestForcedExecute\n");
  iStack_11c = 0x1e;
  piVar2 = (int *)0x0;
  do {
    iStack_124 = 1;
    do {
      LOCK();
      DAT_10029454 = 0;
      UNLOCK();
      bStack_4 = bStack_4 & 0xfc;
      aiStack_114[2] = 0xffffffff;
      aiStack_114[3] = 0xffff8000;
      aiStack_114[0] = iStack_124;
      aiStack_114[1] = 1;
      (**(code **)(*DAT_1002943c + 0xc))(aiStack_114,&DAT_1001dad8);
      if ((_DAT_10058370 & 1) == 0) {
        _DAT_10058370 = _DAT_10058370 | 1;
        iVar4 = 3999;
        puVar3 = &DAT_10029578;
        do {
          *puVar3 = 1;
          puVar3[-2] = &PTR_LAB_1001d928;
          puVar3[-1] = &PTR_LAB_1001d91c;
          puVar3[1] = 4;
          puVar3[2] = 1;
          puVar3[3] = piVar2;
          puVar3[4] = piVar2;
          *(char *)(puVar3 + 5) = (char)piVar2;
          *(undefined1 *)((int)puVar3 + 0x15) = 0xff;
          puVar3[6] = 0xffffffff;
          puVar3[7] = piVar2;
          CThreadEvent::CThreadEvent((CThreadEvent *)(puVar3 + 8),true);
          puVar3[-2] = &PTR_LAB_1001d9f4;
          puVar3[-1] = &PTR_LAB_1001d9e8;
          puVar3 = puVar3 + 0xc;
          iVar4 = iVar4 + -1;
        } while (-1 < iVar4);
        _atexit((_func_4879 *)&LAB_1001c410);
      }
      piVar5 = &DAT_10029570;
      iVar4 = 4000;
      do {
        DAT_10029438 = piVar2;
        iStack_120 = unaff_EBP;
        if ((int)piVar2 < unaff_EBP) {
          do {
            piVar1 = (int *)FUN_100099a0(0x34);
            if (piVar1 == piVar2) {
              piVar1 = (int *)0x0;
            }
            else {
              piVar1[2] = 1;
              *piVar1 = (int)&PTR_LAB_1001d928;
              piVar1[1] = (int)&PTR_LAB_1001d91c;
              piVar1[3] = 4;
              piVar1[4] = 1;
              piVar1[5] = (int)piVar2;
              piVar1[6] = (int)piVar2;
              *(char *)(piVar1 + 7) = (char)piVar2;
              *(undefined1 *)((int)piVar1 + 0x1d) = 0xff;
              piVar1[8] = -1;
              piVar1[9] = (int)piVar2;
              CThreadEvent::CThreadEvent((CThreadEvent *)(piVar1 + 10),true);
              *piVar1 = (int)&PTR_LAB_1001da3c;
              piVar1[1] = (int)&PTR_LAB_1001da30;
            }
            *(undefined1 *)(piVar1 + 7) = 8;
            piVar1[0xc] = (int)piVar5;
            (**(code **)(*DAT_1002943c + 0x38))(piVar1);
            (**(code **)(*piVar1 + 4))();
            iStack_120 = iStack_120 + -1;
          } while (iStack_120 != 0);
        }
        do {
        } while (DAT_10029454 < unaff_EBP);
        DAT_10029438 = (int *)0x1;
        ThreadSleep(piVar2);
        piVar2 = (int *)FUN_100067c0(piVar5);
        do {
        } while (0 < DAT_10029454);
        piVar5 = piVar5 + 0xc;
        iVar4 = iVar4 + -1;
      } while (iVar4 != 0);
      (**(code **)(*DAT_1002943c + 0x14))(0xffffffff);
      iStack_124 = unaff_EBP + 2;
    } while (iStack_124 < 6);
    iStack_11c = iStack_11c + -1;
  } while (iStack_11c != 0);
  Msg("TestForcedExecute DONE\n");
  return;
}


// 10007ad0 FUN_10007ad0

void __fastcall FUN_10007ad0(int *param_1)

{
  int *piVar1;
  undefined4 *puVar2;
  char cVar3;
  int iVar4;
  
  iVar4 = FUN_10007290(param_1);
  while (iVar4 != 0) {
    FUN_10009a00(iVar4);
    iVar4 = FUN_10007290(param_1);
  }
  piVar1 = param_1 + 6;
  do {
    puVar2 = (undefined4 *)*piVar1;
    iVar4 = param_1[7];
    while( true ) {
      if (puVar2 == (undefined4 *)0x0) {
        param_1[1] = 0;
        param_1[3] = 0;
        return;
      }
      cVar3 = ThreadInterlockedAssignIf64(piVar1,*puVar2,iVar4 + -1,puVar2,iVar4);
      if (cVar3 != '\0') break;
      puVar2 = (undefined4 *)*piVar1;
      iVar4 = param_1[7];
    }
    FUN_10009a00(puVar2);
  } while( true );
}


// 10007b50 FUN_10007b50

void __fastcall FUN_10007b50(int *param_1)

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


// 10007bc0 FUN_10007bc0

uint __thiscall FUN_10007bc0(void *this,undefined4 *param_1)

{
  CThreadFastMutex *this_00;
  uint uVar1;
  DWORD DVar2;
  int iVar3;
  undefined4 *puVar4;
  undefined4 uVar5;
  int *piVar6;
  
  this_00 = (CThreadFastMutex *)((int)this + 0x68);
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
      goto LAB_10007bf7;
    }
  }
  *(int *)((int)this + 0x6c) = *(int *)((int)this + 0x6c) + 1;
LAB_10007bf7:
  if (*(int *)((int)this + 0x60) == 0) {
    piVar6 = (int *)((int)this + 0x6c);
    *piVar6 = *piVar6 + -1;
    uVar1 = *(uint *)((int)this + 0x6c);
    if (*piVar6 == 0) {
      LOCK();
      *(undefined4 *)this_00 = 0;
      UNLOCK();
    }
    *param_1 = 0;
    return uVar1 & 0xffffff00;
  }
  iVar3 = *(int *)((int)this + 0x60) + -1;
  *(int *)((int)this + 0x60) = iVar3;
  if (iVar3 == 0) {
    CThreadEvent::Reset((CThreadEvent *)((int)this + 0x70));
  }
  piVar6 = (int *)((int)this + 0x6c);
  *piVar6 = *piVar6 + -1;
  if (*piVar6 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  iVar3 = 2;
  piVar6 = (int *)((int)this + 0x40);
  do {
    puVar4 = (undefined4 *)FUN_10007290(piVar6);
    if (puVar4 != (undefined4 *)0x0) {
      *param_1 = puVar4[1];
      uVar5 = FUN_10006560(piVar6 + 6,puVar4);
      return CONCAT31((int3)((uint)uVar5 >> 8),1);
    }
    iVar3 = iVar3 + -1;
    piVar6 = piVar6 + -8;
  } while (-1 < iVar3);
  *param_1 = 0;
  return 0;
}


// 10007c90 FUN_10007c90

undefined4 __fastcall FUN_10007c90(int param_1)

{
  CThreadFastMutex *pCVar1;
  int *piVar2;
  DWORD DVar3;
  undefined4 *puVar4;
  int iVar5;
  CThreadFastMutex *this;
  int *piVar6;
  
  this = (CThreadFastMutex *)(param_1 + 0x68);
  DVar3 = GetCurrentThreadId();
  if (DVar3 != *(DWORD *)this) {
    LOCK();
    iVar5 = *(int *)this;
    if (iVar5 == 0) {
      *(DWORD *)this = DVar3;
      iVar5 = 0;
    }
    UNLOCK();
    if (iVar5 != 0) {
      CThreadFastMutex::Lock(this,DVar3,0);
      goto LAB_10007cc8;
    }
  }
  *(int *)(param_1 + 0x6c) = *(int *)(param_1 + 0x6c) + 1;
LAB_10007cc8:
  *(undefined4 *)(param_1 + 0x60) = 0;
  CThreadEvent::Reset((CThreadEvent *)(param_1 + 0x70));
  iVar5 = 2;
  piVar6 = (int *)(param_1 + 0x40);
  do {
    while (puVar4 = (undefined4 *)FUN_10007290(piVar6), puVar4 != (undefined4 *)0x0) {
      piVar2 = (int *)puVar4[1];
      FUN_10006560(piVar6 + 6,puVar4);
      this = (CThreadFastMutex *)FUN_10006880(piVar2,1);
      (**(code **)(*piVar2 + 4))();
    }
    iVar5 = iVar5 + -1;
    piVar6 = piVar6 + -8;
  } while (-1 < iVar5);
  pCVar1 = this + 4;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this = 0;
    UNLOCK();
  }
  return *(undefined4 *)(this + 4);
}


// 10007e20 FUN_10007e20

int __fastcall FUN_10007e20(int *param_1)

{
  int *piVar1;
  int iVar2;
  char cVar3;
  undefined4 uVar4;
  int iVar5;
  void *this;
  int *piStack_8;
  int iStack_4;
  
  (**(code **)(*param_1 + 0x24))();
  iVar5 = 0;
  uVar4 = FUN_10007bc0(param_1 + 4,&piStack_8);
  cVar3 = (char)uVar4;
  piVar1 = piStack_8;
  while (piStack_8 = piVar1, cVar3 != '\0') {
    iVar5 = FUN_10006880(piVar1,1);
    (**(code **)(*piVar1 + 4))();
    iVar5 = iVar5 + 1;
    uVar4 = FUN_10007bc0(param_1 + 4,&piStack_8);
    piVar1 = piStack_8;
    cVar3 = (char)uVar4;
  }
  iStack_4 = 0;
  if (0 < param_1[0x28]) {
    do {
      iVar2 = iStack_4;
      this = (void *)(*(int *)(param_1[0x25] + iStack_4 * 4) + 0x80);
      uVar4 = FUN_10007bc0(this,&piStack_8);
      cVar3 = (char)uVar4;
      piVar1 = piStack_8;
      while (piStack_8 = piVar1, cVar3 != '\0') {
        iVar5 = FUN_10006880(piVar1,1);
        (**(code **)(*piVar1 + 4))();
        iVar5 = iVar5 + 1;
        uVar4 = FUN_10007bc0(this,&piStack_8);
        piVar1 = piStack_8;
        iVar2 = iStack_4;
        cVar3 = (char)uVar4;
      }
      iStack_4 = iVar2 + 1;
    } while (iStack_4 < param_1[0x28]);
  }
  LOCK();
  param_1[0x32] = 0;
  UNLOCK();
  (**(code **)(*param_1 + 0x28))();
  return iVar5;
}


// 10007f00 FUN_10007f00

undefined4 __fastcall FUN_10007f00(int param_1)

{
  void *pvVar1;
  int iVar2;
  
  iVar2 = 0;
  if (0 < *(int *)(param_1 + 0xa0)) {
    do {
      CWorkerThread::CallWorker
                (*(CWorkerThread **)(*(int *)(param_1 + 0x94) + iVar2 * 4),0,0xffffffff,true);
      iVar2 = iVar2 + 1;
    } while (iVar2 < *(int *)(param_1 + 0xa0));
  }
  iVar2 = 0;
  if (0 < *(int *)(param_1 + 0xa0)) {
    do {
      pvVar1 = CThread::GetThreadHandle(*(CThread **)(*(int *)(param_1 + 0x94) + iVar2 * 4));
      while (pvVar1 != (void *)0x0) {
        ThreadSleep(0);
        pvVar1 = CThread::GetThreadHandle(*(CThread **)(*(int *)(param_1 + 0x94) + iVar2 * 4));
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < *(int *)(param_1 + 0xa0));
  }
  LOCK();
  *(undefined4 *)(param_1 + 200) = 0;
  UNLOCK();
  FUN_10007c90(param_1 + 0x10);
  LOCK();
  *(undefined4 *)(param_1 + 0x90) = 0;
  UNLOCK();
  *(undefined4 *)(param_1 + 0xa0) = 0;
  *(undefined4 *)(param_1 + 0xb4) = 0;
  return 1;
}


// 10007fd0 FUN_10007fd0

undefined4 * __fastcall FUN_10007fd0(undefined4 *param_1)

{
  int iVar1;
  undefined4 *puVar2;
  
  iVar1 = 2;
  puVar2 = param_1;
  do {
    FUN_10006a10(puVar2);
    puVar2 = puVar2 + 8;
    iVar1 = iVar1 + -1;
  } while (-1 < iVar1);
  param_1[0x18] = 0;
  param_1[0x19] = 0x7fffffff;
  param_1[0x1a] = 0;
  param_1[0x1b] = 0;
  CThreadEvent::CThreadEvent((CThreadEvent *)(param_1 + 0x1c),true);
  return param_1;
}


// 10008020 FUN_10008020

int __thiscall FUN_10008020(void *this,int *param_1)

{
  int *piVar1;
  CThreadFastMutex *this_00;
  int *piVar2;
  undefined4 uVar3;
  int iVar4;
  int iVar5;
  int *piVar6;
  uint uVar7;
  code *pcVar8;
  void *this_01;
  void *pvStack_4;
  
  piVar2 = param_1;
  pvStack_4 = this;
  (**(code **)*param_1)();
  param_1 = (int *)0x0;
  pcVar8 = GetCurrentThreadId_exref;
  while (((*(int *)((int)this + 100) <= *(int *)((int)this + 0x60) &&
          (uVar3 = FUN_10007bc0(this,&pvStack_4), (char)uVar3 != '\0')) &&
         (pvStack_4 != (void *)0x0))) {
    iVar4 = piVar2[3];
    if (((iVar4 == 1) || (iVar4 == 2)) || (iVar4 == 4)) {
      iVar4 = (*pcVar8)();
      piVar6 = piVar2 + 5;
      if (iVar4 != piVar2[5]) {
        LOCK();
        iVar5 = *piVar6;
        if (iVar5 == 0) {
          *piVar6 = iVar4;
          iVar5 = 0;
        }
        UNLOCK();
        if (iVar5 != 0) goto LAB_100080a8;
      }
      piVar2[6] = piVar2[6] + 1;
      *(undefined1 *)((int)piVar2 + 0x1d) = 0xff;
      pcVar8 = (code *)FUN_100067c0(piVar2);
      piVar1 = piVar2 + 6;
      *piVar1 = *piVar1 + -1;
      if (*piVar1 == 0) {
        LOCK();
        *piVar6 = 0;
        UNLOCK();
      }
    }
LAB_100080a8:
    (**(code **)(*piVar2 + 4))();
    param_1 = (int *)((int)param_1 + 1);
  }
  this_01 = (void *)(piVar2[4] * 0x20 + (int)this);
  piVar6 = FUN_100065c0((undefined4 *)((int)this_01 + 0x18));
  if (piVar6 == (int *)0x0) {
    piVar6 = (int *)FUN_100099a0(8);
    if (piVar6 == (int *)0x0) {
      piVar6 = (int *)0x0;
    }
    else {
      piVar6[1] = (int)piVar2;
    }
  }
  else {
    piVar6[1] = (int)piVar2;
  }
  FUN_10007200(this_01,piVar6);
  this_00 = (CThreadFastMutex *)((int)this + 0x68);
  uVar7 = (*pcVar8)();
  if (uVar7 != *(uint *)this_00) {
    LOCK();
    iVar4 = *(int *)this_00;
    if (iVar4 == 0) {
      *(uint *)this_00 = uVar7;
      iVar4 = 0;
    }
    UNLOCK();
    if (iVar4 != 0) {
      CThreadFastMutex::Lock(this_00,uVar7,0);
      goto LAB_10008125;
    }
  }
  *(int *)((int)this + 0x6c) = *(int *)((int)this + 0x6c) + 1;
LAB_10008125:
  *(int *)((int)this + 0x60) = *(int *)((int)this + 0x60) + 1;
  if (*(int *)((int)this + 0x60) == 1) {
    CThreadEvent::Set((CThreadEvent *)((int)this + 0x70));
  }
  piVar2 = (int *)((int)this + 0x6c);
  *piVar2 = *piVar2 + -1;
  if (*piVar2 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return (int)param_1;
}


// 10008150 FUN_10008150

void __fastcall FUN_10008150(int param_1)

{
  int *piVar1;
  int iVar3;
  int *piVar2;
  
  CThreadSyncObject::~CThreadSyncObject((CThreadSyncObject *)(param_1 + 0x70));
  iVar3 = 2;
  piVar1 = (int *)(param_1 + 0x60);
  do {
    piVar2 = piVar1 + -8;
    FUN_10007ad0(piVar2);
    FUN_10009a00(*piVar2);
    FUN_10006600(piVar1 + -2);
    iVar3 = iVar3 + -1;
    piVar1 = piVar2;
  } while (-1 < iVar3);
  return;
}


// 10008190 FUN_10008190

undefined4 * __fastcall FUN_10008190(undefined4 *param_1)

{
  param_1[2] = 1;
  *param_1 = &PTR_LAB_1001db54;
  param_1[1] = &PTR_LAB_1001db48;
  FUN_10007fd0(param_1 + 4);
  param_1[0x24] = 0;
  param_1[0x25] = 0;
  param_1[0x26] = 0;
  param_1[0x27] = 0;
  param_1[0x28] = 0;
  param_1[0x29] = 0;
  param_1[0x2a] = 0;
  param_1[0x2b] = 0;
  param_1[0x2c] = 0;
  param_1[0x2d] = 0;
  param_1[0x2e] = 0;
  param_1[0x2f] = 0;
  param_1[0x30] = 0;
  param_1[0x31] = 0;
  param_1[0x32] = 0;
  return param_1;
}


// 10008250 FUN_10008250

int __fastcall FUN_10008250(int param_1)

{
  int *piVar1;
  char cVar2;
  int iVar3;
  
  piVar1 = (int *)(param_1 + 8);
  LOCK();
  iVar3 = *piVar1;
  *piVar1 = *piVar1 + -1;
  UNLOCK();
  iVar3 = iVar3 + -1;
  if (iVar3 == 0) {
    cVar2 = (**(code **)(*(int *)(param_1 + 4) + 4))();
    if (cVar2 != '\0') {
      (*(code *)**(undefined4 **)(param_1 + 4))(1);
    }
    iVar3 = 0;
  }
  return iVar3;
}


// 10008280 FUN_10008280

int __thiscall FUN_10008280(void *this,int *param_1)

{
  int iVar1;
  int iVar2;
  void *this_00;
  int *piVar3;
  
  if ((*(byte *)(param_1 + 7) & 4) == 0) {
    iVar1 = (int)*(char *)((int)param_1 + 0x1d);
    if (((iVar1 == -1) || (iVar1 < 0)) || (*(int *)((int)this + 0xa0) <= iVar1)) {
      this_00 = (void *)((int)this + 0x10);
      goto LAB_100082c0;
    }
    iVar1 = *(int *)(*(int *)((int)this + 0x94) + iVar1 * 4);
  }
  else {
    iVar1 = **(int **)((int)this + 0x94);
  }
  this_00 = (void *)(iVar1 + 0x80);
LAB_100082c0:
  iVar2 = FUN_10008020(this_00,param_1);
  piVar3 = (int *)((int)this + 200);
  LOCK();
  iVar1 = *piVar3;
  *piVar3 = *piVar3 - iVar2;
  UNLOCK();
  return iVar1;
}


// 10008330 FUN_10008330

int __thiscall FUN_10008330(void *this,int *param_1,undefined *param_2)

{
  DWORD *pDVar1;
  int *piVar2;
  void *pvVar3;
  undefined4 *puVar4;
  char cVar5;
  int iVar6;
  int iVar7;
  DWORD DVar8;
  DWORD DVar9;
  int *piVar10;
  int *piVar11;
  int iVar12;
  int iVar13;
  void *pvVar14;
  int iStack_40;
  int iStack_30;
  int iStack_28;
  int iStack_14;
  int iStack_10;
  int iStack_c;
  int iStack_8;
  int iStack_4;
  
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0x24))();
                    /* WARNING: Load size is inaccurate */
  iStack_40 = 0;
  iVar6 = (**(code **)(*this + 0x18))();
  iStack_14 = 0;
  iStack_10 = 0;
  iStack_c = 0;
  iStack_8 = 0;
  iStack_4 = 0;
  if ((int)param_1 < 3) {
    piVar10 = (int *)((int)this + 0x60);
    iStack_28 = 3 - (int)param_1;
    do {
      iStack_30 = 0;
      if (0 < *(int *)((int)this + 0xa0)) {
        do {
          iVar12 = *(int *)(*(int *)((int)this + 0x94) + iStack_30 * 4);
          pvVar14 = (void *)(iVar12 + 0x80);
          iVar12 = *(int *)((int)piVar10 + iVar12 + (0x70 - (int)this));
          while (iVar12 != 0) {
            FUN_10007bc0(pvVar14,&param_1);
            piVar2 = param_1;
            if ((param_2 == (undefined *)0x0) ||
               (cVar5 = (*(code *)param_2)(param_1), cVar5 != '\0')) {
              iVar12 = piVar2[3];
              if ((iVar12 == 1) || ((iVar12 == 2 || (piVar11 = piVar2, iVar12 == 4)))) {
                DVar8 = GetCurrentThreadId();
                pDVar1 = (DWORD *)(piVar2 + 5);
                if (DVar8 != piVar2[5]) {
                  LOCK();
                  DVar9 = *pDVar1;
                  if (DVar9 == 0) {
                    *pDVar1 = DVar8;
                    DVar9 = 0;
                  }
                  UNLOCK();
                  piVar11 = param_1;
                  if (DVar9 != 0) goto LAB_1000856a;
                }
                piVar2[6] = piVar2[6] + 1;
                *(undefined1 *)((int)param_1 + 0x1d) = 0xff;
                FUN_100067c0(param_1);
                piVar2 = piVar2 + 6;
                *piVar2 = *piVar2 + -1;
                piVar11 = param_1;
                if (*piVar2 == 0) {
                  LOCK();
                  *pDVar1 = 0;
                  UNLOCK();
                }
              }
LAB_1000856a:
              (**(code **)(*piVar11 + 4))();
              LOCK();
              *(int *)((int)this + 200) = *(int *)((int)this + 200) + -1;
              UNLOCK();
              iStack_40 = iStack_40 + 1;
            }
            else if ((piVar2[3] == 1) || (piVar2[3] == 4)) {
              iVar12 = iStack_10;
              if ((iStack_10 < iVar6) && (-1 < iStack_c)) {
                iVar12 = iVar6;
                if (iStack_14 == 0) {
                  iStack_10 = iVar6;
                  iStack_14 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar6 * 4);
                }
                else {
                  iStack_10 = iVar6;
                  iStack_14 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(iStack_14);
                }
              }
              iVar13 = iStack_8;
              if (iVar12 < iStack_8 + 1) {
                iStack_4 = iStack_14;
                FUN_10006730(&iStack_14,(iStack_8 - iVar12) + 1);
              }
              iVar12 = iStack_14;
              iStack_8 = iStack_8 + 1;
              iVar7 = (iStack_8 - iVar13) + -1;
              iStack_4 = iStack_14;
              if (0 < iVar7) {
                pvVar3 = (void *)(iStack_14 + iVar13 * 4);
                FUN_1000b3c0((void *)((int)pvVar3 + 4),pvVar3,iVar7 * 4);
              }
              puVar4 = (undefined4 *)(iVar12 + iVar13 * 4);
              if (puVar4 != (undefined4 *)0x0) {
                *puVar4 = piVar2;
              }
            }
            else {
              LOCK();
              *(int *)((int)this + 200) = *(int *)((int)this + 200) + -1;
              UNLOCK();
              (**(code **)(*piVar2 + 4))();
            }
            iVar12 = *(int *)((int)piVar10 + (-0x10 - (int)this) + (int)pvVar14);
          }
          iStack_30 = iStack_30 + 1;
        } while (iStack_30 < *(int *)((int)this + 0xa0));
      }
      iVar12 = *piVar10;
      while (iVar12 != 0) {
        FUN_10007bc0((void *)((int)this + 0x10),&param_1);
        piVar2 = param_1;
        if ((param_2 == (undefined *)0x0) || (cVar5 = (*(code *)param_2)(param_1), cVar5 != '\0')) {
          iVar12 = piVar2[3];
          iVar13 = 1;
          if ((iVar12 == 1) || ((iVar12 == 2 || (piVar11 = piVar2, iVar12 == 4)))) {
            DVar8 = GetCurrentThreadId();
            pDVar1 = (DWORD *)(piVar2 + 5);
            if (DVar8 != piVar2[5]) {
              LOCK();
              DVar9 = *pDVar1;
              if (DVar9 == 0) {
                *pDVar1 = DVar8;
                DVar9 = 0;
              }
              UNLOCK();
              piVar11 = param_1;
              if (DVar9 != 0) goto LAB_10008724;
            }
            piVar2[6] = piVar2[6] + 1;
            *(undefined1 *)((int)param_1 + 0x1d) = 0xff;
            iVar13 = FUN_100067c0(param_1);
            piVar2 = piVar2 + 6;
            *piVar2 = *piVar2 + -1;
            piVar11 = param_1;
            if (*piVar2 == 0) {
              LOCK();
              *pDVar1 = 0;
              UNLOCK();
            }
          }
LAB_10008724:
          (**(code **)(*piVar11 + 4))();
          LOCK();
          *(int *)((int)this + 200) = *(int *)((int)this + 200) + -1;
          UNLOCK();
          iStack_40 = iStack_40 + iVar13;
        }
        else if ((piVar2[3] == 1) || (piVar2[3] == 4)) {
          iVar12 = iStack_10;
          if ((iStack_10 < iVar6) && (-1 < iStack_c)) {
            iVar12 = iVar6;
            if (iStack_14 == 0) {
              iStack_10 = iVar6;
              iStack_14 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar6 * 4);
            }
            else {
              iStack_10 = iVar6;
              iStack_14 = (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(iStack_14);
            }
          }
          iVar13 = iStack_8;
          if (iVar12 < iStack_8 + 1) {
            iStack_4 = iStack_14;
            FUN_10006730(&iStack_14,(iStack_8 - iVar12) + 1);
          }
          iVar12 = iStack_14;
          iStack_8 = iStack_8 + 1;
          iVar7 = (iStack_8 - iVar13) + -1;
          iStack_4 = iStack_14;
          if (0 < iVar7) {
            pvVar14 = (void *)(iStack_14 + iVar13 * 4);
            FUN_1000b3c0((void *)((int)pvVar14 + 4),pvVar14,iVar7 * 4);
          }
          puVar4 = (undefined4 *)(iVar12 + iVar13 * 4);
          if (puVar4 != (undefined4 *)0x0) {
            *puVar4 = piVar2;
          }
        }
        else {
          LOCK();
          *(int *)((int)this + 200) = *(int *)((int)this + 200) + -1;
          UNLOCK();
          (**(code **)(*piVar2 + 4))();
        }
        iVar12 = *piVar10;
      }
      piVar10 = piVar10 + -8;
      iStack_28 = iStack_28 + -1;
    } while (iStack_28 != 0);
  }
  iVar12 = iStack_8;
  iVar6 = iStack_14;
  iVar13 = 0;
  if (0 < iStack_8) {
    do {
      piVar10 = *(int **)(iVar6 + iVar13 * 4);
      if ((*(byte *)(piVar10 + 7) & 4) == 0) {
        iVar7 = (int)*(char *)((int)piVar10 + 0x1d);
        if (((iVar7 != -1) && (-1 < iVar7)) && (iVar7 < *(int *)((int)this + 0xa0))) {
          iVar7 = *(int *)(*(int *)((int)this + 0x94) + iVar7 * 4);
          goto LAB_100087a6;
        }
        pvVar14 = (void *)((int)this + 0x10);
      }
      else {
        iVar7 = **(int **)((int)this + 0x94);
LAB_100087a6:
        pvVar14 = (void *)(iVar7 + 0x80);
      }
      iVar7 = FUN_10008020(pvVar14,piVar10);
      LOCK();
      *(int *)((int)this + 200) = *(int *)((int)this + 200) - iVar7;
      UNLOCK();
      (**(code **)(**(int **)(iVar6 + iVar13 * 4) + 4))();
      iVar13 = iVar13 + 1;
    } while (iVar13 < iVar12);
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0x28))();
  if ((-1 < iStack_c) && (iVar6 != 0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar6);
  }
  return iStack_40;
}


// 10008810 RunThreadPoolTests

void RunThreadPoolTests(void)

{
  HANDLE pvVar1;
  uint uVar2;
  int iVar3;
  int *piVar4;
  bool bVar6;
  char cVar7;
  ULONG_PTR *pUVar8;
  ULONG_PTR *pUVar9;
  ULONG_PTR UStack_f0;
  uint uStack_ec;
  ULONG_PTR UStack_e8;
  bool bStack_e4;
  undefined3 uStack_e3;
  undefined **local_e0;
  undefined **ppuStack_dc;
  int aiStack_90 [8];
  int aiStack_70 [4];
  CThreadSyncObject aCStack_60 [20];
  int iStack_4c;
  undefined4 uStack_48;
  int iStack_44;
  undefined4 uStack_40;
  int iStack_3c;
  int iStack_38;
  undefined4 uStack_34;
  int iStack_30;
  undefined4 uStack_2c;
  int iStack_28;
  int *piVar5;
  
                    /* 0x8810  25  RunThreadPoolTests */
  FUN_10008190(&local_e0);
  DAT_1002943c = &local_e0;
  RunTSQueueTests(10000,1);
  RunTSListTests(10000,1);
  pUVar9 = &UStack_e8;
  pUVar8 = &UStack_f0;
  UStack_f0 = 0xffffffff;
  UStack_e8 = 0xffffffff;
  pvVar1 = GetCurrentProcess();
  GetProcessAffinityMask(pvVar1,pUVar8,pUVar9);
  Msg("ThreadPoolTest: Job distribution speed\n");
  uStack_ec = 0;
  do {
    uVar2 = uStack_ec & 0x80000001;
    bVar6 = uVar2 == 0;
    if ((int)uVar2 < 0) {
      bVar6 = (uVar2 - 1 | 0xfffffffe) == 0xffffffff;
    }
    _bStack_e4 = CONCAT31(uStack_e3,!bVar6);
    Msg("ThreadPoolTest:     Non-distribute\n");
    cVar7 = (char)_bStack_e4;
    FUN_10007420('\0','\x01',cVar7,0);
    Msg("ThreadPoolTest:     Distribute\n");
    FUN_10007420('\x01','\x01',cVar7,0);
    Msg("ThreadPoolTest:     One core\n");
    ThreadSetAffinity(0,1);
    FUN_10007420('\0','\x01',cVar7,0);
    ThreadSetAffinity(0,UStack_f0);
    Msg("ThreadPoolTest:     NO Sleep\n");
    FUN_10007420('\0','\0',cVar7,0);
    Msg("ThreadPoolTest:     Distribute\n");
    FUN_10007420('\x01','\0',cVar7,0);
    Msg("ThreadPoolTest:     One core\n");
    ThreadSetAffinity(0,1);
    FUN_10007420('\0','\0',cVar7,0);
    ThreadSetAffinity(0,UStack_f0);
    uStack_ec = uStack_ec + 1;
  } while ((int)uStack_ec < 2);
  Msg("ThreadPoolTest: Jobs doing work\n");
  iVar3 = 2;
  do {
    Msg("ThreadPoolTest:     Non-distribute\n");
    FUN_10007420('\0','\x01','\x01',1);
    Msg("ThreadPoolTest:     Distribute\n");
    FUN_10007420('\x01','\x01','\x01',1);
    Msg("ThreadPoolTest:     One core\n");
    ThreadSetAffinity(0,1);
    FUN_10007420('\0','\x01','\x01',1);
    ThreadSetAffinity(0,UStack_f0);
    Msg("ThreadPoolTest:     NO Sleep\n");
    FUN_10007420('\0','\0','\x01',1);
    Msg("ThreadPoolTest:     Distribute\n");
    FUN_10007420('\x01','\0','\x01',1);
    Msg("ThreadPoolTest:     One core\n");
    ThreadSetAffinity(0,1);
    FUN_10007420('\0','\0','\x01',1);
    ThreadSetAffinity(0,UStack_f0);
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  pUVar9 = &UStack_e8;
  pUVar8 = &UStack_f0;
  pvVar1 = GetCurrentProcess();
  GetProcessAffinityMask(pvVar1,pUVar8,pUVar9);
  FUN_10007880();
  local_e0 = &PTR_LAB_1001db54;
  ppuStack_dc = &PTR_LAB_1001db48;
  uStack_2c = 0;
  if (-1 < iStack_30) {
    if (iStack_38 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iStack_38);
      iStack_38 = 0;
    }
    uStack_34 = 0;
  }
  iStack_28 = iStack_38;
  if (-1 < iStack_30) {
    if (iStack_38 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iStack_38);
      iStack_38 = 0;
    }
    uStack_34 = 0;
  }
  uStack_40 = 0;
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
  CThreadSyncObject::~CThreadSyncObject(aCStack_60);
  iVar3 = 2;
  piVar4 = aiStack_70;
  do {
    piVar5 = piVar4 + -8;
    FUN_10007ad0(piVar5);
    FUN_10009a00(*piVar5);
    FUN_10006600(piVar4 + -2);
    iVar3 = iVar3 + -1;
    piVar4 = piVar5;
  } while (-1 < iVar3);
  return;
}


// 10008b60 CreateThreadPool

undefined4 * CreateThreadPool(void)

{
  undefined4 *puVar1;
  
                    /* 0x8b60  16  CreateThreadPool */
  puVar1 = (undefined4 *)FUN_100099a0(0xd0);
  if (puVar1 != (undefined4 *)0x0) {
    puVar1 = FUN_10008190(puVar1);
    return puVar1;
  }
  return (undefined4 *)0x0;
}


// 10008b80 FUN_10008b80

undefined4 __fastcall FUN_10008b80(CWorkerThread *param_1)

{
  int *piVar1;
  CThreadEvent *this;
  int iVar2;
  bool bVar3;
  bool bVar4;
  CThreadEvent *pCVar5;
  DWORD DVar6;
  uint uVar7;
  undefined4 uVar8;
  DWORD DVar9;
  DWORD *pDVar10;
  int *piStack_10;
  HANDLE pvStack_c;
  undefined4 uStack_8;
  undefined4 uStack_4;
  
  LOCK();
  *(int *)(*(int *)(param_1 + 0x104) + 0x90) = *(int *)(*(int *)(param_1 + 0x104) + 0x90) + 1;
  UNLOCK();
  this = (CThreadEvent *)(param_1 + 0x108);
  do {
    CThreadEvent::Set(this);
LAB_10008bb0:
    do {
      pCVar5 = CWorkerThread::GetCallHandle(param_1);
      pvStack_c = *(HANDLE *)pCVar5;
      uStack_8 = *(undefined4 *)(*(int *)(param_1 + 0x100) + 0x70);
      uStack_4 = *(undefined4 *)(param_1 + 0xf0);
      DVar6 = WaitForMultipleObjects(3,&pvStack_c,0,0xffffffff);
      if (DVar6 == 0xffffffff) {
LAB_10008d47:
        LOCK();
        *(int *)(*(int *)(param_1 + 0x104) + 0x90) = *(int *)(*(int *)(param_1 + 0x104) + 0x90) + -1
        ;
        UNLOCK();
        CThreadEvent::Reset(this);
        return 0;
      }
      bVar3 = CWorkerThread::PeekCall(param_1,(uint *)0x0);
      if (bVar3) {
        uVar7 = CWorkerThread::GetCallParam(param_1);
        if (uVar7 == 0) {
          CWorkerThread::Reply(param_1,1);
          goto LAB_10008d47;
        }
        if (uVar7 == 1) {
          CWorkerThread::Reply(param_1,1);
          CThread::Suspend((CThread *)param_1);
        }
        else {
          CWorkerThread::Reply(param_1,0);
        }
        goto LAB_10008bb0;
      }
      bVar3 = false;
      do {
        uVar8 = FUN_10007bc0(param_1 + 0x80,&piStack_10);
        if (((char)uVar8 == '\0') &&
           (uVar8 = FUN_10007bc0(*(void **)(param_1 + 0x100),&piStack_10), (char)uVar8 == '\0'))
        break;
        if (!bVar3) {
          CThreadEvent::Reset(this);
          LOCK();
          *(int *)(*(int *)(param_1 + 0x104) + 0x90) =
               *(int *)(*(int *)(param_1 + 0x104) + 0x90) + -1;
          UNLOCK();
          bVar3 = true;
        }
        piVar1 = piStack_10;
        iVar2 = piStack_10[3];
        uVar8 = *(undefined4 *)(param_1 + 0x110);
        if (((iVar2 == 1) || (iVar2 == 2)) || (iVar2 == 4)) {
          DVar6 = GetCurrentThreadId();
          pDVar10 = (DWORD *)(piVar1 + 5);
          if (DVar6 != piVar1[5]) {
            LOCK();
            DVar9 = *pDVar10;
            if (DVar9 == 0) {
              *pDVar10 = DVar6;
              DVar9 = 0;
            }
            UNLOCK();
            if (DVar9 != 0) goto LAB_10008ced;
          }
          piVar1[6] = piVar1[6] + 1;
          *(char *)((int)piStack_10 + 0x1d) = (char)uVar8;
          FUN_100067c0(piStack_10);
          piVar1 = piVar1 + 6;
          *piVar1 = *piVar1 + -1;
          if (*piVar1 == 0) {
            LOCK();
            *pDVar10 = 0;
            UNLOCK();
          }
        }
LAB_10008ced:
        (**(code **)(*piStack_10 + 4))();
        LOCK();
        *(int *)(*(int *)(param_1 + 0x104) + 200) = *(int *)(*(int *)(param_1 + 0x104) + 200) + -1;
        UNLOCK();
        bVar4 = CWorkerThread::PeekCall(param_1,(uint *)0x0);
      } while (!bVar4);
    } while (!bVar3);
    LOCK();
    *(int *)(*(int *)(param_1 + 0x104) + 0x90) = *(int *)(*(int *)(param_1 + 0x104) + 0x90) + 1;
    UNLOCK();
  } while( true );
}


// 10008d70 FUN_10008d70

CThread * __thiscall FUN_10008d70(void *this,byte param_1)

{
  CThreadSyncObject::~CThreadSyncObject((CThreadSyncObject *)((int)this + 0x108));
  FUN_10008150((int)this + 0x80);
  CThreadSyncObject::~CThreadSyncObject((CThreadSyncObject *)((int)this + 0x68));
  CThreadSyncObject::~CThreadSyncObject((CThreadSyncObject *)((int)this + 0x60));
  CThread::~CThread((CThread *)this);
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (CThread *)this;
}


// 10008dc0 FUN_10008dc0

void __thiscall FUN_10008dc0(void *this,undefined4 *param_1)

{
  int *piVar1;
  int iVar2;
  undefined4 *puVar3;
  
  piVar1 = (int *)CommandLine_Tier0();
  (**(code **)(*piVar1 + 0x1c))("-threads",0xffffffff);
  puVar3 = (undefined4 *)&stack0xfffffed8;
  for (iVar2 = 0x45; iVar2 != 0; iVar2 = iVar2 + -1) {
    *puVar3 = *param_1;
    param_1 = param_1 + 1;
    puVar3 = puVar3 + 1;
  }
                    /* WARNING: Load size is inaccurate */
  (**(code **)(*this + 0xc))(&stack0xfffffed8,0);
  return;
}


// 10008e30 FUN_10008e30

undefined4 * __thiscall FUN_10008e30(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_LAB_1001db54;
  *(undefined ***)((int)this + 4) = &PTR_LAB_1001db48;
  FUN_10007b50((int *)((int)this + 0xa8));
  FUN_10007b50((int *)((int)this + 0x94));
  FUN_10008150((int)this + 0x10);
  *(undefined ***)this = &PTR_LAB_1001d88c;
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (undefined4 *)this;
}


// 10008f70 FUN_10008f70

void __thiscall FUN_10008f70(void *this,int *param_1)

{
  int iVar1;
  
  if (param_1 != (int *)0x0) {
    if (param_1[8] != -1) {
      Warning("Cannot add a thread job already committed to another thread pool\n");
      return;
    }
    if (*(int *)((int)this + 0xa0) == 0) {
LAB_10008fa2:
      FUN_100067c0(param_1);
      return;
    }
    if ((*(byte *)(param_1 + 7) & 9) == 0) {
                    /* WARNING: Load size is inaccurate */
      iVar1 = (**(code **)(*this + 0x20))();
      if (iVar1 == 0) goto LAB_10008fa2;
      param_1[4] = 2;
    }
    if ((param_1[3] == 1) || (param_1[3] == 4)) {
      param_1[9] = (int)this;
      param_1[3] = 1;
      FUN_10008280(this,param_1);
      LOCK();
      *(int *)((int)this + 200) = *(int *)((int)this + 200) + 1;
      UNLOCK();
    }
    else if (DAT_10058378 < 1) {
      Warning("Attempted to add job to job queue that has already been completed\n");
      DAT_10058378 = DAT_10058378 + 1;
      return;
    }
  }
  return;
}


// 10009360 KeyValuesSystem

undefined * KeyValuesSystem(void)

{
                    /* 0x9360  19  KeyValuesSystem */
  return &DAT_10058398;
}


// 100093e0 FUN_100093e0

uint FUN_100093e0(char *param_1,uint param_2)

{
  char *pcVar1;
  char cVar2;
  uint uVar3;
  
  uVar3 = 0;
  cVar2 = *param_1;
  while (cVar2 != '\0') {
    if ((cVar2 < 'A') || ('Z' < cVar2)) {
      uVar3 = (int)cVar2 + uVar3 * 2;
    }
    else {
      uVar3 = cVar2 + 0x20 + uVar3 * 2;
    }
    pcVar1 = param_1 + 1;
    param_1 = param_1 + 1;
    cVar2 = *pcVar1;
  }
  return uVar3 % param_2;
}


// 10009420 FUN_10009420

void __thiscall FUN_10009420(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000947b;
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
LAB_1000947b:
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


// 10009660 FUN_10009660

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __thiscall FUN_10009660(void *this,int param_1)

{
  if ((_DAT_1005838c & 1) == 0) {
    _DAT_1005838c = _DAT_1005838c | 1;
    DAT_1005837c = 0xffffffff;
    _DAT_10058380 = 0xffffffff;
    _DAT_10058384 = 0xffffffff;
    _DAT_10058388 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x18);
  }
  return DAT_1005837c;
}


// 100096b0 FUN_100096b0

void __fastcall FUN_100096b0(void *param_1)

{
  int iVar1;
  int iVar2;
  
  iVar2 = *(int *)((int)param_1 + 0x1c);
  if (iVar2 != -1) {
    iVar1 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (iVar1 != -1) {
      if ((((iVar1 == 0) || (0 < iVar1)) && (iVar1 < *(int *)((int)param_1 + 8))) &&
         ((iVar1 <= iVar2 && (iVar2 = FUN_10009660(param_1,iVar1), iVar2 != iVar1)))) {
        *(undefined4 *)(iVar1 * 0x18 + 4 + *(int *)((int)param_1 + 4)) =
             *(undefined4 *)((int)param_1 + 0x18);
        *(int *)(iVar1 * 0x18 + *(int *)((int)param_1 + 4)) = iVar1;
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


// 10009740 FUN_10009740

void __fastcall FUN_10009740(void *param_1)

{
  FUN_100096b0(param_1);
  *(undefined4 *)((int)param_1 + 0x18) = 0xffffffff;
  if (-1 < *(int *)((int)param_1 + 0xc)) {
    if (*(int *)((int)param_1 + 4) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)param_1 + 4));
      *(undefined4 *)((int)param_1 + 4) = 0;
    }
    *(undefined4 *)((int)param_1 + 8) = 0;
  }
  *(undefined4 *)((int)param_1 + 0x1c) = 0xffffffff;
  if (-1 < *(int *)((int)param_1 + 0xc)) {
    if (*(int *)((int)param_1 + 4) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)((int)param_1 + 4));
      *(undefined4 *)((int)param_1 + 4) = 0;
    }
    *(undefined4 *)((int)param_1 + 8) = 0;
  }
  return;
}


// 100097a0 FUN_100097a0

undefined4 * __fastcall FUN_100097a0(undefined4 *param_1)

{
  undefined4 *this;
  int *this_00;
  undefined1 *puVar1;
  int iVar2;
  undefined1 *puVar3;
  int iVar4;
  int iVar5;
  undefined4 uVar6;
  
  this = param_1 + 2;
  *param_1 = &PTR_LAB_1001def0;
  FUN_1000d7f0(this);
  FUN_1000db80(param_1 + 10,8,0x40,1,"CKeyValuesSystem::m_HashItemMemPool",0);
  param_1[0x16] = 0;
  param_1[0x17] = 0;
  param_1[0x18] = 0;
  param_1[0x19] = 0;
  param_1[0x1a] = 0;
  this_00 = param_1 + 0x16;
  param_1[0x1b] = &LAB_10009340;
  param_1[0x1c] = 0;
  param_1[0x1d] = 0;
  param_1[0x1e] = 0;
  param_1[0x1f] = 0xffffffff;
  param_1[0x21] = 0xffffffff;
  param_1[0x20] = 0;
  param_1[0x22] = 0xffffffff;
  param_1[0x23] = param_1[0x1c];
  param_1[0x24] = 0;
  param_1[0x25] = 0;
  iVar5 = param_1[0x19];
  if ((int)param_1[0x17] < iVar5 + 0x7ff) {
    FUN_10009420(this_00,(iVar5 - param_1[0x17]) + 0x7ff);
  }
  param_1[0x19] = param_1[0x19] + 0x7ff;
  iVar2 = *this_00;
  iVar4 = (param_1[0x19] - iVar5) + -0x7ff;
  param_1[0x1a] = iVar2;
  if (0 < iVar4) {
    FUN_1000b3c0((void *)(iVar2 + (iVar5 + 0x7ff) * 8),(void *)(iVar2 + iVar5 * 8),iVar4 * 8);
  }
  iVar5 = 0;
  if (0 < (int)param_1[0x19]) {
    do {
      *(undefined4 *)(*this_00 + iVar5 * 8) = 0;
      *(undefined4 *)(*this_00 + 4 + iVar5 * 8) = 0;
      iVar5 = iVar5 + 1;
    } while (iVar5 < (int)param_1[0x19]);
  }
  FUN_1000d810(this,0x400000,0x10000,0,4);
  puVar3 = (undefined1 *)*this;
  puVar1 = puVar3 + (~(param_1[7] - 1) & param_1[7]);
  if (((undefined1 *)param_1[3] < puVar1) &&
     (uVar6 = FUN_1000d920(this,(int)puVar1), (char)uVar6 == '\0')) {
    uRam00000000 = 0;
    param_1[1] = 0x24;
    return param_1;
  }
  *this = puVar1;
  *puVar3 = 0;
  param_1[1] = 0x24;
  return param_1;
}


// 10009910 FUN_10009910

char * FUN_10009910(void)

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
    VirtualQuery(&DAT_1005843c,&_Stack_1c,0x1c);
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


// 100099a0 FUN_100099a0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100099a0(undefined4 param_1)

{
  if ((_DAT_10058444 & 1) == 0) {
    _DAT_10058444 = _DAT_10058444 | 1;
    DAT_10058440 = FUN_10009910();
  }
  if (DAT_10058440 != (char *)0x0) {
    (**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_10058440,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  return;
}


// 10009a00 FUN_10009a00

void __cdecl FUN_10009a00(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1);
  return;
}


// 10009a20 FUN_10009a20

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_10009a20(size_t param_1)

{
  void *pvVar1;
  
  if ((_DAT_10058444 & 1) == 0) {
    _DAT_10058444 = _DAT_10058444 | 1;
    DAT_10058440 = FUN_10009910();
  }
  if (DAT_10058440 != (char *)0x0) {
    pvVar1 = (void *)(**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_10058440,0);
    _memset(pvVar1,0,param_1);
    return pvVar1;
  }
  pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  _memset(pvVar1,0,param_1);
  return pvVar1;
}


// 10009aa0 FUN_10009aa0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10009aa0(undefined4 param_1,undefined4 param_2)

{
  if ((_DAT_1005844c & 1) == 0) {
    _DAT_1005844c = _DAT_1005844c | 1;
    DAT_10058448 = FUN_10009910();
  }
  if (DAT_10058448 != (char *)0x0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 8))(param_1,param_2,DAT_10058448,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(param_1,param_2);
  return;
}


// 10009b00 FUN_10009b00

void __cdecl FUN_10009b00(int param_1,int param_2)

{
  FUN_10009a20(param_1 * param_2);
  return;
}


// 10009b30 FUN_10009b30

void __cdecl FUN_10009b30(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x20))(param_1);
  return;
}


// 10009b50 FUN_10009b50

bool FUN_10009b50(void)

{
  return *(int *)g_pMemAlloc_exref != 0;
}


// 10009b80 FUN_10009b80

void __thiscall FUN_10009b80(void *this,int param_1)

{
  int *piVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (0 < param_1) {
    iVar3 = *(int *)((int)this + 8);
    if ((iVar3 == 0) && (iVar3 = *(int *)((int)this + 4), iVar3 == 0)) {
      iVar3 = 1;
    }
    if (iVar3 < param_1) {
      iVar3 = iVar3 * ((iVar3 + -1 + param_1) / iVar3);
    }
    *(int *)((int)this + 4) = *(int *)((int)this + 4) + iVar3;
    puVar2 = (undefined4 *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(iVar3 * 0x60 + 8);
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


// 10009c10 FUN_10009c10

void FUN_10009c10(int *param_1,int *param_2)

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


// 10009c90 FUN_10009c90

void __thiscall FUN_10009c90(void *this,int param_1,int param_2)

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
  FUN_10009b80(this,param_2);
  return;
}


// 10009ce0 FUN_10009ce0

uint FUN_10009ce0(undefined4 *param_1,uint *param_2)

{
  undefined4 *puVar1;
  uint uVar2;
  
  puVar1 = (undefined4 *)*param_2;
  if (((puVar1 != (undefined4 *)0x0) && (uVar2 = param_2[1], -1 < (int)uVar2)) &&
     ((int)uVar2 < (int)puVar1[1])) {
    if ((puVar1 + 2 <= param_1) && (param_1 < puVar1 + 2 + puVar1[1] * 0x18)) {
      return CONCAT31((int3)((uint)(puVar1 + uVar2 * 0x18 + 2) >> 8),
                      (int)(puVar1 + uVar2 * 0x18 + 2) < (int)param_1);
    }
    puVar1 = (undefined4 *)*puVar1;
    while( true ) {
      if (puVar1 == (undefined4 *)0x0) {
        return 0;
      }
      if ((puVar1 + 2 <= param_1) && (param_1 < puVar1 + 2 + puVar1[1] * 0x18)) break;
      puVar1 = (undefined4 *)*puVar1;
    }
    return CONCAT31((int3)((uint)puVar1 >> 8),1);
  }
  return (uint)puVar1 & 0xffffff00;
}


// 10009d90 FUN_10009d90

void __fastcall FUN_10009d90(int *param_1)

{
  if (param_1[8] != 0) {
    (**(code **)(*param_1 + 0x20))(param_1[0xd]);
  }
  *(undefined1 *)(param_1 + 0xe) = 0;
  if ((DAT_100586a4 != 0) && ((char)param_1[1] != '\0')) {
    FUN_1000b5e0();
    return;
  }
  return;
}


// 10009e00 FUN_10009e00

int FUN_10009e00(int param_1)

{
  va_list *dwLanguageId;
  DWORD dwMessageId;
  char *pcVar1;
  undefined4 uVar2;
  CHAR *pCVar3;
  DWORD nSize;
  va_list *Arguments;
  DWORD local_204;
  CHAR aCStack_200 [512];
  
  if (*(HANDLE *)(param_1 + 8) != (HANDLE)0xffffffff) {
    local_204 = 0;
    dwLanguageId = (va_list *)
                   PeekNamedPipe(*(HANDLE *)(param_1 + 8),(LPVOID)0x0,0,(LPDWORD)0x0,&local_204,
                                 (LPDWORD)0x0);
    if (dwLanguageId == (va_list *)0x0) {
      nSize = 0x200;
      pCVar3 = aCStack_200;
      Arguments = dwLanguageId;
      dwMessageId = GetLastError();
      FormatMessageA(0x1000,(LPCVOID)0x0,dwMessageId,(DWORD)dwLanguageId,pCVar3,nSize,Arguments);
      pcVar1 = _strchr(aCStack_200,0xd);
      if (pcVar1 != (char *)0x0) {
        *pcVar1 = '\0';
      }
      pCVar3 = aCStack_200;
      uVar2 = FUN_1000c7f0((undefined4 *)(param_1 + 0x18));
      Warning("Could not read from pipe associated with command %s\nWindows gave the error message:\n   \"%s\"\n"
              ,uVar2,pCVar3);
    }
    else if (local_204 != 0) {
      return local_204 + 1;
    }
  }
  return 0;
}


// 10009eb0 FUN_10009eb0

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

int FUN_10009eb0(uint param_1,int param_2,undefined1 *param_3)

{
  int iVar1;
  undefined1 *puVar2;
  int iVar3;
  BOOL BVar4;
  DWORD DVar5;
  char *pcVar6;
  undefined4 uVar7;
  uint uVar8;
  undefined1 *puVar9;
  DWORD *pDVar10;
  DWORD *pDVar11;
  uint uVar12;
  int aiStack_230 [7];
  undefined1 local_208 [512];
  uint local_8;
  
  puVar2 = param_3;
  uVar8 = param_1;
  iVar1 = *(int *)(param_1 + 8);
  iVar3 = 0;
  if (iVar1 != -1) {
    param_1 = 0;
    local_8 = 0;
    aiStack_230[6] = 0x10009edd;
    iVar3 = -(int)param_3;
    param_3 = local_208 + iVar3 + -0xc;
    *(undefined4 *)((int)aiStack_230 + iVar3 + 0x18) = 0;
    *(uint **)((int)aiStack_230 + iVar3 + 0x14) = &param_1;
    *(undefined4 *)((int)aiStack_230 + iVar3 + 0x10) = 0;
    *(undefined4 *)((int)aiStack_230 + iVar3 + 0xc) = 0;
    *(undefined4 *)((int)aiStack_230 + iVar3 + 8) = 0;
    *(int *)((int)aiStack_230 + iVar3 + 4) = iVar1;
    puVar9 = (undefined1 *)((int)aiStack_230 + iVar3);
    *(undefined4 *)((int)aiStack_230 + iVar3) = 0x10009ef3;
    BVar4 = PeekNamedPipe(*(HANDLE *)((int)aiStack_230 + iVar3 + 4),
                          *(LPVOID *)((int)aiStack_230 + iVar3 + 8),
                          *(DWORD *)((int)aiStack_230 + iVar3 + 0xc),
                          *(LPDWORD *)((int)aiStack_230 + iVar3 + 0x10),
                          *(LPDWORD *)((int)aiStack_230 + iVar3 + 0x14),
                          *(LPDWORD *)((int)aiStack_230 + iVar3 + 0x18));
    if (BVar4 == 0) {
      *(undefined4 *)(puVar9 + -4) = 0;
      *(undefined4 *)(puVar9 + -8) = 0x200;
      *(undefined1 **)(puVar9 + -0xc) = local_208;
      *(undefined4 *)(puVar9 + -0x10) = 0;
      pDVar10 = (DWORD *)(puVar9 + -0x14);
      *(undefined4 *)(puVar9 + -0x14) = 0x10009f0b;
      DVar5 = GetLastError();
      pDVar10[-1] = DVar5;
      pDVar10[-2] = 0;
      pDVar10[-3] = 0x1000;
      pDVar11 = pDVar10 + -4;
      pDVar10[-4] = 0x10009f19;
      FormatMessageA(pDVar10[-3],(LPCVOID)pDVar10[-2],pDVar10[-1],*pDVar10,(LPSTR)pDVar10[1],
                     pDVar10[2],(va_list *)pDVar10[3]);
      *(undefined4 *)((int)pDVar11 + -4) = 0xd;
      *(undefined1 **)((int)pDVar11 + -8) = local_208;
      *(undefined4 *)((int)pDVar11 + -0xc) = 0x10009f27;
      pcVar6 = _strchr(*(char **)((int)pDVar11 + -8),*(int *)((int)pDVar11 + -4));
      if (pcVar6 != (char *)0x0) {
        *pcVar6 = '\0';
      }
      *(undefined1 **)((int)pDVar11 + -4) = local_208;
      *(undefined4 *)((int)pDVar11 + -8) = 0x10009f40;
      uVar7 = FUN_1000c7f0((undefined4 *)(uVar8 + 0x18));
      *(undefined4 *)((int)pDVar11 + -8) = uVar7;
      *(char **)((int)pDVar11 + -0xc) =
           "Could not read from pipe associated with command %s\nWindows gave the error message:\n   \"%s\"\n"
      ;
      *(undefined4 *)((int)pDVar11 + -0x10) = 0x10009f4c;
      Warning();
      return 0;
    }
    if (puVar2 + -1 <= param_1) {
      param_1 = (uint)(puVar2 + -1);
    }
    *(undefined4 *)(puVar9 + -4) = 0;
    *(uint **)(puVar9 + -8) = &local_8;
    *(uint *)(puVar9 + -0xc) = param_1;
    uVar7 = *(undefined4 *)(uVar8 + 8);
    *(undefined1 **)(puVar9 + -0x10) = param_3;
    *(undefined4 *)(puVar9 + -0x14) = uVar7;
    *(undefined4 *)(puVar9 + -0x18) = 0x10009f84;
    ReadFile(*(HANDLE *)(puVar9 + -0x14),*(LPVOID *)(puVar9 + -0x10),*(DWORD *)(puVar9 + -0xc),
             *(LPDWORD *)(puVar9 + -8),*(LPOVERLAPPED *)(puVar9 + -4));
    iVar3 = 0;
    uVar8 = 0;
    if (local_8 != 0) {
      uVar12 = 1;
      do {
        if (((param_3[uVar8] == '\r') && (uVar12 < local_8)) && (param_3[uVar8 + 1] == '\n')) {
          *(undefined1 *)(iVar3 + param_2) = 10;
          uVar8 = uVar8 + 1;
          uVar12 = uVar12 + 1;
        }
        else {
          *(undefined1 *)(iVar3 + param_2) = param_3[uVar8];
        }
        iVar3 = iVar3 + 1;
        uVar8 = uVar8 + 1;
        uVar12 = uVar12 + 1;
      } while (uVar8 < local_8);
    }
  }
  return iVar3;
}


// 1000a190 FUN_1000a190

int __thiscall FUN_1000a190(void *this,char param_1)

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
      piVar2 = (int *)FUN_10009c10(&local_10,piVar1);
    }
    iVar4 = *piVar2;
    iVar3 = piVar2[1];
    if (((iVar4 == 0) || (iVar3 < 0)) || (*(int *)(iVar4 + 4) <= iVar3)) {
      FUN_10009b80(this,1);
      *(undefined4 *)((int)this + 0x28) = 0;
      if (((*piVar1 == 0) || (*(int *)((int)this + 0x24) < 0)) ||
         (*(int *)(*piVar1 + 4) <= *(int *)((int)this + 0x24))) {
        piVar2 = (int *)FUN_10001c70(this,local_8);
      }
      else {
        piVar2 = (int *)FUN_10009c10(&local_18,piVar1);
      }
      iVar4 = *piVar2;
      iVar3 = piVar2[1];
      if (((iVar4 == 0) || (iVar3 < 0)) || (*(int *)(iVar4 + 4) <= iVar3)) {
        if (DAT_10058454 < 10) {
          Warning("CUtlLinkedList overflow! (exhausted memory allocator)\n");
          DAT_10058454 = DAT_10058454 + 1;
        }
        return 0;
      }
    }
    if (iVar3 * 0x60 + 8 + iVar4 == 0) {
      if (DAT_10058450 < 10) {
        Warning("CUtlLinkedList overflow! (exhausted index range)\n");
        DAT_10058450 = DAT_10058450 + 1;
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
      iVar4 = iVar3 * 0x60 + 8 + iVar4;
    }
  }
  else {
    *(undefined4 *)((int)this + 0x14) = *(undefined4 *)(iVar4 + 0x5c);
  }
  if (param_1 == '\0') {
    *(int *)(iVar4 + 0x5c) = iVar4;
    *(int *)(iVar4 + 0x58) = iVar4;
    return iVar4;
  }
  *(undefined4 *)(iVar4 + 0x5c) = 0;
  *(undefined4 *)(iVar4 + 0x58) = 0;
  return iVar4;
}


// 1000a320 FUN_1000a320

void __thiscall FUN_1000a320(void *this,undefined4 *param_1)

{
  int iVar1;
  int iVar2;
  uint uVar3;
  
  if (param_1 != (undefined4 *)0x0) {
    uVar3 = FUN_10009ce0(param_1,(uint *)((int)this + 0x20));
    if (((char)uVar3 == '\0') && ((undefined4 *)param_1[0x16] != param_1)) {
      iVar1 = param_1[0x16];
      iVar2 = param_1[0x17];
      if (iVar1 == 0) {
        *(int *)((int)this + 0xc) = iVar2;
      }
      else {
        *(int *)(iVar1 + 0x5c) = iVar2;
      }
      if (iVar2 != 0) {
        *(int *)(iVar2 + 0x58) = iVar1;
        *(int *)((int)this + 0x18) = *(int *)((int)this + 0x18) + -1;
        param_1[0x17] = param_1;
        param_1[0x16] = param_1;
        return;
      }
      *(int *)((int)this + 0x18) = *(int *)((int)this + 0x18) + -1;
      *(int *)((int)this + 0x10) = iVar1;
      param_1[0x17] = param_1;
      param_1[0x16] = param_1;
    }
  }
  return;
}


// 1000a380 FUN_1000a380

undefined4 __thiscall FUN_1000a380(void *this,int param_1,int param_2,undefined4 param_3)

{
  undefined4 uVar1;
  int unaff_ESI;
  int iVar2;
  int local_10 [4];
  
  FUN_1000c9c0(local_10);
  iVar2 = 0;
  if (0 < param_1) {
    do {
      FUN_1000c870(local_10,*(char **)(param_2 + iVar2 * 4));
      if (iVar2 != param_1 + -1) {
        FUN_1000c870(local_10," ");
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_1);
  }
                    /* WARNING: Load size is inaccurate */
  iVar2 = *this;
  uVar1 = FUN_1000c7f0(local_10);
  uVar1 = (**(code **)(iVar2 + 0x18))(uVar1,param_3);
  if ((-1 < local_10[0]) && (unaff_ESI != 0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(unaff_ESI);
  }
  return uVar1;
}


// 1000a450 FUN_1000a450

undefined4 * __thiscall FUN_1000a450(void *this,undefined4 *param_1)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  
  *(undefined4 *)this = *param_1;
  *(undefined4 *)((int)this + 4) = param_1[1];
  *(undefined4 *)((int)this + 8) = param_1[2];
  *(undefined4 *)((int)this + 0xc) = param_1[3];
  *(undefined4 *)((int)this + 0x10) = param_1[4];
  *(undefined4 *)((int)this + 0x14) = param_1[5];
  FUN_1000c970((void *)((int)this + 0x18),param_1 + 6);
  puVar2 = param_1 + 10;
  puVar3 = (undefined4 *)((int)this + 0x28);
  for (iVar1 = 0xc; iVar1 != 0; iVar1 = iVar1 + -1) {
    *puVar3 = *puVar2;
    puVar2 = puVar2 + 1;
    puVar3 = puVar3 + 1;
  }
  return (undefined4 *)this;
}


// 1000a4a0 FUN_1000a4a0

void __fastcall FUN_1000a4a0(int param_1)

{
  if (-1 < *(int *)(param_1 + 0x30)) {
    if (*(int *)(param_1 + 0x28) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x28));
      *(undefined4 *)(param_1 + 0x28) = 0;
    }
    *(undefined4 *)(param_1 + 0x2c) = 0;
  }
  if (-1 < *(int *)(param_1 + 0x20)) {
    if (*(int *)(param_1 + 0x18) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x18));
      *(undefined4 *)(param_1 + 0x18) = 0;
    }
    *(undefined4 *)(param_1 + 0x1c) = 0;
  }
  return;
}


// 1000a4f0 FUN_1000a4f0

undefined4 * __thiscall FUN_1000a4f0(void *this,undefined4 *param_1)

{
  int iVar1;
  undefined4 *this_00;
  
  this_00 = (undefined4 *)FUN_1000a190(this,'\0');
  if (this_00 == (undefined4 *)0x0) {
    return (undefined4 *)0x0;
  }
  FUN_1000a320(this,this_00);
  this_00[0x17] = 0;
  iVar1 = *(int *)((int)this + 0x10);
  this_00[0x16] = iVar1;
  *(undefined4 **)((int)this + 0x10) = this_00;
  if (iVar1 == 0) {
    *(undefined4 **)((int)this + 0xc) = this_00;
  }
  else {
    *(undefined4 **)(iVar1 + 0x5c) = this_00;
  }
  *(int *)((int)this + 0x18) = *(int *)((int)this + 0x18) + 1;
  FUN_1000a450(this_00,param_1);
  return this_00;
}


// 1000a550 FUN_1000a550

void __fastcall FUN_1000a550(int *param_1)

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
LAB_1000a612:
        param_1[3] = 0;
        param_1[4] = 0;
        param_1[6] = 0;
        return;
      }
    }
    else if (((((-1 < iVar3) && (iVar3 < *(int *)(iVar2 + 4))) &&
              (puVar1 = (undefined4 *)(iVar3 * 0x60 + 8 + iVar2), puVar1 != (undefined4 *)0x0)) &&
             (uVar4 = FUN_10009ce0(puVar1,(uint *)(param_1 + 8)), (char)uVar4 == '\0')) &&
            (((undefined4 *)puVar1[0x16] != puVar1 || ((undefined4 *)puVar1[0x17] == puVar1)))) {
      FUN_1000a4a0((int)puVar1);
      puVar1[0x16] = puVar1;
      puVar1[0x17] = param_1[5];
      param_1[5] = (int)puVar1;
    }
    if ((iVar2 == param_1[8]) && (iVar3 == param_1[9])) goto LAB_1000a612;
    piVar5 = (int *)FUN_10009c10(local_10,&local_18);
  } while( true );
}


// 1000a630 FUN_1000a630

undefined4 * __thiscall FUN_1000a630(void *this,undefined4 *param_1,char param_2)

{
  LPSTR lpCommandLine;
  undefined4 *puVar1;
  char *pcVar2;
  undefined4 uVar3;
  LPSECURITY_ATTRIBUTES lpProcessAttributes;
  LPSECURITY_ATTRIBUTES lpThreadAttributes;
  BOOL BVar4;
  DWORD DVar5;
  LPVOID lpEnvironment;
  DWORD dwLanguageId;
  LPCSTR lpCurrentDirectory;
  CHAR *pCVar6;
  _STARTUPINFOA *lpStartupInfo;
  DWORD nSize;
  _PROCESS_INFORMATION *lpProcessInformation;
  va_list *Arguments;
  _PROCESS_INFORMATION local_254;
  _STARTUPINFOA local_244;
  CHAR aCStack_200 [512];
  
  _memset(&local_244,0,0x44);
  local_244.cb = 0x44;
  if (param_2 != '\0') {
    local_244.hStdInput = (HANDLE)*param_1;
    local_244.hStdError = (HANDLE)param_1[4];
    local_244.hStdOutput = (HANDLE)param_1[3];
    local_244.dwFlags = 0x100;
  }
  lpProcessInformation = &local_254;
  lpStartupInfo = &local_244;
  lpCurrentDirectory = (LPCSTR)0x0;
  lpEnvironment = (LPVOID)0x0;
  DVar5 = 8;
  BVar4 = 1;
  lpThreadAttributes = (LPSECURITY_ATTRIBUTES)0x0;
  lpProcessAttributes = (LPSECURITY_ATTRIBUTES)0x0;
  lpCommandLine = (LPSTR)FUN_1000c7f0(param_1 + 6);
  BVar4 = CreateProcessA((LPCSTR)0x0,lpCommandLine,lpProcessAttributes,lpThreadAttributes,BVar4,
                         DVar5,lpEnvironment,lpCurrentDirectory,lpStartupInfo,lpProcessInformation);
  if (BVar4 != 0) {
    param_1[5] = local_254.hProcess;
    puVar1 = FUN_1000a4f0((void *)((int)this + 8),param_1);
    *(undefined4 **)((int)this + 0x34) = puVar1;
    return puVar1;
  }
  Arguments = (va_list *)0x0;
  nSize = 0x200;
  pCVar6 = aCStack_200;
  dwLanguageId = 0;
  DVar5 = GetLastError();
  FormatMessageA(0x1000,(LPCVOID)0x0,DVar5,dwLanguageId,pCVar6,nSize,Arguments);
  pcVar2 = _strchr(aCStack_200,0xd);
  if (pcVar2 != (char *)0x0) {
    *pcVar2 = '\0';
  }
  pCVar6 = aCStack_200;
  uVar3 = FUN_1000c7f0(param_1 + 6);
  Warning("Could not execute the command:\n   %s\nWindows gave the error message:\n   \"%s\"\n",
          uVar3,pCVar6);
  return (undefined4 *)0x0;
}


// 1000a730 FUN_1000a730

undefined4 * __thiscall FUN_1000a730(void *this,char *param_1,char param_2)

{
  undefined4 *puVar1;
  BOOL BVar2;
  HANDLE hTargetProcessHandle;
  HANDLE hSourceProcessHandle;
  HANDLE hSourceHandle;
  HANDLE *lpTargetHandle;
  DWORD dwDesiredAccess;
  DWORD dwOptions;
  _SECURITY_ATTRIBUTES _Stack_64;
  HANDLE pvStack_58;
  HANDLE pvStack_54;
  HANDLE pvStack_50;
  HANDLE pvStack_4c;
  HANDLE apvStack_48 [2];
  undefined4 local_40 [4];
  undefined1 auStack_30 [48];
  
  if (*(int *)((int)this + 0x34) != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x28))(*(int *)((int)this + 0x34));
  }
  FUN_1000c9c0(local_40);
  FUN_1000d240(auStack_30,0,0,0);
  FUN_1000c830(local_40,param_1);
  if (param_2 == '\0') {
    apvStack_48[0] = (HANDLE)0xffffffff;
    pvStack_54 = (HANDLE)0xffffffff;
    pvStack_58 = (HANDLE)0xffffffff;
    pvStack_4c = (HANDLE)0xffffffff;
    pvStack_50 = (HANDLE)0xffffffff;
    puVar1 = FUN_1000a630(this,&pvStack_58,'\0');
    FUN_1000a4a0((int)&pvStack_58);
    return puVar1;
  }
  _Stack_64.nLength = 0xc;
  _Stack_64.bInheritHandle = 1;
  _Stack_64.lpSecurityDescriptor = (LPVOID)0x0;
  BVar2 = CreatePipe(&pvStack_50,&pvStack_4c,&_Stack_64,0);
  if (BVar2 != 0) {
    BVar2 = CreatePipe(&pvStack_58,&pvStack_54,&_Stack_64,0);
    if (BVar2 != 0) {
      dwOptions = 2;
      BVar2 = 1;
      dwDesiredAccess = 0;
      lpTargetHandle = apvStack_48;
      hTargetProcessHandle = GetCurrentProcess();
      hSourceHandle = pvStack_4c;
      hSourceProcessHandle = GetCurrentProcess();
      BVar2 = DuplicateHandle(hSourceProcessHandle,hSourceHandle,hTargetProcessHandle,lpTargetHandle
                              ,dwDesiredAccess,BVar2,dwOptions);
      if (BVar2 != 0) {
        puVar1 = FUN_1000a630(this,&pvStack_58,'\x01');
        if (puVar1 != (undefined4 *)0x0) {
          FUN_1000a4a0((int)&pvStack_58);
          return puVar1;
        }
        CloseHandle(apvStack_48[0]);
      }
      CloseHandle(pvStack_58);
      CloseHandle(pvStack_54);
    }
    CloseHandle(pvStack_50);
    CloseHandle(pvStack_4c);
  }
  FUN_1000a4a0((int)&pvStack_58);
  return (undefined4 *)0x0;
}


// 1000a890 FUN_1000a890

void __thiscall FUN_1000a890(void *this,undefined4 *param_1)

{
  CloseHandle((HANDLE)param_1[4]);
  CloseHandle((HANDLE)*param_1);
  CloseHandle((HANDLE)param_1[1]);
  CloseHandle((HANDLE)param_1[2]);
  CloseHandle((HANDLE)param_1[3]);
  FUN_1000a320((void *)((int)this + 8),param_1);
  FUN_1000a4a0((int)param_1);
  param_1[0x17] = *(undefined4 *)((int)this + 0x1c);
  *(undefined4 **)((int)this + 0x1c) = param_1;
  return;
}


// 1000a950 InstallUniformRandomStream

void __cdecl InstallUniformRandomStream(undefined *param_1)

{
                    /* 0xa950  18  InstallUniformRandomStream */
  PTR_DAT_100276b8 = param_1;
  if (param_1 == (undefined *)0x0) {
    PTR_DAT_100276b8 = (undefined *)&DAT_10058470;
  }
  return;
}


// 1000a970 RandomSeed

void __cdecl RandomSeed(undefined4 param_1)

{
                    /* 0xa970  24  RandomSeed */
  (*(code *)**(undefined4 **)PTR_DAT_100276b8)(param_1);
  return;
}


// 1000a990 RandomFloat

void __cdecl RandomFloat(undefined4 param_1,undefined4 param_2)

{
                    /* 0xa990  20  RandomFloat */
  (**(code **)(*(int *)PTR_DAT_100276b8 + 4))(param_1,param_2);
  return;
}


// 1000a9b0 RandomFloatExp

void __cdecl RandomFloatExp(undefined4 param_1,undefined4 param_2,undefined4 param_3)

{
                    /* 0xa9b0  21  RandomFloatExp */
  (**(code **)(*(int *)PTR_DAT_100276b8 + 0xc))(param_1,param_2,param_3);
  return;
}


// 1000a9e0 RandomInt

void __cdecl RandomInt(undefined4 param_1,undefined4 param_2)

{
                    /* 0xa9e0  23  RandomInt */
  (**(code **)(*(int *)PTR_DAT_100276b8 + 8))(param_1,param_2);
  return;
}


// 1000aa00 CUniformRandomStream::SetSeed

/* public: virtual void __thiscall CUniformRandomStream::SetSeed(int) */

void __thiscall CUniformRandomStream::SetSeed(CUniformRandomStream *this,int param_1)

{
  CUniformRandomStream *pCVar1;
  CThreadFastMutex *this_00;
  DWORD DVar2;
  int iVar3;
  
                    /* 0xaa00  14  ?SetSeed@CUniformRandomStream@@UAEXH@Z */
  this_00 = (CThreadFastMutex *)(this + 0x8c);
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
      goto LAB_1000aa3b;
    }
  }
  *(int *)(this + 0x90) = *(int *)(this + 0x90) + 1;
LAB_1000aa3b:
  if (-1 < param_1) {
    param_1 = -param_1;
  }
  *(int *)(this + 4) = param_1;
  *(undefined4 *)(this + 8) = 0;
  pCVar1 = this + 0x90;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return;
}


// 1000aa70 CUniformRandomStream::GenerateRandomNumber

/* private: int __thiscall CUniformRandomStream::GenerateRandomNumber(void) */

int __thiscall CUniformRandomStream::GenerateRandomNumber(CUniformRandomStream *this)

{
  CThreadFastMutex *this_00;
  code *pcVar1;
  char cVar2;
  DWORD DVar3;
  int iVar4;
  int iVar5;
  CUniformRandomStream *pCVar6;
  uint uVar7;
  
                    /* 0xaa70  9  ?GenerateRandomNumber@CUniformRandomStream@@AAEHXZ */
  this_00 = (CThreadFastMutex *)(this + 0x8c);
  DVar3 = GetCurrentThreadId();
  if (DVar3 != *(DWORD *)this_00) {
    LOCK();
    iVar5 = *(int *)this_00;
    if (iVar5 == 0) {
      *(DWORD *)this_00 = DVar3;
      iVar5 = 0;
    }
    UNLOCK();
    if (iVar5 != 0) {
      CThreadFastMutex::Lock(this_00,DVar3,0);
      goto LAB_1000aab0;
    }
  }
  *(int *)(this + 0x90) = *(int *)(this + 0x90) + 1;
LAB_1000aab0:
  iVar5 = *(int *)(this + 4);
  if ((iVar5 < 1) || (*(int *)(this + 8) == 0)) {
    if (iVar5 < 0) {
      *(int *)(this + 4) = -iVar5;
    }
    else {
      *(undefined4 *)(this + 4) = 1;
    }
    pCVar6 = this + 0xa4;
    iVar5 = 0x25;
    do {
      iVar4 = *(int *)(this + 4) * 0x41a7 + (*(int *)(this + 4) / 0x1f31d) * -0x7fffffff;
      *(int *)(this + 4) = iVar4;
      if (iVar4 < 0) {
        *(int *)(this + 4) = iVar4 + 0x7fffffff;
      }
      if (iVar5 + 2 < 0x20) {
        *(undefined4 *)(pCVar6 + 4) = *(undefined4 *)(this + 4);
      }
      iVar4 = *(int *)(this + 4) * 0x41a7 + (*(int *)(this + 4) / 0x1f31d) * -0x7fffffff;
      *(int *)(this + 4) = iVar4;
      if (iVar4 < 0) {
        *(int *)(this + 4) = iVar4 + 0x7fffffff;
      }
      if (iVar5 + 1 < 0x20) {
        *(undefined4 *)pCVar6 = *(undefined4 *)(this + 4);
      }
      iVar4 = *(int *)(this + 4) * 0x41a7 + (*(int *)(this + 4) / 0x1f31d) * -0x7fffffff;
      *(int *)(this + 4) = iVar4;
      if (iVar4 < 0) {
        *(int *)(this + 4) = iVar4 + 0x7fffffff;
      }
      if (iVar5 < 0x20) {
        *(undefined4 *)(pCVar6 + -4) = *(undefined4 *)(this + 4);
      }
      iVar4 = *(int *)(this + 4) * 0x41a7 + (*(int *)(this + 4) / 0x1f31d) * -0x7fffffff;
      *(int *)(this + 4) = iVar4;
      if (iVar4 < 0) {
        *(int *)(this + 4) = iVar4 + 0x7fffffff;
      }
      if (iVar5 + -1 < 0x20) {
        *(undefined4 *)(pCVar6 + -8) = *(undefined4 *)(this + 4);
      }
      iVar4 = *(int *)(this + 4) * 0x41a7 + (*(int *)(this + 4) / 0x1f31d) * -0x7fffffff;
      *(int *)(this + 4) = iVar4;
      if (iVar4 < 0) {
        *(int *)(this + 4) = iVar4 + 0x7fffffff;
      }
      if (iVar5 + -2 < 0x20) {
        *(undefined4 *)(pCVar6 + -0xc) = *(undefined4 *)(this + 4);
      }
      iVar4 = iVar5 + -3;
      pCVar6 = pCVar6 + -0x14;
      iVar5 = iVar5 + -5;
    } while (-1 < iVar4);
    *(undefined4 *)(this + 8) = *(undefined4 *)(this + 0xc);
  }
  iVar5 = *(int *)(this + 4) * 0x41a7 + (*(int *)(this + 4) / 0x1f31d) * -0x7fffffff;
  *(int *)(this + 4) = iVar5;
  if (iVar5 < 0) {
    *(int *)(this + 4) = iVar5 + 0x7fffffff;
  }
  uVar7 = (int)(*(int *)(this + 8) + (*(int *)(this + 8) >> 0x1f & 0x3ffffffU)) >> 0x1a;
  if ((0x1f < (int)uVar7) || ((int)uVar7 < 0)) {
    cVar2 = Plat_IsInDebugSession();
    if (cVar2 != '\0') {
      pcVar1 = (code *)swi(3);
      iVar5 = (*pcVar1)();
      return iVar5;
    }
    Warning("CUniformRandomStream had an array overrun: tried to write to element %d of 0..31. Contact Tom or Elan.\n"
            ,uVar7);
    uVar7 = uVar7 & 0x8000001f;
    if ((int)uVar7 < 0) {
      uVar7 = (uVar7 - 1 | 0xffffffe0) + 1;
    }
    uVar7 = uVar7 & 0x7fffffff;
  }
  *(undefined4 *)(this + 8) = *(undefined4 *)(this + uVar7 * 4 + 0xc);
  *(undefined4 *)(this + uVar7 * 4 + 0xc) = *(undefined4 *)(this + 4);
  pCVar6 = this + 0x90;
  *(int *)pCVar6 = *(int *)pCVar6 + -1;
  if (*(int *)pCVar6 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return *(int *)(this + 8);
}


// 1000acd0 CUniformRandomStream::RandomFloat

/* public: virtual float __thiscall CUniformRandomStream::RandomFloat(float,float) */

float __thiscall
CUniformRandomStream::RandomFloat(CUniformRandomStream *this,float param_1,float param_2)

{
  float fVar1;
  int iVar2;
  
                    /* 0xacd0  11  ?RandomFloat@CUniformRandomStream@@UAEMMM@Z */
  iVar2 = GenerateRandomNumber(this);
  fVar1 = (float)iVar2 * 4.656613e-10;
  if (0.9999999 < fVar1) {
    fVar1 = 0.9999999;
  }
  return param_1 + (param_2 - param_1) * fVar1;
}


// 1000ad10 CUniformRandomStream::RandomFloatExp

/* public: virtual float __thiscall CUniformRandomStream::RandomFloatExp(float,float,float) */

float __thiscall
CUniformRandomStream::RandomFloatExp
          (CUniformRandomStream *this,float param_1,float param_2,float param_3)

{
  float fVar1;
  int iVar2;
  float10 fVar3;
  
                    /* 0xad10  12  ?RandomFloatExp@CUniformRandomStream@@UAEMMMM@Z */
  iVar2 = GenerateRandomNumber(this);
  fVar1 = (float)iVar2 * 4.656613e-10;
  if (0.9999999 < fVar1) {
    fVar1 = 0.9999999;
  }
  if (param_3 != 1.0) {
    fVar3 = (float10)FUN_1000e490();
    return (float)((float10)param_1 + ((float10)param_2 - (float10)param_1) * fVar3);
  }
  return param_1 + (param_2 - param_1) * fVar1;
}


// 1000ad80 CUniformRandomStream::RandomInt

/* public: virtual int __thiscall CUniformRandomStream::RandomInt(int,int) */

int __thiscall CUniformRandomStream::RandomInt(CUniformRandomStream *this,int param_1,int param_2)

{
  uint uVar1;
  uint uVar2;
  
                    /* 0xad80  13  ?RandomInt@CUniformRandomStream@@UAEHHH@Z */
  uVar2 = (param_2 - param_1) + 1;
  if ((1 < uVar2) && ((uint)(param_2 - param_1) < 0x80000000)) {
    do {
      uVar1 = GenerateRandomNumber(this);
    } while (0x7fffffffU - (int)(0x80000000 % (ulonglong)uVar2) < uVar1);
    return uVar1 % uVar2 + param_1;
  }
  return param_1;
}


// 1000ade0 CGaussianRandomStream::AttachToStream

/* public: void __thiscall CGaussianRandomStream::AttachToStream(class IUniformRandomStream *) */

void __thiscall
CGaussianRandomStream::AttachToStream(CGaussianRandomStream *this,IUniformRandomStream *param_1)

{
  CThreadFastMutex *this_00;
  CGaussianRandomStream *pCVar1;
  DWORD DVar2;
  int iVar3;
  
                    /* 0xade0  8
                       ?AttachToStream@CGaussianRandomStream@@QAEXPAVIUniformRandomStream@@@Z */
  this_00 = (CThreadFastMutex *)(this + 0xc);
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
      goto LAB_1000ae18;
    }
  }
  *(int *)(this + 0x10) = *(int *)(this + 0x10) + 1;
LAB_1000ae18:
  *(IUniformRandomStream **)this = param_1;
  this[4] = (CGaussianRandomStream)0x0;
  pCVar1 = this + 0x10;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return;
}


// 1000ae40 CGaussianRandomStream::RandomFloat

/* public: float __thiscall CGaussianRandomStream::RandomFloat(float,float) */

float __thiscall
CGaussianRandomStream::RandomFloat(CGaussianRandomStream *this,float param_1,float param_2)

{
  CThreadFastMutex *this_00;
  CGaussianRandomStream *pCVar1;
  DWORD DVar2;
  int iVar3;
  int *piVar4;
  float10 fVar5;
  float10 fVar6;
  float10 fVar7;
  
                    /* 0xae40  10  ?RandomFloat@CGaussianRandomStream@@QAEMMM@Z */
  this_00 = (CThreadFastMutex *)(this + 0xc);
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
      goto LAB_1000ae79;
    }
  }
  *(int *)(this + 0x10) = *(int *)(this + 0x10) + 1;
LAB_1000ae79:
  piVar4 = *(int **)this;
  if (*(int **)this == (int *)0x0) {
    piVar4 = (int *)PTR_DAT_100276b8;
  }
  if (this[4] == (CGaussianRandomStream)0x0) {
    do {
      do {
        (**(code **)(*piVar4 + 4))(0,0x3f800000);
        fVar5 = (float10)(**(code **)(*piVar4 + 4))(0,0x3f800000);
        fVar6 = (fVar5 + fVar5) - (float10)1;
        fVar5 = (float10)(float)this;
        fVar7 = fVar5 * fVar5 + fVar6 * fVar6;
      } while ((float10)1 < fVar7);
    } while ((float10)0 == fVar7);
    this[4] = (CGaussianRandomStream)0x1;
    fVar7 = SQRT((((float10)0.6931471805599453 * fVar7) / fVar7) * (float10)-2.0);
    *(float *)(this + 8) = (float)(fVar7 * fVar5);
    pCVar1 = this + 0x10;
    *(int *)pCVar1 = *(int *)pCVar1 + -1;
    fVar5 = fVar7 * fVar6 * (float10)param_2 + (float10)param_1;
    if (*(int *)pCVar1 == 0) {
      LOCK();
      *(undefined4 *)this_00 = 0;
      UNLOCK();
      return (float)fVar5;
    }
  }
  else {
    this[4] = (CGaussianRandomStream)0x0;
    pCVar1 = this + 0x10;
    *(int *)pCVar1 = *(int *)pCVar1 + -1;
    fVar5 = (float10)*(float *)(this + 8) * (float10)param_2 + (float10)param_1;
    if (*(int *)pCVar1 == 0) {
      LOCK();
      *(undefined4 *)this_00 = 0;
      UNLOCK();
    }
  }
  return (float)fVar5;
}


// 1000af60 RandomGaussianFloat

float10 __cdecl RandomGaussianFloat(float param_1,float param_2)

{
  float fVar1;
  
                    /* 0xaf60  22  RandomGaussianFloat */
  fVar1 = CGaussianRandomStream::RandomFloat((CGaussianRandomStream *)&DAT_10058504,param_1,param_2)
  ;
  return (float10)fVar1;
}


// 1000af80 CUniformRandomStream::CUniformRandomStream

/* public: __thiscall CUniformRandomStream::CUniformRandomStream(void) */

CUniformRandomStream * __thiscall
CUniformRandomStream::CUniformRandomStream(CUniformRandomStream *this)

{
  CUniformRandomStream *pCVar1;
  CThreadFastMutex *this_00;
  DWORD DVar2;
  int iVar3;
  
                    /* 0xaf80  3  ??0CUniformRandomStream@@QAE@XZ */
  *(undefined ***)this = &_vftable_;
  this_00 = (CThreadFastMutex *)(this + 0x8c);
  *(undefined4 *)this_00 = 0;
  *(undefined4 *)(this + 0x90) = 0;
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
      goto LAB_1000afcf;
    }
  }
  *(int *)(this + 0x90) = *(int *)(this + 0x90) + 1;
LAB_1000afcf:
  *(undefined4 *)(this + 4) = 0;
  *(undefined4 *)(this + 8) = 0;
  pCVar1 = this + 0x90;
  *(int *)pCVar1 = *(int *)pCVar1 + -1;
  if (*(int *)pCVar1 == 0) {
    LOCK();
    *(undefined4 *)this_00 = 0;
    UNLOCK();
  }
  return this;
}


// 1000aff0 CGaussianRandomStream::CGaussianRandomStream

/* public: __thiscall CGaussianRandomStream::CGaussianRandomStream(class IUniformRandomStream *) */

CGaussianRandomStream * __thiscall
CGaussianRandomStream::CGaussianRandomStream
          (CGaussianRandomStream *this,IUniformRandomStream *param_1)

{
                    /* 0xaff0  1  ??0CGaussianRandomStream@@QAE@PAVIUniformRandomStream@@@Z */
  *(undefined4 *)(this + 0xc) = 0;
  *(undefined4 *)(this + 0x10) = 0;
  AttachToStream(this,param_1);
  return this;
}


// 1000b010 FUN_1000b010

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * __thiscall FUN_1000b010(void *this,ushort param_1)

{
  if ((_DAT_10058520 & 1) == 0) {
    _DAT_10058520 = _DAT_10058520 | 1;
    _DAT_10058518 = 0xffff;
    _DAT_1005851a = 0xffff;
    _DAT_1005851c = 0xffff;
    _DAT_1005851e = 1;
  }
  if (param_1 != 0xffff) {
    return (undefined *)((uint)param_1 * 0x10 + *(int *)((int)this + 4));
  }
  return &DAT_10058518;
}


// 1000b070 FUN_1000b070

void __fastcall FUN_1000b070(void *param_1)

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
         ((uVar2 <= uVar1 && (puVar3 = (ushort *)FUN_1000b010(param_1,uVar2), *puVar3 != uVar2)))) {
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


// 1000b140 FUN_1000b140

void __fastcall FUN_1000b140(void *param_1)

{
  FUN_1000b070(param_1);
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


// 1000b1a0 FUN_1000b1a0

int __cdecl FUN_1000b1a0(char *param_1)

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


// 1000b1d0 FUN_1000b1d0

int __cdecl FUN_1000b1d0(byte *param_1,byte *param_2)

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


// 1000b230 FUN_1000b230

uint __cdecl FUN_1000b230(byte *param_1,byte *param_2)

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
        if (uVar1 != uVar2) goto LAB_1000b2aa;
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
LAB_1000b2aa:
  return uVar1 - uVar2;
}


// 1000b2b0 FUN_1000b2b0

char * __cdecl FUN_1000b2b0(char *param_1,char *param_2)

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


// 1000b390 FUN_1000b390

void __cdecl FUN_1000b390(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  _memcpy(param_1,param_2,param_3);
  return;
}


// 1000b3c0 FUN_1000b3c0

void __cdecl FUN_1000b3c0(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  _memmove(param_1,param_2,param_3);
  return;
}


// 1000b3f0 FUN_1000b3f0

void __cdecl FUN_1000b3f0(char *param_1,char *param_2,size_t param_3)

{
  _AssertValidWritePtr(param_1,param_3);
  AssertValidStringPtr(param_2,0xffffff);
  _strncpy(param_1,param_2,param_3);
  if (0 < (int)param_3) {
    param_1[param_3 - 1] = '\0';
  }
  return;
}


// 1000b430 FUN_1000b430

size_t __cdecl FUN_1000b430(char *param_1,size_t param_2,char *param_3)

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


// 1000b480 FUN_1000b480

size_t __cdecl FUN_1000b480(char *param_1,size_t param_2,char *param_3,va_list param_4)

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


// 1000b4d0 FUN_1000b4d0

size_t __cdecl FUN_1000b4d0(char *param_1,size_t param_2,char *param_3,va_list param_4,int param_5)

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


// 1000b530 FUN_1000b530

void __cdecl FUN_1000b530(undefined4 param_1,undefined **param_2)

{
  int *piVar1;
  int *piVar2;
  
  if ((DAT_100586a4 != (int *)0x0) && (DAT_10058644 == '\0')) {
    DAT_10058640 = param_1;
    DAT_10058644 = '\x01';
    DAT_10027710 = (**(code **)(*DAT_100586a4 + 0x14))();
    piVar2 = DAT_10058638;
    DAT_1005863c = param_2;
    if (param_2 == (undefined **)0x0) {
      DAT_1005863c = &PTR_PTR_1002771c;
    }
    while (piVar2 != (int *)0x0) {
      piVar1 = (int *)piVar2[1];
      (**(code **)(*piVar2 + 0xc))(DAT_10058640);
      (**(code **)(*piVar2 + 0x2c))();
      piVar2 = piVar1;
    }
    (**(code **)(*DAT_100586a4 + 0x70))();
    (**(code **)(*DAT_100586a4 + 0x94))();
    DAT_10058638 = (int *)0x0;
  }
  return;
}


// 1000b5e0 FUN_1000b5e0

void FUN_1000b5e0(void)

{
  if ((DAT_100586a4 != (int *)0x0) && (DAT_10058644 != '\0')) {
    (**(code **)(*DAT_100586a4 + 0x74))(DAT_10027710);
    (**(code **)(*DAT_100586a4 + 0x20))(DAT_10027710);
    DAT_10027710 = 0xffffffff;
    DAT_10058644 = '\0';
  }
  return;
}


// 1000b690 FUN_1000b690

void __fastcall FUN_1000b690(undefined4 param_1)

{
  if (DAT_100586a4 != (int *)0x0) {
    (**(code **)(*DAT_100586a4 + 0x1c))(param_1);
  }
  return;
}


// 1000b720 FUN_1000b720

int * __thiscall
FUN_1000b720(void *this,int param_1,int param_2,undefined *param_3,uint param_4,int param_5)

{
  *(int *)((int)this + 0x18) = param_2;
  *(int *)((int)this + 0x1c) = param_5;
  *(byte *)((int)this + 0x20) = param_5 != 0 | *(byte *)((int)this + 0x20) & 0xfc | 4;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)this = &PTR_LAB_1001e0ec;
  *(undefined1 *)((int)this + 8) = 0;
  *(int *)((int)this + 0xc) = param_1;
  if (param_3 == (undefined *)0x0) {
    param_3 = PTR_DAT_10027748;
  }
  *(undefined **)((int)this + 0x10) = param_3;
  *(uint *)((int)this + 0x14) = param_4;
  if ((param_4 & 1) == 0) {
    *(void **)((int)this + 4) = DAT_10058638;
    DAT_10058638 = this;
  }
  else {
    *(undefined4 *)((int)this + 4) = 0;
  }
  if (DAT_1005863c != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x2c))();
  }
  return (int *)this;
}


// 1000b7b0 FUN_1000b7b0

void __fastcall FUN_1000b7b0(undefined4 *param_1)

{
  *param_1 = &PTR_LAB_1001e0b8;
  return;
}


// 1000b810 FUN_1000b810

void __fastcall FUN_1000b810(undefined4 *param_1)

{
  *param_1 = &PTR_FUN_1001e16c;
  param_1[6] = &PTR_LAB_1001e14c;
  if (param_1[9] != 0) {
    FUN_10009a00(param_1[9]);
    param_1[9] = 0;
  }
  *param_1 = &PTR_LAB_1001e0b8;
  return;
}


// 1000b8c0 FUN_1000b8c0

void __fastcall FUN_1000b8c0(undefined4 param_1)

{
  if (DAT_1005863c != (undefined4 *)0x0) {
    (**(code **)*DAT_1005863c)(param_1);
  }
  return;
}


// 1000b8e0 FUN_1000b8e0

void __fastcall FUN_1000b8e0(int *param_1)

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
  if ((cVar1 != '\0') && (DAT_100586a4 != (int *)0x0)) {
    cVar1 = (**(code **)(*DAT_100586a4 + 0x80))();
    if (cVar1 == '\0') {
      (**(code **)(*DAT_100586a4 + 0x8c))(param_1,unaff_retaddr);
      return;
    }
  }
  aiStack_28[0] = param_1[0xb];
  piVar4 = unaff_retaddr;
  if (unaff_retaddr == (int *)0x0) {
    piVar4 = (int *)&DAT_1001d2ac;
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
    FUN_1000b430((char *)aiStack_28,0x20,"%f");
    piVar4 = aiStack_28;
    uVar2 = extraout_ECX_00;
    uVar3 = extraout_EDX_00;
  }
  param_1[0xb] = 0xb00000;
  uVar5 = FUN_1000f220(uVar2,uVar3);
  param_1[0xc] = (int)uVar5;
  if ((param_1[5] & 0x1000U) == 0) {
    (**(code **)(*param_1 + 0x40))(piVar4,fVar7);
  }
  return;
}


// 1000b9c0 FUN_1000b9c0

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

void __thiscall FUN_1000b9c0(void *this,undefined4 param_1,undefined4 param_2)

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
  
  asStack_1c[2] = 0x1000b9d6;
  iVar2 = -(*(int *)((int)this + 0x28) + 0xfU & 0xfffffff0);
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x28);
  *(undefined4 *)((int)asStack_1c + iVar2 + 4) = uVar4;
  *(undefined1 **)((int)asStack_1c + iVar2) = &stack0xfffffff0 + iVar2;
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = 0x1000b9e3;
  _memcpy(*(void **)((int)asStack_1c + iVar2),*(void **)((int)asStack_1c + iVar2 + 4),
          *(size_t *)((int)asStack_1c + iVar2 + 8));
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = param_1;
  *(undefined4 *)((int)auStack_24 + iVar2) = 0x1000b9ec;
  iVar3 = FUN_1000b1a0(*(char **)((int)auStack_24 + iVar2 + 4));
  iVar3 = iVar3 + 1;
  puVar6 = &stack0xfffffff0 + iVar2;
  if (*(int *)((int)this + 0x28) < iVar3) {
    if (*(int *)((int)this + 0x24) != 0) {
      *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x24);
      *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x1000ba06;
      FUN_10009a00(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    }
    puVar6 = &stack0xfffffff0 + iVar2;
    *(int *)((int)asStack_1c + iVar2 + 8) = iVar3;
    *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x1000ba0f;
    uVar4 = FUN_100099a0(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    *(undefined4 *)((int)this + 0x24) = uVar4;
    *(int *)((int)this + 0x28) = iVar3;
  }
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)(puVar6 + -4) = iVar3;
  *(undefined4 *)(puVar6 + -8) = param_1;
  *(undefined4 *)(puVar6 + -0xc) = uVar4;
  *(undefined4 *)(puVar6 + -0x10) = 0x1000ba26;
  _memcpy(*(void **)(puVar6 + -0xc),*(void **)(puVar6 + -8),*(size_t *)(puVar6 + -4));
  pcVar1 = *(code **)((int)this + 0x44);
  if (pcVar1 != (code *)0x0) {
    *(undefined4 *)(puVar6 + -4) = extraout_ECX;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(int *)(puVar6 + -0xc) = (int)this + 0x18;
    puVar5 = puVar6 + -0x10;
    *(undefined4 *)(puVar6 + -0x10) = 0x1000ba3e;
    (*pcVar1)();
    puVar6 = puVar5 + 0xc;
  }
  if (DAT_100586a4 != (int *)0x0) {
    pcVar1 = *(code **)(*DAT_100586a4 + 0x48);
    *(int **)(puVar6 + -4) = DAT_100586a4;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(void **)(puVar6 + -0xc) = this;
    *(undefined4 *)(puVar6 + -0x10) = 0x1000ba5b;
    (*pcVar1)();
  }
  return;
}


// 1000bac0 FUN_1000bac0

void __thiscall FUN_1000bac0(void *this,float param_1)

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
    if (((cVar1 != '\0') && (DAT_100586a4 != (int *)0x0)) &&
       (cVar1 = (**(code **)(*DAT_100586a4 + 0x80))(), cVar1 == '\0')) {
      (**(code **)(*DAT_100586a4 + 0x84))();
      return;
    }
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x3c))(&stack0x00000000);
    uVar3 = *(undefined4 *)((int)this + 0x2c);
    *(undefined4 *)((int)this + 0x2c) = uStack_4;
    uVar2 = FUN_1000f220(extraout_ECX,extraout_EDX);
    *(int *)((int)this + 0x30) = (int)uVar2;
    if ((*(uint *)((int)this + 0x14) & 0x1000) == 0) {
      FUN_1000b430(&stack0xffffffd8,0x20,"%f");
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x40))(&stack0xffffffd8,uVar3);
      return;
    }
  }
  return;
}


// 1000bc60 FUN_1000bc60

void __thiscall
FUN_1000bc60(void *this,int param_1,char *param_2,uint param_3,undefined *param_4,undefined1 param_5
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
    param_2 = PTR_DAT_1002774c;
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
  _Dst = (void *)FUN_100099a0(param_2 + (1 - (int)pcVar1));
  *(void **)((int)this + 0x24) = _Dst;
  _memcpy(_Dst,*(void **)((int)this + 0x20),*(size_t *)((int)this + 0x28));
  dVar4 = _atof(*(char **)((int)this + 0x24));
  *(float *)((int)this + 0x2c) = (float)dVar4;
  uVar3 = FUN_1000f220(extraout_ECX,extraout_EDX);
  *(int *)((int)this + 0x30) = (int)uVar3;
  if (((param_3 & 0x84200) == 0) && (-1 < (char)param_3)) {
    param_3 = param_3 | 2;
  }
  *(int *)((int)this + 0xc) = param_1;
  *(undefined1 *)((int)this + 8) = 0;
  if (param_4 == (undefined *)0x0) {
    param_4 = PTR_DAT_10027748;
  }
  *(undefined **)((int)this + 0x10) = param_4;
  *(uint *)((int)this + 0x14) = param_3;
  if ((param_3 & 1) == 0) {
    *(void **)((int)this + 4) = DAT_10058638;
    DAT_10058638 = this;
  }
  else {
    *(undefined4 *)((int)this + 4) = 0;
  }
  if (DAT_1005863c != 0) {
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x2c))();
  }
  return;
}


// 1000bd90 FUN_1000bd90

void __fastcall FUN_1000bd90(int param_1)

{
  (**(code **)(*(int *)(*(int *)(param_1 + 0x1c) + 0x18) + 8))
            (*(undefined4 *)(*(int *)(param_1 + 0x1c) + 0x20));
  return;
}


// 1000bdb0 FUN_1000bdb0

float10 __fastcall FUN_1000bdb0(int param_1)

{
  return (float10)*(float *)(*(int *)(param_1 + 0x1c) + 0x38);
}


// 1000bdc0 FUN_1000bdc0

float10 __fastcall FUN_1000bdc0(int param_1)

{
  return (float10)*(float *)(*(int *)(param_1 + 0x1c) + 0x40);
}


// 1000bdd0 FUN_1000bdd0

undefined4 __fastcall FUN_1000bdd0(int param_1)

{
  return CONCAT31((int3)((uint)*(int *)(param_1 + 0x1c) >> 8),
                  *(undefined1 *)(*(int *)(param_1 + 0x1c) + 0x34));
}


// 1000bde0 FUN_1000bde0

undefined4 __fastcall FUN_1000bde0(int param_1)

{
  return CONCAT31((int3)((uint)*(int *)(param_1 + 0x1c) >> 8),
                  *(undefined1 *)(*(int *)(param_1 + 0x1c) + 0x3c));
}


// 1000bdf0 FUN_1000bdf0

undefined4 __fastcall FUN_1000bdf0(int param_1)

{
  return *(undefined4 *)(*(int *)(param_1 + 0x1c) + 0x20);
}


// 1000be00 FUN_1000be00

void __cdecl FUN_1000be00(int *param_1)

{
  char cVar1;
  char cVar2;
  char cVar3;
  char cVar4;
  char cVar5;
  char cVar6;
  char cVar7;
  char cVar8;
  char cVar9;
  char cVar10;
  char cVar11;
  char cVar12;
  char cVar13;
  
  cVar1 = (**(code **)(*param_1 + 8))(4);
  if (cVar1 != '\0') {
    ConMsg(" game");
  }
  cVar2 = (**(code **)(*param_1 + 8))(8);
  if (cVar2 != '\0') {
    ConMsg(" client");
  }
  cVar3 = (**(code **)(*param_1 + 8))(0x80);
  if (cVar3 != '\0') {
    ConMsg(" archive");
  }
  cVar4 = (**(code **)(*param_1 + 8))(0x100);
  if (cVar4 != '\0') {
    ConMsg(" notify");
  }
  cVar5 = (**(code **)(*param_1 + 8))(0x40);
  if (cVar5 != '\0') {
    ConMsg(" singleplayer");
  }
  cVar6 = (**(code **)(*param_1 + 8))(0x400000);
  if (cVar6 != '\0') {
    ConMsg(" notconnected");
  }
  cVar7 = (**(code **)(*param_1 + 8))(0x4000);
  if (cVar7 != '\0') {
    ConMsg(" cheat");
  }
  cVar8 = (**(code **)(*param_1 + 8))(0x2000);
  if (cVar8 != '\0') {
    ConMsg(" replicated");
  }
  cVar9 = (**(code **)(*param_1 + 8))(0x10000000);
  if (cVar9 != '\0') {
    ConMsg(" server_can_execute");
  }
  cVar10 = (**(code **)(*param_1 + 8))(0x40000000);
  if (cVar10 != '\0') {
    ConMsg(" clientcmd_can_execute");
  }
  cVar11 = (**(code **)(*param_1 + 8))(0x200);
  if (cVar11 != '\0') {
    ConMsg(" user");
  }
  cVar12 = (**(code **)(*param_1 + 8))(0x8000);
  if (cVar12 != '\0') {
    ConMsg(" ss");
  }
  cVar13 = (**(code **)(*param_1 + 8))(0x40000);
  if (cVar13 != '\0') {
    ConMsg(" ss_added");
    ConMsg("\n");
    return;
  }
  if (cVar12 != '\0' ||
      (cVar11 != '\0' ||
      (cVar10 != '\0' ||
      (cVar9 != '\0' ||
      (cVar8 != '\0' ||
      (cVar7 != '\0' ||
      (cVar6 != '\0' ||
      (cVar5 != '\0' || (cVar4 != '\0' || (cVar3 != '\0' || (cVar2 != '\0' || cVar1 != '\0')))))))))
      )) {
    ConMsg("\n");
  }
  return;
}


// 1000bfb0 FUN_1000bfb0

void __cdecl FUN_1000bfb0(int *param_1)

{
  char cVar1;
  int iVar2;
  char cVar3;
  char cVar4;
  int *piVar5;
  undefined4 uVar6;
  uint uVar7;
  char *pcVar8;
  float10 fVar9;
  undefined4 local_70;
  int iStack_6c;
  float fStack_68;
  float fStack_64;
  byte abStack_60 [92];
  
  local_70 = 0xff6464ff;
  cVar3 = (**(code **)(*param_1 + 4))();
  if (cVar3 == '\0') {
    piVar5 = (int *)___RTDynamicCast(param_1,0,(_s_RTTICompleteObjectLocator *)&PTR_PTR_10027028,
                                     (TypeDescriptor *)&PTR_PTR_10027750,0);
    iVar2 = param_1[7];
    fStack_68 = *(float *)(iVar2 + 0x38);
    cVar3 = *(char *)(iVar2 + 0x34);
    cVar1 = *(char *)(iVar2 + 0x3c);
    fStack_64 = *(float *)(iVar2 + 0x40);
    if ((piVar5 == (int *)0x0) && (cVar4 = (**(code **)(*param_1 + 8))(), cVar4 == '\0')) {
      if ((param_1[5] & 0x1000U) == 0) {
        pcVar8 = *(char **)(param_1[7] + 0x24);
        if ((byte *)pcVar8 == (byte *)0x0) {
          pcVar8 = &DAT_1001d2ac;
        }
      }
      else {
        pcVar8 = "FCVAR_NEVER_AS_STRING";
      }
    }
    else {
      pcVar8 = (char *)abStack_60;
      if (piVar5 == (int *)0x0) {
        iStack_6c = *(int *)(param_1[7] + 0x30);
        fVar9 = (float10)*(float *)(param_1[7] + 0x2c);
      }
      else {
        iStack_6c = (**(code **)(*piVar5 + 0x4c))();
        fVar9 = (float10)(**(code **)(*piVar5 + 0x48))();
      }
      if ((float10)1e-06 <= ABS((float10)iStack_6c - fVar9)) {
        FUN_1000b430((char *)abStack_60,0x20,"%f");
      }
      else {
        FUN_1000b430((char *)abStack_60,0x20,"%d");
      }
    }
    uVar6 = (**(code **)(*param_1 + 0x18))();
    ConColorMsg((Color *)&local_70,"\"%s\" = \"%s\"",uVar6);
    uVar7 = FUN_1000b230((byte *)pcVar8,*(byte **)(param_1[7] + 0x20));
    if (uVar7 != 0) {
      ConMsg(" ( def. \"%s\" )");
    }
    if (cVar3 != '\0') {
      ConMsg(" min. %f",(double)fStack_68);
    }
    if (cVar1 != '\0') {
      ConMsg(" max. %f",(double)fStack_64);
    }
    ConColorMsg((Color *)&local_70,"\n");
    if (piVar5 != (int *)0x0) {
      fStack_64 = *(float *)(param_1[7] + 0x2c);
      fVar9 = (float10)(**(code **)(*piVar5 + 0x48))();
      if ((float10)0.0001 < ABS(fVar9 - (float10)fStack_64)) {
        fStack_64 = *(float *)(param_1[7] + 0x2c);
        fVar9 = (float10)(**(code **)(*piVar5 + 0x48))();
        ConColorMsg((Color *)&local_70,
                    "** NOTE: The real value is %.3f but the server has temporarily restricted it to %.3f **\n"
                    ,(double)fStack_64,(double)fVar9);
      }
    }
  }
  else {
    (**(code **)(*param_1 + 0x18))();
    ConColorMsg((Color *)&local_70,"\"%s\"\n");
  }
  FUN_1000be00(param_1);
  pcVar8 = (char *)(**(code **)(*param_1 + 0x1c))();
  if ((pcVar8 != (char *)0x0) && (*pcVar8 != '\0')) {
    ConMsg(" - %s\n");
  }
  return;
}


// 1000c260 FUN_1000c260

undefined4 * __thiscall
FUN_1000c260(void *this,int param_1,char *param_2,uint param_3,undefined *param_4,undefined1 param_5
            ,int param_6,undefined1 param_7,int param_8,int param_9)

{
  *(undefined1 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_1001e12c;
  *(undefined ***)this = &PTR_FUN_1001e16c;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_1001e14c;
  FUN_1000bc60(this,param_1,param_2,param_3,param_4,param_5,param_6,param_7,param_8,param_9);
  return (undefined4 *)this;
}


// 1000c2d0 FUN_1000c2d0

int * __fastcall FUN_1000c2d0(int *param_1)

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
  param_1[6] = (int)&PTR_LAB_1001e12c;
  param_1[0xe] = 0;
  param_1[0x10] = 0;
  *param_1 = (int)&PTR_FUN_1001e16c;
  param_1[6] = (int)&PTR_LAB_1001e14c;
  param_1[7] = (int)param_1;
  param_1[8] = (int)&DAT_1001e37c;
  *(undefined1 *)(param_1 + 0xd) = 0;
  *(undefined1 *)(param_1 + 0xf) = 0;
  param_1[0x11] = 0;
  pcVar2 = "0";
  do {
    pcVar3 = pcVar2;
    pcVar2 = pcVar3 + 1;
  } while (*pcVar3 != '\0');
  param_1[10] = (int)(pcVar3 + -0x1001e37b);
  _Dst = (void *)FUN_100099a0(pcVar3 + -0x1001e37b);
  param_1[9] = (int)_Dst;
  _memcpy(_Dst,(void *)param_1[8],param_1[10]);
  dVar5 = _atof((char *)param_1[9]);
  param_1[0xb] = (int)(float)dVar5;
  uVar4 = FUN_1000f220(extraout_ECX,extraout_EDX);
  puVar1 = PTR_DAT_10027748;
  param_1[0xc] = (int)uVar4;
  *(undefined1 *)(param_1 + 2) = 0;
  param_1[3] = (int)&DAT_1001d2ac;
  param_1[4] = (int)puVar1;
  param_1[5] = 2;
  param_1[1] = (int)DAT_10058638;
  DAT_10058638 = param_1;
  if (DAT_1005863c != 0) {
    (**(code **)(*param_1 + 0x2c))();
  }
  *param_1 = (int)&PTR_FUN_1001e334;
  param_1[6] = (int)&PTR_LAB_1001e314;
  return param_1;
}


// 1000c3f0 FUN_1000c3f0

undefined4 * __thiscall FUN_1000c3f0(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_1001e16c;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_1001e14c;
  if (*(int *)((int)this + 0x24) != 0) {
    FUN_10009a00(*(int *)((int)this + 0x24));
    *(undefined4 *)((int)this + 0x24) = 0;
  }
  *(undefined ***)this = &PTR_LAB_1001e0b8;
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (undefined4 *)this;
}


// 1000c440 FUN_1000c440

void __thiscall FUN_1000c440(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000c49b;
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
LAB_1000c49b:
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


// 1000c4d0 FUN_1000c4d0

int __thiscall FUN_1000c4d0(void *this,int param_1,int *param_2)

{
  int iVar1;
  void *pvVar2;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_1000c440(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar2 = (void *)(*this + param_1 * 0x10);
    FUN_1000b3c0((void *)((int)pvVar2 + 0x10),pvVar2,iVar1 * 0x10);
  }
                    /* WARNING: Load size is inaccurate */
  pvVar2 = (void *)(param_1 * 0x10 + *this);
  if (pvVar2 != (void *)0x0) {
    FUN_1000c970(pvVar2,param_2);
  }
  return param_1;
}


// 1000c540 FUN_1000c540

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

int __thiscall FUN_1000c540(void *this,undefined4 param_1,void *param_2)

{
  int iVar1;
  char *pcVar2;
  int iVar3;
  int aiStack_1010 [4];
  char local_1000 [4092];
  undefined4 uStack_4;
  
  uStack_4 = 0x1000c54a;
  if ((*(byte *)((int)this + 0x20) & 4) == 0) {
    if (*(code **)((int)this + 0x1c) != (code *)0x0) {
      iVar1 = (**(code **)((int)this + 0x1c))(param_1,local_1000);
      if (0 < iVar1) {
        pcVar2 = local_1000;
        iVar3 = iVar1;
        do {
          FUN_1000c930(aiStack_1010,pcVar2);
          FUN_1000c4d0(param_2,*(int *)((int)param_2 + 0xc),aiStack_1010);
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


// 1000c620 FUN_1000c620

uint __fastcall FUN_1000c620(byte *param_1)

{
  uint uVar1;
  uint uVar2;
  uint uVar3;
  
  uVar3 = 0;
  uVar2 = 0;
  uVar1 = _toupper((uint)*param_1);
  while (uVar1 != 0) {
    uVar3 = *(uint *)(&DAT_100277b0 + (uVar1 ^ uVar2) * 4);
    uVar1 = _toupper((uint)param_1[1]);
    param_1 = param_1 + 2;
    if (uVar1 == 0) break;
    uVar2 = *(uint *)(&DAT_100277b0 + (uVar1 ^ uVar3) * 4);
    uVar1 = _toupper((uint)*param_1);
  }
  return uVar3 << 8 | uVar2;
}


// 1000c690 FUN_1000c690

uint __fastcall FUN_1000c690(byte *param_1,int param_2)

{
  uint uVar1;
  uint uVar2;
  
  uVar1 = 0;
  uVar2 = 0;
  if (param_2 != 0) {
    do {
      uVar1 = *(uint *)(&DAT_100277b0 + (*param_1 ^ uVar2) * 4);
      if (param_2 == 1) break;
      param_2 = param_2 + -2;
      uVar2 = *(uint *)(&DAT_100277b0 + (param_1[1] ^ uVar1) * 4);
      param_1 = param_1 + 2;
    } while (param_2 != 0);
  }
  return uVar1 << 8 | uVar2;
}


// 1000c6e0 FUN_1000c6e0

undefined1 * __fastcall FUN_1000c6e0(undefined4 *param_1)

{
  undefined1 *puVar1;
  
  puVar1 = &DAT_1001d2ac;
  if (param_1[3] != 0) {
    puVar1 = (undefined1 *)*param_1;
  }
  return puVar1;
}


// 1000c6f0 FUN_1000c6f0

void __thiscall FUN_1000c6f0(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000c74b;
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
LAB_1000c74b:
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


// 1000c780 FUN_1000c780

void __thiscall FUN_1000c780(void *this,void *param_1,int param_2)

{
  size_t _Size;
  void *_Dst;
  
  if (param_1 == (void *)0x0) {
    param_2 = 0;
  }
  *(int *)((int)this + 0xc) = param_2;
  if (*(int *)((int)this + 4) < param_2) {
    FUN_1000c6f0(this,param_2 - *(int *)((int)this + 4));
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


// 1000c7f0 FUN_1000c7f0

undefined4 __fastcall FUN_1000c7f0(undefined4 *param_1)

{
  if (param_1[3] == 0) {
    param_1[3] = 1;
    if ((int)param_1[1] < 1) {
      FUN_1000c6f0(param_1,1 - param_1[1]);
      if ((int)param_1[1] < 1) {
        param_1[3] = param_1[1];
      }
    }
    *(undefined1 *)*param_1 = 0;
  }
  return *param_1;
}


// 1000c830 FUN_1000c830

void * __thiscall FUN_1000c830(void *this,char *param_1)

{
  int iVar1;
  
  if (param_1 != (char *)0x0) {
    iVar1 = FUN_1000b1a0(param_1);
    FUN_1000c780(this,param_1,iVar1 + 1);
    return this;
  }
  FUN_1000c780(this,(void *)0x0,0);
  return this;
}


// 1000c870 FUN_1000c870

int * __thiscall FUN_1000c870(void *this,char *param_1)

{
  size_t _Size;
  int iVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  
  if (*(int *)((int)this + 0xc) == 0) {
    iVar2 = 0;
  }
  else {
    iVar2 = *(int *)((int)this + 0xc) + -1;
  }
  _Size = FUN_1000b1a0(param_1);
  if ((int)(_Size + iVar2) < 1) {
    iVar3 = 0;
  }
  else {
    iVar3 = _Size + iVar2 + 1;
  }
  iVar1 = *(int *)((int)this + 4);
  *(int *)((int)this + 0xc) = iVar3;
  if (iVar1 < iVar3) {
    FUN_1000c6f0(this,iVar3 - iVar1);
    iVar1 = *(int *)((int)this + 4);
    if (iVar1 < iVar3) {
      *(int *)((int)this + 0xc) = iVar1;
    }
  }
  iVar3 = *(int *)((int)this + 0xc);
  if (iVar3 == 0) {
    iVar4 = 0;
  }
  else {
    iVar4 = iVar3 + -1;
  }
  if (iVar4 - iVar2 < (int)_Size) {
    _Size = iVar4 - iVar2;
  }
  if (iVar3 == 0) {
    *(undefined4 *)((int)this + 0xc) = 1;
    if (iVar1 < 1) {
      FUN_1000c6f0(this,1 - iVar1);
      if (*(int *)((int)this + 4) < 1) {
        *(int *)((int)this + 0xc) = *(int *)((int)this + 4);
      }
    }
                    /* WARNING: Load size is inaccurate */
    **this = 0;
  }
                    /* WARNING: Load size is inaccurate */
  _memcpy((void *)(*this + iVar2),param_1,_Size);
                    /* WARNING: Load size is inaccurate */
  *(undefined1 *)(iVar4 + *this) = 0;
  return (int *)this;
}


// 1000c930 FUN_1000c930

undefined4 * __thiscall FUN_1000c930(void *this,char *param_1)

{
  int iVar1;
  
  iVar1 = 0;
  *(undefined4 *)this = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined4 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  if (param_1 != (char *)0x0) {
    iVar1 = FUN_1000b1a0(param_1);
    iVar1 = iVar1 + 1;
  }
  FUN_1000c780(this,param_1,iVar1);
  return (undefined4 *)this;
}


// 1000c970 FUN_1000c970

undefined4 * __thiscall FUN_1000c970(void *this,int *param_1)

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
    if (pcVar2 == (char *)0x0) goto LAB_1000c9a3;
  }
  iVar1 = FUN_1000b1a0(pcVar2);
  iVar1 = iVar1 + 1;
LAB_1000c9a3:
  FUN_1000c780(this,pcVar2,iVar1);
  return (undefined4 *)this;
}


// 1000c9c0 FUN_1000c9c0

void __fastcall FUN_1000c9c0(undefined4 *param_1)

{
  *param_1 = 0;
  param_1[1] = 0;
  param_1[2] = 0;
  param_1[3] = 0;
  return;
}


// 1000c9d0 FUN_1000c9d0

void __thiscall FUN_1000c9d0(void *this,undefined4 param_1,undefined4 param_2)

{
  *(undefined4 *)((int)this + 4) = param_2;
  *(undefined4 *)this = param_1;
  *(void **)((int)this + 8) = DAT_10058698;
  DAT_10058698 = this;
  return;
}


// 1000c9f0 FUN_1000c9f0

undefined4 __cdecl FUN_1000c9f0(byte *param_1,undefined4 *param_2)

{
  int *piVar1;
  byte bVar2;
  byte *pbVar3;
  int iVar4;
  undefined4 uVar5;
  byte *pbVar6;
  undefined4 *puVar7;
  bool bVar8;
  
  puVar7 = DAT_10058698;
  if (DAT_10058698 != (undefined4 *)0x0) {
    do {
      pbVar3 = (byte *)puVar7[1];
      pbVar6 = param_1;
      do {
        bVar2 = *pbVar3;
        bVar8 = bVar2 < *pbVar6;
        if (bVar2 != *pbVar6) {
LAB_1000ca25:
          iVar4 = (1 - (uint)bVar8) - (uint)(bVar8 != 0);
          goto LAB_1000ca2a;
        }
        if (bVar2 == 0) break;
        bVar2 = pbVar3[1];
        bVar8 = bVar2 < pbVar6[1];
        if (bVar2 != pbVar6[1]) goto LAB_1000ca25;
        pbVar3 = pbVar3 + 2;
        pbVar6 = pbVar6 + 2;
      } while (bVar2 != 0);
      iVar4 = 0;
LAB_1000ca2a:
      if (iVar4 == 0) {
        if (param_2 != (undefined4 *)0x0) {
          *param_2 = 0;
        }
                    /* WARNING: Could not recover jumptable at 0x1000ca5a. Too many branches */
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


// 1000ca70 FUN_1000ca70

undefined * FUN_1000ca70(void)

{
  return FUN_1000c9f0;
}


// 1000ca80 FUN_1000ca80

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1000ca80(int param_1,int param_2)

{
  int iVar1;
  int iVar2;
  
  if (DAT_100586ac == '\0') {
    iVar2 = 0;
    DAT_100586ac = '\x01';
    iVar1 = DAT_100586a4;
    if (0 < param_2) {
      do {
        if (iVar1 == 0) {
          iVar1 = (**(code **)(param_1 + iVar2 * 4))("VEngineCvar007",0);
          _DAT_100586a0 = iVar1;
          DAT_100586a4 = iVar1;
        }
        if (DAT_100586a8 == 0) {
          DAT_100586a8 = (**(code **)(param_1 + iVar2 * 4))("VProcessUtils001",0);
          iVar1 = DAT_100586a4;
        }
        iVar2 = iVar2 + 1;
      } while (iVar2 < param_2);
    }
  }
  return;
}


// 1000cb30 FUN_1000cb30

undefined4 * __thiscall
FUN_1000cb30(void *this,undefined1 param_1,char *param_2,int param_3,int param_4)

{
  int *piVar1;
  char cVar2;
  char *pcVar3;
  int iVar4;
  int iVar5;
  
  *(undefined1 *)((int)this + 4) = param_1;
  *(undefined ***)this = &PTR_FUN_1001e3b0;
  *(char **)((int)this + 8) = param_2;
  *(int *)((int)this + 0x10) = param_3;
  iVar4 = FUN_1000b1a0(param_2);
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
      iVar4 = FUN_1000b1a0(pcVar3);
      *piVar1 = iVar4;
      if (*(int *)((int)this + 0x14) < iVar4) {
        *(int *)((int)this + 0x14) = iVar4;
      }
      iVar5 = iVar5 + 1;
    } while (iVar5 < param_3);
  }
  return (undefined4 *)this;
}


// 1000cbc0 FUN_1000cbc0

uint __thiscall FUN_1000cbc0(void *this,byte *param_1,undefined4 *param_2)

{
  undefined4 uVar1;
  uint in_EAX;
  int iVar2;
  
  iVar2 = 0;
  if (0 < *(int *)((int)this + 0x10)) {
    do {
      in_EAX = FUN_1000b1d0(param_1,*(byte **)((int)this +
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


// 1000cc30 FUN_1000cc30

undefined1 __thiscall FUN_1000cc30(void *this,int param_1)

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


// 1000cc90 FUN_1000cc90

uint __thiscall FUN_1000cc90(void *this,int param_1,int *param_2)

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
    FUN_1000cc30(this,*param_2 + param_1);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
  }
  iVar2 = *(int *)((int)this + 0x1c) - *(int *)((int)this + 0xc);
  if (iVar2 < *param_2) {
    *param_2 = iVar2;
  }
  return (uint)(*param_2 != 0);
}


// 1000cd00 FUN_1000cd00

uint __thiscall FUN_1000cd00(void *this,int param_1)

{
  uint in_EAX;
  
  if (((*(byte *)((int)this + 0x14) & 1) == 0) && ((*(byte *)((int)this + 0x15) & 8) == 0)) {
    if ((*(int *)((int)this + 0x10) < *(int *)((int)this + 0x20)) ||
       (in_EAX = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1,
       *(int *)((int)this + 4) < (int)in_EAX)) {
      in_EAX = (**(code **)((int)this + 0x28))(param_1);
      if ((char)in_EAX == '\0') {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 1;
        goto LAB_1000cd36;
      }
    }
    return CONCAT31((int3)(in_EAX >> 8),1);
  }
LAB_1000cd36:
  return in_EAX & 0xffffff00;
}


// 1000cd50 FUN_1000cd50

void __fastcall FUN_1000cd50(int *param_1)

{
  undefined4 uVar1;
  
  if (param_1[7] < param_1[4]) {
    if (((*(byte *)((int)param_1 + 0x15) & 8) == 0) && ((*(byte *)(param_1 + 5) & 1) == 0)) {
      uVar1 = FUN_1000cd00(param_1,1);
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


// 1000cda0 FUN_1000cda0

void __thiscall FUN_1000cda0(void *this,undefined4 param_1)

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


// 1000cdf0 FUN_1000cdf0

void __thiscall FUN_1000cdf0(void *this,int param_1)

{
  undefined4 uVar1;
  int iVar2;
  int iVar3;
  
  iVar2 = param_1 + 1;
  iVar3 = iVar2;
  if (((*(int *)((int)this + 8) < 0) && (iVar3 = param_1, (*(byte *)((int)this + 0x15) & 2) != 0))
     && (*(int *)((int)this + 4) < iVar2)) {
    FUN_1000cda0(this,0);
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


// 1000ce50 FUN_1000ce50

void __thiscall FUN_1000ce50(void *this,void *param_1,size_t param_2)

{
  char cVar1;
  
  cVar1 = FUN_1000cc30(this,param_2);
  if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    _memcpy(param_1,(void *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this),
            param_2);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
  }
  return;
}


// 1000ce90 FUN_1000ce90

void __fastcall FUN_1000ce90(int *param_1)

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


// 1000cf00 FUN_1000cf00

int __thiscall FUN_1000cf00(void *this,int param_1)

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


// 1000cfa0 FUN_1000cfa0

int __fastcall FUN_1000cfa0(int *param_1)

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
    local_8 = FUN_1000cf00(param_1,0);
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
      if (iVar6 < 1) goto LAB_1000d0a5;
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
LAB_1000d0a5:
      local_8 = local_8 + iVar6;
    }
  }
  if (local_8 != iVar1) {
    return (local_8 - iVar1) + 1;
  }
  return 0;
}


// 1000d100 FUN_1000d100

void __thiscall FUN_1000d100(void *this,void *param_1,size_t param_2)

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
    FUN_1000cd50((int *)this);
  }
  return;
}


// 1000d170 FUN_1000d170

uint __thiscall FUN_1000d170(void *this,int param_1)

{
  uint in_EAX;
  int iVar1;
  
  if (*(int *)((int)this + 8) < 0) {
    if ((*(byte *)((int)this + 0x15) & 2) == 0) {
      return in_EAX & 0xffffff00;
    }
    FUN_1000cda0(this,0);
  }
  iVar1 = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1;
  if (*(int *)((int)this + 4) < iVar1) {
    do {
      iVar1 = FUN_1000c6f0(this,1);
    } while (*(int *)((int)this + 4) <
             (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1);
  }
  return CONCAT31((int3)((uint)iVar1 >> 8),1);
}


// 1000d1c0 FUN_1000d1c0

void __thiscall FUN_1000d1c0(void *this,int param_1,int param_2)

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
      uVar1 = FUN_1000cd00(this,1);
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


// 1000d240 FUN_1000d240

int * __thiscall FUN_1000d240(void *this,int param_1,int param_2,byte param_3)

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
    FUN_1000cd50((int *)this);
  }
  *(undefined1 **)((int)this + 0x24) = &LAB_1000c3c0;
  *(code **)((int)this + 0x28) = FUN_1000d170;
  return (int *)this;
}


// 1000d2c0 FUN_1000d2c0

void __thiscall FUN_1000d2c0(void *this,char *param_1)

{
  int iVar1;
  char *pcVar2;
  size_t sVar3;
  
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    if (param_1 != (char *)0x0) {
      iVar1 = FUN_1000b1a0(param_1);
      FUN_1000d100(this,param_1,iVar1 + 1);
      return;
    }
    FUN_10001c40(this,0);
    return;
  }
  if (param_1 != (char *)0x0) {
    if (((*(byte *)((int)this + 0x15) & 0x10) == 0) && (0 < *(int *)((int)this + 0x18))) {
                    /* WARNING: Load size is inaccurate */
      if ((*(int *)((int)this + 0x10) != 0) &&
         (*(char *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + -1 + *this) == '\n')
         ) {
        FUN_10002760((int *)this);
      }
      pcVar2 = _strchr(param_1,10);
      while (pcVar2 != (char *)0x0) {
        FUN_1000d100(this,param_1,(size_t)(pcVar2 + (1 - (int)param_1)));
        param_1 = pcVar2 + 1;
        if (pcVar2[1] == '\0') break;
        if ((*(byte *)((int)this + 0x15) & 0x10) == 0) {
          iVar1 = *(int *)((int)this + 0x18);
        }
        else {
          iVar1 = 0;
        }
        while (iVar1 = iVar1 + -1, -1 < iVar1) {
          FUN_10001c40(this,9);
        }
        pcVar2 = _strchr(param_1,10);
      }
    }
    sVar3 = FUN_1000b1a0(param_1);
    if (sVar3 != 0) {
      FUN_1000d100(this,param_1,sVar3);
    }
  }
  return;
}


// 1000d3b0 FUN_1000d3b0

void __cdecl FUN_1000d3b0(void *param_1,char *param_2)

{
  char local_800 [2048];
  
  FUN_1000b480(local_800,0x800,param_2,&stack0x0000000c);
  FUN_1000d2c0(param_1,local_800);
  return;
}


// 1000d3f0 FUN_1000d3f0

void __thiscall FUN_1000d3f0(void *this,undefined1 *param_1,int param_2)

{
  int iVar1;
  
  if (*(char *)((int)this + 0x14) != '\0') {
    *param_1 = 0;
    return;
  }
  if (param_2 == 0) {
    param_2 = 0x7fffffff;
  }
  iVar1 = FUN_1000cfa0((int *)this);
  if ((*(byte *)((int)this + 0x15) & 1) != 0) {
    FUN_1000ce90((int *)this);
  }
  if (iVar1 == 0) {
    *param_1 = 0;
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
    return;
  }
  if (param_2 < iVar1) {
    FUN_1000ce50(this,param_1,param_2 - 1);
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
    FUN_1000ce50(this,param_1,iVar1 - 1);
    param_1[iVar1 + -1] = 0;
  }
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    FUN_10004990((int *)this);
  }
  return;
}


// 1000d4b0 FUN_1000d4b0

int __thiscall FUN_1000d4b0(void *this,char *param_1,int param_2)

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
        FUN_1000ce90((int *)this);
        goto LAB_1000d72b;
      }
      if (cVar5 != '%') {
        if ((*(byte *)((int)this + 0x14) & 2) != 0) {
          return local_8;
        }
        cVar4 = FUN_1000cc30(this,1);
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
        if (cVar4 == '\0') {
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        if (cVar5 != *(char *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this)) {
          return local_8;
        }
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
        goto LAB_1000d72b;
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
LAB_1000d756:
          *puVar2 = 0;
          return local_8;
        }
        cVar5 = FUN_1000cc30(this,1);
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
        if (cVar5 == '\0') goto LAB_1000d756;
                    /* WARNING: Load size is inaccurate */
        *puVar2 = *(undefined1 *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc));
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
        local_8 = local_8 + 1;
        break;
      case 'd':
      case 'i':
        puVar9 = *(ulong **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000cc90(this,0,&param_2);
        if ((char)uVar6 == '\0') {
LAB_1000d767:
          *puVar9 = 0;
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        uVar7 = _strtol((char *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc)),
                        &local_4,10);
        goto LAB_1000d5f6;
      default:
        goto switchD_1000d572_caseD_65;
      case 'f':
        pfVar3 = *(float **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000cc90(this,0,&param_2);
        if ((char)uVar6 == '\0') {
          *pfVar3 = 0.0;
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        fVar11 = (float10)FUN_1000fc21((byte *)((*this - *(int *)((int)this + 0x20)) +
                                               *(int *)((int)this + 0xc)),&local_4);
        *pfVar3 = (float)fVar11;
        goto LAB_1000d5fc;
      case 's':
        puVar1 = (undefined4 *)(iVar10 + 4);
        iVar10 = iVar10 + 4;
        FUN_1000d3f0(this,(undefined1 *)*puVar1,0);
        local_8 = local_8 + 1;
        break;
      case 'u':
        puVar9 = *(ulong **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000cc90(this,0,&param_2);
        if ((char)uVar6 == '\0') goto LAB_1000d767;
                    /* WARNING: Load size is inaccurate */
        uVar7 = _strtoul((char *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc)),
                         &local_4,10);
LAB_1000d5f6:
        *puVar9 = uVar7;
LAB_1000d5fc:
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
        uVar6 = FUN_1000cc90(this,0,&param_2);
        if ((char)uVar6 == '\0') goto LAB_1000d767;
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
LAB_1000d72b:
      cVar5 = *pcVar8;
      param_1 = pcVar8;
    } while (cVar5 != '\0');
  }
switchD_1000d572_caseD_65:
  return local_8;
}


// 1000d7d0 FUN_1000d7d0

void __cdecl FUN_1000d7d0(void *param_1,char *param_2)

{
  FUN_1000d4b0(param_1,param_2,(int)&stack0x0000000c);
  return;
}


// 1000d7f0 FUN_1000d7f0

void __fastcall FUN_1000d7f0(undefined4 *param_1)

{
  *param_1 = 0;
  param_1[1] = 0;
  param_1[2] = 0;
  param_1[3] = 0;
  param_1[4] = 0;
  param_1[5] = 0x10;
  param_1[6] = 0;
  param_1[7] = 0;
  return;
}


// 1000d810 FUN_1000d810

uint __thiscall FUN_1000d810(void *this,undefined4 param_1,int param_2,int param_3,int param_4)

{
  int iVar1;
  uint uVar2;
  LPVOID pvVar3;
  _SYSTEM_INFO local_24;
  
  *(undefined4 *)((int)this + 0x10) = param_1;
  *(uint *)((int)this + 0x14) = param_4 + 3U & 0xfffffffc;
  if (param_2 != 0) {
    *(int *)((int)this + 0x18) = param_2;
  }
  GetSystemInfo(&local_24);
  if (*(int *)((int)this + 0x18) == 0) {
    *(DWORD *)((int)this + 0x18) = local_24.dwPageSize;
  }
  else {
    *(DWORD *)((int)this + 0x18) =
         *(int *)((int)this + 0x18) + -1 + local_24.dwPageSize & ~(local_24.dwPageSize - 1);
  }
  uVar2 = *(int *)((int)this + 0x18) + -1 + *(int *)((int)this + 0x10) &
          ~(*(int *)((int)this + 0x18) - 1U);
  *(uint *)((int)this + 0x10) = uVar2;
  pvVar3 = VirtualAlloc((LPVOID)0x0,uVar2,0x2000,1);
  *(LPVOID *)((int)this + 0xc) = pvVar3;
  if (pvVar3 == (LPVOID)0x0) {
    uVar2 = (**(code **)(**(int **)g_pMemAlloc_exref + 0x8c))(0);
    return uVar2 & 0xffffff00;
  }
  *(LPVOID *)this = pvVar3;
  *(LPVOID *)((int)this + 4) = pvVar3;
  if (param_3 != 0) {
    uVar2 = *(int *)((int)this + 0x18) + -1 + param_3 & ~(*(int *)((int)this + 0x18) - 1U);
    pvVar3 = VirtualAlloc(pvVar3,uVar2,0x1000,4);
    if (pvVar3 == (LPVOID)0x0) {
      uVar2 = (**(code **)(**(int **)g_pMemAlloc_exref + 0x8c))(uVar2);
      return uVar2 & 0xffffff00;
    }
    *(int *)((int)this + 4) = *(int *)((int)this + 4) + uVar2;
    *(uint *)((int)this + 0x1c) = uVar2;
  }
  iVar1 = *(int *)((int)this + 0xc);
  *(int *)((int)this + 8) = *(int *)((int)this + 0x10) + iVar1;
  return CONCAT31((int3)((uint)iVar1 >> 8),iVar1 != 0);
}


// 1000d920 FUN_1000d920

uint __thiscall FUN_1000d920(void *this,int param_1)

{
  LPVOID lpAddress;
  SIZE_T dwSize;
  LPVOID lpAddress_00;
  
  lpAddress_00 = (LPVOID)(*(int *)((int)this + 0x18) + -1 + param_1 &
                         ~(*(int *)((int)this + 0x18) - 1U));
  lpAddress = *(LPVOID *)((int)this + 4);
  dwSize = (int)lpAddress_00 - (int)lpAddress;
  if (*(uint *)((int)this + 8) < (int)lpAddress + dwSize) {
LAB_1000d96f:
    return (uint)lpAddress & 0xffffff00;
  }
  if (lpAddress < lpAddress_00) {
    lpAddress = VirtualAlloc(lpAddress,dwSize,0x1000,4);
    if (lpAddress == (LPVOID)0x0) {
      lpAddress = (LPVOID)(**(code **)(**(int **)g_pMemAlloc_exref + 0x8c))(dwSize);
      goto LAB_1000d96f;
    }
  }
  else if (lpAddress_00 < lpAddress) {
                    /* WARNING: Load size is inaccurate */
    if (lpAddress_00 < *this) {
      Warning("ATTEMPTED TO DECOMMIT OWNED MEMORY STACK SPACE\n");
                    /* WARNING: Load size is inaccurate */
      lpAddress_00 = (LPVOID)(*this + -1 + *(int *)((int)this + 0x18) &
                             ~(*(int *)((int)this + 0x18) - 1U));
    }
    lpAddress = *(LPVOID *)((int)this + 4);
    if ((lpAddress_00 < lpAddress) &&
       (lpAddress = (LPVOID)((int)lpAddress - (int)lpAddress_00), lpAddress != (LPVOID)0x0)) {
      lpAddress = (LPVOID)VirtualFree(lpAddress_00,(SIZE_T)lpAddress,0x4000);
      *(LPVOID *)((int)this + 4) = lpAddress_00;
    }
  }
  *(LPVOID *)((int)this + 4) = lpAddress_00;
  return CONCAT31((int3)((uint)lpAddress >> 8),1);
}


// 1000d9d0 FUN_1000d9d0

void __fastcall FUN_1000d9d0(undefined4 *param_1)

{
  LPVOID lpAddress;
  
  lpAddress = (LPVOID)param_1[3];
  if (lpAddress != (LPVOID)0x0) {
    if (0 < param_1[1] - (int)lpAddress) {
      VirtualFree(lpAddress,param_1[1] - (int)lpAddress,0x4000);
      lpAddress = (LPVOID)param_1[3];
      param_1[1] = lpAddress;
      *param_1 = lpAddress;
    }
    if (lpAddress != (LPVOID)0x0) {
      VirtualFree(lpAddress,0,0x8000);
      param_1[3] = 0;
    }
  }
  return;
}


// 1000da20 FUN_1000da20

void __fastcall FUN_1000da20(int param_1)

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


// 1000da60 FUN_1000da60

void __fastcall FUN_1000da60(int param_1)

{
  if ((0 < *(int *)(param_1 + 0x10)) && (DAT_100599e8 != (code *)0x0)) {
    (*DAT_100599e8)("Memory leak: mempool blocks left in memory: %d\n",*(int *)(param_1 + 0x10));
  }
  FUN_1000da20(param_1);
  return;
}


// 1000da90 FUN_1000da90

void __fastcall FUN_1000da90(int *param_1)

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


// 1000db30 FUN_1000db30

uint * __thiscall FUN_1000db30(void *this,uint param_1)

{
  uint *puVar1;
  int iVar2;
  
                    /* WARNING: Load size is inaccurate */
  if (param_1 <= *this) {
    puVar1 = *(uint **)((int)this + 0xc);
    if (puVar1 != (uint *)0x0) {
LAB_1000db5f:
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
      FUN_1000da90((int *)this);
      puVar1 = *(uint **)((int)this + 0xc);
      if (puVar1 != (uint *)0x0) goto LAB_1000db5f;
    }
  }
  return (uint *)0x0;
}


// 1000db80 FUN_1000db80

uint * __thiscall
FUN_1000db80(void *this,uint param_1,uint param_2,uint param_3,char *param_4,uint param_5)

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
  FUN_1000da90((int *)this);
  return (uint *)this;
}


// 1000dc0c FUN_1000dc0c

void FUN_1000dc0c(void)

{
  return;
}


// 1000dc0d __cfltcvt_init

/* Library Function - Single Match
    __cfltcvt_init
   
   Library: Visual Studio 2005 Release */

void __cfltcvt_init(void)

{
  PTR_LAB_10027d50 = __cfltcvt;
  PTR_LAB_10027d54 = &LAB_1001003b;
  PTR_LAB_10027d58 = &LAB_1000fff9;
  PTR_LAB_10027d5c = &LAB_1001002d;
  PTR_LAB_10027d60 = &LAB_1000ffa3;
  PTR_LAB_10027d64 = __cfltcvt;
  PTR_LAB_10027d68 = __cfltcvt_l;
  PTR_LAB_10027d6c = __fassign_l;
  PTR_LAB_10027d70 = __cropzeros_l;
  PTR_LAB_10027d74 = __forcdecpt_l;
  return;
}


// 1000dc6d __fpmath

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __fpmath
   
   Library: Visual Studio 2005 Release */

void __cdecl __fpmath(int param_1)

{
  __cfltcvt_init();
  _DAT_100599f8 = __ms_p5_mp_test_fdiv();
  if (param_1 != 0) {
    __setdefaultprecision();
  }
  return;
}


// 1000dc8b type_info::~type_info

/* Library Function - Single Match
    public: virtual __thiscall type_info::~type_info(void)
   
   Library: Visual Studio 2005 Release */

void __thiscall type_info::~type_info(type_info *this)

{
  *(undefined ***)this = &PTR__scalar_deleting_destructor__1001e458;
  _Type_info_dtor(this);
  return;
}


// 1000dc99 type_info::`scalar_deleting_destructor'

/* Library Function - Single Match
    public: virtual void * __thiscall type_info::`scalar deleting destructor'(unsigned int)
   
   Library: Visual Studio 2005 Release */

void * __thiscall type_info::_scalar_deleting_destructor_(type_info *this,uint param_1)

{
  ~type_info(this);
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return this;
}


// 1000dce0 _memcpy

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
          goto switchD_1000dec3_caseD_2;
        case 3:
          goto switchD_1000dec3_caseD_3;
        }
        goto switchD_1000dec3_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_1000dec3_caseD_0;
      case 1:
        goto switchD_1000dec3_caseD_1;
      case 2:
        goto switchD_1000dec3_caseD_2;
      case 3:
        goto switchD_1000dec3_caseD_3;
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
              goto switchD_1000dec3_caseD_2;
            case 3:
              goto switchD_1000dec3_caseD_3;
            }
            goto switchD_1000dec3_caseD_1;
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
              goto switchD_1000dec3_caseD_2;
            case 3:
              goto switchD_1000dec3_caseD_3;
            }
            goto switchD_1000dec3_caseD_1;
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
              goto switchD_1000dec3_caseD_2;
            case 3:
              goto switchD_1000dec3_caseD_3;
            }
            goto switchD_1000dec3_caseD_1;
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
switchD_1000dec3_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_1000dec3_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_1000dec3_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_1000dec3_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_1005b5fc != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_1000dd3c_caseD_2;
      case 3:
        goto switchD_1000dd3c_caseD_3;
      }
      goto switchD_1000dd3c_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_1000dd3c_caseD_0;
    case 1:
      goto switchD_1000dd3c_caseD_1;
    case 2:
      goto switchD_1000dd3c_caseD_2;
    case 3:
      goto switchD_1000dd3c_caseD_3;
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
            goto switchD_1000dd3c_caseD_2;
          case 3:
            goto switchD_1000dd3c_caseD_3;
          }
          goto switchD_1000dd3c_caseD_1;
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
            goto switchD_1000dd3c_caseD_2;
          case 3:
            goto switchD_1000dd3c_caseD_3;
          }
          goto switchD_1000dd3c_caseD_1;
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
            goto switchD_1000dd3c_caseD_2;
          case 3:
            goto switchD_1000dd3c_caseD_3;
          }
          goto switchD_1000dd3c_caseD_1;
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
switchD_1000dd3c_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_1000dd3c_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_1000dd3c_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_1000dd3c_caseD_0:
  return _Dst;
}


// 1000e050 __alloca_probe_16

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


// 1000e066 __alloca_probe_8

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


// 1000e07c FUN_1000e07c

int __cdecl FUN_1000e07c(int param_1)

{
  int *piVar1;
  int *piVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  
  piVar1 = (int *)FUN_10010ed5(DAT_1005b5ec);
  piVar2 = (int *)FUN_10010ed5(DAT_1005b5e8);
  if (piVar1 <= piVar2) {
    uVar4 = ((int)piVar2 - (int)piVar1) + 4;
    if (3 < uVar4) {
      uVar3 = FUN_10009b30(piVar1);
      if (uVar3 < uVar4) {
        uVar4 = 0x800;
        if (uVar3 < 0x800) {
          uVar4 = uVar3;
        }
        if ((uVar4 + uVar3 < uVar3) ||
           (iVar5 = thunk_FUN_10009aa0(piVar1,uVar4 + uVar3), iVar5 == 0)) {
          if (uVar3 + 0x10 < uVar3) {
            return 0;
          }
          iVar5 = thunk_FUN_10009aa0(piVar1,uVar3 + 0x10);
          if (iVar5 == 0) {
            return 0;
          }
        }
        piVar2 = (int *)(iVar5 + ((int)piVar2 - (int)piVar1 >> 2) * 4);
        DAT_1005b5ec = FUN_10010e5e(iVar5);
      }
      iVar5 = FUN_10010e5e(param_1);
      *piVar2 = iVar5;
      DAT_1005b5e8 = FUN_10010e5e((int)(piVar2 + 1));
      return param_1;
    }
  }
  return 0;
}


// 1000e164 __onexit

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __onexit
   
   Library: Visual Studio 2005 Release */

_onexit_t __cdecl __onexit(_onexit_t _Func)

{
  _onexit_t p_Var1;
  
  FUN_100115fe();
  p_Var1 = (_onexit_t)FUN_1000e07c((int)_Func);
  FUN_1000e19a();
  return p_Var1;
}


// 1000e19a FUN_1000e19a

void FUN_1000e19a(void)

{
  FUN_10011607();
  return;
}


// 1000e1a0 _atexit

/* Library Function - Single Match
    _atexit
   
   Library: Visual Studio 2005 Release */

int __cdecl _atexit(_func_4879 *param_1)

{
  _onexit_t p_Var1;
  
  p_Var1 = __onexit((_onexit_t)param_1);
  return (p_Var1 != (_onexit_t)0x0) - 1;
}


// 1000e1b2 __vsnprintf_l

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
    pDVar1 = FUN_10012790();
    *pDVar1 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
    iVar2 = FUN_10011c97(&local_24,(byte *)_Format,_Locale,(wchar_t *)_ArgList);
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
    pDVar1 = FUN_10012790();
    *pDVar1 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  return iVar2;
}


// 1000e263 __vsnprintf

/* Library Function - Single Match
    __vsnprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl __vsnprintf(char *_Dest,size_t _Count,char *_Format,va_list _Args)

{
  int iVar1;
  
  iVar1 = __vsnprintf_l(_Dest,_Count,_Format,(_locale_t)0x0,_Args);
  return iVar1;
}


// 1000e280 __alloca_probe

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


// 1000e2ab _rand

/* Library Function - Single Match
    _rand
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release, Visual Studio 2008 Release */

int __cdecl _rand(void)

{
  _ptiddata p_Var1;
  uint uVar2;
  
  p_Var1 = __getptd();
  uVar2 = p_Var1->_holdrand * 0x343fd + 0x269ec3;
  p_Var1->_holdrand = uVar2;
  return uVar2 >> 0x10 & 0x7fff;
}


// 1000e2d0 _memset

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
  if ((((char)_Val == '\0') && (0xff < _Size)) && (DAT_1005b5fc != 0)) {
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


// 1000e350 _strrchr

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


// 1000e390 _strchr

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


// 1000e490 FUN_1000e490

void FUN_1000e490(void)

{
  ushort in_FPUControlWord;
  float10 in_ST0;
  float10 in_ST1;
  
  if ((DAT_1005b5f8 != 0) && ((MXCSR & 0x1f80) == 0x1f80 && (in_FPUControlWord & 0x7f) == 0x7f)) {
    FUN_100128c0();
    return;
  }
  FUN_1000e4ed(SUB84((double)in_ST1,0),(uint)((ulonglong)(double)in_ST1 >> 0x20),
               SUB84((double)in_ST0,0),(uint)((ulonglong)(double)in_ST0 >> 0x20));
  return;
}


// 1000e4ed FUN_1000e4ed
// diperbaiki: mode=decompile dengan parameter stack manual

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined1  [10] __cdecl FUN_1000e4ed(byte stack_args [20])

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
    auStack_e._6_4_ = 0x1000e501;
    in_EAX = FUN_10013875();
    uVar1 = extraout_ECX;
  }
  bVar3 = (uVar1 & 0x7ff00000) == 0x7ff00000;
  if (bVar3) {
    if ((in_EAX & 0xfffff) == 0 && stack_args._8_4_ == 0) {
      auStack_e._6_4_ = 0x1000e5ca;
      __fload_withFB(0,(int)stack_args);
      iVar2 = extraout_ECX_01;
      fVar5 = extraout_ST0_01;
      fVar6 = extraout_ST1_01;
      goto LAB_1000e5ca;
    }
    auStack_e._6_4_ = 0x1000e58d;
    __fload_withFB(0,(int)stack_args);
    fVar5 = extraout_ST0_00;
    fVar6 = extraout_ST1_00;
    if ((stack_args[0xe] & 8) == 0) {
      iVar2 = extraout_ECX_00 + 1;
      goto LAB_1000e5ca;
    }
LAB_1000e597:
    auVar4 = (undefined1  [10])(fVar5 + fVar6);
  }
  else {
    auStack_e._6_4_ = 0x1000e51c;
    uVar1 = __fload_withFB(uVar1 & 0x7ff00000,(int)stack_args);
    if (!bVar3) {
      if (((uVar1 & 0x7ff00000) != 0) ||
         ((CONCAT12(stack_args[6],stack_args._4_2_) & 0xfffff) != 0 || stack_args._0_4_ != 0)) {
        if ((stack_args[7] & 0x80) != 0) {
          auStack_e._6_4_ = 0x1000e694;
          FUN_1000e6b2();
          auVar4 = _DAT_10028030;
          if (extraout_CL_01 == '\0') goto LAB_1000e565;
        }
        auStack_e._6_4_ = 0x1000e541;
        auVar4 = FUN_10013860();
        if (extraout_CL == '\x01') {
          auVar4 = (undefined1  [10])-(float10)auVar4;
        }
        if (DAT_100599f4 != 0) {
          return auVar4;
        }
        auVar4 = (undefined1  [10])
                 FUN_10013949(&DAT_10027cc8,0x1d,unaff_retaddr,stack_args._0_4_,
                              CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)),
                              stack_args._8_4_,
                              CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)))
        ;
        return auVar4;
      }
      if ((CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)) & 0x7fffffff) == 0
          && stack_args._8_4_ == 0) {
        auVar4 = (undefined1  [10])FUN_100138fe();
        return auVar4;
      }
      auStack_e._6_4_ = 0x1000e64c;
      FUN_1000e6b2();
      if ((stack_args._15_4_ & 0x80) == 0) {
        if ((stack_args[7] >> 7 & extraout_CL_00) == 0) {
          return (undefined1  [10])(float10)0;
        }
        auVar4 = (undefined1  [10])FUN_100138fe();
        return auVar4;
      }
      auVar4 = _DAT_10027fd0;
      if ((stack_args[7] >> 7 & extraout_CL_00) != 0) {
        auVar4 = (undefined1  [10])-(float10)_DAT_10027fd0;
      }
      goto LAB_1000e565;
    }
    iVar2 = 0;
    fVar5 = extraout_ST0;
    fVar6 = extraout_ST1;
LAB_1000e5ca:
    if (((CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)) & 0x7ff00000) ==
         0x7ff00000) &&
       ((CONCAT12(stack_args[6],stack_args._4_2_) & 0xfffff) != 0 || stack_args._0_4_ != 0)) {
      if ((stack_args[6] & 8) != 0) goto LAB_1000e597;
    }
    else if (iVar2 == 0) {
      pdVar7 = local_78;
      uStack_64 = 0x1000e5f8;
      auStack_e = (undefined1  [10])in_ST7;
      iVar2 = FUN_10013a50(SUB84((double)fVar5,0),(int)((ulonglong)(double)fVar5 >> 0x20),
                           SUB84((double)fVar6,0),(int)((ulonglong)(double)fVar6 >> 0x20),local_78);
      auVar4 = (undefined1  [10])(float10)*pdVar7;
      if (iVar2 == 0) {
        return (undefined1  [10])(float10)*pdVar7;
      }
      goto LAB_1000e565;
    }
    auVar4 = (undefined1  [10])(fVar5 + fVar6);
  }
LAB_1000e565:
  if (DAT_100599f4 != 0) {
    return auVar4;
  }
  auStack_e._6_4_ = 0x1000e582;
  auVar4 = (undefined1  [10])
           FUN_10013800(&DAT_10027cc8,0x1d,in_FPUControlWord,unaff_retaddr,stack_args._0_4_,
                        CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)),
                        stack_args._8_4_,
                        CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)));
  return auVar4;
}
// 1000e6b2 FUN_1000e6b2

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_1000e6b2(void)

{
  float10 in_ST0;
  
  if (ROUND(in_ST0) == in_ST0) {
    return;
  }
  return;
}


// 1000e6da __CRT_INIT@12

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
        _DAT_10059d54 = DVar2 * 0x100 + DVar3;
        DAT_10059d4c = DVar1;
        _DAT_10059d50 = uVar10;
        DAT_10059d58 = DVar2;
        _DAT_10059d5c = DVar3;
        bVar4 = FUN_10009b50();
        if (CONCAT31(extraout_var,bVar4) != 0) {
          iVar7 = __mtinit();
          if (iVar7 != 0) {
            __RTC_Initialize();
            DAT_1005b600 = GetCommandLineA();
            DAT_10059a00 = ___crtGetEnvironmentStringsA();
            iVar7 = __ioinit();
            if (-1 < iVar7) {
              iVar7 = __setargv();
              if (((-1 < iVar7) && (iVar7 = __setenvp(), -1 < iVar7)) &&
                 (iVar7 = __cinit(0), iVar7 == 0)) {
                DAT_100599fc = DAT_100599fc + 1;
                return 1;
              }
              __ioterm();
            }
            __mtterm();
          }
          FUN_10001130();
        }
      }
    }
  }
  else if (param_2 == 0) {
    if (0 < DAT_100599fc) {
      DAT_100599fc = DAT_100599fc + -1;
      if (DAT_10059d88 == 0) {
        __cexit();
      }
      if (param_3 == 0) {
        __ioterm();
        __mtterm();
        FUN_10001130();
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
    FUN_10010f4c();
    pDVar8 = (DWORD *)FUN_10009b00(1,0x214);
    if (pDVar8 != (DWORD *)0x0) {
      uVar12 = DAT_10027e40;
      pDVar13 = pDVar8;
      pcVar9 = (code *)FUN_10010ed5(DAT_10059d34);
      iVar7 = (*pcVar9)(uVar12,pDVar13);
      if (iVar7 != 0) {
        FUN_10010fbb((int)pDVar8,0);
        DVar11 = GetCurrentThreadId();
        pDVar8[1] = 0xffffffff;
        *pDVar8 = DVar11;
        return 1;
      }
      FUN_10009a00(pDVar8);
    }
  }
  return 0;
}


// 1000e8b3 ___DllMainCRTStartup

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x1000e945) */
/* WARNING: Removing unreachable block (ram,0x1000e8f2) */
/* WARNING: Removing unreachable block (ram,0x1000e972) */
/* Library Function - Single Match
    ___DllMainCRTStartup
   
   Library: Visual Studio 2005 Release */

int __fastcall ___DllMainCRTStartup(int param_1,int param_2,undefined4 param_3)

{
  int iVar1;
  int local_20;
  
  if (((param_2 == 0) && (DAT_100599fc == 0)) ||
     (((param_2 == 1 || (param_2 == 2)) &&
      (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)))) {
    local_20 = 0;
  }
  else {
    local_20 = FUN_1001443c();
    if ((param_2 == 1) && (local_20 == 0)) {
      FUN_1001443c();
      __CRT_INIT_12(param_3,0,param_1);
    }
    if (((param_2 == 0) || (param_2 == 3)) &&
       (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)) {
      local_20 = 0;
    }
  }
  return local_20;
}


// 1000e9a9 entry

void entry(undefined4 param_1,int param_2,int param_3)

{
  if (param_2 == 1) {
    ___security_init_cookie();
  }
  ___DllMainCRTStartup(param_3,param_2,param_1);
  return;
}


// 1000ea56 __freea

/* Library Function - Single Match
    __freea
   
   Library: Visual Studio 2005 Release */

void __cdecl __freea(void *_Memory)

{
  if ((_Memory != (void *)0x0) && (*(int *)((int)_Memory + -8) == 0xdddd)) {
    FUN_10009a00((int *)((int)_Memory + -8));
  }
  return;
}


// 1000ea71 _LocaleUpdate::_LocaleUpdate

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
    if ((*(undefined **)this != PTR_DAT_10028728) && ((p_Var2->_ownlocale & DAT_10028644) == 0)) {
      ptVar3 = ___updatetlocinfo();
      *(pthreadlocinfo *)this = ptVar3;
    }
    if ((*(undefined **)(this + 4) != PTR_DAT_10028548) &&
       ((*(uint *)(*(int *)(this + 8) + 0x70) & DAT_10028644) == 0)) {
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


// 1000eaf3 __tolower_l

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
LAB_1000eb52:
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
      pDVar4 = FUN_10012790();
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
    if (iVar3 == 0) goto LAB_1000eb52;
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


// 1000ec0a _tolower

/* Library Function - Single Match
    _tolower
   
   Library: Visual Studio 2005 Release */

int __cdecl _tolower(int _C)

{
  if (DAT_10059ec0 == 0) {
    if (_C - 0x41U < 0x1a) {
      return _C + 0x20;
    }
  }
  else {
    _C = __tolower_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 1000ed80 _memmove

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
          goto switchD_1000ef63_caseD_2;
        case 3:
          goto switchD_1000ef63_caseD_3;
        }
        goto switchD_1000ef63_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_1000ef63_caseD_0;
      case 1:
        goto switchD_1000ef63_caseD_1;
      case 2:
        goto switchD_1000ef63_caseD_2;
      case 3:
        goto switchD_1000ef63_caseD_3;
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
              goto switchD_1000ef63_caseD_2;
            case 3:
              goto switchD_1000ef63_caseD_3;
            }
            goto switchD_1000ef63_caseD_1;
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
              goto switchD_1000ef63_caseD_2;
            case 3:
              goto switchD_1000ef63_caseD_3;
            }
            goto switchD_1000ef63_caseD_1;
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
              goto switchD_1000ef63_caseD_2;
            case 3:
              goto switchD_1000ef63_caseD_3;
            }
            goto switchD_1000ef63_caseD_1;
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
switchD_1000ef63_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_1000ef63_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_1000ef63_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_1000ef63_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_1005b5fc != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_1000eddc_caseD_2;
      case 3:
        goto switchD_1000eddc_caseD_3;
      }
      goto switchD_1000eddc_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_1000eddc_caseD_0;
    case 1:
      goto switchD_1000eddc_caseD_1;
    case 2:
      goto switchD_1000eddc_caseD_2;
    case 3:
      goto switchD_1000eddc_caseD_3;
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
            goto switchD_1000eddc_caseD_2;
          case 3:
            goto switchD_1000eddc_caseD_3;
          }
          goto switchD_1000eddc_caseD_1;
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
            goto switchD_1000eddc_caseD_2;
          case 3:
            goto switchD_1000eddc_caseD_3;
          }
          goto switchD_1000eddc_caseD_1;
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
            goto switchD_1000eddc_caseD_2;
          case 3:
            goto switchD_1000eddc_caseD_3;
          }
          goto switchD_1000eddc_caseD_1;
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
switchD_1000eddc_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_1000eddc_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_1000eddc_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_1000eddc_caseD_0:
  return _Dst;
}


// 1000f0f0 _strncpy

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
        goto joined_r0x1000f13c;
      }
    }
    do {
      if (((uint)puVar5 & 3) == 0) {
        uVar4 = _Count >> 2;
        cVar3 = '\0';
        if (uVar4 == 0) goto LAB_1000f183;
        goto LAB_1000f1f9;
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
joined_r0x1000f1f5:
          while( true ) {
            uVar4 = uVar4 - 1;
            puVar5 = puVar5 + 1;
            if (uVar4 == 0) break;
LAB_1000f1f9:
            *puVar5 = 0;
          }
          cVar3 = '\0';
          _Count = _Count & 3;
          if (_Count != 0) goto LAB_1000f183;
          return _Dest;
        }
        if ((char)(uVar2 >> 8) == '\0') {
          *puVar5 = uVar2 & 0xff;
          goto joined_r0x1000f1f5;
        }
        if ((uVar2 & 0xff0000) == 0) {
          *puVar5 = uVar2 & 0xffff;
          goto joined_r0x1000f1f5;
        }
        if ((uVar2 & 0xff000000) == 0) {
          *puVar5 = uVar2;
          goto joined_r0x1000f1f5;
        }
      }
      *puVar5 = uVar2;
      puVar5 = puVar5 + 1;
      uVar4 = uVar4 - 1;
joined_r0x1000f13c:
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
LAB_1000f183:
        *(char *)puVar5 = cVar3;
        puVar5 = (uint *)((int)puVar5 + 1);
      }
      return _Dest;
    }
    _Count = _Count - 1;
  } while (_Count != 0);
  return _Dest;
}


// 1000f220 FUN_1000f220

ulonglong __fastcall FUN_1000f220(undefined4 param_1,undefined4 param_2)

{
  ulonglong uVar1;
  uint uVar2;
  float fVar3;
  float10 in_ST0;
  uint local_20;
  float fStack_1c;
  
  if (DAT_1005b5fc == 0) {
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


// 1000f43a _atol

/* Library Function - Single Match
    _atol
   
   Library: Visual Studio 2005 Release */

long __cdecl _atol(char *_Str)

{
  long lVar1;
  
  lVar1 = _strtol(_Str,(char **)0x0,10);
  return lVar1;
}


// 1000f44b __atof_l

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
    pDVar2 = FUN_10012790();
    *pDVar2 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 1000f4f4 _atof

/* Library Function - Single Match
    _atof
   
   Library: Visual Studio 2005 Release */

double __cdecl _atof(char *_String)

{
  double dVar1;
  
  dVar1 = __atof_l(_String,(_locale_t)0x0);
  return dVar1;
}


// 1000f502 FUN_1000f502

int __fastcall FUN_1000f502(int *param_1)

{
  int iVar1;
  int iVar2;
  
  iVar2 = (int)param_1 - *(int *)(*(int *)(*param_1 + -4) + 4);
  iVar1 = *(int *)(*(int *)(*param_1 + -4) + 8);
  if (iVar1 != 0) {
    iVar2 = iVar2 - *(int *)((int)param_1 - iVar1);
  }
  return iVar2;
}


// 1000f518 FindSITargetTypeInstance

/* Library Function - Single Match
    struct _s_RTTIBaseClassDescriptor const * __cdecl FindSITargetTypeInstance(struct
   _s_RTTICompleteObjectLocator const *,struct TypeDescriptor *,struct TypeDescriptor *)
   
   Library: Visual Studio 2005 Release */

_s_RTTIBaseClassDescriptor * __cdecl
FindSITargetTypeInstance
          (_s_RTTICompleteObjectLocator *param_1,TypeDescriptor *param_2,TypeDescriptor *param_3)

{
  uint uVar1;
  int iVar2;
  _s_RTTIBaseClassDescriptor *p_Var3;
  int *piVar4;
  _s_RTTICompleteObjectLocator *p_Var5;
  int in_EAX;
  int iVar6;
  uint uVar7;
  
  uVar1 = *(uint *)(*(int *)(in_EAX + 0x10) + 8);
  uVar7 = 0;
  iVar2 = *(int *)(*(int *)(in_EAX + 0x10) + 0xc);
  if (uVar1 != 0) {
    do {
      p_Var3 = *(_s_RTTIBaseClassDescriptor **)(iVar2 + uVar7 * 4);
      if ((*(TypeDescriptor **)p_Var3 == param_2) ||
         (iVar6 = _strcmp((char *)(*(TypeDescriptor **)p_Var3 + 8),(char *)(param_2 + 8)),
         iVar6 == 0)) {
        do {
          uVar7 = uVar7 + 1;
          if (uVar1 <= uVar7) {
            return (_s_RTTIBaseClassDescriptor *)0x0;
          }
          piVar4 = *(int **)(iVar2 + uVar7 * 4);
          if ((*(byte *)(piVar4 + 5) & 4) != 0) {
            return (_s_RTTIBaseClassDescriptor *)0x0;
          }
          p_Var5 = (_s_RTTICompleteObjectLocator *)*piVar4;
          if (p_Var5 == param_1) {
            return p_Var3;
          }
          iVar6 = _strcmp((char *)(p_Var5 + 8),(char *)(param_1 + 8));
        } while (iVar6 != 0);
        return p_Var3;
      }
      uVar7 = uVar7 + 1;
    } while (uVar7 < uVar1);
  }
  return (_s_RTTIBaseClassDescriptor *)0x0;
}


// 1000f58d PMDtoOffset

/* Library Function - Single Match
    int __cdecl PMDtoOffset(void *,struct PMD const &)
   
   Library: Visual Studio 2005 Release */

int __cdecl PMDtoOffset(void *param_1,PMD *param_2)

{
  int iVar1;
  int *in_EAX;
  int iVar2;
  
  iVar1 = in_EAX[1];
  iVar2 = 0;
  if (-1 < iVar1) {
    iVar2 = iVar1 + *(int *)(*(int *)(iVar1 + (int)param_1) + in_EAX[2]);
  }
  return *in_EAX + iVar2;
}


// 1000f5ac FindMITargetTypeInstance

/* Library Function - Single Match
    struct _s_RTTIBaseClassDescriptor const * __cdecl FindMITargetTypeInstance(void *,struct
   _s_RTTICompleteObjectLocator const *,struct TypeDescriptor *,int,struct TypeDescriptor *)
   
   Library: Visual Studio 2005 Release */

_s_RTTIBaseClassDescriptor * __cdecl
FindMITargetTypeInstance
          (void *param_1,_s_RTTICompleteObjectLocator *param_2,TypeDescriptor *param_3,int param_4,
          TypeDescriptor *param_5)

{
  _s_RTTIBaseClassDescriptor _Var1;
  uint uVar2;
  int iVar3;
  _s_RTTIBaseClassDescriptor *p_Var4;
  int in_EAX;
  int iVar5;
  TypeDescriptor *pTVar6;
  PMD *unaff_EDI;
  _s_RTTIBaseClassDescriptor *local_18;
  _s_RTTIBaseClassDescriptor *local_14;
  uint local_10;
  uint local_c;
  uint local_8;
  
  local_c = 0xffffffff;
  uVar2 = *(uint *)(*(int *)(in_EAX + 0x10) + 8);
  iVar3 = *(int *)(*(int *)(in_EAX + 0x10) + 0xc);
  local_14 = (_s_RTTIBaseClassDescriptor *)0x0;
  local_18 = (_s_RTTIBaseClassDescriptor *)0x0;
  local_10 = 0;
  local_8 = 0;
  if (uVar2 != 0) {
    do {
      p_Var4 = *(_s_RTTIBaseClassDescriptor **)(iVar3 + local_8 * 4);
      if ((local_10 < local_8 - local_c) &&
         ((*(int *)p_Var4 == param_4 ||
          (iVar5 = _strcmp((char *)(*(int *)p_Var4 + 8),(char *)(param_4 + 8)), iVar5 == 0)))) {
        local_14 = p_Var4;
        if (local_18 != (_s_RTTIBaseClassDescriptor *)0x0) {
          if (((byte)p_Var4[0x14] & 3) != 0) {
            return (_s_RTTIBaseClassDescriptor *)0x0;
          }
          _Var1 = local_18[0x14];
          goto joined_r0x1000f663;
        }
        local_c = local_8;
        local_10 = *(uint *)(p_Var4 + 4);
      }
      if ((((*(_s_RTTICompleteObjectLocator **)p_Var4 == param_2) ||
           (iVar5 = _strcmp((char *)(*(_s_RTTICompleteObjectLocator **)p_Var4 + 8),
                            (char *)(param_2 + 8)), iVar5 == 0)) &&
          (pTVar6 = (TypeDescriptor *)PMDtoOffset(param_1,unaff_EDI), pTVar6 == param_3)) &&
         (local_18 = p_Var4, local_14 != (_s_RTTIBaseClassDescriptor *)0x0)) {
        if (local_10 < local_8 - local_c) {
          if (((byte)local_14[0x14] & 3) != 0) {
            return (_s_RTTIBaseClassDescriptor *)0x0;
          }
        }
        else {
          if (((byte)local_14[0x14] & 0x40) != 0) {
            return (_s_RTTIBaseClassDescriptor *)
                   ((uint)local_14 &
                   ~-(uint)((*(byte *)(*(int *)(*(int *)(*(int *)(local_14 + 0x18) + 0xc) +
                                               (local_8 - local_c) * 4) + 0x14) & 1) != 0));
          }
          if (local_c != 0) {
            return local_14;
          }
        }
        _Var1 = p_Var4[0x14];
joined_r0x1000f663:
        if (((byte)_Var1 & 1) != 0) {
          return (_s_RTTIBaseClassDescriptor *)0x0;
        }
        return local_14;
      }
      local_8 = local_8 + 1;
    } while (local_8 < uVar2);
  }
  return (_s_RTTIBaseClassDescriptor *)0x0;
}


// 1000f6ae FindVITargetTypeInstance

/* Library Function - Single Match
    struct _s_RTTIBaseClassDescriptor const * __cdecl FindVITargetTypeInstance(void *,struct
   _s_RTTICompleteObjectLocator const *,struct TypeDescriptor *,int,struct TypeDescriptor *)
   
   Library: Visual Studio 2005 Release */

_s_RTTIBaseClassDescriptor * __cdecl
FindVITargetTypeInstance
          (void *param_1,_s_RTTICompleteObjectLocator *param_2,TypeDescriptor *param_3,int param_4,
          TypeDescriptor *param_5)

{
  int iVar1;
  uint uVar2;
  _s_RTTIBaseClassDescriptor *p_Var3;
  uint uVar4;
  bool bVar5;
  byte bVar6;
  int in_EAX;
  int iVar7;
  int iVar8;
  TypeDescriptor *pTVar9;
  PMD *unaff_EDI;
  _s_RTTIBaseClassDescriptor *p_Var10;
  bool bVar11;
  _s_RTTIBaseClassDescriptor *local_24;
  _s_RTTIBaseClassDescriptor *local_20;
  int local_1c;
  _s_RTTIBaseClassDescriptor *local_18;
  uint local_14;
  uint local_10;
  uint local_c;
  
  iVar1 = *(int *)(*(int *)(in_EAX + 0x10) + 0xc);
  local_10 = 0xffffffff;
  local_1c = -1;
  uVar2 = *(uint *)(*(int *)(in_EAX + 0x10) + 8);
  p_Var10 = (_s_RTTIBaseClassDescriptor *)0x0;
  local_18 = (_s_RTTIBaseClassDescriptor *)0x0;
  local_20 = (_s_RTTIBaseClassDescriptor *)0x0;
  local_24 = (_s_RTTIBaseClassDescriptor *)0x0;
  local_14 = 0;
  bVar5 = true;
  local_c = 0;
  if (uVar2 != 0) {
    do {
      p_Var3 = *(_s_RTTIBaseClassDescriptor **)(iVar1 + local_c * 4);
      if ((local_14 < local_c - local_10) &&
         ((*(int *)p_Var3 == param_4 ||
          (iVar7 = _strcmp((char *)(*(int *)p_Var3 + 8),(char *)(param_4 + 8)), iVar7 == 0)))) {
        if (((byte)p_Var3[0x14] & 3) == 0) {
          local_24 = p_Var3;
        }
        local_10 = local_c;
        local_14 = *(uint *)(p_Var3 + 4);
        p_Var10 = p_Var3;
      }
      iVar7 = local_1c;
      if (((*(_s_RTTICompleteObjectLocator **)p_Var3 == param_2) ||
          (iVar8 = _strcmp((char *)(*(_s_RTTICompleteObjectLocator **)p_Var3 + 8),
                           (char *)(param_2 + 8)), iVar8 == 0)) &&
         (pTVar9 = (TypeDescriptor *)PMDtoOffset(param_1,unaff_EDI), pTVar9 == param_3)) {
        if (local_14 < local_c - local_10) {
          if (((byte)p_Var3[0x14] & 5) == 0) {
            local_20 = p_Var3;
          }
        }
        else if (bVar5) {
          if (((byte)p_Var10[0x14] & 0x40) == 0) {
            if ((local_10 == 0) && (((byte)p_Var3[0x14] & 1) != 0)) {
              bVar5 = false;
            }
            bVar6 = 1;
          }
          else {
            uVar4 = *(uint *)(*(int *)(*(int *)(*(int *)(p_Var10 + 0x18) + 0xc) +
                                      (local_c - local_10) * 4) + 0x14);
            if ((uVar4 & 1) != 0) {
              bVar5 = false;
            }
            bVar6 = ~(byte)(uVar4 >> 2) & 1;
          }
          if (((bVar5) && (bVar6 != 0)) &&
             ((iVar7 = PMDtoOffset(param_1,unaff_EDI),
              bVar11 = local_18 != (_s_RTTIBaseClassDescriptor *)0x0, local_18 = p_Var10, bVar11 &&
              (local_1c != iVar7)))) {
            return (_s_RTTIBaseClassDescriptor *)0x0;
          }
        }
      }
      local_1c = iVar7;
      local_c = local_c + 1;
    } while (local_c < uVar2);
    if ((bVar5) && (local_18 != (_s_RTTIBaseClassDescriptor *)0x0)) {
      return local_18;
    }
    if ((local_20 != (_s_RTTIBaseClassDescriptor *)0x0) &&
       (local_24 != (_s_RTTIBaseClassDescriptor *)0x0)) {
      return local_24;
    }
  }
  return (_s_RTTIBaseClassDescriptor *)0x0;
}


// 1000f802 ___RTDynamicCast

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___RTDynamicCast
   
   Library: Visual Studio 2005 Release */

uint __cdecl
___RTDynamicCast(int *param_1,int param_2,_s_RTTICompleteObjectLocator *param_3,
                TypeDescriptor *param_4,int param_5)

{
  uint uVar1;
  void *pvVar2;
  _s_RTTIBaseClassDescriptor *p_Var3;
  int iVar4;
  TypeDescriptor *pTVar5;
  PMD *in_stack_ffffffc4;
  bad_cast local_2c [12];
  undefined4 local_20;
  undefined4 *local_18;
  undefined4 uStack_c;
  undefined4 local_8;
  
  uStack_c = 0x1000f80e;
  if (param_1 == (int *)0x0) {
    uVar1 = 0;
  }
  else {
    local_8 = 0;
    pvVar2 = (void *)FUN_1000f502(param_1);
    pTVar5 = (TypeDescriptor *)((int)param_1 + (-(int)pvVar2 - param_2));
    uVar1 = *(uint *)(*(int *)(*(int *)(*param_1 + -4) + 0x10) + 4);
    if ((uVar1 & 1) == 0) {
      p_Var3 = FindSITargetTypeInstance(param_3,param_4,(TypeDescriptor *)in_stack_ffffffc4);
    }
    else if ((uVar1 & 2) == 0) {
      p_Var3 = FindMITargetTypeInstance
                         (pvVar2,param_3,pTVar5,(int)param_4,(TypeDescriptor *)in_stack_ffffffc4);
    }
    else {
      p_Var3 = FindVITargetTypeInstance
                         (pvVar2,param_3,pTVar5,(int)param_4,(TypeDescriptor *)in_stack_ffffffc4);
    }
    if (p_Var3 == (_s_RTTIBaseClassDescriptor *)0x0) {
      uVar1 = 0;
      local_20 = 0;
      if (param_5 != 0) {
        std::bad_cast::bad_cast(local_2c,"Bad dynamic_cast!");
        __CxxThrowException_8(local_2c,&DAT_10024ac0);
        return (uint)(*(int *)*local_18 == -0x3ffffffb);
      }
    }
    else {
      iVar4 = PMDtoOffset(pvVar2,in_stack_ffffffc4);
      uVar1 = iVar4 + (int)pvVar2;
    }
  }
  return uVar1;
}


// 1000f8d7 __toupper_l

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
LAB_1000f934:
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
      pDVar4 = FUN_10012790();
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
    if (iVar3 == 0) goto LAB_1000f934;
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


// 1000f9ef _toupper

/* Library Function - Single Match
    _toupper
   
   Library: Visual Studio 2005 Release */

int __cdecl _toupper(int _C)

{
  if (DAT_10059ec0 == 0) {
    if (_C - 0x61U < 0x1a) {
      return _C + -0x20;
    }
  }
  else {
    _C = __toupper_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 1000fa16 __isdigit_l

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


// 1000fa65 _isdigit

/* Library Function - Single Match
    _isdigit
   
   Library: Visual Studio 2005 Release */

int __cdecl _isdigit(int _C)

{
  int iVar1;
  
  if (DAT_10059ec0 == 0) {
    return (byte)PTR_DAT_10028718[_C * 2] & 4;
  }
  iVar1 = __isdigit_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 1000fa8e __isspace_l

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


// 1000fadd _isspace

/* Library Function - Single Match
    _isspace
   
   Library: Visual Studio 2005 Release */

int __cdecl _isspace(int _C)

{
  int iVar1;
  
  if (DAT_10059ec0 == 0) {
    return (byte)PTR_DAT_10028718[_C * 2] & 8;
  }
  iVar1 = __isspace_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 1000fb06 FUN_1000fb06

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 __cdecl FUN_1000fb06(byte *param_1,undefined4 *param_2,localeinfo_struct *param_3)

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
    pDVar1 = FUN_10012790();
    *pDVar1 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_1000fc0c;
      }
    }
    else {
      local_c = _DAT_10028870;
      if (*_Str == 0x2d) {
        local_c = -_DAT_10028870;
      }
    }
    pDVar1 = FUN_10012790();
    *pDVar1 = 0x22;
  }
  else {
    local_c = 0.0;
    if (param_2 != (undefined4 *)0x0) {
      *param_2 = param_1;
    }
  }
LAB_1000fc0c:
  if (local_10 != '\0') {
    *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
  }
  return (float10)local_c;
}


// 1000fc21 FUN_1000fc21

void __cdecl FUN_1000fc21(byte *param_1,undefined4 *param_2)

{
  FUN_1000fb06(param_1,param_2,(localeinfo_struct *)0x0);
  return;
}


// 1000fc34 strtoxl

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
    pDVar3 = FUN_10012790();
    *pDVar3 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
LAB_1000fceb:
    bVar8 = *pbVar9;
    pbVar9 = pbVar2 + 2;
  }
  else if (bVar8 == 0x2b) goto LAB_1000fceb;
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
      goto LAB_1000fd51;
    }
    if ((*pbVar9 != 0x78) && (*pbVar9 != 0x58)) {
      param_4 = 8;
      goto LAB_1000fd51;
    }
    param_4 = 0x10;
  }
  else if ((param_4 != 0x10) || (bVar8 != 0x30)) goto LAB_1000fd51;
  if ((*pbVar9 == 0x78) || (*pbVar9 == 0x58)) {
    bVar8 = pbVar9[1];
    pbVar9 = pbVar9 + 2;
  }
LAB_1000fd51:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  do {
    uVar1 = *(ushort *)(ptVar5[1].lc_category[0].locale + (uint)bVar8 * 2);
    if ((uVar1 & 4) == 0) {
      if ((uVar1 & 0x103) == 0) {
LAB_1000fdab:
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
          pDVar3 = FUN_10012790();
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
    if ((uint)param_4 <= uVar6) goto LAB_1000fdab;
    if ((local_8 < uVar4) ||
       ((local_8 == uVar4 && (uVar6 <= (uint)(0xffffffff % (ulonglong)(uint)param_4))))) {
      local_8 = local_8 * param_4 + uVar6;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (char **)0x0) goto LAB_1000fdab;
    }
    bVar8 = *pbVar9;
    pbVar9 = pbVar9 + 1;
  } while( true );
}


// 1000fe5f _strtol

/* Library Function - Single Match
    _strtol
   
   Library: Visual Studio 2005 Release */

long __cdecl _strtol(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_10059ec0 == 0) {
    ppuVar2 = &PTR_DAT_10028730;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 1000fe88 _strtoul

/* Library Function - Single Match
    _strtoul
   
   Library: Visual Studio 2005 Release */

ulong __cdecl _strtoul(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_10059ec0 == 0) {
    ppuVar2 = &PTR_DAT_10028730;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,1);
  return uVar1;
}


// 1000feb2 __forcdecpt_l

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


// 1000ff23 __cropzeros_l

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


// 1000ffb9 __fassign_l

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


// 10010010 __shift

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


// 10010049 __cftoe2_l

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
    pDVar1 = FUN_10012790();
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
      if (((DAT_1005a038 & 1) != 0) && (_Dst[2] == '0')) {
        pcVar9 = (char *)0x3;
        _memmove(_Dst + 2,_Dst + 3,3);
      }
      if (local_8 == '\0') {
        return (DWORD)pcVar9;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return (DWORD)pcVar9;
    }
    pDVar1 = FUN_10012790();
    DVar7 = 0x22;
  }
  DVar8 = 0;
  *pDVar1 = DVar7;
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return DVar8;
}


// 100101b6 __cftoe_l

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_10012790();
    *pDVar1 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10010284 __cftoe

/* Library Function - Single Match
    __cftoe
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __cftoe(double *_Value,char *_Buf,size_t _SizeInBytes,int _Dec,int _Caps)

{
  errno_t eVar1;
  
  eVar1 = __cftoe_l(_Value,_Buf,_SizeInBytes,_Dec,_Caps,(localeinfo_struct *)0x0);
  return eVar1;
}


// 100102a2 __cftoa_l

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
    pDVar2 = FUN_10012790();
    DVar15 = 0x16;
LAB_100102db:
    DVar16 = 0;
    *pDVar2 = DVar15;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_1c != '\0') {
      *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
    }
    return DVar16;
  }
  *param_2 = 0;
  if (param_3 <= param_4 + 0xb) {
    pDVar2 = FUN_10012790();
    DVar15 = 0x22;
    goto LAB_100102db;
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
    goto LAB_100105ff;
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
LAB_100105ae:
    if ((-1 < (int)uVar8) && ((0 < (int)uVar8 || (99 < uVar7)))) goto LAB_100105b9;
  }
  else {
    uVar14 = __alldvrm(uVar7,uVar8,1000,0);
    local_14 = (undefined4)((ulonglong)uVar14 >> 0x20);
    *pcVar10 = (char)uVar14 + '0';
    pcVar11 = pcVar4 + 3;
    uVar8 = 0;
    uVar7 = extraout_ECX;
    if (pcVar11 == pcVar10) goto LAB_100105ae;
LAB_100105b9:
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
LAB_100105ff:
  if (local_1c == '\0') {
    return unaff_EBX;
  }
  *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  return unaff_EBX;
}


// 10010613 __cftof2_l

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
    pDVar2 = FUN_10012790();
    uVar4 = 0x16;
    *pDVar2 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10010708 __cftof_l

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_10012790();
    *pDVar1 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100107c1 __cftog_l

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_34,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar2 = FUN_10012790();
    *pDVar2 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100108b9 __cfltcvt_l

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


// 1001093f __cfltcvt

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


// 10010960 __initp_misc_cfltcvt_tab

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
    piVar1 = (int *)((int)&PTR_LAB_10027d50 + uVar3);
    iVar2 = FUN_10010e5e(*piVar1);
    uVar3 = uVar3 + 4;
    *piVar1 = iVar2;
  } while (uVar3 < 0x28);
  return;
}


// 1001097f __setdefaultprecision

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


// 100109a8 __ms_p5_test_fdiv

/* WARNING: Removing unreachable block (ram,0x100109db) */
/* Library Function - Single Match
    __ms_p5_test_fdiv
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

undefined4 __ms_p5_test_fdiv(void)

{
  return 0;
}


// 100109e4 __ms_p5_mp_test_fdiv

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


// 10010a0d type_info::_Type_info_dtor

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
  piVar1 = DAT_10059a14;
  if (*(int *)(param_1 + 4) != 0) {
    piVar2 = (int *)&DAT_10059a10;
    do {
      piVar3 = piVar2;
      if (DAT_10059a14 == (int *)0x0) goto LAB_10010a51;
      piVar2 = DAT_10059a14;
    } while (*DAT_10059a14 != *(int *)(param_1 + 4));
    piVar3[1] = DAT_10059a14[1];
    FUN_10009a00(piVar1);
LAB_10010a51:
    FUN_10009a00(*(undefined4 *)(param_1 + 4));
    *(undefined4 *)(param_1 + 4) = 0;
  }
  FUN_10010a74();
  return;
}


// 10010a74 FUN_10010a74

void FUN_10010a74(void)

{
  FUN_10015425(0xe);
  return;
}


// 10010a80 _strcmp

/* Library Function - Single Match
    _strcmp
   
   Library: Visual Studio 2005 Release */

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
      if (bVar4 != *_Str2) goto LAB_10010ac4;
      _Str2 = _Str2 + 1;
      if (bVar4 == 0) {
        return 0;
      }
      if (((uint)_Str1 & 2) == 0) goto LAB_10010a90;
    }
    uVar1 = *(undefined2 *)_Str1;
    _Str1 = _Str1 + 2;
    bVar4 = (byte)uVar1;
    bVar5 = bVar4 < (byte)*_Str2;
    if (bVar4 != *_Str2) goto LAB_10010ac4;
    if (bVar4 == 0) {
      return 0;
    }
    bVar4 = (byte)((ushort)uVar1 >> 8);
    bVar5 = bVar4 < ((byte *)_Str2)[1];
    if (bVar4 != ((byte *)_Str2)[1]) goto LAB_10010ac4;
    if (bVar4 == 0) {
      return 0;
    }
    _Str2 = (char *)((byte *)_Str2 + 2);
  }
LAB_10010a90:
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
LAB_10010ac4:
  return (uint)bVar5 * -2 + 1;
}


// 10010b08 _abort

/* Library Function - Single Match
    _abort
   
   Library: Visual Studio 2005 Release */

void __cdecl _abort(void)

{
  _PHNDLR p_Var1;
  EXCEPTION_RECORD local_32c;
  _EXCEPTION_POINTERS local_2dc;
  undefined4 local_2d4;
  
  if (((byte)DAT_10027d80 & 1) != 0) {
    FUN_10010c19(10);
  }
  p_Var1 = ___get_sigabrt();
  if (p_Var1 != (_PHNDLR)0x0) {
    _raise(0x16);
  }
  if (((byte)DAT_10027d80 & 2) != 0) {
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


// 10010bfb FUN_10010bfb

void __cdecl FUN_10010bfb(uint param_1,uint param_2)

{
  DAT_10027d80 = ~param_2 & DAT_10027d80 | param_1 & param_2;
  return;
}


// 10010c19 FUN_10010c19

void __cdecl FUN_10010c19(int param_1)

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
    if (param_1 == (&DAT_10027d88)[uVar6 * 2]) break;
    uVar6 = uVar6 + 1;
  } while (uVar6 < 0x17);
  if (uVar6 < 0x17) {
    iVar2 = __set_error_mode(3);
    if ((iVar2 == 1) || ((iVar2 = __set_error_mode(3), iVar2 == 0 && (DAT_10059a0c == 1)))) {
      hFile = GetStdHandle(0xfffffff4);
      if ((hFile != (HANDLE)0x0) && (hFile != (HANDLE)0xffffffff)) {
        lpOverlapped = (LPOVERLAPPED)0x0;
        lpNumberOfBytesWritten = &DStack_4;
        puVar1 = (undefined4 *)(uVar6 * 8 + 0x10027d8c);
        sVar5 = _strlen((char *)*puVar1);
        WriteFile(hFile,(LPCVOID)*puVar1,sVar5,lpNumberOfBytesWritten,lpOverlapped);
      }
    }
    else if (param_1 != 0xfc) {
      eVar3 = _strcpy_s(&DAT_10059a18,0x314,"Runtime Error!\n\nProgram: ");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      DAT_10059b35 = 0;
      DVar4 = GetModuleFileNameA((HMODULE)0x0,&DAT_10059a31,0x104);
      if ((DVar4 == 0) &&
         (eVar3 = _strcpy_s(&DAT_10059a31,0x2fb,"<program name unknown>"), eVar3 != 0)) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      sVar5 = _strlen(&DAT_10059a31);
      if (0x3c < sVar5 + 1) {
        sVar5 = _strlen(&DAT_10059a31);
        _Dst = (char *)((int)&DAT_100599f4 + sVar5 + 2);
        eVar3 = _strncpy_s(_Dst,(int)&DAT_10059d2c - (int)_Dst,"...",3);
        if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
      }
      eVar3 = _strcat_s(&DAT_10059a18,0x314,"\n\n");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      eVar3 = _strcat_s(&DAT_10059a18,0x314,*(char **)(uVar6 * 8 + 0x10027d8c));
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      FUN_1001668c(&DAT_10059a18,"Microsoft Visual C++ Runtime Library",0x12010);
    }
  }
  return;
}


// 10010db9 __FF_MSGBANNER

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
    if (DAT_10059a0c != 1) {
      return;
    }
  }
  FUN_10010c19(0xfc);
  FUN_10010c19(0xff);
  return;
}


// 10010df2 FUN_10010df2

undefined4 FUN_10010df2(void)

{
  int iVar1;
  HMODULE pHVar2;
  int iVar3;
  uint uVar4;
  char *_Str2;
  int local_8;
  
  uVar4 = 0;
  local_8 = 0;
  FUN_1001167f(&local_8);
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


// 10010e5e FUN_10010e5e

int __cdecl FUN_10010e5e(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_10027e44);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10027e40 != -1)) {
    iVar3 = DAT_10027e40;
    pcVar2 = (code *)TlsGetValue(DAT_10027e44);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1f8);
      goto LAB_10010eb8;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_10010df2();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"EncodePointer");
LAB_10010eb8:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 10010ecc __encoded_null

/* Library Function - Single Match
    __encoded_null
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __encoded_null(void)

{
  FUN_10010e5e(0);
  return;
}


// 10010ed5 FUN_10010ed5

int __cdecl FUN_10010ed5(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_10027e44);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10027e40 != -1)) {
    iVar3 = DAT_10027e40;
    pcVar2 = (code *)TlsGetValue(DAT_10027e44);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1fc);
      goto LAB_10010f2f;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_10010df2();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"DecodePointer");
LAB_10010f2f:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 10010f4c FUN_10010f4c

LPVOID FUN_10010f4c(void)

{
  LPVOID lpTlsValue;
  
  lpTlsValue = TlsGetValue(DAT_10027e44);
  if (lpTlsValue == (LPVOID)0x0) {
    lpTlsValue = (LPVOID)FUN_10010ed5(DAT_10059d30);
    TlsSetValue(DAT_10027e44,lpTlsValue);
  }
  return lpTlsValue;
}


// 10010f7e __mtterm

/* Library Function - Single Match
    __mtterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __mtterm(void)

{
  code *pcVar1;
  int iVar2;
  
  if (DAT_10027e40 != -1) {
    iVar2 = DAT_10027e40;
    pcVar1 = (code *)FUN_10010ed5(DAT_10059d38);
    (*pcVar1)(iVar2);
    DAT_10027e40 = -1;
  }
  if (DAT_10027e44 != 0xffffffff) {
    TlsFree(DAT_10027e44);
    DAT_10027e44 = 0xffffffff;
  }
  __mtdeletelocks();
  return;
}


// 10010fbb FUN_10010fbb

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

void __cdecl FUN_10010fbb(int param_1,int param_2)

{
  HMODULE hModule;
  int iVar1;
  FARPROC pFVar2;
  
  hModule = GetModuleHandleA("KERNEL32.DLL");
  *(undefined **)(param_1 + 0x5c) = &DAT_10028098;
  *(undefined4 *)(param_1 + 0x14) = 1;
  if (hModule != (HMODULE)0x0) {
    iVar1 = FUN_10010df2();
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
  *(undefined **)(param_1 + 0x68) = &DAT_10028120;
  InterlockedIncrement((LONG *)&DAT_10028120);
  __lock(0xc);
  *(int *)(param_1 + 0x6c) = param_2;
  if (param_2 == 0) {
    *(undefined **)(param_1 + 0x6c) = PTR_DAT_10028728;
  }
  ___addlocaleref(*(LONG **)(param_1 + 0x6c));
  FUN_10011071();
  return;
}


// 10011071 FUN_10011071

void FUN_10011071(void)

{
  FUN_10015425(0xc);
  return;
}


// 1001107a FUN_1001107a

DWORD * FUN_1001107a(void)

{
  DWORD dwErrCode;
  code *pcVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD DVar4;
  undefined4 uVar5;
  DWORD *pDVar6;
  
  dwErrCode = GetLastError();
  uVar5 = DAT_10027e40;
  pcVar1 = (code *)FUN_10010f4c();
  pDVar2 = (DWORD *)(*pcVar1)(uVar5);
  if (pDVar2 == (DWORD *)0x0) {
    pDVar2 = (DWORD *)FUN_10009b00(1,0x214);
    if (pDVar2 != (DWORD *)0x0) {
      uVar5 = DAT_10027e40;
      pDVar6 = pDVar2;
      pcVar1 = (code *)FUN_10010ed5(DAT_10059d34);
      iVar3 = (*pcVar1)(uVar5,pDVar6);
      if (iVar3 == 0) {
        FUN_10009a00(pDVar2);
        pDVar2 = (DWORD *)0x0;
      }
      else {
        FUN_10010fbb((int)pDVar2,0);
        DVar4 = GetCurrentThreadId();
        pDVar2[1] = 0xffffffff;
        *pDVar2 = DVar4;
      }
    }
  }
  SetLastError(dwErrCode);
  return pDVar2;
}


// 100110f1 __getptd

/* Library Function - Single Match
    __getptd
   
   Library: Visual Studio 2005 Release */

_ptiddata __cdecl __getptd(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = (_ptiddata)FUN_1001107a();
  if (p_Var1 == (_ptiddata)0x0) {
    __amsg_exit(0x10);
  }
  return p_Var1;
}


// 10011109 __freefls@4

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
      FUN_10009a00(*(int *)(param_1 + 0x24));
    }
    if (*(int *)(param_1 + 0x2c) != 0) {
      FUN_10009a00(*(int *)(param_1 + 0x2c));
    }
    if (*(int *)(param_1 + 0x34) != 0) {
      FUN_10009a00(*(int *)(param_1 + 0x34));
    }
    if (*(int *)(param_1 + 0x3c) != 0) {
      FUN_10009a00(*(int *)(param_1 + 0x3c));
    }
    if (*(int *)(param_1 + 0x44) != 0) {
      FUN_10009a00(*(int *)(param_1 + 0x44));
    }
    if (*(int *)(param_1 + 0x48) != 0) {
      FUN_10009a00(*(int *)(param_1 + 0x48));
    }
    if (*(undefined **)(param_1 + 0x5c) != &DAT_10028098) {
      FUN_10009a00(*(undefined **)(param_1 + 0x5c));
    }
    __lock(0xd);
    pLVar1 = *(LONG **)(param_1 + 0x68);
    if (pLVar1 != (LONG *)0x0) {
      LVar2 = InterlockedDecrement(pLVar1);
      if ((LVar2 == 0) && (pLVar1 != (LONG *)&DAT_10028120)) {
        FUN_10009a00(pLVar1);
      }
    }
    FUN_10011215();
    __lock(0xc);
    pLVar1 = *(LONG **)(param_1 + 0x6c);
    if (pLVar1 != (LONG *)0x0) {
      ___removelocaleref(pLVar1);
      if (((pLVar1 != (LONG *)PTR_DAT_10028728) && (pLVar1 != (LONG *)&DAT_10028650)) &&
         (*pLVar1 == 0)) {
        ___freetlocinfo((int)pLVar1);
      }
    }
    FUN_10011221();
    FUN_10009a00(param_1);
  }
  return;
}


// 10011215 FUN_10011215

void FUN_10011215(void)

{
  FUN_10015425(0xd);
  return;
}


// 10011221 FUN_10011221

void FUN_10011221(void)

{
  FUN_10015425(0xc);
  return;
}


// 1001122a __freeptd

/* Library Function - Single Match
    __freeptd
   
   Library: Visual Studio 2005 Release */

void __cdecl __freeptd(_ptiddata _Ptd)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  undefined4 uVar4;
  
  if (DAT_10027e40 != -1) {
    if ((_Ptd == (_ptiddata)0x0) && (pvVar1 = TlsGetValue(DAT_10027e44), pvVar1 != (LPVOID)0x0)) {
      iVar3 = DAT_10027e40;
      pcVar2 = (code *)TlsGetValue(DAT_10027e44);
      _Ptd = (_ptiddata)(*pcVar2)(iVar3);
    }
    uVar4 = 0;
    iVar3 = DAT_10027e40;
    pcVar2 = (code *)FUN_10010ed5(DAT_10059d34);
    (*pcVar2)(iVar3,uVar4);
    __freefls_4((int)_Ptd);
  }
  if (DAT_10027e44 != 0xffffffff) {
    TlsSetValue(DAT_10027e44,(LPVOID)0x0);
  }
  return;
}


// 10011293 __mtinit

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
  DAT_10059d2c = GetProcAddress(hModule,"FlsAlloc");
  DAT_10059d30 = GetProcAddress(hModule,"FlsGetValue");
  DAT_10059d34 = GetProcAddress(hModule,"FlsSetValue");
  DAT_10059d38 = GetProcAddress(hModule,"FlsFree");
  if ((((DAT_10059d2c == (FARPROC)0x0) || (DAT_10059d30 == (FARPROC)0x0)) ||
      (DAT_10059d34 == (FARPROC)0x0)) || (DAT_10059d38 == (FARPROC)0x0)) {
    DAT_10059d30 = TlsGetValue_exref;
    DAT_10059d2c = (FARPROC)&LAB_10010f43;
    DAT_10059d34 = TlsSetValue_exref;
    DAT_10059d38 = TlsFree_exref;
  }
  DAT_10027e44 = TlsAlloc();
  if ((DAT_10027e44 != 0xffffffff) && (BVar1 = TlsSetValue(DAT_10027e44,DAT_10059d30), BVar1 != 0))
  {
    __init_pointers();
    DAT_10059d2c = (FARPROC)FUN_10010e5e((int)DAT_10059d2c);
    DAT_10059d30 = (FARPROC)FUN_10010e5e((int)DAT_10059d30);
    DAT_10059d34 = (FARPROC)FUN_10010e5e((int)DAT_10059d34);
    DAT_10059d38 = (FARPROC)FUN_10010e5e((int)DAT_10059d38);
    iVar2 = __mtinitlocks();
    if (iVar2 != 0) {
      pcVar6 = __freefls_4;
      pcVar3 = (code *)FUN_10010ed5((int)DAT_10059d2c);
      DAT_10027e40 = (*pcVar3)(pcVar6);
      if ((DAT_10027e40 != -1) && (pDVar4 = (DWORD *)FUN_10009b00(1,0x214), pDVar4 != (DWORD *)0x0))
      {
        iVar2 = DAT_10027e40;
        pDVar7 = pDVar4;
        pcVar3 = (code *)FUN_10010ed5((int)DAT_10059d34);
        iVar2 = (*pcVar3)(iVar2,pDVar7);
        if (iVar2 != 0) {
          FUN_10010fbb((int)pDVar4,0);
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


// 10011417 FUN_10011417

void __cdecl FUN_10011417(undefined4 param_1)

{
  DAT_10059d44 = param_1;
  return;
}


// 10011421 FUN_10011421

void __cdecl FUN_10011421(undefined4 *param_1,undefined4 *param_2,uint param_3)

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


// 100114a8 __VEC_memcpy

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
      FUN_10011421(param_1,param_2,param_3 - uVar3);
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


// 1001159f __amsg_exit

/* Library Function - Single Match
    __amsg_exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __amsg_exit(int param_1)

{
  code *pcVar1;
  
  __FF_MSGBANNER();
  FUN_10010c19(param_1);
  pcVar1 = (code *)FUN_10010ed5((int)PTR___exit_10027e48);
  (*pcVar1)(0xff);
  return;
}


// 100115c3 FUN_100115c3

void __cdecl FUN_100115c3(undefined4 param_1)

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


// 100115e9 ___crtExitProcess

/* Library Function - Single Match
    ___crtExitProcess
   
   Library: Visual Studio 2005 Release */

void __cdecl ___crtExitProcess(int param_1)

{
  FUN_100115c3(param_1);
                    /* WARNING: Subroutine does not return */
  ExitProcess(param_1);
}


// 100115fe FUN_100115fe

void FUN_100115fe(void)

{
  __lock(8);
  return;
}


// 10011607 FUN_10011607

void FUN_10011607(void)

{
  FUN_10015425(8);
  return;
}


// 10011610 FUN_10011610

void __cdecl FUN_10011610(undefined4 *param_1)

{
  undefined4 *in_EAX;
  
  for (; in_EAX < param_1; in_EAX = in_EAX + 1) {
    if ((code *)*in_EAX != (code *)0x0) {
      (*(code *)*in_EAX)();
    }
  }
  return;
}


// 10011628 __initterm_e

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


// 10011648 __get_osplatform

/* Library Function - Single Match
    __get_osplatform
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __get_osplatform(int *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (int *)0x0) && (DAT_10059d4c != 0)) {
    *param_1 = DAT_10059d4c;
    return 0;
  }
  pDVar1 = FUN_10012790();
  *pDVar1 = 0x16;
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 1001167f FUN_1001167f

undefined4 __cdecl FUN_1001167f(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (undefined4 *)0x0) && (DAT_10059d4c != 0)) {
    *param_1 = DAT_10059d58;
    return 0;
  }
  pDVar1 = FUN_10012790();
  *pDVar1 = 0x16;
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 100116bb __cinit

/* Library Function - Single Match
    __cinit
   
   Library: Visual Studio 2005 Release */

int __cdecl __cinit(int param_1)

{
  BOOL BVar1;
  int iVar2;
  undefined4 *puVar3;
  
  BVar1 = __IsNonwritableInCurrentImage((PBYTE)&PTR___fpmath_1001e438);
  if (BVar1 != 0) {
    __fpmath(param_1);
  }
  __initp_misc_cfltcvt_tab();
  iVar2 = __initterm_e((undefined4 *)&DAT_1001d258,(undefined4 *)&DAT_1001d270);
  if (iVar2 == 0) {
    _atexit((_func_4879 *)&LAB_1001429f);
    puVar3 = &DAT_1001d214;
    do {
      if ((code *)*puVar3 != (code *)0x0) {
        (*(code *)*puVar3)();
      }
      puVar3 = puVar3 + 1;
    } while (puVar3 < &DAT_1001d254);
    if ((DAT_1005b5f4 != (code *)0x0) &&
       (BVar1 = __IsNonwritableInCurrentImage((PBYTE)&DAT_1005b5f4), BVar1 != 0)) {
      (*DAT_1005b5f4)(0,2,0);
    }
    iVar2 = 0;
  }
  return iVar2;
}


// 1001174d FUN_1001174d

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x10011820) */

void __cdecl FUN_1001174d(int param_1,int param_2,int param_3)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  int iVar4;
  code *pcVar5;
  
  __lock(8);
  if (DAT_10059d8c != 1) {
    DAT_10059d88 = 1;
    DAT_10059d84 = (undefined1)param_3;
    if (param_2 == 0) {
      piVar2 = (int *)FUN_10010ed5(DAT_1005b5ec);
      piVar3 = (int *)FUN_10010ed5(DAT_1005b5e8);
      if (piVar2 != (int *)0x0) {
        while (piVar3 = piVar3 + -1, piVar2 <= piVar3) {
          if (*piVar3 != 0) {
            iVar1 = *piVar3;
            iVar4 = __encoded_null();
            if (iVar1 != iVar4) {
              pcVar5 = (code *)FUN_10010ed5(iVar1);
              (*pcVar5)();
            }
          }
        }
      }
      FUN_10011610((undefined4 *)&DAT_1001d280);
    }
    FUN_10011610((undefined4 *)&DAT_1001d288);
  }
  FUN_1001181a();
  if (param_3 != 0) {
    return;
  }
  DAT_10059d8c = 1;
  FUN_10015425(8);
  ___crtExitProcess(param_1);
  return;
}


// 1001181a FUN_1001181a

void FUN_1001181a(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + 0x10) != 0) {
    FUN_10015425(8);
  }
  return;
}


// 1001182f __exit

/* Library Function - Single Match
    __exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __exit(int _Code)

{
  FUN_1001174d(_Code,1,0);
  return;
}


// 10011840 __cexit

/* Library Function - Single Match
    __cexit
   
   Library: Visual Studio 2005 Release */

void __cdecl __cexit(void)

{
  FUN_1001174d(0,0,1);
  return;
}


// 1001184f __init_pointers

/* Library Function - Single Match
    __init_pointers
   
   Library: Visual Studio 2005 Release */

void __cdecl __init_pointers(void)

{
  undefined4 uVar1;
  
  uVar1 = __encoded_null();
  FUN_10016cde(uVar1);
  FUN_10016bff(uVar1);
  FUN_1001262b(uVar1);
  FUN_10011417(uVar1);
  FUN_10016bf5(uVar1);
  __initp_misc_winsig(uVar1);
  FUN_1000dc0c();
  __initp_eh_hooks();
  PTR___exit_10027e48 = (undefined *)FUN_10010e5e(0x1001182f);
  return;
}


// 1001189c __SEH_prolog4

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
  *(uint *)((int)auStack_1c + iVar1 + 4) = DAT_10028738 ^ (uint)&param_2;
  *(undefined4 *)((int)auStack_1c + iVar1) = unaff_retaddr;
  *unaff_FS_OFFSET = (int)local_8;
  return;
}


// 100118e1 __SEH_epilog4

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


// 10011900 __except_handler4

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
  
  piVar8 = (int *)(*(uint *)((int)param_2 + 8) ^ DAT_10028738);
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
          goto LAB_100119ab;
        }
        if (0 < iVar4) {
          if (((*param_1 == -0x1f928c9d) && (DAT_1005b5e0 != (code *)0x0)) &&
             (BVar5 = __IsNonwritableInCurrentImage((PBYTE)&DAT_1005b5e0), BVar5 != 0)) {
            (*DAT_1005b5e0)(param_1,1);
          }
          _EH4_GlobalUnwind(param_2);
          if (*(uint *)((int)param_2 + 0xc) != uVar7) {
            _EH4_LocalUnwind((int)param_2,uVar7,iVar1,&DAT_10028738);
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
    _EH4_LocalUnwind((int)param_2,0xfffffffe,iVar1,&DAT_10028738);
  }
LAB_100119ab:
  if (*piVar8 != -2) {
    __security_check_cookie(piVar8[1] + iVar1 ^ *(uint *)(*piVar8 + iVar1));
  }
  __security_check_cookie(piVar8[3] + iVar1 ^ *(uint *)(piVar8[2] + iVar1));
  return local_c;
}


// 10011a96 __flsbuf

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
    pDVar2 = FUN_10012790();
    *pDVar2 = 9;
LAB_10011aba:
    _File_00->_flag = _File_00->_flag | 0x20;
    return -1;
  }
  if ((uVar6 & 0x40) != 0) {
    pDVar2 = FUN_10012790();
    *pDVar2 = 0x22;
    goto LAB_10011aba;
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
     (((ppuVar3 = FUN_1001771b(), _File_00 != (FILE *)(ppuVar3 + 8) &&
       (ppuVar3 = FUN_1001771b(), _File_00 != (FILE *)(ppuVar3 + 0x10))) ||
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
        puVar5 = &DAT_10028060;
      }
      else {
        puVar5 = (undefined *)(((uint)_File & 0x1f) * 0x38 + (&DAT_1005b4e0)[(int)_File >> 5]);
      }
      if (((puVar5[4] & 0x20) != 0) &&
         (lVar7 = __lseeki64((int)_File,0x200000000,unaff_EDI), lVar7 == -1)) goto LAB_10011be0;
    }
    else {
      local_8 = __write((int)_File,_Buf,uVar6);
    }
    *_File_00->_base = (char)_Ch;
  }
  if (local_8 == uVar6) {
    return _Ch & 0xff;
  }
LAB_10011be0:
  _File_00->_flag = _File_00->_flag | 0x20;
  return -1;
}


// 10011bf6 write_char

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


// 10011c29 write_multi_char

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


// 10011c4d write_string

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
        pDVar1 = FUN_10012790();
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


// 10011c97 FUN_10011c97

/* WARNING: Type propagation algorithm not settling */

void __cdecl FUN_10011c97(FILE *param_1,byte *param_2,localeinfo_struct *param_3,wchar_t *param_4)

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
  
  local_8 = DAT_10028738 ^ (uint)local_1fc;
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
        puVar7 = &DAT_10028060;
      }
      else {
        iVar5 = __fileno(local_22c);
        uVar6 = __fileno(local_22c);
        puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_1005b4e0)[iVar5 >> 5]);
      }
      if ((puVar7[0x24] & 0x7f) == 0) {
        iVar5 = __fileno(local_22c);
        if ((iVar5 == -1) || (iVar5 = __fileno(local_22c), iVar5 == -2)) {
          puVar7 = &DAT_10028060;
        }
        else {
          iVar5 = __fileno(local_22c);
          uVar6 = __fileno(local_22c);
          puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_1005b4e0)[iVar5 >> 5]);
        }
        if ((puVar7[0x24] & 0x80) == 0) goto LAB_10011dd5;
      }
    }
    else {
LAB_10011dd5:
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
          local_270 = (int)(char)(&DAT_1001eb30)[uVar6 * 8 + iVar5] >> 4;
          switch(local_270) {
          case 0:
switchD_10011e39_caseD_0:
            local_240 = 0;
            iVar5 = __isleadbyte_l((uint)local_215,&local_260);
            if (iVar5 != 0) {
              write_char();
              local_244 = param_2 + 2;
              if (*pbVar12 == 0) {
LAB_100125cc:
                pDVar4 = FUN_10012790();
                *pDVar4 = 0x16;
                goto LAB_10011d07;
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
                goto switchD_10011e39_caseD_0;
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
LAB_10012218:
                local_214 = local_214 | 0x40;
LAB_1001221c:
                local_224 = 10;
LAB_10012223:
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
                  goto LAB_100120a1;
                }
                if (local_215 == 0x41) {
LAB_1001204d:
                  local_215 = local_215 + 0x20;
                  local_274 = 1;
LAB_1001205a:
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
                      local_250 = (wchar_t *)FUN_100099a0(sVar13);
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
                  pcVar9 = (code *)FUN_10010ed5((int)PTR_LAB_10027d68);
                  (*pcVar9)(puVar17,pwVar18,sVar13,iVar8,iVar5,uVar19,plVar20);
                  uVar6 = local_214 & 0x80;
                  if ((uVar6 != 0) && (local_21c == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_10010ed5((int)PTR_LAB_10027d74);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((local_215 == 0x67) && (uVar6 == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_10010ed5((int)PTR_LAB_10027d70);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((char)*pwVar14 == '-') {
                    local_214 = local_214 | 0x100;
                    local_220 = (wchar_t *)((int)pwVar14 + 1);
                    pwVar14 = local_220;
                  }
LAB_1001218e:
                  local_224 = _strlen((char *)pwVar14);
                }
                else if (local_215 == 0x43) {
                  if ((local_214 & 0x830) == 0) {
                    local_214 = local_214 | 0x800;
                  }
LAB_100120fe:
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
                else if ((local_215 == 0x45) || (local_215 == 0x47)) goto LAB_1001204d;
              }
              else {
                if (local_215 == 0x58) goto LAB_10012323;
                if (local_215 == 0x5a) {
                  psVar2 = *(short **)param_4;
                  local_228 = param_4 + 2;
                  pwVar14 = (wchar_t *)PTR_s__null__10027e4c;
                  local_220 = (wchar_t *)PTR_s__null__10027e4c;
                  if ((psVar2 == (short *)0x0) ||
                     (pwVar18 = *(wchar_t **)(psVar2 + 2), pwVar18 == (wchar_t *)0x0))
                  goto LAB_1001218e;
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
                  if (local_215 == 0x61) goto LAB_1001205a;
                  if (local_215 == 99) goto LAB_100120fe;
                }
              }
LAB_1001249b:
              if (local_24c == 0) {
                if ((local_214 & 0x40) != 0) {
                  if ((local_214 & 0x100) == 0) {
                    if ((local_214 & 1) == 0) {
                      if ((local_214 & 2) == 0) goto LAB_100124d1;
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
LAB_100124d1:
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
LAB_100120a1:
                  iVar5 = local_21c;
                  if (local_21c == -1) {
                    iVar5 = 0x7fffffff;
                  }
                  local_228 = param_4 + 2;
                  local_220 = *(wchar_t **)param_4;
                  if ((local_214 & 0x810) == 0) {
                    pwVar14 = local_220;
                    if (local_220 == (wchar_t *)0x0) {
                      pwVar14 = (wchar_t *)PTR_s__null__10027e4c;
                      local_220 = (wchar_t *)PTR_s__null__10027e4c;
                    }
                    for (; (iVar5 != 0 && (iVar5 = iVar5 + -1, (char)*pwVar14 != '\0'));
                        pwVar14 = (wchar_t *)((int)pwVar14 + 1)) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220;
                  }
                  else {
                    if (local_220 == (wchar_t *)0x0) {
                      local_220 = (wchar_t *)PTR_u__null__10027e50;
                    }
                    local_240 = 1;
                    for (pwVar14 = local_220;
                        (iVar5 != 0 && (iVar5 = iVar5 + -1, *pwVar14 != L'\0'));
                        pwVar14 = pwVar14 + 1) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220 >> 1;
                  }
                  goto LAB_1001249b;
                }
                if (local_215 == 0x75) goto LAB_1001221c;
                if (local_215 != 0x78) goto LAB_1001249b;
                local_248 = 0x27;
LAB_10012349:
                local_224 = 0x10;
                if ((local_214 & 0x80) != 0) {
                  local_233 = (char)local_248 + 'Q';
                  local_234 = 0x30;
                  local_238 = 2;
                }
                goto LAB_10012223;
              }
              if (local_215 == 0x70) {
                local_21c = 8;
LAB_10012323:
                local_248 = 7;
                goto LAB_10012349;
              }
              if ((char)local_215 < 'e') goto LAB_1001249b;
              if ((char)local_215 < 'h') goto LAB_1001205a;
              if (local_215 == 0x69) goto LAB_10012218;
              if (local_215 != 0x6e) {
                if (local_215 != 0x6f) goto LAB_1001249b;
                local_224 = 8;
                if ((local_214 & 0x80) != 0) {
                  local_214 = local_214 | 0x200;
                }
                goto LAB_10012223;
              }
              piVar3 = *(int **)param_4;
              local_228 = param_4 + 2;
              bVar15 = FUN_100178e9();
              if (CONCAT31(extraout_var,bVar15) == 0) goto LAB_100125cc;
              if ((local_214 & 0x20) == 0) {
                *piVar3 = local_230;
              }
              else {
                *(undefined2 *)piVar3 = (undefined2)local_230;
              }
              local_24c = 1;
            }
            if (local_250 != (wchar_t *)0x0) {
              FUN_10009a00(local_250);
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
        goto LAB_100125f3;
      }
    }
  }
  pDVar4 = FUN_10012790();
  *pDVar4 = 0x16;
LAB_10011d07:
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_254 != '\0') {
    *(uint *)(local_258 + 0x70) = *(uint *)(local_258 + 0x70) & 0xfffffffd;
  }
LAB_100125f3:
  __security_check_cookie(local_8 ^ (uint)local_1fc);
  return;
}


// 1001262b FUN_1001262b

void __cdecl FUN_1001262b(undefined4 param_1)

{
  DAT_10059d90 = param_1;
  return;
}


// 10012635 __invoke_watson

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
  
  iVar1 = DAT_10028738;
  local_2d4 = 0x10001;
  _memset(&local_32c,0,0x50);
  local_2dc.ExceptionRecord = &local_32c;
  local_2dc.ContextRecord = (PCONTEXT)&local_2d4;
  local_32c.ExceptionCode = 0xc000000d;
  BVar2 = IsDebuggerPresent();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  LVar3 = UnhandledExceptionFilter(&local_2dc);
  if ((LVar3 == 0) && (BVar2 == 0)) {
    FUN_10017b15();
  }
  uExitCode = 0xc000000d;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  __security_check_cookie(iVar1);
  return;
}


// 10012731 FUN_10012731

void __cdecl
FUN_10012731(wchar_t *param_1,wchar_t *param_2,wchar_t *param_3,uint param_4,uintptr_t param_5)

{
  code *UNRECOVERED_JUMPTABLE;
  
  UNRECOVERED_JUMPTABLE = (code *)FUN_10010ed5(DAT_10059d90);
  if (UNRECOVERED_JUMPTABLE != (code *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x10012745. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (*UNRECOVERED_JUMPTABLE)();
    return;
  }
  FUN_10017b15();
                    /* WARNING: Subroutine does not return */
  __invoke_watson(param_1,param_2,param_3,param_4,param_5);
}


// 10012755 FUN_10012755

int __cdecl FUN_10012755(int param_1)

{
  uint uVar1;
  
  uVar1 = 0;
  do {
    if (param_1 == (&DAT_10027e58)[uVar1 * 2]) {
      return *(int *)(uVar1 * 8 + 0x10027e5c);
    }
    uVar1 = uVar1 + 1;
  } while (uVar1 < 0x2d);
  if (param_1 - 0x13U < 0x12) {
    return 0xd;
  }
  return (-(uint)(0xe < param_1 - 0xbcU) & 0xe) + 8;
}


// 10012790 FUN_10012790

DWORD * FUN_10012790(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_1001107a();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10027fc0;
  }
  return pDVar1 + 2;
}


// 100127a3 FUN_100127a3

DWORD * FUN_100127a3(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_1001107a();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10027fc4;
  }
  return pDVar1 + 3;
}


// 100127b6 FUN_100127b6

void __cdecl FUN_100127b6(DWORD param_1)

{
  DWORD *pDVar1;
  DWORD DVar2;
  
  pDVar1 = FUN_100127a3();
  *pDVar1 = param_1;
  DVar2 = FUN_10012755(param_1);
  pDVar1 = FUN_10012790();
  *pDVar1 = DVar2;
  return;
}


// 100127d4 fastzero_I

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


// 1001282b __VEC_memzero

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


// 100128c0 FUN_100128c0

void FUN_100128c0(void)

{
  float10 in_ST0;
  float10 in_ST1;
  
  FUN_100128d9((double)in_ST1,SUB84((double)in_ST0,0),(uint)((ulonglong)(double)in_ST0 >> 0x20));
  return;
}


// 100128d9 FUN_100128d9

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 __cdecl FUN_100128d9(double param_1,int param_2,uint param_3)

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
           *(double *)((int)&DAT_1001eb90 + uVar2 * 4);
  dVar28 = *(double *)((int)&DAT_1001eb90 + uVar2 * 4);
  dVar11 = *(double *)(&DAT_1001efa0 + uVar2 * 8);
  dVar30 = *(double *)(&UNK_1001efa8 + uVar2 * 8);
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
        goto LAB_100130e0;
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
                goto LAB_10013067;
              }
              if ((param_3 & 0x80000000) == 0) {
                return (float10)-INFINITY;
              }
              goto LAB_10012ef9;
            }
            if ((param_3 & 0x80000000) != 0) {
              return (float10)0;
            }
            goto LAB_10013067;
          }
          goto LAB_10012f3f;
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
            goto LAB_10012956;
          }
          goto LAB_10012d74;
        }
        uVar24 = (ulonglong)param_1 >> 0x20;
        in_XMM2_Qa = auVar20._8_8_ >> 0x20;
        uVar7 = uVar2 | uVar8 & 0x7fffffff;
        uVar6 = 0;
        if (uVar7 != 0) {
          dVar28 = -NAN;
          iVar5 = 0x1c;
          goto LAB_100130e0;
        }
LAB_10012dfa:
        dVar29 = dVar10;
        if ((uVar7 & 0x7fffffff) == 0) {
          if ((param_3 & 0x80000000) == 0) {
            if ((uVar7 & uVar6 << 0xd) == 0) {
              return (float10)0;
            }
LAB_10012ef9:
            return (float10)-0.0;
          }
          dVar28 = (double)((ulonglong)(uVar7 & uVar6 << 0xd | 0x7ff00000) << 0x20);
          iVar5 = 0x1b;
          goto LAB_100130e0;
        }
      }
      else {
        uVar6 = 0;
LAB_10012d74:
        dVar32 = 2.225073858507201e-308;
        uVar24 = 0x3ff0000000000000;
        dVar10 = param_1 * 1.8446744073709552e+19;
        uVar7 = uVar8;
        dVar29 = dVar10;
        if (uVar2 == 0) goto LAB_10012dfa;
      }
      dVar10 = (double)((ulonglong)ABS(dVar29) >> 0x2c);
      uVar7 = (SUB82(dVar10,0) & 0xff) + 1 & 0x1fe;
      dVar32 = (double)((ulonglong)dVar32 & (ulonglong)dVar29 | uVar24) *
               *(double *)((int)&DAT_1001eb90 + uVar7 * 4);
      dVar28 = *(double *)((int)&DAT_1001eb90 + uVar7 * 4);
      dVar11 = *(double *)(&DAT_1001efa0 + uVar7 * 8);
      dVar30 = *(double *)(&UNK_1001efa8 + uVar7 * 8);
      uVar7 = 0x43e7f;
      goto LAB_10012956;
    }
    dVar28 = param_1;
    if ((0x7fefffff < (uVar8 & 0x7fffffff)) && ((0x7ff00000 < (uVar8 & 0x7fffffff) || (uVar2 != 0)))
       ) {
LAB_10012f3f:
      dVar28 = param_1 + param_1;
      iVar5 = 0x3ee;
      goto LAB_100130e0;
    }
    goto LAB_10012ff5;
  }
  uVar6 = 0;
  uVar7 = 0x3fe7f;
LAB_10012956:
  uVar3 = ((ushort)((ulonglong)dVar32 >> 0x26) & 0xff) + 1 & 0x1fe;
  dVar26 = (double)((ulonglong)dVar29 & 0xfffffffffffff | DAT_10022430);
  dVar12 = (double)((ulonglong)dVar26 & 0xfffffffff8000000);
  dVar26 = dVar26 - dVar12;
  uVar4 = ((ushort)((ulonglong)(dVar32 * *(double *)(&DAT_1001f7b0 + uVar3 * 4)) >> 0x1f) & 0x1ff) +
          1 & 0x3fe;
  dVar27 = dVar28 * *(double *)(&DAT_1001f7b0 + uVar3 * 4) * *(double *)(&DAT_100203d0 + uVar4 * 4);
  dVar32 = dVar32 * *(double *)(&DAT_1001f7b0 + uVar3 * 4) * *(double *)(&DAT_100203d0 + uVar4 * 4);
  dVar29 = dVar11 + *(double *)(&DAT_1001fbc0 + uVar3 * 8) +
           (double)(int)((longlong)dVar10 - (ulonglong)uVar7 >> 8) +
           *(double *)(&DAT_10020be0 + uVar4 * 8);
  dVar31 = dVar30 + *(double *)(&DAT_1001fbc8 + uVar3 * 8) + *(double *)(&DAT_10020be8 + uVar4 * 8);
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
          goto LAB_10012ff5;
        }
      }
      else {
LAB_10012ff5:
        uVar24 = CONCAT44(param_3,param_2) & 0xfffffffffffff;
        auVar16._0_4_ = -(uint)((int)uVar24 == 0);
        auVar16._4_4_ = -(uint)((int)(uVar24 >> 0x20) == 0);
        auVar16._8_4_ = -(uint)((int)(in_XMM3_Qa & in_XMM2_Qa) == 0);
        auVar16._12_4_ = -(uint)((int)((in_XMM3_Qa & in_XMM2_Qa) >> 0x20) == 0);
        if ((byte)(SUB161(auVar16 >> 7,0) & 1 | (SUB161(auVar16 >> 0xf,0) & 1) << 1 |
                   (SUB161(auVar16 >> 0x17,0) & 1) << 2 | (SUB161(auVar16 >> 0x1f,0) & 1) << 3 |
                   (SUB161(auVar16 >> 0x27,0) & 1) << 4 | (SUB161(auVar16 >> 0x2f,0) & 1) << 5 |
                   (SUB161(auVar16 >> 0x37,0) & 1) << 6 | SUB161(auVar16 >> 0x3f,0) << 7) != 0xff)
        goto LAB_1001306e;
        if (dVar28 != -1.0) {
          if ((param_3 & 0x80000000) == 0) {
            if ((uVar9 & 0x7ff0) < 0x3ff0) {
              return (float10)0;
            }
          }
          else if (0x3fef < (uVar9 & 0x7ff0)) {
            return (float10)0;
          }
LAB_10013067:
          return (float10)INFINITY;
        }
      }
      dVar28 = 1.0;
      iVar5 = 0x1c;
    }
    else {
LAB_1001306e:
      dVar28 = (double)CONCAT44(param_3,param_2) + (double)CONCAT44(param_3,param_2);
      iVar5 = 0x3ee;
    }
    goto LAB_100130e0;
  }
  iVar5 = (uVar7 - 0x3ff0) + (uVar3 & 0x7ff0);
  if ((0x40a0U - iVar5 | iVar5 - 0x3c70U) < 0x80000000) {
LAB_10012aa8:
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
      dVar29 = *(double *)(&DAT_10021c10 + iVar5) * auVar13._0_8_;
      dVar28 = (dVar30 - ((dVar30 + 6755399441055744.0) - 6755399441055744.0)) * 0.0078125 +
               dVar28 + ((dVar32 * 0.16015105075297303 + dVar32 * dVar32 * -0.08325619496072671) *
                         dVar32 * dVar32 +
                         dVar32 * 9.597935033233511e-08 + dVar32 * dVar32 * -0.3465736568077919 +
                        dVar31 + dVar11) * (double)CONCAT44(param_3,param_2);
      return (float10)(dVar28 * dVar28 * dVar29 *
                       (dVar28 * 0.0013333558146428443 + 0.009618129107628477) * dVar28 * dVar28 +
                       *(double *)(&UNK_10021c18 + iVar5) * auVar13._8_8_ +
                       (dVar28 * 0.055504108664821576 + 0.2402265069591007) * dVar28 * dVar28 *
                       dVar29 + dVar28 * 0.6931471805599453 * dVar29 + dVar29);
    }
    if ((int)uVar2 < 1) {
      if ((int)uVar2 < -0x3fdff) {
LAB_10013339:
        dVar28 = (double)((ulonglong)uVar6 << 0x2d);
        iVar5 = 0x19;
        goto LAB_100130e0;
      }
      uVar6 = uVar6 + 0x80;
      uVar8 = (uVar2 & 0xffffff80) + 0x3fe80;
      uVar9 = 0;
    }
    else {
      if (0x3ffff < uVar2) goto LAB_1001335c;
      uVar6 = uVar6 + 0x3ff00;
      uVar8 = uVar2 - 0x80 & 0xffffff80;
      uVar9 = 0x3ff0;
    }
    iVar5 = (uVar2 & 0x7f) * 0x10;
    uVar2 = ((int)-(uVar8 - 0x1ff80) >> 7) + 2;
    auVar17._0_8_ = (double)((ulonglong)uVar8 << 0x2d);
    auVar17._8_4_ = 0;
    auVar17._12_4_ = (int)((ulonglong)auVar17._0_8_ >> 0x20);
    dVar10 = *(double *)(&DAT_10021c10 + iVar5) * auVar17._0_8_;
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
             *(double *)(&UNK_10021c18 + iVar5) * auVar17._8_8_ +
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
    if ((0x40a0 - uVar2 | uVar8) < 0x80000000) goto LAB_10012aa8;
    if (0x7fffffff < uVar8) {
      return (float10)(double)((ulonglong)(uVar6 | 0x1ff80) << 0x2d);
    }
    if (((uVar18 ^ (uVar9 & 0x7ff0) + 0xc010) & 0x8000) != 0) goto LAB_10013339;
LAB_1001335c:
    iVar5 = 0x18;
    if (uVar6 == 0) {
      dVar28 = INFINITY;
    }
    else {
      dVar28 = -INFINITY;
    }
  }
LAB_100130e0:
  ___libm_error_support(&param_1,(undefined8 *)&param_2,(double *)&stack0xfffffff4,iVar5);
  return (float10)dVar28;
}


// 10013800 FUN_10013800

float10 __fastcall
FUN_10013800(undefined4 param_1,int param_2,undefined2 param_3,undefined4 param_4,undefined4 param_5
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


// 10013817 __startOneArgErrorHandling

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


// 10013860 FUN_10013860

undefined1  [10] FUN_10013860(void)

{
  float10 in_ST0;
  float10 fVar1;
  undefined1 auVar2 [10];
  
  fVar1 = (float10)f2xm1(-(ROUND(in_ST0) - in_ST0));
  auVar2 = (undefined1  [10])fscale((float10)1 + fVar1,ROUND(in_ST0));
  return auVar2;
}


// 10013875 FUN_10013875

void FUN_10013875(void)

{
  return;
}


// 100138a5 __fload_withFB

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


// 100138fe FUN_100138fe

void FUN_100138fe(void)

{
  return;
}


// 10013949 FUN_10013949

void __fastcall
FUN_10013949(undefined4 param_1,int param_2,undefined4 param_3,undefined4 param_4,undefined4 param_5
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
    FUN_10013800(param_1,0x1d,unaff_retaddr,param_3,param_4,param_5,param_6,param_7);
    return;
  }
  __startOneArgErrorHandling(param_1,param_2,unaff_retaddr,param_3,param_4,param_5);
  return;
}


// 100139ec __d_inttype

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


// 10013a50 FUN_10013a50

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __cdecl FUN_10013a50(int param_1,int param_2,int param_3,int param_4,double *param_5)

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
  dVar3 = _DAT_10028c60;
  if (param_4 == 0x7ff00000) {
    if (param_3 == 0) {
      if ((dVar1 <= 1.0) && (dVar3 = 1.0, dVar1 < 1.0)) {
        dVar3 = dVar2;
      }
      goto LAB_10013b7d;
    }
  }
  else if ((param_4 == -0x100000) && (param_3 == 0)) {
    dVar3 = dVar2;
    if ((dVar1 <= 1.0) && (dVar3 = _DAT_10028c60, 1.0 <= dVar1)) {
      *param_5 = _DAT_10028c68;
      return 1;
    }
    goto LAB_10013b7d;
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
          dVar3 = _DAT_10028c80;
        }
      }
    }
    else {
      dVar3 = _DAT_10028c60;
      if (iVar4 == 1) {
        dVar3 = -_DAT_10028c60;
      }
    }
  }
LAB_10013b7d:
  *param_5 = dVar3;
  return 0;
}


// 10013b8e __ioinit

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
  
  uStack_c = 0x10013b9a;
  local_8 = 0;
  GetStartupInfoA(&local_68);
  local_8 = 0xfffffffe;
  puVar2 = (undefined4 *)FUN_10009b00(0x20,0x38);
  if (puVar2 == (undefined4 *)0x0) {
LAB_10013dc5:
    iVar7 = -1;
  }
  else {
    DAT_1005b4c4 = 0x20;
    DAT_1005b4e0 = puVar2;
    for (; puVar2 < DAT_1005b4e0 + 0x1c0; puVar2 = puVar2 + 0xe) {
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
      while ((UVar10 = UVar9, (int)DAT_1005b4c4 < (int)UVar9 &&
             (puVar2 = (undefined4 *)FUN_10009b00(0x20,0x38), UVar10 = DAT_1005b4c4,
             puVar2 != (undefined4 *)0x0))) {
        (&DAT_1005b4e0)[iVar7] = puVar2;
        DAT_1005b4c4 = DAT_1005b4c4 + 0x20;
        puVar1 = puVar2;
        for (; puVar2 < puVar1 + 0x1c0; puVar2 = puVar2 + 0xe) {
          *(undefined1 *)(puVar2 + 1) = 0;
          *puVar2 = 0xffffffff;
          *(undefined1 *)((int)puVar2 + 5) = 10;
          puVar2[2] = 0;
          *(byte *)(puVar2 + 9) = *(byte *)(puVar2 + 9) & 0x80;
          *(undefined1 *)((int)puVar2 + 0x25) = 10;
          *(undefined1 *)((int)puVar2 + 0x26) = 10;
          puVar1 = (&DAT_1005b4e0)[iVar7];
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
                     ((local_24 & 0x1f) * 0x38 + (int)(&DAT_1005b4e0)[(int)local_24 >> 5]);
            *puVar2 = *(undefined4 *)local_20;
            *(byte *)(puVar2 + 1) = (byte)*pUVar6;
            iVar7 = ___crtInitCritSecAndSpinCount(puVar2 + 3,4000);
            if (iVar7 == 0) goto LAB_10013dc5;
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
      piVar8 = DAT_1005b4e0 + iVar7 * 0xe;
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
          if (iVar5 == 0) goto LAB_10013dc5;
          piVar8[2] = piVar8[2] + 1;
        }
      }
      else {
        *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 0x80;
      }
      iVar7 = iVar7 + 1;
    } while (iVar7 < 3);
    SetHandleCount(DAT_1005b4c4);
    iVar7 = 0;
  }
  return iVar7;
}


// 10013dce __ioterm

/* Library Function - Single Match
    __ioterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __ioterm(void)

{
  uint uVar1;
  uint *puVar2;
  uint uVar3;
  
  puVar2 = &DAT_1005b4e0;
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
      FUN_10009a00(*puVar2);
      *puVar2 = 0;
    }
    puVar2 = puVar2 + 1;
  } while ((int)puVar2 < 0x1005b5e0);
  return;
}


// 10013e1a __setenvp

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
  
  if (DAT_1005b5f0 == 0) {
    ___initmbctable();
  }
  iVar5 = 0;
  pcVar4 = DAT_10059a00;
  if (DAT_10059a00 != (char *)0x0) {
    for (; *pcVar4 != '\0'; pcVar4 = pcVar4 + sVar2 + 1) {
      if (*pcVar4 != '=') {
        iVar5 = iVar5 + 1;
      }
      sVar2 = _strlen(pcVar4);
    }
    puVar1 = (undefined4 *)FUN_10009b00(iVar5 + 1,4);
    pcVar4 = DAT_10059a00;
    DAT_10059d6c = puVar1;
    if (puVar1 != (undefined4 *)0x0) {
      do {
        if (*pcVar4 == '\0') {
          FUN_10009a00(DAT_10059a00);
          DAT_10059a00 = (char *)0x0;
          *puVar1 = 0;
          _DAT_1005b5e4 = 1;
          return 0;
        }
        sVar2 = _strlen(pcVar4);
        _SizeInBytes = sVar2 + 1;
        if (*pcVar4 != '=') {
          _Dst = (char *)FUN_10009b00(_SizeInBytes,1);
          *puVar1 = _Dst;
          if (_Dst == (char *)0x0) {
            FUN_10009a00(DAT_10059d6c);
            DAT_10059d6c = (undefined4 *)0x0;
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


// 10013ef5 parse_cmdline

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
        goto LAB_10013f87;
      }
    }
    in_EDX = pbVar7;
  } while ((bVar2) || ((bVar6 != 0x20 && (bVar6 != 9))));
  if (pbVar5 != (byte *)0x0) {
    pbVar5[-1] = 0;
  }
LAB_10013f87:
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


// 1001408d __setargv

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
  
  if (DAT_1005b5f0 == 0) {
    ___initmbctable();
  }
  DAT_10059e9c = 0;
  GetModuleFileNameA((HMODULE)0x0,&DAT_10059d98,0x104);
  _DAT_10059d7c = &DAT_10059d98;
  if ((DAT_1005b600 == (char *)0x0) || (local_8 = DAT_1005b600, *DAT_1005b600 == '\0')) {
    local_8 = &DAT_10059d98;
  }
  parse_cmdline((undefined4 *)0x0,(byte *)0x0,(int *)&local_c);
  uVar2 = local_c;
  if ((local_c < 0x3fffffff) && (local_10 != 0xffffffff)) {
    uVar1 = local_c * 4 + local_10;
    if ((local_10 <= uVar1) &&
       (puVar3 = (undefined4 *)FUN_100099a0(uVar1), puVar3 != (undefined4 *)0x0)) {
      parse_cmdline(puVar3,(byte *)(puVar3 + uVar2),(int *)&local_c);
      _DAT_10059d60 = local_c - 1;
      _DAT_10059d64 = puVar3;
      return 0;
    }
  }
  return -1;
}


// 10014146 ___crtGetEnvironmentStringsA

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
  if (DAT_10059ea0 == 0) {
    lpWideCharStr = GetEnvironmentStringsW();
    if (lpWideCharStr != (LPWCH)0x0) {
      DAT_10059ea0 = 1;
      goto LAB_10014198;
    }
    DVar3 = GetLastError();
    if (DVar3 == 0x78) {
      DAT_10059ea0 = 2;
    }
  }
  if (DAT_10059ea0 != 1) {
    if ((DAT_10059ea0 != 2) && (DAT_10059ea0 != 0)) {
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
    _Dst = (void *)FUN_100099a0(pcVar7 + (1 - (int)_Src));
    if (_Dst == (void *)0x0) {
      FreeEnvironmentStringsA(_Src);
      return (LPVOID)0x0;
    }
    _memcpy(_Dst,_Src,(size_t)(pcVar7 + (1 - (int)_Src)));
    FreeEnvironmentStringsA(_Src);
    return _Dst;
  }
LAB_10014198:
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
      (lpMultiByteStr = (LPSTR)FUN_100099a0(cbMultiByte), lpMultiByteStr != (LPSTR)0x0)) &&
     (iVar6 = WideCharToMultiByte(0,0,lpWideCharStr,iVar6,lpMultiByteStr,cbMultiByte,(LPCSTR)0x0,
                                  (LPBOOL)0x0), local_8 = lpMultiByteStr, iVar6 == 0)) {
    FUN_10009a00(lpMultiByteStr);
    local_8 = (LPSTR)0x0;
  }
  FreeEnvironmentStringsW(lpWideCharStr);
  return local_8;
}


// 1001427b __RTC_Initialize

/* WARNING: Removing unreachable block (ram,0x1001428d) */
/* WARNING: Removing unreachable block (ram,0x10014293) */
/* WARNING: Removing unreachable block (ram,0x10014295) */
/* Library Function - Single Match
    __RTC_Initialize
   
   Library: Visual Studio 2005 Release */

void __RTC_Initialize(void)

{
  return;
}


// 100142c3 FUN_100142c3

DWORD * __cdecl FUN_100142c3(int param_1,DWORD param_2)

{
  int *piVar1;
  code *pcVar2;
  DWORD DVar3;
  DWORD DVar4;
  DWORD *pDVar5;
  int *piVar6;
  int iVar7;
  int iVar8;
  
  pDVar5 = FUN_1001107a();
  if (pDVar5 != (DWORD *)0x0) {
    piVar1 = (int *)pDVar5[0x17];
    piVar6 = piVar1;
    do {
      if (*piVar6 == param_1) break;
      piVar6 = piVar6 + 3;
    } while (piVar6 < piVar1 + DAT_1002811c * 3);
    if ((piVar1 + DAT_1002811c * 3 <= piVar6) || (*piVar6 != param_1)) {
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
          if (DAT_10028110 < DAT_10028114 + DAT_10028110) {
            iVar7 = DAT_10028110 * 0xc;
            iVar8 = DAT_10028110;
            do {
              *(undefined4 *)(iVar7 + 8 + pDVar5[0x17]) = 0;
              iVar8 = iVar8 + 1;
              iVar7 = iVar7 + 0xc;
            } while (iVar8 < DAT_10028114 + DAT_10028110);
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


// 1001443c FUN_1001443c

undefined4 FUN_1001443c(void)

{
  return 1;
}


// 10014442 ___security_init_cookie

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
  if ((DAT_10028738 == 0xbb40e64e) || ((DAT_10028738 & 0xffff0000) == 0)) {
    GetSystemTimeAsFileTime(&local_c);
    uVar4 = local_c.dwHighDateTime ^ local_c.dwLowDateTime;
    DVar1 = GetCurrentProcessId();
    DVar2 = GetCurrentThreadId();
    DVar3 = GetTickCount();
    QueryPerformanceCounter(&local_14);
    DAT_10028738 = uVar4 ^ DVar1 ^ DVar2 ^ DVar3 ^ local_14.s.HighPart ^ local_14.s.LowPart;
    if (DAT_10028738 == 0xbb40e64e) {
      DAT_10028738 = 0xbb40e64f;
    }
    else if ((DAT_10028738 & 0xffff0000) == 0) {
      DAT_10028738 = DAT_10028738 | DAT_10028738 << 0x10;
    }
    DAT_1002873c = ~DAT_10028738;
  }
  else {
    DAT_1002873c = ~DAT_10028738;
  }
  return;
}


// 100144d6 FUN_100144d6

undefined4 FUN_100144d6(void)

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


// 10014505 setSBCS

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
    *puVar1 = puVar1[(int)&DAT_10028120 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  puVar1 = (undefined1 *)(in_EAX + 0x11d);
  iVar2 = 0x100;
  do {
    *puVar1 = puVar1[(int)&DAT_10028120 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  return;
}


// 1001455a setSBUpLow

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
  
  local_8 = DAT_10028738 ^ (uint)local_4a0;
  BVar3 = GetCPInfo(*(UINT *)(unaff_ESI + 4),&local_51c);
  if (BVar3 == 0) {
    uVar4 = 0;
    do {
      pcVar2 = (char *)(unaff_ESI + 0x11d + uVar4);
      if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) + 0x20 < (char *)0x1a) {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        cVar6 = (char)uVar4 + ' ';
LAB_100146c1:
        *pcVar2 = cVar6;
      }
      else {
        if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) < (char *)0x1a) {
          pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
          *pbVar1 = *pbVar1 | 0x20;
          cVar6 = (char)uVar4 + -0x20;
          goto LAB_100146c1;
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
          goto LAB_10014668;
        }
        *(undefined1 *)(unaff_ESI + 0x11d + uVar4) = 0;
      }
      else {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        CVar5 = local_208[uVar4];
LAB_10014668:
        *(CHAR *)(unaff_ESI + 0x11d + uVar4) = CVar5;
      }
      uVar4 = uVar4 + 1;
    } while (uVar4 < 0x100);
  }
  __security_check_cookie(local_8 ^ (uint)local_4a0);
  return;
}


// 100146e4 ___updatetmbcinfo

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
  if (((p_Var1->_ownlocale & DAT_10028644) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xd);
    lpAddend = p_Var1->ptmbcinfo;
    if (lpAddend != (pthreadmbcinfo)PTR_DAT_10028548) {
      if (lpAddend != (pthreadmbcinfo)0x0) {
        LVar2 = InterlockedDecrement(&lpAddend->refcount);
        if ((LVar2 == 0) && (lpAddend != (pthreadmbcinfo)&DAT_10028120)) {
          FUN_10009a00(lpAddend);
        }
      }
      p_Var1->ptmbcinfo = (pthreadmbcinfo)PTR_DAT_10028548;
      lpAddend = (pthreadmbcinfo)PTR_DAT_10028548;
      InterlockedIncrement((LONG *)PTR_DAT_10028548);
    }
    FUN_1001477f();
  }
  else {
    lpAddend = p_Var1->ptmbcinfo;
  }
  if (lpAddend == (pthreadmbcinfo)0x0) {
    __amsg_exit(0x20);
  }
  return lpAddend;
}


// 1001477f FUN_1001477f

void FUN_1001477f(void)

{
  FUN_10015425(0xd);
  return;
}


// 10014788 getSystemCP

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
  DAT_10059ea4 = 0;
  if (unaff_ESI == -2) {
    DAT_10059ea4 = 1;
    UVar1 = GetOEMCP();
  }
  else if (unaff_ESI == -3) {
    DAT_10059ea4 = 1;
    UVar1 = GetACP();
  }
  else {
    if (unaff_ESI != -4) {
      if (local_8 == '\0') {
        DAT_10059ea4 = 0;
        return unaff_ESI;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return unaff_ESI;
    }
    UVar1 = *(UINT *)(local_14[0] + 4);
    DAT_10059ea4 = 1;
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return UVar1;
}


// 10014802 FUN_10014802

void __cdecl FUN_10014802(undefined4 param_1,int param_2)

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  uVar4 = getSystemCP((int)unaff_EDI);
  if (uVar4 != 0) {
    local_20 = (byte *)0x0;
    uVar5 = 0;
LAB_1001483e:
    if (*(uint *)((int)&DAT_10028550 + uVar5) != uVar4) goto code_r0x1001484a;
    _memset((void *)(param_2 + 0x1c),0,0x101);
    local_24 = 0;
    pbVar8 = &DAT_10028560 + (int)local_20 * 0x30;
    local_20 = pbVar8;
    do {
      for (; (*pbVar8 != 0 && (bVar3 = pbVar8[1], bVar3 != 0)); pbVar8 = pbVar8 + 2) {
        for (uVar5 = (uint)*pbVar8; uVar5 <= bVar3; uVar5 = uVar5 + 1) {
          pbVar2 = (byte *)(param_2 + 0x1d + uVar5);
          *pbVar2 = *pbVar2 | *(byte *)(local_24 + 0x1002854c);
          bVar3 = pbVar8[1];
        }
      }
      local_24 = local_24 + 1;
      pbVar8 = local_20 + 8;
      local_20 = pbVar8;
    } while (local_24 < 4);
    *(uint *)(param_2 + 4) = uVar4;
    *(undefined4 *)(param_2 + 8) = 1;
    uVar9 = FUN_100144d6();
    *(undefined4 *)(param_2 + 0xc) = uVar9;
    puVar7 = (undefined2 *)(param_2 + 0x10);
    puVar10 = (undefined2 *)(&DAT_10028554 + extraout_ECX);
    iVar11 = 6;
    do {
      *puVar7 = *puVar10;
      puVar10 = puVar10 + 1;
      puVar7 = puVar7 + 1;
      iVar11 = iVar11 + -1;
    } while (iVar11 != 0);
    goto LAB_1001496f;
  }
LAB_1001482b:
  setSBCS(unaff_EDI);
LAB_100149cc:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
code_r0x1001484a:
  local_20 = (byte *)((int)local_20 + 1);
  uVar5 = uVar5 + 0x30;
  if (0xef < uVar5) goto code_r0x10014857;
  goto LAB_1001483e;
code_r0x10014857:
  if (((uVar4 == 65000) || (uVar4 == 0xfde9)) ||
     (BVar6 = IsValidCodePage(uVar4 & 0xffff), BVar6 == 0)) goto LAB_100149cc;
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
      uVar9 = FUN_100144d6();
      *(undefined4 *)(param_2 + 0xc) = uVar9;
      *(undefined4 *)(param_2 + 8) = extraout_EDX;
    }
    *(undefined4 *)(param_2 + 0x10) = 0;
    *(undefined4 *)(param_2 + 0x14) = 0;
    *(undefined4 *)(param_2 + 0x18) = 0;
LAB_1001496f:
    setSBUpLow(unaff_EDI);
    goto LAB_100149cc;
  }
  if (DAT_10059ea4 == 0) goto LAB_100149cc;
  goto LAB_1001482b;
}


// 100149db __setmbcp

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
    ptVar3 = (pthreadmbcinfo)FUN_100099a0(0x220);
    if (ptVar3 != (pthreadmbcinfo)0x0) {
      ptVar7 = p_Var1->ptmbcinfo;
      ptVar8 = ptVar3;
      for (iVar6 = 0x88; iVar6 != 0; iVar6 = iVar6 + -1) {
        ptVar8->refcount = ptVar7->refcount;
        ptVar7 = (pthreadmbcinfo)&ptVar7->mbcodepage;
        ptVar8 = (pthreadmbcinfo)&ptVar8->mbcodepage;
      }
      ptVar3->refcount = 0;
      local_24 = FUN_10014802(iVar2,(int)ptVar3);
      if (local_24 == 0) {
        LVar4 = InterlockedDecrement(&p_Var1->ptmbcinfo->refcount);
        if ((LVar4 == 0) && (p_Var1->ptmbcinfo != (pthreadmbcinfo)&DAT_10028120)) {
          FUN_10009a00(p_Var1->ptmbcinfo);
        }
        p_Var1->ptmbcinfo = ptVar3;
        InterlockedIncrement((LONG *)ptVar3);
        if (((p_Var1->_ownlocale & 2) == 0) && (((byte)DAT_10028644 & 1) == 0)) {
          __lock(0xd);
          _DAT_10059eb4 = ptVar3->mbcodepage;
          _DAT_10059eb8 = ptVar3->ismbcodepage;
          _DAT_10059ebc = *(undefined4 *)ptVar3->mbulinfo;
          for (iVar2 = 0; iVar2 < 5; iVar2 = iVar2 + 1) {
            (&DAT_10059ea8)[iVar2] = ptVar3->mbulinfo[iVar2 + 2];
          }
          for (iVar2 = 0; iVar2 < 0x101; iVar2 = iVar2 + 1) {
            (&DAT_10028340)[iVar2] = ptVar3->mbctype[iVar2 + 4];
          }
          for (iVar2 = 0; iVar2 < 0x100; iVar2 = iVar2 + 1) {
            (&DAT_10028448)[iVar2] = ptVar3->mbcasemap[iVar2 + 4];
          }
          LVar4 = InterlockedDecrement((LONG *)PTR_DAT_10028548);
          if ((LVar4 == 0) && (PTR_DAT_10028548 != &DAT_10028120)) {
            FUN_10009a00(PTR_DAT_10028548);
          }
          PTR_DAT_10028548 = (undefined *)ptVar3;
          InterlockedIncrement((LONG *)ptVar3);
          FUN_10014b3c();
        }
      }
      else if (local_24 == -1) {
        if (ptVar3 != (pthreadmbcinfo)&DAT_10028120) {
          FUN_10009a00(ptVar3);
        }
        pDVar5 = FUN_10012790();
        *pDVar5 = 0x16;
      }
    }
  }
  return local_24;
}


// 10014b3c FUN_10014b3c

void FUN_10014b3c(void)

{
  FUN_10015425(0xd);
  return;
}


// 10014b75 ___initmbctable

/* Library Function - Single Match
    ___initmbctable
   
   Library: Visual Studio 2005 Release */

undefined4 ___initmbctable(void)

{
  if (DAT_1005b5f0 == 0) {
    __setmbcp(-3);
    DAT_1005b5f0 = 1;
  }
  return 0;
}


// 10014b93 ___freetlocinfo

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
       (*(undefined ***)(param_1 + 0xbc) != &PTR_DAT_10028d50)) &&
      (*(int **)(param_1 + 0xb0) != (int *)0x0)) && (**(int **)(param_1 + 0xb0) == 0)) {
    piVar4 = *(int **)(param_1 + 0xb8);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_10009a00(piVar4);
      ___free_lconv_mon(*(int *)(param_1 + 0xbc));
    }
    piVar4 = *(int **)(param_1 + 0xb4);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_10009a00(piVar4);
      ___free_lconv_num(*(int **)(param_1 + 0xbc));
    }
    FUN_10009a00(*(undefined4 *)(param_1 + 0xb0));
    FUN_10009a00(*(undefined4 *)(param_1 + 0xbc));
  }
  if ((*(int **)(param_1 + 0xc0) != (int *)0x0) && (**(int **)(param_1 + 0xc0) == 0)) {
    FUN_10009a00(*(int *)(param_1 + 0xc4) + -0xfe);
    FUN_10009a00(*(int *)(param_1 + 0xcc) + -0x80);
    FUN_10009a00(*(int *)(param_1 + 0xd0) + -0x80);
    FUN_10009a00(*(undefined4 *)(param_1 + 0xc0));
  }
  ppuVar1 = *(undefined ***)(param_1 + 0xd4);
  if ((ppuVar1 != &PTR_DAT_10028c90) && (ppuVar1[0x2d] == (undefined *)0x0)) {
    ___free_lc_time(ppuVar1);
    FUN_10009a00(*(undefined4 *)(param_1 + 0xd4));
  }
  piVar4 = (int *)(param_1 + 0x50);
  iVar3 = 6;
  do {
    if ((((undefined *)piVar4[-2] != &DAT_10028648) &&
        (piVar2 = (int *)*piVar4, piVar2 != (int *)0x0)) && (*piVar2 == 0)) {
      FUN_10009a00(piVar2);
    }
    if (((piVar4[-1] != 0) && (piVar2 = (int *)piVar4[1], piVar2 != (int *)0x0)) && (*piVar2 == 0))
    {
      FUN_10009a00(piVar2);
    }
    piVar4 = piVar4 + 4;
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  FUN_10009a00(param_1);
  return;
}


// 10014cd3 ___addlocaleref

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
    if (((undefined *)pLVar1[-2] != &DAT_10028648) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 10014d59 ___removelocaleref

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
      if (((undefined *)pLVar1[-2] != &DAT_10028648) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 10014de5 __updatetlocinfoEx_nolock

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
        if ((*pLVar1 == 0) && (pLVar1 != (LONG *)&DAT_10028650)) {
          ___freetlocinfo((int)pLVar1);
        }
      }
    }
    return unaff_EDI;
  }
  return (LONG *)0x0;
}


// 10014e23 ___updatetlocinfo

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___updatetlocinfo
   
   Library: Visual Studio 2005 Release */

pthreadlocinfo __cdecl ___updatetlocinfo(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = __getptd();
  if (((p_Var1->_ownlocale & DAT_10028644) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xc);
    __updatetlocinfoEx_nolock();
    FUN_10014e8d();
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


// 10014e8d FUN_10014e8d

void FUN_10014e8d(void)

{
  FUN_10015425(0xc);
  return;
}


// 10014e99 __security_check_cookie

/* Library Function - Single Match
    @__security_check_cookie@4
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release
   __fastcall __security_check_cookie,4 */

void __fastcall __security_check_cookie(int param_1)

{
  if (param_1 == DAT_10028738) {
    return;
  }
                    /* WARNING: Subroutine does not return */
  ___report_gsfailure();
}


// 10014ea8 __crtLCMapStringA_stat

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  if (DAT_10059ee4 == 0) {
    iVar3 = LCMapStringW(0,0x100,L"",1,(LPWSTR)0x0,0);
    if (iVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_10059ee4 = 2;
      }
    }
    else {
      DAT_10059ee4 = 1;
    }
  }
  pcVar5 = (char *)param_3;
  pcVar8 = param_4;
  if (0 < (int)param_4) {
    do {
      pcVar8 = pcVar8 + -1;
      if (*pcVar5 == '\0') goto LAB_10014f17;
      pcVar5 = pcVar5 + 1;
    } while (pcVar8 != (char *)0x0);
    pcVar8 = (char *)0xffffffff;
LAB_10014f17:
    pcVar5 = param_4 + -(int)pcVar8;
    bVar2 = (int)(pcVar5 + -1) < (int)param_4;
    param_4 = pcVar5 + -1;
    if (bVar2) {
      param_4 = pcVar5;
    }
  }
  if ((DAT_10059ee4 == 2) || (DAT_10059ee4 == 0)) {
    local_10 = (LPCWSTR)0x0;
    local_14 = 0;
    if (param_1 == (localeinfo_struct *)0x0) {
      param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
    }
    if (param_7 == 0) {
      param_7 = *(int *)(*in_ECX + 4);
    }
    UVar7 = ___ansicp((LCID)param_1);
    if (UVar7 == 0xffffffff) goto LAB_10015238;
    if (UVar7 == param_7) {
      LCMapStringA((LCID)param_1,param_2,(LPCSTR)param_3,(int)param_4,(LPSTR)param_5,(int)param_6);
    }
    else {
      local_10 = (LPCWSTR)___convertcp(param_7,UVar7,(char *)param_3,(uint *)&param_4,(LPSTR)0x0,0);
      if (local_10 == (LPCWSTR)0x0) goto LAB_10015238;
      local_c = LCMapStringA((LCID)param_1,param_2,(LPCSTR)local_10,(int)param_4,(LPSTR)0x0,0);
      if (local_c != 0) {
        if (((int)local_c < 1) || (0xffffffe0 < local_c)) {
          pcVar5 = (char *)0x0;
        }
        else if (local_c + 8 < 0x401) {
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_10015215;
          pcVar5 = &stack0xffffffe4;
        }
        else {
          pcVar5 = (char *)FUN_100099a0(local_c + 8);
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
LAB_10015215:
    if (local_10 != (LPCWSTR)0x0) {
      FUN_10009a00(local_10);
    }
    if ((local_14 != 0) && (param_5 != local_14)) {
      FUN_10009a00(local_14);
    }
    goto LAB_10015238;
  }
  if (DAT_10059ee4 != 1) goto LAB_10015238;
  local_c = 0;
  if (param_7 == 0) {
    param_7 = *(int *)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar(param_7,(uint)(param_8 != 0) * 8 + 1,(LPCSTR)param_3,
                                    (int)param_4,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_10015238;
  if (((int)cchWideChar < 1) || (0xffffffe0 / cchWideChar < 2)) {
    local_10 = (LPCWSTR)0x0;
  }
  else {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar6 = (LPCWSTR)&stack0xffffffdc;
      local_10 = (LPCWSTR)&stack0xffffffdc;
      if (&stack0x00000000 != (undefined1 *)0x24) {
LAB_10014fbf:
        local_10 = pWVar6 + 4;
      }
    }
    else {
      pWVar6 = (LPCWSTR)FUN_100099a0(uVar1);
      local_10 = pWVar6;
      if (pWVar6 != (LPCWSTR)0x0) {
        pWVar6[0] = L'\xdddd';
        pWVar6[1] = L'\0';
        goto LAB_10014fbf;
      }
    }
  }
  if (local_10 == (LPCWSTR)0x0) goto LAB_10015238;
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
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_100150ce;
          pWVar6 = (LPCWSTR)&stack0xffffffe4;
        }
        else {
          pWVar6 = (LPCWSTR)FUN_100099a0(uVar1);
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
LAB_100150ce:
  __freea(local_10);
LAB_10015238:
  iVar3 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar3;
}


// 1001524a ___crtLCMapStringA

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


// 1001528d __isleadbyte_l

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


// 100152c3 _isleadbyte

/* Library Function - Single Match
    _isleadbyte
   
   Library: Visual Studio 2005 Release */

int __cdecl _isleadbyte(int _C)

{
  int iVar1;
  
  iVar1 = __isleadbyte_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 100152d1 __isctype_l

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


// 10015387 __mtinitlocks

/* Library Function - Single Match
    __mtinitlocks
   
   Library: Visual Studio 2005 Release */

int __cdecl __mtinitlocks(void)

{
  int iVar1;
  int iVar2;
  undefined *puVar3;
  
  iVar2 = 0;
  puVar3 = &DAT_10059ee8;
  do {
    if ((&DAT_10028744)[iVar2 * 2] == 1) {
      (&DAT_10028740)[iVar2 * 2] = puVar3;
      puVar3 = puVar3 + 0x18;
      iVar1 = ___crtInitCritSecAndSpinCount((&DAT_10028740)[iVar2 * 2],4000);
      if (iVar1 == 0) {
        (&DAT_10028740)[iVar2 * 2] = 0;
        return 0;
      }
    }
    iVar2 = iVar2 + 1;
  } while (iVar2 < 0x24);
  return 1;
}


// 100153d0 __mtdeletelocks

/* Library Function - Single Match
    __mtdeletelocks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __cdecl __mtdeletelocks(void)

{
  LPCRITICAL_SECTION lpCriticalSection;
  undefined4 *puVar1;
  
  puVar1 = &DAT_10028740;
  do {
    lpCriticalSection = (LPCRITICAL_SECTION)*puVar1;
    if ((lpCriticalSection != (LPCRITICAL_SECTION)0x0) && (puVar1[1] != 1)) {
      DeleteCriticalSection(lpCriticalSection);
      FUN_10009a00(lpCriticalSection);
      *puVar1 = 0;
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10028860);
  puVar1 = &DAT_10028740;
  do {
    if (((LPCRITICAL_SECTION)*puVar1 != (LPCRITICAL_SECTION)0x0) && (puVar1[1] == 1)) {
      DeleteCriticalSection((LPCRITICAL_SECTION)*puVar1);
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10028860);
  return;
}


// 10015425 FUN_10015425

void __cdecl FUN_10015425(int param_1)

{
  LeaveCriticalSection((LPCRITICAL_SECTION)(&DAT_10028740)[param_1 * 2]);
  return;
}


// 1001543a __mtinitlocknum

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
  if (DAT_10027590 == 0) {
    __FF_MSGBANNER();
    FUN_10010c19(0x1e);
    ___crtExitProcess(0xff);
  }
  piVar1 = &DAT_10028740 + _LockNum * 2;
  if (*piVar1 == 0) {
    iVar4 = FUN_100099a0(0x18);
    if (iVar4 == 0) {
      pDVar2 = FUN_10012790();
      *pDVar2 = 0xc;
      iVar4 = 0;
    }
    else {
      __lock(10);
      if (*piVar1 == 0) {
        iVar3 = ___crtInitCritSecAndSpinCount(iVar4,4000);
        if (iVar3 == 0) {
          FUN_10009a00(iVar4);
          pDVar2 = FUN_10012790();
          *pDVar2 = 0xc;
          local_20 = 0;
        }
        else {
          *piVar1 = iVar4;
        }
      }
      else {
        FUN_10009a00(iVar4);
      }
      FUN_100154f4();
      iVar4 = local_20;
    }
  }
  return iVar4;
}


// 100154f4 FUN_100154f4

void FUN_100154f4(void)

{
  FUN_10015425(10);
  return;
}


// 100154fd __lock

/* Library Function - Single Match
    __lock
   
   Library: Visual Studio 2005 Release */

void __cdecl __lock(int _File)

{
  int iVar1;
  
  if ((&DAT_10028740)[_File * 2] == 0) {
    iVar1 = __mtinitlocknum(_File);
    if (iVar1 == 0) {
      __amsg_exit(0x11);
    }
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(&DAT_10028740)[_File * 2]);
  return;
}


// 1001552e __fltin2

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  local_24 = _Str;
  uVar3 = 0;
  local_18 = FUN_1001566b((undefined2 *)&local_14,&local_28,_Str,0,0,0,0,in_stack_00000018);
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


// 100155e0 _strlen

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
    if ((char)uVar1 == '\0') goto LAB_10015643;
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
LAB_10015643:
  return (size_t)((int)puVar2 + (-1 - (int)_Str));
}


// 1001566b FUN_1001566b

void __cdecl
FUN_1001566b(undefined2 *param_1,int *param_2,char *param_3,int param_4,int param_5,int param_6,
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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
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
LAB_100156f0:
    cVar8 = *param_3;
    pcVar17 = param_3 + 1;
    switch(iVar14) {
    case 0:
      if ((byte)(cVar8 - 0x31U) < 9) {
LAB_1001570d:
        iVar14 = 3;
        goto LAB_1001570f;
      }
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) {
LAB_10015724:
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
          if (cVar8 == '0') goto LAB_1001573e;
          goto LAB_100158c9;
        }
        iVar14 = 2;
        uVar7 = 0x8000;
        param_3 = pcVar17;
      }
      goto LAB_100156f0;
    case 1:
      bVar4 = true;
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_1001570d;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10015775;
      if ((cVar8 == '+') || (cVar8 == '-')) goto LAB_100157a5;
      if (cVar8 == '0') goto LAB_1001573e;
      goto LAB_10015785;
    case 2:
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_1001570d;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10015724;
      param_3 = local_58;
      if (cVar8 != '0') goto LAB_100158f4;
LAB_1001573e:
      iVar14 = 1;
      param_3 = pcVar17;
      goto LAB_100156f0;
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
      if (cVar8 != *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10015818;
LAB_10015775:
      bVar4 = true;
      iVar14 = 4;
      param_3 = pcVar17;
      goto LAB_100156f0;
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
LAB_10015818:
      if ((cVar8 == '+') || (cVar8 == '-')) {
LAB_100157a5:
        bVar4 = true;
        iVar14 = 0xb;
        param_3 = pcVar17 + -1;
      }
      else {
LAB_10015785:
        bVar4 = true;
        if ((cVar8 < 'D') || (('E' < cVar8 && ((cVar8 < 'd' || ('e' < cVar8)))))) goto LAB_100158c9;
        iVar14 = 6;
        param_3 = pcVar17;
      }
      goto LAB_100156f0;
    case 5:
      bVar6 = true;
      param_3 = local_58;
      if ((byte)(cVar8 - 0x30U) < 10) {
        iVar14 = 4;
        goto LAB_1001570f;
      }
      goto LAB_100158f4;
    case 6:
      local_58 = param_3 + -1;
      if (8 < (byte)(cVar8 - 0x31U)) {
        if (cVar8 == '+') goto LAB_100158b0;
        if (cVar8 == '-') goto LAB_100158a4;
LAB_10015897:
        param_3 = local_58;
        if (cVar8 != '0') goto LAB_100158f4;
        iVar14 = 8;
        param_3 = pcVar17;
        goto LAB_100156f0;
      }
      break;
    case 7:
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_10015897;
      break;
    case 8:
      bVar5 = true;
      while (cVar8 == '0') {
        cVar8 = *pcVar17;
        pcVar17 = pcVar17 + 1;
      }
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_100158c9;
      break;
    case 9:
      bVar5 = true;
      local_68 = 0;
      goto LAB_10015956;
    default:
      goto switchD_100156fc_caseD_a;
    case 0xb:
      if (param_7 != 0) {
        local_58 = param_3;
        if (cVar8 == '+') {
LAB_100158b0:
          iVar14 = 7;
          param_3 = pcVar17;
        }
        else {
          if (cVar8 != '-') goto LAB_100158f4;
LAB_100158a4:
          local_6c = -1;
          iVar14 = 7;
          param_3 = pcVar17;
        }
        goto LAB_100156f0;
      }
      iVar14 = 10;
      pcVar17 = param_3;
switchD_100156fc_caseD_a:
      param_3 = pcVar17;
      if (iVar14 != 10) goto LAB_100156f0;
      goto LAB_100158f4;
    }
    iVar14 = 9;
LAB_1001570f:
    param_3 = pcVar17 + -1;
    goto LAB_100156f0;
  }
  pDVar10 = FUN_10012790();
  *pDVar10 = 0x16;
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
  goto LAB_10015cec;
LAB_10015956:
  if ((cVar8 < '0') || ('9' < cVar8)) goto LAB_10015971;
  local_68 = local_68 * 10 + -0x30 + (int)cVar8;
  if (local_68 < 0x1451) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
    goto LAB_10015956;
  }
  local_68 = 0x1451;
LAB_10015971:
  while (('/' < cVar8 && (cVar8 < ':'))) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
  }
LAB_100158c9:
  param_3 = pcVar17 + -1;
LAB_100158f4:
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
    if (local_50 == 0) goto LAB_10015cce;
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
      if ((int)local_58 < -0x1450) goto LAB_10015cce;
      ppuVar20 = &PTR_DAT_10028d60;
      if (local_58 != (char *)0x0) {
        if ((int)local_58 < 0) {
          local_58 = (char *)-(int)local_58;
          ppuVar20 = (undefined **)&DAT_10028ec0;
        }
        if (param_4 == 0) {
          local_40._0_2_ = 0;
        }
        iVar14 = uStack_4a;
        uVar22 = uStack_2e;
        iVar1 = uStack_2a;
        uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
        iVar12 = local_3c;
joined_r0x100159fc:
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
LAB_10015bb2:
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
                    if ((short)uVar24 < 1) goto LAB_10015bb2;
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
                  if (0x7ffe < uVar24) goto LAB_10015c71;
                  local_40 = uVar22;
                  local_3c = iVar1;
                  uStack_38 = uStack_26;
                  local_36 = uVar24 | uVar9;
                  uVar2 = uVar22;
                  iVar12 = iVar1;
                }
                goto joined_r0x100159fc;
              }
              iVar12 = 0;
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            else {
LAB_10015c71:
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
          goto joined_r0x100159fc;
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
LAB_10015cce:
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
LAB_10015cec:
  local_40 = uVar2;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10015d2b std::exception::exception

/* Library Function - Single Match
    public: __thiscall std::exception::exception(char const * const &)
   
   Library: Visual Studio 2005 Release */

exception * __thiscall std::exception::exception(exception *this,char **param_1)

{
  size_t sVar1;
  char *_Dst;
  
  *(undefined ***)this = &PTR_FUN_100225ac;
  if (*param_1 == (char *)0x0) {
    *(undefined4 *)(this + 4) = 0;
  }
  else {
    sVar1 = _strlen(*param_1);
    _Dst = (char *)FUN_100099a0(sVar1 + 1);
    *(char **)(this + 4) = _Dst;
    if (_Dst != (char *)0x0) {
      _strcpy_s(_Dst,sVar1 + 1,*param_1);
    }
  }
  *(undefined4 *)(this + 8) = 1;
  return this;
}


// 10015d79 FUN_10015d79

undefined4 * __thiscall FUN_10015d79(void *this,int param_1)

{
  int iVar1;
  size_t sVar2;
  char *pcVar3;
  
  *(undefined ***)this = &PTR_FUN_100225ac;
  iVar1 = *(int *)(param_1 + 8);
  *(int *)((int)this + 8) = iVar1;
  pcVar3 = *(char **)(param_1 + 4);
  if (iVar1 == 0) {
    *(char **)((int)this + 4) = pcVar3;
  }
  else if (pcVar3 == (char *)0x0) {
    *(undefined4 *)((int)this + 4) = 0;
  }
  else {
    sVar2 = _strlen(pcVar3);
    pcVar3 = (char *)FUN_100099a0(sVar2 + 1);
    *(char **)((int)this + 4) = pcVar3;
    if (pcVar3 != (char *)0x0) {
      _strcpy_s(pcVar3,sVar2 + 1,*(char **)(param_1 + 4));
    }
  }
  return (undefined4 *)this;
}


// 10015dd1 exception::~exception

/* Library Function - Single Match
    public: virtual __thiscall exception::~exception(void)
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release */

void __thiscall exception::~exception(exception *this)

{
  *(undefined ***)this = &PTR_FUN_100225ac;
  if (*(int *)(this + 8) != 0) {
    FUN_10009a00(*(undefined4 *)(this + 4));
  }
  return;
}


// 10015df4 std::bad_cast::bad_cast

/* Library Function - Single Match
    public: __thiscall std::bad_cast::bad_cast(char const *)
   
   Library: Visual Studio 2005 Release */

bad_cast * __thiscall std::bad_cast::bad_cast(bad_cast *this,char *param_1)

{
  exception::exception((exception *)this,&param_1);
  *(undefined ***)this = &PTR_FUN_100225cc;
  return this;
}


// 10015e9c FUN_10015e9c

exception * __thiscall FUN_10015e9c(void *this,byte param_1)

{
  exception::~exception((exception *)this);
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (exception *)this;
}


// 10015eb8 FUN_10015eb8

exception * __thiscall FUN_10015eb8(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_100225cc;
  exception::~exception((exception *)this);
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (exception *)this;
}


// 10015eda FUN_10015eda

exception * __thiscall FUN_10015eda(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_100225d8;
  exception::~exception((exception *)this);
  if ((param_1 & 1) != 0) {
    FUN_10009a00(this);
  }
  return (exception *)this;
}


// 10015efc __CxxThrowException@8

/* Library Function - Single Match
    __CxxThrowException@8
   
   Library: Visual Studio 2005 Release */

void __CxxThrowException_8(undefined4 param_1,byte *param_2)

{
  int iVar1;
  DWORD *pDVar2;
  DWORD *pDVar3;
  DWORD local_24 [4];
  DWORD local_14;
  ULONG_PTR local_10;
  undefined4 local_c;
  byte *local_8;
  
  pDVar2 = &DAT_100225ec;
  pDVar3 = local_24;
  for (iVar1 = 8; iVar1 != 0; iVar1 = iVar1 + -1) {
    *pDVar3 = *pDVar2;
    pDVar2 = pDVar2 + 1;
    pDVar3 = pDVar3 + 1;
  }
  local_c = param_1;
  local_8 = param_2;
  if ((param_2 != (byte *)0x0) && ((*param_2 & 8) != 0)) {
    local_10 = 0x1994000;
  }
  RaiseException(local_24[0],local_24[1],local_14,&local_10);
  return;
}


// 10015f50 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_1001566b((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tod(&local_14,(_CRT_DOUBLE *)_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_10015fa7:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10015fe7;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_10015fd9:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10015fe7;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_10015fd9;
    goto LAB_10015fa7;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_10015fe7:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 10015ff6 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_1001566b((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tof(&local_14,_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_1001604d:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1001608d;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_1001607f:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_1001608d;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_1001607f;
    goto LAB_1001604d;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_1001608d:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 1001609c _strcpy_s

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
      pDVar2 = FUN_10012790();
      eVar4 = 0x22;
      *pDVar2 = 0x22;
      goto LAB_100160bb;
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_10012790();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_100160bb:
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 10016101 __fptostr

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
    pDVar1 = FUN_10012790();
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
    pDVar1 = FUN_10012790();
    eVar7 = 0x22;
    *pDVar1 = 0x22;
  }
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar7;
}


// 100161be ___dtold

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
      goto LAB_10016270;
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
LAB_10016270:
  *(ushort *)(param_1 + 2) = uVar4;
  return;
}


// 10016279 __fltout2

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  ___dtold(&local_14,(uint *)&_Dbl);
  iVar1 = FUN_1001a434(local_14,uStack_10,uStack_c,0x11,0,&local_30);
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


// 10016310 __alldvrm

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


// 100163f0 __aullshr

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


// 10016418 __controlfp_s

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
    pDVar2 = FUN_10012790();
    eVar3 = 0x16;
    *pDVar2 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return eVar3;
}


// 10016482 __initp_misc_winsig

/* Library Function - Single Match
    __initp_misc_winsig
   
   Library: Visual Studio 2005 Release */

void __cdecl __initp_misc_winsig(undefined4 param_1)

{
  DAT_1005a078 = param_1;
  DAT_1005a07c = param_1;
  DAT_1005a080 = param_1;
  DAT_1005a084 = param_1;
  return;
}


// 1001649b siglookup

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
  } while (uVar1 < DAT_1002811c * 0xc + param_1);
  if ((DAT_1002811c * 0xc + param_1 <= uVar1) || (*(int *)(uVar1 + 4) != in_EDX)) {
    uVar1 = 0;
  }
  return uVar1;
}


// 100164cf ___get_sigabrt

/* Library Function - Single Match
    ___get_sigabrt
   
   Library: Visual Studio 2005 Release */

_PHNDLR __cdecl ___get_sigabrt(void)

{
  _PHNDLR p_Var1;
  
  p_Var1 = (_PHNDLR)FUN_10010ed5(DAT_1005a080);
  return p_Var1;
}


// 100164dc _raise

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
        puVar6 = &DAT_1005a078;
        iVar3 = DAT_1005a078;
        goto LAB_10016591;
      }
      if (_SigNum != 4) {
        if (_SigNum == 6) goto LAB_1001656f;
        if (_SigNum != 8) goto LAB_10016553;
      }
    }
    pDVar7 = FUN_1001107a();
    if (pDVar7 == (DWORD *)0x0) {
      return -1;
    }
    uVar2 = siglookup(pDVar7[0x17]);
    puVar6 = (undefined4 *)(uVar2 + 8);
    pcVar4 = (code *)*puVar6;
  }
  else {
    if (_SigNum == 0xf) {
      puVar6 = &DAT_1005a084;
      iVar3 = DAT_1005a084;
    }
    else if (_SigNum == 0x15) {
      puVar6 = &DAT_1005a07c;
      iVar3 = DAT_1005a07c;
    }
    else {
      if (_SigNum != 0x16) {
LAB_10016553:
        pDVar7 = FUN_10012790();
        *pDVar7 = 0x16;
        FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return -1;
      }
LAB_1001656f:
      puVar6 = &DAT_1005a080;
      iVar3 = DAT_1005a080;
    }
LAB_10016591:
    bVar1 = true;
    pcVar4 = (code *)FUN_10010ed5(iVar3);
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
      goto LAB_100165f5;
    }
  }
  else {
LAB_100165f5:
    if (_SigNum == 8) {
      for (local_28 = DAT_10028110; local_28 < DAT_10028114 + DAT_10028110; local_28 = local_28 + 1)
      {
        *(undefined4 *)(local_28 * 0xc + 8 + pDVar7[0x17]) = 0;
      }
      goto LAB_1001662f;
    }
  }
  uVar5 = __encoded_null();
  *puVar6 = uVar5;
LAB_1001662f:
  FUN_10016650();
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


// 10016650 FUN_10016650

void FUN_10016650(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + -0x1c) != 0) {
    FUN_10015425(0);
  }
  return;
}


// 1001668c FUN_1001668c

undefined4 __cdecl FUN_1001668c(undefined4 param_1,undefined4 param_2,uint param_3)

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
  if (DAT_1005a08c == 0) {
    hModule = LoadLibraryA("USER32.DLL");
    if (hModule == (HMODULE)0x0) {
      return 0;
    }
    pFVar1 = GetProcAddress(hModule,"MessageBoxA");
    if (pFVar1 == (FARPROC)0x0) {
      return 0;
    }
    DAT_1005a08c = FUN_10010e5e((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetActiveWindow");
    DAT_1005a090 = FUN_10010e5e((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetLastActivePopup");
    DAT_1005a094 = FUN_10010e5e((int)pFVar1);
    iVar2 = __get_osplatform(&local_10);
    if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
    if (local_10 == 2) {
      pFVar1 = GetProcAddress(hModule,"GetUserObjectInformationA");
      DAT_1005a09c = FUN_10010e5e((int)pFVar1);
      if (DAT_1005a09c != 0) {
        pFVar1 = GetProcAddress(hModule,"GetProcessWindowStation");
        DAT_1005a098 = FUN_10010e5e((int)pFVar1);
      }
    }
  }
  if ((DAT_1005a098 != local_c) && (DAT_1005a09c != local_c)) {
    pcVar3 = (code *)FUN_10010ed5(DAT_1005a098);
    pcVar4 = (code *)FUN_10010ed5(DAT_1005a09c);
    if (((pcVar3 != (code *)0x0) && (pcVar4 != (code *)0x0)) &&
       (((iVar2 = (*pcVar3)(), iVar2 == 0 ||
         (iVar2 = (*pcVar4)(iVar2,1,local_24,0xc,local_18), iVar2 == 0)) || ((local_1c & 1) == 0))))
    {
      iVar2 = FUN_1001167f(&local_14);
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
      goto LAB_10016822;
    }
  }
  if ((((DAT_1005a090 != local_c) &&
       (pcVar3 = (code *)FUN_10010ed5(DAT_1005a090), pcVar3 != (code *)0x0)) &&
      (local_8 = (*pcVar3)(), local_8 != 0)) &&
     ((DAT_1005a094 != local_c &&
      (pcVar3 = (code *)FUN_10010ed5(DAT_1005a094), pcVar3 != (code *)0x0)))) {
    local_8 = (*pcVar3)(local_8);
  }
LAB_10016822:
  pcVar3 = (code *)FUN_10010ed5(DAT_1005a08c);
  if (pcVar3 == (code *)0x0) {
    return 0;
  }
  uVar5 = (*pcVar3)(local_8,param_1,param_2,param_3);
  return uVar5;
}


// 10016849 _strcat_s

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
        pDVar2 = FUN_10012790();
        eVar4 = 0x22;
        *pDVar2 = 0x22;
        goto LAB_10016868;
      }
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_10012790();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_10016868:
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 100168ba _strncpy_s

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
LAB_100168de:
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
          pDVar2 = FUN_10012790();
          eVar5 = 0x22;
          *pDVar2 = 0x22;
          goto LAB_100168ef;
        }
        *_Dst = '\0';
      }
    }
  }
  else if (_Dst != (char *)0x0) goto LAB_100168de;
  pDVar2 = FUN_10012790();
  eVar5 = 0x16;
  *pDVar2 = 0x16;
LAB_100168ef:
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar5;
}


// 1001696d __set_error_mode

/* Library Function - Single Match
    __set_error_mode
   
   Library: Visual Studio 2005 Release */

int __cdecl __set_error_mode(int _Mode)

{
  int iVar1;
  DWORD *pDVar2;
  
  if (-1 < _Mode) {
    if (_Mode < 3) {
      iVar1 = DAT_10059a08;
      DAT_10059a08 = _Mode;
      return iVar1;
    }
    if (_Mode == 3) {
      return DAT_10059a08;
    }
  }
  pDVar2 = FUN_10012790();
  *pDVar2 = 0x16;
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 100169b3 FUN_100169b3

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

undefined4 FUN_100169b3(void)

{
  return 1;
}


// 10016a03 __get_sse2_info

/* WARNING: Removing unreachable block (ram,0x10016a3e) */
/* WARNING: Removing unreachable block (ram,0x10016a2b) */
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
  if (((local_8 & 0x4000000) == 0) || (iVar2 = FUN_100169b3(), iVar2 == 0)) {
    uVar3 = 0;
  }
  else {
    uVar3 = 1;
  }
  return uVar3;
}


// 10016a70 __ValidateImageBase

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


// 10016aa0 __FindPESection

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


// 10016af0 __IsNonwritableInCurrentImage

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
  local_c = DAT_10028738 ^ 0x10024c80;
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


// 10016be4 __initp_eh_hooks

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __initp_eh_hooks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __initp_eh_hooks(void)

{
  _DAT_1005a0a0 = FUN_10010e5e(0x10016bab);
  return;
}


// 10016bf5 FUN_10016bf5

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10016bf5(undefined4 param_1)

{
  _DAT_1005a0a4 = param_1;
  return;
}


// 10016bff FUN_10016bff

void __cdecl FUN_10016bff(undefined4 param_1)

{
  DAT_1005a0a8 = param_1;
  return;
}


// 10016c09 ___crtInitCritSecNoSpinCount@8

/* Library Function - Single Match
    ___crtInitCritSecNoSpinCount@8
   
   Library: Visual Studio 2005 Release */

undefined4 ___crtInitCritSecNoSpinCount_8(LPCRITICAL_SECTION param_1)

{
  InitializeCriticalSection(param_1);
  return 1;
}


// 10016c19 ___crtInitCritSecAndSpinCount

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
  
  local_8 = &DAT_10024cc0;
  uStack_c = 0x10016c25;
  local_20[0] = 0;
  pcVar1 = (code *)FUN_10010ed5(DAT_1005a0a8);
  if (pcVar1 != (FARPROC)0x0) goto LAB_10016c8f;
  iVar2 = __get_osplatform(local_20);
  if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  if (local_20[0] == 1) {
LAB_10016c7e:
    pcVar1 = ___crtInitCritSecNoSpinCount_8;
  }
  else {
    hModule = GetModuleHandleA("kernel32.dll");
    if (hModule == (HMODULE)0x0) goto LAB_10016c7e;
    pcVar1 = GetProcAddress(hModule,"InitializeCriticalSectionAndSpinCount");
    if (pcVar1 == (FARPROC)0x0) goto LAB_10016c7e;
  }
  DAT_1005a0a8 = FUN_10010e5e((int)pcVar1);
LAB_10016c8f:
  local_8 = (undefined *)0x0;
  iVar2 = (*pcVar1)(param_1,param_2);
  return iVar2;
}


// 10016cde FUN_10016cde

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10016cde(undefined4 param_1)

{
  _DAT_1005a0ac = param_1;
  return;
}


// 10016ce8 __local_unwind4

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
  puStack_24 = &LAB_10016d78;
  uStack_28 = *unaff_FS_OFFSET;
  local_20 = DAT_10028738 ^ (uint)&uStack_28;
  *unaff_FS_OFFSET = &uStack_28;
  while( true ) {
    uVar2 = *(uint *)(param_2 + 0xc);
    if ((uVar2 == 0xfffffffe) || ((param_3 != 0xfffffffe && (uVar2 <= param_3)))) break;
    puVar1 = (undefined4 *)((*(uint *)(param_2 + 8) ^ *param_1) + 0x10 + uVar2 * 0xc);
    *(undefined4 *)(param_2 + 0xc) = *puVar1;
    if (puVar1[1] == 0) {
      __NLG_Notify(0x101);
      FUN_1001b260();
    }
  }
  *unaff_FS_OFFSET = uStack_28;
  return;
}


// 10016dbe FUN_10016dbe

void FUN_10016dbe(int param_1)

{
  __local_unwind4(*(uint **)(param_1 + 0x28),*(int *)(param_1 + 0x18),*(uint *)(param_1 + 0x1c));
  return;
}


// 10016dda _EH4_CallFilterFunc

/* Library Function - Single Match
    @_EH4_CallFilterFunc@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_CallFilterFunc,8 */

void __fastcall _EH4_CallFilterFunc(undefined *param_1)

{
  (*(code *)param_1)();
  return;
}


// 10016df1 _EH4_TransferToHandler

/* Library Function - Single Match
    @_EH4_TransferToHandler@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_TransferToHandler,8 */

void __fastcall _EH4_TransferToHandler(undefined *UNRECOVERED_JUMPTABLE)

{
  __NLG_Notify(1);
                    /* WARNING: Could not recover jumptable at 0x10016e08. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (*(code *)UNRECOVERED_JUMPTABLE)();
  return;
}


// 10016e0a _EH4_GlobalUnwind

/* Library Function - Single Match
    @_EH4_GlobalUnwind@4
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_GlobalUnwind,4 */

void __fastcall _EH4_GlobalUnwind(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x10016e1f,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 10016e24 _EH4_LocalUnwind

/* Library Function - Single Match
    @_EH4_LocalUnwind@16
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_LocalUnwind,16 */

void __fastcall _EH4_LocalUnwind(int param_1,uint param_2,undefined4 param_3,uint *param_4)

{
  __local_unwind4(param_4,param_1,param_2);
  return;
}


// 10016e3b __lseeki64_nolock

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
    pDVar2 = FUN_10012790();
    *pDVar2 = 9;
LAB_10016e6a:
    DVar3 = 0xffffffff;
    local_8 = -1;
  }
  else {
    DVar3 = SetFilePointer(hFile,in_stack_00000008,&local_8,_Offset._4_4_);
    if (DVar3 == 0xffffffff) {
      DVar4 = GetLastError();
      if (DVar4 != 0) {
        FUN_100127b6(DVar4);
        goto LAB_10016e6a;
      }
    }
    pbVar1 = (byte *)((&DAT_1005b4e0)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38);
    *pbVar1 = *pbVar1 & 0xfd;
  }
  return CONCAT44(local_8,DVar3);
}


// 10016ebe __lseeki64

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
    pDVar1 = FUN_100127a3();
    *pDVar1 = 0;
    pDVar1 = FUN_10012790();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1005b4c4)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_1005b4e0)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
        pDVar1 = FUN_100127a3();
        *pDVar1 = 0;
        pDVar1 = FUN_10012790();
        *pDVar1 = 9;
        FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        local_28._4_4_ = 0xffffffff;
        local_28._0_4_ = 0xffffffff;
      }
      else {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_1005b4e0)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_10012790();
          *pDVar1 = 9;
          pDVar1 = FUN_100127a3();
          *pDVar1 = 0;
          local_28 = -1;
        }
        else {
          local_28 = __lseeki64_nolock(_FileHandle,_Offset,in_stack_ffffffc8);
        }
        FUN_10016fcd();
      }
      goto LAB_10016fc7;
    }
    pDVar1 = FUN_100127a3();
    *pDVar1 = 0;
    pDVar1 = FUN_10012790();
    *pDVar1 = 9;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  local_28._0_4_ = 0xffffffff;
  local_28._4_4_ = 0xffffffff;
LAB_10016fc7:
  return CONCAT44(local_28._4_4_,(undefined4)local_28);
}


// 10016fcd FUN_10016fcd

void FUN_10016fcd(void)

{
  int unaff_EBP;
  
  FUN_1001b3f5(*(uint *)(unaff_EBP + 8));
  return;
}


// 10016fd7 __write_nolock

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
  
  local_8 = DAT_10028738 ^ (uint)local_51c;
  local_580 = (WCHAR *)_Buf;
  local_57c = (char *)0x0;
  local_584 = 0;
  if (_MaxCharCount == 0) goto LAB_10017587;
  if (_Buf == (void *)0x0) {
    pDVar6 = FUN_100127a3();
    *pDVar6 = 0;
    pDVar6 = FUN_10012790();
    *pDVar6 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_10017587;
  }
  iVar11 = (_FileHandle & 0x1fU) * 0x38;
  piVar16 = &DAT_1005b4e0 + (_FileHandle >> 5);
  local_571 = (char)(*(char *)(*piVar16 + iVar11 + 0x24) * '\x02') >> 1;
  local_58c = piVar16;
  if (((local_571 == '\x02') || (local_571 == '\x01')) && ((~_MaxCharCount & 1) == 0)) {
    pDVar6 = FUN_100127a3();
    *pDVar6 = 0;
    pDVar6 = FUN_10012790();
    *pDVar6 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_10017587;
  }
  if ((*(byte *)(*piVar16 + iVar11 + 4) & 0x20) != 0) {
    __lseeki64_nolock(_FileHandle,0x200000000,unaff_EDI);
  }
  iVar7 = __isatty(_FileHandle);
  if ((iVar7 == 0) || ((*(byte *)(iVar11 + 4 + *piVar16) & 0x80) == 0)) {
LAB_100172c2:
    if ((*(byte *)((undefined4 *)(*piVar16 + iVar11) + 1) & 0x80) == 0) {
      BVar9 = WriteFile(*(HANDLE *)(*piVar16 + iVar11),local_580,_MaxCharCount,(LPDWORD)&local_588,
                        (LPOVERLAPPED)0x0);
      if (BVar9 == 0) {
LAB_1001751c:
        local_570 = GetLastError();
      }
      else {
        local_570 = 0;
        local_57c = local_588;
      }
LAB_10017525:
      piVar16 = local_58c;
      if (local_57c != (char *)0x0) goto LAB_10017587;
      goto LAB_1001752f;
    }
    local_570 = 0;
    if (local_571 == '\0') {
      local_56c = local_580;
      if (_MaxCharCount == 0) goto LAB_10017556;
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
        if (BVar9 == 0) goto LAB_1001751c;
        local_57c = local_57c + (int)local_588;
      } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
              ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
      goto LAB_10017525;
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
          if (BVar9 == 0) goto LAB_1001751c;
          local_57c = local_57c + (int)local_588;
        } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
                ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
        goto LAB_10017525;
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
          if (iVar7 == 0) goto LAB_1001751c;
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
        goto LAB_10017525;
      }
    }
LAB_10017556:
    DVar10 = 0;
    if (((*(byte *)(iVar11 + 4 + *piVar16) & 0x40) != 0) && ((char)*local_580 == '\x1a'))
    goto LAB_10017587;
    pDVar6 = FUN_10012790();
    *pDVar6 = 0x1c;
  }
  else {
    p_Var8 = __getptd();
    pwVar3 = p_Var8->ptlocinfo->lc_category[0].wlocale;
    BVar9 = GetConsoleMode(*(HANDLE *)(iVar11 + *piVar16),&local_598);
    if ((BVar9 == 0) || ((pwVar3 == (wchar_t *)0x0 && (local_571 == '\0')))) goto LAB_100172c2;
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
          if (BVar9 == 0) goto LAB_1001751c;
          local_57c = local_57c + (int)local_56c;
          if ((int)local_56c < (int)DVar10) break;
          if (local_594 != 0) {
            local_10[0] = '\r';
            BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_10,1,(LPDWORD)&local_56c,
                              (LPOVERLAPPED)0x0);
            if (BVar9 == 0) goto LAB_1001751c;
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
            if (wVar5 != (wchar_t)local_570) goto LAB_1001751c;
            local_57c = local_57c + 1;
            if (local_594 != 0) {
              local_570 = 0xd;
              wVar5 = __putwch_nolock(L'\r');
              if (wVar5 != (wchar_t)local_570) goto LAB_1001751c;
              local_57c = local_57c + 1;
              local_584 = local_584 + 1;
            }
          }
        }
      } while (local_578 < _MaxCharCount);
      goto LAB_10017525;
    }
LAB_1001752f:
    if (local_570 == 0) goto LAB_10017556;
    DVar10 = 5;
    if (local_570 != 5) {
      FUN_100127b6(local_570);
      goto LAB_10017587;
    }
    pDVar6 = FUN_10012790();
    *pDVar6 = 9;
  }
  pDVar6 = FUN_100127a3();
  *pDVar6 = DVar10;
LAB_10017587:
  iVar11 = __security_check_cookie(local_8 ^ (uint)local_51c);
  return iVar11;
}


// 1001759d __write

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
    pDVar1 = FUN_100127a3();
    *pDVar1 = 0;
    pDVar1 = FUN_10012790();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1005b4c4)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_1005b4e0)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_1005b4e0)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_10012790();
          *pDVar1 = 9;
          pDVar1 = FUN_100127a3();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = __write_nolock(_FileHandle,_Buf,_MaxCharCount);
        }
        FUN_1001766f();
        return local_20;
      }
    }
    pDVar1 = FUN_100127a3();
    *pDVar1 = 0;
    pDVar1 = FUN_10012790();
    *pDVar1 = 9;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 1001766f FUN_1001766f

void FUN_1001766f(void)

{
  int unaff_EBP;
  
  FUN_1001b3f5(*(uint *)(unaff_EBP + 8));
  return;
}


// 10017679 __getbuf

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __getbuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __getbuf(FILE *_File)

{
  char *pcVar1;
  
  _DAT_1005a0b0 = _DAT_1005a0b0 + 1;
  pcVar1 = (char *)FUN_100099a0(0x1000);
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


// 100176bd __isatty

/* Library Function - Single Match
    __isatty
   
   Library: Visual Studio 2005 Release */

int __cdecl __isatty(int _FileHandle)

{
  DWORD *pDVar1;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_10012790();
    *pDVar1 = 9;
    return 0;
  }
  if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1005b4c4)) {
    return *(byte *)((&DAT_1005b4e0)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38) & 0x40;
  }
  pDVar1 = FUN_10012790();
  *pDVar1 = 9;
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0;
}


// 1001771b FUN_1001771b

undefined ** FUN_1001771b(void)

{
  return &PTR_DAT_10028880;
}


// 100178bc __fileno

/* Library Function - Single Match
    __fileno
   
   Library: Visual Studio 2005 Release */

int __cdecl __fileno(FILE *_File)

{
  DWORD *pDVar1;
  
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_10012790();
    *pDVar1 = 0x16;
    FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  return _File->_file;
}


// 100178e9 FUN_100178e9

bool FUN_100178e9(void)

{
  return DAT_1005a0b4 == (DAT_10028738 | 1);
}


// 100178ff __wctomb_s_l

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
LAB_10017921:
    DVar1 = 0;
  }
  else {
    if (_SizeConverted != (int *)0x0) {
      *_SizeConverted = -1;
    }
    if (0x7fffffff < _SizeInBytes) {
      pDVar2 = FUN_10012790();
      *pDVar2 = 0x16;
      FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return 0x16;
    }
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
    if (*(int *)(local_14[0] + 0x14) == 0) {
      if ((ushort)_WCh < 0x100) {
        if (lpMultiByteStr != (char *)0x0) {
          if (_Size == 0) goto LAB_100179b1;
          *lpMultiByteStr = (char)_WCh;
        }
        if (_SizeConverted != (int *)0x0) {
          *_SizeConverted = 1;
        }
LAB_100179e8:
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        goto LAB_10017921;
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
LAB_100179b1:
          pDVar2 = FUN_10012790();
          *pDVar2 = 0x22;
          FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_100179e8;
      }
    }
    pDVar2 = FUN_10012790();
    *pDVar2 = 0x2a;
    pDVar2 = FUN_10012790();
    DVar1 = *pDVar2;
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return DVar1;
}


// 10017a5e _wctomb_s

/* Library Function - Single Match
    _wctomb_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _wctomb_s(int *_SizeConverted,char *_MbCh,rsize_t _SizeInBytes,wchar_t _WCh)

{
  errno_t eVar1;
  
  eVar1 = __wctomb_s_l(_SizeConverted,_MbCh,_SizeInBytes,_WCh,(_locale_t)0x0);
  return eVar1;
}


// 10017a80 __aulldvrm

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


// 10017b15 FUN_10017b15

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10017b15(void)

{
  _DAT_1005a4b4 = 0;
  return;
}


// 10017b1d ___libm_error_support

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
  if (DAT_1005a0b8 == 0) {
    pcVar1 = FUN_1001b6ff;
  }
  else {
    pcVar1 = (code *)FUN_10010ed5(DAT_1005a4b0);
  }
  if (param_4 < 0xa7) {
    if (param_4 == 0xa6) {
      local_2c = 3;
      local_28 = "exp10";
LAB_10017bcc:
      local_24 = *param_1;
      local_1c = *param_2;
      local_14 = *param_3;
      iVar2 = (*pcVar1)(&local_2c);
      if (iVar2 == 0) {
        pDVar3 = FUN_10012790();
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
            goto LAB_10017c3a;
          }
          if (param_4 == 8) {
            local_28 = "log10";
          }
          else {
            if (param_4 == 9) {
              local_28 = "log10";
              goto LAB_10017c3a;
            }
            if (param_4 != 0xe) {
              if (param_4 != 0xf) {
                if (param_4 != 0x18) {
                  return;
                }
                local_2c = 3;
                goto LAB_10017bc5;
              }
              local_28 = "exp";
              goto LAB_10017c01;
            }
            local_2c = 3;
            local_28 = "exp";
          }
        }
        goto LAB_10017bcc;
      }
      local_28 = "pow";
LAB_10017c01:
      local_24 = *param_1;
      local_1c = *param_2;
      local_2c = 4;
      local_14 = *param_3;
      (*pcVar1)(&local_2c);
    }
    else {
      if (param_4 != 0x1a) {
        if (param_4 != 0x1b) {
          if (param_4 == 0x1c) goto switchD_10017cf7_caseD_3ee;
          if (param_4 != 0x1d) {
            if (param_4 != 0x3a) {
              if (param_4 != 0x3d) {
                return;
              }
              goto switchD_10017cf7_caseD_3f1;
            }
            goto switchD_10017cf7_caseD_3f0;
          }
          local_28 = "pow";
          goto LAB_10017cb0;
        }
        local_2c = 2;
LAB_10017bc5:
        local_28 = "pow";
        goto LAB_10017bcc;
      }
      local_14 = 1.0;
    }
    goto LAB_10017d90;
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
switchD_10017cf7_caseD_3ee:
    local_28 = "pow";
    goto LAB_10017c3a;
  case 0x3ef:
    local_28 = "modf";
    break;
  case 0x3f0:
switchD_10017cf7_caseD_3f0:
    local_28 = "acos";
    goto LAB_10017c3a;
  case 0x3f1:
switchD_10017cf7_caseD_3f1:
    local_28 = "asin";
    goto LAB_10017c3a;
  case 0x3f2:
    local_28 = "sin";
    goto LAB_10017d5c;
  case 0x3f3:
    local_28 = "cos";
    goto LAB_10017d5c;
  case 0x3f4:
    local_28 = "tan";
LAB_10017d5c:
    local_14 = *param_1 *
               (double)CONCAT17(uStack_5,CONCAT16(uStack_6,CONCAT15(uStack_7,CONCAT14(uStack_8,
                                                  CONCAT13(uStack_9,CONCAT12(uStack_a,CONCAT11(
                                                  uStack_b,local_c)))))));
    *param_3 = local_14;
    local_24 = *param_1;
    local_1c = *param_2;
    goto LAB_10017d6d;
  default:
    goto switchD_10017cf7_default;
  }
LAB_10017cb0:
  *param_3 = *param_1;
LAB_10017c3a:
  local_24 = *param_1;
  local_1c = *param_2;
  local_14 = *param_3;
LAB_10017d6d:
  local_2c = 1;
  iVar2 = (*pcVar1)(&local_2c);
  if (iVar2 == 0) {
    pDVar3 = FUN_10012790();
    *pDVar3 = 0x21;
  }
LAB_10017d90:
  *param_3 = local_14;
switchD_10017cf7_default:
  return;
}


// 10018b5b __87except

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
  
  local_14 = DAT_10028738 ^ (uint)&local_94;
  local_94 = (uint)*param_3;
  iVar2 = *param_2;
  if (iVar2 == 1) {
LAB_10018bb5:
    uVar3 = 8;
LAB_10018bb7:
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
      goto LAB_10018bb7;
    }
    if (iVar2 == 3) {
      uVar3 = 0x11;
      goto LAB_10018bb7;
    }
    if (iVar2 == 4) {
      uVar3 = 0x12;
      goto LAB_10018bb7;
    }
    if (iVar2 == 5) goto LAB_10018bb5;
    if (iVar2 == 7) {
      *param_2 = 1;
    }
    else if (iVar2 == 8) {
      uVar3 = 0x10;
      goto LAB_10018bb7;
    }
  }
  __ctrlfp();
  if ((*param_2 != 8) && (DAT_10029090 == 0)) {
    iVar2 = FUN_1001b6ff();
    if (iVar2 != 0) goto LAB_10018c42;
  }
  __set_errno_from_matherr(*param_2);
LAB_10018c42:
  __security_check_cookie(local_14 ^ (uint)&local_94);
  return;
}


// 10018c57 __frnd

/* Library Function - Single Match
    __frnd
   
   Library: Visual Studio 2005 Release */

float10 __cdecl __frnd(double param_1)

{
  return (float10)ROUND(param_1);
}


// 10018c68 __fpclass

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


// 10018cfc __set_exp

/* Library Function - Single Match
    __set_exp
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release */

float10 __cdecl __set_exp(undefined8 param_1,short param_2)

{
  undefined8 local_c;
  
  local_c = (double)CONCAT26((param_2 + 0x3fe) * 0x10 | param_1._6_2_ & 0x800f,(int6)param_1);
  return (float10)local_c;
}


// 10018d26 __sptype

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


// 10018d81 FUN_10018d81

void __cdecl FUN_10018d81(uint param_1,uint param_2,int *param_3)

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


// 10018e30 x_ismbbtype_l

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
    if (uVar1 == 0) goto LAB_10018e72;
  }
  uVar1 = 1;
LAB_10018e72:
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 10018e81 __ismbblead

/* Library Function - Single Match
    __ismbblead
   
   Library: Visual Studio 2005 Release */

int __cdecl __ismbblead(uint _C)

{
  int iVar1;
  
  iVar1 = x_ismbbtype_l((localeinfo_struct *)0x0,_C,0,4);
  return iVar1;
}


// 10018e94 __crtGetStringTypeA_stat

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  local_c = in_ECX;
  if (DAT_1005a0bc == 0) {
    BVar2 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_1005a0bc = 2;
      }
      goto LAB_10018eed;
    }
    DAT_1005a0bc = 1;
  }
  else {
LAB_10018eed:
    if ((DAT_1005a0bc == 2) || (DAT_1005a0bc == 0)) {
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
        FUN_10009a00(uVar7);
      }
      goto LAB_1001903a;
    }
    if (DAT_1005a0bc != 1) goto LAB_1001903a;
  }
  local_c = (int *)0x0;
  if (param_5 == (ushort *)0x0) {
    param_5 = *(ushort **)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar((UINT)param_5,(uint)(param_7 != 0) * 8 + 1,(LPCSTR)param_2,
                                    (int)param_3,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_1001903a;
  lpWideCharStr = (LPCWSTR)0x0;
  if ((0 < (int)cchWideChar) && (cchWideChar < 0x7ffffff1)) {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar4 = (LPCWSTR)&stack0xffffffe8;
      lpWideCharStr = (LPCWSTR)&stack0xffffffe8;
      if (&stack0x00000000 != (undefined1 *)0x18) {
LAB_10018f7d:
        lpWideCharStr = pWVar4 + 4;
      }
    }
    else {
      pWVar4 = (LPCWSTR)FUN_100099a0(uVar1);
      lpWideCharStr = pWVar4;
      if (pWVar4 != (LPCWSTR)0x0) {
        pWVar4[0] = L'\xdddd';
        pWVar4[1] = L'\0';
        goto LAB_10018f7d;
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
LAB_1001903a:
  iVar5 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar5;
}


// 1001904c ___crtGetStringTypeA

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


// 1001908c ___free_lc_time

/* Library Function - Single Match
    ___free_lc_time
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lc_time(undefined4 *param_1)

{
  if (param_1 != (undefined4 *)0x0) {
    FUN_10009a00(param_1[1]);
    FUN_10009a00(param_1[2]);
    FUN_10009a00(param_1[3]);
    FUN_10009a00(param_1[4]);
    FUN_10009a00(param_1[5]);
    FUN_10009a00(param_1[6]);
    FUN_10009a00(*param_1);
    FUN_10009a00(param_1[8]);
    FUN_10009a00(param_1[9]);
    FUN_10009a00(param_1[10]);
    FUN_10009a00(param_1[0xb]);
    FUN_10009a00(param_1[0xc]);
    FUN_10009a00(param_1[0xd]);
    FUN_10009a00(param_1[7]);
    FUN_10009a00(param_1[0xe]);
    FUN_10009a00(param_1[0xf]);
    FUN_10009a00(param_1[0x10]);
    FUN_10009a00(param_1[0x11]);
    FUN_10009a00(param_1[0x12]);
    FUN_10009a00(param_1[0x13]);
    FUN_10009a00(param_1[0x14]);
    FUN_10009a00(param_1[0x15]);
    FUN_10009a00(param_1[0x16]);
    FUN_10009a00(param_1[0x17]);
    FUN_10009a00(param_1[0x18]);
    FUN_10009a00(param_1[0x19]);
    FUN_10009a00(param_1[0x1a]);
    FUN_10009a00(param_1[0x1b]);
    FUN_10009a00(param_1[0x1c]);
    FUN_10009a00(param_1[0x1d]);
    FUN_10009a00(param_1[0x1e]);
    FUN_10009a00(param_1[0x1f]);
    FUN_10009a00(param_1[0x20]);
    FUN_10009a00(param_1[0x21]);
    FUN_10009a00(param_1[0x22]);
    FUN_10009a00(param_1[0x23]);
    FUN_10009a00(param_1[0x24]);
    FUN_10009a00(param_1[0x25]);
    FUN_10009a00(param_1[0x26]);
    FUN_10009a00(param_1[0x27]);
    FUN_10009a00(param_1[0x28]);
    FUN_10009a00(param_1[0x29]);
    FUN_10009a00(param_1[0x2a]);
  }
  return;
}


// 1001921c ___free_lconv_num

/* Library Function - Single Match
    ___free_lconv_num
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_num(int *param_1)

{
  if (param_1 != (int *)0x0) {
    if ((undefined *)*param_1 != PTR_DAT_10028d50) {
      FUN_10009a00((undefined *)*param_1);
    }
    if ((undefined *)param_1[1] != PTR_DAT_10028d54) {
      FUN_10009a00((undefined *)param_1[1]);
    }
    if ((undefined *)param_1[2] != PTR_DAT_10028d58) {
      FUN_10009a00((undefined *)param_1[2]);
    }
  }
  return;
}


// 1001925c ___free_lconv_mon

/* Library Function - Single Match
    ___free_lconv_mon
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_mon(int param_1)

{
  if (param_1 != 0) {
    if (*(undefined **)(param_1 + 0xc) != PTR_DAT_10028d5c) {
      FUN_10009a00(*(undefined **)(param_1 + 0xc));
    }
    if (*(undefined **)(param_1 + 0x10) != PTR_DAT_10028d60) {
      FUN_10009a00(*(undefined **)(param_1 + 0x10));
    }
    if (*(undefined **)(param_1 + 0x14) != PTR_DAT_10028d64) {
      FUN_10009a00(*(undefined **)(param_1 + 0x14));
    }
    if (*(undefined **)(param_1 + 0x18) != PTR_DAT_10028d68) {
      FUN_10009a00(*(undefined **)(param_1 + 0x18));
    }
    if (*(undefined **)(param_1 + 0x1c) != PTR_DAT_10028d6c) {
      FUN_10009a00(*(undefined **)(param_1 + 0x1c));
    }
    if (*(undefined **)(param_1 + 0x20) != PTR_DAT_10028d70) {
      FUN_10009a00(*(undefined **)(param_1 + 0x20));
    }
    if (*(undefined **)(param_1 + 0x24) != PTR_DAT_10028d74) {
      FUN_10009a00(*(undefined **)(param_1 + 0x24));
    }
  }
  return;
}


// 100192f0 _strcspn

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


// 10019340 _strpbrk

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


// 10019380 ___ansicp

/* Library Function - Single Match
    ___ansicp
   
   Library: Visual Studio 2005 Release */

void __cdecl ___ansicp(LCID param_1)

{
  int iVar1;
  CHAR local_10 [6];
  undefined1 local_a;
  uint local_8;
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  local_a = 0;
  iVar1 = GetLocaleInfoA(param_1,0x1004,local_10,6);
  if (iVar1 != 0) {
    _atol(local_10);
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 100193c7 ___convertcp

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  cbMultiByte = *param_4;
  bVar2 = false;
  if (param_1 == param_2) goto LAB_10019567;
  BVar3 = GetCPInfo(param_1,&local_1c);
  if ((((BVar3 == 0) || (local_1c.MaxCharSize != 1)) ||
      (BVar3 = GetCPInfo(param_2,&local_1c), BVar3 == 0)) || (local_1c.MaxCharSize != 1)) {
    uVar7 = MultiByteToWideChar(param_1,1,param_3,cbMultiByte,(LPWSTR)0x0,0);
    bVar8 = uVar7 == 0;
    if (bVar8) goto LAB_10019567;
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
LAB_100194a7:
        local_20 = pWVar5 + 4;
      }
    }
    else {
      pWVar5 = (LPCWSTR)FUN_100099a0(uVar1);
      local_20 = pWVar5;
      if (pWVar5 != (LPCWSTR)0x0) {
        pWVar5[0] = L'\xdddd';
        pWVar5[1] = L'\0';
        goto LAB_100194a7;
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
           (lpMultiByteStr = (LPSTR)FUN_10009b00(1,uVar7), lpMultiByteStr != (LPSTR)0x0)) {
          uVar7 = WideCharToMultiByte(param_2,0,local_20,uVar7,lpMultiByteStr,uVar7,(LPCSTR)0x0,
                                      (LPBOOL)0x0);
          if (uVar7 == 0) {
            FUN_10009a00(lpMultiByteStr);
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
LAB_10019567:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10019579 ___report_gsfailure

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
  
  _DAT_1005a1e0 =
       (uint)(in_NT & 1) * 0x4000 | (uint)SBORROW4((int)&stack0xfffffffc,0x328) * 0x800 |
       (uint)(in_IF & 1) * 0x200 | (uint)(in_TF & 1) * 0x100 | (uint)((int)&local_32c < 0) * 0x80 |
       (uint)(&stack0x00000000 == (undefined1 *)0x32c) * 0x40 | (uint)(in_AF & 1) * 0x10 |
       (uint)((POPCOUNT((uint)&local_32c & 0xff) & 1U) == 0) * 4 |
       (uint)(&stack0xfffffffc < (undefined1 *)0x328) | (uint)(in_ID & 1) * 0x200000 |
       (uint)(in_VIP & 1) * 0x100000 | (uint)(in_VIF & 1) * 0x80000 | (uint)(in_AC & 1) * 0x40000;
  _DAT_1005a1e4 = &stack0x00000004;
  _DAT_1005a120 = 0x10001;
  _DAT_1005a0c8 = 0xc0000409;
  _DAT_1005a0cc = 1;
  local_32c = DAT_10028738;
  local_328 = DAT_1002873c;
  _DAT_1005a0d4 = unaff_retaddr;
  _DAT_1005a1ac = in_GS;
  _DAT_1005a1b0 = in_FS;
  _DAT_1005a1b4 = in_ES;
  _DAT_1005a1b8 = in_DS;
  _DAT_1005a1bc = unaff_EDI;
  _DAT_1005a1c0 = unaff_ESI;
  _DAT_1005a1c4 = unaff_EBX;
  _DAT_1005a1c8 = in_EDX;
  _DAT_1005a1cc = in_ECX;
  _DAT_1005a1d0 = in_EAX;
  _DAT_1005a1d4 = unaff_EBP;
  DAT_1005a1d8 = unaff_retaddr;
  _DAT_1005a1dc = in_CS;
  _DAT_1005a1e8 = in_SS;
  DAT_1005a118 = IsDebuggerPresent();
  FUN_10017b15();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  UnhandledExceptionFilter((_EXCEPTION_POINTERS *)&PTR_DAT_100237ac);
  if (DAT_1005a118 == 0) {
    FUN_10017b15();
  }
  uExitCode = 0xc0000409;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  return;
}


// 1001967d __mbtowc_l

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
          if (iVar2 != 0) goto LAB_100196c8;
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
        pDVar3 = FUN_10012790();
        *pDVar3 = 0x2a;
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        return -1;
      }
      if (_DstCh != (wchar_t *)0x0) {
        *_DstCh = (ushort)(byte)*_SrcCh;
      }
LAB_100196c8:
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


// 10019790 _mbtowc

/* Library Function - Single Match
    _mbtowc
   
   Library: Visual Studio 2005 Release */

int __cdecl _mbtowc(wchar_t *_DstCh,char *_SrcCh,size_t _SrcSizeInBytes)

{
  int iVar1;
  
  iVar1 = __mbtowc_l(_DstCh,_SrcCh,_SrcSizeInBytes,(_locale_t)0x0);
  return iVar1;
}


// 100197e4 __ld12tod

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
        goto LAB_10019ce3;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10028d98 - 1;
    iVar4 = (int)(DAT_10028d98 + ((int)DAT_10028d98 >> 0x1f & 0x1fU)) >> 5;
    uVar8 = DAT_10028d98 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar6) goto LAB_10019917;
          bVar14 = _Ifp < p_Var9;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_1001991e;
LAB_10019917:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_1001991e:
              iVar4 = iVar4 + -1;
              *puVar6 = (uint)_Ifp;
              if ((iVar4 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_1001992c;
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
LAB_1001992c:
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
    if (iVar12 < (int)(DAT_10028d94 - DAT_10028d98)) {
      local_24[0] = 0;
      local_24[1] = 0;
    }
    else {
      if (DAT_10028d94 < iVar12) {
        if (iVar12 < DAT_10028d90) {
          local_24[0] = local_24[0] & 0x7fffffff;
          iVar12 = iVar12 + DAT_10028da4;
          iVar2 = (int)(DAT_10028d9c + ((int)DAT_10028d9c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_10028d9c & 0x8000001f;
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
          iVar12 = (int)(DAT_10028d9c + ((int)DAT_10028d9c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_10028d9c & 0x8000001f;
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
          iVar12 = DAT_10028da4 + DAT_10028d90;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_10019ce3;
      }
      local_14 = DAT_10028d94 - local_14;
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
      iVar2 = DAT_10028d98 - 1;
      iVar12 = (int)(DAT_10028d98 + ((int)DAT_10028d98 >> 0x1f & 0x1fU)) >> 5;
      uVar10 = DAT_10028d98 & 0x8000001f;
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
          if (2 < iVar12) goto LAB_10019acf;
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
LAB_10019acf:
      *puVar7 = *puVar7 & -1 << ((byte)local_14 & 0x1f);
      iVar12 = local_10 + 1;
      if (iVar12 < 3) {
        puVar7 = local_24 + iVar12;
        for (iVar2 = 3 - iVar12; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar7 = 0;
          puVar7 = puVar7 + 1;
        }
      }
      uVar10 = DAT_10028d9c + 1;
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
LAB_10019ce3:
  local_24[0] = iVar12 << (0x1fU - (char)DAT_10028d9c & 0x1f) |
                -(uint)(local_24[3] != 0) & 0x80000000 | local_24[0];
  if (DAT_10028da0 == 0x40) {
    *(uint *)((int)&_D->x + 4) = local_24[0];
    *(uint *)&_D->x = local_24[1];
  }
  else if (DAT_10028da0 == 0x20) {
    *(uint *)&_D->x = local_24[0];
  }
  return IVar3;
}


// 10019d26 __ld12tof

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
        goto LAB_1001a225;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10028db0 - 1;
    iVar2 = (int)(DAT_10028db0 + ((int)DAT_10028db0 >> 0x1f & 0x1fU)) >> 5;
    uVar7 = DAT_10028db0 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar5) goto LAB_10019e59;
          bVar14 = _Ifp < p_Var8;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_10019e60;
LAB_10019e59:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_10019e60:
              iVar2 = iVar2 + -1;
              *puVar5 = (uint)_Ifp;
              if ((iVar2 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_10019e6e;
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
LAB_10019e6e:
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
    if (iVar11 < (int)(DAT_10028dac - DAT_10028db0)) {
      local_24 = 0;
      local_20 = 0.0;
    }
    else {
      if (DAT_10028dac < iVar11) {
        if (iVar11 < DAT_10028da8) {
          local_24 = local_24 & 0x7fffffff;
          iVar11 = iVar11 + DAT_10028dbc;
          iVar2 = (int)(DAT_10028db4 + ((int)DAT_10028db4 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10028db4 & 0x8000001f;
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
          iVar11 = (int)(DAT_10028db4 + ((int)DAT_10028db4 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10028db4 & 0x8000001f;
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
          iVar11 = DAT_10028dbc + DAT_10028da8;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_1001a225;
      }
      local_1c[2] = DAT_10028dac - local_1c[2];
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
      iVar2 = DAT_10028db0 - 1;
      iVar11 = (int)(DAT_10028db0 + ((int)DAT_10028db0 >> 0x1f & 0x1fU)) >> 5;
      uVar9 = DAT_10028db0 & 0x8000001f;
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
          if (2 < iVar11) goto LAB_1001a011;
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
LAB_1001a011:
      *puVar6 = *puVar6 & -1 << ((byte)local_1c[2] & 0x1f);
      iVar11 = local_1c[3] + 1;
      if (iVar11 < 3) {
        puVar6 = &local_24 + iVar11;
        for (iVar2 = 3 - iVar11; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar6 = 0;
          puVar6 = puVar6 + 1;
        }
      }
      uVar9 = DAT_10028db4 + 1;
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
LAB_1001a225:
  fVar12 = (float)(iVar11 << (0x1fU - (char)DAT_10028db4 & 0x1f) |
                   -(uint)(local_1c[1] != 0) & 0x80000000 | local_24);
  if (DAT_10028db8 == 0x40) {
    _F[1].f = fVar12;
    _F->f = local_20;
  }
  else if (DAT_10028db8 == 0x20) {
    _F->f = fVar12;
  }
  return IVar3;
}


// 1001a268 ___mtold12

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
  
  uVar7 = DAT_10028738 ^ (uint)&stack0xfffffffc;
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


// 1001a434 FUN_1001a434

/* WARNING: Removing unreachable block (ram,0x1001a925) */
/* WARNING: Removing unreachable block (ram,0x1001a92f) */
/* WARNING: Removing unreachable block (ram,0x1001a934) */

void __cdecl
FUN_1001a434(int param_1,uint param_2,ushort param_3,int param_4,byte param_5,short *param_6)

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
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
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
    goto LAB_1001acb2;
  }
  if ((short)uVar14 == 0x7fff) {
    *param_6 = 1;
    if (((param_2 == 0x80000000) && (param_1 == 0)) || ((param_2 & 0x40000000) != 0)) {
      if ((uVar8 == 0) || (param_2 != 0xc0000000)) {
        if ((param_2 != 0x80000000) || (param_1 != 0)) goto LAB_1001a563;
        pcVar25 = "1#INF";
      }
      else {
        if (param_1 != 0) {
LAB_1001a563:
          pcVar25 = "1#QNAN";
          goto LAB_1001a568;
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
LAB_1001a568:
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
    goto LAB_1001acb2;
  }
  local_50 = (short)(((uVar14 >> 8) + (param_2 >> 0x18) * 2) * 0x4d + -0x134312f4 + uVar14 * 0x4d10
                    >> 0x10);
  uVar16 = (uint)local_50;
  local_1a = (undefined1)uVar14;
  bStack_19 = (byte)(uVar14 >> 8);
  local_24._2_2_ = (ushort)param_1;
  local_24._0_2_ = 0;
  local_6c = &PTR_DAT_10028d60;
  uStack_20._0_2_ = uStack_12;
  uStack_20._2_2_ = (ushort)local_10;
  uStack_1c = local_10._2_2_;
  if (uVar16 != 0) {
    iVar20 = param_1;
    uVar14 = -uVar16;
    if (0 < (int)uVar16) {
      local_6c = (undefined **)&DAT_10028ec0;
      uVar14 = uVar16;
    }
    while (uVar14 != 0) {
      uStack_20._0_2_ = (ushort)((uint)iVar20 >> 0x10);
      local_24._2_2_ = (ushort)iVar20;
      local_6c = local_6c + 0x15;
      iVar3 = CONCAT22(local_c,local_10._2_2_);
      if ((uVar14 & 7) == 0) goto LAB_1001a889;
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
LAB_1001a7ad:
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
            if ((short)uVar24 < 1) goto LAB_1001a7ad;
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
          if (0x7ffe < uVar24) goto LAB_1001a86d;
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
LAB_1001a86d:
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
LAB_1001a889:
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
          goto LAB_1001aaf1;
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
LAB_1001a9ff:
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
          if ((short)uVar24 < 1) goto LAB_1001a9ff;
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
        goto LAB_1001aaf1;
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
LAB_1001aaf1:
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
        goto LAB_1001acb2;
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
LAB_1001acb2:
  local_24 = uVar16;
  local_10 = param_2;
  uStack_3e = iVar1;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001acfa __hw_cw

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


// 1001ad88 ___hw_cw_sse2

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


// 1001ae28 __control87

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
  if (DAT_1005b5fc != 0) {
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


// 1001b12c __global_unwind2

/* Library Function - Single Match
    __global_unwind2
   
   Library: Visual Studio */

void __cdecl __global_unwind2(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x1001b144,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 1001b191 __local_unwind2

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
  puStack_1c = &LAB_1001b14c;
  local_20 = *unaff_FS_OFFSET;
  *unaff_FS_OFFSET = &local_20;
  while( true ) {
    uVar1 = *(uint *)(param_1 + 0xc);
    if ((uVar1 == 0xffffffff) || ((param_2 != 0xffffffff && (uVar1 <= param_2)))) break;
    local_18 = *(undefined4 *)(*(int *)(param_1 + 8) + uVar1 * 0xc);
    *(undefined4 *)(param_1 + 0xc) = local_18;
    if (*(int *)(*(int *)(param_1 + 8) + 4 + uVar1 * 0xc) == 0) {
      __NLG_Notify(0x101);
      FUN_1001b260();
    }
  }
  *unaff_FS_OFFSET = local_20;
  return;
}


// 1001b241 __NLG_Notify

/* Library Function - Single Match
    __NLG_Notify
   
   Libraries: Visual Studio 2017 Debug, Visual Studio 2017 Release, Visual Studio 2019 Debug, Visual
   Studio 2019 Release */

void __NLG_Notify(ulong param_1)

{
  undefined4 in_EAX;
  undefined4 unaff_EBP;
  
  DAT_10029084 = param_1;
  DAT_10029080 = in_EAX;
  DAT_10029088 = unaff_EBP;
  return;
}


// 1001b260 FUN_1001b260

void FUN_1001b260(void)

{
  code *in_EAX;
  
  (*in_EAX)();
  return;
}


// 1001b263 FUN_1001b263

undefined4 __cdecl FUN_1001b263(uint param_1)

{
  int *piVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD nStdHandle;
  
  if ((-1 < (int)param_1) && (param_1 < DAT_1005b4c4)) {
    iVar3 = (param_1 & 0x1f) * 0x38;
    piVar1 = (int *)((&DAT_1005b4e0)[(int)param_1 >> 5] + iVar3);
    if (((*(byte *)(piVar1 + 1) & 1) != 0) && (*piVar1 != -1)) {
      if (DAT_10059a0c == 1) {
        if (param_1 == 0) {
          nStdHandle = 0xfffffff6;
        }
        else if (param_1 == 1) {
          nStdHandle = 0xfffffff5;
        }
        else {
          if (param_1 != 2) goto LAB_1001b2c1;
          nStdHandle = 0xfffffff4;
        }
        SetStdHandle(nStdHandle,(HANDLE)0x0);
      }
LAB_1001b2c1:
      *(undefined4 *)(iVar3 + (&DAT_1005b4e0)[(int)param_1 >> 5]) = 0xffffffff;
      return 0;
    }
  }
  pDVar2 = FUN_10012790();
  *pDVar2 = 9;
  pDVar2 = FUN_100127a3();
  *pDVar2 = 0;
  return 0xffffffff;
}


// 1001b2e4 __get_osfhandle

/* Library Function - Single Match
    __get_osfhandle
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __get_osfhandle(int _FileHandle)

{
  DWORD *pDVar1;
  intptr_t *piVar2;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_100127a3();
    *pDVar1 = 0;
    pDVar1 = FUN_10012790();
    *pDVar1 = 9;
    return -1;
  }
  if (((-1 < _FileHandle) && ((uint)_FileHandle < DAT_1005b4c4)) &&
     (piVar2 = (intptr_t *)((_FileHandle & 0x1fU) * 0x38 + (&DAT_1005b4e0)[_FileHandle >> 5]),
     (*(byte *)(piVar2 + 1) & 1) != 0)) {
    return *piVar2;
  }
  pDVar1 = FUN_100127a3();
  *pDVar1 = 0;
  pDVar1 = FUN_10012790();
  *pDVar1 = 9;
  FUN_10012731((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1001b355 ___lock_fhandle

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
  
  iVar2 = (_Filehandle & 0x1fU) * 0x38 + (&DAT_1005b4e0)[_Filehandle >> 5];
  local_20 = 1;
  if (*(int *)(iVar2 + 8) == 0) {
    __lock(10);
    if (*(int *)(iVar2 + 8) == 0) {
      iVar1 = ___crtInitCritSecAndSpinCount(iVar2 + 0xc,4000);
      local_20 = (uint)(iVar1 != 0);
      *(int *)(iVar2 + 8) = *(int *)(iVar2 + 8) + 1;
    }
    FUN_1001b3ec();
  }
  if (local_20 != 0) {
    EnterCriticalSection
              ((LPCRITICAL_SECTION)
               ((&DAT_1005b4e0)[_Filehandle >> 5] + 0xc + (_Filehandle & 0x1fU) * 0x38));
  }
  return local_20;
}


// 1001b3ec FUN_1001b3ec

void FUN_1001b3ec(void)

{
  FUN_10015425(10);
  return;
}


// 1001b3f5 FUN_1001b3f5

void __cdecl FUN_1001b3f5(uint param_1)

{
  LeaveCriticalSection
            ((LPCRITICAL_SECTION)
             ((&DAT_1005b4e0)[(int)param_1 >> 5] + 0xc + (param_1 & 0x1f) * 0x38));
  return;
}


// 1001b417 __putwch_nolock

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
  
  local_8 = DAT_10028738 ^ (uint)&stack0xfffffffc;
  if (DAT_1002908c != 0) {
    if (DAT_10029254 == (HANDLE)0xfffffffe) {
      ___initconout();
    }
    if (DAT_10029254 == (HANDLE)0xffffffff) goto LAB_1001b4c0;
    BVar2 = WriteConsoleW(DAT_10029254,&_WCh,1,&local_14,(LPVOID)0x0);
    if (BVar2 != 0) {
      DAT_1002908c = 1;
      goto LAB_1001b4c0;
    }
    if ((DAT_1002908c != 2) || (DVar3 = GetLastError(), DVar3 != 0x78)) goto LAB_1001b4c0;
    DAT_1002908c = 0;
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
  if (DAT_10029254 != (HANDLE)0xffffffff) {
    WriteConsoleA(DAT_10029254,local_10,DVar3,&local_14,(LPVOID)0x0);
  }
LAB_1001b4c0:
  wVar1 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return wVar1;
}


// 1001b6ff FUN_1001b6ff

undefined4 FUN_1001b6ff(void)

{
  return 0;
}


// 1001b702 __raise_exc_ex

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
        goto LAB_1001b85f;
      }
      uVar3 = *param_1 & 0xfffffffe | 2;
    }
    *param_1 = uVar3;
  }
LAB_1001b85f:
  uVar3 = *puVar1 & 0x300;
  if (uVar3 == 0) {
    uVar3 = *param_1 & 0xffffffeb | 8;
LAB_1001b895:
    *param_1 = uVar3;
  }
  else {
    if (uVar3 == 0x200) {
      uVar3 = *param_1 & 0xffffffe7 | 4;
      goto LAB_1001b895;
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
        goto LAB_1001b99a;
      }
      uVar3 = *puVar1 & 0xfffffbff | 0x800;
    }
    *puVar1 = uVar3;
  }
LAB_1001b99a:
  uVar3 = *param_1 >> 2 & 7;
  if (uVar3 == 0) {
    uVar3 = *puVar1 & 0xfffff3ff | 0x300;
  }
  else {
    if (uVar3 != 1) {
      if (uVar3 == 2) {
        *puVar1 = *puVar1 & 0xfffff3ff;
      }
      goto LAB_1001b9c6;
    }
    uVar3 = *puVar1 & 0xfffff3ff | 0x200;
  }
  *puVar1 = uVar3;
LAB_1001b9c6:
  if (param_7 == 0) {
    *(undefined8 *)puVar2 = *(undefined8 *)(param_1 + 0x14);
  }
  else {
    *puVar2 = param_1[0x14];
  }
  return;
}


// 1001b9dc __raise_exc

/* Library Function - Single Match
    __raise_exc
   
   Library: Visual Studio 2005 Release */

void __cdecl
__raise_exc(uint *param_1,uint *param_2,uint param_3,int param_4,uint *param_5,uint *param_6)

{
  __raise_exc_ex(param_1,param_2,param_3,param_4,param_5,param_6,0);
  return;
}


// 1001b9fd __handle_exc

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
    FUN_1001bc44();
    uVar6 = param_1 & 0x17;
    goto LAB_1001bbbc;
  }
  if (((param_1 & 4) != 0) && ((param_3 & 4) != 0)) {
    FUN_1001bc44();
    uVar6 = param_1 & 0x1b;
    goto LAB_1001bbbc;
  }
  if (((param_1 & 1) == 0) || ((param_3 & 8) == 0)) {
    if (((param_1 & 2) != 0) && ((param_3 & 0x10) != 0)) {
      bVar7 = (param_1 & 0x10) != 0;
      if (*param_2 != 0.0) {
        fVar8 = (float10)FUN_10018d81(SUB84(*param_2,0),(uint)((ulonglong)*param_2 >> 0x20),&local_c
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
        FUN_1001bc44();
      }
      uVar6 = local_8 & 0xfffffffd;
      local_8 = uVar6;
    }
    goto LAB_1001bbbc;
  }
  FUN_1001bc44();
  uVar6 = param_3 & 0xc00;
  dVar2 = _DAT_10028c60;
  dVar5 = _DAT_10028c60;
  if (uVar6 == 0) {
    dVar1 = *param_2;
joined_r0x1001ba9f:
    if (dVar1 <= 0.0) {
      dVar2 = -dVar5;
    }
    *param_2 = dVar2;
  }
  else {
    if (uVar6 == 0x400) {
      dVar1 = *param_2;
      dVar2 = _DAT_10028c70;
      goto joined_r0x1001ba9f;
    }
    dVar5 = _DAT_10028c70;
    if (uVar6 == 0x800) {
      dVar1 = *param_2;
      goto joined_r0x1001ba9f;
    }
    if (uVar6 == 0xc00) {
      dVar1 = *param_2;
      dVar2 = _DAT_10028c70;
      goto joined_r0x1001ba9f;
    }
  }
  uVar6 = param_1 & 0x1e;
LAB_1001bbbc:
  if (((param_1 & 0x10) != 0) && ((param_3 & 0x20) != 0)) {
    FUN_1001bc44();
    uVar6 = uVar6 & 0xffffffef;
  }
  return uVar6 == 0;
}


// 1001bbde __set_errno_from_matherr

/* Library Function - Single Match
    __set_errno_from_matherr
   
   Library: Visual Studio 2005 Release */

void __cdecl __set_errno_from_matherr(int param_1)

{
  DWORD *pDVar1;
  
  if (param_1 == 1) {
    pDVar1 = FUN_10012790();
    *pDVar1 = 0x21;
  }
  else if ((1 < param_1) && (param_1 < 4)) {
    pDVar1 = FUN_10012790();
    *pDVar1 = 0x22;
    return;
  }
  return;
}


// 1001bc06 __statfp

/* Library Function - Single Match
    __statfp
   
   Library: Visual Studio 2005 Release */

int __statfp(void)

{
  short in_FPUStatusWord;
  
  return (int)in_FPUStatusWord;
}


// 1001bc11 __clrfp

/* Library Function - Single Match
    __clrfp
   
   Library: Visual Studio 2005 Release */

int __clrfp(void)

{
  short in_FPUStatusWord;
  
  return (int)in_FPUStatusWord;
}


// 1001bc1d __ctrlfp

/* Library Function - Single Match
    __ctrlfp
   
   Library: Visual Studio 2005 Release */

int __ctrlfp(void)

{
  short in_FPUControlWord;
  
  return (int)in_FPUControlWord;
}


// 1001bc44 FUN_1001bc44

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_1001bc44(void)

{
  return;
}


// 1001bc9a ___set_fpsr_sse2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___set_fpsr_sse2
   
   Library: Visual Studio 2005 Release */

void __cdecl ___set_fpsr_sse2(uint param_1)

{
  if (DAT_1005b5fc != 0) {
    if (((param_1 & 0x40) == 0) || (DAT_10029198 == 0)) {
      MXCSR = param_1 & 0xffffffbf;
    }
    else {
      MXCSR = param_1;
    }
  }
  return;
}


// 1001bd0c ___initconout

/* Library Function - Single Match
    ___initconout
   
   Library: Visual Studio 2005 Release */

void __cdecl ___initconout(void)

{
  DAT_10029254 = CreateFileA("CONOUT$",0x40000000,3,(LPSECURITY_ATTRIBUTES)0x0,3,0,(HANDLE)0x0);
  return;
}


// 1001bf30 ___ascii_strnicmp

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
      if (bVar2 != (byte)uVar3) goto LAB_1001bf81;
      _MaxCount = _MaxCount - 1;
    } while (_MaxCount != 0);
    _MaxCount = 0;
    bVar2 = (byte)(uVar3 >> 8);
    bVar5 = bVar2 < (byte)uVar3;
    if (bVar2 != (byte)uVar3) {
LAB_1001bf81:
      _MaxCount = 0xffffffff;
      if (!bVar5) {
        _MaxCount = 1;
      }
    }
  }
  return _MaxCount;
}


// 1001bf91 FUN_1001bf91

undefined4 __cdecl FUN_1001bf91(uint param_1)

{
  intptr_t iVar1;
  intptr_t iVar2;
  HANDLE hObject;
  BOOL BVar3;
  DWORD DVar4;
  undefined4 uVar5;
  
  iVar1 = __get_osfhandle(param_1);
  if (iVar1 != -1) {
    if (((param_1 == 1) && ((*(byte *)(DAT_1005b4e0 + 0x74) & 1) != 0)) ||
       ((param_1 == 2 && ((*(byte *)(DAT_1005b4e0 + 0x3c) & 1) != 0)))) {
      iVar1 = __get_osfhandle(2);
      iVar2 = __get_osfhandle(1);
      if (iVar2 == iVar1) goto LAB_1001bff0;
    }
    hObject = (HANDLE)__get_osfhandle(param_1);
    BVar3 = CloseHandle(hObject);
    if (BVar3 == 0) {
      DVar4 = GetLastError();
      goto LAB_1001bff2;
    }
  }
LAB_1001bff0:
  DVar4 = 0;
LAB_1001bff2:
  FUN_1001b263(param_1);
  *(undefined1 *)((&DAT_1005b4e0)[(int)param_1 >> 5] + 4 + (param_1 & 0x1f) * 0x38) = 0;
  if (DVar4 == 0) {
    uVar5 = 0;
  }
  else {
    FUN_100127b6(DVar4);
    uVar5 = 0xffffffff;
  }
  return uVar5;
}


// 1001c130 FUN_1001c130

void FUN_1001c130(void)

{
  FUN_10006210((undefined4 *)&DAT_100292d8);
  _atexit((_func_4879 *)&LAB_1001c3e0);
  return;
}


