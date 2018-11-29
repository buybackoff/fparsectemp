using System;
using Microsoft.FSharp.Core;
using System.Runtime.CompilerServices;

namespace FParsec
{
    //public interface IParser<TResult, TUserState>
    //{
    //    Reply<TResult> Parse(CharStream<TUserState> charStream);
    //}

    public abstract class ParserX<TResult, TUserState>
    {
        protected abstract Reply<TResult> InvokeImpl(CharStream<TUserState> charStream);

        protected IntPtr _parseMethodPtr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TResult> Invoke(CharStream<TUserState> charStream)
        {
            if (_parseMethodPtr != default)
            {
                return CalliHelper.InvokeFast<Reply<TResult>, CharStream<TUserState>>(this, charStream, _parseMethodPtr);
               
            }

            return InvokeImpl(charStream);
        }
    }

    internal sealed class SpacesParser<TUserState> : ParserX<Unit, TUserState> //, IParser<Unit, TUserState>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpacesParser()
        {
            _parseMethodPtr = CalliHelper.LdvirtftnSpaces(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Reply<Unit> InvokeImpl(CharStream<TUserState> stream)
        {
            // Console.WriteLine("SpacesParser impl: " + _parseMethodPtr);
            return Parse(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<Unit> Parse(CharStream<TUserState> stream)
        {
            stream.SkipWhitespace();
            return new Reply<Unit>(default(Unit));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Reply<T> Parse<T>(CharStream<TUserState> stream)
        {
            stream.SkipWhitespace();
            return new Reply<T>(default);
        }
    }

    internal class Test
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr TestMe<TLeft, TRight, TUserState>(TakeLeftParser<TLeft, TRight, TUserState> parser, Unit u)
        {
            CharStream<TUserState> str = default;
            var x = parser.Parse(str);
            return IntPtr.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Reply<TResult> InvokeFast<TResult, TUserState>(object parser, CharStream<TUserState> stream,
            IntPtr fnptr)
        {
            var str = parser.ToString();
            return default;
        }
    }

    internal sealed class TakeLeftParser<TLeft, TRight, TUserState> : ParserX<TLeft, TUserState> //, IParser<TLeft, TUserState>
    {
        private readonly ParserX<TLeft, TUserState> _l;
        private readonly ParserX<TRight, TUserState> _r;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TakeLeftParser(ParserX<TLeft, TUserState> l, ParserX<TRight, TUserState> r)
        {
            _l = l;
            _r = r;
            _parseMethodPtr = CalliHelper.LdvirtftnTakeLeft(this);
            // Console.WriteLine(_parseMethodPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Reply<TLeft> InvokeImpl(CharStream<TUserState> stream)
        {
            // Console.WriteLine("TakeLeftParser impl");
            return Parse(stream);

            //if (_parseMethodPtr == default)
            //{
            //    return Parse(stream);
            //}

            //// Console.WriteLine("Calli");
            //return CalliHelper.InvokeFast<Reply<TLeft>, CharStream<TUserState>>(this, stream, _parseMethodPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TLeft> Parse(CharStream<TUserState> stream)
        {
            var reply1 = _l.Invoke(stream);

            if (reply1.Status == ReplyStatus.Ok)
            {
                var stateTag1 = stream.StateTag;
                Reply<TRight> reply2;
                //if (typeof(TRight) == typeof(Unit))
                //{
                //    if (_r is SpacesParser<TUserState> sp)
                //    {
                //        reply2 = sp.Parse<TRight>(stream);
                //    }
                //    else
                //    {
                //        reply2 = _r.Invoke(stream);
                //    }
                //}
                //else
                //{
                //    reply2 = _r.Invoke(stream);
                //}

                reply2 = _r.Invoke(stream);

                var error = reply1.Error == null ? reply2.Error
                    : stateTag1 != stream.StateTag ? reply2.Error
                    : ErrorMessageList.Merge(reply2.Error, reply1.Error);
                reply1.Error = error;
                reply1.Status = reply2.Status;
            }

            return reply1;
        }
    }

    internal sealed class TakeRightParser<TLeft, TRight, TUserState> : ParserX<TRight, TUserState> //, IParser<TRight, TUserState>
    {
        private readonly ParserX<TLeft, TUserState> _l;
        private readonly ParserX<TRight, TUserState> _r;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TakeRightParser(ParserX<TLeft, TUserState> l, ParserX<TRight, TUserState> r)
        {
            _l = l;
            _r = r;
            _parseMethodPtr = CalliHelper.LdvirtftnTakeRight(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Reply<TRight> InvokeImpl(CharStream<TUserState> stream)
        {
            // Console.WriteLine("TakeRightParser impl");
            return Parse(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TRight> Parse(CharStream<TUserState> stream)
        {
            Reply<TLeft> reply1;
            //if (typeof(TLeft) == typeof(Unit))
            //{
            //    if (_l is SpacesParser<TUserState> sp)
            //    {
            //        reply1 = sp.Parse<TLeft>(stream);
            //    }
            //    else
            //    {
            //        reply1 = _l.Invoke(stream);
            //    }
            //}
            //else
            {
                reply1 = _l.Invoke(stream);
            }

            if (reply1.Status == ReplyStatus.Ok)
            {
                if (reply1.Error == null)
                {
                    return _r.Invoke(stream);
                }

                var stateTag1 = stream.StateTag;
                var reply2 = _r.Invoke(stream);
                if (stateTag1 == stream.StateTag)
                {
                    reply2.Error = ErrorMessageList.Merge(reply2.Error, reply1.Error);
                }
                return reply2;
            }

            return new Reply<TRight>(reply1.Status, reply1.Error);
        }
    }
}
