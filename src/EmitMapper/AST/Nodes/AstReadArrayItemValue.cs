﻿using EmitMapper.AST.Helpers;
using EmitMapper.AST.Interfaces;

namespace EmitMapper.AST.Nodes;

internal class AstReadArrayItemValue : AstReadArrayItem, IAstValue
{
  public override void Compile(CompilationContext context)
  {
    CompilationHelper.CheckIsValue(ItemType);
    base.Compile(context);
  }
}