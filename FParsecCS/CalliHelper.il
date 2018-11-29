.class  FParsec.CalliHelper
{
	.method public hidebysig static int32 Square(int32 number) cil managed
	{
		.maxstack 2
		ldarg.1
		dup
		mul
		ret
	}

	.method public hidebysig static 
		native int Ldvirtftn<TResult, TUserState, (class FParsec.IParser`2<!!TResult, !!TUserState>) TParser> () cil managed aggressiveinlining
	{
		.maxstack 1
		ldftn	valuetype FParsec.Reply`1<!0> class FParsec.Parser`3<!!TResult, !!TUserState, !!TParser>::Parse(class FParsec.Parser`3<!0, !1, !2>, class FParsec.CharStream`1<!1>)
		ret
	}

	.method public hidebysig static 
	native int LdvirtftnTakeLeft<TLeft, TRight, TUserState> () cil managed aggressiveinlining
	{
		.maxstack 1
		ldftn	valuetype FParsec.Reply`1<!0> class FParsec.TakeLeftParser`3<!!TLeft, !!TRight, !!TUserState>::Parse(object, class FParsec.CharStream`1<!2>)
		ret
	}

	.method public hidebysig static 
	native int LdvirtftnTakeRight<TLeft, TRight, TUserState> () cil managed aggressiveinlining
	{
		.maxstack 1
		ldftn	valuetype FParsec.Reply`1<!1> class FParsec.TakeRightParser`3<!!TLeft, !!TRight, !!TUserState>::Parse(object, class FParsec.CharStream`1<!2>)
		ret
	}

	.method public hidebysig static 
	native int LdvirtftnOPP<TTerm, TAfterString, TUserState> () cil managed aggressiveinlining
	{
		.maxstack 1
		ldftn	valuetype FParsec.Reply`1<!0> class FParsec.OperatorPrecedenceParser`3<!!TTerm, !!TAfterString, !!TUserState>::Parse(object, class FParsec.CharStream`1<!2>)
		ret
	}

	.method public hidebysig static 
	native int LdvirtftnSpaces<TUserState> () cil managed aggressiveinlining
	{
		.maxstack 1
		ldftn	valuetype FParsec.Reply`1<class [FSharp.Core]Microsoft.FSharp.Core.Unit> class FParsec.SpacesParser`1<!!TUserState>::Parse(object, class FParsec.CharStream`1<!0>)
		ret
	}

	.method public hidebysig static 
	!!TResult InvokeFast<TResult, TStream> (
		object parser,
		!!TStream charStream,
		native int fnptr
	) cil managed aggressiveinlining
	{
		.maxstack 8
		ldarg.0
		ldarg.1
		ldarg.2
		calli default !!TResult(object, !!TStream) 
		ret
	}
}
