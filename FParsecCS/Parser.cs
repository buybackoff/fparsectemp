using Microsoft.FSharp.Core;
using System;
using System.Runtime.CompilerServices;

namespace FParsec
{
    public interface IParser<TResult, TUserState>
    {
        Reply<TResult> Parse(CharStream<TUserState> charStream);
    }

    public abstract class Parser<TResult, TUserState>
    {
        protected abstract Reply<TResult> InvokeImpl(CharStream<TUserState> charStream);

        protected IntPtr _parseMethodPtr;
        internal bool HasParseMethodPtr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TResult> Invoke(CharStream<TUserState> charStream)
        {
            //if (HasParseMethodPtr)
            //{
            //    return CalliHelper.InvokeFast<Reply<TResult>, CharStream<TUserState>>(this, charStream, _parseMethodPtr);
            //}

            return InvokeImpl(charStream);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //internal Reply<TResult> InvokeFast(CharStream<TUserState> charStream)
        //{
        //    return CalliHelper.InvokeFast<Reply<TResult>, CharStream<TUserState>>(this, charStream, _parseMethodPtr);
        //}

    }

    public sealed class Parser<TResult, TUserState, TImpl> : Parser<TResult, TUserState>
        where TImpl : IParser<TResult, TUserState>
    {
        private readonly TImpl _impl;

        public Parser(TImpl impl)
        {
            _impl = impl;
            _parseMethodPtr = CalliHelper.Ldvirtftn<TResult, TUserState, TImpl>();
            HasParseMethodPtr = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Reply<TResult> Parse(Parser<TResult, TUserState, TImpl> th, CharStream<TUserState> stream)
        {
            return th.Parse(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TResult> Parse(CharStream<TUserState> stream)
        {
            return _impl.Parse(stream);
        }

        protected override Reply<TResult> InvokeImpl(CharStream<TUserState> charStream)
        {
            return _impl.Parse(charStream);
        }

        
    }

    internal class Test
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr TestMe<TLeft, TRight, TUserState>(TakeLeftParser<TLeft, TRight, TUserState> parser, Unit u)
        {
            CharStream<TUserState> str = default;
            var x = parser.Parse(str);

            var y = TakeLeftParser<TLeft, TRight, TUserState>.Parse(parser, str);
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

    internal sealed class TakeLeftParser<TLeft, TRight, TUserState>
        : Parser<TLeft, TUserState>, IParser<TLeft, TUserState>
    {
        private readonly Parser<TLeft, TUserState> _l;
        private readonly Parser<TRight, TUserState> _r;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TakeLeftParser(Parser<TLeft, TUserState> l, Parser<TRight, TUserState> r)
        {
            _l = l;
            _r = r;
            _parseMethodPtr = CalliHelper.LdvirtftnTakeLeft<TLeft, TRight, TUserState>();
            HasParseMethodPtr = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Reply<TLeft> InvokeImpl(CharStream<TUserState> stream)
        {
            return Parse(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Reply<TLeft> Parse(object th, CharStream<TUserState> stream)
        {
            return ((TakeLeftParser<TLeft, TRight, TUserState>)th).Parse(stream);
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

    internal sealed class TakeRightParser<TLeft, TRight, TUserState>
        : Parser<TRight, TUserState>, IParser<TRight, TUserState>
    {
        private readonly Parser<TLeft, TUserState> _l;
        private readonly Parser<TRight, TUserState> _r;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TakeRightParser(Parser<TLeft, TUserState> l, Parser<TRight, TUserState> r)
        {
            _l = l;
            _r = r;
            _parseMethodPtr = CalliHelper.LdvirtftnTakeRight<TLeft, TRight, TUserState>();
            HasParseMethodPtr = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Reply<TRight> InvokeImpl(CharStream<TUserState> stream)
        {
            // Console.WriteLine("TakeRightParser impl");
            return Parse(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Reply<TRight> Parse(object th, CharStream<TUserState> stream)
        {
            return ((TakeRightParser<TLeft, TRight, TUserState>)th).Parse(stream);
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
                reply1 =  _l.Invoke(stream);
            }

            if (reply1.Status == ReplyStatus.Ok)
            {
                if (reply1.Error == null)
                {
                    return  _r.Invoke(stream); ;
                }

                var stateTag1 = stream.StateTag;
                var reply2 = _r.Invoke(stream); ;
                if (stateTag1 == stream.StateTag)
                {
                    reply2.Error = ErrorMessageList.Merge(reply2.Error, reply1.Error);
                }
                return reply2;
            }

            return new Reply<TRight>(reply1.Status, reply1.Error);
        }
    }

    internal sealed class SpacesParser<TUserState> : Parser<Unit, TUserState>, IParser<Unit, TUserState>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpacesParser()
        {
            _parseMethodPtr = CalliHelper.LdvirtftnSpaces<TUserState>();
            HasParseMethodPtr = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Reply<Unit> InvokeImpl(CharStream<TUserState> stream)
        {
            // Console.WriteLine("SpacesParser impl: " + _parseMethodPtr);
            return Parse(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Reply<Unit> Parse(object th, CharStream<TUserState> stream)
        {
            return Unsafe.As<SpacesParser<TUserState>>(th).Parse(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<Unit> Parse(CharStream<TUserState> stream)
        {
            stream.SkipWhitespace();
            return new Reply<Unit>(default(Unit));
        }
    }


    [Sealed]
    [Serializable]
    internal sealed class PTakeBoth<a, b, u> : IParser<ValueTuple<a, b>, u>
    {
        // Token: 0x060001E0 RID: 480 RVA: 0x0000840C File Offset: 0x0000660C
        public PTakeBoth(Parser<a, u> p, Parser<b, u> q)
        {
            this.p = p;
            this.q = q;
        }

        // Token: 0x060001E1 RID: 481 RVA: 0x00008424 File Offset: 0x00006624
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<ValueTuple<a, b>> Parse(CharStream<u> stream)
        {
            Reply<a> reply = this.p.Invoke(stream);
            if (reply.Status == ReplyStatus.Ok)
            {
                ulong stateTag = stream.StateTag;
                Reply<b> reply2 = this.q.Invoke(stream);
                ErrorMessageList error = (stateTag == stream.StateTag) ? ErrorMessageList.Merge(reply.Error, reply2.Error) : reply2.Error;
                ValueTuple<a, b> result = (reply2.Status != ReplyStatus.Ok) ? default(ValueTuple<a, b>) : new ValueTuple<a, b>(reply.Result, reply2.Result);
                return new Reply<ValueTuple<a, b>>(reply2.Status, result, error);
            }
            return new Reply<ValueTuple<a, b>>(reply.Status, reply.Error);
        }
        internal Parser<b, u> q;

        internal Parser<a, u> p;
    }


    [Sealed]
    internal sealed class PChoiceArr<a, u> : IParser<a, u>
    {
        private Parser<a, u>[] _ps;

        public PChoiceArr(Parser<a, u>[] ps)
        {
            _ps = ps;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<a> Parse(CharStream<u> stream)
        {
            var stateTag = stream.StateTag;
            ErrorMessageList error = null;
            var p = _ps[0];
            var reply = p.Invoke(stream);
            var i = 1;
            while (reply.Status == ReplyStatus.Error && stateTag == stream.StateTag && i < _ps.Length)
            {
                error = ErrorMessageList.Merge(error, reply.Error);
                p = _ps[i];
                reply = p.Invoke(stream);
                i++;
            }
            if (stateTag == stream.StateTag)
            {
                error = ErrorMessageList.Merge(error, reply.Error);
                reply.Error = error;
            }
            return reply;
        }
    }

    [Sealed]
    internal class PBarGreaterGreater<a, b, u> : IParser<b, u>
    {
        private readonly Parser<a, u> _p;
        private readonly FSharpFunc<a, b> _f;

        public PBarGreaterGreater(Parser<a, u> p, FSharpFunc<a, b> f)
        {
            _p = p;
            _f = f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<b> Parse(CharStream<u> stream)
        {
            Reply<a> reply = _p.Invoke(stream);
            return new Reply<b>(reply.Status, (reply.Status != ReplyStatus.Ok) ? default(b) : _f.Invoke(reply.Result), reply.Error);
        }
    }


    [Sealed]
    [Serializable]
    internal sealed class PLessBarGreater<a, u> : IParser<a, u>
    {
        public PLessBarGreater(Parser<a, u> p1, Parser<a, u> p2) 
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<a> Parse(CharStream<u> stream)
        {
            ulong stateTag = stream.StateTag;
            Reply<a> reply = this.p1.Invoke(stream);
            if (reply.Status == ReplyStatus.Error && stateTag == stream.StateTag)
            {
                ErrorMessageList error = reply.Error;
                reply = this.p2.Invoke(stream);
                if (stateTag == stream.StateTag)
                {
                    reply.Error = ErrorMessageList.Merge(reply.Error, error);
                }
            }
            return reply;
        }

        internal Parser<a, u> p2;

        internal Parser<a, u> p1;
    }
}
