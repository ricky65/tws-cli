using NLog;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeBot.Gui;
using TradeBot.WinGui;
using static TradeBot.AppProperties;

namespace TradeBot
{
    public class Program
    {
        public static void Main()
        {
            MainAsync().Wait();
        }

        public static async Task MainAsync()
        {
            //var controller = new TradeController();
            try
            {
               // await controller.Run();

                //Rick: GUI
                ApplicationConfiguration.Initialize();
                var tradePanel = new TradePanelForm();
                tradePanel.controller.Run();
                Application.Run(tradePanel);
            }
            catch (Exception e)
            {
                IO.ShowMessageCLI(LogLevel.Fatal, e.ToString());
            }
           //finally
           //{
               //if (OS.IsWindows())
           //     {
                    //IO.PromptForChar(Messages.PressAnyKeyToExit);
           //     }
           // }
        }
    }
}
