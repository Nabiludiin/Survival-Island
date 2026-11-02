// mode=sebagian: isi switch di 100d6d18 tidak ikut

uint FUN_100d6c40(int param_1,int param_2)

{
  int iVar1;
  int iVar2;
  int in_ECX;
  int iVar3;
  int *piVar4;
  uint uVar5;
  byte local_299;
  undefined4 local_298;
  int local_294;
  int local_290;
  undefined1 local_28c [36];
  int local_268 [77];
  int local_134;
  undefined1 local_130 [304];
  
  local_294 = 0;
  local_299 = 7;
  FUN_1004b3b0();
  local_290 = FUN_10116110(1);
  if (0 < *(int *)(in_ECX + 0x5ec)) {
    do {
      if ((param_1 + -0x20 <= **(int **)(in_ECX + 0x5e0)) ||
         (local_290 <= (*(int **)(in_ECX + 0x5e0))[0x4d])) break;
      iVar2 = *(int *)(in_ECX + 0x5ec) + -1;
      if (0 < iVar2) {
        FUN_10245bb0(*(int *)(in_ECX + 0x5e0),*(int *)(in_ECX + 0x5e0) + 0x138,iVar2 * 0x138);
      }
      *(int *)(in_ECX + 0x5ec) = *(int *)(in_ECX + 0x5ec) + -1;
    } while (0 < *(int *)(in_ECX + 0x5ec));
  }
  if (*(char *)(in_ECX + 0x726) != '\0') {
    return 0;
  }
  do {
    uVar5 = 0;
    local_298 = 0;
    FUN_10116630(&local_299,&local_294,&local_298);
    if (local_299 - 3 < 7) {
      return local_299 - 3;
    }
    iVar2 = FUN_10116110(1);
    FUN_101160f0(local_130);
    FUN_10116500(local_28c,local_28c);
    FUN_10116740(0,0);
    FUN_1004b3b0();
    local_268[0] = 0;
    do {
      FUN_100d3d50(local_130 + uVar5);
      uVar5 = uVar5 + 0x4c;
    } while (uVar5 < 0x130);
    iVar1 = *(int *)(in_ECX + 0x5ec);
    iVar3 = 0;
    local_268[0] = local_294;
    local_134 = iVar2;
    if (0 < iVar1) {
      piVar4 = (int *)(*(int *)(in_ECX + 0x5e0) + 0x134);
      do {
        if (*piVar4 == iVar2) {
          if (iVar3 < iVar1) goto LAB_100d6e78;
          break;
        }
        iVar3 = iVar3 + 1;
        piVar4 = piVar4 + 0x4e;
      } while (iVar3 < iVar1);
      if ((0 < iVar1) && (local_294 < *(int *)(iVar1 * 0x138 + -0x138 + *(int *)(in_ECX + 0x5e0))))
      {
        *(undefined4 *)(in_ECX + 0x5ec) = 0;
      }
    }
    FUN_100d6950(*(undefined4 *)(in_ECX + 0x5ec),local_268);
LAB_100d6e78:
    if (param_2 < local_294 - param_1) {
      FUN_10116150(local_290,1);
      return 1;
    }
  } while( true );
}

