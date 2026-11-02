// mode=decompile

/* WARNING: Type propagation algorithm not settling */

uint * FUN_1006a89c(IMAGE_DOS_HEADER *param_1,IMAGE_DOS_HEADER *param_2)

{
  int iVar1;
  undefined4 uVar2;
  int iVar3;
  int iVar4;
  undefined4 uVar5;
  undefined4 uVar6;
  undefined4 uVar7;
  int iVar8;
  undefined8 uVar9;
  char *pcVar10;
  char cVar11;
  word wVar12;
  int iVar13;
  IMAGE_DOS_HEADER *pIVar14;
  uint *puVar15;
  undefined4 *puVar16;
  char *pcVar17;
  word *pwVar18;
  int *piVar19;
  IMAGE_DOS_HEADER *pIVar20;
  IMAGE_DOS_HEADER *pIVar21;
  int *in_ECX;
  uint uVar22;
  int iVar23;
  undefined4 *extraout_EDX;
  int extraout_EDX_00;
  int extraout_EDX_01;
  int extraout_EDX_02;
  uint extraout_EDX_03;
  uint extraout_EDX_04;
  int extraout_EDX_05;
  int extraout_EDX_06;
  int extraout_EDX_07;
  int extraout_EDX_08;
  int extraout_EDX_09;
  int extraout_EDX_10;
  uint extraout_EDX_11;
  uint extraout_EDX_12;
  undefined4 *extraout_EDX_13;
  int extraout_EDX_14;
  uint extraout_EDX_15;
  int extraout_EDX_16;
  uint extraout_EDX_17;
  uint extraout_EDX_18;
  uint extraout_EDX_19;
  uint extraout_EDX_20;
  IMAGE_DOS_HEADER *extraout_EDX_21;
  IMAGE_DOS_HEADER *extraout_EDX_22;
  IMAGE_DOS_HEADER *extraout_EDX_23;
  undefined4 *extraout_EDX_24;
  uint extraout_EDX_25;
  undefined4 *extraout_EDX_26;
  IMAGE_DOS_HEADER *extraout_EDX_27;
  int extraout_EDX_28;
  uint extraout_EDX_29;
  int extraout_EDX_30;
  int extraout_EDX_31;
  int extraout_EDX_32;
  int extraout_EDX_33;
  int extraout_EDX_34;
  int extraout_EDX_35;
  int extraout_EDX_36;
  int extraout_EDX_37;
  uint extraout_EDX_38;
  uint uVar24;
  undefined4 local_1f8 [3];
  undefined4 local_1ec [3];
  undefined4 local_1e0 [3];
  undefined4 local_1d4 [3];
  int local_1c8 [3];
  int local_1bc [3];
  int local_1b0 [3];
  int local_1a4 [3];
  int local_198 [3];
  int local_18c [3];
  int local_180 [3];
  int local_174 [3];
  int local_168;
  int local_164;
  int local_160;
  int local_15c [3];
  undefined1 local_150 [12];
  uint *local_144;
  dword local_140;
  undefined4 local_13c;
  undefined1 local_138 [12];
  uint *local_12c;
  dword local_128;
  word local_124 [2];
  uint *local_120;
  word local_11c [2];
  word local_118 [2];
  int local_114;
  int local_110;
  undefined4 local_10c;
  int *local_108 [8];
  IMAGE_DOS_HEADER *local_e8;
  IMAGE_DOS_HEADER *local_e4;
  undefined4 *local_e0;
  int *local_dc;
  undefined4 *local_d8;
  undefined4 *local_d4;
  int local_d0;
  uint *local_cc;
  IMAGE_DOS_HEADER *local_c8;
  uint *local_c4;
  IMAGE_DOS_HEADER *local_c0;
  uint *local_bc;
  IMAGE_DOS_HEADER *local_b8;
  undefined4 local_b4;
  uint *local_b0 [2];
  uint *local_a8 [9];
  IMAGE_DOS_HEADER *local_84;
  dword local_80;
  int local_7c;
  uint *local_78;
  IMAGE_DOS_HEADER *local_74 [4];
  IMAGE_DOS_HEADER *local_64;
  IMAGE_DOS_HEADER *local_60;
  IMAGE_DOS_HEADER *local_5c [2];
  uint local_54;
  IMAGE_DOS_HEADER *local_50;
  IMAGE_DOS_HEADER *local_4c [7];
  IMAGE_DOS_HEADER *local_30 [2];
  IMAGE_DOS_HEADER *local_28;
  uint uStack_24;
  IMAGE_DOS_HEADER *local_20 [4];
  IMAGE_DOS_HEADER *local_10;
  IMAGE_DOS_HEADER *local_c;
  IMAGE_DOS_HEADER *local_8;
  
  if (param_1 == (IMAGE_DOS_HEADER *)0x0) {
    if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
      return (uint *)0x80004005;
    }
    return (uint *)0x0;
  }
  if (*(int *)((int)param_1 + 4) != 0xd) {
    return (uint *)0x80004005;
  }
  local_8 = (IMAGE_DOS_HEADER *)(*(int *)((int)param_1 + 0x14) * *(int *)((int)param_1 + 0x18));
  local_168 = *(int *)((int)param_1 + 0x24);
  if (*(int *)((int)param_1 + 0x1c) != 0x1a) {
    return (uint *)0x80004005;
  }
  local_160 = in_ECX[7];
  local_28 = (IMAGE_DOS_HEADER *)0x0;
  uStack_24 = 0;
  local_20[0] = (IMAGE_DOS_HEADER *)0x0;
  local_20[1] = (IMAGE_DOS_HEADER *)0x0;
  local_c0 = (IMAGE_DOS_HEADER *)0x0;
  local_bc = (uint *)0x0;
  local_b8 = (IMAGE_DOS_HEADER *)0x0;
  local_b4._0_2_ = 0;
  local_b4._2_2_ = 0;
  local_4c[0] = (IMAGE_DOS_HEADER *)0x0;
  local_4c[1] = (IMAGE_DOS_HEADER *)0x0;
  local_4c[2] = (IMAGE_DOS_HEADER *)0x0;
  local_4c[3] = (IMAGE_DOS_HEADER *)0x0;
  local_164 = in_ECX[0x19];
  local_60 = (IMAGE_DOS_HEADER *)0xe000000;
  local_74[3] = *(IMAGE_DOS_HEADER **)(local_168 + 8);
  if (*(IMAGE_DOS_HEADER **)(local_168 + 8) == (IMAGE_DOS_HEADER *)0x0) {
LAB_1006a9cc:
    local_60 = (IMAGE_DOS_HEADER *)0x0;
  }
  else {
    local_10 = (IMAGE_DOS_HEADER *)0x0;
    do {
      pIVar20 = local_10;
      iVar23 = *(int *)&local_74[3]->e_cparhdr;
      if (iVar23 != 0) {
        iVar13 = *(int *)(iVar23 + 0x18) * *(int *)(iVar23 + 0x14);
        *(int *)((int)&local_c0 + (int)local_10) = iVar13;
        piVar19 = (int *)((int)&local_28 + (int)pIVar20);
        *piVar19 = iVar23;
        pIVar14 = (IMAGE_DOS_HEADER *)FUN_10003970(iVar13 * 4);
        *(IMAGE_DOS_HEADER **)((int)local_4c + (int)pIVar20) = pIVar14;
        local_20[2] = pIVar14;
        if (pIVar14 == (IMAGE_DOS_HEADER *)0x0) goto LAB_1006f800;
        puVar15 = (uint *)FUN_10072735(in_ECX,(uint *)*piVar19,(uint *)pIVar14,(uint *)0x0);
        if ((int)puVar15 < 0) goto LAB_10070486;
        if (*(int *)((int)&local_c0 + (int)local_10) != 0) {
          iVar23 = *(int *)((int)&local_c0 + (int)local_10);
          pIVar20 = local_20[2];
          do {
            local_60 = (IMAGE_DOS_HEADER *)
                       ((uint)local_60 &
                       **(uint **)(*(int *)(in_ECX[2] + 0x14) + *(int *)pIVar20 * 4));
            pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
            iVar23 = iVar23 + -1;
          } while (iVar23 != 0);
        }
      }
      pwVar18 = &local_74[3]->e_maxalloc;
      local_10 = (IMAGE_DOS_HEADER *)&local_10->e_cp;
      local_74[3] = *(IMAGE_DOS_HEADER **)pwVar18;
    } while (*(IMAGE_DOS_HEADER **)pwVar18 != (IMAGE_DOS_HEADER *)0x0);
    if (local_60 == (IMAGE_DOS_HEADER *)0xe000000) goto LAB_1006a9cc;
  }
  pIVar14 = local_8;
  pIVar20 = local_c0;
  in_ECX[0x19] = (int)local_60;
  switch(*(undefined4 *)(*(int *)(*(int *)((int)param_1 + 0x20) + 8) + 0x18)) {
  case 0:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_20[3] = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(uint)local_8);
        if (local_20[3] != (IMAGE_DOS_HEADER *)0x0) {
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[3],(int)local_8);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10100000,
                                         (int)local_20[3],(int *)local_4c[0],(undefined4 *)0x0,0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          uVar24 = 4;
          pIVar21 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20100000);
          pIVar14 = local_4c[0];
          pIVar20 = local_20[3];
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
LAB_1006aa9c:
      iVar23 = *(int *)(local_168 + 0xc);
      if (iVar23 != 0) {
        param_1 = (IMAGE_DOS_HEADER *)local_4c;
        do {
          if (*(int *)(iVar23 + 8) != 0) {
            in_ECX[7] = (int)*(IMAGE_DOS_HEADER **)param_1;
            puVar15 = (uint *)FUN_10072735(in_ECX,*(uint **)(iVar23 + 8),(uint *)0x0,(uint *)0x0);
            if ((int)puVar15 < 0) goto LAB_10070486;
          }
          iVar23 = *(int *)(iVar23 + 0xc);
          param_1 = (IMAGE_DOS_HEADER *)((int)param_1 + 4);
        } while (iVar23 != 0);
      }
      puVar15 = (uint *)0x0;
      goto LAB_10070486;
    }
    break;
  case 1:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
LAB_1006afb9:
        puVar15 = (uint *)FUN_100667c0(in_ECX,(uint)param_1,(int *)local_4c[0],(int)pIVar20,
                                       (int)param_2,(uint)local_8);
        goto LAB_1006aa92;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 2:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        if (local_c0 == (IMAGE_DOS_HEADER *)0x1) {
          puVar15 = (uint *)FUN_1006370b(local_20 + 2,1);
          if ((((int)puVar15 < 0) || (puVar15 = (uint *)FUN_1006370b(&local_50,1), (int)puVar15 < 0)
              ) || (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,1), pIVar20 = local_4c[0],
                   (int)puVar15 < 0)) goto LAB_10070486;
        }
        else {
          if (local_c0 == (IMAGE_DOS_HEADER *)0x2) {
            puVar15 = (uint *)FUN_1006370b(local_20 + 2,1);
            if ((((((int)puVar15 < 0) ||
                  (puVar15 = (uint *)FUN_1006370b(&local_50,1), (int)puVar15 < 0)) ||
                 (puVar15 = (uint *)FUN_1006370b(local_74 + 2,1), (int)puVar15 < 0)) ||
                ((puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,1), (int)puVar15 < 0 ||
                 (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)(local_20 + 2)
                                                 ,(int *)local_4c[0],
                                                 (undefined4 *)&local_4c[0]->e_cp,0),
                 (int)puVar15 < 0)))) ||
               (puVar15 = (uint *)FUN_10067e6e(in_ECX,(uint)param_1,(int *)&local_50,
                                               (int *)(local_74 + 2),(int *)(local_20 + 2),1),
               (int)puVar15 < 0)) goto LAB_10070486;
            pIVar20 = (IMAGE_DOS_HEADER *)&local_50;
            pIVar14 = (IMAGE_DOS_HEADER *)(local_74 + 2);
            goto LAB_1006abaf;
          }
          if (local_c0 != (IMAGE_DOS_HEADER *)0x3) {
            pIVar14 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_c0 * 3);
            if (pIVar14 != (IMAGE_DOS_HEADER *)0x0) {
              uVar24 = 0;
              do {
                local_5c[uVar24] = pIVar14;
                uVar24 = uVar24 + 1;
                pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + (int)pIVar20 * 4);
              } while (uVar24 < 3);
              puVar15 = (uint *)FUN_1006370b((undefined4 *)local_5c[0],(int)local_c0);
              if ((((int)puVar15 < 0) ||
                  (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_5c[1],(int)local_c0),
                  (int)puVar15 < 0)) ||
                 ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_54,(int)local_c0),
                  (int)puVar15 < 0 ||
                  (((puVar15 = (uint *)FUN_1006370b(&local_10,1), (int)puVar15 < 0 ||
                    (puVar15 = (uint *)FUN_1006370b(local_20 + 2,1), (int)puVar15 < 0)) ||
                   (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,1), (int)puVar15 < 0))))))
              goto LAB_10070486;
              local_60 = (IMAGE_DOS_HEADER *)((uint)local_c0 & 0xfffff);
              puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_60 | 0x20500000,
                                             (int)local_5c[0],(int *)local_4c[0],
                                             (undefined4 *)local_4c[0],4);
              if (((((int)puVar15 < 0) ||
                   (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_60 | 0x10100000,
                                                   (int)local_5c[1],(int *)local_5c[0],
                                                   (undefined4 *)0x0,8), (int)puVar15 < 0)) ||
                  (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_60 | 0x20300000,
                                                  local_54,(int *)local_5c[1],
                                                  (undefined4 *)local_5c[0],0x17), (int)puVar15 < 0)
                  ) || ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,
                                                        (uint)local_60 | 0x50000000,(int)&local_10,
                                                        (int *)local_54,(undefined4 *)local_54,6),
                        (int)puVar15 < 0 ||
                        (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,
                                                        (int)(local_20 + 2),(int *)&local_10,
                                                        (undefined4 *)0x0,10), (int)puVar15 < 0))))
              goto LAB_10070486;
              uVar24 = 0x17;
              pIVar21 = (IMAGE_DOS_HEADER *)0x20300001;
              pIVar14 = (IMAGE_DOS_HEADER *)(local_20 + 2);
              pIVar20 = (IMAGE_DOS_HEADER *)&local_10;
              goto LAB_1006aa88;
            }
            goto LAB_1006f800;
          }
          puVar15 = (uint *)FUN_1006370b(local_5c + 1,2);
          if ((((((int)puVar15 < 0) ||
                (puVar15 = (uint *)FUN_1006370b(local_20 + 2,1), (int)puVar15 < 0)) ||
               (puVar15 = (uint *)FUN_1006370b(&local_50,1), (int)puVar15 < 0)) ||
              ((puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,1), (int)puVar15 < 0 ||
               (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)(local_5c + 1),
                                               (int *)local_4c[0],(undefined4 *)&local_4c[0]->e_cp,0
                                              ), (int)puVar15 < 0)))) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)&local_54,
                                             (int *)(local_5c + 1),
                                             (undefined4 *)&local_4c[0]->e_cparhdr,0),
             (int)puVar15 < 0)) goto LAB_10070486;
          pIVar20 = (IMAGE_DOS_HEADER *)&local_54;
        }
        puVar15 = (uint *)FUN_10067e6e(in_ECX,(uint)param_1,(int *)(local_20 + 2),(int *)&local_50,
                                       (int *)pIVar20,1);
        if ((int)puVar15 < 0) goto LAB_10070486;
        pIVar20 = (IMAGE_DOS_HEADER *)(local_20 + 2);
        pIVar14 = (IMAGE_DOS_HEADER *)&local_50;
