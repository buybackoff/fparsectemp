// Copyright (c) Stephan Tolksdorf 2008-2010
// License: Simplified BSD License. See accompanying documentation.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Spreads.Slang.FParsec
{
    public enum ReplyStatus
    {
        Ok = 1,
        Error = 0,
        FatalError = -1
    }

    [System.Diagnostics.DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    public struct Reply<TResult> : IEquatable<Reply<TResult>>
    {
        internal ErrorMessageList Error;
        public TResult Result;
        public ReplyStatus Status;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply(TResult result)
        {
            Result = result;
            Error = null;
            // _errorData = null;
            Status = ReplyStatus.Ok;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply(ReplyStatus status, ErrorMessageList error)
        {
            Status = status;
            Error = error;
            // _errorData = null;
            Result = default(TResult);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Reply(ReplyStatus status, TResult result, ErrorMessageList error)
        {
            Status = status;
            Error = error;
            // _errorData = null;
            Result = result;
        }

        public override bool Equals(object other)
        {
            if (!(other is Reply<TResult>)) return false;
            return Equals((Reply<TResult>)other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Reply<TResult> other)
        {
            return Status == other.Status
                   && (Status != ReplyStatus.Ok || EqualityComparer<TResult>.Default.Equals(Result, other.Result))
                   && Error == other.Error;
        }

        public override int GetHashCode()
        {
            return (int)Status
                   ^ (Status != ReplyStatus.Ok ? 0 : EqualityComparer<TResult>.Default.GetHashCode(Result));
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