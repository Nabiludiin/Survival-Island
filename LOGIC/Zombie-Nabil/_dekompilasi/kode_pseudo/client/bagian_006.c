// 103870c9 _strupr_s_l_stat

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* Library Function - Single Match
    int __cdecl _strupr_s_l_stat(char *,unsigned int,struct localeinfo_struct *)
   
   Library: Visual Studio 2005 Release */

int __cdecl _strupr_s_l_stat(char *param_1,uint param_2,localeinfo_struct *param_3)

{
  char cVar1;
  LPCWSTR _LocaleName;
  uint uVar2;
  undefined4 *puVar3;
  char *pcVar4;
  char *pcVar5;
  int iVar6;
  char *unaff_EDI;
  undefined4 uVar7;
  char *local_c;
  
  uVar2 = DAT_10538340 ^ (uint)&stack0xfffffffc;
  if (unaff_EDI == (char *)0x0) {
LAB_103870e1:
    puVar3 = (undefined4 *)FUN_10369c9f();
    uVar7 = 0x16;
  }
  else {
    pcVar4 = (char *)_strnlen(unaff_EDI,(size_t)param_1);
    if (param_1 <= pcVar4) {
      *unaff_EDI = '\0';
      goto LAB_103870e1;
    }
    _LocaleName = *(LPCWSTR *)(*(int *)param_2 + 0x14);
    if (_LocaleName == (LPCWSTR)0x0) {
      cVar1 = *unaff_EDI;
      while (cVar1 != '\0') {
        cVar1 = *unaff_EDI;
        if (('`' < cVar1) && (cVar1 < '{')) {
          *unaff_EDI = cVar1 + -0x20;
        }
        unaff_EDI = unaff_EDI + 1;
        cVar1 = *unaff_EDI;
      }
      goto LAB_10387234;
    }
    pcVar4 = (char *)___crtLCMapStringA((_locale_t)param_2,_LocaleName,0x200,unaff_EDI,-1,(LPSTR)0x0
                                        ,0,*(int *)(*(int *)param_2 + 4),1);
    if (pcVar4 == (char *)0x0) {
      puVar3 = (undefined4 *)FUN_10369c9f();
      *puVar3 = 0x2a;
LAB_1038716b:
      FUN_10369c9f();
      goto LAB_10387234;
    }
    if (pcVar4 <= param_1) {
      if (((int)pcVar4 < 1) || (0xffffffe0U / (uint)pcVar4 == 0)) {
        local_c = (char *)0x0;
      }
      else if (pcVar4 + 8 < (char *)0x401) {
        pcVar5 = &stack0xffffffe8;
        local_c = &stack0xffffffe8;
        if (&stack0x00000000 != (undefined1 *)0x18) {
LAB_103871c8:
          local_c = pcVar5 + 8;
        }
      }
      else {
        pcVar5 = (char *)FUN_10001270(pcVar4 + 8);
        local_c = pcVar5;
        if (pcVar5 != (char *)0x0) {
          pcVar5[0] = -0x23;
          pcVar5[1] = -0x23;
          pcVar5[2] = '\0';
          pcVar5[3] = '\0';
          goto LAB_103871c8;
        }
      }
      if (local_c != (char *)0x0) {
        iVar6 = ___crtLCMapStringA((_locale_t)param_2,*(LPCWSTR *)(*(int *)param_2 + 0x14),0x200,
                                   unaff_EDI,-1,local_c,(int)pcVar4,*(int *)(*(int *)param_2 + 4),1)
        ;
        if (iVar6 == 0) {
          puVar3 = (undefined4 *)FUN_10369c9f();
          *puVar3 = 0x2a;
        }
        else {
          _strcpy_s(unaff_EDI,(rsize_t)param_1,local_c);
        }
        __freea(local_c);
        goto LAB_10387234;
      }
      puVar3 = (undefined4 *)FUN_10369c9f();
      *puVar3 = 0xc;
      goto LAB_1038716b;
    }
    *unaff_EDI = '\0';
    puVar3 = (undefined4 *)FUN_10369c9f();
    uVar7 = 0x22;
  }
  *puVar3 = uVar7;
  FUN_10369c40(0,0,0,0,0);
LAB_10387234:
  iVar6 = __security_check_cookie(uVar2 ^ (uint)&stack0xfffffffc);
  return iVar6;
}


// 10387245 __strupr_s_l

/* Library Function - Single Match
    __strupr_s_l
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __strupr_s_l(char *_Str,size_t _Size,_locale_t _Locale)

{
  int iVar1;
  localeinfo_struct *unaff_EDI;
  _LocaleUpdate local_14 [8];
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate(local_14,_Locale);
  iVar1 = _strupr_s_l_stat((char *)_Size,(uint)local_14,unaff_EDI);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return iVar1;
}


// 10387278 __strupr

/* Library Function - Single Match
    __strupr
   
   Library: Visual Studio 2005 Release */