LAB_1006abaf:
        uVar24 = 0x17;
        pIVar21 = (IMAGE_DOS_HEADER *)0x20200001;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 3:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b(&local_10,1);
        if ((((int)puVar15 < 0) ||
            (puVar15 = (uint *)FUN_1006370b(local_20 + 2,1), (int)puVar15 < 0)) ||
           ((puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,1), (int)puVar15 < 0 ||
            ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,
                                             (uint)local_c0 & 0xfffff | 0x50000000,(int)&local_10,
                                             (int *)local_4c[0],(undefined4 *)local_4c[0],4),
             (int)puVar15 < 0 ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)(local_20 + 2),
                                             (int *)&local_10,(undefined4 *)0x0,8), (int)puVar15 < 0
             )))))) goto LAB_10070486;
        pIVar20 = (IMAGE_DOS_HEADER *)&local_10;
        pIVar14 = (IMAGE_DOS_HEADER *)(local_20 + 2);
        goto LAB_1006abaf;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 4:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        pIVar20 = param_2;
        param_2 = (IMAGE_DOS_HEADER *)0x0;
        goto LAB_1006afb9;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 5:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
LAB_1006b03c:
        puVar15 = (uint *)FUN_10066d3e(in_ECX,(uint)param_1,(int *)local_4c[0],(int *)pIVar20,
                                       (int)param_2,(uint)local_8);
        goto LAB_1006aa92;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 6:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        pIVar20 = local_4c[1];
        if ((int)puVar15 < 0) goto LAB_10070486;
        goto LAB_1006b03c;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 7:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_20[3] = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 2);
        if (local_20[3] != (IMAGE_DOS_HEADER *)0x0) {
          local_10 = (IMAGE_DOS_HEADER *)(local_20[3]->e_magic + (int)local_8 * 4);
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[3],(int)local_8);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_1006370b(extraout_EDX,(int)local_8), (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10100000,
                                         (int)local_20[3],(int *)local_4c[0],(undefined4 *)0x0,0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          uVar22 = 0x14;
          uVar24 = (uint)local_c | 0x10400000;
LAB_1006b0f9:
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,uVar24,(int)local_10,
                                         (int *)local_20[3],(undefined4 *)0x0,uVar22);
          if ((int)puVar15 < 0) goto LAB_10070486;
          uVar24 = 2;
          pIVar14 = local_4c[0];
          pIVar20 = local_10;
          goto LAB_1006b11d;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 8:
    if (((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
        ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
       (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_20[3] = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(uint)local_8);
        if (local_20[3] != (IMAGE_DOS_HEADER *)0x0) {
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[3],(int)local_8);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20100000,
                                         (int)local_20[3],(int *)local_4c[0],
                                         (undefined4 *)local_4c[1],0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          uVar24 = 0;
          pIVar14 = local_20[3];
          pIVar20 = local_4c[2];
LAB_1006ee96:
          pIVar21 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20000000);
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 9:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      puVar15 = (uint *)FUN_10067d39(in_ECX,(uint)param_1,(int *)local_4c[0],(uint)local_c0);
LAB_1006aa92:
      if ((int)puVar15 < 0) goto LAB_10070486;
      goto LAB_1006aa9c;
    }
    break;
  case 10:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        pIVar14 = local_8;
LAB_1006f0a2:
        puVar15 = FUN_10065f1e(in_ECX,(uint)param_1,(uint *)local_4c[0],(undefined4 *)pIVar20,
                               (undefined4 *)param_2,(uint)pIVar14);
        goto LAB_1006aa92;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0xb:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_54 = FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                0x3fe71547652b82fe);
        local_74[1] = (IMAGE_DOS_HEADER *)
                      FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                   0x3fe0000000000000);
        pIVar20 = local_8;
        pIVar14 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 7);
        if (pIVar14 != (IMAGE_DOS_HEADER *)0x0) {
          uVar24 = 0;
          do {
            local_4c[uVar24 + 6] = pIVar14;
            uVar24 = uVar24 + 1;
            pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + (int)pIVar20 * 4);
          } while (uVar24 < 7);
          if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
            pIVar14 = local_20[1];
            do {
              *(uint *)(((int)local_20[0] - (int)local_20[1]) + (int)pIVar14) = local_54;
              *(IMAGE_DOS_HEADER **)pIVar14 = local_74[1];
              pIVar14 = (IMAGE_DOS_HEADER *)&pIVar14->e_cp;
              pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20[-1].e_program + 0xd7);
            } while (pIVar20 != (IMAGE_DOS_HEADER *)0x0);
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[6],(int)local_8);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_30[0],(int)local_8),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_30[1],(int)local_8),
              (int)puVar15 < 0 ||
              (((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8),
                (int)puVar15 < 0 ||
                (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
                (int)puVar15 < 0)) ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0
               )))))) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          local_64 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20500000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)local_4c[6],
                                         (int *)local_4c[0],(undefined4 *)local_20[0],0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_10 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10500000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_10,(int)local_30[0],
                                         (int *)local_4c[6],(undefined4 *)0x0,4);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10100000,
                                              (int)local_30[1],(int *)local_4c[6],(undefined4 *)0x0,
                                              0), (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_10,(int)local_28,
                                              (int *)local_30[1],(undefined4 *)0x0,4),
              (int)puVar15 < 0 ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20400000,
                                              uStack_24,(int *)local_30[0],(undefined4 *)local_28,4)
              , (int)puVar15 < 0)))) goto LAB_10070486;
          uVar24 = 4;
          pIVar21 = local_64;
          pIVar14 = (IMAGE_DOS_HEADER *)uStack_24;
          pIVar20 = local_20[1];
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0xc:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_5c[0] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cp;
        local_5c[1] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cparhdr;
        local_54 = *(uint *)local_4c[0];
        local_cc = *(uint **)&local_4c[1]->e_cparhdr;
        local_c8 = *(IMAGE_DOS_HEADER **)local_4c[1];
        local_c4 = *(uint **)&local_4c[1]->e_cp;
        local_150._0_2_ = local_4c[0]->e_cparhdr;
        local_150._2_2_ = local_4c[0]->e_minalloc;
        local_150._4_4_ = *(undefined4 *)local_4c[0];
        local_150._8_2_ = local_4c[0]->e_cp;
        local_150._10_2_ = local_4c[0]->e_crlc;
        local_78 = *(uint **)&local_4c[1]->e_cp;
        local_74[0] = *(IMAGE_DOS_HEADER **)&local_4c[1]->e_cparhdr;
        local_74[1] = *(IMAGE_DOS_HEADER **)local_4c[1];
        puVar15 = (uint *)FUN_1006370b(&uStack_24,3);
        if (((((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b(local_15c,extraout_EDX_00), (int)puVar15 < 0)) ||
            (puVar15 = (uint *)FUN_1006370b(&local_bc,extraout_EDX_01), (int)puVar15 < 0)) ||
           (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,extraout_EDX_02), (int)puVar15 < 0)
           ) goto LAB_10070486;
        local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
        local_64 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20500000);
        puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)&uStack_24,
                                       (int *)local_5c,&local_cc,0);
        if ((((int)puVar15 < 0) ||
            (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)local_15c,
                                            (int *)local_150,&local_78,0), (int)puVar15 < 0)) ||
           (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10100000,
                                           (int)&local_bc,local_15c,(undefined4 *)0x0,0),
           (int)puVar15 < 0)) goto LAB_10070486;
        pIVar20 = (IMAGE_DOS_HEADER *)&local_bc;
        uVar24 = 0;
        pIVar14 = (IMAGE_DOS_HEADER *)&uStack_24;
LAB_1006b11d:
        pIVar21 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20400000);
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0xd:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_03 & 0xfffff | 0x10d00000);
        pIVar14 = local_4c[0];
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0xe:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_04 & 0xfffff | 0x10e00000);
        pIVar14 = local_4c[0];
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0xf:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar9 = 0x404ca5dc0d03cf26;
LAB_1006e576:
        iVar23 = FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,uVar9);
        piVar19 = (int *)FUN_100635ee(in_ECX,(uint)local_8);
        if (piVar19 != (int *)0x0) {
          pIVar20 = local_8;
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            for (; pIVar20 != (IMAGE_DOS_HEADER *)0x0;
                pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20[-1].e_program + 0xd7)) {
              *piVar19 = iVar23;
              piVar19 = piVar19 + 1;
            }
          }
          goto LAB_1006e5ae;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x10:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        if (local_c0 != (IMAGE_DOS_HEADER *)0x1) {
          iVar1._0_2_ = local_28->e_ip;
          iVar1._2_2_ = local_28->e_cs;
          if (iVar1 == 2) {
            puVar15 = (uint *)FUN_1006370b(&local_cc,3);
            if (((int)puVar15 < 0) ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0
               )) goto LAB_10070486;
            local_74[0] = *(IMAGE_DOS_HEADER **)local_4c[0];
            local_5c[1] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_maxalloc;
            local_74[1] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cp;
            local_54 = *(uint *)&local_4c[0]->e_cparhdr;
            puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500002,(int)&local_cc,
                                           (int *)local_74,local_5c + 1,0);
            if (((int)puVar15 < 0) ||
               (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)&local_c4,
                                               (int *)&local_c8,(undefined4 *)0x0,0),
               (int)puVar15 < 0)) goto LAB_10070486;
            pIVar20 = (IMAGE_DOS_HEADER *)&local_c4;
            uVar24 = 0;
            pIVar14 = (IMAGE_DOS_HEADER *)&local_cc;
LAB_1006b724:
            pIVar21 = (IMAGE_DOS_HEADER *)0x20400001;
          }
          else if (iVar1 == 3) {
            puVar15 = (uint *)FUN_1006370b(local_15c,3);
            if ((((int)puVar15 < 0) ||
                (puVar15 = (uint *)FUN_1006370b(&uStack_24,extraout_EDX_05), (int)puVar15 < 0)) ||
               ((puVar15 = (uint *)FUN_1006370b(&local_bc,extraout_EDX_06), (int)puVar15 < 0 ||
                ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_150,extraout_EDX_07),
                 (int)puVar15 < 0 ||
                 (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8),
                 (int)puVar15 < 0)))))) goto LAB_10070486;
            local_5c[0] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_sp;
            local_5c[1] = *(IMAGE_DOS_HEADER **)local_4c[0]->e_res_4_;
            local_54 = *(uint *)&local_4c[0]->e_cp;
            local_78 = *(uint **)(local_4c[0]->e_res_4_ + 2);
            local_74[0] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cparhdr;
            local_74[1] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_ip;
            puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)local_15c,
                                           (int *)local_5c,&local_78,0);
            if ((int)puVar15 < 0) goto LAB_10070486;
            local_5c[0] = *(IMAGE_DOS_HEADER **)local_4c[0]->e_res_4_;
            local_5c[1] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cp;
            local_54 = *(uint *)&local_4c[0]->e_sp;
            local_78 = *(uint **)&local_4c[0]->e_ip;
            local_74[0] = *(IMAGE_DOS_HEADER **)(local_4c[0]->e_res_4_ + 2);
            local_74[1] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cparhdr;
            puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)&uStack_24,
                                           (int *)local_5c,&local_78,0);
            if ((((int)puVar15 < 0) ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100003,(int)&local_bc,
                                                (int *)&uStack_24,(undefined4 *)0x0,0),
                (int)puVar15 < 0)) ||
               (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20400003,(int)local_150,
                                               local_15c,&local_bc,0), (int)puVar15 < 0))
            goto LAB_10070486;
            local_5c[0] = *(IMAGE_DOS_HEADER **)local_4c[0];
            local_5c[1] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_maxalloc;
            local_54 = *(uint *)&local_4c[0]->e_lfarlc;
            uVar24 = 0;
            pIVar21 = (IMAGE_DOS_HEADER *)0x50000003;
            pIVar14 = (IMAGE_DOS_HEADER *)local_150;
            pIVar20 = (IMAGE_DOS_HEADER *)local_5c;
          }
          else {
            if (iVar1 != 4) goto LAB_1006aa9c;
            puVar15 = (uint *)FUN_1006370b(local_1c8,0xc);
            if (((((int)puVar15 < 0) ||
                 (puVar15 = (uint *)FUN_1006370b(local_198,extraout_EDX_08), (int)puVar15 < 0)) ||
                (puVar15 = (uint *)FUN_1006370b(local_1f8,extraout_EDX_09), (int)puVar15 < 0)) ||
               (((puVar15 = (uint *)FUN_1006370b(local_108 + 2,extraout_EDX_10), (int)puVar15 < 0 ||
                 (puVar15 = (uint *)FUN_1006370b(&local_28,4), (int)puVar15 < 0)) ||
                (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8),
                (int)puVar15 < 0)))) goto LAB_10070486;
            local_b0[0] = *(uint **)local_4c[0]->e_res2_10_;
            local_b0[1]._0_2_ = local_4c[0]->e_res2_10_[8];
            local_b0[1]._2_2_ = local_4c[0]->e_res2_10_[9];
            local_a8[0] = *(uint **)&local_4c[0]->e_lfarlc;
            local_a8[1] = *(uint **)(local_4c[0]->e_res2_10_ + 8);
            local_a8[2] = *(uint **)&local_4c[0]->e_cparhdr;
            local_a8[3] = *(uint **)local_4c[0]->e_res2_10_;
            local_a8[4] = *(uint **)&local_4c[0]->e_lfarlc;
            local_a8[5] = *(uint **)(local_4c[0]->e_res2_10_ + 8);
            local_a8[6] = *(uint **)&local_4c[0]->e_cparhdr;
            local_a8[7] = *(uint **)local_4c[0]->e_res2_10_;
            local_a8[8] = *(uint **)&local_4c[0]->e_cparhdr;
            local_84 = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_lfarlc;
            local_144 = (uint *)local_4c[0]->e_lfanew;
            local_140 = *(dword *)local_4c[0]->e_res_4_;
            local_13c._0_2_ = local_4c[0]->e_res2_10_[2];
            local_13c._2_2_ = local_4c[0]->e_res2_10_[3];
            local_138._0_4_ = *(undefined4 *)(local_4c[0]->e_res2_10_ + 2);
            local_138._4_4_ = local_4c[0]->e_lfanew;
            local_138._8_2_ = local_4c[0]->e_maxalloc;
            local_138._10_2_ = local_4c[0]->e_ss;
            local_12c = (uint *)local_4c[0]->e_lfanew;
            local_128._0_2_ = local_4c[0]->e_maxalloc;
            local_128._2_2_ = local_4c[0]->e_ss;
            local_124 = *(word (*) [2])local_4c[0]->e_res_4_;
            local_120 = *(uint **)local_4c[0]->e_res_4_;
            local_11c = *(word (*) [2])(local_4c[0]->e_res2_10_ + 2);
            local_118[0] = local_4c[0]->e_maxalloc;
            local_118[1] = local_4c[0]->e_ss;
            puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)local_1c8,
                                           (int *)local_b0,&local_144,0);
            if ((((int)puVar15 < 0) ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)local_1bc,
                                                (int *)(local_a8 + 1),(undefined4 *)local_138,0),
                (int)puVar15 < 0)) ||
               ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)local_1b0,
                                                (int *)(local_a8 + 4),&local_12c,0),
                (int)puVar15 < 0 ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)local_1a4,
                                                (int *)(local_a8 + 7),&local_120,0),
                (int)puVar15 < 0)))) goto LAB_10070486;
            local_b0[0] = *(uint **)(local_4c[0]->e_res2_10_ + 8);
            local_b0[1]._0_2_ = local_4c[0]->e_lfarlc;
            local_b0[1]._2_2_ = local_4c[0]->e_ovno;
            local_a8[0] = *(uint **)local_4c[0]->e_res2_10_;
            local_a8[1] = *(uint **)local_4c[0]->e_res2_10_;
            local_a8[2] = *(uint **)(local_4c[0]->e_res2_10_ + 8);
            local_a8[3] = *(uint **)&local_4c[0]->e_cparhdr;
            local_a8[4] = *(uint **)(local_4c[0]->e_res2_10_ + 8);
            local_a8[5] = *(uint **)&local_4c[0]->e_cparhdr;
            local_a8[6] = *(uint **)&local_4c[0]->e_lfarlc;
            local_a8[7] = *(uint **)&local_4c[0]->e_lfarlc;
            local_a8[8] = *(uint **)local_4c[0]->e_res2_10_;
            local_84 = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cparhdr;
            local_144 = *(uint **)(local_4c[0]->e_res2_10_ + 2);
            local_140 = local_4c[0]->e_lfanew;
            local_13c._0_2_ = local_4c[0]->e_res_4_[0];
            local_13c._2_2_ = local_4c[0]->e_res_4_[1];
            local_138._0_4_ = local_4c[0]->e_lfanew;
            local_138._4_2_ = local_4c[0]->e_maxalloc;
            local_138._6_2_ = local_4c[0]->e_ss;
            local_138._8_4_ = *(undefined4 *)(local_4c[0]->e_res2_10_ + 2);
            local_12c = *(uint **)local_4c[0]->e_res_4_;
            local_128 = local_4c[0]->e_lfanew;
            local_124[0] = local_4c[0]->e_maxalloc;
            local_124[1] = local_4c[0]->e_ss;
            local_120 = *(uint **)(local_4c[0]->e_res2_10_ + 2);
            local_11c[0] = local_4c[0]->e_maxalloc;
            local_11c[1] = local_4c[0]->e_ss;
            local_118 = *(word (*) [2])local_4c[0]->e_res_4_;
            puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)local_198,
                                           (int *)local_b0,&local_144,0);
            if (((((int)puVar15 < 0) ||
                 (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)local_18c,
                                                 (int *)(local_a8 + 1),(undefined4 *)local_138,0),
                 (int)puVar15 < 0)) ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)local_180,
                                                (int *)(local_a8 + 4),&local_12c,0),
                (int)puVar15 < 0)) ||
               (((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500003,(int)local_174,
                                                 (int *)(local_a8 + 7),&local_120,0),
                 (int)puVar15 < 0 ||
                 (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100003,(int)local_1f8,
                                                 local_198,(undefined4 *)0x0,0), (int)puVar15 < 0))
                || ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100003,(int)local_1ec,
                                                    local_18c,(undefined4 *)0x0,0), (int)puVar15 < 0
                    || (((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100003,
                                                         (int)local_1e0,local_180,(undefined4 *)0x0,
                                                         0), (int)puVar15 < 0 ||
                         (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100003,
                                                         (int)local_1d4,local_174,(undefined4 *)0x0,
                                                         0), (int)puVar15 < 0)) ||
                        ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20400003,
                                                         (int)(local_108 + 2),local_1c8,local_1f8,0)
                         , (int)puVar15 < 0 ||
                         (((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20400003,
                                                           (int)(local_108 + 5),local_1bc,local_1ec,
                                                           0), (int)puVar15 < 0 ||
                           (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20400003,
                                                           (int)&local_e8,local_1b0,local_1e0,0),
                           (int)puVar15 < 0)) ||
                          (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20400003,
                                                          (int)&local_dc,local_1a4,local_1d4,0),
                          (int)puVar15 < 0)))))))))))) goto LAB_10070486;
            local_b0[0] = *(uint **)&local_4c[0]->e_ip;
            local_b0[1]._0_2_ = local_4c[0]->e_oemid;
            local_b0[1]._2_2_ = local_4c[0]->e_oeminfo;
            local_a8[0] = *(uint **)(local_4c[0]->e_res2_10_ + 6);
            local_a8[1] = *(uint **)&local_4c[0]->e_cp;
            local_a8[2] = *(uint **)&local_4c[0]->e_oemid;
            local_a8[3] = *(uint **)(local_4c[0]->e_res2_10_ + 6);
            local_a8[4] = *(uint **)&local_4c[0]->e_cp;
            local_a8[5] = *(uint **)&local_4c[0]->e_ip;
            local_a8[6] = *(uint **)(local_4c[0]->e_res2_10_ + 6);
            local_a8[7] = *(uint **)&local_4c[0]->e_cp;
            local_a8[8] = *(uint **)&local_4c[0]->e_ip;
            local_84 = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_oemid;
            puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x50000003,(int)&local_28,
                                           (int *)(local_108 + 2),local_b0,0);
            if (((((int)puVar15 < 0) ||
                 (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x50000003,(int)&uStack_24,
                                                 (int *)(local_108 + 5),local_a8 + 1,0),
                 (int)puVar15 < 0)) ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x50000003,(int)local_20,
                                                (int *)&local_e8,local_a8 + 4,0), (int)puVar15 < 0))
               || (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x50000003,
                                                  (int)(local_20 + 1),(int *)&local_dc,local_a8 + 7,
                                                  0), (int)puVar15 < 0)) goto LAB_10070486;
            local_b0[0] = *(uint **)local_4c[0];
            local_b0[1]._0_2_ = local_4c[0]->e_sp;
            local_b0[1]._2_2_ = local_4c[0]->e_csum;
            local_a8[0] = *(uint **)(local_4c[0]->e_res_4_ + 2);
            local_a8[1] = *(uint **)(local_4c[0]->e_res2_10_ + 4);
            uVar24 = 0;
            pIVar21 = (IMAGE_DOS_HEADER *)0x50000004;
            pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
            pIVar20 = (IMAGE_DOS_HEADER *)local_b0;
          }
