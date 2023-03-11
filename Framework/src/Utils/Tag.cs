using System.Runtime.CompilerServices;

namespace Battery;

/// <summary>
///     Interface used to tagged classes.
/// </summary>
public interface ITagged
{
    /// <summary>
    ///     A list of tags.
    /// </summary>
    Tag Tag { get; set; }
}

/// <summary>
///     ITagged method Extensions.
/// </summary>
public static class ITaggedExt
{
    /// <summary>
    ///     Sets the tags of the component.
    /// </summary>
    /// <param name="value">The tag list to set.</param>
	public static ITagged SetTag(this ITagged tagged, Tag value)
	{
		tagged.Tag = value;
		return tagged;
	}
}

/// <summary>
///     A struct for managing tags.
/// </summary>
public class Tag : IEquatable<Tag>, IComparable<Tag>
{
    /// <summary>
    ///     No value.
    /// </summary>
    public static readonly Tag None = new Tag(0u);

    /// <summary>
    ///     Default tag.
    /// </summary>
    public static readonly Tag Default = new Tag(1u);

    /// <summary>
    ///     All tags combined.
    /// </summary>
    public static readonly Tag All = new Tag(uint.MaxValue);
    
    /// <summary>
    ///     Value of the tag.
    /// </summary>
    public uint Value;

    /// <summary>
    ///     Creates a new tag with the specified value.
    /// </summary>
    /// <param name="value">The value of the tag.</param>
    public Tag(uint value)
    {
        Value = value;
    }

    /// <summary>
    ///     Check if the given object is equals this tag.
    /// </summary>
    /// <param name="obj">Object to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj != null && obj is Tag tag && Equals(tag);
    }

    /// <summary>
    ///     Check if the given object is equals this tag.
    /// </summary>
    /// <param name="tag">Tag to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Tag? tag)
    {
        return tag != null && Value == tag.Value;
    }

    /// <summary>
    ///     Gets the tag hash code.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}

    /// <summary>
    ///     Compare the tag to other tag.
    /// </summary>
    /// <param name="other">Tag to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(Tag? other)
	{
		return Value.CompareTo(other == null ? 0u : other.Value);
	}

    /// <summary>
    ///     Checks if the tag contains any tag value.
    /// </summary>
    /// <param name="tag">Tag to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool ContainsAny(Tag tag)
	{
		return (Value & tag.Value) > 0u;
	}

    /// <summary>
    ///     Merge tags.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Tag operator |(Tag a, Tag b)
	{
		return new Tag(a.Value | b.Value);
	}

    /// <summary>
    ///     Unmerge.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
	public static Tag operator &(Tag a, Tag b)
	{
		return new Tag(a.Value & b.Value);
	}
}