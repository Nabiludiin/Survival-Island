// 101166b0 FUN_101166b0

undefined4 * FUN_101166b0(undefined4 *param_1,undefined4 param_2,undefined4 param_3,int param_4)

{
  int iVar1;
  int iVar2;
  undefined4 *puVar3;
  
  _memset(param_1 + 0x26,0xff,0x80);
  param_1[5] = 0;
  param_1[2] = 0;
  param_1[1] = 0;
  param_1[3] = 0;
  *param_1 = 0;
  param_1[0xa6] = 0;
  if (param_4 != -1) {
    puVar3 = &param_4;
    iVar2 = param_4;
    do {
      iVar1 = puVar3[1];
      param_1[iVar2 + 0x26] = iVar1;
      if (iVar1 == -1) {
        param_1[0xa6] = param_1[0xa6] & ~(1 << ((byte)iVar2 & 0x1f));
      }
      else {
        param_1[0xa6] = param_1[0xa6] | 1 << ((byte)iVar2 & 0x1f);
      }
      iVar2 = puVar3[2];
      puVar3 = puVar3 + 2;
    } while (iVar2 != -1);
  }
  FUN_10116590(param_2,param_3,1);
  return param_1;
}


// 1011673e __wcsnicmp_l

/* Library Function - Single Match
    __wcsnicmp_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __wcsnicmp_l(wchar_t *_Str1,wchar_t *_Str2,size_t _MaxCount,_locale_t _Locale)

{
  wchar_t wVar1;
  wchar_t wVar2;
  wint_t wVar3;
  wint_t wVar4;
  int iVar5;
  undefined4 *puVar6;
  uint uVar7;
  uint uVar8;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  iVar5 = 0;
  if (_MaxCount != 0) {
    if ((_Str1 == (wchar_t *)0x0) || (_Str2 == (wchar_t *)0x0)) {
      puVar6 = (undefined4 *)FUN_10100c4a();
      *puVar6 = 0x16;
      FUN_10100beb(0,0,0,0,0);
      iVar5 = 0x7fffffff;
    }
    else {
      _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
      if ((local_14.locinfo)->lc_category[0].wlocale == (wchar_t *)0x0) {
        do {
          wVar1 = *_Str1;
          if ((0x40 < (ushort)wVar1) && ((ushort)wVar1 < 0x5b)) {
            wVar1 = wVar1 + L' ';
          }
          uVar8 = (uint)(ushort)wVar1;
          wVar2 = *_Str2;
          if ((0x40 < (ushort)wVar2) && ((ushort)wVar2 < 0x5b)) {
            wVar2 = wVar2 + L' ';
          }
          _Str1 = _Str1 + 1;
          _Str2 = _Str2 + 1;
          _MaxCount = _MaxCount - 1;
          uVar7 = (uint)(ushort)wVar2;
        } while (((_MaxCount != 0) && (wVar1 != L'\0')) && (wVar1 == wVar2));
      }
      else {
        do {
          wVar3 = __towlower_l(*_Str1,&local_14);
          uVar8 = (uint)wVar3;
          wVar4 = __towlower_l(*_Str2,&local_14);
          _Str1 = _Str1 + 1;
          _Str2 = _Str2 + 1;
          _MaxCount = _MaxCount - 1;
          uVar7 = (uint)wVar4;
          if ((_MaxCount == 0) || (wVar3 == 0)) break;
        } while (wVar3 == wVar4);
      }
      iVar5 = uVar8 - uVar7;
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
    }
  }
  return iVar5;
}


// 10116826 __wcsnicmp

/* Library Function - Single Match
    __wcsnicmp
   
   Library: Visual Studio 2005 Release */

int __cdecl __wcsnicmp(wchar_t *_Str1,wchar_t *_Str2,size_t _MaxCount)

{
  wchar_t wVar1;
  wchar_t wVar2;
  int iVar3;
  undefined4 *puVar4;
  
  if (DAT_101d9c58 == 0) {
    iVar3 = 0;
    if (_MaxCount != 0) {
      if ((_Str1 == (wchar_t *)0x0) || (_Str2 == (wchar_t *)0x0)) {
        puVar4 = (undefined4 *)FUN_10100c4a();
        *puVar4 = 0x16;
        FUN_10100beb(0,0,0,0,0);
        iVar3 = 0x7fffffff;
      }
      else {
        do {
          wVar1 = *_Str1;
          if ((0x40 < (ushort)wVar1) && ((ushort)wVar1 < 0x5b)) {
            wVar1 = wVar1 + L' ';
          }
          wVar2 = *_Str2;
          if ((0x40 < (ushort)wVar2) && ((ushort)wVar2 < 0x5b)) {
            wVar2 = wVar2 + L' ';
          }
          _Str1 = _Str1 + 1;
          _Str2 = _Str2 + 1;
          _MaxCount = _MaxCount - 1;
        } while (((_MaxCount != 0) && (wVar1 != L'\0')) && (wVar1 == wVar2));
        iVar3 = (uint)(ushort)wVar1 - (uint)(ushort)wVar2;
      }
    }
  }
  else {
    iVar3 = __wcsnicmp_l(_Str1,_Str2,_MaxCount,(_locale_t)0x0);
  }
  return iVar3;
}


