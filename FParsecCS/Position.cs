// Copyright (c) Stephan Tolksdorf 2007-2009
// License: Simplified BSD License. See accompanying documentation.

using System;
using System.Runtime.CompilerServices;

namespace FParsec
{
    public readonly struct Position : IEquatable<Position>, IComparable, IComparable<Position>
    {
        public readonly long Index;
        public readonly long Line;
        public readonly long Column;
        public readonly string StreamName;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Position(string streamName, long index, long line, long column)
        {
            StreamName = streamName; Index = index; Line = line; Column = column;
        }

        public override string ToString()
        {
            var ln = String.IsNullOrEmpty(StreamName) ? "(Ln: " : Text.Escape(StreamName, "", "(\"", "\", Ln: ", "", '"');
            return ln + Line.ToString() + ", Col: " + Column.ToString() + ")";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Position other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Position other)
        {
            return Index == other.Index
                       && Line == other.Line
                       && Column == other.Column
                       && StreamName == other.StreamName;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }

        // TODO (VB) compare with struct

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Compare(in Position left, in Position right)
        {
            return left.CompareTo(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Position other)
        {
            if (Equals(other)) return 0;
            if (other == default) return 1;
            int r = String.CompareOrdinal(StreamName, other.StreamName);
            if (r != 0) return r;
            r = Line.CompareTo(other.Line);
            if (r != 0) return r;
            r = Column.CompareTo(other.Column);
            if (r != 0) return r;
            return Index.CompareTo(other.Index);
        }

        int IComparable.CompareTo(object value)
        {
            if (value is Position other) return CompareTo(other);
            if (value == null) return 1;
            throw new ArgumentException("Object must be of type Position.");
        }
    }
}
