using IBApi;

namespace TradeBot.TwsAbstractions
{
    public static class OrderFactory
    {
        public static Order CreateLimitOrder(OrderActions action, double quantity, double limitPrice, bool transmitNow)
        {
            return new Order()
            {
                Action = action.ToString(),
                TotalQuantity = quantity,
                LmtPrice = limitPrice,
                OrderType = OrderTypes.Limit,
                Tif = nameof(TimeInForce.DAY),
                Transmit = transmitNow, //Rick: only last child in the bracket has Transmit = true
                OutsideRth = false
            };
        }

        //Rick
        public static Order CreateStopLimitOrder(OrderActions action, double quantity, double limitPrice, double stopPrice)
        {
            return new Order()
            {
                Action = action.ToString(),
                TotalQuantity = quantity,
                LmtPrice = limitPrice,
                AuxPrice = stopPrice,
                OrderType = OrderTypes.StopLimit,
                Tif = nameof(TimeInForce.DAY),
                Transmit = false, //Rick: only last child in the bracket has Transmit = true
                TriggerMethod = (int)TriggerMethod.LastOfBidAsk, 
                OutsideRth = false               
            };
        }
        //Rick
        public static Order CreateStopOrder(OrderActions action, double quantity,double stopPrice)
        {
            return new Order()
            {
                Action = action.ToString(),
                TotalQuantity = quantity,
                AuxPrice = stopPrice,
                OrderType = OrderTypes.Stop,
                Tif = nameof(TimeInForce.DAY),
                Transmit = true, //Rick: only last child in the bracket has Transmit = true
                TriggerMethod = (int)TriggerMethod.LastOfBidAsk, 
                OutsideRth = false
            };
        }
    }
}
