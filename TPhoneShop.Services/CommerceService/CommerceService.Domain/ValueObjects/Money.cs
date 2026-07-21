namespace CommerceService.Domain.ValueObjects
{
    /// <summary>
    ///     Stored as amount ×100
    /// </summary>
    public record Money(long Amount, string Currency = Constants.Currency.VND)
    {
        public static readonly Money Zero = new(0, Constants.Currency.VND);

        public static Money operator +(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return new Money(left.Amount - right.Amount, left.Currency);
        }

        public static Money operator *(Money money, int quantity)
        {
            return new Money(money.Amount * quantity, money.Currency);
        }

        public static bool operator >(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return left.Amount > right.Amount;
        }

        public static bool operator <(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return left.Amount < right.Amount;
        }

        public static bool operator >=(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return left.Amount >= right.Amount;
        }

        public static bool operator <=(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return left.Amount <= right.Amount;
        }

        public int CompareTo(Money? other)
        {
            if (other is null)
                return 1;

            EnsureSameCurrency(this, other);
            return Amount.CompareTo(other.Amount);
        }
        private static void EnsureSameCurrency(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Currencies must match.");
        }
    }
}