LAB_1006aa88:
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)pIVar21,(int)param_2,
                                         (int *)pIVar14,(undefined4 *)pIVar20,uVar24);
          goto LAB_1006aa92;
        }
        cVar11 = local_4c[0]->e_magic[1];
        wVar12 = local_4c[0]->e_cblp;
        param_2->e_magic[0] = local_4c[0]->e_magic[0];
        param_2->e_magic[1] = cVar11;
        param_2->e_cblp = wVar12;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x11:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        iVar23 = FUN_100635ee(in_ECX,(int)local_c0 * 2);
        if (iVar23 != 0) {
          uVar24 = 0;
          do {
            local_5c[uVar24 + 1] = (IMAGE_DOS_HEADER *)iVar23;
            uVar24 = uVar24 + 1;
            iVar23 = iVar23 + (int)pIVar20 * 4;
          } while (uVar24 < 2);
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_5c[1],(int)local_c0);
          if (((((int)puVar15 < 0) ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_54,(int)local_c0),
               (int)puVar15 < 0)) ||
              (puVar15 = (uint *)FUN_1006370b(local_20 + 2,1), (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_1006370b(&local_50,1), (int)puVar15 < 0 ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,1), (int)puVar15 < 0))))
          goto LAB_10070486;
          local_60 = (IMAGE_DOS_HEADER *)((uint)local_c0 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_60 | 0x10100000,
                                         (int)local_5c[1],(int *)local_4c[1],(undefined4 *)0x0,0);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_60 | 0x20400000,
                                             local_54,(int *)local_4c[0],(undefined4 *)local_5c[1],0
                                            ), pIVar14 = (IMAGE_DOS_HEADER *)local_54,
             pIVar20 = local_60, (int)puVar15 < 0)) goto LAB_10070486;
