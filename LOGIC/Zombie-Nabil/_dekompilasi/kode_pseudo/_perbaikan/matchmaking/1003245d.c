// mode=decompile dengan parameter stack manual

/* WARNING: Globals starting with '_' overlap smaller symbols at the same address */

undefined1  [10] __cdecl FUN_1003245d(byte stack_args [20])

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
    auStack_e._6_4_ = 0x10032471;
    in_EAX = FUN_10032df5();
    uVar1 = extraout_ECX;
  }
  bVar3 = (uVar1 & 0x7ff00000) == 0x7ff00000;
  if (bVar3) {
    if ((in_EAX & 0xfffff) == 0 && stack_args._8_4_ == 0) {
      auStack_e._6_4_ = 0x1003253a;
      __fload_withFB(0,(int)stack_args);
      iVar2 = extraout_ECX_01;
      fVar5 = extraout_ST0_01;
      fVar6 = extraout_ST1_01;
      goto LAB_1003253a;
    }
    auStack_e._6_4_ = 0x100324fd;
    __fload_withFB(0,(int)stack_args);
    fVar5 = extraout_ST0_00;
    fVar6 = extraout_ST1_00;
    if ((stack_args[0xe] & 8) == 0) {
      iVar2 = extraout_ECX_00 + 1;
      goto LAB_1003253a;
    }
LAB_10032507:
    auVar4 = (undefined1  [10])(fVar5 + fVar6);
  }
  else {
    auStack_e._6_4_ = 0x1003248c;
    uVar1 = __fload_withFB(uVar1 & 0x7ff00000,(int)stack_args);
    if (!bVar3) {
      if (((uVar1 & 0x7ff00000) != 0) ||
         ((CONCAT12(stack_args[6],stack_args._4_2_) & 0xfffff) != 0 || stack_args._0_4_ != 0)) {
        if ((stack_args[7] & 0x80) != 0) {
          auStack_e._6_4_ = 0x10032604;
          FUN_10032622();
          auVar4 = _DAT_10050890;
          if (extraout_CL_01 == '\0') goto LAB_100324d5;
        }
        auStack_e._6_4_ = 0x100324b1;
        auVar4 = FUN_10032de0();
        if (extraout_CL == '\x01') {
          auVar4 = (undefined1  [10])-(float10)auVar4;
        }
        if (DAT_10055dd8 != 0) {
          return auVar4;
        }
        auVar4 = (undefined1  [10])
                 FUN_10032ec9(&DAT_10050888,0x1d,unaff_retaddr,stack_args._0_4_,
                              CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)),
                              stack_args._8_4_,
                              CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)))
        ;
        return auVar4;
      }
      if ((CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)) & 0x7fffffff) == 0
          && stack_args._8_4_ == 0) {
        auVar4 = (undefined1  [10])FUN_10032e7e();
        return auVar4;
      }
      auStack_e._6_4_ = 0x100325bc;
      FUN_10032622();
      if ((stack_args._15_4_ & 0x80) == 0) {
        if ((stack_args[7] >> 7 & extraout_CL_00) == 0) {
          return (undefined1  [10])(float10)0;
        }
        auVar4 = (undefined1  [10])FUN_10032e7e();
        return auVar4;
      }
      auVar4 = _DAT_10050a30;
      if ((stack_args[7] >> 7 & extraout_CL_00) != 0) {
        auVar4 = (undefined1  [10])-(float10)_DAT_10050a30;
      }
      goto LAB_100324d5;
    }
    iVar2 = 0;
    fVar5 = extraout_ST0;
    fVar6 = extraout_ST1;
LAB_1003253a:
    if (((CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)) & 0x7ff00000) ==
         0x7ff00000) &&
       ((CONCAT12(stack_args[6],stack_args._4_2_) & 0xfffff) != 0 || stack_args._0_4_ != 0)) {
      if ((stack_args[6] & 8) != 0) goto LAB_10032507;
    }
    else if (iVar2 == 0) {
      pdVar7 = local_78;
      uStack_64 = 0x10032568;
      auStack_e = (undefined1  [10])in_ST7;
      iVar2 = FUN_1003565b(SUB84((double)fVar5,0),(int)((ulonglong)(double)fVar5 >> 0x20),
                           SUB84((double)fVar6,0),(int)((ulonglong)(double)fVar6 >> 0x20),local_78);
      auVar4 = (undefined1  [10])(float10)*pdVar7;
      if (iVar2 == 0) {
        return (undefined1  [10])(float10)*pdVar7;
      }
      goto LAB_100324d5;
    }
    auVar4 = (undefined1  [10])(fVar5 + fVar6);
  }
LAB_100324d5:
  if (DAT_10055dd8 != 0) {
    return auVar4;
  }
  auStack_e._6_4_ = 0x100324f2;
  auVar4 = (undefined1  [10])
           FUN_10032d80(&DAT_10050888,0x1d,in_FPUControlWord,unaff_retaddr,stack_args._0_4_,
                        CONCAT13(stack_args[7],CONCAT12(stack_args[6],stack_args._4_2_)),
                        stack_args._8_4_,
                        CONCAT13(stack_args[0xf],CONCAT12(stack_args[0xe],stack_args._12_2_)));
  return auVar4;
}

