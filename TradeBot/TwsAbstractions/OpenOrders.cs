using IBApi;
using TradeBot.Extensions;

namespace TradeBot.TwsAbstractions
{
    public class OpenOrder
    {
        public OpenOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            OrderId = orderId;
            Contract = contract;
            Order = order;
            OrderState = orderState;
        }

        public int OrderId { get; }
        public Contract Contract { get; }
        public Order Order { get; }
        public OrderState OrderState { get; }

        public override string ToString()
        {
            return this.ToPrettyString(maxIndentLevel: 0);
        }
    }
}
