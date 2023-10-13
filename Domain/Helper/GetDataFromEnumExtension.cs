namespace Domain.Helpers
{
    public static class GetDataFromEnumExtension
    {
        public static string GetEnumNameFromNumber<TEnum>(this int number) where TEnum : struct, Enum
        {
            if (Enum.IsDefined(typeof(TEnum), number))
            {
                return Enum.GetName(typeof(TEnum), number)!;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Invalid enum value");
            }
        }
    }
}
