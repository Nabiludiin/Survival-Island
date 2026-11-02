// 10001000 FUN_10001000

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

void __cdecl FUN_10001000(undefined4 param_1,char *param_2)

{
  char local_2000 [4096];
  char local_1000 [4092];
  undefined4 uStack_4;
  
  uStack_4 = 0x1000100a;
  FUN_10007eb0(local_2000,0x1000,param_2,&stack0x0000000c);
  FUN_10007e60(local_1000,0x1000,"%s=%s");
  __putenv(local_1000);
  return;
}


// 10001070 FUN_10001070

void __cdecl FUN_10001070(char *param_1,uint param_2)

{
  __getcwd(param_1,param_2);
  FUN_10007890(param_1,"\\",param_2,0xffffffff);
  FUN_100079c0(param_1,'\\');
  return;
}


// 100010a0 FUN_100010a0

void __fastcall FUN_100010a0(undefined4 *param_1)

{
  *param_1 = 0;
  *(undefined1 *)(param_1 + 1) = 0;
  *(undefined1 *)((int)param_1 + 7) = 0;
  *(undefined1 *)((int)param_1 + 5) = 1;
  *(undefined1 *)(param_1 + 2) = 0;
  return;
}


// 100010c0 FUN_100010c0

void __fastcall FUN_100010c0(undefined1 *param_1)

{
  *param_1 = 1;
  *(undefined4 *)(param_1 + 4) = 0;
  *(undefined4 *)(param_1 + 8) = 0;
  return;
}


// 100010d0 FUN_100010d0

void FUN_100010d0(void)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  undefined4 uVar4;
  
  piVar2 = (int *)CommandLine_Tier0();
  piVar3 = (int *)CommandLine_Tier0();
  iVar1 = *piVar3;
  uVar4 = (**(code **)(*piVar2 + 0x20))("-game",0);
  (**(code **)(iVar1 + 0x20))("-vproject",uVar4);
  return;
}


// 10001110 FUN_10001110

bool __cdecl FUN_10001110(char *param_1,char *param_2)

{
  int iVar1;
  wchar_t local_104 [130];
  
  FUN_10007e20((char *)local_104,param_1,0x104);
  FUN_10007c70((char *)local_104,0x104);
  FUN_10007890((char *)local_104,param_2,0x104,0xffffffff);
  FUN_100079c0((char *)local_104,'\\');
  iVar1 = FID_conflict___access(local_104,0);
  return iVar1 == 0;
}


// 10001180 FUN_10001180

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

undefined4 __cdecl FUN_10001180(char *param_1,char *param_2)

{
  char cVar1;
  char *pcVar2;
  char *pcVar3;
  char local_2104 [260];
  char local_2000 [8188];
  undefined4 uStack_4;
  
  uStack_4 = 0x1000118a;
  FUN_10007e20(local_2000,param_1,0x2000);
  FUN_100079c0(local_2000,'\\');
  FUN_10007e20(local_2104,param_2,0x104);
  FUN_100079c0(local_2104,'\\');
  pcVar2 = local_2104;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  if (pcVar2 != local_2104 + 1) {
    pcVar2 = local_2104;
    do {
      pcVar3 = pcVar2;
      pcVar2 = pcVar3 + 1;
    } while (*pcVar3 != '\0');
    if (pcVar3[-1] != '\\') {
      pcVar2 = local_2104;
      do {
        pcVar3 = pcVar2;
        pcVar2 = pcVar3 + 1;
      } while (*pcVar3 != '\0');
      if (pcVar3[-1] != '/') goto LAB_1000123f;
    }
    pcVar2 = local_2104;
    do {
      pcVar3 = pcVar2;
      pcVar2 = pcVar3 + 1;
    } while (*pcVar3 != '\0');
    pcVar3[-1] = '\0';
  }
LAB_1000123f:
  pcVar2 = FUN_10007780(local_2000,local_2104);
  while( true ) {
    if (pcVar2 == (char *)0x0) {
      return 0;
    }
    pcVar3 = local_2104;
    do {
      cVar1 = *pcVar3;
      pcVar3 = pcVar3 + 1;
    } while (cVar1 != '\0');
    pcVar2 = pcVar2 + ((int)pcVar3 - (int)(local_2104 + 1));
    cVar1 = *pcVar2;
    if (((cVar1 == '\0') || (cVar1 == ';')) ||
       (((cVar1 == '\\' || (cVar1 == '/')) && (pcVar2[1] == ';')))) break;
    pcVar2 = FUN_10007780(pcVar2,local_2104);
  }
  return CONCAT31((int3)((uint)pcVar2 >> 8),1);
}


// 100012c0 FUN_100012c0

void __cdecl FUN_100012c0(void *param_1,int param_2)

{
  DWORD DVar1;
  int iVar2;
  CHAR local_104 [260];
  
  DVar1 = GetEnvironmentVariableA(*(LPCSTR *)(param_2 + 0x24),local_104,0x104);
  if ((DVar1 == 0) && (param_1 != (void *)0x0)) {
    iVar2 = FUN_10008e20(param_1,"SteamUserPassphrase",0);
    if (iVar2 != 0) {
      FUN_10001000(param_2 + 0x20,"%s");
    }
  }
  return;
}


// 10001320 FUN_10001320

void __cdecl FUN_10001320(undefined4 param_1)

{
  DAT_10024000 = param_1;
  return;
}


// 10001330 FUN_10001330

void __thiscall FUN_10001330(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000138b;
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
LAB_1000138b:
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


// 100013c0 FUN_100013c0

undefined4 __cdecl FUN_100013c0(char *param_1,uint param_2)

{
  int *piVar1;
  int *piVar2;
  undefined4 uVar3;
  int iVar4;
  char *pcVar5;
  size_t sVar6;
  HMODULE hModule;
  uint uVar7;
  bool bVar8;
  char cStack_104;
  byte abStack_103 [251];
  size_t sStack_8;
  
  bVar8 = DAT_10025e40 != '\0';
  *param_1 = '\0';
  if (bVar8) {
    piVar1 = (int *)CommandLine_Tier0();
    piVar2 = (int *)CommandLine_Tier0();
    iVar4 = *piVar2;
    uVar3 = (**(code **)(*piVar1 + 0x20))("-game",0);
    iVar4 = (**(code **)(iVar4 + 0x20))("-vproject",uVar3);
    if (iVar4 == 0) {
      pcVar5 = _getenv("VProject");
      if (pcVar5 == (char *)0x0) goto LAB_1000144a;
    }
    sVar6 = FUN_10007e60(param_1,sStack_8,"%s%c..%cbin");
    return CONCAT31((int3)(sVar6 >> 8),1);
  }
  pcVar5 = param_1;
  uVar7 = param_2;
  hModule = GetModuleHandleA((LPCSTR)0x0);
  pcVar5 = (char *)GetModuleFileNameA(hModule,pcVar5,uVar7);
  if (pcVar5 == (char *)0x0) {
LAB_1000144a:
    return (uint)pcVar5 & 0xffffff00;
  }
  FUN_10007970(param_1);
  FUN_100079c0(param_1,'\\');
  FUN_10008300(param_1,4,&cStack_104,0x104);
  if (cStack_104 == '\\') {
    uVar7 = FUN_10007680(abStack_103,&DAT_1001f348);
    if (uVar7 == 0) goto LAB_100014d8;
  }
  FUN_10007890(param_1,"\\",param_2,0xffffffff);
  FUN_10007890(param_1,"bin",param_2,0xffffffff);
  uVar7 = FUN_100079c0(param_1,'\\');
LAB_100014d8:
  return CONCAT31((int3)(uVar7 >> 8),1);
}


// 100014f0 FUN_100014f0

void FUN_100014f0(void)

{
  char *local_110;
  char *local_10c;
  undefined4 local_108;
  char local_104 [260];
  
  FUN_100013c0(local_104,0x104);
  FUN_10007c70(local_104,0x104);
  FUN_10007890(local_104,"vconfig.exe",0x104,0xffffffff);
  local_110 = local_104;
  local_10c = "-allowdebug";
  local_108 = 0;
  __spawnv(1,local_110,&local_110);
  return;
}


// 10001560 FUN_10001560

undefined4 __cdecl FUN_10001560(char param_1,undefined4 param_2,char *param_3)

{
  int *piVar1;
  int iVar2;
  
  FUN_10007eb0(&DAT_10025d40,0x100,param_3,&stack0x00000010);
  Warning(&DAT_1001f388,&DAT_10025d40);
  if ((param_1 != '\0') && (DAT_10024000 == 1)) {
    piVar1 = (int *)CommandLine_Tier0();
    iVar2 = (**(code **)(*piVar1 + 0x28))("-NoVConfig");
    if (iVar2 == 0) {
      iVar2 = FUN_100010d0();
      if (iVar2 == 0) {
        FUN_100014f0();
      }
    }
  }
  if ((DAT_10024000 == 0) || (DAT_10024000 == 1)) {
    Error(&DAT_1001f388,&DAT_10025d40);
  }
  return param_2;
}


// 100015f0 FUN_100015f0

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

undefined4 __cdecl FUN_100015f0(byte *param_1,size_t param_2,int param_3,char param_4)

{
  bool bVar1;
  undefined4 uVar2;
  char local_2104 [260];
  CHAR local_2000 [8188];
  undefined4 uStack_4;
  
  uStack_4 = 0x100015fa;
  uVar2 = FUN_100013c0(local_2104,0x104);
  if ((char)uVar2 == '\0') {
    if (param_4 != '\0') {
      Warning("SetSteamInstallPath: FileSystem_GetExecutableDir failed.\n");
      return 3;
    }
    uVar2 = FUN_10001560('\0',3,"FileSystem_GetExecutableDir failed.");
    return uVar2;
  }
  FUN_10007e20((char *)param_1,local_2104,param_2);
  do {
    bVar1 = FUN_10001110((char *)param_1,"steam.dll");
    if (bVar1) {
      bVar1 = FUN_10001110((char *)param_1,"steamapp.cfg");
      if (!bVar1) {
        GetEnvironmentVariableA(*(LPCSTR *)(param_3 + 100),local_2000,0x2000);
        uVar2 = FUN_10001180(local_2000,(char *)param_1);
        if ((char)uVar2 == '\0') {
          FUN_10001000(param_3 + 0x60,"%s%s%s");
        }
        return 0;
      }
    }
    uVar2 = FUN_10008140(param_1,param_2);
  } while ((char)uVar2 != '\0');
  if (param_4 != '\0') {
    Warning("Can\'t find %s relative to executable path: %s.\n","steam.dll",local_2104);
    return 5;
  }
  uVar2 = FUN_10001560('\0',5,"Can\'t find %s relative to executable path: %s.");
  return uVar2;
}


// 10001760 FUN_10001760

undefined4 __cdecl FUN_10001760(int *param_1)

{
  undefined4 uVar1;
  
  (**(code **)(*param_1 + 0x28))("EXECUTABLE_PATH");
  uVar1 = FUN_100013c0(&stack0xfffffef8,0x104);
  if ((char)uVar1 == '\0') {
    uVar1 = FUN_10001560('\0',3,"FileSystem_GetExecutableDir failed.");
    return uVar1;
  }
  (**(code **)(*param_1 + 0x1c))(&stack0xfffffef8,"EXECUTABLE_PATH",1,0);
  return 0;
}


// 100017d0 FUN_100017d0

undefined4 __cdecl FUN_100017d0(char *param_1,size_t param_2,undefined1 *param_3)

{
  undefined4 uVar1;
  char local_104 [260];
  
  *param_3 = 0;
  uVar1 = FUN_100013c0(local_104,0x104);
  if ((char)uVar1 == '\0') {
    uVar1 = FUN_10001560('\0',3,"FileSystem_GetExecutableDir failed.");
    return uVar1;
  }
  FUN_10007e60(param_1,param_2,"%s%cfilesystem_stdio.dll");
  return 0;
}


// 10001840 FUN_10001840

void __fastcall FUN_10001840(char *param_1)

{
  char local_200 [512];
  
  if (*param_1 != '\0') {
    if (param_1[8] != '\0') {
      FUN_10001000(param_1,"%s");
      FUN_10004260((int *)(param_1 + 0xc));
      return;
    }
    FUN_10007e60(local_200,0x200,"%s=");
    __putenv(local_200);
  }
  FUN_10004260((int *)(param_1 + 0xc));
  return;
}


// 100018b0 FUN_100018b0

undefined4 __cdecl FUN_100018b0(size_t param_1,char param_2)

{
  char *pcVar1;
  char *pcVar2;
  int iVar3;
  undefined4 uVar4;
  byte *unaff_ESI;
  wchar_t local_104 [130];
  
  pcVar1 = (char *)FUN_10005720(param_1);
  FUN_10007e20(pcVar1,(char *)unaff_ESI,param_1);
  (pcVar1 + param_1)[-1] = '\0';
  for (pcVar2 = pcVar1; pcVar2 < pcVar1 + param_1; pcVar2 = pcVar2 + 1) {
    if (*pcVar2 == '\\') {
      *pcVar2 = '/';
    }
  }
  do {
    FUN_10007e20((char *)local_104,(char *)unaff_ESI,0x104);
    FUN_10007c70((char *)local_104,0x104);
    FUN_10007890((char *)local_104,"gameinfo.txt",0x104,0xffffffff);
    FUN_100079c0((char *)local_104,'\\');
    iVar3 = FID_conflict___access(local_104,0);
    if (iVar3 == 0) {
      FUN_10005520(pcVar1);
      FUN_10005520(0);
      return 0;
    }
  } while ((param_2 != '\0') && (uVar4 = FUN_10008140(unaff_ESI,param_1), (char)uVar4 != '\0'));
  FUN_10007e20((char *)unaff_ESI,pcVar1,param_1);
  unaff_ESI[param_1 - 1] = 0;
  pcVar2 = (char *)FUN_10007860((char *)unaff_ESI,"/content/");
  if (pcVar2 != (char *)0x0) {
    _sprintf(pcVar2,"/game/");
    FID_conflict__memcpy(pcVar2 + 6,pcVar2 + 9,(size_t)(unaff_ESI + param_1 + (-9 - (int)pcVar2)));
    do {
      FUN_10007e20((char *)local_104,(char *)unaff_ESI,0x104);
      FUN_10007c70((char *)local_104,0x104);
      FUN_10007890((char *)local_104,"gameinfo.txt",0x104,0xffffffff);
      FUN_100079c0((char *)local_104,'\\');
      iVar3 = FID_conflict___access(local_104,0);
      if (iVar3 == 0) {
        FUN_10005520(pcVar1);
        FUN_10005520(0);
        return 0;
      }
    } while ((param_2 != '\0') && (uVar4 = FUN_10008140(unaff_ESI,param_1), (char)uVar4 != '\0'));
  }
  FUN_10005520(pcVar1);
  FUN_10005520(0);
  return 1;
}


// 10001a80 FUN_10001a80

undefined4 __cdecl FUN_10001a80(int *param_1,char *param_2,size_t param_3)

{
  bool bVar1;
  char cVar2;
  undefined4 uVar3;
  int *piVar4;
  int *piVar5;
  char *pcVar6;
  int iVar7;
  char *unaff_EBX;
  size_t unaff_EBP;
  char cVar8;
  
  if ((char)param_1[1] != '\0') {
    if ((char *)*param_1 == (char *)0x0) {
      uVar3 = FUN_10001560('\0',1,"bOnlyUseDirectoryName=1 and pDirectoryName=NULL.");
      return uVar3;
    }
    bVar1 = FUN_10001110((char *)*param_1,"gameinfo.txt");
    if (!bVar1) {
      uVar3 = FUN_10001560('\x01',1,
                           "Setup file \'%s\' doesn\'t exist in subdirectory \'%s\'.\nCheck your -game parameter or VCONFIG setting."
                          );
      return uVar3;
    }
    FUN_10007e20(param_2,(char *)*param_1,param_3);
    return 0;
  }
  piVar4 = (int *)CommandLine_Tier0();
  piVar5 = (int *)CommandLine_Tier0();
  iVar7 = *piVar5;
  uVar3 = (**(code **)(*piVar4 + 0x20))("-game",0);
  pcVar6 = (char *)(**(code **)(iVar7 + 0x20))("-vproject",uVar3);
  if (pcVar6 != (char *)0x0) {
    bVar1 = FUN_10001110(pcVar6,"gameinfo.txt");
    if (bVar1) {
      FUN_100081e0(unaff_EBX,unaff_EBP,pcVar6,(char *)0x0);
      return 0;
    }
    if ((char)param_1[2] != '\0') {
      FUN_10007e20(unaff_EBX,"",unaff_EBP);
      return 0;
    }
LAB_10001c5f:
    uVar3 = FUN_10001560('\x01',1,
                         "Unable to find %s. Solutions:\n\n1. Read http://www.valve-erc.com/srcsdk/faq.html#NoGameDir\n2. Run vconfig to specify which game you\'re working on.\n3. Add -game <path> on the command line where <path> is the directory that %s is in.\n"
                        );
    return uVar3;
  }
  if ((char)param_1[2] == '\0') {
    cVar8 = '\x01';
    if (((DAT_10025e4c == (code *)0x0) ||
        (cVar2 = (*DAT_10025e4c)(param_1,unaff_EBX,unaff_EBP,&stack0xfffffff4), cVar2 == '\0')) ||
       (iVar7 = FUN_100018b0(unaff_EBP,cVar8), iVar7 != 0)) {
      pcVar6 = _getenv("VProject");
      if (pcVar6 != (char *)0x0) {
        FUN_100081e0(unaff_EBX,unaff_EBP,pcVar6,(char *)0x0);
        iVar7 = FUN_100018b0(unaff_EBP,'\0');
        if (iVar7 == 0) {
          return 0;
        }
      }
      Warning("Warning: falling back to auto detection of vproject directory.\n");
      pcVar6 = (char *)*param_1;
      if (pcVar6 == (char *)0x0) {
        pcVar6 = ".";
      }
      FUN_100081e0(unaff_EBX,unaff_EBP,pcVar6,(char *)0x0);
      iVar7 = FUN_100018b0(unaff_EBP,'\x01');
      if (iVar7 != 0) {
        FUN_10001070(unaff_EBX,unaff_EBP);
        iVar7 = FUN_100018b0(unaff_EBP,'\x01');
        if (iVar7 != 0) goto LAB_10001c5f;
      }
    }
  }
  else {
    FUN_10007e20(unaff_EBX,"",unaff_EBP);
  }
  return 0;
}


// 10001c80 FUN_10001c80

void __fastcall FUN_10001c80(char *param_1)

{
  FUN_10001840(param_1 + 0x60);
  FUN_10001840(param_1 + 0x40);
  FUN_10001840(param_1 + 0x20);
  FUN_10001840(param_1);
  return;
}


// 10001cb0 FUN_10001cb0

void __thiscall FUN_10001cb0(void *this,int param_1)

{
  void *pvVar1;
  size_t sVar2;
  
  *(undefined4 *)((int)this + 0xc) = 0;
  if (param_1 != 0) {
    if (*(int *)((int)this + 4) < param_1) {
      FUN_10001330(this,param_1 - *(int *)((int)this + 4));
    }
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_1;
                    /* WARNING: Load size is inaccurate */
    pvVar1 = *this;
    sVar2 = *(int *)((int)this + 0xc) - param_1;
    *(void **)((int)this + 0x10) = pvVar1;
    if ((0 < (int)sVar2) && (0 < param_1)) {
      FUN_10007df0((void *)((int)pvVar1 + param_1),pvVar1,sVar2);
    }
  }
  return;
}


// 10001d00 FUN_10001d00

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

undefined1 * __thiscall FUN_10001d00(void *this,LPCSTR param_1)

{
  undefined4 *this_00;
  char cVar1;
  DWORD DVar2;
  char *pcVar3;
  CHAR local_1000 [4092];
  undefined4 uStack_4;
  
  uStack_4 = 0x10001d0a;
  this_00 = (undefined4 *)((int)this + 0xc);
  *this_00 = 0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 0x18) = 0;
  *(undefined4 *)((int)this + 0x1c) = 0;
  *(undefined1 *)this = 1;
  *(LPCSTR *)((int)this + 4) = param_1;
  DVar2 = GetEnvironmentVariableA(param_1,local_1000,0x1000);
  if (DVar2 != 0) {
    pcVar3 = local_1000;
    *(undefined1 *)((int)this + 8) = 1;
    do {
      cVar1 = *pcVar3;
      pcVar3 = pcVar3 + 1;
    } while (cVar1 != '\0');
    FUN_10001cb0(this_00,(int)(pcVar3 + (1 - (int)(local_1000 + 1))));
    _memcpy((void *)*this_00,local_1000,*(size_t *)((int)this + 0x18));
    return (undefined1 *)this;
  }
  *(undefined1 *)((int)this + 8) = 0;
  return (undefined1 *)this;
}


// 10001da0 FUN_10001da0

undefined4 * __cdecl FUN_10001da0(char *param_1)

{
  char *pcVar1;
  int iVar2;
  FILE *_File;
  long lVar3;
  void *this;
  undefined4 *this_00;
  uint uVar4;
  char *local_14;
  undefined4 local_10;
  undefined4 local_c;
  int local_8;
  undefined4 local_4;
  
  _File = _fopen(param_1,"rb");
  if (_File != (FILE *)0x0) {
    local_14 = (char *)0x0;
    local_10 = 0;
    local_c = 0;
    local_8 = 0;
    local_4 = 0;
    _fseek(_File,0,2);
    lVar3 = _ftell(_File);
    FUN_10001cb0(&local_14,lVar3 + 1);
    _fseek(_File,0,0);
    iVar2 = local_8;
    pcVar1 = local_14;
    FUN_1000ebbf(local_14,1,local_8 - 1,_File);
    _fclose(_File);
    pcVar1[iVar2 + -1] = '\0';
    this = (void *)FUN_10008840(0x24);
    if (this == (void *)0x0) {
      this_00 = (undefined4 *)0x0;
    }
    else {
      this_00 = FUN_10008860(this,&DAT_1001f5e8);
    }
    uVar4 = FUN_10009ab0(this_00,param_1,pcVar1,(int *)0x0,(void *)0x0);
    if ((char)uVar4 != '\0') {
      FUN_10004260((int *)&local_14);
      return this_00;
    }
    FUN_10008cb0((int)this_00);
    FUN_10004260((int *)&local_14);
  }
  return (undefined4 *)0x0;
}


// 10001e80 FUN_10001e80

undefined4 __cdecl FUN_10001e80(char *param_1,int *param_2,undefined4 *param_3,undefined4 *param_4)

{
  undefined4 *this;
  undefined4 uVar1;
  int *piVar2;
  char local_104 [260];
  
  FUN_10007e20(local_104,param_1,0x104);
  FUN_10007c70(local_104,0x104);
  FUN_10007890(local_104,"gameinfo.txt",0x104,0xffffffff);
  FUN_100079c0(local_104,'\\');
  this = FUN_10001da0(local_104);
  *param_2 = (int)this;
  if (this == (undefined4 *)0x0) {
    uVar1 = FUN_10001560('\x01',1,"%s is missing.");
    return uVar1;
  }
  piVar2 = FUN_100088b0(this,"FileSystem",0);
  *param_3 = piVar2;
  if (piVar2 != (int *)0x0) {
    piVar2 = FUN_100088b0(piVar2,"SearchPaths",0);
    *param_4 = piVar2;
    if (piVar2 != (int *)0x0) {
      return 0;
    }
  }
  FUN_10008cb0(*param_2);
  uVar1 = FUN_10001560('\x01',2,"%s is not a valid format.");
  return uVar1;
}


// 10001f70 FUN_10001f70

void __cdecl FUN_10001f70(void *param_1,char *param_2,int param_3)

{
  DWORD DVar1;
  undefined4 *this;
  char *pcVar2;
  CHAR local_208 [260];
  char acStack_104 [260];
  
  DVar1 = GetEnvironmentVariableA(*(LPCSTR *)(param_3 + 0x44),local_208,0x104);
  if (DVar1 == 0) {
    pcVar2 = (char *)0x0;
    if ((param_1 == (void *)0x0) ||
       (pcVar2 = (char *)FUN_10008e20(param_1,"SteamAppUser",0), pcVar2 == (char *)0x0)) {
      FUN_10007e20(acStack_104,param_2,0x104);
      FUN_10007c70(acStack_104,0x104);
      FUN_10007890(acStack_104,"config\\SteamAppData.vdf",0x104,0xffffffff);
      this = FUN_10001da0(acStack_104);
      if ((this == (undefined4 *)0x0) ||
         (pcVar2 = (char *)FUN_10008e20(this,"AutoLoginUser",0), pcVar2 == (char *)0x0)) {
        Error("Can\'t find steam app user info.");
      }
      FUN_10007e20(local_208,pcVar2,0x104);
      FUN_10008cb0((int)this);
    }
    else {
      FUN_10007e20(local_208,pcVar2,0x104);
    }
    FUN_10007700(local_208);
    FUN_10001000(param_3 + 0x40,"%s");
    return;
  }
  return;
}


// 100020a0 FUN_100020a0

int __cdecl FUN_100020a0(void *param_1,char *param_2,int param_3)

{
  undefined4 *puVar1;
  int iVar2;
  char local_208 [260];
  byte local_104 [260];
  
  FUN_10007e20(local_208,param_2,0x104);
  FUN_10007c70(local_208,0x104);
  FUN_10007890(local_208,"steaminfo.txt",0x104,0xffffffff);
  puVar1 = FUN_10001da0(local_208);
  iVar2 = FUN_100015f0(local_104,0x104,param_3,'\0');
  if (iVar2 == 0) {
    FUN_10001f70(puVar1,(char *)local_104,param_3);
    FUN_100012c0(puVar1,param_3);
    iVar2 = FUN_10008a10(param_1,"SteamAppId",-1);
    if (iVar2 == -1) {
      Error("Missing SteamAppId in %s\\%s.",param_2,"gameinfo.txt");
    }
    FUN_10001000(param_3,"%d");
    if (puVar1 != (undefined4 *)0x0) {
      FUN_10008cb0((int)puVar1);
    }
    iVar2 = 0;
  }
  return iVar2;
}


// 10002190 FUN_10002190

int __cdecl FUN_10002190(char *param_1,int *param_2)

{
  int iVar1;
  void *local_c;
  int local_8;
  undefined4 local_4;
  
  iVar1 = FUN_10001e80(param_1,&local_8,&local_c,&local_4);
  if (iVar1 == 0) {
    iVar1 = FUN_10008a10(local_c,"ToolsAppId",-1);
    *param_2 = iVar1;
    FUN_10008cb0(local_8);
    iVar1 = 0;
  }
  return iVar1;
}


// 100021e0 FUN_100021e0

undefined4 FUN_100021e0(void)

{
  undefined4 uVar1;
  char local_184 [32];
  char local_164 [32];
  char local_144 [32];
  char local_124 [32];
  byte local_104 [260];
  
  FUN_10001d00(local_184,"SteamAppId");
  FUN_10001d00(local_164,"SteamUserPassphrase");
  FUN_10001d00(local_144,"SteamAppUser");
  FUN_10001d00(local_124,"path");
  uVar1 = FUN_100015f0(local_104,0x104,(int)local_184,'\x01');
  local_124[0] = '\0';
  FUN_10001840(local_124);
  FUN_10001840(local_144);
  FUN_10001840(local_164);
  FUN_10001840(local_184);
  return uVar1;
}


// 10002270 FUN_10002270

int __cdecl FUN_10002270(int *param_1)

{
  char *pcVar1;
  int iVar2;
  void *local_2b4;
  int local_2b0;
  undefined4 local_2ac;
  char local_2a8 [32];
  char local_288 [32];
  char local_268 [32];
  char local_248 [32];
  char local_228 [292];
  byte local_104 [260];
  
  pcVar1 = (char *)((int)param_1 + 9);
  iVar2 = FUN_10001a80(param_1,pcVar1,0x200);
  if (iVar2 == 0) {
    FUN_10007e60(local_228,0x124,"%s=%s");
    __putenv(local_228);
    FUN_10001d00(local_2a8,"SteamAppId");
    FUN_10001d00(local_288,"SteamUserPassphrase");
    FUN_10001d00(local_268,"SteamAppUser");
    FUN_10001d00(local_248,"path");
    if (*(char *)((int)param_1 + 7) != '\0') {
      if (*(char *)((int)param_1 + 5) == '\0') {
        if (*(char *)((int)param_1 + 6) != '\0') {
          FUN_100015f0(local_104,0x104,(int)local_2a8,'\x01');
          local_248[0] = '\0';
        }
      }
      else {
        iVar2 = FUN_10001e80(pcVar1,&local_2b0,&local_2b4,&local_2ac);
        if (iVar2 != 0) {
          FUN_10001c80(local_2a8);
          return iVar2;
        }
        iVar2 = FUN_100020a0(local_2b4,pcVar1,(int)local_2a8);
        if (iVar2 != 0) {
          FUN_10001c80(local_2a8);
          return iVar2;
        }
        local_2a8[0] = '\0';
        FUN_10008cb0(local_2b0);
      }
    }
    FUN_10001840(local_248);
    FUN_10001840(local_268);
    FUN_10001840(local_288);
    FUN_10001840(local_2a8);
    iVar2 = 0;
  }
  return iVar2;
}


// 100023e0 FUN_100023e0

void __cdecl FUN_100023e0(char *param_1)

{
  char cVar1;
  int iVar2;
  undefined4 uVar3;
  int iStack_20c;
  char acStack_208 [260];
  char acStack_104 [260];
  
  cVar1 = (**(code **)(**(int **)(param_1 + 8) + 0x14))();
  if (cVar1 != '\0') {
    iStack_20c = -1;
    if ((*param_1 != '\0') &&
       (iVar2 = FUN_10002190(*(char **)(param_1 + 4),&iStack_20c), iVar2 != 0)) {
      return;
    }
    uVar3 = FUN_100013c0(acStack_104,0x104);
    if ((char)uVar3 == '\0') {
      FUN_10001560('\0',3,"FileSystem_GetBaseDir failed.");
      return;
    }
    FUN_10007970(acStack_104);
    __getcwd(acStack_208,0x104);
    FUN_10007890(acStack_208,"\\",0x104,0xffffffff);
    FUN_100079c0(acStack_208,'\\');
    __chdir(acStack_104);
    iVar2 = (**(code **)(**(int **)(param_1 + 8) + 0x18))(iStack_20c);
    __chdir(acStack_208);
    if (iVar2 != 0) {
      FUN_10001560('\x01',4,"Unable to mount Steam content in the file system");
      return;
    }
  }
  FUN_10001760(*(int **)(param_1 + 8));
  return;
}


// 10002500 FUN_10002500

undefined4 __cdecl FUN_10002500(undefined4 param_1,LPCSTR param_2)

{
  char cVar1;
  byte *pbVar2;
  uint uVar3;
  byte *pbVar4;
  
  OutputDebugStringA(param_2);
  switch(param_1) {
  case 0:
  case 4:
    goto switchD_10002515_caseD_0;
  case 1:
    pbVar4 = &DAT_1001f804;
    pbVar2 = (byte *)GetSpewOutputGroup();
    uVar3 = FUN_10007680(pbVar2,pbVar4);
    if (uVar3 == 0) {
      MessageBoxA((HWND)0x0,param_2,"Warning!",0x1010);
    }
switchD_10002515_caseD_0:
    return 1;
  case 2:
    cVar1 = ShouldUseNewAssertDialog();
    if (cVar1 == '\0') {
      MessageBoxA((HWND)0x0,param_2,"Assert!",0x1010);
    }
    return 0;
  default:
    MessageBoxA((HWND)0x0,param_2,"Error!",0x1010);
                    /* WARNING: Subroutine does not return */
    __exit(1);
  }
}


// 100025a0 FUN_100025a0

undefined * FUN_100025a0(void)

{
  HMODULE hModule;
  wchar_t *lpFilename;
  DWORD DVar1;
  wchar_t local_104 [130];
  
  lpFilename = local_104;
  DVar1 = 0x104;
  DAT_100261d8 = 0;
  hModule = GetModuleHandleA((LPCSTR)0x0);
  DVar1 = GetModuleFileNameA(hModule,(LPSTR)lpFilename,DVar1);
  if (DVar1 != 0) {
    FID_conflict___splitpath
              (local_104,(wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)&DAT_100261d8,(wchar_t *)0x0);
  }
  FUN_10007700(&DAT_100261d8);
  return &DAT_100261d8;
}


// 10002600 FUN_10002600

void FUN_10002600(void)

{
  char cVar1;
  HMODULE hModule;
  char *pcVar2;
  char *pcVar3;
  int *piVar4;
  int iVar5;
  LPSTR lpFilename;
  DWORD DVar6;
  
  DVar6 = 0x104;
  lpFilename = &DAT_10025e58;
  DAT_10025e58 = 0;
  hModule = GetModuleHandleA((LPCSTR)0x0);
  DVar6 = GetModuleFileNameA(hModule,lpFilename,DVar6);
  if (DVar6 != 0) {
    pcVar2 = _strrchr(&DAT_10025e58,0x5c);
    if (*pcVar2 != '\0') {
      pcVar2[1] = '\0';
    }
    pcVar2 = &DAT_10025e58;
    do {
      pcVar3 = pcVar2;
      pcVar2 = pcVar3 + 1;
    } while (*pcVar3 != '\0');
    if ((0 < (int)(pcVar3 + -0x10025e58)) && ((pcVar3[-1] == '\\' || (pcVar3[-1] == '/')))) {
      pcVar3[-1] = '\0';
    }
  }
  piVar4 = (int *)CommandLine_Tier0();
  pcVar2 = (char *)(**(code **)(*piVar4 + 0xc))("-basedir",0);
  if (pcVar2 != (char *)0x0) {
    iVar5 = (int)&DAT_10025e58 - (int)pcVar2;
    do {
      cVar1 = *pcVar2;
      pcVar2[iVar5] = cVar1;
      pcVar2 = pcVar2 + 1;
    } while (cVar1 != '\0');
  }
  FUN_10007700(&DAT_10025e58);
  FUN_100079c0(&DAT_10025e58,'\\');
  return;
}


// 100026e0 FUN_100026e0

void FUN_100026e0(void)

{
  FILE *_File;
  undefined **ppuVar1;
  
  AllocConsole();
  SetConsoleCtrlHandler((PHANDLER_ROUTINE)&LAB_100026c0,1);
  _File = (FILE *)FUN_1000f7b5();
  _freopen("CONIN$","rb",_File);
  ppuVar1 = FUN_1000f7b5();
  _freopen("CONOUT$","wb",(FILE *)(ppuVar1 + 8));
  ppuVar1 = FUN_1000f7b5();
  _freopen("CONOUT$","wb",(FILE *)(ppuVar1 + 0x10));
  return;
}


// 10002780 FUN_10002780

void FUN_10002780(void)

{
  bool bVar1;
  HMODULE pHVar2;
  LSTATUS LVar3;
  HANDLE hProcess;
  BOOL BVar4;
  int iVar5;
  DWORD DStack_114;
  HKEY pHStack_110;
  DWORD DStack_10c;
  DWORD DStack_108;
  BYTE aBStack_104 [260];
  
  pHVar2 = GetModuleHandleA("GameOverlayRenderer.dll");
  if (pHVar2 == (HMODULE)0x0) {
    bVar1 = false;
    DStack_10c = RegOpenKeyExA((HKEY)0x80000001,"Software\\Valve\\Steam\\ActiveProcess",0,0x20019,
                               &pHStack_110);
    if (DStack_10c == 0) {
      DStack_114 = 4;
      LVar3 = RegQueryValueExA(pHStack_110,"pid",(LPDWORD)0x0,(LPDWORD)0x0,(LPBYTE)&DStack_10c,
                               &DStack_114);
      if ((LVar3 == 0) && (hProcess = OpenProcess(0x400,0,DStack_10c), hProcess != (HANDLE)0x0)) {
        DStack_108 = 0;
        BVar4 = GetExitCodeProcess(hProcess,&DStack_108);
        if ((BVar4 == 0) || (DStack_108 != 0x103)) {
          bVar1 = false;
        }
        else {
          bVar1 = true;
        }
        CloseHandle(hProcess);
        if (bVar1) {
          DStack_114 = 0x104;
          LVar3 = RegQueryValueExA(pHStack_110,"SteamClientDll",(LPDWORD)0x0,(LPDWORD)0x0,
                                   aBStack_104,&DStack_114);
          if (LVar3 == 0) {
            if ((DStack_114 == 0) || (iVar5 = FUN_100075f0((char *)aBStack_104), iVar5 < 1)) {
              bVar1 = false;
            }
            else {
              FUN_10007970((char *)aBStack_104);
            }
          }
        }
      }
      RegCloseKey(pHStack_110);
      if (bVar1) {
        FUN_10007890((char *)aBStack_104,"\\GameOverlayRenderer.dll",0x104,0xffffffff);
        LoadLibraryA((LPCSTR)aBStack_104);
      }
    }
  }
  return;
}


// 10002c00 FUN_10002c00

void __fastcall FUN_10002c00(int param_1)

{
  int *piVar1;
  
  if (*(char *)(param_1 + 0x158) == '\0') {
    piVar1 = (int *)CommandLine_Tier0();
    (**(code **)(*piVar1 + 0x20))("-game",&DAT_1001fb80);
    return;
  }
                    /* WARNING: Could not recover jumptable at 0x10002c2e. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(*DAT_10025e50 + 0x24))();
  return;
}


// 10002c30 FUN_10002c30

void __fastcall FUN_10002c30(int param_1)

{
  int *piVar1;
  
  if (*(char *)(param_1 + 0x158) == '\0') {
    piVar1 = (int *)CommandLine_Tier0();
    (**(code **)(*piVar1 + 0x20))("-defaultgamedir",&DAT_1001fb80);
    return;
  }
                    /* WARNING: Could not recover jumptable at 0x10002c5e. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(*DAT_10025e50 + 0x28))();
  return;
}


// 10002c60 FUN_10002c60

uint FUN_10002c60(void)

{
  DWORD DVar1;
  uint uVar2;
  
  DAT_10026068 = CreateMutexA((LPSECURITY_ATTRIBUTES)0x0,0,"hl2_singleton_mutex");
  DVar1 = WaitForSingleObject(DAT_10026068,0);
  if ((DVar1 != 0) && (DVar1 != 0x80)) {
    uVar2 = CloseHandle(DAT_10026068);
    return uVar2 & 0xffffff00;
  }
  return CONCAT31((int3)(DVar1 >> 8),1);
}


// 10002ca0 FUN_10002ca0

void FUN_10002ca0(void)

{
  if (DAT_10026068 != (HANDLE)0x0) {
    ReleaseMutex(DAT_10026068);
    CloseHandle(DAT_10026068);
    DAT_10026068 = (HANDLE)0x0;
  }
  return;
}


// 10002cd0 FUN_10002cd0

void FUN_10002cd0(void)

{
  int *piVar1;
  int iVar2;
  byte *pbVar3;
  uint uVar4;
  int iVar5;
  byte *pbVar6;
  byte *pbVar7;
  
  iVar5 = 0;
  pbVar6 = (byte *)0x0;
  piVar1 = (int *)CommandLine_Tier0();
  iVar2 = (**(code **)(*piVar1 + 0x24))();
  if (iVar2 != 1 && -1 < iVar2 + -1) {
    do {
      piVar1 = (int *)CommandLine_Tier0();
      pbVar7 = pbVar6;
      pbVar3 = (byte *)(**(code **)(*piVar1 + 0x2c))(pbVar6,"-game");
      uVar4 = FUN_10007680(pbVar3,pbVar7);
      if (uVar4 == 0) {
        piVar1 = (int *)CommandLine_Tier0();
        pbVar6 = pbVar6 + 1;
        (**(code **)(*piVar1 + 0x2c))(pbVar6);
        FUN_10007e60(&stack0xfffffef8,0x104,"\"%s\"");
        iVar5 = iVar5 + 1;
      }
      pbVar6 = pbVar6 + 1;
      piVar1 = (int *)CommandLine_Tier0();
      iVar2 = (**(code **)(*piVar1 + 0x24))();
    } while ((int)pbVar6 < iVar2 + -1);
    if (1 < iVar5) {
      piVar1 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar1 + 0x10))("-game");
      piVar1 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar1 + 0x14))("-game",&stack0xfffffef8);
    }
  }
  return;
}


// 10002da0 FUN_10002da0

undefined * __cdecl FUN_10002da0(char *param_1)

{
  DAT_100272e0 = DAT_100272e0 + 1 & 0x80000007;
  if ((int)DAT_100272e0 < 0) {
    DAT_100272e0 = (DAT_100272e0 - 1 | 0xfffffff8) + 1;
  }
  FUN_10007eb0(&DAT_100262e0 + DAT_100272e0 * 0x200,0x200,param_1,&stack0x00000008);
  return &DAT_100262e0 + DAT_100272e0 * 0x200;
}


// 10002df0 FUN_10002df0

undefined * FUN_10002df0(void)

{
  char *in_EAX;
  char *pcVar1;
  int iVar2;
  int *piVar3;
  undefined *puVar4;
  undefined **ppuVar5;
  char local_200 [512];
  
  FUN_10007e20(local_200,in_EAX,0x200);
  FUN_100079c0(local_200,'\\');
  FUN_10007700(local_200);
  pcVar1 = FUN_100079f0(local_200);
  if (pcVar1 == (char *)0x0) {
    return (undefined *)0x0;
  }
  ppuVar5 = &PTR_DAT_1002400c;
  do {
    iVar2 = FUN_10007620((byte *)(pcVar1 + -1),*ppuVar5);
    if (iVar2 == 0) {
      piVar3 = (int *)CommandLine_Tier0();
      iVar2 = *piVar3;
      puVar4 = FUN_10002da0("+%s");
      iVar2 = (**(code **)(iVar2 + 0x28))(puVar4);
      if (iVar2 == 0) {
        FUN_10007e20(&DAT_100272e8,local_200,0x200);
        FUN_10007fb0(&DAT_100272e8,local_200,(char *)0x200);
        FUN_10007e60(&DAT_100272e8,0x200,"%s %s");
        return &DAT_100272e8;
      }
    }
    ppuVar5 = ppuVar5 + 2;
  } while ((int)ppuVar5 < 0x10024024);
  return (undefined *)0x0;
}


// 10002f20 FUN_10002f20

void __thiscall FUN_10002f20(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_10002f7b;
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
LAB_10002f7b:
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


// 10002ff0 FUN_10002ff0

undefined4 * __cdecl FUN_10002ff0(undefined4 *param_1,char *param_2)

{
  char *pcVar1;
  undefined4 *puVar2;
  int iVar3;
  
  puVar2 = param_1;
  pcVar1 = (char *)((int)param_1 + 5);
  *param_1 = &PTR_LAB_1001fbd8;
  *(undefined1 *)(param_1 + 1) = 1;
  param_1 = (undefined4 *)((uint)param_1 & 0xffffff00);
  FUN_10007f00(pcVar1,0xff,param_2,&stack0x0000000c,(int)&param_1);
  *(undefined1 *)(puVar2 + 0x41) = 0;
  if ((((char)param_1 != '\0') && (*(char *)(puVar2 + 1) == '\0')) && (DAT_100274e8 < 5)) {
    DAT_100274e8 = DAT_100274e8 + 1;
  }
  iVar3 = FUN_100075f0(pcVar1);
  puVar2[0x42] = iVar3;
  return puVar2;
}


// 10003060 FUN_10003060

void __thiscall FUN_10003060(void *this,short *param_1,short *param_2)

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


// 100030a0 FUN_100030a0

undefined4 __thiscall FUN_100030a0(void *this,int param_1)

{
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4) + 8);
  }
  return DAT_100274f4;
}


// 100030f0 FUN_100030f0

undefined2 * __thiscall FUN_100030f0(void *this,ushort param_1)

{
  if ((DAT_10027508 & 1) == 0) {
    DAT_10027508 = DAT_10027508 | 1;
    DAT_10027500 = 0xffff;
    DAT_10027502 = 0xffff;
    DAT_10027504 = 0xffff;
    DAT_10027506 = 1;
  }
  if (param_1 != 0xffff) {
    return (undefined2 *)((uint)param_1 * 0x10 + *(int *)((int)this + 4));
  }
  return &DAT_10027500;
}


// 10003150 FUN_10003150

void __fastcall FUN_10003150(int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  int iVar3;
  int iVar4;
  int iVar5;
  undefined4 local_10c [62];
  char *pcStack_14;
  
  FUN_10009ff0((undefined4 *)(param_1 + 300));
  FUN_10009ff0((undefined4 *)(param_1 + 0x13c));
  puVar2 = FUN_10002ff0(local_10c,"%s\\%s\\%s");
  iVar3 = (**(code **)(*(int *)(DAT_10027684 + 4) + 8))((int)puVar2 + 5,&DAT_1001fbe4,&DAT_1001f2bc)
  ;
  if (iVar3 != 0) {
    (**(code **)(*(int *)(DAT_10027684 + 4) + 4))(&DAT_1001fbe0,1,iVar3);
    iVar1 = *(int *)(DAT_10027684 + 4);
    iVar5 = iVar3;
    iVar4 = FUN_100075f0(pcStack_14);
    (**(code **)(iVar1 + 4))(pcStack_14,iVar4,iVar5);
    (**(code **)(*(int *)(DAT_10027684 + 4) + 4))(&DAT_1001fbdc,2,iVar3);
    (**(code **)(*(int *)(DAT_10027684 + 4) + 0xc))(iVar3);
  }
  return;
}


// 10003230 FUN_10003230

void __thiscall FUN_10003230(void *this,undefined1 param_1)

{
  undefined4 uVar1;
  
  uVar1 = FUN_1000a410(this,1);
  if ((char)uVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
    FUN_1000a460((int *)this);
  }
  return;
}


// 10003260 FUN_10003260

undefined4 __thiscall FUN_10003260(void *this,int param_1)

{
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4));
  }
  return DAT_100274ec;
}


// 100032b0 FUN_100032b0

undefined4 __thiscall FUN_100032b0(void *this,int param_1)

{
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (param_1 != -1) {
    return *(undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4) + 4);
  }
  return DAT_100274f0;
}


// 10003300 FUN_10003300

int * __thiscall FUN_10003300(void *this,undefined4 param_1,int *param_2,undefined1 *param_3)

{
  int iVar1;
  char cVar2;
  int *piVar3;
  int iVar4;
  
  iVar1 = *(int *)((int)this + 0x10);
  *param_2 = -1;
  *param_3 = 0;
  piVar3 = param_2;
  while (iVar1 != -1) {
    *param_2 = iVar1;
                    /* WARNING: Load size is inaccurate */
    iVar4 = iVar1 * 0x20;
    cVar2 = (**this)(param_1,*(int *)((int)this + 4) + 0x10 + iVar4);
    if (cVar2 == '\0') {
      *param_3 = 0;
      if ((DAT_100274fc & 1) == 0) {
        DAT_100274fc = DAT_100274fc | 1;
        DAT_100274ec = -1;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
      }
      if (iVar1 == -1) {
        piVar3 = &DAT_100274ec;
      }
      else {
        piVar3 = (int *)(*(int *)((int)this + 4) + iVar4);
      }
      iVar1 = piVar3[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_100274fc & 1) == 0) {
        DAT_100274fc = DAT_100274fc | 1;
        DAT_100274ec = -1;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
      }
      if (iVar1 == -1) {
        piVar3 = &DAT_100274ec;
        iVar1 = DAT_100274ec;
      }
      else {
        piVar3 = (int *)(*(int *)((int)this + 4) + iVar4);
        iVar1 = *(int *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  }
  return piVar3;
}


// 100033f0 FUN_100033f0

int __fastcall FUN_100033f0(int param_1)

{
  undefined4 *puVar1;
  int iVar2;
  int iVar3;
  
  iVar2 = *(int *)(param_1 + 0x18);
  if (iVar2 == -1) {
    iVar2 = *(int *)(param_1 + 0x1c);
    if (((iVar2 == 0) || (0 < iVar2)) && (iVar3 = *(int *)(param_1 + 8), iVar2 < iVar3)) {
      iVar2 = iVar2 + 1;
      if (((iVar2 != 0) && (iVar2 < 1)) || (iVar3 <= iVar2)) {
        iVar2 = -1;
      }
    }
    else {
      iVar3 = *(int *)(param_1 + 8);
      iVar2 = (0 < iVar3) - 1;
    }
    if (((iVar2 != 0) && (iVar2 < 1)) || (iVar3 <= iVar2)) {
      FUN_10002f20((void *)(param_1 + 4),1);
      iVar2 = *(int *)(param_1 + 0x1c);
      if (((iVar2 == 0) || (0 < iVar2)) && (iVar3 = *(int *)(param_1 + 8), iVar2 < iVar3)) {
        iVar2 = iVar2 + 1;
        if (((iVar2 != 0) && (iVar2 < 1)) || (iVar3 <= iVar2)) {
          iVar2 = -1;
        }
      }
      else {
        iVar3 = *(int *)(param_1 + 8);
        iVar2 = (0 < iVar3) - 1;
      }
      if (((iVar2 != 0) && (iVar2 < 1)) || (iVar3 <= iVar2)) {
        Error("CUtlRBTree overflow!\n");
      }
    }
    *(int *)(param_1 + 0x1c) = iVar2;
  }
  else {
    *(undefined4 *)(param_1 + 0x18) = *(undefined4 *)(iVar2 * 0x20 + 4 + *(int *)(param_1 + 4));
  }
  puVar1 = (undefined4 *)(iVar2 * 0x20 + 0x10 + *(int *)(param_1 + 4));
  if (puVar1 != (undefined4 *)0x0) {
    FUN_1000a140(puVar1);
  }
  *(undefined4 *)(param_1 + 0x20) = *(undefined4 *)(param_1 + 4);
  return iVar2;
}


// 100034e0 FUN_100034e0

undefined4 __thiscall FUN_100034e0(void *this,int param_1)

{
  undefined4 *puVar1;
  int *piVar2;
  
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = -1;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (param_1 == -1) {
    puVar1 = &DAT_100274ec;
  }
  else {
    puVar1 = (undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4));
  }
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = -1;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (puVar1[2] != -1) {
    piVar2 = (int *)(puVar1[2] * 0x20 + *(int *)((int)this + 4));
    return CONCAT31((int3)((uint)piVar2 >> 8),*piVar2 == param_1);
  }
  return CONCAT31(0x100274,DAT_100274ec == param_1);
}


// 10003590 FUN_10003590

undefined4 __thiscall FUN_10003590(void *this,int param_1)

{
  undefined4 *puVar1;
  int iVar2;
  
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = -1;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (param_1 == -1) {
    puVar1 = &DAT_100274ec;
  }
  else {
    puVar1 = (undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4));
  }
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = -1;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (puVar1[2] != -1) {
    iVar2 = puVar1[2] * 0x20 + *(int *)((int)this + 4);
    return CONCAT31((int3)((uint)iVar2 >> 8),*(int *)(iVar2 + 4) == param_1);
  }
  return CONCAT31(0x100274,DAT_100274f0 == param_1);
}


// 10003640 FUN_10003640

void __thiscall FUN_10003640(void *this,int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  int *piVar3;
  int iVar4;
  undefined4 uVar5;
  
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (param_1 == -1) {
    puVar2 = &DAT_100274ec;
  }
  else {
    puVar2 = (undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4));
  }
  iVar1 = puVar2[1];
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (iVar1 == -1) {
    puVar2 = &DAT_100274ec;
  }
  else {
    puVar2 = (undefined4 *)(iVar1 * 0x20 + *(int *)((int)this + 4));
  }
  *(undefined4 *)(*(int *)((int)this + 4) + 4 + param_1 * 0x20) = *puVar2;
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (iVar1 == -1) {
    piVar3 = &DAT_100274ec;
  }
  else {
    piVar3 = (int *)(iVar1 * 0x20 + *(int *)((int)this + 4));
  }
  if (*piVar3 != -1) {
    iVar4 = FUN_10003260(this,iVar1);
    *(int *)(iVar4 * 0x20 + 8 + *(int *)((int)this + 4)) = param_1;
  }
  if (iVar1 != -1) {
    uVar5 = FUN_100030a0(this,param_1);
    *(undefined4 *)(iVar1 * 0x20 + 8 + *(int *)((int)this + 4)) = uVar5;
  }
  if (param_1 == *(int *)((int)this + 0x10)) {
    *(int *)((int)this + 0x10) = iVar1;
  }
  else {
    uVar5 = FUN_100034e0(this,param_1);
    if ((char)uVar5 == '\0') {
      iVar4 = FUN_100030a0(this,param_1);
      *(int *)(iVar4 * 0x20 + 4 + *(int *)((int)this + 4)) = iVar1;
    }
    else {
      iVar4 = FUN_100030a0(this,param_1);
      *(int *)(iVar4 * 0x20 + *(int *)((int)this + 4)) = iVar1;
    }
  }
  *(int *)(iVar1 * 0x20 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != -1) {
    *(int *)(*(int *)((int)this + 4) + 8 + param_1 * 0x20) = iVar1;
  }
  return;
}


// 100037b0 FUN_100037b0

void __thiscall FUN_100037b0(void *this,int param_1)

{
  int iVar1;
  int *piVar2;
  undefined4 *puVar3;
  int iVar4;
  undefined4 uVar5;
  
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (param_1 == -1) {
    piVar2 = &DAT_100274ec;
  }
  else {
    piVar2 = (int *)(param_1 * 0x20 + *(int *)((int)this + 4));
  }
  iVar1 = *piVar2;
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (iVar1 == -1) {
    puVar3 = &DAT_100274ec;
  }
  else {
    puVar3 = (undefined4 *)(iVar1 * 0x20 + *(int *)((int)this + 4));
  }
  *(undefined4 *)(*(int *)((int)this + 4) + param_1 * 0x20) = puVar3[1];
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = 0xffffffff;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (iVar1 == -1) {
    puVar3 = &DAT_100274ec;
  }
  else {
    puVar3 = (undefined4 *)(iVar1 * 0x20 + *(int *)((int)this + 4));
  }
  if (puVar3[1] != -1) {
    iVar4 = FUN_100032b0(this,iVar1);
    *(int *)(iVar4 * 0x20 + 8 + *(int *)((int)this + 4)) = param_1;
  }
  if (iVar1 != -1) {
    uVar5 = FUN_100030a0(this,param_1);
    *(undefined4 *)(iVar1 * 0x20 + 8 + *(int *)((int)this + 4)) = uVar5;
  }
  if (param_1 == *(int *)((int)this + 0x10)) {
    *(int *)((int)this + 0x10) = iVar1;
  }
  else {
    uVar5 = FUN_10003590(this,param_1);
    if ((char)uVar5 == '\0') {
      iVar4 = FUN_100030a0(this,param_1);
      *(int *)(iVar4 * 0x20 + *(int *)((int)this + 4)) = iVar1;
    }
    else {
      iVar4 = FUN_100030a0(this,param_1);
      *(int *)(iVar4 * 0x20 + 4 + *(int *)((int)this + 4)) = iVar1;
    }
  }
  *(int *)(iVar1 * 0x20 + 4 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != -1) {
    *(int *)(*(int *)((int)this + 4) + 8 + param_1 * 0x20) = iVar1;
  }
  return;
}


// 10003930 FUN_10003930

void __fastcall FUN_10003930(int *param_1)

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
    uVar2 = FUN_1000a410(param_1,1);
    if ((char)uVar2 != '\0') {
      *(undefined1 *)((*param_1 - param_1[8]) + param_1[4]) = 9;
      param_1[4] = param_1[4] + 1;
      FUN_1000a460(param_1);
    }
  }
  return;
}


// 10003980 FUN_10003980

void __thiscall FUN_10003980(void *this,undefined1 param_1)

{
  undefined4 uVar1;
  
                    /* WARNING: Load size is inaccurate */
  if ((((*(byte *)((int)this + 0x15) & 1) != 0) && (*(int *)((int)this + 0x10) != 0)) &&
     (*(char *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + -1 + *this) == '\n')) {
    FUN_10003930((int *)this);
  }
  uVar1 = FUN_1000a410(this,1);
  if ((char)uVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + *this) = param_1;
    *(int *)((int)this + 0x10) = *(int *)((int)this + 0x10) + 1;
    FUN_1000a460((int *)this);
  }
  return;
}


// 100039e0 FUN_100039e0

int __thiscall FUN_100039e0(void *this,undefined4 param_1)

{
  char cVar1;
  undefined4 *puVar2;
  int iVar3;
  int iVar4;
  bool bVar5;
  
  iVar3 = *(int *)((int)this + 0x10);
  if (iVar3 == -1) {
    return -1;
  }
  do {
                    /* WARNING: Load size is inaccurate */
    iVar4 = iVar3 * 0x20;
    cVar1 = (**this)(param_1,*(int *)((int)this + 4) + 0x10 + iVar4);
    if (cVar1 == '\0') {
                    /* WARNING: Load size is inaccurate */
      cVar1 = (**this)(*(int *)((int)this + 4) + 0x10 + iVar4,param_1);
      if (cVar1 == '\0') {
        return iVar3;
      }
      if ((DAT_100274fc & 1) == 0) {
        DAT_100274fc = DAT_100274fc | 1;
        DAT_100274ec = -1;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
      }
      if (iVar3 == -1) {
        puVar2 = &DAT_100274ec;
      }
      else {
        puVar2 = (undefined4 *)(*(int *)((int)this + 4) + iVar4);
      }
      iVar3 = puVar2[1];
    }
    else {
      if ((DAT_100274fc & 1) == 0) {
        DAT_100274fc = DAT_100274fc | 1;
        DAT_100274ec = -1;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
      }
      bVar5 = iVar3 != -1;
      iVar3 = DAT_100274ec;
      if (bVar5) {
        iVar3 = *(int *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  } while (iVar3 != -1);
  return -1;
}


// 10003ad0 FUN_10003ad0

void __thiscall FUN_10003ad0(void *this,int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 uVar3;
  int *piVar4;
  int iVar5;
  int iVar6;
  
  if (param_1 != *(int *)((int)this + 0x10)) {
    do {
      if ((DAT_100274fc & 1) == 0) {
        DAT_100274fc = DAT_100274fc | 1;
        DAT_100274ec = 0xffffffff;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
      }
      if (param_1 == -1) {
        puVar2 = &DAT_100274ec;
      }
      else {
        puVar2 = (undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4));
      }
      if ((DAT_100274fc & 1) == 0) {
        DAT_100274fc = DAT_100274fc | 1;
        DAT_100274ec = 0xffffffff;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
      }
      if (puVar2[2] == -1) {
        puVar2 = &DAT_100274ec;
      }
      else {
        puVar2 = (undefined4 *)(puVar2[2] * 0x20 + *(int *)((int)this + 4));
      }
      if (puVar2[3] != 0) break;
      if ((DAT_100274fc & 1) == 0) {
        DAT_100274fc = DAT_100274fc | 1;
        DAT_100274ec = 0xffffffff;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
      }
      if (param_1 == -1) {
        puVar2 = &DAT_100274ec;
      }
      else {
        puVar2 = (undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4));
      }
      iVar1 = puVar2[2];
      if ((DAT_100274fc & 1) == 0) {
        DAT_100274fc = DAT_100274fc | 1;
        DAT_100274ec = 0xffffffff;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
      }
      if (iVar1 == -1) {
        puVar2 = &DAT_100274ec;
      }
      else {
        puVar2 = (undefined4 *)(iVar1 * 0x20 + *(int *)((int)this + 4));
      }
      iVar5 = puVar2[2];
      uVar3 = FUN_100034e0(this,iVar1);
      if ((char)uVar3 == '\0') {
        if ((DAT_100274fc & 1) == 0) {
          DAT_100274fc = DAT_100274fc | 1;
          DAT_100274ec = 0xffffffff;
          DAT_100274f0 = 0xffffffff;
          DAT_100274f4 = 0xffffffff;
          DAT_100274f8 = 1;
        }
        if (iVar5 == -1) {
          piVar4 = &DAT_100274ec;
        }
        else {
          piVar4 = (int *)(iVar5 * 0x20 + *(int *)((int)this + 4));
        }
        iVar6 = *piVar4;
        if ((DAT_100274fc & 1) == 0) {
          DAT_100274fc = DAT_100274fc | 1;
          DAT_100274ec = 0xffffffff;
          DAT_100274f0 = 0xffffffff;
          DAT_100274f4 = 0xffffffff;
          DAT_100274f8 = 1;
        }
        if (iVar6 == -1) {
          puVar2 = &DAT_100274ec;
        }
        else {
          puVar2 = (undefined4 *)(iVar6 * 0x20 + *(int *)((int)this + 4));
        }
        if (puVar2[3] == 0) {
          *(undefined4 *)(iVar1 * 0x20 + 0xc + *(int *)((int)this + 4)) = 1;
          *(undefined4 *)(iVar6 * 0x20 + 0xc + *(int *)((int)this + 4)) = 1;
          *(undefined4 *)(iVar5 * 0x20 + 0xc + *(int *)((int)this + 4)) = 0;
          param_1 = iVar5;
        }
        else {
          uVar3 = FUN_100034e0(this,param_1);
          iVar6 = iVar1;
          if ((char)uVar3 != '\0') {
            FUN_100037b0(this,iVar1);
            if ((DAT_100274fc & 1) == 0) {
              DAT_100274fc = DAT_100274fc | 1;
              DAT_100274ec = 0xffffffff;
              DAT_100274f0 = 0xffffffff;
              DAT_100274f4 = 0xffffffff;
              DAT_100274f8 = 1;
            }
            if (iVar1 == -1) {
              puVar2 = &DAT_100274ec;
            }
            else {
              puVar2 = (undefined4 *)(iVar1 * 0x20 + *(int *)((int)this + 4));
            }
            iVar6 = puVar2[2];
            if ((DAT_100274fc & 1) == 0) {
              DAT_100274fc = DAT_100274fc | 1;
              DAT_100274ec = 0xffffffff;
              DAT_100274f0 = 0xffffffff;
              DAT_100274f4 = 0xffffffff;
              DAT_100274f8 = 1;
            }
            if (iVar6 == -1) {
              puVar2 = &DAT_100274ec;
            }
            else {
              puVar2 = (undefined4 *)(iVar6 * 0x20 + *(int *)((int)this + 4));
            }
            iVar5 = puVar2[2];
            param_1 = iVar1;
          }
          *(undefined4 *)(iVar6 * 0x20 + 0xc + *(int *)((int)this + 4)) = 1;
          *(undefined4 *)(iVar5 * 0x20 + 0xc + *(int *)((int)this + 4)) = 0;
          FUN_10003640(this,iVar5);
        }
      }
      else {
        if ((DAT_100274fc & 1) == 0) {
          DAT_100274fc = DAT_100274fc | 1;
          DAT_100274ec = 0xffffffff;
          DAT_100274f0 = 0xffffffff;
          DAT_100274f4 = 0xffffffff;
          DAT_100274f8 = 1;
        }
        if (iVar5 == -1) {
          puVar2 = &DAT_100274ec;
        }
        else {
          puVar2 = (undefined4 *)(iVar5 * 0x20 + *(int *)((int)this + 4));
        }
        iVar6 = puVar2[1];
        if ((DAT_100274fc & 1) == 0) {
          DAT_100274fc = DAT_100274fc | 1;
          DAT_100274ec = 0xffffffff;
          DAT_100274f0 = 0xffffffff;
          DAT_100274f4 = 0xffffffff;
          DAT_100274f8 = 1;
        }
        if (iVar6 == -1) {
          puVar2 = &DAT_100274ec;
        }
        else {
          puVar2 = (undefined4 *)(iVar6 * 0x20 + *(int *)((int)this + 4));
        }
        if (puVar2[3] == 0) {
          *(undefined4 *)(iVar1 * 0x20 + 0xc + *(int *)((int)this + 4)) = 1;
          *(undefined4 *)(iVar6 * 0x20 + 0xc + *(int *)((int)this + 4)) = 1;
          *(undefined4 *)(iVar5 * 0x20 + 0xc + *(int *)((int)this + 4)) = 0;
          param_1 = iVar5;
        }
        else {
          uVar3 = FUN_10003590(this,param_1);
          iVar6 = iVar1;
          if ((char)uVar3 != '\0') {
            FUN_10003640(this,iVar1);
            if ((DAT_100274fc & 1) == 0) {
              DAT_100274fc = DAT_100274fc | 1;
              DAT_100274ec = 0xffffffff;
              DAT_100274f0 = 0xffffffff;
              DAT_100274f4 = 0xffffffff;
              DAT_100274f8 = 1;
            }
            if (iVar1 == -1) {
              puVar2 = &DAT_100274ec;
            }
            else {
              puVar2 = (undefined4 *)(iVar1 * 0x20 + *(int *)((int)this + 4));
            }
            iVar6 = puVar2[2];
            if ((DAT_100274fc & 1) == 0) {
              DAT_100274fc = DAT_100274fc | 1;
              DAT_100274ec = 0xffffffff;
              DAT_100274f0 = 0xffffffff;
              DAT_100274f4 = 0xffffffff;
              DAT_100274f8 = 1;
            }
            if (iVar6 == -1) {
              puVar2 = &DAT_100274ec;
            }
            else {
              puVar2 = (undefined4 *)(iVar6 * 0x20 + *(int *)((int)this + 4));
            }
            iVar5 = puVar2[2];
            param_1 = iVar1;
          }
          *(undefined4 *)(iVar6 * 0x20 + 0xc + *(int *)((int)this + 4)) = 1;
          *(undefined4 *)(iVar5 * 0x20 + 0xc + *(int *)((int)this + 4)) = 0;
          FUN_100037b0(this,iVar5);
        }
      }
    } while (param_1 != *(int *)((int)this + 0x10));
  }
  *(undefined4 *)(*(int *)((int)this + 0x10) * 0x20 + 0xc + *(int *)((int)this + 4)) = 1;
  return;
}


// 10003f00 FUN_10003f00

void __fastcall FUN_10003f00(void *param_1)

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
         ((uVar2 <= uVar1 && (puVar3 = FUN_100030f0(param_1,uVar2), *puVar3 != uVar2)))) {
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


// 10003fd0 FUN_10003fd0

void __fastcall FUN_10003fd0(int param_1)

{
  ushort uVar1;
  ushort uVar2;
  short *psVar3;
  uint uVar4;
  
  uVar4 = DAT_10027508;
  uVar1 = *(ushort *)(param_1 + 0x10);
  while( true ) {
    uVar2 = uVar1;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_10027500 = 0xffff;
      DAT_10027502 = 0xffff;
      DAT_10027504 = 0xffff;
      DAT_10027506 = 1;
      DAT_10027508 = uVar4;
    }
    if (uVar2 == 0xffff) {
      psVar3 = (short *)&DAT_10027500;
    }
    else {
      psVar3 = (short *)((uint)uVar2 * 0x10 + *(int *)(param_1 + 4));
    }
    if (*psVar3 == -1) break;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_10027500 = 0xffff;
      DAT_10027502 = 0xffff;
      DAT_10027504 = 0xffff;
      DAT_10027506 = 1;
      DAT_10027508 = uVar4;
    }
    uVar1 = DAT_10027500;
    if (uVar2 != 0xffff) {
      uVar1 = *(ushort *)((uint)uVar2 * 0x10 + *(int *)(param_1 + 4));
    }
  }
  return;
}


// 10004080 FUN_10004080

void __thiscall FUN_10004080(void *this,ushort param_1)

{
  ushort uVar1;
  ushort uVar2;
  undefined2 *puVar3;
  short *psVar4;
  uint uVar5;
  
  puVar3 = FUN_100030f0(this,param_1);
  if (puVar3[1] == -1) {
    puVar3 = FUN_100030f0(this,param_1);
    uVar5 = DAT_10027508;
    while( true ) {
      uVar1 = puVar3[2];
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
        DAT_10027508 = uVar5;
      }
      if (param_1 == 0xffff) {
        puVar3 = &DAT_10027500;
      }
      else {
        puVar3 = (undefined2 *)((uint)param_1 * 0x10 + *(int *)((int)this + 4));
      }
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
        DAT_10027508 = uVar5;
      }
      if (puVar3[2] == 0xffff) {
        puVar3 = &DAT_10027500;
      }
      else {
        puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if ((puVar3[1] != param_1) || (uVar1 == 0xffff)) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
        DAT_10027508 = uVar5;
      }
      puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      param_1 = uVar1;
    }
  }
  else {
    puVar3 = FUN_100030f0(this,param_1);
    uVar5 = DAT_10027508;
    uVar1 = puVar3[1];
    while( true ) {
      uVar2 = uVar1;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
        DAT_10027508 = uVar5;
      }
      if (uVar2 == 0xffff) {
        psVar4 = (short *)&DAT_10027500;
      }
      else {
        psVar4 = (short *)((uint)uVar2 * 0x10 + *(int *)((int)this + 4));
      }
      if (*psVar4 == -1) break;
      if ((uVar5 & 1) == 0) {
        uVar5 = uVar5 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
        DAT_10027508 = uVar5;
      }
      uVar1 = DAT_10027500;
      if (uVar2 != 0xffff) {
        uVar1 = *(ushort *)((uint)uVar2 * 0x10 + *(int *)((int)this + 4));
      }
    }
  }
  return;
}


// 10004230 FUN_10004230

void __fastcall FUN_10004230(int *param_1)

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


// 10004260 FUN_10004260

void __fastcall FUN_10004260(int *param_1)

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


// 100042d0 FUN_100042d0

void __thiscall FUN_100042d0(void *this,int param_1,int param_2,char param_3)

{
  undefined4 *puVar1;
  
  puVar1 = (undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4));
  puVar1[2] = param_2;
  puVar1[1] = 0xffffffff;
  *puVar1 = 0xffffffff;
  puVar1[3] = 0;
  if (param_2 == -1) {
    *(int *)((int)this + 0x10) = param_1;
    FUN_10003ad0(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(int *)(param_2 * 0x20 + *(int *)((int)this + 4)) = param_1;
    FUN_10003ad0(this,param_1);
    return;
  }
  *(int *)(param_2 * 0x20 + 4 + *(int *)((int)this + 4)) = param_1;
  FUN_10003ad0(this,param_1);
  return;
}


// 10004340 FUN_10004340

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

int FUN_10004340(void)

{
  int *piVar1;
  int iVar2;
  char *pcVar3;
  undefined4 uVar4;
  int iVar5;
  
  if ((_DAT_1002753c & 1) == 0) {
    _DAT_1002753c = _DAT_1002753c | 1;
    FUN_1000aa70(&DAT_1002750c,0,0,1);
    _atexit((_func_4879 *)&LAB_1001ed20);
  }
  _DAT_10027518 = 0;
  DAT_1002751c = 0;
  DAT_10027520 = 0;
  _DAT_1002752c = 0;
  _DAT_10027528 = 0xffffffff;
  FUN_1000a460(&DAT_1002750c);
  iVar5 = 1;
  piVar1 = (int *)CommandLine_Tier0();
  iVar2 = (**(code **)(*piVar1 + 0x24))();
  if (1 < iVar2) {
    do {
      piVar1 = (int *)CommandLine_Tier0();
      pcVar3 = (char *)(**(code **)(*piVar1 + 0x2c))(iVar5);
      if (pcVar3 != (char *)0x0) {
        if (*pcVar3 == '-') {
          piVar1 = (int *)CommandLine_Tier0();
          iVar2 = (**(code **)(*piVar1 + 0x20))(pcVar3,0);
          if (iVar2 != 0) {
            iVar5 = iVar5 + 1;
          }
        }
        else if (*pcVar3 == '+') {
          piVar1 = (int *)CommandLine_Tier0();
          iVar2 = (**(code **)(*piVar1 + 0x20))(pcVar3,0);
          pcVar3 = pcVar3 + 1;
          if (iVar2 == 0) {
LAB_10004442:
            FUN_1000aca0(&DAT_1002750c,pcVar3);
            FUN_10003980(&DAT_1002750c,0x3b);
          }
          else {
            pcVar3 = FUN_10002da0("%s %s;");
            FUN_1000aca0(&DAT_1002750c,pcVar3);
            iVar5 = iVar5 + 1;
          }
        }
        else {
          piVar1 = (int *)CommandLine_Tier0();
          (**(code **)(*piVar1 + 0x2c))(iVar5);
          pcVar3 = FUN_10002df0();
          if (pcVar3 != (char *)0x0) goto LAB_10004442;
        }
      }
      iVar5 = iVar5 + 1;
      piVar1 = (int *)CommandLine_Tier0();
      iVar2 = (**(code **)(*piVar1 + 0x24))();
    } while (iVar5 < iVar2);
  }
  if ((((DAT_10027521 & 1) != 0) && (DAT_1002751c != 0)) &&
     (*(char *)((DAT_1002751c - _DAT_1002752c) + -1 + DAT_1002750c) == '\n')) {
    FUN_10003930(&DAT_1002750c);
  }
  uVar4 = FUN_1000a410(&DAT_1002750c,1);
  if ((char)uVar4 != '\0') {
    *(undefined1 *)((DAT_1002751c - _DAT_1002752c) + DAT_1002750c) = 0;
    DAT_1002751c = DAT_1002751c + 1;
    FUN_1000a460(&DAT_1002750c);
  }
  return DAT_1002750c;
}


// 100044f0 FUN_100044f0

void __fastcall FUN_100044f0(void *param_1)

{
  FUN_10003f00(param_1);
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


// 10004550 FUN_10004550

void __fastcall FUN_10004550(void *param_1)

{
  ushort uVar1;
  
  for (uVar1 = FUN_10003fd0((int)param_1); uVar1 != 0xffff; uVar1 = FUN_10004080(param_1,uVar1)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)((uint)uVar1 * 0x10 + 8 + *(int *)((int)param_1 + 4)));
  }
  FUN_10003f00(param_1);
  return;
}


// 100045a0 FUN_100045a0

int __thiscall FUN_100045a0(void *this,int *param_1)

{
  void *this_00;
  int *piVar1;
  int iVar2;
  void *local_4;
  
  piVar1 = param_1;
  local_4 = this;
  FUN_10003300(this,param_1,(int *)&local_4,(undefined1 *)&param_1);
  iVar2 = FUN_100033f0((int)this);
  FUN_100042d0(this,iVar2,(int)local_4,(char)param_1);
  *(int *)((int)this + 0x14) = *(int *)((int)this + 0x14) + 1;
  this_00 = (void *)(iVar2 * 0x20 + 0x10 + *(int *)((int)this + 4));
  if (this_00 != (void *)0x0) {
    FUN_1000a0f0(this_00,piVar1);
  }
  return iVar2;
}


// 10004600 FUN_10004600

void __fastcall FUN_10004600(void *param_1)

{
  int *piVar1;
  int iVar2;
  int iVar3;
  
  iVar2 = *(int *)((int)param_1 + 0x1c);
  if (iVar2 != -1) {
    iVar3 = (0 < *(int *)((int)param_1 + 8)) - 1;
    while (iVar3 != -1) {
      if ((((iVar3 == 0) || (0 < iVar3)) && (iVar3 < *(int *)((int)param_1 + 8))) &&
         ((iVar3 <= iVar2 && (iVar2 = FUN_10003260(param_1,iVar3), iVar2 != iVar3)))) {
        iVar2 = iVar3 * 0x20;
        piVar1 = (int *)(*(int *)((int)param_1 + 4) + 0x10 + iVar2);
        if (-1 < *(int *)(*(int *)((int)param_1 + 4) + 0x18 + iVar2)) {
          if (*piVar1 != 0) {
            (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar1);
            *piVar1 = 0;
          }
          piVar1[1] = 0;
        }
        *(undefined4 *)(*(int *)((int)param_1 + 4) + 4 + iVar2) =
             *(undefined4 *)((int)param_1 + 0x18);
        *(int *)(*(int *)((int)param_1 + 4) + iVar2) = iVar3;
        *(int *)((int)param_1 + 0x18) = iVar3;
      }
      iVar2 = *(int *)((int)param_1 + 0x1c);
      if (iVar3 == iVar2) break;
      iVar3 = iVar3 + 1;
      if (((iVar3 != 0) && (iVar3 < 1)) || (*(int *)((int)param_1 + 8) <= iVar3)) {
        iVar3 = -1;
      }
    }
    *(undefined4 *)((int)param_1 + 0x10) = 0xffffffff;
    *(undefined4 *)((int)param_1 + 0x14) = 0;
  }
  return;
}


// 100046d0 FUN_100046d0

void __thiscall FUN_100046d0(void *this,char *param_1)

{
  int iVar1;
  char *pcVar2;
  int local_114 [2];
  int local_10c;
  char acStack_104 [260];
  
                    /* WARNING: Load size is inaccurate */
  if (*this != '\0') {
    FUN_1000a0b0(local_114,param_1);
    iVar1 = FUN_100039e0((void *)((int)this + 0x108),local_114);
    if ((-1 < local_10c) && (local_114[0] != 0)) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_114[0]);
    }
    if (iVar1 == -1) {
      FUN_1000a0b0(local_114,param_1);
      FUN_100045a0((void *)((int)this + 0x108),local_114);
      if ((-1 < local_10c) && (local_114[0] != 0)) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_114[0]);
      }
      pcVar2 = FUN_10007780(param_1,&DAT_10025e58);
      if (pcVar2 != (char *)0x0) {
        iVar1 = FUN_100075f0(&DAT_10025e58);
        FUN_10007e20(acStack_104,pcVar2 + iVar1 + 1,0x104);
        FUN_10007700(acStack_104);
        FUN_100079c0(acStack_104,'\\');
        FUN_10003150((int)this);
      }
    }
  }
  return;
}


// 100047f0 FUN_100047f0

void __fastcall FUN_100047f0(void *param_1)

{
  FUN_10004600(param_1);
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


// 10004850 FUN_10004850

void __fastcall FUN_10004850(int param_1)

{
  if (-1 < *(int *)(param_1 + 0x144)) {
    if (*(int *)(param_1 + 0x13c) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x13c));
      *(undefined4 *)(param_1 + 0x13c) = 0;
    }
    *(undefined4 *)(param_1 + 0x140) = 0;
  }
  if (-1 < *(int *)(param_1 + 0x134)) {
    if (*(int *)(param_1 + 300) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 300));
      *(undefined4 *)(param_1 + 300) = 0;
    }
    *(undefined4 *)(param_1 + 0x130) = 0;
  }
  FUN_100047f0((void *)(param_1 + 0x108));
  return;
}


// 100048c0 FUN_100048c0

undefined1 * __fastcall FUN_100048c0(undefined1 *param_1)

{
  *param_1 = 0;
  *(undefined1 **)(param_1 + 0x108) = &LAB_10002740;
  *(undefined4 *)(param_1 + 0x10c) = 0;
  *(undefined4 *)(param_1 + 0x110) = 0;
  *(undefined4 *)(param_1 + 0x114) = 0;
  *(undefined4 *)(param_1 + 0x118) = 0xffffffff;
  *(undefined4 *)(param_1 + 0x11c) = 0;
  *(undefined4 *)(param_1 + 0x120) = 0xffffffff;
  *(undefined4 *)(param_1 + 0x124) = 0xffffffff;
  *(undefined4 *)(param_1 + 0x128) = *(undefined4 *)(param_1 + 0x10c);
  FUN_1000a140((undefined4 *)(param_1 + 300));
  FUN_1000a140((undefined4 *)(param_1 + 0x13c));
  FUN_1000a070(param_1 + 300,"reslists");
  return param_1;
}


// 10004940 FUN_10004940

void __fastcall FUN_10004940(undefined1 *param_1)

{
  int *piVar1;
  int iVar2;
  undefined4 *puVar3;
  char *lpBuffer;
  char acStack_32c [4];
  char *pcStack_328;
  undefined4 uStack_324;
  char acStack_228 [260];
  char acStack_124 [292];
  
  uStack_324 = 0x10004952;
  piVar1 = (int *)CommandLine_Tier0();
  uStack_324 = 0;
  pcStack_328 = "-edit";
  acStack_32c[0] = 'b';
  acStack_32c[1] = 'I';
  acStack_32c[2] = '\0';
  acStack_32c[3] = '\x10';
  iVar2 = (**(code **)(*piVar1 + 0xc))();
  if (iVar2 == 0) {
    acStack_32c[0] = 'l';
    acStack_32c[1] = 'I';
    acStack_32c[2] = '\0';
    acStack_32c[3] = '\x10';
    piVar1 = (int *)CommandLine_Tier0();
    acStack_32c[0] = '\0';
    acStack_32c[1] = '\0';
    acStack_32c[2] = '\0';
    acStack_32c[3] = '\0';
    iVar2 = (**(code **)(*piVar1 + 0xc))();
    if (iVar2 != 0) {
      *param_1 = 1;
      pcStack_328 = (char *)0x0;
      piVar1 = (int *)CommandLine_Tier0();
      iVar2 = (**(code **)(*piVar1 + 0xc))();
      if (iVar2 != 0) {
        FUN_10007e20(acStack_32c,"-makereslists",0x104);
        FUN_10007920(acStack_32c);
        FUN_10007700(acStack_32c);
        FUN_100079c0(acStack_32c,'\\');
        iVar2 = FUN_100075f0(acStack_32c);
        if (0 < iVar2) {
          FUN_1000a070(param_1 + 300,acStack_32c);
        }
      }
      piVar1 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar1 + 0x20))("-game");
      FUN_10007e60(acStack_228,0x104,"%s/%s");
      FUN_100079c0(acStack_228,'\\');
      FUN_10007700(acStack_228);
      puVar3 = (undefined4 *)(param_1 + 0x13c);
      FUN_1000a070(puVar3,acStack_228);
      FUN_10009ff0((undefined4 *)(param_1 + 300));
      FUN_10009ff0(puVar3);
      FUN_10007e60(acStack_124,0x104,"%s\\%s");
      (**(code **)(*DAT_10027684 + 0x48))(acStack_124,&DAT_1001f2bc);
      (**(code **)(*DAT_10027684 + 0x108))(&LAB_100047d0);
      piVar1 = (int *)CommandLine_Tier0();
      iVar2 = (**(code **)(*piVar1 + 0x28))("-startmap");
      if (iVar2 == 0) {
        piVar1 = (int *)CommandLine_Tier0();
        iVar2 = (**(code **)(*piVar1 + 0x28))("-startstage");
        if (iVar2 == 0) {
          FUN_10004600(param_1 + 0x108);
          FUN_10009ff0((undefined4 *)(param_1 + 300));
          FUN_10009ff0(puVar3);
          puVar3 = FUN_10002ff0((undefined4 *)&stack0xfffffcbc,"%s\\%s\\%s");
          (**(code **)(*DAT_10027684 + 0x40))((int)puVar3 + 5,&DAT_1001f2bc);
        }
      }
      lpBuffer = param_1 + 1;
      GetCurrentDirectoryA(0x104,lpBuffer);
      FUN_10007890(lpBuffer,"\\",0x104,1);
      __strlwr(lpBuffer);
    }
  }
  return;
}


// 10004b70 FUN_10004b70

void __fastcall FUN_10004b70(char *param_1)

{
  int *piVar1;
  int iVar2;
  undefined4 *puVar3;
  undefined4 uStack_114;
  
  if (*param_1 != '\0') {
    *param_1 = '\0';
    uStack_114 = 0x10004b8b;
    piVar1 = (int *)CommandLine_Tier0();
    uStack_114 = 0;
    iVar2 = (**(code **)(*piVar1 + 0xc))("-makereslists");
    if (iVar2 != 0) {
      (**(code **)(*DAT_10027684 + 0x10c))(&LAB_100047d0);
    }
    FUN_10009ff0((undefined4 *)(param_1 + 300));
    FUN_10009ff0((undefined4 *)(param_1 + 0x13c));
    puVar3 = FUN_10002ff0(&uStack_114,"%s\\%s\\%s");
    FUN_10006320((int)puVar3 + 5,&DAT_1001f2bc);
    FUN_10009ff0((undefined4 *)(param_1 + 300));
    FUN_10009ff0((undefined4 *)(param_1 + 0x13c));
    puVar3 = FUN_10002ff0(&uStack_114,"%s\\%s\\%s");
    FUN_10006320((int)puVar3 + 5,&DAT_1001f2bc);
    FUN_10004600(param_1 + 0x108);
    param_1[0x120] = -1;
    param_1[0x121] = -1;
    param_1[0x122] = -1;
    param_1[0x123] = -1;
    if (-1 < *(int *)(param_1 + 0x114)) {
      if (*(int *)(param_1 + 0x10c) != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x10c));
        param_1[0x10c] = '\0';
        param_1[0x10d] = '\0';
        param_1[0x10e] = '\0';
        param_1[0x10f] = '\0';
      }
      param_1[0x110] = '\0';
      param_1[0x111] = '\0';
      param_1[0x112] = '\0';
      param_1[0x113] = '\0';
    }
    param_1[0x124] = -1;
    param_1[0x125] = -1;
    param_1[0x126] = -1;
    param_1[0x127] = -1;
  }
  return;
}


// 10004f00 LauncherMain

undefined4 __cdecl LauncherMain(undefined4 param_1)

{
  char cVar1;
  undefined4 uVar2;
  int *piVar3;
  byte *pbVar4;
  int iVar5;
  HWND hWnd;
  char *pcVar6;
  LRESULT LVar7;
  HANDLE hProcess;
  HMODULE hModule;
  FARPROC pFVar8;
  int iVar9;
  LSTATUS LVar10;
  undefined4 uVar11;
  char *pcVar12;
  DWORD dwPriorityClass;
  HKEY pHStack_22c;
  DWORD DStack_228;
  char *pcStack_224;
  int aiStack_200 [5];
  undefined1 auStack_1ec [48];
  undefined **ppuStack_1bc;
  int aiStack_1b8 [5];
  int aiStack_1a4 [2];
  WSADATA WStack_19c;
  
                    /* 0x4f00  2  LauncherMain */
  FUN_1000d7d0(param_1);
  SpewOutputFunc();
  cVar1 = Plat_VerifyHardwareKeyPrompt();
  if (cVar1 == '\0') {
    return 0xffffffff;
  }
  uVar2 = (**(code **)(*(int *)g_pVCR_exref + 0x3c))();
  piVar3 = (int *)CommandLine_Tier0();
  (**(code **)(*piVar3 + 4))();
  FUN_10002600();
  pbVar4 = FUN_100025a0();
  if (*pbVar4 != 0) {
    iVar5 = FUN_10007620(pbVar4,&DAT_1001fb80);
    if (iVar5 != 0) {
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))();
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x14))();
    }
  }
  FUN_10002780();
  piVar3 = (int *)CommandLine_Tier0();
  iVar5 = (**(code **)(*piVar3 + 0xc))();
  if (iVar5 == 0) {
    piVar3 = (int *)CommandLine_Tier0();
    pcStack_224 = (char *)0x10005009;
    iVar5 = (**(code **)(*piVar3 + 0xc))();
    if (iVar5 != 0) {
      DStack_228 = 0x10005024;
      pcStack_224 = (char *)uVar2;
      iVar5 = (*(code *)**(undefined4 **)g_pVCR_exref)();
      if (iVar5 == 0) {
        pcStack_224 = (char *)0x1000503b;
        Error();
        return 0xffffffff;
      }
    }
  }
  else {
    DStack_228 = 0x10004fce;
    pcStack_224 = (char *)uVar2;
    iVar5 = (*(code *)**(undefined4 **)g_pVCR_exref)();
    if (iVar5 == 0) {
      pcStack_224 = (char *)0x10004fe5;
      Error();
      return 0xffffffff;
    }
  }
  FUN_10002cd0();
  pcStack_224 = (char *)0x1000505b;
  iVar5 = WSAStartup(2,&WStack_19c);
  if (iVar5 != 0) {
    pcStack_224 = (char *)0x1000506b;
    Msg();
  }
  piVar3 = (int *)CommandLine_Tier0();
  pcStack_224 = "-textmode";
  DStack_228 = 0x10005081;
  iVar5 = (**(code **)(*piVar3 + 0xc))();
  if (iVar5 == 0) {
    uVar11 = 0xffffffff;
    DStack_228 = 0x100050ba;
    uVar2 = FUN_10002c60();
    if ((char)uVar2 == '\0') {
      DStack_228 = 0x100050c0;
      piVar3 = (int *)CommandLine_Tier0();
      DStack_228 = 0;
      pHStack_22c = (HKEY)0x1001ffe0;
      iVar5 = (**(code **)(*piVar3 + 0xc))();
      if (iVar5 == 0) {
        MessageBoxA((HWND)0x0,"Only one instance of the game can be running at one time.",
                    "Source - Warning",0x40);
        return 0xffffffff;
      }
      hWnd = FindWindowA("Valve001",(LPCSTR)0x0);
      if (hWnd == (HWND)0x0) {
        MessageBoxA((HWND)0x0,
                    "The modified entity keyvalues could not be sent to the Source Engine because the engine does not appear to be running."
                    ,"Source Engine Not Running",0x30);
        return 0xffffffff;
      }
      pcVar6 = (char *)FUN_10004340();
      pcVar12 = pcVar6;
      do {
        cVar1 = *pcVar12;
        pcVar12 = pcVar12 + 1;
      } while (cVar1 != '\0');
      LVar7 = SendMessageA(hWnd,0x4a,0,(LPARAM)&stack0xfffffdec);
      if (LVar7 == 0) {
        MessageBoxA((HWND)0x0,
                    "The Source Engine was found running, but did not accept the request to load a savegame. It may be an old version of the engine that does not support this functionality."
                    ,"Source Engine Declined Request",0x30);
      }
      else {
        uVar11 = 0;
      }
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(pcVar6);
      return uVar11;
    }
  }
  else {
    DAT_10026064 = 1;
    DStack_228 = 0x10005091;
    FUN_100026e0();
  }
  DStack_228 = 0x10005093;
  piVar3 = (int *)CommandLine_Tier0();
  DStack_228 = 0;
  pHStack_22c = (HKEY)&DAT_1001ffe8;
  iVar5 = (**(code **)(*piVar3 + 0xc))();
  if (iVar5 == 0) {
    piVar3 = (int *)CommandLine_Tier0();
    iVar5 = (**(code **)(*piVar3 + 0xc))("-high",0);
    if (iVar5 == 0) goto LAB_100051c1;
    dwPriorityClass = 0x80;
  }
  else {
    dwPriorityClass = 0x40;
  }
  hProcess = GetCurrentProcess();
  SetPriorityClass(hProcess,dwPriorityClass);
LAB_100051c1:
  piVar3 = (int *)CommandLine_Tier0();
  iVar5 = (**(code **)(*piVar3 + 0xc))("-steam",0);
  if (iVar5 == 0) {
    piVar3 = (int *)CommandLine_Tier0();
    (**(code **)(*piVar3 + 0x14))("-insecure",0);
  }
  __chdir(&DAT_10025e58);
  pcVar12 = "SetDllDirectoryA";
  hModule = GetModuleHandleA("kernel32.dll");
  pFVar8 = GetProcAddress(hModule,pcVar12);
  if (pFVar8 != (FARPROC)0x0) {
    (*pFVar8)(&DAT_10025e58);
  }
  piVar3 = (int *)CommandLine_Tier0();
  iVar5 = (**(code **)(*piVar3 + 0xc))("-leakcheck",0);
  DAT_10026084 = iVar5 != 0;
  do {
    FUN_1000d620(&ppuStack_1bc,0);
    ppuStack_1bc = &PTR_LAB_1001fce4;
    FUN_1000d930(&stack0xfffffde8,&ppuStack_1bc);
    iVar5 = FUN_1000d5d0((int *)&stack0xfffffde8);
    *(undefined4 *)g_pPhysicsMiniProfilers_exref = 0;
    *(undefined4 *)g_pOtherMiniProfilers_exref = 0;
    iVar9 = FUN_1000c120((int)&stack0xfffffde8);
    if (iVar9 == 3) {
      cVar1 = iVar5 == 2;
      if ((bool)cVar1) goto LAB_100052b6;
LAB_1000529f:
      cVar1 = (**(code **)(*(int *)PTR_DAT_10024138 + 0x10))();
      if (cVar1 == '\0') goto LAB_100052b6;
    }
    else {
      if (iVar5 != 4) goto LAB_1000529f;
      cVar1 = '\x01';
LAB_100052b6:
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))(&DAT_1001fdd0);
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))(&DAT_1001fdcc);
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))("-width");
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))("-height");
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))(&DAT_1001fdb8);
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))("-startwindowed");
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))("-windowed");
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))("-window");
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))("-full");
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))("-fullscreen");
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))("-autoconfig");
      piVar3 = (int *)CommandLine_Tier0();
      (**(code **)(*piVar3 + 0x10))("+mat_hdr_level");
    }
    FUN_10004550(auStack_1ec);
    FUN_100044f0(auStack_1ec);
    FUN_10004260(aiStack_200);
    FUN_10004260((int *)&stack0xfffffdec);
    FUN_10004550(WStack_19c.szDescription + 8);
    FUN_100044f0(WStack_19c.szDescription + 8);
    FUN_10004260(aiStack_1a4);
    FUN_10004260(aiStack_1b8);
    if (cVar1 == '\0') {
      iVar5 = WSACleanup();
      if (iVar5 != 0) {
        Msg("Warning! Failed to complete WSACleanup = 0x%x.\n",iVar5);
      }
      FUN_10002ca0();
      LVar10 = RegOpenKeyExA((HKEY)0x80000001,"Software\\Valve\\Source",0,0xf003f,&pHStack_22c);
      if (LVar10 == 0) {
        DStack_228 = 0x104;
        LVar10 = RegQueryValueExA(pHStack_22c,"Relaunch URL",(LPDWORD)0x0,(LPDWORD)0x0,
                                  (LPBYTE)&ppuStack_1bc,&DStack_228);
        if (LVar10 == 0) {
          ShellExecuteA((HWND)0x0,"open",(LPCSTR)&ppuStack_1bc,(LPCSTR)0x0,(LPCSTR)0x0,5);
          RegDeleteValueA(pHStack_22c,"Relaunch URL");
        }
        RegCloseKey(pHStack_22c);
      }
      return 0;
    }
  } while( true );
}


// 10005490 FUN_10005490

char * FUN_10005490(void)

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
    VirtualQuery(&DAT_1002754c,&_Stack_1c,0x1c);
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


// 10005520 FUN_10005520

void __cdecl FUN_10005520(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1);
  return;
}


// 10005540 FUN_10005540

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_10005540(int param_1,int param_2)

{
  void *pvVar1;
  size_t _Size;
  
  _Size = param_1 * param_2;
  if ((_DAT_10027554 & 1) == 0) {
    _DAT_10027554 = _DAT_10027554 | 1;
    DAT_10027550 = FUN_10005490();
  }
  if (DAT_10027550 != (char *)0x0) {
    pvVar1 = (void *)(**(code **)**(undefined4 **)g_pMemAlloc_exref)(_Size,DAT_10027550,0);
    _memset(pvVar1,0,_Size);
    return pvVar1;
  }
  pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(_Size);
  _memset(pvVar1,0,_Size);
  return pvVar1;
}


// 100055c0 FUN_100055c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_100055c0(size_t param_1)

{
  void *pvVar1;
  
  if ((_DAT_10027554 & 1) == 0) {
    _DAT_10027554 = _DAT_10027554 | 1;
    DAT_10027550 = FUN_10005490();
  }
  if (DAT_10027550 != (char *)0x0) {
    pvVar1 = (void *)(**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_10027550,0);
    _memset(pvVar1,0,param_1);
    return pvVar1;
  }
  pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  _memset(pvVar1,0,param_1);
  return pvVar1;
}


// 10005640 FUN_10005640

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10005640(undefined4 param_1,undefined4 param_2)

{
  if ((_DAT_1002755c & 1) == 0) {
    _DAT_1002755c = _DAT_1002755c | 1;
    DAT_10027558 = FUN_10005490();
  }
  if (DAT_10027558 != (char *)0x0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 8))(param_1,param_2,DAT_10027558,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(param_1,param_2);
  return;
}


// 100056a0 FUN_100056a0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_100056a0(undefined4 param_1,size_t param_2)

{
  void *pvVar1;
  
  if ((_DAT_1002755c & 1) == 0) {
    _DAT_1002755c = _DAT_1002755c | 1;
    DAT_10027558 = FUN_10005490();
  }
  if (DAT_10027558 != (char *)0x0) {
    pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 8))(param_1,param_2,DAT_10027558,0);
    _memset(pvVar1,0,param_2);
    return pvVar1;
  }
  pvVar1 = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 0xc))(param_1,param_2);
  _memset(pvVar1,0,param_2);
  return pvVar1;
}


// 10005720 FUN_10005720

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10005720(undefined4 param_1)

{
  if ((_DAT_10027554 & 1) == 0) {
    _DAT_10027554 = _DAT_10027554 | 1;
    DAT_10027550 = FUN_10005490();
  }
  if (DAT_10027550 != (char *)0x0) {
    (**(code **)**(undefined4 **)g_pMemAlloc_exref)(param_1,DAT_10027550,0);
    return;
  }
  (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  return;
}


// 10005780 FUN_10005780

void __cdecl FUN_10005780(int param_1,int param_2)

{
  FUN_100055c0(param_1 * param_2);
  return;
}


// 100057b0 FUN_100057b0

void __cdecl FUN_100057b0(undefined4 param_1,int param_2,int param_3)

{
  FUN_100056a0(param_1,param_2 * param_3);
  return;
}


// 100057d0 FUN_100057d0

void __cdecl FUN_100057d0(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x20))(param_1);
  return;
}


// 100057f0 FUN_100057f0

bool FUN_100057f0(void)

{
  return *(int *)g_pMemAlloc_exref != 0;
}


// 10005800 FUN_10005800

void FUN_10005800(void)

{
  return;
}


// 10005810 FUN_10005810

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void * __cdecl FUN_10005810(char *param_1)

{
  char cVar1;
  char *pcVar2;
  void *_Dst;
  
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  pcVar2 = pcVar2 + (1 - (int)(param_1 + 1));
  if ((_DAT_10027554 & 1) == 0) {
    _DAT_10027554 = _DAT_10027554 | 1;
    DAT_10027550 = FUN_10005490();
  }
  if (DAT_10027550 == (char *)0x0) {
    _Dst = (void *)(**(code **)(**(int **)g_pMemAlloc_exref + 4))(pcVar2);
  }
  else {
    _Dst = (void *)(**(code **)**(undefined4 **)g_pMemAlloc_exref)(pcVar2,DAT_10027550,0);
  }
  if (_Dst != (void *)0x0) {
    _memcpy(_Dst,param_1,(size_t)pcVar2);
  }
  return _Dst;
}


// 10005890 FUN_10005890

undefined4 FUN_10005890(void)

{
  return 0;
}


// 100058e0 FUN_100058e0

void __thiscall FUN_100058e0(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000593b;
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
LAB_1000593b:
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


// 10005970 FUN_10005970

void __fastcall FUN_10005970(int param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  uint uVar4;
  
  iVar1 = *(int *)(param_1 + 0x10);
  uVar4 = DAT_100274fc;
  while( true ) {
    iVar2 = iVar1;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_100274ec = -1;
      DAT_100274f0 = 0xffffffff;
      DAT_100274f4 = 0xffffffff;
      DAT_100274f8 = 1;
      DAT_100274fc = uVar4;
    }
    if (iVar2 == -1) {
      piVar3 = &DAT_100274ec;
    }
    else {
      piVar3 = (int *)(iVar2 * 0x20 + *(int *)(param_1 + 4));
    }
    if (*piVar3 == -1) break;
    if ((uVar4 & 1) == 0) {
      uVar4 = uVar4 | 1;
      DAT_100274ec = -1;
      DAT_100274f0 = 0xffffffff;
      DAT_100274f4 = 0xffffffff;
      DAT_100274f8 = 1;
      DAT_100274fc = uVar4;
    }
    iVar1 = DAT_100274ec;
    if (iVar2 != -1) {
      iVar1 = *(int *)(iVar2 * 0x20 + *(int *)(param_1 + 4));
    }
  }
  return;
}


// 10005a00 FUN_10005a00

int __thiscall FUN_10005a00(void *this,int param_1)

{
  char cVar1;
  undefined4 *puVar2;
  int iVar3;
  int iVar4;
  undefined4 uVar5;
  int *piVar6;
  uint uVar7;
  
  if ((DAT_100274fc & 1) == 0) {
    DAT_100274fc = DAT_100274fc | 1;
    DAT_100274ec = -1;
    DAT_100274f0 = 0xffffffff;
    DAT_100274f4 = 0xffffffff;
    DAT_100274f8 = 1;
  }
  if (param_1 == -1) {
    puVar2 = &DAT_100274ec;
  }
  else {
    puVar2 = (undefined4 *)(param_1 * 0x20 + *(int *)((int)this + 4));
  }
  if (puVar2[1] == -1) {
    iVar4 = FUN_100030a0(this,param_1);
    uVar5 = FUN_10003590(this,param_1);
    cVar1 = (char)uVar5;
    while ((cVar1 != '\0' && (iVar4 != -1))) {
      if ((DAT_100274fc & 1) == 0) {
        DAT_100274fc = DAT_100274fc | 1;
        DAT_100274ec = -1;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
      }
      iVar3 = *(int *)(iVar4 * 0x20 + *(int *)((int)this + 4) + 8);
      uVar5 = FUN_10003590(this,iVar4);
      cVar1 = (char)uVar5;
      iVar4 = iVar3;
    }
  }
  else {
    iVar3 = FUN_100032b0(this,param_1);
    uVar7 = DAT_100274fc;
    while( true ) {
      iVar4 = iVar3;
      if ((uVar7 & 1) == 0) {
        uVar7 = uVar7 | 1;
        DAT_100274ec = -1;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
        DAT_100274fc = uVar7;
      }
      if (iVar4 == -1) {
        piVar6 = &DAT_100274ec;
      }
      else {
        piVar6 = (int *)(iVar4 * 0x20 + *(int *)((int)this + 4));
      }
      if (*piVar6 == -1) break;
      if ((uVar7 & 1) == 0) {
        uVar7 = uVar7 | 1;
        DAT_100274ec = -1;
        DAT_100274f0 = 0xffffffff;
        DAT_100274f4 = 0xffffffff;
        DAT_100274f8 = 1;
        DAT_100274fc = uVar7;
      }
      iVar3 = DAT_100274ec;
      if (iVar4 != -1) {
        iVar3 = *(int *)(iVar4 * 0x20 + *(int *)((int)this + 4));
      }
    }
  }
  return iVar4;
}


// 10005b50 FUN_10005b50

int __thiscall FUN_10005b50(void *this,int param_1,int *param_2)

{
  int iVar1;
  void *pvVar2;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_100058e0(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar2 = (void *)(*this + param_1 * 0x10);
    FUN_10007df0((void *)((int)pvVar2 + 0x10),pvVar2,iVar1 * 0x10);
  }
                    /* WARNING: Load size is inaccurate */
  pvVar2 = (void *)(param_1 * 0x10 + *this);
  if (pvVar2 != (void *)0x0) {
    FUN_1000a0f0(pvVar2,param_2);
  }
  return param_1;
}


// 10005bc0 FUN_10005bc0

undefined4 __cdecl FUN_10005bc0(undefined4 param_1,undefined4 param_2,undefined4 param_3)

{
  int iVar1;
  int iVar2;
  int iVar3;
  char *pcVar4;
  undefined1 *puVar5;
  int iVar6;
  undefined4 uVar7;
  int iVar8;
  void *pvStack_8;
  
  iVar2 = (**(code **)(*(int *)(DAT_10027684 + 4) + 8))(param_2,&DAT_100200b0,param_3);
  if (iVar2 != 0) {
    for (iVar3 = FUN_10005970((int)pvStack_8); iVar3 != -1; iVar3 = FUN_10005a00(pvStack_8,iVar3)) {
      pcVar4 = FUN_10009ff0((undefined4 *)(*(int *)((int)pvStack_8 + 4) + 0x10 + iVar3 * 0x20));
      puVar5 = FUN_10009ff0((undefined4 *)(*(int *)((int)pvStack_8 + 4) + 0x10 + iVar3 * 0x20));
      iVar1 = *(int *)(DAT_10027684 + 4);
      iVar8 = iVar2;
      iVar6 = FUN_100075f0(pcVar4);
      (**(code **)(iVar1 + 4))(puVar5,iVar6,iVar8);
      (**(code **)(*(int *)(DAT_10027684 + 4) + 4))(&DAT_100200ac,1,iVar2);
    }
    uVar7 = (**(code **)(*(int *)(DAT_10027684 + 4) + 0xc))(iVar2);
    return CONCAT31((int3)((uint)uVar7 >> 8),1);
  }
  return 0;
}


// 10005ca0 FUN_10005ca0

void __fastcall FUN_10005ca0(int *param_1)

{
  if (-1 < param_1[6]) {
    if (param_1[4] != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1[4]);
      param_1[4] = 0;
    }
    param_1[5] = 0;
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


// 10005d00 FUN_10005d00

void FUN_10005d00(void)

{
  char cVar1;
  int iVar2;
  int iVar3;
  void *pvVar4;
  undefined *puVar5;
  int iVar6;
  int iVar7;
  int iStack_344;
  undefined1 local_334 [23];
  char acStack_31d [261];
  undefined1 auStack_218 [24];
  char local_200 [496];
  int *piStack_10;
  
  FUN_10007e60(local_200,0x200,"%s/%s");
  FUN_1000aa70(local_334,0,0,1);
  iVar7 = 0;
  iVar6 = 0;
  puVar5 = &DAT_1001f2bc;
  cVar1 = (**(code **)(*(int *)(DAT_10027684 + 4) + 0x38))(local_200);
  if (cVar1 == '\0') {
    Error("Unable to maplist file %s\n",auStack_218);
  }
  else {
    FUN_1000a900(&stack0xfffffcb4,acStack_31d + 1,0x104);
    while (acStack_31d[1] != '\0') {
      while (((iVar2 = FUN_100075f0(acStack_31d + 1), 0 < iVar2 && (acStack_31d[iVar2] == '\n')) ||
             (acStack_31d[iVar2] == '\r'))) {
        acStack_31d[iVar2] = '\0';
      }
      FUN_1000a140((undefined4 *)&stack0xfffffca4);
      FUN_1000a070(&stack0xfffffca4,acStack_31d + 1);
      iVar2 = piStack_10[3];
      if (piStack_10[1] < iVar2 + 1) {
        FUN_100058e0(piStack_10,(iVar2 - piStack_10[1]) + 1);
      }
      piStack_10[3] = piStack_10[3] + 1;
      iVar3 = (piStack_10[3] - iVar2) + -1;
      piStack_10[4] = *piStack_10;
      if (0 < iVar3) {
        pvVar4 = (void *)(*piStack_10 + iVar2 * 0x10);
        FUN_10007df0((void *)((int)pvVar4 + 0x10),pvVar4,iVar3 * 0x10);
      }
      pvVar4 = (void *)(iVar2 * 0x10 + *piStack_10);
      if (pvVar4 != (void *)0x0) {
        FUN_1000a0f0(pvVar4,(int *)&stack0xfffffca4);
      }
      if ((-1 < iVar6) && (puVar5 != (undefined *)0x0)) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(puVar5);
        puVar5 = (undefined *)0x0;
      }
      FUN_1000a900(&stack0xfffffcb4,acStack_31d + 1,0x104);
    }
  }
  if ((-1 < iStack_344) && (iVar7 != 0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar7);
  }
  return;
}


// 10005ec0 FUN_10005ec0

void __fastcall FUN_10005ec0(int *param_1)

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


// 10005f20 FUN_10005f20

int __thiscall FUN_10005f20(void *this,int param_1,int *param_2)

{
  int iVar1;
  void *pvVar2;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_10002f20(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar1 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar1) {
    pvVar2 = (void *)(*this + param_1 * 0x20);
    FUN_10007df0((void *)((int)pvVar2 + 0x20),pvVar2,iVar1 * 0x20);
  }
                    /* WARNING: Load size is inaccurate */
  pvVar2 = (void *)(param_1 * 0x20 + *this);
  if (pvVar2 != (void *)0x0) {
    FUN_1000a0f0(pvVar2,param_2);
    FUN_1000a0f0((void *)((int)pvVar2 + 0x10),param_2 + 4);
  }
  return param_1;
}


// 10005fa0 FUN_10005fa0

void __fastcall FUN_10005fa0(int *param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  
  iVar1 = param_1[3] + -1;
  if (-1 < iVar1) {
    iVar2 = iVar1 * 0x20;
    do {
      piVar3 = (int *)(*param_1 + iVar2);
      if (-1 < piVar3[6]) {
        if (piVar3[4] != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(piVar3[4]);
          piVar3[4] = 0;
        }
        piVar3[5] = 0;
      }
      if (-1 < piVar3[2]) {
        if (*piVar3 != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*piVar3);
          *piVar3 = 0;
        }
        piVar3[1] = 0;
      }
      iVar1 = iVar1 + -1;
      iVar2 = iVar2 + -0x20;
    } while (-1 < iVar1);
    param_1[3] = 0;
    return;
  }
  param_1[3] = 0;
  return;
}


// 10006030 FUN_10006030

void __cdecl FUN_10006030(undefined4 param_1,undefined4 param_2,undefined4 param_3)

{
  void *this;
  char cVar1;
  int iVar2;
  char *pcVar3;
  undefined4 *puVar4;
  int unaff_EBX;
  undefined4 uStack_370;
  undefined1 *puStack_36c;
  int iVar5;
  int aiStack_358 [2];
  int iStack_350;
  undefined1 local_340 [24];
  char acStack_328 [260];
  undefined1 auStack_224 [256];
  undefined4 auStack_124 [68];
  void *pvStack_14;
  
  puStack_36c = (undefined1 *)0x10006046;
  FUN_1000aa70(local_340,0,0,1);
  iVar5 = 0;
  puStack_36c = local_340;
  uStack_370 = param_3;
  cVar1 = (**(code **)(*(int *)(DAT_10027684 + 4) + 0x38))(param_2);
  if (cVar1 != '\0') {
    FUN_1000bf80(auStack_224,"");
    iVar2 = FUN_1000b3c0(aiStack_358,(int)auStack_224,acStack_328,0x104,'\x01');
    while (0 < iVar2) {
      FUN_10007700(acStack_328);
      FUN_100079c0(acStack_328,'\\');
      FUN_1000a140(&puStack_36c);
      if (acStack_328[0] == '\"') {
        pcVar3 = acStack_328;
      }
      else {
        puVar4 = FUN_10002ff0(auStack_124,"\"%s\"");
        pcVar3 = (char *)((int)puVar4 + 5);
      }
      FUN_1000a070(&puStack_36c,pcVar3);
      iVar2 = FUN_100039e0(pvStack_14,&puStack_36c);
      if (iVar2 == -1) {
        FUN_10003300(pvStack_14,&puStack_36c,(int *)&stack0xfffffca4,
                     (undefined1 *)((int)&uStack_370 + 3));
        iVar2 = FUN_100033f0((int)pvStack_14);
        puVar4 = (undefined4 *)(*(int *)((int)pvStack_14 + 4) + iVar2 * 0x20);
        puVar4[2] = unaff_EBX;
        puVar4[1] = 0xffffffff;
        *puVar4 = 0xffffffff;
        puVar4[3] = 0;
        if (unaff_EBX == -1) {
          *(int *)((int)pvStack_14 + 0x10) = iVar2;
        }
        else if (uStack_370._3_1_ == '\0') {
          *(int *)(unaff_EBX * 0x20 + 4 + *(int *)((int)pvStack_14 + 4)) = iVar2;
        }
        else {
          *(int *)(unaff_EBX * 0x20 + *(int *)((int)pvStack_14 + 4)) = iVar2;
        }
        FUN_10003ad0(pvStack_14,iVar2);
        *(int *)((int)pvStack_14 + 0x14) = *(int *)((int)pvStack_14 + 0x14) + 1;
        this = (void *)(iVar2 * 0x20 + 0x10 + *(int *)((int)pvStack_14 + 4));
        if (this != (void *)0x0) {
          FUN_1000a0f0(this,(int *)&puStack_36c);
        }
      }
      if ((-1 < iVar5) && (puStack_36c != (undefined1 *)0x0)) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(puStack_36c);
        puStack_36c = (undefined1 *)0x0;
      }
      iVar2 = FUN_1000b3c0(aiStack_358,(int)auStack_224,acStack_328,0x104,'\x01');
    }
  }
  if ((-1 < iStack_350) && (aiStack_358[0] != 0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(aiStack_358[0]);
  }
  return;
}


// 10006220 FUN_10006220

void __fastcall FUN_10006220(int *param_1)

{
  FUN_10005ec0(param_1);
  if (-1 < param_1[2]) {
    if (*param_1 != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*param_1);
      *param_1 = 0;
    }
    param_1[1] = 0;
    param_1[4] = *param_1;
    return;
  }
  param_1[4] = *param_1;
  return;
}


// 10006260 FUN_10006260

void __fastcall FUN_10006260(int *param_1)

{
  int iVar1;
  
  FUN_10005ec0(param_1);
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


// 100062c0 FUN_100062c0

void __fastcall FUN_100062c0(int *param_1)

{
  int iVar1;
  
  FUN_10005fa0(param_1);
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


// 10006320 FUN_10006320

void __cdecl FUN_10006320(undefined4 param_1,undefined4 param_2)

{
  undefined1 *local_24;
  undefined4 local_20;
  undefined4 local_1c;
  undefined4 local_18;
  undefined4 local_14;
  undefined4 local_10;
  undefined4 local_c;
  undefined4 local_8;
  undefined4 local_4;
  
  local_20 = 0;
  local_1c = 0;
  local_18 = 0;
  local_10 = 0;
  local_4 = 0;
  local_24 = &LAB_10002740;
  local_14 = 0xffffffff;
  local_c = 0xffffffff;
  local_8 = 0xffffffff;
  FUN_10006030(&local_24,param_1,param_2);
  FUN_10005bc0(&local_24,param_1,param_2);
  FUN_100047f0(&local_24);
  return;
}


// 10006390 FUN_10006390

/* WARNING: Removing unreachable block (ram,0x1000643b) */

void __cdecl FUN_10006390(int *param_1,undefined4 param_2,undefined4 param_3)

{
  undefined1 *puVar1;
  int iVar2;
  int iVar3;
  undefined4 uVar4;
  undefined1 *local_24;
  int local_20;
  undefined4 local_1c;
  int local_18;
  undefined4 local_14;
  undefined4 local_10;
  undefined4 local_c;
  undefined4 local_8;
  undefined4 local_4;
  
  iVar3 = 0;
  local_24 = &LAB_10002740;
  local_20 = 0;
  local_1c = 0;
  local_18 = 0;
  local_14 = 0xffffffff;
  local_10 = 0;
  local_c = 0xffffffff;
  local_8 = 0xffffffff;
  local_4 = 0;
  if (0 < param_1[3]) {
    iVar2 = 0;
    do {
      uVar4 = param_3;
      puVar1 = FUN_10009ff0((undefined4 *)(*param_1 + iVar2));
      FUN_10006030(&local_24,puVar1,uVar4);
      iVar3 = iVar3 + 1;
      iVar2 = iVar2 + 0x10;
    } while (iVar3 < param_1[3]);
  }
  FUN_10005bc0(&local_24,param_2,param_3);
  FUN_10004600(&local_24);
  if ((-1 < local_18) && (local_20 != 0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_20);
  }
  return;
}


// 10006450 FUN_10006450

void __fastcall FUN_10006450(int param_1)

{
  FUN_10006260((int *)(param_1 + 0xa0));
  FUN_100062c0((int *)(param_1 + 0x8c));
  if (-1 < *(int *)(param_1 + 0x80)) {
    if (*(int *)(param_1 + 0x78) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x78));
      *(undefined4 *)(param_1 + 0x78) = 0;
    }
    *(undefined4 *)(param_1 + 0x7c) = 0;
  }
  if (-1 < *(int *)(param_1 + 0x70)) {
    if (*(int *)(param_1 + 0x68) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x68));
      *(undefined4 *)(param_1 + 0x68) = 0;
    }
    *(undefined4 *)(param_1 + 0x6c) = 0;
  }
  if (-1 < *(int *)(param_1 + 0x60)) {
    if (*(int *)(param_1 + 0x58) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x58));
      *(undefined4 *)(param_1 + 0x58) = 0;
    }
    *(undefined4 *)(param_1 + 0x5c) = 0;
  }
  if (-1 < *(int *)(param_1 + 0x50)) {
    if (*(int *)(param_1 + 0x48) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x48));
      *(undefined4 *)(param_1 + 0x48) = 0;
    }
    *(undefined4 *)(param_1 + 0x4c) = 0;
  }
  if (-1 < *(int *)(param_1 + 0x40)) {
    if (*(int *)(param_1 + 0x38) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 0x38));
      *(undefined4 *)(param_1 + 0x38) = 0;
    }
    *(undefined4 *)(param_1 + 0x3c) = 0;
  }
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
  if (-1 < *(int *)(param_1 + 0x10)) {
    if (*(int *)(param_1 + 8) != 0) {
      (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(*(int *)(param_1 + 8));
      *(undefined4 *)(param_1 + 8) = 0;
    }
    *(undefined4 *)(param_1 + 0xc) = 0;
  }
  return;
}


// 10006590 FUN_10006590

undefined4 * __fastcall FUN_10006590(undefined4 *param_1)

{
  *param_1 = &PTR_LAB_100200e4;
  *(undefined1 *)(param_1 + 1) = 0;
  *(undefined1 *)((int)param_1 + 5) = 0;
  *(undefined1 *)((int)param_1 + 6) = 0;
  FUN_1000a140(param_1 + 2);
  FUN_1000a140(param_1 + 6);
  FUN_1000a140(param_1 + 10);
  FUN_1000a140(param_1 + 0xe);
  FUN_1000a140(param_1 + 0x12);
  FUN_1000a140(param_1 + 0x16);
  FUN_1000a140(param_1 + 0x1a);
  FUN_1000a140(param_1 + 0x1e);
  param_1[0x22] = 0;
  param_1[0x23] = 0;
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
  FUN_1000a070(param_1 + 0xe,"reslists");
  FUN_1000a070(param_1 + 0x12,"reslists_work");
  return param_1;
}


// 10006660 FUN_10006660

void __fastcall FUN_10006660(int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  void *pvVar3;
  int iVar4;
  int iVar5;
  int iVar6;
  int local_134;
  int local_130;
  int local_12c;
  int local_128;
  int local_124;
  int local_120;
  int local_11c [4];
  char local_10c [268];
  
  local_134 = 0;
  local_130 = 0;
  local_12c = 0;
  local_128 = 0;
  local_124 = 0;
  local_120 = 0;
  if (0 < *(int *)(param_1 + 0x98)) {
    iVar4 = 0;
    iVar5 = 0;
    do {
      FUN_10009ff0((undefined4 *)(*(int *)(param_1 + 0x8c) + iVar4));
      FUN_10009ff0((undefined4 *)(param_1 + 0x48));
      FUN_10009ff0((undefined4 *)(param_1 + 0x28));
      FUN_10007e60(local_10c,0x104,"%s\\%s\\%s\\%s");
      FUN_1000a0b0(local_11c,local_10c);
      iVar6 = iVar5;
      if (local_130 < iVar5 + 1) {
        FUN_100058e0(&local_134,(iVar5 - local_130) + 1);
        iVar6 = local_128;
      }
      iVar6 = iVar6 + 1;
      iVar1 = (iVar6 - iVar5) + -1;
      local_124 = local_134;
      local_128 = iVar6;
      if (0 < iVar1) {
        pvVar3 = (void *)(local_134 + iVar5 * 0x10);
        FUN_10007df0((void *)((int)pvVar3 + 0x10),pvVar3,iVar1 * 0x10);
      }
      pvVar3 = (void *)(iVar5 * 0x10 + local_134);
      if (pvVar3 != (void *)0x0) {
        FUN_1000a0f0(pvVar3,local_11c);
      }
      if (-1 < local_11c[2]) {
        if (local_11c[0] != 0) {
          (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_11c[0]);
          local_11c[0] = 0;
        }
        local_11c[1] = 0;
      }
      local_120 = local_120 + 1;
      iVar4 = iVar4 + 0x20;
      iVar5 = iVar6;
    } while (local_120 < *(int *)(param_1 + 0x98));
  }
  FUN_10009ff0((undefined4 *)(param_1 + 0x38));
  FUN_10009ff0((undefined4 *)(param_1 + 0x28));
  puVar2 = FUN_10002ff0((undefined4 *)local_10c,"%s\\%s\\%s");
  FUN_10006390(&local_134,(int)puVar2 + 5,&DAT_1001f2bc);
  FUN_10005ec0(&local_134);
  if ((-1 < local_12c) && (local_134 != 0)) {
                    /* WARNING: Could not recover jumptable at 0x10006816. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))();
    return;
  }
  return;
}


// 10006830 FUN_10006830

void __fastcall FUN_10006830(int param_1)

{
  int iVar1;
  int iVar2;
  char local_210 [252];
  undefined4 auStack_114 [69];
  
  FUN_10009ff0((undefined4 *)(param_1 + 0x38));
  FUN_10009ff0((undefined4 *)(param_1 + 0x28));
  FUN_10007e60(local_210,0x104,"%s\\%s");
  (**(code **)(*DAT_10027684 + 0x48))(local_210,&DAT_1001f2bc);
  FUN_10006660(param_1);
  FUN_10006660(param_1);
  iVar2 = 0;
  if (0 < *(int *)(param_1 + 0xac)) {
    iVar1 = 0;
    do {
      FUN_10009ff0((undefined4 *)(*(int *)(param_1 + 0xa0) + iVar1));
      FUN_10002ff0(auStack_114,"%s.lst");
      FUN_10006660(param_1);
      iVar2 = iVar2 + 1;
      iVar1 = iVar1 + 0x10;
    } while (iVar2 < *(int *)(param_1 + 0xac));
  }
  return;
}


// 10006bc0 FUN_10006bc0

undefined1 __thiscall FUN_10006bc0(void *this,undefined4 param_1,char *param_2)

{
  undefined1 uVar1;
  uint uVar2;
  char cVar3;
  void *this_00;
  undefined4 *this_01;
  uint uVar4;
  char *pcVar5;
  int *piVar6;
  int iVar7;
  undefined4 uVar8;
  undefined1 *puVar9;
  char *pcStack_2ac;
  char *pcVar10;
  int iStack_27c;
  int aiStack_278 [12];
  char *pcStack_248;
  undefined1 auStack_244 [20];
  undefined1 local_230 [48];
  char local_200 [512];
  
  if ((*param_2 == '+') || (*param_2 == '-')) {
    Msg();
    return 0;
  }
  pcStack_2ac = (char *)0x10006c02;
  FUN_10007e60(local_200,0x200,"%s/%s");
  FUN_1000aa70(local_230,0,0,0);
  pcVar10 = "GAME";
  pcStack_2ac = local_200;
  cVar3 = (**(code **)(*(int *)(DAT_10027684 + 4) + 0x38))();
  if (cVar3 == '\0') {
    Error();
    FUN_10004230((int *)&pcStack_248);
    return 0;
  }
  this_00 = (void *)FUN_10008840(0x24);
  if (this_00 == (void *)0x0) {
    this_01 = (undefined4 *)0x0;
  }
  else {
    this_01 = FUN_10008860(this_00,"reslists");
  }
  uVar4 = FUN_10009ab0(this_01,"reslists",pcStack_248,(int *)0x0,(void *)0x0);
  if ((char)uVar4 == '\0') {
    Error();
    FUN_10008cb0((int)this_01);
    FUN_10004230((int *)&pcStack_248);
    return 0;
  }
  pcVar5 = (char *)FUN_10008e20(this_01,"maplist",0x100202c4);
  FUN_1000a0b0(aiStack_278,pcVar5);
  FUN_10009ff0(aiStack_278);
  FUN_10005d00();
  if (*(int *)((int)this + 0xac) < 1) {
    FUN_10009ff0(aiStack_278);
    Error();
    FUN_10008cb0((int)this_01);
    FUN_10004230(aiStack_278);
    FUN_10004230((int *)&pcStack_248);
    return 0;
  }
  piVar6 = (int *)CommandLine_Tier0();
  pcVar5 = "+map";
  iVar7 = (**(code **)(*piVar6 + 0xc))();
  if ((iVar7 != 0) && (pcVar10 != (char *)0x0)) {
    FUN_10006220((int *)((int)this + 0xa0));
    FUN_1000a140((undefined4 *)&stack0xfffffd60);
    FUN_1000a070(&stack0xfffffd60,pcVar10);
    FUN_10005b50((int *)((int)this + 0xa0),*(int *)((int)this + 0xac),(int *)&stack0xfffffd60);
    FUN_10004230((int *)&stack0xfffffd60);
  }
  piVar6 = (int *)CommandLine_Tier0();
  uVar8 = (**(code **)(*piVar6 + 0x1c))();
  *(undefined4 *)((int)this + 0x88) = uVar8;
  piVar6 = (int *)CommandLine_Tier0();
  (**(code **)(*piVar6 + 0xc))("-startmap",&stack0xfffffd4c);
  if (pcVar5 != (char *)0x0) {
    FUN_1000a070((void *)((int)this + 0x78),pcVar5);
  }
  piVar6 = (int *)CommandLine_Tier0();
  pcVar5 = "-startstage";
  (**(code **)(*piVar6 + 0x10))();
  piVar6 = (int *)CommandLine_Tier0();
  (**(code **)(*piVar6 + 0x10))("-makereslists");
  piVar6 = (int *)CommandLine_Tier0();
  (**(code **)(*piVar6 + 0x10))("-reslistdir");
  piVar6 = (int *)CommandLine_Tier0();
  (**(code **)(*piVar6 + 0x10))("-norebuildaudio");
  piVar6 = (int *)CommandLine_Tier0();
  (**(code **)(*piVar6 + 0x10))("-startmap");
  piVar6 = (int *)CommandLine_Tier0();
  pcVar10 = (char *)(**(code **)(*piVar6 + 8))();
  FUN_1000a070((void *)((int)this + 0x68),pcVar10);
  if (pcVar5 != (char *)0x0) {
    piVar6 = (int *)CommandLine_Tier0();
    puVar9 = FUN_10009ff0((undefined4 *)((int)this + 0x78));
    (**(code **)(*piVar6 + 0x14))("-startmap",puVar9);
  }
  pcVar10 = (char *)FUN_10008e20(this_01,"basecommandline",0x1001f5e8);
  FUN_1000a070((void *)((int)this + 0x58),pcVar10);
  puVar9 = FUN_10009ff0((undefined4 *)((int)this + 0x38));
  pcVar10 = (char *)FUN_10008e20(this_01,"finaldir",(int)puVar9);
  FUN_1000a070((undefined4 *)((int)this + 0x38),pcVar10);
  puVar9 = FUN_10009ff0((undefined4 *)((int)this + 0x48));
  pcVar10 = (char *)FUN_10008e20(this_01,"workdir",(int)puVar9);
  FUN_1000a070((undefined4 *)((int)this + 0x48),pcVar10);
  iVar7 = FUN_10008a10(this_01,"xbox",0);
  *(bool *)((int)this + 6) = iVar7 != 0;
  FUN_10007e60(&stack0xfffffd6c,0x20,"%i");
  piVar6 = FUN_100088b0(this_01,&stack0xfffffd6c,0);
  while (piVar6 != (int *)0x0) {
    FUN_1000a140((undefined4 *)&stack0xfffffd3c);
    FUN_1000a140((undefined4 *)&stack0xfffffd4c);
    pcVar10 = (char *)FUN_10008e20(piVar6,"subdir",0x1001f5e8);
    FUN_1000a070(&stack0xfffffd3c,pcVar10);
    pcVar10 = (char *)FUN_10008e20(piVar6,"addcommands",0x1001f5e8);
    FUN_1000a070(&stack0xfffffd4c,pcVar10);
    iVar7 = FUN_10009fe0((int)&stack0xfffffd3c);
    if (iVar7 < 1) {
      Error("%s: failed to specify \'subdir\' for item %s\n",auStack_244,&stack0xfffffd6c);
    }
    else {
      FUN_10005f20((void *)((int)this + 0x8c),*(int *)((int)this + 0x98),(int *)&stack0xfffffd3c);
    }
    FUN_10005ca0((int *)&stack0xfffffd3c);
    FUN_10007e60(&stack0xfffffd6c,0x20,"%i");
    piVar6 = FUN_100088b0(this_01,&stack0xfffffd6c,0);
  }
  uVar4 = *(int *)((int)this + 0x98) - 1;
  uVar2 = *(uint *)((int)this + 0x88);
  *(bool *)((int)this + 5) = 0 < *(int *)((int)this + 0x98);
  if ((int)uVar2 <= (int)uVar4) {
    uVar4 = ((int)uVar2 < 0) - 1 & uVar2;
  }
  *(uint *)((int)this + 0x88) = uVar4;
  piVar6 = (int *)CommandLine_Tier0();
  iVar7 = (**(code **)(*piVar6 + 0xc))("-collate",0);
  if (iVar7 != 0) {
    FUN_10006830((int)this);
    *(undefined1 *)((int)this + 5) = 0;
    FUN_1000f3ad(-1);
  }
  FUN_10008cb0((int)this_01);
  uVar1 = *(undefined1 *)((int)this + 5);
  FUN_10004230((int *)&pcStack_2ac);
  FUN_10004230(&iStack_27c);
  return uVar1;
}


// 10007180 FUN_10007180

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10007180(int param_1,int param_2)

{
  int iVar1;
  
  iVar1 = 0;
  if (0 < param_2) {
    do {
      if (DAT_10027620 == 0) {
        DAT_10027620 = (**(code **)(param_1 + iVar1 * 4))("VStudioRender026",0);
        _DAT_10027624 = DAT_10027620;
      }
      if (DAT_10027638 == 0) {
        DAT_10027638 = (**(code **)(param_1 + iVar1 * 4))("VGUI_ivgui008",0);
      }
      if (DAT_1002762c == 0) {
        DAT_1002762c = (**(code **)(param_1 + iVar1 * 4))("VGUI_Input005",0);
      }
      if (DAT_10027634 == 0) {
        DAT_10027634 = (**(code **)(param_1 + iVar1 * 4))("VGUI_Panel009",0);
      }
      if (DAT_10027630 == 0) {
        DAT_10027630 = (**(code **)(param_1 + iVar1 * 4))("VGUI_Surface030",0);
      }
      if (DAT_10027640 == 0) {
        DAT_10027640 = (**(code **)(param_1 + iVar1 * 4))("VGUI_Scheme010",0);
      }
      if (DAT_10027644 == 0) {
        DAT_10027644 = (**(code **)(param_1 + iVar1 * 4))("VGUI_System010",0);
      }
      if (DAT_1002763c == 0) {
        DAT_1002763c = (**(code **)(param_1 + iVar1 * 4))("VGUI_Localize005",0);
      }
      if (DAT_10027628 == 0) {
        DAT_10027628 = (**(code **)(param_1 + iVar1 * 4))("MatSystemSurface006",0);
      }
      if (DAT_10027648 == 0) {
        DAT_10027648 = (**(code **)(param_1 + iVar1 * 4))("VDataCache003",0);
      }
      if (DAT_1002764c == 0) {
        DAT_1002764c = (**(code **)(param_1 + iVar1 * 4))("MDLCache004",0);
        _DAT_10027650 = DAT_1002764c;
      }
      if (DAT_10027654 == 0) {
        DAT_10027654 = (**(code **)(param_1 + iVar1 * 4))("VAvi001",0);
      }
      if (DAT_10027658 == 0) {
        DAT_10027658 = (**(code **)(param_1 + iVar1 * 4))("VBik001",0);
      }
      if (DAT_1002765c == 0) {
        DAT_1002765c = (**(code **)(param_1 + iVar1 * 4))("IQuickTime001",0);
      }
      if (DAT_10027664 == 0) {
        DAT_10027664 = (**(code **)(param_1 + iVar1 * 4))("VPhysicsCollision007",0);
      }
      if (DAT_10027668 == 0) {
        DAT_10027668 = (**(code **)(param_1 + iVar1 * 4))("VSoundEmitter003",0);
      }
      iVar1 = iVar1 + 1;
    } while (iVar1 < param_2);
  }
  return;
}


// 10007380 FUN_10007380

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10007380(void)

{
  DAT_10027620 = 0;
  _DAT_10027624 = 0;
  DAT_10027638 = 0;
  DAT_1002762c = 0;
  DAT_10027634 = 0;
  DAT_10027630 = 0;
  DAT_1002763c = 0;
  DAT_10027640 = 0;
  DAT_10027644 = 0;
  DAT_10027628 = 0;
  DAT_10027648 = 0;
  DAT_1002764c = 0;
  _DAT_10027650 = 0;
  DAT_10027654 = 0;
  DAT_10027658 = 0;
  DAT_1002765c = 0;
  DAT_10027664 = 0;
  _DAT_10027660 = 0;
  DAT_10027668 = 0;
  return;
}


// 100073f0 FUN_100073f0

void __cdecl FUN_100073f0(int param_1,int param_2)

{
  int iVar1;
  
  iVar1 = 0;
  if (0 < param_2) {
    do {
      if (DAT_10027684 == 0) {
        DAT_10027684 = (**(code **)(param_1 + iVar1 * 4))("VFileSystem018",0);
      }
      if (DAT_10027688 == 0) {
        DAT_10027688 = (**(code **)(param_1 + iVar1 * 4))("VMaterialSystem080",0);
        DAT_1002768c = DAT_10027688;
      }
      if (DAT_10027690 == 0) {
        DAT_10027690 = (**(code **)(param_1 + iVar1 * 4))("InputSystemVersion001",0);
      }
      if (DAT_10027694 == 0) {
        DAT_10027694 = (**(code **)(param_1 + iVar1 * 4))("NetworkSystemVersion001",0);
      }
      if (DAT_10027698 == 0) {
        DAT_10027698 = (**(code **)(param_1 + iVar1 * 4))("MaterialSystemHardwareConfig013",0);
      }
      if (DAT_1002769c == 0) {
        DAT_1002769c = (**(code **)(param_1 + iVar1 * 4))("DebugTextureInfo001",0);
      }
      if (DAT_100276a0 == 0) {
        DAT_100276a0 = (**(code **)(param_1 + iVar1 * 4))("VBAllocTracker001",0);
      }
      if (DAT_100276a4 == 0) {
        DAT_100276a4 = (**(code **)(param_1 + iVar1 * 4))("COLORCORRECTION_VERSION_1",0);
      }
      if (DAT_100276a8 == 0) {
        DAT_100276a8 = (**(code **)(param_1 + iVar1 * 4))("VP4001",0);
      }
      if (DAT_100276ac == 0) {
        DAT_100276ac = (**(code **)(param_1 + iVar1 * 4))("VMDLLIB001",0);
      }
      if (DAT_100276b0 == 0) {
        DAT_100276b0 = (**(code **)(param_1 + iVar1 * 4))("QueuedLoaderVersion001",0);
      }
      if (DAT_100276b4 == 0) {
        DAT_100276b4 = (**(code **)(param_1 + iVar1 * 4))("VResourceAccessControl001",0);
      }
      if (DAT_100276b8 == 0) {
        DAT_100276b8 = (**(code **)(param_1 + iVar1 * 4))("MATCHFRAMEWORK_001",0);
      }
      iVar1 = iVar1 + 1;
    } while (iVar1 < param_2);
  }
  return;
}


// 100075a0 FUN_100075a0

void FUN_100075a0(void)

{
  DAT_10027684 = 0;
  DAT_1002768c = 0;
  DAT_10027688 = 0;
  DAT_10027698 = 0;
  DAT_1002769c = 0;
  DAT_10027690 = 0;
  DAT_10027694 = 0;
  DAT_100276a4 = 0;
  DAT_100276a8 = 0;
  DAT_100276ac = 0;
  DAT_100276b0 = 0;
  DAT_100276b4 = 0;
  DAT_100276b8 = 0;
  return;
}


// 100075f0 FUN_100075f0

int __cdecl FUN_100075f0(char *param_1)

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


// 10007620 FUN_10007620

int __cdecl FUN_10007620(byte *param_1,byte *param_2)

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


// 10007680 FUN_10007680

uint __cdecl FUN_10007680(byte *param_1,byte *param_2)

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
        if (uVar1 != uVar2) goto LAB_100076fa;
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
LAB_100076fa:
  return uVar1 - uVar2;
}


// 10007700 FUN_10007700

void __cdecl FUN_10007700(char *param_1)

{
  AssertValidStringPtr(param_1,0xffffff);
  __strlwr(param_1);
  return;
}


// 10007720 FUN_10007720

int __cdecl FUN_10007720(char *param_1,char *param_2,int param_3)

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


// 10007780 FUN_10007780

char * __cdecl FUN_10007780(char *param_1,char *param_2)

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


// 10007860 FUN_10007860

void __cdecl FUN_10007860(char *param_1,char *param_2)

{
  AssertValidStringPtr(param_1,0xffffff);
  AssertValidStringPtr(param_2,0xffffff);
  FUN_10007780(param_1,param_2);
  return;
}


// 10007890 FUN_10007890

char * __cdecl FUN_10007890(char *param_1,char *param_2,uint param_3,size_t param_4)

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


// 10007920 FUN_10007920

void __cdecl FUN_10007920(char *param_1)

{
  char cVar1;
  char *pcVar2;
  int iVar3;
  
  AssertValidStringPtr(param_1,0xffffff);
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  iVar3 = (int)pcVar2 - (int)(param_1 + 1);
  if ((0 < iVar3) && ((param_1[iVar3 + -1] == '\\' || (param_1[iVar3 + -1] == '/')))) {
    param_1[iVar3 + -1] = '\0';
  }
  return;
}


// 10007970 FUN_10007970

void __cdecl FUN_10007970(char *param_1)

{
  char cVar1;
  char *pcVar2;
  
  AssertValidStringPtr(param_1,0xffffff);
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  pcVar2 = pcVar2 + (-1 - (int)(param_1 + 1));
  if (0 < (int)pcVar2) {
    do {
      if ((param_1[(int)pcVar2] == '\\') || (param_1[(int)pcVar2] == '/')) break;
      pcVar2 = pcVar2 + -1;
    } while (0 < (int)pcVar2);
    param_1[(int)pcVar2] = '\0';
  }
  return;
}


// 100079c0 FUN_100079c0

void __cdecl FUN_100079c0(char *param_1,char param_2)

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


// 100079f0 FUN_100079f0

char * __cdecl FUN_100079f0(char *param_1)

{
  char *pcVar1;
  char *pcVar2;
  
  pcVar1 = param_1;
  do {
    pcVar2 = pcVar1;
    pcVar1 = pcVar2 + 1;
  } while (*pcVar2 != '\0');
  pcVar2 = pcVar2 + -1;
  if (pcVar2 != param_1) {
    while (pcVar2[-1] != '.') {
      pcVar2 = pcVar2 + -1;
      if (pcVar2 == param_1) {
        return (char *)0x0;
      }
    }
    if (((pcVar2 != param_1) && (*pcVar2 != '\\')) && (*pcVar2 != '/')) {
      return pcVar2;
    }
  }
  return (char *)0x0;
}


// 10007a40 FUN_10007a40

undefined4 __cdecl FUN_10007a40(char *param_1,char param_2)

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
        FID_conflict__memcpy(param_1 + iVar4,param_1 + iVar4 + 1,iVar5 - iVar4);
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
        FID_conflict__memcpy(param_1 + iVar4,param_1 + iVar4 + 2,(iVar5 - iVar4) - 1);
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
        if ((*param_1 != '.') || (param_1[1] != param_2)) goto LAB_10007b8e;
        FID_conflict__memcpy(param_1,param_1 + 2,iVar5 - 1);
        iVar5 = iVar5 + -2;
        iVar3 = iVar3 + -2;
      }
      else {
        if (((param_1[iVar4 + -1] != '.') && (param_1[iVar4] == '.')) &&
           (param_1[iVar4 + 1] == param_2)) {
          FID_conflict__memcpy(param_1 + iVar4,param_1 + iVar4 + 1,iVar5 - iVar4);
          iVar5 = iVar5 + -1;
          iVar3 = iVar3 + -1;
        }
LAB_10007b8e:
        iVar4 = iVar4 + 1;
      }
    } while (iVar4 <= iVar3);
  }
  if (((2 < iVar5) && (param_1[iVar5 + -1] == '.')) && (param_2 == param_1[iVar5 + -2])) {
    param_1[iVar5 + -2] = '\0';
    iVar5 = iVar5 + -2;
  }
  if (((1 < iVar5) && (*param_1 == '.')) && (param_1[1] == param_2)) {
    FID_conflict__memcpy(param_1,param_1 + 2,iVar5 - 1);
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
      if ((pcVar2[2] != '\0') && (param_2 != pcVar2[2])) goto LAB_10007c51;
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
      FID_conflict__memcpy(_Dst,_Src,(size_t)(pcVar2 + (1 - (int)(_Src + 1))));
      pcVar2 = param_1;
    }
    else {
LAB_10007c51:
      pcVar2 = pcVar2 + 1;
    }
    cVar1 = *pcVar2;
  } while( true );
}


// 10007c70 FUN_10007c70

void __cdecl FUN_10007c70(char *param_1,int param_2)

{
  char cVar1;
  char *pcVar2;
  int iVar3;
  
  AssertValidStringPtr(param_1,0xffffff);
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  iVar3 = (int)pcVar2 - (int)(param_1 + 1);
  if (((0 < iVar3) && (param_1[iVar3 + -1] != '\\')) && (param_1[iVar3 + -1] != '/')) {
    if (param_2 <= iVar3 + 1) {
      Error("V_AppendSlash: ran out of space on %s.",param_1);
    }
    param_1[iVar3] = '\\';
    param_1[iVar3 + 1] = '\0';
  }
  return;
}


// 10007ce0 FUN_10007ce0

undefined1 __cdecl FUN_10007ce0(char *param_1)

{
  char cVar1;
  
  cVar1 = *param_1;
  if ((((cVar1 == '\0') || (param_1[1] != ':')) && (cVar1 != '/')) && (cVar1 != '\\')) {
    return 0;
  }
  return 1;
}


// 10007d00 FUN_10007d00

void __cdecl FUN_10007d00(char *param_1,int param_2,int param_3,undefined1 *param_4,int param_5)

{
  char cVar1;
  char *pcVar2;
  int iVar3;
  size_t _Size;
  
  if (param_5 != 0) {
    pcVar2 = param_1;
    do {
      cVar1 = *pcVar2;
      pcVar2 = pcVar2 + 1;
    } while (cVar1 != '\0');
    iVar3 = (int)pcVar2 - (int)(param_1 + 1);
    if (param_2 < 0) {
      param_2 = iVar3 - -param_2 % iVar3;
    }
    else if (iVar3 <= param_2) {
      *param_4 = 0;
      return;
    }
    if (param_3 < 0) {
      param_3 = iVar3 - -param_3 % iVar3;
    }
    else if (iVar3 < param_3) {
      param_3 = param_3 % iVar3;
    }
    if (param_3 <= param_2) {
      *param_4 = 0;
      return;
    }
    _Size = param_3 - param_2;
    if ((int)_Size <= param_5 + -1) {
      _memcpy(param_4,param_1 + param_2,_Size);
      param_4[_Size] = 0;
      return;
    }
    _memcpy(param_4,param_1 + param_2,param_5 - 1);
    param_4[param_5 + -1] = 0;
  }
  return;
}


// 10007dc0 FUN_10007dc0

void __cdecl FUN_10007dc0(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  _memcpy(param_1,param_2,param_3);
  return;
}


// 10007df0 FUN_10007df0

void __cdecl FUN_10007df0(void *param_1,void *param_2,size_t param_3)

{
  _AssertValidReadPtr(param_2,param_3);
  _AssertValidWritePtr(param_1,param_3);
  FID_conflict__memcpy(param_1,param_2,param_3);
  return;
}


// 10007e20 FUN_10007e20

void __cdecl FUN_10007e20(char *param_1,char *param_2,size_t param_3)

{
  _AssertValidWritePtr(param_1,param_3);
  AssertValidStringPtr(param_2,0xffffff);
  _strncpy(param_1,param_2,param_3);
  if (0 < (int)param_3) {
    param_1[param_3 - 1] = '\0';
  }
  return;
}


// 10007e60 FUN_10007e60

size_t __cdecl FUN_10007e60(char *param_1,size_t param_2,char *param_3)

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


// 10007eb0 FUN_10007eb0

size_t __cdecl FUN_10007eb0(char *param_1,size_t param_2,char *param_3,va_list param_4)

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


// 10007f00 FUN_10007f00

size_t __cdecl FUN_10007f00(char *param_1,size_t param_2,char *param_3,va_list param_4,int param_5)

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


// 10007f60 FUN_10007f60

void __cdecl FUN_10007f60(LPCWSTR param_1,LPSTR param_2,int param_3)

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


// 10007fb0 FUN_10007fb0

void __cdecl FUN_10007fb0(char *param_1,char *param_2,char *param_3)

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
LAB_10008032:
      pcVar2 = (char *)0x0;
LAB_10008034:
      pcVar3 = pcVar4 + (2 - (int)pcVar2);
      if ((int)param_3 <= (int)(pcVar4 + (2 - (int)pcVar2))) {
        pcVar3 = param_3;
      }
      FUN_10007e20(param_2,param_1 + (int)pcVar2,(size_t)pcVar3);
      return;
    }
    if ((param_1[(int)pcVar2] == '\\') || (param_1[(int)pcVar2] == '/')) {
      if ((-1 < (int)pcVar2) && ((param_1[(int)pcVar2] == '\\' || (param_1[(int)pcVar2] == '/')))) {
        pcVar2 = pcVar2 + 1;
        goto LAB_10008034;
      }
      goto LAB_10008032;
    }
    pcVar2 = pcVar2 + -1;
  } while( true );
}


// 10008080 FUN_10008080

void __cdecl FUN_10008080(char *param_1,char *param_2,size_t param_3)

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
    if ((int)sVar3 < 1) goto LAB_100080cf;
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
LAB_100080cf:
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


// 10008140 FUN_10008140

undefined4 __cdecl FUN_10008140(byte *param_1,size_t param_2)

{
  uint in_EAX;
  int iVar1;
  size_t sVar2;
  
  if (((*param_1 == 0) || (in_EAX = FUN_10007680(param_1,&DAT_100204a8), in_EAX == 0)) ||
     (in_EAX = FUN_10007680(param_1,&DAT_100204a4), in_EAX == 0)) {
    return in_EAX & 0xffffff00;
  }
  iVar1 = FUN_100075f0((char *)param_1);
  if ((param_1[iVar1 + -1] == 0x5c) || (param_1[iVar1 + -1] == 0x2f)) {
    iVar1 = iVar1 + -1;
  }
  do {
    if (iVar1 < 1) {
      if (iVar1 == 0) {
        sVar2 = FUN_10007e60((char *)param_1,param_2,".%c");
        return CONCAT31((int3)(sVar2 >> 8),1);
      }
LAB_100081d1:
      return CONCAT31((int3)((uint)iVar1 >> 8),1);
    }
    if ((param_1[iVar1 + -1] == 0x5c) || (param_1[iVar1 + -1] == 0x2f)) {
      param_1[iVar1] = 0;
      iVar1 = FUN_100079c0((char *)param_1,'\\');
      goto LAB_100081d1;
    }
    iVar1 = iVar1 + -1;
  } while( true );
}


// 100081e0 FUN_100081e0

void __cdecl FUN_100081e0(char *param_1,size_t param_2,char *param_3,char *param_4)

{
  char cVar1;
  char *pcVar2;
  undefined4 uVar3;
  
  cVar1 = *param_3;
  if ((((cVar1 == '\0') || (param_3[1] != ':')) && (cVar1 != '/')) && (cVar1 != '\\')) {
    if ((param_4 == (char *)0x0) ||
       (((cVar1 = *param_4, cVar1 == '\0' || (param_4[1] != ':')) &&
        ((cVar1 != '/' && (cVar1 != '\\')))))) {
      pcVar2 = __getcwd(param_1,param_2);
      if (pcVar2 == (char *)0x0) {
        Error("V_MakeAbsolutePath: _getcwd failed.");
      }
      if (param_4 != (char *)0x0) {
        FUN_10007c70(param_1,param_2);
        FUN_10007890(param_1,param_4,param_2,0xffffffff);
      }
    }
    else {
      FUN_10007e20(param_1,param_4,param_2);
    }
    FUN_10007c70(param_1,param_2);
    FUN_10007890(param_1,param_3,param_2,0xffffffff);
  }
  else {
    _AssertValidWritePtr(param_1,param_2);
    AssertValidStringPtr(param_3,0xffffff);
    _strncpy(param_1,param_3,param_2);
    if (0 < (int)param_2) {
      param_1[param_2 - 1] = '\0';
    }
  }
  uVar3 = FUN_10007a40(param_1,'\\');
  if ((char)uVar3 == '\0') {
    Error("V_MakeAbsolutePath: tried to \"..\" past the root.");
  }
  cVar1 = *param_1;
  while (cVar1 != '\0') {
    if ((*param_1 == '/') || (*param_1 == '\\')) {
      *param_1 = '\\';
    }
    param_1 = param_1 + 1;
    cVar1 = *param_1;
  }
  return;
}


// 10008300 FUN_10008300

void __cdecl FUN_10008300(char *param_1,int param_2,char *param_3,size_t param_4)

{
  char cVar1;
  char *pcVar2;
  
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  if ((int)pcVar2 - (int)(param_1 + 1) <= param_2) {
    _AssertValidWritePtr(param_3,param_4);
    AssertValidStringPtr(param_1,0xffffff);
    _strncpy(param_3,param_1,param_4);
    if (0 < (int)param_4) {
      param_3[param_4 - 1] = '\0';
    }
    return;
  }
  pcVar2 = param_1;
  do {
    cVar1 = *pcVar2;
    pcVar2 = pcVar2 + 1;
  } while (cVar1 != '\0');
  FUN_10007d00(param_1,-param_2,(int)pcVar2 - (int)(param_1 + 1),param_3,param_4);
  return;
}


// 10008390 FUN_10008390

void __thiscall FUN_10008390(void *this,undefined4 param_1)

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
  Warning(&DAT_100200ac);
  return;
}


// 10008470 FUN_10008470

void __fastcall FUN_10008470(int param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  
  iVar2 = *(int *)(param_1 + 0x18);
  while (iVar2 != 0) {
    iVar1 = *(int *)(iVar2 + 0x14);
    *(undefined4 *)(iVar2 + 0x14) = 0;
    FUN_10008470(iVar2);
    piVar3 = (int *)KeyValuesSystem();
    (**(code **)(*piVar3 + 8))(iVar2);
    iVar2 = iVar1;
  }
  iVar2 = *(int *)(param_1 + 0x14);
  while ((iVar2 != 0 && (iVar2 != param_1))) {
    iVar1 = *(int *)(iVar2 + 0x14);
    *(undefined4 *)(iVar2 + 0x14) = 0;
    FUN_10008470(iVar2);
    piVar3 = (int *)KeyValuesSystem();
    (**(code **)(*piVar3 + 8))(iVar2);
    iVar2 = iVar1;
  }
  FUN_10005520(*(undefined4 *)(param_1 + 4));
  *(undefined4 *)(param_1 + 4) = 0;
  FUN_10005520(*(undefined4 *)(param_1 + 8));
  *(undefined4 *)(param_1 + 8) = 0;
  return;
}


// 10008500 FUN_10008500

undefined1 * __thiscall
FUN_10008500(void *this,int *param_1,undefined1 *param_2,undefined1 *param_3)

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
    FUN_1000a540(param_1);
    cVar1 = (char)param_1[5];
    while (cVar1 == '\0') {
      uVar4 = FUN_1000ad90(param_1);
      if ((char)uVar4 == '\0') {
        pcVar5 = (char *)FUN_1000a370(param_1,1,0);
        if (pcVar5 == (char *)0x0) {
          return (undefined1 *)0x0;
        }
        cVar1 = *pcVar5;
        if (cVar1 == '\"') {
          *param_2 = 1;
          if (*(char *)((int)this + 0x11) == '\0') {
            puVar6 = (undefined4 *)FUN_1000a160();
          }
          else {
            puVar6 = (undefined4 *)FUN_1000a150();
          }
          FUN_1000af50(param_1,puVar6,&DAT_100276e0,0x400);
          return &DAT_100276e0;
        }
        if ((cVar1 == '{') || (cVar1 == '}')) {
          DAT_100276e1 = 0;
          DAT_100276e0 = cVar1;
          FUN_1000a3b0(param_1,1,1);
          return &DAT_100276e0;
        }
        iVar8 = 0;
        bVar3 = false;
        bVar2 = false;
        pcVar5 = (char *)FUN_1000a370(param_1,1,0);
        goto joined_r0x100085be;
      }
      FUN_1000a540(param_1);
      cVar1 = (char)param_1[5];
    }
  }
  return (undefined1 *)0x0;
joined_r0x100085be:
  if ((((pcVar5 == (char *)0x0) || (cVar1 = *pcVar5, cVar1 == '\0')) || (cVar1 == '\"')) ||
     ((cVar1 == '{' || (cVar1 == '}')))) {
LAB_10008659:
    (&DAT_100276e0)[iVar8] = 0;
    return &DAT_100276e0;
  }
  if (cVar1 == '[') {
    bVar2 = true;
  }
  if ((cVar1 == ']') && (bVar2)) {
    bVar2 = false;
    *param_3 = 1;
  }
  iVar7 = _isspace((int)*pcVar5);
  if ((iVar7 != 0) && (!bVar2)) goto LAB_10008659;
  if (iVar8 < 0x3ff) {
    (&DAT_100276e0)[iVar8] = *pcVar5;
    iVar8 = iVar8 + 1;
  }
  else if (!bVar3) {
    bVar3 = true;
    FUN_10008390(&DAT_10024270," ReadToken overflow");
  }
  FUN_1000a3b0(param_1,1,1);
  pcVar5 = (char *)FUN_1000a370(param_1,1,0);
  goto joined_r0x100085be;
}


// 10008690 FUN_10008690

void __thiscall FUN_10008690(void *this,int param_1)

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


// 100086e0 FUN_100086e0

void __thiscall FUN_100086e0(void *this,int param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  
  puVar2 = (undefined4 *)0x0;
  for (puVar1 = *(undefined4 **)((int)this + 0x18); puVar1 != (undefined4 *)0x0;
      puVar1 = (undefined4 *)puVar1[5]) {
    puVar3 = FUN_10008b00(puVar1);
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


// 10008720 FUN_10008720

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

bool FUN_10008720(void)

{
  char cVar1;
  int iVar2;
  char *pcVar3;
  uint uVar4;
  char *unaff_ESI;
  bool bVar5;
  byte local_40 [64];
  
  iVar2 = FUN_100075f0(unaff_ESI);
  if (iVar2 < 4) {
    bVar5 = false;
  }
  else {
    cVar1 = unaff_ESI[2];
    pcVar3 = unaff_ESI + 2;
    if (cVar1 == '!') {
      pcVar3 = unaff_ESI + 3;
    }
    FUN_10007e20((char *)local_40,pcVar3,0x40);
    iVar2 = FUN_100075f0((char *)local_40);
    local_40[iVar2 + -1] = 0;
    if ((_DAT_10027b9c & 1) == 0) {
      _DAT_10027b9c = _DAT_10027b9c | 1;
      FUN_1000bed0(&DAT_10027b94,"cl_language");
    }
    uVar4 = FUN_10007680(local_40,*(byte **)(DAT_10027b98 + 0x24));
    bVar5 = uVar4 == 0;
    if (cVar1 == '!') {
      return !bVar5;
    }
  }
  return bVar5;
}


// 100087c0 FUN_100087c0

bool FUN_100087c0(void)

{
  byte *in_EAX;
  char *pcVar1;
  uint uVar2;
  bool bVar3;
  
  pcVar1 = FUN_10007780((char *)in_EAX,"$OSX");
  if (pcVar1 == (char *)0x0) {
    pcVar1 = FUN_10007780((char *)in_EAX,"$LINUX");
    if (pcVar1 == (char *)0x0) {
      pcVar1 = FUN_10007780((char *)in_EAX,"$POSIX");
      if (pcVar1 == (char *)0x0) {
        pcVar1 = FUN_10007780((char *)in_EAX,"$WINDOWS");
        if (pcVar1 != (char *)0x0) {
          return true;
        }
        uVar2 = FUN_10007680((byte *)"[$WIN32]",in_EAX);
        bVar3 = uVar2 == 0;
        if (!bVar3) {
          bVar3 = FUN_10008720();
        }
        return bVar3;
      }
    }
  }
  return false;
}


// 10008840 FUN_10008840

void __cdecl FUN_10008840(undefined4 param_1)

{
  int *piVar1;
  
  piVar1 = (int *)KeyValuesSystem();
  (**(code **)(*piVar1 + 4))(param_1);
  return;
}


// 10008860 FUN_10008860

undefined4 * __thiscall FUN_10008860(void *this,undefined4 param_1)

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


// 100088b0 FUN_100088b0

int * __thiscall FUN_100088b0(void *this,char *param_1,undefined4 param_2)

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
      FUN_10007e20(&local_100,param_1,(size_t)pcVar1);
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
      if (*this == iVar3) goto LAB_100089d5;
      piVar2 = (int *)this;
      piVar4 = *(int **)((int)this + 0x14);
    }
    if (*(void **)(unaff_EBP + 0x1c) != (void *)0x0) {
      this = FUN_100088b0(*(void **)(unaff_EBP + 0x1c),param_1,0);
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
        this = FUN_10008860(this_00,pcVar5);
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
LAB_100089d5:
    if (unaff_ESI != 0) {
      this = FUN_100088b0(this,(char *)(unaff_ESI + 1),unaff_retaddr);
    }
  }
  return (int *)this;
}


// 10008a10 FUN_10008a10

int __thiscall FUN_10008a10(void *this,char *param_1,int param_2)

{
  int *piVar1;
  long lVar2;
  int iVar3;
  undefined4 extraout_EDX;
  ulonglong uVar4;
  
  piVar1 = FUN_100088b0(this,param_1,0);
  if (piVar1 != (int *)0x0) {
    iVar3 = (char)piVar1[4] + -1;
    switch(iVar3) {
    case 0:
      lVar2 = _atol((char *)piVar1[1]);
      return lVar2;
    default:
      return piVar1[3];
    case 2:
      uVar4 = FUN_10010690(iVar3,extraout_EDX);
      return (int)uVar4;
    case 4:
      iVar3 = thunk_FUN_100109dc((wchar_t *)piVar1[2]);
      return iVar3;
    case 6:
      return 0;
    }
  }
  return param_2;
}


// 10008a90 FUN_10008a90

void __thiscall FUN_10008a90(void *this,char *param_1,char *param_2)

{
  int *piVar1;
  int iVar2;
  void *pvVar3;
  
  piVar1 = FUN_100088b0(this,param_1,1);
  if (piVar1 != (int *)0x0) {
    FUN_10005520(piVar1[1]);
    FUN_10005520(piVar1[2]);
    piVar1[2] = 0;
    if (param_2 == (char *)0x0) {
      param_2 = "";
    }
    iVar2 = FUN_100075f0(param_2);
    pvVar3 = (void *)FUN_10005720(iVar2 + 1U);
    piVar1[1] = (int)pvVar3;
    FUN_10007dc0(pvVar3,param_2,iVar2 + 1U);
    *(undefined1 *)(piVar1 + 4) = 1;
  }
  return;
}


// 10008b00 FUN_10008b00

undefined4 * __fastcall FUN_10008b00(undefined4 *param_1)

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
    puVar7 = FUN_10008860(pvVar5,&DAT_1001f5e8);
  }
  else {
    piVar4 = (int *)KeyValuesSystem();
    uVar6 = (**(code **)(*piVar4 + 0x10))(*param_1);
    puVar7 = FUN_10008860(pvVar5,uVar6);
  }
  *(undefined1 *)(puVar7 + 4) = *(undefined1 *)(param_1 + 4);
  switch(*(undefined1 *)(param_1 + 4)) {
  case 1:
    if ((char *)param_1[1] != (char *)0x0) {
      iVar9 = FUN_100075f0((char *)param_1[1]);
      pvVar5 = (void *)FUN_10005720(iVar9 + 1U);
      puVar7[1] = pvVar5;
      FUN_10007dc0(pvVar5,(void *)param_1[1],iVar9 + 1U);
      FUN_100086e0(param_1,(int)puVar7);
      return puVar7;
    }
    break;
  case 2:
    puVar7[3] = param_1[3];
    FUN_100086e0(param_1,(int)puVar7);
    return puVar7;
  case 3:
    puVar7[3] = param_1[3];
    FUN_100086e0(param_1,(int)puVar7);
    return puVar7;
  case 4:
    puVar7[3] = param_1[3];
    FUN_100086e0(param_1,(int)puVar7);
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
      pvVar5 = (void *)FUN_10005720(-(uint)((int)((ulonglong)lVar3 >> 0x20) != 0) | (uint)lVar3);
      puVar7[2] = pvVar5;
      FUN_10007dc0(pvVar5,(void *)param_1[2],iVar9 * 2 + 2);
      FUN_100086e0(param_1,(int)puVar7);
      return puVar7;
    }
    break;
  case 6:
    *(undefined1 *)(puVar7 + 3) = *(undefined1 *)(param_1 + 3);
    *(undefined1 *)((int)puVar7 + 0xd) = *(undefined1 *)((int)param_1 + 0xd);
    *(undefined1 *)((int)puVar7 + 0xe) = *(undefined1 *)((int)param_1 + 0xe);
    *(undefined1 *)((int)puVar7 + 0xf) = *(undefined1 *)((int)param_1 + 0xf);
    FUN_100086e0(param_1,(int)puVar7);
    return puVar7;
  case 7:
    pvVar5 = (void *)FUN_10005720(8);
    puVar7[1] = pvVar5;
    FUN_10007dc0(pvVar5,(void *)param_1[1],8);
  }
  FUN_100086e0(param_1,(int)puVar7);
  return puVar7;
}


// 10008cb0 FUN_10008cb0

void __fastcall FUN_10008cb0(int param_1)

{
  int *piVar1;
  
  if (param_1 != 0) {
    FUN_10008470(param_1);
    piVar1 = (int *)KeyValuesSystem();
    (**(code **)(*piVar1 + 8))(param_1);
  }
  return;
}


// 10008cd0 FUN_10008cd0

void __thiscall FUN_10008cd0(void *this,int param_1)

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
        pbVar5 = &DAT_1001f5e8;
      }
      else {
        piVar3 = (int *)KeyValuesSystem();
        pbVar5 = (byte *)(**(code **)(*piVar3 + 0x10))(*puVar1);
      }
      iVar6 = FUN_10007620(pbVar5,pbVar4);
      if (iVar6 == 0) {
        FUN_10008cd0(puVar7,(int)puVar1);
        goto LAB_10008d72;
      }
    }
    puVar7 = FUN_10008b00(puVar1);
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
LAB_10008d72:
    puVar1 = (undefined4 *)puVar1[5];
  } while( true );
}


// 10008d90 FUN_10008d90

int * __fastcall FUN_10008d90(int param_1)

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
    piVar2 = FUN_10008860(this,unaff_retaddr);
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
      FUN_10008470((int)piVar2);
      piVar3 = (int *)KeyValuesSystem();
      (**(code **)(*piVar3 + 8))(piVar2);
    }
  }
  return (int *)0x0;
}


// 10008e20 FUN_10008e20

int __thiscall FUN_10008e20(void *this,char *param_1,int param_2)

{
  int *piVar1;
  int iVar2;
  char local_240 [64];
  CHAR local_200 [512];
  
  piVar1 = FUN_100088b0(this,param_1,0);
  if (piVar1 != (int *)0x0) {
    switch((char)piVar1[4]) {
    case '\x01':
switchD_10008e54_caseD_1:
      return piVar1[1];
    case '\x02':
    case '\x04':
      FUN_10007e60(local_240,0x40,"%d");
      FUN_10008a90(this,param_1,local_240);
      return piVar1[1];
    case '\x03':
      FUN_10007e60(local_240,0x40,"%f");
      FUN_10008a90(this,param_1,local_240);
      return piVar1[1];
    case '\x05':
      iVar2 = FUN_10007f60((LPCWSTR)piVar1[2],local_200,0x200);
      if (iVar2 != 0) {
        FUN_10008a90(this,param_1,local_200);
        goto switchD_10008e54_caseD_1;
      }
      break;
    case '\a':
      FUN_10007e60(local_240,0x40,"%lld");
      FUN_10008a90(this,param_1,local_240);
      return piVar1[1];
    }
  }
  return param_2;
}


// 10008f70 FUN_10008f70

void __thiscall FUN_10008f70(void *this,int *param_1)

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


// 10008fb0 FUN_10008fb0

void __thiscall FUN_10008fb0(void *this,undefined4 param_1,int *param_2)

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
  if (DAT_10024374 < 0x40) {
    *(undefined4 *)(&DAT_10024270 + DAT_10024374 * 4) = uVar6;
  }
  iVar2 = DAT_10024374 + 1;
  if (DAT_10024378 <= DAT_10024374) {
    DAT_10024378 = DAT_10024374;
  }
  local_1c = this;
  if (100 < DAT_10024374) {
    DAT_10024374 = iVar2;
    FUN_10008390(&DAT_10024270,"RecursiveLoadFromBuffer:  recursion overflow");
    DAT_10024374 = DAT_10024374 + -1;
    return;
  }
  if (iVar2 < 0x40) {
    DAT_10024374 = iVar2;
    *(undefined4 *)(&DAT_10024270 + iVar2 * 4) = 0xffffffff;
    iVar2 = DAT_10024374;
  }
  DAT_10024374 = iVar2 + 1;
  if (DAT_10024378 <= iVar2) {
    DAT_10024378 = iVar2;
  }
  local_25 = '\x01';
  local_18 = iVar2;
  pcVar3 = FUN_10008500(this,param_2,&local_26,&local_27);
  do {
    if (pcVar3 == (char *)0x0) {
      pcVar3 = "RecursiveLoadFromBuffer:  got EOF instead of keyname";
LAB_100093b0:
      FUN_10008390(&DAT_10024270,pcVar3);
LAB_100093ba:
      DAT_10024374 = DAT_10024374 + -2;
      return;
    }
    if (*pcVar3 == '\0') {
      pcVar3 = "RecursiveLoadFromBuffer:  got empty keyname";
      goto LAB_100093b0;
    }
    if ((*pcVar3 == '}') && (local_26 == '\0')) goto LAB_100093ba;
    this_00 = FUN_10008d90((int)this);
    local_10 = this_00;
    if (this_00 == (int *)0x0) {
LAB_1000939d:
      pcVar3 = "RecursiveLoadFromBuffer:  got NULL key";
      goto LAB_100093b0;
    }
    if (iVar2 < 0x40) {
      *(int *)(&DAT_10024270 + iVar2 * 4) = *this_00;
    }
    _Str = FUN_10008500(this,param_2,&local_26,&local_27);
    if (local_27 != '\0') {
      local_20 = _Str;
      if (_Str == (byte *)0x0) goto LAB_1000939d;
      local_25 = FUN_100087c0();
      _Str = FUN_10008500(this,param_2,&local_26,&local_27);
    }
    local_20 = _Str;
    if (_Str == (byte *)0x0) goto LAB_1000939d;
    if ((*_Str == 0x7d) && (local_26 == '\0')) {
      pcVar3 = "RecursiveLoadFromBuffer:  got } in key";
      goto LAB_100093b0;
    }
    if ((*_Str == 0x7b) && (local_26 == '\0')) {
      if (local_18 < 0x40) {
        *(undefined4 *)(&DAT_10024270 + local_18 * 4) = 0xffffffff;
      }
      FUN_10008fb0(this_00,param_1,param_2);
    }
    else {
      if (local_27 != '\0') {
        pcVar3 = "RecursiveLoadFromBuffer:  got conditional between key and value";
        goto LAB_100093b0;
      }
      if (this_00[1] != 0) {
        FUN_10005520(this_00[1]);
        this_00[1] = 0;
      }
      iVar2 = FUN_100075f0((char *)_Str);
      local_c = iVar2;
      local_14 = _strtol((char *)_Str,(char **)&local_4,10);
      fVar7 = (float10)FUN_10010bc4(_Str,&local_8);
      if (*_Str == 0) {
LAB_1000919a:
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
        plVar5 = (longlong *)FUN_10005720(8);
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
        if (local_4 != _Str + iVar2) goto LAB_1000919a;
        this_00[3] = local_14;
        *(undefined1 *)(this_00 + 4) = 2;
      }
      if ((char)this_00[4] == '\x01') {
        pvVar4 = (void *)FUN_10005720(iVar2 + 1U);
        this_00[1] = (int)pvVar4;
        FUN_10007dc0(pvVar4,_Str,iVar2 + 1U);
      }
      iVar2 = param_2[3];
      FUN_10008500(local_1c,param_2,&local_26,&local_27);
      if (local_27 == '\0') {
        FUN_1000a3b0(param_2,0,iVar2);
        this = local_1c;
      }
      else {
        local_25 = FUN_100087c0();
        this = local_1c;
      }
    }
    if (local_25 == '\0') {
      FUN_10008690(this,(int)this_00);
      FUN_10008cb0((int)this_00);
    }
    local_25 = '\x01';
    pcVar3 = FUN_10008500(this,param_2,&local_26,&local_27);
    iVar2 = local_18;
  } while( true );
}


// 100093d0 FUN_100093d0

undefined1 __fastcall FUN_100093d0(int *param_1)

{
  undefined1 uVar1;
  char cVar2;
  
  if ((*(byte *)((int)param_1 + 0x15) & 1) != 0) {
    FUN_1000b3a0(param_1,"%c");
    return 0;
  }
  cVar2 = FUN_1000a2a0(param_1,1);
  if (cVar2 != '\0') {
    uVar1 = *(undefined1 *)((param_1[3] - param_1[8]) + *param_1);
    param_1[3] = param_1[3] + 1;
    return uVar1;
  }
  return 0;
}


// 10009420 FUN_10009420

undefined1 __thiscall FUN_10009420(void *this,int *param_1,undefined *param_2)

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
    PTR_s_unknown_10024258 = param_2;
    uStack_5c = 0x10009468;
    pvStack_58 = this_00;
    iVar3 = (**(code **)(*param_1 + 0x1c))();
    uStack_5c = 0;
    uStack_60 = 0;
    piVar1 = param_1 + -1;
    puStack_64 = auStack_c;
    uVar4 = iVar3 + 2;
    auStack_70[1] = 0x10009489;
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
      FUN_1000aa70(auStack_70,0,0,0);
      iVar3 = FUN_100075f0(pcVar5);
      FUN_1000ab70(auStack_70,(int)pcVar5,(int)this,iVar3,9);
      FUN_100096a0(this_00,pcStack_38,(int *)auStack_70,param_1,(void *)0x0);
      if ((-1 < (int)pvStack_68) && (auStack_70[0] != 0)) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(auStack_70[0]);
      }
    }
    (**(code **)(*piVar1 + 0x158))(pcVar5);
    return unaff_SI;
  }
  return 0;
}


// 10009580 FUN_10009580

void __thiscall FUN_10009580(void *this,char *param_1,char *param_2,int *param_3,void *param_4)

{
  char cVar1;
  int iVar2;
  int *piVar3;
  void *this_00;
  undefined4 *this_01;
  undefined1 auStack_204 [3];
  char acStack_201 [513];
  
  if (param_3 != (int *)0x0) {
    FUN_10007e20(acStack_201 + 1,param_1,0x200);
    for (iVar2 = FUN_100075f0(acStack_201 + 1);
        ((0 < iVar2 && (acStack_201[iVar2] != '\\')) && (acStack_201[iVar2] != '/'));
        iVar2 = iVar2 + -1) {
      acStack_201[iVar2] = '\0';
    }
    FUN_10007890(acStack_201 + 1,param_2,0x200,0xffffffff);
    piVar3 = (int *)KeyValuesSystem();
    this_00 = (void *)(**(code **)(*piVar3 + 4))(0x24);
    if (this_00 == (void *)0x0) {
      this_01 = (undefined4 *)0x0;
    }
    else {
      this_01 = FUN_10008860(this_00,auStack_204);
    }
    *(bool *)((int)this_01 + 0x11) = *(char *)((int)this + 0x11) != '\0';
    cVar1 = FUN_10009420(this_01,param_3,auStack_204);
    if (cVar1 != '\0') {
      FUN_1000c820(param_4,*(int *)((int)param_4 + 0xc),(undefined4 *)&stack0xfffffdf8);
      return;
    }
    DevMsg("KeyValues::ParseIncludedKeys: Couldn\'t load included keyvalue file %s\n",auStack_204);
    FUN_10008470((int)this_01);
    piVar3 = (int *)KeyValuesSystem();
    (**(code **)(*piVar3 + 8))(this_01);
  }
  return;
}


// 100096a0 FUN_100096a0

undefined4 __thiscall FUN_100096a0(void *this,char *param_1,int *param_2,int *param_3,void *param_4)

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
  if (DVar4 != DAT_10027aec) {
    DVar5 = 0;
    LOCK();
    DVar3 = DVar4;
    if (DAT_10027aec != 0) {
      DVar5 = DAT_10027aec;
      DVar3 = DAT_10027aec;
    }
    DAT_10027aec = DVar3;
    UNLOCK();
    if (DVar5 != 0) {
      CThreadFastMutex::Lock((CThreadFastMutex *)&DAT_10027aec,DVar4,0);
      goto LAB_100096e7;
    }
  }
  DAT_10027af0 = DAT_10027af0 + 1;
LAB_100096e7:
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
  PTR_DAT_10024370 = param_1;
  DAT_10024378 = 0;
  this_01 = (undefined4 *)this;
  do {
    cStack_2d = '\x01';
    pbVar6 = FUN_10008500(this,param_2,&cStack_2f,&cStack_2e);
    if ((((char)param_2[5] != '\0') || (pbVar6 == (byte *)0x0)) ||
       ((cStack_2f == '\0' && (*pbVar6 == 0)))) break;
    uVar7 = FUN_10007680(pbVar6,(byte *)"#include");
    puVar12 = this_01;
    if (uVar7 == 0) {
      pcVar8 = FUN_10008500(this,param_2,&cStack_2f,&cStack_2e);
      if ((pcVar8 == (char *)0x0) || (*pcVar8 == '\0')) {
        FUN_10008390(&DAT_10024270,"#include is NULL ");
      }
      else {
        FUN_10009580(this,param_1,pcVar8,param_3,param_4);
      }
    }
    else {
      uVar7 = FUN_10007680(pbVar6,(byte *)"#base");
      if (uVar7 == 0) {
        pcVar8 = FUN_10008500(this,param_2,&cStack_2f,&cStack_2e);
        if ((pcVar8 == (char *)0x0) || (*pcVar8 == '\0')) {
          FUN_10008390(&DAT_10024270,"#base is NULL ");
        }
        else {
          FUN_10009580(this,param_1,pcVar8,param_3,param_4);
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
            this_01 = FUN_10008860(this_00,pbVar6);
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
        pcVar8 = FUN_10008500(this,param_2,&cStack_2f,&cStack_2e);
        if (cStack_2e != '\0') {
          cStack_2d = FUN_100087c0();
          pcVar8 = FUN_10008500(this,param_2,&cStack_2f,&cStack_2e);
        }
        if (((pcVar8 == (char *)0x0) || (*pcVar8 != '{')) || (cStack_2f != '\0')) {
          FUN_10008390(&DAT_10024270,"LoadFromBuffer: missing {");
        }
        else {
          FUN_10008fb0(this_01,param_1,param_2);
        }
        if (cStack_2d == '\0') {
          if (puStack_2c != (undefined4 *)0x0) {
            puStack_2c[5] = 0;
          }
          iVar1 = this_01[6];
          if (iVar1 != 0) {
            FUN_10008470(iVar1);
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
  FUN_10008f70(this,(int *)&pDStack_28);
  iVar13 = iStack_14;
  iVar2 = iStack_8;
  iVar1 = iStack_1c;
  while (iVar1 = iVar1 + -1, iStack_14 = iVar13, iStack_8 = iVar2, 0 < iVar1) {
    DVar4 = pDStack_28[iVar1];
    if (DVar4 != 0) {
      FUN_10008470(DVar4);
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
      FUN_10008cd0(this,*(int *)(iVar13 + iVar11 * 4));
      iVar11 = iVar11 + 1;
      iVar1 = iStack_c;
    } while (iVar11 < iVar2);
  }
  while (iVar2 = iVar2 + -1, iStack_c = iVar1, -1 < iVar2) {
    iVar11 = *(int *)(iVar13 + iVar2 * 4);
    if (iVar11 != 0) {
      FUN_10008470(iVar11);
      piVar10 = (int *)KeyValuesSystem();
      (**(code **)(*piVar10 + 8))(iVar11);
      iVar1 = iStack_c;
    }
  }
  PTR_DAT_10024370 = &DAT_1001f5e8;
  DAT_10024378 = 0;
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
  DAT_10027af0 = DAT_10027af0 + -1;
  if (DAT_10027af0 == 0) {
    pDStack_28 = &DAT_10027aec;
    LOCK();
    DAT_10027aec = 0;
    UNLOCK();
  }
  return CONCAT31((int3)((uint)pDStack_28 >> 8),1);
}


// 10009ab0 FUN_10009ab0

uint __thiscall FUN_10009ab0(void *this,char *param_1,char *param_2,int *param_3,void *param_4)

{
  undefined4 in_EAX;
  int iVar1;
  LPSTR pCVar2;
  int iVar3;
  int local_30 [2];
  int local_28;
  
  if (param_2 == (char *)0x0) {
    return CONCAT31((int3)((uint)in_EAX >> 8),1);
  }
  iVar1 = FUN_100075f0(param_2);
  FUN_1000aaf0(local_30,(int)param_2,iVar1,9);
  if (((2 < iVar1) && (*param_2 == -1)) && (param_2[1] == -2)) {
    iVar1 = FUN_10007f60((LPCWSTR)(param_2 + 2),(LPSTR)0x0,0);
    pCVar2 = (LPSTR)FUN_10005720(iVar1);
    FUN_10007f60((LPCWSTR)(param_2 + 2),pCVar2,iVar1);
    FUN_1000ac10(local_30,(int)pCVar2,iVar1,iVar1,9);
  }
  iVar3 = FUN_100096a0(this,param_1,local_30,param_3,param_4);
  iVar1 = iVar3;
  if ((-1 < local_28) && (iVar1 = local_30[0], local_30[0] != 0)) {
    iVar1 = (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(local_30[0]);
  }
  return CONCAT31((int3)((uint)iVar1 >> 8),(char)iVar3);
}


// 10009b70 FUN_10009b70

undefined4 __cdecl FUN_10009b70(byte *param_1,undefined4 *param_2)

{
  int *piVar1;
  byte bVar2;
  byte *pbVar3;
  int iVar4;
  undefined4 uVar5;
  byte *pbVar6;
  undefined4 *puVar7;
  bool bVar8;
  
  puVar7 = DAT_10027ba0;
  if (DAT_10027ba0 != (undefined4 *)0x0) {
    do {
      pbVar3 = (byte *)puVar7[1];
      pbVar6 = param_1;
      do {
        bVar2 = *pbVar3;
        bVar8 = bVar2 < *pbVar6;
        if (bVar2 != *pbVar6) {
LAB_10009ba5:
          iVar4 = (1 - (uint)bVar8) - (uint)(bVar8 != 0);
          goto LAB_10009baa;
        }
        if (bVar2 == 0) break;
        bVar2 = pbVar3[1];
        bVar8 = bVar2 < pbVar6[1];
        if (bVar2 != pbVar6[1]) goto LAB_10009ba5;
        pbVar3 = pbVar3 + 2;
        pbVar6 = pbVar6 + 2;
      } while (bVar2 != 0);
      iVar4 = 0;
LAB_10009baa:
      if (iVar4 == 0) {
        if (param_2 != (undefined4 *)0x0) {
          *param_2 = 0;
        }
                    /* WARNING: Could not recover jumptable at 0x10009bda. Too many branches */
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


// 10009bf0 FUN_10009bf0

undefined4 __cdecl FUN_10009bf0(undefined4 *param_1)

{
  HMODULE pHVar1;
  
  pHVar1 = LoadLibraryExA((LPCSTR)*param_1,(HANDLE)0x0,8);
  param_1[1] = pHVar1;
  return 0;
}


// 10009c10 FUN_10009c10

HMODULE FUN_10009c10(void)

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
  
  FUN_10007e20(local_400,in_EAX,0x400);
  iVar1 = FUN_10007860(local_400,".dll");
  if (iVar1 == 0) {
    FUN_10007890(local_400,".dll",0x400,0xffffffff);
  }
  FUN_100079c0(local_400,'\\');
  pcVar2 = (code *)GetThreadedLoadLibraryFunc();
  if (pcVar2 != (code *)0x0) {
    pcStack_408 = local_400;
    pHStack_404 = (HMODULE)0x0;
    uVar4 = CreateSimpleThread(FUN_10009bf0,&pcStack_408,0);
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


// 10009d30 FUN_10009d30

void FUN_10009d30(void)

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
    iVar3 = (**(code **)(*piVar2 + 0x28))(&DAT_10020778);
    if (iVar3 == 0) goto LAB_10009dc4;
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
  FUN_10007890(&stack0xfffffef8,pcVar6,0x104,0xffffffff);
  pHVar5 = FUN_10009c10();
  if (pHVar5 != (HMODULE)0x0) {
    return;
  }
LAB_10009dc4:
  FUN_10009c10();
  return;
}


// 10009de0 FUN_10009de0

HMODULE __cdecl FUN_10009de0(char *param_1)

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
  
  cVar1 = FUN_10007ce0(param_1);
  if (cVar1 == '\0') {
    __getcwd(local_904,0x400);
    pcVar7 = local_904;
    do {
      pcVar2 = pcVar7;
      pcVar7 = pcVar2 + 1;
    } while (*pcVar2 != '\0');
    if (pcVar2[-1] == '/') {
LAB_10009e52:
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
      if (pcVar2[-1] == '\\') goto LAB_10009e52;
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
    FUN_10007e60(local_400,0x400,pcVar7);
    hModule = (HMODULE)FUN_10009d30();
    if (hModule != (HMODULE)0x0) goto LAB_10009f01;
  }
  hModule = (HMODULE)FUN_10009d30();
LAB_10009f01:
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
    if (DAT_10027ba4 == '\0') {
      DAT_10027ba4 = '\x01';
      GetCurrentProcessId();
      _sprintf(acStack_504,"VALVE-MODULE-DEBUG-%08X");
      CreateFileMappingA((HANDLE)0xffffffff,(LPSECURITY_ATTRIBUTES)0x0,4,0,0x400,acStack_504);
    }
  }
  return hModule;
}


// 10009fb0 FUN_10009fb0

void __cdecl FUN_10009fb0(HMODULE param_1)

{
  if (param_1 != (HMODULE)0x0) {
    FreeLibrary(param_1);
  }
  return;
}


// 10009fc0 FUN_10009fc0

void __cdecl FUN_10009fc0(HMODULE param_1)

{
  if (param_1 == (HMODULE)0x0) {
    return;
  }
  GetProcAddress(param_1,"CreateInterface");
  return;
}


// 10009fe0 FUN_10009fe0

int __fastcall FUN_10009fe0(int param_1)

{
  if (*(int *)(param_1 + 0xc) != 0) {
    return *(int *)(param_1 + 0xc) + -1;
  }
  return 0;
}


// 10009ff0 FUN_10009ff0

undefined1 * __fastcall FUN_10009ff0(undefined4 *param_1)

{
  undefined1 *puVar1;
  
  puVar1 = &DAT_1001f5e8;
  if (param_1[3] != 0) {
    puVar1 = (undefined1 *)*param_1;
  }
  return puVar1;
}


// 1000a000 FUN_1000a000

void __thiscall FUN_1000a000(void *this,void *param_1,int param_2)

{
  size_t _Size;
  void *_Dst;
  
  if (param_1 == (void *)0x0) {
    param_2 = 0;
  }
  *(int *)((int)this + 0xc) = param_2;
  if (*(int *)((int)this + 4) < param_2) {
    FUN_10001330(this,param_2 - *(int *)((int)this + 4));
    if (*(int *)((int)this + 4) < param_2) {
      *(int *)((int)this + 0xc) = *(int *)((int)this + 4);
    }
  }
  _Size = *(size_t *)((int)this + 0xc);
  if (_Size != 0) {
                    /* WARNING: Load size is inaccurate */
    _Dst = *this;
    if ((_Dst < (void *)((int)param_1 + param_2)) && (param_1 < (void *)((int)_Dst + _Size))) {
      FID_conflict__memcpy(_Dst,param_1,_Size);
      return;
    }
    _memcpy(_Dst,param_1,_Size);
  }
  return;
}


// 1000a070 FUN_1000a070

void * __thiscall FUN_1000a070(void *this,char *param_1)

{
  int iVar1;
  
  if (param_1 != (char *)0x0) {
    iVar1 = FUN_100075f0(param_1);
    FUN_1000a000(this,param_1,iVar1 + 1);
    return this;
  }
  FUN_1000a000(this,(void *)0x0,0);
  return this;
}


// 1000a0b0 FUN_1000a0b0

undefined4 * __thiscall FUN_1000a0b0(void *this,char *param_1)

{
  int iVar1;
  
  iVar1 = 0;
  *(undefined4 *)this = 0;
  *(undefined4 *)((int)this + 4) = 0;
  *(undefined4 *)((int)this + 8) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  if (param_1 != (char *)0x0) {
    iVar1 = FUN_100075f0(param_1);
    iVar1 = iVar1 + 1;
  }
  FUN_1000a000(this,param_1,iVar1);
  return (undefined4 *)this;
}


// 1000a0f0 FUN_1000a0f0

undefined4 * __thiscall FUN_1000a0f0(void *this,int *param_1)

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
    if (pcVar2 == (char *)0x0) goto LAB_1000a123;
  }
  iVar1 = FUN_100075f0(pcVar2);
  iVar1 = iVar1 + 1;
LAB_1000a123:
  FUN_1000a000(this,pcVar2,iVar1);
  return (undefined4 *)this;
}


// 1000a140 FUN_1000a140

void __fastcall FUN_1000a140(undefined4 *param_1)

{
  *param_1 = 0;
  param_1[1] = 0;
  param_1[2] = 0;
  param_1[3] = 0;
  return;
}


// 1000a150 FUN_1000a150

undefined * FUN_1000a150(void)

{
  return &DAT_10027bb0;
}


// 1000a160 FUN_1000a160

undefined * FUN_1000a160(void)

{
  return &DAT_100285c0;
}


// 1000a190 FUN_1000a190

undefined4 * __thiscall
FUN_1000a190(void *this,undefined1 param_1,char *param_2,int param_3,int param_4)

{
  int *piVar1;
  char cVar2;
  char *pcVar3;
  int iVar4;
  int iVar5;
  
  *(undefined1 *)((int)this + 4) = param_1;
  *(undefined ***)this = &PTR_FUN_10020818;
  *(char **)((int)this + 8) = param_2;
  *(int *)((int)this + 0x10) = param_3;
  iVar4 = FUN_100075f0(param_2);
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
      iVar4 = FUN_100075f0(pcVar3);
      *piVar1 = iVar4;
      if (*(int *)((int)this + 0x14) < iVar4) {
        *(int *)((int)this + 0x14) = iVar4;
      }
      iVar5 = iVar5 + 1;
    } while (iVar5 < param_3);
  }
  return (undefined4 *)this;
}


// 1000a220 FUN_1000a220

uint __thiscall FUN_1000a220(void *this,byte *param_1,undefined4 *param_2)

{
  undefined4 uVar1;
  uint in_EAX;
  int iVar2;
  
  iVar2 = 0;
  if (0 < *(int *)((int)this + 0x10)) {
    do {
      in_EAX = FUN_10007620(param_1,*(byte **)((int)this +
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


// 1000a280 FUN_1000a280

undefined1 FUN_1000a280(void)

{
  return 0;
}


// 1000a2a0 FUN_1000a2a0

undefined1 __thiscall FUN_1000a2a0(void *this,int param_1)

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


// 1000a300 FUN_1000a300

uint __thiscall FUN_1000a300(void *this,int param_1,int *param_2)

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
    FUN_1000a2a0(this,*param_2 + param_1);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
  }
  iVar2 = *(int *)((int)this + 0x1c) - *(int *)((int)this + 0xc);
  if (iVar2 < *param_2) {
    *param_2 = iVar2;
  }
  return (uint)(*param_2 != 0);
}


// 1000a370 FUN_1000a370

int __thiscall FUN_1000a370(void *this,int param_1,int param_2)

{
  char cVar1;
  
  if ((*(byte *)((int)this + 0x14) & 2) == 0) {
    cVar1 = FUN_1000a2a0(this,param_1 + param_2);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
    if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
      return (*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this + param_2;
    }
  }
  return 0;
}


// 1000a3b0 FUN_1000a3b0

void __thiscall FUN_1000a3b0(void *this,int param_1,int param_2)

{
  int iVar1;
  
  if (param_1 != 0) {
    if (param_1 == 1) {
      *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
      goto LAB_1000a3dc;
    }
    if (param_1 != 2) goto LAB_1000a3dc;
    param_2 = *(int *)((int)this + 0x1c) - param_2;
  }
  *(int *)((int)this + 0xc) = param_2;
LAB_1000a3dc:
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


// 1000a410 FUN_1000a410

uint __thiscall FUN_1000a410(void *this,int param_1)

{
  uint in_EAX;
  
  if (((*(byte *)((int)this + 0x14) & 1) == 0) && ((*(byte *)((int)this + 0x15) & 8) == 0)) {
    if ((*(int *)((int)this + 0x10) < *(int *)((int)this + 0x20)) ||
       (in_EAX = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1,
       *(int *)((int)this + 4) < (int)in_EAX)) {
      in_EAX = (**(code **)((int)this + 0x28))(param_1);
      if ((char)in_EAX == '\0') {
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 1;
        goto LAB_1000a446;
      }
    }
    return CONCAT31((int3)(in_EAX >> 8),1);
  }
LAB_1000a446:
  return in_EAX & 0xffffff00;
}


// 1000a460 FUN_1000a460

void __fastcall FUN_1000a460(int *param_1)

{
  undefined4 uVar1;
  
  if (param_1[7] < param_1[4]) {
    if (((*(byte *)((int)param_1 + 0x15) & 8) == 0) && ((*(byte *)(param_1 + 5) & 1) == 0)) {
      uVar1 = FUN_1000a410(param_1,1);
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


// 1000a4b0 FUN_1000a4b0

void __thiscall FUN_1000a4b0(void *this,undefined4 param_1)

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


// 1000a500 FUN_1000a500

void __thiscall FUN_1000a500(void *this,void *param_1,size_t param_2)

{
  char cVar1;
  
  cVar1 = FUN_1000a2a0(this,param_2);
  if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
    _memcpy(param_1,(void *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this),
            param_2);
    *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + param_2;
  }
  return;
}


// 1000a540 FUN_1000a540

void __fastcall FUN_1000a540(int *param_1)

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


// 1000a5b0 FUN_1000a5b0

int __thiscall FUN_1000a5b0(void *this,int param_1)

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


// 1000a650 FUN_1000a650

int __fastcall FUN_1000a650(int *param_1)

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
    local_8 = FUN_1000a5b0(param_1,0);
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
      if (iVar6 < 1) goto LAB_1000a755;
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
LAB_1000a755:
      local_8 = local_8 + iVar6;
    }
  }
  if (local_8 != iVar1) {
    return (local_8 - iVar1) + 1;
  }
  return 0;
}


// 1000a7b0 FUN_1000a7b0

int __fastcall FUN_1000a7b0(int *param_1)

{
  int iVar1;
  int iVar2;
  char cVar3;
  int iVar4;
  int iVar5;
  int iVar6;
  
  if ((char)param_1[5] != '\0') {
    return 0;
  }
  iVar5 = 0;
  while( true ) {
    iVar4 = param_1[3];
    iVar2 = param_1[7];
    iVar6 = 0x80;
    if (iVar2 <= iVar4 + iVar5) break;
    if (iVar2 < iVar4 + 0x80 + iVar5) {
      iVar6 = (iVar2 - iVar4) - iVar5;
    }
    if ((*(byte *)(param_1 + 5) & 2) == 0) {
      iVar1 = iVar6 + iVar5;
      if (iVar2 < iVar4 + iVar1) {
        *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) | 2;
      }
      else if ((iVar4 < param_1[8]) || (param_1[1] < (iVar4 - param_1[8]) + iVar1)) {
        cVar3 = (*(code *)param_1[9])(iVar1);
        if (cVar3 == '\0') {
          *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) | 2;
        }
      }
      *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) & 0xfd;
    }
    iVar4 = param_1[7] - param_1[3];
    if (iVar4 < iVar6) {
      iVar6 = iVar4;
    }
    if (iVar6 == 0) break;
    iVar4 = 0;
    if (0 < iVar6) {
      do {
        cVar3 = *(char *)(iVar4 + (param_1[3] - param_1[8]) + *param_1 + iVar5);
        if ((cVar3 == '\n') || (cVar3 == '\r')) {
          return iVar4 + 2 + iVar5;
        }
        if (cVar3 == '\0') {
          return iVar4 + 1 + iVar5;
        }
        iVar4 = iVar4 + 1;
      } while (iVar4 < iVar6);
    }
    iVar5 = iVar5 + iVar6;
  }
  if (iVar5 == 0) {
    return 0;
  }
  return iVar5 + 1;
}


// 1000a8a0 FUN_1000a8a0

uint __thiscall FUN_1000a8a0(void *this,int param_1,char *param_2,int param_3)

{
  char cVar1;
  uint in_EAX;
  undefined3 extraout_var;
  int iVar2;
  
  if ((*(byte *)((int)this + 0x14) & 2) == 0) {
    cVar1 = FUN_1000a2a0(this,param_1 + param_3);
    in_EAX = CONCAT31(extraout_var,cVar1);
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
    if (cVar1 != '\0') {
                    /* WARNING: Load size is inaccurate */
      iVar2 = FUN_10007720((char *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this
                                   + param_1),param_2,param_3);
      return (uint)(iVar2 == 0);
    }
  }
  return in_EAX & 0xffffff00;
}


// 1000a900 FUN_1000a900

void __thiscall FUN_1000a900(void *this,undefined1 *param_1,int param_2)

{
  int iVar1;
  
  if (*(char *)((int)this + 0x14) != '\0') {
    *param_1 = 0;
    return;
  }
  if (param_2 == 0) {
    param_2 = 0x7fffffff;
  }
  iVar1 = FUN_1000a7b0((int *)this);
  if (iVar1 == 0) {
    *param_1 = 0;
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
    return;
  }
  if (iVar1 <= param_2) {
    FUN_1000a500(this,param_1,iVar1 - 1);
    param_1[iVar1 + -1] = 0;
    return;
  }
  FUN_1000a500(this,param_1,param_2 - 1);
  param_1[param_2 + -1] = 0;
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + (iVar1 - param_2) + -1;
  iVar1 = *(int *)((int)this + 0xc);
  if (*(int *)((int)this + 0x1c) < iVar1) {
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
    return;
  }
  *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
  if ((iVar1 < *(int *)((int)this + 0x20)) ||
     (*(int *)((int)this + 4) + *(int *)((int)this + 0x20) <= iVar1)) {
    (**(code **)((int)this + 0x24))(0xffffffff);
  }
  return;
}


// 1000a9b0 FUN_1000a9b0

void __thiscall FUN_1000a9b0(void *this,void *param_1,size_t param_2)

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
    FUN_1000a460((int *)this);
  }
  return;
}


// 1000aa20 FUN_1000aa20

uint __thiscall FUN_1000aa20(void *this,int param_1)

{
  uint in_EAX;
  int iVar1;
  
  if (*(int *)((int)this + 8) < 0) {
    if ((*(byte *)((int)this + 0x15) & 2) == 0) {
      return in_EAX & 0xffffff00;
    }
    FUN_1000a4b0(this,0);
  }
  iVar1 = (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1;
  if (*(int *)((int)this + 4) < iVar1) {
    do {
      iVar1 = FUN_10001330(this,1);
    } while (*(int *)((int)this + 4) <
             (*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + param_1);
  }
  return CONCAT31((int3)((uint)iVar1 >> 8),1);
}


// 1000aa70 FUN_1000aa70

int * __thiscall FUN_1000aa70(void *this,int param_1,int param_2,byte param_3)

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
    FUN_1000a460((int *)this);
  }
  *(code **)((int)this + 0x24) = FUN_1000a280;
  *(code **)((int)this + 0x28) = FUN_1000aa20;
  return (int *)this;
}


// 1000aaf0 FUN_1000aaf0

int * __thiscall FUN_1000aaf0(void *this,int param_1,int param_2,byte param_3)

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
      uVar1 = FUN_1000a410(this,1);
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
  *(code **)((int)this + 0x24) = FUN_1000a280;
  *(code **)((int)this + 0x28) = FUN_1000aa20;
  return (int *)this;
}


// 1000ab70 FUN_1000ab70

void __thiscall FUN_1000ab70(void *this,int param_1,int param_2,int param_3,byte param_4)

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
      uVar1 = FUN_1000a410(this,1);
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


// 1000ac10 FUN_1000ac10

void __thiscall FUN_1000ac10(void *this,int param_1,int param_2,int param_3,byte param_4)

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
  *(int *)this = param_1;
  *(int *)((int)this + 4) = param_2;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(int *)((int)this + 0x10) = param_3;
  *(undefined4 *)((int)this + 0x18) = 0;
  *(undefined1 *)((int)this + 0x14) = 0;
  *(undefined4 *)((int)this + 0x20) = 0;
  *(byte *)((int)this + 0x15) = param_4;
  *(undefined4 *)((int)this + 0x1c) = 0xffffffff;
  if (-1 < param_3) {
    if ((param_4 & 8) == 0) {
      uVar1 = FUN_1000a410(this,1);
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


// 1000aca0 FUN_1000aca0

void __thiscall FUN_1000aca0(void *this,char *param_1)

{
  int iVar1;
  char *pcVar2;
  size_t sVar3;
  
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    if (param_1 != (char *)0x0) {
      iVar1 = FUN_100075f0(param_1);
      FUN_1000a9b0(this,param_1,iVar1 + 1);
      return;
    }
    FUN_10003230(this,0);
    return;
  }
  if (param_1 != (char *)0x0) {
    if (((*(byte *)((int)this + 0x15) & 0x10) == 0) && (0 < *(int *)((int)this + 0x18))) {
                    /* WARNING: Load size is inaccurate */
      if ((*(int *)((int)this + 0x10) != 0) &&
         (*(char *)((*(int *)((int)this + 0x10) - *(int *)((int)this + 0x20)) + -1 + *this) == '\n')
         ) {
        FUN_10003930((int *)this);
      }
      pcVar2 = _strchr(param_1,10);
      while (pcVar2 != (char *)0x0) {
        FUN_1000a9b0(this,param_1,(size_t)(pcVar2 + (1 - (int)param_1)));
        param_1 = pcVar2 + 1;
        if (pcVar2[1] == '\0') break;
        if ((*(byte *)((int)this + 0x15) & 0x10) == 0) {
          iVar1 = *(int *)((int)this + 0x18);
        }
        else {
          iVar1 = 0;
        }
        while (iVar1 = iVar1 + -1, -1 < iVar1) {
          FUN_10003230(this,9);
        }
        pcVar2 = _strchr(param_1,10);
      }
    }
    sVar3 = FUN_100075f0(param_1);
    if (sVar3 != 0) {
      FUN_1000a9b0(this,param_1,sVar3);
    }
  }
  return;
}


// 1000ad90 FUN_1000ad90

uint __fastcall FUN_1000ad90(int *param_1)

{
  undefined3 uVar1;
  char cVar2;
  char cVar3;
  char *in_EAX;
  undefined3 extraout_var;
  undefined3 extraout_var_00;
  undefined3 extraout_var_01;
  
  if (((*(byte *)((int)param_1 + 0x15) & 1) != 0) && ((char)param_1[5] == '\0')) {
    cVar2 = FUN_1000a2a0(param_1,2);
    in_EAX = (char *)CONCAT31(extraout_var,cVar2);
    *(byte *)(param_1 + 5) = *(byte *)(param_1 + 5) & 0xfd;
    if (cVar2 != '\0') {
      in_EAX = (char *)((param_1[3] - param_1[8]) + *param_1);
      if (((in_EAX != (char *)0x0) && (*in_EAX == '/')) && (in_EAX[1] == '/')) {
        param_1[3] = param_1[3] + 2;
        cVar3 = FUN_100093d0(param_1);
        cVar2 = (char)param_1[5];
        uVar1 = extraout_var_00;
        while ((cVar2 == '\0' && (cVar3 != '\n'))) {
          cVar3 = FUN_100093d0(param_1);
          cVar2 = (char)param_1[5];
          uVar1 = extraout_var_01;
        }
        return CONCAT31(uVar1,1);
      }
    }
  }
  return (uint)in_EAX & 0xffffff00;
}


// 1000ae00 FUN_1000ae00

void __thiscall FUN_1000ae00(void *this,undefined1 *param_1,int param_2)

{
  int iVar1;
  
  if (*(char *)((int)this + 0x14) != '\0') {
    *param_1 = 0;
    return;
  }
  if (param_2 == 0) {
    param_2 = 0x7fffffff;
  }
  iVar1 = FUN_1000a650((int *)this);
  if ((*(byte *)((int)this + 0x15) & 1) != 0) {
    FUN_1000a540((int *)this);
  }
  if (iVar1 == 0) {
    *param_1 = 0;
    *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
    return;
  }
  if (param_2 < iVar1) {
    FUN_1000a500(this,param_1,param_2 - 1);
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
    FUN_1000a500(this,param_1,iVar1 - 1);
    param_1[iVar1 + -1] = 0;
  }
  if ((*(byte *)((int)this + 0x15) & 1) == 0) {
    FUN_100093d0((int *)this);
  }
  return;
}


// 1000aec0 FUN_1000aec0

uint __thiscall FUN_1000aec0(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  char cVar2;
  undefined3 extraout_var;
  uint uVar3;
  int iVar4;
  int unaff_EBX;
  
  cVar2 = FUN_100093d0((int *)this);
  puVar1 = param_1;
  iVar4 = CONCAT31(extraout_var,cVar2);
  if (cVar2 == *(char *)(param_1 + 1)) {
    param_1 = (undefined4 *)param_1[5];
    uVar3 = FUN_1000a300(this,0,(int *)&param_1);
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


// 1000af50 FUN_1000af50

void __thiscall FUN_1000af50(void *this,undefined4 *param_1,undefined1 *param_2,int param_3)

{
  char cVar1;
  int iVar2;
  uint uVar3;
  int iVar4;
  
  if (((*(byte *)((int)this + 0x15) & 1) == 0) || (param_1 == (undefined4 *)0x0)) {
    FUN_1000ae00(this,param_2,param_3);
  }
  else {
    if (*(char *)((int)this + 0x14) != '\0') {
      *param_2 = 0;
      return;
    }
    if (param_3 == 0) {
      param_3 = 0x7fffffff;
    }
    FUN_1000a540((int *)this);
    uVar3 = FUN_1000a8a0(this,0,(char *)param_1[2],param_1[3]);
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
LAB_1000b04a:
          if (param_3 <= iVar4) {
            iVar4 = param_3 + -1;
          }
          param_2[iVar4] = 0;
          return;
        }
        uVar3 = FUN_1000a8a0(this,0,(char *)param_1[2],param_1[3]);
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
          goto LAB_1000b04a;
        }
        uVar3 = FUN_1000aec0(this,param_1);
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


// 1000b080 FUN_1000b080

int __thiscall FUN_1000b080(void *this,char *param_1,int param_2)

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
        FUN_1000a540((int *)this);
        goto LAB_1000b2fb;
      }
      if (cVar5 != '%') {
        if ((*(byte *)((int)this + 0x14) & 2) != 0) {
          return local_8;
        }
        cVar4 = FUN_1000a2a0(this,1);
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
        if (cVar4 == '\0') {
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        if (cVar5 != *(char *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this)) {
          return local_8;
        }
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
        goto LAB_1000b2fb;
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
LAB_1000b326:
          *puVar2 = 0;
          return local_8;
        }
        cVar5 = FUN_1000a2a0(this,1);
        *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
        if (cVar5 == '\0') goto LAB_1000b326;
                    /* WARNING: Load size is inaccurate */
        *puVar2 = *(undefined1 *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc));
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
        local_8 = local_8 + 1;
        break;
      case 'd':
      case 'i':
        puVar9 = *(ulong **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000a300(this,0,&param_2);
        if ((char)uVar6 == '\0') {
LAB_1000b337:
          *puVar9 = 0;
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        uVar7 = _strtol((char *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc)),
                        &local_4,10);
        goto LAB_1000b1c6;
      default:
        goto switchD_1000b142_caseD_65;
      case 'f':
        pfVar3 = *(float **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000a300(this,0,&param_2);
        if ((char)uVar6 == '\0') {
          *pfVar3 = 0.0;
          return local_8;
        }
                    /* WARNING: Load size is inaccurate */
        fVar11 = (float10)FUN_10010bc4((byte *)((*this - *(int *)((int)this + 0x20)) +
                                               *(int *)((int)this + 0xc)),&local_4);
        *pfVar3 = (float)fVar11;
        goto LAB_1000b1cc;
      case 's':
        puVar1 = (undefined4 *)(iVar10 + 4);
        iVar10 = iVar10 + 4;
        FUN_1000ae00(this,(undefined1 *)*puVar1,0);
        local_8 = local_8 + 1;
        break;
      case 'u':
        puVar9 = *(ulong **)(iVar10 + 4);
        param_2 = 0x80;
        uVar6 = FUN_1000a300(this,0,&param_2);
        if ((char)uVar6 == '\0') goto LAB_1000b337;
                    /* WARNING: Load size is inaccurate */
        uVar7 = _strtoul((char *)((*this - *(int *)((int)this + 0x20)) + *(int *)((int)this + 0xc)),
                         &local_4,10);
LAB_1000b1c6:
        *puVar9 = uVar7;
LAB_1000b1cc:
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
        uVar6 = FUN_1000a300(this,0,&param_2);
        if ((char)uVar6 == '\0') goto LAB_1000b337;
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
LAB_1000b2fb:
      cVar5 = *pcVar8;
      param_1 = pcVar8;
    } while (cVar5 != '\0');
  }
switchD_1000b142_caseD_65:
  return local_8;
}


// 1000b3a0 FUN_1000b3a0

void __cdecl FUN_1000b3a0(void *param_1,char *param_2)

{
  FUN_1000b080(param_1,param_2,(int)&stack0x0000000c);
  return;
}


// 1000b3c0 FUN_1000b3c0

int __thiscall FUN_1000b3c0(void *this,int param_1,char *param_2,int param_3,char param_4)

{
  char cVar1;
  int iVar2;
  char cVar3;
  uint uVar4;
  int iVar5;
  
  *param_2 = '\0';
  cVar3 = *(char *)((int)this + 0x14);
  while( true ) {
    if (cVar3 != '\0') {
      return -1;
    }
    FUN_1000a540((int *)this);
    if ((param_4 == '\0') || (uVar4 = FUN_1000ad90((int *)this), (char)uVar4 == '\0')) break;
    cVar3 = *(char *)((int)this + 0x14);
  }
  cVar3 = FUN_100093d0((int *)this);
  if (cVar3 == '\0') {
    return -1;
  }
  if (cVar3 != '\"') {
    cVar1 = *(char *)(cVar3 + param_1);
    *param_2 = cVar3;
    if (cVar1 != '\0') {
      param_2[1] = '\0';
      return 1;
    }
    iVar5 = 1;
    if (param_3 != 1) {
      do {
        cVar3 = FUN_100093d0((int *)this);
        if (*(char *)((int)this + 0x14) != '\0') goto LAB_1000b539;
        if (((*(char *)(cVar3 + param_1) != '\0') || (cVar3 == '\"')) || (cVar3 < '!')) {
          *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + -1;
          iVar2 = *(int *)((int)this + 0xc);
          if (*(int *)((int)this + 0x1c) < iVar2) {
            *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) | 2;
            param_2[iVar5] = '\0';
            return iVar5;
          }
          *(byte *)((int)this + 0x14) = *(byte *)((int)this + 0x14) & 0xfd;
          if ((iVar2 < *(int *)((int)this + 0x20)) ||
             (*(int *)((int)this + 4) + *(int *)((int)this + 0x20) <= iVar2)) {
            (**(code **)((int)this + 0x24))(0xffffffff);
          }
          goto LAB_1000b539;
        }
        param_2[iVar5] = cVar3;
        iVar5 = iVar5 + 1;
      } while (iVar5 != param_3);
    }
LAB_1000b4f1:
    param_2[iVar5 + -1] = '\0';
    return param_3;
  }
  iVar5 = 0;
  if (*(char *)((int)this + 0x14) == '\0') {
    do {
      if ((*(byte *)((int)this + 0x15) & 1) == 0) {
        cVar3 = FUN_1000a2a0(this,1);
        if (cVar3 == '\0') break;
                    /* WARNING: Load size is inaccurate */
        param_4 = *(char *)((*(int *)((int)this + 0xc) - *(int *)((int)this + 0x20)) + *this);
        *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
      }
      else {
        param_4 = '\0';
        FUN_1000b3a0(this,"%c");
      }
      if ((param_4 == '\"') || (param_4 == '\0')) break;
      param_2[iVar5] = param_4;
      iVar5 = iVar5 + 1;
      if (iVar5 == param_3) goto LAB_1000b4f1;
      if (*(char *)((int)this + 0x14) != '\0') {
        param_2[iVar5] = '\0';
        return iVar5;
      }
    } while( true );
  }
LAB_1000b539:
  param_2[iVar5] = '\0';
  return iVar5;
}


// 1000b550 FUN_1000b550

void __cdecl FUN_1000b550(undefined4 param_1,undefined **param_2)

{
  int *piVar1;
  int *piVar2;
  
  if ((DAT_10029034 != (int *)0x0) && (DAT_10028fdc == '\0')) {
    DAT_10028fd8 = param_1;
    DAT_10028fdc = '\x01';
    DAT_10024454 = (**(code **)(*DAT_10029034 + 0x14))();
    piVar2 = DAT_10028fd0;
    DAT_10028fd4 = param_2;
    if (param_2 == (undefined **)0x0) {
      DAT_10028fd4 = &PTR_PTR_10024460;
    }
    while (piVar2 != (int *)0x0) {
      piVar1 = (int *)piVar2[1];
      (**(code **)(*piVar2 + 0xc))(DAT_10028fd8);
      (**(code **)(*piVar2 + 0x2c))();
      piVar2 = piVar1;
    }
    (**(code **)(*DAT_10029034 + 0x70))();
    (**(code **)(*DAT_10029034 + 0x94))();
    DAT_10028fd0 = (int *)0x0;
  }
  return;
}


// 1000b600 FUN_1000b600

void FUN_1000b600(void)

{
  if ((DAT_10029034 != (int *)0x0) && (DAT_10028fdc != '\0')) {
    (**(code **)(*DAT_10029034 + 0x74))(DAT_10024454);
    (**(code **)(*DAT_10029034 + 0x20))(DAT_10024454);
    DAT_10024454 = 0xffffffff;
    DAT_10028fdc = '\0';
  }
  return;
}


// 1000b6c0 FUN_1000b6c0

void __fastcall FUN_1000b6c0(undefined4 param_1)

{
  if (DAT_10028fd4 != (undefined4 *)0x0) {
    (**(code **)*DAT_10028fd4)(param_1);
  }
  return;
}


// 1000b7d0 FUN_1000b7d0

void __fastcall FUN_1000b7d0(int *param_1)

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
  if ((cVar1 != '\0') && (DAT_10029034 != (int *)0x0)) {
    cVar1 = (**(code **)(*DAT_10029034 + 0x80))();
    if (cVar1 == '\0') {
      (**(code **)(*DAT_10029034 + 0x8c))(param_1,unaff_retaddr);
      return;
    }
  }
  aiStack_28[0] = param_1[0xb];
  piVar4 = unaff_retaddr;
  if (unaff_retaddr == (int *)0x0) {
    piVar4 = (int *)&DAT_1001f5e8;
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
    FUN_10007e60((char *)aiStack_28,0x20,"%f");
    piVar4 = aiStack_28;
    uVar2 = extraout_ECX_00;
    uVar3 = extraout_EDX_00;
  }
  param_1[0xb] = 0xb00000;
  uVar5 = FUN_10010690(uVar2,uVar3);
  param_1[0xc] = (int)uVar5;
  if ((param_1[5] & 0x1000U) == 0) {
    (**(code **)(*param_1 + 0x40))(piVar4,fVar7);
  }
  return;
}


// 1000b8b0 FUN_1000b8b0

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

void __thiscall FUN_1000b8b0(void *this,undefined4 param_1,undefined4 param_2)

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
  
  asStack_1c[2] = 0x1000b8c6;
  iVar2 = -(*(int *)((int)this + 0x28) + 0xfU & 0xfffffff0);
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x28);
  *(undefined4 *)((int)asStack_1c + iVar2 + 4) = uVar4;
  *(undefined1 **)((int)asStack_1c + iVar2) = &stack0xfffffff0 + iVar2;
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = 0x1000b8d3;
  _memcpy(*(void **)((int)asStack_1c + iVar2),*(void **)((int)asStack_1c + iVar2 + 4),
          *(size_t *)((int)asStack_1c + iVar2 + 8));
  *(undefined4 *)((int)auStack_24 + iVar2 + 4) = param_1;
  *(undefined4 *)((int)auStack_24 + iVar2) = 0x1000b8dc;
  iVar3 = FUN_100075f0(*(char **)((int)auStack_24 + iVar2 + 4));
  iVar3 = iVar3 + 1;
  puVar6 = &stack0xfffffff0 + iVar2;
  if (*(int *)((int)this + 0x28) < iVar3) {
    if (*(int *)((int)this + 0x24) != 0) {
      *(int *)((int)asStack_1c + iVar2 + 8) = *(int *)((int)this + 0x24);
      *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x1000b8f6;
      FUN_10005520(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    }
    puVar6 = &stack0xfffffff0 + iVar2;
    *(int *)((int)asStack_1c + iVar2 + 8) = iVar3;
    *(undefined4 *)((int)asStack_1c + iVar2 + 4) = 0x1000b8ff;
    uVar4 = FUN_10005720(*(undefined4 *)((int)asStack_1c + iVar2 + 8));
    *(undefined4 *)((int)this + 0x24) = uVar4;
    *(int *)((int)this + 0x28) = iVar3;
  }
  uVar4 = *(undefined4 *)((int)this + 0x24);
  *(int *)(puVar6 + -4) = iVar3;
  *(undefined4 *)(puVar6 + -8) = param_1;
  *(undefined4 *)(puVar6 + -0xc) = uVar4;
  *(undefined4 *)(puVar6 + -0x10) = 0x1000b916;
  _memcpy(*(void **)(puVar6 + -0xc),*(void **)(puVar6 + -8),*(size_t *)(puVar6 + -4));
  pcVar1 = *(code **)((int)this + 0x44);
  if (pcVar1 != (code *)0x0) {
    *(undefined4 *)(puVar6 + -4) = extraout_ECX;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(int *)(puVar6 + -0xc) = (int)this + 0x18;
    puVar5 = puVar6 + -0x10;
    *(undefined4 *)(puVar6 + -0x10) = 0x1000b92e;
    (*pcVar1)();
    puVar6 = puVar5 + 0xc;
  }
  if (DAT_10029034 != (int *)0x0) {
    pcVar1 = *(code **)(*DAT_10029034 + 0x48);
    *(int **)(puVar6 + -4) = DAT_10029034;
    *(undefined4 *)(puVar6 + -4) = param_2;
    *(undefined1 **)(puVar6 + -8) = &stack0xfffffff0 + iVar2;
    *(void **)(puVar6 + -0xc) = this;
    *(undefined4 *)(puVar6 + -0x10) = 0x1000b94b;
    (*pcVar1)();
  }
  return;
}


// 1000b9b0 FUN_1000b9b0

void __thiscall FUN_1000b9b0(void *this,float param_1)

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
    if (((cVar1 != '\0') && (DAT_10029034 != (int *)0x0)) &&
       (cVar1 = (**(code **)(*DAT_10029034 + 0x80))(), cVar1 == '\0')) {
      (**(code **)(*DAT_10029034 + 0x84))();
      return;
    }
                    /* WARNING: Load size is inaccurate */
    (**(code **)(*this + 0x3c))(&stack0x00000000);
    uVar3 = *(undefined4 *)((int)this + 0x2c);
    *(undefined4 *)((int)this + 0x2c) = uStack_4;
    uVar2 = FUN_10010690(extraout_ECX,extraout_EDX);
    *(int *)((int)this + 0x30) = (int)uVar2;
    if ((*(uint *)((int)this + 0x14) & 0x1000) == 0) {
      FUN_10007e60(&stack0xffffffd8,0x20,"%f");
                    /* WARNING: Load size is inaccurate */
      (**(code **)(*this + 0x40))(&stack0xffffffd8,uVar3);
      return;
    }
  }
  return;
}


// 1000bce0 FUN_1000bce0

undefined4 * __thiscall FUN_1000bce0(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_FUN_100208ac;
  *(undefined ***)((int)this + 0x18) = &PTR_LAB_10020888;
  if (*(int *)((int)this + 0x24) != 0) {
    FUN_10005520(*(int *)((int)this + 0x24));
    *(undefined4 *)((int)this + 0x24) = 0;
  }
  *(undefined ***)this = &PTR_LAB_10020834;
  if ((param_1 & 1) != 0) {
    FUN_10005520(this);
  }
  return (undefined4 *)this;
}


// 1000bd30 FUN_1000bd30

int * __fastcall FUN_1000bd30(int *param_1)

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
  param_1[6] = (int)&PTR_LAB_10020868;
  param_1[0xe] = 0;
  param_1[0x10] = 0;
  *param_1 = (int)&PTR_FUN_100208ac;
  param_1[6] = (int)&PTR_LAB_10020888;
  param_1[7] = (int)param_1;
  param_1[8] = (int)&DAT_1002096c;
  *(undefined1 *)(param_1 + 0xd) = 0;
  *(undefined1 *)(param_1 + 0xf) = 0;
  param_1[0x11] = 0;
  pcVar2 = "0";
  do {
    pcVar3 = pcVar2;
    pcVar2 = pcVar3 + 1;
  } while (*pcVar3 != '\0');
  param_1[10] = (int)(pcVar3 + -0x1002096b);
  _Dst = (void *)FUN_10005720(pcVar3 + -0x1002096b);
  param_1[9] = (int)_Dst;
  _memcpy(_Dst,(void *)param_1[8],param_1[10]);
  dVar5 = _atof((char *)param_1[9]);
  param_1[0xb] = (int)(float)dVar5;
  uVar4 = FUN_10010690(extraout_ECX,extraout_EDX);
  puVar1 = PTR_DAT_100244ac;
  param_1[0xc] = (int)uVar4;
  *(undefined1 *)(param_1 + 2) = 0;
  param_1[3] = (int)&DAT_1001f5e8;
  param_1[4] = (int)puVar1;
  param_1[5] = 2;
  param_1[1] = (int)DAT_10028fd0;
  DAT_10028fd0 = param_1;
  if (DAT_10028fd4 != 0) {
    (**(code **)(*param_1 + 0x2c))();
  }
  *param_1 = (int)&PTR_FUN_10020924;
  param_1[6] = (int)&PTR_LAB_10020900;
  return param_1;
}


// 1000be50 FUN_1000be50

void __thiscall FUN_1000be50(void *this,undefined4 param_1,char param_2)

{
  int iVar1;
  undefined *puVar2;
  
  if (DAT_10029034 == (int *)0x0) {
    puVar2 = &DAT_10029000;
  }
  else {
    iVar1 = (**(code **)(*DAT_10029034 + 0x34))(param_1);
    if (iVar1 == 0) {
      puVar2 = (undefined *)0x0;
    }
    else {
      puVar2 = (undefined *)(iVar1 + 0x18);
    }
  }
  *(undefined **)this = puVar2;
  if (puVar2 == (undefined *)0x0) {
    *(undefined **)this = &DAT_10029000;
  }
                    /* WARNING: Load size is inaccurate */
  *(undefined **)((int)this + 4) = *this + -0x18;
  if ((*this == &DAT_10029000) && ((DAT_10029034 != (int *)0x0 || (DAT_1002451f != '\0')))) {
    if (param_2 == '\0') {
      Warning("ConVarRef %s doesn\'t point to an existing ConVar\n",param_1);
    }
    DAT_1002451f = '\0';
  }
  return;
}


// 1000bed0 FUN_1000bed0

void * __thiscall FUN_1000bed0(void *this,undefined4 param_1)

{
  FUN_1000be50(this,param_1,'\0');
  return this;
}


// 1000bef0 FUN_1000bef0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_1000bef0(int param_1,int param_2)

{
  int iVar1;
  int iVar2;
  
  if (DAT_1002903c == '\0') {
    iVar2 = 0;
    DAT_1002903c = '\x01';
    iVar1 = DAT_10029034;
    if (0 < param_2) {
      do {
        if (iVar1 == 0) {
          iVar1 = (**(code **)(param_1 + iVar2 * 4))("VEngineCvar007",0);
          _DAT_10029030 = iVar1;
          DAT_10029034 = iVar1;
        }
        if (DAT_10029038 == 0) {
          DAT_10029038 = (**(code **)(param_1 + iVar2 * 4))("VProcessUtils001",0);
          iVar1 = DAT_10029034;
        }
        iVar2 = iVar2 + 1;
      } while (iVar2 < param_2);
    }
  }
  return;
}


// 1000bf60 FUN_1000bf60

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_1000bf60(void)

{
  if (DAT_1002903c != '\0') {
    _DAT_10029030 = 0;
    DAT_10029034 = 0;
    DAT_10029038 = 0;
    DAT_1002903c = '\0';
  }
  return;
}


// 1000bf80 FUN_1000bf80

void __cdecl FUN_1000bf80(void *param_1,char *param_2)

{
  char cVar1;
  
  if ((param_1 != (void *)0x0) && (param_2 != (char *)0x0)) {
    _memset(param_1,0,0x100);
    cVar1 = *param_2;
    while (cVar1 != '\0') {
      param_2 = param_2 + 1;
      *(undefined1 *)((int)cVar1 + (int)param_1) = 1;
      cVar1 = *param_2;
    }
  }
  return;
}


// 1000bfd0 FUN_1000bfd0

void __fastcall FUN_1000bfd0(undefined4 *param_1)

{
  *param_1 = 0;
  return;
}


// 1000bfe0 FUN_1000bfe0

void FUN_1000bfe0(int *param_1)

{
  if ((int *)*param_1 != (int *)0x0) {
    FUN_1000bfe0((int *)*param_1);
    FUN_1000bfe0((int *)(*param_1 + 4));
    FUN_10005520(*param_1);
    *param_1 = 0;
  }
  return;
}


// 1000c020 FUN_1000c020

void __fastcall FUN_1000c020(int *param_1)

{
  if ((int *)*param_1 != (int *)0x0) {
    FUN_1000bfe0((int *)*param_1);
    FUN_1000bfe0((int *)(*param_1 + 4));
    FUN_10005520(*param_1);
    *param_1 = 0;
  }
  return;
}


// 1000c050 FUN_1000c050

int __cdecl FUN_1000c050(char *param_1)

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


// 1000c120 FUN_1000c120

undefined4 __fastcall FUN_1000c120(int param_1)

{
  return *(undefined4 *)(param_1 + 0x4c);
}


// 1000c130 FUN_1000c130

void __thiscall FUN_1000c130(void *this,undefined4 param_1,undefined4 param_2)

{
  *(undefined4 *)((int)this + 0x50) = param_1;
  *(undefined4 *)((int)this + 0x48) = param_2;
  return;
}


// 1000c170 FUN_1000c170

void __thiscall FUN_1000c170(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000c1cb;
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
LAB_1000c1cb:
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


// 1000c210 FUN_1000c210

void __thiscall FUN_1000c210(void *this,int param_1)

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
    if ((int)uVar4 <= iVar2) goto LAB_1000c26b;
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
LAB_1000c26b:
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


// 1000c2a0 FUN_1000c2a0

void __thiscall FUN_1000c2a0(void *this,int param_1)

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


// 1000c340 FUN_1000c340

ushort * __thiscall FUN_1000c340(void *this,undefined4 param_1,ushort *param_2,undefined1 *param_3)

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
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_10027500;
      }
      else {
        puVar3 = (ushort *)(*(int *)((int)this + 4) + iVar4);
      }
      uVar1 = puVar3[1];
    }
    else {
      *param_3 = 1;
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_10027500;
        uVar1 = DAT_10027500;
      }
      else {
        puVar3 = (ushort *)(*(int *)((int)this + 4) + iVar4);
        uVar1 = *(ushort *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  }
  return puVar3;
}


// 1000c450 FUN_1000c450

ushort __fastcall FUN_1000c450(int param_1)

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
    FUN_1000c2a0((void *)(param_1 + 4),1);
    uVar2 = *puVar1;
    if (((uVar2 != 0) && (uVar2 == 0)) || (iVar4 = *(int *)(param_1 + 8), iVar4 <= (int)(uint)uVar2)
       ) {
      iVar4 = *(int *)(param_1 + 8);
      puVar3 = &local_2;
    }
    else {
      puVar3 = (ushort *)FUN_10003060((void *)(param_1 + 4),(short *)&local_2,(short *)puVar1);
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


// 1000c570 FUN_1000c570

void __thiscall FUN_1000c570(void *this,ushort param_1)

{
  ushort uVar1;
  undefined2 *puVar2;
  short *psVar3;
  ushort *puVar4;
  
  puVar2 = FUN_100030f0(this,param_1);
  uVar1 = puVar2[1];
  puVar2 = FUN_100030f0(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + 2 + (uint)param_1 * 0x10) = *puVar2;
  psVar3 = FUN_100030f0(this,uVar1);
  if (*psVar3 != -1) {
    puVar4 = FUN_100030f0(this,uVar1);
    *(ushort *)((uint)*puVar4 * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar2 = FUN_100030f0(this,param_1);
    *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar2[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar2 = FUN_100030f0(this,param_1);
    puVar4 = FUN_100030f0(this,puVar2[2]);
    if (*puVar4 == param_1) {
      puVar2 = FUN_100030f0(this,param_1);
      *(ushort *)((uint)(ushort)puVar2[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
    }
    else {
      puVar2 = FUN_100030f0(this,param_1);
      *(ushort *)((uint)(ushort)puVar2[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
    }
  }
  *(ushort *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x10) = uVar1;
  }
  return;
}


// 1000c660 FUN_1000c660

void __thiscall FUN_1000c660(void *this,ushort param_1)

{
  ushort uVar1;
  ushort *puVar2;
  undefined2 *puVar3;
  
  puVar2 = FUN_100030f0(this,param_1);
  uVar1 = *puVar2;
  puVar3 = FUN_100030f0(this,uVar1);
  *(undefined2 *)(*(int *)((int)this + 4) + (uint)param_1 * 0x10) = puVar3[1];
  puVar3 = FUN_100030f0(this,uVar1);
  if (puVar3[1] != -1) {
    puVar3 = FUN_100030f0(this,uVar1);
    *(ushort *)((uint)(ushort)puVar3[1] * 0x10 + 4 + *(int *)((int)this + 4)) = param_1;
  }
  if (uVar1 != 0xffff) {
    puVar3 = FUN_100030f0(this,param_1);
    *(undefined2 *)((uint)uVar1 * 0x10 + 4 + *(int *)((int)this + 4)) = puVar3[2];
  }
  if (param_1 == *(ushort *)((int)this + 0x10)) {
    *(ushort *)((int)this + 0x10) = uVar1;
  }
  else {
    puVar3 = FUN_100030f0(this,param_1);
    puVar3 = FUN_100030f0(this,puVar3[2]);
    if (puVar3[1] == param_1) {
      puVar3 = FUN_100030f0(this,param_1);
      *(ushort *)((uint)(ushort)puVar3[2] * 0x10 + 2 + *(int *)((int)this + 4)) = uVar1;
    }
    else {
      puVar3 = FUN_100030f0(this,param_1);
      *(ushort *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4)) = uVar1;
    }
  }
  *(ushort *)((uint)uVar1 * 0x10 + 2 + *(int *)((int)this + 4)) = param_1;
  if (param_1 != 0xffff) {
    *(ushort *)(*(int *)((int)this + 4) + 4 + (uint)param_1 * 0x10) = uVar1;
  }
  return;
}


// 1000c760 FUN_1000c760

void __fastcall FUN_1000c760(int param_1)

{
  HMODULE pHVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  
  iVar4 = *(int *)(param_1 + 0x10) + -1;
  if (-1 < iVar4) {
    iVar3 = iVar4 * 0xc;
    do {
      pHVar1 = *(HMODULE *)(iVar3 + *(int *)(param_1 + 4));
      if (pHVar1 != (HMODULE)0x0) {
        FUN_10009fb0(pHVar1);
      }
      iVar2 = *(int *)(*(int *)(param_1 + 4) + 8 + iVar3);
      if (iVar2 != 0) {
        (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(iVar2);
      }
      iVar4 = iVar4 + -1;
      iVar3 = iVar3 + -0xc;
    } while (-1 < iVar4);
  }
  *(undefined4 *)(param_1 + 0x10) = 0;
  return;
}


// 1000c7c0 FUN_1000c7c0

int __thiscall FUN_1000c7c0(void *this,int param_1)

{
  void *pvVar1;
  int iVar2;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_1000c170(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  *(int *)((int)this + 0x10) = *this;
  iVar2 = (*(int *)((int)this + 0xc) - param_1) + -1;
  if (0 < iVar2) {
    pvVar1 = (void *)(*this + param_1 * 0xc);
    FUN_10007df0((void *)((int)pvVar1 + 0xc),pvVar1,iVar2 * 0xc);
  }
  return param_1;
}


// 1000c820 FUN_1000c820

int __thiscall FUN_1000c820(void *this,int param_1,undefined4 *param_2)

{
  void *pvVar1;
  undefined4 *puVar2;
  int iVar3;
  
  if (*(int *)((int)this + 4) < *(int *)((int)this + 0xc) + 1) {
    FUN_1000c210(this,(*(int *)((int)this + 0xc) - *(int *)((int)this + 4)) + 1);
  }
  *(int *)((int)this + 0xc) = *(int *)((int)this + 0xc) + 1;
                    /* WARNING: Load size is inaccurate */
  iVar3 = (*(int *)((int)this + 0xc) - param_1) + -1;
  *(int *)((int)this + 0x10) = *this;
  if (0 < iVar3) {
    pvVar1 = (void *)(*this + param_1 * 4);
    FUN_10007df0((void *)((int)pvVar1 + 4),pvVar1,iVar3 * 4);
  }
                    /* WARNING: Load size is inaccurate */
  puVar2 = (undefined4 *)(*this + param_1 * 4);
  if (puVar2 != (undefined4 *)0x0) {
    *puVar2 = *param_2;
  }
  return param_1;
}


// 1000c890 FUN_1000c890

ushort __thiscall FUN_1000c890(void *this,undefined4 param_1)

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
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      if (uVar3 == 0xffff) {
        puVar2 = &DAT_10027500;
      }
      else {
        puVar2 = (undefined2 *)(*(int *)((int)this + 4) + iVar4);
      }
      uVar3 = puVar2[1];
    }
    else {
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      bVar5 = uVar3 != 0xffff;
      uVar3 = DAT_10027500;
      if (bVar5) {
        uVar3 = *(ushort *)(*(int *)((int)this + 4) + iVar4);
      }
    }
  } while (uVar3 != 0xffff);
  return 0xffff;
}


// 1000c990 FUN_1000c990

void __thiscall FUN_1000c990(void *this,uint param_1)

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
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      sVar7 = (short)param_1;
      if (sVar7 == -1) {
        puVar3 = &DAT_10027500;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      if (puVar3[2] == 0xffff) {
        puVar3 = &DAT_10027500;
      }
      else {
        puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (puVar3[3] != 0) break;
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      if (sVar7 == -1) {
        puVar3 = &DAT_10027500;
      }
      else {
        puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
      }
      uVar1 = puVar3[2];
      uVar8 = (uint)uVar1;
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_10027500;
      }
      else {
        puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      }
      uVar5 = puVar3[2];
      uVar6 = (uint)uVar5;
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      if (uVar1 == 0xffff) {
        puVar3 = &DAT_10027500;
      }
      else {
        puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
      }
      if ((DAT_10027508 & 1) == 0) {
        DAT_10027508 = DAT_10027508 | 1;
        DAT_10027500 = 0xffff;
        DAT_10027502 = 0xffff;
        DAT_10027504 = 0xffff;
        DAT_10027506 = 1;
      }
      if (puVar3[2] == 0xffff) {
        puVar4 = &DAT_10027500;
      }
      else {
        puVar4 = (ushort *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
      }
      if (*puVar4 == uVar1) {
        if ((DAT_10027508 & 1) == 0) {
          DAT_10027508 = DAT_10027508 | 1;
          DAT_10027500 = 0xffff;
          DAT_10027502 = 0xffff;
          DAT_10027504 = 0xffff;
          DAT_10027506 = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_10027500;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        uVar5 = puVar3[1];
        if ((DAT_10027508 & 1) == 0) {
          DAT_10027508 = DAT_10027508 | 1;
          DAT_10027500 = 0xffff;
          DAT_10027502 = 0xffff;
          DAT_10027504 = 0xffff;
          DAT_10027506 = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_10027500;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) {
LAB_1000cdb0:
          *(undefined2 *)((uint)uVar1 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)((uint)uVar5 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          param_1 = uVar6;
        }
        else {
          if ((DAT_10027508 & 1) == 0) {
            DAT_10027508 = DAT_10027508 | 1;
            DAT_10027500 = 0xffff;
            DAT_10027502 = 0xffff;
            DAT_10027504 = 0xffff;
            DAT_10027506 = 1;
          }
          if (sVar7 == -1) {
            puVar3 = &DAT_10027500;
          }
          else {
            puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
          }
          if ((DAT_10027508 & 1) == 0) {
            DAT_10027508 = DAT_10027508 | 1;
            DAT_10027500 = 0xffff;
            DAT_10027502 = 0xffff;
            DAT_10027504 = 0xffff;
            DAT_10027506 = 1;
          }
          if (puVar3[2] == 0xffff) {
            puVar3 = &DAT_10027500;
          }
          else {
            puVar3 = (undefined2 *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
          }
          if (puVar3[1] == sVar7) {
            param_1 = (uint)uVar1;
            FUN_1000c570(this,uVar1);
            if ((DAT_10027508 & 1) == 0) {
              DAT_10027508 = DAT_10027508 | 1;
              DAT_10027500 = 0xffff;
              DAT_10027502 = 0xffff;
              DAT_10027504 = 0xffff;
              DAT_10027506 = 1;
            }
            if (uVar1 == 0xffff) {
              puVar3 = &DAT_10027500;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
            }
            uVar1 = puVar3[2];
            uVar8 = (uint)uVar1;
            if ((DAT_10027508 & 1) == 0) {
              DAT_10027508 = DAT_10027508 | 1;
              DAT_10027500 = 0xffff;
              DAT_10027502 = 0xffff;
              DAT_10027504 = 0xffff;
              DAT_10027506 = 1;
            }
            if (uVar1 == 0xffff) {
              puVar3 = &DAT_10027500;
            }
            else {
              puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
            }
            uVar6 = (uint)(ushort)puVar3[2];
          }
          *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
          *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
          FUN_1000c660(this,(ushort)uVar6);
        }
      }
      else {
        if ((DAT_10027508 & 1) == 0) {
          DAT_10027508 = DAT_10027508 | 1;
          DAT_10027500 = 0xffff;
          DAT_10027502 = 0xffff;
          DAT_10027504 = 0xffff;
          DAT_10027506 = 1;
        }
        if (uVar5 == 0xffff) {
          puVar4 = &DAT_10027500;
        }
        else {
          puVar4 = (ushort *)(uVar6 * 0x10 + *(int *)((int)this + 4));
        }
        uVar5 = *puVar4;
        if ((DAT_10027508 & 1) == 0) {
          DAT_10027508 = DAT_10027508 | 1;
          DAT_10027500 = 0xffff;
          DAT_10027502 = 0xffff;
          DAT_10027504 = 0xffff;
          DAT_10027506 = 1;
        }
        if (uVar5 == 0xffff) {
          puVar3 = &DAT_10027500;
        }
        else {
          puVar3 = (undefined2 *)((uint)uVar5 * 0x10 + *(int *)((int)this + 4));
        }
        if (puVar3[3] == 0) goto LAB_1000cdb0;
        if ((DAT_10027508 & 1) == 0) {
          DAT_10027508 = DAT_10027508 | 1;
          DAT_10027500 = 0xffff;
          DAT_10027502 = 0xffff;
          DAT_10027504 = 0xffff;
          DAT_10027506 = 1;
        }
        if (sVar7 == -1) {
          puVar3 = &DAT_10027500;
        }
        else {
          puVar3 = (undefined2 *)((param_1 & 0xffff) * 0x10 + *(int *)((int)this + 4));
        }
        if ((DAT_10027508 & 1) == 0) {
          DAT_10027508 = DAT_10027508 | 1;
          DAT_10027500 = 0xffff;
          DAT_10027502 = 0xffff;
          DAT_10027504 = 0xffff;
          DAT_10027506 = 1;
        }
        if (puVar3[2] == 0xffff) {
          psVar2 = &DAT_10027500;
        }
        else {
          psVar2 = (short *)((uint)(ushort)puVar3[2] * 0x10 + *(int *)((int)this + 4));
        }
        if (*psVar2 == sVar7) {
          param_1 = (uint)uVar1;
          FUN_1000c660(this,uVar1);
          if ((DAT_10027508 & 1) == 0) {
            DAT_10027508 = DAT_10027508 | 1;
            DAT_10027500 = 0xffff;
            DAT_10027502 = 0xffff;
            DAT_10027504 = 0xffff;
            DAT_10027506 = 1;
          }
          if (uVar1 == 0xffff) {
            puVar3 = &DAT_10027500;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
          }
          uVar1 = puVar3[2];
          uVar8 = (uint)uVar1;
          if ((DAT_10027508 & 1) == 0) {
            DAT_10027508 = DAT_10027508 | 1;
            DAT_10027500 = 0xffff;
            DAT_10027502 = 0xffff;
            DAT_10027504 = 0xffff;
            DAT_10027506 = 1;
          }
          if (uVar1 == 0xffff) {
            puVar3 = &DAT_10027500;
          }
          else {
            puVar3 = (undefined2 *)((uint)uVar1 * 0x10 + *(int *)((int)this + 4));
          }
          uVar6 = (uint)(ushort)puVar3[2];
        }
        *(undefined2 *)(uVar8 * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
        *(undefined2 *)(uVar6 * 0x10 + 6 + *(int *)((int)this + 4)) = 0;
        FUN_1000c570(this,(ushort)uVar6);
      }
    } while ((short)param_1 != *(short *)((int)this + 0x10));
  }
  *(undefined2 *)((uint)*(ushort *)((int)this + 0x10) * 0x10 + 6 + *(int *)((int)this + 4)) = 1;
  return;
}


// 1000cf70 FUN_1000cf70

void __thiscall FUN_1000cf70(void *this,uint param_1,ushort param_2,char param_3)

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
    FUN_1000c990(this,param_1);
    return;
  }
  if (param_3 != '\0') {
    *(undefined2 *)((uint)param_2 * 0x10 + *(int *)((int)this + 4)) = uVar2;
    FUN_1000c990(this,param_1);
    return;
  }
  *(undefined2 *)((uint)param_2 * 0x10 + 2 + *(int *)((int)this + 4)) = uVar2;
  FUN_1000c990(this,param_1);
  return;
}


// 1000cfe0 FUN_1000cfe0

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* WARNING: Unable to track spacebase fully for stack */

int __thiscall FUN_1000cfe0(void *this,char *param_1)

{
  code *pcVar1;
  int iVar2;
  uint uVar3;
  int iVar4;
  undefined4 uVar5;
  int iVar6;
  int iVar7;
  undefined1 *puVar8;
  undefined1 *puVar9;
  int *this_00;
  int iVar10;
  undefined4 auStackY_28 [2];
  undefined4 uStackY_20;
  
  uStackY_20 = 0x1000cff4;
  iVar2 = FUN_100075f0(param_1);
  iVar7 = iVar2 + 1;
  iVar2 = -(iVar2 + 0x10U & 0xfffffff0);
  *(int *)(&stack0xffffffe4 + iVar2) = iVar7;
  *(undefined1 **)((int)&uStackY_20 + iVar2) = &stack0xffffffe8 + iVar2;
  *(char **)((int)auStackY_28 + iVar2 + 4) = param_1;
  *(undefined4 *)((int)auStackY_28 + iVar2) = 0x1000d014;
  FUN_10008080(*(char **)((int)auStackY_28 + iVar2 + 4),*(char **)((int)&uStackY_20 + iVar2),
               *(size_t *)(&stack0xffffffe4 + iVar2));
  iVar10 = *(int *)((int)this + 0x10) + -1;
  if (-1 < iVar10) {
    iVar4 = iVar10 * 0xc;
    do {
      iVar6 = *(int *)((int)this + 4) + iVar4;
      if (*(int *)(iVar6 + 8) != 0) {
        *(undefined4 *)(&stack0xffffffe4 + iVar2) = *(undefined4 *)(iVar6 + 8);
        *(undefined1 **)((int)&uStackY_20 + iVar2) = &stack0xffffffe8 + iVar2;
        *(undefined4 *)((int)auStackY_28 + iVar2 + 4) = 0x1000d048;
        uVar3 = FUN_10007680(*(byte **)((int)&uStackY_20 + iVar2),
                             *(byte **)(&stack0xffffffe4 + iVar2));
        if (uVar3 == 0) {
          return iVar10;
        }
      }
      iVar10 = iVar10 + -1;
      iVar4 = iVar4 + -0xc;
    } while (-1 < iVar10);
  }
                    /* WARNING: Load size is inaccurate */
  pcVar1 = *(code **)(*this + 0x1c);
  *(char **)(&stack0xffffffe4 + iVar2) = param_1;
  puVar8 = (undefined1 *)((int)&uStackY_20 + iVar2);
  *(undefined4 *)((int)&uStackY_20 + iVar2) = 0x1000d06c;
  iVar10 = (*pcVar1)();
  if (iVar10 != 0) {
    this_00 = (int *)((int)this + 4);
    *(undefined4 *)(puVar8 + -4) = *(undefined4 *)((int)this + 0x10);
    *(undefined4 *)(puVar8 + -8) = 0x1000d0ad;
    iVar6 = FUN_1000c7c0(this_00,*(int *)(puVar8 + -4));
    iVar4 = iVar6 * 0xc;
    *(int *)(iVar4 + *this_00) = iVar10;
    *(undefined4 *)(*this_00 + 4 + iVar4) = 0;
    iVar10 = *this_00;
    pcVar1 = *(code **)(**(int **)g_pMemAlloc_exref + 4);
    *(int *)(puVar8 + -4) = iVar7;
    puVar9 = puVar8 + -8;
    *(undefined4 *)(puVar8 + -8) = 0x1000d0e0;
    uVar5 = (*pcVar1)();
    *(undefined4 *)(iVar10 + iVar4 + 8) = uVar5;
    uVar5 = *(undefined4 *)(*this_00 + 8 + iVar4);
    *(int *)(puVar9 + -4) = iVar7;
    *(undefined1 **)((int)puVar9 + -8) = &stack0xffffffe8 + iVar2;
    *(undefined4 *)(puVar9 + -0xc) = uVar5;
    *(undefined4 *)(puVar9 + -0x10) = 0x1000d0f7;
    FUN_10007e20(*(char **)(puVar9 + -0xc),*(char **)(puVar9 + -8),*(size_t *)(puVar9 + -4));
    return iVar6;
  }
  *(char **)(puVar8 + -4) = param_1;
  *(char **)(puVar8 + -8) = "AppFramework : Unable to load module %s!\n";
  *(undefined4 *)(puVar8 + -0xc) = 0x1000d07f;
  Warning();
  return -1;
}


// 1000d110 FUN_1000d110

int __thiscall FUN_1000d110(void *this,int param_1)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  
  if (param_1 == 0) {
    Warning("AppFramework : Unable to load module %p!\n",0);
    return -1;
  }
  iVar1 = *(int *)((int)this + 0x10) + -1;
  if (-1 < iVar1) {
    piVar3 = (int *)(*(int *)((int)this + 4) + 4 + iVar1 * 0xc);
    do {
      if ((*piVar3 != 0) && (*piVar3 == param_1)) {
        return iVar1;
      }
      iVar1 = iVar1 + -1;
      piVar3 = piVar3 + -3;
    } while (-1 < iVar1);
  }
  piVar3 = (int *)((int)this + 4);
  iVar2 = FUN_1000c7c0(piVar3,*(int *)((int)this + 0x10));
  iVar1 = iVar2 * 0xc;
  *(undefined4 *)(iVar1 + *piVar3) = 0;
  *(int *)(*piVar3 + 4 + iVar1) = param_1;
  *(undefined4 *)(*piVar3 + 8 + iVar1) = 0;
  return iVar2;
}


// 1000d190 FUN_1000d190

void __thiscall FUN_1000d190(void *this,uint param_1,int param_2)

{
  uint uVar1;
  ushort uVar2;
  char *pcVar3;
  char *local_4;
  
  local_4 = "Unknown";
  if ((-1 < (int)param_1) && (param_1 < 9)) {
    local_4 = (&PTR_s_CREATION_10024930)[param_1];
  }
  pcVar3 = "(Unknown)";
  uVar2 = FUN_10003fd0((int)this + 0x2c);
  do {
    uVar1 = (uint)uVar2;
    if (uVar1 == 0xffff) {
LAB_1000d204:
      Warning("System (%s) failed during stage %s\n",pcVar3,local_4);
      return;
    }
    if (*(int *)(uVar1 * 0x10 + 0xc + *(int *)((int)this + 0x30)) == param_2) {
      pcVar3 = *(char **)(uVar1 * 0x10 + 8 + *(int *)((int)this + 0x30));
      goto LAB_1000d204;
    }
    uVar2 = FUN_10004080((void *)((int)this + 0x2c),uVar2);
  } while( true );
}


// 1000d220 FUN_1000d220

int __thiscall FUN_1000d220(void *this,int param_1)

{
  ushort uVar1;
  uint uVar2;
  int iVar3;
  uint uVar4;
  int local_8 [2];
  
  if (param_1 != 0) {
    local_8[0] = param_1;
    uVar1 = FUN_1000c890((void *)((int)this + 0x2c),local_8);
    if (uVar1 != 0xffff) {
      return *(int *)(*(int *)((int)this + 0x18) +
                     *(int *)((uint)uVar1 * 0x10 + 0xc + *(int *)((int)this + 0x30)) * 4);
    }
  }
  uVar4 = 0;
  if (0 < *(int *)((int)this + 0x24)) {
    uVar2 = 0;
    do {
      iVar3 = (**(code **)(**(int **)(*(int *)((int)this + 0x18) + uVar2 * 4) + 8))(param_1);
      if (iVar3 != 0) {
        return iVar3;
      }
      uVar4 = uVar4 + 1;
      uVar2 = uVar4 & 0xffff;
    } while ((int)uVar2 < *(int *)((int)this + 0x24));
  }
  if ((*(void **)((int)this + 0x48) != (void *)0x0) &&
     (iVar3 = FUN_1000d220(*(void **)((int)this + 0x48),param_1), iVar3 != 0)) {
    return iVar3;
  }
  return 0;
}


// 1000d2b0 FUN_1000d2b0

int __fastcall FUN_1000d2b0(void *param_1)

{
  int iVar1;
  int iVar2;
  
  iVar2 = 0;
  if (0 < *(int *)((int)param_1 + 0x24)) {
    do {
      iVar1 = (**(code **)(**(int **)(*(int *)((int)param_1 + 0x18) + iVar2 * 4) + 0xc))();
      if (iVar1 != 1) {
        FUN_1000d190(param_1,3,iVar2);
        return iVar1;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < *(int *)((int)param_1 + 0x24));
  }
  return 1;
}


// 1000d330 FUN_1000d330

undefined1 * FUN_1000d330(void)

{
  return &LAB_1000d300;
}


// 1000d340 FUN_1000d340

void __fastcall FUN_1000d340(int *param_1)

{
  int iVar1;
  
  DAT_10029050 = param_1;
  switch(param_1[0x13]) {
  default:
    iVar1 = param_1[9];
    while (iVar1 = iVar1 + -1, -1 < iVar1) {
      (**(code **)(**(int **)(param_1[6] + iVar1 * 4) + 0x10))();
    }
    (**(code **)(*param_1 + 0xc))();
  case 2:
  case 3:
    iVar1 = param_1[9];
    while (iVar1 = iVar1 + -1, -1 < iVar1) {
      (**(code **)(**(int **)(param_1[6] + iVar1 * 4) + 4))();
    }
  case 0:
  case 1:
    param_1[9] = 0;
    FUN_10004550(param_1 + 0xb);
    SpewOutputFunc(PTR_LAB_1002495c);
    FUN_1000c760((int)param_1);
                    /* WARNING: Could not recover jumptable at 0x1000d3c5. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (**(code **)(*param_1 + 0x10))();
    return;
  }
}


// 1000d3e0 FUN_1000d3e0

undefined4 __thiscall FUN_1000d3e0(void *this,undefined4 *param_1)

{
  undefined4 *puVar1;
  undefined4 *puVar2;
  ushort uVar3;
  void *local_4;
  
  puVar2 = param_1;
  local_4 = this;
  FUN_1000c340(this,param_1,(ushort *)&local_4,(undefined1 *)&param_1);
  uVar3 = FUN_1000c450((int)this);
  FUN_1000cf70(this,(uint)uVar3,(ushort)local_4,(char)param_1);
  *(short *)((int)this + 0x12) = *(short *)((int)this + 0x12) + 1;
  puVar1 = (undefined4 *)((uint)uVar3 * 0x10 + 8 + *(int *)((int)this + 4));
  if (puVar1 != (undefined4 *)0x0) {
    *puVar1 = *puVar2;
    puVar1[1] = puVar2[1];
  }
  return CONCAT22((short)((uint)puVar1 >> 0x10),uVar3);
}


// 1000d450 FUN_1000d450

undefined4 * __fastcall FUN_1000d450(undefined4 *param_1)

{
  undefined4 uVar1;
  undefined4 unaff_retaddr;
  
  *param_1 = &PTR_LAB_10020ac4;
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
  param_1[0xd] = 0x10;
  param_1[0xe] = 0;
  uVar1 = (**(code **)(**(int **)g_pMemAlloc_exref + 4))(0x100);
  param_1[0xc] = uVar1;
  *(undefined2 *)(param_1 + 0xf) = 0xffff;
  *(undefined2 *)((int)param_1 + 0x3e) = 0;
  *(undefined2 *)(param_1 + 0x10) = 0xffff;
  *(undefined2 *)((int)param_1 + 0x42) = 0xffff;
  param_1[0x11] = param_1[0xc];
  if (param_1[0xb] == 0) {
    param_1[0xb] = &LAB_1000c0b0;
    param_1[0x12] = unaff_retaddr;
    return param_1;
  }
  param_1[0x12] = unaff_retaddr;
  return param_1;
}


// 1000d4f0 FUN_1000d4f0

undefined4 __fastcall FUN_1000d4f0(void *param_1)

{
  undefined4 in_EAX;
  uint uVar1;
  int iVar2;
  
  iVar2 = 0;
  if (0 < *(int *)((int)param_1 + 0x24)) {
    do {
      in_EAX = (**(code **)**(undefined4 **)(*(int *)((int)param_1 + 0x18) + iVar2 * 4))
                         (&LAB_1000d300);
      if ((char)in_EAX == '\0') {
        uVar1 = FUN_1000d190(param_1,1,iVar2);
        return uVar1 & 0xffffff00;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < *(int *)((int)param_1 + 0x24));
  }
  return CONCAT31((int3)((uint)in_EAX >> 8),1);
}


// 1000d550 FUN_1000d550

undefined4 __fastcall FUN_1000d550(int *param_1)

{
  code *pcVar1;
  char cVar2;
  undefined4 uVar3;
  int iVar4;
  
  pcVar1 = *(code **)*param_1;
  DAT_10029050 = param_1;
  param_1[0x13] = 8;
  cVar2 = (*pcVar1)();
  if (cVar2 == '\0') {
    param_1[0x13] = 0;
    return 0xffffffff;
  }
  uVar3 = FUN_1000d4f0(param_1);
  if ((char)uVar3 == '\0') {
    param_1[0x13] = 1;
    return 0xffffffff;
  }
  cVar2 = (**(code **)(*param_1 + 4))();
  if (cVar2 == '\0') {
    param_1[0x13] = 2;
    return 0xffffffff;
  }
  iVar4 = FUN_1000d2b0(param_1);
  if (iVar4 != 1) {
    param_1[0x13] = 3;
    return 0xffffffff;
  }
  return 1;
}


// 1000d5d0 FUN_1000d5d0

undefined4 __fastcall FUN_1000d5d0(int *param_1)

{
  undefined4 uVar1;
  
  DAT_10029050 = param_1;
  uVar1 = FUN_1000d550(param_1);
  if (param_1[0x13] == 8) {
    uVar1 = (**(code **)(*param_1 + 8))();
    FUN_1000d340(param_1);
    DAT_10029050 = (int *)param_1[0x12];
  }
  return uVar1;
}


// 1000d620 FUN_1000d620

undefined4 * __thiscall FUN_1000d620(void *this,undefined4 param_1)

{
  FUN_1000d450((undefined4 *)this);
  *(undefined4 *)((int)this + 0x50) = param_1;
  *(undefined ***)this = &PTR_LAB_10020ae8;
  *(undefined1 *)((int)this + 0x54) = 0;
  return (undefined4 *)this;
}


// 1000d650 FUN_1000d650

void __thiscall FUN_1000d650(void *this,char *param_1,undefined4 *param_2)

{
  int local_8;
  undefined4 local_4;
  
  local_8 = FUN_1000c050(param_1);
  local_4 = *param_2;
  FUN_1000d3e0(this,&local_8);
  return;
}


// 1000d690 FUN_1000d690

int __thiscall FUN_1000d690(void *this,int param_1,char *param_2)

{
  undefined4 *puVar1;
  char *pcVar2;
  code *pcVar3;
  char *pcVar4;
  
  if (param_1 == -1) {
    return 0;
  }
  puVar1 = (undefined4 *)(*(int *)((int)this + 4) + param_1 * 0xc);
  if (*(int *)(*(int *)((int)this + 4) + param_1 * 0xc) == 0) {
    pcVar3 = (code *)puVar1[1];
  }
  else {
    pcVar3 = (code *)FUN_10009fc0((HMODULE)*puVar1);
  }
  pcVar2 = param_2;
  pcVar4 = (char *)(*pcVar3)(param_2,&param_1);
  if ((param_1 == 0) && (pcVar4 != (char *)0x0)) {
    param_2 = pcVar4;
    param_2 = (char *)FUN_1000c820((void *)((int)this + 0x18),*(int *)((int)this + 0x24),&param_2);
    FUN_1000d650((void *)((int)this + 0x2c),pcVar2,&param_2);
    return (int)pcVar4;
  }
  Warning("AppFramework : Unable to create system %s!\n",pcVar2);
  return 0;
}


// 1000d730 FUN_1000d730

uint __thiscall FUN_1000d730(void *this,undefined4 *param_1)

{
  char cVar1;
  char *pcVar2;
  int iVar3;
  uint uVar4;
  
  pcVar2 = (char *)*param_1;
  cVar1 = *pcVar2;
  while( true ) {
    if (cVar1 == '\0') {
      return CONCAT31((int3)((uint)pcVar2 >> 8),1);
    }
    iVar3 = FUN_1000cfe0(this,pcVar2);
    iVar3 = FUN_1000d690(this,iVar3,(char *)param_1[1]);
    if (iVar3 == 0) break;
    pcVar2 = (char *)param_1[2];
    param_1 = param_1 + 2;
    cVar1 = *pcVar2;
  }
  uVar4 = Warning("Unable to load interface %s from %s\n",param_1[1],*param_1);
  return uVar4 & 0xffffff00;
}


// 1000d7c0 FUN_1000d7c0

undefined4 FUN_1000d7c0(void)

{
  return DAT_1002905c;
}


// 1000d7d0 FUN_1000d7d0

void __cdecl FUN_1000d7d0(undefined4 param_1)

{
  DAT_1002905c = param_1;
  return;
}


// 1000d890 FUN_1000d890

undefined4 __fastcall FUN_1000d890(int param_1)

{
  int iVar1;
  undefined4 uVar2;
  
  iVar1 = FUN_10001760(*(int **)(param_1 + 0x50));
  if (iVar1 != 0) {
    return 0;
  }
  FUN_1000c130(*(void **)(param_1 + 0x54),*(undefined4 *)(param_1 + 0x50),param_1);
  uVar2 = FUN_1000d5d0(*(int **)(param_1 + 0x54));
  return uVar2;
}


// 1000d8c0 FUN_1000d8c0

undefined4 __fastcall FUN_1000d8c0(int *param_1)

{
  undefined4 uVar1;
  int iVar2;
  
  uVar1 = thunk_FUN_1000d550(param_1);
  iVar2 = FUN_1000c120((int)param_1);
  if (iVar2 != 8) {
    return uVar1;
  }
  iVar2 = FUN_10001760((int *)param_1[0x14]);
  if (iVar2 != 0) {
    return 0;
  }
  FUN_1000c130((void *)param_1[0x15],param_1[0x14],param_1);
                    /* WARNING: Could not recover jumptable at 0x1000d908. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  uVar1 = (**(code **)(*(int *)param_1[0x15] + 0x14))();
  return uVar1;
}


// 1000d910 FUN_1000d910

void __fastcall FUN_1000d910(int *param_1)

{
  (**(code **)(*(int *)param_1[0x15] + 0x18))();
  thunk_FUN_1000d340(param_1);
  return;
}


// 1000d930 FUN_1000d930

undefined4 * __thiscall FUN_1000d930(void *this,undefined4 param_1)

{
  FUN_1000d450((undefined4 *)this);
  *(undefined4 *)((int)this + 0x54) = param_1;
  *(undefined ***)this = &PTR_LAB_10020b74;
  *(undefined4 *)((int)this + 0x50) = 0;
  *(undefined1 *)((int)this + 0x58) = 0;
  return (undefined4 *)this;
}


// 1000d958 FUN_1000d958

void FUN_1000d958(void)

{
  return;
}


// 1000d959 __cfltcvt_init

/* Library Function - Single Match
    __cfltcvt_init
   
   Library: Visual Studio 2005 Release */

void __cfltcvt_init(void)

{
  PTR_LAB_10024c34 = __cfltcvt;
  PTR_LAB_10024c38 = &LAB_10011172;
  PTR_LAB_10024c3c = &LAB_10011130;
  PTR_LAB_10024c40 = &LAB_10011164;
  PTR_LAB_10024c44 = &LAB_100110da;
  PTR_LAB_10024c48 = __cfltcvt;
  PTR_LAB_10024c4c = __cfltcvt_l;
  PTR_LAB_10024c50 = __fassign_l;
  PTR_LAB_10024c54 = __cropzeros_l;
  PTR_LAB_10024c58 = __forcdecpt_l;
  return;
}


// 1000d9b9 __fpmath

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __fpmath
   
   Library: Visual Studio 2005 Release */

void __cdecl __fpmath(int param_1)

{
  __cfltcvt_init();
  _DAT_1002906c = __ms_p5_mp_test_fdiv();
  if (param_1 != 0) {
    __setdefaultprecision();
  }
  return;
}


// 1000d9d7 __putenv_helper

/* Library Function - Single Match
    __putenv_helper
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __putenv_helper(uchar *param_1,char *param_2)

{
  rsize_t _SizeInBytes;
  LPWSTR pWVar1;
  DWORD *pDVar2;
  uchar *puVar3;
  size_t sVar4;
  size_t sVar5;
  int iVar6;
  int iVar7;
  int extraout_EAX;
  LPWSTR local_c;
  LPWSTR local_8;
  
  local_c = (LPWSTR)0x0;
  if (DAT_1002ac64 == 0) {
    return 0xffffffff;
  }
  if (param_1 == (uchar *)0x0) {
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0xffffffff;
  }
  if (param_2 == (char *)0x0) {
    puVar3 = __mbschr(param_1,0x3d);
    if ((puVar3 != (uchar *)0x0) &&
       ((0x7ffe < (int)puVar3 - (int)param_1 ||
        (sVar4 = _strnlen((char *)(puVar3 + 1),0x7fff), 0x7ffe < sVar4)))) {
LAB_1000da9a:
      pDVar2 = FUN_100125a7();
      *pDVar2 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return 0xffffffff;
    }
    sVar4 = _strlen((char *)param_1);
    local_c = (LPWSTR)FUN_10005780(sVar4 + 1,1);
    if (local_c == (LPWSTR)0x0) {
      return 0xffffffff;
    }
    _strcpy_s((char *)local_c,sVar4 + 1,(char *)param_1);
  }
  else {
    sVar4 = _strnlen((char *)param_1,0x7fff);
    sVar5 = _strnlen(param_2,0x7fff);
    if ((0x7ffe < sVar4) || (0x7ffe < sVar5)) goto LAB_1000da9a;
    _SizeInBytes = sVar5 + 2 + sVar4;
    local_c = (LPWSTR)FUN_10005780(_SizeInBytes,1);
    if (local_c == (LPWSTR)0x0) {
      return 0xffffffff;
    }
    _strcpy_s((char *)local_c,_SizeInBytes,(char *)param_1);
    *(char *)(sVar4 + (int)local_c) = '=';
    _strcpy_s((char *)(sVar4 + 1 + (int)local_c),_SizeInBytes - (sVar4 + 1),param_2);
  }
  iVar6 = FUN_10012018(&local_c,1);
  pWVar1 = local_c;
  if (iVar6 != 0) goto joined_r0x1000db0d;
  if (DAT_1002909c == 0) {
    return 0;
  }
  local_8 = (LPWSTR)0x0;
  iVar6 = MultiByteToWideChar(0,0,(LPCSTR)param_1,-1,(LPWSTR)0x0,0);
  if (iVar6 == 0) goto LAB_1000dbaf;
  if (param_2 != (char *)0x0) {
    iVar7 = MultiByteToWideChar(0,0,param_2,-1,(LPWSTR)0x0,0);
    if (iVar7 == 0) goto LAB_1000dbaf;
    iVar6 = iVar6 + 1 + iVar7;
  }
  local_8 = (LPWSTR)FUN_10005780(iVar6,2);
  if (local_8 == (LPWSTR)0x0) {
    return 0xffffffff;
  }
  iVar7 = MultiByteToWideChar(0,0,(LPCSTR)param_1,-1,local_8,iVar6);
  if (iVar7 == 0) {
LAB_1000dba6:
    FUN_10005520(local_8);
LAB_1000dbaf:
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x2a;
    return 0xffffffff;
  }
  if (param_2 != (char *)0x0) {
    iVar7 = FUN_10011f58(local_8);
    local_8[iVar7] = L'=';
    iVar6 = MultiByteToWideChar(0,0,param_2,-1,local_8 + iVar7 + 1,iVar6 - (iVar7 + 1));
    if (iVar6 == 0) goto LAB_1000dba6;
  }
  FUN_10011bf2(&local_8,0);
  pWVar1 = local_8;
  if (extraout_EAX == 0) {
    return 0;
  }
joined_r0x1000db0d:
  if (pWVar1 == (LPWSTR)0x0) {
    return 0xffffffff;
  }
  FUN_10005520(pWVar1);
  return 0xffffffff;
}


// 1000dbe6 __putenv

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __putenv
   
   Library: Visual Studio 2005 Release */

int __cdecl __putenv(char *_EnvString)

{
  int iVar1;
  
  __lock(7);
  iVar1 = __putenv_helper((uchar *)_EnvString,(char *)0x0);
  FUN_1000dc22();
  return iVar1;
}


// 1000dc22 FUN_1000dc22

void FUN_1000dc22(void)

{
  FUN_10012689(7);
  return;
}


// 1000dc30 __alloca_probe

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


// 1000dc5b __validdrive

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


// 1000dc92 __getdcwd_nolock

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
      pDVar2 = FUN_100125ba();
      *pDVar2 = 0xf;
      pDVar2 = FUN_100125a7();
      *pDVar2 = 0xd;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return (LPSTR)0x0;
    }
  }
  if (param_2 == (LPSTR)0x0) {
    param_1 = 0;
  }
  else {
    if ((int)param_3 < 1) {
      pDVar2 = FUN_100125a7();
      *pDVar2 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        pDVar2 = FUN_100125a7();
        *pDVar2 = 0x22;
        *param_2 = '\0';
        return (LPSTR)0x0;
      }
      return param_2;
    }
    if ((int)param_3 < (int)DVar4) {
      param_3 = DVar4;
    }
    lpBuffer = (LPSTR)FUN_10005540(param_3,1);
    if (lpBuffer == (LPSTR)0x0) {
      pDVar2 = FUN_100125a7();
      *pDVar2 = 0xc;
      pDVar2 = FUN_100125ba();
      *pDVar2 = 8;
      return (LPSTR)0x0;
    }
    DVar4 = GetFullPathNameA((LPCSTR)&local_8,param_3,lpBuffer,&local_c);
    if ((DVar4 != 0) && ((int)DVar4 < (int)param_3)) {
      return lpBuffer;
    }
  }
  DVar4 = GetLastError();
  FUN_100125cd(DVar4);
  return (LPSTR)0x0;
}


// 1000ddc7 __getcwd

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
  FUN_1000de07();
  return pCVar1;
}


// 1000de07 FUN_1000de07

void FUN_1000de07(void)

{
  FUN_10012689(7);
  return;
}


// 1000de10 __access_s

/* Library Function - Single Match
    __access_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __access_s(char *_Filename,int _AccessMode)

{
  DWORD *pDVar1;
  DWORD DVar2;
  
  if ((_Filename == (char *)0x0) || ((_AccessMode & 0xfffffff9U) != 0)) {
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  DVar2 = GetFileAttributesA(_Filename);
  if (DVar2 == 0xffffffff) {
    DVar2 = GetLastError();
    FUN_100125cd(DVar2);
  }
  else {
    if ((((DVar2 & 0x10) != 0) || ((DVar2 & 1) == 0)) || ((_AccessMode & 2U) == 0)) {
      return 0;
    }
    pDVar1 = FUN_100125ba();
    *pDVar1 = 5;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0xd;
  }
  pDVar1 = FUN_100125a7();
  return *pDVar1;
}


// 1000de97 FID_conflict:__access

/* Library Function - Multiple Matches With Different Base Names
    __access
    __waccess
   
   Library: Visual Studio 2005 Release */

int __cdecl FID_conflict___access(wchar_t *_Filename,int _AccessMode)

{
  errno_t eVar1;
  
  eVar1 = __access_s((char *)_Filename,_AccessMode);
  return -(uint)(eVar1 != 0);
}


// 1000deab __getenv_helper_nolock

/* Library Function - Single Match
    __getenv_helper_nolock
   
   Library: Visual Studio 2005 Release */

char * __cdecl __getenv_helper_nolock(char *param_1)

{
  int iVar1;
  size_t _MaxCount;
  size_t sVar2;
  int *piVar3;
  
  if (DAT_1002ac64 == 0) {
    return (char *)0x0;
  }
  if (((DAT_10029094 != (int *)0x0) ||
      (((DAT_1002909c != 0 && (iVar1 = ___wtomb_environ(), iVar1 == 0)) &&
       (DAT_10029094 != (int *)0x0)))) && (piVar3 = DAT_10029094, param_1 != (char *)0x0)) {
    _MaxCount = _strlen(param_1);
    for (; (char *)*piVar3 != (char *)0x0; piVar3 = piVar3 + 1) {
      sVar2 = _strlen((char *)*piVar3);
      if (((_MaxCount < sVar2) && (((uchar *)*piVar3)[_MaxCount] == '=')) &&
         (iVar1 = __mbsnbicoll((uchar *)*piVar3,(uchar *)param_1,_MaxCount), iVar1 == 0)) {
        return (char *)(*piVar3 + 1 + _MaxCount);
      }
    }
  }
  return (char *)0x0;
}


// 1000df2c _getenv

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _getenv
   
   Library: Visual Studio 2005 Release */

char * __cdecl _getenv(char *_VarName)

{
  DWORD *pDVar1;
  size_t sVar2;
  char *pcVar3;
  
  if ((_VarName != (char *)0x0) && (sVar2 = _strnlen(_VarName,0x7fff), sVar2 < 0x7fff)) {
    __lock(7);
    pcVar3 = __getenv_helper_nolock(_VarName);
    FUN_1000dfa6();
    return pcVar3;
  }
  pDVar1 = FUN_100125a7();
  *pDVar1 = 0x16;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return (char *)0x0;
}


// 1000dfa6 FUN_1000dfa6

void FUN_1000dfa6(void)

{
  FUN_10012689(7);
  return;
}


// 1000dfaf __dupenv_s

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __dupenv_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __dupenv_s(char **_PBuffer,size_t *_PBufferSizeInBytes,char *_VarName)

{
  DWORD *pDVar1;
  char *_Str;
  size_t sVar2;
  char *_Dst;
  errno_t eVar3;
  rsize_t _SizeInBytes;
  undefined4 local_20;
  
  __lock(7);
  if (_PBuffer != (char **)0x0) {
    *_PBuffer = (char *)0x0;
    if (_PBufferSizeInBytes != (size_t *)0x0) {
      *_PBufferSizeInBytes = 0;
    }
    if (_VarName != (char *)0x0) {
      _Str = __getenv_helper_nolock(_VarName);
      if (_Str != (char *)0x0) {
        sVar2 = _strlen(_Str);
        _SizeInBytes = sVar2 + 1;
        _Dst = (char *)FUN_10005540(_SizeInBytes,1);
        *_PBuffer = _Dst;
        if (_Dst == (char *)0x0) {
          pDVar1 = FUN_100125a7();
          *pDVar1 = 0xc;
          pDVar1 = FUN_100125a7();
          local_20 = *pDVar1;
          goto LAB_1000e077;
        }
        eVar3 = _strcpy_s(_Dst,_SizeInBytes,_Str);
        if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
        if (_PBufferSizeInBytes != (size_t *)0x0) {
          *_PBufferSizeInBytes = _SizeInBytes;
        }
      }
      local_20 = 0;
      goto LAB_1000e077;
    }
  }
  pDVar1 = FUN_100125a7();
  *pDVar1 = 0x16;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  local_20 = 0x16;
LAB_1000e077:
  FUN_1000e08c();
  return local_20;
}


// 1000e08c FUN_1000e08c

void FUN_1000e08c(void)

{
  FUN_10012689(7);
  return;
}


// 1000e095 __spawnv

/* Library Function - Single Match
    __spawnv
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __spawnv(int _Mode,char *_Filename,char **_ArgList)

{
  DWORD *pDVar1;
  intptr_t iVar2;
  
  if ((((_Filename != (char *)0x0) && (*_Filename != '\0')) && (_ArgList != (char **)0x0)) &&
     ((*_ArgList != (char *)0x0 && (**_ArgList != '\0')))) {
    iVar2 = __spawnve(_Mode,_Filename,_ArgList,(char **)0x0);
    return iVar2;
  }
  pDVar1 = FUN_100125a7();
  *pDVar1 = 0x16;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1000e0f0 FID_conflict:_memcpy

/* Library Function - Multiple Matches With Different Base Names
    _memcpy
    _memmove
   
   Libraries: Visual Studio 2005 Debug, Visual Studio 2005 Release, Visual Studio 2008 Debug, Visual
   Studio 2008 Release */

void * __cdecl FID_conflict__memcpy(void *_Dst,void *_Src,size_t _Size)

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
          goto switchD_1000e2d3_caseD_2;
        case 3:
          goto switchD_1000e2d3_caseD_3;
        }
        goto switchD_1000e2d3_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_1000e2d3_caseD_0;
      case 1:
        goto switchD_1000e2d3_caseD_1;
      case 2:
        goto switchD_1000e2d3_caseD_2;
      case 3:
        goto switchD_1000e2d3_caseD_3;
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
              goto switchD_1000e2d3_caseD_2;
            case 3:
              goto switchD_1000e2d3_caseD_3;
            }
            goto switchD_1000e2d3_caseD_1;
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
              goto switchD_1000e2d3_caseD_2;
            case 3:
              goto switchD_1000e2d3_caseD_3;
            }
            goto switchD_1000e2d3_caseD_1;
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
              goto switchD_1000e2d3_caseD_2;
            case 3:
              goto switchD_1000e2d3_caseD_3;
            }
            goto switchD_1000e2d3_caseD_1;
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
switchD_1000e2d3_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_1000e2d3_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_1000e2d3_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_1000e2d3_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_10029c44 != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_1000e14c_caseD_2;
      case 3:
        goto switchD_1000e14c_caseD_3;
      }
      goto switchD_1000e14c_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_1000e14c_caseD_0;
    case 1:
      goto switchD_1000e14c_caseD_1;
    case 2:
      goto switchD_1000e14c_caseD_2;
    case 3:
      goto switchD_1000e14c_caseD_3;
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
            goto switchD_1000e14c_caseD_2;
          case 3:
            goto switchD_1000e14c_caseD_3;
          }
          goto switchD_1000e14c_caseD_1;
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
            goto switchD_1000e14c_caseD_2;
          case 3:
            goto switchD_1000e14c_caseD_3;
          }
          goto switchD_1000e14c_caseD_1;
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
            goto switchD_1000e14c_caseD_2;
          case 3:
            goto switchD_1000e14c_caseD_3;
          }
          goto switchD_1000e14c_caseD_1;
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
switchD_1000e14c_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_1000e14c_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_1000e14c_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_1000e14c_caseD_0:
  return _Dst;
}


// 1000e455 _sprintf

/* Library Function - Single Match
    _sprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl _sprintf(char *_Dest,char *_Format,...)

{
  DWORD *pDVar1;
  int iVar2;
  FILE local_24;
  
  if ((_Format == (char *)0x0) || (_Dest == (char *)0x0)) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  else {
    local_24._base = _Dest;
    local_24._ptr = _Dest;
    local_24._cnt = 0x7fffffff;
    local_24._flag = 0x42;
    iVar2 = FUN_100131fa(&local_24,(byte *)_Format,(localeinfo_struct *)0x0,
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


// 1000e4d0 _memcpy

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
          goto switchD_1000e6b3_caseD_2;
        case 3:
          goto switchD_1000e6b3_caseD_3;
        }
        goto switchD_1000e6b3_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_1000e6b3_caseD_0;
      case 1:
        goto switchD_1000e6b3_caseD_1;
      case 2:
        goto switchD_1000e6b3_caseD_2;
      case 3:
        goto switchD_1000e6b3_caseD_3;
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
              goto switchD_1000e6b3_caseD_2;
            case 3:
              goto switchD_1000e6b3_caseD_3;
            }
            goto switchD_1000e6b3_caseD_1;
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
              goto switchD_1000e6b3_caseD_2;
            case 3:
              goto switchD_1000e6b3_caseD_3;
            }
            goto switchD_1000e6b3_caseD_1;
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
              goto switchD_1000e6b3_caseD_2;
            case 3:
              goto switchD_1000e6b3_caseD_3;
            }
            goto switchD_1000e6b3_caseD_1;
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
switchD_1000e6b3_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_1000e6b3_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_1000e6b3_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_1000e6b3_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_10029c44 != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_1000e52c_caseD_2;
      case 3:
        goto switchD_1000e52c_caseD_3;
      }
      goto switchD_1000e52c_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_1000e52c_caseD_0;
    case 1:
      goto switchD_1000e52c_caseD_1;
    case 2:
      goto switchD_1000e52c_caseD_2;
    case 3:
      goto switchD_1000e52c_caseD_3;
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
            goto switchD_1000e52c_caseD_2;
          case 3:
            goto switchD_1000e52c_caseD_3;
          }
          goto switchD_1000e52c_caseD_1;
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
            goto switchD_1000e52c_caseD_2;
          case 3:
            goto switchD_1000e52c_caseD_3;
          }
          goto switchD_1000e52c_caseD_1;
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
            goto switchD_1000e52c_caseD_2;
          case 3:
            goto switchD_1000e52c_caseD_3;
          }
          goto switchD_1000e52c_caseD_1;
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
switchD_1000e52c_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_1000e52c_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_1000e52c_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_1000e52c_caseD_0:
  return _Dst;
}


// 1000e835 FUN_1000e835

int __cdecl FUN_1000e835(FILE *param_1)

{
  DWORD *pDVar1;
  int iVar2;
  int iVar3;
  
  iVar3 = -1;
  if (param_1 == (FILE *)0x0) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar3 = -1;
  }
  else {
    if ((param_1->_flag & 0x83) != 0) {
      iVar3 = __flush(param_1);
      __freebuf(param_1);
      iVar2 = __fileno(param_1);
      iVar2 = __close(iVar2);
      if (iVar2 < 0) {
        iVar3 = -1;
      }
      else if (param_1->_tmpfname != (char *)0x0) {
        FUN_10005520(param_1->_tmpfname);
        param_1->_tmpfname = (char *)0x0;
      }
    }
    param_1->_flag = 0;
  }
  return iVar3;
}


// 1000e8a7 _fclose

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _fclose
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

int __cdecl _fclose(FILE *_File)

{
  DWORD *pDVar1;
  int local_20;
  
  local_20 = -1;
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    local_20 = -1;
  }
  else if ((_File->_flag & 0x40) == 0) {
    FUN_1000f88c((uint)_File);
    local_20 = FUN_1000e835(_File);
    FUN_1000e91b();
  }
  else {
    _File->_flag = 0;
  }
  return local_20;
}


// 1000e91b FUN_1000e91b

void FUN_1000e91b(void)

{
  uint unaff_ESI;
  
  FUN_1000f8f6(unaff_ESI);
  return;
}


// 1000e923 FUN_1000e923

uint __cdecl FUN_1000e923(LPWSTR param_1,LPWSTR param_2,uint param_3,uint param_4,FILE *param_5)

{
  LPWSTR pWVar1;
  LPWSTR pWVar2;
  DWORD *pDVar3;
  LPWSTR pWVar4;
  int iVar5;
  uint uVar6;
  LPWSTR pWVar7;
  LPWSTR pWVar8;
  LPWSTR pWVar9;
  LPWSTR local_10;
  
  if ((param_3 != 0) && (param_4 != 0)) {
    if (param_1 != (LPWSTR)0x0) {
      if ((param_5 != (FILE *)0x0) && (param_4 <= (uint)(0xffffffff / (ulonglong)param_3))) {
LAB_1000e99c:
        pWVar8 = (LPWSTR)(param_3 * param_4);
        pWVar7 = pWVar8;
        pWVar2 = param_1;
        pWVar1 = param_2;
        if ((param_5->_flag & 0x10c) == 0) {
          local_10 = (LPWSTR)0x1000;
        }
        else {
          local_10 = (LPWSTR)param_5->_bufsiz;
        }
joined_r0x1000e9c1:
        do {
          while( true ) {
            if (pWVar7 == (LPWSTR)0x0) {
              return param_4;
            }
            if ((param_5->_flag & 0x10c) == 0) break;
            pWVar4 = (LPWSTR)param_5->_cnt;
            if (pWVar4 == (LPWSTR)0x0) break;
            if ((int)pWVar4 < 0) {
LAB_1000eb11:
              param_5->_flag = param_5->_flag | 0x20;
LAB_1000eb15:
              return (uint)((int)pWVar8 - (int)pWVar7) / param_3;
            }
            pWVar9 = pWVar7;
            if (pWVar4 <= pWVar7) {
              pWVar9 = pWVar4;
            }
            if (pWVar1 < pWVar9) {
              if (param_2 != (LPWSTR)0xffffffff) {
                _memset(param_1,0,(size_t)param_2);
              }
              pDVar3 = FUN_100125a7();
              *pDVar3 = 0x22;
              goto LAB_1000e958;
            }
            _memcpy_s(pWVar2,(rsize_t)pWVar1,param_5->_ptr,(rsize_t)pWVar9);
            param_5->_cnt = param_5->_cnt - (int)pWVar9;
            param_5->_ptr = param_5->_ptr + (int)pWVar9;
            pWVar7 = (LPWSTR)((int)pWVar7 - (int)pWVar9);
            pWVar1 = (LPWSTR)((int)pWVar1 - (int)pWVar9);
            pWVar2 = (LPWSTR)((int)pWVar2 + (int)pWVar9);
          }
          if (local_10 <= pWVar7) {
            if (local_10 == (LPWSTR)0x0) {
              pWVar4 = (LPWSTR)0x7fffffff;
              if (pWVar7 < (LPWSTR)0x80000000) {
                pWVar4 = pWVar7;
              }
            }
            else {
              if (pWVar7 < (LPWSTR)0x80000000) {
                uVar6 = (uint)pWVar7 % (uint)local_10;
                pWVar4 = pWVar7;
              }
              else {
                uVar6 = (uint)(0x7fffffff % ZEXT48(local_10));
                pWVar4 = (LPWSTR)0x7fffffff;
              }
              pWVar4 = (LPWSTR)((int)pWVar4 - uVar6);
            }
            if (pWVar1 < pWVar4) {
LAB_1000eae4:
              if (param_2 != (LPWSTR)0xffffffff) {
                _memset(param_1,0,(size_t)param_2);
              }
              pDVar3 = FUN_100125a7();
              *pDVar3 = 0x22;
              goto LAB_1000e958;
            }
            pWVar9 = pWVar2;
            uVar6 = __fileno(param_5);
            iVar5 = FUN_100145b0(uVar6,pWVar9,pWVar4);
            if (iVar5 == 0) {
              param_5->_flag = param_5->_flag | 0x10;
              goto LAB_1000eb15;
            }
            if (iVar5 == -1) goto LAB_1000eb11;
            pWVar7 = (LPWSTR)((int)pWVar7 - iVar5);
            pWVar1 = (LPWSTR)((int)pWVar1 - iVar5);
            pWVar2 = (LPWSTR)((int)pWVar2 + iVar5);
            goto joined_r0x1000e9c1;
          }
          iVar5 = __filbuf(param_5);
          if (iVar5 == -1) goto LAB_1000eb15;
          if (pWVar1 == (LPWSTR)0x0) goto LAB_1000eae4;
          *(char *)pWVar2 = (char)iVar5;
          local_10 = (LPWSTR)param_5->_bufsiz;
          pWVar7 = (LPWSTR)((int)pWVar7 - 1);
          pWVar1 = (LPWSTR)((int)pWVar1 - 1);
          pWVar2 = (LPWSTR)((int)pWVar2 + 1);
        } while( true );
      }
      if (param_2 != (LPWSTR)0xffffffff) {
        _memset(param_1,0,(size_t)param_2);
      }
      if ((param_5 != (FILE *)0x0) && (param_4 <= (uint)(0xffffffff / (ulonglong)param_3)))
      goto LAB_1000e99c;
    }
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
LAB_1000e958:
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return 0;
}


// 1000eb29 _fread_s

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _fread_s
   
   Library: Visual Studio 2008 Release */

size_t __cdecl _fread_s(void *_DstBuf,size_t _DstSize,size_t _ElementSize,size_t _Count,FILE *_File)

{
  DWORD *pDVar1;
  uint uVar2;
  
  if ((_ElementSize != 0) && (_Count != 0)) {
    if (_File != (FILE *)0x0) {
      FUN_1000f88c((uint)_File);
      uVar2 = FUN_1000e923((LPWSTR)_DstBuf,(LPWSTR)_DstSize,_ElementSize,_Count,_File);
      FUN_1000ebb5();
      return uVar2;
    }
    if (_DstSize != 0xffffffff) {
      _memset(_DstBuf,0,_DstSize);
    }
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return 0;
}


// 1000ebb5 FUN_1000ebb5

void FUN_1000ebb5(void)

{
  int unaff_EBP;
  
  FUN_1000f8f6(*(uint *)(unaff_EBP + 0x18));
  return;
}


// 1000ebbf FUN_1000ebbf

void __cdecl FUN_1000ebbf(void *param_1,size_t param_2,size_t param_3,FILE *param_4)

{
  _fread_s(param_1,0xffffffff,param_2,param_3,param_4);
  return;
}


// 1000ebda __ftell_nolock

/* Library Function - Single Match
    __ftell_nolock
   
   Library: Visual Studio 2005 Release */

long __cdecl __ftell_nolock(FILE *_File)

{
  uint uVar1;
  char *pcVar2;
  DWORD *pDVar3;
  uint _FileHandle;
  FILE *pFVar4;
  long lVar5;
  char *pcVar6;
  FILE *pFVar7;
  char *pcVar8;
  int iVar9;
  bool bVar10;
  int local_10;
  int local_c;
  
  pFVar7 = _File;
  if (_File == (FILE *)0x0) {
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  _FileHandle = __fileno(_File);
  if (_File->_cnt < 0) {
    _File->_cnt = 0;
  }
  local_c = __lseek(_FileHandle,0,1);
  if (local_c < 0) {
    return -1;
  }
  uVar1 = _File->_flag;
  if ((uVar1 & 0x108) == 0) {
    return local_c - _File->_cnt;
  }
  pcVar6 = _File->_ptr;
  pcVar8 = _File->_base;
  local_10 = (int)pcVar6 - (int)pcVar8;
  if ((uVar1 & 3) == 0) {
    if (-1 < (char)uVar1) {
      pDVar3 = FUN_100125a7();
      *pDVar3 = 0x16;
      return -1;
    }
  }
  else {
    pcVar2 = pcVar8;
    if ((*(byte *)((&DAT_10029b40)[(int)_FileHandle >> 5] + 4 + (_FileHandle & 0x1f) * 0x38) & 0x80)
        != 0) {
      for (; pcVar2 < pcVar6; pcVar2 = pcVar2 + 1) {
        if (*pcVar2 == '\n') {
          local_10 = local_10 + 1;
        }
      }
    }
  }
  if (local_c != 0) {
    if ((_File->_flag & 1) != 0) {
      if (_File->_cnt == 0) {
        local_10 = 0;
      }
      else {
        iVar9 = (_FileHandle & 0x1f) * 0x38;
        pFVar4 = (FILE *)(pcVar6 + (_File->_cnt - (int)pcVar8));
        if ((*(byte *)((&DAT_10029b40)[(int)_FileHandle >> 5] + 4 + iVar9) & 0x80) != 0) {
          lVar5 = __lseek(_FileHandle,0,2);
          if (lVar5 == local_c) {
            pcVar6 = _File->_base;
            pcVar8 = pcVar6 + (int)&pFVar4->_ptr;
            _File = pFVar4;
            for (; pcVar6 < pcVar8; pcVar6 = pcVar6 + 1) {
              if (*pcVar6 == '\n') {
                _File = (FILE *)((int)&_File->_ptr + 1);
              }
            }
            bVar10 = (pFVar7->_flag & 0x2000) == 0;
          }
          else {
            lVar5 = __lseek(_FileHandle,local_c,0);
            if (lVar5 < 0) {
              return -1;
            }
            pFVar7 = (FILE *)0x200;
            if ((((FILE *)0x200 < pFVar4) || ((_File->_flag & 8U) == 0)) ||
               ((_File->_flag & 0x400U) != 0)) {
              pFVar7 = (FILE *)_File->_bufsiz;
            }
            bVar10 = (*(byte *)((&DAT_10029b40)[(int)_FileHandle >> 5] + 4 + iVar9) & 4) == 0;
            _File = pFVar7;
          }
          pFVar4 = _File;
          if (!bVar10) {
            pFVar4 = (FILE *)((int)&_File->_ptr + 1);
          }
        }
        _File = pFVar4;
        local_c = local_c - (int)_File;
      }
    }
    return local_10 + local_c;
  }
  return local_10;
}


// 1000ed72 _ftell

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _ftell
   
   Library: Visual Studio 2005 Release */

long __cdecl _ftell(FILE *_File)

{
  DWORD *pDVar1;
  long lVar2;
  
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    lVar2 = -1;
  }
  else {
    FUN_1000f88c((uint)_File);
    lVar2 = __ftell_nolock(_File);
    FUN_1000edd6();
  }
  return lVar2;
}


// 1000edd6 FUN_1000edd6

void FUN_1000edd6(void)

{
  int unaff_EBP;
  
  FUN_1000f8f6(*(uint *)(unaff_EBP + 8));
  return;
}


// 1000ede0 __fseek_nolock

/* Library Function - Single Match
    __fseek_nolock
   
   Library: Visual Studio 2005 Release */

int __cdecl __fseek_nolock(FILE *_File,long _Offset,int _Origin)

{
  uint uVar1;
  DWORD *pDVar2;
  int iVar3;
  long lVar4;
  
  if ((_File->_flag & 0x83U) == 0) {
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    iVar3 = -1;
  }
  else {
    _File->_flag = _File->_flag & 0xffffffef;
    if (_Origin == 1) {
      lVar4 = __ftell_nolock(_File);
      _Offset = _Offset + lVar4;
      _Origin = 0;
    }
    __flush(_File);
    uVar1 = _File->_flag;
    if ((char)uVar1 < '\0') {
      _File->_flag = uVar1 & 0xfffffffc;
    }
    else if ((((uVar1 & 1) != 0) && ((uVar1 & 8) != 0)) && ((uVar1 & 0x400) == 0)) {
      _File->_bufsiz = 0x200;
    }
    iVar3 = __fileno(_File);
    lVar4 = __lseek(iVar3,_Offset,_Origin);
    iVar3 = (lVar4 != -1) - 1;
  }
  return iVar3;
}


// 1000ee67 _fseek

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _fseek
   
   Library: Visual Studio 2005 Release */

int __cdecl _fseek(FILE *_File,long _Offset,int _Origin)

{
  DWORD *pDVar1;
  int iVar2;
  
  if ((_File == (FILE *)0x0) || (((_Origin != 0 && (_Origin != 1)) && (_Origin != 2)))) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  else {
    FUN_1000f88c((uint)_File);
    iVar2 = __fseek_nolock(_File,_Offset,_Origin);
    FUN_1000eee2();
  }
  return iVar2;
}


// 1000eee2 FUN_1000eee2

void FUN_1000eee2(void)

{
  int unaff_EBP;
  
  FUN_1000f8f6(*(uint *)(unaff_EBP + 8));
  return;
}


// 1000eeec __fsopen

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __fsopen
   
   Library: Visual Studio 2005 Release */

FILE * __cdecl __fsopen(char *_Filename,char *_Mode,int _ShFlag)

{
  DWORD *pDVar1;
  undefined4 *puVar2;
  FILE *pFVar3;
  undefined1 local_14 [8];
  undefined4 uStack_c;
  undefined *local_8;
  
  local_8 = &DAT_10022ce0;
  uStack_c = 0x1000eef8;
  if (((_Filename == (char *)0x0) || (_Mode == (char *)0x0)) || (*_Mode == '\0')) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    puVar2 = FUN_10014da2();
    if (puVar2 == (undefined4 *)0x0) {
      pDVar1 = FUN_100125a7();
      *pDVar1 = 0x18;
    }
    else {
      local_8 = (undefined *)0x0;
      if (*_Filename != '\0') {
        pFVar3 = (FILE *)FUN_10014b02(_Filename,_Mode,_ShFlag,puVar2);
        local_8 = (undefined *)0xfffffffe;
        FUN_1000efa6();
        return pFVar3;
      }
      pDVar1 = FUN_100125a7();
      *pDVar1 = 0x16;
      __local_unwind4(&DAT_10025040,(int)local_14,0xfffffffe);
    }
  }
  return (FILE *)0x0;
}


// 1000efa6 FUN_1000efa6

void FUN_1000efa6(void)

{
  int unaff_EBP;
  
  FUN_1000f8f6(*(uint *)(unaff_EBP + 8));
  return;
}


// 1000efb0 _fopen

/* Library Function - Single Match
    _fopen
   
   Library: Visual Studio 2005 Release */

FILE * __cdecl _fopen(char *_Filename,char *_Mode)

{
  FILE *pFVar1;
  
  pFVar1 = __fsopen(_Filename,_Mode,0x40);
  return pFVar1;
}


// 1000efc3 __chdir

/* Library Function - Single Match
    __chdir
   
   Library: Visual Studio 2005 Release */

int __cdecl __chdir(char *_Path)

{
  byte bVar1;
  bool bVar2;
  DWORD *pDVar3;
  BOOL BVar4;
  DWORD DVar5;
  byte *lpBuffer;
  uint uVar6;
  int iVar7;
  CHAR local_114;
  undefined1 local_113;
  undefined1 local_112;
  undefined1 local_111;
  byte local_110 [116];
  undefined1 local_9c [148];
  uint local_8;
  
  local_8 = DAT_10025040 ^ (uint)local_9c;
  lpBuffer = local_110;
  bVar2 = false;
  if (_Path == (char *)0x0) {
    pDVar3 = FUN_100125ba();
    *pDVar3 = 0;
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_1000f0cf;
  }
  BVar4 = SetCurrentDirectoryA(_Path);
  if (BVar4 == 0) {
LAB_1000f0b2:
    DVar5 = GetLastError();
    FUN_100125cd(DVar5);
  }
  else {
    DVar5 = GetCurrentDirectoryA(0x105,(LPSTR)local_110);
    if (0x104 < (int)DVar5) {
      lpBuffer = (byte *)FUN_10005780(DVar5 + 1,1);
      if ((lpBuffer == (byte *)0x0) || (bVar2 = true, DVar5 == 0)) goto LAB_1000f0b2;
      DVar5 = GetCurrentDirectoryA(DVar5 + 1,(LPSTR)lpBuffer);
    }
    if (DVar5 == 0) goto LAB_1000f0b2;
    bVar1 = *lpBuffer;
    if (((bVar1 != 0x5c) && (bVar1 != 0x2f)) || (bVar1 != lpBuffer[1])) {
      local_114 = '=';
      uVar6 = __mbctoupper((uint)*lpBuffer);
      local_113 = (undefined1)uVar6;
      local_112 = 0x3a;
      local_111 = 0;
      BVar4 = SetEnvironmentVariableA(&local_114,(LPCSTR)lpBuffer);
      if (BVar4 == 0) goto LAB_1000f0b2;
    }
  }
  if (bVar2) {
    FUN_10005520(lpBuffer);
  }
LAB_1000f0cf:
  iVar7 = __security_check_cookie(local_8 ^ (uint)local_9c);
  return iVar7;
}


// 1000f0f0 _strrchr

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


// 1000f11d __amsg_exit

/* Library Function - Single Match
    __amsg_exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __amsg_exit(int param_1)

{
  code *pcVar1;
  
  __FF_MSGBANNER();
  FUN_1001570d(param_1);
  pcVar1 = (code *)FUN_100151cb((int)PTR___exit_10024990);
  (*pcVar1)(0xff);
  return;
}


// 1000f141 FUN_1000f141

void __cdecl FUN_1000f141(undefined4 param_1)

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


// 1000f167 ___crtExitProcess

/* Library Function - Single Match
    ___crtExitProcess
   
   Library: Visual Studio 2005 Release */

void __cdecl ___crtExitProcess(int param_1)

{
  FUN_1000f141(param_1);
                    /* WARNING: Subroutine does not return */
  ExitProcess(param_1);
}


// 1000f17c FUN_1000f17c

void FUN_1000f17c(void)

{
  __lock(8);
  return;
}


// 1000f185 FUN_1000f185

void FUN_1000f185(void)

{
  FUN_10012689(8);
  return;
}


// 1000f18e FUN_1000f18e

void __cdecl FUN_1000f18e(undefined4 *param_1)

{
  undefined4 *in_EAX;
  
  for (; in_EAX < param_1; in_EAX = in_EAX + 1) {
    if ((code *)*in_EAX != (code *)0x0) {
      (*(code *)*in_EAX)();
    }
  }
  return;
}


// 1000f1a6 __initterm_e

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


// 1000f1c6 __get_osplatform

/* Library Function - Single Match
    __get_osplatform
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __get_osplatform(int *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (int *)0x0) && (DAT_10029074 != 0)) {
    *param_1 = DAT_10029074;
    return 0;
  }
  pDVar1 = FUN_100125a7();
  *pDVar1 = 0x16;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 1000f1fd FUN_1000f1fd

undefined4 __cdecl FUN_1000f1fd(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (undefined4 *)0x0) && (DAT_10029074 != 0)) {
    *param_1 = DAT_10029080;
    return 0;
  }
  pDVar1 = FUN_100125a7();
  *pDVar1 = 0x16;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 1000f239 __cinit

/* Library Function - Single Match
    __cinit
   
   Library: Visual Studio 2005 Release */

int __cdecl __cinit(int param_1)

{
  BOOL BVar1;
  int iVar2;
  undefined4 *puVar3;
  
  BVar1 = __IsNonwritableInCurrentImage((PBYTE)&PTR___fpmath_10020b94);
  if (BVar1 != 0) {
    __fpmath(param_1);
  }
  __initp_misc_cfltcvt_tab();
  iVar2 = __initterm_e((undefined4 *)&DAT_1001f250,(undefined4 *)&DAT_1001f268);
  if (iVar2 == 0) {
    _atexit((_func_4879 *)&LAB_1001590a);
    puVar3 = &DAT_1001f228;
    do {
      if ((code *)*puVar3 != (code *)0x0) {
        (*(code *)*puVar3)();
      }
      puVar3 = puVar3 + 1;
    } while (puVar3 < &DAT_1001f24c);
    if ((DAT_1002ac74 != (code *)0x0) &&
       (BVar1 = __IsNonwritableInCurrentImage((PBYTE)&DAT_1002ac74), BVar1 != 0)) {
      (*DAT_1002ac74)(0,2,0);
    }
    iVar2 = 0;
  }
  return iVar2;
}


// 1000f2cb FUN_1000f2cb

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x1000f39e) */

void __cdecl FUN_1000f2cb(int param_1,int param_2,int param_3)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  int iVar4;
  code *pcVar5;
  
  __lock(8);
  if (DAT_100290b4 != 1) {
    DAT_100290b0 = 1;
    DAT_100290ac = (undefined1)param_3;
    if (param_2 == 0) {
      piVar2 = (int *)FUN_100151cb(DAT_1002ac6c);
      piVar3 = (int *)FUN_100151cb(DAT_1002ac68);
      if (piVar2 != (int *)0x0) {
        while (piVar3 = piVar3 + -1, piVar2 <= piVar3) {
          if (*piVar3 != 0) {
            iVar1 = *piVar3;
            iVar4 = __encoded_null();
            if (iVar1 != iVar4) {
              pcVar5 = (code *)FUN_100151cb(iVar1);
              (*pcVar5)();
            }
          }
        }
      }
      FUN_1000f18e((undefined4 *)&DAT_1001f278);
    }
    FUN_1000f18e((undefined4 *)&DAT_1001f280);
  }
  FUN_1000f398();
  if (param_3 != 0) {
    return;
  }
  DAT_100290b4 = 1;
  FUN_10012689(8);
  ___crtExitProcess(param_1);
  return;
}


// 1000f398 FUN_1000f398

void FUN_1000f398(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + 0x10) != 0) {
    FUN_10012689(8);
  }
  return;
}


// 1000f3ad FUN_1000f3ad

void __cdecl FUN_1000f3ad(int param_1)

{
  FUN_1000f2cb(param_1,0,0);
  return;
}


// 1000f3be __exit

/* Library Function - Single Match
    __exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __exit(int _Code)

{
  FUN_1000f2cb(_Code,1,0);
  return;
}


// 1000f3cf __cexit

/* Library Function - Single Match
    __cexit
   
   Library: Visual Studio 2005 Release */

void __cdecl __cexit(void)

{
  FUN_1000f2cb(0,0,1);
  return;
}


// 1000f3de __init_pointers

/* Library Function - Single Match
    __init_pointers
   
   Library: Visual Studio 2005 Release */

void __cdecl __init_pointers(void)

{
  undefined4 uVar1;
  
  uVar1 = __encoded_null();
  FUN_10015f2b(uVar1);
  FUN_10015e4c(uVar1);
  FUN_10012442(uVar1);
  FUN_10015e42(uVar1);
  FUN_10015e38(uVar1);
  __initp_misc_winsig(uVar1);
  FUN_1000d958();
  __initp_eh_hooks();
  PTR___exit_10024990 = (undefined *)FUN_10015154(0x1000f3be);
  return;
}


// 1000f42a type_info::~type_info

/* Library Function - Single Match
    public: virtual __thiscall type_info::~type_info(void)
   
   Library: Visual Studio 2005 Release */

void __thiscall type_info::~type_info(type_info *this)

{
  *(undefined ***)this = &PTR__scalar_deleting_destructor__10020bd0;
  _Type_info_dtor(this);
  return;
}


// 1000f438 type_info::`scalar_deleting_destructor'

/* Library Function - Single Match
    public: virtual void * __thiscall type_info::`scalar deleting destructor'(unsigned int)
   
   Library: Visual Studio 2005 Release */

void * __thiscall type_info::_scalar_deleting_destructor_(type_info *this,uint param_1)

{
  ~type_info(this);
  if ((param_1 & 1) != 0) {
    FUN_10005520(this);
  }
  return this;
}


// 1000f47e FUN_1000f47e

undefined4 __cdecl
FUN_1000f47e(uchar *param_1,uchar *param_2,uint param_3,uchar *param_4,uint param_5,uchar *param_6,
            uint param_7,uchar *param_8,uint param_9)

{
  uchar uVar1;
  bool bVar2;
  int iVar3;
  DWORD *pDVar4;
  undefined4 uVar5;
  uchar *puVar6;
  uchar *puVar7;
  uchar *local_8;
  
  bVar2 = false;
  if (param_1 != (uchar *)0x0) {
    if (param_2 == (uchar *)0x0) {
      if (param_3 == 0) {
LAB_1000f4a3:
        if (param_4 == (uchar *)0x0) {
          if (param_5 == 0) {
LAB_1000f4b4:
            if (param_6 == (uchar *)0x0) {
              if (param_7 == 0) {
LAB_1000f4c5:
                if (param_8 == (uchar *)0x0) {
                  if (param_9 == 0) {
LAB_1000f4cf:
                    iVar3 = 1;
                    puVar6 = param_1;
                    do {
                      if (*puVar6 == '\0') break;
                      iVar3 = iVar3 + -1;
                      puVar6 = puVar6 + 1;
                    } while (iVar3 != 0);
                    if (*puVar6 == ':') {
                      if (param_2 != (uchar *)0x0) {
                        if (param_3 < 3) goto LAB_1000f5e7;
                        __mbsnbcpy_s(param_2,0xffffffff,param_1,2);
                      }
                      param_1 = puVar6 + 1;
                    }
                    else if (param_2 != (uchar *)0x0) {
                      *param_2 = '\0';
                    }
                    puVar6 = (uchar *)0x0;
                    local_8 = (uchar *)0x0;
                    puVar7 = param_1;
                    if (*param_1 == '\0') {
LAB_1000f580:
                      puVar6 = param_1;
                      if (param_4 != (uchar *)0x0) {
                        *param_4 = '\0';
                      }
                    }
                    else {
                      do {
                        iVar3 = __ismbblead((int)(char)*puVar7);
                        if (iVar3 == 0) {
                          uVar1 = *puVar7;
                          if ((uVar1 == '/') || (uVar1 == '\\')) {
                            puVar6 = puVar7 + 1;
                          }
                          else if (uVar1 == '.') {
                            local_8 = puVar7;
                          }
                        }
                        else {
                          puVar7 = puVar7 + 1;
                        }
                        puVar7 = puVar7 + 1;
                      } while (*puVar7 != '\0');
                      if (puVar6 == (uchar *)0x0) goto LAB_1000f580;
                      if (param_4 != (uchar *)0x0) {
                        if (param_5 <= (uint)((int)puVar6 - (int)param_1)) goto LAB_1000f5e7;
                        __mbsnbcpy_s(param_4,0xffffffff,param_1,(int)puVar6 - (int)param_1);
                      }
                    }
                    param_1 = puVar6;
                    if ((local_8 == (uchar *)0x0) || (local_8 < param_1)) {
                      if (param_6 != (uchar *)0x0) {
                        if (param_7 <= (uint)((int)puVar7 - (int)param_1)) goto LAB_1000f5e7;
                        __mbsnbcpy_s(param_6,0xffffffff,param_1,(int)puVar7 - (int)param_1);
                      }
                      if (param_8 != (uchar *)0x0) {
                        *param_8 = '\0';
                      }
                      return 0;
                    }
                    if (param_6 != (uchar *)0x0) {
                      if (param_7 <= (uint)((int)local_8 - (int)param_1)) goto LAB_1000f5e7;
                      __mbsnbcpy_s(param_6,0xffffffff,param_1,(int)local_8 - (int)param_1);
                    }
                    if (param_8 == (uchar *)0x0) {
                      return 0;
                    }
                    if ((uint)((int)puVar7 - (int)local_8) < param_9) {
                      __mbsnbcpy_s(param_8,0xffffffff,local_8,(int)puVar7 - (int)local_8);
                      return 0;
                    }
                    goto LAB_1000f5e7;
                  }
                }
                else if (param_9 != 0) goto LAB_1000f4cf;
              }
            }
            else if (param_7 != 0) goto LAB_1000f4c5;
          }
        }
        else if (param_5 != 0) goto LAB_1000f4b4;
      }
    }
    else if (param_3 != 0) goto LAB_1000f4a3;
  }
  bVar2 = true;
LAB_1000f5e7:
  if ((param_2 != (uchar *)0x0) && (param_3 != 0)) {
    *param_2 = '\0';
  }
  if ((param_4 != (uchar *)0x0) && (param_5 != 0)) {
    *param_4 = '\0';
  }
  if ((param_6 != (uchar *)0x0) && (param_7 != 0)) {
    *param_6 = '\0';
  }
  if ((param_8 != (uchar *)0x0) && (param_9 != 0)) {
    *param_8 = '\0';
  }
  pDVar4 = FUN_100125a7();
  if ((param_1 == (uchar *)0x0) || (bVar2)) {
    uVar5 = 0x16;
    *pDVar4 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    uVar5 = 0x22;
    *pDVar4 = 0x22;
  }
  return uVar5;
}


// 1000f66e FID_conflict:__splitpath

/* Library Function - Multiple Matches With Different Base Names
    __splitpath
    __wsplitpath
   
   Libraries: Visual Studio 2008 Release, Visual Studio 2010 Release */

void __cdecl
FID_conflict___splitpath
          (wchar_t *_FullPath,wchar_t *_Drive,wchar_t *_Dir,wchar_t *_Filename,wchar_t *_Ext)

{
  FUN_1000f47e((uchar *)_FullPath,(uchar *)_Drive,-(uint)(_Drive != (wchar_t *)0x0) & 3,
               (uchar *)_Dir,-(uint)(_Dir != (wchar_t *)0x0) & 0x100,(uchar *)_Filename,
               -(uint)(_Filename != (wchar_t *)0x0) & 0x100,(uchar *)_Ext,
               -(uint)(_Ext != (wchar_t *)0x0) & 0x100);
  return;
}


// 1000f6b8 __freopen_helper

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __freopen_helper
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

DWORD __cdecl __freopen_helper(int *param_1,char *param_2,char *param_3,FILE *param_4,int param_5)

{
  DWORD *pDVar1;
  undefined4 *puVar2;
  
  if ((((param_1 == (int *)0x0) || (*param_1 = 0, param_2 == (char *)0x0)) ||
      (param_3 == (char *)0x0)) || (param_4 == (FILE *)0x0)) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  if (*param_2 == '\0') {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
  }
  else {
    FUN_1000f88c((uint)param_4);
    if ((param_4->_flag & 0x83) != 0) {
      FUN_1000e835(param_4);
    }
    param_4->_base = (char *)0x0;
    param_4->_ptr = (char *)0x0;
    param_4->_flag = 0;
    param_4->_cnt = 0;
    puVar2 = FUN_10014b02(param_2,param_3,param_5,&param_4->_ptr);
    *param_1 = (int)puVar2;
    FUN_1000f787();
    if (*param_1 != 0) {
      return 0;
    }
  }
  pDVar1 = FUN_100125a7();
  return *pDVar1;
}


// 1000f787 FUN_1000f787

void FUN_1000f787(void)

{
  int unaff_EBP;
  
  FUN_1000f8f6(*(uint *)(unaff_EBP + -0x1c));
  return;
}


// 1000f791 _freopen

/* Library Function - Single Match
    _freopen
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

FILE * __cdecl _freopen(char *_Filename,char *_Mode,FILE *_File)

{
  FILE *local_8;
  
  local_8 = (FILE *)0x0;
  __freopen_helper((int *)&local_8,_Filename,_Mode,_File,0x40);
  return local_8;
}


// 1000f7b5 FUN_1000f7b5

undefined ** FUN_1000f7b5(void)

{
  return &PTR_DAT_100249b0;
}


// 1000f88c FUN_1000f88c

void __cdecl FUN_1000f88c(uint param_1)

{
  if ((s___AVtype_info___100249a0 + 0xf < param_1) && (param_1 < 0x10024c11)) {
    __lock(((int)(param_1 + 0xeffdb650) >> 5) + 0x10);
    *(uint *)(param_1 + 0xc) = *(uint *)(param_1 + 0xc) | 0x8000;
    return;
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(param_1 + 0x20));
  return;
}


// 1000f8c8 FUN_1000f8c8

void __cdecl FUN_1000f8c8(int param_1,int param_2)

{
  if (param_1 < 0x14) {
    __lock(param_1 + 0x10);
    *(uint *)(param_2 + 0xc) = *(uint *)(param_2 + 0xc) | 0x8000;
    return;
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(param_2 + 0x20));
  return;
}


// 1000f8f6 FUN_1000f8f6

void __cdecl FUN_1000f8f6(uint param_1)

{
  if ((s___AVtype_info___100249a0 + 0xf < param_1) && (param_1 < 0x10024c11)) {
    *(uint *)(param_1 + 0xc) = *(uint *)(param_1 + 0xc) & 0xffff7fff;
    FUN_10012689(((int)(param_1 + 0xeffdb650) >> 5) + 0x10);
    return;
  }
  LeaveCriticalSection((LPCRITICAL_SECTION)(param_1 + 0x20));
  return;
}


// 1000f92c FUN_1000f92c

void __cdecl FUN_1000f92c(int param_1,int param_2)

{
  if (param_1 < 0x14) {
    *(uint *)(param_2 + 0xc) = *(uint *)(param_2 + 0xc) & 0xffff7fff;
    FUN_10012689(param_1 + 0x10);
    return;
  }
  LeaveCriticalSection((LPCRITICAL_SECTION)(param_2 + 0x20));
  return;
}


// 1000f956 FUN_1000f956

int __cdecl FUN_1000f956(int param_1)

{
  int *piVar1;
  int *piVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  
  piVar1 = (int *)FUN_100151cb(DAT_1002ac6c);
  piVar2 = (int *)FUN_100151cb(DAT_1002ac68);
  if (piVar1 <= piVar2) {
    uVar4 = ((int)piVar2 - (int)piVar1) + 4;
    if (3 < uVar4) {
      uVar3 = FUN_100057d0(piVar1);
      if (uVar3 < uVar4) {
        uVar4 = 0x800;
        if (uVar3 < 0x800) {
          uVar4 = uVar3;
        }
        if ((uVar4 + uVar3 < uVar3) ||
           (iVar5 = thunk_FUN_10005640(piVar1,uVar4 + uVar3), iVar5 == 0)) {
          if (uVar3 + 0x10 < uVar3) {
            return 0;
          }
          iVar5 = thunk_FUN_10005640(piVar1,uVar3 + 0x10);
          if (iVar5 == 0) {
            return 0;
          }
        }
        piVar2 = (int *)(iVar5 + ((int)piVar2 - (int)piVar1 >> 2) * 4);
        DAT_1002ac6c = FUN_10015154(iVar5);
      }
      iVar5 = FUN_10015154(param_1);
      *piVar2 = iVar5;
      DAT_1002ac68 = FUN_10015154((int)(piVar2 + 1));
      return param_1;
    }
  }
  return 0;
}


// 1000fa3e __onexit

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __onexit
   
   Library: Visual Studio 2005 Release */

_onexit_t __cdecl __onexit(_onexit_t _Func)

{
  _onexit_t p_Var1;
  
  FUN_1000f17c();
  p_Var1 = (_onexit_t)FUN_1000f956((int)_Func);
  FUN_1000fa74();
  return p_Var1;
}


// 1000fa74 FUN_1000fa74

void FUN_1000fa74(void)

{
  FUN_1000f185();
  return;
}


// 1000fa7a _atexit

/* Library Function - Single Match
    _atexit
   
   Library: Visual Studio 2005 Release */

int __cdecl _atexit(_func_4879 *param_1)

{
  _onexit_t p_Var1;
  
  p_Var1 = __onexit((_onexit_t)param_1);
  return (p_Var1 != (_onexit_t)0x0) - 1;
}


// 1000fa8c __freea

/* Library Function - Single Match
    __freea
   
   Library: Visual Studio 2005 Release */

void __cdecl __freea(void *_Memory)

{
  if ((_Memory != (void *)0x0) && (*(int *)((int)_Memory + -8) == 0xdddd)) {
    FUN_10005520((int *)((int)_Memory + -8));
  }
  return;
}


// 1000faa7 _LocaleUpdate::_LocaleUpdate

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
    if ((*(undefined **)this != PTR_DAT_100257b0) && ((p_Var2->_ownlocale & DAT_100256cc) == 0)) {
      ptVar3 = ___updatetlocinfo();
      *(pthreadlocinfo *)this = ptVar3;
    }
    if ((*(undefined **)(this + 4) != PTR_DAT_100255d0) &&
       ((*(uint *)(*(int *)(this + 8) + 0x70) & DAT_100256cc) == 0)) {
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


// 1000fb29 _strlwr_s_l_stat

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* Library Function - Single Match
    int __cdecl _strlwr_s_l_stat(char *,unsigned int,struct localeinfo_struct *)
   
   Library: Visual Studio 2005 Release */

int __cdecl _strlwr_s_l_stat(char *param_1,uint param_2,localeinfo_struct *param_3)

{
  char cVar1;
  LPCWSTR _LocaleName;
  uint uVar2;
  DWORD *pDVar3;
  size_t sVar4;
  uint _CchDest;
  char *pcVar5;
  int iVar6;
  DWORD DVar7;
  char *local_c;
  
  uVar2 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  if (param_1 == (char *)0x0) {
LAB_1000fb45:
    pDVar3 = FUN_100125a7();
    DVar7 = 0x16;
  }
  else {
    sVar4 = _strnlen(param_1,param_2);
    if (param_2 <= sVar4) {
      *param_1 = '\0';
      goto LAB_1000fb45;
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
      goto LAB_1000fc98;
    }
    _CchDest = ___crtLCMapStringA(param_3,_LocaleName,0x100,param_1,-1,(LPSTR)0x0,0,
                                  param_3->locinfo->lc_codepage,1);
    if (_CchDest == 0) {
      pDVar3 = FUN_100125a7();
      *pDVar3 = 0x2a;
LAB_1000fbcf:
      FUN_100125a7();
      goto LAB_1000fc98;
    }
    if (_CchDest <= param_2) {
      if (((int)_CchDest < 1) || (0xffffffe0 / _CchDest == 0)) {
        local_c = (char *)0x0;
      }
      else if (_CchDest + 8 < 0x401) {
        pcVar5 = &stack0xffffffe4;
        local_c = &stack0xffffffe4;
        if (&stack0x00000000 != (undefined1 *)0x1c) {
LAB_1000fc2c:
          local_c = pcVar5 + 8;
        }
      }
      else {
        pcVar5 = (char *)FUN_10005720(_CchDest + 8);
        local_c = pcVar5;
        if (pcVar5 != (char *)0x0) {
          pcVar5[0] = -0x23;
          pcVar5[1] = -0x23;
          pcVar5[2] = '\0';
          pcVar5[3] = '\0';
          goto LAB_1000fc2c;
        }
      }
      if (local_c != (char *)0x0) {
        iVar6 = ___crtLCMapStringA(param_3,param_3->locinfo->lc_category[0].wlocale,0x100,param_1,-1
                                   ,local_c,_CchDest,param_3->locinfo->lc_codepage,1);
        if (iVar6 == 0) {
          pDVar3 = FUN_100125a7();
          *pDVar3 = 0x2a;
        }
        else {
          _strcpy_s(param_1,param_2,local_c);
        }
        __freea(local_c);
        goto LAB_1000fc98;
      }
      pDVar3 = FUN_100125a7();
      *pDVar3 = 0xc;
      goto LAB_1000fbcf;
    }
    *param_1 = '\0';
    pDVar3 = FUN_100125a7();
    DVar7 = 0x22;
  }
  *pDVar3 = DVar7;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
LAB_1000fc98:
  iVar6 = __security_check_cookie(uVar2 ^ (uint)&stack0xfffffffc);
  return iVar6;
}


// 1000fcaa __strlwr_s_l

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


// 1000fcdc __strlwr

/* Library Function - Single Match
    __strlwr
   
   Library: Visual Studio 2005 Release */

char * __cdecl __strlwr(char *_String)

{
  char cVar1;
  char *pcVar2;
  DWORD *pDVar3;
  
  if (DAT_100295a0 == 0) {
    pcVar2 = _String;
    if (_String == (char *)0x0) {
      pDVar3 = FUN_100125a7();
      *pDVar3 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 1000fd40 _memset

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
  if ((((char)_Val == '\0') && (0xff < _Size)) && (DAT_10029c44 != 0)) {
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


// 1000fdba __vsnprintf_l

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
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
    iVar2 = FUN_100131fa(&local_24,(byte *)_Format,_Locale,(wchar_t *)_ArgList);
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
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  return iVar2;
}


// 1000fe6b __vsnprintf

/* Library Function - Single Match
    __vsnprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl __vsnprintf(char *_Dest,size_t _Count,char *_Format,va_list _Args)

{
  int iVar1;
  
  iVar1 = __vsnprintf_l(_Dest,_Count,_Format,(_locale_t)0x0,_Args);
  return iVar1;
}


// 1000fe86 __CRT_INIT@12

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
        _DAT_1002907c = DVar2 * 0x100 + DVar3;
        DAT_10029074 = DVar1;
        _DAT_10029078 = uVar10;
        DAT_10029080 = DVar2;
        _DAT_10029084 = DVar3;
        bVar4 = FUN_100057f0();
        if (CONCAT31(extraout_var,bVar4) != 0) {
          iVar7 = __mtinit();
          if (iVar7 != 0) {
            __RTC_Initialize();
            DAT_10029c4c = GetCommandLineA();
            DAT_100290c0 = ___crtGetEnvironmentStringsA();
            iVar7 = __ioinit();
            if (-1 < iVar7) {
              iVar7 = __setargv();
              if (((-1 < iVar7) && (iVar7 = __setenvp(), -1 < iVar7)) &&
                 (iVar7 = __cinit(0), iVar7 == 0)) {
                DAT_100290bc = DAT_100290bc + 1;
                return 1;
              }
              __ioterm();
            }
            __mtterm();
          }
          FUN_10005800();
        }
      }
    }
  }
  else if (param_2 == 0) {
    if (0 < DAT_100290bc) {
      DAT_100290bc = DAT_100290bc + -1;
      if (DAT_100290b0 == 0) {
        __cexit();
      }
      if (param_3 == 0) {
        __ioterm();
        __mtterm();
        FUN_10005800();
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
    FUN_10015242();
    pDVar8 = (DWORD *)FUN_10005780(1,0x214);
    if (pDVar8 != (DWORD *)0x0) {
      uVar12 = DAT_10025048;
      pDVar13 = pDVar8;
      pcVar9 = (code *)FUN_100151cb(DAT_10029230);
      iVar7 = (*pcVar9)(uVar12,pDVar13);
      if (iVar7 != 0) {
        FUN_100152b1((int)pDVar8,0);
        DVar11 = GetCurrentThreadId();
        pDVar8[1] = 0xffffffff;
        *pDVar8 = DVar11;
        return 1;
      }
      FUN_10005520(pDVar8);
    }
  }
  return 0;
}


// 1001005f ___DllMainCRTStartup

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x100100f1) */
/* WARNING: Removing unreachable block (ram,0x1001009e) */
/* WARNING: Removing unreachable block (ram,0x1001011e) */
/* Library Function - Single Match
    ___DllMainCRTStartup
   
   Library: Visual Studio 2005 Release */

int __fastcall ___DllMainCRTStartup(int param_1,int param_2,undefined4 param_3)

{
  int iVar1;
  int local_20;
  
  if (((param_2 == 0) && (DAT_100290bc == 0)) ||
     (((param_2 == 1 || (param_2 == 2)) &&
      (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)))) {
    local_20 = 0;
  }
  else {
    local_20 = FUN_1001756b();
    if ((param_2 == 1) && (local_20 == 0)) {
      FUN_1001756b();
      __CRT_INIT_12(param_3,0,param_1);
    }
    if (((param_2 == 0) || (param_2 == 3)) &&
       (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)) {
      local_20 = 0;
    }
  }
  return local_20;
}


// 10010155 entry

void entry(undefined4 param_1,int param_2,int param_3)

{
  if (param_2 == 1) {
    ___security_init_cookie();
  }
  ___DllMainCRTStartup(param_3,param_2,param_1);
  return;
}


// 10010190 _strchr

/* Library Function - Single Match
    _strchr
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

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


// 10010250 _strstr

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
LAB_100102c9:
        return _Str + -1;
      }
      if (*pcVar10 != pcVar8[2]) break;
      pcVar1 = pcVar8 + 3;
      if (*pcVar1 == '\0') goto LAB_100102c9;
      pcVar2 = pcVar10 + 1;
      pcVar8 = pcVar8 + 2;
      pcVar10 = pcVar10 + 2;
    } while (*pcVar1 == *pcVar2);
  } while( true );
}


// 100102d6 __tolower_l

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
LAB_10010335:
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
      pDVar4 = FUN_100125a7();
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
    if (iVar3 == 0) goto LAB_10010335;
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


// 100103ed _tolower

/* Library Function - Single Match
    _tolower
   
   Library: Visual Studio 2005 Release */

int __cdecl _tolower(int _C)

{
  if (DAT_100295a0 == 0) {
    if (_C - 0x41U < 0x1a) {
      return _C + 0x20;
    }
  }
  else {
    _C = __tolower_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 10010420 _strncat

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
      if ((byte)uVar4 == 0) goto LAB_1001047d;
      uVar4 = (uint)puVar5 & 3;
    }
    do {
      do {
        puVar6 = puVar5;
        puVar5 = puVar6 + 1;
      } while (((*puVar6 ^ 0xffffffff ^ *puVar6 + 0x7efefeff) & 0x81010100) == 0);
      uVar4 = *puVar6;
      if ((char)uVar4 == '\0') goto LAB_1001048f;
      if ((char)(uVar4 >> 8) == '\0') {
        puVar6 = (uint *)((int)puVar6 + 1);
        goto LAB_1001048f;
      }
      if ((uVar4 & 0xff0000) == 0) {
        puVar6 = (uint *)((int)puVar6 + 2);
        goto LAB_1001048f;
      }
    } while ((uVar4 & 0xff000000) != 0);
LAB_1001047d:
    puVar6 = (uint *)((int)puVar5 + -1);
LAB_1001048f:
    if (((uint)_Source & 3) == 0) {
      uVar3 = _Count >> 2;
    }
    else {
      do {
        bVar1 = (byte)*(uint *)_Source;
        uVar4 = (uint)bVar1;
        _Source = (char *)((int)_Source + 1);
        if (bVar1 == 0) goto LAB_100104ea;
        *(byte *)puVar6 = bVar1;
        puVar6 = (uint *)((int)puVar6 + 1);
        _Count = _Count - 1;
        if (_Count == 0) goto LAB_100104e0;
      } while (((uint)_Source & 3) != 0);
      uVar3 = _Count >> 2;
    }
    for (; uVar3 != 0; uVar3 = uVar3 - 1) {
      uVar2 = *(uint *)_Source;
      uVar4 = *(uint *)_Source;
      _Source = (char *)((int)_Source + 4);
      if (((uVar2 ^ 0xffffffff ^ uVar2 + 0x7efefeff) & 0x81010100) != 0) {
        if ((char)uVar4 == '\0') {
LAB_100104ea:
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
LAB_100104e0:
    *(byte *)puVar6 = (byte)_Count;
  }
  return _Dest;
}


// 10010560 _strncpy

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
        goto joined_r0x100105ac;
      }
    }
    do {
      if (((uint)puVar5 & 3) == 0) {
        uVar4 = _Count >> 2;
        cVar3 = '\0';
        if (uVar4 == 0) goto LAB_100105f3;
        goto LAB_10010669;
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
joined_r0x10010665:
          while( true ) {
            uVar4 = uVar4 - 1;
            puVar5 = puVar5 + 1;
            if (uVar4 == 0) break;
LAB_10010669:
            *puVar5 = 0;
          }
          cVar3 = '\0';
          _Count = _Count & 3;
          if (_Count != 0) goto LAB_100105f3;
          return _Dest;
        }
        if ((char)(uVar2 >> 8) == '\0') {
          *puVar5 = uVar2 & 0xff;
          goto joined_r0x10010665;
        }
        if ((uVar2 & 0xff0000) == 0) {
          *puVar5 = uVar2 & 0xffff;
          goto joined_r0x10010665;
        }
        if ((uVar2 & 0xff000000) == 0) {
          *puVar5 = uVar2;
          goto joined_r0x10010665;
        }
      }
      *puVar5 = uVar2;
      puVar5 = puVar5 + 1;
      uVar4 = uVar4 - 1;
joined_r0x100105ac:
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
LAB_100105f3:
        *(char *)puVar5 = cVar3;
        puVar5 = (uint *)((int)puVar5 + 1);
      }
      return _Dest;
    }
    _Count = _Count - 1;
  } while (_Count != 0);
  return _Dest;
}


// 10010690 FUN_10010690

ulonglong __fastcall FUN_10010690(undefined4 param_1,undefined4 param_2)

{
  ulonglong uVar1;
  uint uVar2;
  float fVar3;
  float10 in_ST0;
  uint local_20;
  float fStack_1c;
  
  if (DAT_10029c44 == 0) {
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


// 100108aa _atol

/* Library Function - Single Match
    _atol
   
   Library: Visual Studio 2005 Release */

long __cdecl _atol(char *_Str)

{
  long lVar1;
  
  lVar1 = _strtol(_Str,(char **)0x0,10);
  return lVar1;
}


// 100108c0 __isdigit_l

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


// 1001090f _isdigit

/* Library Function - Single Match
    _isdigit
   
   Library: Visual Studio 2005 Release */

int __cdecl _isdigit(int _C)

{
  int iVar1;
  
  if (DAT_100295a0 == 0) {
    return (byte)PTR_DAT_100257a0[_C * 2] & 4;
  }
  iVar1 = __isdigit_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 10010938 __isspace_l

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


// 10010987 _isspace

/* Library Function - Single Match
    _isspace
   
   Library: Visual Studio 2005 Release */

int __cdecl _isspace(int _C)

{
  int iVar1;
  
  if (DAT_100295a0 == 0) {
    return (byte)PTR_DAT_100257a0[_C * 2] & 8;
  }
  iVar1 = __isspace_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 100109b0 __alloca_probe_16

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


// 100109c6 __alloca_probe_8

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


// 100109dc FUN_100109dc

void __cdecl FUN_100109dc(wchar_t *param_1)

{
  _wcstol(param_1,(wchar_t **)0x0,10);
  return;
}


// 100109f2 __atof_l

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
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 10010a9b _atof

/* Library Function - Single Match
    _atof
   
   Library: Visual Studio 2005 Release */

double __cdecl _atof(char *_String)

{
  double dVar1;
  
  dVar1 = __atof_l(_String,(_locale_t)0x0);
  return dVar1;
}


// 10010aa9 FUN_10010aa9

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 __cdecl FUN_10010aa9(byte *param_1,undefined4 *param_2,localeinfo_struct *param_3)

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
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_10010baf;
      }
    }
    else {
      local_c = _DAT_100257c0;
      if (*_Str == 0x2d) {
        local_c = -_DAT_100257c0;
      }
    }
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x22;
  }
  else {
    local_c = 0.0;
    if (param_2 != (undefined4 *)0x0) {
      *param_2 = param_1;
    }
  }
LAB_10010baf:
  if (local_10 != '\0') {
    *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
  }
  return (float10)local_c;
}


// 10010bc4 FUN_10010bc4

void __cdecl FUN_10010bc4(byte *param_1,undefined4 *param_2)

{
  FUN_10010aa9(param_1,param_2,(localeinfo_struct *)0x0);
  return;
}


// 10010bd7 strtoxl

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
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
LAB_10010c8e:
    bVar8 = *pbVar9;
    pbVar9 = pbVar2 + 2;
  }
  else if (bVar8 == 0x2b) goto LAB_10010c8e;
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
      goto LAB_10010cf4;
    }
    if ((*pbVar9 != 0x78) && (*pbVar9 != 0x58)) {
      param_4 = 8;
      goto LAB_10010cf4;
    }
    param_4 = 0x10;
  }
  else if ((param_4 != 0x10) || (bVar8 != 0x30)) goto LAB_10010cf4;
  if ((*pbVar9 == 0x78) || (*pbVar9 == 0x58)) {
    bVar8 = pbVar9[1];
    pbVar9 = pbVar9 + 2;
  }
LAB_10010cf4:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  do {
    uVar1 = *(ushort *)(ptVar5[1].lc_category[0].locale + (uint)bVar8 * 2);
    if ((uVar1 & 4) == 0) {
      if ((uVar1 & 0x103) == 0) {
LAB_10010d4e:
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
          pDVar3 = FUN_100125a7();
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
    if ((uint)param_4 <= uVar6) goto LAB_10010d4e;
    if ((local_8 < uVar4) ||
       ((local_8 == uVar4 && (uVar6 <= (uint)(0xffffffff % (ulonglong)(uint)param_4))))) {
      local_8 = local_8 * param_4 + uVar6;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (char **)0x0) goto LAB_10010d4e;
    }
    bVar8 = *pbVar9;
    pbVar9 = pbVar9 + 1;
  } while( true );
}


// 10010e02 _strtol

/* Library Function - Single Match
    _strtol
   
   Library: Visual Studio 2005 Release */

long __cdecl _strtol(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_100295a0 == 0) {
    ppuVar2 = &PTR_DAT_100257b8;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 10010e2b _strtoul

/* Library Function - Single Match
    _strtoul
   
   Library: Visual Studio 2005 Release */

ulong __cdecl _strtoul(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_100295a0 == 0) {
    ppuVar2 = &PTR_DAT_100257b8;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,1);
  return uVar1;
}


// 10010e60 __allmul

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


// 10010e94 __toupper_l

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
LAB_10010ef1:
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
      pDVar4 = FUN_100125a7();
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
    if (iVar3 == 0) goto LAB_10010ef1;
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


// 10010fac _toupper

/* Library Function - Single Match
    _toupper
   
   Library: Visual Studio 2005 Release */

int __cdecl _toupper(int _C)

{
  if (DAT_100295a0 == 0) {
    if (_C - 0x61U < 0x1a) {
      return _C + -0x20;
    }
  }
  else {
    _C = __toupper_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 10010fd3 FUN_10010fd3

bool FUN_10010fd3(void)

{
  return DAT_100290d0 == (DAT_10025040 | 1);
}


// 10010fe9 __forcdecpt_l

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


// 1001105a __cropzeros_l

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


// 100110f0 __fassign_l

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


// 10011147 __shift

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
    FID_conflict__memcpy(in_EAX + unaff_EDI,in_EAX,sVar1 + 1);
  }
  return;
}


// 10011180 __cftoe2_l

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
    pDVar1 = FUN_100125a7();
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
      if (((DAT_100296dc & 1) != 0) && (_Dst[2] == '0')) {
        pcVar9 = (char *)0x3;
        FID_conflict__memcpy(_Dst + 2,_Dst + 3,3);
      }
      if (local_8 == '\0') {
        return (DWORD)pcVar9;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return (DWORD)pcVar9;
    }
    pDVar1 = FUN_100125a7();
    DVar7 = 0x22;
  }
  DVar8 = 0;
  *pDVar1 = DVar7;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return DVar8;
}


// 100112ed __cftoe_l

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100113bb __cftoe

/* Library Function - Single Match
    __cftoe
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __cftoe(double *_Value,char *_Buf,size_t _SizeInBytes,int _Dec,int _Caps)

{
  errno_t eVar1;
  
  eVar1 = __cftoe_l(_Value,_Buf,_SizeInBytes,_Dec,_Caps,(localeinfo_struct *)0x0);
  return eVar1;
}


// 100113d9 __cftoa_l

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
    pDVar2 = FUN_100125a7();
    DVar15 = 0x16;
LAB_10011412:
    DVar16 = 0;
    *pDVar2 = DVar15;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_1c != '\0') {
      *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
    }
    return DVar16;
  }
  *param_2 = 0;
  if (param_3 <= param_4 + 0xb) {
    pDVar2 = FUN_100125a7();
    DVar15 = 0x22;
    goto LAB_10011412;
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
    goto LAB_10011736;
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
LAB_100116e5:
    if ((-1 < (int)uVar8) && ((0 < (int)uVar8 || (99 < uVar7)))) goto LAB_100116f0;
  }
  else {
    uVar14 = __alldvrm(uVar7,uVar8,1000,0);
    local_14 = (undefined4)((ulonglong)uVar14 >> 0x20);
    *pcVar10 = (char)uVar14 + '0';
    pcVar11 = pcVar4 + 3;
    uVar8 = 0;
    uVar7 = extraout_ECX;
    if (pcVar11 == pcVar10) goto LAB_100116e5;
LAB_100116f0:
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
LAB_10011736:
  if (local_1c == '\0') {
    return unaff_EBX;
  }
  *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  return unaff_EBX;
}


// 1001174a __cftof2_l

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
    pDVar2 = FUN_100125a7();
    uVar4 = 0x16;
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 1001183f __cftof_l

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100118f8 __cftog_l

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_34,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100119f0 __cfltcvt_l

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


// 10011a76 __cfltcvt

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


// 10011a97 __initp_misc_cfltcvt_tab

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
    piVar1 = (int *)((int)&PTR_LAB_10024c34 + uVar3);
    iVar2 = FUN_10015154(*piVar1);
    uVar3 = uVar3 + 4;
    *piVar1 = iVar2;
  } while (uVar3 < 0x28);
  return;
}


// 10011ab6 __setdefaultprecision

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


// 10011adf __ms_p5_test_fdiv

/* WARNING: Removing unreachable block (ram,0x10011b12) */
/* Library Function - Single Match
    __ms_p5_test_fdiv
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

undefined4 __ms_p5_test_fdiv(void)

{
  return 0;
}


// 10011b1b __ms_p5_mp_test_fdiv

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


// 10011b44 wfindenv

/* Library Function - Single Match
    _wfindenv
   
   Library: Visual Studio 2005 Release */

int __cdecl wfindenv(wchar_t *param_1)

{
  short sVar1;
  int iVar2;
  int *piVar3;
  size_t unaff_EDI;
  
  piVar3 = DAT_1002909c;
  while( true ) {
    if ((wchar_t *)*piVar3 == (wchar_t *)0x0) {
      return -((int)piVar3 - (int)DAT_1002909c >> 2);
    }
    iVar2 = __wcsnicoll(param_1,(wchar_t *)*piVar3,unaff_EDI);
    if ((iVar2 == 0) &&
       ((sVar1 = *(short *)(*piVar3 + unaff_EDI * 2), sVar1 == 0x3d || (sVar1 == 0)))) break;
    piVar3 = piVar3 + 1;
  }
  return (int)piVar3 - (int)DAT_1002909c >> 2;
}


// 10011b95 copy_environ

/* Library Function - Single Match
    _copy_environ
   
   Library: Visual Studio 2005 Release */

undefined4 * __cdecl copy_environ(void)

{
  undefined4 *puVar1;
  undefined4 uVar2;
  int iVar3;
  int *piVar4;
  undefined4 *puVar5;
  int *unaff_EDI;
  
  puVar1 = (undefined4 *)0x0;
  if (unaff_EDI != (int *)0x0) {
    iVar3 = *unaff_EDI;
    piVar4 = unaff_EDI;
    while (iVar3 != 0) {
      piVar4 = piVar4 + 1;
      puVar1 = (undefined4 *)((int)puVar1 + 1);
      iVar3 = *piVar4;
    }
    puVar1 = (undefined4 *)FUN_10005780((int)puVar1 + 1,4);
    if (puVar1 == (undefined4 *)0x0) {
      __amsg_exit(9);
    }
    iVar3 = *unaff_EDI;
    puVar5 = puVar1;
    while (iVar3 != 0) {
      uVar2 = FUN_10005890();
      *puVar5 = uVar2;
      puVar5 = puVar5 + 1;
      unaff_EDI = unaff_EDI + 1;
      iVar3 = *unaff_EDI;
    }
    *puVar5 = 0;
    return puVar1;
  }
  return puVar1;
}


// 10011bf2 FUN_10011bf2

int __cdecl FUN_10011bf2(undefined4 *param_1,int param_2)

{
  uint uVar1;
  wchar_t *_Str;
  DWORD *pDVar2;
  wchar_t *pwVar3;
  int iVar4;
  uint uVar5;
  int *piVar6;
  wchar_t *_Dst;
  errno_t eVar7;
  BOOL BVar8;
  DWORD DVar9;
  LPCWSTR lpWideCharStr;
  int *piVar10;
  bool bVar11;
  int iVar12;
  wchar_t *_Src;
  LPCSTR local_1c;
  LPSTR local_18;
  int iVar13;
  
  iVar13 = 0;
  if (param_1 == (undefined4 *)0x0) {
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0;
  }
  _Str = (wchar_t *)*param_1;
  if (((_Str == (wchar_t *)0x0) || (pwVar3 = _wcschr(_Str,L'='), pwVar3 == (wchar_t *)0x0)) ||
     (_Str == pwVar3)) {
LAB_10011c9a:
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    return 0;
  }
  bVar11 = pwVar3[1] == L'\0';
  if (DAT_1002909c == DAT_100290a0) {
    DAT_1002909c = copy_environ();
  }
  if (DAT_1002909c == (int *)0x0) {
    if ((param_2 == 0) || (DAT_10029094 == (undefined4 *)0x0)) {
      if (bVar11) {
        return 0;
      }
      if (DAT_10029094 == (undefined4 *)0x0) {
        DAT_10029094 = (undefined4 *)FUN_10005720(4);
        if (DAT_10029094 == (undefined4 *)0x0) {
          return 0;
        }
        *DAT_10029094 = 0;
        if (DAT_1002909c != (int *)0x0) goto LAB_10011ce8;
      }
      DAT_1002909c = (int *)FUN_10005720(4);
      if (DAT_1002909c == (int *)0x0) {
        return 0;
      }
      *DAT_1002909c = 0;
    }
    else {
      DAT_100290c4 = ___crtGetEnvironmentStringsW();
      iVar4 = __wsetenvp();
      if ((iVar4 < 0) && (iVar4 = ___mbtow_environ(), iVar4 != 0)) goto LAB_10011c9a;
    }
  }
LAB_10011ce8:
  piVar6 = DAT_1002909c;
  if (DAT_1002909c == (int *)0x0) {
    return 0;
  }
  uVar5 = wfindenv(_Str);
  if (((int)uVar5 < 0) || (*piVar6 == 0)) {
    if (bVar11) {
      FUN_10005520(_Str);
      *param_1 = 0;
      return 0;
    }
    if ((int)uVar5 < 0) {
      uVar5 = -uVar5;
    }
    uVar1 = uVar5 + 2;
    if ((int)uVar1 < (int)uVar5) {
      return 0;
    }
    if (0x3ffffffe < uVar1) {
      return 0;
    }
    piVar6 = (int *)FUN_100057b0(DAT_1002909c,4,uVar1);
    if (piVar6 == (int *)0x0) {
      return 0;
    }
    piVar6[uVar5] = (int)_Str;
    (piVar6 + uVar5)[1] = 0;
    *param_1 = 0;
LAB_10011dad:
    DAT_1002909c = piVar6;
  }
  else {
    piVar10 = piVar6 + uVar5;
    FUN_10005520(*piVar10);
    if (bVar11) {
      while (*piVar10 != 0) {
        *piVar10 = piVar10[1];
        uVar5 = uVar5 + 1;
        piVar10 = piVar6 + uVar5;
      }
      if ((uVar5 < 0x3fffffff) &&
         (piVar6 = (int *)FUN_100057b0(DAT_1002909c,uVar5,4), piVar6 != (int *)0x0))
      goto LAB_10011dad;
    }
    else {
      *piVar10 = (int)_Str;
      *param_1 = 0;
    }
  }
  if (param_2 == 0) goto LAB_10011f28;
  iVar12 = 2;
  iVar4 = FUN_10011f58(_Str);
  _Dst = (wchar_t *)FUN_10005780(iVar4 + 2,iVar12);
  if (_Dst == (wchar_t *)0x0) goto LAB_10011f28;
  _Src = _Str;
  iVar4 = FUN_10011f58(_Str);
  eVar7 = _wcscpy_s(_Dst,iVar4 + 2,_Src);
  if (eVar7 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  _Dst[(int)pwVar3 - (int)_Str >> 1] = L'\0';
  lpWideCharStr = _Dst + ((int)pwVar3 - (int)_Str >> 1) + 1;
  if ((DAT_10024c5c == 1) &&
     (BVar8 = SetEnvironmentVariableW(_Dst,(LPCWSTR)(~-(uint)bVar11 & (uint)lpWideCharStr)),
     BVar8 == 0)) {
    DVar9 = GetLastError();
    if (DVar9 != 0x78) {
      iVar13 = -1;
      goto LAB_10011e48;
    }
    DAT_10024c5c = 0;
LAB_10011e54:
    local_1c = (LPCSTR)0x0;
    local_18 = (LPSTR)0x0;
    iVar4 = WideCharToMultiByte(0,0,_Dst,-1,(LPSTR)0x0,0,(LPCSTR)0x0,(LPBOOL)0x0);
    if (((iVar4 == 0) || (local_1c = (LPCSTR)FUN_10005780(iVar4,1), local_1c == (LPCSTR)0x0)) ||
       (iVar4 = WideCharToMultiByte(0,0,_Dst,-1,local_1c,iVar4,(LPCSTR)0x0,(LPBOOL)0x0), iVar4 == 0)
       ) {
LAB_10011ef7:
      iVar13 = -1;
    }
    else {
      if (!bVar11) {
        iVar4 = WideCharToMultiByte(0,0,lpWideCharStr,-1,(LPSTR)0x0,0,(LPCSTR)0x0,(LPBOOL)0x0);
        if ((iVar4 == 0) || (local_18 = (LPSTR)FUN_10005780(iVar4,1), local_18 == (LPSTR)0x0))
        goto LAB_10011ef7;
        iVar4 = WideCharToMultiByte(0,0,lpWideCharStr,-1,local_18,iVar4,(LPCSTR)0x0,(LPBOOL)0x0);
        if (iVar4 == 0) {
          iVar13 = -1;
        }
      }
      if ((iVar13 != -1) &&
         (BVar8 = SetEnvironmentVariableA(local_1c,(LPCSTR)(~-(uint)bVar11 & (uint)local_18)),
         BVar8 == 0)) goto LAB_10011ef7;
    }
    FUN_10005520(local_18);
    FUN_10005520(local_1c);
  }
  else {
LAB_10011e48:
    if (DAT_10024c5c == 0) goto LAB_10011e54;
  }
  if (iVar13 == -1) {
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x2a;
  }
  FUN_10005520(_Dst);
LAB_10011f28:
  if (bVar11) {
    FUN_10005520(_Str);
    *param_1 = 0;
  }
  return iVar13;
}


// 10011f58 FUN_10011f58

int __cdecl FUN_10011f58(short *param_1)

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


// 10011f6e findenv

/* Library Function - Single Match
    _findenv
   
   Library: Visual Studio 2005 Release */

int __cdecl findenv(uchar *param_1)

{
  int iVar1;
  int *piVar2;
  size_t unaff_EDI;
  
  piVar2 = DAT_10029094;
  while( true ) {
    if ((uchar *)*piVar2 == (uchar *)0x0) {
      return -((int)piVar2 - (int)DAT_10029094 >> 2);
    }
    iVar1 = __mbsnbicoll(param_1,(uchar *)*piVar2,unaff_EDI);
    if ((iVar1 == 0) &&
       ((*(char *)(unaff_EDI + *piVar2) == '=' || (*(char *)(unaff_EDI + *piVar2) == '\0')))) break;
    piVar2 = piVar2 + 1;
  }
  return (int)piVar2 - (int)DAT_10029094 >> 2;
}


// 10011fbb copy_environ

/* Library Function - Single Match
    _copy_environ
   
   Library: Visual Studio 2005 Release */

undefined4 * __cdecl copy_environ(void)

{
  int iVar1;
  undefined4 *puVar2;
  void *pvVar3;
  char *pcVar4;
  int *piVar5;
  undefined4 *puVar6;
  int *unaff_EDI;
  
  puVar2 = (undefined4 *)0x0;
  if (unaff_EDI != (int *)0x0) {
    iVar1 = *unaff_EDI;
    piVar5 = unaff_EDI;
    while (iVar1 != 0) {
      piVar5 = piVar5 + 1;
      puVar2 = (undefined4 *)((int)puVar2 + 1);
      iVar1 = *piVar5;
    }
    puVar2 = (undefined4 *)FUN_10005780((int)puVar2 + 1,4);
    if (puVar2 == (undefined4 *)0x0) {
      __amsg_exit(9);
    }
    pcVar4 = (char *)*unaff_EDI;
    puVar6 = puVar2;
    while (pcVar4 != (char *)0x0) {
      pvVar3 = FUN_10005810(pcVar4);
      *puVar6 = pvVar3;
      puVar6 = puVar6 + 1;
      unaff_EDI = unaff_EDI + 1;
      pcVar4 = (char *)*unaff_EDI;
    }
    *puVar6 = 0;
    return puVar2;
  }
  return puVar2;
}


// 10012018 FUN_10012018

undefined4 __cdecl FUN_10012018(undefined4 *param_1,int param_2)

{
  uint uVar1;
  uchar *_Str;
  DWORD *pDVar2;
  uchar *puVar3;
  int iVar4;
  uint uVar5;
  int *piVar6;
  size_t sVar7;
  char *_Dst;
  errno_t eVar8;
  BOOL BVar9;
  int *piVar10;
  bool bVar11;
  uchar *_Src;
  undefined4 local_14;
  
  local_14 = 0;
  if (param_1 == (undefined4 *)0x0) {
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0xffffffff;
  }
  _Str = (uchar *)*param_1;
  if (((_Str == (uchar *)0x0) || (puVar3 = __mbschr(_Str,0x3d), puVar3 == (uchar *)0x0)) ||
     (_Str == puVar3)) {
LAB_100120a6:
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    return 0xffffffff;
  }
  bVar11 = puVar3[1] == '\0';
  if (DAT_10029094 == DAT_10029098) {
    DAT_10029094 = copy_environ();
  }
  if (DAT_10029094 == (int *)0x0) {
    if ((param_2 == 0) || (DAT_1002909c == (undefined4 *)0x0)) {
      if (bVar11) {
        return 0;
      }
      DAT_10029094 = (int *)FUN_10005720(4);
      if (DAT_10029094 == (int *)0x0) {
        return 0xffffffff;
      }
      *DAT_10029094 = 0;
      if (DAT_1002909c == (undefined4 *)0x0) {
        DAT_1002909c = (undefined4 *)FUN_10005720(4);
        if (DAT_1002909c == (undefined4 *)0x0) {
          return 0xffffffff;
        }
        *DAT_1002909c = 0;
      }
    }
    else {
      iVar4 = ___wtomb_environ();
      if (iVar4 != 0) goto LAB_100120a6;
    }
  }
  piVar6 = DAT_10029094;
  if (DAT_10029094 == (int *)0x0) {
    return 0xffffffff;
  }
  uVar5 = findenv(_Str);
  if (((int)uVar5 < 0) || (*piVar6 == 0)) {
    if (bVar11) {
      FUN_10005520(_Str);
      *param_1 = 0;
      return 0;
    }
    if ((int)uVar5 < 0) {
      uVar5 = -uVar5;
    }
    uVar1 = uVar5 + 2;
    if ((int)uVar1 < (int)uVar5) {
      return 0xffffffff;
    }
    if (0x3ffffffe < uVar1) {
      return 0xffffffff;
    }
    piVar6 = (int *)FUN_100057b0(DAT_10029094,4,uVar1);
    if (piVar6 == (int *)0x0) {
      return 0xffffffff;
    }
    piVar6[uVar5] = (int)_Str;
    (piVar6 + uVar5)[1] = 0;
    *param_1 = 0;
  }
  else {
    piVar10 = piVar6 + uVar5;
    FUN_10005520(*piVar10);
    if (!bVar11) {
      *piVar10 = (int)_Str;
      *param_1 = 0;
      goto LAB_100121b7;
    }
    while (*piVar10 != 0) {
      *piVar10 = piVar10[1];
      uVar5 = uVar5 + 1;
      piVar10 = piVar6 + uVar5;
    }
    if ((0x3ffffffe < uVar5) ||
       (piVar6 = (int *)FUN_100057b0(DAT_10029094,uVar5,4), piVar6 == (int *)0x0))
    goto LAB_100121b7;
  }
  DAT_10029094 = piVar6;
LAB_100121b7:
  if (param_2 != 0) {
    iVar4 = 1;
    sVar7 = _strlen((char *)_Str);
    _Dst = (char *)FUN_10005780(sVar7 + 2,iVar4);
    if (_Dst != (char *)0x0) {
      _Src = _Str;
      sVar7 = _strlen((char *)_Str);
      eVar8 = _strcpy_s(_Dst,sVar7 + 2,(char *)_Src);
      if (eVar8 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      puVar3[(int)_Dst - (int)_Str] = '\0';
      BVar9 = SetEnvironmentVariableA
                        (_Dst,(LPCSTR)(~-(uint)bVar11 & (uint)(puVar3 + ((int)_Dst - (int)_Str) + 1)
                                      ));
      if (BVar9 == 0) {
        local_14 = 0xffffffff;
        pDVar2 = FUN_100125a7();
        *pDVar2 = 0x2a;
      }
      FUN_10005520(_Dst);
    }
  }
  if (bVar11) {
    FUN_10005520(_Str);
    *param_1 = 0;
    return local_14;
  }
  return local_14;
}


// 10012264 _strcpy_s

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
      pDVar2 = FUN_100125a7();
      eVar4 = 0x22;
      *pDVar2 = 0x22;
      goto LAB_10012283;
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_100125a7();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_10012283:
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 100122d0 _strlen

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
    if ((char)uVar1 == '\0') goto LAB_10012333;
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
LAB_10012333:
  return (size_t)((int)puVar2 + (-1 - (int)_Str));
}


// 1001235b _strnlen

/* Library Function - Single Match
    _strnlen
   
   Library: Visual Studio 2005 Release */

size_t __cdecl _strnlen(char *_Str,size_t _MaxCount)

{
  uint uVar1;
  
  uVar1 = 0;
  if (_MaxCount != 0) {
    do {
      if (*_Str == '\0') {
        return uVar1;
      }
      uVar1 = uVar1 + 1;
      _Str = _Str + 1;
    } while (uVar1 < _MaxCount);
  }
  return uVar1;
}


// 10012375 __mbschr_l

/* Library Function - Single Match
    __mbschr_l
   
   Library: Visual Studio 2005 Release */

uchar * __cdecl __mbschr_l(uchar *_Str,uint _Ch,_locale_t _Locale)

{
  byte bVar1;
  byte bVar2;
  DWORD *pDVar3;
  byte *pbVar4;
  _LocaleUpdate local_14 [4];
  int local_10;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate(local_14,_Locale);
  if (_Str == (uchar *)0x0) {
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    _Str = (byte *)0x0;
  }
  else {
    if (*(int *)(local_10 + 8) == 0) {
      _Str = (uchar *)_strchr((char *)_Str,_Ch);
    }
    else {
      while( true ) {
        bVar2 = *_Str;
        if (bVar2 == 0) break;
        if ((*(byte *)(bVar2 + 0x1d + local_10) & 4) == 0) {
          pbVar4 = _Str;
          if (_Ch == bVar2) break;
        }
        else {
          bVar1 = _Str[1];
          if (bVar1 == 0) goto LAB_1001241d;
          pbVar4 = _Str + 1;
          if (_Ch == CONCAT11(bVar2,bVar1)) goto LAB_1001240f;
        }
        _Str = pbVar4 + 1;
      }
      if (_Ch != (ushort)bVar2) {
LAB_1001241d:
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        return (uchar *)0x0;
      }
    }
LAB_1001240f:
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return _Str;
}


// 1001242f __mbschr

/* Library Function - Single Match
    __mbschr
   
   Library: Visual Studio 2005 Release */

uchar * __cdecl __mbschr(uchar *_Str,uint _Ch)

{
  uchar *puVar1;
  
  puVar1 = __mbschr_l(_Str,_Ch,(_locale_t)0x0);
  return puVar1;
}


// 10012442 FUN_10012442

void __cdecl FUN_10012442(undefined4 param_1)

{
  DAT_100290d4 = param_1;
  return;
}


// 1001244c __invoke_watson

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
  
  iVar1 = DAT_10025040;
  local_2d4 = 0x10001;
  _memset(&local_32c,0,0x50);
  local_2dc.ExceptionRecord = &local_32c;
  local_2dc.ContextRecord = (PCONTEXT)&local_2d4;
  local_32c.ExceptionCode = 0xc000000d;
  BVar2 = IsDebuggerPresent();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  LVar3 = UnhandledExceptionFilter(&local_2dc);
  if ((LVar3 == 0) && (BVar2 == 0)) {
    FUN_10018b17();
  }
  uExitCode = 0xc000000d;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  __security_check_cookie(iVar1);
  return;
}


// 10012548 FUN_10012548

void __cdecl
FUN_10012548(wchar_t *param_1,wchar_t *param_2,wchar_t *param_3,uint param_4,uintptr_t param_5)

{
  code *UNRECOVERED_JUMPTABLE;
  
  UNRECOVERED_JUMPTABLE = (code *)FUN_100151cb(DAT_100290d4);
  if (UNRECOVERED_JUMPTABLE != (code *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x1001255c. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (*UNRECOVERED_JUMPTABLE)();
    return;
  }
  FUN_10018b17();
                    /* WARNING: Subroutine does not return */
  __invoke_watson(param_1,param_2,param_3,param_4,param_5);
}


// 1001256c FUN_1001256c

int __cdecl FUN_1001256c(int param_1)

{
  uint uVar1;
  
  uVar1 = 0;
  do {
    if (param_1 == (&DAT_10024c60)[uVar1 * 2]) {
      return *(int *)(uVar1 * 8 + 0x10024c64);
    }
    uVar1 = uVar1 + 1;
  } while (uVar1 < 0x2d);
  if (param_1 - 0x13U < 0x12) {
    return 0xd;
  }
  return (-(uint)(0xe < param_1 - 0xbcU) & 0xe) + 8;
}


// 100125a7 FUN_100125a7

DWORD * FUN_100125a7(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_10015370();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10024dc8;
  }
  return pDVar1 + 2;
}


// 100125ba FUN_100125ba

DWORD * FUN_100125ba(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_10015370();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_10024dcc;
  }
  return pDVar1 + 3;
}


// 100125cd FUN_100125cd

void __cdecl FUN_100125cd(DWORD param_1)

{
  DWORD *pDVar1;
  DWORD DVar2;
  
  pDVar1 = FUN_100125ba();
  *pDVar1 = param_1;
  DVar2 = FUN_1001256c(param_1);
  pDVar1 = FUN_100125a7();
  *pDVar1 = DVar2;
  return;
}


// 100125eb __mtinitlocks

/* Library Function - Single Match
    __mtinitlocks
   
   Library: Visual Studio 2005 Release */

int __cdecl __mtinitlocks(void)

{
  int iVar1;
  int iVar2;
  undefined *puVar3;
  
  iVar2 = 0;
  puVar3 = &DAT_100290d8;
  do {
    if ((&DAT_10024dd4)[iVar2 * 2] == 1) {
      (&DAT_10024dd0)[iVar2 * 2] = puVar3;
      puVar3 = puVar3 + 0x18;
      iVar1 = ___crtInitCritSecAndSpinCount((&DAT_10024dd0)[iVar2 * 2],4000);
      if (iVar1 == 0) {
        (&DAT_10024dd0)[iVar2 * 2] = 0;
        return 0;
      }
    }
    iVar2 = iVar2 + 1;
  } while (iVar2 < 0x24);
  return 1;
}


// 10012634 __mtdeletelocks

/* Library Function - Single Match
    __mtdeletelocks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __cdecl __mtdeletelocks(void)

{
  LPCRITICAL_SECTION lpCriticalSection;
  undefined4 *puVar1;
  
  puVar1 = &DAT_10024dd0;
  do {
    lpCriticalSection = (LPCRITICAL_SECTION)*puVar1;
    if ((lpCriticalSection != (LPCRITICAL_SECTION)0x0) && (puVar1[1] != 1)) {
      DeleteCriticalSection(lpCriticalSection);
      FUN_10005520(lpCriticalSection);
      *puVar1 = 0;
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10024ef0);
  puVar1 = &DAT_10024dd0;
  do {
    if (((LPCRITICAL_SECTION)*puVar1 != (LPCRITICAL_SECTION)0x0) && (puVar1[1] == 1)) {
      DeleteCriticalSection((LPCRITICAL_SECTION)*puVar1);
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x10024ef0);
  return;
}


// 10012689 FUN_10012689

void __cdecl FUN_10012689(int param_1)

{
  LeaveCriticalSection((LPCRITICAL_SECTION)(&DAT_10024dd0)[param_1 * 2]);
  return;
}


// 1001269e __mtinitlocknum

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
  if (DAT_10024128 == 0) {
    __FF_MSGBANNER();
    FUN_1001570d(0x1e);
    ___crtExitProcess(0xff);
  }
  piVar1 = &DAT_10024dd0 + _LockNum * 2;
  if (*piVar1 == 0) {
    iVar4 = FUN_10005720(0x18);
    if (iVar4 == 0) {
      pDVar2 = FUN_100125a7();
      *pDVar2 = 0xc;
      iVar4 = 0;
    }
    else {
      __lock(10);
      if (*piVar1 == 0) {
        iVar3 = ___crtInitCritSecAndSpinCount(iVar4,4000);
        if (iVar3 == 0) {
          FUN_10005520(iVar4);
          pDVar2 = FUN_100125a7();
          *pDVar2 = 0xc;
          local_20 = 0;
        }
        else {
          *piVar1 = iVar4;
        }
      }
      else {
        FUN_10005520(iVar4);
      }
      FUN_10012758();
      iVar4 = local_20;
    }
  }
  return iVar4;
}


// 10012758 FUN_10012758

void FUN_10012758(void)

{
  FUN_10012689(10);
  return;
}


// 10012761 __lock

/* Library Function - Single Match
    __lock
   
   Library: Visual Studio 2005 Release */

void __cdecl __lock(int _File)

{
  int iVar1;
  
  if ((&DAT_10024dd0)[_File * 2] == 0) {
    iVar1 = __mtinitlocknum(_File);
    if (iVar1 == 0) {
      __amsg_exit(0x11);
    }
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(&DAT_10024dd0)[_File * 2]);
  return;
}


// 10012794 __SEH_prolog4

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
  *(uint *)((int)auStack_1c + iVar1 + 4) = DAT_10025040 ^ (uint)&param_2;
  *(undefined4 *)((int)auStack_1c + iVar1) = unaff_retaddr;
  *unaff_FS_OFFSET = (int)local_8;
  return;
}


// 100127d9 __SEH_epilog4

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


// 100127f0 __except_handler4

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
  
  piVar8 = (int *)(*(uint *)((int)param_2 + 8) ^ DAT_10025040);
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
          goto LAB_1001289b;
        }
        if (0 < iVar4) {
          if (((*param_1 == -0x1f928c9d) && (DAT_10029c48 != (code *)0x0)) &&
             (BVar5 = __IsNonwritableInCurrentImage((PBYTE)&DAT_10029c48), BVar5 != 0)) {
            (*DAT_10029c48)(param_1,1);
          }
          _EH4_GlobalUnwind(param_2);
          if (*(uint *)((int)param_2 + 0xc) != uVar7) {
            _EH4_LocalUnwind((int)param_2,uVar7,iVar1,&DAT_10025040);
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
    _EH4_LocalUnwind((int)param_2,0xfffffffe,iVar1,&DAT_10025040);
  }
LAB_1001289b:
  if (*piVar8 != -2) {
    __security_check_cookie(piVar8[1] + iVar1 ^ *(uint *)(*piVar8 + iVar1));
  }
  __security_check_cookie(piVar8[3] + iVar1 ^ *(uint *)(piVar8[2] + iVar1));
  return local_c;
}


// 10012986 __getdrive

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
  
  local_8 = DAT_10025040 ^ (uint)local_98;
  bVar1 = false;
  lpBuffer = local_110;
  DVar2 = GetCurrentDirectoryA(0x105,(LPSTR)lpBuffer);
  if (0x104 < (int)DVar2) {
    lpBuffer = (byte *)FUN_10005780(DVar2 + 1,1);
    if (lpBuffer == (byte *)0x0) {
      pDVar3 = FUN_100125a7();
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
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0xc;
  }
  else if (lpBuffer[1] == 0x3a) {
    _toupper((uint)*lpBuffer);
  }
  if (bVar1) {
    FUN_10005520(lpBuffer);
  }
  iVar4 = __security_check_cookie(local_8 ^ (uint)local_98);
  return iVar4;
}


// 10012a4c __mbsnbicoll_l

/* Library Function - Single Match
    __mbsnbicoll_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsnbicoll_l(uchar *_Str1,uchar *_Str2,size_t _MaxCount,_locale_t _Locale)

{
  DWORD *pDVar1;
  int iVar2;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  if (_MaxCount == 0) {
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    return 0;
  }
  if ((_Str1 == (uchar *)0x0) || (_Str2 == (uchar *)0x0)) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    return 0x7fffffff;
  }
  if (_MaxCount < 0x80000000) {
    if ((local_14.mbcinfo)->ismbcodepage == 0) {
      iVar2 = __strnicoll_l((char *)_Str1,(char *)_Str2,_MaxCount,_Locale);
    }
    else {
      iVar2 = ___crtCompareStringA
                        (&local_14,*(LPCWSTR *)(local_14.mbcinfo)->mbulinfo,0x1001,(LPCSTR)_Str1,
                         _MaxCount,(LPCSTR)_Str2,_MaxCount,(local_14.mbcinfo)->mbcodepage);
      if (iVar2 == 0) goto LAB_10012b15;
      iVar2 = iVar2 + -2;
    }
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  else {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
LAB_10012b15:
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    iVar2 = 0x7fffffff;
  }
  return iVar2;
}


// 10012b38 __mbsnbicoll

/* Library Function - Single Match
    __mbsnbicoll
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsnbicoll(uchar *_Str1,uchar *_Str2,size_t _MaxCount)

{
  int iVar1;
  
  iVar1 = __mbsnbicoll_l(_Str1,_Str2,_MaxCount,(_locale_t)0x0);
  return iVar1;
}


// 10012b4f ___wtomb_environ

/* Library Function - Single Match
    ___wtomb_environ
   
   Library: Visual Studio 2005 Release */

int __cdecl ___wtomb_environ(void)

{
  LPCWSTR lpWideCharStr;
  int iVar1;
  undefined4 *puVar2;
  LPSTR local_8;
  
  local_8 = (LPSTR)0x0;
  lpWideCharStr = (LPCWSTR)*DAT_1002909c;
  puVar2 = DAT_1002909c;
  while( true ) {
    if (lpWideCharStr == (LPCWSTR)0x0) {
      return 0;
    }
    iVar1 = WideCharToMultiByte(0,0,lpWideCharStr,-1,(LPSTR)0x0,0,(LPCSTR)0x0,(LPBOOL)0x0);
    if ((iVar1 == 0) || (local_8 = (LPSTR)FUN_10005780(iVar1,1), local_8 == (LPSTR)0x0)) break;
    iVar1 = WideCharToMultiByte(0,0,(LPCWSTR)*puVar2,-1,local_8,iVar1,(LPCSTR)0x0,(LPBOOL)0x0);
    if (iVar1 == 0) {
      FUN_10005520(local_8);
      return -1;
    }
    iVar1 = FUN_10012018(&local_8,0);
    if ((iVar1 < 0) && (local_8 != (LPSTR)0x0)) {
      FUN_10005520(local_8);
      local_8 = (LPSTR)0x0;
    }
    puVar2 = puVar2 + 1;
    lpWideCharStr = (LPCWSTR)*puVar2;
  }
  return -1;
}


// 10012be4 comexecmd

/* Library Function - Single Match
    _comexecmd
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl comexecmd(int param_1,char *param_2,char **param_3,char **param_4)

{
  DWORD *pDVar1;
  int iVar2;
  intptr_t iVar3;
  char *local_c;
  char *local_8;
  
  if ((param_2 == (char *)0x0) || (param_3 == (char **)0x0)) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    iVar2 = __cenvarg(param_3,param_4,&local_8,&local_c,param_2);
    if (iVar2 != -1) {
      iVar3 = __dospawn(param_1,param_2,local_8,local_c);
      FUN_10005520(local_8);
      FUN_10005520(local_c);
      return iVar3;
    }
  }
  return -1;
}


// 10012c5c __spawnve

/* Library Function - Single Match
    __spawnve
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __spawnve(int _Mode,char *_Filename,char **_ArgList,char **_Env)

{
  DWORD DVar1;
  DWORD *pDVar2;
  uchar *puVar3;
  uchar *puVar4;
  size_t sVar5;
  errno_t eVar6;
  char *_Dst;
  rsize_t rVar7;
  intptr_t local_14;
  undefined **local_10;
  char *local_8;
  
  local_8 = _Filename;
  if ((((_Filename == (char *)0x0) || (*_Filename == '\0')) || (_ArgList == (char **)0x0)) ||
     ((*_ArgList == (char *)0x0 || (**_ArgList == '\0')))) {
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  puVar3 = __mbsrchr((uchar *)_Filename,0x5c);
  puVar4 = __mbsrchr((uchar *)_Filename,0x2f);
  if (puVar4 == (uchar *)0x0) {
    if ((puVar3 != (uchar *)0x0) ||
       (puVar3 = __mbschr((uchar *)_Filename,0x3a), puVar3 != (uchar *)0x0)) goto LAB_10012d4e;
    sVar5 = _strlen(_Filename);
    rVar7 = sVar5 + 3;
    local_8 = (char *)FUN_10005780(rVar7,1);
    if (local_8 != (char *)0x0) {
      eVar6 = _strcpy_s(local_8,rVar7,".\\");
      if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      eVar6 = _strcat_s(local_8,rVar7,_Filename);
      if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      puVar3 = (uchar *)(local_8 + 2);
      goto LAB_10012d4e;
    }
LAB_10012dae:
    local_14 = -1;
  }
  else {
    if ((puVar3 == (uchar *)0x0) || (puVar3 < puVar4)) {
      puVar3 = puVar4;
    }
LAB_10012d4e:
    local_14 = -1;
    puVar3 = __mbsrchr(puVar3,0x2e);
    if (puVar3 == (uchar *)0x0) {
      sVar5 = _strlen(local_8);
      rVar7 = sVar5 + 5;
      _Dst = (char *)FUN_10005780(rVar7,1);
      if (_Dst == (char *)0x0) goto LAB_10012dae;
      eVar6 = _strcpy_s(_Dst,rVar7,local_8);
      if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      sVar5 = _strlen(local_8);
      pDVar2 = FUN_100125a7();
      DVar1 = *pDVar2;
      local_10 = &PTR_DAT_10024efc;
      do {
        eVar6 = _strcpy_s(_Dst + sVar5,(rsize_t)(_Dst + (rVar7 - (int)(_Dst + sVar5))),*local_10);
        if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
        eVar6 = __access_s(_Dst,0);
        if (eVar6 == 0) {
          pDVar2 = FUN_100125a7();
          *pDVar2 = DVar1;
          local_14 = comexecmd(_Mode,_Dst,_ArgList,_Env);
          break;
        }
        local_10 = local_10 + -1;
      } while (0x10024eef < (int)local_10);
      FUN_10005520(_Dst);
    }
    else {
      eVar6 = __access_s(local_8,0);
      if (eVar6 == 0) {
        local_14 = comexecmd(_Mode,local_8,_ArgList,_Env);
      }
    }
    if (local_8 != _Filename) {
      FUN_10005520(local_8);
    }
  }
  return local_14;
}


// 10012e7b FUN_10012e7b

void __cdecl FUN_10012e7b(undefined4 *param_1,undefined4 *param_2,uint param_3)

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


// 10012f02 __VEC_memcpy

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
      FUN_10012e7b(param_1,param_2,param_3 - uVar3);
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


// 10012ff9 __flsbuf

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
    pDVar2 = FUN_100125a7();
    *pDVar2 = 9;
LAB_1001301d:
    _File_00->_flag = _File_00->_flag | 0x20;
    return -1;
  }
  if ((uVar6 & 0x40) != 0) {
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x22;
    goto LAB_1001301d;
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
     (((ppuVar3 = FUN_1000f7b5(), _File_00 != (FILE *)(ppuVar3 + 8) &&
       (ppuVar3 = FUN_1000f7b5(), _File_00 != (FILE *)(ppuVar3 + 0x10))) ||
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
        puVar5 = &DAT_10025008;
      }
      else {
        puVar5 = (undefined *)(((uint)_File & 0x1f) * 0x38 + (&DAT_10029b40)[(int)_File >> 5]);
      }
      if (((puVar5[4] & 0x20) != 0) &&
         (lVar7 = __lseeki64((int)_File,0x200000000,unaff_EDI), lVar7 == -1)) goto LAB_10013143;
    }
    else {
      local_8 = __write((int)_File,_Buf,uVar6);
    }
    *_File_00->_base = (char)_Ch;
  }
  if (local_8 == uVar6) {
    return _Ch & 0xff;
  }
LAB_10013143:
  _File_00->_flag = _File_00->_flag | 0x20;
  return -1;
}


// 10013159 write_char

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


// 1001318c write_multi_char

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


// 100131b0 write_string

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
        pDVar1 = FUN_100125a7();
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


// 100131fa FUN_100131fa

/* WARNING: Type propagation algorithm not settling */

void __cdecl FUN_100131fa(FILE *param_1,byte *param_2,localeinfo_struct *param_3,wchar_t *param_4)

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
  
  local_8 = DAT_10025040 ^ (uint)local_1fc;
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
        puVar7 = &DAT_10025008;
      }
      else {
        iVar5 = __fileno(local_22c);
        uVar6 = __fileno(local_22c);
        puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_10029b40)[iVar5 >> 5]);
      }
      if ((puVar7[0x24] & 0x7f) == 0) {
        iVar5 = __fileno(local_22c);
        if ((iVar5 == -1) || (iVar5 = __fileno(local_22c), iVar5 == -2)) {
          puVar7 = &DAT_10025008;
        }
        else {
          iVar5 = __fileno(local_22c);
          uVar6 = __fileno(local_22c);
          puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_10029b40)[iVar5 >> 5]);
        }
        if ((puVar7[0x24] & 0x80) == 0) goto LAB_10013338;
      }
    }
    else {
LAB_10013338:
      if (param_2 != (byte *)0x0) {
        local_215 = *param_2;
        local_230 = 0;
        local_224 = 0;
        local_250 = (wchar_t *)0x0;
        iVar5 = 0;
        while ((local_215 != 0 && (pbVar12 = param_2 + 1, local_244 = pbVar12, -1 < local_230))) {
          if ((byte)(local_215 - 0x20) < 0x59) {
            uVar6 = (byte)(&DAT_10020c30)[(char)local_215] & 0xf;
          }
          else {
            uVar6 = 0;
          }
          local_270 = (int)(char)(&DAT_10020c50)[uVar6 * 8 + iVar5] >> 4;
          switch(local_270) {
          case 0:
switchD_1001339c_caseD_0:
            local_240 = 0;
            iVar5 = __isleadbyte_l((uint)local_215,&local_260);
            if (iVar5 != 0) {
              write_char();
              local_244 = param_2 + 2;
              if (*pbVar12 == 0) {
LAB_10013b2f:
                pDVar4 = FUN_100125a7();
                *pDVar4 = 0x16;
                goto LAB_1001326a;
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
                goto switchD_1001339c_caseD_0;
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
LAB_1001377b:
                local_214 = local_214 | 0x40;
LAB_1001377f:
                local_224 = 10;
LAB_10013786:
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
                  goto LAB_10013604;
                }
                if (local_215 == 0x41) {
LAB_100135b0:
                  local_215 = local_215 + 0x20;
                  local_274 = 1;
LAB_100135bd:
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
                      local_250 = (wchar_t *)FUN_10005720(sVar13);
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
                  pcVar9 = (code *)FUN_100151cb((int)PTR_LAB_10024c4c);
                  (*pcVar9)(puVar17,pwVar18,sVar13,iVar8,iVar5,uVar19,plVar20);
                  uVar6 = local_214 & 0x80;
                  if ((uVar6 != 0) && (local_21c == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_100151cb((int)PTR_LAB_10024c58);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((local_215 == 0x67) && (uVar6 == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_100151cb((int)PTR_LAB_10024c54);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((char)*pwVar14 == '-') {
                    local_214 = local_214 | 0x100;
                    local_220 = (wchar_t *)((int)pwVar14 + 1);
                    pwVar14 = local_220;
                  }
LAB_100136f1:
                  local_224 = _strlen((char *)pwVar14);
                }
                else if (local_215 == 0x43) {
                  if ((local_214 & 0x830) == 0) {
                    local_214 = local_214 | 0x800;
                  }
LAB_10013661:
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
                else if ((local_215 == 0x45) || (local_215 == 0x47)) goto LAB_100135b0;
              }
              else {
                if (local_215 == 0x58) goto LAB_10013886;
                if (local_215 == 0x5a) {
                  psVar2 = *(short **)param_4;
                  local_228 = param_4 + 2;
                  pwVar14 = (wchar_t *)PTR_s__null__10024f00;
                  local_220 = (wchar_t *)PTR_s__null__10024f00;
                  if ((psVar2 == (short *)0x0) ||
                     (pwVar18 = *(wchar_t **)(psVar2 + 2), pwVar18 == (wchar_t *)0x0))
                  goto LAB_100136f1;
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
                  if (local_215 == 0x61) goto LAB_100135bd;
                  if (local_215 == 99) goto LAB_10013661;
                }
              }
LAB_100139fe:
              if (local_24c == 0) {
                if ((local_214 & 0x40) != 0) {
                  if ((local_214 & 0x100) == 0) {
                    if ((local_214 & 1) == 0) {
                      if ((local_214 & 2) == 0) goto LAB_10013a34;
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
LAB_10013a34:
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
LAB_10013604:
                  iVar5 = local_21c;
                  if (local_21c == -1) {
                    iVar5 = 0x7fffffff;
                  }
                  local_228 = param_4 + 2;
                  local_220 = *(wchar_t **)param_4;
                  if ((local_214 & 0x810) == 0) {
                    pwVar14 = local_220;
                    if (local_220 == (wchar_t *)0x0) {
                      pwVar14 = (wchar_t *)PTR_s__null__10024f00;
                      local_220 = (wchar_t *)PTR_s__null__10024f00;
                    }
                    for (; (iVar5 != 0 && (iVar5 = iVar5 + -1, (char)*pwVar14 != '\0'));
                        pwVar14 = (wchar_t *)((int)pwVar14 + 1)) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220;
                  }
                  else {
                    if (local_220 == (wchar_t *)0x0) {
                      local_220 = (wchar_t *)PTR_u__null__10024f04;
                    }
                    local_240 = 1;
                    for (pwVar14 = local_220;
                        (iVar5 != 0 && (iVar5 = iVar5 + -1, *pwVar14 != L'\0'));
                        pwVar14 = pwVar14 + 1) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220 >> 1;
                  }
                  goto LAB_100139fe;
                }
                if (local_215 == 0x75) goto LAB_1001377f;
                if (local_215 != 0x78) goto LAB_100139fe;
                local_248 = 0x27;
LAB_100138ac:
                local_224 = 0x10;
                if ((local_214 & 0x80) != 0) {
                  local_233 = (char)local_248 + 'Q';
                  local_234 = 0x30;
                  local_238 = 2;
                }
                goto LAB_10013786;
              }
              if (local_215 == 0x70) {
                local_21c = 8;
LAB_10013886:
                local_248 = 7;
                goto LAB_100138ac;
              }
              if ((char)local_215 < 'e') goto LAB_100139fe;
              if ((char)local_215 < 'h') goto LAB_100135bd;
              if (local_215 == 0x69) goto LAB_1001377b;
              if (local_215 != 0x6e) {
                if (local_215 != 0x6f) goto LAB_100139fe;
                local_224 = 8;
                if ((local_214 & 0x80) != 0) {
                  local_214 = local_214 | 0x200;
                }
                goto LAB_10013786;
              }
              piVar3 = *(int **)param_4;
              local_228 = param_4 + 2;
              bVar15 = FUN_10010fd3();
              if (CONCAT31(extraout_var,bVar15) == 0) goto LAB_10013b2f;
              if ((local_214 & 0x20) == 0) {
                *piVar3 = local_230;
              }
              else {
                *(undefined2 *)piVar3 = (undefined2)local_230;
              }
              local_24c = 1;
            }
            if (local_250 != (wchar_t *)0x0) {
              FUN_10005520(local_250);
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
        goto LAB_10013b56;
      }
    }
  }
  pDVar4 = FUN_100125a7();
  *pDVar4 = 0x16;
LAB_1001326a:
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_254 != '\0') {
    *(uint *)(local_258 + 0x70) = *(uint *)(local_258 + 0x70) & 0xfffffffd;
  }
LAB_10013b56:
  __security_check_cookie(local_8 ^ (uint)local_1fc);
  return;
}


// 10013b8e __close_nolock

/* Library Function - Single Match
    __close_nolock
   
   Library: Visual Studio 2005 Release */

int __cdecl __close_nolock(int _FileHandle)

{
  intptr_t iVar1;
  intptr_t iVar2;
  HANDLE hObject;
  BOOL BVar3;
  DWORD DVar4;
  int iVar5;
  
  iVar1 = __get_osfhandle(_FileHandle);
  if (iVar1 != -1) {
    if (((_FileHandle == 1) && ((*(byte *)(DAT_10029b40 + 0x74) & 1) != 0)) ||
       ((_FileHandle == 2 && ((*(byte *)(DAT_10029b40 + 0x3c) & 1) != 0)))) {
      iVar1 = __get_osfhandle(2);
      iVar2 = __get_osfhandle(1);
      if (iVar2 == iVar1) goto LAB_10013bed;
    }
    hObject = (HANDLE)__get_osfhandle(_FileHandle);
    BVar3 = CloseHandle(hObject);
    if (BVar3 == 0) {
      DVar4 = GetLastError();
      goto LAB_10013bef;
    }
  }
LAB_10013bed:
  DVar4 = 0;
LAB_10013bef:
  __free_osfhnd(_FileHandle);
  *(undefined1 *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38) = 0;
  if (DVar4 == 0) {
    iVar5 = 0;
  }
  else {
    FUN_100125cd(DVar4);
    iVar5 = -1;
  }
  return iVar5;
}


// 10013c22 __close

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __close
   
   Library: Visual Studio 2005 Release */

int __cdecl __close(int _FileHandle)

{
  DWORD *pDVar1;
  int iVar2;
  int local_20;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10029b38)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_100125a7();
          *pDVar1 = 9;
          local_20 = -1;
        }
        else {
          local_20 = __close_nolock(_FileHandle);
        }
        FUN_10013ce5();
        return local_20;
      }
    }
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 10013ce5 FUN_10013ce5

void FUN_10013ce5(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 10013cef __fileno

/* Library Function - Single Match
    __fileno
   
   Library: Visual Studio 2005 Release */

int __cdecl __fileno(FILE *_File)

{
  DWORD *pDVar1;
  
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  return _File->_file;
}


// 10013d1c __freebuf

/* Library Function - Single Match
    __freebuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __freebuf(FILE *_File)

{
  if (((_File->_flag & 0x83U) != 0) && ((_File->_flag & 8U) != 0)) {
    FUN_10005520(_File->_base);
    _File->_flag = _File->_flag & 0xfffffbf7;
    _File->_ptr = (char *)0x0;
    _File->_base = (char *)0x0;
    _File->_cnt = 0;
  }
  return;
}


// 10013d48 __flush

/* Library Function - Single Match
    __flush
   
   Library: Visual Studio 2005 Release */

int __cdecl __flush(FILE *_File)

{
  int _FileHandle;
  uint uVar1;
  int iVar2;
  uint uVar3;
  char *_Buf;
  
  iVar2 = 0;
  if ((((byte)_File->_flag & 3) == 2) && ((_File->_flag & 0x108U) != 0)) {
    _Buf = _File->_base;
    uVar3 = (int)_File->_ptr - (int)_Buf;
    if (0 < (int)uVar3) {
      uVar1 = uVar3;
      _FileHandle = __fileno(_File);
      uVar1 = __write(_FileHandle,_Buf,uVar1);
      if (uVar1 == uVar3) {
        if ((char)_File->_flag < '\0') {
          _File->_flag = _File->_flag & 0xfffffffd;
        }
      }
      else {
        _File->_flag = _File->_flag | 0x20;
        iVar2 = -1;
      }
    }
  }
  _File->_cnt = 0;
  _File->_ptr = _File->_base;
  return iVar2;
}


// 10013daa __fflush_nolock

/* Library Function - Single Match
    __fflush_nolock
   
   Library: Visual Studio 2005 Release */

int __cdecl __fflush_nolock(FILE *_File)

{
  int iVar1;
  
  if (_File == (FILE *)0x0) {
    iVar1 = flsall(0);
    return iVar1;
  }
  iVar1 = __flush(_File);
  if (iVar1 != 0) {
    return -1;
  }
  if ((_File->_flag & 0x4000) != 0) {
    iVar1 = __fileno(_File);
    iVar1 = __commit(iVar1);
    return -(uint)(iVar1 != 0);
  }
  return 0;
}


// 10013dec flsall

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _flsall
   
   Library: Visual Studio 2005 Release */

int __cdecl flsall(int param_1)

{
  int *piVar1;
  FILE *_File;
  int iVar2;
  int iVar3;
  int local_28;
  int local_20;
  
  local_20 = 0;
  local_28 = 0;
  __lock(1);
  for (iVar3 = 0; iVar3 < DAT_1002ac60; iVar3 = iVar3 + 1) {
    piVar1 = (int *)(DAT_10029c50 + iVar3 * 4);
    if ((*piVar1 != 0) && (iVar2 = *piVar1, (*(byte *)(iVar2 + 0xc) & 0x83) != 0)) {
      FUN_1000f8c8(iVar3,iVar2);
      _File = *(FILE **)(DAT_10029c50 + iVar3 * 4);
      if ((_File->_flag & 0x83U) != 0) {
        if (param_1 == 1) {
          iVar2 = __fflush_nolock(_File);
          if (iVar2 != -1) {
            local_20 = local_20 + 1;
          }
        }
        else if ((param_1 == 0) && ((_File->_flag & 2U) != 0)) {
          iVar2 = __fflush_nolock(_File);
          if (iVar2 == -1) {
            local_28 = -1;
          }
        }
      }
      FUN_10013e8e();
    }
  }
  FUN_10013ebd();
  if (param_1 != 1) {
    local_20 = local_28;
  }
  return local_20;
}


// 10013e8e FUN_10013e8e

void FUN_10013e8e(void)

{
  int unaff_ESI;
  
  FUN_1000f92c(unaff_ESI,*(int *)(DAT_10029c50 + unaff_ESI * 4));
  return;
}


// 10013ebd FUN_10013ebd

void FUN_10013ebd(void)

{
  FUN_10012689(1);
  return;
}


// 10013ecf __filbuf

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
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        iVar4 = FUN_100145b0(uVar3,pWVar6,pWVar7);
        _File->_cnt = iVar4;
        if ((iVar4 != 0) && (iVar4 != -1)) {
          if ((_File->_flag & 0x82) == 0) {
            iVar4 = __fileno(_File);
            if ((iVar4 == -1) || (iVar4 = __fileno(_File), iVar4 == -2)) {
              puVar5 = &DAT_10025008;
            }
            else {
              iVar4 = __fileno(_File);
              uVar3 = __fileno(_File);
              puVar5 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_10029b40)[iVar4 >> 5]);
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


// 10013fef FUN_10013fef

int __cdecl FUN_10013fef(uint param_1,LPWSTR param_2,LPWSTR param_3)

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
    pDVar5 = FUN_100125ba();
    *pDVar5 = 0;
    pDVar5 = FUN_100125a7();
    *pDVar5 = 9;
    return -1;
  }
  if (((int)param_1 < 0) || (DAT_10029b38 <= param_1)) {
    pDVar5 = FUN_100125ba();
    *pDVar5 = 0;
    pDVar5 = FUN_100125a7();
    *pDVar5 = 9;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  iVar12 = (param_1 & 0x1f) * 0x38;
  piVar1 = &DAT_10029b40 + ((int)param_1 >> 5);
  bVar3 = *(byte *)(*piVar1 + iVar12 + 4);
  if ((bVar3 & 1) == 0) {
    pDVar5 = FUN_100125ba();
    *pDVar5 = 0;
    pDVar5 = FUN_100125a7();
    *pDVar5 = 9;
    goto LAB_100140f7;
  }
  if (param_3 < (LPWSTR)0x80000000) {
    local_14 = (LPWSTR)0x0;
    if ((param_3 == (LPWSTR)0x0) || ((bVar3 & 2) != 0)) {
      return 0;
    }
    if (param_2 != (LPWSTR)0x0) {
      local_6 = (char)(*(char *)(*piVar1 + iVar12 + 0x24) * '\x02') >> 1;
      if (local_6 == '\x01') {
        if ((~(uint)param_3 & 1) == 0) goto LAB_100140e5;
        pWVar9 = (LPWSTR)((uint)param_3 >> 1);
        param_3 = (LPWSTR)0x4;
        if ((LPWSTR)0x3 < pWVar9) {
          param_3 = pWVar9;
        }
        local_10 = (LPWSTR)FUN_10005720(param_3);
        if (local_10 == (LPWSTR)0x0) {
          pDVar5 = FUN_100125a7();
          *pDVar5 = 0xc;
          pDVar5 = FUN_100125ba();
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
          if ((~(uint)param_3 & 1) == 0) goto LAB_100140e5;
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
            goto LAB_10014404;
          }
          goto LAB_100143f9;
        }
        pDVar5 = FUN_100125a7();
        *pDVar5 = 9;
        pDVar5 = FUN_100125ba();
        *pDVar5 = 5;
      }
      else {
        local_14 = (LPWSTR)((int)local_14 + (int)local_1c);
        pbVar2 = (byte *)(iVar12 + 4 + *piVar1);
        if ((*pbVar2 & 0x80) == 0) goto LAB_10014404;
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
                    goto LAB_100144a7;
                  }
LAB_1001453c:
                  param_3 = param_3 + 1;
                  *pWVar11 = L'\r';
                }
                else {
                  pWVar9 = param_3 + 1;
                  BVar7 = ReadFile(*(HANDLE *)(iVar12 + *piVar1),&local_c,2,(LPDWORD)&local_1c,
                                   (LPOVERLAPPED)0x0);
                  if (((BVar7 == 0) && (DVar8 = GetLastError(), DVar8 != 0)) ||
                     (local_1c == (LPWSTR)0x0)) goto LAB_1001453c;
                  if ((*(byte *)(iVar12 + 4 + *piVar1) & 0x48) == 0) {
                    if ((pWVar11 == local_10) && (local_c == 10)) goto LAB_100144a7;
                    __lseeki64_nolock(param_1,0x1ffffffff,unaff_EDI);
                    if (local_c == 10) goto LAB_10014543;
                    goto LAB_1001453c;
                  }
                  if (local_c == 10) {
LAB_100144a7:
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
LAB_10014543:
              param_3 = pWVar9;
            } while (param_3 < local_14);
          }
          local_14 = (LPWSTR)((int)pWVar11 - (int)local_10);
          goto LAB_10014404;
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
                  goto LAB_10014284;
                }
LAB_100142fb:
                param_3 = (LPWSTR)((int)param_3 + 1);
                *(byte *)pWVar9 = 0xd;
              }
              else {
                pWVar6 = (LPWSTR)((int)param_3 + 1);
                BVar7 = ReadFile(*(HANDLE *)(iVar12 + *piVar1),&local_5,1,(LPDWORD)&local_1c,
                                 (LPOVERLAPPED)0x0);
                if (((BVar7 == 0) && (DVar8 = GetLastError(), DVar8 != 0)) ||
                   (local_1c == (LPWSTR)0x0)) goto LAB_100142fb;
                if ((*(byte *)(iVar12 + 4 + *piVar1) & 0x48) == 0) {
                  if ((pWVar9 == local_10) && (local_5 == '\n')) goto LAB_10014284;
                  __lseeki64_nolock(param_1,0x1ffffffff,unaff_EDI);
                  if (local_5 == '\n') goto LAB_100142ff;
                  goto LAB_100142fb;
                }
                if (local_5 == '\n') {
LAB_10014284:
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
LAB_100142ff:
            param_3 = pWVar6;
          } while (param_3 < local_14);
        }
        local_14 = (LPWSTR)((int)pWVar9 - (int)local_10);
        if ((local_6 != '\x01') || (local_14 == (LPWSTR)0x0)) goto LAB_10014404;
        bVar3 = *(byte *)((int)pWVar9 + -1);
        if ((char)bVar3 < '\0') {
          iVar10 = 1;
          pWVar9 = (LPWSTR)((int)pWVar9 + -1);
          while ((((&DAT_10024f08)[bVar3] == '\0' && (iVar10 < 5)) && (local_10 <= pWVar9))) {
            pWVar9 = (LPWSTR)((int)pWVar9 + -1);
            bVar3 = *(byte *)pWVar9;
            iVar10 = iVar10 + 1;
          }
          if ((char)(&DAT_10024f08)[(byte)*pWVar9] == 0) {
            pDVar5 = FUN_100125a7();
            *pDVar5 = 0x2a;
            goto LAB_10014400;
          }
          if ((char)(&DAT_10024f08)[(byte)*pWVar9] + 1 == iVar10) {
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
          goto LAB_10014404;
        }
        DVar8 = GetLastError();
LAB_100143f9:
        FUN_100125cd(DVar8);
      }
LAB_10014400:
      local_18 = -1;
LAB_10014404:
      if (local_10 != param_2) {
        FUN_10005520(local_10);
      }
      if (local_18 == -2) {
        return (int)local_14;
      }
      return local_18;
    }
  }
LAB_100140e5:
  pDVar5 = FUN_100125ba();
  *pDVar5 = 0;
  pDVar5 = FUN_100125a7();
  *pDVar5 = 0x16;
LAB_100140f7:
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 100145b0 FUN_100145b0

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

int __cdecl FUN_100145b0(uint param_1,LPWSTR param_2,LPWSTR param_3)

{
  DWORD *pDVar1;
  int iVar2;
  int local_20;
  
  if (param_1 == 0xfffffffe) {
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
    return -1;
  }
  if ((-1 < (int)param_1) && (param_1 < DAT_10029b38)) {
    iVar2 = (param_1 & 0x1f) * 0x38;
    if ((*(byte *)((&DAT_10029b40)[(int)param_1 >> 5] + 4 + iVar2) & 1) != 0) {
      if (param_3 < (LPWSTR)0x80000000) {
        ___lock_fhandle(param_1);
        if ((*(byte *)((&DAT_10029b40)[(int)param_1 >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_100125a7();
          *pDVar1 = 9;
          pDVar1 = FUN_100125ba();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = FUN_10013fef(param_1,param_2,param_3);
        }
        FUN_100146a3();
        return local_20;
      }
      pDVar1 = FUN_100125ba();
      *pDVar1 = 0;
      pDVar1 = FUN_100125a7();
      *pDVar1 = 0x16;
      goto LAB_100145ff;
    }
  }
  pDVar1 = FUN_100125ba();
  *pDVar1 = 0;
  pDVar1 = FUN_100125a7();
  *pDVar1 = 9;
LAB_100145ff:
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 100146a3 FUN_100146a3

void FUN_100146a3(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 100146ad _memcpy_s

/* Library Function - Single Match
    _memcpy_s
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

errno_t __cdecl _memcpy_s(void *_Dst,rsize_t _DstSize,void *_Src,rsize_t _MaxCount)

{
  errno_t eVar1;
  DWORD *pDVar2;
  
  if (_MaxCount == 0) {
LAB_100146bb:
    eVar1 = 0;
  }
  else {
    if (_Dst == (void *)0x0) {
LAB_100146c4:
      pDVar2 = FUN_100125a7();
      eVar1 = 0x16;
      *pDVar2 = 0x16;
    }
    else {
      if ((_Src != (void *)0x0) && (_MaxCount <= _DstSize)) {
        _memcpy(_Dst,_Src,_MaxCount);
        goto LAB_100146bb;
      }
      _memset(_Dst,0,_DstSize);
      if (_Src == (void *)0x0) goto LAB_100146c4;
      if (_MaxCount <= _DstSize) {
        return 0x16;
      }
      pDVar2 = FUN_100125a7();
      eVar1 = 0x22;
      *pDVar2 = 0x22;
    }
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return eVar1;
}


// 10014728 __ioinit

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
  
  uStack_c = 0x10014734;
  local_8 = 0;
  GetStartupInfoA(&local_68);
  local_8 = 0xfffffffe;
  puVar2 = (undefined4 *)FUN_10005780(0x20,0x38);
  if (puVar2 == (undefined4 *)0x0) {
LAB_1001495f:
    iVar7 = -1;
  }
  else {
    DAT_10029b38 = 0x20;
    DAT_10029b40 = puVar2;
    for (; puVar2 < DAT_10029b40 + 0x1c0; puVar2 = puVar2 + 0xe) {
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
      while ((UVar10 = UVar9, (int)DAT_10029b38 < (int)UVar9 &&
             (puVar2 = (undefined4 *)FUN_10005780(0x20,0x38), UVar10 = DAT_10029b38,
             puVar2 != (undefined4 *)0x0))) {
        (&DAT_10029b40)[iVar7] = puVar2;
        DAT_10029b38 = DAT_10029b38 + 0x20;
        puVar1 = puVar2;
        for (; puVar2 < puVar1 + 0x1c0; puVar2 = puVar2 + 0xe) {
          *(undefined1 *)(puVar2 + 1) = 0;
          *puVar2 = 0xffffffff;
          *(undefined1 *)((int)puVar2 + 5) = 10;
          puVar2[2] = 0;
          *(byte *)(puVar2 + 9) = *(byte *)(puVar2 + 9) & 0x80;
          *(undefined1 *)((int)puVar2 + 0x25) = 10;
          *(undefined1 *)((int)puVar2 + 0x26) = 10;
          puVar1 = (&DAT_10029b40)[iVar7];
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
                     ((local_24 & 0x1f) * 0x38 + (int)(&DAT_10029b40)[(int)local_24 >> 5]);
            *puVar2 = *(undefined4 *)local_20;
            *(byte *)(puVar2 + 1) = (byte)*pUVar6;
            iVar7 = ___crtInitCritSecAndSpinCount(puVar2 + 3,4000);
            if (iVar7 == 0) goto LAB_1001495f;
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
      piVar8 = DAT_10029b40 + iVar7 * 0xe;
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
          if (iVar5 == 0) goto LAB_1001495f;
          piVar8[2] = piVar8[2] + 1;
        }
      }
      else {
        *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 0x80;
      }
      iVar7 = iVar7 + 1;
    } while (iVar7 < 3);
    SetHandleCount(DAT_10029b38);
    iVar7 = 0;
  }
  return iVar7;
}


// 10014968 __ioterm

/* Library Function - Single Match
    __ioterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __ioterm(void)

{
  uint uVar1;
  uint *puVar2;
  uint uVar3;
  
  puVar2 = &DAT_10029b40;
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
      FUN_10005520(*puVar2);
      *puVar2 = 0;
    }
    puVar2 = puVar2 + 1;
  } while ((int)puVar2 < 0x10029c40);
  return;
}


// 100149b4 __lseek_nolock

/* Library Function - Single Match
    __lseek_nolock
   
   Library: Visual Studio 2005 Release */

long __cdecl __lseek_nolock(int _FileHandle,long _Offset,int _Origin)

{
  byte *pbVar1;
  HANDLE hFile;
  DWORD *pDVar2;
  DWORD DVar3;
  DWORD DVar4;
  
  hFile = (HANDLE)__get_osfhandle(_FileHandle);
  if (hFile == (HANDLE)0xffffffff) {
    pDVar2 = FUN_100125a7();
    *pDVar2 = 9;
    return -1;
  }
  DVar3 = SetFilePointer(hFile,_Offset,(PLONG)0x0,_Origin);
  if (DVar3 == 0xffffffff) {
    DVar4 = GetLastError();
  }
  else {
    DVar4 = 0;
  }
  if (DVar4 == 0) {
    pbVar1 = (byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38);
    *pbVar1 = *pbVar1 & 0xfd;
  }
  else {
    FUN_100125cd(DVar4);
    DVar3 = 0xffffffff;
  }
  return DVar3;
}


// 10014a26 __lseek

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __lseek
   
   Library: Visual Studio 2005 Release */

long __cdecl __lseek(int _FileHandle,long _Offset,int _Origin)

{
  DWORD *pDVar1;
  int iVar2;
  long local_20;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10029b38)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_100125a7();
          *pDVar1 = 9;
          pDVar1 = FUN_100125ba();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = __lseek_nolock(_FileHandle,_Offset,_Origin);
        }
        FUN_10014af8();
        return local_20;
      }
    }
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 10014af8 FUN_10014af8

void FUN_10014af8(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 10014b02 FUN_10014b02

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 * __cdecl FUN_10014b02(char *param_1,char *param_2,int param_3,undefined4 *param_4)

{
  char cVar1;
  bool bVar2;
  uchar uVar3;
  DWORD *pDVar4;
  int iVar5;
  errno_t eVar6;
  char *pcVar7;
  uchar *_Str2;
  uchar *_Str1;
  int local_14;
  int local_10;
  int local_c;
  uint local_8;
  
  local_8 = DAT_100296e8;
  local_10 = 0;
  local_c = 0;
  local_14 = 0;
  for (pcVar7 = param_2; *pcVar7 == ' '; pcVar7 = pcVar7 + 1) {
  }
  cVar1 = *pcVar7;
  if (cVar1 == 'a') {
    param_2 = (char *)0x109;
LAB_10014b6e:
    local_8 = DAT_100296e8 | 2;
  }
  else {
    if (cVar1 != 'r') {
      if (cVar1 != 'w') {
        pDVar4 = FUN_100125a7();
        *pDVar4 = 0x16;
        FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return (undefined4 *)0x0;
      }
      param_2 = (char *)0x301;
      goto LAB_10014b6e;
    }
    local_8 = DAT_100296e8 | 1;
    param_2 = (char *)0x0;
  }
  bVar2 = true;
  _Str2 = (uchar *)(pcVar7 + 1);
  uVar3 = *_Str2;
  if (uVar3 != '\0') {
    do {
      if (!bVar2) break;
      if ((char)uVar3 < 'T') {
        if (uVar3 == 'S') {
          if (local_c != 0) goto LAB_10014ca6;
          param_2 = (char *)((uint)param_2 | 0x20);
          local_c = 1;
        }
        else if (uVar3 != ' ') {
          if (uVar3 == '+') {
            if (((uint)param_2 & 2) != 0) goto LAB_10014ca6;
            param_2 = (char *)((uint)param_2 & 0xfffffffe | 2);
            local_8 = local_8 & 0xfffffffc | 0x80;
          }
          else if (uVar3 == ',') {
            local_14 = 1;
LAB_10014ca6:
            bVar2 = false;
          }
          else if (uVar3 == 'D') {
            if (((uint)param_2 & 0x40) != 0) goto LAB_10014ca6;
            param_2 = (char *)((uint)param_2 | 0x40);
          }
          else if (uVar3 == 'N') {
            param_2 = (char *)((uint)param_2 | 0x80);
          }
          else {
            if (uVar3 != 'R') goto LAB_10014d41;
            if (local_c != 0) goto LAB_10014ca6;
            param_2 = (char *)((uint)param_2 | 0x10);
            local_c = 1;
          }
        }
      }
      else if (uVar3 == 'T') {
        if (((uint)param_2 & 0x1000) != 0) goto LAB_10014ca6;
        param_2 = (char *)((uint)param_2 | 0x1000);
      }
      else if (uVar3 == 'b') {
        if (((uint)param_2 & 0xc000) != 0) goto LAB_10014ca6;
        param_2 = (char *)((uint)param_2 | 0x8000);
      }
      else if (uVar3 == 'c') {
        if (local_10 != 0) goto LAB_10014ca6;
        local_8 = local_8 | 0x4000;
        local_10 = 1;
      }
      else if (uVar3 == 'n') {
        if (local_10 != 0) goto LAB_10014ca6;
        local_8 = local_8 & 0xffffbfff;
        local_10 = 1;
      }
      else {
        if (uVar3 != 't') goto LAB_10014d41;
        if (((uint)param_2 & 0xc000) != 0) goto LAB_10014ca6;
        param_2 = (char *)((uint)param_2 | 0x4000);
      }
      _Str2 = _Str2 + 1;
      uVar3 = *_Str2;
    } while (uVar3 != '\0');
    if (local_14 != 0) {
      for (; *_Str2 == ' '; _Str2 = _Str2 + 1) {
      }
      iVar5 = __mbsnbcmp("ccs=",_Str2,4);
      if (iVar5 != 0) goto LAB_10014d41;
      _Str1 = _Str2 + 4;
      iVar5 = __mbsicmp(_Str1,(uchar *)"UTF-8");
      if (iVar5 == 0) {
        _Str2 = _Str2 + 9;
        param_2 = (char *)((uint)param_2 | 0x40000);
      }
      else {
        iVar5 = __mbsicmp(_Str1,(uchar *)"UTF-16LE");
        if (iVar5 == 0) {
          _Str2 = _Str2 + 0xc;
          param_2 = (char *)((uint)param_2 | 0x20000);
        }
        else {
          iVar5 = __mbsicmp(_Str1,(uchar *)"UNICODE");
          if (iVar5 != 0) goto LAB_10014d41;
          _Str2 = _Str2 + 0xb;
          param_2 = (char *)((uint)param_2 | 0x10000);
        }
      }
    }
  }
  for (; *_Str2 == ' '; _Str2 = _Str2 + 1) {
  }
  if (*_Str2 == '\0') {
    eVar6 = FID_conflict___sopen_s(&local_14,param_1,(int)param_2,param_3,0x180);
    if (eVar6 != 0) {
      return (undefined4 *)0x0;
    }
    _DAT_100290b8 = _DAT_100290b8 + 1;
    param_4[3] = local_8;
    param_4[1] = 0;
    *param_4 = 0;
    param_4[2] = 0;
    param_4[7] = 0;
    param_4[4] = local_14;
    return param_4;
  }
LAB_10014d41:
  pDVar4 = FUN_100125a7();
  *pDVar4 = 0x16;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return (undefined4 *)0x0;
}


// 10014da2 FUN_10014da2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

undefined4 * FUN_10014da2(void)

{
  int *piVar1;
  uint uVar2;
  int iVar3;
  undefined4 uVar4;
  int iVar5;
  undefined4 *puVar6;
  undefined4 *puVar7;
  
  puVar6 = (undefined4 *)0x0;
  __lock(1);
  iVar5 = 0;
  do {
    puVar7 = puVar6;
    if (DAT_1002ac60 <= iVar5) {
LAB_10014e98:
      if (puVar7 != (undefined4 *)0x0) {
        puVar7[3] = puVar7[3] & 0x8000;
        puVar7[1] = 0;
        puVar7[2] = 0;
        *puVar7 = 0;
        puVar7[7] = 0;
        puVar7[4] = 0xffffffff;
      }
      FUN_10014ec9();
      return puVar7;
    }
    piVar1 = (int *)(DAT_10029c50 + iVar5 * 4);
    if (*piVar1 == 0) {
      iVar5 = iVar5 * 4;
      uVar4 = FUN_10005720(0x38);
      *(undefined4 *)(iVar5 + DAT_10029c50) = uVar4;
      if (*(int *)(DAT_10029c50 + iVar5) != 0) {
        iVar3 = ___crtInitCritSecAndSpinCount(*(int *)(DAT_10029c50 + iVar5) + 0x20,4000);
        if (iVar3 == 0) {
          FUN_10005520(*(undefined4 *)(iVar5 + DAT_10029c50));
          *(undefined4 *)(iVar5 + DAT_10029c50) = 0;
        }
        else {
          EnterCriticalSection((LPCRITICAL_SECTION)(*(int *)(iVar5 + DAT_10029c50) + 0x20));
          puVar7 = *(undefined4 **)(iVar5 + DAT_10029c50);
        }
      }
      goto LAB_10014e98;
    }
    uVar2 = *(uint *)(*piVar1 + 0xc);
    if (((uVar2 & 0x83) == 0) && (-1 < (short)uVar2)) {
      if ((iVar5 - 3U < 0x11) && (iVar3 = __mtinitlocknum(iVar5 + 0x10), iVar3 == 0))
      goto LAB_10014e98;
      FUN_1000f8c8(iVar5,*(int *)(DAT_10029c50 + iVar5 * 4));
      puVar7 = *(undefined4 **)(DAT_10029c50 + iVar5 * 4);
      if ((*(byte *)(puVar7 + 3) & 0x83) == 0) goto LAB_10014e98;
      FUN_1000f92c(iVar5,(int)puVar7);
    }
    iVar5 = iVar5 + 1;
  } while( true );
}


// 10014ec9 FUN_10014ec9

void FUN_10014ec9(void)

{
  FUN_10012689(1);
  return;
}


// 10014ed4 __local_unwind4

/* Library Function - Single Match
    __local_unwind4
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

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
  puStack_24 = &LAB_10014f64;
  uStack_28 = *unaff_FS_OFFSET;
  local_20 = DAT_10025040 ^ (uint)&uStack_28;
  *unaff_FS_OFFSET = &uStack_28;
  while( true ) {
    uVar2 = *(uint *)(param_2 + 0xc);
    if ((uVar2 == 0xfffffffe) || ((param_3 != 0xfffffffe && (uVar2 <= param_3)))) break;
    puVar1 = (undefined4 *)((*(uint *)(param_2 + 8) ^ *param_1) + 0x10 + uVar2 * 0xc);
    *(undefined4 *)(param_2 + 0xc) = *puVar1;
    if (puVar1[1] == 0) {
      __NLG_Notify(0x101);
      FUN_1001b48c();
    }
  }
  *unaff_FS_OFFSET = uStack_28;
  return;
}


// 10014faa FUN_10014faa

void FUN_10014faa(int param_1)

{
  __local_unwind4(*(uint **)(param_1 + 0x28),*(int *)(param_1 + 0x18),*(uint *)(param_1 + 0x1c));
  return;
}


// 10014fc6 _EH4_CallFilterFunc

/* Library Function - Single Match
    @_EH4_CallFilterFunc@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_CallFilterFunc,8 */

void __fastcall _EH4_CallFilterFunc(undefined *param_1)

{
  (*(code *)param_1)();
  return;
}


// 10014fdd _EH4_TransferToHandler

/* Library Function - Single Match
    @_EH4_TransferToHandler@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_TransferToHandler,8 */

void __fastcall _EH4_TransferToHandler(undefined *UNRECOVERED_JUMPTABLE)

{
  __NLG_Notify(1);
                    /* WARNING: Could not recover jumptable at 0x10014ff4. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (*(code *)UNRECOVERED_JUMPTABLE)();
  return;
}


// 10014ff6 _EH4_GlobalUnwind

/* Library Function - Single Match
    @_EH4_GlobalUnwind@4
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_GlobalUnwind,4 */

void __fastcall _EH4_GlobalUnwind(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x1001500b,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 10015010 _EH4_LocalUnwind

/* Library Function - Single Match
    @_EH4_LocalUnwind@16
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_LocalUnwind,16 */

void __fastcall _EH4_LocalUnwind(int param_1,uint param_2,undefined4 param_3,uint *param_4)

{
  __local_unwind4(param_4,param_1,param_2);
  return;
}


// 10015027 __mbctoupper_l

/* Library Function - Single Match
    __mbctoupper_l
   
   Library: Visual Studio 2005 Release */

uint __cdecl __mbctoupper_l(uint _Ch,_locale_t _Locale)

{
  int iVar1;
  localeinfo_struct local_1c;
  int local_14;
  char local_10;
  CHAR local_c;
  undefined1 local_b;
  CHAR local_8;
  undefined1 local_7;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_1c,_Locale);
  if (_Ch < 0x100) {
    if (((local_1c.mbcinfo)->mbctype[_Ch + 5] & 0x20) != 0) {
      _Ch = (uint)(local_1c.mbcinfo)->mbcasemap[_Ch + 4];
    }
  }
  else {
    local_8 = (CHAR)(_Ch >> 8);
    local_7 = (undefined1)_Ch;
    if ((((local_1c.mbcinfo)->mbctype[(_Ch >> 8 & 0xff) + 5] & 4) == 0) ||
       (iVar1 = ___crtLCMapStringA(&local_1c,*(LPCWSTR *)(local_1c.mbcinfo)->mbulinfo,0x200,&local_8
                                   ,2,&local_c,2,(local_1c.mbcinfo)->mbcodepage,1), iVar1 == 0)) {
      if (local_10 == '\0') {
        return _Ch;
      }
      *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
      return _Ch;
    }
    _Ch = (uint)CONCAT11(local_c,local_b);
  }
  if (local_10 != '\0') {
    *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
  }
  return _Ch;
}


// 100150cb __mbctoupper

/* Library Function - Single Match
    __mbctoupper
   
   Library: Visual Studio 2005 Release */

uint __cdecl __mbctoupper(uint _Ch)

{
  uint uVar1;
  
  uVar1 = __mbctoupper_l(_Ch,(_locale_t)0x0);
  return uVar1;
}


// 100150d9 __security_check_cookie

/* Library Function - Single Match
    @__security_check_cookie@4
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release
   __fastcall __security_check_cookie,4 */

void __fastcall __security_check_cookie(int param_1)

{
  if (param_1 == DAT_10025040) {
    return;
  }
                    /* WARNING: Subroutine does not return */
  ___report_gsfailure();
}


// 100150e8 FUN_100150e8

undefined4 FUN_100150e8(void)

{
  int iVar1;
  HMODULE pHVar2;
  int iVar3;
  uint uVar4;
  char *_Str2;
  int local_8;
  
  uVar4 = 0;
  local_8 = 0;
  FUN_1000f1fd(&local_8);
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


// 10015154 FUN_10015154

int __cdecl FUN_10015154(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_1002504c);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10025048 != -1)) {
    iVar3 = DAT_10025048;
    pcVar2 = (code *)TlsGetValue(DAT_1002504c);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1f8);
      goto LAB_100151ae;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_100150e8();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"EncodePointer");
LAB_100151ae:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 100151c2 __encoded_null

/* Library Function - Single Match
    __encoded_null
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __encoded_null(void)

{
  FUN_10015154(0);
  return;
}


// 100151cb FUN_100151cb

int __cdecl FUN_100151cb(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_1002504c);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_10025048 != -1)) {
    iVar3 = DAT_10025048;
    pcVar2 = (code *)TlsGetValue(DAT_1002504c);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1fc);
      goto LAB_10015225;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_100150e8();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"DecodePointer");
LAB_10015225:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 10015242 FUN_10015242

LPVOID FUN_10015242(void)

{
  LPVOID lpTlsValue;
  
  lpTlsValue = TlsGetValue(DAT_1002504c);
  if (lpTlsValue == (LPVOID)0x0) {
    lpTlsValue = (LPVOID)FUN_100151cb(DAT_1002922c);
    TlsSetValue(DAT_1002504c,lpTlsValue);
  }
  return lpTlsValue;
}


// 10015274 __mtterm

/* Library Function - Single Match
    __mtterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __mtterm(void)

{
  code *pcVar1;
  int iVar2;
  
  if (DAT_10025048 != -1) {
    iVar2 = DAT_10025048;
    pcVar1 = (code *)FUN_100151cb(DAT_10029234);
    (*pcVar1)(iVar2);
    DAT_10025048 = -1;
  }
  if (DAT_1002504c != 0xffffffff) {
    TlsFree(DAT_1002504c);
    DAT_1002504c = 0xffffffff;
  }
  __mtdeletelocks();
  return;
}


// 100152b1 FUN_100152b1

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

void __cdecl FUN_100152b1(int param_1,int param_2)

{
  HMODULE hModule;
  int iVar1;
  FARPROC pFVar2;
  
  hModule = GetModuleHandleA("KERNEL32.DLL");
  *(undefined **)(param_1 + 0x5c) = &DAT_10025108;
  *(undefined4 *)(param_1 + 0x14) = 1;
  if (hModule != (HMODULE)0x0) {
    iVar1 = FUN_100150e8();
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
  *(undefined **)(param_1 + 0x68) = &DAT_100251a8;
  InterlockedIncrement((LONG *)&DAT_100251a8);
  __lock(0xc);
  *(int *)(param_1 + 0x6c) = param_2;
  if (param_2 == 0) {
    *(undefined **)(param_1 + 0x6c) = PTR_DAT_100257b0;
  }
  ___addlocaleref(*(LONG **)(param_1 + 0x6c));
  FUN_10015367();
  return;
}


// 10015367 FUN_10015367

void FUN_10015367(void)

{
  FUN_10012689(0xc);
  return;
}


// 10015370 FUN_10015370

DWORD * FUN_10015370(void)

{
  DWORD dwErrCode;
  code *pcVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD DVar4;
  undefined4 uVar5;
  DWORD *pDVar6;
  
  dwErrCode = GetLastError();
  uVar5 = DAT_10025048;
  pcVar1 = (code *)FUN_10015242();
  pDVar2 = (DWORD *)(*pcVar1)(uVar5);
  if (pDVar2 == (DWORD *)0x0) {
    pDVar2 = (DWORD *)FUN_10005780(1,0x214);
    if (pDVar2 != (DWORD *)0x0) {
      uVar5 = DAT_10025048;
      pDVar6 = pDVar2;
      pcVar1 = (code *)FUN_100151cb(DAT_10029230);
      iVar3 = (*pcVar1)(uVar5,pDVar6);
      if (iVar3 == 0) {
        FUN_10005520(pDVar2);
        pDVar2 = (DWORD *)0x0;
      }
      else {
        FUN_100152b1((int)pDVar2,0);
        DVar4 = GetCurrentThreadId();
        pDVar2[1] = 0xffffffff;
        *pDVar2 = DVar4;
      }
    }
  }
  SetLastError(dwErrCode);
  return pDVar2;
}


// 100153e7 __getptd

/* Library Function - Single Match
    __getptd
   
   Library: Visual Studio 2005 Release */

_ptiddata __cdecl __getptd(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = (_ptiddata)FUN_10015370();
  if (p_Var1 == (_ptiddata)0x0) {
    __amsg_exit(0x10);
  }
  return p_Var1;
}


// 100153ff __freefls@4

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
      FUN_10005520(*(int *)(param_1 + 0x24));
    }
    if (*(int *)(param_1 + 0x2c) != 0) {
      FUN_10005520(*(int *)(param_1 + 0x2c));
    }
    if (*(int *)(param_1 + 0x34) != 0) {
      FUN_10005520(*(int *)(param_1 + 0x34));
    }
    if (*(int *)(param_1 + 0x3c) != 0) {
      FUN_10005520(*(int *)(param_1 + 0x3c));
    }
    if (*(int *)(param_1 + 0x44) != 0) {
      FUN_10005520(*(int *)(param_1 + 0x44));
    }
    if (*(int *)(param_1 + 0x48) != 0) {
      FUN_10005520(*(int *)(param_1 + 0x48));
    }
    if (*(undefined **)(param_1 + 0x5c) != &DAT_10025108) {
      FUN_10005520(*(undefined **)(param_1 + 0x5c));
    }
    __lock(0xd);
    pLVar1 = *(LONG **)(param_1 + 0x68);
    if (pLVar1 != (LONG *)0x0) {
      LVar2 = InterlockedDecrement(pLVar1);
      if ((LVar2 == 0) && (pLVar1 != (LONG *)&DAT_100251a8)) {
        FUN_10005520(pLVar1);
      }
    }
    FUN_1001550b();
    __lock(0xc);
    pLVar1 = *(LONG **)(param_1 + 0x6c);
    if (pLVar1 != (LONG *)0x0) {
      ___removelocaleref(pLVar1);
      if (((pLVar1 != (LONG *)PTR_DAT_100257b0) && (pLVar1 != (LONG *)&DAT_100256d8)) &&
         (*pLVar1 == 0)) {
        ___freetlocinfo((int)pLVar1);
      }
    }
    FUN_10015517();
    FUN_10005520(param_1);
  }
  return;
}


// 1001550b FUN_1001550b

void FUN_1001550b(void)

{
  FUN_10012689(0xd);
  return;
}


// 10015517 FUN_10015517

void FUN_10015517(void)

{
  FUN_10012689(0xc);
  return;
}


// 10015520 __freeptd

/* Library Function - Single Match
    __freeptd
   
   Library: Visual Studio 2005 Release */

void __cdecl __freeptd(_ptiddata _Ptd)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  undefined4 uVar4;
  
  if (DAT_10025048 != -1) {
    if ((_Ptd == (_ptiddata)0x0) && (pvVar1 = TlsGetValue(DAT_1002504c), pvVar1 != (LPVOID)0x0)) {
      iVar3 = DAT_10025048;
      pcVar2 = (code *)TlsGetValue(DAT_1002504c);
      _Ptd = (_ptiddata)(*pcVar2)(iVar3);
    }
    uVar4 = 0;
    iVar3 = DAT_10025048;
    pcVar2 = (code *)FUN_100151cb(DAT_10029230);
    (*pcVar2)(iVar3,uVar4);
    __freefls_4((int)_Ptd);
  }
  if (DAT_1002504c != 0xffffffff) {
    TlsSetValue(DAT_1002504c,(LPVOID)0x0);
  }
  return;
}


// 10015589 __mtinit

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
  DAT_10029228 = GetProcAddress(hModule,"FlsAlloc");
  DAT_1002922c = GetProcAddress(hModule,"FlsGetValue");
  DAT_10029230 = GetProcAddress(hModule,"FlsSetValue");
  DAT_10029234 = GetProcAddress(hModule,"FlsFree");
  if ((((DAT_10029228 == (FARPROC)0x0) || (DAT_1002922c == (FARPROC)0x0)) ||
      (DAT_10029230 == (FARPROC)0x0)) || (DAT_10029234 == (FARPROC)0x0)) {
    DAT_1002922c = TlsGetValue_exref;
    DAT_10029228 = (FARPROC)&LAB_10015239;
    DAT_10029230 = TlsSetValue_exref;
    DAT_10029234 = TlsFree_exref;
  }
  DAT_1002504c = TlsAlloc();
  if ((DAT_1002504c != 0xffffffff) && (BVar1 = TlsSetValue(DAT_1002504c,DAT_1002922c), BVar1 != 0))
  {
    __init_pointers();
    DAT_10029228 = (FARPROC)FUN_10015154((int)DAT_10029228);
    DAT_1002922c = (FARPROC)FUN_10015154((int)DAT_1002922c);
    DAT_10029230 = (FARPROC)FUN_10015154((int)DAT_10029230);
    DAT_10029234 = (FARPROC)FUN_10015154((int)DAT_10029234);
    iVar2 = __mtinitlocks();
    if (iVar2 != 0) {
      pcVar6 = __freefls_4;
      pcVar3 = (code *)FUN_100151cb((int)DAT_10029228);
      DAT_10025048 = (*pcVar3)(pcVar6);
      if ((DAT_10025048 != -1) && (pDVar4 = (DWORD *)FUN_10005780(1,0x214), pDVar4 != (DWORD *)0x0))
      {
        iVar2 = DAT_10025048;
        pDVar7 = pDVar4;
        pcVar3 = (code *)FUN_100151cb((int)DAT_10029230);
        iVar2 = (*pcVar3)(iVar2,pDVar7);
        if (iVar2 != 0) {
          FUN_100152b1((int)pDVar4,0);
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


// 1001570d FUN_1001570d

void __cdecl FUN_1001570d(int param_1)

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
    if (param_1 == (&DAT_10025050)[uVar6 * 2]) break;
    uVar6 = uVar6 + 1;
  } while (uVar6 < 0x17);
  if (uVar6 < 0x17) {
    iVar2 = __set_error_mode(3);
    if ((iVar2 == 1) || ((iVar2 = __set_error_mode(3), iVar2 == 0 && (DAT_100290cc == 1)))) {
      hFile = GetStdHandle(0xfffffff4);
      if ((hFile != (HANDLE)0x0) && (hFile != (HANDLE)0xffffffff)) {
        lpOverlapped = (LPOVERLAPPED)0x0;
        lpNumberOfBytesWritten = &DStack_4;
        puVar1 = (undefined4 *)(uVar6 * 8 + 0x10025054);
        sVar5 = _strlen((char *)*puVar1);
        WriteFile(hFile,(LPCVOID)*puVar1,sVar5,lpNumberOfBytesWritten,lpOverlapped);
      }
    }
    else if (param_1 != 0xfc) {
      eVar3 = _strcpy_s(&DAT_10029238,0x314,"Runtime Error!\n\nProgram: ");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      DAT_10029355 = 0;
      DVar4 = GetModuleFileNameA((HMODULE)0x0,&DAT_10029251,0x104);
      if ((DVar4 == 0) &&
         (eVar3 = _strcpy_s(&DAT_10029251,0x2fb,"<program name unknown>"), eVar3 != 0)) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      sVar5 = _strlen(&DAT_10029251);
      if (0x3c < sVar5 + 1) {
        sVar5 = _strlen(&DAT_10029251);
        eVar3 = _strncpy_s((char *)(sVar5 + 0x10029216),
                           (int)&DAT_1002954c - (int)(sVar5 + 0x10029216),"...",3);
        if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
      }
      eVar3 = _strcat_s(&DAT_10029238,0x314,"\n\n");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      eVar3 = _strcat_s(&DAT_10029238,0x314,*(char **)(uVar6 * 8 + 0x10025054));
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      FUN_1001b593(&DAT_10029238,"Microsoft Visual C++ Runtime Library",0x12010);
    }
  }
  return;
}


// 100158ad __FF_MSGBANNER

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
    if (DAT_100290cc != 1) {
      return;
    }
  }
  FUN_1001570d(0xfc);
  FUN_1001570d(0xff);
  return;
}


// 100158e6 __RTC_Initialize

/* WARNING: Removing unreachable block (ram,0x100158f8) */
/* WARNING: Removing unreachable block (ram,0x100158fe) */
/* WARNING: Removing unreachable block (ram,0x10015900) */
/* Library Function - Single Match
    __RTC_Initialize
   
   Library: Visual Studio 2005 Release */

void __RTC_Initialize(void)

{
  return;
}


// 10015930 __ValidateImageBase

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


// 10015960 __FindPESection

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


// 100159b0 __IsNonwritableInCurrentImage

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
  local_c = DAT_10025040 ^ 0x10022eb0;
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


// 10015aa4 __initp_eh_hooks

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __initp_eh_hooks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __initp_eh_hooks(void)

{
  _DAT_1002954c = FUN_10015154(0x10015a6b);
  return;
}


// 10015ab5 FUN_10015ab5

DWORD * __cdecl FUN_10015ab5(int param_1,DWORD param_2)

{
  int *piVar1;
  code *pcVar2;
  DWORD DVar3;
  DWORD DVar4;
  DWORD *pDVar5;
  int *piVar6;
  int iVar7;
  int iVar8;
  
  pDVar5 = FUN_10015370();
  if (pDVar5 != (DWORD *)0x0) {
    piVar1 = (int *)pDVar5[0x17];
    piVar6 = piVar1;
    do {
      if (*piVar6 == param_1) break;
      piVar6 = piVar6 + 3;
    } while (piVar6 < piVar1 + DAT_1002518c * 3);
    if ((piVar1 + DAT_1002518c * 3 <= piVar6) || (*piVar6 != param_1)) {
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
          if (DAT_10025180 < DAT_10025184 + DAT_10025180) {
            iVar7 = DAT_10025180 * 0xc;
            iVar8 = DAT_10025180;
            do {
              *(undefined4 *)(iVar7 + 8 + pDVar5[0x17]) = 0;
              iVar8 = iVar8 + 1;
              iVar7 = iVar7 + 0xc;
            } while (iVar8 < DAT_10025184 + DAT_10025180);
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


// 10015c2e __initp_misc_winsig

/* Library Function - Single Match
    __initp_misc_winsig
   
   Library: Visual Studio 2005 Release */

void __cdecl __initp_misc_winsig(undefined4 param_1)

{
  DAT_10029550 = param_1;
  DAT_10029554 = param_1;
  DAT_10029558 = param_1;
  DAT_1002955c = param_1;
  return;
}


// 10015c47 siglookup

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
  } while (uVar1 < DAT_1002518c * 0xc + param_1);
  if ((DAT_1002518c * 0xc + param_1 <= uVar1) || (*(int *)(uVar1 + 4) != in_EDX)) {
    uVar1 = 0;
  }
  return uVar1;
}


// 10015c7b ___get_sigabrt

/* Library Function - Single Match
    ___get_sigabrt
   
   Library: Visual Studio 2005 Release */

_PHNDLR __cdecl ___get_sigabrt(void)

{
  _PHNDLR p_Var1;
  
  p_Var1 = (_PHNDLR)FUN_100151cb(DAT_10029558);
  return p_Var1;
}


// 10015c88 _raise

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
        puVar6 = &DAT_10029550;
        iVar3 = DAT_10029550;
        goto LAB_10015d3d;
      }
      if (_SigNum != 4) {
        if (_SigNum == 6) goto LAB_10015d1b;
        if (_SigNum != 8) goto LAB_10015cff;
      }
    }
    pDVar7 = FUN_10015370();
    if (pDVar7 == (DWORD *)0x0) {
      return -1;
    }
    uVar2 = siglookup(pDVar7[0x17]);
    puVar6 = (undefined4 *)(uVar2 + 8);
    pcVar4 = (code *)*puVar6;
  }
  else {
    if (_SigNum == 0xf) {
      puVar6 = &DAT_1002955c;
      iVar3 = DAT_1002955c;
    }
    else if (_SigNum == 0x15) {
      puVar6 = &DAT_10029554;
      iVar3 = DAT_10029554;
    }
    else {
      if (_SigNum != 0x16) {
LAB_10015cff:
        pDVar7 = FUN_100125a7();
        *pDVar7 = 0x16;
        FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return -1;
      }
LAB_10015d1b:
      puVar6 = &DAT_10029558;
      iVar3 = DAT_10029558;
    }
LAB_10015d3d:
    bVar1 = true;
    pcVar4 = (code *)FUN_100151cb(iVar3);
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
      goto LAB_10015da1;
    }
  }
  else {
LAB_10015da1:
    if (_SigNum == 8) {
      for (local_28 = DAT_10025180; local_28 < DAT_10025184 + DAT_10025180; local_28 = local_28 + 1)
      {
        *(undefined4 *)(local_28 * 0xc + 8 + pDVar7[0x17]) = 0;
      }
      goto LAB_10015ddb;
    }
  }
  uVar5 = __encoded_null();
  *puVar6 = uVar5;
LAB_10015ddb:
  FUN_10015dfc();
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


// 10015dfc FUN_10015dfc

void FUN_10015dfc(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + -0x1c) != 0) {
    FUN_10012689(0);
  }
  return;
}


// 10015e38 FUN_10015e38

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10015e38(undefined4 param_1)

{
  _DAT_10029564 = param_1;
  return;
}


// 10015e42 FUN_10015e42

void __cdecl FUN_10015e42(undefined4 param_1)

{
  DAT_10029570 = param_1;
  return;
}


// 10015e4c FUN_10015e4c

void __cdecl FUN_10015e4c(undefined4 param_1)

{
  DAT_10029574 = param_1;
  return;
}


// 10015e56 ___crtInitCritSecNoSpinCount@8

/* Library Function - Single Match
    ___crtInitCritSecNoSpinCount@8
   
   Library: Visual Studio 2005 Release */

undefined4 ___crtInitCritSecNoSpinCount_8(LPCRITICAL_SECTION param_1)

{
  InitializeCriticalSection(param_1);
  return 1;
}


// 10015e66 ___crtInitCritSecAndSpinCount

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
  
  local_8 = &DAT_10022f10;
  uStack_c = 0x10015e72;
  local_20[0] = 0;
  pcVar1 = (code *)FUN_100151cb(DAT_10029574);
  if (pcVar1 != (FARPROC)0x0) goto LAB_10015edc;
  iVar2 = __get_osplatform(local_20);
  if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  if (local_20[0] == 1) {
LAB_10015ecb:
    pcVar1 = ___crtInitCritSecNoSpinCount_8;
  }
  else {
    hModule = GetModuleHandleA("kernel32.dll");
    if (hModule == (HMODULE)0x0) goto LAB_10015ecb;
    pcVar1 = GetProcAddress(hModule,"InitializeCriticalSectionAndSpinCount");
    if (pcVar1 == (FARPROC)0x0) goto LAB_10015ecb;
  }
  DAT_10029574 = FUN_10015154((int)pcVar1);
LAB_10015edc:
  local_8 = (undefined *)0x0;
  iVar2 = (*pcVar1)(param_1,param_2);
  return iVar2;
}


// 10015f2b FUN_10015f2b

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10015f2b(undefined4 param_1)

{
  _DAT_10029578 = param_1;
  return;
}


// 10015f35 type_info::_Type_info_dtor

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
  piVar1 = DAT_10029580;
  if (*(int *)(param_1 + 4) != 0) {
    piVar2 = (int *)&DAT_1002957c;
    do {
      piVar3 = piVar2;
      if (DAT_10029580 == (int *)0x0) goto LAB_10015f79;
      piVar2 = DAT_10029580;
    } while (*DAT_10029580 != *(int *)(param_1 + 4));
    piVar3[1] = DAT_10029580[1];
    FUN_10005520(piVar1);
LAB_10015f79:
    FUN_10005520(*(undefined4 *)(param_1 + 4));
    *(undefined4 *)(param_1 + 4) = 0;
  }
  FUN_10015f9c();
  return;
}


// 10015f9c FUN_10015f9c

void FUN_10015f9c(void)

{
  FUN_10012689(0xe);
  return;
}


// 10015fb0 _strcmp

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
      if (bVar4 != *_Str2) goto LAB_10015ff4;
      _Str2 = _Str2 + 1;
      if (bVar4 == 0) {
        return 0;
      }
      if (((uint)_Str1 & 2) == 0) goto LAB_10015fc0;
    }
    uVar1 = *(undefined2 *)_Str1;
    _Str1 = _Str1 + 2;
    bVar4 = (byte)uVar1;
    bVar5 = bVar4 < (byte)*_Str2;
    if (bVar4 != *_Str2) goto LAB_10015ff4;
    if (bVar4 == 0) {
      return 0;
    }
    bVar4 = (byte)((ushort)uVar1 >> 8);
    bVar5 = bVar4 < ((byte *)_Str2)[1];
    if (bVar4 != ((byte *)_Str2)[1]) goto LAB_10015ff4;
    if (bVar4 == 0) {
      return 0;
    }
    _Str2 = (char *)((byte *)_Str2 + 2);
  }
LAB_10015fc0:
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
LAB_10015ff4:
  return (uint)bVar5 * -2 + 1;
}


// 10016038 _abort

/* Library Function - Single Match
    _abort
   
   Library: Visual Studio 2005 Release */

void __cdecl _abort(void)

{
  _PHNDLR p_Var1;
  EXCEPTION_RECORD local_32c;
  _EXCEPTION_POINTERS local_2dc;
  undefined4 local_2d4;
  
  if (((byte)DAT_100251a0 & 1) != 0) {
    FUN_1001570d(10);
  }
  p_Var1 = ___get_sigabrt();
  if (p_Var1 != (_PHNDLR)0x0) {
    _raise(0x16);
  }
  if (((byte)DAT_100251a0 & 2) != 0) {
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


// 1001612b FUN_1001612b

void __cdecl FUN_1001612b(uint param_1,uint param_2)

{
  DAT_100251a0 = ~param_2 & DAT_100251a0 | param_1 & param_2;
  return;
}


// 10016149 x_ismbbtype_l

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
    if (uVar1 == 0) goto LAB_1001618b;
  }
  uVar1 = 1;
LAB_1001618b:
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 1001619a __ismbblead_l

/* Library Function - Single Match
    __ismbblead_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __ismbblead_l(uint _C,_locale_t _Locale)

{
  int iVar1;
  
  iVar1 = x_ismbbtype_l(_Locale,_C,0,4);
  return iVar1;
}


// 100161af __ismbblead

/* Library Function - Single Match
    __ismbblead
   
   Library: Visual Studio 2005 Release */

int __cdecl __ismbblead(uint _C)

{
  int iVar1;
  
  iVar1 = x_ismbbtype_l((localeinfo_struct *)0x0,_C,0,4);
  return iVar1;
}


// 100161c2 __mbsnbcpy_s

/* Library Function - Single Match
    __mbsnbcpy_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __mbsnbcpy_s(uchar *_Dst,size_t _SizeInBytes,uchar *_Src,size_t _MaxCount)

{
  errno_t eVar1;
  
  eVar1 = __mbsnbcpy_s_l(_Dst,_SizeInBytes,_Src,_MaxCount,(_locale_t)0x0);
  return eVar1;
}


// 1001627c FUN_1001627c

undefined4 FUN_1001627c(void)

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


// 100162ab setSBCS

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
    *puVar1 = puVar1[(int)&DAT_100251a8 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  puVar1 = (undefined1 *)(in_EAX + 0x11d);
  iVar2 = 0x100;
  do {
    *puVar1 = puVar1[(int)&DAT_100251a8 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  return;
}


// 10016300 setSBUpLow

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
  
  local_8 = DAT_10025040 ^ (uint)local_4a0;
  BVar3 = GetCPInfo(*(UINT *)(unaff_ESI + 4),&local_51c);
  if (BVar3 == 0) {
    uVar4 = 0;
    do {
      pcVar2 = (char *)(unaff_ESI + 0x11d + uVar4);
      if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) + 0x20 < (char *)0x1a) {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        cVar6 = (char)uVar4 + ' ';
LAB_10016467:
        *pcVar2 = cVar6;
      }
      else {
        if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) < (char *)0x1a) {
          pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
          *pbVar1 = *pbVar1 | 0x20;
          cVar6 = (char)uVar4 + -0x20;
          goto LAB_10016467;
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
          goto LAB_1001640e;
        }
        *(undefined1 *)(unaff_ESI + 0x11d + uVar4) = 0;
      }
      else {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        CVar5 = local_208[uVar4];
LAB_1001640e:
        *(CHAR *)(unaff_ESI + 0x11d + uVar4) = CVar5;
      }
      uVar4 = uVar4 + 1;
    } while (uVar4 < 0x100);
  }
  __security_check_cookie(local_8 ^ (uint)local_4a0);
  return;
}


// 1001648a ___updatetmbcinfo

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
  if (((p_Var1->_ownlocale & DAT_100256cc) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xd);
    lpAddend = p_Var1->ptmbcinfo;
    if (lpAddend != (pthreadmbcinfo)PTR_DAT_100255d0) {
      if (lpAddend != (pthreadmbcinfo)0x0) {
        LVar2 = InterlockedDecrement(&lpAddend->refcount);
        if ((LVar2 == 0) && (lpAddend != (pthreadmbcinfo)&DAT_100251a8)) {
          FUN_10005520(lpAddend);
        }
      }
      p_Var1->ptmbcinfo = (pthreadmbcinfo)PTR_DAT_100255d0;
      lpAddend = (pthreadmbcinfo)PTR_DAT_100255d0;
      InterlockedIncrement((LONG *)PTR_DAT_100255d0);
    }
    FUN_10016525();
  }
  else {
    lpAddend = p_Var1->ptmbcinfo;
  }
  if (lpAddend == (pthreadmbcinfo)0x0) {
    __amsg_exit(0x20);
  }
  return lpAddend;
}


// 10016525 FUN_10016525

void FUN_10016525(void)

{
  FUN_10012689(0xd);
  return;
}


// 1001652e getSystemCP

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
  DAT_10029584 = 0;
  if (unaff_ESI == -2) {
    DAT_10029584 = 1;
    UVar1 = GetOEMCP();
  }
  else if (unaff_ESI == -3) {
    DAT_10029584 = 1;
    UVar1 = GetACP();
  }
  else {
    if (unaff_ESI != -4) {
      if (local_8 == '\0') {
        DAT_10029584 = 0;
        return unaff_ESI;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return unaff_ESI;
    }
    UVar1 = *(UINT *)(local_14[0] + 4);
    DAT_10029584 = 1;
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return UVar1;
}


// 100165a8 FUN_100165a8

void __cdecl FUN_100165a8(undefined4 param_1,int param_2)

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  uVar4 = getSystemCP((int)unaff_EDI);
  if (uVar4 != 0) {
    local_20 = (byte *)0x0;
    uVar5 = 0;
LAB_100165e4:
    if (*(uint *)((int)&DAT_100255d8 + uVar5) != uVar4) goto code_r0x100165f0;
    _memset((void *)(param_2 + 0x1c),0,0x101);
    local_24 = 0;
    pbVar8 = &DAT_100255e8 + (int)local_20 * 0x30;
    local_20 = pbVar8;
    do {
      for (; (*pbVar8 != 0 && (bVar3 = pbVar8[1], bVar3 != 0)); pbVar8 = pbVar8 + 2) {
        for (uVar5 = (uint)*pbVar8; uVar5 <= bVar3; uVar5 = uVar5 + 1) {
          pbVar2 = (byte *)(param_2 + 0x1d + uVar5);
          *pbVar2 = *pbVar2 | *(byte *)(local_24 + 0x100255d4);
          bVar3 = pbVar8[1];
        }
      }
      local_24 = local_24 + 1;
      pbVar8 = local_20 + 8;
      local_20 = pbVar8;
    } while (local_24 < 4);
    *(uint *)(param_2 + 4) = uVar4;
    *(undefined4 *)(param_2 + 8) = 1;
    uVar9 = FUN_1001627c();
    *(undefined4 *)(param_2 + 0xc) = uVar9;
    puVar7 = (undefined2 *)(param_2 + 0x10);
    puVar10 = (undefined2 *)(&DAT_100255dc + extraout_ECX);
    iVar11 = 6;
    do {
      *puVar7 = *puVar10;
      puVar10 = puVar10 + 1;
      puVar7 = puVar7 + 1;
      iVar11 = iVar11 + -1;
    } while (iVar11 != 0);
    goto LAB_10016715;
  }
LAB_100165d1:
  setSBCS(unaff_EDI);
LAB_10016772:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
code_r0x100165f0:
  local_20 = (byte *)((int)local_20 + 1);
  uVar5 = uVar5 + 0x30;
  if (0xef < uVar5) goto code_r0x100165fd;
  goto LAB_100165e4;
code_r0x100165fd:
  if (((uVar4 == 65000) || (uVar4 == 0xfde9)) ||
     (BVar6 = IsValidCodePage(uVar4 & 0xffff), BVar6 == 0)) goto LAB_10016772;
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
      uVar9 = FUN_1001627c();
      *(undefined4 *)(param_2 + 0xc) = uVar9;
      *(undefined4 *)(param_2 + 8) = extraout_EDX;
    }
    *(undefined4 *)(param_2 + 0x10) = 0;
    *(undefined4 *)(param_2 + 0x14) = 0;
    *(undefined4 *)(param_2 + 0x18) = 0;
LAB_10016715:
    setSBUpLow(unaff_EDI);
    goto LAB_10016772;
  }
  if (DAT_10029584 == 0) goto LAB_10016772;
  goto LAB_100165d1;
}


// 10016781 __setmbcp

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
    ptVar3 = (pthreadmbcinfo)FUN_10005720(0x220);
    if (ptVar3 != (pthreadmbcinfo)0x0) {
      ptVar7 = p_Var1->ptmbcinfo;
      ptVar8 = ptVar3;
      for (iVar6 = 0x88; iVar6 != 0; iVar6 = iVar6 + -1) {
        ptVar8->refcount = ptVar7->refcount;
        ptVar7 = (pthreadmbcinfo)&ptVar7->mbcodepage;
        ptVar8 = (pthreadmbcinfo)&ptVar8->mbcodepage;
      }
      ptVar3->refcount = 0;
      local_24 = FUN_100165a8(iVar2,(int)ptVar3);
      if (local_24 == 0) {
        LVar4 = InterlockedDecrement(&p_Var1->ptmbcinfo->refcount);
        if ((LVar4 == 0) && (p_Var1->ptmbcinfo != (pthreadmbcinfo)&DAT_100251a8)) {
          FUN_10005520(p_Var1->ptmbcinfo);
        }
        p_Var1->ptmbcinfo = ptVar3;
        InterlockedIncrement((LONG *)ptVar3);
        if (((p_Var1->_ownlocale & 2) == 0) && (((byte)DAT_100256cc & 1) == 0)) {
          __lock(0xd);
          _DAT_10029594 = ptVar3->mbcodepage;
          _DAT_10029598 = ptVar3->ismbcodepage;
          _DAT_1002959c = *(undefined4 *)ptVar3->mbulinfo;
          for (iVar2 = 0; iVar2 < 5; iVar2 = iVar2 + 1) {
            (&DAT_10029588)[iVar2] = ptVar3->mbulinfo[iVar2 + 2];
          }
          for (iVar2 = 0; iVar2 < 0x101; iVar2 = iVar2 + 1) {
            (&DAT_100253c8)[iVar2] = ptVar3->mbctype[iVar2 + 4];
          }
          for (iVar2 = 0; iVar2 < 0x100; iVar2 = iVar2 + 1) {
            (&DAT_100254d0)[iVar2] = ptVar3->mbcasemap[iVar2 + 4];
          }
          LVar4 = InterlockedDecrement((LONG *)PTR_DAT_100255d0);
          if ((LVar4 == 0) && (PTR_DAT_100255d0 != &DAT_100251a8)) {
            FUN_10005520(PTR_DAT_100255d0);
          }
          PTR_DAT_100255d0 = (undefined *)ptVar3;
          InterlockedIncrement((LONG *)ptVar3);
          FUN_100168e2();
        }
      }
      else if (local_24 == -1) {
        if (ptVar3 != (pthreadmbcinfo)&DAT_100251a8) {
          FUN_10005520(ptVar3);
        }
        pDVar5 = FUN_100125a7();
        *pDVar5 = 0x16;
      }
    }
  }
  return local_24;
}


// 100168e2 FUN_100168e2

void FUN_100168e2(void)

{
  FUN_10012689(0xd);
  return;
}


// 1001691b ___initmbctable

/* Library Function - Single Match
    ___initmbctable
   
   Library: Visual Studio 2005 Release */

undefined4 ___initmbctable(void)

{
  if (DAT_1002ac70 == 0) {
    __setmbcp(-3);
    DAT_1002ac70 = 1;
  }
  return 0;
}


// 10016939 ___freetlocinfo

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
       (*(undefined ***)(param_1 + 0xbc) != &PTR_DAT_100258b8)) &&
      (*(int **)(param_1 + 0xb0) != (int *)0x0)) && (**(int **)(param_1 + 0xb0) == 0)) {
    piVar4 = *(int **)(param_1 + 0xb8);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_10005520(piVar4);
      ___free_lconv_mon(*(int *)(param_1 + 0xbc));
    }
    piVar4 = *(int **)(param_1 + 0xb4);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_10005520(piVar4);
      ___free_lconv_num(*(int **)(param_1 + 0xbc));
    }
    FUN_10005520(*(undefined4 *)(param_1 + 0xb0));
    FUN_10005520(*(undefined4 *)(param_1 + 0xbc));
  }
  if ((*(int **)(param_1 + 0xc0) != (int *)0x0) && (**(int **)(param_1 + 0xc0) == 0)) {
    FUN_10005520(*(int *)(param_1 + 0xc4) + -0xfe);
    FUN_10005520(*(int *)(param_1 + 0xcc) + -0x80);
    FUN_10005520(*(int *)(param_1 + 0xd0) + -0x80);
    FUN_10005520(*(undefined4 *)(param_1 + 0xc0));
  }
  ppuVar1 = *(undefined ***)(param_1 + 0xd4);
  if ((ppuVar1 != &PTR_DAT_100257f8) && (ppuVar1[0x2d] == (undefined *)0x0)) {
    ___free_lc_time(ppuVar1);
    FUN_10005520(*(undefined4 *)(param_1 + 0xd4));
  }
  piVar4 = (int *)(param_1 + 0x50);
  iVar3 = 6;
  do {
    if ((((undefined *)piVar4[-2] != &DAT_100256d0) &&
        (piVar2 = (int *)*piVar4, piVar2 != (int *)0x0)) && (*piVar2 == 0)) {
      FUN_10005520(piVar2);
    }
    if (((piVar4[-1] != 0) && (piVar2 = (int *)piVar4[1], piVar2 != (int *)0x0)) && (*piVar2 == 0))
    {
      FUN_10005520(piVar2);
    }
    piVar4 = piVar4 + 4;
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  FUN_10005520(param_1);
  return;
}


// 10016a79 ___addlocaleref

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
    if (((undefined *)pLVar1[-2] != &DAT_100256d0) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 10016aff ___removelocaleref

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
      if (((undefined *)pLVar1[-2] != &DAT_100256d0) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 10016b8b __updatetlocinfoEx_nolock

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
        if ((*pLVar1 == 0) && (pLVar1 != (LONG *)&DAT_100256d8)) {
          ___freetlocinfo((int)pLVar1);
        }
      }
    }
    return unaff_EDI;
  }
  return (LONG *)0x0;
}


// 10016bc9 ___updatetlocinfo

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___updatetlocinfo
   
   Library: Visual Studio 2005 Release */

pthreadlocinfo __cdecl ___updatetlocinfo(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = __getptd();
  if (((p_Var1->_ownlocale & DAT_100256cc) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xc);
    __updatetlocinfoEx_nolock();
    FUN_10016c33();
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


// 10016c33 FUN_10016c33

void FUN_10016c33(void)

{
  FUN_10012689(0xc);
  return;
}


// 10016c3f __crtLCMapStringA_stat

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  if (DAT_100295c4 == 0) {
    iVar3 = LCMapStringW(0,0x100,L"",1,(LPWSTR)0x0,0);
    if (iVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_100295c4 = 2;
      }
    }
    else {
      DAT_100295c4 = 1;
    }
  }
  pcVar5 = (char *)param_3;
  pcVar8 = param_4;
  if (0 < (int)param_4) {
    do {
      pcVar8 = pcVar8 + -1;
      if (*pcVar5 == '\0') goto LAB_10016cae;
      pcVar5 = pcVar5 + 1;
    } while (pcVar8 != (char *)0x0);
    pcVar8 = (char *)0xffffffff;
LAB_10016cae:
    pcVar5 = param_4 + -(int)pcVar8;
    bVar2 = (int)(pcVar5 + -1) < (int)param_4;
    param_4 = pcVar5 + -1;
    if (bVar2) {
      param_4 = pcVar5;
    }
  }
  if ((DAT_100295c4 == 2) || (DAT_100295c4 == 0)) {
    local_10 = (LPCWSTR)0x0;
    local_14 = 0;
    if (param_1 == (localeinfo_struct *)0x0) {
      param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
    }
    if (param_7 == 0) {
      param_7 = *(int *)(*in_ECX + 4);
    }
    UVar7 = ___ansicp((LCID)param_1);
    if (UVar7 == 0xffffffff) goto LAB_10016fcf;
    if (UVar7 == param_7) {
      LCMapStringA((LCID)param_1,param_2,(LPCSTR)param_3,(int)param_4,(LPSTR)param_5,(int)param_6);
    }
    else {
      local_10 = (LPCWSTR)___convertcp(param_7,UVar7,(char *)param_3,(uint *)&param_4,(LPSTR)0x0,0);
      if (local_10 == (LPCWSTR)0x0) goto LAB_10016fcf;
      local_c = LCMapStringA((LCID)param_1,param_2,(LPCSTR)local_10,(int)param_4,(LPSTR)0x0,0);
      if (local_c != 0) {
        if (((int)local_c < 1) || (0xffffffe0 < local_c)) {
          pcVar5 = (char *)0x0;
        }
        else if (local_c + 8 < 0x401) {
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_10016fac;
          pcVar5 = &stack0xffffffe4;
        }
        else {
          pcVar5 = (char *)FUN_10005720(local_c + 8);
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
LAB_10016fac:
    if (local_10 != (LPCWSTR)0x0) {
      FUN_10005520(local_10);
    }
    if ((local_14 != 0) && (param_5 != local_14)) {
      FUN_10005520(local_14);
    }
    goto LAB_10016fcf;
  }
  if (DAT_100295c4 != 1) goto LAB_10016fcf;
  local_c = 0;
  if (param_7 == 0) {
    param_7 = *(int *)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar(param_7,(uint)(param_8 != 0) * 8 + 1,(LPCSTR)param_3,
                                    (int)param_4,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_10016fcf;
  if (((int)cchWideChar < 1) || (0xffffffe0 / cchWideChar < 2)) {
    local_10 = (LPCWSTR)0x0;
  }
  else {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar6 = (LPCWSTR)&stack0xffffffdc;
      local_10 = (LPCWSTR)&stack0xffffffdc;
      if (&stack0x00000000 != (undefined1 *)0x24) {
LAB_10016d56:
        local_10 = pWVar6 + 4;
      }
    }
    else {
      pWVar6 = (LPCWSTR)FUN_10005720(uVar1);
      local_10 = pWVar6;
      if (pWVar6 != (LPCWSTR)0x0) {
        pWVar6[0] = L'\xdddd';
        pWVar6[1] = L'\0';
        goto LAB_10016d56;
      }
    }
  }
  if (local_10 == (LPCWSTR)0x0) goto LAB_10016fcf;
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
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_10016e65;
          pWVar6 = (LPCWSTR)&stack0xffffffe4;
        }
        else {
          pWVar6 = (LPCWSTR)FUN_10005720(uVar1);
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
LAB_10016e65:
  __freea(local_10);
LAB_10016fcf:
  iVar3 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar3;
}


// 10016fe1 ___crtLCMapStringA

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


// 10017024 fastzero_I

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


// 1001707b __VEC_memzero

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


// 1001710a __setenvp

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
  
  if (DAT_1002ac70 == 0) {
    ___initmbctable();
  }
  iVar5 = 0;
  pcVar4 = DAT_100290c0;
  if (DAT_100290c0 != (char *)0x0) {
    for (; *pcVar4 != '\0'; pcVar4 = pcVar4 + sVar2 + 1) {
      if (*pcVar4 != '=') {
        iVar5 = iVar5 + 1;
      }
      sVar2 = _strlen(pcVar4);
    }
    puVar1 = (undefined4 *)FUN_10005780(iVar5 + 1,4);
    pcVar4 = DAT_100290c0;
    DAT_10029094 = puVar1;
    if (puVar1 != (undefined4 *)0x0) {
      do {
        if (*pcVar4 == '\0') {
          FUN_10005520(DAT_100290c0);
          DAT_100290c0 = (char *)0x0;
          *puVar1 = 0;
          DAT_1002ac64 = 1;
          return 0;
        }
        sVar2 = _strlen(pcVar4);
        _SizeInBytes = sVar2 + 1;
        if (*pcVar4 != '=') {
          _Dst = (char *)FUN_10005780(_SizeInBytes,1);
          *puVar1 = _Dst;
          if (_Dst == (char *)0x0) {
            FUN_10005520(DAT_10029094);
            DAT_10029094 = (undefined4 *)0x0;
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


// 100171e5 parse_cmdline

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
        goto LAB_10017277;
      }
    }
    in_EDX = pbVar7;
  } while ((bVar2) || ((bVar6 != 0x20 && (bVar6 != 9))));
  if (pbVar5 != (byte *)0x0) {
    pbVar5[-1] = 0;
  }
LAB_10017277:
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


// 1001737d __setargv

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
  
  if (DAT_1002ac70 == 0) {
    ___initmbctable();
  }
  DAT_100296cc = 0;
  GetModuleFileNameA((HMODULE)0x0,&DAT_100295c8,0x104);
  _DAT_100290a4 = &DAT_100295c8;
  if ((DAT_10029c4c == (char *)0x0) || (local_8 = DAT_10029c4c, *DAT_10029c4c == '\0')) {
    local_8 = &DAT_100295c8;
  }
  parse_cmdline((undefined4 *)0x0,(byte *)0x0,(int *)&local_c);
  uVar2 = local_c;
  if ((local_c < 0x3fffffff) && (local_10 != 0xffffffff)) {
    uVar1 = local_c * 4 + local_10;
    if ((local_10 <= uVar1) &&
       (puVar3 = (undefined4 *)FUN_10005720(uVar1), puVar3 != (undefined4 *)0x0)) {
      parse_cmdline(puVar3,(byte *)(puVar3 + uVar2),(int *)&local_c);
      _DAT_10029088 = local_c - 1;
      _DAT_1002908c = puVar3;
      return 0;
    }
  }
  return -1;
}


// 10017436 ___crtGetEnvironmentStringsA

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
  if (DAT_100296d0 == 0) {
    lpWideCharStr = GetEnvironmentStringsW();
    if (lpWideCharStr != (LPWCH)0x0) {
      DAT_100296d0 = 1;
      goto LAB_10017488;
    }
    DVar3 = GetLastError();
    if (DVar3 == 0x78) {
      DAT_100296d0 = 2;
    }
  }
  if (DAT_100296d0 != 1) {
    if ((DAT_100296d0 != 2) && (DAT_100296d0 != 0)) {
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
    _Dst = (void *)FUN_10005720(pcVar7 + (1 - (int)_Src));
    if (_Dst == (void *)0x0) {
      FreeEnvironmentStringsA(_Src);
      return (LPVOID)0x0;
    }
    _memcpy(_Dst,_Src,(size_t)(pcVar7 + (1 - (int)_Src)));
    FreeEnvironmentStringsA(_Src);
    return _Dst;
  }
LAB_10017488:
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
      (lpMultiByteStr = (LPSTR)FUN_10005720(cbMultiByte), lpMultiByteStr != (LPSTR)0x0)) &&
     (iVar6 = WideCharToMultiByte(0,0,lpWideCharStr,iVar6,lpMultiByteStr,cbMultiByte,(LPCSTR)0x0,
                                  (LPBOOL)0x0), local_8 = lpMultiByteStr, iVar6 == 0)) {
    FUN_10005520(lpMultiByteStr);
    local_8 = (LPSTR)0x0;
  }
  FreeEnvironmentStringsW(lpWideCharStr);
  return local_8;
}


// 1001756b FUN_1001756b

undefined4 FUN_1001756b(void)

{
  return 1;
}


// 10017571 ___security_init_cookie

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
  if ((DAT_10025040 == 0xbb40e64e) || ((DAT_10025040 & 0xffff0000) == 0)) {
    GetSystemTimeAsFileTime(&local_c);
    uVar4 = local_c.dwHighDateTime ^ local_c.dwLowDateTime;
    DVar1 = GetCurrentProcessId();
    DVar2 = GetCurrentThreadId();
    DVar3 = GetTickCount();
    QueryPerformanceCounter(&local_14);
    DAT_10025040 = uVar4 ^ DVar1 ^ DVar2 ^ DVar3 ^ local_14.s.HighPart ^ local_14.s.LowPart;
    if (DAT_10025040 == 0xbb40e64e) {
      DAT_10025040 = 0xbb40e64f;
    }
    else if ((DAT_10025040 & 0xffff0000) == 0) {
      DAT_10025040 = DAT_10025040 | DAT_10025040 << 0x10;
    }
    DAT_10025044 = ~DAT_10025040;
  }
  else {
    DAT_10025044 = ~DAT_10025040;
  }
  return;
}


// 10017605 _wcscpy_s

/* Library Function - Single Match
    _wcscpy_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _wcscpy_s(wchar_t *_Dst,rsize_t _SizeInWords,wchar_t *_Src)

{
  wchar_t wVar1;
  DWORD *pDVar2;
  wchar_t *pwVar3;
  errno_t eVar4;
  
  if ((_Dst != (wchar_t *)0x0) && (_SizeInWords != 0)) {
    pwVar3 = _Dst;
    if (_Src != (wchar_t *)0x0) {
      do {
        wVar1 = *_Src;
        *pwVar3 = wVar1;
        _Src = _Src + 1;
        if (wVar1 == L'\0') break;
        _SizeInWords = _SizeInWords - 1;
        pwVar3 = pwVar3 + 1;
      } while (_SizeInWords != 0);
      if (_SizeInWords != 0) {
        return 0;
      }
      *_Dst = L'\0';
      pDVar2 = FUN_100125a7();
      eVar4 = 0x22;
      *pDVar2 = 0x22;
      goto LAB_10017624;
    }
    *_Dst = L'\0';
  }
  pDVar2 = FUN_100125a7();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_10017624:
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 10017671 __isleadbyte_l

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


// 100176a7 _isleadbyte

/* Library Function - Single Match
    _isleadbyte
   
   Library: Visual Studio 2005 Release */

int __cdecl _isleadbyte(int _C)

{
  int iVar1;
  
  iVar1 = __isleadbyte_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 100176b5 __isctype_l

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


// 1001776b wcstoxl

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
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
LAB_10017802:
    _C = *pwVar7;
    pwVar7 = pwVar1 + 2;
  }
  else if (_C == L'+') goto LAB_10017802;
  uVar6 = (uint)(ushort)_C;
  if (param_4 == 0) {
    iVar3 = __wchartodigit(_C);
    if (iVar3 != 0) {
      param_4 = 10;
      goto LAB_10017866;
    }
    if ((*pwVar7 != L'x') && (*pwVar7 != L'X')) {
      param_4 = 8;
      goto LAB_10017866;
    }
    param_4 = 0x10;
  }
  if (((param_4 == 0x10) && (iVar3 = __wchartodigit(_C), iVar3 == 0)) &&
     ((*pwVar7 == L'x' || (*pwVar7 == L'X')))) {
    uVar6 = (uint)(ushort)pwVar7[1];
    pwVar7 = pwVar7 + 2;
  }
LAB_10017866:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  local_c = (uint)(0xffffffff % (ulonglong)(uint)param_4);
  do {
    uVar8 = (ushort)uVar6;
    uVar5 = __wchartodigit(uVar8);
    if (uVar5 == 0xffffffff) {
      if (((uVar8 < 0x41) || (0x5a < uVar8)) && (0x19 < (ushort)(uVar8 - 0x61))) {
LAB_100178c5:
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
          pDVar2 = FUN_100125a7();
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
    if ((uint)param_4 <= uVar5) goto LAB_100178c5;
    if ((uVar9 < uVar4) || ((uVar9 == uVar4 && (uVar5 <= local_c)))) {
      uVar9 = uVar9 * param_4 + uVar5;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (wchar_t **)0x0) goto LAB_100178c5;
    }
    uVar6 = (uint)(ushort)*pwVar7;
    pwVar7 = pwVar7 + 1;
  } while( true );
}


// 10017962 _wcstol

/* Library Function - Single Match
    _wcstol
   
   Library: Visual Studio 2005 Release */

long __cdecl _wcstol(wchar_t *_Str,wchar_t **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_100295a0 == 0) {
    ppuVar2 = &PTR_DAT_100257b8;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = wcstoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 1001798b __iswctype_l

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
    local_8[0] = *(ushort *)(PTR_DAT_100257f4 + (uint)_C * 2) & _Type;
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


// 10017a0d __fltin2

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  local_24 = _Str;
  uVar3 = 0;
  local_18 = FUN_10017ab2((undefined2 *)&local_14,&local_28,_Str,0,0,0,0,in_stack_00000018);
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


// 10017ab2 FUN_10017ab2

void __cdecl
FUN_10017ab2(undefined2 *param_1,int *param_2,char *param_3,int param_4,int param_5,int param_6,
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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
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
LAB_10017b37:
    cVar8 = *param_3;
    pcVar17 = param_3 + 1;
    switch(iVar14) {
    case 0:
      if ((byte)(cVar8 - 0x31U) < 9) {
LAB_10017b54:
        iVar14 = 3;
        goto LAB_10017b56;
      }
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) {
LAB_10017b6b:
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
          if (cVar8 == '0') goto LAB_10017b85;
          goto LAB_10017d10;
        }
        iVar14 = 2;
        uVar7 = 0x8000;
        param_3 = pcVar17;
      }
      goto LAB_10017b37;
    case 1:
      bVar4 = true;
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_10017b54;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10017bbc;
      if ((cVar8 == '+') || (cVar8 == '-')) goto LAB_10017bec;
      if (cVar8 == '0') goto LAB_10017b85;
      goto LAB_10017bcc;
    case 2:
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_10017b54;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10017b6b;
      param_3 = local_58;
      if (cVar8 != '0') goto LAB_10017d3b;
LAB_10017b85:
      iVar14 = 1;
      param_3 = pcVar17;
      goto LAB_10017b37;
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
      if (cVar8 != *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_10017c5f;
LAB_10017bbc:
      bVar4 = true;
      iVar14 = 4;
      param_3 = pcVar17;
      goto LAB_10017b37;
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
LAB_10017c5f:
      if ((cVar8 == '+') || (cVar8 == '-')) {
LAB_10017bec:
        bVar4 = true;
        iVar14 = 0xb;
        param_3 = pcVar17 + -1;
      }
      else {
LAB_10017bcc:
        bVar4 = true;
        if ((cVar8 < 'D') || (('E' < cVar8 && ((cVar8 < 'd' || ('e' < cVar8)))))) goto LAB_10017d10;
        iVar14 = 6;
        param_3 = pcVar17;
      }
      goto LAB_10017b37;
    case 5:
      bVar6 = true;
      param_3 = local_58;
      if ((byte)(cVar8 - 0x30U) < 10) {
        iVar14 = 4;
        goto LAB_10017b56;
      }
      goto LAB_10017d3b;
    case 6:
      local_58 = param_3 + -1;
      if (8 < (byte)(cVar8 - 0x31U)) {
        if (cVar8 == '+') goto LAB_10017cf7;
        if (cVar8 == '-') goto LAB_10017ceb;
LAB_10017cde:
        param_3 = local_58;
        if (cVar8 != '0') goto LAB_10017d3b;
        iVar14 = 8;
        param_3 = pcVar17;
        goto LAB_10017b37;
      }
      break;
    case 7:
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_10017cde;
      break;
    case 8:
      bVar5 = true;
      while (cVar8 == '0') {
        cVar8 = *pcVar17;
        pcVar17 = pcVar17 + 1;
      }
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_10017d10;
      break;
    case 9:
      bVar5 = true;
      local_68 = 0;
      goto LAB_10017d9d;
    default:
      goto switchD_10017b43_caseD_a;
    case 0xb:
      if (param_7 != 0) {
        local_58 = param_3;
        if (cVar8 == '+') {
LAB_10017cf7:
          iVar14 = 7;
          param_3 = pcVar17;
        }
        else {
          if (cVar8 != '-') goto LAB_10017d3b;
LAB_10017ceb:
          local_6c = -1;
          iVar14 = 7;
          param_3 = pcVar17;
        }
        goto LAB_10017b37;
      }
      iVar14 = 10;
      pcVar17 = param_3;
switchD_10017b43_caseD_a:
      param_3 = pcVar17;
      if (iVar14 != 10) goto LAB_10017b37;
      goto LAB_10017d3b;
    }
    iVar14 = 9;
LAB_10017b56:
    param_3 = pcVar17 + -1;
    goto LAB_10017b37;
  }
  pDVar10 = FUN_100125a7();
  *pDVar10 = 0x16;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
  goto LAB_10018133;
LAB_10017d9d:
  if ((cVar8 < '0') || ('9' < cVar8)) goto LAB_10017db8;
  local_68 = local_68 * 10 + -0x30 + (int)cVar8;
  if (local_68 < 0x1451) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
    goto LAB_10017d9d;
  }
  local_68 = 0x1451;
LAB_10017db8:
  while (('/' < cVar8 && (cVar8 < ':'))) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
  }
LAB_10017d10:
  param_3 = pcVar17 + -1;
LAB_10017d3b:
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
    if (local_50 == 0) goto LAB_10018115;
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
      if ((int)local_58 < -0x1450) goto LAB_10018115;
      ppuVar20 = &PTR_DAT_100258d0;
      if (local_58 != (char *)0x0) {
        if ((int)local_58 < 0) {
          local_58 = (char *)-(int)local_58;
          ppuVar20 = (undefined **)&DAT_10025a30;
        }
        if (param_4 == 0) {
          local_40._0_2_ = 0;
        }
        iVar14 = uStack_4a;
        uVar22 = uStack_2e;
        iVar1 = uStack_2a;
        uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
        iVar12 = local_3c;
joined_r0x10017e43:
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
LAB_10017ff9:
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
                    if ((short)uVar24 < 1) goto LAB_10017ff9;
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
                  if (0x7ffe < uVar24) goto LAB_100180b8;
                  local_40 = uVar22;
                  local_3c = iVar1;
                  uStack_38 = uStack_26;
                  local_36 = uVar24 | uVar9;
                  uVar2 = uVar22;
                  iVar12 = iVar1;
                }
                goto joined_r0x10017e43;
              }
              iVar12 = 0;
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            else {
LAB_100180b8:
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
          goto joined_r0x10017e43;
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
LAB_10018115:
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
LAB_10018133:
  local_40 = uVar2;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 10018180 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_10017ab2((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tod(&local_14,(_CRT_DOUBLE *)_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_100181d7:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10018217;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_10018209:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_10018217;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_10018209;
    goto LAB_100181d7;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_10018217:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 10018226 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_10017ab2((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tof(&local_14,_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_1001827d:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_100182bd;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_100182af:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_100182bd;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_100182af;
    goto LAB_1001827d;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_100182bd:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 100182cc __fptostr

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
    pDVar1 = FUN_100125a7();
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
        FID_conflict__memcpy(_Buf,_Buf + 1,sVar4 + 1);
      }
      return 0;
    }
    pDVar1 = FUN_100125a7();
    eVar7 = 0x22;
    *pDVar1 = 0x22;
  }
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar7;
}


// 10018389 ___dtold

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
      goto LAB_1001843b;
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
LAB_1001843b:
  *(ushort *)(param_1 + 2) = uVar4;
  return;
}


// 10018444 __fltout2

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  ___dtold(&local_14,(uint *)&_Dbl);
  iVar1 = FUN_1001d357(local_14,uStack_10,uStack_c,0x11,0,&local_30);
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


// 100184e0 __alldvrm

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


// 100185c0 __aullshr

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


// 100185e8 __controlfp_s

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
    pDVar2 = FUN_100125a7();
    eVar3 = 0x16;
    *pDVar2 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return eVar3;
}


// 10018652 __wcsnicoll_l

/* Library Function - Single Match
    __wcsnicoll_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __wcsnicoll_l(wchar_t *_Str1,wchar_t *_Str2,size_t _MaxCount,_locale_t _Locale)

{
  wchar_t wVar1;
  wchar_t wVar2;
  DWORD *pDVar3;
  int iVar4;
  int local_14 [2];
  int local_c;
  char local_8;
  
  if (_MaxCount == 0) {
    return 0;
  }
  if (_Str1 == (wchar_t *)0x0) {
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x7fffffff;
  }
  if (_Str2 == (wchar_t *)0x0) {
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x7fffffff;
  }
  if (_MaxCount < 0x80000000) {
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
    if (*(DWORD *)(local_14[0] + 0x10) == 0) {
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
      iVar4 = (uint)(ushort)wVar1 - (uint)(ushort)wVar2;
    }
    else {
      iVar4 = ___crtCompareStringW
                        ((LPCWSTR)local_14,*(DWORD *)(local_14[0] + 0x10),(LPCWSTR)0x1001,(int)_Str1
                         ,(LPCWSTR)_MaxCount,(int)_Str2);
      if (iVar4 == 0) {
        pDVar3 = FUN_100125a7();
        *pDVar3 = 0x16;
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        goto LAB_1001877b;
      }
      iVar4 = iVar4 + -2;
    }
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  else {
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
LAB_1001877b:
    iVar4 = 0x7fffffff;
  }
  return iVar4;
}


// 10018793 __wcsnicoll

/* Library Function - Single Match
    __wcsnicoll
   
   Library: Visual Studio 2005 Release */

int __cdecl __wcsnicoll(wchar_t *_Str1,wchar_t *_Str2,size_t _MaxCount)

{
  wchar_t wVar1;
  wchar_t wVar2;
  DWORD *pDVar3;
  int iVar4;
  
  if (DAT_100295a0 == 0) {
    if (((_Str1 == (wchar_t *)0x0) || (_Str2 == (wchar_t *)0x0)) || (0x7fffffff < _MaxCount)) {
      pDVar3 = FUN_100125a7();
      *pDVar3 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      iVar4 = 0x7fffffff;
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
      iVar4 = (uint)(ushort)wVar1 - (uint)(ushort)wVar2;
    }
  }
  else {
    iVar4 = __wcsnicoll_l(_Str1,_Str2,_MaxCount,(_locale_t)0x0);
  }
  return iVar4;
}


// 10018835 ___mbtow_environ

/* Library Function - Single Match
    ___mbtow_environ
   
   Library: Visual Studio 2005 Release */

int __cdecl ___mbtow_environ(void)

{
  LPCSTR lpMultiByteStr;
  int iVar1;
  int extraout_EAX;
  undefined4 *puVar2;
  LPWSTR local_8;
  
  local_8 = (LPWSTR)0x0;
  lpMultiByteStr = (LPCSTR)*DAT_10029094;
  puVar2 = DAT_10029094;
  while( true ) {
    if (lpMultiByteStr == (LPCSTR)0x0) {
      return 0;
    }
    iVar1 = MultiByteToWideChar(0,0,lpMultiByteStr,-1,(LPWSTR)0x0,0);
    if (iVar1 == 0) {
      return -1;
    }
    local_8 = (LPWSTR)FUN_10005780(iVar1,2);
    if (local_8 == (LPWSTR)0x0) {
      return -1;
    }
    iVar1 = MultiByteToWideChar(0,0,(LPCSTR)*puVar2,-1,local_8,iVar1);
    if (iVar1 == 0) break;
    FUN_10011bf2(&local_8,0);
    if (extraout_EAX < 0) {
      if (local_8 == (LPWSTR)0x0) {
        return -1;
      }
      break;
    }
    puVar2 = puVar2 + 1;
    lpMultiByteStr = (LPCSTR)*puVar2;
  }
  FUN_10005520(local_8);
  return -1;
}


// 100188ba __wsetenvp

/* Library Function - Single Match
    __wsetenvp
   
   Library: Visual Studio 2005 Release */

int __cdecl __wsetenvp(void)

{
  int iVar1;
  undefined4 *puVar2;
  wchar_t *_Dst;
  errno_t eVar3;
  wchar_t *pwVar4;
  int iVar5;
  rsize_t _SizeInWords;
  
  iVar5 = 0;
  pwVar4 = DAT_100290c4;
  if (DAT_100290c4 == (wchar_t *)0x0) {
    iVar5 = -1;
  }
  else {
    for (; *pwVar4 != L'\0'; pwVar4 = pwVar4 + iVar1 + 1) {
      if (*pwVar4 != L'=') {
        iVar5 = iVar5 + 1;
      }
      iVar1 = FUN_10011f58(pwVar4);
    }
    puVar2 = (undefined4 *)FUN_10005780(iVar5 + 1,4);
    pwVar4 = DAT_100290c4;
    DAT_1002909c = puVar2;
    if (puVar2 == (undefined4 *)0x0) {
      iVar5 = -1;
    }
    else {
      for (; *pwVar4 != L'\0'; pwVar4 = pwVar4 + _SizeInWords) {
        iVar5 = FUN_10011f58(pwVar4);
        _SizeInWords = iVar5 + 1;
        if (*pwVar4 != L'=') {
          _Dst = (wchar_t *)FUN_10005780(_SizeInWords,2);
          *puVar2 = _Dst;
          if (_Dst == (wchar_t *)0x0) {
            FUN_10005520(DAT_1002909c);
            DAT_1002909c = (undefined4 *)0x0;
            return -1;
          }
          eVar3 = _wcscpy_s(_Dst,_SizeInWords,pwVar4);
          if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
            __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
          }
          puVar2 = puVar2 + 1;
        }
      }
      FUN_10005520(DAT_100290c4);
      DAT_100290c4 = (wchar_t *)0x0;
      *puVar2 = 0;
      DAT_1002ac64 = 1;
      iVar5 = 0;
    }
  }
  return iVar5;
}


// 10018994 ___crtGetEnvironmentStringsW

/* Library Function - Single Match
    ___crtGetEnvironmentStringsW
   
   Library: Visual Studio 2005 Release */

LPVOID __cdecl ___crtGetEnvironmentStringsW(void)

{
  char *pcVar1;
  char cVar2;
  WCHAR WVar3;
  LPWSTR lpWideCharStr;
  DWORD DVar4;
  WCHAR *pWVar5;
  WCHAR *pWVar6;
  size_t sVar7;
  void *_Dst;
  LPCH pCVar8;
  int iVar9;
  LPWSTR pWVar10;
  LPWCH _Src;
  int iVar11;
  
  iVar11 = 0;
  _Src = (LPWCH)0x0;
  if (DAT_100296e0 == 0) {
    _Src = GetEnvironmentStringsW();
    if (_Src != (LPWCH)0x0) {
      DAT_100296e0 = 1;
      goto LAB_100189df;
    }
    DVar4 = GetLastError();
    if (DVar4 == 0x78) {
      DAT_100296e0 = 2;
    }
  }
  if (DAT_100296e0 != 1) {
    if ((DAT_100296e0 != 2) && (DAT_100296e0 != 0)) {
      return (LPVOID)0x0;
    }
    pCVar8 = GetEnvironmentStrings();
    if (pCVar8 == (LPCH)0x0) {
      return (LPVOID)0x0;
    }
    cVar2 = *pCVar8;
    pcVar1 = pCVar8;
    while (cVar2 != '\0') {
      iVar9 = MultiByteToWideChar(0,1,pcVar1,-1,(LPWSTR)0x0,0);
      if (iVar9 == 0) {
        return (LPVOID)0x0;
      }
      iVar11 = iVar11 + iVar9;
      sVar7 = _strlen(pcVar1);
      pcVar1 = pcVar1 + sVar7 + 1;
      cVar2 = *pcVar1;
    }
    pWVar10 = (LPWSTR)FUN_10005780(iVar11 + 1,2);
    if (pWVar10 != (LPWSTR)0x0) {
      cVar2 = *pCVar8;
      pcVar1 = pCVar8;
      lpWideCharStr = pWVar10;
      while( true ) {
        if (cVar2 == '\0') {
          *lpWideCharStr = L'\0';
          FreeEnvironmentStringsA(pCVar8);
          return pWVar10;
        }
        iVar9 = MultiByteToWideChar(0,1,pcVar1,-1,lpWideCharStr,
                                    (iVar11 + 1) - ((int)lpWideCharStr - (int)pWVar10 >> 1));
        if (iVar9 == 0) break;
        sVar7 = _strlen(pcVar1);
        pcVar1 = pcVar1 + sVar7 + 1;
        iVar9 = FUN_10011f58(lpWideCharStr);
        cVar2 = *pcVar1;
        lpWideCharStr = lpWideCharStr + iVar9 + 1;
      }
      FUN_10005520(pWVar10);
    }
    FreeEnvironmentStringsA(pCVar8);
    return (LPVOID)0x0;
  }
LAB_100189df:
  if ((_Src == (LPWCH)0x0) && (_Src = GetEnvironmentStringsW(), _Src == (LPWCH)0x0)) {
    return (LPVOID)0x0;
  }
  WVar3 = *_Src;
  pWVar6 = _Src;
  while (WVar3 != L'\0') {
    do {
      pWVar5 = pWVar6;
      pWVar6 = pWVar5 + 1;
    } while (*pWVar6 != L'\0');
    pWVar6 = pWVar5 + 2;
    WVar3 = *pWVar6;
  }
  sVar7 = (int)pWVar6 + (2 - (int)_Src);
  _Dst = (void *)FUN_10005720(sVar7);
  if (_Dst != (void *)0x0) {
    _memcpy(_Dst,_Src,sVar7);
  }
  FreeEnvironmentStringsW(_Src);
  return _Dst;
}


// 10018af5 _wcschr

/* Library Function - Single Match
    _wcschr
   
   Library: Visual Studio 2005 Release */

wchar_t * __cdecl _wcschr(wchar_t *_Str,wchar_t _Ch)

{
  while( true ) {
    if (*_Str == L'\0') {
      if (*_Str != _Ch) {
        _Str = (wchar_t *)0x0;
      }
      return _Str;
    }
    if (*_Str == _Ch) break;
    _Str = _Str + 1;
  }
  return _Str;
}


// 10018b17 FUN_10018b17

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10018b17(void)

{
  _DAT_10029b34 = 0;
  return;
}


// 10018b1f __crtCompareStringA_stat

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* Library Function - Single Match
    int __cdecl __crtCompareStringA_stat(struct localeinfo_struct *,unsigned long,unsigned long,char
   const *,int,char const *,int,int)
   
   Library: Visual Studio 2005 Release */

int __cdecl
__crtCompareStringA_stat
          (localeinfo_struct *param_1,ulong param_2,ulong param_3,char *param_4,int param_5,
          char *param_6,int param_7,int param_8)

{
  uint uVar1;
  char *lpMultiByteStr;
  int iVar2;
  DWORD DVar3;
  BOOL BVar4;
  BYTE *pBVar5;
  uint cchWideChar;
  PCNZWCH pWVar6;
  uint uVar7;
  LPWSTR lpWideCharStr;
  char *pcVar8;
  int *in_ECX;
  ulong uVar9;
  byte *in_EDX;
  byte *pbVar10;
  PCNZCH pCVar11;
  byte *local_28;
  PCNZWCH local_24;
  char *local_20;
  _cpinfo local_1c;
  uint local_8;
  
  lpMultiByteStr = param_4;
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  local_20 = param_4;
  if (DAT_100296e4 == 0) {
    iVar2 = CompareStringW(0,0,L"",1,L"",1);
    if (iVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_100296e4 = 2;
      }
    }
    else {
      DAT_100296e4 = 1;
    }
  }
  pbVar10 = in_EDX;
  uVar9 = param_3;
  if ((int)param_3 < 1) {
    if ((int)param_3 < -1) goto LAB_10018e96;
  }
  else {
    do {
      uVar9 = uVar9 - 1;
      if (*pbVar10 == 0) goto LAB_10018b99;
      pbVar10 = pbVar10 + 1;
    } while (uVar9 != 0);
    uVar9 = 0xffffffff;
LAB_10018b99:
    param_3 = param_3 + (-1 - uVar9);
  }
  pcVar8 = lpMultiByteStr;
  iVar2 = param_5;
  if (param_5 < 1) {
    if (param_5 < -1) goto LAB_10018e96;
  }
  else {
    do {
      iVar2 = iVar2 + -1;
      if (*pcVar8 == '\0') goto LAB_10018bbb;
      pcVar8 = pcVar8 + 1;
    } while (iVar2 != 0);
    iVar2 = -1;
LAB_10018bbb:
    param_5 = param_5 + (-1 - iVar2);
  }
  if ((DAT_100296e4 == 2) || (DAT_100296e4 == 0)) {
    pCVar11 = (PCNZCH)0x0;
    pbVar10 = (byte *)0x0;
    if (param_1 == (localeinfo_struct *)0x0) {
      param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
    }
    if (param_6 == (char *)0x0) {
      param_6 = *(char **)(*in_ECX + 4);
    }
    pcVar8 = (char *)___ansicp((LCID)param_1);
    if (pcVar8 == (char *)0xffffffff) goto LAB_10018e96;
    local_28 = in_EDX;
    if (pcVar8 != param_6) {
      pbVar10 = (byte *)___convertcp((UINT)param_6,(UINT)pcVar8,(char *)in_EDX,&param_3,(LPSTR)0x0,0
                                    );
      if (pbVar10 == (byte *)0x0) goto LAB_10018e96;
      pCVar11 = (PCNZCH)___convertcp((UINT)param_6,(UINT)pcVar8,lpMultiByteStr,(uint *)&param_5,
                                     (LPSTR)0x0,0);
      local_28 = pbVar10;
      local_20 = pCVar11;
      if (pCVar11 == (PCNZCH)0x0) {
        FUN_10005520(pbVar10);
        goto LAB_10018e96;
      }
    }
    CompareStringA((LCID)param_1,param_2,(PCNZCH)local_28,param_3,local_20,param_5);
    if (pbVar10 != (byte *)0x0) {
      FUN_10005520(pbVar10);
      FUN_10005520(pCVar11);
    }
    goto LAB_10018e96;
  }
  if (DAT_100296e4 != 1) goto LAB_10018e96;
  if (param_6 == (char *)0x0) {
    param_6 = *(char **)(*in_ECX + 4);
  }
  if ((param_3 == 0) || (param_5 == 0)) {
    if ((param_3 == param_5) ||
       (((1 < param_5 || (1 < (int)param_3)) ||
        (BVar4 = GetCPInfo((UINT)param_6,&local_1c), BVar4 == 0)))) goto LAB_10018e96;
    if (0 < (int)param_3) {
      if (1 < local_1c.MaxCharSize) {
        pBVar5 = local_1c.LeadByte;
        while (((local_1c.LeadByte[0] != 0 && (pBVar5[1] != 0)) &&
               ((*in_EDX < *pBVar5 || (pBVar5[1] < *in_EDX))))) {
          pBVar5 = pBVar5 + 2;
          local_1c.LeadByte[0] = *pBVar5;
        }
      }
      goto LAB_10018e96;
    }
    if (0 < param_5) {
      if (1 < local_1c.MaxCharSize) {
        pBVar5 = local_1c.LeadByte;
        while (((local_1c.LeadByte[0] != 0 && (pBVar5[1] != 0)) &&
               (((byte)*lpMultiByteStr < *pBVar5 || (pBVar5[1] < (byte)*lpMultiByteStr))))) {
          pBVar5 = pBVar5 + 2;
          local_1c.LeadByte[0] = *pBVar5;
        }
      }
      goto LAB_10018e96;
    }
  }
  cchWideChar = MultiByteToWideChar((UINT)param_6,9,(LPCSTR)in_EDX,param_3,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_10018e96;
  if (((int)cchWideChar < 1) || (0xffffffe0 / cchWideChar < 2)) {
    local_24 = (PCNZWCH)0x0;
  }
  else {
    uVar7 = cchWideChar * 2 + 8;
    if (uVar7 < 0x401) {
      pWVar6 = (PCNZWCH)&stack0xffffffc4;
      local_24 = (PCNZWCH)&stack0xffffffc4;
      if (&stack0x00000000 != (undefined1 *)0x3c) {
LAB_10018d0f:
        local_24 = pWVar6 + 4;
      }
    }
    else {
      pWVar6 = (PCNZWCH)FUN_10005720(uVar7);
      local_24 = pWVar6;
      if (pWVar6 != (PCNZWCH)0x0) {
        pWVar6[0] = L'\xdddd';
        pWVar6[1] = L'\0';
        goto LAB_10018d0f;
      }
    }
  }
  if (local_24 == (PCNZWCH)0x0) goto LAB_10018e96;
  iVar2 = MultiByteToWideChar((UINT)param_6,1,(LPCSTR)in_EDX,param_3,local_24,cchWideChar);
  if ((iVar2 != 0) &&
     (uVar7 = MultiByteToWideChar((UINT)param_6,9,lpMultiByteStr,param_5,(LPWSTR)0x0,0), uVar7 != 0)
     ) {
    if (((int)uVar7 < 1) || (0xffffffe0 / uVar7 < 2)) {
      lpWideCharStr = (LPWSTR)0x0;
    }
    else {
      uVar1 = uVar7 * 2 + 8;
      if (uVar1 < 0x401) {
        if (&stack0x00000000 == (undefined1 *)0x3c) goto LAB_10018dd0;
        lpWideCharStr = (LPWSTR)&stack0xffffffcc;
      }
      else {
        lpWideCharStr = (LPWSTR)FUN_10005720(uVar1);
        if (lpWideCharStr != (LPWSTR)0x0) {
          lpWideCharStr[0] = L'\xdddd';
          lpWideCharStr[1] = L'\0';
          lpWideCharStr = lpWideCharStr + 4;
        }
      }
    }
    if (lpWideCharStr != (LPWSTR)0x0) {
      iVar2 = MultiByteToWideChar((UINT)param_6,1,lpMultiByteStr,param_5,lpWideCharStr,uVar7);
      if (iVar2 != 0) {
        CompareStringW((LCID)param_1,param_2,local_24,cchWideChar,lpWideCharStr,uVar7);
      }
      __freea(lpWideCharStr);
    }
  }
LAB_10018dd0:
  __freea(local_24);
LAB_10018e96:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 10018ea8 ___crtCompareStringA

/* Library Function - Single Match
    ___crtCompareStringA
   
   Library: Visual Studio 2005 Release */

int __cdecl
___crtCompareStringA
          (_locale_t _Plocinfo,LPCWSTR _LocaleName,DWORD _DwCmpFlags,LPCSTR _LpString1,
          int _CchCount1,LPCSTR _LpString2,int _CchCount2,int _Code_page)

{
  int iVar1;
  int in_stack_ffffffec;
  int in_stack_fffffff0;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&stack0xffffffec,_Plocinfo);
  iVar1 = __crtCompareStringA_stat
                    ((localeinfo_struct *)_LocaleName,_DwCmpFlags,_CchCount1,_LpString2,_CchCount2,
                     (char *)_Code_page,in_stack_ffffffec,in_stack_fffffff0);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return iVar1;
}


// 10018ee8 __strnicoll_l

/* Library Function - Single Match
    __strnicoll_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __strnicoll_l(char *_Str1,char *_Str2,size_t _MaxCount,_locale_t _Locale)

{
  LPCWSTR _LocaleName;
  DWORD *pDVar1;
  int iVar2;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  if (_MaxCount == 0) {
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    return 0;
  }
  if ((_Str1 == (char *)0x0) || (_Str2 == (char *)0x0)) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    return 0x7fffffff;
  }
  if (_MaxCount < 0x80000000) {
    _LocaleName = (LPCWSTR)(local_14.locinfo)->lc_category[0].locale;
    if (_LocaleName == (LPCWSTR)0x0) {
      iVar2 = __strnicmp_l(_Str1,_Str2,_MaxCount,&local_14);
    }
    else {
      iVar2 = ___crtCompareStringA
                        (&local_14,_LocaleName,0x1001,_Str1,_MaxCount,_Str2,_MaxCount,
                         (local_14.locinfo)->lc_collate_cp);
      if (iVar2 == 0) {
        pDVar1 = FUN_100125a7();
        *pDVar1 = 0x16;
        goto LAB_10018fbd;
      }
      iVar2 = iVar2 + -2;
    }
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  else {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
LAB_10018fbd:
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    iVar2 = 0x7fffffff;
  }
  return iVar2;
}


// 10018fe0 __dospawn

/* Library Function - Single Match
    __dospawn
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __dospawn(int _Mode,char *_Name,char *_Cmd,char *_Env)

{
  byte bVar1;
  char *pcVar2;
  uint uVar3;
  undefined4 *puVar4;
  undefined4 *puVar5;
  DWORD *pDVar6;
  BOOL BVar7;
  DWORD DVar8;
  uint *puVar9;
  int iVar10;
  HANDLE pvVar11;
  uint uVar12;
  _STARTUPINFOA local_6c;
  _PROCESS_INFORMATION local_28;
  HANDLE local_18;
  char *local_14;
  DWORD local_10;
  uint local_c;
  char local_5;
  
  pvVar11 = (HANDLE)0x0;
  local_10 = 0;
  local_5 = '\0';
  if ((_Mode != 0) && (_Mode != 1)) {
    if (_Mode < 2) {
LAB_10019014:
      pDVar6 = FUN_100125ba();
      *pDVar6 = 0;
      pDVar6 = FUN_100125a7();
      *pDVar6 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return -1;
    }
    if (3 < _Mode) {
      if (_Mode != 4) goto LAB_10019014;
      local_5 = '\x01';
    }
  }
  local_14 = _Cmd;
  while (*_Cmd != '\0') {
    do {
      pcVar2 = _Cmd;
      _Cmd = pcVar2 + 1;
    } while (*_Cmd != '\0');
    pcVar2 = pcVar2 + 2;
    if (*pcVar2 != '\0') {
      *_Cmd = ' ';
      _Cmd = pcVar2;
    }
  }
  _memset(&local_6c,0,0x44);
  local_6c.cb = 0x44;
  uVar12 = DAT_10029b38;
  uVar3 = DAT_10029b38;
  if (DAT_10029b38 != 0) {
    do {
      uVar3 = uVar3 - 1;
      if (*(char *)((&DAT_10029b40)[(int)uVar3 >> 5] + 4 + (uVar3 & 0x1f) * 0x38) != '\0') break;
      uVar12 = uVar12 - 1;
    } while (uVar12 != 0);
    if ((int)uVar12 < 0) goto LAB_100191da;
  }
  if (0x3331 < uVar12) {
LAB_100191da:
    pDVar6 = FUN_100125a7();
    *pDVar6 = 0xc;
    return -1;
  }
  local_6c.cbReserved2 = (short)uVar12 * 5 + 4;
  local_6c.lpReserved2 = (LPBYTE)FUN_10005780((uint)local_6c.cbReserved2,1);
  if ((uint *)local_6c.lpReserved2 == (uint *)0x0) goto LAB_100191da;
  *(uint *)local_6c.lpReserved2 = uVar12;
  puVar9 = (uint *)((int)local_6c.lpReserved2 + 4);
  puVar5 = (undefined4 *)((int)local_6c.lpReserved2 + uVar12 + 4);
  local_c = 0;
  if (0 < (int)uVar12) {
    do {
      puVar4 = (undefined4 *)((local_c & 0x1f) * 0x38 + (&DAT_10029b40)[(int)local_c >> 5]);
      bVar1 = *(byte *)(puVar4 + 1);
      if ((bVar1 & 0x10) == 0) {
        *(byte *)puVar9 = bVar1;
        *puVar5 = *puVar4;
      }
      else {
        *(byte *)puVar9 = 0;
        *puVar5 = 0xffffffff;
      }
      local_c = local_c + 1;
      puVar9 = (uint *)((int)puVar9 + 1);
      puVar5 = puVar5 + 1;
    } while ((int)local_c < (int)uVar12);
  }
  if (local_5 != '\0') {
    puVar9 = (uint *)((int)local_6c.lpReserved2 + 4);
    iVar10 = 0;
    puVar5 = (undefined4 *)((int)local_6c.lpReserved2 + uVar12 + 4);
    while( true ) {
      uVar3 = uVar12;
      if (2 < (int)uVar12) {
        uVar3 = 3;
      }
      if ((int)uVar3 <= iVar10) break;
      *(undefined1 *)puVar9 = 0;
      *puVar5 = 0xffffffff;
      iVar10 = iVar10 + 1;
      puVar9 = (uint *)((int)puVar9 + 1);
      puVar5 = puVar5 + 1;
    }
    local_10 = 8;
  }
  pDVar6 = FUN_100125ba();
  *pDVar6 = 0;
  BVar7 = CreateProcessA(_Name,local_14,(LPSECURITY_ATTRIBUTES)0x0,(LPSECURITY_ATTRIBUTES)0x0,1,
                         local_10,_Env,(LPCSTR)0x0,&local_6c,&local_28);
  DVar8 = GetLastError();
  FUN_10005520(local_6c.lpReserved2);
  if (BVar7 == 0) {
    FUN_100125cd(DVar8);
    return -1;
  }
  if (_Mode == 2) {
                    /* WARNING: Subroutine does not return */
    __exit(0);
  }
  if (_Mode == 0) {
    WaitForSingleObject(local_28.hProcess,0xffffffff);
    GetExitCodeProcess(local_28.hProcess,(LPDWORD)&local_18);
    pvVar11 = local_18;
  }
  else if (_Mode != 4) goto LAB_100191d1;
  CloseHandle(local_28.hProcess);
  local_28.hProcess = pvVar11;
LAB_100191d1:
  CloseHandle(local_28.hThread);
  return (intptr_t)local_28.hProcess;
}


// 100191ed __cenvarg

/* Library Function - Single Match
    __cenvarg
   
   Library: Visual Studio 2005 Release */

int __cdecl __cenvarg(char **_Argv,char **_Env,char **_ArgBlk,char **_EnvBlk,char *_Name)

{
  bool bVar1;
  char *pcVar2;
  DWORD *pDVar3;
  errno_t eVar4;
  size_t sVar5;
  int iVar6;
  char cVar7;
  char *pcVar8;
  rsize_t _SizeInBytes;
  char **ppcVar9;
  char *local_44;
  rsize_t local_34;
  char *local_2c;
  char *local_20;
  char *local_18;
  uchar local_14 [12];
  uint local_8;
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  builtin_memcpy(local_14,"SystemRoot",0xb);
  pcVar2 = *_Argv;
  local_18 = (char *)0x0;
  bVar1 = false;
  _SizeInBytes = 2;
  ppcVar9 = _Argv;
  while (pcVar2 != (char *)0x0) {
    sVar5 = _strlen(pcVar2);
    ppcVar9 = ppcVar9 + 1;
    _SizeInBytes = _SizeInBytes + 1 + sVar5;
    pcVar2 = *ppcVar9;
  }
  pcVar2 = (char *)FUN_10005780(_SizeInBytes,1);
  *_ArgBlk = pcVar2;
  if (pcVar2 == (char *)0x0) {
    *_EnvBlk = (char *)0x0;
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0xc;
    pDVar3 = FUN_100125ba();
    *pDVar3 = 8;
    goto LAB_100195b5;
  }
  eVar4 = __dupenv_s(&local_18,(size_t *)0x0,(char *)local_14);
  if (eVar4 == 0) {
    sVar5 = _strlen((char *)local_14);
    local_34 = sVar5 + 2;
    if (local_18 != (char *)0x0) {
      sVar5 = _strlen(local_18);
      local_34 = local_34 + sVar5;
    }
    if (_Env == (char **)0x0) {
      *_EnvBlk = (char *)0x0;
LAB_1001932e:
      pcVar2 = *_ArgBlk;
      if (*_Argv != (char *)0x0) {
        eVar4 = _strcpy_s(pcVar2,_SizeInBytes,*_Argv);
        if (eVar4 == 0) {
          sVar5 = _strlen(*_Argv);
          pcVar2 = pcVar2 + sVar5 + 1;
          goto LAB_10019497;
        }
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      pcVar2 = pcVar2 + 1;
      for (; *_Argv != (char *)0x0; _Argv = _Argv + 1) {
        eVar4 = _strcpy_s(pcVar2,(rsize_t)(*_ArgBlk + (_SizeInBytes - (int)pcVar2)),*_Argv);
        if (eVar4 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
        sVar5 = _strlen(*_Argv);
        pcVar2[sVar5] = ' ';
        pcVar2 = pcVar2 + sVar5 + 1;
LAB_10019497:
      }
      pcVar2[-1] = '\0';
      *pcVar2 = '\0';
      pcVar2 = *_EnvBlk;
      if (_Env != (char **)0x0) {
        _memcpy(pcVar2,DAT_100290c0 + (int)local_20,(int)local_2c - (int)local_20);
        pcVar2 = pcVar2 + ((int)local_2c - (int)local_20);
        for (; *_Env != (char *)0x0; _Env = _Env + 1) {
          eVar4 = _strcpy_s(pcVar2,(rsize_t)(*_EnvBlk + ((int)local_44 - (int)pcVar2)),*_Env);
          if (eVar4 != 0) {
                    /* WARNING: Subroutine does not return */
            __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
          }
          sVar5 = _strlen(*_Env);
          pcVar2 = pcVar2 + sVar5 + 1;
        }
        if (!bVar1) {
          eVar4 = _strcpy_s(pcVar2,local_34,(char *)local_14);
          if (eVar4 != 0) {
                    /* WARNING: Subroutine does not return */
            __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
          }
          eVar4 = _strcat_s(pcVar2,local_34,"=");
          if (eVar4 != 0) {
                    /* WARNING: Subroutine does not return */
            __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
          }
          if ((local_18 != (char *)0x0) && (eVar4 = _strcat_s(pcVar2,local_34,local_18), eVar4 != 0)
             ) {
                    /* WARNING: Subroutine does not return */
            __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
          }
          pcVar2 = pcVar2 + local_34;
        }
      }
      if (pcVar2 != (char *)0x0) {
        if (pcVar2 == *_EnvBlk) {
          *pcVar2 = '\0';
          pcVar2 = pcVar2 + 1;
        }
        *pcVar2 = '\0';
      }
    }
    else {
      iVar6 = 2;
      pcVar2 = *_Env;
      ppcVar9 = _Env;
      while (pcVar2 != (char *)0x0) {
        sVar5 = _strlen(pcVar2);
        ppcVar9 = ppcVar9 + 1;
        iVar6 = iVar6 + 1 + sVar5;
        pcVar2 = *ppcVar9;
      }
      if ((DAT_100290c0 == (char *)0x0) &&
         (DAT_100290c0 = (char *)___crtGetEnvironmentStringsA(), DAT_100290c0 == (char *)0x0))
      goto LAB_10019311;
      local_20 = (char *)0x0;
      if (*DAT_100290c0 != '\0') {
        cVar7 = *DAT_100290c0;
        pcVar2 = DAT_100290c0;
        do {
          if (cVar7 == '=') break;
          sVar5 = _strlen(pcVar2);
          local_20 = local_20 + sVar5 + 1;
          pcVar2 = DAT_100290c0 + (int)local_20;
          cVar7 = *pcVar2;
        } while (cVar7 != '\0');
      }
      local_2c = local_20;
      pcVar2 = DAT_100290c0;
      pcVar8 = local_20;
      while ((((pcVar2 = pcVar2 + (int)pcVar8, *pcVar2 == '=' && (pcVar2[1] != '\0')) &&
              (pcVar2[2] == ':')) && (pcVar2[3] == '='))) {
        sVar5 = _strlen(pcVar2 + 4);
        pcVar2 = local_2c + sVar5 + 5;
        pcVar8 = DAT_100290c0;
        local_2c = pcVar2;
      }
      local_44 = local_2c + (iVar6 - (int)local_20);
      for (ppcVar9 = _Env; *ppcVar9 != (char *)0x0; ppcVar9 = ppcVar9 + 1) {
        sVar5 = _strlen((char *)local_14);
        iVar6 = __mbsnicmp((uchar *)*ppcVar9,local_14,sVar5);
        if (iVar6 == 0) {
          bVar1 = true;
          pcVar2 = local_44;
          goto LAB_100193e2;
        }
      }
      pcVar2 = local_44 + local_34;
LAB_100193e2:
      pcVar2 = (char *)FUN_10005780((int)pcVar2,1);
      *_EnvBlk = pcVar2;
      if (pcVar2 != (char *)0x0) goto LAB_1001932e;
      FUN_10005520(*_ArgBlk);
      *_ArgBlk = (char *)0x0;
      pDVar3 = FUN_100125a7();
      *pDVar3 = 0xc;
      pDVar3 = FUN_100125ba();
      *pDVar3 = 8;
    }
  }
  else {
    if (eVar4 == 0x16) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
LAB_10019311:
    FUN_10005520(*_ArgBlk);
    *_ArgBlk = (char *)0x0;
    *_EnvBlk = (char *)0x0;
  }
  if (DAT_100290c0 != (char *)0x0) {
    FUN_10005520(DAT_100290c0);
  }
  DAT_100290c0 = (char *)0x0;
  if (local_18 != (char *)0x0) {
    FUN_10005520(local_18);
  }
LAB_100195b5:
  iVar6 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar6;
}


// 100195c4 _strcat_s

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
        pDVar2 = FUN_100125a7();
        eVar4 = 0x22;
        *pDVar2 = 0x22;
        goto LAB_100195e3;
      }
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_100125a7();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_100195e3:
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 10019635 __mbsrchr_l

/* Library Function - Single Match
    __mbsrchr_l
   
   Library: Visual Studio 2005 Release */

uchar * __cdecl __mbsrchr_l(uchar *_Str,uint _Ch,_locale_t _Locale)

{
  byte bVar1;
  ushort uVar2;
  DWORD *pDVar3;
  byte *pbVar4;
  byte bVar5;
  bool bVar6;
  _LocaleUpdate local_18 [4];
  int local_14;
  int local_10;
  char local_c;
  byte *local_8;
  
  local_8 = (byte *)0x0;
  _LocaleUpdate::_LocaleUpdate(local_18,_Locale);
  if (*(int *)(local_14 + 8) == 0) {
    local_8 = (byte *)_strrchr((char *)_Str,_Ch);
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
    }
  }
  else if (_Str == (uchar *)0x0) {
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
    }
    local_8 = (byte *)0x0;
  }
  else {
    do {
      bVar5 = *_Str;
      if ((*(byte *)(bVar5 + 0x1d + local_14) & 4) == 0) {
        bVar6 = _Ch == bVar5;
LAB_100196d7:
        pbVar4 = _Str;
        if (bVar6) {
          local_8 = _Str;
        }
      }
      else {
        pbVar4 = _Str + 1;
        bVar1 = *pbVar4;
        if (bVar1 == 0) {
          bVar6 = local_8 == (byte *)0x0;
          _Str = pbVar4;
          bVar5 = bVar1;
          goto LAB_100196d7;
        }
        uVar2 = CONCAT11(bVar5,bVar1);
        bVar5 = bVar1;
        if (_Ch == uVar2) {
          local_8 = _Str;
        }
      }
      _Str = pbVar4 + 1;
    } while (bVar5 != 0);
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
    }
  }
  return local_8;
}


// 100196f5 __mbsrchr

/* Library Function - Single Match
    __mbsrchr
   
   Library: Visual Studio 2005 Release */

uchar * __cdecl __mbsrchr(uchar *_Str,uint _Ch)

{
  uchar *puVar1;
  
  puVar1 = __mbsrchr_l(_Str,_Ch,(_locale_t)0x0);
  return puVar1;
}


// 10019708 FUN_10019708

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

undefined4 FUN_10019708(void)

{
  return 1;
}


// 10019758 __get_sse2_info

/* WARNING: Removing unreachable block (ram,0x10019793) */
/* WARNING: Removing unreachable block (ram,0x10019780) */
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
  if (((local_8 & 0x4000000) == 0) || (iVar2 = FUN_10019708(), iVar2 == 0)) {
    uVar3 = 0;
  }
  else {
    uVar3 = 1;
  }
  return uVar3;
}


// 100197c5 __lseeki64_nolock

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
    pDVar2 = FUN_100125a7();
    *pDVar2 = 9;
LAB_100197f4:
    DVar3 = 0xffffffff;
    local_8 = -1;
  }
  else {
    DVar3 = SetFilePointer(hFile,in_stack_00000008,&local_8,_Offset._4_4_);
    if (DVar3 == 0xffffffff) {
      DVar4 = GetLastError();
      if (DVar4 != 0) {
        FUN_100125cd(DVar4);
        goto LAB_100197f4;
      }
    }
    pbVar1 = (byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38);
    *pbVar1 = *pbVar1 & 0xfd;
  }
  return CONCAT44(local_8,DVar3);
}


// 10019848 __lseeki64

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
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10029b38)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
        pDVar1 = FUN_100125ba();
        *pDVar1 = 0;
        pDVar1 = FUN_100125a7();
        *pDVar1 = 9;
        FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        local_28._4_4_ = 0xffffffff;
        local_28._0_4_ = 0xffffffff;
      }
      else {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_100125a7();
          *pDVar1 = 9;
          pDVar1 = FUN_100125ba();
          *pDVar1 = 0;
          local_28 = -1;
        }
        else {
          local_28 = __lseeki64_nolock(_FileHandle,_Offset,in_stack_ffffffc8);
        }
        FUN_10019957();
      }
      goto LAB_10019951;
    }
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  local_28._0_4_ = 0xffffffff;
  local_28._4_4_ = 0xffffffff;
LAB_10019951:
  return CONCAT44(local_28._4_4_,(undefined4)local_28);
}


// 10019957 FUN_10019957

void FUN_10019957(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 10019961 __write_nolock

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
  
  local_8 = DAT_10025040 ^ (uint)local_51c;
  local_580 = (WCHAR *)_Buf;
  local_57c = (char *)0x0;
  local_584 = 0;
  if (_MaxCharCount == 0) goto LAB_10019f11;
  if (_Buf == (void *)0x0) {
    pDVar6 = FUN_100125ba();
    *pDVar6 = 0;
    pDVar6 = FUN_100125a7();
    *pDVar6 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_10019f11;
  }
  iVar11 = (_FileHandle & 0x1fU) * 0x38;
  piVar16 = &DAT_10029b40 + (_FileHandle >> 5);
  local_571 = (char)(*(char *)(*piVar16 + iVar11 + 0x24) * '\x02') >> 1;
  local_58c = piVar16;
  if (((local_571 == '\x02') || (local_571 == '\x01')) && ((~_MaxCharCount & 1) == 0)) {
    pDVar6 = FUN_100125ba();
    *pDVar6 = 0;
    pDVar6 = FUN_100125a7();
    *pDVar6 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_10019f11;
  }
  if ((*(byte *)(*piVar16 + iVar11 + 4) & 0x20) != 0) {
    __lseeki64_nolock(_FileHandle,0x200000000,unaff_EDI);
  }
  iVar7 = __isatty(_FileHandle);
  if ((iVar7 == 0) || ((*(byte *)(iVar11 + 4 + *piVar16) & 0x80) == 0)) {
LAB_10019c4c:
    if ((*(byte *)((undefined4 *)(*piVar16 + iVar11) + 1) & 0x80) == 0) {
      BVar9 = WriteFile(*(HANDLE *)(*piVar16 + iVar11),local_580,_MaxCharCount,(LPDWORD)&local_588,
                        (LPOVERLAPPED)0x0);
      if (BVar9 == 0) {
LAB_10019ea6:
        local_570 = GetLastError();
      }
      else {
        local_570 = 0;
        local_57c = local_588;
      }
LAB_10019eaf:
      piVar16 = local_58c;
      if (local_57c != (char *)0x0) goto LAB_10019f11;
      goto LAB_10019eb9;
    }
    local_570 = 0;
    if (local_571 == '\0') {
      local_56c = local_580;
      if (_MaxCharCount == 0) goto LAB_10019ee0;
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
        if (BVar9 == 0) goto LAB_10019ea6;
        local_57c = local_57c + (int)local_588;
      } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
              ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
      goto LAB_10019eaf;
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
          if (BVar9 == 0) goto LAB_10019ea6;
          local_57c = local_57c + (int)local_588;
        } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
                ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
        goto LAB_10019eaf;
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
          if (iVar7 == 0) goto LAB_10019ea6;
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
        goto LAB_10019eaf;
      }
    }
LAB_10019ee0:
    DVar10 = 0;
    if (((*(byte *)(iVar11 + 4 + *piVar16) & 0x40) != 0) && ((char)*local_580 == '\x1a'))
    goto LAB_10019f11;
    pDVar6 = FUN_100125a7();
    *pDVar6 = 0x1c;
  }
  else {
    p_Var8 = __getptd();
    pwVar3 = p_Var8->ptlocinfo->lc_category[0].wlocale;
    BVar9 = GetConsoleMode(*(HANDLE *)(iVar11 + *piVar16),&local_598);
    if ((BVar9 == 0) || ((pwVar3 == (wchar_t *)0x0 && (local_571 == '\0')))) goto LAB_10019c4c;
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
          if (BVar9 == 0) goto LAB_10019ea6;
          local_57c = local_57c + (int)local_56c;
          if ((int)local_56c < (int)DVar10) break;
          if (local_594 != 0) {
            local_10[0] = '\r';
            BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_10,1,(LPDWORD)&local_56c,
                              (LPOVERLAPPED)0x0);
            if (BVar9 == 0) goto LAB_10019ea6;
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
            if (wVar5 != (wchar_t)local_570) goto LAB_10019ea6;
            local_57c = local_57c + 1;
            if (local_594 != 0) {
              local_570 = 0xd;
              wVar5 = __putwch_nolock(L'\r');
              if (wVar5 != (wchar_t)local_570) goto LAB_10019ea6;
              local_57c = local_57c + 1;
              local_584 = local_584 + 1;
            }
          }
        }
      } while (local_578 < _MaxCharCount);
      goto LAB_10019eaf;
    }
LAB_10019eb9:
    if (local_570 == 0) goto LAB_10019ee0;
    DVar10 = 5;
    if (local_570 != 5) {
      FUN_100125cd(local_570);
      goto LAB_10019f11;
    }
    pDVar6 = FUN_100125a7();
    *pDVar6 = 9;
  }
  pDVar6 = FUN_100125ba();
  *pDVar6 = DVar10;
LAB_10019f11:
  iVar11 = __security_check_cookie(local_8 ^ (uint)local_51c);
  return iVar11;
}


// 10019f27 __write

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
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10029b38)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_100125a7();
          *pDVar1 = 9;
          pDVar1 = FUN_100125ba();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = __write_nolock(_FileHandle,_Buf,_MaxCharCount);
        }
        FUN_10019ff9();
        return local_20;
      }
    }
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 10019ff9 FUN_10019ff9

void FUN_10019ff9(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 1001a003 __getbuf

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __getbuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __getbuf(FILE *_File)

{
  char *pcVar1;
  
  _DAT_100290b8 = _DAT_100290b8 + 1;
  pcVar1 = (char *)FUN_10005720(0x1000);
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


// 1001a047 __isatty

/* Library Function - Single Match
    __isatty
   
   Library: Visual Studio 2005 Release */

int __cdecl __isatty(int _FileHandle)

{
  DWORD *pDVar1;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
    return 0;
  }
  if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10029b38)) {
    return *(byte *)((&DAT_10029b40)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38) & 0x40;
  }
  pDVar1 = FUN_100125a7();
  *pDVar1 = 9;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0;
}


// 1001a0a5 __wctomb_s_l

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
LAB_1001a0c7:
    DVar1 = 0;
  }
  else {
    if (_SizeConverted != (int *)0x0) {
      *_SizeConverted = -1;
    }
    if (0x7fffffff < _SizeInBytes) {
      pDVar2 = FUN_100125a7();
      *pDVar2 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return 0x16;
    }
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
    if (*(int *)(local_14[0] + 0x14) == 0) {
      if ((ushort)_WCh < 0x100) {
        if (lpMultiByteStr != (char *)0x0) {
          if (_Size == 0) goto LAB_1001a157;
          *lpMultiByteStr = (char)_WCh;
        }
        if (_SizeConverted != (int *)0x0) {
          *_SizeConverted = 1;
        }
LAB_1001a18e:
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        goto LAB_1001a0c7;
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
LAB_1001a157:
          pDVar2 = FUN_100125a7();
          *pDVar2 = 0x22;
          FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_1001a18e;
      }
    }
    pDVar2 = FUN_100125a7();
    *pDVar2 = 0x2a;
    pDVar2 = FUN_100125a7();
    DVar1 = *pDVar2;
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return DVar1;
}


// 1001a204 _wctomb_s

/* Library Function - Single Match
    _wctomb_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _wctomb_s(int *_SizeConverted,char *_MbCh,rsize_t _SizeInBytes,wchar_t _WCh)

{
  errno_t eVar1;
  
  eVar1 = __wctomb_s_l(_SizeConverted,_MbCh,_SizeInBytes,_WCh,(_locale_t)0x0);
  return eVar1;
}


// 1001a220 __aulldvrm

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


// 1001a2b5 __set_osfhnd

/* Library Function - Single Match
    __set_osfhnd
   
   Library: Visual Studio 2005 Release */

int __cdecl __set_osfhnd(int param_1,intptr_t param_2)

{
  DWORD *pDVar1;
  int iVar2;
  DWORD nStdHandle;
  
  if ((-1 < param_1) && ((uint)param_1 < DAT_10029b38)) {
    iVar2 = (param_1 & 0x1fU) * 0x38;
    if (*(int *)(iVar2 + (&DAT_10029b40)[param_1 >> 5]) == -1) {
      if (DAT_100290cc == 1) {
        if (param_1 == 0) {
          nStdHandle = 0xfffffff6;
        }
        else if (param_1 == 1) {
          nStdHandle = 0xfffffff5;
        }
        else {
          if (param_1 != 2) goto LAB_1001a30f;
          nStdHandle = 0xfffffff4;
        }
        SetStdHandle(nStdHandle,(HANDLE)param_2);
      }
LAB_1001a30f:
      *(intptr_t *)(iVar2 + (&DAT_10029b40)[param_1 >> 5]) = param_2;
      return 0;
    }
  }
  pDVar1 = FUN_100125a7();
  *pDVar1 = 9;
  pDVar1 = FUN_100125ba();
  *pDVar1 = 0;
  return -1;
}


// 1001a332 __free_osfhnd

/* Library Function - Single Match
    __free_osfhnd
   
   Library: Visual Studio 2005 Release */

int __cdecl __free_osfhnd(int param_1)

{
  int *piVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD nStdHandle;
  
  if ((-1 < param_1) && ((uint)param_1 < DAT_10029b38)) {
    iVar3 = (param_1 & 0x1fU) * 0x38;
    piVar1 = (int *)((&DAT_10029b40)[param_1 >> 5] + iVar3);
    if (((*(byte *)(piVar1 + 1) & 1) != 0) && (*piVar1 != -1)) {
      if (DAT_100290cc == 1) {
        if (param_1 == 0) {
          nStdHandle = 0xfffffff6;
        }
        else if (param_1 == 1) {
          nStdHandle = 0xfffffff5;
        }
        else {
          if (param_1 != 2) goto LAB_1001a390;
          nStdHandle = 0xfffffff4;
        }
        SetStdHandle(nStdHandle,(HANDLE)0x0);
      }
LAB_1001a390:
      *(undefined4 *)(iVar3 + (&DAT_10029b40)[param_1 >> 5]) = 0xffffffff;
      return 0;
    }
  }
  pDVar2 = FUN_100125a7();
  *pDVar2 = 9;
  pDVar2 = FUN_100125ba();
  *pDVar2 = 0;
  return -1;
}


// 1001a3b3 __get_osfhandle

/* Library Function - Single Match
    __get_osfhandle
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __get_osfhandle(int _FileHandle)

{
  DWORD *pDVar1;
  intptr_t *piVar2;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_100125ba();
    *pDVar1 = 0;
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
    return -1;
  }
  if (((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10029b38)) &&
     (piVar2 = (intptr_t *)((_FileHandle & 0x1fU) * 0x38 + (&DAT_10029b40)[_FileHandle >> 5]),
     (*(byte *)(piVar2 + 1) & 1) != 0)) {
    return *piVar2;
  }
  pDVar1 = FUN_100125ba();
  *pDVar1 = 0;
  pDVar1 = FUN_100125a7();
  *pDVar1 = 9;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1001a424 ___lock_fhandle

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
  
  iVar2 = (_Filehandle & 0x1fU) * 0x38 + (&DAT_10029b40)[_Filehandle >> 5];
  local_20 = 1;
  if (*(int *)(iVar2 + 8) == 0) {
    __lock(10);
    if (*(int *)(iVar2 + 8) == 0) {
      iVar1 = ___crtInitCritSecAndSpinCount(iVar2 + 0xc,4000);
      local_20 = (uint)(iVar1 != 0);
      *(int *)(iVar2 + 8) = *(int *)(iVar2 + 8) + 1;
    }
    FUN_1001a4bb();
  }
  if (local_20 != 0) {
    EnterCriticalSection
              ((LPCRITICAL_SECTION)
               ((&DAT_10029b40)[_Filehandle >> 5] + 0xc + (_Filehandle & 0x1fU) * 0x38));
  }
  return local_20;
}


// 1001a4bb FUN_1001a4bb

void FUN_1001a4bb(void)

{
  FUN_10012689(10);
  return;
}


// 1001a4c4 __unlock_fhandle

/* Library Function - Single Match
    __unlock_fhandle
   
   Library: Visual Studio 2005 Release */

void __cdecl __unlock_fhandle(int _Filehandle)

{
  LeaveCriticalSection
            ((LPCRITICAL_SECTION)
             ((&DAT_10029b40)[_Filehandle >> 5] + 0xc + (_Filehandle & 0x1fU) * 0x38));
  return;
}


// 1001a4e6 __alloc_osfhnd

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __alloc_osfhnd
   
   Library: Visual Studio 2005 Release */

int __cdecl __alloc_osfhnd(void)

{
  bool bVar1;
  int iVar2;
  undefined4 *puVar3;
  int iVar4;
  int local_20;
  
  local_20 = -1;
  iVar4 = 0;
  bVar1 = false;
  iVar2 = __mtinitlocknum(0xb);
  if (iVar2 == 0) {
    local_20 = -1;
  }
  else {
    __lock(0xb);
    for (; iVar4 < 0x40; iVar4 = iVar4 + 1) {
      puVar3 = (undefined4 *)(&DAT_10029b40)[iVar4];
      if (puVar3 == (undefined4 *)0x0) {
        puVar3 = (undefined4 *)FUN_10005780(0x20,0x38);
        if (puVar3 != (undefined4 *)0x0) {
          (&DAT_10029b40)[iVar4] = puVar3;
          DAT_10029b38 = DAT_10029b38 + 0x20;
          for (; puVar3 < (undefined4 *)((&DAT_10029b40)[iVar4] + 0x700); puVar3 = puVar3 + 0xe) {
            *(undefined1 *)(puVar3 + 1) = 0;
            *puVar3 = 0xffffffff;
            *(undefined1 *)((int)puVar3 + 5) = 10;
            puVar3[2] = 0;
          }
          local_20 = iVar4 << 5;
          *(undefined1 *)((&DAT_10029b40)[local_20 >> 5] + 4) = 1;
          iVar2 = ___lock_fhandle(local_20);
          if (iVar2 == 0) {
            local_20 = -1;
          }
        }
        break;
      }
      for (; puVar3 < (undefined4 *)((&DAT_10029b40)[iVar4] + 0x700); puVar3 = puVar3 + 0xe) {
        if ((*(byte *)(puVar3 + 1) & 1) == 0) {
          if (puVar3[2] == 0) {
            __lock(10);
            if (puVar3[2] == 0) {
              iVar2 = ___crtInitCritSecAndSpinCount(puVar3 + 3,4000);
              if (iVar2 == 0) {
                bVar1 = true;
              }
              else {
                puVar3[2] = puVar3[2] + 1;
              }
            }
            FUN_1001a5b9();
          }
          if (!bVar1) {
            EnterCriticalSection((LPCRITICAL_SECTION)(puVar3 + 3));
            if ((*(byte *)(puVar3 + 1) & 1) == 0) {
              *(undefined1 *)(puVar3 + 1) = 1;
              *puVar3 = 0xffffffff;
              local_20 = ((int)puVar3 - (&DAT_10029b40)[iVar4]) / 0x38 + iVar4 * 0x20;
              break;
            }
            LeaveCriticalSection((LPCRITICAL_SECTION)(puVar3 + 3));
          }
        }
      }
      if (local_20 != -1) break;
    }
    FUN_1001a67c();
  }
  return local_20;
}


// 1001a5b9 FUN_1001a5b9

void FUN_1001a5b9(void)

{
  FUN_10012689(10);
  return;
}


// 1001a67c FUN_1001a67c

void FUN_1001a67c(void)

{
  FUN_10012689(0xb);
  return;
}


// 1001a685 __commit

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __commit
   
   Library: Visual Studio 2005 Release */

int __cdecl __commit(int _FileHandle)

{
  DWORD *pDVar1;
  HANDLE hFile;
  BOOL BVar2;
  int iVar3;
  DWORD local_20;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_10029b38)) {
      iVar3 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)(iVar3 + 4 + (&DAT_10029b40)[_FileHandle >> 5]) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)(iVar3 + 4 + (&DAT_10029b40)[_FileHandle >> 5]) & 1) != 0) {
          hFile = (HANDLE)__get_osfhandle(_FileHandle);
          BVar2 = FlushFileBuffers(hFile);
          if (BVar2 == 0) {
            local_20 = GetLastError();
          }
          else {
            local_20 = 0;
          }
          if (local_20 == 0) goto LAB_1001a747;
          pDVar1 = FUN_100125ba();
          *pDVar1 = local_20;
        }
        pDVar1 = FUN_100125a7();
        *pDVar1 = 9;
        local_20 = 0xffffffff;
LAB_1001a747:
        FUN_1001a75c();
        return local_20;
      }
    }
    pDVar1 = FUN_100125a7();
    *pDVar1 = 9;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 1001a75c FUN_1001a75c

void FUN_1001a75c(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 1001a766 FUN_1001a766

DWORD __cdecl FUN_1001a766(undefined4 *param_1,LPCSTR param_2,uint param_3,int param_4,byte param_5)

{
  byte *pbVar1;
  byte bVar2;
  uint *in_EAX;
  int iVar3;
  uint uVar4;
  DWORD *pDVar5;
  DWORD DVar6;
  HANDLE pvVar7;
  long lVar8;
  int iVar9;
  byte bVar10;
  int unaff_EDI;
  bool bVar11;
  longlong lVar12;
  _SECURITY_ATTRIBUTES local_38;
  int local_2c;
  uint local_28 [2];
  HANDLE local_20;
  uint local_1c;
  DWORD local_18;
  uint local_14;
  uint local_10;
  uint local_c;
  char local_8;
  byte local_7;
  byte local_6;
  byte local_5;
  
  bVar11 = (param_3 & 0x80) == 0;
  local_28[0] = 0;
  local_2c = 0;
  local_6 = 0;
  local_38.nLength = 0xc;
  local_38.lpSecurityDescriptor = (LPVOID)0x0;
  if (bVar11) {
    local_5 = 0;
  }
  else {
    local_5 = 0x10;
  }
  local_38.bInheritHandle = (BOOL)bVar11;
  iVar3 = FUN_1001e958(local_28);
  if (iVar3 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  iVar3 = __get_osplatform(&local_2c);
  if (iVar3 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  if (((param_3 & 0x8000) == 0) && (((param_3 & 0x74000) != 0 || (local_28[0] != 0x8000)))) {
    local_5 = local_5 | 0x80;
  }
  uVar4 = param_3 & 3;
  if (uVar4 == 0) {
    local_c = 0x80000000;
  }
  else {
    if (uVar4 == 1) {
      if (((param_3 & 8) == 0) || ((param_3 & 0x70000) == 0)) {
        local_c = 0x40000000;
        goto LAB_1001a84f;
      }
    }
    else if (uVar4 != 2) goto LAB_1001a80b;
    local_c = 0xc0000000;
  }
LAB_1001a84f:
  if (param_4 == 0x10) {
    local_14 = 0;
  }
  else if (param_4 == 0x20) {
    local_14 = 1;
  }
  else if (param_4 == 0x30) {
    local_14 = 2;
  }
  else if (param_4 == 0x40) {
    local_14 = 3;
  }
  else {
    if (param_4 != 0x80) goto LAB_1001a80b;
    local_14 = (uint)(local_c == 0x80000000);
  }
  uVar4 = param_3 & 0x700;
  if (uVar4 < 0x401) {
    if ((uVar4 == 0x400) || (uVar4 == 0)) {
      local_18 = 3;
    }
    else if (uVar4 == 0x100) {
      local_18 = 4;
    }
    else {
      if (uVar4 == 0x200) goto LAB_1001a959;
      if (uVar4 != 0x300) goto LAB_1001a80b;
      local_18 = 2;
    }
  }
  else {
    if (uVar4 != 0x500) {
      if (uVar4 == 0x600) {
LAB_1001a959:
        local_18 = 5;
        goto LAB_1001a903;
      }
      if (uVar4 != 0x700) {
LAB_1001a80b:
        pDVar5 = FUN_100125ba();
        *pDVar5 = 0;
        *in_EAX = 0xffffffff;
        pDVar5 = FUN_100125a7();
        *pDVar5 = 0x16;
        FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return 0x16;
      }
    }
    local_18 = 1;
  }
LAB_1001a903:
  local_10 = 0x80;
  if (((param_3 & 0x100) != 0) && (-1 < (char)(~(byte)DAT_10029070 & param_5))) {
    local_10 = 1;
  }
  if ((param_3 & 0x40) != 0) {
    local_10 = local_10 | 0x4000000;
    local_c = local_c | 0x10000;
    if (local_2c == 2) {
      local_14 = local_14 | 4;
    }
  }
  if ((param_3 & 0x1000) != 0) {
    local_10 = local_10 | 0x100;
  }
  if ((param_3 & 0x20) == 0) {
    if ((param_3 & 0x10) != 0) {
      local_10 = local_10 | 0x10000000;
    }
  }
  else {
    local_10 = local_10 | 0x8000000;
  }
  uVar4 = __alloc_osfhnd();
  *in_EAX = uVar4;
  if (uVar4 == 0xffffffff) {
    pDVar5 = FUN_100125ba();
    *pDVar5 = 0;
    *in_EAX = 0xffffffff;
    pDVar5 = FUN_100125a7();
    *pDVar5 = 0x18;
    goto LAB_1001aa21;
  }
  *param_1 = 1;
  local_20 = CreateFileA(param_2,local_c,local_14,&local_38,local_18,local_10,(HANDLE)0x0);
  if (local_20 == (HANDLE)0xffffffff) {
    if (((local_c & 0xc0000000) == 0xc0000000) && ((param_3 & 1) != 0)) {
      local_c = local_c & 0x7fffffff;
      local_20 = CreateFileA(param_2,local_c,local_14,&local_38,local_18,local_10,(HANDLE)0x0);
      if (local_20 != (HANDLE)0xffffffff) goto LAB_1001aa2d;
    }
    pbVar1 = (byte *)((&DAT_10029b40)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38);
    *pbVar1 = *pbVar1 & 0xfe;
    DVar6 = GetLastError();
    FUN_100125cd(DVar6);
    goto LAB_1001aa21;
  }
LAB_1001aa2d:
  DVar6 = GetFileType(local_20);
  if (DVar6 == 0) {
    pbVar1 = (byte *)((&DAT_10029b40)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38);
    *pbVar1 = *pbVar1 & 0xfe;
    DVar6 = GetLastError();
    FUN_100125cd(DVar6);
    CloseHandle(local_20);
    if (DVar6 == 0) {
      pDVar5 = FUN_100125a7();
      *pDVar5 = 0xd;
    }
    goto LAB_1001aa21;
  }
  if (DVar6 == 2) {
    local_5 = local_5 | 0x40;
  }
  else if (DVar6 == 3) {
    local_5 = local_5 | 8;
  }
  __set_osfhnd(*in_EAX,(intptr_t)local_20);
  bVar10 = local_5 | 1;
  *(byte *)((&DAT_10029b40)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38) = bVar10;
  pbVar1 = (byte *)((&DAT_10029b40)[(int)*in_EAX >> 5] + 0x24 + (*in_EAX & 0x1f) * 0x38);
  *pbVar1 = *pbVar1 & 0x80;
  local_7 = local_5 & 0x48;
  if (local_7 == 0) {
    bVar2 = local_5 & 0x80;
    local_5 = bVar10;
    if (bVar2 == 0) goto LAB_1001ac37;
    if ((param_3 & 2) == 0) goto LAB_1001ab67;
    local_1c = __lseek_nolock(*in_EAX,-1,2);
    if (local_1c == 0xffffffff) {
      pDVar5 = FUN_100125ba();
      bVar10 = local_5;
      if (*pDVar5 == 0x83) goto LAB_1001ab67;
    }
    else {
      local_8 = '\0';
      iVar3 = FUN_10013fef(*in_EAX,(LPWSTR)&local_8,(LPWSTR)0x1);
      if ((((iVar3 != 0) || (local_8 != '\x1a')) ||
          (iVar3 = __chsize_nolock(*in_EAX,CONCAT44(unaff_EDI,(int)local_1c >> 0x1f)), iVar3 != -1))
         && (lVar8 = __lseek_nolock(*in_EAX,0,0), bVar10 = local_5, lVar8 != -1)) goto LAB_1001ab67;
    }
LAB_1001ab1a:
    __close_nolock(*in_EAX);
    goto LAB_1001aa21;
  }
LAB_1001ab67:
  local_5 = bVar10;
  if ((local_5 & 0x80) != 0) {
    if ((param_3 & 0x74000) == 0) {
      if ((local_28[0] & 0x74000) == 0) {
        param_3 = param_3 | 0x4000;
      }
      else {
        param_3 = param_3 | local_28[0] & 0x74000;
      }
    }
    uVar4 = param_3 & 0x74000;
    if (uVar4 == 0x4000) {
      local_6 = 0;
    }
    else if ((uVar4 == 0x10000) || (uVar4 == 0x14000)) {
      if ((param_3 & 0x301) == 0x301) goto LAB_1001abd6;
    }
    else if ((uVar4 == 0x20000) || (uVar4 == 0x24000)) {
LAB_1001abd6:
      local_6 = 2;
    }
    else if ((uVar4 == 0x40000) || (uVar4 == 0x44000)) {
      local_6 = 1;
    }
    if (((param_3 & 0x70000) != 0) && (local_1c = 0, (local_5 & 0x40) == 0)) {
      uVar4 = local_c & 0xc0000000;
      if (uVar4 == 0x40000000) {
        if (local_18 == 0) goto LAB_1001ac37;
        if (2 < local_18) {
          if (local_18 < 5) {
            lVar12 = __lseeki64_nolock(*in_EAX,0x200000000,unaff_EDI);
            if (lVar12 == 0) goto LAB_1001ae39;
            lVar12 = __lseeki64_nolock(*in_EAX,0,unaff_EDI);
            uVar4 = (uint)lVar12 & (uint)((ulonglong)lVar12 >> 0x20);
            goto LAB_1001adcf;
          }
LAB_1001ac2e:
          if (local_18 != 5) goto LAB_1001ac37;
        }
LAB_1001ae39:
        iVar3 = 0;
        if (local_6 == 1) {
          local_1c = 0xbfbbef;
          local_18 = 3;
        }
        else {
          if (local_6 != 2) goto LAB_1001ac37;
          local_1c = 0xfeff;
          local_18 = 2;
        }
        do {
          iVar9 = __write(*in_EAX,(void *)((int)&local_1c + iVar3),local_18 - iVar3);
          if (iVar9 == -1) {
            __close_nolock(*in_EAX);
            pDVar5 = FUN_100125a7();
            return *pDVar5;
          }
          iVar3 = iVar3 + iVar9;
        } while (iVar3 < (int)local_18);
      }
      else {
        if (uVar4 != 0x80000000) {
          if ((uVar4 == 0xc0000000) && (local_18 != 0)) {
            if (2 < local_18) {
              if (4 < local_18) goto LAB_1001ac2e;
              lVar12 = __lseeki64_nolock(*in_EAX,0x200000000,unaff_EDI);
              if (lVar12 != 0) {
                lVar12 = __lseeki64_nolock(*in_EAX,0,unaff_EDI);
                if (lVar12 == -1) goto LAB_1001ab1a;
                goto LAB_1001ad58;
              }
            }
            goto LAB_1001ae39;
          }
          goto LAB_1001ac37;
        }
LAB_1001ad58:
        iVar3 = FUN_10013fef(*in_EAX,(LPWSTR)&local_1c,(LPWSTR)0x3);
        if (iVar3 == -1) goto LAB_1001ab1a;
        if (iVar3 == 2) {
LAB_1001addd:
          if ((local_1c & 0xffff) == 0xfffe) {
            __close_nolock(*in_EAX);
            pDVar5 = FUN_100125a7();
            *pDVar5 = 0x16;
            return 0x16;
          }
          if ((local_1c & 0xffff) == 0xfeff) {
            lVar8 = __lseek_nolock(*in_EAX,2,0);
            if (lVar8 == -1) goto LAB_1001ab1a;
            local_6 = 2;
            goto LAB_1001ac37;
          }
        }
        else if (iVar3 == 3) {
          if (local_1c == 0xbfbbef) {
            local_6 = 1;
            goto LAB_1001ac37;
          }
          goto LAB_1001addd;
        }
        uVar4 = __lseek_nolock(*in_EAX,0,0);
LAB_1001adcf:
        if (uVar4 == 0xffffffff) goto LAB_1001ab1a;
      }
    }
  }
LAB_1001ac37:
  uVar4 = local_c;
  pbVar1 = (byte *)((&DAT_10029b40)[(int)*in_EAX >> 5] + 0x24 + (*in_EAX & 0x1f) * 0x38);
  *pbVar1 = *pbVar1 ^ (*pbVar1 ^ local_6) & 0x7f;
  pbVar1 = (byte *)((&DAT_10029b40)[(int)*in_EAX >> 5] + 0x24 + (*in_EAX & 0x1f) * 0x38);
  *pbVar1 = (char)(param_3 >> 0x10) << 7 | *pbVar1 & 0x7f;
  if ((local_7 == 0) && ((param_3 & 8) != 0)) {
    pbVar1 = (byte *)((&DAT_10029b40)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38);
    *pbVar1 = *pbVar1 | 0x20;
  }
  if (((local_c & 0xc0000000) == 0xc0000000) && ((param_3 & 1) != 0)) {
    CloseHandle(local_20);
    pvVar7 = CreateFileA(param_2,uVar4 & 0x7fffffff,local_14,&local_38,3,local_10,(HANDLE)0x0);
    if (pvVar7 == (HANDLE)0xffffffff) {
      DVar6 = GetLastError();
      FUN_100125cd(DVar6);
      pbVar1 = (byte *)((&DAT_10029b40)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38);
      *pbVar1 = *pbVar1 & 0xfe;
      __free_osfhnd(*in_EAX);
LAB_1001aa21:
      pDVar5 = FUN_100125a7();
      return *pDVar5;
    }
    *(HANDLE *)((*in_EAX & 0x1f) * 0x38 + (&DAT_10029b40)[(int)*in_EAX >> 5]) = pvVar7;
  }
  return 0;
}


// 1001aebf FID_conflict:__sopen_helper

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Multiple Matches With Different Base Names
    __sopen_helper
    __wsopen_helper
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl
FID_conflict___sopen_helper
          (char *_Filename,int _OFlag,int _ShFlag,int _PMode,int *_PFileHandle,int _BSecure)

{
  DWORD *pDVar1;
  DWORD DVar2;
  undefined4 local_20 [5];
  undefined4 uStack_c;
  undefined *local_8;
  
  local_8 = &DAT_10023098;
  uStack_c = 0x1001aecb;
  local_20[0] = 0;
  if (((_PFileHandle == (int *)0x0) || (*_PFileHandle = -1, _Filename == (char *)0x0)) ||
     ((_BSecure != 0 && ((_PMode & 0xfffffe7fU) != 0)))) {
    pDVar1 = FUN_100125a7();
    DVar2 = 0x16;
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    local_8 = (undefined *)0x0;
    DVar2 = FUN_1001a766(local_20,_Filename,_OFlag,_ShFlag,(byte)_PMode);
    local_8 = (undefined *)0xfffffffe;
    FUN_1001af5d();
    if (DVar2 != 0) {
      *_PFileHandle = -1;
    }
  }
  return DVar2;
}


// 1001af5d FUN_1001af5d

void FUN_1001af5d(void)

{
  byte *pbVar1;
  int unaff_EBP;
  int unaff_ESI;
  uint *unaff_EDI;
  
  if (*(int *)(unaff_EBP + -0x1c) != unaff_ESI) {
    if (*(int *)(unaff_EBP + -0x20) != unaff_ESI) {
      pbVar1 = (byte *)((&DAT_10029b40)[(int)*unaff_EDI >> 5] + 4 + (*unaff_EDI & 0x1f) * 0x38);
      *pbVar1 = *pbVar1 & 0xfe;
    }
    __unlock_fhandle(*unaff_EDI);
  }
  return;
}


// 1001af8b FID_conflict:__sopen_s

/* Library Function - Multiple Matches With Different Base Names
    __sopen_s
    __wsopen_s
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

errno_t __cdecl
FID_conflict___sopen_s
          (int *_FileHandle,char *_Filename,int _OpenFlag,int _ShareFlag,int _PermissionMode)

{
  errno_t eVar1;
  
  eVar1 = FID_conflict___sopen_helper(_Filename,_OpenFlag,_ShareFlag,_PermissionMode,_FileHandle,1);
  return eVar1;
}


// 1001afa9 __mbsicmp_l

/* Library Function - Single Match
    __mbsicmp_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsicmp_l(uchar *_Str1,uchar *_Str2,_locale_t _Locale)

{
  byte bVar1;
  ushort uVar2;
  ushort uVar3;
  DWORD *pDVar4;
  int iVar5;
  pthreadmbcinfo ptVar6;
  byte *pbVar7;
  localeinfo_struct local_18;
  int local_10;
  char local_c;
  byte local_8;
  byte local_7;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_18,_Locale);
  if (_Str1 == (uchar *)0x0) {
    pDVar4 = FUN_100125a7();
    *pDVar4 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
    }
    iVar5 = 0x7fffffff;
  }
  else if (_Str2 == (uchar *)0x0) {
    pDVar4 = FUN_100125a7();
    *pDVar4 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
    }
    iVar5 = 0x7fffffff;
  }
  else {
    ptVar6 = local_18.mbcinfo;
    if ((local_18.mbcinfo)->ismbcodepage == 0) {
      iVar5 = __stricmp_l((char *)_Str1,(char *)_Str2,&local_18);
      if (local_c != '\0') {
        *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
      }
    }
    else {
      do {
        bVar1 = *_Str1;
        pbVar7 = _Str1 + 1;
        if ((ptVar6->mbctype[bVar1 + 5] & 4) == 0) {
          if ((ptVar6->mbctype[bVar1 + 5] & 0x10) == 0) {
            uVar2 = (ushort)bVar1;
            _Str1 = pbVar7;
          }
          else {
            uVar2 = (ushort)ptVar6->mbcasemap[bVar1 + 4];
            _Str1 = pbVar7;
          }
        }
        else if (*pbVar7 == 0) {
          uVar2 = 0;
          _Str1 = pbVar7;
        }
        else {
          iVar5 = ___crtLCMapStringA(&local_18,*(LPCWSTR *)ptVar6->mbulinfo,0x200,(LPCSTR)_Str1,2,
                                     (LPSTR)&local_8,2,ptVar6->mbcodepage,1);
          if (iVar5 == 1) {
            uVar2 = (ushort)local_8;
          }
          else {
            if (iVar5 != 2) goto LAB_1001b17b;
            uVar2 = (ushort)local_8 * 0x100 + (ushort)local_7;
          }
          ptVar6 = local_18.mbcinfo;
          _Str1 = _Str1 + 2;
        }
        bVar1 = *_Str2;
        pbVar7 = _Str2 + 1;
        if ((ptVar6->mbctype[bVar1 + 5] & 4) == 0) {
          if ((ptVar6->mbctype[bVar1 + 5] & 0x10) == 0) {
            uVar3 = (ushort)bVar1;
          }
          else {
            uVar3 = (ushort)ptVar6->mbcasemap[bVar1 + 4];
          }
        }
        else if (*pbVar7 == 0) {
          uVar3 = 0;
        }
        else {
          iVar5 = ___crtLCMapStringA(&local_18,*(LPCWSTR *)ptVar6->mbulinfo,0x200,(LPCSTR)_Str2,2,
                                     (LPSTR)&local_8,2,ptVar6->mbcodepage,1);
          if (iVar5 == 1) {
            uVar3 = (ushort)local_8;
          }
          else {
            if (iVar5 != 2) {
LAB_1001b17b:
              pDVar4 = FUN_100125a7();
              *pDVar4 = 0x16;
              if (local_c != '\0') {
                *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
              }
              return 0x7fffffff;
            }
            uVar3 = (ushort)local_8 * 0x100 + (ushort)local_7;
          }
          pbVar7 = _Str2 + 2;
          ptVar6 = local_18.mbcinfo;
        }
        if (uVar3 != uVar2) {
          iVar5 = (-(uint)(uVar3 < uVar2) & 2) - 1;
          if (local_c == '\0') {
            return iVar5;
          }
          *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
          return iVar5;
        }
        _Str2 = pbVar7;
      } while (uVar2 != 0);
      if (local_c != '\0') {
        *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
      }
      iVar5 = 0;
    }
  }
  return iVar5;
}


// 1001b1c3 __mbsicmp

/* Library Function - Single Match
    __mbsicmp
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsicmp(uchar *_Str1,uchar *_Str2)

{
  int iVar1;
  
  iVar1 = __mbsicmp_l(_Str1,_Str2,(_locale_t)0x0);
  return iVar1;
}


// 1001b1d6 __mbsnbcmp_l

/* Library Function - Single Match
    __mbsnbcmp_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsnbcmp_l(uchar *_Str1,uchar *_Str2,size_t _MaxCount,_locale_t _Locale)

{
  size_t sVar1;
  byte bVar2;
  int iVar3;
  DWORD *pDVar4;
  uchar *puVar5;
  uint uVar6;
  ushort uVar7;
  byte *pbVar8;
  _LocaleUpdate local_14 [4];
  int local_10;
  int local_c;
  char local_8;
  
  if (_MaxCount == 0) {
    iVar3 = 0;
  }
  else {
    _LocaleUpdate::_LocaleUpdate(local_14,_Locale);
    if (*(int *)(local_10 + 8) == 0) {
      iVar3 = _strncmp((char *)_Str1,(char *)_Str2,_MaxCount);
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
    }
    else if (_Str1 == (uchar *)0x0) {
      pDVar4 = FUN_100125a7();
      *pDVar4 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
      iVar3 = 0x7fffffff;
    }
    else {
      if (_Str2 != (uchar *)0x0) {
        do {
          bVar2 = *_Str1;
          uVar7 = (ushort)bVar2;
          sVar1 = _MaxCount - 1;
          puVar5 = _Str1 + 1;
          if ((*(byte *)(bVar2 + 0x1d + local_10) & 4) == 0) {
LAB_1001b2d5:
            _Str1 = puVar5;
            uVar6 = (uint)*_Str2;
            pbVar8 = _Str2 + 1;
            if ((*(byte *)(uVar6 + 0x1d + local_10) & 4) != 0) {
              if (sVar1 != 0) {
                sVar1 = _MaxCount - 2;
                if (*pbVar8 != 0) {
                  uVar6 = (uint)CONCAT11(*_Str2,*pbVar8);
                  pbVar8 = _Str2 + 2;
                  goto LAB_1001b305;
                }
              }
              _MaxCount = sVar1;
              uVar6 = 0;
              sVar1 = _MaxCount;
            }
          }
          else {
            if (sVar1 != 0) {
              if (*puVar5 == '\0') {
                uVar7 = 0;
              }
              else {
                uVar7 = CONCAT11(bVar2,*puVar5);
                puVar5 = _Str1 + 2;
              }
              goto LAB_1001b2d5;
            }
            uVar6 = (uint)*_Str2;
            uVar7 = 0;
            pbVar8 = _Str2;
            _Str1 = puVar5;
            if ((*(byte *)(uVar6 + 0x1d + local_10) & 4) != 0) {
LAB_1001b318:
              if (local_8 != '\0') {
                *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
              }
              return 0;
            }
          }
LAB_1001b305:
          _MaxCount = sVar1;
          if ((ushort)uVar6 != uVar7) {
            iVar3 = (-(uint)((ushort)uVar6 < uVar7) & 2) - 1;
            if (local_8 == '\0') {
              return iVar3;
            }
            *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
            return iVar3;
          }
          if ((uVar7 == 0) || (_Str2 = pbVar8, _MaxCount == 0)) goto LAB_1001b318;
        } while( true );
      }
      pDVar4 = FUN_100125a7();
      *pDVar4 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
      iVar3 = 0x7fffffff;
    }
  }
  return iVar3;
}


// 1001b33f __mbsnbcmp

/* Library Function - Single Match
    __mbsnbcmp
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsnbcmp(uchar *_Str1,uchar *_Str2,size_t _MaxCount)

{
  int iVar1;
  
  iVar1 = __mbsnbcmp_l(_Str1,_Str2,_MaxCount,(_locale_t)0x0);
  return iVar1;
}


// 1001b358 __global_unwind2

/* Library Function - Single Match
    __global_unwind2
   
   Library: Visual Studio */

void __cdecl __global_unwind2(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x1001b370,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 1001b3bd __local_unwind2

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
  puStack_1c = &LAB_1001b378;
  local_20 = *unaff_FS_OFFSET;
  *unaff_FS_OFFSET = &local_20;
  while( true ) {
    uVar1 = *(uint *)(param_1 + 0xc);
    if ((uVar1 == 0xffffffff) || ((param_2 != 0xffffffff && (uVar1 <= param_2)))) break;
    local_18 = *(undefined4 *)(*(int *)(param_1 + 8) + uVar1 * 0xc);
    *(undefined4 *)(param_1 + 0xc) = local_18;
    if (*(int *)(*(int *)(param_1 + 8) + 4 + uVar1 * 0xc) == 0) {
      __NLG_Notify(0x101);
      FUN_1001b48c();
    }
  }
  *unaff_FS_OFFSET = local_20;
  return;
}


// 1001b46d __NLG_Notify

/* Library Function - Single Match
    __NLG_Notify
   
   Libraries: Visual Studio 2017 Debug, Visual Studio 2017 Release, Visual Studio 2019 Debug, Visual
   Studio 2019 Release */

void __NLG_Notify(ulong param_1)

{
  undefined4 in_EAX;
  undefined4 unaff_EBP;
  
  DAT_100257e8 = param_1;
  DAT_100257e4 = in_EAX;
  DAT_100257ec = unaff_EBP;
  return;
}


// 1001b48c FUN_1001b48c

void FUN_1001b48c(void)

{
  code *in_EAX;
  
  (*in_EAX)();
  return;
}


// 1001b48f ___report_gsfailure

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
  
  _DAT_10029808 =
       (uint)(in_NT & 1) * 0x4000 | (uint)SBORROW4((int)&stack0xfffffffc,0x328) * 0x800 |
       (uint)(in_IF & 1) * 0x200 | (uint)(in_TF & 1) * 0x100 | (uint)((int)&local_32c < 0) * 0x80 |
       (uint)(&stack0x00000000 == (undefined1 *)0x32c) * 0x40 | (uint)(in_AF & 1) * 0x10 |
       (uint)((POPCOUNT((uint)&local_32c & 0xff) & 1U) == 0) * 4 |
       (uint)(&stack0xfffffffc < (undefined1 *)0x328) | (uint)(in_ID & 1) * 0x200000 |
       (uint)(in_VIP & 1) * 0x100000 | (uint)(in_VIF & 1) * 0x80000 | (uint)(in_AC & 1) * 0x40000;
  _DAT_1002980c = &stack0x00000004;
  _DAT_10029748 = 0x10001;
  _DAT_100296f0 = 0xc0000409;
  _DAT_100296f4 = 1;
  local_32c = DAT_10025040;
  local_328 = DAT_10025044;
  _DAT_100296fc = unaff_retaddr;
  _DAT_100297d4 = in_GS;
  _DAT_100297d8 = in_FS;
  _DAT_100297dc = in_ES;
  _DAT_100297e0 = in_DS;
  _DAT_100297e4 = unaff_EDI;
  _DAT_100297e8 = unaff_ESI;
  _DAT_100297ec = unaff_EBX;
  _DAT_100297f0 = in_EDX;
  _DAT_100297f4 = in_ECX;
  _DAT_100297f8 = in_EAX;
  _DAT_100297fc = unaff_EBP;
  DAT_10029800 = unaff_retaddr;
  _DAT_10029804 = in_CS;
  _DAT_10029810 = in_SS;
  DAT_10029740 = IsDebuggerPresent();
  FUN_10018b17();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  UnhandledExceptionFilter((_EXCEPTION_POINTERS *)&PTR_DAT_10021414);
  if (DAT_10029740 == 0) {
    FUN_10018b17();
  }
  uExitCode = 0xc0000409;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  return;
}


// 1001b593 FUN_1001b593

undefined4 __cdecl FUN_1001b593(undefined4 param_1,undefined4 param_2,uint param_3)

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
  if (DAT_10029a14 == 0) {
    hModule = LoadLibraryA("USER32.DLL");
    if (hModule == (HMODULE)0x0) {
      return 0;
    }
    pFVar1 = GetProcAddress(hModule,"MessageBoxA");
    if (pFVar1 == (FARPROC)0x0) {
      return 0;
    }
    DAT_10029a14 = FUN_10015154((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetActiveWindow");
    DAT_10029a18 = FUN_10015154((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetLastActivePopup");
    DAT_10029a1c = FUN_10015154((int)pFVar1);
    iVar2 = __get_osplatform(&local_10);
    if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
    if (local_10 == 2) {
      pFVar1 = GetProcAddress(hModule,"GetUserObjectInformationA");
      DAT_10029a24 = FUN_10015154((int)pFVar1);
      if (DAT_10029a24 != 0) {
        pFVar1 = GetProcAddress(hModule,"GetProcessWindowStation");
        DAT_10029a20 = FUN_10015154((int)pFVar1);
      }
    }
  }
  if ((DAT_10029a20 != local_c) && (DAT_10029a24 != local_c)) {
    pcVar3 = (code *)FUN_100151cb(DAT_10029a20);
    pcVar4 = (code *)FUN_100151cb(DAT_10029a24);
    if (((pcVar3 != (code *)0x0) && (pcVar4 != (code *)0x0)) &&
       (((iVar2 = (*pcVar3)(), iVar2 == 0 ||
         (iVar2 = (*pcVar4)(iVar2,1,local_24,0xc,local_18), iVar2 == 0)) || ((local_1c & 1) == 0))))
    {
      iVar2 = FUN_1000f1fd(&local_14);
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
      goto LAB_1001b729;
    }
  }
  if ((((DAT_10029a18 != local_c) &&
       (pcVar3 = (code *)FUN_100151cb(DAT_10029a18), pcVar3 != (code *)0x0)) &&
      (local_8 = (*pcVar3)(), local_8 != 0)) &&
     ((DAT_10029a1c != local_c &&
      (pcVar3 = (code *)FUN_100151cb(DAT_10029a1c), pcVar3 != (code *)0x0)))) {
    local_8 = (*pcVar3)(local_8);
  }
LAB_1001b729:
  pcVar3 = (code *)FUN_100151cb(DAT_10029a14);
  if (pcVar3 == (code *)0x0) {
    return 0;
  }
  uVar5 = (*pcVar3)(local_8,param_1,param_2,param_3);
  return uVar5;
}


// 1001b750 _strncpy_s

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
LAB_1001b774:
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
          pDVar2 = FUN_100125a7();
          eVar5 = 0x22;
          *pDVar2 = 0x22;
          goto LAB_1001b785;
        }
        *_Dst = '\0';
      }
    }
  }
  else if (_Dst != (char *)0x0) goto LAB_1001b774;
  pDVar2 = FUN_100125a7();
  eVar5 = 0x16;
  *pDVar2 = 0x16;
LAB_1001b785:
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar5;
}


// 1001b803 __set_error_mode

/* Library Function - Single Match
    __set_error_mode
   
   Library: Visual Studio 2005 Release */

int __cdecl __set_error_mode(int _Mode)

{
  int iVar1;
  DWORD *pDVar2;
  
  if (-1 < _Mode) {
    if (_Mode < 3) {
      iVar1 = DAT_100290c8;
      DAT_100290c8 = _Mode;
      return iVar1;
    }
    if (_Mode == 3) {
      return DAT_100290c8;
    }
  }
  pDVar2 = FUN_100125a7();
  *pDVar2 = 0x16;
  FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 1001b849 __mbsnbcpy_s_l

/* Library Function - Single Match
    __mbsnbcpy_s_l
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl
__mbsnbcpy_s_l(uchar *_Dst,size_t _DstSizeInBytes,uchar *_Src,size_t _MaxCount,_locale_t _Locale)

{
  byte *pbVar1;
  uchar uVar2;
  DWORD *pDVar3;
  errno_t eVar4;
  uchar *puVar5;
  uchar *puVar6;
  int iVar7;
  size_t sVar8;
  byte *pbVar9;
  localeinfo_struct local_18;
  int local_10;
  char local_c;
  byte *local_8;
  
  if (_MaxCount == 0) {
    if (_Dst == (uchar *)0x0) {
      if (_DstSizeInBytes == 0) {
        return 0;
      }
      goto LAB_1001b874;
    }
  }
  else if (_Dst == (uchar *)0x0) goto LAB_1001b874;
  if (_DstSizeInBytes == 0) {
LAB_1001b874:
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  if (_MaxCount == 0) {
    *_Dst = '\0';
    return 0;
  }
  if (_Src == (uchar *)0x0) {
    *_Dst = '\0';
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_18,_Locale);
  if ((local_18.mbcinfo)->ismbcodepage == 0) {
    eVar4 = _strncpy_s((char *)_Dst,_DstSizeInBytes,(char *)_Src,_MaxCount);
    goto LAB_1001ba35;
  }
  puVar5 = _Dst;
  sVar8 = _DstSizeInBytes;
  if (_MaxCount == 0xffffffff) {
    do {
      uVar2 = *_Src;
      *puVar5 = uVar2;
      puVar5 = puVar5 + 1;
      _Src = _Src + 1;
      if (uVar2 == '\0') break;
      sVar8 = sVar8 - 1;
    } while (sVar8 != 0);
  }
  else {
    do {
      puVar6 = puVar5;
      uVar2 = *_Src;
      *puVar6 = uVar2;
      puVar5 = puVar6 + 1;
      _Src = _Src + 1;
      if ((uVar2 == '\0') || (sVar8 = sVar8 - 1, sVar8 == 0)) break;
      _MaxCount = _MaxCount - 1;
    } while (_MaxCount != 0);
    if (_MaxCount == 0) {
      *puVar5 = '\0';
      puVar5 = puVar6 + 2;
    }
  }
  if (sVar8 != 0) {
    if ((int)puVar5 - (int)_Dst < 2) {
LAB_1001ba43:
      if (local_c != '\0') {
        *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
      }
      return 0;
    }
    pbVar9 = puVar5 + -2;
    pbVar1 = pbVar9;
    while ((_Dst <= pbVar1 && (iVar7 = __ismbblead_l((uint)*pbVar1,&local_18), iVar7 != 0))) {
      pbVar1 = pbVar1 + -1;
    }
    if (((int)pbVar9 - (int)pbVar1 & 1U) == 0) goto LAB_1001ba43;
LAB_1001ba27:
    *pbVar9 = 0;
    pDVar3 = FUN_100125a7();
    eVar4 = 0x2a;
    *pDVar3 = 0x2a;
LAB_1001ba35:
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
      return eVar4;
    }
    return eVar4;
  }
  if ((*_Src == '\0') || (_MaxCount == 1)) {
    pbVar9 = puVar5 + -1;
    local_8 = pbVar9;
    while ((_Dst <= local_8 && (iVar7 = __ismbblead_l((uint)*local_8,&local_18), iVar7 != 0))) {
      local_8 = local_8 + -1;
    }
    if (((int)pbVar9 - (int)local_8 & 1U) != 0) goto LAB_1001ba27;
  }
  if (_MaxCount != 0xffffffff) {
    *_Dst = '\0';
    pDVar3 = FUN_100125a7();
    *pDVar3 = 0x22;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
      return 0x22;
    }
    return 0x22;
  }
  if (1 < _DstSizeInBytes) {
    pbVar9 = _Dst + (_DstSizeInBytes - 2);
    pbVar1 = pbVar9;
    while ((_Dst <= pbVar1 && (iVar7 = __ismbblead_l((uint)*pbVar1,&local_18), iVar7 != 0))) {
      pbVar1 = pbVar1 + -1;
    }
    if (((int)pbVar9 - (int)pbVar1 & 1U) != 0) {
      *pbVar9 = 0;
      goto LAB_1001b9b0;
    }
  }
  _Dst[_DstSizeInBytes - 1] = '\0';
LAB_1001b9b0:
  if (local_c != '\0') {
    *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
  }
  return 0x50;
}


// 1001ba56 __crtGetStringTypeA_stat

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  local_c = in_ECX;
  if (DAT_10029a64 == 0) {
    BVar2 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_10029a64 = 2;
      }
      goto LAB_1001baaf;
    }
    DAT_10029a64 = 1;
  }
  else {
LAB_1001baaf:
    if ((DAT_10029a64 == 2) || (DAT_10029a64 == 0)) {
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
        FUN_10005520(uVar7);
      }
      goto LAB_1001bbfc;
    }
    if (DAT_10029a64 != 1) goto LAB_1001bbfc;
  }
  local_c = (int *)0x0;
  if (param_5 == (ushort *)0x0) {
    param_5 = *(ushort **)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar((UINT)param_5,(uint)(param_7 != 0) * 8 + 1,(LPCSTR)param_2,
                                    (int)param_3,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_1001bbfc;
  lpWideCharStr = (LPCWSTR)0x0;
  if ((0 < (int)cchWideChar) && (cchWideChar < 0x7ffffff1)) {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar4 = (LPCWSTR)&stack0xffffffe8;
      lpWideCharStr = (LPCWSTR)&stack0xffffffe8;
      if (&stack0x00000000 != (undefined1 *)0x18) {
LAB_1001bb3f:
        lpWideCharStr = pWVar4 + 4;
      }
    }
    else {
      pWVar4 = (LPCWSTR)FUN_10005720(uVar1);
      lpWideCharStr = pWVar4;
      if (pWVar4 != (LPCWSTR)0x0) {
        pWVar4[0] = L'\xdddd';
        pWVar4[1] = L'\0';
        goto LAB_1001bb3f;
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
LAB_1001bbfc:
  iVar5 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar5;
}


// 1001bc0e ___crtGetStringTypeA

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


// 1001bc4e ___free_lc_time

/* Library Function - Single Match
    ___free_lc_time
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lc_time(undefined4 *param_1)

{
  if (param_1 != (undefined4 *)0x0) {
    FUN_10005520(param_1[1]);
    FUN_10005520(param_1[2]);
    FUN_10005520(param_1[3]);
    FUN_10005520(param_1[4]);
    FUN_10005520(param_1[5]);
    FUN_10005520(param_1[6]);
    FUN_10005520(*param_1);
    FUN_10005520(param_1[8]);
    FUN_10005520(param_1[9]);
    FUN_10005520(param_1[10]);
    FUN_10005520(param_1[0xb]);
    FUN_10005520(param_1[0xc]);
    FUN_10005520(param_1[0xd]);
    FUN_10005520(param_1[7]);
    FUN_10005520(param_1[0xe]);
    FUN_10005520(param_1[0xf]);
    FUN_10005520(param_1[0x10]);
    FUN_10005520(param_1[0x11]);
    FUN_10005520(param_1[0x12]);
    FUN_10005520(param_1[0x13]);
    FUN_10005520(param_1[0x14]);
    FUN_10005520(param_1[0x15]);
    FUN_10005520(param_1[0x16]);
    FUN_10005520(param_1[0x17]);
    FUN_10005520(param_1[0x18]);
    FUN_10005520(param_1[0x19]);
    FUN_10005520(param_1[0x1a]);
    FUN_10005520(param_1[0x1b]);
    FUN_10005520(param_1[0x1c]);
    FUN_10005520(param_1[0x1d]);
    FUN_10005520(param_1[0x1e]);
    FUN_10005520(param_1[0x1f]);
    FUN_10005520(param_1[0x20]);
    FUN_10005520(param_1[0x21]);
    FUN_10005520(param_1[0x22]);
    FUN_10005520(param_1[0x23]);
    FUN_10005520(param_1[0x24]);
    FUN_10005520(param_1[0x25]);
    FUN_10005520(param_1[0x26]);
    FUN_10005520(param_1[0x27]);
    FUN_10005520(param_1[0x28]);
    FUN_10005520(param_1[0x29]);
    FUN_10005520(param_1[0x2a]);
  }
  return;
}


// 1001bdde ___free_lconv_num

/* Library Function - Single Match
    ___free_lconv_num
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_num(int *param_1)

{
  if (param_1 != (int *)0x0) {
    if ((undefined *)*param_1 != PTR_DAT_100258b8) {
      FUN_10005520((undefined *)*param_1);
    }
    if ((undefined *)param_1[1] != PTR_DAT_100258bc) {
      FUN_10005520((undefined *)param_1[1]);
    }
    if ((undefined *)param_1[2] != PTR_DAT_100258c0) {
      FUN_10005520((undefined *)param_1[2]);
    }
  }
  return;
}


// 1001be1e ___free_lconv_mon

/* Library Function - Single Match
    ___free_lconv_mon
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_mon(int param_1)

{
  if (param_1 != 0) {
    if (*(undefined **)(param_1 + 0xc) != PTR_DAT_100258c4) {
      FUN_10005520(*(undefined **)(param_1 + 0xc));
    }
    if (*(undefined **)(param_1 + 0x10) != PTR_DAT_100258c8) {
      FUN_10005520(*(undefined **)(param_1 + 0x10));
    }
    if (*(undefined **)(param_1 + 0x14) != PTR_DAT_100258cc) {
      FUN_10005520(*(undefined **)(param_1 + 0x14));
    }
    if (*(undefined **)(param_1 + 0x18) != PTR_DAT_100258d0) {
      FUN_10005520(*(undefined **)(param_1 + 0x18));
    }
    if (*(undefined **)(param_1 + 0x1c) != PTR_DAT_100258d4) {
      FUN_10005520(*(undefined **)(param_1 + 0x1c));
    }
    if (*(undefined **)(param_1 + 0x20) != PTR_DAT_100258d8) {
      FUN_10005520(*(undefined **)(param_1 + 0x20));
    }
    if (*(undefined **)(param_1 + 0x24) != PTR_DAT_100258dc) {
      FUN_10005520(*(undefined **)(param_1 + 0x24));
    }
  }
  return;
}


// 1001beb0 _strcspn

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


// 1001bef6 _strncmp

/* Library Function - Single Match
    _strncmp
   
   Library: Visual Studio 2005 Release */

int __cdecl _strncmp(char *_Str1,char *_Str2,size_t _MaxCount)

{
  byte *pbVar1;
  uint uVar2;
  byte *pbVar3;
  uint uVar4;
  uint local_8;
  
  local_8 = 0;
  if (_MaxCount != 0) {
    if ((3 < _MaxCount) && (pbVar1 = (byte *)_Str1, pbVar3 = (byte *)_Str2, _MaxCount != 4)) {
      do {
        _Str1 = (char *)(pbVar1 + 4);
        _Str2 = (char *)(pbVar3 + 4);
        if ((*pbVar1 == 0) || (*pbVar1 != *pbVar3)) {
          uVar2 = (uint)*pbVar1;
          uVar4 = (uint)*pbVar3;
          goto LAB_1001bfb0;
        }
        if ((pbVar1[1] == 0) || (pbVar1[1] != pbVar3[1])) {
          uVar2 = (uint)pbVar1[1];
          uVar4 = (uint)pbVar3[1];
          goto LAB_1001bfb0;
        }
        if ((pbVar1[2] == 0) || (pbVar1[2] != pbVar3[2])) {
          uVar2 = (uint)pbVar1[2];
          uVar4 = (uint)pbVar3[2];
          goto LAB_1001bfb0;
        }
        if ((pbVar1[3] == 0) || (pbVar1[3] != pbVar3[3])) {
          uVar2 = (uint)pbVar1[3];
          uVar4 = (uint)pbVar3[3];
          goto LAB_1001bfb0;
        }
        local_8 = local_8 + 4;
        pbVar1 = (byte *)_Str1;
        pbVar3 = (byte *)_Str2;
      } while (local_8 < _MaxCount - 4);
    }
    for (; local_8 < _MaxCount; local_8 = local_8 + 1) {
      if ((*_Str1 == 0) || (*_Str1 != *_Str2)) {
        uVar2 = (uint)(byte)*_Str1;
        uVar4 = (uint)(byte)*_Str2;
LAB_1001bfb0:
        return uVar2 - uVar4;
      }
      _Str1 = (char *)((byte *)_Str1 + 1);
      _Str2 = (char *)((byte *)_Str2 + 1);
    }
  }
  return 0;
}


// 1001bfc0 _strpbrk

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


// 1001c000 ___ansicp

/* Library Function - Single Match
    ___ansicp
   
   Library: Visual Studio 2005 Release */

void __cdecl ___ansicp(LCID param_1)

{
  int iVar1;
  CHAR local_10 [6];
  undefined1 local_a;
  uint local_8;
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  local_a = 0;
  iVar1 = GetLocaleInfoA(param_1,0x1004,local_10,6);
  if (iVar1 != 0) {
    _atol(local_10);
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001c047 ___convertcp

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  cbMultiByte = *param_4;
  bVar2 = false;
  if (param_1 == param_2) goto LAB_1001c1e7;
  BVar3 = GetCPInfo(param_1,&local_1c);
  if ((((BVar3 == 0) || (local_1c.MaxCharSize != 1)) ||
      (BVar3 = GetCPInfo(param_2,&local_1c), BVar3 == 0)) || (local_1c.MaxCharSize != 1)) {
    uVar7 = MultiByteToWideChar(param_1,1,param_3,cbMultiByte,(LPWSTR)0x0,0);
    bVar8 = uVar7 == 0;
    if (bVar8) goto LAB_1001c1e7;
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
LAB_1001c127:
        local_20 = pWVar5 + 4;
      }
    }
    else {
      pWVar5 = (LPCWSTR)FUN_10005720(uVar1);
      local_20 = pWVar5;
      if (pWVar5 != (LPCWSTR)0x0) {
        pWVar5[0] = L'\xdddd';
        pWVar5[1] = L'\0';
        goto LAB_1001c127;
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
           (lpMultiByteStr = (LPSTR)FUN_10005780(1,uVar7), lpMultiByteStr != (LPSTR)0x0)) {
          uVar7 = WideCharToMultiByte(param_2,0,local_20,uVar7,lpMultiByteStr,uVar7,(LPCSTR)0x0,
                                      (LPBOOL)0x0);
          if (uVar7 == 0) {
            FUN_10005520(lpMultiByteStr);
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
LAB_1001c1e7:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001c1f9 __mbtowc_l

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
          if (iVar2 != 0) goto LAB_1001c244;
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
        pDVar3 = FUN_100125a7();
        *pDVar3 = 0x2a;
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        return -1;
      }
      if (_DstCh != (wchar_t *)0x0) {
        *_DstCh = (ushort)(byte)*_SrcCh;
      }
LAB_1001c244:
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


// 1001c30c _mbtowc

/* Library Function - Single Match
    _mbtowc
   
   Library: Visual Studio 2005 Release */

int __cdecl _mbtowc(wchar_t *_DstCh,char *_SrcCh,size_t _SrcSizeInBytes)

{
  int iVar1;
  
  iVar1 = __mbtowc_l(_DstCh,_SrcCh,_SrcSizeInBytes,(_locale_t)0x0);
  return iVar1;
}


// 1001c323 __wchartodigit

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
      if (param_1 < 0x66a) goto LAB_1001c361;
      iVar1 = 0x6f0;
      if (param_1 < 0x6f0) {
        return -1;
      }
      if (param_1 < 0x6fa) goto LAB_1001c361;
      iVar1 = 0x966;
      if (param_1 < 0x966) {
        return -1;
      }
      if (param_1 < 0x970) goto LAB_1001c361;
      iVar1 = 0x9e6;
      if (param_1 < 0x9e6) {
        return -1;
      }
      if (param_1 < 0x9f0) goto LAB_1001c361;
      iVar1 = 0xa66;
      if (param_1 < 0xa66) {
        return -1;
      }
      if (param_1 < 0xa70) goto LAB_1001c361;
      iVar1 = 0xae6;
      if (param_1 < 0xae6) {
        return -1;
      }
      if (param_1 < 0xaf0) goto LAB_1001c361;
      iVar1 = 0xb66;
      if (param_1 < 0xb66) {
        return -1;
      }
      if (param_1 < 0xb70) goto LAB_1001c361;
      iVar1 = 0xc66;
      if (param_1 < 0xc66) {
        return -1;
      }
      if (param_1 < 0xc70) goto LAB_1001c361;
      iVar1 = 0xce6;
      if (param_1 < 0xce6) {
        return -1;
      }
      if (param_1 < 0xcf0) goto LAB_1001c361;
      iVar1 = 0xd66;
      if (param_1 < 0xd66) {
        return -1;
      }
      if (param_1 < 0xd70) goto LAB_1001c361;
      iVar1 = 0xe50;
      if (param_1 < 0xe50) {
        return -1;
      }
      if (param_1 < 0xe5a) goto LAB_1001c361;
      iVar1 = 0xed0;
      if (param_1 < 0xed0) {
        return -1;
      }
      if (param_1 < 0xeda) goto LAB_1001c361;
      iVar1 = 0xf20;
      if (param_1 < 0xf20) {
        return -1;
      }
      if (param_1 < 0xf2a) goto LAB_1001c361;
      iVar1 = 0x1040;
      if (param_1 < 0x1040) {
        return -1;
      }
      if (param_1 < 0x104a) goto LAB_1001c361;
      iVar1 = 0x17e0;
      if (param_1 < 0x17e0) {
        return -1;
      }
      if (param_1 < 0x17ea) goto LAB_1001c361;
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
LAB_1001c361:
      return (uint)param_1 - iVar1;
    }
  }
  return -1;
}


// 1001c4a5 __ld12tod

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
        goto LAB_1001c9a4;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10025908 - 1;
    iVar4 = (int)(DAT_10025908 + ((int)DAT_10025908 >> 0x1f & 0x1fU)) >> 5;
    uVar8 = DAT_10025908 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar6) goto LAB_1001c5d8;
          bVar14 = _Ifp < p_Var9;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_1001c5df;
LAB_1001c5d8:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_1001c5df:
              iVar4 = iVar4 + -1;
              *puVar6 = (uint)_Ifp;
              if ((iVar4 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_1001c5ed;
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
LAB_1001c5ed:
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
    if (iVar12 < (int)(DAT_10025904 - DAT_10025908)) {
      local_24[0] = 0;
      local_24[1] = 0;
    }
    else {
      if (DAT_10025904 < iVar12) {
        if (iVar12 < DAT_10025900) {
          local_24[0] = local_24[0] & 0x7fffffff;
          iVar12 = iVar12 + DAT_10025914;
          iVar2 = (int)(DAT_1002590c + ((int)DAT_1002590c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_1002590c & 0x8000001f;
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
          iVar12 = (int)(DAT_1002590c + ((int)DAT_1002590c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_1002590c & 0x8000001f;
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
          iVar12 = DAT_10025914 + DAT_10025900;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_1001c9a4;
      }
      local_14 = DAT_10025904 - local_14;
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
      iVar2 = DAT_10025908 - 1;
      iVar12 = (int)(DAT_10025908 + ((int)DAT_10025908 >> 0x1f & 0x1fU)) >> 5;
      uVar10 = DAT_10025908 & 0x8000001f;
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
          if (2 < iVar12) goto LAB_1001c790;
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
LAB_1001c790:
      *puVar7 = *puVar7 & -1 << ((byte)local_14 & 0x1f);
      iVar12 = local_10 + 1;
      if (iVar12 < 3) {
        puVar7 = local_24 + iVar12;
        for (iVar2 = 3 - iVar12; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar7 = 0;
          puVar7 = puVar7 + 1;
        }
      }
      uVar10 = DAT_1002590c + 1;
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
LAB_1001c9a4:
  local_24[0] = iVar12 << (0x1fU - (char)DAT_1002590c & 0x1f) |
                -(uint)(local_24[3] != 0) & 0x80000000 | local_24[0];
  if (DAT_10025910 == 0x40) {
    *(uint *)((int)&_D->x + 4) = local_24[0];
    *(uint *)&_D->x = local_24[1];
  }
  else if (DAT_10025910 == 0x20) {
    *(uint *)&_D->x = local_24[0];
  }
  return IVar3;
}


// 1001c9e7 __ld12tof

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
        goto LAB_1001cee6;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_10025920 - 1;
    iVar2 = (int)(DAT_10025920 + ((int)DAT_10025920 >> 0x1f & 0x1fU)) >> 5;
    uVar7 = DAT_10025920 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar5) goto LAB_1001cb1a;
          bVar14 = _Ifp < p_Var8;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_1001cb21;
LAB_1001cb1a:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_1001cb21:
              iVar2 = iVar2 + -1;
              *puVar5 = (uint)_Ifp;
              if ((iVar2 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_1001cb2f;
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
LAB_1001cb2f:
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
    if (iVar11 < (int)(DAT_1002591c - DAT_10025920)) {
      local_24 = 0;
      local_20 = 0.0;
    }
    else {
      if (DAT_1002591c < iVar11) {
        if (iVar11 < DAT_10025918) {
          local_24 = local_24 & 0x7fffffff;
          iVar11 = iVar11 + DAT_1002592c;
          iVar2 = (int)(DAT_10025924 + ((int)DAT_10025924 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10025924 & 0x8000001f;
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
          iVar11 = (int)(DAT_10025924 + ((int)DAT_10025924 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_10025924 & 0x8000001f;
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
          iVar11 = DAT_1002592c + DAT_10025918;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_1001cee6;
      }
      local_1c[2] = DAT_1002591c - local_1c[2];
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
      iVar2 = DAT_10025920 - 1;
      iVar11 = (int)(DAT_10025920 + ((int)DAT_10025920 >> 0x1f & 0x1fU)) >> 5;
      uVar9 = DAT_10025920 & 0x8000001f;
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
          if (2 < iVar11) goto LAB_1001ccd2;
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
LAB_1001ccd2:
      *puVar6 = *puVar6 & -1 << ((byte)local_1c[2] & 0x1f);
      iVar11 = local_1c[3] + 1;
      if (iVar11 < 3) {
        puVar6 = &local_24 + iVar11;
        for (iVar2 = 3 - iVar11; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar6 = 0;
          puVar6 = puVar6 + 1;
        }
      }
      uVar9 = DAT_10025924 + 1;
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
LAB_1001cee6:
  fVar12 = (float)(iVar11 << (0x1fU - (char)DAT_10025924 & 0x1f) |
                   -(uint)(local_1c[1] != 0) & 0x80000000 | local_24);
  if (DAT_10025928 == 0x40) {
    _F[1].f = fVar12;
    _F->f = local_20;
  }
  else if (DAT_10025928 == 0x20) {
    _F->f = fVar12;
  }
  return IVar3;
}


// 1001cf29 __crtGetStringTypeW_stat

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  if (param_4 < -1) goto LAB_1001d13b;
  if (DAT_10029a6c == 0) {
    BVar3 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_10029a6c = 2;
      }
      goto LAB_1001cf90;
    }
    DAT_10029a6c = 1;
  }
  else {
LAB_1001cf90:
    if (DAT_10029a6c != 1) {
      if ((DAT_10029a6c != 2) && (DAT_10029a6c != 0)) goto LAB_1001d13b;
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
      if (_Size == 0) goto LAB_1001d13b;
      if (((int)_Size < 1) || (0xffffffe0 < _Size)) {
        local_c = (LPSTR)0x0;
      }
      else if (_Size + 8 < 0x401) {
        pCVar5 = &stack0xffffffe0;
        local_c = &stack0xffffffe0;
        if (&stack0x00000000 != (undefined1 *)0x20) {
LAB_1001d041:
          local_c = pCVar5 + 8;
        }
      }
      else {
        pCVar5 = (LPSTR)FUN_10005720(_Size + 8);
        local_c = pCVar5;
        if (pCVar5 != (LPSTR)0x0) {
          pCVar5[0] = -0x23;
          pCVar5[1] = -0x23;
          pCVar5[2] = '\0';
          pCVar5[3] = '\0';
          goto LAB_1001d041;
        }
      }
      if (local_c == (LPSTR)0x0) goto LAB_1001d13b;
      _memset(local_c,0,_Size);
      iVar6 = WideCharToMultiByte(param_6,0,param_3,param_4,local_c,_Size,(LPCSTR)0x0,(LPBOOL)0x0);
      if (iVar6 != 0) {
        if (((int)(_Size + 1) < 1) || (0x7ffffff0 < _Size + 1)) {
          lpCharType = (LPWORD)0x0;
        }
        else {
          uVar2 = _Size * 2 + 10;
          if (uVar2 < 0x401) {
            if (&stack0x00000000 == (undefined1 *)0x20) goto LAB_1001d12f;
            lpCharType = (LPWORD)&stack0xffffffe8;
          }
          else {
            lpCharType = (LPWORD)FUN_10005720(uVar2);
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
            FID_conflict__memcpy(param_5,lpCharType,param_4 * 2);
          }
          __freea(lpCharType);
        }
      }
LAB_1001d12f:
      __freea(local_c);
      goto LAB_1001d13b;
    }
  }
  GetStringTypeW(param_2,param_3,param_4,param_5);
LAB_1001d13b:
  iVar6 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar6;
}


// 1001d14d ___crtGetStringTypeW

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


// 1001d18b ___mtold12

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
  
  uVar7 = DAT_10025040 ^ (uint)&stack0xfffffffc;
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


// 1001d357 FUN_1001d357

/* WARNING: Removing unreachable block (ram,0x1001d848) */
/* WARNING: Removing unreachable block (ram,0x1001d852) */
/* WARNING: Removing unreachable block (ram,0x1001d857) */

void __cdecl
FUN_1001d357(int param_1,uint param_2,ushort param_3,int param_4,byte param_5,short *param_6)

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
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
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
    goto LAB_1001dbd5;
  }
  if ((short)uVar14 == 0x7fff) {
    *param_6 = 1;
    if (((param_2 == 0x80000000) && (param_1 == 0)) || ((param_2 & 0x40000000) != 0)) {
      if ((uVar8 == 0) || (param_2 != 0xc0000000)) {
        if ((param_2 != 0x80000000) || (param_1 != 0)) goto LAB_1001d486;
        pcVar25 = "1#INF";
      }
      else {
        if (param_1 != 0) {
LAB_1001d486:
          pcVar25 = "1#QNAN";
          goto LAB_1001d48b;
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
LAB_1001d48b:
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
    goto LAB_1001dbd5;
  }
  local_50 = (short)(((uVar14 >> 8) + (param_2 >> 0x18) * 2) * 0x4d + -0x134312f4 + uVar14 * 0x4d10
                    >> 0x10);
  uVar16 = (uint)local_50;
  local_1a = (undefined1)uVar14;
  bStack_19 = (byte)(uVar14 >> 8);
  local_24._2_2_ = (ushort)param_1;
  local_24._0_2_ = 0;
  local_6c = &PTR_DAT_100258d0;
  uStack_20._0_2_ = uStack_12;
  uStack_20._2_2_ = (ushort)local_10;
  uStack_1c = local_10._2_2_;
  if (uVar16 != 0) {
    iVar20 = param_1;
    uVar14 = -uVar16;
    if (0 < (int)uVar16) {
      local_6c = (undefined **)&DAT_10025a30;
      uVar14 = uVar16;
    }
    while (uVar14 != 0) {
      uStack_20._0_2_ = (ushort)((uint)iVar20 >> 0x10);
      local_24._2_2_ = (ushort)iVar20;
      local_6c = local_6c + 0x15;
      iVar3 = CONCAT22(local_c,local_10._2_2_);
      if ((uVar14 & 7) == 0) goto LAB_1001d7ac;
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
LAB_1001d6d0:
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
            if ((short)uVar24 < 1) goto LAB_1001d6d0;
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
          if (0x7ffe < uVar24) goto LAB_1001d790;
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
LAB_1001d790:
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
LAB_1001d7ac:
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
          goto LAB_1001da14;
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
LAB_1001d922:
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
          if ((short)uVar24 < 1) goto LAB_1001d922;
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
        goto LAB_1001da14;
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
LAB_1001da14:
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
        goto LAB_1001dbd5;
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
LAB_1001dbd5:
  local_24 = uVar16;
  local_10 = param_2;
  uStack_3e = iVar1;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 1001dc1d __hw_cw

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


// 1001dcab ___hw_cw_sse2

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


// 1001dd4b __control87

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
  if (DAT_10029c44 != 0) {
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


// 1001e04e wcsncnt

/* Library Function - Single Match
    int __cdecl wcsncnt(wchar_t const *,int)
   
   Library: Visual Studio 2005 Release */

int __cdecl wcsncnt(wchar_t *param_1,int param_2)

{
  short *in_EAX;
  wchar_t *pwVar1;
  
  pwVar1 = param_1;
  for (; (pwVar1 != (wchar_t *)0x0 && (*in_EAX != 0)); in_EAX = in_EAX + 1) {
    pwVar1 = (wchar_t *)((int)pwVar1 + -1);
  }
  return (int)param_1 + (-1 - ((int)pwVar1 + -1));
}


// 1001e06a __crtCompareStringW_stat

/* WARNING: Function: __alloca_probe_16 replaced with injection: alloca_probe */
/* Library Function - Single Match
    int __cdecl __crtCompareStringW_stat(struct localeinfo_struct *,unsigned long,unsigned
   long,wchar_t const *,int,wchar_t const *,int,int)
   
   Library: Visual Studio 2005 Release */

int __cdecl
__crtCompareStringW_stat
          (localeinfo_struct *param_1,ulong param_2,ulong param_3,wchar_t *param_4,int param_5,
          wchar_t *param_6,int param_7,int param_8)

{
  uint uVar1;
  int iVar2;
  DWORD DVar3;
  uint cbMultiByte;
  PCNZCH pCVar4;
  uint cbMultiByte_00;
  LPSTR pCVar5;
  int *in_ECX;
  LPSTR lpMultiByteStr;
  int unaff_EDI;
  PCNZCH local_c;
  
  uVar1 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  if (DAT_10029a70 == 0) {
    iVar2 = CompareStringW(0,0,L"",1,L"",1);
    if (iVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_10029a70 = 2;
      }
    }
    else {
      DAT_10029a70 = 1;
    }
  }
  if (0 < (int)param_4) {
    param_4 = (wchar_t *)wcsncnt(param_4,unaff_EDI);
  }
  if (0 < (int)param_6) {
    param_6 = (wchar_t *)wcsncnt(param_6,unaff_EDI);
  }
  if ((param_4 == (wchar_t *)0x0) || (param_6 == (wchar_t *)0x0)) goto LAB_1001e2a8;
  if (DAT_10029a70 == 1) {
    CompareStringW((LCID)param_1,param_2,(PCNZWCH)param_3,(int)param_4,(PCNZWCH)param_5,(int)param_6
                  );
    goto LAB_1001e2a8;
  }
  if ((DAT_10029a70 != 2) && (DAT_10029a70 != 0)) goto LAB_1001e2a8;
  if (param_1 == (localeinfo_struct *)0x0) {
    param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
  }
  if (param_7 == 0) {
    param_7 = *(int *)(*in_ECX + 4);
  }
  iVar2 = ___ansicp((LCID)param_1);
  if ((param_7 != iVar2) && (iVar2 != -1)) {
    param_7 = iVar2;
  }
  cbMultiByte = WideCharToMultiByte(param_7,0,(LPCWSTR)param_3,(int)param_4,(LPSTR)0x0,0,(LPCSTR)0x0
                                    ,(LPBOOL)0x0);
  if (cbMultiByte == 0) goto LAB_1001e2a8;
  if (((int)cbMultiByte < 1) || (0xffffffe0 / cbMultiByte == 0)) {
    local_c = (PCNZCH)0x0;
  }
  else if (cbMultiByte + 8 < 0x401) {
    pCVar4 = &stack0xffffffdc;
    local_c = &stack0xffffffdc;
    if (&stack0x00000000 != (undefined1 *)0x24) {
LAB_1001e1be:
      local_c = pCVar4 + 8;
    }
  }
  else {
    pCVar4 = (PCNZCH)FUN_10005720(cbMultiByte + 8);
    local_c = pCVar4;
    if (pCVar4 != (PCNZCH)0x0) {
      pCVar4[0] = -0x23;
      pCVar4[1] = -0x23;
      pCVar4[2] = '\0';
      pCVar4[3] = '\0';
      goto LAB_1001e1be;
    }
  }
  if (local_c == (PCNZCH)0x0) goto LAB_1001e2a8;
  iVar2 = WideCharToMultiByte(param_7,0,(LPCWSTR)param_3,(int)param_4,local_c,cbMultiByte,
                              (LPCSTR)0x0,(LPBOOL)0x0);
  if ((iVar2 != 0) &&
     (cbMultiByte_00 =
           WideCharToMultiByte(param_7,0,(LPCWSTR)param_5,(int)param_6,(LPSTR)0x0,0,(LPCSTR)0x0,
                               (LPBOOL)0x0), cbMultiByte_00 != 0)) {
    if (((int)cbMultiByte_00 < 1) || (0xffffffe0 / cbMultiByte_00 == 0)) {
      lpMultiByteStr = (LPSTR)0x0;
    }
    else if (cbMultiByte_00 + 8 < 0x401) {
      pCVar5 = &stack0xffffffdc;
      lpMultiByteStr = &stack0xffffffdc;
      if (&stack0x00000000 != (undefined1 *)0x24) {
LAB_1001e23d:
        lpMultiByteStr = pCVar5 + 8;
      }
    }
    else {
      pCVar5 = (LPSTR)FUN_10005720(cbMultiByte_00 + 8);
      lpMultiByteStr = pCVar5;
      if (pCVar5 != (LPSTR)0x0) {
        pCVar5[0] = -0x23;
        pCVar5[1] = -0x23;
        pCVar5[2] = '\0';
        pCVar5[3] = '\0';
        goto LAB_1001e23d;
      }
    }
    if (lpMultiByteStr != (LPSTR)0x0) {
      iVar2 = WideCharToMultiByte(param_7,0,(LPCWSTR)param_5,(int)param_6,lpMultiByteStr,
                                  cbMultiByte_00,(LPCSTR)0x0,(LPBOOL)0x0);
      if (iVar2 != 0) {
        CompareStringA((LCID)param_1,param_2,local_c,cbMultiByte,lpMultiByteStr,cbMultiByte_00);
      }
      __freea(lpMultiByteStr);
    }
  }
  __freea(local_c);
LAB_1001e2a8:
  iVar2 = __security_check_cookie(uVar1 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 1001e2ba ___crtCompareStringW

/* Library Function - Single Match
    ___crtCompareStringW
   
   Library: Visual Studio 2005 Release */

int __cdecl
___crtCompareStringW
          (LPCWSTR _LocaleName,DWORD _DwCmpFlags,LPCWSTR _LpString1,int _CchCount1,
          LPCWSTR _LpString2,int _CchCount2)

{
  int iVar1;
  wchar_t *in_stack_0000001c;
  int in_stack_00000020;
  int in_stack_ffffffec;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&stack0xffffffec,(localeinfo_struct *)_LocaleName);
  iVar1 = __crtCompareStringW_stat
                    ((localeinfo_struct *)_DwCmpFlags,(ulong)_LpString1,_CchCount1,_LpString2,
                     _CchCount2,in_stack_0000001c,in_stack_00000020,in_stack_ffffffec);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return iVar1;
}


// 1001e2fa __strnicmp_l

/* Library Function - Single Match
    __strnicmp_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __strnicmp_l(char *_Str1,char *_Str2,size_t _MaxCount,_locale_t _Locale)

{
  DWORD *pDVar1;
  int iVar2;
  int iVar3;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  if (_MaxCount == 0) {
    iVar2 = 0;
  }
  else {
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
    if ((_Str1 == (char *)0x0) || (_Str2 == (char *)0x0)) {
      pDVar1 = FUN_100125a7();
      *pDVar1 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
      iVar2 = 0x7fffffff;
    }
    else if (_MaxCount < 0x80000000) {
      if ((local_14.locinfo)->lc_category[0].wlocale == (wchar_t *)0x0) {
        iVar2 = ___ascii_strnicmp(_Str1,_Str2,_MaxCount);
      }
      else {
        do {
          iVar2 = __tolower_l((uint)(byte)*_Str1,&local_14);
          _Str1 = _Str1 + 1;
          iVar3 = __tolower_l((uint)(byte)*_Str2,&local_14);
          _Str2 = (char *)((byte *)_Str2 + 1);
          _MaxCount = _MaxCount - 1;
          if ((_MaxCount == 0) || (iVar2 == 0)) break;
        } while (iVar2 == iVar3);
        iVar2 = iVar2 - iVar3;
      }
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
    }
    else {
      pDVar1 = FUN_100125a7();
      *pDVar1 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
      iVar2 = 0x7fffffff;
    }
  }
  return iVar2;
}


// 1001e3ea __strnicmp

/* Library Function - Single Match
    __strnicmp
   
   Library: Visual Studio 2005 Release */

int __cdecl __strnicmp(char *_Str1,char *_Str2,size_t _MaxCount)

{
  DWORD *pDVar1;
  int iVar2;
  
  if (DAT_100295a0 == 0) {
    if (((_Str1 != (char *)0x0) && (_Str2 != (char *)0x0)) && (_MaxCount < 0x80000000)) {
      iVar2 = ___ascii_strnicmp(_Str1,_Str2,_MaxCount);
      return iVar2;
    }
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = 0x7fffffff;
  }
  else {
    iVar2 = __strnicmp_l(_Str1,_Str2,_MaxCount,(_locale_t)0x0);
  }
  return iVar2;
}


// 1001e446 __mbsnicmp_l

/* Library Function - Single Match
    __mbsnicmp_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsnicmp_l(uchar *_Str1,uchar *_Str2,size_t _MaxCount,_locale_t _Locale)

{
  byte bVar1;
  int iVar2;
  DWORD *pDVar3;
  uint uVar4;
  uchar *puVar5;
  ushort uVar6;
  byte *pbVar7;
  _LocaleUpdate local_18 [4];
  int local_14;
  int local_10;
  char local_c;
  ushort local_8;
  
  if (_MaxCount == 0) {
    iVar2 = 0;
  }
  else {
    _LocaleUpdate::_LocaleUpdate(local_18,_Locale);
    if (*(int *)(local_14 + 8) == 0) {
      iVar2 = __strnicmp((char *)_Str1,(char *)_Str2,_MaxCount);
      if (local_c != '\0') {
        *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
      }
    }
    else if ((_Str1 == (uchar *)0x0) || (_Str2 == (uchar *)0x0)) {
      pDVar3 = FUN_100125a7();
      *pDVar3 = 0x16;
      FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      if (local_c != '\0') {
        *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
      }
      iVar2 = 0x7fffffff;
    }
    else {
      do {
        uVar4 = (uint)*_Str1;
        _MaxCount = _MaxCount - 1;
        pbVar7 = _Str1 + 1;
        if ((*(byte *)(uVar4 + 0x1d + local_14) & 4) == 0) {
          if ((*(byte *)(uVar4 + local_14 + 0x1d) & 0x10) != 0) {
            uVar4 = (uint)*(byte *)(uVar4 + local_14 + 0x11d);
          }
          local_8 = (ushort)uVar4;
          uVar6 = local_8;
        }
        else {
          bVar1 = *pbVar7;
          if (bVar1 == 0) {
            local_8 = 0;
            uVar6 = local_8;
          }
          else {
            pbVar7 = _Str1 + 2;
            uVar6 = CONCAT11(*_Str1,bVar1);
            if ((uVar6 < *(ushort *)(local_14 + 0x10)) || (*(ushort *)(local_14 + 0x12) < uVar6)) {
              if ((*(ushort *)(local_14 + 0x16) <= uVar6) && (uVar6 <= *(ushort *)(local_14 + 0x18))
                 ) {
                uVar6 = uVar6 + *(short *)(local_14 + 0x1a);
              }
            }
            else {
              uVar6 = uVar6 + *(short *)(local_14 + 0x14);
            }
          }
        }
        bVar1 = *_Str2;
        uVar4 = (uint)bVar1;
        puVar5 = _Str2 + 1;
        if ((*(byte *)(uVar4 + 0x1d + local_14) & 4) == 0) {
          if ((*(byte *)(uVar4 + local_14 + 0x1d) & 0x10) != 0) {
            uVar4 = (uint)*(byte *)(uVar4 + local_14 + 0x11d);
          }
          local_8 = (ushort)uVar4;
          _Str2 = puVar5;
        }
        else if (*puVar5 == '\0') {
          local_8 = 0;
          _Str2 = puVar5;
        }
        else {
          _Str2 = _Str2 + 2;
          local_8 = CONCAT11(bVar1,*puVar5);
          if ((local_8 < *(ushort *)(local_14 + 0x10)) || (*(ushort *)(local_14 + 0x12) < local_8))
          {
            if ((*(ushort *)(local_14 + 0x16) <= local_8) &&
               (local_8 <= *(ushort *)(local_14 + 0x18))) {
              local_8 = local_8 + *(short *)(local_14 + 0x1a);
            }
          }
          else {
            local_8 = local_8 + *(short *)(local_14 + 0x14);
          }
        }
        if (local_8 != uVar6) {
          iVar2 = (-(uint)(local_8 < uVar6) & 2) - 1;
          if (local_c == '\0') {
            return iVar2;
          }
          *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
          return iVar2;
        }
      } while ((uVar6 != 0) && (_Str1 = pbVar7, _MaxCount != 0));
      if (local_c != '\0') {
        *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
      }
      iVar2 = 0;
    }
  }
  return iVar2;
}


// 1001e60f __mbsnicmp

/* Library Function - Single Match
    __mbsnicmp
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsnicmp(uchar *_Str1,uchar *_Str2,size_t _MaxCount)

{
  int iVar1;
  
  iVar1 = __mbsnicmp_l(_Str1,_Str2,_MaxCount,(_locale_t)0x0);
  return iVar1;
}


// 1001e626 __putwch_nolock

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
  
  local_8 = DAT_10025040 ^ (uint)&stack0xfffffffc;
  if (DAT_10025bec != 0) {
    if (DAT_10025cc4 == (HANDLE)0xfffffffe) {
      ___initconout();
    }
    if (DAT_10025cc4 == (HANDLE)0xffffffff) goto LAB_1001e6cf;
    BVar2 = WriteConsoleW(DAT_10025cc4,&_WCh,1,&local_14,(LPVOID)0x0);
    if (BVar2 != 0) {
      DAT_10025bec = 1;
      goto LAB_1001e6cf;
    }
    if ((DAT_10025bec != 2) || (DVar3 = GetLastError(), DVar3 != 0x78)) goto LAB_1001e6cf;
    DAT_10025bec = 0;
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
  if (DAT_10025cc4 != (HANDLE)0xffffffff) {
    WriteConsoleA(DAT_10025cc4,local_10,DVar3,&local_14,(LPVOID)0x0);
  }
LAB_1001e6cf:
  wVar1 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return wVar1;
}


// 1001e6e8 __chsize_nolock

/* Library Function - Single Match
    __chsize_nolock
   
   Library: Visual Studio 2005 Release */

int __cdecl __chsize_nolock(int _FileHandle,longlong _Size)

{
  int iVar1;
  HANDLE pvVar2;
  LPVOID _Buf;
  DWORD *pDVar3;
  int iVar4;
  uint uVar5;
  BOOL BVar6;
  uint uVar7;
  int unaff_EDI;
  int iVar8;
  bool bVar9;
  bool bVar10;
  ulonglong uVar11;
  longlong lVar12;
  uint in_stack_00000008;
  DWORD DVar13;
  SIZE_T dwBytes;
  uint local_14;
  uint local_10;
  
  local_14 = 0;
  local_10 = 0;
  uVar11 = __lseeki64_nolock(_FileHandle,0x100000000,unaff_EDI);
  if (uVar11 == 0xffffffffffffffff) goto LAB_1001e76e;
  lVar12 = __lseeki64_nolock(_FileHandle,0x200000000,unaff_EDI);
  iVar4 = (int)((ulonglong)lVar12 >> 0x20);
  if (lVar12 == -1) goto LAB_1001e76e;
  uVar7 = in_stack_00000008 - (uint)lVar12;
  uVar5 = (uint)(in_stack_00000008 < (uint)lVar12);
  iVar1 = (int)_Size - iVar4;
  iVar8 = iVar1 - uVar5;
  if ((iVar8 < 0) ||
     ((iVar8 == 0 || (SBORROW4((int)_Size,iVar4) != SBORROW4(iVar1,uVar5)) != iVar8 < 0 &&
      (uVar7 == 0)))) {
    if ((iVar8 < 1) && (iVar8 < 0)) {
      lVar12 = __lseeki64_nolock(_FileHandle,_Size & 0xffffffff,unaff_EDI);
      if (lVar12 == -1) goto LAB_1001e76e;
      pvVar2 = (HANDLE)__get_osfhandle(_FileHandle);
      BVar6 = SetEndOfFile(pvVar2);
      local_14 = (BVar6 != 0) - 1;
      local_10 = (int)local_14 >> 0x1f;
      if ((local_14 & local_10) == 0xffffffff) {
        pDVar3 = FUN_100125a7();
        *pDVar3 = 0xd;
        pDVar3 = FUN_100125ba();
        DVar13 = GetLastError();
        *pDVar3 = DVar13;
        goto LAB_1001e86c;
      }
    }
  }
  else {
    dwBytes = 0x1000;
    DVar13 = 8;
    pvVar2 = GetProcessHeap();
    _Buf = HeapAlloc(pvVar2,DVar13,dwBytes);
    if (_Buf == (LPVOID)0x0) {
      pDVar3 = FUN_100125a7();
      *pDVar3 = 0xc;
      goto LAB_1001e76e;
    }
    iVar4 = __setmode_nolock(_FileHandle,0x8000);
    while( true ) {
      uVar5 = uVar7;
      if ((-1 < iVar8) && ((0 < iVar8 || (0xfff < uVar7)))) {
        uVar5 = 0x1000;
      }
      uVar5 = __write_nolock(_FileHandle,_Buf,uVar5);
      if (uVar5 == 0xffffffff) break;
      bVar9 = uVar7 < uVar5;
      uVar7 = uVar7 - uVar5;
      bVar10 = SBORROW4(iVar8,(int)uVar5 >> 0x1f);
      iVar1 = iVar8 - ((int)uVar5 >> 0x1f);
      iVar8 = iVar1 - (uint)bVar9;
      if ((iVar8 < 0) ||
         ((iVar8 == 0 || (bVar10 != SBORROW4(iVar1,(uint)bVar9)) != iVar8 < 0 && (uVar7 == 0))))
      goto LAB_1001e7c0;
    }
    pDVar3 = FUN_100125ba();
    if (*pDVar3 == 5) {
      pDVar3 = FUN_100125a7();
      *pDVar3 = 0xd;
    }
    local_14 = 0xffffffff;
    local_10 = 0xffffffff;
LAB_1001e7c0:
    __setmode_nolock(_FileHandle,iVar4);
    DVar13 = 0;
    pvVar2 = GetProcessHeap();
    HeapFree(pvVar2,DVar13,_Buf);
LAB_1001e86c:
    if ((local_14 & local_10) == 0xffffffff) goto LAB_1001e76e;
  }
  lVar12 = __lseeki64_nolock(_FileHandle,uVar11 >> 0x20,unaff_EDI);
  if (lVar12 != -1) {
    return 0;
  }
LAB_1001e76e:
  pDVar3 = FUN_100125a7();
  return *pDVar3;
}


// 1001e89c __setmode_nolock

/* Library Function - Single Match
    __setmode_nolock
   
   Library: Visual Studio 2005 Release */

int __cdecl __setmode_nolock(int _FileHandle,int _Mode)

{
  int *piVar1;
  byte bVar2;
  char cVar3;
  int iVar4;
  byte *pbVar5;
  byte bVar6;
  int iVar7;
  
  iVar7 = (_FileHandle & 0x1fU) * 0x38;
  piVar1 = &DAT_10029b40 + (_FileHandle >> 5);
  iVar4 = *piVar1 + iVar7;
  bVar2 = *(byte *)(iVar4 + 4);
  cVar3 = *(char *)(iVar4 + 0x24);
  if (_Mode == 0x4000) {
    *(byte *)(iVar4 + 4) = *(byte *)(iVar4 + 4) | 0x80;
    pbVar5 = (byte *)(*piVar1 + 0x24 + iVar7);
    *pbVar5 = *pbVar5 & 0x80;
  }
  else if (_Mode == 0x8000) {
    *(byte *)(iVar4 + 4) = *(byte *)(iVar4 + 4) & 0x7f;
  }
  else {
    if ((_Mode == 0x10000) || (_Mode == 0x20000)) {
      *(byte *)(iVar4 + 4) = *(byte *)(iVar4 + 4) | 0x80;
      pbVar5 = (byte *)(*piVar1 + 0x24 + iVar7);
      bVar6 = *pbVar5 & 0x82 | 2;
    }
    else {
      if (_Mode != 0x40000) goto LAB_1001e93b;
      *(byte *)(iVar4 + 4) = *(byte *)(iVar4 + 4) | 0x80;
      pbVar5 = (byte *)(*piVar1 + 0x24 + iVar7);
      bVar6 = *pbVar5 & 0x81 | 1;
    }
    *pbVar5 = bVar6;
  }
LAB_1001e93b:
  if ((bVar2 & 0x80) == 0) {
    iVar4 = 0x8000;
  }
  else {
    iVar4 = (-(uint)((char)(cVar3 * '\x02') >> 1 != '\0') & 0xc000) + 0x4000;
  }
  return iVar4;
}


// 1001e958 FUN_1001e958

undefined4 __cdecl FUN_1001e958(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if (param_1 == (undefined4 *)0x0) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  *param_1 = DAT_10029b30;
  return 0;
}


// 1001e98c ___ascii_stricmp

/* Library Function - Single Match
    ___ascii_stricmp
   
   Library: Visual Studio 2005 Release */

int __cdecl ___ascii_stricmp(char *_Str1,char *_Str2)

{
  uint uVar1;
  uint uVar2;
  
  do {
    uVar1 = (uint)(byte)*_Str1;
    _Str1 = (char *)((byte *)_Str1 + 1);
    if (uVar1 - 0x41 < 0x1a) {
      uVar1 = uVar1 + 0x20;
    }
    uVar2 = (uint)(byte)*_Str2;
    _Str2 = (char *)((byte *)_Str2 + 1);
    if (uVar2 - 0x41 < 0x1a) {
      uVar2 = uVar2 + 0x20;
    }
  } while ((uVar1 != 0) && (uVar1 == uVar2));
  return uVar1 - uVar2;
}


// 1001e9c1 __stricmp_l

/* Library Function - Single Match
    __stricmp_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __stricmp_l(char *_Str1,char *_Str2,_locale_t _Locale)

{
  DWORD *pDVar1;
  int iVar2;
  int iVar3;
  localeinfo_struct local_14;
  int local_c;
  char local_8;
  
  _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)&local_14,_Locale);
  if (_Str1 == (char *)0x0) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    iVar2 = 0x7fffffff;
  }
  else if (_Str2 == (char *)0x0) {
    pDVar1 = FUN_100125a7();
    *pDVar1 = 0x16;
    FUN_10012548((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    iVar2 = 0x7fffffff;
  }
  else {
    if ((local_14.locinfo)->lc_category[0].wlocale == (wchar_t *)0x0) {
      iVar2 = ___ascii_stricmp(_Str1,_Str2);
    }
    else {
      do {
        iVar2 = __tolower_l((uint)(byte)*_Str1,&local_14);
        _Str1 = _Str1 + 1;
        iVar3 = __tolower_l((uint)(byte)*_Str2,&local_14);
        _Str2 = (char *)((byte *)_Str2 + 1);
        if (iVar2 == 0) break;
      } while (iVar2 == iVar3);
      iVar2 = iVar2 - iVar3;
    }
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return iVar2;
}


// 1001ea94 ___set_fpsr_sse2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___set_fpsr_sse2
   
   Library: Visual Studio 2005 Release */

void __cdecl ___set_fpsr_sse2(uint param_1)

{
  if (DAT_10029c44 != 0) {
    if (((param_1 & 0x40) == 0) || (DAT_10025cb8 == 0)) {
      MXCSR = param_1 & 0xffffffbf;
    }
    else {
      MXCSR = param_1;
    }
  }
  return;
}


// 1001eb10 ___ascii_strnicmp

/* Library Function - Single Match
    ___ascii_strnicmp
   
   Library: Visual Studio 2005 Release */

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
      if (bVar2 != (byte)uVar3) goto LAB_1001eb61;
      _MaxCount = _MaxCount - 1;
    } while (_MaxCount != 0);
    _MaxCount = 0;
    bVar2 = (byte)(uVar3 >> 8);
    bVar5 = bVar2 < (byte)uVar3;
    if (bVar2 != (byte)uVar3) {
LAB_1001eb61:
      _MaxCount = 0xffffffff;
      if (!bVar5) {
        _MaxCount = 1;
      }
    }
  }
  return _MaxCount;
}


// 1001eb71 ___initconout

/* Library Function - Single Match
    ___initconout
   
   Library: Visual Studio 2005 Release */

void __cdecl ___initconout(void)

{
  DAT_10025cc4 = CreateFileA("CONOUT$",0x40000000,3,(LPSECURITY_ATTRIBUTES)0x0,3,0,(HANDLE)0x0);
  return;
}


// 1001ebd0 FUN_1001ebd0

void FUN_1001ebd0(void)

{
  _atexit((_func_4879 *)&LAB_1001ed00);
  return;
}


