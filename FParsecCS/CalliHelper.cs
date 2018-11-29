using System;
using System.Runtime.CompilerServices;

namespace FParsec
{
    static class CalliHelper
    {
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern int Square(int number);

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern IntPtr Ldvirtftn<TResult, TUserState, TParser>() 
            where TParser : IParser<TResult, TUserState>;

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern IntPtr LdvirtftnTakeLeft<TLeft, TRight, TUserState>();

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern IntPtr LdvirtftnTakeRight<TLeft, TRight, TUserState>();

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern IntPtr LdvirtftnOPP<TTerm, TAfterString, TUserState>();

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern IntPtr LdvirtftnSpaces<TUserState>();

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern TResult InvokeFast<TResult, TStream>(object parser, TStream stream, IntPtr fnptr);
    }
}