char * __cdecl __strupr(char *_String)

{
  char cVar1;
  char *pcVar2;
  undefined4 *puVar3;
  
  if (DAT_105fef38 == 0) {
    pcVar2 = _String;
    if (_String == (char *)0x0) {
      puVar3 = (undefined4 *)FUN_10369c9f();
      *puVar3 = 0x16;
      FUN_10369c40(0,0,0,0,0);
      return (char *)0x0;
    }
    if (*_String != '\0') {
      do {
        cVar1 = *_String;
        if (('`' < cVar1) && (cVar1 < '{')) {
          *_String = cVar1 + -0x20;
        }
        _String = _String + 1;
      } while (*_String != '\0');
      return pcVar2;
    }
  }
  else {
    __strupr_s_l(_String,0xffffffff,(_locale_t)0x0);
  }
  return _String;
}


// 103872da _strlwr_s_l_stat

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* Library Function - Single Match
    int __cdecl _strlwr_s_l_stat(char *,unsigned int,struct localeinfo_struct *)
   
   Library: Visual Studio 2005 Release */

int __cdecl _strlwr_s_l_stat(char *param_1,uint param_2,localeinfo_struct *param_3)

{
  char cVar1;
  LPCWSTR _LocaleName;
  uint uVar2;
  undefined4 *puVar3;
  size_t sVar4;
  uint _CchDest;
  char *pcVar5;
  int iVar6;
  undefined4 uVar7;
  char *local_c;
  
  uVar2 = DAT_10538340 ^ (uint)&stack0xfffffffc;
  if (param_1 == (char *)0x0) {
LAB_103872f6:
    puVar3 = (undefined4 *)FUN_10369c9f();
    uVar7 = 0x16;
  }
  else {
    sVar4 = _strnlen(param_1,param_2);
    if (param_2 <= sVar4) {
      *param_1 = '\0';
      goto LAB_103872f6;
    }
    _LocaleName = param_3->locinfo->lc_category[0].wlocale;
    if (_LocaleName == (LPCWSTR)0x0) {
      cVar1 = *param_1;
      while (cVar1 != '\0') {
        cVar1 = *param_1;
        if (('@' < cVar1) && (cVar1 < '[')) {
          *param_1 = cVar1 + ' ';
        }
        param_1 = param_1 + 1;
        cVar1 = *param_1;
      }
      goto LAB_10387449;
    }
    _CchDest = ___crtLCMapStringA(param_3,_LocaleName,0x100,param_1,-1,(LPSTR)0x0,0,
                                  param_3->locinfo->lc_codepage,1);
    if (_CchDest == 0) {
      puVar3 = (undefined4 *)FUN_10369c9f();
      *puVar3 = 0x2a;
LAB_10387380:
      FUN_10369c9f();
      goto LAB_10387449;
    }
    if (_CchDest <= param_2) {
      if (((int)_CchDest < 1) || (0xffffffe0 / _CchDest == 0)) {
        local_c = (char *)0x0;
      }
      else if (_CchDest + 8 < 0x401) {
        pcVar5 = &stack0xffffffe4;
        local_c = &stack0xffffffe4;
        if (&stack0x00000000 != (undefined1 *)0x1c) {
LAB_103873dd:
          local_c = pcVar5 + 8;
        }
      }
      else {
        pcVar5 = (char *)FUN_10001270(_CchDest + 8);
        local_c = pcVar5;
        if (pcVar5 != (char *)0x0) {
          pcVar5[0] = -0x23;
          pcVar5[1] = -0x23;
          pcVar5[2] = '\0';
          pcVar5[3] = '\0';
          goto LAB_103873dd;
        }
      }
      if (local_c != (char *)0x0) {
        iVar6 = ___crtLCMapStringA(param_3,param_3->locinfo->lc_category[0].wlocale,0x100,param_1,-1
                                   ,local_c,_CchDest,param_3->locinfo->lc_codepage,1);
        if (iVar6 == 0) {
          puVar3 = (undefined4 *)FUN_10369c9f();
          *puVar3 = 0x2a;
        }
        else {
          _strcpy_s(param_1,param_2,local_c);
        }
        __freea(local_c);
        goto LAB_10387449;
      }
      puVar3 = (undefined4 *)FUN_10369c9f();
      *puVar3 = 0xc;
      goto LAB_10387380;
    }
    *param_1 = '\0';
    puVar3 = (undefined4 *)FUN_10369c9f();
    uVar7 = 0x22;
  }
  *puVar3 = uVar7;
  FUN_10369c40(0,0,0,0,0);
LAB_10387449:
  iVar6 = __security_check_cookie(uVar2 ^ (uint)&stack0xfffffffc);
  return iVar6;
}