// 101168ca _strlwr_s_l_stat

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
  
  uVar2 = DAT_101b229c ^ (uint)&stack0xfffffffc;
  if (param_1 == (char *)0x0) {
LAB_101168e6:
    puVar3 = (undefined4 *)FUN_10100c4a();
    uVar7 = 0x16;
  }
  else {
    sVar4 = _strnlen(param_1,param_2);
    if (param_2 <= sVar4) {
      *param_1 = '\0';
      goto LAB_101168e6;
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
      goto LAB_10116a39;
    }
    _CchDest = ___crtLCMapStringA(param_3,_LocaleName,0x100,param_1,-1,(LPSTR)0x0,0,
                                  param_3->locinfo->lc_codepage,1);
    if (_CchDest == 0) {
      puVar3 = (undefined4 *)FUN_10100c4a();
      *puVar3 = 0x2a;
LAB_10116970:
      FUN_10100c4a();
      goto LAB_10116a39;
    }
    if (_CchDest <= param_2) {
      if (((int)_CchDest < 1) || (0xffffffe0 / _CchDest == 0)) {
        local_c = (char *)0x0;
      }
      else if (_CchDest + 8 < 0x401) {
        pcVar5 = &stack0xffffffe4;
        local_c = &stack0xffffffe4;
        if (&stack0x00000000 != (undefined1 *)0x1c) {
LAB_101169cd:
          local_c = pcVar5 + 8;
        }
      }
      else {
        pcVar5 = (char *)FUN_10003140(_CchDest + 8);
        local_c = pcVar5;
        if (pcVar5 != (char *)0x0) {
          pcVar5[0] = -0x23;
          pcVar5[1] = -0x23;
          pcVar5[2] = '\0';
          pcVar5[3] = '\0';
          goto LAB_101169cd;
        }
      }
      if (local_c != (char *)0x0) {
        iVar6 = ___crtLCMapStringA(param_3,param_3->locinfo->lc_category[0].wlocale,0x100,param_1,-1
                                   ,local_c,_CchDest,param_3->locinfo->lc_codepage,1);
        if (iVar6 == 0) {
          puVar3 = (undefined4 *)FUN_10100c4a();
          *puVar3 = 0x2a;
        }
        else {
          _strcpy_s(param_1,param_2,local_c);
        }
        __freea(local_c);
        goto LAB_10116a39;
      }
      puVar3 = (undefined4 *)FUN_10100c4a();
      *puVar3 = 0xc;
      goto LAB_10116970;
    }
    *param_1 = '\0';
    puVar3 = (undefined4 *)FUN_10100c4a();
    uVar7 = 0x22;
  }
  *puVar3 = uVar7;
  FUN_10100beb(0,0,0,0,0);
LAB_10116a39:
  iVar6 = __security_check_cookie(uVar2 ^ (uint)&stack0xfffffffc);
  return iVar6;
}


// 10116a4b __strlwr_s_l

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


// 10116a7d __strlwr

/* Library Function - Single Match
    __strlwr
   
   Library: Visual Studio 2005 Release */

char * __cdecl __strlwr(char *_String)

{
  char cVar1;
  char *pcVar2;
  undefined4 *puVar3;
  
  if (DAT_101d9c58 == 0) {
    pcVar2 = _String;
    if (_String == (char *)0x0) {
      puVar3 = (undefined4 *)FUN_10100c4a();
      *puVar3 = 0x16;
      FUN_10100beb(0,0,0,0,0);
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


// 10116c70 FUN_10116c70

void FUN_10116c70(void)

{
  _atexit((_func_4879 *)&LAB_10119490);
  return;
}


// 10119440 FUN_10119440

void FUN_10119440(void)

{
  _SYSTEM_INFO local_24;
  
  GetSystemInfo(&local_24);
  DAT_101c17b8 = local_24.dwNumberOfProcessors;
  return;
}


