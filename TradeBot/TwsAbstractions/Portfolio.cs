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

                ts.openOrderEndTCS = new TaskCompletionSource();

                IEnumerable<OpenOrder> openOrders = await ts.RequestOpenOrdersForTickerAsync(tickerSymbol);

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
                    //Rick: TODO: Test this Monday morning
                    double absNewPosSize = Math.Abs(position.PositionSize);
                    if (absNewPosSize < Math.Abs(this[tickerSymbol].PositionSize))
                    {
                        //Rick: New position size is now less than previous position size (part of position closed) so reduce any active orders (Stop Loss/Limit Take Profit) > New Pos Size to the New Pos Size
                        ts.OpenOrdersList = new List<OpenOrder>();

                        ts.clientSocket.reqOpenOrders(); // Bind previous opened orders

                        ts.openOrderEndTCS = new TaskCompletionSource();

                        IEnumerable<OpenOrder> openOrders = await ts.RequestOpenOrdersForTickerAsync(tickerSymbol);

                        foreach (OpenOrder openOrder in openOrders)
                        {
                            if (openOrder.Order.TotalQuantity > absNewPosSize) { 
                                openOrder.Order.TotalQuantity = absNewPosSize;
                                openOrder.Order.ParentId = 0;//Rick: Need to set ParentId to 0 as Stop was probably a child 
                                ts.ModifyOrder(openOrder.OrderID, openOrder.Contract, openOrder.Order);
                            }
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
