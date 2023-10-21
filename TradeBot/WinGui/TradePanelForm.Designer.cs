namespace TradeBot.WinGui
{
    partial class TradePanelForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            stock1GroupBox = new GroupBox();
            BidAskLabel = new Label();
            BidAskOutput = new Label();
            BidAskSizeLabel = new Label();
            BidAskSizeOutput = new Label();
            ButtonCFD = new Button();
            ButtonStock = new Button();
            BuyStop05Percent = new Button();
            BuyStop125Percent = new Button();
            BuyStop15Percent = new Button();
            BuyStop1Percent = new Button();
            BuyStop25Percent = new Button();
            BuyStop2Percent = new Button();
            BuyStopPriceLabel = new Label();
            BuyStopPriceTextBox = new TextBox();
            Close100Percent = new Button();
            Close25Percent = new Button();
            Close33Percent = new Button();
            Close50Percent = new Button();
            Close67Percent = new Button();
            LastPriceLabel = new Label();
            LastPriceOutputLabel = new Label();
            Long05Percent = new Button();
            Long125percent = new Button();
            Long15Percent = new Button();
            Long1Percent = new Button();
            Long25Percent = new Button();
            Long2Percent = new Button();
            OutsideRTHCheckbox = new CheckBox();
            PercentageChangeLabel = new Label();
            PercentageChangeOutputLabel = new Label();
            PositionLabel = new Label();
            PositionOutputLabel = new Label();
            ReverseButton = new Button();
            SellStop05Percent = new Button();
            SellStop125Percent = new Button();
            SellStop15Percent = new Button();
            SellStop1Percent = new Button();
            SellStop2Percent = new Button();
            SellStopPriceLabel = new Label();
            SellStopPriceTextBox = new TextBox();
            Short05Percent = new Button();
            Short125Percent = new Button();
            Short15Percent = new Button();
            Short1Percent = new Button();
            Short2Percent = new Button();
            TakeProfit25Percent = new Button();
            TakeProfit33Percent = new Button();
            TakeProfit50Percent = new Button();
            TakeProfitLimitPriceLabel = new Label();
            TakeProfitLimitPriceTextBox = new TextBox();
            TickerInput = new TextBox();
            TickerLabel = new Label();
            GlobalOutputTextBox = new TextBox();
            stock1GroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // stock1GroupBox
            // 
            stock1GroupBox.Controls.Add(BidAskLabel);
            stock1GroupBox.Controls.Add(BidAskOutput);
            stock1GroupBox.Controls.Add(BidAskSizeLabel);
            stock1GroupBox.Controls.Add(BidAskSizeOutput);
            stock1GroupBox.Controls.Add(ButtonCFD);
            stock1GroupBox.Controls.Add(ButtonStock);
            stock1GroupBox.Controls.Add(BuyStop05Percent);
            stock1GroupBox.Controls.Add(BuyStop125Percent);
            stock1GroupBox.Controls.Add(BuyStop15Percent);
            stock1GroupBox.Controls.Add(BuyStop1Percent);
            stock1GroupBox.Controls.Add(BuyStop25Percent);
            stock1GroupBox.Controls.Add(BuyStop2Percent);
            stock1GroupBox.Controls.Add(BuyStopPriceLabel);
            stock1GroupBox.Controls.Add(BuyStopPriceTextBox);
            stock1GroupBox.Controls.Add(Close100Percent);
            stock1GroupBox.Controls.Add(Close25Percent);
            stock1GroupBox.Controls.Add(Close33Percent);
            stock1GroupBox.Controls.Add(Close50Percent);
            stock1GroupBox.Controls.Add(Close67Percent);
            stock1GroupBox.Controls.Add(LastPriceLabel);
            stock1GroupBox.Controls.Add(LastPriceOutputLabel);
            stock1GroupBox.Controls.Add(Long05Percent);
            stock1GroupBox.Controls.Add(Long125percent);
            stock1GroupBox.Controls.Add(Long15Percent);
            stock1GroupBox.Controls.Add(Long1Percent);
            stock1GroupBox.Controls.Add(Long25Percent);
            stock1GroupBox.Controls.Add(Long2Percent);
            stock1GroupBox.Controls.Add(OutsideRTHCheckbox);
            stock1GroupBox.Controls.Add(PercentageChangeLabel);
            stock1GroupBox.Controls.Add(PercentageChangeOutputLabel);
            stock1GroupBox.Controls.Add(PositionLabel);
            stock1GroupBox.Controls.Add(PositionOutputLabel);
            stock1GroupBox.Controls.Add(ReverseButton);
            stock1GroupBox.Controls.Add(SellStop05Percent);
            stock1GroupBox.Controls.Add(SellStop125Percent);
            stock1GroupBox.Controls.Add(SellStop15Percent);
            stock1GroupBox.Controls.Add(SellStop1Percent);
            stock1GroupBox.Controls.Add(SellStop2Percent);
            stock1GroupBox.Controls.Add(SellStopPriceLabel);
            stock1GroupBox.Controls.Add(SellStopPriceTextBox);
            stock1GroupBox.Controls.Add(Short05Percent);
            stock1GroupBox.Controls.Add(Short125Percent);
            stock1GroupBox.Controls.Add(Short15Percent);
            stock1GroupBox.Controls.Add(Short1Percent);
            stock1GroupBox.Controls.Add(Short2Percent);
            stock1GroupBox.Controls.Add(TakeProfit25Percent);
            stock1GroupBox.Controls.Add(TakeProfit33Percent);
            stock1GroupBox.Controls.Add(TakeProfit50Percent);
            stock1GroupBox.Controls.Add(TakeProfitLimitPriceLabel);
            stock1GroupBox.Controls.Add(TakeProfitLimitPriceTextBox);
            stock1GroupBox.Controls.Add(TickerInput);
            stock1GroupBox.Controls.Add(TickerLabel);
            stock1GroupBox.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            stock1GroupBox.Location = new Point(12, 106);
            stock1GroupBox.Name = "stock1GroupBox";
            stock1GroupBox.Size = new Size(711, 587);
            stock1GroupBox.TabIndex = 1;
            stock1GroupBox.TabStop = false;
            stock1GroupBox.Text = "Stock 1";
            stock1GroupBox.Enter += stock1GroupBox_Enter;
            // 
            // BidAskLabel
            // 
            BidAskLabel.AutoSize = true;
            BidAskLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            BidAskLabel.Location = new Point(12, 145);
            BidAskLabel.Name = "BidAskLabel";
            BidAskLabel.Size = new Size(63, 21);
            BidAskLabel.TabIndex = 11;
            BidAskLabel.Text = "Bid/Ask";
            // 
            // BidAskOutput
            // 
            BidAskOutput.AutoSize = false;
            BidAskOutput.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            BidAskOutput.Location = new Point(124, 140);
            BidAskOutput.Name = "BidAskOutput";
            BidAskOutput.Size = new Size(300, 25);
            BidAskOutput.TabIndex = 12;
            BidAskOutput.Text = "";
            BidAskOutput.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // BidAskSizeLabel
            // 
            BidAskSizeLabel.AutoSize = true;
            BidAskSizeLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            BidAskSizeLabel.Location = new Point(12, 169);
            BidAskSizeLabel.Name = "BidAskSizeLabel";
            BidAskSizeLabel.Size = new Size(38, 21);
            BidAskSizeLabel.TabIndex = 10;
            BidAskSizeLabel.Text = "Size";
            // 
            // BidAskSizeOutput
            // 
            BidAskSizeOutput.AutoSize = false;
            BidAskSizeOutput.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            BidAskSizeOutput.Location = new Point(124, 165);
            BidAskSizeOutput.Name = "BidAskSizeOutput";
            BidAskSizeOutput.Size = new Size(300, 25);
            BidAskSizeOutput.TabIndex = 13;
            BidAskSizeOutput.Text = "";
            BidAskSizeOutput.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ButtonCFD
            // 
            ButtonCFD.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            ButtonCFD.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            ButtonCFD.Location = new Point(241, 32);
            ButtonCFD.Name = "ButtonCFD";
            ButtonCFD.Size = new Size(71, 40);
            ButtonCFD.TabIndex = 3;
            ButtonCFD.Text = "CFD";
            ButtonCFD.UseVisualStyleBackColor = true;
            ButtonCFD.Click += buttonCFD_Click;
            // 
            // ButtonStock
            // 
            ButtonStock.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            ButtonStock.Location = new Point(318, 32);
            ButtonStock.Name = "ButtonStock";
            ButtonStock.Size = new Size(71, 40);
            ButtonStock.TabIndex = 2;
            ButtonStock.Text = "Stock";
            ButtonStock.UseVisualStyleBackColor = true;
            ButtonStock.Click += buttonStock_Click;
            // 
            // BuyStop05Percent
            // 
            BuyStop05Percent.BackColor = Color.Lime;
            BuyStop05Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            BuyStop05Percent.Location = new Point(8, 346);
            BuyStop05Percent.Name = "BuyStop05Percent";
            BuyStop05Percent.Size = new Size(114, 40);
            BuyStop05Percent.TabIndex = 17;
            BuyStop05Percent.Text = "Buy Stop 0.5%";
            BuyStop05Percent.UseVisualStyleBackColor = false;
            BuyStop05Percent.Click += BuyStop05Percent_Click;
            // 
            // BuyStop125Percent
            // 
            BuyStop125Percent.BackColor = Color.Lime;
            BuyStop125Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            BuyStop125Percent.Location = new Point(241, 346);
            BuyStop125Percent.Name = "BuyStop125Percent";
            BuyStop125Percent.Size = new Size(126, 40);
            BuyStop125Percent.TabIndex = 19;
            BuyStop125Percent.Text = "Buy Stop 1.25%";
            BuyStop125Percent.UseVisualStyleBackColor = false;
            BuyStop125Percent.Click += BuyStop125Percent_Click;
            // 
            // BuyStop15Percent
            // 
            BuyStop15Percent.BackColor = Color.Lime;
            BuyStop15Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            BuyStop15Percent.Location = new Point(373, 346);
            BuyStop15Percent.Name = "BuyStop15Percent";
            BuyStop15Percent.Size = new Size(111, 40);
            BuyStop15Percent.TabIndex = 20;
            BuyStop15Percent.Text = "Buy Stop 1.5%";
            BuyStop15Percent.UseVisualStyleBackColor = false;
            BuyStop15Percent.Click += BuyStop15Percent_Click;
            // 
            // BuyStop1Percent
            // 
            BuyStop1Percent.BackColor = Color.Lime;
            BuyStop1Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            BuyStop1Percent.Location = new Point(128, 346);
            BuyStop1Percent.Name = "BuyStop1Percent";
            BuyStop1Percent.Size = new Size(107, 40);
            BuyStop1Percent.TabIndex = 18;
            BuyStop1Percent.Text = "Buy Stop 1%";
            BuyStop1Percent.UseVisualStyleBackColor = false;
            BuyStop1Percent.Click += BuyStop1Percent_Click;
            // 
            // BuyStop25Percent
            // 
            BuyStop25Percent.BackColor = Color.Lime;
            BuyStop25Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            BuyStop25Percent.Location = new Point(595, 346);
            BuyStop25Percent.Name = "BuyStop25Percent";
            BuyStop25Percent.Size = new Size(111, 40);
            BuyStop25Percent.TabIndex = 22;
            BuyStop25Percent.Text = "Buy Stop 2.5%";
            BuyStop25Percent.UseVisualStyleBackColor = false;
            BuyStop25Percent.Click += BuyStop25Percent_Click;
            // 
            // BuyStop2Percent
            // 
            BuyStop2Percent.BackColor = Color.Lime;
            BuyStop2Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            BuyStop2Percent.Location = new Point(490, 346);
            BuyStop2Percent.Name = "BuyStop2Percent";
            BuyStop2Percent.Size = new Size(101, 40);
            BuyStop2Percent.TabIndex = 21;
            BuyStop2Percent.Text = "Buy Stop 2%";
            BuyStop2Percent.UseVisualStyleBackColor = false;
            BuyStop2Percent.Click += BuyStop2Percent_Click;
            // 
            // BuyStopPriceLabel
            // 
            BuyStopPriceLabel.AutoSize = true;
            BuyStopPriceLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            BuyStopPriceLabel.Location = new Point(241, 208);
            BuyStopPriceLabel.Name = "BuyStopPriceLabel";
            BuyStopPriceLabel.Size = new Size(101, 21);
            BuyStopPriceLabel.TabIndex = 54;
            BuyStopPriceLabel.Text = "BuyStopPrice";
            // 
            // BuyStopPriceTextBox
            // 
            BuyStopPriceTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            BuyStopPriceTextBox.Location = new Point(356, 205);
            BuyStopPriceTextBox.Name = "BuyStopPriceTextBox";
            BuyStopPriceTextBox.Size = new Size(100, 29);
            BuyStopPriceTextBox.TabIndex = 5;
            BuyStopPriceTextBox.TextChanged += BuyStopPriceTextBox_TextChanged;
            // 
            // Close100Percent
            // 
            Close100Percent.BackColor = Color.DarkOrange;
            Close100Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Close100Percent.Location = new Point(8, 443);
            Close100Percent.Name = "Close100Percent";
            Close100Percent.Size = new Size(107, 40);
            Close100Percent.TabIndex = 28;
            Close100Percent.Text = "Close 100%";
            Close100Percent.UseVisualStyleBackColor = false;
            Close100Percent.Click += Close100Percent_Click;
            // 
            // Close25Percent
            // 
            Close25Percent.BackColor = Color.DarkOrange;
            Close25Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Close25Percent.Location = new Point(438, 443);
            Close25Percent.Name = "Close25Percent";
            Close25Percent.Size = new Size(99, 40);
            Close25Percent.TabIndex = 32;
            Close25Percent.Text = "Close 25%";
            Close25Percent.UseVisualStyleBackColor = false;
            Close25Percent.Click += Close25Percent_Click;
            // 
            // Close33Percent
            // 
            Close33Percent.BackColor = Color.DarkOrange;
            Close33Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Close33Percent.Location = new Point(229, 443);
            Close33Percent.Name = "Close33Percent";
            Close33Percent.Size = new Size(99, 40);
            Close33Percent.TabIndex = 30;
            Close33Percent.Text = "Close 33%";
            Close33Percent.UseVisualStyleBackColor = false;
            Close33Percent.Click += Close33Percent_Click;
            // 
            // Close50Percent
            // 
            Close50Percent.BackColor = Color.DarkOrange;
            Close50Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Close50Percent.Location = new Point(125, 443);
            Close50Percent.Name = "Close50Percent";
            Close50Percent.Size = new Size(99, 40);
            Close50Percent.TabIndex = 29;
            Close50Percent.Text = "Close 50%";
            Close50Percent.UseVisualStyleBackColor = false;
            Close50Percent.Click += Close50Percent_Click;
            // 
            // Close67Percent
            // 
            Close67Percent.BackColor = Color.DarkOrange;
            Close67Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Close67Percent.Location = new Point(333, 443);
            Close67Percent.Name = "Close67Percent";
            Close67Percent.Size = new Size(99, 40);
            Close67Percent.TabIndex = 31;
            Close67Percent.Text = "Close 67%";
            Close67Percent.UseVisualStyleBackColor = false;
            Close67Percent.Click += Close67Percent_Click;
            // 
            // LastPriceLabel
            // 
            LastPriceLabel.AutoSize = true;
            LastPriceLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            LastPriceLabel.Location = new Point(12, 98);
            LastPriceLabel.Name = "LastPriceLabel";
            LastPriceLabel.Size = new Size(38, 21);
            LastPriceLabel.TabIndex = 8;
            LastPriceLabel.Text = "Last";
            // 
            // LastPriceOutputLabel
            // 
            LastPriceOutputLabel.AutoSize = true;
            LastPriceOutputLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            LastPriceOutputLabel.Location = new Point(124, 98);
            LastPriceOutputLabel.Name = "LastPriceOutputLabel";
            LastPriceOutputLabel.Size = new Size(59, 21);
            LastPriceOutputLabel.TabIndex = 66;
            LastPriceOutputLabel.Text = "$24.49";
            // 
            // Long05Percent
            // 
            Long05Percent.BackColor = Color.Lime;
            Long05Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Long05Percent.Location = new Point(8, 245);
            Long05Percent.Name = "Long05Percent";
            Long05Percent.Size = new Size(110, 40);
            Long05Percent.TabIndex = 6;
            Long05Percent.Text = "Long 0.5%";
            Long05Percent.UseVisualStyleBackColor = false;
            Long05Percent.Click += long05Percent_Click;
            // 
            // Long125percent
            // 
            Long125percent.BackColor = Color.Lime;
            Long125percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Long125percent.Location = new Point(240, 245);
            Long125percent.Name = "Long125percent";
            Long125percent.Size = new Size(110, 40);
            Long125percent.TabIndex = 8;
            Long125percent.Text = "Long 1.25%";
            Long125percent.UseVisualStyleBackColor = false;
            Long125percent.Click += Long125percent_Click;
            // 
            // Long15Percent
            // 
            Long15Percent.BackColor = Color.Lime;
            Long15Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Long15Percent.Location = new Point(356, 245);
            Long15Percent.Name = "Long15Percent";
            Long15Percent.Size = new Size(110, 40);
            Long15Percent.TabIndex = 9;
            Long15Percent.Text = "Long 1.5%";
            Long15Percent.UseVisualStyleBackColor = false;
            Long15Percent.Click += Long15Percent_Click;
            // 
            // Long1Percent
            // 
            Long1Percent.BackColor = Color.Lime;
            Long1Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Long1Percent.Location = new Point(124, 245);
            Long1Percent.Name = "Long1Percent";
            Long1Percent.Size = new Size(110, 40);
            Long1Percent.TabIndex = 7;
            Long1Percent.Text = "Long 1%";
            Long1Percent.UseVisualStyleBackColor = false;
            Long1Percent.Click += Long1Percent_Click;
            // 
            // Long25Percent
            // 
            Long25Percent.BackColor = Color.Lime;
            Long25Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Long25Percent.Location = new Point(588, 245);
            Long25Percent.Name = "Long25Percent";
            Long25Percent.Size = new Size(110, 40);
            Long25Percent.TabIndex = 11;
            Long25Percent.Text = "Long 2.5%";
            Long25Percent.UseVisualStyleBackColor = false;
            Long25Percent.Click += Long25Percent_Click;
            // 
            // Long2Percent
            // 
            Long2Percent.BackColor = Color.Lime;
            Long2Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Long2Percent.Location = new Point(472, 245);
            Long2Percent.Name = "Long2Percent";
            Long2Percent.Size = new Size(110, 40);
            Long2Percent.TabIndex = 10;
            Long2Percent.Text = "Long 2%";
            Long2Percent.UseVisualStyleBackColor = false;
            Long2Percent.Click += Long2Percent_Click;
            // 
            // OutsideRTHCheckbox
            // 
            OutsideRTHCheckbox.AutoSize = true;
            OutsideRTHCheckbox.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            OutsideRTHCheckbox.Location = new Point(416, 41);
            OutsideRTHCheckbox.Name = "OutsideRTHCheckbox";
            OutsideRTHCheckbox.Size = new Size(121, 25);
            OutsideRTHCheckbox.TabIndex = 68;
            OutsideRTHCheckbox.Text = "Outside RTH";
            OutsideRTHCheckbox.UseVisualStyleBackColor = true;
            OutsideRTHCheckbox.CheckedChanged += OutsideRTHCheckbox_CheckedChanged;
            // 
            // PercentageChangeLabel
            // 
            PercentageChangeLabel.AutoSize = true;
            PercentageChangeLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            PercentageChangeLabel.Location = new Point(12, 119);
            PercentageChangeLabel.Name = "PercentageChangeLabel";
            PercentageChangeLabel.Size = new Size(83, 21);
            PercentageChangeLabel.TabIndex = 9;
            PercentageChangeLabel.Text = "PctChange";
            // 
            // PercentageChangeOutputLabel
            // 
            PercentageChangeOutputLabel.AutoSize = true;
            PercentageChangeOutputLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            PercentageChangeOutputLabel.Location = new Point(124, 119);
            PercentageChangeOutputLabel.Name = "PercentageChangeOutputLabel";
            PercentageChangeOutputLabel.Size = new Size(65, 21);
            PercentageChangeOutputLabel.TabIndex = 67;
            PercentageChangeOutputLabel.Text = "+0.56%";
            // 
            // PositionLabel
            // 
            PositionLabel.AutoSize = true;
            PositionLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            PositionLabel.Location = new Point(12, 74);
            PositionLabel.Name = "PositionLabel";
            PositionLabel.Size = new Size(65, 21);
            PositionLabel.TabIndex = 57;
            PositionLabel.Text = "Position";
            // 
            // PositionOutputLabel
            // 
            PositionOutputLabel.AutoSize = true;
            PositionOutputLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            PositionOutputLabel.Location = new Point(124, 74);
            PositionOutputLabel.Name = "PositionOutputLabel";
            PositionOutputLabel.Size = new Size(71, 21);
            PositionOutputLabel.TabIndex = 58;
            PositionOutputLabel.Text = "0 Shares";
            // 
            // ReverseButton
            // 
            ReverseButton.BackColor = Color.Tomato;
            ReverseButton.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ReverseButton.Location = new Point(570, 443);
            ReverseButton.Name = "ReverseButton";
            ReverseButton.Size = new Size(99, 40);
            ReverseButton.TabIndex = 33;
            ReverseButton.Text = "Reverse";
            ReverseButton.UseVisualStyleBackColor = false;
            ReverseButton.Click += reverseButton_Click;
            // 
            // SellStop05Percent
            // 
            SellStop05Percent.BackColor = Color.Red;
            SellStop05Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            SellStop05Percent.ForeColor = Color.White;
            SellStop05Percent.Location = new Point(9, 392);
            SellStop05Percent.Name = "SellStop05Percent";
            SellStop05Percent.Size = new Size(114, 40);
            SellStop05Percent.TabIndex = 23;
            SellStop05Percent.Text = "Sell Stop 0.5%";
            SellStop05Percent.UseVisualStyleBackColor = false;
            SellStop05Percent.Click += SellStop05Percent_Click;
            // 
            // SellStop125Percent
            // 
            SellStop125Percent.BackColor = Color.Red;
            SellStop125Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            SellStop125Percent.ForeColor = Color.White;
            SellStop125Percent.Location = new Point(241, 392);
            SellStop125Percent.Name = "SellStop125Percent";
            SellStop125Percent.Size = new Size(126, 40);
            SellStop125Percent.TabIndex = 25;
            SellStop125Percent.Text = "Sell Stop 1.25%";
            SellStop125Percent.UseVisualStyleBackColor = false;
            SellStop125Percent.Click += SellStop125Percent_Click;
            // 
            // SellStop15Percent
            // 
            SellStop15Percent.BackColor = Color.Red;
            SellStop15Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            SellStop15Percent.ForeColor = Color.White;
            SellStop15Percent.Location = new Point(373, 392);
            SellStop15Percent.Name = "SellStop15Percent";
            SellStop15Percent.Size = new Size(111, 40);
            SellStop15Percent.TabIndex = 26;
            SellStop15Percent.Text = "Sell Stop 1.5%";
            SellStop15Percent.UseVisualStyleBackColor = false;
            SellStop15Percent.Click += SellStop15Percent_Click;
            // 
            // SellStop1Percent
            // 
            SellStop1Percent.BackColor = Color.Red;
            SellStop1Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            SellStop1Percent.ForeColor = Color.White;
            SellStop1Percent.Location = new Point(128, 392);
            SellStop1Percent.Name = "SellStop1Percent";
            SellStop1Percent.Size = new Size(107, 40);
            SellStop1Percent.TabIndex = 24;
            SellStop1Percent.Text = "Sell Stop 1%";
            SellStop1Percent.UseVisualStyleBackColor = false;
            SellStop1Percent.Click += SellStop1Percent_Click;
            // 
            // SellStop2Percent
            // 
            SellStop2Percent.BackColor = Color.Red;
            SellStop2Percent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            SellStop2Percent.ForeColor = Color.White;
            SellStop2Percent.Location = new Point(490, 392);
            SellStop2Percent.Name = "SellStop2Percent";
            SellStop2Percent.Size = new Size(101, 40);
            SellStop2Percent.TabIndex = 27;
            SellStop2Percent.Text = "Sell Stop 2%";
            SellStop2Percent.UseVisualStyleBackColor = false;
            SellStop2Percent.Click += SellStop2Percent_Click;
            // 
            // SellStopPriceLabel
            // 
            SellStopPriceLabel.AutoSize = true;
            SellStopPriceLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SellStopPriceLabel.Location = new Point(9, 208);
            SellStopPriceLabel.Name = "SellStopPriceLabel";
            SellStopPriceLabel.Size = new Size(100, 21);
            SellStopPriceLabel.TabIndex = 52;
            SellStopPriceLabel.Text = "SellStopPrice";
            // 
            // SellStopPriceTextBox
            // 
            SellStopPriceTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SellStopPriceTextBox.Location = new Point(125, 205);
            SellStopPriceTextBox.Name = "SellStopPriceTextBox";
            SellStopPriceTextBox.Size = new Size(100, 29);
            SellStopPriceTextBox.TabIndex = 4;
            SellStopPriceTextBox.TextChanged += SellStopPriceTextBox_TextChanged;
            // 
            // Short05Percent
            // 
            Short05Percent.BackColor = Color.Red;
            Short05Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Short05Percent.ForeColor = Color.White;
            Short05Percent.Location = new Point(8, 291);
            Short05Percent.Name = "Short05Percent";
            Short05Percent.Size = new Size(110, 40);
            Short05Percent.TabIndex = 12;
            Short05Percent.Text = "Short 0.5%";
            Short05Percent.UseVisualStyleBackColor = false;
            Short05Percent.Click += short05Percent_Click;
            // 
            // Short125Percent
            // 
            Short125Percent.BackColor = Color.Red;
            Short125Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Short125Percent.ForeColor = Color.White;
            Short125Percent.Location = new Point(240, 291);
            Short125Percent.Name = "Short125Percent";
            Short125Percent.Size = new Size(110, 40);
            Short125Percent.TabIndex = 14;
            Short125Percent.Text = "Short 1.25%";
            Short125Percent.UseVisualStyleBackColor = false;
            Short125Percent.Click += Short125Percent_Click;
            // 
            // Short15Percent
            // 
            Short15Percent.BackColor = Color.Red;
            Short15Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Short15Percent.ForeColor = Color.White;
            Short15Percent.Location = new Point(356, 291);
            Short15Percent.Name = "Short15Percent";
            Short15Percent.Size = new Size(110, 40);
            Short15Percent.TabIndex = 15;
            Short15Percent.Text = "Short 1.5%";
            Short15Percent.UseVisualStyleBackColor = false;
            Short15Percent.Click += Short15Percent_Click;
            // 
            // Short1Percent
            // 
            Short1Percent.BackColor = Color.Red;
            Short1Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Short1Percent.ForeColor = Color.White;
            Short1Percent.Location = new Point(124, 291);
            Short1Percent.Name = "Short1Percent";
            Short1Percent.Size = new Size(110, 40);
            Short1Percent.TabIndex = 13;
            Short1Percent.Text = "Short 1%";
            Short1Percent.UseVisualStyleBackColor = false;
            Short1Percent.Click += Short1Percent_Click;
            // 
            // Short2Percent
            // 
            Short2Percent.BackColor = Color.Red;
            Short2Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Short2Percent.ForeColor = Color.White;
            Short2Percent.Location = new Point(472, 291);
            Short2Percent.Name = "Short2Percent";
            Short2Percent.Size = new Size(110, 40);
            Short2Percent.TabIndex = 16;
            Short2Percent.Text = "Short 2%";
            Short2Percent.UseVisualStyleBackColor = false;
            Short2Percent.Click += Short2Percent_Click;
            // 
            // TakeProfit25Percent
            // 
            TakeProfit25Percent.BackColor = Color.FromArgb(255, 253, 174);
            TakeProfit25Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TakeProfit25Percent.Location = new Point(299, 535);
            TakeProfit25Percent.Name = "TakeProfit25Percent";
            TakeProfit25Percent.Size = new Size(133, 40);
            TakeProfit25Percent.TabIndex = 37;
            TakeProfit25Percent.Text = "Take Profit 25%";
            TakeProfit25Percent.UseVisualStyleBackColor = false;
            TakeProfit25Percent.Click += TakeProfit25Percent_Click;
            // 
            // TakeProfit33Percent
            // 
            TakeProfit33Percent.BackColor = Color.FromArgb(255, 255, 128);
            TakeProfit33Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TakeProfit33Percent.Location = new Point(156, 535);
            TakeProfit33Percent.Name = "TakeProfit33Percent";
            TakeProfit33Percent.Size = new Size(133, 40);
            TakeProfit33Percent.TabIndex = 36;
            TakeProfit33Percent.Text = "Take Profit 33%";
            TakeProfit33Percent.UseVisualStyleBackColor = false;
            TakeProfit33Percent.Click += TakeProfit33Percent_Click;
            // 
            // TakeProfit50Percent
            // 
            TakeProfit50Percent.BackColor = Color.Khaki;
            TakeProfit50Percent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TakeProfit50Percent.Location = new Point(11, 535);
            TakeProfit50Percent.Name = "TakeProfit50Percent";
            TakeProfit50Percent.Size = new Size(133, 40);
            TakeProfit50Percent.TabIndex = 35;
            TakeProfit50Percent.Text = "Take Profit 50%";
            TakeProfit50Percent.UseVisualStyleBackColor = false;
            TakeProfit50Percent.Click += TakeProfit50Percent_Click;
            // 
            // TakeProfitLimitPriceLabel
            // 
            TakeProfitLimitPriceLabel.AutoSize = true;
            TakeProfitLimitPriceLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TakeProfitLimitPriceLabel.Location = new Point(8, 501);
            TakeProfitLimitPriceLabel.Name = "TakeProfitLimitPriceLabel";
            TakeProfitLimitPriceLabel.Size = new Size(147, 21);
            TakeProfitLimitPriceLabel.TabIndex = 42;
            TakeProfitLimitPriceLabel.Text = "TakeProfitLimitPrice";
            // 
            // TakeProfitLimitPriceTextBox
            // 
            TakeProfitLimitPriceTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TakeProfitLimitPriceTextBox.Location = new Point(161, 498);
            TakeProfitLimitPriceTextBox.Name = "TakeProfitLimitPriceTextBox";
            TakeProfitLimitPriceTextBox.Size = new Size(100, 29);
            TakeProfitLimitPriceTextBox.TabIndex = 34;
            TakeProfitLimitPriceTextBox.TextChanged += TakeProfitLimitPriceTextBox_TextChanged;
            // 
            // TickerInput
            // 
            TickerInput.BorderStyle = BorderStyle.FixedSingle;
            TickerInput.CharacterCasing = CharacterCasing.Upper;
            TickerInput.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TickerInput.Location = new Point(128, 37);
            TickerInput.Name = "TickerInput";
            TickerInput.Size = new Size(100, 29);
            TickerInput.TabIndex = 1;
            // 
            // TickerLabel
            // 
            TickerLabel.AutoSize = true;
            TickerLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TickerLabel.Location = new Point(12, 37);
            TickerLabel.Name = "TickerLabel";
            TickerLabel.Size = new Size(51, 21);
            TickerLabel.TabIndex = 0;
            TickerLabel.Text = "Ticker";
            // 
            // GlobalOutputTextBox
            // 
            GlobalOutputTextBox.BackColor = SystemColors.Info;
            GlobalOutputTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            GlobalOutputTextBox.Location = new Point(12, 12);
            GlobalOutputTextBox.Multiline = true;
            GlobalOutputTextBox.Name = "GlobalOutputTextBox";
            GlobalOutputTextBox.ReadOnly = true;
            GlobalOutputTextBox.ScrollBars = ScrollBars.Vertical;
            GlobalOutputTextBox.Size = new Size(711, 88);
            GlobalOutputTextBox.TabIndex = 38;
            // 
            // TradePanelForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(731, 699);
            Controls.Add(GlobalOutputTextBox);
            Controls.Add(stock1GroupBox);
            Name = "TradePanelForm";
            Text = "IBKR Trade Panel";
            stock1GroupBox.ResumeLayout(false);
            stock1GroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox stock1GroupBox;

        private Label TickerLabel;
        private TextBox TickerInput;

        private Label BidAskLabel;
        private Label BidAskSizeLabel;
        private Label PercentageChangeLabel;
        private Label LastPriceLabel;
        public Label BidAskSizeOutput;
        public Label BidAskOutput;

        private Button BuyStop05Percent;
        private Button BuyStop1Percent;
        private Button BuyStop125Percent;
        private Button BuyStop15Percent;
        private Button BuyStop2Percent;
        private Button BuyStop25Percent;

        private Button SellStop05Percent;
        private Button SellStop1Percent;
        private Button SellStop125Percent;
        private Button SellStop15Percent;
        private Button SellStop2Percent;
        private Button Short125Percent;
        private Button Short15Percent;
        private Button Short2Percent;

        private Button TakeProfit25Percent;
        private Button TakeProfit33Percent;
        private Button TakeProfit50Percent;

        private Button Close25Percent;
        private Button Close33Percent;
        private Button Close50Percent;
        private Button Close67Percent;
        private Button Close100Percent;

        private Button ReverseButton;

        private TextBox TakeProfitLimitPriceTextBox;
        private Label TakeProfitLimitPriceLabel;

        private Button Long05Percent;
        private Button Long1Percent;
        private Button Long125percent;
        private Button Long15Percent;
        private Button Long2Percent;
        private Button Long25Percent;

        private Button Short05Percent;
        private Button Short1Percent;

        private TextBox BuyStopPriceTextBox;
        private Label BuyStopPriceLabel;
        private TextBox SellStopPriceTextBox;
        private Label SellStopPriceLabel;

        public Label PositionOutputLabel;
        private Label PositionLabel;
        private TextBox GlobalOutputTextBox;

        private Button ButtonCFD;
        private Button ButtonStock;
        public Label PercentageChangeOutputLabel;
        public Label LastPriceOutputLabel;
        private CheckBox OutsideRTHCheckbox;
    }
}