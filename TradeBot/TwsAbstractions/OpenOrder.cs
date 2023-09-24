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
            OrderId = orderId;
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

        public int OrderId { get; }

        public Contract Contract { get; }

        public Order Order { get; }

        public OrderState OrderState { get; }

        public override string ToString()
        {
            //return this.ToPrettyString(maxIndentLevel: 0);
            return "OpenOrder: OrderId: " + Util.IntMaxString(OrderId) + ", ClientId: " + Util.IntMaxString(Order.ClientId) +", PermID: " + Util.IntMaxString(Order.PermId) +
            ", Account: " + Order.Account + ", Symbol: " + Contract.Symbol + ", SecType: " + Contract.SecType + " , Exchange: " + Contract.Exchange + ", Action: " + Order.Action +
            ", OrderType: " + Order.OrderType + ", TotalQty: " + Util.DoubleMaxString(Order.TotalQuantity) + ", CashQty: " + Util.DoubleMaxString(Order.CashQty) +
            ", LmtPrice: " + Util.DoubleMaxString(Order.LmtPrice) + ", AuxPrice: " + Util.DoubleMaxString(Order.AuxPrice) + ", Status: " + OrderState.Status;

        }
    }
}