// 1038745b __strlwr_s_l

/* Library Function - Single Match
    __strlwr_s_l
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __strlwr_s_l(char *_Str,size_t _Size,_locale_t _Locale)

{
  int iVar1;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  iVar1 = _strlwr_s_l_stat(_Str,_Size,&local_14);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return iVar1;
}


// 1038748d __strlwr

/* Library Function - Single Match
    __strlwr
   
   Library: Visual Studio 2005 Release */

char * __cdecl __strlwr(char *_String)

{
  char cVar1;
  char *pcVar2;
  undefined4 *puVar3;
  
  if (DAT_105fef38 == 0) {
    pcVar2 = _String;
    if (_String == (char *)0x0) {
      puVar3 = (undefined4 *)FUN_10369c9f();
      *puVar3 = 0x16;
      FUN_10369c40(0,0,0,0,0);
      return (char *)0x0;
    }
    if (*_String != '\0') {
      do {
        cVar1 = *_String;
        if (('@' < cVar1) && (cVar1 < '[')) {
          *_String = cVar1 + ' ';
        }
        _String = _String + 1;
      } while (*_String != '\0');
      return pcVar2;
    }
  }
  else {
    __strlwr_s_l(_String,0xffffffff,(_locale_t)0x0);
  }
  return _String;
}


// 10387680 FUN_10387680

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10387680(void)

{
  _DAT_10539b14 = 0;
  return;
}


// 1038e1f0 FUN_1038e1f0

void FUN_1038e1f0(void)

{
  FUN_100ea720(0,0,0x42800000,0xc1800000,0xc1800000,0,0x41800000,0x41800000,0x42900000,0xc1800000,
               0xc1800000,0,0x41800000,0x41800000,0x42100000,0,0,0x41e00000,0xc1200000,0xc1200000,
               0xc1200000,0x41200000,0x41200000,0x41200000,0,0,0x41600000);
  return;
}


// 103926e0 FUN_103926e0

void FUN_103926e0(void)

{
  int *piVar1;
  
  piVar1 = (int *)FUN_10117d90();
  (**(code **)(*piVar1 + 8))("ParticleSphereProxy",FUN_10128940);
  return;
}


// 103a0c50 FUN_103a0c50

void FUN_103a0c50(void)

{
  FUN_100ea720(0,0,0x42780000,0xc1800000,0xc1800000,0,0x41800000,0x41800000,0x428e0000,0xc1800000,
               0xc1800000,0,0x41800000,0x41800000,0x425c0000,0,0,0x42300000,0xc1200000,0xc1200000,
               0xc1200000,0x41200000,0x41200000,0x41200000,0,0,0x41600000);
  return;
}


// 103a7a20 FUN_103a7a20

void FUN_103a7a20(void)

{
  _SYSTEM_INFO local_24;
  
  GetSystemInfo(&local_24);
  DAT_105e2678 = local_24.dwNumberOfProcessors;
  return;
}


// 103a82d0 FUN_103a82d0

void FUN_103a82d0(void)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  int *piVar4;
  
  iVar2 = 1;
  piVar3 = (int *)&DAT_1053ea5c;
  do {
    piVar4 = piVar3 + -7;
    piVar3[-4] = 0;
    if (-1 < piVar3[-5]) {
      if (*piVar4 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar4);
        *piVar4 = 0;
      }
      piVar3[-6] = 0;
    }
    iVar1 = *piVar4;
    piVar3[-3] = iVar1;
    if (-1 < piVar3[-5]) {
      if (iVar1 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
        *piVar4 = 0;
      }
      piVar3[-6] = 0;
    }
    iVar2 = iVar2 + -1;
    piVar3 = piVar4;
  } while (-1 < iVar2);
  return;
}


// 103a8a80 FUN_103a8a80

void FUN_103a8a80(void)