LAB_1006c07f:
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)pIVar20 | 0x50000000,
                                         (int)(local_20 + 2),(int *)pIVar14,(undefined4 *)pIVar14,4)
          ;
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10700001,(int)&local_50,
                                             (int *)(local_20 + 2),(undefined4 *)0x0,4),
             (int)puVar15 < 0)) goto LAB_10070486;
          uVar24 = 4;
          pIVar21 = (IMAGE_DOS_HEADER *)0x10300001;
          pIVar14 = (IMAGE_DOS_HEADER *)&local_50;
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x12:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)((uint)local_c0 & 0xfffff | 0x50000000);
        pIVar14 = local_4c[0];
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x13:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_4c[5] = (IMAGE_DOS_HEADER *)in_ECX[8];
        local_4c[6] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cp;
        local_30[0] = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cparhdr;
        uStack_24 = *(uint *)&local_4c[1]->e_cp;
        local_20[1] = *(IMAGE_DOS_HEADER **)&local_4c[1]->e_maxalloc;
        local_30[1] = local_4c[5];
        local_28 = local_4c[5];
        local_20[0] = local_4c[5];
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_11 & 0xfffff | 0x20500000);
        pIVar14 = (IMAGE_DOS_HEADER *)(local_4c + 5);
        pIVar20 = (IMAGE_DOS_HEADER *)&local_28;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x14:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_54 = FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                0x3fe71547652b82fe);
        pIVar20 = local_8;
        puVar15 = (uint *)FUN_100635ee(in_ECX,(int)local_8 * 3);
        if (puVar15 != (uint *)0x0) {
          uVar24 = 0;
          do {
            (&local_cc)[uVar24] = puVar15;
            uVar24 = uVar24 + 1;
            puVar15 = puVar15 + (int)pIVar20;
          } while (uVar24 < 3);
          pIVar14 = local_8;
          puVar15 = local_cc;
          if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
            for (; pIVar14 != (IMAGE_DOS_HEADER *)0x0;
                pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14[-1].e_program + 0xd7)) {
              *puVar15 = local_54;
              puVar15 = puVar15 + 1;
            }
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_c8,(int)local_8);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20500000,
                                         (int)local_c8,(int *)local_4c[0],local_cc,0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          uVar24 = 4;
          pIVar21 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10500000);
          pIVar14 = local_c8;
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x15:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 4;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_12 & 0xfffff | 0x10500000);
        pIVar14 = local_4c[0];
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x16:
    if (((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
        ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
       (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b(local_20 + 2,1);
        if (((int)puVar15 < 0) || (puVar15 = (uint *)FUN_1006370b(&local_50,1), (int)puVar15 < 0))
        goto LAB_10070486;
        local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
        puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x50000000,
                                       (int)(local_20 + 2),(int *)local_4c[1],
                                       (undefined4 *)local_4c[2],0);
        if (((int)puVar15 < 0) ||
           (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20300001,(int)&local_50,
                                           (int *)(local_20 + 2),in_ECX + 9,0x17), (int)puVar15 < 0)
           ) goto LAB_10070486;
        uVar24 = (int)local_8 * 4;
        pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,uVar24);
        if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
          uVar22 = 0;
          do {
            (&local_28)[uVar22] = pIVar20;
            uVar22 = uVar22 + 1;
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + uVar24);
          } while (uVar22 < 4);
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            do {
              *(IMAGE_DOS_HEADER **)(local_20[1]->e_magic + (int)pIVar20 * 4) = local_50;
              pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
            } while (pIVar20 < local_8);
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8);
          if (((((int)puVar15 < 0) ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
               (int)puVar15 < 0)) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],(int)local_8),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0
              || (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10100000,
                                                 (int)local_28,(int *)local_4c[0],(undefined4 *)0x0,
                                                 0), (int)puVar15 < 0)))) goto LAB_10070486;
          local_74[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20400000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],uStack_24,
                                         (int *)local_28,(undefined4 *)local_28,0);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20500000,
                                             (int)local_20[0],(int *)uStack_24,
                                             (undefined4 *)local_20[1],0), (int)puVar15 < 0))
          goto LAB_10070486;
          uVar24 = 0;
          pIVar21 = local_74[3];
          pIVar14 = local_4c[0];
          pIVar20 = local_20[0];
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x17:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_20[3] = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 2);
        if (local_20[3] != (IMAGE_DOS_HEADER *)0x0) {
          local_10 = (IMAGE_DOS_HEADER *)(local_20[3]->e_magic + (int)local_8 * 4);
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[3],(int)local_8);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_1006370b(extraout_EDX_13,(int)local_8), (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10400000,
                                         (int)local_20[3],(int *)local_4c[0],(undefined4 *)0x0,0x14)
          ;
          if ((int)puVar15 < 0) goto LAB_10070486;
          uVar22 = 0x18;
          uVar24 = (uint)local_c | 0x10100000;
          goto LAB_1006b0f9;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x18:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 << 3);
        if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
          uVar24 = 0;
          do {
            local_4c[uVar24 + 5] = pIVar20;
            uVar24 = uVar24 + 1;
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + (int)pIVar14 * 4);
          } while (uVar24 < 8);
          uVar24 = 0;
          do {
            puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[uVar24 + 5],(int)local_8);
            if ((int)puVar15 < 0) goto LAB_10070486;
            uVar24 = extraout_EDX_14 + 1;
          } while (uVar24 < 8);
          puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10300000,
                                         (int)local_4c[5],(int *)local_4c[1],(undefined4 *)0x0,0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_64 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20500000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)local_4c[6],
                                         (int *)local_4c[0],(undefined4 *)local_4c[5],0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_20[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10100000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_30[0],
                                         (int *)local_4c[6],(undefined4 *)0x0,0);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20100000,
                                              (int)local_30[1],(int *)local_4c[6],
                                              (undefined4 *)local_30[0],4), (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10400000,
                                              (int)local_28,(int *)local_30[1],(undefined4 *)0x0,
                                              0x14), (int)puVar15 < 0 ||
              (((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20300000,
                                                uStack_24,(int *)local_4c[6],
                                                (undefined4 *)local_30[0],0x17), (int)puVar15 < 0 ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],
                                                (int)local_20[0],(int *)local_28,(undefined4 *)0x0,0
                                               ), (int)puVar15 < 0)) ||
               (puVar15 = (uint *)FUN_10067564(in_ECX,(uint)param_1,(undefined4 *)local_20[1],
                                               (int *)local_20[0],(int *)local_28,(int *)uStack_24,
                                               (uint)local_8), (int)puVar15 < 0))))))
          goto LAB_10070486;
          uVar24 = 0;
          pIVar21 = local_64;
          pIVar14 = local_20[1];
          pIVar20 = local_4c[1];
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x19:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_15 & 0xfffff | 0x10400000);
        pIVar14 = local_4c[0];
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x1a:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      local_10 = (IMAGE_DOS_HEADER *)((int)local_8 << 2);
      local_4c[1] = (IMAGE_DOS_HEADER *)FUN_10003970(local_10);
      if ((local_4c[1] == (IMAGE_DOS_HEADER *)0x0) ||
         (puVar15 = (uint *)FUN_100635ee(in_ECX,(int)pIVar14 * 10), puVar15 == (uint *)0x0))
      goto LAB_1006f800;
      uVar24 = 0;
      do {
        local_a8[uVar24] = puVar15;
        puVar15 = (uint *)((int)puVar15 + (int)local_10);
        uVar24 = uVar24 + 1;
      } while (uVar24 < 10);
      puVar15 = (uint *)FUN_1006370b(local_a8[0],(int)local_8);
      if (((((int)puVar15 < 0) ||
           (puVar15 = (uint *)FUN_1006370b(local_a8[1],(int)local_8), (int)puVar15 < 0)) ||
          (puVar15 = (uint *)FUN_1006370b(local_a8[2],(int)local_8), (int)puVar15 < 0)) ||
         ((((puVar15 = (uint *)FUN_1006370b(local_a8[3],(int)local_8), (int)puVar15 < 0 ||
            (puVar15 = (uint *)FUN_1006370b(local_a8[4],(int)local_8), (int)puVar15 < 0)) ||
           ((puVar15 = (uint *)FUN_1006370b(local_a8[5],(int)local_8), (int)puVar15 < 0 ||
            ((puVar15 = (uint *)FUN_1006370b(local_a8[6],(int)local_8), (int)puVar15 < 0 ||
             (puVar15 = (uint *)FUN_1006370b(local_a8[7],(int)local_8), (int)puVar15 < 0)))))) ||
          (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[1],(int)local_8), (int)puVar15 < 0)
          ))) goto LAB_10070486;
      local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
      local_20[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10100000);
      puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_a8[0],
                                     (int *)local_4c[0],(undefined4 *)0x0,0);
      if (((((int)puVar15 < 0) ||
           (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20100000,
                                           (int)local_a8[1],(int *)local_a8[0],
                                           (undefined4 *)local_4c[0],4), (int)puVar15 < 0)) ||
          (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_a8[2],
                                          (int *)local_a8[1],(undefined4 *)0x0,8), (int)puVar15 < 0)
          ) || (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20300000,
                                               (int)local_a8[3],(int *)local_a8[2],local_a8[1],0x17)
               , (int)puVar15 < 0)) goto LAB_10070486;
      local_74[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20400000);
      puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],(int)local_a8[4],
                                     (int *)local_a8[3],local_a8[1],4);
      if ((((int)puVar15 < 0) ||
          (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10600000,
                                          (int)local_a8[5],(int *)local_a8[4],(undefined4 *)0x0,0),
          (int)puVar15 < 0)) ||
         ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_a8[6],
                                          (int *)local_a8[5],(undefined4 *)0x0,0), (int)puVar15 < 0
          || ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10400000,
                                              (int)local_a8[7],(int *)local_a8[6],(undefined4 *)0x0,
                                              0x14), (int)puVar15 < 0 ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],
                                              (int)local_4c[1],(int *)local_a8[5],local_a8[7],2),
              (int)puVar15 < 0)))))) goto LAB_10070486;
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b(local_a8[8],(int)local_8);
        if (((((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_84,(int)local_8), (int)puVar15 < 0)
             ) || (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8),
                  (int)puVar15 < 0)) ||
           (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10500000,
                                           (int)local_a8[8],(int *)local_4c[1],(undefined4 *)0x0,4),
           (int)puVar15 < 0)) goto LAB_10070486;
        puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10300000,(int)local_84
                                       ,(int *)local_a8[8],(undefined4 *)0x0,4);
        pIVar14 = local_4c[0];
        pIVar20 = local_84;
        pIVar21 = local_c;
        goto joined_r0x1006f799;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x1b:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 6);
        if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
          uVar24 = 0;
          do {
            local_4c[uVar24 + 7] = pIVar20;
            uVar24 = uVar24 + 1;
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + (int)pIVar14 * 4);
          } while (uVar24 < 6);
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_30[0],(int)local_8);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_30[1],(int)local_8),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8), (int)puVar15 < 0
              || ((((puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
                    (int)puVar15 < 0 ||
                    (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],(int)local_8),
                    (int)puVar15 < 0)) ||
                   (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[1],(int)local_8),
                   (int)puVar15 < 0)) ||
                  (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8),
                  (int)puVar15 < 0)))))) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10d00000,
                                         (int)local_30[0],(int *)local_4c[0],(undefined4 *)0x0,0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_20[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10100000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_30[1],
                                         (int *)local_30[0],(undefined4 *)0x0,0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_54 = (uint)local_c | 0x20100000;
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,local_54,(int)local_28,
                                         (int *)local_30[1],(undefined4 *)local_30[0],4);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10e00000,
                                              uStack_24,(int *)local_4c[0],(undefined4 *)0x0,0),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],
                                              (int)local_20[0],(int *)uStack_24,(undefined4 *)0x0,0)
              , (int)puVar15 < 0 ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,local_54,(int)local_20[1],
                                              (int *)local_20[0],(undefined4 *)uStack_24,4),
              (int)puVar15 < 0)))) goto LAB_10070486;
          uVar24 = 4;
          pIVar14 = local_28;
          pIVar20 = local_20[1];
          goto LAB_1006b11d;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x1c:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        if ((*(byte *)(in_ECX[2] + 0x6e) & 0x10) == 0) {
          uVar24 = (int)local_8 * 4;
          pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,uVar24);
          if (pIVar20 == (IMAGE_DOS_HEADER *)0x0) goto LAB_1006f800;
          uVar22 = 0;
          do {
            (&local_28)[uVar22] = pIVar20;
            uVar22 = uVar22 + 1;
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + uVar24);
          } while (uVar22 < 4);
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],(int)local_8),
              (int)puVar15 < 0 ||
              ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[1],(int)local_8),
               (int)puVar15 < 0 ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0
               )))))) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          local_20[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10100000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_28,
                                         (int *)local_4c[0],(undefined4 *)0x0,0x40);
          if (((int)puVar15 < 0) ||
             ((((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20400000,
                                                uStack_24,(int *)local_4c[0],(undefined4 *)local_28,
                                                0x40), (int)puVar15 < 0 ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20500000,
                                                (int)local_20[0],(int *)uStack_24,
                                                (undefined4 *)uStack_24,0x40), (int)puVar15 < 0)) ||
               (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],
                                               (int)local_20[1],(int *)local_20[0],(undefined4 *)0x0
                                               ,0x40), (int)puVar15 < 0)) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20300000,
                                              (int)param_2,(int *)local_20[1],
                                              (undefined4 *)local_20[0],0x57), (int)puVar15 < 0))))
          goto LAB_10070486;
          local_10 = (IMAGE_DOS_HEADER *)0x0;
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            iVar23 = (int)local_4c[0] - (int)param_2;
            param_1 = param_2;
            do {
              puVar15 = (uint *)FUN_10063a6f(in_ECX,*(uint *)(iVar23 + (int)param_1),
                                             (double *)&local_28);
              if ((int)puVar15 < 0) goto LAB_10070486;
              if (((double)CONCAT44(uStack_24,local_28) <= *(double *)(in_ECX + 0x2a)) ||
                 (*(double *)(in_ECX + 0x28) <= (double)CONCAT44(local_20[1],local_20[0]))) {
                if (((double)CONCAT44(uStack_24,local_28) == *(double *)(in_ECX + 0x28)) ||
                   ((double)CONCAT44(local_20[1],local_20[0]) == *(double *)(in_ECX + 0x2a))) {
                  pIVar20 = (IMAGE_DOS_HEADER *)in_ECX[9];
                  goto LAB_1006ce9b;
                }
              }
              else {
                pIVar20 = (IMAGE_DOS_HEADER *)in_ECX[8];
LAB_1006ce9b:
                *(IMAGE_DOS_HEADER **)param_1 = pIVar20;
              }
              pcVar17 = local_10->e_magic;
              local_10 = (IMAGE_DOS_HEADER *)(pcVar17 + 1);
              param_1 = (IMAGE_DOS_HEADER *)&param_1->e_cp;
            } while ((IMAGE_DOS_HEADER *)(pcVar17 + 1) < local_8);
          }
        }
        else {
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            do {
              *(int *)(param_2->e_magic + (int)pIVar20 * 4) = in_ECX[8];
              pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
            } while (pIVar20 < local_8);
          }
        }
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x1d:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        if ((*(byte *)(in_ECX[2] + 0x6e) & 0x10) == 0) {
          uVar24 = (int)local_8 * 4;
          pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,uVar24);
          if (pIVar20 == (IMAGE_DOS_HEADER *)0x0) goto LAB_1006f800;
          uVar22 = 0;
          do {
            (&local_28)[uVar22] = pIVar20;
            uVar22 = uVar22 + 1;
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + uVar24);
          } while (uVar22 < 4);
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],(int)local_8),
              (int)puVar15 < 0 ||
              ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[1],(int)local_8),
               (int)puVar15 < 0 ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0
               )))))) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10300000,
                                         (int)local_28,(int *)local_4c[0],(undefined4 *)0x0,0x40);
          if (((int)puVar15 < 0) ||
             (((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20500000,
                                               uStack_24,(int *)local_28,(undefined4 *)local_28,0x40
                                              ), (int)puVar15 < 0 ||
               (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10100000,
                                               (int)local_20[0],(int *)uStack_24,(undefined4 *)0x0,
                                               0x40), (int)puVar15 < 0)) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20300000,
                                              (int)param_2,(int *)local_20[0],
                                              (undefined4 *)uStack_24,0x57), (int)puVar15 < 0))))
          goto LAB_10070486;
          local_10 = (IMAGE_DOS_HEADER *)0x0;
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            iVar23 = (int)local_4c[0] - (int)param_2;
            param_1 = param_2;
            do {
              puVar15 = (uint *)FUN_10063a6f(in_ECX,*(uint *)((int)param_1 + iVar23),
                                             (double *)&local_28);
              if ((int)puVar15 < 0) goto LAB_10070486;
              if (((double)CONCAT44(uStack_24,local_28) <= *(double *)(in_ECX + 0x2a)) ||
                 (*(double *)(in_ECX + 0x28) <= (double)CONCAT44(local_20[1],local_20[0]))) {
                if (((double)CONCAT44(uStack_24,local_28) == *(double *)(in_ECX + 0x28)) ||
                   ((double)CONCAT44(local_20[1],local_20[0]) == *(double *)(in_ECX + 0x2a))) {
                  pIVar20 = (IMAGE_DOS_HEADER *)in_ECX[8];
                  goto LAB_1006d0b2;
                }
              }
              else {
                pIVar20 = (IMAGE_DOS_HEADER *)in_ECX[9];
LAB_1006d0b2:
                *(IMAGE_DOS_HEADER **)param_1 = pIVar20;
              }
              pcVar17 = local_10->e_magic;
              local_10 = (IMAGE_DOS_HEADER *)(pcVar17 + 1);
              param_1 = (IMAGE_DOS_HEADER *)&param_1->e_cp;
            } while ((IMAGE_DOS_HEADER *)(pcVar17 + 1) < local_8);
          }
        }
        else {
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            do {
              *(int *)(param_2->e_magic + (int)pIVar20 * 4) = in_ECX[9];
              pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
            } while (pIVar20 < local_8);
          }
        }
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x1e:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        if ((*(byte *)(in_ECX[2] + 0x6e) & 0x10) == 0) {
          uVar24 = (int)local_8 * 4;
          pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,uVar24);
          if (pIVar20 == (IMAGE_DOS_HEADER *)0x0) goto LAB_1006f800;
          uVar22 = 0;
          do {
            (&local_28)[uVar22] = pIVar20;
            uVar22 = uVar22 + 1;
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + uVar24);
          } while (uVar22 < 4);
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8);
          if (((((int)puVar15 < 0) ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
               (int)puVar15 < 0)) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],(int)local_8),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[1],(int)local_8),
              (int)puVar15 < 0 ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0)
              ))) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20500000,
                                         (int)local_28,(int *)local_4c[0],(undefined4 *)local_4c[0],
                                         0x40);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_20[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10100000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],uStack_24,
                                         (int *)local_28,(undefined4 *)0x0,0x40);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_54 = (uint)local_c | 0x20300000;
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,local_54,(int)local_20[0],
                                         (int *)local_28,(undefined4 *)uStack_24,0x57);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],
                                              (int)local_20[1],(int *)local_20[0],(undefined4 *)0x0,
                                              0x5a), (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,local_54,(int)param_2,
                                             (int *)local_20[1],(undefined4 *)local_20[0],0x57),
             (int)puVar15 < 0)) goto LAB_10070486;
          local_10 = (IMAGE_DOS_HEADER *)0x0;
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            iVar23 = (int)local_4c[0] - (int)param_2;
            param_1 = param_2;
            do {
              puVar15 = (uint *)FUN_10063a6f(in_ECX,*(uint *)((int)param_1 + iVar23),
                                             (double *)&local_28);
              if ((int)puVar15 < 0) goto LAB_10070486;
              if ((((*(double *)(in_ECX + 0x2a) < (double)CONCAT44(uStack_24,local_28)) ||
                   ((double)CONCAT44(local_20[1],local_20[0]) < *(double *)(in_ECX + 0x28))) ||
                  ((double)CONCAT44(uStack_24,local_28) == *(double *)(in_ECX + 0x28))) ||
                 ((double)CONCAT44(local_20[1],local_20[0]) == *(double *)(in_ECX + 0x2a))) {
                *(IMAGE_DOS_HEADER **)param_1 = (IMAGE_DOS_HEADER *)in_ECX[9];
              }
              pcVar17 = local_10->e_magic;
              local_10 = (IMAGE_DOS_HEADER *)(pcVar17 + 1);
              param_1 = (IMAGE_DOS_HEADER *)&param_1->e_cp;
            } while ((IMAGE_DOS_HEADER *)(pcVar17 + 1) < local_8);
          }
        }
        else {
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            do {
              *(int *)(param_2->e_magic + (int)pIVar20 * 4) = in_ECX[9];
              pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
            } while (pIVar20 < local_8);
          }
        }
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x1f:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_20[3] = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(uint)local_8);
        if (local_20[3] != (IMAGE_DOS_HEADER *)0x0) {
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[3],(int)local_8);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10500000,
                                         (int)local_20[3],(int *)local_4c[1],(undefined4 *)0x0,4);
          pIVar14 = local_4c[0];
          pIVar20 = local_20[3];
          pIVar21 = local_c;
          goto joined_r0x1006f799;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x20:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b(local_20 + 2,1);
        if (extraout_EDX_16 != 1) {
          if ((((int)puVar15 < 0) || (puVar15 = (uint *)FUN_1006370b(&local_50,1), (int)puVar15 < 0)
              ) || (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8),
                   (int)puVar15 < 0)) goto LAB_10070486;
          pIVar14 = local_4c[0];
          pIVar20 = (IMAGE_DOS_HEADER *)(extraout_EDX_17 & 0xfffff);
          goto LAB_1006c07f;
        }
        if ((((int)puVar15 < 0) ||
            (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,1), (int)puVar15 < 0)) ||
           (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)(local_20 + 2),
                                           (int *)local_4c[0],(undefined4 *)0x0,0), (int)puVar15 < 0
           )) goto LAB_10070486;
        uVar24 = 4;
        pIVar21 = (IMAGE_DOS_HEADER *)0x20100001;
        pIVar14 = local_4c[0];
        pIVar20 = (IMAGE_DOS_HEADER *)(local_20 + 2);
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x21:
    if (((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
        ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
       (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_10067564(in_ECX,(uint)param_1,(undefined4 *)param_2,(int *)local_4c[0]
                                       ,(int *)local_4c[1],(int *)local_4c[2],(uint)local_8);
        goto LAB_1006aa92;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x22:
    if ((((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
         ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
        (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) && (local_8 == (IMAGE_DOS_HEADER *)0x4)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        if ((*(byte *)(in_ECX[2] + 0x6c) & 0x40) == 0) {
          puVar16 = (undefined4 *)FUN_100635ee(in_ECX,6);
          if (puVar16 == (undefined4 *)0x0) goto LAB_1006f800;
          uVar24 = 0;
          do {
            local_4c[uVar24 + 7] = (IMAGE_DOS_HEADER *)(puVar16 + uVar24);
            uVar24 = uVar24 + 1;
          } while (uVar24 < 6);
          puVar15 = (uint *)FUN_1006370b(puVar16,6);
          if (((((int)puVar15 < 0) ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,4), (int)puVar15 < 0)) ||
              ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)local_30[0],
                                               (int *)local_4c[0],(undefined4 *)0x0,0),
               (int)puVar15 < 0 ||
               ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20200001,(int)local_30[1],
                                                (int *)local_30[0],(undefined4 *)local_4c[0],0x17),
                (int)puVar15 < 0 ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)&param_2->e_cp,
                                                (int *)local_30[1],(undefined4 *)local_4c[0],0),
                (int)puVar15 < 0)))))) ||
             ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)local_28,
                                              (int *)local_4c[1],(undefined4 *)0x0,0),
              (int)puVar15 < 0 ||
              ((((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20200001,uStack_24,
                                                 (int *)local_28,(undefined4 *)local_4c[1],0x17),
                 (int)puVar15 < 0 ||
                 (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)local_20[0],
                                                 (int *)local_30[1],(undefined4 *)uStack_24,0x17),
                 (int)puVar15 < 0)) ||
                (puVar15 = (uint *)FUN_1006790d(in_ECX,(uint)param_1,(int *)local_20[1],
                                                (int *)local_4c[1],(int *)local_4c[2],1),
                (int)puVar15 < 0)) ||
               ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,
                                                (int)&param_2->e_cparhdr,(int *)local_20[0],
                                                (undefined4 *)local_20[1],4), (int)puVar15 < 0 ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10000001,(int)param_2,
                                                in_ECX + 8,(undefined4 *)0x0,0x17), (int)puVar15 < 0
                )))))))) goto LAB_10070486;
          uVar24 = 0x17;
          param_2 = (IMAGE_DOS_HEADER *)&param_2->e_maxalloc;
          pIVar21 = (IMAGE_DOS_HEADER *)(IMAGE_DOS_HEADER_10000000.e_magic + 1);
          pIVar14 = (IMAGE_DOS_HEADER *)(in_ECX + 8);
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        }
        else {
          local_28 = *(IMAGE_DOS_HEADER **)local_4c[0];
          uStack_24 = *(uint *)local_4c[1];
          local_20[0] = *(IMAGE_DOS_HEADER **)local_4c[2];
          local_20[1] = *(IMAGE_DOS_HEADER **)local_4c[2];
          puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,4);
          if ((int)puVar15 < 0) goto LAB_10070486;
          uVar24 = 0;
          pIVar21 = (IMAGE_DOS_HEADER *)0x50100004;
          pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        }
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x23:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_54 = FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                0x3ff62e42fefa39ef);
        pIVar20 = local_8;
        pIVar14 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 2);
        if (pIVar14 != (IMAGE_DOS_HEADER *)0x0) {
          uVar24 = 0;
          do {
            local_74[uVar24] = pIVar14;
            uVar24 = uVar24 + 1;
            pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + (int)pIVar20 * 4);
          } while (uVar24 < 2);
