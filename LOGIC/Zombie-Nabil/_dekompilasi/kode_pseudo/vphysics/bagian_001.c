// 10091c20 FUN_10091c20

void __cdecl FUN_10091c20(int param_1)

{
  int *piVar1;
  undefined4 *puVar2;
  
  if (param_1 != 0) {
    puVar2 = (undefined4 *)(param_1 + 4);
    piVar1 = *(int **)(param_1 + 4);
    while (piVar1 != (int *)0x0) {
      puVar2 = puVar2 + 1;
      if ((piVar1[5] & 0x10000000U) == 0) {
        FUN_100782a0(piVar1);
        FUN_100778b0(piVar1);
      }
      piVar1 = (int *)*puVar2;
    }
  }
  return;
}


// 10091c60 FUN_10091c60

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __cdecl FUN_10091c60(int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  int iVar3;
  int *piVar4;
  int *piVar5;
  undefined4 local_4;
  
  local_4 = 0;
  if (3 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_remove_extravertices: test f%d for extra vertices\n",
             *(undefined4 *)(param_1 + 0x4c));
  }
  if (*(int *)(param_1 + 0x34) != 0) {
    piVar4 = (int *)(*(int *)(param_1 + 0x34) + 4);
    iVar1 = *piVar4;
    while (iVar1 != 0) {
      *(uint *)(iVar1 + 0x14) = *(uint *)(iVar1 + 0x14) & 0xfeffffff;
      piVar5 = piVar4 + 1;
      piVar4 = piVar4 + 1;
      iVar1 = *piVar5;
    }
  }
  if (*(int *)(param_1 + 0x38) != 0) {
    puVar2 = (undefined4 *)(*(int *)(param_1 + 0x38) + 4);
    piVar4 = (int *)*puVar2;
    while (piVar4 != (int *)0x0) {
      puVar2 = puVar2 + 1;
      if (*piVar4 != 0) {
        piVar4 = (int *)(*piVar4 + 4);
        iVar1 = *piVar4;
        while (iVar1 != 0) {
          *(uint *)(iVar1 + 0x14) = *(uint *)(iVar1 + 0x14) | 0x1000000;
          piVar5 = piVar4 + 1;
          piVar4 = piVar4 + 1;
          iVar1 = *piVar5;
        }
      }
      piVar4 = (int *)*puVar2;
    }
  }
  iVar1 = *(int *)(param_1 + 0x34);
  if (iVar1 != 0) {
    iVar3 = *(int *)(iVar1 + 4);
    piVar4 = (int *)(iVar1 + 4);
    if (iVar3 != 0) {
      do {
        piVar5 = piVar4 + 1;
        if ((*(byte *)(iVar3 + 0x17) & 1) == 0) {
          _DAT_100f4868 = _DAT_100f4868 + 1;
          local_4 = 1;
          FUN_10076950(*(int **)(param_1 + 0x34),iVar3);
          FUN_100768b0(*(int **)(iVar3 + 0xc),param_1);
          iVar1 = FUN_10076e50(*(int **)(iVar3 + 0xc));
          piVar5 = piVar4;
          if (iVar1 == 0) {
            *(uint *)(iVar3 + 0x14) = *(uint *)(iVar3 + 0x14) | 0x8000000;
            FUN_100775a0(&DAT_100f42a0,iVar3);
            _DAT_100f4870 = _DAT_100f4870 + 1;
            if (1 < DAT_100f3a18) {
              _fprintf(DAT_100f419c,
                       "qh_remove_extravertices: v%d deleted because it\'s lost all ridges\n",
                       *(uint *)(iVar3 + 0x14) & 0xffffff);
            }
          }
          else if (2 < DAT_100f3a18) {
            _fprintf(DAT_100f419c,
                     "qh_remove_extravertices: v%d removed from f%d because it\'s lost all ridges\n"
                     ,*(uint *)(iVar3 + 0x14) & 0xffffff,*(undefined4 *)(param_1 + 0x4c));
          }
        }
        iVar3 = *piVar5;
        piVar4 = piVar5;
      } while (iVar3 != 0);
      return local_4;
    }
  }
  return 0;
}


// 10091df0 FUN_10091df0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10091df0(int *param_1,int param_2,int param_3)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  uint uVar4;
  int iVar5;
  bool bVar6;
  
  iVar5 = 0;
  iVar2 = FUN_10076c30((int *)*param_1,param_2);
  FUN_100771f0((int *)*param_1,iVar2);
  if (*param_1 != 0) {
    piVar3 = (int *)(*param_1 + 4);
    iVar1 = *piVar3;
    while (iVar1 != 0) {
      piVar3 = piVar3 + 1;
      if (iVar1 == param_3) {
        _DAT_100f4508 = _DAT_100f4508 + 1;
        if ((param_1[3] & 0x4000000U) != 0) {
          FUN_10090370((int)param_1);
        }
        FUN_10079860(param_1);
        if (DAT_100f3a18 < 2) {
          return;
        }
        _fprintf(DAT_100f419c,
                 "qh_renameridgevertex: ridge r%d deleted.  It contained both v%d and v%d\n",
                 param_1[3] & 0xffffff,*(uint *)(param_2 + 0x14) & 0xffffff,
                 *(uint *)(param_3 + 0x14) & 0xffffff);
        return;
      }
      if ((*(uint *)(iVar1 + 0x14) & 0xffffff) < (*(uint *)(param_3 + 0x14) & 0xffffff)) break;
      iVar5 = iVar5 + 1;
      iVar1 = *piVar3;
    }
  }
  FUN_100774f0(param_1,iVar5,param_3);
  uVar4 = iVar2 - iVar5 >> 0x1f;
  uVar4 = (iVar2 - iVar5 ^ uVar4) - uVar4 & 0x80000001;
  bVar6 = uVar4 == 0;
  if ((int)uVar4 < 0) {
    bVar6 = (uVar4 - 1 | 0xfffffffe) == 0xffffffff;
  }
  if (!bVar6) {
    if (2 < DAT_100f3a18) {
      _fprintf(DAT_100f419c,"qh_renameridgevertex: swapped the top and bottom of ridge r%d\n",
               param_1[3] & 0xffffff);
    }
    iVar5 = param_1[1];
    param_1[1] = param_1[2];
    param_1[2] = iVar5;
  }
  return;
}


// 10091f10 FUN_10091f10

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10091f10(int param_1,int param_2,int param_3,int param_4,int param_5)

{
  int iVar1;
  bool bVar2;
  int iVar3;
  int *piVar4;
  undefined4 *puVar5;
  int *piVar6;
  
  bVar2 = false;
  if (((1 < DAT_100f3a18) || ((*(uint *)(param_1 + 0x14) & 0xffffff) == DAT_100f41d0)) ||
     ((*(uint *)(param_2 + 0x14) & 0xffffff) == DAT_100f41d0)) {
    bVar2 = true;
  }
  if (param_3 != 0) {
    puVar5 = (undefined4 *)(param_3 + 4);
    piVar6 = *(int **)(param_3 + 4);
    while (piVar6 != (int *)0x0) {
      puVar5 = puVar5 + 1;
      FUN_10091df0(piVar6,param_1,param_2);
      piVar6 = (int *)*puVar5;
    }
  }
  if (param_4 == 0) {
    _DAT_100f4878 = _DAT_100f4878 + 1;
    if (bVar2) {
      FUN_10056200("qh_renamevertex: renamed v%d to v%d in several facets\n");
    }
    iVar3 = *(int *)(param_1 + 0xc);
    if (iVar3 != 0) {
      iVar1 = *(int *)(iVar3 + 4);
      piVar6 = (int *)(iVar3 + 4);
      while (iVar1 != 0) {
        FUN_10090cc0(iVar1);
        FUN_10076950(*(int **)(iVar1 + 0x34),param_1);
        iVar3 = FUN_10091c60(iVar1);
        piVar4 = piVar6 + 1;
        if (iVar3 != 0) {
          piVar4 = piVar6;
        }
        piVar6 = piVar4;
        iVar1 = *piVar4;
      }
    }
    if ((*(uint *)(param_1 + 0x14) & 0x8000000) == 0) {
      *(uint *)(param_1 + 0x14) = *(uint *)(param_1 + 0x14) | 0x8000000;
      FUN_100775a0(&DAT_100f42a0,param_1);
      return;
    }
  }
  else {
    iVar3 = FUN_10076e50(*(int **)(param_1 + 0xc));
    if (iVar3 == 2) {
      _DAT_100f4888 = _DAT_100f4888 + 1;
      if (bVar2) {
        FUN_10056200("qh_renamevertex: renamed v%d to v%d in oldfacet f%d\n");
      }
      if (*(int *)(param_1 + 0xc) != 0) {
        piVar6 = (int *)(*(int *)(param_1 + 0xc) + 4);
        iVar3 = *piVar6;
        while (iVar3 != 0) {
          piVar6 = piVar6 + 1;
          FUN_10076950(*(int **)(iVar3 + 0x34),param_1);
          iVar3 = *piVar6;
        }
      }
      *(uint *)(param_1 + 0x14) = *(uint *)(param_1 + 0x14) | 0x8000000;
      FUN_100775a0(&DAT_100f42a0,param_1);
      return;
    }
    _DAT_100f4880 = _DAT_100f4880 + 1;
    if ((bVar2) || (DAT_100f3a18 != 0)) {
      FUN_10056200("qh_renamevertex: renamed pinched v%d to v%d between f%d and f%d\n");
    }
    FUN_10076950(*(int **)(param_4 + 0x34),param_1);
    FUN_100768b0(*(int **)(param_1 + 0xc),param_4);
    FUN_10091c60(param_5);
  }
  return;
}


// 10092100 FUN_10092100

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __cdecl FUN_10092100(int param_1,int param_2)

{
  bool bVar1;
  bool bVar2;
  bool bVar3;
  undefined4 uVar4;
  float10 fVar5;
  double local_58;
  double local_50;
  double local_48 [8];
  
  local_50 = -1.7976931348623157e+308;
  local_58 = -1.7976931348623157e+308;
  bVar3 = false;
  bVar2 = false;
  if ((DAT_100f3b8c != 0) && (DAT_100f4260 == 0)) {
    return 0;
  }
  if (((DAT_100f3a40 == 0) || (DAT_100f4260 != 0)) && (_DAT_100f4038 < 8.988465674311579e+307)) {
    fVar5 = FUN_10082dd0(*(double **)(param_1 + 0x18),*(double **)(param_2 + 0x18));
    local_58 = (double)fVar5;
    _DAT_100f43e0 = _DAT_100f43e0 + 1;
    if ((float10)_DAT_100f4038 < fVar5) {
      _DAT_100f4488 = _DAT_100f4488 + 1;
      FUN_1008fe80(param_1,param_2,2,&local_58);
      if (1 < DAT_100f3a18) {
        _fprintf(DAT_100f419c,"qh_test_appendmerge: coplanar angle %4.4g between f%d and f%d\n",
                 SUB84(local_58,0),(int)((ulonglong)local_58 >> 0x20),
                 *(undefined4 *)(param_1 + 0x4c),*(undefined4 *)(param_2 + 0x4c));
      }
      return 1;
    }
    bVar2 = true;
  }
  if (*(int *)(param_1 + 0x28) == 0) {
    uVar4 = FUN_10083860(param_1);
    *(undefined4 *)(param_1 + 0x28) = uVar4;
  }
  _DAT_100f4430 = _DAT_100f4430 + 1;
  FUN_10081fc0(*(double **)(param_1 + 0x28),param_2,local_48);
  if (local_48[0] <= _DAT_100f4030) {
    bVar1 = local_48[0] <= -_DAT_100f4030;
    if (*(int *)(param_2 + 0x28) == 0) {
      uVar4 = FUN_10083860(param_2);
      *(undefined4 *)(param_2 + 0x28) = uVar4;
    }
    _DAT_100f4430 = _DAT_100f4430 + 1;
    FUN_10081fc0(*(double **)(param_2 + 0x28),param_1,&local_50);
    if (local_50 <= _DAT_100f4030) {
      if ((bVar1) && (local_50 <= -_DAT_100f4030)) {
        return 0;
      }
      if ((DAT_100f3a40 != 0) && (DAT_100f4260 == 0)) {
        return 0;
      }
    }
    else {
      bVar3 = true;
    }
  }
  else {
    bVar3 = true;
  }
  if ((!bVar2) && (DAT_100f39b4 != 0)) {
    fVar5 = FUN_10082dd0(*(double **)(param_1 + 0x18),*(double **)(param_2 + 0x18));
    local_58 = (double)fVar5;
    _DAT_100f43e0 = _DAT_100f43e0 + 1;
  }
  if (bVar3) {
    _DAT_100f4468 = _DAT_100f4468 + 1;
    if (DAT_100f39b4 != 0) {
      local_58 = local_58 + 2.0;
    }
    FUN_1008fe80(param_1,param_2,3,&local_58);
    if (DAT_100f3a18 != 0) {
      _fprintf(DAT_100f419c,
               "qh_test_appendmerge: concave f%d to f%d dist %4.4g and reverse dist %4.4g angle %4.4g during p%d\n"
               ,*(undefined4 *)(param_1 + 0x4c),*(undefined4 *)(param_2 + 0x4c),SUB84(local_48[0],0)
               ,(int)((ulonglong)local_48[0] >> 0x20),SUB84(local_50,0),
               (int)((ulonglong)local_50 >> 0x20),SUB84(local_58,0),
               (int)((ulonglong)local_58 >> 0x20),DAT_100f4210);
      return 1;
    }
  }
  else {
    _DAT_100f4490 = _DAT_100f4490 + 1;
    FUN_1008fe80(param_1,param_2,1,&local_58);
    if (1 < DAT_100f3a18) {
      _fprintf(DAT_100f419c,
               "qh_test_appendmerge: coplanar f%d to f%d dist %4.4g, reverse dist %4.4g angle %4.4g\n"
               ,*(undefined4 *)(param_1 + 0x4c),*(undefined4 *)(param_2 + 0x4c),SUB84(local_48[0],0)
               ,(int)((ulonglong)local_48[0] >> 0x20),SUB84(local_50,0),
               (int)((ulonglong)local_50 >> 0x20),SUB84(local_58,0),
               (int)((ulonglong)local_58 >> 0x20));
    }
  }
  return 1;
}


// 100923f0 FUN_100923f0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

bool FUN_100923f0(void)

{
  int iVar1;
  int iVar2;
  int *piVar3;
  int iVar4;
  int *piVar5;
  int local_4;
  
  iVar4 = 0;
  local_4 = 0;
  if (0 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_test_vneighbors: testing vertex neighbors for convexity\n");
  }
  iVar1 = DAT_100f41bc;
  if (DAT_100f4010 == 0) {
    FUN_1007a810();
    iVar1 = DAT_100f41bc;
  }
  while ((iVar2 = DAT_100f41bc, iVar1 != 0 && (*(int *)(iVar1 + 0x30) != 0))) {
    *(uint *)(iVar1 + 0x50) = *(uint *)(iVar1 + 0x50) & 0xffffdfff;
    iVar1 = *(int *)(iVar1 + 0x30);
  }
  for (; (iVar2 != 0 && (*(int *)(iVar2 + 0x30) != 0)); iVar2 = *(int *)(iVar2 + 0x30)) {
    *(uint *)(iVar2 + 0x50) = *(uint *)(iVar2 + 0x50) | 0x2000;
    *(int *)(iVar2 + 0x48) = DAT_100f4280;
    DAT_100f4280 = DAT_100f4280 + 1;
    if (*(int *)(iVar2 + 0x3c) != 0) {
      piVar3 = (int *)(*(int *)(iVar2 + 0x3c) + 4);
      iVar1 = *piVar3;
      while (iVar1 != 0) {
        piVar3 = piVar3 + 1;
        *(int *)(iVar2 + 0x48) = DAT_100f4280;
        iVar1 = *piVar3;
      }
    }
    if (*(int *)(iVar2 + 0x34) != 0) {
      piVar3 = (int *)(*(int *)(iVar2 + 0x34) + 4);
      iVar1 = *piVar3;
      while (iVar1 != 0) {
        piVar3 = piVar3 + 1;
        if (*(int *)(iVar1 + 0xc) != 0) {
          piVar5 = (int *)(*(int *)(iVar1 + 0xc) + 4);
          iVar4 = *piVar5;
          while (iVar4 != 0) {
            piVar5 = piVar5 + 1;
            if ((((*(uint *)(iVar4 + 0x50) & 0x2000) == 0) &&
                (*(int *)(iVar4 + 0x48) != DAT_100f4280)) &&
               (iVar4 = FUN_10092100(iVar2,iVar4), iVar4 != 0)) {
              local_4 = local_4 + 1;
            }
            iVar4 = *piVar5;
          }
        }
        iVar4 = local_4;
        iVar1 = *piVar3;
      }
    }
  }
  _DAT_100f4910 = _DAT_100f4910 + iVar4;
  if (0 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_test_vneighbors: found %d non-convex, vertex neighbors\n",iVar4);
  }
  return 0 < iVar4;
}


// 10092530 FUN_10092530

void __cdecl FUN_10092530(undefined4 *param_1,int *param_2)

{
  int *piVar1;
  undefined8 *puVar2;
  int unaff_ESI;
  int unaff_retaddr;
  undefined4 *puVar3;
  
  piVar1 = param_2;
  puVar3 = (undefined4 *)0x0;
  if (3 < DAT_100f3a18) {
    FUN_10080a90("MERGED",(undefined8 *)param_2,(undefined8 *)0x0,(int *)0x0,0);
  }
  if (piVar1 == DAT_100f41cc) {
LAB_10092579:
    FUN_10056200("qh_tracemerge: trace facet and vertex after merge of f%d and f%d, furthest p%d\n")
    ;
    if (piVar1 != DAT_100f41cc) {
      if ((DAT_100f41d4 == 0) || (*(int *)(DAT_100f41d4 + 0xc) == 0)) {
        puVar2 = (undefined8 *)0x0;
      }
      else {
        puVar2 = *(undefined8 **)(*(int *)(DAT_100f41d4 + 0xc) + 4);
      }
      FUN_10080a90("TRACE",(undefined8 *)DAT_100f41cc,puVar2,(int *)0x0,DAT_100f41d4);
    }
  }
  else {
    if (DAT_100f41d4 == 0) goto LAB_100925fd;
    if ((*(uint *)(DAT_100f41d4 + 0x14) & 0x10000000) != 0) goto LAB_10092579;
  }
  if (DAT_100f41d4 != 0) {
    if ((*(uint *)(DAT_100f41d4 + 0x14) & 0x8000000) == 0) {
      FUN_100796d0(DAT_100f41d4);
    }
    else {
      FUN_10056200("qh_tracemerge: trace vertex deleted at furthest p%d\n");
    }
  }
LAB_100925fd:
  if ((DAT_100f41cc != (int *)0x0) &&
     (FUN_1007ac30(DAT_100f41cc,1,&stack0xfffffffc,unaff_ESI,puVar3), puVar3 != (undefined4 *)0x0))
  {
    FUN_10080c90((undefined8 *)0x5,DAT_100f41cc);
  }
  if (((DAT_100f39dc != 0) || (3 < DAT_100f3a18)) &&
     (FUN_1007ac30(piVar1,1,&param_1,unaff_retaddr,param_1), param_1 != (undefined4 *)0x0)) {
    FUN_10080c90((undefined8 *)0x5,(int *)0x0);
  }
  return;
}


// 10092670 FUN_10092670

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_10092670(void)

{
  __time64_t local_8;
  
  DAT_100f42e4 = DAT_100f4928;
  __time64(&local_8);
  FID_conflict___localtime64((__time32_t *)&local_8);
  FUN_10076e50(DAT_100f42a0);
  FUN_10056200(
              "\nAt %d:%d:%d & %2.5g CPU secs, qhull has merged %d facets.  The hull\n  contains %d facets and %d vertices.\n"
              );
  return;
}


// 10092700 FUN_10092700

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10092700(int param_1,int param_2)

{
  int *piVar1;
  uint uVar2;
  int iVar3;
  int iVar4;
  int *piVar5;
  
  *(uint *)(param_2 + 0x50) = *(uint *)(param_2 + 0x50) & 0xfbffffff;
  if (*(int *)(param_1 + 0x38) != 0) {
    piVar5 = (int *)(*(int *)(param_1 + 0x38) + 4);
    iVar4 = *piVar5;
    while (iVar4 != 0) {
      *(uint *)(iVar4 + 0xc) = *(uint *)(iVar4 + 0xc) & 0xfdffffff;
      piVar1 = piVar5 + 1;
      piVar5 = piVar5 + 1;
      iVar4 = *piVar1;
    }
  }
  if (*(int *)(param_2 + 0x28) != 0) {
    iVar4 = FUN_10076e50(*(int **)(param_2 + 0x34));
    uVar2 = *(uint *)(param_2 + 0x50);
    if ((uVar2 & 0x8000000) == 0) {
      if (DAT_100f3bec + 5 < iVar4) {
        *(uint *)(param_2 + 0x50) = uVar2 | 0x8000000;
        _DAT_100f49b0 = _DAT_100f49b0 + 1;
      }
    }
    else if ((iVar4 <= DAT_100f3bec + 5) && ((iVar4 == DAT_100f3bec || (DAT_100f4260 != 0)))) {
      *(uint *)(param_2 + 0x50) = uVar2 & 0xf7ffffff;
    }
    if ((*(uint *)(param_2 + 0x50) & 0x8000000) == 0) {
      FUN_10080480(*(undefined4 **)(param_2 + 0x28),DAT_100f41a4);
      *(undefined4 *)(param_2 + 0x28) = 0;
      iVar4 = *(int *)(param_2 + 0x38);
      if (iVar4 != 0) {
        iVar3 = *(int *)(iVar4 + 4);
        while (iVar3 != 0) {
          *(uint *)(iVar3 + 0xc) = *(uint *)(iVar3 + 0xc) & 0xfdffffff;
          iVar3 = *(int *)(iVar4 + 8);
          iVar4 = iVar4 + 4;
        }
      }
    }
  }
  return;
}


// 100927e0 FUN_100927e0

void __cdecl FUN_100927e0(int param_1,int param_2,int *param_3)

{
  int iVar1;
  int *piVar2;
  undefined4 *puVar3;
  
  iVar1 = *(int *)(param_2 + 0x38);
  if (iVar1 == 0) {
    *(int *)(param_2 + 0x48) = DAT_100f4280 + -1;
    return;
  }
  piVar2 = *(int **)(iVar1 + 4);
  puVar3 = (undefined4 *)(iVar1 + 4);
  if (piVar2 == (int *)0x0) {
    *(int *)(param_2 + 0x48) = DAT_100f4280 + -1;
    return;
  }
  do {
    iVar1 = piVar2[1];
    puVar3 = puVar3 + 1;
    if (iVar1 == param_2) {
      iVar1 = piVar2[2];
    }
    if ((*(int *)(iVar1 + 0x48) == DAT_100f4280) &&
       (iVar1 = FUN_10076c00(*piVar2,param_1), iVar1 != 0)) {
      FUN_100775a0(param_3,(int)piVar2);
    }
    piVar2 = (int *)*puVar3;
  } while (piVar2 != (int *)0x0);
  *(int *)(param_2 + 0x48) = DAT_100f4280 + -1;
  return;
}


// 10092870 FUN_10092870

void __cdecl FUN_10092870(int param_1,undefined4 param_2)

{
  FUN_10078220(param_1);
  FUN_1007a3b0(param_1,&DAT_100f41c0);
  DAT_100f41c4 = DAT_100f41c4 + 1;
  *(uint *)(param_1 + 0x50) = *(uint *)(param_1 + 0x50) | 0x400;
  *(undefined4 *)(param_1 + 0x20) = param_2;
  return;
}


// 100928a0 FUN_100928a0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100928a0(int param_1)

{
  int *piVar1;
  int iVar2;
  uint uVar3;
  int iVar4;
  size_t _NumOfElements;
  int *piVar5;
  _PtFuncCompare *_PtFuncCompare;
  
  FUN_10076e50(DAT_100f4290);
  if (3 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_getmergeset: started.\n");
  }
  DAT_100f4280 = DAT_100f4280 + 1;
  if (param_1 != 0) {
    do {
      iVar2 = DAT_100f4280;
      if (*(int *)(param_1 + 0x30) == 0) break;
      if ((*(uint *)(param_1 + 0x50) & 0x4000000) == 0) {
        *(uint *)(param_1 + 0x50) = *(uint *)(param_1 + 0x50) | 0x4000000;
        *(int *)(param_1 + 0x48) = iVar2;
        if (*(int *)(param_1 + 0x3c) != 0) {
          piVar5 = (int *)(*(int *)(param_1 + 0x3c) + 4);
          iVar2 = *piVar5;
          while (iVar2 != 0) {
            *(uint *)(iVar2 + 0x50) = *(uint *)(iVar2 + 0x50) & 0xffffdfff;
            piVar1 = piVar5 + 1;
            piVar5 = piVar5 + 1;
            iVar2 = *piVar1;
          }
        }
        iVar2 = *(int *)(param_1 + 0x38);
        if (iVar2 != 0) {
          piVar5 = (int *)(iVar2 + 4);
          iVar2 = *(int *)(iVar2 + 4);
          while (iVar2 != 0) {
            uVar3 = *(uint *)(iVar2 + 0xc);
            piVar5 = piVar5 + 1;
            if (((uVar3 & 0x2000000) == 0) || ((uVar3 & 0x4000000) != 0)) {
              iVar4 = *(int *)(iVar2 + 4);
              if (iVar4 == param_1) {
                iVar4 = *(int *)(iVar2 + 8);
              }
              if ((*(uint *)(iVar4 + 0x50) & 0x2000) == 0) {
                if (*(int *)(iVar4 + 0x48) != DAT_100f4280) {
                  *(uint *)(iVar2 + 0xc) = uVar3 & 0xfbffffff | 0x2000000;
                  *(uint *)(iVar4 + 0x50) = *(uint *)(iVar4 + 0x50) | 0x2000;
                  iVar4 = FUN_10092100(param_1,iVar4);
                  if (iVar4 != 0) {
                    *(uint *)(iVar2 + 0xc) = *(uint *)(iVar2 + 0xc) | 0x4000000;
                  }
                }
              }
              else {
                *(uint *)(iVar2 + 0xc) = uVar3 & 0xfbffffff | 0x2000000;
              }
            }
            iVar2 = *piVar5;
          }
        }
      }
      param_1 = *(int *)(param_1 + 0x30);
    } while (param_1 != 0);
  }
  _NumOfElements = FUN_10076e50(DAT_100f4290);
  if (DAT_100f39b4 == 0) {
    _PtFuncCompare = (_PtFuncCompare *)&LAB_10090350;
  }
  else {
    _PtFuncCompare = (_PtFuncCompare *)&LAB_10090330;
  }
  _qsort(DAT_100f4290 + 1,_NumOfElements,4,_PtFuncCompare);
  if (DAT_100f4260 == 0) {
    _DAT_100f4718 = _DAT_100f4718 + _NumOfElements;
    if ((int)DAT_100f4720 < (int)_NumOfElements) {
      DAT_100f4720 = _NumOfElements;
    }
  }
  else {
    _DAT_100f4728 = _DAT_100f4728 + _NumOfElements;
  }
  if (1 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_getmergeset: %d merges found\n",_NumOfElements);
  }
  return;
}


// 10092a60 FUN_10092a60

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10092a60(int param_1)

{
  int iVar1;
  int iVar2;
  size_t _NumOfElements;
  int iVar3;
  int *piVar4;
  int *piVar5;
  _PtFuncCompare *_PtFuncCompare;
  
  DAT_100f4280 = DAT_100f4280 + 1;
  if (param_1 != 0) {
    do {
      iVar1 = DAT_100f4280;
      if (*(int *)(param_1 + 0x30) == 0) break;
      *(uint *)(param_1 + 0x50) = *(uint *)(param_1 + 0x50) | 0x4000000;
      *(int *)(param_1 + 0x48) = iVar1;
      iVar1 = *(int *)(param_1 + 0x3c);
      if (iVar1 != 0) {
        piVar5 = (int *)(iVar1 + 4);
        iVar1 = *(int *)(iVar1 + 4);
        while (iVar1 != 0) {
          piVar5 = piVar5 + 1;
          if (((*(int *)(iVar1 + 0x48) != DAT_100f4280) &&
              (iVar2 = FUN_10092100(param_1,iVar1), iVar2 != 0)) && (*(int *)(iVar1 + 0x38) != 0)) {
            piVar4 = (int *)(*(int *)(iVar1 + 0x38) + 4);
            iVar2 = *piVar4;
            while (iVar2 != 0) {
              iVar3 = *(int *)(iVar2 + 4);
              piVar4 = piVar4 + 1;
              if (iVar3 == iVar1) {
                iVar3 = *(int *)(iVar2 + 8);
              }
              if (param_1 == iVar3) {
                *(uint *)(iVar2 + 0xc) = *(uint *)(iVar2 + 0xc) | 0x4000000;
                break;
              }
              iVar2 = *piVar4;
            }
          }
          iVar1 = *piVar5;
        }
      }
      if (*(int *)(param_1 + 0x38) != 0) {
        piVar5 = (int *)(*(int *)(param_1 + 0x38) + 4);
        iVar1 = *piVar5;
        while (iVar1 != 0) {
          *(uint *)(iVar1 + 0xc) = *(uint *)(iVar1 + 0xc) | 0x2000000;
          piVar4 = piVar5 + 1;
          piVar5 = piVar5 + 1;
          iVar1 = *piVar4;
        }
      }
      param_1 = *(int *)(param_1 + 0x30);
    } while (param_1 != 0);
  }
  _NumOfElements = FUN_10076e50(DAT_100f4290);
  if (DAT_100f39b4 == 0) {
    _PtFuncCompare = (_PtFuncCompare *)&LAB_10090350;
  }
  else {
    _PtFuncCompare = (_PtFuncCompare *)&LAB_10090330;
  }
  _qsort(DAT_100f4290 + 1,_NumOfElements,4,_PtFuncCompare);
  if (DAT_100f4260 == 0) {
    _DAT_100f46f0 = _DAT_100f46f0 + _NumOfElements;
    if ((int)DAT_100f46f8 < (int)_NumOfElements) {
      DAT_100f46f8 = _NumOfElements;
    }
  }
  else {
    _DAT_100f4700 = _DAT_100f4700 + _NumOfElements;
  }
  if (1 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_getmergeset_initial: %d merges found\n",_NumOfElements);
  }
  return;
}


// 10092bc0 FUN_10092bc0

void __cdecl FUN_10092bc0(uint param_1,int param_2)

{
  uint uVar1;
  int iVar2;
  uint uVar3;
  
  if (3 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_mergecycle_facets: make newfacet new and samecycle deleted\n");
  }
  FUN_10078220(param_2);
  FUN_10077840(param_2);
  *(uint *)(param_2 + 0x50) = *(uint *)(param_2 + 0x50) & 0xffffefff | 0x10000200;
  for (uVar3 = *(uint *)(param_1 + 0x20); uVar3 != 0; uVar3 = -(uint)(uVar3 != param_1) & uVar1) {
    uVar1 = *(uint *)(uVar3 + 0x20);
    FUN_10092870(uVar3,param_2);
  }
  if ((*(int *)(param_2 + 0x28) != 0) &&
     (iVar2 = FUN_10076e50(*(int **)(param_2 + 0x34)), iVar2 <= DAT_100f3bec + 5)) {
    FUN_10080480(*(undefined4 **)(param_2 + 0x28),DAT_100f41a4);
    *(undefined4 *)(param_2 + 0x28) = 0;
  }
  if (2 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_mergecycle_facets: merged facets from cycle f%d into f%d\n",
             *(undefined4 *)(param_1 + 0x4c),*(undefined4 *)(param_2 + 0x4c));
  }
  return;
}


// 10092c90 FUN_10092c90

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10092c90(int *param_1,int param_2,int param_3)

{
  undefined4 *puVar1;
  bool bVar2;
  int *piVar3;
  int *piVar4;
  int iVar5;
  uint uVar6;
  int *piVar7;
  undefined4 *puVar8;
  int iVar9;
  int local_8;
  
  piVar3 = param_1;
  bVar2 = false;
  local_8 = -1;
  if (param_3 == 0) {
    _DAT_100f4730 = _DAT_100f4730 + 1;
    if (param_1[0xd] != 0) {
      piVar4 = (int *)(param_1[0xd] + 4);
      iVar9 = *piVar4;
      while (iVar9 != 0) {
        *(uint *)(iVar9 + 0x14) = *(uint *)(iVar9 + 0x14) & 0xfeffffff;
        piVar7 = piVar4 + 1;
        piVar4 = piVar4 + 1;
        iVar9 = *piVar7;
      }
    }
    if (param_1[0xe] != 0) {
      puVar8 = (undefined4 *)(param_1[0xe] + 4);
      piVar4 = (int *)*puVar8;
      while (piVar4 != (int *)0x0) {
        piVar7 = (int *)piVar4[1];
        puVar8 = puVar8 + 1;
        if (piVar7 == param_1) {
          piVar7 = (int *)piVar4[2];
        }
        if (piVar7 == (int *)param_2) {
          if (*piVar4 != 0) {
            piVar4 = (int *)(*piVar4 + 4);
            iVar9 = *piVar4;
            while (iVar9 != 0) {
              *(uint *)(iVar9 + 0x14) = *(uint *)(iVar9 + 0x14) | 0x5000000;
              piVar7 = piVar4 + 1;
              piVar4 = piVar4 + 1;
              iVar9 = *piVar7;
            }
          }
          break;
        }
        piVar4 = (int *)*puVar8;
      }
    }
    iVar9 = param_1[0xd];
    piVar4 = (int *)0x0;
    if (iVar9 != 0) {
      piVar7 = (int *)(iVar9 + 4);
      piVar4 = *(int **)(iVar9 + 4);
      while ((piVar4 != (int *)0x0 &&
             (piVar7 = piVar7 + 1, (*(byte *)((int)piVar4 + 0x17) & 1) != 0))) {
        piVar4 = (int *)*piVar7;
      }
    }
    if (3 < DAT_100f3a18) {
      _fprintf(DAT_100f419c,"qh_mergesimplex: merge apex v%d of f%d into facet f%d\n",
               piVar4[5] & 0xffffff,param_1[0x13],*(undefined4 *)(param_2 + 0x4c));
    }
    piVar7 = (int *)(param_2 + 0x34);
    if (*piVar7 != 0) {
      local_8 = 0;
      iVar9 = ((int *)*piVar7)[1];
      iVar5 = FUN_10076e50((int *)*piVar7);
      if (0 < iVar5) {
        do {
          uVar6 = *(uint *)(iVar9 + 0x14) & 0xffffff;
          if (uVar6 < (piVar4[5] & 0xffffffU)) break;
          if (uVar6 == (piVar4[5] & 0xffffffU)) {
            bVar2 = true;
            goto LAB_10092e34;
          }
          iVar9 = *(int *)(*piVar7 + 8 + local_8 * 4);
          local_8 = local_8 + 1;
        } while (local_8 < iVar5);
      }
    }
    FUN_100774f0(piVar7,local_8,piVar4);
LAB_10092e34:
    param_1 = piVar4;
    if ((*(uint *)(param_2 + 0x50) & 0x200) == 0) {
      FUN_10091c20(*piVar7);
    }
    else if ((piVar4[5] & 0x10000000U) == 0) {
      FUN_100782a0(piVar4);
      FUN_100778b0(piVar4);
    }
  }
  else {
    if ((*(uint *)(param_2 + 0x50) & 0x200) == 0) {
      FUN_10091c20(*(int *)(param_2 + 0x34));
    }
    param_1 = *(int **)(param_1[0xd] + 4);
    if (*(int **)(*(int *)(param_2 + 0x34) + 4) == param_1) {
      bVar2 = true;
    }
    else {
      FUN_100774f0((int *)(param_2 + 0x34),0,param_1);
    }
  }
  if (3 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_mergesimplex: update vertex neighbors of f%d\n",piVar3[0x13]);
  }
  iVar9 = piVar3[0xd];
  if (iVar9 != 0) {
    piVar4 = (int *)(iVar9 + 4);
    piVar7 = *(int **)(iVar9 + 4);
    while (piVar7 != (int *)0x0) {
      piVar4 = piVar4 + 1;
      if ((piVar7 != param_1) || (bVar2)) {
        FUN_100768b0((int *)piVar7[3],(int)piVar3);
        if (*(int *)(piVar7[3] + 8) == 0) {
          FUN_10091810((int)piVar7,(int)piVar3,param_2);
        }
      }
      else {
        FUN_10076de0((int *)piVar7[3],(int)piVar3,param_2);
      }
      piVar7 = (int *)*piVar4;
    }
  }
  if (3 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_mergesimplex: merge ridges and neighbors of f%d into f%d\n",
             piVar3[0x13],*(undefined4 *)(param_2 + 0x4c));
  }
  DAT_100f4280 = DAT_100f4280 + 1;
  if (*(int *)(param_2 + 0x3c) != 0) {
    piVar4 = (int *)(*(int *)(param_2 + 0x3c) + 4);
    iVar9 = *piVar4;
    while (iVar9 != 0) {
      piVar4 = piVar4 + 1;
      *(int *)(iVar9 + 0x48) = DAT_100f4280;
      iVar9 = *piVar4;
    }
  }
  iVar9 = piVar3[0xe];
  if (iVar9 != 0) {
    puVar8 = (undefined4 *)(iVar9 + 4);
    puVar1 = *(undefined4 **)(iVar9 + 4);
    while (puVar1 != (undefined4 *)0x0) {
      piVar4 = (int *)puVar1[1];
      puVar8 = puVar8 + 1;
      if (piVar4 == piVar3) {
        piVar4 = (int *)puVar1[2];
      }
      if (piVar4 == (int *)param_2) {
        FUN_100768b0(*(int **)(param_2 + 0x38),(int)puVar1);
        FUN_10076b70(puVar1);
        FUN_10080480(puVar1,0x10);
        FUN_100768b0(*(int **)(param_2 + 0x3c),(int)piVar3);
      }
      else {
        FUN_100775a0((int *)(param_2 + 0x38),(int)puVar1);
        if (piVar4[0x12] == DAT_100f4280) {
          if ((piVar4[0x14] & 0x1000U) != 0) {
            FUN_10090a10((int)piVar4);
          }
          piVar7 = (int *)piVar4[0xf];
          if ((int *)piVar7[1] == piVar3) {
            FUN_100768b0(piVar7,param_2);
            FUN_10076de0((int *)piVar4[0xf],(int)piVar3,param_2);
          }
          else {
            FUN_100768b0(piVar7,(int)piVar3);
          }
        }
        else {
          FUN_100775a0((int *)(param_2 + 0x3c),(int)piVar4);
          FUN_10076de0((int *)piVar4[0xf],(int)piVar3,param_2);
          piVar4[0x12] = DAT_100f4280;
        }
        if ((int *)puVar1[1] == piVar3) {
          puVar1[1] = param_2;
        }
        else {
          puVar1[2] = param_2;
        }
      }
      puVar1 = (undefined4 *)*puVar8;
    }
  }
  *(undefined4 *)(piVar3[0xe] + 4) = 0;
  if (2 < DAT_100f3a18) {
    if (param_1 == (int *)0x0) {
      uVar6 = 0xffffffff;
    }
    else {
      uVar6 = param_1[5] & 0xffffff;
    }
    _fprintf(DAT_100f419c,"qh_mergesimplex: merged simplex f%d apex v%d into facet f%d\n",
             piVar3[0x13],uVar6,*(undefined4 *)(param_2 + 0x4c));
  }
  return;
}


// 100930a0 FUN_100930a0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

int * __cdecl FUN_100930a0(int param_1)

{
  int iVar1;
  int *piVar2;
  int *local_4;
  
  local_4 = (int *)FUN_100776f0(DAT_100f41ac);
  DAT_100f4280 = DAT_100f4280 + 1;
  if (*(int *)(param_1 + 0xc) != 0) {
    piVar2 = (int *)(*(int *)(param_1 + 0xc) + 4);
    iVar1 = *piVar2;
    while (iVar1 != 0) {
      piVar2 = piVar2 + 1;
      *(int *)(iVar1 + 0x48) = DAT_100f4280;
      iVar1 = *piVar2;
    }
  }
  if (*(int *)(param_1 + 0xc) != 0) {
    piVar2 = (int *)(*(int *)(param_1 + 0xc) + 4);
    iVar1 = *piVar2;
    while (iVar1 != 0) {
      piVar2 = piVar2 + 1;
      if (*piVar2 != 0) {
        FUN_100927e0(param_1,iVar1,(int *)&local_4);
      }
      iVar1 = *piVar2;
    }
  }
  if ((DAT_100f3b30 != 0) || (DAT_100f3a18 != 0)) {
    iVar1 = FUN_10076e50(local_4);
    _DAT_100f4968 = _DAT_100f4968 + 1;
    _DAT_100f4970 = _DAT_100f4970 + iVar1;
    if (DAT_100f4978 < iVar1) {
      DAT_100f4978 = iVar1;
    }
    if (2 < DAT_100f3a18) {
      _fprintf(DAT_100f419c,"qh_vertexridges: found %d ridges for v%d\n",iVar1,
               *(uint *)(param_1 + 0x14) & 0xffffff);
    }
  }
  return local_4;
}


// 10093180 FUN_10093180

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

uint __cdecl FUN_10093180(uint param_1,int *param_2,int *param_3)

{
  uint *_Base;
  int iVar1;
  size_t _NumOfElements;
  uint uVar2;
  int *piVar3;
  int iVar4;
  uint uVar5;
  int *piVar6;
  int *piVar7;
  int iVar8;
  size_t _SizeOfElements;
  _PtFuncCompare *_PtFuncCompare;
  int *local_c;
  uint *local_8;
  uint local_4;
  
  if (3 < DAT_100f3a18) {
    FUN_10056200("qh_find_newvertex: find new vertex for v%d from ");
    if (param_2 != (int *)0x0) {
      piVar7 = param_2 + 1;
      iVar1 = param_2[1];
      while (iVar1 != 0) {
        piVar7 = piVar7 + 1;
        FUN_10056200("v%d ");
        iVar1 = *piVar7;
      }
    }
    if (param_3 != (int *)0x0) {
      piVar7 = param_3 + 1;
      iVar1 = param_3[1];
      while (iVar1 != 0) {
        piVar7 = piVar7 + 1;
        FUN_10056200("r%d ");
        iVar1 = *piVar7;
      }
    }
    FUN_10056200("\n");
  }
  if (param_2 != (int *)0x0) {
    piVar7 = param_2 + 1;
    iVar1 = param_2[1];
    while (iVar1 != 0) {
      piVar7 = piVar7 + 1;
      *(undefined4 *)(iVar1 + 0x10) = 0;
      iVar1 = *piVar7;
    }
  }
  if (param_3 != (int *)0x0) {
    piVar7 = param_3 + 1;
    piVar6 = (int *)param_3[1];
    while (piVar6 != (int *)0x0) {
      piVar7 = piVar7 + 1;
      if (*piVar6 != 0) {
        piVar6 = (int *)(*piVar6 + 4);
        iVar1 = *piVar6;
        while (iVar1 != 0) {
          *(int *)(iVar1 + 0x10) = *(int *)(iVar1 + 0x10) + 1;
          piVar3 = piVar6 + 1;
          piVar6 = piVar6 + 1;
          iVar1 = *piVar3;
        }
      }
      piVar6 = (int *)*piVar7;
    }
  }
  if (param_2 != (int *)0x0) {
    iVar1 = param_2[1];
    if (iVar1 != 0) {
      piVar7 = param_2 + 1;
      iVar4 = (int)(param_2 + 1) + (-8 - (int)param_2);
      do {
        piVar6 = piVar7 + 1;
        iVar8 = iVar4 + 4;
        if (*(int *)(iVar1 + 0x10) == 0) {
          FUN_10077180(param_2,iVar4 + 4 >> 2);
          piVar6 = piVar7;
          iVar8 = iVar4;
        }
        iVar1 = *piVar6;
        piVar7 = piVar6;
        iVar4 = iVar8;
      } while (iVar1 != 0);
    }
  }
  iVar1 = FUN_10076e50(param_3);
  DAT_100f4284 = DAT_100f4284 + iVar1;
  iVar1 = FUN_10076e50(param_2);
  if (iVar1 == 0) {
    if (3 < DAT_100f3a18) {
      _fprintf(DAT_100f419c,"qh_find_newvertex: vertices not in ridges for v%d\n",
               *(uint *)(param_1 + 0x14) & 0xffffff);
    }
    return 0;
  }
  _PtFuncCompare = (_PtFuncCompare *)&LAB_10090350;
  _SizeOfElements = 4;
  _Base = (uint *)(param_2 + 1);
  _NumOfElements = FUN_10076e50(param_2);
  _qsort(_Base,_NumOfElements,_SizeOfElements,_PtFuncCompare);
  if (DAT_100f3b30 != 0) {
    iVar1 = FUN_10076e50(param_2);
    _DAT_100f4660 = _DAT_100f4660 + 1;
    _DAT_100f4680 = _DAT_100f4680 + iVar1;
    if (DAT_100f4670 < iVar1) {
      DAT_100f4670 = iVar1;
    }
  }
  iVar1 = FUN_10076e50(param_3);
  uVar2 = FUN_10079f50(iVar1);
  if (param_3 != (int *)0x0) {
    piVar7 = param_3 + 1;
    piVar6 = (int *)param_3[1];
    while (piVar6 != (int *)0x0) {
      piVar7 = piVar7 + 1;
      FUN_10090910(DAT_100f4298,uVar2,piVar6,param_1);
      piVar6 = (int *)*piVar7;
    }
  }
  uVar5 = 0;
  if (param_2 != (int *)0x0) {
    uVar5 = *_Base;
    while (uVar5 != 0) {
      local_8 = _Base + 1;
      local_c = FUN_100930a0(uVar5);
      piVar7 = (int *)0x0;
      if (local_c != (int *)0x0) {
        piVar6 = local_c + 1;
        piVar7 = (int *)local_c[1];
        while (piVar7 != (int *)0x0) {
          piVar6 = piVar6 + 1;
          piVar3 = FUN_10090960(DAT_100f4298,uVar2,piVar7,uVar5,param_1,&local_4);
          if (piVar3 != (int *)0x0) {
            _DAT_100f45f0 = _DAT_100f45f0 + 1;
            break;
          }
          piVar7 = (int *)*piVar6;
        }
      }
      FUN_100777d0((int *)&local_c);
      if (piVar7 == (int *)0x0) {
        if (uVar5 != 0) {
          if (1 < DAT_100f3a18) {
            iVar1 = FUN_10076e50(param_3);
            iVar4 = FUN_10076e50(param_2);
            _fprintf(DAT_100f419c,
                     "qh_find_newvertex: found v%d for old v%d from %d vertices and %d ridges.\n",
                     *(uint *)(uVar5 + 0x14) & 0xffffff,*(uint *)(param_1 + 0x14) & 0xffffff,iVar4,
                     iVar1);
          }
          goto LAB_10093465;
        }
        break;
      }
      _Base = local_8;
      uVar5 = *local_8;
    }
  }
  _DAT_100f4600 = _DAT_100f4600 + 1;
  if (DAT_100f3a18 != 0) {
    _fprintf(DAT_100f419c,
             "qh_find_newvertex: no vertex for renaming v%d (all duplicated ridges) during p%d\n",
             *(uint *)(param_1 + 0x14) & 0xffffff,DAT_100f4210);
  }
LAB_10093465:
  FUN_10076b70(&DAT_100f4298);
  return uVar5;
}


// 100934e0 FUN_100934e0

void __cdecl FUN_100934e0(int *param_1,int *param_2)

{
  int *piVar1;
  int iVar2;
  bool bVar3;
  int local_4;
  
  local_4 = 0;
  if (DAT_100f4010 == 0) {
    FUN_1007a810();
  }
  DAT_100f4928 = DAT_100f4928 + 1;
  if (((DAT_100f3b70 != 0) && (DAT_100f4260 != 0)) && (DAT_100f42e4 + DAT_100f3b70 < DAT_100f4928))
  {
    FUN_10092670();
  }
  if (DAT_100f3bb8 == DAT_100f4928) {
    DAT_100f3a18 = DAT_100f3ba0;
    DAT_100f4340 = DAT_100f3ba0;
  }
  if (1 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,
             "qh_mergecycle: merge #%d for facets from cycle f%d into coplanar horizon f%d\n",
             DAT_100f4928,param_1[0x13],param_2[0x13]);
  }
  iVar2 = DAT_100f3a18;
  bVar3 = param_2 == DAT_100f41cc;
  if (bVar3) {
    DAT_100f3a18 = 4;
    local_4 = iVar2;
    FUN_10056200(
                "qh_mergecycle: ========= trace merge %d of samecycle %d into trace f%d, furthest is p%d\n"
                );
  }
  if (3 < DAT_100f3a18) {
    FUN_10056200("  same cycle:");
    for (piVar1 = (int *)param_1[8];
        (piVar1 != (int *)0x0 && (FUN_10056200(" f%d"), piVar1 != param_1));
        piVar1 = (int *)piVar1[8]) {
    }
    FUN_10056200("\n");
    if (3 < DAT_100f3a18) {
      FUN_10080a90("MERGING CYCLE",(undefined8 *)param_1,(undefined8 *)param_2,(int *)0x0,0);
    }
  }
  iVar2 = *(int *)(param_1[0xd] + 4);
  FUN_10090a10((int)param_2);
  FUN_10090e60(param_1,(int)param_2);
  FUN_100910a0((int)param_1,(int)param_2);
  FUN_10091370((int)param_1,(int)param_2);
  if (*(int *)(param_2[0xd] + 4) != iVar2) {
    FUN_100774f0(param_2 + 0xd,0,iVar2);
  }
  if ((param_2[0x14] & 0x200U) == 0) {
    FUN_10091c20(param_2[0xd]);
  }
  FUN_10092bc0((uint)param_1,(int)param_2);
  FUN_10092530(param_1,param_2);
  if (bVar3) {
    FUN_10056200("qh_mergecycle: end of trace facet\n");
    DAT_100f3a18 = local_4;
  }
  return;
}


// 100936b0 FUN_100936b0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100936b0(int *param_1,int *param_2,double *param_3,double *param_4)

{
  uint uVar1;
  uint uVar2;
  int iVar3;
  int *piVar4;
  int in_stack_00000020;
  
  DAT_100f4928 = DAT_100f4928 + 1;
  if (((DAT_100f3b70 != 0) && (DAT_100f4260 != 0)) && (DAT_100f42e4 + DAT_100f3b70 < DAT_100f4928))
  {
    FUN_10092670();
  }
  if (DAT_100f3b74 <= DAT_100f4208) {
    if ((param_3 == (double *)0x0) || ((-*param_3 <= _DAT_100f3bb0 && (*param_4 <= _DAT_100f3bb0))))
    {
      if ((param_1 == DAT_100f41cc) || (param_2 == DAT_100f41cc)) {
        DAT_100f3a18 = (int *)0x4;
        FUN_10056200("qh_mergefacet: ========= trace merge #%d involving f%d, furthest is p%d\n");
      }
    }
    else {
      DAT_100f3a18 = DAT_100f3ba0;
      FUN_10056200(
                  "qh_mergefacet: ========= trace wide merge #%d (%2.2g) for f%d into f%d, last point was p%d\n"
                  );
    }
  }
  if (1 < (int)DAT_100f3a18) {
    FUN_10056200("qh_mergefacet: #%d merge f%d into f%d, mindist= %2.2g, maxdist= %2.2g\n");
  }
  if (((param_1 == param_2) || ((param_1[0x14] & 0x400U) != 0)) || ((param_2[0x14] & 0x400U) != 0))
  {
    FUN_10056200(
                "qhull internal error (qh_mergefacet): either f%d and f%d are the same or one is a visible facet\n"
                );
    FUN_10086dd0(5,param_1,param_2);
  }
  if (DAT_100f41e4 - DAT_100f41c4 <= DAT_100f3bec + 1) {
    FUN_10056200(
                "\nqhull precision error: Only %d facets remain.  Can not merge another\npair.  The convexity constraints may be too strong.  Reduce the\nmagnitude of \'Cn\' or increase the magnitude of \'An\'.  For example,\ntry \'C-0.001\' instead of \'C-0.1\' or \'A-0.999\' instead of \'A-0.9\'.\n"
                );
    if ((4 < DAT_100f3bec) && (DAT_100f3a40 == 0)) {
      FUN_10056200("Option \'Qx\' may avoid this problem.\n");
    }
    FUN_10080c90((undefined8 *)0x1,(int *)0x0);
  }
  if (DAT_100f4010 == 0) {
    FUN_1007a810();
  }
  FUN_10090a10((int)param_1);
  FUN_10090a10((int)param_2);
  if (3 < (int)DAT_100f3a18) {
    FUN_10080a90("MERGING",(undefined8 *)param_1,(undefined8 *)param_2,(int *)0x0,0);
  }
  if (param_3 != (double *)0x0) {
    if (_DAT_100f4228 < *param_4) {
      _DAT_100f4228 = *param_4;
    }
    if (_DAT_100f4230 < *param_4) {
      _DAT_100f4230 = *param_4;
    }
    if (*(double *)(param_2 + 2) < *param_4) {
      *(double *)(param_2 + 2) = *param_4;
    }
    if (*param_3 < _DAT_100f4238) {
      _DAT_100f4238 = *param_3;
    }
    if (((param_2[0x14] & 0x8000000U) == 0) &&
       ((_DAT_100f40b0 < *param_4 || (*param_3 < -_DAT_100f40b0)))) {
      param_2[0x14] = param_2[0x14] | 0x8000000;
      _DAT_100f49a8 = _DAT_100f49a8 + 1;
    }
  }
  uVar2 = param_2[0x14];
  uVar1 = (param_1[0x14] & 0x1ffU) + 1 + (uVar2 & 0x1ff);
  if (uVar1 < 0x1ff) {
    param_2[0x14] = (uVar2 ^ uVar1) & 0x1ff ^ uVar2;
  }
  else {
    param_2[0x14] = uVar2 | 0x1ff;
  }
  param_2[0x14] = param_2[0x14] & 0xffefffffU | 0x10000000;
  FUN_10092700((int)param_1,(int)param_2);
  if ((DAT_100f3bec < 3) || (iVar3 = FUN_10076e50((int *)param_1[0xd]), iVar3 != DAT_100f3bec)) {
    DAT_100f4284 = DAT_100f4284 + 1;
    if (param_2[0xd] != 0) {
      piVar4 = (int *)(param_2[0xd] + 4);
      iVar3 = *piVar4;
      while (iVar3 != 0) {
        piVar4 = piVar4 + 1;
        *(int *)(iVar3 + 0x10) = DAT_100f4284;
        iVar3 = *piVar4;
      }
    }
    if (DAT_100f3bec == 2) {
      FUN_100914e0((int)param_1,(int)param_2);
    }
    else {
      FUN_10091630((int)param_1,(int)param_2);
      FUN_10091980((int *)param_1[0xd],param_2 + 0xd);
    }
    FUN_10091740((int)param_1,(int)param_2);
    FUN_10091880((int)param_1,(int)param_2);
    if ((param_2[0x14] & 0x200U) == 0) {
      FUN_10091c20(param_2[0xd]);
    }
  }
  else {
    FUN_10092c90(param_1,(int)param_2,in_stack_00000020);
  }
  if (in_stack_00000020 == 0) {
    FUN_10090520((int)param_2,(int)param_1);
  }
  if (((param_2[0x14] & 0x800000U) == 0) && ((param_2[0x14] & 0x200U) != 0)) {
    if ((param_1[0x14] & 0x200U) == 0) {
      _DAT_100f46e8 = _DAT_100f46e8 + 1;
    }
    else {
      _DAT_100f4710 = _DAT_100f4710 + 1;
    }
  }
  else {
    _DAT_100f4708 = _DAT_100f4708 + 1;
  }
  FUN_10092870((int)param_1,param_2);
  FUN_10078220((int)param_2);
  FUN_10077840((int)param_2);
  param_2[0x14] = param_2[0x14] & 0xfbffffffU | 0x200;
  FUN_10092530(param_1,param_2);
  if (param_1 != (int *)0x0) {
    FUN_10056200("qh_mergefacet: end of wide tracing\n");
    DAT_100f3a18 = param_2;
  }
  return;
}


// 10093b20 FUN_10093b20

uint __cdecl FUN_10093b20(int *param_1)

{
  uint uVar1;
  int *local_8;
  int *local_4;
  
  uVar1 = 0;
  if (2 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_redundant_vertex: check if v%d can be renamed\n",param_1[5] & 0xffffff
            );
  }
  local_4 = FUN_10091af0(param_1);
  if (local_4 != (int *)0x0) {
    local_8 = FUN_100930a0((int)param_1);
    uVar1 = FUN_10093180((uint)param_1,local_4,local_8);
    if (uVar1 != 0) {
      FUN_10091f10((int)param_1,uVar1,(int)local_8,0,0);
    }
    FUN_100777d0((int *)&local_8);
    FUN_100777d0((int *)&local_4);
  }
  return uVar1;
}


// 10093bc0 FUN_10093bc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

uint __cdecl FUN_10093bc0(int *param_1,undefined8 *param_2,int *param_3)

{
  int *piVar1;
  undefined8 *puVar2;
  int iVar3;
  undefined8 *puVar4;
  int iVar5;
  int *piVar6;
  uint uVar7;
  undefined8 *puVar8;
  bool bVar9;
  undefined4 uVar10;
  undefined4 uVar11;
  
  piVar1 = param_1;
  puVar8 = (undefined8 *)0x0;
  iVar3 = FUN_10076e50((int *)param_1[3]);
  puVar2 = param_2;
  if (iVar3 == 2) {
    puVar8 = *(undefined8 **)(piVar1[3] + 4);
    if (puVar8 == param_2) {
      puVar8 = *(undefined8 **)(piVar1[3] + 8);
    }
  }
  else {
    if (DAT_100f3bec == 3) {
      return 0;
    }
    DAT_100f4280 = DAT_100f4280 + 1;
    if (*(int *)((int)param_2 + 0x3c) != 0) {
      piVar6 = (int *)(*(int *)((int)param_2 + 0x3c) + 4);
      iVar3 = *piVar6;
      while (iVar3 != 0) {
        piVar6 = piVar6 + 1;
        *(int *)(iVar3 + 0x48) = DAT_100f4280;
        iVar3 = *piVar6;
      }
    }
    iVar3 = piVar1[3];
    if (iVar3 != 0) {
      puVar4 = *(undefined8 **)(iVar3 + 4);
      piVar6 = (int *)(iVar3 + 4);
      if (puVar4 != (undefined8 *)0x0) {
        do {
          piVar6 = piVar6 + 1;
          if ((*(int *)(puVar4 + 9) == DAT_100f4280) &&
             (bVar9 = puVar8 != (undefined8 *)0x0, puVar8 = puVar4, bVar9)) {
            return 0;
          }
          puVar4 = (undefined8 *)*piVar6;
        } while (puVar4 != (undefined8 *)0x0);
        if (puVar8 != (undefined8 *)0x0) goto LAB_10093c9e;
      }
    }
    FUN_10056200("qhull internal error (qh_rename_sharedvertex): v%d\'s neighbors not in f%d\n");
    FUN_10080a90("ERRONEOUS",puVar2,(undefined8 *)0x0,(int *)0x0,(int)piVar1);
    FUN_10080c90((undefined8 *)0x5,(int *)0x0);
  }
LAB_10093c9e:
  param_3 = (int *)FUN_100776f0(DAT_100f41ac);
  DAT_100f4280 = DAT_100f4280 + 1;
  *(int *)(puVar8 + 9) = DAT_100f4280;
  FUN_100927e0((int)piVar1,(int)puVar2,(int *)&param_3);
  if (1 < DAT_100f3a18) {
    uVar11 = *(undefined4 *)((int)puVar8 + 0x4c);
    iVar3 = FUN_10076e50(param_3);
    uVar10 = *(undefined4 *)((int)puVar2 + 0x4c);
    uVar7 = piVar1[5] & 0xffffff;
    iVar5 = FUN_100781b0(piVar1[2]);
    _fprintf(DAT_100f419c,"qh_rename_sharedvertex: p%d (v%d) is shared by f%d (%d ridges) and f%d\n"
             ,iVar5,uVar7,uVar10,iVar3,uVar11);
  }
  _DAT_100f4678 = _DAT_100f4678 + 1;
  param_1 = (int *)FUN_1007a790(*(undefined4 *)((int)puVar2 + 0x34),
                                *(undefined4 *)((int)puVar8 + 0x34));
  FUN_100768b0(param_1,(int)piVar1);
  FUN_10077740(param_1);
  uVar7 = FUN_10093180((uint)piVar1,param_1,param_3);
  if (uVar7 != 0) {
    FUN_10091f10((int)piVar1,uVar7,(int)param_3,(int)puVar2,(int)puVar8);
  }
  FUN_100777d0((int *)&param_1);
  FUN_100777d0((int *)&param_3);
  return uVar7;
}


// 10093d90 FUN_10093d90

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10093d90(undefined4 *param_1)

{
  double dVar1;
  int iVar2;
  uint uVar3;
  undefined4 *puVar4;
  int *piVar5;
  int *piVar6;
  int iVar7;
  float10 fVar8;
  int *local_80;
  int local_7c;
  int local_78;
  int *local_74;
  double dStack_70;
  double dStack_68;
  double local_60;
  double local_58;
  double dStack_50;
  double adStack_48 [8];
  
  iVar7 = 0;
  local_7c = 0;
  local_78 = 0;
  if (DAT_100f3bb8 + -1 == DAT_100f4928) {
    DAT_100f3a18 = DAT_100f3ba0;
    DAT_100f4340 = DAT_100f3ba0;
  }
  if (3 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_forcedmerges: begin\n");
  }
  local_80 = FUN_10076f00();
  DAT_100f4290 = FUN_100776f0(DAT_100f41ac);
  FUN_10077740(local_80);
  if (local_80 != (int *)0x0) {
    iVar2 = local_80[1];
    piVar6 = local_80 + 1;
    while (iVar2 != 0) {
      local_74 = piVar6 + 1;
      dVar1 = _DAT_100f45e0;
      if (*(int *)(iVar2 + 0x10) == 5) {
        piVar6 = *(int **)(iVar2 + 8);
        piVar5 = *(int **)(iVar2 + 0xc);
        uVar3 = piVar6[0x14];
        while ((uVar3 & 0x400) != 0) {
          piVar6 = (int *)piVar6[8];
          uVar3 = piVar6[0x14];
        }
        uVar3 = piVar5[0x14];
        while ((uVar3 & 0x400) != 0) {
          piVar5 = (int *)piVar5[8];
          uVar3 = piVar5[0x14];
        }
        if (piVar6 != piVar5) {
          iVar7 = FUN_10076c00(piVar5[0xf],(int)piVar6);
          if (iVar7 == 0) {
            FUN_10056200(
                        "qhull internal error (qh_forcedmerges): f%d and f%d had a duplicate ridge but as f%d and f%d they are no longer neighbors\n"
                        );
            FUN_10086dd0(5,piVar6,piVar5);
          }
          if (DAT_100f3bb8 + -1 == DAT_100f4928) {
            DAT_100f3a18 = DAT_100f3ba0;
            DAT_100f4340 = DAT_100f3ba0;
          }
          fVar8 = (float10)FUN_10082f10((int)piVar6,(int)piVar5,&local_58,&local_60);
          dStack_70 = (double)fVar8;
          fVar8 = (float10)FUN_10082f10((int)piVar5,(int)piVar6,adStack_48,&dStack_50);
          dStack_68 = (double)fVar8;
          if (DAT_100f3a18 != 0) {
            _fprintf(DAT_100f419c,
                     "qh_forcedmerges: duplicate ridge between f%d and f%d, dist %2.2g and reverse dist %2.2g during p%d\n"
                     ,piVar6[0x13],piVar5[0x13],dStack_70,(double)fVar8,DAT_100f4210);
            fVar8 = (float10)dStack_68;
          }
          if (fVar8 <= (float10)dStack_70) {
            FUN_100936b0(piVar5,piVar6,adStack_48,&dStack_50);
            dVar1 = dStack_68;
          }
          else {
            FUN_100936b0(piVar6,piVar5,&local_58,&local_60);
            piVar5 = piVar6;
            dVar1 = dStack_70;
          }
          if ((piVar5[0x14] & 0x8000U) == 0) {
            local_7c = local_7c + 1;
          }
          else {
            _DAT_100f46e0 = _DAT_100f46e0 + 1;
            local_78 = local_78 + 1;
          }
          if (DAT_100f3b30 != 0) {
            _DAT_100f45d8 = _DAT_100f45d8 + 1;
            _DAT_100f45e8 = _DAT_100f45e8 + dVar1;
            if (_DAT_100f45e0 < dVar1) goto LAB_10093fc5;
          }
          dVar1 = _DAT_100f45e0;
        }
      }
LAB_10093fc5:
      _DAT_100f45e0 = dVar1;
      iVar7 = local_7c;
      piVar6 = local_74;
      iVar2 = *local_74;
    }
    if (local_80 != (int *)0x0) {
      piVar6 = local_80 + 1;
      puVar4 = (undefined4 *)*piVar6;
      while (puVar4 != (undefined4 *)0x0) {
        piVar6 = piVar6 + 1;
        if (puVar4[4] == 5) {
          FUN_10080480(puVar4,0x18);
        }
        else {
          FUN_100775a0(&DAT_100f4290,(int)puVar4);
        }
        puVar4 = (undefined4 *)*piVar6;
      }
    }
  }
  FUN_100777d0((int *)&local_80);
  if (iVar7 != 0) {
    *param_1 = 1;
  }
  if (0 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,
             "qh_forcedmerges: merged %d facets and %d flipped facets across duplicated ridges\n");
  }
  return;
}


// 10094060 FUN_10094060

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

int FUN_10094060(void)

{
  int *piVar1;
  uint uVar2;
  undefined4 *puVar3;
  int iVar4;
  int *piVar5;
  int iVar6;
  double *unaff_ESI;
  double *unaff_EDI;
  double local_58;
  double local_50;
  double local_48 [8];
  
  iVar6 = 0;
  puVar3 = (undefined4 *)FUN_10076910(DAT_100f4294);
  while (puVar3 != (undefined4 *)0x0) {
    piVar1 = (int *)puVar3[2];
    piVar5 = (int *)puVar3[3];
    iVar4 = puVar3[4];
    FUN_10080480(puVar3,0x18);
    if ((piVar1[0x14] & 0x400U) == 0) {
      piVar1[0x14] = piVar1[0x14] & 0x9fffffff;
      if (DAT_100f3bb8 + -1 == DAT_100f4928) {
        DAT_100f3a18 = DAT_100f3ba0;
        DAT_100f4340 = DAT_100f3ba0;
      }
      if (iVar4 == 7) {
        _DAT_100f4768 = _DAT_100f4768 + 1;
        uVar2 = piVar5[0x14];
        while ((uVar2 & 0x400) != 0) {
          if (piVar5[8] == 0) {
            FUN_10056200(
                        "qhull internal error (qh_merge_degenredunant): f%d redundant but f%d has no replacement\n"
                        );
            FUN_10086dd0(5,piVar1,piVar5);
          }
          piVar5 = (int *)piVar5[8];
          uVar2 = piVar5[0x14];
        }
        if (piVar1 == piVar5) {
          FUN_100903f0((int)piVar1);
        }
        else {
          if (1 < DAT_100f3a18) {
            _fprintf(DAT_100f419c,
                     "qh_merge_degenredundant: facet f%d is contained in f%d, will merge\n",
                     piVar1[0x13]);
          }
          FUN_100936b0(piVar1,piVar5,(double *)0x0,(double *)0x0);
          iVar6 = iVar6 + 1;
        }
      }
      else {
        iVar4 = FUN_10076e50((int *)piVar1[0xf]);
        if (iVar4 == 0) {
          _DAT_100f4500 = _DAT_100f4500 + 1;
          if (1 < DAT_100f3a18) {
            _fprintf(DAT_100f419c,"qh_merge_degenredundant: facet f%d has no neighbors.  Deleted\n")
            ;
          }
          FUN_10092870((int)piVar1,0);
          iVar4 = piVar1[0xd];
          if (iVar4 != 0) {
            piVar5 = (int *)(iVar4 + 4);
            iVar4 = *(int *)(iVar4 + 4);
            while (iVar4 != 0) {
              piVar5 = piVar5 + 1;
              FUN_100768b0(*(int **)(iVar4 + 0xc),(int)piVar1);
              if (*(int *)(*(int *)(iVar4 + 0xc) + 4) == 0) {
                _DAT_100f44f8 = _DAT_100f44f8 + 1;
                if (1 < DAT_100f3a18) {
                  _fprintf(DAT_100f419c,
                           "qh_merge_degenredundant: deleted v%d because f%d has no neighbors\n",
                           *(uint *)(iVar4 + 0x14) & 0xffffff);
                }
                *(uint *)(iVar4 + 0x14) = *(uint *)(iVar4 + 0x14) | 0x8000000;
                FUN_100775a0(&DAT_100f42a0,iVar4);
              }
              iVar4 = *piVar5;
            }
          }
          iVar6 = iVar6 + 1;
        }
        else if (iVar4 < DAT_100f3bec) {
          piVar5 = (int *)FUN_10090770(piVar1,&local_58,local_48,&local_50,unaff_EDI,unaff_ESI);
          if (1 < DAT_100f3a18) {
            _fprintf(DAT_100f419c,
                     "qh_merge_degenredundant: facet f%d has %d neighbors, merge into f%d dist %2.2g\n"
                     ,piVar1[0x13],iVar4,piVar5[0x13],local_58);
          }
          FUN_100936b0(piVar1,piVar5,local_48,&local_50);
          iVar6 = iVar6 + 1;
          if (DAT_100f3b30 != 0) {
            _DAT_100f44e0 = _DAT_100f44e0 + 1;
            _DAT_100f44f0 = _DAT_100f44f0 + local_58;
            if (_DAT_100f44e8 < local_58) {
              _DAT_100f44e8 = local_58;
            }
          }
        }
      }
    }
    puVar3 = (undefined4 *)FUN_10076910(DAT_100f4294);
  }
  return iVar6;
}


// 10094300 FUN_10094300

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10094300(double *param_1,double *param_2,int param_3)

{
  int *piVar1;
  int *piVar2;
  double *unaff_ESI;
  double *pdVar3;
  double *unaff_EDI;
  double *pdVar4;
  double *pdVar5;
  double local_70;
  double local_68;
  double local_60;
  double local_58;
  double local_50;
  double local_48 [8];
  
  if (DAT_100f3bb8 + -1 == DAT_100f4928) {
    DAT_100f3a18 = DAT_100f3ba0;
    DAT_100f4340 = DAT_100f3ba0;
  }
  if (2 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_merge_nonconvex: merge #%d for f%d and f%d type %d\n");
  }
  pdVar3 = param_2;
  if (((ulonglong)param_1[10] & 0x200) == 0) {
    pdVar3 = param_1;
    param_1 = param_2;
  }
  pdVar5 = &local_60;
  pdVar4 = param_1;
  piVar1 = (int *)FUN_10090770((int *)param_1,pdVar5,&local_68,&local_70,unaff_EDI,unaff_ESI);
  piVar2 = (int *)FUN_10090770((int *)pdVar3,&local_58,local_48,&local_50,pdVar4,pdVar5);
  if (local_58 <= local_60) {
    if (((DAT_100f39cc == 0) || (((ulonglong)pdVar3[10] & 0x200) != 0)) ||
       (((local_68 < -_DAT_100f3a38 || (_DAT_100f4228 < local_70)) && (local_58 <= local_60 * 1.5)))
       ) {
      FUN_100936b0((int *)pdVar3,piVar2,local_48,&local_50);
      local_60 = local_58;
      goto LAB_100944a4;
    }
    _DAT_100f4400 = _DAT_100f4400 + 1;
    _DAT_100f4410 = _DAT_100f4410 + local_60;
    if (_DAT_100f4408 < local_60) {
      _DAT_100f4408 = local_60;
    }
    if (1 < DAT_100f3a18) {
      _fprintf(DAT_100f419c,
               "qh_merge_nonconvex: avoid merging old facet f%d dist %2.2g.  Use f%d dist %2.2g instead\n"
               ,*(int *)((int)pdVar3 + 0x4c),local_58,*(int *)((int)param_1 + 0x4c),local_58);
    }
  }
  FUN_100936b0((int *)param_1,piVar1,&local_68,&local_70);
LAB_100944a4:
  if (DAT_100f3b30 != 0) {
    if (param_3 == 2) {
      DAT_100f43a8._0_4_ = (int)DAT_100f43a8 + 1;
      _DAT_100f43b8 = _DAT_100f43b8 + local_60;
      if (DAT_100f43b0 < local_60) {
        DAT_100f43b0 = local_60;
        return;
      }
    }
    else if (param_3 == 3) {
      _DAT_100f4448 = _DAT_100f4448 + 1;
      _DAT_100f4458 = _DAT_100f4458 + local_60;
      if (_DAT_100f4450 < local_60) {
        _DAT_100f4450 = local_60;
        return;
      }
    }
    else {
      _DAT_100f4470 = _DAT_100f4470 + 1;
      _DAT_100f4480 = _DAT_100f4480 + local_60;
      if (_DAT_100f4478 < local_60) {
        _DAT_100f4478 = local_60;
        return;
      }
    }
  }
  return;
}


// 10094560 FUN_10094560

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10094560(int *param_1,undefined4 *param_2)

{
  int *piVar1;
  int iVar2;
  uint uVar3;
  int iVar4;
  uint uVar5;
  int iVar6;
  int *piVar7;
  int *piVar8;
  int *piVar9;
  int local_c;
  
  local_c = 0;
  if (1 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_mergecycle_all: begin\n");
  }
  piVar9 = param_1;
  if (param_1 != (int *)0x0) {
    do {
      param_1 = (int *)piVar9[0xc];
      if (param_1 == (int *)0x0) break;
      if (piVar9[6] == 0) {
        if ((*(byte *)((int)piVar9 + 0x53) & 1) == 0) {
          FUN_10056200("qh_mergecycle_all: f%d without normal\n");
          FUN_10080c90((undefined8 *)0x5,piVar9);
        }
        piVar8 = (int *)piVar9[8];
        piVar1 = *(int **)(piVar9[0xf] + 4);
        if (piVar8 == piVar9) {
          _DAT_100f4808 = _DAT_100f4808 + 1;
          iVar6 = piVar9[0xd];
          iVar2 = *(int *)(iVar6 + 4);
          piVar8 = (int *)(iVar6 + 4);
          iVar4 = iVar2;
          if (iVar6 != 0) {
            while (iVar4 != 0) {
              piVar8 = piVar8 + 1;
              if (iVar4 != iVar2) {
                *(uint *)(iVar4 + 0x14) = *(uint *)(iVar4 + 0x14) | 0x4000000;
              }
              iVar4 = *piVar8;
            }
          }
          piVar1[8] = 0;
          FUN_100936b0(piVar9,piVar1,(double *)0x0,(double *)0x0);
        }
        else {
          iVar6 = 0;
          piVar7 = piVar9;
          for (; piVar8 != (int *)0x0; piVar8 = (int *)(-(uint)(piVar8 != piVar9) & uVar3)) {
            uVar3 = piVar8[8];
            if ((piVar8[0x14] & 0x2000400U) != 0) {
              FUN_100799c0(piVar8);
            }
            piVar8[0x14] = piVar8[0x14] | 0x2000000;
            if (piVar8[6] == 0) {
              iVar6 = iVar6 + 1;
              piVar7 = piVar8;
            }
            else {
              piVar7[8] = piVar8[8];
              piVar8[8] = 0;
            }
          }
          do {
            if ((param_1[0x14] & 0x2000000U) == 0) break;
            param_1 = (int *)param_1[0xc];
          } while (param_1 != (int *)0x0);
          piVar1[8] = 0;
          FUN_100934e0(piVar9,piVar1);
          uVar3 = piVar1[0x14];
          uVar5 = (uVar3 & 0x1ff) + iVar6;
          if ((int)uVar5 < 0x200) {
            piVar1[0x14] = (uVar3 ^ uVar5) & 0x1ff ^ uVar3;
          }
          else {
            piVar1[0x14] = uVar3 | 0x1ff;
          }
          _DAT_100f44d0 = _DAT_100f44d0 + 1;
          DAT_100f44c8 = DAT_100f44c8 + iVar6;
          if (DAT_100f44c0 < iVar6) {
            DAT_100f44c0 = iVar6;
          }
        }
        local_c = local_c + 1;
      }
      piVar9 = param_1;
    } while (param_1 != (int *)0x0);
    if (local_c != 0) {
      *param_2 = 1;
    }
  }
  if (0 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,
             "qh_mergecycle_all: merged %d same cycles or facets into coplanar horizons\n",local_c);
  }
  return;
}


// 10094760 FUN_10094760

uint FUN_10094760(void)

{
  undefined4 *puVar1;
  undefined8 *puVar2;
  int iVar3;
  uint uVar4;
  int iVar5;
  undefined4 *puVar6;
  int *piVar7;
  int *unaff_EDI;
  uint local_8;
  int local_4;
  
  iVar5 = 0;
  local_4 = 0;
  if (DAT_100f3bec == 2) {
    return 0;
  }
  iVar3 = FUN_10094060();
  local_8 = (uint)(iVar3 != 0);
  puVar2 = DAT_100f41bc;
joined_r0x100947a8:
  for (; (puVar2 != (undefined8 *)0x0 && (*(int *)(puVar2 + 6) != 0));
      puVar2 = *(undefined8 **)(puVar2 + 6)) {
    if ((*(uint *)(puVar2 + 10) & 0x10000000) != 0) {
      if (DAT_100f3a60 == 0) {
        *(uint *)(puVar2 + 10) = *(uint *)(puVar2 + 10) & 0xefffffff;
      }
      FUN_10091c60((int)puVar2);
    }
  }
  puVar2 = DAT_100f41bc;
  piVar7 = DAT_100f41e0;
  if (DAT_100f3a60 == 0) {
    return 0;
  }
  for (; (DAT_100f41e0 = piVar7, puVar2 != (undefined8 *)0x0 && (*(int *)(puVar2 + 6) != 0));
      puVar2 = *(undefined8 **)(puVar2 + 6)) {
    if ((*(uint *)(puVar2 + 10) & 0x10000000) != 0) {
      *(uint *)(puVar2 + 10) = *(uint *)(puVar2 + 10) & 0xefffffff;
      if (*(int *)((int)puVar2 + 0x34) != 0) {
        puVar1 = (undefined4 *)(*(int *)((int)puVar2 + 0x34) + 4);
        piVar7 = (int *)*puVar1;
        while (piVar7 != (int *)0x0) {
          puVar6 = puVar1 + 1;
          if (((piVar7[5] & 0x4000000U) != 0) &&
             (uVar4 = FUN_10093bc0(piVar7,puVar2,unaff_EDI), uVar4 != 0)) {
            iVar5 = iVar5 + 1;
            puVar6 = puVar1;
          }
          puVar1 = puVar6;
          piVar7 = (int *)*puVar6;
        }
      }
    }
    piVar7 = DAT_100f41e0;
  }
  do {
    if ((piVar7 == (int *)0x0) || (*piVar7 == 0)) {
      if (0 < DAT_100f3a18) {
        _fprintf(DAT_100f419c,
                 "qh_reducevertices: renamed %d shared vertices and %d redundant vertices. Degen? %d\n"
                 ,iVar5,local_4,local_8);
      }
      return local_8;
    }
    uVar4 = piVar7[5];
    if (((((uVar4 & 0x4000000) != 0) && ((uVar4 & 0x8000000) == 0)) &&
        (piVar7[5] = uVar4 & 0xfbffffff, 3 < DAT_100f3bec)) &&
       (uVar4 = FUN_10093b20(piVar7), uVar4 != 0)) {
      local_4 = local_4 + 1;
      iVar3 = FUN_10094060();
      if (iVar3 != 0) break;
    }
    piVar7 = (int *)*piVar7;
  } while( true );
  local_8 = 1;
  puVar2 = DAT_100f41bc;
  goto joined_r0x100947a8;
}


// 100948f0 FUN_100948f0

void __cdecl FUN_100948f0(int param_1,int param_2)

{
  undefined4 *puVar1;
  double *pdVar2;
  double *pdVar3;
  bool bVar4;
  int *piVar5;
  double dVar6;
  double dVar7;
  bool bVar8;
  undefined4 uVar9;
  int iVar10;
  undefined4 *puVar11;
  int iVar12;
  uint uVar13;
  undefined3 extraout_var;
  int local_10;
  int local_c;
  int local_8;
  int local_4;
  
  local_4 = 0;
  local_8 = 0;
  local_c = 0;
  local_10 = 0;
  if (1 < DAT_100f3a18) {
    if (DAT_100f41bc == 0) {
      uVar9 = 0xffffffff;
    }
    else {
      uVar9 = *(undefined4 *)(DAT_100f41bc + 0x4c);
    }
    _fprintf(DAT_100f419c,"qh_all_merges: starting to merge facets beginning from f%d\n",uVar9);
  }
LAB_10094940:
  do {
    bVar8 = false;
    iVar10 = FUN_10076e50(DAT_100f4290);
    while (iVar10 != 0) {
      puVar11 = (undefined4 *)FUN_10076910(DAT_100f4290);
      while (puVar11 != (undefined4 *)0x0) {
        pdVar2 = (double *)puVar11[2];
        pdVar3 = (double *)puVar11[3];
        iVar10 = puVar11[4];
        DAT_100f4354 = DAT_100f4354 + 1;
        puVar1 = (undefined4 *)(DAT_100f4320 + *(int *)(DAT_100f4328 + 0x60) * 4);
        *puVar11 = *puVar1;
        *puVar1 = puVar11;
        dVar6 = pdVar2[10];
        if (((((ulonglong)dVar6 & 0x400) == 0) &&
            (dVar7 = pdVar3[10], ((ulonglong)dVar7 & 0x400) == 0)) &&
           ((((((ulonglong)dVar6 & 0x200) == 0 || (((ulonglong)dVar6 & 0x4000000) != 0)) &&
             ((((ulonglong)dVar7 & 0x200) == 0 || (((ulonglong)dVar7 & 0x4000000) != 0)))) ||
            ((DAT_100f3a44 == 0 || (2 < iVar10)))))) {
          FUN_10094300(pdVar2,pdVar3,iVar10);
          iVar12 = FUN_10094060();
          local_c = local_c + iVar12;
          local_10 = local_10 + 1;
          bVar8 = true;
          if (iVar10 == 3) {
            local_8 = local_8 + 1;
          }
          else {
            local_4 = local_4 + 1;
          }
        }
        puVar11 = (undefined4 *)FUN_10076910(DAT_100f4290);
      }
      if (((DAT_100f4260 != 0) && (DAT_100f3bec < 6)) && (2 < local_10)) {
        local_10 = 0;
        FUN_10094760();
      }
      FUN_100928a0(DAT_100f41bc);
      iVar10 = FUN_10076e50(DAT_100f4290);
    }
    if (DAT_100f4010 != 0) {
      bVar4 = false;
      if ((3 < DAT_100f3bec) && (piVar5 = DAT_100f41d8, DAT_100f4260 != 0)) {
        while ((piVar5 != (int *)0x0 && ((int *)*piVar5 != (int *)0x0))) {
          piVar5[5] = piVar5[5] | 0x4000000;
          piVar5 = (int *)*piVar5;
        }
        bVar4 = true;
      }
      if ((((bVar8) || (param_1 != 0)) && ((DAT_100f3a40 == 0 || (DAT_100f4260 != 0)))) &&
         (DAT_100f3bec < 6)) {
        param_1 = 0;
      }
      else if (!bVar4) goto LAB_10094afe;
      uVar13 = FUN_10094760();
      if (uVar13 != 0) {
        FUN_100928a0(DAT_100f41bc);
        goto LAB_10094940;
      }
    }
LAB_10094afe:
    if ((param_2 == 0) || (bVar8 = FUN_100923f0(), CONCAT31(extraout_var,bVar8) == 0)) {
      if ((DAT_100f39dc != 0) && (DAT_100f3a40 == 0)) {
        DAT_100f42e8 = DAT_100f3b48;
        DAT_100f3b48 = 0;
        FUN_100791a0(DAT_100f41bc,0);
        DAT_100f3b48 = DAT_100f42e8;
      }
      if (0 < DAT_100f3a18) {
        _fprintf(DAT_100f419c,
                 "qh_all_merges: merged %d coplanar facets %d concave facets and %d degen or redundant facets.\n"
                 ,local_4,local_8,local_c);
      }
      if ((3 < DAT_100f3a18) && (DAT_100f41e4 < 0x32)) {
        FUN_1007a5b0();
        return;
      }
      return;
    }
  } while( true );
}


// 10094ba0 FUN_10094ba0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10094ba0(int param_1,undefined4 *param_2)

{
  int iVar1;
  int *piVar2;
  undefined4 *puVar3;
  int *piVar4;
  int *piVar5;
  double *unaff_ESI;
  double *unaff_EDI;
  int iVar6;
  int *local_60;
  int local_5c;
  double local_58;
  double local_50;
  double local_48 [8];
  
  iVar6 = 0;
  local_5c = 0;
  if (3 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_flippedmerges: begin\n");
  }
  if (param_1 != 0) {
    do {
      if (*(int *)(param_1 + 0x30) == 0) break;
      if (((*(uint *)(param_1 + 0x50) & 0x8000) != 0) && ((*(uint *)(param_1 + 0x50) & 0x400) == 0))
      {
        FUN_1008fe80(param_1,param_1,4,(undefined8 *)0x0);
      }
      param_1 = *(int *)(param_1 + 0x30);
    } while (param_1 != 0);
  }
  local_60 = FUN_10076f00();
  DAT_100f4290 = FUN_100776f0(DAT_100f41ac);
  FUN_10077740(local_60);
  if (local_60 != (int *)0x0) {
    piVar5 = local_60 + 1;
    iVar1 = local_60[1];
    while (iVar1 != 0) {
      piVar2 = *(int **)(iVar1 + 8);
      piVar5 = piVar5 + 1;
      if ((*(int *)(iVar1 + 0x10) == 4) && ((piVar2[0x14] & 0x400U) == 0)) {
        if (DAT_100f3bb8 + -1 == DAT_100f4928) {
          DAT_100f3a18 = DAT_100f3ba0;
          DAT_100f4340 = DAT_100f3ba0;
        }
        piVar4 = (int *)FUN_10090770(piVar2,&local_58,local_48,&local_50,unaff_EDI,unaff_ESI);
        if (DAT_100f3a18 != 0) {
          _fprintf(DAT_100f419c,
                   "qh_flippedmerges: merge flipped f%d into f%d dist %2.2g during p%d\n",
                   piVar2[0x13],piVar4[0x13],local_58,DAT_100f4210);
        }
        FUN_100936b0(piVar2,piVar4,local_48,&local_50);
        local_5c = local_5c + 1;
        if (DAT_100f3b30 != 0) {
          _DAT_100f4608 = _DAT_100f4608 + 1;
          _DAT_100f4618 = _DAT_100f4618 + local_58;
          if (_DAT_100f4610 < local_58) {
            _DAT_100f4610 = local_58;
          }
        }
        FUN_10094060();
      }
      iVar6 = local_5c;
      iVar1 = *piVar5;
    }
    if (local_60 != (int *)0x0) {
      piVar5 = local_60 + 1;
      puVar3 = (undefined4 *)local_60[1];
      while (puVar3 != (undefined4 *)0x0) {
        piVar5 = piVar5 + 1;
        if (((*(uint *)(puVar3[2] + 0x50) & 0x400) == 0) &&
           ((*(uint *)(puVar3[3] + 0x50) & 0x400) == 0)) {
          FUN_100775a0(&DAT_100f4290,(int)puVar3);
        }
        else {
          FUN_10080480(puVar3,0x18);
        }
        puVar3 = (undefined4 *)*piVar5;
      }
    }
  }
  FUN_100777d0((int *)&local_60);
  if (iVar6 != 0) {
    *param_2 = 1;
  }
  if (0 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_flippedmerges: merged %d flipped facets into a good neighbor\n",iVar6)
    ;
  }
  return;
}


// 10094dd0 FUN_10094dd0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10094dd0(int param_1,undefined8 param_2,undefined8 param_3)

{
  int *piVar1;
  int iVar2;
  int local_4;
  
  local_4 = 0;
  if ((DAT_100f4014 == 0) || (iVar2 = FUN_10090040(0), iVar2 == 0)) {
    if (1 < DAT_100f3a18) {
      if (DAT_100f41bc == (int *)0x0) {
        iVar2 = -1;
      }
      else {
        iVar2 = DAT_100f41bc[0x13];
      }
      _fprintf(DAT_100f419c,
               "qh_premerge: premerge centrum %2.2g angle %2.2g for apex v%d facetlist f%d\n",
               param_2,param_3,*(uint *)(param_1 + 0x14) & 0xffffff,iVar2);
    }
    if ((3 < DAT_100f3a18) && (DAT_100f41e4 < 0x32)) {
      FUN_1007a5b0();
    }
    _DAT_100f4030 = param_2;
    _DAT_100f4038 = param_3;
    DAT_100f4294 = FUN_100776f0(DAT_100f41ac);
    DAT_100f4290 = FUN_100776f0(DAT_100f41ac);
    if (DAT_100f3bec < 3) {
      FUN_10094560(DAT_100f41bc,&local_4);
    }
    else {
      FUN_10090b70((int)DAT_100f41bc);
      FUN_10094560(DAT_100f41bc,&local_4);
      FUN_10093d90(&local_4);
      for (piVar1 = DAT_100f41bc; (piVar1 != (int *)0x0 && (piVar1[0xc] != 0));
          piVar1 = (int *)piVar1[0xc]) {
        if ((piVar1[0x14] & 0x201000U) == 0) {
          FUN_10090520((int)piVar1,0);
        }
      }
      iVar2 = FUN_10094060();
      if (iVar2 != 0) {
        local_4 = 1;
      }
    }
    FUN_10094ba0((int)DAT_100f41bc,&local_4);
    if ((DAT_100f3a40 == 0) || (DAT_100f4928 != 0)) {
      _DAT_100f4858 = _DAT_100f4858 + 1;
      DAT_100f4260 = 0;
      FUN_10092a60((int)DAT_100f41bc);
      FUN_100948f0(local_4,0);
    }
    FUN_100777d0(&DAT_100f4290);
    FUN_100777d0(&DAT_100f4294);
  }
  return;
}


// 10094f80 FUN_10094f80

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10094f80(undefined4 param_1,undefined8 param_2,undefined8 param_3,int param_4)

{
  int iVar1;
  int *piVar2;
  int iVar3;
  
  if ((DAT_100f3b6c != 0) || (DAT_100f3a18 != 0)) {
    FUN_10086ae0((double *)0x0,0);
    FUN_100872b0(DAT_100f419c);
    if (DAT_100f3b30 != 0) {
      FUN_10086ab0(DAT_100f419c);
    }
    FUN_10056200("\n%s with \'C%.2g\' and \'A%.2g\'\n");
  }
  iVar3 = param_4;
  if (1 < DAT_100f3a18) {
    _fprintf(DAT_100f419c,"qh_postmerge: postmerge.  test vneighbors? %d\n");
  }
  _DAT_100f4030 = param_2;
  _DAT_100f4038 = param_3;
  DAT_100f4260 = 1;
  DAT_100f4294 = FUN_100776f0(DAT_100f41ac);
  DAT_100f4290 = FUN_100776f0(DAT_100f41ac);
  if (DAT_100f41c0 != DAT_100f41b0) {
    DAT_100f4240 = 1;
    DAT_100f41bc = DAT_100f41b0;
    DAT_100f41c0 = DAT_100f41b0;
    for (iVar1 = DAT_100f41b0;
        (DAT_100f41e0 = DAT_100f41d8, piVar2 = DAT_100f41d8, iVar1 != 0 &&
        (*(int *)(iVar1 + 0x30) != 0)); iVar1 = *(int *)(iVar1 + 0x30)) {
      *(uint *)(iVar1 + 0x50) = *(uint *)(iVar1 + 0x50) | 0x200;
      if ((*(uint *)(iVar1 + 0x50) & 0x1000) == 0) {
        *(uint *)(iVar1 + 0x50) = *(uint *)(iVar1 + 0x50) | 0x10000000;
      }
      _DAT_100f4850 = _DAT_100f4850 + 1;
    }
    while ((piVar2 != (int *)0x0 && ((int *)*piVar2 != (int *)0x0))) {
      piVar2[5] = piVar2[5] | 0x10000000;
      piVar2 = (int *)*piVar2;
    }
    piVar2 = DAT_100f41d8;
    if (DAT_100f4010 != 0) {
      while ((piVar2 != (int *)0x0 && ((int *)*piVar2 != (int *)0x0))) {
        piVar2[5] = piVar2[5] | 0x4000000;
        piVar2 = (int *)*piVar2;
      }
      if ((DAT_100f3a40 != 0) && (DAT_100f3bec < 6)) {
        FUN_10094760();
      }
    }
    if ((DAT_100f3a88 == 0) && (DAT_100f3a40 == 0)) {
      FUN_10094ba0(DAT_100f41bc,(undefined4 *)&param_2);
    }
  }
  FUN_10092a60(DAT_100f41bc);
  FUN_100948f0(0,iVar3);
  FUN_100777d0(&DAT_100f4290);
  FUN_100777d0(&DAT_100f4294);
  return;
}


// 10095160 FUN_10095160

void __thiscall FUN_10095160(void *this,float *param_1)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  
  fVar1 = *param_1;
  fVar2 = param_1[1];
  fVar4 = param_1[2] * 2.0;
  fVar7 = fVar2 * 2.0;
  fVar8 = param_1[2] * fVar4;
  fVar5 = fVar1 * fVar1 * 2.0;
  fVar3 = param_1[3];
  fVar6 = fVar3 * fVar1 * 2.0;
  *(float *)this = 1.0 - (fVar8 + fVar2 * fVar7);
  *(float *)((int)this + 4) = fVar3 * fVar4 + fVar1 * fVar7;
  *(float *)((int)this + 8) = fVar1 * fVar4 - fVar3 * fVar7;
  *(float *)((int)this + 0x10) = fVar1 * fVar7 - fVar3 * fVar4;
  *(float *)((int)this + 0x14) = 1.0 - (fVar8 + fVar5);
  *(float *)((int)this + 0x18) = fVar6 + fVar2 * fVar4;
  *(float *)((int)this + 0x20) = fVar3 * fVar7 + fVar1 * fVar4;
  *(float *)((int)this + 0x24) = fVar2 * fVar4 - fVar6;
  *(float *)((int)this + 0x28) = 1.0 - (fVar2 * fVar7 + fVar5);
  return;
}


// 10095290 FUN_10095290

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_10095290(undefined4 param_1)

{
  _DAT_100de994 = param_1;
  return;
}


// 100952a0 FUN_100952a0

void __fastcall FUN_100952a0(undefined4 *param_1)

{
  *param_1 = 0x3f800000;
  param_1[1] = 0;
  param_1[2] = 0;
  param_1[4] = 0;
  param_1[5] = 0x3f800000;
  param_1[6] = 0;
  param_1[8] = 0;
  param_1[9] = 0;
  param_1[10] = 0x3f800000;
  param_1[0xc] = 0;
  param_1[0xd] = 0;
  param_1[0xe] = 0;
  return;
}


// 100952f0 FUN_100952f0

void __thiscall FUN_100952f0(void *this,float *param_1)

{
  float fVar1;
  float fVar2;
  uint uVar3;
  float fVar4;
  float fVar5;
  float local_10 [4];
  
  fVar1 = param_1[10];
  fVar4 = *param_1 + param_1[5] + fVar1;
  if (0.0 < fVar4) {
    fVar2 = SQRT(fVar4 + 1.0);
    fVar5 = 0.5 / fVar2;
    *(float *)this = (param_1[6] - param_1[9]) * fVar5;
    *(float *)((int)this + 4) = (param_1[8] - param_1[2]) * fVar5;
    fVar1 = param_1[1];
    fVar4 = param_1[4];
    *(float *)((int)this + 0xc) = fVar2 * 0.5;
    *(float *)((int)this + 8) = (fVar1 - fVar4) * fVar5;
    return;
  }
  local_10[0] = 1.4013e-45;
  local_10[1] = 2.8026e-45;
  local_10[2] = 0.0;
  uVar3 = (uint)(*param_1 < param_1[5]);
  if (param_1[uVar3 * 5] <= fVar1 && fVar1 != param_1[uVar3 * 5]) {
    uVar3 = 2;
  }
  fVar1 = local_10[uVar3];
  fVar4 = local_10[(int)fVar1];
  fVar2 = SQRT((param_1[uVar3 * 5] - (param_1[(int)fVar4 * 5] + param_1[(int)fVar1 * 5])) + 1.0);
  fVar5 = 0.5 / fVar2;
  local_10[3] = (param_1[(int)fVar4 + (int)fVar1 * 4] - param_1[(int)fVar1 + (int)fVar4 * 4]) *
                fVar5;
  local_10[uVar3] = fVar2 * 0.5;
  local_10[(int)fVar1] = (param_1[uVar3 + (int)fVar1 * 4] + param_1[(int)fVar1 + uVar3 * 4]) * fVar5
  ;
  local_10[(int)fVar4] = (param_1[uVar3 + (int)fVar4 * 4] + param_1[(int)fVar4 + uVar3 * 4]) * fVar5
  ;
  *(float *)this = local_10[0];
  *(float *)((int)this + 4) = local_10[1];
  *(float *)((int)this + 8) = local_10[2];
  *(float *)((int)this + 0xc) = local_10[3];
  return;
}


// 10095480 FUN_10095480

void FUN_10095480(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 4))(param_1);
  return;
}


// 100954a0 FUN_100954a0

void FUN_100954a0(undefined4 param_1)

{
  (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))(param_1);
  return;
}


// 100954c0 FUN_100954c0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined * FUN_100954c0(void)

{
  if ((_DAT_100f5154 & 1) == 0) {
    _DAT_100f5154 = _DAT_100f5154 | 1;
    _atexit((_func_4879 *)&DAT_100bbfc0);
  }
  return &DAT_100f5150;
}


// 100954f0 FUN_100954f0

void __thiscall FUN_100954f0(void *this,undefined4 *param_1)

{
  *param_1 = 0x3f800000;
  param_1[1] = 0x3f800000;
  param_1[2] = *(int *)((int)this + 0x10) + -2;
  *(undefined1 *)(param_1 + 5) = *(undefined1 *)((int)this + 0x48);
  param_1[3] = *(undefined4 *)((int)this + 0x28);
  param_1[4] = *(undefined4 *)((int)this + 0x44);
  return;
}


// 10095530 FUN_10095530

void __fastcall FUN_10095530(ushort *param_1)

{
  if ((*param_1 & 0x12) == 0) {
    return;
  }
  return;
}


// 10095550 FUN_10095550

void __fastcall FUN_10095550(int param_1)

{
  undefined1 in_XMM0 [16];
  undefined1 auVar1 [16];
  
  auVar1 = maxps(*(undefined1 (*) [16])(param_1 + 0x50),in_XMM0);
  *(undefined1 (*) [16])(param_1 + 0x50) = auVar1;
  return;
}


// 10095560 FUN_10095560

void __fastcall FUN_10095560(int *param_1)

{
  if (((char)param_1[0x12] == '\0') && (*(short *)((int)param_1 + 0x22) != 0)) {
    FUN_10037ae0(*(void **)(param_1[1] + 0x34),param_1);
    *(undefined1 *)(param_1 + 0x12) = 1;
  }
  return;
}


// 10095590 FUN_10095590

void __fastcall FUN_10095590(int *param_1)

{
  if (((char)param_1[0x12] != '\0') && (*(short *)((int)param_1 + 10) != 0)) {
    FUN_100371d0(param_1,0);
    *(undefined1 *)(param_1 + 0x12) = 0;
  }
  return;
}


// 100955f0 FUN_100955f0

undefined4 * __thiscall FUN_100955f0(void *this,byte param_1)

{
  int iVar1;
  
  *(undefined ***)this = &PTR_LAB_100ce510;
  iVar1 = *(int *)((int)this + 0xc);
  if (iVar1 != (int)this + 0x10) {
    if (iVar1 != 0) {
      FUN_10002ea0(iVar1);
    }
    *(undefined4 *)((int)this + 0xc) = 0;
    *(undefined2 *)((int)this + 8) = 0;
  }
  *(undefined2 *)((int)this + 10) = 0;
  *(undefined ***)this = &PTR_LAB_100bc3f0;
  if ((param_1 & 1) != 0) {
    FUN_10002ea0(this);
  }
  return (undefined4 *)this;
}


// 10095640 FUN_10095640

void __thiscall FUN_10095640(void *this,undefined4 param_1,int param_2)

{
  *(undefined2 *)((int)this + 8) = 0;
  *(undefined2 *)((int)this + 10) = 0;
  *(undefined4 *)((int)this + 0xc) = 0;
  *(undefined4 *)((int)this + 4) = param_1;
  *(undefined ***)this = &PTR_LAB_100ce548;
  *(undefined2 *)((int)this + 0x1a) = 0;
  *(undefined2 *)((int)this + 0x18) = 0;
  *(undefined4 *)((int)this + 0x1c) = 0;
  *(undefined2 *)((int)this + 0x22) = 0;
  *(undefined2 *)((int)this + 0x20) = 0;
  *(undefined4 *)((int)this + 0x24) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined1 *)((int)this + 0x48) = 0;
  *(undefined4 *)((int)this + 0x50) = 0;
  *(undefined4 *)((int)this + 0x54) = 0;
  *(undefined4 *)((int)this + 0x58) = 0;
  *(undefined4 *)((int)this + 0x5c) = 0;
  *(undefined4 *)((int)this + 0x2c) = 0;
  *(undefined4 *)((int)this + 0x60) = 0;
  *(int *)((int)this + 0x10) = *(int *)(param_2 + 8) + 2;
  *(undefined4 *)((int)this + 0x44) = *(undefined4 *)(param_2 + 0x10);
  *(undefined4 *)((int)this + 0x28) = *(undefined4 *)(param_2 + 0xc);
  *(undefined1 *)((int)this + 0x49) = 0;
  *(undefined4 *)((int)this + 0x30) = 0;
  return;
}


// 100956b0 FUN_100956b0

void __fastcall FUN_100956b0(int *param_1)

{
  int *piVar1;
  uint uVar2;
  int iVar3;
  
  uVar2 = (uint)*(ushort *)((int)param_1 + 0x1a);
  *param_1 = (int)&PTR_LAB_100ce548;
  while (uVar2 = uVar2 - 1, -1 < (int)uVar2) {
    iVar3 = *(int *)(param_1[7] + uVar2 * 4);
    if (*(int **)(iVar3 + 0x10) == param_1) {
      *(undefined4 *)(iVar3 + 0x10) = 0;
    }
  }
  if ((char)param_1[0x12] != '\0') {
    FUN_100371d0(param_1,1);
  }
  iVar3 = param_1[9];
  if (iVar3 != 0) {
    FUN_100954c0();
    FUN_100954a0(iVar3);
  }
  iVar3 = param_1[7];
  if (iVar3 != 0) {
    FUN_100954c0();
    FUN_100954a0(iVar3);
  }
  *param_1 = (int)&PTR_LAB_100ce510;
  piVar1 = (int *)param_1[3];
  if (piVar1 != param_1 + 4) {
    if (piVar1 != (int *)0x0) {
      FUN_10002ea0(piVar1);
    }
    param_1[3] = 0;
    *(undefined2 *)(param_1 + 2) = 0;
  }
  *(undefined2 *)((int)param_1 + 10) = 0;
  *param_1 = (int)&PTR_LAB_100bc3f0;
  return;
}


// 100957c0 FUN_100957c0

void __fastcall FUN_100957c0(int param_1)

{
  int iVar1;
  uint uVar2;
  
  uVar2 = (uint)*(ushort *)(param_1 + 0x1a);
  *(undefined4 *)(param_1 + 0x14) = 0;
  while (uVar2 = uVar2 - 1, -1 < (int)uVar2) {
    iVar1 = (**(code **)(**(int **)(*(int *)(param_1 + 0x1c) + uVar2 * 4) + 8))();
    *(int *)(param_1 + 0x14) = *(int *)(param_1 + 0x14) + iVar1;
  }
  return;
}


// 100957f0 FUN_100957f0

void __thiscall FUN_100957f0(void *this,int param_1,int param_2)

{
  char cVar1;
  int iVar2;
  byte *pbVar3;
  int *piVar4;
  int iVar5;
  int *piVar6;
  
  cVar1 = *(char *)((int)this + 0x48);
  if (cVar1 != '\0') {
    FUN_10095590((int *)this);
  }
  if (*(ushort *)((int)this + 0x18) <= *(ushort *)((int)this + 0x1a)) {
    FUN_100bb120((void *)((int)this + 0x18),4);
  }
  *(int *)(*(int *)((int)this + 0x1c) + (uint)*(ushort *)((int)this + 0x1a) * 4) = param_1;
  *(short *)((int)this + 0x1a) = *(short *)((int)this + 0x1a) + 1;
  piVar4 = (int *)(param_1 + 8);
  param_1 = 1;
  do {
    iVar2 = *piVar4;
    iVar5 = *(ushort *)((int)this + 0x22) - 1;
    if (-1 < iVar5) {
      piVar6 = (int *)(*(int *)((int)this + 0x24) + iVar5 * 4);
      do {
        if (*piVar6 == iVar2) {
          if (-1 < iVar5) goto LAB_100958c0;
          break;
        }
        iVar5 = iVar5 + -1;
        piVar6 = piVar6 + -1;
      } while (-1 < iVar5);
    }
    if (*(ushort *)((int)this + 0x20) <= *(ushort *)((int)this + 0x22)) {
      FUN_100bb120((ushort *)((int)this + 0x20),4);
    }
    *(int *)(*(int *)((int)this + 0x24) + (uint)*(ushort *)((int)this + 0x22) * 4) = iVar2;
    *(short *)((int)this + 0x22) = *(short *)((int)this + 0x22) + 1;
    pbVar3 = *(byte **)(iVar2 + 0x94);
    if ((*pbVar3 & 2) == 0) {
      if (*(ushort *)((int)this + 8) <= *(ushort *)((int)this + 10)) {
        FUN_10037c70((ushort *)((int)this + 8));
      }
      *(byte **)(*(int *)((int)this + 0xc) + (uint)*(ushort *)((int)this + 10) * 4) = pbVar3;
      *(short *)((int)this + 10) = *(short *)((int)this + 10) + 1;
    }
LAB_100958c0:
    param_1 = param_1 + -1;
    piVar4 = piVar4 + -1;
    if (param_1 < 0) {
      *(undefined1 *)((int)this + 0x49) = 1;
      if (cVar1 == '\0') {
        *(int *)((int)this + 0x14) = *(int *)((int)this + 0x14) + param_2;
        return;
      }
      FUN_10095560((int *)this);
      *(int *)((int)this + 0x14) = *(int *)((int)this + 0x14) + param_2;
      return;
    }
  } while( true );
}


// 10095910 FUN_10095910

void __thiscall FUN_10095910(void *this,ushort *param_1)

{
  undefined4 uVar1;
  uint uVar2;
  
  uVar2 = (uint)*(ushort *)((int)this + 0x1a);
  while (uVar2 = uVar2 - 1, -1 < (int)uVar2) {
    uVar1 = *(undefined4 *)(*(int *)((int)this + 0x1c) + uVar2 * 4);
    if (*param_1 <= param_1[1]) {
      FUN_100bb120(param_1,4);
    }
    *(undefined4 *)(*(int *)(param_1 + 2) + (uint)param_1[1] * 4) = uVar1;
    param_1[1] = param_1[1] + 1;
  }
  return;
}


// 10095960 FUN_10095960

void __fastcall FUN_10095960(int param_1)

{
  ushort uVar1;
  undefined4 uVar2;
  int iVar3;
  int *piVar4;
  uint uVar5;
  ushort local_8;
  ushort local_6;
  int local_4;
  
  uVar5 = (uint)*(ushort *)(param_1 + 0x1a);
  local_6 = 0;
  local_8 = 0;
  local_4 = 0;
  while (uVar5 = uVar5 - 1, -1 < (int)uVar5) {
    iVar3 = (**(code **)(**(int **)(*(int *)(param_1 + 0x1c) + uVar5 * 4) + 0x18))();
    if (iVar3 == 0) {
      uVar2 = *(undefined4 *)(*(int *)(param_1 + 0x1c) + uVar5 * 4);
      if (local_8 <= local_6) {
        FUN_100bb120(&local_8,4);
      }
      *(undefined4 *)(local_4 + (uint)local_6 * 4) = uVar2;
      local_6 = local_6 + 1;
    }
  }
  uVar5 = (uint)local_6;
  do {
    uVar5 = uVar5 - 1;
    if ((int)uVar5 < 0) {
      iVar3 = local_6 - 1;
      if (-1 < iVar3) {
        do {
          uVar2 = *(undefined4 *)(local_4 + iVar3 * 4);
          if (*(ushort *)(param_1 + 0x18) <= *(ushort *)(param_1 + 0x1a)) {
            FUN_100bb120((ushort *)(param_1 + 0x18),4);
          }
          *(undefined4 *)(*(int *)(param_1 + 0x1c) + (uint)*(ushort *)(param_1 + 0x1a) * 4) = uVar2;
          *(short *)(param_1 + 0x1a) = *(short *)(param_1 + 0x1a) + 1;
          iVar3 = iVar3 + -1;
        } while (-1 < iVar3);
      }
      if (local_4 != 0) {
        iVar3 = local_4;
        FUN_100954c0();
        FUN_100954a0(iVar3);
      }
      return;
    }
    iVar3 = *(ushort *)(param_1 + 0x1a) - 1;
    if (-1 < iVar3) {
      piVar4 = (int *)(*(int *)(param_1 + 0x1c) + iVar3 * 4);
      do {
        if (*piVar4 == *(int *)(local_4 + uVar5 * 4)) goto LAB_10095a00;
        iVar3 = iVar3 + -1;
        piVar4 = piVar4 + -1;
      } while (-1 < iVar3);
    }
    iVar3 = -1;
LAB_10095a00:
    uVar1 = *(ushort *)(param_1 + 0x1a) - 1;
    *(ushort *)(param_1 + 0x1a) = uVar1;
    if (iVar3 < (int)(uint)uVar1) {
      *(undefined4 *)(*(int *)(param_1 + 0x1c) + iVar3 * 4) =
           *(undefined4 *)(*(int *)(param_1 + 0x1c) + (uint)uVar1 * 4);
    }
  } while( true );
}


// 10095b20 FUN_10095b20

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

void __thiscall FUN_10095b20(void *this,undefined4 param_1)

{
  float *pfVar1;
  float fVar2;
  float fVar3;
  code *pcVar4;
  byte *pbVar5;
  void *pvVar6;
  int iVar7;
  short *psVar8;
  int extraout_EDX;
  int iVar9;
  undefined1 *puVar10;
  int *piVar11;
  float10 fVar12;
  float fVar13;
  int iStack_25a44;
  float fStack_25a40;
  float fStack_25a3c;
  float fStack_25a38;
  float fStack_25a30;
  float fStack_25a2c;
  float fStack_25a28;
  float fStack_25a20;
  byte *pbStack_25a1c;
  void *pvStack_25a18;
  void *pvStack_25a14;
  float afStack_25a10 [21];
  undefined4 uStack_259bc;
  undefined4 uStack_259b8;
  float afStack_259b0 [4];
  undefined4 auStack_259a0 [1000];
  undefined1 auStack_24a00 [150012];
  
  if (*(char *)((int)this + 0x49) != '\0') {
    FUN_10095960((int)this);
    *(undefined1 *)((int)this + 0x49) = 0;
  }
  afStack_25a10[0xd] = 0.6;
  iVar9 = 0;
  *(undefined4 *)((int)this + 0x50) = 0;
  *(undefined4 *)((int)this + 0x54) = 0;
  *(undefined4 *)((int)this + 0x58) = 0;
  *(undefined4 *)((int)this + 0x5c) = 0;
  fVar13 = 0.8;
  afStack_25a10[0xe] = 0.4;
  afStack_25a10[0xf] = 0.4;
  afStack_25a10[0x10] = 0.4;
  afStack_25a10[0x11] = 0.4;
  afStack_25a10[0x12] = 0.4;
  puVar10 = auStack_24a00;
  afStack_25a10[10] = 1.0;
  afStack_25a10[0xb] = 1.0;
  afStack_25a10[0xc] = 0.8;
  afStack_25a10[0x13] = 0.0;
  afStack_25a10[0] = 1.0;
  afStack_25a10[1] = 1.0;
  afStack_25a10[2] = 0.8;
  afStack_25a10[3] = 0.8;
  afStack_25a10[4] = 0.8;
  afStack_25a10[5] = 0.8;
  afStack_25a10[6] = 0.8;
  afStack_25a10[7] = 0.8;
  afStack_25a10[8] = 0.8;
  afStack_25a10[9] = 0.0;
  if (*(short *)((int)this + 0x1a) != 0) {
    do {
      pcVar4 = *(code **)(**(int **)(*(int *)((int)this + 0x1c) + iVar9 * 4) + 0xc);
      auStack_259a0[iVar9] = puVar10;
      iVar7 = (*pcVar4)(param_1,puVar10,0x3f800000,0x3f800000);
      puVar10 = puVar10 + iVar7;
      iVar9 = iVar9 + 1;
    } while (iVar9 < (int)(uint)*(ushort *)((int)this + 0x1a));
  }
  iVar9 = 0;
  if (0 < *(int *)((int)this + 0x10)) {
    do {
      fVar2 = afStack_25a10[iVar9 + 10];
      fVar13 = fVar2;
      if (fVar2 == 0.0) break;
      iVar7 = *(ushort *)((int)this + 0x1a) - 1;
      if (-1 < iVar7) {
        fVar3 = afStack_25a10[iVar9];
        fVar13 = fVar3;
        do {
          (**(code **)(**(int **)(*(int *)((int)this + 0x1c) + iVar7 * 4) + 0x10))
                    (param_1,auStack_259a0[iVar7],fVar2,fVar3);
          iVar7 = iVar7 + -1;
        } while (-1 < iVar7);
      }
      iVar7 = 0;
      if (*(short *)((int)this + 0x1a) != 0) {
        fVar3 = afStack_25a10[iVar9];
        fVar13 = fVar3;
        do {
          (**(code **)(**(int **)(*(int *)((int)this + 0x1c) + iVar7 * 4) + 0x10))
                    (param_1,auStack_259a0[iVar7],fVar2,fVar3);
          iVar7 = iVar7 + 1;
        } while (iVar7 < (int)(uint)*(ushort *)((int)this + 0x1a));
      }
      iVar9 = iVar9 + 1;
    } while (iVar9 < *(int *)((int)this + 0x10));
  }
  if (*(int *)((int)this + 0x30) != 0) {
    fVar2 = *(float *)(*(int *)((int)this + 4) + 0xa8);
    fVar12 = FUN_100384d0((float *)(*(int *)((int)this + 4) + 0xb0));
    iVar9 = *(ushort *)((int)this + 0x22) - 1;
    fStack_25a20 = 0.0;
    if (-1 < iVar9) {
      piVar11 = (int *)(*(int *)((int)this + 0x24) + iVar9 * 4);
      do {
        if (*piVar11 != 0) {
          FUN_10095530(*(ushort **)(*piVar11 + 0x94));
          fVar13 = fVar13 + fStack_25a20;
          iVar9 = extraout_EDX;
          fStack_25a20 = fVar13;
        }
        iVar9 = iVar9 + -1;
        piVar11 = piVar11 + -1;
      } while (-1 < iVar9);
    }
    iStack_25a44 = 0;
    if (0 < *(int *)((int)this + 0x30)) {
      fStack_25a20 = fStack_25a20 * (float)((fVar12 + fVar12) * (float10)fVar2);
      psVar8 = (short *)((int)this + 0x36);
      do {
        pvStack_25a18 = *(void **)(*(int *)((int)this + 0x24) + psVar8[-1] * 4);
        pvStack_25a14 = *(void **)(*(int *)((int)this + 0x24) + *psVar8 * 4);
        pbVar5 = *(byte **)((int)pvStack_25a14 + 0x94);
        pbStack_25a1c = *(byte **)((int)pvStack_25a18 + 0x94);
        fStack_25a2c = *(float *)(pbVar5 + 0x94) - *(float *)(pbStack_25a1c + 0x94);
        fStack_25a28 = *(float *)(pbVar5 + 0x98) - *(float *)(pbStack_25a1c + 0x98);
        fStack_25a30 = *(float *)(pbVar5 + 0x90) - *(float *)(pbStack_25a1c + 0x90);
        pfVar1 = (float *)(pbStack_25a1c + 0x90);
        fVar12 = FUN_100384f0(&fStack_25a30);
        pvVar6 = pvStack_25a14;
        if (fVar12 <= (float10)0.01) {
          fStack_25a30 = 1.0;
          fStack_25a2c = 0.0;
          fStack_25a28 = 0.0;
        }
        fStack_25a40 = fStack_25a20 * fStack_25a30;
        fStack_25a3c = fStack_25a2c * fStack_25a20;
        fStack_25a38 = fStack_25a28 * fStack_25a20;
        if ((pbVar5[1] < 8) && ((*pbVar5 & 0x10) == 0)) {
          FUN_10035e50(pvStack_25a14,pfVar1,&fStack_25a40);
          uStack_259bc = 0;
          uStack_259b8 = 0;
          afStack_25a10[0x14] = fVar2;
          FUN_10035de0(pvVar6,afStack_25a10 + 0x14);
        }
        pvVar6 = pvStack_25a18;
        if ((pbStack_25a1c[1] < 8) && ((*pbStack_25a1c & 0x10) == 0)) {
          fStack_25a40 = fStack_25a40 * -1.0;
          fStack_25a3c = fStack_25a3c * -1.0;
          fStack_25a38 = fStack_25a38 * -1.0;
          FUN_10035e50(pvStack_25a18,pfVar1,&fStack_25a40);
          afStack_259b0[0] = -0.0 - fVar2;
          afStack_259b0[1] = 0.0;
          afStack_259b0[2] = 0.0;
          FUN_10035de0(pvVar6,afStack_259b0);
        }
        psVar8 = psVar8 + 2;
        iStack_25a44 = iStack_25a44 + 1;
      } while (iStack_25a44 < *(int *)((int)this + 0x30));
    }
    *(undefined4 *)((int)this + 0x30) = 0;
  }
  if (*(float *)((int)this + 0x50) <= *(float *)((int)this + 0x44) * *(float *)((int)this + 0x44)) {
    *(undefined4 *)((int)this + 0x2c) = 0;
  }
  else if (*(int *)((int)this + 0x2c) <= *(int *)((int)this + 0x28)) {
    *(int *)((int)this + 0x2c) = *(int *)((int)this + 0x2c) + 1;
    return;
  }
  return;
}


// 10095fc0 FUN_10095fc0

int * __thiscall FUN_10095fc0(void *this,byte param_1)

{
  FUN_100956b0((int *)this);
  if (((param_1 & 1) != 0) && (this != (void *)0x0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)(((uint)this & 0xfffffffc) - 4));
  }
  return (int *)this;
}


// 10096050 FUN_10096050

void __thiscall FUN_10096050(void *this,int param_1)

{
  ushort uVar1;
  int iVar2;
  int *piVar3;
  
  iVar2 = *(ushort *)((int)this + 0x1a) - 1;
  if (-1 < iVar2) {
    piVar3 = (int *)(*(int *)((int)this + 0x1c) + iVar2 * 4);
    do {
      if (*piVar3 == param_1) goto LAB_10096079;
      iVar2 = iVar2 + -1;
      piVar3 = piVar3 + -1;
    } while (-1 < iVar2);
  }
  iVar2 = -1;
LAB_10096079:
  uVar1 = *(ushort *)((int)this + 0x1a) - 1;
  *(ushort *)((int)this + 0x1a) = uVar1;
  if (iVar2 < (int)(uint)uVar1) {
    do {
      *(undefined4 *)(*(int *)((int)this + 0x1c) + iVar2 * 4) =
           *(undefined4 *)(*(int *)((int)this + 0x1c) + 4 + iVar2 * 4);
      iVar2 = iVar2 + 1;
    } while (iVar2 < (int)(uint)*(ushort *)((int)this + 0x1a));
  }
  if (*(short *)((int)this + 0x1a) != 0) {
    FUN_100957c0((int)this);
  }
  return;
}


// 100960d0 FUN_100960d0

void __fastcall FUN_100960d0(undefined4 *param_1)

{
  *param_1 = &PTR_LAB_100ce5ac;
  if ((int *)param_1[6] != (int *)0x0) {
    (**(code **)(*(int *)param_1[6] + 4))(1);
  }
  FUN_100a4e40(param_1);
  return;
}


// 10096120 FUN_10096120

void __thiscall FUN_10096120(void *this,undefined4 *param_1)

{
  undefined4 uVar1;
  
  *(undefined4 *)((int)this + 0x18) = *param_1;
  *(undefined4 *)((int)this + 0x1c) = param_1[1];
  uVar1 = param_1[2];
  *(undefined1 *)((int)this + 0x2c) = 0;
  *(undefined4 *)((int)this + 0x20) = uVar1;
  *(undefined4 *)((int)this + 0x24) = param_1[3];
  *(undefined4 *)((int)this + 0x28) = param_1[4];
  return;
}


// 10096150 FUN_10096150

void __thiscall FUN_10096150(void *this,undefined4 *param_1)

{
  *param_1 = *(undefined4 *)((int)this + 0x18);
  param_1[1] = *(undefined4 *)((int)this + 0x1c);
  param_1[2] = *(undefined4 *)((int)this + 0x20);
  param_1[3] = *(undefined4 *)((int)this + 0x24);
  param_1[4] = *(undefined4 *)((int)this + 0x28);
  return;
}


// 10096190 FUN_10096190

int __thiscall FUN_10096190(void *this,float param_1,float param_2)

{
  int iVar1;
  uint3 uVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  
                    /* WARNING: Load size is inaccurate */
  iVar1 = *this;
  fVar5 = *(float *)(iVar1 + 0xd8) - *(float *)((int)this + 0xc);
  fVar3 = *(float *)(iVar1 + 0xd0) - *(float *)((int)this + 4);
  fVar4 = *(float *)(iVar1 + 0xd4) - *(float *)((int)this + 8);
  uVar2 = (uint3)((uint)iVar1 >> 8);
  if (((fVar5 * fVar5 + fVar3 * fVar3 + fVar4 * fVar4) * *(float *)(iVar1 + 0x1c) <
       param_1 * param_1) &&
     (fVar3 = *(float *)(iVar1 + 0x10) * (*(float *)(iVar1 + 0xc0) - *(float *)((int)this + 0x14)),
     fVar4 = *(float *)(iVar1 + 0x14) * (*(float *)(iVar1 + 0xc4) - *(float *)((int)this + 0x18)),
     fVar5 = *(float *)(iVar1 + 0x18) * (*(float *)(iVar1 + 200) - *(float *)((int)this + 0x1c)),
     fVar5 * fVar5 + fVar4 * fVar4 + fVar3 * fVar3 < param_2 * param_2)) {
    return (uint)uVar2 << 8;
  }
  return CONCAT31(uVar2,1);
}


// 10096270 FUN_10096270

void __fastcall FUN_10096270(int param_1)

{
  void *this;
  
  if (((*(char *)(param_1 + 0x2c) != '\0') && (*(char *)(*(int *)(param_1 + 0x10) + 0x48) != '\0'))
     && (this = *(void **)(*(int *)(param_1 + 0x10) + 4), this != (void *)0x0)) {
    FUN_100450c0(this,param_1);
  }
  *(undefined1 *)(param_1 + 0x2c) = 0;
  return;
}


// 100962a0 FUN_100962a0

undefined4 * __thiscall FUN_100962a0(void *this,byte param_1)

{
  FUN_100960d0((undefined4 *)this);
  if (((param_1 & 1) != 0) && (this != (void *)0x0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)(((uint)this & 0xfffffffc) - 4));
  }
  return (undefined4 *)this;
}


// 100962d0 FUN_100962d0

undefined4 * __thiscall FUN_100962d0(void *this,void *param_1,int *param_2)

{
  int *piVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  
  piVar1 = (int *)*param_2;
  iVar2 = piVar1[2];
  iVar3 = piVar1[1];
  iVar4 = (**(code **)(*piVar1 + 8))();
  FUN_100a4e00(this,param_1,iVar3,iVar2,0x195,iVar4);
  *(undefined ***)this = &PTR_LAB_100ce5ac;
  FUN_10096120(this,param_2);
  return (undefined4 *)this;
}


// 10096330 FUN_10096330

int * __thiscall FUN_10096330(void *this,int param_1,float param_2)

{
  undefined4 uVar1;
  undefined4 uVar2;
  int iVar3;
  float fVar4;
  
  iVar3 = *(int *)(param_1 + 0x94);
  *(int *)this = iVar3;
  *(undefined4 *)((int)this + 4) = *(undefined4 *)(iVar3 + 0xd0);
  *(undefined4 *)((int)this + 8) = *(undefined4 *)(iVar3 + 0xd4);
  *(undefined4 *)((int)this + 0xc) = *(undefined4 *)(iVar3 + 0xd8);
                    /* WARNING: Load size is inaccurate */
  iVar3 = *this;
  *(undefined4 *)((int)this + 0x14) = *(undefined4 *)(iVar3 + 0xc0);
  *(undefined4 *)((int)this + 0x18) = *(undefined4 *)(iVar3 + 0xc4);
  *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)(iVar3 + 200);
  *(int *)((int)this + 0x28) = param_1;
  *(float *)((int)this + 0x24) = param_2;
                    /* WARNING: Load size is inaccurate */
  if (((param_2 != 1.0) && (0.0 < param_2)) && ((**this & 0x12) == 0)) {
    iVar3 = *(int *)(param_1 + 0x94);
    uVar1 = *(undefined4 *)(iVar3 + 0x14);
    uVar2 = *(undefined4 *)(iVar3 + 0x18);
    *(undefined4 *)((int)this + 0x30) = *(undefined4 *)(iVar3 + 0x10);
    *(undefined4 *)((int)this + 0x38) = uVar2;
    *(undefined4 *)((int)this + 0x34) = uVar1;
    *(undefined4 *)((int)this + 0x3c) = *(undefined4 *)(iVar3 + 0x1c);
    uVar1 = *(undefined4 *)(iVar3 + 0x34);
    uVar2 = *(undefined4 *)(iVar3 + 0x38);
    *(undefined4 *)((int)this + 0x40) = *(undefined4 *)(iVar3 + 0x30);
    *(undefined4 *)((int)this + 0x48) = uVar2;
    *(undefined4 *)((int)this + 0x44) = uVar1;
    *(undefined4 *)((int)this + 0x4c) = *(undefined4 *)(iVar3 + 0x3c);
                    /* WARNING: Load size is inaccurate */
    if (10000.0 < *(float *)(*this + 0x1c) * param_2) {
      param_2 = 10000.0 / *(float *)(*this + 0x1c);
    }
    *(float *)(iVar3 + 0x10) = *(float *)(iVar3 + 0x10) * param_2;
    *(float *)(iVar3 + 0x14) = *(float *)(iVar3 + 0x14) * param_2;
    *(float *)(iVar3 + 0x18) = *(float *)(iVar3 + 0x18) * param_2;
    fVar4 = 1.0 / param_2;
    *(float *)(iVar3 + 0x1c) = *(float *)(iVar3 + 0x1c) * param_2;
    *(float *)(iVar3 + 0x30) = *(float *)(iVar3 + 0x30) * fVar4;
    *(float *)(iVar3 + 0x34) = *(float *)(iVar3 + 0x34) * fVar4;
    *(float *)(iVar3 + 0x38) = fVar4 * *(float *)(iVar3 + 0x38);
    *(float *)(iVar3 + 0x3c) = *(float *)(iVar3 + 0x3c) * fVar4;
    return (int *)this;
  }
  *(undefined4 *)((int)this + 0x24) = 0x3f800000;
  return (int *)this;
}


// 100964a0 FUN_100964a0

undefined4 __thiscall
FUN_100964a0(void *this,undefined4 param_1,undefined4 param_2,undefined4 param_3,undefined4 param_4)

{
  float fVar1;
  float fVar2;
  undefined1 uVar3;
  int iVar4;
  undefined4 uVar5;
  float unaff_ESI;
  ushort *unaff_EDI;
  float fVar6;
  float fVar7;
  float fVar8;
  float fStack_b8;
  float fStack_b4;
  undefined1 local_b0 [4];
  float fStack_ac;
  float fStack_a8;
  float fStack_a4;
  float fStack_9c;
  int iStack_98;
  undefined4 uStack_90;
  undefined4 uStack_8c;
  undefined4 uStack_88;
  undefined4 uStack_84;
  undefined4 uStack_80;
  undefined4 uStack_7c;
  undefined4 uStack_78;
  undefined4 uStack_74;
  ushort *puStack_70;
  undefined4 uStack_6c;
  undefined4 uStack_68;
  undefined4 uStack_64;
  undefined1 local_60 [4];
  undefined4 uStack_5c;
  undefined4 uStack_58;
  undefined4 uStack_54;
  float fStack_4c;
  int iStack_48;
  undefined4 uStack_40;
  undefined4 uStack_3c;
  undefined4 uStack_38;
  undefined4 uStack_34;
  undefined4 uStack_30;
  undefined4 uStack_2c;
  undefined4 uStack_28;
  undefined4 uStack_24;
  
  *(undefined1 *)((int)this + 0x2c) = 0;
  FUN_10096330(local_b0,*(int *)((int)this + 4),*(float *)((int)this + 0x24));
  FUN_10096330(local_60,*(int *)((int)this + 8),*(float *)((int)this + 0x28));
  (**(code **)(**(int **)((int)this + 0x18) + 0xc))(param_1,param_2,param_3,param_4);
  fVar1 = *(float *)((int)this + 0x1c);
  fVar2 = *(float *)((int)this + 0x20);
  if ((((*(float *)(unaff_EDI + 0x6a) - fStack_b8) * (*(float *)(unaff_EDI + 0x6a) - fStack_b8) +
        (*(float *)(unaff_EDI + 0x6c) - fStack_b4) * (*(float *)(unaff_EDI + 0x6c) - fStack_b4) +
       (*(float *)(unaff_EDI + 0x68) - unaff_ESI) * (*(float *)(unaff_EDI + 0x68) - unaff_ESI)) *
       *(float *)(unaff_EDI + 0xe) < fVar1 * fVar1) &&
     (fVar6 = *(float *)(unaff_EDI + 8) * (*(float *)(unaff_EDI + 0x60) - fStack_ac),
     fVar7 = *(float *)(unaff_EDI + 10) * (*(float *)(unaff_EDI + 0x62) - fStack_a8),
     fVar8 = *(float *)(unaff_EDI + 0xc) * (*(float *)(unaff_EDI + 100) - fStack_a4),
     fVar8 * fVar8 + fVar7 * fVar7 + fVar6 * fVar6 < fVar2 * fVar2)) {
    iVar4 = FUN_10096190(&puStack_70,fVar1,fVar2);
    if ((char)iVar4 == '\0') {
      uVar3 = 0;
      goto LAB_1009660a;
    }
  }
  uVar3 = 1;
LAB_1009660a:
  *(undefined1 *)((int)this + 0x2c) = uVar3;
  FUN_10096270((int)this);
  if (*(char *)(*(int *)((int)this + 0x10) + 0x48) == '\0') {
    if ((*unaff_EDI & 0x12) == 0) {
      *(float *)(unaff_EDI + 0x68) = unaff_ESI;
      *(float *)(unaff_EDI + 0x6a) = fStack_b8;
      *(float *)(unaff_EDI + 0x6c) = fStack_b4;
      *(float *)(unaff_EDI + 0x60) = fStack_ac;
      *(float *)(unaff_EDI + 0x62) = fStack_a8;
      *(float *)(unaff_EDI + 100) = fStack_a4;
    }
    if ((*puStack_70 & 0x12) == 0) {
      *(undefined4 *)(puStack_70 + 0x68) = uStack_6c;
      *(undefined4 *)(puStack_70 + 0x6a) = uStack_68;
      *(undefined4 *)(puStack_70 + 0x6c) = uStack_64;
      *(undefined4 *)(puStack_70 + 0x60) = uStack_5c;
      *(undefined4 *)(puStack_70 + 0x62) = uStack_58;
      *(undefined4 *)(puStack_70 + 100) = uStack_54;
    }
  }
  uVar5 = (**(code **)(**(int **)((int)this + 0x18) + 8))();
  if ((fStack_4c != 1.0) && (0.0 < fStack_4c)) {
    iVar4 = *(int *)(iStack_48 + 0x94);
    *(undefined4 *)(iVar4 + 0x10) = uStack_40;
    *(undefined4 *)(iVar4 + 0x14) = uStack_3c;
    *(undefined4 *)(iVar4 + 0x18) = uStack_38;
    *(undefined4 *)(iVar4 + 0x1c) = uStack_34;
    *(undefined4 *)(iVar4 + 0x30) = uStack_30;
    *(undefined4 *)(iVar4 + 0x34) = uStack_2c;
    *(undefined4 *)(iVar4 + 0x38) = uStack_28;
    *(undefined4 *)(iVar4 + 0x3c) = uStack_24;
  }
  if ((fStack_9c != 1.0) && (0.0 < fStack_9c)) {
    iVar4 = *(int *)(iStack_98 + 0x94);
    *(undefined4 *)(iVar4 + 0x10) = uStack_90;
    *(undefined4 *)(iVar4 + 0x14) = uStack_8c;
    *(undefined4 *)(iVar4 + 0x18) = uStack_88;
    *(undefined4 *)(iVar4 + 0x1c) = uStack_84;
    *(undefined4 *)(iVar4 + 0x30) = uStack_80;
    *(undefined4 *)(iVar4 + 0x34) = uStack_7c;
    *(undefined4 *)(iVar4 + 0x38) = uStack_78;
    *(undefined4 *)(iVar4 + 0x3c) = uStack_74;
  }
  return uVar5;
}


// 10096810 FUN_10096810

void __thiscall
FUN_10096810(void *this,undefined4 param_1,undefined4 param_2,undefined4 param_3,undefined4 param_4)

{
  float fVar1;
  float fVar2;
  undefined1 uVar3;
  int iVar4;
  float unaff_ESI;
  ushort *unaff_EDI;
  float fVar5;
  float fVar6;
  float fVar7;
  float fStack_b8;
  float fStack_b4;
  undefined1 local_b0 [4];
  float fStack_ac;
  float fStack_a8;
  float fStack_a4;
  float fStack_9c;
  int iStack_98;
  undefined4 uStack_90;
  undefined4 uStack_8c;
  undefined4 uStack_88;
  undefined4 uStack_84;
  undefined4 uStack_80;
  undefined4 uStack_7c;
  undefined4 uStack_78;
  undefined4 uStack_74;
  ushort *puStack_70;
  undefined4 uStack_6c;
  undefined4 uStack_68;
  undefined4 uStack_64;
  undefined1 local_60 [4];
  undefined4 uStack_5c;
  undefined4 uStack_58;
  undefined4 uStack_54;
  float fStack_4c;
  int iStack_48;
  undefined4 uStack_40;
  undefined4 uStack_3c;
  undefined4 uStack_38;
  undefined4 uStack_34;
  undefined4 uStack_30;
  undefined4 uStack_2c;
  undefined4 uStack_28;
  undefined4 uStack_24;
  
  FUN_10096330(local_b0,*(int *)((int)this + 4),*(float *)((int)this + 0x24));
  FUN_10096330(local_60,*(int *)((int)this + 8),*(float *)((int)this + 0x28));
  (**(code **)(**(int **)((int)this + 0x18) + 0x10))(param_1,param_2,param_3,param_4);
  fVar1 = *(float *)((int)this + 0x1c);
  fVar2 = *(float *)((int)this + 0x20);
  if ((((*(float *)(unaff_EDI + 0x6a) - fStack_b8) * (*(float *)(unaff_EDI + 0x6a) - fStack_b8) +
        (*(float *)(unaff_EDI + 0x6c) - fStack_b4) * (*(float *)(unaff_EDI + 0x6c) - fStack_b4) +
       (*(float *)(unaff_EDI + 0x68) - unaff_ESI) * (*(float *)(unaff_EDI + 0x68) - unaff_ESI)) *
       *(float *)(unaff_EDI + 0xe) < fVar1 * fVar1) &&
     (fVar5 = *(float *)(unaff_EDI + 8) * (*(float *)(unaff_EDI + 0x60) - fStack_ac),
     fVar6 = *(float *)(unaff_EDI + 10) * (*(float *)(unaff_EDI + 0x62) - fStack_a8),
     fVar7 = *(float *)(unaff_EDI + 0xc) * (*(float *)(unaff_EDI + 100) - fStack_a4),
     fVar7 * fVar7 + fVar6 * fVar6 + fVar5 * fVar5 < fVar2 * fVar2)) {
    iVar4 = FUN_10096190(&puStack_70,fVar1,fVar2);
    if ((char)iVar4 == '\0') {
      uVar3 = 0;
      goto LAB_10096976;
    }
  }
  uVar3 = 1;
LAB_10096976:
  *(undefined1 *)((int)this + 0x2c) = uVar3;
  FUN_10096270((int)this);
  if (*(char *)(*(int *)((int)this + 0x10) + 0x48) == '\0') {
    if ((*unaff_EDI & 0x12) == 0) {
      *(float *)(unaff_EDI + 0x68) = unaff_ESI;
      *(float *)(unaff_EDI + 0x6a) = fStack_b8;
      *(float *)(unaff_EDI + 0x6c) = fStack_b4;
      *(float *)(unaff_EDI + 0x60) = fStack_ac;
      *(float *)(unaff_EDI + 0x62) = fStack_a8;
      *(float *)(unaff_EDI + 100) = fStack_a4;
    }
    if ((*puStack_70 & 0x12) == 0) {
      *(undefined4 *)(puStack_70 + 0x68) = uStack_6c;
      *(undefined4 *)(puStack_70 + 0x6a) = uStack_68;
      *(undefined4 *)(puStack_70 + 0x6c) = uStack_64;
      *(undefined4 *)(puStack_70 + 0x60) = uStack_5c;
      *(undefined4 *)(puStack_70 + 0x62) = uStack_58;
      *(undefined4 *)(puStack_70 + 100) = uStack_54;
    }
  }
  if ((fStack_4c != 1.0) && (0.0 < fStack_4c)) {
    iVar4 = *(int *)(iStack_48 + 0x94);
    *(undefined4 *)(iVar4 + 0x10) = uStack_40;
    *(undefined4 *)(iVar4 + 0x14) = uStack_3c;
    *(undefined4 *)(iVar4 + 0x18) = uStack_38;
    *(undefined4 *)(iVar4 + 0x1c) = uStack_34;
    *(undefined4 *)(iVar4 + 0x30) = uStack_30;
    *(undefined4 *)(iVar4 + 0x34) = uStack_2c;
    *(undefined4 *)(iVar4 + 0x38) = uStack_28;
    *(undefined4 *)(iVar4 + 0x3c) = uStack_24;
  }
  if ((fStack_9c != 1.0) && (0.0 < fStack_9c)) {
    iVar4 = *(int *)(iStack_98 + 0x94);
    *(undefined4 *)(iVar4 + 0x10) = uStack_90;
    *(undefined4 *)(iVar4 + 0x14) = uStack_8c;
    *(undefined4 *)(iVar4 + 0x18) = uStack_88;
    *(undefined4 *)(iVar4 + 0x1c) = uStack_84;
    *(undefined4 *)(iVar4 + 0x30) = uStack_80;
    *(undefined4 *)(iVar4 + 0x34) = uStack_7c;
    *(undefined4 *)(iVar4 + 0x38) = uStack_78;
    *(undefined4 *)(iVar4 + 0x3c) = uStack_74;
  }
  return;
}


// 10096be0 FUN_10096be0

void __thiscall FUN_10096be0(void *this,undefined4 param_1,float param_2)

{
  *(bool *)((int)this + 0x25) = param_2 != 0.0;
  *(float *)((int)this + 0x34) = ABS(param_2);
  *(undefined4 *)((int)this + 0x38) = param_1;
  return;
}


// 10096c20 FUN_10096c20

void __thiscall FUN_10096c20(void *this,int param_1)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  
  *(undefined4 *)(param_1 + 0x2c) = *(undefined4 *)((int)this + 0x1c);
  *(undefined4 *)(param_1 + 0x28) = *(undefined4 *)((int)this + 0x18);
  *(undefined4 *)(param_1 + 8) = *(undefined4 *)((int)this + 0x60);
  *(undefined4 *)(param_1 + 0xc) = *(undefined4 *)((int)this + 100);
  *(undefined4 *)(param_1 + 0x10) = *(undefined4 *)((int)this + 0x68);
  *(undefined4 *)(param_1 + 0x14) = *(undefined4 *)((int)this + 0x6c);
  *(undefined4 *)(param_1 + 0x18) = *(undefined4 *)((int)this + 0x70);
  *(undefined4 *)(param_1 + 0x1c) = *(undefined4 *)((int)this + 0x74);
  *(undefined4 *)(param_1 + 0x20) = *(undefined4 *)((int)this + 0x78);
  *(undefined4 *)(param_1 + 0x30) = *(undefined4 *)((int)this + 0x40);
  *(undefined4 *)(param_1 + 0x34) = *(undefined4 *)((int)this + 0x44);
  *(undefined4 *)(param_1 + 0x38) = *(undefined4 *)((int)this + 0x48);
  puVar2 = (undefined4 *)((int)this + 0x24);
  puVar3 = (undefined4 *)(param_1 + 0x40);
  for (iVar1 = 7; iVar1 != 0; iVar1 = iVar1 + -1) {
    *puVar3 = *puVar2;
    puVar2 = puVar2 + 1;
    puVar3 = puVar3 + 1;
  }
  return;
}


// 10096c80 FUN_10096c80

void __fastcall FUN_10096c80(int param_1)

{
  int iVar1;
  int iVar2;
  float *pfVar3;
  undefined4 *puVar4;
  float *pfVar5;
  undefined4 *puVar6;
  float local_e0;
  undefined4 local_dc;
  undefined4 local_d8;
  undefined4 local_d4;
  undefined4 local_d0;
  undefined4 local_cc;
  undefined4 local_c8;
  undefined4 local_c4;
  undefined4 local_c0;
  undefined4 local_bc;
  undefined4 local_b8;
  undefined4 local_b4;
  float local_b0;
  float local_ac;
  float local_a8;
  undefined4 local_a4;
  float local_a0 [4];
  undefined4 local_90;
  undefined4 local_8c;
  undefined4 local_88;
  undefined4 local_80;
  undefined4 local_7c;
  undefined4 local_78;
  float local_70;
  float local_6c;
  float local_68;
  float local_60;
  float local_5c;
  float local_58;
  undefined4 local_50 [12];
  float local_20;
  float local_1c;
  float local_18;
  
  iVar1 = *(int *)(param_1 + 4);
  if ((*(uint *)(iVar1 + 0x44) & 0x800) == 0) {
    pfVar3 = (float *)(*(int *)(iVar1 + 0x94) + 0x60);
    pfVar5 = local_a0;
    for (iVar2 = 0x10; iVar2 != 0; iVar2 = iVar2 + -1) {
      *pfVar5 = *pfVar3;
      pfVar3 = pfVar3 + 1;
      pfVar5 = pfVar5 + 1;
    }
    FUN_10039500(local_a0,(float *)(iVar1 + 0x30),&local_70);
    local_e0 = local_a0[0];
    local_d0 = local_a0[1];
    local_dc = local_90;
    local_cc = local_8c;
    local_d8 = local_80;
    local_c8 = local_7c;
    local_c0 = local_a0[2];
    local_b0 = local_70;
    local_bc = local_88;
    local_ac = local_6c;
    local_b8 = local_78;
  }
  else {
    iVar1 = *(int *)(iVar1 + 0x94);
    local_e0 = *(float *)(iVar1 + 0x60);
    local_dc = *(undefined4 *)(iVar1 + 0x70);
    local_d8 = *(undefined4 *)(iVar1 + 0x80);
    local_d0 = *(undefined4 *)(iVar1 + 100);
    local_cc = *(undefined4 *)(iVar1 + 0x74);
    local_c8 = *(undefined4 *)(iVar1 + 0x84);
    local_c0 = *(undefined4 *)(iVar1 + 0x68);
    local_bc = *(undefined4 *)(iVar1 + 0x78);
    local_b8 = *(undefined4 *)(iVar1 + 0x88);
    local_b0 = *(float *)(iVar1 + 0x90);
    local_ac = *(float *)(iVar1 + 0x94);
    local_68 = *(float *)(iVar1 + 0x98);
  }
  local_b4 = 0;
  local_c4 = 0;
  local_d4 = 0;
  iVar1 = *(int *)(param_1 + 8);
  local_a4 = 0x3f800000;
  local_a8 = local_68;
  if ((*(uint *)(iVar1 + 0x44) & 0x800) == 0) {
    puVar4 = (undefined4 *)(*(int *)(iVar1 + 0x94) + 0x60);
    puVar6 = local_50;
    for (iVar2 = 0x10; iVar2 != 0; iVar2 = iVar2 + -1) {
      *puVar6 = *puVar4;
      puVar4 = puVar4 + 1;
      puVar6 = puVar6 + 1;
    }
    FUN_10039500(local_50,(float *)(iVar1 + 0x30),&local_20);
    local_70 = local_20;
    local_6c = local_1c;
  }
  else {
    iVar1 = *(int *)(iVar1 + 0x94);
    local_70 = *(float *)(iVar1 + 0x90);
    local_6c = *(float *)(iVar1 + 0x94);
    local_18 = *(float *)(iVar1 + 0x98);
  }
  local_68 = local_18;
  FUN_100b9e30(&local_60,&local_e0,(float *)(param_1 + 0x40));
  *(float *)(param_1 + 0x3c) =
       local_58 * ((local_68 - local_a8) - *(float *)(param_1 + 0x78)) +
       local_5c * ((local_6c - local_ac) - *(float *)(param_1 + 0x74)) +
       local_60 * ((local_70 - local_b0) - *(float *)(param_1 + 0x70));
  return;
}


// 10096f70 FUN_10096f70

void __fastcall FUN_10096f70(undefined4 *param_1)

{
  int iVar1;
  
  param_1[0x31] = param_1 + 8;
  param_1[0x2e] = param_1 + 4;
  param_1[0x30] = param_1 + 0x18;
  param_1[0x2d] = param_1 + 0xc;
  param_1[0x32] = 0;
  param_1[0x33] = 0;
  iVar1 = param_1[0x27] * param_1[0x25] + -1;
  if (3 < (int)(param_1[0x27] * param_1[0x25])) {
    do {
      *(undefined4 *)(param_1[0x24] + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[0x24] + -4 + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[0x24] + -8 + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[0x24] + -0xc + iVar1 * 4) = 0;
      iVar1 = iVar1 + -4;
    } while (2 < iVar1);
  }
  while (-1 < iVar1) {
    iVar1 = iVar1 + -1;
    *(undefined4 *)(param_1[0x24] + 4 + iVar1 * 4) = 0;
  }
  param_1[3] = 0;
  param_1[2] = 0;
  param_1[1] = 0;
  *param_1 = 0;
  return;
}


// 10097030 FUN_10097030

void __fastcall FUN_10097030(undefined4 *param_1)

{
  int iVar1;
  
  param_1[0xd9] = param_1 + 0x1c;
  param_1[0xd6] = param_1 + 8;
  param_1[0xd8] = param_1 + 0x6c;
  param_1[0xd5] = param_1 + 0x30;
  param_1[0xda] = 0;
  param_1[0xdb] = 0;
  iVar1 = param_1[0xab] * param_1[0xa9] + -1;
  if (3 < (int)(param_1[0xab] * param_1[0xa9])) {
    do {
      *(undefined4 *)(param_1[0xa8] + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[0xa8] + -4 + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[0xa8] + -8 + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[0xa8] + -0xc + iVar1 * 4) = 0;
      iVar1 = iVar1 + -4;
    } while (2 < iVar1);
  }
  while (-1 < iVar1) {
    iVar1 = iVar1 + -1;
    *(undefined4 *)(param_1[0xa8] + 4 + iVar1 * 4) = 0;
  }
  param_1[3] = 0;
  param_1[2] = 0;
  param_1[1] = 0;
  *param_1 = 0;
  param_1[7] = 0;
  param_1[6] = 0;
  param_1[5] = 0;
  param_1[4] = 0;
  return;
}


// 10097100 FUN_10097100

void __thiscall FUN_10097100(void *this,int param_1)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float *pfVar4;
  float fVar5;
  float local_10 [4];
  
  pfVar4 = (float *)((int)this + 0x40);
  *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)(param_1 + 0x2c);
  *(undefined4 *)((int)this + 0x18) = *(undefined4 *)(param_1 + 0x28);
  *(undefined4 *)((int)this + 0x60) = *(undefined4 *)(param_1 + 8);
  *(undefined4 *)((int)this + 100) = *(undefined4 *)(param_1 + 0xc);
  *(undefined4 *)((int)this + 0x68) = *(undefined4 *)(param_1 + 0x10);
  *(undefined4 *)((int)this + 0x6c) = *(undefined4 *)(param_1 + 0x14);
  *(undefined4 *)((int)this + 0x70) = *(undefined4 *)(param_1 + 0x18);
  *(undefined4 *)((int)this + 0x74) = *(undefined4 *)(param_1 + 0x1c);
  *(undefined4 *)((int)this + 0x78) = *(undefined4 *)(param_1 + 0x20);
  *pfVar4 = *(float *)(param_1 + 0x30);
  *(undefined4 *)((int)this + 0x44) = *(undefined4 *)(param_1 + 0x34);
  *(undefined4 *)((int)this + 0x48) = *(undefined4 *)(param_1 + 0x38);
  fVar1 = *(float *)((int)this + 0x44);
  fVar2 = *pfVar4;
  fVar3 = *(float *)((int)this + 0x48);
  fVar5 = fVar2 * fVar2 + fVar1 * fVar1 + fVar3 * fVar3;
  if (fVar5 == 0.0) {
    fVar5 = 0.0;
  }
  else {
    fVar5 = 1.0 / SQRT(fVar5);
  }
  *pfVar4 = fVar2 * fVar5;
  *(float *)((int)this + 0x44) = fVar1 * fVar5;
  *(float *)((int)this + 0x48) = fVar3 * fVar5;
  pfVar4 = FUN_100b9f50(local_10,pfVar4);
  *(float *)((int)this + 0x50) = *pfVar4;
  *(float *)((int)this + 0x54) = pfVar4[1];
  *(float *)((int)this + 0x58) = pfVar4[2];
  *(undefined1 *)((int)this + 0x24) = *(undefined1 *)(param_1 + 0x40);
  *(undefined1 *)((int)this + 0x25) = *(undefined1 *)(param_1 + 0x41);
  *(undefined4 *)((int)this + 0x28) = *(undefined4 *)(param_1 + 0x44);
  *(undefined4 *)((int)this + 0x2c) = *(undefined4 *)(param_1 + 0x48);
  *(undefined4 *)((int)this + 0x30) = *(undefined4 *)(param_1 + 0x4c);
  *(float *)((int)this + 0x34) = ABS(*(float *)(param_1 + 0x50));
  *(undefined4 *)((int)this + 0x38) = *(undefined4 *)(param_1 + 0x54);
  fVar1 = *(float *)(*(int *)(*(int *)((int)this + 4) + 0x94) + 0x1c);
  fVar2 = *(float *)(*(int *)(*(int *)((int)this + 8) + 0x94) + 0x1c);
  if (fVar1 == 0.0) {
    *(undefined4 *)((int)this + 0x20) = 0x3f800000;
    FUN_10096c80((int)this);
    return;
  }
  if (fVar2 == 0.0) {
    *(undefined4 *)((int)this + 0x20) = 0;
    FUN_10096c80((int)this);
    return;
  }
  *(float *)((int)this + 0x20) = fVar2 / (fVar2 + fVar1);
  FUN_10096c80((int)this);
  return;
}


// 10097290 FUN_10097290

undefined4 * __thiscall
FUN_10097290(void *this,undefined4 param_1,int param_2,undefined4 param_3,undefined4 param_4)

{
  FUN_100a4de0(this,param_1,param_3,param_4);
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined ***)this = &PTR_LAB_100ce640;
  *(undefined1 *)((int)this + 0x24) = 0;
  *(undefined1 *)((int)this + 0x25) = 0;
  *(undefined4 *)((int)this + 0x28) = 0;
  *(undefined4 *)((int)this + 0x2c) = 0;
  *(undefined4 *)((int)this + 0x30) = 0x3f800000;
  *(undefined4 *)((int)this + 0x34) = 0;
  *(undefined4 *)((int)this + 0x38) = 0;
  *(undefined4 *)((int)this + 0x3c) = 0;
  FUN_10097100(this,param_2);
  return (undefined4 *)this;
}


// 10097310 FUN_10097310

undefined4 * __thiscall
FUN_10097310(void *this,void *param_1,int param_2,undefined4 param_3,undefined4 param_4)

{
  FUN_100a4e00(this,param_1,param_3,param_4,0x195,0x3a0);
  *(undefined ***)this = &PTR_LAB_100ce640;
  *(undefined1 *)((int)this + 0x24) = 0;
  *(undefined1 *)((int)this + 0x25) = 0;
  *(undefined4 *)((int)this + 0x28) = 0;
  *(undefined4 *)((int)this + 0x2c) = 0;
  *(undefined4 *)((int)this + 0x30) = 0x3f800000;
  *(undefined4 *)((int)this + 0x34) = 0;
  *(undefined4 *)((int)this + 0x38) = 0;
  *(undefined4 *)((int)this + 0x3c) = 0;
  FUN_10097100(this,param_2);
  return (undefined4 *)this;
}


// 10097380 FUN_10097380

void __thiscall FUN_10097380(void *this,float *param_1,int param_2,float param_3,float param_4)

{
  float *pfVar1;
  float *pfVar2;
  float *pfVar3;
  float *pfVar4;
  float *pfVar5;
  int iVar6;
  float *pfVar7;
  int iVar8;
  int iVar9;
  float *pfVar10;
  undefined4 *puVar11;
  undefined4 *puVar12;
  float fVar13;
  float fVar14;
  float fVar15;
  float fVar16;
  float fVar17;
  float fVar18;
  float fVar19;
  float fVar20;
  float fVar21;
  float local_244;
  int *local_240;
  int local_23c;
  int local_238;
  int *local_234;
  float local_230;
  undefined4 local_22c;
  undefined4 local_228;
  undefined4 local_224;
  undefined4 local_220;
  undefined4 local_21c;
  undefined4 local_218;
  undefined4 local_214;
  undefined4 local_210;
  undefined4 local_20c;
  undefined4 local_208;
  undefined4 local_204;
  float local_200;
  float local_1fc;
  float local_1f8;
  undefined4 local_1f4;
  float local_1f0;
  float local_1ec;
  float local_1e8;
  float local_1e0;
  float local_1dc;
  float local_1d8;
  float local_1d0 [4];
  undefined4 local_1c0;
  undefined4 local_1bc;
  undefined4 local_1b8;
  undefined4 local_1b0;
  undefined4 local_1ac;
  undefined4 local_1a8;
  float local_1a0;
  float local_19c;
  float local_198;
  float local_190;
  float local_18c;
  float local_188;
  undefined1 *local_180;
  float local_17c [3];
  undefined1 local_170 [16];
  float local_160;
  float local_150 [12];
  float afStack_120 [8];
  int aiStack_100 [4];
  float afStack_f0 [8];
  int aiStack_d0 [4];
  float *local_c0;
  undefined4 local_bc;
  undefined4 local_b8;
  undefined4 local_b4;
  float local_b0 [4];
  int local_a0 [3];
  int local_94 [3];
  int local_88;
  int local_84;
  undefined1 *local_80;
  undefined4 local_7c;
  undefined4 local_78;
  undefined1 local_70 [32];
  undefined4 local_50 [12];
  float local_20;
  float local_1c;
  float local_18;
  
  iVar9 = *(int *)((int)this + 4);
  local_23c = *(int *)((int)this + 8);
  local_238 = iVar9;
  if (*(char *)((int)this + 0x25) != '\0' || *(char *)((int)this + 0x24) != '\0') {
    local_240 = (int *)this;
    if ((*(uint *)(iVar9 + 0x44) & 0x800) == 0) {
      pfVar7 = (float *)(*(int *)(iVar9 + 0x94) + 0x60);
      pfVar10 = local_1d0;
      for (iVar8 = 0x10; iVar8 != 0; iVar8 = iVar8 + -1) {
        *pfVar10 = *pfVar7;
        pfVar7 = pfVar7 + 1;
        pfVar10 = pfVar10 + 1;
      }
      FUN_10039500(local_1d0,(float *)(iVar9 + 0x30),&local_1a0);
      local_230 = local_1d0[0];
      local_220 = local_1d0[1];
      local_22c = local_1c0;
      local_21c = local_1bc;
      local_228 = local_1b0;
      local_218 = local_1ac;
      local_210 = local_1d0[2];
      local_200 = local_1a0;
      local_20c = local_1b8;
      local_1fc = local_19c;
      local_208 = local_1a8;
    }
    else {
      iVar9 = *(int *)(iVar9 + 0x94);
      local_230 = *(float *)(iVar9 + 0x60);
      local_22c = *(undefined4 *)(iVar9 + 0x70);
      local_228 = *(undefined4 *)(iVar9 + 0x80);
      local_220 = *(undefined4 *)(iVar9 + 100);
      local_21c = *(undefined4 *)(iVar9 + 0x74);
      local_218 = *(undefined4 *)(iVar9 + 0x84);
      local_210 = *(undefined4 *)(iVar9 + 0x68);
      local_20c = *(undefined4 *)(iVar9 + 0x78);
      local_208 = *(undefined4 *)(iVar9 + 0x88);
      local_200 = *(float *)(iVar9 + 0x90);
      local_1fc = *(float *)(iVar9 + 0x94);
      local_198 = *(float *)(iVar9 + 0x98);
    }
    local_204 = 0;
    local_214 = 0;
    local_224 = 0;
    local_1f4 = 0x3f800000;
    local_1f8 = local_198;
    if ((*(uint *)(local_23c + 0x44) & 0x800) == 0) {
      puVar11 = (undefined4 *)(*(int *)(local_23c + 0x94) + 0x60);
      puVar12 = local_50;
      for (iVar9 = 0x10; iVar9 != 0; iVar9 = iVar9 + -1) {
        *puVar12 = *puVar11;
        puVar11 = puVar11 + 1;
        puVar12 = puVar12 + 1;
      }
      FUN_10039500(local_50,(float *)(local_23c + 0x30),&local_20);
      local_1a0 = local_20;
      local_19c = local_1c;
    }
    else {
      iVar9 = *(int *)(local_23c + 0x94);
      local_1a0 = *(float *)(iVar9 + 0x90);
      local_19c = *(float *)(iVar9 + 0x94);
      local_18 = *(float *)(iVar9 + 0x98);
    }
    this = local_240;
    local_198 = local_18;
    FUN_100b9e30(&local_1f0,&local_230,(float *)((int)local_240 + 0x40));
    fVar21 = *(float *)((int)this + 0x20);
    fVar13 = 1.0 - fVar21;
    local_1e0 = local_1a0 * fVar21 + local_200 * fVar13;
    local_1dc = local_19c * fVar21 + local_1fc * fVar13;
    local_1d8 = local_198 * fVar21 + local_1f8 * fVar13;
    FUN_100b9e30(&local_190,&local_230,(float *)((int)this + 0x70));
    local_c0 = local_b0;
    local_244 = local_1f0 * ((local_1a0 - local_200) - local_190) +
                local_1e8 * ((local_198 - local_1f8) - local_188) +
                local_1ec * ((local_19c - local_1fc) - local_18c);
    local_bc = 1;
    local_b8 = 1;
    local_b4 = 4;
    FUN_10096f70(local_150);
    iVar6 = local_88;
    iVar9 = local_238;
    iVar8 = *(int *)(local_238 + 0x94);
    fVar21 = *(float *)(iVar8 + 0x94);
    fVar13 = *(float *)(iVar8 + 0x98);
    pfVar7 = (float *)((int)afStack_120 + local_88);
    fVar20 = local_1e0 - *(float *)(iVar8 + 0x90);
    *pfVar7 = local_1f0;
    *(float *)((int)afStack_120 + iVar6 + 4) = local_1ec;
    fVar21 = local_1dc - fVar21;
    fVar13 = local_1d8 - fVar13;
    *(float *)((int)afStack_120 + iVar6 + 8) = local_1e8;
    fVar15 = *pfVar7;
    fVar17 = *(float *)((int)afStack_120 + iVar6 + 4);
    local_240 = (int *)(fVar15 * fVar21);
    fVar19 = *(float *)((int)afStack_120 + iVar6 + 8);
    fVar16 = fVar15 * fVar13 - fVar19 * fVar20;
    *(float *)((int)afStack_120 + iVar6 + 0x14) = fVar16;
    fVar18 = fVar19 * fVar21 - fVar17 * fVar13;
    *(float *)((int)afStack_120 + iVar6 + 0x10) = fVar18;
    fVar14 = fVar17 * fVar20 - (float)local_240;
    *(float *)((int)afStack_120 + iVar6 + 0x18) = fVar14;
    fVar21 = *(float *)(iVar8 + 0x78);
    fVar13 = *(float *)(iVar8 + 0x68);
    fVar15 = *(float *)(iVar8 + 0x60);
    fVar17 = *(float *)(iVar8 + 0x88);
    fVar19 = *(float *)(iVar8 + 0x70);
    fVar20 = *(float *)(iVar8 + 0x80);
    *(float *)((int)afStack_120 + iVar6 + 0x14) =
         *(float *)(iVar8 + 100) * fVar18 + *(float *)(iVar8 + 0x74) * fVar16 +
         *(float *)(iVar8 + 0x84) * fVar14;
    *(float *)((int)afStack_120 + iVar6 + 0x18) =
         fVar13 * fVar18 + fVar21 * fVar16 + fVar17 * fVar14;
    *(float *)((int)afStack_120 + iVar6 + 0x10) =
         fVar19 * fVar16 + fVar20 * fVar14 + fVar18 * fVar15;
    *(int *)((int)aiStack_100 + iVar6) = local_84;
    iVar6 = local_88;
    iVar8 = *(int *)(local_23c + 0x94);
    fVar19 = local_1e0 - *(float *)(iVar8 + 0x90);
    fVar15 = local_1dc - *(float *)(iVar8 + 0x94);
    fVar17 = local_1d8 - *(float *)(iVar8 + 0x98);
    *(float *)((int)afStack_f0 + local_88) = local_1f0 * -1.0;
    *(float *)((int)afStack_f0 + iVar6 + 4) = local_1ec * -1.0;
    *(float *)((int)afStack_f0 + iVar6 + 8) = local_1e8 * -1.0;
    fVar21 = *(float *)((int)afStack_f0 + iVar6 + 4);
    fVar16 = fVar21 * fVar19 - fVar15 * *(float *)((int)afStack_f0 + iVar6);
    fVar13 = *(float *)((int)afStack_f0 + iVar6 + 8);
    *(float *)((int)afStack_f0 + iVar6 + 0x18) = fVar16;
    fVar18 = fVar17 * *(float *)((int)afStack_f0 + iVar6) - fVar19 * fVar13;
    *(float *)((int)afStack_f0 + iVar6 + 0x14) = fVar18;
    fVar14 = fVar15 * fVar13 - fVar21 * fVar17;
    *(float *)((int)afStack_f0 + iVar6 + 0x10) = fVar14;
    fVar21 = *(float *)(iVar8 + 0x78);
    fVar13 = *(float *)(iVar8 + 0x68);
    fVar15 = *(float *)(iVar8 + 0x88);
    fVar17 = *(float *)(iVar8 + 0x70);
    fVar19 = *(float *)(iVar8 + 0x80);
    fVar20 = *(float *)(iVar8 + 0x60);
    *(float *)((int)afStack_f0 + iVar6 + 0x14) =
         *(float *)(iVar8 + 100) * fVar14 + *(float *)(iVar8 + 0x74) * fVar18 +
         *(float *)(iVar8 + 0x84) * fVar16;
    *(float *)((int)afStack_f0 + iVar6 + 0x18) = fVar13 * fVar14 + fVar21 * fVar18 + fVar15 * fVar16
    ;
    *(float *)((int)afStack_f0 + iVar6 + 0x10) = fVar17 * fVar18 + fVar19 * fVar16 + fVar20 * fVar14
    ;
    *(int *)((int)aiStack_d0 + iVar6) = local_84;
    local_88 = local_88 + 0x30;
    local_a0[0] = local_84 + 1;
    local_94[0] = local_a0[0];
    local_84 = local_a0[0];
    FUN_100a4e60(*(void **)(local_238 + 0x94),local_a0,(int *)&local_c0,(int)local_150);
    local_a0[0] = local_84;
    local_94[0] = local_84;
    FUN_100a4e60(*(void **)(local_23c + 0x94),local_94,(int *)&local_c0,(int)local_150);
    fVar21 = *local_c0;
    fVar13 = 0.0;
    if (*(char *)((int)this + 0x25) != '\0') {
      local_234 = (int *)(local_244 - *(float *)((int)this + 0x3c));
      fVar15 = *(float *)((int)this + 0x38) * *param_1 + *(float *)((int)this + 0x3c);
      *(float *)((int)this + 0x3c) = fVar15;
      fVar13 = (param_3 * 0.8 * (float)local_234 * param_1[1] - local_150[0] * param_4) *
               (1.0 / fVar21);
      local_240 = (int *)ABS(fVar13);
      if (*(float *)((int)this + 0x34) < (float)local_240 * param_1[1]) {
        fVar17 = (*(float *)((int)this + 0x34) / (float)local_240) * *param_1;
        fVar13 = fVar17 * fVar13;
        *(float *)((int)this + 0x3c) = (1.0 - fVar17) * (float)local_234 + fVar15;
      }
    }
    if (*(char *)((int)this + 0x24) != '\0') {
      local_244 = local_244 - (*local_c0 * fVar13 + local_150[0]) * *param_1;
      fVar15 = *(float *)((int)this + 0x2c);
      if ((fVar15 < local_244) || (fVar15 = *(float *)((int)this + 0x28), local_244 < fVar15)) {
        fVar13 = (local_244 - fVar15) * *(float *)((int)this + 0x30) * param_1[1] * param_3 *
                 (1.0 / fVar21) + fVar13;
      }
    }
    if (fVar13 != 0.0) {
      local_244 = fVar13;
      FUN_100a53e0(*(void **)(iVar9 + 0x94),local_a0,(int)&local_244);
      FUN_100a53e0(*(void **)(local_23c + 0x94),local_94,(int)&local_244);
    }
  }
  local_234 = (int *)(param_2 + 0x380);
  *(undefined4 *)(param_2 + 0x3c) = 0;
  *(undefined4 *)(param_2 + 0x38) = 0;
  *(undefined4 *)(param_2 + 0x34) = 0;
  *(undefined4 *)(param_2 + 0x30) = 0;
  *(undefined4 *)(param_2 + 0x4c) = 0;
  *(undefined4 *)(param_2 + 0x48) = 0;
  *(undefined4 *)(param_2 + 0x44) = 0;
  *(undefined4 *)(param_2 + 0x40) = 0;
  iVar9 = *(int *)(iVar9 + 0x94);
  local_244 = (float)(*local_234 + -1);
  pfVar10 = *(float **)(param_2 + 900);
  pfVar7 = pfVar10 + 5;
  do {
    pfVar1 = pfVar7 + 1;
    pfVar2 = pfVar7 + -1;
    pfVar3 = pfVar7 + -3;
    pfVar4 = pfVar7 + 3;
    fVar21 = *pfVar7;
    pfVar5 = pfVar7 + -4;
    fVar13 = *pfVar10;
    pfVar10 = pfVar10 + 0xc;
    pfVar7 = pfVar7 + 0xc;
    local_244 = (float)((int)local_244 + -1);
    *(float *)(param_2 + 0x30 + (int)*pfVar4 * 4) =
         *pfVar1 * *(float *)(iVar9 + 200) + *pfVar2 * *(float *)(iVar9 + 0xc0) +
         *(float *)(iVar9 + 0xc4) * fVar21 +
         *pfVar5 * *(float *)(iVar9 + 0xd4) + *pfVar3 * *(float *)(iVar9 + 0xd8) +
         fVar13 * *(float *)(iVar9 + 0xd0) + *(float *)(param_2 + 0x30 + (int)*pfVar4 * 4);
  } while (-1 < (int)local_244);
  iVar9 = *(int *)(local_23c + 0x94);
  pfVar10 = *(float **)(param_2 + 0x390);
  local_240 = (int *)(param_2 + 0x38c);
  local_244 = (float)(*(int *)(param_2 + 0x38c) + -1);
  pfVar7 = pfVar10 + 5;
  do {
    pfVar1 = pfVar7 + -1;
    pfVar2 = pfVar7 + 1;
    pfVar3 = pfVar7 + -3;
    pfVar4 = pfVar7 + 3;
    fVar21 = *pfVar7;
    pfVar5 = pfVar7 + -4;
    fVar13 = *pfVar10;
    pfVar10 = pfVar10 + 0xc;
    pfVar7 = pfVar7 + 0xc;
    local_244 = (float)((int)local_244 + -1);
    *(float *)(param_2 + 0x30 + (int)*pfVar4 * 4) =
         *pfVar1 * *(float *)(iVar9 + 0xc0) + *pfVar2 * *(float *)(iVar9 + 200) +
         *(float *)(iVar9 + 0xc4) * fVar21 +
         *pfVar5 * *(float *)(iVar9 + 0xd4) + *pfVar3 * *(float *)(iVar9 + 0xd8) +
         fVar13 * *(float *)(iVar9 + 0xd0) + *(float *)(param_2 + 0x30 + (int)*pfVar4 * 4);
  } while (-1 < (int)local_244);
  fVar21 = *(float *)((int)this + 0x18) * param_4 * -1.0;
  local_180 = local_170;
  local_160 = *(float *)(param_2 + 0x20) * param_3 + fVar21 * *(float *)(param_2 + 0x40);
  local_17c[0] = 7.00649e-45;
  local_17c[1] = 1.12104e-44;
  iVar9 = 3;
  pfVar7 = (float *)(param_2 + 0x18);
  do {
    fVar13 = *pfVar7;
    *(float *)(((int)local_17c - param_2) + (int)pfVar7) =
         *(float *)(param_2 + 0x30 + iVar9 * 4) * fVar21 + pfVar7[1] * param_3;
    *(float *)(((int)&local_180 - param_2) + (int)pfVar7) =
         *(float *)(param_2 + 0x2c + iVar9 * 4) * fVar21 + fVar13 * param_3;
    iVar9 = iVar9 + -2;
    pfVar7 = pfVar7 + -2;
  } while (0 < iVar9);
  local_80 = local_70;
  local_7c = 5;
  local_78 = 8;
  FUN_100ba040((int *)(param_2 + 0x2d0),(int *)&local_180,(int *)&local_80);
  FUN_100a53e0(*(void **)(local_238 + 0x94),local_234,(int)local_80);
  FUN_100a53e0(*(void **)(local_23c + 0x94),local_240,(int)local_80);
  return;
}


// 10097ee0 FUN_10097ee0

undefined4 __thiscall
FUN_10097ee0(void *this,float *param_1,int *param_2,float param_3,float param_4)

{
  undefined4 *puVar1;
  float fVar2;
  int iVar3;
  int iVar4;
  int *piVar5;
  int *piVar6;
  void *pvVar7;
  int iVar8;
  float *pfVar9;
  int *piVar10;
  float *pfVar11;
  int *piVar12;
  float fVar13;
  float fVar14;
  int *piVar15;
  float fVar16;
  float fVar17;
  float fVar18;
  float fVar19;
  float fVar20;
  float fVar21;
  float fVar22;
  float fVar23;
  float fVar24;
  float fVar25;
  float fVar26;
  int *local_260;
  int *local_25c;
  int *local_258;
  void *local_254;
  float local_250;
  float local_24c;
  float local_248;
  float local_240;
  float local_23c;
  float local_238;
  undefined4 local_234;
  float local_230;
  float local_22c;
  float local_228;
  undefined4 local_224;
  float local_220;
  float local_21c;
  float local_218;
  undefined4 local_214;
  float local_210;
  float local_20c;
  float local_208;
  undefined4 local_204;
  float local_200;
  float local_1fc;
  float local_1f8;
  float local_1f4;
  float local_1f0;
  float local_1ec;
  float local_1e8;
  float local_1e0;
  float local_1dc;
  float local_1d8;
  float local_1d0;
  float local_1cc;
  float local_1c8;
  float local_1c0;
  float local_1bc;
  float local_1b8;
  undefined4 local_1b4;
  float local_1b0;
  float local_1ac;
  float local_1a8;
  undefined4 local_1a4;
  float local_1a0;
  float local_19c;
  float local_198;
  undefined4 local_194;
  float local_190;
  float local_18c;
  float local_188;
  undefined4 local_184;
  float local_180 [4];
  float local_170;
  float local_16c;
  float local_168;
  float local_160;
  float local_15c;
  float local_158;
  float local_150;
  float local_14c;
  float local_148;
  undefined1 *local_140;
  float local_13c [3];
  undefined1 local_130 [16];
  float local_120;
  float local_110 [12];
  float afStack_e0 [8];
  int aiStack_c0 [4];
  float afStack_b0 [8];
  int aiStack_90 [4];
  float *local_80;
  undefined4 local_7c;
  undefined4 local_78;
  undefined4 local_74;
  float local_70 [4];
  int local_60 [3];
  int local_54 [3];
  int local_48;
  int local_44;
  undefined1 *local_40;
  undefined4 local_3c;
  undefined4 local_38;
  undefined1 local_30 [44];
  
  if (param_2 == (int *)0x0) {
    local_258 = (int *)0x0;
  }
  else {
    *param_2 = (int)(param_2 + 4);
    param_2[1] = 5;
    param_2[2] = 8;
    param_2[0xb4] = (int)(param_2 + 0xb8);
    param_2[0xb5] = 5;
    param_2[0xb6] = 5;
    param_2[0xb7] = 8;
    local_258 = param_2;
  }
  iVar3 = *(int *)((int)this + 4);
  local_25c = *(int **)((int)this + 8);
  iVar4 = *(int *)(iVar3 + 0x94);
  local_254 = this;
  if ((*(uint *)(iVar3 + 0x44) & 0x800) == 0) {
    pfVar9 = (float *)(iVar4 + 0x60);
    pfVar11 = local_180;
    for (iVar8 = 0x10; iVar8 != 0; iVar8 = iVar8 + -1) {
      *pfVar11 = *pfVar9;
      pfVar9 = pfVar9 + 1;
      pfVar11 = pfVar11 + 1;
    }
    FUN_10039500(local_180,(float *)(iVar3 + 0x30),&local_150);
    local_230 = local_180[1];
    local_22c = local_16c;
    local_228 = local_15c;
    local_220 = local_180[2];
    local_21c = local_168;
    local_240 = local_180[0];
    local_218 = local_158;
    local_23c = local_170;
    local_210 = local_150;
    local_238 = local_160;
    local_20c = local_14c;
    local_208 = local_148;
  }
  else {
    local_240 = *(float *)(iVar4 + 0x60);
    local_23c = *(float *)(iVar4 + 0x70);
    local_238 = *(float *)(iVar4 + 0x80);
    local_230 = *(float *)(iVar4 + 100);
    local_22c = *(float *)(iVar4 + 0x74);
    local_228 = *(float *)(iVar4 + 0x84);
    local_220 = *(float *)(iVar4 + 0x68);
    local_21c = *(float *)(iVar4 + 0x78);
    local_218 = *(float *)(iVar4 + 0x88);
    local_210 = *(float *)(iVar4 + 0x90);
    local_20c = *(float *)(iVar4 + 0x94);
    local_208 = *(float *)(iVar4 + 0x98);
  }
  local_214 = 0;
  local_224 = 0;
  local_234 = 0;
  local_204 = 0x3f800000;
  iVar4 = *(int *)((int)local_25c + 0x94);
  if ((*(uint *)((int)local_25c + 0x44) & 0x800) == 0) {
    pfVar9 = (float *)(iVar4 + 0x60);
    pfVar11 = local_180;
    for (iVar8 = 0x10; iVar8 != 0; iVar8 = iVar8 + -1) {
      *pfVar11 = *pfVar9;
      pfVar9 = pfVar9 + 1;
      pfVar11 = pfVar11 + 1;
    }
    FUN_10039500(local_180,(float *)((int)local_25c + 0x30),&local_150);
    local_1c0 = local_180[0];
    local_1bc = local_170;
    local_1b8 = local_160;
    local_1b0 = local_180[1];
    local_1ac = local_16c;
    local_1a8 = local_15c;
    local_190 = local_150;
    local_1a0 = local_180[2];
    local_18c = local_14c;
    local_19c = local_168;
    local_188 = local_148;
    local_198 = local_158;
  }
  else {
    local_1c0 = *(float *)(iVar4 + 0x60);
    local_1bc = *(float *)(iVar4 + 0x70);
    local_1b8 = *(float *)(iVar4 + 0x80);
    local_1b0 = *(float *)(iVar4 + 100);
    local_1ac = *(float *)(iVar4 + 0x74);
    local_1a8 = *(float *)(iVar4 + 0x84);
    local_1a0 = *(float *)(iVar4 + 0x68);
    local_19c = *(float *)(iVar4 + 0x78);
    local_198 = *(float *)(iVar4 + 0x88);
    local_190 = *(float *)(iVar4 + 0x90);
    local_18c = *(float *)(iVar4 + 0x94);
    local_188 = *(float *)(iVar4 + 0x98);
  }
  pvVar7 = local_254;
  piVar12 = local_258;
  local_184 = 0x3f800000;
  local_194 = 0;
  local_1a4 = 0;
  local_1b4 = 0;
  FUN_100bae30(local_180,&local_1c0,&local_240);
  FUN_100952f0(&local_200,local_180);
  fVar25 = *(float *)((int)pvVar7 + 100);
  fVar20 = *(float *)((int)pvVar7 + 0x68);
  fVar14 = *(float *)((int)pvVar7 + 0x60);
  fVar22 = *(float *)((int)pvVar7 + 0x6c);
  fVar24 = *(float *)((int)pvVar7 + 0x68);
  fVar13 = param_3 * param_1[1] * *(float *)((int)pvVar7 + 0x1c) * 2.0;
  fVar2 = *(float *)((int)pvVar7 + 100);
  if (0.0 < local_1f4) {
    fVar13 = -0.0 - fVar13;
  }
  *(float *)*piVar12 =
       (local_1f4 * *(float *)((int)pvVar7 + 0x60) +
       local_200 * fVar22 + (fVar25 * local_1f8 - fVar20 * local_1fc)) * fVar13;
  *(float *)(*piVar12 + 4) =
       (local_1f4 * fVar2 + fVar22 * local_1fc + (local_200 * fVar20 - fVar14 * local_1f8)) * fVar13
  ;
  *(float *)(*piVar12 + 8) =
       (fVar24 * local_1f4 + fVar22 * local_1f8 + (fVar14 * local_1fc - local_200 * fVar25)) *
       fVar13;
  FUN_100b9e30(&local_1e0,&local_240,(float *)((int)pvVar7 + 0x40));
  FUN_100b9e30(&local_1f0,&local_240,(float *)((int)pvVar7 + 0x50));
  local_1d0 = local_1dc * local_1e8 - local_1d8 * local_1ec;
  local_1cc = local_1f0 * local_1d8 - local_1e0 * local_1e8;
  local_1c8 = local_1e0 * local_1ec - local_1f0 * local_1dc;
  FUN_100b9e30(&local_200,&local_240,(float *)((int)pvVar7 + 0x70));
  local_200 = (local_190 - local_210) - local_200;
  local_1f8 = (local_188 - local_208) - local_1f8;
  local_1fc = (local_18c - local_20c) - local_1fc;
  *(float *)(*piVar12 + 0xc) =
       (local_1f0 * local_200 + local_1e8 * local_1f8 + local_1ec * local_1fc) * param_3 *
       param_1[1] * *(float *)((int)pvVar7 + 0x1c);
  *(float *)(*piVar12 + 0x10) =
       (local_200 * local_1d0 + local_1f8 * local_1c8 + local_1fc * local_1cc) * param_3 *
       param_1[1] * *(float *)((int)pvVar7 + 0x1c);
  fVar25 = *(float *)((int)pvVar7 + 0x20);
  fVar20 = 1.0 - fVar25;
  local_250 = local_210 * fVar20 + local_190 * fVar25;
  local_24c = local_20c * fVar20 + local_18c * fVar25;
  local_248 = local_208 * fVar20 + fVar25 * local_188;
  piVar12 = local_25c;
  if (*(char *)((int)pvVar7 + 0x25) != '\0' || *(char *)((int)pvVar7 + 0x24) != '\0') {
    local_80 = local_70;
    local_260 = (int *)(local_1e0 * local_200 + local_1d8 * local_1f8 + local_1dc * local_1fc);
    local_7c = 1;
    local_78 = 1;
    local_74 = 4;
    FUN_10096f70(local_110);
    iVar8 = local_48;
    piVar12 = local_25c;
    iVar4 = *(int *)(iVar3 + 0x94);
    fVar22 = local_24c - *(float *)(iVar4 + 0x94);
    fVar24 = local_248 - *(float *)(iVar4 + 0x98);
    fVar14 = local_250 - *(float *)(iVar4 + 0x90);
    *(float *)((int)afStack_e0 + local_48 + 4) = local_1dc;
    *(float *)((int)afStack_e0 + iVar8 + 8) = local_1d8;
    *(float *)((int)afStack_e0 + iVar8) = local_1e0;
    fVar25 = *(float *)((int)afStack_e0 + iVar8 + 4);
    fVar20 = *(float *)((int)afStack_e0 + iVar8 + 8);
    fVar17 = fVar14 * fVar25 - local_1e0 * fVar22;
    *(float *)((int)afStack_e0 + iVar8 + 0x18) = fVar17;
    fVar18 = local_1e0 * fVar24 - fVar14 * fVar20;
    *(float *)((int)afStack_e0 + iVar8 + 0x14) = fVar18;
    *(float *)((int)afStack_e0 + iVar8 + 0x10) = fVar20 * fVar22 - fVar25 * fVar24;
    fVar25 = *(float *)((int)afStack_e0 + iVar8 + 0x10);
    fVar20 = *(float *)(iVar4 + 0x74);
    fVar14 = *(float *)(iVar4 + 0x78);
    fVar22 = *(float *)(iVar4 + 100);
    fVar24 = *(float *)(iVar4 + 0x84);
    fVar2 = *(float *)(iVar4 + 0x68);
    fVar13 = *(float *)(iVar4 + 0x88);
    *(float *)((int)afStack_e0 + iVar8 + 0x10) =
         *(float *)(iVar4 + 0x70) * fVar18 + *(float *)(iVar4 + 0x80) * fVar17 +
         fVar25 * *(float *)(iVar4 + 0x60);
    *(float *)((int)afStack_e0 + iVar8 + 0x14) = fVar22 * fVar25 + fVar20 * fVar18 + fVar24 * fVar17
    ;
    *(float *)((int)afStack_e0 + iVar8 + 0x18) = fVar2 * fVar25 + fVar14 * fVar18 + fVar13 * fVar17;
    *(int *)((int)aiStack_c0 + iVar8) = local_44;
    iVar8 = local_48;
    iVar4 = *(int *)((int)local_25c + 0x94);
    fVar25 = *(float *)(iVar4 + 0x98);
    fVar22 = local_250 - *(float *)(iVar4 + 0x90);
    pfVar9 = (float *)((int)afStack_b0 + local_48);
    fVar24 = local_24c - *(float *)(iVar4 + 0x94);
    *(float *)((int)afStack_b0 + local_48 + 4) = local_1dc * -1.0;
    *(float *)((int)afStack_b0 + iVar8 + 8) = local_1d8 * -1.0;
    fVar25 = local_248 - fVar25;
    local_1e0 = local_1e0 * -1.0;
    *pfVar9 = local_1e0;
    fVar20 = *(float *)((int)afStack_b0 + iVar8 + 4);
    fVar14 = *(float *)((int)afStack_b0 + iVar8 + 8);
    *(float *)((int)afStack_b0 + iVar8 + 0x14) = local_1e0 * fVar25 - fVar22 * fVar14;
    *(float *)((int)afStack_b0 + iVar8 + 0x10) = fVar14 * fVar24 - fVar20 * fVar25;
    fVar18 = fVar22 * fVar20 - local_1e0 * fVar24;
    *(float *)((int)afStack_b0 + iVar8 + 0x18) = fVar18;
    fVar25 = *(float *)((int)afStack_b0 + iVar8 + 0x14);
    fVar20 = *(float *)((int)afStack_b0 + iVar8 + 0x10);
    fVar14 = *(float *)(iVar4 + 0x78);
    fVar22 = *(float *)(iVar4 + 0x68);
    fVar24 = *(float *)(iVar4 + 0x88);
    fVar2 = *(float *)(iVar4 + 0x70);
    fVar13 = *(float *)(iVar4 + 0x80);
    fVar17 = *(float *)(iVar4 + 0x60);
    *(float *)((int)afStack_b0 + iVar8 + 0x14) =
         *(float *)(iVar4 + 100) * fVar20 + *(float *)(iVar4 + 0x74) * fVar25 +
         *(float *)(iVar4 + 0x84) * fVar18;
    *(float *)((int)afStack_b0 + iVar8 + 0x18) = fVar22 * fVar20 + fVar14 * fVar25 + fVar24 * fVar18
    ;
    *(float *)((int)afStack_b0 + iVar8 + 0x10) = fVar2 * fVar25 + fVar13 * fVar18 + fVar17 * fVar20;
    *(int *)((int)aiStack_90 + iVar8) = local_44;
    local_60[0] = local_44 + 1;
    local_48 = local_48 + 0x30;
    local_54[0] = local_60[0];
    local_44 = local_60[0];
    FUN_100a4e60(*(void **)(iVar3 + 0x94),local_60,(int *)&local_80,(int)local_110);
    local_60[0] = local_44;
    local_54[0] = local_44;
    FUN_100a4e60(*(void **)((int)piVar12 + 0x94),local_54,(int *)&local_80,(int)local_110);
    piVar15 = (int *)0x0;
    fVar25 = *local_80;
    if (*(char *)((int)pvVar7 + 0x25) != '\0') {
      fVar20 = *(float *)((int)pvVar7 + 0x38) * *param_1 + *(float *)((int)pvVar7 + 0x3c);
      fVar14 = (float)local_260 - *(float *)((int)pvVar7 + 0x3c);
      *(float *)((int)pvVar7 + 0x3c) = fVar20;
      piVar15 = (int *)((param_3 * 0.8 * fVar14 * param_1[1] - local_110[0] * param_4) *
                       (1.0 / fVar25));
      local_25c = (int *)ABS((float)piVar15);
      if (*(float *)((int)pvVar7 + 0x34) < (float)local_25c * param_1[1]) {
        fVar22 = (*(float *)((int)pvVar7 + 0x34) / (float)local_25c) * *param_1;
        piVar15 = (int *)(fVar22 * (float)piVar15);
        *(float *)((int)pvVar7 + 0x3c) = (1.0 - fVar22) * fVar14 + fVar20;
      }
    }
    if (*(char *)((int)pvVar7 + 0x24) != '\0') {
      fVar14 = (float)local_260 - ((float)piVar15 * *local_80 + local_110[0]) * *param_1;
      fVar20 = *(float *)((int)pvVar7 + 0x2c);
      if ((fVar20 < fVar14) || (fVar20 = *(float *)((int)pvVar7 + 0x28), fVar14 < fVar20)) {
        piVar15 = (int *)((fVar14 - fVar20) * *(float *)((int)pvVar7 + 0x30) * param_3 * param_1[1]
                          * (1.0 / fVar25) + (float)piVar15);
      }
    }
    if ((float)piVar15 != 0.0) {
      local_260 = piVar15;
      FUN_100a53e0(*(void **)(iVar3 + 0x94),local_60,(int)&local_260);
      FUN_100a53e0(*(void **)((int)piVar12 + 0x94),local_54,(int)&local_260);
    }
  }
  piVar6 = local_258;
  piVar10 = local_258 + 0xc;
  FUN_10097030(piVar10);
  iVar4 = piVar6[0xe6];
  *(undefined4 *)((int)piVar6 + iVar4 + 0xf8) = 0;
  *(undefined4 *)((int)piVar6 + iVar4 + 0xf4) = 0;
  *(undefined4 *)((int)piVar6 + iVar4 + 0xf0) = 0;
  *(float *)((int)piVar6 + iVar4 + 0x100) = local_240;
  *(float *)((int)piVar6 + iVar4 + 0x104) = local_23c;
  *(float *)((int)piVar6 + iVar4 + 0x108) = local_238;
  iVar8 = *(int *)(iVar3 + 0x94);
  fVar25 = *(float *)((int)piVar6 + iVar4 + 0x104);
  fVar20 = *(float *)((int)piVar6 + iVar4 + 0x100);
  fVar14 = *(float *)(iVar8 + 0x74);
  fVar22 = *(float *)(iVar8 + 100);
  fVar24 = *(float *)(iVar8 + 0x84);
  fVar2 = *(float *)(iVar8 + 0x70);
  fVar13 = *(float *)(iVar8 + 0x80);
  fVar17 = *(float *)(iVar8 + 0x60);
  *(float *)((int)piVar6 + iVar4 + 0x108) =
       *(float *)(iVar8 + 0x68) * fVar20 + *(float *)(iVar8 + 0x78) * fVar25 +
       *(float *)(iVar8 + 0x88) * local_238;
  *(float *)((int)piVar6 + iVar4 + 0x104) = fVar22 * fVar20 + fVar14 * fVar25 + fVar24 * local_238;
  *(float *)((int)piVar6 + iVar4 + 0x100) = fVar2 * fVar25 + fVar13 * local_238 + fVar17 * fVar20;
  *(int *)((int)piVar6 + iVar4 + 0x110) = piVar6[0xe7];
  iVar4 = piVar6[0xe6];
  *(undefined4 *)((int)piVar6 + iVar4 + 0x1e8) = 0;
  *(undefined4 *)((int)piVar6 + iVar4 + 0x1e4) = 0;
  *(undefined4 *)((int)piVar6 + iVar4 + 0x1e0) = 0;
  *(float *)((int)piVar6 + iVar4 + 0x1f0) = local_240 * -1.0;
  *(float *)((int)piVar6 + iVar4 + 500) = local_23c * -1.0;
  fVar18 = local_238 * -1.0;
  *(float *)((int)piVar6 + iVar4 + 0x1f8) = fVar18;
  iVar8 = *(int *)((int)piVar12 + 0x94);
  fVar25 = *(float *)((int)piVar6 + iVar4 + 500);
  fVar20 = *(float *)(iVar8 + 100);
  fVar14 = *(float *)(iVar8 + 0x74);
  fVar22 = *(float *)((int)piVar6 + iVar4 + 0x1f0);
  fVar24 = *(float *)(iVar8 + 0x84);
  fVar2 = *(float *)(iVar8 + 0x60);
  fVar13 = *(float *)(iVar8 + 0x70);
  fVar17 = *(float *)(iVar8 + 0x80);
  *(float *)((int)piVar6 + iVar4 + 0x1f8) =
       *(float *)(iVar8 + 0x68) * fVar22 + *(float *)(iVar8 + 0x78) * fVar25 +
       *(float *)(iVar8 + 0x88) * fVar18;
  *(float *)((int)piVar6 + iVar4 + 500) = fVar20 * fVar22 + fVar14 * fVar25 + fVar24 * fVar18;
  *(float *)((int)piVar6 + iVar4 + 0x1f0) = fVar13 * fVar25 + fVar17 * fVar18 + fVar22 * fVar2;
  *(int *)((int)piVar6 + iVar4 + 0x200) = piVar6[0xe7];
  iVar4 = piVar6[0xe6];
  *(undefined4 *)((int)piVar6 + iVar4 + 0x128) = 0;
  *(undefined4 *)((int)piVar6 + iVar4 + 0x124) = 0;
  *(undefined4 *)((int)piVar6 + iVar4 + 0x120) = 0;
  *(float *)((int)piVar6 + iVar4 + 0x130) = local_230;
  *(float *)((int)piVar6 + iVar4 + 0x134) = local_22c;
  *(float *)((int)piVar6 + iVar4 + 0x138) = local_228;
  iVar8 = *(int *)(iVar3 + 0x94);
  fVar25 = *(float *)((int)piVar6 + iVar4 + 0x134);
  fVar20 = *(float *)(iVar8 + 100);
  fVar14 = *(float *)(iVar8 + 0x74);
  fVar22 = *(float *)((int)piVar6 + iVar4 + 0x130);
  fVar24 = *(float *)(iVar8 + 0x84);
  fVar2 = *(float *)(iVar8 + 0x60);
  fVar13 = *(float *)(iVar8 + 0x70);
  fVar17 = *(float *)(iVar8 + 0x80);
  *(float *)((int)piVar6 + iVar4 + 0x138) =
       *(float *)(iVar8 + 0x68) * fVar22 + *(float *)(iVar8 + 0x78) * fVar25 +
       *(float *)(iVar8 + 0x88) * local_228;
  *(float *)((int)piVar6 + iVar4 + 0x134) = fVar20 * fVar22 + fVar14 * fVar25 + fVar24 * local_228;
  *(float *)((int)piVar6 + iVar4 + 0x130) = fVar13 * fVar25 + fVar17 * local_228 + fVar22 * fVar2;
  *(int *)((int)piVar6 + iVar4 + 0x140) = piVar6[0xe7] + 1;
  puVar1 = (undefined4 *)((int)piVar6 + piVar6[0xe6] + 0x210);
  puVar1[2] = 0;
  puVar1[1] = 0;
  *puVar1 = 0;
  puVar1[4] = local_230 * -1.0;
  puVar1[5] = local_22c * -1.0;
  fVar18 = local_228 * -1.0;
  puVar1[6] = fVar18;
  iVar4 = *(int *)((int)piVar12 + 0x94);
  fVar25 = (float)puVar1[5];
  fVar20 = *(float *)(iVar4 + 100);
  fVar14 = *(float *)(iVar4 + 0x74);
  fVar22 = (float)puVar1[4];
  fVar24 = *(float *)(iVar4 + 0x84);
  fVar2 = *(float *)(iVar4 + 0x70);
  fVar13 = *(float *)(iVar4 + 0x80);
  fVar17 = *(float *)(iVar4 + 0x60);
  puVar1[6] = *(float *)(iVar4 + 0x68) * fVar22 + *(float *)(iVar4 + 0x78) * fVar25 +
              *(float *)(iVar4 + 0x88) * fVar18;
  puVar1[5] = fVar20 * fVar22 + fVar14 * fVar25 + fVar24 * fVar18;
  puVar1[4] = fVar2 * fVar25 + fVar13 * fVar18 + fVar17 * fVar22;
  puVar1[8] = piVar6[0xe7] + 1;
  iVar4 = piVar6[0xe6];
  *(undefined4 *)((int)piVar6 + iVar4 + 0x158) = 0;
  *(undefined4 *)((int)piVar6 + iVar4 + 0x154) = 0;
  *(undefined4 *)((int)piVar6 + iVar4 + 0x150) = 0;
  *(float *)((int)piVar6 + iVar4 + 0x160) = local_220;
  *(float *)((int)piVar6 + iVar4 + 0x164) = local_21c;
  *(float *)((int)piVar6 + iVar4 + 0x168) = local_218;
  iVar8 = *(int *)(iVar3 + 0x94);
  fVar25 = *(float *)((int)piVar6 + iVar4 + 0x164);
  fVar20 = *(float *)((int)piVar6 + iVar4 + 0x160);
  fVar14 = *(float *)(iVar8 + 0x78);
  fVar22 = *(float *)(iVar8 + 0x68);
  fVar24 = *(float *)(iVar8 + 0x88);
  fVar2 = *(float *)(iVar8 + 0x70);
  fVar13 = *(float *)(iVar8 + 0x80);
  fVar17 = *(float *)(iVar8 + 0x60);
  *(float *)((int)piVar6 + iVar4 + 0x164) =
       *(float *)(iVar8 + 100) * fVar20 + *(float *)(iVar8 + 0x74) * fVar25 +
       *(float *)(iVar8 + 0x84) * local_218;
  *(float *)((int)piVar6 + iVar4 + 0x168) = fVar22 * fVar20 + fVar14 * fVar25 + fVar24 * local_218;
  *(float *)((int)piVar6 + iVar4 + 0x160) = fVar2 * fVar25 + fVar13 * local_218 + fVar17 * fVar20;
  *(int *)((int)piVar6 + iVar4 + 0x170) = piVar6[0xe7] + 2;
  puVar1 = (undefined4 *)((int)piVar6 + piVar6[0xe6] + 0x240);
  puVar1[2] = 0;
  puVar1[1] = 0;
  *puVar1 = 0;
  puVar1[4] = local_220 * -1.0;
  puVar1[5] = local_21c * -1.0;
  fVar18 = local_218 * -1.0;
  puVar1[6] = fVar18;
  fVar25 = (float)puVar1[5];
  fVar20 = (float)puVar1[4];
  iVar4 = *(int *)((int)piVar12 + 0x94);
  fVar14 = *(float *)(iVar4 + 0x78);
  fVar22 = *(float *)(iVar4 + 0x68);
  fVar24 = *(float *)(iVar4 + 0x88);
  fVar2 = *(float *)(iVar4 + 0x70);
  fVar13 = *(float *)(iVar4 + 0x80);
  fVar17 = *(float *)(iVar4 + 0x60);
  puVar1[5] = *(float *)(iVar4 + 100) * fVar20 + *(float *)(iVar4 + 0x74) * fVar25 +
              *(float *)(iVar4 + 0x84) * fVar18;
  puVar1[6] = fVar22 * fVar20 + fVar14 * fVar25 + fVar24 * fVar18;
  puVar1[4] = fVar2 * fVar25 + fVar13 * fVar18 + fVar17 * fVar20;
  puVar1[8] = piVar6[0xe7] + 2;
  iVar4 = *(int *)(iVar3 + 0x94);
  iVar8 = *(int *)((int)piVar12 + 0x94);
  fVar16 = local_250 - *(float *)(iVar4 + 0x90);
  fVar19 = local_24c - *(float *)(iVar4 + 0x94);
  fVar21 = local_248 - *(float *)(iVar4 + 0x98);
  local_200 = *(float *)(iVar8 + 0x90);
  local_1fc = *(float *)(iVar8 + 0x94);
  local_250 = local_250 - local_200;
  local_1f8 = *(float *)(iVar8 + 0x98);
  local_24c = local_24c - local_1fc;
  pfVar9 = (float *)((int)piVar6 + piVar6[0xe6] + 0x180);
  *pfVar9 = local_1f0;
  pfVar9[1] = local_1ec;
  local_248 = local_248 - local_1f8;
  pfVar9[2] = local_1e8;
  fVar23 = fVar16 * pfVar9[1] - *pfVar9 * fVar19;
  pfVar9[6] = fVar23;
  pfVar9[5] = *pfVar9 * fVar21 - fVar16 * pfVar9[2];
  pfVar9[4] = pfVar9[2] * fVar19 - pfVar9[1] * fVar21;
  iVar4 = *(int *)(iVar3 + 0x94);
  fVar25 = pfVar9[5];
  fVar20 = *(float *)(iVar4 + 0x68);
  fVar14 = *(float *)(iVar4 + 0x78);
  fVar22 = pfVar9[6];
  fVar24 = *(float *)(iVar4 + 0x88);
  fVar2 = *(float *)(iVar4 + 0x70);
  fVar13 = pfVar9[5];
  fVar17 = *(float *)(iVar4 + 0x80);
  fVar18 = *(float *)(iVar4 + 0x60);
  pfVar9[5] = *(float *)(iVar4 + 100) * pfVar9[4] + *(float *)(iVar4 + 0x74) * fVar25 +
              *(float *)(iVar4 + 0x84) * fVar23;
  pfVar9[6] = fVar20 * pfVar9[4] + fVar14 * fVar25 + fVar24 * fVar22;
  pfVar9[4] = fVar2 * fVar13 + fVar17 * fVar22 + fVar18 * pfVar9[4];
  pfVar9[8] = (float)(piVar6[0xe7] + 3);
  iVar4 = piVar6[0xe6];
  *(float *)((int)piVar6 + iVar4 + 0x270) = local_1f0 * -1.0;
  pfVar9 = (float *)((int)piVar6 + iVar4 + 0x270);
  pfVar9[1] = local_1ec * -1.0;
  pfVar9[2] = local_1e8 * -1.0;
  pfVar9[6] = local_250 * pfVar9[1] - *pfVar9 * local_24c;
  fVar26 = *pfVar9 * local_248 - local_250 * pfVar9[2];
  pfVar9[5] = fVar26;
  pfVar9[4] = pfVar9[2] * local_24c - pfVar9[1] * local_248;
  iVar4 = *(int *)((int)piVar12 + 0x94);
  fVar25 = pfVar9[4];
  fVar20 = pfVar9[5];
  fVar14 = *(float *)(iVar4 + 0x78);
  fVar22 = pfVar9[6];
  fVar24 = *(float *)(iVar4 + 0x68);
  fVar2 = *(float *)(iVar4 + 0x60);
  fVar13 = *(float *)(iVar4 + 0x88);
  fVar17 = *(float *)(iVar4 + 0x70);
  fVar18 = pfVar9[5];
  fVar23 = *(float *)(iVar4 + 0x80);
  pfVar9[5] = *(float *)(iVar4 + 100) * fVar25 + *(float *)(iVar4 + 0x74) * fVar26 +
              *(float *)(iVar4 + 0x84) * pfVar9[6];
  pfVar9[6] = fVar24 * fVar25 + fVar14 * fVar20 + fVar13 * fVar22;
  pfVar9[4] = fVar17 * fVar18 + fVar23 * fVar22 + fVar25 * fVar2;
  pfVar9[8] = (float)(piVar6[0xe7] + 3);
  pfVar9 = (float *)((int)piVar6 + piVar6[0xe6] + 0x1b0);
  *pfVar9 = local_1d0;
  pfVar9[1] = local_1cc;
  pfVar9[2] = local_1c8;
  fVar18 = pfVar9[2] * fVar19 - pfVar9[1] * fVar21;
  fVar17 = *pfVar9 * fVar21 - pfVar9[2] * fVar16;
  pfVar9[4] = fVar18;
  fVar13 = pfVar9[1] * fVar16 - *pfVar9 * fVar19;
  pfVar9[6] = fVar13;
  pfVar9[5] = fVar17;
  iVar4 = *(int *)(iVar3 + 0x94);
  fVar25 = *(float *)(iVar4 + 0x78);
  fVar20 = *(float *)(iVar4 + 0x68);
  fVar14 = *(float *)(iVar4 + 0x88);
  fVar22 = *(float *)(iVar4 + 0x70);
  fVar24 = *(float *)(iVar4 + 0x80);
  fVar2 = *(float *)(iVar4 + 0x60);
  pfVar9[5] = *(float *)(iVar4 + 100) * fVar18 + *(float *)(iVar4 + 0x74) * fVar17 +
              *(float *)(iVar4 + 0x84) * fVar13;
  pfVar9[6] = fVar20 * fVar18 + fVar25 * fVar17 + fVar14 * fVar13;
  pfVar9[4] = fVar22 * fVar17 + fVar24 * fVar13 + fVar2 * fVar18;
  pfVar9[8] = (float)(piVar6[0xe7] + 4);
  iVar4 = piVar6[0xe6];
  *(float *)((int)piVar6 + iVar4 + 0x2a0) = local_1d0 * -1.0;
  pfVar9 = (float *)((int)piVar6 + iVar4 + 0x2a0);
  pfVar9[1] = local_1cc * -1.0;
  pfVar9[2] = local_1c8 * -1.0;
  fVar18 = pfVar9[2] * local_24c - pfVar9[1] * local_248;
  pfVar9[4] = fVar18;
  fVar13 = pfVar9[1] * local_250 - *pfVar9 * local_24c;
  pfVar9[6] = fVar13;
  fVar17 = *pfVar9 * local_248 - pfVar9[2] * local_250;
  pfVar9[5] = fVar17;
  iVar4 = *(int *)((int)piVar12 + 0x94);
  fVar25 = *(float *)(iVar4 + 0x78);
  fVar20 = *(float *)(iVar4 + 0x68);
  fVar14 = *(float *)(iVar4 + 0x60);
  fVar22 = *(float *)(iVar4 + 0x88);
  fVar24 = *(float *)(iVar4 + 0x70);
  fVar2 = *(float *)(iVar4 + 0x80);
  pfVar9[5] = *(float *)(iVar4 + 100) * fVar18 + *(float *)(iVar4 + 0x74) * fVar17 +
              *(float *)(iVar4 + 0x84) * fVar13;
  pfVar9[6] = fVar20 * fVar18 + fVar25 * fVar17 + fVar22 * fVar13;
  pfVar9[4] = fVar24 * fVar17 + fVar2 * fVar13 + fVar18 * fVar14;
  pfVar9[8] = (float)(piVar6[0xe7] + 4);
  piVar6[0xe7] = piVar6[0xe7] + 5;
  piVar6[0xe6] = piVar6[0xe6] + 0xf0;
  local_25c = piVar6 + 0xe0;
  piVar15 = piVar6 + 0xe3;
  *local_25c = piVar6[0xe7];
  *piVar15 = piVar6[0xe7];
  local_260 = piVar6 + 0xb4;
  FUN_100a4e60(*(void **)(iVar3 + 0x94),local_25c,local_260,(int)piVar10);
  iVar4 = piVar6[0xe7];
  *local_25c = iVar4;
  *piVar15 = iVar4;
  FUN_100a4e60(*(void **)((int)piVar12 + 0x94),piVar15,local_260,(int)piVar10);
  piVar5 = local_260;
  fVar25 = *(float *)((int)local_254 + 0x18) * param_4 * -1.0;
  local_140 = local_130;
  local_120 = (float)piVar6[0x10] * fVar25 + (float)local_258[8];
  local_254 = (void *)0x3;
  pfVar9 = (float *)(local_258 + 6);
  local_13c[0] = 7.00649e-45;
  local_13c[1] = 1.12104e-44;
  do {
    *(float *)(((int)local_13c - (int)local_258) + (int)pfVar9) =
         (float)piVar10[(int)local_254] * fVar25 + pfVar9[1];
    iVar4 = (int)local_254 + 0xb;
    local_254 = (void *)((int)local_254 + -2);
    *(float *)(((int)&local_140 - (int)local_258) + (int)pfVar9) =
         (float)piVar6[iVar4] * fVar25 + *pfVar9;
    pfVar9 = pfVar9 + -2;
  } while (0 < (int)local_254);
  local_258 = (int *)((int)local_13c - (int)local_258);
  FUN_100ba5f0((int)local_260);
  local_40 = local_30;
  local_3c = 5;
  local_38 = 8;
  FUN_100ba040(piVar5,(int *)&local_140,(int *)&local_40);
  FUN_100a53e0(*(void **)(iVar3 + 0x94),local_25c,(int)local_40);
  FUN_100a53e0(*(void **)((int)piVar12 + 0x94),piVar15,(int)local_40);
  return 0x3a0;
}


// 10099890 FUN_10099890

void __thiscall FUN_10099890(void *this,undefined4 *param_1)

{
  *param_1 = *(undefined4 *)((int)this + 0x78);
  param_1[1] = *(undefined4 *)((int)this + 0x7c);
  param_1[8] = *(undefined4 *)((int)this + 0x18);
  param_1[9] = *(undefined4 *)((int)this + 0x1c);
  param_1[10] = *(undefined4 *)((int)this + 0x20);
  param_1[0xc] = *(undefined4 *)((int)this + 0x28);
  param_1[0xd] = *(undefined4 *)((int)this + 0x2c);
  param_1[0xe] = *(undefined4 *)((int)this + 0x30);
  param_1[0x18] = *(undefined4 *)((int)this + 0x58);
  param_1[0x19] = *(undefined4 *)((int)this + 0x5c);
  param_1[0x1a] = *(undefined4 *)((int)this + 0x60);
  param_1[0x10] = *(undefined4 *)((int)this + 0x38);
  param_1[0x11] = *(undefined4 *)((int)this + 0x3c);
  param_1[0x12] = *(undefined4 *)((int)this + 0x40);
  param_1[0x14] = *(undefined4 *)((int)this + 0x48);
  param_1[0x15] = *(undefined4 *)((int)this + 0x4c);
  param_1[0x16] = *(undefined4 *)((int)this + 0x50);
  param_1[0x1c] = *(undefined4 *)((int)this + 0x68);
  param_1[0x1d] = *(undefined4 *)((int)this + 0x6c);
  param_1[0x1e] = *(undefined4 *)((int)this + 0x70);
  param_1[2] = *(undefined4 *)((int)this + 0x80);
  param_1[3] = *(undefined4 *)((int)this + 0x84);
  param_1[4] = *(undefined4 *)((int)this + 0x88);
  param_1[5] = *(undefined4 *)((int)this + 0x8c);
  param_1[6] = *(undefined4 *)((int)this + 0x90);
  param_1[7] = *(undefined4 *)((int)this + 0x94);
  return;
}


// 10099950 FUN_10099950

void __thiscall FUN_10099950(void *this,undefined4 param_1,float param_2)

{
  *(bool *)((int)this + 0x81) = param_2 != 0.0;
  *(float *)((int)this + 0x90) = ABS(param_2);
  *(undefined4 *)((int)this + 0x94) = param_1;
  return;
}


// 10099990 FUN_10099990

void __thiscall FUN_10099990(void *this,undefined4 *param_1)

{
  float fVar1;
  float fVar2;
  float fVar3;
  int iVar4;
  float *pfVar5;
  int iVar6;
  float *pfVar7;
  int iVar8;
  float10 fVar9;
  float fVar10;
  float local_130;
  undefined4 local_12c;
  undefined4 local_128;
  undefined4 local_124;
  undefined4 local_120;
  undefined4 local_11c;
  undefined4 local_118;
  undefined4 local_114;
  undefined4 local_110;
  undefined4 local_10c;
  undefined4 local_108;
  undefined4 local_104;
  float local_100;
  undefined4 local_fc;
  undefined4 local_f8;
  undefined4 local_f4;
  float local_f0;
  undefined4 local_ec;
  undefined4 local_e8;
  undefined4 local_e4;
  undefined4 local_e0;
  undefined4 local_dc;
  undefined4 local_d8;
  undefined4 local_d4;
  undefined4 local_d0;
  undefined4 local_cc;
  undefined4 local_c8;
  undefined4 local_c4;
  float local_c0;
  undefined4 local_bc;
  undefined4 local_b8;
  undefined4 local_b4;
  float local_b0;
  float local_ac;
  float local_a8;
  float local_a0 [4];
  undefined4 local_90;
  undefined4 local_8c;
  undefined4 local_88;
  undefined4 local_80;
  undefined4 local_7c;
  undefined4 local_78;
  float local_70;
  undefined4 local_6c;
  undefined4 local_68;
  float local_60;
  float local_5c;
  float local_58;
  float local_50;
  float local_4c;
  float local_48;
  float local_40;
  float local_3c;
  float local_38;
  undefined1 local_30 [16];
  float local_20 [7];
  
  *(undefined4 *)((int)this + 0x78) = *param_1;
  *(undefined4 *)((int)this + 0x7c) = param_1[1];
  *(undefined4 *)((int)this + 0x18) = param_1[8];
  *(undefined4 *)((int)this + 0x1c) = param_1[9];
  *(undefined4 *)((int)this + 0x20) = param_1[10];
  *(undefined4 *)((int)this + 0x28) = param_1[0xc];
  *(undefined4 *)((int)this + 0x2c) = param_1[0xd];
  *(undefined4 *)((int)this + 0x30) = param_1[0xe];
  *(undefined4 *)((int)this + 0x38) = param_1[0x10];
  *(undefined4 *)((int)this + 0x3c) = param_1[0x11];
  *(undefined4 *)((int)this + 0x40) = param_1[0x12];
  *(undefined4 *)((int)this + 0x48) = param_1[0x14];
  *(undefined4 *)((int)this + 0x4c) = param_1[0x15];
  *(undefined4 *)((int)this + 0x50) = param_1[0x16];
  *(undefined4 *)((int)this + 0x80) = param_1[2];
  *(undefined4 *)((int)this + 0x84) = param_1[3];
  *(undefined4 *)((int)this + 0x88) = param_1[4];
  *(undefined4 *)((int)this + 0x8c) = param_1[5];
  *(undefined4 *)((int)this + 0x90) = param_1[6];
  *(undefined4 *)((int)this + 0x94) = param_1[7];
  iVar8 = *(int *)((int)this + 4);
  iVar4 = *(int *)((int)this + 8);
  if ((*(uint *)(iVar8 + 0x44) & 0x800) == 0) {
    pfVar5 = (float *)(*(int *)(iVar8 + 0x94) + 0x60);
    pfVar7 = local_a0;
    for (iVar6 = 0x10; iVar6 != 0; iVar6 = iVar6 + -1) {
      *pfVar7 = *pfVar5;
      pfVar5 = pfVar5 + 1;
      pfVar7 = pfVar7 + 1;
    }
    FUN_10039500(local_a0,(float *)(iVar8 + 0x30),&local_70);
    local_f0 = local_a0[0];
    local_ec = local_90;
    local_e8 = local_80;
    local_e0 = local_a0[1];
    local_dc = local_8c;
    local_d8 = local_7c;
    local_d0 = local_a0[2];
    local_cc = local_88;
    local_c8 = local_78;
    local_c0 = local_70;
    local_bc = local_6c;
    local_b8 = local_68;
  }
  else {
    iVar8 = *(int *)(iVar8 + 0x94);
    local_f0 = *(float *)(iVar8 + 0x60);
    local_ec = *(undefined4 *)(iVar8 + 0x70);
    local_e8 = *(undefined4 *)(iVar8 + 0x80);
    local_e0 = *(undefined4 *)(iVar8 + 100);
    local_dc = *(undefined4 *)(iVar8 + 0x74);
    local_d8 = *(undefined4 *)(iVar8 + 0x84);
    local_d0 = *(undefined4 *)(iVar8 + 0x68);
    local_cc = *(undefined4 *)(iVar8 + 0x78);
    local_c8 = *(undefined4 *)(iVar8 + 0x88);
    local_c0 = *(float *)(iVar8 + 0x90);
    local_bc = *(undefined4 *)(iVar8 + 0x94);
    local_b8 = *(undefined4 *)(iVar8 + 0x98);
  }
  local_c4 = 0;
  local_d4 = 0;
  local_e4 = 0;
  local_b4 = 0x3f800000;
  iVar8 = *(int *)(iVar4 + 0x94);
  if ((*(uint *)(iVar4 + 0x44) & 0x800) == 0) {
    pfVar5 = (float *)(iVar8 + 0x60);
    pfVar7 = local_a0;
    for (iVar6 = 0x10; iVar6 != 0; iVar6 = iVar6 + -1) {
      *pfVar7 = *pfVar5;
      pfVar5 = pfVar5 + 1;
      pfVar7 = pfVar7 + 1;
    }
    FUN_10039500(local_a0,(float *)(iVar4 + 0x30),&local_70);
    local_130 = local_a0[0];
    local_12c = local_90;
    local_128 = local_80;
    local_120 = local_a0[1];
    local_11c = local_8c;
    local_118 = local_7c;
    local_110 = local_a0[2];
    local_10c = local_88;
    local_108 = local_78;
    local_100 = local_70;
    local_fc = local_6c;
    local_f8 = local_68;
  }
  else {
    local_130 = *(float *)(iVar8 + 0x60);
    local_12c = *(undefined4 *)(iVar8 + 0x70);
    local_128 = *(undefined4 *)(iVar8 + 0x80);
    local_120 = *(undefined4 *)(iVar8 + 100);
    local_11c = *(undefined4 *)(iVar8 + 0x74);
    local_118 = *(undefined4 *)(iVar8 + 0x84);
    local_110 = *(undefined4 *)(iVar8 + 0x68);
    local_10c = *(undefined4 *)(iVar8 + 0x78);
    local_108 = *(undefined4 *)(iVar8 + 0x88);
    local_100 = *(float *)(iVar8 + 0x90);
    local_fc = *(undefined4 *)(iVar8 + 0x94);
    local_f8 = *(undefined4 *)(iVar8 + 0x98);
  }
  local_104 = 0;
  local_114 = 0;
  local_124 = 0;
  local_f4 = 0x3f800000;
  iVar8 = 0;
  do {
    pfVar5 = (float *)(param_1 + (iVar8 + 6) * 4);
    if (SQRT(*pfVar5 * *pfVar5 + pfVar5[1] * pfVar5[1] +
             (float)param_1[(iVar8 + 6) * 4 + 2] * (float)param_1[(iVar8 + 6) * 4 + 2]) == 0.0) {
      if (iVar8 == 0) {
        pfVar5 = FUN_100b9f50(local_20,(float *)((int)this + 0x28));
        *(float *)((int)this + 0x58) = *pfVar5;
        *(float *)((int)this + 0x5c) = pfVar5[1];
        *(float *)((int)this + 0x60) = pfVar5[2];
      }
      else {
        FUN_100b9e30(&local_b0,&local_f0,(float *)((int)this + 0x58));
        FUN_100b9ec0((void *)(iVar8 * 0x10 + 0x58 + (int)this),&local_130,&local_b0);
      }
    }
    else {
      pfVar7 = (float *)(iVar8 * 0x10 + 0x58 + (int)this);
      *pfVar7 = *pfVar5;
      pfVar7[1] = pfVar5[1];
      pfVar7[2] = pfVar5[2];
      fVar1 = pfVar7[1];
      fVar2 = *pfVar7;
      fVar3 = pfVar7[2];
      fVar10 = fVar2 * fVar2 + fVar1 * fVar1 + fVar3 * fVar3;
      if (fVar10 == 0.0) {
        fVar10 = 0.0;
      }
      else {
        fVar10 = 1.0 / SQRT(fVar10);
      }
      *pfVar7 = fVar10 * fVar2;
      pfVar7[1] = fVar10 * fVar1;
      pfVar7[2] = fVar10 * fVar3;
    }
    iVar8 = iVar8 + 1;
  } while (iVar8 < 2);
  FUN_100b9e30(&local_40,&local_f0,(float *)((int)this + 0x28));
  FUN_100b9e30(local_30,&local_130,(float *)((int)this + 0x48));
  FUN_100b9e30(&local_60,&local_f0,(float *)((int)this + 0x58));
  local_b0 = local_38 * local_5c - local_3c * local_58;
  local_ac = local_40 * local_58 - local_38 * local_60;
  local_a8 = local_3c * local_60 - local_5c * local_40;
  FUN_100b9e30(&local_50,&local_130,(float *)((int)this + 0x68));
  fVar9 = (float10)fpatan((float10)local_b0 * (float10)local_50 +
                          (float10)local_ac * (float10)local_4c +
                          (float10)local_a8 * (float10)local_48,
                          (float10)local_50 * (float10)local_60 +
                          (float10)local_5c * (float10)local_4c +
                          (float10)local_58 * (float10)local_48);
  fVar1 = (float)fVar9;
  if (((fVar9 < (float10)0) && (0.0 < *(float *)((int)this + 0x98))) &&
     (3.1415927 < *(float *)((int)this + 0x98) - fVar1)) {
    *(float *)((int)this + 0x98) = fVar1 + 6.2831855;
  }
  if (((0.0 < fVar1) && (*(float *)((int)this + 0x98) < 0.0)) &&
     (3.1415927 < fVar1 - *(float *)((int)this + 0x98))) {
    *(float *)((int)this + 0x98) = fVar1 - 6.2831855;
  }
  return;
}


// 1009a0b0 FUN_1009a0b0

void __thiscall FUN_1009a0b0(void *this,undefined4 param_1,int *param_2,float param_3,float param_4)

{
  float *pfVar1;
  float *pfVar2;
  float fVar3;
  float fVar4;
  int iVar5;
  int iVar6;
  int iVar7;
  float *pfVar8;
  float *pfVar9;
  float fVar10;
  float fVar11;
  float fVar12;
  float fVar13;
  float fVar14;
  float fVar15;
  float fVar16;
  float fVar17;
  float fVar18;
  float fVar19;
  float fVar20;
  float fVar21;
  float fVar22;
  float fVar23;
  float fVar24;
  float fVar25;
  float fVar26;
  float fVar27;
  float fVar28;
  float fVar29;
  float fVar30;
  float fVar31;
  float fVar32;
  float fVar33;
  float fVar34;
  float fVar35;
  float fVar36;
  float *pfVar37;
  float *pfVar38;
  float *pfVar39;
  int iVar40;
  float fVar41;
  float fVar42;
  float fVar43;
  float fVar44;
  float fVar45;
  float fVar46;
  float fVar47;
  
  iVar5 = *(int *)((int)this + 8);
  iVar6 = *(int *)((int)this + 4);
  fVar41 = *(float *)((int)this + 0x78) * param_3;
  fVar42 = *(float *)((int)this + 0x7c) * param_4 * -1.0;
  iVar7 = *(int *)(iVar6 + 0x94);
  param_2[0x1b] = 0;
  param_2[0x1a] = 0;
  param_2[0x19] = 0;
  param_2[0x18] = 0;
  param_2[0x1f] = 0;
  param_2[0x1e] = 0;
  param_2[0x1d] = 0;
  param_2[0x1c] = 0;
  pfVar39 = (float *)param_2[0xed];
  iVar40 = param_2[0xec] + -1;
  pfVar37 = pfVar39 + 5;
  do {
    pfVar1 = pfVar37 + -4;
    pfVar8 = pfVar37 + -3;
    pfVar9 = pfVar37 + 1;
    pfVar38 = pfVar37 + 3;
    fVar3 = *pfVar39;
    pfVar2 = pfVar37 + -1;
    fVar4 = *pfVar37;
    pfVar39 = pfVar39 + 0xc;
    pfVar37 = pfVar37 + 0xc;
    iVar40 = iVar40 + -1;
    param_2[(int)*pfVar38 + 0x18] =
         (int)(*pfVar1 * *(float *)(iVar7 + 0xd4) + *pfVar8 * *(float *)(iVar7 + 0xd8) +
               fVar3 * *(float *)(iVar7 + 0xd0) +
               *pfVar2 * *(float *)(iVar7 + 0xc0) + *pfVar9 * *(float *)(iVar7 + 200) +
               fVar4 * *(float *)(iVar7 + 0xc4) + (float)param_2[(int)*pfVar38 + 0x18]);
  } while (-1 < iVar40);
  iVar7 = *(int *)(iVar5 + 0x94);
  pfVar39 = (float *)param_2[0xf0];
  iVar40 = param_2[0xef] + -1;
  pfVar37 = pfVar39 + 5;
  do {
    pfVar1 = pfVar37 + -4;
    pfVar8 = pfVar37 + -3;
    pfVar9 = pfVar37 + 1;
    pfVar38 = pfVar37 + 3;
    fVar3 = *pfVar39;
    pfVar2 = pfVar37 + -1;
    fVar4 = *pfVar37;
    pfVar39 = pfVar39 + 0xc;
    pfVar37 = pfVar37 + 0xc;
    iVar40 = iVar40 + -1;
    param_2[(int)*pfVar38 + 0x18] =
         (int)(*pfVar1 * *(float *)(iVar7 + 0xd4) + *pfVar8 * *(float *)(iVar7 + 0xd8) +
               fVar3 * *(float *)(iVar7 + 0xd0) +
               *pfVar2 * *(float *)(iVar7 + 0xc0) + *pfVar9 * *(float *)(iVar7 + 200) +
               fVar4 * *(float *)(iVar7 + 0xc4) + (float)param_2[(int)*pfVar38 + 0x18]);
  } while (-1 < iVar40);
  pfVar37 = (float *)*param_2;
  iVar7 = param_2[0xc3];
  fVar43 = (float)param_2[0x18] * fVar42 + *pfVar37 * fVar41;
  fVar44 = (float)param_2[0x19] * fVar42 + pfVar37[1] * fVar41;
  fVar45 = (float)param_2[0x1a] * fVar42 + pfVar37[2] * fVar41;
  fVar46 = (float)param_2[0x1b] * fVar42 + pfVar37[3] * fVar41;
  pfVar8 = (float *)param_2[0xc0];
  fVar3 = pfVar8[4];
  fVar4 = pfVar8[5];
  fVar10 = pfVar8[6];
  fVar11 = pfVar8[7];
  fVar47 = pfVar37[4] * fVar41 + (float)param_2[0x1c] * fVar42;
  fVar41 = pfVar8[1];
  fVar42 = pfVar8[2];
  fVar12 = pfVar8[3];
  pfVar37 = pfVar8 + iVar7;
  fVar13 = pfVar37[1];
  fVar14 = pfVar37[2];
  fVar15 = pfVar37[3];
  pfVar39 = pfVar8 + iVar7 + 4;
  fVar16 = *pfVar39;
  fVar17 = pfVar39[1];
  fVar18 = pfVar39[2];
  fVar19 = pfVar39[3];
  pfVar39 = pfVar8 + iVar7 * 2;
  fVar20 = pfVar39[1];
  fVar21 = pfVar39[2];
  fVar22 = pfVar39[3];
  fVar23 = pfVar8[iVar7 * 2 + 4];
  fVar24 = pfVar8[iVar7 * 2 + 5];
  fVar25 = pfVar8[iVar7 * 2 + 6];
  fVar26 = pfVar8[iVar7 * 2 + 7];
  pfVar38 = pfVar8 + iVar7 * 3;
  fVar27 = pfVar38[1];
  fVar28 = pfVar38[2];
  fVar29 = pfVar38[3];
  pfVar1 = pfVar38 + iVar7;
  fVar30 = pfVar1[1];
  fVar31 = pfVar1[2];
  fVar32 = pfVar1[3];
  fVar33 = pfVar38[4];
  fVar34 = pfVar38[5];
  fVar35 = pfVar38[6];
  fVar36 = pfVar38[7];
  pfVar9 = (float *)param_2[0xc];
  *pfVar9 = *pfVar8 * fVar43 + 0.0 + *pfVar37 * fVar44 + *pfVar39 * fVar45 + *pfVar38 * fVar46 +
            *pfVar1 * fVar47;
  pfVar9[1] = fVar41 * fVar43 + 0.0 + fVar13 * fVar44 + fVar20 * fVar45 + fVar27 * fVar46 +
              fVar30 * fVar47;
  pfVar9[2] = fVar42 * fVar43 + 0.0 + fVar14 * fVar44 + fVar21 * fVar45 + fVar28 * fVar46 +
              fVar31 * fVar47;
  pfVar9[3] = fVar12 * fVar43 + 0.0 + fVar15 * fVar44 + fVar22 * fVar45 + fVar29 * fVar46 +
              fVar32 * fVar47;
  fVar41 = pfVar38[iVar7 + 5];
  fVar42 = pfVar38[iVar7 + 6];
  fVar12 = pfVar38[iVar7 + 7];
  iVar40 = param_2[0xc];
  *(float *)(iVar40 + 0x10) =
       pfVar38[iVar7 + 4] * fVar47 +
       fVar3 * fVar43 + 0.0 + fVar16 * fVar44 + fVar23 * fVar45 + fVar33 * fVar46;
  *(float *)(iVar40 + 0x14) =
       fVar41 * fVar47 + fVar4 * fVar43 + 0.0 + fVar17 * fVar44 + fVar24 * fVar45 + fVar34 * fVar46;
  *(float *)(iVar40 + 0x18) =
       fVar42 * fVar47 + fVar10 * fVar43 + 0.0 + fVar18 * fVar44 + fVar25 * fVar45 + fVar35 * fVar46
  ;
  *(float *)(iVar40 + 0x1c) =
       fVar12 * fVar47 + fVar11 * fVar43 + 0.0 + fVar19 * fVar44 + fVar26 * fVar45 + fVar36 * fVar46
  ;
  FUN_100a53e0(*(void **)(iVar6 + 0x94),param_2 + 0xec,param_2[0xc]);
  FUN_100a53e0(*(void **)(iVar5 + 0x94),param_2 + 0xef,param_2[0xc]);
  return;
}


// 1009a390 FUN_1009a390

undefined4 * __thiscall
FUN_1009a390(void *this,undefined4 param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4
            )

{
  FUN_100a4de0(this,param_1,param_3,param_4);
  *(undefined ***)this = &PTR_LAB_100ce6e0;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined1 *)((int)this + 0x80) = 0;
  *(undefined1 *)((int)this + 0x81) = 0;
  *(undefined4 *)((int)this + 0x84) = 0;
  *(undefined4 *)((int)this + 0x88) = 0;
  *(undefined4 *)((int)this + 0x8c) = 0x3f800000;
  *(undefined4 *)((int)this + 0x90) = 0;
  *(undefined4 *)((int)this + 0x94) = 0;
  *(undefined4 *)((int)this + 0x98) = 0;
  thunk_FUN_10099990(this,param_2);
  return (undefined4 *)this;
}


// 1009a440 FUN_1009a440

undefined4 * __thiscall
FUN_1009a440(void *this,void *param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4)

{
  FUN_100a4e00(this,param_1,param_3,param_4,0x195,0x3d0);
  *(undefined ***)this = &PTR_LAB_100ce6e0;
  *(undefined1 *)((int)this + 0x80) = 0;
  *(undefined1 *)((int)this + 0x81) = 0;
  *(undefined4 *)((int)this + 0x84) = 0;
  *(undefined4 *)((int)this + 0x88) = 0;
  *(undefined4 *)((int)this + 0x8c) = 0x3f800000;
  *(undefined4 *)((int)this + 0x90) = 0;
  *(undefined4 *)((int)this + 0x94) = 0;
  *(undefined4 *)((int)this + 0x98) = 0;
  thunk_FUN_10099990(this,param_2);
  return (undefined4 *)this;
}


// 1009a4d0 FUN_1009a4d0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __thiscall
FUN_1009a4d0(void *this,float *param_1,int *param_2,float param_3,float param_4)

{
  int *piVar1;
  int *piVar2;
  undefined4 *puVar3;
  int iVar4;
  int iVar5;
  ushort *puVar6;
  ushort *puVar7;
  float *pfVar8;
  undefined1 auVar9 [16];
  undefined1 auVar10 [16];
  float fVar11;
  float fVar12;
  float fVar13;
  float fVar14;
  float fVar15;
  float fVar16;
  float fVar17;
  float fVar18;
  float fVar19;
  float fVar20;
  float fVar21;
  float fVar22;
  float fVar23;
  float fVar24;
  float fVar25;
  void *pvVar26;
  int iVar27;
  int iVar28;
  float *pfVar29;
  float *pfVar30;
  float10 fVar31;
  uint uVar32;
  uint uVar33;
  uint uVar34;
  uint uVar35;
  float fVar36;
  float fVar37;
  float fVar40;
  float fVar41;
  float fVar42;
  float fVar43;
  undefined1 auVar38 [16];
  undefined1 auVar39 [16];
  float fVar44;
  float fVar47;
  float fVar48;
  undefined1 auVar45 [16];
  undefined1 auVar46 [16];
  float fVar49;
  float fVar50;
  float fVar51;
  float fVar52;
  float fVar53;
  float fVar54;
  float fVar55;
  float fVar56;
  undefined1 auVar57 [16];
  undefined1 auVar58 [16];
  undefined1 auVar59 [16];
  undefined1 auVar60 [16];
  int *local_274;
  float local_268;
  float local_264;
  float local_260;
  void *local_258;
  float local_254;
  float local_250;
  float local_24c;
  int *local_244;
  undefined1 local_240 [8];
  float fStack_238;
  float fStack_234;
  float local_230;
  undefined4 local_22c;
  undefined4 local_228;
  undefined4 local_224;
  undefined4 local_220;
  undefined4 local_21c;
  undefined4 local_218;
  undefined4 local_214;
  undefined4 local_210;
  undefined4 local_20c;
  undefined4 local_208;
  undefined4 local_204;
  float local_200;
  undefined4 local_1fc;
  undefined4 local_1f8;
  undefined4 local_1f4;
  float local_1f0;
  undefined4 local_1ec;
  undefined4 local_1e8;
  undefined4 local_1e4;
  undefined4 local_1e0;
  undefined4 local_1dc;
  undefined4 local_1d8;
  undefined4 local_1d4;
  undefined4 local_1d0;
  undefined4 local_1cc;
  undefined4 local_1c8;
  undefined4 local_1c4;
  undefined4 local_1c0;
  undefined4 local_1bc;
  undefined4 local_1b8;
  undefined4 local_1b4;
  float local_1b0;
  undefined4 uStack_1ac;
  undefined4 uStack_1a8;
  undefined4 uStack_1a4;
  float local_1a0;
  float fStack_19c;
  float fStack_198;
  float fStack_194;
  float local_190;
  float fStack_18c;
  float fStack_188;
  float fStack_184;
  float local_180;
  undefined4 uStack_17c;
  undefined4 uStack_178;
  undefined4 uStack_174;
  float local_170;
  float local_16c;
  float local_168;
  float local_160;
  float local_15c;
  float local_158;
  float local_150;
  float local_14c;
  float local_148;
  float local_140;
  undefined4 uStack_13c;
  undefined4 uStack_138;
  undefined4 uStack_134;
  float local_130;
  float fStack_12c;
  float fStack_128;
  float local_120;
  float local_11c;
  float local_118;
  float local_110;
  float fStack_10c;
  float fStack_108;
  float fStack_104;
  float local_100;
  float local_f8;
  float local_f0;
  float fStack_ec;
  float fStack_e8;
  float fStack_e4;
  float local_e0;
  float local_dc;
  float local_d8;
  float local_d0 [4];
  undefined4 local_c0;
  undefined4 local_bc;
  undefined4 local_b8;
  undefined4 local_b0;
  undefined4 local_ac;
  undefined4 local_a8;
  float local_a0;
  undefined4 local_9c;
  undefined4 local_98;
  float local_90 [4];
  undefined4 local_80;
  undefined4 local_7c;
  undefined4 local_78;
  undefined1 local_70 [16];
  undefined1 local_60 [2] [16];
  float local_40;
  float fStack_3c;
  float fStack_38;
  float fStack_34;
  uint local_30;
  undefined4 uStack_2c;
  undefined4 uStack_28;
  undefined4 uStack_24;
  
  iVar4 = *(int *)((int)this + 4);
  iVar5 = *(int *)((int)this + 8);
  iVar28 = *(int *)(iVar4 + 0x94);
  local_258 = this;
  if ((*(uint *)(iVar4 + 0x44) & 0x800) == 0) {
    pfVar29 = (float *)(iVar28 + 0x60);
    pfVar30 = local_d0;
    for (iVar27 = 0x10; iVar27 != 0; iVar27 = iVar27 + -1) {
      *pfVar30 = *pfVar29;
      pfVar29 = pfVar29 + 1;
      pfVar30 = pfVar30 + 1;
    }
    FUN_10039500(local_d0,(float *)(iVar4 + 0x30),&local_a0);
    local_220 = local_d0[1];
    local_21c = local_bc;
    local_218 = local_ac;
    local_210 = local_d0[2];
    local_20c = local_b8;
    local_230 = local_d0[0];
    local_208 = local_a8;
    local_22c = local_c0;
    local_200 = local_a0;
    local_228 = local_b0;
    local_1fc = local_9c;
  }
  else {
    local_230 = *(float *)(iVar28 + 0x60);
    local_22c = *(undefined4 *)(iVar28 + 0x70);
    local_228 = *(undefined4 *)(iVar28 + 0x80);
    local_220 = *(undefined4 *)(iVar28 + 100);
    local_21c = *(undefined4 *)(iVar28 + 0x74);
    local_218 = *(undefined4 *)(iVar28 + 0x84);
    local_210 = *(undefined4 *)(iVar28 + 0x68);
    local_20c = *(undefined4 *)(iVar28 + 0x78);
    local_208 = *(undefined4 *)(iVar28 + 0x88);
    local_200 = *(float *)(iVar28 + 0x90);
    local_1fc = *(undefined4 *)(iVar28 + 0x94);
    local_98 = *(undefined4 *)(iVar28 + 0x98);
  }
  local_204 = 0;
  local_214 = 0;
  local_224 = 0;
  local_1f4 = 0x3f800000;
  iVar28 = *(int *)(iVar5 + 0x94);
  local_1f8 = local_98;
  if ((*(uint *)(iVar5 + 0x44) & 0x800) == 0) {
    pfVar29 = (float *)(iVar28 + 0x60);
    pfVar30 = local_90;
    for (iVar27 = 0x10; iVar27 != 0; iVar27 = iVar27 + -1) {
      *pfVar30 = *pfVar29;
      pfVar29 = pfVar29 + 1;
      pfVar30 = pfVar30 + 1;
    }
    FUN_10039500(local_90,(float *)(iVar5 + 0x30),(float *)local_60);
    local_1f0 = local_90[0];
    local_1ec = local_80;
    local_1e8 = local_70._0_4_;
    local_1e0 = local_90[1];
    local_1dc = local_7c;
    local_1d8 = local_70._4_4_;
    local_1c0 = local_60[0]._0_4_;
    local_1d0 = local_90[2];
    local_1bc = local_60[0]._4_4_;
    local_1cc = local_78;
    local_1b8 = local_60[0]._8_4_;
    local_1c8 = local_70._8_4_;
  }
  else {
    local_1f0 = *(float *)(iVar28 + 0x60);
    local_1ec = *(undefined4 *)(iVar28 + 0x70);
    local_1e8 = *(undefined4 *)(iVar28 + 0x80);
    local_1e0 = *(undefined4 *)(iVar28 + 100);
    local_1dc = *(undefined4 *)(iVar28 + 0x74);
    local_1d8 = *(undefined4 *)(iVar28 + 0x84);
    local_1d0 = *(undefined4 *)(iVar28 + 0x68);
    local_1cc = *(undefined4 *)(iVar28 + 0x78);
    local_1c8 = *(undefined4 *)(iVar28 + 0x88);
    local_1c0 = *(undefined4 *)(iVar28 + 0x90);
    local_1bc = *(undefined4 *)(iVar28 + 0x94);
    local_1b8 = *(undefined4 *)(iVar28 + 0x98);
  }
  pvVar26 = local_258;
  local_1b4 = 0x3f800000;
  local_1c4 = 0;
  local_1d4 = 0;
  local_1e4 = 0;
  FUN_100b9d90(&local_170,&local_230,(float *)((int)local_258 + 0x18));
  FUN_100b9d90(&local_160,&local_1f0,(float *)((int)pvVar26 + 0x38));
  FUN_100b9e30(&local_130,&local_230,(float *)((int)pvVar26 + 0x28));
  FUN_100b9e30(&local_120,&local_1f0,(float *)((int)pvVar26 + 0x48));
  FUN_100b9e30(&local_268,&local_230,(float *)((int)pvVar26 + 0x58));
  local_254 = fStack_128 * local_264 - fStack_12c * local_260;
  local_250 = local_130 * local_260 - local_268 * fStack_128;
  local_24c = local_268 * fStack_12c - local_130 * local_264;
  if (*(char *)((int)pvVar26 + 0x81) != '\0' || *(char *)((int)pvVar26 + 0x80) != '\0') {
    local_110 = param_3;
    fStack_10c = param_4;
    fStack_108 = 0.0;
    fStack_104 = 0.0;
    FUN_100b9e30(&local_e0,&local_1f0,(float *)((int)pvVar26 + 0x68));
    fVar31 = (float10)fpatan((float10)local_250 * (float10)local_dc +
                             (float10)local_24c * (float10)local_d8 +
                             (float10)local_254 * (float10)local_e0,
                             (float10)local_e0 * (float10)local_268 +
                             (float10)local_264 * (float10)local_dc +
                             (float10)local_260 * (float10)local_d8);
    fStack_e4 = (float)fVar31;
    puVar6 = *(ushort **)(iVar4 + 0x94);
    puVar7 = *(ushort **)(iVar5 + 0x94);
    fVar52 = DAT_100bf720;
    fVar54 = DAT_100bf720._4_4_;
    fVar55 = DAT_100bf720._8_4_;
    fVar56 = DAT_100bf720._12_4_;
    fVar49 = fStack_128 * *(float *)(puVar6 + 0x40) + fStack_12c * *(float *)(puVar6 + 0x38) +
             local_130 * *(float *)(puVar6 + 0x30);
    fVar50 = fStack_128 * *(float *)(puVar6 + 0x42) + fStack_12c * *(float *)(puVar6 + 0x3a) +
             local_130 * *(float *)(puVar6 + 0x32);
    fVar51 = fStack_128 * *(float *)(puVar6 + 0x44) + fStack_12c * *(float *)(puVar6 + 0x3c) +
             local_130 * *(float *)(puVar6 + 0x34);
    fVar36 = fVar52 - local_130;
    fVar40 = fVar54 - fStack_12c;
    fVar42 = fVar55 - fStack_128;
    fVar44 = fVar42 * *(float *)(puVar7 + 0x40) + fVar40 * *(float *)(puVar7 + 0x38) +
             fVar36 * *(float *)(puVar7 + 0x30);
    fVar47 = fVar42 * *(float *)(puVar7 + 0x42) + fVar40 * *(float *)(puVar7 + 0x3a) +
             fVar36 * *(float *)(puVar7 + 0x32);
    fVar48 = fVar42 * *(float *)(puVar7 + 0x44) + fVar40 * *(float *)(puVar7 + 0x3c) +
             fVar36 * *(float *)(puVar7 + 0x34);
    auVar38 = _DAT_100bf720;
    auVar60 = _DAT_100bf720;
    local_70 = _DAT_100bf720;
    if ((*puVar6 & 0x12) == 0) {
      auVar38._0_4_ =
           *(float *)(puVar6 + 100) * fVar51 + *(float *)(puVar6 + 0x62) * fVar50 +
           *(float *)(puVar6 + 0x60) * fVar49;
      local_70._0_4_ = *(float *)(puVar6 + 0x18) * fVar49;
      local_70._4_4_ = *(float *)(puVar6 + 0x1a) * fVar50;
      local_70._8_4_ = *(float *)(puVar6 + 0x1c) * fVar51;
      local_70._12_4_ =
           *(float *)(puVar6 + 0x1e) *
           (fStack_128 * *(float *)(puVar6 + 0x46) + fStack_12c * *(float *)(puVar6 + 0x3e) +
           local_130 * *(float *)(puVar6 + 0x36));
      fVar49 = (float)local_70._8_4_ * fVar51 + (float)local_70._4_4_ * fVar50 +
               (float)local_70._0_4_ * fVar49;
      auVar38._4_4_ = auVar38._0_4_;
      auVar38._8_4_ = auVar38._0_4_;
      auVar38._12_4_ = auVar38._0_4_;
      auVar60._4_4_ = fVar49;
      auVar60._0_4_ = fVar49;
      auVar60._8_4_ = fVar49;
      auVar60._12_4_ = fVar49;
    }
    local_60[0] = _DAT_100bf720;
    _local_240 = auVar38;
    if ((*puVar7 & 0x12) == 0) {
      fVar51 = *(float *)(puVar7 + 100) * fVar48 + *(float *)(puVar7 + 0x62) * fVar47 +
               *(float *)(puVar7 + 0x60) * fVar44;
      local_60[0]._0_4_ = *(float *)(puVar7 + 0x18) * fVar44;
      local_60[0]._4_4_ = *(float *)(puVar7 + 0x1a) * fVar47;
      local_60[0]._8_4_ = *(float *)(puVar7 + 0x1c) * fVar48;
      local_60[0]._12_4_ =
           *(float *)(puVar7 + 0x1e) *
           (fVar42 * *(float *)(puVar7 + 0x46) + fVar40 * *(float *)(puVar7 + 0x3e) +
           fVar36 * *(float *)(puVar7 + 0x36));
      fVar36 = auVar38._0_4_;
      fVar42 = auVar38._4_4_;
      fVar49 = auVar38._8_4_;
      fVar50 = auVar38._12_4_;
      fVar40 = (float)local_60[0]._8_4_ * fVar48 + (float)local_60[0]._4_4_ * fVar47 +
               (float)local_60[0]._0_4_ * fVar44;
      auVar38 = ZEXT416((uint)fVar40);
      auVar57._4_4_ = fVar40 + auVar60._4_4_;
      auVar57._0_4_ = fVar40 + auVar60._0_4_;
      local_240._4_4_ = fVar51 + fVar42;
      local_240._0_4_ = fVar51 + fVar36;
      fStack_238 = fVar51 + fVar49;
      fStack_234 = fVar51 + fVar50;
      auVar57._8_4_ = fVar40 + auVar60._8_4_;
      auVar57._12_4_ = fVar40 + auVar60._12_4_;
      auVar60 = auVar57;
    }
    auVar38 = rcpps(auVar38,auVar60);
    fVar36 = auVar38._0_4_;
    fVar40 = auVar38._4_4_;
    fVar42 = auVar38._8_4_;
    fVar44 = auVar38._12_4_;
    fVar47 = auVar60._0_4_;
    fVar48 = auVar60._4_4_;
    fVar49 = auVar60._8_4_;
    fVar50 = auVar60._12_4_;
    iVar28 = (*(byte *)((int)pvVar26 + 0x80) & 1) * 0x10;
    local_40 = (float)((uint)((fVar36 + fVar36) - fVar36 * fVar36 * fVar47) &
                      -(uint)(1.1920929e-07 < fVar47));
    fStack_3c = (float)((uint)((fVar40 + fVar40) - fVar40 * fVar40 * fVar48) &
                       -(uint)(1.1920929e-07 < fVar48));
    fStack_38 = (float)((uint)((fVar42 + fVar42) - fVar42 * fVar42 * fVar49) &
                       -(uint)(1.1920929e-07 < fVar49));
    fStack_34 = (float)((uint)((fVar44 + fVar44) - fVar44 * fVar44 * fVar50) &
                       -(uint)(1.1920929e-07 < fVar50));
    local_30 = *(uint *)(&DAT_100ce680 + iVar28);
    uStack_2c = *(undefined4 *)(&UNK_100ce684 + iVar28);
    uStack_28 = *(undefined4 *)(&UNK_100ce688 + iVar28);
    uStack_24 = *(undefined4 *)(&UNK_100ce68c + iVar28);
    local_140 = *(float *)((int)pvVar26 + 0x90);
    uStack_13c = 0;
    uStack_138 = 0;
    uStack_134 = 0;
    local_1b0 = *param_1;
    fVar36 = *(float *)((int)pvVar26 + 0x98);
    uStack_1ac = 0;
    uStack_1a8 = 0;
    uStack_1a4 = 0;
    local_180 = param_1[1];
    uStack_17c = 0;
    uStack_178 = 0;
    uStack_174 = 0;
    auVar46 = _DAT_100bf720;
    local_f0 = fStack_e4;
    fStack_ec = fStack_e4;
    fStack_e8 = fStack_e4;
    if (*(char *)((int)pvVar26 + 0x81) != '\0') {
      fVar40 = fVar36 - fStack_e4;
      fVar42 = fVar36 - fStack_e4;
      fVar44 = fVar36 - fStack_e4;
      fVar51 = fVar36 - fStack_e4;
      uVar32 = -(uint)(fVar40 < fVar52 - 3.1415927);
      uVar33 = -(uint)(fVar42 < fVar54 - 3.1415927);
      uVar34 = -(uint)(fVar44 < fVar55 - 3.1415927);
      uVar35 = -(uint)(fVar51 < fVar56 - 3.1415927);
      local_f0 = (float)(~-(uint)(3.1415927 < fVar40) &
                         ((uint)(fVar52 - 6.2831855) & uVar32 | ~uVar32 & (uint)fVar52) |
                        -(uint)(3.1415927 < fVar40) & 0x40c90fdb);
      fVar52 = (float)(~-(uint)(3.1415927 < fVar42) &
                       ((uint)(fVar54 - 6.2831855) & uVar33 | ~uVar33 & (uint)fVar54) |
                      -(uint)(3.1415927 < fVar42) & 0x40c90fdb);
      fVar54 = (float)(~-(uint)(3.1415927 < fVar44) &
                       ((uint)(fVar55 - 6.2831855) & uVar34 | ~uVar34 & (uint)fVar55) |
                      -(uint)(3.1415927 < fVar44) & 0x40c90fdb);
      fVar55 = (float)(~-(uint)(3.1415927 < fVar51) &
                       ((uint)(fVar56 - 6.2831855) & uVar35 | ~uVar35 & (uint)fVar56) |
                      -(uint)(3.1415927 < fVar51) & 0x40c90fdb);
      fVar40 = fVar40 - local_f0;
      local_f0 = local_f0 + fStack_e4;
      fStack_ec = fVar52 + fStack_e4;
      fStack_e8 = fVar54 + fStack_e4;
      fStack_e4 = fVar55 + fStack_e4;
      auVar45._0_4_ = fStack_10c * (float)local_240._0_4_;
      auVar45._4_4_ = fStack_10c * (float)local_240._4_4_;
      auVar45._8_4_ = fStack_10c * fStack_238;
      auVar45._12_4_ = fStack_10c * fStack_234;
      auVar39._0_4_ = (local_110 * 0.8 * fVar40 * local_180 - auVar45._0_4_) * local_40;
      auVar39._4_4_ = (fStack_10c * 0.8 * (fVar42 - fVar52) * 0.0 - auVar45._4_4_) * fStack_3c;
      auVar39._8_4_ = (fStack_108 * 0.8 * (fVar44 - fVar54) * 0.0 - auVar45._8_4_) * fStack_38;
      auVar39._12_4_ = (fStack_104 * 0.8 * (fVar51 - fVar55) * 0.0 - auVar45._12_4_) * fStack_34;
      auVar38 = rcpps(auVar45,auVar39);
      fVar52 = auVar38._0_4_;
      fVar54 = auVar38._4_4_;
      fVar55 = auVar38._8_4_;
      fVar56 = auVar38._12_4_;
      fVar52 = ABS((fVar52 + fVar52) - fVar52 * fVar52 * auVar39._0_4_) * local_140 * local_1b0;
      fVar54 = ABS((fVar54 + fVar54) - fVar54 * fVar54 * auVar39._4_4_) * local_140 * 0.0;
      fVar55 = ABS((fVar55 + fVar55) - fVar55 * fVar55 * auVar39._8_4_) * local_140 * 0.0;
      fVar56 = ABS((fVar56 + fVar56) - fVar56 * fVar56 * auVar39._12_4_) * local_140 * 0.0;
      uVar32 = -(uint)(fVar52 < 1.0);
      uVar33 = -(uint)(fVar54 < 1.0);
      uVar34 = -(uint)(fVar55 < 1.0);
      uVar35 = -(uint)(fVar56 < 1.0);
      fVar52 = (float)(uVar32 & (uint)fVar52 | ~uVar32 & 0x3f800000);
      auVar46._0_4_ = fVar52 * auVar39._0_4_;
      auVar46._4_4_ = (float)(uVar33 & (uint)fVar54 | ~uVar33 & 0x3f800000) * auVar39._4_4_;
      auVar46._8_4_ = (float)(uVar34 & (uint)fVar55 | ~uVar34 & 0x3f800000) * auVar39._8_4_;
      auVar46._12_4_ = (float)(uVar35 & (uint)fVar56 | ~uVar35 & 0x3f800000) * auVar39._12_4_;
      *(float *)((int)pvVar26 + 0x98) =
           (*(float *)((int)pvVar26 + 0x94) * local_1b0 + fVar36) - (fVar40 - fVar52 * fVar40);
      fVar52 = 0.0;
      fVar54 = 0.0;
      fVar55 = 0.0;
      fVar56 = 0.0;
    }
    fVar36 = *(float *)((int)pvVar26 + 0x8c);
    local_1a0 = *(float *)((int)pvVar26 + 0x88);
    fStack_19c = 0.0;
    fStack_198 = 0.0;
    fStack_194 = 0.0;
    local_190 = *(float *)((int)pvVar26 + 0x84);
    fVar47 = (fVar47 * auVar46._0_4_ + (float)local_240._0_4_) * local_1b0 + local_f0;
    fVar48 = (fVar48 * auVar46._4_4_ + (float)local_240._4_4_) * 0.0 + fStack_ec;
    fVar49 = (fVar49 * auVar46._8_4_ + fStack_238) * 0.0 + fStack_e8;
    fVar50 = (fVar50 * auVar46._12_4_ + fStack_234) * 0.0 + fStack_e4;
    fVar40 = fVar36 * local_110 * local_180 * local_40;
    fVar42 = fVar36 * fStack_10c * fStack_3c * 0.0;
    fVar44 = fVar36 * fStack_108 * fStack_38 * 0.0;
    fVar36 = fVar36 * fStack_104 * fStack_34 * 0.0;
    fStack_18c = 0.0;
    fStack_188 = 0.0;
    fStack_184 = 0.0;
    auVar59._0_4_ = (fVar47 - local_190) * fVar40;
    auVar59._4_4_ = (fVar48 - local_190) * fVar42;
    auVar59._8_4_ = (fVar49 - local_190) * fVar44;
    auVar59._12_4_ = (fVar50 - local_190) * fVar36;
    auVar58._0_4_ = (fVar47 - local_1a0) * fVar40;
    auVar58._4_4_ = (fVar48 - local_1a0) * fVar42;
    auVar58._8_4_ = (fVar49 - local_1a0) * fVar44;
    auVar58._12_4_ = (fVar50 - local_1a0) * fVar36;
    auVar9._4_4_ = fVar54;
    auVar9._0_4_ = fVar52;
    auVar9._8_4_ = fVar55;
    auVar9._12_4_ = fVar56;
    auVar60 = minps(auVar59,auVar9);
    auVar10._4_4_ = fVar54;
    auVar10._0_4_ = fVar52;
    auVar10._8_4_ = fVar55;
    auVar10._12_4_ = fVar56;
    auVar38 = maxps(auVar58,auVar10);
    fVar52 = auVar46._0_4_ - (float)((uint)(auVar60._0_4_ + auVar38._0_4_) & local_30);
    *(float *)(puVar6 + 0x60) = local_70._0_4_ * fVar52 + *(float *)(puVar6 + 0x60);
    *(float *)(puVar6 + 0x62) = local_70._4_4_ * fVar52 + *(float *)(puVar6 + 0x62);
    *(float *)(puVar6 + 100) = local_70._8_4_ * fVar52 + *(float *)(puVar6 + 100);
    puVar6[0x66] = 0;
    puVar6[0x67] = 0;
    *(float *)(puVar7 + 0x60) = local_60[0]._0_4_ * fVar52 + *(float *)(puVar7 + 0x60);
    *(float *)(puVar7 + 0x62) = local_60[0]._4_4_ * fVar52 + *(float *)(puVar7 + 0x62);
    *(float *)(puVar7 + 100) = local_60[0]._8_4_ * fVar52 + *(float *)(puVar7 + 100);
    puVar7[0x66] = 0;
    puVar7[0x67] = 0;
  }
  if (param_2 == (int *)0x0) {
    local_274 = (int *)0x0;
  }
  else {
    *param_2 = (int)(param_2 + 4);
    param_2[1] = 5;
    param_2[2] = 8;
    param_2[0xc] = (int)(param_2 + 0x10);
    param_2[0xd] = 5;
    param_2[0xe] = 8;
    param_2[0xc0] = (int)(param_2 + 0xc4);
    param_2[0xc1] = 5;
    param_2[0xc2] = 5;
    param_2[0xc3] = 8;
    local_274 = param_2;
  }
  pfVar30 = (float *)(local_274 + 0x18);
  FUN_10097030(pfVar30);
  iVar28 = *(int *)(iVar4 + 0x94);
  iVar27 = *(int *)(iVar5 + 0x94);
  local_148 = local_168 - *(float *)(iVar28 + 0x98);
  local_150 = local_170 - *(float *)(iVar28 + 0x90);
  local_14c = local_16c - *(float *)(iVar28 + 0x94);
  puVar3 = (undefined4 *)((int)local_274 + local_274[0xf2] + 0x120);
  local_100 = local_160 - *(float *)(iVar27 + 0x90);
  fVar47 = local_15c - *(float *)(iVar27 + 0x94);
  local_f8 = local_158 - *(float *)(iVar27 + 0x98);
  puVar3[2] = 0;
  puVar3[1] = 0;
  *puVar3 = 0x3f800000;
  puVar3[5] = local_148;
  puVar3[4] = 0;
  fVar44 = -0.0 - local_14c;
  puVar3[6] = fVar44;
  iVar28 = *(int *)(iVar4 + 0x94);
  fVar52 = (float)puVar3[5];
  fVar54 = *(float *)(iVar28 + 0x68);
  fVar55 = *(float *)(iVar28 + 0x78);
  fVar56 = *(float *)(iVar28 + 0x88);
  fVar36 = *(float *)(iVar28 + 0x70);
  fVar40 = *(float *)(iVar28 + 0x80);
  fVar42 = *(float *)(iVar28 + 0x60);
  puVar3[5] = *(float *)(iVar28 + 100) * 0.0 + *(float *)(iVar28 + 0x74) * fVar52 +
              *(float *)(iVar28 + 0x84) * fVar44;
  puVar3[6] = fVar54 * 0.0 + fVar55 * fVar52 + fVar56 * fVar44;
  puVar3[4] = fVar36 * fVar52 + fVar40 * fVar44 + fVar42 * 0.0;
  puVar3[8] = local_274[0xf3];
  puVar3 = (undefined4 *)((int)local_274 + local_274[0xf2] + 0x210);
  puVar3[2] = 0;
  puVar3[1] = 0;
  *puVar3 = 0xbf800000;
  puVar3[4] = 0;
  puVar3[6] = fVar47;
  fVar42 = local_f8 * -1.0;
  puVar3[5] = fVar42;
  iVar28 = *(int *)(iVar5 + 0x94);
  fVar52 = *(float *)(iVar28 + 0x68);
  fVar54 = *(float *)(iVar28 + 0x78);
  fVar55 = *(float *)(iVar28 + 0x88);
  fVar56 = *(float *)(iVar28 + 0x70);
  fVar36 = *(float *)(iVar28 + 0x80);
  fVar40 = *(float *)(iVar28 + 0x60);
  puVar3[5] = *(float *)(iVar28 + 100) * 0.0 + *(float *)(iVar28 + 0x74) * fVar42 +
              *(float *)(iVar28 + 0x84) * fVar47;
  puVar3[6] = fVar52 * 0.0 + fVar54 * fVar42 + fVar55 * fVar47;
  puVar3[4] = fVar56 * fVar42 + fVar36 * fVar47 + fVar40 * 0.0;
  puVar3[8] = local_274[0xf3];
  iVar28 = local_274[0xf2];
  *(undefined4 *)((int)local_274 + iVar28 + 0x158) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x150) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x154) = 0x3f800000;
  *(float *)((int)local_274 + iVar28 + 0x160) = -0.0 - local_148;
  *(float *)((int)local_274 + iVar28 + 0x168) = local_150;
  *(undefined4 *)((int)local_274 + iVar28 + 0x164) = 0;
  iVar27 = *(int *)(iVar4 + 0x94);
  fVar52 = *(float *)((int)local_274 + iVar28 + 0x160);
  fVar54 = *(float *)(iVar27 + 0x68);
  fVar55 = *(float *)(iVar27 + 0x78);
  fVar56 = *(float *)(iVar27 + 0x88);
  fVar36 = *(float *)(iVar27 + 0x70);
  fVar40 = *(float *)(iVar27 + 0x80);
  fVar42 = *(float *)(iVar27 + 0x60);
  *(float *)((int)local_274 + iVar28 + 0x164) =
       *(float *)(iVar27 + 100) * fVar52 + *(float *)(iVar27 + 0x74) * 0.0 +
       *(float *)(iVar27 + 0x84) * local_150;
  *(float *)((int)local_274 + iVar28 + 0x168) = fVar54 * fVar52 + fVar55 * 0.0 + fVar56 * local_150;
  *(float *)((int)local_274 + iVar28 + 0x160) =
       fVar36 * 0.0 + fVar40 * local_150 + fVar42 * *(float *)((int)local_274 + iVar28 + 0x160);
  *(int *)((int)local_274 + iVar28 + 0x170) = local_274[0xf3] + 1;
  iVar28 = local_274[0xf2];
  *(undefined4 *)((int)local_274 + iVar28 + 0x248) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x240) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x244) = 0xbf800000;
  *(float *)((int)local_274 + iVar28 + 0x250) = local_f8;
  *(undefined4 *)((int)local_274 + iVar28 + 0x254) = 0;
  *(float *)((int)local_274 + iVar28 + 600) = local_100 * -1.0;
  iVar27 = *(int *)(iVar5 + 0x94);
  fVar52 = *(float *)((int)local_274 + iVar28 + 0x250);
  fVar54 = *(float *)(iVar27 + 0x68);
  fVar55 = *(float *)(iVar27 + 0x78);
  fVar56 = *(float *)(iVar27 + 0x60);
  fVar36 = *(float *)((int)local_274 + iVar28 + 600);
  fVar40 = *(float *)(iVar27 + 0x88);
  fVar42 = *(float *)(iVar27 + 0x70);
  fVar44 = *(float *)(iVar27 + 0x80);
  *(float *)((int)local_274 + iVar28 + 0x254) =
       *(float *)(iVar27 + 100) * fVar52 + *(float *)(iVar27 + 0x74) * 0.0 +
       *(float *)(iVar27 + 0x84) * local_100 * -1.0;
  *(float *)((int)local_274 + iVar28 + 600) = fVar54 * fVar52 + fVar55 * 0.0 + fVar40 * fVar36;
  *(float *)((int)local_274 + iVar28 + 0x250) = fVar42 * 0.0 + fVar44 * fVar36 + fVar52 * fVar56;
  *(int *)((int)local_274 + iVar28 + 0x260) = local_274[0xf3] + 1;
  puVar3 = (undefined4 *)((int)local_274 + local_274[0xf2] + 0x180);
  puVar3[1] = 0;
  *puVar3 = 0;
  puVar3[2] = 0x3f800000;
  puVar3[4] = local_14c;
  fVar48 = -0.0 - local_150;
  puVar3[5] = fVar48;
  puVar3[6] = 0;
  iVar28 = *(int *)(iVar4 + 0x94);
  fVar52 = (float)puVar3[4];
  fVar54 = *(float *)(iVar28 + 0x68);
  fVar55 = *(float *)(iVar28 + 0x78);
  fVar56 = *(float *)(iVar28 + 0x60);
  fVar36 = *(float *)(iVar28 + 0x88);
  fVar40 = *(float *)(iVar28 + 0x70);
  fVar42 = (float)puVar3[5];
  fVar44 = *(float *)(iVar28 + 0x80);
  puVar3[5] = *(float *)(iVar28 + 100) * fVar52 + *(float *)(iVar28 + 0x74) * fVar48 +
              *(float *)(iVar28 + 0x84) * 0.0;
  puVar3[4] = fVar40 * fVar42 + fVar44 * 0.0 + fVar52 * fVar56;
  puVar3[6] = fVar54 * fVar52 + fVar55 * fVar48 + fVar36 * 0.0;
  puVar3[8] = local_274[0xf3] + 2;
  iVar28 = local_274[0xf2];
  *(undefined4 *)((int)local_274 + iVar28 + 0x274) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x270) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x278) = 0xbf800000;
  *(undefined4 *)((int)local_274 + iVar28 + 0x288) = 0;
  *(float *)((int)local_274 + iVar28 + 0x280) = fVar47 * -1.0;
  *(float *)((int)local_274 + iVar28 + 0x284) = local_100;
  iVar27 = *(int *)(iVar5 + 0x94);
  fVar52 = *(float *)(iVar27 + 0x78);
  fVar54 = *(float *)((int)local_274 + iVar28 + 0x280);
  fVar55 = *(float *)(iVar27 + 0x68);
  fVar56 = *(float *)(iVar27 + 0x60);
  fVar36 = *(float *)(iVar27 + 0x88);
  fVar40 = *(float *)(iVar27 + 0x70);
  fVar42 = *(float *)(iVar27 + 0x80);
  *(float *)((int)local_274 + iVar28 + 0x284) =
       *(float *)(iVar27 + 100) * fVar54 + *(float *)(iVar27 + 0x74) * local_100 +
       *(float *)(iVar27 + 0x84) * 0.0;
  *(float *)((int)local_274 + iVar28 + 0x288) = fVar55 * fVar54 + fVar52 * local_100 + fVar36 * 0.0;
  *(float *)((int)local_274 + iVar28 + 0x280) = fVar40 * local_100 + fVar42 * 0.0 + fVar54 * fVar56;
  *(int *)((int)local_274 + iVar28 + 0x290) = local_274[0xf3] + 2;
  puVar3 = (undefined4 *)((int)local_274 + local_274[0xf2] + 0x1b0);
  puVar3[2] = 0;
  puVar3[1] = 0;
  *puVar3 = 0;
  puVar3[4] = local_268;
  puVar3[5] = local_264;
  puVar3[6] = local_260;
  iVar28 = *(int *)(iVar4 + 0x94);
  fVar52 = (float)puVar3[5];
  fVar54 = *(float *)(iVar28 + 100);
  fVar55 = *(float *)(iVar28 + 0x74);
  fVar56 = (float)puVar3[4];
  fVar36 = *(float *)(iVar28 + 0x84);
  fVar40 = *(float *)(iVar28 + 0x60);
  fVar42 = *(float *)(iVar28 + 0x70);
  fVar44 = *(float *)(iVar28 + 0x80);
  puVar3[6] = *(float *)(iVar28 + 0x68) * fVar56 + *(float *)(iVar28 + 0x78) * fVar52 +
              *(float *)(iVar28 + 0x88) * local_260;
  puVar3[5] = fVar54 * fVar56 + fVar55 * fVar52 + fVar36 * local_260;
  puVar3[4] = fVar42 * fVar52 + fVar44 * local_260 + fVar56 * fVar40;
  puVar3[8] = local_274[0xf3] + 3;
  iVar28 = local_274[0xf2];
  *(undefined4 *)((int)local_274 + iVar28 + 0x2a8) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x2a4) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x2a0) = 0;
  *(float *)((int)local_274 + iVar28 + 0x2b0) = local_268 * -1.0;
  *(float *)((int)local_274 + iVar28 + 0x2b4) = local_264 * -1.0;
  fVar47 = local_260 * -1.0;
  *(float *)((int)local_274 + iVar28 + 0x2b8) = fVar47;
  iVar27 = *(int *)(iVar5 + 0x94);
  fVar52 = *(float *)((int)local_274 + iVar28 + 0x2b4);
  fVar54 = *(float *)(iVar27 + 100);
  fVar55 = *(float *)(iVar27 + 0x74);
  fVar56 = *(float *)(iVar27 + 0x78);
  fVar36 = *(float *)((int)local_274 + iVar28 + 0x2b0);
  fVar40 = *(float *)(iVar27 + 0x84);
  fVar42 = *(float *)(iVar27 + 0x68);
  fVar44 = *(float *)(iVar27 + 0x88);
  *(float *)((int)local_274 + iVar28 + 0x2b0) =
       *(float *)(iVar27 + 0x70) * fVar52 + *(float *)(iVar27 + 0x80) * fVar47 +
       fVar36 * *(float *)(iVar27 + 0x60);
  *(float *)((int)local_274 + iVar28 + 0x2b4) = fVar54 * fVar36 + fVar55 * fVar52 + fVar40 * fVar47;
  *(float *)((int)local_274 + iVar28 + 0x2b8) = fVar42 * fVar36 + fVar56 * fVar52 + fVar44 * fVar47;
  *(int *)((int)local_274 + iVar28 + 0x2c0) = local_274[0xf3] + 3;
  iVar28 = local_274[0xf2];
  *(undefined4 *)((int)local_274 + iVar28 + 0x1e8) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x1e4) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x1e0) = 0;
  *(float *)((int)local_274 + iVar28 + 0x1f0) = local_254;
  *(float *)((int)local_274 + iVar28 + 500) = local_250;
  *(float *)((int)local_274 + iVar28 + 0x1f8) = local_24c;
  iVar27 = *(int *)(iVar4 + 0x94);
  fVar52 = *(float *)((int)local_274 + iVar28 + 500);
  fVar54 = *(float *)(iVar27 + 0x78);
  fVar55 = *(float *)((int)local_274 + iVar28 + 0x1f0);
  fVar56 = *(float *)(iVar27 + 0x68);
  fVar36 = *(float *)(iVar27 + 0x60);
  fVar40 = *(float *)(iVar27 + 0x88);
  fVar42 = *(float *)(iVar27 + 0x70);
  fVar44 = *(float *)(iVar27 + 0x80);
  *(float *)((int)local_274 + iVar28 + 500) =
       *(float *)(iVar27 + 100) * fVar55 + *(float *)(iVar27 + 0x74) * fVar52 +
       *(float *)(iVar27 + 0x84) * local_24c;
  *(float *)((int)local_274 + iVar28 + 0x1f8) =
       fVar56 * fVar55 + fVar54 * fVar52 + fVar40 * local_24c;
  *(float *)((int)local_274 + iVar28 + 0x1f0) =
       fVar42 * fVar52 + fVar44 * local_24c + fVar55 * fVar36;
  *(int *)((int)local_274 + iVar28 + 0x200) = local_274[0xf3] + 4;
  iVar28 = local_274[0xf2];
  *(undefined4 *)((int)local_274 + iVar28 + 0x2d8) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x2d4) = 0;
  *(undefined4 *)((int)local_274 + iVar28 + 0x2d0) = 0;
  *(float *)((int)local_274 + iVar28 + 0x2e0) = local_254 * -1.0;
  *(float *)((int)local_274 + iVar28 + 0x2e4) = local_250 * -1.0;
  fVar47 = local_24c * -1.0;
  *(float *)((int)local_274 + iVar28 + 0x2e8) = fVar47;
  iVar27 = *(int *)(iVar5 + 0x94);
  fVar52 = *(float *)((int)local_274 + iVar28 + 0x2e4);
  fVar54 = *(float *)(iVar27 + 0x78);
  fVar55 = *(float *)((int)local_274 + iVar28 + 0x2e0);
  fVar56 = *(float *)(iVar27 + 0x68);
  fVar36 = *(float *)(iVar27 + 0x88);
  fVar40 = *(float *)(iVar27 + 0x60);
  fVar42 = *(float *)(iVar27 + 0x70);
  fVar44 = *(float *)(iVar27 + 0x80);
  *(float *)((int)local_274 + iVar28 + 0x2e4) =
       *(float *)(iVar27 + 100) * fVar55 + *(float *)(iVar27 + 0x74) * fVar52 +
       *(float *)(iVar27 + 0x84) * fVar47;
  *(float *)((int)local_274 + iVar28 + 0x2e8) = fVar56 * fVar55 + fVar54 * fVar52 + fVar36 * fVar47;
  *(float *)((int)local_274 + iVar28 + 0x2e0) = fVar42 * fVar52 + fVar44 * fVar47 + fVar55 * fVar40;
  *(int *)((int)local_274 + iVar28 + 0x2f0) = local_274[0xf3] + 4;
  local_274[0xf3] = local_274[0xf3] + 5;
  local_274[0xf2] = local_274[0xf2] + 0xf0;
  local_244 = local_274 + 0xec;
  piVar1 = local_274 + 0xef;
  *local_244 = local_274[0xf3];
  *piVar1 = local_274[0xf3];
  piVar2 = local_274 + 0xc0;
  FUN_100a4e60(*(void **)(iVar4 + 0x94),local_244,piVar2,(int)pfVar30);
  iVar28 = local_274[0xf3];
  *local_244 = iVar28;
  *piVar1 = iVar28;
  FUN_100a4e60(*(void **)(iVar5 + 0x94),piVar1,piVar2,(int)pfVar30);
  local_190 = *(float *)((int)local_258 + 0x78) * param_3;
  local_1a0 = *(float *)((int)local_258 + 0x7c) * param_4 * -1.0;
  pfVar29 = (float *)*local_274;
  pfVar29[2] = local_158 - local_168;
  *pfVar29 = local_160 - local_170;
  pfVar29[1] = local_15c - local_16c;
  fVar52 = param_1[1];
  pfVar29[2] = (local_158 - local_168) * fVar52;
  *pfVar29 = *pfVar29 * fVar52;
  pfVar29[1] = (local_15c - local_16c) * fVar52;
  pfVar29[3] = (local_118 * local_24c + local_11c * local_250 + local_254 * local_120) * param_1[1];
  pfVar29[4] = -0.0 - (local_11c * local_264 + local_118 * local_260 + local_268 * local_120) *
                      param_1[1];
  fStack_19c = local_1a0;
  fStack_198 = local_1a0;
  fStack_194 = local_1a0;
  fStack_18c = local_190;
  fStack_188 = local_190;
  fStack_184 = local_190;
  FUN_100ba5f0((int)piVar2);
  pfVar29 = (float *)*local_274;
  fVar37 = *pfVar29 * local_190 + *pfVar30 * local_1a0;
  fVar41 = pfVar29[1] * fStack_18c + (float)local_274[0x19] * fStack_19c;
  fVar43 = pfVar29[2] * fStack_188 + (float)local_274[0x1a] * fStack_198;
  fStack_18c = pfVar29[3] * fStack_184 + (float)local_274[0x1b] * fStack_194;
  pfVar30 = (float *)*piVar2;
  fVar52 = pfVar30[9];
  fVar54 = pfVar30[10];
  fVar55 = pfVar30[0xb];
  fVar56 = pfVar30[0xc];
  fVar36 = pfVar30[0xd];
  fVar40 = pfVar30[0xe];
  fVar42 = pfVar30[0xf];
  pfVar8 = (float *)local_274[0xc];
  fVar44 = pfVar30[1];
  fVar47 = pfVar30[2];
  fVar48 = pfVar30[3];
  fVar53 = (float)local_274[0x1c] * local_1a0 + pfVar29[4] * local_190;
  fVar49 = pfVar30[4];
  fVar50 = pfVar30[5];
  fVar51 = pfVar30[6];
  fVar11 = pfVar30[7];
  local_1a0 = pfVar30[0x10] * fVar43;
  fStack_19c = pfVar30[0x11] * fVar43;
  fStack_198 = pfVar30[0x12] * fVar43;
  fStack_194 = pfVar30[0x13] * fVar43;
  fVar12 = pfVar30[0x14];
  fVar13 = pfVar30[0x15];
  fVar14 = pfVar30[0x16];
  fVar15 = pfVar30[0x17];
  fVar16 = pfVar30[0x19];
  fVar17 = pfVar30[0x1a];
  fVar18 = pfVar30[0x1b];
  fVar19 = pfVar30[0x1c];
  fVar20 = pfVar30[0x1d];
  fVar21 = pfVar30[0x1e];
  fVar22 = pfVar30[0x1f];
  fVar23 = pfVar30[0x21];
  fVar24 = pfVar30[0x22];
  fVar25 = pfVar30[0x23];
  *pfVar8 = *pfVar30 * fVar37 + 0.0 + pfVar30[8] * fVar41 + local_1a0 + pfVar30[0x18] * fStack_18c +
            pfVar30[0x20] * fVar53;
  pfVar8[1] = fVar44 * fVar37 + 0.0 + fVar52 * fVar41 + fStack_19c + fVar16 * fStack_18c +
              fVar23 * fVar53;
  pfVar8[2] = fVar47 * fVar37 + 0.0 + fVar54 * fVar41 + fStack_198 + fVar17 * fStack_18c +
              fVar24 * fVar53;
  pfVar8[3] = fVar48 * fVar37 + 0.0 + fVar55 * fVar41 + fStack_194 + fVar18 * fStack_18c +
              fVar25 * fVar53;
  iVar28 = local_274[0xc];
  fVar52 = pfVar30[0x25];
  fVar54 = pfVar30[0x26];
  fVar55 = pfVar30[0x27];
  *(float *)(iVar28 + 0x10) =
       pfVar30[0x24] * fVar53 +
       fVar49 * fVar37 + 0.0 + fVar56 * fVar41 + fVar12 * fVar43 + fVar19 * fStack_18c;
  *(float *)(iVar28 + 0x14) =
       fVar52 * fVar53 +
       fVar50 * fVar37 + 0.0 + fVar36 * fVar41 + fVar13 * fVar43 + fVar20 * fStack_18c;
  *(float *)(iVar28 + 0x18) =
       fVar54 * fVar53 +
       fVar51 * fVar37 + 0.0 + fVar40 * fVar41 + fVar14 * fVar43 + fVar21 * fStack_18c;
  *(float *)(iVar28 + 0x1c) =
       fVar55 * fVar53 +
       fVar11 * fVar37 + 0.0 + fVar42 * fVar41 + fVar15 * fVar43 + fVar22 * fStack_18c;
  local_190 = fStack_18c;
  fStack_188 = fStack_18c;
  fStack_184 = fStack_18c;
  FUN_100a53e0(*(void **)(iVar4 + 0x94),local_244,local_274[0xc]);
  FUN_100a53e0(*(void **)(iVar5 + 0x94),piVar1,local_274[0xc]);
  return 0x3d0;
}


// 1009bc30 FUN_1009bc30

void __thiscall FUN_1009bc30(void *this,float *param_1)

{
  float fVar1;
  
                    /* WARNING: Load size is inaccurate */
  *(float *)this = *param_1 + *this;
  *(undefined4 *)((int)this + 4) = *(undefined4 *)((int)this + 4);
  *(undefined4 *)((int)this + 8) = *(undefined4 *)((int)this + 8);
  *(undefined4 *)((int)this + 0xc) = *(undefined4 *)((int)this + 0xc);
  fVar1 = param_1[1];
  *(undefined4 *)((int)this + 0x10) = *(undefined4 *)((int)this + 0x10);
  *(float *)((int)this + 0x14) = fVar1 + *(float *)((int)this + 0x14);
  *(undefined4 *)((int)this + 0x18) = *(undefined4 *)((int)this + 0x18);
  *(undefined4 *)((int)this + 0x1c) = *(undefined4 *)((int)this + 0x1c);
  fVar1 = param_1[2];
  *(undefined4 *)((int)this + 0x20) = *(undefined4 *)((int)this + 0x20);
  *(undefined4 *)((int)this + 0x24) = *(undefined4 *)((int)this + 0x24);
  *(float *)((int)this + 0x28) = fVar1 + *(float *)((int)this + 0x28);
  *(undefined4 *)((int)this + 0x2c) = *(undefined4 *)((int)this + 0x2c);
  return;
}


// 1009bca0 FUN_1009bca0

void __thiscall FUN_1009bca0(void *this,undefined4 *param_1)

{
  int iVar1;
  uint uVar2;
  undefined4 *puVar3;
  undefined4 *puVar4;
  undefined4 local_70 [4];
  undefined4 local_60;
  undefined4 local_5c;
  undefined4 local_58;
  undefined4 local_50;
  undefined4 local_4c;
  undefined4 local_48;
  undefined4 local_40 [4];
  undefined4 local_30;
  undefined4 local_2c;
  undefined4 local_28;
  undefined4 local_20;
  undefined4 local_1c;
  undefined4 local_18;
  
  puVar3 = (undefined4 *)((int)this + 0x20);
  puVar4 = param_1;
  for (iVar1 = 0xc; iVar1 != 0; iVar1 = iVar1 + -1) {
    *puVar4 = *puVar3;
    puVar3 = puVar3 + 1;
    puVar4 = puVar4 + 1;
  }
  param_1[0xc] = *(undefined4 *)((int)this + 0x50);
  param_1[0xd] = *(undefined4 *)((int)this + 0x54);
  param_1[0xe] = *(undefined4 *)((int)this + 0x58);
  puVar3 = (undefined4 *)((int)this + 0x60);
  puVar4 = param_1 + 0x10;
  for (iVar1 = 0xc; iVar1 != 0; iVar1 = iVar1 + -1) {
    *puVar4 = *puVar3;
    puVar3 = puVar3 + 1;
    puVar4 = puVar4 + 1;
  }
  param_1[0x1c] = *(undefined4 *)((int)this + 0x90);
  param_1[0x1d] = *(undefined4 *)((int)this + 0x94);
  param_1[0x1e] = *(undefined4 *)((int)this + 0x98);
  puVar3 = (undefined4 *)((int)this + 0x20);
  puVar4 = local_70;
  for (iVar1 = 0xc; iVar1 != 0; iVar1 = iVar1 + -1) {
    *puVar4 = *puVar3;
    puVar3 = puVar3 + 1;
    puVar4 = puVar4 + 1;
  }
  puVar3 = (undefined4 *)((int)this + 0x60);
  puVar4 = local_40;
  for (iVar1 = 0xc; iVar1 != 0; iVar1 = iVar1 + -1) {
    *puVar4 = *puVar3;
    puVar3 = puVar3 + 1;
    puVar4 = puVar4 + 1;
  }
  uVar2 = (uint)*(byte *)((int)this + 0x144);
  param_1[uVar2 * 4] = local_70[0];
  param_1[uVar2 * 4 + 1] = local_70[1];
  param_1[uVar2 * 4 + 2] = local_70[2];
  iVar1 = *(byte *)((int)this + 0x144) + 4;
  param_1[iVar1 * 4] = local_40[0];
  param_1[iVar1 * 4 + 1] = local_40[1];
  param_1[iVar1 * 4 + 2] = local_40[2];
  uVar2 = (uint)*(byte *)((int)this + 0x146);
  param_1[uVar2 * 4] = local_60;
  param_1[uVar2 * 4 + 1] = local_5c;
  param_1[uVar2 * 4 + 2] = local_58;
  iVar1 = *(byte *)((int)this + 0x146) + 4;
  param_1[iVar1 * 4] = local_30;
  param_1[iVar1 * 4 + 1] = local_2c;
  param_1[iVar1 * 4 + 2] = local_28;
  puVar3 = param_1 + (uint)*(byte *)((int)this + 0x145) * 4;
  *puVar3 = local_50;
  puVar3[1] = local_4c;
  puVar3[2] = local_48;
  iVar1 = *(byte *)((int)this + 0x145) + 4;
  param_1[iVar1 * 4] = local_20;
  param_1[iVar1 * 4 + 1] = local_1c;
  param_1[iVar1 * 4 + 2] = local_18;
  param_1[0x20] = *(undefined4 *)((int)this + 0xf4);
  param_1[0x21] = *(undefined4 *)((int)this + 0xf8);
  param_1[0x22] = *(undefined4 *)((int)this + 0xfc);
  param_1[0x23] = *(undefined4 *)((int)this + 0x100);
  param_1[0x24] = *(undefined4 *)((int)this + 0x104);
  param_1[0x25] = *(undefined4 *)((int)this + 0x108);
  param_1[0x21] = -0.0 - *(float *)((int)this + 0xfc);
  param_1[0x22] = -0.0 - *(float *)((int)this + 0xf8);
  param_1[0x26] = *(undefined4 *)((int)this + 0x10c);
  param_1[0x27] = *(undefined4 *)((int)this + 0x110);
  param_1[0x28] = *(undefined4 *)((int)this + 0x114);
  param_1[0x29] = *(undefined4 *)((int)this + 0x118);
  param_1[0x2a] = *(undefined4 *)((int)this + 0x11c);
  param_1[0x2b] = *(undefined4 *)((int)this + 0x120);
  param_1[0x2c] = *(undefined4 *)((int)this + 0x124);
  param_1[0x2d] = *(undefined4 *)((int)this + 0x128);
  param_1[0x2e] = *(undefined4 *)((int)this + 300);
  param_1[0x2f] = *(undefined4 *)((int)this + 0x130);
  param_1[0x30] = *(undefined4 *)((int)this + 0x134);
  param_1[0x31] = *(undefined4 *)((int)this + 0x138);
  param_1[0x32] = *(undefined4 *)((int)this + 0x13c);
  param_1[0x33] = *(undefined4 *)((int)this + 0x140);
  *(undefined1 *)((int)param_1 + 0xd3) = *(undefined1 *)((int)this + 0x147);
  *(undefined1 *)(param_1 + 0x34) = *(undefined1 *)((int)this + 0x144);
  *(undefined1 *)((int)param_1 + 0xd1) = *(undefined1 *)((int)this + 0x145);
  *(undefined1 *)((int)param_1 + 0xd2) = *(undefined1 *)((int)this + 0x146);
  return;
}


// 1009bf90 FUN_1009bf90

void __thiscall FUN_1009bf90(void *this,int param_1,int param_2)

{
  undefined4 *puVar1;
  
  puVar1 = (undefined4 *)((uint)*(byte *)((int)this + 0x144) * 0x10 + param_1);
  *(undefined4 *)((int)this + 0x20) = *puVar1;
  *(undefined4 *)((int)this + 0x24) = puVar1[1];
  *(undefined4 *)((int)this + 0x28) = puVar1[2];
  puVar1 = (undefined4 *)((uint)*(byte *)((int)this + 0x144) * 0x10 + param_2);
  *(undefined4 *)((int)this + 0x60) = *puVar1;
  *(undefined4 *)((int)this + 100) = puVar1[1];
  *(undefined4 *)((int)this + 0x68) = puVar1[2];
  puVar1 = (undefined4 *)((uint)*(byte *)((int)this + 0x146) * 0x10 + param_1);
  *(undefined4 *)((int)this + 0x30) = *puVar1;
  *(undefined4 *)((int)this + 0x34) = puVar1[1];
  *(undefined4 *)((int)this + 0x38) = puVar1[2];
  puVar1 = (undefined4 *)((uint)*(byte *)((int)this + 0x146) * 0x10 + param_2);
  *(undefined4 *)((int)this + 0x70) = *puVar1;
  *(undefined4 *)((int)this + 0x74) = puVar1[1];
  *(undefined4 *)((int)this + 0x78) = puVar1[2];
  puVar1 = (undefined4 *)((uint)*(byte *)((int)this + 0x145) * 0x10 + param_1);
  *(undefined4 *)((int)this + 0x40) = *puVar1;
  *(undefined4 *)((int)this + 0x44) = puVar1[1];
  *(undefined4 *)((int)this + 0x48) = puVar1[2];
  puVar1 = (undefined4 *)((uint)*(byte *)((int)this + 0x145) * 0x10 + param_2);
  *(undefined4 *)((int)this + 0x80) = *puVar1;
  *(undefined4 *)((int)this + 0x84) = puVar1[1];
  *(undefined4 *)((int)this + 0x88) = puVar1[2];
  return;
}


// 1009c060 FUN_1009c060

void __thiscall FUN_1009c060(void *this,float param_1)

{
  float *pfVar1;
  float *pfVar2;
  int iVar3;
  
  pfVar2 = (float *)((int)this + 0x104);
  pfVar1 = (float *)((int)this + 0xb0);
  iVar3 = 3;
  do {
    *(bool *)((int)pfVar1 + -0xf) = param_1 != 0.0;
    *pfVar1 = ABS(param_1);
    pfVar1[1] = 0.0;
    *(bool *)((int)pfVar2 + -0xf) = param_1 != 0.0;
    *pfVar2 = ABS(param_1);
    pfVar2[1] = 0.0;
    pfVar1 = pfVar1 + 7;
    pfVar2 = pfVar2 + 6;
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  return;
}


// 1009c0f0 FUN_1009c0f0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __thiscall FUN_1009c0f0(void *this,float *param_1,int param_2)

{
  int iVar1;
  ushort *puVar2;
  ushort *puVar3;
  undefined4 uVar4;
  undefined4 uVar5;
  undefined4 uVar6;
  int iVar7;
  int iVar8;
  uint uVar9;
  uint uVar10;
  uint uVar11;
  uint uVar12;
  float fVar13;
  float fVar14;
  float fVar24;
  float fVar25;
  float fVar26;
  undefined1 auVar15 [16];
  undefined1 auVar16 [16];
  undefined1 auVar17 [16];
  undefined1 auVar18 [16];
  undefined1 auVar19 [16];
  undefined1 auVar20 [16];
  undefined1 auVar21 [16];
  undefined1 auVar22 [16];
  undefined1 auVar23 [16];
  float fVar27;
  undefined1 auVar28 [16];
  float fVar35;
  float fVar36;
  undefined1 auVar29 [16];
  undefined1 auVar30 [16];
  undefined1 auVar31 [16];
  undefined1 auVar32 [16];
  undefined1 auVar33 [16];
  undefined1 auVar34 [16];
  float fVar37;
  float fVar38;
  float fVar46;
  float fVar47;
  float fVar48;
  float fVar49;
  undefined1 auVar39 [16];
  undefined1 auVar40 [16];
  undefined1 auVar41 [16];
  undefined1 auVar42 [16];
  undefined1 auVar43 [16];
  undefined1 auVar44 [16];
  undefined1 auVar45 [16];
  float fVar50;
  float fVar51;
  float fVar56;
  float fVar58;
  undefined1 auVar52 [16];
  undefined1 auVar53 [16];
  float fVar57;
  undefined1 auVar54 [16];
  undefined1 auVar55 [16];
  float fVar61;
  undefined1 auVar59 [16];
  undefined1 auVar60 [16];
  undefined1 auVar62 [16];
  undefined1 auVar63 [16];
  float local_80;
  float fStack_7c;
  float fStack_78;
  float fStack_74;
  
  iVar8 = *(int *)((int)this + 8);
  fVar38 = *(float *)(param_2 + 0x2d0);
  fVar47 = *(float *)(param_2 + 0x2d4);
  fVar49 = *(float *)(param_2 + 0x2d8);
  fVar50 = *(float *)(param_2 + 0x2dc);
  fStack_74 = *(float *)(param_2 + 0x100);
  iVar1 = *(int *)((int)this + 4);
  puVar2 = *(ushort **)(iVar1 + 0x94);
  puVar3 = *(ushort **)(iVar8 + 0x94);
  fVar61 = fVar50 * fVar47;
  if (*(int *)(param_2 + 0x1c0) == 1) {
    fVar13 = *(float *)(puVar3 + 0x62) * *(float *)(param_2 + 0x164) +
             *(float *)(puVar3 + 0x60) * *(float *)(param_2 + 0x160) +
             *(float *)(puVar3 + 100) * *(float *)(param_2 + 0x168);
    fVar27 = *(float *)(puVar2 + 0x62) * *(float *)(param_2 + 0x154) +
             *(float *)(puVar2 + 0x60) * *(float *)(param_2 + 0x150) +
             *(float *)(puVar2 + 100) * *(float *)(param_2 + 0x158);
    fVar37 = fVar27 + fVar13;
    fVar46 = fVar27 + fVar13;
    fVar48 = fVar27 + fVar13;
    fVar27 = fVar27 + fVar13;
LAB_1009c3ac:
    fVar13 = *(float *)((int)this + 0xb8);
    fVar24 = 0.0;
    fVar25 = 0.0;
    fVar26 = 0.0;
    fVar36 = 0.0;
    fVar51 = *param_1;
    fVar35 = *(float *)((int)this + 0xb0);
    fVar57 = param_1[1];
    local_80 = fStack_74;
    fStack_7c = fStack_74;
    fStack_78 = fStack_74;
    if (*(char *)((int)this + 0xa1) != '\0') {
      fVar14 = fVar13 - fStack_74;
      fVar24 = fVar13 - fStack_74;
      fVar25 = fVar13 - fStack_74;
      fVar26 = fVar13 - fStack_74;
      local_80 = (float)(~-(uint)(3.1415927 < fVar14) & -(uint)(fVar14 < -3.1415927) & 0xc0c90fdb |
                        -(uint)(3.1415927 < fVar14) & 0x40c90fdb);
      fVar36 = (float)(~-(uint)(3.1415927 < fVar24) & -(uint)(fVar24 < -3.1415927) & 0xc0c90fdb |
                      -(uint)(3.1415927 < fVar24) & 0x40c90fdb);
      fVar56 = (float)(~-(uint)(3.1415927 < fVar25) & -(uint)(fVar25 < -3.1415927) & 0xc0c90fdb |
                      -(uint)(3.1415927 < fVar25) & 0x40c90fdb);
      fVar58 = (float)(~-(uint)(3.1415927 < fVar26) & -(uint)(fVar26 < -3.1415927) & 0xc0c90fdb |
                      -(uint)(3.1415927 < fVar26) & 0x40c90fdb);
      fVar14 = fVar14 - local_80;
      local_80 = local_80 + fStack_74;
      fStack_7c = fVar36 + fStack_74;
      fStack_78 = fVar56 + fStack_74;
      fStack_74 = fVar58 + fStack_74;
      auVar28 = *(undefined1 (*) [16])(param_2 + 0x1a0);
      auVar63._0_4_ = (fVar50 * fVar38 * 0.8 * fVar14 * fVar57 - fVar61 * fVar37) * auVar28._0_4_;
      auVar63._4_4_ = (fVar61 * 0.8 * (fVar24 - fVar36) * 0.0 - fVar61 * fVar46) * auVar28._4_4_;
      auVar63._8_4_ =
           (fVar50 * fVar49 * 0.8 * (fVar25 - fVar56) * 0.0 - fVar61 * fVar48) * auVar28._8_4_;
      auVar63._12_4_ =
           (fVar50 * fVar50 * 0.8 * (fVar26 - fVar58) * 0.0 - fVar61 * fVar27) * auVar28._12_4_;
      auVar28 = rcpps(auVar28,auVar63);
      fVar24 = auVar28._0_4_;
      fVar25 = auVar28._4_4_;
      fVar26 = auVar28._8_4_;
      fVar36 = auVar28._12_4_;
      fVar24 = ABS((fVar24 + fVar24) - fVar24 * fVar24 * auVar63._0_4_) * fVar35 * fVar51;
      fVar25 = ABS((fVar25 + fVar25) - fVar25 * fVar25 * auVar63._4_4_) * fVar35 * 0.0;
      fVar26 = ABS((fVar26 + fVar26) - fVar26 * fVar26 * auVar63._8_4_) * fVar35 * 0.0;
      fVar35 = ABS((fVar36 + fVar36) - fVar36 * fVar36 * auVar63._12_4_) * fVar35 * 0.0;
      uVar9 = -(uint)(fVar24 < 1.0);
      uVar10 = -(uint)(fVar25 < 1.0);
      uVar11 = -(uint)(fVar26 < 1.0);
      uVar12 = -(uint)(fVar35 < 1.0);
      fVar56 = (float)(uVar9 & (uint)fVar24 | ~uVar9 & 0x3f800000);
      fVar24 = fVar56 * auVar63._0_4_;
      fVar25 = (float)(uVar10 & (uint)fVar25 | ~uVar10 & 0x3f800000) * auVar63._4_4_;
      fVar26 = (float)(uVar11 & (uint)fVar26 | ~uVar11 & 0x3f800000) * auVar63._8_4_;
      fVar36 = (float)(uVar12 & (uint)fVar35 | ~uVar12 & 0x3f800000) * auVar63._12_4_;
      *(float *)((int)this + 0xb8) =
           (*(float *)((int)this + 0xb4) * fVar51 + fVar13) - (fVar14 - fVar56 * fVar14);
    }
    fVar13 = *(float *)((int)this + 0xac);
    fVar35 = *(float *)((int)this + 0xa8);
    fVar14 = *(float *)((int)this + 0xa4);
    local_80 = (*(float *)(param_2 + 400) * fVar24 + fVar37) * fVar51 + local_80;
    fStack_7c = (*(float *)(param_2 + 0x194) * fVar25 + fVar46) * 0.0 + fStack_7c;
    fStack_78 = (*(float *)(param_2 + 0x198) * fVar26 + fVar48) * 0.0 + fStack_78;
    fStack_74 = (*(float *)(param_2 + 0x19c) * fVar36 + fVar27) * 0.0 + fStack_74;
    fVar46 = fVar13 * fVar50 * fVar38 * *(float *)(param_2 + 0x1a0) * fVar57;
    fVar61 = fVar13 * fVar61 * *(float *)(param_2 + 0x1a4) * 0.0;
    fVar48 = fVar13 * fVar50 * fVar49 * *(float *)(param_2 + 0x1a8) * 0.0;
    fVar27 = fVar13 * fVar50 * fVar50 * *(float *)(param_2 + 0x1ac) * 0.0;
    auVar62._0_4_ = (local_80 - fVar14) * fVar46;
    auVar62._4_4_ = (fStack_7c - fVar14) * fVar61;
    auVar62._8_4_ = (fStack_78 - fVar14) * fVar48;
    auVar62._12_4_ = (fStack_74 - fVar14) * fVar27;
    auVar15._0_4_ = (local_80 - fVar35) * fVar46;
    auVar15._4_4_ = (fStack_7c - fVar35) * fVar61;
    auVar15._8_4_ = (fStack_78 - fVar35) * fVar48;
    auVar15._12_4_ = (fStack_74 - fVar35) * fVar27;
    auVar28 = maxps(auVar15,ZEXT816(0));
    auVar63 = minps(auVar62,ZEXT816(0));
    fVar61 = *(float *)(param_2 + 0x174);
    fVar46 = *(float *)(param_2 + 0x178);
    fVar24 = fVar24 - (float)((uint)(auVar63._0_4_ + auVar28._0_4_) & *(uint *)(param_2 + 0x1b0));
    *(float *)(puVar2 + 0x60) = *(float *)(param_2 + 0x170) * fVar24 + *(float *)(puVar2 + 0x60);
    *(float *)(puVar2 + 0x62) = fVar61 * fVar24 + *(float *)(puVar2 + 0x62);
    *(float *)(puVar2 + 100) = fVar46 * fVar24 + *(float *)(puVar2 + 100);
    puVar2[0x66] = 0;
    puVar2[0x67] = 0;
    fVar61 = *(float *)(param_2 + 0x184);
    fVar46 = *(float *)(param_2 + 0x188);
    *(float *)(puVar3 + 0x60) = *(float *)(param_2 + 0x180) * fVar24 + *(float *)(puVar3 + 0x60);
    *(float *)(puVar3 + 0x62) = fVar61 * fVar24 + *(float *)(puVar3 + 0x62);
    *(float *)(puVar3 + 100) = fVar46 * fVar24 + *(float *)(puVar3 + 100);
    puVar3[0x66] = 0;
    puVar3[0x67] = 0;
  }
  else if (*(int *)(param_2 + 0x1c0) != 2) {
    fVar46 = *(float *)(param_2 + 0x110);
    fVar48 = *(float *)(param_2 + 0x114);
    fVar27 = *(float *)(param_2 + 0x118);
    *(undefined4 *)(param_2 + 0x1c0) = 1;
    fVar13 = fVar27 * *(float *)(puVar2 + 0x40) + fVar48 * *(float *)(puVar2 + 0x38) +
             fVar46 * *(float *)(puVar2 + 0x30);
    fVar51 = fVar27 * *(float *)(puVar2 + 0x42) + fVar48 * *(float *)(puVar2 + 0x3a) +
             fVar46 * *(float *)(puVar2 + 0x32);
    fVar35 = fVar27 * *(float *)(puVar2 + 0x44) + fVar48 * *(float *)(puVar2 + 0x3c) +
             fVar46 * *(float *)(puVar2 + 0x34);
    fVar57 = fVar27 * *(float *)(puVar2 + 0x46) + fVar48 * *(float *)(puVar2 + 0x3e) +
             fVar46 * *(float *)(puVar2 + 0x36);
    fVar46 = 0.0 - fVar46;
    fVar48 = 0.0 - fVar48;
    fVar27 = 0.0 - fVar27;
    fVar24 = fVar27 * *(float *)(puVar3 + 0x40) + fVar48 * *(float *)(puVar3 + 0x38) +
             fVar46 * *(float *)(puVar3 + 0x30);
    fVar25 = fVar27 * *(float *)(puVar3 + 0x42) + fVar48 * *(float *)(puVar3 + 0x3a) +
             fVar46 * *(float *)(puVar3 + 0x32);
    fVar26 = fVar27 * *(float *)(puVar3 + 0x44) + fVar48 * *(float *)(puVar3 + 0x3c) +
             fVar46 * *(float *)(puVar3 + 0x34);
    fVar36 = fVar27 * *(float *)(puVar3 + 0x46) + fVar48 * *(float *)(puVar3 + 0x3e) +
             fVar46 * *(float *)(puVar3 + 0x36);
    *(float *)(param_2 + 0x150) = fVar13;
    *(float *)(param_2 + 0x154) = fVar51;
    *(float *)(param_2 + 0x158) = fVar35;
    *(float *)(param_2 + 0x15c) = fVar57;
    *(float *)(param_2 + 0x160) = fVar24;
    *(float *)(param_2 + 0x164) = fVar25;
    *(float *)(param_2 + 0x168) = fVar26;
    *(float *)(param_2 + 0x16c) = fVar36;
    auVar28 = _DAT_100bf720;
    fVar37 = DAT_100bf720;
    fVar46 = DAT_100bf720._4_4_;
    fVar48 = DAT_100bf720._8_4_;
    fVar27 = DAT_100bf720._12_4_;
    if ((*puVar2 & 0x12) == 0) {
      fVar37 = *(float *)(puVar2 + 100) * fVar35 + *(float *)(puVar2 + 0x62) * fVar51 +
               *(float *)(puVar2 + 0x60) * fVar13;
      fVar46 = *(float *)(puVar2 + 0x18);
      fVar48 = *(float *)(puVar2 + 0x1a);
      fVar27 = *(float *)(puVar2 + 0x1c);
      fVar14 = *(float *)(puVar2 + 0x1e);
      *(float *)(param_2 + 0x170) = fVar46 * fVar13;
      *(float *)(param_2 + 0x174) = fVar48 * fVar51;
      *(float *)(param_2 + 0x178) = fVar27 * fVar35;
      *(float *)(param_2 + 0x17c) = fVar14 * fVar57;
      fVar46 = fVar27 * fVar35 * fVar35 + fVar48 * fVar51 * fVar51 + fVar46 * fVar13 * fVar13;
      auVar28._4_4_ = fVar46;
      auVar28._0_4_ = fVar46;
      auVar28._8_4_ = fVar46;
      auVar28._12_4_ = fVar46;
      fVar46 = fVar37;
      fVar48 = fVar37;
      fVar27 = fVar37;
    }
    else {
      *(undefined1 (*) [16])(param_2 + 0x170) = _DAT_100bf720;
    }
    auVar63 = _DAT_100bf720;
    if ((*puVar3 & 0x12) == 0) {
      fVar14 = *(float *)(puVar3 + 100) * fVar26 + *(float *)(puVar3 + 0x62) * fVar25 +
               *(float *)(puVar3 + 0x60) * fVar24;
      fVar13 = *(float *)(puVar3 + 0x18);
      fVar51 = *(float *)(puVar3 + 0x1a);
      fVar35 = *(float *)(puVar3 + 0x1c);
      fVar57 = *(float *)(puVar3 + 0x1e);
      *(float *)(param_2 + 0x180) = fVar13 * fVar24;
      *(float *)(param_2 + 0x184) = fVar51 * fVar25;
      *(float *)(param_2 + 0x188) = fVar35 * fVar26;
      *(float *)(param_2 + 0x18c) = fVar57 * fVar36;
      fVar13 = fVar35 * fVar26 * fVar26 + fVar51 * fVar25 * fVar25 + fVar13 * fVar24 * fVar24;
      auVar63 = ZEXT416((uint)fVar13);
      fVar37 = fVar14 + fVar37;
      fVar46 = fVar14 + fVar46;
      fVar48 = fVar14 + fVar48;
      fVar27 = fVar14 + fVar27;
      auVar39._4_4_ = fVar13 + auVar28._4_4_;
      auVar39._0_4_ = fVar13 + auVar28._0_4_;
      auVar39._8_4_ = fVar13 + auVar28._8_4_;
      auVar39._12_4_ = fVar13 + auVar28._12_4_;
      auVar28 = auVar39;
    }
    else {
      *(undefined1 (*) [16])(param_2 + 0x180) = _DAT_100bf720;
    }
    auVar63 = rcpps(auVar63,auVar28);
    fVar13 = auVar63._0_4_;
    fVar51 = auVar63._4_4_;
    fVar35 = auVar63._8_4_;
    fVar57 = auVar63._12_4_;
    *(undefined1 (*) [16])(param_2 + 400) = auVar28;
    *(uint *)(param_2 + 0x1a0) =
         (uint)((fVar13 + fVar13) - fVar13 * fVar13 * auVar28._0_4_) &
         -(uint)(1.1920929e-07 < auVar28._0_4_);
    *(uint *)(param_2 + 0x1a4) =
         (uint)((fVar51 + fVar51) - fVar51 * fVar51 * auVar28._4_4_) &
         -(uint)(1.1920929e-07 < auVar28._4_4_);
    *(uint *)(param_2 + 0x1a8) =
         (uint)((fVar35 + fVar35) - fVar35 * fVar35 * auVar28._8_4_) &
         -(uint)(1.1920929e-07 < auVar28._8_4_);
    *(uint *)(param_2 + 0x1ac) =
         (uint)((fVar57 + fVar57) - fVar57 * fVar57 * auVar28._12_4_) &
         -(uint)(1.1920929e-07 < auVar28._12_4_);
    iVar7 = (*(byte *)((int)this + 0xa0) & 1) * 0x10;
    uVar4 = *(undefined4 *)(&UNK_100ce724 + iVar7);
    uVar5 = *(undefined4 *)(&UNK_100ce728 + iVar7);
    uVar6 = *(undefined4 *)(&UNK_100ce72c + iVar7);
    *(undefined4 *)(param_2 + 0x1b0) = *(undefined4 *)(&DAT_100ce720 + iVar7);
    *(undefined4 *)(param_2 + 0x1b4) = uVar4;
    *(undefined4 *)(param_2 + 0x1b8) = uVar5;
    *(undefined4 *)(param_2 + 0x1bc) = uVar6;
    goto LAB_1009c3ac;
  }
  puVar2 = *(ushort **)(iVar8 + 0x94);
  iVar7 = *(int *)(param_2 + 0x240);
  fVar61 = *(float *)(param_2 + 0x108);
  puVar3 = *(ushort **)(iVar1 + 0x94);
  if (iVar7 == 1) {
    fVar46 = *(float *)(puVar2 + 100) * *(float *)(param_2 + 0x1e8) +
             *(float *)(puVar2 + 0x62) * *(float *)(param_2 + 0x1e4) +
             *(float *)(puVar2 + 0x60) * *(float *)(param_2 + 0x1e0);
    fVar48 = *(float *)(puVar3 + 100) * *(float *)(param_2 + 0x1d8) +
             *(float *)(puVar3 + 0x62) * *(float *)(param_2 + 0x1d4) +
             *(float *)(puVar3 + 0x60) * *(float *)(param_2 + 0x1d0);
    auVar40._0_4_ = fVar48 + fVar46;
    auVar40._4_4_ = fVar48 + fVar46;
    auVar40._8_4_ = fVar48 + fVar46;
    auVar40._12_4_ = fVar48 + fVar46;
LAB_1009c942:
    fVar46 = *(float *)((int)this + 0xf0);
    fVar48 = *param_1;
    fVar27 = param_1[1];
    fVar51 = 0.0;
    fVar35 = 0.0;
    fVar57 = 0.0;
    fVar24 = 0.0;
    fVar13 = *(float *)((int)this + 0xe8);
    if (*(char *)((int)this + 0xd9) != '\0') {
      fVar25 = fVar61 - fVar46;
      auVar53._0_4_ = fVar47 * auVar40._0_4_;
      auVar53._4_4_ = fVar47 * auVar40._4_4_;
      auVar53._8_4_ = fVar47 * auVar40._8_4_;
      auVar53._12_4_ = fVar47 * auVar40._12_4_;
      auVar18._0_4_ = (fVar38 * 0.8 * fVar25 * fVar27 - auVar53._0_4_) * *(float *)(param_2 + 0x220)
      ;
      auVar18._4_4_ =
           (fVar47 * 0.8 * (fVar61 - fVar46) * 0.0 - auVar53._4_4_) * *(float *)(param_2 + 0x224);
      auVar18._8_4_ =
           (fVar49 * 0.8 * (fVar61 - fVar46) * 0.0 - auVar53._8_4_) * *(float *)(param_2 + 0x228);
      auVar18._12_4_ =
           (fVar50 * 0.8 * (fVar61 - fVar46) * 0.0 - auVar53._12_4_) * *(float *)(param_2 + 0x22c);
      auVar28 = maxps(auVar18,_DAT_100bf800);
      auVar42._0_8_ = auVar28._0_8_ & 0x7fffffff7fffffff;
      auVar42._8_4_ = ABS(auVar28._8_4_);
      auVar42._12_4_ = ABS(auVar28._12_4_);
      auVar63 = rcpps(auVar53,auVar42);
      fVar51 = auVar63._0_4_;
      fVar35 = auVar63._4_4_;
      fVar57 = auVar63._8_4_;
      fVar24 = auVar63._12_4_;
      auVar59._0_4_ = ((fVar51 + fVar51) - fVar51 * fVar51 * ABS(auVar28._0_4_)) * fVar13 * fVar48;
      auVar59._4_4_ = ((fVar35 + fVar35) - fVar35 * fVar35 * ABS(auVar28._4_4_)) * fVar13 * 0.0;
      auVar59._8_4_ = ((fVar57 + fVar57) - fVar57 * fVar57 * auVar42._8_4_) * fVar13 * 0.0;
      auVar59._12_4_ = ((fVar24 + fVar24) - fVar24 * fVar24 * auVar42._12_4_) * fVar13 * 0.0;
      auVar63 = minps(_DAT_100bf730,auVar59);
      fVar51 = auVar63._0_4_ * auVar28._0_4_;
      fVar35 = auVar63._4_4_ * auVar28._4_4_;
      fVar57 = auVar63._8_4_ * auVar28._8_4_;
      fVar24 = auVar63._12_4_ * auVar28._12_4_;
      *(float *)((int)this + 0xf0) =
           *(float *)((int)this + 0xec) * fVar48 + fVar46 + (fVar25 - auVar63._0_4_ * fVar25);
    }
    fVar46 = *(float *)((int)this + 0xe0);
    fVar13 = *(float *)((int)this + 0xe4);
    fVar25 = *(float *)((int)this + 0xdc);
    fVar48 = fVar61 - (*(float *)(param_2 + 0x210) * fVar51 + auVar40._0_4_) * fVar48;
    fVar35 = fVar61 - (*(float *)(param_2 + 0x214) * fVar35 + auVar40._4_4_) * 0.0;
    fVar57 = fVar61 - (*(float *)(param_2 + 0x218) * fVar57 + auVar40._8_4_) * 0.0;
    fVar61 = fVar61 - (*(float *)(param_2 + 0x21c) * fVar24 + auVar40._12_4_) * 0.0;
    fVar27 = fVar13 * fVar38 * *(float *)(param_2 + 0x220) * fVar27;
    fVar24 = fVar13 * fVar47 * *(float *)(param_2 + 0x224) * 0.0;
    fVar26 = fVar13 * fVar49 * *(float *)(param_2 + 0x228) * 0.0;
    fVar13 = fVar13 * fVar50 * *(float *)(param_2 + 0x22c) * 0.0;
    auVar19._0_4_ = (fVar48 - fVar25) * fVar27;
    auVar19._4_4_ = (fVar35 - fVar25) * fVar24;
    auVar19._8_4_ = (fVar57 - fVar25) * fVar26;
    auVar19._12_4_ = (fVar61 - fVar25) * fVar13;
    auVar28 = minps(auVar19,_DAT_100bf720);
    auVar31._0_4_ = (fVar48 - fVar46) * fVar27;
    auVar31._4_4_ = (fVar35 - fVar46) * fVar24;
    auVar31._8_4_ = (fVar57 - fVar46) * fVar26;
    auVar31._12_4_ = (fVar61 - fVar46) * fVar13;
    auVar63 = maxps(auVar31,_DAT_100bf720);
    fVar61 = *(float *)(param_2 + 500);
    fVar46 = *(float *)(param_2 + 0x1f8);
    fVar51 = (float)((uint)(auVar28._0_4_ + auVar63._0_4_) & *(uint *)(param_2 + 0x230)) + fVar51;
    *(float *)(puVar3 + 0x60) = *(float *)(param_2 + 0x1f0) * fVar51 + *(float *)(puVar3 + 0x60);
    *(float *)(puVar3 + 0x62) = fVar61 * fVar51 + *(float *)(puVar3 + 0x62);
    *(float *)(puVar3 + 100) = fVar46 * fVar51 + *(float *)(puVar3 + 100);
    puVar3[0x66] = 0;
    puVar3[0x67] = 0;
    fVar61 = *(float *)(param_2 + 0x204);
    fVar46 = *(float *)(param_2 + 0x208);
    *(float *)(puVar2 + 0x60) = *(float *)(param_2 + 0x200) * fVar51 + *(float *)(puVar2 + 0x60);
    *(float *)(puVar2 + 0x62) = fVar61 * fVar51 + *(float *)(puVar2 + 0x62);
    *(float *)(puVar2 + 100) = fVar46 * fVar51 + *(float *)(puVar2 + 100);
    puVar2[0x66] = 0;
    puVar2[0x67] = 0;
  }
  else if (iVar7 != 2) {
    fVar46 = *(float *)(param_2 + 0x130);
    fVar48 = *(float *)(param_2 + 0x140);
    fVar27 = *(float *)(param_2 + 0x134) * *(float *)(param_2 + 0x148) -
             *(float *)(param_2 + 0x138) * *(float *)(param_2 + 0x144);
    fVar13 = *(float *)(param_2 + 0x138) * fVar48 - fVar46 * *(float *)(param_2 + 0x148);
    fVar51 = fVar46 * *(float *)(param_2 + 0x144) - *(float *)(param_2 + 0x134) * fVar48;
    fVar46 = fVar46 * fVar48 - fVar46 * fVar48;
    auVar16._0_4_ = fVar46 * fVar46 + fVar51 * fVar51 + fVar13 * fVar13 + fVar27 * fVar27;
    auVar16._4_4_ = auVar16._0_4_;
    auVar16._8_4_ = auVar16._0_4_;
    auVar16._12_4_ = auVar16._0_4_;
    auVar41._4_4_ = -(uint)(auVar16._0_4_ <= 1.1920929e-07);
    auVar41._0_4_ = -(uint)(auVar16._0_4_ <= 1.1920929e-07);
    auVar41._8_4_ = -(uint)(auVar16._0_4_ <= 1.1920929e-07);
    auVar41._12_4_ = -(uint)(auVar16._0_4_ <= 1.1920929e-07);
    iVar7 = movmskps(iVar7,auVar41);
    if (iVar7 == 0) {
      auVar28 = rsqrtps(auVar41,auVar16);
      fVar46 = auVar28._0_4_;
      fVar48 = auVar28._4_4_;
      fVar35 = auVar28._8_4_;
      fVar57 = auVar28._12_4_;
      auVar17._0_4_ = (3.0 - fVar46 * fVar46 * auVar16._0_4_) * fVar46 * 0.5 * 0.1;
      auVar17._4_4_ = (3.0 - fVar48 * fVar48 * auVar16._0_4_) * fVar48 * 0.5 * 0.1;
      auVar17._8_4_ = (3.0 - fVar35 * fVar35 * auVar16._0_4_) * fVar35 * 0.5 * 0.1;
      auVar17._12_4_ = (3.0 - fVar57 * fVar57 * auVar16._0_4_) * fVar57 * 0.5 * 0.1;
      auVar28 = maxps(auVar17,_DAT_100bf730);
      fVar27 = auVar28._0_4_ * fVar27;
      fVar13 = auVar28._4_4_ * fVar13;
      fVar51 = auVar28._8_4_ * fVar51;
      *(undefined4 *)(param_2 + 0x240) = 1;
      fVar57 = fVar51 * *(float *)(puVar3 + 0x40) + fVar13 * *(float *)(puVar3 + 0x38) +
               fVar27 * *(float *)(puVar3 + 0x30);
      fVar24 = fVar51 * *(float *)(puVar3 + 0x42) + fVar13 * *(float *)(puVar3 + 0x3a) +
               fVar27 * *(float *)(puVar3 + 0x32);
      fVar25 = fVar51 * *(float *)(puVar3 + 0x44) + fVar13 * *(float *)(puVar3 + 0x3c) +
               fVar27 * *(float *)(puVar3 + 0x34);
      fVar26 = fVar51 * *(float *)(puVar3 + 0x46) + fVar13 * *(float *)(puVar3 + 0x3e) +
               fVar27 * *(float *)(puVar3 + 0x36);
      fVar27 = 0.0 - fVar27;
      fVar13 = 0.0 - fVar13;
      fVar51 = 0.0 - fVar51;
      fVar46 = fVar51 * *(float *)(puVar2 + 0x40) + fVar13 * *(float *)(puVar2 + 0x38) +
               fVar27 * *(float *)(puVar2 + 0x30);
      fVar48 = fVar51 * *(float *)(puVar2 + 0x42) + fVar13 * *(float *)(puVar2 + 0x3a) +
               fVar27 * *(float *)(puVar2 + 0x32);
      fVar35 = fVar51 * *(float *)(puVar2 + 0x44) + fVar13 * *(float *)(puVar2 + 0x3c) +
               fVar27 * *(float *)(puVar2 + 0x34);
      fVar27 = fVar51 * *(float *)(puVar2 + 0x46) + fVar13 * *(float *)(puVar2 + 0x3e) +
               fVar27 * *(float *)(puVar2 + 0x36);
      *(float *)(param_2 + 0x1d0) = fVar57;
      *(float *)(param_2 + 0x1d4) = fVar24;
      *(float *)(param_2 + 0x1d8) = fVar25;
      *(float *)(param_2 + 0x1dc) = fVar26;
      *(float *)(param_2 + 0x1e0) = fVar46;
      *(float *)(param_2 + 0x1e4) = fVar48;
      *(float *)(param_2 + 0x1e8) = fVar35;
      *(float *)(param_2 + 0x1ec) = fVar27;
      auVar29 = _DAT_100bf720;
      if ((*puVar3 & 0x12) == 0) {
        fVar14 = *(float *)(puVar3 + 100) * fVar25 + *(float *)(puVar3 + 0x62) * fVar24 +
                 *(float *)(puVar3 + 0x60) * fVar57;
        fVar13 = *(float *)(puVar3 + 0x18);
        fVar51 = *(float *)(puVar3 + 0x1a);
        fVar36 = *(float *)(puVar3 + 0x1c);
        fVar37 = *(float *)(puVar3 + 0x1e);
        *(float *)(param_2 + 0x1f0) = fVar13 * fVar57;
        *(float *)(param_2 + 500) = fVar51 * fVar24;
        *(float *)(param_2 + 0x1f8) = fVar36 * fVar25;
        *(float *)(param_2 + 0x1fc) = fVar37 * fVar26;
        auVar29._0_4_ =
             fVar36 * fVar25 * fVar25 + fVar51 * fVar24 * fVar24 + fVar13 * fVar57 * fVar57;
        auVar40._4_4_ = fVar14;
        auVar40._0_4_ = fVar14;
        auVar40._8_4_ = fVar14;
        auVar40._12_4_ = fVar14;
        auVar29._4_4_ = auVar29._0_4_;
        auVar29._8_4_ = auVar29._0_4_;
        auVar29._12_4_ = auVar29._0_4_;
      }
      else {
        *(undefined1 (*) [16])(param_2 + 0x1f0) = _DAT_100bf720;
        auVar40 = auVar29;
      }
      auVar28 = _DAT_100bf720;
      if ((*puVar2 & 0x12) == 0) {
        fVar25 = *(float *)(puVar2 + 100) * fVar35 + *(float *)(puVar2 + 0x62) * fVar48 +
                 *(float *)(puVar2 + 0x60) * fVar46;
        fVar13 = *(float *)(puVar2 + 0x18);
        fVar51 = *(float *)(puVar2 + 0x1a);
        fVar57 = *(float *)(puVar2 + 0x1c);
        fVar24 = *(float *)(puVar2 + 0x1e);
        *(float *)(param_2 + 0x200) = fVar13 * fVar46;
        *(float *)(param_2 + 0x204) = fVar51 * fVar48;
        *(float *)(param_2 + 0x208) = fVar57 * fVar35;
        *(float *)(param_2 + 0x20c) = fVar24 * fVar27;
        fVar46 = fVar57 * fVar35 * fVar35 + fVar51 * fVar48 * fVar48 + fVar13 * fVar46 * fVar46;
        auVar28 = ZEXT416((uint)fVar46);
        auVar52._4_4_ = fVar25 + auVar40._4_4_;
        auVar52._0_4_ = fVar25 + auVar40._0_4_;
        auVar30._4_4_ = fVar46 + auVar29._4_4_;
        auVar30._0_4_ = fVar46 + auVar29._0_4_;
        auVar52._8_4_ = fVar25 + auVar40._8_4_;
        auVar52._12_4_ = fVar25 + auVar40._12_4_;
        auVar30._8_4_ = fVar46 + auVar29._8_4_;
        auVar30._12_4_ = fVar46 + auVar29._12_4_;
        auVar29 = auVar30;
        auVar40 = auVar52;
      }
      else {
        *(undefined1 (*) [16])(param_2 + 0x200) = _DAT_100bf720;
      }
      auVar28 = rcpps(auVar28,auVar29);
      fVar46 = auVar28._0_4_;
      fVar48 = auVar28._4_4_;
      fVar27 = auVar28._8_4_;
      fVar13 = auVar28._12_4_;
      *(undefined1 (*) [16])(param_2 + 0x210) = auVar29;
      *(uint *)(param_2 + 0x220) =
           (uint)((fVar46 + fVar46) - fVar46 * fVar46 * auVar29._0_4_) &
           -(uint)(1.1920929e-07 < auVar29._0_4_);
      *(uint *)(param_2 + 0x224) =
           (uint)((fVar48 + fVar48) - fVar48 * fVar48 * auVar29._4_4_) &
           -(uint)(1.1920929e-07 < auVar29._4_4_);
      *(uint *)(param_2 + 0x228) =
           (uint)((fVar27 + fVar27) - fVar27 * fVar27 * auVar29._8_4_) &
           -(uint)(1.1920929e-07 < auVar29._8_4_);
      *(uint *)(param_2 + 0x22c) =
           (uint)((fVar13 + fVar13) - fVar13 * fVar13 * auVar29._12_4_) &
           -(uint)(1.1920929e-07 < auVar29._12_4_);
      iVar7 = (*(byte *)((int)this + 0xd8) & 1) * 0x10;
      uVar4 = *(undefined4 *)(&UNK_100ce724 + iVar7);
      uVar5 = *(undefined4 *)(&UNK_100ce728 + iVar7);
      uVar6 = *(undefined4 *)(&UNK_100ce72c + iVar7);
      *(undefined4 *)(param_2 + 0x230) = *(undefined4 *)(&DAT_100ce720 + iVar7);
      *(undefined4 *)(param_2 + 0x234) = uVar4;
      *(undefined4 *)(param_2 + 0x238) = uVar5;
      *(undefined4 *)(param_2 + 0x23c) = uVar6;
      goto LAB_1009c942;
    }
    *(undefined4 *)(param_2 + 0x240) = 2;
  }
  puVar2 = *(ushort **)(iVar8 + 0x94);
  iVar8 = *(int *)(param_2 + 0x2c0);
  fVar61 = *(float *)(param_2 + 0x104);
  puVar3 = *(ushort **)(iVar1 + 0x94);
  if (iVar8 == 1) {
    fVar46 = *(float *)(puVar2 + 100) * *(float *)(param_2 + 0x268) +
             *(float *)(puVar2 + 0x62) * *(float *)(param_2 + 0x264) +
             *(float *)(puVar2 + 0x60) * *(float *)(param_2 + 0x260);
    fVar48 = *(float *)(puVar3 + 100) * *(float *)(param_2 + 600) +
             *(float *)(puVar3 + 0x62) * *(float *)(param_2 + 0x254) +
             *(float *)(puVar3 + 0x60) * *(float *)(param_2 + 0x250);
    auVar43._0_4_ = fVar48 + fVar46;
    auVar43._4_4_ = fVar48 + fVar46;
    auVar43._8_4_ = fVar48 + fVar46;
    auVar43._12_4_ = fVar48 + fVar46;
LAB_1009ce94:
    fVar46 = *(float *)((int)this + 0xd4);
    fVar48 = *param_1;
    fVar27 = param_1[1];
    fVar51 = 0.0;
    fVar35 = 0.0;
    fVar57 = 0.0;
    fVar24 = 0.0;
    fVar13 = *(float *)((int)this + 0xcc);
    if (*(char *)((int)this + 0xbd) != '\0') {
      fVar25 = fVar61 - fVar46;
      auVar55._0_4_ = fVar47 * auVar43._0_4_;
      auVar55._4_4_ = fVar47 * auVar43._4_4_;
      auVar55._8_4_ = fVar47 * auVar43._8_4_;
      auVar55._12_4_ = fVar47 * auVar43._12_4_;
      auVar22._0_4_ = (fVar38 * 0.8 * fVar25 * fVar27 - auVar55._0_4_) * *(float *)(param_2 + 0x2a0)
      ;
      auVar22._4_4_ =
           (fVar47 * 0.8 * (fVar61 - fVar46) * 0.0 - auVar55._4_4_) * *(float *)(param_2 + 0x2a4);
      auVar22._8_4_ =
           (fVar49 * 0.8 * (fVar61 - fVar46) * 0.0 - auVar55._8_4_) * *(float *)(param_2 + 0x2a8);
      auVar22._12_4_ =
           (fVar50 * 0.8 * (fVar61 - fVar46) * 0.0 - auVar55._12_4_) * *(float *)(param_2 + 0x2ac);
      auVar28 = maxps(auVar22,_DAT_100bf800);
      auVar45._0_8_ = auVar28._0_8_ & 0x7fffffff7fffffff;
      auVar45._8_4_ = ABS(auVar28._8_4_);
      auVar45._12_4_ = ABS(auVar28._12_4_);
      auVar63 = rcpps(auVar55,auVar45);
      fVar51 = auVar63._0_4_;
      fVar35 = auVar63._4_4_;
      fVar57 = auVar63._8_4_;
      fVar24 = auVar63._12_4_;
      auVar60._0_4_ = ((fVar51 + fVar51) - fVar51 * fVar51 * ABS(auVar28._0_4_)) * fVar13 * fVar48;
      auVar60._4_4_ = ((fVar35 + fVar35) - fVar35 * fVar35 * ABS(auVar28._4_4_)) * fVar13 * 0.0;
      auVar60._8_4_ = ((fVar57 + fVar57) - fVar57 * fVar57 * auVar45._8_4_) * fVar13 * 0.0;
      auVar60._12_4_ = ((fVar24 + fVar24) - fVar24 * fVar24 * auVar45._12_4_) * fVar13 * 0.0;
      auVar63 = minps(_DAT_100bf730,auVar60);
      fVar51 = auVar63._0_4_ * auVar28._0_4_;
      fVar35 = auVar63._4_4_ * auVar28._4_4_;
      fVar57 = auVar63._8_4_ * auVar28._8_4_;
      fVar24 = auVar63._12_4_ * auVar28._12_4_;
      *(float *)((int)this + 0xd4) =
           *(float *)((int)this + 0xd0) * fVar48 + fVar46 + (fVar25 - auVar63._0_4_ * fVar25);
    }
    fVar46 = *(float *)((int)this + 0xc4);
    fVar13 = *(float *)((int)this + 200);
    fVar25 = *(float *)((int)this + 0xc0);
    fVar48 = fVar61 - (*(float *)(param_2 + 0x290) * fVar51 + auVar43._0_4_) * fVar48;
    fVar35 = fVar61 - (*(float *)(param_2 + 0x294) * fVar35 + auVar43._4_4_) * 0.0;
    fVar57 = fVar61 - (*(float *)(param_2 + 0x298) * fVar57 + auVar43._8_4_) * 0.0;
    fVar61 = fVar61 - (*(float *)(param_2 + 0x29c) * fVar24 + auVar43._12_4_) * 0.0;
    fVar38 = fVar13 * fVar38 * *(float *)(param_2 + 0x2a0) * fVar27;
    fVar47 = fVar13 * fVar47 * *(float *)(param_2 + 0x2a4) * 0.0;
    fVar49 = fVar13 * fVar49 * *(float *)(param_2 + 0x2a8) * 0.0;
    fVar50 = fVar13 * fVar50 * *(float *)(param_2 + 0x2ac) * 0.0;
    auVar23._0_4_ = (fVar48 - fVar46) * fVar38;
    auVar23._4_4_ = (fVar35 - fVar46) * fVar47;
    auVar23._8_4_ = (fVar57 - fVar46) * fVar49;
    auVar23._12_4_ = (fVar61 - fVar46) * fVar50;
    auVar28 = maxps(auVar23,_DAT_100bf720);
    auVar34._0_4_ = (fVar48 - fVar25) * fVar38;
    auVar34._4_4_ = (fVar35 - fVar25) * fVar47;
    auVar34._8_4_ = (fVar57 - fVar25) * fVar49;
    auVar34._12_4_ = (fVar61 - fVar25) * fVar50;
    auVar63 = minps(auVar34,_DAT_100bf720);
    fVar38 = *(float *)(param_2 + 0x274);
    fVar47 = *(float *)(param_2 + 0x278);
    fVar51 = (float)((uint)(auVar28._0_4_ + auVar63._0_4_) & *(uint *)(param_2 + 0x2b0)) + fVar51;
    *(float *)(puVar3 + 0x60) = *(float *)(param_2 + 0x270) * fVar51 + *(float *)(puVar3 + 0x60);
    *(float *)(puVar3 + 0x62) = fVar38 * fVar51 + *(float *)(puVar3 + 0x62);
    *(float *)(puVar3 + 100) = fVar47 * fVar51 + *(float *)(puVar3 + 100);
    puVar3[0x66] = 0;
    puVar3[0x67] = 0;
    fVar38 = *(float *)(param_2 + 0x284);
    fVar47 = *(float *)(param_2 + 0x288);
    *(float *)(puVar2 + 0x60) = *(float *)(param_2 + 0x280) * fVar51 + *(float *)(puVar2 + 0x60);
    *(float *)(puVar2 + 0x62) = fVar38 * fVar51 + *(float *)(puVar2 + 0x62);
    *(float *)(puVar2 + 100) = fVar47 * fVar51 + *(float *)(puVar2 + 100);
    puVar2[0x66] = 0;
    puVar2[0x67] = 0;
    return;
  }
  if (iVar8 != 2) {
    fVar46 = *(float *)(param_2 + 0x130);
    fVar48 = *(float *)(param_2 + 0x120);
    fVar27 = *(float *)(param_2 + 0x134) * *(float *)(param_2 + 0x128) -
             *(float *)(param_2 + 0x138) * *(float *)(param_2 + 0x124);
    fVar13 = *(float *)(param_2 + 0x138) * fVar48 - fVar46 * *(float *)(param_2 + 0x128);
    fVar51 = fVar46 * *(float *)(param_2 + 0x124) - *(float *)(param_2 + 0x134) * fVar48;
    fVar46 = fVar46 * fVar48 - fVar46 * fVar48;
    auVar20._0_4_ = fVar46 * fVar46 + fVar51 * fVar51 + fVar13 * fVar13 + fVar27 * fVar27;
    auVar20._4_4_ = auVar20._0_4_;
    auVar20._8_4_ = auVar20._0_4_;
    auVar20._12_4_ = auVar20._0_4_;
    auVar44._4_4_ = -(uint)(auVar20._0_4_ <= 1.1920929e-07);
    auVar44._0_4_ = -(uint)(auVar20._0_4_ <= 1.1920929e-07);
    auVar44._8_4_ = -(uint)(auVar20._0_4_ <= 1.1920929e-07);
    auVar44._12_4_ = -(uint)(auVar20._0_4_ <= 1.1920929e-07);
    iVar8 = movmskps(iVar8,auVar44);
    if (iVar8 == 0) {
      auVar28 = rsqrtps(auVar44,auVar20);
      fVar46 = auVar28._0_4_;
      fVar48 = auVar28._4_4_;
      fVar35 = auVar28._8_4_;
      fVar57 = auVar28._12_4_;
      auVar21._0_4_ = (3.0 - fVar46 * fVar46 * auVar20._0_4_) * fVar46 * 0.5 * 0.1;
      auVar21._4_4_ = (3.0 - fVar48 * fVar48 * auVar20._0_4_) * fVar48 * 0.5 * 0.1;
      auVar21._8_4_ = (3.0 - fVar35 * fVar35 * auVar20._0_4_) * fVar35 * 0.5 * 0.1;
      auVar21._12_4_ = (3.0 - fVar57 * fVar57 * auVar20._0_4_) * fVar57 * 0.5 * 0.1;
      auVar28 = maxps(auVar21,_DAT_100bf730);
      fVar27 = auVar28._0_4_ * fVar27;
      fVar13 = auVar28._4_4_ * fVar13;
      fVar51 = auVar28._8_4_ * fVar51;
      *(undefined4 *)(param_2 + 0x2c0) = 1;
      fVar57 = fVar51 * *(float *)(puVar3 + 0x40) + fVar13 * *(float *)(puVar3 + 0x38) +
               fVar27 * *(float *)(puVar3 + 0x30);
      fVar24 = fVar51 * *(float *)(puVar3 + 0x42) + fVar13 * *(float *)(puVar3 + 0x3a) +
               fVar27 * *(float *)(puVar3 + 0x32);
      fVar25 = fVar51 * *(float *)(puVar3 + 0x44) + fVar13 * *(float *)(puVar3 + 0x3c) +
               fVar27 * *(float *)(puVar3 + 0x34);
      fVar26 = fVar51 * *(float *)(puVar3 + 0x46) + fVar13 * *(float *)(puVar3 + 0x3e) +
               fVar27 * *(float *)(puVar3 + 0x36);
      fVar27 = 0.0 - fVar27;
      fVar13 = 0.0 - fVar13;
      fVar51 = 0.0 - fVar51;
      fVar46 = fVar51 * *(float *)(puVar2 + 0x40) + fVar13 * *(float *)(puVar2 + 0x38) +
               fVar27 * *(float *)(puVar2 + 0x30);
      fVar48 = fVar51 * *(float *)(puVar2 + 0x42) + fVar13 * *(float *)(puVar2 + 0x3a) +
               fVar27 * *(float *)(puVar2 + 0x32);
      fVar35 = fVar51 * *(float *)(puVar2 + 0x44) + fVar13 * *(float *)(puVar2 + 0x3c) +
               fVar27 * *(float *)(puVar2 + 0x34);
      fVar27 = fVar51 * *(float *)(puVar2 + 0x46) + fVar13 * *(float *)(puVar2 + 0x3e) +
               fVar27 * *(float *)(puVar2 + 0x36);
      *(float *)(param_2 + 0x250) = fVar57;
      *(float *)(param_2 + 0x254) = fVar24;
      *(float *)(param_2 + 600) = fVar25;
      *(float *)(param_2 + 0x25c) = fVar26;
      *(float *)(param_2 + 0x260) = fVar46;
      *(float *)(param_2 + 0x264) = fVar48;
      *(float *)(param_2 + 0x268) = fVar35;
      *(float *)(param_2 + 0x26c) = fVar27;
      auVar32 = _DAT_100bf720;
      if ((*puVar3 & 0x12) == 0) {
        fVar14 = *(float *)(puVar3 + 100) * fVar25 + *(float *)(puVar3 + 0x62) * fVar24 +
                 *(float *)(puVar3 + 0x60) * fVar57;
        fVar13 = *(float *)(puVar3 + 0x18);
        fVar51 = *(float *)(puVar3 + 0x1a);
        fVar36 = *(float *)(puVar3 + 0x1c);
        fVar37 = *(float *)(puVar3 + 0x1e);
        *(float *)(param_2 + 0x270) = fVar13 * fVar57;
        *(float *)(param_2 + 0x274) = fVar51 * fVar24;
        *(float *)(param_2 + 0x278) = fVar36 * fVar25;
        *(float *)(param_2 + 0x27c) = fVar37 * fVar26;
        auVar32._0_4_ =
             fVar36 * fVar25 * fVar25 + fVar51 * fVar24 * fVar24 + fVar13 * fVar57 * fVar57;
        auVar43._4_4_ = fVar14;
        auVar43._0_4_ = fVar14;
        auVar43._8_4_ = fVar14;
        auVar43._12_4_ = fVar14;
        auVar32._4_4_ = auVar32._0_4_;
        auVar32._8_4_ = auVar32._0_4_;
        auVar32._12_4_ = auVar32._0_4_;
      }
      else {
        *(undefined1 (*) [16])(param_2 + 0x270) = _DAT_100bf720;
        auVar43 = auVar32;
      }
      auVar28 = _DAT_100bf720;
      if ((*puVar2 & 0x12) == 0) {
        fVar25 = *(float *)(puVar2 + 100) * fVar35 + *(float *)(puVar2 + 0x62) * fVar48 +
                 *(float *)(puVar2 + 0x60) * fVar46;
        fVar13 = *(float *)(puVar2 + 0x18);
        fVar51 = *(float *)(puVar2 + 0x1a);
        fVar57 = *(float *)(puVar2 + 0x1c);
        fVar24 = *(float *)(puVar2 + 0x1e);
        *(float *)(param_2 + 0x280) = fVar13 * fVar46;
        *(float *)(param_2 + 0x284) = fVar51 * fVar48;
        *(float *)(param_2 + 0x288) = fVar57 * fVar35;
        *(float *)(param_2 + 0x28c) = fVar24 * fVar27;
        fVar46 = fVar57 * fVar35 * fVar35 + fVar51 * fVar48 * fVar48 + fVar13 * fVar46 * fVar46;
        auVar28 = ZEXT416((uint)fVar46);
        auVar54._4_4_ = fVar25 + auVar43._4_4_;
        auVar54._0_4_ = fVar25 + auVar43._0_4_;
        auVar33._4_4_ = fVar46 + auVar32._4_4_;
        auVar33._0_4_ = fVar46 + auVar32._0_4_;
        auVar54._8_4_ = fVar25 + auVar43._8_4_;
        auVar54._12_4_ = fVar25 + auVar43._12_4_;
        auVar33._8_4_ = fVar46 + auVar32._8_4_;
        auVar33._12_4_ = fVar46 + auVar32._12_4_;
        auVar32 = auVar33;
        auVar43 = auVar54;
      }
      else {
        *(undefined1 (*) [16])(param_2 + 0x280) = _DAT_100bf720;
      }
      auVar28 = rcpps(auVar28,auVar32);
      fVar46 = auVar28._0_4_;
      fVar48 = auVar28._4_4_;
      fVar27 = auVar28._8_4_;
      fVar13 = auVar28._12_4_;
      *(undefined1 (*) [16])(param_2 + 0x290) = auVar32;
      *(uint *)(param_2 + 0x2a0) =
           (uint)((fVar46 + fVar46) - fVar46 * fVar46 * auVar32._0_4_) &
           -(uint)(1.1920929e-07 < auVar32._0_4_);
      *(uint *)(param_2 + 0x2a4) =
           (uint)((fVar48 + fVar48) - fVar48 * fVar48 * auVar32._4_4_) &
           -(uint)(1.1920929e-07 < auVar32._4_4_);
      *(uint *)(param_2 + 0x2a8) =
           (uint)((fVar27 + fVar27) - fVar27 * fVar27 * auVar32._8_4_) &
           -(uint)(1.1920929e-07 < auVar32._8_4_);
      *(uint *)(param_2 + 0x2ac) =
           (uint)((fVar13 + fVar13) - fVar13 * fVar13 * auVar32._12_4_) &
           -(uint)(1.1920929e-07 < auVar32._12_4_);
      iVar8 = (*(byte *)((int)this + 0xbc) & 1) * 0x10;
      uVar4 = *(undefined4 *)(&UNK_100ce724 + iVar8);
      uVar5 = *(undefined4 *)(&UNK_100ce728 + iVar8);
      uVar6 = *(undefined4 *)(&UNK_100ce72c + iVar8);
      *(undefined4 *)(param_2 + 0x2b0) = *(undefined4 *)(&DAT_100ce720 + iVar8);
      *(undefined4 *)(param_2 + 0x2b4) = uVar4;
      *(undefined4 *)(param_2 + 0x2b8) = uVar5;
      *(undefined4 *)(param_2 + 700) = uVar6;
      goto LAB_1009ce94;
    }
    *(undefined4 *)(param_2 + 0x2c0) = 2;
  }
  return;
}


// 1009d0c0 FUN_1009d0c0

void FUN_1009d0c0(void)

{
  undefined1 auVar1 [16];
  undefined1 auVar2 [16];
  undefined1 auVar3 [16];
  uint uVar4;
  uint uVar5;
  uint uVar6;
  uint uVar7;
  float in_XMM0_Da;
  float fVar8;
  float in_XMM0_Db;
  float fVar9;
  float in_XMM0_Dc;
  float fVar10;
  float in_XMM0_Dd;
  float fVar11;
  float in_XMM1_Da;
  float in_XMM1_Db;
  float in_XMM1_Dc;
  float in_XMM1_Dd;
  undefined1 auVar12 [16];
  undefined1 auVar13 [16];
  float fVar14;
  float fVar15;
  float fVar16;
  float fVar17;
  undefined1 in_XMM6 [16];
  
  fVar14 = ABS(in_XMM1_Da);
  fVar15 = ABS(in_XMM1_Db);
  fVar16 = ABS(in_XMM1_Dc);
  fVar17 = ABS(in_XMM1_Dd);
  fVar8 = ABS(in_XMM0_Da);
  fVar9 = ABS(in_XMM0_Db);
  fVar10 = ABS(in_XMM0_Dc);
  fVar11 = ABS(in_XMM0_Dd);
  uVar4 = -(uint)(fVar14 < fVar8);
  uVar5 = -(uint)(fVar15 < fVar9);
  uVar6 = -(uint)(fVar16 < fVar10);
  uVar7 = -(uint)(fVar17 < fVar11);
  auVar12._0_4_ = uVar4 & (uint)fVar8;
  auVar12._4_4_ = uVar5 & (uint)fVar9;
  auVar12._8_4_ = uVar6 & (uint)fVar10;
  auVar12._12_4_ = uVar7 & (uint)fVar11;
  auVar1._4_4_ = ~uVar5 & (uint)fVar15;
  auVar1._0_4_ = ~uVar4 & (uint)fVar14;
  auVar1._8_4_ = ~uVar6 & (uint)fVar16;
  auVar1._12_4_ = ~uVar7 & (uint)fVar17;
  rcpps(in_XMM6,auVar12 | auVar1);
  auVar13._0_4_ = ~uVar4 & (uint)fVar8;
  auVar13._4_4_ = ~uVar5 & (uint)fVar9;
  auVar13._8_4_ = ~uVar6 & (uint)fVar10;
  auVar13._12_4_ = ~uVar7 & (uint)fVar11;
  auVar3._4_4_ = uVar5 & (uint)fVar15;
  auVar3._0_4_ = uVar4 & (uint)fVar14;
  auVar3._8_4_ = uVar6 & (uint)fVar16;
  auVar3._12_4_ = uVar7 & (uint)fVar17;
  auVar2._4_4_ = fVar9 + fVar15;
  auVar2._0_4_ = fVar8 + fVar14;
  auVar2._8_4_ = fVar10 + fVar16;
  auVar2._12_4_ = fVar11 + fVar17;
  rcpps(auVar13 | auVar3,auVar2);
  return;
}


// 1009d200 FUN_1009d200

void __thiscall FUN_1009d200(void *this,float *param_1,float *param_2,float param_3,float param_4)

{
  ushort *puVar1;
  ushort *puVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  float fVar10;
  float fVar11;
  float fVar12;
  float fVar13;
  float fVar14;
  float fVar15;
  
  puVar1 = *(ushort **)(*(int *)((int)this + 4) + 0x94);
  puVar2 = *(ushort **)(*(int *)((int)this + 8) + 0x94);
  param_2[0xb4] = param_3;
  param_2[0xb5] = param_4;
  param_2[0xb6] = 1e+20;
  param_2[0xb7] = param_2[0x47];
  FUN_1009c0f0(this,param_1,(int)param_2);
  if (*(char *)((int)this + 0x147) != '\0') {
    fVar3 = *(float *)(puVar1 + 0x60);
    fVar4 = *(float *)(puVar1 + 0x62);
    fVar5 = *(float *)(puVar1 + 100);
    fVar12 = *(float *)((int)this + 0x140) * param_3 * param_1[1];
    fVar10 = *(float *)((int)this + 0x13c);
    fVar11 = *(float *)(puVar2 + 0x60);
    fVar15 = *(float *)(puVar2 + 0x62);
    fVar6 = *(float *)(puVar2 + 100);
    fVar13 = fVar12 * *param_2 -
             ((0.0 - *(float *)(puVar2 + 0x68)) + fVar6 * param_2[0x18] + fVar15 * param_2[0x14] +
              fVar11 * param_2[0x10] +
             fVar5 * param_2[0xc] + *(float *)(puVar1 + 0x68) + fVar4 * param_2[8] +
             fVar3 * param_2[4]) * fVar10 * param_4;
    fVar14 = fVar12 * param_2[1] -
             ((0.0 - *(float *)(puVar2 + 0x6a)) + fVar6 * param_2[0x19] + fVar15 * param_2[0x15] +
              fVar11 * param_2[0x11] +
             fVar5 * param_2[0xd] + *(float *)(puVar1 + 0x6a) + fVar4 * param_2[9] +
             fVar3 * param_2[5]) * fVar10 * param_4;
    fVar15 = fVar12 * param_2[2] -
             ((0.0 - *(float *)(puVar2 + 0x6c)) + fVar6 * param_2[0x1a] + fVar15 * param_2[0x16] +
              fVar11 * param_2[0x12] +
             fVar5 * param_2[0xe] + *(float *)(puVar1 + 0x6c) + fVar4 * param_2[10] +
             fVar3 * param_2[6]) * fVar10 * param_4;
    fVar10 = fVar15 * param_2[0x3c] + fVar14 * param_2[0x38] + fVar13 * param_2[0x34];
    fVar11 = fVar15 * param_2[0x3d] + fVar14 * param_2[0x39] + fVar13 * param_2[0x35];
    fVar15 = fVar15 * param_2[0x3e] + fVar14 * param_2[0x3a] + fVar13 * param_2[0x36];
    if ((*puVar1 & 0x12) == 0) {
      fVar6 = param_2[0x25];
      fVar12 = param_2[0x26];
      fVar13 = param_2[0x21];
      fVar14 = param_2[0x22];
      fVar7 = *(float *)(puVar1 + 0x1e);
      fVar8 = param_2[0x1d];
      fVar9 = param_2[0x1e];
      *(float *)(puVar1 + 0x60) =
           fVar15 * param_2[0x24] + fVar11 * param_2[0x20] + fVar10 * param_2[0x1c] + fVar3;
      *(float *)(puVar1 + 0x62) = fVar15 * fVar6 + fVar11 * fVar13 + fVar10 * fVar8 + fVar4;
      *(float *)(puVar1 + 100) = fVar15 * fVar12 + fVar11 * fVar14 + fVar10 * fVar9 + fVar5;
      puVar1[0x66] = 0;
      puVar1[0x67] = 0;
      *(float *)(puVar1 + 0x68) = fVar7 * fVar10 + *(float *)(puVar1 + 0x68);
      *(float *)(puVar1 + 0x6a) = fVar7 * fVar11 + *(float *)(puVar1 + 0x6a);
      *(float *)(puVar1 + 0x6c) = fVar7 * fVar15 + *(float *)(puVar1 + 0x6c);
      puVar1[0x6e] = 0;
      puVar1[0x6f] = 0;
    }
    if ((*puVar2 & 0x12) == 0) {
      fVar3 = param_2[0x31];
      fVar4 = param_2[0x32];
      fVar5 = param_2[0x2d];
      fVar6 = param_2[0x2e];
      fVar12 = *(float *)(puVar2 + 0x1e);
      fVar13 = param_2[0x29];
      fVar14 = param_2[0x2a];
      *(float *)(puVar2 + 0x60) =
           fVar15 * param_2[0x30] + fVar11 * param_2[0x2c] + fVar10 * param_2[0x28] +
           *(float *)(puVar2 + 0x60);
      *(float *)(puVar2 + 0x62) =
           fVar15 * fVar3 + fVar11 * fVar5 + fVar10 * fVar13 + *(float *)(puVar2 + 0x62);
      *(float *)(puVar2 + 100) =
           fVar15 * fVar4 + fVar11 * fVar6 + fVar10 * fVar14 + *(float *)(puVar2 + 100);
      puVar2[0x66] = 0;
      puVar2[0x67] = 0;
      *(float *)(puVar2 + 0x68) = fVar12 * (0.0 - fVar10) + *(float *)(puVar2 + 0x68);
      *(float *)(puVar2 + 0x6a) = fVar12 * (0.0 - fVar11) + *(float *)(puVar2 + 0x6a);
      *(float *)(puVar2 + 0x6c) = fVar12 * (0.0 - fVar15) + *(float *)(puVar2 + 0x6c);
      puVar2[0x6e] = 0;
      puVar2[0x6f] = 0;
    }
  }
  return;
}


// 1009d4d0 FUN_1009d4d0

void __thiscall FUN_1009d4d0(void *this,undefined4 *param_1)

{
  float fVar1;
  float fVar2;
  int iVar3;
  undefined4 *puVar4;
  undefined4 *puVar5;
  float10 fVar6;
  float fVar7;
  
  puVar4 = param_1;
  puVar5 = (undefined4 *)((int)this + 0x20);
  for (iVar3 = 0xc; iVar3 != 0; iVar3 = iVar3 + -1) {
    *puVar5 = *puVar4;
    puVar4 = puVar4 + 1;
    puVar5 = puVar5 + 1;
  }
  *(undefined4 *)((int)this + 0x50) = param_1[0xc];
  *(undefined4 *)((int)this + 0x54) = param_1[0xd];
  *(undefined4 *)((int)this + 0x58) = param_1[0xe];
  puVar4 = param_1 + 0x10;
  puVar5 = (undefined4 *)((int)this + 0x60);
  for (iVar3 = 0xc; iVar3 != 0; iVar3 = iVar3 + -1) {
    *puVar5 = *puVar4;
    puVar4 = puVar4 + 1;
    puVar5 = puVar5 + 1;
  }
  *(undefined4 *)((int)this + 0x90) = param_1[0x1c];
  *(undefined4 *)((int)this + 0x94) = param_1[0x1d];
  *(undefined4 *)((int)this + 0x98) = param_1[0x1e];
  *(undefined4 *)((int)this + 0x140) = param_1[0x33];
  *(undefined4 *)((int)this + 0x13c) = param_1[0x32];
  *(undefined1 *)((int)this + 0x147) = *(undefined1 *)((int)param_1 + 0xd3);
  *(undefined1 *)((int)this + 0x144) = *(undefined1 *)(param_1 + 0x34);
  *(undefined1 *)((int)this + 0x145) = *(undefined1 *)((int)param_1 + 0xd1);
  *(undefined1 *)((int)this + 0x146) = *(undefined1 *)((int)param_1 + 0xd2);
  *(undefined1 *)((int)this + 0xa0) = *(undefined1 *)(param_1 + 0x20);
  *(undefined1 *)((int)this + 0xa1) = *(undefined1 *)((int)param_1 + 0x81);
  *(undefined4 *)((int)this + 0xa4) = param_1[0x21];
  *(undefined4 *)((int)this + 0xa8) = param_1[0x22];
  *(undefined4 *)((int)this + 0xac) = param_1[0x23];
  *(float *)((int)this + 0xb0) = ABS((float)param_1[0x24]);
  *(undefined4 *)((int)this + 0xb4) = param_1[0x25];
  if (6.2831855 <= *(float *)((int)this + 0xa8) - *(float *)((int)this + 0xa4)) {
    *(undefined1 *)((int)this + 0xa0) = 0;
  }
  *(undefined1 *)((int)this + 0xbc) = *(undefined1 *)(param_1 + 0x26);
  *(undefined1 *)((int)this + 0xbd) = *(undefined1 *)((int)param_1 + 0x99);
  *(undefined4 *)((int)this + 0xc0) = param_1[0x27];
  *(undefined4 *)((int)this + 0xc4) = param_1[0x28];
  *(undefined4 *)((int)this + 200) = param_1[0x29];
  *(float *)((int)this + 0xcc) = ABS((float)param_1[0x2a]);
  *(undefined4 *)((int)this + 0xd0) = param_1[0x2b];
  if (6.2831855 <= *(float *)((int)this + 0xc4) - *(float *)((int)this + 0xc0)) {
    *(undefined1 *)((int)this + 0xbc) = 0;
  }
  *(undefined1 *)((int)this + 0xd8) = *(undefined1 *)(param_1 + 0x2c);
  *(undefined1 *)((int)this + 0xd9) = *(undefined1 *)((int)param_1 + 0xb1);
  *(undefined4 *)((int)this + 0xdc) = param_1[0x2d];
  *(undefined4 *)((int)this + 0xe0) = param_1[0x2e];
  *(undefined4 *)((int)this + 0xe4) = param_1[0x2f];
  *(float *)((int)this + 0xe8) = ABS((float)param_1[0x30]);
  *(undefined4 *)((int)this + 0xec) = param_1[0x31];
  if (6.2831855 <= *(float *)((int)this + 0xe0) - *(float *)((int)this + 0xdc)) {
    *(undefined1 *)((int)this + 0xd8) = 0;
  }
  fVar1 = (float)param_1[0x27];
  fVar2 = (float)param_1[0x28];
  fVar7 = (fVar1 + fVar2) * 0.5;
  if (0.001 < ABS(fVar7)) {
    FUN_100bb090((void *)((int)this + 0x20),2,fVar7);
  }
  *(undefined4 *)((int)this + 0xc4) = 0x42c80000;
  fVar6 = (float10)fcos((float10)(fVar2 - fVar1) * (float10)0.5);
  *(float *)((int)this + 0xc0) = (float)fVar6;
  fVar6 = (float10)fsin((float10)(float)param_1[0x2e]);
  *(float *)((int)this + 0xdc) = (float)-fVar6;
  fVar6 = (float10)fsin((float10)(float)param_1[0x2d]);
  *(float *)((int)this + 0xe0) = (float)-fVar6;
  *(undefined4 *)((int)this + 0xf4) = param_1[0x20];
  *(undefined4 *)((int)this + 0xf8) = param_1[0x21];
  *(undefined4 *)((int)this + 0xfc) = param_1[0x22];
  *(undefined4 *)((int)this + 0x100) = param_1[0x23];
  *(undefined4 *)((int)this + 0x104) = param_1[0x24];
  *(undefined4 *)((int)this + 0x108) = param_1[0x25];
  *(undefined4 *)((int)this + 0x10c) = param_1[0x26];
  *(undefined4 *)((int)this + 0x110) = param_1[0x27];
  *(undefined4 *)((int)this + 0x114) = param_1[0x28];
  *(undefined4 *)((int)this + 0x118) = param_1[0x29];
  *(undefined4 *)((int)this + 0x11c) = param_1[0x2a];
  *(undefined4 *)((int)this + 0x120) = param_1[0x2b];
  *(undefined4 *)((int)this + 0x124) = param_1[0x2c];
  *(undefined4 *)((int)this + 0x128) = param_1[0x2d];
  *(undefined4 *)((int)this + 300) = param_1[0x2e];
  *(undefined4 *)((int)this + 0x130) = param_1[0x2f];
  *(undefined4 *)((int)this + 0x134) = param_1[0x30];
  *(undefined4 *)((int)this + 0x138) = param_1[0x31];
  return;
}


// 1009d850 FUN_1009d850

undefined4 * __thiscall
FUN_1009d850(void *this,undefined4 param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4
            )

{
  FUN_100a4de0(this,param_1,param_3,param_4);
  *(undefined ***)this = &PTR_LAB_100ce780;
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined1 *)((int)this + 0xa0) = 0;
  *(undefined1 *)((int)this + 0xa1) = 0;
  *(undefined1 *)((int)this + 0xbc) = 0;
  *(undefined1 *)((int)this + 0xbd) = 0;
  *(undefined1 *)((int)this + 0xd8) = 0;
  *(undefined1 *)((int)this + 0xd9) = 0;
  *(undefined4 *)((int)this + 0xa4) = 0;
  *(undefined4 *)((int)this + 0xa8) = 0;
  *(undefined4 *)((int)this + 0xac) = 0x3f800000;
  *(undefined4 *)((int)this + 0xb0) = 0;
  *(undefined4 *)((int)this + 0xb4) = 0;
  *(undefined4 *)((int)this + 0xb8) = 0;
  *(undefined4 *)((int)this + 0xc0) = 0;
  *(undefined4 *)((int)this + 0xc4) = 0;
  *(undefined4 *)((int)this + 200) = 0x3f800000;
  *(undefined4 *)((int)this + 0xcc) = 0;
  *(undefined4 *)((int)this + 0xd0) = 0;
  *(undefined4 *)((int)this + 0xd4) = 0;
  *(undefined4 *)((int)this + 0xdc) = 0;
  *(undefined4 *)((int)this + 0xe0) = 0;
  *(undefined4 *)((int)this + 0xe4) = 0x3f800000;
  *(undefined4 *)((int)this + 0xe8) = 0;
  *(undefined4 *)((int)this + 0xec) = 0;
  *(undefined4 *)((int)this + 0xf0) = 0;
  *(undefined1 *)((int)this + 0xf4) = 0;
  *(undefined1 *)((int)this + 0xf5) = 0;
  *(undefined1 *)((int)this + 0x10c) = 0;
  *(undefined1 *)((int)this + 0x10d) = 0;
  *(undefined1 *)((int)this + 0x124) = 0;
  *(undefined1 *)((int)this + 0x125) = 0;
  *(undefined4 *)((int)this + 0xf8) = 0;
  *(undefined4 *)((int)this + 0xfc) = 0;
  *(undefined4 *)((int)this + 0x100) = 0x3f800000;
  *(undefined4 *)((int)this + 0x104) = 0;
  *(undefined4 *)((int)this + 0x108) = 0;
  *(undefined4 *)((int)this + 0x110) = 0;
  *(undefined4 *)((int)this + 0x114) = 0;
  *(undefined4 *)((int)this + 0x118) = 0x3f800000;
  *(undefined4 *)((int)this + 0x11c) = 0;
  *(undefined4 *)((int)this + 0x120) = 0;
  *(undefined4 *)((int)this + 0x128) = 0;
  *(undefined4 *)((int)this + 300) = 0;
  *(undefined4 *)((int)this + 0x130) = 0x3f800000;
  *(undefined4 *)((int)this + 0x134) = 0;
  *(undefined4 *)((int)this + 0x138) = 0;
  FUN_1009d4d0(this,param_2);
  return (undefined4 *)this;
}


// 1009da10 FUN_1009da10

undefined4 * __thiscall
FUN_1009da10(void *this,void *param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4)

{
  FUN_100a4e00(this,param_1,param_3,param_4,0x195,0x2e0);
  *(undefined ***)this = &PTR_LAB_100ce780;
  *(undefined4 *)((int)this + 0xa4) = 0;
  *(undefined4 *)((int)this + 0xa8) = 0;
  *(undefined4 *)((int)this + 0xac) = 0x3f800000;
  *(undefined4 *)((int)this + 0xb0) = 0;
  *(undefined4 *)((int)this + 0xb4) = 0;
  *(undefined4 *)((int)this + 0xb8) = 0;
  *(undefined4 *)((int)this + 0xc0) = 0;
  *(undefined4 *)((int)this + 0xc4) = 0;
  *(undefined4 *)((int)this + 200) = 0x3f800000;
  *(undefined4 *)((int)this + 0xcc) = 0;
  *(undefined4 *)((int)this + 0xd0) = 0;
  *(undefined4 *)((int)this + 0xd4) = 0;
  *(undefined4 *)((int)this + 0xdc) = 0;
  *(undefined4 *)((int)this + 0xe0) = 0;
  *(undefined4 *)((int)this + 0xe4) = 0x3f800000;
  *(undefined4 *)((int)this + 0xe8) = 0;
  *(undefined4 *)((int)this + 0xec) = 0;
  *(undefined4 *)((int)this + 0xf0) = 0;
  *(undefined1 *)((int)this + 0xa0) = 0;
  *(undefined1 *)((int)this + 0xa1) = 0;
  *(undefined1 *)((int)this + 0xbc) = 0;
  *(undefined1 *)((int)this + 0xbd) = 0;
  *(undefined1 *)((int)this + 0xd8) = 0;
  *(undefined1 *)((int)this + 0xd9) = 0;
  *(undefined1 *)((int)this + 0xf4) = 0;
  *(undefined1 *)((int)this + 0xf5) = 0;
  *(undefined4 *)((int)this + 0xf8) = 0;
  *(undefined4 *)((int)this + 0xfc) = 0;
  *(undefined4 *)((int)this + 0x100) = 0x3f800000;
  *(undefined4 *)((int)this + 0x104) = 0;
  *(undefined4 *)((int)this + 0x108) = 0;
  *(undefined1 *)((int)this + 0x10c) = 0;
  *(undefined1 *)((int)this + 0x10d) = 0;
  *(undefined4 *)((int)this + 0x110) = 0;
  *(undefined4 *)((int)this + 0x114) = 0;
  *(undefined4 *)((int)this + 0x118) = 0x3f800000;
  *(undefined4 *)((int)this + 0x11c) = 0;
  *(undefined4 *)((int)this + 0x120) = 0;
  *(undefined1 *)((int)this + 0x124) = 0;
  *(undefined1 *)((int)this + 0x125) = 0;
  *(undefined4 *)((int)this + 0x128) = 0;
  *(undefined4 *)((int)this + 300) = 0;
  *(undefined4 *)((int)this + 0x130) = 0x3f800000;
  *(undefined4 *)((int)this + 0x134) = 0;
  *(undefined4 *)((int)this + 0x138) = 0;
  FUN_1009d4d0(this,param_2);
  return (undefined4 *)this;
}


// 1009dbc0 FUN_1009dbc0

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __thiscall
FUN_1009dbc0(void *this,float *param_1,float *param_2,float param_3,float param_4)

{
  float *pfVar1;
  ushort uVar2;
  int iVar3;
  ushort *puVar4;
  int iVar5;
  ushort *puVar6;
  int iVar7;
  byte bVar8;
  float fVar9;
  float fVar12;
  float fVar13;
  undefined1 auVar10 [16];
  undefined1 auVar11 [16];
  float fVar14;
  float fVar15;
  float fVar16;
  float fVar17;
  float fVar18;
  float fVar19;
  float fVar20;
  float fVar21;
  float fVar22;
  float fVar23;
  float fVar24;
  float fVar26;
  float fVar27;
  float fVar28;
  float fVar29;
  undefined1 auVar25 [16];
  float fVar30;
  float fVar31;
  float fVar32;
  undefined1 auVar33 [16];
  undefined1 auVar34 [16];
  undefined1 auVar35 [16];
  undefined1 auVar36 [16];
  float local_150;
  float fStack_14c;
  float fStack_148;
  float fStack_144;
  float local_140;
  float fStack_13c;
  float fStack_138;
  float fStack_134;
  float local_130;
  float fStack_12c;
  float fStack_128;
  float fStack_124;
  undefined1 local_120 [16];
  float local_110;
  float fStack_10c;
  float fStack_108;
  float fStack_104;
  float local_100;
  float fStack_fc;
  float fStack_f8;
  float fStack_f4;
  float local_f0;
  float fStack_ec;
  float fStack_e8;
  float fStack_e4;
  float local_e0;
  float fStack_dc;
  float fStack_d8;
  float fStack_d4;
  float local_d0;
  float fStack_cc;
  float fStack_c8;
  float fStack_c4;
  float local_c0;
  float fStack_bc;
  float fStack_b8;
  float fStack_b4;
  float local_b0;
  float fStack_ac;
  float fStack_a8;
  float fStack_a4;
  float local_a0;
  float fStack_9c;
  float fStack_98;
  float fStack_94;
  float local_90;
  float fStack_8c;
  float fStack_88;
  float fStack_84;
  float local_80;
  float fStack_7c;
  float fStack_78;
  float fStack_74;
  float local_70;
  float fStack_6c;
  float fStack_68;
  float fStack_64;
  float local_60;
  float fStack_5c;
  float fStack_58;
  float fStack_54;
  float local_50;
  float fStack_4c;
  float fStack_48;
  float fStack_44;
  float local_40;
  float fStack_3c;
  float fStack_38;
  float fStack_34;
  float local_30;
  float fStack_2c;
  float fStack_28;
  float fStack_24;
  float local_20;
  float fStack_1c;
  float fStack_18;
  float fStack_14;
  
  iVar3 = *(int *)((int)this + 8);
  puVar4 = *(ushort **)(iVar3 + 0x94);
  param_2[0x70] = 0.0;
  param_2[0x90] = 0.0;
  param_2[0xb0] = 0.0;
  iVar5 = *(int *)((int)this + 4);
  puVar6 = *(ushort **)(iVar5 + 0x94);
  local_60 = *(float *)(puVar6 + 0x30);
  local_50 = *(float *)(puVar6 + 0x32);
  local_40 = *(float *)(puVar6 + 0x34);
  fStack_5c = *(float *)(puVar6 + 0x38);
  fStack_4c = *(float *)(puVar6 + 0x3a);
  fStack_3c = *(float *)(puVar6 + 0x3c);
  fStack_58 = *(float *)(puVar6 + 0x40);
  fStack_48 = *(float *)(puVar6 + 0x42);
  fStack_38 = *(float *)(puVar6 + 0x44);
  local_30 = *(float *)(puVar6 + 0x48);
  fStack_2c = *(float *)(puVar6 + 0x4a);
  fStack_28 = *(float *)(puVar6 + 0x4c);
  fStack_24 = *(float *)(puVar6 + 0x4e);
  fStack_54 = 0.0;
  fStack_44 = 0.0;
  fStack_34 = 0.0;
  if ((*(uint *)(iVar5 + 0x44) & 0x800) == 0) {
    fVar14 = *(float *)(iVar5 + 0x30);
    fVar15 = *(float *)(iVar5 + 0x34);
    fVar16 = *(float *)(iVar5 + 0x38);
    local_30 = fVar16 * local_40 + local_30 + fVar15 * local_50 + fVar14 * local_60;
    fStack_2c = fVar16 * fStack_3c + fStack_2c + fVar15 * fStack_4c + fVar14 * fStack_5c;
    fStack_28 = fVar16 * fStack_38 + fStack_28 + fVar15 * fStack_48 + fVar14 * fStack_58;
    fStack_24 = fVar16 * 0.0 + fStack_24 + fVar15 * 0.0 + fVar14 * 0.0;
  }
  local_110 = *(float *)(puVar4 + 0x30);
  local_100 = *(float *)(puVar4 + 0x32);
  local_f0 = *(float *)(puVar4 + 0x34);
  fStack_10c = *(float *)(puVar4 + 0x38);
  fStack_fc = *(float *)(puVar4 + 0x3a);
  fStack_108 = *(float *)(puVar4 + 0x40);
  fStack_f8 = *(float *)(puVar4 + 0x42);
  fStack_e8 = *(float *)(puVar4 + 0x44);
  fStack_ec = *(float *)(puVar4 + 0x3c);
  local_e0 = *(float *)(puVar4 + 0x48);
  fStack_dc = *(float *)(puVar4 + 0x4a);
  fStack_d8 = *(float *)(puVar4 + 0x4c);
  fStack_d4 = *(float *)(puVar4 + 0x4e);
  fStack_104 = 0.0;
  fStack_f4 = 0.0;
  fStack_e4 = 0.0;
  if ((*(uint *)(iVar3 + 0x44) & 0x800) == 0) {
    fVar14 = *(float *)(iVar3 + 0x30);
    fVar15 = *(float *)(iVar3 + 0x34);
    fVar16 = *(float *)(iVar3 + 0x38);
    local_e0 = fVar16 * local_f0 + local_e0 + fVar15 * local_100 + fVar14 * local_110;
    fStack_dc = fVar16 * fStack_ec + fStack_dc + fVar15 * fStack_fc + fVar14 * fStack_10c;
    fStack_d8 = fVar16 * fStack_e8 + fStack_d8 + fVar15 * fStack_f8 + fVar14 * fStack_108;
    fStack_d4 = fVar16 * 0.0 + fStack_d4 + fVar15 * 0.0 + fVar14 * 0.0;
  }
  fVar14 = *(float *)((int)this + 0x20);
  fVar15 = *(float *)((int)this + 0x24);
  fVar16 = *(float *)((int)this + 0x28);
  fVar24 = *(float *)((int)this + 0x40);
  fVar27 = *(float *)((int)this + 0x44);
  fVar29 = *(float *)((int)this + 0x48);
  fVar30 = fVar16 * local_40 + fVar15 * local_50 + fVar14 * local_60;
  fVar31 = fVar16 * fStack_3c + fVar15 * fStack_4c + fVar14 * fStack_5c;
  fVar32 = fVar16 * fStack_38 + fVar15 * fStack_48 + fVar14 * fStack_58;
  fVar9 = *(float *)((int)this + 0x60);
  fVar12 = *(float *)((int)this + 100);
  fVar13 = *(float *)((int)this + 0x68);
  fVar20 = fVar13 * local_f0 + fVar12 * local_100 + fVar9 * local_110;
  fVar21 = fVar13 * fStack_ec + fVar12 * fStack_fc + fVar9 * fStack_10c;
  fVar22 = fVar13 * fStack_e8 + fVar12 * fStack_f8 + fVar9 * fStack_108;
  fVar17 = fVar29 * local_40 + fVar27 * local_50 + fVar24 * local_60;
  fVar18 = fVar29 * fStack_3c + fVar27 * fStack_4c + fVar24 * fStack_5c;
  fVar19 = fVar29 * fStack_38 + fVar27 * fStack_48 + fVar24 * fStack_58;
  fVar23 = fVar20 + fVar30;
  fVar26 = fVar21 + fVar31;
  fVar28 = fVar22 + fVar32;
  fStack_94 = fVar28 * fVar28 + fVar26 * fVar26 + fVar23 * fVar23;
  auVar10._0_8_ = CONCAT44(fStack_94,fStack_94);
  auVar10._8_4_ = fStack_94;
  auVar10._12_4_ = fStack_94;
  auVar33._8_4_ = fStack_94;
  auVar33._0_8_ = auVar10._0_8_;
  auVar33._12_4_ = fStack_94;
  auVar10 = rsqrtps(auVar10,auVar33);
  auVar35._0_4_ = auVar10._0_4_ * auVar10._0_4_ * fStack_94;
  auVar35._4_4_ = auVar10._4_4_ * auVar10._4_4_ * fStack_94;
  auVar35._8_4_ = auVar10._8_4_ * auVar10._8_4_ * fStack_94;
  auVar35._12_4_ = auVar10._12_4_ * auVar10._12_4_ * fStack_94;
  auVar10 = rsqrtps(auVar35,auVar33);
  fVar23 = (3.0 - auVar35._0_4_) * auVar10._0_4_ * 0.5 * _DAT_100dea40 * fVar23;
  fVar26 = (3.0 - auVar35._4_4_) * auVar10._4_4_ * 0.5 * fRam100dea44 * fVar26;
  fVar28 = (3.0 - auVar35._8_4_) * auVar10._8_4_ * 0.5 * fRam100dea48 * fVar28;
  fStack_94 = (3.0 - auVar35._12_4_) * auVar10._12_4_ * 0.5 * fRam100dea4c * fStack_94;
  param_2[0x48] = fVar30;
  param_2[0x49] = fVar31;
  param_2[0x4a] = fVar32;
  param_2[0x4b] = fVar16 * 0.0 + fVar15 * 0.0 + fVar14 * 0.0;
  param_2[0x50] = fVar17;
  param_2[0x51] = fVar18;
  param_2[0x52] = fVar19;
  param_2[0x53] = fVar29 * 0.0 + fVar27 * 0.0 + fVar24 * 0.0;
  param_2[0x4c] = fVar20;
  param_2[0x4d] = fVar21;
  param_2[0x4e] = fVar22;
  param_2[0x4f] = fVar13 * 0.0 + fVar12 * 0.0 + fVar9 * 0.0;
  local_a0 = param_3;
  fStack_9c = param_4;
  fVar24 = fVar19 * fVar26 - fVar18 * fVar28;
  fVar27 = fVar17 * fVar28 - fVar19 * fVar23;
  fVar29 = fVar18 * fVar23 - fVar17 * fVar26;
  auVar34._0_4_ = fVar29 * fVar29 + fVar27 * fVar27 + fVar24 * fVar24;
  auVar34._4_4_ = auVar34._0_4_;
  auVar34._8_4_ = auVar34._0_4_;
  auVar34._12_4_ = auVar34._0_4_;
  auVar10 = rsqrtps(auVar34,auVar34);
  auVar36._0_4_ = auVar10._0_4_ * auVar10._0_4_ * auVar34._0_4_;
  auVar36._4_4_ = auVar10._4_4_ * auVar10._4_4_ * auVar34._0_4_;
  auVar36._8_4_ = auVar10._8_4_ * auVar10._8_4_ * auVar34._0_4_;
  auVar36._12_4_ = auVar10._12_4_ * auVar10._12_4_ * auVar34._0_4_;
  auVar10 = rsqrtps(auVar36,auVar34);
  fVar14 = *(float *)((int)this + 0x80);
  fVar15 = *(float *)((int)this + 0x84);
  fVar16 = *(float *)((int)this + 0x88);
  param_2[0x44] = fVar23;
  param_2[0x45] = fVar26;
  param_2[0x46] = fVar28;
  param_2[0x47] = fStack_94;
  param_2[0xb4] = param_3;
  param_2[0xb5] = param_4;
  param_2[0xb6] = fStack_98;
  param_2[0xb7] = fStack_94;
  fVar24 = (fVar16 * fStack_e8 + fVar15 * fStack_f8 + fVar14 * fStack_108) *
           (float)((uint)((3.0 - auVar36._8_4_) * auVar10._8_4_ * 0.5 * fVar29) &
                  -(uint)(1.1920929e-07 < auVar34._0_4_)) +
           (fVar16 * fStack_ec + fVar15 * fStack_fc + fVar14 * fStack_10c) *
           (float)((uint)((3.0 - auVar36._4_4_) * auVar10._4_4_ * 0.5 * fVar27) &
                  -(uint)(1.1920929e-07 < auVar34._0_4_)) +
           (fVar16 * local_f0 + fVar15 * local_100 + fVar14 * local_110) *
           (float)((uint)((3.0 - auVar36._0_4_) * auVar10._0_4_ * 0.5 * fVar24) &
                  -(uint)(1.1920929e-07 < auVar34._0_4_));
  local_150 = -NAN;
  fStack_14c = 0.0;
  fStack_148 = 0.0;
  fStack_144 = 0.0;
  local_120 = ZEXT416((uint)(fVar22 * fVar32 + fVar21 * fVar31 + fVar20 * fVar30));
  fVar14 = fVar24;
  fVar15 = fVar24;
  fVar16 = fVar24;
  FUN_1009d0c0();
  param_2[0x40] =
       (float)((uint)(0.0 - fVar24) & (uint)local_150 | ~(uint)local_150 & local_120._0_4_);
  param_2[0x41] =
       (float)((uint)(0.0 - fVar14) & (uint)fStack_14c | ~(uint)fStack_14c & local_120._0_4_);
  param_2[0x42] =
       (float)((uint)(0.0 - fVar15) & (uint)fStack_148 |
              ~(uint)fStack_148 & (uint)(fVar22 * fVar19 + fVar21 * fVar18 + fVar20 * fVar17));
  param_2[0x43] =
       (float)((uint)(0.0 - fVar16) & (uint)fStack_144 | ~(uint)fStack_144 & local_120._0_4_);
  FUN_1009c0f0(this,param_1,(int)param_2);
  if (*(char *)((int)this + 0x147) != '\0') {
    fVar14 = *(float *)((int)this + 0x50);
    fVar15 = *(float *)((int)this + 0x54);
    fVar16 = *(float *)((int)this + 0x58);
    iVar7 = *(int *)((int)this + 0x10);
    fVar24 = *(float *)((int)this + 0x90);
    fVar27 = *(float *)((int)this + 0x94);
    fVar29 = *(float *)((int)this + 0x98);
    local_150 = fVar16 * local_40 + local_30 + fVar15 * local_50 + fVar14 * local_60;
    fStack_14c = fVar16 * fStack_3c + fStack_2c + fVar15 * fStack_4c + fVar14 * fStack_5c;
    fStack_148 = fVar16 * fStack_38 + fStack_28 + fVar15 * fStack_48 + fVar14 * fStack_58;
    fStack_144 = fVar16 * fStack_34 + fStack_24 + fVar15 * fStack_44 + fVar14 * fStack_54;
    local_140 = fVar29 * local_f0 + local_e0 + fVar27 * local_100 + fVar24 * local_110;
    fStack_13c = fVar29 * fStack_ec + fStack_dc + fVar27 * fStack_fc + fVar24 * fStack_10c;
    fStack_138 = fVar29 * fStack_e8 + fStack_d8 + fVar27 * fStack_f8 + fVar24 * fStack_108;
    fStack_134 = fVar29 * fStack_e4 + fStack_d4 + fVar27 * fStack_f4 + fVar24 * fStack_104;
    local_20 = local_140 - local_150;
    fStack_1c = fStack_13c - fStack_14c;
    fStack_18 = fStack_138 - fStack_148;
    fStack_14 = fStack_134 - fStack_144;
    *param_2 = local_20;
    param_2[1] = fStack_1c;
    param_2[2] = fStack_18;
    param_2[3] = fStack_14;
    if (iVar7 != 0) {
      FUN_10095550(iVar7);
    }
    iVar3 = *(int *)(iVar3 + 0x94);
    iVar5 = *(int *)(iVar5 + 0x94);
    uVar2 = *puVar6;
    local_90 = 0.0;
    fStack_8c = 0.0;
    fStack_88 = 0.0;
    fStack_84 = 0.0;
    fVar14 = local_140 - *(float *)(iVar3 + 0x90);
    fVar15 = fStack_13c - *(float *)(iVar3 + 0x94);
    fVar16 = fStack_138 - *(float *)(iVar3 + 0x98);
    bVar8 = (byte)uVar2;
    local_110 = local_150 - *(float *)(iVar5 + 0x90);
    local_100 = fStack_14c - *(float *)(iVar5 + 0x94);
    fStack_148 = fStack_148 - *(float *)(iVar5 + 0x98);
    local_80 = 0.0;
    fStack_7c = 0.0;
    fStack_78 = 0.0;
    fStack_74 = 0.0;
    if (((bVar8 >> 3 | bVar8) & 2) == 0) {
      local_d0 = *(float *)(puVar6 + 0x38) * fStack_148 - *(float *)(puVar6 + 0x40) * local_100;
      local_c0 = *(float *)(puVar6 + 0x3a) * fStack_148 - *(float *)(puVar6 + 0x42) * local_100;
      local_b0 = *(float *)(puVar6 + 0x3c) * fStack_148 - *(float *)(puVar6 + 0x44) * local_100;
      local_130 = *(float *)(puVar6 + 0x30) * local_100 - *(float *)(puVar6 + 0x38) * local_110;
      fStack_12c = *(float *)(puVar6 + 0x32) * local_100 - *(float *)(puVar6 + 0x3a) * local_110;
      fStack_128 = *(float *)(puVar6 + 0x34) * local_100 - *(float *)(puVar6 + 0x3c) * local_110;
      fStack_124 = *(float *)(puVar6 + 0x36) * local_100 - *(float *)(puVar6 + 0x3e) * local_110;
      local_140 = *(float *)(puVar6 + 0x40) * local_110 - *(float *)(puVar6 + 0x30) * fStack_148;
      fStack_13c = *(float *)(puVar6 + 0x42) * local_110 - *(float *)(puVar6 + 0x32) * fStack_148;
      fStack_138 = *(float *)(puVar6 + 0x44) * local_110 - *(float *)(puVar6 + 0x34) * fStack_148;
      fStack_134 = *(float *)(puVar6 + 0x46) * local_110 - *(float *)(puVar6 + 0x36) * fStack_148;
      fStack_c4 = 0.0;
      fStack_a4 = 0.0;
      local_40 = *(float *)(puVar6 + 0x18);
      fStack_3c = *(float *)(puVar6 + 0x1a);
      fStack_38 = *(float *)(puVar6 + 0x1c);
      local_150 = *(float *)(puVar6 + 0x1e);
      local_60 = local_40 * local_d0;
      fStack_5c = fStack_3c * local_c0;
      fStack_58 = fStack_38 * local_b0;
      fStack_54 = local_150 *
                  (*(float *)(puVar6 + 0x3e) * fStack_148 - *(float *)(puVar6 + 0x46) * local_100);
      local_90 = fStack_58 * local_b0 + 0.0 + fStack_5c * local_c0 + local_60 * local_d0;
      fStack_8c = fStack_58 * fStack_138 + 0.0 + fStack_5c * fStack_13c + local_60 * local_140;
      fStack_88 = fStack_58 * fStack_128 + 0.0 + fStack_5c * fStack_12c + local_60 * local_130;
      fStack_84 = fStack_58 * 0.0 + 0.0 + fStack_5c * 0.0 + local_60 * 0.0;
      local_50 = local_40 * local_140;
      fStack_4c = fStack_3c * fStack_13c;
      fStack_48 = fStack_38 * fStack_138;
      fStack_44 = local_150 * fStack_134;
      local_40 = local_40 * local_130;
      fStack_3c = fStack_3c * fStack_12c;
      fStack_38 = fStack_38 * fStack_128;
      fStack_34 = local_150 * fStack_124;
      local_80 = fStack_48 * local_b0 + 0.0 + fStack_4c * local_c0 + local_50 * local_d0;
      fStack_7c = fStack_48 * fStack_138 + 0.0 + fStack_4c * fStack_13c + local_50 * local_140;
      fStack_78 = fStack_48 * fStack_128 + 0.0 + fStack_4c * fStack_12c + local_50 * local_130;
      fStack_74 = fStack_48 * 0.0 + 0.0 + fStack_4c * 0.0 + local_50 * 0.0;
      fStack_b4 = 0.0;
      local_70 = fStack_38 * local_b0 + 0.0 + fStack_3c * local_c0 + local_40 * local_d0;
      fStack_6c = fStack_38 * fStack_138 + 0.0 + fStack_3c * fStack_13c + local_40 * local_140;
      fStack_68 = fStack_38 * fStack_128 + 0.0 + fStack_3c * fStack_12c + local_40 * local_130;
      fStack_64 = fStack_38 * 0.0 + 0.0 + fStack_3c * 0.0 + local_40 * 0.0;
      fStack_14c = local_150;
      fStack_148 = local_150;
      fStack_144 = local_150;
      fStack_10c = local_110;
      fStack_108 = local_110;
      fStack_104 = local_110;
      fStack_fc = local_100;
      fStack_f8 = local_100;
      fStack_f4 = local_100;
      fStack_cc = local_140;
      fStack_c8 = local_130;
      fStack_bc = fStack_13c;
      fStack_b8 = fStack_12c;
      fStack_ac = fStack_138;
      fStack_a8 = fStack_128;
      FUN_1009bc30(&local_90,&local_150);
      param_2[4] = local_d0;
      param_2[5] = fStack_cc;
      param_2[6] = fStack_c8;
      param_2[7] = fStack_c4;
      param_2[8] = local_c0;
      param_2[9] = fStack_bc;
      param_2[10] = fStack_b8;
      param_2[0xb] = fStack_b4;
      param_2[0xc] = local_b0;
      param_2[0xd] = fStack_ac;
      param_2[0xe] = fStack_a8;
      param_2[0xf] = fStack_a4;
      param_2[0x1c] = local_60;
      param_2[0x1d] = fStack_5c;
      param_2[0x1e] = fStack_58;
      param_2[0x1f] = fStack_54;
      param_2[0x20] = local_50;
      param_2[0x21] = fStack_4c;
      param_2[0x22] = fStack_48;
      param_2[0x23] = fStack_44;
      param_2[0x24] = local_40;
      param_2[0x25] = fStack_3c;
      param_2[0x26] = fStack_38;
      param_2[0x27] = fStack_34;
    }
    else {
      param_2[0x24] = 0.0;
      param_2[0x25] = 0.0;
      param_2[0x26] = 0.0;
      param_2[0x27] = 0.0;
      param_2[0x20] = param_2[0x24];
      param_2[0x21] = param_2[0x25];
      param_2[0x22] = param_2[0x26];
      param_2[0x23] = param_2[0x27];
      param_2[0x1c] = param_2[0x24];
      param_2[0x1d] = param_2[0x25];
      param_2[0x1e] = param_2[0x26];
      param_2[0x1f] = param_2[0x27];
      param_2[0xc] = 0.0;
      param_2[0xd] = 0.0;
      param_2[0xe] = 0.0;
      param_2[0xf] = 0.0;
      param_2[8] = param_2[0xc];
      param_2[9] = param_2[0xd];
      param_2[10] = param_2[0xe];
      param_2[0xb] = param_2[0xf];
      param_2[4] = param_2[0xc];
      param_2[5] = param_2[0xd];
      param_2[6] = param_2[0xe];
      param_2[7] = param_2[0xf];
      local_70 = local_90;
      fStack_6c = fStack_8c;
      fStack_68 = fStack_88;
      fStack_64 = fStack_84;
    }
    if ((((byte)*puVar4 >> 3 | (byte)*puVar4) & 2) == 0) {
      local_d0 = fVar15 * *(float *)(puVar4 + 0x40) - fVar16 * *(float *)(puVar4 + 0x38);
      local_c0 = fVar15 * *(float *)(puVar4 + 0x42) - fVar16 * *(float *)(puVar4 + 0x3a);
      local_b0 = fVar15 * *(float *)(puVar4 + 0x44) - fVar16 * *(float *)(puVar4 + 0x3c);
      fStack_54 = fVar15 * *(float *)(puVar4 + 0x46) - fVar16 * *(float *)(puVar4 + 0x3e);
      fStack_cc = fVar16 * *(float *)(puVar4 + 0x30) - fVar14 * *(float *)(puVar4 + 0x40);
      fStack_bc = fVar16 * *(float *)(puVar4 + 0x32) - fVar14 * *(float *)(puVar4 + 0x42);
      fStack_ac = fVar16 * *(float *)(puVar4 + 0x34) - fVar14 * *(float *)(puVar4 + 0x44);
      fStack_c8 = fVar14 * *(float *)(puVar4 + 0x38) - fVar15 * *(float *)(puVar4 + 0x30);
      fStack_b8 = fVar14 * *(float *)(puVar4 + 0x3a) - fVar15 * *(float *)(puVar4 + 0x32);
      fStack_a8 = fVar14 * *(float *)(puVar4 + 0x3c) - fVar15 * *(float *)(puVar4 + 0x34);
      fStack_34 = fVar14 * *(float *)(puVar4 + 0x3e) - fVar15 * *(float *)(puVar4 + 0x36);
      fStack_c4 = 0.0;
      fStack_b4 = 0.0;
      fStack_a4 = 0.0;
      local_f0 = *(float *)(puVar4 + 0x18);
      fStack_ec = *(float *)(puVar4 + 0x1a);
      fStack_e8 = *(float *)(puVar4 + 0x1c);
      local_150 = *(float *)(puVar4 + 0x1e);
      local_110 = local_f0 * local_d0;
      fStack_10c = fStack_ec * local_c0;
      fStack_108 = fStack_e8 * local_b0;
      fStack_104 = local_150 * fStack_54;
      local_100 = local_f0 * fStack_cc;
      fStack_fc = fStack_ec * fStack_bc;
      fStack_f8 = fStack_e8 * fStack_ac;
      fStack_f4 = local_150 *
                  (fVar16 * *(float *)(puVar4 + 0x36) - fVar14 * *(float *)(puVar4 + 0x46));
      local_90 = fStack_108 * local_b0 + local_90 + fStack_10c * local_c0 + local_110 * local_d0;
      fStack_8c = fStack_108 * fStack_ac + fStack_8c + fStack_10c * fStack_bc +
                  local_110 * fStack_cc;
      fStack_88 = fStack_108 * fStack_a8 + fStack_88 + fStack_10c * fStack_b8 +
                  local_110 * fStack_c8;
      fStack_84 = fStack_108 * 0.0 + fStack_84 + fStack_10c * 0.0 + local_110 * 0.0;
      local_f0 = local_f0 * fStack_c8;
      fStack_ec = fStack_ec * fStack_b8;
      fStack_e8 = fStack_e8 * fStack_a8;
      fStack_e4 = local_150 * fStack_34;
      local_80 = fStack_f8 * local_b0 + local_80 + fStack_fc * local_c0 + local_100 * local_d0;
      fStack_7c = fStack_f8 * fStack_ac + fStack_7c + fStack_fc * fStack_bc + local_100 * fStack_cc;
      fStack_78 = fStack_f8 * fStack_a8 + fStack_78 + fStack_fc * fStack_b8 + local_100 * fStack_c8;
      fStack_74 = fStack_f8 * 0.0 + fStack_74 + fStack_fc * 0.0 + local_100 * 0.0;
      local_70 = fStack_e8 * local_b0 + local_70 + fStack_ec * local_c0 + local_f0 * local_d0;
      fStack_6c = fStack_e8 * fStack_ac + fStack_6c + fStack_ec * fStack_bc + local_f0 * fStack_cc;
      fStack_68 = fStack_e8 * fStack_a8 + fStack_68 + fStack_ec * fStack_b8 + local_f0 * fStack_c8;
      fStack_64 = fStack_e8 * 0.0 + fStack_64 + fStack_ec * 0.0 + local_f0 * 0.0;
      fStack_14c = local_150;
      fStack_148 = local_150;
      fStack_144 = local_150;
      local_60 = local_d0;
      fStack_5c = local_c0;
      fStack_58 = local_b0;
      local_40 = fStack_c8;
      fStack_3c = fStack_b8;
      fStack_38 = fStack_a8;
      FUN_1009bc30(&local_90,&local_150);
      param_2[0x10] = local_d0;
      param_2[0x11] = fStack_cc;
      param_2[0x12] = fStack_c8;
      param_2[0x13] = fStack_c4;
      param_2[0x14] = local_c0;
      param_2[0x15] = fStack_bc;
      param_2[0x16] = fStack_b8;
      param_2[0x17] = fStack_b4;
      param_2[0x18] = local_b0;
      param_2[0x19] = fStack_ac;
      param_2[0x1a] = fStack_a8;
      param_2[0x1b] = fStack_a4;
      param_2[0x28] = local_110;
      param_2[0x29] = fStack_10c;
      param_2[0x2a] = fStack_108;
      param_2[0x2b] = fStack_104;
      param_2[0x2c] = local_100;
      param_2[0x2d] = fStack_fc;
      param_2[0x2e] = fStack_f8;
      param_2[0x2f] = fStack_f4;
      param_2[0x30] = local_f0;
      param_2[0x31] = fStack_ec;
      param_2[0x32] = fStack_e8;
      param_2[0x33] = fStack_e4;
    }
    else {
      param_2[0x30] = 0.0;
      param_2[0x31] = 0.0;
      param_2[0x32] = 0.0;
      param_2[0x33] = 0.0;
      param_2[0x2c] = param_2[0x30];
      param_2[0x2d] = param_2[0x31];
      param_2[0x2e] = param_2[0x32];
      param_2[0x2f] = param_2[0x33];
      param_2[0x28] = param_2[0x30];
      param_2[0x29] = param_2[0x31];
      param_2[0x2a] = param_2[0x32];
      param_2[0x2b] = param_2[0x33];
      param_2[0x18] = 0.0;
      param_2[0x19] = 0.0;
      param_2[0x1a] = 0.0;
      param_2[0x1b] = 0.0;
      param_2[0x14] = param_2[0x18];
      param_2[0x15] = param_2[0x19];
      param_2[0x16] = param_2[0x1a];
      param_2[0x17] = param_2[0x1b];
      param_2[0x10] = param_2[0x18];
      param_2[0x11] = param_2[0x19];
      param_2[0x12] = param_2[0x1a];
      param_2[0x13] = param_2[0x1b];
    }
    fVar26 = fStack_78 * fStack_8c - fStack_7c * fStack_88;
    fVar28 = local_80 * fStack_88 - fStack_78 * local_90;
    fVar17 = fStack_7c * local_90 - local_80 * fStack_8c;
    fVar14 = fStack_68 * fVar17 + fStack_6c * fVar28 + local_70 * fVar26;
    auVar11._0_8_ = CONCAT44(fVar14,fVar14);
    auVar11._8_4_ = fVar14;
    auVar11._12_4_ = fVar14;
    auVar25._8_4_ = fVar14;
    auVar25._0_8_ = auVar11._0_8_;
    auVar25._12_4_ = fVar14;
    auVar10 = rcpps(auVar11,auVar25);
    fVar15 = auVar10._0_4_;
    fVar16 = auVar10._4_4_;
    fVar24 = auVar10._8_4_;
    fVar27 = auVar10._12_4_;
    fVar9 = (fVar15 + fVar15) - fVar15 * fVar15 * fVar14;
    fVar12 = (fVar16 + fVar16) - fVar16 * fVar16 * fVar14;
    fVar13 = (fVar24 + fVar24) - fVar24 * fVar24 * fVar14;
    fVar23 = (fVar27 + fVar27) - fVar27 * fVar27 * fVar14;
    fVar14 = *(float *)((int)this + 0x13c);
    param_2[0x38] = (fStack_6c * fStack_88 - fStack_68 * fStack_8c) * fVar9;
    param_2[0x39] = (fStack_68 * local_90 - local_70 * fStack_88) * fVar12;
    param_2[0x3a] = (local_70 * fStack_8c - fStack_6c * local_90) * fVar13;
    param_2[0x3b] = (local_70 * fStack_8c - fStack_6c * local_90) * fVar23;
    fVar24 = *(float *)(puVar6 + 0x60);
    fVar27 = *(float *)(puVar6 + 0x62);
    fVar29 = *(float *)(puVar6 + 100);
    param_2[0x34] = (fStack_68 * fStack_7c - fStack_6c * fStack_78) * fVar9;
    param_2[0x35] = (local_70 * fStack_78 - fStack_68 * local_80) * fVar12;
    param_2[0x36] = (fStack_6c * local_80 - local_70 * fStack_7c) * fVar13;
    param_2[0x37] = (fStack_6c * local_80 - local_70 * fStack_7c) * fVar23;
    fVar15 = param_1[1];
    fVar16 = *(float *)((int)this + 0x140);
    param_2[0x3c] = fVar9 * fVar26;
    param_2[0x3d] = fVar12 * fVar28;
    param_2[0x3e] = fVar13 * fVar17;
    param_2[0x3f] = fVar23 * (fStack_7c * local_90 - local_80 * fStack_8c);
    fVar9 = *(float *)(puVar4 + 0x60);
    fVar12 = *(float *)(puVar4 + 0x62);
    fVar13 = *(float *)(puVar4 + 100);
    fVar15 = fVar16 * local_a0 * fVar15;
    pfVar1 = (float *)(puVar4 + 0x68);
    fVar16 = (0.0 - (((fVar29 * param_2[0xc] + *(float *)(puVar6 + 0x68) + fVar27 * param_2[8] +
                      fVar24 * param_2[4]) - *pfVar1) + fVar13 * param_2[0x18] +
                     fVar12 * param_2[0x14] + fVar9 * param_2[0x10]) * fVar14 * fStack_9c) +
             fVar15 * local_20;
    fVar23 = (0.0 - (((fVar29 * param_2[0xd] + *(float *)(puVar6 + 0x6a) + fVar27 * param_2[9] +
                      fVar24 * param_2[5]) - *(float *)(puVar4 + 0x6a)) + fVar13 * param_2[0x19] +
                     fVar12 * param_2[0x15] + fVar9 * param_2[0x11]) * fVar14 * fStack_9c) +
             fVar15 * fStack_1c;
    fVar9 = (0.0 - (((fVar29 * param_2[0xe] + *(float *)(puVar6 + 0x6c) + fVar27 * param_2[10] +
                     fVar24 * param_2[6]) - *(float *)(puVar4 + 0x6c)) + fVar13 * param_2[0x1a] +
                    fVar12 * param_2[0x16] + fVar9 * param_2[0x12]) * fVar14 * fStack_9c) +
            fVar15 * fStack_18;
    fVar14 = fVar9 * param_2[0x3c] + fVar23 * param_2[0x38] + fVar16 * param_2[0x34];
    fVar15 = fVar9 * param_2[0x3d] + fVar23 * param_2[0x39] + fVar16 * param_2[0x35];
    fVar16 = fVar9 * param_2[0x3e] + fVar23 * param_2[0x3a] + fVar16 * param_2[0x36];
    if ((uVar2 & 0x12) == 0) {
      fVar9 = param_2[0x25];
      fVar12 = param_2[0x26];
      fVar13 = param_2[0x21];
      fVar23 = param_2[0x22];
      fVar26 = *(float *)(puVar6 + 0x1e);
      fVar28 = param_2[0x1d];
      fVar17 = param_2[0x1e];
      *(float *)(puVar6 + 0x60) =
           fVar16 * param_2[0x24] + fVar15 * param_2[0x20] + fVar14 * param_2[0x1c] + fVar24;
      *(float *)(puVar6 + 0x62) = fVar16 * fVar9 + fVar15 * fVar13 + fVar14 * fVar28 + fVar27;
      *(float *)(puVar6 + 100) = fVar16 * fVar12 + fVar15 * fVar23 + fVar14 * fVar17 + fVar29;
      puVar6[0x66] = 0;
      puVar6[0x67] = 0;
      *(float *)(puVar6 + 0x68) = fVar26 * fVar14 + *(float *)(puVar6 + 0x68);
      *(float *)(puVar6 + 0x6a) = fVar26 * fVar15 + *(float *)(puVar6 + 0x6a);
      *(float *)(puVar6 + 0x6c) = fVar26 * fVar16 + *(float *)(puVar6 + 0x6c);
      puVar6[0x6e] = 0;
      puVar6[0x6f] = 0;
    }
    if ((*puVar4 & 0x12) == 0) {
      fVar24 = param_2[0x31];
      fVar27 = param_2[0x32];
      fVar29 = param_2[0x2d];
      fVar9 = param_2[0x2e];
      fVar12 = *(float *)(puVar4 + 0x1e);
      fVar13 = param_2[0x29];
      fVar23 = param_2[0x2a];
      *(float *)(puVar4 + 0x60) =
           fVar16 * param_2[0x30] + fVar15 * param_2[0x2c] + fVar14 * param_2[0x28] +
           *(float *)(puVar4 + 0x60);
      *(float *)(puVar4 + 0x62) =
           fVar16 * fVar24 + fVar15 * fVar29 + fVar14 * fVar13 + *(float *)(puVar4 + 0x62);
      *(float *)(puVar4 + 100) =
           fVar16 * fVar27 + fVar15 * fVar9 + fVar14 * fVar23 + *(float *)(puVar4 + 100);
      puVar4[0x66] = 0;
      puVar4[0x67] = 0;
      *pfVar1 = fVar12 * (0.0 - fVar14) + *pfVar1;
      *(float *)(puVar4 + 0x6a) = fVar12 * (0.0 - fVar15) + *(float *)(puVar4 + 0x6a);
      *(float *)(puVar4 + 0x6c) = fVar12 * (0.0 - fVar16) + *(float *)(puVar4 + 0x6c);
      puVar4[0x6e] = 0;
      puVar4[0x6f] = 0;
    }
  }
  return 0x2e0;
}


// 1009e9c0 FUN_1009e9c0

void __thiscall FUN_1009e9c0(void *this,undefined4 *param_1)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  
  param_1[0x11] = *(undefined4 *)((int)this + 100);
  param_1[0x10] = *(undefined4 *)((int)this + 0x60);
  puVar2 = (undefined4 *)((int)this + 0x20);
  puVar3 = param_1;
  for (iVar1 = 0xc; iVar1 != 0; iVar1 = iVar1 + -1) {
    *puVar3 = *puVar2;
    puVar2 = puVar2 + 1;
    puVar3 = puVar3 + 1;
  }
  param_1[0xc] = *(undefined4 *)((int)this + 0x50);
  param_1[0xd] = *(undefined4 *)((int)this + 0x54);
  param_1[0xe] = *(undefined4 *)((int)this + 0x58);
  return;
}


// 1009ea10 FUN_1009ea10

void __thiscall FUN_1009ea10(void *this,undefined4 *param_1)

{
  int iVar1;
  undefined4 *puVar2;
  undefined4 *puVar3;
  
  *(undefined4 *)((int)this + 100) = param_1[0x11];
  *(undefined4 *)((int)this + 0x60) = param_1[0x10];
  puVar2 = param_1;
  puVar3 = (undefined4 *)((int)this + 0x20);
  for (iVar1 = 0xc; iVar1 != 0; iVar1 = iVar1 + -1) {
    *puVar3 = *puVar2;
    puVar2 = puVar2 + 1;
    puVar3 = puVar3 + 1;
  }
  *(undefined4 *)((int)this + 0x50) = param_1[0xc];
  *(undefined4 *)((int)this + 0x54) = param_1[0xd];
  *(undefined4 *)((int)this + 0x58) = param_1[0xe];
  return;
}


// 1009ea50 FUN_1009ea50

undefined4 * __thiscall
FUN_1009ea50(void *this,undefined4 param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4
            )

{
  FUN_100a4de0(this,param_1,param_3,param_4);
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined ***)this = &PTR_LAB_100ce7ac;
  FUN_1009ea10(this,param_2);
  return (undefined4 *)this;
}


// 1009eaa0 FUN_1009eaa0

undefined4 * __thiscall
FUN_1009eaa0(void *this,void *param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4)

{
  FUN_100a4e00(this,param_1,param_3,param_4,0x195,0x440);
  *(undefined ***)this = &PTR_LAB_100ce7ac;
  FUN_1009ea10(this,param_2);
  return (undefined4 *)this;
}


// 1009eaf0 FUN_1009eaf0

void __thiscall FUN_1009eaf0(void *this,undefined4 param_1,int param_2,float param_3,float param_4)

{
  float *pfVar1;
  float *pfVar2;
  float *pfVar3;
  float *pfVar4;
  float *pfVar5;
  float fVar6;
  int iVar7;
  int iVar8;
  float *pfVar9;
  float *pfVar10;
  int iVar11;
  int iVar12;
  float fVar13;
  undefined1 *local_70;
  float local_6c [3];
  undefined1 local_60 [32];
  undefined1 *local_40;
  undefined4 local_3c;
  undefined4 local_38;
  undefined1 local_30 [44];
  
  iVar7 = *(int *)((int)this + 8);
  iVar8 = *(int *)((int)this + 4);
  iVar11 = *(int *)(iVar8 + 0x94);
  *(undefined4 *)(param_2 + 0x3c) = 0;
  *(undefined4 *)(param_2 + 0x38) = 0;
  *(undefined4 *)(param_2 + 0x34) = 0;
  *(undefined4 *)(param_2 + 0x30) = 0;
  *(undefined4 *)(param_2 + 0x4c) = 0;
  *(undefined4 *)(param_2 + 0x48) = 0;
  *(undefined4 *)(param_2 + 0x44) = 0;
  *(undefined4 *)(param_2 + 0x40) = 0;
  pfVar9 = *(float **)(param_2 + 0x424);
  iVar12 = *(int *)(param_2 + 0x420) + -1;
  pfVar10 = pfVar9 + 5;
  do {
    pfVar1 = pfVar10 + -4;
    pfVar2 = pfVar10 + -3;
    pfVar3 = pfVar10 + -1;
    pfVar4 = pfVar10 + 3;
    fVar13 = *pfVar9;
    pfVar5 = pfVar10 + 1;
    fVar6 = *pfVar10;
    pfVar9 = pfVar9 + 0xc;
    pfVar10 = pfVar10 + 0xc;
    iVar12 = iVar12 + -1;
    *(float *)(param_2 + 0x30 + (int)*pfVar4 * 4) =
         *pfVar1 * *(float *)(iVar11 + 0xd4) + *pfVar2 * *(float *)(iVar11 + 0xd8) +
         fVar13 * *(float *)(iVar11 + 0xd0) +
         *pfVar5 * *(float *)(iVar11 + 200) + *pfVar3 * *(float *)(iVar11 + 0xc0) +
         *(float *)(iVar11 + 0xc4) * fVar6 + *(float *)(param_2 + 0x30 + (int)*pfVar4 * 4);
  } while (-1 < iVar12);
  iVar11 = *(int *)(iVar7 + 0x94);
  pfVar9 = *(float **)(param_2 + 0x430);
  iVar12 = *(int *)(param_2 + 0x42c) + -1;
  pfVar10 = pfVar9 + 5;
  do {
    pfVar1 = pfVar10 + -4;
    pfVar2 = pfVar10 + -3;
    pfVar3 = pfVar10 + -1;
    pfVar4 = pfVar10 + 3;
    fVar13 = *pfVar9;
    pfVar5 = pfVar10 + 1;
    fVar6 = *pfVar10;
    pfVar9 = pfVar9 + 0xc;
    pfVar10 = pfVar10 + 0xc;
    iVar12 = iVar12 + -1;
    *(float *)(param_2 + 0x30 + (int)*pfVar4 * 4) =
         *pfVar1 * *(float *)(iVar11 + 0xd4) + *pfVar2 * *(float *)(iVar11 + 0xd8) +
         fVar13 * *(float *)(iVar11 + 0xd0) +
         *pfVar5 * *(float *)(iVar11 + 200) + *pfVar3 * *(float *)(iVar11 + 0xc0) +
         *(float *)(iVar11 + 0xc4) * fVar6 + *(float *)(param_2 + 0x30 + (int)*pfVar4 * 4);
  } while (-1 < iVar12);
  fVar13 = *(float *)((int)this + 0x60) * param_4 * -1.0;
  local_70 = local_60;
  local_6c[0] = 8.40779e-45;
  local_6c[1] = 1.12104e-44;
  iVar11 = 5;
  pfVar10 = (float *)(param_2 + 0x20);
  do {
    fVar6 = *pfVar10;
    *(float *)(((int)local_6c - param_2) + (int)pfVar10) =
         *(float *)(param_2 + 0x30 + iVar11 * 4) * fVar13 + pfVar10[1] * param_3;
    *(float *)(((int)&local_70 - param_2) + (int)pfVar10) =
         *(float *)(param_2 + 0x2c + iVar11 * 4) * fVar13 + fVar6 * param_3;
    iVar11 = iVar11 + -2;
    pfVar10 = pfVar10 + -2;
  } while (0 < iVar11);
  local_40 = local_30;
  local_3c = 6;
  local_38 = 8;
  FUN_100ba040((int *)(param_2 + 0x350),(int *)&local_70,(int *)&local_40);
  FUN_100a53e0(*(void **)(iVar8 + 0x94),(int *)(param_2 + 0x420),(int)local_40);
  FUN_100a53e0(*(void **)(iVar7 + 0x94),(int *)(param_2 + 0x42c),(int)local_40);
  return;
}


// 1009ed70 FUN_1009ed70

void __fastcall FUN_1009ed70(undefined4 *param_1)

{
  int iVar1;
  
  param_1[0x101] = param_1 + 0x20;
  param_1[0xfe] = param_1 + 8;
  param_1[0x100] = param_1 + 0x80;
  param_1[0xfd] = param_1 + 0x38;
  param_1[0x102] = 0;
  param_1[0x103] = 0;
  iVar1 = param_1[0xcb] * param_1[0xc9] + -1;
  if (3 < (int)(param_1[0xcb] * param_1[0xc9])) {
    do {
      *(undefined4 *)(param_1[200] + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[200] + -4 + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[200] + -8 + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[200] + -0xc + iVar1 * 4) = 0;
      iVar1 = iVar1 + -4;
    } while (2 < iVar1);
  }
  while (-1 < iVar1) {
    iVar1 = iVar1 + -1;
    *(undefined4 *)(param_1[200] + 4 + iVar1 * 4) = 0;
  }
  param_1[3] = 0;
  param_1[2] = 0;
  param_1[1] = 0;
  *param_1 = 0;
  param_1[7] = 0;
  param_1[6] = 0;
  param_1[5] = 0;
  param_1[4] = 0;
  return;
}


// 1009ee50 FUN_1009ee50

undefined4 __thiscall FUN_1009ee50(void *this,int param_1,int *param_2,float param_3,float param_4)

{
  int *piVar1;
  int *piVar2;
  int *piVar3;
  undefined4 *puVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  int iVar10;
  int iVar11;
  int iVar12;
  float *pfVar13;
  int iVar14;
  int *piVar15;
  float *pfVar16;
  float fVar17;
  float fVar18;
  float fVar19;
  float fVar20;
  int local_1a4;
  int *local_198;
  float local_190;
  float local_18c;
  float local_188;
  undefined4 local_184;
  float local_180;
  float local_17c;
  float local_178;
  undefined4 local_174;
  float local_170;
  float local_16c;
  float local_168;
  undefined4 local_164;
  float local_160;
  float local_15c;
  float local_158;
  undefined4 local_154;
  float local_150 [4];
  float local_140;
  float local_13c;
  float local_138;
  float local_130;
  float local_12c;
  float local_128;
  float local_120;
  float local_11c;
  float local_118;
  float local_110;
  float local_10c;
  float local_108;
  float local_100;
  float local_fc;
  float local_f8;
  float local_f0;
  float local_ec;
  float local_e8;
  float local_e0;
  float local_dc;
  float local_d8;
  float local_d0;
  float local_cc;
  float local_c8;
  float local_c0;
  float local_b8;
  float local_b0 [4];
  float local_a0;
  float local_9c;
  float local_90;
  float local_8c;
  float local_80;
  float local_7c;
  float local_78;
  undefined1 *local_70;
  float local_6c [3];
  undefined1 local_60 [32];
  undefined1 *local_40;
  undefined4 local_3c;
  undefined4 local_38;
  undefined1 local_30 [44];
  
  iVar10 = *(int *)((int)this + 4);
  iVar11 = *(int *)((int)this + 8);
  iVar12 = *(int *)(iVar10 + 0x94);
  if ((*(uint *)(iVar10 + 0x44) & 0x800) == 0) {
    pfVar13 = (float *)(iVar12 + 0x60);
    pfVar16 = local_150;
    for (iVar14 = 0x10; iVar14 != 0; iVar14 = iVar14 + -1) {
      *pfVar16 = *pfVar13;
      pfVar13 = pfVar13 + 1;
      pfVar16 = pfVar16 + 1;
    }
    FUN_10039500(local_150,(float *)(iVar10 + 0x30),&local_120);
    local_190 = local_150[0];
    local_180 = local_150[1];
    local_18c = local_140;
    local_17c = local_13c;
    local_188 = local_130;
    local_178 = local_12c;
    local_170 = local_150[2];
    local_160 = local_120;
    local_16c = local_138;
    local_15c = local_11c;
    local_168 = local_128;
  }
  else {
    local_190 = *(float *)(iVar12 + 0x60);
    local_18c = *(float *)(iVar12 + 0x70);
    local_188 = *(float *)(iVar12 + 0x80);
    local_180 = *(float *)(iVar12 + 100);
    local_17c = *(float *)(iVar12 + 0x74);
    local_178 = *(float *)(iVar12 + 0x84);
    local_170 = *(float *)(iVar12 + 0x68);
    local_16c = *(float *)(iVar12 + 0x78);
    local_168 = *(float *)(iVar12 + 0x88);
    local_160 = *(float *)(iVar12 + 0x90);
    local_15c = *(float *)(iVar12 + 0x94);
    local_118 = *(float *)(iVar12 + 0x98);
  }
  local_164 = 0;
  local_174 = 0;
  local_184 = 0;
  local_154 = 0x3f800000;
  iVar12 = *(int *)(iVar11 + 0x94);
  local_158 = local_118;
  if ((*(uint *)(iVar11 + 0x44) & 0x800) == 0) {
    pfVar13 = (float *)(iVar12 + 0x60);
    pfVar16 = local_b0;
    for (iVar14 = 0x10; iVar14 != 0; iVar14 = iVar14 + -1) {
      *pfVar16 = *pfVar13;
      pfVar13 = pfVar13 + 1;
      pfVar16 = pfVar16 + 1;
    }
    FUN_10039500(local_b0,(float *)(iVar11 + 0x30),&local_80);
    local_150[0] = local_b0[0];
    local_150[1] = local_a0;
    local_150[2] = local_90;
    local_140 = local_b0[1];
    local_13c = local_9c;
    local_138 = local_8c;
    local_120 = local_80;
    local_11c = local_7c;
  }
  else {
    local_150[0] = *(float *)(iVar12 + 0x60);
    local_150[1] = *(float *)(iVar12 + 0x70);
    local_150[2] = *(float *)(iVar12 + 0x80);
    local_140 = *(float *)(iVar12 + 100);
    local_13c = *(float *)(iVar12 + 0x74);
    local_138 = *(float *)(iVar12 + 0x84);
    local_120 = *(float *)(iVar12 + 0x90);
    local_11c = *(float *)(iVar12 + 0x94);
    local_78 = *(float *)(iVar12 + 0x98);
  }
  local_118 = local_78;
  FUN_100babc0(&local_100,&local_190,(float *)((int)this + 0x20));
  fVar18 = *(float *)((int)this + 0x50);
  fVar17 = *(float *)((int)this + 0x54);
  fVar5 = *(float *)((int)this + 0x58);
  local_110 = fVar18 * local_190 + local_180 * fVar17 + local_170 * fVar5 + local_160;
  local_10c = local_17c * fVar17 + fVar18 * local_18c + local_16c * fVar5 + local_15c;
  local_108 = fVar18 * local_188 + local_178 * fVar17 + local_168 * fVar5 + local_158;
  if (param_2 == (int *)0x0) {
    local_198 = (int *)0x0;
  }
  else {
    *param_2 = (int)(param_2 + 4);
    param_2[1] = 6;
    param_2[2] = 8;
    param_2[0xd4] = (int)(param_2 + 0xd8);
    param_2[0xd5] = 6;
    param_2[0xd6] = 6;
    param_2[0xd7] = 8;
    local_198 = param_2;
  }
  piVar15 = local_198 + 0xc;
  FUN_1009ed70(piVar15);
  iVar12 = *(int *)(iVar10 + 0x94);
  iVar14 = *(int *)(iVar11 + 0x94);
  fVar18 = *(float *)(iVar14 + 0x94);
  local_d0 = local_110 - *(float *)(iVar12 + 0x90);
  local_cc = local_10c - *(float *)(iVar12 + 0x94);
  local_c8 = local_108 - *(float *)(iVar12 + 0x98);
  puVar4 = (undefined4 *)(local_198[0x10e] + 0xe0 + (int)piVar15);
  local_c0 = local_110 - *(float *)(iVar14 + 0x90);
  local_b8 = local_108 - *(float *)(iVar14 + 0x98);
  puVar4[2] = 0;
  puVar4[1] = 0;
  *puVar4 = 0x3f800000;
  puVar4[5] = local_c8;
  puVar4[4] = 0;
  fVar19 = -0.0 - local_cc;
  puVar4[6] = fVar19;
  iVar12 = *(int *)(iVar10 + 0x94);
  fVar17 = (float)puVar4[5];
  fVar5 = *(float *)(iVar12 + 0x78);
  fVar18 = local_10c - fVar18;
  fVar6 = *(float *)(iVar12 + 0x68);
  fVar7 = *(float *)(iVar12 + 0x60);
  fVar8 = *(float *)(iVar12 + 0x88);
  fVar9 = *(float *)(iVar12 + 0x70);
  fVar20 = *(float *)(iVar12 + 0x80);
  puVar4[5] = *(float *)(iVar12 + 100) * 0.0 + *(float *)(iVar12 + 0x74) * fVar17 +
              *(float *)(iVar12 + 0x84) * fVar19;
  puVar4[6] = fVar6 * 0.0 + fVar5 * fVar17 + fVar8 * fVar19;
  puVar4[4] = fVar9 * fVar17 + fVar20 * fVar19 + fVar7 * 0.0;
  puVar4[8] = local_198[0x10f];
  iVar12 = local_198[0x10e];
  *(undefined4 *)(iVar12 + 0x208 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x204 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x200 + (int)piVar15) = 0xbf800000;
  *(undefined4 *)(iVar12 + 0x210 + (int)piVar15) = 0;
  *(float *)(iVar12 + 0x218 + (int)piVar15) = fVar18;
  fVar19 = local_b8 * -1.0;
  *(float *)((int)piVar15 + iVar12 + 0x214) = fVar19;
  iVar14 = *(int *)(iVar11 + 0x94);
  fVar17 = *(float *)(iVar14 + 0x68);
  fVar5 = *(float *)(iVar14 + 0x78);
  fVar6 = *(float *)(iVar14 + 0x60);
  fVar7 = *(float *)(iVar14 + 0x88);
  fVar8 = *(float *)(iVar14 + 0x70);
  fVar9 = *(float *)((int)piVar15 + iVar12 + 0x214);
  fVar20 = *(float *)(iVar14 + 0x80);
  *(float *)((int)piVar15 + iVar12 + 0x214) =
       *(float *)(iVar14 + 100) * 0.0 + *(float *)(iVar14 + 0x74) * fVar19 +
       *(float *)(iVar14 + 0x84) * fVar18;
  *(float *)((int)piVar15 + iVar12 + 0x210) = fVar8 * fVar9 + fVar20 * fVar18 + fVar6 * 0.0;
  *(float *)((int)piVar15 + iVar12 + 0x218) = fVar17 * 0.0 + fVar5 * fVar19 + fVar7 * fVar18;
  *(int *)((int)piVar15 + iVar12 + 0x220) = local_198[0x10f];
  iVar12 = local_198[0x10e];
  *(undefined4 *)(iVar12 + 0x118 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x110 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x114 + (int)piVar15) = 0x3f800000;
  *(float *)((int)piVar15 + iVar12 + 0x120) = -0.0 - local_c8;
  *(undefined4 *)((int)piVar15 + iVar12 + 0x124) = 0;
  *(float *)((int)piVar15 + iVar12 + 0x128) = local_d0;
  iVar14 = *(int *)(iVar10 + 0x94);
  fVar17 = *(float *)((int)piVar15 + iVar12 + 0x120);
  fVar5 = *(float *)(iVar14 + 0x68);
  fVar6 = *(float *)(iVar14 + 0x78);
  fVar7 = *(float *)((int)piVar15 + iVar12 + 0x128);
  fVar8 = *(float *)(iVar14 + 0x88);
  fVar9 = *(float *)(iVar14 + 0x60);
  fVar20 = *(float *)(iVar14 + 0x70);
  fVar19 = *(float *)(iVar14 + 0x80);
  *(float *)((int)piVar15 + iVar12 + 0x124) =
       *(float *)(iVar14 + 100) * fVar17 + *(float *)(iVar14 + 0x74) * 0.0 +
       *(float *)(iVar14 + 0x84) * local_d0;
  *(float *)((int)piVar15 + iVar12 + 0x120) = fVar20 * 0.0 + fVar19 * fVar7 + fVar17 * fVar9;
  *(float *)((int)piVar15 + iVar12 + 0x128) = fVar5 * fVar17 + fVar6 * 0.0 + fVar8 * fVar7;
  *(int *)((int)piVar15 + iVar12 + 0x130) = local_198[0x10f] + 1;
  iVar12 = local_198[0x10e];
  *(undefined4 *)(iVar12 + 0x238 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x230 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x234 + (int)piVar15) = 0xbf800000;
  *(float *)(iVar12 + 0x240 + (int)piVar15) = local_b8;
  *(undefined4 *)(iVar12 + 0x244 + (int)piVar15) = 0;
  fVar19 = local_c0 * -1.0;
  *(float *)((int)piVar15 + iVar12 + 0x248) = fVar19;
  iVar14 = *(int *)(iVar11 + 0x94);
  fVar17 = *(float *)((int)piVar15 + iVar12 + 0x240);
  fVar5 = *(float *)(iVar14 + 0x68);
  fVar6 = *(float *)(iVar14 + 0x78);
  fVar7 = *(float *)(iVar14 + 0x88);
  fVar8 = *(float *)(iVar14 + 0x70);
  fVar9 = *(float *)(iVar14 + 0x80);
  fVar20 = *(float *)(iVar14 + 0x60);
  *(float *)((int)piVar15 + iVar12 + 0x244) =
       *(float *)(iVar14 + 100) * fVar17 + *(float *)(iVar14 + 0x74) * 0.0 +
       *(float *)(iVar14 + 0x84) * fVar19;
  *(float *)((int)piVar15 + iVar12 + 0x248) = fVar5 * fVar17 + fVar6 * 0.0 + fVar7 * fVar19;
  *(float *)((int)piVar15 + iVar12 + 0x240) =
       fVar8 * 0.0 + fVar9 * fVar19 + fVar20 * *(float *)((int)piVar15 + iVar12 + 0x240);
  *(int *)((int)piVar15 + iVar12 + 0x250) = local_198[0x10f] + 1;
  puVar4 = (undefined4 *)(local_198[0x10e] + 0x140 + (int)piVar15);
  puVar4[1] = 0;
  *puVar4 = 0;
  puVar4[2] = 0x3f800000;
  puVar4[4] = local_cc;
  fVar20 = -0.0 - local_d0;
  puVar4[5] = fVar20;
  puVar4[6] = 0;
  iVar12 = *(int *)(iVar10 + 0x94);
  fVar17 = *(float *)(iVar12 + 0x68);
  fVar5 = *(float *)(iVar12 + 0x78);
  fVar6 = *(float *)(iVar12 + 0x88);
  fVar7 = *(float *)(iVar12 + 0x70);
  fVar8 = *(float *)(iVar12 + 0x80);
  fVar9 = *(float *)(iVar12 + 0x60);
  puVar4[5] = *(float *)(iVar12 + 100) * (float)puVar4[4] + *(float *)(iVar12 + 0x74) * fVar20 +
              *(float *)(iVar12 + 0x84) * 0.0;
  puVar4[6] = fVar17 * (float)puVar4[4] + fVar5 * fVar20 + fVar6 * 0.0;
  puVar4[4] = fVar7 * fVar20 + fVar8 * 0.0 + fVar9 * (float)puVar4[4];
  puVar4[8] = local_198[0x10f] + 2;
  iVar12 = local_198[0x10e];
  *(undefined4 *)(iVar12 + 0x264 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x260 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x268 + (int)piVar15) = 0xbf800000;
  *(undefined4 *)(iVar12 + 0x278 + (int)piVar15) = 0;
  *(float *)((int)piVar15 + iVar12 + 0x270) = fVar18 * -1.0;
  *(float *)((int)piVar15 + iVar12 + 0x274) = local_c0;
  iVar14 = *(int *)(iVar11 + 0x94);
  fVar18 = *(float *)((int)piVar15 + iVar12 + 0x270);
  fVar17 = *(float *)(iVar14 + 0x78);
  fVar5 = *(float *)(iVar14 + 0x68);
  fVar6 = *(float *)(iVar14 + 0x88);
  fVar7 = *(float *)(iVar14 + 0x70);
  fVar8 = *(float *)(iVar14 + 0x80);
  fVar9 = *(float *)(iVar14 + 0x60);
  *(float *)((int)piVar15 + iVar12 + 0x274) =
       *(float *)(iVar14 + 100) * fVar18 + *(float *)(iVar14 + 0x74) * local_c0 +
       *(float *)(iVar14 + 0x84) * 0.0;
  *(float *)((int)piVar15 + iVar12 + 0x278) = fVar5 * fVar18 + fVar17 * local_c0 + fVar6 * 0.0;
  *(float *)((int)piVar15 + iVar12 + 0x270) = fVar7 * local_c0 + fVar8 * 0.0 + fVar9 * fVar18;
  *(int *)((int)piVar15 + iVar12 + 0x280) = local_198[0x10f] + 2;
  puVar4 = (undefined4 *)(local_198[0x10e] + 0x170 + (int)piVar15);
  puVar4[2] = 0;
  puVar4[1] = 0;
  *puVar4 = 0;
  puVar4[4] = local_100;
  puVar4[5] = local_fc;
  puVar4[6] = local_f8;
  iVar12 = *(int *)(iVar10 + 0x94);
  fVar18 = (float)puVar4[5];
  fVar17 = (float)puVar4[4];
  fVar5 = *(float *)(iVar12 + 0x78);
  fVar6 = *(float *)(iVar12 + 0x68);
  fVar7 = *(float *)(iVar12 + 0x88);
  fVar8 = *(float *)(iVar12 + 0x70);
  fVar9 = *(float *)(iVar12 + 0x80);
  fVar20 = *(float *)(iVar12 + 0x60);
  puVar4[5] = *(float *)(iVar12 + 100) * fVar17 + *(float *)(iVar12 + 0x74) * fVar18 +
              *(float *)(iVar12 + 0x84) * local_f8;
  puVar4[6] = fVar6 * fVar17 + fVar5 * fVar18 + fVar7 * local_f8;
  puVar4[4] = fVar8 * fVar18 + fVar9 * local_f8 + fVar20 * fVar17;
  puVar4[8] = local_198[0x10f] + 3;
  iVar12 = local_198[0x10e];
  *(undefined4 *)(iVar12 + 0x298 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x294 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x290 + (int)piVar15) = 0;
  *(float *)((int)piVar15 + iVar12 + 0x2a0) = local_100 * -1.0;
  *(float *)((int)piVar15 + iVar12 + 0x2a4) = local_fc * -1.0;
  local_f8 = local_f8 * -1.0;
  *(float *)((int)piVar15 + iVar12 + 0x2a8) = local_f8;
  iVar14 = *(int *)(iVar11 + 0x94);
  fVar18 = *(float *)((int)piVar15 + iVar12 + 0x2a4);
  fVar17 = *(float *)((int)piVar15 + iVar12 + 0x2a0);
  fVar5 = *(float *)(iVar14 + 0x78);
  fVar6 = *(float *)(iVar14 + 0x68);
  fVar7 = *(float *)(iVar14 + 0x88);
  fVar8 = *(float *)(iVar14 + 0x70);
  fVar9 = *(float *)(iVar14 + 0x80);
  fVar20 = *(float *)(iVar14 + 0x60);
  *(float *)((int)piVar15 + iVar12 + 0x2a4) =
       *(float *)(iVar14 + 100) * fVar17 + *(float *)(iVar14 + 0x74) * fVar18 +
       *(float *)(iVar14 + 0x84) * local_f8;
  *(float *)((int)piVar15 + iVar12 + 0x2a8) = fVar6 * fVar17 + fVar5 * fVar18 + fVar7 * local_f8;
  *(float *)((int)piVar15 + iVar12 + 0x2a0) = fVar8 * fVar18 + fVar9 * local_f8 + fVar20 * fVar17;
  *(int *)((int)piVar15 + iVar12 + 0x2b0) = local_198[0x10f] + 3;
  iVar12 = local_198[0x10e];
  *(undefined4 *)(iVar12 + 0x1a8 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x1a4 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x1a0 + (int)piVar15) = 0;
  *(float *)(iVar12 + 0x1b0 + (int)piVar15) = local_f0;
  *(float *)(iVar12 + 0x1b4 + (int)piVar15) = local_ec;
  *(float *)(iVar12 + 0x1b8 + (int)piVar15) = local_e8;
  iVar14 = *(int *)(iVar10 + 0x94);
  fVar18 = *(float *)(iVar12 + 0x1b4 + (int)piVar15);
  fVar17 = *(float *)(iVar12 + 0x1b0 + (int)piVar15);
  fVar5 = *(float *)(iVar14 + 0x78);
  fVar6 = *(float *)(iVar14 + 0x68);
  fVar7 = *(float *)(iVar14 + 0x88);
  fVar8 = *(float *)(iVar14 + 0x70);
  fVar9 = *(float *)(iVar14 + 0x80);
  fVar20 = *(float *)(iVar14 + 0x60);
  *(float *)((int)piVar15 + iVar12 + 0x1b4) =
       *(float *)(iVar14 + 100) * fVar17 + *(float *)(iVar14 + 0x74) * fVar18 +
       *(float *)(iVar14 + 0x84) * local_e8;
  *(float *)((int)piVar15 + iVar12 + 0x1b8) = fVar6 * fVar17 + fVar5 * fVar18 + fVar7 * local_e8;
  *(float *)((int)piVar15 + iVar12 + 0x1b0) = fVar8 * fVar18 + fVar9 * local_e8 + fVar20 * fVar17;
  *(int *)((int)piVar15 + iVar12 + 0x1c0) = local_198[0x10f] + 4;
  iVar12 = local_198[0x10e];
  *(undefined4 *)(iVar12 + 0x2c8 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x2c4 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x2c0 + (int)piVar15) = 0;
  *(float *)((int)piVar15 + iVar12 + 0x2d0) = local_f0 * -1.0;
  *(float *)((int)piVar15 + iVar12 + 0x2d4) = local_ec * -1.0;
  fVar19 = local_e8 * -1.0;
  *(float *)((int)piVar15 + iVar12 + 0x2d8) = fVar19;
  iVar14 = *(int *)(iVar11 + 0x94);
  fVar18 = *(float *)((int)piVar15 + iVar12 + 0x2d4);
  fVar17 = *(float *)((int)piVar15 + iVar12 + 0x2d0);
  fVar5 = *(float *)(iVar14 + 0x74);
  fVar6 = *(float *)(iVar14 + 100);
  fVar7 = *(float *)(iVar14 + 0x84);
  fVar8 = *(float *)(iVar14 + 0x70);
  fVar9 = *(float *)(iVar14 + 0x80);
  fVar20 = *(float *)(iVar14 + 0x60);
  *(float *)((int)piVar15 + iVar12 + 0x2d8) =
       *(float *)(iVar14 + 0x68) * fVar17 + *(float *)(iVar14 + 0x78) * fVar18 +
       *(float *)(iVar14 + 0x88) * fVar19;
  *(float *)((int)piVar15 + iVar12 + 0x2d4) = fVar6 * fVar17 + fVar5 * fVar18 + fVar7 * fVar19;
  *(float *)((int)piVar15 + iVar12 + 0x2d0) = fVar8 * fVar18 + fVar9 * fVar19 + fVar20 * fVar17;
  *(int *)((int)piVar15 + iVar12 + 0x2e0) = local_198[0x10f] + 4;
  iVar12 = local_198[0x10e];
  *(undefined4 *)(iVar12 + 0x1d8 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x1d4 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x1d0 + (int)piVar15) = 0;
  *(float *)(iVar12 + 0x1e0 + (int)piVar15) = local_e0;
  *(float *)(iVar12 + 0x1e4 + (int)piVar15) = local_dc;
  *(float *)(iVar12 + 0x1e8 + (int)piVar15) = local_d8;
  iVar14 = *(int *)(iVar10 + 0x94);
  fVar18 = *(float *)(iVar12 + 0x1e4 + (int)piVar15);
  fVar17 = *(float *)(iVar14 + 0x78);
  fVar5 = *(float *)((int)piVar15 + iVar12 + 0x1e0);
  fVar6 = *(float *)(iVar14 + 0x68);
  fVar7 = *(float *)(iVar14 + 0x60);
  fVar8 = *(float *)(iVar14 + 0x88);
  fVar9 = *(float *)(iVar14 + 0x70);
  fVar20 = *(float *)(iVar14 + 0x80);
  *(float *)((int)piVar15 + iVar12 + 0x1e4) =
       *(float *)(iVar14 + 100) * fVar5 + *(float *)(iVar14 + 0x74) * fVar18 +
       *(float *)(iVar14 + 0x84) * local_d8;
  *(float *)((int)piVar15 + iVar12 + 0x1e8) = fVar6 * fVar5 + fVar17 * fVar18 + fVar8 * local_d8;
  *(float *)((int)piVar15 + iVar12 + 0x1e0) = fVar9 * fVar18 + fVar20 * local_d8 + fVar5 * fVar7;
  *(int *)((int)piVar15 + iVar12 + 0x1f0) = local_198[0x10f] + 5;
  iVar12 = local_198[0x10e];
  *(undefined4 *)(iVar12 + 0x2f8 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x2f4 + (int)piVar15) = 0;
  *(undefined4 *)(iVar12 + 0x2f0 + (int)piVar15) = 0;
  *(float *)((int)piVar15 + iVar12 + 0x300) = local_e0 * -1.0;
  *(float *)((int)piVar15 + iVar12 + 0x304) = local_dc * -1.0;
  fVar19 = local_d8 * -1.0;
  *(float *)((int)piVar15 + iVar12 + 0x308) = fVar19;
  iVar14 = *(int *)(iVar11 + 0x94);
  fVar18 = *(float *)((int)piVar15 + iVar12 + 0x304);
  fVar17 = *(float *)(iVar14 + 0x78);
  fVar5 = *(float *)((int)piVar15 + iVar12 + 0x300);
  fVar6 = *(float *)(iVar14 + 0x68);
  fVar7 = *(float *)(iVar14 + 0x60);
  fVar8 = *(float *)(iVar14 + 0x88);
  fVar9 = *(float *)(iVar14 + 0x70);
  fVar20 = *(float *)(iVar14 + 0x80);
  *(float *)((int)piVar15 + iVar12 + 0x304) =
       *(float *)(iVar14 + 100) * fVar5 + *(float *)(iVar14 + 0x74) * fVar18 +
       *(float *)(iVar14 + 0x84) * fVar19;
  *(float *)((int)piVar15 + iVar12 + 0x308) = fVar6 * fVar5 + fVar17 * fVar18 + fVar8 * fVar19;
  *(float *)((int)piVar15 + iVar12 + 0x300) = fVar9 * fVar18 + fVar20 * fVar19 + fVar5 * fVar7;
  *(int *)((int)piVar15 + iVar12 + 0x310) = local_198[0x10f] + 5;
  local_198[0x10f] = local_198[0x10f] + 6;
  local_198[0x10e] = local_198[0x10e] + 0x120;
  piVar1 = local_198 + 0x108;
  piVar2 = local_198 + 0x10b;
  *piVar1 = local_198[0x10f];
  *piVar2 = local_198[0x10f];
  piVar3 = local_198 + 0xd4;
  FUN_100a4e60(*(void **)(iVar10 + 0x94),piVar1,piVar3,(int)piVar15);
  *piVar1 = local_198[0x10f];
  *piVar2 = local_198[0x10f];
  FUN_100a4e60(*(void **)(iVar11 + 0x94),piVar2,piVar3,(int)piVar15);
  local_70 = local_60;
  pfVar13 = (float *)*local_198;
  local_6c[0] = 8.40779e-45;
  local_6c[1] = 1.12104e-44;
  *pfVar13 = local_120 - local_110;
  pfVar13[1] = local_11c - local_10c;
  pfVar13[2] = local_118 - local_108;
  fVar18 = *(float *)(param_1 + 4);
  *pfVar13 = (local_120 - local_110) * fVar18;
  pfVar13[1] = (local_11c - local_10c) * fVar18;
  pfVar13[2] = (local_118 - local_108) * fVar18;
  pfVar13[3] = (local_d8 * local_138 + local_dc * local_13c + local_e0 * local_140) *
               *(float *)(param_1 + 4);
  pfVar13[4] = -0.0 - (local_d8 * local_150[2] + local_dc * local_150[1] + local_e0 * local_150[0])
                      * *(float *)(param_1 + 4);
  pfVar13[5] = (local_e8 * local_150[2] + local_ec * local_150[1] + local_f0 * local_150[0]) *
               *(float *)(param_1 + 4);
  fVar17 = *(float *)((int)this + 100) * param_3;
  fVar18 = *(float *)((int)this + 0x60) * param_4 * -1.0;
  local_1a4 = 5;
  pfVar13 = (float *)(local_198 + 8);
  do {
    *(float *)(((int)local_6c - (int)local_198) + (int)pfVar13) =
         (float)piVar15[local_1a4] * fVar18 + pfVar13[1] * fVar17;
    iVar12 = local_1a4 + 0xb;
    local_1a4 = local_1a4 + -2;
    *(float *)(((int)&local_70 - (int)local_198) + (int)pfVar13) =
         (float)local_198[iVar12] * fVar18 + fVar17 * *pfVar13;
    pfVar13 = pfVar13 + -2;
  } while (0 < local_1a4);
  FUN_100ba1b0((int)piVar3);
  local_40 = local_30;
  local_3c = 6;
  local_38 = 8;
  FUN_100ba040(piVar3,(int *)&local_70,(int *)&local_40);
  FUN_100a53e0(*(void **)(iVar10 + 0x94),piVar1,(int)local_40);
  FUN_100a53e0(*(void **)(iVar11 + 0x94),piVar2,(int)local_40);
  return 0x440;
}


// 100a0160 FUN_100a0160

void __thiscall FUN_100a0160(void *this,undefined4 *param_1)

{
  *(undefined4 *)((int)this + 0x38) = *param_1;
  *(undefined4 *)((int)this + 0x3c) = param_1[1];
  *(undefined4 *)((int)this + 0x18) = param_1[2];
  *(undefined4 *)((int)this + 0x1c) = param_1[3];
  *(undefined4 *)((int)this + 0x20) = param_1[4];
  *(undefined4 *)((int)this + 0x28) = param_1[6];
  *(undefined4 *)((int)this + 0x2c) = param_1[7];
  *(undefined4 *)((int)this + 0x30) = param_1[8];
  return;
}


// 100a01a0 FUN_100a01a0

void __thiscall FUN_100a01a0(void *this,undefined4 *param_1)

{
  *param_1 = *(undefined4 *)((int)this + 0x38);
  param_1[1] = *(undefined4 *)((int)this + 0x3c);
  param_1[2] = *(undefined4 *)((int)this + 0x18);
  param_1[3] = *(undefined4 *)((int)this + 0x1c);
  param_1[4] = *(undefined4 *)((int)this + 0x20);
  param_1[6] = *(undefined4 *)((int)this + 0x28);
  param_1[7] = *(undefined4 *)((int)this + 0x2c);
  param_1[8] = *(undefined4 *)((int)this + 0x30);
  return;
}


// 100a01f0 FUN_100a01f0

undefined4 * __thiscall
FUN_100a01f0(void *this,undefined4 param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4
            )

{
  FUN_100a4de0(this,param_1,param_3,param_4);
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined ***)this = &PTR_LAB_100ce7d8;
  FUN_100a0160(this,param_2);
  return (undefined4 *)this;
}


// 100a0240 FUN_100a0240

undefined4 * __thiscall FUN_100a0240(void *this,byte param_1)

{
  FUN_100a4e40((undefined4 *)this);
  if (((param_1 & 1) != 0) && (this != (void *)0x0)) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)(((uint)this & 0xfffffffc) - 4));
  }
  return (undefined4 *)this;
}


// 100a0270 FUN_100a0270

undefined4 * __thiscall
FUN_100a0270(void *this,void *param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4)

{
  FUN_100a4e00(this,param_1,param_3,param_4,0x195,0x200);
  *(undefined ***)this = &PTR_LAB_100ce7d8;
  FUN_100a0160(this,param_2);
  return (undefined4 *)this;
}


// 100a02c0 FUN_100a02c0

void __thiscall FUN_100a02c0(void *this,int param_1,float *param_2,float param_3,float param_4)

{
  float *pfVar1;
  float *pfVar2;
  float *pfVar3;
  float *pfVar4;
  float *pfVar5;
  float fVar6;
  int iVar7;
  int iVar8;
  int iVar9;
  float *pfVar10;
  float *pfVar11;
  int iVar12;
  float fVar13;
  float fVar14;
  float fVar15;
  float fVar16;
  float local_20;
  float local_1c;
  float local_18;
  
  param_2[6] = 0.0;
  param_2[5] = 0.0;
  param_2[4] = 0.0;
  iVar7 = *(int *)((int)this + 4);
  iVar8 = *(int *)((int)this + 8);
  iVar9 = *(int *)(iVar7 + 0x94);
  pfVar10 = (float *)param_2[0x79];
  iVar12 = (int)param_2[0x78] + -1;
  pfVar11 = pfVar10 + 5;
  do {
    pfVar1 = pfVar11 + -4;
    pfVar2 = pfVar11 + -3;
    pfVar3 = pfVar11 + 1;
    pfVar4 = pfVar11 + 3;
    fVar6 = *pfVar10;
    pfVar5 = pfVar11 + -1;
    fVar13 = *pfVar11;
    pfVar10 = pfVar10 + 0xc;
    pfVar11 = pfVar11 + 0xc;
    iVar12 = iVar12 + -1;
    param_2[(int)*pfVar4 + 4] =
         *pfVar1 * *(float *)(iVar9 + 0xd4) + *pfVar2 * *(float *)(iVar9 + 0xd8) +
         fVar6 * *(float *)(iVar9 + 0xd0) +
         *pfVar5 * *(float *)(iVar9 + 0xc0) + *pfVar3 * *(float *)(iVar9 + 200) +
         *(float *)(iVar9 + 0xc4) * fVar13 + param_2[(int)*pfVar4 + 4];
  } while (-1 < iVar12);
  iVar9 = *(int *)(iVar8 + 0x94);
  pfVar10 = (float *)param_2[0x7c];
  iVar12 = (int)param_2[0x7b] + -1;
  pfVar11 = pfVar10 + 5;
  do {
    pfVar1 = pfVar11 + -4;
    pfVar2 = pfVar11 + -3;
    pfVar3 = pfVar11 + -1;
    pfVar4 = pfVar11 + 3;
    fVar6 = *pfVar10;
    pfVar5 = pfVar11 + 1;
    fVar13 = *pfVar11;
    pfVar10 = pfVar10 + 0xc;
    pfVar11 = pfVar11 + 0xc;
    iVar12 = iVar12 + -1;
    param_2[(int)*pfVar4 + 4] =
         *pfVar1 * *(float *)(iVar9 + 0xd4) + *pfVar2 * *(float *)(iVar9 + 0xd8) +
         fVar6 * *(float *)(iVar9 + 0xd0) +
         *pfVar5 * *(float *)(iVar9 + 200) + *pfVar3 * *(float *)(iVar9 + 0xc0) +
         *(float *)(iVar9 + 0xc4) * fVar13 + param_2[(int)*pfVar4 + 4];
  } while (-1 < iVar12);
  fVar13 = *(float *)(param_1 + 4) * *(float *)((int)this + 0x38) * param_3;
  pfVar10 = (float *)param_2[0x68];
  fVar14 = *(float *)((int)this + 0x3c) * param_4 * -1.0;
  fVar15 = fVar14 * param_2[4] + *param_2 * fVar13;
  fVar16 = param_2[5] * fVar14 + param_2[1] * fVar13;
  fVar6 = param_2[0x6b];
  fVar13 = param_2[6] * fVar14 + param_2[2] * fVar13;
  pfVar11 = pfVar10 + (int)fVar6;
  local_20 = fVar13 * pfVar11[(int)fVar6] + *pfVar11 * fVar16 + *pfVar10 * fVar15;
  local_1c = pfVar11[(int)fVar6 + 1] * fVar13 + pfVar11[1] * fVar16 + pfVar10[1] * fVar15;
  local_18 = pfVar11[(int)fVar6 + 2] * fVar13 + pfVar11[2] * fVar16 + pfVar10[2] * fVar15;
  FUN_100a53e0(*(void **)(iVar7 + 0x94),(int *)(param_2 + 0x78),(int)&local_20);
  FUN_100a53e0(*(void **)(iVar8 + 0x94),(int *)(param_2 + 0x7b),(int)&local_20);
  return;
}


// 100a0580 FUN_100a0580

void __fastcall FUN_100a0580(undefined4 *param_1)

{
  int iVar1;
  
  param_1[0x79] = param_1 + 0x10;
  param_1[0x76] = param_1 + 4;
  param_1[0x78] = param_1 + 0x40;
  param_1[0x75] = param_1 + 0x1c;
  param_1[0x7a] = 0;
  param_1[0x7b] = 0;
  iVar1 = param_1[0x67] * param_1[0x65] + -1;
  if (3 < (int)(param_1[0x67] * param_1[0x65])) {
    do {
      *(undefined4 *)(param_1[100] + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[100] + -4 + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[100] + -8 + iVar1 * 4) = 0;
      *(undefined4 *)(param_1[100] + -0xc + iVar1 * 4) = 0;
      iVar1 = iVar1 + -4;
    } while (2 < iVar1);
  }
  while (-1 < iVar1) {
    iVar1 = iVar1 + -1;
    *(undefined4 *)(param_1[100] + 4 + iVar1 * 4) = 0;
  }
  param_1[3] = 0;
  param_1[2] = 0;
  param_1[1] = 0;
  *param_1 = 0;
  return;
}


// 100a0640 FUN_100a0640

undefined4 __thiscall
FUN_100a0640(void *this,int param_1,float *param_2,float param_3,float param_4)

{
  float *pfVar1;
  undefined4 *puVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  int iVar7;
  int iVar8;
  float *pfVar9;
  int iVar10;
  int iVar11;
  float *pfVar12;
  float *pfVar13;
  float fVar14;
  float fVar15;
  float fVar16;
  float fVar17;
  float fVar18;
  float fVar19;
  float fVar20;
  float *local_dc;
  float local_b8;
  float local_b0;
  float local_ac;
  float local_a8;
  float local_9c;
  float local_98;
  float local_90;
  float local_8c;
  float local_70;
  float local_6c;
  float local_68;
  float local_60 [4];
  float local_50;
  float local_4c;
  float local_48;
  float local_40;
  float local_3c;
  float local_38;
  float local_30;
  float local_2c;
  float local_28;
  float local_20;
  float local_1c;
  float local_18;
  
  if (param_2 == (float *)0x0) {
    local_dc = (float *)0x0;
  }
  else {
    param_2[0x68] = (float)(param_2 + 0x6c);
    param_2[0x69] = 4.2039e-45;
    param_2[0x6a] = 4.2039e-45;
    param_2[0x6b] = 5.60519e-45;
    local_dc = param_2;
  }
  iVar7 = *(int *)((int)this + 4);
  iVar8 = *(int *)((int)this + 8);
  pfVar1 = local_dc + 4;
  if ((*(uint *)(iVar7 + 0x44) & 0x800) == 0) {
    pfVar12 = (float *)(*(int *)(iVar7 + 0x94) + 0x60);
    pfVar13 = local_60;
    for (iVar10 = 0x10; iVar10 != 0; iVar10 = iVar10 + -1) {
      *pfVar13 = *pfVar12;
      pfVar12 = pfVar12 + 1;
      pfVar13 = pfVar13 + 1;
    }
    FUN_10039500(local_60,(float *)(iVar7 + 0x30),&local_30);
    local_b8 = local_40;
    local_ac = local_4c;
    local_a8 = local_3c;
    local_9c = local_48;
    local_98 = local_38;
    local_90 = local_30;
    local_8c = local_2c;
    fVar14 = local_28;
    fVar19 = local_60[0];
    fVar18 = local_50;
    fVar15 = local_60[1];
    fVar20 = local_60[2];
  }
  else {
    iVar10 = *(int *)(iVar7 + 0x94);
    local_b8 = *(float *)(iVar10 + 0x80);
    local_ac = *(float *)(iVar10 + 0x74);
    local_a8 = *(float *)(iVar10 + 0x84);
    local_9c = *(float *)(iVar10 + 0x78);
    local_98 = *(float *)(iVar10 + 0x88);
    local_90 = *(float *)(iVar10 + 0x90);
    local_8c = *(float *)(iVar10 + 0x94);
    fVar14 = *(float *)(iVar10 + 0x98);
    fVar19 = *(float *)(iVar10 + 0x60);
    fVar18 = *(float *)(iVar10 + 0x70);
    fVar15 = *(float *)(iVar10 + 100);
    fVar20 = *(float *)(iVar10 + 0x68);
  }
  fVar3 = *(float *)((int)this + 0x20);
  fVar4 = *(float *)((int)this + 0x18);
  iVar10 = *(int *)(iVar8 + 0x94);
  fVar17 = *(float *)((int)this + 0x1c);
  fVar19 = fVar3 * fVar20 + fVar4 * fVar19 + fVar17 * fVar15 + local_90;
  fVar18 = fVar3 * local_9c + fVar17 * local_ac + fVar4 * fVar18 + local_8c;
  fVar14 = fVar17 * local_a8 + fVar3 * local_98 + fVar4 * local_b8 + fVar14;
  if ((*(uint *)(iVar8 + 0x44) & 0x800) == 0) {
    pfVar12 = (float *)(iVar10 + 0x60);
    pfVar13 = local_60;
    for (iVar11 = 0x10; iVar11 != 0; iVar11 = iVar11 + -1) {
      *pfVar13 = *pfVar12;
      pfVar12 = pfVar12 + 1;
      pfVar13 = pfVar13 + 1;
    }
    FUN_10039500(local_60,(float *)(iVar8 + 0x30),&local_30);
    local_b8 = local_40;
    local_b0 = local_60[1];
    local_ac = local_4c;
    local_a8 = local_3c;
    local_9c = local_48;
    local_98 = local_38;
    local_90 = local_30;
    local_8c = local_2c;
  }
  else {
    local_b8 = *(float *)(iVar10 + 0x80);
    local_60[0] = *(float *)(iVar10 + 0x60);
    local_50 = *(float *)(iVar10 + 0x70);
    local_60[2] = *(float *)(iVar10 + 0x68);
    local_b0 = *(float *)(iVar10 + 100);
    local_ac = *(float *)(iVar10 + 0x74);
    local_a8 = *(float *)(iVar10 + 0x84);
    local_9c = *(float *)(iVar10 + 0x78);
    local_98 = *(float *)(iVar10 + 0x88);
    local_90 = *(float *)(iVar10 + 0x90);
    local_8c = *(float *)(iVar10 + 0x94);
    local_28 = *(float *)(iVar10 + 0x98);
  }
  fVar15 = *(float *)((int)this + 0x28);
  fVar20 = *(float *)((int)this + 0x2c);
  fVar3 = *(float *)((int)this + 0x30);
  local_90 = fVar15 * local_60[0] + fVar3 * local_60[2] + fVar20 * local_b0 + local_90;
  local_48 = fVar3 * local_98 + fVar15 * local_b8 + fVar20 * local_a8 + local_28;
  local_4c = fVar3 * local_9c + fVar15 * local_50 + fVar20 * local_ac + local_8c;
  *local_dc = local_90 - fVar19;
  local_dc[1] = local_4c - fVar18;
  local_dc[2] = local_48 - fVar14;
  local_50 = local_90;
  FUN_100a0580(pfVar1);
  iVar10 = *(int *)(iVar7 + 0x94);
  fVar19 = fVar19 - *(float *)(iVar10 + 0x90);
  iVar11 = *(int *)(iVar8 + 0x94);
  fVar18 = fVar18 - *(float *)(iVar10 + 0x94);
  fVar14 = fVar14 - *(float *)(iVar10 + 0x98);
  local_20 = local_50 - *(float *)(iVar11 + 0x90);
  local_1c = local_4c - *(float *)(iVar11 + 0x94);
  local_18 = local_48 - *(float *)(iVar11 + 0x98);
  puVar2 = (undefined4 *)((int)local_dc + (int)local_dc[0x7e] + 0x80);
  puVar2[2] = 0;
  puVar2[1] = 0;
  *puVar2 = 0x3f800000;
  puVar2[5] = fVar14;
  puVar2[4] = 0;
  fVar16 = -0.0 - fVar18;
  puVar2[6] = fVar16;
  fVar15 = (float)puVar2[5];
  iVar10 = *(int *)(iVar7 + 0x94);
  fVar20 = *(float *)(iVar10 + 0x78);
  fVar3 = *(float *)(iVar10 + 0x68);
  fVar4 = *(float *)(iVar10 + 0x60);
  fVar17 = *(float *)(iVar10 + 0x88);
  fVar5 = *(float *)(iVar10 + 0x70);
  fVar6 = *(float *)(iVar10 + 0x80);
  puVar2[5] = *(float *)(iVar10 + 100) * 0.0 + *(float *)(iVar10 + 0x74) * fVar15 +
              *(float *)(iVar10 + 0x84) * fVar16;
  puVar2[6] = fVar3 * 0.0 + fVar20 * fVar15 + fVar17 * fVar16;
  puVar2[4] = fVar5 * fVar15 + fVar6 * fVar16 + fVar4 * 0.0;
  puVar2[8] = local_dc[0x7f];
  fVar6 = local_dc[0x7e];
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0x118) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0x114) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0x110) = 0xbf800000;
  *(float *)((int)local_dc + (int)fVar6 + 0x128) = local_1c;
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0x120) = 0;
  fVar16 = local_18 * -1.0;
  *(float *)((int)local_dc + (int)fVar6 + 0x124) = fVar16;
  iVar10 = *(int *)(iVar8 + 0x94);
  fVar15 = *(float *)(iVar10 + 100);
  fVar20 = *(float *)(iVar10 + 0x74);
  fVar3 = *(float *)(iVar10 + 0x78);
  fVar4 = *(float *)(iVar10 + 0x84);
  fVar17 = *(float *)(iVar10 + 0x68);
  fVar5 = *(float *)(iVar10 + 0x88);
  *(float *)((int)local_dc + (int)fVar6 + 0x120) =
       *(float *)(iVar10 + 0x70) * fVar16 + *(float *)(iVar10 + 0x80) * local_1c +
       *(float *)(iVar10 + 0x60) * 0.0;
  *(float *)((int)local_dc + (int)fVar6 + 0x124) = fVar15 * 0.0 + fVar20 * fVar16 + fVar4 * local_1c
  ;
  *(float *)((int)local_dc + (int)fVar6 + 0x128) = fVar17 * 0.0 + fVar3 * fVar16 + fVar5 * local_1c;
  *(float *)((int)local_dc + (int)fVar6 + 0x130) = local_dc[0x7f];
  fVar6 = local_dc[0x7e];
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0xb8) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0xb0) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0xb4) = 0x3f800000;
  *(float *)((int)local_dc + (int)fVar6 + 0xc0) = -0.0 - fVar14;
  *(float *)((int)local_dc + (int)fVar6 + 200) = fVar19;
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0xc4) = 0;
  iVar10 = *(int *)(iVar7 + 0x94);
  fVar14 = *(float *)(iVar10 + 0x78);
  fVar15 = *(float *)((int)local_dc + (int)fVar6 + 0xc0);
  fVar20 = *(float *)(iVar10 + 0x68);
  fVar3 = *(float *)(iVar10 + 0x60);
  fVar4 = *(float *)(iVar10 + 0x88);
  fVar17 = *(float *)(iVar10 + 0x70);
  fVar5 = *(float *)(iVar10 + 0x80);
  *(float *)((int)local_dc + (int)fVar6 + 0xc4) =
       *(float *)(iVar10 + 100) * fVar15 + *(float *)(iVar10 + 0x74) * 0.0 +
       *(float *)(iVar10 + 0x84) * fVar19;
  *(float *)((int)local_dc + (int)fVar6 + 200) = fVar20 * fVar15 + fVar14 * 0.0 + fVar4 * fVar19;
  *(float *)((int)local_dc + (int)fVar6 + 0xc0) = fVar17 * 0.0 + fVar5 * fVar19 + fVar15 * fVar3;
  *(int *)((int)local_dc + (int)fVar6 + 0xd0) = (int)local_dc[0x7f] + 1;
  fVar6 = local_dc[0x7e];
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0x148) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0x140) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0x144) = 0xbf800000;
  *(float *)((int)local_dc + (int)fVar6 + 0x150) = local_18;
  *(undefined4 *)((int)local_dc + (int)fVar6 + 0x154) = 0;
  fVar16 = local_20 * -1.0;
  *(float *)((int)local_dc + (int)fVar6 + 0x158) = fVar16;
  iVar10 = *(int *)(iVar8 + 0x94);
  fVar14 = *(float *)((int)local_dc + (int)fVar6 + 0x150);
  fVar15 = *(float *)(iVar10 + 0x78);
  fVar20 = *(float *)(iVar10 + 0x68);
  fVar3 = *(float *)(iVar10 + 0x88);
  fVar4 = *(float *)(iVar10 + 0x70);
  fVar17 = *(float *)(iVar10 + 0x80);
  fVar5 = *(float *)(iVar10 + 0x60);
  *(float *)((int)local_dc + (int)fVar6 + 0x154) =
       *(float *)(iVar10 + 100) * fVar14 + *(float *)(iVar10 + 0x74) * 0.0 +
       *(float *)(iVar10 + 0x84) * fVar16;
  *(float *)((int)local_dc + (int)fVar6 + 0x158) = fVar20 * fVar14 + fVar15 * 0.0 + fVar3 * fVar16;
  *(float *)((int)local_dc + (int)fVar6 + 0x150) = fVar4 * 0.0 + fVar17 * fVar16 + fVar5 * fVar14;
  *(int *)((int)local_dc + (int)fVar6 + 0x160) = (int)local_dc[0x7f] + 1;
  fVar5 = local_dc[0x7e];
  *(undefined4 *)((int)local_dc + (int)fVar5 + 0xe4) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar5 + 0xe0) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar5 + 0xe8) = 0x3f800000;
  *(float *)((int)local_dc + (int)fVar5 + 0xf0) = fVar18;
  fVar19 = -0.0 - fVar19;
  *(float *)((int)local_dc + (int)fVar5 + 0xf4) = fVar19;
  *(undefined4 *)((int)local_dc + (int)fVar5 + 0xf8) = 0;
  iVar10 = *(int *)(iVar7 + 0x94);
  fVar14 = *(float *)((int)local_dc + (int)fVar5 + 0xf0);
  fVar18 = *(float *)(iVar10 + 0x78);
  fVar15 = *(float *)(iVar10 + 0x68);
  fVar20 = *(float *)(iVar10 + 0x88);
  fVar3 = *(float *)(iVar10 + 0x70);
  fVar4 = *(float *)(iVar10 + 0x80);
  fVar17 = *(float *)(iVar10 + 0x60);
  *(float *)((int)local_dc + (int)fVar5 + 0xf4) =
       *(float *)(iVar10 + 100) * fVar14 + *(float *)(iVar10 + 0x74) * fVar19 +
       *(float *)(iVar10 + 0x84) * 0.0;
  *(float *)((int)local_dc + (int)fVar5 + 0xf8) = fVar15 * fVar14 + fVar18 * fVar19 + fVar20 * 0.0;
  *(float *)((int)local_dc + (int)fVar5 + 0xf0) = fVar3 * fVar19 + fVar4 * 0.0 + fVar17 * fVar14;
  *(int *)((int)local_dc + (int)fVar5 + 0x100) = (int)local_dc[0x7f] + 2;
  fVar4 = local_dc[0x7e];
  *(undefined4 *)((int)local_dc + (int)fVar4 + 0x174) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar4 + 0x170) = 0;
  *(undefined4 *)((int)local_dc + (int)fVar4 + 0x178) = 0xbf800000;
  fVar17 = local_1c * -1.0;
  *(float *)((int)local_dc + (int)fVar4 + 0x184) = local_20;
  *(float *)((int)local_dc + (int)fVar4 + 0x180) = fVar17;
  *(undefined4 *)((int)local_dc + (int)fVar4 + 0x188) = 0;
  iVar10 = *(int *)(iVar8 + 0x94);
  fVar14 = *(float *)(iVar10 + 0x78);
  fVar19 = *(float *)(iVar10 + 0x68);
  fVar18 = *(float *)(iVar10 + 0x60);
  fVar15 = *(float *)(iVar10 + 0x88);
  fVar20 = *(float *)(iVar10 + 0x70);
  fVar3 = *(float *)(iVar10 + 0x80);
  *(float *)((int)local_dc + (int)fVar4 + 0x184) =
       *(float *)(iVar10 + 100) * fVar17 + *(float *)(iVar10 + 0x74) * local_20 +
       *(float *)(iVar10 + 0x84) * 0.0;
  *(float *)((int)local_dc + (int)fVar4 + 0x188) =
       fVar19 * fVar17 + fVar14 * local_20 + fVar15 * 0.0;
  *(float *)((int)local_dc + (int)fVar4 + 0x180) = fVar20 * local_20 + fVar3 * 0.0 + fVar17 * fVar18
  ;
  *(int *)((int)local_dc + (int)fVar4 + 400) = (int)local_dc[0x7f] + 2;
  local_dc[0x7f] = (float)((int)local_dc[0x7f] + 3);
  local_dc[0x7e] = (float)((int)local_dc[0x7e] + 0x90);
  pfVar12 = local_dc + 0x7b;
  *pfVar12 = local_dc[0x7f];
  pfVar13 = local_dc + 0x78;
  pfVar9 = local_dc + 0x68;
  *pfVar13 = local_dc[0x7f];
  FUN_100a4e60(*(void **)(iVar7 + 0x94),(int *)pfVar13,(int *)pfVar9,(int)pfVar1);
  *pfVar13 = local_dc[0x7f];
  *pfVar12 = local_dc[0x7f];
  FUN_100a4e60(*(void **)(iVar8 + 0x94),(int *)pfVar12,(int *)pfVar9,(int)pfVar1);
  fVar14 = *(float *)(param_1 + 4) * *(float *)((int)this + 0x38) * param_3;
  fVar19 = *(float *)((int)this + 0x3c) * param_4 * -1.0;
  fVar18 = fVar19 * *pfVar1 + *local_dc * fVar14;
  fVar15 = local_dc[5] * fVar19 + local_dc[1] * fVar14;
  fVar19 = local_dc[6] * fVar19 + local_dc[2] * fVar14;
  FUN_100ba980(pfVar9,0.0);
  pfVar9 = (float *)*pfVar9;
  fVar14 = local_dc[0x6b];
  local_70 = pfVar9[(int)fVar14 * 2] * fVar19 + pfVar9[(int)fVar14] * fVar15 + *pfVar9 * fVar18;
  local_6c = pfVar9[(int)fVar14 * 2 + 1] * fVar19 +
             pfVar9[(int)fVar14 + 1] * fVar15 + pfVar9[1] * fVar18;
  local_68 = pfVar9[(int)fVar14 * 2 + 2] * fVar19 +
             pfVar9[(int)fVar14 + 2] * fVar15 + pfVar9[2] * fVar18;
  FUN_100a53e0(*(void **)(iVar7 + 0x94),(int *)pfVar13,(int)&local_70);
  FUN_100a53e0(*(void **)(iVar8 + 0x94),(int *)pfVar12,(int)&local_70);
  return 0x200;
}


// 100a1300 FUN_100a1300

void __thiscall FUN_100a1300(void *this,undefined4 *param_1)

{
  *(undefined4 *)((int)this + 0x58) = param_1[1];
  *(undefined4 *)((int)this + 0x5c) = param_1[2];
  *(undefined4 *)((int)this + 0x60) = *param_1;
  *(undefined4 *)((int)this + 100) = param_1[3];
  *(undefined4 *)((int)this + 0x18) = param_1[4];
  *(undefined4 *)((int)this + 0x1c) = param_1[5];
  *(undefined4 *)((int)this + 0x20) = param_1[6];
  *(undefined4 *)((int)this + 0x28) = param_1[8];
  *(undefined4 *)((int)this + 0x2c) = param_1[9];
  *(undefined4 *)((int)this + 0x30) = param_1[10];
  *(undefined4 *)((int)this + 0x38) = param_1[0xc];
  *(undefined4 *)((int)this + 0x3c) = param_1[0xd];
  *(undefined4 *)((int)this + 0x40) = param_1[0xe];
  *(undefined4 *)((int)this + 0x48) = param_1[0x10];
  *(undefined4 *)((int)this + 0x4c) = param_1[0x11];
  *(undefined4 *)((int)this + 0x50) = param_1[0x12];
  *(undefined1 *)((int)this + 0x68) = *(undefined1 *)(param_1 + 0x14);
  return;
}


// 100a1370 FUN_100a1370

void __thiscall FUN_100a1370(void *this,undefined4 *param_1)

{
  param_1[1] = *(undefined4 *)((int)this + 0x58);
  param_1[2] = *(undefined4 *)((int)this + 0x5c);
  *param_1 = *(undefined4 *)((int)this + 0x60);
  param_1[3] = *(undefined4 *)((int)this + 100);
  param_1[4] = *(undefined4 *)((int)this + 0x18);
  param_1[5] = *(undefined4 *)((int)this + 0x1c);
  param_1[6] = *(undefined4 *)((int)this + 0x20);
  param_1[8] = *(undefined4 *)((int)this + 0x28);
  param_1[9] = *(undefined4 *)((int)this + 0x2c);
  param_1[10] = *(undefined4 *)((int)this + 0x30);
  param_1[0xc] = *(undefined4 *)((int)this + 0x38);
  param_1[0xd] = *(undefined4 *)((int)this + 0x3c);
  param_1[0xe] = *(undefined4 *)((int)this + 0x40);
  param_1[0x10] = *(undefined4 *)((int)this + 0x48);
  param_1[0x11] = *(undefined4 *)((int)this + 0x4c);
  param_1[0x12] = *(undefined4 *)((int)this + 0x50);
  *(undefined1 *)(param_1 + 0x14) = *(undefined1 *)((int)this + 0x68);
  return;
}


// 100a13e0 FUN_100a13e0

undefined4 * __thiscall
FUN_100a13e0(void *this,undefined4 param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4
            )

{
  FUN_100a4de0(this,param_1,param_3,param_4);
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined ***)this = &PTR_LAB_100ce808;
  FUN_100a1300(this,param_2);
  return (undefined4 *)this;
}


// 100a1440 FUN_100a1440

undefined4 * __thiscall
FUN_100a1440(void *this,void *param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4)

{
  FUN_100a4e00(this,param_1,param_3,param_4,0x195,0xe0);
  *(undefined ***)this = &PTR_LAB_100ce808;
  FUN_100a1300(this,param_2);
  return (undefined4 *)this;
}


// 100a1490 FUN_100a1490

undefined4 * __thiscall FUN_100a1490(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_LAB_100ce808;
  FUN_100a4e40((undefined4 *)this);
  if ((param_1 & 1) != 0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)(((uint)this & 0xfffffffc) - 4));
  }
  return (undefined4 *)this;
}


// 100a14e0 FUN_100a14e0

void __thiscall FUN_100a14e0(void *this,int param_1,float *param_2,float param_3,float param_4)

{
  float *pfVar1;
  float *pfVar2;
  float *pfVar3;
  float *pfVar4;
  float *pfVar5;
  float fVar6;
  float fVar7;
  int iVar8;
  void *this_00;
  float *pfVar9;
  int iVar10;
  float *pfVar11;
  int iVar12;
  float local_10 [4];
  
  if (*(char *)(param_2 + 0x35) == '\0') {
    *param_2 = 0.0;
    iVar8 = *(int *)((int)this + 8);
    this_00 = *(void **)(*(int *)((int)this + 4) + 0x94);
    pfVar11 = (float *)param_2[0x2d];
    iVar10 = (int)param_2[0x2c] + -1;
    pfVar9 = pfVar11 + 5;
    do {
      pfVar1 = pfVar9 + -4;
      pfVar2 = pfVar9 + -3;
      pfVar3 = pfVar9 + 1;
      pfVar4 = pfVar9 + 3;
      fVar6 = *pfVar11;
      pfVar5 = pfVar9 + -1;
      fVar7 = *pfVar9;
      pfVar11 = pfVar11 + 0xc;
      pfVar9 = pfVar9 + 0xc;
      iVar10 = iVar10 + -1;
      param_2[(int)*pfVar4] =
           *pfVar1 * *(float *)((int)this_00 + 0xd4) + *pfVar2 * *(float *)((int)this_00 + 0xd8) +
           fVar6 * *(float *)((int)this_00 + 0xd0) +
           *pfVar5 * *(float *)((int)this_00 + 0xc0) + *pfVar3 * *(float *)((int)this_00 + 200) +
           *(float *)((int)this_00 + 0xc4) * fVar7 + param_2[(int)*pfVar4];
    } while (-1 < iVar10);
    iVar10 = *(int *)(iVar8 + 0x94);
    pfVar11 = (float *)param_2[0x30];
    iVar12 = (int)param_2[0x2f] + -1;
    pfVar9 = pfVar11 + 5;
    do {
      pfVar1 = pfVar9 + -4;
      pfVar2 = pfVar9 + -3;
      pfVar3 = pfVar9 + -1;
      pfVar4 = pfVar9 + 3;
      fVar6 = *pfVar11;
      pfVar5 = pfVar9 + 1;
      fVar7 = *pfVar9;
      pfVar11 = pfVar11 + 0xc;
      pfVar9 = pfVar9 + 0xc;
      iVar12 = iVar12 + -1;
      param_2[(int)*pfVar4] =
           *pfVar1 * *(float *)(iVar10 + 0xd4) + *pfVar2 * *(float *)(iVar10 + 0xd8) +
           fVar6 * *(float *)(iVar10 + 0xd0) +
           *pfVar5 * *(float *)(iVar10 + 200) + *pfVar3 * *(float *)(iVar10 + 0xc0) +
           *(float *)(iVar10 + 0xc4) * fVar7 + param_2[(int)*pfVar4];
    } while (-1 < iVar12);
    local_10[0] = (param_2[0x34] * *(float *)((int)this + 0x58) * *(float *)(param_1 + 4) * param_3
                  - *(float *)((int)this + 0x5c) * *param_2 * param_4) * *(float *)param_2[0x24];
    FUN_100a53e0(this_00,(int *)(param_2 + 0x2c),(int)local_10);
    FUN_100a53e0(*(void **)(iVar8 + 0x94),(int *)(param_2 + 0x2f),(int)local_10);
  }
  return;
}


// 100a16c0 FUN_100a16c0

undefined4 __thiscall
FUN_100a16c0(void *this,float *param_1,float *param_2,float param_3,float param_4)

{
  int iVar1;
  int iVar2;
  int iVar3;
  int iVar4;
  float *pfVar5;
  float *pfVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  float fVar10;
  float fVar11;
  float fVar12;
  float fVar13;
  float fVar14;
  float fVar15;
  float local_f0;
  float local_ec;
  float local_e8;
  float local_e4;
  float local_e0;
  float local_dc;
  float local_d8;
  float local_cc;
  float local_c8;
  float local_c0;
  float local_bc;
  float local_b8;
  float local_b0 [4];
  float local_a0;
  float local_9c;
  float local_98;
  float local_90;
  float local_8c;
  float local_88;
  float local_80;
  float local_7c;
  float local_78;
  float local_6c;
  float local_68;
  float local_64;
  float local_60;
  float local_5c;
  float local_58;
  float local_54;
  float local_44;
  float local_40;
  float local_3c;
  float local_38;
  float local_34;
  float local_30;
  float local_2c;
  float local_28;
  float local_24;
  float local_20;
  float local_1c;
  float local_18;
  float local_14;
  
  if (param_2 == (float *)0x0) {
    param_2 = (float *)0x0;
  }
  else {
    param_2[0x24] = (float)(param_2 + 0x28);
    param_2[0x25] = 1.4013e-45;
    param_2[0x26] = 1.4013e-45;
    param_2[0x27] = 5.60519e-45;
  }
  iVar1 = *(int *)((int)this + 4);
  iVar2 = *(int *)((int)this + 8);
  iVar4 = *(int *)(iVar1 + 0x94);
  if ((*(uint *)(iVar1 + 0x44) & 0x800) == 0) {
    pfVar5 = (float *)(iVar4 + 0x60);
    pfVar6 = local_b0;
    for (iVar3 = 0x10; iVar3 != 0; iVar3 = iVar3 + -1) {
      *pfVar6 = *pfVar5;
      pfVar5 = pfVar5 + 1;
      pfVar6 = pfVar6 + 1;
    }
    FUN_10039500(local_b0,(float *)(iVar1 + 0x30),&local_80);
    local_e0 = local_b0[1];
    local_dc = local_9c;
    local_d8 = local_8c;
    local_cc = local_98;
    local_c8 = local_88;
    local_c0 = local_80;
    local_bc = local_7c;
    local_b8 = local_78;
    fVar7 = local_b0[0];
    fVar12 = local_a0;
    fVar14 = local_90;
    fVar8 = local_b0[2];
  }
  else {
    local_e0 = *(float *)(iVar4 + 100);
    local_dc = *(float *)(iVar4 + 0x74);
    local_d8 = *(float *)(iVar4 + 0x84);
    local_cc = *(float *)(iVar4 + 0x78);
    local_c8 = *(float *)(iVar4 + 0x88);
    local_c0 = *(float *)(iVar4 + 0x90);
    local_bc = *(float *)(iVar4 + 0x94);
    local_b8 = *(float *)(iVar4 + 0x98);
    fVar7 = *(float *)(iVar4 + 0x60);
    fVar12 = *(float *)(iVar4 + 0x70);
    fVar14 = *(float *)(iVar4 + 0x80);
    fVar8 = *(float *)(iVar4 + 0x68);
  }
  fVar13 = *(float *)((int)this + 0x20);
  fVar15 = *(float *)((int)this + 0x18);
  fVar9 = *(float *)((int)this + 0x1c);
  local_3c = fVar13 * fVar8 + fVar15 * fVar7 + fVar9 * local_e0 + local_c0;
  local_40 = fVar13 * local_cc + fVar9 * local_dc + fVar15 * fVar12 + local_bc;
  fVar7 = fVar9 * local_d8 + fVar15 * fVar14 + fVar13 * local_c8 + local_b8;
  if ((*(uint *)(iVar2 + 0x44) & 0x800) == 0) {
    pfVar5 = (float *)(*(int *)(iVar2 + 0x94) + 0x60);
    pfVar6 = local_b0;
    for (iVar4 = 0x10; iVar4 != 0; iVar4 = iVar4 + -1) {
      *pfVar6 = *pfVar5;
      pfVar5 = pfVar5 + 1;
      pfVar6 = pfVar6 + 1;
    }
    FUN_10039500(local_b0,(float *)(iVar2 + 0x30),&local_80);
    local_dc = local_9c;
    local_d8 = local_8c;
    local_cc = local_98;
    local_c8 = local_88;
    local_c0 = local_80;
    local_bc = local_7c;
    fVar12 = local_a0;
    fVar14 = local_90;
    fVar8 = local_b0[1];
    local_b8 = local_78;
    fVar13 = local_b0[0];
    fVar15 = local_b0[2];
  }
  else {
    iVar4 = *(int *)(iVar2 + 0x94);
    local_dc = *(float *)(iVar4 + 0x74);
    local_d8 = *(float *)(iVar4 + 0x84);
    local_cc = *(float *)(iVar4 + 0x78);
    local_c8 = *(float *)(iVar4 + 0x88);
    local_c0 = *(float *)(iVar4 + 0x90);
    local_bc = *(float *)(iVar4 + 0x94);
    fVar12 = *(float *)(iVar4 + 0x70);
    fVar14 = *(float *)(iVar4 + 0x80);
    fVar8 = *(float *)(iVar4 + 100);
    local_b8 = *(float *)(iVar4 + 0x98);
    fVar13 = *(float *)(iVar4 + 0x60);
    fVar15 = *(float *)(iVar4 + 0x68);
  }
  fVar9 = *(float *)((int)this + 0x2c);
  fVar10 = *(float *)((int)this + 0x28);
  fVar11 = *(float *)((int)this + 0x30);
  local_38 = fVar8 * fVar9 + fVar15 * fVar11 + fVar13 * fVar10 + local_c0;
  local_20 = fVar12 * fVar10 + local_dc * fVar9 + local_cc * fVar11 + local_bc;
  local_24 = *(float *)((int)this + 0x38) - local_3c;
  fVar13 = *(float *)((int)this + 0x40) - fVar7;
  local_28 = fVar14 * fVar10 + local_d8 * fVar9 + local_c8 * fVar11 + local_b8;
  local_1c = *(float *)((int)this + 0x3c) - local_40;
  local_18 = *(float *)((int)this + 100);
  local_34 = local_18 * (*(float *)((int)this + 0x4c) - local_20);
  fVar8 = local_18 * (*(float *)((int)this + 0x50) - local_28);
  local_18 = local_18 * (*(float *)((int)this + 0x48) - local_38);
  fVar12 = SQRT(fVar13 * fVar13 + local_1c * local_1c + local_24 * local_24);
  local_14 = 1.0 / (fVar12 + 1e-18);
  local_24 = local_14 * local_24;
  local_1c = local_14 * local_1c;
  local_14 = local_14 * fVar13;
  fVar14 = SQRT(fVar8 * fVar8 + local_34 * local_34 + local_18 * local_18);
  fVar12 = (fVar14 + fVar12) - *(float *)((int)this + 0x60);
  local_2c = 1.0 / (fVar14 + 1e-18);
  local_18 = local_2c * local_18;
  param_2[0x34] = fVar12;
  local_34 = local_2c * local_34;
  local_2c = local_2c * fVar8;
  if ((*(char *)((int)this + 0x68) != '\0') || (0.0 <= fVar12)) {
    *(undefined1 *)(param_2 + 0x35) = 0;
  }
  else {
    local_44 = *param_1;
    FUN_10033240(&local_60,(float *)(*(int *)(iVar1 + 0x94) + 0xc0),local_44);
    iVar4 = *(int *)(iVar1 + 0x94);
    local_64 = *(float *)(iVar4 + 0x110);
    fVar12 = *(float *)(iVar4 + 0x114);
    fVar14 = *(float *)(iVar4 + 0x11c);
    fVar8 = *(float *)(iVar4 + 0x118);
    local_f0 = (local_54 * local_64 + local_58 * fVar12 + local_60 * fVar14) - local_5c * fVar8;
    local_ec = (local_54 * fVar12 + local_5c * fVar14 + local_60 * fVar8) - local_58 * local_64;
    local_e8 = (local_54 * fVar8 + local_5c * local_64 + local_58 * fVar14) - local_60 * fVar12;
    local_e4 = ((local_54 * fVar14 - local_60 * local_64) - local_5c * fVar12) - local_58 * fVar8;
    FUN_10033710(&local_f0);
    FUN_10033320(&local_f0,local_b0);
    iVar4 = *(int *)(iVar1 + 0x94);
    local_80 = local_44 * *(float *)(iVar4 + 0xd0) + *(float *)(iVar4 + 0x90);
    local_7c = *(float *)(iVar4 + 0xd4) * local_44 + *(float *)(iVar4 + 0x94);
    local_78 = *(float *)(iVar4 + 0xd8) * local_44 + *(float *)(iVar4 + 0x98);
    if ((*(uint *)(iVar1 + 0x44) & 0x800) == 0) {
      FUN_10039500(local_b0,(float *)(iVar1 + 0x30),&local_80);
    }
    fVar12 = *(float *)((int)this + 0x1c);
    fVar14 = *(float *)((int)this + 0x18);
    fVar8 = *(float *)((int)this + 0x20);
    local_64 = *param_1;
    local_30 = local_b0[0] * fVar14 + local_b0[1] * fVar12 + local_b0[2] * fVar8 + local_80;
    local_68 = local_a0 * fVar14 + local_9c * fVar12 + local_98 * fVar8 + local_7c;
    local_6c = local_90 * fVar14 + local_8c * fVar12 + local_88 * fVar8 + local_78;
    FUN_10033240(&local_60,(float *)(*(int *)(iVar2 + 0x94) + 0xc0),local_64);
    iVar4 = *(int *)(iVar2 + 0x94);
    fVar12 = *(float *)(iVar4 + 0x118);
    fVar14 = *(float *)(iVar4 + 0x11c);
    local_44 = *(float *)(iVar4 + 0x110);
    fVar8 = *(float *)(iVar4 + 0x114);
    local_f0 = (local_60 * fVar14 + local_54 * local_44 + local_58 * fVar8) - local_5c * fVar12;
    local_ec = (local_60 * fVar12 + local_5c * fVar14 + local_54 * fVar8) - local_58 * local_44;
    local_e8 = (local_54 * fVar12 + local_5c * local_44 + local_58 * fVar14) - local_60 * fVar8;
    local_e4 = ((local_54 * fVar14 - local_60 * local_44) - local_5c * fVar8) - local_58 * fVar12;
    FUN_10033710(&local_f0);
    FUN_10033320(&local_f0,local_b0);
    iVar4 = *(int *)(iVar2 + 0x94);
    local_80 = *(float *)(iVar4 + 0xd0) * local_64 + *(float *)(iVar4 + 0x90);
    local_7c = *(float *)(iVar4 + 0xd4) * local_64 + *(float *)(iVar4 + 0x94);
    local_78 = *(float *)(iVar4 + 0xd8) * local_64 + *(float *)(iVar4 + 0x98);
    if ((*(uint *)(iVar2 + 0x44) & 0x800) == 0) {
      FUN_10039500(local_b0,(float *)(iVar2 + 0x30),&local_80);
    }
    fVar12 = *(float *)((int)this + 0x2c);
    fVar13 = *(float *)((int)this + 0x38) - local_30;
    fVar14 = *(float *)((int)this + 0x28);
    fVar8 = *(float *)((int)this + 0x30);
    local_68 = *(float *)((int)this + 0x3c) - local_68;
    local_6c = *(float *)((int)this + 0x40) - local_6c;
    local_68 = SQRT(local_68 * local_68 + local_6c * local_6c + fVar13 * fVar13);
    local_dc = *(float *)((int)this + 100);
    local_e0 = local_dc *
               (*(float *)((int)this + 0x48) -
               (fVar14 * local_b0[0] + local_b0[1] * fVar12 + local_b0[2] * fVar8 + local_80));
    local_d8 = local_dc *
               (*(float *)((int)this + 0x50) -
               (fVar14 * local_90 + local_8c * fVar12 + local_88 * fVar8 + local_78));
    local_dc = local_dc *
               (*(float *)((int)this + 0x4c) -
               (fVar14 * local_a0 + local_9c * fVar12 + local_98 * fVar8 + local_7c));
    local_6c = SQRT(local_e0 * local_e0 + local_dc * local_dc + local_d8 * local_d8);
    if ((local_6c + local_68) - *(float *)((int)this + 0x60) < *(float *)((int)this + 0x60) * 0.01)
    {
      *(undefined1 *)(param_2 + 0x35) = 1;
    }
    else {
      *(undefined1 *)(param_2 + 0x35) = 0;
    }
  }
  if (*(char *)(param_2 + 0x35) == '\0') {
    FUN_10096f70(param_2);
    pfVar5 = (float *)((int)param_2[0x32] + 0x30 + (int)param_2);
    iVar4 = *(int *)(iVar1 + 0x94);
    fVar12 = *(float *)(iVar4 + 0x94);
    fVar14 = local_3c - *(float *)(iVar4 + 0x90);
    fVar7 = fVar7 - *(float *)(iVar4 + 0x98);
    *pfVar5 = local_24;
    pfVar5[1] = local_1c;
    pfVar5[2] = local_14;
    fVar12 = local_40 - fVar12;
    fVar10 = *pfVar5 * fVar7 - pfVar5[2] * fVar14;
    pfVar5[5] = fVar10;
    fVar11 = pfVar5[2] * fVar12 - pfVar5[1] * fVar7;
    pfVar5[4] = fVar11;
    fVar9 = pfVar5[1] * fVar14 - *pfVar5 * fVar12;
    pfVar5[6] = fVar9;
    iVar4 = *(int *)(iVar1 + 0x94);
    fVar7 = *(float *)(iVar4 + 0x78);
    fVar12 = *(float *)(iVar4 + 0x68);
    fVar14 = *(float *)(iVar4 + 0x60);
    fVar8 = *(float *)(iVar4 + 0x88);
    fVar13 = *(float *)(iVar4 + 0x70);
    fVar15 = *(float *)(iVar4 + 0x80);
    pfVar5[5] = *(float *)(iVar4 + 100) * fVar11 + *(float *)(iVar4 + 0x74) * fVar10 +
                *(float *)(iVar4 + 0x84) * fVar9;
    pfVar5[6] = fVar12 * fVar11 + fVar7 * fVar10 + fVar8 * fVar9;
    pfVar5[4] = fVar13 * fVar10 + fVar15 * fVar9 + fVar11 * fVar14;
    pfVar5[8] = param_2[0x33];
    iVar4 = *(int *)(iVar2 + 0x94);
    pfVar5 = (float *)((int)param_2[0x32] + 0x60 + (int)param_2);
    fVar7 = *(float *)(iVar4 + 0x94);
    fVar12 = local_38 - *(float *)(iVar4 + 0x90);
    local_e8 = local_28 - *(float *)(iVar4 + 0x98);
    *pfVar5 = local_18;
    pfVar5[1] = local_34;
    pfVar5[2] = local_2c;
    fVar7 = local_20 - fVar7;
    fVar10 = *pfVar5 * local_e8 - pfVar5[2] * fVar12;
    pfVar5[5] = fVar10;
    fVar11 = pfVar5[2] * fVar7 - pfVar5[1] * local_e8;
    pfVar5[4] = fVar11;
    fVar9 = pfVar5[1] * fVar12 - *pfVar5 * fVar7;
    pfVar5[6] = fVar9;
    iVar4 = *(int *)(iVar2 + 0x94);
    fVar7 = *(float *)(iVar4 + 0x78);
    fVar12 = *(float *)(iVar4 + 0x68);
    fVar14 = *(float *)(iVar4 + 0x60);
    fVar8 = *(float *)(iVar4 + 0x88);
    fVar13 = *(float *)(iVar4 + 0x70);
    fVar15 = *(float *)(iVar4 + 0x80);
    pfVar5[5] = *(float *)(iVar4 + 100) * fVar11 + *(float *)(iVar4 + 0x74) * fVar10 +
                *(float *)(iVar4 + 0x84) * fVar9;
    pfVar5[6] = fVar12 * fVar11 + fVar7 * fVar10 + fVar8 * fVar9;
    pfVar5[4] = fVar13 * fVar10 + fVar15 * fVar9 + fVar11 * fVar14;
    pfVar5[8] = param_2[0x33];
    param_2[0x33] = (float)((int)param_2[0x33] + 1);
    param_2[0x32] = (float)((int)param_2[0x32] + 0x30);
    pfVar5 = param_2 + 0x2f;
    pfVar6 = param_2 + 0x2c;
    *pfVar6 = param_2[0x33];
    *pfVar5 = param_2[0x33];
    FUN_100a4e60(*(void **)(iVar1 + 0x94),(int *)pfVar6,(int *)(param_2 + 0x24),(int)param_2);
    *pfVar6 = param_2[0x33];
    *pfVar5 = param_2[0x33];
    FUN_100a4e60(*(void **)(iVar2 + 0x94),(int *)pfVar5,(int *)(param_2 + 0x24),(int)param_2);
    *(float *)param_2[0x24] = 1.0 / *(float *)param_2[0x24];
    local_f0 = (*(float *)((int)this + 0x58) * param_1[1] * param_2[0x34] * param_3 -
               *(float *)((int)this + 0x5c) * *param_2 * param_4) * *(float *)param_2[0x24];
    FUN_100a53e0(*(void **)(iVar1 + 0x94),(int *)pfVar6,(int)&local_f0);
    FUN_100a53e0(*(void **)(iVar2 + 0x94),(int *)pfVar5,(int)&local_f0);
  }
  return 0xe0;
}


// 100a2660 FUN_100a2660

void __thiscall FUN_100a2660(void *this,undefined4 *param_1)

{
  *(undefined4 *)((int)this + 0x38) = param_1[1];
  *(undefined4 *)((int)this + 0x3c) = param_1[2];
  *(undefined4 *)((int)this + 0x40) = *param_1;
  *(undefined4 *)((int)this + 0x18) = param_1[3];
  *(undefined4 *)((int)this + 0x1c) = param_1[4];
  *(undefined4 *)((int)this + 0x20) = param_1[5];
  *(undefined4 *)((int)this + 0x28) = param_1[7];
  *(undefined4 *)((int)this + 0x2c) = param_1[8];
  *(undefined4 *)((int)this + 0x30) = param_1[9];
  *(undefined4 *)((int)this + 0x44) = param_1[0xb];
  return;
}


// 100a26b0 FUN_100a26b0

void __thiscall FUN_100a26b0(void *this,undefined4 *param_1)

{
  param_1[1] = *(undefined4 *)((int)this + 0x38);
  param_1[2] = *(undefined4 *)((int)this + 0x3c);
  *param_1 = *(undefined4 *)((int)this + 0x40);
  param_1[3] = *(undefined4 *)((int)this + 0x18);
  param_1[4] = *(undefined4 *)((int)this + 0x1c);
  param_1[5] = *(undefined4 *)((int)this + 0x20);
  param_1[7] = *(undefined4 *)((int)this + 0x28);
  param_1[8] = *(undefined4 *)((int)this + 0x2c);
  param_1[9] = *(undefined4 *)((int)this + 0x30);
  param_1[0xb] = *(undefined4 *)((int)this + 0x44);
  return;
}


// 100a2700 FUN_100a2700

undefined4 * __thiscall
FUN_100a2700(void *this,undefined4 param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4
            )

{
  FUN_100a4de0(this,param_1,param_3,param_4);
  *(undefined4 *)((int)this + 0x10) = 0;
  *(undefined4 *)((int)this + 0x14) = 0;
  *(undefined ***)this = &PTR_LAB_100ce834;
  FUN_100a2660(this,param_2);
  return (undefined4 *)this;
}


// 100a2750 FUN_100a2750

undefined4 * __thiscall
FUN_100a2750(void *this,void *param_1,undefined4 *param_2,undefined4 param_3,undefined4 param_4)

{
  FUN_100a4e00(this,param_1,param_3,param_4,0x195,0xe0);
  *(undefined ***)this = &PTR_LAB_100ce834;
  FUN_100a2660(this,param_2);
  return (undefined4 *)this;
}


// 100a2790 FUN_100a2790

undefined4 * __thiscall FUN_100a2790(void *this,byte param_1)

{
  *(undefined ***)this = &PTR_LAB_100ce834;
  FUN_100a4e40((undefined4 *)this);
  if ((param_1 & 1) != 0) {
    (**(code **)(**(int **)g_pMemAlloc_exref + 0x14))
              (*(undefined4 *)(((uint)this & 0xfffffffc) - 4));
  }
  return (undefined4 *)this;
}


// 100a27e0 FUN_100a27e0

void __thiscall FUN_100a27e0(void *this,int param_1,float *param_2,float param_3,float param_4)

{
  float *pfVar1;
  float *pfVar2;
  float *pfVar3;
  float *pfVar4;
  float *pfVar5;
  float fVar6;
  float fVar7;
  int iVar8;
  void *this_00;
  float *pfVar9;
  int iVar10;
  float *pfVar11;
  int iVar12;
  float local_10 [4];
  
  if (*(char *)(param_2 + 0x35) == '\0') {
    *param_2 = 0.0;
    iVar8 = *(int *)((int)this + 8);
    this_00 = *(void **)(*(int *)((int)this + 4) + 0x94);
    pfVar11 = (float *)param_2[0x2d];
    iVar10 = (int)param_2[0x2c] + -1;
    pfVar9 = pfVar11 + 5;
    do {
      pfVar1 = pfVar9 + -4;
      pfVar2 = pfVar9 + -3;
      pfVar3 = pfVar9 + 1;
      pfVar4 = pfVar9 + 3;
      fVar6 = *pfVar11;
      pfVar5 = pfVar9 + -1;
      fVar7 = *pfVar9;
      pfVar11 = pfVar11 + 0xc;
      pfVar9 = pfVar9 + 0xc;
      iVar10 = iVar10 + -1;
      param_2[(int)*pfVar4] =
           *pfVar1 * *(float *)((int)this_00 + 0xd4) + *pfVar2 * *(float *)((int)this_00 + 0xd8) +
           fVar6 * *(float *)((int)this_00 + 0xd0) +
           *pfVar5 * *(float *)((int)this_00 + 0xc0) + *pfVar3 * *(float *)((int)this_00 + 200) +
           *(float *)((int)this_00 + 0xc4) * fVar7 + param_2[(int)*pfVar4];
    } while (-1 < iVar10);
    iVar10 = *(int *)(iVar8 + 0x94);
    pfVar11 = (float *)param_2[0x30];
    iVar12 = (int)param_2[0x2f] + -1;
    pfVar9 = pfVar11 + 5;
    do {
      pfVar1 = pfVar9 + -4;
      pfVar2 = pfVar9 + -3;
      pfVar3 = pfVar9 + -1;
      pfVar4 = pfVar9 + 3;
      fVar6 = *pfVar11;
      pfVar5 = pfVar9 + 1;
      fVar7 = *pfVar9;
      pfVar11 = pfVar11 + 0xc;
      pfVar9 = pfVar9 + 0xc;
      iVar12 = iVar12 + -1;
      param_2[(int)*pfVar4] =
           *pfVar1 * *(float *)(iVar10 + 0xd4) + *pfVar2 * *(float *)(iVar10 + 0xd8) +
           fVar6 * *(float *)(iVar10 + 0xd0) +
           *pfVar5 * *(float *)(iVar10 + 200) + *pfVar3 * *(float *)(iVar10 + 0xc0) +
           *(float *)(iVar10 + 0xc4) * fVar7 + param_2[(int)*pfVar4];
    } while (-1 < iVar12);
    local_10[0] = (param_2[0x34] * *(float *)(param_1 + 4) * *(float *)((int)this + 0x38) * param_3
                  - *(float *)((int)this + 0x3c) * *param_2 * param_4) * *(float *)param_2[0x24];
    FUN_100a53e0(this_00,(int *)(param_2 + 0x2c),(int)local_10);
    FUN_100a53e0(*(void **)(iVar8 + 0x94),(int *)(param_2 + 0x2f),(int)local_10);
  }
  return;
}


// 100a29c0 FUN_100a29c0

undefined4 __thiscall
FUN_100a29c0(void *this,float *param_1,float *param_2,float param_3,float param_4)

{
  float fVar1;
  float fVar2;
  int iVar3;
  void *pvVar4;
  int iVar5;
  int iVar6;
  float *pfVar7;
  float *pfVar8;
  float fVar9;
  float fVar10;
  float fVar11;
  float fVar12;
  float fVar13;
  float fVar14;
  float fVar15;
  float fVar16;
  float fVar17;
  float local_100;
  float local_fc;
  float local_f8;
  float local_f4;
  int local_e4;
  float local_e0;
  float local_dc;
  float local_d8;
  float local_d4;
  float local_d0;
  float local_cc;
  float local_c8;
  float local_c0;
  float local_bc;
  float local_b8;
  float local_b0;
  float local_ac;
  float local_a8;
  float local_a0;
  float local_9c;
  float local_98;
  float local_84;
  float local_80;
  float local_7c;
  float local_78;
  float local_74;
  void *local_70;
  float local_6c;
  float local_68;
  float local_64;
  float local_60;
  float local_5c;
  float local_58;
  float local_54;
  float local_50 [4];
  float local_40;
  float local_3c;
  float local_38;
  float local_30;
  float local_2c;
  float local_28;
  float local_20;
  float local_1c;
  float local_18;
  
  if (param_2 == (float *)0x0) {
    param_2 = (float *)0x0;
  }
  else {
    param_2[0x24] = (float)(param_2 + 0x28);
    param_2[0x25] = 1.4013e-45;
    param_2[0x26] = 1.4013e-45;
    param_2[0x27] = 5.60519e-45;
  }
  local_e4 = *(int *)((int)this + 4);
  iVar3 = *(int *)((int)this + 8);
  iVar6 = *(int *)(local_e4 + 0x94);
  local_70 = this;
  if ((*(uint *)(local_e4 + 0x44) & 0x800) == 0) {
    pfVar7 = (float *)(iVar6 + 0x60);
    pfVar8 = local_50;
    for (iVar5 = 0x10; iVar5 != 0; iVar5 = iVar5 + -1) {
      *pfVar8 = *pfVar7;
      pfVar7 = pfVar7 + 1;
      pfVar8 = pfVar8 + 1;
    }
    FUN_10039500(local_50,(float *)(local_e4 + 0x30),&local_20);
    local_c8 = local_30;
    local_bc = local_3c;
    local_b8 = local_2c;
    local_ac = local_38;
    local_a8 = local_28;
    local_a0 = local_20;
    local_9c = local_1c;
    local_98 = local_18;
    fVar17 = local_50[1];
    fVar10 = local_50[0];
    fVar11 = local_50[2];
    fVar9 = local_40;
  }
  else {
    local_c8 = *(float *)(iVar6 + 0x80);
    local_bc = *(float *)(iVar6 + 0x74);
    local_b8 = *(float *)(iVar6 + 0x84);
    local_ac = *(float *)(iVar6 + 0x78);
    local_a8 = *(float *)(iVar6 + 0x88);
    local_a0 = *(float *)(iVar6 + 0x90);
    local_9c = *(float *)(iVar6 + 0x94);
    local_98 = *(float *)(iVar6 + 0x98);
    fVar17 = *(float *)(iVar6 + 100);
    fVar10 = *(float *)(iVar6 + 0x60);
    fVar11 = *(float *)(iVar6 + 0x68);
    fVar9 = *(float *)(iVar6 + 0x70);
  }
  fVar14 = *(float *)((int)local_70 + 0x1c);
  fVar1 = *(float *)((int)local_70 + 0x18);
  fVar2 = *(float *)((int)local_70 + 0x20);
  local_7c = fVar17 * fVar14 + fVar2 * fVar11 + fVar1 * fVar10 + local_a0;
  local_74 = fVar2 * local_ac + fVar1 * fVar9 + fVar14 * local_bc + local_9c;
  local_80 = fVar2 * local_a8 + fVar1 * local_c8 + fVar14 * local_b8 + local_98;
  if ((*(uint *)(iVar3 + 0x44) & 0x800) == 0) {
    pfVar7 = (float *)(*(int *)(iVar3 + 0x94) + 0x60);
    pfVar8 = local_50;
    for (iVar6 = 0x10; iVar6 != 0; iVar6 = iVar6 + -1) {
      *pfVar8 = *pfVar7;
      pfVar7 = pfVar7 + 1;
      pfVar8 = pfVar8 + 1;
    }
    FUN_10039500(local_50,(float *)(iVar3 + 0x30),&local_20);
    local_c8 = local_30;
    local_c0 = local_50[1];
    local_bc = local_3c;
    local_b8 = local_2c;
    local_ac = local_38;
    local_a8 = local_28;
    local_a0 = local_20;
    local_9c = local_1c;
  }
  else {
    iVar6 = *(int *)(iVar3 + 0x94);
    local_c8 = *(float *)(iVar6 + 0x80);
    local_50[0] = *(float *)(iVar6 + 0x60);
    local_40 = *(float *)(iVar6 + 0x70);
    local_50[2] = *(float *)(iVar6 + 0x68);
    local_c0 = *(float *)(iVar6 + 100);
    local_bc = *(float *)(iVar6 + 0x74);
    local_b8 = *(float *)(iVar6 + 0x84);
    local_ac = *(float *)(iVar6 + 0x78);
    local_a8 = *(float *)(iVar6 + 0x88);
    local_a0 = *(float *)(iVar6 + 0x90);
    local_9c = *(float *)(iVar6 + 0x94);
    local_18 = *(float *)(iVar6 + 0x98);
  }
  pvVar4 = local_70;
  fVar17 = *(float *)((int)local_70 + 0x28);
  fVar10 = *(float *)((int)local_70 + 0x30);
  fVar11 = *(float *)((int)local_70 + 0x2c);
  local_5c = (fVar17 * local_50[0] + fVar10 * local_50[2] + fVar11 * local_c0 + local_a0) - local_7c
  ;
  fVar9 = (fVar11 * local_b8 + fVar10 * local_a8 + fVar17 * local_c8 + local_18) - local_80;
  local_6c = (fVar10 * local_ac + fVar17 * local_40 + fVar11 * local_bc + local_9c) - local_74;
  fVar17 = SQRT(fVar9 * fVar9 + local_6c * local_6c + local_5c * local_5c);
  fVar10 = 1.0 / (fVar17 + 1e-18);
  local_5c = fVar10 * local_5c;
  local_6c = fVar10 * local_6c;
  if (*(float *)((int)local_70 + 0x44) <= fVar17) {
    if (fVar17 <= *(float *)((int)local_70 + 0x40)) {
      fVar17 = 0.0;
    }
    else {
      fVar17 = fVar17 - *(float *)((int)local_70 + 0x40);
    }
  }
  else {
    fVar17 = fVar17 - *(float *)((int)local_70 + 0x44);
  }
  param_2[0x34] = fVar17;
  local_98 = local_18;
  if ((*(float *)((int)local_70 + 0x44) == *(float *)((int)local_70 + 0x40)) || (fVar17 != 0.0)) {
    *(undefined1 *)(param_2 + 0x35) = 0;
  }
  else {
    local_84 = *param_1;
    FUN_10033240(&local_e0,(float *)(*(int *)(local_e4 + 0x94) + 0xc0),local_84);
    iVar5 = local_e4;
    iVar6 = *(int *)(local_e4 + 0x94);
    fVar17 = *(float *)(iVar6 + 0x11c);
    fVar11 = *(float *)(iVar6 + 0x110);
    fVar14 = *(float *)(iVar6 + 0x114);
    fVar1 = *(float *)(iVar6 + 0x118);
    local_100 = (fVar17 * local_e0 + fVar11 * local_d4 + fVar14 * local_d8) - fVar1 * local_dc;
    local_fc = (fVar14 * local_d4 + fVar17 * local_dc + fVar1 * local_e0) - fVar11 * local_d8;
    local_f8 = (fVar1 * local_d4 + fVar11 * local_dc + fVar17 * local_d8) - fVar14 * local_e0;
    local_f4 = ((fVar17 * local_d4 - fVar11 * local_e0) - fVar14 * local_dc) - fVar1 * local_d8;
    FUN_10033710(&local_100);
    FUN_10033320(&local_100,&local_d0);
    iVar6 = *(int *)(iVar5 + 0x94);
    local_a0 = local_84 * *(float *)(iVar6 + 0xd0) + *(float *)(iVar6 + 0x90);
    local_9c = *(float *)(iVar6 + 0xd4) * local_84 + *(float *)(iVar6 + 0x94);
    local_98 = *(float *)(iVar6 + 0xd8) * local_84 + *(float *)(iVar6 + 0x98);
    if ((*(uint *)(iVar5 + 0x44) & 0x800) == 0) {
      FUN_10039500(&local_d0,(float *)(iVar5 + 0x30),&local_a0);
    }
    fVar17 = *(float *)((int)pvVar4 + 0x1c);
    fVar11 = *(float *)((int)pvVar4 + 0x18);
    fVar14 = *(float *)((int)pvVar4 + 0x20);
    local_58 = *param_1;
    local_54 = local_d0 * fVar11 + local_cc * fVar17 + local_c8 * fVar14 + local_a0;
    local_60 = local_c0 * fVar11 + local_bc * fVar17 + local_b8 * fVar14 + local_9c;
    local_78 = local_b0 * fVar11 + local_ac * fVar17 + local_a8 * fVar14 + local_98;
    FUN_10033240(&local_e0,(float *)(*(int *)(iVar3 + 0x94) + 0xc0),local_58);
    iVar6 = *(int *)(iVar3 + 0x94);
    fVar17 = *(float *)(iVar6 + 0x118);
    local_84 = *(float *)(iVar6 + 0x11c);
    local_64 = *(float *)(iVar6 + 0x110);
    local_68 = *(float *)(iVar6 + 0x114);
    local_100 = (local_e0 * local_84 + local_d4 * local_64 + local_d8 * local_68) -
                local_dc * fVar17;
    local_fc = (local_e0 * fVar17 + local_dc * local_84 + local_d4 * local_68) - local_d8 * local_64
    ;
    local_f8 = (local_d4 * fVar17 + local_dc * local_64 + local_d8 * local_84) - local_e0 * local_68
    ;
    local_f4 = ((local_d4 * local_84 - local_e0 * local_64) - local_dc * local_68) -
               local_d8 * fVar17;
    FUN_10033710(&local_100);
    FUN_10033320(&local_100,&local_d0);
    iVar6 = *(int *)(iVar3 + 0x94);
    local_a0 = *(float *)(iVar6 + 0xd0) * local_58 + *(float *)(iVar6 + 0x90);
    local_9c = *(float *)(iVar6 + 0xd4) * local_58 + *(float *)(iVar6 + 0x94);
    local_98 = *(float *)(iVar6 + 0xd8) * local_58 + *(float *)(iVar6 + 0x98);
    if ((*(uint *)(iVar3 + 0x44) & 0x800) == 0) {
      FUN_10039500(&local_d0,(float *)(iVar3 + 0x30),&local_a0);
    }
    fVar17 = *(float *)((int)pvVar4 + 0x2c);
    fVar11 = *(float *)((int)pvVar4 + 0x28);
    fVar14 = *(float *)((int)pvVar4 + 0x30);
    local_100 = (local_d0 * fVar11 + local_cc * fVar17 + local_c8 * fVar14 + local_a0) - local_54;
    local_fc = (local_c0 * fVar11 + local_bc * fVar17 + local_b8 * fVar14 + local_9c) - local_60;
    local_f8 = (local_b0 * fVar11 + local_ac * fVar17 + local_a8 * fVar14 + local_98) - local_78;
    fVar17 = *(float *)((int)pvVar4 + 0x44);
    local_78 = SQRT(local_100 * local_100 + local_fc * local_fc + local_f8 * local_f8);
    if (((fVar17 <= local_78) &&
        (fVar17 = *(float *)((int)pvVar4 + 0x40), local_78 <= *(float *)((int)pvVar4 + 0x40))) ||
       (local_78 - fVar17 == 0.0)) {
      *(undefined1 *)(param_2 + 0x35) = 1;
    }
    else {
      *(undefined1 *)(param_2 + 0x35) = 0;
    }
  }
  if (*(char *)(param_2 + 0x35) == '\0') {
    FUN_10096f70(param_2);
    pfVar7 = (float *)((int)param_2[0x32] + 0x30 + (int)param_2);
    iVar6 = *(int *)(local_e4 + 0x94);
    fVar17 = *(float *)(iVar6 + 0x90);
    fVar11 = *(float *)(iVar6 + 0x94);
    fVar14 = *(float *)(iVar6 + 0x98);
    *pfVar7 = local_5c;
    fVar17 = local_7c - fVar17;
    fVar11 = local_74 - fVar11;
    fVar14 = local_80 - fVar14;
    pfVar7[1] = local_6c;
    pfVar7[2] = fVar10 * fVar9;
    fVar15 = fVar17 * pfVar7[1] - fVar11 * local_5c;
    fVar16 = fVar14 * local_5c - fVar17 * pfVar7[2];
    pfVar7[5] = fVar16;
    pfVar7[6] = fVar15;
    fVar12 = fVar11 * pfVar7[2] - fVar14 * pfVar7[1];
    pfVar7[4] = fVar12;
    iVar6 = *(int *)(local_e4 + 0x94);
    fVar17 = *(float *)(iVar6 + 0x78);
    fVar11 = *(float *)(iVar6 + 0x68);
    fVar14 = *(float *)(iVar6 + 0x60);
    fVar1 = *(float *)(iVar6 + 0x88);
    fVar2 = *(float *)(iVar6 + 0x70);
    fVar13 = *(float *)(iVar6 + 0x80);
    pfVar7[5] = *(float *)(iVar6 + 100) * fVar12 + *(float *)(iVar6 + 0x74) * fVar16 +
                *(float *)(iVar6 + 0x84) * fVar15;
    pfVar7[6] = fVar11 * fVar12 + fVar17 * fVar16 + fVar1 * fVar15;
    pfVar7[4] = fVar2 * fVar16 + fVar13 * fVar15 + fVar12 * fVar14;
    pfVar7[8] = param_2[0x33];
    iVar6 = *(int *)(iVar3 + 0x94);
    pfVar7 = (float *)((int)param_2[0x32] + 0x60 + (int)param_2);
    fVar17 = local_7c - *(float *)(iVar6 + 0x90);
    fVar11 = local_74 - *(float *)(iVar6 + 0x94);
    local_f8 = local_80 - *(float *)(iVar6 + 0x98);
    pfVar7[1] = local_6c * -1.0;
    pfVar7[2] = fVar10 * fVar9 * -1.0;
    fVar10 = local_5c * -1.0;
    *pfVar7 = fVar10;
    fVar12 = fVar10 * local_f8 - pfVar7[2] * fVar17;
    pfVar7[5] = fVar12;
    pfVar7[4] = pfVar7[2] * fVar11 - pfVar7[1] * local_f8;
    fVar13 = pfVar7[1] * fVar17 - fVar10 * fVar11;
    pfVar7[6] = fVar13;
    iVar6 = *(int *)(iVar3 + 0x94);
    fVar17 = pfVar7[4];
    fVar10 = *(float *)(iVar6 + 0x78);
    fVar11 = *(float *)(iVar6 + 0x68);
    fVar9 = *(float *)(iVar6 + 0x88);
    fVar14 = *(float *)(iVar6 + 0x70);
    fVar1 = *(float *)(iVar6 + 0x80);
    fVar2 = *(float *)(iVar6 + 0x60);
    pfVar7[5] = *(float *)(iVar6 + 100) * fVar17 + *(float *)(iVar6 + 0x74) * fVar12 +
                *(float *)(iVar6 + 0x84) * fVar13;
    pfVar7[6] = fVar11 * fVar17 + fVar10 * fVar12 + fVar9 * fVar13;
    pfVar7[4] = fVar14 * fVar12 + fVar1 * fVar13 + fVar2 * fVar17;
    pfVar7[8] = param_2[0x33];
    param_2[0x33] = (float)((int)param_2[0x33] + 1);
    param_2[0x32] = (float)((int)param_2[0x32] + 0x30);
    pfVar7 = param_2 + 0x2c;
    pfVar8 = param_2 + 0x2f;
    *pfVar7 = param_2[0x33];
    *pfVar8 = param_2[0x33];
    FUN_100a4e60(*(void **)(local_e4 + 0x94),(int *)pfVar7,(int *)(param_2 + 0x24),(int)param_2);
    *pfVar7 = param_2[0x33];
    *pfVar8 = param_2[0x33];
    FUN_100a4e60(*(void **)(iVar3 + 0x94),(int *)pfVar8,(int *)(param_2 + 0x24),(int)param_2);
    *(float *)param_2[0x24] = 1.0 / *(float *)param_2[0x24];
    local_100 = (param_1[1] * *(float *)((int)local_70 + 0x38) * param_2[0x34] * param_3 -
                *(float *)((int)local_70 + 0x3c) * *param_2 * param_4) * *(float *)param_2[0x24];
    FUN_100a53e0(*(void **)(local_e4 + 0x94),(int *)pfVar7,(int)&local_100);
    FUN_100a53e0(*(void **)(iVar3 + 0x94),(int *)pfVar8,(int)&local_100);
  }
  return 0xe0;
}


// 100a38b0 FUN_100a38b0

void __thiscall FUN_100a38b0(void *this,int param_1,undefined4 *param_2)

{
  undefined4 *puVar1;
  
  puVar1 = (undefined4 *)((param_1 + 1) * 0x20 + (int)this);
  *puVar1 = *param_2;
  puVar1[1] = param_2[1];
  puVar1[2] = param_2[2];
  return;
}


// 100a38e0 FUN_100a38e0

void __thiscall FUN_100a38e0(void *this,undefined4 param_1,float param_2)

{
  *(bool *)((int)this + 9) = param_2 != 0.0;
  *(float *)((int)this + 0x18) = ABS(param_2);
  *(undefined4 *)((int)this + 0x1c) = param_1;
  return;
}


// 100a3920 FUN_100a3920

void __thiscall FUN_100a3920(void *this,float param_1,float param_2)

{
  if ((0.0001 < ABS(param_1 + 3.1415927)) || (0.0001 < ABS(param_2 - 3.1415927))) {
    *(undefined1 *)((int)this + 8) = 1;
  }
  else {
    *(undefined1 *)((int)this + 8) = 0;
  }
  *(float *)((int)this + 0xc) = param_1;
  *(float *)((int)this + 0x10) = param_2;
  return;
}


// 100a3980 FUN_100a3980

void __thiscall FUN_100a3980(void *this,undefined4 param_1)

{
  *(undefined4 *)this = param_1;
  return;
}


// 100a3990 FUN_100a3990

void __thiscall FUN_100a3990(void *this,int param_1,float *param_2)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float *pfVar4;
  float fVar5;
  float fVar6;
  
  fVar1 = *param_2;
  fVar2 = param_2[1];
  fVar3 = param_2[2];
  fVar6 = fVar1 * fVar1 + fVar2 * fVar2 + fVar3 * fVar3;
  fVar5 = 0.0;
  if (fVar6 != 0.0) {
    fVar5 = 1.0 / SQRT(fVar6);
  }
  pfVar4 = (float *)((param_1 + 6) * 0x10 + (int)this);
  *pfVar4 = fVar5 * fVar1;
  pfVar4[1] = fVar5 * fVar2;
  pfVar4[2] = fVar5 * fVar3;
  return;
}


// 100a3a20 FUN_100a3a20

void __thiscall FUN_100a3a20(void *this,int param_1,int param_2,float *param_3)

{
  float fVar1;
  int iVar2;
  float *pfVar3;
  float *pfVar4;
  float local_b0;
  undefined4 local_ac;
  undefined4 local_a8;
  undefined4 local_a4;
  undefined4 local_a0;
  undefined4 local_9c;
  undefined4 local_98;
  undefined4 local_94;
  undefined4 local_90;
  undefined4 local_8c;
  undefined4 local_88;
  undefined4 local_84;
  float local_80;
  undefined4 local_7c;
  undefined4 local_78;
  undefined4 local_74;
  float local_68;
  void *local_64;
  float local_60 [4];
  undefined4 local_50;
  undefined4 local_4c;
  undefined4 local_48;
  undefined4 local_40;
  undefined4 local_3c;
  undefined4 local_38;
  float local_30;
  undefined4 local_2c;
  undefined4 local_28;
  float local_20;
  float local_1c;
  float local_18;
  
  local_20 = *param_3;
  fVar1 = param_3[2];
  local_1c = param_3[1];
  local_68 = local_20 * local_20 + fVar1 * fVar1 + local_1c * local_1c;
  if (local_68 == 0.0) {
    local_18 = 0.0;
  }
  else {
    local_18 = 1.0 / SQRT(local_68);
    local_68 = local_18;
  }
  local_20 = local_18 * local_20;
  local_1c = local_18 * local_1c;
  local_18 = local_18 * fVar1;
  local_64 = this;
  if ((*(uint *)(param_1 + 0x44) & 0x800) == 0) {
    pfVar3 = (float *)(*(int *)(param_1 + 0x94) + 0x60);
    pfVar4 = local_60;
    for (iVar2 = 0x10; iVar2 != 0; iVar2 = iVar2 + -1) {
      *pfVar4 = *pfVar3;
      pfVar3 = pfVar3 + 1;
      pfVar4 = pfVar4 + 1;
    }
    FUN_10039500(local_60,(float *)(param_1 + 0x30),&local_30);
    local_b0 = local_60[0];
    local_a0 = local_60[1];
    local_ac = local_50;
    local_9c = local_4c;
    local_a8 = local_40;
    local_98 = local_3c;
    local_90 = local_60[2];
    local_80 = local_30;
    local_8c = local_48;
    local_7c = local_2c;
    local_88 = local_38;
    local_78 = local_28;
  }
  else {
    iVar2 = *(int *)(param_1 + 0x94);
    local_b0 = *(float *)(iVar2 + 0x60);
    local_ac = *(undefined4 *)(iVar2 + 0x70);
    local_a8 = *(undefined4 *)(iVar2 + 0x80);
    local_a0 = *(undefined4 *)(iVar2 + 100);
    local_9c = *(undefined4 *)(iVar2 + 0x74);
    local_98 = *(undefined4 *)(iVar2 + 0x84);
    local_90 = *(undefined4 *)(iVar2 + 0x68);
    local_8c = *(undefined4 *)(iVar2 + 0x78);
    local_88 = *(undefined4 *)(iVar2 + 0x88);
    local_80 = *(float *)(iVar2 + 0x90);
    local_7c = *(undefined4 *)(iVar2 + 0x94);
    local_78 = *(undefined4 *)(iVar2 + 0x98);
  }
  local_84 = 0;
  local_94 = 0;
  local_a4 = 0;
  local_74 = 0x3f800000;
  FUN_100b9ec0((void *)((int)local_64 + 0x30),&local_b0,&local_20);
  if ((*(uint *)(param_2 + 0x44) & 0x800) == 0) {
    pfVar3 = (float *)(*(int *)(param_2 + 0x94) + 0x60);
    pfVar4 = local_60;
    for (iVar2 = 0x10; iVar2 != 0; iVar2 = iVar2 + -1) {
      *pfVar4 = *pfVar3;
      pfVar3 = pfVar3 + 1;
      pfVar4 = pfVar4 + 1;
    }
    FUN_10039500(local_60,(float *)(param_2 + 0x30),&local_30);
    local_b0 = local_60[0];
    local_a0 = local_60[1];
    local_ac = local_50;
    local_9c = local_4c;
    local_a8 = local_40;
    local_98 = local_3c;
    local_90 = local_60[2];
    local_80 = local_30;
    local_8c = local_48;
    local_7c = local_2c;
    local_88 = local_38;
  }
  else {
    iVar2 = *(int *)(param_2 + 0x94);
    local_b0 = *(float *)(iVar2 + 0x60);
    local_ac = *(undefined4 *)(iVar2 + 0x70);
    local_a8 = *(undefined4 *)(iVar2 + 0x80);
    local_a0 = *(undefined4 *)(iVar2 + 100);
    local_9c = *(undefined4 *)(iVar2 + 0x74);
    local_98 = *(undefined4 *)(iVar2 + 0x84);
    local_90 = *(undefined4 *)(iVar2 + 0x68);
    local_8c = *(undefined4 *)(iVar2 + 0x78);
    local_88 = *(undefined4 *)(iVar2 + 0x88);
    local_80 = *(float *)(iVar2 + 0x90);
    local_7c = *(undefined4 *)(iVar2 + 0x94);
    local_28 = *(undefined4 *)(iVar2 + 0x98);
  }
  local_84 = 0;
  local_94 = 0;
  local_a4 = 0;
  local_74 = 0x3f800000;
  local_78 = local_28;
  FUN_100b9ec0((void *)((int)local_64 + 0x50),&local_b0,&local_20);
  return;
}


// 100a3e50 FUN_100a3e50

/* WARNING: Removing unreachable block (ram,0x100a3fe4) */
/* WARNING: Removing unreachable block (ram,0x100a3fa9) */
/* WARNING: Removing unreachable block (ram,0x100a3ff2) */
/* WARNING: Removing unreachable block (ram,0x100a3fa1) */

undefined4 __thiscall FUN_100a3e50(void *this,float *param_1,int param_2,int param_3)

{
  float fVar1;
  float fVar2;
  int iVar3;
  void *this_00;
  int iVar4;
  undefined1 *puVar5;
  char cVar6;
  undefined4 *puVar7;
  float *pfVar8;
  undefined4 *puVar9;
  float *pfVar10;
  float fVar11;
  float fVar12;
  float fVar13;
  float fVar14;
  float fVar15;
  float fVar16;
  float fVar17;
  int local_248;
  float local_244;
  int local_240;
  float local_23c [4];
  float local_22c;
  float local_228;
  float local_220;
  float local_21c;
  float local_218;
  float local_210 [4];
  undefined4 local_200;
  undefined4 local_1fc;
  undefined4 local_1f8;
  undefined4 local_1f0;
  undefined4 local_1ec;
  undefined4 local_1e8;
  float local_1e0;
  undefined4 local_1dc;
  undefined4 local_1d8;
  float local_1d0;
  float local_1cc;
  float local_1c8;
  float local_1c0;
  undefined4 local_1bc;
  undefined4 local_1b8;
  undefined4 local_1b4;
  undefined4 local_1b0;
  undefined4 local_1ac;
  undefined4 local_1a8;
  undefined4 local_1a4;
  undefined4 local_1a0;
  undefined4 local_19c;
  undefined4 local_198;
  undefined4 local_194;
  float local_190;
  undefined4 local_18c;
  undefined4 local_188;
  undefined4 local_184;
  float local_180;
  undefined4 local_17c;
  undefined4 local_178;
  undefined4 local_174;
  undefined4 local_170;
  undefined4 local_16c;
  undefined4 local_168;
  undefined4 local_164;
  undefined4 local_160;
  undefined4 local_15c;
  undefined4 local_158;
  undefined4 local_154;
  float local_150;
  undefined4 local_14c;
  undefined4 local_148;
  undefined4 local_144;
  undefined4 local_140 [12];
  float afStack_110 [4];
  float local_100;
  float local_fc;
  float local_f8;
  undefined4 local_f0 [12];
  float afStack_c0 [8];
  int aiStack_a0 [4];
  float afStack_90 [8];
  int aiStack_70 [4];
  float *local_60;
  undefined4 local_5c;
  undefined4 local_58;
  undefined4 local_54;
  float local_50 [4];
  int local_40 [3];
  int local_34 [3];
  int local_28;
  int local_24;
  float local_18;
  
  pfVar8 = param_1;
  pfVar10 = (float *)this;
  for (iVar4 = 0xc; iVar4 != 0; iVar4 = iVar4 + -1) {
    *pfVar10 = *pfVar8;
    pfVar8 = pfVar8 + 1;
    pfVar10 = pfVar10 + 1;
  }
  *(float *)((int)this + 0x30) = param_1[0xc];
  *(float *)((int)this + 0x34) = param_1[0xd];
  *(float *)((int)this + 0x38) = param_1[0xe];
  pfVar8 = param_1 + 0x10;
  pfVar10 = (float *)((int)this + 0x40);
  for (iVar4 = 0xc; iVar4 != 0; iVar4 = iVar4 + -1) {
    *pfVar10 = *pfVar8;
    pfVar8 = pfVar8 + 1;
    pfVar10 = pfVar10 + 1;
  }
  *(float *)((int)this + 0x70) = param_1[0x1c];
  *(float *)((int)this + 0x74) = param_1[0x1d];
  *(float *)((int)this + 0x78) = param_1[0x1e];
  *(undefined1 *)((int)this + 0xd3) = *(undefined1 *)(param_1 + 0x2a);
  local_23c[0] = param_1[0x23] - param_1[0x22];
  afStack_110[1] = (param_1[0x22] + param_1[0x23]) * 0.5;
  cVar6 = 1e-16 < local_23c[0];
  local_23c[1] = param_1[0x25] - param_1[0x24];
  afStack_110[2] = (param_1[0x24] + param_1[0x25]) * 0.5;
  if (1e-16 < local_23c[1]) {
    cVar6 = cVar6 + '\x01';
  }
  local_23c[2] = param_1[0x27] - param_1[0x26];
  afStack_110[3] = (param_1[0x26] + param_1[0x27]) * 0.5;
  if (1e-16 < local_23c[2]) {
    cVar6 = cVar6 + '\x01';
  }
  iVar4 = 0;
  puVar5 = (undefined1 *)((int)this + 0x80);
  do {
    *(char *)((int)this + iVar4 + 0xd0) = (char)iVar4;
    *puVar5 = 1;
    *(undefined4 *)(puVar5 + 4) = 0;
    *(undefined4 *)(puVar5 + 8) = 0;
    iVar4 = iVar4 + 1;
    puVar5 = puVar5 + 0x18;
  } while (iVar4 < 3);
  switch(cVar6) {
  case '\0':
    *(undefined1 *)((int)this + 0x98) = 1;
    *(undefined4 *)((int)this + 0x9c) = 0xbdcccccd;
    *(undefined4 *)((int)this + 0xa0) = 0x3dcccccd;
    return 1;
  case '\x01':
    break;
  case '\x02':
  case '\x03':
    local_244 = -1.0;
    local_240 = 0;
    if ((*(uint *)(param_2 + 0x44) & 0x800) == 0) {
      pfVar8 = (float *)(*(int *)(param_2 + 0x94) + 0x60);
      pfVar10 = local_210;
      for (iVar4 = 0x10; iVar4 != 0; iVar4 = iVar4 + -1) {
        *pfVar10 = *pfVar8;
        pfVar8 = pfVar8 + 1;
        pfVar10 = pfVar10 + 1;
      }
      FUN_10039500(local_210,(float *)(param_2 + 0x30),&local_1e0);
      local_1b0 = local_210[1];
      local_1ac = local_1fc;
      local_1a8 = local_1ec;
      local_1a0 = local_210[2];
      local_19c = local_1f8;
      local_1c0 = local_210[0];
      local_198 = local_1e8;
      local_1bc = local_200;
      local_190 = local_1e0;
      local_1b8 = local_1f0;
      local_18c = local_1dc;
      local_188 = local_1d8;
    }
    else {
      iVar4 = *(int *)(param_2 + 0x94);
      local_1c0 = *(float *)(iVar4 + 0x60);
      local_1bc = *(undefined4 *)(iVar4 + 0x70);
      local_1b8 = *(undefined4 *)(iVar4 + 0x80);
      local_1b0 = *(undefined4 *)(iVar4 + 100);
      local_1ac = *(undefined4 *)(iVar4 + 0x74);
      local_1a8 = *(undefined4 *)(iVar4 + 0x84);
      local_1a0 = *(undefined4 *)(iVar4 + 0x68);
      local_19c = *(undefined4 *)(iVar4 + 0x78);
      local_198 = *(undefined4 *)(iVar4 + 0x88);
      local_190 = *(float *)(iVar4 + 0x90);
      local_18c = *(undefined4 *)(iVar4 + 0x94);
      local_188 = *(undefined4 *)(iVar4 + 0x98);
    }
    local_194 = 0;
    local_1a4 = 0;
    local_1b4 = 0;
    local_184 = 0x3f800000;
    if ((*(uint *)(param_3 + 0x44) & 0x800) == 0) {
      pfVar8 = (float *)(*(int *)(param_3 + 0x94) + 0x60);
      pfVar10 = local_210;
      for (iVar4 = 0x10; iVar4 != 0; iVar4 = iVar4 + -1) {
        *pfVar10 = *pfVar8;
        pfVar8 = pfVar8 + 1;
        pfVar10 = pfVar10 + 1;
      }
      FUN_10039500(local_210,(float *)(param_3 + 0x30),&local_1e0);
      local_180 = local_210[0];
      local_17c = local_200;
      local_178 = local_1f0;
      local_170 = local_210[1];
      local_16c = local_1fc;
      local_168 = local_1ec;
      local_150 = local_1e0;
      local_160 = local_210[2];
      local_14c = local_1dc;
      local_15c = local_1f8;
      local_148 = local_1d8;
      local_158 = local_1e8;
    }
    else {
      iVar4 = *(int *)(param_3 + 0x94);
      local_180 = *(float *)(iVar4 + 0x60);
      local_17c = *(undefined4 *)(iVar4 + 0x70);
      local_178 = *(undefined4 *)(iVar4 + 0x80);
      local_170 = *(undefined4 *)(iVar4 + 100);
      local_16c = *(undefined4 *)(iVar4 + 0x74);
      local_168 = *(undefined4 *)(iVar4 + 0x84);
      local_160 = *(undefined4 *)(iVar4 + 0x68);
      local_15c = *(undefined4 *)(iVar4 + 0x78);
      local_158 = *(undefined4 *)(iVar4 + 0x88);
      local_150 = *(float *)(iVar4 + 0x90);
      local_14c = *(undefined4 *)(iVar4 + 0x94);
      local_148 = *(undefined4 *)(iVar4 + 0x98);
    }
    local_144 = 0x3f800000;
    local_154 = 0;
    local_164 = 0;
    local_174 = 0;
    FUN_100b9d90(&local_220,&local_1c0,(float *)((int)this + 0x30));
    FUN_100b9d90(local_23c + 3,&local_180,(float *)((int)this + 0x70));
    iVar4 = *(int *)(param_2 + 0x94);
    local_220 = local_220 - *(float *)(iVar4 + 0x90);
    iVar3 = *(int *)(param_3 + 0x94);
    local_21c = local_21c - *(float *)(iVar4 + 0x94);
    local_218 = local_218 - *(float *)(iVar4 + 0x98);
    local_23c[3] = local_23c[3] - *(float *)(iVar3 + 0x90);
    local_22c = local_22c - *(float *)(iVar3 + 0x94);
    local_228 = local_228 - *(float *)(iVar3 + 0x98);
    local_248 = 0;
    pfVar8 = (float *)this;
    do {
      FUN_100b9e30(&local_1d0,&local_1c0,pfVar8);
      FUN_100b9e30(afStack_110 + 4,&local_180,pfVar8 + 0x10);
      local_60 = local_50;
      local_5c = 1;
      local_58 = 1;
      local_54 = 4;
      FUN_10096f70(local_f0);
      iVar4 = local_28;
      this_00 = *(void **)(param_2 + 0x94);
      *(undefined4 *)((int)afStack_c0 + local_28 + 8) = 0;
      *(undefined4 *)((int)afStack_c0 + iVar4 + 4) = 0;
      *(undefined4 *)((int)afStack_c0 + iVar4) = 0;
      *(float *)((int)afStack_c0 + iVar4 + 0x14) = local_1cc;
      *(float *)((int)afStack_c0 + iVar4 + 0x18) = local_1c8;
      *(float *)((int)afStack_c0 + iVar4 + 0x10) = local_1d0;
      fVar12 = *(float *)((int)afStack_c0 + iVar4 + 0x14);
      fVar13 = *(float *)((int)this_00 + 0x88);
      fVar14 = *(float *)((int)this_00 + 0x78);
      fVar15 = *(float *)((int)this_00 + 0x68);
      fVar17 = *(float *)((int)this_00 + 0x60);
      fVar1 = *(float *)((int)this_00 + 0x70);
      fVar2 = *(float *)((int)this_00 + 0x80);
      *(float *)((int)afStack_c0 + iVar4 + 0x14) =
           *(float *)((int)this_00 + 0x74) * fVar12 + *(float *)((int)this_00 + 0x84) * local_1c8 +
           *(float *)((int)this_00 + 100) * local_1d0;
      *(float *)((int)afStack_c0 + iVar4 + 0x18) =
           fVar14 * fVar12 + fVar13 * local_1c8 + fVar15 * local_1d0;
      *(float *)((int)afStack_c0 + iVar4 + 0x10) =
           fVar1 * fVar12 + fVar2 * local_1c8 + local_1d0 * fVar17;
      *(int *)((int)aiStack_a0 + iVar4) = local_24;
      iVar3 = local_28;
      iVar4 = *(int *)(param_3 + 0x94);
      puVar7 = (undefined4 *)((int)afStack_90 + local_28);
      *(undefined4 *)((int)afStack_90 + local_28 + 8) = 0;
      *(undefined4 *)((int)afStack_90 + iVar3 + 4) = 0;
      *puVar7 = 0;
      *(float *)((int)afStack_90 + iVar3 + 0x14) = local_1cc * -1.0;
      fVar16 = local_1d0 * -1.0;
      fVar11 = local_1c8 * -1.0;
      *(float *)((int)afStack_90 + iVar3 + 0x10) = fVar16;
      *(float *)((int)afStack_90 + iVar3 + 0x18) = fVar11;
      fVar12 = *(float *)((int)afStack_90 + iVar3 + 0x14);
      fVar13 = *(float *)(iVar4 + 0x78);
      fVar14 = *(float *)(iVar4 + 0x68);
      fVar15 = *(float *)(iVar4 + 0x88);
      fVar17 = *(float *)(iVar4 + 0x70);
      fVar1 = *(float *)(iVar4 + 0x80);
      fVar2 = *(float *)(iVar4 + 0x60);
      *(float *)((int)afStack_90 + iVar3 + 0x14) =
           *(float *)(iVar4 + 100) * fVar16 + *(float *)(iVar4 + 0x74) * fVar12 +
           *(float *)(iVar4 + 0x84) * fVar11;
      *(float *)((int)afStack_90 + iVar3 + 0x18) =
           fVar14 * fVar16 + fVar13 * fVar12 + fVar15 * fVar11;
      *(float *)((int)afStack_90 + iVar3 + 0x10) = fVar17 * fVar12 + fVar1 * fVar11 + fVar2 * fVar16
      ;
      *(int *)((int)aiStack_70 + iVar3) = local_24;
      local_28 = local_28 + 0x30;
      local_40[0] = local_24 + 1;
      local_34[0] = local_40[0];
      local_24 = local_40[0];
      FUN_100a4e60(this_00,local_40,(int *)&local_60,(int)local_f0);
      local_40[0] = local_24;
      local_34[0] = local_24;
      FUN_100a4e60(*(void **)(param_2 + 0x94),local_34,(int *)&local_60,(int)local_f0);
      fVar12 = local_1c8 * local_21c - local_1cc * local_218;
      fVar13 = local_218 * local_1d0 - local_1c8 * local_220;
      local_18 = local_1cc * local_220 - local_21c * local_1d0;
      fVar14 = local_f8 * local_22c - local_fc * local_228;
      fVar15 = local_100 * local_228 - local_f8 * local_23c[3];
      fVar17 = local_fc * local_23c[3] - local_100 * local_22c;
      fVar12 = (local_18 * local_18 + fVar13 * fVar13 + fVar12 * fVar12) *
               *(float *)(*(int *)(param_2 + 0x94) + 0x3c) + *local_60 +
               (fVar17 * fVar17 + fVar15 * fVar15 + fVar14 * fVar14) *
               *(float *)(*(int *)(param_3 + 0x94) + 0x3c);
      if (local_244 < fVar12) {
        local_240 = local_248;
        local_244 = fVar12;
      }
      local_248 = local_248 + 1;
      pfVar8 = pfVar8 + 4;
    } while (local_248 < 3);
    if (local_240 == 2) {
      local_248 = 0;
    }
    else {
      local_248 = local_240 + 1;
      if (local_240 == 0) {
        local_244 = 2.8026e-45;
        goto LAB_100a4b4d;
      }
    }
    local_244 = (float)(local_240 + -1);
LAB_100a4b4d:
    fVar12 = local_244;
    if (local_23c[(int)local_244] <= local_23c[local_248] &&
        local_23c[local_248] != local_23c[(int)local_244]) {
      local_244 = (float)local_248;
      local_248 = (int)fVar12;
    }
    fVar12 = afStack_110[(int)local_244 + 1];
    puVar7 = (undefined4 *)this;
    puVar9 = local_140;
    for (iVar4 = 0xc; iVar4 != 0; iVar4 = iVar4 + -1) {
      *puVar9 = *puVar7;
      puVar7 = puVar7 + 1;
      puVar9 = puVar9 + 1;
    }
    pfVar8 = (float *)((int)this + 0x40);
    pfVar10 = local_210;
    for (iVar4 = 0xc; iVar4 != 0; iVar4 = iVar4 + -1) {
      *pfVar10 = *pfVar8;
      pfVar8 = pfVar8 + 1;
      pfVar10 = pfVar10 + 1;
    }
    FUN_100bb090(local_140,(int)local_244,fVar12);
    *(undefined4 *)this = local_140[local_240 * 4];
    *(undefined4 *)((int)this + 4) = local_140[local_240 * 4 + 1];
    *(undefined4 *)((int)this + 8) = local_140[local_240 * 4 + 2];
    *(float *)((int)this + 0x40) = local_210[local_240 * 4];
    *(float *)((int)this + 0x44) = local_210[local_240 * 4 + 1];
    *(float *)((int)this + 0x48) = local_210[local_240 * 4 + 2];
    *(undefined4 *)((int)this + 0x10) = local_140[local_248 * 4];
    *(undefined4 *)((int)this + 0x14) = local_140[local_248 * 4 + 1];
    *(undefined4 *)((int)this + 0x18) = local_140[local_248 * 4 + 2];
    *(float *)((int)this + 0x50) = local_210[local_248 * 4];
    *(float *)((int)this + 0x54) = local_210[local_248 * 4 + 1];
    *(float *)((int)this + 0x58) = local_210[local_248 * 4 + 2];
    *(undefined4 *)((int)this + 0x20) = local_140[(int)local_244 * 4];
    *(undefined4 *)((int)this + 0x24) = local_140[(int)local_244 * 4 + 1];
    *(undefined4 *)((int)this + 0x28) = local_140[(int)local_244 * 4 + 2];
    *(float *)((int)this + 0x60) = local_210[(int)local_244 * 4];
    *(float *)((int)this + 100) = local_210[(int)local_244 * 4 + 1];
    *(float *)((int)this + 0x68) = local_210[(int)local_244 * 4 + 2];
    fVar12 = param_1[local_240 * 2 + 0x22];
    fVar13 = param_1[local_240 * 2 + 0x23];
    if ((0.0001 < ABS(-fVar13 + 3.1415927)) || (0.0001 < ABS((-0.0 - fVar12) - 3.1415927))) {
      *(undefined1 *)((int)this + 0x80) = 1;
    }
    else {
      *(undefined1 *)((int)this + 0x80) = 0;
    }
    *(float *)((int)this + 0x88) = -0.0 - fVar12;
    fVar12 = local_23c[(int)local_244];
    *(float *)((int)this + 0x84) = -fVar13;
    fVar13 = fVar12 * -0.5;
    fVar12 = fVar12 * 0.5;
    if ((0.0001 < ABS(fVar13 + 3.1415927)) || (0.0001 < ABS(fVar12 - 3.1415927))) {
      *(undefined1 *)((int)this + 0x98) = 1;
    }
    else {
      *(undefined1 *)((int)this + 0x98) = 0;
    }
    *(float *)((int)this + 0x9c) = fVar13;
    *(float *)((int)this + 0xa0) = fVar12;
    fVar13 = param_1[local_248 * 2 + 0x22];
    fVar12 = param_1[local_248 * 2 + 0x23];
    if ((0.0001 < ABS(fVar13 + 3.1415927)) || (0.0001 < ABS(fVar12 - 3.1415927))) {
      *(undefined1 *)((int)this + 0xb0) = 1;
    }
    else {
      *(undefined1 *)((int)this + 0xb0) = 0;
    }
    *(float *)((int)this + 0xb4) = fVar13;
    *(float *)((int)this + 0xb8) = fVar12;
    *(char *)((int)this + 0xd0) = (char)local_240;
    *(char *)((int)this + 0xd2) = (char)local_248;
    *(undefined1 *)((int)this + 0xd1) = local_244._0_1_;
    return 0;
  default:
    return 1;
  }
  local_244 = -NAN;
  if (1e-16 < local_23c[0]) {
    local_244 = 0.0;
  }
  if (1e-16 < local_23c[1]) {
    local_244 = 1.4013e-45;
  }
  if (1e-16 < local_23c[2]) {
    local_244 = 2.8026e-45;
  }
  fVar12 = afStack_110[(int)local_244 + 1];
  puVar7 = (undefined4 *)this;
  puVar9 = local_140;
  for (iVar4 = 0xc; iVar4 != 0; iVar4 = iVar4 + -1) {
    *puVar9 = *puVar7;
    puVar7 = puVar7 + 1;
    puVar9 = puVar9 + 1;
  }
  pfVar8 = (float *)((int)this + 0x40);
  pfVar10 = local_210;
  for (iVar4 = 0xc; iVar4 != 0; iVar4 = iVar4 + -1) {
    *pfVar10 = *pfVar8;
    pfVar8 = pfVar8 + 1;
    pfVar10 = pfVar10 + 1;
  }
  FUN_100bb090(local_140,(int)local_244,fVar12);
  if (local_244 == 2.8026e-45) {
    local_248 = 0;
  }
  else {
    local_248 = (int)local_244 + 1;
    iVar4 = 2;
    if (local_244 == 0.0) goto LAB_100a40bf;
  }
  iVar4 = (int)local_244 + -1;
LAB_100a40bf:
  fVar12 = local_23c[(int)local_244];
  *(undefined4 *)this = local_140[local_248 * 4];
  *(undefined4 *)((int)this + 4) = local_140[local_248 * 4 + 1];
  *(undefined4 *)((int)this + 8) = local_140[local_248 * 4 + 2];
  *(float *)((int)this + 0x40) = local_210[local_248 * 4];
  *(float *)((int)this + 0x44) = local_210[local_248 * 4 + 1];
  *(float *)((int)this + 0x48) = local_210[local_248 * 4 + 2];
  *(undefined4 *)((int)this + 0x10) = local_140[iVar4 * 4];
  *(undefined4 *)((int)this + 0x14) = local_140[iVar4 * 4 + 1];
  *(undefined4 *)((int)this + 0x18) = local_140[iVar4 * 4 + 2];
  *(float *)((int)this + 0x50) = local_210[iVar4 * 4];
  *(float *)((int)this + 0x54) = local_210[iVar4 * 4 + 1];
  *(float *)((int)this + 0x58) = local_210[iVar4 * 4 + 2];
  *(undefined4 *)((int)this + 0x20) = local_140[(int)local_244 * 4];
  *(undefined4 *)((int)this + 0x24) = local_140[(int)local_244 * 4 + 1];
  *(undefined4 *)((int)this + 0x28) = local_140[(int)local_244 * 4 + 2];
  *(float *)((int)this + 0x60) = local_210[(int)local_244 * 4];
  *(float *)((int)this + 100) = local_210[(int)local_244 * 4 + 1];
  *(float *)((int)this + 0x68) = local_210[(int)local_244 * 4 + 2];
  if ((3.1415927 < fVar12) && (fVar12 < 6.2831855)) {
    local_23c[(int)local_244] = 3.1415927;
  }
  fVar13 = local_23c[(int)local_244] * -0.5;
  fVar12 = local_23c[(int)local_244] * 0.5;
  if ((0.0001 < ABS(fVar13 + 3.1415927)) || (0.0001 < ABS(fVar12 - 3.1415927))) {
    *(undefined1 *)((int)this + 0x98) = 1;
  }
  else {
    *(undefined1 *)((int)this + 0x98) = 0;
  }
  *(float *)((int)this + 0x9c) = fVar13;
  *(float *)((int)this + 0xa0) = fVar12;
  *(undefined1 *)((int)this + 0xd0) = (undefined1)local_248;
  *(char *)((int)this + 0xd2) = (char)iVar4;
  *(char *)((int)this + 0xd1) = SUB41(local_244,0);
  return 0;
}


// 100a4de0 FUN_100a4de0

void __thiscall FUN_100a4de0(void *this,undefined4 param_1,undefined4 param_2,undefined4 param_3)

{
  *(undefined4 *)((int)this + 4) = param_2;
  *(undefined ***)this = &PTR_LAB_100ce580;
  *(undefined4 *)((int)this + 8) = param_3;
  *(undefined4 *)((int)this + 0xc) = param_1;
  return;
}


// 100a4e00 FUN_100a4e00

undefined4 * __thiscall
FUN_100a4e00(void *this,void *param_1,undefined4 param_2,undefined4 param_3,undefined4 param_4,
            int param_5)

{
  FUN_100a4de0(this,0,param_2,param_3);
  *(undefined ***)this = &PTR_LAB_100ce584;
  *(void **)((int)this + 0x10) = param_1;
  FUN_100957f0(param_1,(int)this,param_5);
  return (undefined4 *)this;
}


// 100a4e40 FUN_100a4e40

void __fastcall FUN_100a4e40(undefined4 *param_1)

{
  *param_1 = &PTR_LAB_100ce584;
  if ((void *)param_1[4] != (void *)0x0) {
    FUN_10096050((void *)param_1[4],(int)param_1);
  }
  *param_1 = &PTR_LAB_100ce580;
  return;
}


// 100a4e60 FUN_100a4e60

void __thiscall FUN_100a4e60(void *this,int *param_1,int *param_2,int param_3)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  float fVar10;
  float fVar11;
  float fVar12;
  int *piVar13;
  float *pfVar14;
  int *piVar15;
  float *pfVar16;
  int iVar17;
  float *pfVar18;
  float *pfVar19;
  float fVar20;
  float fVar21;
  float *local_18;
  float *local_10;
  int *local_c;
  uint local_8;
  
  piVar13 = param_2;
                    /* WARNING: Load size is inaccurate */
  if ((((byte)*this >> 3 | (byte)*this) & 2) == 0) {
    pfVar16 = (float *)param_1[1];
    local_18 = (float *)param_1[2];
    pfVar19 = local_18 + 1;
    param_2 = (int *)0xffffffff;
    pfVar18 = pfVar16 + 5;
    do {
      fVar1 = pfVar18[1];
      fVar20 = *pfVar18;
      fVar21 = *(float *)((int)this + 200);
      fVar2 = *(float *)((int)this + 0xc4);
      fVar3 = pfVar18[-1];
      fVar4 = pfVar18[-4];
      fVar5 = *(float *)((int)this + 0xd4);
      fVar6 = *(float *)((int)this + 0x34);
      fVar7 = *(float *)((int)this + 0xc0);
      fVar8 = *(float *)((int)this + 0x30);
      fVar9 = pfVar18[-3];
      fVar10 = *(float *)((int)this + 0xd8);
      fVar11 = *pfVar16;
      fVar12 = *(float *)((int)this + 0xd0);
      pfVar19[1] = fVar1 * *(float *)((int)this + 0x38);
      *local_18 = fVar3 * fVar8;
      *pfVar19 = fVar20 * fVar6;
      fVar6 = pfVar18[3];
      *(float *)(param_3 + (int)fVar6 * 4) =
           *(float *)(param_3 + (int)fVar6 * 4) + fVar9 * fVar10 + fVar4 * fVar5 + fVar11 * fVar12 +
           fVar21 * fVar1 + fVar2 * fVar20 + fVar7 * fVar3;
      local_10 = (float *)param_1[1];
      piVar15 = (int *)((int)param_2 + 1);
      local_c = param_2;
      if (3 < (int)piVar15) {
        local_8 = (uint)piVar15 >> 2;
        pfVar14 = local_10 + 5;
        local_c = param_2 + -local_8;
        do {
          fVar21 = pfVar14[-1] * *local_18 + pfVar14[1] * pfVar19[1] + *pfVar19 * *pfVar14;
          fVar20 = (pfVar14[-4] * pfVar18[-4] + pfVar14[-3] * pfVar18[-3] + *pfVar16 * *local_10) *
                   *(float *)((int)this + 0x3c);
          fVar1 = pfVar14[3];
          iVar17 = piVar13[3] * (int)fVar1 + (int)fVar6;
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
          iVar17 = piVar13[3] * (int)fVar6 + (int)fVar1;
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
          fVar21 = pfVar14[0xc] * *pfVar19 + pfVar14[0xb] * *local_18 + pfVar14[0xd] * pfVar19[1];
          fVar20 = (pfVar14[7] * *pfVar16 + pfVar14[9] * pfVar18[-3] + pfVar14[8] * pfVar18[-4]) *
                   *(float *)((int)this + 0x3c);
          fVar1 = pfVar14[0xf];
          iVar17 = piVar13[3] * (int)fVar1 + (int)fVar6;
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
          iVar17 = piVar13[3] * (int)fVar6 + (int)fVar1;
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
          fVar21 = pfVar14[0x18] * *pfVar19 + pfVar14[0x17] * *local_18 + pfVar14[0x19] * pfVar19[1]
          ;
          fVar20 = (pfVar14[0x13] * *pfVar16 + pfVar14[0x15] * pfVar18[-3] +
                   pfVar14[0x14] * pfVar18[-4]) * *(float *)((int)this + 0x3c);
          fVar1 = pfVar14[0x1b];
          iVar17 = piVar13[3] * (int)fVar1 + (int)fVar6;
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
          iVar17 = piVar13[3] * (int)fVar6 + (int)fVar1;
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
          fVar21 = pfVar14[0x24] * *pfVar19 + pfVar14[0x23] * *local_18 + pfVar14[0x25] * pfVar19[1]
          ;
          fVar20 = (pfVar14[0x1f] * *pfVar16 + pfVar14[0x21] * pfVar18[-3] +
                   pfVar14[0x20] * pfVar18[-4]) * *(float *)((int)this + 0x3c);
          fVar1 = pfVar14[0x27];
          iVar17 = piVar13[3] * (int)fVar1 + (int)fVar6;
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
          iVar17 = piVar13[3] * (int)fVar6 + (int)fVar1;
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
          local_10 = local_10 + 0x30;
          pfVar14 = pfVar14 + 0x30;
          local_8 = local_8 - 1;
        } while (local_8 != 0);
      }
      if (-1 < (int)local_c) {
        pfVar14 = local_10 + 5;
        do {
          fVar21 = pfVar14[-1] * *local_18 + pfVar14[1] * pfVar19[1] + *pfVar14 * *pfVar19;
          fVar20 = (pfVar14[-4] * pfVar18[-4] + pfVar14[-3] * pfVar18[-3] + *pfVar16 * *local_10) *
                   *(float *)((int)this + 0x3c);
          fVar1 = pfVar14[3];
          iVar17 = piVar13[3] * (int)fVar1 + (int)fVar6;
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
          iVar17 = piVar13[3] * (int)fVar6 + (int)fVar1;
          pfVar14 = pfVar14 + 0xc;
          local_10 = local_10 + 0xc;
          local_c = (int *)((int)local_c + -1);
          *(float *)(*piVar13 + iVar17 * 4) = *(float *)(*piVar13 + iVar17 * 4) + fVar20 + fVar21;
        } while (-1 < (int)local_c);
      }
      pfVar14 = (float *)(*piVar13 + (piVar13[3] + 1) * (int)fVar6 * 4);
      fVar1 = *pfVar16;
      pfVar16 = pfVar16 + 0xc;
      fVar20 = *local_18;
      local_18 = local_18 + 4;
      *pfVar14 = (fVar1 * fVar1 + pfVar18[-4] * pfVar18[-4] + pfVar18[-3] * pfVar18[-3]) *
                 *(float *)((int)this + 0x3c) +
                 pfVar18[1] * pfVar19[1] + pfVar18[-1] * fVar20 + *pfVar19 * *pfVar18 + *pfVar14;
      iVar17 = (int)param_2 + 2;
      pfVar18 = pfVar18 + 0xc;
      pfVar19 = pfVar19 + 4;
      param_2 = piVar15;
    } while (iVar17 < *param_1);
  }
  return;
}


// 100a53e0 FUN_100a53e0

void __thiscall FUN_100a53e0(void *this,int *param_1,int param_2)

{
  float *pfVar1;
  float *pfVar2;
  float *pfVar3;
  int iVar4;
  float fVar5;
  
                    /* WARNING: Load size is inaccurate */
  if ((*this & 0x12) == 0) {
    pfVar2 = (float *)param_1[1];
    pfVar3 = (float *)param_1[2];
    iVar4 = *param_1 + -1;
    do {
      fVar5 = *(float *)(param_2 + (int)pfVar2[8] * 4);
      *(float *)((int)this + 0xc0) = *pfVar3 * fVar5 + *(float *)((int)this + 0xc0);
      *(float *)((int)this + 0xc4) = pfVar3[1] * fVar5 + *(float *)((int)this + 0xc4);
      *(float *)((int)this + 200) = pfVar3[2] * fVar5 + *(float *)((int)this + 200);
      fVar5 = fVar5 * *(float *)((int)this + 0x3c);
      *(float *)((int)this + 0xd0) = *pfVar2 * fVar5 + *(float *)((int)this + 0xd0);
      *(float *)((int)this + 0xd4) = pfVar2[1] * fVar5 + *(float *)((int)this + 0xd4);
      pfVar1 = pfVar2 + 2;
      pfVar2 = pfVar2 + 0xc;
      pfVar3 = pfVar3 + 4;
      iVar4 = iVar4 + -1;
      *(float *)((int)this + 0xd8) = *pfVar1 * fVar5 + *(float *)((int)this + 0xd8);
    } while (-1 < iVar4);
  }
  return;
}


// 100a54c5 __cfltcvt_init

/* Library Function - Single Match
    __cfltcvt_init
   
   Library: Visual Studio 2005 Release */

void __cfltcvt_init(void)

{
  PTR_LAB_100dedd4 = __cfltcvt;
  PTR_LAB_100dedd8 = &LAB_100a9049;
  PTR_LAB_100deddc = &LAB_100a9007;
  PTR_LAB_100dede0 = &LAB_100a903b;
  PTR_LAB_100dede4 = &LAB_100a8fb1;
  PTR_LAB_100dede8 = __cfltcvt;
  PTR_LAB_100dedec = __cfltcvt_l;
  PTR_LAB_100dedf0 = __fassign_l;
  PTR_LAB_100dedf4 = __cropzeros_l;
  PTR_LAB_100dedf8 = __forcdecpt_l;
  return;
}


// 100a5525 __fpmath

/* Library Function - Single Match
    __fpmath
   
   Library: Visual Studio 2005 Release */

void __cdecl __fpmath(int param_1)

{
  __cfltcvt_init();
  DAT_100f51d0 = __ms_p5_mp_test_fdiv();
  if (param_1 != 0) {
    __setdefaultprecision();
  }
  return;
}


// 100a5570 _memset

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
  if ((((char)_Val == '\0') && (0xff < _Size)) && (DAT_100f5e08 != 0)) {
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


// 100a55ea FUN_100a55ea

int __cdecl FUN_100a55ea(int param_1)

{
  int *piVar1;
  int *piVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  
  piVar1 = (int *)FUN_100a9de8(DAT_100f5e14);
  piVar2 = (int *)FUN_100a9de8(DAT_100f5e10);
  if (piVar1 <= piVar2) {
    uVar4 = ((int)piVar2 - (int)piVar1) + 4;
    if (3 < uVar4) {
      uVar3 = FUN_10002e70(piVar1);
      if (uVar3 < uVar4) {
        uVar4 = 0x800;
        if (uVar3 < 0x800) {
          uVar4 = uVar3;
        }
        if ((uVar4 + uVar3 < uVar3) ||
           (iVar5 = thunk_FUN_10002ce0(piVar1,uVar4 + uVar3), iVar5 == 0)) {
          if (uVar3 + 0x10 < uVar3) {
            return 0;
          }
          iVar5 = thunk_FUN_10002ce0(piVar1,uVar3 + 0x10);
          if (iVar5 == 0) {
            return 0;
          }
        }
        piVar2 = (int *)(iVar5 + ((int)piVar2 - (int)piVar1 >> 2) * 4);
        DAT_100f5e14 = FUN_100a9d71(iVar5);
      }
      iVar5 = FUN_100a9d71(param_1);
      *piVar2 = iVar5;
      DAT_100f5e10 = FUN_100a9d71((int)(piVar2 + 1));
      return param_1;
    }
  }
  return 0;
}


// 100a56d2 __onexit

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __onexit
   
   Library: Visual Studio 2005 Release */

_onexit_t __cdecl __onexit(_onexit_t _Func)

{
  _onexit_t p_Var1;
  
  FUN_100a818c();
  p_Var1 = (_onexit_t)FUN_100a55ea((int)_Func);
  FUN_100a5708();
  return p_Var1;
}


// 100a5708 FUN_100a5708

void FUN_100a5708(void)

{
  FUN_100a8195();
  return;
}


// 100a570e _atexit

/* Library Function - Single Match
    _atexit
   
   Library: Visual Studio 2005 Release */

int __cdecl _atexit(_func_4879 *param_1)

{
  _onexit_t p_Var1;
  
  p_Var1 = __onexit((_onexit_t)param_1);
  return (p_Var1 != (_onexit_t)0x0) - 1;
}


// 100a5720 _strrchr

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


// 100a574d __vsnprintf_l

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
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
    iVar2 = FUN_100aa827(&local_24,(byte *)_Format,_Locale,(wchar_t *)_ArgList);
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
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  return iVar2;
}


// 100a57fe __vsnprintf

/* Library Function - Single Match
    __vsnprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl __vsnprintf(char *_Dest,size_t _Count,char *_Format,va_list _Args)

{
  int iVar1;
  
  iVar1 = __vsnprintf_l(_Dest,_Count,_Format,(_locale_t)0x0,_Args);
  return iVar1;
}


// 100a5820 _memcpy

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
          goto switchD_100a5a03_caseD_2;
        case 3:
          goto switchD_100a5a03_caseD_3;
        }
        goto switchD_100a5a03_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_100a5a03_caseD_0;
      case 1:
        goto switchD_100a5a03_caseD_1;
      case 2:
        goto switchD_100a5a03_caseD_2;
      case 3:
        goto switchD_100a5a03_caseD_3;
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
              goto switchD_100a5a03_caseD_2;
            case 3:
              goto switchD_100a5a03_caseD_3;
            }
            goto switchD_100a5a03_caseD_1;
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
              goto switchD_100a5a03_caseD_2;
            case 3:
              goto switchD_100a5a03_caseD_3;
            }
            goto switchD_100a5a03_caseD_1;
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
              goto switchD_100a5a03_caseD_2;
            case 3:
              goto switchD_100a5a03_caseD_3;
            }
            goto switchD_100a5a03_caseD_1;
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
switchD_100a5a03_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_100a5a03_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_100a5a03_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_100a5a03_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_100f5e08 != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_100a587c_caseD_2;
      case 3:
        goto switchD_100a587c_caseD_3;
      }
      goto switchD_100a587c_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_100a587c_caseD_0;
    case 1:
      goto switchD_100a587c_caseD_1;
    case 2:
      goto switchD_100a587c_caseD_2;
    case 3:
      goto switchD_100a587c_caseD_3;
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
            goto switchD_100a587c_caseD_2;
          case 3:
            goto switchD_100a587c_caseD_3;
          }
          goto switchD_100a587c_caseD_1;
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
            goto switchD_100a587c_caseD_2;
          case 3:
            goto switchD_100a587c_caseD_3;
          }
          goto switchD_100a587c_caseD_1;
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
            goto switchD_100a587c_caseD_2;
          case 3:
            goto switchD_100a587c_caseD_3;
          }
          goto switchD_100a587c_caseD_1;
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
switchD_100a587c_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_100a587c_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_100a587c_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_100a587c_caseD_0:
  return _Dst;
}


// 100a5b90 __alloca_probe_16

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


// 100a5ba6 __alloca_probe_8

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


// 100a5bc0 __alloca_probe

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


// 100a5beb _LocaleUpdate::_LocaleUpdate

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
    if ((*(undefined **)this != PTR_DAT_100df640) && ((p_Var2->_ownlocale & DAT_100df55c) == 0)) {
      ptVar3 = ___updatetlocinfo();
      *(pthreadlocinfo *)this = ptVar3;
    }
    if ((*(undefined **)(this + 4) != PTR_DAT_100df460) &&
       ((*(uint *)(*(int *)(this + 8) + 0x70) & DAT_100df55c) == 0)) {
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


// 100a5c6d __toupper_l

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
LAB_100a5cca:
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
      pDVar4 = FUN_100ab320();
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
    if (iVar3 == 0) goto LAB_100a5cca;
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


// 100a5d85 _toupper

/* Library Function - Single Match
    _toupper
   
   Library: Visual Studio 2005 Release */

int __cdecl _toupper(int _C)

{
  if (DAT_100f5588 == 0) {
    if (_C - 0x61U < 0x1a) {
      return _C + -0x20;
    }
  }
  else {
    _C = __toupper_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 100a5dac __isdigit_l

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


// 100a5dfb _isdigit

/* Library Function - Single Match
    _isdigit
   
   Library: Visual Studio 2005 Release */

int __cdecl _isdigit(int _C)

{
  int iVar1;
  
  if (DAT_100f5588 == 0) {
    return (byte)PTR_DAT_100df630[_C * 2] & 4;
  }
  iVar1 = __isdigit_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 100a5e24 __isxdigit_l

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


// 100a5ea3 __isspace_l

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


// 100a5ef2 _isspace

/* Library Function - Single Match
    _isspace
   
   Library: Visual Studio 2005 Release */

int __cdecl _isspace(int _C)

{
  int iVar1;
  
  if (DAT_100f5588 == 0) {
    return (byte)PTR_DAT_100df630[_C * 2] & 8;
  }
  iVar1 = __isspace_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 100a5f1b _atol

/* Library Function - Single Match
    _atol
   
   Library: Visual Studio 2005 Release */

long __cdecl _atol(char *_Str)

{
  long lVar1;
  
  lVar1 = _strtol(_Str,(char **)0x0,10);
  return lVar1;
}


// 100a5f31 __atof_l

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
    pDVar2 = FUN_100ab320();
    *pDVar2 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100a5fda _atof

/* Library Function - Single Match
    _atof
   
   Library: Visual Studio 2005 Release */

double __cdecl _atof(char *_String)

{
  double dVar1;
  
  dVar1 = __atof_l(_String,(_locale_t)0x0);
  return dVar1;
}


// 100a6030 FUN_100a6030

void FUN_100a6030(void)

{
  ushort in_FPUControlWord;
  float10 in_ST0;
  double dVar1;
  
  if ((DAT_100f5e04 != 0) && ((MXCSR & 0x1f80) == 0x1f80 && (in_FPUControlWord & 0x7f) == 0x7f)) {
    FUN_100acb70();
    return;
  }
  dVar1 = (double)in_ST0;
  FUN_100ad388(SUB84(dVar1,0),(uint)((ulonglong)dVar1 >> 0x20));
  FUN_100a6088(SUB84(dVar1,0),(uint)((ulonglong)dVar1 >> 0x20));
  return;
}


// 100a6088 FUN_100a6088

void __cdecl FUN_100a6088(int param_1,uint param_2)

{
  uint in_EAX;
  bool in_ZF;
  ushort in_FPUControlWord;
  float10 in_ST0;
  float10 extraout_ST0;
  undefined4 unaff_retaddr;
  
  if (in_ZF) {
    if (((in_EAX & 0xfffff) != 0) || (param_1 != 0)) {
      FUN_100ad32c();
    }
LAB_100a6117:
    if (DAT_100f51cc == 0) {
      __startOneArgErrorHandling(&DAT_100deab0,0xd,in_FPUControlWord,unaff_retaddr,param_1,param_2);
      return;
    }
  }
  else {
    if (in_FPUControlWord != 0x27f) {
      in_EAX = FUN_100ad315();
      in_ST0 = extraout_ST0;
    }
    if (in_EAX < 0x3ff00000) {
      fpatan(SQRT(((float10)1 - in_ST0) * ((float10)1 + in_ST0)),in_ST0);
    }
    else if ((0x3ff00000 < in_EAX) || ((param_2 & 0xfffff) != 0 || param_1 != 0)) goto LAB_100a6117;
    if (DAT_100f51cc == 0) {
      __math_exit(&DAT_100deab0,0xd,unaff_retaddr,param_1,param_2);
      return;
    }
  }
  return;
}


// 100a6180 FUN_100a6180

void FUN_100a6180(void)

{
  ushort in_FPUControlWord;
  float10 in_ST0;
  float10 in_ST1;
  
  if ((DAT_100f5e04 != 0) && ((MXCSR & 0x1f80) == 0x1f80 && (in_FPUControlWord & 0x7f) == 0x7f)) {
    FUN_100ad490();
    return;
  }
  FUN_100a61dd(SUB84((double)in_ST1,0),(uint)((ulonglong)(double)in_ST1 >> 0x20),
               SUB84((double)in_ST0,0),(uint)((ulonglong)(double)in_ST0 >> 0x20));
  return;
}


// 100a61dd FUN_100a61dd
// diperbaiki: mode=decompile dengan parameter stack manual

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined1  [10] __cdecl FUN_100a61dd(byte stack_args [20])

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
    auStack_e._6_4_ = 0x100a61f1;
    in_EAX = FUN_100ad315();
    uVar1 = extraout_ECX;
  }
  bVar3 = (uVar1 & 0x7ff00000) == 0x7ff00000;
  if (bVar3) {
    if ((in_EAX & 0xfffff) == 0 && stack_args._8_4_ == 0) {
      auStack_e._6_4_ = 0x100a62ba;
      __fload_withFB(0,(int)stack_args);
      iVar2 = extraout_ECX_01;
      fVar5 = extraout_ST0_01;
      fVar6 = extraout_ST1_01;
      goto LAB_100a62ba;
    }
    auStack_e._6_4_ = 0x100a627d;
    __fload_withFB(0,(int)stack_args);
    fVar5 = extraout_ST0_00;
    fVar6 = extraout_ST1_00;
    if ((stack_args[0xe] & 8) == 0) {
      iVar2 = extraout_ECX_00 + 1;
      goto LAB_100a62ba;
    }
LAB_100a6287:
    auVar4 = (undefined1  [10])(fVar5 + fVar6);
  }
  else {
    auStack_e._6_4_ = 0x100a620c;
    uVar1 = __fload_withFB(uVar1 & 0x7ff00000,(int)stack_args);
    if (!bVar3) {
      if (((uVar1 & 0x7ff00000) != 0) ||
         ((CONCAT12(stack_args[6],stack_args._4_2_) & 0xfffff) != 0 || stack_args._0_4_ != 0)) {
        if ((stack_args[7] & 0x80) != 0) {
          auStack_e._6_4_ = 0x100a6384;
          FUN_100a63a2();
          auVar4 = _DAT_100df650;
          if (extraout_CL_01 == '\0') goto LAB_100a6255;
        }
        auStack_e._6_4_ = 0x100a6231;
        auVar4 = FUN_100ad300();
        if (extraout_CL == '\x01') {
          auVar4 = (undefined1  [10])-(float10)auVar4;
        }
        if (DAT_100f51cc != 0) {
          return auVar4;
        }
        auVar4 = (undefined1  [10])
                 FUN_100ad3e9(&DAT_100deac8,0x1d,unaff_retaddr,stack_args._0_4_,
                              CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)),
                              stack_args._8_4_,
                              CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)))
        ;
        return auVar4;
      }
      if ((CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)) & 0x7fffffff) == 0
          && stack_args._8_4_ == 0) {
        auVar4 = (undefined1  [10])FUN_100ad39e();
        return auVar4;
      }
      auStack_e._6_4_ = 0x100a633c;
      FUN_100a63a2();
      if ((stack_args._15_4_ & 0x80) == 0) {
        if ((stack_args[7] >> 7 & extraout_CL_00) == 0) {
          return (undefined1  [10])(float10)0;
        }
        auVar4 = (undefined1  [10])FUN_100ad39e();
        return auVar4;
      }
      auVar4 = _DAT_100df680;
      if ((stack_args[7] >> 7 & extraout_CL_00) != 0) {
        auVar4 = (undefined1  [10])-(float10)_DAT_100df680;
      }
      goto LAB_100a6255;
    }
    iVar2 = 0;
    fVar5 = extraout_ST0;
    fVar6 = extraout_ST1;
LAB_100a62ba:
    if (((CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)) & 0x7ff00000) ==
         0x7ff00000) &&
       ((CONCAT12(stack_args[6],stack_args._4_2_) & 0xfffff) != 0 || stack_args._0_4_ != 0)) {
      if ((stack_args[6] & 8) != 0) goto LAB_100a6287;
    }
    else if (iVar2 == 0) {
      pdVar7 = local_78;
      uStack_64 = 0x100a62e8;
      auStack_e = (undefined1  [10])in_ST7;
      iVar2 = FUN_100ae25b(SUB84((double)fVar5,0),(int)((ulonglong)(double)fVar5 >> 0x20),
                           SUB84((double)fVar6,0),(int)((ulonglong)(double)fVar6 >> 0x20),local_78);
      auVar4 = (undefined1  [10])(float10)*pdVar7;
      if (iVar2 == 0) {
        return (undefined1  [10])(float10)*pdVar7;
      }
      goto LAB_100a6255;
    }
    auVar4 = (undefined1  [10])(fVar5 + fVar6);
  }
LAB_100a6255:
  if (DAT_100f51cc != 0) {
    return auVar4;
  }
  auStack_e._6_4_ = 0x100a6272;
  auVar4 = (undefined1  [10])
           FUN_100ad2a0(&DAT_100deac8,0x1d,in_FPUControlWord,unaff_retaddr,stack_args._0_4_,
                        CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)),
                        stack_args._8_4_,
                        CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)));
  return auVar4;
}
// 100a63a2 FUN_100a63a2

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_100a63a2(void)

{
  float10 in_ST0;
  
  if (ROUND(in_ST0) == in_ST0) {
    return;
  }
  return;
}


// 100a63ca vscan_fn

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
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    uVar2 = 0xffffffff;
  }
  else {
    uVar2 = (*(code *)param_1)(&stack0xffffffdc,param_2,param_3,param_4);
  }
  return uVar2;
}


// 100a6432 FID_conflict:_sscanf

/* Library Function - Multiple Matches With Different Base Names
    _sscanf
    _sscanf_s
   
   Library: Visual Studio 2005 Release */

int __cdecl FID_conflict__sscanf(char *_Src,char *_Format,...)

{
  int iVar1;
  
  iVar1 = vscan_fn(&DAT_100ae428,(int)_Format,0,&stack0x0000000c);
  return iVar1;
}


// 100a6451 __tolower_l

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
LAB_100a64b0:
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
      pDVar4 = FUN_100ab320();
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
    if (iVar3 == 0) goto LAB_100a64b0;
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


// 100a6568 _tolower

/* Library Function - Single Match
    _tolower
   
   Library: Visual Studio 2005 Release */

int __cdecl _tolower(int _C)

{
  if (DAT_100f5588 == 0) {
    if (_C - 0x41U < 0x1a) {
      return _C + 0x20;
    }
  }
  else {
    _C = __tolower_l(_C,(_locale_t)0x0);
  }
  return _C;
}


// 100a658f __CRT_INIT@12

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
        _DAT_100f51fc = DVar2 * 0x100 + DVar3;
        DAT_100f51f4 = DVar1;
        _DAT_100f51f8 = uVar10;
        DAT_100f5200 = DVar2;
        _DAT_100f5204 = DVar3;
        bVar4 = FUN_10002e90();
        if (CONCAT31(extraout_var,bVar4) != 0) {
          iVar7 = __mtinit();
          if (iVar7 != 0) {
            __RTC_Initialize();
            DAT_100f6e44 = GetCommandLineA();
            DAT_100f51d8 = ___crtGetEnvironmentStringsA();
            iVar7 = __ioinit();
            if (-1 < iVar7) {
              iVar7 = __setargv();
              if (((-1 < iVar7) && (iVar7 = __setenvp(), -1 < iVar7)) &&
                 (iVar7 = __cinit(0), iVar7 == 0)) {
                DAT_100f51d4 = DAT_100f51d4 + 1;
                return 1;
              }
              __ioterm();
            }
            __mtterm();
          }
          FUN_10086430();
        }
      }
    }
  }
  else if (param_2 == 0) {
    if (0 < DAT_100f51d4) {
      DAT_100f51d4 = DAT_100f51d4 + -1;
      if (DAT_100f5230 == 0) {
        __cexit();
      }
      if (param_3 == 0) {
        __ioterm();
        __mtterm();
        FUN_10086430();
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
    FUN_100a9e5f();
    pDVar8 = (DWORD *)FUN_10002e20(1,0x214);
    if (pDVar8 != (DWORD *)0x0) {
      uVar12 = DAT_100deeb8;
      pDVar13 = pDVar8;
      pcVar9 = (code *)FUN_100a9de8(DAT_100f5554);
      iVar7 = (*pcVar9)(uVar12,pDVar13);
      if (iVar7 != 0) {
        FUN_100a9ece((int)pDVar8,0);
        DVar11 = GetCurrentThreadId();
        pDVar8[1] = 0xffffffff;
        *pDVar8 = DVar11;
        return 1;
      }
      FUN_10002ea0(pDVar8);
    }
  }
  return 0;
}


// 100a6768 ___DllMainCRTStartup

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x100a67fa) */
/* WARNING: Removing unreachable block (ram,0x100a67a7) */
/* WARNING: Removing unreachable block (ram,0x100a6827) */
/* Library Function - Single Match
    ___DllMainCRTStartup
   
   Library: Visual Studio 2005 Release */

int __fastcall ___DllMainCRTStartup(int param_1,int param_2,undefined4 param_3)

{
  int iVar1;
  int local_20;
  
  if (((param_2 == 0) && (DAT_100f51d4 == 0)) ||
     (((param_2 == 1 || (param_2 == 2)) &&
      (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)))) {
    local_20 = 0;
  }
  else {
    local_20 = FUN_100af924();
    if ((param_2 == 1) && (local_20 == 0)) {
      FUN_100af924();
      __CRT_INIT_12(param_3,0,param_1);
    }
    if (((param_2 == 0) || (param_2 == 3)) &&
       (iVar1 = __CRT_INIT_12(param_3,param_2,param_1), iVar1 == 0)) {
      local_20 = 0;
    }
  }
  return local_20;
}


// 100a685e entry

void entry(undefined4 param_1,int param_2,int param_3)

{
  if (param_2 == 1) {
    ___security_init_cookie();
  }
  ___DllMainCRTStartup(param_3,param_2,param_1);
  return;
}


// 100a6890 _strchr

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


// 100a69d6 __freea

/* Library Function - Single Match
    __freea
   
   Library: Visual Studio 2005 Release */

void __cdecl __freea(void *_Memory)

{
  if ((_Memory != (void *)0x0) && (*(int *)((int)_Memory + -8) == 0xdddd)) {
    FUN_10002ea0((int *)((int)_Memory + -8));
  }
  return;
}


// 100a6a00 _strncat

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
      if ((byte)uVar4 == 0) goto LAB_100a6a5d;
      uVar4 = (uint)puVar5 & 3;
    }
    do {
      do {
        puVar6 = puVar5;
        puVar5 = puVar6 + 1;
      } while (((*puVar6 ^ 0xffffffff ^ *puVar6 + 0x7efefeff) & 0x81010100) == 0);
      uVar4 = *puVar6;
      if ((char)uVar4 == '\0') goto LAB_100a6a6f;
      if ((char)(uVar4 >> 8) == '\0') {
        puVar6 = (uint *)((int)puVar6 + 1);
        goto LAB_100a6a6f;
      }
      if ((uVar4 & 0xff0000) == 0) {
        puVar6 = (uint *)((int)puVar6 + 2);
        goto LAB_100a6a6f;
      }
    } while ((uVar4 & 0xff000000) != 0);
LAB_100a6a5d:
    puVar6 = (uint *)((int)puVar5 + -1);
LAB_100a6a6f:
    if (((uint)_Source & 3) == 0) {
      uVar3 = _Count >> 2;
    }
    else {
      do {
        bVar1 = (byte)*(uint *)_Source;
        uVar4 = (uint)bVar1;
        _Source = (char *)((int)_Source + 1);
        if (bVar1 == 0) goto LAB_100a6aca;
        *(byte *)puVar6 = bVar1;
        puVar6 = (uint *)((int)puVar6 + 1);
        _Count = _Count - 1;
        if (_Count == 0) goto LAB_100a6ac0;
      } while (((uint)_Source & 3) != 0);
      uVar3 = _Count >> 2;
    }
    for (; uVar3 != 0; uVar3 = uVar3 - 1) {
      uVar2 = *(uint *)_Source;
      uVar4 = *(uint *)_Source;
      _Source = (char *)((int)_Source + 4);
      if (((uVar2 ^ 0xffffffff ^ uVar2 + 0x7efefeff) & 0x81010100) != 0) {
        if ((char)uVar4 == '\0') {
LAB_100a6aca:
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
LAB_100a6ac0:
    *(byte *)puVar6 = (byte)_Count;
  }
  return _Dest;
}


// 100a6b40 _memmove

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
          goto switchD_100a6d23_caseD_2;
        case 3:
          goto switchD_100a6d23_caseD_3;
        }
        goto switchD_100a6d23_caseD_1;
      }
    }
    else {
      switch(_Size) {
      case 0:
        goto switchD_100a6d23_caseD_0;
      case 1:
        goto switchD_100a6d23_caseD_1;
      case 2:
        goto switchD_100a6d23_caseD_2;
      case 3:
        goto switchD_100a6d23_caseD_3;
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
              goto switchD_100a6d23_caseD_2;
            case 3:
              goto switchD_100a6d23_caseD_3;
            }
            goto switchD_100a6d23_caseD_1;
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
              goto switchD_100a6d23_caseD_2;
            case 3:
              goto switchD_100a6d23_caseD_3;
            }
            goto switchD_100a6d23_caseD_1;
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
              goto switchD_100a6d23_caseD_2;
            case 3:
              goto switchD_100a6d23_caseD_3;
            }
            goto switchD_100a6d23_caseD_1;
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
switchD_100a6d23_caseD_1:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      return _Dst;
    case 2:
switchD_100a6d23_caseD_2:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      return _Dst;
    case 3:
switchD_100a6d23_caseD_3:
      *(undefined1 *)((int)puVar4 + 3) = *(undefined1 *)((int)puVar1 + 3);
      *(undefined1 *)((int)puVar4 + 2) = *(undefined1 *)((int)puVar1 + 2);
      *(undefined1 *)((int)puVar4 + 1) = *(undefined1 *)((int)puVar1 + 1);
      return _Dst;
    }
switchD_100a6d23_caseD_0:
    return _Dst;
  }
  if (((0xff < _Size) && (DAT_100f5e08 != 0)) && (((uint)_Dst & 0xf) == ((uint)_Src & 0xf))) {
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
        goto switchD_100a6b9c_caseD_2;
      case 3:
        goto switchD_100a6b9c_caseD_3;
      }
      goto switchD_100a6b9c_caseD_1;
    }
  }
  else {
    switch(_Size) {
    case 0:
      goto switchD_100a6b9c_caseD_0;
    case 1:
      goto switchD_100a6b9c_caseD_1;
    case 2:
      goto switchD_100a6b9c_caseD_2;
    case 3:
      goto switchD_100a6b9c_caseD_3;
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
            goto switchD_100a6b9c_caseD_2;
          case 3:
            goto switchD_100a6b9c_caseD_3;
          }
          goto switchD_100a6b9c_caseD_1;
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
            goto switchD_100a6b9c_caseD_2;
          case 3:
            goto switchD_100a6b9c_caseD_3;
          }
          goto switchD_100a6b9c_caseD_1;
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
            goto switchD_100a6b9c_caseD_2;
          case 3:
            goto switchD_100a6b9c_caseD_3;
          }
          goto switchD_100a6b9c_caseD_1;
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
switchD_100a6b9c_caseD_1:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    return _Dst;
  case 2:
switchD_100a6b9c_caseD_2:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    return _Dst;
  case 3:
switchD_100a6b9c_caseD_3:
                    /* WARNING: Load size is inaccurate */
    *(undefined1 *)puVar1 = *_Src;
    *(undefined1 *)((int)puVar1 + 1) = *(undefined1 *)((int)_Src + 1);
    *(undefined1 *)((int)puVar1 + 2) = *(undefined1 *)((int)_Src + 2);
    return _Dst;
  }
switchD_100a6b9c_caseD_0:
  return _Dst;
}


// 100a6eb0 _strncpy

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
        goto joined_r0x100a6efc;
      }
    }
    do {
      if (((uint)puVar5 & 3) == 0) {
        uVar4 = _Count >> 2;
        cVar3 = '\0';
        if (uVar4 == 0) goto LAB_100a6f43;
        goto LAB_100a6fb9;
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
joined_r0x100a6fb5:
          while( true ) {
            uVar4 = uVar4 - 1;
            puVar5 = puVar5 + 1;
            if (uVar4 == 0) break;
LAB_100a6fb9:
            *puVar5 = 0;
          }
          cVar3 = '\0';
          _Count = _Count & 3;
          if (_Count != 0) goto LAB_100a6f43;
          return _Dest;
        }
        if ((char)(uVar2 >> 8) == '\0') {
          *puVar5 = uVar2 & 0xff;
          goto joined_r0x100a6fb5;
        }
        if ((uVar2 & 0xff0000) == 0) {
          *puVar5 = uVar2 & 0xffff;
          goto joined_r0x100a6fb5;
        }
        if ((uVar2 & 0xff000000) == 0) {
          *puVar5 = uVar2;
          goto joined_r0x100a6fb5;
        }
      }
      *puVar5 = uVar2;
      puVar5 = puVar5 + 1;
      uVar4 = uVar4 - 1;
joined_r0x100a6efc:
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
LAB_100a6f43:
        *(char *)puVar5 = cVar3;
        puVar5 = (uint *)((int)puVar5 + 1);
      }
      return _Dest;
    }
    _Count = _Count - 1;
  } while (_Count != 0);
  return _Dest;
}


// 100a6fe0 FUN_100a6fe0

ulonglong __fastcall FUN_100a6fe0(undefined4 param_1,undefined4 param_2)

{
  ulonglong uVar1;
  uint uVar2;
  float fVar3;
  float10 in_ST0;
  uint local_20;
  float fStack_1c;
  
  if (DAT_100f5e08 == 0) {
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


// 100a7090 __allrem

/* Library Function - Single Match
    __allrem
   
   Library: Visual Studio */

undefined8 __allrem(uint param_1,uint param_2,uint param_3,uint param_4)

{
  ulonglong uVar1;
  longlong lVar2;
  uint uVar3;
  uint uVar4;
  int iVar5;
  int iVar6;
  uint uVar7;
  uint uVar8;
  uint uVar9;
  uint uVar10;
  uint uVar11;
  bool bVar12;
  bool bVar13;
  
  bVar13 = (int)param_2 < 0;
  if (bVar13) {
    bVar12 = param_1 != 0;
    param_1 = -param_1;
    param_2 = -(uint)bVar12 - param_2;
  }
  uVar11 = (uint)bVar13;
  if ((int)param_4 < 0) {
    bVar13 = param_3 != 0;
    param_3 = -param_3;
    param_4 = -(uint)bVar13 - param_4;
  }
  uVar3 = param_1;
  uVar4 = param_3;
  uVar8 = param_2;
  uVar9 = param_4;
  if (param_4 == 0) {
    iVar5 = (int)(((ulonglong)param_2 % (ulonglong)param_3 << 0x20 | (ulonglong)param_1) %
                 (ulonglong)param_3);
    iVar6 = 0;
    if ((int)(uVar11 - 1) < 0) goto LAB_100a713d;
  }
  else {
    do {
      uVar10 = uVar9 >> 1;
      uVar4 = uVar4 >> 1 | (uint)((uVar9 & 1) != 0) << 0x1f;
      uVar7 = uVar8 >> 1;
      uVar3 = uVar3 >> 1 | (uint)((uVar8 & 1) != 0) << 0x1f;
      uVar8 = uVar7;
      uVar9 = uVar10;
    } while (uVar10 != 0);
    uVar1 = CONCAT44(uVar7,uVar3) / (ulonglong)uVar4;
    uVar3 = (int)uVar1 * param_4;
    lVar2 = (uVar1 & 0xffffffff) * (ulonglong)param_3;
    uVar8 = (uint)((ulonglong)lVar2 >> 0x20);
    uVar4 = (uint)lVar2;
    uVar9 = uVar8 + uVar3;
    if (((CARRY4(uVar8,uVar3)) || (param_2 < uVar9)) || ((param_2 <= uVar9 && (param_1 < uVar4)))) {
      bVar13 = uVar4 < param_3;
      uVar4 = uVar4 - param_3;
      uVar9 = (uVar9 - param_4) - (uint)bVar13;
    }
    iVar5 = uVar4 - param_1;
    iVar6 = (uVar9 - param_2) - (uint)(uVar4 < param_1);
    if (-1 < (int)(uVar11 - 1)) goto LAB_100a713d;
  }
  bVar13 = iVar5 != 0;
  iVar5 = -iVar5;
  iVar6 = -(uint)bVar13 - iVar6;
LAB_100a713d:
  return CONCAT44(iVar6,iVar5);
}


// 100a7150 __alldiv

/* Library Function - Single Match
    __alldiv
   
   Library: Visual Studio 2005 Release */

undefined8 __alldiv(uint param_1,uint param_2,uint param_3,uint param_4)

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


// 100a71fa type_info::~type_info

/* Library Function - Single Match
    public: virtual __thiscall type_info::~type_info(void)
   
   Library: Visual Studio 2005 Release */

void __thiscall type_info::~type_info(type_info *this)

{
  *(undefined ***)this = &PTR__scalar_deleting_destructor__100ce8c4;
  _Type_info_dtor(this);
  return;
}


// 100a7208 type_info::`scalar_deleting_destructor'

/* Library Function - Single Match
    public: virtual void * __thiscall type_info::`scalar deleting destructor'(unsigned int)
   
   Library: Visual Studio 2005 Release */

void * __thiscall type_info::_scalar_deleting_destructor_(type_info *this,uint param_1)

{
  ~type_info(this);
  if ((param_1 & 1) != 0) {
    FUN_10002ea0(this);
  }
  return this;
}


// 100a7224 FUN_100a7224

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 __cdecl FUN_100a7224(byte *param_1,undefined4 *param_2,localeinfo_struct *param_3)

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
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_100a732a;
      }
    }
    else {
      local_c = _DAT_100df8d0;
      if (*_Str == 0x2d) {
        local_c = -_DAT_100df8d0;
      }
    }
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x22;
  }
  else {
    local_c = 0.0;
    if (param_2 != (undefined4 *)0x0) {
      *param_2 = param_1;
    }
  }
LAB_100a732a:
  if (local_10 != '\0') {
    *(uint *)(local_14 + 0x70) = *(uint *)(local_14 + 0x70) & 0xfffffffd;
  }
  return (float10)local_c;
}


// 100a733f FUN_100a733f

void __cdecl FUN_100a733f(byte *param_1,undefined4 *param_2)

{
  FUN_100a7224(param_1,param_2,(localeinfo_struct *)0x0);
  return;
}


// 100a7352 strtoxl

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
    pDVar3 = FUN_100ab320();
    *pDVar3 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
LAB_100a7409:
    bVar8 = *pbVar9;
    pbVar9 = pbVar2 + 2;
  }
  else if (bVar8 == 0x2b) goto LAB_100a7409;
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
      goto LAB_100a746f;
    }
    if ((*pbVar9 != 0x78) && (*pbVar9 != 0x58)) {
      param_4 = 8;
      goto LAB_100a746f;
    }
    param_4 = 0x10;
  }
  else if ((param_4 != 0x10) || (bVar8 != 0x30)) goto LAB_100a746f;
  if ((*pbVar9 == 0x78) || (*pbVar9 == 0x58)) {
    bVar8 = pbVar9[1];
    pbVar9 = pbVar9 + 2;
  }
LAB_100a746f:
  uVar4 = (uint)(0xffffffff / (ulonglong)(uint)param_4);
  do {
    uVar1 = *(ushort *)(ptVar5[1].lc_category[0].locale + (uint)bVar8 * 2);
    if ((uVar1 & 4) == 0) {
      if ((uVar1 & 0x103) == 0) {
LAB_100a74c9:
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
          pDVar3 = FUN_100ab320();
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
    if ((uint)param_4 <= uVar6) goto LAB_100a74c9;
    if ((local_8 < uVar4) ||
       ((local_8 == uVar4 && (uVar6 <= (uint)(0xffffffff % (ulonglong)(uint)param_4))))) {
      local_8 = local_8 * param_4 + uVar6;
      param_5 = param_5 | 8;
    }
    else {
      param_5 = param_5 | 0xc;
      if (param_3 == (char **)0x0) goto LAB_100a74c9;
    }
    bVar8 = *pbVar9;
    pbVar9 = pbVar9 + 1;
  } while( true );
}


// 100a757d _strtol

/* Library Function - Single Match
    _strtol
   
   Library: Visual Studio 2005 Release */

long __cdecl _strtol(char *_Str,char **_EndPtr,int _Radix)

{
  ulong uVar1;
  undefined **ppuVar2;
  
  if (DAT_100f5588 == 0) {
    ppuVar2 = &PTR_DAT_100df648;
  }
  else {
    ppuVar2 = (undefined **)0x0;
  }
  uVar1 = strtoxl((localeinfo_struct *)ppuVar2,_Str,_EndPtr,_Radix,0);
  return uVar1;
}


// 100a75a6 _sprintf

/* Library Function - Single Match
    _sprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl _sprintf(char *_Dest,char *_Format,...)

{
  DWORD *pDVar1;
  int iVar2;
  FILE local_24;
  
  if ((_Format == (char *)0x0) || (_Dest == (char *)0x0)) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  else {
    local_24._base = _Dest;
    local_24._ptr = _Dest;
    local_24._cnt = 0x7fffffff;
    local_24._flag = 0x42;
    iVar2 = FUN_100aa827(&local_24,(byte *)_Format,(localeinfo_struct *)0x0,
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


// 100a7670 FUN_100a7670

void FUN_100a7670(void)

{
  ushort in_FPUControlWord;
  float10 in_ST0;
  double dVar1;
  
  if ((DAT_100f5e04 != 0) && ((MXCSR & 0x1f80) == 0x1f80 && (in_FPUControlWord & 0x7f) == 0x7f)) {
    FUN_100afc80();
    return;
  }
  dVar1 = (double)in_ST0;
  FUN_100ad388(SUB84(dVar1,0),(uint)((ulonglong)dVar1 >> 0x20));
  FUN_100a76c8(SUB84(dVar1,0),(uint)((ulonglong)dVar1 >> 0x20));
  return;
}


// 100a76c8 FUN_100a76c8

void __cdecl FUN_100a76c8(int param_1,uint param_2)

{
  uint in_EAX;
  bool in_ZF;
  ushort in_FPUControlWord;
  float10 in_ST0;
  float10 extraout_ST0;
  undefined4 unaff_retaddr;
  
  if (in_ZF) {
    if (((in_EAX & 0xfffff) != 0) || (param_1 != 0)) {
      FUN_100ad32c();
    }
LAB_100a7757:
    if (DAT_100f51cc == 0) {
      __startOneArgErrorHandling(&DAT_100deaf0,0xe,in_FPUControlWord,unaff_retaddr,param_1,param_2);
      return;
    }
  }
  else {
    if (in_FPUControlWord != 0x27f) {
      in_EAX = FUN_100ad315();
      in_ST0 = extraout_ST0;
    }
    if (in_EAX < 0x3ff00000) {
      fpatan(in_ST0,SQRT(((float10)1 - in_ST0) * ((float10)1 + in_ST0)));
    }
    else if ((0x3ff00000 < in_EAX) || ((param_2 & 0xfffff) != 0 || param_1 != 0)) goto LAB_100a7757;
    if (DAT_100f51cc == 0) {
      __math_exit(&DAT_100deaf0,0xe,unaff_retaddr,param_1,param_2);
      return;
    }
  }
  return;
}


// 100a7780 FUN_100a7780

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 __cdecl FUN_100a7780(double param_1,undefined2 param_2)

{
  uint uVar1;
  uint uVar2;
  int iVar3;
  ushort in_FPUControlWord;
  float10 fVar4;
  double dVar5;
  ulonglong uVar6;
  undefined8 uVar7;
  
  if ((DAT_100f5e04 != 0) && ((MXCSR & 0x1f80) == 0x1f80 && (in_FPUControlWord & 0x7f) == 0x7f)) {
    uVar2 = (uint)((ulonglong)param_1 >> 0x20);
    uVar1 = uVar2 >> 0x14;
    uVar6 = (ulonglong)(0x433 - (uVar2 >> 0x14 & 0x7ff));
    if ((uVar1 & 0x800) == 0) {
      if (uVar1 < 0x3ff) {
        return (float10)0;
      }
      if (uVar1 < 0x433) {
        return (float10)(double)(((ulonglong)param_1 >> uVar6) << uVar6);
      }
    }
    else {
      dVar5 = (double)(((ulonglong)param_1 >> uVar6) << uVar6);
      if (uVar1 < 0xbff) {
        return (float10)(double)((-(ulonglong)(param_1 < -0.0) | (ulonglong)DAT_100ce900) &
                                0xbff0000000000000);
      }
      if (uVar1 < 0xc33) {
        return (float10)(dVar5 - (double)(-(ulonglong)(param_1 < dVar5) & 0x3ff0000000000000));
      }
    }
    if (NAN(param_1)) {
      ___libm_error_support(&param_1,&param_1,&param_1,0x3ed);
    }
    return (float10)(double)CONCAT26(param_1._6_2_,param_1._0_6_);
  }
  uVar2 = __ctrlfp();
  uVar1 = (uint)(CONCAT26(param_1._6_2_,param_1._0_6_) >> 0x20);
  if ((param_1._6_2_ & 0x7ff0) == 0x7ff0) {
    iVar3 = __sptype((int)param_1._0_6_,uVar1);
    if (0 < iVar3) {
      if (iVar3 < 3) {
        __ctrlfp();
        return (float10)(double)CONCAT26(param_1._6_2_,param_1._0_6_);
      }
      if (iVar3 == 3) {
        fVar4 = __handle_qnan1(0xb,(double)CONCAT44((int)(CONCAT26(param_1._6_2_,param_1._0_6_) >>
                                                         0x20),(int)param_1._0_6_));
        return fVar4;
      }
    }
    dVar5 = (double)CONCAT26(param_1._6_2_,param_1._0_6_) + 1.0;
    uVar7 = CONCAT26(param_1._6_2_,param_1._0_6_);
    uVar1 = 8;
  }
  else {
    fVar4 = __frnd((double)CONCAT44(uVar1,(int)param_1._0_6_));
    dVar5 = (double)fVar4;
    if (((double)CONCAT26(param_1._6_2_,param_1._0_6_) == dVar5) || ((uVar2 & 0x20) != 0)) {
      __ctrlfp();
      return (float10)dVar5;
    }
    uVar7 = CONCAT26(param_1._6_2_,param_1._0_6_);
    uVar1 = 0x10;
  }
  fVar4 = (float10)__except1(uVar1,0xb,uVar7,dVar5,uVar2);
                    /* WARNING: Read-only address (ram,0x100ce900) is written */
  return fVar4;
}


// 100a78e2 FID_conflict:_wprintf

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Multiple Matches With Different Base Names
    _printf
    _wprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl FID_conflict__wprintf(char *_Format,...)

{
  DWORD *pDVar1;
  int iVar2;
  undefined **ppuVar3;
  int _Flag;
  localeinfo_struct *plVar4;
  wchar_t *pwVar5;
  
  if (_Format == (char *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  else {
    ppuVar3 = FUN_100a7c8d();
    FUN_100a7da0(1,(int)(ppuVar3 + 8));
    ppuVar3 = FUN_100a7c8d();
    _Flag = __stbuf((FILE *)(ppuVar3 + 8));
    pwVar5 = (wchar_t *)&stack0x00000008;
    plVar4 = (localeinfo_struct *)0x0;
    ppuVar3 = FUN_100a7c8d();
    iVar2 = FUN_100aa827((FILE *)(ppuVar3 + 8),(byte *)_Format,plVar4,pwVar5);
    ppuVar3 = FUN_100a7c8d();
    __ftbuf(_Flag,(FILE *)(ppuVar3 + 8));
    FUN_100a797e();
  }
  return iVar2;
}


// 100a797e FUN_100a797e

void FUN_100a797e(void)

{
  undefined **ppuVar1;
  
  ppuVar1 = FUN_100a7c8d();
  FUN_100a7e04(1,(int)(ppuVar1 + 8));
  return;
}


// 100a7991 FUN_100a7991

bool FUN_100a7991(void)

{
  return DAT_100f51e8 == (DAT_100df7a0 | 1);
}


// 100a79a7 _fprintf

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _fprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl _fprintf(FILE *_File,char *_Format,...)

{
  DWORD *pDVar1;
  int iVar2;
  uint uVar3;
  undefined *puVar4;
  int local_20;
  
  local_20 = 0;
  if ((_File == (FILE *)0x0) || (_Format == (char *)0x0)) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  FUN_100a7d64((uint)_File);
  if ((_File->_flag & 0x40) == 0) {
    iVar2 = __fileno(_File);
    if ((iVar2 == -1) || (iVar2 = __fileno(_File), iVar2 == -2)) {
      puVar4 = &DAT_100df6e0;
    }
    else {
      iVar2 = __fileno(_File);
      uVar3 = __fileno(_File);
      puVar4 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_100f5d00)[iVar2 >> 5]);
    }
    if ((puVar4[0x24] & 0x7f) == 0) {
      iVar2 = __fileno(_File);
      if ((iVar2 == -1) || (iVar2 = __fileno(_File), iVar2 == -2)) {
        puVar4 = &DAT_100df6e0;
      }
      else {
        iVar2 = __fileno(_File);
        uVar3 = __fileno(_File);
        puVar4 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_100f5d00)[iVar2 >> 5]);
      }
      if ((puVar4[0x24] & 0x80) == 0) goto LAB_100a7aaf;
    }
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    local_20 = -1;
  }
LAB_100a7aaf:
  if (local_20 == 0) {
    iVar2 = __stbuf(_File);
    local_20 = FUN_100aa827(_File,(byte *)_Format,(localeinfo_struct *)0x0,
                            (wchar_t *)&stack0x0000000c);
    __ftbuf(iVar2,_File);
  }
  FUN_100a7aec();
  return local_20;
}


// 100a7aec FUN_100a7aec

void FUN_100a7aec(void)

{
  int unaff_EBP;
  
  FUN_100a7dce(*(uint *)(unaff_EBP + 8));
  return;
}


// 100a7af6 FUN_100a7af6

bool __cdecl FUN_100a7af6(undefined4 param_1,undefined4 param_2)

{
  return (param_2._2_2_ & 0x7ff0) != 0x7ff0;
}


// 100a7b0b __fpclass

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


// 100a7b9f FUN_100a7b9f

int __cdecl FUN_100a7b9f(FILE *param_1)

{
  DWORD *pDVar1;
  int iVar2;
  int iVar3;
  
  iVar3 = -1;
  if (param_1 == (FILE *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        FUN_10002ea0(param_1->_tmpfname);
        param_1->_tmpfname = (char *)0x0;
      }
    }
    param_1->_flag = 0;
  }
  return iVar3;
}


// 100a7c8d FUN_100a7c8d

undefined ** FUN_100a7c8d(void)

{
  return &PTR_DAT_100deb50;
}


// 100a7d64 FUN_100a7d64

void __cdecl FUN_100a7d64(uint param_1)

{
  if ((0x100deb4f < param_1) && (param_1 < 0x100dedb1)) {
    __lock(((int)(param_1 + 0xeff214b0) >> 5) + 0x10);
    *(uint *)(param_1 + 0xc) = *(uint *)(param_1 + 0xc) | 0x8000;
    return;
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(param_1 + 0x20));
  return;
}


// 100a7da0 FUN_100a7da0

void __cdecl FUN_100a7da0(int param_1,int param_2)

{
  if (param_1 < 0x14) {
    __lock(param_1 + 0x10);
    *(uint *)(param_2 + 0xc) = *(uint *)(param_2 + 0xc) | 0x8000;
    return;
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(param_2 + 0x20));
  return;
}


// 100a7dce FUN_100a7dce

void __cdecl FUN_100a7dce(uint param_1)

{
  if ((0x100deb4f < param_1) && (param_1 < 0x100dedb1)) {
    *(uint *)(param_1 + 0xc) = *(uint *)(param_1 + 0xc) & 0xffff7fff;
    FUN_100afa6b(((int)(param_1 + 0xeff214b0) >> 5) + 0x10);
    return;
  }
  LeaveCriticalSection((LPCRITICAL_SECTION)(param_1 + 0x20));
  return;
}


// 100a7e04 FUN_100a7e04

void __cdecl FUN_100a7e04(int param_1,int param_2)

{
  if (param_1 < 0x14) {
    *(uint *)(param_2 + 0xc) = *(uint *)(param_2 + 0xc) & 0xffff7fff;
    FUN_100afa6b(param_1 + 0x10);
    return;
  }
  LeaveCriticalSection((LPCRITICAL_SECTION)(param_2 + 0x20));
  return;
}


// 100a7e2e __time64

/* Library Function - Single Match
    __time64
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release */

__time64_t __cdecl __time64(__time64_t *_Time)

{
  __time64_t _Var1;
  _FILETIME local_c;
  
  GetSystemTimeAsFileTime(&local_c);
  _Var1 = __aulldiv(local_c.dwLowDateTime + 0x2ac18000,
                    local_c.dwHighDateTime + 0xfe624e21 + (uint)(0xd53e7fff < local_c.dwLowDateTime)
                    ,10000000,0);
  if (_Time != (__time64_t *)0x0) {
    *_Time = _Var1;
  }
  return _Var1;
}


// 100a7e6a ___ascii_stricmp

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


// 100a7e9f __stricmp_l

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
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    iVar2 = 0x7fffffff;
  }
  else if (_Str2 == (char *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100a7f72 __vsprintf_l

/* Library Function - Single Match
    __vsprintf_l
   
   Library: Visual Studio 2005 Release */

int __cdecl __vsprintf_l(char *_DstBuf,char *_Format,_locale_t param_3,va_list _ArgList)

{
  DWORD *pDVar1;
  int iVar2;
  FILE local_24;
  
  if ((_Format == (char *)0x0) || (_DstBuf == (char *)0x0)) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  else {
    local_24._base = _DstBuf;
    local_24._ptr = _DstBuf;
    local_24._cnt = 0x7fffffff;
    local_24._flag = 0x42;
    iVar2 = FUN_100aa827(&local_24,(byte *)_Format,param_3,(wchar_t *)_ArgList);
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


// 100a7fee _vsprintf

/* Library Function - Single Match
    _vsprintf
   
   Library: Visual Studio 2005 Release */

int __cdecl _vsprintf(char *_Dest,char *_Format,va_list _Args)

{
  int iVar1;
  
  iVar1 = __vsprintf_l(_Dest,_Format,(_locale_t)0x0,_Args);
  return iVar1;
}


// 100a8010 FUN_100a8010

float10 __cdecl FUN_100a8010(double param_1,undefined2 param_2)

{
  uint uVar1;
  uint uVar2;
  int iVar3;
  ushort in_FPUControlWord;
  float10 fVar4;
  double dVar5;
  ulonglong uVar6;
  undefined8 uVar7;
  
  if ((DAT_100f5e04 != 0) && ((MXCSR & 0x1f80) == 0x1f80 && (in_FPUControlWord & 0x7f) == 0x7f)) {
    uVar2 = (uint)((ulonglong)param_1 >> 0x20);
    uVar1 = uVar2 >> 0x14;
    uVar6 = (ulonglong)(0x433 - (uVar2 >> 0x14 & 0x7ff));
    if ((uVar1 & 0x800) == 0) {
      dVar5 = (double)(((ulonglong)param_1 >> uVar6) << uVar6);
      if (uVar1 < 0x3ff) {
        return (float10)(double)(-(ulonglong)(0.0 < param_1) & 0x3ff0000000000000);
      }
      if (uVar1 < 0x433) {
        return (float10)(dVar5 + (double)(-(ulonglong)(dVar5 < param_1) & 0x3ff0000000000000));
      }
    }
    else {
      if (uVar1 < 0xbff) {
        return (float10)-0.0;
      }
      if (uVar1 < 0xc33) {
        return (float10)(double)(((ulonglong)param_1 >> uVar6) << uVar6);
      }
    }
    if (NAN(param_1)) {
      ___libm_error_support(&param_1,&param_1,&param_1,0x3ec);
    }
    return (float10)(double)CONCAT26(param_1._6_2_,param_1._0_6_);
  }
  uVar2 = __ctrlfp();
  uVar1 = (uint)(CONCAT26(param_1._6_2_,param_1._0_6_) >> 0x20);
  if ((param_1._6_2_ & 0x7ff0) == 0x7ff0) {
    iVar3 = __sptype((int)param_1._0_6_,uVar1);
    if (0 < iVar3) {
      if (iVar3 < 3) {
        __ctrlfp();
        return (float10)(double)CONCAT26(param_1._6_2_,param_1._0_6_);
      }
      if (iVar3 == 3) {
        fVar4 = __handle_qnan1(0xc,(double)CONCAT44((int)(CONCAT26(param_1._6_2_,param_1._0_6_) >>
                                                         0x20),(int)param_1._0_6_));
        return fVar4;
      }
    }
    dVar5 = (double)CONCAT26(param_1._6_2_,param_1._0_6_) + 1.0;
    uVar7 = CONCAT26(param_1._6_2_,param_1._0_6_);
    uVar1 = 8;
  }
  else {
    fVar4 = __frnd((double)CONCAT44(uVar1,(int)param_1._0_6_));
    dVar5 = (double)fVar4;
    if (((double)CONCAT26(param_1._6_2_,param_1._0_6_) == dVar5) || ((uVar2 & 0x20) != 0)) {
      __ctrlfp();
      return (float10)dVar5;
    }
    uVar7 = CONCAT26(param_1._6_2_,param_1._0_6_);
    uVar1 = 0x10;
  }
  fVar4 = (float10)__except1(uVar1,0xc,uVar7,dVar5,uVar2);
  return fVar4;
}


// 100a812d __amsg_exit

/* Library Function - Single Match
    __amsg_exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __amsg_exit(int param_1)

{
  code *pcVar1;
  
  __FF_MSGBANNER();
  FUN_100a9b2c(param_1);
  pcVar1 = (code *)FUN_100a9de8((int)PTR___exit_100dedd0);
  (*pcVar1)(0xff);
  return;
}


// 100a8151 FUN_100a8151

void __cdecl FUN_100a8151(undefined4 param_1)

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


// 100a8177 ___crtExitProcess

/* Library Function - Single Match
    ___crtExitProcess
   
   Library: Visual Studio 2005 Release */

void __cdecl ___crtExitProcess(int param_1)

{
  FUN_100a8151(param_1);
                    /* WARNING: Subroutine does not return */
  ExitProcess(param_1);
}


// 100a818c FUN_100a818c

void FUN_100a818c(void)

{
  __lock(8);
  return;
}


// 100a8195 FUN_100a8195

void FUN_100a8195(void)

{
  FUN_100afa6b(8);
  return;
}


// 100a819e FUN_100a819e

void __cdecl FUN_100a819e(undefined4 *param_1)

{
  undefined4 *in_EAX;
  
  for (; in_EAX < param_1; in_EAX = in_EAX + 1) {
    if ((code *)*in_EAX != (code *)0x0) {
      (*(code *)*in_EAX)();
    }
  }
  return;
}


// 100a81b6 __initterm_e

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


// 100a81d6 __get_osplatform

/* Library Function - Single Match
    __get_osplatform
   
   Library: Visual Studio 2005 Release */

undefined4 __cdecl __get_osplatform(int *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (int *)0x0) && (DAT_100f51f4 != 0)) {
    *param_1 = DAT_100f51f4;
    return 0;
  }
  pDVar1 = FUN_100ab320();
  *pDVar1 = 0x16;
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 100a820d FUN_100a820d

undefined4 __cdecl FUN_100a820d(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if ((param_1 != (undefined4 *)0x0) && (DAT_100f51f4 != 0)) {
    *param_1 = DAT_100f5200;
    return 0;
  }
  pDVar1 = FUN_100ab320();
  *pDVar1 = 0x16;
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0x16;
}


// 100a8249 __cinit

/* Library Function - Single Match
    __cinit
   
   Library: Visual Studio 2005 Release */

int __cdecl __cinit(int param_1)

{
  BOOL BVar1;
  int iVar2;
  undefined4 *puVar3;
  
  BVar1 = __IsNonwritableInCurrentImage((PBYTE)&PTR___fpmath_100ce8a0);
  if (BVar1 != 0) {
    __fpmath(param_1);
  }
  __initp_misc_cfltcvt_tab();
  iVar2 = __initterm_e((undefined4 *)&DAT_100bc28c,(undefined4 *)&DAT_100bc2a4);
  if (iVar2 == 0) {
    _atexit((_func_4879 *)&LAB_100af786);
    puVar3 = &DAT_100bc18c;
    do {
      if ((code *)*puVar3 != (code *)0x0) {
        (*(code *)*puVar3)();
      }
      puVar3 = puVar3 + 1;
    } while (puVar3 < &DAT_100bc288);
    if ((DAT_100f5e1c != (code *)0x0) &&
       (BVar1 = __IsNonwritableInCurrentImage((PBYTE)&DAT_100f5e1c), BVar1 != 0)) {
      (*DAT_100f5e1c)(0,2,0);
    }
    iVar2 = 0;
  }
  return iVar2;
}


// 100a82db FUN_100a82db

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* WARNING: Removing unreachable block (ram,0x100a83ae) */

void __cdecl FUN_100a82db(int param_1,int param_2,int param_3)

{
  int iVar1;
  int *piVar2;
  int *piVar3;
  int iVar4;
  code *pcVar5;
  
  __lock(8);
  if (DAT_100f5234 != 1) {
    DAT_100f5230 = 1;
    DAT_100f522c = (undefined1)param_3;
    if (param_2 == 0) {
      piVar2 = (int *)FUN_100a9de8(DAT_100f5e14);
      piVar3 = (int *)FUN_100a9de8(DAT_100f5e10);
      if (piVar2 != (int *)0x0) {
        while (piVar3 = piVar3 + -1, piVar2 <= piVar3) {
          if (*piVar3 != 0) {
            iVar1 = *piVar3;
            iVar4 = __encoded_null();
            if (iVar1 != iVar4) {
              pcVar5 = (code *)FUN_100a9de8(iVar1);
              (*pcVar5)();
            }
          }
        }
      }
      FUN_100a819e((undefined4 *)&DAT_100bc2b4);
    }
    FUN_100a819e((undefined4 *)&DAT_100bc2bc);
  }
  FUN_100a83a8();
  if (param_3 != 0) {
    return;
  }
  DAT_100f5234 = 1;
  FUN_100afa6b(8);
  ___crtExitProcess(param_1);
  return;
}


// 100a83a8 FUN_100a83a8

void FUN_100a83a8(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + 0x10) != 0) {
    FUN_100afa6b(8);
  }
  return;
}


// 100a83bd FUN_100a83bd

void __cdecl FUN_100a83bd(int param_1)

{
  FUN_100a82db(param_1,0,0);
  return;
}


// 100a83ce __exit

/* Library Function - Single Match
    __exit
   
   Library: Visual Studio 2005 Release */

void __cdecl __exit(int _Code)

{
  FUN_100a82db(_Code,1,0);
  return;
}


// 100a83df __cexit

/* Library Function - Single Match
    __cexit
   
   Library: Visual Studio 2005 Release */

void __cdecl __cexit(void)

{
  FUN_100a82db(0,0,1);
  return;
}


// 100a83ee __init_pointers

/* Library Function - Single Match
    __init_pointers
   
   Library: Visual Studio 2005 Release */

void __cdecl __init_pointers(void)

{
  undefined4 uVar1;
  
  uVar1 = __encoded_null();
  FUN_100b2a38(uVar1);
  FUN_100b2959(uVar1);
  FUN_100ab1bb(uVar1);
  FUN_100aa32a(uVar1);
  FUN_100b294f(uVar1);
  __initp_misc_winsig(uVar1);
  FUN_100af7aa();
  __initp_eh_hooks();
  PTR___exit_100dedd0 = (undefined *)FUN_100a9d71(0x100a83ce);
  return;
}


// 100a843a __freopen_helper

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
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  if (*param_2 == '\0') {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
  }
  else {
    FUN_100a7d64((uint)param_4);
    if ((param_4->_flag & 0x83) != 0) {
      FUN_100a7b9f(param_4);
    }
    param_4->_base = (char *)0x0;
    param_4->_ptr = (char *)0x0;
    param_4->_flag = 0;
    param_4->_cnt = 0;
    puVar2 = FUN_100b2a42(param_2,param_3,param_5,&param_4->_ptr);
    *param_1 = (int)puVar2;
    FUN_100a8509();
    if (*param_1 != 0) {
      return 0;
    }
  }
  pDVar1 = FUN_100ab320();
  return *pDVar1;
}


// 100a8509 FUN_100a8509

void FUN_100a8509(void)

{
  int unaff_EBP;
  
  FUN_100a7dce(*(uint *)(unaff_EBP + -0x1c));
  return;
}


// 100a8513 _freopen

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


// 100a8540 swap

/* Library Function - Single Match
    _swap
   
   Library: Visual Studio 2005 Release */

void __cdecl swap(void)

{
  undefined1 uVar1;
  undefined1 *in_EAX;
  undefined1 *in_ECX;
  int in_EDX;
  
  if ((in_EAX != in_ECX) && (in_EDX != 0)) {
    do {
      uVar1 = *in_EAX;
      *in_EAX = *in_ECX;
      in_EDX = in_EDX + -1;
      *in_ECX = uVar1;
      in_EAX = in_EAX + 1;
      in_ECX = in_ECX + 1;
    } while (in_EDX != 0);
  }
  return;
}


// 100a8570 shortsort

/* Library Function - Single Match
    _shortsort
   
   Library: Visual Studio 2005 Release */

void __cdecl shortsort(undefined1 *param_1,undefined1 *param_2,int param_3,undefined *param_4)

{
  undefined1 uVar1;
  int iVar2;
  undefined1 *puVar3;
  undefined1 *puVar4;
  
  if (param_1 < param_2) {
    puVar3 = param_1 + param_3;
    puVar4 = param_1;
    do {
      for (; puVar3 <= param_2; puVar3 = puVar3 + param_3) {
        iVar2 = (*(code *)param_4)(puVar3,puVar4);
        if (0 < iVar2) {
          puVar4 = puVar3;
        }
      }
      if ((puVar4 != param_2) && (param_3 != 0)) {
        puVar3 = param_2;
        iVar2 = param_3;
        do {
          uVar1 = puVar3[(int)puVar4 - (int)param_2];
          puVar3[(int)puVar4 - (int)param_2] = *puVar3;
          iVar2 = iVar2 + -1;
          *puVar3 = uVar1;
          puVar3 = puVar3 + 1;
        } while (iVar2 != 0);
      }
      param_2 = param_2 + -param_3;
      puVar3 = param_1 + param_3;
      puVar4 = param_1;
    } while (param_1 < param_2);
  }
  return;
}


// 100a8600 _qsort

/* Library Function - Single Match
    _qsort
   
   Library: Visual Studio 2005 Release */

void __cdecl
_qsort(void *_Base,size_t _NumOfElements,size_t _SizeOfElements,_PtFuncCompare *_PtFuncCompare)

{
  undefined1 uVar1;
  DWORD *pDVar2;
  uint uVar3;
  undefined1 *puVar4;
  int iVar5;
  undefined1 *puVar6;
  size_t sVar7;
  undefined1 *puVar8;
  undefined1 *puVar9;
  undefined1 *local_100;
  undefined1 *local_fc;
  int local_f4;
  undefined4 auStack_f0 [30];
  undefined4 auStack_78 [30];
  
  if ((_Base == (void *)0x0) && (_NumOfElements != 0)) {
    pDVar2 = FUN_100ab320();
    *pDVar2 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return;
  }
  if ((_SizeOfElements == 0) || (_PtFuncCompare == (_PtFuncCompare *)0x0)) {
    pDVar2 = FUN_100ab320();
    *pDVar2 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return;
  }
  if (_NumOfElements < 2) {
    return;
  }
  local_f4 = 0;
  local_100 = (undefined1 *)_Base;
  local_fc = (undefined1 *)((_NumOfElements - 1) * _SizeOfElements + (int)_Base);
LAB_100a86a3:
  while (uVar3 = (uint)((int)local_fc - (int)local_100) / _SizeOfElements + 1, 8 < uVar3) {
    puVar4 = local_100 + (uVar3 >> 1) * _SizeOfElements;
    iVar5 = (*_PtFuncCompare)(local_100,puVar4);
    if (0 < iVar5) {
      swap();
    }
    iVar5 = (*_PtFuncCompare)(local_100,local_fc);
    if (0 < iVar5) {
      swap();
    }
    iVar5 = (*_PtFuncCompare)(puVar4,local_fc);
    puVar8 = local_100;
    puVar9 = local_fc;
    if (0 < iVar5) {
      swap();
    }
LAB_100a8750:
    if (puVar8 < puVar4) {
      do {
        puVar8 = puVar8 + _SizeOfElements;
        if (puVar4 <= puVar8) goto LAB_100a8770;
        iVar5 = (*_PtFuncCompare)(puVar8,puVar4);
      } while (iVar5 < 1);
      if (puVar4 <= puVar8) goto LAB_100a8770;
    }
    else {
LAB_100a8770:
      do {
        puVar8 = puVar8 + _SizeOfElements;
        if (local_fc < puVar8) break;
        iVar5 = (*_PtFuncCompare)(puVar8,puVar4);
      } while (iVar5 < 1);
    }
    do {
      puVar9 = puVar9 + -_SizeOfElements;
      if (puVar9 <= puVar4) break;
      iVar5 = (*_PtFuncCompare)(puVar9,puVar4);
    } while (0 < iVar5);
    if (puVar8 <= puVar9) {
      if (puVar8 != puVar9) {
        puVar6 = puVar9;
        sVar7 = _SizeOfElements;
        do {
          uVar1 = puVar6[(int)puVar8 - (int)puVar9];
          puVar6[(int)puVar8 - (int)puVar9] = *puVar6;
          sVar7 = sVar7 - 1;
          *puVar6 = uVar1;
          puVar6 = puVar6 + 1;
        } while (sVar7 != 0);
      }
      if (puVar4 == puVar9) {
        puVar4 = puVar8;
      }
      goto LAB_100a8750;
    }
    puVar9 = puVar9 + _SizeOfElements;
    if (puVar4 < puVar9) {
      do {
        puVar9 = puVar9 + -_SizeOfElements;
        if (puVar9 <= puVar4) goto LAB_100a8810;
        iVar5 = (*_PtFuncCompare)(puVar9,puVar4);
      } while (iVar5 == 0);
      if (puVar9 <= puVar4) goto LAB_100a8810;
    }
    else {
LAB_100a8810:
      do {
        puVar9 = puVar9 + -_SizeOfElements;
        if (puVar9 <= local_100) break;
        iVar5 = (*_PtFuncCompare)(puVar9,puVar4);
      } while (iVar5 == 0);
    }
    if ((int)puVar9 - (int)local_100 < (int)local_fc - (int)puVar8) goto LAB_100a886d;
    if (local_100 < puVar9) {
      auStack_f0[local_f4] = local_100;
      auStack_78[local_f4] = puVar9;
      local_f4 = local_f4 + 1;
    }
    local_100 = puVar8;
    if (local_fc <= puVar8) goto LAB_100a86c6;
  }
  shortsort(local_100,local_fc,_SizeOfElements,_PtFuncCompare);
  goto LAB_100a86c6;
LAB_100a886d:
  if (puVar8 < local_fc) {
    auStack_f0[local_f4] = puVar8;
    auStack_78[local_f4] = local_fc;
    local_f4 = local_f4 + 1;
  }
  local_fc = puVar9;
  if (puVar9 <= local_100) {
LAB_100a86c6:
    local_f4 = local_f4 + -1;
    if (local_f4 < 0) {
      return;
    }
    local_100 = (undefined1 *)auStack_f0[local_f4];
    local_fc = (undefined1 *)auStack_78[local_f4];
  }
  goto LAB_100a86a3;
}


// 100a88a7 _strncmp

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
          goto LAB_100a8961;
        }
        if ((pbVar1[1] == 0) || (pbVar1[1] != pbVar3[1])) {
          uVar2 = (uint)pbVar1[1];
          uVar4 = (uint)pbVar3[1];
          goto LAB_100a8961;
        }
        if ((pbVar1[2] == 0) || (pbVar1[2] != pbVar3[2])) {
          uVar2 = (uint)pbVar1[2];
          uVar4 = (uint)pbVar3[2];
          goto LAB_100a8961;
        }
        if ((pbVar1[3] == 0) || (pbVar1[3] != pbVar3[3])) {
          uVar2 = (uint)pbVar1[3];
          uVar4 = (uint)pbVar3[3];
          goto LAB_100a8961;
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
LAB_100a8961:
        return uVar2 - uVar4;
      }
      _Str1 = (char *)((byte *)_Str1 + 1);
      _Str2 = (char *)((byte *)_Str2 + 1);
    }
  }
  return 0;
}


// 100a8968 __setjmp3

/* Library Function - Single Match
    __setjmp3
   
   Library: Visual Studio */

undefined4 __cdecl __setjmp3(undefined4 *param_1,int param_2,int param_3,undefined4 param_4)

{
  int iVar1;
  uint uVar2;
  undefined4 unaff_EBX;
  undefined4 unaff_EBP;
  undefined4 unaff_ESI;
  undefined4 *puVar3;
  undefined4 unaff_EDI;
  undefined4 *puVar4;
  int *unaff_FS_OFFSET;
  undefined4 unaff_retaddr;
  
  *param_1 = unaff_EBP;
  param_1[1] = unaff_EBX;
  param_1[2] = unaff_EDI;
  param_1[3] = unaff_ESI;
  param_1[4] = register0x00000010;
  param_1[5] = unaff_retaddr;
  param_1[8] = 0x56433230;
  param_1[9] = 0;
  iVar1 = *unaff_FS_OFFSET;
  param_1[6] = iVar1;
  if (iVar1 == -1) {
    param_1[7] = 0xffffffff;
  }
  else if ((param_2 == 0) || (param_1[9] = param_3, iVar1 = param_3, param_2 == 1)) {
    param_1[7] = *(undefined4 *)(iVar1 + 0xc);
  }
  else {
    param_1[7] = param_4;
    uVar2 = param_2 - 2;
    if (uVar2 != 0) {
      puVar3 = (undefined4 *)&stack0x00000014;
      puVar4 = param_1 + 10;
      if (6 < uVar2) {
        uVar2 = 6;
      }
      for (; uVar2 != 0; uVar2 = uVar2 - 1) {
        *puVar4 = *puVar3;
        puVar3 = puVar3 + 1;
        puVar4 = puVar4 + 1;
      }
    }
  }
  return 0;
}


// 100a89e4 FID_conflict:___longjmp_internal

/* WARNING: Unable to track spacebase fully for stack */
/* Library Function - Multiple Matches With Different Base Names
    ___longjmp_internal
    _longjmp
   
   Library: Visual Studio 2019 Release */

void __cdecl FID_conflict____longjmp_internal(int *_Buf,int _Value)

{
  int iVar1;
  PVOID TargetFrame;
  int *unaff_FS_OFFSET;
  EXCEPTION_RECORD local_54;
  
  local_54.ExceptionCode = 0x80000026;
  local_54.ExceptionFlags = 0;
  local_54.ExceptionRecord = (_EXCEPTION_RECORD *)0x0;
  local_54.ExceptionAddress = (PVOID)0x0;
  local_54.NumberParameters = 0;
  TargetFrame = (PVOID)_Buf[6];
  if (TargetFrame != (PVOID)*unaff_FS_OFFSET) {
    RtlUnwind(TargetFrame,(PVOID)0x100a8a32,&local_54,(PVOID)0x0);
  }
  if (TargetFrame != (PVOID)0x0) {
    iVar1 = __rt_probe_read4_4();
    if ((iVar1 == 0) || (_Buf[8] != 0x56433230)) {
      __local_unwind2((int)TargetFrame,_Buf[7]);
    }
    else if ((code *)_Buf[9] != (code *)0x0) {
      (*(code *)_Buf[9])(_Buf);
    }
  }
  __NLG_Notify(0);
                    /* WARNING: Could not recover jumptable at 0x100a8a8e. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (*(code *)_Buf[5])();
  return;
}


// 100a8a95 FUN_100a8a95

errno_t __cdecl FUN_100a8a95(tm *param_1,uint *param_2)

{
  DWORD *pDVar1;
  errno_t eVar2;
  int iVar3;
  uint uVar4;
  uint uVar5;
  uint uVar6;
  bool bVar7;
  undefined8 uVar8;
  longlong lVar9;
  uint local_18;
  uint local_14;
  uint local_10;
  int local_c;
  uint local_8;
  
  local_c = 0;
  local_10 = 0;
  local_8 = 0;
  if (param_1 == (tm *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  _memset(param_1,0xff,0x24);
  if (param_2 == (uint *)0x0) {
LAB_100a8b0f:
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  uVar6 = param_2[1];
  if (((int)uVar6 < 1) && ((int)uVar6 < 0)) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    return 0x16;
  }
  if ((6 < (int)uVar6) && ((7 < (int)uVar6 || (0x93406fff < *param_2)))) goto LAB_100a8b0f;
  ___tzset();
  eVar2 = __get_daylight(&local_c);
  if (eVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  eVar2 = __get_dstbias((long *)&local_10);
  if (eVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  iVar3 = FUN_100b3886(&local_8);
  if (iVar3 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  uVar6 = param_2[1];
  uVar5 = *param_2;
  if ((-1 < (int)uVar6) && ((0 < (int)uVar6 || (0x3f480 < uVar5)))) {
    local_18 = uVar5 - local_8;
    local_14 = (uVar6 - ((int)local_8 >> 0x1f)) - (uint)(uVar5 < local_8);
    eVar2 = __gmtime64_s(param_1,(__time64_t *)&local_18);
    if (eVar2 != 0) {
      return eVar2;
    }
    if (local_c == 0) {
      return 0;
    }
    iVar3 = __isindst(param_1);
    if (iVar3 == 0) {
      return 0;
    }
    bVar7 = local_18 < local_10;
    local_18 = local_18 - local_10;
    local_14 = (local_14 - ((int)local_10 >> 0x1f)) - (uint)bVar7;
    eVar2 = __gmtime64_s(param_1,(__time64_t *)&local_18);
    if (eVar2 == 0) {
      param_1->tm_isdst = 1;
      return 0;
    }
    return eVar2;
  }
  eVar2 = __gmtime64_s(param_1,(__time64_t *)param_2);
  if (eVar2 != 0) {
    return eVar2;
  }
  if ((local_c == 0) || (iVar3 = __isindst(param_1), iVar3 == 0)) {
    uVar6 = param_1->tm_sec;
    uVar5 = uVar6 - local_8;
    local_14 = (((int)uVar6 >> 0x1f) - ((int)local_8 >> 0x1f)) - (uint)(uVar6 < local_8);
  }
  else {
    uVar4 = local_8 + local_10;
    uVar6 = param_1->tm_sec;
    uVar5 = uVar6 - uVar4;
    local_14 = (((int)uVar6 >> 0x1f) - ((int)uVar4 >> 0x1f)) - (uint)(uVar6 < uVar4);
    param_1->tm_isdst = 1;
  }
  uVar8 = __allrem(uVar5,local_14,0x3c,0);
  iVar3 = (int)uVar8;
  param_1->tm_sec = iVar3;
  if (iVar3 < 0) {
    bVar7 = 0x3b < uVar5;
    uVar5 = uVar5 - 0x3c;
    local_14 = (local_14 - 1) + (uint)bVar7;
    param_1->tm_sec = iVar3 + 0x3c;
  }
  lVar9 = __alldiv(uVar5,local_14,0x3c,0);
  lVar9 = lVar9 + param_1->tm_min;
  uVar6 = (uint)lVar9;
  local_14 = (uint)((ulonglong)lVar9 >> 0x20);
  uVar8 = __allrem(uVar6,local_14,0x3c,0);
  iVar3 = (int)uVar8;
  param_1->tm_min = iVar3;
  if (iVar3 < 0) {
    bVar7 = 0x3b < uVar6;
    uVar6 = uVar6 - 0x3c;
    local_14 = (local_14 - 1) + (uint)bVar7;
    param_1->tm_min = iVar3 + 0x3c;
  }
  lVar9 = __alldiv(uVar6,local_14,0x3c,0);
  lVar9 = lVar9 + param_1->tm_hour;
  uVar6 = (uint)lVar9;
  local_14 = (uint)((ulonglong)lVar9 >> 0x20);
  uVar8 = __allrem(uVar6,local_14,0x18,0);
  iVar3 = (int)uVar8;
  param_1->tm_hour = iVar3;
  if (iVar3 < 0) {
    bVar7 = 0x17 < uVar6;
    uVar6 = uVar6 - 0x18;
    local_14 = (local_14 - 1) + (uint)bVar7;
    param_1->tm_hour = iVar3 + 0x18;
  }
  lVar9 = __alldiv(uVar6,local_14,0x18,0);
  iVar3 = (int)lVar9;
  if (-1 < lVar9) {
    if ((0xffffffff < lVar9) || (iVar3 != 0)) {
      param_1->tm_mday = param_1->tm_mday + iVar3;
      param_1->tm_wday = (param_1->tm_wday + iVar3) % 7;
      goto LAB_100a8d11;
    }
    if (0xffffffff < lVar9) {
      return 0;
    }
    if (-1 < lVar9) {
      return 0;
    }
  }
  param_1->tm_mday = param_1->tm_mday + iVar3;
  param_1->tm_wday = (param_1->tm_wday + 7 + iVar3) % 7;
  if (param_1->tm_mday < 1) {
    param_1->tm_yday = param_1->tm_yday + iVar3 + 0x16d;
    param_1->tm_year = param_1->tm_year + -1;
    param_1->tm_mday = param_1->tm_mday + 0x1f;
    param_1->tm_mon = 0xb;
    return 0;
  }
LAB_100a8d11:
  param_1->tm_yday = param_1->tm_yday + iVar3;
  return 0;
}


// 100a8d5a FID_conflict:__localtime64

/* Library Function - Multiple Matches With Different Base Names
    __gmtime32
    __gmtime64
    __localtime32
    __localtime64
   
   Library: Visual Studio 2005 Release */

tm * __cdecl FID_conflict___localtime64(__time32_t *_Time)

{
  tm *ptVar1;
  errno_t eVar2;
  
  ptVar1 = ___getgmtimebuf();
  if (ptVar1 == (tm *)0x0) {
    return (tm *)0x0;
  }
  eVar2 = FUN_100a8a95(ptVar1,(uint *)_Time);
  return (tm *)(~-(uint)(eVar2 != 0) & (uint)ptVar1);
}


// 100a8d7e _fputs

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    _fputs
   
   Library: Visual Studio 2005 Release */

int __cdecl _fputs(char *_Str,FILE *_File)

{
  DWORD *pDVar1;
  int iVar2;
  uint uVar3;
  undefined *puVar4;
  size_t sVar5;
  
  if ((_Str == (char *)0x0) || (_File == (FILE *)0x0)) {
LAB_100a8d98:
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    iVar2 = -1;
  }
  else {
    if ((_File->_flag & 0x40) == 0) {
      iVar2 = __fileno(_File);
      if (iVar2 == -1) {
LAB_100a8e0a:
        puVar4 = &DAT_100df6e0;
      }
      else {
        iVar2 = __fileno(_File);
        if (iVar2 == -2) goto LAB_100a8e0a;
        iVar2 = __fileno(_File);
        uVar3 = __fileno(_File);
        puVar4 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_100f5d00)[iVar2 >> 5]);
      }
      if ((puVar4[0x24] & 0x7f) != 0) goto LAB_100a8d98;
      iVar2 = __fileno(_File);
      if (iVar2 == -1) {
LAB_100a8e4f:
        puVar4 = &DAT_100df6e0;
      }
      else {
        iVar2 = __fileno(_File);
        if (iVar2 == -2) goto LAB_100a8e4f;
        iVar2 = __fileno(_File);
        uVar3 = __fileno(_File);
        puVar4 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_100f5d00)[iVar2 >> 5]);
      }
      if ((puVar4[0x24] & 0x80) != 0) goto LAB_100a8d98;
    }
    sVar5 = _strlen(_Str);
    FUN_100a7d64((uint)_File);
    iVar2 = __stbuf(_File);
    uVar3 = FUN_100b391a(_Str,1,sVar5,_File);
    __ftbuf(iVar2,_File);
    FUN_100a8eb8();
    iVar2 = (uVar3 == sVar5) - 1;
  }
  return iVar2;
}


// 100a8eb8 FUN_100a8eb8

void FUN_100a8eb8(void)

{
  uint unaff_ESI;
  
  FUN_100a7dce(unaff_ESI);
  return;
}


// 100a8ec0 __forcdecpt_l

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


// 100a8f31 __cropzeros_l

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


// 100a8fc7 __fassign_l

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


// 100a901e __shift

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


// 100a9057 __cftoe2_l

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
    pDVar1 = FUN_100ab320();
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
      if (((DAT_100f5900 & 1) != 0) && (_Dst[2] == '0')) {
        pcVar9 = (char *)0x3;
        _memmove(_Dst + 2,_Dst + 3,3);
      }
      if (local_8 == '\0') {
        return (DWORD)pcVar9;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return (DWORD)pcVar9;
    }
    pDVar1 = FUN_100ab320();
    DVar7 = 0x22;
  }
  DVar8 = 0;
  *pDVar1 = DVar7;
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return DVar8;
}


// 100a91c4 __cftoe_l

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100a9292 __cftoe

/* Library Function - Single Match
    __cftoe
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __cftoe(double *_Value,char *_Buf,size_t _SizeInBytes,int _Dec,int _Caps)

{
  errno_t eVar1;
  
  eVar1 = __cftoe_l(_Value,_Buf,_SizeInBytes,_Dec,_Caps,(localeinfo_struct *)0x0);
  return eVar1;
}


// 100a92b0 __cftoa_l

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
    pDVar2 = FUN_100ab320();
    DVar15 = 0x16;
LAB_100a92e9:
    DVar16 = 0;
    *pDVar2 = DVar15;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_1c != '\0') {
      *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
    }
    return DVar16;
  }
  *param_2 = 0;
  if (param_3 <= param_4 + 0xb) {
    pDVar2 = FUN_100ab320();
    DVar15 = 0x22;
    goto LAB_100a92e9;
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
    goto LAB_100a960d;
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
LAB_100a95bc:
    if ((-1 < (int)uVar8) && ((0 < (int)uVar8 || (99 < uVar7)))) goto LAB_100a95c7;
  }
  else {
    uVar14 = __alldvrm(uVar7,uVar8,1000,0);
    local_14 = (undefined4)((ulonglong)uVar14 >> 0x20);
    *pcVar10 = (char)uVar14 + '0';
    pcVar11 = pcVar4 + 3;
    uVar8 = 0;
    uVar7 = extraout_ECX;
    if (pcVar11 == pcVar10) goto LAB_100a95bc;
LAB_100a95c7:
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
LAB_100a960d:
  if (local_1c == '\0') {
    return unaff_EBX;
  }
  *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  return unaff_EBX;
}


// 100a9621 __cftof2_l

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
    pDVar2 = FUN_100ab320();
    uVar4 = 0x16;
    *pDVar2 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100a9716 __cftof_l

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_30,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100a97cf __cftog_l

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  __fltout2((_CRT_DOUBLE)*param_1,&local_34,local_20,0x16);
  if ((param_2 == (undefined1 *)0x0) || (param_3 == 0)) {
    pDVar2 = FUN_100ab320();
    *pDVar2 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100a98c7 __cfltcvt_l

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


// 100a994d __cfltcvt

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


// 100a996e __initp_misc_cfltcvt_tab

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
    piVar1 = (int *)((int)&PTR_LAB_100dedd4 + uVar3);
    iVar2 = FUN_100a9d71(*piVar1);
    uVar3 = uVar3 + 4;
    *piVar1 = iVar2;
  } while (uVar3 < 0x28);
  return;
}


// 100a998d __setdefaultprecision

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


// 100a99b6 __ms_p5_test_fdiv

/* WARNING: Removing unreachable block (ram,0x100a99e9) */
/* Library Function - Single Match
    __ms_p5_test_fdiv
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

undefined4 __ms_p5_test_fdiv(void)

{
  return 0;
}


// 100a99f2 __ms_p5_mp_test_fdiv

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


// 100a9a1b _abort

/* Library Function - Single Match
    _abort
   
   Library: Visual Studio 2005 Release */

void __cdecl _abort(void)

{
  _PHNDLR p_Var1;
  EXCEPTION_RECORD local_32c;
  _EXCEPTION_POINTERS local_2dc;
  undefined4 local_2d4;
  
  if (((byte)DAT_100dedfc & 1) != 0) {
    FUN_100a9b2c(10);
  }
  p_Var1 = ___get_sigabrt();
  if (p_Var1 != (_PHNDLR)0x0) {
    _raise(0x16);
  }
  if (((byte)DAT_100dedfc & 2) != 0) {
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


// 100a9b0e FUN_100a9b0e

void __cdecl FUN_100a9b0e(uint param_1,uint param_2)

{
  DAT_100dedfc = ~param_2 & DAT_100dedfc | param_1 & param_2;
  return;
}


// 100a9b2c FUN_100a9b2c

void __cdecl FUN_100a9b2c(int param_1)

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
    if (param_1 == (&DAT_100dee00)[uVar6 * 2]) break;
    uVar6 = uVar6 + 1;
  } while (uVar6 < 0x17);
  if (uVar6 < 0x17) {
    iVar2 = __set_error_mode(3);
    if ((iVar2 == 1) || ((iVar2 = __set_error_mode(3), iVar2 == 0 && (DAT_100f51e4 == 1)))) {
      hFile = GetStdHandle(0xfffffff4);
      if ((hFile != (HANDLE)0x0) && (hFile != (HANDLE)0xffffffff)) {
        lpOverlapped = (LPOVERLAPPED)0x0;
        lpNumberOfBytesWritten = &DStack_4;
        puVar1 = (undefined4 *)(uVar6 * 8 + 0x100dee04);
        sVar5 = _strlen((char *)*puVar1);
        WriteFile(hFile,(LPCVOID)*puVar1,sVar5,lpNumberOfBytesWritten,lpOverlapped);
      }
    }
    else if (param_1 != 0xfc) {
      eVar3 = _strcpy_s(&DAT_100f5238,0x314,"Runtime Error!\n\nProgram: ");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      DAT_100f5355 = 0;
      DVar4 = GetModuleFileNameA((HMODULE)0x0,&DAT_100f5251,0x104);
      if ((DVar4 == 0) &&
         (eVar3 = _strcpy_s(&DAT_100f5251,0x2fb,"<program name unknown>"), eVar3 != 0)) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      sVar5 = _strlen(&DAT_100f5251);
      if (0x3c < sVar5 + 1) {
        sVar5 = _strlen(&DAT_100f5251);
        _Dst = (char *)((int)&DAT_100f5214 + sVar5 + 2);
        eVar3 = _strncpy_s(_Dst,(int)&DAT_100f554c - (int)_Dst,"...",3);
        if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
          __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        }
      }
      eVar3 = _strcat_s(&DAT_100f5238,0x314,"\n\n");
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      eVar3 = _strcat_s(&DAT_100f5238,0x314,*(char **)(uVar6 * 8 + 0x100dee04));
      if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      FUN_100b3fa2(&DAT_100f5238,"Microsoft Visual C++ Runtime Library",0x12010);
    }
  }
  return;
}


// 100a9ccc __FF_MSGBANNER

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
    if (DAT_100f51e4 != 1) {
      return;
    }
  }
  FUN_100a9b2c(0xfc);
  FUN_100a9b2c(0xff);
  return;
}


// 100a9d05 FUN_100a9d05

undefined4 FUN_100a9d05(void)

{
  int iVar1;
  HMODULE pHVar2;
  int iVar3;
  uint uVar4;
  char *_Str2;
  int local_8;
  
  uVar4 = 0;
  local_8 = 0;
  FUN_100a820d(&local_8);
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


// 100a9d71 FUN_100a9d71

int __cdecl FUN_100a9d71(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_100deebc);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_100deeb8 != -1)) {
    iVar3 = DAT_100deeb8;
    pcVar2 = (code *)TlsGetValue(DAT_100deebc);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1f8);
      goto LAB_100a9dcb;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_100a9d05();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"EncodePointer");
LAB_100a9dcb:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 100a9ddf __encoded_null

/* Library Function - Single Match
    __encoded_null
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __encoded_null(void)

{
  FUN_100a9d71(0);
  return;
}


// 100a9de8 FUN_100a9de8

int __cdecl FUN_100a9de8(int param_1)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  HMODULE hModule;
  FARPROC pFVar4;
  
  pvVar1 = TlsGetValue(DAT_100deebc);
  if ((pvVar1 != (LPVOID)0x0) && (DAT_100deeb8 != -1)) {
    iVar3 = DAT_100deeb8;
    pcVar2 = (code *)TlsGetValue(DAT_100deebc);
    iVar3 = (*pcVar2)(iVar3);
    if (iVar3 != 0) {
      pFVar4 = *(FARPROC *)(iVar3 + 0x1fc);
      goto LAB_100a9e42;
    }
  }
  hModule = GetModuleHandleA("KERNEL32.DLL");
  if (hModule == (HMODULE)0x0) {
    return param_1;
  }
  iVar3 = FUN_100a9d05();
  if (iVar3 == 0) {
    return param_1;
  }
  pFVar4 = GetProcAddress(hModule,"DecodePointer");
LAB_100a9e42:
  if (pFVar4 != (FARPROC)0x0) {
    param_1 = (*pFVar4)(param_1);
  }
  return param_1;
}


// 100a9e5f FUN_100a9e5f

LPVOID FUN_100a9e5f(void)

{
  LPVOID lpTlsValue;
  
  lpTlsValue = TlsGetValue(DAT_100deebc);
  if (lpTlsValue == (LPVOID)0x0) {
    lpTlsValue = (LPVOID)FUN_100a9de8(DAT_100f5550);
    TlsSetValue(DAT_100deebc,lpTlsValue);
  }
  return lpTlsValue;
}


// 100a9e91 __mtterm

/* Library Function - Single Match
    __mtterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __mtterm(void)

{
  code *pcVar1;
  int iVar2;
  
  if (DAT_100deeb8 != -1) {
    iVar2 = DAT_100deeb8;
    pcVar1 = (code *)FUN_100a9de8(DAT_100f5558);
    (*pcVar1)(iVar2);
    DAT_100deeb8 = -1;
  }
  if (DAT_100deebc != 0xffffffff) {
    TlsFree(DAT_100deebc);
    DAT_100deebc = 0xffffffff;
  }
  __mtdeletelocks();
  return;
}


// 100a9ece FUN_100a9ece

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

void __cdecl FUN_100a9ece(int param_1,int param_2)

{
  HMODULE hModule;
  int iVar1;
  FARPROC pFVar2;
  
  hModule = GetModuleHandleA("KERNEL32.DLL");
  *(undefined **)(param_1 + 0x5c) = &DAT_100df718;
  *(undefined4 *)(param_1 + 0x14) = 1;
  if (hModule != (HMODULE)0x0) {
    iVar1 = FUN_100a9d05();
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
  *(undefined **)(param_1 + 0x68) = &DAT_100df038;
  InterlockedIncrement((LONG *)&DAT_100df038);
  __lock(0xc);
  *(int *)(param_1 + 0x6c) = param_2;
  if (param_2 == 0) {
    *(undefined **)(param_1 + 0x6c) = PTR_DAT_100df640;
  }
  ___addlocaleref(*(LONG **)(param_1 + 0x6c));
  FUN_100a9f84();
  return;
}


// 100a9f84 FUN_100a9f84

void FUN_100a9f84(void)

{
  FUN_100afa6b(0xc);
  return;
}


// 100a9f8d FUN_100a9f8d

DWORD * FUN_100a9f8d(void)

{
  DWORD dwErrCode;
  code *pcVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD DVar4;
  undefined4 uVar5;
  DWORD *pDVar6;
  
  dwErrCode = GetLastError();
  uVar5 = DAT_100deeb8;
  pcVar1 = (code *)FUN_100a9e5f();
  pDVar2 = (DWORD *)(*pcVar1)(uVar5);
  if (pDVar2 == (DWORD *)0x0) {
    pDVar2 = (DWORD *)FUN_10002e20(1,0x214);
    if (pDVar2 != (DWORD *)0x0) {
      uVar5 = DAT_100deeb8;
      pDVar6 = pDVar2;
      pcVar1 = (code *)FUN_100a9de8(DAT_100f5554);
      iVar3 = (*pcVar1)(uVar5,pDVar6);
      if (iVar3 == 0) {
        FUN_10002ea0(pDVar2);
        pDVar2 = (DWORD *)0x0;
      }
      else {
        FUN_100a9ece((int)pDVar2,0);
        DVar4 = GetCurrentThreadId();
        pDVar2[1] = 0xffffffff;
        *pDVar2 = DVar4;
      }
    }
  }
  SetLastError(dwErrCode);
  return pDVar2;
}


// 100aa004 __getptd

/* Library Function - Single Match
    __getptd
   
   Library: Visual Studio 2005 Release */

_ptiddata __cdecl __getptd(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = (_ptiddata)FUN_100a9f8d();
  if (p_Var1 == (_ptiddata)0x0) {
    __amsg_exit(0x10);
  }
  return p_Var1;
}


// 100aa01c __freefls@4

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
      FUN_10002ea0(*(int *)(param_1 + 0x24));
    }
    if (*(int *)(param_1 + 0x2c) != 0) {
      FUN_10002ea0(*(int *)(param_1 + 0x2c));
    }
    if (*(int *)(param_1 + 0x34) != 0) {
      FUN_10002ea0(*(int *)(param_1 + 0x34));
    }
    if (*(int *)(param_1 + 0x3c) != 0) {
      FUN_10002ea0(*(int *)(param_1 + 0x3c));
    }
    if (*(int *)(param_1 + 0x44) != 0) {
      FUN_10002ea0(*(int *)(param_1 + 0x44));
    }
    if (*(int *)(param_1 + 0x48) != 0) {
      FUN_10002ea0(*(int *)(param_1 + 0x48));
    }
    if (*(undefined **)(param_1 + 0x5c) != &DAT_100df718) {
      FUN_10002ea0(*(undefined **)(param_1 + 0x5c));
    }
    __lock(0xd);
    pLVar1 = *(LONG **)(param_1 + 0x68);
    if (pLVar1 != (LONG *)0x0) {
      LVar2 = InterlockedDecrement(pLVar1);
      if ((LVar2 == 0) && (pLVar1 != (LONG *)&DAT_100df038)) {
        FUN_10002ea0(pLVar1);
      }
    }
    FUN_100aa128();
    __lock(0xc);
    pLVar1 = *(LONG **)(param_1 + 0x6c);
    if (pLVar1 != (LONG *)0x0) {
      ___removelocaleref(pLVar1);
      if (((pLVar1 != (LONG *)PTR_DAT_100df640) && (pLVar1 != (LONG *)&DAT_100df568)) &&
         (*pLVar1 == 0)) {
        ___freetlocinfo((int)pLVar1);
      }
    }
    FUN_100aa134();
    FUN_10002ea0(param_1);
  }
  return;
}


// 100aa128 FUN_100aa128

void FUN_100aa128(void)

{
  FUN_100afa6b(0xd);
  return;
}


// 100aa134 FUN_100aa134

void FUN_100aa134(void)

{
  FUN_100afa6b(0xc);
  return;
}


// 100aa13d __freeptd

/* Library Function - Single Match
    __freeptd
   
   Library: Visual Studio 2005 Release */

void __cdecl __freeptd(_ptiddata _Ptd)

{
  LPVOID pvVar1;
  code *pcVar2;
  int iVar3;
  undefined4 uVar4;
  
  if (DAT_100deeb8 != -1) {
    if ((_Ptd == (_ptiddata)0x0) && (pvVar1 = TlsGetValue(DAT_100deebc), pvVar1 != (LPVOID)0x0)) {
      iVar3 = DAT_100deeb8;
      pcVar2 = (code *)TlsGetValue(DAT_100deebc);
      _Ptd = (_ptiddata)(*pcVar2)(iVar3);
    }
    uVar4 = 0;
    iVar3 = DAT_100deeb8;
    pcVar2 = (code *)FUN_100a9de8(DAT_100f5554);
    (*pcVar2)(iVar3,uVar4);
    __freefls_4((int)_Ptd);
  }
  if (DAT_100deebc != 0xffffffff) {
    TlsSetValue(DAT_100deebc,(LPVOID)0x0);
  }
  return;
}


// 100aa1a6 __mtinit

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
  DAT_100f554c = GetProcAddress(hModule,"FlsAlloc");
  DAT_100f5550 = GetProcAddress(hModule,"FlsGetValue");
  DAT_100f5554 = GetProcAddress(hModule,"FlsSetValue");
  DAT_100f5558 = GetProcAddress(hModule,"FlsFree");
  if ((((DAT_100f554c == (FARPROC)0x0) || (DAT_100f5550 == (FARPROC)0x0)) ||
      (DAT_100f5554 == (FARPROC)0x0)) || (DAT_100f5558 == (FARPROC)0x0)) {
    DAT_100f5550 = TlsGetValue_exref;
    DAT_100f554c = (FARPROC)&LAB_100a9e56;
    DAT_100f5554 = TlsSetValue_exref;
    DAT_100f5558 = TlsFree_exref;
  }
  DAT_100deebc = TlsAlloc();
  if ((DAT_100deebc != 0xffffffff) && (BVar1 = TlsSetValue(DAT_100deebc,DAT_100f5550), BVar1 != 0))
  {
    __init_pointers();
    DAT_100f554c = (FARPROC)FUN_100a9d71((int)DAT_100f554c);
    DAT_100f5550 = (FARPROC)FUN_100a9d71((int)DAT_100f5550);
    DAT_100f5554 = (FARPROC)FUN_100a9d71((int)DAT_100f5554);
    DAT_100f5558 = (FARPROC)FUN_100a9d71((int)DAT_100f5558);
    iVar2 = __mtinitlocks();
    if (iVar2 != 0) {
      pcVar6 = __freefls_4;
      pcVar3 = (code *)FUN_100a9de8((int)DAT_100f554c);
      DAT_100deeb8 = (*pcVar3)(pcVar6);
      if ((DAT_100deeb8 != -1) && (pDVar4 = (DWORD *)FUN_10002e20(1,0x214), pDVar4 != (DWORD *)0x0))
      {
        iVar2 = DAT_100deeb8;
        pDVar7 = pDVar4;
        pcVar3 = (code *)FUN_100a9de8((int)DAT_100f5554);
        iVar2 = (*pcVar3)(iVar2,pDVar7);
        if (iVar2 != 0) {
          FUN_100a9ece((int)pDVar4,0);
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


// 100aa32a FUN_100aa32a

void __cdecl FUN_100aa32a(undefined4 param_1)

{
  DAT_100f5564 = param_1;
  return;
}


// 100aa334 fastzero_I

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


// 100aa38b __VEC_memzero

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


// 100aa430 __SEH_prolog4

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
  *(uint *)((int)auStack_1c + iVar1 + 4) = DAT_100df7a0 ^ (uint)&param_2;
  *(undefined4 *)((int)auStack_1c + iVar1) = unaff_retaddr;
  *unaff_FS_OFFSET = (int)local_8;
  return;
}


// 100aa475 __SEH_epilog4

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


// 100aa490 __except_handler4

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
  
  piVar8 = (int *)(*(uint *)((int)param_2 + 8) ^ DAT_100df7a0);
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
          goto LAB_100aa53b;
        }
        if (0 < iVar4) {
          if (((*param_1 == -0x1f928c9d) && (DAT_100f5e00 != (code *)0x0)) &&
             (BVar5 = __IsNonwritableInCurrentImage((PBYTE)&DAT_100f5e00), BVar5 != 0)) {
            (*DAT_100f5e00)(param_1,1);
          }
          _EH4_GlobalUnwind(param_2);
          if (*(uint *)((int)param_2 + 0xc) != uVar7) {
            _EH4_LocalUnwind((int)param_2,uVar7,iVar1,&DAT_100df7a0);
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
    _EH4_LocalUnwind((int)param_2,0xfffffffe,iVar1,&DAT_100df7a0);
  }
LAB_100aa53b:
  if (*piVar8 != -2) {
    __security_check_cookie(piVar8[1] + iVar1 ^ *(uint *)(*piVar8 + iVar1));
  }
  __security_check_cookie(piVar8[3] + iVar1 ^ *(uint *)(piVar8[2] + iVar1));
  return local_c;
}


// 100aa626 __flsbuf

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
    pDVar2 = FUN_100ab320();
    *pDVar2 = 9;
LAB_100aa64a:
    _File_00->_flag = _File_00->_flag | 0x20;
    return -1;
  }
  if ((uVar6 & 0x40) != 0) {
    pDVar2 = FUN_100ab320();
    *pDVar2 = 0x22;
    goto LAB_100aa64a;
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
     (((ppuVar3 = FUN_100a7c8d(), _File_00 != (FILE *)(ppuVar3 + 8) &&
       (ppuVar3 = FUN_100a7c8d(), _File_00 != (FILE *)(ppuVar3 + 0x10))) ||
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
        puVar5 = &DAT_100df6e0;
      }
      else {
        puVar5 = (undefined *)(((uint)_File & 0x1f) * 0x38 + (&DAT_100f5d00)[(int)_File >> 5]);
      }
      if (((puVar5[4] & 0x20) != 0) &&
         (lVar7 = __lseeki64((int)_File,0x200000000,unaff_EDI), lVar7 == -1)) goto LAB_100aa770;
    }
    else {
      local_8 = __write((int)_File,_Buf,uVar6);
    }
    *_File_00->_base = (char)_Ch;
  }
  if (local_8 == uVar6) {
    return _Ch & 0xff;
  }
LAB_100aa770:
  _File_00->_flag = _File_00->_flag | 0x20;
  return -1;
}


// 100aa786 write_char

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


// 100aa7b9 write_multi_char

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


// 100aa7dd write_string

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
        pDVar1 = FUN_100ab320();
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


// 100aa827 FUN_100aa827

/* WARNING: Type propagation algorithm not settling */

void __cdecl FUN_100aa827(FILE *param_1,byte *param_2,localeinfo_struct *param_3,wchar_t *param_4)

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
  
  local_8 = DAT_100df7a0 ^ (uint)local_1fc;
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
        puVar7 = &DAT_100df6e0;
      }
      else {
        iVar5 = __fileno(local_22c);
        uVar6 = __fileno(local_22c);
        puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_100f5d00)[iVar5 >> 5]);
      }
      if ((puVar7[0x24] & 0x7f) == 0) {
        iVar5 = __fileno(local_22c);
        if ((iVar5 == -1) || (iVar5 = __fileno(local_22c), iVar5 == -2)) {
          puVar7 = &DAT_100df6e0;
        }
        else {
          iVar5 = __fileno(local_22c);
          uVar6 = __fileno(local_22c);
          puVar7 = (undefined *)((uVar6 & 0x1f) * 0x38 + (&DAT_100f5d00)[iVar5 >> 5]);
        }
        if ((puVar7[0x24] & 0x80) == 0) goto LAB_100aa965;
      }
    }
    else {
LAB_100aa965:
      if (param_2 != (byte *)0x0) {
        local_215 = *param_2;
        local_230 = 0;
        local_224 = 0;
        local_250 = (wchar_t *)0x0;
        iVar5 = 0;
        while ((local_215 != 0 && (pbVar12 = param_2 + 1, local_244 = pbVar12, -1 < local_230))) {
          if ((byte)(local_215 - 0x20) < 0x59) {
            uVar6 = (byte)"FlsAlloc"[(char)local_215 + 8] & 0xf;
          }
          else {
            uVar6 = 0;
          }
          local_270 = (int)(char)(&DAT_100cf000)[uVar6 * 8 + iVar5] >> 4;
          switch(local_270) {
          case 0:
switchD_100aa9c9_caseD_0:
            local_240 = 0;
            iVar5 = __isleadbyte_l((uint)local_215,&local_260);
            if (iVar5 != 0) {
              write_char();
              local_244 = param_2 + 2;
              if (*pbVar12 == 0) {
LAB_100ab15c:
                pDVar4 = FUN_100ab320();
                *pDVar4 = 0x16;
                goto LAB_100aa897;
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
                goto switchD_100aa9c9_caseD_0;
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
LAB_100aada8:
                local_214 = local_214 | 0x40;
LAB_100aadac:
                local_224 = 10;
LAB_100aadb3:
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
                  goto LAB_100aac31;
                }
                if (local_215 == 0x41) {
LAB_100aabdd:
                  local_215 = local_215 + 0x20;
                  local_274 = 1;
LAB_100aabea:
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
                      local_250 = (wchar_t *)FUN_10002dc0(sVar13);
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
                  pcVar9 = (code *)FUN_100a9de8((int)PTR_LAB_100dedec);
                  (*pcVar9)(puVar17,pwVar18,sVar13,iVar8,iVar5,uVar19,plVar20);
                  uVar6 = local_214 & 0x80;
                  if ((uVar6 != 0) && (local_21c == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_100a9de8((int)PTR_LAB_100dedf8);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((local_215 == 0x67) && (uVar6 == 0)) {
                    plVar20 = &local_260;
                    pwVar18 = pwVar14;
                    pcVar9 = (code *)FUN_100a9de8((int)PTR_LAB_100dedf4);
                    (*pcVar9)(pwVar18,plVar20);
                  }
                  if ((char)*pwVar14 == '-') {
                    local_214 = local_214 | 0x100;
                    local_220 = (wchar_t *)((int)pwVar14 + 1);
                    pwVar14 = local_220;
                  }
LAB_100aad1e:
                  local_224 = _strlen((char *)pwVar14);
                }
                else if (local_215 == 0x43) {
                  if ((local_214 & 0x830) == 0) {
                    local_214 = local_214 | 0x800;
                  }
LAB_100aac8e:
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
                else if ((local_215 == 0x45) || (local_215 == 0x47)) goto LAB_100aabdd;
              }
              else {
                if (local_215 == 0x58) goto LAB_100aaeb3;
                if (local_215 == 0x5a) {
                  psVar2 = *(short **)param_4;
                  local_228 = param_4 + 2;
                  pwVar14 = (wchar_t *)PTR_s__null__100deec0;
                  local_220 = (wchar_t *)PTR_s__null__100deec0;
                  if ((psVar2 == (short *)0x0) ||
                     (pwVar18 = *(wchar_t **)(psVar2 + 2), pwVar18 == (wchar_t *)0x0))
                  goto LAB_100aad1e;
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
                  if (local_215 == 0x61) goto LAB_100aabea;
                  if (local_215 == 99) goto LAB_100aac8e;
                }
              }
LAB_100ab02b:
              if (local_24c == 0) {
                if ((local_214 & 0x40) != 0) {
                  if ((local_214 & 0x100) == 0) {
                    if ((local_214 & 1) == 0) {
                      if ((local_214 & 2) == 0) goto LAB_100ab061;
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
LAB_100ab061:
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
LAB_100aac31:
                  iVar5 = local_21c;
                  if (local_21c == -1) {
                    iVar5 = 0x7fffffff;
                  }
                  local_228 = param_4 + 2;
                  local_220 = *(wchar_t **)param_4;
                  if ((local_214 & 0x810) == 0) {
                    pwVar14 = local_220;
                    if (local_220 == (wchar_t *)0x0) {
                      pwVar14 = (wchar_t *)PTR_s__null__100deec0;
                      local_220 = (wchar_t *)PTR_s__null__100deec0;
                    }
                    for (; (iVar5 != 0 && (iVar5 = iVar5 + -1, (char)*pwVar14 != '\0'));
                        pwVar14 = (wchar_t *)((int)pwVar14 + 1)) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220;
                  }
                  else {
                    if (local_220 == (wchar_t *)0x0) {
                      local_220 = (wchar_t *)PTR_u__null__100deec4;
                    }
                    local_240 = 1;
                    for (pwVar14 = local_220;
                        (iVar5 != 0 && (iVar5 = iVar5 + -1, *pwVar14 != L'\0'));
                        pwVar14 = pwVar14 + 1) {
                    }
                    local_224 = (int)pwVar14 - (int)local_220 >> 1;
                  }
                  goto LAB_100ab02b;
                }
                if (local_215 == 0x75) goto LAB_100aadac;
                if (local_215 != 0x78) goto LAB_100ab02b;
                local_248 = 0x27;
LAB_100aaed9:
                local_224 = 0x10;
                if ((local_214 & 0x80) != 0) {
                  local_233 = (char)local_248 + 'Q';
                  local_234 = 0x30;
                  local_238 = 2;
                }
                goto LAB_100aadb3;
              }
              if (local_215 == 0x70) {
                local_21c = 8;
LAB_100aaeb3:
                local_248 = 7;
                goto LAB_100aaed9;
              }
              if ((char)local_215 < 'e') goto LAB_100ab02b;
              if ((char)local_215 < 'h') goto LAB_100aabea;
              if (local_215 == 0x69) goto LAB_100aada8;
              if (local_215 != 0x6e) {
                if (local_215 != 0x6f) goto LAB_100ab02b;
                local_224 = 8;
                if ((local_214 & 0x80) != 0) {
                  local_214 = local_214 | 0x200;
                }
                goto LAB_100aadb3;
              }
              piVar3 = *(int **)param_4;
              local_228 = param_4 + 2;
              bVar15 = FUN_100a7991();
              if (CONCAT31(extraout_var,bVar15) == 0) goto LAB_100ab15c;
              if ((local_214 & 0x20) == 0) {
                *piVar3 = local_230;
              }
              else {
                *(undefined2 *)piVar3 = (undefined2)local_230;
              }
              local_24c = 1;
            }
            if (local_250 != (wchar_t *)0x0) {
              FUN_10002ea0(local_250);
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
        goto LAB_100ab183;
      }
    }
  }
  pDVar4 = FUN_100ab320();
  *pDVar4 = 0x16;
LAB_100aa897:
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  if (local_254 != '\0') {
    *(uint *)(local_258 + 0x70) = *(uint *)(local_258 + 0x70) & 0xfffffffd;
  }
LAB_100ab183:
  __security_check_cookie(local_8 ^ (uint)local_1fc);
  return;
}


// 100ab1bb FUN_100ab1bb

void __cdecl FUN_100ab1bb(undefined4 param_1)

{
  DAT_100f5568 = param_1;
  return;
}


// 100ab1c5 __invoke_watson

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
  
  iVar1 = DAT_100df7a0;
  local_2d4 = 0x10001;
  _memset(&local_32c,0,0x50);
  local_2dc.ExceptionRecord = &local_32c;
  local_2dc.ContextRecord = (PCONTEXT)&local_2d4;
  local_32c.ExceptionCode = 0xc000000d;
  BVar2 = IsDebuggerPresent();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  LVar3 = UnhandledExceptionFilter(&local_2dc);
  if ((LVar3 == 0) && (BVar2 == 0)) {
    FUN_100b4fd5();
  }
  uExitCode = 0xc000000d;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  __security_check_cookie(iVar1);
  return;
}


// 100ab2c1 FUN_100ab2c1

void __cdecl
FUN_100ab2c1(wchar_t *param_1,wchar_t *param_2,wchar_t *param_3,uint param_4,uintptr_t param_5)

{
  code *UNRECOVERED_JUMPTABLE;
  
  UNRECOVERED_JUMPTABLE = (code *)FUN_100a9de8(DAT_100f5568);
  if (UNRECOVERED_JUMPTABLE != (code *)0x0) {
                    /* WARNING: Could not recover jumptable at 0x100ab2d5. Too many branches */
                    /* WARNING: Treating indirect jump as call */
    (*UNRECOVERED_JUMPTABLE)();
    return;
  }
  FUN_100b4fd5();
                    /* WARNING: Subroutine does not return */
  __invoke_watson(param_1,param_2,param_3,param_4,param_5);
}


// 100ab2e5 FUN_100ab2e5

int __cdecl FUN_100ab2e5(int param_1)

{
  uint uVar1;
  
  uVar1 = 0;
  do {
    if (param_1 == (&DAT_100deec8)[uVar1 * 2]) {
      return *(int *)(uVar1 * 8 + 0x100deecc);
    }
    uVar1 = uVar1 + 1;
  } while (uVar1 < 0x2d);
  if (param_1 - 0x13U < 0x12) {
    return 0xd;
  }
  return (-(uint)(0xe < param_1 - 0xbcU) & 0xe) + 8;
}


// 100ab320 FUN_100ab320

DWORD * FUN_100ab320(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_100a9f8d();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_100df030;
  }
  return pDVar1 + 2;
}


// 100ab333 FUN_100ab333

DWORD * FUN_100ab333(void)

{
  DWORD *pDVar1;
  
  pDVar1 = FUN_100a9f8d();
  if (pDVar1 == (DWORD *)0x0) {
    return (DWORD *)&DAT_100df034;
  }
  return pDVar1 + 3;
}


// 100ab346 FUN_100ab346

void __cdecl FUN_100ab346(DWORD param_1)

{
  DWORD *pDVar1;
  DWORD DVar2;
  
  pDVar1 = FUN_100ab333();
  *pDVar1 = param_1;
  DVar2 = FUN_100ab2e5(param_1);
  pDVar1 = FUN_100ab320();
  *pDVar1 = DVar2;
  return;
}


// 100ab364 FUN_100ab364

void __cdecl FUN_100ab364(undefined4 *param_1,undefined4 *param_2,uint param_3)

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


// 100ab3eb __VEC_memcpy

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
      FUN_100ab364(param_1,param_2,param_3 - uVar3);
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


// 100ab4ce FUN_100ab4ce

undefined4 FUN_100ab4ce(void)

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


// 100ab4fd setSBCS

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
    *puVar1 = puVar1[(int)&DAT_100df038 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  puVar1 = (undefined1 *)(in_EAX + 0x11d);
  iVar2 = 0x100;
  do {
    *puVar1 = puVar1[(int)&DAT_100df038 - in_EAX];
    puVar1 = puVar1 + 1;
    iVar2 = iVar2 + -1;
  } while (iVar2 != 0);
  return;
}


// 100ab552 setSBUpLow

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
  
  local_8 = DAT_100df7a0 ^ (uint)local_4a0;
  BVar3 = GetCPInfo(*(UINT *)(unaff_ESI + 4),&local_51c);
  if (BVar3 == 0) {
    uVar4 = 0;
    do {
      pcVar2 = (char *)(unaff_ESI + 0x11d + uVar4);
      if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) + 0x20 < (char *)0x1a) {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        cVar6 = (char)uVar4 + ' ';
LAB_100ab6b9:
        *pcVar2 = cVar6;
      }
      else {
        if (pcVar2 + (-0x61 - (unaff_ESI + 0x11d)) < (char *)0x1a) {
          pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
          *pbVar1 = *pbVar1 | 0x20;
          cVar6 = (char)uVar4 + -0x20;
          goto LAB_100ab6b9;
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
          goto LAB_100ab660;
        }
        *(undefined1 *)(unaff_ESI + 0x11d + uVar4) = 0;
      }
      else {
        pbVar1 = (byte *)(unaff_ESI + 0x1d + uVar4);
        *pbVar1 = *pbVar1 | 0x10;
        CVar5 = local_208[uVar4];
LAB_100ab660:
        *(CHAR *)(unaff_ESI + 0x11d + uVar4) = CVar5;
      }
      uVar4 = uVar4 + 1;
    } while (uVar4 < 0x100);
  }
  __security_check_cookie(local_8 ^ (uint)local_4a0);
  return;
}


// 100ab6dc ___updatetmbcinfo

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
  if (((p_Var1->_ownlocale & DAT_100df55c) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xd);
    lpAddend = p_Var1->ptmbcinfo;
    if (lpAddend != (pthreadmbcinfo)PTR_DAT_100df460) {
      if (lpAddend != (pthreadmbcinfo)0x0) {
        LVar2 = InterlockedDecrement(&lpAddend->refcount);
        if ((LVar2 == 0) && (lpAddend != (pthreadmbcinfo)&DAT_100df038)) {
          FUN_10002ea0(lpAddend);
        }
      }
      p_Var1->ptmbcinfo = (pthreadmbcinfo)PTR_DAT_100df460;
      lpAddend = (pthreadmbcinfo)PTR_DAT_100df460;
      InterlockedIncrement((LONG *)PTR_DAT_100df460);
    }
    FUN_100ab777();
  }
  else {
    lpAddend = p_Var1->ptmbcinfo;
  }
  if (lpAddend == (pthreadmbcinfo)0x0) {
    __amsg_exit(0x20);
  }
  return lpAddend;
}


// 100ab777 FUN_100ab777

void FUN_100ab777(void)

{
  FUN_100afa6b(0xd);
  return;
}


// 100ab780 getSystemCP

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
  DAT_100f556c = 0;
  if (unaff_ESI == -2) {
    DAT_100f556c = 1;
    UVar1 = GetOEMCP();
  }
  else if (unaff_ESI == -3) {
    DAT_100f556c = 1;
    UVar1 = GetACP();
  }
  else {
    if (unaff_ESI != -4) {
      if (local_8 == '\0') {
        DAT_100f556c = 0;
        return unaff_ESI;
      }
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      return unaff_ESI;
    }
    UVar1 = *(UINT *)(local_14[0] + 4);
    DAT_100f556c = 1;
  }
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return UVar1;
}


// 100ab7fa FUN_100ab7fa

void __cdecl FUN_100ab7fa(undefined4 param_1,int param_2)

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  uVar4 = getSystemCP((int)unaff_EDI);
  if (uVar4 != 0) {
    local_20 = (byte *)0x0;
    uVar5 = 0;
LAB_100ab836:
    if (*(uint *)((int)&DAT_100df468 + uVar5) != uVar4) goto code_r0x100ab842;
    _memset((void *)(param_2 + 0x1c),0,0x101);
    local_24 = 0;
    pbVar8 = &DAT_100df478 + (int)local_20 * 0x30;
    local_20 = pbVar8;
    do {
      for (; (*pbVar8 != 0 && (bVar3 = pbVar8[1], bVar3 != 0)); pbVar8 = pbVar8 + 2) {
        for (uVar5 = (uint)*pbVar8; uVar5 <= bVar3; uVar5 = uVar5 + 1) {
          pbVar2 = (byte *)(param_2 + 0x1d + uVar5);
          *pbVar2 = *pbVar2 | *(byte *)(local_24 + 0x100df464);
          bVar3 = pbVar8[1];
        }
      }
      local_24 = local_24 + 1;
      pbVar8 = local_20 + 8;
      local_20 = pbVar8;
    } while (local_24 < 4);
    *(uint *)(param_2 + 4) = uVar4;
    *(undefined4 *)(param_2 + 8) = 1;
    uVar9 = FUN_100ab4ce();
    *(undefined4 *)(param_2 + 0xc) = uVar9;
    puVar7 = (undefined2 *)(param_2 + 0x10);
    puVar10 = (undefined2 *)(&DAT_100df46c + extraout_ECX);
    iVar11 = 6;
    do {
      *puVar7 = *puVar10;
      puVar10 = puVar10 + 1;
      puVar7 = puVar7 + 1;
      iVar11 = iVar11 + -1;
    } while (iVar11 != 0);
    goto LAB_100ab967;
  }
LAB_100ab823:
  setSBCS(unaff_EDI);
LAB_100ab9c4:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
code_r0x100ab842:
  local_20 = (byte *)((int)local_20 + 1);
  uVar5 = uVar5 + 0x30;
  if (0xef < uVar5) goto code_r0x100ab84f;
  goto LAB_100ab836;
code_r0x100ab84f:
  if (((uVar4 == 65000) || (uVar4 == 0xfde9)) ||
     (BVar6 = IsValidCodePage(uVar4 & 0xffff), BVar6 == 0)) goto LAB_100ab9c4;
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
      uVar9 = FUN_100ab4ce();
      *(undefined4 *)(param_2 + 0xc) = uVar9;
      *(undefined4 *)(param_2 + 8) = extraout_EDX;
    }
    *(undefined4 *)(param_2 + 0x10) = 0;
    *(undefined4 *)(param_2 + 0x14) = 0;
    *(undefined4 *)(param_2 + 0x18) = 0;
LAB_100ab967:
    setSBUpLow(unaff_EDI);
    goto LAB_100ab9c4;
  }
  if (DAT_100f556c == 0) goto LAB_100ab9c4;
  goto LAB_100ab823;
}


// 100ab9d3 __setmbcp

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
    ptVar3 = (pthreadmbcinfo)FUN_10002dc0(0x220);
    if (ptVar3 != (pthreadmbcinfo)0x0) {
      ptVar7 = p_Var1->ptmbcinfo;
      ptVar8 = ptVar3;
      for (iVar6 = 0x88; iVar6 != 0; iVar6 = iVar6 + -1) {
        ptVar8->refcount = ptVar7->refcount;
        ptVar7 = (pthreadmbcinfo)&ptVar7->mbcodepage;
        ptVar8 = (pthreadmbcinfo)&ptVar8->mbcodepage;
      }
      ptVar3->refcount = 0;
      local_24 = FUN_100ab7fa(iVar2,(int)ptVar3);
      if (local_24 == 0) {
        LVar4 = InterlockedDecrement(&p_Var1->ptmbcinfo->refcount);
        if ((LVar4 == 0) && (p_Var1->ptmbcinfo != (pthreadmbcinfo)&DAT_100df038)) {
          FUN_10002ea0(p_Var1->ptmbcinfo);
        }
        p_Var1->ptmbcinfo = ptVar3;
        InterlockedIncrement((LONG *)ptVar3);
        if (((p_Var1->_ownlocale & 2) == 0) && (((byte)DAT_100df55c & 1) == 0)) {
          __lock(0xd);
          _DAT_100f557c = ptVar3->mbcodepage;
          _DAT_100f5580 = ptVar3->ismbcodepage;
          _DAT_100f5584 = *(undefined4 *)ptVar3->mbulinfo;
          for (iVar2 = 0; iVar2 < 5; iVar2 = iVar2 + 1) {
            (&DAT_100f5570)[iVar2] = ptVar3->mbulinfo[iVar2 + 2];
          }
          for (iVar2 = 0; iVar2 < 0x101; iVar2 = iVar2 + 1) {
            (&DAT_100df258)[iVar2] = ptVar3->mbctype[iVar2 + 4];
          }
          for (iVar2 = 0; iVar2 < 0x100; iVar2 = iVar2 + 1) {
            (&DAT_100df360)[iVar2] = ptVar3->mbcasemap[iVar2 + 4];
          }
          LVar4 = InterlockedDecrement((LONG *)PTR_DAT_100df460);
          if ((LVar4 == 0) && (PTR_DAT_100df460 != &DAT_100df038)) {
            FUN_10002ea0(PTR_DAT_100df460);
          }
          PTR_DAT_100df460 = (undefined *)ptVar3;
          InterlockedIncrement((LONG *)ptVar3);
          FUN_100abb34();
        }
      }
      else if (local_24 == -1) {
        if (ptVar3 != (pthreadmbcinfo)&DAT_100df038) {
          FUN_10002ea0(ptVar3);
        }
        pDVar5 = FUN_100ab320();
        *pDVar5 = 0x16;
      }
    }
  }
  return local_24;
}


// 100abb34 FUN_100abb34

void FUN_100abb34(void)

{
  FUN_100afa6b(0xd);
  return;
}


// 100abb6d ___initmbctable

/* Library Function - Single Match
    ___initmbctable
   
   Library: Visual Studio 2005 Release */

undefined4 ___initmbctable(void)

{
  if (DAT_100f5e18 == 0) {
    __setmbcp(-3);
    DAT_100f5e18 = 1;
  }
  return 0;
}


// 100abb8b ___freetlocinfo

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
       (*(undefined ***)(param_1 + 0xbc) != &PTR_DAT_100dfd18)) &&
      (*(int **)(param_1 + 0xb0) != (int *)0x0)) && (**(int **)(param_1 + 0xb0) == 0)) {
    piVar4 = *(int **)(param_1 + 0xb8);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_10002ea0(piVar4);
      ___free_lconv_mon(*(int *)(param_1 + 0xbc));
    }
    piVar4 = *(int **)(param_1 + 0xb4);
    if ((piVar4 != (int *)0x0) && (*piVar4 == 0)) {
      FUN_10002ea0(piVar4);
      ___free_lconv_num(*(int **)(param_1 + 0xbc));
    }
    FUN_10002ea0(*(undefined4 *)(param_1 + 0xb0));
    FUN_10002ea0(*(undefined4 *)(param_1 + 0xbc));
  }
  if ((*(int **)(param_1 + 0xc0) != (int *)0x0) && (**(int **)(param_1 + 0xc0) == 0)) {
    FUN_10002ea0(*(int *)(param_1 + 0xc4) + -0xfe);
    FUN_10002ea0(*(int *)(param_1 + 0xcc) + -0x80);
    FUN_10002ea0(*(int *)(param_1 + 0xd0) + -0x80);
    FUN_10002ea0(*(undefined4 *)(param_1 + 0xc0));
  }
  ppuVar1 = *(undefined ***)(param_1 + 0xd4);
  if ((ppuVar1 != &PTR_DAT_100dfc58) && (ppuVar1[0x2d] == (undefined *)0x0)) {
    ___free_lc_time(ppuVar1);
    FUN_10002ea0(*(undefined4 *)(param_1 + 0xd4));
  }
  piVar4 = (int *)(param_1 + 0x50);
  iVar3 = 6;
  do {
    if ((((undefined *)piVar4[-2] != &DAT_100df560) &&
        (piVar2 = (int *)*piVar4, piVar2 != (int *)0x0)) && (*piVar2 == 0)) {
      FUN_10002ea0(piVar2);
    }
    if (((piVar4[-1] != 0) && (piVar2 = (int *)piVar4[1], piVar2 != (int *)0x0)) && (*piVar2 == 0))
    {
      FUN_10002ea0(piVar2);
    }
    piVar4 = piVar4 + 4;
    iVar3 = iVar3 + -1;
  } while (iVar3 != 0);
  FUN_10002ea0(param_1);
  return;
}


// 100abccb ___addlocaleref

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
    if (((undefined *)pLVar1[-2] != &DAT_100df560) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 100abd51 ___removelocaleref

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
      if (((undefined *)pLVar1[-2] != &DAT_100df560) && ((LONG *)*pLVar1 != (LONG *)0x0)) {
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


// 100abddd __updatetlocinfoEx_nolock

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
        if ((*pLVar1 == 0) && (pLVar1 != (LONG *)&DAT_100df568)) {
          ___freetlocinfo((int)pLVar1);
        }
      }
    }
    return unaff_EDI;
  }
  return (LONG *)0x0;
}


// 100abe1b ___updatetlocinfo

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___updatetlocinfo
   
   Library: Visual Studio 2005 Release */

pthreadlocinfo __cdecl ___updatetlocinfo(void)

{
  _ptiddata p_Var1;
  
  p_Var1 = __getptd();
  if (((p_Var1->_ownlocale & DAT_100df55c) == 0) || (p_Var1->ptlocinfo == (pthreadlocinfo)0x0)) {
    __lock(0xc);
    __updatetlocinfoEx_nolock();
    FUN_100abe85();
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


// 100abe85 FUN_100abe85

void FUN_100abe85(void)

{
  FUN_100afa6b(0xc);
  return;
}


// 100abe91 __crtLCMapStringA_stat

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  if (DAT_100f55ac == 0) {
    iVar3 = LCMapStringW(0,0x100,L"",1,(LPWSTR)0x0,0);
    if (iVar3 == 0) {
      DVar4 = GetLastError();
      if (DVar4 == 0x78) {
        DAT_100f55ac = 2;
      }
    }
    else {
      DAT_100f55ac = 1;
    }
  }
  pcVar5 = (char *)param_3;
  pcVar8 = param_4;
  if (0 < (int)param_4) {
    do {
      pcVar8 = pcVar8 + -1;
      if (*pcVar5 == '\0') goto LAB_100abf00;
      pcVar5 = pcVar5 + 1;
    } while (pcVar8 != (char *)0x0);
    pcVar8 = (char *)0xffffffff;
LAB_100abf00:
    pcVar5 = param_4 + -(int)pcVar8;
    bVar2 = (int)(pcVar5 + -1) < (int)param_4;
    param_4 = pcVar5 + -1;
    if (bVar2) {
      param_4 = pcVar5;
    }
  }
  if ((DAT_100f55ac == 2) || (DAT_100f55ac == 0)) {
    local_10 = (LPCWSTR)0x0;
    local_14 = 0;
    if (param_1 == (localeinfo_struct *)0x0) {
      param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
    }
    if (param_7 == 0) {
      param_7 = *(int *)(*in_ECX + 4);
    }
    UVar7 = ___ansicp((LCID)param_1);
    if (UVar7 == 0xffffffff) goto LAB_100ac221;
    if (UVar7 == param_7) {
      LCMapStringA((LCID)param_1,param_2,(LPCSTR)param_3,(int)param_4,(LPSTR)param_5,(int)param_6);
    }
    else {
      local_10 = (LPCWSTR)___convertcp(param_7,UVar7,(char *)param_3,(uint *)&param_4,(LPSTR)0x0,0);
      if (local_10 == (LPCWSTR)0x0) goto LAB_100ac221;
      local_c = LCMapStringA((LCID)param_1,param_2,(LPCSTR)local_10,(int)param_4,(LPSTR)0x0,0);
      if (local_c != 0) {
        if (((int)local_c < 1) || (0xffffffe0 < local_c)) {
          pcVar5 = (char *)0x0;
        }
        else if (local_c + 8 < 0x401) {
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_100ac1fe;
          pcVar5 = &stack0xffffffe4;
        }
        else {
          pcVar5 = (char *)FUN_10002dc0(local_c + 8);
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
LAB_100ac1fe:
    if (local_10 != (LPCWSTR)0x0) {
      FUN_10002ea0(local_10);
    }
    if ((local_14 != 0) && (param_5 != local_14)) {
      FUN_10002ea0(local_14);
    }
    goto LAB_100ac221;
  }
  if (DAT_100f55ac != 1) goto LAB_100ac221;
  local_c = 0;
  if (param_7 == 0) {
    param_7 = *(int *)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar(param_7,(uint)(param_8 != 0) * 8 + 1,(LPCSTR)param_3,
                                    (int)param_4,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_100ac221;
  if (((int)cchWideChar < 1) || (0xffffffe0 / cchWideChar < 2)) {
    local_10 = (LPCWSTR)0x0;
  }
  else {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar6 = (LPCWSTR)&stack0xffffffdc;
      local_10 = (LPCWSTR)&stack0xffffffdc;
      if (&stack0x00000000 != (undefined1 *)0x24) {
LAB_100abfa8:
        local_10 = pWVar6 + 4;
      }
    }
    else {
      pWVar6 = (LPCWSTR)FUN_10002dc0(uVar1);
      local_10 = pWVar6;
      if (pWVar6 != (LPCWSTR)0x0) {
        pWVar6[0] = L'\xdddd';
        pWVar6[1] = L'\0';
        goto LAB_100abfa8;
      }
    }
  }
  if (local_10 == (LPCWSTR)0x0) goto LAB_100ac221;
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
          if (&stack0x00000000 == (undefined1 *)0x24) goto LAB_100ac0b7;
          pWVar6 = (LPCWSTR)&stack0xffffffe4;
        }
        else {
          pWVar6 = (LPCWSTR)FUN_10002dc0(uVar1);
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
LAB_100ac0b7:
  __freea(local_10);
LAB_100ac221:
  iVar3 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar3;
}


// 100ac233 ___crtLCMapStringA

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


// 100ac276 __isleadbyte_l

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


// 100ac2ac _isleadbyte

/* Library Function - Single Match
    _isleadbyte
   
   Library: Visual Studio 2005 Release */

int __cdecl _isleadbyte(int _C)

{
  int iVar1;
  
  iVar1 = __isleadbyte_l(_C,(_locale_t)0x0);
  return iVar1;
}


// 100ac2ba __isctype_l

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


// 100ac370 __fltin2

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  local_24 = _Str;
  uVar3 = 0;
  local_18 = FUN_100ac4ab((undefined2 *)&local_14,&local_28,_Str,0,0,0,0,in_stack_00000018);
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


// 100ac420 _strlen

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
    if ((char)uVar1 == '\0') goto LAB_100ac483;
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
LAB_100ac483:
  return (size_t)((int)puVar2 + (-1 - (int)_Str));
}


// 100ac4ab FUN_100ac4ab

void __cdecl
FUN_100ac4ab(undefined2 *param_1,int *param_2,char *param_3,int param_4,int param_5,int param_6,
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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
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
LAB_100ac530:
    cVar8 = *param_3;
    pcVar17 = param_3 + 1;
    switch(iVar14) {
    case 0:
      if ((byte)(cVar8 - 0x31U) < 9) {
LAB_100ac54d:
        iVar14 = 3;
        goto LAB_100ac54f;
      }
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) {
LAB_100ac564:
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
          if (cVar8 == '0') goto LAB_100ac57e;
          goto LAB_100ac709;
        }
        iVar14 = 2;
        uVar7 = 0x8000;
        param_3 = pcVar17;
      }
      goto LAB_100ac530;
    case 1:
      bVar4 = true;
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_100ac54d;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_100ac5b5;
      if ((cVar8 == '+') || (cVar8 == '-')) goto LAB_100ac5e5;
      if (cVar8 == '0') goto LAB_100ac57e;
      goto LAB_100ac5c5;
    case 2:
      if ((byte)(cVar8 - 0x31U) < 9) goto LAB_100ac54d;
      if (cVar8 == *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_100ac564;
      param_3 = local_58;
      if (cVar8 != '0') goto LAB_100ac734;
LAB_100ac57e:
      iVar14 = 1;
      param_3 = pcVar17;
      goto LAB_100ac530;
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
      if (cVar8 != *(char *)**(undefined4 **)(*param_8 + 0xbc)) goto LAB_100ac658;
LAB_100ac5b5:
      bVar4 = true;
      iVar14 = 4;
      param_3 = pcVar17;
      goto LAB_100ac530;
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
LAB_100ac658:
      if ((cVar8 == '+') || (cVar8 == '-')) {
LAB_100ac5e5:
        bVar4 = true;
        iVar14 = 0xb;
        param_3 = pcVar17 + -1;
      }
      else {
LAB_100ac5c5:
        bVar4 = true;
        if ((cVar8 < 'D') || (('E' < cVar8 && ((cVar8 < 'd' || ('e' < cVar8)))))) goto LAB_100ac709;
        iVar14 = 6;
        param_3 = pcVar17;
      }
      goto LAB_100ac530;
    case 5:
      bVar6 = true;
      param_3 = local_58;
      if ((byte)(cVar8 - 0x30U) < 10) {
        iVar14 = 4;
        goto LAB_100ac54f;
      }
      goto LAB_100ac734;
    case 6:
      local_58 = param_3 + -1;
      if (8 < (byte)(cVar8 - 0x31U)) {
        if (cVar8 == '+') goto LAB_100ac6f0;
        if (cVar8 == '-') goto LAB_100ac6e4;
LAB_100ac6d7:
        param_3 = local_58;
        if (cVar8 != '0') goto LAB_100ac734;
        iVar14 = 8;
        param_3 = pcVar17;
        goto LAB_100ac530;
      }
      break;
    case 7:
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_100ac6d7;
      break;
    case 8:
      bVar5 = true;
      while (cVar8 == '0') {
        cVar8 = *pcVar17;
        pcVar17 = pcVar17 + 1;
      }
      if (8 < (byte)(cVar8 - 0x31U)) goto LAB_100ac709;
      break;
    case 9:
      bVar5 = true;
      local_68 = 0;
      goto LAB_100ac796;
    default:
      goto switchD_100ac53c_caseD_a;
    case 0xb:
      if (param_7 != 0) {
        local_58 = param_3;
        if (cVar8 == '+') {
LAB_100ac6f0:
          iVar14 = 7;
          param_3 = pcVar17;
        }
        else {
          if (cVar8 != '-') goto LAB_100ac734;
LAB_100ac6e4:
          local_6c = -1;
          iVar14 = 7;
          param_3 = pcVar17;
        }
        goto LAB_100ac530;
      }
      iVar14 = 10;
      pcVar17 = param_3;
switchD_100ac53c_caseD_a:
      param_3 = pcVar17;
      if (iVar14 != 10) goto LAB_100ac530;
      goto LAB_100ac734;
    }
    iVar14 = 9;
LAB_100ac54f:
    param_3 = pcVar17 + -1;
    goto LAB_100ac530;
  }
  pDVar10 = FUN_100ab320();
  *pDVar10 = 0x16;
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
  goto LAB_100acb2c;
LAB_100ac796:
  if ((cVar8 < '0') || ('9' < cVar8)) goto LAB_100ac7b1;
  local_68 = local_68 * 10 + -0x30 + (int)cVar8;
  if (local_68 < 0x1451) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
    goto LAB_100ac796;
  }
  local_68 = 0x1451;
LAB_100ac7b1:
  while (('/' < cVar8 && (cVar8 < ':'))) {
    cVar8 = *pcVar17;
    pcVar17 = pcVar17 + 1;
  }
LAB_100ac709:
  param_3 = pcVar17 + -1;
LAB_100ac734:
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
    if (local_50 == 0) goto LAB_100acb0e;
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
      if ((int)local_58 < -0x1450) goto LAB_100acb0e;
      ppuVar20 = &PTR_DAT_100dfd30;
      if (local_58 != (char *)0x0) {
        if ((int)local_58 < 0) {
          local_58 = (char *)-(int)local_58;
          ppuVar20 = (undefined **)&DAT_100dfe90;
        }
        if (param_4 == 0) {
          local_40._0_2_ = 0;
        }
        iVar14 = uStack_4a;
        uVar22 = uStack_2e;
        iVar1 = uStack_2a;
        uVar2 = CONCAT22(local_40._2_2_,(undefined2)local_40);
        iVar12 = local_3c;
joined_r0x100ac83c:
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
LAB_100ac9f2:
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
                    if ((short)uVar24 < 1) goto LAB_100ac9f2;
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
                  if (0x7ffe < uVar24) goto LAB_100acab1;
                  local_40 = uVar22;
                  local_3c = iVar1;
                  uStack_38 = uStack_26;
                  local_36 = uVar24 | uVar9;
                  uVar2 = uVar22;
                  iVar12 = iVar1;
                }
                goto joined_r0x100ac83c;
              }
              iVar12 = 0;
              local_3c._0_2_ = 0;
              local_3c._2_2_ = 0;
              local_40._0_2_ = 0;
              local_40._2_2_ = 0;
            }
            else {
LAB_100acab1:
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
          goto joined_r0x100ac83c;
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
LAB_100acb0e:
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
LAB_100acb2c:
  local_40 = uVar2;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 100acb70 FUN_100acb70

void FUN_100acb70(void)

{
  float10 in_ST0;
  
  FUN_100acb8e((double)in_ST0);
  return;
}


// 100acb8e FUN_100acb8e

float10 __cdecl FUN_100acb8e(double param_1)

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
    dVar8 = dVar2 * *(double *)(&DAT_100d0000 + iVar5 * 2) - dVar12;
    uVar4 = SUB164(auVar10 | ZEXT816(0x200000000000),4) & 0x80000000;
    dVar9 = dVar8 * dVar8;
    auVar3._4_8_ = 0;
    auVar3._0_4_ = uVar4;
    auVar11._0_12_ = auVar3 << 0x20;
    auVar11._12_4_ = uVar4;
    return (float10)((((dVar9 * -0.044642857142857144 + -0.075) * dVar8 * dVar9 * dVar9 +
                      (dVar8 * dVar9 * -0.16666666666666666 -
                      ((double)(*(ulonglong *)(&DAT_100cf100 + iVar6) ^ (ulonglong)uVar4 << 0x20) -
                      6.123233995736766e-17))) -
                     ((dVar2 + dVar13) * ((double)CONCAT44(in_XMM0_Db,in_XMM0_Da) - dVar13)) /
                     (dVar2 * *(double *)(&DAT_100d0000 + iVar5 * 2) + dVar12)) -
                    ((double)(*(ulonglong *)(&DAT_100cf108 + iVar6) ^ auVar11._8_8_) -
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
    dVar12 = dVar12 * *(double *)(&DAT_100d0000 + iVar6 * 8);
    dVar18 = (dVar18 * dVar9 - dVar12) + (dVar2 - dVar18) * dVar9;
    dVar2 = ((dVar13 - dVar9 * dVar9) - dVar8) / (dVar12 + dVar12 + dVar18);
    iVar6 = iVar6 * 0x10;
    dVar9 = (double)(auVar14._8_8_ & 0xc00921fb54442d18) + *(double *)(&DAT_100cf108 + iVar6);
    dVar13 = dVar18 * dVar18;
    dVar8 = dVar2 + dVar9;
    return (float10)(double)((ulonglong)
                             ((dVar13 * -0.044642857142857144 + -0.075) * dVar18 * dVar13 * dVar13 +
                              dVar18 * dVar13 * -0.16666666666666666 +
                              (double)(auVar14._0_8_ & 0xbca1a62633145c07) +
                              *(double *)(&DAT_100cf100 + iVar6) + dVar2 + (dVar9 - dVar8) + dVar8)
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


// 100ad0d0 __trandisp1

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
                    /* WARNING: Could not recover jumptable at 0x100ad135. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(param_2 + (char)(&DAT_100df66c)[(byte)((bVar2 & 7) << 1 | (char)bVar2 < '\0')] + 0x10
              ))();
  return;
}


// 100ad137 __trandisp2

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
                    /* WARNING: Could not recover jumptable at 0x100ad1c1. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (**(code **)(param_2 + (char)((&DAT_100df66c)[(byte)(cVar2 << 1 | cVar2 < '\0') & 0xf] |
                               (&DAT_100df66c)[(byte)((bVar3 & 7) << 1 | (char)bVar3 < '\0')] << 2)
              + 0x10))();
  return;
}


// 100ad2a0 FUN_100ad2a0

float10 __fastcall
FUN_100ad2a0(undefined4 param_1,int param_2,undefined2 param_3,undefined4 param_4,undefined4 param_5
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


// 100ad2b7 __startOneArgErrorHandling

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


// 100ad300 FUN_100ad300

undefined1  [10] FUN_100ad300(void)

{
  float10 in_ST0;
  float10 fVar1;
  undefined1 auVar2 [10];
  
  fVar1 = (float10)f2xm1(-(ROUND(in_ST0) - in_ST0));
  auVar2 = (undefined1  [10])fscale((float10)1 + fVar1,ROUND(in_ST0));
  return auVar2;
}


// 100ad315 FUN_100ad315

void FUN_100ad315(void)

{
  return;
}


// 100ad32c FUN_100ad32c

undefined4 FUN_100ad32c(void)

{
  uint in_EAX;
  
  if ((in_EAX & 0x80000) != 0) {
    return 7;
  }
  return 1;
}


// 100ad345 __fload_withFB

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


// 100ad388 FUN_100ad388

uint __cdecl FUN_100ad388(undefined4 param_1,uint param_2)

{
  if ((param_2 & 0x7ff00000) != 0x7ff00000) {
    return param_2 & 0x7ff00000;
  }
  return param_2;
}


// 100ad39e FUN_100ad39e

void FUN_100ad39e(void)

{
  return;
}


// 100ad3ab __math_exit

/* Library Function - Single Match
    __math_exit
   
   Library: Visual Studio */

void __fastcall
__math_exit(undefined4 param_1,int param_2,undefined4 param_3,undefined4 param_4,undefined4 param_5)

{
  ushort in_FPUStatusWord;
  ushort unaff_retaddr;
  
  if (((unaff_retaddr != 0x27f) && ((unaff_retaddr & 0x20) != 0)) &&
     ((in_FPUStatusWord & 0x20) != 0)) {
    __startOneArgErrorHandling(param_1,param_2,unaff_retaddr,param_3,param_4,param_5);
    return;
  }
  return;
}


// 100ad3e9 FUN_100ad3e9

void __fastcall
FUN_100ad3e9(undefined4 param_1,int param_2,undefined4 param_3,undefined4 param_4,undefined4 param_5
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
    FUN_100ad2a0(param_1,0x1d,unaff_retaddr,param_3,param_4,param_5,param_6,param_7);
    return;
  }
  __startOneArgErrorHandling(param_1,param_2,unaff_retaddr,param_3,param_4,param_5);
  return;
}


// 100ad490 FUN_100ad490

void FUN_100ad490(void)

{
  float10 in_ST0;
  float10 in_ST1;
  
  FUN_100ad4a9((double)in_ST1,SUB84((double)in_ST0,0),(uint)((ulonglong)(double)in_ST0 >> 0x20));
  return;
}


// 100ad4a9 FUN_100ad4a9

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 __cdecl FUN_100ad4a9(double param_1,int param_2,uint param_3)

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
           *(double *)((int)&DAT_100d0960 + uVar2 * 4);
  dVar28 = *(double *)((int)&DAT_100d0960 + uVar2 * 4);
  dVar11 = *(double *)(&DAT_100d0d70 + uVar2 * 8);
  dVar30 = *(double *)(&UNK_100d0d78 + uVar2 * 8);
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
        goto LAB_100adcb0;
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
                goto LAB_100adc37;
              }
              if ((param_3 & 0x80000000) == 0) {
                return (float10)-INFINITY;
              }
              goto LAB_100adac9;
            }
            if ((param_3 & 0x80000000) != 0) {
              return (float10)0;
            }
            goto LAB_100adc37;
          }
          goto LAB_100adb0f;
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
            goto LAB_100ad526;
          }
          goto LAB_100ad944;
        }
        uVar24 = (ulonglong)param_1 >> 0x20;
        in_XMM2_Qa = auVar20._8_8_ >> 0x20;
        uVar7 = uVar2 | uVar8 & 0x7fffffff;
        uVar6 = 0;
        if (uVar7 != 0) {
          dVar28 = -NAN;
          iVar5 = 0x1c;
          goto LAB_100adcb0;
        }
LAB_100ad9ca:
        dVar29 = dVar10;
        if ((uVar7 & 0x7fffffff) == 0) {
          if ((param_3 & 0x80000000) == 0) {
            if ((uVar7 & uVar6 << 0xd) == 0) {
              return (float10)0;
            }
LAB_100adac9:
            return (float10)-0.0;
          }
          dVar28 = (double)((ulonglong)(uVar7 & uVar6 << 0xd | 0x7ff00000) << 0x20);
          iVar5 = 0x1b;
          goto LAB_100adcb0;
        }
      }
      else {
        uVar6 = 0;
LAB_100ad944:
        dVar32 = 2.225073858507201e-308;
        uVar24 = 0x3ff0000000000000;
        dVar10 = param_1 * 1.8446744073709552e+19;
        uVar7 = uVar8;
        dVar29 = dVar10;
        if (uVar2 == 0) goto LAB_100ad9ca;
      }
      dVar10 = (double)((ulonglong)ABS(dVar29) >> 0x2c);
      uVar7 = (SUB82(dVar10,0) & 0xff) + 1 & 0x1fe;
      dVar32 = (double)((ulonglong)dVar32 & (ulonglong)dVar29 | uVar24) *
               *(double *)((int)&DAT_100d0960 + uVar7 * 4);
      dVar28 = *(double *)((int)&DAT_100d0960 + uVar7 * 4);
      dVar11 = *(double *)(&DAT_100d0d70 + uVar7 * 8);
      dVar30 = *(double *)(&UNK_100d0d78 + uVar7 * 8);
      uVar7 = 0x43e7f;
      goto LAB_100ad526;
    }
    dVar28 = param_1;
    if ((0x7fefffff < (uVar8 & 0x7fffffff)) && ((0x7ff00000 < (uVar8 & 0x7fffffff) || (uVar2 != 0)))
       ) {
LAB_100adb0f:
      dVar28 = param_1 + param_1;
      iVar5 = 0x3ee;
      goto LAB_100adcb0;
    }
    goto LAB_100adbc5;
  }
  uVar6 = 0;
  uVar7 = 0x3fe7f;
LAB_100ad526:
  uVar3 = ((ushort)((ulonglong)dVar32 >> 0x26) & 0xff) + 1 & 0x1fe;
  dVar26 = (double)((ulonglong)dVar29 & 0xfffffffffffff | DAT_100d4200);
  dVar12 = (double)((ulonglong)dVar26 & 0xfffffffff8000000);
  dVar26 = dVar26 - dVar12;
  uVar4 = ((ushort)((ulonglong)(dVar32 * *(double *)(&DAT_100d1580 + uVar3 * 4)) >> 0x1f) & 0x1ff) +
          1 & 0x3fe;
  dVar27 = dVar28 * *(double *)(&DAT_100d1580 + uVar3 * 4) * *(double *)(&DAT_100d21a0 + uVar4 * 4);
  dVar32 = dVar32 * *(double *)(&DAT_100d1580 + uVar3 * 4) * *(double *)(&DAT_100d21a0 + uVar4 * 4);
  dVar29 = dVar11 + *(double *)(&DAT_100d1990 + uVar3 * 8) +
           (double)(int)((longlong)dVar10 - (ulonglong)uVar7 >> 8) +
           *(double *)(&DAT_100d29b0 + uVar4 * 8);
  dVar31 = dVar30 + *(double *)(&DAT_100d1998 + uVar3 * 8) + *(double *)(&DAT_100d29b8 + uVar4 * 8);
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
          goto LAB_100adbc5;
        }
      }
      else {
LAB_100adbc5:
        uVar24 = CONCAT44(param_3,param_2) & 0xfffffffffffff;
        auVar16._0_4_ = -(uint)((int)uVar24 == 0);
        auVar16._4_4_ = -(uint)((int)(uVar24 >> 0x20) == 0);
        auVar16._8_4_ = -(uint)((int)(in_XMM3_Qa & in_XMM2_Qa) == 0);
        auVar16._12_4_ = -(uint)((int)((in_XMM3_Qa & in_XMM2_Qa) >> 0x20) == 0);
        if ((byte)(SUB161(auVar16 >> 7,0) & 1 | (SUB161(auVar16 >> 0xf,0) & 1) << 1 |
                   (SUB161(auVar16 >> 0x17,0) & 1) << 2 | (SUB161(auVar16 >> 0x1f,0) & 1) << 3 |
                   (SUB161(auVar16 >> 0x27,0) & 1) << 4 | (SUB161(auVar16 >> 0x2f,0) & 1) << 5 |
                   (SUB161(auVar16 >> 0x37,0) & 1) << 6 | SUB161(auVar16 >> 0x3f,0) << 7) != 0xff)
        goto LAB_100adc3e;
        if (dVar28 != -1.0) {
          if ((param_3 & 0x80000000) == 0) {
            if ((uVar9 & 0x7ff0) < 0x3ff0) {
              return (float10)0;
            }
          }
          else if (0x3fef < (uVar9 & 0x7ff0)) {
            return (float10)0;
          }
LAB_100adc37:
          return (float10)INFINITY;
        }
      }
      dVar28 = 1.0;
      iVar5 = 0x1c;
    }
    else {
LAB_100adc3e:
      dVar28 = (double)CONCAT44(param_3,param_2) + (double)CONCAT44(param_3,param_2);
      iVar5 = 0x3ee;
    }
    goto LAB_100adcb0;
  }
  iVar5 = (uVar7 - 0x3ff0) + (uVar3 & 0x7ff0);
  if ((0x40a0U - iVar5 | iVar5 - 0x3c70U) < 0x80000000) {
LAB_100ad678:
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
      dVar29 = *(double *)(&DAT_100d39e0 + iVar5) * auVar13._0_8_;
      dVar28 = (dVar30 - ((dVar30 + 6755399441055744.0) - 6755399441055744.0)) * 0.0078125 +
               dVar28 + ((dVar32 * 0.16015105075297303 + dVar32 * dVar32 * -0.08325619496072671) *
                         dVar32 * dVar32 +
                         dVar32 * 9.597935033233511e-08 + dVar32 * dVar32 * -0.3465736568077919 +
                        dVar31 + dVar11) * (double)CONCAT44(param_3,param_2);
      return (float10)(dVar28 * dVar28 * dVar29 *
                       (dVar28 * 0.0013333558146428443 + 0.009618129107628477) * dVar28 * dVar28 +
                       *(double *)(&UNK_100d39e8 + iVar5) * auVar13._8_8_ +
                       (dVar28 * 0.055504108664821576 + 0.2402265069591007) * dVar28 * dVar28 *
                       dVar29 + dVar28 * 0.6931471805599453 * dVar29 + dVar29);
    }
    if ((int)uVar2 < 1) {
      if ((int)uVar2 < -0x3fdff) {
LAB_100adf09:
        dVar28 = (double)((ulonglong)uVar6 << 0x2d);
        iVar5 = 0x19;
        goto LAB_100adcb0;
      }
      uVar6 = uVar6 + 0x80;
      uVar8 = (uVar2 & 0xffffff80) + 0x3fe80;
      uVar9 = 0;
    }
    else {
      if (0x3ffff < uVar2) goto LAB_100adf2c;
      uVar6 = uVar6 + 0x3ff00;
      uVar8 = uVar2 - 0x80 & 0xffffff80;
      uVar9 = 0x3ff0;
    }
    iVar5 = (uVar2 & 0x7f) * 0x10;
    uVar2 = ((int)-(uVar8 - 0x1ff80) >> 7) + 2;
    auVar17._0_8_ = (double)((ulonglong)uVar8 << 0x2d);
    auVar17._8_4_ = 0;
    auVar17._12_4_ = (int)((ulonglong)auVar17._0_8_ >> 0x20);
    dVar10 = *(double *)(&DAT_100d39e0 + iVar5) * auVar17._0_8_;
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
             *(double *)(&UNK_100d39e8 + iVar5) * auVar17._8_8_ +
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
    if ((0x40a0 - uVar2 | uVar8) < 0x80000000) goto LAB_100ad678;
    if (0x7fffffff < uVar8) {
      return (float10)(double)((ulonglong)(uVar6 | 0x1ff80) << 0x2d);
    }
    if (((uVar18 ^ (uVar9 & 0x7ff0) + 0xc010) & 0x8000) != 0) goto LAB_100adf09;
LAB_100adf2c:
    iVar5 = 0x18;
    if (uVar6 == 0) {
      dVar28 = INFINITY;
    }
    else {
      dVar28 = -INFINITY;
    }
  }
LAB_100adcb0:
  ___libm_error_support(&param_1,(undefined8 *)&param_2,(double *)&stack0xfffffff4,iVar5);
  return (float10)dVar28;
}


// 100ae1f7 __d_inttype

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


// 100ae25b FUN_100ae25b

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __cdecl FUN_100ae25b(int param_1,int param_2,int param_3,int param_4,double *param_5)

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
  dVar3 = _DAT_100dfa50;
  if (param_4 == 0x7ff00000) {
    if (param_3 == 0) {
      if ((dVar1 <= 1.0) && (dVar3 = 1.0, dVar1 < 1.0)) {
        dVar3 = dVar2;
      }
      goto LAB_100ae388;
    }
  }
  else if ((param_4 == -0x100000) && (param_3 == 0)) {
    dVar3 = dVar2;
    if ((dVar1 <= 1.0) && (dVar3 = _DAT_100dfa50, 1.0 <= dVar1)) {
      *param_5 = _DAT_100dfa58;
      return 1;
    }
    goto LAB_100ae388;
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
          dVar3 = _DAT_100dfa70;
        }
      }
    }
    else {
      dVar3 = _DAT_100dfa50;
      if (iVar4 == 1) {
        dVar3 = -_DAT_100dfa50;
      }
    }
  }
LAB_100ae388:
  *param_5 = dVar3;
  return 0;
}


// 100af075 __ioinit

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
  
  uStack_c = 0x100af081;
  local_8 = 0;
  GetStartupInfoA(&local_68);
  local_8 = 0xfffffffe;
  puVar2 = (undefined4 *)FUN_10002e20(0x20,0x38);
  if (puVar2 == (undefined4 *)0x0) {
LAB_100af2ac:
    iVar7 = -1;
  }
  else {
    DAT_100f5ce8 = 0x20;
    DAT_100f5d00 = puVar2;
    for (; puVar2 < DAT_100f5d00 + 0x1c0; puVar2 = puVar2 + 0xe) {
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
      while ((UVar10 = UVar9, (int)DAT_100f5ce8 < (int)UVar9 &&
             (puVar2 = (undefined4 *)FUN_10002e20(0x20,0x38), UVar10 = DAT_100f5ce8,
             puVar2 != (undefined4 *)0x0))) {
        (&DAT_100f5d00)[iVar7] = puVar2;
        DAT_100f5ce8 = DAT_100f5ce8 + 0x20;
        puVar1 = puVar2;
        for (; puVar2 < puVar1 + 0x1c0; puVar2 = puVar2 + 0xe) {
          *(undefined1 *)(puVar2 + 1) = 0;
          *puVar2 = 0xffffffff;
          *(undefined1 *)((int)puVar2 + 5) = 10;
          puVar2[2] = 0;
          *(byte *)(puVar2 + 9) = *(byte *)(puVar2 + 9) & 0x80;
          *(undefined1 *)((int)puVar2 + 0x25) = 10;
          *(undefined1 *)((int)puVar2 + 0x26) = 10;
          puVar1 = (&DAT_100f5d00)[iVar7];
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
                     ((local_24 & 0x1f) * 0x38 + (int)(&DAT_100f5d00)[(int)local_24 >> 5]);
            *puVar2 = *(undefined4 *)local_20;
            *(byte *)(puVar2 + 1) = (byte)*pUVar6;
            iVar7 = ___crtInitCritSecAndSpinCount(puVar2 + 3,4000);
            if (iVar7 == 0) goto LAB_100af2ac;
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
      piVar8 = DAT_100f5d00 + iVar7 * 0xe;
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
          if (iVar5 == 0) goto LAB_100af2ac;
          piVar8[2] = piVar8[2] + 1;
        }
      }
      else {
        *(byte *)(piVar8 + 1) = *(byte *)(piVar8 + 1) | 0x80;
      }
      iVar7 = iVar7 + 1;
    } while (iVar7 < 3);
    SetHandleCount(DAT_100f5ce8);
    iVar7 = 0;
  }
  return iVar7;
}


// 100af2b5 __ioterm

/* Library Function - Single Match
    __ioterm
   
   Library: Visual Studio 2005 Release */

void __cdecl __ioterm(void)

{
  uint uVar1;
  uint *puVar2;
  uint uVar3;
  
  puVar2 = &DAT_100f5d00;
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
      FUN_10002ea0(*puVar2);
      *puVar2 = 0;
    }
    puVar2 = puVar2 + 1;
  } while ((int)puVar2 < 0x100f5e00);
  return;
}


// 100af301 __setenvp

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
  
  if (DAT_100f5e18 == 0) {
    ___initmbctable();
  }
  iVar5 = 0;
  pcVar4 = DAT_100f51d8;
  if (DAT_100f51d8 != (char *)0x0) {
    for (; *pcVar4 != '\0'; pcVar4 = pcVar4 + sVar2 + 1) {
      if (*pcVar4 != '=') {
        iVar5 = iVar5 + 1;
      }
      sVar2 = _strlen(pcVar4);
    }
    puVar1 = (undefined4 *)FUN_10002e20(iVar5 + 1,4);
    pcVar4 = DAT_100f51d8;
    DAT_100f5214 = puVar1;
    if (puVar1 != (undefined4 *)0x0) {
      do {
        if (*pcVar4 == '\0') {
          FUN_10002ea0(DAT_100f51d8);
          DAT_100f51d8 = (char *)0x0;
          *puVar1 = 0;
          DAT_100f5e0c = 1;
          return 0;
        }
        sVar2 = _strlen(pcVar4);
        _SizeInBytes = sVar2 + 1;
        if (*pcVar4 != '=') {
          _Dst = (char *)FUN_10002e20(_SizeInBytes,1);
          *puVar1 = _Dst;
          if (_Dst == (char *)0x0) {
            FUN_10002ea0(DAT_100f5214);
            DAT_100f5214 = (undefined4 *)0x0;
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


// 100af3dc parse_cmdline

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
        goto LAB_100af46e;
      }
    }
    in_EDX = pbVar7;
  } while ((bVar2) || ((bVar6 != 0x20 && (bVar6 != 9))));
  if (pbVar5 != (byte *)0x0) {
    pbVar5[-1] = 0;
  }
LAB_100af46e:
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


// 100af574 __setargv

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
  
  if (DAT_100f5e18 == 0) {
    ___initmbctable();
  }
  DAT_100f56b4 = 0;
  GetModuleFileNameA((HMODULE)0x0,&DAT_100f55b0,0x104);
  _DAT_100f5224 = &DAT_100f55b0;
  if ((DAT_100f6e44 == (char *)0x0) || (local_8 = DAT_100f6e44, *DAT_100f6e44 == '\0')) {
    local_8 = &DAT_100f55b0;
  }
  parse_cmdline((undefined4 *)0x0,(byte *)0x0,(int *)&local_c);
  uVar2 = local_c;
  if ((local_c < 0x3fffffff) && (local_10 != 0xffffffff)) {
    uVar1 = local_c * 4 + local_10;
    if ((local_10 <= uVar1) &&
       (puVar3 = (undefined4 *)FUN_10002dc0(uVar1), puVar3 != (undefined4 *)0x0)) {
      parse_cmdline(puVar3,(byte *)(puVar3 + uVar2),(int *)&local_c);
      _DAT_100f5208 = local_c - 1;
      _DAT_100f520c = puVar3;
      return 0;
    }
  }
  return -1;
}


// 100af62d ___crtGetEnvironmentStringsA

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
  if (DAT_100f56b8 == 0) {
    lpWideCharStr = GetEnvironmentStringsW();
    if (lpWideCharStr != (LPWCH)0x0) {
      DAT_100f56b8 = 1;
      goto LAB_100af67f;
    }
    DVar3 = GetLastError();
    if (DVar3 == 0x78) {
      DAT_100f56b8 = 2;
    }
  }
  if (DAT_100f56b8 != 1) {
    if ((DAT_100f56b8 != 2) && (DAT_100f56b8 != 0)) {
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
    _Dst = (void *)FUN_10002dc0(pcVar7 + (1 - (int)_Src));
    if (_Dst == (void *)0x0) {
      FreeEnvironmentStringsA(_Src);
      return (LPVOID)0x0;
    }
    _memcpy(_Dst,_Src,(size_t)(pcVar7 + (1 - (int)_Src)));
    FreeEnvironmentStringsA(_Src);
    return _Dst;
  }
LAB_100af67f:
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
      (lpMultiByteStr = (LPSTR)FUN_10002dc0(cbMultiByte), lpMultiByteStr != (LPSTR)0x0)) &&
     (iVar6 = WideCharToMultiByte(0,0,lpWideCharStr,iVar6,lpMultiByteStr,cbMultiByte,(LPCSTR)0x0,
                                  (LPBOOL)0x0), local_8 = lpMultiByteStr, iVar6 == 0)) {
    FUN_10002ea0(lpMultiByteStr);
    local_8 = (LPSTR)0x0;
  }
  FreeEnvironmentStringsW(lpWideCharStr);
  return local_8;
}


// 100af762 __RTC_Initialize

/* WARNING: Removing unreachable block (ram,0x100af774) */
/* WARNING: Removing unreachable block (ram,0x100af77a) */
/* WARNING: Removing unreachable block (ram,0x100af77c) */
/* Library Function - Single Match
    __RTC_Initialize
   
   Library: Visual Studio 2005 Release */

void __RTC_Initialize(void)

{
  return;
}


// 100af7aa FUN_100af7aa

void FUN_100af7aa(void)

{
  return;
}


// 100af7ab FUN_100af7ab

DWORD * __cdecl FUN_100af7ab(int param_1,DWORD param_2)

{
  int *piVar1;
  code *pcVar2;
  DWORD DVar3;
  DWORD DVar4;
  DWORD *pDVar5;
  int *piVar6;
  int iVar7;
  int iVar8;
  
  pDVar5 = FUN_100a9f8d();
  if (pDVar5 != (DWORD *)0x0) {
    piVar1 = (int *)pDVar5[0x17];
    piVar6 = piVar1;
    do {
      if (*piVar6 == param_1) break;
      piVar6 = piVar6 + 3;
    } while (piVar6 < piVar1 + DAT_100df79c * 3);
    if ((piVar1 + DAT_100df79c * 3 <= piVar6) || (*piVar6 != param_1)) {
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
          if (DAT_100df790 < DAT_100df794 + DAT_100df790) {
            iVar7 = DAT_100df790 * 0xc;
            iVar8 = DAT_100df790;
            do {
              *(undefined4 *)(iVar7 + 8 + pDVar5[0x17]) = 0;
              iVar8 = iVar8 + 1;
              iVar7 = iVar7 + 0xc;
            } while (iVar8 < DAT_100df794 + DAT_100df790);
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


// 100af924 FUN_100af924

undefined4 FUN_100af924(void)

{
  return 1;
}


// 100af92a ___security_init_cookie

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
  if ((DAT_100df7a0 == 0xbb40e64e) || ((DAT_100df7a0 & 0xffff0000) == 0)) {
    GetSystemTimeAsFileTime(&local_c);
    uVar4 = local_c.dwHighDateTime ^ local_c.dwLowDateTime;
    DVar1 = GetCurrentProcessId();
    DVar2 = GetCurrentThreadId();
    DVar3 = GetTickCount();
    QueryPerformanceCounter(&local_14);
    DAT_100df7a0 = uVar4 ^ DVar1 ^ DVar2 ^ DVar3 ^ local_14.s.HighPart ^ local_14.s.LowPart;
    if (DAT_100df7a0 == 0xbb40e64e) {
      DAT_100df7a0 = 0xbb40e64f;
    }
    else if ((DAT_100df7a0 & 0xffff0000) == 0) {
      DAT_100df7a0 = DAT_100df7a0 | DAT_100df7a0 << 0x10;
    }
    DAT_100df7a4 = ~DAT_100df7a0;
  }
  else {
    DAT_100df7a4 = ~DAT_100df7a0;
  }
  return;
}


// 100af9be __security_check_cookie

/* Library Function - Single Match
    @__security_check_cookie@4
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release
   __fastcall __security_check_cookie,4 */

void __fastcall __security_check_cookie(int param_1)

{
  if (param_1 == DAT_100df7a0) {
    return;
  }
                    /* WARNING: Subroutine does not return */
  ___report_gsfailure();
}


// 100af9cd __mtinitlocks

/* Library Function - Single Match
    __mtinitlocks
   
   Library: Visual Studio 2005 Release */

int __cdecl __mtinitlocks(void)

{
  int iVar1;
  int iVar2;
  undefined *puVar3;
  
  iVar2 = 0;
  puVar3 = &DAT_100f56c0;
  do {
    if ((&DAT_100df7ac)[iVar2 * 2] == 1) {
      (&DAT_100df7a8)[iVar2 * 2] = puVar3;
      puVar3 = puVar3 + 0x18;
      iVar1 = ___crtInitCritSecAndSpinCount((&DAT_100df7a8)[iVar2 * 2],4000);
      if (iVar1 == 0) {
        (&DAT_100df7a8)[iVar2 * 2] = 0;
        return 0;
      }
    }
    iVar2 = iVar2 + 1;
  } while (iVar2 < 0x24);
  return 1;
}


// 100afa16 __mtdeletelocks

/* Library Function - Single Match
    __mtdeletelocks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __cdecl __mtdeletelocks(void)

{
  LPCRITICAL_SECTION lpCriticalSection;
  undefined4 *puVar1;
  
  puVar1 = &DAT_100df7a8;
  do {
    lpCriticalSection = (LPCRITICAL_SECTION)*puVar1;
    if ((lpCriticalSection != (LPCRITICAL_SECTION)0x0) && (puVar1[1] != 1)) {
      DeleteCriticalSection(lpCriticalSection);
      FUN_10002ea0(lpCriticalSection);
      *puVar1 = 0;
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x100df8c8);
  puVar1 = &DAT_100df7a8;
  do {
    if (((LPCRITICAL_SECTION)*puVar1 != (LPCRITICAL_SECTION)0x0) && (puVar1[1] == 1)) {
      DeleteCriticalSection((LPCRITICAL_SECTION)*puVar1);
    }
    puVar1 = puVar1 + 2;
  } while ((int)puVar1 < 0x100df8c8);
  return;
}


// 100afa6b FUN_100afa6b

void __cdecl FUN_100afa6b(int param_1)

{
  LeaveCriticalSection((LPCRITICAL_SECTION)(&DAT_100df7a8)[param_1 * 2]);
  return;
}


// 100afa80 __mtinitlocknum

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
  if (DAT_100d9074 == 0) {
    __FF_MSGBANNER();
    FUN_100a9b2c(0x1e);
    ___crtExitProcess(0xff);
  }
  piVar1 = &DAT_100df7a8 + _LockNum * 2;
  if (*piVar1 == 0) {
    iVar4 = FUN_10002dc0(0x18);
    if (iVar4 == 0) {
      pDVar2 = FUN_100ab320();
      *pDVar2 = 0xc;
      iVar4 = 0;
    }
    else {
      __lock(10);
      if (*piVar1 == 0) {
        iVar3 = ___crtInitCritSecAndSpinCount(iVar4,4000);
        if (iVar3 == 0) {
          FUN_10002ea0(iVar4);
          pDVar2 = FUN_100ab320();
          *pDVar2 = 0xc;
          local_20 = 0;
        }
        else {
          *piVar1 = iVar4;
        }
      }
      else {
        FUN_10002ea0(iVar4);
      }
      FUN_100afb3a();
      iVar4 = local_20;
    }
  }
  return iVar4;
}


// 100afb3a FUN_100afb3a

void FUN_100afb3a(void)

{
  FUN_100afa6b(10);
  return;
}


// 100afb43 __lock

/* Library Function - Single Match
    __lock
   
   Library: Visual Studio 2005 Release */

void __cdecl __lock(int _File)

{
  int iVar1;
  
  if ((&DAT_100df7a8)[_File * 2] == 0) {
    iVar1 = __mtinitlocknum(_File);
    if (iVar1 == 0) {
      __amsg_exit(0x11);
    }
  }
  EnterCriticalSection((LPCRITICAL_SECTION)(&DAT_100df7a8)[_File * 2]);
  return;
}


// 100afb74 type_info::_Type_info_dtor

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
  piVar1 = DAT_100f5814;
  if (*(int *)(param_1 + 4) != 0) {
    piVar2 = (int *)&DAT_100f5810;
    do {
      piVar3 = piVar2;
      if (DAT_100f5814 == (int *)0x0) goto LAB_100afbb8;
      piVar2 = DAT_100f5814;
    } while (*DAT_100f5814 != *(int *)(param_1 + 4));
    piVar3[1] = DAT_100f5814[1];
    FUN_10002ea0(piVar1);
LAB_100afbb8:
    FUN_10002ea0(*(undefined4 *)(param_1 + 4));
    *(undefined4 *)(param_1 + 4) = 0;
  }
  FUN_100afbdb();
  return;
}


// 100afbdb FUN_100afbdb

void FUN_100afbdb(void)

{
  FUN_100afa6b(0xe);
  return;
}


// 100afbf0 _strcmp

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
      if (bVar4 != *_Str2) goto LAB_100afc34;
      _Str2 = _Str2 + 1;
      if (bVar4 == 0) {
        return 0;
      }
      if (((uint)_Str1 & 2) == 0) goto LAB_100afc00;
    }
    uVar1 = *(undefined2 *)_Str1;
    _Str1 = _Str1 + 2;
    bVar4 = (byte)uVar1;
    bVar5 = bVar4 < (byte)*_Str2;
    if (bVar4 != *_Str2) goto LAB_100afc34;
    if (bVar4 == 0) {
      return 0;
    }
    bVar4 = (byte)((ushort)uVar1 >> 8);
    bVar5 = bVar4 < ((byte *)_Str2)[1];
    if (bVar4 != ((byte *)_Str2)[1]) goto LAB_100afc34;
    if (bVar4 == 0) {
      return 0;
    }
    _Str2 = (char *)((byte *)_Str2 + 2);
  }
LAB_100afc00:
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
LAB_100afc34:
  return (uint)bVar5 * -2 + 1;
}


// 100afc80 FUN_100afc80

void FUN_100afc80(void)

{
  float10 in_ST0;
  
  FUN_100afc9e((double)in_ST0);
  return;
}


// 100afc9e FUN_100afc9e

float10 __cdecl FUN_100afc9e(double param_1)

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
    dVar1 = dVar9 * *(double *)(&DAT_100d51b0 + iVar7 * 2) - dVar12;
    uVar5 = SUB164(auVar10 | ZEXT816(0x200000000000),4) & 0x80000000;
    dVar2 = dVar1 * dVar1;
    auVar4._4_8_ = 0;
    auVar4._0_4_ = uVar5;
    auVar11._0_12_ = auVar4 << 0x20;
    auVar11._12_4_ = uVar5;
    return (float10)(((dVar9 - dVar13) * (dVar9 + dVar13)) /
                     (dVar9 * *(double *)(&DAT_100d51b0 + iVar7 * 2) + dVar12) +
                     (dVar2 * 0.044642857142857144 + 0.075) * dVar1 * dVar2 * dVar2 +
                     dVar1 * dVar2 * 0.16666666666666666 +
                     (double)(*(ulonglong *)(&DAT_100d42b0 + iVar8) ^ (ulonglong)uVar5 << 0x20) +
                    (double)(*(ulonglong *)(&UNK_100d42b8 + iVar8) ^ auVar11._8_8_));
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
    dVar13 = dVar13 * *(double *)(&DAT_100d51b0 + iVar8 * 8);
    dVar14 = (dVar2 * dVar12 - dVar13) + dVar1 * dVar12;
    iVar8 = iVar8 * 0x10;
    dVar15 = dVar14 * dVar14;
    return (float10)(double)((ulonglong)
                             ((((dVar15 * 0.044642857142857144 + 0.075) * dVar14 * dVar15 * dVar15 +
                               dVar14 * dVar15 * 0.16666666666666666 +
                               (6.123233995736766e-17 - *(double *)(&DAT_100d42b0 + iVar8))) -
                              (((1.0 - dVar2 * dVar2) - dVar12 * dVar12) -
                              (ABS(dVar9) + dVar2) * dVar1) / (dVar13 + dVar13 + dVar14)) +
                             (1.5707963267948966 - *(double *)(&UNK_100d42b8 + iVar8))) |
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


// 100b0200 ___libm_error_support

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
  if (DAT_100f5818 == 0) {
    pcVar1 = FUN_100b6831;
  }
  else {
    pcVar1 = (code *)FUN_100a9de8(DAT_100f5ce4);
  }
  if (param_4 < 0xa7) {
    if (param_4 == 0xa6) {
      local_2c = 3;
      local_28 = "exp10";
LAB_100b02af:
      local_24 = *param_1;
      local_1c = *param_2;
      local_14 = *param_3;
      iVar2 = (*pcVar1)(&local_2c);
      if (iVar2 == 0) {
        pDVar3 = FUN_100ab320();
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
            goto LAB_100b031d;
          }
          if (param_4 == 8) {
            local_28 = "log10";
          }
          else {
            if (param_4 == 9) {
              local_28 = "log10";
              goto LAB_100b031d;
            }
            if (param_4 != 0xe) {
              if (param_4 != 0xf) {
                if (param_4 != 0x18) {
                  return;
                }
                local_2c = 3;
                goto LAB_100b02a8;
              }
              local_28 = "exp";
              goto LAB_100b02e4;
            }
            local_2c = 3;
            local_28 = "exp";
          }
        }
        goto LAB_100b02af;
      }
      local_28 = "pow";
LAB_100b02e4:
      local_24 = *param_1;
      local_1c = *param_2;
      local_2c = 4;
      local_14 = *param_3;
      (*pcVar1)(&local_2c);
    }
    else {
      if (param_4 != 0x1a) {
        if (param_4 != 0x1b) {
          if (param_4 == 0x1c) goto switchD_100b03da_caseD_3ee;
          if (param_4 != 0x1d) {
            if (param_4 != 0x3a) {
              if (param_4 != 0x3d) {
                return;
              }
              goto switchD_100b03da_caseD_3f1;
            }
            goto switchD_100b03da_caseD_3f0;
          }
          local_28 = "pow";
          goto LAB_100b0393;
        }
        local_2c = 2;
LAB_100b02a8:
        local_28 = "pow";
        goto LAB_100b02af;
      }
      local_14 = 1.0;
    }
    goto LAB_100b0473;
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
switchD_100b03da_caseD_3ee:
    local_28 = "pow";
    goto LAB_100b031d;
  case 0x3ef:
    local_28 = "modf";
    break;
  case 0x3f0:
switchD_100b03da_caseD_3f0:
    local_28 = "acos";
    goto LAB_100b031d;
  case 0x3f1:
switchD_100b03da_caseD_3f1:
    local_28 = "asin";
    goto LAB_100b031d;
  case 0x3f2:
    local_28 = "sin";
    goto LAB_100b043f;
  case 0x3f3:
    local_28 = "cos";
    goto LAB_100b043f;
  case 0x3f4:
    local_28 = "tan";
LAB_100b043f:
    local_14 = *param_1 *
               (double)CONCAT17(uStack_5,CONCAT16(uStack_6,CONCAT15(uStack_7,CONCAT14(uStack_8,
                                                  CONCAT13(uStack_9,CONCAT12(uStack_a,CONCAT11(
                                                  uStack_b,local_c)))))));
    *param_3 = local_14;
    local_24 = *param_1;
    local_1c = *param_2;
    goto LAB_100b0450;
  default:
    goto switchD_100b03da_default;
  }
LAB_100b0393:
  *param_3 = *param_1;
LAB_100b031d:
  local_24 = *param_1;
  local_1c = *param_2;
  local_14 = *param_3;
LAB_100b0450:
  local_2c = 1;
  iVar2 = (*pcVar1)(&local_2c);
  if (iVar2 == 0) {
    pDVar3 = FUN_100ab320();
    *pDVar3 = 0x21;
  }
LAB_100b0473:
  *param_3 = local_14;
switchD_100b03da_default:
  return;
}


// 100b0590 __cintrindisp2

/* Library Function - Single Match
    __cintrindisp2
   
   Library: Visual Studio */

void __fastcall __cintrindisp2(undefined4 param_1,int param_2)

{
  __trandisp2(param_1,param_2);
  FUN_100b065a();
  return;
}


// 100b05ce __cintrindisp1

/* Library Function - Single Match
    __cintrindisp1
   
   Library: Visual Studio */

void __fastcall __cintrindisp1(undefined4 param_1,int param_2)

{
  __trandisp1(param_1,param_2);
  FUN_100b065a();
  return;
}


// 100b060b __ctrandisp2

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
  FUN_100b0653();
  return;
}


// 100b0653 FUN_100b0653

void FUN_100b0653(void)

{
  char cVar1;
  ushort uVar2;
  int unaff_EBP;
  ushort in_FPUStatusWord;
  float10 in_ST0;
  
  *(byte *)(unaff_EBP + -0x2c8) = *(byte *)(unaff_EBP + -0x2c8) & 0xfe;
  if (DAT_100f51cc != 0) {
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
        goto LAB_100b073f;
      }
      uVar2 = *(ushort *)(unaff_EBP + -0x2ca) & 0x7ff0;
      if (uVar2 == 0) {
        *(undefined4 *)(unaff_EBP + -0x8e) = 4;
        in_ST0 = (float10)fscale(in_ST0,(float10)1536.0);
        if (ABS(in_ST0) < (float10)2.2250738585072014e-308) {
          in_ST0 = in_ST0 * (float10)0.0;
        }
        goto LAB_100b073f;
      }
    }
    if (uVar2 == 0x7ff0) {
      *(undefined4 *)(unaff_EBP + -0x8e) = 3;
      in_ST0 = (float10)fscale(in_ST0,(float10)-1536.0);
      if ((float10)1.7976931348623157e+308 < ABS(in_ST0)) {
        in_ST0 = in_ST0 * (float10)INFINITY;
      }
      goto LAB_100b073f;
    }
  }
  if ((*(ushort *)(unaff_EBP + -0xa4) & 0x20) != 0) {
    return;
  }
  if ((in_FPUStatusWord & 0x20) == 0) {
    return;
  }
  *(undefined4 *)(unaff_EBP + -0x8e) = 8;
LAB_100b073f:
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


// 100b065a FUN_100b065a

void FUN_100b065a(void)

{
  char cVar1;
  ushort uVar2;
  int unaff_EBP;
  ushort in_FPUStatusWord;
  float10 in_ST0;
  
  if (DAT_100f51cc != 0) {
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
        goto LAB_100b073f;
      }
      uVar2 = *(ushort *)(unaff_EBP + -0x2ca) & 0x7ff0;
      if (uVar2 == 0) {
        *(undefined4 *)(unaff_EBP + -0x8e) = 4;
        in_ST0 = (float10)fscale(in_ST0,(float10)1536.0);
        if (ABS(in_ST0) < (float10)2.2250738585072014e-308) {
          in_ST0 = in_ST0 * (float10)0.0;
        }
        goto LAB_100b073f;
      }
    }
    if (uVar2 == 0x7ff0) {
      *(undefined4 *)(unaff_EBP + -0x8e) = 3;
      in_ST0 = (float10)fscale(in_ST0,(float10)-1536.0);
      if ((float10)1.7976931348623157e+308 < ABS(in_ST0)) {
        in_ST0 = in_ST0 * (float10)INFINITY;
      }
      goto LAB_100b073f;
    }
  }
  if ((*(ushort *)(unaff_EBP + -0xa4) & 0x20) != 0) {
    return;
  }
  if ((in_FPUStatusWord & 0x20) == 0) {
    return;
  }
  *(undefined4 *)(unaff_EBP + -0x8e) = 8;
LAB_100b073f:
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


// 100b07a1 __ctrandisp1

/* Library Function - Single Match
    __ctrandisp1
   
   Library: Visual Studio */

void __cdecl __ctrandisp1(uint param_1,int param_2)

{
  undefined4 extraout_ECX;
  int extraout_EDX;
  
  __fload(param_1,param_2);
  __trandisp1(extraout_ECX,extraout_EDX);
  FUN_100b0653();
  return;
}


// 100b07d4 __fload

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


// 100b1016 FUN_100b1016

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 __cdecl FUN_100b1016(undefined4 param_1,uint param_2,ushort param_3)

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
       ((&DAT_100df90c)[(param_2._2_2_ & 0x7800) >> 0xb] != '\0')) && ((param_3 & 0x7fff) != 0x7fff)
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
        fVar3 = fVar3 * (float10)_DAT_100df93c;
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


// 100b121c FUN_100b121c

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

float10 FUN_100b121c(void)

{
  float10 in_ST0;
  float10 extraout_ST0;
  float10 extraout_ST0_00;
  float10 in_ST1;
  uint uVar2;
  float10 fVar1;
  
  uVar2 = (uint)((unkuint10)in_ST1 >> 0x20);
  if (((unkuint10)in_ST1 & 0) != 0) {
    FUN_100b1016(SUB104(in_ST1,0),uVar2,(ushort)((unkuint10)in_ST1 >> 0x40));
    return extraout_ST0;
  }
  if (SUB104(in_ST1,0) != 0 || uVar2 != 0) {
    fVar1 = in_ST1 * (float10)_DAT_100df924;
    FUN_100b1016(SUB104(fVar1,0),(uint)((unkuint10)fVar1 >> 0x20),(ushort)((unkuint10)fVar1 >> 0x40)
                );
    return extraout_ST0_00;
  }
  return in_ST0 - (in_ST0 / in_ST1) * in_ST1;
}


// 100b159b __stbuf

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __stbuf
   
   Library: Visual Studio 2005 Release */

int __cdecl __stbuf(FILE *_File)

{
  int *piVar1;
  char *pcVar2;
  int iVar3;
  undefined **ppuVar4;
  
  iVar3 = __fileno(_File);
  iVar3 = __isatty(iVar3);
  if (iVar3 == 0) {
    return 0;
  }
  ppuVar4 = FUN_100a7c8d();
  if (_File == (FILE *)(ppuVar4 + 8)) {
    iVar3 = 0;
  }
  else {
    ppuVar4 = FUN_100a7c8d();
    if (_File != (FILE *)(ppuVar4 + 0x10)) {
      return 0;
    }
    iVar3 = 1;
  }
  _DAT_100f51ec = _DAT_100f51ec + 1;
  if ((_File->_flag & 0x10c) != 0) {
    return 0;
  }
  piVar1 = &DAT_100f581c + iVar3;
  if (*piVar1 == 0) {
    iVar3 = FUN_10002dc0(0x1000);
    *piVar1 = iVar3;
    if (iVar3 == 0) {
      _File->_base = (char *)&_File->_charbuf;
      _File->_ptr = (char *)&_File->_charbuf;
      _File->_bufsiz = 2;
      _File->_cnt = 2;
      goto LAB_100b161f;
    }
  }
  pcVar2 = (char *)*piVar1;
  _File->_base = pcVar2;
  _File->_ptr = pcVar2;
  _File->_bufsiz = 0x1000;
  _File->_cnt = 0x1000;
LAB_100b161f:
  _File->_flag = _File->_flag | 0x1102;
  return 1;
}


// 100b1631 __ftbuf

/* Library Function - Single Match
    __ftbuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __ftbuf(int _Flag,FILE *_File)

{
  if ((_Flag != 0) && ((_File->_flag & 0x1000) != 0)) {
    __flush(_File);
    _File->_flag = _File->_flag & 0xffffeeff;
    _File->_bufsiz = 0;
    _File->_ptr = (char *)0x0;
    _File->_base = (char *)0x0;
  }
  return;
}


// 100b1660 __fileno

/* Library Function - Single Match
    __fileno
   
   Library: Visual Studio 2005 Release */

int __cdecl __fileno(FILE *_File)

{
  DWORD *pDVar1;
  
  if (_File == (FILE *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  return _File->_file;
}


// 100b168d __set_exp

/* Library Function - Single Match
    __set_exp
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release */

float10 __cdecl __set_exp(undefined8 param_1,short param_2)

{
  undefined8 local_c;
  
  local_c = (double)CONCAT26((param_2 + 0x3fe) * 0x10 | param_1._6_2_ & 0x800f,(int6)param_1);
  return (float10)local_c;
}


// 100b16b7 __sptype

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


// 100b1712 FUN_100b1712

void __cdecl FUN_100b1712(uint param_1,uint param_2,int *param_3)

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


// 100b17c1 __raise_exc_ex

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
        goto LAB_100b191e;
      }
      uVar3 = *param_1 & 0xfffffffe | 2;
    }
    *param_1 = uVar3;
  }
LAB_100b191e:
  uVar3 = *puVar1 & 0x300;
  if (uVar3 == 0) {
    uVar3 = *param_1 & 0xffffffeb | 8;
LAB_100b1954:
    *param_1 = uVar3;
  }
  else {
    if (uVar3 == 0x200) {
      uVar3 = *param_1 & 0xffffffe7 | 4;
      goto LAB_100b1954;
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
        goto LAB_100b1a59;
      }
      uVar3 = *puVar1 & 0xfffffbff | 0x800;
    }
    *puVar1 = uVar3;
  }
LAB_100b1a59:
  uVar3 = *param_1 >> 2 & 7;
  if (uVar3 == 0) {
    uVar3 = *puVar1 & 0xfffff3ff | 0x300;
  }
  else {
    if (uVar3 != 1) {
      if (uVar3 == 2) {
        *puVar1 = *puVar1 & 0xfffff3ff;
      }
      goto LAB_100b1a85;
    }
    uVar3 = *puVar1 & 0xfffff3ff | 0x200;
  }
  *puVar1 = uVar3;
LAB_100b1a85:
  if (param_7 == 0) {
    *(undefined8 *)puVar2 = *(undefined8 *)(param_1 + 0x14);
  }
  else {
    *puVar2 = param_1[0x14];
  }
  return;
}


// 100b1a9b __raise_exc

/* Library Function - Single Match
    __raise_exc
   
   Library: Visual Studio 2005 Release */

void __cdecl
__raise_exc(uint *param_1,uint *param_2,uint param_3,int param_4,uint *param_5,uint *param_6)

{
  __raise_exc_ex(param_1,param_2,param_3,param_4,param_5,param_6,0);
  return;
}


// 100b1abc __handle_exc

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
    FUN_100b1edb();
    uVar6 = param_1 & 0x17;
    goto LAB_100b1c7b;
  }
  if (((param_1 & 4) != 0) && ((param_3 & 4) != 0)) {
    FUN_100b1edb();
    uVar6 = param_1 & 0x1b;
    goto LAB_100b1c7b;
  }
  if (((param_1 & 1) == 0) || ((param_3 & 8) == 0)) {
    if (((param_1 & 2) != 0) && ((param_3 & 0x10) != 0)) {
      bVar7 = (param_1 & 0x10) != 0;
      if (*param_2 != 0.0) {
        fVar8 = (float10)FUN_100b1712(SUB84(*param_2,0),(uint)((ulonglong)*param_2 >> 0x20),&local_c
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
        FUN_100b1edb();
      }
      uVar6 = local_8 & 0xfffffffd;
      local_8 = uVar6;
    }
    goto LAB_100b1c7b;
  }
  FUN_100b1edb();
  uVar6 = param_3 & 0xc00;
  dVar2 = _DAT_100dfa50;
  dVar5 = _DAT_100dfa50;
  if (uVar6 == 0) {
    dVar1 = *param_2;
joined_r0x100b1b5e:
    if (dVar1 <= 0.0) {
      dVar2 = -dVar5;
    }
    *param_2 = dVar2;
  }
  else {
    if (uVar6 == 0x400) {
      dVar1 = *param_2;
      dVar2 = _DAT_100dfa60;
      goto joined_r0x100b1b5e;
    }
    dVar5 = _DAT_100dfa60;
    if (uVar6 == 0x800) {
      dVar1 = *param_2;
      goto joined_r0x100b1b5e;
    }
    if (uVar6 == 0xc00) {
      dVar1 = *param_2;
      dVar2 = _DAT_100dfa60;
      goto joined_r0x100b1b5e;
    }
  }
  uVar6 = param_1 & 0x1e;
LAB_100b1c7b:
  if (((param_1 & 0x10) != 0) && ((param_3 & 0x20) != 0)) {
    FUN_100b1edb();
    uVar6 = uVar6 & 0xffffffef;
  }
  return uVar6 == 0;
}


// 100b1c9d __set_errno_from_matherr

/* Library Function - Single Match
    __set_errno_from_matherr
   
   Library: Visual Studio 2005 Release */

void __cdecl __set_errno_from_matherr(int param_1)

{
  DWORD *pDVar1;
  
  if (param_1 == 1) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x21;
  }
  else if ((1 < param_1) && (param_1 < 4)) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x22;
    return;
  }
  return;
}


// 100b1cc5 __errcode

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


// 100b1cf2 __umatherr

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
    if ((&DAT_100dfa78)[iVar1 * 2] == param_2) {
      iVar1 = *(int *)(iVar1 * 8 + 0x100dfa7c);
      goto LAB_100b1d0e;
    }
    iVar1 = iVar1 + 1;
  } while (iVar1 < 0x1d);
  iVar1 = 0;
LAB_100b1d0e:
  if (iVar1 != 0) {
    __ctrlfp();
    iVar1 = FUN_100b6831();
    if (iVar1 == 0) {
      __set_errno_from_matherr(param_1);
    }
    return (float10)(double)CONCAT44(in_stack_00000020,in_stack_0000001c);
  }
  __ctrlfp();
  __set_errno_from_matherr(param_1);
  return (float10)(double)CONCAT44(in_stack_00000020,in_stack_0000001c);
}


// 100b1d90 __handle_qnan1

/* Library Function - Single Match
    __handle_qnan1
   
   Library: Visual Studio 2005 Release */

float10 __cdecl __handle_qnan1(int param_1,double param_2)

{
  DWORD *pDVar1;
  float10 fVar2;
  
  if (DAT_100e004c == 0) {
    fVar2 = __umatherr(1,param_1);
    return fVar2;
  }
  pDVar1 = FUN_100ab320();
  *pDVar1 = 0x21;
  __ctrlfp();
  return (float10)param_2;
}


// 100b1de3 __except1

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
  
  local_14 = DAT_100df7a0 ^ (uint)local_90;
  bVar1 = __handle_exc(param_1,&param_4,param_5);
  if (CONCAT31(extraout_var,bVar1) == 0) {
    local_50 = local_50 & 0xfffffffe;
    __raise_exc_ex(local_90,&param_5,param_1,param_2,(uint *)&param_3,(uint *)&param_4,0);
  }
  cVar2 = __errcode((byte)param_1);
  iVar3 = CONCAT31(extraout_var_00,cVar2);
  if ((DAT_100e004c == 0) && (iVar3 != 0)) {
    __umatherr(iVar3,param_2);
  }
  else {
    __set_errno_from_matherr(iVar3);
    __ctrlfp();
  }
  __security_check_cookie(local_14 ^ (uint)local_90);
  return;
}


// 100b1e9d __statfp

/* Library Function - Single Match
    __statfp
   
   Library: Visual Studio 2005 Release */

int __statfp(void)

{
  short in_FPUStatusWord;
  
  return (int)in_FPUStatusWord;
}


// 100b1ea8 __clrfp

/* Library Function - Single Match
    __clrfp
   
   Library: Visual Studio 2005 Release */

int __clrfp(void)

{
  short in_FPUStatusWord;
  
  return (int)in_FPUStatusWord;
}


// 100b1eb4 __ctrlfp

/* Library Function - Single Match
    __ctrlfp
   
   Library: Visual Studio 2005 Release */

int __ctrlfp(void)

{
  short in_FPUControlWord;
  
  return (int)in_FPUControlWord;
}


// 100b1edb FUN_100b1edb

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_100b1edb(void)

{
  return;
}


// 100b1f31 ___set_fpsr_sse2

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___set_fpsr_sse2
   
   Library: Visual Studio 2005 Release */

void __cdecl ___set_fpsr_sse2(uint param_1)

{
  if (DAT_100f5e08 != 0) {
    if (((param_1 & 0x40) == 0) || (DAT_100dfb78 == 0)) {
      MXCSR = param_1 & 0xffffffbf;
    }
    else {
      MXCSR = param_1;
    }
  }
  return;
}


// 100b1fa3 __close_nolock

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
    if (((_FileHandle == 1) && ((*(byte *)(DAT_100f5d00 + 0x74) & 1) != 0)) ||
       ((_FileHandle == 2 && ((*(byte *)(DAT_100f5d00 + 0x3c) & 1) != 0)))) {
      iVar1 = __get_osfhandle(2);
      iVar2 = __get_osfhandle(1);
      if (iVar2 == iVar1) goto LAB_100b2002;
    }
    hObject = (HANDLE)__get_osfhandle(_FileHandle);
    BVar3 = CloseHandle(hObject);
    if (BVar3 == 0) {
      DVar4 = GetLastError();
      goto LAB_100b2004;
    }
  }
LAB_100b2002:
  DVar4 = 0;
LAB_100b2004:
  __free_osfhnd(_FileHandle);
  *(undefined1 *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38) = 0;
  if (DVar4 == 0) {
    iVar5 = 0;
  }
  else {
    FUN_100ab346(DVar4);
    iVar5 = -1;
  }
  return iVar5;
}


// 100b2037 __close

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
    pDVar1 = FUN_100ab333();
    *pDVar1 = 0;
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_100f5ce8)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_100ab320();
          *pDVar1 = 9;
          local_20 = -1;
        }
        else {
          local_20 = __close_nolock(_FileHandle);
        }
        FUN_100b20fa();
        return local_20;
      }
    }
    pDVar1 = FUN_100ab333();
    *pDVar1 = 0;
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 100b20fa FUN_100b20fa

void FUN_100b20fa(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 100b2104 __freebuf

/* Library Function - Single Match
    __freebuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __freebuf(FILE *_File)

{
  if (((_File->_flag & 0x83U) != 0) && ((_File->_flag & 8U) != 0)) {
    FUN_10002ea0(_File->_base);
    _File->_flag = _File->_flag & 0xfffffbf7;
    _File->_ptr = (char *)0x0;
    _File->_base = (char *)0x0;
    _File->_cnt = 0;
  }
  return;
}


// 100b2130 __flush

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


// 100b2192 FUN_100b2192

int __cdecl FUN_100b2192(FILE *param_1)

{
  int iVar1;
  uint uVar2;
  DWORD DVar3;
  
  if (param_1 == (FILE *)0x0) {
    iVar1 = FUN_100b21d4(0);
    return iVar1;
  }
  iVar1 = __flush(param_1);
  if (iVar1 != 0) {
    return -1;
  }
  if ((param_1->_flag & 0x4000) != 0) {
    uVar2 = __fileno(param_1);
    DVar3 = FUN_100b6c04(uVar2);
    return -(uint)(DVar3 != 0);
  }
  return 0;
}


// 100b21d4 FUN_100b21d4

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

int __cdecl FUN_100b21d4(int param_1)

{
  int *piVar1;
  FILE *pFVar2;
  int iVar3;
  int iVar4;
  int local_28;
  int local_20;
  
  local_20 = 0;
  local_28 = 0;
  __lock(1);
  for (iVar4 = 0; iVar4 < DAT_100f6e40; iVar4 = iVar4 + 1) {
    piVar1 = (int *)(DAT_100f5e20 + iVar4 * 4);
    if ((*piVar1 != 0) && (iVar3 = *piVar1, (*(byte *)(iVar3 + 0xc) & 0x83) != 0)) {
      FUN_100a7da0(iVar4,iVar3);
      pFVar2 = *(FILE **)(DAT_100f5e20 + iVar4 * 4);
      if ((pFVar2->_flag & 0x83U) != 0) {
        if (param_1 == 1) {
          iVar3 = FUN_100b2192(pFVar2);
          if (iVar3 != -1) {
            local_20 = local_20 + 1;
          }
        }
        else if ((param_1 == 0) && ((pFVar2->_flag & 2U) != 0)) {
          iVar3 = FUN_100b2192(pFVar2);
          if (iVar3 == -1) {
            local_28 = -1;
          }
        }
      }
      FUN_100b2276();
    }
  }
  FUN_100b22a5();
  if (param_1 != 1) {
    local_20 = local_28;
  }
  return local_20;
}


// 100b2276 FUN_100b2276

void FUN_100b2276(void)

{
  int unaff_ESI;
  
  FUN_100a7e04(unaff_ESI,*(int *)(DAT_100f5e20 + unaff_ESI * 4));
  return;
}


// 100b22a5 FUN_100b22a5

void FUN_100b22a5(void)

{
  FUN_100afa6b(1);
  return;
}


// 100b2360 __aulldiv

/* Library Function - Single Match
    __aulldiv
   
   Libraries: Visual Studio 2003 Release, Visual Studio 2005 Release */

undefined8 __aulldiv(uint param_1,uint param_2,uint param_3,uint param_4)

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


// 100b23c8 FUN_100b23c8

uint __cdecl FUN_100b23c8(FILE *param_1)

{
  byte bVar1;
  DWORD *pDVar2;
  uint uVar3;
  int iVar4;
  undefined *puVar5;
  LPWSTR pWVar6;
  LPWSTR pWVar7;
  
  if (param_1 == (FILE *)0x0) {
    pDVar2 = FUN_100ab320();
    *pDVar2 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    uVar3 = param_1->_flag;
    if (((uVar3 & 0x83) != 0) && ((uVar3 & 0x40) == 0)) {
      if ((uVar3 & 2) == 0) {
        param_1->_flag = uVar3 | 1;
        if ((uVar3 & 0x10c) == 0) {
          __getbuf(param_1);
        }
        else {
          param_1->_ptr = param_1->_base;
        }
        pWVar7 = (LPWSTR)param_1->_bufsiz;
        pWVar6 = (LPWSTR)param_1->_base;
        uVar3 = __fileno(param_1);
        iVar4 = FUN_100b72a6(uVar3,pWVar6,pWVar7);
        param_1->_cnt = iVar4;
        if ((iVar4 != 0) && (iVar4 != -1)) {
          if ((param_1->_flag & 0x82) == 0) {
            iVar4 = __fileno(param_1);
            if ((iVar4 == -1) || (iVar4 = __fileno(param_1), iVar4 == -2)) {
              puVar5 = &DAT_100df6e0;
            }
            else {
              iVar4 = __fileno(param_1);
              uVar3 = __fileno(param_1);
              puVar5 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_100f5d00)[iVar4 >> 5]);
            }
            if ((puVar5[4] & 0x82) == 0x82) {
              param_1->_flag = param_1->_flag | 0x2000;
            }
          }
          if (((param_1->_bufsiz == 0x200) && ((param_1->_flag & 8U) != 0)) &&
             ((param_1->_flag & 0x400U) == 0)) {
            param_1->_bufsiz = 0x1000;
          }
          param_1->_cnt = param_1->_cnt + -1;
          bVar1 = *param_1->_ptr;
          param_1->_ptr = param_1->_ptr + 1;
          return (uint)bVar1;
        }
        param_1->_flag = param_1->_flag | (-(uint)(iVar4 != 0) & 0x10) + 0x10;
        param_1->_cnt = 0;
      }
      else {
        param_1->_flag = uVar3 | 0x20;
      }
    }
  }
  return 0xffffffff;
}


// 100b24e8 __ceil_default

/* Library Function - Single Match
    __ceil_default
   
   Library: Visual Studio 2005 Release */

float10 __cdecl __ceil_default(double param_1)

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
        fVar4 = __handle_qnan1(0xc,param_1);
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
  fVar4 = (float10)__except1(uVar5,0xc,param_1,dVar1,uVar2);
  return fVar4;
}


// 100b25c0 __ValidateImageBase

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


// 100b25f0 __FindPESection

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


// 100b2640 __IsNonwritableInCurrentImage

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
  local_c = DAT_100df7a0 ^ 0x100d75f0;
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


// 100b2734 __initp_eh_hooks

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __initp_eh_hooks
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

void __initp_eh_hooks(void)

{
  _DAT_100f5824 = FUN_100a9d71(0x100b26fb);
  return;
}


// 100b2745 __initp_misc_winsig

/* Library Function - Single Match
    __initp_misc_winsig
   
   Library: Visual Studio 2005 Release */

void __cdecl __initp_misc_winsig(undefined4 param_1)

{
  DAT_100f5828 = param_1;
  DAT_100f582c = param_1;
  DAT_100f5830 = param_1;
  DAT_100f5834 = param_1;
  return;
}


// 100b275e siglookup

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
  } while (uVar1 < DAT_100df79c * 0xc + param_1);
  if ((DAT_100df79c * 0xc + param_1 <= uVar1) || (*(int *)(uVar1 + 4) != in_EDX)) {
    uVar1 = 0;
  }
  return uVar1;
}


// 100b2792 ___get_sigabrt

/* Library Function - Single Match
    ___get_sigabrt
   
   Library: Visual Studio 2005 Release */

_PHNDLR __cdecl ___get_sigabrt(void)

{
  _PHNDLR p_Var1;
  
  p_Var1 = (_PHNDLR)FUN_100a9de8(DAT_100f5830);
  return p_Var1;
}


// 100b279f _raise

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
        puVar6 = &DAT_100f5828;
        iVar3 = DAT_100f5828;
        goto LAB_100b2854;
      }
      if (_SigNum != 4) {
        if (_SigNum == 6) goto LAB_100b2832;
        if (_SigNum != 8) goto LAB_100b2816;
      }
    }
    pDVar7 = FUN_100a9f8d();
    if (pDVar7 == (DWORD *)0x0) {
      return -1;
    }
    uVar2 = siglookup(pDVar7[0x17]);
    puVar6 = (undefined4 *)(uVar2 + 8);
    pcVar4 = (code *)*puVar6;
  }
  else {
    if (_SigNum == 0xf) {
      puVar6 = &DAT_100f5834;
      iVar3 = DAT_100f5834;
    }
    else if (_SigNum == 0x15) {
      puVar6 = &DAT_100f582c;
      iVar3 = DAT_100f582c;
    }
    else {
      if (_SigNum != 0x16) {
LAB_100b2816:
        pDVar7 = FUN_100ab320();
        *pDVar7 = 0x16;
        FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return -1;
      }
LAB_100b2832:
      puVar6 = &DAT_100f5830;
      iVar3 = DAT_100f5830;
    }
LAB_100b2854:
    bVar1 = true;
    pcVar4 = (code *)FUN_100a9de8(iVar3);
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
      goto LAB_100b28b8;
    }
  }
  else {
LAB_100b28b8:
    if (_SigNum == 8) {
      for (local_28 = DAT_100df790; local_28 < DAT_100df794 + DAT_100df790; local_28 = local_28 + 1)
      {
        *(undefined4 *)(local_28 * 0xc + 8 + pDVar7[0x17]) = 0;
      }
      goto LAB_100b28f2;
    }
  }
  uVar5 = __encoded_null();
  *puVar6 = uVar5;
LAB_100b28f2:
  FUN_100b2913();
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


// 100b2913 FUN_100b2913

void FUN_100b2913(void)

{
  int unaff_EBP;
  
  if (*(int *)(unaff_EBP + -0x1c) != 0) {
    FUN_100afa6b(0);
  }
  return;
}


// 100b294f FUN_100b294f

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100b294f(undefined4 param_1)

{
  _DAT_100f583c = param_1;
  return;
}


// 100b2959 FUN_100b2959

void __cdecl FUN_100b2959(undefined4 param_1)

{
  DAT_100f5840 = param_1;
  return;
}


// 100b2963 ___crtInitCritSecNoSpinCount@8

/* Library Function - Single Match
    ___crtInitCritSecNoSpinCount@8
   
   Library: Visual Studio 2005 Release */

undefined4 ___crtInitCritSecNoSpinCount_8(LPCRITICAL_SECTION param_1)

{
  InitializeCriticalSection(param_1);
  return 1;
}


// 100b2973 ___crtInitCritSecAndSpinCount

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
  
  local_8 = &DAT_100d7650;
  uStack_c = 0x100b297f;
  local_20[0] = 0;
  pcVar1 = (code *)FUN_100a9de8(DAT_100f5840);
  if (pcVar1 != (FARPROC)0x0) goto LAB_100b29e9;
  iVar2 = __get_osplatform(local_20);
  if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  if (local_20[0] == 1) {
LAB_100b29d8:
    pcVar1 = ___crtInitCritSecNoSpinCount_8;
  }
  else {
    hModule = GetModuleHandleA("kernel32.dll");
    if (hModule == (HMODULE)0x0) goto LAB_100b29d8;
    pcVar1 = GetProcAddress(hModule,"InitializeCriticalSectionAndSpinCount");
    if (pcVar1 == (FARPROC)0x0) goto LAB_100b29d8;
  }
  DAT_100f5840 = FUN_100a9d71((int)pcVar1);
LAB_100b29e9:
  local_8 = (undefined *)0x0;
  iVar2 = (*pcVar1)(param_1,param_2);
  return iVar2;
}


// 100b2a38 FUN_100b2a38

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void __cdecl FUN_100b2a38(undefined4 param_1)

{
  _DAT_100f5844 = param_1;
  return;
}


// 100b2a42 FUN_100b2a42

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined4 * __cdecl FUN_100b2a42(char *param_1,char *param_2,int param_3,undefined4 *param_4)

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
  
  local_8 = DAT_100f5c80;
  local_10 = 0;
  local_c = 0;
  local_14 = 0;
  for (pcVar7 = param_2; *pcVar7 == ' '; pcVar7 = pcVar7 + 1) {
  }
  cVar1 = *pcVar7;
  if (cVar1 == 'a') {
    param_2 = (char *)0x109;
LAB_100b2aae:
    local_8 = DAT_100f5c80 | 2;
  }
  else {
    if (cVar1 != 'r') {
      if (cVar1 != 'w') {
        pDVar4 = FUN_100ab320();
        *pDVar4 = 0x16;
        FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return (undefined4 *)0x0;
      }
      param_2 = (char *)0x301;
      goto LAB_100b2aae;
    }
    local_8 = DAT_100f5c80 | 1;
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
          if (local_c != 0) goto LAB_100b2be6;
          param_2 = (char *)((uint)param_2 | 0x20);
          local_c = 1;
        }
        else if (uVar3 != ' ') {
          if (uVar3 == '+') {
            if (((uint)param_2 & 2) != 0) goto LAB_100b2be6;
            param_2 = (char *)((uint)param_2 & 0xfffffffe | 2);
            local_8 = local_8 & 0xfffffffc | 0x80;
          }
          else if (uVar3 == ',') {
            local_14 = 1;
LAB_100b2be6:
            bVar2 = false;
          }
          else if (uVar3 == 'D') {
            if (((uint)param_2 & 0x40) != 0) goto LAB_100b2be6;
            param_2 = (char *)((uint)param_2 | 0x40);
          }
          else if (uVar3 == 'N') {
            param_2 = (char *)((uint)param_2 | 0x80);
          }
          else {
            if (uVar3 != 'R') goto LAB_100b2c81;
            if (local_c != 0) goto LAB_100b2be6;
            param_2 = (char *)((uint)param_2 | 0x10);
            local_c = 1;
          }
        }
      }
      else if (uVar3 == 'T') {
        if (((uint)param_2 & 0x1000) != 0) goto LAB_100b2be6;
        param_2 = (char *)((uint)param_2 | 0x1000);
      }
      else if (uVar3 == 'b') {
        if (((uint)param_2 & 0xc000) != 0) goto LAB_100b2be6;
        param_2 = (char *)((uint)param_2 | 0x8000);
      }
      else if (uVar3 == 'c') {
        if (local_10 != 0) goto LAB_100b2be6;
        local_8 = local_8 | 0x4000;
        local_10 = 1;
      }
      else if (uVar3 == 'n') {
        if (local_10 != 0) goto LAB_100b2be6;
        local_8 = local_8 & 0xffffbfff;
        local_10 = 1;
      }
      else {
        if (uVar3 != 't') goto LAB_100b2c81;
        if (((uint)param_2 & 0xc000) != 0) goto LAB_100b2be6;
        param_2 = (char *)((uint)param_2 | 0x4000);
      }
      _Str2 = _Str2 + 1;
      uVar3 = *_Str2;
    } while (uVar3 != '\0');
    if (local_14 != 0) {
      for (; *_Str2 == ' '; _Str2 = _Str2 + 1) {
      }
      iVar5 = __mbsnbcmp("ccs=",_Str2,4);
      if (iVar5 != 0) goto LAB_100b2c81;
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
          if (iVar5 != 0) goto LAB_100b2c81;
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
    _DAT_100f51ec = _DAT_100f51ec + 1;
    param_4[3] = local_8;
    param_4[1] = 0;
    *param_4 = 0;
    param_4[2] = 0;
    param_4[7] = 0;
    param_4[4] = local_14;
    return param_4;
  }
LAB_100b2c81:
  pDVar4 = FUN_100ab320();
  *pDVar4 = 0x16;
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return (undefined4 *)0x0;
}


// 100b2ce4 __global_unwind2

/* Library Function - Single Match
    __global_unwind2
   
   Library: Visual Studio */

void __cdecl __global_unwind2(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x100b2cfc,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 100b2d49 __local_unwind2

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
  puStack_1c = &LAB_100b2d04;
  local_20 = *unaff_FS_OFFSET;
  *unaff_FS_OFFSET = &local_20;
  while( true ) {
    uVar1 = *(uint *)(param_1 + 0xc);
    if ((uVar1 == 0xffffffff) || ((param_2 != 0xffffffff && (uVar1 <= param_2)))) break;
    local_18 = *(undefined4 *)(*(int *)(param_1 + 8) + uVar1 * 0xc);
    *(undefined4 *)(param_1 + 0xc) = local_18;
    if (*(int *)(*(int *)(param_1 + 8) + 4 + uVar1 * 0xc) == 0) {
      __NLG_Notify(0x101);
      FUN_100b2e18();
    }
  }
  *unaff_FS_OFFSET = local_20;
  return;
}


// 100b2df9 __NLG_Notify

/* Library Function - Single Match
    __NLG_Notify
   
   Libraries: Visual Studio 2017 Debug, Visual Studio 2017 Release, Visual Studio 2019 Debug, Visual
   Studio 2019 Release */

void __NLG_Notify(ulong param_1)

{
  undefined4 in_EAX;
  undefined4 unaff_EBP;
  
  DAT_100dfb8c = param_1;
  DAT_100dfb88 = in_EAX;
  DAT_100dfb90 = unaff_EBP;
  return;
}


// 100b2e18 FUN_100b2e18

void FUN_100b2e18(void)

{
  code *in_EAX;
  
  (*in_EAX)();
  return;
}


// 100b2e1b __rt_probe_read4@4

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __rt_probe_read4@4
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release,
   Visual Studio 2019 Release */

undefined4 __rt_probe_read4_4(void)

{
  return 1;
}


// 100b2e66 FUN_100b2e66

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

void FUN_100b2e66(void)

{
  char cVar1;
  char cVar2;
  undefined **ppuVar3;
  int iVar4;
  int iVar5;
  errno_t eVar6;
  UINT CodePage;
  char *_Str1;
  size_t sVar7;
  DWORD DVar8;
  int *piVar9;
  long lVar10;
  char *pcVar11;
  int local_34;
  int local_30;
  long local_2c;
  int local_28;
  undefined **local_24;
  int local_20 [5];
  undefined4 uStack_c;
  undefined *local_8;
  
  local_8 = &DAT_100d7690;
  uStack_c = 0x100b2e72;
  local_30 = 0;
  local_20[0] = 0;
  local_28 = 0;
  local_2c = 0;
  local_24 = (undefined **)0x0;
  __lock(7);
  local_8 = (undefined *)0x0;
  local_24 = FUN_100b38cc();
  iVar5 = FUN_100b3886(local_20);
  if (iVar5 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  eVar6 = __get_daylight(&local_28);
  if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  eVar6 = __get_dstbias(&local_2c);
  if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  CodePage = ____lc_codepage_func();
  DAT_100f58f4 = 0;
  DAT_100dfba0 = 0xffffffff;
  DAT_100dfb94 = 0xffffffff;
  _Str1 = __getenv_helper_nolock("TZ");
  if ((_Str1 == (char *)0x0) || (*_Str1 == '\0')) {
    if (DAT_100f58f8 != (char *)0x0) {
      FUN_10002ea0(DAT_100f58f8);
      DAT_100f58f8 = (char *)0x0;
    }
    DVar8 = GetTimeZoneInformation((LPTIME_ZONE_INFORMATION)&DAT_100f5848);
    if (DVar8 != 0xffffffff) {
      DAT_100f58f4 = 1;
      local_20[0] = DAT_100f5848 * 0x3c;
      if (DAT_100f588e != 0) {
        local_20[0] = local_20[0] + DAT_100f589c * 0x3c;
      }
      if ((DAT_100f58e2 == 0) || (DAT_100f58f0 == 0)) {
        local_28 = 0;
        local_2c = 0;
      }
      else {
        local_28 = 1;
        local_2c = (DAT_100f58f0 - DAT_100f589c) * 0x3c;
      }
      iVar5 = WideCharToMultiByte(CodePage,0,(LPCWSTR)&DAT_100f584c,-1,*local_24,0x3f,(LPCSTR)0x0,
                                  &local_34);
      if ((iVar5 == 0) || (local_34 != 0)) {
        **local_24 = 0;
      }
      else {
        (*local_24)[0x3f] = 0;
      }
      iVar5 = WideCharToMultiByte(CodePage,0,(LPCWSTR)&DAT_100f58a0,-1,local_24[1],0x3f,(LPCSTR)0x0,
                                  &local_34);
      if ((iVar5 == 0) || (local_34 != 0)) {
        *local_24[1] = 0;
      }
      else {
        local_24[1][0x3f] = 0;
      }
    }
  }
  else {
    if (DAT_100f58f8 != (char *)0x0) {
      iVar5 = _strcmp(_Str1,DAT_100f58f8);
      if (iVar5 == 0) goto LAB_100b3082;
      if (DAT_100f58f8 != (char *)0x0) {
        FUN_10002ea0(DAT_100f58f8);
      }
    }
    sVar7 = _strlen(_Str1);
    DAT_100f58f8 = (char *)FUN_10002dc0(sVar7 + 1);
    if (DAT_100f58f8 != (char *)0x0) {
      pcVar11 = _Str1;
      sVar7 = _strlen(_Str1);
      eVar6 = _strcpy_s(DAT_100f58f8,sVar7 + 1,pcVar11);
      if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
      goto LAB_100b3089;
    }
  }
LAB_100b3082:
  local_30 = 1;
LAB_100b3089:
  iVar4 = local_20[0];
  piVar9 = FUN_100b38c6();
  iVar5 = local_28;
  *piVar9 = iVar4;
  piVar9 = FUN_100b38ba();
  lVar10 = local_2c;
  *piVar9 = iVar5;
  piVar9 = FUN_100b38c0();
  *piVar9 = lVar10;
  local_8 = (undefined *)0xfffffffe;
  FUN_100b3115();
  ppuVar3 = local_24;
  if (local_30 == 0) {
    eVar6 = _strncpy_s(*local_24,0x40,_Str1,3);
    if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
    pcVar11 = _Str1 + 3;
    cVar1 = *pcVar11;
    if (cVar1 == '-') {
      pcVar11 = _Str1 + 4;
    }
    lVar10 = _atol(pcVar11);
    local_20[0] = lVar10 * 0xe10;
    for (; (cVar2 = *pcVar11, cVar2 == '+' || (('/' < cVar2 && (cVar2 < ':'))));
        pcVar11 = pcVar11 + 1) {
    }
    if (*pcVar11 == ':') {
      pcVar11 = pcVar11 + 1;
      lVar10 = _atol(pcVar11);
      local_20[0] = local_20[0] + lVar10 * 0x3c;
      for (; ('/' < *pcVar11 && (*pcVar11 < ':')); pcVar11 = pcVar11 + 1) {
      }
      if (*pcVar11 == ':') {
        pcVar11 = pcVar11 + 1;
        lVar10 = _atol(pcVar11);
        local_20[0] = local_20[0] + lVar10;
        for (; ('/' < *pcVar11 && (*pcVar11 < ':')); pcVar11 = pcVar11 + 1) {
        }
      }
    }
    if (cVar1 == '-') {
      local_20[0] = -local_20[0];
    }
    local_28 = (int)*pcVar11;
    if (local_28 == 0) {
      *ppuVar3[1] = 0;
    }
    else {
      eVar6 = _strncpy_s(ppuVar3[1],0x40,pcVar11,3);
      if (eVar6 != 0) {
                    /* WARNING: Subroutine does not return */
        __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      }
    }
    iVar4 = local_20[0];
    piVar9 = FUN_100b38c6();
    iVar5 = local_28;
    *piVar9 = iVar4;
    piVar9 = FUN_100b38ba();
    *piVar9 = iVar5;
  }
  return;
}


// 100b3115 FUN_100b3115

void FUN_100b3115(void)

{
  FUN_100afa6b(7);
  return;
}


// 100b31af cvtdate

/* Library Function - Single Match
    _cvtdate
   
   Library: Visual Studio 2005 Release */

int __cdecl
cvtdate(int param_1,int param_2,uint param_3,int param_4,int param_5,int param_6,int param_7,
       int param_8,int param_9)

{
  int in_EAX;
  uint uVar1;
  int iVar2;
  errno_t eVar3;
  int in_ECX;
  int iVar4;
  int iVar5;
  int iVar6;
  bool bVar7;
  long local_8;
  
  local_8 = 0;
  if (param_2 == 1) {
    uVar1 = param_3 & 0x80000003;
    if ((int)uVar1 < 0) {
      uVar1 = (uVar1 - 1 | 0xfffffffc) + 1;
    }
    if (((uVar1 == 0) && ((int)param_3 % 100 != 0)) || ((int)(param_3 + 0x76c) % 400 == 0)) {
      iVar4 = *(int *)(&DAT_100e014c + in_EAX * 4);
    }
    else {
      iVar4 = *(int *)(&DAT_100e0180 + in_EAX * 4);
    }
    iVar6 = iVar4 + 1;
    iVar5 = (int)(param_3 * 0x16d + -0x63db +
                 ((int)((param_3 - 1) + ((int)(param_3 - 1) >> 0x1f & 3U)) >> 2) + iVar6 +
                 ((int)(param_3 + 299) / 400 - (int)(param_3 - 1) / 100)) % 7;
    iVar2 = (param_4 * 7 - iVar5) + param_5;
    if (iVar5 <= param_5) {
      iVar6 = iVar4 + -6;
    }
    iVar6 = iVar6 + iVar2;
    if (param_4 == 5) {
      if (((uVar1 == 0) && ((int)param_3 % 100 != 0)) || ((int)(param_3 + 0x76c) % 400 == 0)) {
        iVar2 = (&DAT_100e0150)[in_EAX];
      }
      else {
        iVar2 = *(int *)(&DAT_100e0184 + in_EAX * 4);
      }
      if (iVar2 < iVar6) {
        iVar6 = iVar6 + -7;
      }
    }
  }
  else {
    uVar1 = param_3 & 0x80000003;
    bVar7 = uVar1 == 0;
    if ((int)uVar1 < 0) {
      bVar7 = (uVar1 - 1 | 0xfffffffc) == 0xffffffff;
    }
    if (((bVar7) && (iVar2 = (int)param_3 / 100, (int)param_3 % 100 != 0)) ||
       (iVar2 = (int)(param_3 + 0x76c) / 400, (int)(param_3 + 0x76c) % 400 == 0)) {
      iVar6 = *(int *)(&DAT_100e014c + in_EAX * 4);
    }
    else {
      iVar6 = *(int *)(&DAT_100e0180 + in_EAX * 4);
    }
    iVar6 = iVar6 + param_6;
  }
  iVar4 = ((in_ECX * 0x3c + param_7) * 0x3c + param_8) * 1000 + param_9;
  if (param_1 == 1) {
    DAT_100dfb94 = param_3;
    DAT_100dfb98 = iVar6;
    DAT_100dfb9c = iVar4;
  }
  else {
    DAT_100dfba4 = iVar6;
    DAT_100dfba8 = iVar4;
    eVar3 = __get_dstbias(&local_8);
    if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
    iVar2 = local_8 * 1000;
    DAT_100dfba8 = DAT_100dfba8 + iVar2;
    if (DAT_100dfba8 < 0) {
      DAT_100dfba8 = DAT_100dfba8 + 86400000;
      DAT_100dfba4 = DAT_100dfba4 + -1;
    }
    else {
      iVar2 = 86400000;
      if (86399999 < DAT_100dfba8) {
        DAT_100dfba8 = DAT_100dfba8 + -86400000;
        DAT_100dfba4 = DAT_100dfba4 + 1;
      }
    }
    DAT_100dfba0 = param_3;
  }
  return iVar2;
}


// 100b33a4 __isindst_nolock

/* Library Function - Single Match
    __isindst_nolock
   
   Library: Visual Studio 2005 Release */

bool __isindst_nolock(void)

{
  int iVar1;
  bool bVar2;
  errno_t eVar3;
  uint uVar4;
  uint uVar5;
  int iVar6;
  int *unaff_EDI;
  uint uVar7;
  uint uVar8;
  int local_8;
  
  local_8 = 0;
  eVar3 = __get_daylight(&local_8);
  if (eVar3 != 0) {
                    /* WARNING: Subroutine does not return */
    __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  if (local_8 == 0) {
    return false;
  }
  uVar7 = unaff_EDI[5];
  if ((uVar7 != DAT_100dfb94) || (uVar7 != DAT_100dfba0)) {
    if (DAT_100f58f4 == 0) {
      cvtdate(1,1,uVar7,1,0,0,0,0,0);
      cvtdate(0,1,unaff_EDI[5],5,0,0,0,0,0);
    }
    else {
      if (DAT_100f58e0 != 0) {
        uVar8 = (uint)DAT_100f58e6;
        uVar4 = 0;
        uVar5 = 0;
      }
      else {
        uVar4 = (uint)DAT_100f58e4;
        uVar8 = 0;
        uVar5 = (uint)DAT_100f58e6;
      }
      cvtdate(1,(uint)(DAT_100f58e0 == 0),uVar7,uVar5,uVar4,uVar8,(uint)DAT_100f58ea,
              (uint)DAT_100f58ec,(uint)DAT_100f58ee);
      if (DAT_100f588c != 0) {
        uVar8 = (uint)DAT_100f5892;
        uVar4 = 0;
        uVar5 = 0;
        uVar7 = unaff_EDI[5];
      }
      else {
        uVar4 = (uint)DAT_100f5890;
        uVar8 = 0;
        uVar5 = (uint)DAT_100f5892;
        uVar7 = unaff_EDI[5];
      }
      cvtdate(0,(uint)(DAT_100f588c == 0),uVar7,uVar5,uVar4,uVar8,(uint)DAT_100f5896,
              (uint)DAT_100f5898,(uint)DAT_100f589a);
    }
  }
  iVar1 = unaff_EDI[7];
  if (DAT_100dfb98 < DAT_100dfba4) {
    if ((iVar1 < DAT_100dfb98) || (DAT_100dfba4 < iVar1)) {
      return false;
    }
    if ((DAT_100dfb98 < iVar1) && (iVar1 < DAT_100dfba4)) {
      return true;
    }
  }
  else {
    if (iVar1 < DAT_100dfba4) {
      return true;
    }
    if (DAT_100dfb98 < iVar1) {
      return true;
    }
    if ((DAT_100dfba4 < iVar1) && (iVar1 < DAT_100dfb98)) {
      return false;
    }
  }
  iVar6 = ((unaff_EDI[2] * 0x3c + unaff_EDI[1]) * 0x3c + *unaff_EDI) * 1000;
  if (iVar1 == DAT_100dfb98) {
    bVar2 = DAT_100dfb9c <= iVar6;
  }
  else {
    bVar2 = iVar6 < DAT_100dfba8;
  }
  return bVar2;
}


// 100b3552 ___tzset

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    ___tzset
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release, Visual Studio 2010 Release */

void __cdecl ___tzset(void)

{
  if (DAT_100f58fc == 0) {
    __lock(6);
    if (DAT_100f58fc == 0) {
      FUN_100b2e66();
      DAT_100f58fc = DAT_100f58fc + 1;
    }
    FUN_100b3598();
  }
  return;
}


// 100b3598 FUN_100b3598

void FUN_100b3598(void)

{
  FUN_100afa6b(6);
  return;
}


// 100b35a1 __isindst

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */
/* Library Function - Single Match
    __isindst
   
   Library: Visual Studio 2005 Release */

int __cdecl __isindst(tm *_Time)

{
  bool bVar1;
  undefined3 extraout_var;
  
  __lock(6);
  bVar1 = __isindst_nolock();
  FUN_100b35d9();
  return CONCAT31(extraout_var,bVar1);
}


// 100b35d9 FUN_100b35d9

void FUN_100b35d9(void)

{
  FUN_100afa6b(6);
  return;
}


// 100b35e2 __gmtime64_s

/* Library Function - Single Match
    __gmtime64_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __gmtime64_s(tm *_Tm,__time64_t *_Time)

{
  tm *ptVar1;
  bool bVar2;
  tm *ptVar3;
  DWORD *pDVar4;
  uint uVar5;
  int iVar6;
  undefined4 *puVar7;
  int iVar8;
  uint uVar9;
  bool bVar10;
  undefined8 uVar11;
  longlong lVar12;
  longlong lVar13;
  uint local_10;
  
  ptVar3 = _Tm;
  bVar2 = false;
  if ((_Tm == (tm *)0x0) || (_memset(_Tm,0xff,0x24), _Time == (__time64_t *)0x0)) {
LAB_100b35f6:
    pDVar4 = FUN_100ab320();
    *pDVar4 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  uVar9 = *(uint *)((int)_Time + 4);
  lVar13 = *_Time;
  if (((int)uVar9 < 1) && ((int)uVar9 < 0)) {
    pDVar4 = FUN_100ab320();
    *pDVar4 = 0x16;
    return 0x16;
  }
  if ((6 < (int)uVar9) && ((7 < (int)uVar9 || (0x93406fff < (uint)*_Time)))) goto LAB_100b35f6;
  uVar11 = __alldiv((uint)*_Time,uVar9,0x1e13380,0);
  uVar9 = (uint)uVar11;
  _Tm = (tm *)(uVar9 + 0x46);
  ptVar1 = (tm *)(uVar9 + 0x45);
  lVar12 = __allmul(uVar9,(int)uVar9 >> 0x1f,0xfffffe93,-1);
  lVar12 = lVar12 - (((int)(uVar9 + 0x171) / 400 - (int)ptVar1 / 100) + -0x11 +
                    ((int)((int)&ptVar1->tm_sec + ((int)ptVar1 >> 0x1f & 3U)) >> 2));
  lVar12 = __allmul((uint)lVar12,(int)((ulonglong)lVar12 >> 0x20),0x15180,0);
  lVar12 = lVar12 + lVar13;
  iVar8 = (int)lVar12;
  if ((lVar12 < 0x100000000) && (lVar12 < 0)) {
    lVar12 = lVar12 + 0x1e13380;
    uVar5 = (uint)ptVar1 & 0x80000003;
    bVar10 = uVar5 == 0;
    if ((int)uVar5 < 0) {
      bVar10 = (uVar5 - 1 | 0xfffffffc) == 0xffffffff;
    }
    _Tm = ptVar1;
    if (((!bVar10) || ((int)ptVar1 % 100 == 0)) && ((int)(uVar9 + 0x7b1) % 400 != 0))
    goto LAB_100b375f;
    lVar12 = CONCAT44((int)((ulonglong)lVar12 >> 0x20) + (uint)(0xfffeae7f < (uint)lVar12),
                      iVar8 + 0x1e28500);
  }
  else {
    uVar5 = (uint)_Tm & 0x80000003;
    bVar10 = uVar5 == 0;
    if ((int)uVar5 < 0) {
      bVar10 = (uVar5 - 1 | 0xfffffffc) == 0xffffffff;
    }
    if (((!bVar10) || ((int)_Tm % 100 == 0)) && ((int)(uVar9 + 0x7b2) % 400 != 0))
    goto LAB_100b375f;
  }
  bVar2 = true;
LAB_100b375f:
  local_10 = (uint)((ulonglong)lVar12 >> 0x20);
  ptVar3->tm_year = (int)_Tm;
  uVar11 = __alldiv((uint)lVar12,local_10,0x15180,0);
  uVar9 = (uint)uVar11;
  ptVar3->tm_yday = uVar9;
  lVar13 = __allmul(uVar9,(int)uVar9 >> 0x1f,0xfffeae80,-1);
  lVar13 = lVar13 + lVar12;
  puVar7 = &DAT_100e0150;
  if (!bVar2) {
    puVar7 = (undefined4 *)&DAT_100e0184;
  }
  iVar6 = 1;
  iVar8 = puVar7[1];
  while (iVar8 < ptVar3->tm_yday) {
    iVar6 = iVar6 + 1;
    iVar8 = puVar7[iVar6];
  }
  ptVar3->tm_mon = iVar6 + -1;
  ptVar3->tm_mday = ptVar3->tm_yday - puVar7[iVar6 + -1];
  uVar11 = __alldiv((uint)*_Time,*(uint *)((int)_Time + 4),0x15180,0);
  ptVar3->tm_wday = ((int)uVar11 + 4) % 7;
  uVar11 = __alldiv((uint)lVar13,(uint)((ulonglong)lVar13 >> 0x20),0xe10,0);
  uVar9 = (uint)uVar11;
  ptVar3->tm_hour = uVar9;
  lVar12 = __allmul(uVar9,(int)uVar9 >> 0x1f,0xfffff1f0,-1);
  uVar9 = (uint)(lVar13 + lVar12);
  uVar11 = __alldiv(uVar9,(uint)((ulonglong)(lVar13 + lVar12) >> 0x20),0x3c,0);
  ptVar3->tm_min = (int)uVar11;
  ptVar3->tm_isdst = 0;
  ptVar3->tm_sec = uVar9 + (int)uVar11 * -0x3c;
  return 0;
}


// 100b381e __get_daylight

/* Library Function - Single Match
    __get_daylight
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __get_daylight(int *_Daylight)

{
  DWORD *pDVar1;
  
  if (_Daylight == (int *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  *_Daylight = DAT_100dfbb4;
  return 0;
}


// 100b3852 __get_dstbias

/* Library Function - Single Match
    __get_dstbias
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl __get_dstbias(long *_Daylight_savings_bias)

{
  DWORD *pDVar1;
  
  if (_Daylight_savings_bias == (long *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  *_Daylight_savings_bias = DAT_100dfbb8;
  return 0;
}


// 100b3886 FUN_100b3886

undefined4 __cdecl FUN_100b3886(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if (param_1 == (undefined4 *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  *param_1 = DAT_100dfbb0;
  return 0;
}


// 100b38ba FUN_100b38ba

undefined4 * FUN_100b38ba(void)

{
  return &DAT_100dfbb4;
}


// 100b38c0 FUN_100b38c0

undefined4 * FUN_100b38c0(void)

{
  return &DAT_100dfbb8;
}


// 100b38c6 FUN_100b38c6

undefined4 * FUN_100b38c6(void)

{
  return &DAT_100dfbb0;
}


// 100b38cc FUN_100b38cc

undefined ** FUN_100b38cc(void)

{
  return &PTR_DAT_100dfc40;
}


// 100b38d2 ___getgmtimebuf

/* Library Function - Single Match
    ___getgmtimebuf
   
   Library: Visual Studio 2005 Release */

tm * __cdecl ___getgmtimebuf(void)

{
  DWORD *pDVar1;
  tm *ptVar2;
  DWORD DVar3;
  tm *in_ECX;
  
  pDVar1 = FUN_100a9f8d();
  if (pDVar1 == (DWORD *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0xc;
    ptVar2 = (tm *)0x0;
  }
  else {
    if (pDVar1[0x11] == 0) {
      DVar3 = FUN_10002dc0(0x24);
      pDVar1[0x11] = DVar3;
      if (DVar3 == 0) {
        pDVar1 = FUN_100ab320();
        *pDVar1 = 0xc;
        return in_ECX;
      }
    }
    ptVar2 = (tm *)pDVar1[0x11];
  }
  return ptVar2;
}


// 100b391a FUN_100b391a

uint __cdecl FUN_100b391a(char *param_1,uint param_2,uint param_3,FILE *param_4)

{
  DWORD *pDVar1;
  int iVar2;
  uint uVar3;
  uint uVar4;
  uint _Size;
  uint uVar5;
  uint uVar6;
  char *_Buf;
  uint local_c;
  char *local_8;
  
  if ((param_2 != 0) && (param_3 != 0)) {
    if ((param_4 != (FILE *)0x0) &&
       ((param_1 != (char *)0x0 && (param_3 <= (uint)(0xffffffff / (ulonglong)param_2))))) {
      uVar6 = param_2 * param_3;
      uVar5 = uVar6;
      if ((param_4->_flag & 0x10c) == 0) {
        local_c = 0x1000;
      }
      else {
        local_c = param_4->_bufsiz;
      }
      do {
        while( true ) {
          if (uVar5 == 0) {
            return param_3;
          }
          uVar4 = param_4->_flag & 0x108;
          if (uVar4 == 0) break;
          uVar3 = param_4->_cnt;
          if (uVar3 == 0) break;
          if ((int)uVar3 < 0) {
            param_4->_flag = param_4->_flag | 0x20;
            goto LAB_100b3a64;
          }
          _Size = uVar5;
          if (uVar3 <= uVar5) {
            _Size = uVar3;
          }
          _memcpy(param_4->_ptr,param_1,_Size);
          param_4->_cnt = param_4->_cnt - _Size;
          param_4->_ptr = param_4->_ptr + _Size;
          uVar5 = uVar5 - _Size;
LAB_100b3a20:
          local_8 = param_1 + _Size;
          param_1 = local_8;
        }
        if (local_c <= uVar5) {
          if ((uVar4 != 0) && (iVar2 = __flush(param_4), iVar2 != 0)) goto LAB_100b3a64;
          uVar4 = uVar5;
          if (local_c != 0) {
            uVar4 = uVar5 - uVar5 % local_c;
          }
          _Buf = param_1;
          uVar3 = uVar4;
          iVar2 = __fileno(param_4);
          uVar3 = __write(iVar2,_Buf,uVar3);
          if (uVar3 != 0xffffffff) {
            _Size = uVar4;
            if (uVar3 <= uVar4) {
              _Size = uVar3;
            }
            uVar5 = uVar5 - _Size;
            if (uVar4 <= uVar3) goto LAB_100b3a20;
          }
          param_4->_flag = param_4->_flag | 0x20;
LAB_100b3a64:
          return (uVar6 - uVar5) / param_2;
        }
        iVar2 = __flsbuf((int)*param_1,param_4);
        if (iVar2 == -1) goto LAB_100b3a64;
        param_1 = param_1 + 1;
        local_c = param_4->_bufsiz;
        uVar5 = uVar5 - 1;
        if ((int)local_c < 1) {
          local_c = 1;
        }
      } while( true );
    }
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return 0;
}


// 100b3a79 FID_conflict:__atoflt_l

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_100ac4ab((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tod(&local_14,(_CRT_DOUBLE *)_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_100b3ad0:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_100b3b10;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_100b3b02:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_100b3b10;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_100b3b02;
    goto LAB_100b3ad0;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_100b3b10:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 100b3b1f FID_conflict:__atoflt_l

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  _LocaleUpdate::_LocaleUpdate(local_28,_Locale);
  local_18 = FUN_100ac4ab((undefined2 *)&local_14,&local_2c,_Str,0,0,0,0,(int *)local_28);
  IVar1 = __ld12tof(&local_14,_Result);
  if ((local_18 & 3) == 0) {
    if (IVar1 == INTRNCVT_OVERFLOW) {
LAB_100b3b76:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_100b3bb6;
    }
    if (IVar1 != INTRNCVT_UNDERFLOW) {
LAB_100b3ba8:
      if (local_1c != '\0') {
        *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
      }
      goto LAB_100b3bb6;
    }
  }
  else if ((local_18 & 1) == 0) {
    if ((local_18 & 2) == 0) goto LAB_100b3ba8;
    goto LAB_100b3b76;
  }
  if (local_1c != '\0') {
    *(uint *)(local_20 + 0x70) = *(uint *)(local_20 + 0x70) & 0xfffffffd;
  }
LAB_100b3bb6:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 100b3bc5 _strcpy_s

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
      pDVar2 = FUN_100ab320();
      eVar4 = 0x22;
      *pDVar2 = 0x22;
      goto LAB_100b3be4;
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_100ab320();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_100b3be4:
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 100b3c2a __fptostr

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
    pDVar1 = FUN_100ab320();
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
    pDVar1 = FUN_100ab320();
    eVar7 = 0x22;
    *pDVar1 = 0x22;
  }
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar7;
}


// 100b3ce7 ___dtold

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
      goto LAB_100b3d99;
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
LAB_100b3d99:
  *(ushort *)(param_1 + 2) = uVar4;
  return;
}


// 100b3da2 __fltout2

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  ___dtold(&local_14,(uint *)&_Dbl);
  iVar1 = FUN_100b8014(local_14,uStack_10,uStack_c,0x11,0,&local_30);
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


// 100b3e30 __alldvrm

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


// 100b3f10 __aullshr

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


// 100b3f38 __controlfp_s

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
    pDVar2 = FUN_100ab320();
    eVar3 = 0x16;
    *pDVar2 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return eVar3;
}


// 100b3fa2 FUN_100b3fa2

undefined4 __cdecl FUN_100b3fa2(undefined4 param_1,undefined4 param_2,uint param_3)

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
  if (DAT_100f5904 == 0) {
    hModule = LoadLibraryA("USER32.DLL");
    if (hModule == (HMODULE)0x0) {
      return 0;
    }
    pFVar1 = GetProcAddress(hModule,"MessageBoxA");
    if (pFVar1 == (FARPROC)0x0) {
      return 0;
    }
    DAT_100f5904 = FUN_100a9d71((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetActiveWindow");
    DAT_100f5908 = FUN_100a9d71((int)pFVar1);
    pFVar1 = GetProcAddress(hModule,"GetLastActivePopup");
    DAT_100f590c = FUN_100a9d71((int)pFVar1);
    iVar2 = __get_osplatform(&local_10);
    if (iVar2 != 0) {
                    /* WARNING: Subroutine does not return */
      __invoke_watson((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    }
    if (local_10 == 2) {
      pFVar1 = GetProcAddress(hModule,"GetUserObjectInformationA");
      DAT_100f5914 = FUN_100a9d71((int)pFVar1);
      if (DAT_100f5914 != 0) {
        pFVar1 = GetProcAddress(hModule,"GetProcessWindowStation");
        DAT_100f5910 = FUN_100a9d71((int)pFVar1);
      }
    }
  }
  if ((DAT_100f5910 != local_c) && (DAT_100f5914 != local_c)) {
    pcVar3 = (code *)FUN_100a9de8(DAT_100f5910);
    pcVar4 = (code *)FUN_100a9de8(DAT_100f5914);
    if (((pcVar3 != (code *)0x0) && (pcVar4 != (code *)0x0)) &&
       (((iVar2 = (*pcVar3)(), iVar2 == 0 ||
         (iVar2 = (*pcVar4)(iVar2,1,local_24,0xc,local_18), iVar2 == 0)) || ((local_1c & 1) == 0))))
    {
      iVar2 = FUN_100a820d(&local_14);
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
      goto LAB_100b4138;
    }
  }
  if ((((DAT_100f5908 != local_c) &&
       (pcVar3 = (code *)FUN_100a9de8(DAT_100f5908), pcVar3 != (code *)0x0)) &&
      (local_8 = (*pcVar3)(), local_8 != 0)) &&
     ((DAT_100f590c != local_c &&
      (pcVar3 = (code *)FUN_100a9de8(DAT_100f590c), pcVar3 != (code *)0x0)))) {
    local_8 = (*pcVar3)(local_8);
  }
LAB_100b4138:
  pcVar3 = (code *)FUN_100a9de8(DAT_100f5904);
  if (pcVar3 == (code *)0x0) {
    return 0;
  }
  uVar5 = (*pcVar3)(local_8,param_1,param_2,param_3);
  return uVar5;
}


// 100b415f _strcat_s

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
        pDVar2 = FUN_100ab320();
        eVar4 = 0x22;
        *pDVar2 = 0x22;
        goto LAB_100b417e;
      }
    }
    *_Dst = '\0';
  }
  pDVar2 = FUN_100ab320();
  eVar4 = 0x16;
  *pDVar2 = 0x16;
LAB_100b417e:
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar4;
}


// 100b41d0 _strncpy_s

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
LAB_100b41f4:
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
          pDVar2 = FUN_100ab320();
          eVar5 = 0x22;
          *pDVar2 = 0x22;
          goto LAB_100b4205;
        }
        *_Dst = '\0';
      }
    }
  }
  else if (_Dst != (char *)0x0) goto LAB_100b41f4;
  pDVar2 = FUN_100ab320();
  eVar5 = 0x16;
  *pDVar2 = 0x16;
LAB_100b4205:
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return eVar5;
}


// 100b4283 __set_error_mode

/* Library Function - Single Match
    __set_error_mode
   
   Library: Visual Studio 2005 Release */

int __cdecl __set_error_mode(int _Mode)

{
  int iVar1;
  DWORD *pDVar2;
  
  if (-1 < _Mode) {
    if (_Mode < 3) {
      iVar1 = DAT_100f51e0;
      DAT_100f51e0 = _Mode;
      return iVar1;
    }
    if (_Mode == 3) {
      return DAT_100f51e0;
    }
  }
  pDVar2 = FUN_100ab320();
  *pDVar2 = 0x16;
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 100b42c9 FUN_100b42c9

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

undefined4 FUN_100b42c9(void)

{
  return 1;
}


// 100b4319 __get_sse2_info

/* WARNING: Removing unreachable block (ram,0x100b4354) */
/* WARNING: Removing unreachable block (ram,0x100b4341) */
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
  if (((local_8 & 0x4000000) == 0) || (iVar2 = FUN_100b42c9(), iVar2 == 0)) {
    uVar3 = 0;
  }
  else {
    uVar3 = 1;
  }
  return uVar3;
}


// 100b4388 __local_unwind4

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
  puStack_24 = &LAB_100b4418;
  uStack_28 = *unaff_FS_OFFSET;
  local_20 = DAT_100df7a0 ^ (uint)&uStack_28;
  *unaff_FS_OFFSET = &uStack_28;
  while( true ) {
    uVar2 = *(uint *)(param_2 + 0xc);
    if ((uVar2 == 0xfffffffe) || ((param_3 != 0xfffffffe && (uVar2 <= param_3)))) break;
    puVar1 = (undefined4 *)((*(uint *)(param_2 + 8) ^ *param_1) + 0x10 + uVar2 * 0xc);
    *(undefined4 *)(param_2 + 0xc) = *puVar1;
    if (puVar1[1] == 0) {
      __NLG_Notify(0x101);
      FUN_100b2e18();
    }
  }
  *unaff_FS_OFFSET = uStack_28;
  return;
}


// 100b445e FUN_100b445e

void FUN_100b445e(int param_1)

{
  __local_unwind4(*(uint **)(param_1 + 0x28),*(int *)(param_1 + 0x18),*(uint *)(param_1 + 0x1c));
  return;
}


// 100b447a _EH4_CallFilterFunc

/* Library Function - Single Match
    @_EH4_CallFilterFunc@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_CallFilterFunc,8 */

void __fastcall _EH4_CallFilterFunc(undefined *param_1)

{
  (*(code *)param_1)();
  return;
}


// 100b4491 _EH4_TransferToHandler

/* Library Function - Single Match
    @_EH4_TransferToHandler@8
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_TransferToHandler,8 */

void __fastcall _EH4_TransferToHandler(undefined *UNRECOVERED_JUMPTABLE)

{
  __NLG_Notify(1);
                    /* WARNING: Could not recover jumptable at 0x100b44a8. Too many branches */
                    /* WARNING: Treating indirect jump as call */
  (*(code *)UNRECOVERED_JUMPTABLE)();
  return;
}


// 100b44aa _EH4_GlobalUnwind

/* Library Function - Single Match
    @_EH4_GlobalUnwind@4
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_GlobalUnwind,4 */

void __fastcall _EH4_GlobalUnwind(PVOID param_1)

{
  RtlUnwind(param_1,(PVOID)0x100b44bf,(PEXCEPTION_RECORD)0x0,(PVOID)0x0);
  return;
}


// 100b44c4 _EH4_LocalUnwind

/* Library Function - Single Match
    @_EH4_LocalUnwind@16
   
   Library: Visual Studio 2005 Release
   __fastcall _EH4_LocalUnwind,16 */

void __fastcall _EH4_LocalUnwind(int param_1,uint param_2,undefined4 param_3,uint *param_4)

{
  __local_unwind4(param_4,param_1,param_2);
  return;
}


// 100b44db __lseeki64_nolock

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
    pDVar2 = FUN_100ab320();
    *pDVar2 = 9;
LAB_100b450a:
    DVar3 = 0xffffffff;
    local_8 = -1;
  }
  else {
    DVar3 = SetFilePointer(hFile,in_stack_00000008,&local_8,_Offset._4_4_);
    if (DVar3 == 0xffffffff) {
      DVar4 = GetLastError();
      if (DVar4 != 0) {
        FUN_100ab346(DVar4);
        goto LAB_100b450a;
      }
    }
    pbVar1 = (byte *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38);
    *pbVar1 = *pbVar1 & 0xfd;
  }
  return CONCAT44(local_8,DVar3);
}


// 100b455e __lseeki64

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
    pDVar1 = FUN_100ab333();
    *pDVar1 = 0;
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_100f5ce8)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
        pDVar1 = FUN_100ab333();
        *pDVar1 = 0;
        pDVar1 = FUN_100ab320();
        *pDVar1 = 9;
        FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        local_28._4_4_ = 0xffffffff;
        local_28._0_4_ = 0xffffffff;
      }
      else {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_100ab320();
          *pDVar1 = 9;
          pDVar1 = FUN_100ab333();
          *pDVar1 = 0;
          local_28 = -1;
        }
        else {
          local_28 = __lseeki64_nolock(_FileHandle,_Offset,in_stack_ffffffc8);
        }
        FUN_100b466d();
      }
      goto LAB_100b4667;
    }
    pDVar1 = FUN_100ab333();
    *pDVar1 = 0;
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  local_28._0_4_ = 0xffffffff;
  local_28._4_4_ = 0xffffffff;
LAB_100b4667:
  return CONCAT44(local_28._4_4_,(undefined4)local_28);
}


// 100b466d FUN_100b466d

void FUN_100b466d(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 100b4677 __write_nolock

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
  
  local_8 = DAT_100df7a0 ^ (uint)local_51c;
  local_580 = (WCHAR *)_Buf;
  local_57c = (char *)0x0;
  local_584 = 0;
  if (_MaxCharCount == 0) goto LAB_100b4c27;
  if (_Buf == (void *)0x0) {
    pDVar6 = FUN_100ab333();
    *pDVar6 = 0;
    pDVar6 = FUN_100ab320();
    *pDVar6 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_100b4c27;
  }
  iVar11 = (_FileHandle & 0x1fU) * 0x38;
  piVar16 = &DAT_100f5d00 + (_FileHandle >> 5);
  local_571 = (char)(*(char *)(*piVar16 + iVar11 + 0x24) * '\x02') >> 1;
  local_58c = piVar16;
  if (((local_571 == '\x02') || (local_571 == '\x01')) && ((~_MaxCharCount & 1) == 0)) {
    pDVar6 = FUN_100ab333();
    *pDVar6 = 0;
    pDVar6 = FUN_100ab320();
    *pDVar6 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    goto LAB_100b4c27;
  }
  if ((*(byte *)(*piVar16 + iVar11 + 4) & 0x20) != 0) {
    __lseeki64_nolock(_FileHandle,0x200000000,unaff_EDI);
  }
  iVar7 = __isatty(_FileHandle);
  if ((iVar7 == 0) || ((*(byte *)(iVar11 + 4 + *piVar16) & 0x80) == 0)) {
LAB_100b4962:
    if ((*(byte *)((undefined4 *)(*piVar16 + iVar11) + 1) & 0x80) == 0) {
      BVar9 = WriteFile(*(HANDLE *)(*piVar16 + iVar11),local_580,_MaxCharCount,(LPDWORD)&local_588,
                        (LPOVERLAPPED)0x0);
      if (BVar9 == 0) {
LAB_100b4bbc:
        local_570 = GetLastError();
      }
      else {
        local_570 = 0;
        local_57c = local_588;
      }
LAB_100b4bc5:
      piVar16 = local_58c;
      if (local_57c != (char *)0x0) goto LAB_100b4c27;
      goto LAB_100b4bcf;
    }
    local_570 = 0;
    if (local_571 == '\0') {
      local_56c = local_580;
      if (_MaxCharCount == 0) goto LAB_100b4bf6;
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
        if (BVar9 == 0) goto LAB_100b4bbc;
        local_57c = local_57c + (int)local_588;
      } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
              ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
      goto LAB_100b4bc5;
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
          if (BVar9 == 0) goto LAB_100b4bbc;
          local_57c = local_57c + (int)local_588;
        } while (((int)pWVar13 - (int)local_568 <= (int)local_588) &&
                ((uint)((int)local_56c - (int)local_580) < _MaxCharCount));
        goto LAB_100b4bc5;
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
          if (iVar7 == 0) goto LAB_100b4bbc;
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
        goto LAB_100b4bc5;
      }
    }
LAB_100b4bf6:
    DVar10 = 0;
    if (((*(byte *)(iVar11 + 4 + *piVar16) & 0x40) != 0) && ((char)*local_580 == '\x1a'))
    goto LAB_100b4c27;
    pDVar6 = FUN_100ab320();
    *pDVar6 = 0x1c;
  }
  else {
    p_Var8 = __getptd();
    pwVar3 = p_Var8->ptlocinfo->lc_category[0].wlocale;
    BVar9 = GetConsoleMode(*(HANDLE *)(iVar11 + *piVar16),&local_598);
    if ((BVar9 == 0) || ((pwVar3 == (wchar_t *)0x0 && (local_571 == '\0')))) goto LAB_100b4962;
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
          if (BVar9 == 0) goto LAB_100b4bbc;
          local_57c = local_57c + (int)local_56c;
          if ((int)local_56c < (int)DVar10) break;
          if (local_594 != 0) {
            local_10[0] = '\r';
            BVar9 = WriteFile(*(HANDLE *)(iVar11 + *piVar16),local_10,1,(LPDWORD)&local_56c,
                              (LPOVERLAPPED)0x0);
            if (BVar9 == 0) goto LAB_100b4bbc;
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
            if (wVar5 != (wchar_t)local_570) goto LAB_100b4bbc;
            local_57c = local_57c + 1;
            if (local_594 != 0) {
              local_570 = 0xd;
              wVar5 = __putwch_nolock(L'\r');
              if (wVar5 != (wchar_t)local_570) goto LAB_100b4bbc;
              local_57c = local_57c + 1;
              local_584 = local_584 + 1;
            }
          }
        }
      } while (local_578 < _MaxCharCount);
      goto LAB_100b4bc5;
    }
LAB_100b4bcf:
    if (local_570 == 0) goto LAB_100b4bf6;
    DVar10 = 5;
    if (local_570 != 5) {
      FUN_100ab346(local_570);
      goto LAB_100b4c27;
    }
    pDVar6 = FUN_100ab320();
    *pDVar6 = 9;
  }
  pDVar6 = FUN_100ab333();
  *pDVar6 = DVar10;
LAB_100b4c27:
  iVar11 = __security_check_cookie(local_8 ^ (uint)local_51c);
  return iVar11;
}


// 100b4c3d __write

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
    pDVar1 = FUN_100ab333();
    *pDVar1 = 0;
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_100f5ce8)) {
      iVar2 = (_FileHandle & 0x1fU) * 0x38;
      if ((*(byte *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + iVar2) & 1) != 0) {
        ___lock_fhandle(_FileHandle);
        if ((*(byte *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_100ab320();
          *pDVar1 = 9;
          pDVar1 = FUN_100ab333();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = __write_nolock(_FileHandle,_Buf,_MaxCharCount);
        }
        FUN_100b4d0f();
        return local_20;
      }
    }
    pDVar1 = FUN_100ab333();
    *pDVar1 = 0;
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return -1;
}


// 100b4d0f FUN_100b4d0f

void FUN_100b4d0f(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 100b4d19 __getbuf

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */
/* Library Function - Single Match
    __getbuf
   
   Library: Visual Studio 2005 Release */

void __cdecl __getbuf(FILE *_File)

{
  char *pcVar1;
  
  _DAT_100f51ec = _DAT_100f51ec + 1;
  pcVar1 = (char *)FUN_10002dc0(0x1000);
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


// 100b4d5d __isatty

/* Library Function - Single Match
    __isatty
   
   Library: Visual Studio 2005 Release */

int __cdecl __isatty(int _FileHandle)

{
  DWORD *pDVar1;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
    return 0;
  }
  if ((-1 < _FileHandle) && ((uint)_FileHandle < DAT_100f5ce8)) {
    return *(byte *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38) & 0x40;
  }
  pDVar1 = FUN_100ab320();
  *pDVar1 = 9;
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return 0;
}


// 100b4dbb __wctomb_s_l

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
LAB_100b4ddd:
    DVar1 = 0;
  }
  else {
    if (_SizeConverted != (int *)0x0) {
      *_SizeConverted = -1;
    }
    if (0x7fffffff < _SizeInBytes) {
      pDVar2 = FUN_100ab320();
      *pDVar2 = 0x16;
      FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      return 0x16;
    }
    _LocaleUpdate::_LocaleUpdate((_LocaleUpdate *)local_14,_Locale);
    if (*(int *)(local_14[0] + 0x14) == 0) {
      if ((ushort)_WCh < 0x100) {
        if (lpMultiByteStr != (char *)0x0) {
          if (_Size == 0) goto LAB_100b4e6d;
          *lpMultiByteStr = (char)_WCh;
        }
        if (_SizeConverted != (int *)0x0) {
          *_SizeConverted = 1;
        }
LAB_100b4ea4:
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        goto LAB_100b4ddd;
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
LAB_100b4e6d:
          pDVar2 = FUN_100ab320();
          *pDVar2 = 0x22;
          FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        goto LAB_100b4ea4;
      }
    }
    pDVar2 = FUN_100ab320();
    *pDVar2 = 0x2a;
    pDVar2 = FUN_100ab320();
    DVar1 = *pDVar2;
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return DVar1;
}


// 100b4f1a _wctomb_s

/* Library Function - Single Match
    _wctomb_s
   
   Library: Visual Studio 2005 Release */

errno_t __cdecl _wctomb_s(int *_SizeConverted,char *_MbCh,rsize_t _SizeInBytes,wchar_t _WCh)

{
  errno_t eVar1;
  
  eVar1 = __wctomb_s_l(_SizeConverted,_MbCh,_SizeInBytes,_WCh,(_locale_t)0x0);
  return eVar1;
}


// 100b4f40 __aulldvrm

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


// 100b4fd5 FUN_100b4fd5

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

void FUN_100b4fd5(void)

{
  _DAT_100f5ce0 = 0;
  return;
}


// 100b4fdd __crtGetStringTypeA_stat

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  local_c = in_ECX;
  if (DAT_100f5918 == 0) {
    BVar2 = GetStringTypeW(1,L"",1,(LPWORD)&local_c);
    if (BVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_100f5918 = 2;
      }
      goto LAB_100b5036;
    }
    DAT_100f5918 = 1;
  }
  else {
LAB_100b5036:
    if ((DAT_100f5918 == 2) || (DAT_100f5918 == 0)) {
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
        FUN_10002ea0(uVar7);
      }
      goto LAB_100b5183;
    }
    if (DAT_100f5918 != 1) goto LAB_100b5183;
  }
  local_c = (int *)0x0;
  if (param_5 == (ushort *)0x0) {
    param_5 = *(ushort **)(*in_ECX + 4);
  }
  cchWideChar = MultiByteToWideChar((UINT)param_5,(uint)(param_7 != 0) * 8 + 1,(LPCSTR)param_2,
                                    (int)param_3,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_100b5183;
  lpWideCharStr = (LPCWSTR)0x0;
  if ((0 < (int)cchWideChar) && (cchWideChar < 0x7ffffff1)) {
    uVar1 = cchWideChar * 2 + 8;
    if (uVar1 < 0x401) {
      pWVar4 = (LPCWSTR)&stack0xffffffe8;
      lpWideCharStr = (LPCWSTR)&stack0xffffffe8;
      if (&stack0x00000000 != (undefined1 *)0x18) {
LAB_100b50c6:
        lpWideCharStr = pWVar4 + 4;
      }
    }
    else {
      pWVar4 = (LPCWSTR)FUN_10002dc0(uVar1);
      lpWideCharStr = pWVar4;
      if (pWVar4 != (LPCWSTR)0x0) {
        pWVar4[0] = L'\xdddd';
        pWVar4[1] = L'\0';
        goto LAB_100b50c6;
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
LAB_100b5183:
  iVar5 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar5;
}


// 100b5195 ___crtGetStringTypeA

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


// 100b51d5 ___free_lc_time

/* Library Function - Single Match
    ___free_lc_time
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lc_time(undefined4 *param_1)

{
  if (param_1 != (undefined4 *)0x0) {
    FUN_10002ea0(param_1[1]);
    FUN_10002ea0(param_1[2]);
    FUN_10002ea0(param_1[3]);
    FUN_10002ea0(param_1[4]);
    FUN_10002ea0(param_1[5]);
    FUN_10002ea0(param_1[6]);
    FUN_10002ea0(*param_1);
    FUN_10002ea0(param_1[8]);
    FUN_10002ea0(param_1[9]);
    FUN_10002ea0(param_1[10]);
    FUN_10002ea0(param_1[0xb]);
    FUN_10002ea0(param_1[0xc]);
    FUN_10002ea0(param_1[0xd]);
    FUN_10002ea0(param_1[7]);
    FUN_10002ea0(param_1[0xe]);
    FUN_10002ea0(param_1[0xf]);
    FUN_10002ea0(param_1[0x10]);
    FUN_10002ea0(param_1[0x11]);
    FUN_10002ea0(param_1[0x12]);
    FUN_10002ea0(param_1[0x13]);
    FUN_10002ea0(param_1[0x14]);
    FUN_10002ea0(param_1[0x15]);
    FUN_10002ea0(param_1[0x16]);
    FUN_10002ea0(param_1[0x17]);
    FUN_10002ea0(param_1[0x18]);
    FUN_10002ea0(param_1[0x19]);
    FUN_10002ea0(param_1[0x1a]);
    FUN_10002ea0(param_1[0x1b]);
    FUN_10002ea0(param_1[0x1c]);
    FUN_10002ea0(param_1[0x1d]);
    FUN_10002ea0(param_1[0x1e]);
    FUN_10002ea0(param_1[0x1f]);
    FUN_10002ea0(param_1[0x20]);
    FUN_10002ea0(param_1[0x21]);
    FUN_10002ea0(param_1[0x22]);
    FUN_10002ea0(param_1[0x23]);
    FUN_10002ea0(param_1[0x24]);
    FUN_10002ea0(param_1[0x25]);
    FUN_10002ea0(param_1[0x26]);
    FUN_10002ea0(param_1[0x27]);
    FUN_10002ea0(param_1[0x28]);
    FUN_10002ea0(param_1[0x29]);
    FUN_10002ea0(param_1[0x2a]);
  }
  return;
}


// 100b5365 ___free_lconv_num

/* Library Function - Single Match
    ___free_lconv_num
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_num(int *param_1)

{
  if (param_1 != (int *)0x0) {
    if ((undefined *)*param_1 != PTR_DAT_100dfd18) {
      FUN_10002ea0((undefined *)*param_1);
    }
    if ((undefined *)param_1[1] != PTR_DAT_100dfd1c) {
      FUN_10002ea0((undefined *)param_1[1]);
    }
    if ((undefined *)param_1[2] != PTR_DAT_100dfd20) {
      FUN_10002ea0((undefined *)param_1[2]);
    }
  }
  return;
}


// 100b53a5 ___free_lconv_mon

/* Library Function - Single Match
    ___free_lconv_mon
   
   Library: Visual Studio 2005 Release */

void __cdecl ___free_lconv_mon(int param_1)

{
  if (param_1 != 0) {
    if (*(undefined **)(param_1 + 0xc) != PTR_DAT_100dfd24) {
      FUN_10002ea0(*(undefined **)(param_1 + 0xc));
    }
    if (*(undefined **)(param_1 + 0x10) != PTR_DAT_100dfd28) {
      FUN_10002ea0(*(undefined **)(param_1 + 0x10));
    }
    if (*(undefined **)(param_1 + 0x14) != PTR_DAT_100dfd2c) {
      FUN_10002ea0(*(undefined **)(param_1 + 0x14));
    }
    if (*(undefined **)(param_1 + 0x18) != PTR_DAT_100dfd30) {
      FUN_10002ea0(*(undefined **)(param_1 + 0x18));
    }
    if (*(undefined **)(param_1 + 0x1c) != PTR_DAT_100dfd34) {
      FUN_10002ea0(*(undefined **)(param_1 + 0x1c));
    }
    if (*(undefined **)(param_1 + 0x20) != PTR_DAT_100dfd38) {
      FUN_10002ea0(*(undefined **)(param_1 + 0x20));
    }
    if (*(undefined **)(param_1 + 0x24) != PTR_DAT_100dfd3c) {
      FUN_10002ea0(*(undefined **)(param_1 + 0x24));
    }
  }
  return;
}


// 100b542e ____lc_codepage_func

/* Library Function - Single Match
    ____lc_codepage_func
   
   Libraries: Visual Studio 2005 Release, Visual Studio 2008 Release */

UINT __cdecl ____lc_codepage_func(void)

{
  _ptiddata p_Var1;
  pthreadlocinfo ptVar2;
  
  p_Var1 = __getptd();
  ptVar2 = p_Var1->ptlocinfo;
  if ((ptVar2 != (pthreadlocinfo)PTR_DAT_100df640) && ((p_Var1->_ownlocale & DAT_100df55c) == 0)) {
    ptVar2 = ___updatetlocinfo();
  }
  return ptVar2->lc_codepage;
}


// 100b5460 _strcspn

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


// 100b54b0 _strpbrk

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


// 100b54f0 ___ansicp

/* Library Function - Single Match
    ___ansicp
   
   Library: Visual Studio 2005 Release */

void __cdecl ___ansicp(LCID param_1)

{
  int iVar1;
  CHAR local_10 [6];
  undefined1 local_a;
  uint local_8;
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  local_a = 0;
  iVar1 = GetLocaleInfoA(param_1,0x1004,local_10,6);
  if (iVar1 != 0) {
    _atol(local_10);
  }
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 100b5537 ___convertcp

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  cbMultiByte = *param_4;
  bVar2 = false;
  if (param_1 == param_2) goto LAB_100b56d7;
  BVar3 = GetCPInfo(param_1,&local_1c);
  if ((((BVar3 == 0) || (local_1c.MaxCharSize != 1)) ||
      (BVar3 = GetCPInfo(param_2,&local_1c), BVar3 == 0)) || (local_1c.MaxCharSize != 1)) {
    uVar7 = MultiByteToWideChar(param_1,1,param_3,cbMultiByte,(LPWSTR)0x0,0);
    bVar8 = uVar7 == 0;
    if (bVar8) goto LAB_100b56d7;
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
LAB_100b5617:
        local_20 = pWVar5 + 4;
      }
    }
    else {
      pWVar5 = (LPCWSTR)FUN_10002dc0(uVar1);
      local_20 = pWVar5;
      if (pWVar5 != (LPCWSTR)0x0) {
        pWVar5[0] = L'\xdddd';
        pWVar5[1] = L'\0';
        goto LAB_100b5617;
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
           (lpMultiByteStr = (LPSTR)FUN_10002e20(1,uVar7), lpMultiByteStr != (LPSTR)0x0)) {
          uVar7 = WideCharToMultiByte(param_2,0,local_20,uVar7,lpMultiByteStr,uVar7,(LPCSTR)0x0,
                                      (LPBOOL)0x0);
          if (uVar7 == 0) {
            FUN_10002ea0(lpMultiByteStr);
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
LAB_100b56d7:
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 100b56f0 __allmul

/* Library Function - Single Match
    __allmul
   
   Library: Visual Studio 2005 Release */

longlong __allmul(uint param_1,int param_2,uint param_3,int param_4)

{
  if (param_4 == 0 && param_2 == 0) {
    return (ulonglong)param_1 * (ulonglong)param_3;
  }
  return CONCAT44((int)((ulonglong)param_1 * (ulonglong)param_3 >> 0x20) +
                  param_2 * param_3 + param_1 * param_4,
                  (int)((ulonglong)param_1 * (ulonglong)param_3));
}


// 100b5724 __ld12tod

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
        goto LAB_100b5c23;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_100dfd68 - 1;
    iVar4 = (int)(DAT_100dfd68 + ((int)DAT_100dfd68 >> 0x1f & 0x1fU)) >> 5;
    uVar8 = DAT_100dfd68 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar6) goto LAB_100b5857;
          bVar14 = _Ifp < p_Var9;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_100b585e;
LAB_100b5857:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_100b585e:
              iVar4 = iVar4 + -1;
              *puVar6 = (uint)_Ifp;
              if ((iVar4 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_100b586c;
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
LAB_100b586c:
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
    if (iVar12 < (int)(DAT_100dfd64 - DAT_100dfd68)) {
      local_24[0] = 0;
      local_24[1] = 0;
    }
    else {
      if (DAT_100dfd64 < iVar12) {
        if (iVar12 < DAT_100dfd60) {
          local_24[0] = local_24[0] & 0x7fffffff;
          iVar12 = iVar12 + DAT_100dfd74;
          iVar2 = (int)(DAT_100dfd6c + ((int)DAT_100dfd6c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_100dfd6c & 0x8000001f;
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
          iVar12 = (int)(DAT_100dfd6c + ((int)DAT_100dfd6c >> 0x1f & 0x1fU)) >> 5;
          uVar10 = DAT_100dfd6c & 0x8000001f;
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
          iVar12 = DAT_100dfd74 + DAT_100dfd60;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_100b5c23;
      }
      local_14 = DAT_100dfd64 - local_14;
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
      iVar2 = DAT_100dfd68 - 1;
      iVar12 = (int)(DAT_100dfd68 + ((int)DAT_100dfd68 >> 0x1f & 0x1fU)) >> 5;
      uVar10 = DAT_100dfd68 & 0x8000001f;
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
          if (2 < iVar12) goto LAB_100b5a0f;
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
LAB_100b5a0f:
      *puVar7 = *puVar7 & -1 << ((byte)local_14 & 0x1f);
      iVar12 = local_10 + 1;
      if (iVar12 < 3) {
        puVar7 = local_24 + iVar12;
        for (iVar2 = 3 - iVar12; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar7 = 0;
          puVar7 = puVar7 + 1;
        }
      }
      uVar10 = DAT_100dfd6c + 1;
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
LAB_100b5c23:
  local_24[0] = iVar12 << (0x1fU - (char)DAT_100dfd6c & 0x1f) |
                -(uint)(local_24[3] != 0) & 0x80000000 | local_24[0];
  if (DAT_100dfd70 == 0x40) {
    *(uint *)((int)&_D->x + 4) = local_24[0];
    *(uint *)&_D->x = local_24[1];
  }
  else if (DAT_100dfd70 == 0x20) {
    *(uint *)&_D->x = local_24[0];
  }
  return IVar3;
}


// 100b5c66 __ld12tof

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
        goto LAB_100b6165;
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < 3);
    IVar3 = INTRNCVT_OK;
  }
  else {
    _Ifp = (_LDBL12 *)0x0;
    iVar13 = DAT_100dfd80 - 1;
    iVar2 = (int)(DAT_100dfd80 + ((int)DAT_100dfd80 >> 0x1f & 0x1fU)) >> 5;
    uVar7 = DAT_100dfd80 & 0x8000001f;
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
          if (_Ifp < (_LDBL12 *)*puVar5) goto LAB_100b5d99;
          bVar14 = _Ifp < p_Var8;
          do {
            local_8 = (_LDBL12 *)0x0;
            if (!bVar14) goto LAB_100b5da0;
LAB_100b5d99:
            do {
              local_8 = (_LDBL12 *)0x1;
LAB_100b5da0:
              iVar2 = iVar2 + -1;
              *puVar5 = (uint)_Ifp;
              if ((iVar2 < 0) || (local_8 == (_LDBL12 *)0x0)) {
                _Ifp = local_8;
                goto LAB_100b5dae;
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
LAB_100b5dae:
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
    if (iVar11 < (int)(DAT_100dfd7c - DAT_100dfd80)) {
      local_24 = 0;
      local_20 = 0.0;
    }
    else {
      if (DAT_100dfd7c < iVar11) {
        if (iVar11 < DAT_100dfd78) {
          local_24 = local_24 & 0x7fffffff;
          iVar11 = iVar11 + DAT_100dfd8c;
          iVar2 = (int)(DAT_100dfd84 + ((int)DAT_100dfd84 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_100dfd84 & 0x8000001f;
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
          iVar11 = (int)(DAT_100dfd84 + ((int)DAT_100dfd84 >> 0x1f & 0x1fU)) >> 5;
          uVar9 = DAT_100dfd84 & 0x8000001f;
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
          iVar11 = DAT_100dfd8c + DAT_100dfd78;
          IVar3 = INTRNCVT_OVERFLOW;
        }
        goto LAB_100b6165;
      }
      local_1c[2] = DAT_100dfd7c - local_1c[2];
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
      iVar2 = DAT_100dfd80 - 1;
      iVar11 = (int)(DAT_100dfd80 + ((int)DAT_100dfd80 >> 0x1f & 0x1fU)) >> 5;
      uVar9 = DAT_100dfd80 & 0x8000001f;
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
          if (2 < iVar11) goto LAB_100b5f51;
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
LAB_100b5f51:
      *puVar6 = *puVar6 & -1 << ((byte)local_1c[2] & 0x1f);
      iVar11 = local_1c[3] + 1;
      if (iVar11 < 3) {
        puVar6 = &local_24 + iVar11;
        for (iVar2 = 3 - iVar11; iVar2 != 0; iVar2 = iVar2 + -1) {
          *puVar6 = 0;
          puVar6 = puVar6 + 1;
        }
      }
      uVar9 = DAT_100dfd84 + 1;
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
LAB_100b6165:
  fVar12 = (float)(iVar11 << (0x1fU - (char)DAT_100dfd84 & 0x1f) |
                   -(uint)(local_1c[1] != 0) & 0x80000000 | local_24);
  if (DAT_100dfd88 == 0x40) {
    _F[1].f = fVar12;
    _F->f = local_20;
  }
  else if (DAT_100dfd88 == 0x20) {
    _F->f = fVar12;
  }
  return IVar3;
}


// 100b61a8 ___mtold12

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
  
  uVar7 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
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


// 100b6374 __87except

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
  
  local_14 = DAT_100df7a0 ^ (uint)&local_94;
  local_94 = (uint)*param_3;
  iVar2 = *param_2;
  if (iVar2 == 1) {
LAB_100b63ce:
    uVar3 = 8;
LAB_100b63d0:
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
      goto LAB_100b63d0;
    }
    if (iVar2 == 3) {
      uVar3 = 0x11;
      goto LAB_100b63d0;
    }
    if (iVar2 == 4) {
      uVar3 = 0x12;
      goto LAB_100b63d0;
    }
    if (iVar2 == 5) goto LAB_100b63ce;
    if (iVar2 == 7) {
      *param_2 = 1;
    }
    else if (iVar2 == 8) {
      uVar3 = 0x10;
      goto LAB_100b63d0;
    }
  }
  __ctrlfp();
  if ((*param_2 != 8) && (DAT_100e004c == 0)) {
    iVar2 = FUN_100b6831();
    if (iVar2 != 0) goto LAB_100b645b;
  }
  __set_errno_from_matherr(*param_2);
LAB_100b645b:
  __security_check_cookie(local_14 ^ (uint)&local_94);
  return;
}


// 100b6470 __frnd

/* Library Function - Single Match
    __frnd
   
   Library: Visual Studio 2005 Release */

float10 __cdecl __frnd(double param_1)

{
  return (float10)ROUND(param_1);
}


// 100b6481 __ungetc_nolock

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
      puVar4 = &DAT_100df6e0;
    }
    else {
      iVar2 = __fileno(_File);
      uVar3 = __fileno(_File);
      puVar4 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_100f5d00)[iVar2 >> 5]);
    }
    if ((puVar4[0x24] & 0x7f) == 0) {
      iVar2 = __fileno(_File);
      if ((iVar2 == -1) || (iVar2 = __fileno(_File), iVar2 == -2)) {
        puVar4 = &DAT_100df6e0;
      }
      else {
        iVar2 = __fileno(_File);
        uVar3 = __fileno(_File);
        puVar4 = (undefined *)((uVar3 & 0x1f) * 0x38 + (&DAT_100f5d00)[iVar2 >> 5]);
      }
      if ((puVar4[0x24] & 0x80) == 0) goto LAB_100b653e;
    }
    pDVar5 = FUN_100ab320();
    *pDVar5 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
LAB_100b653e:
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


// 100b659f __mbtowc_l

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
          if (iVar2 != 0) goto LAB_100b65ea;
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
        pDVar3 = FUN_100ab320();
        *pDVar3 = 0x2a;
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        return -1;
      }
      if (_DstCh != (wchar_t *)0x0) {
        *_DstCh = (ushort)(byte)*_SrcCh;
      }
LAB_100b65ea:
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


// 100b66b2 _mbtowc

/* Library Function - Single Match
    _mbtowc
   
   Library: Visual Studio 2005 Release */

int __cdecl _mbtowc(wchar_t *_DstCh,char *_SrcCh,size_t _SrcSizeInBytes)

{
  int iVar1;
  
  iVar1 = __mbtowc_l(_DstCh,_SrcCh,_SrcSizeInBytes,(_locale_t)0x0);
  return iVar1;
}


// 100b66c9 x_ismbbtype_l

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
    if (uVar1 == 0) goto LAB_100b670b;
  }
  uVar1 = 1;
LAB_100b670b:
  if (local_8 != '\0') {
    *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
  }
  return uVar1;
}


// 100b671a __ismbblead

/* Library Function - Single Match
    __ismbblead
   
   Library: Visual Studio 2005 Release */

int __cdecl __ismbblead(uint _C)

{
  int iVar1;
  
  iVar1 = x_ismbbtype_l((localeinfo_struct *)0x0,_C,0,4);
  return iVar1;
}


// 100b672d ___report_gsfailure

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
  
  _DAT_100f5a38 =
       (uint)(in_NT & 1) * 0x4000 | (uint)SBORROW4((int)&stack0xfffffffc,0x328) * 0x800 |
       (uint)(in_IF & 1) * 0x200 | (uint)(in_TF & 1) * 0x100 | (uint)((int)&local_32c < 0) * 0x80 |
       (uint)(&stack0x00000000 == (undefined1 *)0x32c) * 0x40 | (uint)(in_AF & 1) * 0x10 |
       (uint)((POPCOUNT((uint)&local_32c & 0xff) & 1U) == 0) * 4 |
       (uint)(&stack0xfffffffc < (undefined1 *)0x328) | (uint)(in_ID & 1) * 0x200000 |
       (uint)(in_VIP & 1) * 0x100000 | (uint)(in_VIF & 1) * 0x80000 | (uint)(in_AC & 1) * 0x40000;
  _DAT_100f5a3c = &stack0x00000004;
  _DAT_100f5978 = 0x10001;
  _DAT_100f5920 = 0xc0000409;
  _DAT_100f5924 = 1;
  local_32c = DAT_100df7a0;
  local_328 = DAT_100df7a4;
  _DAT_100f592c = unaff_retaddr;
  _DAT_100f5a04 = in_GS;
  _DAT_100f5a08 = in_FS;
  _DAT_100f5a0c = in_ES;
  _DAT_100f5a10 = in_DS;
  _DAT_100f5a14 = unaff_EDI;
  _DAT_100f5a18 = unaff_ESI;
  _DAT_100f5a1c = unaff_EBX;
  _DAT_100f5a20 = in_EDX;
  _DAT_100f5a24 = in_ECX;
  _DAT_100f5a28 = in_EAX;
  _DAT_100f5a2c = unaff_EBP;
  DAT_100f5a30 = unaff_retaddr;
  _DAT_100f5a34 = in_CS;
  _DAT_100f5a40 = in_SS;
  DAT_100f5970 = IsDebuggerPresent();
  FUN_100b4fd5();
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)0x0);
  UnhandledExceptionFilter((_EXCEPTION_POINTERS *)&PTR_DAT_100d662c);
  if (DAT_100f5970 == 0) {
    FUN_100b4fd5();
  }
  uExitCode = 0xc0000409;
  hProcess = GetCurrentProcess();
  TerminateProcess(hProcess,uExitCode);
  return;
}


// 100b6831 FUN_100b6831

undefined4 FUN_100b6831(void)

{
  return 0;
}


// 100b6834 __set_osfhnd

/* Library Function - Single Match
    __set_osfhnd
   
   Library: Visual Studio 2005 Release */

int __cdecl __set_osfhnd(int param_1,intptr_t param_2)

{
  DWORD *pDVar1;
  int iVar2;
  DWORD nStdHandle;
  
  if ((-1 < param_1) && ((uint)param_1 < DAT_100f5ce8)) {
    iVar2 = (param_1 & 0x1fU) * 0x38;
    if (*(int *)(iVar2 + (&DAT_100f5d00)[param_1 >> 5]) == -1) {
      if (DAT_100f51e4 == 1) {
        if (param_1 == 0) {
          nStdHandle = 0xfffffff6;
        }
        else if (param_1 == 1) {
          nStdHandle = 0xfffffff5;
        }
        else {
          if (param_1 != 2) goto LAB_100b688e;
          nStdHandle = 0xfffffff4;
        }
        SetStdHandle(nStdHandle,(HANDLE)param_2);
      }
LAB_100b688e:
      *(intptr_t *)(iVar2 + (&DAT_100f5d00)[param_1 >> 5]) = param_2;
      return 0;
    }
  }
  pDVar1 = FUN_100ab320();
  *pDVar1 = 9;
  pDVar1 = FUN_100ab333();
  *pDVar1 = 0;
  return -1;
}


// 100b68b1 __free_osfhnd

/* Library Function - Single Match
    __free_osfhnd
   
   Library: Visual Studio 2005 Release */

int __cdecl __free_osfhnd(int param_1)

{
  int *piVar1;
  DWORD *pDVar2;
  int iVar3;
  DWORD nStdHandle;
  
  if ((-1 < param_1) && ((uint)param_1 < DAT_100f5ce8)) {
    iVar3 = (param_1 & 0x1fU) * 0x38;
    piVar1 = (int *)((&DAT_100f5d00)[param_1 >> 5] + iVar3);
    if (((*(byte *)(piVar1 + 1) & 1) != 0) && (*piVar1 != -1)) {
      if (DAT_100f51e4 == 1) {
        if (param_1 == 0) {
          nStdHandle = 0xfffffff6;
        }
        else if (param_1 == 1) {
          nStdHandle = 0xfffffff5;
        }
        else {
          if (param_1 != 2) goto LAB_100b690f;
          nStdHandle = 0xfffffff4;
        }
        SetStdHandle(nStdHandle,(HANDLE)0x0);
      }
LAB_100b690f:
      *(undefined4 *)(iVar3 + (&DAT_100f5d00)[param_1 >> 5]) = 0xffffffff;
      return 0;
    }
  }
  pDVar2 = FUN_100ab320();
  *pDVar2 = 9;
  pDVar2 = FUN_100ab333();
  *pDVar2 = 0;
  return -1;
}


// 100b6932 __get_osfhandle

/* Library Function - Single Match
    __get_osfhandle
   
   Library: Visual Studio 2005 Release */

intptr_t __cdecl __get_osfhandle(int _FileHandle)

{
  DWORD *pDVar1;
  intptr_t *piVar2;
  
  if (_FileHandle == -2) {
    pDVar1 = FUN_100ab333();
    *pDVar1 = 0;
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
    return -1;
  }
  if (((-1 < _FileHandle) && ((uint)_FileHandle < DAT_100f5ce8)) &&
     (piVar2 = (intptr_t *)((_FileHandle & 0x1fU) * 0x38 + (&DAT_100f5d00)[_FileHandle >> 5]),
     (*(byte *)(piVar2 + 1) & 1) != 0)) {
    return *piVar2;
  }
  pDVar1 = FUN_100ab333();
  *pDVar1 = 0;
  pDVar1 = FUN_100ab320();
  *pDVar1 = 9;
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 100b69a3 ___lock_fhandle

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
  
  iVar2 = (_Filehandle & 0x1fU) * 0x38 + (&DAT_100f5d00)[_Filehandle >> 5];
  local_20 = 1;
  if (*(int *)(iVar2 + 8) == 0) {
    __lock(10);
    if (*(int *)(iVar2 + 8) == 0) {
      iVar1 = ___crtInitCritSecAndSpinCount(iVar2 + 0xc,4000);
      local_20 = (uint)(iVar1 != 0);
      *(int *)(iVar2 + 8) = *(int *)(iVar2 + 8) + 1;
    }
    FUN_100b6a3a();
  }
  if (local_20 != 0) {
    EnterCriticalSection
              ((LPCRITICAL_SECTION)
               ((&DAT_100f5d00)[_Filehandle >> 5] + 0xc + (_Filehandle & 0x1fU) * 0x38));
  }
  return local_20;
}


// 100b6a3a FUN_100b6a3a

void FUN_100b6a3a(void)

{
  FUN_100afa6b(10);
  return;
}


// 100b6a43 __unlock_fhandle

/* Library Function - Single Match
    __unlock_fhandle
   
   Library: Visual Studio 2005 Release */

void __cdecl __unlock_fhandle(int _Filehandle)

{
  LeaveCriticalSection
            ((LPCRITICAL_SECTION)
             ((&DAT_100f5d00)[_Filehandle >> 5] + 0xc + (_Filehandle & 0x1fU) * 0x38));
  return;
}


// 100b6a65 __alloc_osfhnd

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
      puVar3 = (undefined4 *)(&DAT_100f5d00)[iVar4];
      if (puVar3 == (undefined4 *)0x0) {
        puVar3 = (undefined4 *)FUN_10002e20(0x20,0x38);
        if (puVar3 != (undefined4 *)0x0) {
          (&DAT_100f5d00)[iVar4] = puVar3;
          DAT_100f5ce8 = DAT_100f5ce8 + 0x20;
          for (; puVar3 < (undefined4 *)((&DAT_100f5d00)[iVar4] + 0x700); puVar3 = puVar3 + 0xe) {
            *(undefined1 *)(puVar3 + 1) = 0;
            *puVar3 = 0xffffffff;
            *(undefined1 *)((int)puVar3 + 5) = 10;
            puVar3[2] = 0;
          }
          local_20 = iVar4 << 5;
          *(undefined1 *)((&DAT_100f5d00)[local_20 >> 5] + 4) = 1;
          iVar2 = ___lock_fhandle(local_20);
          if (iVar2 == 0) {
            local_20 = -1;
          }
        }
        break;
      }
      for (; puVar3 < (undefined4 *)((&DAT_100f5d00)[iVar4] + 0x700); puVar3 = puVar3 + 0xe) {
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
            FUN_100b6b38();
          }
          if (!bVar1) {
            EnterCriticalSection((LPCRITICAL_SECTION)(puVar3 + 3));
            if ((*(byte *)(puVar3 + 1) & 1) == 0) {
              *(undefined1 *)(puVar3 + 1) = 1;
              *puVar3 = 0xffffffff;
              local_20 = ((int)puVar3 - (&DAT_100f5d00)[iVar4]) / 0x38 + iVar4 * 0x20;
              break;
            }
            LeaveCriticalSection((LPCRITICAL_SECTION)(puVar3 + 3));
          }
        }
      }
      if (local_20 != -1) break;
    }
    FUN_100b6bfb();
  }
  return local_20;
}


// 100b6b38 FUN_100b6b38

void FUN_100b6b38(void)

{
  FUN_100afa6b(10);
  return;
}


// 100b6bfb FUN_100b6bfb

void FUN_100b6bfb(void)

{
  FUN_100afa6b(0xb);
  return;
}


// 100b6c04 FUN_100b6c04

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

DWORD __cdecl FUN_100b6c04(uint param_1)

{
  DWORD *pDVar1;
  HANDLE hFile;
  BOOL BVar2;
  int iVar3;
  DWORD local_20;
  
  if (param_1 == 0xfffffffe) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
  }
  else {
    if ((-1 < (int)param_1) && (param_1 < DAT_100f5ce8)) {
      iVar3 = (param_1 & 0x1f) * 0x38;
      if ((*(byte *)(iVar3 + 4 + (&DAT_100f5d00)[(int)param_1 >> 5]) & 1) != 0) {
        ___lock_fhandle(param_1);
        if ((*(byte *)(iVar3 + 4 + (&DAT_100f5d00)[(int)param_1 >> 5]) & 1) != 0) {
          hFile = (HANDLE)__get_osfhandle(param_1);
          BVar2 = FlushFileBuffers(hFile);
          if (BVar2 == 0) {
            local_20 = GetLastError();
          }
          else {
            local_20 = 0;
          }
          if (local_20 == 0) goto LAB_100b6cc6;
          pDVar1 = FUN_100ab333();
          *pDVar1 = local_20;
        }
        pDVar1 = FUN_100ab320();
        *pDVar1 = 9;
        local_20 = 0xffffffff;
LAB_100b6cc6:
        FUN_100b6cdb();
        return local_20;
      }
    }
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  return 0xffffffff;
}


// 100b6cdb FUN_100b6cdb

void FUN_100b6cdb(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 100b6ce5 FUN_100b6ce5

int __cdecl FUN_100b6ce5(uint param_1,LPWSTR param_2,LPWSTR param_3)

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
    pDVar5 = FUN_100ab333();
    *pDVar5 = 0;
    pDVar5 = FUN_100ab320();
    *pDVar5 = 9;
    return -1;
  }
  if (((int)param_1 < 0) || (DAT_100f5ce8 <= param_1)) {
    pDVar5 = FUN_100ab333();
    *pDVar5 = 0;
    pDVar5 = FUN_100ab320();
    *pDVar5 = 9;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return -1;
  }
  iVar12 = (param_1 & 0x1f) * 0x38;
  piVar1 = &DAT_100f5d00 + ((int)param_1 >> 5);
  bVar3 = *(byte *)(*piVar1 + iVar12 + 4);
  if ((bVar3 & 1) == 0) {
    pDVar5 = FUN_100ab333();
    *pDVar5 = 0;
    pDVar5 = FUN_100ab320();
    *pDVar5 = 9;
    goto LAB_100b6ded;
  }
  if (param_3 < (LPWSTR)0x80000000) {
    local_14 = (LPWSTR)0x0;
    if ((param_3 == (LPWSTR)0x0) || ((bVar3 & 2) != 0)) {
      return 0;
    }
    if (param_2 != (LPWSTR)0x0) {
      local_6 = (char)(*(char *)(*piVar1 + iVar12 + 0x24) * '\x02') >> 1;
      if (local_6 == '\x01') {
        if ((~(uint)param_3 & 1) == 0) goto LAB_100b6ddb;
        pWVar9 = (LPWSTR)((uint)param_3 >> 1);
        param_3 = (LPWSTR)0x4;
        if ((LPWSTR)0x3 < pWVar9) {
          param_3 = pWVar9;
        }
        local_10 = (LPWSTR)FUN_10002dc0(param_3);
        if (local_10 == (LPWSTR)0x0) {
          pDVar5 = FUN_100ab320();
          *pDVar5 = 0xc;
          pDVar5 = FUN_100ab333();
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
          if ((~(uint)param_3 & 1) == 0) goto LAB_100b6ddb;
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
            goto LAB_100b70fa;
          }
          goto LAB_100b70ef;
        }
        pDVar5 = FUN_100ab320();
        *pDVar5 = 9;
        pDVar5 = FUN_100ab333();
        *pDVar5 = 5;
      }
      else {
        local_14 = (LPWSTR)((int)local_14 + (int)local_1c);
        pbVar2 = (byte *)(iVar12 + 4 + *piVar1);
        if ((*pbVar2 & 0x80) == 0) goto LAB_100b70fa;
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
                    goto LAB_100b719d;
                  }
LAB_100b7232:
                  param_3 = param_3 + 1;
                  *pWVar11 = L'\r';
                }
                else {
                  pWVar9 = param_3 + 1;
                  BVar7 = ReadFile(*(HANDLE *)(iVar12 + *piVar1),&local_c,2,(LPDWORD)&local_1c,
                                   (LPOVERLAPPED)0x0);
                  if (((BVar7 == 0) && (DVar8 = GetLastError(), DVar8 != 0)) ||
                     (local_1c == (LPWSTR)0x0)) goto LAB_100b7232;
                  if ((*(byte *)(iVar12 + 4 + *piVar1) & 0x48) == 0) {
                    if ((pWVar11 == local_10) && (local_c == 10)) goto LAB_100b719d;
                    __lseeki64_nolock(param_1,0x1ffffffff,unaff_EDI);
                    if (local_c == 10) goto LAB_100b7239;
                    goto LAB_100b7232;
                  }
                  if (local_c == 10) {
LAB_100b719d:
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
LAB_100b7239:
              param_3 = pWVar9;
            } while (param_3 < local_14);
          }
          local_14 = (LPWSTR)((int)pWVar11 - (int)local_10);
          goto LAB_100b70fa;
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
                  goto LAB_100b6f7a;
                }
LAB_100b6ff1:
                param_3 = (LPWSTR)((int)param_3 + 1);
                *(byte *)pWVar9 = 0xd;
              }
              else {
                pWVar6 = (LPWSTR)((int)param_3 + 1);
                BVar7 = ReadFile(*(HANDLE *)(iVar12 + *piVar1),&local_5,1,(LPDWORD)&local_1c,
                                 (LPOVERLAPPED)0x0);
                if (((BVar7 == 0) && (DVar8 = GetLastError(), DVar8 != 0)) ||
                   (local_1c == (LPWSTR)0x0)) goto LAB_100b6ff1;
                if ((*(byte *)(iVar12 + 4 + *piVar1) & 0x48) == 0) {
                  if ((pWVar9 == local_10) && (local_5 == '\n')) goto LAB_100b6f7a;
                  __lseeki64_nolock(param_1,0x1ffffffff,unaff_EDI);
                  if (local_5 == '\n') goto LAB_100b6ff5;
                  goto LAB_100b6ff1;
                }
                if (local_5 == '\n') {
LAB_100b6f7a:
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
LAB_100b6ff5:
            param_3 = pWVar6;
          } while (param_3 < local_14);
        }
        local_14 = (LPWSTR)((int)pWVar9 - (int)local_10);
        if ((local_6 != '\x01') || (local_14 == (LPWSTR)0x0)) goto LAB_100b70fa;
        bVar3 = *(byte *)((int)pWVar9 + -1);
        if ((char)bVar3 < '\0') {
          iVar10 = 1;
          pWVar9 = (LPWSTR)((int)pWVar9 + -1);
          while ((((&DAT_100e0050)[bVar3] == '\0' && (iVar10 < 5)) && (local_10 <= pWVar9))) {
            pWVar9 = (LPWSTR)((int)pWVar9 + -1);
            bVar3 = *(byte *)pWVar9;
            iVar10 = iVar10 + 1;
          }
          if ((char)(&DAT_100e0050)[(byte)*pWVar9] == 0) {
            pDVar5 = FUN_100ab320();
            *pDVar5 = 0x2a;
            goto LAB_100b70f6;
          }
          if ((char)(&DAT_100e0050)[(byte)*pWVar9] + 1 == iVar10) {
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
          goto LAB_100b70fa;
        }
        DVar8 = GetLastError();
LAB_100b70ef:
        FUN_100ab346(DVar8);
      }
LAB_100b70f6:
      local_18 = -1;
LAB_100b70fa:
      if (local_10 != param_2) {
        FUN_10002ea0(local_10);
      }
      if (local_18 == -2) {
        return (int)local_14;
      }
      return local_18;
    }
  }
LAB_100b6ddb:
  pDVar5 = FUN_100ab333();
  *pDVar5 = 0;
  pDVar5 = FUN_100ab320();
  *pDVar5 = 0x16;
LAB_100b6ded:
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 100b72a6 FUN_100b72a6

/* WARNING: Function: __SEH_prolog4 replaced with injection: SEH_prolog4 */
/* WARNING: Function: __SEH_epilog4 replaced with injection: EH_epilog3 */

int __cdecl FUN_100b72a6(uint param_1,LPWSTR param_2,LPWSTR param_3)

{
  DWORD *pDVar1;
  int iVar2;
  int local_20;
  
  if (param_1 == 0xfffffffe) {
    pDVar1 = FUN_100ab333();
    *pDVar1 = 0;
    pDVar1 = FUN_100ab320();
    *pDVar1 = 9;
    return -1;
  }
  if ((-1 < (int)param_1) && (param_1 < DAT_100f5ce8)) {
    iVar2 = (param_1 & 0x1f) * 0x38;
    if ((*(byte *)((&DAT_100f5d00)[(int)param_1 >> 5] + 4 + iVar2) & 1) != 0) {
      if (param_3 < (LPWSTR)0x80000000) {
        ___lock_fhandle(param_1);
        if ((*(byte *)((&DAT_100f5d00)[(int)param_1 >> 5] + 4 + iVar2) & 1) == 0) {
          pDVar1 = FUN_100ab320();
          *pDVar1 = 9;
          pDVar1 = FUN_100ab333();
          *pDVar1 = 0;
          local_20 = -1;
        }
        else {
          local_20 = FUN_100b6ce5(param_1,param_2,param_3);
        }
        FUN_100b7399();
        return local_20;
      }
      pDVar1 = FUN_100ab333();
      *pDVar1 = 0;
      pDVar1 = FUN_100ab320();
      *pDVar1 = 0x16;
      goto LAB_100b72f5;
    }
  }
  pDVar1 = FUN_100ab333();
  *pDVar1 = 0;
  pDVar1 = FUN_100ab320();
  *pDVar1 = 9;
LAB_100b72f5:
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  return -1;
}


// 100b7399 FUN_100b7399

void FUN_100b7399(void)

{
  int unaff_EBP;
  
  __unlock_fhandle(*(int *)(unaff_EBP + 8));
  return;
}


// 100b73a3 FUN_100b73a3

DWORD __cdecl FUN_100b73a3(undefined4 *param_1,LPCSTR param_2,uint param_3,int param_4,byte param_5)

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
  iVar3 = FUN_100b919f(local_28);
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
        goto LAB_100b748c;
      }
    }
    else if (uVar4 != 2) goto LAB_100b7448;
    local_c = 0xc0000000;
  }
LAB_100b748c:
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
    if (param_4 != 0x80) goto LAB_100b7448;
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
      if (uVar4 == 0x200) goto LAB_100b7596;
      if (uVar4 != 0x300) goto LAB_100b7448;
      local_18 = 2;
    }
  }
  else {
    if (uVar4 != 0x500) {
      if (uVar4 == 0x600) {
LAB_100b7596:
        local_18 = 5;
        goto LAB_100b7540;
      }
      if (uVar4 != 0x700) {
LAB_100b7448:
        pDVar5 = FUN_100ab333();
        *pDVar5 = 0;
        *in_EAX = 0xffffffff;
        pDVar5 = FUN_100ab320();
        *pDVar5 = 0x16;
        FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
        return 0x16;
      }
    }
    local_18 = 1;
  }
LAB_100b7540:
  local_10 = 0x80;
  if (((param_3 & 0x100) != 0) && (-1 < (char)(~(byte)DAT_100f51f0 & param_5))) {
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
    pDVar5 = FUN_100ab333();
    *pDVar5 = 0;
    *in_EAX = 0xffffffff;
    pDVar5 = FUN_100ab320();
    *pDVar5 = 0x18;
    goto LAB_100b765e;
  }
  *param_1 = 1;
  local_20 = CreateFileA(param_2,local_c,local_14,&local_38,local_18,local_10,(HANDLE)0x0);
  if (local_20 == (HANDLE)0xffffffff) {
    if (((local_c & 0xc0000000) == 0xc0000000) && ((param_3 & 1) != 0)) {
      local_c = local_c & 0x7fffffff;
      local_20 = CreateFileA(param_2,local_c,local_14,&local_38,local_18,local_10,(HANDLE)0x0);
      if (local_20 != (HANDLE)0xffffffff) goto LAB_100b766a;
    }
    pbVar1 = (byte *)((&DAT_100f5d00)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38);
    *pbVar1 = *pbVar1 & 0xfe;
    DVar6 = GetLastError();
    FUN_100ab346(DVar6);
    goto LAB_100b765e;
  }
LAB_100b766a:
  DVar6 = GetFileType(local_20);
  if (DVar6 == 0) {
    pbVar1 = (byte *)((&DAT_100f5d00)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38);
    *pbVar1 = *pbVar1 & 0xfe;
    DVar6 = GetLastError();
    FUN_100ab346(DVar6);
    CloseHandle(local_20);
    if (DVar6 == 0) {
      pDVar5 = FUN_100ab320();
      *pDVar5 = 0xd;
    }
    goto LAB_100b765e;
  }
  if (DVar6 == 2) {
    local_5 = local_5 | 0x40;
  }
  else if (DVar6 == 3) {
    local_5 = local_5 | 8;
  }
  __set_osfhnd(*in_EAX,(intptr_t)local_20);
  bVar10 = local_5 | 1;
  *(byte *)((&DAT_100f5d00)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38) = bVar10;
  pbVar1 = (byte *)((&DAT_100f5d00)[(int)*in_EAX >> 5] + 0x24 + (*in_EAX & 0x1f) * 0x38);
  *pbVar1 = *pbVar1 & 0x80;
  local_7 = local_5 & 0x48;
  if (local_7 == 0) {
    bVar2 = local_5 & 0x80;
    local_5 = bVar10;
    if (bVar2 == 0) goto LAB_100b7874;
    if ((param_3 & 2) == 0) goto LAB_100b77a4;
    local_1c = __lseek_nolock(*in_EAX,-1,2);
    if (local_1c == 0xffffffff) {
      pDVar5 = FUN_100ab333();
      bVar10 = local_5;
      if (*pDVar5 == 0x83) goto LAB_100b77a4;
    }
    else {
      local_8 = '\0';
      iVar3 = FUN_100b6ce5(*in_EAX,(LPWSTR)&local_8,(LPWSTR)0x1);
      if ((((iVar3 != 0) || (local_8 != '\x1a')) ||
          (iVar3 = __chsize_nolock(*in_EAX,CONCAT44(unaff_EDI,(int)local_1c >> 0x1f)), iVar3 != -1))
         && (lVar8 = __lseek_nolock(*in_EAX,0,0), bVar10 = local_5, lVar8 != -1)) goto LAB_100b77a4;
    }
LAB_100b7757:
    __close_nolock(*in_EAX);
    goto LAB_100b765e;
  }
LAB_100b77a4:
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
      if ((param_3 & 0x301) == 0x301) goto LAB_100b7813;
    }
    else if ((uVar4 == 0x20000) || (uVar4 == 0x24000)) {
LAB_100b7813:
      local_6 = 2;
    }
    else if ((uVar4 == 0x40000) || (uVar4 == 0x44000)) {
      local_6 = 1;
    }
    if (((param_3 & 0x70000) != 0) && (local_1c = 0, (local_5 & 0x40) == 0)) {
      uVar4 = local_c & 0xc0000000;
      if (uVar4 == 0x40000000) {
        if (local_18 == 0) goto LAB_100b7874;
        if (2 < local_18) {
          if (local_18 < 5) {
            lVar12 = __lseeki64_nolock(*in_EAX,0x200000000,unaff_EDI);
            if (lVar12 == 0) goto LAB_100b7a76;
            lVar12 = __lseeki64_nolock(*in_EAX,0,unaff_EDI);
            uVar4 = (uint)lVar12 & (uint)((ulonglong)lVar12 >> 0x20);
            goto LAB_100b7a0c;
          }
LAB_100b786b:
          if (local_18 != 5) goto LAB_100b7874;
        }
LAB_100b7a76:
        iVar3 = 0;
        if (local_6 == 1) {
          local_1c = 0xbfbbef;
          local_18 = 3;
        }
        else {
          if (local_6 != 2) goto LAB_100b7874;
          local_1c = 0xfeff;
          local_18 = 2;
        }
        do {
          iVar9 = __write(*in_EAX,(void *)((int)&local_1c + iVar3),local_18 - iVar3);
          if (iVar9 == -1) {
            __close_nolock(*in_EAX);
            pDVar5 = FUN_100ab320();
            return *pDVar5;
          }
          iVar3 = iVar3 + iVar9;
        } while (iVar3 < (int)local_18);
      }
      else {
        if (uVar4 != 0x80000000) {
          if ((uVar4 == 0xc0000000) && (local_18 != 0)) {
            if (2 < local_18) {
              if (4 < local_18) goto LAB_100b786b;
              lVar12 = __lseeki64_nolock(*in_EAX,0x200000000,unaff_EDI);
              if (lVar12 != 0) {
                lVar12 = __lseeki64_nolock(*in_EAX,0,unaff_EDI);
                if (lVar12 == -1) goto LAB_100b7757;
                goto LAB_100b7995;
              }
            }
            goto LAB_100b7a76;
          }
          goto LAB_100b7874;
        }
LAB_100b7995:
        iVar3 = FUN_100b6ce5(*in_EAX,(LPWSTR)&local_1c,(LPWSTR)0x3);
        if (iVar3 == -1) goto LAB_100b7757;
        if (iVar3 == 2) {
LAB_100b7a1a:
          if ((local_1c & 0xffff) == 0xfffe) {
            __close_nolock(*in_EAX);
            pDVar5 = FUN_100ab320();
            *pDVar5 = 0x16;
            return 0x16;
          }
          if ((local_1c & 0xffff) == 0xfeff) {
            lVar8 = __lseek_nolock(*in_EAX,2,0);
            if (lVar8 == -1) goto LAB_100b7757;
            local_6 = 2;
            goto LAB_100b7874;
          }
        }
        else if (iVar3 == 3) {
          if (local_1c == 0xbfbbef) {
            local_6 = 1;
            goto LAB_100b7874;
          }
          goto LAB_100b7a1a;
        }
        uVar4 = __lseek_nolock(*in_EAX,0,0);
LAB_100b7a0c:
        if (uVar4 == 0xffffffff) goto LAB_100b7757;
      }
    }
  }
LAB_100b7874:
  uVar4 = local_c;
  pbVar1 = (byte *)((&DAT_100f5d00)[(int)*in_EAX >> 5] + 0x24 + (*in_EAX & 0x1f) * 0x38);
  *pbVar1 = *pbVar1 ^ (*pbVar1 ^ local_6) & 0x7f;
  pbVar1 = (byte *)((&DAT_100f5d00)[(int)*in_EAX >> 5] + 0x24 + (*in_EAX & 0x1f) * 0x38);
  *pbVar1 = (char)(param_3 >> 0x10) << 7 | *pbVar1 & 0x7f;
  if ((local_7 == 0) && ((param_3 & 8) != 0)) {
    pbVar1 = (byte *)((&DAT_100f5d00)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38);
    *pbVar1 = *pbVar1 | 0x20;
  }
  if (((local_c & 0xc0000000) == 0xc0000000) && ((param_3 & 1) != 0)) {
    CloseHandle(local_20);
    pvVar7 = CreateFileA(param_2,uVar4 & 0x7fffffff,local_14,&local_38,3,local_10,(HANDLE)0x0);
    if (pvVar7 == (HANDLE)0xffffffff) {
      DVar6 = GetLastError();
      FUN_100ab346(DVar6);
      pbVar1 = (byte *)((&DAT_100f5d00)[(int)*in_EAX >> 5] + 4 + (*in_EAX & 0x1f) * 0x38);
      *pbVar1 = *pbVar1 & 0xfe;
      __free_osfhnd(*in_EAX);
LAB_100b765e:
      pDVar5 = FUN_100ab320();
      return *pDVar5;
    }
    *(HANDLE *)((*in_EAX & 0x1f) * 0x38 + (&DAT_100f5d00)[(int)*in_EAX >> 5]) = pvVar7;
  }
  return 0;
}


// 100b7afc FID_conflict:__sopen_helper

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
  
  local_8 = &DAT_100d77d8;
  uStack_c = 0x100b7b08;
  local_20[0] = 0;
  if (((_PFileHandle == (int *)0x0) || (*_PFileHandle = -1, _Filename == (char *)0x0)) ||
     ((_BSecure != 0 && ((_PMode & 0xfffffe7fU) != 0)))) {
    pDVar1 = FUN_100ab320();
    DVar2 = 0x16;
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
  }
  else {
    local_8 = (undefined *)0x0;
    DVar2 = FUN_100b73a3(local_20,_Filename,_OFlag,_ShFlag,(byte)_PMode);
    local_8 = (undefined *)0xfffffffe;
    FUN_100b7b9a();
    if (DVar2 != 0) {
      *_PFileHandle = -1;
    }
  }
  return DVar2;
}


// 100b7b9a FUN_100b7b9a

void FUN_100b7b9a(void)

{
  byte *pbVar1;
  int unaff_EBP;
  int unaff_ESI;
  uint *unaff_EDI;
  
  if (*(int *)(unaff_EBP + -0x1c) != unaff_ESI) {
    if (*(int *)(unaff_EBP + -0x20) != unaff_ESI) {
      pbVar1 = (byte *)((&DAT_100f5d00)[(int)*unaff_EDI >> 5] + 4 + (*unaff_EDI & 0x1f) * 0x38);
      *pbVar1 = *pbVar1 & 0xfe;
    }
    __unlock_fhandle(*unaff_EDI);
  }
  return;
}


// 100b7bc8 FID_conflict:__sopen_s

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


// 100b7be6 __mbsicmp_l

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
    pDVar4 = FUN_100ab320();
    *pDVar4 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    if (local_c != '\0') {
      *(uint *)(local_10 + 0x70) = *(uint *)(local_10 + 0x70) & 0xfffffffd;
    }
    iVar5 = 0x7fffffff;
  }
  else if (_Str2 == (uchar *)0x0) {
    pDVar4 = FUN_100ab320();
    *pDVar4 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
            if (iVar5 != 2) goto LAB_100b7db8;
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
LAB_100b7db8:
              pDVar4 = FUN_100ab320();
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


// 100b7e00 __mbsicmp

/* Library Function - Single Match
    __mbsicmp
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsicmp(uchar *_Str1,uchar *_Str2)

{
  int iVar1;
  
  iVar1 = __mbsicmp_l(_Str1,_Str2,(_locale_t)0x0);
  return iVar1;
}


// 100b7e13 __mbsnbcmp_l

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
      pDVar4 = FUN_100ab320();
      *pDVar4 = 0x16;
      FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
LAB_100b7f12:
            _Str1 = puVar5;
            uVar6 = (uint)*_Str2;
            pbVar8 = _Str2 + 1;
            if ((*(byte *)(uVar6 + 0x1d + local_10) & 4) != 0) {
              if (sVar1 != 0) {
                sVar1 = _MaxCount - 2;
                if (*pbVar8 != 0) {
                  uVar6 = (uint)CONCAT11(*_Str2,*pbVar8);
                  pbVar8 = _Str2 + 2;
                  goto LAB_100b7f42;
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
              goto LAB_100b7f12;
            }
            uVar6 = (uint)*_Str2;
            uVar7 = 0;
            pbVar8 = _Str2;
            _Str1 = puVar5;
            if ((*(byte *)(uVar6 + 0x1d + local_10) & 4) != 0) {
LAB_100b7f55:
              if (local_8 != '\0') {
                *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
              }
              return 0;
            }
          }
LAB_100b7f42:
          _MaxCount = sVar1;
          if ((ushort)uVar6 != uVar7) {
            iVar3 = (-(uint)((ushort)uVar6 < uVar7) & 2) - 1;
            if (local_8 == '\0') {
              return iVar3;
            }
            *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
            return iVar3;
          }
          if ((uVar7 == 0) || (_Str2 = pbVar8, _MaxCount == 0)) goto LAB_100b7f55;
        } while( true );
      }
      pDVar4 = FUN_100ab320();
      *pDVar4 = 0x16;
      FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
      iVar3 = 0x7fffffff;
    }
  }
  return iVar3;
}


// 100b7f7c __mbsnbcmp

/* Library Function - Single Match
    __mbsnbcmp
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsnbcmp(uchar *_Str1,uchar *_Str2,size_t _MaxCount)

{
  int iVar1;
  
  iVar1 = __mbsnbcmp_l(_Str1,_Str2,_MaxCount,(_locale_t)0x0);
  return iVar1;
}


// 100b7f93 __getenv_helper_nolock

/* Library Function - Single Match
    __getenv_helper_nolock
   
   Library: Visual Studio 2005 Release */

char * __cdecl __getenv_helper_nolock(char *param_1)

{
  int iVar1;
  size_t _MaxCount;
  size_t sVar2;
  int *piVar3;
  
  if (DAT_100f5e0c == 0) {
    return (char *)0x0;
  }
  if (((DAT_100f5214 != (int *)0x0) ||
      (((DAT_100f521c != 0 && (iVar1 = ___wtomb_environ(), iVar1 == 0)) &&
       (DAT_100f5214 != (int *)0x0)))) && (piVar3 = DAT_100f5214, param_1 != (char *)0x0)) {
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


// 100b8014 FUN_100b8014

/* WARNING: Removing unreachable block (ram,0x100b8505) */
/* WARNING: Removing unreachable block (ram,0x100b850f) */
/* WARNING: Removing unreachable block (ram,0x100b8514) */

void __cdecl
FUN_100b8014(int param_1,uint param_2,ushort param_3,int param_4,byte param_5,short *param_6)

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
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
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
    goto LAB_100b8892;
  }
  if ((short)uVar14 == 0x7fff) {
    *param_6 = 1;
    if (((param_2 == 0x80000000) && (param_1 == 0)) || ((param_2 & 0x40000000) != 0)) {
      if ((uVar8 == 0) || (param_2 != 0xc0000000)) {
        if ((param_2 != 0x80000000) || (param_1 != 0)) goto LAB_100b8143;
        pcVar25 = "1#INF";
      }
      else {
        if (param_1 != 0) {
LAB_100b8143:
          pcVar25 = "1#QNAN";
          goto LAB_100b8148;
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
LAB_100b8148:
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
    goto LAB_100b8892;
  }
  local_50 = (short)(((uVar14 >> 8) + (param_2 >> 0x18) * 2) * 0x4d + -0x134312f4 + uVar14 * 0x4d10
                    >> 0x10);
  uVar16 = (uint)local_50;
  local_1a = (undefined1)uVar14;
  bStack_19 = (byte)(uVar14 >> 8);
  local_24._2_2_ = (ushort)param_1;
  local_24._0_2_ = 0;
  local_6c = &PTR_DAT_100dfd30;
  uStack_20._0_2_ = uStack_12;
  uStack_20._2_2_ = (ushort)local_10;
  uStack_1c = local_10._2_2_;
  if (uVar16 != 0) {
    iVar20 = param_1;
    uVar14 = -uVar16;
    if (0 < (int)uVar16) {
      local_6c = (undefined **)&DAT_100dfe90;
      uVar14 = uVar16;
    }
    while (uVar14 != 0) {
      uStack_20._0_2_ = (ushort)((uint)iVar20 >> 0x10);
      local_24._2_2_ = (ushort)iVar20;
      local_6c = local_6c + 0x15;
      iVar3 = CONCAT22(local_c,local_10._2_2_);
      if ((uVar14 & 7) == 0) goto LAB_100b8469;
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
LAB_100b838d:
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
            if ((short)uVar24 < 1) goto LAB_100b838d;
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
          if (0x7ffe < uVar24) goto LAB_100b844d;
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
LAB_100b844d:
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
LAB_100b8469:
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
          goto LAB_100b86d1;
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
LAB_100b85df:
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
          if ((short)uVar24 < 1) goto LAB_100b85df;
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
        goto LAB_100b86d1;
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
LAB_100b86d1:
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
        goto LAB_100b8892;
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
LAB_100b8892:
  local_24 = uVar16;
  local_10 = param_2;
  uStack_3e = iVar1;
  __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return;
}


// 100b88da __hw_cw

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


// 100b8968 ___hw_cw_sse2

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


// 100b8a08 __control87

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
  if (DAT_100f5e08 != 0) {
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


// 100b8d0b __putwch_nolock

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
  
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  if (DAT_100e01b8 != 0) {
    if (DAT_100e01c0 == (HANDLE)0xfffffffe) {
      ___initconout();
    }
    if (DAT_100e01c0 == (HANDLE)0xffffffff) goto LAB_100b8db4;
    BVar2 = WriteConsoleW(DAT_100e01c0,&_WCh,1,&local_14,(LPVOID)0x0);
    if (BVar2 != 0) {
      DAT_100e01b8 = 1;
      goto LAB_100b8db4;
    }
    if ((DAT_100e01b8 != 2) || (DVar3 = GetLastError(), DVar3 != 0x78)) goto LAB_100b8db4;
    DAT_100e01b8 = 0;
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
  if (DAT_100e01c0 != (HANDLE)0xffffffff) {
    WriteConsoleA(DAT_100e01c0,local_10,DVar3,&local_14,(LPVOID)0x0);
  }
LAB_100b8db4:
  wVar1 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return wVar1;
}


// 100b8dcd __strnicmp_l

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
      pDVar1 = FUN_100ab320();
      *pDVar1 = 0x16;
      FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
      pDVar1 = FUN_100ab320();
      *pDVar1 = 0x16;
      FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
      if (local_8 != '\0') {
        *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
      }
      iVar2 = 0x7fffffff;
    }
  }
  return iVar2;
}


// 100b8ebd __chsize_nolock

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
  if (uVar11 == 0xffffffffffffffff) goto LAB_100b8f43;
  lVar12 = __lseeki64_nolock(_FileHandle,0x200000000,unaff_EDI);
  iVar4 = (int)((ulonglong)lVar12 >> 0x20);
  if (lVar12 == -1) goto LAB_100b8f43;
  uVar7 = in_stack_00000008 - (uint)lVar12;
  uVar5 = (uint)(in_stack_00000008 < (uint)lVar12);
  iVar1 = (int)_Size - iVar4;
  iVar8 = iVar1 - uVar5;
  if ((iVar8 < 0) ||
     ((iVar8 == 0 || (SBORROW4((int)_Size,iVar4) != SBORROW4(iVar1,uVar5)) != iVar8 < 0 &&
      (uVar7 == 0)))) {
    if ((iVar8 < 1) && (iVar8 < 0)) {
      lVar12 = __lseeki64_nolock(_FileHandle,_Size & 0xffffffff,unaff_EDI);
      if (lVar12 == -1) goto LAB_100b8f43;
      pvVar2 = (HANDLE)__get_osfhandle(_FileHandle);
      BVar6 = SetEndOfFile(pvVar2);
      local_14 = (BVar6 != 0) - 1;
      local_10 = (int)local_14 >> 0x1f;
      if ((local_14 & local_10) == 0xffffffff) {
        pDVar3 = FUN_100ab320();
        *pDVar3 = 0xd;
        pDVar3 = FUN_100ab333();
        DVar13 = GetLastError();
        *pDVar3 = DVar13;
        goto LAB_100b9041;
      }
    }
  }
  else {
    dwBytes = 0x1000;
    DVar13 = 8;
    pvVar2 = GetProcessHeap();
    _Buf = HeapAlloc(pvVar2,DVar13,dwBytes);
    if (_Buf == (LPVOID)0x0) {
      pDVar3 = FUN_100ab320();
      *pDVar3 = 0xc;
      goto LAB_100b8f43;
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
      goto LAB_100b8f95;
    }
    pDVar3 = FUN_100ab333();
    if (*pDVar3 == 5) {
      pDVar3 = FUN_100ab320();
      *pDVar3 = 0xd;
    }
    local_14 = 0xffffffff;
    local_10 = 0xffffffff;
LAB_100b8f95:
    __setmode_nolock(_FileHandle,iVar4);
    DVar13 = 0;
    pvVar2 = GetProcessHeap();
    HeapFree(pvVar2,DVar13,_Buf);
LAB_100b9041:
    if ((local_14 & local_10) == 0xffffffff) goto LAB_100b8f43;
  }
  lVar12 = __lseeki64_nolock(_FileHandle,uVar11 >> 0x20,unaff_EDI);
  if (lVar12 != -1) {
    return 0;
  }
LAB_100b8f43:
  pDVar3 = FUN_100ab320();
  return *pDVar3;
}


// 100b9071 __lseek_nolock

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
    pDVar2 = FUN_100ab320();
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
    pbVar1 = (byte *)((&DAT_100f5d00)[_FileHandle >> 5] + 4 + (_FileHandle & 0x1fU) * 0x38);
    *pbVar1 = *pbVar1 & 0xfd;
  }
  else {
    FUN_100ab346(DVar4);
    DVar3 = 0xffffffff;
  }
  return DVar3;
}


// 100b90e3 __setmode_nolock

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
  piVar1 = &DAT_100f5d00 + (_FileHandle >> 5);
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
      if (_Mode != 0x40000) goto LAB_100b9182;
      *(byte *)(iVar4 + 4) = *(byte *)(iVar4 + 4) | 0x80;
      pbVar5 = (byte *)(*piVar1 + 0x24 + iVar7);
      bVar6 = *pbVar5 & 0x81 | 1;
    }
    *pbVar5 = bVar6;
  }
LAB_100b9182:
  if ((bVar2 & 0x80) == 0) {
    iVar4 = 0x8000;
  }
  else {
    iVar4 = (-(uint)((char)(cVar3 * '\x02') >> 1 != '\0') & 0xc000) + 0x4000;
  }
  return iVar4;
}


// 100b919f FUN_100b919f

undefined4 __cdecl FUN_100b919f(undefined4 *param_1)

{
  DWORD *pDVar1;
  
  if (param_1 == (undefined4 *)0x0) {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0x16;
  }
  *param_1 = DAT_100f5c84;
  return 0;
}


// 100b91d3 __mbsnbicoll_l

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
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
      if (iVar2 == 0) goto LAB_100b929c;
      iVar2 = iVar2 + -2;
    }
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  else {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
LAB_100b929c:
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    iVar2 = 0x7fffffff;
  }
  return iVar2;
}


// 100b92bf __mbsnbicoll

/* Library Function - Single Match
    __mbsnbicoll
   
   Library: Visual Studio 2005 Release */

int __cdecl __mbsnbicoll(uchar *_Str1,uchar *_Str2,size_t _MaxCount)

{
  int iVar1;
  
  iVar1 = __mbsnbicoll_l(_Str1,_Str2,_MaxCount,(_locale_t)0x0);
  return iVar1;
}


// 100b92d6 ___wtomb_environ

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
  lpWideCharStr = (LPCWSTR)*DAT_100f521c;
  puVar2 = DAT_100f521c;
  while( true ) {
    if (lpWideCharStr == (LPCWSTR)0x0) {
      return 0;
    }
    iVar1 = WideCharToMultiByte(0,0,lpWideCharStr,-1,(LPSTR)0x0,0,(LPCSTR)0x0,(LPBOOL)0x0);
    if ((iVar1 == 0) || (local_8 = (LPSTR)FUN_10002e20(iVar1,1), local_8 == (LPSTR)0x0)) break;
    iVar1 = WideCharToMultiByte(0,0,(LPCWSTR)*puVar2,-1,local_8,iVar1,(LPCSTR)0x0,(LPBOOL)0x0);
    if (iVar1 == 0) {
      FUN_10002ea0(local_8);
      return -1;
    }
    iVar1 = FUN_100b99ac(&local_8,0);
    if ((iVar1 < 0) && (local_8 != (LPSTR)0x0)) {
      FUN_10002ea0(local_8);
      local_8 = (LPSTR)0x0;
    }
    puVar2 = puVar2 + 1;
    lpWideCharStr = (LPCWSTR)*puVar2;
  }
  return -1;
}


// 100b936b _strnlen

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


// 100b9385 ___initconout

/* Library Function - Single Match
    ___initconout
   
   Library: Visual Studio 2005 Release */

void __cdecl ___initconout(void)

{
  DAT_100e01c0 = CreateFileA("CONOUT$",0x40000000,3,(LPSECURITY_ATTRIBUTES)0x0,3,0,(HANDLE)0x0);
  return;
}


// 100b93e0 ___ascii_strnicmp

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
      if (bVar2 != (byte)uVar3) goto LAB_100b9431;
      _MaxCount = _MaxCount - 1;
    } while (_MaxCount != 0);
    _MaxCount = 0;
    bVar2 = (byte)(uVar3 >> 8);
    bVar5 = bVar2 < (byte)uVar3;
    if (bVar2 != (byte)uVar3) {
LAB_100b9431:
      _MaxCount = 0xffffffff;
      if (!bVar5) {
        _MaxCount = 1;
      }
    }
  }
  return _MaxCount;
}


// 100b9441 __crtCompareStringA_stat

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
  local_8 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  local_20 = param_4;
  if (DAT_100f5c88 == 0) {
    iVar2 = CompareStringW(0,0,L"",1,L"",1);
    if (iVar2 == 0) {
      DVar3 = GetLastError();
      if (DVar3 == 0x78) {
        DAT_100f5c88 = 2;
      }
    }
    else {
      DAT_100f5c88 = 1;
    }
  }
  pbVar10 = in_EDX;
  uVar9 = param_3;
  if ((int)param_3 < 1) {
    if ((int)param_3 < -1) goto LAB_100b97b8;
  }
  else {
    do {
      uVar9 = uVar9 - 1;
      if (*pbVar10 == 0) goto LAB_100b94bb;
      pbVar10 = pbVar10 + 1;
    } while (uVar9 != 0);
    uVar9 = 0xffffffff;
LAB_100b94bb:
    param_3 = param_3 + (-1 - uVar9);
  }
  pcVar8 = lpMultiByteStr;
  iVar2 = param_5;
  if (param_5 < 1) {
    if (param_5 < -1) goto LAB_100b97b8;
  }
  else {
    do {
      iVar2 = iVar2 + -1;
      if (*pcVar8 == '\0') goto LAB_100b94dd;
      pcVar8 = pcVar8 + 1;
    } while (iVar2 != 0);
    iVar2 = -1;
LAB_100b94dd:
    param_5 = param_5 + (-1 - iVar2);
  }
  if ((DAT_100f5c88 == 2) || (DAT_100f5c88 == 0)) {
    pCVar11 = (PCNZCH)0x0;
    pbVar10 = (byte *)0x0;
    if (param_1 == (localeinfo_struct *)0x0) {
      param_1 = *(localeinfo_struct **)(*in_ECX + 0x14);
    }
    if (param_6 == (char *)0x0) {
      param_6 = *(char **)(*in_ECX + 4);
    }
    pcVar8 = (char *)___ansicp((LCID)param_1);
    if (pcVar8 == (char *)0xffffffff) goto LAB_100b97b8;
    local_28 = in_EDX;
    if (pcVar8 != param_6) {
      pbVar10 = (byte *)___convertcp((UINT)param_6,(UINT)pcVar8,(char *)in_EDX,&param_3,(LPSTR)0x0,0
                                    );
      if (pbVar10 == (byte *)0x0) goto LAB_100b97b8;
      pCVar11 = (PCNZCH)___convertcp((UINT)param_6,(UINT)pcVar8,lpMultiByteStr,(uint *)&param_5,
                                     (LPSTR)0x0,0);
      local_28 = pbVar10;
      local_20 = pCVar11;
      if (pCVar11 == (PCNZCH)0x0) {
        FUN_10002ea0(pbVar10);
        goto LAB_100b97b8;
      }
    }
    CompareStringA((LCID)param_1,param_2,(PCNZCH)local_28,param_3,local_20,param_5);
    if (pbVar10 != (byte *)0x0) {
      FUN_10002ea0(pbVar10);
      FUN_10002ea0(pCVar11);
    }
    goto LAB_100b97b8;
  }
  if (DAT_100f5c88 != 1) goto LAB_100b97b8;
  if (param_6 == (char *)0x0) {
    param_6 = *(char **)(*in_ECX + 4);
  }
  if ((param_3 == 0) || (param_5 == 0)) {
    if ((param_3 == param_5) ||
       (((1 < param_5 || (1 < (int)param_3)) ||
        (BVar4 = GetCPInfo((UINT)param_6,&local_1c), BVar4 == 0)))) goto LAB_100b97b8;
    if (0 < (int)param_3) {
      if (1 < local_1c.MaxCharSize) {
        pBVar5 = local_1c.LeadByte;
        while (((local_1c.LeadByte[0] != 0 && (pBVar5[1] != 0)) &&
               ((*in_EDX < *pBVar5 || (pBVar5[1] < *in_EDX))))) {
          pBVar5 = pBVar5 + 2;
          local_1c.LeadByte[0] = *pBVar5;
        }
      }
      goto LAB_100b97b8;
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
      goto LAB_100b97b8;
    }
  }
  cchWideChar = MultiByteToWideChar((UINT)param_6,9,(LPCSTR)in_EDX,param_3,(LPWSTR)0x0,0);
  if (cchWideChar == 0) goto LAB_100b97b8;
  if (((int)cchWideChar < 1) || (0xffffffe0 / cchWideChar < 2)) {
    local_24 = (PCNZWCH)0x0;
  }
  else {
    uVar7 = cchWideChar * 2 + 8;
    if (uVar7 < 0x401) {
      pWVar6 = (PCNZWCH)&stack0xffffffc4;
      local_24 = (PCNZWCH)&stack0xffffffc4;
      if (&stack0x00000000 != (undefined1 *)0x3c) {
LAB_100b9631:
        local_24 = pWVar6 + 4;
      }
    }
    else {
      pWVar6 = (PCNZWCH)FUN_10002dc0(uVar7);
      local_24 = pWVar6;
      if (pWVar6 != (PCNZWCH)0x0) {
        pWVar6[0] = L'\xdddd';
        pWVar6[1] = L'\0';
        goto LAB_100b9631;
      }
    }
  }
  if (local_24 == (PCNZWCH)0x0) goto LAB_100b97b8;
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
        if (&stack0x00000000 == (undefined1 *)0x3c) goto LAB_100b96f2;
        lpWideCharStr = (LPWSTR)&stack0xffffffcc;
      }
      else {
        lpWideCharStr = (LPWSTR)FUN_10002dc0(uVar1);
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
LAB_100b96f2:
  __freea(local_24);
LAB_100b97b8:
  iVar2 = __security_check_cookie(local_8 ^ (uint)&stack0xfffffffc);
  return iVar2;
}


// 100b97ca ___crtCompareStringA

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


// 100b980a __strnicoll_l

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
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
        pDVar1 = FUN_100ab320();
        *pDVar1 = 0x16;
        goto LAB_100b98df;
      }
      iVar2 = iVar2 + -2;
    }
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  else {
    pDVar1 = FUN_100ab320();
    *pDVar1 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
LAB_100b98df:
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
    iVar2 = 0x7fffffff;
  }
  return iVar2;
}


// 100b9902 findenv

/* Library Function - Single Match
    _findenv
   
   Library: Visual Studio 2005 Release */

int __cdecl findenv(uchar *param_1)

{
  int iVar1;
  int *piVar2;
  size_t unaff_EDI;
  
  piVar2 = DAT_100f5214;
  while( true ) {
    if ((uchar *)*piVar2 == (uchar *)0x0) {
      return -((int)piVar2 - (int)DAT_100f5214 >> 2);
    }
    iVar1 = __mbsnbicoll(param_1,(uchar *)*piVar2,unaff_EDI);
    if ((iVar1 == 0) &&
       ((*(char *)(unaff_EDI + *piVar2) == '=' || (*(char *)(unaff_EDI + *piVar2) == '\0')))) break;
    piVar2 = piVar2 + 1;
  }
  return (int)piVar2 - (int)DAT_100f5214 >> 2;
}


// 100b994f copy_environ

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
    puVar2 = (undefined4 *)FUN_10002e20((int)puVar2 + 1,4);
    if (puVar2 == (undefined4 *)0x0) {
      __amsg_exit(9);
    }
    pcVar4 = (char *)*unaff_EDI;
    puVar6 = puVar2;
    while (pcVar4 != (char *)0x0) {
      pvVar3 = FUN_10002ec0(pcVar4);
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


// 100b99ac FUN_100b99ac

undefined4 __cdecl FUN_100b99ac(undefined4 *param_1,int param_2)

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
    pDVar2 = FUN_100ab320();
    *pDVar2 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
    return 0xffffffff;
  }
  _Str = (uchar *)*param_1;
  if (((_Str == (uchar *)0x0) || (puVar3 = __mbschr(_Str,0x3d), puVar3 == (uchar *)0x0)) ||
     (_Str == puVar3)) {
LAB_100b9a3a:
    pDVar2 = FUN_100ab320();
    *pDVar2 = 0x16;
    return 0xffffffff;
  }
  bVar11 = puVar3[1] == '\0';
  if (DAT_100f5214 == DAT_100f5218) {
    DAT_100f5214 = copy_environ();
  }
  if (DAT_100f5214 == (int *)0x0) {
    if ((param_2 == 0) || (DAT_100f521c == (undefined4 *)0x0)) {
      if (bVar11) {
        return 0;
      }
      DAT_100f5214 = (int *)FUN_10002dc0(4);
      if (DAT_100f5214 == (int *)0x0) {
        return 0xffffffff;
      }
      *DAT_100f5214 = 0;
      if (DAT_100f521c == (undefined4 *)0x0) {
        DAT_100f521c = (undefined4 *)FUN_10002dc0(4);
        if (DAT_100f521c == (undefined4 *)0x0) {
          return 0xffffffff;
        }
        *DAT_100f521c = 0;
      }
    }
    else {
      iVar4 = ___wtomb_environ();
      if (iVar4 != 0) goto LAB_100b9a3a;
    }
  }
  piVar6 = DAT_100f5214;
  if (DAT_100f5214 == (int *)0x0) {
    return 0xffffffff;
  }
  uVar5 = findenv(_Str);
  if (((int)uVar5 < 0) || (*piVar6 == 0)) {
    if (bVar11) {
      FUN_10002ea0(_Str);
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
    piVar6 = (int *)FUN_10002e50(DAT_100f5214,4,uVar1);
    if (piVar6 == (int *)0x0) {
      return 0xffffffff;
    }
    piVar6[uVar5] = (int)_Str;
    (piVar6 + uVar5)[1] = 0;
    *param_1 = 0;
  }
  else {
    piVar10 = piVar6 + uVar5;
    FUN_10002ea0(*piVar10);
    if (!bVar11) {
      *piVar10 = (int)_Str;
      *param_1 = 0;
      goto LAB_100b9b4b;
    }
    while (*piVar10 != 0) {
      *piVar10 = piVar10[1];
      uVar5 = uVar5 + 1;
      piVar10 = piVar6 + uVar5;
    }
    if ((0x3ffffffe < uVar5) ||
       (piVar6 = (int *)FUN_10002e50(DAT_100f5214,uVar5,4), piVar6 == (int *)0x0))
    goto LAB_100b9b4b;
  }
  DAT_100f5214 = piVar6;
LAB_100b9b4b:
  if (param_2 != 0) {
    iVar4 = 1;
    sVar7 = _strlen((char *)_Str);
    _Dst = (char *)FUN_10002e20(sVar7 + 2,iVar4);
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
        pDVar2 = FUN_100ab320();
        *pDVar2 = 0x2a;
      }
      FUN_10002ea0(_Dst);
    }
  }
  if (bVar11) {
    FUN_10002ea0(_Str);
    *param_1 = 0;
    return local_14;
  }
  return local_14;
}


// 100b9bf8 __mbschr_l

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
    pDVar3 = FUN_100ab320();
    *pDVar3 = 0x16;
    FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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
          if (bVar1 == 0) goto LAB_100b9ca0;
          pbVar4 = _Str + 1;
          if (_Ch == CONCAT11(bVar2,bVar1)) goto LAB_100b9c92;
        }
        _Str = pbVar4 + 1;
      }
      if (_Ch != (ushort)bVar2) {
LAB_100b9ca0:
        if (local_8 != '\0') {
          *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
        }
        return (uchar *)0x0;
      }
    }
LAB_100b9c92:
    if (local_8 != '\0') {
      *(uint *)(local_c + 0x70) = *(uint *)(local_c + 0x70) & 0xfffffffd;
    }
  }
  return _Str;
}


// 100b9cb2 __mbschr

/* Library Function - Single Match
    __mbschr
   
   Library: Visual Studio 2005 Release */

uchar * __cdecl __mbschr(uchar *_Str,uint _Ch)

{
  uchar *puVar1;
  
  puVar1 = __mbschr_l(_Str,_Ch,(_locale_t)0x0);
  return puVar1;
}


// 100b9cd0 FUN_100b9cd0

undefined4 __thiscall FUN_100b9cd0(void *this,int param_1)

{
  if (((param_1 < 0x800) && (*(int *)((int)this + param_1 * 4 + 8) != 0)) &&
     (*(int *)((int)this + 4) != 0)) {
    return 1;
  }
  return 0;
}


// 100b9d00 FUN_100b9d00

/* WARNING: Function: __alloca_probe replaced with injection: alloca_probe */

void __cdecl FUN_100b9d00(undefined4 *param_1,undefined4 param_2,char *param_3)

{
  char local_1000 [4092];
  undefined4 uStack_4;
  
  uStack_4 = 0x100b9d0a;
  _vsprintf(local_1000,param_3,&stack0x00000010);
  (**(code **)*param_1)(param_2,local_1000);
  return;
}


// 100b9d90 FUN_100b9d90

void __thiscall FUN_100b9d90(void *this,float *param_1,float *param_2)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  float fVar10;
  float fVar11;
  
  fVar1 = param_2[2];
  fVar2 = param_2[1];
  fVar3 = *param_2;
  fVar4 = param_1[1];
  fVar5 = param_1[5];
  fVar6 = param_1[6];
  fVar7 = param_1[9];
  fVar8 = param_1[2];
  fVar9 = param_1[0xd];
  fVar10 = param_1[10];
  fVar11 = param_1[0xe];
  *(float *)this = param_1[4] * fVar2 + param_1[8] * fVar1 + *param_1 * fVar3 + param_1[0xc];
  *(float *)((int)this + 4) = fVar4 * fVar3 + fVar5 * fVar2 + fVar7 * fVar1 + fVar9;
  *(float *)((int)this + 8) = fVar8 * fVar3 + fVar6 * fVar2 + fVar10 * fVar1 + fVar11;
  return;
}


// 100b9e30 FUN_100b9e30

void __thiscall FUN_100b9e30(void *this,float *param_1,float *param_2)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  
  fVar1 = param_2[2];
  fVar2 = param_2[1];
  fVar3 = *param_2;
  fVar4 = param_1[1];
  fVar5 = param_1[5];
  fVar6 = param_1[6];
  fVar7 = param_1[9];
  fVar8 = param_1[2];
  fVar9 = param_1[10];
  *(float *)this = param_1[4] * fVar2 + param_1[8] * fVar1 + *param_1 * fVar3;
  *(float *)((int)this + 4) = fVar4 * fVar3 + fVar5 * fVar2 + fVar7 * fVar1;
  *(float *)((int)this + 8) = fVar8 * fVar3 + fVar6 * fVar2 + fVar9 * fVar1;
  return;
}


// 100b9ec0 FUN_100b9ec0

void __thiscall FUN_100b9ec0(void *this,float *param_1,float *param_2)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  
  fVar1 = param_2[2];
  fVar2 = param_2[1];
  fVar3 = *param_2;
  fVar4 = param_1[4];
  fVar5 = param_1[5];
  fVar6 = param_1[9];
  fVar7 = param_1[6];
  fVar8 = param_1[8];
  fVar9 = param_1[10];
  *(float *)this = param_1[1] * fVar2 + param_1[2] * fVar1 + *param_1 * fVar3;
  *(float *)((int)this + 4) = fVar4 * fVar3 + fVar5 * fVar2 + fVar7 * fVar1;
  *(float *)((int)this + 8) = fVar8 * fVar3 + fVar6 * fVar2 + fVar9 * fVar1;
  return;
}


// 100b9f50 FUN_100b9f50

float * __cdecl FUN_100b9f50(float *param_1,float *param_2)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  
  fVar3 = param_2[1];
  fVar2 = 0.0;
  if (0.57 <= ABS(*param_2)) {
    fVar5 = param_2[2];
    *param_1 = fVar5 - fVar3 * 0.0;
    fVar5 = *param_2 * 0.0 - fVar5 * 0.0;
    fVar3 = fVar3 * 0.0 - *param_2;
  }
  else {
    fVar5 = param_2[2];
    *param_1 = fVar5 * 0.0 - fVar3 * 0.0;
    fVar5 = *param_2 * 0.0 - fVar5;
    fVar3 = fVar3 - *param_2 * 0.0;
  }
  param_1[2] = fVar3;
  param_1[1] = fVar5;
  fVar1 = *param_1;
  fVar4 = fVar1 * fVar1 + fVar5 * fVar5 + fVar3 * fVar3;
  if (fVar4 != 0.0) {
    fVar2 = 1.0 / SQRT(fVar4);
  }
  *param_1 = fVar1 * fVar2;
  param_1[1] = fVar5 * fVar2;
  param_1[2] = fVar3 * fVar2;
  return param_1;
}


// 100ba040 FUN_100ba040

void __cdecl FUN_100ba040(int *param_1,int *param_2,int *param_3)

{
  int iVar1;
  int iVar2;
  float *pfVar3;
  float *pfVar4;
  int iVar5;
  int iVar6;
  int iVar7;
  int iVar8;
  int iVar9;
  int iVar10;
  
  iVar5 = param_1[1];
  iVar6 = param_1[2];
  iVar8 = 0;
  if (0 < param_3[1]) {
    do {
      *(undefined4 *)(*param_3 + iVar8 * 4) = 0;
      iVar8 = iVar8 + 1;
    } while (iVar8 < param_3[1]);
  }
  iVar8 = 0;
  if (0 < iVar6) {
    do {
      iVar9 = 0;
      if (3 < iVar5) {
        pfVar3 = (float *)(*param_2 + iVar8 * 4);
        do {
          pfVar4 = (float *)(*param_3 + iVar9 * 4);
          *pfVar4 = *(float *)(*param_1 + (param_1[3] * iVar8 + iVar9) * 4) * *pfVar3 + *pfVar4;
          pfVar4 = (float *)(*param_3 + 4 + iVar9 * 4);
          *pfVar4 = *(float *)(*param_1 + 4 + (param_1[3] * iVar8 + iVar9) * 4) * *pfVar3 + *pfVar4;
          pfVar4 = (float *)(*param_3 + 8 + iVar9 * 4);
          *pfVar4 = *(float *)(*param_1 + 8 + (param_1[3] * iVar8 + iVar9) * 4) * *pfVar3 + *pfVar4;
          pfVar4 = (float *)(*param_3 + 0xc + iVar9 * 4);
          *pfVar4 = *(float *)(*param_1 + 0xc + (param_1[3] * iVar8 + iVar9) * 4) * *pfVar3 +
                    *pfVar4;
          iVar9 = iVar9 + 4;
        } while (iVar9 < iVar5 + -3);
      }
      if (iVar9 < iVar5) {
        iVar7 = *param_2;
        do {
          iVar10 = param_1[3] * iVar8 + iVar9;
          iVar1 = iVar9 * 4;
          iVar2 = iVar9 * 4;
          iVar9 = iVar9 + 1;
          *(float *)(*param_3 + iVar2) =
               *(float *)(*param_1 + iVar10 * 4) * *(float *)(iVar7 + iVar8 * 4) +
               *(float *)(*param_3 + iVar1);
        } while (iVar9 < iVar5);
      }
      iVar8 = iVar8 + 1;
    } while (iVar8 < iVar6);
  }
  return;
}


// 100ba1b0 FUN_100ba1b0

undefined4 __cdecl FUN_100ba1b0(int param_1)

{
  float fVar1;
  float *pfVar2;
  int iVar3;
  float *pfVar4;
  float *pfVar5;
  float *pfVar6;
  int iVar7;
  int iVar8;
  int iVar9;
  int iVar10;
  int iVar11;
  float *local_d8;
  float *local_d4;
  int local_d0;
  int local_cc;
  float local_c0 [5];
  undefined4 local_ac;
  undefined4 local_a0;
  undefined4 local_9c;
  undefined4 local_98;
  undefined4 local_94;
  undefined4 local_90;
  undefined4 local_8c;
  undefined4 local_80;
  undefined4 local_7c;
  undefined4 local_78;
  undefined4 local_74;
  undefined4 local_70;
  undefined4 local_6c;
  undefined4 local_60;
  undefined4 local_5c;
  undefined4 local_58;
  undefined4 local_54;
  undefined4 local_50;
  undefined4 local_4c;
  undefined4 local_40;
  undefined4 local_3c;
  undefined4 local_38;
  undefined4 local_34;
  undefined4 local_30;
  undefined4 local_2c;
  undefined4 local_20;
  undefined4 local_1c;
  undefined4 local_18;
  undefined4 local_14;
  undefined4 local_10;
  undefined4 local_c;
  
  local_d8 = local_c0;
  iVar7 = 0x17;
  pfVar2 = local_d8;
  do {
    *pfVar2 = 0.0;
    pfVar2[1] = 0.0;
    pfVar2 = pfVar2 + 2;
    iVar7 = iVar7 + -1;
  } while (-1 < iVar7);
  local_c0[0] = 1.0;
  local_9c = 0x3f800000;
  local_78 = 0x3f800000;
  local_54 = 0x3f800000;
  local_30 = 0x3f800000;
  local_c = 0x3f800000;
  iVar7 = 0;
  local_d4 = (float *)(param_1 + 0x10);
  do {
    pfVar2 = local_d4 + iVar7;
    fVar1 = *pfVar2;
    iVar3 = iVar7;
    do {
      local_d4[iVar3] = (1.0 / fVar1) * local_d4[iVar3];
      iVar3 = iVar3 + 1;
    } while (iVar3 < 6);
    iVar3 = 0;
    do {
      local_d8[iVar3] = local_d8[iVar3] * (1.0 / fVar1);
      iVar3 = iVar3 + 1;
    } while (iVar3 <= iVar7);
    iVar3 = iVar7 + 1;
    if (iVar3 < 6) {
      iVar8 = 0;
      iVar11 = 6 - iVar3;
      pfVar6 = local_d8;
      do {
        fVar1 = pfVar2[8];
        pfVar2 = pfVar2 + 8;
        pfVar6 = pfVar6 + 8;
        iVar8 = iVar8 + -0x20;
        if (fVar1 != 0.0) {
          iVar9 = 0;
          pfVar4 = pfVar6;
          do {
            *pfVar4 = *pfVar4 - *(float *)(iVar8 + (int)pfVar4) * fVar1;
            iVar9 = iVar9 + 1;
            pfVar4 = pfVar4 + 1;
            pfVar5 = pfVar2;
            iVar10 = iVar7;
          } while (iVar9 <= iVar7);
          do {
            *pfVar5 = *pfVar5 - *(float *)(iVar8 + (int)pfVar5) * fVar1;
            iVar10 = iVar10 + 1;
            pfVar5 = pfVar5 + 1;
          } while (iVar10 < 6);
        }
        iVar11 = iVar11 + -1;
      } while (iVar11 != 0);
    }
    local_d4 = local_d4 + 8;
    local_d8 = local_d8 + 8;
    iVar7 = iVar3;
  } while (iVar3 < 6);
  local_cc = 0;
  iVar7 = 5;
  do {
    iVar3 = iVar7 + -1;
    local_d8 = local_d8 + -8;
    if (-1 < iVar3) {
      pfVar2 = local_d4 + -3;
      iVar8 = 0;
      local_d0 = iVar3;
      do {
        fVar1 = *(float *)((int)pfVar2 + local_cc + -0x20);
        pfVar2 = pfVar2 + -8;
        iVar8 = iVar8 + 0x20;
        if (fVar1 != 0.0) {
          iVar11 = 5;
          pfVar6 = (float *)((int)pfVar2 + ((int)local_d8 - (int)(local_d4 + -8)));
          do {
            *pfVar6 = *pfVar6 - *(float *)(iVar8 + (int)pfVar6) * fVar1;
            iVar11 = iVar11 + -1;
            pfVar6 = pfVar6 + -1;
          } while (-1 < iVar11);
          iVar11 = 5;
          pfVar6 = pfVar2;
          do {
            *pfVar6 = *pfVar6 - *(float *)(iVar8 + (int)pfVar6) * fVar1;
            iVar11 = iVar11 + -1;
            pfVar6 = pfVar6 + -1;
          } while (iVar7 <= iVar11);
        }
        local_d0 = local_d0 + -1;
      } while (-1 < local_d0);
    }
    local_cc = local_cc + -4;
    local_d4 = local_d4 + -8;
    iVar7 = iVar3;
  } while (-0x15 < local_cc);
  *(float *)(param_1 + 0x10) = local_c0[0];
  *(float *)(param_1 + 0x14) = local_c0[1];
  *(float *)(param_1 + 0x18) = local_c0[2];
  *(float *)(param_1 + 0x1c) = local_c0[3];
  *(float *)(param_1 + 0x20) = local_c0[4];
  *(undefined4 *)(param_1 + 0x24) = local_ac;
  *(undefined4 *)(param_1 + 0x30) = local_a0;
  *(undefined4 *)(param_1 + 0x34) = local_9c;
  *(undefined4 *)(param_1 + 0x38) = local_98;
  *(undefined4 *)(param_1 + 0x3c) = local_94;
  *(undefined4 *)(param_1 + 0x40) = local_90;
  *(undefined4 *)(param_1 + 0x44) = local_8c;
  *(undefined4 *)(param_1 + 0x50) = local_80;
  *(undefined4 *)(param_1 + 0x54) = local_7c;
  *(undefined4 *)(param_1 + 0x58) = local_78;
  *(undefined4 *)(param_1 + 0x5c) = local_74;
  *(undefined4 *)(param_1 + 0x60) = local_70;
  *(undefined4 *)(param_1 + 100) = local_6c;
  *(undefined4 *)(param_1 + 0x70) = local_60;
  *(undefined4 *)(param_1 + 0x74) = local_5c;
  *(undefined4 *)(param_1 + 0x78) = local_58;
  *(undefined4 *)(param_1 + 0x7c) = local_54;
  *(undefined4 *)(param_1 + 0x80) = local_50;
  *(undefined4 *)(param_1 + 0x84) = local_4c;
  *(undefined4 *)(param_1 + 0x90) = local_40;
  *(undefined4 *)(param_1 + 0x94) = local_3c;
  *(undefined4 *)(param_1 + 0x98) = local_38;
  *(undefined4 *)(param_1 + 0x9c) = local_34;
  *(undefined4 *)(param_1 + 0xa0) = local_30;
  *(undefined4 *)(param_1 + 0xa4) = local_2c;
  *(undefined4 *)(param_1 + 0xb0) = local_20;
  *(undefined4 *)(param_1 + 0xb4) = local_1c;
  *(undefined4 *)(param_1 + 0xb8) = local_18;
  *(undefined4 *)(param_1 + 0xbc) = local_14;
  *(undefined4 *)(param_1 + 0xc0) = local_10;
  *(undefined4 *)(param_1 + 0xc4) = local_c;
  return 0;
}


// 100ba5f0 FUN_100ba5f0

undefined4 __cdecl FUN_100ba5f0(int param_1)

{
  float fVar1;
  float *pfVar2;
  int iVar3;
  float *pfVar4;
  float *pfVar5;
  float *pfVar6;
  int iVar7;
  int iVar8;
  int iVar9;
  int iVar10;
  int iVar11;
  float *local_b8;
  float *local_b4;
  int local_b0;
  int local_ac;
  float local_a0 [8];
  undefined4 local_80;
  undefined4 local_7c;
  undefined4 local_78;
  undefined4 local_74;
  undefined4 local_70;
  undefined4 local_60;
  undefined4 local_5c;
  undefined4 local_58;
  undefined4 local_54;
  undefined4 local_50;
  undefined4 local_40;
  undefined4 local_3c;
  undefined4 local_38;
  undefined4 local_34;
  undefined4 local_30;
  undefined4 local_20;
  undefined4 local_1c;
  undefined4 local_18;
  undefined4 local_14;
  undefined4 local_10;
  
  local_b8 = local_a0;
  iVar7 = 0x13;
  pfVar2 = local_b8;
  do {
    *pfVar2 = 0.0;
    pfVar2[1] = 0.0;
    pfVar2 = pfVar2 + 2;
    iVar7 = iVar7 + -1;
  } while (-1 < iVar7);
  local_a0[0] = 1.0;
  local_7c = 0x3f800000;
  local_58 = 0x3f800000;
  local_34 = 0x3f800000;
  local_10 = 0x3f800000;
  iVar7 = 0;
  local_b4 = (float *)(param_1 + 0x10);
  do {
    pfVar2 = local_b4 + iVar7;
    fVar1 = *pfVar2;
    iVar3 = iVar7;
    do {
      local_b4[iVar3] = local_b4[iVar3] * (1.0 / fVar1);
      iVar3 = iVar3 + 1;
    } while (iVar3 < 5);
    iVar3 = 0;
    do {
      local_b8[iVar3] = local_b8[iVar3] * (1.0 / fVar1);
      iVar3 = iVar3 + 1;
    } while (iVar3 <= iVar7);
    iVar3 = iVar7 + 1;
    if (iVar3 < 5) {
      iVar8 = 0;
      iVar11 = 5 - iVar3;
      pfVar6 = local_b8;
      do {
        fVar1 = pfVar2[8];
        pfVar2 = pfVar2 + 8;
        pfVar6 = pfVar6 + 8;
        iVar8 = iVar8 + -0x20;
        if (fVar1 != 0.0) {
          iVar9 = 0;
          pfVar4 = pfVar6;
          do {
            *pfVar4 = *pfVar4 - *(float *)(iVar8 + (int)pfVar4) * fVar1;
            iVar9 = iVar9 + 1;
            pfVar4 = pfVar4 + 1;
            pfVar5 = pfVar2;
            iVar10 = iVar7;
          } while (iVar9 <= iVar7);
          do {
            *pfVar5 = *pfVar5 - *(float *)(iVar8 + (int)pfVar5) * fVar1;
            iVar10 = iVar10 + 1;
            pfVar5 = pfVar5 + 1;
          } while (iVar10 < 5);
        }
        iVar11 = iVar11 + -1;
      } while (iVar11 != 0);
    }
    local_b4 = local_b4 + 8;
    local_b8 = local_b8 + 8;
    iVar7 = iVar3;
  } while (iVar3 < 5);
  local_ac = 0;
  iVar7 = 4;
  do {
    iVar3 = iVar7 + -1;
    local_b8 = local_b8 + -8;
    if (-1 < iVar3) {
      pfVar2 = local_b4 + -4;
      iVar8 = 0;
      local_b0 = iVar3;
      do {
        fVar1 = *(float *)(local_ac + -0x20 + (int)pfVar2);
        pfVar2 = pfVar2 + -8;
        iVar8 = iVar8 + 0x20;
        if (fVar1 != 0.0) {
          iVar11 = 4;
          pfVar6 = (float *)((int)pfVar2 + ((int)local_b8 - (int)(local_b4 + -8)));
          do {
            *pfVar6 = *pfVar6 - *(float *)(iVar8 + (int)pfVar6) * fVar1;
            iVar11 = iVar11 + -1;
            pfVar6 = pfVar6 + -1;
          } while (-1 < iVar11);
          iVar11 = 4;
          pfVar6 = pfVar2;
          do {
            *pfVar6 = *pfVar6 - *(float *)(iVar8 + (int)pfVar6) * fVar1;
            iVar11 = iVar11 + -1;
            pfVar6 = pfVar6 + -1;
          } while (iVar7 <= iVar11);
        }
        local_b0 = local_b0 + -1;
      } while (-1 < local_b0);
    }
    local_ac = local_ac + -4;
    local_b4 = local_b4 + -8;
    iVar7 = iVar3;
  } while (-0x11 < local_ac);
  *(float *)(param_1 + 0x10) = local_a0[0];
  *(float *)(param_1 + 0x14) = local_a0[1];
  *(float *)(param_1 + 0x18) = local_a0[2];
  *(float *)(param_1 + 0x1c) = local_a0[3];
  *(float *)(param_1 + 0x20) = local_a0[4];
  *(undefined4 *)(param_1 + 0x30) = local_80;
  *(undefined4 *)(param_1 + 0x34) = local_7c;
  *(undefined4 *)(param_1 + 0x38) = local_78;
  *(undefined4 *)(param_1 + 0x3c) = local_74;
  *(undefined4 *)(param_1 + 0x40) = local_70;
  *(undefined4 *)(param_1 + 0x50) = local_60;
  *(undefined4 *)(param_1 + 0x54) = local_5c;
  *(undefined4 *)(param_1 + 0x58) = local_58;
  *(undefined4 *)(param_1 + 0x5c) = local_54;
  *(undefined4 *)(param_1 + 0x60) = local_50;
  *(undefined4 *)(param_1 + 0x70) = local_40;
  *(undefined4 *)(param_1 + 0x74) = local_3c;
  *(undefined4 *)(param_1 + 0x78) = local_38;
  *(undefined4 *)(param_1 + 0x7c) = local_34;
  *(undefined4 *)(param_1 + 0x80) = local_30;
  *(undefined4 *)(param_1 + 0x90) = local_20;
  *(undefined4 *)(param_1 + 0x94) = local_1c;
  *(undefined4 *)(param_1 + 0x98) = local_18;
  *(undefined4 *)(param_1 + 0x9c) = local_14;
  *(undefined4 *)(param_1 + 0xa0) = local_10;
  return 0;
}


// 100ba980 FUN_100ba980

undefined4 __cdecl FUN_100ba980(undefined4 *param_1,float param_2)

{
  float *pfVar1;
  float fVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  float fVar10;
  float fVar11;
  float fVar12;
  float fVar13;
  float fVar14;
  float *pfVar15;
  int iVar16;
  float fVar17;
  float fVar18;
  float fVar19;
  float fVar20;
  
  pfVar15 = (float *)*param_1;
  iVar16 = param_1[3];
  pfVar1 = pfVar15 + iVar16;
  fVar2 = pfVar1[iVar16 + 2];
  fVar3 = pfVar1[iVar16 + 1];
  fVar4 = pfVar1[iVar16];
  fVar17 = pfVar15[iVar16 + 1] * fVar2 - pfVar15[iVar16 + 2] * fVar3;
  fVar5 = pfVar15[2];
  fVar19 = pfVar15[iVar16] * fVar3 - fVar4 * pfVar15[iVar16 + 1];
  fVar18 = fVar4 * pfVar15[iVar16 + 2] - pfVar15[iVar16] * fVar2;
  fVar6 = pfVar15[1];
  fVar7 = *pfVar15;
  fVar8 = pfVar1[iVar16 + 1];
  fVar9 = pfVar1[2];
  fVar10 = pfVar1[1];
  fVar11 = *pfVar1;
  fVar12 = pfVar1[2];
  fVar13 = pfVar1[1];
  fVar14 = *pfVar1;
  fVar20 = pfVar15[2] * fVar19 + *pfVar15 * fVar17 + fVar18 * pfVar15[1];
  if (ABS(fVar20) < param_2 * param_2 * param_2) {
    return 1;
  }
  fVar20 = 1.0 / fVar20;
  *pfVar15 = fVar17 * fVar20;
  pfVar15[1] = fVar18 * fVar20;
  pfVar15[2] = fVar19 * fVar20;
  *pfVar1 = (fVar5 * fVar3 - fVar6 * fVar2) * fVar20;
  pfVar1[1] = (fVar7 * fVar2 - fVar5 * fVar4) * fVar20;
  pfVar1[2] = (fVar6 * fVar4 - fVar7 * fVar8) * fVar20;
  pfVar1[iVar16 + 1] = (fVar5 * fVar11 - fVar7 * fVar12) * fVar20;
  pfVar1[iVar16] = (fVar6 * fVar9 - fVar5 * fVar10) * fVar20;
  pfVar1[iVar16 + 2] = (fVar7 * fVar13 - fVar6 * fVar14) * fVar20;
  return 0;
}


// 100bab90 FUN_100bab90

void __fastcall FUN_100bab90(undefined4 *param_1)

{
  param_1[2] = 0;
  param_1[1] = 0;
  *param_1 = 0;
  param_1[6] = 0;
  param_1[5] = 0;
  param_1[4] = 0;
  param_1[10] = 0;
  param_1[9] = 0;
  param_1[8] = 0;
  return;
}


// 100babc0 FUN_100babc0

void __thiscall FUN_100babc0(void *this,float *param_1,float *param_2)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  
  fVar1 = param_1[1];
  fVar2 = param_1[5];
  fVar3 = param_1[9];
  fVar4 = param_1[2];
  fVar5 = *param_2;
  fVar6 = param_1[6];
  fVar7 = param_2[2];
  fVar8 = param_1[10];
  fVar9 = param_2[1];
  *(float *)this = fVar5 * *param_1 + param_1[8] * fVar7 + fVar9 * param_1[4];
  *(float *)((int)this + 8) = fVar7 * fVar8 + fVar9 * fVar6 + fVar5 * fVar4;
  *(float *)((int)this + 4) = fVar7 * fVar3 + fVar9 * fVar2 + fVar5 * fVar1;
  fVar1 = param_1[2];
  fVar2 = param_1[1];
  fVar3 = param_1[9];
  fVar4 = param_1[5];
  fVar5 = param_1[6];
  fVar6 = param_2[4];
  fVar7 = param_1[10];
  fVar8 = param_2[6];
  fVar9 = param_2[5];
  *(float *)((int)this + 0x10) = fVar6 * *param_1 + fVar8 * param_1[8] + fVar9 * param_1[4];
  *(float *)((int)this + 0x14) = fVar8 * fVar3 + fVar9 * fVar4 + fVar6 * fVar2;
  *(float *)((int)this + 0x18) = fVar8 * fVar7 + fVar9 * fVar5 + fVar6 * fVar1;
  fVar1 = param_1[2];
  fVar2 = param_1[1];
  fVar3 = param_1[9];
  fVar4 = param_1[5];
  fVar5 = param_1[6];
  fVar6 = param_2[8];
  fVar7 = param_1[10];
  fVar8 = param_2[10];
  fVar9 = param_2[9];
  *(float *)((int)this + 0x20) = fVar6 * *param_1 + fVar8 * param_1[8] + fVar9 * param_1[4];
  *(float *)((int)this + 0x24) = fVar8 * fVar3 + fVar9 * fVar4 + fVar6 * fVar2;
  *(float *)((int)this + 0x28) = fVar8 * fVar7 + fVar9 * fVar5 + fVar6 * fVar1;
  return;
}


// 100bae30 FUN_100bae30

void __thiscall FUN_100bae30(void *this,float *param_1,float *param_2)

{
  float fVar1;
  float fVar2;
  float fVar3;
  float fVar4;
  float fVar5;
  float fVar6;
  float fVar7;
  float fVar8;
  float fVar9;
  
  fVar1 = param_1[4];
  fVar2 = param_1[8];
  fVar3 = param_2[1];
  fVar4 = *param_2;
  fVar5 = param_1[5];
  fVar6 = param_1[6];
  fVar7 = param_1[9];
  fVar8 = param_2[2];
  fVar9 = param_1[10];
  *(float *)this = fVar8 * param_1[2] + fVar3 * param_1[1] + fVar4 * *param_1;
  *(float *)((int)this + 8) = fVar8 * fVar9 + fVar3 * fVar7 + fVar4 * fVar2;
  *(float *)((int)this + 4) = fVar8 * fVar6 + fVar3 * fVar5 + fVar4 * fVar1;
  fVar1 = param_1[4];
  fVar2 = param_1[8];
  fVar3 = param_2[5];
  fVar4 = param_2[4];
  fVar5 = param_1[5];
  fVar6 = param_1[6];
  fVar7 = param_1[9];
  fVar8 = param_2[6];
  fVar9 = param_1[10];
  *(float *)((int)this + 0x10) = fVar8 * param_1[2] + fVar3 * param_1[1] + fVar4 * *param_1;
  *(float *)((int)this + 0x14) = fVar8 * fVar6 + fVar3 * fVar5 + fVar4 * fVar1;
  *(float *)((int)this + 0x18) = fVar8 * fVar9 + fVar3 * fVar7 + fVar4 * fVar2;
  fVar1 = param_1[4];
  fVar2 = param_1[8];
  fVar3 = param_2[9];
  fVar4 = param_2[8];
  fVar5 = param_1[5];
  fVar6 = param_1[6];
  fVar7 = param_1[9];
  fVar8 = param_2[10];
  fVar9 = param_1[10];
  *(float *)((int)this + 0x20) = fVar8 * param_1[2] + fVar3 * param_1[1] + fVar4 * *param_1;
  *(float *)((int)this + 0x24) = fVar8 * fVar6 + fVar3 * fVar5 + fVar4 * fVar1;
  *(float *)((int)this + 0x28) = fVar8 * fVar9 + fVar3 * fVar7 + fVar4 * fVar2;
  return;
}


// 100bb090 FUN_100bb090

void __thiscall FUN_100bb090(void *this,int param_1,float param_2)

{
  int iVar1;
  int iVar2;
  float *pfVar3;
  float10 fVar4;
  float10 fVar5;
  float local_70 [12];
  float local_40 [15];
  
  FUN_100bab90(local_70);
  iVar1 = param_1 + 1;
  if (iVar1 == 3) {
    iVar1 = 0;
  }
  iVar2 = iVar1 + 1;
  if (iVar2 == 3) {
    iVar2 = 0;
  }
  fVar4 = (float10)fcos((float10)param_2);
  fVar5 = (float10)fsin((float10)param_2);
  local_70[iVar1 * 5] = (float)fVar4;
  local_70[iVar2 * 5] = (float)fVar4;
  local_40[iVar2 + iVar1 * 4 + -0xc] = (float)fVar5;
  local_40[iVar1 + iVar2 * 4 + -0xc] = (float)-fVar5;
  local_70[param_1 * 5] = 1.0;
  FUN_100babc0(local_40,(float *)this,local_70);
  pfVar3 = local_40;
  for (iVar1 = 0xc; iVar1 != 0; iVar1 = iVar1 + -1) {
    *(float *)this = *pfVar3;
    pfVar3 = pfVar3 + 1;
    this = (float *)((int)this + 4);
  }
  return;
}


// 100bb120 FUN_100bb120

void __thiscall FUN_100bb120(void *this,int param_1)

{
  int iVar1;
  void *_Dst;
  int iVar2;
  
                    /* WARNING: Load size is inaccurate */
  iVar2 = (uint)*this * 2;
  if (*this == 0) {
    iVar2 = 2;
  }
  iVar1 = iVar2 * param_1;
  FUN_100954c0();
  _Dst = (void *)FUN_10095480(iVar1);
                    /* WARNING: Load size is inaccurate */
  _memcpy(_Dst,*(void **)((int)this + 4),(uint)*this * param_1);
  iVar1 = *(int *)((int)this + 4);
  if ((iVar1 != 0) && (iVar1 != (int)this + 8)) {
    FUN_100954c0();
    FUN_100954a0(iVar1);
  }
  *(short *)this = (short)iVar2;
  *(void **)((int)this + 4) = _Dst;
  return;
}


// 100bb1b4 _strlwr_s_l_stat

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
  
  uVar2 = DAT_100df7a0 ^ (uint)&stack0xfffffffc;
  if (param_1 == (char *)0x0) {
LAB_100bb1d0:
    pDVar3 = FUN_100ab320();
    DVar7 = 0x16;
  }
  else {
    sVar4 = _strnlen(param_1,param_2);
    if (param_2 <= sVar4) {
      *param_1 = '\0';
      goto LAB_100bb1d0;
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
      goto LAB_100bb323;
    }
    _CchDest = ___crtLCMapStringA(param_3,_LocaleName,0x100,param_1,-1,(LPSTR)0x0,0,
                                  param_3->locinfo->lc_codepage,1);
    if (_CchDest == 0) {
      pDVar3 = FUN_100ab320();
      *pDVar3 = 0x2a;
LAB_100bb25a:
      FUN_100ab320();
      goto LAB_100bb323;
    }
    if (_CchDest <= param_2) {
      if (((int)_CchDest < 1) || (0xffffffe0 / _CchDest == 0)) {
        local_c = (char *)0x0;
      }
      else if (_CchDest + 8 < 0x401) {
        pcVar5 = &stack0xffffffe4;
        local_c = &stack0xffffffe4;
        if (&stack0x00000000 != (undefined1 *)0x1c) {
LAB_100bb2b7:
          local_c = pcVar5 + 8;
        }
      }
      else {
        pcVar5 = (char *)FUN_10002dc0(_CchDest + 8);
        local_c = pcVar5;
        if (pcVar5 != (char *)0x0) {
          pcVar5[0] = -0x23;
          pcVar5[1] = -0x23;
          pcVar5[2] = '\0';
          pcVar5[3] = '\0';
          goto LAB_100bb2b7;
        }
      }
      if (local_c != (char *)0x0) {
        iVar6 = ___crtLCMapStringA(param_3,param_3->locinfo->lc_category[0].wlocale,0x100,param_1,-1
                                   ,local_c,_CchDest,param_3->locinfo->lc_codepage,1);
        if (iVar6 == 0) {
          pDVar3 = FUN_100ab320();
          *pDVar3 = 0x2a;
        }
        else {
          _strcpy_s(param_1,param_2,local_c);
        }
        __freea(local_c);
        goto LAB_100bb323;
      }
      pDVar3 = FUN_100ab320();
      *pDVar3 = 0xc;
      goto LAB_100bb25a;
    }
    *param_1 = '\0';
    pDVar3 = FUN_100ab320();
    DVar7 = 0x22;
  }
  *pDVar3 = DVar7;
  FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
LAB_100bb323:
  iVar6 = __security_check_cookie(uVar2 ^ (uint)&stack0xfffffffc);
  return iVar6;
}


// 100bb335 __strlwr_s_l

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


// 100bb367 __strlwr

/* Library Function - Single Match
    __strlwr
   
   Library: Visual Studio 2005 Release */

char * __cdecl __strlwr(char *_String)

{
  char cVar1;
  char *pcVar2;
  DWORD *pDVar3;
  
  if (DAT_100f5588 == 0) {
    pcVar2 = _String;
    if (_String == (char *)0x0) {
      pDVar3 = FUN_100ab320();
      *pDVar3 = 0x16;
      FUN_100ab2c1((wchar_t *)0x0,(wchar_t *)0x0,(wchar_t *)0x0,0,0);
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


// 100bb3d0 FUN_100bb3d0

void FUN_100bb3d0(void)

{
  FUN_10032410();
  _atexit((_func_4879 *)&LAB_100bbb20);
  return;
}


