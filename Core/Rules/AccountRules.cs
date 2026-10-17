using System.Linq.Expressions;
using Core.Entities;

namespace Core.Rules;

public static class AccountRules
{
    public static Expression<Func<PsnAccount, bool>> HasFreeSlot(RentalType type) =>
        a => 
            a.Status == AccountStatus.Active
            && !a.Rentals.Any(r => r.Type == type && r.Status == RentalStatus.Active);
}