LAB_1006d749:
          pIVar14 = local_8;
          pIVar21 = local_74[0];
          if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
            for (; pIVar14 != (IMAGE_DOS_HEADER *)0x0;
                pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14[-1].e_program + 0xd7)) {
              pIVar21->e_magic[0] = (undefined1)local_54;
              pIVar21->e_magic[1] = local_54._1_1_;
              pIVar21->e_cblp = local_54._2_2_;
              pIVar21 = (IMAGE_DOS_HEADER *)&pIVar21->e_cp;
            }
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_74[1],(int)local_8);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10600000,
                                         (int)local_74[1],(int *)local_4c[0],(undefined4 *)0x0,0);
          pIVar14 = local_74[0];
          pIVar20 = local_74[1];
          pIVar21 = local_c;
          goto joined_r0x1006f799;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x24:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_54 = FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                0x3fc9999999999999);
        pIVar20 = local_8;
        pIVar14 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 2);
        if (pIVar14 != (IMAGE_DOS_HEADER *)0x0) {
          uVar24 = 0;
          do {
            local_74[uVar24] = pIVar14;
            uVar24 = uVar24 + 1;
            pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + (int)pIVar20 * 4);
          } while (uVar24 < 2);
          goto LAB_1006d749;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x25:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_18 & 0xfffff | 0x10600000);
        pIVar14 = local_4c[0];
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x26:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_19 & 0xfffff | 0x20100000);
        pIVar14 = local_4c[0];
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x27:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_20 & 0xfffff | 0x20000000);
        pIVar14 = local_4c[0];
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x28:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      local_10 = (IMAGE_DOS_HEADER *)((int)local_8 << 2);
      local_4c[1] = (IMAGE_DOS_HEADER *)FUN_10003970(local_10);
      if ((local_4c[1] == (IMAGE_DOS_HEADER *)0x0) ||
         (puVar15 = (uint *)FUN_100635ee(in_ECX,(int)pIVar14 * 10), puVar15 == (uint *)0x0))
      goto LAB_1006f800;
      uVar24 = 0;
      do {
        local_a8[uVar24] = puVar15;
        puVar15 = (uint *)((int)puVar15 + (int)local_10);
        uVar24 = uVar24 + 1;
      } while (uVar24 < 10);
      puVar15 = (uint *)FUN_1006370b(local_a8[0],(int)local_8);
      if (((((((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b(local_a8[1],(int)local_8), (int)puVar15 < 0)) ||
            (puVar15 = (uint *)FUN_1006370b(local_a8[2],(int)local_8), (int)puVar15 < 0)) ||
           ((puVar15 = (uint *)FUN_1006370b(local_a8[3],(int)local_8), (int)puVar15 < 0 ||
            (puVar15 = (uint *)FUN_1006370b(local_a8[4],(int)local_8), (int)puVar15 < 0)))) ||
          ((puVar15 = (uint *)FUN_1006370b(local_a8[5],(int)local_8), (int)puVar15 < 0 ||
           ((puVar15 = (uint *)FUN_1006370b(local_a8[6],(int)local_8), (int)puVar15 < 0 ||
            (puVar15 = (uint *)FUN_1006370b(local_a8[7],(int)local_8), (int)puVar15 < 0)))))) ||
         ((puVar15 = (uint *)FUN_1006370b(local_a8[8],(int)local_8), (int)puVar15 < 0 ||
          (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[1],(int)local_8), (int)puVar15 < 0)
          ))) goto LAB_10070486;
      local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
      puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10200000,
                                     (int)local_a8[0],(int *)local_4c[0],(undefined4 *)0x0,0);
      if (((int)puVar15 < 0) ||
         (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10400000,
                                         (int)local_a8[1],(int *)local_a8[0],(undefined4 *)0x0,0x14)
         , (int)puVar15 < 0)) goto LAB_10070486;
      local_20[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10100000);
      puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_a8[2],
                                     (int *)local_a8[1],(undefined4 *)0x0,0x18);
      if ((int)puVar15 < 0) goto LAB_10070486;
      local_74[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20400000);
      puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],(int)local_a8[3],
                                     (int *)local_a8[2],local_a8[0],2);
      if (((int)puVar15 < 0) ||
         (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_a8[4],
                                         (int *)local_4c[0],(undefined4 *)0x0,0), (int)puVar15 < 0))
      goto LAB_10070486;
      local_20[2] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20200000);
      puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[2],(int)local_a8[5],
                                     (int *)local_4c[0],local_a8[4],0x17);
      if ((((int)puVar15 < 0) ||
          (((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_a8[6],
                                            (int *)local_a8[1],(undefined4 *)0x0,0),
            (int)puVar15 < 0 ||
            (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[2],(int)local_a8[7],
                                            (int *)local_a8[6],local_a8[1],0x17), (int)puVar15 < 0))
           || (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20500000,
                                              (int)local_a8[8],(int *)local_a8[5],local_a8[7],0x17),
              (int)puVar15 < 0)))) ||
         (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],(int)local_4c[1],
                                         (int *)local_a8[3],local_a8[8],2), (int)puVar15 < 0))
      goto LAB_10070486;
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)local_84,(int)local_8);
        if ((((int)puVar15 < 0) ||
            (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
           || (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_84,
                                              (int *)local_4c[1],(undefined4 *)0x0,2),
              (int)puVar15 < 0)) goto LAB_10070486;
        uVar24 = 0x10;
        pIVar21 = local_74[3];
        pIVar14 = local_4c[0];
        pIVar20 = local_84;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x29:
  case 0x2a:
  case 0x2b:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        iVar23 = FUN_100635ee(in_ECX,(uint)local_8);
        if (iVar23 == 0) goto LAB_1006f800;
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
          do {
            *(IMAGE_DOS_HEADER **)(iVar23 + (int)pIVar20 * 4) = *(IMAGE_DOS_HEADER **)local_4c[0];
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
          } while (pIVar20 < local_8);
        }
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        pIVar14 = extraout_EDX_21;
        pIVar20 = local_4c[1];
joined_r0x1006dcd1:
        if ((int)puVar15 < 0) goto LAB_10070486;
        pIVar21 = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
LAB_1006f7a9:
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)((uint)pIVar21 | 0x20500000);
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x2c:
  case 0x2f:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        iVar23 = FUN_100635ee(in_ECX,(uint)local_8);
        if (iVar23 == 0) goto LAB_1006f800;
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
          do {
            *(IMAGE_DOS_HEADER **)(iVar23 + (int)pIVar20 * 4) = *(IMAGE_DOS_HEADER **)local_4c[1];
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
          } while (pIVar20 < local_8);
        }
