using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace DigitalOperation
{
    public partial class TestGame : Form
    {
        public TestGame()
        {
            InitializeComponent();
        }

        string Answer;
        bool SendAnswer = false;
        bool IsCorrect;
        bool ProgresbarStart = false;
        double addbal;
        int MAX_Speed;
        
        
        SoundPlayer sound_success = new SoundPlayer("./success.wav");
        SoundPlayer sound_fail = new SoundPlayer("./fail.wav");
        SoundPlayer sound_gameover =new SoundPlayer("./gameover.wav");


        Operation operation = Operation.Add;
        Diapazon diapazon = Diapazon.Low;
        TimeRegime timeRegime = TimeRegime.Regim30sec;
        private void SetOperationTag()
        {
            ButtonAdd.Tag = Operation.Add;
            ButtonDifference.Tag = Operation.Difference;
            ButtonMultiplication.Tag = Operation.Multiplication;
            ButtonDivision.Tag = Operation.Division;
        }
        private void SetDiapazonTag()
        {
            ButtonDiapazonLow.Tag = Diapazon.Low;
            ButtonDiapazonMedium.Tag = Diapazon.Medium;
            ButtonDiapazonHard.Tag = Diapazon.Hard;
        }
        private void SetTimeRegimTag()
        {
            ButtonTime30s.Tag = TimeRegime.Regim30sec;
            ButtonTime45s.Tag = TimeRegime.Regim45sec;
            ButtonTime60s.Tag = TimeRegime.Regim60sec;
        }


        private void OperationSelectForm()
        {
            switch (operation)
            {
                case Operation.Add: AddForm(); break;
                case Operation.Difference: DifferenceForm(); break;
                case Operation.Multiplication: MultiplicationForm(); break;
                case Operation.Division: DivisionForm(); break;
            }
        }
        private void SelectDiapazon()
        {
            switch (diapazon)
            {
                case Diapazon.Low: SettingFormForLowDiapazon(); addbal = 1; break;
                case Diapazon.Medium: SettingFormForMediumDiapazon(); addbal = 3; break;
                case Diapazon.Hard: SettingFormForHardDiapazon(); addbal = 8; break;
            }
        }
        private void SelectTimeRegime()
        {
            switch (timeRegime)
            {
                case TimeRegime.Regim30sec:
                    progressBar.Maximum = 30; progressBar.Value = 0; ShowStatusMessage("Режим 30 секунд"); MAX_Speed = 10; break;
                case TimeRegime.Regim45sec:
                    progressBar.Maximum = 45; progressBar.Value = 0; ShowStatusMessage("Режим 45 секунд"); MAX_Speed = 15; break;
                case TimeRegime.Regim60sec:
                    progressBar.Maximum = 60; progressBar.Value = 0; ShowStatusMessage("Режим 60 секунд"); MAX_Speed = 20; break;
            }
        }

      

        private void DivisionForm()
        {          
            SelectDiapazon();            
            labelOperationSelected.Text = ButtonDivision.Text;
        }

        private void MultiplicationForm()
        {
            SelectDiapazon();           
            labelOperationSelected.Text = ButtonMultiplication.Text;         
        }
        private void DifferenceForm()
        {
            SelectDiapazon();            
            labelOperationSelected.Text = ButtonDifference.Text;
        }
        private void AddForm()
        {
            SelectDiapazon();          
            labelOperationSelected.Text = ButtonAdd.Text;
        }
        private void SettingFormForHardDiapazon()
        {
            Random random_hard = new Random();
            Num1.Text = random_hard.Next(0, 1000).ToString();
            Num2.Text = random_hard.Next(0, 1000).ToString();
        }

        private void SettingFormForMediumDiapazon()
        {
            Random random_medium = new Random();
            Num1.Text = random_medium.Next(0, 100).ToString();
            Num2.Text = random_medium.Next(0, 100).ToString();
        }

        private void SettingFormForLowDiapazon()
        {
            Random randomlow = new Random();
            Num1.Text = randomlow.Next(0, 10).ToString();
            Num2.Text=randomlow.Next(0, 10).ToString();
        }
        private bool IsAnswerCorrect()
        {            
            try
            {
                double res = 0;
                double num1 = double.Parse(Num1.Text);
                double num2 = double.Parse(Num2.Text);
                double answer_num = double.Parse(Answer);
                switch (operation)
                {
                    case Operation.Add: res = num1 + num2; break;
                    case Operation.Difference: res = num1 - num2; break;
                    case Operation.Multiplication: res = num1 * num2; addbal *= 2; break;
                    case Operation.Division: res = num1 / num2; res = Math.Round(res,2); addbal *= 2.5; break;
                    
                }
                if (res == answer_num)
                {
                    return true;
                }
                else
                {
                    ShowStatusMessage("Неправильно! Спробуй ще раз ");
                    return false;
                }                        
            }
            catch
            {               
                AnswertextBox.Clear();
                OperationSelectForm();
                SelectDiapazon();
                SelectTimeRegime();
                ShowStatusMessage("Ви ввели не число");
                return false;
            }

        }

        private void TestGame_Load(object sender, EventArgs e)
        {
            SetOperationTag();
            SetDiapazonTag();
            SetTimeRegimTag();
            ShowStatusMessage("Натисни старт щоб розпочати");
        }

        private void ButtonSelectingOperation_CheckedChanged(object sender, EventArgs e)
        {
            operation = (Operation)(sender as Control).Tag;
        }

        private void ButtonSelectingDiapazon_CheckedChanged(object sender, EventArgs e)
        {
            diapazon = (Diapazon)(sender as Control).Tag;
        }
        private void ButtonSelectingTimeRegim_CheckedChanged(object sender, EventArgs e)
        {
            timeRegime = (TimeRegime)(sender as Control).Tag;
        }

        private void Startbutton_Click(object sender, EventArgs e)
        {
            OperationSelectForm();
            SelectDiapazon();
            SelectTimeRegime();
            ResetScore();
            UnlockActions();
            ShowStatusMessage("Удачі!");          
        }

        private void AnswertextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter&&SendAnswer==true)
            {
                Answer = AnswertextBox.Text.Trim();
                Answer = Answer.Replace(".", ",");
                
                IsCorrect = IsAnswerCorrect();
                double AmountScore = double.Parse(toolStripTextBoxScoreAmount.Text);
                int speed = int.Parse(toolStripLabelSpeed.Text);
                if (IsCorrect == true)
                {
                    AmountScore+=addbal;
                    progressBar.Increment(-5);
                    if (AmountScore >= speed * 10 && speed!=MAX_Speed)
                    {
                        speed++;
                        toolStripLabelSpeed.Text = speed.ToString();
                        progressBar.Maximum -= 3;                      
                    }
                    ShowStatusMessage($"Правильно! +{addbal}");
                    toolStripTextBoxScoreAmount.Text = AmountScore.ToString();
                    SuccessVoice();
                 
                }
                else if (IsCorrect == false)
                {
                    if (AmountScore > 0)
                    {
                        AmountScore -= 0.5;
                        toolStripTextBoxScoreAmount.Text = AmountScore.ToString();
                        FailVoice();
                    }                  
                }
                AnswertextBox.Clear();
                OperationSelectForm();
                SelectDiapazon();
            }
        }

        private void timerAnswer_Tick(object sender, EventArgs e)
        {
            if (ProgresbarStart == true)
            {
                progressBar.PerformStep();
                if (progressBar.Value == progressBar.Maximum)
                {
                    GameOver();
                }

            }          

        }
        private void timerShowTime_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabelTime.Text = DateTime.Now.ToLongTimeString();
        }
        private void ShowStatusMessage(string message)
        {
            toolStripStatusLabelMessage.Text = message;
        }
        private void SuccessVoice()
        {
            sound_success.Load();
            sound_success.Play();
        }
        private void FailVoice()
        {
            sound_fail.Load();
            sound_fail.Play();
        }
        private void GameOverVoice()
        {
            sound_gameover.Load();
            sound_gameover.Play();
        }
        private void GameOver()
        {
            progressBar.Value = 0;
            ProgresbarStart = false;
            double AmountScore = double.Parse(toolStripTextBoxScoreAmount.Text);
            GameOverVoice();
            MessageBox.Show($"Час вийшов, твій результат: {AmountScore}", "", MessageBoxButtons.OK);
            ShowStatusMessage($"Ваш результат був :{AmountScore}");
            ResetScore();
            LockActions();
        }
        private void LockActions()
        {           
            SendAnswer = false;
            AnswertextBox.ReadOnly = true;
            ProgresbarStart = false;
        }
        private void UnlockActions()
        {
            SendAnswer = true;
            AnswertextBox.ReadOnly = false;
            ProgresbarStart = true;
        }
        private void ResetScore()
        {
            toolStripTextBoxScoreAmount.Text = "0";
            toolStripLabelSpeed.Text = "1";
            progressBar.Value = 0;
            AnswertextBox.Clear();
            progressBar.Maximum = progressBar.Maximum;
        }

        private void ButtonOperation_Click(object sender, EventArgs e)
        {
            OperationSelectForm();
            SelectDiapazon();
            
        }

        private void ButtonDiapazon_Click(object sender, EventArgs e)
        {
            OperationSelectForm();
            SelectDiapazon();
            
        }

        private void ButtonTime_Click(object sender, EventArgs e)
        {
            OperationSelectForm();
            SelectDiapazon();
            SelectTimeRegime();
            ResetScore();
            LockActions();
        }     
    }
}
