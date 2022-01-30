using KAutoHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoLuckyUnicorn
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public struct Pet
        {
            public int id;
            public String element;
            public String priority;
            public String status;
            public bool flag_check_easy;
            public bool flag_check_medium;
        }
        public Pet pet;
        public enum State
        {
            Play,
            Refresh,
            Close,
            Idle,
            Easy,
            Check,
            CheckRefresh,
            CheckPet,
            Battle,
            Roll,
            Medium,
            Confirm,
            Recive,
            Back

        }
        State state = State.Play;

        public enum Element
        {
            None,
            Earth,
            Metal,
            Water,
            Wood,
            Fire,
            Darkness,
            Light
        }
        public Element element = Element.None;

        public bool flag_finish = false;
        public bool flag_battle_end = false;
        public bool flag_battle_start = false;
        public int count = 0;

        private System.Windows.Forms.Timer atimer;
        private int Counter = 240;

       /* private void btn_Auto_Click(object sender, EventArgs e)
        {
            if (btn_Auto.Text == "Auto")
            {
                btn_Auto.Text = "Off";
                state = State.Play;
                _Auto();

                _TimeCount();
            }
            else
                btn_Auto.Text = "Auto";
            
        }*/



        //-----------------Methods--------------------------------------------
        public void _TimeCount()
        {
            atimer = new System.Windows.Forms.Timer();
            atimer.Tick += new EventHandler(aTimer_Tick);
            atimer.Interval = 60000;
            atimer.Start();
            txt_Timer.Text = Counter.ToString();
        }

        private void aTimer_Tick(object sender, EventArgs e)
        {
            Counter --;
            txt_Timer.Text = Counter.ToString();
            if (Counter == 0)
            {
                atimer.Stop();
                if(ckb_OnMessage.Checked == true)
                {
                    DialogResult result = MessageBox.Show("Yes: Auto will run now \n No: Auto will delay 1 min", "Warning", MessageBoxButtons.YesNo);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            state = State.Play;
                            _ResetStatus();
                            _Auto();
                            break;
                        case DialogResult.No:
                            Counter = 1;
                            _TimeCount();
                            break;
                    }
                }
                else
                {
                    state = State.Play;
                    _ResetStatus();
                    _Auto();
                }

            }           
        }

        public void _ResetStatus()
        {
            status1.Text = "Yes";
            status2.Text = "Yes";
            status3.Text = "Yes";
            status4.Text = "Yes";
            status5.Text = "Yes";
            status6.Text = "Yes";
            status7.Text = "Yes";
            status8.Text = "Yes";
            status9.Text = "Yes";
            status10.Text = "Yes";
            status11.Text = "Yes";
            status12.Text = "Yes";
            status13.Text = "Yes";
            status14.Text = "Yes";
            status15.Text = "Yes";
        }

        public void _Auto()
        {
            while( state != State.Idle)
            {
                switch ((int)state)
                {
                    case (int)State.Play:
                        _SearchAndClick("Play.PNG", 1);
                        _TransState(State.Refresh);
                        flag_battle_start = false;
                        break;
                    case (int)State.Refresh:
                        _SearchAndClick("Refresh.PNG", 1);
                        _TransState(State.CheckRefresh);
                        pet.flag_check_medium = false;
                        pet.flag_check_easy = false;
                        break;
                    case (int)State.CheckRefresh:
                        _SearchAndClick("CheckRefresh.PNG", 1);
                        _TransState(State.Close);
                        break;
                    case (int)State.Close:
                        _SearchAndClick("Close.PNG", 1);
                        _TransState(State.Easy);
                        Thread.Sleep(5000);
                        break;
                    case (int)State.Easy:
                        //_SearchAndClick("Webup.PNG", 10);
                        _SearchAndClick("Easy.PNG", 1);
                        Thread.Sleep(1000);
                        if( flag_battle_start == true)
                        {
                            flag_battle_start = false;
                            //flag_finish = true;
                            _TransState(State.Battle);
                        }
                        else
                        {
                            pet.flag_check_easy = true;
                            _TransState(State.Check);
                        }
                        break;
                    case (int)State.Medium:
                        _SearchAndClick("Webdown.PNG", 6);
                        _SearchAndClick("Medium.PNG", 1);
                        Thread.Sleep(1000);
                        if (flag_battle_start == true)
                        {
                            flag_battle_start = false;
                            flag_finish = true;
                            _TransState(State.Battle);
                        }
                        else
                        {
                            pet.flag_check_medium = true;
                            _TransState(State.Check);
                        }
                        break;
                    case (int)State.Battle:
                        _Search("Close.PNG");
                        if (flag_finish == true)
                        {
                            _SearchAndClick("Battle.PNG", 1);
                            flag_battle_end = true;
                            _TransState(State.Confirm);
                        }

                        break;
                    case (int)State.Recive:
                        _SearchAndClick("OK.PNG", 1);
                        _TransState(State.Back);
                        break;
                    case (int)State.Back:
                        _SearchAndClick("Webup.PNG", 6);
                        var screen = CaptureHelper.CaptureScreen();
                        var subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Back.PNG");
                        var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                        if (resBitmap != null)
                        {
                            var x = resBitmap.Value.X;
                            var y = resBitmap.Value.Y;
                            AutoControl.MouseClick(x + 2, y + 2);
                            flag_finish = true;
                            count = count - 1;
                            if (count == 0)
                            {
                                _TransState(State.Idle);
                                _TimeCount();
                            }
                            else
                                _TransState(State.Play);
                        }
                        break;
                    case (int)State.Roll:
                        _SearchAndClick("Roll.PNG", 1);
                        _TransState(State.Confirm);
                        break;
                    case (int)State.Confirm:
                        //flag_finish = false;
                        screen = CaptureHelper.CaptureScreen();
                        subBitMap = ImageScanOpenCV.GetImage("Image\\" + "PageConfirm.PNG");
                        resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                        if (resBitmap != null)
                        {
                            _SearchAndClick("Webdown.PNG", 3);
                            flag_finish = false;
                        }
                        screen = CaptureHelper.CaptureScreen();
                        subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Confirm.PNG");
                        resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                        if (resBitmap != null)
                        {
                            var x = resBitmap.Value.X;
                            var y = resBitmap.Value.Y;
                            AutoControl.MouseClick(x + 2, y + 2);
                            flag_finish = true;
                        }
                        if (flag_finish == true)
                        {
                            if (flag_battle_end == true)
                            {
                                _TransState(State.Recive);
                                flag_battle_end = false;
                            }
                            else
                            {
                                _Search("History.PNG");
                                _TransState(State.Refresh);
                                flag_finish = true;
                            }
                        }
                        break;

                    case (int)State.Check:
                        _CheckElement();
                        _SearchAndClick("Close.PNG", 1);
                        _SearchAndClick("Webdown.PNG", 1);                        
                        if (pet.flag_check_easy == true)
                        {
                            pet.flag_check_easy = false;
                            if( element == Element.Wood)
                            {
                                _CheckStatus(1);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(1);
                                    status1.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                                    
                            }
                            if (element == Element.Water)
                            {
                                _CheckStatus(2);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(2);
                                    status2.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                                _CheckStatus(8);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(8);
                                    status2.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                                _CheckStatus(12);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(12);
                                    status2.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                                _CheckStatus(15);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(15);
                                    status2.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                                _CheckStatus(4);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(4);
                                    status4.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                                _CheckStatus(5);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(5);
                                    status4.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                            }
                            if (element == Element.Darkness)
                            {
                                _CheckStatus(6);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(6);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                                _CheckStatus(10);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(10);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                                _CheckStatus(3);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(3);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                                _CheckStatus(7);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(7);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                            }
                            if( element == Element.Metal)
                            {
                                _CheckStatus(9);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(9);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                            }
                            if( element == Element.Earth)
                            {
                                _CheckStatus(14);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(14);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                            }
                            if( element == Element.Light)
                            {
                                _CheckStatus(13);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(13);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Easy);
                                    break;
                                }
                            }
                            flag_finish = true;
                            _TransState(State.Medium);
                            break;

                        }
                        else if(pet.flag_check_medium == true)
                        {
                            pet.flag_check_medium = false;
                            if (element == Element.Wood)
                            {
                                _CheckStatus(1);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(1);
                                    status1.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Medium);
                                    break;
                                }

                            }
                            if (element == Element.Water)
                            {
                                _CheckStatus(4);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(4);
                                    status4.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Medium);
                                    break;
                                }
                                _CheckStatus(5);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(5);
                                    status4.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Medium);
                                    break;
                                }

                            }
                            if (element == Element.Darkness)
                            {
                                _CheckStatus(3);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(3);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Medium);
                                    break;
                                }
                                _CheckStatus(7);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(7);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Medium);
                                    break;
                                }
                                _CheckStatus(11);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(11);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Medium);
                                    break;
                                }
                            }
                            if( element == Element.Light)
                            {
                                _CheckStatus(13);
                                if (pet.status == "Yes")
                                {
                                    _SwapPet(13);
                                    status3.Text = "No";
                                    flag_finish = true;
                                    flag_battle_start = true;
                                    _TransState(State.Medium);
                                    break;
                                }
                            }
                            flag_finish = true;
                            _TransState(State.Roll);
                            break;
                        }
                        break;
                }
            }
                      
        }

        public void _Search(string ImgName)
        {
            var screen = CaptureHelper.CaptureScreen();
            // screen.Save("mainScreen.PNG");
            var subBitMap = ImageScanOpenCV.GetImage("Image\\" + ImgName);
            var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
            //resBitmap.Save("res.PNG");
            if (resBitmap != null)
            {
                flag_finish = true;
            }
            else flag_finish = false;
        }

        public void _SearchAndClick(string ImgName, int n)
        {
            var screen = CaptureHelper.CaptureScreen();
            // screen.Save("mainScreen.PNG");
            var subBitMap = ImageScanOpenCV.GetImage("Image\\" + ImgName);
            var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
            //resBitmap.Save("res.PNG");
            if (resBitmap != null)
            {
                var x = resBitmap.Value.X;
                var y = resBitmap.Value.Y;
                for(int i = 0; i < n; i++)
                {
                    AutoControl.MouseClick(x + 2, y + 2);
                    Thread.Sleep(200);
                }
                
                flag_finish = true;
            }
            else flag_finish = false;
        }

        public void _TransState(State lstate)
        {
            if (flag_finish == true)
                state = lstate;
            flag_finish = false;
        }

        public void _CheckStatus(int i)
        {
            switch (i)
            {
                case 1:
                    pet.status = status1.Text;
                    break;
                case 2:
                    pet.status = status2.Text;
                    break;
                case 3:
                    pet.status = status3.Text;
                    break;
                case 4:
                    pet.status = status4.Text;
                    break;
                case 5:
                    pet.status = status5.Text;
                    break;
                case 6:
                    pet.status = status6.Text;
                    break;
                case 7:
                    pet.status = status7.Text;
                    break;
                case 8:
                    pet.status = status8.Text;
                    break;
                case 9:
                    pet.status = status9.Text;
                    break;
                case 10:
                    pet.status = status10.Text;
                    break;
                case 11:
                    pet.status = status11.Text;
                    break;
                case 12:
                    pet.status = status12.Text;
                    break;
                case 13:
                    pet.status = status13.Text;
                    break;
                case 14:
                    pet.status = status14.Text;
                    break;
                case 15:
                    pet.status = status15.Text;
                    break;
            }
        }

        public void _SwapPet(int i)
        {
            do
            {
                switch(i)
                {
                    case 1:
                       // _Search("ID_Pet1.PNG");
                       // if (flag_finish == false)
                       // {
                            _SearchAndClick("SwapPetL.PNG", 1);
                         //   flag_finish = false;
                        //}                           
                        break;
                    case 2:
                       // _Search("ID_Pet2.PNG");
                       // if (flag_finish == false)
                       // {
                            _SearchAndClick("SwapPetR.PNG", 1);
                        //    flag_finish = false;
                       // }
                        break;
                    case 3:
                        
                       // _Search("ID_Pet3.PNG");
                        // if (flag_finish == false)
                        // {
                             _SearchAndClick("SwapPetR.PNG", 2);
 
                        // }
                        break;
                    case 4:
                       
                       // _Search("ID_Pet4.PNG");
                       // if (flag_finish == false)
                        // {
                            _SearchAndClick("SwapPetR.PNG", 3);
                         //   flag_finish = false;
                        // }
                        break;
                    case 5:
                        _SearchAndClick("SwapPetR.PNG", 4);
                        break;
                    case 6:
                        _SearchAndClick("SwapPetR.PNG", 5);
                        break;
                    case 7:
                        _SearchAndClick("SwapPetR.PNG", 6);
                        break;
                    case 8:
                        _SearchAndClick("SwapPetR.PNG", 7);
                        break;
                    case 9:
                        _SearchAndClick("SwapPetR.PNG", 8);
                        break;
                    case 10:
                        _SearchAndClick("SwapPetR.PNG", 9);
                        break;
                    case 11:
                        _SearchAndClick("SwapPetR.PNG", 10);
                        break;
                    case 12:
                        _SearchAndClick("SwapPetR.PNG", 11);
                        break;
                    case 13:
                        _SearchAndClick("SwapPetR.PNG", 12);
                        break;
                    case 14:
                        _SearchAndClick("SwapPetR.PNG", 13);
                        break;
                    case 15:
                        _SearchAndClick("SwapPetR.PNG", 14);
                        break;
                }
            }
            while (flag_finish == false);
        }

        public void _CheckElement()
        {
            element = Element.None;
            while (element == Element.None)
            {
                var screen = CaptureHelper.CaptureScreen();
                var subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Earth.PNG");
                var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                {
                    element = Element.Earth;
                    return;
                }
                subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Darkness.PNG");
                resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                {
                    element = Element.Darkness;
                    return;
                }
                subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Fire.PNG");
                resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                {
                    element = Element.Fire;
                    return;
                }
                subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Light.PNG");
                resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                {
                    element = Element.Light;
                    return;
                }
                subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Water.PNG");
                resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                {
                    element = Element.Water;
                    return;
                }
                subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Wood.PNG");
                resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                {
                    element = Element.Wood;
                    return;
                }
                subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Metal.PNG");
                resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                {
                    element = Element.Metal;
                    return;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //status2.Text = "No";
            //status1.Text = "No";
            //status4.Text = "No";
            count = 4;
            //_TimeCount();
        }

        private void btn_setTime_Click(object sender, EventArgs e)
        {
            Counter = Int32.Parse(txt_setTime.Text);
            _TimeCount();
        }
        //------------------Test------------------------------------
        /*
        public void _Auto()
        {
            //for(int i = 1; i < 4; i++)
           // {
           while (state != State.Idle)
                {
                    switch ((int)state)
                    {
                        case (int)State.Play:
                            _SearchAndClick("Play.PNG");
                            _TransState(State.Refresh);
                            break;
                        case (int)State.Refresh:
                            _SearchAndClick("Refresh.PNG");
                            _TransState(State.CheckRefresh);
                            break;
                        case (int)State.CheckRefresh:
                            _SearchAndClick("CheckRefresh.PNG");
                            _TransState(State.Close);
                            break;
                        case (int)State.Close:
                            _SearchAndClick("Close.PNG");
                            _TransState(State.Easy);
                            break;
                        case (int)State.CheckPet:
                        var screen = CaptureHelper.CaptureScreen();
                        var subBitMap = ImageScanOpenCV.GetImage("Image\\" + "SwapPet.PNG");
                        var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                        if (resBitmap != null)
                        {
                            if( element == Element.Wood)
                            {
                                if (pet.flag_check_easy == true)
                                {
                                    _CheckPet(1);
                                    pet.flag_check_easy = false;
                                }
                                else if (pet.flag_check_medium == true)
                                {

                                }
                                _TransState(State.Battle);
                            }
                            else
                            _TransState(State.Roll);
                        }
                        else
                        {

                            _SearchAndClick("Webdown.PNG");
                            
                        }
                        /*var screen = CaptureHelper.CaptureScreen();
                        var subBitMap = ImageScanOpenCV.GetImage("Image\\" + "SwapPet.PNG");
                        var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                        if (resBitmap != null)
                        {
                            _CheckPet(i);
                            flag_finish = true;
                            pet.flag_check_easy = false;
                            pet.flag_check_medium = false;
                            if (pet.priority == "Easy")
                                _TransState(State.Easy);
                            else if (pet.priority == "Medium")
                                _TransState(State.Medium);
                            else if (pet.priority == "OnlyEasy")
                            {
                                _TransState(State.Easy);
                                pet.flag_check_medium = true;
                            }
                        }
                        else
                        {
                            _SearchAndClick("Webdown.PNG");
                        }
                        break;
                        case (int)State.Easy:
                            //_SearchAndClick("Easy.PNG");
                            //pet.flag_check_easy = true;
                            //_TransState(State.Check);
                             screen = CaptureHelper.CaptureScreen();
                             subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Easy.PNG");
                             resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                            if (resBitmap == null)
                            {
                                _SearchAndClick("Webup.PNG");
                                _SearchAndClick("Webup.PNG");
                                _SearchAndClick("Webup.PNG");
                                _SearchAndClick("Webup.PNG");

                            }
                            else
                            {
                                var x = resBitmap.Value.X;
                                var y = resBitmap.Value.Y;
                                AutoControl.MouseClick(x + 2, y + 2);
                                flag_finish = true;
                                pet.flag_check_easy = true;
                                _TransState(State.Check);
                            }
                            break;
                        case (int)State.Medium:
                            //_SearchAndClick("Medium.PNG");
                            //_TransState(State.Check);
                            _SearchAndClick("Webdown.PNG");
                            _SearchAndClick("Webdown.PNG");
                            _SearchAndClick("Webdown.PNG");
                            _SearchAndClick("Webdown.PNG");
                            _SearchAndClick("Webdown.PNG");
                            _SearchAndClick("Webdown.PNG");
                            screen = CaptureHelper.CaptureScreen();
                            subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Medium.PNG");
                            resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                            if (resBitmap != null)
                            {
                                var x = resBitmap.Value.X;
                                var y = resBitmap.Value.Y;
                                AutoControl.MouseClick(x + 2, y + 2);
                                flag_finish = true;
                                pet.flag_check_medium = true;
                                _TransState(State.Check);
                            }
                            break;
                        case (int)State.Check:
                            _CheckElement();
                        _SearchAndClick("Close.PNG");
                        _TransState(State.CheckPet);
                        screen = CaptureHelper.CaptureScreen();
                            subBitMap = ImageScanOpenCV.GetImage("Image\\" + "SwapPet.PNG");
                            resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                            if(resBitmap != null)
                            {
                                
                            }
                            else
                            {
                                
                                _SearchAndClick("Webdown.PNG");
                                _SearchAndClick("Webdown.PNG");
                                _SearchAndClick("Webdown.PNG");
                                _SearchAndClick("Webdown.PNG");
                                _SearchAndClick("Webdown.PNG");
                                _SearchAndClick("Webdown.PNG");
                            }*/

        /*
            flag_finish = true;
            if (pet.element == "Metal")
            {
                if (element == Element.Wood)
                {
                    _TransState(State.Battle);
                    return;
                }
                else if ((pet.priority == "Easy") && (pet.flag_check_medium == false))
                    _TransState(State.Medium);
                else if ((pet.priority == "Medium") && (pet.flag_check_easy == false))
                    _TransState(State.Easy);
                else
                    _TransState(State.Roll);
            }
            else if (pet.element == "Wood")
            {
                if (element == Element.Earth)
                {
                    _TransState(State.Battle);
                    return;
                }
                else if ((pet.priority == "Easy") && (pet.flag_check_medium == false))
                    _TransState(State.Medium);
                else if ((pet.priority == "Medium") && (pet.flag_check_easy == false))
                    _TransState(State.Easy);
                else
                    _TransState(State.Roll);
            }
            else if (pet.element == "Earth")
            {
                if (element == Element.Water)
                {
                    _TransState(State.Battle);
                    return;
                }
                else if ((pet.priority == "Easy") && (pet.flag_check_medium == false))
                    _TransState(State.Medium);
                else if ((pet.priority == "Medium") && (pet.flag_check_easy == false))
                    _TransState(State.Easy);
                else
                    _TransState(State.Roll);
            }
            else if (pet.element == "Water")
            {
                if (element == Element.Fire)
                {
                    _TransState(State.Battle);
                    return;
                }
                else if ((pet.priority == "Easy") && (pet.flag_check_medium == false))
                    _TransState(State.Medium);
                else if ((pet.priority == "Medium") && (pet.flag_check_easy == false))
                    _TransState(State.Easy);
                else
                    _TransState(State.Roll);
            }
            else if (pet.element == "Fire")
            {
                if (element == Element.Metal)
                {
                    _TransState(State.Battle);
                    return;
                }
                else if ((pet.priority == "Easy") && (pet.flag_check_medium == false))
                    _TransState(State.Medium);
                else if ((pet.priority == "Medium") && (pet.flag_check_easy == false))
                    _TransState(State.Easy);
                else
                    _TransState(State.Roll);
            }
            else if (pet.element == "Darkness")
            {
                if (element == Element.Darkness)
                {
                    _TransState(State.Battle);
                    return;
                }
                else if ((pet.priority == "Easy") && (pet.flag_check_medium == false))
                    _TransState(State.Medium);
                else if ((pet.priority == "Medium") && (pet.flag_check_easy == false))
                    _TransState(State.Easy);
                else
                    _TransState(State.Roll);
            }
            else if (pet.element == "Light")
            {
                if (element == Element.Light)
                {
                    _TransState(State.Battle);
                    return;
                }
                else if ((pet.priority == "Easy") && (pet.flag_check_medium == false))
                    _TransState(State.Medium);
                else if ((pet.priority == "Medium") && (pet.flag_check_easy == false))
                    _TransState(State.Easy);
                else
                    _TransState(State.Roll);
            }
            _SearchAndClick("Close.PNG");

            break;
        case (int)State.Battle:
            screen = CaptureHelper.CaptureScreen();
            subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Close.PNG");
            resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
            if (resBitmap != null)
            {
                screen = CaptureHelper.CaptureScreen();
                subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Battle.PNG");
                resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                {
                    var x = resBitmap.Value.X;
                    var y = resBitmap.Value.Y;
                    AutoControl.MouseClick(x + 2, y + 2);
                    flag_finish = true;
                    _TransState(State.Confirm);
                    flag_battle = true;

                }
                flag_finish = true;
                _TransState(State.Idle);
            }

            break;
        case (int)State.Recive:
            _SearchAndClick("OK.PNG");
            _TransState(State.Back);
            break;
        case (int)State.Back:
            _SearchAndClick("Webup.PNG");
            _SearchAndClick("Webup.PNG");
            _SearchAndClick("Webup.PNG");
            _SearchAndClick("Webup.PNG");
            _SearchAndClick("Webup.PNG");
            _SearchAndClick("Webup.PNG");
            screen = CaptureHelper.CaptureScreen();
            subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Back.PNG");
            resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
            if (resBitmap != null)
            {
                var x = resBitmap.Value.X;
                var y = resBitmap.Value.Y;
                AutoControl.MouseClick(x + 2, y + 2);
                flag_finish = true;
                pet.flag_check_medium = true;
                _TransState(State.Idle);
            }
            break;
        case (int)State.Roll:
            _SearchAndClick("Roll.PNG");
            _TransState(State.Confirm);
            break;
        case (int)State.Confirm:
            screen = CaptureHelper.CaptureScreen();
            subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Confirm.PNG");
            resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
            if (resBitmap == null)
            {
                screen = CaptureHelper.CaptureScreen();
                subBitMap = ImageScanOpenCV.GetImage("Image\\" + "PageConfirm.PNG");
                resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                {
                    _SearchAndClick("Webdown.PNG");
                    _SearchAndClick("Webdown.PNG");
                    _SearchAndClick("Webdown.PNG");

                }
            }
            else
            {
                var x = resBitmap.Value.X;
                var y = resBitmap.Value.Y;
                AutoControl.MouseClick(x + 2, y + 2);
            }
            if (flag_battle == true)
            {
                _TransState(State.Recive);
                flag_battle = false;
            }
            else
            {
                screen = CaptureHelper.CaptureScreen();
                subBitMap = ImageScanOpenCV.GetImage("Image\\" + "History.PNG");
                resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
                if (resBitmap != null)
                    _TransState(State.CheckPet);
            }
            break;
    }

}
}

}

public void _SearchAndClick(string ImgName)
{
var screen = CaptureHelper.CaptureScreen();
// screen.Save("mainScreen.PNG");
var subBitMap = ImageScanOpenCV.GetImage("Image\\" + ImgName);
var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
//resBitmap.Save("res.PNG");
if (resBitmap != null)
{
var x = resBitmap.Value.X;
var y = resBitmap.Value.Y;
AutoControl.MouseClick(x + 2, y + 2);
flag_finish = true;
}
else flag_finish = false;

}

public void _TransState(State lstate)
{
if (flag_finish == true)
state = lstate;
}

public void _CheckPet(int i)
{
do
{
flag_finish = false;
switch (i)
{
    case 1:
        var screen = CaptureHelper.CaptureScreen();
        var subBitMap = ImageScanOpenCV.GetImage("Image\\" + "ID_Pet1.PNG");
        var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
        if (resBitmap != null)
        {
            pet.id = Int32.Parse(id_1.Text);
            pet.element = txt_element1.Text;
            pet.priority = txt_priority1.Text;
            pet.status = status1.Text;
            return;
        }
        else
        {
            screen = CaptureHelper.CaptureScreen();
            subBitMap = ImageScanOpenCV.GetImage("Image\\" + "SwapPet.PNG");
            resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
            if (resBitmap != null)
            {
                var x = resBitmap.Value.X;
                var y = resBitmap.Value.Y;
                AutoControl.MouseClick(x + 2, y + 2);
            }

        }
        break;
    case 2:
        screen = CaptureHelper.CaptureScreen();
        subBitMap = ImageScanOpenCV.GetImage("Image\\" + "ID_Pet2.PNG");
        resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
        if (resBitmap != null)
        {
            pet.id = Int32.Parse(id_2.Text);
            pet.element = txt_element2.Text;
            pet.priority = txt_priority2.Text;
            pet.status = status2.Text;
            return;
        }
        else
        {
            screen = CaptureHelper.CaptureScreen();
            subBitMap = ImageScanOpenCV.GetImage("Image\\" + "SwapPet.PNG");
            resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
            if (resBitmap != null)
            {
                var x = resBitmap.Value.X;
                var y = resBitmap.Value.Y;
                AutoControl.MouseClick(x + 2, y + 2);
            }

        }
        break;
    case 3:
        screen = CaptureHelper.CaptureScreen();
        subBitMap = ImageScanOpenCV.GetImage("Image\\" + "ID_Pet3.PNG");
        resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
        if (resBitmap != null)
        {
            pet.id = Int32.Parse(id_3.Text);
            pet.element = txt_element3.Text;
            pet.priority = txt_priority3.Text;
            pet.status = status3.Text;
            return;
        }
        else
        {
            screen = CaptureHelper.CaptureScreen();
            subBitMap = ImageScanOpenCV.GetImage("Image\\" + "SwapPet.PNG");
            resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
            if (resBitmap != null)
            {
                var x = resBitmap.Value.X;
                var y = resBitmap.Value.Y;
                AutoControl.MouseClick(x + 2, y + 2);
            }

        }
        break;
    case 4:
        screen = CaptureHelper.CaptureScreen();
        subBitMap = ImageScanOpenCV.GetImage("Image\\" + "ID_Pet4.PNG");
        resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
        if (resBitmap != null)
        {
            pet.id = Int32.Parse(id_4.Text);
            pet.element = txt_element4.Text;
            pet.priority = txt_priority4.Text;
            pet.status = status4.Text;
            return;
        }
        else
        {
            screen = CaptureHelper.CaptureScreen();
            subBitMap = ImageScanOpenCV.GetImage("Image\\" + "SwapPet.PNG");
            resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
            if (resBitmap != null)
            {
                var x = resBitmap.Value.X;
                var y = resBitmap.Value.Y;
                AutoControl.MouseClick(x + 2, y + 2);
            }

        }
        break;
}
}
while (flag_finish == false);
}



public void _CheckElement()
{
element = Element.None;
while (element == Element.None)
{
var screen = CaptureHelper.CaptureScreen();
var subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Earth.PNG");
var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
if (resBitmap != null)
{
    element = Element.Earth;
    return;
}
subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Darkness.PNG");
resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
if (resBitmap != null)
{
    element = Element.Darkness;
    return;
}
subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Fire.PNG");
resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
if (resBitmap != null)
{
    element = Element.Fire;
    return;
}
subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Light.PNG");
resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
if (resBitmap != null)
{
    element = Element.Light;
    return;
}
subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Water.PNG");
resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
if (resBitmap != null)
{
    element = Element.Water;
    return;
}
subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Wood.PNG");
resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
if (resBitmap != null)
{
    element = Element.Wood;
    return;
}
subBitMap = ImageScanOpenCV.GetImage("Image\\" + "Metal.PNG");
resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitMap);
if (resBitmap != null)
{
    element = Element.Metal;
    return;
}
}
}*/
    }
}