LAB_1006e5ae:
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        pIVar14 = local_4c[0];
        pIVar20 = extraout_EDX_23;
        goto joined_r0x1006dcd1;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x2d:
  case 0x2e:
  case 0x30:
  case 0x31:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 == (IMAGE_DOS_HEADER *)0x0) goto LAB_1006aa9c;
      pIVar20 = *(IMAGE_DOS_HEADER **)&local_28->e_sp;
      local_10 = pIVar20;
      if ((pIVar20 != (IMAGE_DOS_HEADER *)0x0) &&
         (iVar13._0_2_ = pIVar20->e_cp, iVar13._2_2_ = pIVar20->e_crlc, iVar13 == 9)) {
        iVar8._0_2_ = pIVar20->e_sp;
        iVar8._2_2_ = pIVar20->e_csum;
        if (iVar8 != 1) {
          uVar7._0_2_ = pIVar20->e_lfarlc;
          uVar7._2_2_ = pIVar20->e_ovno;
          uVar2._0_2_ = pIVar20->e_ip;
          uVar2._2_2_ = pIVar20->e_cs;
          FUN_10079a2c(local_138,2,uVar2,uVar7,*(undefined4 *)pIVar20->e_res_4_,0x400);
          pIVar20 = (IMAGE_DOS_HEADER *)local_138;
        }
        puVar15 = (uint *)FUN_100681c4(in_ECX,(int)pIVar20,(uint *)local_4c[0],(uint)local_c0);
        if ((int)puVar15 < 0) goto LAB_10070486;
        local_74[3] = *(IMAGE_DOS_HEADER **)local_10->e_res_4_;
        local_20[3] = *(IMAGE_DOS_HEADER **)(uStack_24 + 0x10);
        local_64 = *(IMAGE_DOS_HEADER **)&local_10->e_lfarlc;
        if ((local_20[3] != (IMAGE_DOS_HEADER *)0x0) &&
           (iVar3._0_2_ = local_20[3]->e_cp, iVar3._2_2_ = local_20[3]->e_crlc, iVar3 == 9)) {
          iVar4._0_2_ = local_20[3]->e_sp;
          iVar4._2_2_ = local_20[3]->e_csum;
          if (iVar4 == 1) {
            puVar15 = (uint *)FUN_100681c4(in_ECX,(int)local_20[3],(uint *)local_4c[1],
                                           (uint)local_bc);
            if ((int)puVar15 < 0) goto LAB_10070486;
            local_60 = *(IMAGE_DOS_HEADER **)local_20[3]->e_res_4_;
            local_c = *(IMAGE_DOS_HEADER **)&local_20[3]->e_lfarlc;
          }
          else {
            uVar5._0_2_ = local_20[3]->e_lfarlc;
            uVar5._2_2_ = local_20[3]->e_ovno;
            uVar6._0_2_ = local_20[3]->e_ip;
            uVar6._2_2_ = local_20[3]->e_cs;
            FUN_10079a2c(local_a8 + 1,2,uVar6,uVar5,*(undefined4 *)local_20[3]->e_res_4_,0x800);
            puVar15 = (uint *)FUN_100681c4(in_ECX,(int)(local_a8 + 1),(uint *)local_4c[1],
                                           (uint)local_bc);
            if ((int)puVar15 < 0) goto LAB_10070486;
            local_60 = *(IMAGE_DOS_HEADER **)&local_20[3]->e_lfarlc;
            local_c = *(IMAGE_DOS_HEADER **)local_20[3]->e_res_4_;
          }
          pIVar20 = local_64;
          if ((local_74[3] == local_60) &&
             (local_8 == (IMAGE_DOS_HEADER *)((int)local_c * (int)local_64))) {
            uVar24 = FUN_1006477e(in_ECX,(int *)local_4c[0],*(int **)&local_10->e_sp,(uint)local_64,
                                  (uint)local_74[3]);
            pIVar14 = local_c;
            if (uVar24 == 0) {
              pIVar14 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)pIVar20 * 5);
              if (pIVar14 == (IMAGE_DOS_HEADER *)0x0) goto LAB_1006f800;
              uVar24 = 0;
              do {
                local_4c[uVar24 + 8] = pIVar14;
                uVar24 = uVar24 + 1;
                pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + (int)pIVar20 * 4);
              } while (uVar24 < 5);
              local_10 = (IMAGE_DOS_HEADER *)0x0;
              if (local_c != (IMAGE_DOS_HEADER *)0x0) {
                local_20[3] = param_2;
                do {
                  param_2 = (IMAGE_DOS_HEADER *)0x0;
                  if (local_60 != (IMAGE_DOS_HEADER *)0x0) {
                    do {
                      if (local_64 != (IMAGE_DOS_HEADER *)0x0) {
                        local_20[2] = local_64;
                        local_54 = (uint)(local_4c[1]->e_magic +
                                         (int)(local_10->e_magic + (int)param_2 * (int)local_c) * 4)
                        ;
                        local_74[1] = (IMAGE_DOS_HEADER *)((int)local_74[3] * 4);
                        pcVar17 = local_4c[0]->e_magic + (int)param_2 * 4;
                        pIVar20 = local_28;
                        do {
                          uVar7 = *(undefined4 *)pcVar17;
                          pcVar17 = pcVar17 + (int)local_74[3] * 4;
                          *(undefined4 *)(((int)local_30[1] - (int)local_28) + (int)pIVar20) = uVar7
                          ;
                          cVar11 = ((IMAGE_DOS_HEADER *)local_54)->e_magic[1];
                          wVar12 = ((IMAGE_DOS_HEADER *)local_54)->e_cblp;
                          pIVar20->e_magic[0] = ((IMAGE_DOS_HEADER *)local_54)->e_magic[0];
                          pIVar20->e_magic[1] = cVar11;
                          pIVar20->e_cblp = wVar12;
                          pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
                          local_20[2] = (IMAGE_DOS_HEADER *)(local_20[2][-1].e_program + 0xd7);
                        } while (local_20[2] != (IMAGE_DOS_HEADER *)0x0);
                      }
                      puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],(int)local_64);
                      if ((int)puVar15 < 0) goto LAB_10070486;
                      local_54 = (uint)local_64 & 0xfffff;
                      puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,local_54 | 0x20500000,
                                                     (int)local_20[0],(int *)local_30[1],
                                                     (undefined4 *)local_28,0);
                      if (((int)puVar15 < 0) ||
                         ((pIVar20 = local_20[0], param_2 != (IMAGE_DOS_HEADER *)0x0 &&
                          ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[1],(int)local_64),
                           (int)puVar15 < 0 ||
                           (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,
                                                           local_54 | 0x20400000,(int)local_20[1],
                                                           (int *)uStack_24,
                                                           (undefined4 *)local_20[0],0),
                           pIVar20 = local_20[1], (int)puVar15 < 0)))))) goto LAB_10070486;
                      pIVar14 = (IMAGE_DOS_HEADER *)uStack_24;
                      for (uVar24 = (uint)local_64 & 0x3fffffff; uVar24 != 0; uVar24 = uVar24 - 1) {
                        cVar11 = pIVar20->e_magic[1];
                        wVar12 = pIVar20->e_cblp;
                        pIVar14->e_magic[0] = pIVar20->e_magic[0];
                        pIVar14->e_magic[1] = cVar11;
                        pIVar14->e_cblp = wVar12;
                        pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
                        pIVar14 = (IMAGE_DOS_HEADER *)&pIVar14->e_cp;
                      }
                      param_2 = (IMAGE_DOS_HEADER *)(param_2->e_magic + 1);
                      for (iVar23 = 0; iVar23 != 0; iVar23 = iVar23 + -1) {
                        pIVar14->e_magic[0] = pIVar20->e_magic[0];
                        pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
                        pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + 1);
                      }
                    } while (param_2 < local_60);
                  }
                  pIVar14 = (IMAGE_DOS_HEADER *)0x0;
                  pIVar20 = local_20[3];
                  if (local_64 != (IMAGE_DOS_HEADER *)0x0) {
                    do {
                      *(IMAGE_DOS_HEADER **)pIVar20 =
                           *(IMAGE_DOS_HEADER **)((char *)uStack_24 + (int)pIVar14 * 4);
                      pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + 1);
                      pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + (int)local_c * 4);
                    } while (pIVar14 < local_64);
                  }
                  pcVar17 = local_10->e_magic;
                  local_10 = (IMAGE_DOS_HEADER *)(pcVar17 + 1);
                  local_20[3] = (IMAGE_DOS_HEADER *)&local_20[3]->e_cp;
                } while ((IMAGE_DOS_HEADER *)(pcVar17 + 1) < local_c);
              }
            }
            else {
              uVar24 = FUN_10064912(in_ECX,(int *)local_4c[1],*(int **)&local_20[3]->e_sp,
                                    (uint)local_60,(uint)local_c);
              if (uVar24 == 0) {
                pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)pIVar14 * 5);
                if (pIVar20 == (IMAGE_DOS_HEADER *)0x0) goto LAB_1006f800;
                uVar24 = 0;
                do {
                  local_4c[uVar24 + 8] = pIVar20;
                  uVar24 = uVar24 + 1;
                  pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + (int)local_c * 4);
                } while (uVar24 < 5);
                local_74[2] = (IMAGE_DOS_HEADER *)0x0;
                if (local_64 != (IMAGE_DOS_HEADER *)0x0) {
                  local_20[2] = (IMAGE_DOS_HEADER *)0x0;
                  local_50 = param_2;
                  do {
                    param_2 = (IMAGE_DOS_HEADER *)0x0;
                    if (local_74[3] != (IMAGE_DOS_HEADER *)0x0) {
                      do {
                        if (local_c != (IMAGE_DOS_HEADER *)0x0) {
                          local_54 = (uint)(local_4c[0]->e_magic +
                                           (int)(param_2->e_magic + (int)local_20[2]->e_magic) * 4);
                          pwVar18 = (word *)(local_4c[1]->e_magic + (int)local_c * (int)param_2 * 4)
                          ;
                          local_10 = local_c;
                          pIVar20 = local_28;
                          do {
                            *(IMAGE_DOS_HEADER **)
                             (((int)local_30[1] - (int)local_28) + (int)pIVar20) =
                                 *(IMAGE_DOS_HEADER **)local_54;
                            *(IMAGE_DOS_HEADER **)pIVar20 = *(IMAGE_DOS_HEADER **)pwVar18;
                            pwVar18 = pwVar18 + 2;
                            pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
                            local_10 = (IMAGE_DOS_HEADER *)(local_10[-1].e_program + 0xd7);
                          } while (local_10 != (IMAGE_DOS_HEADER *)0x0);
                        }
                        puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],(int)local_c);
                        if ((int)puVar15 < 0) goto LAB_10070486;
                        local_54 = (uint)local_c & 0xfffff;
                        puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,local_54 | 0x20500000,
                                                       (int)local_20[0],(int *)local_30[1],
                                                       (undefined4 *)local_28,0);
                        if (((int)puVar15 < 0) ||
                           ((pIVar20 = local_20[0], param_2 != (IMAGE_DOS_HEADER *)0x0 &&
                            ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[1],(int)local_c)
                             , (int)puVar15 < 0 ||
                             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,
                                                             local_54 | 0x20400000,(int)local_20[1],
                                                             (int *)uStack_24,
                                                             (undefined4 *)local_20[0],0),
                             pIVar20 = local_20[1], (int)puVar15 < 0)))))) goto LAB_10070486;
                        pIVar14 = (IMAGE_DOS_HEADER *)uStack_24;
                        for (uVar24 = (uint)local_c & 0x3fffffff; uVar24 != 0; uVar24 = uVar24 - 1)
                        {
                          cVar11 = pIVar20->e_magic[1];
                          wVar12 = pIVar20->e_cblp;
                          pIVar14->e_magic[0] = pIVar20->e_magic[0];
                          pIVar14->e_magic[1] = cVar11;
                          pIVar14->e_cblp = wVar12;
                          pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
                          pIVar14 = (IMAGE_DOS_HEADER *)&pIVar14->e_cp;
                        }
                        param_2 = (IMAGE_DOS_HEADER *)(param_2->e_magic + 1);
                        for (iVar23 = 0; iVar23 != 0; iVar23 = iVar23 + -1) {
                          pIVar14->e_magic[0] = pIVar20->e_magic[0];
                          pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
                          pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + 1);
                        }
                      } while (param_2 < local_74[3]);
                    }
                    pIVar14 = (IMAGE_DOS_HEADER *)0x0;
                    pIVar20 = local_50;
                    if (local_c != (IMAGE_DOS_HEADER *)0x0) {
                      do {
                        *(IMAGE_DOS_HEADER **)pIVar20 =
                             *(IMAGE_DOS_HEADER **)((char *)uStack_24 + (int)pIVar14 * 4);
                        pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + 1);
                        pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
                      } while (pIVar14 < local_c);
                    }
                    local_20[2] = (IMAGE_DOS_HEADER *)
                                  (local_74[3]->e_magic + (int)local_20[2]->e_magic);
                    pcVar17 = local_74[2]->e_magic;
                    local_74[2] = (IMAGE_DOS_HEADER *)(pcVar17 + 1);
                    local_50 = (IMAGE_DOS_HEADER *)(local_50->e_magic + (int)local_c * 4);
                  } while ((IMAGE_DOS_HEADER *)(pcVar17 + 1) < local_64);
                }
              }
              else {
                local_20[3] = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(uint)local_bc);
                if (local_20[3] == (IMAGE_DOS_HEADER *)0x0) goto LAB_1006f800;
                if (local_60 != (IMAGE_DOS_HEADER *)0x0) {
                  local_10 = local_4c[1];
                  local_50 = local_60;
                  local_74[2] = local_20[3];
                  do {
                    pIVar20 = local_10;
                    pIVar21 = local_74[2];
                    if (pIVar14 != (IMAGE_DOS_HEADER *)0x0) {
                      do {
                        local_20[2] = pIVar14;
                        cVar11 = pIVar20->e_magic[1];
                        wVar12 = pIVar20->e_cblp;
                        pIVar21->e_magic[0] = pIVar20->e_magic[0];
                        pIVar21->e_magic[1] = cVar11;
                        pIVar21->e_cblp = wVar12;
                        pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
                        pIVar21 = (IMAGE_DOS_HEADER *)(pIVar21->e_magic + (int)local_60 * 4);
                        pIVar14 = (IMAGE_DOS_HEADER *)(local_20[2][-1].e_program + 0xd7);
                      } while ((IMAGE_DOS_HEADER *)(local_20[2][-1].e_program + 0xd7) !=
                               (IMAGE_DOS_HEADER *)0x0);
                      local_20[2] = (IMAGE_DOS_HEADER *)0x0;
                      pIVar14 = local_c;
                    }
                    local_74[2] = (IMAGE_DOS_HEADER *)&local_74[2]->e_cp;
                    local_10 = (IMAGE_DOS_HEADER *)(local_10->e_magic + (int)pIVar14 * 4);
                    local_50 = (IMAGE_DOS_HEADER *)(local_50[-1].e_program + 0xd7);
                  } while (local_50 != (IMAGE_DOS_HEADER *)0x0);
                }
                puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
                if ((int)puVar15 < 0) goto LAB_10070486;
                local_20[2] = (IMAGE_DOS_HEADER *)0x0;
                if (local_64 != (IMAGE_DOS_HEADER *)0x0) {
                  local_50 = param_2;
                  local_8 = local_4c[0];
                  do {
                    local_10 = (IMAGE_DOS_HEADER *)0x0;
                    if (local_c != (IMAGE_DOS_HEADER *)0x0) {
                      local_54 = (uint)local_74[3] & 0xfffff | 0x50000000;
                      param_2 = local_50;
                      local_74[2] = local_20[3];
                      do {
                        puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,local_54,(int)param_2,
                                                       (int *)local_8,(undefined4 *)local_74[2],0);
                        if ((int)puVar15 < 0) goto LAB_10070486;
                        pcVar17 = local_10->e_magic;
                        local_10 = (IMAGE_DOS_HEADER *)(pcVar17 + 1);
                        param_2 = (IMAGE_DOS_HEADER *)&param_2->e_cp;
                        local_74[2] = (IMAGE_DOS_HEADER *)(local_74[2]->e_magic + (int)local_60 * 4)
                        ;
                      } while ((IMAGE_DOS_HEADER *)(pcVar17 + 1) < local_c);
                    }
                    pcVar17 = local_20[2]->e_magic;
                    local_20[2] = (IMAGE_DOS_HEADER *)(pcVar17 + 1);
                    local_8 = (IMAGE_DOS_HEADER *)(local_8->e_magic + (int)local_74[3] * 4);
                    local_50 = (IMAGE_DOS_HEADER *)(local_50->e_magic + (int)local_c * 4);
                  } while ((IMAGE_DOS_HEADER *)(pcVar17 + 1) < local_64);
                }
              }
            }
            goto LAB_1006aa9c;
          }
        }
      }
    }
    break;
  case 0x32:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,1);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0x10;
        pIVar21 = (IMAGE_DOS_HEADER *)((uint)local_c0 & 0xfffff | 0x50200000);
        pIVar14 = local_4c[0];
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x33:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        if (local_8 == (IMAGE_DOS_HEADER *)0x1) {
          iVar23 = FUN_100635ee(in_ECX,4);
          if (iVar23 != 0) {
            uVar24 = 0;
            do {
              (&local_28)[uVar24] = (IMAGE_DOS_HEADER *)(iVar23 + uVar24 * 4);
              uVar24 = uVar24 + 1;
            } while (uVar24 < 4);
            puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,1);
            if (((((int)puVar15 < 0) ||
                 (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,1), (int)puVar15 < 0)) ||
                (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],1), (int)puVar15 < 0)) ||
               ((((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[1],1), (int)puVar15 < 0 ||
                  (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,1), (int)puVar15 < 0)) ||
                 ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)local_28,
                                                  (int *)local_4c[0],(undefined4 *)0x0,0),
                  (int)puVar15 < 0 ||
                  ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20200001,uStack_24,
                                                   (int *)local_28,(undefined4 *)local_4c[0],0x17),
                   (int)puVar15 < 0 ||
                   (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20200001,(int)local_20[0],
                                                   (int *)local_4c[0],(undefined4 *)local_28,0x17),
                   (int)puVar15 < 0)))))) ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)local_20[1],
                                                (int *)local_20[0],(undefined4 *)0x0,0x1a),
                (int)puVar15 < 0)))) goto LAB_10070486;
            uVar24 = 0x12;
            pIVar14 = (IMAGE_DOS_HEADER *)uStack_24;
            pIVar20 = local_20[1];
            goto LAB_1006b724;
          }
        }
        else {
          puVar15 = (uint *)FUN_1006370b(local_20 + 2,1);
          if (((int)puVar15 < 0) || (puVar15 = (uint *)FUN_1006370b(&local_50,1), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x50000000,
                                         (int)(local_20 + 2),(int *)local_4c[0],
                                         (undefined4 *)local_4c[0],4);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10700001,(int)&local_50,
                                             (int *)(local_20 + 2),(undefined4 *)0x0,4),
             (int)puVar15 < 0)) goto LAB_10070486;
          iVar23 = FUN_100635ee(in_ECX,(uint)local_8);
          if (iVar23 != 0) {
            pIVar20 = (IMAGE_DOS_HEADER *)0x0;
            if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
              do {
                *(IMAGE_DOS_HEADER **)(iVar23 + (int)pIVar20 * 4) = local_50;
                pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
              } while (pIVar20 < local_8);
            }
            puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
            pIVar14 = local_4c[0];
            pIVar20 = extraout_EDX_22;
            pIVar21 = local_c;
            goto joined_r0x1006f799;
          }
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x34:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006790d(in_ECX,(uint)param_1,(int *)param_2,(int *)local_4c[0],
                                       (int *)local_4c[1],(uint)local_8);
        goto LAB_1006aa92;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x35:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar9 = 0x3f91df46aaaaaaab;
        goto LAB_1006e576;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x36:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b(local_20 + 2,1);
        if ((((int)puVar15 < 0) || (puVar15 = (uint *)FUN_1006370b(&local_50,1), (int)puVar15 < 0))
           || (puVar15 = (uint *)FUN_1006370b(local_74 + 2,1), (int)puVar15 < 0)) goto LAB_10070486;
        local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
        puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x50000000,
                                       (int)(local_20 + 2),(int *)local_4c[0],
                                       (undefined4 *)local_4c[1],0);
        if ((((int)puVar15 < 0) ||
            (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20400001,(int)&local_50,
                                            (int *)(local_20 + 2),local_20 + 2,0), (int)puVar15 < 0)
            ) || (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)(local_74 + 2)
                                                 ,(int *)&local_50,(undefined4 *)0x0,0),
                 (int)puVar15 < 0)) goto LAB_10070486;
        iVar23 = FUN_100635ee(in_ECX,(int)local_8 * 2);
        if (iVar23 != 0) {
          uVar24 = 0;
          do {
            local_5c[uVar24 + 1] = (IMAGE_DOS_HEADER *)iVar23;
            uVar24 = uVar24 + 1;
            iVar23 = iVar23 + (int)local_8 * 4;
          } while (uVar24 < 2);
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            do {
              *(IMAGE_DOS_HEADER **)(local_5c[1]->e_magic + (int)pIVar20 * 4) = local_74[2];
              pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + 1);
            } while (pIVar20 < local_8);
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_54,(int)local_8);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0)
              ) || (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20500000,
                                                   local_54,(int *)local_4c[1],extraout_EDX_24,0),
                   (int)puVar15 < 0)) goto LAB_10070486;
          uVar24 = 0;
          pIVar14 = (IMAGE_DOS_HEADER *)local_54;
          pIVar20 = local_4c[0];
          goto LAB_1006b11d;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x37:
    if (((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
        ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
       (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b(&local_10,1);
        if ((((((int)puVar15 < 0) || (puVar15 = (uint *)FUN_1006370b(&local_60,1), (int)puVar15 < 0)
              ) || ((puVar15 = (uint *)FUN_1006370b(&local_110,1), (int)puVar15 < 0 ||
                    (((((puVar15 = (uint *)FUN_1006370b(local_74 + 3,1), (int)puVar15 < 0 ||
                        (puVar15 = (uint *)FUN_1006370b(local_20 + 3,1), (int)puVar15 < 0)) ||
                       (puVar15 = (uint *)FUN_1006370b(&local_114,1), (int)puVar15 < 0)) ||
                      ((puVar15 = (uint *)FUN_1006370b(&local_10c,1), (int)puVar15 < 0 ||
                       (puVar15 = (uint *)FUN_1006370b(local_20 + 2,1), (int)puVar15 < 0)))) ||
                     (puVar15 = (uint *)FUN_1006370b(&local_50,1), (int)puVar15 < 0)))))) ||
            ((puVar15 = (uint *)FUN_1006370b(&local_d0,1), (int)puVar15 < 0 ||
             (puVar15 = (uint *)FUN_1006370b(local_74 + 2,1), (int)puVar15 < 0)))) ||
           ((puVar15 = (uint *)FUN_1006370b(local_74 + 1,1), (int)puVar15 < 0 ||
            (((puVar15 = (uint *)FUN_1006370b(&local_80,1), (int)puVar15 < 0 ||
              (puVar15 = (uint *)FUN_1006370b(&local_7c,1), (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_1006370b(&local_54,1), (int)puVar15 < 0)))))) goto LAB_10070486;
        local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
        puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x50000000,
                                       (int)&local_10,(int *)local_4c[0],(undefined4 *)local_4c[1],0
                                      );
        if ((((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)&local_60,
                                              (int *)&local_10,&local_10,4), (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)&local_110,
                                              (int *)local_4c[2],(undefined4 *)local_4c[2],4),
              (int)puVar15 < 0 ||
              ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)(local_74 + 3),
                                               (int *)&local_60,(undefined4 *)0x0,8),
               (int)puVar15 < 0 ||
               (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20400001,(int)(local_20 + 3),
                                               in_ECX + 8,local_74 + 3,0), (int)puVar15 < 0)))))) ||
            (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)&local_114,
                                            &local_110,local_20 + 3,0), (int)puVar15 < 0)) ||
           ((((((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10100001,(int)&local_10c,
                                                &local_114,(undefined4 *)0x0,0), (int)puVar15 < 0 ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20400001,(int)(local_20 + 2),
                                                in_ECX + 8,&local_10c,0), (int)puVar15 < 0)) ||
               (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20300001,(int)&local_50,
                                               (int *)(local_20 + 2),in_ECX + 9,0x17),
               (int)puVar15 < 0)) ||
              ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)&local_d0,
                                               (int *)(local_20 + 2),&local_50,4), (int)puVar15 < 0
               || (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,
                                                  (int)(local_74 + 2),(int *)local_4c[2],&local_50,0
                                                 ), (int)puVar15 < 0)))) ||
             ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20500001,(int)(local_74 + 1),
                                              (int *)(local_74 + 2),&local_10,0), (int)puVar15 < 0
              || ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10700001,(int)&local_80,
                                                  &local_d0,(undefined4 *)0x0,4), (int)puVar15 < 0
                  || (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x10300001,(int)&local_7c,
                                                     (int *)&local_80,(undefined4 *)0x0,4),
                     (int)puVar15 < 0)))))) ||
            (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,0x20400001,(int)&local_54,
                                            (int *)(local_74 + 1),&local_7c,0), pIVar20 = local_8,
            (int)puVar15 < 0)))) goto LAB_10070486;
        pIVar14 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 5);
        if (pIVar14 != (IMAGE_DOS_HEADER *)0x0) {
          uVar24 = 0;
          do {
            local_4c[uVar24 + 8] = pIVar14;
            uVar24 = uVar24 + 1;
            pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + (int)pIVar20 * 4);
          } while (uVar24 < 5);
          if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
            pIVar14 = local_20[1];
            do {
              *(IMAGE_DOS_HEADER **)(((int)local_20[0] - (int)local_20[1]) + (int)pIVar14) =
                   local_74[2];
              pIVar14->e_magic[0] = (undefined1)local_54;
              pIVar14->e_magic[1] = local_54._1_1_;
              pIVar14->e_cblp = local_54._2_2_;
              pIVar14 = (IMAGE_DOS_HEADER *)&pIVar14->e_cp;
              pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20[-1].e_program + 0xd7);
            } while (pIVar20 != (IMAGE_DOS_HEADER *)0x0);
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_30[1],(int)local_8);
          if (((((int)puVar15 < 0) ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8),
               (int)puVar15 < 0)) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
              (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_64 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20500000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)local_30[1],
                                         (int *)local_20[0],(undefined4 *)local_4c[0],0);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)local_28,
                                              (int *)local_20[1],(undefined4 *)local_4c[1],0),
              (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10100000,
                                             uStack_24,(int *)local_28,(undefined4 *)0x0,0),
             (int)puVar15 < 0)) goto LAB_10070486;
          uVar24 = 0;
          pIVar14 = local_30[1];
          pIVar20 = (IMAGE_DOS_HEADER *)uStack_24;
          goto LAB_1006b11d;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x38:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_54 = FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                0x3fe0000000000000);
        uVar24 = (int)local_8 * 4;
        pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,uVar24);
        if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
          uVar22 = 0;
          do {
            (&local_28)[uVar22] = pIVar20;
            uVar22 = uVar22 + 1;
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + uVar24);
          } while (uVar22 < 4);
          pIVar20 = local_8;
          pIVar14 = local_20[1];
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            for (; pIVar20 != (IMAGE_DOS_HEADER *)0x0;
                pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20[-1].e_program + 0xd7)) {
              pIVar14->e_magic[0] = (undefined1)local_54;
              pIVar14->e_magic[1] = local_54._1_1_;
              pIVar14->e_cblp = local_54._2_2_;
              pIVar14 = (IMAGE_DOS_HEADER *)&pIVar14->e_cp;
            }
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],(int)local_8),
              (int)puVar15 < 0 ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0)
              ))) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          local_74[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20400000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],(int)local_28,
                                         (int *)local_4c[0],(undefined4 *)local_20[1],0);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10400000,
                                              uStack_24,(int *)local_28,(undefined4 *)0x0,0x14),
              (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10100000,
                                             (int)local_20[0],(int *)uStack_24,(undefined4 *)0x0,
                                             0x18), (int)puVar15 < 0)) goto LAB_10070486;
          uVar24 = 2;
          pIVar21 = local_74[3];
          pIVar14 = local_28;
          pIVar20 = local_20[0];
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x39:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 4;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_25 & 0xfffff | 0x10700000);
        pIVar14 = local_4c[0];
        pIVar20 = (IMAGE_DOS_HEADER *)0x0;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x3a:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 3);
        if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
          uVar24 = 0;
          do {
            local_5c[uVar24] = pIVar20;
            uVar24 = uVar24 + 1;
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + (int)pIVar14 * 4);
          } while (uVar24 < 3);
          if (pIVar14 != (IMAGE_DOS_HEADER *)0x0) {
            pIVar20 = local_5c[1];
            do {
              *(int *)(((int)local_5c[0] - (int)local_5c[1]) + (int)pIVar20) = in_ECX[9];
              iVar23 = in_ECX[8];
              pIVar20->e_magic[0] = (char)iVar23;
              pIVar20->e_magic[1] = (char)((uint)iVar23 >> 8);
              pIVar20->e_cblp = (short)((uint)iVar23 >> 0x10);
              pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
              pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14[-1].e_program + 0xd7);
            } while (pIVar14 != (IMAGE_DOS_HEADER *)0x0);
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_54,(int)local_8);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20100000,local_54,
                                         (int *)local_4c[0],(undefined4 *)local_5c[0],4);
          if ((int)puVar15 < 0) goto LAB_10070486;
          uVar24 = 0x14;
          pIVar14 = (IMAGE_DOS_HEADER *)local_54;
          pIVar20 = local_5c[1];
          goto LAB_1006ee96;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x3b:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = (int)local_8 * 4;
        pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,uVar24);
        if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
          uVar22 = 0;
          do {
            (&local_28)[uVar22] = pIVar20;
            uVar22 = uVar22 + 1;
            pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + uVar24);
          } while (uVar22 < 4);
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],(int)local_8),
              (int)puVar15 < 0 ||
              ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[1],(int)local_8),
               (int)puVar15 < 0 ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0
               )))))) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          local_20[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10100000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_28,
                                         (int *)local_4c[0],(undefined4 *)0x0,0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_20[2] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20200000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[2],uStack_24,
                                         (int *)local_28,(undefined4 *)local_4c[0],0x17);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[2],
                                              (int)local_20[0],(int *)local_4c[0],
                                              (undefined4 *)local_28,0x17), (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_20[1]
                                             ,(int *)local_20[0],(undefined4 *)0x0,0x1a),
             (int)puVar15 < 0)) goto LAB_10070486;
          uVar24 = 0x12;
          pIVar14 = (IMAGE_DOS_HEADER *)uStack_24;
          pIVar20 = local_20[1];
          goto LAB_1006b11d;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x3c:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        pIVar20 = param_2;
        param_2 = (IMAGE_DOS_HEADER *)0x0;
        pIVar14 = local_8;
        goto LAB_1006f0a2;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x3d:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      iVar23 = (int)local_c0 << 2;
      local_4c[1] = (IMAGE_DOS_HEADER *)FUN_10003970(iVar23);
      if (local_4c[1] != (IMAGE_DOS_HEADER *)0x0) {
        local_4c[2] = (IMAGE_DOS_HEADER *)FUN_10003970(iVar23);
        if (local_4c[2] != (IMAGE_DOS_HEADER *)0x0) {
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[1],(int)local_c0);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b(extraout_EDX_26,(int)local_c0), pIVar20 = local_4c[1],
             param_2 = extraout_EDX_27, pIVar14 = local_c0, (int)puVar15 < 0)) goto LAB_10070486;
          goto LAB_1006f0a2;
        }
      }
