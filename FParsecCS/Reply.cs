// Copyright (c) Stephan Tolksdorf 2008-2010
// License: Simplified BSD License. See accompanying documentation.

using System;
using System.Runtime.CompilerServices;

namespace FParsec
{
    public enum ReplyStatus
    {
        Ok = 1,
        Error = 0,
        FatalError = -1
    }

    [System.Diagnostics.DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    public readonly struct Reply<TResult> : IEquatable<Reply<TResult>>
    {
        internal readonly ErrorMessageList _error;
        // internal object _errorData;

        public ErrorMessageList Error
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _error;
            //is ErrorMessageList el
            //    ? el
            //    : _errorObject is FSharpFunc<string, ErrorMessageList> factory
            //        ? factory.Invoke((string)_errorData)
            //        : _errorObject is Func<object, ErrorMessageList> factory2
            //            ? factory2(_errorData) : null;
            // set => _error = value;
        }

        public readonly TResult Result;
        public readonly ReplyStatus Status;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply(TResult result)
        {
            Result = result;
            _error = null;
            // _errorData = null;
            Status = ReplyStatus.Ok;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply(ReplyStatus status, ErrorMessageList error)
        {
            Status = status;
            _error = error;
            // _errorData = null;
            Result = default(TResult);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply(ReplyStatus status, TResult result, ErrorMessageList error)
        {
            Status = status;
            _error = error;
            // _errorData = null;
            Result = result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TResult> WithStatus(ReplyStatus status)
        {
            return new Reply<TResult>(status, Result, Error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TResult> WithStatusError(ReplyStatus status, ErrorMessageList error)
        {
            return new Reply<TResult>(status, Result, error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TResult> WithStatusResult(ReplyStatus status, TResult result)
        {
            return new Reply<TResult>(status, result, Error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TResult> WithError(ErrorMessageList error)
        {
            return new Reply<TResult>(Status, Result, error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply<TResult> WithResult(TResult result)
        {
            return new Reply<TResult>(Status, result, Error);
        }

        //public Reply(ReplyStatus status, TResult result, FSharpFunc<object, ErrorMessageList> errorFactory, object errorData)
        //{
        //    Status = status;
        //    _errorObject = errorFactory;
        //    _errorData = errorData;
        //    Result = result;
        //}

        //public Reply(ReplyStatus status, TResult result, FSharpFunc<string, ErrorMessageList> errorFactory, object errorData)
        //{
        //    Status = status;
        //    _errorObject = errorFactory;
        //    _errorData = errorData;
        //    Result = result;
        //}

        public override bool Equals(object other)
        {
            if (!(other is Reply<TResult>)) return false;
            return Equals((Reply<TResult>)other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Reply<TResult> other)
        {
            return Status == other.Status
                   && (Status != ReplyStatus.Ok || FastGenericEqualityERComparer<TResult>.Instance.Equals(Result, other.Result))
                   && Error == other.Error;
        }

        public override int GetHashCode()
        {
            return (int)Status
                   ^ (Status != ReplyStatus.Ok ? 0 : FastGenericEqualityERComparer<TResult>.Instance.GetHashCode(Result));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Reply<TResult> r1, Reply<TResult> r2)
        {
            return r1.Equals(r2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Reply<TResult> r1, Reply<TResult> r2)
        {
            return !r1.Equals(r2);
        }

        private string GetDebuggerDisplay()
        {
            if (Status == ReplyStatus.Ok)
            {
                string result;
                if (Result == null)
                    result = typeof(TResult) == typeof(Microsoft.FSharp.Core.Unit) ? "()" : "null";
                else if (typeof(TResult) == typeof(string))
                    result = Text.DoubleQuote(Result.ToString());
                else
                    result = Result.ToString();

                return Error == null
                       ? "Reply(" + result + ")"
                       : "Reply(Ok, " + result + ", " + ErrorMessageList.GetDebuggerDisplay(Error) + ")";
            }
            else
            {
                var status = Status == ReplyStatus.Error ? "Error" :
                             Status == ReplyStatus.FatalError ? "FatalError" :
                             "(ReplyStatus)" + ((int)Status).ToString();

                return Error == null
                       ? "Reply(" + status + ", NoErrorMessages)"
                       : "Reply(" + status + ", " + ErrorMessageList.GetDebuggerDisplay(Error) + ")";
            }
        }
    }
}