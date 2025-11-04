namespace Shorty.Domain.Utilities;

public static class Base62
{
	private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

	public static string Encode(ulong value)
	{
		if (value == 0) return "000000";

		Span<char> buf = stackalloc char[11]; 
		int i = buf.Length;

		while (value > 0)
		{
			buf[--i] = Alphabet[(int)(value % 62)];
			value /= 62;
		}

		return new string(buf[i..]).PadLeft(6, '0');
	}
}