LAB_1006f800:
      puVar15 = (uint *)0x8007000e;
      goto LAB_10070486;
    }
    break;
  case 0x3e:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_54 = FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                0x3fe71547652b82fe);
        local_74[1] = (IMAGE_DOS_HEADER *)
                      FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                   0x3fe0000000000000);
        pIVar20 = local_8;
        pIVar14 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 << 3);
        if (pIVar14 != (IMAGE_DOS_HEADER *)0x0) {
          uVar24 = 0;
          do {
            local_4c[uVar24 + 5] = pIVar14;
            uVar24 = uVar24 + 1;
            pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + (int)pIVar20 * 4);
          } while (uVar24 < 8);
          if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
            pIVar14 = local_20[1];
            do {
              *(uint *)(((int)local_20[0] - (int)local_20[1]) + (int)pIVar14) = local_54;
              *(IMAGE_DOS_HEADER **)pIVar14 = local_74[1];
              pIVar14 = (IMAGE_DOS_HEADER *)&pIVar14->e_cp;
              pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20[-1].e_program + 0xd7);
            } while (pIVar20 != (IMAGE_DOS_HEADER *)0x0);
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[5],(int)local_8);
          if (((((int)puVar15 < 0) ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[6],(int)local_8),
               (int)puVar15 < 0)) ||
              (((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_30[0],(int)local_8),
                (int)puVar15 < 0 ||
                ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_30[1],(int)local_8),
                 (int)puVar15 < 0 ||
                 (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,(int)local_8),
                 (int)puVar15 < 0)))) ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,(int)local_8),
               (int)puVar15 < 0)))) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          local_64 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20500000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)local_4c[5],
                                         (int *)local_4c[0],(undefined4 *)local_20[0],0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_10 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10500000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_10,(int)local_4c[6],
                                         (int *)local_4c[5],(undefined4 *)0x0,4);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_20[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10100000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_30[0],
                                         (int *)local_4c[5],(undefined4 *)0x0,0);
          if (((((int)puVar15 < 0) ||
               (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_10,(int)local_30[1],
                                               (int *)local_30[0],(undefined4 *)0x0,4),
               (int)puVar15 < 0)) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_28,
                                              (int *)local_30[1],(undefined4 *)0x0,8),
              (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20400000,
                                             uStack_24,(int *)local_4c[6],(undefined4 *)local_28,0),
             (int)puVar15 < 0)) goto LAB_10070486;
          uVar24 = 0;
          pIVar21 = local_64;
          pIVar14 = (IMAGE_DOS_HEADER *)uStack_24;
          pIVar20 = local_20[1];
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x3f:
    if (((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
        ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
       (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_74[1] = (IMAGE_DOS_HEADER *)
                      FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                   0xc000000000000000);
        local_80 = FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                0x4008000000000000);
        pIVar20 = local_8;
        piVar19 = (int *)FUN_100635ee(in_ECX,(int)local_8 * 0xe);
        if (piVar19 != (int *)0x0) {
          uVar24 = 0;
          do {
            local_108[uVar24] = piVar19;
            uVar24 = uVar24 + 1;
            piVar19 = piVar19 + (int)pIVar20;
          } while (uVar24 < 0xe);
          uVar24 = 0;
          do {
            puVar15 = (uint *)FUN_1006370b(local_108[uVar24],(int)local_8);
            if ((int)puVar15 < 0) goto LAB_10070486;
            uVar24 = extraout_EDX_28 + 1;
          } while (uVar24 < 10);
          if (local_8 != (IMAGE_DOS_HEADER *)0x0) {
            local_7c = (int)local_d8 - (int)local_dc;
            local_d0 = (int)local_d4 - (int)local_dc;
            local_54 = (uint)local_8;
            piVar19 = local_dc;
            do {
              *(int *)(((int)local_e0 - (int)local_dc) + (int)piVar19) = in_ECX[9];
              *piVar19 = in_ECX[8];
              *(IMAGE_DOS_HEADER **)(local_7c + (int)piVar19) = local_74[1];
              *(dword *)(local_d0 + (int)piVar19) = local_80;
              piVar19 = piVar19 + 1;
              local_54 = (uint)(((IMAGE_DOS_HEADER *)(local_54 + -0x118))->e_program + 0xd7);
            } while ((IMAGE_DOS_HEADER *)local_54 != (IMAGE_DOS_HEADER *)0x0);
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10100000,
                                         (int)local_108[0],(int *)local_4c[0],(undefined4 *)0x0,0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_74[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20400000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],(int)local_108[1],
                                         (int *)local_4c[1],local_108[0],0);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],
                                              (int)local_108[2],(int *)local_4c[2],local_108[0],0),
              (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10300000,
                                             (int)local_108[3],local_108[1],(undefined4 *)0x0,0),
             (int)puVar15 < 0)) goto LAB_10070486;
          local_64 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20500000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)local_108[4],
                                         local_108[2],local_108[3],0);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20100000,
                                              (int)local_108[5],local_108[4],local_e0,4),
              (int)puVar15 < 0)) ||
             ((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x20000000,
                                              (int)local_108[6],local_108[5],local_dc,0x14),
              (int)puVar15 < 0 ||
              (((puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,
                                                (int)local_108[7],local_108[6],local_d8,8),
                (int)puVar15 < 0 ||
                (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],(int)local_e8
                                                ,local_108[7],local_d4,0), (int)puVar15 < 0)) ||
               (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)local_e4,
                                               local_108[6],local_108[6],0x14), (int)puVar15 < 0))))
             )) goto LAB_10070486;
          uVar24 = 0;
          pIVar21 = local_64;
          pIVar14 = local_e8;
          pIVar20 = local_e4;
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x40:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_20[3] = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(uint)local_8);
        if (local_20[3] != (IMAGE_DOS_HEADER *)0x0) {
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[3],(int)local_8);
          if (((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0))
          goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10700000,
                                         (int)local_20[3],(int *)local_4c[0],(undefined4 *)0x0,4);
          if ((int)puVar15 < 0) goto LAB_10070486;
          uVar24 = 4;
          pIVar21 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10300000);
          pIVar14 = local_20[3];
          pIVar20 = (IMAGE_DOS_HEADER *)0x0;
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x41:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0x17;
        pIVar21 = (IMAGE_DOS_HEADER *)(extraout_EDX_29 & 0xfffff | 0x20300000);
        pIVar14 = local_4c[1];
        pIVar20 = local_4c[0];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x42:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      pIVar20 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 3);
      if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          local_5c[uVar24] = pIVar20;
          uVar24 = uVar24 + 1;
          pIVar20 = (IMAGE_DOS_HEADER *)(pIVar20->e_magic + (int)pIVar14 * 4);
        } while (uVar24 < 3);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)local_5c[0],(int)local_8);
        if (((((int)puVar15 < 0) ||
             (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_5c[1],(int)local_8),
             (int)puVar15 < 0)) ||
            (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_54,(int)local_8), (int)puVar15 < 0))
           || ((puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8), (int)puVar15 < 0
               || (puVar15 = FUN_10065f1e(in_ECX,(uint)param_1,(uint *)local_4c[0],
                                          (undefined4 *)local_5c[0],(undefined4 *)local_5c[1],
                                          (uint)local_8), (int)puVar15 < 0)))) goto LAB_10070486;
        local_c = (IMAGE_DOS_HEADER *)((uint)local_8 & 0xfffff);
        puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10300000,local_54,
                                       (int *)local_5c[1],(undefined4 *)0x0,0);
        pIVar14 = local_5c[0];
        pIVar20 = (IMAGE_DOS_HEADER *)local_54;
        pIVar21 = local_c;