// assembly lengkap, termasuk isi switch:
// 100d6c40  SUB ESP,0x29c
// 100d6c46  PUSH EBX
// 100d6c47  PUSH EBP
// 100d6c48  PUSH ESI
// 100d6c49  PUSH EDI
// 100d6c4a  MOV EBX,ECX
// 100d6c4c  XOR EDI,EDI
// 100d6c4e  LEA ECX,[ESP + 0x17c]
// 100d6c55  MOV dword ptr [ESP + 0x18],EDI
// 100d6c59  MOV byte ptr [ESP + 0x13],0x7
// 100d6c5e  CALL 0x1004b3b0
// 100d6c63  LEA ESI,[EBX + 0x4]
// 100d6c66  PUSH 0x1
// 100d6c68  MOV ECX,ESI
// 100d6c6a  CALL 0x10116110
// 100d6c6f  CMP dword ptr [EBX + 0x5ec],EDI
// 100d6c75  MOV dword ptr [ESP + 0x1c],EAX
// 100d6c79  JLE 0x100d6cd4
// 100d6c7b  MOV EBP,dword ptr [ESP + 0x2b0]
// 100d6c82  ADD EBP,-0x20
// 100d6c85  MOV EAX,dword ptr [EBX + 0x5e0]
// 100d6c8b  CMP dword ptr [EAX],EBP
// 100d6c8d  JGE 0x100d6cd4
// 100d6c8f  MOV ECX,dword ptr [ESP + 0x1c]
// 100d6c93  CMP dword ptr [EAX + 0x134],ECX
// 100d6c99  JGE 0x100d6cd4
// 100d6c9b  MOV EAX,dword ptr [EBX + 0x5ec]
// 100d6ca1  SUB EAX,0x1
// 100d6ca4  CMP EAX,EDI
// 100d6ca6  JLE 0x100d6cc5
// 100d6ca8  MOV ECX,dword ptr [EBX + 0x5e0]
// 100d6cae  IMUL EAX,EAX,0x138
// 100d6cb4  PUSH EAX
// 100d6cb5  LEA EDX,[ECX + 0x138]
// 100d6cbb  PUSH EDX
// 100d6cbc  PUSH ECX
// 100d6cbd  CALL 0x10245bb0
// 100d6cc2  ADD ESP,0xc
// 100d6cc5  ADD dword ptr [EBX + 0x5ec],-0x1
// 100d6ccc  CMP dword ptr [EBX + 0x5ec],EDI
// 100d6cd2  JG 0x100d6c85
// 100d6cd4  CMP byte ptr [EBX + 0x726],0x0
// 100d6cdb  JZ 0x100d6cf1
// 100d6cdd  POP EDI
// 100d6cde  POP ESI
// 100d6cdf  POP EBP
// 100d6ce0  XOR AL,AL
// 100d6ce2  POP EBX
// 100d6ce3  ADD ESP,0x29c
// 100d6ce9  RET 0x8
// 100d6cec  CALL 0x100c7ae0
// 100d6cf1  LEA EAX,[ESP + 0x14]
// 100d6cf5  PUSH EAX
// 100d6cf6  LEA ECX,[ESP + 0x1c]
// 100d6cfa  PUSH ECX
// 100d6cfb  LEA EDX,[ESP + 0x1b]
// 100d6cff  PUSH EDX
// 100d6d00  MOV ECX,ESI
// 100d6d02  MOV dword ptr [ESP + 0x20],EDI
// 100d6d06  CALL 0x10116630
// 100d6d0b  MOVZX EAX,byte ptr [ESP + 0x13]
// 100d6d10  ADD EAX,-0x3
// 100d6d13  CMP EAX,0x6
// 100d6d16  JA 0x100d6d97
// 100d6d18  JMP dword ptr [EAX*0x4 + 0x100d6ecc]
// 100d6d1f  MOV EAX,dword ptr [ESP + 0x14]
// 100d6d23  PUSH EAX
// 100d6d24  PUSH 0x411
// 100d6d29  PUSH 0x102fe8a0
// 100d6d2e  LEA ECX,[ESP + 0x40]
// 100d6d32  CALL 0x100c7a90
// 100d6d37  MOV ECX,ESI
// 100d6d39  CALL 0x101168c0
// 100d6d3e  LEA ECX,[ESP + 0x34]
// 100d6d42  JMP 0x100d6cec
// 100d6d44  PUSH EDI
// 100d6d45  MOV ECX,ESI
// 100d6d47  CALL 0x10116980
// 100d6d4c  JMP 0x100d6cf1
// 100d6d4e  MOV ECX,dword ptr [ESP + 0x14]
// 100d6d52  PUSH ECX
// 100d6d53  PUSH 0x41c
// 100d6d58  PUSH 0x102fe8a0
// 100d6d5d  LEA ECX,[ESP + 0x30]
// 100d6d61  CALL 0x100c7a90
// 100d6d66  LEA EDX,[ESP + 0x20]
// 100d6d6a  PUSH EDX
// 100d6d6b  PUSH EDI
// 100d6d6c  MOV ECX,ESI
// 100d6d6e  CALL 0x10116a90
// 100d6d73  LEA ECX,[ESP + 0x24]
// 100d6d77  JMP 0x100d6cec
// 100d6d7c  PUSH EDI
// 100d6d7d  PUSH EDI
// 100d6d7e  MOV ECX,ESI
// 100d6d80  CALL 0x10116af0
// 100d6d85  JMP 0x100d6cf1
// 100d6d8a  PUSH EDI
// 100d6d8b  MOV ECX,ESI
// 100d6d8d  CALL 0x10116980
// 100d6d92  JMP 0x100d6cf1
// 100d6d97  PUSH 0x1
// 100d6d99  MOV ECX,ESI
// 100d6d9b  CALL 0x10116110
// 100d6da0  MOV EBP,EAX
// 100d6da2  LEA EAX,[ESP + 0x17c]
// 100d6da9  PUSH EAX
// 100d6daa  MOV ECX,ESI
// 100d6dac  CALL 0x101160f0
// 100d6db1  LEA ECX,[ESP + 0x20]
// 100d6db5  PUSH ECX
// 100d6db6  MOV EDX,ECX
// 100d6db8  PUSH EDX
// 100d6db9  MOV ECX,ESI
// 100d6dbb  CALL 0x10116500
// 100d6dc0  PUSH EDI
// 100d6dc1  PUSH EDI
// 100d6dc2  MOV ECX,ESI
// 100d6dc4  CALL 0x10116740
// 100d6dc9  LEA ECX,[ESP + 0x48]
// 100d6dcd  CALL 0x1004b3b0
// 100d6dd2  MOV dword ptr [ESP + 0x44],EDI
// 100d6dd6  JMP 0x100d6de0
// 100d6de0  LEA EAX,[ESP + EDI*0x1 + 0x17c]
// 100d6de7  PUSH EAX
// 100d6de8  LEA ECX,[ESP + EDI*0x1 + 0x4c]
// 100d6dec  CALL 0x100d3d50
// 100d6df1  ADD EDI,0x4c
// 100d6df4  CMP EDI,0x130
// 100d6dfa  JC 0x100d6de0
// 100d6dfc  MOV EAX,dword ptr [EBX + 0x5ec]
// 100d6e02  MOV EDI,dword ptr [ESP + 0x18]
// 100d6e06  XOR ECX,ECX
// 100d6e08  TEST EAX,EAX
// 100d6e0a  MOV dword ptr [ESP + 0x44],EDI
// 100d6e0e  MOV dword ptr [ESP + 0x178],EBP
// 100d6e15  JLE 0x100d6e5d
// 100d6e17  MOV EDX,dword ptr [EBX + 0x5e0]
// 100d6e1d  ADD EDX,0x134
// 100d6e23  CMP dword ptr [EDX],EBP
// 100d6e25  JZ 0x100d6e36
// 100d6e27  ADD ECX,0x1
// 100d6e2a  ADD EDX,0x138
// 100d6e30  CMP ECX,EAX
// 100d6e32  JL 0x100d6e23
// 100d6e34  JMP 0x100d6e3a
// 100d6e36  CMP ECX,EAX
// 100d6e38  JL 0x100d6e78
// 100d6e3a  TEST EAX,EAX
// 100d6e3c  JLE 0x100d6e5d
// 100d6e3e  MOV ECX,dword ptr [EBX + 0x5e0]
// 100d6e44  IMUL EAX,EAX,0x138
// 100d6e4a  CMP dword ptr [EAX + ECX*0x1 + 0xfffffec8],EDI
// 100d6e51  JLE 0x100d6e5d
// 100d6e53  MOV dword ptr [EBX + 0x5ec],0x0
// 100d6e5d  MOV EAX,dword ptr [EBX + 0x5ec]
// 100d6e63  LEA ECX,[EBX + 0x5e0]
// 100d6e69  LEA EDX,[ESP + 0x44]
// 100d6e6d  PUSH EDX
// 100d6e6e  PUSH EAX
// 100d6e6f  CALL 0x100d6950
// 100d6e74  MOV EDI,dword ptr [ESP + 0x18]
// 100d6e78  SUB EDI,dword ptr [ESP + 0x2b0]
// 100d6e7f  CMP EDI,dword ptr [ESP + 0x2b4]
// 100d6e86  JG 0x100d6eac
// 100d6e88  XOR EDI,EDI
// 100d6e8a  JMP 0x100d6cf1
// 100d6e8f  MOV ECX,dword ptr [ESP + 0x1c]
// 100d6e93  PUSH 0x1
// 100d6e95  PUSH ECX
// 100d6e96  MOV ECX,ESI
// 100d6e98  CALL 0x10116150
// 100d6e9d  POP EDI
// 100d6e9e  POP ESI
// 100d6e9f  POP EBP
// 100d6ea0  XOR AL,AL
// 100d6ea2  POP EBX
// 100d6ea3  ADD ESP,0x29c
// 100d6ea9  RET 0x8
// 100d6eac  MOV EDX,dword ptr [ESP + 0x1c]
// 100d6eb0  PUSH 0x1
// 100d6eb2  PUSH EDX
// 100d6eb3  MOV ECX,ESI
// 100d6eb5  CALL 0x10116150
// 100d6eba  POP EDI
// 100d6ebb  POP ESI
// 100d6ebc  POP EBP
// 100d6ebd  MOV AL,0x1
// 100d6ebf  POP EBX
// 100d6ec0  ADD ESP,0x29c
// 100d6ec6  RET 0x8
