using System;
using System.Threading.Tasks;

namespace model.costs
{
    public class Free : ICost
    {
        bool ICost.Payable => true;
        public event Action<ICost, bool> ChangedPayability = delegate { };

        async Task ICost.Pay()
        {
            await Task.CompletedTask;
        }
    }
}