{
  int iVar1;
  int iVar2;
  undefined *puVar3;
  undefined4 *puVar4;
  
  iVar2 = 1;
  puVar3 = &DAT_10546dd0;
  do {
    *(undefined4 *)(puVar3 + -200) = 0;
    if (-1 < *(int *)(puVar3 + -0xcc)) {
      if (*(int *)(puVar3 + -0xd4) != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(puVar3 + -0xd4));
        *(undefined4 *)(puVar3 + -0xd4) = 0;
      }
      *(undefined4 *)(puVar3 + -0xd0) = 0;
    }
    iVar1 = *(int *)(puVar3 + -0xd4);
    *(int *)(puVar3 + -0xc4) = iVar1;
    if (-1 < *(int *)(puVar3 + -0xcc)) {
      if (iVar1 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
        *(undefined4 *)(puVar3 + -0xd4) = 0;
      }
      *(undefined4 *)(puVar3 + -0xd0) = 0;
    }
    *(undefined4 *)(puVar3 + -0xdc) = 0;
    if (-1 < *(int *)(puVar3 + -0xe0)) {
      if (*(int *)(puVar3 + -0xe8) != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(puVar3 + -0xe8));
        *(undefined4 *)(puVar3 + -0xe8) = 0;
      }
      *(undefined4 *)(puVar3 + -0xe4) = 0;
    }
    iVar1 = *(int *)(puVar3 + -0xe8);
    *(int *)(puVar3 + -0xd8) = iVar1;
    if (-1 < *(int *)(puVar3 + -0xe0)) {
      if (iVar1 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
        *(undefined4 *)(puVar3 + -0xe8) = 0;
      }
      *(undefined4 *)(puVar3 + -0xe4) = 0;
    }
    if (puVar3 + -0x100 == (undefined *)0x0) {
      puVar4 = (undefined4 *)0x0;
    }
    else {
      puVar4 = (undefined4 *)(puVar3 + -0xf4);
    }
    *puVar4 = &PTR_FUN_103b6774;
    puVar4[1] = 0xd;
    if (*(char *)(puVar4 + 2) != '\0') {
      if (DAT_1054ca58 != (int *)0x0) {
        (**(code **)(*DAT_1054ca58 + 0x14))(puVar4);
      }
      *(undefined1 *)(puVar4 + 2) = 0;
    }
    *puVar4 = &PTR_LAB_103b6718;
    FUN_1010d390();
    iVar2 = iVar2 + -1;
    puVar3 = puVar3 + -0x100;
  } while (-1 < iVar2);
  return;
}


// 103ac050 FUN_103ac050

void FUN_103ac050(void)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  int *piVar4;
  
  iVar2 = 1;
  piVar3 = (int *)&DAT_10595a88;
  do {
    piVar4 = piVar3 + -0x14;
    piVar3[-0x1c] = (int)&PTR_LAB_1040c2c0;
    FUN_1017de60();
    piVar3[-0x13] = 0;
    if (-1 < *piVar4) {
      if (piVar3[-0x16] != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar3[-0x16]);
        piVar3[-0x16] = 0;
      }
      piVar3[-0x15] = 0;
    }
    iVar1 = piVar3[-0x16];
    piVar3[-0x12] = iVar1;
    if (-1 < *piVar4) {
      if (iVar1 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
        piVar3[-0x16] = 0;
      }
      piVar3[-0x15] = 0;
    }
    piVar3[-0x18] = 0;
    if (-1 < piVar3[-0x19]) {
      if (piVar3[-0x1b] != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar3[-0x1b]);
        piVar3[-0x1b] = 0;
      }
      piVar3[-0x1a] = 0;
    }
    iVar1 = piVar3[-0x1b];
    piVar3[-0x17] = iVar1;
    if (-1 < piVar3[-0x19]) {
      if (iVar1 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar1);
        piVar3[-0x1b] = 0;
      }
      piVar3[-0x1a] = 0;
    }
    iVar2 = iVar2 + -1;
    piVar3 = piVar4;
  } while (-1 < iVar2);
  return;
}


// 103af3a0 FUN_103af3a0

void FUN_103af3a0(void)

{
  int iVar1;
  char *pcVar2;
  char *pcVar3;
  
  iVar1 = 1;
  pcVar3 = &DAT_105b50e0;
  do {
    pcVar2 = pcVar3 + -0x128;
    *(undefined ***)(pcVar3 + -0x230) = &PTR_FUN_10436acc;
    *(undefined ***)(pcVar3 + -0x22c) = &PTR_LAB_10436ab8;
    FUN_102134f0();
    if (*pcVar2 != '\0') {
      *pcVar2 = '\0';
    }
    FUN_102d7cd0();
    FUN_101d5b30();
    iVar1 = iVar1 + -1;
    pcVar3 = pcVar2;
  } while (-1 < iVar1);
  return;
}


// 103b0c30 FUN_103b0c30

void FUN_103b0c30(void)

{
  FUN_102e65d0();
  return;
}


// 103b0c40 FUN_103b0c40

void FUN_103b0c40(void)

{
  FUN_102e1e60();
  return;
}


// 103b0cd0 FUN_103b0cd0

void FUN_103b0cd0(void)

{
  FUN_102e1e60();
  return;
}


// 103b0ce0 FUN_103b0ce0

void FUN_103b0ce0(void)

{
  FUN_102e1e60();
  return;
}


