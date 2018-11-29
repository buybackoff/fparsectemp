using System;
using System.Runtime.CompilerServices;

namespace FParsec
{
    static class CalliHelper
    {
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern int Square(int number);

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern IntPtr Ldvirtftn<TResult, TUserState, TParser>(TParser parser);

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern IntPtr LdvirtftnTakeLeft<TLeft, TRight, TUserState>(TakeLeftParser<TLeft, TRight, TUserState> parser);

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern IntPtr LdvirtftnTakeRight<TLeft, TRight, TUserState>(TakeRightParser<TLeft, TRight, TUserState> parser);

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern IntPtr LdvirtftnSpaces<TUserState>(SpacesParser<TUserState> parser);

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern TResult InvokeFast<TResult, TStream>(object parser, TStream stream, IntPtr fnptr);
    }
}
