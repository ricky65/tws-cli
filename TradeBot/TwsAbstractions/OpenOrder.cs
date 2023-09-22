using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeBot.Extensions;

namespace TradeBot.TwsAbstractions
{
    public class OpenOrder
    {
        public OpenOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            OrderID = orderId;
            Contract = contract;
            Order = order;
            OrderState = orderState;            
        }

        public string Symbol
        {
            get
            {
                return Contract?.Symbol;
            }
        }

        public int OrderID { get; }

        public Contract Contract { get; }

        public Order Order { get; }

        public OrderState OrderState { get; }

        public override string ToString()
        {
            return this.ToPrettyString(maxIndentLevel: 0);
        }
    }
}