joined_r0x1006f799:
        local_c = pIVar21;
        if ((int)puVar15 < 0) goto LAB_10070486;
        goto LAB_1006f7a9;
      }
      goto LAB_1006f800;
    }
    break;
  case 0x43:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_54 = FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                0x3fe71547652b82fe);
        pIVar20 = local_8;
        pIVar14 = (IMAGE_DOS_HEADER *)FUN_100635ee(in_ECX,(int)local_8 * 9);
        if (pIVar14 != (IMAGE_DOS_HEADER *)0x0) {
          uVar24 = 0;
          do {
            local_4c[uVar24 + 4] = pIVar14;
            uVar24 = uVar24 + 1;
            pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14->e_magic + (int)pIVar20 * 4);
          } while (uVar24 < 9);
          pIVar14 = local_8;
          pIVar21 = local_20[1];
          if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
            for (; pIVar14 != (IMAGE_DOS_HEADER *)0x0;
                pIVar14 = (IMAGE_DOS_HEADER *)(pIVar14[-1].e_program + 0xd7)) {
              pIVar21->e_magic[0] = (undefined1)local_54;
              pIVar21->e_magic[1] = local_54._1_1_;
              pIVar21->e_cblp = local_54._2_2_;
              pIVar21 = (IMAGE_DOS_HEADER *)&pIVar21->e_cp;
            }
          }
          puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[4],(int)local_8);
          if ((((((int)puVar15 < 0) ||
                (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[5],extraout_EDX_30),
                (int)puVar15 < 0)) ||
               ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_4c[6],extraout_EDX_31),
                (int)puVar15 < 0 ||
                ((puVar15 = (uint *)FUN_1006370b((undefined4 *)local_30[0],extraout_EDX_32),
                 (int)puVar15 < 0 ||
                 (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_30[1],extraout_EDX_33),
                 (int)puVar15 < 0)))))) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_28,extraout_EDX_34),
              (int)puVar15 < 0)) ||
             (((puVar15 = (uint *)FUN_1006370b((undefined4 *)uStack_24,extraout_EDX_35),
               (int)puVar15 < 0 ||
               (puVar15 = (uint *)FUN_1006370b((undefined4 *)local_20[0],extraout_EDX_36),
               (int)puVar15 < 0)) ||
              (puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,extraout_EDX_37),
              (int)puVar15 < 0)))) goto LAB_10070486;
          local_c = (IMAGE_DOS_HEADER *)(extraout_EDX_38 & 0xfffff);
          local_64 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20500000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_64,(int)local_4c[4],
                                         (int *)local_4c[0],(undefined4 *)local_20[1],0);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_10 = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10500000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_10,(int)local_4c[5],
                                         (int *)local_4c[4],(undefined4 *)0x0,4);
          if ((int)puVar15 < 0) goto LAB_10070486;
          local_20[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x10100000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_4c[6],
                                         (int *)local_4c[4],(undefined4 *)0x0,0);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_10,(int)local_30[0],
                                              (int *)local_4c[6],(undefined4 *)0x0,4),
              (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_20[3],(int)local_30[1]
                                             ,(int *)local_30[0],(undefined4 *)0x0,8),
             (int)puVar15 < 0)) goto LAB_10070486;
          local_74[3] = (IMAGE_DOS_HEADER *)((uint)local_c | 0x20400000);
          puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],(int)local_28,
                                         (int *)local_4c[5],(undefined4 *)local_30[1],0);
          if ((((int)puVar15 < 0) ||
              (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_74[3],uStack_24,
                                              (int *)local_4c[5],(undefined4 *)local_30[0],4),
              (int)puVar15 < 0)) ||
             (puVar15 = (uint *)FUN_10065452(in_ECX,(uint)param_1,(uint)local_c | 0x10300000,
                                             (int)local_20[0],(int *)uStack_24,(undefined4 *)0x0,4),
             (int)puVar15 < 0)) goto LAB_10070486;
          uVar24 = 0;
          pIVar21 = local_64;
          pIVar14 = local_28;
          pIVar20 = local_20[0];
          goto LAB_1006aa88;
        }
        goto LAB_1006f800;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x44:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          local_20[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[1];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 2);
        local_54 = *(uint *)local_4c[1];
        FUN_100681c4(in_ECX,0,&local_54,1);
        iVar23 = in_ECX[2];
        iVar13 = *(int *)(*(int *)(iVar23 + 0x14) + (int)local_20[0] * 4);
        if (((*(byte *)(*(int *)(*(int *)(iVar23 + 0x10) + *(int *)(iVar13 + 4) * 4) + 4) & 0x10) !=
             0) && (*(int *)(iVar13 + 0x10) == 0)) {
          local_10 = (IMAGE_DOS_HEADER *)0x0;
          if (*(int *)(iVar23 + 8) != 0) {
            local_20[2] = *(IMAGE_DOS_HEADER **)(iVar23 + 0x14);
            do {
              iVar8 = *(int *)local_20[2];
              if ((((*(int *)(iVar13 + 4) == *(int *)(iVar8 + 4)) &&
                   (*(int *)(iVar13 + 8) == *(int *)(iVar8 + 8))) &&
                  (*(int *)(iVar13 + 0xc) == *(int *)(iVar8 + 0xc))) &&
                 (*(int *)(iVar8 + 0x10) == 1)) {
                local_20[1] = local_10;
                break;
              }
              local_10 = (IMAGE_DOS_HEADER *)(local_10->e_magic + 1);
              local_20[2] = (IMAGE_DOS_HEADER *)&local_20[2]->e_cp;
            } while (local_10 < *(IMAGE_DOS_HEADER **)(in_ECX[2] + 8));
          }
          if (local_10 == *(IMAGE_DOS_HEADER **)(iVar23 + 8)) {
            FUN_10064eb8(in_ECX,(int)param_1,0xdb5,
                         "tex1D will considered dependent since texcoord was not declared as at least float2"
                        );
          }
        }
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60000002;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = (IMAGE_DOS_HEADER *)local_20;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x45:
    if ((((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
         ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
        (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) && (local_20[1] != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          local_4c[uVar24 + 5] = *(IMAGE_DOS_HEADER **)local_4c[0];
          local_4c[uVar24 + 7] = *(IMAGE_DOS_HEADER **)local_4c[1];
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[2];
          uVar22 = uVar24 + 1;
          local_20[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[3];
          uVar24 = uVar22;
        } while (uVar22 < 2);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60100002;
        pIVar14 = (IMAGE_DOS_HEADER *)(local_4c + 5);
        pIVar20 = (IMAGE_DOS_HEADER *)(local_4c + 7);
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x46:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60200004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x47:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60300004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x48:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60400004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x49:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          local_5c[uVar24 + 1] = (IMAGE_DOS_HEADER *)*(undefined4 *)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 2);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60500002;
        pIVar14 = (IMAGE_DOS_HEADER *)(local_5c + 1);
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x4a:
    if ((((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
         ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
        (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) && (local_20[1] != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_74[1] = (IMAGE_DOS_HEADER *)((int)local_4c + (0x1c - (int)local_4c[1]));
        local_10 = (IMAGE_DOS_HEADER *)((int)local_4c[2] - (int)local_4c[1]);
        local_7c = (int)&local_28 - (int)local_4c[1];
        local_60 = (IMAGE_DOS_HEADER *)((int)local_4c[3] - (int)local_4c[1]);
        local_80 = (int)local_20 - (int)local_4c[1];
        local_54 = 2;
        pIVar20 = local_4c[1];
        do {
          *(undefined4 *)((int)local_4c + (0x14 - (int)local_4c[1]) + (int)pIVar20) =
               *(undefined4 *)local_4c[0];
          *(undefined4 *)(pIVar20->e_magic + (int)local_74[1]->e_magic) = *(undefined4 *)pIVar20;
          *(undefined4 *)(local_7c + (int)pIVar20) =
               *(undefined4 *)(pIVar20->e_magic + (int)local_10->e_magic);
          *(undefined4 *)(local_80 + (int)pIVar20) =
               *(undefined4 *)(pIVar20->e_magic + (int)local_60->e_magic);
          pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
          local_54 = (uint)(((IMAGE_DOS_HEADER *)(local_54 + -0x118))->e_program + 0xd7);
        } while ((IMAGE_DOS_HEADER *)local_54 != (IMAGE_DOS_HEADER *)0x0);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60600002;
        pIVar14 = (IMAGE_DOS_HEADER *)(local_4c + 5);
        pIVar20 = (IMAGE_DOS_HEADER *)(local_4c + 7);
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x4b:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60700004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x4c:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60800004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x4d:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60900004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x4e:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&uStack_24)[uVar24] = *(uint *)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 3);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60a00003;
        pIVar14 = (IMAGE_DOS_HEADER *)&uStack_24;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x4f:
    if ((((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
         ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
        (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) && (local_20[1] != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_74[1] = (IMAGE_DOS_HEADER *)((int)local_108 + (0x14 - (int)local_4c[1]));
        local_10 = (IMAGE_DOS_HEADER *)((int)local_4c[2] - (int)local_4c[1]);
        local_7c = (int)&local_e8 - (int)local_4c[1];
        local_60 = (IMAGE_DOS_HEADER *)((int)local_4c[3] - (int)local_4c[1]);
        iVar23 = 8 - (int)local_4c[1];
        local_80 = (int)&local_dc - (int)local_4c[1];
        local_54 = 3;
        pIVar20 = local_4c[1];
        do {
          pIVar14 = local_74[1];
          *(IMAGE_DOS_HEADER **)((int)local_108 + iVar23 + (int)pIVar20) =
               *(IMAGE_DOS_HEADER **)local_4c[0];
          *(undefined4 *)(pIVar20->e_magic + (int)pIVar14->e_magic) = *(undefined4 *)pIVar20;
          *(undefined4 *)(pIVar20->e_magic + local_7c) =
               *(undefined4 *)(local_10->e_magic + (int)pIVar20->e_magic);
          *(undefined4 *)(pIVar20->e_magic + local_80) =
               *(undefined4 *)(local_60->e_magic + (int)pIVar20->e_magic);
          pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
          local_54 = (uint)(((IMAGE_DOS_HEADER *)(local_54 + -0x118))->e_program + 0xd7);
        } while ((IMAGE_DOS_HEADER *)local_54 != (IMAGE_DOS_HEADER *)0x0);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60b00003;
        pIVar14 = (IMAGE_DOS_HEADER *)(local_108 + 2);
        pIVar20 = (IMAGE_DOS_HEADER *)(local_108 + 5);
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x50:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60c00004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x51:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60d00004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x52:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60e00004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x53:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&uStack_24)[uVar24] = *(uint *)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 3);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x60f00003;
        pIVar14 = (IMAGE_DOS_HEADER *)&uStack_24;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x54:
    if ((((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
         ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) &&
        (local_20[0] != (IMAGE_DOS_HEADER *)0x0)) && (local_20[1] != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_74[1] = (IMAGE_DOS_HEADER *)((int)local_108 + (0x14 - (int)local_4c[1]));
        local_10 = (IMAGE_DOS_HEADER *)((int)local_4c[2] - (int)local_4c[1]);
        local_7c = (int)&local_e8 - (int)local_4c[1];
        local_60 = (IMAGE_DOS_HEADER *)((int)local_4c[3] - (int)local_4c[1]);
        iVar23 = 8 - (int)local_4c[1];
        local_80 = (int)&local_dc - (int)local_4c[1];
        local_54 = 3;
        pIVar20 = local_4c[1];
        do {
          pIVar14 = local_74[1];
          *(IMAGE_DOS_HEADER **)((int)local_108 + iVar23 + (int)pIVar20) =
               *(IMAGE_DOS_HEADER **)local_4c[0];
          *(undefined4 *)(pIVar20->e_magic + (int)pIVar14->e_magic) = *(undefined4 *)pIVar20;
          *(undefined4 *)(pIVar20->e_magic + local_7c) =
               *(undefined4 *)(local_10->e_magic + (int)pIVar20->e_magic);
          *(undefined4 *)(pIVar20->e_magic + local_80) =
               *(undefined4 *)(local_60->e_magic + (int)pIVar20->e_magic);
          pIVar20 = (IMAGE_DOS_HEADER *)&pIVar20->e_cp;
          local_54 = (uint)(((IMAGE_DOS_HEADER *)(local_54 + -0x118))->e_program + 0xd7);
        } while ((IMAGE_DOS_HEADER *)local_54 != (IMAGE_DOS_HEADER *)0x0);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x61000003;
        pIVar14 = (IMAGE_DOS_HEADER *)(local_108 + 2);
        pIVar20 = (IMAGE_DOS_HEADER *)(local_108 + 5);
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x55:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x61100004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x56:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x61200004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x57:
    if ((local_28 != (IMAGE_DOS_HEADER *)0x0) &&
       ((IMAGE_DOS_HEADER *)uStack_24 != (IMAGE_DOS_HEADER *)0x0)) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        uVar24 = 0;
        do {
          (&local_28)[uVar24] = *(IMAGE_DOS_HEADER **)local_4c[0];
          uVar24 = uVar24 + 1;
        } while (uVar24 < 4);
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,(int)local_8);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 0;
        pIVar21 = (IMAGE_DOS_HEADER *)0x61300004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_28;
        pIVar20 = local_4c[1];
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x58:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if ((param_2 != (IMAGE_DOS_HEADER *)0x0) &&
         (iVar23._0_2_ = local_28->e_lfarlc, iVar23._2_2_ = local_28->e_ovno,
         local_60 = (IMAGE_DOS_HEADER *)0x0, iVar23 != 0)) {
        pIVar20 = *(IMAGE_DOS_HEADER **)&local_28->e_ip;
        do {
          local_10 = (IMAGE_DOS_HEADER *)0x0;
          if (pIVar20 != (IMAGE_DOS_HEADER *)0x0) {
            do {
              iVar23 = *(int *)&local_28->e_lfarlc * (int)local_10;
              pcVar17 = local_10->e_magic;
              pcVar10 = local_10->e_magic;
              local_10 = (IMAGE_DOS_HEADER *)(pcVar10 + 1);
              *(undefined4 *)
               (param_2->e_magic +
               (int)(pcVar17 + *(int *)((int)param_1 + 0x18) * (int)local_60) * 4) =
                   *(undefined4 *)(local_4c[0]->e_magic + (int)(local_60->e_magic + iVar23) * 4);
              pIVar20 = *(IMAGE_DOS_HEADER **)&local_28->e_ip;
            } while ((IMAGE_DOS_HEADER *)(pcVar10 + 1) < pIVar20);
          }
          pcVar17 = local_60->e_magic;
          local_60 = (IMAGE_DOS_HEADER *)(pcVar17 + 1);
        } while ((IMAGE_DOS_HEADER *)(pcVar17 + 1) < *(IMAGE_DOS_HEADER **)&local_28->e_lfarlc);
      }
      goto LAB_1006aa9c;
    }
    break;
  case 0x59:
    if (local_28 != (IMAGE_DOS_HEADER *)0x0) {
      if (param_2 != (IMAGE_DOS_HEADER *)0x0) {
        local_28 = (IMAGE_DOS_HEADER *)
                   FUN_1007c203((void *)in_ECX[2],*(undefined4 *)(in_ECX[2] + 0x78),0,0,
                                0x406fe01000000000);
        local_c0 = *(IMAGE_DOS_HEADER **)&local_4c[0]->e_cparhdr;
        local_bc = *(uint **)&local_4c[0]->e_cp;
        local_b8 = *(IMAGE_DOS_HEADER **)local_4c[0];
        local_b4._0_2_ = local_4c[0]->e_maxalloc;
        local_b4._2_2_ = local_4c[0]->e_ss;
        uStack_24 = (uint)local_28;
        local_20[0] = local_28;
        local_20[1] = local_28;
        puVar15 = (uint *)FUN_1006370b((undefined4 *)param_2,4);
        if ((int)puVar15 < 0) goto LAB_10070486;
        uVar24 = 6;
        pIVar21 = (IMAGE_DOS_HEADER *)0x20500004;
        pIVar14 = (IMAGE_DOS_HEADER *)&local_c0;
        pIVar20 = (IMAGE_DOS_HEADER *)&local_28;
        goto LAB_1006aa88;
      }
      goto LAB_1006aa9c;
    }
    break;
  default:
    FUN_10064e46(in_ECX,(int)param_1,0xdac,"intrinsic function \'%s\' is not yet implemented");
  }
  puVar15 = (uint *)0x80004005;
LAB_10070486:
  uVar24 = 0;
  in_ECX[7] = local_160;
  in_ECX[0x19] = local_164;
  do {
    FUN_10003870(local_4c[uVar24]);
    uVar24 = uVar24 + 1;
  } while (uVar24 < 4);
  return puVar15;
}

