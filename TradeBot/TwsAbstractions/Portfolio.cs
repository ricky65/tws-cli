using IBApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace TradeBot.TwsAbstractions
{
    public class Portfolio : Dictionary<string, Position>
    {
        public Position Get(string tickerSymbol)
        {
            if (tickerSymbol == null)
            {
                return null;
            }

            Position position;
            TryGetValue(tickerSymbol, out position);
            return position;
        }

        public async void Update(Position position, TradeService ts)
        {
            string tickerSymbol = position.Symbol;
            if (position.PositionSize == 0)
            {
                //Rick: Check if any active orders (Stop Loss/Limit Take Profit) and cancel them
                ts.OpenOrdersList = new List<OpenOrder>();

                ts.clientSocket.reqOpenOrders(); // Bind previous opened orders
                ts.clientSocket.reqAutoOpenOrders(true);  //Rick TODO This prob just needs calling once early on // Bind all future orders

                ts.openOrderEndTCS = new TaskCompletionSource();


                IEnumerable<OpenOrder> openOrders = await ts.RequestCurrentOpenOrdersAsync();

                foreach (OpenOrder openOrder in openOrders)
                {
                    ts.CancelOrder(openOrder.OrderID);
                }

                Remove(tickerSymbol);
            }            
            else
            {
                if (ContainsKey(tickerSymbol))
                {
                    //TODO: Rick: Need to figure out this logic

                    if (position.Symbol == this[tickerSymbol].Symbol && position.PositionSize != this[tickerSymbol].PositionSize)
                    {
                        //Rick: Position size has changed so modify any active orders (Stop Loss/Limit Take Profit) to the new size
                         ts.OpenOrdersList = new List<OpenOrder>();

                        ts.clientSocket.reqOpenOrders(); // Bind previous opened orders
                        ts.clientSocket.reqAutoOpenOrders(true);  // Bind all future orders

                        ts.openOrderEndTCS = new TaskCompletionSource();


                        IEnumerable<OpenOrder> openOrders = await ts.RequestCurrentOpenOrdersAsync();

                        foreach (OpenOrder openOrder in openOrders)
                        {
                            openOrder.Order.TotalQuantity = Math.Abs(position.PositionSize);
                            openOrder.Order.ParentId = 0;//Rick: Need to set ParentId to 0 as it was a child 
                            ts.ModifyOrderSize(openOrder.OrderID, openOrder.Contract, openOrder.Order);
                        }      
                    }



                    this[tickerSymbol] = position;
                    
                }
                else
                {
                    Add(tickerSymbol, position);
                }
            }
        }
    }
}
